// Decompiled with JetBrains decompiler
// Type: NodeCanvas.StateMachines.FSM
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using NodeCanvas.Framework;
using ParadoxNotion;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace NodeCanvas.StateMachines;

[GraphInfo(packageName = "NodeCanvas", docsURL = "http://nodecanvas.paradoxnotion.com/documentation/", resourcesURL = "http://nodecanvas.paradoxnotion.com/downloads/", forumsURL = "http://nodecanvas.paradoxnotion.com/forums-page/")]
public class FSM : Graph
{
  public bool hasInitialized;
  public List<IUpdatable> updatableNodes;
  public List<AnyState> anyStates;
  public List<ConcurrentState> concurentStates;
  [CompilerGenerated]
  public FSMState \u003CcurrentState\u003Ek__BackingField;
  [CompilerGenerated]
  public FSMState \u003CpreviousState\u003Ek__BackingField;

  public event Action<IState> CallbackEnter;

  public event Action<IState> CallbackStay;

  public event Action<IState> CallbackExit;

  public FSMState currentState
  {
    get => this.\u003CcurrentState\u003Ek__BackingField;
    set => this.\u003CcurrentState\u003Ek__BackingField = value;
  }

  public FSMState previousState
  {
    get => this.\u003CpreviousState\u003Ek__BackingField;
    set => this.\u003CpreviousState\u003Ek__BackingField = value;
  }

  public string currentStateName
  {
    get => this.currentState == null ? (string) null : this.currentState.name;
  }

  public string previousStateName
  {
    get => this.previousState == null ? (string) null : this.previousState.name;
  }

  public override System.Type baseNodeType => typeof (FSMState);

  public override bool requiresAgent => true;

  public override bool requiresPrimeNode => true;

  public override bool autoSort => false;

  public override bool useLocalBlackboard => false;

  public override void OnGraphStarted()
  {
    if (!this.hasInitialized)
    {
      this.hasInitialized = true;
      this.GatherDelegates();
      this.updatableNodes = new List<IUpdatable>();
      this.anyStates = new List<AnyState>();
      this.concurentStates = new List<ConcurrentState>();
      for (int index = 0; index < this.allNodes.Count; ++index)
      {
        if (this.allNodes[index] is FSMState allNode)
        {
          if (allNode is IUpdatable)
            this.updatableNodes.Add((IUpdatable) allNode);
          if (allNode is AnyState)
            this.anyStates.Add((AnyState) allNode);
          if (allNode is ConcurrentState)
            this.concurentStates.Add((ConcurrentState) allNode);
        }
      }
    }
    for (int index = 0; index < this.anyStates.Count; ++index)
    {
      int num = (int) this.anyStates[index].Execute(this.agent, this.blackboard);
    }
    for (int index = 0; index < this.concurentStates.Count; ++index)
    {
      int num = (int) this.concurentStates[index].Execute(this.agent, this.blackboard);
    }
    this.EnterState(this.previousState == null ? (FSMState) this.primeNode : this.previousState);
  }

  public override void OnGraphUnpaused()
  {
    this.EnterState(this.previousState == null ? (FSMState) this.primeNode : this.previousState);
  }

  public override void OnGraphUpdate()
  {
    if (this.currentState == null)
      this.Stop(false);
    else if (this.currentState.status != NodeCanvas.Status.Running && this.currentState.outConnections.Count == 0 && this.anyStates.Count == 0)
    {
      this.Stop();
    }
    else
    {
      for (int index = 0; index < this.updatableNodes.Count; ++index)
        this.updatableNodes[index].Update();
      if (this.currentState == null)
        return;
      this.currentState.Update();
      if (this.CallbackStay == null || this.currentState.status != NodeCanvas.Status.Running)
        return;
      this.CallbackStay((IState) this.currentState);
    }
  }

