// Decompiled with JetBrains decompiler
// Type: NodeCanvas.BehaviourTrees.WaitUntil
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.BehaviourTrees;

[Category("Decorators")]
[ParadoxNotion.Design.Icon("Halt", false, "")]
[Description("Returns Running until the assigned condition becomes true")]
public class WaitUntil : BTDecorator, ITaskAssignable<ConditionTask>, ITaskAssignable
{
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
    if (this.accessed)
      return this.decoratedConnection.Execute(agent, blackboard);
    if (this.condition.CheckCondition(agent, blackboard))
      this.accessed = true;
    return !this.accessed ? NodeCanvas.Status.Running : this.decoratedConnection.Execute(agent, blackboard);
  }

  public override void OnReset() => this.accessed = false;
}
