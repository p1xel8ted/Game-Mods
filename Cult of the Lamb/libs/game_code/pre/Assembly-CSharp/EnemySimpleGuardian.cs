// Decompiled with JetBrains decompiler
// Type: EnemySimpleGuardian
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMODUnity;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnemySimpleGuardian : UnitObject
{
  public bool CanDoRingShot = true;
  public bool CanDoBoomerangShot = true;
  [SerializeField]
  private bool requireLineOfSite = true;
  private GameObject TargetObject;
  private Health EnemyHealth;
  public ColliderEvents damageColliderEvents;
  public SkeletonAnimation Spine;
  private SimpleSpineFlash SimpleSpineFlash;
  private SimpleSpineEventListener simpleSpineEventListener;
  private List<Collider2D> collider2DList;
  private Collider2D HealthCollider;
  public ParticleSystem DashParticles;
  [EventRef]
  public string attackSoundPath = string.Empty;
  [EventRef]
  public string onHitSoundPath = string.Empty;
  public DeadBodySliding deadBodySliding;
  private static List<EnemySimpleGuardian> SimpleGuardians = new List<EnemySimpleGuardian>();
  public string WalkAnimation = "walk2";
  private float GlobalAttackDelay = 10f;
  public float GlobalRingAttackDelay = 30f;
  private Vector3 TargetPosition;
  [SerializeField]
  private ProjectileCircle projectilePatternRings;
  [SerializeField]
  private float projectilePatternRingsSpeed = 2.5f;
  [SerializeField]
  private float projectilePatternRingsRadius = 1f;
  [SerializeField]
  private float projectilePatternRingsAcceleration = 7.5f;
  [SerializeField]
  private GameObject BoomerangArrow;
  [SerializeField]
  private float BoomerangCount = 20f;
  [SerializeField]
  private float BoomerangSpeed = 5f;
  [SerializeField]
  private float BoomerangReturnSpeed = -1f;
  [SerializeField]
  private float OutwardDuration = 3f;
  [SerializeField]
  private float ReturnDuration = 2f;
  [SerializeField]
  private float PauseDuration = 0.5f;
  private List<Projectile> Boomeranges = new List<Projectile>();

  public override void OnEnable()
  {
    this.SeperateObject = true;
    base.OnEnable();
    EnemySimpleGuardian.SimpleGuardians.Add(this);
    this.SimpleSpineFlash = this.GetComponentInChildren<SimpleSpineFlash>();
    this.simpleSpineEventListener = this.GetComponent<SimpleSpineEventListener>();
    this.simpleSpineEventListener.OnSpineEvent += new SimpleSpineEventListener.SpineEvent(this.OnSpineEvent);
    this.HealthCollider = this.GetComponent<Collider2D>();
    this.DashParticles.Stop();
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.damageColliderEvents.SetActive(false);
    }
    this.StartCoroutine((IEnumerator) this.WaitForTarget());
  }

  public override void OnDisable()
  {
    base.OnDisable();
    EnemySimpleGuardian.SimpleGuardians.Remove(this);
    this.simpleSpineEventListener.OnSpineEvent -= new SimpleSpineEventListener.SpineEvent(this.OnSpineEvent);
    if (!((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null))
      return;
    this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
  }

  protected IEnumerator WaitForTarget()
  {
    EnemySimpleGuardian enemySimpleGuardian = this;
    enemySimpleGuardian.Spine.Initialize(false);
    while (!GameManager.RoomActive)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    while ((UnityEngine.Object) enemySimpleGuardian.TargetObject == (UnityEngine.Object) null)
    {
      Health closestTarget = enemySimpleGuardian.GetClosestTarget();
      if ((bool) (UnityEngine.Object) closestTarget)
      {
        enemySimpleGuardian.TargetObject = closestTarget.gameObject;
        enemySimpleGuardian.requireLineOfSite = false;
        enemySimpleGuardian.VisionRange = int.MaxValue;
      }
      yield return (object) null;
    }
    bool InRange = false;
    while (!InRange)
    {
      if ((UnityEngine.Object) enemySimpleGuardian.TargetObject == (UnityEngine.Object) null)
      {
        enemySimpleGuardian.StartCoroutine((IEnumerator) enemySimpleGuardian.WaitForTarget());
        yield break;
      }
      float a = Vector3.Distance(enemySimpleGuardian.TargetObject.transform.position, enemySimpleGuardian.transform.position);
      if ((double) a <= (double) enemySimpleGuardian.VisionRange)
      {
        if (!enemySimpleGuardian.requireLineOfSite || enemySimpleGuardian.CheckLineOfSight(enemySimpleGuardian.TargetObject.transform.position, Mathf.Min(a, (float) enemySimpleGuardian.VisionRange)))
          InRange = true;
        else
          enemySimpleGuardian.LookAtTarget();
      }
      yield return (object) null;
    }
    enemySimpleGuardian.StartCoroutine((IEnumerator) enemySimpleGuardian.FightPlayer());
  }

  private void LookAtTarget()
  {
    if ((UnityEngine.Object) this.GetClosestTarget() == (UnityEngine.Object) null)
      return;
    float angle = Utils.GetAngle(this.transform.position, this.GetClosestTarget().transform.position);
    this.state.facingAngle = angle;
    this.state.LookAngle = angle;
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/humanoid_large/gethit", this.transform.position);
    if (!string.IsNullOrEmpty(this.onHitSoundPath))
      AudioManager.Instance.PlayOneShot(this.onHitSoundPath, this.transform.position);
    CameraManager.shakeCamera(0.5f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
    this.SimpleSpineFlash.FlashFillRed();
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/humanoid_large/death", this.transform.position);
    AudioManager.Instance.SetMusicRoomID(0, "deathcat_room_id");
    foreach (Projectile boomerange in this.Boomeranges)
      boomerange.DestroyProjectile();
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    this.SimpleSpineFlash.FlashWhite(false);
    this.StopAllCoroutines();
  }

  private void OnSpineEvent(string EventName)
  {
    switch (EventName)
    {
      case "Invincible On":
        this.health.invincible = true;
        this.HealthCollider.enabled = false;
        break;
      case "Invincible Off":
        this.health.invincible = false;
        this.HealthCollider.enabled = true;
        break;
    }
  }

  private void GetPath()
  {
    if (EnemySimpleGuardian.SimpleGuardians.Count > 1)
    {
      Debug.Log((object) "CHECK! ");
      float num1 = float.MaxValue;
      EnemySimpleGuardian enemySimpleGuardian = (EnemySimpleGuardian) null;
      foreach (EnemySimpleGuardian simpleGuardian in EnemySimpleGuardian.SimpleGuardians)
      {
        float num2 = Vector3.Distance(simpleGuardian.transform.position, this.TargetObject.transform.position);
        if ((double) num2 < (double) num1)
        {
          num1 = num2;
          enemySimpleGuardian = simpleGuardian;
        }
      }
      this.TargetPosition = !((UnityEngine.Object) enemySimpleGuardian == (UnityEngine.Object) this) ? Vector3.zero : this.TargetObject.transform.position;
    }
    else
      this.TargetPosition = this.TargetObject.transform.position;
    if ((double) Vector3.Distance(this.TargetPosition, this.transform.position) > (double) this.StoppingDistance)
    {
      this.givePath(this.TargetPosition);
      if (!(this.Spine.AnimationName != this.WalkAnimation))
        return;
      this.Spine.AnimationState.SetAnimation(0, this.WalkAnimation, true);
    }
    else
    {
      if (!(this.Spine.AnimationName != "idle"))
        return;
      this.Spine.AnimationState.SetAnimation(0, "idle", true);
    }
  }

  private IEnumerator FightPlayer()
  {
    EnemySimpleGuardian enemySimpleGuardian = this;
    while (EnemySimpleGuardian.SimpleGuardians.Count <= 1 && ((UnityEngine.Object) enemySimpleGuardian.TargetObject == (UnityEngine.Object) null || (double) Vector3.Distance(enemySimpleGuardian.TargetObject.transform.position, enemySimpleGuardian.transform.position) > 12.0))
    {
      if ((UnityEngine.Object) enemySimpleGuardian.TargetObject != (UnityEngine.Object) null)
        enemySimpleGuardian.LookAtTarget();
      yield return (object) null;
    }
    enemySimpleGuardian.GetPath();
    float RepathTimer = 0.0f;
    int NumAttacks = 2;
    float AttackSpeed = 15f;
    bool Loop = true;
    while (Loop)
    {
      switch (enemySimpleGuardian.state.CURRENT_STATE)
      {
        case StateMachine.State.Idle:
        case StateMachine.State.Moving:
          enemySimpleGuardian.LookAtTarget();
          if ((double) Vector2.Distance((Vector2) enemySimpleGuardian.transform.position, (Vector2) enemySimpleGuardian.TargetObject.transform.position) < 3.0)
          {
            if ((double) TimeManager.TotalElapsedGameTime > (double) DataManager.Instance.LastSimpleGuardianAttacked + (double) enemySimpleGuardian.GlobalAttackDelay)
            {
              DataManager.Instance.LastSimpleGuardianAttacked = TimeManager.TotalElapsedGameTime;
              enemySimpleGuardian.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
              enemySimpleGuardian.Spine.AnimationState.SetAnimation(0, "attack" + (object) (4 - NumAttacks), false);
              enemySimpleGuardian.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
            }
            else if (enemySimpleGuardian.state.CURRENT_STATE != StateMachine.State.Idle)
            {
              enemySimpleGuardian.state.CURRENT_STATE = StateMachine.State.Idle;
              enemySimpleGuardian.Spine.AnimationState.SetAnimation(0, "idle", false);
            }
          }
          if (enemySimpleGuardian.CanDoRingShot && (double) Vector2.Distance((Vector2) enemySimpleGuardian.transform.position, (Vector2) enemySimpleGuardian.TargetObject.transform.position) >= 5.0 && (double) TimeManager.TotalElapsedGameTime > (double) DataManager.Instance.LastSimpleGuardianRingProjectiles + (double) enemySimpleGuardian.GlobalRingAttackDelay)
          {
            DataManager.Instance.LastSimpleGuardianRingProjectiles = TimeManager.TotalElapsedGameTime;
            enemySimpleGuardian.ProjectileRings();
            yield break;
          }
          if (enemySimpleGuardian.CanDoBoomerangShot && ((double) Vector2.Distance((Vector2) enemySimpleGuardian.transform.position, (Vector2) enemySimpleGuardian.TargetObject.transform.position) >= 5.0 || (double) Vector2.Distance((Vector2) enemySimpleGuardian.transform.position, (Vector2) enemySimpleGuardian.TargetObject.transform.position) < 2.0) && (double) TimeManager.TotalElapsedGameTime > (double) DataManager.Instance.LastSimpleGuardianRingProjectiles + (double) enemySimpleGuardian.GlobalRingAttackDelay)
          {
            DataManager.Instance.LastSimpleGuardianRingProjectiles = TimeManager.TotalElapsedGameTime;
            enemySimpleGuardian.ProjectileBoomerangs();
            yield break;
          }
          if ((double) Vector2.Distance((Vector2) enemySimpleGuardian.transform.position, (Vector2) enemySimpleGuardian.TargetObject.transform.position) >= 3.0 && (double) (RepathTimer += Time.deltaTime) > 0.20000000298023224)
          {
            RepathTimer = 0.0f;
            enemySimpleGuardian.GetPath();
          }
          if ((UnityEngine.Object) enemySimpleGuardian.damageColliderEvents != (UnityEngine.Object) null)
          {
            enemySimpleGuardian.damageColliderEvents.SetActive(false);
            break;
          }
          break;
        case StateMachine.State.SignPostAttack:
          enemySimpleGuardian.state.facingAngle = Utils.GetAngle(enemySimpleGuardian.transform.position, enemySimpleGuardian.TargetPosition);
          if ((double) (enemySimpleGuardian.state.Timer += Time.deltaTime) >= 0.5)
          {
            enemySimpleGuardian.SimpleSpineFlash.FlashWhite(false);
            enemySimpleGuardian.DashParticles.Play();
            CameraManager.shakeCamera(0.4f, enemySimpleGuardian.state.facingAngle);
            enemySimpleGuardian.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
            enemySimpleGuardian.speed = AttackSpeed * Time.deltaTime;
            if ((UnityEngine.Object) enemySimpleGuardian.damageColliderEvents != (UnityEngine.Object) null)
              enemySimpleGuardian.damageColliderEvents.SetActive(true);
            if (!string.IsNullOrEmpty(enemySimpleGuardian.attackSoundPath))
              AudioManager.Instance.PlayOneShot(enemySimpleGuardian.attackSoundPath, enemySimpleGuardian.transform.position);
            AudioManager.Instance.PlayOneShot("event:/enemy/vocals/humanoid_large/warning", enemySimpleGuardian.transform.position);
            break;
          }
          enemySimpleGuardian.SimpleSpineFlash.FlashWhite(enemySimpleGuardian.state.Timer / 0.5f);
          break;
        case StateMachine.State.RecoverFromAttack:
          if ((double) AttackSpeed > 0.0)
            AttackSpeed -= 1f * GameManager.DeltaTime;
          enemySimpleGuardian.speed = AttackSpeed * Time.deltaTime;
          if ((double) enemySimpleGuardian.state.Timer >= 0.25 && (UnityEngine.Object) enemySimpleGuardian.damageColliderEvents != (UnityEngine.Object) null)
            enemySimpleGuardian.damageColliderEvents.SetActive(false);
          if ((double) (enemySimpleGuardian.state.Timer += Time.deltaTime) >= 0.5)
          {
            enemySimpleGuardian.DashParticles.Stop();
            AudioManager.Instance.PlayOneShot("event:/enemy/vocals/humanoid_large/attack", enemySimpleGuardian.transform.position);
            if (--NumAttacks > 0)
            {
              AttackSpeed = (float) (15 + (3 - NumAttacks) * 2);
              enemySimpleGuardian.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
              enemySimpleGuardian.Spine.AnimationState.SetAnimation(0, "attack" + (object) (4 - NumAttacks), false);
              enemySimpleGuardian.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
              break;
            }
            Loop = false;
            enemySimpleGuardian.state.CURRENT_STATE = StateMachine.State.Idle;
            enemySimpleGuardian.Spine.AnimationState.SetAnimation(0, "idle", true);
            break;
          }
          break;
      }
      yield return (object) null;
    }
    yield return (object) new WaitForSeconds(0.5f);
    enemySimpleGuardian.state.CURRENT_STATE = StateMachine.State.Idle;
    enemySimpleGuardian.Spine.AnimationState.SetAnimation(0, "idle", true);
    if ((UnityEngine.Object) enemySimpleGuardian.TargetObject != (UnityEngine.Object) null && (double) Vector3.Distance(enemySimpleGuardian.TargetObject.transform.position, enemySimpleGuardian.transform.position) > 5.0)
    {
      enemySimpleGuardian.LookAtTarget();
      yield return (object) new WaitForSeconds(1f);
    }
    enemySimpleGuardian.StartCoroutine((IEnumerator) enemySimpleGuardian.FightPlayer());
  }

  private void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.health.team)
      return;
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }

  private void ProjectileRings()
  {
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.ProjectileRingsRoutine());
  }

  private IEnumerator ProjectileRingsRoutine()
  {
    EnemySimpleGuardian enemySimpleGuardian = this;
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/humanoid_large/warning", enemySimpleGuardian.transform.position);
    enemySimpleGuardian.state.CURRENT_STATE = StateMachine.State.Idle;
    yield return (object) null;
    enemySimpleGuardian.Spine.AnimationState.SetAnimation(0, "summon", false);
    enemySimpleGuardian.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    yield return (object) new WaitForSeconds(1f);
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/humanoid_large/attack", enemySimpleGuardian.transform.position);
    Projectile arrow = UnityEngine.Object.Instantiate<ProjectileCircle>(enemySimpleGuardian.projectilePatternRings, enemySimpleGuardian.transform.parent).GetComponent<Projectile>();
    arrow.transform.position = enemySimpleGuardian.transform.position;
    arrow.health = enemySimpleGuardian.health;
    arrow.team = Health.Team.Team2;
    arrow.Speed = enemySimpleGuardian.projectilePatternRingsSpeed;
    arrow.Acceleration = enemySimpleGuardian.projectilePatternRingsAcceleration;
    arrow.GetComponent<ProjectileCircle>().InitDelayed(PlayerFarming.Instance.gameObject, enemySimpleGuardian.projectilePatternRingsRadius, 0.0f, (System.Action) (() =>
    {
      CameraManager.instance.ShakeCameraForDuration(0.8f, 0.9f, 0.3f, false);
      AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/grenade_mass_launch", this.gameObject);
      arrow.Angle = Mathf.Round(arrow.Angle / 45f) * 45f;
    }));
    yield return (object) new WaitForSeconds(1.56666672f);
    enemySimpleGuardian.state.CURRENT_STATE = StateMachine.State.Idle;
    enemySimpleGuardian.Spine.AnimationState.SetAnimation(0, "idle", true);
    enemySimpleGuardian.StartCoroutine((IEnumerator) enemySimpleGuardian.FightPlayer());
  }

  private void ProjectileBoomerangs()
  {
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.ProjectileBoomerangsRoutine());
  }

  private IEnumerator ProjectileBoomerangsRoutine()
  {
    EnemySimpleGuardian enemySimpleGuardian = this;
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/humanoid_large/warning", enemySimpleGuardian.transform.position);
    enemySimpleGuardian.state.CURRENT_STATE = StateMachine.State.Idle;
    yield return (object) null;
    enemySimpleGuardian.Spine.AnimationState.SetAnimation(0, "projectiles-start", false);
    enemySimpleGuardian.Spine.AnimationState.AddAnimation(0, "projectiles-loop", true, 0.0f);
    yield return (object) new WaitForSeconds(1.76666665f);
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/humanoid_large/attack", enemySimpleGuardian.transform.position);
    CameraManager.instance.ShakeCameraForDuration(0.8f, 0.9f, 0.3f, false);
    enemySimpleGuardian.Boomeranges = new List<Projectile>();
    float boomerangCount = enemySimpleGuardian.BoomerangCount;
    for (float num = 0.0f; (double) num < (double) boomerangCount; ++num)
    {
      Projectile component = UnityEngine.Object.Instantiate<GameObject>(enemySimpleGuardian.BoomerangArrow, enemySimpleGuardian.transform.parent).GetComponent<Projectile>();
      component.transform.position = enemySimpleGuardian.transform.position;
      component.team = Health.Team.Team2;
      component.Speed = enemySimpleGuardian.BoomerangSpeed;
      component.Angle = 360f / boomerangCount * num;
      component.LifeTime = 30f;
      component.IgnoreIsland = true;
      component.Trail.time = 0.3f;
      enemySimpleGuardian.Boomeranges.Add(component);
    }
    AudioManager.Instance.PlayOneShot("event:/enemy/shoot_magicenergy", enemySimpleGuardian.transform.position);
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/grenade_mass_launch", enemySimpleGuardian.gameObject);
    yield return (object) new WaitForSeconds(enemySimpleGuardian.OutwardDuration);
    foreach (Projectile boomerange in enemySimpleGuardian.Boomeranges)
    {
      Projectile a = boomerange;
      if ((UnityEngine.Object) a != (UnityEngine.Object) null)
        DOTween.To((DOGetter<float>) (() => a.SpeedMultiplier), (DOSetter<float>) (x => a.SpeedMultiplier = x), 0.0f, 0.2f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutSine);
    }
    yield return (object) new WaitForSeconds(enemySimpleGuardian.PauseDuration);
    foreach (Projectile boomerange in enemySimpleGuardian.Boomeranges)
    {
      Projectile a = boomerange;
      if ((UnityEngine.Object) a != (UnityEngine.Object) null)
        DOTween.To((DOGetter<float>) (() => a.SpeedMultiplier), (DOSetter<float>) (x => a.SpeedMultiplier = x), enemySimpleGuardian.BoomerangReturnSpeed, 0.3f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutBack);
    }
    AudioManager.Instance.PlayOneShot("event:/enemy/shoot_magicenergy", enemySimpleGuardian.transform.position);
    yield return (object) new WaitForSeconds(enemySimpleGuardian.ReturnDuration);
    bool flag = true;
    foreach (Projectile boomerange in enemySimpleGuardian.Boomeranges)
    {
      if ((UnityEngine.Object) boomerange != (UnityEngine.Object) null)
      {
        boomerange.EndOfLife();
        if (flag)
        {
          boomerange.EndOfLife();
          flag = false;
        }
        else
          boomerange.DestroyProjectile();
      }
    }
    enemySimpleGuardian.Boomeranges.Clear();
    enemySimpleGuardian.Spine.AnimationState.SetAnimation(0, "projectiles-stop", true);
    enemySimpleGuardian.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    yield return (object) new WaitForSeconds(0.7f);
    enemySimpleGuardian.state.CURRENT_STATE = StateMachine.State.Idle;
    enemySimpleGuardian.StartCoroutine((IEnumerator) enemySimpleGuardian.FightPlayer());
  }
}
