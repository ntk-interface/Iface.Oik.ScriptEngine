using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iface.Oik.Tm.Helpers;
using Iface.Oik.Tm.Interfaces;
using Iface.Oik.Tm.Utils;
using Microsoft.Extensions.Hosting;

namespace Iface.Oik.ScriptEngine
{
  public abstract class Worker : BackgroundService
  {
    private readonly IOikDataApi _api;

    private readonly string _name;

    private int  _scriptTimeout = 2000;
    private bool _isScriptTimeoutOverriden;
    private bool _isTelecontrolAllowed;


    protected abstract void DoWork();


    protected Worker(IOikDataApi api, string filename)
    {
      _api  = api;
      _name = Path.GetFileName(filename);
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      await Task.Delay(500, stoppingToken); // такое асинхронное ожидание даёт хосту возможность завершить инициализацию

      while (!stoppingToken.IsCancellationRequested)
      {
        try
        {
          var sw = Stopwatch.StartNew();
          DoWork();
          Tms.PrintDebug($"Скрипт \"{_name}\" рассчитан за {sw.ElapsedMilliseconds} мс");
        }
        catch (Exception ex)
        {
          Tms.PrintDebug($"Ошибка при расчете скрипта \"{_name}\": {ex.Message}");
        }
        await Task.Delay(_scriptTimeout, stoppingToken);
      }
    }


    public void AllowTelecontrol()
    {
      if (_isTelecontrolAllowed)
      {
        return;
      }
      _isTelecontrolAllowed = true;
      LogDebug("Команды ТУ разрешены");
    }


    public void OverrideScriptTimeout(int timeout)
    {
      if (_isScriptTimeoutOverriden)
      {
        return;
      }
      _scriptTimeout            = timeout;
      _isScriptTimeoutOverriden = true;
    }


    public void Telecontrol(int ch, int rtu, int point, int explicitNewStatus)
    {
      var tmStatus = new TmStatus(ch, rtu, point);

      if (!_isTelecontrolAllowed)
      {
        Tms.PrintDebug($"Не подана команда ТУ на {tmStatus.TmAddr} - в скрипте не разрешены команды");
        return;
      }
      var result = _api.TelecontrolExplicitly(tmStatus, explicitNewStatus).GetAwaiter().GetResult();
      if (result == TmTelecontrolResult.Success)
      {
        Tms.PrintDebug($"Выполнена команда ТУ на {tmStatus.TmAddr}");
      }
      else
      {
        Tms.PrintDebug($"Ошибка команды ТУ на {tmStatus.TmAddr} - {result.GetDescription()}");
      }
    }


    public bool IsTmStatusOn(int ch, int rtu, int point)
    {
      return GetTmStatus(ch, rtu, point) > 0;
    }


    protected bool IsTmStatusFlagRaised(int ch, int rtu, int point, TmFlags flag)
    {
      var tmStatus = new TmStatus(ch, rtu, point);
      _api.UpdateStatus(tmStatus).Wait();
      return tmStatus.HasFlag(flag);
    }


    public int GetTmStatus(int ch, int rtu, int point)
    {
      return _api.GetStatus(ch, rtu, point).GetAwaiter().GetResult();
    }


    public int GetTmStatus(TmAddr addr)
    {
      return GetTmStatus(addr.Ch, addr.Rtu, addr.Point);
    }


    public int GetTmStatusFromRetro(int ch, int rtu, int point, long timestamp)
    {
      var time = DateUtil.GetDateTimeFromTimestamp(timestamp);

      return _api.GetStatusFromRetro(ch, rtu, point, time).GetAwaiter().GetResult();
    }


    public float GetTmAnalog(int ch, int rtu, int point)
    {
      return _api.GetAnalog(ch, rtu, point).GetAwaiter().GetResult();
    }


    public float GetTmAnalog(TmAddr addr)
    {
      return GetTmAnalog(addr.Ch, addr.Rtu, addr.Point);
    }


    public float GetTmAnalogFromRetro(int ch, int rtu, int point, long timestamp, int? retroNum)
    {
      var time = DateUtil.GetDateTimeFromTimestamp(timestamp);

      return _api.GetAnalogFromRetro(ch, rtu, point, time, retroNum ?? 0).GetAwaiter().GetResult();
    }


