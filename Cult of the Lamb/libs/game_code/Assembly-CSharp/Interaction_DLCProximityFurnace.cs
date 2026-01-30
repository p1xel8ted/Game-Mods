// Decompiled with JetBrains decompiler
// Type: Interaction_DLCProximityFurnace
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using UnityEngine;

#nullable disable
public class Interaction_DLCProximityFurnace : MonoBehaviour
{
  public const int RANGE = 15;
  [SerializeField]
  public GameObject lit;
  [SerializeField]
  public SpriteRenderer rangeSprite;
  [SerializeField]
  public ParticleSystem radiusParticleSystem;
  [SerializeField]
  public ParticleSystem radiusParticleSystemIntro;
  public Vector3 _updatePos;
  public float distanceRadius = 1f;
  public bool distanceChanged;
  public string proximityHeaterTurnOnSFX = "event:/dlc/building/furnace/heater_turn_on";
  public string proximityHeaterTurnOffSFX = "event:/dlc/building/furnace/heater_turn_off";
  public string proximityHeaterIdleLoopSFX = "event:/dlc/building/furnace/heater_fire_burning_loop";
  public EventInstance idleLoopInstance;
  public bool wasLit;

  public bool Lit
  {
    get
    {
      return SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter && (Object) Interaction_DLCFurnace.Instance != (Object) null && Interaction_DLCFurnace.Instance.Lit;
    }
  }

  public void DebugTurnOnFurnace() => this.OnFurnaceLit();

  public void DebugTurnOffFurnace() => this.OnFurnaceTurnOff();

  public void Start()
  {
    Interaction_DLCFurnace.OnFurnaceLit += new Interaction_DLCFurnace.FurnaceEvent(this.OnFurnaceLit);
    Interaction_DLCFurnace.OnFurnaceTurnOff += new Interaction_DLCFurnace.FurnaceEvent(this.OnFurnaceTurnOff);
    this.InitializeParticleSystem();
  }

  public void OnEnable() => this.UpdateLitStatus(true);

  public void OnDisable() => AudioManager.Instance.StopLoop(this.idleLoopInstance);

  public void Update()
  {
    this.UpdateRangeVisuals();
    this.UpdateLitStatus(false);
  }

  public void UpdateRangeVisuals()
  {
    if (!DataManager.Instance.OnboardedSnowedUnder)
      return;
    if (!GameManager.overridePlayerPosition && (Object) PlayerFarming.Instance != (Object) null)
    {
      this._updatePos = PlayerFarming.Instance.transform.position;
      this.distanceRadius = 2f;
    }
    else
      this._updatePos = PlacementRegion.Instance.PlacementPosition;
    if ((double) Vector3.Distance(this._updatePos, this.transform.position) < (double) this.distanceRadius)
    {
      this.rangeSprite.gameObject.SetActive(true);
      this.rangeSprite.DOKill();
      this.rangeSprite.DOColor(StaticColors.OffWhiteColor, 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      this.distanceChanged = true;
    }
    else
    {
      if (!this.distanceChanged)
        return;
      this.rangeSprite.DOKill();
      this.rangeSprite.DOColor(new Color(1f, 1f, 1f, 0.0f), 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      this.distanceChanged = false;
    }
  }

  public void UpdateLitStatus(bool force)
  {
    bool lit = this.Lit;
    if (!force && this.wasLit == this.Lit)
      return;
    this.lit.gameObject.SetActive(lit);
    if (!this.wasLit & lit)
      this.OnFurnaceLit();
    else if (this.wasLit && !lit)
      this.OnFurnaceTurnOff();
    this.wasLit = this.Lit;
  }

  public void InitializeParticleSystem()
  {
    if (!DataManager.Instance.OnboardedSnowedUnder)
    {
      this.HideVisuals();
    }
    else
    {
      if ((Object) this.radiusParticleSystem != (Object) null)
      {
        this.radiusParticleSystem.main.startSize = (ParticleSystem.MinMaxCurve) 15f;
        this.radiusParticleSystem.emission.enabled = this.Lit;
      }
      if (!((Object) this.radiusParticleSystemIntro != (Object) null))
        return;
      ParticleSystem.MainModule main = this.radiusParticleSystemIntro.main;
      this.radiusParticleSystemIntro.emission.enabled = this.Lit;
    }
  }

  public void HideVisuals()
  {
    this.rangeSprite?.gameObject.SetActive(false);
    this.radiusParticleSystem?.gameObject.SetActive(false);
    this.radiusParticleSystemIntro?.gameObject.SetActive(false);
  }

  public void OnDestroy()
  {
    Interaction_DLCFurnace.OnFurnaceLit -= new Interaction_DLCFurnace.FurnaceEvent(this.OnFurnaceLit);
    Interaction_DLCFurnace.OnFurnaceTurnOff -= new Interaction_DLCFurnace.FurnaceEvent(this.OnFurnaceTurnOff);
    AudioManager.Instance.StopLoop(this.idleLoopInstance);
    if ((Object) this.radiusParticleSystem != (Object) null)
      this.radiusParticleSystem.emission.enabled = false;
    if (!((Object) this.radiusParticleSystemIntro != (Object) null))
      return;
    this.radiusParticleSystemIntro.emission.enabled = false;
  }

  public void OnFurnaceLit()
  {
    AudioManager.Instance.PlayOneShot(this.proximityHeaterTurnOnSFX, this.gameObject);
    AudioManager.Instance.StopLoop(this.idleLoopInstance, STOP_MODE.IMMEDIATE);
    this.idleLoopInstance = AudioManager.Instance.CreateLoop(this.proximityHeaterIdleLoopSFX, this.gameObject, true);
    if ((Object) this.radiusParticleSystem != (Object) null)
      this.radiusParticleSystem.emission.enabled = true;
    if (!((Object) this.radiusParticleSystemIntro != (Object) null))
      return;
    this.radiusParticleSystemIntro.emission.enabled = true;
  }

  public void OnFurnaceTurnOff()
  {
    AudioManager.Instance.PlayOneShot(this.proximityHeaterTurnOffSFX, this.gameObject);
    AudioManager.Instance.StopLoop(this.idleLoopInstance);
    if ((Object) this.radiusParticleSystem != (Object) null)
      this.radiusParticleSystem.emission.enabled = false;
    if (!((Object) this.radiusParticleSystemIntro != (Object) null))
      return;
    this.radiusParticleSystemIntro.emission.enabled = false;
  }
}
