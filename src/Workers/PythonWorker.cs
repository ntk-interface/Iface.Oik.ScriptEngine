using System;
using Iface.Oik.Tm.Interfaces;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace Iface.Oik.ScriptEngine.Workers
{
  public class PythonWorker : Worker
  {
    private readonly Microsoft.Scripting.Hosting.ScriptEngine _engine;
    private readonly ScriptScope  _engineScope;


    public PythonWorker(IOikDataApi api, string name, string script)
      : base(api, name, script)
    {
      _engine      = Python.CreateEngine();
      _engineScope = _engine.CreateScope();
      _engineScope.SetVariable("OverrideScriptTimeout", new Action<int>(OverrideScriptTimeout));
      _engineScope.SetVariable("GetTmStatus",           new Func<int, int, int, int>(GetTmStatus));
      _engineScope.SetVariable("GetTmAnalog",           new Func<int, int, int, float>(GetTmAnalog));
      _engineScope.SetVariable("SetTmStatus",           new Action<int, int, int, int>(SetTmStatus));
      _engineScope.SetVariable("SetTmAnalog",           new Action<int, int, int, float>(SetTmAnalog));
    }


    protected override void DoWork()
    {
      _engine.Execute(Script, _engineScope);
    }
  }
}