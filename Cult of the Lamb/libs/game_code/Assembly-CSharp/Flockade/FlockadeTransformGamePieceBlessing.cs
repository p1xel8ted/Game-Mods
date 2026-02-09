// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeTransformGamePieceBlessing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
namespace Flockade;

[Serializable]
public class FlockadeTransformGamePieceBlessing : FlockadeBlessingBase, IFlockadeBlessing
{
  [SerializeField]
  public FlockadeGamePieceConfiguration[] _into;
  [SerializeField]
  public FlockadeTransformGamePieceBlessing.Mode _mode;

  IFlockadeBlessing.OnDuelPhaseStartedResult IFlockadeBlessing.OnDuelPhaseStarted(
    IFlockadeGameBoardTile target)
  {
    FlockadeGamePieceConfiguration[] pieceConfigurationArray1;
    switch (this._mode)
    {
      case FlockadeTransformGamePieceBlessing.Mode.Any:
        pieceConfigurationArray1 = this._into;
        break;
      case FlockadeTransformGamePieceBlessing.Mode.ExcludeSameKindAsSelf:
        pieceConfigurationArray1 = ((IEnumerable<FlockadeGamePieceConfiguration>) this._into).Where<FlockadeGamePieceConfiguration>((Func<FlockadeGamePieceConfiguration, bool>) (candidate =>
        {
          int kind = (int) candidate.BaseConfiguration.Kind;
          FlockadeGamePiece.Kind? nullable = (bool) (UnityEngine.Object) target.GamePiece.Configuration ? new FlockadeGamePiece.Kind?(target.GamePiece.Configuration.BaseConfiguration.Kind) : new FlockadeGamePiece.Kind?();
          int valueOrDefault = (int) nullable.GetValueOrDefault();
          return !(kind == valueOrDefault & nullable.HasValue);
        })).ToArray<FlockadeGamePieceConfiguration>();
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
    FlockadeGamePieceConfiguration[] pieceConfigurationArray2 = pieceConfigurationArray1;
    FlockadeGamePieceConfiguration gamePiece = pieceConfigurationArray2[UnityEngine.Random.Range(0, pieceConfigurationArray2.Length)];
    Sequence sequence = (Sequence) null;
    if (target is FlockadeGameBoardTile flockadeGameBoardTile)
    {
      sequence = DOTween.Sequence().AppendCallback((TweenCallback) (() => AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/piece_transform"))).Append((Tween) flockadeGameBoardTile.GamePiece.Blessing.Activate(true)).Append((Tween) flockadeGameBoardTile.GamePiece.Pop(out IFlockadeGamePiece.State _)).Append((Tween) flockadeGameBoardTile.GamePiece.Set(gamePiece, true));
    }
    else
    {
      target.GamePiece.Pop();
      target.GamePiece.Set(gamePiece);
    }
    return new IFlockadeBlessing.OnDuelPhaseStartedResult(sequence);
  }

  public enum Mode
  {
    Any,
    ExcludeSameKindAsSelf,
  }
}
