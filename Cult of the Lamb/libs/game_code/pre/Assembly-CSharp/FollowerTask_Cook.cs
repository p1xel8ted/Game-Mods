// Decompiled with JetBrains decompiler
// Type: FollowerTask_Cook
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerTask_Cook : FollowerTask
{
  private Structures_Kitchen kitchenStructure;
  private int kitchenStationID;

  public override FollowerTaskType Type => FollowerTaskType.Cook;

  public override FollowerLocation Location => this.kitchenStructure.Data.Location;

  public override float Priorty => 30f;

  public override PriorityCategory GetPriorityCategory(
    FollowerRole FollowerRole,
    WorkerPriority WorkerPriority,
    FollowerBrain brain)
  {
    return FollowerRole == FollowerRole.Chef ? PriorityCategory.WorkPriority : PriorityCategory.Low;
  }

  public FollowerTask_Cook(int kitchenStationID)
  {
    this.kitchenStationID = kitchenStationID;
    this.kitchenStructure = StructureManager.GetStructureByID<Structures_Kitchen>(kitchenStationID);
  }

  protected override int GetSubTaskCode() => this.kitchenStationID;

  public override void ClaimReservations()
  {
    Structures_Kitchen structureById = StructureManager.GetStructureByID<Structures_Kitchen>(this.kitchenStationID);
    if (structureById == null)
      return;
    structureById.ReservedForTask = true;
  }

  public override void ReleaseReservations()
  {
    Structures_Kitchen structureById = StructureManager.GetStructureByID<Structures_Kitchen>(this.kitchenStationID);
    if (structureById == null)
      return;
    structureById.ReservedForTask = false;
  }

  protected override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  protected override void TaskTick(float deltaGameTime)
  {
    if (this.State != FollowerTaskState.Doing || this.kitchenStructure.Data.QueuedMeals.Count <= 0)
      return;
    if (this.kitchenStructure.Data.CurrentCookingMeal == null)
      this.kitchenStructure.Data.CurrentCookingMeal = this.kitchenStructure.Data.QueuedMeals[0];
    else if ((double) this.kitchenStructure.Data.CurrentCookingMeal.CookedTime >= (double) this.kitchenStructure.Data.CurrentCookingMeal.CookingDuration)
      this.MealFinishedCooking();
    else
      this.kitchenStructure.Data.CurrentCookingMeal.CookedTime += deltaGameTime * this._brain.Info.ProductivityMultiplier;
  }

  private void MealFinishedCooking()
  {
    ++DataManager.Instance.MealsCooked;
    ObjectiveManager.CheckObjectives(Objectives.TYPES.COOK_MEALS);
    Interaction_Kitchen kitchen = this.FindKitchen();
    if ((bool) (Object) kitchen)
    {
      kitchen.MealFinishedCooking();
    }
    else
    {
      Structures_FoodStorage availableFoodStorage = Structures_FoodStorage.GetAvailableFoodStorage(this.kitchenStructure.Data.Position, this.Location);
      if (availableFoodStorage != null)
      {
        availableFoodStorage.DepositItemUnstacked(this.kitchenStructure.Data.CurrentCookingMeal.MealType);
      }
      else
      {
        StructureBrain.TYPES mealStructureType = StructuresData.GetMealStructureType(this.kitchenStructure.Data.CurrentCookingMeal.MealType);
        Vector3 position = this.kitchenStructure.Data.Position + (Vector3) Random.insideUnitCircle * 2f;
        StructureManager.BuildStructure(this.kitchenStructure.Data.Location, StructuresData.GetInfoByType(mealStructureType, 0), position, Vector2Int.one);
      }
      CookingData.CookedMeal(this.kitchenStructure.Data.CurrentCookingMeal.MealType);
      ObjectiveManager.CheckObjectives(Objectives.TYPES.COOK_MEALS);
      this.kitchenStructure.Data.QueuedMeals.Remove(this.kitchenStructure.Data.CurrentCookingMeal);
      this.kitchenStructure.Data.CurrentCookingMeal = (Interaction_Kitchen.QueuedMeal) null;
      this.kitchenStructure.Data.Fuel -= 10;
    }
    if (this.kitchenStructure.Data.QueuedMeals.Count > 0)
      return;
    this.Complete();
  }

  protected override Vector3 UpdateDestination(Follower follower)
  {
    return this.kitchenStructure.Data.Position + new Vector3(0.0f, 2.121f);
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    if (this.kitchenStationID == 0)
      return;
    follower.SetHat(HatType.Chef);
  }

  public override void OnDoingBegin(Follower follower)
  {
    if (this.kitchenStationID == 0)
      follower.SetHat(HatType.Chef);
    follower.FacePosition(this.kitchenStructure.Data.Position);
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    double num = (double) follower.SetBodyAnimation("action", true);
  }

  public override void Cleanup(Follower follower)
  {
    follower.SetHat(HatType.None);
    base.Cleanup(follower);
  }

  private Interaction_Kitchen FindKitchen()
  {
    foreach (Interaction_Kitchen kitchen in Interaction_Kitchen.Kitchens)
    {
      if (kitchen.structure.Structure_Info.ID == this.kitchenStationID)
        return kitchen;
    }
    return (Interaction_Kitchen) null;
  }
}
