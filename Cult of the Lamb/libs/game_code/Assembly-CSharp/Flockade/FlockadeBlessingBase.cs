// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeBlessingBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;

#nullable disable
namespace Flockade;

[Serializable]
public class FlockadeBlessingBase : IFlockadeBlessing
{
  public int tileIndex;

  void IFlockadeBlessing.BeforeMoving(IFlockadeGameBoardTile target)
  {
    this.tileIndex = target.Index;
  }

  void IFlockadeBlessing.OnMoved(IFlockadeGameBoardTile target)
  {
    if (target.Index >= this.tileIndex || !(target is FlockadeGameBoardTile flockadeGameBoardTile) || flockadeGameBoardTile.GamePiece.Blessing.Consumed)
      return;
    target.GamePiece.Blessing.OnDuelPhaseStarted(target);
  }
}
