// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Alerts.LocationAlert
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
