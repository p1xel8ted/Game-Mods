// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadePlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using Spine.Unity;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Flockade;

public class FlockadePlayer : FlockadePlayerBase
{
  [SerializeField]
  public SkeletonGraphic _avatar;
  [SerializeField]
  public Color _highlight;
  [SerializeField]
  [SpineAnimation("", "_avatar", true, false)]
  public string _idleAnimation;
  [SerializeField]
  [SpineAnimation("", "_avatar", true, false)]
  public string _losePieceOrPointsAnimation;
  [SerializeField]
  [SpineAnimation("", "_avatar", true, false)]
  public string _loseRoundAnimation;
  [SerializeField]
  [SpineAnimation("", "_avatar", true, false)]
  public string _loseGameAnimation;
  [SerializeField]
  [SpineAnimation("", "_avatar", true, false)]
  public string _playPieceAnimation;
  [SerializeField]
  [SpineAnimation("", "_avatar", true, false)]
  public string _winPieceOrPointsAnimation;
  [SerializeField]
  [SpineAnimation("", "_avatar", true, false)]
  public string _winRoundAnimation;
  [SerializeField]
  [SpineAnimation("", "_avatar", true, false)]
  public string _winGameAnimation;
  [SerializeField]
  [TermsPopup("")]
  public string _victorySentence;

  public override Graphic Avatar => (Graphic) this._avatar;

  public override Color Highlight => this._highlight;

  public override string VictorySentence => this._victorySentence;

  public new void Configure(
    FlockadeGameBoardSide side,
    FlockadeGamePieceBag bag,
    FlockadeControlPrompts controlPrompts,
    UIMenuBase parent,
    PlayerFarming playerFarming = null)
  {
    base.Configure(side, bag, controlPrompts, parent, playerFarming);
  }

  public override void SetConfirmPhaseControlsVisible(bool visible, bool cancellable = false)
  {
    if (visible)
    {
      this._controlPrompts.ShowAcceptButton();
      if (!cancellable)
        return;
      this._controlPrompts.HideBackButton();
      this._controlPrompts.ShowCancelButton();
    }
    else
    {
      this._controlPrompts.HideAcceptButton();
      if (!cancellable)
        return;
      this._controlPrompts.HideCancelButton();
      this._controlPrompts.ShowBackButton();
    }
  }

  public override void PrefocusChoice(FlockadeGamePieceChoice choice)
  {
    choice.Selectable.Confirmable = false;
    choice.SetInteractable(true, this.Highlight);
    MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) choice.Selectable);
  }

  public override IEnumerator SelectGamePiece(FlockadeGamePieceChoice[] availableChoices)
  {
    FlockadePlayer requester = this;
    FlockadeGamePieceChoice choice = (FlockadeGamePieceChoice) null;
    requester._bag.SetInteractable(true, requester.Highlight);
    requester._gameBoard.GetTiles().SetInspectable<FlockadeGameBoardTile>(true, requester.Highlight);
    ((IEnumerable<FlockadeGamePieceChoice>) availableChoices).SetConfirmable<FlockadeGamePieceChoice>(true);
    ((IEnumerable<FlockadeGamePieceChoice>) availableChoices).SetInteractable<FlockadeGamePieceChoice>(true, requester.Highlight, (Action<FlockadeGamePieceChoice>) (chosen => choice = chosen));
    yield return (object) new WaitUntil((Func<bool>) (() => (bool) (UnityEngine.Object) choice));
    requester._gameBoard.Information.Lock((object) requester);
    FlockadeGamePieceChoice[] self = availableChoices;
    Action<FlockadeGamePieceChoice> action = (Action<FlockadeGamePieceChoice>) (chosen => choice = chosen);
    Color highlight = new Color();
    Action<FlockadeGamePieceChoice> onClick = action;
    ((IEnumerable<FlockadeGamePieceChoice>) self).SetInteractable<FlockadeGamePieceChoice>(false, highlight, onClick);
    requester._gameBoard.GetTiles().SetInspectable<FlockadeGameBoardTile>(false);
    requester._bag.SetInteractable(false);
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    yield return (object) choice;
  }

  public override IEnumerator PlaceGamePiece(
    FlockadeGameBoardTile[] availableTiles,
    CancellationToken? cancellation = null)
  {
    FlockadePlayer flockadePlayer = this;
    FlockadeGameBoardTile target = (FlockadeGameBoardTile) null;
    ((IEnumerable<FlockadeGameBoardTile>) availableTiles).SetInteractable<FlockadeGameBoardTile>(true, flockadePlayer.Highlight, (Action<FlockadeGameBoardTile>) (tile => target = tile), (Action<FlockadeGameBoardTile>) (tile => this.SelectedTile = tile));
    MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) availableTiles[0].Selectable);
    yield return (object) new WaitUntil((Func<bool>) (() =>
    {
      if ((bool) (UnityEngine.Object) target)
        return true;
      ref CancellationToken? local = ref cancellation;
      return local.HasValue && local.GetValueOrDefault().IsCancellationRequested;
    }));
    FlockadeGameBoardTile[] self = availableTiles;
    Action<FlockadeGameBoardTile> action1 = (Action<FlockadeGameBoardTile>) (tile => target = tile);
    Action<FlockadeGameBoardTile> action2 = (Action<FlockadeGameBoardTile>) (tile => this.SelectedTile = tile);
    Color highlight = new Color();
    Action<FlockadeGameBoardTile> onClick = action1;
    Action<FlockadeGameBoardTile> onSelected = action2;
    ((IEnumerable<FlockadeGameBoardTile>) self).SetInteractable<FlockadeGameBoardTile>(false, highlight, onClick, onSelected);
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    yield return (object) target;
  }

  public override void Animate(FlockadePlayerBase.AnimationState state)
  {
    string animationName;
    switch (state)
    {
      case FlockadePlayerBase.AnimationState.Idle:
        animationName = this._idleAnimation;
        break;
      case FlockadePlayerBase.AnimationState.PlayPiece:
        animationName = this._playPieceAnimation;
        break;
      case FlockadePlayerBase.AnimationState.LosePieceOrPoints:
        animationName = this._losePieceOrPointsAnimation;
        break;
      case FlockadePlayerBase.AnimationState.WinPieceOrPoints:
        animationName = this._winPieceOrPointsAnimation;
        break;
      case FlockadePlayerBase.AnimationState.LoseRound:
        animationName = this._loseRoundAnimation;
        break;
      case FlockadePlayerBase.AnimationState.WinRound:
        animationName = this._winRoundAnimation;
        break;
      case FlockadePlayerBase.AnimationState.LoseGame:
        animationName = this._loseGameAnimation;
        break;
      case FlockadePlayerBase.AnimationState.WinGame:
        animationName = this._winGameAnimation;
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
    this._avatar.AnimationState.SetAnimation(0, animationName, false);
    this._avatar.AnimationState.AddAnimation(0, this._idleAnimation, true, 0.0f);
  }
}
