// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.DebugLogVariable
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Description("Logs the value of a variable in the console")]
[Category("✫ Utility")]
public class DebugLogVariable : ActionTask
{
  [BlackboardOnly]
  public BBParameter<object> log;
  public BBParameter<string> prefix;
  public float secondsToRun = 1f;
  public CompactStatus finishStatus = CompactStatus.Success;

  public override string info
  {
    get
    {
      return $"Log '{this.log?.ToString()}'{((double) this.secondsToRun > 0.0 ? $" for {this.secondsToRun.ToString()} sec." : "")}";
    }
  }

  public override void OnExecute()
  {
    Debug.Log((object) $"<b>({this.agent.gameObject.name}) ({this.prefix.value}) | Var '{this.log.name}' = </b> {this.log.value}", (Object) this.agent.gameObject);
  }

  public override void OnUpdate()
  {
    if ((double) this.elapsedTime < (double) this.secondsToRun)
      return;
    this.EndAction(this.finishStatus == CompactStatus.Success);
  }
}
