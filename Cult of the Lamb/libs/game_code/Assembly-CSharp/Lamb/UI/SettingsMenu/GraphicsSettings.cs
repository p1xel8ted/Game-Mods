// Decompiled with JetBrains decompiler
// Type: Lamb.UI.SettingsMenu.GraphicsSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Steamworks;
using System;
using System.Collections.Generic;
using Unify;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI.SettingsMenu;

public class GraphicsSettings : UISubmenuBase
{
  [SerializeField]
  public UIMenuControlPrompts controlPrompts;
  [SerializeField]
  public MMScrollRect _scrollRect;
  [Header("General Settings")]
  [SerializeField]
  public MMHorizontalSelector _graphicsPresetSelector;
  [SerializeField]
  public MMDropdown _resolutionDropdown;
  [SerializeField]
  public MMSelectable_Dropdown _resolutionDropdownSepectable;
  [SerializeField]
  public MMHorizontalSelector _fullScreenModeSelector;
  [SerializeField]
  public MMSelectable_HorizontalSelector _targetFpsSelectable;
  [SerializeField]
  public MMHorizontalSelector _targetFpsSelector;
  [SerializeField]
  public MMToggle _vSyncSwitch;
  [SerializeField]
  public MMHorizontalSelector _lightingQuality;
  [SerializeField]
  public MMHorizontalSelector _environmentDetail;
  [SerializeField]
  public MMToggle _shadowsToggle;
  [SerializeField]
  public MMToggle _bloomSwitch;
  [SerializeField]
  public MMToggle _chromaticAberrationSwitch;
  [SerializeField]
  public MMToggle _vignetteSwitch;
  [SerializeField]
  public MMToggle _depthOfFieldToggle;
  [SerializeField]
  public MMToggle _antiAliasing;
  [Header("Disable on Consoles")]
  public List<GameObject> InactiveOnConsole;
  [Header("Disable on Steam Deck")]
  public List<GameObject> InactiveOnDeck;
  public Selectable ConsoleDefaultSelectable;
  public List<string> _lowHigh = new List<string>()
  {
    "UI/Settings/Graphics/QualitySettingsOption/Low",
    "UI/Settings/Graphics/QualitySettingsOption/High"
  };
  public List<string> _lowMediumHigh = new List<string>()
  {
    "UI/Settings/Graphics/QualitySettingsOption/Low",
    "UI/Settings/Graphics/QualitySettingsOption/Medium",
    "UI/Settings/Graphics/QualitySettingsOption/High"
  };
  public List<string> _lowMediumHighUltraCustom = new List<string>()
  {
    "UI/Settings/Graphics/QualitySettingsOption/Low",
    "UI/Settings/Graphics/QualitySettingsOption/Medium",
    "UI/Settings/Graphics/QualitySettingsOption/High",
    "UI/Settings/Graphics/QualitySettingsOption/Ultra",
    "UI/Settings/Graphics/QualitySettingsOption/Custom"
  };
  public List<string> _framerateSettings = new List<string>()
  {
    "30",
    "60",
    "<sprite name=\"icon_Infinity\">"
  };
  public List<string> _fullscreenModes = new List<string>()
  {
    "UI/Settings/Graphics/FullScreenMode/Windowed",
    "UI/Settings/Graphics/FullScreenMode/BorderlessWindowed",
    "UI/Settings/Graphics/FullScreenMode/FullScreen"
  };

  public override void Awake()
  {
    base.Awake();
    switch (UnifyManager.platform)
    {
      case UnifyManager.Platform.None:
      case UnifyManager.Platform.Standalone:
        if (SteamUtils.IsSteamRunningOnSteamDeck())
        {
          for (int index = 0; index < this.InactiveOnDeck.Count; ++index)
            this.InactiveOnDeck[index].SetActive(false);
          this._defaultSelectable = (Selectable) this._resolutionDropdownSepectable;
        }
        this._parent.OnHide += new System.Action(((UIMenuBase) this).OnHideStarted);
        break;
      default:
        for (int index = 0; index < this.InactiveOnConsole.Count; ++index)
          this.InactiveOnConsole[index].SetActive(false);
        goto case UnifyManager.Platform.None;
    }
  }

