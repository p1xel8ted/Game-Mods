// Decompiled with JetBrains decompiler
// Type: EnemyHopperAOE
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class EnemyHopperAOE : EnemyHopper
{
  [SerializeField]
  private bool shoot;
  [SerializeField]
  private GameObject bulletPrefab;
  [SerializeField]
  private float shootAnticipation;
  [SerializeField]
  private float bulletSpeed;
  [SerializeField]
  private int amountToShoot;
  [SerializeField]
  private float shootRadius;
  private bool shooting;
  public ParticleSystem aoeParticles;
  private bool shootAgain;

  protected override void UpdateStateIdle()
  {
    this.speed = 0.0f;
    if ((double) this.gm.TimeSince(this.idleTimestamp) >= (double) this.idleDur - (double) this.signPostParryWindow)
      this.canBeParried = true;
    double idleDur = (double) this.idleDur;
    double signPostDur = (double) this.signPostDur;
    if ((double) this.gm.TimeSince(this.idleTimestamp) < (double) this.idleDur || this.shooting)
      return;
    if ((Object) this.targetObject == (Object) null && (Object) this.GetClosestTarget() != (Object) null)
      this.targetObject = this.GetClosestTarget().gameObject;
    this.TargetIsVisible();
    if (this.canLayEggs && (double) this.gm.TimeSince(this.lastLaidEggTimestamp - 0.5f) >= (double) this.minTimeBetweenEggs && EnemyHopper.EnemyHoppers.Count < EnemyHopper.maxHoppersPerRoom && EnemyEgg.EnemyEggs.Count < EnemyHopper.maxEggsPerRoom)
    {
      this.alwaysTargetPlayer = false;
      this.isFleeing = true;
    }
    if ((Object) this.targetObject != (Object) null)
    {
      if ((double) Vector3.Distance(this.transform.position, this.targetObject.transform.position) < 2.0)
        this.TargetAngle = this.GetFleeAngle();
      else if ((double) Random.Range(0.0f, 1f) > 0.40000000596046448)
        this.TargetAngle = this.GetAngleToTarget();
      else
        this.TargetAngle = this.GetRandomFacingAngle();
    }
    this.state.LookAngle = this.TargetAngle;
    this.state.facingAngle = this.TargetAngle;
    foreach (SkeletonRenderer skeletonRenderer in this.Spine)
      skeletonRenderer.skeleton.ScaleX = (double) this.state.LookAngle <= 90.0 || (double) this.state.LookAngle >= 270.0 ? -1f : 1f;
    if (this.shootAgain || (double) Random.Range(0.0f, 1f) < 0.30000001192092896)
    {
      this.Shoot();
    }
    else
    {
      this.state.CURRENT_STATE = StateMachine.State.Moving;
      this.idleDur = this.signPostParryWindow;
    }
  }

  protected override void UpdateStateMoving()
  {
    if (!this._playedVO)
    {
      AudioManager.Instance.PlayOneShot(this.WarningVO, this.gameObject);
      this._playedVO = true;
    }
    this.speed = this.hopSpeedCurve.Evaluate(this.gm.TimeSince(this.hoppingTimestamp) / this.hoppingDur) * this.hopMoveSpeed;
    this.Spine[0].transform.localPosition = -Vector3.forward * this.hopZCurve.Evaluate(this.gm.TimeSince(this.hoppingTimestamp) / this.hoppingDur) * this.hopZHeight;
    if ((double) this.gm.TimeSince(this.hoppingTimestamp) / (double) this.hoppingDur > 0.10000000149011612 && (double) this.gm.TimeSince(this.hoppingTimestamp) / (double) this.hoppingDur < 0.89999997615814209)
      this.health.enabled = false;
    else if (!this.health.enabled)
      this.health.enabled = true;
    this.canBeParried = false;
    this.SimpleSpineFlash.FlashWhite(1f - Mathf.Clamp01(this.gm.TimeSince(this.hoppingTimestamp) / (this.attackingDur * 0.5f)));
    if ((double) this.gm.TimeSince(this.hoppingTimestamp) < (double) this.hoppingDur)
      return;
    this.speed = 0.0f;
    this.DoAttack();
    this._playedVO = false;
    if (this.ShouldStartCharging())
    {
      this.state.CURRENT_STATE = StateMachine.State.Charging;
      this.idleDur = this.signPostParryWindow;
    }
    else
    {
      this.state.CURRENT_STATE = StateMachine.State.Idle;
      this.idleDur = this.signPostParryWindow;
    }
  }

  protected override bool ShouldStartCharging()
  {
    return base.ShouldStartCharging() && this.state.CURRENT_STATE != StateMachine.State.Aiming;
  }

  private void Shoot() => this.StartCoroutine((IEnumerator) this.ShootIE());

  private IEnumerator ShootIE()
  {
    EnemyHopperAOE enemyHopperAoe = this;
    enemyHopperAoe.shooting = true;
    yield return (object) new WaitForSeconds(0.5f);
    enemyHopperAoe.state.CURRENT_STATE = StateMachine.State.Aiming;
    foreach (SkeletonAnimation skeletonAnimation in enemyHopperAoe.Spine)
    {
      if (skeletonAnimation.AnimationState != null)
      {
        skeletonAnimation.AnimationState.SetAnimation(0, "burp", false);
        skeletonAnimation.AnimationState.AddAnimation(0, "idle", true, 0.0f);
      }
    }
    float t = 0.0f;
    while ((double) t < (double) enemyHopperAoe.shootAnticipation)
    {
      t += Time.deltaTime;
      enemyHopperAoe.SimpleSpineFlash.FlashWhite((float) ((double) t / (double) enemyHopperAoe.shootAnticipation * 0.75));
      yield return (object) null;
    }
    enemyHopperAoe.SimpleSpineFlash.FlashWhite(false);
    for (int index = 0; index < enemyHopperAoe.amountToShoot; ++index)
    {
      Vector3 vector3 = (Vector3) (Random.insideUnitCircle * enemyHopperAoe.shootRadius);
      Projectile component = ObjectPool.Spawn(enemyHopperAoe.bulletPrefab, enemyHopperAoe.transform.parent).GetComponent<Projectile>();
      component.transform.position = enemyHopperAoe.transform.position + vector3;
      component.Angle = Utils.GetAngle(enemyHopperAoe.transform.position, PlayerFarming.Instance.transform.position + vector3);
      component.team = enemyHopperAoe.health.team;
      component.Speed = enemyHopperAoe.bulletSpeed;
      component.LifeTime = 4f + Random.Range(0.0f, 0.3f);
      component.Owner = enemyHopperAoe.health;
    }
    enemyHopperAoe.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyHopperAoe.shooting = false;
    if ((double) Random.Range(0.0f, 1f) > 0.75 && !enemyHopperAoe.shootAgain)
    {
      enemyHopperAoe.shootAgain = true;
      enemyHopperAoe.idleDur = 0.5f;
    }
    else
    {
      enemyHopperAoe.shootAgain = false;
      enemyHopperAoe.idleDur = enemyHopperAoe.signPostParryWindow;
    }
  }

  protected override void DoAttack()
  {
    base.DoAttack();
    if ((Object) this.aoeParticles != (Object) null)
      this.aoeParticles.Play();
    BiomeConstants.Instance.EmitSmokeExplosionVFX(this.transform.position + Vector3.back * 0.5f);
    CameraManager.shakeCamera(2f);
  }
}
