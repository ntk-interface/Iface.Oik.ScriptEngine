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
             .SetValue("AllowTelecontrol",       new Action(AllowTelecontrol))
             .SetValue("TM",                     new Func<string, float>(GetExpressionResultFloat))
             .SetValue("GetTmStatus",            new Func<int, int, int, int>(GetTmStatus))
             .SetValue("IsTmStatusOn",           new Func<int, int, int, bool>(IsTmStatusOn))
             .SetValue("IsTmStatusFlagRaised",   new Func<int, int, int, TmFlags, bool>(IsTmStatusFlagRaised))
             .SetValue("GetTmStatusFromRetro",   new Func<int, int, int, long, int>(GetTmStatusFromRetro))
             .SetValue("GetTmAnalog",            new Func<int, int, int, float>(GetTmAnalog))
             .SetValue("IsTmAnalogFlagRaised",   new Func<int, int, int, TmFlags, bool>(IsTmAnalogFlagRaised))
             .SetValue("GetTmAnalogFromRetro",   new Func<int, int, int, long, int?, float>(GetTmAnalogFromRetro))
             .SetValue("GetTmAnalogRetro",       new Func<int, int, int, long, long, int, int?, float[]>(GetTmAnalogRetro))
             .SetValue("GetTmAnalogImpulseArchiveAverage", 
                       new Func<int, int, int, long, long, int, float[]>(GetTmAnalogImpulseArchiveAverage))
             .SetValue("GetTmAnalogMicroSeries", new Func<int, int, int, float[]>(GetTmAnalogMicroSeries))
             .SetValue("SetTmStatus",            new Action<int, int, int, int>(SetTmStatus))
             .SetValue("RaiseTmStatusFlag",      new Action<int, int, int, TmFlags>(RaiseTmStatusFlag))
             .SetValue("ClearTmStatusFlag",      new Action<int, int, int, TmFlags>(ClearTmStatusFlag))
             .SetValue("SetTmAnalog",            new Action<int, int, int, float>(SetTmAnalog))
             .SetValue("RaiseTmAnalogFlag",      new Action<int, int, int, TmFlags>(RaiseTmAnalogFlag))
             .SetValue("ClearTmAnalogFlag",      new Action<int, int, int, TmFlags>(ClearTmAnalogFlag))
             .SetValue("Telecontrol",            new Action<int, int, int, int>(Telecontrol))
             .SetValue("SetToStorage",           new Action<string, object>(SetToStorage))
             .SetValue("GetFromStorage",         new Func<string, object>(GetFromStorage))
             .SetValue("LogDebug",               new Action<string>(LogDebug))
             .SetValue("TmFlagUnreliable",       (int)TmFlags.Unreliable)
             .SetValue("TmFlagInvalid",          (int)TmFlags.Invalid)
             .SetValue("TmFlagAbnormal",         (int)TmFlags.Abnormal)
             .SetValue("TmFlagManuallyBlocked",  (int)TmFlags.ManuallyBlocked)
             .SetValue("TmFlagManuallySet",      (int)TmFlags.ManuallySet)
             .SetValue("TmFlagLevel1",           (int)TmFlags.LevelA)
             .SetValue("TmFlagLevel2",           (int)TmFlags.LevelB)
             .SetValue("TmFlagLevel3",           (int)TmFlags.LevelC)
             .SetValue("TmFlagLevel4",           (int)TmFlags.LevelD)
             .SetValue("TmFlagUnacked",          (int)TmFlags.Unacked)
        ;
    }


    protected override void DoWork()
    {
      _engine.Execute(_script);
    }
  }
}