// Decompiled with JetBrains decompiler
// Type: DungeonWorldMapIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

#nullable disable
public class DungeonWorldMapIcon : WorldMapIcon
{
  public Action<DungeonWorldMapIcon.NodeType> OnNodeSelected;
  public Action<DungeonWorldMapIcon.NodeType> OnNodeDeselected;
  public Action<DungeonWorldMapIcon> OnNodeChosen;
  [SerializeField]
  public bool isEndGameNode;
  [SerializeField]
  public bool isWolfNode;
  [SerializeField]
  public TMP_Text miniBossNodesCompletedText;
  [SerializeField]
  public DungeonWorldMapIcon[] miniBossNodes;
  [SerializeField]
  public bool _useShowVariableCondition;
  [SerializeField]
  public DataManager.Variables _showIfVariable;
  [SerializeField]
  public bool _useEnableVariableCondition;
  [SerializeField]
  public DataManager.Variables _enableIfVariable;
  public bool ForceState;
  public DungeonWorldMapIcon.IconState ForceStateValue = DungeonWorldMapIcon.IconState.Selectable;
  [Header("Dungeon")]
  [SerializeField]
  public DungeonWorldMapIcon.IconActionType _selectAction = DungeonWorldMapIcon.IconActionType.SendToLocation;
  [SerializeField]
  public bool _selectableWhenBeaten;
  [SerializeField]
  public DungeonMapIconContent _content;
  [FormerlySerializedAs("SuggestedScale")]
  [SerializeField]
  public float _suggestedScale = 1f;
  [SerializeField]
  public bool _scaleConstant;
  [SerializeField]
  public DungeonWorldMapIcon.NodeType nodeType;
  [SerializeField]
  public int _dungeonLayer = -1;
  [SerializeField]
  public CanvasGroup _canvasGroup;
  [SerializeField]
  public List<DungeonWorldMapIcon.RequireNode> _requiredNodes = new List<DungeonWorldMapIcon.RequireNode>();
  [SerializeField]
  public List<DungeonWorldMapIcon> _childNodes = new List<DungeonWorldMapIcon>();
  [SerializeField]
  public DungeonWorldMapIcon.IconState _initialState = DungeonWorldMapIcon.IconState.Unrevealed;
  [SerializeField]
  public bool _visitedByDefault;
  [SerializeField]
  [Tooltip("If true, node will start off locked and block progression until unlocked")]
  public bool _isLocked;
  public bool IsHidden;
  [SerializeField]
  public bool completeInstantly;
  [SerializeField]
  public string _lockName = "";
  [SerializeField]
  public BuyEntry[] rewards;
  [SerializeField]
  public float _selectableChildStaggerDelay = 0.3f;
  [SerializeField]
  public float _previewChildStaggerDelay = 0.4f;
  [SerializeField]
  public int variant;
  [Header("Goop Effect")]
  [SerializeField]
  public float m_GoopRadiusScale = 1f;
  [Header("Random Node")]
  [SerializeField]
  public int _minNodesGenerated;
  [SerializeField]
  public int _maxNodesGenerated;
  [SerializeField]
  public Vector2 _generationDirection = Vector2.left;
  [SerializeField]
  public float _generationDistance = 200f;
  [SerializeField]
  public float _arcSize = 60f;
  [SerializeField]
  public List<DungeonWorldMapIcon> _randomPrefabs = new List<DungeonWorldMapIcon>();
  [SerializeField]
  public DungeonWorldMapIcon _doorwayDestination;
  [SerializeField]
  public UIDLCMapMenuController.DLCDungeonSide DungeonSide = UIDLCMapMenuController.DLCDungeonSide.OutsideMountain;
  public UIDLCMapMenuController _dlcMapMenu;
  public Dictionary<int, DLCMapConnection> _parentConnections = new Dictionary<int, DLCMapConnection>();
  public Dictionary<int, DLCMapConnection> _childConnections = new Dictionary<int, DLCMapConnection>();
  public List<DungeonWorldMapIconEventListener> _eventListeners = new List<DungeonWorldMapIconEventListener>();
  [CompilerGenerated]
  [SerializeField]
  public DungeonWorldMapIcon.IconState \u003CCurrentState\u003Ek__BackingField = DungeonWorldMapIcon.IconState.Unrevealed;
  public Vector3 startingScale;
  public int UnlocksLockCount = -1;
  [CompilerGenerated]
  public bool \u003CIsTemporary\u003Ek__BackingField;
  public Action<DungeonWorldMapIcon> OnDungeonLocationSelected;
  public bool _isHighlighted;
  public const float StateBasedGameplayScaleModifier = 1f;
  public HashSet<int> _visited = new HashSet<int>();

