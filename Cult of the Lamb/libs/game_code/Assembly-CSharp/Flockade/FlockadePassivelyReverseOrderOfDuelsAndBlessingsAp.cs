// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadePassivelyReverseOrderOfDuelsAndBlessingsApplicationBlessing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Flockade;

[Serializable]
public class FlockadePassivelyReverseOrderOfDuelsAndBlessingsApplicationBlessing : 
  FlockadeBlessingBase,
  IFlockadeBlessing
{
  public const string _ACTIVATION_SOUND = "event:/dlc/ui/flockade_minigame/piece_blessing_trickstershepherd_activate";
  public const string _QUERIES_SOUND = "event:/dlc/ui/flockade_minigame/piece_blessing_trickstershepherd_battle_phase_start";
  public static Dictionary<IFlockadeGamePiece, Func<Sequence>> _CACHED_QUERIES_SUBSCRIPTIONS = new Dictionary<IFlockadeGamePiece, Func<Sequence>>();

  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
  public static void ResetCaches()
  {
    FlockadePassivelyReverseOrderOfDuelsAndBlessingsApplicationBlessing._CACHED_QUERIES_SUBSCRIPTIONS.Clear();
  }

  IFlockadeBlessing.OnPlacedResult IFlockadeBlessing.OnPlaced(IFlockadeGameBoardTile target)
  {
    target.GameBoard.PassiveManager.ReverseCurrentOrderOfDuelsAndBlessingsApplication();
    Sequence sequence1 = (Sequence) null;
    FlockadeGameBoardTile uiTarget = target as FlockadeGameBoardTile;
    if (uiTarget != null)
    {
      Func<Sequence> sequence2 = (Func<Sequence>) (() => DOTween.Sequence().AppendCallback((TweenCallback) (() => AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/piece_blessing_trickstershepherd_battle_phase_start"))).Append((Tween) uiTarget.GamePiece.Blessing.Activate()));
      uiTarget.GameBoard.PassiveManager.SubscribeToOrderOfDuelsAndBlessingsApplicationQueries(sequence2);
      FlockadePassivelyReverseOrderOfDuelsAndBlessingsApplicationBlessing._CACHED_QUERIES_SUBSCRIPTIONS.Add((IFlockadeGamePiece) uiTarget.GamePiece, sequence2);
      sequence1 = DOTween.Sequence().AppendCallback((TweenCallback) (() => AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/piece_blessing_trickstershepherd_activate"))).Append((Tween) uiTarget.GameBoard.Indicators.FlipIcon());
    }
    return new IFlockadeBlessing.OnPlacedResult(sequence1);
  }

  IFlockadeBlessing.OnRemovedResult IFlockadeBlessing.OnRemoved(
    IFlockadeGamePiece target,
    FlockadeGameBoardSide side,
    IFlockadeGameBoard gameBoard)
  {
    gameBoard.PassiveManager.ReverseCurrentOrderOfDuelsAndBlessingsApplication();
    Sequence sequence1 = (Sequence) null;
    if (target is FlockadeGamePiece flockadeGamePiece)
    {
      Func<Sequence> sequence2;
      if (FlockadePassivelyReverseOrderOfDuelsAndBlessingsApplicationBlessing._CACHED_QUERIES_SUBSCRIPTIONS.Remove((IFlockadeGamePiece) flockadeGamePiece, ref sequence2))
        gameBoard.PassiveManager.UnsubscribeFromOrderOfDuelsAndBlessingsApplicationQueries(sequence2);
      if (gameBoard is FlockadeGameBoard flockadeGameBoard)
        sequence1 = flockadeGameBoard.Indicators.FlipIcon();
    }
    return new IFlockadeBlessing.OnRemovedResult(sequence1);
  }
}
