// Decompiled with JetBrains decompiler
// Type: NodeCanvas.BehaviourTrees.Interruptor
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.BehaviourTrees;

[Name("Interrupt", 0)]
[Category("Decorators")]
[Description("Interrupt the child node and return Failure if the condition is or becomes true while running. Otherwise execute and return the child Status")]
[ParadoxNotion.Design.Icon("Interruptor", false, "")]
public class Interruptor : BTDecorator, ITaskAssignable<ConditionTask>, ITaskAssignable
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

  public override NodeCanvas.Status OnExecute(Component agent, IBlackboard blackboard)
  {
    if (this.decoratedConnection == null)
      return NodeCanvas.Status.Resting;
    if (this.condition == null || !this.condition.CheckCondition(agent, blackboard))
      return this.decoratedConnection.Execute(agent, blackboard);
    if (this.decoratedConnection.status == NodeCanvas.Status.Running)
      this.decoratedConnection.Reset();
    return NodeCanvas.Status.Failure;
  }
}
