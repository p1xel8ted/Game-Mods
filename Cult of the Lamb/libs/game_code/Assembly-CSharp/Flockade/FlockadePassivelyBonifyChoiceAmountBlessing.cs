// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadePassivelyBonifyChoiceAmountBlessing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System;
using UnityEngine;

#nullable disable
namespace Flockade;

[Serializable]
public class FlockadePassivelyBonifyChoiceAmountBlessing : FlockadeBlessingBase, IFlockadeBlessing
{
  public const string _CHOICE_AMOUNT_MALUS_SOUND = "event:/dlc/ui/flockade_minigame/piece_blessing_loneshepherd_activate";
  [SerializeField]
  public int _bonus;
  [SerializeField]
  public FlockadePassivelyBonifyChoiceAmountBlessing.Side _side;

  IFlockadeBlessing.OnPlacedResult IFlockadeBlessing.OnPlaced(IFlockadeGameBoardTile target)
  {
    FlockadeGameBoardSide side = this._side == FlockadePassivelyBonifyChoiceAmountBlessing.Side.Current ? target.Side : target.Side.GetOpposing();
    target.GameBoard.PassiveManager.ApplyBonusToChoicesAmount(side, this._bonus);
    Sequence sequence = DOTween.Sequence();
    FlockadeGameBoardTile uiTarget = target as FlockadeGameBoardTile;
    if (uiTarget != null)
    {
      uiTarget.GameBoard.PassiveManager.SubscribeToChoiceAmountBonusQueries(side, new Func<Sequence>(uiTarget.GamePiece.Blessing.Activate));
      Sequence t = uiTarget.GameBoard.GetPlayer(side).BindBlessing(uiTarget.GamePiece.Blessing.Get());
      if (t != null)
        sequence.Append((Tween) t);
      if (this._bonus > 0)
        sequence.PrependCallback((TweenCallback) (() => uiTarget.GameBoard.GetPlayer(side).Animate(FlockadePlayerBase.AnimationState.WinPieceOrPoints)));
      else if (this._bonus < 0)
        sequence.PrependCallback((TweenCallback) (() =>
        {
          uiTarget.GameBoard.GetPlayer(side).Animate(FlockadePlayerBase.AnimationState.LosePieceOrPoints);
          AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/piece_blessing_loneshepherd_activate");
        }));
    }
    return new IFlockadeBlessing.OnPlacedResult(sequence);
  }

  IFlockadeBlessing.OnRemovedResult IFlockadeBlessing.OnRemoved(
    IFlockadeGamePiece target,
    FlockadeGameBoardSide side,
    IFlockadeGameBoard gameBoard)
  {
    FlockadeGameBoardSide side1 = this._side == FlockadePassivelyBonifyChoiceAmountBlessing.Side.Current ? side : side.GetOpposing();
    gameBoard.PassiveManager.ApplyBonusToChoicesAmount(side1, -this._bonus);
    Sequence sequence = (Sequence) null;
    if (target is FlockadeGamePiece flockadeGamePiece)
    {
      gameBoard.PassiveManager.UnsubscribeFromChoiceAmountBonusQueries(side1, new Func<Sequence>(flockadeGamePiece.Blessing.Activate));
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
