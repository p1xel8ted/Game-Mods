// Decompiled with JetBrains decompiler
// Type: GrenadeBullet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Ara;
using Spine.Unity;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class GrenadeBullet : BaseMonoBehaviour, ISpellOwning
{
  public Transform t;
  public float GravSpeed = 0.2f;
  public ColliderEvents damageColliderEvents;
  public Health.Team Team = Health.Team.Team2;
  public SpriteRenderer ShadowSpriteRenderer;
  [SerializeField]
  public SpriteRenderer indicatorIcon;
  [SerializeField]
  public AraTrail trail;
  [SerializeField]
  public TrailRenderer lowQualityTrail;
  [SerializeField]
  public GameObject particleOverride;
  public System.Action OnDestroyed;
  public SkeletonAnimation parentSpine;
  [CompilerGenerated]
  public float \u003CDamage\u003Ek__BackingField = 1f;
  [CompilerGenerated]
  public float \u003CAngle\u003Ek__BackingField;
  [CompilerGenerated]
  public float \u003CSpeed\u003Ek__BackingField;
  [CompilerGenerated]
  public float \u003CGrav\u003Ek__BackingField;
  public string customImpactSFX;
  public float time;
  public float flashTickTimer;
  public Color indicatorColor = Color.white;
  public Vector3 NewPosition;
  public GameObject owner;
  public Vector3 dir;
  public Vector3 targetPosition;
  public bool isMoving;
  public bool isCleaningUp;
  public float timeToTravel;
  public float cleanupTimer;
  public Coroutine damageColliderRoutine;

  public float SpineTimeScale
  {
    get => !(bool) (UnityEngine.Object) this.parentSpine ? 1f : this.parentSpine.timeScale;
  }

  public float Damage
  {
    get => this.\u003CDamage\u003Ek__BackingField;
    set => this.\u003CDamage\u003Ek__BackingField = value;
  }

  public void OnEnable()
  {
    this.trail = this.indicatorIcon.transform.parent.Find("GrenadeBullet_Tail").GetComponent<AraTrail>();
    if ((bool) (UnityEngine.Object) this.trail)
    {
      if ((bool) (UnityEngine.Object) this.lowQualityTrail)
        this.lowQualityTrail.enabled = false;
      this.trail.enabled = true;
      this.trail.Clear();
      this.trail.emit = true;
    }
    this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    this.damageColliderEvents.SetActive(false);
  }

  public void OnDisable()
  {
    this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    this.damageColliderEvents.SetActive(false);
  }

  public float Angle
  {
    get => this.\u003CAngle\u003Ek__BackingField;
    set => this.\u003CAngle\u003Ek__BackingField = value;
  }

  public float Speed
  {
    get => this.\u003CSpeed\u003Ek__BackingField;
    set => this.\u003CSpeed\u003Ek__BackingField = value;
  }

  public float Grav
  {
    get => this.\u003CGrav\u003Ek__BackingField;
    set => this.\u003CGrav\u003Ek__BackingField = value;
  }

  public void Play(
    float StartingHeight,
    float Angle,
    float Speed,
    float Grav,
    Health.Team team = Health.Team.Team2,
    bool hideIndicator = false,
    string customImpactSFX = "",
    SkeletonAnimation parentSpine = null)
  {
    this.trail.Clear();
    this.isMoving = false;
    this.isCleaningUp = false;
    this.damageColliderEvents.SetActive(false);
    this.t.localPosition = Vector3.forward * StartingHeight;
    this.time = Grav;
    this.Angle = Angle * ((float) Math.PI / 180f);
    this.Speed = Speed;
    this.Grav = Grav * 0.0166666675f;
    this.Team = team;
    this.customImpactSFX = customImpactSFX;
    this.parentSpine = parentSpine;
    this.indicatorIcon.gameObject.SetActive(!hideIndicator);
    this.t.gameObject.SetActive(true);
    this.ShadowSpriteRenderer.enabled = true;
    this.StartMoving();
  }

  public void ResetPosition() => this.transform.position = Vector3.zero;

  public void Update()
  {
    if ((double) Time.timeScale == 0.0 || PlayerRelic.TimeFrozen)
      return;
    if (this.indicatorIcon.gameObject.activeSelf && (double) this.flashTickTimer >= 0.11999999731779099 && BiomeConstants.Instance.IsFlashLightsActive)
    {
      this.indicatorColor = this.indicatorColor == Color.white ? Color.red : Color.white;
      this.indicatorIcon.material.SetColor("_Color", this.indicatorColor);
      this.flashTickTimer = 0.0f;
    }
    this.flashTickTimer += Time.deltaTime * this.SpineTimeScale;
    if (!this.isCleaningUp)
      return;
    this.cleanupTimer -= Time.deltaTime * this.SpineTimeScale;
    if ((double) this.cleanupTimer > 0.0)
      return;
    this.gameObject.Recycle();
  }

  public void FixedUpdate()
  {
    if (!this.isMoving || PlayerRelic.TimeFrozen)
      return;
    this.NewPosition = new Vector3(this.Speed * Mathf.Cos(this.Angle), this.Speed * Mathf.Sin(this.Angle)) * Time.fixedDeltaTime * this.SpineTimeScale;
    this.transform.position += this.NewPosition;
    this.Grav += this.GravSpeed * Time.fixedDeltaTime * this.SpineTimeScale;
    this.t.localPosition += Vector3.forward * this.Grav;
    this.targetPosition.z = -0.1f;
    this.indicatorIcon.transform.position = this.targetPosition;
    if ((double) this.t.position.z <= 0.0)
      return;
    this.isMoving = false;
    this.DoCollision();
  }

  public void StartMoving()
  {
    this.dir = (Vector3) Utils.RadianToVector2(this.Angle);
    this.timeToTravel = 5.99999952f;
    this.timeToTravel += this.t.localPosition.z / 2f;
    this.timeToTravel -= 1f - Mathf.Sqrt(Mathf.Abs(this.t.localPosition.z));
    this.timeToTravel = Mathf.Abs(this.time) / this.timeToTravel;
    this.targetPosition = this.transform.position + this.dir * (Mathf.Abs(this.Speed) * this.timeToTravel);
    this.targetPosition.z = 0.0f;
    this.isMoving = true;
  }

  public void DoCollision()
  {
    this.indicatorIcon.gameObject.SetActive(false);
    this.t.gameObject.SetActive(false);
    this.ShadowSpriteRenderer.enabled = false;
    if (this.damageColliderRoutine != null)
      this.StopCoroutine(this.damageColliderRoutine);
    this.damageColliderRoutine = this.StartCoroutine(this.TurnOnDamageColliderForDuration(0.2f));
    GameObject gameObject = (UnityEngine.Object) this.particleOverride != (UnityEngine.Object) null ? this.particleOverride.Spawn() : BiomeConstants.Instance.GrenadeBulletImpact_A.Spawn();
    gameObject.transform.position = this.t.transform.position;
    gameObject.transform.rotation = Quaternion.identity;
    gameObject.transform.localScale = Vector3.one * 0.5f;
    CameraManager.shakeCamera(0.5f, false);
    AudioManager.Instance.PlayOneShot(!string.IsNullOrEmpty(this.customImpactSFX) ? this.customImpactSFX : "event:/enemy/spit_gross_projectile");
    System.Action onDestroyed = this.OnDestroyed;
    if (onDestroyed != null)
      onDestroyed();
    if ((bool) (UnityEngine.Object) this.trail)
    {
      double time = (double) this.trail.time;
      this.trail.emit = false;
    }
    this.isCleaningUp = true;
    this.cleanupTimer = 1f;
  }

  public IEnumerator TurnOnDamageColliderForDuration(float duration)
  {
    this.damageColliderEvents.SetActive(true);
    yield return (object) new WaitForSeconds(duration);
    this.damageColliderEvents.SetActive(false);
  }

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.Team)
      return;
    component.DealDamage(this.Damage, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }

  public GameObject GetOwner() => this.owner;

  public void SetOwner(GameObject owner) => this.owner = owner;
}
