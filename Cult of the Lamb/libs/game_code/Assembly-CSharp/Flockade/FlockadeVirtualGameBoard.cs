// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeVirtualGameBoard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace Flockade;

public class FlockadeVirtualGameBoard : IFlockadeGameBoard
{
  public IFlockadePlayer _leftPlayer;
  public IFlockadePlayer _rightPlayer;
  public IFlockadeGameBoardTile[] _leftSide;
  public IFlockadeGameBoardTile[] _rightSide;
  public IFlockadeGameBoard _wrapper;
  [CompilerGenerated]
  public FlockadePassiveManager \u003CPassiveManager\u003Ek__BackingField;
  [CompilerGenerated]
  public int \u003CRowCount\u003Ek__BackingField;

  public FlockadeVirtualGameBoard(
    FlockadePassiveManager passiveManager,
    int rowCount,
    IFlockadePlayer leftPlayer,
    IFlockadePlayer rightPlayer,
    IFlockadeGameBoardTile[] leftSide = null,
    IFlockadeGameBoardTile[] rightSide = null,
    IFlockadeGameBoard wrapper = null)
  {
    this._wrapper = wrapper;
    this.\u003CPassiveManager\u003Ek__BackingField = passiveManager;
    this.\u003CRowCount\u003Ek__BackingField = rowCount;
    this._leftPlayer = leftPlayer;
    this._rightPlayer = rightPlayer;
    this._leftSide = leftSide;
    this._rightSide = rightSide;
  }

  public bool AllTilesAreLocked
  {
    get
    {
      return ((IEnumerable<IFlockadeGameBoardTile>) this._leftSide).All<IFlockadeGameBoardTile>((Func<IFlockadeGameBoardTile, bool>) (tile => tile.Locked)) && ((IEnumerable<IFlockadeGameBoardTile>) this._rightSide).All<IFlockadeGameBoardTile>((Func<IFlockadeGameBoardTile, bool>) (tile => tile.Locked));
    }
  }

  public FlockadeVirtualGameBoard Core => this;

  public FlockadePassiveManager PassiveManager => this.\u003CPassiveManager\u003Ek__BackingField;

  public int RowCount => this.\u003CRowCount\u003Ek__BackingField;

  public IFlockadeGameBoard This => this._wrapper ?? (IFlockadeGameBoard) this;

  public IFlockadePlayer GetPlayer(FlockadeGameBoardSide side)
  {
    if (side == FlockadeGameBoardSide.Left)
      return this._leftPlayer;
    if (side == FlockadeGameBoardSide.Right)
      return this._rightPlayer;
    throw new ArgumentOutOfRangeException();
  }

  public IEnumerable<IFlockadePlayer> GetPlayers()
  {
    yield return this._leftPlayer;
    yield return this._rightPlayer;
  }

  public IEnumerable<IFlockadeGameBoardTile> GetColumnOf(IFlockadeGameBoardTile tile)
  {
    return this.This.GetTiles(tile.Side).Skip<IFlockadeGameBoardTile>(tile.Index / this.RowCount * this.RowCount).Take<IFlockadeGameBoardTile>(this.RowCount);
  }

  public IEnumerable<IFlockadeGameBoardTile> GetRowOf(IFlockadeGameBoardTile tile)
  {
    return this.This.GetTiles(tile.Side).Where<IFlockadeGameBoardTile>((Func<IFlockadeGameBoardTile, int, bool>) ((_, currentIndex) => currentIndex % this.RowCount == tile.Index % this.RowCount));
  }

  public int GetTileCount(FlockadeGameBoardSide side)
  {
    return ((IFlockadeGameBoardTile[]) this.This.GetTiles(side)).Length;
  }

  public IEnumerable<IFlockadeGameBoardTile> GetTiles(FlockadeGameBoardSide side)
  {
    if (side == FlockadeGameBoardSide.Left)
      return (IEnumerable<IFlockadeGameBoardTile>) this._leftSide;
    if (side == FlockadeGameBoardSide.Right)
      return (IEnumerable<IFlockadeGameBoardTile>) this._rightSide;
    throw new ArgumentOutOfRangeException();
  }

  public IEnumerable<IFlockadeGameBoardTile> GetTiles()
  {
    return ((IEnumerable<IFlockadeGameBoardTile>) this._leftSide).Concat<IFlockadeGameBoardTile>((IEnumerable<IFlockadeGameBoardTile>) this._rightSide);
  }

  public IFlockadeGameBoardTile GetTile(FlockadeGameBoardSide side, int index)
  {
    return ((IFlockadeGameBoardTile[]) this.This.GetTiles(side))[index];
  }

