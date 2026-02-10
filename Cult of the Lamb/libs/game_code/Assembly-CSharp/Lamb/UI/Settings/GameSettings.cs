// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Settings.GameSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System;
using Unify;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace Lamb.UI.Settings;

public class GameSettings : UISubmenuBase
{
  [Header("Settings")]
  [SerializeField]
  public GameObject _difficultyLockIcon;
  [SerializeField]
  public MMSelectable_HorizontalSelector _difficultySelectable;
  [SerializeField]
  public MMSelectable_Toggle _permadeathToggle;
  [SerializeField]
  public MMHorizontalSelector _difficultySelector;
  [SerializeField]
  public UnityEngine.UI.Slider _rumbleSlider;
  [SerializeField]
  public MMToggle _disableTutorialsSwitch;
  [SerializeField]
  public MMHorizontalSelector _languageSelector;
  [SerializeField]
  public Localize _rumbleLocalize;
  [SerializeField]
  public MMHorizontalSelector _gamepadPrompts;
  [SerializeField]
  public MMToggle _showFollowerNames;
  [SerializeField]
  public UnityEngine.UI.Slider _horizontalAimSensitivity;
  [SerializeField]
  public UnityEngine.UI.Slider _verticalAimSensitivity;
  [SerializeField]
  public MMToggle _invertAimingToggle;
  [SerializeField]
  public MMToggle _performanceModeToggle;
  public string _cachedLanguage;
  public string[] _gamepadPromptsContent = new string[5]
  {
    "UI/Settings/Graphics/ControlPrompts/Auto",
    "UI/Settings/Graphics/ControlPrompts/Xbox",
    "UI/Settings/Controls/Controller_PLAYSTATION4",
    "UI/Settings/Controls/Controller_PLAYSTATION5",
    "UI/Settings/Graphics/ControlPrompts/Switch"
  };

  public override void Awake()
  {
    this._performanceModeToggle.gameObject.GetComponentInParent<disablePerPlatform>().gameObject.SetActive(false);
  }

  public override void SetActiveStateForMenu(GameObject target, bool state)
  {
    base.SetActiveStateForMenu(target, state);
    if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
    {
      this._difficultySelectable.Interactable = DataManager.Instance.DifficultyChosen && !DataManager.Instance.PermadeDeathActive;
      this._difficultyLockIcon.SetActive(DataManager.Instance.PermadeDeathActive);
      this._permadeathToggle.gameObject.SetActive(DataManager.Instance.PermadeDeathActive);
      this._permadeathToggle.Interactable = false;
    }
    else
    {
      this._difficultySelectable.Interactable = false;
      this._permadeathToggle.gameObject.SetActive(false);
      this._difficultyLockIcon.SetActive(false);
    }
  }

  public override void OnShowStarted()
  {
    if (SettingsManager.Settings == null)
      return;
    if ((this._difficultySelectable.Interactable || DataManager.Instance.PermadeDeathActive) && DataManager.Instance.DifficultyChosen)
    {
      this._difficultySelector.LocalizeContent = true;
      this._difficultySelector.PrefillContent(DifficultyManager.GetDifficultyLocalisation());
    }
    else
      this._difficultySelector.PrefillContent("---");
    this._languageSelector.LocalizeContent = true;
    this._languageSelector.PrefillContent(LanguageUtilities.AllLanguagesLocalizations);
    this._gamepadPrompts.LocalizeContent = true;
    this._gamepadPrompts.PrefillContent(this._gamepadPromptsContent);
    this._difficultySelector.OnSelectionChanged += new Action<int>(this.OnDifficultyChanged);
    this._rumbleSlider.onValueChanged.AddListener(new UnityAction<float>(this.OnRumbleIntensityChanged));
    this._disableTutorialsSwitch.OnValueChanged += new Action<bool>(this.OnTutorialSwitchValueChanged);
    this._languageSelector.OnSelectionChanged += new Action<int>(this.OnLanguageChanged);
    this._gamepadPrompts.OnSelectionChanged += new Action<int>(this.OnGamepadPromptsChanged);
    this._showFollowerNames.OnValueChanged += new Action<bool>(this.OnShowFollowerNamesChanged);
    this._horizontalAimSensitivity.onValueChanged.AddListener(new UnityAction<float>(this.OnHorizontalAimSensitivitySettingChanged));
    this._verticalAimSensitivity.onValueChanged.AddListener(new UnityAction<float>(this.OnVerticalAimSensitivitySettingChanged));
    this._invertAimingToggle.OnValueChanged += new Action<bool>(this.OnInvertAimingValueChanged);
    this._performanceModeToggle.OnValueChanged += new Action<bool>(this.OnPerformanceModeValueChanged);
    switch (UnifyManager.platform)
    {
      case UnifyManager.Platform.XboxOne:
        this._rumbleLocalize.Term = "UI/Settings/Game/GamepadRumbleIntensity_XBOX";
        break;
      case UnifyManager.Platform.PS4:
      case UnifyManager.Platform.PS5:
        this._rumbleLocalize.Term = "UI/Settings/Game/GamepadRumbleIntensity_PLAYSTATION";
        break;
      case UnifyManager.Platform.Switch:
        this._rumbleLocalize.Term = "UI/Settings/Game/GamepadRumbleIntensity_SWITCH";
        break;
    }
    this.Configure(SettingsManager.Settings.Game, DataManager.Instance.MetaData);
  }

