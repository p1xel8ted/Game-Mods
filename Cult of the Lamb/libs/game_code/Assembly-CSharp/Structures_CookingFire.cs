// Decompiled with JetBrains decompiler
// Type: Structures_CookingFire
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Structures_CookingFire : StructureBrain, ITaskProvider
{
  public List<Structures_Meal> cachedMeals = new List<Structures_Meal>();

  public FollowerTask GetOverrideTask(FollowerBrain brain)
  {
    int hungerScoreIndex = FollowerManager.GetHungerScoreIndex(brain);
    Debug.Log((object) $"{brain.Info.Name} hunger score index: {hungerScoreIndex}");
    if (hungerScoreIndex != -1)
    {
      StructureManager.TryGetAllStructuresOfType<Structures_Meal>(ref this.cachedMeals, this.Data.Location, (Func<Structures_Meal, bool>) (m => !m.ReservedForTask));
      Debug.Log((object) $"MealCount: {this.cachedMeals.Count}");
      if (this.cachedMeals.Count > hungerScoreIndex && brain.CurrentTaskType != FollowerTaskType.Sleep)
      {
        foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
        {
          if (allBrain.CurrentOverrideStructureType == this.Data.Type)
          {
            this.cachedMeals.Clear();
            return (FollowerTask) null;
          }
        }
        Structures_Meal cachedMeal = this.cachedMeals[hungerScoreIndex];
        this.cachedMeals.Clear();
        return (FollowerTask) new FollowerTask_EatMeal(cachedMeal.Data.ID);
      }
      this.cachedMeals.Clear();
    }
    return (FollowerTask) null;
  }

  public bool CheckOverrideComplete()
  {
    bool firstStructureOfType = StructureManager.TryGetFirstStructureOfType<Structures_Meal>(out Structures_Meal _, in this.Data.Location, (Func<Structures_Meal, bool>) (m => !m.ReservedForTask));
    bool flag = false;
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if ((double) allBrain.Stats.Satiation < 80.0)
      {
        flag = true;
        break;
      }
    }
    return !firstStructureOfType || !flag;
  }

  public virtual void GetAvailableTasks(
    ScheduledActivity activity,
    SortedList<float, FollowerTask> tasks)
  {
  }
}
