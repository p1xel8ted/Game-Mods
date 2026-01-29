// Decompiled with JetBrains decompiler
// Type: LightningBossOrb
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class LightningBossOrb : MonoBehaviour
{
  public Health health;
  public GameObject beam;
  public float beamWidth;
  public float beamWidthMax = 0.5f;
  [HideInInspector]
  public GameObject parent;
  [HideInInspector]
  public bool dead;
  public IndicatorFlash spriteFlash;
  [SerializeField]
  public float pingPongSpeed;
  [SerializeField]
  public float pingPongOffset;
  public float pulseSpeed = 6f;
  public float pulseScaleMultiplier = 1.1f;
  public Vector3 minPulseScale = Vector3.one;
  public Vector3 maxPulseScale;
  public float pulseScaleTimer;
  public string BounceSFX = "event:/dlc/dungeon05/enemy/miniboss_lightning/attack_orb_bounce";
  public Health parentHealth;
  public ColliderEvents damageColliderEvents;
  public LightningBossOrbSpinner spinner;
  public Vector3 startingPosition;
  public float lerpTimer;
  public float previousValue;
  public bool isForward = true;
  public float adjustedTime;
  public Vector3 targetPosition;
  public float totalCycleDuration;
  public float halfCycleDuration;
  [HideInInspector]
  public bool beamActive;
  public Health EnemyHealth;

  public float PingPongSpeed => this.pingPongSpeed;

  public float PingPongOffset => this.pingPongOffset;

  public void Awake()
  {
    this.spinner = this.GetComponentInParent<LightningBossOrbSpinner>();
    this.startingPosition = this.transform.localPosition;
    this.targetPosition = this.startingPosition.normalized;
    this.pingPongSpeed = Mathf.Max(0.0f, this.pingPongSpeed);
    if ((double) this.pingPongSpeed > 0.0)
    {
      this.totalCycleDuration = 2f / this.pingPongSpeed;
      this.halfCycleDuration = 1f / this.pingPongSpeed;
      this.adjustedTime = (double) this.pingPongOffset > 0.0 ? this.pingPongOffset / this.pingPongSpeed : 0.0f;
      this.lerpTimer = this.adjustedTime % this.halfCycleDuration;
      this.isForward = (double) this.adjustedTime % (double) this.totalCycleDuration < (double) this.halfCycleDuration;
    }
    else
    {
      this.totalCycleDuration = float.PositiveInfinity;
      this.halfCycleDuration = float.PositiveInfinity;
      this.lerpTimer = 0.0f;
      this.isForward = true;
    }
  }

  public void Start()
  {
    this.health.OnHit += new Health.HitAction(this.OrbHit);
    this.beamWidth = this.beamWidthMax * 2f;
    if (!((Object) this.damageColliderEvents != (Object) null))
      return;
    this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    this.damageColliderEvents.SetActive(true);
  }

  public void OrbHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    this.beamWidth = 0.0f;
    if ((double) this.health.HP > 0.0)
      return;
    Object.Destroy((Object) this.beam);
    this.dead = true;
  }

  public void Update()
  {
    if ((Object) this.beam != (Object) null && (Object) this.parent != (Object) null && this.beamActive)
      this.UpdateBeam(this.parent.transform.position, this.transform.position);
    if (!this.spinner.SpinActive || (double) this.spinner.currentSpeed <= 0.0)
      return;
    this.lerpTimer += Time.deltaTime * this.spinner.parentSpine.timeScale;
    if ((double) this.lerpTimer >= (double) this.halfCycleDuration)
    {
      this.lerpTimer = 0.0f;
      this.isForward = !this.isForward;
      if (!this.isForward)
        AudioManager.Instance.PlayOneShot(this.BounceSFX);
    }
    float t = this.lerpTimer / this.halfCycleDuration;
    this.transform.localPosition = this.isForward ? Vector3.Lerp(this.startingPosition, this.targetPosition, t) : Vector3.Lerp(this.targetPosition, this.startingPosition, t);
  }

  public void OnDestroy()
  {
    if (!((Object) this.damageColliderEvents != (Object) null))
      return;
    this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    this.damageColliderEvents.SetActive(false);
  }

  public void UpdateBeam(Vector3 startPos, Vector3 endPos)
  {
    this.beam.gameObject.SetActive(true);
    this.beam.transform.position = (startPos + endPos) / 2f;
    Vector3 toDirection = endPos - startPos;
    this.beam.transform.localScale = new Vector3(toDirection.magnitude, this.beamWidth, 1f);
    this.beam.transform.rotation = Quaternion.FromToRotation(Vector3.up, toDirection) * Quaternion.Euler(0.0f, 0.0f, 90f);
    this.beamWidth += (float) (((double) this.beamWidthMax - (double) this.beamWidth) / 30.0);
  }

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    this.EnemyHealth = collider.GetComponent<Health>();
    if (!((Object) this.EnemyHealth != (Object) null))
      return;
    if (this.EnemyHealth.isPlayer)
    {
      if ((Object) this.spinner != (Object) null)
      {
        this.spinner.currentSpeed = 0.0f;
        this.spinner.currentSpeedDelay = 1.5f;
      }
      DOVirtual.DelayedCall(this.spinner.currentSpeedDelay * 2f, (TweenCallback) (() =>
      {
        this.beamActive = true;
        if (!((Object) this.beam != (Object) null))
          return;
        this.beam.SetActive(true);
      }));
      this.beamActive = false;
      this.EnemyHealth.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f));
      if (!((Object) this.beam != (Object) null))
        return;
      this.beam.SetActive(false);
    }
    else if (this.EnemyHealth.team != this.health.team)
    {
      this.EnemyHealth.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f));
    }
    else
    {
      if (this.health.team != Health.Team.PlayerTeam || this.health.isPlayerAlly || this.EnemyHealth.isPlayer)
        return;
      this.EnemyHealth.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f));
    }
  }

  [CompilerGenerated]
  public void \u003COnDamageTriggerEnter\u003Eb__38_0()
  {
    this.beamActive = true;
    if (!((Object) this.beam != (Object) null))
      return;
    this.beam.SetActive(true);
  }
}
