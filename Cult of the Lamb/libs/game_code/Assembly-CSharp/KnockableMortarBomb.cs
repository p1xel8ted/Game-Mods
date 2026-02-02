// Decompiled with JetBrains decompiler
// Type: KnockableMortarBomb
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using FMOD.Studio;
using FMODUnity;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

#nullable disable
public class KnockableMortarBomb : BaseMonoBehaviour, ISpellOwning, ITimeFreezable
{
  public static List<KnockableMortarBomb> KnockableMortarBombs = new List<KnockableMortarBomb>();
  [SerializeField]
  public SimpleSpineFlash simpleSpineFlash;
  public GameObject BombVisual;
  public SpriteRenderer Target;
  public SpriteRenderer TargetWarning;
  public CircleCollider2D circleCollider2D;
  public GameObject BombShadow;
  public bool ExplodeOnTouch = true;
  public float ExplodeOnTouchDelay = -1f;
  public ParticleSystem SmokeParticles;
  [FormerlySerializedAs("enemyCollider")]
  public ColliderEvents colliderEvents;
  public DOTweenAnimation rotationAnimation;
  public float moveDuration = 1f;
  public float arcHeight = 2f;
  public AnimationCurve arcCurve;
  [FormerlySerializedAs("explosionRadius")]
  [SerializeField]
  public float regularExplosionRadius = 0.5f;
  [SerializeField]
  public float afterHitExplosionRadius = 0.5f;
  [SerializeField]
  public SkeletonAnimation Spine;
  public Health Origin;
  public bool isHit;
  public bool isExploding;
  public GameObject hitInitiator;
  public bool exploded;
  public bool canBeHit;
  public float enemyExplodeDistance = 0.5f;
  public float flashTime = 0.15f;
  public float flashSpeed = 3f;
  public float startExplodeTimer = -1f;
  [SerializeField]
  public float contactExplosionDelay = 0.15f;
  public float animSpeedLerpTarget = 1f;
  [SerializeField]
  public float onHitPlayerDetectionGracePeriod = 0.2f;
  [SerializeField]
  public float explodeTimer;
  [SerializeField]
  public bool addDelayOnKnock = true;
  [SerializeField]
  public float knockExplodeDelay;
  [SerializeField]
  public bool hittableWhileExploding;
  [SerializeField]
  public bool explodeWhenMeleed = true;
  [Range(0.0f, 1f)]
  public float hittableAtTrajectoryProgress = 0.8f;
  public bool triggeredByOwner = true;
  public bool canBeTriggeredByOthersOfSameTeam;
  public bool canHurtOwner = true;
  [SerializeField]
  public Health.Team defaultBombTeam;
  [SerializeField]
  public Health.Team onHitBombTeam;
  public Health owner;
  [SerializeField]
  public float knockbackMultiplier;
  [SerializeField]
  public float lockOnDistance;
  [SerializeField]
  public float lockOnAngle;
  [SerializeField]
  public LayerMask triggerLayer;
  [SerializeField]
  public LayerMask lockOnMask;
  public Coroutine knockRoutine;
  public bool detectPlayer = true;
  public Rigidbody2D rb;
  public bool DisableForces;
  public Health health;
  public bool destroyOnFinish = true;
  [EventRef]
  public string BombLandSFX = string.Empty;
  [EventRef]
  public string BombWindUpSFX = string.Empty;
  [EventRef]
  public string BombExplodeSFX = string.Empty;
  [EventRef]
  public string BombKnockBackSFX = string.Empty;
  public EventInstance windUpInstanceSFX;
  public bool isKnocked;
  public static bool isRunning;
  public Collider2D[] colliders = new Collider2D[20];
  public bool DelayingDestroy;
  public float DelayTriggerEnterTime;

  public event System.Action OnExplode;

  public event System.Action OnDestroyProjectile;

  public void OnEnable()
  {
    KnockableMortarBomb.KnockableMortarBombs.Add(this);
    this.StartCoroutine((IEnumerator) this.ScaleCircle());
    this.BombVisual.SetActive(false);
    this.BombShadow.SetActive(false);
    this.health.team = this.defaultBombTeam;
    Health.AddToTeam(this.health, this.health.team);
  }

