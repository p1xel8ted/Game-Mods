// Decompiled with JetBrains decompiler
// Type: src.Alerts.CharacterSkinAlerts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using System;
using UnityEngine;

#nullable disable
namespace src.Alerts;

[MessagePackObject(false)]
[Serializable]
public class CharacterSkinAlerts : AlertCategory<string>
{
  public CharacterSkinAlerts()
  {
    DataManager.OnSkinUnlocked += new Action<string>(this.OnSkinUnlocked);
  }

  void object.Finalize()
  {
    try
    {
      DataManager.OnSkinUnlocked -= new Action<string>(this.OnSkinUnlocked);
    }
    finally
    {
      // ISSUE: explicit finalizer call
      base.Finalize();
    }
  }

  public void OnSkinUnlocked(string skinName)
  {
    if (string.IsNullOrEmpty(skinName))
    {
      Debug.Log((object) "Skin unlocked name is null or empty");
    }
    else
    {
      if (WorshipperData.Instance.GetCharacters(skinName) == null)
        throw new Exception("MISSING SKIN: " + skinName);
      if (WorshipperData.Instance.GetCharacters(skinName).Invariant)
        return;
      this.AddOnce(skinName);
    }
  }
}
