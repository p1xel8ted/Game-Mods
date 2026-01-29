// Decompiled with JetBrains decompiler
// Type: Structures_JanitorStation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class Structures_JanitorStation : StructureBrain, ITaskProvider
{
  public override int SoulMax => this.Data.Type == StructureBrain.TYPES.JANITOR_STATION ? 10 : 30;

  public FollowerTask GetOverrideTask(FollowerBrain brain) => (FollowerTask) null;

  public bool CheckOverrideComplete() => true;

  public void GetAvailableTasks(ScheduledActivity activity, SortedList<float, FollowerTask> tasks)
  {
    if (activity != ScheduledActivity.Work || this.ReservedForTask || this.Data.IsSnowedUnder || this.Data.Destroyed)
      return;
    List<StructureBrain> structureBrainList = new List<StructureBrain>();
    structureBrainList.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(in this.Data.Location, StructureBrain.TYPES.POOP));
    structureBrainList.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(in this.Data.Location, StructureBrain.TYPES.POOP_GLOW));
    structureBrainList.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(in this.Data.Location, StructureBrain.TYPES.POOP_GOLD));
    structureBrainList.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(in this.Data.Location, StructureBrain.TYPES.POOP_ROTSTONE));
    structureBrainList.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(in this.Data.Location, StructureBrain.TYPES.POOP_RAINBOW));
    structureBrainList.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(in this.Data.Location, StructureBrain.TYPES.POOP_DEVOTION));
    structureBrainList.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(in this.Data.Location, StructureBrain.TYPES.VOMIT));
    structureBrainList.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(in this.Data.Location, StructureBrain.TYPES.TOXIC_WASTE));
    if (this.Data.Type == StructureBrain.TYPES.JANITOR_STATION_2)
    {
      foreach (Structures_Outhouse structuresOuthouse in new List<Structures_Outhouse>((IEnumerable<Structures_Outhouse>) StructureManager.GetAllStructuresOfType<Structures_Outhouse>()))
      {
        if (structuresOuthouse.GetPoopCount() > 0)
          structureBrainList.Add((StructureBrain) structuresOuthouse);
      }
    }
    for (int index = structureBrainList.Count - 1; index >= 0; --index)
    {
      if (structureBrainList[index].ReservedByPlayer || structureBrainList[index].ReservedForTask)
        structureBrainList.RemoveAt(index);
      else if (structureBrainList[index] is Structures_IceSculpture structuresIceSculpture && !structuresIceSculpture.IsFinished)
        structureBrainList.RemoveAt(index);
    }
    if (this.ReservedForTask || structureBrainList.Count <= 0)
      return;
    FollowerTask_Janitor followerTaskJanitor = new FollowerTask_Janitor(this.Data.ID);
    tasks.Add(followerTaskJanitor.Priorty, (FollowerTask) followerTaskJanitor);
  }
}
