// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Framework.Graph
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using NodeCanvas.Framework.Internal;
using ParadoxNotion;
using ParadoxNotion.Design;
using ParadoxNotion.Serialization;
using ParadoxNotion.Services;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Framework;

[Serializable]
public abstract class Graph : ScriptableObject, ITaskSystem, ISerializationCallbackReceiver
{
  [SerializeField]
  public string _serializedGraph;
  [SerializeField]
  public List<UnityEngine.Object> _objectReferences;
  [SerializeField]
  public bool _deserializationFailed;
  [NonSerialized]
  public bool hasDeserialized;
  public string _category = string.Empty;
  public string _comments = string.Empty;
  public Vector2 _translation = new Vector2(-5000f, -5000f);
  public float _zoomFactor = 1f;
  public List<Node> _nodes = new List<Node>();
  public Node _primeNode;
  public List<CanvasGroup> _canvasGroups;
  public BlackboardSource _localBlackboard;
  public static bool globally_enabled = true;
  [NonSerialized]
  public Component _agent;
  [NonSerialized]
  public IBlackboard _blackboard;
  [NonSerialized]
  public static List<Graph> runningGraphs = new List<Graph>();
  [NonSerialized]
  public float timeStarted;
  [NonSerialized]
  public bool _isRunning;
  [NonSerialized]
  public bool _isPaused;

  void ISerializationCallbackReceiver.OnBeforeSerialize() => this.Serialize();

  void ISerializationCallbackReceiver.OnAfterDeserialize() => this.Deserialize();

  public void Serialize()
  {
  }

  public void Deserialize()
  {
    if (this.hasDeserialized && JSONSerializer.applicationPlaying)
      return;
    this.hasDeserialized = true;
    this.Deserialize(this._serializedGraph, false, this._objectReferences);
  }

  public void OnEnable() => this.Validate();

  public void OnDisable()
  {
  }

  public void OnDestroy()
  {
  }

  public void OnValidate()
  {
  }

  public string Serialize(bool pretyJson, List<UnityEngine.Object> objectReferences)
  {
    if (this._deserializationFailed)
    {
      this._deserializationFailed = false;
      return this._serializedGraph;
    }
    if (objectReferences == null)
      objectReferences = this._objectReferences = new List<UnityEngine.Object>();
    this.UpdateNodeIDs(true);
    return JSONSerializer.Serialize(typeof (GraphSerializationData), (object) new GraphSerializationData(this), pretyJson, objectReferences);
  }

  public GraphSerializationData Deserialize(
    string serializedGraph,
    bool validate,
    List<UnityEngine.Object> objectReferences)
  {
    if (string.IsNullOrEmpty(serializedGraph))
      return (GraphSerializationData) null;
    if (objectReferences == null)
      objectReferences = this._objectReferences;
    try
    {
      GraphSerializationData data = JSONSerializer.Deserialize<GraphSerializationData>(serializedGraph, objectReferences);
      if (this.LoadGraphData(data, validate))
      {
        this._deserializationFailed = false;
        this._serializedGraph = serializedGraph;
        this._objectReferences = objectReferences;
        return data;
      }
      this._deserializationFailed = true;
      return (GraphSerializationData) null;
    }
    catch (Exception ex)
    {
      ParadoxNotion.Services.Logger.LogException(ex, "NodeCanvas", (object) this);
      this._deserializationFailed = true;
      return (GraphSerializationData) null;
    }
  }

  public bool LoadGraphData(GraphSerializationData data, bool validate)
  {
    if (data == null)
    {
      ParadoxNotion.Services.Logger.LogError((object) "Can't Load graph, cause of null GraphSerializationData provided", "Serialization", (object) this);
      return false;
    }
    if (System.Type.op_Inequality(data.type, this.GetType()))
    {
      ParadoxNotion.Services.Logger.LogError((object) "Can't Load graph, cause of different Graph type serialized and required", "Serialization", (object) this);
      return false;
    }
    data.Reconstruct(this);
    this._category = data.category;
    this._comments = data.comments;
    this._translation = data.translation;
    this._zoomFactor = data.zoomFactor;
    this._nodes = data.nodes;
    this._primeNode = data.primeNode;
    this._canvasGroups = data.canvasGroups;
    this._localBlackboard = data.localBlackboard;
    if (validate)
      this.Validate();
    return true;
  }

