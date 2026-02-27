// Decompiled with JetBrains decompiler
// Type: Lamb.UI.AdventureMapNode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Map;
using MMBiomeGeneration;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class AdventureMapNode : 
  BaseMonoBehaviour,
  ISelectHandler,
  IEventSystemHandler,
  IDeselectHandler,
  IPoolListener
{
  private const float kHoverScaleFactor = 1.066f;
  public Action<AdventureMapNode> OnNodeSelected;
  public Action<AdventureMapNode> OnNodeDeselected;
  public Action<AdventureMapNode> OnNodeChosen;
  [Header("General Components")]
  [SerializeField]
  private RectTransform _rectTransform;
  [SerializeField]
  private MMButton _button;
  [SerializeField]
  private RectTransform _shakeContainer;
  [SerializeField]
  private CanvasGroup _canvasGroup;
  [Header("Main")]
  [SerializeField]
  private Image _icon;
  [SerializeField]
  private Image _selected;
  [SerializeField]
  private Image _whiteFill;
  [SerializeField]
  private Image _lockedFill;
  [SerializeField]
  private Image _imageOutline;
  [SerializeField]
  private Image _selectionIcon;
  [SerializeField]
  private GameObject _notification;
  [SerializeField]
  private CanvasGroup _startingIconCanvasGroup;
  [Header("Materials")]
  [SerializeField]
  private Material _normalOutline;
  [SerializeField]
  private Material _unselectedOutline;
  [SerializeField]
  private Material _selectedOutline;
  [SerializeField]
  private Material _completedOutline;
  [SerializeField]
  private Material _specialOutline;
  [Header("Flair")]
  [SerializeField]
  private Image _flairImage;
  [Header("Modifier")]
  [SerializeField]
  private Image _modifierIcon;
  [SerializeField]
  private CanvasGroup _modifierCanvasGroup;
  [Header("Sprites")]
  [SerializeField]
  private Sprite _questionMarkSprite;
  [SerializeField]
  private Sprite _potentialSelectionSprite;
  [SerializeField]
  private Sprite _selectedSelectionSprite;
  private Node _mapNode;
  private NodeStates _state;
  private float _initialScale;

  public RectTransform RectTransform => this._rectTransform;

  public MMButton Button => this._button;

  public Node MapNode => this._mapNode;

  public NodeBlueprint NodeBlueprint => this._mapNode.blueprint;

  public DungeonModifier Modifier => this._mapNode.Modifier;

  public NodeStates State => this._state;

  public CanvasGroup CanvasGroup => this._canvasGroup;

  private void Awake()
  {
    this._button.onClick.AddListener(new UnityAction(this.OnNodeClicked));
    this._button.OnConfirmDenied += new System.Action(this.Shake);
  }

  public void Configure(Node mapNode)
  {
    this._mapNode = mapNode;
    this._modifierCanvasGroup.alpha = 0.0f;
    this._selectionIcon.enabled = false;
    this._startingIconCanvasGroup.alpha = 0.0f;
    this._selected.enabled = false;
    this._icon.sprite = this.NodeBlueprint.GetSprite(this._mapNode.DungeonLocation);
    if (this._mapNode.nodeType == NodeType.Boss)
      this.transform.localScale *= 1.5f;
    this._initialScale = this._icon.transform.localScale.x;
    this.SetState(NodeStates.Locked);
    if ((UnityEngine.Object) this.NodeBlueprint.flair != (UnityEngine.Object) null)
    {
      this._flairImage.enabled = true;
      this._flairImage.sprite = this.NodeBlueprint.flair;
    }
    else
      this._flairImage.enabled = false;
    if (this.NodeBlueprint.CanHaveModifier && (bool) (UnityEngine.Object) this.Modifier && !this._mapNode.Hidden)
    {
      this._modifierCanvasGroup.alpha = 1f;
      this._modifierIcon.sprite = this.Modifier.modifierIcon;
      this._modifierCanvasGroup.gameObject.transform.localPosition = new Vector3(0.0f, 100f, 0.0f);
      this._modifierCanvasGroup.gameObject.transform.DOLocalMove(Vector3.zero, 0.5f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InQuart);
    }
    if (this.NodeBlueprint.ForceDisplayModifier && !this._mapNode.Hidden)
    {
      this._modifierCanvasGroup.alpha = 1f;
      this._modifierIcon.sprite = this.NodeBlueprint.ForceDisplayModifierIcon;
      this._modifierCanvasGroup.gameObject.transform.localPosition = new Vector3(0.0f, 100f, 0.0f);
      this._modifierCanvasGroup.gameObject.transform.DOLocalMove(Vector3.zero, 0.5f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InQuart);
    }
    if (this._mapNode.nodeType == NodeType.None)
      this.gameObject.SetActive(false);
    if (this._mapNode.Hidden)
      this._icon.sprite = this._questionMarkSprite;
    if (this.NodeBlueprint.nodeType == NodeType.Follower_Easy || this.NodeBlueprint.nodeType == NodeType.Follower_Medium || this.NodeBlueprint.nodeType == NodeType.Follower_Hard)
    {
      foreach (ObjectivesData objective in DataManager.Instance.Objectives)
      {
        if (objective.Type == Objectives.TYPES.FIND_FOLLOWER && ((Objectives_FindFollower) objective).TargetLocation == BiomeGenerator.Instance.DungeonLocation)
        {
          this.ShowNotification(true);
          break;
        }
      }
    }
    this._button.SetInteractionState(true);
  }

  public void SetStartingNode()
  {
    Debug.Log((object) "Set Starting Node Sprite");
    this._startingIconCanvasGroup.alpha = 1f;
  }

  public void SetState(NodeStates state, bool changeAppearance = true)
  {
    this.ScaleOutline(0.01f, Vector3.one, new Vector3(0.925f, 0.925f));
    this._lockedFill.enabled = false;
    switch (state)
    {
      case NodeStates.Locked:
        if (changeAppearance)
        {
          if ((UnityEngine.Object) this.NodeBlueprint.flair != (UnityEngine.Object) null)
            this._imageOutline.material = this._specialOutline;
          else
            this._imageOutline.material = this._normalOutline;
          this._imageOutline.color = new Color(1f, 1f, 1f, 0.75f);
          this._icon.DOKill();
          this._icon.color = UIAdventureMapOverlayController.LockedColourLight;
          this._lockedFill.enabled = true;
          break;
        }
        break;
      case NodeStates.Visited:
        this._imageOutline.material = this._completedOutline;
        this._imageOutline.color = Color.white;
        this._icon.DOKill();
        this._icon.color = UIAdventureMapOverlayController.VisitedColour;
        this._icon.sprite = this.NodeBlueprint.GetSprite(this.MapNode.DungeonLocation);
        break;
      case NodeStates.Attainable:
        this._imageOutline.material = this._unselectedOutline;
        this._imageOutline.color = Color.white;
        this._selectionIcon.enabled = true;
        this._icon.color = UIAdventureMapOverlayController.LockedColourLight;
        this._icon.DOKill();
        DOTweenModuleUI.DOColor(this._icon, Color.white, 0.5f).SetLoops<TweenerCore<Color, Color, ColorOptions>>(-1, DG.Tweening.LoopType.Yoyo).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
        break;
    }
    this._state = state;
    this._button.Confirmable = this._state == NodeStates.Attainable;
    if (state != NodeStates.Visited)
      return;
    this.ShowNotification(false);
  }

  public void ShowSwirlAnimation()
  {
    this._whiteFill.color = Color.white;
    DOTweenModuleUI.DOFade(this._whiteFill, 0.0f, 1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    this.gameObject.transform.DOScale(Vector3.one * 1.05f, 1f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack);
  }

  public void ShowNotification(bool show) => this._notification.SetActive(show);

  private void OnNodeClicked()
  {
    Action<AdventureMapNode> onNodeChosen = this.OnNodeChosen;
    if (onNodeChosen != null)
      onNodeChosen(this);
    this.OnSelect((BaseEventData) null);
  }

  public void OnSelect(BaseEventData eventData)
  {
    this.transform.DOScale(1.1f, 0.5f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart);
    this._imageOutline.material = this._selectedOutline;
    this.ScaleOutline(0.125f, new Vector3(0.925f, 0.925f), Vector3.one);
    if (this._state == NodeStates.Attainable)
    {
      this._selectionIcon.enabled = true;
      this._selectionIcon.sprite = this._selectedSelectionSprite;
      this.SelectionIconFade(1.5f);
    }
    this._selected.enabled = true;
    this._icon.transform.DOKill();
    this._icon.transform.DOScale(this._initialScale * 1.066f, 0.3f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    Action<AdventureMapNode> onNodeSelected = this.OnNodeSelected;
    if (onNodeSelected == null)
      return;
    onNodeSelected(this);
  }

  public void OnDeselect(BaseEventData eventData)
  {
    this.transform.DOScale(0.9f, 0.5f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart);
    this.ScaleOutline(0.01f, Vector3.one, new Vector3(0.925f, 0.925f));
    if (this._state == NodeStates.Locked)
    {
      if ((UnityEngine.Object) this.NodeBlueprint.flair != (UnityEngine.Object) null)
        this._imageOutline.material = this._specialOutline;
      else
        this._imageOutline.material = this._normalOutline;
    }
    else if (this._state == NodeStates.Visited)
      this._imageOutline.material = this._completedOutline;
    else
      this._imageOutline.material = this._unselectedOutline;
    if (this._state == NodeStates.Attainable)
    {
      this._selectionIcon.enabled = true;
      this._selectionIcon.sprite = this._potentialSelectionSprite;
      this.SelectionIconFade(1f);
    }
    this._selected.enabled = false;
    this._icon.transform.DOKill();
    this._icon.transform.DOScale(this._initialScale, 0.3f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart);
    Action<AdventureMapNode> onNodeDeselected = this.OnNodeDeselected;
    if (onNodeDeselected == null)
      return;
    onNodeDeselected(this);
  }

  private void SelectionIconFade(float scale)
  {
    Transform transform = this._selectionIcon.transform;
    transform.DOKill();
    transform.DOScale(new Vector3(scale, scale), 0.3f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart);
    this._selectionIcon.DOKill();
    this._selectionIcon.color = new Color(1f, 1f, 1f, 0.0f);
    DOTweenModuleUI.DOFade(this._selectionIcon, 1f, 0.3f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutQuart);
  }

  public void Shake()
  {
    this._shakeContainer.DOKill();
    this._shakeContainer.anchoredPosition = Vector2.zero;
    this._shakeContainer.DOShakePosition(1f, new Vector3(10f, 0.0f)).SetUpdate<Tweener>(true);
  }

  public void Punch()
  {
    this._whiteFill.color = Color.white;
    DOTweenModuleUI.DOFade(this._whiteFill, 0.0f, 1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    this._rectTransform.DOPunchScale(Vector3.one * 1.25f, 0.5f, 1).SetUpdate<Tweener>(true);
  }

  public void ScaleIn()
  {
    this._whiteFill.color = Color.white;
    DOTweenModuleUI.DOFade(this._whiteFill, 0.0f, 1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    this._rectTransform.localScale = Vector3.one * 2f;
    this._rectTransform.DOScale(Vector3.one, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
  }

  private void ScaleOutline(float duration, Vector3 from, Vector3 to)
  {
    Transform transform = this._imageOutline.gameObject.transform;
    transform.DOKill();
    transform.localScale = from;
    transform.DOScale(to, duration).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InQuart);
  }

  public void OnRecycled()
  {
    this._button.navigation = this._button.navigation with
    {
      selectOnLeft = (Selectable) null,
      selectOnRight = (Selectable) null,
      mode = Navigation.Mode.Automatic
    };
    this.OnNodeSelected = (Action<AdventureMapNode>) null;
    this.OnNodeDeselected = (Action<AdventureMapNode>) null;
    this.OnNodeChosen = (Action<AdventureMapNode>) null;
  }
}
