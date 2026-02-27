// Decompiled with JetBrains decompiler
// Type: Flockade.IFlockadeBlessing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections;

#nullable disable
namespace Flockade;

public interface IFlockadeBlessing
{
  IFlockadeBlessing.BeforePlacingResult BeforePlacing(
    IFlockadeGameBoard gameBoard,
    FlockadeGameBoardSide side)
  {
    return new IFlockadeBlessing.BeforePlacingResult();
  }

  void OnEvaluated(IFlockadeGameBoardTile target, IFlockadeGameBoardTile opposing)
  {
  }

  IFlockadeBlessing.OnPlacedResult OnPlaced(IFlockadeGameBoardTile target)
  {
    return new IFlockadeBlessing.OnPlacedResult();
  }

  IFlockadeBlessing.OnDuelPhaseStartedResult OnDuelPhaseStarted(IFlockadeGameBoardTile target)
  {
    return new IFlockadeBlessing.OnDuelPhaseStartedResult();
  }

  void BeforeMoving(IFlockadeGameBoardTile target)
  {
  }

  void OnMoved(IFlockadeGameBoardTile target)
  {
  }

  IFlockadeBlessing.OnResolvedResult OnResolved(
    IFlockadeGameBoardTile target,
    IFlockadeGameBoardTile opposing)
  {
    return new IFlockadeBlessing.OnResolvedResult();
  }

  IFlockadeBlessing.OnDuelWonResult OnDuelWon(IFlockadeGameBoardTile target)
  {
    return new IFlockadeBlessing.OnDuelWonResult();
  }

  IFlockadeBlessing.OnDuelLostResult OnDuelLost(IFlockadeGameBoardTile target)
  {
    return new IFlockadeBlessing.OnDuelLostResult();
  }

  IFlockadeBlessing.OnDuelPhaseEndedResult OnDuelPhaseEnded(IFlockadeGameBoardTile target)
  {
    return new IFlockadeBlessing.OnDuelPhaseEndedResult();
  }

  IFlockadeBlessing.OnRemovedResult OnRemoved(
    IFlockadeGamePiece target,
    FlockadeGameBoardSide side,
    IFlockadeGameBoard gameBoard)
  {
    return new IFlockadeBlessing.OnRemovedResult();
  }

  class BlessingResult
  {
    public Sequence Sequence;
    public IEnumerator WaitForCompletion;

    public BlessingResult(Sequence sequence = null)
    {
      this.Sequence = sequence;
      this.WaitForCompletion = (sequence != null ? (IEnumerator) sequence.WaitForCompletion(true) : (IEnumerator) null) ?? IFlockadeBlessing.BlessingResult.YieldBreak();
    }

    public static IEnumerator YieldBreak()
    {
      yield break;
    }
  }

  class BeforePlacingResult : IFlockadeBlessing.BlessingResult
  {
    public IFlockadeGameBoardTile[] OverriddenAvailableTiles;

    public BeforePlacingResult(IFlockadeGameBoardTile[] overriddenAvailableTiles = null, Sequence sequence = null)
      : base(sequence)
    {
      this.OverriddenAvailableTiles = overriddenAvailableTiles;
    }
  }

  class OnPlacedResult(Sequence sequence = null) : IFlockadeBlessing.BlessingResult(sequence)
  {
  }

  class OnDuelPhaseStartedResult(Sequence sequence = null) : IFlockadeBlessing.BlessingResult(sequence)
  {
  }

  class OnResolvedResult : IFlockadeBlessing.BlessingResult
  {
    public FlockadeFight.Result? OverriddenResolution;

    public OnResolvedResult(FlockadeFight.Result? overriddenResolution = null, Sequence sequence = null)
      : base(sequence)
    {
      this.OverriddenResolution = overriddenResolution;
    }
  }

  class OnDuelWonResult : IFlockadeBlessing.BlessingResult
  {
    public bool IsScoringOverridden;

    public OnDuelWonResult(bool isScoringOverridden = false, Sequence sequence = null)
      : base(sequence)
    {
      this.IsScoringOverridden = isScoringOverridden;
    }
  }

  class OnDuelLostResult(Sequence sequence = null) : IFlockadeBlessing.BlessingResult(sequence)
  {
  }

  class OnDuelPhaseEndedResult : IFlockadeBlessing.BlessingResult
  {
    public bool IsScoringOverridden;

    public OnDuelPhaseEndedResult(bool isScoringOverridden = false, Sequence sequence = null)
      : base(sequence)
    {
      this.IsScoringOverridden = isScoringOverridden;
    }
  }

  class OnRemovedResult(Sequence sequence = null) : IFlockadeBlessing.BlessingResult(sequence)
  {
  }
}
