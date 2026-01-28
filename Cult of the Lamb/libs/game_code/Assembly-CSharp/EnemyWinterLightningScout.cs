// Decompiled with JetBrains decompiler
// Type: EnemyWinterLightningScout
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMRoomGeneration;
using System;
using UnityEngine;

#nullable disable
public class EnemyWinterLightningScout : EnemyScuttleScout
{
  public bool hasBeenAggrod;
  public bool DropBombOnDeath;
  public float DistanceToChasePlayer = 7f;

  public override void GetNewTargetPosition()
  {
    float num1 = 100f;
    Health closestTarget = this.GetClosestTarget();
    if ((UnityEngine.Object) closestTarget == (UnityEngine.Object) null)
      return;
    float num2 = Vector3.Distance(this.transform.position, closestTarget.transform.position);
    if ((double) num2 >= (double) this.DistanceToChasePlayer && this.hasBeenAggrod || (UnityEngine.Object) closestTarget != (UnityEngine.Object) null && (double) this.ChanceToPathTowardsPlayer > 0.0 && (double) UnityEngine.Random.value < (double) this.ChanceToPathTowardsPlayer && (double) num2 < (double) this.DistanceToPathTowardsPlayer && this.CheckLineOfSightOnTarget(closestTarget.gameObject, closestTarget.transform.position, (float) this.DistanceToPathTowardsPlayer))
    {
      this.PathingToPlayer = true;
      this.RandomDirection = Utils.GetAngle(this.transform.position, closestTarget.transform.position) * ((float) Math.PI / 180f);
    }
    while ((double) --num1 > 0.0)
    {
      float distance = UnityEngine.Random.Range(this.DistanceRange.x, this.DistanceRange.y);
      if (!this.PathingToPlayer)
        this.RandomDirection += UnityEngine.Random.Range(-this.TurningArc, this.TurningArc) * ((float) Math.PI / 180f);
      this.PathingToPlayer = false;
      float radius = 0.2f;
      Vector3 targetLocation = this.transform.position + new Vector3(distance * Mathf.Cos(this.RandomDirection), distance * Mathf.Sin(this.RandomDirection));
      RaycastHit2D raycastHit2D = Physics2D.CircleCast((Vector2) this.transform.position, radius, (Vector2) Vector3.Normalize(targetLocation - this.transform.position), distance, (int) this.layerToCheck);
      if ((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null)
      {
        if (this.ShowDebug)
        {
          this.Points.Add(new Vector3(raycastHit2D.centroid.x, raycastHit2D.centroid.y) + Vector3.Normalize(this.transform.position - targetLocation) * this.CircleCastOffset);
          this.PointsLink.Add(new Vector3(this.transform.position.x, this.transform.position.y));
        }
        this.RandomDirection = 180f - this.RandomDirection;
      }
      else
      {
        if (this.ShowDebug)
        {
          this.EndPoints.Add(new Vector3(targetLocation.x, targetLocation.y));
          this.EndPointsLink.Add(new Vector3(this.transform.position.x, this.transform.position.y));
        }
        this.IdleWait = UnityEngine.Random.Range(this.IdleWaitRange.x, this.IdleWaitRange.y);
        this.givePath(targetLocation);
        this.hasBeenAggrod = true;
        break;
      }
    }
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    if (!string.IsNullOrEmpty(this.DeathVO))
      AudioManager.Instance.PlayOneShot(this.DeathVO, this.transform.position);
    if (!this.DropBombOnDeath)
      return;
    Vector3 vector3 = new Vector3(this.transform.position.x, this.transform.position.y, (UnityEngine.Object) GenerateRoom.Instance != (UnityEngine.Object) null ? GenerateRoom.Instance.transform.position.z : this.transform.position.z);
    BombLightning.CreateBomb(vector3, this.health, this.transform.parent, 1f);
    AudioManager.Instance.PlayOneShot("event:/boss/spider/bomb_shoot", vector3);
  }
}
