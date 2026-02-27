// Decompiled with JetBrains decompiler
// Type: UITwitchExtensionButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class UITwitchExtensionButton : MMButton
{
  public override void Awake()
  {
    base.Awake();
    this.onClick.AddListener(new UnityAction(this.OnClick));
  }

  public void OnClick()
  {
    if (!this.interactable)
      return;
    Application.OpenURL("https://dashboard.twitch.tv/extensions/wph0p912gucvcee0114kfoukn319db");
  }
}
