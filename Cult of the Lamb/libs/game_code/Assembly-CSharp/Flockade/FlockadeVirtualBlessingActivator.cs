// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeVirtualBlessingActivator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace Flockade;

public class FlockadeVirtualBlessingActivator : FlockadeBlessingBase, IFlockadeBlessing
{
  [CompilerGenerated]
  public IFlockadeGamePiece \u003CGamePiece\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CNullified\u003Ek__BackingField;

  public FlockadeVirtualBlessingActivator(IFlockadeGamePiece gamePiece, bool nullified = false)
  {
    this.GamePiece = gamePiece;
    this.Nullified = nullified;
  }

  public static implicit operator bool(FlockadeVirtualBlessingActivator self)
  {
    return self.GamePiece != null && (bool) (Object) self.GamePiece.Configuration && (bool) (Object) self.GamePiece.Configuration.BlessingConfiguration;
  }

  public bool Active => (bool) this && !this.Nullified;

  public IFlockadeBlessing Blessing
  {
    get
    {
      return !this.Active ? (IFlockadeBlessing) null : this.GamePiece.Configuration.BlessingConfiguration.Blessing;
    }
  }

  public IFlockadeGamePiece GamePiece
  {
    get => this.\u003CGamePiece\u003Ek__BackingField;
    set => this.\u003CGamePiece\u003Ek__BackingField = value;
  }

  public bool Nullified
  {
    get => this.\u003CNullified\u003Ek__BackingField;
    set => this.\u003CNullified\u003Ek__BackingField = value;
  }

  public virtual Sequence Nullify()
  {
    if (!(bool) this || this.Nullified)
      return (Sequence) null;
    IFlockadeBlessing.OnRemovedResult onRemovedResult = this.OnRemoved(this.GamePiece, this.GamePiece.Tile.Side, this.GamePiece.Tile.GameBoard);
    this.Nullified = true;
    return onRemovedResult.Sequence;
  }

  public T OfType<T>() where T : class, IFlockadeBlessing => this.Blessing as T;

  public IFlockadeBlessing.BeforePlacingResult BeforePlacing(
    IFlockadeGameBoard gameBoard,
    FlockadeGameBoardSide side)
  {
    return this.Blessing?.BeforePlacing(gameBoard, side) ?? new IFlockadeBlessing.BeforePlacingResult();
  }

  public void OnEvaluated(IFlockadeGameBoardTile target, IFlockadeGameBoardTile opposing)
  {
    this.Blessing?.OnEvaluated(target, opposing);
  }

  public IFlockadeBlessing.OnPlacedResult OnPlaced(IFlockadeGameBoardTile target)
  {
    return this.Blessing?.OnPlaced(target) ?? new IFlockadeBlessing.OnPlacedResult();
  }

  public IFlockadeBlessing.OnDuelPhaseStartedResult OnDuelPhaseStarted(IFlockadeGameBoardTile target)
  {
    return this.Blessing?.OnDuelPhaseStarted(target) ?? new IFlockadeBlessing.OnDuelPhaseStartedResult();
  }

  public void BeforeMoving(IFlockadeGameBoardTile target) => this.Blessing?.BeforeMoving(target);

  public void OnMoved(IFlockadeGameBoardTile target) => this.Blessing?.OnMoved(target);

  public IFlockadeBlessing.OnResolvedResult OnResolved(
    IFlockadeGameBoardTile target,
    IFlockadeGameBoardTile opposing)
  {
    return this.Blessing?.OnResolved(target, opposing) ?? new IFlockadeBlessing.OnResolvedResult();
  }

  public IFlockadeBlessing.OnDuelWonResult OnDuelWon(IFlockadeGameBoardTile target)
  {
    return this.Blessing?.OnDuelWon(target) ?? new IFlockadeBlessing.OnDuelWonResult();
  }

  public IFlockadeBlessing.OnDuelLostResult OnDuelLost(IFlockadeGameBoardTile target)
  {
    return this.Blessing?.OnDuelLost(target) ?? new IFlockadeBlessing.OnDuelLostResult();
  }

  public IFlockadeBlessing.OnDuelPhaseEndedResult OnDuelPhaseEnded(IFlockadeGameBoardTile target)
  {
    return this.Blessing?.OnDuelPhaseEnded(target) ?? new IFlockadeBlessing.OnDuelPhaseEndedResult();
  }

  public IFlockadeBlessing.OnRemovedResult OnRemoved(
    IFlockadeGamePiece target,
    FlockadeGameBoardSide side,
    IFlockadeGameBoard gameBoard)
  {
    return this.Blessing?.OnRemoved(target, side, gameBoard) ?? new IFlockadeBlessing.OnRemovedResult();
  }
}
