// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.InputMove
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Description("Move & turn the agent based on input values provided ranging from -1 to 1, per second (using delta time)")]
[Category("Movement/Direct")]
public class InputMove : ActionTask<Transform>
{
  [BlackboardOnly]
  public BBParameter<float> strafe;
  [BlackboardOnly]
  public BBParameter<float> turn;
  [BlackboardOnly]
  public BBParameter<float> forward;
  [BlackboardOnly]
  public BBParameter<float> up;
  public BBParameter<float> moveSpeed = (BBParameter<float>) 1f;
  public BBParameter<float> rotationSpeed = (BBParameter<float>) 1f;
  public bool repeat;

  public override void OnUpdate()
  {
    this.agent.rotation = Quaternion.Slerp(this.agent.rotation, this.agent.rotation * Quaternion.Euler(Vector3.up * this.turn.value * 10f), this.rotationSpeed.value * Time.deltaTime);
    Vector3 vector3_1 = this.agent.forward * this.forward.value * this.moveSpeed.value * Time.deltaTime;
    Vector3 vector3_2 = this.agent.right * this.strafe.value * this.moveSpeed.value * Time.deltaTime;
    Vector3 vector3_3 = this.agent.up * this.up.value * this.moveSpeed.value * Time.deltaTime;
    this.agent.position += vector3_2 + vector3_1 + vector3_3;
    if (this.repeat)
      return;
    this.EndAction();
  }
}
