// Decompiled with JetBrains decompiler
// Type: UI.Menus.TwitchMenu.UITwitchSettingsMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using src.Data;
using src.UI;
using src.UINavigator;
using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace UI.Menus.TwitchMenu;

public class UITwitchSettingsMenuController : UIMenuBase
{
  [Header("Admin")]
  [SerializeField]
  public MMButton connectionButton;
  [SerializeField]
  public TMP_Text _connectionText;
  [SerializeField]
  public MMButton _integrationConfiguration;
  [Header("Settings")]
  [SerializeField]
  public MMToggle _helpHinderToggle;
  [SerializeField]
  public MMSlider _helpHinderFrequency;
  [SerializeField]
  public MMToggle _enableTotem;
  [SerializeField]
  public MMToggle _showTwitchFollowerNames;
  [SerializeField]
  public MMToggle _twitchMessages;

  public override void Awake()
  {
    base.Awake();
    this.connectionButton.onClick.AddListener(new UnityAction(this.OnConnectionButtonPressed));
    this._integrationConfiguration.onClick.AddListener(new UnityAction(this.OnIntegrationConfigurationButtonClicked));
    this.Configure(DataManager.Instance.TwitchSettings);
    this._helpHinderToggle.OnValueChanged += new Action<bool>(this.OnHelpHinderToggleValueChanged);
    this._helpHinderFrequency.onValueChanged.AddListener(new UnityAction<float>(this.OnHelpHinderFrequencyValueChanged));
    this._enableTotem.OnValueChanged += new Action<bool>(this.OnEnableTotemValueChanged);
    this._showTwitchFollowerNames.OnValueChanged += new Action<bool>(this.OnShowTwitchFollowersValueChanged);
    this._twitchMessages.OnValueChanged += new Action<bool>(this.OnTwitchMessagesValueChanged);
    this._helpHinderFrequency.minValue = 20f;
    this._helpHinderFrequency.maxValue = 50f;
    this._helpHinderFrequency.GetCustomDisplayFormat = new Func<float, string>(this.SetSliderText);
    if (!string.IsNullOrEmpty(TwitchManager.ChannelName) && TwitchAuthentication.IsAuthenticated)
      this._connectionText.text = $"{TwitchManager.ChannelName} - {ScriptLocalization.UI_Twitch.SignOut}";
    else
      this._connectionText.text = ScriptLocalization.UI_Twitch.Connect;
  }

  public void Configure(TwitchSettings twitchSettings)
  {
    this._helpHinderToggle.Value = twitchSettings.HelpHinderEnabled;
    this._helpHinderFrequency.value = twitchSettings.HelpHinderFrequency;
    this._enableTotem.Value = twitchSettings.TotemEnabled;
    this._showTwitchFollowerNames.Value = twitchSettings.FollowerNamesEnabled;
    this._twitchMessages.Value = twitchSettings.TwitchMessagesEnabled;
  }

  public void Update()
  {
    if (!InputManager.UI.GetResetAllSettingsButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
      return;
    this.ResetAll();
  }

  public void ResetAll()
  {
    UIMenuConfirmationWindow uiMenuConfirmationWindowInstance = this.Push<UIMenuConfirmationWindow>(MonoSingleton<UIManager>.Instance.ConfirmationWindowTemplate);
    uiMenuConfirmationWindowInstance.Configure(LocalizationManager.GetTranslation("UI/Settings/Graphics/RevertToDefaultHeader"), LocalizationManager.GetTranslation("UI/Settings/Graphics/RevertToDefault"));
    uiMenuConfirmationWindowInstance.OnConfirm += (System.Action) (() =>
    {
      DataManager.Instance.TwitchSettings = new TwitchSettings();
      this.Configure(DataManager.Instance.TwitchSettings);
    });
    UIMenuConfirmationWindow confirmationWindow = uiMenuConfirmationWindowInstance;
    confirmationWindow.OnHidden = confirmationWindow.OnHidden + (System.Action) (() => uiMenuConfirmationWindowInstance = (UIMenuConfirmationWindow) null);
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    this.Hide();
  }

  public override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

  public string SetSliderText(float value) => $"{value} mins";

  public void OnConnectionButtonPressed()
  {
    if (!TwitchAuthentication.IsAuthenticated)
    {
      TwitchAuthentication.RequestLogIn((Action<TwitchRequest.ResponseType>) (response =>
      {
        if (string.IsNullOrEmpty(TwitchManager.ChannelName))
          return;
        this._connectionText.text = $"{TwitchManager.ChannelName} - {ScriptLocalization.UI_Twitch.SignOut}";
      }));
    }
    else
    {
      TwitchAuthentication.SignOut();
      this._connectionText.text = ScriptLocalization.UI_Twitch.Connect;
    }
  }

  public void OnIntegrationConfigurationButtonClicked()
  {
    Application.OpenURL("https://dashboard.twitch.tv/extensions/wph0p912gucvcee0114kfoukn319db");
  }

  public void OnHelpHinderToggleValueChanged(bool value) => TwitchManager.HelpHinderEnabled = value;

  public void OnHelpHinderFrequencyValueChanged(float value)
  {
    TwitchManager.HelpHinderFrequency = value;
  }

  public void OnEnableTotemValueChanged(bool value) => TwitchManager.TotemEnabled = value;

  public void OnShowTwitchFollowersValueChanged(bool value)
  {
    TwitchManager.FollowerNamesEnabled = value;
  }

  public void OnTwitchMessagesValueChanged(bool value) => TwitchManager.MessagesEnabled = value;

  [CompilerGenerated]
  public void \u003COnConnectionButtonPressed\u003Eb__15_0(TwitchRequest.ResponseType response)
  {
    if (string.IsNullOrEmpty(TwitchManager.ChannelName))
      return;
    this._connectionText.text = $"{TwitchManager.ChannelName} - {ScriptLocalization.UI_Twitch.SignOut}";
  }
}
