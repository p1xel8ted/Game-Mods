// Decompiled with JetBrains decompiler
// Type: NodeCanvas.BehaviourTrees.ConditionNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.BehaviourTrees;

[Name("Condition", 0)]
[Description("Check a condition and return Success or Failure")]
[ParadoxNotion.Design.Icon("Condition", false, "")]
public class ConditionNode : BTNode, ITaskAssignable<ConditionTask>, ITaskAssignable
{
  [SerializeField]
  public ConditionTask _condition;

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

  public override string name => base.name.ToUpper();

  public override NodeCanvas.Status OnExecute(Component agent, IBlackboard blackboard)
  {
    return this.condition != null && this.condition.CheckCondition(agent, blackboard) ? NodeCanvas.Status.Success : NodeCanvas.Status.Failure;
  }
}
