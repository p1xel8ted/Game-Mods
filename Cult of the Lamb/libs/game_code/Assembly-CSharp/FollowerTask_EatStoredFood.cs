// Decompiled with JetBrains decompiler
// Type: FollowerTask_EatStoredFood
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerTask_EatStoredFood : FollowerTask
{
  public const int EAT_DURATION_GAME_MINUTES = 15;
  public int _foodStorageID;
  public Structures_Kitchen _kitchen;
  public InventoryItem.ITEM_TYPE _foodType;
  public bool _reservationHeld;
  public float _progress;
  public float satationAmount;

  public override FollowerTaskType Type => FollowerTaskType.EatStoredFood;

  public override FollowerLocation Location => this._kitchen.Data.Location;

  public override bool BlockTaskChanges => true;

  public override int UsingStructureID => this._foodStorageID;

  public FollowerTask_EatStoredFood(int foodStorageID, InventoryItem.ITEM_TYPE foodType)
  {
    Debug.Log((object) "Follower eat stored food");
    this._foodStorageID = foodStorageID;
    this._kitchen = StructureManager.GetStructureByID<Structures_Kitchen>(this._foodStorageID);
    this._foodType = foodType;
  }

  public override int GetSubTaskCode() => this._foodStorageID;

  public override void ClaimReservations()
  {
    if (this._kitchen == null || !this._kitchen.FoodStorage.TryClaimFoodReservation(this._foodType) && !this._kitchen.FoodStorage.TryClaimFoodReservation(out this._foodType))
      return;
    this._reservationHeld = true;
    HungerBar.ReservedSatiation += (float) CookingData.GetSatationAmount(this._foodType) / FollowerManager.GetTotalNonLockedFollowers();
  }

  public override void ReleaseReservations()
  {
    if (this._kitchen == null || this._foodType == InventoryItem.ITEM_TYPE.NONE || !this._reservationHeld)
      return;
    this._kitchen.FoodStorage.ReleaseFoodReservation(this._foodType);
    this._reservationHeld = false;
    HungerBar.ReservedSatiation -= (float) CookingData.GetSatationAmount(this._foodType) / FollowerManager.GetTotalNonLockedFollowers();
  }

  public override void OnEnd() => base.OnEnd();

  public override void OnAbort()
  {
    base.OnAbort();
    if (this.State == FollowerTaskState.Doing)
    {
      this.Eat();
      this.AddThought();
      ObjectiveManager.CompleteEatMealObjective(CookingData.GetStructureFromMealType(this._foodType), this._brain.Info.ID);
    }
    if (!((UnityEngine.Object) FollowerManager.FindFollowerByID(this._brain.Info.ID) != (UnityEngine.Object) null))
      return;
    Interaction component = (Interaction) FollowerManager.FindFollowerByID(this._brain.Info.ID).GetComponent<interaction_FollowerInteraction>();
    if (!(bool) (UnityEngine.Object) component)
      return;
    component.enabled = true;
  }

  public override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  public override void OnArrive()
  {
    if (!this._kitchen.FoodStorage.TryEatReservedFood(this._foodType))
    {
      this.End();
    }
    else
    {
      this._kitchen.Data.QueuedResources.Remove(this._foodType);
      this._reservationHeld = false;
      base.OnArrive();
    }
  }

  public override void TaskTick(float deltaGameTime)
  {
    if (this.State != FollowerTaskState.Doing)
      return;
    this._brain.Stats.TargetBathroom = 30f;
    this._progress += deltaGameTime;
    if ((double) this._progress < 15.0)
      return;
    this.Eat();
    this.End();
  }

  public override float SatiationChange(float deltaGameTime) => 0.0f;

  public override Vector3 UpdateDestination(Follower follower)
  {
    Interaction_FollowerKitchen followerKitchen = this.FindFollowerKitchen();
    return (UnityEngine.Object) followerKitchen != (UnityEngine.Object) null ? followerKitchen.foodStorage.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * UnityEngine.Random.Range(0.25f, 0.65f) : this._kitchen.Data.Position + (Vector3) UnityEngine.Random.insideUnitCircle * UnityEngine.Random.Range(1f, 1.5f);
  }

  public Interaction_FollowerKitchen FindFollowerKitchen()
  {
    foreach (Interaction_FollowerKitchen followerKitchen in Interaction_FollowerKitchen.FollowerKitchens)
    {
      if (followerKitchen.StructureInfo.ID == this._kitchen.Data.ID)
        return followerKitchen;
    }
    return (Interaction_FollowerKitchen) null;
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    if (this.State != FollowerTaskState.Doing)
      return;
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    double num = (double) follower.SetBodyAnimation("Food/food_eat", true);
    this.SetMealSkin(follower);
  }

  public override void OnDoingBegin(Follower follower)
  {
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    double num = (double) follower.SetBodyAnimation("Food/food_eat", true);
    this.satationAmount = (float) InventoryItem.FoodSatitation(this._foodType);
    this.SetMealSkin(follower);
    Interaction component = (Interaction) follower.GetComponent<interaction_FollowerInteraction>();
    if (!(bool) (UnityEngine.Object) component)
      return;
    component.enabled = false;
  }

  public override void OnFinaliseBegin(Follower follower)
  {
    string animation = this.GetMealReaction(CookingData.GetStructureFromMealType(this._foodType));
    if (this.HasMealQuest(CookingData.GetStructureFromMealType(this._foodType)))
      animation = "Food/food-finish-good";
    follower.TimedAnimation(animation, 1.8f, (System.Action) (() =>
    {
      this.AddThought();
      this.Complete();
      ObjectiveManager.CompleteEatMealObjective(CookingData.GetStructureFromMealType(this._foodType), this._brain.Info.ID);
      if (!((UnityEngine.Object) follower != (UnityEngine.Object) null))
        return;
      Interaction component = (Interaction) follower.GetComponent<interaction_FollowerInteraction>();
      if (!(bool) (UnityEngine.Object) component)
        return;
      component.enabled = true;
    }));
  }

  public string GetMealReaction(StructureBrain.TYPES type)
  {
    switch (type)
    {
      case StructureBrain.TYPES.MEAL:
      case StructureBrain.TYPES.MEAL_MEAT:
      case StructureBrain.TYPES.MEAL_BAD_FISH:
        return "Food/food-finish";
      case StructureBrain.TYPES.MEAL_GREAT:
      case StructureBrain.TYPES.MEAL_GOOD_FISH:
      case StructureBrain.TYPES.MEAL_GREAT_FISH:
      case StructureBrain.TYPES.MEAL_EGG:
        return "Food/food-finish-good";
      case StructureBrain.TYPES.MEAL_GRASS:
        return !this._brain.HasTrait(FollowerTrait.TraitType.GrassEater) ? "Food/food-finish-bad" : "Food/food-finish";
      case StructureBrain.TYPES.MEAL_FOLLOWER_MEAT:
        return !this._brain.HasTrait(FollowerTrait.TraitType.Cannibal) ? "Food/food-finish-bad" : "Food/food-finish";
      case StructureBrain.TYPES.MEAL_POOP:
        return "Food/food-finish-bad";
      default:
        return "Food/food-finish";
    }
  }

  public void SetMealSkin(Follower follower)
  {
    Skin newSkin = new Skin("MealSkin");
    newSkin.AddSkin(follower.Spine.skeleton.Skin);
    newSkin.AddSkin(follower.Spine.Skeleton.Data.FindSkin(CookingData.GetMealSkin(CookingData.GetStructureFromMealType(this._foodType))));
    follower.OverridingOutfit = true;
    follower.Spine.skeleton.SetSkin(newSkin);
  }

  public override void Cleanup(Follower follower)
  {
    follower.OverridingOutfit = false;
    follower.SetOutfit(follower.Outfit.CurrentOutfit, false);
    if ((UnityEngine.Object) follower != (UnityEngine.Object) null)
    {
      Interaction component = (Interaction) follower.GetComponent<interaction_FollowerInteraction>();
      if ((bool) (UnityEngine.Object) component)
        component.enabled = true;
    }
    base.Cleanup(follower);
  }

  public override void SimFinaliseBegin(SimFollower simFollower)
  {
    base.SimFinaliseBegin(simFollower);
    ObjectiveManager.CompleteEatMealObjective(CookingData.GetStructureFromMealType(this._foodType), this._brain.Info.ID);
    this.AddThought();
  }

  public bool HasMealQuest(StructureBrain.TYPES mealType)
  {
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      if (objective is Objectives_EatMeal && objective.Follower == this._brain.Info.ID && ((Objectives_EatMeal) objective).MealType == mealType)
        return true;
    }
    return false;
  }

  public void Eat()
  {
    float satationAmount = (float) CookingData.GetSatationAmount(this._foodType);
    float num1 = (float) ((double) this._brain.Stats.Satiation + (double) satationAmount - 100.0);
    this._brain.Stats.Satiation += satationAmount;
    HungerBar.ReservedSatiation -= (float) CookingData.GetSatationAmount(this._foodType) / FollowerManager.GetTotalNonLockedFollowers();
    if (FollowerManager.FollowerLocked(this.Brain.Info.ID))
      num1 = satationAmount;
    if ((double) num1 <= 0.0)
      return;
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (!FollowerManager.FollowerLocked(allBrain.Info.ID))
      {
        if ((double) num1 <= 0.0)
          break;
        float num2 = (float) ((double) allBrain.Stats.Satiation + (double) num1 - 100.0);
        allBrain.Stats.Satiation += num1;
        num1 = num2;
      }
    }
  }

  public void AddThought()
  {
    CookingData.DoMealEffect(this._foodType, this._brain);
    if (this._brain.CurrentOverrideTaskType == FollowerTaskType.EatMeal)
      this._brain.ClearPersonalOverrideTaskProvider();
    switch (CookingData.GetStructureFromMealType(this._foodType))
    {
      case StructureBrain.TYPES.MEAL:
        this._brain.AddThought(Thought.AteMeal);
        break;
      case StructureBrain.TYPES.MEAL_MEAT:
        this._brain.AddThought(Thought.AteGoodMeal);
        break;
      case StructureBrain.TYPES.MEAL_GREAT:
      case StructureBrain.TYPES.MEAL_GREAT_FISH:
        this._brain.AddThought(Thought.AteSpecialMealGood);
        break;
      case StructureBrain.TYPES.MEAL_GRASS:
        if (this._brain.HasTrait(FollowerTrait.TraitType.GrassEater))
        {
          this._brain.AddThought(Thought.AteGrassMealGrassEater);
          break;
        }
        this._brain.AddThought(Thought.AteSpecialMealBad);
        break;
      case StructureBrain.TYPES.MEAL_GOOD_FISH:
        this._brain.AddThought(Thought.AteGoodMealFish);
        break;
      case StructureBrain.TYPES.MEAL_FOLLOWER_MEAT:
        if (this._brain.HasTrait(FollowerTrait.TraitType.Cannibal))
        {
          this._brain.AddThought(Thought.AteFollowerMealCannibal);
          break;
        }
        if (this._brain.Info.CursedState == Thought.Zombie)
        {
          this._brain.AddThought(Thought.ZombieAteMeal);
          break;
        }
        this._brain.AddThought(Thought.AteFollowerMeal);
        break;
      case StructureBrain.TYPES.MEAL_MUSHROOMS:
        this._brain.Stats.Brainwash(this._brain);
        using (List<FollowerBrain>.Enumerator enumerator = FollowerBrain.AllBrains.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            FollowerBrain current = enumerator.Current;
            if (current != this._brain)
            {
              if (current.HasTrait(FollowerTrait.TraitType.MushroomEncouraged))
                current.AddThought(Thought.FollowerBrainwashedSubstanceEncouraged);
              else if (current.HasTrait(FollowerTrait.TraitType.MushroomBanned))
                current.AddThought(Thought.FollowerBrainwashedSubstanceBanned);
              else
                current.AddThought(Thought.FollowerBrainwashed);
            }
          }
          break;
        }
      case StructureBrain.TYPES.MEAL_POOP:
        this._brain.AddThought(Thought.AtePoopMeal);
        break;
      case StructureBrain.TYPES.MEAL_BAD_FISH:
        this._brain.AddThought(Thought.AteBadMealFish);
        break;
    }
    switch (CookingData.GetStructureFromMealType(this._foodType))
    {
      case StructureBrain.TYPES.MEAL_GREAT:
        CultFaithManager.AddThought(Thought.Cult_AteGreatMeal, this._brain.Info.ID);
        break;
      case StructureBrain.TYPES.MEAL_GRASS:
        if (DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.GrassEater))
          break;
        CultFaithManager.AddThought(Thought.Cult_AteGrassMeal, this._brain.Info.ID);
        break;
      case StructureBrain.TYPES.MEAL_FOLLOWER_MEAT:
        if (DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.Cannibal))
          break;
        CultFaithManager.AddThought(Thought.Cult_AteFollowerMeat, this._brain.Info.ID, this.HasMealQuest(StructureBrain.TYPES.MEAL_FOLLOWER_MEAT) ? 0.0f : 1f);
        break;
      case StructureBrain.TYPES.MEAL_POOP:
        CultFaithManager.AddThought(Thought.AtePoopMeal, this._brain.Info.ID, this.HasMealQuest(StructureBrain.TYPES.MEAL_POOP) ? 0.0f : 1f);
        break;
      case StructureBrain.TYPES.MEAL_GREAT_FISH:
        CultFaithManager.AddThought(Thought.Cult_AteGreatFishMeal, this._brain.Info.ID);
        break;
    }
  }
}
