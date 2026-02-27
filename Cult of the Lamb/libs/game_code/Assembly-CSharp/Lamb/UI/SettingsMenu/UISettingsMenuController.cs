// Decompiled with JetBrains decompiler
// Type: Lamb.UI.SettingsMenu.UISettingsMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using src.UI;
using src.UINavigator;
using UnityEngine;

#nullable disable
namespace Lamb.UI.SettingsMenu;

public class UISettingsMenuController : UIMenuBase
{
  [Header("Menus")]
  [SerializeField]
  public Lamb.UI.Settings.GameSettings _gameSettings;
  [SerializeField]
  public GraphicsSettings _graphicsSettings;
  [SerializeField]
  public AccessibilitySettings _accessibilitySettings;
  [SerializeField]
  public AudioSettings _audioSettings;
  [SerializeField]
  public ControlSettings _controlSettings;

  public override void OnShowStarted() => UIManager.PlayAudio("event:/ui/open_menu");

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    this.SaveAndApply();
    this.Hide();
  }

  public void Update()
  {
    if (!this._canvasGroup.interactable || !InputManager.UI.GetResetAllSettingsButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
      return;
    this.ResetAll();
  }

  public override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

  public void ResetAll()
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

  public void SaveAndApply()
  {
    if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
      DifficultyManager.ForceDifficulty(DataManager.Instance.MetaData.Difficulty);
    Singleton<SettingsManager>.Instance.SaveSettings();
    Singleton<SettingsManager>.Instance.ApplySettings();
  }

  public override void OnHideStarted()
  {
    base.OnHideStarted();
    UIManager.PlayAudio("event:/ui/close_menu");
  }
}
