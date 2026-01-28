// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeScorePointsOnMovedBlessing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System;
using UnityEngine;

#nullable disable
namespace Flockade;

[Serializable]
public class FlockadeScorePointsOnMovedBlessing : FlockadeBlessingBase, IFlockadeBlessing
{
  public const string _ACTIVATION_SOUND = "event:/dlc/ui/flockade_minigame/piece_blessing_wanderingshepherd_activate";
  [SerializeField]
  public int _points;

  new void IFlockadeBlessing.BeforeMoving(IFlockadeGameBoardTile target)
  {
    if (!(target is FlockadeGameBoardTile flockadeGameBoardTile))
      return;
    DOTween.Sequence().AppendCallback((TweenCallback) (() => AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/piece_blessing_wanderingshepherd_activate"))).Append((Tween) flockadeGameBoardTile.GamePiece.Blessing.Activate(false));
  }

  new void IFlockadeBlessing.OnMoved(IFlockadeGameBoardTile target)
  {
    if (target is FlockadeGameBoardTile flockadeGameBoardTile)
    {
      FlockadePlayerBase player = flockadeGameBoardTile.GameBoard.GetPlayer(flockadeGameBoardTile.Side);
      int points = this._points;
      if (points <= 0)
      {
        if (points < 0)
          player.Animate(FlockadePlayerBase.AnimationState.LosePieceOrPoints);
      }
      else
      {
        player.Points.SetNextAsBonus(amount: this._points);
        player.Animate(FlockadePlayerBase.AnimationState.WinPieceOrPoints);
      }
    }
    target.GameBoard.GetPlayer(target.Side).Points.Count += this._points;
  }
}
