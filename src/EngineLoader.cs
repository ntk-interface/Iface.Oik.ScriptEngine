using System.IO;
using System.Reflection;
using Iface.Oik.ScriptEngine.Engines;
using Iface.Oik.Tm.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Iface.Oik.ScriptEngine
{
  public static class EngineLoader
  {
    private static readonly string ScriptsPath = Path.Combine(
      Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
      "scripts");


    public static void AddEngines(this IServiceCollection services)
    {
      if (!Directory.Exists(ScriptsPath)) return;

      foreach (var file in Directory.GetFiles(ScriptsPath, "*"))
      {
        var name   = Path.GetFileName(file);
        var script = File.ReadAllText(file);
        switch (Path.GetExtension(file))
        {
          case ".js":
            services.AddSingleton<IHostedService>(provider => new JavascriptEngine(provider.GetService<IOikDataApi>(),
                                                                                   name,
                                                                                   script));
            break;

          case ".py":
            services.AddSingleton<IHostedService>(provider => new PythonEngine(provider.GetService<IOikDataApi>(),
                                                                               name,
                                                                               script));
            break;
        }
      }
    }
  }
}