// Decompiled with JetBrains decompiler
// Type: src.Alerts.FlockadePieceAlerts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
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
