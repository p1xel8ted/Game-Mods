// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.MecanimSetLookAt
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Name("Set Look At", 0)]
[Category("Animator")]
[Task.EventReceiver(new string[] {"OnAnimatorIK"})]
public class MecanimSetLookAt : ActionTask<Animator>
{
  public BBParameter<GameObject> targetPosition;
  public BBParameter<float> targetWeight;

  public override string info => "Mec.SetLookAt " + this.targetPosition?.ToString();

  public void OnAnimatorIK()
  {
    this.agent.SetLookAtPosition(this.targetPosition.value.transform.position);
    this.agent.SetLookAtWeight(this.targetWeight.value);
    this.EndAction();
  }
}
