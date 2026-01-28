// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeStealPointsBlessing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System;
using UnityEngine;

#nullable disable
namespace Flockade;

[Serializable]
public class FlockadeStealPointsBlessing : FlockadeBlessingBase, IFlockadeBlessing
{
  public const string _ACTIVATION_SOUND = "event:/dlc/ui/flockade_minigame/piece_blessing_marked_activate";
  [SerializeField]
  public int _points = 1;
  [SerializeField]
  public FlockadeAnimation _stolenPointAnimation;
  [SerializeField]
  public bool _stolenPointAnimationFlipX;
  [SerializeField]
  public bool _stolenPointAnimationFlipY;

  IFlockadeBlessing.OnResolvedResult IFlockadeBlessing.OnResolved(
    IFlockadeGameBoardTile target,
    IFlockadeGameBoardTile opposing)
  {
    Sequence sequence = (Sequence) null;
    if (target is FlockadeGameBoardTile flockadeGameBoardTile && target.GamePiece.Fight(opposing.GamePiece) == FlockadeFight.Result.Win)
      sequence = DOTween.Sequence().AppendCallback((TweenCallback) (() => AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/piece_blessing_marked_activate"))).Append((Tween) flockadeGameBoardTile.GamePiece.Blessing.Activate(true));
    return new IFlockadeBlessing.OnResolvedResult(sequence: sequence);
  }

  IFlockadeBlessing.OnDuelWonResult IFlockadeBlessing.OnDuelWon(IFlockadeGameBoardTile target)
  {
    IFlockadeCounter points1 = target.GameBoard.GetPlayer(target.Side).Points;
    FlockadeGameBoardSide opposing = target.Side.GetOpposing();
    IFlockadeCounter points2 = target.GameBoard.GetPlayer(opposing).Points;
    int num1 = Math.Min(points2.Count, this._points);
    float result1 = 1f;
    float result2 = 1f;
    Sequence sequence = (Sequence) null;
    if (num1 < 0)
      sequence = target.GameBoard.PassiveManager.GetPointsWonInDuelMultiplier(opposing, out result1);
    else if (this._points > 0)
      sequence = target.GameBoard.PassiveManager.GetPointsWonInDuelMultiplier(target.Side, out result2);
    int num2 = Mathf.RoundToInt((float) num1 * result1);
    int num3 = Mathf.RoundToInt((float) this._points * result2);
    FlockadePointCounter flockadePointCounter = (FlockadePointCounter) null;
    if (target is FlockadeGameBoardTile flockadeGameBoardTile)
    {
      flockadePointCounter = flockadeGameBoardTile.GameBoard.GetPlayer(opposing).Points;
      if (num2 < 0)
        flockadePointCounter.SetNextAsBonus(amount: -num2);
      else if (this._points > 1)
        flockadeGameBoardTile.GameBoard.GetPlayer(target.Side).Points.SetNextAsBonus(1, this._points - 1);
    }
    if ((bool) (UnityEngine.Object) this._stolenPointAnimation && (bool) (UnityEngine.Object) flockadePointCounter && num2 > 0)
      flockadePointCounter.SetCount(flockadePointCounter.Count - num2, this._stolenPointAnimation, (this._stolenPointAnimationFlipX, this._stolenPointAnimationFlipY));
    else
      points2.Count -= num2;
    points1.Count += num3;
    return new IFlockadeBlessing.OnDuelWonResult(true, sequence);
  }
}