  public virtual object OnDerivedDataSerialization() => (object) null;

  public virtual void OnDerivedDataDeserialization(object data)
  {
  }

  public void GetSerializationData(out string json, out List<UnityEngine.Object> references)
  {
    json = this._serializedGraph;
    references = this._objectReferences != null ? new List<UnityEngine.Object>((IEnumerable<UnityEngine.Object>) this._objectReferences) : (List<UnityEngine.Object>) null;
  }

  public void SetSerializationObjectReferences(List<UnityEngine.Object> references)
  {
    this._objectReferences = references;
  }

  public static T Clone<T>(T graph) where T : Graph
  {
    T obj = UnityEngine.Object.Instantiate<T>(graph);
    obj.name = obj.name.Replace("(Clone)", "");
    return obj;
  }

  public string SerializeLocalBlackboard()
  {
    return JSONSerializer.Serialize(typeof (BlackboardSource), (object) this._localBlackboard, objectReferences: this._objectReferences);
  }

  public bool DeserializeLocalBlackboard(string json)
  {
    try
    {
      this._localBlackboard = JSONSerializer.Deserialize<BlackboardSource>(json, this._objectReferences);
      if (this._localBlackboard == null)
        this._localBlackboard = new BlackboardSource();
      return true;
    }
    catch (Exception ex)
    {
      ParadoxNotion.Services.Logger.LogException(ex, "Serialization", (object) this);
      return false;
    }
  }

  public void CopySerialized(Graph target)
  {
    string serializedGraph = this.Serialize(false, target._objectReferences);
    target.Deserialize(serializedGraph, true, this._objectReferences);
  }

  public void Validate()
  {
    for (int index = 0; index < this.allNodes.Count; ++index)
    {
      try
      {
        this.allNodes[index].OnValidate(this);
      }
      catch (Exception ex)
      {
        Node allNode = this.allNodes[index];
        ParadoxNotion.Services.Logger.LogException(ex, "Validation", (object) allNode);
      }
    }
    List<Task> allTasksOfType = this.GetAllTasksOfType<Task>();
    for (int index = 0; index < allTasksOfType.Count; ++index)
    {
      try
      {
        allTasksOfType[index].OnValidate((ITaskSystem) this);
      }
      catch (Exception ex)
      {
        Task context = allTasksOfType[index];
        ParadoxNotion.Services.Logger.LogException(ex, "Validation", (object) context);
      }
    }
    this.OnGraphValidate();
    if (!Application.isPlaying || !this.useLocalBlackboard)
      return;
    this.localBlackboard.InitializePropertiesBinding((GameObject) null, false);
  }

  public virtual void OnGraphValidate()
  {
  }

  public event Action<bool> OnFinish;

  public abstract System.Type baseNodeType { get; }

  public abstract bool requiresAgent { get; }

  public abstract bool requiresPrimeNode { get; }

  public abstract bool autoSort { get; }

  public abstract bool useLocalBlackboard { get; }

  public new string name
  {
    get => base.name;
    set => base.name = value;
  }

  public string category
  {
    get => this._category;
    set => this._category = value;
  }

  public string graphComments
  {
    get => this._comments;
    set => this._comments = value;
  }

  public float elapsedTime
  {
    get => !this.isRunning && !this.isPaused ? 0.0f : Time.time - this.timeStarted;
  }

  public bool isRunning
  {
    get => this._isRunning;
    set => this._isRunning = value;
  }

  public bool isPaused
  {
    get => this._isPaused;
    set => this._isPaused = value;
  }

  public List<Node> allNodes
  {
    get => this._nodes;
    set => this._nodes = value;
  }

  public Node primeNode
  {
    get => this._primeNode;
    set
    {
      if (this._primeNode == value || value != null && !value.allowAsPrime)
        return;
      if (this.isRunning)
      {
        if (this._primeNode != null)
          this._primeNode.Reset();
        value?.Reset();
      }
      this.RecordUndo("Set Start");
      this._primeNode = value;
      this.UpdateNodeIDs(true);
    }
  }

