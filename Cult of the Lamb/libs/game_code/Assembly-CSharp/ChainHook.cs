// Decompiled with JetBrains decompiler
// Type: ChainHook
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class ChainHook : MonoBehaviour, ISpellOwning
{
  [Header("General")]
  [SerializeField]
  public Transform hook;
  [SerializeField]
  public Transform startChainPoint;
  [SerializeField]
  public GameObject owner;
  [SerializeField]
  public AnimationCurve moveProgress;
  [SerializeField]
  public float hideTime = 0.2f;
  [SerializeField]
  public float delayHideTime = 0.6f;
  [Header("Visual")]
  [SerializeField]
  public GameObject hook2DContainer;
  [SerializeField]
  public SpriteRenderer hook2DVisual;
  [SerializeField]
  public GameObject hook3DContainer;
  [SerializeField]
  public SpriteRenderer hook3DVisual;
  [SerializeField]
  public LineRenderer chain;
  [SerializeField]
  public List<WeaponTypeToSprite> spriteTypeDictionary;
  [SerializeField]
  public Sprite heavyAttackSprite;
  [SerializeField]
  public ParticleSystem bloodTrail;
  [Header("Bezier Settings")]
  [SerializeField]
  public float bezierScale = 0.3f;
  [Range(0.0f, 1f)]
  [SerializeField]
  public float durationMultiplier = 0.3f;
  [Range(0.0f, 1f)]
  [SerializeField]
  public float straightLineTime = 0.6f;
  [SerializeField]
  public float lineBreakSinLength = 2f;
  [Header("Collisions")]
  [SerializeField]
  public CircleCollider2D hookCollider;
  [SerializeField]
  public ColliderEvents hookColliderEvents;
  [SerializeField]
  public BoxCollider2D chainCollider;
  [SerializeField]
  public ColliderEvents chainColliderEvents;
  [SerializeField]
  public float chainDamageMultiplier = 0.5f;
  [SerializeField]
  public float chainCollisionWidth = 0.2f;
  public Action<Vector3> onComplete;
  public Action<Health> onDamageCollision;
  public EllipseMovement ellipseMovement;
  public EllipseMovement bezierEllipseMovement;
  public float currentTime;
  public float currentUnscaledTime;
  public bool updateTime = true;
  public AnimationCurve scaleMultiplierOverTime = new AnimationCurve();
  public bool chainDamage;
  public bool animatedHide = true;
  public bool hiding;
  public bool hideOnComplete = true;
  public float hideTimeMultiplier = 1f;
  public System.Action onHideStart;
  public float colliderEnabledDelay;
  public float colliderDisabledDelay = float.MaxValue;
  public List<Health> damagedEnemies = new List<Health>();
  public float team2DamageMult = 5f;
  public int randomBreakMultiplier = 1;
  public SkeletonAnimation ownerSpine;
  public Health ownerHealth;
  public Health.AttackTypes attackType;
  public Health.AttackFlags attackFlags;
  public float damage;
  public bool canHurtFriendlyUnits;
  public bool isTargetSet;
  public Transform target;
  public Vector3 targetPosition = Vector3.zero;
  public bool useBezier = true;
  public bool perpetual;
  public int xScaleDirection = 1;
  [Tooltip("Changes time change to opposite. It doesn't work properly for non-perpetual movement")]
  public bool invertedMovement;

  public float HideTime => this.hideTime * this.hideTimeMultiplier;

  public Transform Hook => this.hook;

  public void Init(
    EllipseMovement ellipseMovement,
    float damage,
    float hookDamageRadius,
    Health.AttackFlags attackFlags,
    bool animatedHide,
    AnimationCurve scaleMultiplierOverTime,
    bool chainDamage = true,
    float colliderEnabledDelay = 0.0f,
    float colliderDisabledDelay = 3.40282347E+38f,
    bool is3D = false,
    bool canHurtFriendlyUnits = false,
    Action<Vector3> onComplete = null,
    Action<Health> onDamageCollision = null,
    SkeletonAnimation ownerSpine = null,
    System.Action onHideStart = null)
  {
    this.ellipseMovement = ellipseMovement;
    this.bezierEllipseMovement = new EllipseMovement(ellipseMovement.Up, ellipseMovement.Right, ellipseMovement.CenterOffset * this.bezierScale, ellipseMovement.Radius1 * this.bezierScale, ellipseMovement.Radius2 * this.bezierScale, ellipseMovement.StartAngle, ellipseMovement.AngleToMove, ellipseMovement.Duration * this.durationMultiplier, ellipseMovement.RadiusMultiplierOverTime);
    this.damage = damage;
    this.attackFlags = attackFlags;
    this.canHurtFriendlyUnits = canHurtFriendlyUnits;
    this.currentTime = 0.0f;
    this.currentUnscaledTime = 0.0f;
    this.updateTime = true;
    this.scaleMultiplierOverTime = scaleMultiplierOverTime;
    this.animatedHide = animatedHide;
    this.hiding = false;
    this.hideTimeMultiplier = 1f;
    this.chainDamage = chainDamage;
    this.onComplete = onComplete;
    this.onHideStart = onHideStart;
    this.onDamageCollision = onDamageCollision;
    this.colliderEnabledDelay = colliderEnabledDelay;
    this.colliderDisabledDelay = colliderDisabledDelay;
    this.hook2DContainer.SetActive(!is3D);
    this.hook3DContainer.SetActive(is3D);
    this.useBezier = true;
    this.ownerSpine = ownerSpine;
    if ((UnityEngine.Object) this.bloodTrail != (UnityEngine.Object) null)
      this.bloodTrail.Play();
    this.attackType = Health.AttackTypes.Melee;
    this.target = (Transform) null;
    this.isTargetSet = false;
    this.damagedEnemies.Clear();
    this.randomBreakMultiplier = (double) UnityEngine.Random.Range(0.0f, 1f) >= 0.5 ? 1 : -1;
    this.hookCollider.radius = hookDamageRadius;
    this.hook.DOKill();
    this.PlayUpdatingTime();
    this.hook.position = this.GetEllipsePosition(ellipseMovement);
    this.HandleHookRotation();
    this.HandleChainCollider();
    this.HandleLineRendererPoints();
    this.gameObject.SetActive(true);
    if (is3D)
      return;
    this.xScaleDirection = (double) ellipseMovement.StartAngle <= 90.0 || (double) ellipseMovement.StartAngle >= 280.0 ? -1 : 1;
  }

  public void Init(
    EllipseMovement ellipseMovement,
    float damage,
    float hookDamageRadius,
    Health.AttackFlags attackFlags,
    bool animatedHide,
    AnimationCurve scaleMultiplierOverTime,
    bool useBezier,
    bool perpetual = false,
    bool chainDamage = true,
    float colliderEnabledDelay = 0.0f,
    float colliderDisabledDelay = 3.40282347E+38f,
    bool is3D = false,
    Action<Vector3> onComplete = null,
    Action<Health> onDamageCollision = null,
    SkeletonAnimation ownerSpine = null)
  {
    this.ellipseMovement = ellipseMovement;
    this.bezierEllipseMovement = new EllipseMovement(ellipseMovement.Up, ellipseMovement.Right, ellipseMovement.CenterOffset * this.bezierScale, ellipseMovement.Radius1 * this.bezierScale, ellipseMovement.Radius2 * this.bezierScale, ellipseMovement.StartAngle, ellipseMovement.AngleToMove, ellipseMovement.Duration * this.durationMultiplier, ellipseMovement.RadiusMultiplierOverTime);
    this.damage = damage;
    this.attackFlags = attackFlags;
    this.currentTime = 0.0f;
    this.currentUnscaledTime = 0.0f;
    this.updateTime = true;
    this.scaleMultiplierOverTime = scaleMultiplierOverTime;
    this.animatedHide = animatedHide;
    this.hiding = false;
    this.hideTimeMultiplier = 1f;
    this.chainDamage = chainDamage;
    this.onComplete = onComplete;
    this.onDamageCollision = onDamageCollision;
    this.colliderEnabledDelay = colliderEnabledDelay;
    this.colliderDisabledDelay = perpetual ? float.MaxValue : colliderDisabledDelay;
    this.hook2DContainer.SetActive(!is3D);
    this.hook3DContainer.SetActive(is3D);
    this.ownerSpine = ownerSpine;
    if ((UnityEngine.Object) this.bloodTrail != (UnityEngine.Object) null)
      this.bloodTrail.Play();
    this.useBezier = useBezier;
    this.perpetual = perpetual;
    this.attackType = Health.AttackTypes.Melee;
    this.target = (Transform) null;
    this.isTargetSet = false;
    this.damagedEnemies.Clear();
    this.randomBreakMultiplier = (double) UnityEngine.Random.Range(0.0f, 1f) >= 0.5 ? 1 : -1;
    this.hookCollider.radius = hookDamageRadius;
    this.hook.DOKill();
    this.PlayUpdatingTime();
    this.hook.position = this.GetEllipsePosition(ellipseMovement);
    this.HandleHookRotation();
    this.HandleChainCollider();
    this.HandleLineRendererPoints();
    this.gameObject.SetActive(true);
  }

  public void Awake()
  {
    this.ownerHealth = this.owner.GetComponent<Health>();
    this.ownerHealth.OnDie += new Health.DieAction(this.Health_OnDie);
    this.hookColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnHookCollision);
    this.chainColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnChainCollision);
  }

  public void OnDisable()
  {
    this.hiding = false;
    this.hideOnComplete = true;
    this.target = (Transform) null;
    this.isTargetSet = false;
    this.damagedEnemies.Clear();
    this.hook2DContainer.SetActive(true);
    this.hook3DContainer.SetActive(false);
    if ((UnityEngine.Object) this.bloodTrail != (UnityEngine.Object) null)
      this.bloodTrail.Stop();
    this.SetCollidersActive(true);
  }

  public void OnDestroy()
  {
    this.ownerHealth.OnDie -= new Health.DieAction(this.Health_OnDie);
    this.hookColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnHookCollision);
    this.chainColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnChainCollision);
  }

  public void Update()
  {
    if (this.animatedHide && !this.hiding)
    {
      float num = Mathf.Clamp(this.currentTime, 0.0f, this.ellipseMovement.Duration);
      Vector3 one = Vector3.one;
      one.y *= this.scaleMultiplierOverTime.Evaluate(num / this.ellipseMovement.Duration);
      this.hook.transform.localScale = new Vector3(one.x * (float) this.xScaleDirection, one.y, one.z);
    }
    if (this.isTargetSet)
      this.hook.position = !((UnityEngine.Object) this.target != (UnityEngine.Object) null) ? this.targetPosition : this.target.position;
    else if ((double) this.currentTime <= (double) this.ellipseMovement.Duration)
    {
      if (this.perpetual && (double) this.currentTime < 0.0)
        this.currentTime = this.ellipseMovement.Duration;
      this.hook.position = this.GetEllipsePosition(this.ellipseMovement);
    }
    else if (this.perpetual)
    {
      this.currentTime %= this.ellipseMovement.Duration;
      this.hook.position = this.GetEllipsePosition(this.ellipseMovement);
      this.damagedEnemies.Clear();
    }
    else if (this.animatedHide)
    {
      Action<Vector3> onComplete = this.onComplete;
      if (onComplete != null)
        onComplete(this.hook.position);
      this.gameObject.SetActive(false);
    }
    else if (!this.hiding)
    {
      this.hook.position = this.GetEllipsePosition(this.ellipseMovement);
      this.hiding = true;
      Action<Vector3> onComplete = this.onComplete;
      if (onComplete != null)
        onComplete(this.hook.position);
      if (this.hideOnComplete)
        this.Hide();
    }
    if ((UnityEngine.Object) this.bloodTrail != (UnityEngine.Object) null)
      this.bloodTrail.transform.position = this.hook2DContainer.transform.position;
    this.HandleHookRotation();
    this.HandleChainCollider();
    this.HandleLineRendererPoints();
  }

  public void FixedUpdate()
  {
    if (this.updateTime)
      this.currentTime = !((UnityEngine.Object) this.ownerSpine != (UnityEngine.Object) null) ? (this.invertedMovement ? this.currentTime - Time.fixedDeltaTime : this.currentTime + Time.fixedDeltaTime) : (this.invertedMovement ? this.currentTime - Time.fixedDeltaTime * this.ownerSpine.timeScale : this.currentTime + Time.fixedDeltaTime * this.ownerSpine.timeScale);
    this.currentUnscaledTime += Time.fixedDeltaTime;
  }

  public void SetCollidersActive(bool active)
  {
    this.hookCollider.enabled = active;
    this.chainCollider.enabled = active;
  }

  public void StopUpdatingTime() => this.updateTime = false;

  public void PlayUpdatingTime() => this.updateTime = true;

  public void Hide(bool instant = false)
  {
    this.hiding = true;
    this.hook.DOKill();
    if ((UnityEngine.Object) this.ownerSpine != (UnityEngine.Object) null)
      this.StartCoroutine((IEnumerator) this.HideRoutine(instant));
    else if (instant)
    {
      System.Action onHideStart = this.onHideStart;
      if (onHideStart != null)
        onHideStart();
      this.hook.DOMove(this.startChainPoint.position, this.HideTime).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InQuad).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
      {
        this.hiding = false;
        this.gameObject.SetActive(false);
      }));
    }
    else
    {
      this.StartCoroutine((IEnumerator) this.DelayedAction(this.onHideStart, this.delayHideTime));
      this.hook.DOMove(this.startChainPoint.position, this.HideTime).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(this.delayHideTime).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InQuad).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
      {
        this.hiding = false;
        this.gameObject.SetActive(false);
      }));
    }
  }

  public IEnumerator DelayedAction(System.Action action, float delay)
  {
    yield return (object) CoroutineStatics.WaitForScaledSeconds(delay, this.ownerSpine);
    System.Action action1 = action;
    if (action1 != null)
      action1();
  }

  public IEnumerator HideRoutine(bool instant)
  {
    ChainHook chainHook = this;
    Vector3 startPos = chainHook.hook.position;
    float seconds = instant ? 0.0f : chainHook.delayHideTime;
    float timeElapsed = 0.0f;
    if ((double) seconds > 0.0)
      yield return (object) CoroutineStatics.WaitForScaledSeconds(seconds, chainHook.ownerSpine);
    System.Action onHideStart = chainHook.onHideStart;
    if (onHideStart != null)
      onHideStart();
    while ((double) timeElapsed < (double) chainHook.HideTime)
    {
      float num1 = (UnityEngine.Object) chainHook.ownerSpine != (UnityEngine.Object) null ? chainHook.ownerSpine.timeScale : 1f;
      double num2 = (double) timeElapsed / (double) chainHook.HideTime;
      float t = (float) (num2 * num2);
      chainHook.hook.position = Vector3.Lerp(startPos, chainHook.startChainPoint.position, t);
      timeElapsed += Time.deltaTime * num1;
      yield return (object) null;
    }
    chainHook.hiding = false;
    chainHook.gameObject.SetActive(false);
  }

  public void HideThenDestroy()
  {
    this.hiding = true;
    this.hook.DOMove(this.startChainPoint.position, this.HideTime).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(this.delayHideTime).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InQuad).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      this.hiding = false;
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }));
  }

  public void SetHookTarget(Transform target)
  {
    this.target = target;
    this.isTargetSet = true;
  }

  public void SetHookTargetPosition(Vector3 targetPosition)
  {
    this.targetPosition = targetPosition;
    this.isTargetSet = true;
  }

  public void SetAttackType(Health.AttackTypes attackType) => this.attackType = attackType;

  public void SetVisuals(EquipmentType equipmentType)
  {
    Sprite sprite = (Sprite) null;
    for (int index = 0; index < this.spriteTypeDictionary.Count; ++index)
    {
      if (this.spriteTypeDictionary[index].equipmentType == equipmentType)
      {
        sprite = this.spriteTypeDictionary[index].sprite;
        break;
      }
    }
    this.hook2DVisual.sprite = sprite;
    this.hook3DVisual.sprite = sprite;
  }

  public void SetHeavyAttackVisuals()
  {
    this.hook2DVisual.sprite = this.heavyAttackSprite;
    this.hook3DVisual.sprite = this.heavyAttackSprite;
  }

  public void SetHideOnComplete(bool hideOnComplete) => this.hideOnComplete = hideOnComplete;

  public void HandleHookRotation()
  {
    this.hook.rotation = Quaternion.LookRotation(this.hook.position - (this.startChainPoint.position + this.ellipseMovement.CenterOffset), Vector3.back);
    if (!this.hook3DContainer.activeInHierarchy)
      return;
    this.hook.rotation *= Quaternion.Euler(Vector3.Lerp(Vector3.zero, new Vector3(90f, 0.0f, 0.0f), this.currentUnscaledTime / this.ellipseMovement.Duration));
  }

  public void HandleChainCollider()
  {
    Vector2 upwards = (Vector2) (this.hook.position - this.startChainPoint.position);
    this.chainCollider.transform.rotation = Quaternion.LookRotation(Vector3.back, (Vector3) upwards);
    Vector2 vector2 = Vector2.up * 0.5f * upwards.magnitude;
    if ((UnityEngine.Object) this.owner != (UnityEngine.Object) null)
    {
      Vector3 normalized = (this.hook.position - this.owner.transform.position).normalized;
      Vector3 from = new Vector3(normalized.y, -normalized.x);
      Vector3 to = this.owner.transform.position - this.startChainPoint.position;
      float num1 = Vector2.Angle((Vector2) from, (Vector2) to);
      float num2 = to.magnitude * Mathf.Cos(num1 * ((float) Math.PI / 180f));
      vector2.x = num2;
    }
    this.chainCollider.offset = vector2;
    this.chainCollider.size = new Vector2(this.chainCollisionWidth, upwards.magnitude);
    this.chainCollider.transform.localPosition = Vector3.zero;
  }

  public void HandleLineRendererPoints()
  {
    this.chain.SetPosition(0, this.startChainPoint.position);
    Vector2 normalized = (Vector2) (this.hook.position - this.startChainPoint.position).normalized;
    for (int index = 1; index < this.chain.positionCount - 1; ++index)
    {
      float t = (float) index / (float) this.chain.positionCount;
      Vector3 zero = Vector3.zero;
      Vector3 position;
      if (!this.hiding && (double) Mathf.Abs(this.ellipseMovement.AngleToMove) < 0.10000000149011612)
      {
        float num = Mathf.Clamp01((float) (1.0 - (double) this.currentUnscaledTime / ((double) this.ellipseMovement.Duration * (double) this.straightLineTime)));
        Vector3 vector3 = new Vector3(-normalized.y, normalized.x) * num * Mathf.Sin(this.lineBreakSinLength * 3.14159274f * t) * (float) this.randomBreakMultiplier;
        position = this.startChainPoint.position + t * (this.hook.position - this.startChainPoint.position) + vector3;
      }
      else if (!this.hiding)
      {
        position = !this.useBezier ? Vector3.Lerp(this.startChainPoint.position, this.hook.position, t) : this.QuadraticBezierCurve(this.startChainPoint.position, this.GetEllipsePosition(this.bezierEllipseMovement), this.hook.position, t);
      }
      else
      {
        Vector3 vector3 = (this.hook.position - this.startChainPoint.position) * 0.5f;
        position = !this.useBezier ? Vector3.Lerp(this.startChainPoint.position, this.hook.position, t) : this.QuadraticBezierCurve(this.startChainPoint.position, this.startChainPoint.position + vector3, this.hook.position, t);
      }
      this.chain.SetPosition(index, position);
    }
    this.chain.SetPosition(this.chain.positionCount - 1, this.hook.position);
  }

  public Vector3 GetEllipsePosition(EllipseMovement movement)
  {
    float num1 = Mathf.Clamp(this.currentTime, 0.0f, movement.Duration);
    float t = this.moveProgress.Evaluate(num1 / movement.Duration);
    float num2 = movement.RadiusMultiplierOverTime.Evaluate(num1 / movement.Duration);
    float f = Mathf.Lerp(movement.StartAngle, movement.StartAngle + movement.AngleToMove, t) * ((float) Math.PI / 180f);
    float num3 = movement.Radius1 * Mathf.Cos(f) * num2;
    float num4 = movement.Radius2 * Mathf.Sin(f) * num2;
    return this.transform.position + (movement.Right.normalized * num3 + movement.Up.normalized * num4) + movement.CenterOffset;
  }

  public Vector3 QuadraticBezierCurve(Vector3 point0, Vector3 point1, Vector3 point2, float t)
  {
    t = Mathf.Clamp01(t);
    return Mathf.Pow(1f - t, 2f) * point0 + (float) (2.0 * (1.0 - (double) t)) * t * point1 + Mathf.Pow(t, 2f) * point2;
  }

  public void OnHookCollision(Collider2D collider)
  {
    if ((UnityEngine.Object) collider == (UnityEngine.Object) null)
      return;
    Health component = collider.GetComponent<Health>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null || (double) this.currentTime < (double) this.colliderEnabledDelay || (double) this.currentTime > (double) this.colliderDisabledDelay)
      return;
    this.DealDamage(this.damage, component);
  }

  public void OnChainCollision(Collider2D collider)
  {
    if ((UnityEngine.Object) collider == (UnityEngine.Object) null)
      return;
    Health component = collider.GetComponent<Health>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null || (double) this.currentTime < (double) this.colliderEnabledDelay || (double) this.currentTime > (double) this.colliderDisabledDelay || !this.chainDamage)
      return;
    this.DealDamage(this.damage * this.chainDamageMultiplier, component);
  }

  public void DealDamage(float damage, Health health)
  {
    if (this.damagedEnemies.Contains(health) || !((UnityEngine.Object) health != (UnityEngine.Object) null) || !health.enabled || health.team == this.ownerHealth.team && !health.IsCharmedEnemy && (!((UnityEngine.Object) health != (UnityEngine.Object) this.ownerHealth) || health.team != this.ownerHealth.team || !this.canHurtFriendlyUnits) || health.untouchable || health.invincible || !((UnityEngine.Object) health.state == (UnityEngine.Object) null) && health.state.CURRENT_STATE == StateMachine.State.Dodging)
      return;
    float num = this.ownerHealth.team != health.team || !this.canHurtFriendlyUnits ? 1f : this.team2DamageMult;
    health.DealDamage(damage * num, this.owner, this.transform.position, AttackType: this.attackType, AttackFlags: this.attackFlags);
    this.damagedEnemies.Add(health);
    Action<Health> onDamageCollision = this.onDamageCollision;
    if (onDamageCollision == null)
      return;
    onDamageCollision(health);
  }

  public void Health_OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    this.hiding = false;
    this.gameObject.SetActive(false);
    this.SetCollidersActive(true);
  }

  public GameObject GetOwner() => this.owner;

  public void SetOwner(GameObject owner) => this.owner = owner;

  public Vector3 GetHookPosition() => this.hook.position;

  public void InvertMovmenet() => this.invertedMovement = !this.invertedMovement;

  public void SetHideTimeMultiplier(float multiplier)
  {
    if ((double) multiplier < 0.0)
      multiplier = 0.0f;
    this.hideTimeMultiplier = multiplier;
  }

  [CompilerGenerated]
  public void \u003CHide\u003Eb__70_0()
  {
    this.hiding = false;
    this.gameObject.SetActive(false);
  }

  [CompilerGenerated]
  public void \u003CHide\u003Eb__70_1()
  {
    this.hiding = false;
    this.gameObject.SetActive(false);
  }

  [CompilerGenerated]
  public void \u003CHideThenDestroy\u003Eb__73_0()
  {
    this.hiding = false;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }
}
