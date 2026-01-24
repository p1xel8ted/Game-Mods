// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeGamePieceBag
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using src.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace Flockade;

[RequireComponent(typeof (CanvasGroup), typeof (RectTransform))]
public class FlockadeGamePieceBag : FlockadeSelectableBase
{
  public const float _BETWEEN_CHOICES_APPEARANCE_DISAPPEARANCE_DELAY = 0.06666667f;
  public const string _AVAILABLE_GAME_PIECES_APPEARANCE_SOUND = "event:/dlc/ui/flockade_minigame/pieces_appear";
  [Header("Game Piece Bag Specifics")]
  [SerializeField]
  public FlockadeBottomContainer _container;
  [SerializeField]
  public FlockadeGameBoardSide _closestMargin;
  [SerializeField]
  public FlockadeGamePieceChoice[] _gamePieceChoices;
  [SerializeField]
  public float _horizontalShiftToUnselect;
  [SerializeField]
  public FlockadeGamePieceBagMenu _menuPrefab;
  [SerializeField]
  public bool _useDebugOverridePieces;
  [SerializeField]
  public List<FlockadeGamePieceConfiguration> _debugOverridePieces;
  public FlockadeGamePieceConfiguration[] _allGamePieces;
  public CanvasGroup _canvasGroup;
  public FlockadePlayerBase _currentPlayer;
  public Queue<FlockadeGamePieceConfiguration> _debugOverridePiecesBag = new Queue<FlockadeGamePieceConfiguration>();
  public DG.Tweening.Sequence _entryOrExit;
  public Vector2 _originAnchoredPosition;
  public Vector3 _originScale;
  public UIMenuBase _parent;
  public FlockadePassiveManager _passiveManager;
  public RectTransform _rectTransform;
  public List<FlockadeGamePieceConfiguration> _remainingGamePieces = new List<FlockadeGamePieceConfiguration>();
  public float _width;

  public IEnumerable<FlockadeGamePieceChoice> Choices
  {
    get => (IEnumerable<FlockadeGamePieceChoice>) this._gamePieceChoices;
  }

  public IEnumerable<FlockadeGamePieceConfiguration> Content
  {
    get => (IEnumerable<FlockadeGamePieceConfiguration>) this._allGamePieces;
  }

  public bool DebugMode => false;

  public int Count => this._remainingGamePieces.Count;

  public int Total => this._allGamePieces.Length;

  public void Configure(
    IEnumerable<FlockadeGamePieceConfiguration> gamePiecesPool,
    FlockadeGamePieceInformation information,
    UIMenuBase parent,
    FlockadePassiveManager passiveManager)
  {
    this.Configure();
    this._allGamePieces = gamePiecesPool.ToArray<FlockadeGamePieceConfiguration>();
    this._parent = parent;
    this._passiveManager = passiveManager;
    foreach (FlockadeGamePieceChoice gamePieceChoice in this._gamePieceChoices)
      gamePieceChoice.Configure(information);
  }

  public override void OnLateConfigure()
  {
    this._width = this._rectTransform.rect.width;
    this.Exit().Complete(true);
  }

  public override void Awake()
  {
    base.Awake();
    this._canvasGroup = this.GetComponent<CanvasGroup>();
    this._rectTransform = this.GetComponent<RectTransform>();
    this._originAnchoredPosition = this._rectTransform.anchoredPosition;
    this._originScale = this._rectTransform.localScale;
  }

  public virtual void OnEnable()
  {
    this._container.ConfigurationChanged += new Action<bool>(this.Reconfigure);
    this.Selectable.onClick.AddListener(new UnityAction(this.ShowRemainingGamePieces));
  }

  public virtual void OnDisable()
  {
    this._container.ConfigurationChanged -= new Action<bool>(this.Reconfigure);
    this.Selectable.onClick.RemoveListener(new UnityAction(this.ShowRemainingGamePieces));
  }