  public List<CanvasGroup> canvasGroups
  {
    get => this._canvasGroups;
    set => this._canvasGroups = value;
  }

  public Vector2 translation
  {
    get => this._translation;
    set => this._translation = value;
  }

  public float zoomFactor
  {
    get => this._zoomFactor;
    set => this._zoomFactor = value;
  }

  public Component agent
  {
    get => this._agent;
    set => this._agent = value;
  }

  public IBlackboard blackboard
  {
    get => this.useLocalBlackboard ? (IBlackboard) this.localBlackboard : this._blackboard;
    set
    {
      if (this._blackboard == value || this.isRunning || this.useLocalBlackboard)
        return;
      this._blackboard = value;
    }
  }

  public BlackboardSource localBlackboard
  {
    get
    {
      if (this._localBlackboard == null)
      {
        this._localBlackboard = new BlackboardSource();
        this._localBlackboard.name = "Local Blackboard";
      }
      return this._localBlackboard;
    }
  }

  UnityEngine.Object ITaskSystem.contextObject => (UnityEngine.Object) this;

  public static List<Node> CloneNodes(
    List<Node> originalNodes,
    Graph targetGraph = null,
    Vector2 originPosition = default (Vector2))
  {
    if ((UnityEngine.Object) targetGraph != (UnityEngine.Object) null && originalNodes.Any<Node>((Func<Node, bool>) (n => !n.GetType().IsSubclassOf(targetGraph.baseNodeType))))
      return (List<Node>) null;
    List<Node> nodeList1 = new List<Node>();
    Dictionary<Connection, KeyValuePair<int, int>> dictionary = new Dictionary<Connection, KeyValuePair<int, int>>();
    foreach (Node originalNode in originalNodes)
    {
      Node node;
      if ((UnityEngine.Object) targetGraph != (UnityEngine.Object) null)
      {
        node = originalNode.Duplicate(targetGraph);
        if ((UnityEngine.Object) targetGraph != (UnityEngine.Object) originalNode.graph && (UnityEngine.Object) originalNode.graph != (UnityEngine.Object) null && originalNode == originalNode.graph.primeNode)
          targetGraph.primeNode = node;
      }
      else
        node = JSONSerializer.Clone<Node>(originalNode);
      nodeList1.Add(node);
      foreach (Connection outConnection in originalNode.outConnections)
      {
        int key = originalNodes.IndexOf(outConnection.sourceNode);
        int num = originalNodes.IndexOf(outConnection.targetNode);
        dictionary[outConnection] = new KeyValuePair<int, int>(key, num);
      }
    }
    foreach (KeyValuePair<Connection, KeyValuePair<int, int>> keyValuePair1 in dictionary)
    {
      KeyValuePair<int, int> keyValuePair2 = keyValuePair1.Value;
      if (keyValuePair2.Value != -1)
      {
        List<Node> nodeList2 = nodeList1;
        keyValuePair2 = keyValuePair1.Value;
        int key = keyValuePair2.Key;
        Node newSource = nodeList2[key];
        List<Node> nodeList3 = nodeList1;
        keyValuePair2 = keyValuePair1.Value;
        int index = keyValuePair2.Value;
        Node newTarget = nodeList3[index];
        if ((UnityEngine.Object) targetGraph != (UnityEngine.Object) null)
        {
          keyValuePair1.Key.Duplicate(newSource, newTarget);
        }
        else
        {
          Connection connection = JSONSerializer.Clone<Connection>(keyValuePair1.Key);
          connection.SetSource(newSource, false);
          connection.SetTarget(newTarget, false);
        }
      }
    }
    if (originPosition != new Vector2() && nodeList1.Count > 0)
    {
      if (nodeList1.Count == 1)
      {
        nodeList1[0].nodePosition = originPosition;
      }
      else
      {
        Vector2 vector2 = nodeList1[0].nodePosition - originPosition;
        nodeList1[0].nodePosition = originPosition;
        for (int index = 1; index < nodeList1.Count; ++index)
          nodeList1[index].nodePosition -= vector2;
      }
    }
    return nodeList1;
  }

  public void UpdateReferences()
  {
    this.UpdateNodeBBFields();
    this.SendTaskOwnerDefaults();
  }

