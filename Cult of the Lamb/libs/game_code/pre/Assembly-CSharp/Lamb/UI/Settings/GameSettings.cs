// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Settings.GameSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private MMSelectable_HorizontalSelector _difficultySelectable;
  [SerializeField]
  private MMHorizontalSelector _difficultySelector;
  [SerializeField]
  private UnityEngine.UI.Slider _rumbleSlider;
  [SerializeField]
  private MMToggle _disableTutorialsSwitch;
  [SerializeField]
  private MMHorizontalSelector _languageSelector;
  [SerializeField]
  private Localize _rumbleLocalize;
  [SerializeField]
  private MMHorizontalSelector _gamepadPrompts;
  private string _cachedLanguage;
  private string[] _gamepadPromptsContent = new string[5]
  {
    "UI/Settings/Graphics/ControlPrompts/Auto",
    "UI/Settings/Graphics/ControlPrompts/Xbox",
    "UI/Settings/Controls/Controller_PLAYSTATION4",
    "UI/Settings/Controls/Controller_PLAYSTATION5",
    "UI/Settings/Graphics/ControlPrompts/Switch"
  };

  public override void Awake()
  {
  }

  protected override void SetActiveStateForMenu(GameObject target, bool state)
  {
    base.SetActiveStateForMenu(target, state);
    if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
      this._difficultySelectable.Interactable = DataManager.Instance.DifficultyChosen;
    else
      this._difficultySelectable.Interactable = false;
  }

  protected override void OnShowStarted()
  {
    if (SettingsManager.Settings == null)
      return;
    if (this._difficultySelectable.Interactable)
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
    this._cachedLanguage = gameSettings.Language;
  }

  public void Reset()
  {
    DataManager.Instance.MetaData = MetaData.Default(DataManager.Instance);
    SettingsManager.Settings.Game = new SettingsData.GameSettings()
    {
      Language = this._cachedLanguage
    };
  }

  private int GetLanguageIndex(string language)
  {
    return LanguageUtilities.AllLanguages.IndexOf<string>(language);
  }

  private void OnRumbleIntensityChanged(float rumbleIntensity)
  {
    rumbleIntensity /= 100f;
    SettingsManager.Settings.Game.RumbleIntensity = rumbleIntensity;
    MMVibrate.SetHapticsIntensity(rumbleIntensity);
    RumbleManager.Instance.Rumble();
    Debug.Log((object) $"GameSettings - Change rumble intensity to {rumbleIntensity}");
  }

  private void OnTutorialSwitchValueChanged(bool value)
  {
    SettingsManager.Settings.Game.ShowTutorials = !value;
    Debug.Log((object) $"GameSettings - Change tutorial value to {!value}");
  }

  private void OnLanguageChanged(int index)
  {
    string allLanguage = LanguageUtilities.AllLanguages[index];
    SettingsManager.Settings.Game.Language = allLanguage;
    LocalizationManager.CurrentLanguage = allLanguage;
    this._cachedLanguage = allLanguage;
    if (TwitchAuthentication.IsAuthenticated)
      TwitchRequest.SendEBSData();
    Debug.Log((object) ("GameSettings - Change Language to " + allLanguage));
  }

  private void OnDifficultyChanged(int index)
  {
    DifficultyManager.Difficulty availableDifficulty = DifficultyManager.AllAvailableDifficulties()[index];
    DifficultyManager.ForceDifficulty(availableDifficulty);
    DataManager.Instance.MetaData.Difficulty = index;
    Debug.Log((object) $"GameSettings - Change Difficulty to {availableDifficulty}".Colour(Color.yellow));
  }

  private void OnGamepadPromptsChanged(int index)
  {
    Debug.Log((object) $"GameSettings - Change Gamepad Prompts to {index}".Colour(Color.yellow));
    SettingsManager.Settings.Game.GamepadPrompts = index;
    ControlSettingsUtilities.UpdateGamepadPrompts();
  }
}
