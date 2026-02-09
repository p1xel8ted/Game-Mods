// Decompiled with JetBrains decompiler
// Type: NodeCanvas.StateMachines.ConcurrentState
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.StateMachines;

[Name("Concurrent", 0)]
[Color("ff64cb")]
[Description("Execute a number of Actions with optional conditional requirement and in parallel to any other state, as soon as the FSM is started.\nAll actions will prematurely be stoped as soon as the FSM stops as well.\nThis is not a state per-se and thus can have neither incomming, nor outgoing transitions.")]
public class ConcurrentState : FSMState, IUpdatable, ISubTasksContainer
{
  [SerializeField]
  public ConditionList _conditionList;
  [SerializeField]
  public ActionList _actionList;
  [SerializeField]
  public bool _repeatStateActions;
  public bool accessed;

  public ConditionList conditionList
  {
    get => this._conditionList;
    set => this._conditionList = value;
  }

  public ActionList actionList
  {
    get => this._actionList;
    set => this._actionList = value;
  }

  public Task[] GetSubTasks()
  {
    return new Task[2]
    {
      (Task) this._conditionList,
      (Task) this._actionList
    };
  }

  public bool repeatStateActions
  {
    get => this._repeatStateActions;
    set => this._repeatStateActions = value;
  }

  public override string name => base.name.ToUpper();

  public override int maxInConnections => 0;

  public override int maxOutConnections => 0;

  public override bool allowAsPrime => false;

  public override void OnValidate(Graph assignedGraph)
  {
    if (this.conditionList == null)
    {
      this.conditionList = (ConditionList) Task.Create(typeof (ConditionList), (ITaskSystem) assignedGraph);
      this.conditionList.checkMode = ConditionList.ConditionsCheckMode.AllTrueRequired;
    }
    if (this.actionList != null)
      return;
    this.actionList = (ActionList) Task.Create(typeof (ActionList), (ITaskSystem) assignedGraph);
    this.actionList.executionMode = ActionList.ActionsExecutionMode.ActionsRunInParallel;
  }

  public override void OnEnter()
  {
    this.accessed = false;
    this.Update();
  }

  public new void Update()
  {
    if (this.status != NodeCanvas.Status.Resting && this.status != NodeCanvas.Status.Running)
      return;
    if (this.conditionList.CheckCondition(this.graphAgent, this.graphBlackboard))
      this.accessed = true;
    if (!this.accessed || this.actionList.ExecuteAction(this.graphAgent, this.graphBlackboard) == NodeCanvas.Status.Running)
      return;
    this.accessed = false;
    if (this.repeatStateActions)
      return;
    this.Finish();
  }

  public override void OnExit() => this.actionList.EndAction(new bool?());

  public override void OnPause() => this.actionList.PauseAction();
}
