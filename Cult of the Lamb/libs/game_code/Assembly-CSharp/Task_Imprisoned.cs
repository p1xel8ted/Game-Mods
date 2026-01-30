// Decompiled with JetBrains decompiler
// Type: Task_Imprisoned
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Task_Imprisoned : Task
{
  public Worshipper w;
  public Prison Prison;

  public Task_Imprisoned() => this.Type = Task_Type.IMPRISONED;

  public override void StartTask(TaskDoer t, GameObject TargetObject)
  {
    base.StartTask(t, TargetObject);
    this.w = t.GetComponent<Worshipper>();
    this.Prison = TargetObject.GetComponent<Prison>();
    this.w.simpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "picked-up-hate");
    this.w.transform.position = this.Prison.PrisonerLocation.position;
    this.w.StartCoroutine((IEnumerator) this.ArriveAtPrisonRoutine());
  }

  public IEnumerator ArriveAtPrisonRoutine()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Task_Imprisoned taskImprisoned = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      taskImprisoned.state.CURRENT_STATE = StateMachine.State.Idle;
      taskImprisoned.w.wim.v_i.Starve = 0.0f;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    taskImprisoned.state.CURRENT_STATE = StateMachine.State.Moving;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) null;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public override void ClearTask()
  {
    base.ClearTask();
    this.w.simpleAnimator.ResetAnimationsToDefaults();
  }
}