  public void UpdateNodeBBFields()
  {
    for (int index = 0; index < this.allNodes.Count; ++index)
      BBParameter.SetBBFields((object) this.allNodes[index], this.blackboard);
  }

  public void SendTaskOwnerDefaults()
  {
    List<Task> allTasksOfType = this.GetAllTasksOfType<Task>();
    for (int index = 0; index < allTasksOfType.Count; ++index)
      allTasksOfType[index].SetOwnerSystem((ITaskSystem) this);
  }

  public void UpdateNodeIDs(bool alsoReorderList)
  {
    int lastID = 0;
    if (this.primeNode != null)
      lastID = this.primeNode.AssignIDToGraph(lastID);
    List<Node> list = this.allNodes.OrderBy<Node, bool>((Func<Node, bool>) (n => n.inConnections.Count != 0)).ToList<Node>();
    for (int index = 0; index < list.Count; ++index)
      lastID = list[index].AssignIDToGraph(lastID);
    for (int index = 0; index < this.allNodes.Count; ++index)
      this.allNodes[index].ResetRecursion();
    if (!alsoReorderList)
      return;
    this.allNodes = this.allNodes.OrderBy<Node, int>((Func<Node, int>) (node => node.ID)).ToList<Node>();
  }

  public void StartGraph(
    Component agent,
    IBlackboard blackboard,
    bool autoUpdate,
    Action<bool> callback = null)
  {
    if (this.isRunning)
    {
      if (callback != null)
        this.OnFinish += callback;
      ParadoxNotion.Services.Logger.LogWarning((object) "Graph is already Active.", "NodeCanvas", (object) this);
    }
    else if ((UnityEngine.Object) agent == (UnityEngine.Object) null && this.requiresAgent)
      ParadoxNotion.Services.Logger.LogWarning((object) "You've tried to start a graph with null Agent.", "NodeCanvas", (object) this);
    else if (this.primeNode == null && this.requiresPrimeNode)
    {
      ParadoxNotion.Services.Logger.LogWarning((object) "You've tried to start graph without a 'Start' node.", "NodeCanvas", (object) this);
    }
    else
    {
      if (blackboard == null)
      {
        if ((UnityEngine.Object) agent != (UnityEngine.Object) null)
        {
          ParadoxNotion.Services.Logger.Log((object) $"Graph started without blackboard. Looking for blackboard on agent '{agent.gameObject?.ToString()}'...", "NodeCanvas", (object) this);
          blackboard = agent.GetComponent(typeof (IBlackboard)) as IBlackboard;
        }
        if (blackboard == null)
        {
          ParadoxNotion.Services.Logger.LogWarning((object) "Started with null Blackboard. Using Local Blackboard instead.", "NodeCanvas", (object) this);
          blackboard = (IBlackboard) this.localBlackboard;
        }
      }
      this.agent = agent;
      this.blackboard = blackboard;
      this.UpdateReferences();
      if (callback != null)
        this.OnFinish = callback;
      this.isRunning = true;
      Graph.runningGraphs.Add(this);
      if (!this.isPaused)
      {
        this.timeStarted = Time.time;
        this.OnGraphStarted();
      }
      else
        this.OnGraphUnpaused();
      for (int index = 0; index < this.allNodes.Count; ++index)
      {
        if (!this.isPaused)
          this.allNodes[index].OnGraphStarted();
        else
          this.allNodes[index].OnGraphUnpaused();
      }
      this.isPaused = false;
      if (!autoUpdate)
        return;
      MonoManager.current.onUpdate += new Action(this.UpdateGraph);
      this.UpdateGraph();
    }
  }

  public void Stop(bool success = true)
  {
    if (!this.isRunning && !this.isPaused)
      return;
    Graph.runningGraphs.Remove(this);
    MonoManager.current.onUpdate -= new Action(this.UpdateGraph);
    this.isRunning = false;
    this.isPaused = false;
    for (int index = 0; index < this.allNodes.Count; ++index)
    {
      this.allNodes[index].Reset(false);
      this.allNodes[index].OnGraphStoped();
    }
    this.OnGraphStoped();
    if (this.OnFinish == null)
      return;
    this.OnFinish(success);
    this.OnFinish = (Action<bool>) null;
  }

