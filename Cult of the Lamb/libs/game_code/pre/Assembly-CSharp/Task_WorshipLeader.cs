// Decompiled with JetBrains decompiler
// Type: Task_WorshipLeader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Task_WorshipLeader : Task
{
  private bool GivenSoul;
  private Worshipper w;

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
