// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeNullifyOpposingBlessingsBlessing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System;
using System.Linq;
using UnityEngine;

#nullable disable
namespace Flockade;

[Serializable]
public class FlockadeNullifyOpposingBlessingsBlessing : FlockadeBlessingBase, IFlockadeBlessing
{
  public const string _ACTIVATION_SOUND = "event:/dlc/ui/flockade_minigame/piece_blessing_rotten_activate";
  public const float _BETWEEN_COLUMNS_NULLIFICATION_DELAY = 0.333333343f;
  [SerializeField]
  public FlockadeNullifyOpposingBlessingsBlessing.Target _target;

  IFlockadeBlessing.OnPlacedResult IFlockadeBlessing.OnPlaced(IFlockadeGameBoardTile target)
  {
    IFlockadeGameBoardTile[] flockadeGameBoardTileArray;
    switch (this._target)
    {
      case FlockadeNullifyOpposingBlessingsBlessing.Target.OpposingTile:
        flockadeGameBoardTileArray = new IFlockadeGameBoardTile[1]
        {
          target.GameBoard.GetOpposingTile(target)
        };
        break;
      case FlockadeNullifyOpposingBlessingsBlessing.Target.OpposingRow:
        flockadeGameBoardTileArray = target.GameBoard.GetRowOf(target.GameBoard.GetOpposingTile(target)).ToArray<IFlockadeGameBoardTile>();
        break;
      case FlockadeNullifyOpposingBlessingsBlessing.Target.OpposingColumn:
        flockadeGameBoardTileArray = target.GameBoard.GetColumnOf(target.GameBoard.GetOpposingTile(target)).ToArray<IFlockadeGameBoardTile>();
        break;
      case FlockadeNullifyOpposingBlessingsBlessing.Target.OpposingBoard:
        flockadeGameBoardTileArray = target.GameBoard.GetTiles(target.Side.GetOpposing()).ToArray<IFlockadeGameBoardTile>();
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
    IFlockadeGameBoardTile[] source = flockadeGameBoardTileArray;
    Sequence sequence = (Sequence) null;
    FlockadeGameBoardTile uiTarget = target as FlockadeGameBoardTile;
    if (uiTarget != null)
    {
      FlockadeGameBoardTile[] array = Enumerable.OfType<FlockadeGameBoardTile>(source).ToArray<FlockadeGameBoardTile>();
      sequence = DOTween.Sequence().AppendCallback((TweenCallback) (() => AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/piece_blessing_rotten_activate"))).Append((Tween) uiTarget.GamePiece.Blessing.Activate(true));
      int num1 = 0;
      bool flag = false;
      for (int index = 0; index < array.Length; ++index)
      {
        int num2;
        switch (this._target)
        {
          case FlockadeNullifyOpposingBlessingsBlessing.Target.OpposingRow:
            num2 = index;
            break;
          case FlockadeNullifyOpposingBlessingsBlessing.Target.OpposingBoard:
            num2 = index / uiTarget.GameBoard.RowCount;
            break;
          default:
            num2 = num1;
            break;
        }
        num1 = num2;
        flag = ((flag ? 1 : 0) | (!array[index].GamePiece.Blessing.Active ? 0 : (!array[index].GamePiece.Blessing.Consumed ? 1 : 0))) != 0;
        Sequence t = array[index].GamePiece.Blessing.Nullify();
        if (t != null)
          sequence.Insert((float) num1 * 0.333333343f, (Tween) t);
      }
      if (flag)
        sequence.PrependCallback((TweenCallback) (() => uiTarget.GameBoard.GetPlayer(uiTarget.Side.GetOpposing()).Animate(FlockadePlayerBase.AnimationState.LosePieceOrPoints)));
    }
    else
    {
      foreach (IFlockadeGameBoardTile flockadeGameBoardTile in source)
        flockadeGameBoardTile.GamePiece.Blessing.Nullify();
    }
    return new IFlockadeBlessing.OnPlacedResult(sequence);
  }

  public enum Target
  {
    OpposingTile,
    OpposingRow,
    OpposingColumn,
    OpposingBoard,
  }
}
