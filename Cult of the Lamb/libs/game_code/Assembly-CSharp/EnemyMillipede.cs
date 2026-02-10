// Decompiled with JetBrains decompiler
// Type: EnemyMillipede
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class EnemyMillipede : UnitObject
{
  public SkeletonAnimation Spine;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string idleAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string movingAnimation;
  [SerializeField]
  public float rayDistance;
  [SerializeField]
  public float turnDamper;
  [SerializeField]
  public LayerMask avoidLayers;
  [SerializeField]
  public float knockModifier;
  [SerializeField]
  public Vector2 turnExaggerationRange = new Vector2(30f, 120f);
  [SerializeField]
  public float invincibleTime;
  public Health[] bodyParts;
  public List<SimpleSpineFlash> flashes;
  public List<SkeletonAnimation> spines;
  public ColliderEvents[] colliderEvents;
  public bool focusOnTarget;
  public bool damaged;
  public float directionChanger;
  public float directionRand = 1f;
  public float subtleOffset;

  public override void Awake()
  {
    base.Awake();
    this.bodyParts = this.GetComponentsInChildren<Health>();
    this.spines = ((IEnumerable<SkeletonAnimation>) this.GetComponentsInChildren<SkeletonAnimation>()).ToList<SkeletonAnimation>();
    this.flashes = ((IEnumerable<SimpleSpineFlash>) this.GetComponentsInChildren<SimpleSpineFlash>()).ToList<SimpleSpineFlash>();
    this.colliderEvents = this.GetComponentsInChildren<ColliderEvents>(true);
    foreach (ColliderEvents colliderEvent in this.colliderEvents)
    {
      colliderEvent.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      colliderEvent.gameObject.SetActive(false);
    }
  }

  public override void OnEnable()
  {
    base.OnEnable();
    foreach (SimpleSpineFlash flash in this.flashes)
      flash.FlashWhite(false);
    foreach (SkeletonAnimation spine in this.spines)
    {
      if ((bool) (Object) spine)
        spine.AnimationState.SetAnimation(0, "run", true);
    }
    foreach (Health bodyPart in this.bodyParts)
    {
      bodyPart.invincible = false;
      if ((Object) bodyPart != (Object) this.health)
        bodyPart.CanSpawnTarotSinOnDie = false;
    }
    this.DisableForces = false;
  }

  public virtual void Start() => this.LookAtAngle((float) Random.Range(0, 360));

  public override void Update()
  {
    if (!this.DisableForces)
    {
      this.speed = this.maxSpeed * this.SpeedMultiplier;
      if (float.IsNaN(this.state.facingAngle))
        this.LookAtAngle((float) Random.Range(0, 360));
      if ((double) Time.time > (double) this.directionChanger / (double) this.Spine.timeScale)
      {
        this.directionChanger = Time.time + Random.Range(0.5f, 1.5f);
        this.directionRand *= -1f;
      }
      Vector2 position1 = (Vector2) this.transform.position;
      Vector2 normalized1 = new Vector2(this.moveVX, this.moveVY).normalized;
      double rayDistance = (double) this.rayDistance;
      int avoidLayers1 = (int) this.avoidLayers;
      RaycastHit2D raycastHit2D1;
      if ((Object) (raycastHit2D1 = Physics2D.Raycast(position1, normalized1, (float) rayDistance, avoidLayers1)).collider != (Object) null)
      {
        float angle = Utils.GetAngle(Vector3.zero, (Vector3) new Vector2(this.moveVX, this.moveVY));
        float degree1 = Utils.Repeat(angle + 90f, 360f);
        float degree2 = Utils.Repeat(angle - 90f, 360f);
        Vector2 position2 = (Vector2) this.transform.position;
        Vector2 vector2 = Utils.DegreeToVector2(degree1);
        Vector2 normalized2 = vector2.normalized;
        double distance1 = (double) this.rayDistance / 2.0;
        int avoidLayers2 = (int) this.avoidLayers;
        RaycastHit2D raycastHit2D2 = Physics2D.Raycast(position2, normalized2, (float) distance1, avoidLayers2);
        Vector2 position3 = (Vector2) this.transform.position;
        vector2 = Utils.DegreeToVector2(degree2);
        Vector2 normalized3 = vector2.normalized;
        double distance2 = (double) this.rayDistance / 2.0;
        int avoidLayers3 = (int) this.avoidLayers;
        RaycastHit2D raycastHit2D3 = Physics2D.Raycast(position3, normalized3, (float) distance2, avoidLayers3);
        double num1 = (Object) raycastHit2D2.collider != (Object) null ? (double) Vector3.Distance(this.transform.position, (Vector3) raycastHit2D2.point) : double.PositiveInfinity;
        float num2 = (Object) raycastHit2D3.collider != (Object) null ? Vector3.Distance(this.transform.position, (Vector3) raycastHit2D3.point) : float.PositiveInfinity;
        if (float.IsNaN(this.state.facingAngle))
          this.state.facingAngle = angle;
        float num3 = num1 >= (double) num2 ? 0.5f : -0.5f;
        if (num1 == (double) num2)
          num3 = this.directionRand;
        float TargetAngle = Utils.Repeat(this.state.facingAngle + Random.Range(this.turnExaggerationRange.x, this.turnExaggerationRange.y), 360f);
        if (((Object) raycastHit2D2.collider == (Object) null && (Object) raycastHit2D3.collider == (Object) null || this.focusOnTarget) && (Object) this.GetClosestTarget() != (Object) null)
        {
          TargetAngle = Utils.GetAngle(this.transform.position, this.GetClosestTarget().transform.position);
          num3 = 1f;
        }
        float num4 = Vector3.Distance(this.transform.position, (Vector3) raycastHit2D1.point);
        this.LookAtAngle(Utils.SmoothAngle(this.state.facingAngle, TargetAngle, this.turnDamper * num4 * num3));
      }
      else
      {
        this.subtleOffset = Mathf.PingPong(Time.time, 4f) - 2f;
        this.LookAtAngle(this.state.facingAngle + this.subtleOffset);
      }
    }
    base.Update();
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/spider/gethit", this.gameObject);
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    foreach (SimpleSpineFlash flash in this.flashes)
      flash.FlashFillRed();
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/spider/death", this.gameObject);
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    foreach (Health bodyPart in this.bodyParts)
    {
      if ((Object) bodyPart != (Object) this.health)
      {
        bodyPart.enabled = true;
        bodyPart.DamageModifier = 1f;
        bodyPart.DealDamage(bodyPart.totalHP, this.gameObject, AttackLocation, AttackType: Health.AttackTypes.Heavy);
      }
    }
  }

  public void LookAtAngle(float angle)
  {
    this.state.facingAngle = angle;
    this.state.LookAngle = angle;
  }

  public void LookAtTarget()
  {
    if (!(bool) (Object) this.GetClosestTarget())
      return;
    this.LookAtAngle(Utils.GetAngle(this.transform.position, this.GetClosestTarget().transform.position));
  }

  public void EnableDamageColliders()
  {
    this.StartCoroutine((IEnumerator) this.EnableDamageCollidersIE());
  }

  public IEnumerator EnableDamageCollidersIE()
  {
    foreach (Component colliderEvent in this.colliderEvents)
      colliderEvent.gameObject.SetActive(true);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * this.Spine.timeScale) < 0.10000000149011612)
      yield return (object) null;
    foreach (Component colliderEvent in this.colliderEvents)
      colliderEvent.gameObject.SetActive(false);
  }

  public virtual void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((Object) component != (Object) null) || component.team == this.health.team && this.health.team != Health.Team.PlayerTeam || this.bodyParts.Contains<Health>(component))
      return;
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }

  public void SetAnimation(string animationName, bool loop = false)
  {
    foreach (SkeletonAnimation spine in this.spines)
      spine.AnimationState.SetAnimation(0, animationName, loop);
  }

  public void AddAnimation(string animationName, bool loop = false)
  {
    foreach (SkeletonAnimation spine in this.spines)
      spine.AnimationState.AddAnimation(0, animationName, loop, 0.0f);
  }

  public Vector3 GetCenterPosition()
  {
    Vector3 zero = Vector3.zero;
    foreach (SkeletonAnimation spine in this.spines)
      zero += spine.transform.position;
    return zero / (float) this.spines.Count;
  }

  public void DamageFromBody(
    GameObject attacker,
    Vector3 attackLocation,
    float damage,
    Health.AttackTypes attackType,
    Health.AttackFlags attackFlag)
  {
    if (this.damaged)
      return;
    this.health.DealDamage(damage, attacker, attackLocation, AttackType: attackType, AttackFlags: attackFlag);
    this.StartCoroutine((IEnumerator) this.InvincibleDelay());
  }

  public IEnumerator InvincibleDelay()
  {
    this.damaged = true;
    foreach (Health bodyPart in this.bodyParts)
      bodyPart.invincible = true;
    yield return (object) new WaitForSeconds(this.invincibleTime);
    this.damaged = false;
    foreach (Health bodyPart in this.bodyParts)
      bodyPart.invincible = false;
  }
}
