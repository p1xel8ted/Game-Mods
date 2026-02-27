// Decompiled with JetBrains decompiler
// Type: Structures_JanitorStation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class Structures_JanitorStation : StructureBrain, ITaskProvider
{
  public FollowerTask GetOverrideTask(FollowerBrain brain) => (FollowerTask) null;

  public bool CheckOverrideComplete() => true;

  public void GetAvailableTasks(ScheduledActivity activity, SortedList<float, FollowerTask> tasks)
  {
    if (activity != ScheduledActivity.Work || this.ReservedForTask)
      return;
    List<StructureBrain> structureBrainList = new List<StructureBrain>();
    structureBrainList.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(this.Data.Location, StructureBrain.TYPES.POOP));
    structureBrainList.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(this.Data.Location, StructureBrain.TYPES.VOMIT));
    for (int index = structureBrainList.Count - 1; index >= 0; --index)
    {
      if (structureBrainList[index].ReservedByPlayer || structureBrainList[index].ReservedForTask)
        structureBrainList.RemoveAt(index);
    }
    if (this.ReservedForTask || structureBrainList.Count <= 0)
      return;
    FollowerTask_Janitor followerTaskJanitor = new FollowerTask_Janitor(this.Data.ID);
    tasks.Add(followerTaskJanitor.Priorty, (FollowerTask) followerTaskJanitor);
  }
}
