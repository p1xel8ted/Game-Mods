// Decompiled with JetBrains decompiler
// Type: Lamb.UI.SettingsMenu.UISettingsMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using src.UI;
using UnityEngine;

#nullable disable
namespace Lamb.UI.SettingsMenu;

public class UISettingsMenuController : UIMenuBase
{
  [Header("Menus")]
  [SerializeField]
  private Lamb.UI.Settings.GameSettings _gameSettings;
  [SerializeField]
  private GraphicsSettings _graphicsSettings;
  [SerializeField]
  private AccessibilitySettings _accessibilitySettings;
  [SerializeField]
  private AudioSettings _audioSettings;
  [SerializeField]
  private ControlSettings _controlSettings;

  protected override void OnShowStarted() => UIManager.PlayAudio("event:/ui/open_menu");

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    this.SaveAndApply();
    this.Hide();
  }

  private void Update()
  {
    if (!this._canvasGroup.interactable || !InputManager.UI.GetResetAllSettingsButtonDown())
      return;
    this.ResetAll();
  }

  protected override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

  private void ResetAll()
  {
    UIMenuConfirmationWindow uiMenuConfirmationWindowInstance = this.Push<UIMenuConfirmationWindow>(MonoSingleton<UIManager>.Instance.ConfirmationWindowTemplate);
    uiMenuConfirmationWindowInstance.Configure(LocalizationManager.GetTranslation("UI/Settings/Graphics/RevertToDefaultHeader"), LocalizationManager.GetTranslation("UI/Settings/Graphics/RevertToDefault"));
    uiMenuConfirmationWindowInstance.OnConfirm += (System.Action) (() =>
    {
      this._gameSettings.Reset();
      this._graphicsSettings.Reset();
      this._accessibilitySettings.Reset();
      this._audioSettings.Reset();
      this._controlSettings.Reset();
      this._gameSettings.Configure(SettingsManager.Settings.Game, DataManager.Instance.MetaData);
      this._graphicsSettings.Configure(SettingsManager.Settings.Graphics);
      this._accessibilitySettings.Configure(SettingsManager.Settings.Accessibility);
      this._audioSettings.Configure(SettingsManager.Settings.Audio);
      this._controlSettings.Configure(SettingsManager.Settings.Control);
      this.SaveAndApply();
    });
    UIMenuConfirmationWindow confirmationWindow = uiMenuConfirmationWindowInstance;
    confirmationWindow.OnHidden = confirmationWindow.OnHidden + (System.Action) (() => uiMenuConfirmationWindowInstance = (UIMenuConfirmationWindow) null);
  }

  private void SaveAndApply()
  {
    if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
      DifficultyManager.ForceDifficulty(DataManager.Instance.MetaData.Difficulty);
    Singleton<SettingsManager>.Instance.SaveSettings();
    Singleton<SettingsManager>.Instance.ApplySettings();
  }

  protected override void OnHideStarted()
  {
    base.OnHideStarted();
    UIManager.PlayAudio("event:/ui/close_menu");
  }
}
