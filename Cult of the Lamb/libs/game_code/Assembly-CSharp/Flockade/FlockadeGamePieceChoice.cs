// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeGamePieceChoice
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Flockade;

[RequireComponent(typeof (RectTransform))]
public class FlockadeGamePieceChoice : FlockadeGamePieceHolder
{
  public const float _ENTRY_OR_EXIT_DURATION = 0.333333343f;
  public const float _SELECTION_CHANGE_DURATION = 0.25f;
  public const Ease _ENTRY_EASING = Ease.OutCubic;
  public const Ease _EXIT_EASING = Ease.InCubic;
  public const string _AVAILABLE_GAME_PIECES_DISAPPEARANCE_SOUND = "event:/dlc/ui/flockade_minigame/pieces_hide";
  [Header("Game Piece Choice Specifics")]
  [SerializeField]
  [Range(0.0f, 1f)]
  public float _hiddenVerticalRatio;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float _selectedVerticalRatio;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float _unselectedVerticalRatio;
  [SerializeField]
  public Image _gamePieceHighlight;
  [SerializeField]
  public GameObject _withBlessing;
  [SerializeField]
  public GameObject _withoutBlessing;
  [SerializeField]
  public FlockadeBottomContainer _container;
  public bool _entered;
  public DG.Tweening.Sequence _entryOrExit;
  public Vector3 _originGamePieceScale;
  public RectTransform _rectTransform;
  public bool _selected;
  [CompilerGenerated]
  public bool \u003CPicked\u003Ek__BackingField;

  public override RectTransform GamePieceContainer => this._rectTransform;

  public bool Picked
  {
    get => this.\u003CPicked\u003Ek__BackingField;
    set => this.\u003CPicked\u003Ek__BackingField = value;
  }

  public new void Configure(FlockadeGamePieceInformation information)
  {
    base.Configure(information);
  }

  public override void Awake()
  {
    base.Awake();
    this._rectTransform = this.GetComponent<RectTransform>();
    this._originGamePieceScale = this._gamePiece.RectTransform.localScale;
  }

  public virtual void OnEnable()
  {
    this._container.ConfigurationChanged += new Action<bool>(this.Reconfigure);
  }

  public virtual void OnDisable()
  {
    this._container.ConfigurationChanged -= new Action<bool>(this.Reconfigure);
  }

  public void Reconfigure(bool isFlipped)
  {
    this._gamePiece.RectTransform.localScale = isFlipped ? new Vector3(-this._originGamePieceScale.x, this._originGamePieceScale.y, this._originGamePieceScale.z) : this._originGamePieceScale;
  }

  public override DG.Tweening.Sequence Enter(bool killOtherAnimations = true)
  {
    this.Picked = false;
    if (killOtherAnimations)
    {
      DG.Tweening.Sequence entryOrExit = this._entryOrExit;
      if (entryOrExit != null)
        entryOrExit.Kill();
    }
    this._entryOrExit = DOTween.Sequence().AppendInterval(0.0166666675f).AppendCallback((TweenCallback) (() =>
    {
      this._gamePieceHighlight.sprite = this.GamePiece.Configuration.Outline;
      bool active = this.GamePiece.Blessing.Active;
      this._withoutBlessing.SetActive(!active);
      this._withBlessing.SetActive(active);
    })).Append((Tween) this.GamePieceContainer.DOAnchorPosY((float) (-(double) this.Height * (this._selected ? (double) this._selectedVerticalRatio : (double) this._unselectedVerticalRatio)), 0.333333343f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutCubic)).AppendCallback((TweenCallback) (() => this._entered = true));
    return this._entryOrExit;
  }

  public override DG.Tweening.Sequence Exit(bool killOtherAnimations = true)
  {
    this._entered = false;
    this.Picked = true;
    if (killOtherAnimations)
    {
      DG.Tweening.Sequence entryOrExit = this._entryOrExit;
      if (entryOrExit != null)
        entryOrExit.Kill();
    }
    this._entryOrExit = DOTween.Sequence().AppendCallback((TweenCallback) (() => AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/pieces_hide"))).Append((Tween) this.GamePieceContainer.DOAnchorPosY(-this.Height * this._hiddenVerticalRatio, 0.333333343f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.InCubic));
    return this._entryOrExit;
  }

  public override void OnSelect()
  {
    base.OnSelect();
    this._selected = true;
    if (!this._entered)
      return;
    this.GamePieceContainer.DOKill();
    this.GamePieceContainer.DOAnchorPosY(-this.Height * this._selectedVerticalRatio, 0.25f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutCubic);
  }

  public override void OnDeselect()
  {
    base.OnDeselect();
    this._selected = false;
    if (!this._entered)
      return;
    this.GamePieceContainer.DOKill();
    this.GamePieceContainer.DOAnchorPosY(-this.Height * this._unselectedVerticalRatio, 0.25f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.InCubic);
  }

  [CompilerGenerated]
  public void \u003CEnter\u003Eb__28_0()
  {
    this._gamePieceHighlight.sprite = this.GamePiece.Configuration.Outline;
    bool active = this.GamePiece.Blessing.Active;
    this._withoutBlessing.SetActive(!active);
    this._withBlessing.SetActive(active);
  }

  [CompilerGenerated]
  public void \u003CEnter\u003Eb__28_1() => this._entered = true;
}
