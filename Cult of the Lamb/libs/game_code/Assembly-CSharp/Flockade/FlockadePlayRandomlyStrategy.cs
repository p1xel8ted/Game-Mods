// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadePlayRandomlyStrategy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
