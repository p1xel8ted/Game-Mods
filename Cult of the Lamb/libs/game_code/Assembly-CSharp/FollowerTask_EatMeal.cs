// Decompiled with JetBrains decompiler
// Type: FollowerTask_EatMeal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerTask_EatMeal : FollowerTask
{
  public const int EAT_DURATION_GAME_MINUTES = 15;
  public int _mealID;
  public Structures_Meal _meal;
  public float _progress;
  public bool _mealRotten;
  public bool _mealEatenByPlayer;
  public StructureBrain.TYPES MealType;
  public static Action<int> OnEatRottenFood;
  public float satationAmount;
  public bool hasArrived;

  public override FollowerTaskType Type => FollowerTaskType.EatMeal;

  public override FollowerLocation Location => this._meal.Data.Location;

  public override bool BlockTaskChanges => true;

  public override int UsingStructureID => this._mealID;

  public int TargetMeal => this._mealID;

  public FollowerTask_EatMeal(int mealID)
  {
    this._mealID = mealID;
    this._meal = StructureManager.GetStructureByID<Structures_Meal>(this._mealID);
    this.MealType = this._meal.Data.Type;
  }

  public override void OnEnd()
  {
    base.OnEnd();
    if (!this.hasArrived)
      return;
    Structures_Meal structureById = StructureManager.GetStructureByID<Structures_Meal>(this._mealID);
    if (structureById == null)
      return;
    structureById.Data.Eaten = true;
  }

  public override int GetSubTaskCode() => this._mealID;

  public override void ClaimReservations()
  {
    Structures_Meal structureById = StructureManager.GetStructureByID<Structures_Meal>(this._mealID);
    if (structureById == null)
      return;
    if (!structureById.ReservedForTask)
      HungerBar.ReservedSatiation += (float) CookingData.GetSatationAmount(CookingData.GetMealFromStructureType(this.MealType)) / FollowerManager.GetTotalNonLockedFollowers();
    structureById.ReservedForTask = true;
  }

  public override void ReleaseReservations()
  {
    Structures_Meal structureById = StructureManager.GetStructureByID<Structures_Meal>(this._mealID);
    if (structureById == null || structureById.Data.Eaten)
      return;
    if (structureById.ReservedForTask)
      HungerBar.ReservedSatiation -= (float) CookingData.GetSatationAmount(CookingData.GetMealFromStructureType(this.MealType)) / FollowerManager.GetTotalNonLockedFollowers();
    structureById.ReservedForTask = false;
  }

  public override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  public override void OnArrive()
  {
    Structures_Meal structureById = StructureManager.GetStructureByID<Structures_Meal>(this._mealID);
    if (structureById == null || structureById.ReservedByPlayer || structureById.Data.Eaten)
    {
      this.Abort();
    }
    else
    {
      this.hasArrived = true;
      this._mealRotten = structureById.Data.Rotten;
      structureById.Data.Eaten = true;
      this.MealType = structureById.Data.Type;
      this._mealID = 0;
      if (this._mealRotten)
      {
        Action<int> onEatRottenFood = FollowerTask_EatMeal.OnEatRottenFood;
        if (onEatRottenFood != null)
          onEatRottenFood(this._brain.Info.ID);
      }
      base.OnArrive();
    }
  }

  public override void TaskTick(float deltaGameTime)
  {
    if (this.State == FollowerTaskState.GoingTo && LocationManager.GetLocationState(FollowerLocation.Base) == LocationState.Active)
    {
      Meal meal = this.FindMeal();
      if ((UnityEngine.Object) meal != (UnityEngine.Object) null && meal.TakenByPlayer)
        this._mealEatenByPlayer = true;
      if ((UnityEngine.Object) meal == (UnityEngine.Object) null || meal.TakenByPlayer)
      {
        this.End();
        return;
      }
    }
    if (this.State != FollowerTaskState.Doing)
      return;
    this._brain.Stats.TargetBathroom = 30f;
    int num = this._mealRotten ? 1 : 0;
    this._progress += deltaGameTime;
    if ((double) this._progress < 15.0)
      return;
    this.Eat();
    this.End();
  }

  public void Eat()
  {
    float satationAmount = (float) CookingData.GetSatationAmount(CookingData.GetMealFromStructureType(this.MealType));
    float num1 = (float) ((double) this._brain.Stats.Satiation + (double) satationAmount - 100.0);
    this._brain.Stats.Satiation += satationAmount;
    HungerBar.ReservedSatiation -= (float) CookingData.GetSatationAmount(CookingData.GetMealFromStructureType(this.MealType)) / FollowerManager.GetTotalNonLockedFollowers();
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

  public override void OnAbort()
  {
    base.OnAbort();
    if (this.State == FollowerTaskState.Doing && !this._mealEatenByPlayer && !this._meal.Data.Eaten)
    {
      this.Eat();
      this.AddThought();
      ObjectiveManager.CompleteEatMealObjective(this.MealType, this._brain.Info.ID);
      Structures_Meal structureById = StructureManager.GetStructureByID<Structures_Meal>(this._mealID);
      if (structureById != null)
        structureById.Data.Eaten = true;
    }
    if (!((UnityEngine.Object) FollowerManager.FindFollowerByID(this._brain.Info.ID) != (UnityEngine.Object) null))
      return;
    Interaction component = (Interaction) FollowerManager.FindFollowerByID(this._brain.Info.ID).GetComponent<interaction_FollowerInteraction>();
    if (!(bool) (UnityEngine.Object) component)
      return;
    component.enabled = true;
  }

  public override void OnFinaliseBegin(Follower follower)
  {
    string animation = this.GetMealReaction(this.MealType);
    if (this.HasMealQuest(this.MealType))
      animation = "Food/food-finish-good";
    FollowerTask personalOverrideTask = this._brain.GetPersonalOverrideTask();
    if ((personalOverrideTask != null ? (personalOverrideTask.Type == FollowerTaskType.EatMeal ? 1 : 0) : 0) != 0)
      this._brain.ClearPersonalOverrideTaskProvider();
    if (this._mealEatenByPlayer)
      return;
    follower.TimedAnimation(animation, 1.8f, (System.Action) (() =>
    {
      this.AddThought();
      this.Complete();
      ObjectiveManager.CompleteEatMealObjective(this.MealType, this._brain.Info.ID);
      if (!((UnityEngine.Object) follower != (UnityEngine.Object) null))
        return;
      Interaction component = (Interaction) follower.GetComponent<interaction_FollowerInteraction>();
      if (!(bool) (UnityEngine.Object) component)
        return;
      component.enabled = true;
    }));
  }

  public override void SimFinaliseBegin(SimFollower simFollower)
  {
    this.AddThought();
    ObjectiveManager.CompleteEatMealObjective(this.MealType, this._brain.Info.ID);
    FollowerTask personalOverrideTask = this._brain.GetPersonalOverrideTask();
    if ((personalOverrideTask != null ? (personalOverrideTask.Type == FollowerTaskType.EatMeal ? 1 : 0) : 0) != 0)
      this._brain.ClearPersonalOverrideTaskProvider();
    base.SimFinaliseBegin(simFollower);
  }

  public override float SatiationChange(float deltaGameTime) => 0.0f;

  public override Vector3 UpdateDestination(Follower follower)
  {
    Structures_Meal structureById = StructureManager.GetStructureByID<Structures_Meal>(this._mealID);
    return structureById != null && structureById.Data != null ? structureById.Data.Position : follower.Brain.LastPosition;
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    follower.Brain.ShouldReconsiderTask = false;
    if (this.State != FollowerTaskState.Doing)
      return;
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    double num = (double) follower.SetBodyAnimation("Food/food_eat", true);
    this.SetMealSkin(follower);
  }

  public void SetMealSkin(Follower follower)
  {
    Skin newSkin = new Skin("MealSkin");
    newSkin.AddSkin(follower.Spine.skeleton.Skin);
    newSkin.AddSkin(follower.Spine.Skeleton.Data.FindSkin(CookingData.GetMealSkin(this.MealType)));
    follower.OverridingOutfit = true;
    follower.Spine.skeleton.SetSkin(newSkin);
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
      case StructureBrain.TYPES.MEAL_GREAT_MIXED:
      case StructureBrain.TYPES.MEAL_GREAT_MEAT:
      case StructureBrain.TYPES.MEAL_EGG:
        return "Food/food-finish-good";
      case StructureBrain.TYPES.MEAL_GRASS:
        return !this._brain.HasTrait(FollowerTrait.TraitType.GrassEater) ? "Food/food-finish-bad" : "Food/food-finish";
      case StructureBrain.TYPES.MEAL_FOLLOWER_MEAT:
        return !this._brain.HasTrait(FollowerTrait.TraitType.Cannibal) ? "Food/food-finish-bad" : "Food/food-finish";
      case StructureBrain.TYPES.MEAL_POOP:
        return "Food/food-finish-bad";
      case StructureBrain.TYPES.MEAL_BURNED:
        return "Food/food-finish-hot";
      default:
        return "Food/food-finish";
    }
  }

  public override void OnDoingBegin(Follower follower)
  {
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    double num = (double) follower.SetBodyAnimation("Food/food_eat", true);
    this.SetMealSkin(follower);
    this.satationAmount = (float) InventoryItem.FoodSatitation(CookingData.GetMealFromStructureType(this.MealType));
    Interaction component = (Interaction) follower.GetComponent<interaction_FollowerInteraction>();
    if (!(bool) (UnityEngine.Object) component)
      return;
    component.enabled = false;
  }

  public override void OnComplete() => base.OnComplete();

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

  public void AddThought()
  {
    Debug.Log((object) "ADD THOUGHT!");
    CookingData.DoMealEffect(CookingData.GetMealFromStructureType(this.MealType), this._brain);
    if (this._brain.CurrentOverrideTaskType == FollowerTaskType.EatMeal)
      this._brain.ClearPersonalOverrideTaskProvider();
    if (this._mealRotten)
    {
      if (this.MealType == StructureBrain.TYPES.MEAL_FOLLOWER_MEAT)
      {
        if (this._brain.HasTrait(FollowerTrait.TraitType.Cannibal))
          this._brain.AddThought(Thought.AteRottenFollowerMealCannibal);
        else
          this._brain.AddThought(Thought.AteRottenFollowerMeal);
      }
      else
        this._brain.AddThought(Thought.AteRottenMeal);
    }
    else
    {
      switch (this.MealType)
      {
        case StructureBrain.TYPES.MEAL:
          this._brain.AddThought(Thought.AteMeal);
          break;
        case StructureBrain.TYPES.MEAL_MEAT:
          this._brain.AddThought(Thought.AteGoodMeal);
          break;
        case StructureBrain.TYPES.MEAL_GREAT:
        case StructureBrain.TYPES.MEAL_GREAT_FISH:
        case StructureBrain.TYPES.MEAL_GREAT_MIXED:
        case StructureBrain.TYPES.MEAL_GREAT_MEAT:
        case StructureBrain.TYPES.MEAL_EGG:
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
    }
    switch (this.MealType)
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

  public Meal FindMeal()
  {
    Meal meal1 = (Meal) null;
    foreach (Meal meal2 in Meal.Meals)
    {
      if ((UnityEngine.Object) meal2 != (UnityEngine.Object) null && meal2.StructureInfo != null && meal2.StructureInfo.ID == this._mealID)
      {
        meal1 = meal2;
        break;
      }
    }
    return meal1;
  }
}
