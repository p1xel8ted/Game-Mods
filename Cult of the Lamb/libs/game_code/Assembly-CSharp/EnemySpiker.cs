// Decompiled with JetBrains decompiler
// Type: EnemySpiker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class EnemySpiker : UnitObject
{
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
  [Range(0.0f, 1f)]
  public float ChanceToPathTowardsPlayer;
  public int DistanceToPathTowardsPlayer = 6;
  public float SlamAttackRange;
  public float TimeBetweenSlams;
  public GameObject SlamRockPrefab;
  public float SlamTimer;
  public SkeletonAnimation warningIcon;
  public GameObject TargetObject;
  public float RandomDirection;
  public float AttackDelayTime;
  public bool Attacking;
  public bool IsStunned;
  [HideInInspector]
  public float AttackDelay;
  public float AttackDuration = 1f;
  public float SignPostAttackDuration = 0.5f;
  public bool DisableKnockback;
  public float Angle;
  public Vector3 Force;
  public float TurningArc = 90f;
  public Vector2 DistanceRange = new Vector2(1f, 3f);
  public Vector2 IdleWaitRange = new Vector2(1f, 3f);
  public float IdleWait;
  public bool PathingToPlayer;
  public Health EnemyHealth;
  public int DetectEnemyRange = 8;

  public override void OnEnable()
  {
    base.OnEnable();
    this.SlamTimer = this.TimeBetweenSlams;
    this.RandomDirection = (float) UnityEngine.Random.Range(0, 360) * ((float) Math.PI / 180f);
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.damageColliderEvents.SetActive(false);
    }
    this.SimpleSpineFlashes = this.GetComponentsInChildren<SimpleSpineFlash>();
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.StartCoroutine((IEnumerator) this.ActiveRoutine());
  }

  public override void OnDisable()
  {
    base.OnDisable();
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.SetActive(false);
      this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    }
    this.ClearPaths();
    this.StopAllCoroutines();
    foreach (SimpleSpineFlash simpleSpineFlash in this.SimpleSpineFlashes)
      simpleSpineFlash.FlashWhite(false);
  }

  public IEnumerator ActiveRoutine()
  {
    EnemySpiker enemySpiker = this;
    yield return (object) new WaitForEndOfFrame();
    while (true)
    {
      if (enemySpiker.state.CURRENT_STATE == StateMachine.State.Idle && (double) (enemySpiker.IdleWait -= Time.deltaTime) <= 0.0)
        enemySpiker.GetNewTargetPosition();
      if ((UnityEngine.Object) enemySpiker.TargetObject != (UnityEngine.Object) null && !enemySpiker.Attacking && !enemySpiker.IsStunned && GameManager.RoomActive)
        enemySpiker.state.LookAngle = Utils.GetAngle(enemySpiker.transform.position, enemySpiker.TargetObject.transform.position);
      else
        enemySpiker.state.LookAngle = enemySpiker.state.facingAngle;
      if (enemySpiker.MovingAnimation != "")
      {
        if (enemySpiker.state.CURRENT_STATE == StateMachine.State.Moving && enemySpiker.Spine.AnimationName != enemySpiker.MovingAnimation)
          enemySpiker.Spine.AnimationState.SetAnimation(0, enemySpiker.MovingAnimation, true);
        if (enemySpiker.state.CURRENT_STATE == StateMachine.State.Idle && enemySpiker.Spine.AnimationName != enemySpiker.IdleAnimation)
          enemySpiker.Spine.AnimationState.SetAnimation(0, enemySpiker.IdleAnimation, true);
      }
      if ((UnityEngine.Object) enemySpiker.TargetObject == (UnityEngine.Object) null)
      {
        enemySpiker.GetNewTarget();
      }
      else
      {
        if (enemySpiker.ShouldAttack())
          enemySpiker.StartCoroutine((IEnumerator) enemySpiker.SlamRoutine());
        if (enemySpiker.ShouldSlam())
          enemySpiker.StartCoroutine((IEnumerator) enemySpiker.AttackRoutine());
      }
      yield return (object) null;
    }
  }

  public virtual bool ShouldAttack()
  {
    return (double) (this.SlamTimer -= Time.deltaTime) < 0.0 && !this.Attacking && (double) Vector3.Distance(this.transform.position, this.TargetObject.transform.position) < (double) this.SlamAttackRange;
  }

  public virtual bool ShouldSlam()
  {
    return (double) (this.AttackDelay -= Time.deltaTime) < 0.0 && !this.Attacking && (double) Vector3.Distance(this.transform.position, this.TargetObject.transform.position) < (double) this.VisionRange;
  }

  public IEnumerator SlamRoutine()
  {
    EnemySpiker enemySpiker = this;
    enemySpiker.Attacking = true;
    enemySpiker.ClearPaths();
    enemySpiker.Spine.AnimationState.SetAnimation(0, enemySpiker.SignPostSlamAnimation, false);
    enemySpiker.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
    float Progress = 0.0f;
    float Duration = 0.5f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      foreach (SimpleSpineFlash simpleSpineFlash in enemySpiker.SimpleSpineFlashes)
        simpleSpineFlash.FlashWhite(Progress / Duration);
      yield return (object) null;
    }
    foreach (SimpleSpineFlash simpleSpineFlash in enemySpiker.SimpleSpineFlashes)
      simpleSpineFlash.FlashWhite(false);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.6f, 0.5f);
    enemySpiker.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
    enemySpiker.Spine.AnimationState.SetAnimation(0, enemySpiker.SlamAnimation, false);
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
        UnityEngine.Object.Instantiate<GameObject>(enemySpiker.SlamRockPrefab, enemySpiker.transform.position + new Vector3(SlamDistance * Mathf.Cos(f), SlamDistance * Mathf.Sin(f)), Quaternion.identity, enemySpiker.transform.parent).GetComponent<ForestScuttlerSlamBarricade>().Play(0.0f);
      }
      yield return (object) new WaitForSeconds(0.2f);
      ++SlamDistance;
      Rocks += 2f;
    }
    yield return (object) new WaitForSeconds(1f);
    enemySpiker.Spine.AnimationState.SetAnimation(0, enemySpiker.IdleAnimation, true);
    enemySpiker.state.CURRENT_STATE = StateMachine.State.Idle;
    enemySpiker.IdleWait = 0.0f;
    enemySpiker.SlamTimer = enemySpiker.TimeBetweenSlams;
    enemySpiker.TargetObject = (GameObject) null;
    enemySpiker.Attacking = false;
  }

  public virtual IEnumerator AttackRoutine()
  {
    EnemySpiker enemySpiker = this;
    enemySpiker.Attacking = true;
    enemySpiker.ClearPaths();
    int CurrentAttack = 0;
    while (++CurrentAttack <= enemySpiker.NumberOfAttacks)
    {
      enemySpiker.Spine.AnimationState.SetAnimation(0, enemySpiker.SignPostAttackAnimation, enemySpiker.LoopSignPostAttackAnimation);
      enemySpiker.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
      AudioManager.Instance.PlayOneShot("event:/enemy/chaser/chaser_charge", enemySpiker.transform.position);
      if ((UnityEngine.Object) enemySpiker.TargetObject != (UnityEngine.Object) null)
      {
        enemySpiker.state.LookAngle = Utils.GetAngle(enemySpiker.transform.position, enemySpiker.TargetObject.transform.position);
        enemySpiker.state.facingAngle = enemySpiker.state.LookAngle;
      }
      float Progress = 0.0f;
      float Duration = enemySpiker.SignPostAttackDuration;
      while ((double) (Progress += Time.deltaTime) < (double) Duration)
      {
        foreach (SimpleSpineFlash simpleSpineFlash in enemySpiker.SimpleSpineFlashes)
          simpleSpineFlash.FlashWhite(Progress / Duration);
        yield return (object) null;
      }
      foreach (SimpleSpineFlash simpleSpineFlash in enemySpiker.SimpleSpineFlashes)
        simpleSpineFlash.FlashWhite(false);
      enemySpiker.DisableForces = true;
      enemySpiker.Force = (Vector3) (new Vector2(2500f * Mathf.Cos(enemySpiker.state.LookAngle * ((float) Math.PI / 180f)), 2500f * Mathf.Sin(enemySpiker.state.LookAngle * ((float) Math.PI / 180f))) * enemySpiker.AttackForceModifier);
      enemySpiker.rb.AddForce((Vector2) enemySpiker.Force);
      enemySpiker.damageColliderEvents.SetActive(true);
      AudioManager.Instance.PlayOneShot("event:/enemy/chaser/chaser_attack", enemySpiker.transform.position);
      enemySpiker.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
      enemySpiker.Spine.AnimationState.SetAnimation(0, enemySpiker.AttackAnimation, false);
      enemySpiker.Spine.AnimationState.AddAnimation(0, enemySpiker.IdleAnimation, true, 0.0f);
      yield return (object) new WaitForSeconds(enemySpiker.AttackDuration);
      enemySpiker.damageColliderEvents.SetActive(false);
      yield return (object) new WaitForSeconds(0.5f);
    }
    yield return (object) new WaitForSeconds(enemySpiker.AttackDuration * 0.3f);
    enemySpiker.DisableForces = false;
    enemySpiker.state.CURRENT_STATE = StateMachine.State.Idle;
    enemySpiker.IdleWait = 0.0f;
    enemySpiker.AttackDelay = enemySpiker.AttackDelayTime;
    enemySpiker.TargetObject = (GameObject) null;
    enemySpiker.Attacking = false;
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    if (!this.DisableKnockback)
      this.damageColliderEvents.SetActive(false);
    if (!this.Attacking && this.CanBeInterrupted)
    {
      this.StopAllCoroutines();
      this.StartCoroutine((IEnumerator) this.HurtRoutine());
    }
    if (AttackType != Health.AttackTypes.NoKnockBack && !this.DisableKnockback && this.CanBeInterrupted)
      this.StartCoroutine((IEnumerator) this.ApplyForceRoutine(Attacker));
    foreach (SimpleSpineFlash simpleSpineFlash in this.SimpleSpineFlashes)
      simpleSpineFlash.FlashFillRed();
  }

  public IEnumerator ApplyForceRoutine(GameObject Attacker)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemySpiker enemySpiker = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      enemySpiker.DisableForces = false;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    enemySpiker.DisableForces = true;
    enemySpiker.Angle = Utils.GetAngle(Attacker.transform.position, enemySpiker.transform.position) * ((float) Math.PI / 180f);
    enemySpiker.Force = (Vector3) (new Vector2(1500f * Mathf.Cos(enemySpiker.Angle), 1500f * Mathf.Sin(enemySpiker.Angle)) * enemySpiker.KnockbackModifier);
    enemySpiker.rb.AddForce((Vector2) enemySpiker.Force);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.5f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator ApplyForceRoutine(Vector3 forcePosition)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemySpiker enemySpiker = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      enemySpiker.DisableForces = false;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    enemySpiker.DisableForces = true;
    enemySpiker.Angle = Utils.GetAngle(forcePosition, enemySpiker.transform.position) * ((float) Math.PI / 180f);
    enemySpiker.Force = (Vector3) (new Vector2(1500f * Mathf.Cos(enemySpiker.Angle), 1500f * Mathf.Sin(enemySpiker.Angle)) * enemySpiker.KnockbackModifier);
    enemySpiker.rb.AddForce((Vector2) enemySpiker.Force);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.5f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator HurtRoutine()
  {
    EnemySpiker enemySpiker = this;
    enemySpiker.damageColliderEvents.SetActive(false);
    enemySpiker.Attacking = false;
    enemySpiker.ClearPaths();
    enemySpiker.state.CURRENT_STATE = StateMachine.State.KnockBack;
    yield return (object) new WaitForSeconds(0.5f);
    enemySpiker.DisableForces = false;
    enemySpiker.IdleWait = 0.0f;
    enemySpiker.state.CURRENT_STATE = StateMachine.State.Idle;
    enemySpiker.Spine.AnimationState.SetAnimation(0, enemySpiker.IdleAnimation, true);
    enemySpiker.StartCoroutine((IEnumerator) enemySpiker.ActiveRoutine());
    if (enemySpiker.CounterAttack)
      enemySpiker.StartCoroutine(enemySpiker.SlamAttack ? (IEnumerator) enemySpiker.SlamRoutine() : (IEnumerator) enemySpiker.AttackRoutine());
  }

  public void GetNewTargetPosition()
  {
    float num = 100f;
    PlayerFarming closestPlayer = PlayerFarming.FindClosestPlayer(this.transform.position);
    if ((UnityEngine.Object) closestPlayer != (UnityEngine.Object) null && (double) this.ChanceToPathTowardsPlayer > 0.0 && (double) UnityEngine.Random.value < (double) this.ChanceToPathTowardsPlayer && (double) Vector3.Distance(this.transform.position, closestPlayer.transform.position) < (double) this.DistanceToPathTowardsPlayer && this.CheckLineOfSightOnTarget(closestPlayer.gameObject, closestPlayer.transform.position, (float) this.DistanceToPathTowardsPlayer))
    {
      this.PathingToPlayer = true;
      this.RandomDirection = Utils.GetAngle(this.transform.position, closestPlayer.transform.position) * ((float) Math.PI / 180f);
    }
    while ((double) --num > 0.0)
    {
      float distance = UnityEngine.Random.Range(this.DistanceRange.x, this.DistanceRange.y);
      if (!this.PathingToPlayer)
        this.RandomDirection += UnityEngine.Random.Range(-this.TurningArc, this.TurningArc) * ((float) Math.PI / 180f);
      this.PathingToPlayer = false;
      float radius = 0.2f;
      Vector3 targetLocation = this.transform.position + new Vector3(distance * Mathf.Cos(this.RandomDirection), distance * Mathf.Sin(this.RandomDirection));
      if ((UnityEngine.Object) Physics2D.CircleCast((Vector2) this.transform.position, radius, (Vector2) Vector3.Normalize(targetLocation - this.transform.position), distance, (int) this.layerToCheck).collider != (UnityEngine.Object) null)
      {
        this.RandomDirection = 180f - this.RandomDirection;
      }
      else
      {
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
    Health health = (Health) null;
    float num1 = float.MaxValue;
    foreach (Health allUnit in Health.allUnits)
    {
      if (allUnit.team != this.health.team && !allUnit.InanimateObject && allUnit.team != Health.Team.Neutral && (this.health.team != Health.Team.PlayerTeam || this.health.team == Health.Team.PlayerTeam && allUnit.team != Health.Team.DangerousAnimals) && (double) Vector2.Distance((Vector2) this.transform.position, (Vector2) allUnit.gameObject.transform.position) < (double) this.VisionRange && this.CheckLineOfSightOnTarget(allUnit.gameObject, allUnit.gameObject.transform.position, Vector2.Distance((Vector2) allUnit.gameObject.transform.position, (Vector2) this.transform.position)))
      {
        float num2 = Vector3.Distance(this.transform.position, allUnit.gameObject.transform.position);
        if ((double) num2 < (double) num1)
        {
          health = allUnit;
          num1 = num2;
        }
      }
    }
    if (!((UnityEngine.Object) health != (UnityEngine.Object) null))
      return;
    this.TargetObject = health.gameObject;
    this.EnemyHealth = health;
  }

  public virtual void OnDamageTriggerEnter(Collider2D collider)
  {
    this.EnemyHealth = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) this.EnemyHealth != (UnityEngine.Object) null) || this.EnemyHealth.team == this.health.team && this.health.team != Health.Team.PlayerTeam)
      return;
    this.EnemyHealth.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f));
  }
}
