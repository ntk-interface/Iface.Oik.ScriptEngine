using System;
using System.IO;
using Iface.Oik.Tm.Interfaces;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace Iface.Oik.ScriptEngine.Workers
{
  public class PythonWorker : Worker
  {
    private readonly string _script;

    private readonly Microsoft.Scripting.Hosting.ScriptEngine _engine;
    private readonly ScriptScope                              _engineScope;


    public PythonWorker(IOikDataApi api, string filename)
      : base(api, filename)
    {
      _script = File.ReadAllText(filename);

      _engine = Python.CreateEngine();
      UpdateEngineSearchPath();

      _engineScope = _engine.CreateScope();
      _engineScope.SetVariable("OverrideScriptTimeout", new Action<int>(OverrideScriptTimeout));
      _engineScope.SetVariable("AllowTelecontrol",      new Action(AllowTelecontrol));
      _engineScope.SetVariable("TM",                    new Func<string, float>(GetExpressionResultFloat));
      _engineScope.SetVariable("GetTmStatus",           new Func<int, int, int, int>(GetTmStatus));
      _engineScope.SetVariable("IsTmStatusOn",          new Func<int, int, int, bool>(IsTmStatusOn));
      _engineScope.SetVariable("IsTmStatusFlagRaised",  new Func<int, int, int, TmFlags, bool>(IsTmStatusFlagRaised));
      _engineScope.SetVariable("GetTmAnalog",           new Func<int, int, int, float>(GetTmAnalog));
      _engineScope.SetVariable("IsTmAnalogFlagRaised",  new Func<int, int, int, TmFlags, bool>(IsTmAnalogFlagRaised));
      _engineScope.SetVariable("SetTmStatus",           new Action<int, int, int, int>(SetTmStatus));
      _engineScope.SetVariable("RaiseTmStatusFlag",     new Action<int, int, int, TmFlags>(RaiseTmStatusFlag));
      _engineScope.SetVariable("ClearTmStatusFlag",     new Action<int, int, int, TmFlags>(ClearTmStatusFlag));
      _engineScope.SetVariable("SetTmAnalog",           new Action<int, int, int, float>(SetTmAnalog));
      _engineScope.SetVariable("RaiseTmAnalogFlag",     new Action<int, int, int, TmFlags>(RaiseTmAnalogFlag));
      _engineScope.SetVariable("ClearTmAnalogFlag",     new Action<int, int, int, TmFlags>(ClearTmAnalogFlag));
      _engineScope.SetVariable("Telecontrol",           new Action<int, int, int, int>(Telecontrol));
      _engineScope.SetVariable("FLAG_UNRELIABLE",       TmFlags.Unreliable);
      _engineScope.SetVariable("FLAG_INVALID",          TmFlags.Invalid);
      _engineScope.SetVariable("FLAG_ABNORMAL",         TmFlags.Abnormal);
      _engineScope.SetVariable("FLAG_MANUALLY_BLOCKED", TmFlags.ManuallyBlocked);
      _engineScope.SetVariable("FLAG_MANUALLY_SET",     TmFlags.ManuallySet);
      _engineScope.SetVariable("FLAG_LEVEL_1",          TmFlags.LevelA);
      _engineScope.SetVariable("FLAG_LEVEL_2",          TmFlags.LevelB);
      _engineScope.SetVariable("FLAG_LEVEL_3",          TmFlags.LevelC);
      _engineScope.SetVariable("FLAG_LEVEL_4",          TmFlags.LevelD);
      _engineScope.SetVariable("FLAG_UNACKED",          TmFlags.Unacked);
    }


    protected override void DoWork()
    {
      _engine.Execute(_script, _engineScope);
    }


    private void UpdateEngineSearchPath()
    {
      var paths = _engine.GetSearchPaths();
      paths.Add("PythonLib.zip");
      _engine.SetSearchPaths(paths);
    }
  }
}