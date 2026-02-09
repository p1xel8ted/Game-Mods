// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeStrategy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;

#nullable disable
namespace Flockade;

public abstract class FlockadeStrategy
{
  public FlockadeGameBoard _gameBoard;
  public FlockadeGameBoardSide _side;
  public FlockadeGamePieceChoice _selectedChoice;
  public FlockadeGameBoardTile _selectedTile;

  public FlockadeStrategy(FlockadeGameBoard gameBoard, FlockadeGameBoardSide side)
  {
    this._gameBoard = gameBoard;
    this._side = side;
  }

  public bool IsApplicable(FlockadeGamePieceChoice[] choices)
  {
    return this.IsApplicable(choices, ref this._selectedChoice, ref this._selectedTile);
  }

  public FlockadeGamePieceChoice Select(FlockadeGamePieceChoice[] choices)
  {
    return choices.Contains<FlockadeGamePieceChoice>(this._selectedChoice) ? this._selectedChoice : throw new Exception($"Invalid choice made by {this.GetType().Name}. Please review its algorithm!");
  }

  public FlockadeGameBoardTile Place(FlockadeGameBoardTile[] availableTiles)
  {
    return availableTiles.Contains<FlockadeGameBoardTile>(this._selectedTile) ? this._selectedTile : throw new Exception($"Invalid placement made by {this.GetType().Name}. Please review its algorithm!");
  }

  public abstract bool IsApplicable(
    FlockadeGamePieceChoice[] choices,
    ref FlockadeGamePieceChoice selectedChoice,
    ref FlockadeGameBoardTile selectedTile);
}
