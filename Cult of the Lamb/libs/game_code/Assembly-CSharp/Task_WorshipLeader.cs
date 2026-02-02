// Decompiled with JetBrains decompiler
// Type: Task_WorshipLeader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Task_WorshipLeader : Task
{
  public bool GivenSoul;
  public Worshipper w;

  public override void StartTask(TaskDoer t, GameObject TargetObject)
  {
    base.StartTask(t, TargetObject);
    this.state.CURRENT_STATE = StateMachine.State.Preach;
    this.state.facingAngle = Utils.GetAngle(t.transform.position, TargetObject.transform.position);
    this.w = t.GetComponent<Worshipper>();
  }

  public override void TaskUpdate()
  {
    if (this.state.CURRENT_STATE != StateMachine.State.Preach)
      this.state.CURRENT_STATE = StateMachine.State.Preach;
    if (this.w.simpleAnimator.CurrentAnimation() != "bowed-down")
      this.w.SetAnimation("bowed-down", true);
    if (!this.GivenSoul)
    {
      if ((double) (this.Timer += Time.deltaTime) <= 1.0)
        return;
      this.GivenSoul = true;
      SoulCustomTarget.Create(this.TargetObject, this.t.transform.position, Color.black, (System.Action) null);
      this.Timer = 0.0f;
    }
    else
    {
      if ((double) (this.Timer += Time.deltaTime) <= 1.5)
        return;
      this.w.EndWorship();
    }
  }
}