  public void OnDisable()
  {
    if (!string.IsNullOrEmpty(this.BombWindUpSFX))
      AudioManager.Instance.StopOneShotInstanceEarly(this.windUpInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
    System.Action destroyProjectile = this.OnDestroyProjectile;
    if (destroyProjectile != null)
      destroyProjectile();
    KnockableMortarBomb.KnockableMortarBombs.Remove(this);
    Health.RemoveFromTeam(this.health, this.health.team);
    if (!this.destroyOnFinish)
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public void Play(Vector3 Position, float moveDuration, Health owner = null, bool playDefaultSFX = true)
  {
    this.moveDuration = moveDuration;
    this.owner = owner;
    this.StartCoroutine((IEnumerator) this.MoveRock(Position));
    this.StartCoroutine((IEnumerator) this.FlashCircle());
    if (!playDefaultSFX)
      return;
    AudioManager.Instance.PlayOneShot("event:/enemy/fly_spawn", this.gameObject);
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/frog_large/attack", this.gameObject);
  }

  public void Play(
    Vector3 Position,
    float moveDuration,
    float explodeTimer,
    Health owner = null,
    bool playDefaultSFX = true)
  {
    this.moveDuration = moveDuration;
    this.owner = owner;
    this.explodeTimer = explodeTimer;
    this.StartCoroutine((IEnumerator) this.MoveRock(Position));
    this.StartCoroutine((IEnumerator) this.FlashCircle());
    if (!playDefaultSFX)
      return;
    AudioManager.Instance.PlayOneShot("event:/enemy/fly_spawn", this.gameObject);
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/frog_large/attack", this.gameObject);
  }

  public IEnumerator ScaleCircle()
  {
    float Scale = 0.0f;
    while ((double) (Scale += Time.deltaTime * 8f) <= 1.0)
    {
      this.Target.transform.localScale = Vector3.one * this.circleCollider2D.radius * Scale;
      this.TargetWarning.transform.localScale = Vector3.one * this.circleCollider2D.radius * Scale;
      yield return (object) null;
    }
  }

  public void Awake()
  {
    this.rotationAnimation = this.BombVisual.GetComponent<DOTweenAnimation>();
    this.health.OnHit += new Health.HitAction(this.OnHit);
    if (!(bool) (UnityEngine.Object) this.colliderEvents)
      return;
    this.colliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnTriggerEnterEvent);
    this.colliderEvents.OnTriggerStayEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerStay);
    this.DisableTriggerCollider();
  }

  public void Update()
  {
    this.HandleRotationAnimation();
    this.Target.transform.Rotate(new Vector3(0.0f, 0.0f, 150f) * Time.deltaTime);
  }

  public void HandleRotationAnimation()
  {
    if ((UnityEngine.Object) this.rotationAnimation == (UnityEngine.Object) null)
      return;
    if (PlayerRelic.TimeFrozen)
    {
      this.rotationAnimation.DOPause();
    }
    else
    {
      if (this.rotationAnimation.tween.IsPlaying())
        return;
      this.rotationAnimation.DOPlay();
    }
  }

  public bool IsHit => this.isHit;

  public IEnumerator FlashCircle()
  {
    while ((double) Vector2.Distance((Vector2) this.BombVisual.transform.localPosition, (Vector2) Vector3.zero) >= 6.0)
      yield return (object) null;
    Color white = new Color(1f, 1f, 1f, 1f);
    Color color = white;
    float flashTickTimer = 0.0f;
    while ((double) Vector2.Distance((Vector2) this.BombVisual.transform.localPosition, (Vector2) Vector3.zero) < 6.0)
    {
      if ((double) flashTickTimer >= 0.11999999731779099 && (double) Time.timeScale == 1.0 && BiomeConstants.Instance.IsFlashLightsActive)
      {
        this.Target.material.SetColor("_Color", color = color == white ? Color.red : white);
        this.TargetWarning.material.SetColor("_Color", color);
        flashTickTimer = 0.0f;
      }
      flashTickTimer += Time.deltaTime;
      yield return (object) null;
    }
  }

