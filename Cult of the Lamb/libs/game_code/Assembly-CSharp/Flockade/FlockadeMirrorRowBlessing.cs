// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeMirrorRowBlessing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System;
using System.Linq;
using UnityEngine;

#nullable disable
namespace Flockade;

[Serializable]
public class FlockadeMirrorRowBlessing : FlockadeBlessingBase, IFlockadeBlessing
{
  public const string _ACTIVATION_SOUND = "event:/dlc/ui/flockade_minigame/piece_blessing_mercenary_activate";
  public const float _BETWEEN_GAME_PIECE_PAIRS_APPEARANCE_DISAPPEARANCE_DELAY = 0.06666667f;

  IFlockadeBlessing.OnDuelPhaseStartedResult IFlockadeBlessing.OnDuelPhaseStarted(
    IFlockadeGameBoardTile target)
  {
    Sequence sequence = (Sequence) null;
    if (!(target is FlockadeGameBoardTile tile))
    {
      IFlockadeGameBoardTile[] array = target.GameBoard.GetRowOf(target).ToArray<IFlockadeGameBoardTile>();
      for (int index = 0; index < array.Length / 2; ++index)
      {
        IFlockadeGameBoardTile target1 = array[array.Length - 1 - index];
        IFlockadeGameBoardTile target2 = array[index];
        IFlockadeGamePiece.State gamePiece1 = target1.GamePiece.Pop();
        IFlockadeGamePiece.State gamePiece2 = target2.GamePiece.Pop();
        target1.GamePiece.Set(gamePiece2);
        target2.GamePiece.Set(gamePiece1);
        target1.GamePiece.Blessing.OnMoved(target1);
        target2.GamePiece.Blessing.OnMoved(target2);
      }
    }
    else
    {
      sequence = DOTween.Sequence();
      FlockadeGameBoardTile[] array = tile.GameBoard.GetRowOf(tile).ToArray<FlockadeGameBoardTile>();
      foreach (FlockadeGameBoardTile target3 in array)
        target3.GamePiece.Blessing.BeforeMoving((IFlockadeGameBoardTile) target3);
      sequence.AppendCallback((TweenCallback) (() => AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/piece_blessing_mercenary_activate"))).Append((Tween) tile.GamePiece.Blessing.Activate(true)).AppendInterval(0.0166666675f);
      int origin = array.IndexOf<FlockadeGameBoardTile>(tile);
      for (int index = 0; index < array.Length / 2; ++index)
      {
        FlockadeGameBoardTile leftTile = array[array.Length - 1 - index];
        FlockadeGameBoardTile rightTile = array[index];
        float interval = 0.06666667f * (float) this.GetMirroredPairwiseRank(index, origin, array.Length);
        IFlockadeGamePiece.State gamePiece3;
        Sequence s1 = leftTile.GamePiece.Pop(out gamePiece3);
        IFlockadeGamePiece.State gamePiece4;
        Sequence s2 = rightTile.GamePiece.Pop(out gamePiece4);
        sequence.Join((Tween) s1.PrependCallback((TweenCallback) (() => AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/piece_move"))).PrependInterval(interval).AppendCallback((TweenCallback) (() => leftTile.GamePiece.Blessing.OnMoved((IFlockadeGameBoardTile) leftTile))).Append((Tween) leftTile.GamePiece.Set(gamePiece4, false))).Join((Tween) s2.PrependCallback((TweenCallback) (() => AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/piece_move"))).PrependInterval(interval).AppendCallback((TweenCallback) (() => rightTile.GamePiece.Blessing.OnMoved((IFlockadeGameBoardTile) rightTile))).Append((Tween) rightTile.GamePiece.Set(gamePiece3, false)));
      }
    }
    return new IFlockadeBlessing.OnDuelPhaseStartedResult(sequence);
  }

  public int GetMirroredPairwiseRank(int index, int origin, int length)
  {
    int a = Mathf.Min(FlockadeUtils.Rank(index, origin, length), FlockadeUtils.Rank(index, origin, length, false));
    return Mathf.Min(a, length - 1 - a);
  }
}
