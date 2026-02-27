// Decompiled with JetBrains decompiler
// Type: EnemySpiderMonster
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unify;
using UnityEngine;

#nullable disable
public class EnemySpiderMonster : UnitObject
{
  public Interaction_MonsterHeart interaction_MonsterHeart;
  public bool Mutated;
  private Vector3 StartPosition;
  private SimpleSpineEventListener simpleSpineEventListener;
  public MeshRenderer SpineMonster;
  public SpriteRenderer Shadow;
  public ColliderEvents damageColliderEvents;
  private Coroutine damageColliderCoroutine;
  public CircleCollider2D Collider;
  public GameObject CameraTarget;
  public float SeperationRadius = 0.5f;
  private GameObject TargetObject;
  public Vector2 Range = new Vector2(6f, 3f);
  public float KnockbackSpeed = 0.1f;
  public SimpleSpineAnimator simpleSpineAnimator;
  public SimpleSpineFlash simpleSpineFlash;
  public GameObject poisonBomb;
  public Transform BombPoint;
  public GameObject SpawnWeb;
  public GameObject SpawnSlime;
  [SerializeField]
  private GameObject bulletPrefab;
  [SerializeField]
  public float numberOfShotsToFire = 45f;
  [SerializeField]
  private Vector2 gravSpeed;
  [SerializeField]
  private float arc;
  [SerializeField]
  private Vector2 randomArcOffset = new Vector2(0.0f, 0.0f);
  [SerializeField]
  private Vector2 shootDistanceRange = new Vector2(2f, 3f);
  [SerializeField]
  private GameObject ShootBone;
  [Space]
  public ProjectilePatternBase LandProjectilePattern;
  private List<SpiderNest> spiderNests = new List<SpiderNest>();
  private bool active;
  private Health EnemyHealth;
  private float FireWebsDelay;
  private EnemySpiderMonster.BombType CurrentBombType;
  private int Ammo;
  private float ChargeDelay;
  private bool JumpAttacking;
  private bool JumpAroundAttacking;
  private float ZipAwayDelay;
  private Vector3 ShadowSize;
  private Coroutine cShrinkShadow;
  private float CloseRangeAttackDelay;
  private float AttackDashSpeed;
  private float AttackSpeedValue = 0.6f;
  private bool DontPlayHurtAnimation = true;
  private bool Roared;

  public override void Awake()
  {
    base.Awake();
    this.spiderNests = ((IEnumerable<SpiderNest>) UnityEngine.Object.FindObjectsOfType<SpiderNest>()).ToList<SpiderNest>();
    foreach (SpiderNest spiderNest in this.spiderNests)
      spiderNest.Droppable = false;
  }

  public override void OnEnable()
  {
    base.OnEnable();
    if (!GameManager.RoomActive || !this.active)
      return;
    this.health.enabled = true;
    this.StartCoroutine((IEnumerator) this.Roar());
    this.StartCoroutine((IEnumerator) this.DelayAddCamera());
  }

