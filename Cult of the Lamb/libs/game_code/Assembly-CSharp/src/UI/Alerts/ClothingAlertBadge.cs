// Decompiled with JetBrains decompiler
// Type: src.UI.Alerts.ClothingAlertBadge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
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
