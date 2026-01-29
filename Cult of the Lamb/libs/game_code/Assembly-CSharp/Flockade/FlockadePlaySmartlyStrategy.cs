// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadePlaySmartlyStrategy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
namespace Flockade;

public class FlockadePlaySmartlyStrategy(FlockadeGameBoard gameBoard, FlockadeGameBoardSide side) : 
  FlockadeStrategy(gameBoard, side)
{
  public static FlockadePieceType[] _RANDOM_GAME_PIECES = new FlockadePieceType[3]
  {
    FlockadePieceType.SwordTrickster,
    FlockadePieceType.ShieldTrickster,
    FlockadePieceType.ScribeTrickster
  };
  public static FlockadePieceType[] _MOVEMENT_GAME_PIECES = new FlockadePieceType[9]
  {
    FlockadePieceType.SwordMercenary,
    FlockadePieceType.ShieldMercenary,
    FlockadePieceType.ScribeMercenary,
    FlockadePieceType.SwordRisen,
    FlockadePieceType.ShieldRisen,
    FlockadePieceType.ScribeRisen,
    FlockadePieceType.SwordFallen,
    FlockadePieceType.ShieldFallen,
    FlockadePieceType.ScribeFallen
  };
  public static FlockadePieceType[] _SCOUT_GAME_PIECES = new FlockadePieceType[3]
  {
    FlockadePieceType.SwordScout,
    FlockadePieceType.ShieldScout,
    FlockadePieceType.ScribeScout
  };

  public override bool IsApplicable(
    FlockadeGamePieceChoice[] choices,
    ref FlockadeGamePieceChoice selectedChoice,
    ref FlockadeGameBoardTile selectedTile)
  {
    FlockadeGameBoardSide opposing = this._side.GetOpposing();
    int num1 = this._gameBoard.GetPlayer(this._side).Victories.Count - this._gameBoard.GetPlayer(opposing).Victories.Count;
    FlockadeVirtualGameBoard virtualGameBoard1 = this._gameBoard.Core.Clone();
    virtualGameBoard1.StartDuelPhase();
    virtualGameBoard1.Resolve();
    int count = virtualGameBoard1.GetPlayer(this._side).Points.Count;
    int num2 = count - virtualGameBoard1.GetPlayer(opposing).Points.Count;
    int key1 = num2;
    Dictionary<int, List<(FlockadeGamePieceChoice, FlockadeGameBoardTile)>> source1 = new Dictionary<int, List<(FlockadeGamePieceChoice, FlockadeGameBoardTile)>>();
    List<(FlockadeGamePieceChoice, FlockadeGameBoardTile)> valueTupleList1 = new List<(FlockadeGamePieceChoice, FlockadeGameBoardTile)>();
    FlockadeGamePieceChoice[] flockadeGamePieceChoiceArray = choices;
    int index1;
    for (index1 = 0; index1 < flockadeGamePieceChoiceArray.Length; ++index1)
    {
      FlockadeGamePieceChoice flockadeGamePieceChoice = flockadeGamePieceChoiceArray[index1];
      foreach (FlockadeGameBoardTile availableTile in this._gameBoard.GetAvailableTiles(this._gameBoard.GetPlayer(this._side), flockadeGamePieceChoice.GamePiece.Configuration))
      {
        FlockadeVirtualGameBoard virtualGameBoard2 = this._gameBoard.Core.Clone();
        IFlockadeGameBoardTile tile1 = virtualGameBoard2.GetTile(availableTile.Side, availableTile.Index);
        IFlockadeGamePiece virtualGamePiece = tile1.GamePiece;
        tile1.GamePiece.Set(flockadeGamePieceChoice.GamePiece.Configuration);
        tile1.Place();
        virtualGameBoard2.StartDuelPhase();
        IFlockadeGameBoardTile tile2 = virtualGameBoard2.GetTiles(tile1.Side).First<IFlockadeGameBoardTile>((Func<IFlockadeGameBoardTile, bool>) (tile => tile.GamePiece.Blessing == virtualGamePiece.Blessing));
        if (!(bool) (UnityEngine.Object) virtualGameBoard2.GetOpposingTile(tile2).GamePiece.Configuration)
          valueTupleList1.Add((flockadeGamePieceChoice, availableTile));
        virtualGameBoard2.Resolve();
        int key2 = virtualGameBoard2.GetPlayer(this._side).Points.Count - virtualGameBoard2.GetPlayer(opposing).Points.Count;
        int num3 = virtualGameBoard2.GetPlayer(this._side).Victories.Count - virtualGameBoard2.GetPlayer(opposing).Victories.Count;
        if (key2 <= 0 && num3 > num1)
          key2 = int.MaxValue;
        else if (key2 >= 0 && num3 < num1)
          key2 = int.MinValue;
        if (key2 > key1)
          key1 = key2;
        source1.TryAdd(key2, new List<(FlockadeGamePieceChoice, FlockadeGameBoardTile)>());
        source1[key2].Add((flockadeGamePieceChoice, availableTile));
      }
    }
    if (key1 > num2)
    {
      if (source1[key1].Any<(FlockadeGamePieceChoice, FlockadeGameBoardTile)>((Func<(FlockadeGamePieceChoice, FlockadeGameBoardTile), bool>) (move => !FlockadePlaySmartlyStrategy._RANDOM_GAME_PIECES.Contains<FlockadePieceType>(move.Choice.GamePiece.Configuration.Type))))
        source1[key1].RemoveAll((Predicate<(FlockadeGamePieceChoice, FlockadeGameBoardTile)>) (move => FlockadePlaySmartlyStrategy._RANDOM_GAME_PIECES.Contains<FlockadePieceType>(move.Choice.GamePiece.Configuration.Type)));
      int bestMoveIndex = UnityEngine.Random.Range(0, source1[key1].Count);
      (selectedChoice, selectedTile) = source1[key1][bestMoveIndex];
      Debug.Log((object) $"[NPC AI] Played one of the best moves improving relative score by {key1 - num2} amongst {source1[key1].Count} possible move(s).\r\nPlayed: {selectedChoice.LocalizedIdentifier} on {selectedTile.LocalizedIdentifier}.\r\nOther possible moves: {string.Join<string>('\n', source1[key1].Where<(FlockadeGamePieceChoice, FlockadeGameBoardTile)>((Func<(FlockadeGamePieceChoice, FlockadeGameBoardTile), int, bool>) ((_, index) => index != bestMoveIndex)).Select<(FlockadeGamePieceChoice, FlockadeGameBoardTile), string>((Func<(FlockadeGamePieceChoice, FlockadeGameBoardTile), string>) (move => $"{move.Choice.LocalizedIdentifier} on {move.Tile.LocalizedIdentifier}.")))}");
      return true;
    }
    int num4 = 0;
    List<(FlockadeGamePieceChoice, FlockadeGameBoardTile)> source2 = new List<(FlockadeGamePieceChoice, FlockadeGameBoardTile)>();
    if (valueTupleList1.Count > 0)
    {
      foreach ((FlockadeGamePieceChoice flockadeGamePieceChoice, FlockadeGameBoardTile tile) in valueTupleList1)
      {
        int potential = this.GetPotential(flockadeGamePieceChoice.GamePiece.Configuration, tile, count);
        if (potential >= num4)
        {
          if (potential > num4)
          {
            num4 = potential;
            source2.Clear();
          }
          source2.Add((flockadeGamePieceChoice, tile));
        }
      }
      if (num4 > 0)
      {
        int bestMoveIndex = UnityEngine.Random.Range(0, source2.Count);
        (selectedChoice, selectedTile) = source2[bestMoveIndex];
        Debug.Log((object) $"[NPC AI] Played (or screwed) one of the highest potential unopposed game piece with a potential rating of {num4} amongst {source2.Count} possible move(s).\r\nPlayed: {selectedChoice.LocalizedIdentifier} on {selectedTile.LocalizedIdentifier}.\r\nOther possible moves: {string.Join<string>('\n', source2.Where<(FlockadeGamePieceChoice, FlockadeGameBoardTile)>((Func<(FlockadeGamePieceChoice, FlockadeGameBoardTile), int, bool>) ((_, index) => index != bestMoveIndex)).Select<(FlockadeGamePieceChoice, FlockadeGameBoardTile), string>((Func<(FlockadeGamePieceChoice, FlockadeGameBoardTile), string>) (move => $"{move.Choice.LocalizedIdentifier} on {move.Tile.LocalizedIdentifier}.")))}");
        return true;
      }
    }
    List<(FlockadeGamePieceChoice, FlockadeGameBoardTile)> valueTupleList2;
    source1.OrderByDescending<KeyValuePair<int, List<(FlockadeGamePieceChoice, FlockadeGameBoardTile)>>, int>((Func<KeyValuePair<int, List<(FlockadeGamePieceChoice, FlockadeGameBoardTile)>>, int>) (pair => pair.Key)).FirstOrDefault<KeyValuePair<int, List<(FlockadeGamePieceChoice, FlockadeGameBoardTile)>>>().Deconstruct(ref index1, ref valueTupleList2);
    int num5 = index1;
    List<(FlockadeGamePieceChoice, FlockadeGameBoardTile)> source3 = valueTupleList2;
    if (source3 == null)
      return false;
    int bestMoveIndex1 = UnityEngine.Random.Range(0, source3.Count);
    (selectedChoice, selectedTile) = source3[bestMoveIndex1];
    Debug.Log((object) $"[NPC AI] Played one of the moves that change nothing or minimise lost in terms of relative score by {num5 - num2} amongst {source3.Count} possible move(s).\r\nPlayed: {selectedChoice.LocalizedIdentifier} on {selectedTile.LocalizedIdentifier}.\r\nOther possible moves: {string.Join<string>('\n', source3.Where<(FlockadeGamePieceChoice, FlockadeGameBoardTile)>((Func<(FlockadeGamePieceChoice, FlockadeGameBoardTile), int, bool>) ((_, index) => index != bestMoveIndex1)).Select<(FlockadeGamePieceChoice, FlockadeGameBoardTile), string>((Func<(FlockadeGamePieceChoice, FlockadeGameBoardTile), string>) (move => $"{move.Choice.LocalizedIdentifier} on {move.Tile.LocalizedIdentifier}.")))}");
    return true;
  }

  public int GetPotential(
    FlockadeGamePieceConfiguration gamePiece,
    FlockadeGameBoardTile tile1,
    int currentPointScore)
  {
    if (!(bool) (UnityEngine.Object) gamePiece)
      return 0;
    switch (gamePiece.Type)
    {
      case FlockadePieceType.SwordJust:
      case FlockadePieceType.ShieldJust:
      case FlockadePieceType.ScribeJust:
        return 1;
      case FlockadePieceType.SwordPrize:
      case FlockadePieceType.ShieldPrize:
      case FlockadePieceType.ScribePrize:
        return (double) this._gameBoard.PassiveManager.GetPointsWonInDuelMultiplier(tile1.Side) <= 1.0 ? 2 : 4;
      case FlockadePieceType.SwordScout:
      case FlockadePieceType.ShieldScout:
      case FlockadePieceType.ScribeScout:
        FlockadeGamePieceConfiguration configuration = this._gameBoard.GetTile(tile1.Side, tile1.Index).GamePiece.Configuration;
        return (!(bool) (UnityEngine.Object) configuration ? 0 : (FlockadePlaySmartlyStrategy._SCOUT_GAME_PIECES.Contains<FlockadePieceType>(configuration.Type) ? 1 : 0)) != 0 ? 0 : this.GetPotential(configuration, tile1, currentPointScore);
      case FlockadePieceType.SwordMarked:
      case FlockadePieceType.ShieldMarked:
      case FlockadePieceType.ScribeMarked:
        int tileCount1 = this._gameBoard.GetTileCount(tile1.Side);
        return (this._gameBoard.PassiveManager.IsOrderOfDuelsAndBlessingsApplicationReversed() ? tileCount1 - 1 - tile1.Index : tile1.Index) < tileCount1 / 2 ? 0 : 2;
      case FlockadePieceType.ShepherdFalse:
        return (double) this._gameBoard.PassiveManager.GetPointsWonInDuelMultiplier(tile1.Side) <= 1.0 ? 2 : 4;
      case FlockadePieceType.ShepherdGolden:
        return currentPointScore / 2 + (this._gameBoard.GetTiles(tile1.Side).Count<FlockadeGameBoardTile>((Func<FlockadeGameBoardTile, bool>) (tile2 => !tile2.Locked)) - 1);
      case FlockadePieceType.ShepherdLone:
        FlockadeGameBoardSide opposing = tile1.Side.GetOpposing();
        int tileCount2 = this._gameBoard.GetTileCount(opposing);
        return this._gameBoard.GetTiles(opposing).Count<FlockadeGameBoardTile>((Func<FlockadeGameBoardTile, bool>) (tile3 => !tile3.Locked)) < tileCount2 ? 0 : 3;
      case FlockadePieceType.ShepherdWandering:
        return 2 * (this._gameBoard.GetRowOf(tile1).Except<FlockadeGameBoardTile>((IEnumerable<FlockadeGameBoardTile>) new FlockadeGameBoardTile[1]
        {
          tile1
        }).Concat<FlockadeGameBoardTile>(this._gameBoard.GetColumnOf(tile1).Except<FlockadeGameBoardTile>((IEnumerable<FlockadeGameBoardTile>) new FlockadeGameBoardTile[1]
        {
          tile1
        })).Count<FlockadeGameBoardTile>((Func<FlockadeGameBoardTile, bool>) (tile4 => !tile4.Locked || FlockadePlaySmartlyStrategy._MOVEMENT_GAME_PIECES.Contains<FlockadePieceType>(tile4.GamePiece.Configuration.Type))) - 1);
      default:
        return 0;
    }
  }
}
