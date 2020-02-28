using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Iface.Oik.Tm.Helpers;
using Iface.Oik.Tm.Interfaces;
using Microsoft.Extensions.Hosting;

namespace Iface.Oik.ScriptEngine.Engines
{
  public abstract class AbstractEngine : BackgroundService
  {
    private readonly   string _name;
    protected readonly string _script;
    private            int    _scriptTimeout = 2000;
    private            bool   _isScriptTimeoutOverriden;

    private IOikDataApi _api;


    protected AbstractEngine(string name,
                             string script)
    {
      _name   = name;
      _script = script;
    }


    public void InitApi(IOikDataApi api)
    {
      _api = api;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      while (!stoppingToken.IsCancellationRequested)
      {
        try
        {
          var sw = Stopwatch.StartNew();
          ExecuteScript();
          Tms.PrintDebug($"Скрипт \"{_name}\" рассчитан за {sw.ElapsedMilliseconds} мс");
        }
        catch (Exception ex)
        {
          Tms.PrintDebug($"Ошибка при расчете скрипта \"{_name}\": {ex.Message}");
        }
        await Task.Delay(_scriptTimeout, stoppingToken);
      }
    }


    protected void OverrideScriptTimeout(int timeout)
    {
      if (_isScriptTimeoutOverriden) return;

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
      // Tms.PrintDebug($"#TC{ch}:{rtu}:{point} <- {status}");
    }


    protected void SetTmAnalog(int ch, int rtu, int point, float value)
    {
      _api.SetAnalog(ch, rtu, point, value).Wait();
      // Tms.PrintDebug($"#TT{ch}:{rtu}:{point} <- {value}");
    }


    public abstract    void InitEngine();
    protected abstract void ExecuteScript();
  }
}