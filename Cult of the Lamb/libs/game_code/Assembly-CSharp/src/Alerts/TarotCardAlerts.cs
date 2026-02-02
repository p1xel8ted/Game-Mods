// Decompiled with JetBrains decompiler
// Type: src.Alerts.TarotCardAlerts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
