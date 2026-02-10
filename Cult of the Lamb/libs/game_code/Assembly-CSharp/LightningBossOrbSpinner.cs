// Decompiled with JetBrains decompiler
// Type: LightningBossOrbSpinner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class LightningBossOrbSpinner : MonoBehaviour
{
  public List<LightningBossOrb> Orbs;
  public float baseSpeed = 45f;
  [HideInInspector]
  public float currentSpeed;
  [HideInInspector]
  public float currentSpeedDelay;
  public float speedIncreasePerOrbDead = 20f;
  public float speedDir = 1f;
  public bool PingPong;
  public float timeUntilSelfDestruct = 10f;
  [HideInInspector]
  public int orbDeadCount;
  [HideInInspector]
  public GameObject parent;
  public SkeletonAnimation parentSpine;
  public float orbDistanceMultiplier = 1f;
  public float speed;
  public bool spinActive;
  public float speedChangeOnHit = -1.1f;
  public float forceOrbHP;
  public float pulseSpeed = 6f;
  public float pulseScaleMultiplier = 1.1f;
  public Vector3 minPulseScale;
  public Vector3 maxPulseScale;
  public float pulseScaleTimer;
  public float pulseStartTimeLeft = 2f;
  public string OrbDeathSFX = "event:/dlc/dungeon05/enemy/miniboss_lightning/attack_orb_destroy";
  public string OrbBounceSFX = "event:/dlc/dungeon05/enemy/miniboss_lightning/attack_orb_bounce";
  public Health parentHealth;
  public EnemyLightningMiniboss parentBoss;

  public bool SpinActive => this.spinActive;

  public void Start()
  {
    this.minPulseScale = this.Orbs[0].transform.localScale;
    this.maxPulseScale = this.minPulseScale * this.pulseScaleMultiplier;
    CameraManager.instance.ShakeCameraForDuration(0.5f, 1f, 1f);
    this.parentHealth = this.parent.GetComponent<Health>();
    this.parentBoss = this.parent.GetComponent<EnemyLightningMiniboss>();
    if ((bool) (Object) this.parentBoss)
      this.parentBoss.UpdateDeadOrbParam(0);
    for (int index = 0; index < this.Orbs.Count; ++index)
    {
      LightningBossOrb orb = this.Orbs[index];
      orb.parent = this.gameObject;
      orb.parentHealth = this.parentHealth;
      orb.health.OnDie += new Health.DieAction(this.OrbDeath);
      orb.health.OnHit += new Health.HitAction(this.OrbHit);
      if ((double) this.forceOrbHP != 0.0)
        orb.health.totalHP = orb.health.HP = this.forceOrbHP;
      orb.transform.localScale = Vector3.zero;
      Vector3 localPosition = orb.transform.localPosition;
      orb.transform.localPosition = Vector3.zero;
      Vector3 endValue = localPosition * this.orbDistanceMultiplier;
      Vector3 normalized = endValue.normalized;
      if (this.PingPong)
      {
        float t = Mathf.PingPong(orb.PingPongOffset, 1f);
        endValue = Vector3.Lerp(localPosition * this.orbDistanceMultiplier, normalized, t);
      }
      orb.beamActive = true;
      orb.health.invincible = true;
      orb.transform.DOLocalMove(endValue, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutQuad).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(0.25f).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
      {
        this.spinActive = true;
        orb.health.invincible = false;
      }));
      orb.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutQuad).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(0.25f * (float) index);
    }
  }

  public void OrbHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    this.speedDir *= this.speedChangeOnHit;
    CameraManager.instance.ShakeCameraForDuration(0.5f, 1f, 0.5f);
    EnemyLightningMiniboss component = this.parent.GetComponent<EnemyLightningMiniboss>();
    if (!((Object) component != (Object) null))
      return;
    component.flinchFromOrbHit();
  }

  public void FixedUpdate()
  {
    if (this.spinActive)
    {
      if ((double) this.currentSpeedDelay < 0.0)
        this.currentSpeed += (float) (((double) this.baseSpeed - (double) this.currentSpeed) / 30.0);
      else
        this.currentSpeedDelay -= Time.deltaTime * this.parentSpine.timeScale;
      this.transform.Rotate(0.0f, 0.0f, (this.currentSpeed + this.speedIncreasePerOrbDead * (float) this.orbDeadCount) * this.speedDir * Time.fixedDeltaTime * this.parentSpine.timeScale);
    }
    this.timeUntilSelfDestruct -= Time.deltaTime * this.parentSpine.timeScale;
    if ((double) this.timeUntilSelfDestruct <= (double) this.pulseStartTimeLeft && (double) this.timeUntilSelfDestruct > 0.0)
      this.PulseRemainingOrbs();
    if ((double) this.timeUntilSelfDestruct >= 0.0)
      return;
    for (int index = 0; index < this.Orbs.Count; ++index)
    {
      LightningBossOrb orb = this.Orbs[index];
      if (!orb.dead)
      {
        LightningRingExplosion.CreateExplosion(orb.transform.position, orb.health.team, orb.health, 1f, -1f, -1f, false, 1f, (Health.AttackFlags) 0, false, 4f, (UnitObject[]) null);
        Explosion.CreateExplosion(orb.transform.position + new Vector3(0.0f, 0.0f, -0.5f), orb.health.team, orb.health, 1f, 0.0f);
        Object.Destroy((Object) orb.beam);
        orb.dead = true;
      }
    }
    Object.Destroy((Object) this.gameObject);
  }

  public void PulseRemainingOrbs()
  {
    this.pulseScaleTimer += Time.deltaTime * this.pulseSpeed * this.parentSpine.timeScale;
    float t = Mathf.PingPong(this.pulseScaleTimer, 1f);
    for (int index = 0; index < this.Orbs.Count; ++index)
    {
      if (!this.Orbs[index].dead)
      {
        if (!this.Orbs[index].spriteFlash.enabled)
          this.Orbs[index].spriteFlash.enabled = true;
        this.Orbs[index].transform.localScale = Vector3.Lerp(this.minPulseScale, this.maxPulseScale, t);
      }
    }
  }

  public void OrbDeath(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    for (int index = 0; index < this.Orbs.Count; ++index)
    {
      if ((Object) this.Orbs[index] != (Object) null && (Object) this.Orbs[index].health == (Object) Victim)
      {
        Object.Destroy((Object) this.Orbs[index].beam);
        this.Orbs[index].dead = true;
        BiomeConstants.Instance.EmitSmokeExplosionVFX(this.Orbs[index].transform.position);
        CameraManager.instance.ShakeCameraForDuration(0.5f, 1f, 0.1f);
        MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact);
        if (!string.IsNullOrEmpty(this.OrbDeathSFX))
          AudioManager.Instance.PlayOneShot(this.OrbDeathSFX);
        Health component = this.parent.GetComponent<Health>();
        if ((bool) (Object) component)
        {
          component.invincible = false;
          component.DealDamage(0.0f, this.gameObject, this.Orbs[index].transform.position, true);
          component.invincible = true;
        }
        this.speedDir *= this.speedChangeOnHit;
        CameraManager.instance.ShakeCameraForDuration(0.5f, 1f, 0.5f);
        Object.Destroy((Object) this.Orbs[index].gameObject);
      }
    }
    Debug.Log((object) ("Orb has been destroyed " + this.orbDeadCount.ToString()));
    ++this.orbDeadCount;
    this.parentBoss.UpdateDeadOrbParam(this.orbDeadCount);
    if (this.orbDeadCount != this.Orbs.Count)
      return;
    Object.Destroy((Object) this.gameObject, 0.25f);
  }

  public void OnDestroy()
  {
    for (int index = 0; index < this.Orbs.Count; ++index)
    {
      if ((Object) this.Orbs[index] != (Object) null)
      {
        this.Orbs[index].health.OnDie -= new Health.DieAction(this.OrbDeath);
        this.Orbs[index].health.OnHit -= new Health.HitAction(this.OrbHit);
        Object.Destroy((Object) this.Orbs[index].gameObject);
      }
    }
    this.Orbs.Clear();
  }
}
