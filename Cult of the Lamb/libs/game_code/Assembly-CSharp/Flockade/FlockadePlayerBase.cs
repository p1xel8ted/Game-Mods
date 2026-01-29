// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadePlayerBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Febucci.UI;
using I2.Loc;
using Lamb.UI;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Flockade;

[RequireComponent(typeof (CanvasGroup), typeof (RectTransform))]
public abstract class FlockadePlayerBase : MonoBehaviour, IFlockadePlayer
{
  public const string _CANCEL_SELECTION_SOUND = "event:/dlc/ui/flockade_minigame/click_back";
  public const string _CONFIRM_PLACEMENT_SOUND = "event:/dlc/ui/flockade_minigame/piece_place";
  public const string _BLESSINGS_CONTAINER = "Blessings";
  public const float _HIGHLIGHT_DURATION = 0.5f;
  public const Ease _HIGHLIGHT_EASING = Ease.InOutQuad;
  [SerializeField]
  public Localize _name;
  [SerializeField]
  public TextAnimator _nameAnimator;
  [SerializeField]
  public CanvasGroup _inactiveOverlay;
  [SerializeField]
  public FlockadePointCounter _pointCounter;
  [SerializeField]
  public FlockadeVictoryCounter _victoryCounter;
  public FlockadeGamePieceBag _bag;
  public FlockadeControlPrompts _controlPrompts;
  public FlockadeGameBoard _gameBoard;
  public UIMenuBase _parent;
  public FlockadeBlessing[] _blessings;
  public CanvasGroup _canvasGroup;
  public RectTransform _rectTransform;
  public Vector2 _originAnchoredPosition;
  public FlockadePlayerBase.FlockadeBlessingLink[] _blessingLinks;
  public DG.Tweening.Sequence _placingGamePiece;
  public FlockadeGamePieceChoice _selectedChoice;
  public FlockadeGameBoardTile _selectedTile;
  public bool _shouldTryShiftBlessings;
  public FlockadeGameBoardTile _targetedTile;
  [CompilerGenerated]
  public FlockadeVirtualPlayer \u003CCore\u003Ek__BackingField;
  [CompilerGenerated]
  public FlockadePlayerBase \u003COpponent\u003Ek__BackingField;
  [CompilerGenerated]
  public PlayerFarming \u003CPlayerFarming\u003Ek__BackingField;

  public abstract Graphic Avatar { get; }

  public bool CanPerformTurn
  {
    get
    {
      return this._gameBoard.GetTiles(this.Side).Any<FlockadeGameBoardTile>((Func<FlockadeGameBoardTile, bool>) (tile => !tile.Locked));
    }
  }

  public FlockadeVirtualPlayer Core
  {
    get => this.\u003CCore\u003Ek__BackingField;
    set => this.\u003CCore\u003Ek__BackingField = value;
  }

  public abstract Color Highlight { get; }

  public string Name => this._name.Term;

  public FlockadePlayerBase Opponent
  {
    get => this.\u003COpponent\u003Ek__BackingField;
    set => this.\u003COpponent\u003Ek__BackingField = value;
  }

  public PlayerFarming PlayerFarming
  {
    get => this.\u003CPlayerFarming\u003Ek__BackingField;
    set => this.\u003CPlayerFarming\u003Ek__BackingField = value;
  }

  public FlockadeGameBoardTile SelectedTile
  {
    get => this._selectedTile;
    set
    {
      if ((UnityEngine.Object) this._selectedTile == (UnityEngine.Object) value)
        return;
      if ((bool) (UnityEngine.Object) this._selectedTile)
      {
        if (this._selectedTile.Overwriting)
          this._selectedTile.StopOverwriting(true);
        else if (!this._selectedTile.Locked)
          this._selectedTile.GamePiece.Pop();
      }
      if ((bool) (UnityEngine.Object) this._targetedTile)
        this._targetedTile.SetTargeted(false);
      this._selectedTile = value;
      if ((bool) (UnityEngine.Object) this._selectedTile)
      {
        if (this._selectedTile.Locked)
          this._selectedTile.Overwrite();
        this._placingGamePiece = this._selectedTile.GamePiece.Set(this._selectedChoice.GamePiece.Configuration, true);
        this._targetedTile = this._gameBoard.GetOpposingTile(value);
        if (!(bool) (UnityEngine.Object) this._targetedTile)
          return;
        this._targetedTile.SetTargeted(true);
      }
      else
        this._targetedTile = (FlockadeGameBoardTile) null;
    }
  }

