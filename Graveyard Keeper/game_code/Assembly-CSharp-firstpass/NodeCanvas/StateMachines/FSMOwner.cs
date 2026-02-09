// Decompiled with JetBrains decompiler
// Type: NodeCanvas.StateMachines.FSMOwner
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using UnityEngine;

#nullable disable
namespace NodeCanvas.StateMachines;

[AddComponentMenu("NodeCanvas/FSM Owner")]
public class FSMOwner : GraphOwner<FSM>
{
  public string currentRootStateName
  {
    get
    {
      return !((Object) this.behaviour != (Object) null) ? (string) null : this.behaviour.currentStateName;
    }
  }

  public string previousRootStateName
  {
    get
    {
      return !((Object) this.behaviour != (Object) null) ? (string) null : this.behaviour.previousStateName;
    }
  }

  public string currentDeepStateName => this.GetCurrentState()?.name;

  public string previousDeepStateName => this.GetPreviousState()?.name;

  public IState GetCurrentState(bool includeSubFSMs = true)
  {
    if ((Object) this.behaviour == (Object) null)
      return (IState) null;
    FSMState currentState = this.behaviour.currentState;
    NestedFSMState nestedFsmState;
    if (includeSubFSMs)
    {
      for (; currentState is NestedFSMState; currentState = (Object) nestedFsmState.nestedFSM != (Object) null ? nestedFsmState.nestedFSM.currentState : (FSMState) null)
        nestedFsmState = (NestedFSMState) currentState;
    }
    return (IState) currentState;
  }

  public IState GetPreviousState(bool includeSubFSMs = true)
  {
    if ((Object) this.behaviour == (Object) null)
      return (IState) null;
    FSMState currentState = this.behaviour.currentState;
    FSMState previousState = this.behaviour.previousState;
    if (includeSubFSMs)
    {
      while (currentState is NestedFSMState)
      {
        NestedFSMState nestedFsmState = (NestedFSMState) currentState;
        currentState = (Object) nestedFsmState.nestedFSM != (Object) null ? nestedFsmState.nestedFSM.currentState : (FSMState) null;
        previousState = (Object) nestedFsmState.nestedFSM != (Object) null ? nestedFsmState.nestedFSM.previousState : (FSMState) null;
      }
    }
    return (IState) previousState;
  }

  public IState TriggerState(string stateName)
  {
    return (Object) this.behaviour != (Object) null ? (IState) this.behaviour.TriggerState(stateName) : (IState) null;
  }

  public string[] GetStateNames()
  {
    return (Object) this.behaviour != (Object) null ? this.behaviour.GetStateNames() : (string[]) null;
  }
}
