using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Iface.Oik.Tm.Helpers;
using Iface.Oik.Tm.Interfaces;
using Iface.Oik.Tm.Services;
using Jint;

namespace Iface.Oik.ScriptEngine
{
  public class App : BackgroundService
  {
    private readonly ICommonInfrastructure _infr;
    private readonly IOikDataApi           _api;
    private readonly Engine                _engine;
    private readonly string                _script;


    public App(ICommonInfrastructure infr,
               IOikDataApi           api)
    {
      _infr   = infr;
      _api    = api;
      _script = File.ReadAllText("ПС Демо.js");
      
      _engine = new Engine();
      _engine.SetValue("GetTmStatus", new Func<int, int, int, Task<int>>(GetTmStatus))
             .SetValue("GetTmAnalog", new Func<int, int, int, Task<float>>(GetTmAnalog))
             .SetValue("SetTmStatus", new Func<int, int, int, int, Task>(SetTmStatus))
             .SetValue("SetTmAnalog", new Func<int, int, int, float, Task>(SetTmAnalog));
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      while (!stoppingToken.IsCancellationRequested)
      {
        await DoSomething();
        await Task.Delay(10000, stoppingToken);
      }
    }


    private async Task DoSomething()
    {
      Tms.PrintDebug(_infr.TmUserInfo?.Name);
      Tms.PrintDebug(await _api.GetSystemTimeString());

      var ts = new TmStatus(20, 1, 1);
      var ti = new TmAnalog(20, 1, 1);

      await _api.UpdateTagPropertiesAndClassData(ts);
      await _api.UpdateStatus(ts);

      await _api.UpdateTagPropertiesAndClassData(ti);
      await _api.UpdateAnalog(ti);

      Tms.PrintDebug(ts);
      Tms.PrintDebug(ti);

      try
      {
        _engine.Execute(_script);
      }
      catch (Exception ex)
      {
        Tms.PrintDebug("Ошибка выполнения скрипта: " + ex.Message);
      }
    }


    private async Task<int> GetTmStatus(int ch, int rtu, int point)
    {
      return await _api.GetStatus(ch, rtu, point);
    }


    private async Task<float> GetTmAnalog(int ch, int rtu, int point)
    {
      return await _api.GetAnalog(ch, rtu, point);
    }


    private async Task SetTmStatus(int ch, int rtu, int point, int status)
    {
      await _api.SetStatus(ch, rtu, point, status);
      Console.WriteLine($"#TC{ch}:{rtu}:{point} <- {status}");
    }


    private async Task SetTmAnalog(int ch, int rtu, int point, float value)
    {
      await _api.SetAnalog(ch, rtu, point, value);
      Console.WriteLine($"#TT{ch}:{rtu}:{point} <- {value}");
    }
  }
}