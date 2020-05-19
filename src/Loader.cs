using System.IO;
using System.Reflection;
using Iface.Oik.ScriptEngine.Workers;
using Iface.Oik.Tm.Helpers;
using Iface.Oik.Tm.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Iface.Oik.ScriptEngine
{
  public static class Loader
  {
    private static readonly string ScriptsPath = Path.Combine(
      Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
      "..",
      "scripts");


    public static bool AddWorkers(this IServiceCollection services)
    {
      if (!Directory.Exists(ScriptsPath))
      {
        Tms.PrintError("Не найден каталог с файлами скриптов");
        return false;
      }

      var workersCount = 0;
      foreach (var file in Directory.GetFiles(ScriptsPath, "*"))
      {
        var name   = Path.GetFileName(file);
        var script = File.ReadAllText(file);
        switch (Path.GetExtension(file))
        {
          case ".js":
            services.AddSingleton<IHostedService>(provider => new JavascriptWorker(provider.GetService<IOikDataApi>(),
                                                                                   name,
                                                                                   script));
            workersCount++;
            break;

          case ".py":
            services.AddSingleton<IHostedService>(provider => new PythonWorker(provider.GetService<IOikDataApi>(),
                                                                               name,
                                                                               script));
            workersCount++;
            break;
        }
      }
      
      if (workersCount == 0)
      {
        Tms.PrintError("Не найдено ни одного скрипта");
        return false;
      }

      Tms.PrintMessage($"Всего скриптов: {workersCount}");
      return true;
    }
  }
}