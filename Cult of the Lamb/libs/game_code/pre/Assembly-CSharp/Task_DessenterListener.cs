// Decompiled with JetBrains decompiler
// Type: Task_DessenterListener
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class Task_DessenterListener : Task
{
  public Worshipper w;
  private GameObject GoToObject;

  public override void StartTask(TaskDoer t, GameObject TargetObject)
  {
    base.StartTask(t, TargetObject);
    this.Type = Task_Type.DISSENTER_LISTENER;
    this.w = t.GetComponent<Worshipper>();
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    float num = (float) UnityEngine.Random.Range(2, 3);
    this.GoToObject = new GameObject();
    float f = Utils.GetAngle(TargetObject.transform.position, t.transform.position) * ((float) Math.PI / 180f);
    this.GoToObject.transform.position = TargetObject.transform.position + new Vector3(num * Mathf.Cos(f), num * Mathf.Sin(f));
    this.w.GoToAndStop(this.GoToObject, new System.Action(this.ContinueListen), TargetObject, false);
  }

  private void ContinueListen()
  {
    this.w.state.facingAngle = Utils.GetAngle(this.w.transform.position, this.TargetObject.transform.position);
    this.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    this.w.SetAnimation("Dissenters/dissenter-listening", true);
  }

  public override void ClearTask()
  {
    UnityEngine.Object.Destroy((UnityEngine.Object) this.GoToObject);
    base.ClearTask();
  }
}
