// Decompiled with JetBrains decompiler
// Type: Structures_CookingFire
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Structures_CookingFire : StructureBrain, ITaskProvider
{
  public FollowerTask GetOverrideTask(FollowerBrain brain)
  {
    int hungerScoreIndex = FollowerManager.GetHungerScoreIndex(brain);
    Debug.Log((object) $"{brain.Info.Name} hunger score index: {hungerScoreIndex}");
    if (hungerScoreIndex != -1)
    {
      List<Structures_Meal> structuresMealList = new List<Structures_Meal>();
      foreach (Structures_Meal structuresMeal in StructureManager.GetAllStructuresOfType<Structures_Meal>(this.Data.Location))
      {
        if (!structuresMeal.ReservedForTask)
          structuresMealList.Add(structuresMeal);
      }
      Debug.Log((object) $"MealCount: {structuresMealList.Count}");
      if (structuresMealList.Count > hungerScoreIndex && brain.CurrentTaskType != FollowerTaskType.Sleep)
        return (FollowerTask) new FollowerTask_EatMeal(structuresMealList[hungerScoreIndex].Data.ID);
    }
    return (FollowerTask) null;
  }

  public bool CheckOverrideComplete()
  {
    bool flag1 = false;
    foreach (StructureBrain structureBrain in StructureManager.GetAllStructuresOfType<Structures_Meal>(this.Data.Location))
    {
      if (!structureBrain.ReservedForTask)
      {
        flag1 = true;
        break;
      }
    }
    bool flag2 = false;
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if ((double) allBrain.Stats.Satiation < 80.0)
      {
        flag2 = true;
        break;
      }
    }
    return !flag1 || !flag2;
  }

  public void GetAvailableTasks(ScheduledActivity activity, SortedList<float, FollowerTask> tasks)
  {
  }
}
