// Decompiled with JetBrains decompiler
// Type: src.Alerts.TarotCardAlerts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace src.Alerts;

public class TarotCardAlerts : AlertCategory<TarotCards.Card>
{
  ~TarotCardAlerts()
  {
  }

  private void OnTarotCardAdded(TarotCards.Card card) => this.AddOnce(card);
}
