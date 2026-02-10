// Decompiled with JetBrains decompiler
// Type: Task_Follow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class Task_Follow : Task
{
  public float MaxRange = 2f;
  public float FollowDistance = 1f;
  public float posAngle;
  public float RepathTimer;

  public Task_Follow() => this.Type = Task_Type.FOLLOW;

  public override void StartTask(TaskDoer t, GameObject TargetObject)
  {
    base.StartTask(t, TargetObject);
    this.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public override void TaskUpdate()
  {
    if ((UnityEngine.Object) this.TargetObject != (UnityEngine.Object) null)
    {
      this.posAngle = (float) (90 * this.t.Position);
      this.TargetV3 = AstarPath.active.GetNearest(this.TargetObject.transform.position + new Vector3(1f * Mathf.Cos(this.posAngle * ((float) Math.PI / 180f)), 1f * Mathf.Sin(this.posAngle * ((float) Math.PI / 180f)), 0.0f)).position;
    }
    switch (this.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
        if (!((UnityEngine.Object) this.TargetObject != (UnityEngine.Object) null) || this.state.CURRENT_STATE != StateMachine.State.Idle || (double) Vector3.Distance(this.t.transform.position, this.TargetV3) <= (double) this.MaxRange)
          break;
        this.Timer = 0.0f;
        this.PathToPosition(this.TargetV3);
        break;
      case StateMachine.State.Moving:
        if ((UnityEngine.Object) this.TargetObject == (UnityEngine.Object) null)
        {
          this.state.CURRENT_STATE = StateMachine.State.Idle;
          break;
        }
        if ((double) Vector3.Distance(this.t.transform.position, this.TargetV3) < 0.5)
        {
          this.t.ClearPaths();
          this.state.CURRENT_STATE = StateMachine.State.Idle;
          break;
        }
        if ((double) (this.RepathTimer += Time.deltaTime) <= 1.0)
          break;
        this.t.ClearPaths();
        if (!this.t.IsPathPossible(this.t.transform.position, this.TargetV3) && this.t.OnGround(this.TargetV3 + Vector3.back * 10f))
        {
          this.t.transform.position = this.TargetV3;
          this.RepathTimer = 0.0f;
        }
        else
          this.RepathTimer = 0.0f;
        this.PathToPosition(this.TargetV3);
        break;
    }
  }
}
