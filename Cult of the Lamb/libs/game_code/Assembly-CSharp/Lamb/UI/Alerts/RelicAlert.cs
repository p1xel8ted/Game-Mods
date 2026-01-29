// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Alerts.RelicAlert
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
namespace Lamb.UI.Alerts;

public class RelicAlert : AlertBadge<RelicType>
{
  public override AlertCategory<RelicType> _source
  {
    get => (AlertCategory<RelicType>) DataManager.Instance.Alerts.RelicAlerts;
  }
}