  public void Start()
  {
    if (SettingsManager.Settings == null)
      return;
    this._graphicsPresetSelector.LocalizeContent = true;
    this._graphicsPresetSelector.PrefillContent(this._lowMediumHighUltraCustom);
    List<string> content = new List<string>();
    foreach (Vector2Int availableResolution in ScreenUtilities.GetAvailableResolutions())
    {
      Vector2 vector2 = (Vector2) availableResolution;
      content.Add(vector2.ToResolutionString());
    }
    this._resolutionDropdown.PrefillContent(content);
    this._resolutionDropdown.ForceLTR();
    this._fullScreenModeSelector.LocalizeContent = true;
    this._fullScreenModeSelector.PrefillContent(this._fullscreenModes);
    this._targetFpsSelector.PrefillContent(this._framerateSettings);
    this._targetFpsSelector.ForceLTR();
    this._lightingQuality.LocalizeContent = true;
    this._lightingQuality.PrefillContent(this._lowMediumHigh);
    this._environmentDetail.LocalizeContent = true;
    this._environmentDetail.PrefillContent(this._lowHigh);
    this.Configure(SettingsManager.Settings.Graphics);
    this._graphicsPresetSelector.OnSelectionChanged += new Action<int>(this.OnPresetValueChanged);
    this._fullScreenModeSelector.OnSelectionChanged += new Action<int>(this.OnFullscreenModeSelectorValueChanged);
    this._resolutionDropdown.OnValueChanged += new Action<int>(this.OnResolutionSelectorValueChanged);
    this._resolutionDropdown.OnOpenDropdownOverlay += new System.Action(this.HideControlPrompt);
    this._resolutionDropdown.OnCloseDropdownOverlay += new System.Action(this.ShowControlPrompt);
    this._targetFpsSelector.OnSelectionChanged += new Action<int>(this.OnTargetFramerateValueChanged);
    this._vSyncSwitch.OnValueChanged += new Action<bool>(this.OnVSyncSwitchValueChanged);
    this._lightingQuality.OnSelectionChanged += new Action<int>(this.OnLightingQualityValueChanged);
    this._environmentDetail.OnSelectionChanged += new Action<int>(this.OnEnvironmentDetailValueChanged);
    this._shadowsToggle.OnValueChanged += new Action<bool>(this.OnShadowsToggleChanged);
    this._bloomSwitch.OnValueChanged += new Action<bool>(this.OnBloomSwitchValueChanged);
    this._chromaticAberrationSwitch.OnValueChanged += new Action<bool>(this.OnChromaticAberrationSwitchValueChanged);
    this._vignetteSwitch.OnValueChanged += new Action<bool>(this.OnVignetteSwitchValueChanged);
    this._depthOfFieldToggle.OnValueChanged += new Action<bool>(this.OnDepthOfFieldToggleValueChanged);
    this._antiAliasing.OnValueChanged += new Action<bool>(this.OnAntiAliasingToggleValueChanged);
  }

  public void Configure(SettingsData.GraphicsSettings graphicsSettings)
  {
    this._graphicsPresetSelector.ContentIndex = graphicsSettings.GraphicsPreset;
    this._fullScreenModeSelector.ContentIndex = graphicsSettings.FullscreenMode;
    this._resolutionDropdown.ContentIndex = graphicsSettings.Resolution;
    this._targetFpsSelector.ContentIndex = graphicsSettings.TargetFrameRate;
    this._vSyncSwitch.Value = graphicsSettings.VSync;
    this._lightingQuality.ContentIndex = graphicsSettings.LightingQuality;
    this._environmentDetail.ContentIndex = graphicsSettings.EnvironmentDetail;
    this._shadowsToggle.Value = graphicsSettings.Shadows;
    this._bloomSwitch.Value = graphicsSettings.Bloom;
    this._chromaticAberrationSwitch.Value = graphicsSettings.ChromaticAberration;
    this._vignetteSwitch.Value = graphicsSettings.Vignette;
    this._depthOfFieldToggle.Value = graphicsSettings.DepthOfField;
    this._antiAliasing.Value = graphicsSettings.AntiAliasing;
  }