  public abstract string VictorySentence { get; }

  public void Configure(
    FlockadeGameBoardSide side,
    FlockadeGamePieceBag bag,
    FlockadeControlPrompts controlPrompts,
    UIMenuBase parent,
    PlayerFarming playerFarming = null)
  {
    this._bag = bag;
    this._controlPrompts = controlPrompts;
    this._parent = parent;
    this.PlayerFarming = playerFarming;
    this._pointCounter.Configure(side);
    this._victoryCounter.Configure(side);
    this.Core = new FlockadeVirtualPlayer(side, (IFlockadeCounter) this._pointCounter, (IFlockadeCounter) this._victoryCounter);
  }

  public virtual void LateConfigure(FlockadeGameBoard gameBoard) => this._gameBoard = gameBoard;

  public virtual void Awake()
  {
    this._blessings = this.transform.parent.Find("Blessings").GetComponentsInChildren<FlockadeBlessing>();
    this._blessingLinks = new FlockadePlayerBase.FlockadeBlessingLink[this._blessings.Length];
    this._canvasGroup = this.GetComponent<CanvasGroup>();
    this._rectTransform = this.GetComponent<RectTransform>();
    this._originAnchoredPosition = this._rectTransform.anchoredPosition;
    this.Exit();
    this.SetTurn(false).Complete(true);
  }

  public virtual void LateUpdate()
  {
    if (!this._shouldTryShiftBlessings)
      return;
    this.TryShiftBlessings();
    this._shouldTryShiftBlessings = false;
  }

  public void Exit()
  {
    float num = this.Side == FlockadeGameBoardSide.Left ? -1f : 1f;
    this._canvasGroup.alpha = 0.0f;
    this._rectTransform.anchoredPosition = this._originAnchoredPosition + new Vector2((float) ((double) num * (double) this._rectTransform.rect.width / 2.0), 0.0f);
  }

  public DG.Tweening.Sequence Enter()
  {
    return DOTween.Sequence().Append((Tween) this._canvasGroup.DOFade(1f, 0.8333333f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutCubic)).Join((Tween) this._rectTransform.DOAnchorPosX(this._originAnchoredPosition.x, 0.8333333f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutCubic));
  }

  public bool GetAcceptButtonDown()
  {
    return this._parent.CanvasGroup.interactable && this._controlPrompts.IsAccepting && InputManager.UI.GetAcceptButtonDown(this.PlayerFarming);
  }

  public bool GetFastForwardButtonHeld()
  {
    return this._parent.CanvasGroup.interactable && this._controlPrompts.IsFastForwarding && InputManager.UI.GetAcceptButtonHeld(this.PlayerFarming);
  }

  public bool GetSkipButtonDown()
  {
    return this._parent.CanvasGroup.interactable && this._controlPrompts.IsSkipping && InputManager.UI.GetAcceptButtonDown(this.PlayerFarming);
  }

  public virtual DG.Tweening.Sequence SetTurn(bool active, bool animateName = true)
  {
    this._nameAnimator.enabled = animateName & active;
    DG.Tweening.Sequence s = DOTween.Sequence().Append(this.SetHighlighted(active));
    foreach (FlockadeGameBoardTile tile in this._gameBoard.GetTiles(this.Side))
      s.Join((Tween) tile.SetActive(active));
    return s;
  }

  public Tween SetHighlighted(bool highlighted)
  {
    return (Tween) this._inactiveOverlay.DOFade(highlighted ? 0.0f : 1f, 0.5f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InOutQuad);
  }

  public virtual void SetConfirmPhaseControlsVisible(bool visible, bool cancellable = false)
  {
  }

  public abstract void PrefocusChoice(FlockadeGamePieceChoice choice);

