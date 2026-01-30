// Decompiled with JetBrains decompiler
// Type: Task_Ill
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine;
using UnityEngine;

#nullable disable
public class Task_Ill : Task
{
  public Worshipper w;
  public bool Chundered;

  public override void StartTask(TaskDoer t, GameObject TargetObject)
  {
    base.StartTask(t, TargetObject);
    this.Type = Task_Type.ILL;
    this.w = t.GetComponent<Worshipper>();
    this.w.maxSpeed = 0.02f;
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.w.simpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Sick/idle-sick");
    this.w.simpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Sick/walk-sick");
    this.w.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
  }

  public void HandleEvent(TrackEntry trackEntry, Spine.Event e)
  {
  }

  public override void TaskUpdate()
  {
    if (this.state.CURRENT_STATE != StateMachine.State.Idle)
      return;
    if (!this.Chundered && (double) UnityEngine.Random.Range(0.0f, 1f) < 1.0 / 1000.0)
    {
      this.w.TimedAnimation("Sick/chunder", 3.5f, new System.Action(this.w.BackToIdle));
      this.Chundered = true;
    }
    else
    {
      if ((double) (this.Timer -= Time.deltaTime) >= 0.0)
        return;
      this.Timer = UnityEngine.Random.Range(4f, 6f);
      this.Chundered = false;
      this.t.givePath(TownCentre.Instance.RandomPositionInTownCentre());
    }
  }

  public override void ClearTask()
  {
    this.w.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
    this.w.maxSpeed = 0.04f;
    this.t.ClearPaths();
    base.ClearTask();
    this.w.simpleAnimator.ResetAnimationsToDefaults();
  }
}
