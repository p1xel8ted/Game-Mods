// Decompiled with JetBrains decompiler
// Type: EnemySwordsman
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using FMODUnity;
using MMBiomeGeneration;
using MMTools;
using Spine.Unity;
using Spine.Unity.Examples;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class EnemySwordsman : UnitObject, IAttackResilient
{
  public int NewPositionDistance = 3;
  public float MaintainTargetDistance = 4.5f;
  public float MoveCloserDistance = 4f;
  public float AttackWithinRange = 4f;
  public bool DoubleAttack = true;
  public bool ChargeAndAttack = true;
  public bool DodgeOnHit;
  public bool canDodgeOnHit;
  public Vector2 MaxAttackDelayRandomRange = new Vector2(4f, 6f);
  public Vector2 AttackDelayRandomRange = new Vector2(0.5f, 2f);
  public ColliderEvents damageColliderEvents;
  [SerializeField]
  public bool requireLineOfSite = true;
  [SerializeField]
  public float dodgeMultiplier = 1f;
  [SerializeField]
  public float dodgeDuration = 1f;
  [SerializeField]
  public float dodgeCooldown = 1f;
  [SerializeField]
  public float pauseAfterDodgeTime;
  [SerializeField]
  public SkeletonGhost dodgeGhost;
  public SkeletonAnimation Spine;
  public SimpleSpineFlash SimpleSpineFlash;
  public float CircleCastRadius = 0.5f;
  public float CircleCastOffset = 1f;
  public float TeleportDelayTarget = 1f;
  [EventRef]
  public string attackSoundPath = string.Empty;
  [EventRef]
  public string onHitSoundPath = string.Empty;
  public ParticleSystem spawnParticles;
  [EventRef]
  public string AttackVO = string.Empty;
  [EventRef]
  public string DeathVO = string.Empty;
  [EventRef]
  public string GetHitVO = string.Empty;
  [EventRef]
  public string WarningVO = string.Empty;
  [EventRef]
  public string DashSFX = string.Empty;
  [EventRef]
  public string RollSFX = string.Empty;
  [EventRef]
  public string DrawBackBladeSFX = string.Empty;
  public GameObject TargetObject;
  public float AttackDelay;
  public bool canBeParried;
  public static float signPostParryWindow = 0.2f;
  public static float attackParryWindow = 0.15f;
  [SerializeField]
  public bool isSnowman;
  [CompilerGenerated]
  public float \u003CDamage\u003Ek__BackingField = 1f;
  [CompilerGenerated]
  public bool \u003CFollowPlayer\u003Ek__BackingField;
  public float dodgeCooldownTimer = -1f;
  public Vector3 Force;
  public float KnockbackModifier = 1f;
  [HideInInspector]
  public float Angle;
  public Vector3 TargetPosition;
  public float RepathTimer;
  public float TeleportDelay;
  public float ObstacleRaycastDelay;
  public EnemySwordsman.State MyState;
  public float MaxAttackDelay;
  public LayerMask obstacleMask;
  public Health EnemyHealth;
  public static bool SnowmanConvoTriggered = false;
  public bool ShowDebug;
  public List<Vector3> Points = new List<Vector3>();
  public List<Vector3> PointsLink = new List<Vector3>();
  public List<Vector3> EndPoints = new List<Vector3>();
  public List<Vector3> EndPointsLink = new List<Vector3>();

  public float Damage
  {
    get => this.\u003CDamage\u003Ek__BackingField;
    set => this.\u003CDamage\u003Ek__BackingField = value;
  }

  public bool FollowPlayer
  {
    get => this.\u003CFollowPlayer\u003Ek__BackingField;
    set => this.\u003CFollowPlayer\u003Ek__BackingField = value;
  }

  public override void Awake()
  {
    base.Awake();
    if ((UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null)
      this.GetComponent<Health>().totalHP *= BiomeGenerator.Instance.HumanoidHealthMultiplier;
    BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(this.BiomeGenerator_OnBiomeChangeRoom);
    if (this.isSnowman)
      this.health.DestroyOnDeath = false;
    this.canDodgeOnHit = this.DodgeOnHit;
    this.obstacleMask = (LayerMask) LayerMask.GetMask("Obstacles");
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.BiomeGenerator_OnBiomeChangeRoom);
  }

  public override void OnEnable()
  {
    this.ResetState();
    this.SeperateObject = true;
    base.OnEnable();
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.damageColliderEvents.SetActive(false);
    }
    this.StartCoroutine((IEnumerator) this.WaitForTarget());
    this.rb.simulated = true;
    this.rb.simulated = true;
  }

  public override void OnDisable()
  {
    this.health.invincible = false;
    this.SimpleSpineFlash.FlashWhite(false);
    if ((UnityEngine.Object) this.dodgeGhost != (UnityEngine.Object) null)
      this.dodgeGhost.ghostingEnabled = false;
    base.OnDisable();
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
      this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    this.ClearPaths();
    this.StopAllCoroutines();
    this.DisableForces = false;
  }

  public override void OnHitEarly(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    base.OnHitEarly(Attacker, AttackLocation, AttackType, FromBehind);
    if (PlayerController.CanParryAttacks && this.canBeParried && !FromBehind && AttackType == Health.AttackTypes.Melee && (double) this.Spine.timeScale > 1.0 / 1000.0)
    {
      this.health.WasJustParried = true;
      this.SimpleSpineFlash.FlashWhite(false);
      this.SeperateObject = true;
      this.UsePathing = true;
      this.health.invincible = false;
      this.StopAllCoroutines();
      this.DisableForces = false;
    }
    if (!((UnityEngine.Object) Attacker != (UnityEngine.Object) null) || !this.DodgeOnHit || !this.canDodgeOnHit || (double) this.dodgeCooldownTimer > 0.0 || this.state.CURRENT_STATE == StateMachine.State.RecoverFromAttack || !((UnityEngine.Object) Attacker.GetComponent<PlayerFarming>() != (UnityEngine.Object) null) || AttackType.HasFlag((Enum) Health.AttackTypes.Poison) || AttackType.HasFlag((Enum) Health.AttackTypes.Burn) || AttackType == Health.AttackTypes.Heavy || (double) this.Spine.timeScale <= 1.0 / 1000.0)
      return;
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.DodgeIE(Attacker));
  }

  public IEnumerator DodgeIE(GameObject attacker)
  {
    EnemySwordsman enemySwordsman = this;
    if (!string.IsNullOrEmpty(enemySwordsman.DashSFX))
      AudioManager.Instance.PlayOneShot(enemySwordsman.DashSFX, enemySwordsman.gameObject);
    enemySwordsman.state.LookAngle = Utils.GetAngle(enemySwordsman.transform.position, attacker.transform.position);
    enemySwordsman.Spine.skeleton.ScaleX = (double) enemySwordsman.state.LookAngle <= 90.0 || (double) enemySwordsman.state.LookAngle >= 270.0 ? -1f : 1f;
    enemySwordsman.state.LookAngle = enemySwordsman.state.facingAngle = Utils.GetAngle(enemySwordsman.transform.position, attacker.transform.position);
    enemySwordsman.dodgeCooldownTimer = enemySwordsman.dodgeCooldown;
    enemySwordsman.health.invincible = true;
    enemySwordsman.DoKnockBack(attacker, enemySwordsman.dodgeMultiplier, enemySwordsman.dodgeDuration, false);
    enemySwordsman.Spine.AnimationState.SetAnimation(0, "dodge", false);
    enemySwordsman.Spine.AnimationState.AddAnimation(0, "dodge-attack-anticipate", true, 0.0f);
    enemySwordsman.dodgeGhost.ghostingEnabled = true;
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemySwordsman.Spine.timeScale) < (double) enemySwordsman.dodgeDuration)
    {
      enemySwordsman.SimpleSpineFlash.FlashMeWhite(Mathf.Lerp(0.0f, 0.8f, time / enemySwordsman.dodgeDuration));
      yield return (object) null;
    }
    enemySwordsman.ClearPaths();
    if ((double) enemySwordsman.pauseAfterDodgeTime > 0.0)
    {
      enemySwordsman.health.invincible = false;
      yield return (object) CoroutineStatics.WaitForScaledSeconds(enemySwordsman.pauseAfterDodgeTime, enemySwordsman.Spine);
      enemySwordsman.health.invincible = true;
    }
    enemySwordsman.DoKnockBack(attacker, -enemySwordsman.dodgeMultiplier, enemySwordsman.dodgeDuration, false);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.2f, enemySwordsman.Spine);
    enemySwordsman.health.invincible = false;
    enemySwordsman.StartCoroutine((IEnumerator) enemySwordsman.FightPlayer(float.MaxValue, false, false));
    enemySwordsman.SimpleSpineFlash.FlashWhite(false);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.2f, enemySwordsman.Spine);
    enemySwordsman.dodgeGhost.ghostingEnabled = false;
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
    if (this.state.CURRENT_STATE == StateMachine.State.Flying)
      this.health.BloodOnDie = false;
    if (this.isSnowman && Health.team2.Count <= 1 && !EnemySwordsman.SnowmanConvoTriggered)
    {
      Cower component = this.GetComponent<Cower>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        component.enabled = false;
        this.health.untouchable = true;
      }
      this.health.DestroyOnDeath = false;
      this.Spine.AnimationState.SetAnimation(0, "idle", true);
      EnemySwordsman.SnowmanConvoTriggered = true;
      PlayerFarming attackingPlayer = Attacker.GetComponent<PlayerFarming>();
      if ((UnityEngine.Object) attackingPlayer == (UnityEngine.Object) null)
      {
        GameObject spellOwner = Health.GetSpellOwner(Attacker);
        attackingPlayer = !((UnityEngine.Object) spellOwner == (UnityEngine.Object) null) ? PlayerFarming.GetPlayerFarmingComponent(spellOwner) : PlayerFarming.GetPlayerFarmingComponent(Attacker);
      }
      foreach (PlayerFarming player in PlayerFarming.players)
      {
        player.state.CURRENT_STATE = StateMachine.State.InActive;
        player.state.LockStateChanges = true;
      }
      GameManager.GetInstance().StartCoroutine((IEnumerator) this.SnowmanConversationIE((System.Action) (() =>
      {
        if ((UnityEngine.Object) attackingPlayer == (UnityEngine.Object) null)
          attackingPlayer = PlayerFarming.Instance;
        this.health.untouchable = false;
        this.\u003C\u003En__0(attackingPlayer.gameObject, AttackLocation, Victim, AttackType, AttackFlags);
        DataManager.Instance.EncounteredIcegoreRoom = true;
        DataManager.Instance.ShowIcegoreRoom = false;
        foreach (PlayerFarming player in PlayerFarming.players)
        {
          player.state.LockStateChanges = false;
          player.state.CURRENT_STATE = StateMachine.State.Idle;
        }
        Interaction_Chest.Instance?.Reveal();
        RoomLockController.RoomCompleted(true);
        Health.team2.Clear();
        EnemySwordsman.SnowmanConvoTriggered = false;
        UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
      })));
    }
    else if (this.isSnowman && Health.team2.Count <= 1 && EnemySwordsman.SnowmanConvoTriggered)
    {
      this.Spine.AnimationState.SetAnimation(0, "idle", true);
    }
    else
    {
      base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
      if (!this.isSnowman)
        return;
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    if (this.health.HasShield || this.health.WasJustParried)
      return;
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
      this.damageColliderEvents.SetActive(false);
    if (!string.IsNullOrEmpty(this.onHitSoundPath))
      AudioManager.Instance.PlayOneShot(this.onHitSoundPath, this.transform.position);
    if (!string.IsNullOrEmpty(this.GetHitVO))
      AudioManager.Instance.PlayOneShot(this.GetHitVO, this.transform.position);
    this.Spine.AnimationState.SetAnimation(1, "hurt-eyes", false);
    if (this.MyState != EnemySwordsman.State.Attacking)
    {
      this.SimpleSpineFlash.FlashWhite(false);
      this.SeperateObject = true;
      this.UsePathing = true;
      this.health.invincible = false;
      this.StopAllCoroutines();
      this.DisableForces = false;
      if ((double) AttackLocation.x > (double) this.transform.position.x && this.state.CURRENT_STATE != StateMachine.State.HitRight)
        this.state.CURRENT_STATE = StateMachine.State.HitRight;
      if ((double) AttackLocation.x < (double) this.transform.position.x && this.state.CURRENT_STATE != StateMachine.State.HitLeft)
        this.state.CURRENT_STATE = StateMachine.State.HitLeft;
      if (AttackType != Health.AttackTypes.Heavy && (!(AttackType == Health.AttackTypes.Projectile & FromBehind) || this.health.HasShield))
        this.StartCoroutine((IEnumerator) this.HurtRoutine());
    }
    if (AttackType == Health.AttackTypes.Projectile && !this.health.HasShield)
    {
      this.state.facingAngle = this.state.LookAngle = Utils.GetAngle(this.transform.position, Attacker.transform.position);
      this.Spine.skeleton.ScaleX = (double) this.state.LookAngle <= 90.0 || (double) this.state.LookAngle >= 270.0 ? -1f : 1f;
    }
    if (AttackType != Health.AttackTypes.NoKnockBack)
      this.StartCoroutine((IEnumerator) this.ApplyForceRoutine(Attacker));
    this.SimpleSpineFlash.FlashFillRed();
  }

  public virtual IEnumerator ApplyForceRoutine(GameObject Attacker)
  {
    EnemySwordsman enemySwordsman = this;
    enemySwordsman.DisableForces = true;
    enemySwordsman.Force = (enemySwordsman.transform.position - Attacker.transform.position).normalized * 500f;
    enemySwordsman.rb.AddForce((Vector2) (enemySwordsman.Force * enemySwordsman.KnockbackModifier));
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemySwordsman.Spine.timeScale) < 0.5)
      yield return (object) null;
    enemySwordsman.DisableForces = false;
  }

  public IEnumerator HurtRoutine()
  {
    EnemySwordsman enemySwordsman = this;
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemySwordsman.Spine.timeScale) < 0.30000001192092896)
      yield return (object) null;
    enemySwordsman.StartCoroutine((IEnumerator) enemySwordsman.WaitForTarget());
  }

  public IEnumerator WaitForTarget()
  {
    EnemySwordsman enemySwordsman = this;
    enemySwordsman.Spine.Initialize(false);
    while (!GameManager.RoomActive)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    while ((UnityEngine.Object) enemySwordsman.TargetObject == (UnityEngine.Object) null)
    {
      Health closestTarget = enemySwordsman.GetClosestTarget(enemySwordsman.health.team == Health.Team.PlayerTeam);
      if ((bool) (UnityEngine.Object) closestTarget)
      {
        enemySwordsman.TargetObject = closestTarget.gameObject;
        enemySwordsman.requireLineOfSite = false;
        enemySwordsman.VisionRange = int.MaxValue;
      }
      enemySwordsman.RepathTimer -= Time.deltaTime * enemySwordsman.Spine.timeScale;
      if ((double) enemySwordsman.RepathTimer <= 0.0)
      {
        if (enemySwordsman.state.CURRENT_STATE == StateMachine.State.Moving)
        {
          if (enemySwordsman.Spine.AnimationName != "run")
            enemySwordsman.Spine.AnimationState.SetAnimation(0, "run", true);
        }
        else if (enemySwordsman.Spine.AnimationName != "idle")
          enemySwordsman.Spine.AnimationState.SetAnimation(0, "idle", true);
        if (!enemySwordsman.FollowPlayer)
          enemySwordsman.TargetPosition = enemySwordsman.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 2f;
        enemySwordsman.FindPath(enemySwordsman.TargetPosition);
        enemySwordsman.state.LookAngle = Utils.GetAngle(enemySwordsman.transform.position, enemySwordsman.TargetPosition);
        enemySwordsman.Spine.skeleton.ScaleX = (double) enemySwordsman.state.LookAngle <= 90.0 || (double) enemySwordsman.state.LookAngle >= 270.0 ? -1f : 1f;
      }
      yield return (object) null;
    }
    bool InRange = false;
    while (!InRange)
    {
      if ((UnityEngine.Object) enemySwordsman.TargetObject == (UnityEngine.Object) null)
      {
        enemySwordsman.StartCoroutine((IEnumerator) enemySwordsman.WaitForTarget());
        yield break;
      }
      float a = Vector3.Distance(enemySwordsman.TargetObject.transform.position, enemySwordsman.transform.position);
      if ((double) a <= (double) enemySwordsman.VisionRange)
      {
        if (!enemySwordsman.requireLineOfSite || enemySwordsman.CheckLineOfSightOnTarget(enemySwordsman.TargetObject, enemySwordsman.TargetObject.transform.position, Mathf.Min(a, (float) enemySwordsman.VisionRange)))
          InRange = true;
        else if ((double) enemySwordsman.Spine.timeScale > 1.0 / 1000.0)
          enemySwordsman.LookAtTarget();
      }
      yield return (object) null;
    }
    enemySwordsman.StartCoroutine((IEnumerator) enemySwordsman.ChaseTarget());
  }

  public override void Update()
  {
    base.Update();
    if ((double) this.dodgeCooldownTimer <= 0.0)
      return;
    this.dodgeCooldownTimer -= Time.deltaTime * this.Spine.timeScale;
    if ((double) this.dodgeCooldownTimer > 0.0)
      return;
    this.dodgeCooldownTimer = -1f;
  }

  public void LookAtTarget()
  {
    if ((UnityEngine.Object) this.GetClosestTarget() == (UnityEngine.Object) null || (double) this.Spine.timeScale < 1.0 / 1000.0)
      return;
    float angle = Utils.GetAngle(this.transform.position, this.GetClosestTarget().transform.position);
    this.state.facingAngle = angle;
    this.state.LookAngle = angle;
    if (!(this.Spine.AnimationName != "jeer"))
      return;
    this.Spine.randomOffset = true;
    this.Spine.AnimationState.SetAnimation(0, "jeer", true);
  }

  public void DoCustomBoneWallCheck()
  {
    this.ObstacleRaycastDelay = 0.2f;
    foreach (RaycastHit2D raycastHit2D in Physics2D.RaycastAll((Vector2) this.transform.position, (Vector2) (Vector3) Utils.DegreeToVector2(this.state.facingAngle), 1.5f, (int) this.obstacleMask))
    {
      Health component = raycastHit2D.transform.GetComponent<Health>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.team != this.health.team)
      {
        this.TargetObject = component.gameObject;
        break;
      }
    }
  }

  public IEnumerator ChaseTarget()
  {
    EnemySwordsman enemySwordsman = this;
    enemySwordsman.MyState = EnemySwordsman.State.WaitAndTaunt;
    enemySwordsman.state.CURRENT_STATE = StateMachine.State.Idle;
    enemySwordsman.AttackDelay = UnityEngine.Random.Range(enemySwordsman.AttackDelayRandomRange.x, enemySwordsman.AttackDelayRandomRange.y);
    if (enemySwordsman.health.HasShield)
      enemySwordsman.AttackDelay = 2.5f;
    enemySwordsman.MaxAttackDelay = UnityEngine.Random.Range(enemySwordsman.MaxAttackDelayRandomRange.x, enemySwordsman.MaxAttackDelayRandomRange.y);
    bool Loop = true;
    while (Loop)
    {
      if ((UnityEngine.Object) enemySwordsman.TargetObject == (UnityEngine.Object) null)
      {
        enemySwordsman.StartCoroutine((IEnumerator) enemySwordsman.WaitForTarget());
        break;
      }
      if ((UnityEngine.Object) enemySwordsman.damageColliderEvents != (UnityEngine.Object) null)
        enemySwordsman.damageColliderEvents.SetActive(false);
      if (enemySwordsman.health.team == Health.Team.PlayerTeam && (double) (enemySwordsman.ObstacleRaycastDelay -= Time.deltaTime * enemySwordsman.Spine.timeScale) <= 0.0)
        enemySwordsman.DoCustomBoneWallCheck();
      enemySwordsman.TeleportDelay -= Time.deltaTime * enemySwordsman.Spine.timeScale;
      enemySwordsman.AttackDelay -= Time.deltaTime * enemySwordsman.Spine.timeScale;
      enemySwordsman.MaxAttackDelay -= Time.deltaTime * enemySwordsman.Spine.timeScale;
      if (enemySwordsman.MyState == EnemySwordsman.State.WaitAndTaunt)
      {
        if (enemySwordsman.Spine.AnimationName != "roll-stop" && enemySwordsman.state.CURRENT_STATE == StateMachine.State.Moving)
        {
          if (enemySwordsman.Spine.AnimationName != "run")
            enemySwordsman.Spine.AnimationState.SetAnimation(0, "run", true);
        }
        else if (enemySwordsman.Spine.AnimationName != "cheer1")
          enemySwordsman.Spine.AnimationState.SetAnimation(0, "cheer1", true);
        if ((UnityEngine.Object) enemySwordsman.TargetObject == (UnityEngine.Object) PlayerFarming.Instance.gameObject && enemySwordsman.health.IsCharmed && (UnityEngine.Object) enemySwordsman.GetClosestTarget() != (UnityEngine.Object) null)
          enemySwordsman.TargetObject = enemySwordsman.GetClosestTarget().gameObject;
        if ((double) enemySwordsman.Spine.timeScale > 1.0 / 1000.0)
        {
          enemySwordsman.state.LookAngle = Utils.GetAngle(enemySwordsman.transform.position, enemySwordsman.TargetObject.transform.position);
          enemySwordsman.Spine.skeleton.ScaleX = (double) enemySwordsman.state.LookAngle <= 90.0 || (double) enemySwordsman.state.LookAngle >= 270.0 ? -1f : 1f;
        }
        if (enemySwordsman.state.CURRENT_STATE == StateMachine.State.Idle)
        {
          if ((double) (enemySwordsman.RepathTimer -= Time.deltaTime * enemySwordsman.Spine.timeScale) < 0.0)
          {
            if (enemySwordsman.CustomAttackLogic())
              break;
            if ((double) enemySwordsman.MaxAttackDelay < 0.0 || (double) Vector3.Distance(enemySwordsman.transform.position, enemySwordsman.TargetObject.transform.position) < (double) enemySwordsman.AttackWithinRange)
            {
              enemySwordsman.AttackWithinRange = float.MaxValue;
              if ((bool) (UnityEngine.Object) enemySwordsman.TargetObject)
              {
                if (enemySwordsman.ChargeAndAttack && ((double) enemySwordsman.MaxAttackDelay < 0.0 || (double) enemySwordsman.AttackDelay < 0.0))
                {
                  enemySwordsman.health.invincible = false;
                  enemySwordsman.StopAllCoroutines();
                  enemySwordsman.DisableForces = false;
                  enemySwordsman.StartCoroutine((IEnumerator) enemySwordsman.FightPlayer());
                }
                else if (!enemySwordsman.health.HasShield)
                {
                  enemySwordsman.Angle = (float) (((double) Utils.GetAngle(enemySwordsman.TargetObject.transform.position, enemySwordsman.transform.position) + (double) UnityEngine.Random.Range(-20, 20)) * (Math.PI / 180.0));
                  enemySwordsman.TargetPosition = enemySwordsman.TargetObject.transform.position + new Vector3(enemySwordsman.MaintainTargetDistance * Mathf.Cos(enemySwordsman.Angle), enemySwordsman.MaintainTargetDistance * Mathf.Sin(enemySwordsman.Angle));
                  enemySwordsman.FindPath(enemySwordsman.TargetPosition);
                }
              }
            }
            else if ((bool) (UnityEngine.Object) enemySwordsman.TargetObject && (double) Vector3.Distance(enemySwordsman.transform.position, enemySwordsman.TargetObject.transform.position) > (double) enemySwordsman.MoveCloserDistance + (enemySwordsman.health.HasShield ? 0.0 : 1.0))
            {
              enemySwordsman.Angle = (float) (((double) Utils.GetAngle(enemySwordsman.TargetObject.transform.position, enemySwordsman.transform.position) + (double) UnityEngine.Random.Range(-20, 20)) * (Math.PI / 180.0));
              enemySwordsman.TargetPosition = enemySwordsman.TargetObject.transform.position + new Vector3(enemySwordsman.MaintainTargetDistance * Mathf.Cos(enemySwordsman.Angle), enemySwordsman.MaintainTargetDistance * Mathf.Sin(enemySwordsman.Angle));
              enemySwordsman.FindPath(enemySwordsman.TargetPosition);
            }
          }
        }
        else if ((double) (enemySwordsman.RepathTimer += Time.deltaTime * enemySwordsman.Spine.timeScale) > 2.0)
        {
          enemySwordsman.RepathTimer = 0.0f;
          enemySwordsman.state.CURRENT_STATE = StateMachine.State.Idle;
        }
      }
      enemySwordsman.Seperate(0.5f);
      yield return (object) null;
    }
  }

  public override void BeAlarmed(GameObject TargetObject)
  {
    base.BeAlarmed(TargetObject);
    if (!string.IsNullOrEmpty(this.WarningVO))
      AudioManager.Instance.PlayOneShot(this.WarningVO, this.transform.position);
    this.TargetObject = TargetObject;
    float a = Vector3.Distance(TargetObject.transform.position, this.transform.position);
    if ((double) a > (double) this.VisionRange)
      return;
    if (!this.requireLineOfSite || this.CheckLineOfSightOnTarget(TargetObject, TargetObject.transform.position, Mathf.Min(a, (float) this.VisionRange)))
    {
      this.StartCoroutine((IEnumerator) this.WaitForTarget());
    }
    else
    {
      if ((double) this.Spine.timeScale <= 1.0 / 1000.0)
        return;
      this.LookAtTarget();
    }
  }

  public virtual bool CustomAttackLogic() => false;

  public void FindPath(Vector3 PointToCheck)
  {
    this.RepathTimer = 0.2f;
    RaycastHit2D raycastHit2D = Physics2D.CircleCast((Vector2) this.transform.position, 0.2f, (Vector2) Vector3.Normalize(PointToCheck - this.transform.position), (float) this.NewPositionDistance, (int) this.layerToCheck);
    if ((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null)
    {
      if ((double) Vector3.Distance(this.transform.position, (Vector3) raycastHit2D.centroid) > 1.0)
      {
        if (this.ShowDebug)
        {
          this.Points.Add(new Vector3(raycastHit2D.centroid.x, raycastHit2D.centroid.y) + Vector3.Normalize(this.transform.position - PointToCheck) * this.CircleCastOffset);
          this.PointsLink.Add(new Vector3(this.transform.position.x, this.transform.position.y));
        }
        this.TargetPosition = (Vector3) raycastHit2D.centroid + Vector3.Normalize(this.transform.position - PointToCheck) * this.CircleCastOffset;
        this.givePath(this.TargetPosition);
      }
      else
      {
        if ((double) this.TeleportDelay >= 0.0)
          return;
        this.Teleport();
      }
    }
    else if (this.FollowPlayer && (UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
    {
      if ((double) Vector3.Distance(this.transform.position, PlayerFarming.Instance.transform.position) <= 1.5)
        return;
      this.TargetPosition = PlayerFarming.Instance.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle;
      this.givePath(this.TargetPosition);
    }
    else
    {
      this.TargetPosition = PointToCheck;
      this.givePath(PointToCheck);
    }
  }

  public void Teleport()
  {
    if (this.MyState != EnemySwordsman.State.WaitAndTaunt || (double) this.health.HP <= 0.0)
      return;
    float num1 = 100f;
    float num2;
    if ((double) (num2 = num1 - 1f) <= 0.0 || (UnityEngine.Object) this.TargetObject == (UnityEngine.Object) null)
      return;
    float f = (float) (((double) Utils.GetAngle(this.transform.position, this.TargetObject.transform.position) + (double) UnityEngine.Random.Range(-90, 90)) * (Math.PI / 180.0));
    float distance = 4.5f;
    float radius = 0.2f;
    Vector3 Position = this.TargetObject.transform.position + new Vector3(distance * Mathf.Cos(f), distance * Mathf.Sin(f));
    RaycastHit2D raycastHit2D = Physics2D.CircleCast((Vector2) this.transform.position, radius, (Vector2) Vector3.Normalize(Position - this.transform.position), distance, (int) this.layerToCheck);
    if ((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null)
    {
      if (this.ShowDebug)
      {
        this.Points.Add(new Vector3(raycastHit2D.centroid.x, raycastHit2D.centroid.y) + Vector3.Normalize(this.transform.position - Position) * this.CircleCastOffset);
        this.PointsLink.Add(new Vector3(this.transform.position.x, this.transform.position.y));
      }
      this.StartCoroutine((IEnumerator) this.TeleportRoutine((Vector3) raycastHit2D.centroid));
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

  public IEnumerator TeleportRoutine(Vector3 Position)
  {
    EnemySwordsman enemySwordsman = this;
    enemySwordsman.ClearPaths();
    enemySwordsman.state.CURRENT_STATE = StateMachine.State.Moving;
    enemySwordsman.UsePathing = false;
    enemySwordsman.health.invincible = true;
    enemySwordsman.SeperateObject = false;
    enemySwordsman.MyState = EnemySwordsman.State.Teleporting;
    enemySwordsman.ClearPaths();
    Vector3 position = enemySwordsman.transform.position;
    float Progress = 0.0f;
    enemySwordsman.Spine.AnimationState.SetAnimation(0, "roll", true);
    enemySwordsman.state.facingAngle = enemySwordsman.state.LookAngle = Utils.GetAngle(enemySwordsman.transform.position, Position);
    enemySwordsman.Spine.skeleton.ScaleX = (double) enemySwordsman.state.LookAngle <= 90.0 || (double) enemySwordsman.state.LookAngle >= 270.0 ? -1f : 1f;
    Vector3 b = Position;
    float Duration = Vector3.Distance(position, b) / 10f;
    if (!string.IsNullOrEmpty(enemySwordsman.RollSFX))
      AudioManager.Instance.PlayOneShot(enemySwordsman.RollSFX, enemySwordsman.gameObject);
    while ((double) (Progress += Time.deltaTime * enemySwordsman.Spine.timeScale) < (double) Duration)
    {
      enemySwordsman.speed = 10f * Time.deltaTime * enemySwordsman.Spine.timeScale;
      yield return (object) null;
    }
    enemySwordsman.state.CURRENT_STATE = StateMachine.State.Idle;
    enemySwordsman.Spine.AnimationState.SetAnimation(0, "roll-stop", false);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemySwordsman.Spine.timeScale) < 0.30000001192092896)
      yield return (object) null;
    enemySwordsman.UsePathing = true;
    enemySwordsman.RepathTimer = 0.5f;
    enemySwordsman.TeleportDelay = enemySwordsman.TeleportDelayTarget;
    enemySwordsman.SeperateObject = true;
    enemySwordsman.health.invincible = false;
    enemySwordsman.MyState = EnemySwordsman.State.WaitAndTaunt;
  }

  public void GraveSpawn(bool longAnim = false, bool waitForEndOfFrame = true)
  {
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.GraveSpawnRoutine(longAnim, waitForEndOfFrame));
  }

  public IEnumerator GraveSpawnRoutine(bool longAnim = false, bool waitForEndOfFrame = true)
  {
    EnemySwordsman enemySwordsman = this;
    enemySwordsman.health.invincible = true;
    enemySwordsman.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    if (waitForEndOfFrame)
      yield return (object) new WaitForEndOfFrame();
    if ((UnityEngine.Object) enemySwordsman.spawnParticles != (UnityEngine.Object) null)
      enemySwordsman.spawnParticles.Play();
    enemySwordsman.Spine.AnimationState.SetAnimation(0, longAnim ? "grave-spawn-long" : "grave-spawn", false);
    enemySwordsman.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(2.33f, enemySwordsman.Spine);
    enemySwordsman.health.invincible = false;
    enemySwordsman.state.CURRENT_STATE = StateMachine.State.Idle;
    enemySwordsman.StartCoroutine((IEnumerator) enemySwordsman.WaitForTarget());
  }

  public void OnDrawGizmos()
  {
    Gizmos.color = Color.yellow;
    Gizmos.DrawLine(this.transform.position, this.transform.position + (Vector3) Utils.DegreeToVector2(this.state.facingAngle) * 5f);
  }

  public IEnumerator FightPlayer(float AttackDistance = 1.5f, bool signpost = true, bool canChangeDirection = true)
  {
    EnemySwordsman enemySwordsman = this;
    if ((UnityEngine.Object) enemySwordsman.TargetObject == (UnityEngine.Object) null)
    {
      enemySwordsman.StartCoroutine((IEnumerator) enemySwordsman.WaitForTarget());
    }
    else
    {
      enemySwordsman.MyState = EnemySwordsman.State.Attacking;
      enemySwordsman.UsePathing = true;
      enemySwordsman.givePath(enemySwordsman.TargetObject.transform.position);
      enemySwordsman.Spine.AnimationState.SetAnimation(0, "run-charge", true);
      if (!string.IsNullOrEmpty(enemySwordsman.DrawBackBladeSFX))
        AudioManager.Instance.PlayOneShot(enemySwordsman.DrawBackBladeSFX, enemySwordsman.gameObject);
      enemySwordsman.RepathTimer = 0.0f;
      int NumAttacks = enemySwordsman.DoubleAttack ? 2 : 1;
      int AttackCount = 1;
      float MaxAttackSpeed = 15f;
      float AttackSpeed = MaxAttackSpeed;
      bool Loop = true;
      float SignPostDelay = 0.5f;
      if (!signpost)
        SignPostDelay = 0.0f;
      while (Loop)
      {
        if ((UnityEngine.Object) enemySwordsman.Spine == (UnityEngine.Object) null || enemySwordsman.Spine.AnimationState == null || enemySwordsman.Spine.Skeleton == null)
          yield return (object) null;
        else if (enemySwordsman.isSnowman && EnemySwordsman.SnowmanConvoTriggered)
        {
          yield return (object) null;
        }
        else
        {
          enemySwordsman.Seperate(0.5f);
          switch (enemySwordsman.state.CURRENT_STATE)
          {
            case StateMachine.State.Idle:
              enemySwordsman.TargetObject = (GameObject) null;
              enemySwordsman.StartCoroutine((IEnumerator) enemySwordsman.WaitForTarget());
              yield break;
            case StateMachine.State.Moving:
              if ((bool) (UnityEngine.Object) enemySwordsman.TargetObject & canChangeDirection && (double) enemySwordsman.Spine.timeScale > 1.0 / 1000.0)
              {
                enemySwordsman.state.LookAngle = Utils.GetAngle(enemySwordsman.transform.position, enemySwordsman.TargetObject.transform.position);
                enemySwordsman.Spine.skeleton.ScaleX = (double) enemySwordsman.state.LookAngle <= 90.0 || (double) enemySwordsman.state.LookAngle >= 270.0 ? -1f : 1f;
                enemySwordsman.state.LookAngle = enemySwordsman.state.facingAngle = Utils.GetAngle(enemySwordsman.transform.position, enemySwordsman.TargetObject.transform.position);
              }
              if (enemySwordsman.health.team == Health.Team.PlayerTeam && (double) (enemySwordsman.ObstacleRaycastDelay -= Time.deltaTime * enemySwordsman.Spine.timeScale) <= 0.0)
                enemySwordsman.DoCustomBoneWallCheck();
              if ((UnityEngine.Object) enemySwordsman.TargetObject != (UnityEngine.Object) null && (double) Vector2.Distance((Vector2) enemySwordsman.transform.position, (Vector2) enemySwordsman.TargetObject.transform.position) < (double) AttackDistance)
              {
                enemySwordsman.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
                enemySwordsman.Spine.AnimationState.SetAnimation(0, AttackCount == NumAttacks ? "grunt-attack-charge2" : "grunt-attack-charge", false);
              }
              else
              {
                if ((double) (enemySwordsman.RepathTimer += Time.deltaTime * enemySwordsman.Spine.timeScale) > 0.20000000298023224 && (bool) (UnityEngine.Object) enemySwordsman.TargetObject)
                {
                  enemySwordsman.RepathTimer = 0.0f;
                  enemySwordsman.givePath(enemySwordsman.TargetObject.transform.position);
                }
                if ((UnityEngine.Object) enemySwordsman.damageColliderEvents != (UnityEngine.Object) null)
                {
                  if ((double) enemySwordsman.state.Timer < 0.20000000298023224 && !enemySwordsman.health.WasJustParried)
                    enemySwordsman.damageColliderEvents.SetActive(true);
                  else
                    enemySwordsman.damageColliderEvents.SetActive(false);
                }
              }
              if ((UnityEngine.Object) enemySwordsman.damageColliderEvents != (UnityEngine.Object) null)
              {
                enemySwordsman.damageColliderEvents.SetActive(false);
                break;
              }
              break;
            case StateMachine.State.SignPostAttack:
              if ((UnityEngine.Object) enemySwordsman.damageColliderEvents != (UnityEngine.Object) null)
                enemySwordsman.damageColliderEvents.SetActive(false);
              enemySwordsman.SimpleSpineFlash.FlashWhite(enemySwordsman.state.Timer / SignPostDelay);
              enemySwordsman.state.Timer += Time.deltaTime * enemySwordsman.Spine.timeScale;
              if ((double) enemySwordsman.state.Timer >= (double) SignPostDelay - (double) EnemySwordsman.signPostParryWindow)
                enemySwordsman.canBeParried = true;
              if ((double) enemySwordsman.state.Timer >= (double) SignPostDelay)
              {
                enemySwordsman.SimpleSpineFlash.FlashWhite(false);
                CameraManager.shakeCamera(0.4f, enemySwordsman.state.LookAngle);
                enemySwordsman.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
                enemySwordsman.speed = AttackSpeed * 0.0166666675f;
                enemySwordsman.Spine.AnimationState.SetAnimation(0, AttackCount == NumAttacks ? "grunt-attack-impact2" : "grunt-attack-impact", false);
                enemySwordsman.canBeParried = true;
                enemySwordsman.StartCoroutine((IEnumerator) enemySwordsman.EnableDamageCollider(0.0f));
                if (!string.IsNullOrEmpty(enemySwordsman.attackSoundPath))
                  AudioManager.Instance.PlayOneShot(enemySwordsman.attackSoundPath, enemySwordsman.transform.position);
                if (!string.IsNullOrEmpty(enemySwordsman.AttackVO))
                {
                  AudioManager.Instance.PlayOneShot(enemySwordsman.AttackVO, enemySwordsman.transform.position);
                  break;
                }
                break;
              }
              break;
            case StateMachine.State.RecoverFromAttack:
              if ((double) AttackSpeed > 0.0)
                AttackSpeed -= 1f * GameManager.DeltaTime * enemySwordsman.Spine.timeScale;
              enemySwordsman.speed = AttackSpeed * Time.deltaTime * enemySwordsman.Spine.timeScale;
              enemySwordsman.SimpleSpineFlash.FlashWhite(false);
              enemySwordsman.canBeParried = (double) enemySwordsman.state.Timer <= (double) EnemySwordsman.attackParryWindow;
              if ((double) (enemySwordsman.state.Timer += Time.deltaTime * enemySwordsman.Spine.timeScale) >= (AttackCount + 1 <= NumAttacks ? 0.5 : 1.0))
              {
                if (++AttackCount <= NumAttacks)
                {
                  AttackSpeed = MaxAttackSpeed + (float) ((3 - NumAttacks) * 2);
                  enemySwordsman.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
                  enemySwordsman.Spine.AnimationState.SetAnimation(0, "grunt-attack-charge2", false);
                  SignPostDelay = 0.3f;
                  break;
                }
                Loop = false;
                enemySwordsman.SimpleSpineFlash.FlashWhite(false);
                break;
              }
              break;
          }
          yield return (object) null;
        }
      }
      enemySwordsman.TargetObject = (GameObject) null;
      enemySwordsman.StartCoroutine((IEnumerator) enemySwordsman.WaitForTarget());
    }
  }

  public void BiomeGenerator_OnBiomeChangeRoom()
  {
    if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null) || !this.FollowPlayer)
      return;
    this.ClearPaths();
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.PlaceIE());
  }

  public IEnumerator PlaceIE()
  {
    EnemySwordsman enemySwordsman = this;
    enemySwordsman.ClearPaths();
    Vector3 offset = (Vector3) UnityEngine.Random.insideUnitCircle;
    while (PlayerFarming.Instance.GoToAndStopping)
    {
      enemySwordsman.state.CURRENT_STATE = StateMachine.State.Moving;
      Vector3 vector3 = (PlayerFarming.Instance.transform.position + offset) with
      {
        z = 0.0f
      };
      enemySwordsman.transform.position = vector3;
      yield return (object) null;
    }
    enemySwordsman.state.CURRENT_STATE = StateMachine.State.Idle;
    enemySwordsman.Spine.AnimationState.SetAnimation(0, "idle-enemy", true);
  }

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    this.EnemyHealth = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) this.EnemyHealth != (UnityEngine.Object) null))
      return;
    if (this.EnemyHealth.team != this.health.team)
    {
      this.EnemyHealth.DealDamage(this.Damage, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f));
    }
    else
    {
      if (this.health.team != Health.Team.PlayerTeam || this.health.isPlayerAlly || this.EnemyHealth.isPlayer)
        return;
      this.EnemyHealth.DealDamage(this.Damage, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f));
    }
  }

  public IEnumerator SnowmanConversationIE(System.Action callback)
  {
    EnemySwordsman enemySwordsman = this;
    enemySwordsman.ClearPaths();
    enemySwordsman.enabled = false;
    List<ConversationEntry> Entries = new List<ConversationEntry>()
    {
      new ConversationEntry(enemySwordsman.gameObject, "Conversation_NPC/Snowman/Enemy/0"),
      new ConversationEntry(enemySwordsman.gameObject, "Conversation_NPC/Snowman/Enemy/1")
    };
    foreach (ConversationEntry conversationEntry in Entries)
      conversationEntry.SkeletonData = enemySwordsman.Spine;
    MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) null));
    yield return (object) null;
    while (MMConversation.isPlaying)
    {
      enemySwordsman.enabled = false;
      yield return (object) null;
    }
    System.Action action = callback;
    if (action != null)
      action();
  }

  public IEnumerator EnableDamageCollider(float initialDelay)
  {
    if ((bool) (UnityEngine.Object) this.damageColliderEvents)
    {
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * this.Spine.timeScale) < (double) initialDelay)
        yield return (object) null;
      this.damageColliderEvents.SetActive(true);
      time = 0.0f;
      while ((double) (time += Time.deltaTime * this.Spine.timeScale) < 0.20000000298023224)
        yield return (object) null;
      this.damageColliderEvents.SetActive(false);
    }
  }

  public void ResetResilience() => this.canDodgeOnHit = this.DodgeOnHit;

  public void StopResilience() => this.canDodgeOnHit = false;

  public void ResetState()
  {
    this.enabled = true;
    this.health.invincible = false;
    this.transform.DOKill();
    this.transform.position = this.transform.position with
    {
      z = 0.0f
    };
    this.Spine.AnimationState.SetAnimation(0, "idle", true);
    if ((UnityEngine.Object) this.state != (UnityEngine.Object) null)
    {
      this.state.CURRENT_STATE = StateMachine.State.Idle;
      this.state.LockStateChanges = false;
      this.state.facingAngle = (float) UnityEngine.Random.Range(0, 360);
    }
    this.TargetObject = (GameObject) null;
    this.ClearPaths();
  }

  [CompilerGenerated]
  [DebuggerHidden]
  public void \u003C\u003En__0(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
  }

  public enum State
  {
    WaitAndTaunt,
    Teleporting,
    Attacking,
  }
}
