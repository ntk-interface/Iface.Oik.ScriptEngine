using System;
using System.IO;
using Iface.Oik.ScriptEngine.Workers;
using Iface.Oik.Tm.Helpers;
using Iface.Oik.Tm.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Iface.Oik.ScriptEngine
{
  public static class Loader
  {
    private static readonly string ScriptsPath = Path.Combine(AppContext.BaseDirectory, "scripts");


    public static bool AddWorkers(this IServiceCollection services)
    {
      if (!Directory.Exists(ScriptsPath))
      {
        Tms.PrintError("Не найден каталог с файлами скриптов");
        return false;
      }

      var workersCount = 0;
      foreach (var filename in Directory.GetFiles(ScriptsPath, "*"))
      {
        switch (Path.GetExtension(filename))
        {
          case ".js":
            services.AddSingleton<IHostedService>(provider => new JavascriptWorker(provider.GetService<IOikDataApi>(),
                                                                                   filename));
            workersCount++;
            break;

          case ".py":
            services.AddSingleton<IHostedService>(provider => new PythonWorker(provider.GetService<IOikDataApi>(),
                                                                               filename));
            workersCount++;
            break;

          case ".xlsx":
            services.AddSingleton<IHostedService>(provider => new ExcelWorker(provider.GetService<IOikDataApi>(),
                                                                              filename));
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