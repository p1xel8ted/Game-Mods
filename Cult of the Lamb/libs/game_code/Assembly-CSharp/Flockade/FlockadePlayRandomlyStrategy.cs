// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadePlayRandomlyStrategy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Linq;
using UnityEngine;

#nullable disable
namespace Flockade;

public class FlockadePlayRandomlyStrategy(FlockadeGameBoard gameBoard, FlockadeGameBoardSide side) : 
  FlockadeStrategy(gameBoard, side)
{
  public override bool IsApplicable(
    FlockadeGamePieceChoice[] choices,
    ref FlockadeGamePieceChoice selectedChoice,
    ref FlockadeGameBoardTile selectedTile)
  {
    selectedChoice = choices[Random.Range(0, choices.Length)];
    FlockadeGameBoardTile[] array = this._gameBoard.GetAvailableTiles(this._gameBoard.GetPlayer(this._side), selectedChoice.GamePiece.Configuration).ToArray<FlockadeGameBoardTile>();
    selectedTile = array[Random.Range(0, array.Length)];
    Debug.Log((object) $"[NPC AI] Played randomly {selectedChoice.LocalizedIdentifier} on {selectedTile.LocalizedIdentifier}.");
    return true;
  }
}
