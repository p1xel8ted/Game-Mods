// Decompiled with JetBrains decompiler
// Type: UITwitchButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

#nullable disable
public class UITwitchButton : MMButton
{
  private TMP_Text buttonText;

  protected override void Awake()
  {
    base.Awake();
    this.buttonText = this.GetComponentInChildren<TMP_Text>();
    this.onClick.AddListener(new UnityAction(this.OnClick));
  }

  protected override void OnEnable()
  {
    base.OnEnable();
    this.buttonText.text = LocalizationManager.GetTranslation("UI/Twitch/Connect");
    if (string.IsNullOrEmpty(TwitchManager.ChannelName) || !TwitchAuthentication.IsAuthenticated)
      return;
    this.buttonText.text = $"{TwitchManager.ChannelName} - {LocalizationManager.GetTranslation("UI/Twitch/SignOut")}";
  }

  public override void OnPointerClick(PointerEventData eventData) => base.OnPointerClick(eventData);

  private void OnClick()
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
}