  public void Pause()
  {
    if (!this.isRunning)
      return;
    Graph.runningGraphs.Remove(this);
    MonoManager.current.onUpdate -= new Action(this.UpdateGraph);
    this.isRunning = false;
    this.isPaused = true;
    for (int index = 0; index < this.allNodes.Count; ++index)
      this.allNodes[index].OnGraphPaused();
    this.OnGraphPaused();
  }

  public void UpdateGraph()
  {
    if (!this.isRunning)
      return;
    this.OnGraphUpdate();
  }

  public virtual void OnGraphStarted()
  {
  }

  public virtual void OnGraphUpdate()
  {
  }

  public virtual void OnGraphStoped()
  {
  }

  public virtual void OnGraphPaused()
  {
  }

  public virtual void OnGraphUnpaused()
  {
  }

  public void SendEvent(string name) => this.SendEvent(new EventData(name));

  public void SendEvent<T>(string name, T value)
  {
    this.SendEvent((EventData) new EventData<T>(name, value));
  }

  public void SendEvent(EventData eventData)
  {
    if (!this.isRunning || eventData == null || !((UnityEngine.Object) this.agent != (UnityEngine.Object) null))
      return;
    MessageRouter component = this.agent.GetComponent<MessageRouter>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.Dispatch<EventData>("OnCustomEvent", eventData);
    component.Dispatch<object>(eventData.name, eventData.value);
  }

  public static void SendGlobalEvent(string name) => Graph.SendGlobalEvent(new EventData(name));

  public static void SendGlobalEvent<T>(string name, T value)
  {
    Graph.SendGlobalEvent((EventData) new EventData<T>(name, value));
  }

  public static void SendGlobalEvent(EventData eventData)
  {
    List<GameObject> gameObjectList = new List<GameObject>();
    foreach (Graph graph in Graph.runningGraphs.ToArray())
    {
      if ((UnityEngine.Object) graph.agent != (UnityEngine.Object) null && !gameObjectList.Contains(graph.agent.gameObject))
      {
        gameObjectList.Add(graph.agent.gameObject);
        graph.SendEvent(eventData);
      }
    }
  }

  public Node GetNodeWithID(int searchID)
  {
    return searchID <= this.allNodes.Count && searchID >= 0 ? this.allNodes.Find((Predicate<Node>) (n => n.ID == searchID)) : (Node) null;
  }

  public List<T> GetAllNodesOfType<T>() where T : Node => this.allNodes.OfType<T>().ToList<T>();

  public T GetNodeWithTag<T>(string tagName) where T : Node
  {
    foreach (T nodeWithTag in this.allNodes.OfType<T>())
    {
      if (nodeWithTag.tag == tagName)
        return nodeWithTag;
    }
    return default (T);
  }

  public List<T> GetNodesWithTag<T>(string tagName) where T : Node
  {
    List<T> nodesWithTag = new List<T>();
    foreach (T obj in this.allNodes.OfType<T>())
    {
      if (obj.tag == tagName)
        nodesWithTag.Add(obj);
    }
    return nodesWithTag;
  }

  public List<T> GetAllTagedNodes<T>() where T : Node
  {
    List<T> allTagedNodes = new List<T>();
    foreach (T obj in this.allNodes.OfType<T>())
    {
      if (!string.IsNullOrEmpty(obj.tag))
        allTagedNodes.Add(obj);
    }
    return allTagedNodes;
  }

  public T GetNodeWithName<T>(string name) where T : Node
  {
    foreach (T nodeWithName in this.allNodes.OfType<T>())
    {
      if (this.StripNameColor(nodeWithName.name).ToLower() == name.ToLower())
        return nodeWithName;
    }
    return default (T);
  }

  public string StripNameColor(string name)
  {
    if (name.StartsWith("<") && name.EndsWith(">"))
    {
      name = name.Replace(name.Substring(0, name.IndexOf(">") + 1), "");
      name = name.Replace(name.Substring(name.IndexOf("<"), name.LastIndexOf(">") + 1 - name.IndexOf("<")), "");
    }
    return name;
  }

