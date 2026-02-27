// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeScoreMultiplePointsBlessing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System;
using UnityEngine;

#nullable disable
namespace Flockade;

[Serializable]
public class FlockadeScoreMultiplePointsBlessing : FlockadeBlessingBase, IFlockadeBlessing
{
  public const string _WINNING_ACTIVATION_SOUND = "event:/dlc/ui/flockade_minigame/piece_blessing_prize_activate";
  public const string _LOSING_ACTIVATION_SOUND = "event:/dlc/ui/flockade_minigame/piece_blessing_falseshepherd_activate";
  public const string _POINTS_SCORED_WHILE_LOSING_SOUND = "event:/dlc/ui/flockade_minigame/piece_blessing_falseshepherd_duel_outcome";
  [SerializeField]
  public FlockadeScoreMultiplePointsBlessing.Condition _condition;
  [SerializeField]
  public int _points = 1;

  IFlockadeBlessing.OnResolvedResult IFlockadeBlessing.OnResolved(
    IFlockadeGameBoardTile target,
    IFlockadeGameBoardTile opposing)
  {
    Sequence sequence = (Sequence) null;
    if (target is FlockadeGameBoardTile flockadeGameBoardTile)
    {
      FlockadeFight.Result result = target.GamePiece.Fight(opposing.GamePiece);
      if (this._condition == FlockadeScoreMultiplePointsBlessing.Condition.Won && result == FlockadeFight.Result.Win)
        sequence = DOTween.Sequence().AppendCallback((TweenCallback) (() => AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/piece_blessing_prize_activate"))).Append((Tween) flockadeGameBoardTile.GamePiece.Blessing.Activate(true));
      if (this._condition == FlockadeScoreMultiplePointsBlessing.Condition.Lost && result == FlockadeFight.Result.Defeat)
        sequence = DOTween.Sequence().AppendCallback((TweenCallback) (() => AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/piece_blessing_falseshepherd_activate"))).Append((Tween) flockadeGameBoardTile.GamePiece.Blessing.Activate(true));
    }
    return new IFlockadeBlessing.OnResolvedResult(sequence: sequence);
  }

  IFlockadeBlessing.OnDuelWonResult IFlockadeBlessing.OnDuelWon(IFlockadeGameBoardTile target)
  {
    if (this._condition != FlockadeScoreMultiplePointsBlessing.Condition.Won)
      return new IFlockadeBlessing.OnDuelWonResult();
    Sequence sequence = (Sequence) null;
    float result = 1f;
    if ((double) this._points > 0.0)
      sequence = target.GameBoard.PassiveManager.GetPointsWonInDuelMultiplier(target.Side, out result);
    int num = Mathf.RoundToInt((float) this._points * result);
    if (num > 1 && target is FlockadeGameBoardTile flockadeGameBoardTile)
      flockadeGameBoardTile.GameBoard.GetPlayer(target.Side).Points.SetNextAsBonus(1, num - 1);
    target.GameBoard.GetPlayer(target.Side).Points.Count += num;
    return new IFlockadeBlessing.OnDuelWonResult(true, sequence);
  }

  IFlockadeBlessing.OnDuelLostResult IFlockadeBlessing.OnDuelLost(IFlockadeGameBoardTile target)
  {
    if (this._condition != FlockadeScoreMultiplePointsBlessing.Condition.Lost)
      return new IFlockadeBlessing.OnDuelLostResult();
    Sequence sequence = (Sequence) null;
    float result = 1f;
    if ((double) this._points > 0.0)
    {
      sequence = DOTween.Sequence().AppendCallback((TweenCallback) (() => AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/piece_blessing_falseshepherd_duel_outcome")));
      Sequence inDuelMultiplier = target.GameBoard.PassiveManager.GetPointsWonInDuelMultiplier(target.Side, out result);
      if (inDuelMultiplier != null)
        sequence.Append((Tween) inDuelMultiplier);
    }
    int amount = Mathf.RoundToInt((float) this._points * result);
    if (amount > 0 && target is FlockadeGameBoardTile flockadeGameBoardTile)
      flockadeGameBoardTile.GameBoard.GetPlayer(target.Side).Points.SetNextAsBonus(amount: amount);
    target.GameBoard.GetPlayer(target.Side).Points.Count += amount;
    return new IFlockadeBlessing.OnDuelLostResult(sequence);
  }

  public enum Condition
  {
    Won,
    Lost,
  }
}