  public IEnumerator SelectGamePiece()
  {
    MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = this.PlayerFarming;
    yield return (object) this._bag.Choices.SetActive<FlockadeGamePieceChoice>(true).WaitForCompletion();
    this.SetConfirmPhaseControlsVisible(true);
    FlockadeGamePieceChoice[] array = this._bag.Choices.Where<FlockadeGamePieceChoice>((Func<FlockadeGamePieceChoice, bool>) (choice => (bool) (UnityEngine.Object) choice.GamePiece.Configuration)).ToArray<FlockadeGamePieceChoice>();
    IEnumerator coroutine;
    yield return (object) (coroutine = this.SelectGamePiece(array));
    FlockadeGamePieceChoice current = coroutine.Current as FlockadeGamePieceChoice;
    this._selectedChoice = current;
    this.SetConfirmPhaseControlsVisible(false);
    yield return (object) FlockadeUtils.WaitForCompletion((IEnumerable<Tween>) new DG.Tweening.Sequence[2]
    {
      current.Exit(true),
      this._bag.Choices.SetActive<FlockadeGamePieceChoice>(false)
    });
    MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = (PlayerFarming) null;
  }

  public abstract IEnumerator SelectGamePiece(FlockadeGamePieceChoice[] availableChoices);

  public IEnumerator PlaceGamePiece(CancellationToken? cancellation = null)
  {
    FlockadePlayerBase flockadePlayerBase = this;
    MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = flockadePlayerBase.PlayerFarming;
    flockadePlayerBase.SetConfirmPhaseControlsVisible(true, true);
    FlockadeGameBoardTile[] array = flockadePlayerBase._gameBoard.GetAvailableTiles(flockadePlayerBase, flockadePlayerBase._selectedChoice.GamePiece.Configuration).ToArray<FlockadeGameBoardTile>();
    IEnumerator coroutine;
    yield return (object) (coroutine = flockadePlayerBase.PlaceGamePiece(array, cancellation));
    FlockadeGameBoardTile target = coroutine.Current as FlockadeGameBoardTile;
    flockadePlayerBase.Animate(FlockadePlayerBase.AnimationState.PlayPiece);
    flockadePlayerBase.SetConfirmPhaseControlsVisible(false, true);
    if (cancellation.HasValue && cancellation.GetValueOrDefault().IsCancellationRequested)
    {
      flockadePlayerBase.SelectedTile = (FlockadeGameBoardTile) null;
      AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/click_back");
      flockadePlayerBase._gameBoard.Information.Unlock((object) flockadePlayerBase);
      flockadePlayerBase.PrefocusChoice(flockadePlayerBase._bag.Choices.First<FlockadeGamePieceChoice>());
      flockadePlayerBase._selectedChoice.Enter(true);
      flockadePlayerBase._selectedChoice = (FlockadeGamePieceChoice) null;
    }
    else if ((bool) (UnityEngine.Object) target)
    {
      AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/piece_place");
      if (flockadePlayerBase._placingGamePiece != null && flockadePlayerBase._placingGamePiece.IsActive() && !flockadePlayerBase._placingGamePiece.IsComplete())
        yield return (object) flockadePlayerBase._placingGamePiece.WaitForCompletion();
      IFlockadeBlessing.OnPlacedResult onPlacedResult = target.Place();
      IFlockadeBlessing.OnRemovedResult onRemovedResult = (IFlockadeBlessing.OnRemovedResult) null;
      if (flockadePlayerBase.SelectedTile.Overwriting)
        onRemovedResult = flockadePlayerBase.SelectedTile.StopOverwriting(false);
      yield return (object) FlockadeUtils.WaitForCompletion((IEnumerable<Tween>) new DG.Tweening.Sequence[2]
      {
        onPlacedResult.Sequence,
        onRemovedResult?.Sequence
      });
      flockadePlayerBase.SelectedTile = (FlockadeGameBoardTile) null;
      flockadePlayerBase._selectedChoice = (FlockadeGamePieceChoice) null;
    }
    MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = (PlayerFarming) null;
  }

  public abstract IEnumerator PlaceGamePiece(
    FlockadeGameBoardTile[] availableTiles,
    CancellationToken? cancellation = null);