  public override void OnGraphStoped()
  {
    if (this.currentState != null)
    {
      if (this.CallbackExit != null)
        this.CallbackExit((IState) this.currentState);
      this.currentState.Finish();
      this.currentState.Reset();
    }
    this.previousState = (FSMState) null;
    this.currentState = (FSMState) null;
  }

  public override void OnGraphPaused()
  {
    this.previousState = this.currentState;
    this.currentState = (FSMState) null;
  }

  public bool EnterState(FSMState newState)
  {
    if (!this.isRunning)
    {
      Debug.LogWarning((object) "Tried to EnterState on an FSM that was not running", (UnityEngine.Object) this);
      return false;
    }
    if (newState == null)
    {
      Debug.LogWarning((object) "Tried to Enter Null State");
      return false;
    }
    if (this.currentState != null)
    {
      if (this.CallbackExit != null)
        this.CallbackExit((IState) this.currentState);
      this.currentState.Finish();
      this.currentState.Reset();
    }
    this.previousState = this.currentState;
    this.currentState = newState;
    if (this.CallbackEnter != null)
      this.CallbackEnter((IState) this.currentState);
    int num = (int) this.currentState.Execute(this.agent, this.blackboard);
    return true;
  }

  public FSMState TriggerState(string stateName)
  {
    FSMState stateWithName = this.GetStateWithName(stateName);
    if (stateWithName != null)
    {
      this.EnterState(stateWithName);
      return stateWithName;
    }
    Debug.LogWarning((object) $"No State with name '{stateName}' found on FSM '{this.name}'");
    return (FSMState) null;
  }

  public string[] GetStateNames()
  {
    return this.allNodes.Where<NodeCanvas.Framework.Node>((Func<NodeCanvas.Framework.Node, bool>) (n => n.allowAsPrime)).Select<NodeCanvas.Framework.Node, string>((Func<NodeCanvas.Framework.Node, string>) (n => n.name)).ToArray<string>();
  }

  public FSMState GetStateWithName(string name)
  {
    return (FSMState) this.allNodes.Find((Predicate<NodeCanvas.Framework.Node>) (n => n.allowAsPrime && n.name == name));
  }

  public void GatherDelegates()
  {
    foreach (MonoBehaviour component in this.agent.gameObject.GetComponents<MonoBehaviour>())
    {
      MonoBehaviour mono = component;
      System.Type[] paramTypes = new System.Type[1]
      {
        typeof (IState)
      };
      MethodInfo enterMethod = mono.GetType().RTGetMethod("OnStateEnter", paramTypes);
      MethodInfo stayMethod = mono.GetType().RTGetMethod("OnStateUpdate", paramTypes);
      MethodInfo exitMethod = mono.GetType().RTGetMethod("OnStateExit", paramTypes);
      if (MethodInfo.op_Inequality(enterMethod, (MethodInfo) null))
      {
        try
        {
          this.CallbackEnter += enterMethod.RTCreateDelegate<Action<IState>>((object) mono);
        }
        catch
        {
          this.CallbackEnter += (Action<IState>) (m => enterMethod.Invoke((object) mono, new object[1]
          {
            (object) m
          }));
        }
      }
      if (MethodInfo.op_Inequality(stayMethod, (MethodInfo) null))
      {
        try
        {
          this.CallbackStay += stayMethod.RTCreateDelegate<Action<IState>>((object) mono);
        }
        catch
        {
          this.CallbackStay += (Action<IState>) (m => stayMethod.Invoke((object) mono, new object[1]
          {
            (object) m
          }));
        }
      }
      if (MethodInfo.op_Inequality(exitMethod, (MethodInfo) null))
      {
        try
        {
          this.CallbackExit += exitMethod.RTCreateDelegate<Action<IState>>((object) mono);
        }
        catch
        {
          this.CallbackExit += (Action<IState>) (m => exitMethod.Invoke((object) mono, new object[1]
          {
            (object) m
          }));
        }
      }
    }
  }
}
