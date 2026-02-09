// Decompiled with JetBrains decompiler
// Type: NodeCanvas.StateMachines.NestedFSMState
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace NodeCanvas.StateMachines;

[Name("FSM", 0)]
[Category("Nested")]
[Description("Execute a nested FSM OnEnter and Stop that FSM OnExit. This state is Finished when the nested FSM is finished as well")]
public class NestedFSMState : FSMState, IGraphAssignable
{
  [SerializeField]
  public BBParameter<FSM> _nestedFSM;
  public Dictionary<FSM, FSM> instances = new Dictionary<FSM, FSM>();
  public FSM currentInstance;

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

  public override void OnEnter()
  {
    if ((UnityEngine.Object) this.nestedFSM == (UnityEngine.Object) null)
    {
      this.Finish(false);
    }
    else
    {
      this.currentInstance = this.CheckInstance();
      this.currentInstance.StartGraph(this.graphAgent, this.graphBlackboard, false, new Action<bool>(((FSMState) this).Finish));
    }
  }

  public override void OnUpdate() => this.currentInstance.UpdateGraph();

  public override void OnExit()
  {
    if (!((UnityEngine.Object) this.currentInstance != (UnityEngine.Object) null) || !this.currentInstance.isRunning && !this.currentInstance.isPaused)
      return;
    this.currentInstance.Stop();
  }

  public override void OnPause()
  {
    if (!((UnityEngine.Object) this.currentInstance != (UnityEngine.Object) null))
      return;
    this.currentInstance.Pause();
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
