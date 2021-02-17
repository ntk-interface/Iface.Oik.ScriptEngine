using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Iface.Oik.Tm.Helpers;
using Iface.Oik.Tm.Interfaces;
using Microsoft.Extensions.Hosting;

namespace Iface.Oik.ScriptEngine
{
  public abstract class Worker : BackgroundService
  {
    private readonly IOikDataApi _api;

    private readonly string _name;

    private int  _scriptTimeout = 2000;
    private bool _isScriptTimeoutOverriden;


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


    public void OverrideScriptTimeout(int timeout)
    {
      if (_isScriptTimeoutOverriden)
      {
        return;
      }
      _scriptTimeout            = timeout;
      _isScriptTimeoutOverriden = true;
    }


    public int GetTmStatus(int ch, int rtu, int point)
    {
      return _api.GetStatus(ch, rtu, point).GetAwaiter().GetResult();
    }


    public int GetTmStatus(TmAddr addr)
    {
      return GetTmStatus(addr.Ch, addr.Rtu, addr.Point);
    }


    public float GetTmAnalog(int ch, int rtu, int point)
    {
      return _api.GetAnalog(ch, rtu, point).GetAwaiter().GetResult();
    }


    public float GetTmAnalog(TmAddr addr)
    {
      return GetTmAnalog(addr.Ch, addr.Rtu, addr.Point);
    }


    public void SetTmStatus(int ch, int rtu, int point, int status)
    {
      _api.SetStatus(ch, rtu, point, status).Wait();
    }


    public void SetTmStatus(TmAddr addr, int status)
    {
      SetTmStatus(addr.Ch, addr.Rtu, addr.Point, status);
    }


    public void SetTmAnalog(int ch, int rtu, int point, float value)
    {
      _api.SetAnalog(ch, rtu, point, value).Wait();
    }


    public void SetTmAnalog(TmAddr addr, float value)
    {
      SetTmAnalog(addr.Ch, addr.Rtu, addr.Point, value);
    }


    public void SetTmStatusFlags(int ch, int rtu, int point, TmFlags flags)
    {
      _api.SetTagFlags(new TmStatus(ch, rtu, point), flags);
    }


    public void SetTmStatusFlags(TmAddr addr, TmFlags flags)
    {
      SetTmStatusFlags(addr.Ch, addr.Rtu, addr.Point, flags);
    }


    public void SetTmAnalogFlags(int ch, int rtu, int point, TmFlags flags)
    {
      _api.SetTagFlags(new TmAnalog(ch, rtu, point), flags);
    }


    public void SetTmAnalogFlags(TmAddr addr, TmFlags flags)
    {
      SetTmAnalogFlags(addr.Ch, addr.Rtu, addr.Point, flags);
    }


    public string GetExpressionResult(string expression)
    {
      return _api.GetExpressionResult(expression).GetAwaiter().GetResult();
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