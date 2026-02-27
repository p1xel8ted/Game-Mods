// Decompiled with JetBrains decompiler
// Type: Task_WonderFreely
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class Task_WonderFreely : Task
{
  private float TargetAngle;
  private GameObject Player;
  private float BowingDelay;
  private Worshipper w;

  public override void StartTask(TaskDoer t, GameObject TargetObject)
  {
    base.StartTask(t, TargetObject);
    t.UsePathing = false;
    t.OnCollision += new System.Action(this.OnCollision);
    SimpleInventory component = t.GetComponent<SimpleInventory>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      component.DropItem();
    this.Timer = (float) UnityEngine.Random.Range(1, 3);
    this.Type = Task_Type.NONE;
    this.Player = GameObject.FindWithTag("Player");
    this.w = t.GetComponent<Worshipper>();
  }

  public override void TaskUpdate()
  {
    if ((UnityEngine.Object) this.Player != (UnityEngine.Object) null && (UnityEngine.Object) this.w != (UnityEngine.Object) null && (double) (this.BowingDelay += Time.deltaTime) > 2.0 && this.state.CURRENT_STATE != StateMachine.State.TimedAction && (double) Vector3.Distance(this.t.transform.position, this.Player.transform.position) < 2.0)
    {
      this.w.TimedAnimation("bowed-down", 3f, new System.Action(this.w.BackToIdle));
      this.t.state.facingAngle = Utils.GetAngle(this.t.transform.position, this.Player.transform.position);
      this.BowingDelay = 0.0f;
    }
    base.TaskUpdate();
    switch (this.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
        if ((double) (this.Timer -= Time.deltaTime) >= 0.0)
          break;
        this.Timer = UnityEngine.Random.Range(0.5f, 2f);
        this.TargetAngle = (float) UnityEngine.Random.Range(0, 360);
        this.t.UsePathing = false;
        this.state.CURRENT_STATE = StateMachine.State.Moving;
        break;
      case StateMachine.State.Moving:
        this.state.facingAngle += (float) ((double) Mathf.Atan2(Mathf.Sin((float) (((double) this.TargetAngle - (double) this.state.facingAngle) * (Math.PI / 180.0))), Mathf.Cos((float) (((double) this.TargetAngle - (double) this.state.facingAngle) * (Math.PI / 180.0)))) * 57.295780181884766 / (10.0 * (double) GameManager.DeltaTime));
        if ((double) (this.Timer -= Time.deltaTime) >= 0.0)
          break;
        this.Timer = (float) UnityEngine.Random.Range(3, 7);
        this.state.CURRENT_STATE = StateMachine.State.Idle;
        break;
    }
  }

  private void OnCollision() => this.TargetAngle = this.state.facingAngle + 90f;

  public override void ClearTask()
  {
    this.t.OnCollision -= new System.Action(this.OnCollision);
    this.t.UsePathing = true;
    base.ClearTask();
  }
}
