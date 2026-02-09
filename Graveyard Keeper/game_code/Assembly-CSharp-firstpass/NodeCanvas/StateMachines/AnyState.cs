// Decompiled with JetBrains decompiler
// Type: NodeCanvas.StateMachines.AnyState
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;

#nullable disable
namespace NodeCanvas.StateMachines;

[Name("Any State", 0)]
[Description("The Transitions of this node will constantly be checked. If any becomes true, the target connected State will Enter regardless of the current State. This node can have no incomming transitions.")]
[Color("b3ff7f")]
public class AnyState : FSMState, IUpdatable
{
  public bool dontRetriggerStates;

  public override string name => "FROM ANY STATE";

  public override int maxInConnections => 0;

  public override int maxOutConnections => -1;

  public override bool allowAsPrime => false;

  public new void Update()
  {
    if (this.outConnections.Count == 0)
      return;
    this.status = Status.Running;
    for (int index = 0; index < this.outConnections.Count; ++index)
    {
      FSMConnection outConnection = (FSMConnection) this.outConnections[index];
      ConditionTask condition = outConnection.condition;
      if (outConnection.isActive && condition != null && (!this.dontRetriggerStates || this.FSM.currentState != (FSMState) outConnection.targetNode || this.FSM.currentState.status != Status.Running))
      {
        if (condition.CheckCondition(this.graphAgent, this.graphBlackboard))
        {
          this.FSM.EnterState((FSMState) outConnection.targetNode);
          outConnection.status = Status.Success;
          break;
        }
        outConnection.status = Status.Failure;
      }
    }
  }
}
