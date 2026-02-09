// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.IsInFront
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Name("Target In View Angle", 0)]
[Category("GameObject")]
[Description("Checks whether the target is in the view angle of the agent")]
public class IsInFront : ConditionTask<Transform>
{
  [RequiredField]
  public BBParameter<GameObject> checkTarget;
  [SliderField(1, 180)]
  public BBParameter<float> viewAngle = (BBParameter<float>) 70f;

  public override string info => this.checkTarget?.ToString() + " in view angle";

  public override bool OnCheck()
  {
    return (double) Vector3.Angle(this.checkTarget.value.transform.position - this.agent.position, this.agent.forward) < (double) this.viewAngle.value;
  }

  public override void OnDrawGizmosSelected()
  {
    if (!((Object) this.agent != (Object) null))
      return;
    Gizmos.matrix = Matrix4x4.TRS(this.agent.position, this.agent.rotation, Vector3.one);
    Gizmos.DrawFrustum(Vector3.zero, this.viewAngle.value, 5f, 0.0f, 1f);
  }
}
