// Decompiled with JetBrains decompiler
// Type: Task_Farming
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Task_Farming : Task
{
  private Worshipper worshipper;
  private FarmStation farmStation;
  private FarmPlot TargetFarmPlot;

  public override void StartTask(TaskDoer t, GameObject TargetObject)
  {
    base.StartTask(t, TargetObject);
    this.worshipper = t.GetComponent<Worshipper>();
    this.farmStation = TargetObject.GetComponent<FarmStation>();
    this.Type = Task_Type.FARMER;
    this.worshipper.GoToAndStop(this.farmStation.WorshipperPosition, new System.Action(this.ContinueFarmStation), this.farmStation.gameObject, false);
  }

  private void ContinueFarmStation() => this.t.StartCoroutine((IEnumerator) this.GoToFarmStation());

  private IEnumerator GoToFarmStation()
  {
    Task_Farming taskFarming = this;
    while (taskFarming.worshipper.GoToAndStopping)
      yield return (object) null;
    yield return (object) new WaitForSeconds(0.5f);
    taskFarming.worshipper.simpleAnimator.SetAttachment("HAT", "Hats/HAT_Farm");
    yield return (object) new WaitForSeconds(0.5f);
    taskFarming.t.StartCoroutine((IEnumerator) taskFarming.GoToPlots());
  }

  private IEnumerator GoToPlots()
  {
    yield return (object) new WaitForSeconds(0.5f);
  }

  public override void ClearTask()
  {
    this.worshipper.simpleAnimator.SetAttachment("HAT", (string) null);
    base.ClearTask();
  }
}
