// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Alerts.TarotCardAlert_Run
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
namespace Lamb.UI.Alerts;

public class TarotCardAlert_Run : TarotCardAlertBase
{
  public override AlertCategory<TarotCards.Card> _source
  {
    get => (AlertCategory<TarotCards.Card>) DataManager.Instance.Alerts.RunTarotCardAlerts;
  }
}
