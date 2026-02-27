// Decompiled with JetBrains decompiler
// Type: EnemyFollowerPossessed
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMODUnity;
using MMBiomeGeneration;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class EnemyFollowerPossessed : UnitObject
{
  public static float LastSimpleGuardianRingProjectiles = float.MinValue;
  public bool CanDoRingShot = true;
  public bool CanDoBoomerangShot = true;
  [SerializeField]
  public bool requireLineOfSite = true;
  public GameObject TargetObject;
  public Health EnemyHealth;
  public ColliderEvents damageColliderEvents;
  public SkeletonAnimation Spine;
  public SimpleSpineFlash SimpleSpineFlash;
  public List<Collider2D> collider2DList;
  public Collider2D HealthCollider;
  public GameObject guardianGameObject;
  public ParticleSystem DashParticles;
  [EventRef]
  public string attackSoundPath = string.Empty;
  [EventRef]
  public string onHitSoundPath = string.Empty;
  public DeadBodySliding deadBodySliding;
  public static List<EnemySimpleGuardian> SimpleGuardians = new List<EnemySimpleGuardian>();
  public string WalkAnimation = "walk2";
  public Vector3 TargetPosition;
  [SerializeField]
  public ProjectileCircle projectilePatternRings;
  [SerializeField]
  public float projectilePatternRingsSpeed = 2.5f;
  [SerializeField]
  public float projectilePatternRingsRadius = 1f;
  [SerializeField]
  public float projectilePatternRingsAcceleration = 7.5f;
  [SerializeField]
  public GameObject BoomerangArrow;
  [SerializeField]
  public GameObject BouncingArrow;
  [SerializeField]
  public float BoomerangCount = 20f;
  [SerializeField]
  public float BoomerangSpeed = 5f;
  [SerializeField]
  public float BoomerangReturnSpeed = -1f;
  [SerializeField]
  public float OutwardDuration = 3f;
  [SerializeField]
  public float ReturnDuration = 2f;
  [SerializeField]
  public float PauseDuration = 0.5f;
  public List<Projectile> Boomeranges = new List<Projectile>();

  public override void OnEnable()
  {
    this.SeperateObject = true;
    base.OnEnable();
    this.SimpleSpineFlash = this.GetComponentInChildren<SimpleSpineFlash>();
    this.HealthCollider = this.GetComponent<Collider2D>();
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.damageColliderEvents.SetActive(false);
    }
    this.StartCoroutine(this.WaitForTarget());
    this.health.OnAddCharm += new Health.StasisEvent(this.ReconsiderTarget);
    this.health.OnStasisCleared += new Health.StasisEvent(this.ReconsiderTarget);
    EnemyFollowerPossessed.LastSimpleGuardianRingProjectiles = GameManager.GetInstance().CurrentTime;
  }

  public override void OnDisable()
  {
    base.OnDisable();
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
      this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    this.health.OnAddCharm -= new Health.StasisEvent(this.ReconsiderTarget);
    this.health.OnStasisCleared -= new Health.StasisEvent(this.ReconsiderTarget);
  }

  public override void Awake()
  {
    base.Awake();
    this.guardianGameObject = this.gameObject;
    DataManager.Instance.LastSimpleGuardianAttacked = GameManager.GetInstance().CurrentTime;
  }

  public IEnumerator WaitForTarget()
  {
    EnemyFollowerPossessed followerPossessed = this;
    followerPossessed.Spine.Initialize(false);
    while (!GameManager.RoomActive)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    while ((UnityEngine.Object) followerPossessed.TargetObject == (UnityEngine.Object) null)
    {
      followerPossessed.SetTargetObject();
      yield return (object) null;
    }
    bool InRange = false;
    while (!InRange)
    {
      if ((UnityEngine.Object) followerPossessed.TargetObject == (UnityEngine.Object) null)
      {
        followerPossessed.StartCoroutine(followerPossessed.WaitForTarget());
        yield break;
      }
      float a = Vector3.Distance(followerPossessed.TargetObject.transform.position, followerPossessed.transform.position);
      if ((double) a <= (double) followerPossessed.VisionRange)
      {
        if (!followerPossessed.requireLineOfSite || followerPossessed.CheckLineOfSightOnTarget(followerPossessed.TargetObject, followerPossessed.TargetObject.transform.position, Mathf.Min(a, (float) followerPossessed.VisionRange)))
          InRange = true;
        else
          followerPossessed.LookAtTarget();
      }
      yield return (object) null;
    }
    followerPossessed.StartCoroutine(followerPossessed.FightPlayer());
  }

  public void SetTargetObject()
  {
    Health closestTarget = this.GetClosestTarget();
    if (!(bool) (UnityEngine.Object) closestTarget)
      return;
    this.TargetObject = closestTarget.gameObject;
    this.requireLineOfSite = false;
    this.VisionRange = int.MaxValue;
  }

  public void ReconsiderTarget()
  {
    this.TargetObject = (GameObject) null;
    this.SetTargetObject();
  }

  public void LookAtTarget()
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
    if (!string.IsNullOrEmpty(this.onHitSoundPath))
      AudioManager.Instance.PlayOneShot(this.onHitSoundPath, this.transform.position);
    CameraManager.shakeCamera(0.5f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
    this.SimpleSpineFlash.FlashFillRed();
    this.DoKnockBack(Attacker, 1f, 0.33f);
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    AudioManager.Instance.SetMusicRoomID(0, "deathcat_room_id");
    foreach (Projectile boomerange in this.Boomeranges)
      boomerange.DestroyProjectile();
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    this.SimpleSpineFlash.FlashWhite(false);
    this.StopAllCoroutines();
  }

  public void GetPath()
  {
    if (EnemyFollowerPossessed.SimpleGuardians.Count > 1)
    {
      Debug.Log((object) "CHECK! ");
      float num1 = float.MaxValue;
      EnemySimpleGuardian enemySimpleGuardian = (EnemySimpleGuardian) null;
      foreach (EnemySimpleGuardian simpleGuardian in EnemyFollowerPossessed.SimpleGuardians)
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

  public float GlobalRingAttackDelay
  {
    get
    {
      if (!((UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null))
        return 3f;
      if (BiomeGenerator.Instance.PossessedEnemyEncounterCount == 0)
        return 4f;
      return BiomeGenerator.Instance.PossessedEnemyEncounterCount == 1 ? 3.75f : 3.5f;
    }
  }

  public IEnumerator FightPlayer()
  {
    EnemyFollowerPossessed followerPossessed = this;
    while (EnemyFollowerPossessed.SimpleGuardians.Count <= 1 && ((UnityEngine.Object) followerPossessed.TargetObject == (UnityEngine.Object) null || (double) Vector3.Distance(followerPossessed.TargetObject.transform.position, followerPossessed.transform.position) > 12.0))
    {
      if ((UnityEngine.Object) followerPossessed.TargetObject != (UnityEngine.Object) null)
        followerPossessed.LookAtTarget();
      yield return (object) null;
    }
    followerPossessed.GetPath();
    float RepathTimer = 0.0f;
    int NumAttacks = 2;
    float AttackSpeed = 15f;
    bool Loop = true;
    while (Loop)
    {
      switch (followerPossessed.state.CURRENT_STATE)
      {
        case StateMachine.State.Idle:
        case StateMachine.State.Moving:
          followerPossessed.LookAtTarget();
          if ((double) GameManager.GetInstance().CurrentTime > ((double) EnemyFollowerPossessed.LastSimpleGuardianRingProjectiles + (double) followerPossessed.GlobalRingAttackDelay) / (double) followerPossessed.Spine.timeScale)
          {
            EnemyFollowerPossessed.LastSimpleGuardianRingProjectiles = GameManager.GetInstance().CurrentTime;
            followerPossessed.ReconsiderTarget();
            int num;
            do
            {
              num = UnityEngine.Random.Range(0, 4);
              if (num == 0)
              {
                followerPossessed.ProjectileRings();
                yield break;
              }
              if ((UnityEngine.Object) followerPossessed.GetClosestTarget() != (UnityEngine.Object) null && (double) Vector3.Distance(followerPossessed.GetClosestTarget().transform.position, followerPossessed.transform.position) < 4.0 && num == 1)
              {
                followerPossessed.ProjectileBoomerangs();
                yield break;
              }
              if (BiomeGenerator.Instance.PossessedEnemyEncounterCount > 1 && num == 2)
              {
                followerPossessed.ProjectilePulse();
                yield break;
              }
            }
            while (BiomeGenerator.Instance.PossessedEnemyEncounterCount <= 2 || num != 3);
            followerPossessed.ProjectileBounce();
            yield break;
          }
          if ((double) Vector2.Distance((Vector2) followerPossessed.transform.position, (Vector2) followerPossessed.TargetObject.transform.position) >= 3.0 && (double) (RepathTimer += Time.deltaTime * followerPossessed.Spine.timeScale) > 0.20000000298023224)
          {
            RepathTimer = 0.0f;
            followerPossessed.GetPath();
          }
          if ((UnityEngine.Object) followerPossessed.damageColliderEvents != (UnityEngine.Object) null)
          {
            followerPossessed.damageColliderEvents.SetActive(false);
            break;
          }
          break;
        case StateMachine.State.SignPostAttack:
          followerPossessed.state.facingAngle = Utils.GetAngle(followerPossessed.transform.position, followerPossessed.TargetPosition);
          if ((double) (followerPossessed.state.Timer += Time.deltaTime * followerPossessed.Spine.timeScale) >= 0.5)
          {
            followerPossessed.SimpleSpineFlash.FlashWhite(false);
            CameraManager.shakeCamera(0.4f, followerPossessed.state.facingAngle);
            followerPossessed.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
            followerPossessed.speed = AttackSpeed * Time.deltaTime;
            if ((UnityEngine.Object) followerPossessed.damageColliderEvents != (UnityEngine.Object) null)
              followerPossessed.damageColliderEvents.SetActive(true);
            if (!string.IsNullOrEmpty(followerPossessed.attackSoundPath))
            {
              AudioManager.Instance.PlayOneShot(followerPossessed.attackSoundPath, followerPossessed.transform.position);
              break;
            }
            break;
          }
          followerPossessed.SimpleSpineFlash.FlashWhite(followerPossessed.state.Timer / 0.5f);
          break;
        case StateMachine.State.RecoverFromAttack:
          if ((double) AttackSpeed > 0.0)
            AttackSpeed -= 1f * GameManager.DeltaTime * followerPossessed.Spine.timeScale;
          followerPossessed.speed = AttackSpeed * Time.deltaTime * followerPossessed.Spine.timeScale;
          if ((double) followerPossessed.state.Timer >= 0.25 && (UnityEngine.Object) followerPossessed.damageColliderEvents != (UnityEngine.Object) null)
            followerPossessed.damageColliderEvents.SetActive(false);
          if ((double) (followerPossessed.state.Timer += Time.deltaTime * followerPossessed.Spine.timeScale) >= 0.5)
          {
            if (--NumAttacks > 0)
            {
              AttackSpeed = (float) (15 + (3 - NumAttacks) * 2);
              followerPossessed.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
              followerPossessed.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
              break;
            }
            Loop = false;
            followerPossessed.state.CURRENT_STATE = StateMachine.State.Idle;
            followerPossessed.Spine.AnimationState.SetAnimation(0, "idle", true);
            break;
          }
          break;
      }
      yield return (object) null;
    }
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * followerPossessed.Spine.timeScale) < 0.5)
      yield return (object) null;
    followerPossessed.state.CURRENT_STATE = StateMachine.State.Idle;
    followerPossessed.Spine.AnimationState.SetAnimation(0, "idle", true);
    if ((UnityEngine.Object) followerPossessed.TargetObject != (UnityEngine.Object) null && (double) Vector3.Distance(followerPossessed.TargetObject.transform.position, followerPossessed.transform.position) > 5.0)
    {
      followerPossessed.LookAtTarget();
      time = 0.0f;
      while ((double) (time += Time.deltaTime * followerPossessed.Spine.timeScale) < 1.0)
        yield return (object) null;
    }
    followerPossessed.StartCoroutine(followerPossessed.FightPlayer());
  }

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.health.team && !component.IsCharmedEnemy)
      return;
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }

  public void ProjectileRings()
  {
    this.StopAllCoroutines();
    this.StartCoroutine(this.ProjectileRingsRoutine());
  }

  public IEnumerator ProjectileRingsRoutine()
  {
    EnemyFollowerPossessed followerPossessed = this;
    followerPossessed.state.CURRENT_STATE = StateMachine.State.Idle;
    yield return (object) null;
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * followerPossessed.Spine.timeScale) < 1.0)
      yield return (object) null;
    Projectile arrow = UnityEngine.Object.Instantiate<ProjectileCircle>(followerPossessed.projectilePatternRings, followerPossessed.transform.parent).GetComponent<Projectile>();
    arrow.transform.position = followerPossessed.transform.position;
    arrow.health = followerPossessed.health;
    arrow.team = followerPossessed.health.team;
    arrow.Speed = followerPossessed.projectilePatternRingsSpeed;
    arrow.Acceleration = followerPossessed.projectilePatternRingsAcceleration;
    arrow.Owner = followerPossessed.health;
    arrow.GetComponent<ProjectileCircle>().InitDelayed(PlayerFarming.FindClosestPlayerGameObject(followerPossessed.transform.position), followerPossessed.projectilePatternRingsRadius, 0.0f, (System.Action) (() =>
    {
      CameraManager.instance.ShakeCameraForDuration(0.8f, 0.9f, 0.3f, false);
      if ((UnityEngine.Object) this.guardianGameObject != (UnityEngine.Object) null)
      {
        AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/grenade_mass_launch", this.guardianGameObject);
        arrow.Angle = Mathf.Round(arrow.Angle / 45f) * 45f;
      }
      else
        arrow.DestroyProjectile();
    }));
    time = 0.0f;
    while ((double) (time += Time.deltaTime * followerPossessed.Spine.timeScale) < 1.5666667222976685)
      yield return (object) null;
    followerPossessed.state.CURRENT_STATE = StateMachine.State.Idle;
    followerPossessed.Spine.AnimationState.SetAnimation(0, "idle", true);
    followerPossessed.StartCoroutine(followerPossessed.FightPlayer());
  }

  public void ProjectileBounce()
  {
    this.StopAllCoroutines();
    this.StartCoroutine(this.ProjectileBounceRoutine());
  }

  public IEnumerator ProjectileBounceRoutine()
  {
    EnemyFollowerPossessed followerPossessed = this;
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * followerPossessed.Spine.timeScale) < 1.7666666507720947)
      yield return (object) null;
    for (float num = 0.0f; (double) num < 10.0; ++num)
    {
      Projectile component = UnityEngine.Object.Instantiate<GameObject>(followerPossessed.BouncingArrow, followerPossessed.transform.parent).GetComponent<Projectile>();
      component.transform.position = followerPossessed.transform.position;
      component.team = followerPossessed.health.team;
      component.Speed = followerPossessed.BoomerangSpeed * 3f;
      component.Angle = 72f * num;
      component.Owner = followerPossessed.health;
    }
    followerPossessed.state.CURRENT_STATE = StateMachine.State.Idle;
    followerPossessed.StartCoroutine(followerPossessed.FightPlayer());
  }

  public void ProjectileBoomerangs()
  {
    this.StopAllCoroutines();
    this.StartCoroutine(this.ProjectileBoomerangsRoutine());
  }

  public IEnumerator ProjectileBoomerangsRoutine()
  {
    EnemyFollowerPossessed followerPossessed = this;
    followerPossessed.state.CURRENT_STATE = StateMachine.State.Idle;
    yield return (object) null;
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * followerPossessed.Spine.timeScale) < 1.7666666507720947)
      yield return (object) null;
    CameraManager.instance.ShakeCameraForDuration(0.8f, 0.9f, 0.3f, false);
    followerPossessed.Boomeranges = new List<Projectile>();
    float boomerangCount = followerPossessed.BoomerangCount;
    for (float num = 0.0f; (double) num < (double) boomerangCount; ++num)
    {
      Projectile component = UnityEngine.Object.Instantiate<GameObject>(followerPossessed.BoomerangArrow, followerPossessed.transform.parent).GetComponent<Projectile>();
      component.transform.position = followerPossessed.transform.position;
      component.team = followerPossessed.health.team;
      component.Speed = followerPossessed.BoomerangSpeed;
      component.Angle = 360f / boomerangCount * num;
      component.LifeTime = 30f;
      component.IgnoreIsland = true;
      component.Trail.time = 0.3f;
      component.Owner = followerPossessed.health;
      followerPossessed.Boomeranges.Add(component);
    }
    AudioManager.Instance.PlayOneShot("event:/enemy/shoot_magicenergy", followerPossessed.transform.position);
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/grenade_mass_launch", followerPossessed.gameObject);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * followerPossessed.Spine.timeScale) < (double) followerPossessed.OutwardDuration)
      yield return (object) null;
    foreach (Projectile boomerange in followerPossessed.Boomeranges)
    {
      Projectile a = boomerange;
      if ((UnityEngine.Object) a != (UnityEngine.Object) null)
        DOTween.To((DOGetter<float>) (() => a.SpeedMultiplier), (DOSetter<float>) (x => a.SpeedMultiplier = x), 0.0f, 0.2f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutSine);
    }
    time = 0.0f;
    while ((double) (time += Time.deltaTime * followerPossessed.Spine.timeScale) < (double) followerPossessed.PauseDuration)
      yield return (object) null;
    foreach (Projectile boomerange in followerPossessed.Boomeranges)
    {
      Projectile a = boomerange;
      if ((UnityEngine.Object) a != (UnityEngine.Object) null)
        DOTween.To((DOGetter<float>) (() => a.SpeedMultiplier), (DOSetter<float>) (x => a.SpeedMultiplier = x), followerPossessed.BoomerangReturnSpeed, 0.3f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutBack);
    }
    AudioManager.Instance.PlayOneShot("event:/enemy/shoot_magicenergy", followerPossessed.transform.position);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * followerPossessed.Spine.timeScale) < (double) followerPossessed.ReturnDuration)
      yield return (object) null;
    bool flag = true;
    foreach (Projectile boomerange in followerPossessed.Boomeranges)
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
    followerPossessed.Boomeranges.Clear();
    followerPossessed.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * followerPossessed.Spine.timeScale) < 0.699999988079071)
      yield return (object) null;
    followerPossessed.state.CURRENT_STATE = StateMachine.State.Idle;
    followerPossessed.StartCoroutine(followerPossessed.FightPlayer());
  }

  public void ProjectilePulse()
  {
    this.StopAllCoroutines();
    this.StartCoroutine(this.ProjectilePulseRoutine());
  }

  public IEnumerator ProjectilePulseRoutine()
  {
    EnemyFollowerPossessed followerPossessed = this;
    followerPossessed.state.CURRENT_STATE = StateMachine.State.Idle;
    int amount = UnityEngine.Random.Range(3, 6);
    float a = 0.0f;
    for (int i = 0; i < amount; ++i)
    {
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * followerPossessed.Spine.timeScale) < 0.5)
        yield return (object) null;
      CameraManager.instance.ShakeCameraForDuration(1f, 1f, 0.25f);
      BiomeConstants.Instance.EmitSmokeExplosionVFX(followerPossessed.transform.position + Vector3.back * 0.5f);
      EnemyFollowerPossessed.LastSimpleGuardianRingProjectiles = GameManager.GetInstance().CurrentTime;
      Projectile.CreateProjectiles(10, followerPossessed.health, followerPossessed.transform.position, 15f, angleOffset: a += 45f, callback: new System.Action<List<Projectile>>(followerPossessed.\u003CProjectilePulseRoutine\u003Eb__53_0));
      time = 0.0f;
      while ((double) (time += Time.deltaTime * followerPossessed.Spine.timeScale) < 0.40000000596046448)
        yield return (object) null;
    }
    followerPossessed.state.CURRENT_STATE = StateMachine.State.Idle;
    followerPossessed.StartCoroutine(followerPossessed.FightPlayer());
  }

  public IEnumerator SetProjectilePulse(List<Projectile> projectiles)
  {
    yield return (object) new WaitForEndOfFrame();
    foreach (Projectile projectile in projectiles)
    {
      projectile.Deceleration = 15f;
      projectile.pulseMove = true;
    }
  }

  [CompilerGenerated]
  public void \u003CProjectilePulseRoutine\u003Eb__53_0(List<Projectile> projectiles)
  {
    this.StartCoroutine(this.SetProjectilePulse(projectiles));
  }
}
