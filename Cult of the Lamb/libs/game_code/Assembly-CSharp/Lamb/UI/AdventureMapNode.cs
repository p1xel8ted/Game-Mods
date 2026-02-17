// Decompiled with JetBrains decompiler
// Type: Lamb.UI.AdventureMapNode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Map;
using MMBiomeGeneration;
using System;
using System.Collections;
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
  public const float kHoverScaleFactor = 1.066f;
  public Action<AdventureMapNode> OnNodeSelected;
  public Action<AdventureMapNode> OnNodeDeselected;
  public Action<AdventureMapNode> OnNodeChosen;
  [Header("General Components")]
  [SerializeField]
  public RectTransform _rectTransform;
  [SerializeField]
  public MMButton _button;
  [SerializeField]
  public RectTransform _shakeContainer;
  [SerializeField]
  public CanvasGroup _canvasGroup;
  [Header("Main")]
  [SerializeField]
  public Image _icon;
  [SerializeField]
  public Image _selected;
  [SerializeField]
  public Image _whiteFill;
  [SerializeField]
  public Image _lockedFill;
  [SerializeField]
  public Image _imageOutline;
  [SerializeField]
  public Image _selectionIcon;
  [SerializeField]
  public GameObject _notification;
  [SerializeField]
  public CanvasGroup _startingIconCanvasGroup;
  [SerializeField]
  public GameObject breakParticle;
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
  [Header("Flair")]
  [SerializeField]
  public Image _flairImage;
  [Header("Modifier")]
  [SerializeField]
  public Image _modifierIcon;
  [SerializeField]
  public CanvasGroup _modifierCanvasGroup;
  [Header("Sprites")]
  [SerializeField]
  public Sprite _questionMarkSprite;
  [SerializeField]
  public Sprite _potentialSelectionSprite;
  [SerializeField]
  public Sprite _selectedSelectionSprite;
  [Header("DLC")]
  [SerializeField]
  public Sprite _rescueRoomSprite;
  public Node _mapNode;
  public NodeStates _state;
  public float _initialScale;
  public Map.NodeType NodeType;

  public RectTransform RectTransform => this._rectTransform;

  public MMButton Button => this._button;

  public Node MapNode => this._mapNode;

  public NodeBlueprint NodeBlueprint
  {
    get
    {
      return DataManager.Instance.GetVariable(this._mapNode.blueprint.UnlockedVariable.Variable) == this._mapNode.blueprint.UnlockedVariable.Condition ? this._mapNode.blueprint.UnlockedBlueprint : this._mapNode.blueprint;
    }
  }

  public DungeonModifier Modifier => this._mapNode.Modifier;

  public NodeStates State => this._state;

  public CanvasGroup CanvasGroup => this._canvasGroup;

  public void Awake()
  {
    this._button.onClick.AddListener(new UnityAction(this.OnNodeClicked));
    this._button.OnConfirmDenied += new System.Action(this.Shake);
  }

  public void Configure(Node mapNode)
  {
    bool flag1 = DataManager.Instance.PlayerFleece == 13;
    this._mapNode = mapNode;
    this._modifierCanvasGroup.alpha = 0.0f;
    this._selectionIcon.enabled = false;
    this._startingIconCanvasGroup.alpha = 0.0f;
    this._selected.enabled = false;
    this.NodeType = mapNode.nodeType;
    this._icon.sprite = this.NodeBlueprint.GetSprite(this._mapNode.DungeonLocation);
    if ((PlayerFarming.Location == FollowerLocation.Dungeon1_5 || PlayerFarming.Location == FollowerLocation.Dungeon1_6) && this._mapNode.nodeType == Map.NodeType.MiniBossFloor && DataManager.Instance.IsLambGhostRescue)
      this._icon.sprite = this._rescueRoomSprite;
    if (this._mapNode.nodeType == Map.NodeType.Boss)
      this.transform.localScale *= 1.5f;
    this._initialScale = this._icon.transform.localScale.x;
    this.SetState(NodeStates.Locked);
    if ((UnityEngine.Object) this.NodeBlueprint.flair != (UnityEngine.Object) null && !flag1)
    {
      this._flairImage.enabled = true;
      this._flairImage.sprite = this.NodeBlueprint.flair;
    }
    else
      this._flairImage.enabled = false;
    if (this.NodeBlueprint.CanHaveModifier && !flag1 && (bool) (UnityEngine.Object) this.Modifier && !this._mapNode.Hidden)
    {
      this._modifierCanvasGroup.alpha = 1f;
      this._modifierIcon.sprite = this.Modifier.modifierIcon;
      this._modifierCanvasGroup.gameObject.transform.localPosition = new Vector3(0.0f, 100f, 0.0f);
      this._modifierCanvasGroup.gameObject.transform.DOLocalMove(Vector3.zero, 0.5f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InQuart);
    }
    if (this.NodeBlueprint.ForceDisplayModifier && !this._mapNode.Hidden && !flag1)
    {
      this._modifierCanvasGroup.alpha = 1f;
      this._modifierIcon.sprite = this.NodeBlueprint.ForceDisplayModifierIcon;
      this._modifierCanvasGroup.gameObject.transform.localPosition = new Vector3(0.0f, 100f, 0.0f);
      this._modifierCanvasGroup.gameObject.transform.DOLocalMove(Vector3.zero, 0.5f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InQuart);
    }
    if (this._mapNode.nodeType == Map.NodeType.None)
      this.gameObject.SetActive(false);
    if (this._mapNode.Hidden | flag1)
      this._icon.sprite = this._questionMarkSprite;
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      if (this.NodeBlueprint.nodeType == Map.NodeType.Special_FindRelic)
      {
        if (objective.Type == Objectives.TYPES.FIND_RELIC && ((Objective_FindRelic) objective).TargetLocation == BiomeGenerator.Instance.DungeonLocation)
          this.ShowNotification(true);
      }
      else if ((this.NodeBlueprint.nodeType == Map.NodeType.Follower_Beginner || this.NodeBlueprint.nodeType == Map.NodeType.Follower_Easy || this.NodeBlueprint.nodeType == Map.NodeType.Follower_Medium || this.NodeBlueprint.nodeType == Map.NodeType.Follower_Hard) && objective.Type == Objectives.TYPES.FIND_FOLLOWER && ((Objectives_FindFollower) objective).TargetLocation == BiomeGenerator.Instance.DungeonLocation)
        this.ShowNotification(true);
    }
    if (this.NodeBlueprint.nodeType == Map.NodeType.Special_Healing && BiomeGenerator.Instance.DungeonLocation == FollowerLocation.Dungeon1_6 && Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.ILLEGIBLE_LETTER_SCYLLA) > 0)
      this.ShowNotification(true);
    if (this.NodeBlueprint.nodeType == Map.NodeType.Special_Healing && BiomeGenerator.Instance.DungeonLocation == FollowerLocation.Dungeon1_5 && (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.ILLEGIBLE_LETTER_CHARYBDIS) > 0 || Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.FISHING_ROD) > 0))
      this.ShowNotification(true);
    if (this.NodeBlueprint.nodeType == Map.NodeType.MarketPlaceClothes && !DataManager.Instance.LeftBopAtTailor && Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BOP) > 0 && PlayerFarming.Location == FollowerLocation.Dungeon1_5)
      this.ShowNotification(true);
    if (this.NodeBlueprint.nodeType == Map.NodeType.MarketPlaceCat)
    {
      int num = DataManager.Instance.HasReturnedBaal ? 1 : 0;
      bool hasReturnedBaal = DataManager.Instance.HasReturnedBaal;
      bool flag2 = DataManager.Instance.Followers_Demons_Types.Contains(6);
      bool flag3 = DataManager.Instance.Followers_Demons_Types.Contains(7);
      if ((num == 0 & flag2 || !hasReturnedBaal & flag3) && !DungeonSandboxManager.Active)
        this.ShowNotification(true);
    }
    if (this.NodeBlueprint.nodeType == Map.NodeType.MarketplaceBlacksmith && DataManager.Instance.PalworldEggsCollected <= 0)
      this.ShowNotification(true);
    if (this.NodeBlueprint.nodeType == Map.NodeType.MarketPlaceClothes && !DataManager.Instance.RevealedTailor && PlayerFarming.Location == FollowerLocation.Dungeon1_4)
      this.ShowNotification(true);
    if (this.NodeBlueprint.nodeType == Map.NodeType.Cotton && !DataManager.Instance.EnteredCottonRoom || this.NodeBlueprint.nodeType == Map.NodeType.Grapes && !DataManager.Instance.EnteredGrapeRoom || this.NodeBlueprint.nodeType == Map.NodeType.Hops && !DataManager.Instance.EnteredHopRoom)
      this.ShowNotification(true);
    if (this.NodeBlueprint.nodeType != Map.NodeType.Follower_Beginner && this.NodeBlueprint.nodeType != Map.NodeType.Follower_Easy && this.NodeBlueprint.nodeType != Map.NodeType.Follower_Hard && this.NodeBlueprint.nodeType != Map.NodeType.Follower_Medium || !DataManager.Instance.GiveExecutionerFollower)
      return;
    this.ShowNotification(true);
  }

  public void SetStartingNode()
  {
    Debug.Log((object) "Set Starting Node Sprite");
    this._startingIconCanvasGroup.alpha = 1f;
  }

  public void SetState(NodeStates state, bool changeAppearance = true)
  {
    if (state == NodeStates.Attainable && this.NodeBlueprint.HasCost && Inventory.GetItemQuantity(this.NodeBlueprint.CostType) < this.NodeBlueprint.CostAmount)
      state = NodeStates.Locked;
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

  public void OnNodeClicked()
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

  public void SelectionIconFade(float scale)
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

  public void ScaleOutline(float duration, Vector3 from, Vector3 to)
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
      selectOnUp = (Selectable) null,
      selectOnDown = (Selectable) null,
      mode = Navigation.Mode.Automatic
    };
    this.OnNodeSelected = (Action<AdventureMapNode>) null;
    this.OnNodeDeselected = (Action<AdventureMapNode>) null;
    this.OnNodeChosen = (Action<AdventureMapNode>) null;
  }

  public void BreakNode(float duration, float strength, float delay, AnimationCurve curve)
  {
    this.StartCoroutine((IEnumerator) this.BreakNodeIE(duration, strength, delay, curve));
  }

  public IEnumerator BreakNodeIE(
    float duration,
    float strength,
    float delay,
    AnimationCurve curve)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    AdventureMapNode adventureMapNode = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      adventureMapNode.breakParticle.transform.parent = adventureMapNode.transform.parent;
      adventureMapNode.breakParticle.gameObject.SetActive(true);
      adventureMapNode.gameObject.SetActive(false);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) adventureMapNode.StartCoroutine((IEnumerator) adventureMapNode.ShakeNodeIE(duration, strength, delay, curve));
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void ShakeNode(float duration, float strength, float delay, AnimationCurve curve)
  {
    this.StartCoroutine((IEnumerator) this.ShakeNodeIE(duration, strength, delay, curve));
  }

  public IEnumerator ShakeNodeIE(
    float duration,
    float strength,
    float delay,
    AnimationCurve curve)
  {
    yield return (object) new WaitForSecondsRealtime(delay);
    Vector3 pos = this._shakeContainer.transform.localPosition;
    float time = 0.0f;
    while ((double) time < (double) duration)
    {
      time += Time.unscaledDeltaTime;
      this._shakeContainer.transform.localPosition = (pos + (Vector3) UnityEngine.Random.insideUnitCircle * strength) * curve.Evaluate(time / duration);
      yield return (object) null;
    }
  }

  public void UnlockNode()
  {
    DataManager.Instance.SetVariable(this.NodeBlueprint.UnlockedVariable.Variable, this.NodeBlueprint.UnlockedVariable.Condition);
    this.Configure(this._mapNode);
    this.SetState(NodeStates.Attainable);
    this.Punch();
  }
}
