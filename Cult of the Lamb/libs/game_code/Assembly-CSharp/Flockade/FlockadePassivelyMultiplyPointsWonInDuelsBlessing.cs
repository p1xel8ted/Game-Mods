// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadePassivelyMultiplyPointsWonInDuelsBlessing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System;
using UnityEngine;

#nullable disable
namespace Flockade;

[Serializable]
public class FlockadePassivelyMultiplyPointsWonInDuelsBlessing : 
  FlockadeBlessingBase,
  IFlockadeBlessing
{
  public const string _ACTIVATION_SOUND = "event:/dlc/ui/flockade_minigame/piece_blessing_goldenshepherd_activate";
  [SerializeField]
  public float _multiplier = 1f;
  [SerializeField]
  public FlockadePassivelyMultiplyPointsWonInDuelsBlessing.Side _side;

  IFlockadeBlessing.OnPlacedResult IFlockadeBlessing.OnPlaced(IFlockadeGameBoardTile target)
  {
    FlockadeGameBoardSide side = this._side == FlockadePassivelyMultiplyPointsWonInDuelsBlessing.Side.Current ? target.Side : target.Side.GetOpposing();
    target.GameBoard.PassiveManager.ApplyMultiplierToPointsWonInDuel(side, this._multiplier);
    Sequence sequence = (Sequence) null;
    if (target is FlockadeGameBoardTile flockadeGameBoardTile)
    {
      flockadeGameBoardTile.GameBoard.PassiveManager.SubscribeToPointsWonInDuelMultiplierQueries(side, new Func<Sequence>(flockadeGameBoardTile.GamePiece.Blessing.Activate));
      sequence = DOTween.Sequence().AppendCallback((TweenCallback) (() => AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/piece_blessing_goldenshepherd_activate"))).Append((Tween) flockadeGameBoardTile.GameBoard.GetPlayer(side).BindBlessing(flockadeGameBoardTile.GamePiece.Blessing.Get()));
    }
    return new IFlockadeBlessing.OnPlacedResult(sequence);
  }

  IFlockadeBlessing.OnRemovedResult IFlockadeBlessing.OnRemoved(
    IFlockadeGamePiece target,
    FlockadeGameBoardSide side,
    IFlockadeGameBoard gameBoard)
  {
    FlockadeGameBoardSide side1 = this._side == FlockadePassivelyMultiplyPointsWonInDuelsBlessing.Side.Current ? side : side.GetOpposing();
    gameBoard.PassiveManager.ApplyMultiplierToPointsWonInDuel(side1, 1f / this._multiplier);
    Sequence sequence = (Sequence) null;
    if (target is FlockadeGamePiece flockadeGamePiece)
    {
      gameBoard.PassiveManager.UnsubscribeFromPointsWonInDuelMultiplierQueries(side1, new Func<Sequence>(flockadeGamePiece.Blessing.Activate));
      if (gameBoard.GetPlayer(side1) is FlockadePlayerBase player)
        sequence = player.UnbindBlessing(flockadeGamePiece.Blessing.Get());
    }
    return new IFlockadeBlessing.OnRemovedResult(sequence);
  }

  public enum Side
  {
    Current,
    Opposing,
  }
}
