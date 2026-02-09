// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Framework.Node
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using ParadoxNotion;
using ParadoxNotion.Design;
using ParadoxNotion.Serialization;
using ParadoxNotion.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Framework;

[SpoofAOT]
[Serializable]
public abstract class Node
{
  [SerializeField]
  public Vector2 _position;
  [SerializeField]
  public string _UID;
  [SerializeField]
  public string _name;
  [SerializeField]
  public string _tag;
  [SerializeField]
  public string _comment;
  [SerializeField]
  public bool _isBreakpoint;
  public Graph _graph;
  public List<Connection> _inConnections = new List<Connection>();
  public List<Connection> _outConnections = new List<Connection>();
  public int _ID;
  [NonSerialized]
  public NodeCanvas.Status _status = NodeCanvas.Status.Resting;
  [NonSerialized]
  public string _nodeName;
  [NonSerialized]
  public string _nodeDescription;
  [CompilerGenerated]
  public bool \u003CisChecked\u003Ek__BackingField;

  public Graph graph
  {
    get => this._graph;
    set => this._graph = value;
  }

  public int ID
  {
    get => this._ID;
    set => this._ID = value;
  }

  public List<Connection> inConnections
  {
    get => this._inConnections;
    set => this._inConnections = value;
  }

  public List<Connection> outConnections
  {
    get => this._outConnections;
    set => this._outConnections = value;
  }

  public Vector2 nodePosition
  {
    get => this._position;
    set => this._position = value;
  }

  public string UID
  {
    get => !string.IsNullOrEmpty(this._UID) ? this._UID : (this._UID = Guid.NewGuid().ToString());
  }

  public string customName
  {
    get => this._name;
    set => this._name = value;
  }

  public string tag
  {
    get => this._tag;
    set => this._tag = value;
  }

  public string nodeComment
  {
    get => this._comment;
    set => this._comment = value;
  }

  public bool isBreakpoint
  {
    get => this._isBreakpoint;
    set => this._isBreakpoint = value;
  }

  public virtual string name
  {
    get
    {
      if (!string.IsNullOrEmpty(this.customName))
        return this.customName;
      if (string.IsNullOrEmpty(this._nodeName))
      {
        NameAttribute attribute = ReflectionTools.RTGetAttribute<NameAttribute>(this.GetType(), true);
        this._nodeName = attribute != null ? attribute.name : this.GetType().FriendlyName().SplitCamelCase();
      }
      return this._nodeName;
    }
    set => this.customName = value;
  }

  public virtual string description
  {
    get
    {
      if (string.IsNullOrEmpty(this._nodeDescription))
      {
        DescriptionAttribute attribute = ReflectionTools.RTGetAttribute<DescriptionAttribute>(this.GetType(), true);
        this._nodeDescription = attribute != null ? attribute.description : "No Description";
      }
      return this._nodeDescription;
    }
  }

  public abstract int maxInConnections { get; }

  public abstract int maxOutConnections { get; }

  public abstract System.Type outConnectionType { get; }

  public abstract bool allowAsPrime { get; }

  public abstract Alignment2x2 commentsAlignment { get; }

  public abstract Alignment2x2 iconAlignment { get; }

  public NodeCanvas.Status status
  {
    get => this._status;
    set => this._status = value;
  }

  public Component graphAgent
  {
    get => !((UnityEngine.Object) this.graph != (UnityEngine.Object) null) ? (Component) null : this.graph.agent;
  }

  public IBlackboard graphBlackboard
  {
    get => !((UnityEngine.Object) this.graph != (UnityEngine.Object) null) ? (IBlackboard) null : this.graph.blackboard;
  }

  public bool isChecked
  {
    get => this.\u003CisChecked\u003Ek__BackingField;
    set => this.\u003CisChecked\u003Ek__BackingField = value;
  }

  public static Node Create(Graph targetGraph, System.Type nodeType, Vector2 pos)
  {
    if ((UnityEngine.Object) targetGraph == (UnityEngine.Object) null)
    {
      ParadoxNotion.Services.Logger.LogError((object) "Can't Create a Node without providing a Target Graph", "NodeCanvas");
      return (Node) null;
    }
    Node instance = (Node) Activator.CreateInstance(nodeType);
    if ((UnityEngine.Object) targetGraph != (UnityEngine.Object) null)
      targetGraph.RecordUndo("Create Node");
    instance.graph = targetGraph;
    instance.nodePosition = pos;
    BBParameter.SetBBFields((object) instance, targetGraph.blackboard);
    instance.OnValidate(targetGraph);
    instance.OnCreate(targetGraph);
    return instance;
  }