  public List<Node> GetRootNodes()
  {
    return this.allNodes.Where<Node>((Func<Node, bool>) (n => n.inConnections.Count == 0)).ToList<Node>();
  }

  public List<Node> GetLeafNodes()
  {
    return this.allNodes.Where<Node>((Func<Node, bool>) (n => n.inConnections.Count == 0)).ToList<Node>();
  }

  public List<T> GetAllNestedGraphs<T>(bool recursive) where T : Graph
  {
    List<T> allNestedGraphs = new List<T>();
    foreach (IGraphAssignable graphAssignable in this.allNodes.OfType<IGraphAssignable>())
    {
      if (graphAssignable.nestedGraph is T)
      {
        if (!allNestedGraphs.Contains((T) graphAssignable.nestedGraph))
          allNestedGraphs.Add((T) graphAssignable.nestedGraph);
        if (recursive)
          allNestedGraphs.AddRange((IEnumerable<T>) graphAssignable.nestedGraph.GetAllNestedGraphs<T>(recursive));
      }
    }
    return allNestedGraphs;
  }

  public List<Graph> GetAllInstancedNestedGraphs()
  {
    List<Graph> instancedNestedGraphs = new List<Graph>();
    foreach (IGraphAssignable graphAssignable in this.allNodes.OfType<IGraphAssignable>())
    {
      Graph[] instances = graphAssignable.GetInstances();
      instancedNestedGraphs.AddRange((IEnumerable<Graph>) instances);
      foreach (Graph graph in instances)
        instancedNestedGraphs.AddRange((IEnumerable<Graph>) graph.GetAllInstancedNestedGraphs());
    }
    return instancedNestedGraphs;
  }

  public List<T> GetAllTasksOfType<T>() where T : Task
  {
    List<Task> taskList = new List<Task>();
    List<T> allTasksOfType = new List<T>();
    for (int index1 = 0; index1 < this.allNodes.Count; ++index1)
    {
      Node allNode = this.allNodes[index1];
      if (allNode is ITaskAssignable && (allNode as ITaskAssignable).task != null)
        taskList.Add((allNode as ITaskAssignable).task);
      if (allNode is ISubTasksContainer)
        taskList.AddRange((IEnumerable<Task>) (allNode as ISubTasksContainer).GetSubTasks());
      for (int index2 = 0; index2 < allNode.outConnections.Count; ++index2)
      {
        Connection outConnection = allNode.outConnections[index2];
        if (outConnection is ITaskAssignable && (outConnection as ITaskAssignable).task != null)
          taskList.Add((outConnection as ITaskAssignable).task);
        if (outConnection is ISubTasksContainer)
          taskList.AddRange((IEnumerable<Task>) (outConnection as ISubTasksContainer).GetSubTasks());
      }
    }
    for (int index = 0; index < taskList.Count; ++index)
    {
      Task task = taskList[index];
      if (task is ActionList)
        allTasksOfType.AddRange((task as ActionList).actions.OfType<T>());
      if (task is ConditionList)
        allTasksOfType.AddRange((task as ConditionList).conditions.OfType<T>());
      if (task is T obj)
        allTasksOfType.Add(obj);
    }
    return allTasksOfType;
  }

  public Node GetTaskParent(Task task)
  {
    if (task == null)
      return (Node) null;
    for (int index1 = 0; index1 < this.allNodes.Count; ++index1)
    {
      Node allNode = this.allNodes[index1];
      if (allNode is ITaskAssignable)
      {
        Task task1 = ((ITaskAssignable) allNode).task;
        if (task1 == task)
          return allNode;
        if (task1 is ActionList)
        {
          List<ActionTask> actions = (task1 as ActionList).actions;
          for (int index2 = 0; index2 < actions.Count; ++index2)
          {
            if (actions[index2] == task)
              return allNode;
          }
        }
        if (task1 is ConditionList)
        {
          List<ConditionTask> conditions = (task1 as ConditionList).conditions;
          for (int index3 = 0; index3 < conditions.Count; ++index3)
          {
            if (conditions[index3] == task)
              return allNode;
          }
        }
      }
      if (allNode is ISubTasksContainer)
      {
        foreach (Task subTask in ((ISubTasksContainer) allNode).GetSubTasks())
        {
          if (subTask == task)
            return allNode;
          if (subTask is ActionList)
          {
            List<ActionTask> actions = (subTask as ActionList).actions;
            for (int index4 = 0; index4 < actions.Count; ++index4)
            {
              if (actions[index4] == task)
                return allNode;
            }
          }
          if (subTask is ConditionList)
          {
            List<ConditionTask> conditions = (subTask as ConditionList).conditions;
            for (int index5 = 0; index5 < conditions.Count; ++index5)
            {
              if (conditions[index5] == task)
                return allNode;
            }
          }
        }
      }
    }
    return (Node) null;
  }

