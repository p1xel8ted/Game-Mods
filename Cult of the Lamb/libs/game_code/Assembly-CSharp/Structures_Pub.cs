// Decompiled with JetBrains decompiler
// Type: Structures_Pub
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class Structures_Pub : StructureBrain, ITaskProvider
{
  public Structures_FoodStorage FoodStorage;
  public static bool IsDrinking;

  public int MaxQueue => this.Data.Type == StructureBrain.TYPES.PUB_2 ? 6 : 3;

  public event Structures_Pub.BrewEvent OnDrinkFinishedBrewing;

  public bool IsContainingFoodStorage => this.FoodStorage != null;

  public Interaction_Kitchen.QueuedMeal GetBestPossibleDrink()
  {
    return this.Data.QueuedMeals.Count > 0 ? this.Data.QueuedMeals[0] : (Interaction_Kitchen.QueuedMeal) null;
  }

  public int GetAllPossibleDrinks() => this.Data.QueuedMeals.Count;

  public void DrinkBrewed()
  {
    Structures_Pub.BrewEvent drinkFinishedBrewing = this.OnDrinkFinishedBrewing;
    if (drinkFinishedBrewing == null)
      return;
    drinkFinishedBrewing();
  }

  public void GetAvailableTasks(ScheduledActivity activity, SortedList<float, FollowerTask> tasks)
  {
    if (activity == ScheduledActivity.Work && !this.ReservedForTask && this.GetAllPossibleDrinks() > 0 && this.Data.QueuedResources.Count < this.MaxQueue && !Structures_Pub.IsDrinking)
    {
      FollowerTask_Brew followerTaskBrew = new FollowerTask_Brew(this.Data.ID);
      tasks.Add(followerTaskBrew.Priorty, (FollowerTask) followerTaskBrew);
    }
    if (!Structures_Pub.IsDrinking || this.FoodStorage == null || this.FoodStorage.Data == null)
      return;
    for (int index = 0; index < this.FoodStorage.Data.Inventory.Count; ++index)
    {
      if (this.FoodStorage.Data.Inventory[index] != null && this.FoodStorage.Data.Inventory[index].QuantityReserved <= 0 && !this.IsDrinkReserved(index))
      {
        FollowerTask_Drink followerTaskDrink = new FollowerTask_Drink(index, this.FoodStorage.Data.Inventory[index], this);
        tasks.Add(followerTaskDrink.Priorty, (FollowerTask) followerTaskDrink);
      }
    }
  }

  public FollowerTask GetOverrideTask(FollowerBrain brain) => (FollowerTask) null;

  public bool CheckOverrideComplete() => true;

  public void BeginDrinking()
  {
    if (this.Data.QueuedResources.Count <= 0)
      return;
    Structures_Pub.IsDrinking = true;
  }

  public void StopDrinking() => Structures_Pub.IsDrinking = false;

  public void FinishedDrink(int seat, InventoryItem drink)
  {
    this.Data.ReservedFollowers.Remove(seat);
    this.FoodStorage.Data.Inventory[seat] = (InventoryItem) null;
    this.Data.QueuedResources.Remove((InventoryItem.ITEM_TYPE) drink.type);
    if (this.GetAmountOfPreparedDrinks() > 0)
      return;
    this.StopDrinking();
  }

  public void SetDrinkReserved(int seat, int followerID)
  {
    this.Data.ReservedFollowers.Add(seat, followerID);
    this.FoodStorage.Data.Inventory[seat].QuantityReserved = 1;
  }

  public void SetDrinkUnreserved(int seat)
  {
    this.FoodStorage.Data.Inventory[seat].QuantityReserved = 0;
    this.Data.ReservedFollowers.Remove(seat);
  }

  public int GetDrinkReservedFollower(int seat)
  {
    return this.IsDrinkReserved(seat) ? this.Data.ReservedFollowers[seat] : -1;
  }

  public bool IsDrinkReserved(int seat)
  {
    return this.Data.ReservedFollowers.ContainsKey(seat) && this.FoodStorage.Data.Inventory.Count >= seat && this.FoodStorage.Data.Inventory[seat] != null && FollowerInfo.GetInfoByID(this.Data.ReservedFollowers[seat]) != null;
  }

  public bool DoesFollowerHaveDrinkReserved(int followerID)
  {
    foreach (KeyValuePair<int, int> reservedFollower in this.Data.ReservedFollowers)
    {
      if (reservedFollower.Value == followerID)
        return true;
    }
    return false;
  }

  public int GetFollowerDrinkReservedSeat(int followerID)
  {
    foreach (KeyValuePair<int, int> reservedFollower in this.Data.ReservedFollowers)
    {
      if (reservedFollower.Value == followerID)
        return reservedFollower.Key;
    }
    return -1;
  }

  public int GetAmountOfPreparedDrinks()
  {
    int ofPreparedDrinks = 0;
    for (int index = 0; index < this.FoodStorage.Data.Inventory.Count; ++index)
    {
      if (this.FoodStorage.Data.Inventory[index] != null)
        ++ofPreparedDrinks;
    }
    return ofPreparedDrinks;
  }

  public override void OnRemoved()
  {
    base.OnRemoved();
    Structures_Pub.IsDrinking = false;
  }

  public delegate void BrewEvent();
}
