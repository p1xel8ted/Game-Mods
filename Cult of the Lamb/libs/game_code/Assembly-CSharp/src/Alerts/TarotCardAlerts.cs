// Decompiled with JetBrains decompiler
// Type: src.Alerts.TarotCardAlerts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;

#nullable disable
namespace src.Alerts;

[MessagePackObject(false)]
public class TarotCardAlerts : AlertCategory<TarotCards.Card>
{
  void object.Finalize()
  {
    try
    {
    }
    finally
    {
      // ISSUE: explicit finalizer call
      base.Finalize();
    }
  }

  public void OnTarotCardAdded(TarotCards.Card card) => this.AddOnce(card);
}
