// Decompiled with JetBrains decompiler
// Type: Structures_Weeds
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Text;
using UnityEngine;

#nullable disable
public class Structures_Weeds : StructureBrain, ITaskProvider
{
  public System.Action OnPrioritised;
  public System.Action OnProgressChanged;
  public System.Action OnComplete;
  public int WeedType = -1;
  public const int MaxGrowth = 5;

  public bool DropWeed => false;

  public float WeedHP => this.Data.ProgressTarget - this.Data.Progress;

  public bool PickedWeeds => (double) this.Data.Progress >= (double) this.Data.ProgressTarget;

  public void PickWeeds(float Progress)
  {
    if (this.PickedWeeds)
      return;
    this.Data.Progress += Progress;
    System.Action onProgressChanged = this.OnProgressChanged;
    if (onProgressChanged != null)
      onProgressChanged();
    if (!this.PickedWeeds)
      return;
    System.Action onComplete = this.OnComplete;
    if (onComplete == null)
      return;
    onComplete();
  }

  public override void OnAdded() => TimeManager.OnNewDayStarted += new System.Action(this.OnNewDayStarted);

  public override void OnRemoved()
  {
    TimeManager.OnNewDayStarted -= new System.Action(this.OnNewDayStarted);
  }

  private void OnNewDayStarted()
  {
    this.Data.GrowthStage = Mathf.Clamp(++this.Data.GrowthStage, 0.0f, 5f);
  }

  public FollowerTask GetOverrideTask(FollowerBrain brain) => (FollowerTask) null;

  public bool CheckOverrideComplete() => true;

  public void GetAvailableTasks(ScheduledActivity activity, SortedList<float, FollowerTask> tasks)
  {
    if (!this.Data.PrioritisedAsBuildingObstruction || activity != ScheduledActivity.Work || this.ReservedForTask)
      return;
    FollowerTask_ClearWeeds followerTaskClearWeeds = new FollowerTask_ClearWeeds(this.Data.ID);
    tasks.Add(followerTaskClearWeeds.Priorty, (FollowerTask) followerTaskClearWeeds);
  }

  public override void ToDebugString(StringBuilder sb)
  {
    base.ToDebugString(sb);
    sb.AppendLine($"StartingScale: {this.Data.StartingScale}; GrowthStage: {this.Data.GrowthStage}");
    sb.AppendLine($"Prioritised: {this.Data.Prioritised}");
  }

  public void MarkMeAsPriorityWhenBuildingPlaced(bool value)
  {
    this.Data.PrioritisedAsBuildingObstruction = value;
    if (!value)
      return;
    System.Action onPrioritised = this.OnPrioritised;
    if (onPrioritised == null)
      return;
    onPrioritised();
  }
}
