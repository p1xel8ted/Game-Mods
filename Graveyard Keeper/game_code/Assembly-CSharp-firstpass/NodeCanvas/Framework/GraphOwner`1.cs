// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Framework.GraphOwner`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Framework;

public abstract class GraphOwner<T> : GraphOwner where T : Graph
{
  [SerializeField]
  public T _graph;
  [SerializeField]
  public UnityEngine.Object _blackboard;

  public sealed override Graph graph
  {
    get => (Graph) this._graph;
    set => this._graph = (T) value;
  }

  public T behaviour
  {
    get => this._graph;
    set => this._graph = value;
  }

  public sealed override IBlackboard blackboard
  {
    get
    {
      if ((UnityEngine.Object) this.graph != (UnityEngine.Object) null && this.graph.useLocalBlackboard)
        return (IBlackboard) this.graph.localBlackboard;
      if (this._blackboard == (UnityEngine.Object) null)
        this._blackboard = (UnityEngine.Object) this.GetComponent<Blackboard>();
      return this._blackboard as IBlackboard;
    }
    set
    {
      if ((object) this._blackboard == (object) value)
        return;
      this._blackboard = (UnityEngine.Object) value;
      if (!((UnityEngine.Object) this.graph != (UnityEngine.Object) null) || this.graph.useLocalBlackboard)
        return;
      this.graph.blackboard = value;
    }
  }

  public sealed override System.Type graphType => typeof (T);

  public void StartBehaviour(T newGraph) => this.SwitchBehaviour(newGraph);

  public void StartBehaviour(T newGraph, Action<bool> callback)
  {
    this.SwitchBehaviour(newGraph, callback);
  }

  public void SwitchBehaviour(T newGraph) => this.SwitchBehaviour(newGraph, (Action<bool>) null);

  public void SwitchBehaviour(T newGraph, Action<bool> callback)
  {
    this.StopBehaviour();
    this.graph = (Graph) newGraph;
    this.StartBehaviour(callback);
  }
}
