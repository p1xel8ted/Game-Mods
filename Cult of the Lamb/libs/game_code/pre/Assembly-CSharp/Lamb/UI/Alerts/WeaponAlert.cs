// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Alerts.WeaponAlert
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace Lamb.UI.Alerts;

public class WeaponAlert : AlertBadge<EquipmentType>
{
  protected override AlertCategory<EquipmentType> _source
  {
    get => (AlertCategory<EquipmentType>) DataManager.Instance.Alerts.Weapons;
  }
}
