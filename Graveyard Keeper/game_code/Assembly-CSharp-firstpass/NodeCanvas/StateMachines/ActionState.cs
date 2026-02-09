// Decompiled with JetBrains decompiler
// Type: NodeCanvas.StateMachines.ActionState
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.StateMachines;

[Description("Execute a number of Action Tasks OnEnter. All actions will be stoped OnExit. This state is Finished when all Actions are finished as well")]
[Name("Action State", 0)]
public class ActionState : FSMState, ITaskAssignable
{
  [SerializeField]
  public ActionList _actionList;
  [SerializeField]
  public bool _repeatStateActions;

  public Task task
  {
    get => (Task) this.actionList;
    set => this.actionList = (ActionList) value;
  }

  public ActionList actionList
  {
    get => this._actionList;
    set => this._actionList = value;
  }

  public bool repeatStateActions
  {
    get => this._repeatStateActions;
    set => this._repeatStateActions = value;
  }

  public override void OnValidate(Graph assignedGraph)
  {
    if (this.actionList != null)
      return;
    this.actionList = (ActionList) Task.Create(typeof (ActionList), (ITaskSystem) assignedGraph);
    this.actionList.executionMode = ActionList.ActionsExecutionMode.ActionsRunInParallel;
  }

  public override void OnEnter() => this.OnUpdate();

  public override void OnUpdate()
  {
    NodeCanvas.Status status = this.actionList.ExecuteAction(this.graphAgent, this.graphBlackboard);
    if (this.repeatStateActions)
      return;
    int num;
    switch (status)
    {
      case NodeCanvas.Status.Success:
        num = 1;
        break;
      case NodeCanvas.Status.Running:
        return;
      default:
        num = 0;
        break;
    }
    this.Finish(num != 0);
  }

  public override void OnExit() => this.actionList.EndAction(new bool?());

  public override void OnPause() => this.actionList.PauseAction();
}