  private IEnumerator DelayAddCamera()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemySpiderMonster enemySpiderMonster = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      GameManager.GetInstance().AddToCamera(enemySpiderMonster.gameObject, 0.25f);
      GameManager.GetInstance().CamFollowTarget.MinZoom = 9f;
      GameManager.GetInstance().CamFollowTarget.MaxZoom = 18f;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(1f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private void OnSpineEvent(string EventName)
  {
    // ISSUE: reference to a compiler-generated method
    switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(EventName))
    {
      case 502195891:
        if (!(EventName == "roar shake"))
          break;
        CameraManager.instance.ShakeCameraForDuration(0.1f, 0.3f, 1f);
        CameraManager.shakeCamera(0.5f, Utils.GetAngle(this.transform.position, this.TargetObject.transform.position));
        break;
      case 545011306:
        if (!(EventName == "turn on colliders"))
          break;
        this.SeperateObject = true;
        Physics2D.IgnoreCollision((Collider2D) this.Collider, (Collider2D) PlayerFarming.Instance.circleCollider2D, false);
        this.health.invincible = false;
        break;
      case 1017970633:
        if (!(EventName == "battlecry"))
          break;
        AudioManager.Instance.PlayOneShot("event:/enemy/shoot_acidslime", this.gameObject);
        break;
      case 1207211535:
        if (!(EventName == "show name"))
          break;
        HUD_DisplayName.Play("Kumo");
        break;
      case 1274151103:
        if (!(EventName == "jump attack"))
          break;
        this.JumpAttacking = false;
        this.speed = 0.0f;
        this.DoDamageAttack(0.0f);
        AudioManager.Instance.PlayOneShot("event:/boss/spider/jump_attack_land", this.gameObject);
        if (!this.simpleSpineAnimator.IsVisible)
          break;
        CameraManager.instance.ShakeCameraForDuration(0.3f, 0.4f, 0.3f);
        break;
      case 1454297646:
        if (!(EventName == "grow shadow"))
          break;
        this.StartCoroutine((IEnumerator) this.GrowShadow());
        break;
      case 1785338614:
        if (!(EventName == "turn off colliders"))
          break;
        this.SeperateObject = false;
        Physics2D.IgnoreCollision((Collider2D) this.Collider, (Collider2D) PlayerFarming.Instance.circleCollider2D, true);
        this.health.invincible = true;
        break;
      case 1844065948:
        if (!(EventName == "land") || !this.JumpAroundAttacking)
          break;
        this.StartCoroutine((IEnumerator) this.LandProjectilePattern.ShootIE());
        this.JumpAroundAttacking = false;
        break;
      case 2151091521:
        int num1 = EventName == "start flash" ? 1 : 0;
        break;
      case 2163513076:
        if (!(EventName == "shrink shadow"))
          break;
        this.cShrinkShadow = this.StartCoroutine((IEnumerator) this.ShrinkShadow());
        break;
      case 2704129201:
        if (!(EventName == "webshot"))
          break;
        AudioManager.Instance.PlayOneShot("event:/enemy/shoot_acidslime", this.gameObject);
        break;
      case 3049036174:
        if (!(EventName == "mutate zoom"))
          break;
        GameManager.GetInstance().OnConversationNext(this.CameraTarget, 6f);
        break;
      case 3155283432:
        if (!(EventName == "deal damage"))
          break;
        this.DoDamageAttack(0.0f);
        break;
      case 3399030161:
        if (!(EventName == "bomb"))
          break;
        Debug.Log((object) ("BOMB!   " + (object) this.CurrentBombType));
        AudioManager.Instance.PlayOneShot("event:/boss/spider/bomb_shoot", this.gameObject);
        switch (this.CurrentBombType)
        {
          case EnemySpiderMonster.BombType.Web:
            float num2 = (float) (120 * this.Ammo) * ((float) Math.PI / 180f);
            UnityEngine.Object.Instantiate<GameObject>(this.poisonBomb, (Vector3) (UnityEngine.Random.insideUnitCircle * 8f), Quaternion.identity, this.transform.parent).GetComponent<MortarBomb>().Play(this.BombPoint.position, 2f, Health.Team.Team2);
            ++this.Ammo;
            break;
          case EnemySpiderMonster.BombType.Egg:
            for (int index = 0; (double) index < (double) this.numberOfShotsToFire; ++index)
            {
              float angle = Utils.GetAngle(this.transform.position, PlayerFarming.Instance.transform.position);
              UnityEngine.Object.Instantiate<GameObject>(this.bulletPrefab, this.ShootBone.transform.position, Quaternion.identity).GetComponent<GrenadeBullet>().Play(-6f, angle + UnityEngine.Random.Range(this.randomArcOffset.x, this.randomArcOffset.y), UnityEngine.Random.Range(this.shootDistanceRange.x, this.shootDistanceRange.y), UnityEngine.Random.Range(this.gravSpeed.x, this.gravSpeed.y));
            }
            break;
        }
        CameraManager.shakeCamera(0.3f, Utils.GetAngle(this.transform.position, this.TargetObject.transform.position));
        break;
      case 4013347183:
        int num3 = EventName == "stop flash" ? 1 : 0;
        break;
      case 4049887943:
        if (!(EventName == "dash attack"))
          break;
        CameraManager.shakeCamera(0.4f, this.state.facingAngle);
        this.AttackDashSpeed = this.AttackSpeedValue;
        break;
      case 4077446761:
        if (!(EventName == "die-explode"))
          break;
        CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
        MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
        AudioManager.Instance.PlayOneShot("event:/enemy/impact_squishy", this.gameObject);
        AudioManager.Instance.PlayOneShot("event:/enemy/impact_squishy", this.gameObject);
        break;
    }
  }

