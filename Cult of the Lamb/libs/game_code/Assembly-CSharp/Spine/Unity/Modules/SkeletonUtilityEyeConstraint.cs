// Decompiled with JetBrains decompiler
// Type: Spine.Unity.Modules.SkeletonUtilityEyeConstraint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Spine.Unity.Modules;

public class SkeletonUtilityEyeConstraint : SkeletonUtilityConstraint
{
  public Transform[] eyes;
  public float radius = 0.5f;
  public Transform target;
  public Vector3 targetPosition;
  public float speed = 10f;
  public Vector3[] origins;
  public Vector3 centerPoint;

  public override void OnEnable()
  {
    if (!Application.isPlaying)
      return;
    base.OnEnable();
    Bounds bounds = new Bounds(this.eyes[0].localPosition, Vector3.zero);
    this.origins = new Vector3[this.eyes.Length];
    for (int index = 0; index < this.eyes.Length; ++index)
    {
      this.origins[index] = this.eyes[index].localPosition;
      bounds.Encapsulate(this.origins[index]);
    }
    this.centerPoint = bounds.center;
  }

  public override void OnDisable()
  {
    if (!Application.isPlaying)
      return;
    base.OnDisable();
  }

  public override void DoUpdate()
  {
    if ((Object) this.target != (Object) null)
      this.targetPosition = this.target.position;
    Vector3 vector3_1 = this.targetPosition - this.transform.TransformPoint(this.centerPoint);
    if ((double) vector3_1.magnitude > 1.0)
      vector3_1.Normalize();
    for (int index = 0; index < this.eyes.Length; ++index)
    {
      Vector3 vector3_2 = this.transform.TransformPoint(this.origins[index]);
      this.eyes[index].position = Vector3.MoveTowards(this.eyes[index].position, vector3_2 + vector3_1 * this.radius, this.speed * Time.deltaTime);
    }
  }
}
