using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Iface.Oik.ScriptEngine.Engines;
using Iface.Oik.Tm.Interfaces;
using Iface.Oik.Tm.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Iface.Oik.ScriptEngine
{
  public class EngineStartup : IHostedService
  {
    public static readonly string ScriptsPath = Path.Combine(
      Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
      "scripts");


    private readonly IServiceProvider _serviceProvider;


    public EngineStartup(IServiceProvider serviceProvider)
    {
      _serviceProvider = serviceProvider;
    }


    public Task StartAsync(CancellationToken cancellationToken)
    {
      var oikDataApi = _serviceProvider.GetService<IOikDataApi>();
      
      _serviceProvider.GetServices<AbstractEngine>()
                      .ForEach(engine => engine.InitApi(oikDataApi));
      
      return Task.CompletedTask;
    }


    public Task StopAsync(CancellationToken cancellationToken)
    {
      return Task.CompletedTask;
    }
  }


  public static class ServicesExtensions
  {
    public static void AddEngines(this IServiceCollection services)
    {
      if (!Directory.Exists(EngineStartup.ScriptsPath)) return;

      foreach (var file in Directory.GetFiles(EngineStartup.ScriptsPath, "*"))
      {
        var            name   = Path.GetFileName(file);
        var            script = File.ReadAllText(file);
        AbstractEngine engine;
        switch (Path.GetExtension(file))
        {
          case ".js":
            engine = new JavascriptEngine(name, script);
            break;

          case ".py":
            engine = new PythonEngine(name, script);
            break;

          default:
            continue;
        }
        engine.InitEngine();

        services.AddSingleton<AbstractEngine>(engine);
        services.AddSingleton<IHostedService>(engine);
      }
    }
  }
}