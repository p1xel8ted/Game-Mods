// Decompiled with JetBrains decompiler
// Type: LightingManagerVolume
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
[ExecuteAlways]
[RequireComponent(typeof (BoxCollider2D))]
public class LightingManagerVolume : BaseMonoBehaviour
{
  public bool isGlobal;
  public bool lastGlobalState;
  public bool isPlayerToCheck;
  public PlayerFarming playerToCheck;
  public bool TransitionOnEnableDisable;
  public float transitionDurationMultiplierAdjustment = 0.2f;
  public bool isExitTransitionOn;
  public float exitTransitionDurationMultiplierAdjustment = 0.2f;
  public BiomeLightingSettings LightingSettings;
  public BiomeLightingSettings targetLightSettings;
  public OverrideLightingProperties overrideLightingProperties;
  public bool isLeaderEncounter = true;
  public bool _isActualLeaderEncounter;
  public bool AllowInterupt = true;
  public bool inTrigger;
  [SerializeField]
  public bool _ignoreLightingAccessibilitySetting;

  public void Start()
  {
    this.gameObject.layer = 21;
    if ((bool) (UnityEngine.Object) this.LightingSettings)
      this.LightingSettings.overrideLightingProperties = this.overrideLightingProperties;
    if (this.isGlobal && (UnityEngine.Object) LightingManager.Instance != (UnityEngine.Object) null)
    {
      LightingManager.Instance.lerpActive = (double) this.transitionDurationMultiplierAdjustment != 0.0;
      LightingManager.Instance.globalOverrideSettings = this.LightingSettings;
      LightingManager.Instance.inGlobalOverride = true;
      LightingManager.Instance.transitionDurationMultiplier = this.transitionDurationMultiplierAdjustment;
      LightingManager.Instance.UpdateLighting(false, this._ignoreLightingAccessibilitySetting);
      LightingManager.Instance.transitionDurationMultiplier = 1f;
    }
    this.lastGlobalState = this.isGlobal;
    if (SettingsManager.Settings == null || !SettingsManager.Settings.Accessibility.RemoveLightingEffects || !((UnityEngine.Object) DeathCatRoomManager.Instance != (UnityEngine.Object) null))
      return;
    this.StartCoroutine((IEnumerator) this.ReenableCollider());
  }

  public void OnEnable()
  {
    this.inTrigger = false;
    if (!this.TransitionOnEnableDisable)
      return;
    this.TransitionIn();
  }

  public void OnDestroy()
  {
    if ((UnityEngine.Object) LightingManager.Instance == (UnityEngine.Object) null)
      return;
    if (this.isGlobal)
      LightingManager.Instance.inGlobalOverride = false;
    LightingManager.Instance.lerpActive = (double) this.transitionDurationMultiplierAdjustment != 0.0;
    LightingManager.Instance.inOverride = false;
    LightingManager.Instance.transitionDurationMultiplier = this.transitionDurationMultiplierAdjustment;
    LightingManager.Instance.UpdateLighting(false, this._ignoreLightingAccessibilitySetting);
    LightingManager.Instance.transitionDurationMultiplier = 1f;
    if (!this._isActualLeaderEncounter)
      return;
    LightingManager.Instance.inLeaderEncounter = false;
  }

  public void OnDisable()
  {
    if ((UnityEngine.Object) LightingManager.Instance == (UnityEngine.Object) null)
      return;
    if (this.isGlobal)
      LightingManager.Instance.inGlobalOverride = false;
    if (this.TransitionOnEnableDisable)
    {
      this.TransitionOut();
      this.inTrigger = false;
    }
    if (!this._isActualLeaderEncounter)
      return;
    LightingManager.Instance.inLeaderEncounter = false;
  }

  public void TransitionIn()
  {
    if ((UnityEngine.Object) LightingManager.Instance == (UnityEngine.Object) null)
      return;
    this.StartCoroutine((IEnumerator) this.WaitAframeTurnOn());
  }

  public IEnumerator WaitAframeTurnOn()
  {
    yield return (object) new WaitForEndOfFrame();
    LightingManager.Instance.lerpActive = (double) this.transitionDurationMultiplierAdjustment != 0.0;
    LightingManager.Instance.inOverride = true;
    this.LightingSettings.overrideLightingProperties = this.overrideLightingProperties;
    LightingManager.Instance.overrideSettings = this.LightingSettings;
    LightingManager.Instance.transitionDurationMultiplier = this.transitionDurationMultiplierAdjustment;
    LightingManager.Instance.UpdateLighting(true, this._ignoreLightingAccessibilitySetting);
    LightingManager.Instance.transitionDurationMultiplier = 1f;
  }

  public void TransitionOut()
  {
    LightingManager.Instance.lerpActive = (this.isExitTransitionOn ? (double) this.exitTransitionDurationMultiplierAdjustment : (double) this.transitionDurationMultiplierAdjustment) != 0.0;
    LightingManager.Instance.inOverride = false;
    LightingManager.Instance.transitionDurationMultiplier = this.transitionDurationMultiplierAdjustment;
    LightingManager.Instance.UpdateLighting(this.AllowInterupt, this._ignoreLightingAccessibilitySetting);
    LightingManager.Instance.transitionDurationMultiplier = 1f;
  }

