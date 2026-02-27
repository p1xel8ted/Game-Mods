// Decompiled with JetBrains decompiler
// Type: Structures_Waste
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class Structures_Waste : StructureBrain, ITaskProvider
{
  public FollowerTask GetOverrideTask(FollowerBrain brain) => (FollowerTask) null;

  public bool CheckOverrideComplete() => true;

  public void GetAvailableTasks(ScheduledActivity activity, SortedList<float, FollowerTask> tasks)
  {
    if (activity != ScheduledActivity.Work || this.ReservedForTask)
      return;
    FollowerTask_CleanWaste followerTaskCleanWaste = new FollowerTask_CleanWaste(this.Data.ID);
    tasks.Add(followerTaskCleanWaste.Priorty, (FollowerTask) followerTaskCleanWaste);
  }
}
