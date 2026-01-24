// Decompiled with JetBrains decompiler
// Type: src.UI.Alerts.ClothingCustomiseAlertBadge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.Alerts;

#nullable disable
namespace src.UI.Alerts;

public class ClothingCustomiseAlertBadge : AlertBadge<FollowerClothingType>
{
  public override AlertCategory<FollowerClothingType> _source
  {
    get
    {
      return (AlertCategory<FollowerClothingType>) DataManager.Instance.Alerts.ClothingCustomiseAlerts;
    }
  }
}
