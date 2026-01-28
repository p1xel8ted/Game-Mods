// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Alerts.FlockadePieceAlert
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
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
