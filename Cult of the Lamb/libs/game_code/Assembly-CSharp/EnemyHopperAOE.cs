// Decompiled with JetBrains decompiler
// Type: EnemyHopperAOE
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class EnemyHopperAOE : EnemyHopper
{
  [SerializeField]
  public bool shoot;
  [SerializeField]
  public GameObject bulletPrefab;
  [SerializeField]
  public float shootAnticipation;
  [SerializeField]
  public float bulletSpeed;
  [SerializeField]
  public float bulletSpeedRandomness;
  [SerializeField]
  public Vector2 timeBetweenBullets;
  [SerializeField]
  public int amountToShoot;
  [SerializeField]
  public float shootRadius;
  [SerializeField]
  public float shootChance = 0.3f;
  [SerializeField]
  public float shootAgainChance = 0.3f;
  [SerializeField]
  public bool doRageHop;
  [SerializeField]
  public float rageHopChance;
  [SerializeField]
  public float rageHopBulletSpeed;
  [SerializeField]
  public float rageHopAnticipation;
  [SerializeField]
  public int rageHopBulletAmount;
  [SerializeField]
  public Vector2 rageHopAmount;
  public bool shooting;
  public ParticleSystem aoeParticles;
  public bool shootAgain;

  public override void Awake()
  {
    base.Awake();
    this.bulletPrefab.CreatePool(this.amountToShoot);
  }

  public override void UpdateStateIdle()
  {
    this.speed = 0.0f;
    if (PlayerRelic.TimeFrozen)
    {
      this.idleTimestamp += Time.deltaTime;
      this.lastLaidEggTimestamp += Time.deltaTime;
    }
    if ((double) this.gm.TimeSince(this.idleTimestamp) >= ((double) this.idleDur - (double) this.signPostParryWindow) * (double) this.Spine[0].timeScale)
      this.canBeParried = true;
    double idleDur = (double) this.idleDur;
    double signPostDur = (double) this.signPostDur;
    if ((double) this.gm.TimeSince(this.idleTimestamp) < (double) this.idleDur * (double) this.Spine[0].timeScale || this.shooting)
      return;
    if ((Object) this.targetObject == (Object) null && (Object) this.GetClosestTarget() != (Object) null)
      this.targetObject = this.GetClosestTarget().gameObject;
    this.TargetIsVisible();
    if (this.canLayEggs && (double) this.gm.TimeSince(this.lastLaidEggTimestamp - 0.5f) >= (double) this.minTimeBetweenEggs * (double) this.Spine[0].timeScale && EnemyHopper.EnemyHoppers.Count < EnemyHopper.maxHoppersPerRoom && EnemyEgg.EnemyEggs.Count < EnemyHopper.maxEggsPerRoom)
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
    if (this.shootAgain || (double) Random.Range(0.0f, 1f) < (double) this.shootChance)
      this.Shoot();
    else if ((double) Random.Range(0.0f, 1f) < (double) this.rageHopChance && this.doRageHop)
    {
      this.RageHop();
    }
    else
    {
      this.state.CURRENT_STATE = StateMachine.State.Moving;
      this.idleDur = this.signPostParryWindow;
    }
  }

  public override void UpdateStateMoving()
  {
    if (!this._playedVO)
    {
      AudioManager.Instance.PlayOneShot(this.WarningVO, this.gameObject);
      this._playedVO = true;
    }
    if (PlayerRelic.TimeFrozen)
      this.hoppingTimestamp += Time.deltaTime;
    this.speed = this.hopSpeedCurve.Evaluate(this.gm.TimeSince(this.hoppingTimestamp) / this.hoppingDur) * this.hopMoveSpeed * this.Spine[0].timeScale;
    this.Spine[0].transform.localPosition = -Vector3.forward * this.hopZCurve.Evaluate(this.gm.TimeSince(this.hoppingTimestamp) / this.hoppingDur) * this.hopZHeight;
    if ((double) this.gm.TimeSince(this.hoppingTimestamp) / (double) this.hoppingDur > 0.10000000149011612 && (double) this.gm.TimeSince(this.hoppingTimestamp) / (double) this.hoppingDur < 0.89999997615814209)
      this.health.untouchable = true;
    else
      this.health.untouchable = false;
    this.canBeParried = false;
    this.SimpleSpineFlash.FlashWhite(1f - Mathf.Clamp01(this.gm.TimeSince(this.hoppingTimestamp) / (this.attackingDur * 0.5f * this.Spine[0].timeScale)));
    if ((double) this.gm.TimeSince(this.hoppingTimestamp) < (double) this.hoppingDur / (double) this.Spine[0].timeScale)
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

  public override void OnEnable()
  {
    base.OnEnable();
    this.shooting = false;
  }

  public override bool ShouldStartCharging()
  {
    return base.ShouldStartCharging() && this.state.CURRENT_STATE != StateMachine.State.Aiming;
  }

  public void Shoot() => this.StartCoroutine((IEnumerator) this.ShootIE());

  public IEnumerator ShootIE()
  {
    EnemyHopperAOE enemyHopperAoe = this;
    enemyHopperAoe.shooting = true;
    while (PlayerRelic.TimeFrozen)
      yield return (object) null;
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
    for (int i = 0; i < enemyHopperAoe.amountToShoot; ++i)
    {
      while (PlayerRelic.TimeFrozen)
        yield return (object) null;
      Vector3 vector3 = (Vector3) (Random.insideUnitCircle * enemyHopperAoe.shootRadius);
      Projectile component = ObjectPool.Spawn(enemyHopperAoe.bulletPrefab, enemyHopperAoe.transform.parent).GetComponent<Projectile>();
      component.transform.position = enemyHopperAoe.transform.position;
      component.Angle = (Object) enemyHopperAoe.GetClosestTarget() != (Object) null ? Utils.GetAngle(enemyHopperAoe.transform.position, enemyHopperAoe.GetClosestTarget().transform.position + vector3) : Random.value * 360f;
      component.team = enemyHopperAoe.health.team;
      component.Speed = enemyHopperAoe.bulletSpeed + Random.Range(-enemyHopperAoe.bulletSpeedRandomness, enemyHopperAoe.bulletSpeedRandomness);
      component.LifeTime = 4f + Random.Range(0.0f, 0.3f);
      component.Owner = enemyHopperAoe.health;
      if (enemyHopperAoe.timeBetweenBullets != Vector2.zero)
        yield return (object) new WaitForSeconds(Random.Range(enemyHopperAoe.timeBetweenBullets.x, enemyHopperAoe.timeBetweenBullets.y));
    }
    enemyHopperAoe.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyHopperAoe.shooting = false;
    if ((double) Random.Range(0.0f, 1f) < (double) enemyHopperAoe.shootAgainChance && !enemyHopperAoe.shootAgain)
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

  public void RageHop() => this.StartCoroutine((IEnumerator) this.RageHopIE());

  public IEnumerator RageHopIE()
  {
    EnemyHopperAOE enemyHopperAoe = this;
    enemyHopperAoe.shooting = true;
    float t = 0.0f;
    while ((double) t < (double) enemyHopperAoe.rageHopAnticipation)
    {
      t += Time.deltaTime;
      enemyHopperAoe.SimpleSpineFlash.FlashWhite((float) ((double) t / (double) enemyHopperAoe.rageHopAnticipation * 0.75));
      yield return (object) null;
    }
    enemyHopperAoe.SimpleSpineFlash.FlashWhite(false);
    foreach (SkeletonAnimation skeletonAnimation in enemyHopperAoe.Spine)
      skeletonAnimation.AnimationState.SetAnimation(0, "angry", false);
    yield return (object) new WaitForSeconds(enemyHopperAoe.rageHopAnticipation);
    float a = 0.0f;
    for (int i = 0; (double) i < (double) Random.Range(enemyHopperAoe.rageHopAmount.x, enemyHopperAoe.rageHopAmount.y + 1f); ++i)
    {
      foreach (SkeletonAnimation skeletonAnimation in enemyHopperAoe.Spine)
      {
        if (skeletonAnimation.AnimationState != null)
        {
          skeletonAnimation.AnimationState.SetAnimation(0, "jumpcombined", false);
          skeletonAnimation.AnimationState.AddAnimation(0, "idle", true, 0.0f);
        }
      }
      while (PlayerRelic.TimeFrozen)
        yield return (object) null;
      yield return (object) new WaitForSeconds(0.5f);
      CameraManager.instance.ShakeCameraForDuration(1f, 2f, 0.25f);
      BiomeConstants.Instance.EmitSmokeExplosionVFX(enemyHopperAoe.transform.position + Vector3.back * 0.5f);
      Projectile.CreateProjectiles(enemyHopperAoe.rageHopBulletAmount, enemyHopperAoe.health, enemyHopperAoe.transform.position, enemyHopperAoe.rageHopBulletSpeed, angleOffset: a += 45f);
      yield return (object) new WaitForSeconds(0.4f);
    }
    enemyHopperAoe.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyHopperAoe.shooting = false;
  }

  public override void DoAttack()
  {
    base.DoAttack();
    if ((Object) this.aoeParticles != (Object) null)
      this.aoeParticles.Play();
    BiomeConstants.Instance.EmitSmokeExplosionVFX(this.transform.position + Vector3.back * 0.5f);
    CameraManager.shakeCamera(2f);
  }
}
