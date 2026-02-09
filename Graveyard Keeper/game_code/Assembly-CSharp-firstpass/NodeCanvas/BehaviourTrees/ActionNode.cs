// Decompiled with JetBrains decompiler
// Type: NodeCanvas.BehaviourTrees.ActionNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.BehaviourTrees;

[Name("Action", 0)]
[ParadoxNotion.Design.Icon("Action", false, "")]
[Description("Executes an action and returns Success or Failure.\nReturns Running until the action is finished.")]
public class ActionNode : BTNode, ITaskAssignable<ActionTask>, ITaskAssignable
{
  [SerializeField]
  public ActionTask _action;

  public Task task
  {
    get => (Task) this.action;
    set => this.action = (ActionTask) value;
  }

  public ActionTask action
  {
    get => this._action;
    set => this._action = value;
  }

  public override string name => base.name.ToUpper();

  public override NodeCanvas.Status OnExecute(Component agent, IBlackboard blackboard)
  {
    if (this.action == null)
      return NodeCanvas.Status.Failure;
    return this.status == NodeCanvas.Status.Resting || this.status == NodeCanvas.Status.Running ? this.action.ExecuteAction(agent, blackboard) : this.status;
  }

  public override void OnReset()
  {
    if (this.action == null)
      return;
    this.action.EndAction(new bool?());
  }

  public override void OnGraphPaused()
  {
    if (this.action == null)
      return;
    this.action.PauseAction();
  }
}
