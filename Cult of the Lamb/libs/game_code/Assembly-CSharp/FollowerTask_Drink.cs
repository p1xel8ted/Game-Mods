// Decompiled with JetBrains decompiler
// Type: FollowerTask_Drink
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using Spine;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerTask_Drink : FollowerTask
{
  public const int DRINK_DURATION_GAME_MINUTES = 20;
  public int _seatPosition;
  public float _progress;
  public InventoryItem _targetDrink;
  public Structures_Pub _pub;
  public bool leader;
  public List<FollowerBrain> otherDrinkers = new List<FollowerBrain>();
  public static int reservedDrinkIndex = -1;
  public Follower follower;
  public bool arrived;
  public EventInstance loop;

  public override FollowerTaskType Type => FollowerTaskType.Drinking;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockTaskChanges => true;

  public override bool BlockReactTasks => true;

  public override bool BlockSocial => true;

  public override int UsingStructureID => this._seatPosition;

  public InventoryItem.ITEM_TYPE DrinkType => (InventoryItem.ITEM_TYPE) this._targetDrink.type;

  public FollowerTask_Drink(int seatPosition, InventoryItem targetDrink, Structures_Pub pub)
  {
    this._seatPosition = seatPosition;
    this._targetDrink = targetDrink;
    this._pub = pub;
  }

  public override PriorityCategory GetPriorityCategory(
    FollowerRole FollowerRole,
    WorkerPriority WorkerPriority,
    FollowerBrain brain)
  {
    List<Objectives_Drink> objectivesDrinkList = new List<Objectives_Drink>();
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      if (objective is Objectives_Drink objectivesDrink)
        objectivesDrinkList.Add(objectivesDrink);
    }
    foreach (Objectives_Drink objectivesDrink in objectivesDrinkList)
    {
      if (objectivesDrink.DrinkType == (InventoryItem.ITEM_TYPE) this._targetDrink.type)
      {
        int num = -1;
        for (int index = 0; index < this._pub.FoodStorage.Data.Inventory.Count; ++index)
        {
          if (this._pub.FoodStorage.Data.Inventory[index] != null && this._pub.FoodStorage.Data.Inventory[index].type == this._targetDrink.type && FollowerTask_Drink.reservedDrinkIndex != index)
          {
            num = index;
            break;
          }
        }
        if (objectivesDrink.TargetFollower == brain.Info.ID && FollowerTask_Drink.reservedDrinkIndex == -1)
          FollowerTask_Drink.reservedDrinkIndex = num;
        return num != -1 ? PriorityCategory.ExtremelyUrgent : PriorityCategory.Ignore;
      }
    }
    foreach (Structures_Pub structuresPub in StructureManager.GetAllStructuresOfType<Structures_Pub>())
    {
      if (structuresPub.IsDrinkReserved(this._seatPosition))
        return structuresPub.GetDrinkReservedFollower(this._seatPosition) == brain.Info.ID ? PriorityCategory.ExtremelyUrgent : PriorityCategory.Ignore;
    }
    foreach (CookingData.MealEffect mealEffect in CookingData.GetMealEffects((InventoryItem.ITEM_TYPE) this._targetDrink.type))
    {
      if (mealEffect.MealEffectType == CookingData.MealEffectType.RemoveFreezing && brain.Info.CursedState == Thought.Freezing || mealEffect.MealEffectType == CookingData.MealEffectType.RemovesIllness && brain.Info.CursedState == Thought.Ill || mealEffect.MealEffectType == CookingData.MealEffectType.RemoveMutation && brain.Info.HasTrait(FollowerTrait.TraitType.Mutated) || mealEffect.MealEffectType == CookingData.MealEffectType.RemoveMajorNegativeStates && brain.Info.CursedState != Thought.None)
        return PriorityCategory.ExtremelyUrgent;
    }
    return (double) (brain.Info.Pleasure + FollowerBrain.GetPleasureAmount(CookingData.GetPleasure((InventoryItem.ITEM_TYPE) this._targetDrink.type))) >= 65.0 ? PriorityCategory.ExtremelyUrgent : PriorityCategory.OverrideWorkPriority;
  }

  public override int GetSubTaskCode() => this._seatPosition;

  public override void ClaimReservations()
  {
    base.ClaimReservations();
    ++this._targetDrink.QuantityReserved;
  }

  public override void ReleaseReservations()
  {
    base.ReleaseReservations();
    this._targetDrink.QuantityReserved = Mathf.Max(this._targetDrink.QuantityReserved - 1, 0);
  }

  public override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  public override void OnArrive()
  {
    base.OnArrive();
    this.arrived = true;
  }

  public override void TaskTick(float deltaGameTime)
  {
    if (this.State == FollowerTaskState.Doing && ((UnityEngine.Object) BiomeBaseManager.Instance == (UnityEngine.Object) null || !BiomeBaseManager.Instance.IsInTemple))
    {
      this._progress += deltaGameTime;
      if ((double) this._progress >= 20.0 || PlayerFarming.Location != FollowerLocation.Base)
        this.End();
    }
    if (!((UnityEngine.Object) BiomeBaseManager.Instance == (UnityEngine.Object) null) && !BiomeBaseManager.Instance.IsInTemple && this._pub != null && this._pub.Data != null && !this._pub.Data.Destroyed)
      return;
    this.Abort();
  }

  public override void OnAbort()
  {
    base.OnAbort();
    AudioManager.Instance.StopLoop(this.loop);
    if ((UnityEngine.Object) FollowerManager.FindFollowerByID(this._brain.Info.ID) != (UnityEngine.Object) null)
    {
      Interaction component = (Interaction) FollowerManager.FindFollowerByID(this._brain.Info.ID).GetComponent<interaction_FollowerInteraction>();
      if ((bool) (UnityEngine.Object) component)
        component.enabled = true;
    }
    Interaction_Pub pub = this.GetPub();
    if (!((UnityEngine.Object) pub != (UnityEngine.Object) null))
      return;
    pub.foodStorage.UpdateFoodDisplayed(this._targetDrink);
  }

  public override void OnFinaliseBegin(Follower follower)
  {
    AudioManager.Instance.StopLoop(this.loop);
    if (!this.arrived)
      return;
    string animation = "Drinking/Drinking-finish";
    if (CookingData.GetSatationLevel((InventoryItem.ITEM_TYPE) this._targetDrink.type) <= 1)
      animation = "Drinking/Drinking-finish-bad";
    else if (CookingData.GetSatationLevel((InventoryItem.ITEM_TYPE) this._targetDrink.type) >= 3)
      animation = "Drinking/Drinking-finish-good";
    GameManager.GetInstance().WaitForSeconds(3.0666666f, (System.Action) (() => CookingData.DoMealEffect((InventoryItem.ITEM_TYPE) this._targetDrink.type, this._brain)));
    follower.TimedAnimation(animation, 3.0666666f, (System.Action) (() =>
    {
      this.Complete();
      if ((UnityEngine.Object) follower != (UnityEngine.Object) null)
      {
        Interaction component = (Interaction) follower.GetComponent<interaction_FollowerInteraction>();
        if ((bool) (UnityEngine.Object) component)
          component.enabled = true;
      }
      if (!this.leader)
        return;
      GameManager.GetInstance().WaitForSeconds(1f, new System.Action(this.DoAfterDrinkingActivities));
    }));
    this.FinishedDrinking();
  }

  public override void SimFinaliseBegin(SimFollower simFollower)
  {
    this.FinishedDrinking();
    base.SimFinaliseBegin(simFollower);
  }

  public void FinishedDrinking()
  {
    FollowerTask personalOverrideTask = this._brain.GetPersonalOverrideTask();
    if ((personalOverrideTask != null ? (personalOverrideTask.Type == FollowerTaskType.Drinking ? 1 : 0) : 0) != 0)
      this._brain.ClearPersonalOverrideTaskProvider();
    this._brain.AddPleasure(CookingData.GetPleasure((InventoryItem.ITEM_TYPE) this._targetDrink.type));
    this._pub.FinishedDrink(this._seatPosition, this._targetDrink);
    FollowerTask_Drink.reservedDrinkIndex = -1;
    ObjectiveManager.CompleteDrinkObjective((InventoryItem.ITEM_TYPE) this._targetDrink.type, this._brain.Info.ID);
  }

  public override float SatiationChange(float deltaGameTime) => 0.0f;

  public override Vector3 UpdateDestination(Follower follower)
  {
    return this.GetPub().GetSeatPosition(this._seatPosition);
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    if (this.State == FollowerTaskState.Doing)
    {
      follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
      double num = (double) follower.SetBodyAnimation(this.GetDrinkingAnimation(), true);
      this.SetDrinkSkin(follower);
    }
    this.follower = follower;
    follower.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.AnimationState_Event);
  }

  public void SetDrinkSkin(Follower follower)
  {
    Skin newSkin = new Skin("MealSkin");
    newSkin.AddSkin(follower.Spine.skeleton.Skin);
    newSkin.AddSkin(follower.Spine.Skeleton.Data.FindSkin(CookingData.GetDrinkSkin((InventoryItem.ITEM_TYPE) this._targetDrink.type)));
    follower.OverridingOutfit = true;
    follower.Spine.skeleton.SetSkin(newSkin);
  }

  public void AnimationState_Event(TrackEntry trackEntry, Spine.Event e)
  {
    if (e.Data.Name == "Audio/drinking vo/fol glug1")
    {
      AudioManager.Instance.StopLoop(this.loop);
      this.loop = AudioManager.Instance.CreateLoop("event:/dialogue/followers/drunk/drink_glug", this.follower.gameObject, true);
    }
    else
    {
      if (!(e.Data.Name == "Audio/drinking vo/fol refreshed ahh1"))
        return;
      AudioManager.Instance.StopLoop(this.loop);
    }
  }

  public override void OnDoingBegin(Follower follower)
  {
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    double num = (double) follower.SetBodyAnimation(this.GetDrinkingAnimation(), true);
    follower.FacePosition(this._pub.Data.Position);
    this.SetDrinkSkin(follower);
    Interaction_Pub pub = this.GetPub();
    if ((UnityEngine.Object) pub != (UnityEngine.Object) null)
      pub.RemoveDrinkFromTable(this._seatPosition);
    Interaction component = (Interaction) follower.GetComponent<interaction_FollowerInteraction>();
    if (!(bool) (UnityEngine.Object) component)
      return;
    component.enabled = false;
  }

  public override void Cleanup(Follower follower)
  {
    AudioManager.Instance.StopLoop(this.loop);
    follower.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.AnimationState_Event);
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

  public Interaction_Pub GetPub()
  {
    using (List<Interaction_Pub>.Enumerator enumerator = Interaction_Pub.Pubs.GetEnumerator())
    {
      if (enumerator.MoveNext())
        return enumerator.Current;
    }
    return (Interaction_Pub) null;
  }

  public string GetDrinkingAnimation()
  {
    return this.Brain.CurrentState.Type == FollowerStateType.Drunk ? "Drinking/Drinking-drunk" : "Drinking/Drinking";
  }

  public void DoAfterDrinkingActivities()
  {
    foreach (FollowerBrain otherDrinker in this.otherDrinkers)
    {
      float num = 0.05f;
      if (this._brain.CurrentState.Type == FollowerStateType.Drunk)
        num += 0.05f;
      if (otherDrinker.CurrentState.Type == FollowerStateType.Drunk)
        num += 0.05f;
      if ((double) UnityEngine.Random.value < (double) num)
      {
        this.Brain.HardSwapToTask((FollowerTask) new FollowerTask_FightFollower(otherDrinker.Info.ID, true));
        break;
      }
      this.Brain._directInfoAccess.Social = 0.0f;
      this.RelationshipUp(otherDrinker);
    }
    this.otherDrinkers.Clear();
  }

  public void RelationshipUp(FollowerBrain otherBrain)
  {
    int id = otherBrain.Info.ID;
    IDAndRelationship relationship = this._brain.Info.GetOrCreateRelationship(id);
    ++relationship.Relationship;
    bool flag = FollowerManager.AreSiblings(this._brain.Info.ID, id);
    if (relationship.CurrentRelationshipState < IDAndRelationship.RelationshipState.Friends && (double) relationship.Relationship >= 5.0)
    {
      relationship.CurrentRelationshipState = IDAndRelationship.RelationshipState.Friends;
      this._brain.AddThought(Thought.NewFriend);
      Action<FollowerInfo, FollowerInfo, IDAndRelationship.RelationshipState> changeRelationship = FollowerTask_Chat.OnChangeRelationship;
      if (changeRelationship == null)
        return;
      changeRelationship(this.Brain._directInfoAccess, otherBrain._directInfoAccess, IDAndRelationship.RelationshipState.Friends);
    }
    else
    {
      if (relationship.CurrentRelationshipState >= IDAndRelationship.RelationshipState.Lovers || (double) relationship.Relationship < 10.0 || flag)
        return;
      relationship.CurrentRelationshipState = IDAndRelationship.RelationshipState.Lovers;
      this._brain.AddThought(Thought.NewLover);
      Action<FollowerInfo, FollowerInfo, IDAndRelationship.RelationshipState> changeRelationship = FollowerTask_Chat.OnChangeRelationship;
      if (changeRelationship == null)
        return;
      changeRelationship(this.Brain._directInfoAccess, otherBrain._directInfoAccess, IDAndRelationship.RelationshipState.Lovers);
    }
  }
}
