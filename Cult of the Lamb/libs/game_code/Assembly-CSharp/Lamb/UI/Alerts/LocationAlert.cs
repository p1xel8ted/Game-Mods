// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Alerts.LocationAlert
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
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
