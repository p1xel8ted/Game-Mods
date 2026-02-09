// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Framework.Connection
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework.Internal;
using ParadoxNotion.Design;
using ParadoxNotion.Serialization;
using System;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Framework;

[SpoofAOT]
public abstract class Connection
{
  [SerializeField]
  public Node _sourceNode;
  [SerializeField]
  public Node _targetNode;
  [SerializeField]
  public bool _isDisabled;
  [NonSerialized]
  public NodeCanvas.Status _status = NodeCanvas.Status.Resting;

  public Node sourceNode
  {
    get => this._sourceNode;
    set => this._sourceNode = value;
  }

  public Node targetNode
  {
    get => this._targetNode;
    set => this._targetNode = value;
  }

  public bool isActive
  {
    get => !this._isDisabled;
    set
    {
      if (!this._isDisabled && !value)
        this.Reset();
      this._isDisabled = !value;
    }
  }

  public NodeCanvas.Status status
  {
    get => this._status;
    set => this._status = value;
  }

  public Graph graph => this.sourceNode.graph;

  public static Connection Create(Node source, Node target, int sourceIndex)
  {
    if (source == null || target == null)
    {
      Debug.LogError((object) "Can't Create a Connection without providing Source and Target Nodes");
      return (Connection) null;
    }
    if (source is MissingNode)
    {
      Debug.LogError((object) "Creating new Connections from a 'MissingNode' is not allowed. Please resolve the MissingNode node first");
      return (Connection) null;
    }
    Connection instance = (Connection) Activator.CreateInstance(source.outConnectionType);
    if ((UnityEngine.Object) source.graph != (UnityEngine.Object) null)
      source.graph.RecordUndo("Create Connection");
    instance.sourceNode = source;
    instance.targetNode = target;
    source.outConnections.Insert(sourceIndex, instance);
    target.inConnections.Add(instance);
    int targetIndex = target.inConnections.IndexOf(instance);
    instance.OnValidate(sourceIndex, targetIndex);
    instance.OnCreate(sourceIndex, targetIndex);
    return instance;
  }

  public Connection Duplicate(Node newSource, Node newTarget)
  {
    if (newSource == null || newTarget == null)
    {
      Debug.LogError((object) "Can't Duplicate a Connection without providing NewSource and NewTarget Nodes");
      return (Connection) null;
    }
    Connection connection = JSONSerializer.Clone<Connection>(this);
    if ((UnityEngine.Object) newSource.graph != (UnityEngine.Object) null)
      newSource.graph.RecordUndo("Duplicate Connection");
    connection.SetSource(newSource, false);
    connection.SetTarget(newTarget, false);
    if (this is ITaskAssignable taskAssignable && taskAssignable.task != null)
      (connection as ITaskAssignable).task = taskAssignable.task.Duplicate((ITaskSystem) newSource.graph);
    int sourceIndex = newSource.outConnections.IndexOf(connection);
    int targetIndex = newTarget.inConnections.IndexOf(connection);
    connection.OnValidate(sourceIndex, targetIndex);
    return connection;
  }

  public virtual void OnCreate(int sourceIndex, int targetIndex)
  {
  }

  public virtual void OnValidate(int sourceIndex, int targetIndex)
  {
  }

  public virtual void OnDestroy()
  {
  }

  public void SetSource(Node newSource, bool isRelink = true)
  {
    if (this.sourceNode == newSource)
      return;
    if ((UnityEngine.Object) this.graph != (UnityEngine.Object) null)
      this.graph.RecordUndo("Set Source");
    if (isRelink)
    {
      int connectionIndex = this.sourceNode.outConnections.IndexOf(this);
      this.sourceNode.OnChildDisconnected(connectionIndex);
      newSource.OnChildConnected(connectionIndex);
      this.sourceNode.outConnections.Remove(this);
    }
    newSource.outConnections.Add(this);
    this.sourceNode = newSource;
  }

  public void SetTarget(Node newTarget, bool isRelink = true)
  {
    if (this.targetNode == newTarget)
      return;
    if ((UnityEngine.Object) this.graph != (UnityEngine.Object) null)
      this.graph.RecordUndo("Set Target");
    if (isRelink)
    {
      int connectionIndex = this.targetNode.inConnections.IndexOf(this);
      this.targetNode.OnParentDisconnected(connectionIndex);
      newTarget.OnParentConnected(connectionIndex);
      this.targetNode.inConnections.Remove(this);
    }
    newTarget.inConnections.Add(this);
    this.targetNode = newTarget;
  }

  public NodeCanvas.Status Execute(Component agent, IBlackboard blackboard)
  {
    if (!this.isActive)
      return NodeCanvas.Status.Resting;
    this.status = this.targetNode.Execute(agent, blackboard);
    return this.status;
  }

  public void Reset(bool recursively = true)
  {
    if (this.status == NodeCanvas.Status.Resting)
      return;
    this.status = NodeCanvas.Status.Resting;
    if (!recursively)
      return;
    this.targetNode.Reset(recursively);
  }
}
