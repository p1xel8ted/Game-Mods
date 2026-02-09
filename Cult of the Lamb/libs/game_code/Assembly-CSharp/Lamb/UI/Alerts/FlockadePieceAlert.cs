// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Alerts.FlockadePieceAlert
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Flockade;

#nullable disable
namespace Lamb.UI.Alerts;

public class FlockadePieceAlert : AlertBadge<FlockadePieceType>
{
  public override AlertCategory<FlockadePieceType> _source
  {
    get => (AlertCategory<FlockadePieceType>) DataManager.Instance.Alerts.FlockadePieceAlerts;
  }
}
