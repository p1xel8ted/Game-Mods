// Decompiled with JetBrains decompiler
// Type: src.Alerts.CurseAlerts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using System;

#nullable disable
namespace src.Alerts;

[MessagePackObject(false)]
[Serializable]
public class CurseAlerts : AlertCategory<TarotCards.Card>
{
  public CurseAlerts()
  {
    DataManager.OnCurseUnlocked += new Action<TarotCards.Card>(this.OnCurseUnlocked);
  }

  void object.Finalize()
  {
    try
    {
      if (DataManager.Instance == null)
        return;
      DataManager.OnCurseUnlocked -= new Action<TarotCards.Card>(this.OnCurseUnlocked);
    }
    finally
    {
      // ISSUE: explicit finalizer call
      base.Finalize();
    }
  }

  public void OnCurseUnlocked(TarotCards.Card curse) => this.AddOnce(curse);
}
