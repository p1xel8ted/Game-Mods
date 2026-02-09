// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeBlessingBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
