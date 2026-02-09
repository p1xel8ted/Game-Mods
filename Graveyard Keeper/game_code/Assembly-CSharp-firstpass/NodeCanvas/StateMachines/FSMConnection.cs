// Decompiled with JetBrains decompiler
// Type: NodeCanvas.StateMachines.FSMConnection
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using UnityEngine;

#nullable disable
namespace NodeCanvas.StateMachines;

public class FSMConnection : Connection, ITaskAssignable<ConditionTask>, ITaskAssignable
{
  [SerializeField]
  public ConditionTask _condition;

  public ConditionTask condition
  {
    get => this._condition;
    set => this._condition = value;
  }

  public Task task
  {
    get => (Task) this.condition;
    set => this.condition = (ConditionTask) value;
  }

  public void PerformTransition() => (this.graph as FSM).EnterState((FSMState) this.targetNode);
}