  public IEnumerator PlayerDetectionGracePeriod()
  {
    this.detectPlayer = false;
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * this.Spine.timeScale) < (double) this.onHitPlayerDetectionGracePeriod)
      yield return (object) null;
    this.detectPlayer = true;
  }

  public void KnockTowardsEnemy(Vector3 AttackPosition, Health.AttackTypes attackType)
  {
    this.isKnocked = true;
    this.StartCoroutine((IEnumerator) this.PlayerDetectionGracePeriod());
    this.StartCoroutine((IEnumerator) this.NotExplodingHittable(AttackPosition, attackType));
  }

  public IEnumerator NotExplodingHittable(Vector3 AttackerPosition, Health.AttackTypes AttackType)
  {
    KnockableMortarBomb knockableMortarBomb = this;
    if (KnockableMortarBomb.isRunning)
      knockableMortarBomb.StopCoroutine((IEnumerator) knockableMortarBomb.NotExplodingHittable(AttackerPosition, AttackType));
    KnockableMortarBomb.isRunning = true;
    if (!knockableMortarBomb.isExploding || knockableMortarBomb.hittableWhileExploding)
    {
      Collider2D[] colliders = Physics2D.OverlapCircleAll((Vector2) knockableMortarBomb.transform.position, knockableMortarBomb.lockOnDistance, (int) knockableMortarBomb.lockOnMask);
      yield return (object) null;
      Vector3 vector3 = knockableMortarBomb.transform.position - AttackerPosition;
      Vector3 normalized1 = vector3.normalized;
      Collider2D collider2D1 = (Collider2D) null;
      float num = 0.0f;
      knockableMortarBomb.simpleSpineFlash.FlashFillRed();
      if (colliders.Length != 0)
      {
        foreach (Collider2D collider2D2 in colliders)
        {
          if (!((UnityEngine.Object) collider2D2 == (UnityEngine.Object) null) && !((UnityEngine.Object) collider2D2.gameObject == (UnityEngine.Object) null) && !((UnityEngine.Object) collider2D2.GetComponent<UnitObject>() == (UnityEngine.Object) null))
          {
            vector3 = knockableMortarBomb.transform.position - collider2D2.transform.position;
            Vector3 normalized2 = vector3.normalized;
            float f = Mathf.Abs(Vector3.Angle(normalized1, normalized2) - 180f);
            bool flag = (double) f < (double) knockableMortarBomb.lockOnAngle;
            if ((UnityEngine.Object) collider2D1 == (UnityEngine.Object) null || (double) Mathf.Abs(f) < (double) num)
            {
              Health component = collider2D2.GetComponent<Health>();
              if (flag && (bool) (UnityEngine.Object) component && component.team == Health.Team.Team2 && (UnityEngine.Object) collider2D2.gameObject != (UnityEngine.Object) knockableMortarBomb.gameObject)
              {
                collider2D1 = collider2D2;
                num = f;
              }
            }
          }
        }
      }
      Collider2D collider2D3 = collider2D1;
      if ((UnityEngine.Object) collider2D3 != (UnityEngine.Object) null && (UnityEngine.Object) collider2D3.gameObject != (UnityEngine.Object) knockableMortarBomb.gameObject && (double) knockableMortarBomb.MagnitudeFindDistanceBetween(collider2D3.transform.position, knockableMortarBomb.transform.position) < (double) knockableMortarBomb.lockOnDistance * (double) knockableMortarBomb.lockOnDistance)
      {
        float angle = Utils.GetAngle(knockableMortarBomb.transform.position, collider2D3.transform.position) * ((float) Math.PI / 180f);
        knockableMortarBomb.DoKnockBack(angle, knockableMortarBomb.knockbackMultiplier, 1f, false);
      }
      else
        knockableMortarBomb.DoKnockBack(AttackerPosition, knockableMortarBomb.knockbackMultiplier, 1f, false);
      if (!knockableMortarBomb.isExploding)
        knockableMortarBomb.explodeTimer = (knockableMortarBomb.addDelayOnKnock ? knockableMortarBomb.knockExplodeDelay : knockableMortarBomb.explodeTimer) / knockableMortarBomb.Spine.timeScale;
      if (knockableMortarBomb.explodeWhenMeleed || !knockableMortarBomb.explodeWhenMeleed && AttackType != Health.AttackTypes.Melee)
        knockableMortarBomb.StartCoroutine((IEnumerator) knockableMortarBomb.DelayedExplodeCharge());
      else if (knockableMortarBomb.gameObject.activeInHierarchy && !knockableMortarBomb.DelayingDestroy)
      {
        knockableMortarBomb.StartCoroutine((IEnumerator) knockableMortarBomb.DelayedDestroy());
        knockableMortarBomb.Spine.gameObject.SetActive(false);
      }
      colliders = (Collider2D[]) null;
    }
    KnockableMortarBomb.isRunning = false;
  }

  public IEnumerator ApplyForceRoutine(float angle, float KnockbackModifier, float Duration)
  {
    KnockableMortarBomb knockableMortarBomb = this;
    knockableMortarBomb.DisableForces = true;
    Vector3 vector3 = (Vector3) new Vector2(25f * Mathf.Cos(angle), 25f * Mathf.Sin(angle));
    knockableMortarBomb.rb.velocity = (Vector2) (vector3 * KnockbackModifier);
    if (!string.IsNullOrEmpty(knockableMortarBomb.BombKnockBackSFX))
      AudioManager.Instance.PlayOneShot(knockableMortarBomb.BombKnockBackSFX, knockableMortarBomb.gameObject);
    yield return (object) new WaitForSeconds(Duration);
    knockableMortarBomb.DisableForces = false;
    knockableMortarBomb.knockRoutine = (Coroutine) null;
  }

  public float MagnitudeFindDistanceBetween(Vector3 a, Vector3 b)
  {
    double num1 = (double) a.x - (double) b.x;
    float num2 = a.y - b.y;
    float num3 = a.z - b.z;
    return (float) (num1 * num1 + (double) num2 * (double) num2 + (double) num3 * (double) num3);
  }

  public IEnumerator MoveRock(Vector3 startPos)
  {
    KnockableMortarBomb knockableMortarBomb = this;
    knockableMortarBomb.BombVisual.SetActive(true);
    knockableMortarBomb.BombShadow.SetActive(true);
    knockableMortarBomb.BombVisual.transform.position = startPos;
    Vector2 targetPos = (Vector2) knockableMortarBomb.transform.position;
    float t = 0.0f;
    while ((double) t < (double) knockableMortarBomb.moveDuration)
    {
      if (!PlayerRelic.TimeFrozen)
      {
        t += Time.deltaTime;
        knockableMortarBomb.BombVisual.transform.position = Vector3.Lerp(startPos, (Vector3) targetPos, t / knockableMortarBomb.moveDuration);
        knockableMortarBomb.BombShadow.transform.position = Vector3.Lerp(startPos, (Vector3) targetPos, t / knockableMortarBomb.moveDuration);
        knockableMortarBomb.BombShadow.transform.localScale = Vector3.one * knockableMortarBomb.circleCollider2D.radius * (float) (1.5 - (double) Mathf.Clamp01(knockableMortarBomb.arcCurve.Evaluate(t / knockableMortarBomb.moveDuration)) * 0.5);
        knockableMortarBomb.BombVisual.transform.position += new Vector3(0.0f, 0.0f, -knockableMortarBomb.arcCurve.Evaluate(t / knockableMortarBomb.moveDuration) * knockableMortarBomb.arcHeight);
        if (!knockableMortarBomb.canBeHit && (double) t / (double) knockableMortarBomb.moveDuration > (double) knockableMortarBomb.hittableAtTrajectoryProgress)
          knockableMortarBomb.canBeHit = true;
      }
      yield return (object) null;
    }
    if (!string.IsNullOrEmpty(knockableMortarBomb.BombLandSFX))
      AudioManager.Instance.PlayOneShot(knockableMortarBomb.BombLandSFX, knockableMortarBomb.gameObject);
    knockableMortarBomb.canBeHit = true;
    knockableMortarBomb.Target.transform.localScale = Vector3.zero;
    knockableMortarBomb.TargetWarning.transform.localScale = Vector3.zero;
    if (!string.IsNullOrEmpty(knockableMortarBomb.BombWindUpSFX))
      knockableMortarBomb.windUpInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(knockableMortarBomb.BombWindUpSFX, knockableMortarBomb.gameObject.transform);
    knockableMortarBomb.StartCoroutine((IEnumerator) knockableMortarBomb.EnableTriggerCollider(knockableMortarBomb.contactExplosionDelay));
    knockableMortarBomb.StartCoroutine((IEnumerator) knockableMortarBomb.DelayedExplodeCharge());
  }

  public virtual void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    if (!this.canBeHit || this.isKnocked)
      return;
    Health component = Attacker.GetComponent<Health>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.team != Health.Team.PlayerTeam)
      return;
    this.StartCoroutine((IEnumerator) this.EnableTriggerCollider(0.0f));
    this.Target.transform.localScale = Vector3.zero;
    this.TargetWarning.transform.localScale = Vector3.zero;
    this.BombVisual.transform.position = new Vector3(this.BombVisual.transform.position.x, this.BombVisual.transform.position.y, 0.0f);
    this.isHit = true;
    Health.AddToTeam(this.health, Health.Team.PlayerTeam);
    this.StopAllCoroutines();
    this.KnockTowardsEnemy((UnityEngine.Object) Attacker != (UnityEngine.Object) null ? Attacker.transform.position : (Vector3) UnityEngine.Random.insideUnitCircle, AttackType);
    if (string.IsNullOrEmpty(this.BombWindUpSFX))
      return;
    AudioManager.Instance.StopOneShotInstanceEarly(this.windUpInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
  }

  public IEnumerator DelayedExplodeCharge()
  {
    KnockableMortarBomb knockableMortarBomb = this;
    if (!knockableMortarBomb.isExploding)
    {
      TrackEntry trackEntry = knockableMortarBomb.Spine.state.GetCurrent(0);
      float spineTimescaleStart = trackEntry.TimeScale;
      float spineTimescaleTarget = spineTimescaleStart * knockableMortarBomb.animSpeedLerpTarget;
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * knockableMortarBomb.Spine.timeScale) < (double) knockableMortarBomb.explodeTimer)
      {
        trackEntry.TimeScale = Mathf.Lerp(spineTimescaleStart, spineTimescaleTarget, time / knockableMortarBomb.explodeTimer);
        knockableMortarBomb.simpleSpineFlash?.FlashWhite(time / knockableMortarBomb.explodeTimer);
        yield return (object) null;
      }
      knockableMortarBomb.isExploding = true;
      bool playSFX = true;
      if (!string.IsNullOrEmpty(knockableMortarBomb.BombExplodeSFX))
      {
        AudioManager.Instance.PlayOneShot(knockableMortarBomb.BombExplodeSFX, knockableMortarBomb.gameObject);
        playSFX = false;
      }
      MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact);
      Explosion.CreateExplosion(knockableMortarBomb.transform.position, knockableMortarBomb.health.team, knockableMortarBomb.canHurtOwner ? knockableMortarBomb.health : knockableMortarBomb.owner, knockableMortarBomb.isHit ? knockableMortarBomb.afterHitExplosionRadius : knockableMortarBomb.regularExplosionRadius, 1f, playSFX: playSFX);
      System.Action onExplode = knockableMortarBomb.OnExplode;
      if (onExplode != null)
        onExplode();
      System.Action destroyProjectile = knockableMortarBomb.OnDestroyProjectile;
      if (destroyProjectile != null)
        destroyProjectile();
      if ((UnityEngine.Object) knockableMortarBomb.SmokeParticles != (UnityEngine.Object) null)
      {
        knockableMortarBomb.SmokeParticles.transform.parent = knockableMortarBomb.transform.parent;
        knockableMortarBomb.SmokeParticles.Stop();
      }
      if (knockableMortarBomb.destroyOnFinish)
        UnityEngine.Object.Destroy((UnityEngine.Object) knockableMortarBomb.gameObject);
      else if ((UnityEngine.Object) knockableMortarBomb != (UnityEngine.Object) null)
        knockableMortarBomb.gameObject.Recycle();
    }
  }

  public IEnumerator DelayedDestroy()
  {
    KnockableMortarBomb knockableMortarBomb = this;
    knockableMortarBomb.DelayingDestroy = true;
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * knockableMortarBomb.Spine.timeScale) < 1.0)
      yield return (object) null;
    System.Action destroyProjectile = knockableMortarBomb.OnDestroyProjectile;
    if (destroyProjectile != null)
      destroyProjectile();
    if ((bool) (UnityEngine.Object) knockableMortarBomb.gameObject)
      UnityEngine.Object.Destroy((UnityEngine.Object) knockableMortarBomb.gameObject);
  }

  public virtual void DoKnockBack(
    Vector3 AttackerPosition,
    float KnockbackModifier,
    float Duration,
    bool appendForce = true)
  {
    if ((UnityEngine.Object) this.rb == (UnityEngine.Object) null)
      return;
    if (this.knockRoutine != null)
      this.StopCoroutine(this.knockRoutine);
    if (!appendForce)
      this.rb.velocity = (Vector2) Vector3.zero;
    this.knockRoutine = this.StartCoroutine((IEnumerator) this.ApplyForceRoutine(Utils.GetAngle(AttackerPosition, this.transform.position) * ((float) Math.PI / 180f), KnockbackModifier, Duration));
  }

  public IEnumerator EnableTriggerCollider(float delay)
  {
    if ((bool) (UnityEngine.Object) this.colliderEvents)
    {
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * this.Spine.timeScale) < (double) delay)
        yield return (object) null;
      this.colliderEvents.SetActive(true);
    }
  }

  public void DisableTriggerCollider()
  {
    if (!(bool) (UnityEngine.Object) this.colliderEvents)
      return;
    this.colliderEvents.SetActive(false);
  }

  public void OnCollisionEnter2D(Collision2D collision)
  {
    if ((this.triggerLayer.value & 1 << collision.gameObject.layer) <= 0 || !this.isHit)
      return;
    this.StopAllCoroutines();
    this.explodeTimer = 0.0f;
    this.StartCoroutine((IEnumerator) this.DelayedExplodeCharge());
  }

  public void OnTriggerEnterEvent(Collider2D collision)
  {
    if (!this.ExplodeOnTouch)
      return;
    Health component = collision.GetComponent<Health>();
    if (this.IsNotValidTrigger(collision, component) || component.team == Health.Team.PlayerTeam && !this.detectPlayer || (UnityEngine.Object) component == (UnityEngine.Object) this.owner && !this.triggeredByOwner && !this.isKnocked || !this.canBeTriggeredByOthersOfSameTeam && this.TeamsMatch(component) && this.CurrentBombTeam != Health.Team.KillAll || this.onHitBombTeam != Health.Team.KillAll && component.team != this.CurrentBombTeam)
      return;
    this.StopAllCoroutines();
    this.explodeTimer = 0.0f;
    this.StartCoroutine((IEnumerator) this.DelayedExplodeCharge());
  }

  public void OnDamageTriggerStay(Collider2D collider)
  {
    if (this.ExplodeOnTouch || (double) this.ExplodeOnTouchDelay == -1.0)
      return;
    if ((double) this.DelayTriggerEnterTime == 0.0)
    {
      this.DelayTriggerEnterTime = Time.time + this.ExplodeOnTouchDelay;
    }
    else
    {
      if ((double) this.DelayTriggerEnterTime >= (double) Time.time)
        return;
      this.ExplodeOnTouch = true;
      this.OnTriggerEnterEvent(collider);
    }
  }

  public bool IsNotValidTrigger(Collider2D collision, Health enemyHealth)
  {
    return (UnityEngine.Object) enemyHealth == (UnityEngine.Object) null || (UnityEngine.Object) collision.gameObject == (UnityEngine.Object) this.gameObject || (UnityEngine.Object) collision.GetComponent<KnockableMortarBomb>() != (UnityEngine.Object) null;
  }

  public bool TeamsDoNotMatch(Health compareTo)
  {
    return compareTo.team != this.CurrentBombTeam && this.CurrentBombTeam != Health.Team.KillAll;
  }

  public bool TeamsMatch(Health compareTo) => compareTo.team == this.CurrentBombTeam;

  public Health.Team CurrentBombTeam => !this.isHit ? this.defaultBombTeam : this.onHitBombTeam;

  public virtual void DoKnockBack(
    float angle,
    float KnockbackModifier,
    float Duration,
    bool appendForce = true)
  {
    if (this.knockRoutine != null)
      this.StopCoroutine(this.knockRoutine);
    if (!appendForce)
      this.rb.velocity = (Vector2) Vector3.zero;
    this.knockRoutine = this.StartCoroutine((IEnumerator) this.ApplyForceRoutine(angle, KnockbackModifier, Duration));
  }

  public SkeletonAnimation SkeletonAnimation => this.Spine;

  public Health GetOrigin() => this.Origin;

  public void SetOwner(GameObject owner) => this.Origin = owner.GetComponent<Health>();

  public GameObject GetOwner() => this.Origin?.gameObject;
}