  public DG.Tweening.Sequence BindBlessing(FlockadeBlessingActivator.State blessing)
  {
    int index1 = Array.FindIndex<FlockadePlayerBase.FlockadeBlessingLink>(this._blessingLinks, (Predicate<FlockadePlayerBase.FlockadeBlessingLink>) (link => link != null && (UnityEngine.Object) link.Configuration == (UnityEngine.Object) blessing.Configuration));
    if (index1 >= 0)
    {
      this._blessingLinks[index1].Requesters.Add(blessing.Activator);
      return (DG.Tweening.Sequence) null;
    }
    int index2 = Array.FindIndex<FlockadePlayerBase.FlockadeBlessingLink>(this._blessingLinks, (Predicate<FlockadePlayerBase.FlockadeBlessingLink>) (link => link == null));
    if (index2 < 0)
      return (DG.Tweening.Sequence) null;
    this._blessingLinks[index2] = new FlockadePlayerBase.FlockadeBlessingLink(blessing.Configuration, new FlockadeBlessingActivator[1]
    {
      blessing.Activator
    });
    return this._blessings[index2].Show(blessing);
  }

  public DG.Tweening.Sequence UnbindBlessing(FlockadeBlessingActivator.State blessing)
  {
    int index = Array.FindIndex<FlockadePlayerBase.FlockadeBlessingLink>(this._blessingLinks, (Predicate<FlockadePlayerBase.FlockadeBlessingLink>) (link => link != null && (UnityEngine.Object) link.Configuration == (UnityEngine.Object) blessing.Configuration));
    if (index < 0)
      return (DG.Tweening.Sequence) null;
    this._blessingLinks[index].Requesters.Remove(blessing.Activator);
    if (this._blessingLinks[index].Requesters.Count != 0)
      return (DG.Tweening.Sequence) null;
    this._blessingLinks[index] = (FlockadePlayerBase.FlockadeBlessingLink) null;
    return this._blessings[index].Hide().AppendCallback((TweenCallback) (() => this._shouldTryShiftBlessings = true));
  }

  public void TryShiftBlessings()
  {
    for (int index1 = 0; index1 < this._blessingLinks.Length; ++index1)
    {
      if (this._blessingLinks[index1] == null)
      {
        bool flag = false;
        int index2;
        for (index2 = index1 + 1; index2 < this._blessingLinks.Length; ++index2)
        {
          if (this._blessingLinks[index2] != null)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          break;
        this._blessingLinks[index1] = this._blessingLinks[index2];
        this._blessingLinks[index2] = (FlockadePlayerBase.FlockadeBlessingLink) null;
        DOTween.Sequence().Append((Tween) this._blessings[index2].Hide().Append((Tween) this._blessings[index1].Show(new FlockadeBlessingActivator.State(this._blessingLinks[index1].Configuration, this._blessingLinks[index1].Requesters[0]), false)));
      }
    }
  }

  public FlockadeBlessing GetBoundBlessing(FlockadeBlessingActivator blessing)
  {
    int index = Array.FindIndex<FlockadePlayerBase.FlockadeBlessingLink>(this._blessingLinks, (Predicate<FlockadePlayerBase.FlockadeBlessingLink>) (link => link != null && link.Requesters.Contains(blessing)));
    return index < 0 ? (FlockadeBlessing) null : this._blessings[index];
  }

  public virtual void Animate(FlockadePlayerBase.AnimationState state)
  {
  }

  public virtual void OnBeforeTurn()
  {
  }

  public FlockadePointCounter Points => this.Core.Points as FlockadePointCounter;

  public FlockadeGameBoardSide Side => this.Core.Side;

  public FlockadeVictoryCounter Victories => this.Core.Victories as FlockadeVictoryCounter;

  public enum AnimationState
  {
    Idle,
    PlayPiece,
    LosePieceOrPoints,
    WinPieceOrPoints,
    LoseRound,
    WinRound,
    LoseGame,
    WinGame,
  }

  public class FlockadeBlessingLink
  {
    public FlockadeBlessingConfiguration Configuration;
    public List<FlockadeBlessingActivator> Requesters = new List<FlockadeBlessingActivator>();

    public FlockadeBlessingLink(
      FlockadeBlessingConfiguration configuration,
      params FlockadeBlessingActivator[] requesters)
    {
      this.Configuration = configuration;
      this.Requesters.AddRange((IEnumerable<FlockadeBlessingActivator>) requesters);
    }
  }
}
