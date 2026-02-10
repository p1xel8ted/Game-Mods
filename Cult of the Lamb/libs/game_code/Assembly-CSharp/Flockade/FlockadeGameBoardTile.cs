// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeGameBoardTile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Flockade;

[RequireComponent(typeof (RectTransform))]
public class FlockadeGameBoardTile : FlockadeGamePieceHolder, IFlockadeGameBoardTile
{
  public const float _DUELLING_ANGLE_TILT = -10f;
  public const float _DUELLING_DURATION = 0.25f;
  public const Ease _DUELLING_EASING = Ease.InOutQuad;
  public const float _ENTRY_OR_EXIT_DURATION = 0.233333334f;
  public const Ease _ENTRY_EASING = Ease.OutQuart;
  public const Ease _EXIT_EASING = Ease.InQuart;
  public const string _FRONT_BUFFER_GAME_PIECE_WARNING = "Front buffer must have the same FlockadeGamePiece as its parent FlockadeSelectableBase and was corrected accordingly!";
  [SerializeField]
  public RectTransform _orderBadgeRoot;
  [SerializeField]
  public TextMeshProUGUI _orderBadgeText;
  [Header("Game Board Tile Specifics")]
  [SerializeField]
  public CanvasGroup _canvasGroup;
  [SerializeField]
  public Image _background;
  [SerializeField]
  public FlockadeGameBoardTile.GamePieceBuffer[] _gamePieceBuffers;
  [SerializeField]
  public Image[] _highlightsForInspection;
  [SerializeField]
  public Color _highlightTargetedColor;
  [SerializeField]
  public Color _winningPointColor;
  [SerializeField]
  public Color _winningRoundColor;
  public Color _backgroundOriginColor;
  public DG.Tweening.Sequence _duelling;
  public DG.Tweening.Sequence _entryOrExit;
  public bool _inspectable;
  public bool _inspected;
  public FlockadeSelectableBase.FlockadeHighlight[] _inspectionHighlights;
  public bool _selected;
  public bool _targeted;
  public bool _winningPoint;
  public bool _winningRound;
  [CompilerGenerated]
  public FlockadeVirtualGameBoardTile \u003CCore\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003COverwriting\u003Ek__BackingField;
  [CompilerGenerated]
  public RectTransform \u003CRectTransform\u003Ek__BackingField;