  public Node Duplicate(Graph targetGraph)
  {
    if ((UnityEngine.Object) targetGraph == (UnityEngine.Object) null)
    {
      ParadoxNotion.Services.Logger.LogError((object) "Can't duplicate a Node without providing a Target Graph", "NodeCanvas");
      return (Node) null;
    }
    Node o = JSONSerializer.Clone<Node>(this);
    if ((UnityEngine.Object) targetGraph != (UnityEngine.Object) null)
      targetGraph.RecordUndo("Duplicate Node");
    targetGraph.allNodes.Add(o);
    o.inConnections.Clear();
    o.outConnections.Clear();
    if ((UnityEngine.Object) targetGraph == (UnityEngine.Object) this.graph)
      o.nodePosition += new Vector2(50f, 50f);
    o._UID = (string) null;
    o.graph = targetGraph;
    BBParameter.SetBBFields((object) o, targetGraph.blackboard);
    if (this is ITaskAssignable taskAssignable && taskAssignable.task != null)
      (o as ITaskAssignable).task = taskAssignable.task.Duplicate((ITaskSystem) targetGraph);
    o.OnValidate(targetGraph);
    return o;
  }

  public virtual void OnCreate(Graph assignedGraph)
  {
  }

  public virtual void OnValidate(Graph assignedGraph)
  {
  }

  public virtual void OnDestroy()
  {
  }

  public NodeCanvas.Status Execute(Component agent, IBlackboard blackboard)
  {
    if (this.isChecked)
      return this.Error("Infinite Loop. Please check for other errors that may have caused this in the log before this.");
    this.isChecked = true;
    this.status = this.OnExecute(agent, blackboard);
    this.isChecked = false;
    return this.status;
  }

  public IEnumerator YieldBreak(Component agent, IBlackboard blackboard)
  {
    Debug.Break();
    yield return (object) null;
    this.status = this.OnExecute(agent, blackboard);
  }

  public NodeCanvas.Status Error(string error)
  {
    ParadoxNotion.Services.Logger.LogError((object) $"{error} | On Node '{this.name}' | ID '{this.ID}' | Graph '{this.graph.name}'", "Execution Error", (object) this);
    this.status = NodeCanvas.Status.Error;
    return NodeCanvas.Status.Error;
  }

  public NodeCanvas.Status Fail(Exception e)
  {
    ParadoxNotion.Services.Logger.LogException(e, "Execution Failure", (object) this);
    this.status = NodeCanvas.Status.Failure;
    return NodeCanvas.Status.Failure;
  }

  public NodeCanvas.Status Fail(string error)
  {
    ParadoxNotion.Services.Logger.LogError((object) $"{error} | On Node '{this.name}' | ID '{this.ID}' | Graph '{this.graph.name}'", "Execution Failure", (object) this);
    this.status = NodeCanvas.Status.Failure;
    return NodeCanvas.Status.Failure;
  }

  public void SetStatus(NodeCanvas.Status status) => this.status = status;

  public void Reset(bool recursively = true)
  {
    if (this.status == NodeCanvas.Status.Resting || this.isChecked)
      return;
    this.OnReset();
    this.status = NodeCanvas.Status.Resting;
    this.isChecked = true;
    for (int index = 0; index < this.outConnections.Count; ++index)
      this.outConnections[index].Reset(recursively);
    this.isChecked = false;
  }

  public void SendEvent(EventData eventData) => this.graph.SendEvent(eventData);

  public void RegisterEvents(params string[] eventNames)
  {
    this.RegisterEvents(this.graphAgent, eventNames);
  }

  public void RegisterEvents(Component targetAgent, params string[] eventNames)
  {
    if ((UnityEngine.Object) targetAgent == (UnityEngine.Object) null)
    {
      ParadoxNotion.Services.Logger.LogError((object) "Null Agent provided for event registration", "Events", (object) this);
    }
    else
    {
      MessageRouter messageRouter = targetAgent.GetComponent<MessageRouter>();
      if ((UnityEngine.Object) messageRouter == (UnityEngine.Object) null)
        messageRouter = targetAgent.gameObject.AddComponent<MessageRouter>();
      messageRouter.Register((object) this, eventNames);
    }
  }

