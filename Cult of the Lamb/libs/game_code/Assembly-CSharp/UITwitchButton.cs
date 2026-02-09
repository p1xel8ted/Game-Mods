// Decompiled with JetBrains decompiler
// Type: UITwitchButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

#nullable disable
public class UITwitchButton : MMButton
{
  public TMP_Text buttonText;

  public override void Awake()
  {
    base.Awake();
    this.buttonText = this.GetComponentInChildren<TMP_Text>();
    this.onClick.AddListener(new UnityAction(this.OnClick));
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.buttonText.text = LocalizationManager.GetTranslation("UI/Twitch/Connect");
    if (string.IsNullOrEmpty(TwitchManager.ChannelName) || !TwitchAuthentication.IsAuthenticated)
      return;
    this.buttonText.text = $"{TwitchManager.ChannelName} - {LocalizationManager.GetTranslation("UI/Twitch/SignOut")}";
  }

  public override void OnPointerClick(PointerEventData eventData) => base.OnPointerClick(eventData);

  public void OnClick()
  {
    if (!this.interactable)
      return;
    if (!TwitchAuthentication.IsAuthenticated)
    {
      TwitchAuthentication.RequestLogIn((Action<TwitchRequest.ResponseType>) (response =>
      {
        if (string.IsNullOrEmpty(TwitchManager.ChannelName))
          return;
        this.buttonText.text = $"{TwitchManager.ChannelName} - {LocalizationManager.GetTranslation("UI/Twitch/SignOut")}";
      }));
    }
    else
    {
      TwitchAuthentication.SignOut();
      this.buttonText.text = LocalizationManager.GetTranslation("UI/Twitch/Connect");
    }
  }

  [CompilerGenerated]
  public void \u003COnClick\u003Eb__4_0(TwitchRequest.ResponseType response)
  {
    if (string.IsNullOrEmpty(TwitchManager.ChannelName))
      return;
    this.buttonText.text = $"{TwitchManager.ChannelName} - {LocalizationManager.GetTranslation("UI/Twitch/SignOut")}";
  }
}
