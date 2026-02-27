// Decompiled with JetBrains decompiler
// Type: Task_BuryBody
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Task_BuryBody : Task
{
  private Worshipper Worshipper;
  private Grave Grave;
  public DeadWorshipper DeadWorshipper;

  public override void StartTask(TaskDoer t, GameObject TargetObject)
  {
    base.StartTask(t, TargetObject);
    this.Worshipper = t.GetComponent<Worshipper>();
    this.Grave = TargetObject.GetComponent<Grave>();
    this.Type = Task_Type.BURY_BUILDING;
    this.Worshipper.GoToAndStop(this.DeadWorshipper.gameObject, new System.Action(this.PickUpBody), this.DeadWorshipper.gameObject, false);
  }

  private void PickUpBody() => this.t.StartCoroutine((IEnumerator) this.PickUpBodyRoutine());

  private IEnumerator PickUpBodyRoutine()
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

  private void BuryBody() => this.t.StartCoroutine((IEnumerator) this.BuryBodyRoutine());

  private IEnumerator BuryBodyRoutine()
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