  public void Update()
  {
    if (this.TransitionOnEnableDisable || this.lastGlobalState == this.isGlobal || (UnityEngine.Object) LightingManager.Instance == (UnityEngine.Object) null)
      return;
    if (this.isGlobal)
    {
      LightingManager.Instance.lerpActive = (double) this.transitionDurationMultiplierAdjustment != 0.0;
      this.LightingSettings.overrideLightingProperties = this.overrideLightingProperties;
      LightingManager.Instance.globalOverrideSettings = this.LightingSettings;
      LightingManager.Instance.inGlobalOverride = true;
      LightingManager.Instance.transitionDurationMultiplier = this.transitionDurationMultiplierAdjustment;
      LightingManager.Instance.UpdateLighting(false, this._ignoreLightingAccessibilitySetting);
      LightingManager.Instance.transitionDurationMultiplier = 1f;
    }
    else
    {
      LightingManager.Instance.lerpActive = (double) this.transitionDurationMultiplierAdjustment != 0.0;
      LightingManager.Instance.inGlobalOverride = false;
      LightingManager.Instance.transitionDurationMultiplier = this.transitionDurationMultiplierAdjustment;
      LightingManager.Instance.UpdateLighting(false, this._ignoreLightingAccessibilitySetting);
      LightingManager.Instance.transitionDurationMultiplier = 1f;
    }
    this.lastGlobalState = this.isGlobal;
  }

  public void OnTriggerStay2D(Collider2D other)
  {
    if (this.TransitionOnEnableDisable || this.isGlobal || !other.gameObject.CompareTag("Player") || this.inTrigger || !this.gameObject.activeInHierarchy || this.isPlayerToCheck && (!((UnityEngine.Object) this.playerToCheck != (UnityEngine.Object) null) || !((UnityEngine.Object) this.playerToCheck.gameObject == (UnityEngine.Object) other.gameObject)))
      return;
    LightingManager.Instance.inOverride = true;
    this.LightingSettings.overrideLightingProperties = this.overrideLightingProperties;
    LightingManager.Instance.overrideSettings = this.LightingSettings;
    LightingManager.Instance.transitionDurationMultiplier = this.transitionDurationMultiplierAdjustment;
    LightingManager.Instance.lerpActive = (double) this.transitionDurationMultiplierAdjustment != 0.0;
    if (this.isLeaderEncounter)
    {
      LightingManager.Instance.isTODTransition = true;
      if (this._isActualLeaderEncounter)
        LightingManager.Instance.inLeaderEncounter = true;
    }
    LightingManager.Instance.UpdateLighting(true, this._ignoreLightingAccessibilitySetting);
    LightingManager.Instance.transitionDurationMultiplier = 1f;
    this.inTrigger = true;
  }

  public void OnTriggerExit2D(Collider2D other)
  {
    if (this.TransitionOnEnableDisable || this.isGlobal)
      return;
    if ((UnityEngine.Object) LightingManager.Instance == (UnityEngine.Object) null)
    {
      Debug.Log((object) "Lighting manager does not exist or has been destroyed");
    }
    else
    {
      if (!other.gameObject.CompareTag("Player") || !this.inTrigger || this.isPlayerToCheck && (!((UnityEngine.Object) this.playerToCheck != (UnityEngine.Object) null) || !((UnityEngine.Object) this.playerToCheck.gameObject == (UnityEngine.Object) other.gameObject)))
        return;
      float num = this.isExitTransitionOn ? this.exitTransitionDurationMultiplierAdjustment : this.transitionDurationMultiplierAdjustment;
      LightingManager.Instance.inOverride = false;
      LightingManager.Instance.lerpActive = false;
      LightingManager.Instance.transitionDurationMultiplier = num;
      if (this._isActualLeaderEncounter)
        LightingManager.Instance.inLeaderEncounter = false;
      LightingManager.Instance.UpdateLighting(true, this._ignoreLightingAccessibilitySetting);
      LightingManager.Instance.transitionDurationMultiplier = 1f;
      this.inTrigger = false;
    }
  }

  public IEnumerator ReenableCollider()
  {
    LightingManagerVolume lightingManagerVolume = this;
    BoxCollider2D collider = lightingManagerVolume.GetComponent<BoxCollider2D>();
    bool colliderInitialState = collider.enabled;
    collider.enabled = true;
    yield return (object) new WaitUntil((Func<bool>) new Func<bool>(lightingManagerVolume.\u003CReenableCollider\u003Eb__26_0));
    collider.enabled = false;
    yield return (object) new WaitUntil((Func<bool>) new Func<bool>(lightingManagerVolume.\u003CReenableCollider\u003Eb__26_1));
    collider.enabled = colliderInitialState;
  }

  [CompilerGenerated]
  public bool \u003CReenableCollider\u003Eb__26_0() => this.inTrigger;

  [CompilerGenerated]
  public bool \u003CReenableCollider\u003Eb__26_1() => !this.inTrigger;
}
