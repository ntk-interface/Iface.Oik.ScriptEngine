using System;
using Iface.Oik.Tm.Interfaces;
using Jint;

namespace Iface.Oik.ScriptEngine.Workers
{
  public class JavascriptWorker : Worker
  {
    private readonly Engine _engine;


    public JavascriptWorker(IOikDataApi api, string name, string script)
      : base(api, name, script)
    {
      _engine = new Engine();
      _engine.SetValue("OverrideScriptTimeout", new Action<int>(OverrideScriptTimeout))
             .SetValue("GetTmStatus", new Func<int, int, int, int>(GetTmStatus))
             .SetValue("GetTmAnalog", new Func<int, int, int, float>(GetTmAnalog))
             .SetValue("SetTmStatus", new Action<int, int, int, int>(SetTmStatus))
             .SetValue("SetTmAnalog", new Action<int, int, int, float>(SetTmAnalog));
    }


    protected override void DoWork()
    {
      _engine.Execute(Script);
    }
  }
}