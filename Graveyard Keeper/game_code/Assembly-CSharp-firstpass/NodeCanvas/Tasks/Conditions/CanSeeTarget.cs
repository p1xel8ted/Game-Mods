// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.CanSeeTarget
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Description("A combination of line of sight and view angle check")]
[Category("GameObject")]
public class CanSeeTarget : ConditionTask<Transform>
{
  [RequiredField]
  public BBParameter<GameObject> target;
  public BBParameter<float> maxDistance = (BBParameter<float>) 50f;
  public BBParameter<float> awarnessDistance = (BBParameter<float>) 0.0f;
  [SliderField(1, 180)]
  public BBParameter<float> viewAngle = (BBParameter<float>) 70f;
  public Vector3 offset;
  public RaycastHit hit;

  public override string info => "Can See " + this.target?.ToString();

  public override bool OnCheck()
  {
    Transform transform = this.target.value.transform;
    return (double) Vector3.Distance(this.agent.position, transform.position) <= (double) this.maxDistance.value && (!Physics.Linecast(this.agent.position + this.offset, transform.position + this.offset, out this.hit) || !((Object) this.hit.collider != (Object) transform.GetComponent<Collider>())) && ((double) Vector3.Angle(transform.position - this.agent.position, this.agent.forward) < (double) this.viewAngle.value || (double) Vector3.Distance(this.agent.position, transform.position) < (double) this.awarnessDistance.value);
  }

  public override void OnDrawGizmosSelected()
  {
    if (!((Object) this.agent != (Object) null))
      return;
    Gizmos.DrawLine(this.agent.position, this.agent.position + this.offset);
    Gizmos.DrawLine(this.agent.position + this.offset, this.agent.position + this.offset + this.agent.forward * this.maxDistance.value);
    Gizmos.DrawWireSphere(this.agent.position + this.offset + this.agent.forward * this.maxDistance.value, 0.1f);
    Gizmos.DrawWireSphere(this.agent.position, this.awarnessDistance.value);
    Gizmos.matrix = Matrix4x4.TRS(this.agent.position + this.offset, this.agent.rotation, Vector3.one);
    Gizmos.DrawFrustum(Vector3.zero, this.viewAngle.value, 5f, 0.0f, 1f);
  }
}