  public static List<string> GetUsedParameterNamesOfTarget(object target)
  {
    List<string> parameterNamesOfTarget = new List<string>();
    if (target == null)
      return parameterNamesOfTarget;
    parameterNamesOfTarget.AddRange(BBParameter.GetObjectBBParameters(target).Select<BBParameter, string>((Func<BBParameter, string>) (p => p.name)));
    if (target is Task o)
    {
      parameterNamesOfTarget.AddRange(BBParameter.GetObjectBBParameters((object) o).Select<BBParameter, string>((Func<BBParameter, string>) (p => p.name)));
      if (!string.IsNullOrEmpty(o.overrideAgentParameterName))
        parameterNamesOfTarget.Add(o.overrideAgentParameterName);
    }
    if (target is ActionList actionList)
    {
      for (int index = 0; index < actionList.actions.Count; ++index)
      {
        ActionTask action = actionList.actions[index];
        parameterNamesOfTarget.AddRange(BBParameter.GetObjectBBParameters((object) action).Select<BBParameter, string>((Func<BBParameter, string>) (p => p.name)));
        if (!string.IsNullOrEmpty(action.overrideAgentParameterName))
          parameterNamesOfTarget.Add(action.overrideAgentParameterName);
      }
    }
    if (target is ConditionList conditionList)
    {
      for (int index = 0; index < conditionList.conditions.Count; ++index)
      {
        ConditionTask condition = conditionList.conditions[index];
        parameterNamesOfTarget.AddRange(BBParameter.GetObjectBBParameters((object) condition).Select<BBParameter, string>((Func<BBParameter, string>) (p => p.name)));
        if (!string.IsNullOrEmpty(condition.overrideAgentParameterName))
          parameterNamesOfTarget.Add(condition.overrideAgentParameterName);
      }
    }
    if (target is ISubTasksContainer subTasksContainer)
    {
      foreach (object subTask in subTasksContainer.GetSubTasks())
        parameterNamesOfTarget.AddRange((IEnumerable<string>) Graph.GetUsedParameterNamesOfTarget(subTask));
    }
    if (target is ITaskAssignable taskAssignable && taskAssignable.task != null)
      parameterNamesOfTarget.AddRange((IEnumerable<string>) Graph.GetUsedParameterNamesOfTarget((object) taskAssignable.task));
    return parameterNamesOfTarget;
  }

  public static Graph GetElementGraph(object obj)
  {
    switch (obj)
    {
      case null:
        return (Graph) null;
      case Graph _:
        return (Graph) obj;
      case Node _:
        return (obj as Node).graph;
      case Connection _:
        return (obj as Connection).sourceNode.graph;
      case Task _:
        Task task = (Task) obj;
        Graph ownerSystem = task.ownerSystem as Graph;
        if ((UnityEngine.Object) ownerSystem != (UnityEngine.Object) null)
        {
          Node taskParent = ownerSystem.GetTaskParent(task);
          if (taskParent != null)
            return taskParent.graph;
          break;
        }
        break;
    }
    return (Graph) null;
  }