  public void SetActivationOrderBadge(int? number, bool dim = false)
  {
    if (!number.HasValue)
    {
      if (!this._orderBadgeRoot.gameObject.activeSelf)
        return;
      this._orderBadgeRoot.transform.DOKill();
      this._orderBadgeRoot.transform.DOScale(Vector3.zero, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this._orderBadgeRoot.gameObject.SetActive(false)));
    }
    else
    {
      this._orderBadgeText.text = number.Value.ToString();
      if (!this._orderBadgeRoot.gameObject.activeSelf)
      {
        this._orderBadgeRoot.gameObject.SetActive(true);
        this._orderBadgeRoot.transform.DOKill();
        this._orderBadgeRoot.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
      }
      this._orderBadgeRoot.gameObject.SetActive(true);
      Vector2 anchoredPosition = this._orderBadgeRoot.anchoredPosition with
      {
        y = -15f
      };
      this._orderBadgeRoot.pivot = new Vector2(0.5f, 0.5f);
      if (this.Side == FlockadeGameBoardSide.Left)
      {
        this._orderBadgeRoot.anchorMin = new Vector2(1f, 1f);
        this._orderBadgeRoot.anchorMax = new Vector2(1f, 1f);
        anchoredPosition.x = -20f;
      }
      else
      {
        this._orderBadgeRoot.anchorMin = new Vector2(0.0f, 1f);
        this._orderBadgeRoot.anchorMax = new Vector2(0.0f, 1f);
        anchoredPosition.x = 20f;
      }
      this._orderBadgeRoot.anchoredPosition = anchoredPosition;
      float num = dim ? 0.35f : 1f;
      Color color1 = this._orderBadgeText.color;
      if ((double) color1.a != (double) num)
      {
        color1.a = num;
        this._orderBadgeText.DOKill();
        ShortcutExtensionsTMPText.DOColor(this._orderBadgeText, color1, 0.5f);
      }
      Image component = this._orderBadgeRoot.GetComponent<Image>();
      Color color2 = component.color;
      if ((double) color2.a == (double) num)
        return;
      color2.a = num;
      component.DOKill();
      DOTweenModuleUI.DOColor(component, color2, 0.5f);
    }
  }

  public FlockadeVirtualGameBoardTile Core
  {
    get => this.\u003CCore\u003Ek__BackingField;
    set => this.\u003CCore\u003Ek__BackingField = value;
  }

  public override bool Disabled => base.Disabled && !this._targeted && !this._selected;

  public override RectTransform GamePieceContainer => this._gamePieceBuffers[0].RectTransform;

  public override FlockadeSelectableBase.FlockadeHighlight[] Highlights
  {
    get => !this.Inspectable ? base.Highlights : this._inspectionHighlights;
  }

  public bool Inspectable => this._inspectable;

  public bool Overwriting
  {
    get => this.\u003COverwriting\u003Ek__BackingField;
    set => this.\u003COverwriting\u003Ek__BackingField = value;
  }

  public RectTransform RectTransform
  {
    get => this.\u003CRectTransform\u003Ek__BackingField;
    set => this.\u003CRectTransform\u003Ek__BackingField = value;
  }

  public void Configure(FlockadeGameBoard gameBoard, FlockadeGameBoardSide side, int index)
  {
    this.Configure(gameBoard.Information);
    this._gamePieceBuffers[1].GamePiece.Configure();
    this.Core = new FlockadeVirtualGameBoardTile((IFlockadeGameBoard) gameBoard, side, index, (IFlockadeGamePiece) base.GamePiece, (IFlockadeGameBoardTile) this);
  }

  public override void Awake()
  {
    base.Awake();
    this.RectTransform = this.GetComponent<RectTransform>();
    this.SetDuelled(true).Complete(true);
    this._backgroundOriginColor = this._background.color;
    this._inspectionHighlights = ((IEnumerable<Image>) this._highlightsForInspection).Select<Image, FlockadeSelectableBase.FlockadeHighlight>((Func<Image, FlockadeSelectableBase.FlockadeHighlight>) (highlight => new FlockadeSelectableBase.FlockadeHighlight(highlight))).ToArray<FlockadeSelectableBase.FlockadeHighlight>();
    this._gamePieceBuffers[1].RectTransform.gameObject.SetActive(false);
    this.SetActivationOrderBadge(new int?());
  }

  public override DG.Tweening.Sequence Enter(bool killOtherAnimations = true)
  {
    FlockadeGameBoardTile.GamePieceBuffer gamePieceBuffer = this._gamePieceBuffers[0];
    FlockadeGameBoardTile.GamePieceBuffer backBuffer = this._gamePieceBuffers[1];
    if (killOtherAnimations)
    {
      DG.Tweening.Sequence entryOrExit = this._entryOrExit;
      if (entryOrExit != null)
        entryOrExit.Kill();
    }
    this._entryOrExit = DOTween.Sequence().Append((Tween) gamePieceBuffer.RectTransform.DOAnchorPosY(0.0f, 0.233333334f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutQuart)).Join((Tween) DOTweenModuleUI.DOFade(gamePieceBuffer.Pedestal, 1f, 0.233333334f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutQuart)).Join((Tween) DOTweenModuleUI.DOFade(gamePieceBuffer.GamePiece.Image, 1f, 0.233333334f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutQuart)).Join((Tween) backBuffer.RectTransform.DOAnchorPosY(-this.Height, 0.233333334f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.InQuart)).Join((Tween) DOTweenModuleUI.DOFade(backBuffer.Pedestal, 0.0f, 0.233333334f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.InQuart)).Join((Tween) DOTweenModuleUI.DOFade(backBuffer.GamePiece.Image, 0.0f, 0.233333334f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.InQuart));
    if (!this.Overwriting && backBuffer.RectTransform.gameObject.activeSelf)
      this._entryOrExit.AppendCallback((TweenCallback) (() =>
      {
        backBuffer.GamePiece.Pop();
        backBuffer.RectTransform.gameObject.SetActive(false);
      }));
    return this._entryOrExit;
  }

  public override DG.Tweening.Sequence Exit(bool killOtherAnimations = true)
  {
    FlockadeGameBoardTile.GamePieceBuffer gamePieceBuffer1 = this._gamePieceBuffers[0];
    FlockadeGameBoardTile.GamePieceBuffer gamePieceBuffer2 = this._gamePieceBuffers[1];
    if (killOtherAnimations)
    {
      DG.Tweening.Sequence entryOrExit = this._entryOrExit;
      if (entryOrExit != null)
        entryOrExit.Kill();
    }
    this._entryOrExit = DOTween.Sequence().Append((Tween) gamePieceBuffer1.RectTransform.DOAnchorPosY(-this.Height, 0.233333334f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.InQuart)).Join((Tween) DOTweenModuleUI.DOFade(gamePieceBuffer1.Pedestal, 0.0f, 0.233333334f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.InQuart)).Join((Tween) DOTweenModuleUI.DOFade(gamePieceBuffer1.GamePiece.Image, 0.0f, 0.233333334f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.InQuart)).Join((Tween) gamePieceBuffer2.RectTransform.DOAnchorPosY(0.0f, 0.233333334f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutQuart)).Join((Tween) DOTweenModuleUI.DOFade(gamePieceBuffer2.Pedestal, 1f, 0.233333334f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutQuart)).Join((Tween) DOTweenModuleUI.DOFade(gamePieceBuffer2.GamePiece.Image, 1f, 0.233333334f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutQuart));
    return this._entryOrExit;
  }

  public override void OnSelect()
  {
    this.GameBoard.Indicators.SetEnabledLine(this.Index / this.GameBoard.RowCount > 0 ? FlockadeGameBoardIndicators.Line.Back : FlockadeGameBoardIndicators.Line.Front, true);
    if (this._inspectable)
    {
      this._inspected = true;
      base.OnSelect();
    }
    else
      this._selected = true;
    this.UpdateInactiveOverlays();
  }

  public override void OnDeselect()
  {
    this.GameBoard.Indicators.SetEnabledLine(FlockadeGameBoardIndicators.Line.None);
    if (this._inspectable)
    {
      this._inspected = false;
      base.OnDeselect();
    }
    else
      this._selected = false;
    this.UpdateInactiveOverlays();
  }

  public DG.Tweening.Sequence SetDuelled(bool duelled)
  {
    DG.Tweening.Sequence duelling = this._duelling;
    if (duelling != null)
      duelling.Kill();
    this._duelling = DOTween.Sequence().Append((Tween) this._canvasGroup.DOFade(duelled ? 0.0f : 1f, 0.25f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InOutQuad)).Join((Tween) this._background.rectTransform.DORotate(duelled ? new Vector3(0.0f, 0.0f, -10f) : Vector3.zero, 0.25f).From<Quaternion, Vector3, QuaternionOptions>(duelled ? Vector3.zero : new Vector3(0.0f, 0.0f, 10f)).SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(Ease.InOutQuad)).Join((Tween) this._background.rectTransform.DOScale(duelled ? 0.0f : 1f, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutQuad));
    return this._duelling;
  }

  public void SetInspectable(bool inspectable, Color highlight = default (Color))
  {
    if (this._inspectable == inspectable)
      return;
    this.TryPerformDeselectAction();
    this._inspectable = inspectable;
    this.SetInteractable(inspectable, highlight);
    this.Selectable.Confirmable = !inspectable;
  }

  public void SetTargeted(bool targeted)
  {
    this._targeted = targeted;
    this.SetHighlighted(targeted, targeted ? this._highlightTargetedColor : new Color());
    this.UpdateInactiveOverlays();
  }

  public void SetEffectHighlighted(bool on)
  {
    this.SetHighlighted(on, new Color(1f, 0.11f, 0.004f, 1f));
  }

  public void SetWinningPoint()
  {
    this._winningPoint = true;
    this.ChangeBackgroundColor(this._winningPointColor);
  }

  public void SetWinningRound()
  {
    this._winningRound = true;
    this.ChangeBackgroundColor(this._winningRoundColor);
  }

  public void UnsetWinningStates()
  {
    this._winningPoint = false;
    this._winningRound = false;
    this.ChangeBackgroundColor(this._backgroundOriginColor);
  }

  public void FadeActivationOrderBadge(float toAlpha, float duration)
  {
    this._orderBadgeText.DOKill();
    Image component = this._orderBadgeRoot.GetComponent<Image>();
    if ((bool) (UnityEngine.Object) component)
      component.DOKill();
    Color color1 = this._orderBadgeText.color;
    ShortcutExtensionsTMPText.DOColor(this._orderBadgeText, new Color(color1.r, color1.g, color1.b, toAlpha), duration);
    if (!(bool) (UnityEngine.Object) component)
      return;
    Color color2 = component.color;
    DOTweenModuleUI.DOColor(component, new Color(color2.r, color2.g, color2.b, toAlpha), duration);
  }

  public void ChangeBackgroundColor(Color to)
  {
    this._background.DOKill();
    DOTweenModuleUI.DOColor(this._background, to, 0.25f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutCubic);
  }

  public void Overwrite()
  {
    if (this.Overwriting)
      return;
    this.Overwriting = true;
    IFlockadeGamePiece.State gamePiece = this._gamePieceBuffers[0].GamePiece.Pop();
    this._gamePieceBuffers[1].RectTransform.gameObject.SetActive(true);
    this._gamePieceBuffers[1].GamePiece.Set(gamePiece);
    this._entryOrExit.Complete(true);
  }

  public IFlockadeBlessing.OnRemovedResult StopOverwriting(bool restore)
  {
    if (!this.Overwriting)
      return new IFlockadeBlessing.OnRemovedResult();
    this.Overwriting = false;
    if (restore)
    {
      IFlockadeGamePiece.State gamePiece = this._gamePieceBuffers[1].GamePiece.Pop();
      this._gamePieceBuffers[1].GamePiece.Set(this._gamePieceBuffers[0].GamePiece.Pop());
      this._entryOrExit.Complete(true);
      this._gamePieceBuffers[0].GamePiece.Set(gamePiece);
      this.GameBoard.PaintTentativeDuelOrder();
      return new IFlockadeBlessing.OnRemovedResult();
    }
    IFlockadeBlessing.OnRemovedResult onRemovedResult = this._gamePieceBuffers[1].GamePiece.Blessing.OnRemoved((IFlockadeGamePiece) this._gamePieceBuffers[1].GamePiece, this.Side, (IFlockadeGameBoard) this.GameBoard);
    this._gamePieceBuffers[1].GamePiece.Pop();
    this._gamePieceBuffers[1].RectTransform.gameObject.SetActive(false);
    return onRemovedResult;
  }

  IFlockadeBlessing.OnRemovedResult IFlockadeGameBoardTile.Wipe()
  {
    IFlockadeBlessing.OnRemovedResult onRemovedResult = this.Core.Wipe();
    this.UnsetWinningStates();
    return onRemovedResult;
  }

  public FlockadeGameBoard GameBoard => this.Core.GameBoard as FlockadeGameBoard;

  public override FlockadeGamePiece GamePiece => this.Core.GamePiece as FlockadeGamePiece;

  public int Index => this.Core.Index;

  public bool Locked => this.Core.Locked;

  public FlockadeGameBoardSide Side => this.Core.Side;

  public IFlockadeBlessing.OnPlacedResult Place()
  {
    IFlockadeBlessing.OnPlacedResult onPlacedResult = this.Core.Place();
    this.GameBoard.PaintTentativeDuelOrder();
    return onPlacedResult;
  }

  public IFlockadeBlessing.OnRemovedResult Wipe()
  {
    IFlockadeBlessing.OnRemovedResult onRemovedResult = this.Core.Wipe();
    this.UnsetWinningStates();
    this.GameBoard.PaintTentativeDuelOrder();
    return onRemovedResult;
  }

  [CompilerGenerated]
  public void \u003CSetActivationOrderBadge\u003Eb__9_0()
  {
    this._orderBadgeRoot.gameObject.SetActive(false);
  }

  [Serializable]
  public class GamePieceBuffer
  {
    public FlockadeGamePiece GamePiece;
    public Image Pedestal;
    public RectTransform RectTransform;

    public void DOKill()
    {
      this.GamePiece.Image.DOKill();
      this.Pedestal.DOKill();
      this.RectTransform.DOKill();
    }
  }
}
