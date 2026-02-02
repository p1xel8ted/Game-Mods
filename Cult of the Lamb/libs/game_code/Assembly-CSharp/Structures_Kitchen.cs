// Decompiled with JetBrains decompiler
// Type: Structures_Kitchen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class Structures_Kitchen : Structures_CookingFire
{
  public Structures_FoodStorage FoodStorage;

  public event Structures_Kitchen.CookEvent OnMealFinishedCooking;

  public bool IsContainingFoodStorage => this.FoodStorage != null;

  public int GetAllPossibleMeals() => this.Data.QueuedMeals.Count;

  public Interaction_Kitchen.QueuedMeal GetBestPossibleMeal()
  {
    return this.Data.QueuedMeals.Count > 0 ? this.Data.QueuedMeals[0] : (Interaction_Kitchen.QueuedMeal) null;
  }

  public override void GetAvailableTasks(
    ScheduledActivity activity,
    SortedList<float, FollowerTask> tasks)
  {
    if (activity != ScheduledActivity.Work || this.ReservedForTask || this.GetAllPossibleMeals() <= 0 || this.Data.QueuedResources.Count >= (this.FoodStorage != null ? this.FoodStorage.Capacity : 10))
      return;
    FollowerTask_Cook followerTaskCook = new FollowerTask_Cook(this.Data.ID);
    tasks.Add(followerTaskCook.Priorty, (FollowerTask) followerTaskCook);
  }

  public void MealCooked()
  {
    Structures_Kitchen.CookEvent mealFinishedCooking = this.OnMealFinishedCooking;
    if (mealFinishedCooking == null)
      return;
    mealFinishedCooking();
  }

  public delegate void CookEvent();
}
