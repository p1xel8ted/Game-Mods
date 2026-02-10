// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeTiesAreWinsBlessing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
namespace Flockade;

[Serializable]
public class FlockadeTiesAreWinsBlessing : FlockadeBlessingBase, IFlockadeBlessing
{
  public const string _ACTIVATION_SOUND = "event:/dlc/ui/flockade_minigame/piece_blessing_just_activate";
  [SerializeField]
  public FlockadeTiesAreWinsBlessing.Condition _condition;

  public bool IsActive(IFlockadeGameBoardTile target)
  {
    if (this._condition == FlockadeTiesAreWinsBlessing.Condition.None)
      return true;
    IFlockadeGameBoardTile[] array;
    switch (this._condition)
    {
      case FlockadeTiesAreWinsBlessing.Condition.AllOfDifferentTypeInColumn:
      case FlockadeTiesAreWinsBlessing.Condition.AllOfSameTypeInColumn:
      case FlockadeTiesAreWinsBlessing.Condition.OtherOfSameTypeInColumn:
        array = target.GameBoard.GetColumnOf(target).ToArray<IFlockadeGameBoardTile>();
        break;
      default:
        array = target.GameBoard.GetRowOf(target).ToArray<IFlockadeGameBoardTile>();
        break;
    }
    IFlockadeGameBoardTile[] flockadeGameBoardTileArray = array;
    bool flag;
    switch (this._condition)
    {
      case FlockadeTiesAreWinsBlessing.Condition.AllOfDifferentTypeInRow:
      case FlockadeTiesAreWinsBlessing.Condition.AllOfDifferentTypeInColumn:
        flag = ((IEnumerable<IFlockadeGameBoardTile>) flockadeGameBoardTileArray).GroupBy<IFlockadeGameBoardTile, FlockadeGamePiece.Kind?>((Func<IFlockadeGameBoardTile, FlockadeGamePiece.Kind?>) (tile => !(bool) (UnityEngine.Object) tile.GamePiece.Configuration ? new FlockadeGamePiece.Kind?() : new FlockadeGamePiece.Kind?(tile.GamePiece.Configuration.BaseConfiguration.Kind))).All<IGrouping<FlockadeGamePiece.Kind?, IFlockadeGameBoardTile>>((Func<IGrouping<FlockadeGamePiece.Kind?, IFlockadeGameBoardTile>, bool>) (group => !group.Key.HasValue || group.Count<IFlockadeGameBoardTile>() == 1));
        break;
      case FlockadeTiesAreWinsBlessing.Condition.AllOfSameTypeInRow:
      case FlockadeTiesAreWinsBlessing.Condition.AllOfSameTypeInColumn:
        flag = ((IEnumerable<IFlockadeGameBoardTile>) flockadeGameBoardTileArray).Except<IFlockadeGameBoardTile>((IEnumerable<IFlockadeGameBoardTile>) new IFlockadeGameBoardTile[1]
        {
          target
        }).All<IFlockadeGameBoardTile>((Func<IFlockadeGameBoardTile, bool>) (tile =>
        {
          FlockadeGamePiece.Kind? nullable1 = (bool) (UnityEngine.Object) tile.GamePiece.Configuration ? new FlockadeGamePiece.Kind?(tile.GamePiece.Configuration.BaseConfiguration.Kind) : new FlockadeGamePiece.Kind?();
          FlockadeGamePiece.Kind? nullable2 = (bool) (UnityEngine.Object) target.GamePiece.Configuration ? new FlockadeGamePiece.Kind?(target.GamePiece.Configuration.BaseConfiguration.Kind) : new FlockadeGamePiece.Kind?();
          return nullable1.GetValueOrDefault() == nullable2.GetValueOrDefault() & nullable1.HasValue == nullable2.HasValue;
        }));
        break;
      case FlockadeTiesAreWinsBlessing.Condition.OtherOfSameTypeInRow:
      case FlockadeTiesAreWinsBlessing.Condition.OtherOfSameTypeInColumn:
        flag = ((IEnumerable<IFlockadeGameBoardTile>) flockadeGameBoardTileArray).Except<IFlockadeGameBoardTile>((IEnumerable<IFlockadeGameBoardTile>) new IFlockadeGameBoardTile[1]
        {
          target
        }).Any<IFlockadeGameBoardTile>((Func<IFlockadeGameBoardTile, bool>) (tile =>
        {
          FlockadeGamePiece.Kind? nullable3 = (bool) (UnityEngine.Object) tile.GamePiece.Configuration ? new FlockadeGamePiece.Kind?(tile.GamePiece.Configuration.BaseConfiguration.Kind) : new FlockadeGamePiece.Kind?();
          FlockadeGamePiece.Kind? nullable4 = (bool) (UnityEngine.Object) target.GamePiece.Configuration ? new FlockadeGamePiece.Kind?(target.GamePiece.Configuration.BaseConfiguration.Kind) : new FlockadeGamePiece.Kind?();
          return nullable3.GetValueOrDefault() == nullable4.GetValueOrDefault() & nullable3.HasValue == nullable4.HasValue;
        }));
        break;
      default:
        flag = false;
        break;
    }
    return flag;
  }

  public bool IsApplicable(IFlockadeGameBoardTile target, IFlockadeGameBoardTile opposing)
  {
    if (target.GamePiece.Fight(opposing.GamePiece) != FlockadeFight.Result.Tie)
      return false;
    FlockadeTiesAreWinsBlessing tiesAreWinsBlessing = opposing.GamePiece.Blessing.OfType<FlockadeTiesAreWinsBlessing>();
    return tiesAreWinsBlessing == null || !tiesAreWinsBlessing.IsActive(opposing);
  }

  void IFlockadeBlessing.OnEvaluated(IFlockadeGameBoardTile target, IFlockadeGameBoardTile opposing)
  {
    if (!(target is FlockadeGameBoardTile flockadeGameBoardTile) || !this.IsActive(target) || !this.IsApplicable(target, opposing))
      return;
    flockadeGameBoardTile.SetWinningPoint();
  }

  IFlockadeBlessing.OnResolvedResult IFlockadeBlessing.OnResolved(
    IFlockadeGameBoardTile target,
    IFlockadeGameBoardTile opposing)
  {
    if (!this.IsActive(target) || !this.IsApplicable(target, opposing))
      return new IFlockadeBlessing.OnResolvedResult();
    Sequence sequence = (Sequence) null;
    if (target is FlockadeGameBoardTile flockadeGameBoardTile)
      sequence = DOTween.Sequence().AppendCallback((TweenCallback) (() => AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/piece_blessing_just_activate"))).Append((Tween) flockadeGameBoardTile.GamePiece.Blessing.Activate(true));
    return new IFlockadeBlessing.OnResolvedResult(new FlockadeFight.Result?(FlockadeFight.Result.Win), sequence);
  }

  public enum Condition
  {
    None,
    AllOfDifferentTypeInRow,
    AllOfDifferentTypeInColumn,
    AllOfSameTypeInRow,
    AllOfSameTypeInColumn,
    OtherOfSameTypeInRow,
    OtherOfSameTypeInColumn,
  }
}
