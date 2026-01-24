// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeBlessingBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
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