  public void Reset()
  {
    SettingsManager.Settings.Graphics = new SettingsData.GraphicsSettings()
    {
      Resolution = ScreenUtilities.GetDefaultResolution()
    };
    if (!SteamUtils.IsSteamRunningOnSteamDeck())
      return;
    SettingsManager.Settings.Accessibility.TextScale = 1.25f;
  }

  public override void OnShowStarted()
  {
    this._scrollRect.normalizedPosition = Vector2.one;
    this._targetFpsSelectable.Interactable = !this._vSyncSwitch.Value;
    this._scrollRect.enabled = true;
  }

  public override void OnHideStarted() => this._scrollRect.enabled = false;

  public void OnPresetValueChanged(int index)
  {
    SettingsManager.Settings.Graphics.GraphicsPreset = index;
    switch (index)
    {
      case 0:
        this.SetToPreset(GraphicsSettingsUtilities.LowPreset);
        break;
      case 1:
        this.SetToPreset(GraphicsSettingsUtilities.MediumPreset);
        break;
      case 2:
        this.SetToPreset(GraphicsSettingsUtilities.HighPreset);
        break;
      case 3:
        this.SetToPreset(GraphicsSettingsUtilities.UltraPreset);
        break;
      default:
        return;
    }
    this.Configure(SettingsManager.Settings.Graphics);
    GraphicsSettingsUtilities.SetLightingQuality(SettingsManager.Settings.Graphics.LightingQuality);
    GraphicsSettingsUtilities.SetEnvironmentDetail(SettingsManager.Settings.Graphics.EnvironmentDetail);
    GraphicsSettingsUtilities.UpdateShadows(SettingsManager.Settings.Graphics.Shadows);
    GraphicsSettingsUtilities.UpdatePostProcessing();
  }

  public void SetToPreset(
    GraphicsSettingsUtilities.GraphicsPresetValues preset)
  {
    SettingsManager.Settings.Graphics.LightingQuality = preset.LightingQuality;
    SettingsManager.Settings.Graphics.EnvironmentDetail = preset.EnvironmentDetail;
    SettingsManager.Settings.Graphics.Shadows = preset.Shadows;
    SettingsManager.Settings.Graphics.Bloom = preset.Bloom;
    SettingsManager.Settings.Graphics.Vignette = preset.Vignette;
    SettingsManager.Settings.Graphics.ChromaticAberration = preset.ChromaticAberration;
    SettingsManager.Settings.Graphics.DepthOfField = preset.DepthOfField;
    SettingsManager.Settings.Graphics.AntiAliasing = preset.AntiAliasing;
  }

  public void SetToCustomPreset()
  {
    this._graphicsPresetSelector.ContentIndex = SettingsManager.Settings.Graphics.GraphicsPreset = 4;
  }

  public void ShowControlPrompt()
  {
    if (!(bool) (UnityEngine.Object) this.controlPrompts)
      return;
    this.controlPrompts.gameObject.SetActive(true);
  }

  public void HideControlPrompt()
  {
    if (!(bool) (UnityEngine.Object) this.controlPrompts)
      return;
    this.controlPrompts.gameObject.SetActive(false);
  }

  public void OnFullscreenModeSelectorValueChanged(int value)
  {
    SettingsManager.Settings.Graphics.FullscreenMode = value;
    Screen.fullScreenMode = ScreenUtilities.GetFullScreenMode();
    Debug.Log((object) $"GraphicsSettings - FullScreen Mode value changed to {Screen.fullScreenMode}".Colour(Color.yellow));
  }

