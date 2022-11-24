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
             .SetValue("AllowTelecontrol",      new Action(AllowTelecontrol))
             .SetValue("TM",                    new Func<string, float>(GetExpressionResultFloat))
             .SetValue("GetTmStatus",           new Func<int, int, int, int>(GetTmStatus))
             .SetValue("IsTmStatusOn",          new Func<int, int, int, bool>(IsTmStatusOn))
             .SetValue("IsTmStatusFlagRaised",  new Func<int, int, int, TmFlags, bool>(IsTmStatusFlagRaised))
             .SetValue("GetTmStatusFromRetro",  new Func<int, int, int, long, float>(GetTmStatusFromRetro))
             .SetValue("GetTmAnalog",           new Func<int, int, int, float>(GetTmAnalog))
             .SetValue("IsTmAnalogFlagRaised",  new Func<int, int, int, TmFlags, bool>(IsTmAnalogFlagRaised))
             .SetValue("GetTmAnalogFromRetro",  new Func<int, int, int, long, int?, float>(GetTmAnalogFromRetro))
             .SetValue("SetTmStatus",           new Action<int, int, int, int>(SetTmStatus))
             .SetValue("RaiseTmStatusFlag",     new Action<int, int, int, TmFlags>(RaiseTmStatusFlag))
             .SetValue("ClearTmStatusFlag",     new Action<int, int, int, TmFlags>(ClearTmStatusFlag))
             .SetValue("SetTmAnalog",           new Action<int, int, int, float>(SetTmAnalog))
             .SetValue("RaiseTmAnalogFlag",     new Action<int, int, int, TmFlags>(RaiseTmAnalogFlag))
             .SetValue("ClearTmAnalogFlag",     new Action<int, int, int, TmFlags>(ClearTmAnalogFlag))
             .SetValue("Telecontrol",           new Action<int, int, int, int>(Telecontrol))
             .SetValue("FLAG_UNRELIABLE",       (int)TmFlags.Unreliable)
             .SetValue("FLAG_INVALID",          (int)TmFlags.Invalid)
             .SetValue("FLAG_ABNORMAL",         (int)TmFlags.Abnormal)
             .SetValue("FLAG_MANUALLY_BLOCKED", (int)TmFlags.ManuallyBlocked)
             .SetValue("FLAG_MANUALLY_SET",     (int)TmFlags.ManuallySet)
             .SetValue("FLAG_LEVEL_1",          (int)TmFlags.LevelA)
             .SetValue("FLAG_LEVEL_2",          (int)TmFlags.LevelB)
             .SetValue("FLAG_LEVEL_3",          (int)TmFlags.LevelC)
             .SetValue("FLAG_LEVEL_4",          (int)TmFlags.LevelD)
             .SetValue("FLAG_UNACKED",          (int)TmFlags.Unacked)
        ;
    }


    protected override void DoWork()
    {
      _engine.Execute(_script);
    }
  }
}