  public void Reconfigure(bool isFlipped)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    FlockadeGamePieceBag.\u003C\u003Ec__DisplayClass36_0 cDisplayClass360 = new FlockadeGamePieceBag.\u003C\u003Ec__DisplayClass36_0();
    // ISSUE: reference to a compiler-generated field
    cDisplayClass360.isFlipped = isFlipped;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass360.\u003C\u003E4__this = this;
    if (this._entryOrExit != null && this._entryOrExit.IsActive() && !this._entryOrExit.IsComplete())
    {
      // ISSUE: reference to a compiler-generated method
      this._entryOrExit.OnComplete<DG.Tweening.Sequence>(new TweenCallback(cDisplayClass360.\u003CReconfigure\u003Eg__Reconfigure\u007C0));
    }
    else
    {
      // ISSUE: reference to a compiler-generated method
      cDisplayClass360.\u003CReconfigure\u003Eg__Reconfigure\u007C0();
    }
  }

  public void ShowRemainingGamePieces()
  {
    FlockadeGamePieceBagMenu menu = this._menuPrefab.Instantiate<FlockadeGamePieceBagMenu>();
    menu.Show(this, this._currentPlayer);
    this._parent.PushInstance<FlockadeGamePieceBagMenu>(menu);
  }

  public DG.Tweening.Sequence Enter()
  {
    float endValue = this._originAnchoredPosition.x + (this._container.IsFlipped ? -1f : 1f) * this._horizontalShiftToUnselect;
    DG.Tweening.Sequence entryOrExit = this._entryOrExit;
    if (entryOrExit != null)
      entryOrExit.Kill();
    this._entryOrExit = DOTween.Sequence().Append((Tween) this._canvasGroup.DOFade(1f, 0.5833333f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutQuad)).Join((Tween) this._rectTransform.DOAnchorPosX(endValue, 0.5833333f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutQuad));
    return this._entryOrExit;
  }

  public DG.Tweening.Sequence Exit()
  {
    float endValue = this._originAnchoredPosition.x - (float) ((this._container.IsFlipped ? -1.0 : 1.0) * ((double) this._width / 2.0 + (double) this._container.GetMargin(this._closestMargin)));
    DG.Tweening.Sequence entryOrExit = this._entryOrExit;
    if (entryOrExit != null)
      entryOrExit.Kill();
    this._entryOrExit = DOTween.Sequence().Append((Tween) this._canvasGroup.DOFade(0.0f, 0.5833333f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InQuad)).Join((Tween) this._rectTransform.DOAnchorPosX(endValue, 0.5833333f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.InQuad));
    return this._entryOrExit;
  }

  public override void OnSelect()
  {
    this._rectTransform.DOKill();
    this._rectTransform.DOAnchorPosX(this._originAnchoredPosition.x, 0.25f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutCubic);
  }

  public override void OnDeselect()
  {
    float endValue = this._originAnchoredPosition.x + (this._container.IsFlipped ? -1f : 1f) * this._horizontalShiftToUnselect;
    this._rectTransform.DOKill();
    this._rectTransform.DOAnchorPosX(endValue, 0.25f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.InCubic);
  }

  public DG.Tweening.Sequence Draw(FlockadePlayerBase player)
  {
    this._currentPlayer = player;
    DG.Tweening.Sequence s = DOTween.Sequence();
    int result;
    DG.Tweening.Sequence choiceAmountBonus = this._passiveManager.GetChoiceAmountBonus(player.Side, out result);
    if (choiceAmountBonus != null)
      s.Append((Tween) choiceAmountBonus);
    s.AppendInterval(0.0166666675f).AppendCallback((TweenCallback) (() => AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/pieces_appear")));
    for (int index1 = 0; (double) index1 < (double) Mathf.Max(1f, (float) (this._gamePieceChoices.Length + result)); ++index1)
    {
      if (this.Count != 0)
      {
        if (index1 == 0)
          player.PrefocusChoice(this._gamePieceChoices[index1]);
        int index2 = UnityEngine.Random.Range(0, this.Count);
        s.Join((Tween) this._gamePieceChoices[index1].GamePiece.Set(this._remainingGamePieces[index2], true).Join((Tween) this._gamePieceChoices[index1].SetActive(true)).PrependInterval((float) index1 * 0.06666667f));
        this._remainingGamePieces.RemoveAt(index2);
      }
    }
    return s;
  }

  public void Refill()
  {
    this._remainingGamePieces.Clear();
    this._remainingGamePieces.AddRange((IEnumerable<FlockadeGamePieceConfiguration>) this._allGamePieces);
  }

  public DG.Tweening.Sequence ReturnUnpicked()
  {
    DG.Tweening.Sequence s = DOTween.Sequence().AppendInterval(0.0166666675f);
    for (int index = 0; index < this._gamePieceChoices.Length; ++index)
    {
      FlockadeGamePieceChoice gamePieceChoice = this._gamePieceChoices[index];
      int num = gamePieceChoice.Picked ? 1 : 0;
      IFlockadeGamePiece.State gamePiece;
      s.Join((Tween) gamePieceChoice.GamePiece.Pop(out gamePiece).PrependInterval((float) index * 0.06666667f));
      if (num == 0)
        this._remainingGamePieces.Add(gamePiece.Configuration);
    }
    return s;
  }
}
