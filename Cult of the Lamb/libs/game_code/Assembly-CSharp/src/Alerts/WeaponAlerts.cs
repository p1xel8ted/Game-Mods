// Decompiled with JetBrains decompiler
// Type: src.Alerts.WeaponAlerts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using System;

#nullable disable
namespace src.Alerts;

[MessagePackObject(false)]
[Serializable]
public class WeaponAlerts : AlertCategory<EquipmentType>
{
  public WeaponAlerts()
  {
    DataManager.OnWeaponUnlocked += new Action<EquipmentType>(this.OnWeaponUnlocked);
  }

  void object.Finalize()
  {
    try
    {
      if (DataManager.Instance == null)
        return;
      DataManager.OnWeaponUnlocked -= new Action<EquipmentType>(this.OnWeaponUnlocked);
    }
    finally
    {
      // ISSUE: explicit finalizer call
      base.Finalize();
    }
  }

  public void OnWeaponUnlocked(EquipmentType weapon) => this.AddOnce(weapon);
}
