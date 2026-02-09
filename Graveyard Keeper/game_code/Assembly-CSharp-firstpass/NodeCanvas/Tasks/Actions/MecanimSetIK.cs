// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.MecanimSetIK
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Task.EventReceiver(new string[] {"OnAnimatorIK"})]
[Name("Set IK", 0)]
[Category("Animator")]
public class MecanimSetIK : ActionTask<Animator>
{
  public AvatarIKGoal IKGoal;
  [RequiredField]
  public BBParameter<GameObject> goal;
  public BBParameter<float> weight;

  public override string info => $"Set '{this.IKGoal.ToString()}' {this.goal?.ToString()}";

  public void OnAnimatorIK()
  {
    this.agent.SetIKPositionWeight(this.IKGoal, this.weight.value);
    this.agent.SetIKPosition(this.IKGoal, this.goal.value.transform.position);
    this.EndAction();
  }
}
