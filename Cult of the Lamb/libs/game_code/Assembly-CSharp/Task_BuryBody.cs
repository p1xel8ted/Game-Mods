// Decompiled with JetBrains decompiler
// Type: Task_BuryBody
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Task_BuryBody : Task
{
  public Worshipper Worshipper;
  public Grave Grave;
  public DeadWorshipper DeadWorshipper;

  public override void StartTask(TaskDoer t, GameObject TargetObject)
  {
    base.StartTask(t, TargetObject);
    this.Worshipper = t.GetComponent<Worshipper>();
    this.Grave = TargetObject.GetComponent<Grave>();
    this.Type = Task_Type.BURY_BUILDING;
    this.Worshipper.GoToAndStop(this.DeadWorshipper.gameObject, new System.Action(this.PickUpBody), this.DeadWorshipper.gameObject, false);
  }

  public void PickUpBody() => this.t.StartCoroutine((IEnumerator) this.PickUpBodyRoutine());

  public IEnumerator PickUpBodyRoutine()
  {
    Task_BuryBody taskBuryBody = this;
    yield return (object) new WaitForSeconds(0.5f);
    taskBuryBody.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    yield return (object) new WaitForSeconds(1f);
    taskBuryBody.DeadWorshipper.WrapBody();
    taskBuryBody.state.CURRENT_STATE = StateMachine.State.Idle;
    yield return (object) new WaitForSeconds(0.5f);
    taskBuryBody.DeadWorshipper.HideBody();
    taskBuryBody.Worshipper.simpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run-corpse");
    taskBuryBody.Worshipper.GoToAndStop(taskBuryBody.TargetObject, new System.Action(taskBuryBody.BuryBody), taskBuryBody.TargetObject, false);
    while (taskBuryBody.Worshipper.GoToAndStopping)
    {
      taskBuryBody.DeadWorshipper.transform.position = taskBuryBody.t.transform.position;
      yield return (object) null;
    }
  }

  public void BuryBody() => this.t.StartCoroutine((IEnumerator) this.BuryBodyRoutine());

  public IEnumerator BuryBodyRoutine()
  {
    Task_BuryBody taskBuryBody = this;
    taskBuryBody.DeadWorshipper.ShowBody();
    yield return (object) new WaitForSeconds(0.5f);
    taskBuryBody.Worshipper.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    taskBuryBody.Worshipper.SetAnimation("dig", true);
    yield return (object) new WaitForSeconds(5f);
    taskBuryBody.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    yield return (object) new WaitForSeconds(0.5f);
    taskBuryBody.state.CURRENT_STATE = StateMachine.State.Idle;
    yield return (object) new WaitForSeconds(0.5f);
    taskBuryBody.ClearTask();
  }

  public override void ClearTask()
  {
    this.Worshipper.simpleAnimator.ResetAnimationsToDefaults();
    int num = (UnityEngine.Object) this.DeadWorshipper != (UnityEngine.Object) null ? 1 : 0;
    base.ClearTask();
  }
}
