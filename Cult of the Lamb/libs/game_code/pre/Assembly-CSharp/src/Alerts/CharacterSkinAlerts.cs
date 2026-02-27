// Decompiled with JetBrains decompiler
// Type: src.Alerts.CharacterSkinAlerts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace src.Alerts;

public class CharacterSkinAlerts : AlertCategory<string>
{
  public CharacterSkinAlerts()
  {
    DataManager.OnSkinUnlocked += new Action<string>(this.OnSkinUnlocked);
  }

  ~CharacterSkinAlerts() => DataManager.OnSkinUnlocked -= new Action<string>(this.OnSkinUnlocked);

  private void OnSkinUnlocked(string skinName)
  {
    if (skinName.Equals("Boss Death Cat"))
      return;
    this.AddOnce(skinName);
  }
}
