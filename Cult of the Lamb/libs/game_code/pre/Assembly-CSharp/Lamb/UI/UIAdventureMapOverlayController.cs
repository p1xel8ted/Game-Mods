// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIAdventureMapOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Map;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
  private GoopFade _goopFade;
  [SerializeField]
  private CanvasGroup _containerCanvasGroup;
  [SerializeField]
  private MMScrollRect _scrollView;
  [SerializeField]
  private RectTransform _scrollContent;
  [SerializeField]
  private GameObject _controlPrompts;
  [Header("Nodes")]
  [SerializeField]
  private RectTransform _nodeContent;
  [Header("Connections")]
  [SerializeField]
  private RectTransform _connectionContent;
  [SerializeField]
  private Texture _connectionTexture;
  [SerializeField]
  private Material _scrollingMaterial;
  [SerializeField]
  private Material _idleDottedMaterial;
  [Header("Crown Spine")]
  [SerializeField]
  private RectTransform _crownSpineRectTransform;
  [SerializeField]
  private RectTransform _eyeSpineRectTransform;
  private Map.Map _map;
  private bool _disableInput;
  private float _mapHeight;
  private float _contentHeight;
  private List<AdventureMapNode> _adventureMapNodes = new List<AdventureMapNode>();
  private List<NodeConnection> _nodeConnections = new List<NodeConnection>();
  private AdventureMapNode _currentNode;
  private AdventureMapNode _selectedNode;
  private Tweener _scrollTween;
  private bool _didCancel;
  private bool _doCameraPositionMoveOnShow = true;
  private bool _FadeInNodesAndConnections;
  private Coroutine _scrollCoroutine;
  private bool cancellable;
  private bool set;

  private bool _shouldScroll
  {
    get => (double) this._contentHeight > (double) this._scrollView.viewport.rect.height;
  }

  public static Color VisitedColour => StaticColors.GreenColor;

  public static Color LockedColour => StaticColors.DarkGreyColor;

  public static Color LockedColourLight => StaticColors.LightGreyColor;

  public static Color TryVisitColour => StaticColors.RedColor;

  public static Color AvailableColour => StaticColors.OffWhiteColor;

  public override void Awake()
  {
    base.Awake();
    this._containerCanvasGroup.alpha = 0.0f;
  }

  public void Show(Map.Map map, bool disableInput = false, bool instant = false)
  {
    this._map = map;
    this._disableInput = disableInput;
    this._controlPrompts.SetActive(false);
    this._crownSpineRectTransform.localScale = Vector3.zero;
    this.Show(instant);
  }

  protected override void OnShowStarted()
  {
    UIManager.PlayAudio("event:/enter_leave_buildings/enter_building");
    foreach (Map.Node node in this._map.nodes)
      this._adventureMapNodes.Add(this.MakeMapNode(node));
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
    Point point = this._map.path.LastElement<Point>();
    Map.Node node1 = this._map.GetNode(point);
    this._currentNode = this.NodeFromPoint(point);
    this._currentNode.SetStartingNode();
    if (this._doCameraPositionMoveOnShow)
      this._scrollView.content.anchoredPosition = new Vector2(0.0f, this.GetScrollPosition(this._currentNode.RectTransform));
    this._crownSpineRectTransform.position = this._currentNode.transform.position;
    if (!this._disableInput)
    {
      foreach (Point p in node1.outgoing)
      {
        AdventureMapNode adventureMapNode = this.NodeFromPoint(p);
        if ((UnityEngine.Object) adventureMapNode != (UnityEngine.Object) null)
          adventureMapNode.SetState(NodeStates.Attainable);
      }
    }
    List<AdventureMapNode> adventureMapNodeList = new List<AdventureMapNode>();
    foreach (Point p in node1.outgoing)
      adventureMapNodeList.Add(this.NodeFromPoint(p));
    adventureMapNodeList.Sort((Comparison<AdventureMapNode>) ((n1, n2) => n1.MapNode.point.x.CompareTo(n2.MapNode.point.x)));
    for (int index = 0; index < adventureMapNodeList.Count; ++index)
    {
      MMButton button = adventureMapNodeList[index].Button;
      Selectable selectableOnUp = button.FindSelectableOnUp();
      Selectable selectableOnDown = button.FindSelectableOnDown();
      Navigation navigation = button.Selectable.navigation with
      {
        mode = Navigation.Mode.Explicit,
        selectOnUp = selectableOnUp,
        selectOnDown = selectableOnDown
      };
      if (index < adventureMapNodeList.Count - 1)
        navigation.selectOnRight = (Selectable) adventureMapNodeList[index + 1].Button;
      if (index > 0)
        navigation.selectOnLeft = (Selectable) adventureMapNodeList[index - 1].Button;
      adventureMapNodeList[index].Button.Selectable.navigation = navigation;
    }
    foreach (AdventureMapNode adventureMapNode in this._adventureMapNodes)
      adventureMapNode.Button.SetInteractionState(false);
    this._selectedNode = this.NodeFromPoint(this._map.GetNextNode(point).point);
    this.OverrideDefault((Selectable) this._selectedNode.Button);
    foreach (AdventureMapNode adventureMapNode in this._adventureMapNodes)
    {
      foreach (Point p in adventureMapNode.MapNode.outgoing)
        this.MakeLineConnection(adventureMapNode, this.NodeFromPoint(p));
    }
  }

  protected override IEnumerator DoShowAnimation()
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
      overlayController._controlPrompts.SetActive(true);
      overlayController.cancellable = true;
    }
    if ((UnityEngine.Object) BiomeConstants.Instance != (UnityEngine.Object) null)
      BiomeConstants.Instance.ChromaticAbberationTween(0.1f, 0.6f, BiomeConstants.Instance.ChromaticAberrationDefaultValue);
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable || !this.cancellable)
      return;
    this._didCancel = true;
    this.Hide();
    UIManager.PlayAudio("event:/enter_leave_buildings/leave_building");
    UIManager.PlayAudio("event:/ui/close_menu");
  }

  protected override IEnumerator DoHideAnimation()
  {
    this._goopFade.FadeOut(0.5f, UseDeltaTime: false);
    this._containerCanvasGroup.DOFade(0.0f, 0.25f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.5f);
  }

  protected override void OnHideCompleted()
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

  private float GetScrollPosition(RectTransform rectTransform)
  {
    return this._mapHeight - (rectTransform.anchoredPosition.y + this._mapHeight * 0.5f);
  }

  private IEnumerator ScrollTo(
    RectTransform rectTransform,
    float duration,
    Ease ease = Ease.OutSine,
    bool renableScrollViewAfter = true)
  {
    yield return (object) this.ScrollTo(this.GetScrollPosition(rectTransform), duration, ease, renableScrollViewAfter);
  }

  private IEnumerator ScrollTo(
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

  private void ScrollToTween(float position, float duration, Ease ease)
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
        if ((UnityEngine.Object) adventureMapNode != (UnityEngine.Object) null && nodesList[i].nodeType != NodeType.DungeonFloor && nodesList[i].nodeType != NodeType.MiniBossFloor && nodesList[i].nodeType != NodeType.FirstFloor)
        {
          AudioManager.Instance.PlayOneShot("event:/ui/level_node_end_screen_ui_appear");
          nodesList[i].nodeType = NodeType.DungeonFloor;
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
    bossMapNode.nodeType = NodeType.MiniBossFloor;
    bossMapNode.Hidden = false;
    bossMapNode.blueprint = dungeonConfig.MiniBossFloorBluePrint;
    bossNode.Configure(bossMapNode);
    for (int index = 0; index < list.Count; ++index)
    {
      if (list[index].nodeType == NodeType.MiniBossFloor)
      {
        list[index].nodeType = NodeType.MiniBossFloor;
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
        if ((UnityEngine.Object) adventureMapNode != (UnityEngine.Object) null && nodesList[i].nodeType != NodeType.DungeonFloor && nodesList[i].nodeType != NodeType.MiniBossFloor && nodesList[i].nodeType != NodeType.FirstFloor)
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

  private void ResetMap()
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
    yield return (object) overlayController.ScrollTo(overlayController._mapHeight / 2f, 1.5f, Ease.InOutCubic);
    yield return (object) new WaitForSecondsRealtime(0.25f);
    overlayController._scrollView.DOKill();
    overlayController.ResetMap();
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

  private void OnNodeSelected(AdventureMapNode node)
  {
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
    if (InputManager.General.MouseInputActive)
      return;
    if (this._scrollCoroutine != null)
      this.StopCoroutine(this._scrollCoroutine);
    this._scrollCoroutine = this.StartCoroutine((IEnumerator) this.ScrollTo(node.RectTransform, 0.5f));
  }

  private void OnNodeDeselected(AdventureMapNode node)
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
    node.OnNodeDeselected -= new Action<AdventureMapNode>(this.OnNodeDeselected);
    this._canvasGroup.interactable = false;
    this.SetActiveStateForMenu(false);
    foreach (AdventureMapNode adventureMapNode in this._adventureMapNodes)
      adventureMapNode.Button.Interactable = false;
    NodeConnection traversalConnection = this.MakeLineConnection(this.NodeFromPoint(this._map.path.LastElement<Point>()), node);
    traversalConnection.LineRenderer.Color = UIAdventureMapOverlayController.TryVisitColour;
    traversalConnection.LineRenderer.material = (Material) null;
    traversalConnection.LineRenderer.Fill = 0.0f;
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

  private AdventureMapNode MakeMapNode(Map.Node mapNode)
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

  private NodeConnection MakeLineConnection(AdventureMapNode from, AdventureMapNode to)
  {
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
}
