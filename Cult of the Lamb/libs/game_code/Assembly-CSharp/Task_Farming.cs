// Decompiled with JetBrains decompiler
// Type: Task_Farming
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Task_Farming : Task
{
  public Worshipper worshipper;
  public FarmStation farmStation;
  public FarmPlot TargetFarmPlot;

  public override void StartTask(TaskDoer t, GameObject TargetObject)
  {
    base.StartTask(t, TargetObject);
    this.worshipper = t.GetComponent<Worshipper>();
    this.farmStation = TargetObject.GetComponent<FarmStation>();
    this.Type = Task_Type.FARMER;
    this.worshipper.GoToAndStop(this.farmStation.WorshipperPosition, new System.Action(this.ContinueFarmStation), this.farmStation.gameObject, false);
  }

  public void ContinueFarmStation() => this.t.StartCoroutine((IEnumerator) this.GoToFarmStation());

  public IEnumerator GoToFarmStation()
  {
    Task_Farming taskFarming = this;
    while (taskFarming.worshipper.GoToAndStopping)
      yield return (object) null;
    yield return (object) new WaitForSeconds(0.5f);
    taskFarming.worshipper.simpleAnimator.SetAttachment("HAT", "Hats/HAT_Farm");
    yield return (object) new WaitForSeconds(0.5f);
    taskFarming.t.StartCoroutine((IEnumerator) taskFarming.GoToPlots());
  }

  public IEnumerator GoToPlots()
  {
    yield return (object) new WaitForSeconds(0.5f);
  }

  public override void ClearTask()
  {
    this.worshipper.simpleAnimator.SetAttachment("HAT", (string) null);
    base.ClearTask();
  }
}