  public IFlockadeGameBoardTile GetOpposingTile(IFlockadeGameBoardTile tile)
  {
    return ((IFlockadeGameBoardTile[]) this.This.GetTiles(tile.Side.GetOpposing()))[tile.Index];
  }

  public IEnumerable<IFlockadeGameBoardTile> GetAvailableTiles(
    IFlockadePlayer player,
    FlockadeGamePieceConfiguration gamePiece)
  {
    IFlockadeBlessing.BeforePlacingResult beforePlacingResult = (IFlockadeBlessing.BeforePlacingResult) null;
    if ((bool) (UnityEngine.Object) gamePiece.BlessingConfiguration)
      beforePlacingResult = gamePiece.BlessingConfiguration.Blessing.BeforePlacing(this.This, player.Side);
    return (IEnumerable<IFlockadeGameBoardTile>) beforePlacingResult?.OverriddenAvailableTiles ?? (IEnumerable<IFlockadeGameBoardTile>) this.This.GetTiles(player.Side).Where<IFlockadeGameBoardTile>((Func<IFlockadeGameBoardTile, bool>) (tile => !tile.Locked)).ToArray<IFlockadeGameBoardTile>();
  }

  public void StartDuelPhase()
  {
    FlockadeUtils.RunCoroutineSynchronously(this.This.StartDuelPhase());
  }

  public IEnumerator StartDuelPhase(
    Func<FlockadeVirtualGameBoard.BeforeApplyingBlessingsContext, IEnumerator> beforeApplyingBlessings,
    Func<FlockadeVirtualGameBoard.OnApplyingBlessingContext, IEnumerator> onApplyingBlessing)
  {
    List<FlockadeVirtualBlessingActivator> orderedLeftSideBlessings = new List<FlockadeVirtualBlessingActivator>();
    List<FlockadeVirtualBlessingActivator> orderedRightSideBlessings = new List<FlockadeVirtualBlessingActivator>();
    for (int index = 0; index < this._leftSide.Length; ++index)
    {
      orderedLeftSideBlessings.Add(this._leftSide[index].GamePiece.Blessing);
      orderedRightSideBlessings.Add(this._rightSide[index].GamePiece.Blessing);
    }
    List<List<FlockadeVirtualBlessingActivator>> orderedSides = new List<List<FlockadeVirtualBlessingActivator>>()
    {
      orderedLeftSideBlessings,
      orderedRightSideBlessings
    };
    bool isOrderReversed;
    FlockadeVirtualGameBoard.BeforeApplyingBlessingsContext blessingsContext = new FlockadeVirtualGameBoard.BeforeApplyingBlessingsContext()
    {
      OrderOfDuelsAndBlessingsApplicationReversedBlessing = this.PassiveManager.IsOrderOfDuelsAndBlessingsApplicationReversed(out isOrderReversed)
    };
    if (beforeApplyingBlessings != null)
      yield return (object) beforeApplyingBlessings(blessingsContext);
    if (isOrderReversed)
    {
      orderedLeftSideBlessings.Reverse();
      orderedRightSideBlessings.Reverse();
      orderedSides.Reverse();
    }
    for (int index = 0; index < this._leftSide.Length; ++index)
    {
      foreach (List<FlockadeVirtualBlessingActivator> blessingActivatorList in orderedSides)
      {
        FlockadeVirtualBlessingActivator blessingActivator = blessingActivatorList[index];
        FlockadeGameBoardSide flockadeGameBoardSide = blessingActivatorList == orderedLeftSideBlessings ? FlockadeGameBoardSide.Left : FlockadeGameBoardSide.Right;
        IFlockadeGameBoardTile target = flockadeGameBoardSide == FlockadeGameBoardSide.Left ? this._leftSide[index] : this._rightSide[index];
        if (!(target is FlockadeGameBoardTile flockadeGameBoardTile) || !flockadeGameBoardTile.GamePiece.Blessing.Consumed)
        {
          IFlockadeBlessing.OnDuelPhaseStartedResult phaseStartedResult = blessingActivator.OnDuelPhaseStarted(target);
          FlockadeVirtualGameBoard.OnApplyingBlessingContext applyingBlessingContext = new FlockadeVirtualGameBoard.OnApplyingBlessingContext()
          {
            BlessingTileIndex = index,
            OnDuelPhaseStartedBlessing = phaseStartedResult,
            Side = flockadeGameBoardSide,
            HasEffect = phaseStartedResult != null && phaseStartedResult.Sequence != null && (double) phaseStartedResult.Sequence.Duration() > 0.0
          };
          if (onApplyingBlessing != null)
            yield return (object) onApplyingBlessing(applyingBlessingContext);
        }
      }
    }
  }

