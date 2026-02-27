// Decompiled with JetBrains decompiler
// Type: src.Alerts.WeaponAlerts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace src.Alerts;

public class WeaponAlerts : AlertCategory<EquipmentType>
{
  public WeaponAlerts()
  {
    DataManager.OnWeaponUnlocked += new Action<EquipmentType>(this.OnWeaponUnlocked);
  }

  ~WeaponAlerts()
  {
    if (DataManager.Instance == null)
      return;
    DataManager.OnWeaponUnlocked -= new Action<EquipmentType>(this.OnWeaponUnlocked);
  }

  private void OnWeaponUnlocked(EquipmentType weapon) => this.AddOnce(weapon);
}
