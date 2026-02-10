// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeWinsRoundBlessing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System;
using UnityEngine;

#nullable disable
namespace Flockade;

[Serializable]
public class FlockadeWinsRoundBlessing : FlockadeBlessingBase, IFlockadeBlessing
{
  public const string _ACTIVATION_SOUND = "event:/dlc/ui/flockade_minigame/piece_blessing_guardianshepherd_activate_start";
  [SerializeField]
  public FlockadeGamePieceBaseConfiguration[] _againstKinds;
  [SerializeField]
  public FlockadeBlessingConfiguration[] _againstBlessings;
  [SerializeField]
  public FlockadeGamePieceConfiguration[] _againstPieces;
  [SerializeField]
  public bool _againstSameKind;
  [SerializeField]
  public bool _againstSameBlessing;
  [SerializeField]
  public bool _againstSamePiece;

  public bool IsActive(IFlockadeGameBoardTile target, IFlockadeGameBoardTile opposing)
  {
    if (!(bool) (UnityEngine.Object) opposing.GamePiece.Configuration)
      return false;
    if (this._againstKinds.Contains<FlockadeGamePieceBaseConfiguration>(opposing.GamePiece.Configuration.BaseConfiguration) || this._againstBlessings.Contains<FlockadeBlessingConfiguration>(opposing.GamePiece.Configuration.BlessingConfiguration) && opposing.GamePiece.Blessing.Active || this._againstPieces.Contains<FlockadeGamePieceConfiguration>(opposing.GamePiece.Configuration) || this._againstSameKind && (UnityEngine.Object) opposing.GamePiece.Configuration.BaseConfiguration == (UnityEngine.Object) target.GamePiece.Configuration.BaseConfiguration || this._againstSameBlessing && (UnityEngine.Object) opposing.GamePiece.Configuration.BlessingConfiguration == (UnityEngine.Object) target.GamePiece.Configuration.BlessingConfiguration && opposing.GamePiece.Blessing.Active)
      return true;
    return this._againstSamePiece && (UnityEngine.Object) opposing.GamePiece.Configuration == (UnityEngine.Object) target.GamePiece.Configuration;
  }

  void IFlockadeBlessing.OnEvaluated(IFlockadeGameBoardTile target, IFlockadeGameBoardTile opposing)
  {
    if (!(target is FlockadeGameBoardTile flockadeGameBoardTile) || !this.IsActive(target, opposing))
      return;
    flockadeGameBoardTile.SetWinningRound();
  }

  IFlockadeBlessing.OnResolvedResult IFlockadeBlessing.OnResolved(
    IFlockadeGameBoardTile target,
    IFlockadeGameBoardTile opposing)
  {
    if (!this.IsActive(target, opposing))
      return new IFlockadeBlessing.OnResolvedResult();
    Sequence sequence = (Sequence) null;
    if (target is FlockadeGameBoardTile flockadeGameBoardTile)
      sequence = DOTween.Sequence().AppendCallback((TweenCallback) (() => AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/piece_blessing_guardianshepherd_activate_start"))).Append((Tween) flockadeGameBoardTile.GamePiece.Blessing.Activate(true));
    return new IFlockadeBlessing.OnResolvedResult(new FlockadeFight.Result?(FlockadeFight.Result.WinAndDuelPhaseEnd), sequence);
  }

  IFlockadeBlessing.OnDuelPhaseEndedResult IFlockadeBlessing.OnDuelPhaseEnded(
    IFlockadeGameBoardTile target)
  {
    ++target.GameBoard.GetPlayer(target.Side).Victories.Count;
    return new IFlockadeBlessing.OnDuelPhaseEndedResult(true);
  }
}