  public void UnRegisterEvents(params string[] eventNames)
  {
    this.UnRegisterEvents(this.graphAgent, eventNames);
  }

  public void UnRegisterEvents(Component targetAgent, params string[] eventNames)
  {
    if ((UnityEngine.Object) targetAgent == (UnityEngine.Object) null)
      return;
    MessageRouter component = targetAgent.GetComponent<MessageRouter>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.UnRegister((object) this, eventNames);
  }

  public void UnregisterAllEvents() => this.UnregisterAllEvents(this.graphAgent);

  public void UnregisterAllEvents(Component targetAgent)
  {
    if ((UnityEngine.Object) targetAgent == (UnityEngine.Object) null)
      return;
    MessageRouter component = targetAgent.GetComponent<MessageRouter>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.UnRegister((object) this);
  }

  public bool IsNewConnectionAllowed() => this.IsNewConnectionAllowed((Node) null);

  public bool IsNewConnectionAllowed(Node sourceNode)
  {
    if (sourceNode != null)
    {
      if (this == sourceNode)
      {
        ParadoxNotion.Services.Logger.LogWarning((object) "Node can't connect to itself", "Editor", (object) this);
        return false;
      }
      if (sourceNode.outConnections.Count >= sourceNode.maxOutConnections && sourceNode.maxOutConnections != -1)
      {
        ParadoxNotion.Services.Logger.LogWarning((object) "Source node can have no more out connections.", "Editor", (object) this);
        return false;
      }
    }
    if (this == this.graph.primeNode && this.maxInConnections == 1)
    {
      ParadoxNotion.Services.Logger.LogWarning((object) "Target node can have no more connections", "Editor", (object) this);
      return false;
    }
    if (this.maxInConnections > this.inConnections.Count || this.maxInConnections == -1)
      return true;
    ParadoxNotion.Services.Logger.LogWarning((object) "Target node can have no more connections", "Editor", (object) this);
    return false;
  }

  public int AssignIDToGraph(int lastID)
  {
    if (this.isChecked)
      return lastID;
    this.isChecked = true;
    ++lastID;
    this.ID = lastID;
    for (int index = 0; index < this.outConnections.Count; ++index)
      lastID = this.outConnections[index].targetNode.AssignIDToGraph(lastID);
    return lastID;
  }

  public void ResetRecursion()
  {
    if (!this.isChecked)
      return;
    this.isChecked = false;
    for (int index = 0; index < this.outConnections.Count; ++index)
      this.outConnections[index].targetNode.ResetRecursion();
  }

  public Coroutine StartCoroutine(IEnumerator routine)
  {
    return MonoManager.current.StartCoroutine(routine);
  }

  public void StopCoroutine(Coroutine routine) => MonoManager.current.StopCoroutine(routine);

  public List<Node> GetParentNodes()
  {
    return this.inConnections.Count != 0 ? this.inConnections.Select<Connection, Node>((Func<Connection, Node>) (c => c.sourceNode)).ToList<Node>() : new List<Node>();
  }

  public List<Node> GetChildNodes()
  {
    return this.outConnections.Count != 0 ? this.outConnections.Select<Connection, Node>((Func<Connection, Node>) (c => c.targetNode)).ToList<Node>() : new List<Node>();
  }

  public virtual NodeCanvas.Status OnExecute(Component agent, IBlackboard blackboard)
  {
    return this.status;
  }

  public virtual void OnReset()
  {
  }

  public virtual void OnParentConnected(int connectionIndex)
  {
  }

  public virtual void OnParentDisconnected(int connectionIndex)
  {
  }

  public virtual void OnChildConnected(int connectionIndex)
  {
  }

  public virtual void OnChildDisconnected(int connectionIndex)
  {
  }

  public virtual void OnGraphStarted()
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

  public sealed override string ToString() => $"{this.name} ({this.tag})";

  public void OnDrawGizmos()
  {
    if (!(this is ITaskAssignable) || (this as ITaskAssignable).task == null)
      return;
    (this as ITaskAssignable).task.OnDrawGizmos();
  }

  public void OnDrawGizmosSelected()
  {
    if (!(this is ITaskAssignable) || (this as ITaskAssignable).task == null)
      return;
    (this as ITaskAssignable).task.OnDrawGizmosSelected();
  }
}