  public bool IsEndGameNode => this.isEndGameNode;

  public void FixDuplicateComponents()
  {
    GameObject gameObject = this.gameObject;
    DungeonWorldMapIcon[] components = gameObject.GetComponents<DungeonWorldMapIcon>();
    if (components.Length != 2)
    {
      Debug.LogWarning((object) $"'{gameObject.name}' does not have exactly two DungeonWorldMapIcon components.");
    }
    else
    {
      DungeonWorldMapIcon dungeonWorldMapIcon1 = components[0];
      DungeonWorldMapIcon dungeonWorldMapIcon2 = components[1];
      this.CopySerializedFields((Component) dungeonWorldMapIcon2, (Component) dungeonWorldMapIcon1);
      this.SwapReferences((Component) dungeonWorldMapIcon2, (Component) dungeonWorldMapIcon1);
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) dungeonWorldMapIcon2, true);
      Debug.Log((object) ("Copied data and removed duplicate DungeonWorldMapIcon on " + gameObject.name), (UnityEngine.Object) gameObject);
    }
  }

  public void CopySerializedFields(Component source, Component destination)
  {
  }

  public void SwapReferences(Component oldComp, Component newComp)
  {
  }

  public static bool IsHiddenState(DungeonWorldMapIcon.IconState state)
  {
    return state == DungeonWorldMapIcon.IconState.Unrevealed;
  }

  public static bool IsStateInteractable(DungeonWorldMapIcon.IconState state)
  {
    return state == DungeonWorldMapIcon.IconState.Selectable || state == DungeonWorldMapIcon.IconState.Completed;
  }

  public bool UseShowVariableCondition => this._useShowVariableCondition;

  public DataManager.Variables ShowIfVariable => this._showIfVariable;

  public DungeonWorldMapIcon.NodeType Type => this.nodeType;

  public bool CompleteInstantly => this.completeInstantly;

  public BuyEntry[] Rewards => this.rewards;

  public int Variant => this.variant;

  public UIDLCMapMenuController dlcMapMenu
  {
    get
    {
      return this._dlcMapMenu ?? (this._dlcMapMenu = this.GetComponentInParent<UIDLCMapMenuController>());
    }
  }

  public DungeonWorldMapIcon.IconState CurrentState
  {
    get => this.\u003CCurrentState\u003Ek__BackingField;
    set => this.\u003CCurrentState\u003Ek__BackingField = value;
  }

  public bool IsHighlighted => this._isHighlighted && this.Button.Selectable.IsInteractable();

  public bool VisitedByDefault => this._visitedByDefault;

  public int MinNodesGenerated => this._minNodesGenerated;

  public int MaxNodesGenerated => this._maxNodesGenerated;

  public float ArcSize => this._arcSize;

  public DungeonWorldMapIcon DoorwayDestination => this._doorwayDestination;

  public IReadOnlyList<DungeonWorldMapIcon> RandomPrefabs
  {
    get => (IReadOnlyList<DungeonWorldMapIcon>) this._randomPrefabs;
  }

  public Vector2 GenerationDirection => this._generationDirection;

  public float GenerationDistance => this._generationDistance;

  public bool IsKey
  {
    get
    {
      return this.nodeType == DungeonWorldMapIcon.NodeType.Key || this.nodeType == DungeonWorldMapIcon.NodeType.Key_2 || this.nodeType == DungeonWorldMapIcon.NodeType.Key_3;
    }
  }

  public bool IsLock
  {
    get
    {
      return this.nodeType == DungeonWorldMapIcon.NodeType.Lock || this.nodeType == DungeonWorldMapIcon.NodeType.Lock_2 || this.nodeType == DungeonWorldMapIcon.NodeType.Lock_3;
    }
  }

  public bool IsLockWithoutKey
  {
    get
    {
      if (this.Type == DungeonWorldMapIcon.NodeType.Lock && DataManager.Instance.DLCKey_1 == 0 || this.Type == DungeonWorldMapIcon.NodeType.Lock_2 && DataManager.Instance.DLCKey_2 == 0)
        return true;
      return this.Type == DungeonWorldMapIcon.NodeType.Lock_3 && DataManager.Instance.DLCKey_3 == 0;
    }
  }

  public bool IsBigBoss
  {
    get
    {
      return this.nodeType == DungeonWorldMapIcon.NodeType.Yngya || this.nodeType == DungeonWorldMapIcon.NodeType.Dungeon5_Boss || this.nodeType == DungeonWorldMapIcon.NodeType.Dungeon6_Boss;
    }
  }

  public bool ShouldBeHidden
  {
    get => DungeonWorldMapIcon.IsHiddenState(this.CurrentState) || this.IsClearedInstaCollect();
  }

  public bool IsClearedInstaCollect()
  {
    return this.SelectAction == DungeonWorldMapIcon.IconActionType.InstantCollect && this.IsBeaten && this.Type != DungeonWorldMapIcon.NodeType.Lock && this.Type != DungeonWorldMapIcon.NodeType.Lock_2;
  }

  public bool ShouldBeInteractable
  {
    get
    {
      if (this.isEndGameNode)
        return true;
      if (this.IsLock)
        return false;
      bool shouldBeInteractable = DungeonWorldMapIcon.IsStateInteractable(this.CurrentState);
      Debug.Log((object) $"Interactable? : {{Node:{this.gameObject.name},Result:{shouldBeInteractable}, MapSide:{this.dlcMapMenu.CurrentDLCDungeonSide}, NodeSide:{this.DungeonSide}, IsStateInteractable:{DungeonWorldMapIcon.IsStateInteractable(this.CurrentState)}}}");
      return shouldBeInteractable;
    }
  }

  public bool IsObstacle
  {
    get
    {
      if (this.IsLocked)
        return true;
      if (this.CurrentState == DungeonWorldMapIcon.IconState.Completed)
        return false;
      return this.CurrentState != DungeonWorldMapIcon.IconState.Selectable || this.SelectAction != 0;
    }
  }

  public DungeonWorldMapIcon.IconActionType SelectAction => this._selectAction;

  public int DungeonLayer => this._dungeonLayer;

  public int ID
  {
    get
    {
      UIDLCMapMenuController dlcMapMenu = this.dlcMapMenu;
      return dlcMapMenu == null ? -1 : dlcMapMenu._locations.IndexOf<DungeonWorldMapIcon>(this);
    }
  }

  public bool IsMiniBoss
  {
    get
    {
      return this.nodeType == DungeonWorldMapIcon.NodeType.Dungeon5_MiniBoss || this.nodeType == DungeonWorldMapIcon.NodeType.Dungeon6_MiniBoss;
    }
  }

  public bool IsBoss
  {
    get
    {
      return this.nodeType == DungeonWorldMapIcon.NodeType.Dungeon5_Boss || this.nodeType == DungeonWorldMapIcon.NodeType.Dungeon6_Boss;
    }
  }

  public bool IsNPC
  {
    get
    {
      return this.nodeType == DungeonWorldMapIcon.NodeType.Dungeon5_GhostNPC || this.nodeType == DungeonWorldMapIcon.NodeType.Dungeon6_GhostNPC;
    }
  }

  public bool IsBeaten
  {
    get
    {
      return (this.ID != 10 || !DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_5)) && (this.ID != 100 || !DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_6)) && DataManager.Instance.DLCDungeonNodesCompleted.Contains(this.ID);
    }
  }

  public bool IsVisiblyBeaten => this.IsBeaten;

  public bool IsLocked => this._isLocked;

  public bool IsLockNode
  {
    get
    {
      return this._isLocked || this.Type == DungeonWorldMapIcon.NodeType.Lock || this.Type == DungeonWorldMapIcon.NodeType.Lock_2;
    }
  }

  public string LockName => this._lockName;

  public DungeonMapIconContent Content
  {
    get => this._content ?? (this._content = this.GetComponentInChildren<DungeonMapIconContent>());
  }

  public bool IsTemporary
  {
    get => this.\u003CIsTemporary\u003Ek__BackingField;
    set => this.\u003CIsTemporary\u003Ek__BackingField = value;
  }

  public float UnlockedWeight => 1f;

  public float HiddenWeight => 0.0f;

  public float GoopRadiusScale => this.m_GoopRadiusScale;

  public List<DungeonWorldMapIcon.RequireNode> RequiredNodes => this._requiredNodes;

  public IEnumerable<DungeonWorldMapIcon> ChildNodes
  {
    get => (IEnumerable<DungeonWorldMapIcon>) this._childNodes;
  }

  public IEnumerable<DLCMapConnection> ChildConnections
  {
    get => (IEnumerable<DLCMapConnection>) this._childConnections.Values;
  }

  public bool HasParentNodes => this._parentConnections.Count > 0;

  public IEnumerable<DungeonWorldMapIcon> ParentNodes
  {
    get
    {
      foreach (DLCMapConnection dlcMapConnection in this._parentConnections.Values)
        yield return dlcMapConnection.From;
    }
  }

  public DungeonWorldMapIcon Parent => this.ParentNodes.FirstOrDefault<DungeonWorldMapIcon>();

  public bool AnyParentsCompleted
  {
    get
    {
      foreach (int key in this._parentConnections.Keys)
      {
        if (DataManager.Instance.DLCDungeonNodesCompleted.Contains(key))
          return true;
      }
      return false;
    }
  }

  public bool HasChildConnections => this._childConnections.Count > 0;

  public bool HasParentConnections => this._parentConnections.Count > 0;

  public float DesiredIconScale => 1f * this.RawScale;

  public bool IsComplete => DataManager.Instance.DLCDungeonNodesCompleted.Contains(this.ID);

  public float RawScale => this._suggestedScale;

  public void CompleteNode()
  {
    if (this.isEndGameNode)
      return;
    int id = this.ID;
    if (DataManager.Instance.DLCDungeonNodesCompleted.Contains(id))
      Debug.LogWarning((object) $"Node {id} is already completed.");
    else
      DataManager.Instance.DLCDungeonNodesCompleted.Add(id);
  }

  public void SetCurrentNode() => DataManager.Instance.DLCDungeonNodeCurrent = this.ID;

  public override void Awake()
  {
    base.Awake();
    this.startingScale = this.transform.localScale;
    this._eventListeners.AddRange((IEnumerable<DungeonWorldMapIconEventListener>) this.GetComponentsInChildren<DungeonWorldMapIconEventListener>());
    this._button.OnConfirmDenied += new System.Action(this._content.Shake);
    this.SetPosition((Vector2) this.transform.position);
  }

  public void SetDungeonSide(UIDLCMapMenuController.DLCDungeonSide side)
  {
    Debug.Log((object) $"Setting Dungeon Side: {{node:{this.gameObject.name}, side:{side}}}");
    this.DungeonSide = side;
    this._button.interactable = this.ShouldBeInteractable;
    this._canvasGroup.blocksRaycasts = this.ShouldBeInteractable;
  }

  public void Configure(DungeonWorldMapIcon node, bool revealNodes)
  {
    if ((UnityEngine.Object) node == (UnityEngine.Object) null)
      return;
    if (this.ForceState)
      this.CurrentState = this.ForceStateValue;
    bool variable = DataManager.Instance.GetVariable(this._enableIfVariable);
    if (this._useEnableVariableCondition && !variable)
    {
      foreach (DungeonWorldMapIcon location in this.dlcMapMenu.Locations)
        location.RemoveChildNode(this);
      node.CurrentState = DungeonWorldMapIcon.IconState.Preview;
      this._button.interactable = this.ShouldBeInteractable;
      this._canvasGroup.blocksRaycasts = this.ShouldBeInteractable;
    }
    this._content.Configure(node.Type, node.SelectAction, node.CurrentState);
    if (revealNodes)
      this.RevealPath();
    if (!this.IsLock || this.CurrentState != DungeonWorldMapIcon.IconState.Completed)
      return;
    this.transform.localScale = Vector3.zero;
  }

  public void Configure(
    DungeonWorldMapIcon.NodeType type,
    DungeonWorldMapIcon.IconActionType selectAction,
    DungeonWorldMapIcon.IconState? persistentState,
    bool revealNodes)
  {
    this.CurrentState = (DungeonWorldMapIcon.IconState) ((int) persistentState ?? (int) this._initialState);
    if (this.isWolfNode && (this.CurrentState == DungeonWorldMapIcon.IconState.None || this.CurrentState == DungeonWorldMapIcon.IconState.Unrevealed))
      this.CurrentState = DungeonWorldMapIcon.IconState.Preview;
    this._content.SetState(this.Type, this.CurrentState, this._selectAction);
    this._button.interactable = this.ShouldBeInteractable;
    this._canvasGroup.blocksRaycasts = this.ShouldBeInteractable;
    Debug.Log((object) $"Has node been visited? {{Name:{this.gameObject.name},CurrentState:{this.CurrentState}, IsVisiblyBeaten:{this.IsVisiblyBeaten}}}");
    this.BroadcastStateInitialise();
    if (!this.IsLock || this.CurrentState != DungeonWorldMapIcon.IconState.Completed)
      return;
    this.transform.localScale = Vector3.zero;
  }

  public void SetRuntimeComplete()
  {
    this._visited.Clear();
    if (this.IsLock)
      return;
    this.CompleteRecursively(this, 0);
  }

  public int GetPriority(DungeonWorldMapIcon.IconState s)
  {
    DungeonWorldMapIcon.IconState[] depthRules = this._dlcMapMenu.DepthRules;
    int num = Array.IndexOf<DungeonWorldMapIcon.IconState>(depthRules, s);
    return num < 0 ? depthRules.Length : num;
  }

  public void CompleteRecursively(DungeonWorldMapIcon node, int depth)
  {
    if (!this._visited.Add(node.ID))
      return;
    DungeonWorldMapIcon.IconState[] depthRules = this._dlcMapMenu.DepthRules;
    if (depth >= depthRules.Length)
      return;
    DungeonWorldMapIcon.IconState s = depthRules[depth];
    DungeonWorldMapIcon.IconState iconState = this.GetPriority(node.CurrentState) <= this.GetPriority(s) ? node.CurrentState : s;
    if (node.IsLock && node.IsComplete)
    {
      iconState = DungeonWorldMapIcon.IconState.Completed;
      --depth;
    }
    if (node.IsLock && !node.IsComplete)
    {
      iconState = DungeonWorldMapIcon.IconState.Locked;
      depth = depthRules.Length - 2;
    }
    if (iconState != node.CurrentState)
      node.Configure(node.Type, node.SelectAction, new DungeonWorldMapIcon.IconState?(iconState), false);
    foreach (DLCMapConnection childConnection in node.ChildConnections)
      childConnection.RefreshState(true);
    foreach (DungeonWorldMapIcon childNode in node.ChildNodes)
      this.CompleteRecursively(childNode, depth + 1);
  }

  public override void OnButtonClicked()
  {
    base.OnButtonClicked();
    if (this.IsLocked)
    {
      this._button.OnConfirmDenied();
      this.PerformLockedFeedback();
    }
    else
    {
      Action<DungeonWorldMapIcon> locationSelected = this.OnDungeonLocationSelected;
      if (locationSelected == null)
        return;
      locationSelected(this);
    }
  }

  public void OnEnable()
  {
    if (!this.isWolfNode)
      return;
    this.miniBossNodesCompletedText.text = $"{this.MiniBossNodesBeaten()}/{8}";
  }

  public void OnDisable() => this.OnDehighlight();

  public void PerformEnterFeedback()
  {
    this.transform.DOPunchScale(Vector3.one * 0.25f, 0.5f).SetUpdate<Tweener>(true);
  }

  public void PerformLockedFeedback()
  {
    this.transform.DOPunchPosition(Vector3.right * 5f, 0.5f).SetUpdate<Tweener>(true);
  }

  public void SelectInvalid()
  {
    UIManager.PlayAudio("event:/dlc/ui/map/node_click_invalid");
    this.Content.Shake();
  }

  public void SelectValid()
  {
    this.transform.DOPunchScale(Vector3.one * 0.25f, 0.5f).SetUpdate<Tweener>(true);
    if (this.IsBigBoss)
      UIManager.PlayAudio("event:/dlc/ui/map/node_select_boss");
    else
      UIManager.PlayAudio("event:/dlc/ui/map/node_select_generic");
  }

  public void UpdateRevealPaths()
  {
    foreach (DLCMapConnection dlcMapConnection in this._parentConnections.Values)
    {
      if (DataManager.Instance.DLCDungeonNodesCompleted.Contains(dlcMapConnection.From.ID))
        dlcMapConnection.SetFill(1f);
    }
  }

  public void SetVisible(bool alsoRevealPaths)
  {
    foreach (DungeonWorldMapIconEventListener eventListener in this._eventListeners)
      eventListener.OnVisible(this);
    this.gameObject.SetActive(true);
  }

  public bool RevealPath()
  {
    if (this.HasParentNodes && !this.AnyParentsCompleted)
      return false;
    bool flag = false;
    for (int index = 0; index < this._requiredNodes.Count; ++index)
    {
      if (DataManager.Instance.DLCDungeonNodesCompleted.Contains(this._requiredNodes[index].ID) && !this._requiredNodes[index].LineRenderer.gameObject.activeSelf)
      {
        foreach (MMUILineRenderer.BranchPoint point in this._requiredNodes[index].LineRenderer.Points)
        {
          foreach (MMUILineRenderer.Branch branch in point.Branches)
            branch.Fill = 0.0f;
        }
        this._requiredNodes[index].LineRenderer.gameObject.SetActive(true);
        GameManager.GetInstance().WaitForSecondsRealtime(2.25f, (System.Action) (() =>
        {
          this.SetVisible(false);
          this.transform.DOPunchScale(this.startingScale * 0.25f, 0.5f).SetUpdate<Tweener>(true);
        }));
        this.TweenPath(this._requiredNodes[index].LineRenderer, 1f, 1f, 1f);
        flag = true;
      }
    }
    foreach (DLCMapConnection dlcMapConnection in this._parentConnections.Values)
    {
      if (DataManager.Instance.DLCDungeonNodesCompleted.Contains(dlcMapConnection.From.ID) && !dlcMapConnection.Visible)
      {
        dlcMapConnection.SetFill(0.0f);
        GameManager.GetInstance().WaitForSecondsRealtime(2.25f, (System.Action) (() =>
        {
          this.SetVisible(false);
          this.transform.DOPunchScale(this.startingScale * 0.25f, 0.5f).SetUpdate<Tweener>(true);
        }));
        dlcMapConnection.Reveal(1f, 1f);
        flag = true;
      }
    }
    return flag;
  }

  public void HidePath(float duration = 0.5f)
  {
    this.Button.Interactable = false;
    this.transform.DOScale(0.0f, duration / 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    for (int index = 0; index < this._requiredNodes.Count; ++index)
      this.TweenPath(this._requiredNodes[index].LineRenderer, 0.0f, duration, 0.0f);
  }

  public DLCMapConnection GetParentConnection()
  {
    DungeonWorldMapIcon parent = this.Parent;
    DLCMapConnection dlcMapConnection;
    return (UnityEngine.Object) parent == (UnityEngine.Object) null || !this._parentConnections.TryGetValue(parent.ID, out dlcMapConnection) ? (DLCMapConnection) null : dlcMapConnection;
  }

  public void TweenPath(MMUILineRenderer lineRenderer, float target, float duration, float delay)
  {
    if ((double) target == 0.0 && (double) lineRenderer.Points[0].Branches[0].Fill <= 0.0)
      return;
    MonoBehaviour.print((object) "hi");
    float t = 0.0f;
    DOTween.To((DOGetter<float>) (() => t), (DOSetter<float>) (x => t = x), target, duration).SetDelay<TweenerCore<float, float, FloatOptions>>(delay).SetUpdate<TweenerCore<float, float, FloatOptions>>(true).OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() =>
    {
      lineRenderer.Fill = t;
      foreach (MMUILineRenderer.BranchPoint point in lineRenderer.Points)
      {
        foreach (MMUILineRenderer.Branch branch in point.Branches)
          branch.Fill = t;
      }
    })).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() =>
    {
      if ((double) target != 0.0)
        return;
      lineRenderer.gameObject.SetActive(false);
    }));
  }

  public bool AddParentConnection(DLCMapConnection connection)
  {
    return this._parentConnections.TryAdd(connection.From.ID, connection);
  }

  public bool AddChildConnection(DLCMapConnection connection)
  {
    int num = this._childConnections.TryAdd(connection.To.ID, connection) ? 1 : 0;
    if (!((UnityEngine.Object) this._childNodes.Find((Predicate<DungeonWorldMapIcon>) (x => x.ID == connection.To.ID)) == (UnityEngine.Object) null))
      return num != 0;
    this._childNodes.Add(connection.To);
    return num != 0;
  }

  public void RemoveChildNode(DungeonWorldMapIcon child)
  {
    if (!this._childNodes.Contains(child))
      return;
    int id = child.ID;
    this._childNodes.Remove(child);
    UnityEngine.Object.Destroy((UnityEngine.Object) this._childConnections[id].gameObject);
    this._childConnections.Remove(id);
  }

  public void RemoveNode(bool hide = true, bool removeConnections = true)
  {
    if (removeConnections)
    {
      foreach (DungeonWorldMapIcon location in this.dlcMapMenu.Locations)
        location.RemoveChildNode(this);
    }
    this.gameObject.SetActive(!hide);
    this.Button.Interactable = false;
  }

  public void ClearChildConnections(HashSet<int> childNodeIds)
  {
    foreach (int childNodeId in childNodeIds)
      this._childConnections.Remove(childNodeId);
  }

  public void BroadcastStateInitialise()
  {
    foreach (DungeonWorldMapIconEventListener eventListener in this._eventListeners)
      eventListener.Initialise(this.dlcMapMenu, this);
  }

  public void RefreshInteractability()
  {
    this._button.Interactable = this.ShouldBeInteractable;
    if (!this.IsLock || this.CurrentState != DungeonWorldMapIcon.IconState.Completed)
      return;
    this.transform.localScale = Vector3.zero;
  }

  public void OnHighlight()
  {
    this._isHighlighted = true;
    foreach (DLCMapConnection dlcMapConnection in this._parentConnections.Values)
      dlcMapConnection.RefreshState(false);
    Action<DungeonWorldMapIcon.NodeType> onNodeSelected = this.OnNodeSelected;
    if (onNodeSelected == null)
      return;
    onNodeSelected(this.Type);
  }

  public void OnDehighlight()
  {
    this._isHighlighted = false;
    foreach (DLCMapConnection dlcMapConnection in this._parentConnections.Values)
      dlcMapConnection.RefreshState(false);
    Action<DungeonWorldMapIcon.NodeType> onNodeDeselected = this.OnNodeDeselected;
    if (onNodeDeselected == null)
      return;
    onNodeDeselected(this.Type);
  }

  public void GatherActiveNeighbours(List<DungeonWorldMapIcon> neighbours)
  {
    neighbours.Clear();
    foreach (DungeonWorldMapIcon childNode in this.ChildNodes)
    {
      if (childNode.CurrentState == DungeonWorldMapIcon.IconState.Selectable || childNode.CurrentState == DungeonWorldMapIcon.IconState.Completed || childNode.IsBeaten || childNode.IsLock)
        neighbours.Add(childNode);
    }
    foreach (DLCMapConnection dlcMapConnection in this._parentConnections.Values)
    {
      DungeonWorldMapIcon from = dlcMapConnection.From;
      if (from.CurrentState == DungeonWorldMapIcon.IconState.Selectable || from.CurrentState == DungeonWorldMapIcon.IconState.Completed || from.IsBeaten || from.IsLock)
        neighbours.Add(from);
    }
  }

  public int MiniBossNodesBeaten()
  {
    int num = 0;
    for (int index = 0; index < this.miniBossNodes.Length; ++index)
    {
      if (DataManager.Instance.DLCDungeonNodesCompleted.Contains(this.miniBossNodes[index].ID))
        ++num;
    }
    return num;
  }

  [CompilerGenerated]
  public void \u003CRevealPath\u003Eb__191_0()
  {
    this.SetVisible(false);
    this.transform.DOPunchScale(this.startingScale * 0.25f, 0.5f).SetUpdate<Tweener>(true);
  }

  [CompilerGenerated]
  public void \u003CRevealPath\u003Eb__191_1()
  {
    this.SetVisible(false);
    this.transform.DOPunchScale(this.startingScale * 0.25f, 0.5f).SetUpdate<Tweener>(true);
  }

  public enum NodeType
  {
    Base,
    Dungeon1,
    Dungeon5,
    Dungeon5_MiniBoss,
    Dungeon5_Boss,
    Dungeon5_Follower,
    Dungeon6,
    Dungeon6_MiniBoss,
    Dungeon6_Boss,
    Dungeon5_GhostNPC,
    GhostResource,
    Dungeon5_Puzzle,
    Door,
    Key,
    Lock,
    Random,
    Dungeon5_Story,
    Reward,
    Shrine,
    Dungeon6_Follower,
    Dungeon6_GhostNPC,
    Dungeon6_Puzzle,
    Dungeon6_Story,
    Key_2,
    Lock_2,
    Dungeon5_Witness,
    Dungeon6_Witness,
    Completed,
    Yngya,
    Key_3,
    Lock_3,
  }

  public enum IconState
  {
    None,
    Unrevealed,
    Preview,
    Selectable,
    Completed,
    Locked,
  }

  public enum IconActionType
  {
    Nothing,
    SendToLocation,
    MakeRandomNodes,
    Doorway,
    InstantCollect,
  }

  [Serializable]
  public struct RequireNode
  {
    public int ID;
    public MMUILineRenderer LineRenderer;
  }

  [MessagePackObject(false)]
  [Serializable]
  public struct DLCTemporaryMapNode
  {
    [Key(0)]
    public int NodeID;
    [Key(1)]
    public int ParentNodeID;
    [Key(2)]
    public int PrefabIndex;
    [Key(3)]
    public Vector3 Position;
  }
}
