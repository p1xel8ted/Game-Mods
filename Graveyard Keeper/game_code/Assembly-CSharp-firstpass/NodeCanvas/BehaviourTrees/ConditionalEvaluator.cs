// Decompiled with JetBrains decompiler
// Type: NodeCanvas.BehaviourTrees.ConditionalEvaluator
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.BehaviourTrees;

[Name("Conditional", 0)]
[Category("Decorators")]
[Description("Execute and return the child node status if the condition is true, otherwise return Failure. The condition is evaluated only once in the first Tick and when the node is not already Running unless it is set as 'Dynamic' in which case it will revaluate even while running")]
[ParadoxNotion.Design.Icon("Accessor", false, "")]
public class ConditionalEvaluator : BTDecorator, ITaskAssignable<ConditionTask>, ITaskAssignable
{
  public bool isDynamic;
  [SerializeField]
  public ConditionTask _condition;
  public bool accessed;

  public Task task
  {
    get => (Task) this.condition;
    set => this.condition = (ConditionTask) value;
  }

  public ConditionTask condition
  {
    get => this._condition;
    set => this._condition = value;
  }

  public override NodeCanvas.Status OnExecute(Component agent, IBlackboard blackboard)
  {
    if (this.decoratedConnection == null)
      return NodeCanvas.Status.Resting;
    if (this.condition == null)
      return this.decoratedConnection.Execute(agent, blackboard);
    if (this.isDynamic)
    {
      if (this.condition.CheckCondition(agent, blackboard))
        return this.decoratedConnection.Execute(agent, blackboard);
      this.decoratedConnection.Reset();
      return NodeCanvas.Status.Failure;
    }
    if (this.status != NodeCanvas.Status.Running && this.condition.CheckCondition(agent, blackboard))
      this.accessed = true;
    return !this.accessed ? NodeCanvas.Status.Failure : this.decoratedConnection.Execute(agent, blackboard);
  }

  public override void OnReset() => this.accessed = false;
}
