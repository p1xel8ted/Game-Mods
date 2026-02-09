// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeShiftColumnBlessing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
public class FlockadeShiftColumnBlessing : FlockadeBlessingBase, IFlockadeBlessing
{
  public const float _BETWEEN_GAME_PIECES_APPEARANCE_DISAPPEARANCE_DELAY = 0.06666667f;
  public const string _GOING_DOWN_ACTIVATION_SOUND = "event:/dlc/ui/flockade_minigame/piece_blessing_fallen_activate";
  public const string _GOING_UP_ACTIVATION_SOUND = "event:/dlc/ui/flockade_minigame/piece_blessing_risen_activate";
  [SerializeField]
  public FlockadeShiftColumnBlessing.Direction _direction;

  IFlockadeBlessing.OnDuelPhaseStartedResult IFlockadeBlessing.OnDuelPhaseStarted(
    IFlockadeGameBoardTile target)
  {
    Sequence sequence = (Sequence) null;
    if (!(target is FlockadeGameBoardTile tile1))
    {
      IFlockadeGameBoardTile[] array1 = target.GameBoard.GetColumnOf(target).ToArray<IFlockadeGameBoardTile>();
      IFlockadeGamePiece.State[] array2 = ((IEnumerable<IFlockadeGameBoardTile>) array1).Select<IFlockadeGameBoardTile, IFlockadeGamePiece.State>((Func<IFlockadeGameBoardTile, IFlockadeGamePiece.State>) (tile => tile.GamePiece.Pop())).ToArray<IFlockadeGamePiece.State>();
      for (int index = 0; index < array1.Length; ++index)
      {
        IFlockadeGamePiece.State gamePiece = array2[index];
        int destinationIndex = this.GetDestinationIndex<IFlockadeGameBoardTile>(index, array1);
        IFlockadeGameBoardTile target1 = array1[destinationIndex];
        target1.GamePiece.Set(gamePiece);
        target1.GamePiece.Blessing.OnMoved(target1);
      }
    }
    else
    {
      sequence = DOTween.Sequence();
      FlockadeGameBoardTile[] array = tile1.GameBoard.GetColumnOf(tile1).ToArray<FlockadeGameBoardTile>();
      foreach (FlockadeGameBoardTile target2 in array)
        target2.GamePiece.Blessing.BeforeMoving((IFlockadeGameBoardTile) target2);
      sequence.AppendCallback((TweenCallback) (() => AudioManager.Instance.PlayOneShot(this._direction == FlockadeShiftColumnBlessing.Direction.Up ? "event:/dlc/ui/flockade_minigame/piece_blessing_risen_activate" : "event:/dlc/ui/flockade_minigame/piece_blessing_fallen_activate"))).Append((Tween) tile1.GamePiece.Blessing.Activate(true)).AppendInterval(0.0166666675f);
      IFlockadeGamePiece.State[] stateArray = new IFlockadeGamePiece.State[array.Length];
      Sequence[] sequenceArray = new Sequence[array.Length];
      int origin = array.IndexOf<FlockadeGameBoardTile>(tile1);
      for (int index = 0; index < array.Length; ++index)
      {
        FlockadeGameBoardTile flockadeGameBoardTile = array[index];
        float interval = 0.06666667f * (float) FlockadeUtils.Rank(index, origin, array.Length, this._direction == FlockadeShiftColumnBlessing.Direction.Down);
        sequenceArray[index] = flockadeGameBoardTile.GamePiece.Pop(out stateArray[index]).PrependCallback((TweenCallback) (() => AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/piece_move"))).PrependInterval(interval);
      }
      for (int index = 0; index < array.Length; ++index)
      {
        IFlockadeGamePiece.State gamePiece = stateArray[index];
        int destinationIndex = this.GetDestinationIndex<FlockadeGameBoardTile>(index, array);
        FlockadeGameBoardTile destinationTile = array[destinationIndex];
        sequenceArray[destinationIndex].AppendCallback((TweenCallback) (() => gamePiece.Blessing.OnMoved((IFlockadeGameBoardTile) destinationTile))).Append((Tween) destinationTile.GamePiece.Set(gamePiece, false));
      }
      foreach (Sequence t in sequenceArray)
        sequence.Join((Tween) t);
    }
    return new IFlockadeBlessing.OnDuelPhaseStartedResult(sequence);
  }

  public int GetDestinationIndex<T>(int index, T[] column) where T : IFlockadeGameBoardTile
  {
    return FlockadeUtils.Modulo(index + (this._direction == FlockadeShiftColumnBlessing.Direction.Up ? -1 : 1), column.Length);
  }

  [CompilerGenerated]
  public void \u003CFlockade\u002EIFlockadeBlessing\u002EOnDuelPhaseStarted\u003Eb__5_1()
  {
    AudioManager.Instance.PlayOneShot(this._direction == FlockadeShiftColumnBlessing.Direction.Up ? "event:/dlc/ui/flockade_minigame/piece_blessing_risen_activate" : "event:/dlc/ui/flockade_minigame/piece_blessing_fallen_activate");
  }

  public enum Direction
  {
    Up,
    Down,
  }
}
