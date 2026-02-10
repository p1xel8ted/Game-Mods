// Decompiled with JetBrains decompiler
// Type: EnemyLavaHopperAOE
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class EnemyLavaHopperAOE : EnemyLavaHopper
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
  public float RandomFaceDirChance = 0.4f;

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
    if (this.canLayEggs && (double) this.gm.TimeSince(this.lastLaidEggTimestamp - 0.5f) >= (double) this.minTimeBetweenEggs * (double) this.Spine[0].timeScale && EnemyLavaHopper.EnemyHoppers.Count < EnemyLavaHopper.maxHoppersPerRoom && EnemyEgg.EnemyEggs.Count < EnemyLavaHopper.maxEggsPerRoom)
    {
      this.alwaysTargetPlayer = false;
      this.isFleeing = true;
    }
    if ((Object) this.targetObject != (Object) null)
    {
      if (this.seekingRegenPoint)
        this.TargetAngle = this.SeekRegenPointBehavior();
      else if ((double) Vector3.Distance(this.transform.position, this.targetObject.transform.position) < 2.0)
        this.TargetAngle = this.GetFleeAngle();
      else if ((double) Random.Range(0.0f, 1f) > (double) this.RandomFaceDirChance)
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
      if (this.ShouldSprayPoisonOnLand() && !this.seekingRegenPoint)
        this.SprayPoison();
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
    EnemyLavaHopperAOE enemyLavaHopperAoe = this;
    enemyLavaHopperAoe.shooting = true;
    while (PlayerRelic.TimeFrozen)
      yield return (object) null;
    yield return (object) new WaitForSeconds(0.5f);
    enemyLavaHopperAoe.state.CURRENT_STATE = StateMachine.State.Aiming;
    foreach (SkeletonAnimation skeletonAnimation in enemyLavaHopperAoe.Spine)
    {
      if (skeletonAnimation.AnimationState != null)
      {
        skeletonAnimation.AnimationState.SetAnimation(0, "burp", false);
        skeletonAnimation.AnimationState.AddAnimation(0, "idle", true, 0.0f);
      }
    }
    float t = 0.0f;
    while ((double) t < (double) enemyLavaHopperAoe.shootAnticipation)
    {
      t += Time.deltaTime;
      enemyLavaHopperAoe.SimpleSpineFlash.FlashWhite((float) ((double) t / (double) enemyLavaHopperAoe.shootAnticipation * 0.75));
      yield return (object) null;
    }
    enemyLavaHopperAoe.SimpleSpineFlash.FlashWhite(false);
    for (int i = 0; i < enemyLavaHopperAoe.amountToShoot; ++i)
    {
      while (PlayerRelic.TimeFrozen)
        yield return (object) null;
      Vector3 vector3 = (Vector3) (Random.insideUnitCircle * enemyLavaHopperAoe.shootRadius);
      Projectile component = ObjectPool.Spawn(enemyLavaHopperAoe.bulletPrefab, enemyLavaHopperAoe.transform.parent).GetComponent<Projectile>();
      component.transform.position = enemyLavaHopperAoe.transform.position;
      component.Angle = (Object) enemyLavaHopperAoe.GetClosestTarget() != (Object) null ? Utils.GetAngle(enemyLavaHopperAoe.transform.position, enemyLavaHopperAoe.GetClosestTarget().transform.position + vector3) : Random.value * 360f;
      component.team = enemyLavaHopperAoe.health.team;
      component.Speed = enemyLavaHopperAoe.bulletSpeed + Random.Range(-enemyLavaHopperAoe.bulletSpeedRandomness, enemyLavaHopperAoe.bulletSpeedRandomness);
      component.LifeTime = 4f + Random.Range(0.0f, 0.3f);
      component.Owner = enemyLavaHopperAoe.health;
      if (enemyLavaHopperAoe.timeBetweenBullets != Vector2.zero)
        yield return (object) new WaitForSeconds(Random.Range(enemyLavaHopperAoe.timeBetweenBullets.x, enemyLavaHopperAoe.timeBetweenBullets.y));
    }
    enemyLavaHopperAoe.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyLavaHopperAoe.shooting = false;
    if ((double) Random.Range(0.0f, 1f) < (double) enemyLavaHopperAoe.shootAgainChance && !enemyLavaHopperAoe.shootAgain)
    {
      enemyLavaHopperAoe.shootAgain = true;
      enemyLavaHopperAoe.idleDur = 0.5f;
    }
    else
    {
      enemyLavaHopperAoe.shootAgain = false;
      enemyLavaHopperAoe.idleDur = enemyLavaHopperAoe.signPostParryWindow;
    }
  }

  public void RageHop() => this.StartCoroutine((IEnumerator) this.RageHopIE());

  public IEnumerator RageHopIE()
  {
    EnemyLavaHopperAOE enemyLavaHopperAoe = this;
    enemyLavaHopperAoe.shooting = true;
    float t = 0.0f;
    while ((double) t < (double) enemyLavaHopperAoe.rageHopAnticipation)
    {
      t += Time.deltaTime;
      enemyLavaHopperAoe.SimpleSpineFlash.FlashWhite((float) ((double) t / (double) enemyLavaHopperAoe.rageHopAnticipation * 0.75));
      yield return (object) null;
    }
    enemyLavaHopperAoe.SimpleSpineFlash.FlashWhite(false);
    foreach (SkeletonAnimation skeletonAnimation in enemyLavaHopperAoe.Spine)
      skeletonAnimation.AnimationState.SetAnimation(0, "angry", false);
    yield return (object) new WaitForSeconds(enemyLavaHopperAoe.rageHopAnticipation);
    float a = 0.0f;
    for (int i = 0; (double) i < (double) Random.Range(enemyLavaHopperAoe.rageHopAmount.x, enemyLavaHopperAoe.rageHopAmount.y + 1f); ++i)
    {
      foreach (SkeletonAnimation skeletonAnimation in enemyLavaHopperAoe.Spine)
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
      BiomeConstants.Instance.EmitSmokeExplosionVFX(enemyLavaHopperAoe.transform.position + Vector3.back * 0.5f).transform.localScale = Vector3.one * 0.5f;
      Projectile.CreateProjectiles(enemyLavaHopperAoe.rageHopBulletAmount, enemyLavaHopperAoe.health, enemyLavaHopperAoe.transform.position, enemyLavaHopperAoe.rageHopBulletSpeed, angleOffset: a += 45f);
      yield return (object) new WaitForSeconds(0.4f);
    }
    enemyLavaHopperAoe.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyLavaHopperAoe.shooting = false;
  }

  public override void DoAttack()
  {
    if (this.jumpingToRegenPoint)
      return;
    base.DoAttack();
    if ((Object) this.aoeParticles != (Object) null)
      this.aoeParticles.Play();
    if (this.WillSeekRegenPointsAtLowHealth && this.seekingRegenPoint)
    {
      if ((Object) this.targetRegenPoint == (Object) null)
      {
        this.seekingRegenPoint = false;
      }
      else
      {
        double num = (double) Vector3.Distance(this.transform.position, new Vector3(this.targetRegenPoint.transform.position.x, this.targetRegenPoint.transform.position.y, this.transform.position.z));
        if (num < 8.0)
          this.ModifyExcludeLayers(this.GetComponent<CircleCollider2D>(), "Island", true);
        if (num < 6.0)
        {
          this.jumpingToRegenPoint = true;
          this.StartCoroutine((IEnumerator) this.JumpToRegenPoint(this.targetRegenPoint));
        }
      }
    }
    if (!this.EmitSmokeVfxOnLand)
      return;
    BiomeConstants.Instance.EmitSmokeExplosionVFX(this.transform.position + Vector3.back * 0.5f);
  }
}
