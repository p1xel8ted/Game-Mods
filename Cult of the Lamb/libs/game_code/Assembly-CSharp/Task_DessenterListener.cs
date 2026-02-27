// Decompiled with JetBrains decompiler
// Type: Task_DessenterListener
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class Task_DessenterListener : Task
{
  public Worshipper w;
  public GameObject GoToObject;

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

  public void ContinueListen()
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
