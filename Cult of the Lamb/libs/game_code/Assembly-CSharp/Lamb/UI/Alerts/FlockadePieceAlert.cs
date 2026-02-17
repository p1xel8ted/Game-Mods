// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Alerts.FlockadePieceAlert
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
