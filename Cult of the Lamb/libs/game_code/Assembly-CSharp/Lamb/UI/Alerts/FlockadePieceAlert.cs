// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Alerts.FlockadePieceAlert
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
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
