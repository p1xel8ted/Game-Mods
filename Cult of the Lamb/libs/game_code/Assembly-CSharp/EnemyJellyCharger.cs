// Decompiled with JetBrains decompiler
// Type: EnemyJellyCharger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using Spine.Unity;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class EnemyJellyCharger : EnemyExploder
{
  [SerializeField]
  public SpriteRenderer Aiming;
  [SerializeField]
  public SkeletonAnimation Warning;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string movingAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string chargeAttackAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string chargeAttackEndAnimation;
  public SimpleSpineFlash[] SimpleSpineFlashes;
  [Header("Charging")]
  [SerializeField]
  public bool canCharge = true;
  [SerializeField]
  public bool canDeflect;
  [SerializeField]
  public float distanceToCharge;
  [SerializeField]
  public float chargeTime;
  [SerializeField]
  [Range(0.0f, 1f)]
  [Tooltip("0.75 means charger will stop targeting the player in the last 25% of charge time")]
  public float followPercentage = 0.7f;
  [SerializeField]
  public float chargeSpeed;
  [SerializeField]
  public float attackEndCooldown;
  public float warmingTimer;
  public bool warming;
  public bool chargeAttacking;
  public float chargeAttackEndTimestamp;
  public float flashTickTimer;
  public PoisonTrail poisonTrail;
  [CompilerGenerated]
  public bool \u003CAllowMultipleChargers\u003Ek__BackingField;
  public static EnemyJellyCharger CurrentCharger;
  public float TargetAngle;
  public bool playedSfx;
  public EventInstance LoopedSound;

  public bool AllowMultipleChargers
  {
    get => this.\u003CAllowMultipleChargers\u003Ek__BackingField;
    set => this.\u003CAllowMultipleChargers\u003Ek__BackingField = value;
  }

  public override void Start()
  {
    base.Start();
    this.poisonTrail = this.GetComponent<PoisonTrail>();
    if (!((Object) this.Aiming != (Object) null))
      return;
    this.Aiming.gameObject.SetActive(false);
  }

  public IEnumerator ShowWarning()
  {
    this.Warning.gameObject.SetActive(true);
    yield return (object) this.Warning.YieldForAnimation("warn");
    this.Warning.gameObject.SetActive(false);
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.chargeAttacking = false;
    this.warming = false;
  }

  public override void Update()
  {
    base.Update();
    if (this.canCharge)
    {
      if (!this.chargeAttacking && !this.warming && !this.isExploding && (Object) this.targetObject != (Object) null)
        this.state.LookAngle = Utils.GetAngle(this.transform.position, this.targetObject.transform.position);
      if (((Object) EnemyJellyCharger.CurrentCharger == (Object) null || (Object) EnemyJellyCharger.CurrentCharger == (Object) this || this.AllowMultipleChargers) && this.inRange && !this.warming && !this.chargeAttacking && (bool) (Object) this.gm && (double) this.gm.CurrentTime > (double) this.chargeAttackEndTimestamp && (double) this.distanceToTarget < (double) this.distanceToCharge && (bool) (Object) this.gm && (double) this.gm.CurrentTime > (double) this.initialSpawnTimestamp / (double) this.Spine.timeScale && (bool) (Object) this.targetObject && (Object) this.targetObject.state != (Object) null && this.targetObject.state.CURRENT_STATE != StateMachine.State.HitRecover && this.CheckLineOfSight(this.distanceToCharge))
        this.WarmUp();
      else if (this.warming && !this.isExploding)
      {
        this.warmingTimer += Time.deltaTime * this.Spine.timeScale;
        float amt = this.warmingTimer / this.chargeTime;
        this.simpleSpineFlash.FlashWhite(amt);
        if (!this.playedSfx)
        {
          AudioManager.Instance.PlayOneShot("event:/enemy/vocals/jellyfish/warning", this.gameObject);
          this.LoopedSound = AudioManager.Instance.CreateLoop("event:/enemy/jellyfish_charge", this.gameObject, true);
          this.playedSfx = true;
        }
        if ((double) amt < (double) this.followPercentage)
          this.TargetAngle = this.LookAtTarget(true);
        if ((double) amt > 1.0 && this.targetObject.state.CURRENT_STATE != StateMachine.State.HitRecover)
        {
          AudioManager.Instance.StopLoop(this.LoopedSound);
          this.playedSfx = false;
          this.ChargeAtTarget(this.TargetAngle);
        }
      }
      if (this.chargeAttacking && !this.isExploding)
      {
        this.DisableForces = false;
        this.speed = this.chargeSpeed * this.Spine.timeScale;
        this.maxSpeed = this.chargeSpeed;
        this.move();
      }
      else
        this.maxSpeed = 0.025f;
    }
    if (!(bool) (Object) this.poisonTrail)
      return;
    this.poisonTrail.enabled = this.chargeAttacking;
  }

  public override void FixedUpdate()
  {
    if ((double) this.Spine.timeScale == 9.9999997473787516E-05)
      return;
    base.FixedUpdate();
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    if (this.canDeflect && (this.chargeAttacking || this.warming))
    {
      this.warming = false;
      float angle = Utils.GetAngle(Attacker.transform.position, AttackLocation);
      this.LookAtAngle(angle);
      this.ChargeAtTarget(angle);
    }
    foreach (SimpleSpineFlash simpleSpineFlash in this.SimpleSpineFlashes)
      simpleSpineFlash.FlashFillRed();
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if (this.canDeflect && this.chargeAttacking && (Object) Attacker == (Object) PlayerFarming.Instance.gameObject)
    {
      this.warming = false;
      float angle = Utils.GetAngle(Attacker.transform.position, AttackLocation);
      this.LookAtAngle(angle);
      this.ChargeAtTarget(angle);
    }
    else
      base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
  }

  public void WarmUp()
  {
    if ((Object) this.Spine != (Object) null && this.Spine.AnimationState != null && this.Spine.AnimationState.GetCurrent(0) != null && this.Spine.AnimationState.GetCurrent(0).Animation.Name != this.anticipationAnimation)
    {
      AudioManager.Instance.PlayOneShot("event:/enemy/chaser/chaser_charge", this.gameObject);
      AudioManager.Instance.PlayOneShot("event:/enemy/vocals/jellyfish/warning", this.gameObject);
      this.StartCoroutine(this.ShowWarning());
    }
    this.warmingTimer = 0.0f;
    this.warming = true;
    if ((Object) this.Spine != (Object) null && this.Spine.AnimationState != null)
      this.Spine.AnimationState.SetAnimation(0, this.anticipationAnimation, true);
    this.ClearPaths();
    EnemyJellyCharger.CurrentCharger = this;
  }

  public override void WithinDistanceOfTarget()
  {
    if (this.chargeAttacking)
      return;
    base.WithinDistanceOfTarget();
  }

  public void ChargeAtTarget(float angle)
  {
    angle = Utils.Repeat(angle, 360f);
    angle = Mathf.Round(angle / 45f) * 45f;
    this.LookAtAngle(angle);
    if ((Object) this.Aiming != (Object) null && (double) this.Spine.timeScale != 9.9999997473787516E-05)
      this.Aiming.transform.eulerAngles = new Vector3(0.0f, 0.0f, angle);
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/jellyfish/attack", this.gameObject);
    AudioManager.Instance.PlayOneShot("event:/enemy/chaser/chaser_attack", this.gameObject);
    this.damageColliderEvents.gameObject.SetActive(true);
    this.warming = false;
    this.chargeAttacking = true;
    this.UsePathing = false;
    this.state.CURRENT_STATE = StateMachine.State.Charging;
    this.StartCoroutine(this.FlashDelay());
    string animationName = this.chargeAttackAnimation;
    if ((double) angle <= 135.0)
    {
      if ((double) angle <= 45.0)
      {
        if ((double) angle != 0.0)
        {
          if ((double) angle == 45.0)
            animationName = "attacking-up-diagonal";
        }
        else
          animationName = "attacking";
      }
      else if ((double) angle != 90.0)
      {
        if ((double) angle == 135.0)
          animationName = "attacking-up-diagonal";
      }
      else
        animationName = "attacking-up";
    }
    else if ((double) angle <= 225.0)
    {
      if ((double) angle != 180.0)
      {
        if ((double) angle == 225.0)
          animationName = "attacking-down-diagonal";
      }
      else
        animationName = "attacking";
    }
    else if ((double) angle != 270.0)
    {
      if ((double) angle == 315.0)
        animationName = "attacking-down-diagonal";
    }
    else
      animationName = "attacking-down";
    this.Spine.AnimationState.SetAnimation(0, animationName, true).MixDuration = 0.0f;
  }

  public float LookAtTarget(bool LimitTo45 = false)
  {
    float num = Utils.GetAngle(this.transform.position, this.targetObject.transform.position);
    if (LimitTo45)
      num = Mathf.Round(num / 45f) * 45f;
    this.LookAtAngle(num);
    if ((Object) this.Aiming != (Object) null)
    {
      if (!this.Aiming.gameObject.activeSelf && (double) this.Spine.timeScale != 9.9999997473787516E-05)
        this.Aiming.transform.eulerAngles = new Vector3(0.0f, 0.0f, num);
      this.Aiming.gameObject.SetActive(true);
      float z = Mathf.LerpAngle(this.Aiming.transform.eulerAngles.z, num, 5f * Time.deltaTime);
      if ((double) this.Spine.timeScale != 9.9999997473787516E-05)
        this.Aiming.transform.eulerAngles = new Vector3(0.0f, 0.0f, z);
      if ((double) this.flashTickTimer >= 0.11999999731779099 && BiomeConstants.Instance.IsFlashLightsActive)
      {
        this.Aiming.color = this.Aiming.color == Color.red ? Color.white : Color.red;
        this.flashTickTimer = 0.0f;
      }
      this.flashTickTimer += Time.deltaTime;
    }
    return num;
  }

  public IEnumerator FlashDelay()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyJellyCharger enemyJellyCharger = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      enemyJellyCharger.simpleSpineFlash.FlashWhite(false);
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

  public override void UpdateMoving()
  {
    if (!(bool) (Object) this.gm || (double) this.gm.CurrentTime <= (double) this.chargeAttackEndTimestamp / (double) this.Spine.timeScale || this.chargeAttacking || this.warming)
      return;
    if (this.state.CURRENT_STATE == StateMachine.State.Idle && this.movingAnimation != "")
      this.Spine.AnimationState.SetAnimation(0, this.movingAnimation, true);
    base.UpdateMoving();
  }

  public virtual void AttackEnd()
  {
    CameraManager.instance.ShakeCameraForDuration(0.8f, 1f, 0.35f);
    AudioManager.Instance.PlayOneShot("event:/enemy/impact_squishy", this.transform.position);
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/jellyfish/gethit", this.transform.position);
    this.chargeAttacking = false;
    this.chargeAttackEndTimestamp = this.gm.CurrentTime + this.attackEndCooldown;
    this.ClearPaths();
    if ((Object) this.Aiming != (Object) null)
      this.Aiming.gameObject.SetActive(false);
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.Spine.AnimationState.SetAnimation(0, this.chargeAttackEndAnimation, false);
    this.damageColliderEvents.gameObject.SetActive(false);
    this.speed = 0.0f;
    this.targetObject = (Health) null;
    if ((Object) EnemyJellyCharger.CurrentCharger == (Object) this)
      EnemyJellyCharger.CurrentCharger = (EnemyJellyCharger) null;
    if (!this.canExplode)
      return;
    this.Explode();
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if (!((Object) EnemyJellyCharger.CurrentCharger == (Object) this))
      return;
    EnemyJellyCharger.CurrentCharger = (EnemyJellyCharger) null;
  }

  public override void OnDisable()
  {
    base.OnDisable();
    if (!((Object) EnemyJellyCharger.CurrentCharger == (Object) this))
      return;
    EnemyJellyCharger.CurrentCharger = (EnemyJellyCharger) null;
  }

  public override void KnockTowardsEnemy(GameObject Attacker, Health.AttackTypes AttackType)
  {
    if (this.chargeAttacking && !this.isHit)
      return;
    base.KnockTowardsEnemy(Attacker, AttackType);
  }

  public new void OnCollisionEnter2D(Collision2D collision)
  {
    if (!this.chargeAttacking || (this.layerToCheck.value & 1 << collision.gameObject.layer) <= 0)
      return;
    this.OnDamageTriggerEnter(collision.collider);
    this.AttackEnd();
  }

  public override void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component1 = collider.GetComponent<Health>();
    UnitObject component2 = collider.GetComponent<UnitObject>();
    if (this.canExplode)
    {
      if (!((Object) component1 != (Object) null) || component1.team != Health.Team.PlayerTeam || !this.canExplode || component2.state.CURRENT_STATE == StateMachine.State.Dodging)
        return;
      this.Explode();
    }
    else
      base.OnDamageTriggerEnter(collider);
  }
}
