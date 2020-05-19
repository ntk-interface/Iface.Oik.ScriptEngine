using System;
using System.Diagnostics;
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

    private readonly   string _name;
    protected readonly string Script;

    private int  _scriptTimeout = 2000;
    private bool _isScriptTimeoutOverriden;


    protected Worker(IOikDataApi api,
                     string      name,
                     string      script)
    {
      _api = api;

      _name  = name;
      Script = script;
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


    protected abstract void DoWork();


    protected void OverrideScriptTimeout(int timeout)
    {
      if (_isScriptTimeoutOverriden)
      {
        return;
      }
      _scriptTimeout            = timeout;
      _isScriptTimeoutOverriden = true;
    }


    protected int GetTmStatus(int ch, int rtu, int point)
    {
      return _api.GetStatus(ch, rtu, point).GetAwaiter().GetResult();
    }


    protected float GetTmAnalog(int ch, int rtu, int point)
    {
      return _api.GetAnalog(ch, rtu, point).GetAwaiter().GetResult();
    }


    protected void SetTmStatus(int ch, int rtu, int point, int status)
    {
      _api.SetStatus(ch, rtu, point, status).Wait();
    }


    protected void SetTmAnalog(int ch, int rtu, int point, float value)
    {
      _api.SetAnalog(ch, rtu, point, value).Wait();
    }
  }
}