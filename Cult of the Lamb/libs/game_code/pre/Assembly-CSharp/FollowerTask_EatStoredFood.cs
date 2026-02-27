// Decompiled with JetBrains decompiler
// Type: FollowerTask_EatStoredFood
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerTask_EatStoredFood : FollowerTask
{
  public const int EAT_DURATION_GAME_MINUTES = 15;
  private int _foodStorageID;
  private Structures_FoodStorage _foodStorage;
  public InventoryItem.ITEM_TYPE _foodType;
  private bool _reservationHeld;
  private float _progress;
  private float satationAmount;

  public override FollowerTaskType Type => FollowerTaskType.EatStoredFood;

  public override FollowerLocation Location => this._foodStorage.Data.Location;

  public override bool BlockTaskChanges => true;

  public override int UsingStructureID => this._foodStorageID;

  public FollowerTask_EatStoredFood(int foodStorageID, InventoryItem.ITEM_TYPE foodType)
  {
    Debug.Log((object) "Follower eat stored food");
    this._foodStorageID = foodStorageID;
    this._foodStorage = StructureManager.GetStructureByID<Structures_FoodStorage>(this._foodStorageID);
    this._foodType = foodType;
  }

  protected override int GetSubTaskCode() => this._foodStorageID;

  public override void ClaimReservations()
  {
    if (this._foodStorage == null || !this._foodStorage.TryClaimFoodReservation(this._foodType) && !this._foodStorage.TryClaimFoodReservation(out this._foodType))
      return;
    this._reservationHeld = true;
    HungerBar.ReservedSatiation += (float) CookingData.GetSatationAmount(this._foodType) / FollowerManager.GetTotalNonLockedFollowers();
  }

  public override void ReleaseReservations()
  {
    if (this._foodStorage == null || this._foodType == InventoryItem.ITEM_TYPE.NONE || !this._reservationHeld)
      return;
    this._foodStorage.ReleaseFoodReservation(this._foodType);
    this._reservationHeld = false;
    HungerBar.ReservedSatiation -= (float) CookingData.GetSatationAmount(this._foodType) / FollowerManager.GetTotalNonLockedFollowers();
  }

  protected override void OnEnd() => base.OnEnd();

  protected override void OnAbort()
  {
    base.OnAbort();
    if (this.State != FollowerTaskState.Doing)
      return;
    this._brain.Stats.Satiation += (float) CookingData.GetSatationAmount(this._foodType);
    HungerBar.ReservedSatiation -= (float) CookingData.GetSatationAmount(this._foodType) / FollowerManager.GetTotalNonLockedFollowers();
    this.AddThought();
    ObjectiveManager.CompleteEatMealObjective(CookingData.GetStructureFromMealType(this._foodType), this._brain.Info.ID);
  }

  protected override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  protected override void OnArrive()
  {
    if (!this._foodStorage.TryEatReservedFood(this._foodType))
    {
      this.End();
    }
    else
    {
      this._reservationHeld = false;
      base.OnArrive();
    }
  }

  protected override void TaskTick(float deltaGameTime)
  {
    if (this.State != FollowerTaskState.Doing)
      return;
    this._brain.Stats.TargetBathroom = 30f;
    this._progress += deltaGameTime;
    if ((double) this._progress < 15.0)
      return;
    this._brain.Stats.Satiation += (float) CookingData.GetSatationAmount(this._foodType);
    HungerBar.ReservedSatiation -= (float) CookingData.GetSatationAmount(this._foodType) / FollowerManager.GetTotalNonLockedFollowers();
    this.End();
  }

  protected override float SatiationChange(float deltaGameTime) => 0.0f;

  protected override Vector3 UpdateDestination(Follower follower)
  {
    return this._foodStorage.Data.Position + (Vector3) UnityEngine.Random.insideUnitCircle * UnityEngine.Random.Range(1f, 1.5f);
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
    }));
  }

  private string GetMealReaction(StructureBrain.TYPES type)
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

  private void SetMealSkin(Follower follower)
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
    base.Cleanup(follower);
  }

  public override void SimCleanup(SimFollower simFollower)
  {
    base.SimCleanup(simFollower);
    ObjectiveManager.CompleteEatMealObjective(CookingData.GetStructureFromMealType(this._foodType), this._brain.Info.ID);
    this.AddThought();
  }

  private bool HasMealQuest(StructureBrain.TYPES mealType)
  {
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      if (objective is Objectives_EatMeal && objective.Follower == this._brain.Info.ID && ((Objectives_EatMeal) objective).MealType == mealType)
        return true;
    }
    return false;
  }

  private void AddThought()
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
        {
          CultFaithManager.AddThought(Thought.Cult_AteGrassMealTrait, this._brain.Info.ID);
          break;
        }
        CultFaithManager.AddThought(Thought.Cult_AteGrassMeal, this._brain.Info.ID);
        break;
      case StructureBrain.TYPES.MEAL_FOLLOWER_MEAT:
        if (!DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.Cannibal))
        {
          CultFaithManager.AddThought(Thought.Cult_AteFollowerMeat, this._brain.Info.ID, this.HasMealQuest(StructureBrain.TYPES.MEAL_FOLLOWER_MEAT) ? 0.0f : 1f);
          break;
        }
        CultFaithManager.AddThought(Thought.Cult_AteFollowerMeatTrait, this._brain.Info.ID);
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
