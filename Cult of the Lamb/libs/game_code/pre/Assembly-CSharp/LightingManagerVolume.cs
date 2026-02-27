// Decompiled with JetBrains decompiler
// Type: LightingManagerVolume
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteAlways]
[RequireComponent(typeof (BoxCollider2D))]
public class LightingManagerVolume : BaseMonoBehaviour
{
  public bool isGlobal;
  private bool lastGlobalState;
  public bool TransitionOnEnableDisable;
  public float transitionDurationMultiplierAdjustment = 0.2f;
  public BiomeLightingSettings LightingSettings;
  private BiomeLightingSettings targetLightSettings;
  public OverrideLightingProperties overrideLightingProperties;
  private bool isLeaderEncounter = true;
  public bool _isActualLeaderEncounter;
  public bool inTrigger;

  public void Start()
  {
    this.gameObject.layer = 21;
    if ((bool) (Object) this.LightingSettings)
      this.LightingSettings.overrideLightingProperties = this.overrideLightingProperties;
    if (this.isGlobal && (Object) LightingManager.Instance != (Object) null)
    {
      LightingManager.Instance.lerpActive = (double) this.transitionDurationMultiplierAdjustment != 0.0;
      LightingManager.Instance.globalOverrideSettings = this.LightingSettings;
      LightingManager.Instance.inGlobalOverride = true;
      LightingManager.Instance.transitionDurationMultiplier = this.transitionDurationMultiplierAdjustment;
      LightingManager.Instance.UpdateLighting(false);
    }
    this.lastGlobalState = this.isGlobal;
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
    if ((Object) LightingManager.Instance == (Object) null)
      return;
    if (this.isGlobal)
      LightingManager.Instance.inGlobalOverride = false;
    LightingManager.Instance.lerpActive = (double) this.transitionDurationMultiplierAdjustment != 0.0;
    LightingManager.Instance.inOverride = false;
    LightingManager.Instance.transitionDurationMultiplier = this.transitionDurationMultiplierAdjustment;
    LightingManager.Instance.UpdateLighting(false);
    if (!this._isActualLeaderEncounter)
      return;
    LightingManager.Instance.inLeaderEncounter = false;
  }

  public void OnDisable()
  {
    if ((Object) LightingManager.Instance == (Object) null)
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

  private void TransitionIn()
  {
    if ((Object) LightingManager.Instance == (Object) null)
      return;
    LightingManager.Instance.lerpActive = (double) this.transitionDurationMultiplierAdjustment != 0.0;
    LightingManager.Instance.inOverride = true;
    this.LightingSettings.overrideLightingProperties = this.overrideLightingProperties;
    LightingManager.Instance.overrideSettings = this.LightingSettings;
    LightingManager.Instance.transitionDurationMultiplier = this.transitionDurationMultiplierAdjustment;
    LightingManager.Instance.UpdateLighting(true);
  }

  private void TransitionOut()
  {
    LightingManager.Instance.lerpActive = (double) this.transitionDurationMultiplierAdjustment != 0.0;
    LightingManager.Instance.inOverride = false;
    LightingManager.Instance.transitionDurationMultiplier = this.transitionDurationMultiplierAdjustment;
    LightingManager.Instance.UpdateLighting(true);
  }

  public void Update()
  {
    if (this.TransitionOnEnableDisable || this.lastGlobalState == this.isGlobal || (Object) LightingManager.Instance == (Object) null)
      return;
    if (this.isGlobal)
    {
      LightingManager.Instance.lerpActive = (double) this.transitionDurationMultiplierAdjustment != 0.0;
      this.LightingSettings.overrideLightingProperties = this.overrideLightingProperties;
      LightingManager.Instance.globalOverrideSettings = this.LightingSettings;
      LightingManager.Instance.inGlobalOverride = true;
      LightingManager.Instance.transitionDurationMultiplier = this.transitionDurationMultiplierAdjustment;
      LightingManager.Instance.UpdateLighting(false);
    }
    else
    {
      LightingManager.Instance.lerpActive = (double) this.transitionDurationMultiplierAdjustment != 0.0;
      LightingManager.Instance.inGlobalOverride = false;
      LightingManager.Instance.transitionDurationMultiplier = this.transitionDurationMultiplierAdjustment;
      LightingManager.Instance.UpdateLighting(false);
    }
    this.lastGlobalState = this.isGlobal;
  }

  private void OnTriggerStay2D(Collider2D other)
  {
    if (this.TransitionOnEnableDisable || this.isGlobal || !(other.gameObject.tag == "Player") || this.inTrigger || !this.gameObject.activeInHierarchy)
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
    LightingManager.Instance.UpdateLighting(true);
    this.inTrigger = true;
  }

  private void OnTriggerExit2D(Collider2D other)
  {
    if (this.TransitionOnEnableDisable || this.isGlobal || !(other.gameObject.tag == "Player") || !this.inTrigger)
      return;
    LightingManager.Instance.inOverride = false;
    LightingManager.Instance.lerpActive = false;
    LightingManager.Instance.transitionDurationMultiplier = this.transitionDurationMultiplierAdjustment;
    if (this._isActualLeaderEncounter)
      LightingManager.Instance.inLeaderEncounter = false;
    LightingManager.Instance.UpdateLighting(true);
    this.inTrigger = false;
  }
}
