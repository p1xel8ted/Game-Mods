// Decompiled with JetBrains decompiler
// Type: TrapLavaTimed
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using FMODUnity;
using MMBiomeGeneration;
using Spine.Unity;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class TrapLavaTimed : TrapLava
{
  [SerializeField]
  public ColliderEvents damageColliderEvents;
  [Header("Global Timer Settings")]
  [SerializeField]
  public float activeDuration = 10f;
  [SerializeField]
  public float inactiveDuration = 5f;
  [SerializeField]
  public float cycleOffset;
  [Header("Fire Blast Settings")]
  [SerializeField]
  public GameObject fireBlastGameObject;
  [SerializeField]
  public ParticleSystem smokeParticleSystem;
  [SerializeField]
  public MeshRenderer fauxFluidRenderer;
  [SerializeField]
  public ParticleSystem fireParticleSystem;
  [SerializeField]
  public float activationScaleDuration = 0.5f;
  [SerializeField]
  public float deactivationScaleDuration = 0.5f;
  [SerializeField]
  public float flameScale = 0.66f;
  [SerializeField]
  public bool deactivateAfter;
  [SerializeField]
  public float DamageOnTouch = 1f;
  [Header("Grate Settings")]
  [SerializeField]
  public GameObject grateGameObject;
  [SerializeField]
  public float grateShakeBeforeBlast = 0.5f;
  [SerializeField]
  public float grateShakeDuration = 0.3f;
  [SerializeField]
  public Vector3 grateShakeStrength = new Vector3(0.05f, 0.05f, 0.0f);
  [SerializeField]
  public int grateShakeVibrato = 5;
  [EventRef]
  public string IdleLoopSFX = "event:/dlc/dungeon06/trap/fire_grate/idle_loop";
  [EventRef]
  public string ActivateSFX = "event:/dlc/dungeon06/trap/fire_grate/active_start";
  public EventInstance idleLoopInstance;
  public EventInstance activateInstanceSFX;
  public bool isActive = true;
  public bool isGrateShaking;
  public bool grateShaked;
  public bool roomComplete;
  public float lavaTimer;

  public override void OnEnable()
  {
    RoomLockController.OnDoorsClosed += new RoomLockController.RoomEvent(this.OnAllDoorsClosed);
    RoomLockController.OnRoomCleared += new RoomLockController.RoomEvent(this.DeactiveOnRoomCleared);
    if (!this.roomComplete)
    {
      this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.DealDamage);
      if (!this.isActive && !string.IsNullOrEmpty(this.IdleLoopSFX) && !AudioManager.Instance.IsEventInstancePlaying(this.idleLoopInstance))
        this.idleLoopInstance = AudioManager.Instance.CreateLoop(this.IdleLoopSFX, this.gameObject, true);
    }
    base.OnEnable();
  }

  public new void OnDisable()
  {
    AudioManager.Instance.StopLoop(this.idleLoopInstance);
    RoomLockController.OnRoomCleared -= new RoomLockController.RoomEvent(this.DeactiveOnRoomCleared);
    RoomLockController.OnDoorsClosed -= new RoomLockController.RoomEvent(this.OnAllDoorsClosed);
    BiomeGenerator.OnBiomeLeftRoom -= new BiomeGenerator.BiomeAction(((TrapLava) this).ClearOnRoomChange);
    this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.DealDamage);
  }

  public void OnDestroy() => this.CleanupEventInstances();

  public void CleanupEventInstances()
  {
    AudioManager.Instance.StopLoop(this.idleLoopInstance, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.activateInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
  }

  public void DeactiveOnRoomCleared()
  {
    this.roomComplete = true;
    this.DeactivateLava();
    this.CleanupEventInstances();
    this.DeactivateVisuals();
  }

  public void OnAllDoorsClosed() => this.ActivateVisuals();

  public new void Update()
  {
    if (!PlayerRelic.TimeFrozen && !this.roomComplete)
    {
      this.lavaTimer += Time.deltaTime;
      float num1 = this.activeDuration + this.inactiveDuration;
      float num2 = (this.lavaTimer - this.cycleOffset + num1) % num1;
      if ((double) num2 < (double) this.activeDuration && !this.isActive && !this.grateShaked)
      {
        this.grateShaked = true;
        this.ShakeGrate();
      }
      else if ((double) num2 >= (double) this.activeDuration && this.isActive)
      {
        this.grateShaked = false;
        this.DeactivateLava();
      }
    }
    if (this.roomComplete || Health.team2.Count > 0)
      return;
    this.DeactiveOnRoomCleared();
  }

  public void ActivateLava()
  {
    if (this.roomComplete)
      return;
    this.isActive = true;
    this.isGrateShaking = false;
    AudioManager.Instance.StopLoop(this.idleLoopInstance);
    if ((bool) (Object) this.fireBlastGameObject)
    {
      GameObject gameObject = PlayerFarming.GetClosestPlayer(this.transform.position)?.gameObject;
      if ((Object) gameObject != (Object) null && (double) Vector3.Distance(this.transform.position, gameObject.transform.position) <= 2.0)
      {
        CameraManager.shakeCamera(2f);
        MMVibrate.Haptic(MMVibrate.HapticTypes.SoftImpact, coroutineSupport: (MonoBehaviour) GameManager.GetInstance());
      }
      this.damageColliderEvents.gameObject.SetActive(true);
      this.fireBlastGameObject.transform.DOKill();
      this.fireBlastGameObject.transform.localScale = Vector3.zero;
      this.fireBlastGameObject.transform.DOScale(Vector3.one * this.flameScale, this.activationScaleDuration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutExpo).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.fireBlastGameObject.transform.DOShakeScale(0.3f, new Vector3(0.2f, 0.2f, 0.0f))));
    }
    if (!((Object) this.areaBurn != (Object) null))
      return;
    this.areaBurn.EnableDamage(this.areaBurn.tickDamage, this.areaBurn.tickIntervalPerEnemy, this.areaBurn.burnDuration, Health.AttackFlags.Trap);
  }

  public void DeactivateLava()
  {
    if (!this.isActive)
      return;
    this.isActive = false;
    if ((bool) (Object) this.fireBlastGameObject)
    {
      foreach (Component componentsInChild in this.fireBlastGameObject.GetComponentsInChildren<SkeletonAnimation>(true))
      {
        MeshRenderer component = componentsInChild.GetComponent<MeshRenderer>();
        if ((Object) component != (Object) null)
          component.enabled = true;
      }
      this.damageColliderEvents.gameObject.SetActive(false);
      this.fireBlastGameObject.transform.DOKill();
      this.fireBlastGameObject.transform.DOScale(Vector3.zero, this.deactivationScaleDuration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InExpo).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
      {
        if (!this.deactivateAfter)
          return;
        this.gameObject.SetActive(false);
        this.lavaTimer = 0.0f;
        this.fireBlastGameObject.transform.DOKill();
        this.fireBlastGameObject.transform.localScale = Vector3.one * this.flameScale;
        this.particleSystem.Clear();
      }));
    }
    this.areaBurn?.DisableDamage();
    if (string.IsNullOrEmpty(this.IdleLoopSFX) || AudioManager.Instance.IsEventInstancePlaying(this.idleLoopInstance))
      return;
    this.idleLoopInstance = AudioManager.Instance.CreateLoop(this.IdleLoopSFX, this.gameObject, true);
  }

  public void ActivateVisuals()
  {
    if ((bool) (Object) this.fauxFluidRenderer)
      this.fauxFluidRenderer.gameObject.SetActive(true);
    if ((bool) (Object) this.smokeParticleSystem)
    {
      this.smokeParticleSystem.gameObject.SetActive(true);
      this.smokeParticleSystem.Play();
    }
    if ((bool) (Object) this.particleSystem)
      this.particleSystem.Play();
    if (!(bool) (Object) this.fireParticleSystem)
      return;
    this.fireParticleSystem.gameObject.SetActive(true);
    this.fireParticleSystem.Play();
  }

  public void DeactivateVisuals()
  {
    if ((bool) (Object) this.fauxFluidRenderer)
      this.fauxFluidRenderer.gameObject.SetActive(false);
    if ((bool) (Object) this.smokeParticleSystem)
    {
      this.smokeParticleSystem.Stop();
      this.smokeParticleSystem.gameObject.SetActive(false);
    }
    if ((bool) (Object) this.particleSystem)
      this.particleSystem.Stop();
    if (!(bool) (Object) this.fireParticleSystem)
      return;
    this.fireParticleSystem.Stop();
    this.fireParticleSystem.gameObject.SetActive(false);
  }

  public void ShakeGrate()
  {
    this.isGrateShaking = true;
    if (!string.IsNullOrEmpty(this.ActivateSFX))
      this.activateInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(this.ActivateSFX, this.transform);
    if (!((Object) this.grateGameObject != (Object) null))
      return;
    this.grateGameObject.transform.DOShakePosition(this.grateShakeDuration, this.grateShakeStrength, this.grateShakeVibrato).SetEase<Tweener>(Ease.OutQuad).OnComplete<Tweener>((TweenCallback) (() => this.ActivateLava()));
  }

  public void DealDamage(Collider2D collider)
  {
    Health component = collider.gameObject.GetComponent<Health>();
    if (!((Object) component != (Object) null) || component.team != Health.Team.PlayerTeam || !((Object) component.state != (Object) null) || component.state.CURRENT_STATE == StateMachine.State.Dodging)
      return;
    Health.AttackFlags attackFlags = Health.AttackFlags.Burn | Health.AttackFlags.Trap;
    if ((double) this.DamageOnTouch > 0.0)
      component.DealDamage(this.DamageOnTouch, this.gameObject, this.transform.position, AttackFlags: attackFlags);
    else
      component.AddBurn(this.gameObject, attackFlags: attackFlags);
  }

  public override void ClearOnRoomChange()
  {
    if (!this.deactivateAfter)
      return;
    this.DisableLavaImmediate();
  }

  [CompilerGenerated]
  public void \u003CActivateLava\u003Eb__34_0()
  {
    this.fireBlastGameObject.transform.DOShakeScale(0.3f, new Vector3(0.2f, 0.2f, 0.0f));
  }

  [CompilerGenerated]
  public void \u003CDeactivateLava\u003Eb__35_0()
  {
    if (!this.deactivateAfter)
      return;
    this.gameObject.SetActive(false);
    this.lavaTimer = 0.0f;
    this.fireBlastGameObject.transform.DOKill();
    this.fireBlastGameObject.transform.localScale = Vector3.one * this.flameScale;
    this.particleSystem.Clear();
  }

  [CompilerGenerated]
  public void \u003CShakeGrate\u003Eb__38_0() => this.ActivateLava();
}
