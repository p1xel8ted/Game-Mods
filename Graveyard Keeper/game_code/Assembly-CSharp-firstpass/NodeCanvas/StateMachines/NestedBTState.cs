// Decompiled with JetBrains decompiler
// Type: NodeCanvas.StateMachines.NestedBTState
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace NodeCanvas.StateMachines;

[Description("Execute a Behaviour Tree OnEnter. OnExit that Behavior Tree will be stoped or paused based on the relevant specified setting. You can optionaly specify a Success Event and a Failure Event which will be sent when the BT's root node status returns either of the two. If so, use alongside with a CheckEvent on a transition.")]
[Category("Nested")]
[Name("BehaviourTree", 0)]
public class NestedBTState : FSMState, IGraphAssignable
{
  [SerializeField]
  public BBParameter<BehaviourTree> _nestedBT;
  public NestedBTState.BTExecutionMode executionMode = NestedBTState.BTExecutionMode.Repeat;
  public NestedBTState.BTExitMode exitMode;
  public string successEvent;
  public string failureEvent;
  public Dictionary<BehaviourTree, BehaviourTree> instances = new Dictionary<BehaviourTree, BehaviourTree>();
  public BehaviourTree currentInstance;

  public BehaviourTree nestedBT
  {
    get => this._nestedBT.value;
    set => this._nestedBT.value = value;
  }

  Graph IGraphAssignable.nestedGraph
  {
    get => (Graph) this.nestedBT;
    set => this.nestedBT = (BehaviourTree) value;
  }

  Graph[] IGraphAssignable.GetInstances()
  {
    return (Graph[]) this.instances.Values.ToArray<BehaviourTree>();
  }

  public override void OnEnter()
  {
    if ((UnityEngine.Object) this.nestedBT == (UnityEngine.Object) null)
    {
      this.Finish(false);
    }
    else
    {
      this.currentInstance = this.CheckInstance();
      this.currentInstance.repeat = this.executionMode == NestedBTState.BTExecutionMode.Repeat;
      this.currentInstance.updateInterval = 0.0f;
      this.currentInstance.StartGraph(this.graphAgent, this.graphBlackboard, false, new Action<bool>(this.OnFinish));
    }
  }

  public override void OnUpdate()
  {
    this.currentInstance.UpdateGraph();
    if (!string.IsNullOrEmpty(this.successEvent) && this.currentInstance.rootStatus == NodeCanvas.Status.Success)
      this.currentInstance.Stop();
    if (string.IsNullOrEmpty(this.failureEvent) || this.currentInstance.rootStatus != NodeCanvas.Status.Failure)
      return;
    this.currentInstance.Stop(false);
  }

  public void OnFinish(bool success)
  {
    if (this.status != NodeCanvas.Status.Running)
      return;
    if (!string.IsNullOrEmpty(this.successEvent) & success)
      this.SendEvent(new EventData(this.successEvent));
    if (!string.IsNullOrEmpty(this.failureEvent) && !success)
      this.SendEvent(new EventData(this.failureEvent));
    this.Finish(success);
  }

  public override void OnExit()
  {
    if (!((UnityEngine.Object) this.currentInstance != (UnityEngine.Object) null) || !this.currentInstance.isRunning)
      return;
    if (this.exitMode == NestedBTState.BTExitMode.StopAndRestart)
      this.currentInstance.Stop();
    else
      this.currentInstance.Pause();
  }

  public override void OnPause()
  {
    if (!((UnityEngine.Object) this.currentInstance != (UnityEngine.Object) null) || !this.currentInstance.isRunning)
      return;
    this.currentInstance.Pause();
  }

  public BehaviourTree CheckInstance()
  {
    if ((UnityEngine.Object) this.nestedBT == (UnityEngine.Object) this.currentInstance)
      return this.currentInstance;
    BehaviourTree behaviourTree = (BehaviourTree) null;
    if (!this.instances.TryGetValue(this.nestedBT, out behaviourTree))
    {
      behaviourTree = Graph.Clone<BehaviourTree>(this.nestedBT);
      this.instances[this.nestedBT] = behaviourTree;
    }
    behaviourTree.agent = this.graphAgent;
    behaviourTree.blackboard = this.graphBlackboard;
    this.nestedBT = behaviourTree;
    return behaviourTree;
  }

  public enum BTExecutionMode
  {
    Once,
    Repeat,
  }

  public enum BTExitMode
  {
    StopAndRestart,
    PauseAndResume,
  }
}
