// Decompiled with JetBrains decompiler
// Type: EnemyMillipedeSplitterMiniboss
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class EnemyMillipedeSplitterMiniboss : EnemyMillipedeSpiker
{
  [SerializeField]
  private float speedIncrementPerHit;
  [SerializeField]
  private float height;
  [SerializeField]
  private float duration;
  [SerializeField]
  private float partRadius;
  [SerializeField]
  private AnimationCurve heightCurve;
  [SerializeField]
  private LayerMask avoidMask;
  private List<MillipedeBodyPart> parts;
  private float startingHP;
  private int totalBodyParts;

  public override void Awake()
  {
    base.Awake();
    this.parts = ((IEnumerable<MillipedeBodyPart>) this.GetComponentsInChildren<MillipedeBodyPart>()).ToList<MillipedeBodyPart>();
    this.totalBodyParts = this.parts.Count;
    this.startingHP = this.health.totalHP;
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    int index1 = this.totalBodyParts - Mathf.RoundToInt((this.startingHP - this.health.HP) / (this.startingHP / (float) this.totalBodyParts));
    this.maxSpeed += (float) (this.parts.Count - index1) * this.speedIncrementPerHit;
    this.turnDamper -= (float) (this.parts.Count - index1) * 0.05f;
    if (this.parts.Count <= index1)
      return;
    for (int index2 = index1; index2 < this.parts.Count; ++index2)
      this.DropBodyPart(this.parts[index2]);
    this.parts.RemoveRange(index1, this.parts.Count - index1);
  }

  private void DropBodyPart(MillipedeBodyPart bodyPart)
  {
    bodyPart.GetComponent<FollowAsTail>().enabled = false;
    bodyPart.GetComponent<Health>().enabled = false;
    this.flashes.RemoveAt(this.flashes.Count - 1);
    bodyPart.DroppedPart();
    this.spines.Remove(bodyPart.GetComponent<SkeletonAnimation>());
    this.StartCoroutine((IEnumerator) this.ThrowBodyPart(bodyPart));
  }

  private IEnumerator ThrowBodyPart(MillipedeBodyPart bodyPart)
  {
    Vector3 fromPosition = bodyPart.transform.position;
    Vector3 targetPosition = this.GetRandomPosition() with
    {
      z = 0.0f
    };
    float t = 0.0f;
    while ((double) t < (double) this.duration)
    {
      float num = t / this.duration;
      Vector3 vector3 = Vector3.Lerp(fromPosition, targetPosition, num) with
      {
        z = (float) ((double) this.heightCurve.Evaluate(num) * (double) this.height * -1.0)
      };
      bodyPart.transform.position = vector3;
      t += Time.deltaTime;
      yield return (object) null;
    }
  }

  private Vector3 GetRandomPosition()
  {
    int num = 20;
    float distance = 2f;
    while (--num > 0)
    {
      Vector3 origin = this.GetCenterPosition() + (Vector3) Random.insideUnitCircle * distance;
      if ((bool) Physics2D.CircleCast((Vector2) origin, this.partRadius, Vector2.zero, 0.0f, (int) this.avoidMask))
      {
        distance = Mathf.Clamp(distance + 1f, 0.0f, 6f);
      }
      else
      {
        Vector3 centerPosition = this.GetCenterPosition();
        Vector3 normalized = (origin - centerPosition).normalized;
        if (!(bool) Physics2D.Raycast((Vector2) centerPosition, (Vector2) normalized, distance, (int) this.layerToCheck))
          return origin;
      }
    }
    return (Vector3) (Random.insideUnitCircle * 5f);
  }
}
