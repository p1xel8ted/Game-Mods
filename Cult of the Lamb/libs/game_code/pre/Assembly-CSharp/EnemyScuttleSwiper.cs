// Decompiled with JetBrains decompiler
// Type: EnemyScuttleSwiper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using FMODUnity;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnemyScuttleSwiper : UnitObject
{
  private static List<EnemyScuttleSwiper> Scuttlers = new List<EnemyScuttleSwiper>();
  public EnemyScuttleSwiper.StartingStates StartHidden;
  public bool DetectPlayerWhileHidden = true;
  public bool HiddenOffsetIsGlobalPosition;
  public Vector3 HiddenOffset = Vector3.zero;
  public float HiddenRadius = 5f;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string UnawareAnimation;
  public ColliderEvents damageColliderEvents;
  public SkeletonAnimation Spine;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string IdleAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string MovingAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string SignPostAttackAnimation;
  public bool LoopSignPostAttackAnimation = true;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string AttackAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string FallAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string LandAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string SignPostSlamAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string SlamAnimation;
  public SpriteRenderer ShadowSpriteRenderer;
  public SimpleSpineFlash[] SimpleSpineFlashes;
  public float KnockbackModifier = 1f;
  public int NumberOfAttacks = 1;
  public float AttackForceModifier = 1f;
  public bool CounterAttack;
  public bool SlamAttack;
  public bool CanBeInterrupted = true;
  public bool AttackTowardsPlayer;
  public float DamageColliderDuration = -1f;
  [Range(0.0f, 1f)]
  public float ChanceToPathTowardsPlayer;
  public int DistanceToPathTowardsPlayer = 6;
  public float SlamAttackRange;
  public float TimeBetweenSlams;
  public GameObject SlamRockPrefab;
  private float SlamTimer;
  public SkeletonAnimation warningIcon;
  [EventRef]
  public string AttackVO = string.Empty;
  [EventRef]
  public string DeathVO = string.Empty;
  [EventRef]
  public string GetHitVO = string.Empty;
  [EventRef]
  public string WarningVO = string.Empty;
  protected GameObject TargetObject;
  private float RandomDirection;
  private bool ShownWarning;
  private float GravitySpeed = 1f;
  private float HidingHeight = 5f;
  public float AttackDelayTime;
  protected bool Attacking;
  protected bool IsStunned;
  [HideInInspector]
  public float AttackDelay;
  public float AttackDuration = 1f;
  public float SignPostAttackDuration = 0.5f;
  public bool DisableKnockback;
  private float Angle;
  private Vector3 Force;
  public float TurningArc = 90f;
  public Vector2 DistanceRange = new Vector2(1f, 3f);
  public Vector2 IdleWaitRange = new Vector2(1f, 3f);
  protected float IdleWait;
  private bool PathingToPlayer;
  protected Health EnemyHealth;
  public float CircleCastRadius = 0.5f;
  public float CircleCastOffset = 1f;
  public bool ShowDebug;
  public List<Vector3> Points = new List<Vector3>();
  public List<Vector3> PointsLink = new List<Vector3>();
  public List<Vector3> EndPoints = new List<Vector3>();
  public List<Vector3> EndPointsLink = new List<Vector3>();

  public override void Awake()
  {
    base.Awake();
    this.SimpleSpineFlashes = this.GetComponentsInChildren<SimpleSpineFlash>();
  }

  public override void OnEnable()
  {
    base.OnEnable();
    EnemyScuttleSwiper.Scuttlers.Add(this);
    this.SlamTimer = this.TimeBetweenSlams;
    this.RandomDirection = (float) UnityEngine.Random.Range(0, 360) * ((float) Math.PI / 180f);
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.damageColliderEvents.SetActive(false);
    }
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    if (GameManager.RoomActive)
    {
      this.IdleWait = 0.0f;
      this.TargetObject = (GameObject) null;
      this.Attacking = false;
      this.IsStunned = false;
      foreach (SimpleSpineFlash simpleSpineFlash in this.SimpleSpineFlashes)
        simpleSpineFlash.FlashWhite(false);
      this.StartCoroutine((IEnumerator) this.ActiveRoutine());
    }
    else
    {
      switch (this.StartHidden)
      {
        case EnemyScuttleSwiper.StartingStates.Hidden:
          this.StartCoroutine((IEnumerator) this.Hidden());
          break;
        case EnemyScuttleSwiper.StartingStates.Wandering:
          this.StartCoroutine((IEnumerator) this.ActiveRoutine());
          break;
        case EnemyScuttleSwiper.StartingStates.Animation:
          this.StartCoroutine((IEnumerator) this.AnimationRoutine());
          break;
      }
    }
  }

  public override void OnDisable()
  {
    base.OnDisable();
    EnemyScuttleSwiper.Scuttlers.Remove(this);
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.SetActive(false);
      this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    }
    this.ClearPaths();
    this.StopAllCoroutines();
    this.DisableForces = false;
    foreach (SimpleSpineFlash simpleSpineFlash in this.SimpleSpineFlashes)
      simpleSpineFlash.FlashWhite(false);
  }

  public void ShowWarningIcon()
  {
    if ((UnityEngine.Object) this.warningIcon == (UnityEngine.Object) null || this.ShownWarning)
      return;
    this.warningIcon.AnimationState.SetAnimation(0, "warn-start", false);
    this.warningIcon.AnimationState.AddAnimation(0, "warn-stop", false, 2f);
    this.ShownWarning = true;
    if (string.IsNullOrEmpty(this.WarningVO))
      return;
    AudioManager.Instance.PlayOneShot(this.WarningVO, this.transform.position);
  }

  private IEnumerator AnimationRoutine()
  {
    EnemyScuttleSwiper enemyScuttleSwiper = this;
    yield return (object) new WaitForEndOfFrame();
    enemyScuttleSwiper.health.invincible = true;
    Debug.Log((object) ("Spine " + (object) enemyScuttleSwiper.Spine));
    Debug.Log((object) ("Spine.AnimationState " + (object) enemyScuttleSwiper.Spine.AnimationState));
    Debug.Log((object) ("UnawareAnimation " + enemyScuttleSwiper.UnawareAnimation));
    enemyScuttleSwiper.Spine.AnimationState.SetAnimation(0, enemyScuttleSwiper.UnawareAnimation, true);
    while (GameManager.RoomActive)
      yield return (object) null;
    while ((UnityEngine.Object) enemyScuttleSwiper.TargetObject == (UnityEngine.Object) null)
    {
      if (Time.frameCount % 5 == 0)
        enemyScuttleSwiper.GetNewTarget();
      yield return (object) null;
    }
    while ((UnityEngine.Object) enemyScuttleSwiper.TargetObject != (UnityEngine.Object) null && (double) Vector3.Distance((enemyScuttleSwiper.HiddenOffsetIsGlobalPosition ? Vector3.zero : enemyScuttleSwiper.transform.position) + enemyScuttleSwiper.HiddenOffset, enemyScuttleSwiper.TargetObject.transform.position) > (double) enemyScuttleSwiper.HiddenRadius)
      yield return (object) null;
    if ((UnityEngine.Object) enemyScuttleSwiper.TargetObject != (UnityEngine.Object) null)
      enemyScuttleSwiper.state.LookAngle = Utils.GetAngle(enemyScuttleSwiper.transform.position, enemyScuttleSwiper.TargetObject.transform.position);
    enemyScuttleSwiper.ShowWarningIcon();
    enemyScuttleSwiper.health.invincible = false;
    enemyScuttleSwiper.Spine.AnimationState.SetAnimation(0, enemyScuttleSwiper.IdleAnimation, true);
    enemyScuttleSwiper.AttackDelay = 0.0f;
    enemyScuttleSwiper.StartCoroutine((IEnumerator) enemyScuttleSwiper.ActiveRoutine());
  }

  private IEnumerator Hidden()
  {
    EnemyScuttleSwiper enemyScuttleSwiper = this;
    enemyScuttleSwiper.health.invincible = true;
    enemyScuttleSwiper.Spine.transform.localPosition = Vector3.back * enemyScuttleSwiper.HidingHeight;
    enemyScuttleSwiper.ShadowSpriteRenderer.enabled = false;
    enemyScuttleSwiper.Spine.gameObject.GetComponent<MeshRenderer>().enabled = false;
    while (GameManager.RoomActive)
      yield return (object) null;
    while ((UnityEngine.Object) enemyScuttleSwiper.TargetObject == (UnityEngine.Object) null)
    {
      if (Time.frameCount % 5 == 0)
        enemyScuttleSwiper.GetNewTarget();
      yield return (object) null;
    }
    while (enemyScuttleSwiper.DetectPlayerWhileHidden && (double) Vector3.Distance((enemyScuttleSwiper.HiddenOffsetIsGlobalPosition ? Vector3.zero : enemyScuttleSwiper.transform.position) + enemyScuttleSwiper.HiddenOffset, enemyScuttleSwiper.TargetObject.transform.position) > (double) enemyScuttleSwiper.HiddenRadius)
      yield return (object) null;
    enemyScuttleSwiper.RevealAll();
  }

  private void RevealAll()
  {
    float num = -0.2f;
    foreach (EnemyScuttleSwiper scuttler in EnemyScuttleSwiper.Scuttlers)
    {
      if (scuttler.StartHidden == EnemyScuttleSwiper.StartingStates.Hidden)
      {
        scuttler.StopAllCoroutines();
        this.DisableForces = false;
        scuttler.StartCoroutine((IEnumerator) scuttler.Reveal(num += 0.2f));
      }
    }
  }

  public IEnumerator Reveal(float Delay)
  {
    EnemyScuttleSwiper enemyScuttleSwiper = this;
    yield return (object) new WaitForSeconds(Delay);
    enemyScuttleSwiper.Spine.gameObject.GetComponent<MeshRenderer>().enabled = true;
    enemyScuttleSwiper.Spine.AnimationState.SetAnimation(0, enemyScuttleSwiper.FallAnimation, true);
    AudioManager.Instance.PlayOneShot("event:/enemy/fall_from_sky", enemyScuttleSwiper.Spine.transform.gameObject);
    enemyScuttleSwiper.ShadowSpriteRenderer.enabled = true;
    float Grav = 0.0f;
    while ((double) enemyScuttleSwiper.Spine.transform.localPosition.z + (double) Grav < 0.0)
    {
      Grav += Time.fixedDeltaTime * enemyScuttleSwiper.GravitySpeed;
      enemyScuttleSwiper.Spine.transform.localPosition = enemyScuttleSwiper.Spine.transform.localPosition + Vector3.forward * Grav;
      enemyScuttleSwiper.ShadowSpriteRenderer.transform.localScale = Vector3.one * ((-enemyScuttleSwiper.Spine.transform.localPosition.z - enemyScuttleSwiper.HidingHeight) / enemyScuttleSwiper.HidingHeight);
      yield return (object) new WaitForFixedUpdate();
    }
    enemyScuttleSwiper.Spine.transform.localPosition = Vector3.zero;
    enemyScuttleSwiper.ShadowSpriteRenderer.transform.localScale = Vector3.one;
    enemyScuttleSwiper.health.invincible = false;
    enemyScuttleSwiper.Spine.AnimationState.SetAnimation(0, enemyScuttleSwiper.LandAnimation, false);
    AudioManager.Instance.PlayOneShot("event:/enemy/land_normal", enemyScuttleSwiper.Spine.transform.gameObject);
    yield return (object) new WaitForSeconds(0.5f);
    enemyScuttleSwiper.Spine.AnimationState.SetAnimation(0, enemyScuttleSwiper.IdleAnimation, true);
    enemyScuttleSwiper.AttackDelay = 0.0f;
    enemyScuttleSwiper.StartCoroutine((IEnumerator) enemyScuttleSwiper.ActiveRoutine());
  }

  protected virtual IEnumerator ActiveRoutine()
  {
    EnemyScuttleSwiper enemyScuttleSwiper = this;
    yield return (object) new WaitForEndOfFrame();
    while (true)
    {
      if (enemyScuttleSwiper.state.CURRENT_STATE == StateMachine.State.Idle && (double) (enemyScuttleSwiper.IdleWait -= Time.deltaTime) <= 0.0)
        enemyScuttleSwiper.GetNewTargetPosition();
      if ((UnityEngine.Object) enemyScuttleSwiper.TargetObject != (UnityEngine.Object) null && !enemyScuttleSwiper.Attacking && !enemyScuttleSwiper.IsStunned && GameManager.RoomActive)
        enemyScuttleSwiper.state.LookAngle = Utils.GetAngle(enemyScuttleSwiper.transform.position, enemyScuttleSwiper.TargetObject.transform.position);
      else
        enemyScuttleSwiper.state.LookAngle = enemyScuttleSwiper.state.facingAngle;
      if (enemyScuttleSwiper.MovingAnimation != "")
      {
        if (enemyScuttleSwiper.state.CURRENT_STATE == StateMachine.State.Moving && enemyScuttleSwiper.Spine.AnimationName != enemyScuttleSwiper.MovingAnimation)
          enemyScuttleSwiper.Spine.AnimationState.SetAnimation(0, enemyScuttleSwiper.MovingAnimation, true);
        if (enemyScuttleSwiper.state.CURRENT_STATE == StateMachine.State.Idle && enemyScuttleSwiper.Spine.AnimationName != enemyScuttleSwiper.IdleAnimation)
          enemyScuttleSwiper.Spine.AnimationState.SetAnimation(0, enemyScuttleSwiper.IdleAnimation, true);
      }
      if ((UnityEngine.Object) enemyScuttleSwiper.TargetObject == (UnityEngine.Object) null)
      {
        enemyScuttleSwiper.GetNewTarget();
        if ((UnityEngine.Object) enemyScuttleSwiper.TargetObject != (UnityEngine.Object) null)
          enemyScuttleSwiper.ShowWarningIcon();
      }
      else
      {
        if (enemyScuttleSwiper.ShouldSlam())
          enemyScuttleSwiper.StartCoroutine((IEnumerator) enemyScuttleSwiper.SlamRoutine());
        if (enemyScuttleSwiper.ShouldAttack())
          enemyScuttleSwiper.StartCoroutine((IEnumerator) enemyScuttleSwiper.AttackRoutine());
      }
      yield return (object) null;
    }
  }

  protected virtual bool ShouldSlam()
  {
    return (double) (this.SlamTimer -= Time.deltaTime) < 0.0 && !this.Attacking && (double) Vector3.Distance(this.transform.position, this.TargetObject.transform.position) < (double) this.SlamAttackRange && GameManager.RoomActive;
  }

  protected virtual bool ShouldAttack()
  {
    return (double) (this.AttackDelay -= Time.deltaTime) < 0.0 && !this.Attacking && (double) Vector3.Distance(this.transform.position, this.TargetObject.transform.position) < (double) this.VisionRange && GameManager.RoomActive;
  }

  private IEnumerator SlamRoutine()
  {
    EnemyScuttleSwiper enemyScuttleSwiper = this;
    enemyScuttleSwiper.Attacking = true;
    enemyScuttleSwiper.ClearPaths();
    enemyScuttleSwiper.Spine.AnimationState.SetAnimation(0, enemyScuttleSwiper.SignPostSlamAnimation, false);
    enemyScuttleSwiper.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
    float Progress = 0.0f;
    float Duration = 0.5f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      foreach (SimpleSpineFlash simpleSpineFlash in enemyScuttleSwiper.SimpleSpineFlashes)
        simpleSpineFlash.FlashWhite(Progress / Duration);
      yield return (object) null;
    }
    foreach (SimpleSpineFlash simpleSpineFlash in enemyScuttleSwiper.SimpleSpineFlashes)
      simpleSpineFlash.FlashWhite(false);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.6f, 0.5f);
    enemyScuttleSwiper.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
    enemyScuttleSwiper.Spine.AnimationState.SetAnimation(0, enemyScuttleSwiper.SlamAnimation, false);
    float SlamDistance = 1.5f;
    float Rocks = 10f;
    int j = -1;
    while (++j < 3)
    {
      int num = -1;
      float f = 0.0f;
      while ((double) ++num <= (double) Rocks)
      {
        f += (float) (360.0 / (double) Rocks * (Math.PI / 180.0));
        UnityEngine.Object.Instantiate<GameObject>(enemyScuttleSwiper.SlamRockPrefab, enemyScuttleSwiper.transform.position + new Vector3(SlamDistance * Mathf.Cos(f), SlamDistance * Mathf.Sin(f)), Quaternion.identity, enemyScuttleSwiper.transform.parent).GetComponent<ForestScuttlerSlamBarricade>().Play(0.0f);
      }
      yield return (object) new WaitForSeconds(0.2f);
      ++SlamDistance;
      Rocks += 2f;
    }
    yield return (object) new WaitForSeconds(1f);
    enemyScuttleSwiper.Spine.AnimationState.SetAnimation(0, enemyScuttleSwiper.IdleAnimation, true);
    enemyScuttleSwiper.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyScuttleSwiper.IdleWait = 0.0f;
    enemyScuttleSwiper.SlamTimer = enemyScuttleSwiper.TimeBetweenSlams;
    enemyScuttleSwiper.TargetObject = (GameObject) null;
    enemyScuttleSwiper.Attacking = false;
  }

  protected virtual IEnumerator AttackRoutine()
  {
    EnemyScuttleSwiper enemyScuttleSwiper = this;
    enemyScuttleSwiper.Attacking = true;
    enemyScuttleSwiper.ClearPaths();
    int CurrentAttack = 0;
    while (++CurrentAttack <= enemyScuttleSwiper.NumberOfAttacks)
    {
      enemyScuttleSwiper.Spine.AnimationState.SetAnimation(0, enemyScuttleSwiper.SignPostAttackAnimation, enemyScuttleSwiper.LoopSignPostAttackAnimation);
      enemyScuttleSwiper.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
      AudioManager.Instance.PlayOneShot("event:/enemy/chaser/chaser_charge", enemyScuttleSwiper.transform.position);
      if ((UnityEngine.Object) enemyScuttleSwiper.TargetObject != (UnityEngine.Object) null)
      {
        enemyScuttleSwiper.state.LookAngle = Utils.GetAngle(enemyScuttleSwiper.transform.position, enemyScuttleSwiper.TargetObject.transform.position);
        enemyScuttleSwiper.state.facingAngle = enemyScuttleSwiper.state.LookAngle;
      }
      float Progress = 0.0f;
      float Duration = enemyScuttleSwiper.SignPostAttackDuration;
      while ((double) (Progress += Time.deltaTime) < (double) Duration)
      {
        foreach (SimpleSpineFlash simpleSpineFlash in enemyScuttleSwiper.SimpleSpineFlashes)
          simpleSpineFlash.FlashWhite(Progress / Duration);
        yield return (object) null;
      }
      foreach (SimpleSpineFlash simpleSpineFlash in enemyScuttleSwiper.SimpleSpineFlashes)
        simpleSpineFlash.FlashWhite(false);
      if (enemyScuttleSwiper.AttackTowardsPlayer)
      {
        if ((UnityEngine.Object) enemyScuttleSwiper.TargetObject != (UnityEngine.Object) null)
        {
          enemyScuttleSwiper.state.LookAngle = Utils.GetAngle(enemyScuttleSwiper.transform.position, enemyScuttleSwiper.TargetObject.transform.position);
          enemyScuttleSwiper.state.facingAngle = enemyScuttleSwiper.state.LookAngle;
        }
        enemyScuttleSwiper.DoKnockBack(enemyScuttleSwiper.TargetObject, -1f, 1f);
      }
      else
      {
        enemyScuttleSwiper.DisableForces = true;
        enemyScuttleSwiper.Force = (Vector3) (new Vector2(2500f * Mathf.Cos(enemyScuttleSwiper.state.LookAngle * ((float) Math.PI / 180f)), 2500f * Mathf.Sin(enemyScuttleSwiper.state.LookAngle * ((float) Math.PI / 180f))) * enemyScuttleSwiper.AttackForceModifier);
        enemyScuttleSwiper.rb.AddForce((Vector2) enemyScuttleSwiper.Force);
      }
      enemyScuttleSwiper.damageColliderEvents.SetActive(true);
      if (!string.IsNullOrEmpty(enemyScuttleSwiper.AttackVO))
        AudioManager.Instance.PlayOneShot(enemyScuttleSwiper.AttackVO, enemyScuttleSwiper.transform.position);
      AudioManager.Instance.PlayOneShot("event:/enemy/chaser/chaser_attack", enemyScuttleSwiper.transform.position);
      enemyScuttleSwiper.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
      enemyScuttleSwiper.Spine.AnimationState.SetAnimation(0, enemyScuttleSwiper.AttackAnimation, false);
      enemyScuttleSwiper.Spine.AnimationState.AddAnimation(0, enemyScuttleSwiper.IdleAnimation, true, 0.0f);
      if ((double) enemyScuttleSwiper.DamageColliderDuration != -1.0)
        enemyScuttleSwiper.StartCoroutine((IEnumerator) enemyScuttleSwiper.EnableCollider(enemyScuttleSwiper.DamageColliderDuration));
      yield return (object) new WaitForSeconds(enemyScuttleSwiper.AttackDuration * 0.7f);
      enemyScuttleSwiper.damageColliderEvents.SetActive(false);
    }
    yield return (object) new WaitForSeconds(enemyScuttleSwiper.AttackDuration * 0.3f);
    enemyScuttleSwiper.DisableForces = false;
    enemyScuttleSwiper.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyScuttleSwiper.IdleWait = 0.0f;
    enemyScuttleSwiper.AttackDelay = enemyScuttleSwiper.AttackDelayTime;
    enemyScuttleSwiper.TargetObject = (GameObject) null;
    enemyScuttleSwiper.Attacking = false;
  }

  private IEnumerator EnableCollider(float dur)
  {
    yield return (object) new WaitForSeconds(dur);
    this.damageColliderEvents.SetActive(false);
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

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    if (!string.IsNullOrEmpty(this.GetHitVO))
      AudioManager.Instance.PlayOneShot(this.GetHitVO, this.transform.position);
    if (!this.DisableKnockback)
      this.damageColliderEvents.SetActive(false);
    if (this.CanBeInterrupted)
    {
      this.StopAllCoroutines();
      this.DisableForces = false;
      this.StartCoroutine((IEnumerator) this.HurtRoutine());
    }
    if (AttackType != Health.AttackTypes.NoKnockBack && !this.DisableKnockback && this.CanBeInterrupted)
      this.StartCoroutine((IEnumerator) this.ApplyForceRoutine(Attacker));
    foreach (SimpleSpineFlash simpleSpineFlash in this.SimpleSpineFlashes)
      simpleSpineFlash.FlashFillRed();
  }

  private IEnumerator ApplyForceRoutine(GameObject Attacker)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyScuttleSwiper enemyScuttleSwiper = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      enemyScuttleSwiper.DisableForces = false;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    enemyScuttleSwiper.DisableForces = true;
    enemyScuttleSwiper.Angle = Utils.GetAngle(Attacker.transform.position, enemyScuttleSwiper.transform.position) * ((float) Math.PI / 180f);
    enemyScuttleSwiper.Force = (Vector3) (new Vector2(1500f * Mathf.Cos(enemyScuttleSwiper.Angle), 1500f * Mathf.Sin(enemyScuttleSwiper.Angle)) * enemyScuttleSwiper.KnockbackModifier);
    enemyScuttleSwiper.rb.AddForce((Vector2) enemyScuttleSwiper.Force);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.5f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  protected IEnumerator ApplyForceRoutine(Vector3 forcePosition)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyScuttleSwiper enemyScuttleSwiper = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      enemyScuttleSwiper.DisableForces = false;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    enemyScuttleSwiper.DisableForces = true;
    enemyScuttleSwiper.Angle = Utils.GetAngle(forcePosition, enemyScuttleSwiper.transform.position) * ((float) Math.PI / 180f);
    enemyScuttleSwiper.Force = (Vector3) (new Vector2(1500f * Mathf.Cos(enemyScuttleSwiper.Angle), 1500f * Mathf.Sin(enemyScuttleSwiper.Angle)) * enemyScuttleSwiper.KnockbackModifier);
    enemyScuttleSwiper.rb.AddForce((Vector2) enemyScuttleSwiper.Force);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.5f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private IEnumerator HurtRoutine()
  {
    EnemyScuttleSwiper enemyScuttleSwiper = this;
    enemyScuttleSwiper.damageColliderEvents.SetActive(false);
    enemyScuttleSwiper.Attacking = false;
    enemyScuttleSwiper.ClearPaths();
    enemyScuttleSwiper.state.CURRENT_STATE = StateMachine.State.KnockBack;
    enemyScuttleSwiper.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyScuttleSwiper.Spine.AnimationState.SetAnimation(0, enemyScuttleSwiper.IdleAnimation, true);
    yield return (object) new WaitForSeconds(0.5f);
    enemyScuttleSwiper.DisableForces = false;
    enemyScuttleSwiper.IdleWait = 0.0f;
    enemyScuttleSwiper.StartCoroutine((IEnumerator) enemyScuttleSwiper.ActiveRoutine());
    if (enemyScuttleSwiper.CounterAttack)
      enemyScuttleSwiper.StartCoroutine(enemyScuttleSwiper.SlamAttack ? (IEnumerator) enemyScuttleSwiper.SlamRoutine() : (IEnumerator) enemyScuttleSwiper.AttackRoutine());
  }

  public void GetNewTargetPosition()
  {
    float num = 100f;
    if ((UnityEngine.Object) this.GetClosestTarget() != (UnityEngine.Object) null && (double) this.ChanceToPathTowardsPlayer > 0.0 && (double) UnityEngine.Random.value < (double) this.ChanceToPathTowardsPlayer && (double) Vector3.Distance(this.transform.position, this.GetClosestTarget().transform.position) < (double) this.DistanceToPathTowardsPlayer && this.CheckLineOfSight(this.GetClosestTarget().transform.position, (float) this.DistanceToPathTowardsPlayer))
    {
      this.PathingToPlayer = true;
      this.RandomDirection = Utils.GetAngle(this.transform.position, this.GetClosestTarget().transform.position) * ((float) Math.PI / 180f);
    }
    while ((double) --num > 0.0)
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
        break;
      }
    }
  }

  public void GetNewTarget()
  {
    if (!GameManager.RoomActive)
      return;
    Health closestTarget = this.GetClosestTarget();
    if (!((UnityEngine.Object) closestTarget != (UnityEngine.Object) null))
      return;
    this.TargetObject = closestTarget.gameObject;
    this.EnemyHealth = closestTarget;
  }

  public void DoBusiness() => this.StartCoroutine((IEnumerator) this.BusinessRoutine());

  private IEnumerator BusinessRoutine()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyScuttleSwiper enemyScuttleSwiper = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", enemyScuttleSwiper.gameObject);
      GameManager.GetInstance().OnConversationNew();
      PlayerFarming.Instance._state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      DecorationCustomTarget.Create(enemyScuttleSwiper.transform.position, PlayerFarming.Instance.gameObject.transform.position, 1f, StructureBrain.TYPES.DECORATION_MONSTERSHRINE, new System.Action(enemyScuttleSwiper.FinishedGettingDecoration));
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForEndOfFrame();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private void FinishedGettingDecoration()
  {
    GameManager.GetInstance().OnConversationEnd();
    PlayerFarming.Instance._state.CURRENT_STATE = StateMachine.State.Idle;
  }

  protected virtual void OnDamageTriggerEnter(Collider2D collider)
  {
    this.EnemyHealth = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) this.EnemyHealth != (UnityEngine.Object) null) || this.EnemyHealth.team == this.health.team && this.health.team != Health.Team.PlayerTeam)
      return;
    this.EnemyHealth.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f));
  }

  private void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position, (float) this.VisionRange, Color.red);
    if (this.DetectPlayerWhileHidden && (this.StartHidden == EnemyScuttleSwiper.StartingStates.Hidden || this.StartHidden == EnemyScuttleSwiper.StartingStates.Animation))
      Utils.DrawCircleXY((this.HiddenOffsetIsGlobalPosition ? Vector3.zero : this.transform.position) + this.HiddenOffset, this.HiddenRadius, Color.yellow);
    if ((double) this.ChanceToPathTowardsPlayer > 0.0)
      Utils.DrawCircleXY(this.transform.position, (float) this.DistanceToPathTowardsPlayer, Color.cyan);
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

  public enum StartingStates
  {
    Hidden,
    Wandering,
    Animation,
    Intro,
  }
}