  public BBParameter[] GetDefinedParameters()
  {
    List<BBParameter> bbParameterList = new List<BBParameter>();
    List<object> objectList = new List<object>();
    objectList.AddRange(this.GetAllTasksOfType<Task>().Cast<object>());
    objectList.AddRange(this.allNodes.Cast<object>());
    for (int index = 0; index < objectList.Count; ++index)
    {
      object o = objectList[index];
      if (o is Task)
      {
        Task task = (Task) o;
        if (task.agentIsOverride && !string.IsNullOrEmpty(task.overrideAgentParameterName))
          bbParameterList.Add(typeof (Task).RTGetField("overrideAgent").GetValue((object) task) as BBParameter);
      }
      foreach (BBParameter objectBbParameter in BBParameter.GetObjectBBParameters(o))
      {
        if (objectBbParameter != null && objectBbParameter.useBlackboard && !objectBbParameter.isNone)
          bbParameterList.Add(objectBbParameter);
      }
    }
    return bbParameterList.ToArray();
  }

  public void CreateDefinedParameterVariables(IBlackboard bb)
  {
    foreach (BBParameter definedParameter in this.GetDefinedParameters())
      definedParameter.PromoteToVariable(bb);
  }

  public T AddNode<T>() where T : Node => (T) this.AddNode(typeof (T));

  public T AddNode<T>(Vector2 pos) where T : Node => (T) this.AddNode(typeof (T), pos);

  public Node AddNode(System.Type nodeType) => this.AddNode(nodeType, new Vector2(50f, 50f));

  public Node AddNode(System.Type nodeType, Vector2 pos)
  {
    if (nodeType.IsGenericTypeDefinition)
      nodeType = nodeType.RTMakeGenericType(nodeType.GetFirstGenericParameterConstraintType());
    if (!nodeType.RTIsSubclassOf(this.baseNodeType))
    {
      ParadoxNotion.Services.Logger.LogWarning((object) $"{nodeType?.ToString()} can't be added to {this.GetType().FriendlyName()} graph", "NodeCanvas", (object) this);
      return (Node) null;
    }
    Node node = Node.Create(this, nodeType, pos);
    this.RecordUndo("New Node");
    this.allNodes.Add(node);
    if (this.primeNode == null)
      this.primeNode = node;
    this.UpdateNodeIDs(false);
    return node;
  }

  public void RemoveNode(Node node, bool recordUndo = true)
  {
    if (ReflectionTools.RTIsDefined<ProtectedAttribute>(node.GetType(), true))
      return;
    if (!this.allNodes.Contains(node))
    {
      ParadoxNotion.Services.Logger.LogWarning((object) "Node is not part of this graph", "NodeCanvas", (object) this);
    }
    else
    {
      node.OnDestroy();
      foreach (Connection connection in node.inConnections.ToArray())
        this.RemoveConnection(connection);
      foreach (Connection connection in node.outConnections.ToArray())
        this.RemoveConnection(connection);
      if (recordUndo)
        this.RecordUndo("Delete Node");
      this.allNodes.Remove(node);
      if (node == this.primeNode)
        this.primeNode = this.GetNodeWithID(this.primeNode.ID + 1);
      this.UpdateNodeIDs(false);
    }
  }

  public Connection ConnectNodes(Node sourceNode, Node targetNode)
  {
    return this.ConnectNodes(sourceNode, targetNode, sourceNode.outConnections.Count);
  }

  public Connection ConnectNodes(Node sourceNode, Node targetNode, int indexToInsert)
  {
    if (!targetNode.IsNewConnectionAllowed(sourceNode))
      return (Connection) null;
    this.RecordUndo("New Connection");
    Connection connection = Connection.Create(sourceNode, targetNode, indexToInsert);
    sourceNode.OnChildConnected(indexToInsert);
    targetNode.OnParentConnected(targetNode.inConnections.IndexOf(connection));
    this.UpdateNodeIDs(false);
    return connection;
  }

  public void RemoveConnection(Connection connection, bool recordUndo = true)
  {
    if (Application.isPlaying)
      connection.Reset();
    if (recordUndo)
      this.RecordUndo("Delete Connection");
    connection.OnDestroy();
    connection.sourceNode.OnChildDisconnected(connection.sourceNode.outConnections.IndexOf(connection));
    connection.targetNode.OnParentDisconnected(connection.targetNode.inConnections.IndexOf(connection));
    connection.sourceNode.outConnections.Remove(connection);
    connection.targetNode.inConnections.Remove(connection);
    this.UpdateNodeIDs(false);
  }

  public void RecordUndo(string name)
  {
  }
}
