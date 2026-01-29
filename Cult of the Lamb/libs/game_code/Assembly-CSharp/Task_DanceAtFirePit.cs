// Decompiled with JetBrains decompiler
// Type: Task_DanceAtFirePit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Task_DanceAtFirePit : Task
{
  public Worshipper Worshipper;
  public GameObject Position;
  public Coroutine cDanceRoutine;

  public override void StartTask(TaskDoer t, GameObject TargetObject)
  {
    base.StartTask(t, TargetObject);
    this.Worshipper = t.GetComponent<Worshipper>();
    this.Type = Task_Type.DANCE_AT_FIREPIT;
  }

  public void GivePosition(GameObject Position)
  {
    this.Position = Position;
    this.Worshipper.GoToAndStop(Position, new System.Action(this.DoDance), this.TargetObject, false);
  }

  public void DoDance()
  {
    this.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    this.Worshipper.SetAnimation("dance", true);
    this.cDanceRoutine = this.Worshipper.StartCoroutine((IEnumerator) this.DanceRoutine());
  }

  public IEnumerator DanceRoutine()
  {
    while (true)
    {
      this.Worshipper.wim.v_i.IncreaseCultFaith(1f);
      yield return (object) null;
    }
  }

  public void Cheer()
  {
    Debug.Log((object) "Cheer!");
    if (this.cDanceRoutine != null)
      this.Worshipper.StopCoroutine(this.cDanceRoutine);
    this.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    this.Worshipper.SetAnimation("cheer", true);
  }

  public void EndDance()
  {
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.ClearTask();
  }

  public override void ClearTask()
  {
    if ((UnityEngine.Object) this.Position != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.Position);
    if (this.cDanceRoutine != null)
      this.Worshipper.StopCoroutine(this.cDanceRoutine);
    base.ClearTask();
  }
}
