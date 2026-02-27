// Decompiled with JetBrains decompiler
// Type: EnemyHopperMortar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class EnemyHopperMortar : EnemyHopperBurp
{
  protected const float aimDuration = 1f;
  protected const float minBombRange = 2.5f;
  protected const float maxBombRange = 5f;
  protected const float timeBetweenShots = 0.15f;
  public float bombDuration = 0.75f;

  public override void OnEnable()
  {
    base.OnEnable();
    this.minTimeBetweenBurps = 2f;
    this.chargingAnimationString = "burp";
  }

  protected override void UpdateStateIdle()
  {
    if ((Object) this.targetObject != (Object) null)
      this.isFleeing = (double) (this.targetObject.transform.position - this.transform.position).magnitude > (double) this.attackRange;
    base.UpdateStateIdle();
  }

  protected override void UpdateStateCharging()
  {
    this.SimpleSpineFlash.FlashMeWhite();
    if ((double) this.gm.TimeSince(this.chargingTimestamp) < (double) this.chargingDuration)
      return;
    this.BurpFlies();
    if (this.ShouldStartCharging() && (double) Random.Range(0.0f, 1f) < 0.5)
      this.state.CURRENT_STATE = StateMachine.State.Charging;
    else
      this.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  protected override bool ShouldStartCharging()
  {
    return !((Object) this.targetObject == (Object) null) && base.ShouldStartCharging();
  }

  protected override IEnumerator ShootProjectileRoutine()
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
        MortarBomb component = Object.Instantiate<GameObject>(enemyHopperMortar.projectilePrefab, enemyHopperMortar.targetObject.transform.position, Quaternion.identity, enemyHopperMortar.transform.parent).GetComponent<MortarBomb>();
        if ((double) Vector2.Distance((Vector2) enemyHopperMortar.transform.position, (Vector2) targetPosition) < 2.5)
          component.transform.position = enemyHopperMortar.transform.position + (targetPosition - enemyHopperMortar.transform.position).normalized * 2.5f;
        else
          component.transform.position = enemyHopperMortar.transform.position + (targetPosition - enemyHopperMortar.transform.position).normalized * 5f;
        component.Play(enemyHopperMortar.transform.position + new Vector3(0.0f, 0.0f, -1.5f), enemyHopperMortar.bombDuration, Health.Team.Team2);
        enemyHopperMortar.SimpleSpineFlash.FlashWhite(false);
        yield return (object) new WaitForSeconds(0.15f);
      }
    }
  }
}
