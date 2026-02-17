// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Alerts.WeaponAlert
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
namespace Lamb.UI.Alerts;

public class WeaponAlert : AlertBadge<EquipmentType>
{
  public override AlertCategory<EquipmentType> _source
  {
    get => (AlertCategory<EquipmentType>) DataManager.Instance.Alerts.Weapons;
  }
}