  private void DoDamageAttack(float startDelay)
  {
    if (this.damageColliderCoroutine != null)
      this.StopCoroutine(this.damageColliderCoroutine);
    this.damageColliderCoroutine = this.StartCoroutine((IEnumerator) this.DoDamageAttackTimed(startDelay, 0.1f));
  }

  private IEnumerator DoDamageAttackTimed(float startDelay, float dur)
  {
    if (!((UnityEngine.Object) this.damageColliderEvents == (UnityEngine.Object) null))
    {
      yield return (object) new WaitForSeconds(startDelay);
      this.damageColliderEvents.SetActive(true);
      yield return (object) new WaitForSeconds(dur);
      this.damageColliderEvents.SetActive(false);
      this.damageColliderCoroutine = (Coroutine) null;
    }
  }

  private IEnumerator WaitForTarget()
  {
    float Timer = 0.0f;
    while ((double) (Timer += Time.deltaTime) < 0.5)
      yield return (object) null;
    while ((UnityEngine.Object) this.TargetObject == (UnityEngine.Object) null)
    {
      this.TargetObject = PlayerFarming.Instance.gameObject;
      yield return (object) null;
    }
  }

  private IEnumerator ChasePlayer()
  {
    EnemySpiderMonster enemySpiderMonster = this;
    if (!GameManager.GetInstance()._CamFollowTarget.Contains(enemySpiderMonster.gameObject))
    {
      GameManager.GetInstance().AddToCamera(enemySpiderMonster.gameObject, 0.25f);
      GameManager.GetInstance().CamFollowTarget.MinZoom = 9f;
      GameManager.GetInstance().CamFollowTarget.MaxZoom = 18f;
    }
    enemySpiderMonster.state.CURRENT_STATE = StateMachine.State.Idle;
    if ((UnityEngine.Object) enemySpiderMonster.damageColliderEvents != (UnityEngine.Object) null)
      enemySpiderMonster.damageColliderEvents.SetActive(false);
    float ActionDelay = enemySpiderMonster.Roared ? UnityEngine.Random.Range(0.3f, 0.5f) : UnityEngine.Random.Range(0.5f, 1f);
    enemySpiderMonster.givePath(enemySpiderMonster.TargetObject.transform.position + (Vector3) (UnityEngine.Random.insideUnitCircle * UnityEngine.Random.Range(1f, 2f)));
    enemySpiderMonster.speed = 0.0f;
    float RepathTimer = 0.0f;
    while (true)
    {
      if ((double) enemySpiderMonster.speed < (double) enemySpiderMonster.maxSpeed)
        enemySpiderMonster.speed += 0.01f * Time.deltaTime;
      enemySpiderMonster.state.facingAngle = Utils.SmoothAngle(enemySpiderMonster.state.facingAngle, Utils.GetAngle(enemySpiderMonster.transform.position, enemySpiderMonster.TargetObject.transform.position), 10f);
      if ((double) (RepathTimer += Time.deltaTime) > 0.25)
      {
        enemySpiderMonster.givePath(enemySpiderMonster.TargetObject.transform.position + (Vector3) (UnityEngine.Random.insideUnitCircle * UnityEngine.Random.Range(1f, 2f)));
        RepathTimer = 0.0f;
        AudioManager.Instance.PlayOneShot("event:/boss/spider/footstep", enemySpiderMonster.gameObject);
      }
      enemySpiderMonster.CloseRangeAttackDelay -= Time.deltaTime;
      enemySpiderMonster.ZipAwayDelay -= Time.deltaTime;
      enemySpiderMonster.ChargeDelay -= Time.deltaTime;
      enemySpiderMonster.FireWebsDelay -= Time.deltaTime;
      float num = Vector3.Distance(enemySpiderMonster.transform.position, enemySpiderMonster.TargetObject.transform.position);
      if ((double) (ActionDelay -= Time.deltaTime) <= 0.0)
      {
        if ((double) num >= 5.0 || (double) enemySpiderMonster.CloseRangeAttackDelay > 0.0)
        {
          if ((double) enemySpiderMonster.FireWebsDelay > 0.0)
          {
            if ((double) enemySpiderMonster.ZipAwayDelay > 0.0)
            {
              if ((double) enemySpiderMonster.ChargeDelay <= 0.0)
                goto label_17;
            }
            else
              goto label_15;
          }
          else
            goto label_13;
        }
        else
          break;
      }
      yield return (object) null;
    }
    enemySpiderMonster.StartCoroutine((IEnumerator) enemySpiderMonster.CloseRangeAttack());
    yield break;
label_13:
    enemySpiderMonster.StartCoroutine((IEnumerator) enemySpiderMonster.FireWebs());
    yield break;
label_15:
    enemySpiderMonster.StartCoroutine((IEnumerator) enemySpiderMonster.ZipAway());
    yield break;
label_17:
    if (UnityEngine.Random.Range(0, 2) == 0)
      enemySpiderMonster.StartCoroutine((IEnumerator) enemySpiderMonster.JumpAttack());
    else
      enemySpiderMonster.StartCoroutine((IEnumerator) enemySpiderMonster.JumpAround());
  }

