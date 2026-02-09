// Decompiled with JetBrains decompiler
// Type: NodeCanvas.DialogueTrees.ConditionNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.DialogueTrees;

[Name("Task Condition", 0)]
[Description("Execute the first child node if a Condition is true, or the second one if that Condition is false. The Actor selected is used for the Condition check")]
[Category("Branch")]
[ParadoxNotion.Design.Icon("Condition", false, "")]
[Color("b3ff7f")]
public class ConditionNode : DTNode, ITaskAssignable<ConditionTask>, ITaskAssignable
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

  public override int maxOutConnections => 2;

  public override bool requireActorSelection => true;

  public override NodeCanvas.Status OnExecute(Component agent, IBlackboard bb)
  {
    if (this.outConnections.Count == 0)
      return this.Error("There are no connections on the Dialogue Condition Node");
    if (this.condition == null)
      return this.Error("There is no Conidition on the Dialoge Condition Node");
    bool flag = this.condition.CheckCondition((Component) this.finalActor.transform, this.graphBlackboard);
    this.status = flag ? NodeCanvas.Status.Success : NodeCanvas.Status.Failure;
    this.DLGTree.Continue(flag ? 0 : 1);
    return this.status;
  }
}