  public void OnResolutionSelectorValueChanged(int index)
  {
    SettingsManager.Settings.Graphics.Resolution = index;
    ScreenUtilities.ApplyScreenSettings();
    Debug.Log((object) $"GraphicsSettings - Resolution value changed to {index}".Colour(Color.yellow));
  }

  public void OnVSyncSwitchValueChanged(bool value)
  {
    SettingsManager.Settings.Graphics.VSync = value;
    ScreenUtilities.ApplyVSyncSettings();
    this._targetFpsSelectable.Interactable = !this._vSyncSwitch.Value;
    Debug.Log((object) $"GraphicsSettings - VSync value changed to {value}".Colour(Color.yellow));
  }

  public void OnTargetFramerateValueChanged(int index)
  {
    SettingsManager.Settings.Graphics.TargetFrameRate = index;
    GraphicsSettingsUtilities.SetTargetFramerate(index);
    Debug.Log((object) $"GraphicsSettings - Target FPS value changed to {index}".Colour(Color.yellow));
  }

  public void OnLightingQualityValueChanged(int index)
  {
    Debug.Log((object) $"GraphicsSettings - Lighting Quality changed to {index}".Colour(Color.yellow));
    this.SetToCustomPreset();
    SettingsManager.Settings.Graphics.LightingQuality = index;
    GraphicsSettingsUtilities.SetLightingQuality(index);
  }

  public void OnEnvironmentDetailValueChanged(int index)
  {
    Debug.Log((object) $"GraphicsSettings - Environment Detail changed to {index}".Colour(Color.yellow));
    this.SetToCustomPreset();
    SettingsManager.Settings.Graphics.EnvironmentDetail = index;
    GraphicsSettingsUtilities.SetEnvironmentDetail(index);
  }

  public void OnShadowsToggleChanged(bool value)
  {
    Debug.Log((object) $"GraphicsSettings - Shadow toggle value changed to {value}".Colour(Color.yellow));
    this.SetToCustomPreset();
    SettingsManager.Settings.Graphics.Shadows = value;
    GraphicsSettingsUtilities.UpdateShadows(value);
  }

  public void OnBloomSwitchValueChanged(bool value)
  {
    Debug.Log((object) $"GraphicsSettings - Bloom value changed to {value}".Colour(Color.yellow));
    this.SetToCustomPreset();
    SettingsManager.Settings.Graphics.Bloom = value;
    GraphicsSettingsUtilities.UpdatePostProcessing();
  }

  public void OnChromaticAberrationSwitchValueChanged(bool value)
  {
    Debug.Log((object) $"GraphicsSettings - Chromatic Aberration value changed to {value}".Colour(Color.yellow));
    this.SetToCustomPreset();
    SettingsManager.Settings.Graphics.ChromaticAberration = value;
    GraphicsSettingsUtilities.UpdatePostProcessing();
  }

  public void OnVignetteSwitchValueChanged(bool value)
  {
    Debug.Log((object) $"GraphicsSettings - Vignette value changed to {value}".Colour(Color.yellow));
    this.SetToCustomPreset();
    SettingsManager.Settings.Graphics.Vignette = value;
    GraphicsSettingsUtilities.UpdatePostProcessing();
  }

  public void OnDepthOfFieldToggleValueChanged(bool value)
  {
    Debug.Log((object) $"GraphicsSettings - Depth of Field value changed to {value}".Colour(Color.yellow));
    this.SetToCustomPreset();
    SettingsManager.Settings.Graphics.DepthOfField = value;
    GraphicsSettingsUtilities.UpdatePostProcessing();
  }

  public void OnAntiAliasingToggleValueChanged(bool value)
  {
    Debug.Log((object) $"GraphicsSettings - Depth of Field value changed to {value}".Colour(Color.yellow));
    this.SetToCustomPreset();
    SettingsManager.Settings.Graphics.AntiAliasing = value;
    GraphicsSettingsUtilities.UpdatePostProcessing();
  }
}
