// Decompiled with JetBrains decompiler
// Type: EnemyArcherTennis
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMODUnity;
using MMBiomeGeneration;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnemyArcherTennis : UnitObject
{
  public float GlobalShotDelay = 5f;
  public int ShotsToFire = 1;
  public float DelayBetweenShots = 0.2f;
  public float DelayReaiming = 0.5f;
  public TennisBall tennisBall;
  public GameObject TargetObject;
  public SkeletonAnimation Spine;
  public int DistanceFromTargetPosition = 3;
  public float maxTennisDist = 16f;
  public SimpleSpineFlash SimpleSpineFlash;
  public SpriteRenderer Aiming;
  public ColliderEvents damageColliderEvents;
  public List<ParticleSystem> invincibleEffects;
  public float chanceOfMissingTennisReturnBase = 0.1f;
  public float chanceOfMissingTennisReturnIncrease = 0.05f;
  public bool canBeParried;
  public static float signPostParryWindow = 0.2f;
  public static float attackParryWindow = 0.15f;
  [EventRef]
  public string attackSoundPath = string.Empty;
  [EventRef]
  public string shootSoundPath = string.Empty;
  [EventRef]
  public string AttackVO = string.Empty;
  [EventRef]
  public string DeathVO = string.Empty;
  [EventRef]
  public string GetHitVO = string.Empty;
  [EventRef]
  public string WarningVO = string.Empty;
  public GameObject prefabForFleeceSwap;
  public Coroutine cWaitForTarget;
  public float KnockbackMultiplier = 1f;
  public float ShootDelay;
  public float TeleportDelay;
  [HideInInspector]
  public float Angle;
  public float RandomChangeAngle = 3f;
  public Vector3 TargetPosition;
  public int MaintainDistance = 3;
  public float RepathTimer;
  public EnemyArcherTennis.State MyState;
  public float lastShot;
  public float CloseCombatCooldown;
  public float CircleCastRadius = 0.5f;
  public float CircleCastOffset = 1f;
  public int AcceptableMove = 2;
  public bool ShowDebug;
  public List<Vector3> Points = new List<Vector3>();
  public List<Vector3> PointsLink = new List<Vector3>();
  public List<Vector3> EndPoints = new List<Vector3>();
  public List<Vector3> EndPointsLink = new List<Vector3>();

  public override void Awake()
  {
    base.Awake();
    if ((UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null)
      this.GetComponent<Health>().totalHP *= BiomeGenerator.Instance.HumanoidHealthMultiplier;
    if (!PlayerFleeceManager.FleeceSwapsWeaponForCurse())
      return;
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.prefabForFleeceSwap);
    gameObject.transform.SetParent(this.transform.parent);
    gameObject.transform.localPosition = Vector3.zero;
    Interaction_Chest.Instance?.AddEnemy(gameObject.GetComponent<Health>());
    foreach (Health componentsInChild in gameObject.GetComponentsInChildren<Health>(true))
      Interaction_Chest.Instance?.AddEnemy(componentsInChild);
    Interaction_Chest.Instance?.Enemies.Remove(this.health);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public void Start()
  {
    if (PlayerFleeceManager.FleeceSwapsWeaponForCurse())
      return;
    this.SeperateObject = true;
    this.Aiming.gameObject.SetActive(false);
    this.SetInvincible(true);
  }

  public void SetInvincible(bool enabled)
  {
    if (enabled)
    {
      for (int index = 0; index < this.invincibleEffects.Count; ++index)
      {
        if ((UnityEngine.Object) this.invincibleEffects[index] != (UnityEngine.Object) null)
          this.invincibleEffects[index].Play();
      }
      this.health.invincible = false;
      this.health.DamageModifier = 0.25f;
      Cower component = this.GetComponent<Cower>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      component.preventStandardStagger = true;
    }
    else
    {
      for (int index = 0; index < this.invincibleEffects.Count; ++index)
      {
        if ((UnityEngine.Object) this.invincibleEffects[index] != (UnityEngine.Object) null)
          this.invincibleEffects[index].Stop();
      }
      this.health.invincible = false;
      this.health.DamageModifier = 1.5f;
      Cower component = this.GetComponent<Cower>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      component.preventStandardStagger = false;
    }
  }

  public void DoWaitForTarget()
  {
    if (this.cWaitForTarget != null)
      this.StopCoroutine(this.cWaitForTarget);
    this.cWaitForTarget = this.StartCoroutine((IEnumerator) this.WaitForTarget());
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.DoWaitForTarget();
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.damageColliderEvents.SetActive(false);
    }
    this.SetInvincible(true);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    this.Aiming.gameObject.SetActive(false);
    this.ClearPaths();
    this.StopAllCoroutines();
    this.DisableForces = false;
    this.SimpleSpineFlash.FlashWhite(false);
    if (!((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null))
      return;
    this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
  }

  public override void BeAlarmed(GameObject TargetObject)
  {
    base.BeAlarmed(TargetObject);
    if (!string.IsNullOrEmpty(this.WarningVO))
      AudioManager.Instance.PlayOneShot(this.WarningVO, this.transform.position);
    this.TargetObject = TargetObject;
    this.DoWaitForTarget();
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    if (this.health.WasJustParried)
      return;
    if ((double) this.health.DamageModifier < 1.0)
    {
      Vector3 position = this.transform.position;
      position.y -= 0.5f;
      position.z -= 0.75f;
      BiomeConstants.Instance.EmitBlockImpact(position, this.Angle, this.transform);
      this.DoKnockBack(Utils.GetAngle(Attacker.transform.position, this.transform.position) * ((float) Math.PI / 180f), this.KnockbackMultiplier * 0.5f, 0.25f);
      base.OnHit(Attacker, AttackLocation, AttackType);
    }
    else
    {
      this.SimpleSpineFlash.FlashWhite(false);
      this.SimpleSpineFlash.FlashFillRed();
      base.OnHit(Attacker, AttackLocation, AttackType);
      this.DoKnockBack(Utils.GetAngle(Attacker.transform.position, this.transform.position) * ((float) Math.PI / 180f), this.KnockbackMultiplier, 0.5f);
    }
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if (!string.IsNullOrEmpty(this.DeathVO))
      AudioManager.Instance.PlayOneShot(this.DeathVO, this.transform.position);
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
  }

  public IEnumerator HurtRoutine()
  {
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
      this.damageColliderEvents.SetActive(false);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * this.Spine.timeScale) < 0.5)
      yield return (object) null;
    this.DoWaitForTarget();
    this.ShootDelay = 0.0f;
    if ((double) this.TeleportDelay < 0.0 && (double) UnityEngine.Random.Range(0.0f, 0.75f) < 0.5)
      this.Teleport();
  }

  public IEnumerator WaitForTarget()
  {
    EnemyArcherTennis enemyArcherTennis = this;
    if ((UnityEngine.Object) enemyArcherTennis.damageColliderEvents != (UnityEngine.Object) null)
      enemyArcherTennis.damageColliderEvents.SetActive(false);
    enemyArcherTennis.RepathTimer = 0.5f;
    while ((UnityEngine.Object) enemyArcherTennis.TargetObject == (UnityEngine.Object) null)
    {
      Health closestTarget = enemyArcherTennis.GetClosestTarget();
      if ((bool) (UnityEngine.Object) closestTarget)
      {
        enemyArcherTennis.TargetObject = closestTarget.gameObject;
        enemyArcherTennis.VisionRange = int.MaxValue;
      }
      enemyArcherTennis.RepathTimer -= Time.deltaTime * enemyArcherTennis.Spine.timeScale;
      if ((double) enemyArcherTennis.RepathTimer <= 0.0)
      {
        if (enemyArcherTennis.state.CURRENT_STATE == StateMachine.State.Moving)
        {
          if (enemyArcherTennis.Spine.AnimationName != "run")
            enemyArcherTennis.Spine.AnimationState.SetAnimation(0, "run", true);
        }
        else if (enemyArcherTennis.Spine.AnimationName != "idle")
          enemyArcherTennis.Spine.AnimationState.SetAnimation(0, "idle", true);
        enemyArcherTennis.TargetPosition = enemyArcherTennis.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 2f;
        enemyArcherTennis.FindPath(enemyArcherTennis.TargetPosition);
        enemyArcherTennis.state.LookAngle = Utils.GetAngle(enemyArcherTennis.transform.position, enemyArcherTennis.TargetPosition);
        enemyArcherTennis.Spine.skeleton.ScaleX = (double) enemyArcherTennis.state.LookAngle <= 90.0 || (double) enemyArcherTennis.state.LookAngle >= 270.0 ? -1f : 1f;
      }
      yield return (object) null;
    }
    bool InRange = false;
    while (!InRange)
    {
      if ((UnityEngine.Object) enemyArcherTennis.TargetObject == (UnityEngine.Object) null)
      {
        enemyArcherTennis.DoWaitForTarget();
        yield break;
      }
      float a = Vector3.Distance(enemyArcherTennis.TargetObject.transform.position, enemyArcherTennis.transform.position);
      if ((double) a <= (double) enemyArcherTennis.VisionRange && enemyArcherTennis.CheckLineOfSightOnTarget(enemyArcherTennis.TargetObject, enemyArcherTennis.TargetObject.transform.position, Mathf.Min(a, (float) enemyArcherTennis.VisionRange)))
      {
        InRange = true;
        yield return (object) null;
      }
      else
      {
        enemyArcherTennis.StartCoroutine((IEnumerator) enemyArcherTennis.ChasePlayer());
        yield break;
      }
    }
    enemyArcherTennis.StartCoroutine((IEnumerator) enemyArcherTennis.ChasePlayer());
  }

  public void ReturnFire(bool success)
  {
    if (success)
    {
      CameraManager.shakeCamera(2f);
      AudioManager.Instance.PlayOneShot("event:/player/Curses/arrow_hit", this.transform.position);
      this.StartCoroutine((IEnumerator) this.CloseCombatAttack(0.1f, 0.1f));
    }
    else
    {
      AudioManager.Instance.PlayOneShot("event:/player/Curses/arrow_hit", this.transform.position);
      CameraManager.shakeCamera(5f);
    }
  }

  public IEnumerator ChasePlayer()
  {
    EnemyArcherTennis enemyArcherTennis = this;
    if ((UnityEngine.Object) enemyArcherTennis.damageColliderEvents != (UnityEngine.Object) null)
      enemyArcherTennis.damageColliderEvents.SetActive(false);
    enemyArcherTennis.MyState = EnemyArcherTennis.State.Idle;
    bool Loop = true;
    enemyArcherTennis.Angle = Utils.GetAngle(enemyArcherTennis.TargetObject.transform.position, enemyArcherTennis.transform.position) * ((float) Math.PI / 180f);
    enemyArcherTennis.TargetPosition = enemyArcherTennis.TargetObject.transform.position + new Vector3((float) enemyArcherTennis.MaintainDistance * Mathf.Cos(enemyArcherTennis.Angle), (float) enemyArcherTennis.MaintainDistance * Mathf.Sin(enemyArcherTennis.Angle));
    while (Loop)
    {
      if (enemyArcherTennis.MyState == EnemyArcherTennis.State.Idle)
      {
        if ((UnityEngine.Object) enemyArcherTennis.damageColliderEvents != (UnityEngine.Object) null)
          enemyArcherTennis.damageColliderEvents.SetActive(false);
        if ((UnityEngine.Object) enemyArcherTennis.TargetObject == (UnityEngine.Object) null)
        {
          enemyArcherTennis.DoWaitForTarget();
          break;
        }
        if ((double) (enemyArcherTennis.CloseCombatCooldown -= Time.deltaTime * enemyArcherTennis.Spine.timeScale) < 0.0 && (double) Vector3.Distance(enemyArcherTennis.transform.position, enemyArcherTennis.TargetObject.transform.position) < 2.0)
        {
          enemyArcherTennis.StartCoroutine((IEnumerator) enemyArcherTennis.CloseCombatAttack());
          break;
        }
        if (enemyArcherTennis.state.CURRENT_STATE == StateMachine.State.Moving)
        {
          if (enemyArcherTennis.Spine.AnimationName != "run")
            enemyArcherTennis.Spine.AnimationState.SetAnimation(0, "run", true);
        }
        else if (enemyArcherTennis.Spine.AnimationName != "idle")
          enemyArcherTennis.Spine.AnimationState.SetAnimation(0, "idle", true);
        enemyArcherTennis.TeleportDelay -= Time.deltaTime * enemyArcherTennis.Spine.timeScale;
        enemyArcherTennis.state.LookAngle = Utils.GetAngle(enemyArcherTennis.transform.position, enemyArcherTennis.TargetObject.transform.position);
        enemyArcherTennis.Spine.skeleton.ScaleX = (double) enemyArcherTennis.state.LookAngle <= 90.0 || (double) enemyArcherTennis.state.LookAngle >= 270.0 ? -1f : 1f;
        if ((double) (enemyArcherTennis.RepathTimer -= Time.deltaTime * enemyArcherTennis.Spine.timeScale) < 0.0)
          enemyArcherTennis.TargetPosition = enemyArcherTennis.TargetObject.transform.position + new Vector3((float) enemyArcherTennis.MaintainDistance * Mathf.Cos(enemyArcherTennis.Angle), (float) enemyArcherTennis.MaintainDistance * Mathf.Sin(enemyArcherTennis.Angle));
        if ((double) Vector3.Distance(enemyArcherTennis.TargetPosition, enemyArcherTennis.transform.position) > (double) enemyArcherTennis.DistanceFromTargetPosition && Time.frameCount % 5 == 0)
          enemyArcherTennis.FindPath(enemyArcherTennis.TargetPosition);
        if (!enemyArcherTennis.tennisBall.isActive && (double) (enemyArcherTennis.ShootDelay -= Time.deltaTime * enemyArcherTennis.Spine.timeScale) < 0.0 && (double) Vector3.Distance(enemyArcherTennis.transform.position, enemyArcherTennis.TargetObject.transform.position) < (double) enemyArcherTennis.maxTennisDist && (double) Time.realtimeSinceStartup > ((double) enemyArcherTennis.lastShot + (double) enemyArcherTennis.GlobalShotDelay) / (double) enemyArcherTennis.Spine.timeScale)
        {
          enemyArcherTennis.lastShot = Time.realtimeSinceStartup;
          enemyArcherTennis.StartCoroutine((IEnumerator) enemyArcherTennis.ShootArrowRoutine());
          break;
        }
      }
      yield return (object) null;
    }
  }

  public IEnumerator CloseCombatAttack(float waitTime = 0.4f, float signpostDelay = 0.5f)
  {
    EnemyArcherTennis enemyArcherTennis = this;
    enemyArcherTennis.ClearPaths();
    enemyArcherTennis.MyState = EnemyArcherTennis.State.CloseCombatAttack;
    enemyArcherTennis.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    float Progress = 0.0f;
    enemyArcherTennis.Spine.AnimationState.SetAnimation(0, "grunt-attack-charge2", false);
    enemyArcherTennis.state.facingAngle = enemyArcherTennis.state.LookAngle = Utils.GetAngle(enemyArcherTennis.transform.position, enemyArcherTennis.TargetObject.transform.position);
    enemyArcherTennis.Spine.skeleton.ScaleX = (double) enemyArcherTennis.state.LookAngle <= 90.0 || (double) enemyArcherTennis.state.LookAngle >= 270.0 ? -1f : 1f;
    while ((double) (Progress += Time.deltaTime * enemyArcherTennis.Spine.timeScale) < (double) signpostDelay)
    {
      if ((double) Progress >= (double) signpostDelay - (double) EnemyArcherTennis.signPostParryWindow)
        enemyArcherTennis.canBeParried = true;
      enemyArcherTennis.SimpleSpineFlash.FlashWhite(Progress / signpostDelay);
      yield return (object) null;
    }
    enemyArcherTennis.speed = 0.2f;
    enemyArcherTennis.SimpleSpineFlash.FlashWhite(false);
    enemyArcherTennis.Spine.AnimationState.SetAnimation(0, "grunt-attack-impact2", false);
    if (!string.IsNullOrEmpty(enemyArcherTennis.AttackVO))
      AudioManager.Instance.PlayOneShot(enemyArcherTennis.AttackVO, enemyArcherTennis.transform.position);
    if (!string.IsNullOrEmpty(enemyArcherTennis.attackSoundPath))
      AudioManager.Instance.PlayOneShot(enemyArcherTennis.attackSoundPath, enemyArcherTennis.transform.position);
    Progress = 0.0f;
    float Duration = 0.2f;
    while ((double) (Progress += Time.deltaTime * enemyArcherTennis.Spine.timeScale) < (double) Duration)
    {
      if ((UnityEngine.Object) enemyArcherTennis.damageColliderEvents != (UnityEngine.Object) null)
        enemyArcherTennis.damageColliderEvents.SetActive(true);
      enemyArcherTennis.canBeParried = (double) Progress <= (double) EnemyArcherTennis.attackParryWindow;
      yield return (object) null;
    }
    if ((UnityEngine.Object) enemyArcherTennis.damageColliderEvents != (UnityEngine.Object) null)
      enemyArcherTennis.damageColliderEvents.SetActive(false);
    enemyArcherTennis.canBeParried = false;
    yield return (object) new WaitForSeconds(waitTime);
    enemyArcherTennis.CloseCombatCooldown = 0.5f;
    enemyArcherTennis.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyArcherTennis.DoWaitForTarget();
  }

  public IEnumerator ShootArrowRoutine()
  {
    EnemyArcherTennis enemyArcherTennis = this;
    enemyArcherTennis.ClearPaths();
    enemyArcherTennis.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    enemyArcherTennis.MyState = EnemyArcherTennis.State.Shooting;
    enemyArcherTennis.ShootDelay = UnityEngine.Random.Range(1f, 1.5f);
    yield return (object) null;
    enemyArcherTennis.Spine.AnimationState.SetAnimation(0, "archer-attack-charge2", false);
    enemyArcherTennis.Aiming.gameObject.SetActive(true);
    float time = 0.0f;
    float Progress = 0.0f;
    while ((double) (Progress += Time.deltaTime * enemyArcherTennis.Spine.timeScale) < 1.0)
    {
      enemyArcherTennis.Aiming.transform.eulerAngles = new Vector3(0.0f, 0.0f, enemyArcherTennis.state.LookAngle);
      enemyArcherTennis.SimpleSpineFlash?.FlashWhite(Progress / 1f);
      if (Time.frameCount % 5 == 0)
        enemyArcherTennis.Aiming.color = enemyArcherTennis.Aiming.color == Color.red ? Color.white : Color.red;
      yield return (object) null;
    }
    enemyArcherTennis.SimpleSpineFlash.FlashWhite(false);
    enemyArcherTennis.Aiming.gameObject.SetActive(false);
    if (!string.IsNullOrEmpty(enemyArcherTennis.AttackVO))
      AudioManager.Instance.PlayOneShot(enemyArcherTennis.AttackVO, enemyArcherTennis.transform.position);
    int i = enemyArcherTennis.ShotsToFire;
    while (--i >= 0)
    {
      if (!string.IsNullOrEmpty(enemyArcherTennis.shootSoundPath))
        AudioManager.Instance.PlayOneShot(enemyArcherTennis.shootSoundPath, enemyArcherTennis.transform.position);
      CameraManager.shakeCamera(0.2f, enemyArcherTennis.state.LookAngle);
      enemyArcherTennis.tennisBall.transform.position = enemyArcherTennis.transform.position + new Vector3(0.5f * Mathf.Cos(enemyArcherTennis.state.LookAngle * ((float) Math.PI / 180f)), 0.5f * Mathf.Sin(enemyArcherTennis.state.LookAngle * ((float) Math.PI / 180f)));
      Health health = enemyArcherTennis.ReconsiderPlayerTarget();
      enemyArcherTennis.tennisBall.Init((UnityEngine.Object) health == (UnityEngine.Object) null ? (Health) PlayerFarming.Instance.health : health, enemyArcherTennis.health);
      enemyArcherTennis.Spine.AnimationState.SetAnimation(0, "archer-attack-impact2", false);
      time = 0.0f;
      while ((double) (time += Time.deltaTime * enemyArcherTennis.Spine.timeScale) < (double) enemyArcherTennis.DelayBetweenShots)
        yield return (object) null;
      if ((UnityEngine.Object) enemyArcherTennis.TargetObject != (UnityEngine.Object) null && i > 0)
      {
        enemyArcherTennis.Aiming.gameObject.SetActive(true);
        enemyArcherTennis.state.LookAngle = Utils.GetAngle(enemyArcherTennis.transform.position, enemyArcherTennis.TargetObject.transform.position);
        enemyArcherTennis.Spine.skeleton.ScaleX = (double) enemyArcherTennis.state.LookAngle <= 90.0 || (double) enemyArcherTennis.state.LookAngle >= 270.0 ? -1f : 1f;
        enemyArcherTennis.Aiming.transform.eulerAngles = new Vector3(0.0f, 0.0f, enemyArcherTennis.state.LookAngle);
        yield return (object) new WaitForSeconds(enemyArcherTennis.DelayReaiming);
      }
    }
    enemyArcherTennis.Aiming.gameObject.SetActive(false);
    enemyArcherTennis.TargetObject = (GameObject) null;
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyArcherTennis.Spine.timeScale) < 0.30000001192092896)
      yield return (object) null;
    enemyArcherTennis.MyState = EnemyArcherTennis.State.Idle;
    enemyArcherTennis.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyArcherTennis.DoWaitForTarget();
  }

  public IEnumerator TeleportRoutine(Vector3 Position)
  {
    EnemyArcherTennis enemyArcherTennis = this;
    enemyArcherTennis.ClearPaths();
    enemyArcherTennis.state.CURRENT_STATE = StateMachine.State.Moving;
    enemyArcherTennis.UsePathing = false;
    enemyArcherTennis.SeperateObject = false;
    enemyArcherTennis.MyState = EnemyArcherTennis.State.Teleporting;
    enemyArcherTennis.ClearPaths();
    enemyArcherTennis.ShootDelay = 1f;
    Vector3 position = enemyArcherTennis.transform.position;
    float Progress = 0.0f;
    enemyArcherTennis.Spine.AnimationState.SetAnimation(0, "roll", true);
    enemyArcherTennis.state.facingAngle = enemyArcherTennis.state.LookAngle = Utils.GetAngle(enemyArcherTennis.transform.position, Position);
    enemyArcherTennis.Spine.skeleton.ScaleX = (double) enemyArcherTennis.state.LookAngle <= 90.0 || (double) enemyArcherTennis.state.LookAngle >= 270.0 ? -1f : 1f;
    Vector3 b = Position;
    float Duration = Vector3.Distance(position, b) / 12f;
    while ((double) (Progress += Time.deltaTime * enemyArcherTennis.Spine.timeScale) < (double) Duration)
    {
      enemyArcherTennis.speed = 10f * Time.deltaTime;
      yield return (object) null;
    }
    enemyArcherTennis.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyArcherTennis.Spine.AnimationState.SetAnimation(0, "roll-stop", false);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyArcherTennis.Spine.timeScale) < 0.30000001192092896)
      yield return (object) null;
    enemyArcherTennis.UsePathing = true;
    enemyArcherTennis.SeperateObject = true;
    enemyArcherTennis.TargetPosition = Position;
    enemyArcherTennis.RepathTimer = 0.5f;
    enemyArcherTennis.ShootDelay = UnityEngine.Random.Range(0.0f, 0.25f);
    enemyArcherTennis.TeleportDelay = 0.5f;
    enemyArcherTennis.MyState = EnemyArcherTennis.State.Idle;
  }

  public void FindPath(Vector3 PointToCheck)
  {
    if (this.MyState == EnemyArcherTennis.State.Teleporting)
      return;
    if ((UnityEngine.Object) this.TargetObject == (UnityEngine.Object) null)
    {
      this.givePath(this.TargetPosition);
      this.RepathTimer = 0.5f;
    }
    else
    {
      this.Angle = Utils.GetAngle(this.TargetObject.transform.position, this.transform.position) * ((float) Math.PI / 180f);
      RaycastHit2D raycastHit2D = Physics2D.CircleCast((Vector2) this.transform.position, 1f, (Vector2) Vector3.Normalize(PointToCheck - this.transform.position), (float) this.MaintainDistance, (int) this.layerToCheck);
      if ((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null)
      {
        if ((double) Vector3.Distance(this.transform.position, (Vector3) raycastHit2D.centroid) > (double) this.AcceptableMove)
        {
          if ((double) this.TeleportDelay > 0.0 || (double) UnityEngine.Random.Range(0.0f, 0.5f) == 0.0)
          {
            if (this.ShowDebug)
            {
              this.Points.Add(new Vector3(raycastHit2D.centroid.x, raycastHit2D.centroid.y) + Vector3.Normalize(this.transform.position - PointToCheck) * this.CircleCastOffset);
              this.PointsLink.Add(new Vector3(this.transform.position.x, this.transform.position.y));
            }
            this.TargetPosition = (Vector3) raycastHit2D.centroid + Vector3.Normalize(this.transform.position - PointToCheck) * this.CircleCastOffset;
            this.givePath(this.TargetPosition);
            this.RepathTimer = 0.5f;
          }
          else
            this.Teleport();
        }
        else if ((double) this.TeleportDelay < 0.0 && (double) Vector3.Distance(this.transform.position, this.TargetObject.transform.position) < 2.0)
        {
          this.Teleport();
        }
        else
        {
          if ((double) Vector3.Distance(this.transform.position, this.TargetObject.transform.position) <= 5.0)
            return;
          if ((double) this.TeleportDelay > 0.0 || (double) UnityEngine.Random.Range(0.0f, 0.5f) == 0.0)
          {
            this.TargetPosition = Vector3.Lerp(this.transform.position, this.TargetObject.transform.position, 0.5f);
            this.givePath(this.TargetPosition);
            this.RepathTimer = 0.5f;
          }
          else
            this.Teleport();
        }
      }
      else
      {
        this.TargetPosition = PointToCheck;
        this.givePath(PointToCheck);
        this.RepathTimer = 0.5f;
      }
    }
  }

  public void Teleport()
  {
    if (this.MyState != EnemyArcherTennis.State.Idle)
      return;
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
      this.damageColliderEvents.SetActive(false);
    float num1 = 100f;
    float num2;
    if ((double) (num2 = num1 - 1f) <= 0.0)
      return;
    float f = (float) (((double) Utils.GetAngle(this.transform.position, this.TargetObject.transform.position) + (double) UnityEngine.Random.Range(-90, 90)) * (Math.PI / 180.0));
    float distance = 4f;
    float radius = 1f;
    Vector3 Position = this.TargetObject.transform.position + new Vector3(distance * Mathf.Cos(f), distance * Mathf.Sin(f));
    RaycastHit2D raycastHit2D = Physics2D.CircleCast((Vector2) this.transform.position, radius, (Vector2) Vector3.Normalize(Position - this.transform.position), distance, (int) this.layerToCheck);
    if ((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null)
    {
      if (this.ShowDebug)
      {
        this.Points.Add(new Vector3(raycastHit2D.centroid.x, raycastHit2D.centroid.y) + Vector3.Normalize(this.transform.position - Position) * this.CircleCastOffset);
        this.PointsLink.Add(new Vector3(this.transform.position.x, this.transform.position.y));
      }
      this.StartCoroutine((IEnumerator) this.TeleportRoutine((Vector3) raycastHit2D.centroid + Vector3.Normalize(this.transform.position - Position) * this.CircleCastOffset));
    }
    else
    {
      if (this.ShowDebug)
      {
        this.EndPoints.Add(new Vector3(Position.x, Position.y));
        this.EndPointsLink.Add(new Vector3(this.transform.position.x, this.transform.position.y));
      }
      this.StartCoroutine((IEnumerator) this.TeleportRoutine(Position));
    }
  }

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.health.team && this.health.team != Health.Team.PlayerTeam)
      return;
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }

  public void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position, (float) this.VisionRange, Color.yellow);
    Utils.DrawCircleXY(this.TargetPosition, (float) this.DistanceFromTargetPosition, Color.red);
    int index1 = -1;
    while (++index1 < this.Points.Count)
    {
      Utils.DrawCircleXY(this.PointsLink[index1], 0.5f, Color.blue);
      Utils.DrawCircleXY(this.Points[index1], this.CircleCastRadius, Color.blue);
      Utils.DrawLine(this.Points[index1], this.PointsLink[index1], Color.blue);
    }
    int index2 = -1;
    while (++index2 < this.EndPoints.Count)
    {
      Utils.DrawCircleXY(this.EndPointsLink[index2], 0.5f, Color.red);
      Utils.DrawCircleXY(this.EndPoints[index2], this.CircleCastRadius, Color.red);
      Utils.DrawLine(this.EndPointsLink[index2], this.EndPoints[index2], Color.red);
    }
  }

  public enum State
  {
    Idle,
    Shooting,
    Teleporting,
    CloseCombatAttack,
  }
}