  public void Resolve() => FlockadeUtils.RunCoroutineSynchronously(this.This.Resolve());

  public IEnumerator Resolve(
    Action<FlockadeVirtualGameBoard.BeforeDuelDefaultScoringContext> beforeDuelDefaultScoring,
    Func<FlockadeVirtualGameBoard.OnDuelResolvedContext, IEnumerator> onDuelResolved,
    Func<IEnumerator> beforeDuelPhaseEnding)
  {
    bool aborted = false;
    IFlockadeGameBoardTile abortedBy = (IFlockadeGameBoardTile) null;
    for (int index = this.This.PassiveManager.IsOrderOfDuelsAndBlessingsApplicationReversed() ? this._leftSide.Length - 1 : 0; (this.This.PassiveManager.IsOrderOfDuelsAndBlessingsApplicationReversed() ? (index >= 0 ? 1 : 0) : (index < this._leftSide.Length ? 1 : 0)) != 0; index += this.This.PassiveManager.IsOrderOfDuelsAndBlessingsApplicationReversed() ? -1 : 1)
    {
      FlockadeVirtualGameBoard.OnDuelResolvedContext duelResolvedContext = new FlockadeVirtualGameBoard.OnDuelResolvedContext()
      {
        Index = index
      };
      IFlockadeGameBoardTile flockadeGameBoardTile1 = this._leftSide[index];
      IFlockadeGameBoardTile flockadeGameBoardTile2 = this._rightSide[index];
      IFlockadeBlessing.OnResolvedResult onResolvedResult1 = flockadeGameBoardTile1.GamePiece.Blessing.OnResolved(flockadeGameBoardTile1, flockadeGameBoardTile2);
      IFlockadeBlessing.OnResolvedResult onResolvedResult2 = flockadeGameBoardTile2.GamePiece.Blessing.OnResolved(flockadeGameBoardTile2, flockadeGameBoardTile1);
      IFlockadeBlessing.OnResolvedResult onResolvedResult3 = onResolvedResult2;
      ref FlockadeFight.Result? local = ref onResolvedResult2.OverriddenResolution;
      FlockadeFight.Result? nullable1;
      FlockadeFight.Result? nullable2;
      if (!local.HasValue)
      {
        nullable1 = new FlockadeFight.Result?();
        nullable2 = nullable1;
      }
      else
        nullable2 = new FlockadeFight.Result?(local.GetValueOrDefault().Reverse());
      onResolvedResult3.OverriddenResolution = nullable2;
      duelResolvedContext.OnResolvedBlessings = new IFlockadeBlessing.OnResolvedResult[2]
      {
        onResolvedResult1,
        onResolvedResult2
      };
      nullable1 = onResolvedResult1.OverriddenResolution;
      FlockadeFight.Result result1 = (FlockadeFight.Result) ((int) nullable1 ?? (int) onResolvedResult2.OverriddenResolution ?? (int) flockadeGameBoardTile1.GamePiece.Fight(flockadeGameBoardTile2.GamePiece));
      duelResolvedContext.Resolution = result1;
      nullable1 = onResolvedResult1.OverriddenResolution;
      bool flag1;
      if (nullable1.HasValue)
      {
        switch (nullable1.GetValueOrDefault())
        {
          case FlockadeFight.Result.DefeatAndDuelPhaseEnd:
          case FlockadeFight.Result.WinAndDuelPhaseEnd:
            flag1 = true;
            goto label_8;
        }
      }
      flag1 = false;
label_8:
      if (flag1)
      {
        aborted = true;
        abortedBy = flockadeGameBoardTile1;
      }
      nullable1 = onResolvedResult2.OverriddenResolution;
      bool flag2;
      if (nullable1.HasValue)
      {
        switch (nullable1.GetValueOrDefault())
        {
          case FlockadeFight.Result.DefeatAndDuelPhaseEnd:
          case FlockadeFight.Result.WinAndDuelPhaseEnd:
            flag2 = true;
            goto label_14;
        }
      }
      flag2 = false;
label_14:
      if (flag2)
      {
        aborted = true;
        abortedBy = flockadeGameBoardTile2;
      }
      if (!aborted)
      {
        List<Sequence> sequenceList = new List<Sequence>();
        switch (result1)
        {
          case FlockadeFight.Result.Unknown:
          case FlockadeFight.Result.Tie:
            duelResolvedContext.OnScoringBlessings = sequenceList.ToArray();
            break;
          case FlockadeFight.Result.Defeat:
            IFlockadeBlessing.OnDuelLostResult onDuelLostResult1 = flockadeGameBoardTile1.GamePiece.Blessing.OnDuelLost(flockadeGameBoardTile1);
            sequenceList.Add(onDuelLostResult1.Sequence);
            IFlockadeBlessing.OnDuelWonResult onDuelWonResult1 = flockadeGameBoardTile2.GamePiece.Blessing.OnDuelWon(flockadeGameBoardTile2);
            sequenceList.Add(onDuelWonResult1.Sequence);
            if (!onDuelWonResult1.IsScoringOverridden)
            {
              float result2;
              sequenceList.Add(this.This.PassiveManager.GetPointsWonInDuelMultiplier(FlockadeGameBoardSide.Right, out result2));
              int num = Mathf.RoundToInt(result2);
              Action<FlockadeVirtualGameBoard.BeforeDuelDefaultScoringContext> action = beforeDuelDefaultScoring;
              if (action != null)
                action(new FlockadeVirtualGameBoard.BeforeDuelDefaultScoringContext()
                {
                  Points = num,
                  Side = FlockadeGameBoardSide.Right
                });
              this.This.GetPlayer(FlockadeGameBoardSide.Right).Points.Count += num;
              goto case FlockadeFight.Result.Unknown;
            }
            goto case FlockadeFight.Result.Unknown;
          case FlockadeFight.Result.Win:
            IFlockadeBlessing.OnDuelLostResult onDuelLostResult2 = flockadeGameBoardTile2.GamePiece.Blessing.OnDuelLost(flockadeGameBoardTile2);
            sequenceList.Add(onDuelLostResult2.Sequence);
            IFlockadeBlessing.OnDuelWonResult onDuelWonResult2 = flockadeGameBoardTile1.GamePiece.Blessing.OnDuelWon(flockadeGameBoardTile1);
            sequenceList.Add(onDuelWonResult2.Sequence);
            if (!onDuelWonResult2.IsScoringOverridden)
            {
              float result3;
              sequenceList.Add(this.This.PassiveManager.GetPointsWonInDuelMultiplier(FlockadeGameBoardSide.Left, out result3));
              int num = Mathf.RoundToInt(result3);
              Action<FlockadeVirtualGameBoard.BeforeDuelDefaultScoringContext> action = beforeDuelDefaultScoring;
              if (action != null)
                action(new FlockadeVirtualGameBoard.BeforeDuelDefaultScoringContext()
                {
                  Points = num,
                  Side = FlockadeGameBoardSide.Left
                });
              this.This.GetPlayer(FlockadeGameBoardSide.Left).Points.Count += num;
              goto case FlockadeFight.Result.Unknown;
            }
            goto case FlockadeFight.Result.Unknown;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
      if (onDuelResolved != null)
        yield return (object) onDuelResolved(duelResolvedContext);
      if (aborted)
        break;
    }
    if (beforeDuelPhaseEnding != null)
      yield return (object) beforeDuelPhaseEnding();
    IFlockadeBlessing.OnDuelPhaseEndedResult phaseEndedResult = (IFlockadeBlessing.OnDuelPhaseEndedResult) null;
    if (aborted)
      phaseEndedResult = abortedBy.GamePiece.Blessing.OnDuelPhaseEnded(abortedBy);
    bool flag = phaseEndedResult != null && phaseEndedResult.IsScoringOverridden;
    if (!flag)
    {
      IFlockadePlayer player1 = this.This.GetPlayer(FlockadeGameBoardSide.Left);
      IFlockadePlayer player2 = this.This.GetPlayer(FlockadeGameBoardSide.Right);
      if (player1.Points.Count > player2.Points.Count)
        ++player1.Victories.Count;
      else if (player1.Points.Count < player2.Points.Count)
      {
        ++player2.Victories.Count;
      }
      else
      {
        ++player1.Victories.Count;
        ++player2.Victories.Count;
      }
    }
    yield return flag ? (object) abortedBy.Side : (object) (Enum) null;
  }

  public FlockadeVirtualGameBoard Clone()
  {
    FlockadeVirtualGameBoard clone = new FlockadeVirtualGameBoard(this.PassiveManager.Clone(), this.RowCount, (IFlockadePlayer) this._leftPlayer.Core.Clone(), (IFlockadePlayer) this._rightPlayer.Core.Clone());
    clone._leftSide = ((IEnumerable<IFlockadeGameBoardTile>) this._leftSide).Select<IFlockadeGameBoardTile, IFlockadeGameBoardTile>((Func<IFlockadeGameBoardTile, IFlockadeGameBoardTile>) (tile => (IFlockadeGameBoardTile) tile.Core.Clone((IFlockadeGameBoard) clone))).ToArray<IFlockadeGameBoardTile>();
    clone._rightSide = ((IEnumerable<IFlockadeGameBoardTile>) this._rightSide).Select<IFlockadeGameBoardTile, IFlockadeGameBoardTile>((Func<IFlockadeGameBoardTile, IFlockadeGameBoardTile>) (tile => (IFlockadeGameBoardTile) tile.Core.Clone((IFlockadeGameBoard) clone))).ToArray<IFlockadeGameBoardTile>();
    return clone;
  }

  public class BeforeApplyingBlessingsContext
  {
    [CompilerGenerated]
    public Sequence \u003COrderOfDuelsAndBlessingsApplicationReversedBlessing\u003Ek__BackingField;

    public Sequence OrderOfDuelsAndBlessingsApplicationReversedBlessing
    {
      get => this.\u003COrderOfDuelsAndBlessingsApplicationReversedBlessing\u003Ek__BackingField;
      set
      {
        this.\u003COrderOfDuelsAndBlessingsApplicationReversedBlessing\u003Ek__BackingField = value;
      }
    }
  }

  public class OnApplyingBlessingContext
  {
    [CompilerGenerated]
    public int \u003CBlessingTileIndex\u003Ek__BackingField;
    [CompilerGenerated]
    public IFlockadeBlessing.OnDuelPhaseStartedResult \u003COnDuelPhaseStartedBlessing\u003Ek__BackingField;
    [CompilerGenerated]
    public FlockadeGameBoardSide \u003CSide\u003Ek__BackingField;
    [CompilerGenerated]
    public bool \u003CHasEffect\u003Ek__BackingField;

    public int BlessingTileIndex
    {
      get => this.\u003CBlessingTileIndex\u003Ek__BackingField;
      set => this.\u003CBlessingTileIndex\u003Ek__BackingField = value;
    }

    public IFlockadeBlessing.OnDuelPhaseStartedResult OnDuelPhaseStartedBlessing
    {
      get => this.\u003COnDuelPhaseStartedBlessing\u003Ek__BackingField;
      set => this.\u003COnDuelPhaseStartedBlessing\u003Ek__BackingField = value;
    }

    public FlockadeGameBoardSide Side
    {
      get => this.\u003CSide\u003Ek__BackingField;
      set => this.\u003CSide\u003Ek__BackingField = value;
    }

    public bool HasEffect
    {
      get => this.\u003CHasEffect\u003Ek__BackingField;
      set => this.\u003CHasEffect\u003Ek__BackingField = value;
    }
  }

  public class BeforeDuelDefaultScoringContext
  {
    [CompilerGenerated]
    public int \u003CPoints\u003Ek__BackingField;
    [CompilerGenerated]
    public FlockadeGameBoardSide \u003CSide\u003Ek__BackingField;

    public int Points
    {
      get => this.\u003CPoints\u003Ek__BackingField;
      set => this.\u003CPoints\u003Ek__BackingField = value;
    }

    public FlockadeGameBoardSide Side
    {
      get => this.\u003CSide\u003Ek__BackingField;
      set => this.\u003CSide\u003Ek__BackingField = value;
    }
  }

  public class OnDuelResolvedContext
  {
    [CompilerGenerated]
    public int \u003CIndex\u003Ek__BackingField;
    [CompilerGenerated]
    public IFlockadeBlessing.OnResolvedResult[] \u003COnResolvedBlessings\u003Ek__BackingField;
    [CompilerGenerated]
    public Sequence[] \u003COnScoringBlessings\u003Ek__BackingField;
    [CompilerGenerated]
    public FlockadeFight.Result \u003CResolution\u003Ek__BackingField;

    public int Index
    {
      get => this.\u003CIndex\u003Ek__BackingField;
      set => this.\u003CIndex\u003Ek__BackingField = value;
    }

    public IFlockadeBlessing.OnResolvedResult[] OnResolvedBlessings
    {
      get => this.\u003COnResolvedBlessings\u003Ek__BackingField;
      set => this.\u003COnResolvedBlessings\u003Ek__BackingField = value;
    }

    public Sequence[] OnScoringBlessings
    {
      get => this.\u003COnScoringBlessings\u003Ek__BackingField;
      set => this.\u003COnScoringBlessings\u003Ek__BackingField = value;
    }

    public FlockadeFight.Result Resolution
    {
      get => this.\u003CResolution\u003Ek__BackingField;
      set => this.\u003CResolution\u003Ek__BackingField = value;
    }
  }
}
