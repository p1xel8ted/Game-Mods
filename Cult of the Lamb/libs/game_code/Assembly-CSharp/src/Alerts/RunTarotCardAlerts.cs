// Decompiled with JetBrains decompiler
// Type: src.Alerts.RunTarotCardAlerts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using System;

#nullable disable
namespace src.Alerts;

[MessagePackObject(false)]
[Serializable]
public class RunTarotCardAlerts : AlertCategory<TarotCards.Card>
{
  public RunTarotCardAlerts()
  {
    TrinketManager.OnTrinketAdded += new TrinketManager.TrinketUpdated(this.OnTarotCardAdded);
  }

  void object.Finalize()
  {
    try
    {
      TrinketManager.OnTrinketAdded -= new TrinketManager.TrinketUpdated(this.OnTarotCardAdded);
    }
    finally
    {
      // ISSUE: explicit finalizer call
      base.Finalize();
    }
  }

  public void OnTarotCardAdded(TarotCards.Card card, PlayerFarming playerFarming = null)
  {
    this.AddOnce(card);
  }
}
