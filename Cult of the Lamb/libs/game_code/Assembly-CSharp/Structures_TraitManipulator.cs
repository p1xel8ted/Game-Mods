// Decompiled with JetBrains decompiler
// Type: Structures_TraitManipulator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class Structures_TraitManipulator : StructureBrain, ITaskProvider
{
  public float Duration
  {
    get
    {
      switch (this.Data.Type)
      {
        case StructureBrain.TYPES.TRAIT_MANIPULATOR_2:
          return 480f;
        case StructureBrain.TYPES.TRAIT_MANIPULATOR_3:
          return 240f;
        default:
          return 720f;
      }
    }
  }

  public bool CheckOverrideComplete() => true;

  public void GetAvailableTasks(
    ScheduledActivity activity,
    SortedList<float, FollowerTask> sortedTasks)
  {
    if (activity != ScheduledActivity.Work || this.ReservedForTask || this.Data.FollowerID == -1 || (double) this.Data.Progress >= (double) this.Duration)
      return;
    FollowerTask_TraitManipulator traitManipulator = new FollowerTask_TraitManipulator(this.Data.ID);
    sortedTasks.Add(traitManipulator.Priorty, (FollowerTask) traitManipulator);
  }

  public FollowerTask GetOverrideTask(FollowerBrain brain) => (FollowerTask) null;

  public static void RemoveFollower(int ID)
  {
    List<Structures_TraitManipulator> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_TraitManipulator>();
    for (int index = 0; index < structuresOfType.Count; ++index)
    {
      if (structuresOfType[index].Data.FollowerID == ID)
      {
        structuresOfType[index].Data.FollowerID = -1;
        structuresOfType[index].Data.Progress = 0.0f;
      }
    }
    if (!DataManager.Instance.Followers_TraitManipulating_IDs.Contains(ID))
      return;
    DataManager.Instance.Followers_TraitManipulating_IDs.Remove(ID);
  }
}
