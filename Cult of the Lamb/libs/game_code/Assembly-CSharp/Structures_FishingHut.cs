// Decompiled with JetBrains decompiler
// Type: Structures_FishingHut
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class Structures_FishingHut : StructureBrain, ITaskProvider
{
  public const int MaxFish = 5;

  public FollowerTask GetOverrideTask(FollowerBrain brain) => (FollowerTask) null;

  public bool CheckOverrideComplete() => true;

  public void GetAvailableTasks(ScheduledActivity activity, SortedList<float, FollowerTask> tasks)
  {
    if (activity != ScheduledActivity.Work || this.ReservedForTask)
      return;
    FollowerTask_Fisherman followerTaskFisherman = new FollowerTask_Fisherman(this.Data.ID);
    tasks.Add(followerTaskFisherman.Priorty, (FollowerTask) followerTaskFisherman);
  }
}
