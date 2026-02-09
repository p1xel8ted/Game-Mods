// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.RotateTowards
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("Movement/Direct")]
[Description("Rotate the agent towards the target per frame")]
public class RotateTowards : ActionTask<Transform>
{
  [RequiredField]
  public BBParameter<GameObject> target;
  public BBParameter<float> speed = (BBParameter<float>) 2f;
  [SliderField(1, 180)]
  public BBParameter<float> angleDifference = (BBParameter<float>) 5f;
  public BBParameter<Vector3> upVector = (BBParameter<Vector3>) Vector3.up;
  public bool waitActionFinish;

  public override void OnUpdate()
  {
    if ((double) Vector3.Angle(this.target.value.transform.position - this.agent.position, this.agent.forward) <= (double) this.angleDifference.value)
    {
      this.EndAction();
    }
    else
    {
      this.agent.rotation = Quaternion.LookRotation(Vector3.RotateTowards(this.agent.forward, this.target.value.transform.position - this.agent.position, this.speed.value * Time.deltaTime, 0.0f), this.upVector.value);
      if (this.waitActionFinish)
        return;
      this.EndAction();
    }
  }
}
