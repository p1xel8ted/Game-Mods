// Decompiled with JetBrains decompiler
// Type: src.Alerts.FlockadePieceAlerts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Flockade;
using MessagePack;
using System;

#nullable disable
namespace src.Alerts;

[MessagePackObject(false)]
[Serializable]
public class FlockadePieceAlerts : AlertCategory<FlockadePieceType>
{
  public FlockadePieceAlerts()
  {
    FlockadePieceManager.OnFlockadePieceUnlocked += new FlockadePieceManager.FlockadePieceUpdated(this.OnFlockadePieceUnlocked);
  }

  void object.Finalize()
  {
    try
    {
      FlockadePieceManager.OnFlockadePieceUnlocked -= new FlockadePieceManager.FlockadePieceUpdated(this.OnFlockadePieceUnlocked);
    }
    finally
    {
      // ISSUE: explicit finalizer call
      base.Finalize();
    }
  }

  public void OnFlockadePieceUnlocked(FlockadePieceType piece) => this.AddOnce(piece);
}
