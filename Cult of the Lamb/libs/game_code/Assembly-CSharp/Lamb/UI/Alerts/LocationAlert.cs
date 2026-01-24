// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Alerts.LocationAlert
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
namespace Lamb.UI.Alerts;

public class LocationAlert : AlertBadge<FollowerLocation>
{
  public override AlertCategory<FollowerLocation> _source
  {
    get => (AlertCategory<FollowerLocation>) DataManager.Instance.Alerts.Locations;
  }

  public override bool HasAlertSingle()
  {
    return DataManager.Instance.DiscoveredLocations.Contains(this._alert) && !DataManager.Instance.VisitedLocations.Contains(this._alert) || this._source.HasAlert(this._alert);
  }
}
