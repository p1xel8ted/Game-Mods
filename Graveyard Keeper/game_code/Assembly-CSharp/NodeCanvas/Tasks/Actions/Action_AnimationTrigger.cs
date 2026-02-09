// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.Action_AnimationTrigger
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Name("Animation Trigger", 0)]
[Category("Animation")]
public class Action_AnimationTrigger : WGOBehaviourAction
{
  public BBParameter<string> trigger_name = new BBParameter<string>("");
  public BBParameter<bool> return_true = new BBParameter<bool>(true);

  public override void OnExecute()
  {
    this.self_wgo.components.animator.SetTrigger(this.trigger_name.value);
    this.EndAction(this.return_true.value);
  }

  public override string info => $"Animation {this.trigger_name?.ToString()} trigger";
}
