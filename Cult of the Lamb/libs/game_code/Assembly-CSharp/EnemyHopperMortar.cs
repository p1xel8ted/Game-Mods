// Decompiled with JetBrains decompiler
// Type: EnemyHopperMortar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnemyHopperMortar : EnemyHopperBurp
{
  public const float aimDuration = 1f;
  public const float minBombRange = 2.5f;
  public const float maxBombRange = 5f;
  public const float timeBetweenShots = 0.15f;
  public float bombDuration = 0.75f;

  public override void Awake()
  {
    base.Awake();
    this.Preload();
  }

  public void Preload() => this.InitializeMortarStrikes();

  public void InitializeMortarStrikes()
  {
    int num = Mathf.Max(this.ShotsToFire);
    List<MortarBomb> mortarBombList = new List<MortarBomb>();
    for (int index = 0; index < num; ++index)
    {
      MortarBomb component = ObjectPool.Spawn(this.projectilePrefab, this.transform.parent).GetComponent<MortarBomb>();
      component.destroyOnFinish = false;
      mortarBombList.Add(component);
    }
    for (int index = 0; index < mortarBombList.Count; ++index)
      mortarBombList[index].gameObject.Recycle();
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.minTimeBetweenBurps = 2f;
    this.chargingAnimationString = "burp";
  }

  public override void UpdateStateIdle()
  {
    if ((Object) this.targetObject != (Object) null)
      this.isFleeing = (double) (this.targetObject.transform.position - this.transform.position).magnitude > (double) this.attackRange;
    base.UpdateStateIdle();
  }

  public override void UpdateStateCharging()
  {
    this.SimpleSpineFlash.FlashMeWhite();
    if ((double) this.gm.TimeSince(this.chargingTimestamp) < (double) this.chargingDuration / (double) this.Spine[0].timeScale)
      return;
    this.BurpFlies();
    if (this.ShouldStartCharging() && (double) Random.Range(0.0f, 1f) < 0.5)
      this.state.CURRENT_STATE = StateMachine.State.Charging;
    else
      this.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public override bool ShouldStartCharging()
  {
    return !((Object) this.targetObject == (Object) null) && base.ShouldStartCharging();
  }

  public override IEnumerator ShootProjectileRoutine()
  {
    EnemyHopperMortar enemyHopperMortar = this;
    if ((Object) enemyHopperMortar.targetObject == (Object) null)
      enemyHopperMortar.targetObject = enemyHopperMortar.GetClosestTarget()?.gameObject;
    if (!((Object) enemyHopperMortar.targetObject == (Object) null))
    {
      enemyHopperMortar.state.facingAngle = Utils.GetAngle(enemyHopperMortar.transform.position, enemyHopperMortar.targetObject.transform.position);
      Vector3 targetPosition = enemyHopperMortar.targetObject.transform.position;
      for (int i = 0; i < enemyHopperMortar.ShotsToFire && !((Object) enemyHopperMortar.targetObject == (Object) null); ++i)
      {
        if (enemyHopperMortar.ShotsToFire > 1)
          targetPosition = enemyHopperMortar.targetObject.transform.position + (Vector3) Random.insideUnitCircle * 2f;
        MortarBomb component = ObjectPool.Spawn(enemyHopperMortar.projectilePrefab, enemyHopperMortar.transform.parent, enemyHopperMortar.targetObject.transform.position, Quaternion.identity).GetComponent<MortarBomb>();
        Vector3 vector3_1;
        if ((double) Vector2.Distance((Vector2) enemyHopperMortar.transform.position, (Vector2) targetPosition) < 2.5)
        {
          Transform transform = component.transform;
          Vector3 position = enemyHopperMortar.transform.position;
          vector3_1 = targetPosition - enemyHopperMortar.transform.position;
          Vector3 vector3_2 = vector3_1.normalized * 2.5f;
          Vector3 vector3_3 = position + vector3_2;
          transform.position = vector3_3;
        }
        else
        {
          Transform transform = component.transform;
          Vector3 position = enemyHopperMortar.transform.position;
          vector3_1 = targetPosition - enemyHopperMortar.transform.position;
          Vector3 vector3_4 = vector3_1.normalized * 5f;
          Vector3 vector3_5 = position + vector3_4;
          transform.position = vector3_5;
        }
        component.Play(enemyHopperMortar.transform.position + new Vector3(0.0f, 0.0f, -1.5f), enemyHopperMortar.bombDuration, Health.Team.Team2);
        enemyHopperMortar.SimpleSpineFlash.FlashWhite(false);
        float time = 0.0f;
        while ((double) (time += Time.deltaTime * enemyHopperMortar.Spine[0].timeScale) < 0.15000000596046448)
          yield return (object) null;
      }
    }
  }
}
