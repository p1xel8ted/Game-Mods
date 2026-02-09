// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.Wait
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("✫ Utility")]
public class Wait : ActionTask
{
  public BBParameter<float> waitTime = (BBParameter<float>) 1f;
  public CompactStatus finishStatus = CompactStatus.Success;

  public override string info => $"Wait {this.waitTime?.ToString()} sec.";

  public override void OnUpdate()
  {
    if ((double) this.elapsedTime < (double) this.waitTime.value)
      return;
    this.EndAction(this.finishStatus == CompactStatus.Success);
  }
}