  private IEnumerator FireWebs()
  {
    EnemySpiderMonster enemySpiderMonster = this;
    enemySpiderMonster.Ammo = 0;
    enemySpiderMonster.state.CURRENT_STATE = StateMachine.State.Attacking;
    if (enemySpiderMonster.CurrentBombType == EnemySpiderMonster.BombType.Web)
      enemySpiderMonster.simpleSpineAnimator.Animate("bomb-even-more", 0, false);
    else
      enemySpiderMonster.simpleSpineAnimator.Animate(enemySpiderMonster.Roared ? "bomb-more" : "bombs", 0, false);
    enemySpiderMonster.simpleSpineAnimator.AddAnimate("idle-boss", 0, true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/boss/spider/bomb_start", enemySpiderMonster.gameObject);
    if (enemySpiderMonster.CurrentBombType == EnemySpiderMonster.BombType.Web)
      yield return (object) new WaitForSeconds(4.3f);
    else
      yield return (object) new WaitForSeconds(enemySpiderMonster.Roared ? 3f : 2.3f);
    AudioManager.Instance.PlayOneShot("event:/boss/spider/bomb_end", enemySpiderMonster.gameObject);
    enemySpiderMonster.FireWebsDelay = 2f;
    enemySpiderMonster.CurrentBombType = (EnemySpiderMonster.BombType) UnityEngine.Random.Range(0, 2);
    enemySpiderMonster.StartCoroutine((IEnumerator) enemySpiderMonster.ChasePlayer());
  }

  private IEnumerator JumpAttack()
  {
    EnemySpiderMonster enemySpiderMonster = this;
    enemySpiderMonster.state.CURRENT_STATE = StateMachine.State.Attacking;
    enemySpiderMonster.simpleSpineAnimator.Animate("angry intro", 0, true);
    AudioManager.Instance.PlayOneShot("event:/boss/spider/angry", enemySpiderMonster.gameObject);
    yield return (object) new WaitForSeconds(1f);
    AudioManager.Instance.PlayOneShot("event:/boss/spider/jump_attack_jump", enemySpiderMonster.gameObject);
    int Loop = UnityEngine.Random.Range(2, 5);
    while (Loop > 0)
    {
      float degree = Utils.GetAngle(enemySpiderMonster.transform.position, enemySpiderMonster.TargetObject.transform.position);
      int num = 0;
      while (num++ < 32 /*0x20*/ && (bool) Physics2D.Raycast((Vector2) enemySpiderMonster.transform.position, Utils.DegreeToVector2(degree), 5f, (int) enemySpiderMonster.layerToCheck))
        degree = Mathf.Repeat(degree + (float) UnityEngine.Random.Range(0, 360), 360f);
      enemySpiderMonster.state.facingAngle = degree;
      enemySpiderMonster.JumpAttacking = true;
      enemySpiderMonster.simpleSpineAnimator.Animate("jump-attack", 0, false);
      CameraManager.shakeCamera(0.3f, enemySpiderMonster.state.facingAngle);
      while (enemySpiderMonster.JumpAttacking)
      {
        enemySpiderMonster.speed = 15f * Time.deltaTime;
        yield return (object) null;
      }
      yield return (object) new WaitForSeconds(0.5f);
      --Loop;
      yield return (object) null;
    }
    enemySpiderMonster.ChargeDelay = 1f;
    enemySpiderMonster.StartCoroutine((IEnumerator) enemySpiderMonster.ChasePlayer());
  }

  private IEnumerator JumpAround()
  {
    EnemySpiderMonster enemySpiderMonster = this;
    enemySpiderMonster.state.CURRENT_STATE = StateMachine.State.Attacking;
    enemySpiderMonster.simpleSpineAnimator.Animate("angry intro", 0, true);
    AudioManager.Instance.PlayOneShot("event:/boss/spider/angry", enemySpiderMonster.gameObject);
    yield return (object) new WaitForSeconds(1f);
    AudioManager.Instance.PlayOneShot("event:/boss/spider/jump_attack_jump", enemySpiderMonster.gameObject);
    int Loop = UnityEngine.Random.Range(2, 5);
    while (Loop > 0)
    {
      float degree = Utils.GetAngle(enemySpiderMonster.transform.position, (Vector3) (UnityEngine.Random.insideUnitCircle * 5f));
      int num = 0;
      while (num++ < 32 /*0x20*/ && (bool) Physics2D.Raycast((Vector2) enemySpiderMonster.transform.position, Utils.DegreeToVector2(degree), 5f, (int) enemySpiderMonster.layerToCheck))
        degree = Mathf.Repeat(degree + (float) UnityEngine.Random.Range(0, 360), 360f);
      enemySpiderMonster.state.facingAngle = degree;
      enemySpiderMonster.JumpAroundAttacking = true;
      enemySpiderMonster.simpleSpineAnimator.Animate("jump", 0, false);
      CameraManager.shakeCamera(0.3f, enemySpiderMonster.state.facingAngle);
      while (enemySpiderMonster.JumpAroundAttacking)
      {
        enemySpiderMonster.speed = 15f * Time.deltaTime;
        yield return (object) null;
      }
      yield return (object) new WaitForSeconds(0.5f);
      --Loop;
      yield return (object) null;
    }
    enemySpiderMonster.ChargeDelay = 1f;
    enemySpiderMonster.StartCoroutine((IEnumerator) enemySpiderMonster.ChasePlayer());
  }

  private IEnumerator ZipAway()
  {
    EnemySpiderMonster enemySpiderMonster = this;
    enemySpiderMonster.state.CURRENT_STATE = StateMachine.State.Attacking;
    enemySpiderMonster.simpleSpineAnimator.Animate("swing-away", 0, false);
    AudioManager.Instance.PlayOneShot("event:/boss/spider/swing_away", enemySpiderMonster.gameObject);
    AudioManager.Instance.PlayOneShot("event:/dialogue/dun4_cult_leader_shamura/battle_cry_shamura", enemySpiderMonster.gameObject);
    yield return (object) new WaitForSeconds(2.5f);
    enemySpiderMonster.transform.position = enemySpiderMonster.TargetObject.transform.position;
    enemySpiderMonster.simpleSpineAnimator.Animate("swing-in", 0, false);
    AudioManager.Instance.PlayOneShot("event:/boss/spider/swing_in", enemySpiderMonster.gameObject);
    yield return (object) new WaitForSeconds(0.5f);
    CameraManager.shakeCamera(0.7f, Utils.GetAngle(enemySpiderMonster.transform.position, enemySpiderMonster.TargetObject.transform.position));
    int num = 1;
    for (int index = 0; index < num && enemySpiderMonster.spiderNests.Count > 0; ++index)
    {
      SpiderNest spiderNest = enemySpiderMonster.spiderNests[UnityEngine.Random.Range(0, enemySpiderMonster.spiderNests.Count)];
      spiderNest.Droppable = true;
      enemySpiderMonster.spiderNests.Remove(spiderNest);
      spiderNest.DropEnemies();
    }
    foreach (SpiderNest spiderNest in enemySpiderMonster.spiderNests)
      spiderNest.Shake();
    yield return (object) new WaitForSeconds(0.65f);
    enemySpiderMonster.ZipAwayDelay = 4f;
    enemySpiderMonster.StartCoroutine((IEnumerator) enemySpiderMonster.ChasePlayer());
  }

  private IEnumerator ShrinkShadow()
  {
    float Scale = 1f;
    while ((double) Scale > 0.0)
    {
      Scale -= Time.deltaTime * 5f;
      this.Shadow.transform.localScale = this.ShadowSize * Scale;
      yield return (object) null;
    }
    this.Shadow.transform.localScale = Vector3.zero;
  }

  private IEnumerator GrowShadow()
  {
    EnemySpiderMonster enemySpiderMonster = this;
    if (enemySpiderMonster.cShrinkShadow != null)
      enemySpiderMonster.StopCoroutine(enemySpiderMonster.cShrinkShadow);
    enemySpiderMonster.cShrinkShadow = (Coroutine) null;
    float Scale = 0.0f;
    while ((double) Scale < 1.0)
    {
      Scale += Time.deltaTime * 3f;
      enemySpiderMonster.Shadow.transform.localScale = enemySpiderMonster.ShadowSize * Scale;
      yield return (object) null;
    }
    enemySpiderMonster.Shadow.transform.localScale = enemySpiderMonster.ShadowSize;
  }

  private IEnumerator CloseRangeAttack()
  {
    EnemySpiderMonster enemySpiderMonster = this;
    AudioManager.Instance.PlayOneShot("event:/boss/spider/attack", enemySpiderMonster.gameObject);
    enemySpiderMonster.state.CURRENT_STATE = StateMachine.State.Attacking;
    enemySpiderMonster.simpleSpineAnimator.Animate("attack", 0, false);
    enemySpiderMonster.simpleSpineAnimator.AddAnimate("idle-boss", 0, true, 0.0f);
    enemySpiderMonster.state.facingAngle = Utils.GetAngle(enemySpiderMonster.transform.position, enemySpiderMonster.TargetObject.transform.position);
    enemySpiderMonster.AttackDashSpeed = 0.0f;
    float Duration = 2f;
    enemySpiderMonster.DoDamageAttack(0.5f);
    while ((double) (Duration -= Time.deltaTime) > 0.0)
    {
      if ((double) enemySpiderMonster.AttackDashSpeed > 0.0)
      {
        enemySpiderMonster.AttackDashSpeed -= 3f * Time.deltaTime;
        enemySpiderMonster.speed = enemySpiderMonster.AttackDashSpeed;
      }
      yield return (object) null;
    }
    enemySpiderMonster.CloseRangeAttackDelay = 1f;
    enemySpiderMonster.StartCoroutine((IEnumerator) enemySpiderMonster.ChasePlayer());
  }

  public void Play()
  {
    this.simpleSpineEventListener = this.GetComponent<SimpleSpineEventListener>();
    this.simpleSpineEventListener.OnSpineEvent += new SimpleSpineEventListener.SpineEvent(this.OnSpineEvent);
    this.simpleSpineEventListener.skeletonAnimation.ForceVisible = true;
    this.StartCoroutine((IEnumerator) this.WaitForTarget());
    this.SeperateObject = true;
    this.StartPosition = this.transform.position;
    this.ShadowSize = this.Shadow.transform.localScale;
    this.TargetObject = PlayerFarming.Instance.gameObject;
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.damageColliderEvents.SetActive(false);
    }
    this.active = true;
    this.StartCoroutine((IEnumerator) this.ChasePlayer());
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    this.simpleSpineFlash.FlashFillRed(0.25f);
    AudioManager.Instance.PlayOneShot("event:/boss/spider/gethit", this.gameObject);
    CameraManager.shakeCamera(0.3f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
    BiomeConstants.Instance.EmitHitVFX(AttackLocation - Vector3.back * 0.5f, Quaternion.identity.z, "HitFX_Weak");
    if (!this.Roared && (double) this.health.HP <= (double) this.health.totalHP * 0.5)
    {
      AudioManager.Instance.PlayOneShot("event:/dialogue/dun4_cult_leader_shamura/wounded_shamura", this.gameObject);
      this.StopAllCoroutines();
      this.DisableForces = false;
      if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
        this.damageColliderEvents.SetActive(false);
      this.StartCoroutine((IEnumerator) this.Roar());
    }
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    PlayerFarming.Instance.health.invincible = true;
    if (this.state.CURRENT_STATE == StateMachine.State.Dieing)
      return;
    AchievementsWrapper.UnlockAchievement(Achievements.Instance.Lookup("KILL_BOSS_4"));
    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0.0f);
    this.knockBackVX = (float) (-(double) this.KnockbackSpeed * 1.0) * Mathf.Cos(Utils.GetAngle(this.transform.position, Attacker.transform.position) * ((float) Math.PI / 180f));
    this.knockBackVY = (float) (-(double) this.KnockbackSpeed * 1.0) * Mathf.Sin(Utils.GetAngle(this.transform.position, Attacker.transform.position) * ((float) Math.PI / 180f));
    GameObject gameObject = BiomeConstants.Instance.GroundSmash_Medium.Spawn();
    gameObject.transform.position = this.transform.position;
    gameObject.transform.rotation = Quaternion.identity;
    this.damageColliderEvents.gameObject.SetActive(false);
    CameraManager.shakeCamera(0.5f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
    this.ClearPaths();
    this.StopAllCoroutines();
    this.DisableForces = false;
    this.StartCoroutine((IEnumerator) this.Die());
  }

  private IEnumerator Roar()
  {
    EnemySpiderMonster enemySpiderMonster = this;
    AudioManager.Instance.PlayOneShot("event:/boss/spider/roar", enemySpiderMonster.gameObject);
    int count1 = EnemySlime.Slimes.Count;
    while (--count1 > 0)
    {
      EnemySlime slime = EnemySlime.Slimes[count1];
      slime.ExplodeOnDeath = false;
      slime.health.DestroyNextFrame();
    }
    int count2 = EnemySpawner.EnemySpawners.Count;
    while (--count2 > 0)
      UnityEngine.Object.Destroy((UnityEngine.Object) EnemySpawner.EnemySpawners[count2]);
    enemySpiderMonster.ClearPaths();
    enemySpiderMonster.health.invincible = true;
    enemySpiderMonster.speed = 0.0f;
    enemySpiderMonster.simpleSpineAnimator.Animate("idle-boss", 0, false);
    yield return (object) new WaitForSeconds(0.5f);
    enemySpiderMonster.state.CURRENT_STATE = StateMachine.State.Attacking;
    enemySpiderMonster.simpleSpineAnimator.SetSkin("NoMask");
    enemySpiderMonster.simpleSpineAnimator.Animate("roar", 0, false);
    enemySpiderMonster.simpleSpineAnimator.AddAnimate("idle-boss", 0, true, 0.0f);
    CameraManager.instance.ShakeCameraForDuration(2f, 2.5f, 2f);
    enemySpiderMonster.DontPlayHurtAnimation = true;
    yield return (object) new WaitForSeconds(2.3f);
    enemySpiderMonster.Roared = true;
    enemySpiderMonster.DontPlayHurtAnimation = false;
    enemySpiderMonster.health.invincible = false;
    if (UnityEngine.Random.Range(0, 2) == 0)
      enemySpiderMonster.StartCoroutine((IEnumerator) enemySpiderMonster.JumpAttack());
    else
      enemySpiderMonster.StartCoroutine((IEnumerator) enemySpiderMonster.JumpAround());
    yield return (object) null;
  }

  private IEnumerator Die()
  {
    EnemySpiderMonster enemySpiderMonster = this;
    AudioManager.Instance.PlayOneShot("event:/boss/spider/death", enemySpiderMonster.gameObject);
    enemySpiderMonster.ClearPaths();
    enemySpiderMonster.speed = 0.0f;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(enemySpiderMonster.CameraTarget, 7f);
    enemySpiderMonster.simpleSpineAnimator.FlashWhite(false);
    enemySpiderMonster.state.CURRENT_STATE = StateMachine.State.Dieing;
    enemySpiderMonster.rb.velocity = (Vector2) Vector3.zero;
    enemySpiderMonster.rb.isKinematic = true;
    enemySpiderMonster.rb.simulated = false;
    enemySpiderMonster.rb.bodyType = RigidbodyType2D.Static;
    if ((double) enemySpiderMonster.transform.position.x > 11.0)
      enemySpiderMonster.transform.position = new Vector3(11f, enemySpiderMonster.transform.position.y, 0.0f);
    if ((double) enemySpiderMonster.transform.position.x < -11.0)
      enemySpiderMonster.transform.position = new Vector3(-11f, enemySpiderMonster.transform.position.y, 0.0f);
    if ((double) enemySpiderMonster.transform.position.y > 7.0)
      enemySpiderMonster.transform.position = new Vector3(enemySpiderMonster.transform.position.x, 7f, 0.0f);
    if ((double) enemySpiderMonster.transform.position.y < -7.0)
      enemySpiderMonster.transform.position = new Vector3(enemySpiderMonster.transform.position.x, -7f, 0.0f);
    yield return (object) new WaitForEndOfFrame();
    enemySpiderMonster.simpleSpineFlash.StopAllCoroutines();
    enemySpiderMonster.DisableForces = false;
    enemySpiderMonster.simpleSpineFlash.SetColor(new Color(0.0f, 0.0f, 0.0f, 0.0f));
    TrapPoison.RemoveAllPoison();
    for (int index = Health.team2.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) Health.team2[index] != (UnityEngine.Object) enemySpiderMonster.health && (UnityEngine.Object) Health.team2[index] != (UnityEngine.Object) null && Health.team2[index].gameObject.activeInHierarchy)
      {
        Health.team2[index].invincible = false;
        Health.team2[index].untouchable = false;
        if ((UnityEngine.Object) Health.team2[index].GetComponent<SpawnEnemyOnDeath>() != (UnityEngine.Object) null)
          Health.team2[index].GetComponent<SpawnEnemyOnDeath>().SpawnEnemies = false;
        Health.team2[index].DealDamage(Health.team2[index].totalHP, enemySpiderMonster.gameObject, enemySpiderMonster.transform.position, AttackType: Health.AttackTypes.Heavy);
      }
    }
    yield return (object) new WaitForEndOfFrame();
    if (!DataManager.Instance.BossesCompleted.Contains(PlayerFarming.Location))
    {
      enemySpiderMonster.simpleSpineAnimator.Animate("die", 0, false);
      enemySpiderMonster.simpleSpineAnimator.AddAnimate("dead", 0, true, 0.0f);
    }
    else
    {
      enemySpiderMonster.simpleSpineAnimator.Animate("die-noheart", 0, false);
      enemySpiderMonster.simpleSpineAnimator.AddAnimate("dead-noheart", 0, true, 0.0f);
    }
    yield return (object) new WaitForSeconds(2.7f);
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationEnd();
    GameManager.GetInstance().RemoveFromCamera(enemySpiderMonster.gameObject);
    GameManager.GetInstance().CamFollowTarget.MinZoom = 6f;
    GameManager.GetInstance().CamFollowTarget.MaxZoom = 20f;
    if (!DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_4))
      enemySpiderMonster.interaction_MonsterHeart.ObjectiveToComplete = Objectives.CustomQuestTypes.BishopsOfTheOldFaith4;
    enemySpiderMonster.interaction_MonsterHeart.Play();
    enemySpiderMonster.enabled = false;
  }

  private void OnDamageTriggerEnter(Collider2D collider)
  {
    this.EnemyHealth = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) this.EnemyHealth != (UnityEngine.Object) null) || this.EnemyHealth.team == this.health.team)
      return;
    this.EnemyHealth.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f));
  }

  private enum BombType
  {
    Web,
    Egg,
    Count,
  }
}
