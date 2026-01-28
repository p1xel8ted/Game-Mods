// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIAdventureMapOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Map;
using MMBiomeGeneration;
using Rewired;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UIAdventureMapOverlayController : UIMenuBase
{
  public const float kNodePositionScale = 300f;
  [Header("General")]
  [SerializeField]
  public GoopFade _goopFade;
  [SerializeField]
  public CanvasGroup _containerCanvasGroup;
  [SerializeField]
  public MMScrollRect _scrollView;
  [SerializeField]
  public RectTransform _scrollContent;
  [SerializeField]
  public GameObject _controlPrompts;
  [SerializeField]
  public GameObject _shufflePrompt;
  [Header("Nodes")]
  [SerializeField]
  public RectTransform _nodeContent;
  [Header("Connections")]
  [SerializeField]
  public RectTransform _connectionContent;
  [SerializeField]
  public Texture _connectionTexture;
  [SerializeField]
  public Material _scrollingMaterial;
  [SerializeField]
  public Material _idleDottedMaterial;
  [Header("Crown Spine")]
  [SerializeField]
  public RectTransform _crownSpineRectTransform;
  [SerializeField]
  public RectTransform _eyeSpineRectTransform;
  [Header("Indicator")]
  [SerializeField]
  public RectTransform _indicatorContainer;
  [SerializeField]
  public TMP_Text _indicatorText;
  [Header("Misc")]
  [SerializeField]
  public Image burst;
  public Map.Map _map;
  public bool _disableInput;
  public float _mapHeight;
  public float _contentHeight;
  public List<AdventureMapNode> _adventureMapNodes = new List<AdventureMapNode>();
  public List<NodeConnection> _nodeConnections = new List<NodeConnection>();
  public AdventureMapNode _currentNode;
  public AdventureMapNode _selectedNode;
  public Tweener _scrollTween;
  public bool _didCancel;
  public bool _doCameraPositionMoveOnShow = true;
  public bool _FadeInNodesAndConnections;
  public Coroutine _scrollCoroutine;
  public bool cancellable;
  [CompilerGenerated]
  public bool \u003CCancellable\u003Ek__BackingField = true;
  public System.Action OnNodeEntered;
  public bool set;

  public Map.Map Map => this._map;

  public bool _shouldScroll
  {
    get => (double) this._contentHeight > (double) this._scrollView.viewport.rect.height;
  }

  public static Color VisitedColour => StaticColors.GreenColor;

  public static Color LockedColour => StaticColors.DarkGreyColor;

  public static Color LockedColourLight => StaticColors.LightGreyColor;

  public static Color TryVisitColour => StaticColors.RedColor;

  public static Color AvailableColour => StaticColors.OffWhiteColor;

  public bool Cancellable
  {
    get => this.\u003CCancellable\u003Ek__BackingField;
    set => this.\u003CCancellable\u003Ek__BackingField = value;
  }

  public void OnEnable()
  {
    if (!(bool) (UnityEngine.Object) MonoSingleton<UIManager>.Instance)
      return;
    MonoSingleton<UIManager>.Instance.SetCurrentCursor(0);
  }

  public new void OnDisable()
  {
    if ((bool) (UnityEngine.Object) MonoSingleton<UIManager>.Instance)
      MonoSingleton<UIManager>.Instance.SetPreviousCursor();
    if (!(bool) (UnityEngine.Object) MonoSingleton<UIManager>.Instance)
      return;
    MonoSingleton<UIManager>.Instance.ResetPreviousCursor();
  }

  public override void Awake()
  {
    base.Awake();
    this._containerCanvasGroup.alpha = 0.0f;
  }

  public void Update()
  {
    if (!InputManager.UI.GetCookButtonDown() || !MapManager.Instance.CanShuffle || !((UnityEngine.Object) this._selectedNode != (UnityEngine.Object) null) || this._selectedNode.State != NodeStates.Attainable)
      return;
    this.StartCoroutine((IEnumerator) this.Shuffle());
  }

  public void Show(Map.Map map, bool disableInput = false, bool instant = false)
  {
    this._map = map;
    this._disableInput = disableInput;
    this._shufflePrompt.SetActive(false);
    this._controlPrompts.SetActive(false);
    this._crownSpineRectTransform.localScale = Vector3.zero;
    this.Show(instant);
  }

  public override void OnShowStarted()
  {
    AudioManager.Instance.PauseActiveLoops();
    UIManager.PlayAudio("event:/enter_leave_buildings/enter_building");
    foreach (Map.Node node in this._map.nodes)
    {
      if (node.incoming.Count != 0 || node.outgoing.Count != 0)
        this._adventureMapNodes.Add(this.MakeMapNode(node));
    }
    Bounds bounds = new Bounds(this._adventureMapNodes[0].RectTransform.localPosition, Vector3.zero);
    for (int index = 1; index < this._adventureMapNodes.Count; ++index)
      bounds.Encapsulate(this._adventureMapNodes[index].RectTransform.localPosition);
    foreach (AdventureMapNode adventureMapNode in this._adventureMapNodes)
    {
      RectTransform rectTransform = adventureMapNode.RectTransform;
      rectTransform.localPosition = rectTransform.localPosition - bounds.center;
    }
    this._mapHeight = bounds.size.y;
    this._contentHeight = this._mapHeight;
    double contentHeight1 = (double) this._contentHeight;
    Rect rect = this._scrollView.viewport.rect;
    double num = (double) rect.height / 3.0 * 2.0;
    if (contentHeight1 > num)
    {
      double contentHeight2 = (double) this._contentHeight;
      rect = this._scrollView.viewport.rect;
      double height = (double) rect.height;
      this._contentHeight = (float) (contentHeight2 + height);
    }
    if (this._doCameraPositionMoveOnShow)
    {
      RectTransform scrollContent = this._scrollContent;
      double contentHeight3 = (double) this._contentHeight;
      rect = this._scrollView.viewport.rect;
      double height = (double) rect.height;
      double size = (double) Mathf.Max((float) contentHeight3, (float) height);
      scrollContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float) size);
    }
    if (this._map.path.Count == 0)
      this._map.path.Add(this._map.GetFirstNode().point);
    foreach (Point p in this._map.path)
    {
      AdventureMapNode adventureMapNode = this.NodeFromPoint(p);
      if ((UnityEngine.Object) adventureMapNode != (UnityEngine.Object) null)
        adventureMapNode.SetState(NodeStates.Visited);
    }
    Point point1 = this._map.path.LastElement<Point>();
    Map.Node node1 = this._map.GetNode(point1);
    this._currentNode = this.NodeFromPoint(point1);
    this._currentNode.SetStartingNode();
    if (this._doCameraPositionMoveOnShow)
      this._scrollView.content.anchoredPosition = new Vector2(0.0f, this.GetScrollPosition(this._currentNode.RectTransform));
    if (!this._disableInput)
    {
      List<AdventureMapNode> adventureMapNodes = this.GetNextAdventureMapNodes(node1);
      this.SetAdventureMapNodesAttainable(adventureMapNodes);
      if (TrinketManager.HasTrinket(TarotCards.Card.ExtraMove, MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
      {
        foreach (AdventureMapNode adventureMapNode in adventureMapNodes)
          this.SetAdventureMapNodesAttainable(this.GetNextAdventureMapNodes(adventureMapNode.MapNode));
      }
      if (TrinketManager.HasTrinket(TarotCards.Card.AdventureMapFreedom, MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
      {
        Point point2 = node1.point;
        foreach (AdventureMapNode adventureMapNode in this._adventureMapNodes)
        {
          if (!((UnityEngine.Object) adventureMapNode == (UnityEngine.Object) null) && adventureMapNode.State != NodeStates.Visited && adventureMapNode.MapNode.point.y == point2.y + 1)
            adventureMapNode.SetState(NodeStates.Attainable);
        }
      }
    }
    List<AdventureMapNode> adventureMapNodeList = new List<AdventureMapNode>()
    {
      this.NodeFromPoint(point1)
    };
    if (TrinketManager.HasTrinket(TarotCards.Card.ExtraMove, MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
    {
      foreach (AdventureMapNode adventureMapNode in this.GetNextAdventureMapNodes(node1))
        adventureMapNodeList.Add(adventureMapNode);
    }
    foreach (AdventureMapNode adventureMapNode in this._adventureMapNodes)
      adventureMapNode.Button.SetInteractionState(true);
    foreach (AdventureMapNode adventureMapNode in adventureMapNodeList)
    {
      List<AdventureMapNode> adventureMapNodes = this.GetNextAdventureMapNodes(adventureMapNode.MapNode);
      adventureMapNodes.Sort((Comparison<AdventureMapNode>) ((n1, n2) => n1.MapNode.point.x.CompareTo(n2.MapNode.point.x)));
      for (int index = 0; index < adventureMapNodes.Count; ++index)
      {
        MMButton button = adventureMapNodes[index].Button;
        Navigation navigation = button.Selectable.navigation with
        {
          mode = Navigation.Mode.Explicit
        };
        Selectable selectableOnUp = button.FindSelectableOnUp();
        Selectable selectableOnDown = button.FindSelectableOnDown();
        navigation.selectOnUp = selectableOnUp;
        navigation.selectOnDown = selectableOnDown;
        if (index < adventureMapNodes.Count - 1)
          navigation.selectOnRight = (Selectable) adventureMapNodes[index + 1].Button;
        if (index > 0)
          navigation.selectOnLeft = (Selectable) adventureMapNodes[index - 1].Button;
        adventureMapNodes[index].Button.Selectable.navigation = navigation;
      }
    }
    foreach (AdventureMapNode adventureMapNode in this._adventureMapNodes)
      adventureMapNode.Button.SetInteractionState(false);
    this._selectedNode = this.NodeFromPoint(this._map.GetNextNode(point1).point);
    this.OverrideDefault((Selectable) this._selectedNode.Button);
    foreach (AdventureMapNode adventureMapNode in this._adventureMapNodes)
    {
      if (adventureMapNode.MapNode.nodeType == Map.NodeType.Boss)
        adventureMapNode.transform.localPosition = new Vector3(0.0f, adventureMapNode.transform.localPosition.y, adventureMapNode.transform.localPosition.z);
    }
    this._crownSpineRectTransform.position = this._currentNode.transform.position;
    foreach (AdventureMapNode adventureMapNode in this._adventureMapNodes)
    {
      foreach (Point p in adventureMapNode.MapNode.outgoing)
        this.MakeLineConnection(adventureMapNode, this.NodeFromPoint(p));
    }
  }

  public override IEnumerator DoShowAnimation()
  {
    UIAdventureMapOverlayController overlayController = this;
    AudioManager.Instance.PlayOneShot("event:/ui/open_menu");
    overlayController._goopFade.FadeIn(0.5f, UseDeltaTime: false);
    yield return (object) new WaitForSecondsRealtime(0.25f);
    overlayController._containerCanvasGroup.DOFade(1f, 0.25f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    if (!overlayController._disableInput)
    {
      float position = (float) (((double) overlayController.GetScrollPosition(overlayController._selectedNode.RectTransform) + (double) overlayController.GetScrollPosition(overlayController._currentNode.RectTransform)) / 2.0);
      yield return (object) overlayController.ScrollTo(position, 0.5f);
    }
    overlayController._crownSpineRectTransform.DOScale(Vector3.one * 0.75f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.3f);
    if (!overlayController._disableInput)
    {
      overlayController.SetActiveStateForMenu(true);
      overlayController.ActivateNavigation();
      if (overlayController.Cancellable)
      {
        overlayController._controlPrompts.SetActive(true);
        Point point = overlayController._map.path.LastElement<Point>();
        Map.Node node = overlayController._map.GetNode(point);
        List<AdventureMapNode> adventureMapNodes = overlayController.GetNextAdventureMapNodes(node);
        if (MapManager.Instance.CanShuffle && (adventureMapNodes.Count != 1 || adventureMapNodes[0].NodeType != Map.NodeType.MiniBossFloor && adventureMapNodes[0].NodeType != Map.NodeType.Boss && adventureMapNodes[0].NodeType != Map.NodeType.FinalBoss))
          overlayController._shufflePrompt.SetActive(true);
      }
      overlayController.cancellable = true;
    }
    if ((UnityEngine.Object) BiomeConstants.Instance != (UnityEngine.Object) null)
      BiomeConstants.Instance.ChromaticAbberationTween(0.1f, 0.6f, BiomeConstants.Instance.ChromaticAberrationDefaultValue);
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable || !this.cancellable || !this.Cancellable)
      return;
    this._didCancel = true;
    this.Hide();
    AudioManager.Instance.ResumePausedLoopsAndSFX();
    UIManager.PlayAudio("event:/enter_leave_buildings/leave_building");
    UIManager.PlayAudio("event:/ui/close_menu");
  }

  public override IEnumerator DoHideAnimation()
  {
    this._goopFade.FadeOut(0.5f, UseDeltaTime: false);
    this._containerCanvasGroup.DOFade(0.0f, 0.25f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.5f);
  }

  public override void OnHideCompleted()
  {
    foreach (AdventureMapNode adventureMapNode in this._adventureMapNodes)
      adventureMapNode.Recycle<AdventureMapNode>();
    if (this._didCancel)
    {
      System.Action onCancel = this.OnCancel;
      if (onCancel != null)
        onCancel();
    }
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public float GetScrollPosition(RectTransform rectTransform)
  {
    return this._mapHeight - (rectTransform.anchoredPosition.y + this._mapHeight * 0.5f);
  }

  public List<AdventureMapNode> GetNextAdventureMapNodes(Map.Node currentNode)
  {
    List<AdventureMapNode> adventureMapNodes = new List<AdventureMapNode>();
    foreach (Point p in currentNode.outgoing)
    {
      AdventureMapNode adventureMapNode = this.NodeFromPoint(p);
      if ((UnityEngine.Object) adventureMapNode != (UnityEngine.Object) null)
        adventureMapNodes.Add(adventureMapNode);
    }
    return adventureMapNodes;
  }

  public void SetAdventureMapNodesAttainable(List<AdventureMapNode> mapNodes)
  {
    foreach (AdventureMapNode mapNode in mapNodes)
    {
      if ((UnityEngine.Object) mapNode != (UnityEngine.Object) null)
        mapNode.SetState(NodeStates.Attainable);
    }
  }

  public IEnumerator ScrollTo(
    RectTransform rectTransform,
    float duration,
    Ease ease = Ease.OutSine,
    bool renableScrollViewAfter = true)
  {
    yield return (object) this.ScrollTo(this.GetScrollPosition(rectTransform), duration, ease, renableScrollViewAfter);
  }

  public IEnumerator ScrollTo(
    float position,
    float duration,
    Ease ease = Ease.OutSine,
    bool renableScrollViewAfter = true)
  {
    if (this._shouldScroll)
    {
      this._scrollView.enabled = false;
      this.ScrollToTween(position, duration, ease);
      yield return (object) new WaitForSecondsRealtime(duration);
      if (renableScrollViewAfter)
        this._scrollView.enabled = true;
    }
    this._scrollCoroutine = (Coroutine) null;
  }

  public void ScrollToTween(float position, float duration, Ease ease)
  {
    if (!this._shouldScroll)
      return;
    this._scrollView.DOKill();
    this._scrollTween = (Tweener) this._scrollView.content.DOAnchorPosY(position, duration).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(ease).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true);
  }

  public IEnumerator ConvertAllNodesToCombatNodes()
  {
    this.cancellable = false;
    this.ScrollToTween(1f, 3.5f, Ease.InOutSine);
    List<Map.Node> nodes = MapGenerator.GetAllFutureNodes(MapGenerator.GetNodeLayer(this._currentNode.MapNode));
    float increment = 8f / (float) nodes.Count;
    yield return (object) new WaitForSecondsRealtime(0.1f);
    List<Map.Node> nodesList = MapGenerator.Nodes.SelectMany<List<Map.Node>, Map.Node>((Func<List<Map.Node>, IEnumerable<Map.Node>>) (n => (IEnumerable<Map.Node>) n)).Where<Map.Node>((Func<Map.Node, bool>) (n => n.incoming.Count > 0 || n.outgoing.Count > 0)).ToList<Map.Node>();
    for (int i = 0; i < nodesList.Count; ++i)
    {
      if (nodes.Contains(nodesList[i]))
      {
        AdventureMapNode adventureMapNode = this.NodeFromPoint(nodesList[i].point);
        if ((UnityEngine.Object) adventureMapNode != (UnityEngine.Object) null && nodesList[i].nodeType != Map.NodeType.DungeonFloor && nodesList[i].nodeType != Map.NodeType.MiniBossFloor && nodesList[i].nodeType != Map.NodeType.FirstFloor)
        {
          AudioManager.Instance.PlayOneShot("event:/ui/level_node_end_screen_ui_appear");
          nodesList[i].nodeType = Map.NodeType.DungeonFloor;
          nodesList[i].Hidden = false;
          nodesList[i].blueprint = MapManager.Instance.DungeonConfig.SecondFloorBluePrint;
          nodesList[i].Modifier = (DungeonModifier) null;
          adventureMapNode.Configure(nodesList[i]);
          adventureMapNode.Button.SetInteractionState(false);
          adventureMapNode.Punch();
          yield return (object) new WaitForSecondsRealtime(increment);
        }
      }
    }
    while (this._scrollTween != null && this._scrollTween.active)
      yield return (object) null;
    yield return (object) new WaitForSecondsRealtime(0.1f);
    this._map.nodes = nodesList;
  }

  public IEnumerator ConvertMiniBossNodeToBossNode()
  {
    this.cancellable = false;
    List<Map.Node> list = this._map.nodes.Select<Map.Node, Map.Node>((Func<Map.Node, Map.Node>) (n => n)).Where<Map.Node>((Func<Map.Node, bool>) (n => n.incoming.Count > 0 || n.outgoing.Count > 0)).ToList<Map.Node>();
    MapConfig dungeonConfig = MapManager.Instance.DungeonConfig;
    Map.Node bossMapNode = this._map.GetBossNode();
    AdventureMapNode bossNode = this.NodeFromPoint(bossMapNode.point);
    bossMapNode.nodeType = Map.NodeType.MiniBossFloor;
    bossMapNode.Hidden = false;
    bossMapNode.blueprint = dungeonConfig.MiniBossFloorBluePrint;
    bossNode.Configure(bossMapNode);
    for (int index = 0; index < list.Count; ++index)
    {
      if (list[index].nodeType == Map.NodeType.MiniBossFloor)
      {
        list[index].nodeType = Map.NodeType.MiniBossFloor;
        list[index].Hidden = false;
        list[index].blueprint = dungeonConfig.LeaderFloorBluePrint;
        break;
      }
    }
    yield return (object) this.ScrollTo(bossNode.RectTransform, 1.5f, Ease.InOutCubic);
    yield return (object) new WaitForSecondsRealtime(0.25f);
    bossMapNode.blueprint = dungeonConfig.LeaderFloorBluePrint;
    bossNode.Configure(bossMapNode);
    bossNode.Button.SetInteractionState(false);
    bossNode.ScaleIn();
    AudioManager.Instance.PlayOneShot("event:/ui/level_node_beat_level");
    yield return (object) new WaitForSecondsRealtime(1.25f);
  }

  public IEnumerator RandomiseNextNodes()
  {
    this.cancellable = false;
    this.ScrollToTween(1f, 3.5f, Ease.InOutSine);
    List<Map.Node> nodes = MapGenerator.GetAllFutureNodes(MapGenerator.GetNodeLayer(this._currentNode.MapNode));
    float increment = 8f / (float) nodes.Count;
    yield return (object) new WaitForSecondsRealtime(0.1f);
    MapConfig dungeonConfig = MapManager.Instance.DungeonConfig;
    List<Map.Node> nodesList = MapGenerator.Nodes.SelectMany<List<Map.Node>, Map.Node>((Func<List<Map.Node>, IEnumerable<Map.Node>>) (n => (IEnumerable<Map.Node>) n)).Where<Map.Node>((Func<Map.Node, bool>) (n => n.incoming.Count > 0 || n.outgoing.Count > 0)).ToList<Map.Node>();
    for (int i = 0; i < nodesList.Count; ++i)
    {
      if (nodes.Contains(nodesList[i]))
      {
        AdventureMapNode adventureMapNode = this.NodeFromPoint(nodesList[i].point);
        if ((UnityEngine.Object) adventureMapNode != (UnityEngine.Object) null && nodesList[i].nodeType != Map.NodeType.DungeonFloor && nodesList[i].nodeType != Map.NodeType.MiniBossFloor && nodesList[i].nodeType != Map.NodeType.FirstFloor)
        {
          nodesList[i].nodeType = MapGenerator.GetRandomNode(dungeonConfig);
          nodesList[i].Hidden = false;
          string blueprintName = dungeonConfig.nodeBlueprints.Where<NodeBlueprint>((Func<NodeBlueprint, bool>) (b => b.nodeType == nodesList[i].nodeType)).ToList<NodeBlueprint>().Random<NodeBlueprint>().name;
          nodesList[i].blueprint = dungeonConfig.nodeBlueprints.FirstOrDefault<NodeBlueprint>((Func<NodeBlueprint, bool>) (n => n.name == blueprintName));
          adventureMapNode.Configure(nodesList[i]);
          adventureMapNode.Button.SetInteractionState(false);
          adventureMapNode.Punch();
          yield return (object) new WaitForSecondsRealtime(increment);
        }
      }
    }
    this._map.nodes = nodesList;
    while (this._scrollTween != null && this._scrollTween.active)
      yield return (object) null;
    yield return (object) new WaitForSecondsRealtime(0.1f);
  }

  public void ResetMap()
  {
    foreach (AdventureMapNode adventureMapNode in this._adventureMapNodes)
    {
      AdventureMapNode mapNode = adventureMapNode;
      mapNode.CanvasGroup.DOFade(0.0f, 1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() =>
      {
        mapNode.CanvasGroup.alpha = 1f;
        mapNode.Recycle<AdventureMapNode>();
      }));
    }
    this._adventureMapNodes.Clear();
    foreach (NodeConnection nodeConnection in this._nodeConnections)
    {
      NodeConnection connection = nodeConnection;
      connection.LineRenderer.material.DOFade(0.0f, 1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true).OnComplete<TweenerCore<Color, Color, ColorOptions>>((TweenCallback) (() =>
      {
        connection.LineRenderer.material.DOFade(1f, 0.0f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
        UnityEngine.Object.Destroy((UnityEngine.Object) connection.gameObject);
      }));
    }
    this._nodeConnections.Clear();
    this._map.path.Clear();
    this._map.nodes.Clear();
  }

  public IEnumerator RegenerateMapRoutine()
  {
    UIAdventureMapOverlayController overlayController = this;
    AdventureMapNode firstNode = overlayController.NodeFromPoint(overlayController._map.GetFirstNode().point);
    overlayController._doCameraPositionMoveOnShow = false;
    overlayController._FadeInNodesAndConnections = true;
    BiomeGenerator.Instance.SpawnedLoreTotemsThisDungeon = false;
    yield return (object) overlayController.ScrollTo(overlayController._mapHeight / 2f, 1.5f, Ease.InOutCubic);
    yield return (object) new WaitForSecondsRealtime(0.25f);
    overlayController._scrollView.DOKill();
    overlayController.ResetMap();
    AudioManager.Instance.PlayOneShot("event:/dlc/ui/dungeon_map/infinitedungeon_refresh");
    yield return (object) new WaitForSecondsRealtime(1f);
    overlayController._map = MapManager.Instance.GenerateNewMap();
    overlayController.OnShowStarted();
    AudioManager.Instance.PlayOneShot("event:/ui/level_node_beat_level");
    yield return (object) new WaitForSecondsRealtime(1f);
    firstNode = overlayController.NodeFromPoint(overlayController._map.GetFirstNode().point);
    yield return (object) overlayController.ScrollTo(firstNode.RectTransform, 1f, Ease.InOutCubic, false);
    yield return (object) new WaitForSecondsRealtime(0.25f);
    overlayController.OnNodeChosen(firstNode);
    overlayController._doCameraPositionMoveOnShow = true;
    overlayController._FadeInNodesAndConnections = false;
  }

  public IEnumerator TeleportNode(Map.Node node)
  {
    AdventureMapNode target = this.NodeFromPoint(node.point);
    yield return (object) new WaitForSecondsRealtime(this._crownSpineRectTransform.DOScale(Vector3.zero, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).Duration());
    yield return (object) new WaitForSecondsRealtime(0.1f);
    this._crownSpineRectTransform.anchoredPosition = target.RectTransform.anchoredPosition;
    yield return (object) this.ScrollTo(target.RectTransform, 1.5f, Ease.InOutCubic);
    yield return (object) new WaitForSecondsRealtime(0.1f);
    target.SetState(NodeStates.Attainable);
    target.OnSelect((BaseEventData) null);
    target.Punch();
    yield return (object) new WaitForSecondsRealtime(0.25f);
    yield return (object) new WaitForSecondsRealtime(this._crownSpineRectTransform.DOScale(Vector3.one * 0.75f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).Duration());
    yield return (object) new WaitForSecondsRealtime(0.1f);
    MapManager.Instance.AddNodeToPath(node);
    MapManager.Instance.EnterNode(node);
  }

  public IEnumerator NextSandboxLayer()
  {
    yield return (object) null;
  }

  public AdventureMapNode NodeFromPoint(Point p)
  {
    return this._adventureMapNodes.FirstOrDefault<AdventureMapNode>((Func<AdventureMapNode, bool>) (n => n.MapNode.point.Equals(p)));
  }

  public void OnNodeSelected(AdventureMapNode node)
  {
    this._selectedNode = node;
    this.OverrideDefault((Selectable) this._selectedNode.Button);
    Shader.SetGlobalVector("SelectedObject", (Vector4) node.transform.position);
    Vector2 endValue = Vector2.zero;
    if ((UnityEngine.Object) node != (UnityEngine.Object) this._currentNode)
      endValue = (Vector2) ((node.transform.position - this._eyeSpineRectTransform.position).normalized * 25f);
    this._eyeSpineRectTransform.DOLocalMove((Vector3) endValue, 0.75f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart);
    foreach (Point point1 in node.MapNode.incoming)
    {
      Point point = point1;
      NodeConnection nodeConnection = this._nodeConnections.FirstOrDefault<NodeConnection>((Func<NodeConnection, bool>) (conn => conn.To.MapNode == node.MapNode && conn.From.MapNode.point.Equals(point) && conn.From.MapNode == this._map.GetCurrentNode()));
      if ((UnityEngine.Object) nodeConnection != (UnityEngine.Object) null)
      {
        nodeConnection.LineRenderer.Color = UIAdventureMapOverlayController.TryVisitColour;
        nodeConnection.LineRenderer.material = this._scrollingMaterial;
      }
    }
    Controller activeController = InputManager.General.GetLastActiveController(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer);
    if ((activeController == null ? 0 : (activeController.type != ControllerType.Mouse ? 1 : 0)) != 0)
    {
      if (this._scrollCoroutine != null)
        this.StopCoroutine(this._scrollCoroutine);
      this._scrollCoroutine = this.StartCoroutine((IEnumerator) this.ScrollTo(node.RectTransform, 0.5f));
    }
    if (node.NodeBlueprint.HasCost)
    {
      this._indicatorContainer.gameObject.SetActive(true);
      this._indicatorContainer.transform.position = node.transform.position - Vector3.down * 300f;
      this._indicatorText.text = string.Format(ScriptLocalization.UI_UpgradeTree.Requires, (object) CostFormatter.FormatCost(node.NodeBlueprint.CostType, node.NodeBlueprint.CostAmount));
    }
    else
      this._indicatorContainer.gameObject.SetActive(false);
  }

  public void OnNodeDeselected(AdventureMapNode node)
  {
    foreach (Point point1 in node.MapNode.incoming)
    {
      Point point = point1;
      NodeConnection nodeConnection = this._nodeConnections.FirstOrDefault<NodeConnection>((Func<NodeConnection, bool>) (conn => conn.To.MapNode == node.MapNode && conn.From.MapNode.point.Equals(point) && conn.From.MapNode == this._map.GetCurrentNode()));
      if ((UnityEngine.Object) nodeConnection != (UnityEngine.Object) null)
      {
        nodeConnection.LineRenderer.Color = UIAdventureMapOverlayController.AvailableColour;
        nodeConnection.LineRenderer.material = this._idleDottedMaterial;
      }
    }
  }

  public void OnNodeChosen(AdventureMapNode node)
  {
    if (node.NodeBlueprint.HasCost && Inventory.GetItemQuantity(node.NodeBlueprint.CostType) >= node.NodeBlueprint.CostAmount)
    {
      Inventory.ChangeItemQuantity(node.NodeBlueprint.CostType, -node.NodeBlueprint.CostAmount);
      node.UnlockNode();
    }
    else
    {
      MapManager.Instance.CanShuffle = true;
      node.OnNodeDeselected -= new Action<AdventureMapNode>(this.OnNodeDeselected);
      this._canvasGroup.interactable = false;
      System.Action onNodeEntered = this.OnNodeEntered;
      if (onNodeEntered != null)
        onNodeEntered();
      this.SetActiveStateForMenu(false);
      foreach (AdventureMapNode adventureMapNode in this._adventureMapNodes)
        adventureMapNode.Button.Interactable = false;
      NodeConnection traversalConnection = this.MakeLineConnection(this.NodeFromPoint(this._map.path.LastElement<Point>()), node);
      traversalConnection.LineRenderer.Color = UIAdventureMapOverlayController.TryVisitColour;
      traversalConnection.LineRenderer.material = (Material) null;
      traversalConnection.LineRenderer.Fill = 0.0f;
      if (!this._currentNode.gameObject.activeSelf && this._currentNode.MapNode.outgoing.Count <= 1)
      {
        foreach (Point point1 in node.MapNode.incoming)
        {
          Point point = point1;
          NodeConnection nodeConnection = this._nodeConnections.FirstOrDefault<NodeConnection>((Func<NodeConnection, bool>) (conn => conn.To.MapNode == node.MapNode && conn.From.MapNode.point.Equals(point) && conn.From.MapNode == this._map.GetCurrentNode()));
          if ((UnityEngine.Object) nodeConnection != (UnityEngine.Object) null)
            nodeConnection.gameObject.SetActive(false);
        }
        traversalConnection.gameObject.SetActive(false);
      }
      DOTween.To((DOGetter<float>) (() => traversalConnection.LineRenderer.Fill), (DOSetter<float>) (x => traversalConnection.LineRenderer.Fill = x), 1f, 1f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutExpo).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
      this._crownSpineRectTransform.transform.DOLocalMove(node.RectTransform.localPosition, 1f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutExpo);
      DG.Tweening.Sequence sequence = DOTween.Sequence();
      sequence.AppendInterval(0.4f).SetUpdate<DG.Tweening.Sequence>(true);
      sequence.Append((Tween) this._crownSpineRectTransform.DOScale(Vector3.zero, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true));
      sequence.Play<DG.Tweening.Sequence>().SetUpdate<DG.Tweening.Sequence>(true);
      sequence.onComplete = sequence.onComplete + (TweenCallback) (() => MapManager.Instance.EnterNode(node.MapNode));
      AudioManager.Instance.PlayOneShot("event:/ui/map_location_pan");
      AudioManager.Instance.PlayOneShot("event:/Stings/Choir_Short");
      MMVibrate.Haptic(MMVibrate.HapticTypes.Selection);
      MapManager.Instance.AddNodeToPath(node.MapNode);
      node.ShowSwirlAnimation();
    }
  }

  public AdventureMapNode MakeMapNode(Map.Node mapNode)
  {
    AdventureMapNode adventureMapNode = MonoSingleton<UIManager>.Instance.AdventureMapNodeTemplate.Spawn<AdventureMapNode>((Transform) this._nodeContent);
    adventureMapNode.Configure(mapNode);
    adventureMapNode.SetState(NodeStates.Locked);
    Vector2 vector2 = new Vector2((float) mapNode.point.x, (float) mapNode.point.y) * 300f + UnityEngine.Random.insideUnitCircle * 50f;
    adventureMapNode.RectTransform.localPosition = (Vector3) vector2;
    adventureMapNode.RectTransform.localScale = Vector3.one;
    adventureMapNode.OnNodeSelected += new Action<AdventureMapNode>(this.OnNodeSelected);
    adventureMapNode.OnNodeDeselected += new Action<AdventureMapNode>(this.OnNodeDeselected);
    adventureMapNode.OnNodeChosen += new Action<AdventureMapNode>(this.OnNodeChosen);
    if (this._FadeInNodesAndConnections)
    {
      adventureMapNode.CanvasGroup.alpha = 0.0f;
      adventureMapNode.CanvasGroup.DOFade(1f, 1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    }
    else
      adventureMapNode.CanvasGroup.alpha = 1f;
    return adventureMapNode;
  }

  public NodeConnection MakeLineConnection(AdventureMapNode from, AdventureMapNode to)
  {
    if ((UnityEngine.Object) from == (UnityEngine.Object) null || (UnityEngine.Object) to == (UnityEngine.Object) null)
      return (NodeConnection) null;
    GameObject gameObject = new GameObject();
    RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
    rectTransform.SetParent((Transform) this._connectionContent);
    rectTransform.localPosition = (Vector3) Vector2.zero;
    rectTransform.localScale = Vector3.one;
    MMUILineRenderer lineRenderer = gameObject.AddComponent<MMUILineRenderer>();
    lineRenderer.Texture = this._connectionTexture;
    lineRenderer.Points = new List<MMUILineRenderer.BranchPoint>()
    {
      new MMUILineRenderer.BranchPoint((Vector2) from.RectTransform.localPosition),
      new MMUILineRenderer.BranchPoint((Vector2) to.RectTransform.localPosition)
    };
    lineRenderer.Width = 7.5f;
    NodeConnection nodeConnection = gameObject.AddComponent<NodeConnection>();
    nodeConnection.Configure(from, to, lineRenderer, (Material) null, this._idleDottedMaterial);
    this._nodeConnections.Add(nodeConnection);
    if (this._FadeInNodesAndConnections)
    {
      if (!this.set)
      {
        nodeConnection.LineRenderer.material.DOFade(0.0f, 0.0f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
        this.set = true;
      }
      nodeConnection.LineRenderer.material.DOFade(1f, 1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    }
    else
      nodeConnection.LineRenderer.material.DOFade(1f, 0.0f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    return nodeConnection;
  }

  public IEnumerator Shuffle()
  {
    UIAdventureMapOverlayController overlayController = this;
    overlayController._shufflePrompt.SetActive(false);
    overlayController.SetActiveStateForMenu(false);
    MapManager.Instance.CanShuffle = false;
    overlayController.cancellable = false;
    MapConfig dungeonConfig = MapManager.Instance.DungeonConfig;
    Point point = overlayController._map.path.LastElement<Point>();
    overlayController._map.GetNode(point);
    List<Map.Node> nodesList = MapGenerator.Nodes.SelectMany<List<Map.Node>, Map.Node>((Func<List<Map.Node>, IEnumerable<Map.Node>>) (n => (IEnumerable<Map.Node>) n)).Where<Map.Node>((Func<Map.Node, bool>) (n => n.incoming.Count > 0 || n.outgoing.Count > 0)).ToList<Map.Node>();
    float increment = 1.5f;
    for (int i = 0; i < nodesList.Count; ++i)
    {
      if (nodesList[i] == overlayController._selectedNode.MapNode)
      {
        AdventureMapNode adventureMapNode = overlayController.NodeFromPoint(nodesList[i].point);
        if ((UnityEngine.Object) adventureMapNode != (UnityEngine.Object) null && nodesList[i].nodeType != Map.NodeType.MiniBossFloor && nodesList[i].nodeType != Map.NodeType.FirstFloor && nodesList[i].nodeType != Map.NodeType.Boss && nodesList[i].nodeType != Map.NodeType.FinalBoss)
        {
          nodesList[i].nodeType = MapGenerator.GetRandomNode(dungeonConfig);
          nodesList[i].Hidden = false;
          string blueprintName = dungeonConfig.nodeBlueprints.Where<NodeBlueprint>((Func<NodeBlueprint, bool>) (b => b.nodeType == nodesList[i].nodeType)).ToList<NodeBlueprint>().Random<NodeBlueprint>().name;
          nodesList[i].blueprint = dungeonConfig.nodeBlueprints.FirstOrDefault<NodeBlueprint>((Func<NodeBlueprint, bool>) (n => n.name == blueprintName));
          adventureMapNode.Button.SetInteractionState(true);
          adventureMapNode.Configure(nodesList[i]);
          adventureMapNode.Punch();
          yield return (object) new WaitForSecondsRealtime(increment);
        }
      }
    }
    overlayController._map.nodes = nodesList;
    yield return (object) new WaitForSecondsRealtime(0.1f);
    overlayController.cancellable = true;
    overlayController._selectedNode.SetState(NodeStates.Attainable);
    overlayController.SetActiveStateForMenu(true);
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) overlayController._selectedNode.Button);
  }
}
