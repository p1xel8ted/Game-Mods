// Decompiled with JetBrains decompiler
// Type: NodeCanvas.StateMachines.SuperActionState
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.StateMachines;

[Description("The Super Action State provides finer control on when to execute actions. This state is never Finished by it's own if there is any Actions in the OnUpdate list and thus OnFinish transitions will never be called in that case. OnExit Actions are only called for 1 frame when the state exits.")]
public class SuperActionState : FSMState, ISubTasksContainer
{
  [SerializeField]
  public ActionList _onEnterList;
  [SerializeField]
  public ActionList _onUpdateList;
  [SerializeField]
  public ActionList _onExitList;
  public bool enterListFinished;

  public Task[] GetSubTasks()
  {
    return new Task[3]
    {
      (Task) this._onEnterList,
      (Task) this._onUpdateList,
      (Task) this._onExitList
    };
  }

  public override void OnValidate(Graph assignedGraph)
  {
    if (this._onEnterList == null)
    {
      this._onEnterList = (ActionList) Task.Create(typeof (ActionList), (ITaskSystem) assignedGraph);
      this._onEnterList.executionMode = ActionList.ActionsExecutionMode.ActionsRunInParallel;
    }
    if (this._onUpdateList == null)
    {
      this._onUpdateList = (ActionList) Task.Create(typeof (ActionList), (ITaskSystem) assignedGraph);
      this._onUpdateList.executionMode = ActionList.ActionsExecutionMode.ActionsRunInParallel;
    }
    if (this._onExitList != null)
      return;
    this._onExitList = (ActionList) Task.Create(typeof (ActionList), (ITaskSystem) assignedGraph);
    this._onExitList.executionMode = ActionList.ActionsExecutionMode.ActionsRunInParallel;
  }

  public override void OnEnter()
  {
    this.enterListFinished = false;
    this.OnUpdate();
  }

  public override void OnUpdate()
  {
    if (!this.enterListFinished && this._onEnterList.ExecuteAction(this.graphAgent, this.graphBlackboard) != NodeCanvas.Status.Running)
    {
      this.enterListFinished = true;
      if (this._onUpdateList.actions.Count == 0)
        this.Finish();
    }
    int num = (int) this._onUpdateList.ExecuteAction(this.graphAgent, this.graphBlackboard);
  }

  public override void OnExit()
  {
    this._onEnterList.EndAction(new bool?());
    this._onUpdateList.EndAction(new bool?());
    int num = (int) this._onExitList.ExecuteAction(this.graphAgent, this.graphBlackboard);
    this._onExitList.EndAction(new bool?());
  }

  public override void OnPause()
  {
    this._onEnterList.PauseAction();
    this._onUpdateList.PauseAction();
  }
}
