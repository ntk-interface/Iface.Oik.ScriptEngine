using System;
using Iface.Oik.Tm.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Iface.Oik.ScriptEngine
{
  class Program
  {
    static void Main(string[] args)
    {
      using (var startup = new Startup(args))
      {
        startup.ConfigureServices(new ServiceCollection());
        try
        {
          startup.StartServices();
        }
        catch (Exception ex)
        {
          Tms.PrintError(ex.Message);
          return;
        }

        var isCancelRequested = false;
        Console.CancelKeyPress += (s, e) =>
        {
          isCancelRequested = true;
          e.Cancel          = true;
        };
      
        while (true)
        {
          if (isCancelRequested || Tms.StopEventSignalDuringWait(startup.StopEventHandle, 1000))
          {
            break;
          }
        }
      }
    }
  }
}