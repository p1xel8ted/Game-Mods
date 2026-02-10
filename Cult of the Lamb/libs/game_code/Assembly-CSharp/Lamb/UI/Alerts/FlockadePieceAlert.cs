// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Alerts.FlockadePieceAlert
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
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
