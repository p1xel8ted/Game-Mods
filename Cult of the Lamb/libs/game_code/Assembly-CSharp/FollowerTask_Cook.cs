// Decompiled with JetBrains decompiler
// Type: FollowerTask_Cook
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerTask_Cook : FollowerTask
{
  public int structureID;
  public Structures_Kitchen kitchenStructure;

  public override FollowerTaskType Type => FollowerTaskType.Cook;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override float Priorty => 30f;

  public override bool BlockReactTasks => true;

  public override bool BlockSocial => true;

  public override int UsingStructureID => this.structureID;

  public override PriorityCategory GetPriorityCategory(
    FollowerRole FollowerRole,
    WorkerPriority WorkerPriority,
    FollowerBrain brain)
  {
    return FollowerRole == FollowerRole.Chef && this.ShouldCook() ? PriorityCategory.WorkPriority : PriorityCategory.Low;
  }

  public FollowerTask_Cook(int structureID)
  {
    this.structureID = structureID;
    this.kitchenStructure = StructureManager.GetStructureByID<Structures_Kitchen>(structureID);
  }

  public override int GetSubTaskCode() => 0;

  public override void ClaimReservations()
  {
    if (this.kitchenStructure == null)
      return;
    this.kitchenStructure.ReservedForTask = true;
  }

  public override void ReleaseReservations()
  {
    if (this.kitchenStructure == null)
      return;
    this.kitchenStructure.ReservedForTask = false;
  }

  public override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  public override void TaskTick(float deltaGameTime)
  {
    if (this.State != FollowerTaskState.Doing)
      return;
    if (this.kitchenStructure == null)
      this.kitchenStructure = StructureManager.GetStructureByID<Structures_Kitchen>(this.structureID);
    if (this.kitchenStructure.Data.CurrentCookingMeal == null)
    {
      Interaction_Kitchen.QueuedMeal bestPossibleMeal = this.kitchenStructure.GetBestPossibleMeal();
      if (bestPossibleMeal == null)
        this.End();
      else
        this.kitchenStructure.Data.CurrentCookingMeal = bestPossibleMeal;
    }
    else if ((double) this.kitchenStructure.Data.CurrentCookingMeal.CookedTime >= (double) this.kitchenStructure.Data.CurrentCookingMeal.CookingDuration)
      this.MealFinishedCooking();
    else
      this.kitchenStructure.Data.CurrentCookingMeal.CookedTime += deltaGameTime * this._brain.Info.ProductivityMultiplier;
  }

  public void MealFinishedCooking()
  {
    ++DataManager.Instance.MealsCooked;
    ObjectiveManager.CheckObjectives(Objectives.TYPES.COOK_MEALS);
    InventoryItem.ITEM_TYPE mealType = this.kitchenStructure.Data.QueuedMeals[0].MealType;
    this.kitchenStructure.Data.QueuedMeals.RemoveAt(0);
    foreach (InventoryItem ingredient in this.kitchenStructure.Data.CurrentCookingMeal.Ingredients)
      this.kitchenStructure.RemoveItems((InventoryItem.ITEM_TYPE) ingredient.type, ingredient.quantity);
    Interaction_FollowerKitchen kitchen = this.FindKitchen();
    if ((bool) (Object) kitchen)
    {
      kitchen.MealFinishedCooking();
    }
    else
    {
      this.kitchenStructure.Data.QueuedResources.Add(this.kitchenStructure.Data.CurrentCookingMeal.MealType);
      this.kitchenStructure.FoodStorage.DepositItemUnstacked(this.kitchenStructure.Data.CurrentCookingMeal.MealType);
      CookingData.CookedMeal(this.kitchenStructure.Data.CurrentCookingMeal.MealType);
      ObjectiveManager.CheckObjectives(Objectives.TYPES.COOK_MEALS);
      this.kitchenStructure.Data.CurrentCookingMeal = (Interaction_Kitchen.QueuedMeal) null;
    }
    this.kitchenStructure.MealCooked();
    foreach (CookingData.MealEffect mealEffect in CookingData.GetMealEffects(mealType))
    {
      foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
      {
        if (!FollowerManager.FollowerLocked(allBrain.Info.ID, true, excludeFreezing: true) && CookingData.GetMealFromStructureType(allBrain.CurrentOverrideStructureType) == InventoryItem.ITEM_TYPE.NONE && !(allBrain.CurrentTask is FollowerTask_AttendRitual) && (mealEffect.MealEffectType == CookingData.MealEffectType.RemovesIllness && allBrain.Info.CursedState == Thought.Ill || mealEffect.MealEffectType == CookingData.MealEffectType.OldFollowerYoung && allBrain.Info.CursedState == Thought.OldAge || mealEffect.MealEffectType == CookingData.MealEffectType.RemoveFreezing && allBrain.Info.CursedState == Thought.Freezing || mealEffect.MealEffectType == CookingData.MealEffectType.RemovesDissent && allBrain.Info.CursedState == Thought.Dissenter || mealEffect.MealEffectType == CookingData.MealEffectType.InstantlyDie && allBrain.Info.Necklace == InventoryItem.ITEM_TYPE.Necklace_Targeted || mealEffect.MealEffectType == CookingData.MealEffectType.CausesIllPoopy && allBrain.Info.Necklace == InventoryItem.ITEM_TYPE.Necklace_Targeted))
        {
          allBrain.SetPersonalOverrideTask(FollowerTaskType.EatMeal, CookingData.GetStructureFromMealType(mealType));
          allBrain.HardSwapToTask((FollowerTask) new FollowerTask_EatStoredFood(this.FindKitchenStructure().Data.ID, mealType));
          break;
        }
      }
    }
    if (this.ShouldCook() && this.kitchenStructure.GetAllPossibleMeals() > 0)
      return;
    this.Complete();
  }

  public override Vector3 UpdateDestination(Follower follower) => this.GetKitchenPosition();

  public Vector3 GetKitchenPosition()
  {
    Interaction_FollowerKitchen kitchen = this.FindKitchen();
    return (Object) kitchen != (Object) null ? kitchen.CookPosition.transform.position : this.kitchenStructure.Data.Position + new Vector3(0.0f, 2.121f);
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    if (this.kitchenStructure == null)
      return;
    follower.SetHat(FollowerHatType.Chef);
  }

  public override void OnDoingBegin(Follower follower)
  {
    if ((Object) this.FindKitchen() == (Object) null)
    {
      this.End();
    }
    else
    {
      follower.SetHat(FollowerHatType.Chef);
      follower.FacePosition(this.FindKitchen().CookingMealAnimation.transform.position);
      follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
      double num = (double) follower.SetBodyAnimation("cook", true);
    }
  }

  public override void Cleanup(Follower follower)
  {
    follower.SetHat(FollowerHatType.None);
    base.Cleanup(follower);
  }

  public Interaction_FollowerKitchen FindKitchen()
  {
    foreach (Interaction_FollowerKitchen followerKitchen in Interaction_FollowerKitchen.FollowerKitchens)
    {
      if (followerKitchen.StructureInfo.ID == this.structureID)
        return followerKitchen;
    }
    return (Interaction_FollowerKitchen) null;
  }

  public Structures_Kitchen FindKitchenStructure()
  {
    return StructureManager.GetStructureByID<Structures_Kitchen>(this.structureID);
  }

  public bool ShouldCook()
  {
    return this.kitchenStructure.FoodStorage.Data.Inventory.Count < this.kitchenStructure.FoodStorage.Capacity - 1;
  }
}
