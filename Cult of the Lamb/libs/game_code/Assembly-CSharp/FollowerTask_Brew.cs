// Decompiled with JetBrains decompiler
// Type: FollowerTask_Brew
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using Spine;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerTask_Brew : FollowerTask
{
  public int structureID;
  public Structures_Pub pubStructure;
  public Follower follower;
  public EventInstance loop;
  public bool loopStarted;

  public override FollowerTaskType Type => FollowerTaskType.Brew;

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
    return FollowerRole == FollowerRole.Bartender ? PriorityCategory.OverrideWorkPriority : PriorityCategory.Low;
  }

  public FollowerTask_Brew(int structureID)
  {
    this.structureID = structureID;
    this.pubStructure = StructureManager.GetStructureByID<Structures_Pub>(structureID);
  }

  public override int GetSubTaskCode() => 0;

  public override void ClaimReservations()
  {
    if (this.pubStructure == null)
      return;
    this.pubStructure.ReservedForTask = true;
  }

  public override void ReleaseReservations()
  {
    if (this.pubStructure == null)
      return;
    this.pubStructure.ReservedForTask = false;
  }

  public override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  public override void TaskTick(float deltaGameTime)
  {
    if (this.State != FollowerTaskState.Doing)
      return;
    if (this.pubStructure == null)
      this.pubStructure = StructureManager.GetStructureByID<Structures_Pub>(this.structureID);
    if (this.pubStructure.Data.CurrentCookingMeal == null)
    {
      Interaction_Kitchen.QueuedMeal bestPossibleDrink = this.pubStructure.GetBestPossibleDrink();
      if (bestPossibleDrink == null)
        this.End();
      else
        this.pubStructure.Data.CurrentCookingMeal = bestPossibleDrink;
    }
    else if ((double) this.pubStructure.Data.CurrentCookingMeal.CookedTime >= (double) this.pubStructure.Data.CurrentCookingMeal.CookingDuration)
      this.DrinkFinishedBrewing();
    else
      this.pubStructure.Data.CurrentCookingMeal.CookedTime += deltaGameTime * this._brain.Info.ProductivityMultiplier;
    if (!Structures_Pub.IsDrinking)
      return;
    this.End();
  }

  public void DrinkFinishedBrewing()
  {
    ++DataManager.Instance.DrinksBrewed;
    InventoryItem.ITEM_TYPE mealType = this.pubStructure.Data.QueuedMeals[0].MealType;
    this.pubStructure.Data.QueuedMeals.RemoveAt(0);
    foreach (InventoryItem ingredient in this.pubStructure.Data.CurrentCookingMeal.Ingredients)
      this.pubStructure.RemoveItems((InventoryItem.ITEM_TYPE) ingredient.type, ingredient.quantity);
    Interaction_Pub pub = this.FindPub();
    if ((bool) (Object) pub)
    {
      pub.DrinkFinishedBrewing();
    }
    else
    {
      this.pubStructure.Data.QueuedResources.Add(this.pubStructure.Data.CurrentCookingMeal.MealType);
      this.pubStructure.FoodStorage.DepositItemUnstacked(this.pubStructure.Data.CurrentCookingMeal.MealType);
      CookingData.CookedMeal(this.pubStructure.Data.CurrentCookingMeal.MealType);
      this.pubStructure.Data.CurrentCookingMeal = (Interaction_Kitchen.QueuedMeal) null;
    }
    List<Objectives_Drink> objectivesDrinkList = new List<Objectives_Drink>();
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      if (objective is Objectives_Drink objectivesDrink && objectivesDrink.DrinkType == mealType)
      {
        Follower followerById = FollowerManager.FindFollowerByID(objectivesDrink.TargetFollower);
        if ((Object) followerById != (Object) null && !FollowerManager.FollowerLocked(in objectivesDrink.TargetFollower))
        {
          followerById.Brain.CancelTargetedDrink(mealType);
          followerById.Brain.SetPersonalOverrideTask(FollowerTaskType.Drinking, CookingData.GetStructureFromMealType(mealType));
          followerById.Brain.CompleteCurrentTask();
          break;
        }
      }
    }
    this.pubStructure.DrinkBrewed();
    if (this.ShouldCook() && this.pubStructure.GetAllPossibleDrinks() > 0)
      return;
    this.Complete();
  }

  public override Vector3 UpdateDestination(Follower follower) => this.GetKitchenPosition();

  public Vector3 GetKitchenPosition()
  {
    Interaction_Pub pub = this.FindPub();
    return (Object) pub != (Object) null ? pub.FollowerPosiiton.transform.position : this.pubStructure.Data.Position + new Vector3(0.0f, 2.121f);
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    follower.SetHat(FollowerHatType.Bartender);
  }

  public override void OnDoingBegin(Follower follower)
  {
    this.follower = follower;
    follower.SetHat(FollowerHatType.Bartender);
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    double num = (double) follower.SetBodyAnimation("Drinking/bartender-up", false);
    follower.AddBodyAnimation("Drinking/bartender-shaker", true, 0.0f);
    follower.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.OnEvent);
    follower.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.OnEvent);
  }

  public void OnEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (e.Data.Name == "Audio/egg_bounce" && !this.loopStarted)
    {
      this.loopStarted = true;
      this.loop = AudioManager.Instance.CreateLoop("event:/building/brewery/fol_shake_drink", this.follower.gameObject, true);
    }
    else
    {
      if (!(e.Data.Name == "Audio/Follower poors keg"))
        return;
      AudioManager.Instance.PlayOneShot("event:/building/brewery/fol_poors_keg", this.follower.gameObject);
    }
  }

  public override void OnEnd()
  {
    base.OnEnd();
    AudioManager.Instance.StopLoop(this.loop);
  }

  public override void OnAbort()
  {
    base.OnAbort();
    AudioManager.Instance.StopLoop(this.loop);
  }

  public override void Cleanup(Follower follower)
  {
    follower.SetHat(FollowerHatType.None);
    base.Cleanup(follower);
    follower.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.OnEvent);
    AudioManager.Instance.StopLoop(this.loop);
    this.loopStarted = false;
  }

  public Interaction_Pub FindPub()
  {
    foreach (Interaction_Pub pub in Interaction_Pub.Pubs)
    {
      if (pub.Structure.Brain.Data.ID == this.structureID)
        return pub;
    }
    return (Interaction_Pub) null;
  }

  public bool ShouldCook()
  {
    return this.pubStructure.GetAmountOfPreparedDrinks() < this.pubStructure.MaxQueue;
  }
}
