// Decompiled with JetBrains decompiler
// Type: NodeCanvas.BehaviourTrees.NestedFSM
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace NodeCanvas.BehaviourTrees;

[Name("FSM", 0)]
[Category("Nested")]
[Description("NestedFSM can be assigned an entire FSM. This node will return Running for as long as the FSM is Running. If a Success or Failure State is selected, then it will return Success or Failure as soon as the Nested FSM enters that state at which point the FSM will also be stoped. If the Nested FSM ends otherwise, this node will return Success.")]
[ParadoxNotion.Design.Icon("FSM", false, "")]
public class NestedFSM : BTNode, IGraphAssignable
{
  [SerializeField]
  public BBParameter<FSM> _nestedFSM;
  public Dictionary<FSM, FSM> instances = new Dictionary<FSM, FSM>();
  public FSM currentInstance;
  public string successState;
  public string failureState;

  public override string name => base.name.ToUpper();

  public FSM nestedFSM
  {
    get => this._nestedFSM.value;
    set => this._nestedFSM.value = value;
  }

  Graph IGraphAssignable.nestedGraph
  {
    get => (Graph) this.nestedFSM;
    set => this.nestedFSM = (FSM) value;
  }

  Graph[] IGraphAssignable.GetInstances() => (Graph[]) this.instances.Values.ToArray<FSM>();

  public override NodeCanvas.Status OnExecute(Component agent, IBlackboard blackboard)
  {
    if ((UnityEngine.Object) this.nestedFSM == (UnityEngine.Object) null || this.nestedFSM.primeNode == null)
      return NodeCanvas.Status.Failure;
    if (this.status == NodeCanvas.Status.Resting)
      this.currentInstance = this.CheckInstance();
    if (this.status == NodeCanvas.Status.Resting || this.currentInstance.isPaused)
    {
      this.status = NodeCanvas.Status.Running;
      this.currentInstance.StartGraph(agent, blackboard, false, new Action<bool>(this.OnFSMFinish));
    }
    if (this.status == NodeCanvas.Status.Running)
      this.currentInstance.UpdateGraph();
    if (!string.IsNullOrEmpty(this.successState) && this.currentInstance.currentStateName == this.successState)
    {
      this.currentInstance.Stop();
      return NodeCanvas.Status.Success;
    }
    if (string.IsNullOrEmpty(this.failureState) || !(this.currentInstance.currentStateName == this.failureState))
      return this.status;
    this.currentInstance.Stop(false);
    return NodeCanvas.Status.Failure;
  }

  public void OnFSMFinish(bool success)
  {
    if (this.status != NodeCanvas.Status.Running)
      return;
    this.status = success ? NodeCanvas.Status.Success : NodeCanvas.Status.Failure;
  }

  public override void OnReset()
  {
    if (!((UnityEngine.Object) this.currentInstance != (UnityEngine.Object) null))
      return;
    this.currentInstance.Stop();
  }

  public override void OnGraphPaused()
  {
    if (!((UnityEngine.Object) this.currentInstance != (UnityEngine.Object) null))
      return;
    this.currentInstance.Pause();
  }

  public override void OnGraphStoped()
  {
    if (!((UnityEngine.Object) this.currentInstance != (UnityEngine.Object) null))
      return;
    this.currentInstance.Stop();
  }

  public FSM CheckInstance()
  {
    if ((UnityEngine.Object) this.nestedFSM == (UnityEngine.Object) this.currentInstance)
      return this.currentInstance;
    FSM fsm = (FSM) null;
    if (!this.instances.TryGetValue(this.nestedFSM, out fsm))
    {
      fsm = Graph.Clone<FSM>(this.nestedFSM);
      this.instances[this.nestedFSM] = fsm;
    }
    fsm.agent = this.graphAgent;
    fsm.blackboard = this.graphBlackboard;
    this.nestedFSM = fsm;
    return fsm;
  }
}
