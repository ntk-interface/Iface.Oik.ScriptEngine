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
      _engineScope.SetVariable("GetTmStatusOrDefault",  new Func<int, int, int, int, int>(GetTmStatusOrDefault));
      _engineScope.SetVariable("GetTmStatusName",       new Func<int, int, int, string>(GetTmStatusName));
      _engineScope.SetVariable("GetTmStatusFromRetro",  new Func<int, int, int, long, int?>(GetTmStatusFromRetro));
      _engineScope.SetVariable("IsTmStatusOn",          new Func<int, int, int, bool>(IsTmStatusOn));
      _engineScope.SetVariable("IsTmStatusFlagRaised",  new Func<int, int, int, TmFlags, bool>(IsTmStatusFlagRaised));
      _engineScope.SetVariable("GetTmAnalog",           new Func<int, int, int, float>(GetTmAnalog));
      _engineScope.SetVariable("GetTmAnalogOrDefault",  new Func<int, int, int, float, float>(GetTmAnalogOrDefault));
      _engineScope.SetVariable("GetTmAnalogName",       new Func<int, int, int, string>(GetTmAnalogName));
      _engineScope.SetVariable("GetTmAnalogUnit",       new Func<int, int, int, string>(GetTmAnalogUnit));
      _engineScope.SetVariable("GetTmAnalogFromRetro",  new Func<int, int, int, long, int?, float?>(GetTmAnalogFromRetro));
      _engineScope.SetVariable("GetTmAnalogRetro",      new Func<int, int, int, long, long, int, int?, float?[]>(GetTmAnalogRetro));
      _engineScope.SetVariable("GetTmAnalogImpulseArchiveAverage", new Func<int, int, int, long, long, int, float?[]>(GetTmAnalogImpulseArchiveAverage));
      _engineScope.SetVariable("GetTmAnalogMicroSeries", new Func<int, int, int, float?[]>(GetTmAnalogMicroSeries));
      _engineScope.SetVariable("IsTmAnalogFlagRaised",  new Func<int, int, int, TmFlags, bool>(IsTmAnalogFlagRaised));
      _engineScope.SetVariable("GetTmAccum",            new Func<int, int, int, float>(GetTmAccum));
      _engineScope.SetVariable("GetTmAccumOrDefault",   new Func<int, int, int, float, float>(GetTmAccumOrDefault));
      _engineScope.SetVariable("GetTmAccumLoad",         new Func<int, int, int, float>(GetTmAccumLoad));
      _engineScope.SetVariable("GetTmAccumLoadOrDefault", new Func<int, int, int, float, float>(GetTmAccumLoadOrDefault));
      _engineScope.SetVariable("GetTmAccumName",        new Func<int, int, int, string>(GetTmAccumName));
      _engineScope.SetVariable("GetTmAccumUnit",        new Func<int, int, int, string>(GetTmAccumUnit));
      _engineScope.SetVariable("GetTmAccumFromRetro",   new Func<int, int, int, long, float?>(GetTmAccumFromRetro));
      _engineScope.SetVariable("IsTmAccumFlagRaised",   new Func<int, int, int, TmFlags, bool>(IsTmAccumFlagRaised));
      _engineScope.SetVariable("SetTmStatus",           new Action<int, int, int, int>(SetTmStatus));
      _engineScope.SetVariable("RaiseTmStatusFlag",     new Action<int, int, int, TmFlags>(RaiseTmStatusFlag));
      _engineScope.SetVariable("ClearTmStatusFlag",     new Action<int, int, int, TmFlags>(ClearTmStatusFlag));
      _engineScope.SetVariable("SetTmAnalog",           new Action<int, int, int, float>(SetTmAnalog));
      _engineScope.SetVariable("RaiseTmAnalogFlag",     new Action<int, int, int, TmFlags>(RaiseTmAnalogFlag));
      _engineScope.SetVariable("ClearTmAnalogFlag",     new Action<int, int, int, TmFlags>(ClearTmAnalogFlag));
      _engineScope.SetVariable("Telecontrol",           new Action<int, int, int, int>(Telecontrol));
      _engineScope.SetVariable("TeleregulateByStepUp",  new Action<int, int, int>(TeleregulateByStepUp));
      _engineScope.SetVariable("TeleregulateByStepDown",new Action<int, int, int>(TeleregulateByStepDown));
      _engineScope.SetVariable("TeleregulateByCode",    new Action<int, int, int, int>(TeleregulateByCode));
      _engineScope.SetVariable("TeleregulateByValue",   new Action<int, int, int, float>(TeleregulateByValue));
      _engineScope.SetVariable("WriteToStorage",        new Action<string, object>(WriteToStorage));
      _engineScope.SetVariable("ReadFromStorage",       new Func<string, object>(ReadFromStorage));
      _engineScope.SetVariable("LogDebug",              new Action<string>(LogDebug));
      _engineScope.SetVariable("TmFlagUnreliable",      TmFlags.Unreliable);
      _engineScope.SetVariable("TmFlagInvalid",         TmFlags.Invalid);
      _engineScope.SetVariable("TmFlagAbnormal",        TmFlags.Abnormal);
      _engineScope.SetVariable("TmFlagManuallyBlocked", TmFlags.ManuallyBlocked);
      _engineScope.SetVariable("TmFlagManuallySet",     TmFlags.ManuallySet);
      _engineScope.SetVariable("TmFlagLevel1",          TmFlags.LevelA);
      _engineScope.SetVariable("TmFlagLevel2",          TmFlags.LevelB);
      _engineScope.SetVariable("TmFlagLevel3",          TmFlags.LevelC);
      _engineScope.SetVariable("TmFlagLevel4",          TmFlags.LevelD);
      _engineScope.SetVariable("TmFlagUnacked",         TmFlags.Unacked);
    }


    protected override void DoWork()
    {
      _engine.Execute(_script, _engineScope);
    }


    private void UpdateEngineSearchPath()
    {
      var paths = _engine.GetSearchPaths();
      paths.Add(Path.Combine(AppContext.BaseDirectory, "PythonLib"));
      paths.Add(Path.Combine(AppContext.BaseDirectory, "PythonLib.zip"));
      _engine.SetSearchPaths(paths);
    }
  }
}