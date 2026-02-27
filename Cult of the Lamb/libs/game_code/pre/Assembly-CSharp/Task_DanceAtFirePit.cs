// Decompiled with JetBrains decompiler
// Type: Task_DanceAtFirePit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Task_DanceAtFirePit : Task
{
  private Worshipper Worshipper;
  private GameObject Position;
  private Coroutine cDanceRoutine;

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

  private void DoDance()
  {
    this.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    this.Worshipper.SetAnimation("dance", true);
    this.cDanceRoutine = this.Worshipper.StartCoroutine((IEnumerator) this.DanceRoutine());
  }

  private IEnumerator DanceRoutine()
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
