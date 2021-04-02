using System;
using System.IO;
using Iface.Oik.Tm.Interfaces;
using Jint;

namespace Iface.Oik.ScriptEngine.Workers
{
  public class JavascriptWorker : Worker
  {
    private readonly string _script;

    private readonly Engine _engine;


    public JavascriptWorker(IOikDataApi api, string filename)
      : base(api, filename)
    {
      _script = File.ReadAllText(filename);

      _engine = new Engine();
      _engine.SetValue("OverrideScriptTimeout", new Action<int>(OverrideScriptTimeout))
             .SetValue("AllowTelecontrol", new Action(AllowTelecontrol))
             .SetValue("GetTmStatus",      new Func<int, int, int, int>(GetTmStatus))
             .SetValue("GetTmAnalog",      new Func<int, int, int, float>(GetTmAnalog))
             .SetValue("SetTmStatus",      new Action<int, int, int, int>(SetTmStatus))
             .SetValue("SetTmAnalog",      new Action<int, int, int, float>(SetTmAnalog))
             .SetValue("IsTmStatusOn",     new Func<int, int, int, bool>(IsTmStatusOn))
             .SetValue("Telecontrol",      new Action<int, int, int, int>(Telecontrol));
    }


    protected override void DoWork()
    {
      _engine.Execute(_script);
    }
  }
}