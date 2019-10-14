using System;
using System.Threading.Tasks;
using IronPython.Hosting;

namespace Iface.Oik.ScriptEngine.Engines
{
  public class PythonEngine : ScriptEngine
  {
    private Microsoft.Scripting.Hosting.ScriptEngine _engine;
    private Microsoft.Scripting.Hosting.ScriptScope  _engineScope;


    public PythonEngine(string name, string script)
      : base(name, script)
    {
    }


    public override void InitEngine()
    {
      _engine      = Python.CreateEngine();
      _engineScope = _engine.CreateScope();
      _engineScope.SetVariable("OverrideScriptTimeout", new Action<int>(OverrideScriptTimeout));
      _engineScope.SetVariable("GetTmStatus",           new Func<int, int, int, int>(GetTmStatus));
      _engineScope.SetVariable("GetTmAnalog",           new Func<int, int, int, float>(GetTmAnalog));
      _engineScope.SetVariable("SetTmStatus",           new Action<int, int, int, int>(SetTmStatus));
      _engineScope.SetVariable("SetTmAnalog",           new Action<int, int, int, float>(SetTmAnalog));
    }


    public override async Task ExecuteScript()
    {
      _engine.Execute(_script, _engineScope);
    }
  }
}