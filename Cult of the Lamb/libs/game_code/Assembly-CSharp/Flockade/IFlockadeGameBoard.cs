// Decompiled with JetBrains decompiler
// Type: Flockade.IFlockadeGameBoard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Flockade;

public interface IFlockadeGameBoard
{
  bool AllTilesAreLocked => this.Core.AllTilesAreLocked;

  FlockadeVirtualGameBoard Core { get; }

  FlockadePassiveManager PassiveManager => this.Core.PassiveManager;

  int RowCount => this.Core.RowCount;

  IFlockadePlayer GetPlayer(FlockadeGameBoardSide side) => this.Core.GetPlayer(side);

  IEnumerable<IFlockadePlayer> GetPlayers() => this.Core.GetPlayers();

  IEnumerable<IFlockadeGameBoardTile> GetColumnOf(IFlockadeGameBoardTile tile)
  {
    return this.Core.GetColumnOf(tile);
  }

  IEnumerable<IFlockadeGameBoardTile> GetRowOf(IFlockadeGameBoardTile tile)
  {
    return this.Core.GetRowOf(tile);
  }

  int GetTileCount(FlockadeGameBoardSide side) => this.Core.GetTileCount(side);

  IEnumerable<IFlockadeGameBoardTile> GetTiles(FlockadeGameBoardSide side)
  {
    return this.Core.GetTiles(side);
  }

  IEnumerable<IFlockadeGameBoardTile> GetTiles() => this.Core.GetTiles();

  IFlockadeGameBoardTile GetTile(FlockadeGameBoardSide side, int index)
  {
    return this.Core.GetTile(side, index);
  }

  IFlockadeGameBoardTile GetOpposingTile(IFlockadeGameBoardTile tile)
  {
    return this.Core.GetOpposingTile(tile);
  }

  IEnumerable<IFlockadeGameBoardTile> GetAvailableTiles(
    IFlockadePlayer player,
    FlockadeGamePieceConfiguration gamePiece)
  {
    return this.Core.GetAvailableTiles(player, gamePiece);
  }

  IEnumerator StartDuelPhase()
  {
    return this.Core.StartDuelPhase((Func<FlockadeVirtualGameBoard.BeforeApplyingBlessingsContext, IEnumerator>) null, (Func<FlockadeVirtualGameBoard.OnApplyingBlessingContext, IEnumerator>) null);
  }

  IEnumerator Resolve()
  {
    return this.Core.Resolve((Action<FlockadeVirtualGameBoard.BeforeDuelDefaultScoringContext>) null, (Func<FlockadeVirtualGameBoard.OnDuelResolvedContext, IEnumerator>) null, (Func<IEnumerator>) null);
  }
}
