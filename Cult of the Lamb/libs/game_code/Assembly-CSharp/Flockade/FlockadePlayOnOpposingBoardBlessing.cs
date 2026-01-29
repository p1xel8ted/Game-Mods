// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadePlayOnOpposingBoardBlessing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace Flockade;

[Serializable]
public class FlockadePlayOnOpposingBoardBlessing : FlockadeBlessingBase, IFlockadeBlessing
{
  [SerializeField]
  public FlockadePlayOnOpposingBoardBlessing.Mode _mode;
  public bool _isPlacingOnSelf;

  IFlockadeBlessing.BeforePlacingResult IFlockadeBlessing.BeforePlacing(
    IFlockadeGameBoard gameBoard,
    FlockadeGameBoardSide side)
  {
    FlockadeGameBoardSide opposing = side.GetOpposing();
    IFlockadeGameBoardTile[] array = gameBoard.GetTiles(opposing).ToArray<IFlockadeGameBoardTile>();
    if (((IEnumerable<IFlockadeGameBoardTile>) array).All<IFlockadeGameBoardTile>((Func<IFlockadeGameBoardTile, bool>) (tile => this._mode != FlockadePlayOnOpposingBoardBlessing.Mode.PlayOnEmptyTiles ? !tile.Locked : tile.Locked)))
    {
      this._isPlacingOnSelf = true;
      return new IFlockadeBlessing.BeforePlacingResult();
    }
    this._isPlacingOnSelf = false;
    return new IFlockadeBlessing.BeforePlacingResult(((IEnumerable<IFlockadeGameBoardTile>) array).Where<IFlockadeGameBoardTile>((Func<IFlockadeGameBoardTile, bool>) (tile => this._mode != FlockadePlayOnOpposingBoardBlessing.Mode.PlayOnEmptyTiles ? tile.Locked : !tile.Locked)).ToArray<IFlockadeGameBoardTile>());
  }

  IFlockadeBlessing.OnPlacedResult IFlockadeBlessing.OnPlaced(IFlockadeGameBoardTile target)
  {
    Sequence sequence = DOTween.Sequence();
    FlockadeGameBoardTile uiTarget = target as FlockadeGameBoardTile;
    if (uiTarget != null)
    {
      if (!this._isPlacingOnSelf)
        sequence.AppendCallback((TweenCallback) (() =>
        {
          AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/piece_transform");
          uiTarget.GameBoard.GetPlayer(uiTarget.Side).Animate(FlockadePlayerBase.AnimationState.LosePieceOrPoints);
        }));
      sequence.Append((Tween) uiTarget.GamePiece.Blessing.Activate(true));
    }
    return new IFlockadeBlessing.OnPlacedResult(sequence);
  }

  [CompilerGenerated]
  public bool \u003CFlockade\u002EIFlockadeBlessing\u002EBeforePlacing\u003Eb__3_0(
    IFlockadeGameBoardTile tile)
  {
    return this._mode != FlockadePlayOnOpposingBoardBlessing.Mode.PlayOnEmptyTiles ? !tile.Locked : tile.Locked;
  }

  [CompilerGenerated]
  public bool \u003CFlockade\u002EIFlockadeBlessing\u002EBeforePlacing\u003Eb__3_1(
    IFlockadeGameBoardTile tile)
  {
    return this._mode != FlockadePlayOnOpposingBoardBlessing.Mode.PlayOnEmptyTiles ? tile.Locked : !tile.Locked;
  }

  public enum Mode
  {
    PlayOnEmptyTiles,
    ReplaceExistingPiecesOnly,
  }
}
