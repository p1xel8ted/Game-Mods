// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeNpc
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using Lamb.UI;
using Spine.Unity;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Flockade;

public class FlockadeNpc : FlockadeNpcBase
{
  [SerializeField]
  public SkeletonGraphic _avatar;
  public FlockadeNpcAI _ai;
  public FlockadeNpcConfiguration _configuration;
  public bool _turnSkipped;

  public override Graphic Avatar => (Graphic) this._avatar;

  public override Color Highlight => this._configuration.Highlight;

  public override bool TurnSkipped => this._turnSkipped |= this.GetSkipButtonDown();

  public override string VictorySentence => this._configuration.VictorySentence;

  public string WinGameSound => this._configuration.WinGameSound;

  public string LoseGameSound => this._configuration.LoseGameSound;

  public void Configure(
    FlockadeNpcConfiguration configuration,
    FlockadeGameBoardSide side,
    FlockadeGamePieceBag bag,
    FlockadeControlPrompts controlPrompts,
    UIMenuBase parent)
  {
    this._configuration = configuration;
    this._name.Term = configuration.Name;
    this._avatar.skeletonDataAsset = configuration.Skeleton;
    this._avatar.initialSkinName = configuration.Skin;
    this._avatar.startingAnimation = configuration.IdleAnimation;
    this._avatar.initialFlipX = false;
    this._avatar.color = (bool) (UnityEngine.Object) configuration.Skeleton ? Color.white : Color.clear;
    this._avatar.Initialize(true);
    this.Configure(side, bag, controlPrompts, parent);
  }

  public override void LateConfigure(FlockadeGameBoard gameBoard)
  {
    this._ai = this._configuration.CreateAI(gameBoard, this.Side);
    base.LateConfigure(gameBoard);
  }

  public override Sequence SetTurn(bool active, bool animateName = true)
  {
    if (!active)
      this._turnSkipped = false;
    return base.SetTurn(active, animateName);
  }

  public override void SetConfirmPhaseControlsVisible(bool visible, bool cancellable = false)
  {
    if (visible)
      this._controlPrompts.ShowSkipButton();
    else
      this._controlPrompts.HideSkipButton();
  }

  public override IEnumerator Select(FlockadeGamePieceChoice[] choices)
  {
    yield return (object) this._ai.Select(choices);
  }

  public override IEnumerator Place(FlockadeGameBoardTile[] availableTiles)
  {
    yield return (object) this._ai.Place(availableTiles);
  }

  public override void Animate(FlockadePlayerBase.AnimationState state)
  {
    string animationName;
    switch (state)
    {
      case FlockadePlayerBase.AnimationState.Idle:
        animationName = this._configuration.IdleAnimation;
        break;
      case FlockadePlayerBase.AnimationState.PlayPiece:
        animationName = this._configuration.PlayPieceAnimation;
        break;
      case FlockadePlayerBase.AnimationState.LosePieceOrPoints:
        animationName = this._configuration.LosePieceOrPointsAnimation;
        break;
      case FlockadePlayerBase.AnimationState.WinPieceOrPoints:
        animationName = this._configuration.WinPieceOrPointsAnimation;
        break;
      case FlockadePlayerBase.AnimationState.LoseRound:
        animationName = this._configuration.LoseRoundAnimation;
        break;
      case FlockadePlayerBase.AnimationState.WinRound:
        animationName = this._configuration.WinRoundAnimation;
        break;
      case FlockadePlayerBase.AnimationState.LoseGame:
        animationName = this._configuration.LoseGameAnimation;
        break;
      case FlockadePlayerBase.AnimationState.WinGame:
        animationName = this._configuration.WinGameAnimation;
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
    this._avatar.AnimationState.SetAnimation(0, animationName, false);
    this._avatar.AnimationState.AddAnimation(0, this._configuration.IdleAnimation, true, 0.0f);
  }
}
