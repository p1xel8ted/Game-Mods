// Decompiled with JetBrains decompiler
// Type: src.UI.Alerts.ClothingAlertBadge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.Alerts;

#nullable disable
namespace src.UI.Alerts;

public class ClothingAlertBadge : AlertBadge<FollowerClothingType>
{
  public override AlertCategory<FollowerClothingType> _source
  {
    get => (AlertCategory<FollowerClothingType>) DataManager.Instance.Alerts.ClothingAlerts;
  }
}
