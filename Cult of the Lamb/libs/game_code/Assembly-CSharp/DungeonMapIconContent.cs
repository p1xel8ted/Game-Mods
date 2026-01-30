// Decompiled with JetBrains decompiler
// Type: DungeonMapIconContent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class DungeonMapIconContent : MonoBehaviour
{
  public const float kHoverScaleFactor = 1.066f;
  [Header("General")]
  [SerializeField]
  public DungeonWorldMapIcon _dungeonWorldMapIcon;
  [SerializeField]
  public RectTransform _shakeContainer;
  [SerializeField]
  public RectTransform _selectIconTransform;
  public CanvasGroup _canvasGroup;
  [Header("Images")]
  [SerializeField]
  public Image _icon;
  [SerializeField]
  public Image _iconOutline;
  [SerializeField]
  public Image _imageOutline;
  [SerializeField]
  public Image _selectionIcon;
  [SerializeField]
  public Image _selected;
  [SerializeField]
  public Image _lockedFill;
  [SerializeField]
  public GameObject _notification;
  [SerializeField]
  public GameObject _portalEffect;
  [Header("Materials")]
  [SerializeField]
  public Material _normalOutline;
  [SerializeField]
  public Material _unselectedOutline;
  [SerializeField]
  public Material _selectedOutline;
  [SerializeField]
  public Material _completedOutline;
  [SerializeField]
  public Material _specialOutline;
  [Header("Sprites")]
  [SerializeField]
  public Sprite _questionMarkSprite;
  [SerializeField]
  public Sprite _potentialSelectionSprite;
  [SerializeField]
  public Sprite _selectedSelectionSprite;
  [SerializeField]
  public DungeonWorldMapIcon.IconState _state;
  public Vector3 _initialScale;
  [SerializeField]
  public bool _isHighlighted;
  public List<DungeonWorldMapIcon.NodeType> ignoreColorChanges = new List<DungeonWorldMapIcon.NodeType>()
  {
    DungeonWorldMapIcon.NodeType.Base,
    DungeonWorldMapIcon.NodeType.Door,
    DungeonWorldMapIcon.NodeType.Lock,
    DungeonWorldMapIcon.NodeType.Lock_2,
    DungeonWorldMapIcon.NodeType.Lock_3
  };

  public DungeonWorldMapIcon.NodeType NodeType => this._dungeonWorldMapIcon.Type;

  public bool IsBoss
  {
    get
    {
      DungeonWorldMapIcon.NodeType nodeType = this.NodeType;
      int num;
      switch (nodeType)
      {
        case DungeonWorldMapIcon.NodeType.Dungeon5_Boss:
        case DungeonWorldMapIcon.NodeType.Yngya:
          num = 0;
          break;
        default:
          num = nodeType != DungeonWorldMapIcon.NodeType.Dungeon6_Boss ? 1 : 0;
          break;
      }
      return num == 0;
    }
  }

  public void Awake()
  {
    this._initialScale = this.transform.localScale;
    this._dungeonWorldMapIcon.OnNodeSelected += new Action<DungeonWorldMapIcon.NodeType>(this.OnNodeSelected);
    this._dungeonWorldMapIcon.OnNodeDeselected += new Action<DungeonWorldMapIcon.NodeType>(this.OnNodeUnselected);
    this._iconOutline.gameObject.SetActive(false);
  }

  public void OnNodeUnselected(DungeonWorldMapIcon.NodeType nodeType)
  {
    this._isHighlighted = false;
    Debug.Log((object) $"[Deselected] {nodeType}");
    this.transform.DOScale(0.9f, 0.5f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart);
    this.ScaleOutline(0.01f, Vector3.one, new Vector3(0.925f, 0.925f));
    if (this._state == DungeonWorldMapIcon.IconState.Locked)
      this._imageOutline.material = this._normalOutline;
    else if (this._state == DungeonWorldMapIcon.IconState.Completed)
      this._imageOutline.material = this._completedOutline;
    else
      this._imageOutline.material = this._unselectedOutline;
    this._selectionIcon.enabled = true;
    this._selectionIcon.sprite = this._potentialSelectionSprite;
    this.SelectionIconFade(1f);
    this._selected.enabled = false;
  }

  public void OnNodeSelected(DungeonWorldMapIcon.NodeType nodeType)
  {
    this._isHighlighted = true;
    Debug.Log((object) $"[Selected] {nodeType}");
    this._shakeContainer.DOKill();
    this._shakeContainer.DOShakeAnchorPos(0.25f, (Vector2) new Vector3(1f, -1f), 20).SetUpdate<Tweener>(true);
    this.transform.DOScale(1.1f, 0.5f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart);
    this._imageOutline.material = this._selectedOutline;
    this.ScaleOutline(0.125f, new Vector3(0.925f, 0.925f), Vector3.one);
    this._selectionIcon.enabled = true;
    this._selectionIcon.sprite = this._selectedSelectionSprite;
    this.SelectionIconFade(1.5f);
    this._selected.enabled = true;
    this._icon.transform.DOKill();
    this._icon.transform.DOScale(this._initialScale * 1.066f, 0.3f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
  }

  public void SelectionIconFade(float scale)
  {
    Transform transform = this._selectionIcon.transform;
    transform.DOKill();
    transform.DOScale(new Vector3(scale, scale), 0.3f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart);
    this._selectionIcon.DOKill();
    this._selectionIcon.color = new Color(1f, 1f, 1f, 0.0f);
    DOTweenModuleUI.DOFade(this._selectionIcon, 1f, 0.3f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutQuart);
  }

  public void ScaleOutline(float duration, Vector3 from, Vector3 to)
  {
    Transform transform = this._imageOutline.gameObject.transform;
    transform.DOKill();
    transform.localScale = from;
    transform.DOScale(to, duration).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InQuart);
  }

  public void Configure(
    DungeonWorldMapIcon.NodeType nodeType,
    DungeonWorldMapIcon.IconActionType selectAction,
    DungeonWorldMapIcon.IconState state)
  {
    if ((UnityEngine.Object) this._icon == (UnityEngine.Object) null)
      Debug.Log((object) ("Icon = null" + this.gameObject.transform.parent.name));
    this.SetState(nodeType, state, selectAction);
  }

  public void SetState(
    DungeonWorldMapIcon.NodeType nodeType,
    DungeonWorldMapIcon.IconState state,
    DungeonWorldMapIcon.IconActionType selectAction,
    bool changeAppearance = true)
  {
    if (this._dungeonWorldMapIcon.SelectAction == DungeonWorldMapIcon.IconActionType.Nothing)
      state = DungeonWorldMapIcon.IconState.Completed;
    int num1 = nodeType == DungeonWorldMapIcon.NodeType.Key || nodeType == DungeonWorldMapIcon.NodeType.Key_2 ? 1 : (nodeType == DungeonWorldMapIcon.NodeType.Key_3 ? 1 : 0);
    bool flag = nodeType == DungeonWorldMapIcon.NodeType.Lock || nodeType == DungeonWorldMapIcon.NodeType.Lock_2 || nodeType == DungeonWorldMapIcon.NodeType.Lock_3;
    this.ScaleOutline(0.01f, Vector3.one, new Vector3(0.925f, 0.925f));
    this._lockedFill.enabled = false;
    this._selectionIcon.enabled = true;
    this._icon.enabled = true;
    this._imageOutline.enabled = true;
    this._notification.gameObject.SetActive(false);
    this._imageOutline.gameObject.SetActive(true);
    this._canvasGroup.alpha = 1f;
    int num2 = flag ? 1 : 0;
    if ((num1 | num2) != 0)
      this._iconOutline.gameObject.SetActive(true);
    else
      this._iconOutline.gameObject.SetActive(false);
    if (flag && state >= DungeonWorldMapIcon.IconState.Preview)
      return;
    if ((UnityEngine.Object) this._portalEffect != (UnityEngine.Object) null && this.IsBoss)
      this._portalEffect.SetActive(false);
    switch (state)
    {
      case DungeonWorldMapIcon.IconState.None:
        this._canvasGroup.alpha = 0.0f;
        break;
      case DungeonWorldMapIcon.IconState.Unrevealed:
        this._icon.enabled = false;
        this._selectionIcon.enabled = false;
        this._imageOutline.enabled = false;
        this._notification.gameObject.SetActive(false);
        this._iconOutline.gameObject.SetActive(false);
        this._canvasGroup.alpha = 0.5f;
        break;
      case DungeonWorldMapIcon.IconState.Preview:
        this._imageOutline.enabled = false;
        this._selectionIcon.enabled = false;
        this._icon.color = UIAdventureMapOverlayController.LockedColourLight;
        this._icon.DOKill();
        this._icon.color = Color.gray;
        break;
      case DungeonWorldMapIcon.IconState.Selectable:
        this._imageOutline.material = this._unselectedOutline;
        this._imageOutline.color = Color.white;
        this._selectionIcon.enabled = true;
        this._icon.color = UIAdventureMapOverlayController.LockedColourLight;
        if ((UnityEngine.Object) this._portalEffect != (UnityEngine.Object) null && this.IsBoss)
          this._portalEffect.SetActive(true);
        this._icon.DOKill();
        DOTweenModuleUI.DOColor(this._icon, Color.white, 0.5f).SetLoops<TweenerCore<Color, Color, ColorOptions>>(-1, DG.Tweening.LoopType.Yoyo).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
        break;
      case DungeonWorldMapIcon.IconState.Completed:
        this._imageOutline.material = this._completedOutline;
        this._imageOutline.color = Color.white;
        this._icon.DOKill();
        if (!this.ignoreColorChanges.Contains(nodeType))
        {
          this._icon.color = UIAdventureMapOverlayController.VisitedColour;
          break;
        }
        break;
      case DungeonWorldMapIcon.IconState.Locked:
        this._imageOutline.material = this._normalOutline;
        this._imageOutline.color = new Color(1f, 1f, 1f, 0.75f);
        this._icon.DOKill();
        this._icon.color = UIAdventureMapOverlayController.LockedColourLight;
        this._lockedFill.enabled = true;
        break;
      default:
        Debug.Log((object) ("Something gone wrong" + this.gameObject.transform.parent.name));
        this._icon.color = Color.magenta;
        break;
    }
    this._state = state;
    if (state == DungeonWorldMapIcon.IconState.Completed)
      this.ShowNotification(false);
    if (!this._isHighlighted)
      return;
    this.OnNodeSelected(nodeType);
  }

  public void ShowNotification(bool show) => this._notification.SetActive(show);

  public void Shake()
  {
    this._shakeContainer.DOKill();
    this._shakeContainer.anchoredPosition = Vector2.zero;
    this._shakeContainer.DOShakePosition(1f, new Vector3(10f, 0.0f)).SetUpdate<Tweener>(true);
  }

  public void PopIn()
  {
    this.transform.localScale = Vector3.zero;
    this.transform.DOKill();
    this.transform.DOScale(1f, 0.5f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBounce);
  }

  public void Punch()
  {
    this.transform.DOKill();
    this.transform.DOPunchScale(Vector3.one * 0.5f, 0.25f, elasticity: 10f).SetUpdate<Tweener>(true);
  }
}
