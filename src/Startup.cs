using System;
using System.Linq;
using System.Text;
using Iface.Oik.Tm.Native.Api;
using Iface.Oik.Tm.Native.Interfaces;
using Iface.Oik.Tm.Api;
using Iface.Oik.Tm.Helpers;
using Iface.Oik.Tm.Interfaces;
using Iface.Oik.Tm.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Iface.Oik.ScriptEngine
{
  public class Startup : IDisposable
  {
    private IServiceProvider _serviceProvider;
    private bool             _servicesStarted;

    private readonly string _host;
    private readonly string _tmServer;
    private readonly string _user;
    private readonly string _password;

    public uint StopEventHandle { get; private set; }


    public Startup(string[] args)
    {
      Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // требуется для работы с кодировкой Win-1251

      _tmServer = args.ElementAtOrDefault(0) ?? "TMS";
      _host     = args.ElementAtOrDefault(1) ?? ".";
      _user     = args.ElementAtOrDefault(2) ?? "";
      _password = args.ElementAtOrDefault(3) ?? "";
    }


    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
      services.AddTransient<App>();

      services.AddSingleton<ITmNative, TmNative>();
      services.AddSingleton<ITmsApi, TmsApi>();
      services.AddSingleton<IOikSqlApi, OikSqlApi>();
      services.AddSingleton<IOikDataApi, OikDataApi>();
      services.AddSingleton<ICommonServerService, CommonServerService>();
      services.AddSingleton<ICommonInfrastructure, CommonInfrastructure>();

      services.AddSingleton<IBackgroundService>(provider => provider.GetService<ICommonServerService>());
      services.AddSingleton<IBackgroundService, App>();

      _serviceProvider = services.BuildServiceProvider();

      return _serviceProvider;
    }


    public void StartServices()
    {
      var (tmCid, userInfo, stopEventHandle) = Tms.InitializeAsTaskWithoutSql(new TmOikTaskOptions
                                                                              {
                                                                                TraceName    = "OikTaskName",
                                                                                TraceComment = "<OikTaskComment>",
                                                                              },
                                                                              new TmInitializeOptions
                                                                              {
                                                                                ApplicationName = "<OikTask>",
                                                                                Host            = _host,
                                                                                TmServer        = _tmServer,
                                                                                User            = _user,
                                                                                Password        = _password,
                                                                              });

      Tms.PrintMessage($"Соединение с сервером {_host}/{_tmServer} установлено");

      _serviceProvider.GetService<ICommonInfrastructure>()
                      .InitializeTmWithoutSql(tmCid, userInfo);

      _serviceProvider.GetServices<IBackgroundService>()
                      .ForEach(s => s.StartAsync().Wait());

      _servicesStarted = true;
      StopEventHandle  = stopEventHandle;
    }


    public void Dispose()
    {
      if (!_servicesStarted)
      {
        return;
      }

      _serviceProvider.GetServices<IBackgroundService>()
                      .ForEach(s => s.StopAsync().Wait());

      var infr = _serviceProvider.GetService<ICommonInfrastructure>();

      infr.TerminateTm();
      Tms.TerminateWithoutSql(infr.TmCid);

      Tms.PrintMessage($"Соединение с сервером закрыто");
    }
  }
}