    public float[] GetTmAnalogRetro(int  ch,
                                    int  rtu,
                                    int  point,
                                    long startTimestamp,
                                    long endTimestamp,
                                    int  step,
                                    int? retroNum)
    {
      var retro = _api.GetAnalogRetro(new TmAnalog(ch, rtu, point),
                                      new TmAnalogRetroFilter(startTimestamp, endTimestamp, step),
                                      retroNum ?? 0)
                      .GetAwaiter().GetResult();

      return retro?.Select(ms => ms.Value).ToArray()
             ?? Array.Empty<float>();
    }


    public float[] GetTmAnalogImpulseArchiveAverage(int  ch,
                                                    int  rtu,
                                                    int  point,
                                                    long startTimestamp,
                                                    long endTimestamp,
                                                    int  step)
    {
      var impulseArchive = _api.GetImpulseArchiveAverage(new TmAnalog(ch, rtu, point),
                                                         new TmAnalogRetroFilter(startTimestamp, endTimestamp, step))
                               .GetAwaiter().GetResult();

      return impulseArchive?.Select(ms => ms.Value).ToArray()
             ?? Array.Empty<float>();
    }


    public float[] GetTmAnalogMicroSeries(int ch, int rtu, int point)
    {
      var microSeries = _api.GetAnalogsMicroSeries(new[] { new TmAnalog(ch, rtu, point) }).GetAwaiter().GetResult();

      return microSeries?.FirstOrDefault()?.Select(ms => ms.Value).ToArray()
             ?? Array.Empty<float>();
    }


    protected bool IsTmAnalogFlagRaised(int ch, int rtu, int point, TmFlags flag)
    {
      var tmAnalog = new TmAnalog(ch, rtu, point);
      _api.UpdateAnalog(tmAnalog).Wait();
      return tmAnalog.HasFlag(flag);
    }


    public void SetTmStatus(int ch, int rtu, int point, int status)
    {
      _api.SetStatus(ch, rtu, point, status).Wait();
    }


    public void SetTmStatus(TmAddr addr, int status)
    {
      SetTmStatus(addr.Ch, addr.Rtu, addr.Point, status);
    }


    public void RaiseTmStatusFlag(int ch, int rtu, int point, TmFlags flags)
    {
      _api.SetTagFlagsExplicitly(new TmStatus(ch, rtu, point), flags);
    }


    public void RaiseTmStatusFlag(TmAddr addr, TmFlags flags)
    {
      RaiseTmStatusFlag(addr.Ch, addr.Rtu, addr.Point, flags);
    }


    public void ClearTmStatusFlag(int ch, int rtu, int point, TmFlags flags)
    {
      _api.ClearTagFlagsExplicitly(new TmStatus(ch, rtu, point), flags);
    }


    public void ClearTmStatusFlag(TmAddr addr, TmFlags flags)
    {
      ClearTmStatusFlag(addr.Ch, addr.Rtu, addr.Point, flags);
    }


    public void SetTmAnalog(int ch, int rtu, int point, float value)
    {
      _api.SetAnalog(ch, rtu, point, value).Wait();
    }


    public void SetTmAnalog(TmAddr addr, float value)
    {
      SetTmAnalog(addr.Ch, addr.Rtu, addr.Point, value);
    }


    public void RaiseTmAnalogFlag(int ch, int rtu, int point, TmFlags flags)
    {
      _api.SetTagFlagsExplicitly(new TmAnalog(ch, rtu, point), flags);
    }


    public void RaiseTmAnalogFlag(TmAddr addr, TmFlags flags)
    {
      RaiseTmAnalogFlag(addr.Ch, addr.Rtu, addr.Point, flags);
    }


    public void ClearTmAnalogFlag(int ch, int rtu, int point, TmFlags flags)
    {
      _api.ClearTagFlagsExplicitly(new TmAnalog(ch, rtu, point), flags);
    }


    public void ClearTmAnalogFlag(TmAddr addr, TmFlags flags)
    {
      ClearTmAnalogFlag(addr.Ch, addr.Rtu, addr.Point, flags);
    }


    public string GetExpressionResult(string expression)
    {
      return _api.GetExpressionResult(expression).GetAwaiter().GetResult();
    }


    public float GetExpressionResultFloat(string expression)
    {
      if (!TryGetExpressionResult(expression, out var value))
      {
        throw new Exception("Ошибка функции TM");
      }
      return value;
    }


    public bool TryGetExpressionResult(string expression, out float value)
    {
      var expressionResult = GetExpressionResult(expression);
      if (!float.TryParse(expressionResult, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
      {
        LogDebug(expressionResult);
        return false;
      }
      return true;
    }


    public void LogDebug(string message)
    {
      Tms.PrintDebug($"Отладочное сообщение скрипта \"{_name}\": {message}");
    }
  }
}