  public void Configure(SettingsData.GameSettings gameSettings, MetaData metaData)
  {
    this._difficultySelector.ContentIndex = !((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null) ? 0 : metaData.Difficulty;
    this._rumbleSlider.value = gameSettings.RumbleIntensity * 100f;
    this._disableTutorialsSwitch.Value = !gameSettings.ShowTutorials;
    this._languageSelector.ContentIndex = this.GetLanguageIndex(gameSettings.Language);
    this._gamepadPrompts.ContentIndex = gameSettings.GamepadPrompts;
    this._showFollowerNames.Value = gameSettings.ShowFollowerNames;
    this._cachedLanguage = gameSettings.Language;
    this._horizontalAimSensitivity.value = gameSettings.HorizontalAimSensitivity * 100f;
    this._verticalAimSensitivity.value = gameSettings.HorizontalAimSensitivity * 100f;
    this._invertAimingToggle.Value = gameSettings.InvertAiming;
    this._performanceModeToggle.Value = gameSettings.PerformanceMode;
  }

  public void Reset()
  {
    DataManager.Instance.MetaData = MetaData.Default(DataManager.Instance);
    SettingsManager.Settings.Game = new SettingsData.GameSettings()
    {
      Language = this._cachedLanguage
    };
  }

  public int GetLanguageIndex(string language)
  {
    return LanguageUtilities.AllLanguages.IndexOf<string>(language);
  }

  public void OnRumbleIntensityChanged(float rumbleIntensity)
  {
    rumbleIntensity /= 100f;
    SettingsManager.Settings.Game.RumbleIntensity = rumbleIntensity;
    MMVibrate.SetHapticsIntensity(rumbleIntensity);
    RumbleManager.Instance.Rumble();
    Debug.Log((object) $"GameSettings - Change rumble intensity to {rumbleIntensity}");
  }

  public void OnTutorialSwitchValueChanged(bool value)
  {
    SettingsManager.Settings.Game.ShowTutorials = !value;
    Debug.Log((object) $"GameSettings - Change tutorial value to {!value}");
  }

  public void OnLanguageChanged(int index)
  {
    LocalizationManager.ForceLoadSynchronous = true;
    string allLanguage = LanguageUtilities.AllLanguages[index];
    SettingsManager.Settings.Game.Language = allLanguage;
    LocalizationManager.CurrentLanguage = allLanguage;
    LocalizationManager.EnableChangingCultureInfo(true);
    this._cachedLanguage = allLanguage;
    LocalizationManager.SetupFonts();
    LocalizationManager.ForceLoadSynchronous = false;
    TwitchManager.SetLanguage(LocalizationManager.CurrentLanguageCode);
    Debug.Log((object) ("GameSettings - Change Language to " + allLanguage));
  }

  public void OnDifficultyChanged(int index)
  {
    DifficultyManager.Difficulty availableDifficulty = DifficultyManager.AllAvailableDifficulties()[index];
    DifficultyManager.ForceDifficulty(availableDifficulty);
    DataManager.Instance.MetaData.Difficulty = index;
    Debug.Log((object) $"GameSettings - Change Difficulty to {availableDifficulty}".Colour(Color.yellow));
  }

  public void OnGamepadPromptsChanged(int index)
  {
    Debug.Log((object) $"GameSettings - Change Gamepad Prompts to {index}".Colour(Color.yellow));
    SettingsManager.Settings.Game.GamepadPrompts = index;
    ControlSettingsUtilities.UpdateGamepadPrompts();
  }

  public void OnShowFollowerNamesChanged(bool value)
  {
    Debug.Log((object) $"GameSettings - Change Show Follower Names to {value}".Colour(Color.yellow));
    SettingsManager.Settings.Game.ShowFollowerNames = value;
    GameplaySettingsUtilities.UpdateShowFollowerNamesSetting(value);
  }

  public void OnHorizontalAimSensitivitySettingChanged(float value)
  {
    Debug.Log((object) $"GameSettings - Change Horizontal Aim Sensitivity to {value}".Colour(Color.yellow));
    value /= 100f;
    SettingsManager.Settings.Game.HorizontalAimSensitivity = value;
  }

  public void OnVerticalAimSensitivitySettingChanged(float value)
  {
    Debug.Log((object) $"GameSettings - Change Vertical Aim Sensitivity to {value}".Colour(Color.yellow));
    value /= 100f;
    SettingsManager.Settings.Game.VerticalAimSensitivity = value;
  }

  public void OnInvertAimingValueChanged(bool value)
  {
    Debug.Log((object) $"GameSettings - Change Invert Aiming to {value}".Colour(Color.yellow));
    SettingsManager.Settings.Game.InvertAiming = value;
  }

  public void OnPerformanceModeValueChanged(bool value)
  {
  }
}
