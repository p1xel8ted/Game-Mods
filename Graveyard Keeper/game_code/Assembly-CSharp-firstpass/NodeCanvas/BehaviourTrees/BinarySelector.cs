// Decompiled with JetBrains decompiler
// Type: NodeCanvas.BehaviourTrees.BinarySelector
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.BehaviourTrees;

[Color("b3ff7f")]
[ParadoxNotion.Design.Icon("Condition", false, "")]
[Description("Quick way to execute the left, or the right child node based on a Condition Task evaluation.")]
[Category("Composites")]
public class BinarySelector : BTNode, ITaskAssignable<ConditionTask>, ITaskAssignable
{
  public bool dynamic;
  [SerializeField]
  public ConditionTask _condition;
  public int succeedIndex;

  public override int maxOutConnections => 2;

  public override Alignment2x2 commentsAlignment => Alignment2x2.Right;

  public override string name => base.name.ToUpper();

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
    if (this.condition == null || this.outConnections.Count < 2)
      return NodeCanvas.Status.Failure;
    if (this.dynamic || this.status == NodeCanvas.Status.Resting)
    {
      int succeedIndex = this.succeedIndex;
      this.succeedIndex = this.condition.CheckCondition(agent, blackboard) ? 0 : 1;
      if (this.succeedIndex != succeedIndex)
        this.outConnections[succeedIndex].Reset();
    }
    return this.outConnections[this.succeedIndex].Execute(agent, blackboard);
  }
}
