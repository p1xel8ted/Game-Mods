// Decompiled with JetBrains decompiler
// Type: UITwitchExtensionButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
