// Decompiled with JetBrains decompiler
// Type: UITwitchExtensionButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class UITwitchExtensionButton : MMButton
{
  protected override void Awake()
  {
    base.Awake();
    this.onClick.AddListener(new UnityAction(this.OnClick));
  }

  private void OnClick()
  {
    if (!this.interactable)
      return;
    Application.OpenURL("https://dashboard.twitch.tv/extensions/wph0p912gucvcee0114kfoukn319db");
  }
}
