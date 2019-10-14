using System;
using System.Threading.Tasks;
using Jint;

namespace Iface.Oik.ScriptEngine.Engines
{
  public class JavascriptEngine : ScriptEngine
  {
    private Engine _engine;


    public JavascriptEngine(string name, string script)
      : base(name, script)
    {
    }


    public override void InitEngine()
    {
      _engine = new Engine();
      _engine.SetValue("OverrideScriptTimeout", new Action<int>(OverrideScriptTimeout))
             .SetValue("GetTmStatus", new Func<int, int, int, int>(GetTmStatus))
             .SetValue("GetTmAnalog", new Func<int, int, int, float>(GetTmAnalog))
             .SetValue("SetTmStatus", new Action<int, int, int, int>(SetTmStatus))
             .SetValue("SetTmAnalog", new Action<int, int, int, float>(SetTmAnalog));
    }


    public override async Task ExecuteScript()
    {
      _engine.Execute(_script);
    }
  }
}