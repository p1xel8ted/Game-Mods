// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Framework.GraphOwner
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Framework;

public abstract class GraphOwner : MonoBehaviour
{
  [HideInInspector]
  [SerializeField]
  public string boundGraphSerialization;
  [SerializeField]
  [HideInInspector]
  public List<UnityEngine.Object> boundGraphObjectReferences;
  [HideInInspector]
  public GraphOwner.EnableAction enableAction;
  [HideInInspector]
  public GraphOwner.DisableAction disableAction;
  public static Action<GraphOwner> onOwnerBehaviourStateChange;
  public Dictionary<Graph, Graph> instances = new Dictionary<Graph, Graph>();
  public bool awakeCalled;
  public bool startCalled;
  public static bool isQuiting;

  public abstract Graph graph { get; set; }

  public abstract IBlackboard blackboard { get; set; }

  public abstract System.Type graphType { get; }

  public bool isRunning => (UnityEngine.Object) this.graph != (UnityEngine.Object) null && this.graph.isRunning;

  public bool isPaused => (UnityEngine.Object) this.graph != (UnityEngine.Object) null && this.graph.isPaused;

  public float elapsedTime
  {
    get => !((UnityEngine.Object) this.graph != (UnityEngine.Object) null) ? 0.0f : this.graph.elapsedTime;
  }

  public Graph GetInstance(Graph originalGraph)
  {
    if ((UnityEngine.Object) originalGraph == (UnityEngine.Object) null)
      return (Graph) null;
    if (this.instances.ContainsValue(originalGraph))
      return originalGraph;
    Graph instance = (Graph) null;
    if (!this.instances.TryGetValue(originalGraph, out instance))
    {
      instance = Graph.Clone<Graph>(originalGraph);
      this.instances[originalGraph] = instance;
    }
    instance.agent = (Component) this;
    instance.blackboard = this.blackboard;
    return instance;
  }

  public void StartBehaviour()
  {
    this.graph = this.GetInstance(this.graph);
    if (!((UnityEngine.Object) this.graph != (UnityEngine.Object) null))
      return;
    this.graph.StartGraph((Component) this, this.blackboard, true);
    if (GraphOwner.onOwnerBehaviourStateChange == null)
      return;
    GraphOwner.onOwnerBehaviourStateChange(this);
  }

  public void StartBehaviour(Action<bool> callback)
  {
    this.graph = this.GetInstance(this.graph);
    if (!((UnityEngine.Object) this.graph != (UnityEngine.Object) null))
      return;
    this.graph.StartGraph((Component) this, this.blackboard, true, callback);
    if (GraphOwner.onOwnerBehaviourStateChange == null)
      return;
    GraphOwner.onOwnerBehaviourStateChange(this);
  }

  public void PauseBehaviour()
  {
    if (!((UnityEngine.Object) this.graph != (UnityEngine.Object) null))
      return;
    this.graph.Pause();
    if (GraphOwner.onOwnerBehaviourStateChange == null)
      return;
    GraphOwner.onOwnerBehaviourStateChange(this);
  }

  public void StopBehaviour()
  {
    if (!((UnityEngine.Object) this.graph != (UnityEngine.Object) null))
      return;
    this.graph.Stop();
    if (GraphOwner.onOwnerBehaviourStateChange == null)
      return;
    GraphOwner.onOwnerBehaviourStateChange(this);
  }

  public void UpdateBehaviour()
  {
    if (!((UnityEngine.Object) this.graph != (UnityEngine.Object) null))
      return;
    this.graph.UpdateGraph();
  }

  public void SendEvent(string eventName) => this.SendEvent(new EventData(eventName));

  public void SendEvent<T>(string eventName, T eventValue)
  {
    this.SendEvent((EventData) new EventData<T>(eventName, eventValue));
  }

  public void SendEvent(EventData eventData)
  {
    if (!((UnityEngine.Object) this.graph != (UnityEngine.Object) null))
      return;
    this.graph.SendEvent(eventData);
  }

  public static void SendGlobalEvent(string eventName)
  {
    Graph.SendGlobalEvent(new EventData(eventName));
  }

  public static void SendGlobalEvent<T>(string eventName, T eventValue)
  {
    Graph.SendGlobalEvent((EventData) new EventData<T>(eventName, eventValue));
  }

  public void Awake()
  {
    if (this.awakeCalled)
      return;
    this.awakeCalled = true;
    if (!string.IsNullOrEmpty(this.boundGraphSerialization))
    {
      if ((UnityEngine.Object) this.graph == (UnityEngine.Object) null)
      {
        this.graph = (Graph) ScriptableObject.CreateInstance(this.graphType);
        this.graph.Deserialize(this.boundGraphSerialization, true, this.boundGraphObjectReferences);
        this.instances[this.graph] = this.graph;
        return;
      }
      this.graph.SetSerializationObjectReferences(this.boundGraphObjectReferences);
    }
    this.graph = this.GetInstance(this.graph);
  }

  public void Start()
  {
    this.startCalled = true;
    if (this.enableAction != GraphOwner.EnableAction.EnableBehaviour)
      return;
    this.StartBehaviour();
  }

  public void OnEnable()
  {
    if (!this.startCalled || this.enableAction != GraphOwner.EnableAction.EnableBehaviour)
      return;
    this.StartBehaviour();
  }

  public void OnDisable()
  {
    if (GraphOwner.isQuiting)
      return;
    if (this.disableAction == GraphOwner.DisableAction.DisableBehaviour)
      this.StopBehaviour();
    if (this.disableAction != GraphOwner.DisableAction.PauseBehaviour)
      return;
    this.PauseBehaviour();
  }

  public void OnDestroy()
  {
    if (GraphOwner.isQuiting)
      return;
    this.StopBehaviour();
    foreach (Graph graph in this.instances.Values)
    {
      foreach (UnityEngine.Object instancedNestedGraph in graph.GetAllInstancedNestedGraphs())
        UnityEngine.Object.Destroy(instancedNestedGraph);
      UnityEngine.Object.Destroy((UnityEngine.Object) graph);
    }
  }

  public void OnApplicationQuit() => GraphOwner.isQuiting = true;

  public enum EnableAction
  {
    EnableBehaviour,
    DoNothing,
  }

  public enum DisableAction
  {
    DisableBehaviour,
    PauseBehaviour,
    DoNothing,
  }
}
