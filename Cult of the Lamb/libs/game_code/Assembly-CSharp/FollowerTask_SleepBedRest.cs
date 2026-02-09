// Decompiled with JetBrains decompiler
// Type: FollowerTask_SleepBedRest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerTask_SleepBedRest : FollowerTask
{
  public const int SLEEP_DURATION_GAME_MINUTES = 240 /*0xF0*/;
  public const int HEAL_DURATION_GAME_MINUTES = 2400;
  public static Action<int> OnHomelessSleep;
  public static Action<int> OnWake;
  public bool sleepingOnFloor;
  public int overrideHealingBayID = -1;
  public float HealingDelay;
  public PlacementRegion.TileGridTile ClosestTile;

  public override FollowerTaskType Type => FollowerTaskType.SleepBedRest;

  public override FollowerLocation Location
  {
    get
    {
      return this._sleepingInBed ? this._brain.GetAssignedDwellingStructure().Data.Location : this._brain.Location;
    }
  }

  public override bool BlockTaskChanges => true;

  public override bool BlockReactTasks => true;

  public override bool BlockSermon => true;

  public override int UsingStructureID
  {
    get
    {
      int usingStructureId = 0;
      if (this._sleepingInBed)
        usingStructureId = this._brain.GetAssignedDwellingStructure().Data.ID;
      return usingStructureId;
    }
  }

  public bool _sleepingInBed
  {
    get
    {
      return this._brain.HasHome && this._brain.GetAssignedDwellingStructure() != null && !this._brain.GetAssignedDwellingStructure().IsCollapsed;
    }
  }

  public FollowerTask_SleepBedRest(int healingBayID = -1)
  {
    this.overrideHealingBayID = healingBayID;
  }

  public override int GetSubTaskCode() => 0;

  public override void ClaimReservations()
  {
    if (this.ClosestTile == null)
      return;
    this.ClosestTile.ReservedForWaste = true;
  }

  public override void ReleaseReservations()
  {
    if (this.ClosestTile == null)
      return;
    this.ClosestTile.ReservedForWaste = false;
  }

  public override void OnStart()
  {
    Debug.Log((object) (this._brain.Info.Name + " OnStart!"));
    if (this._sleepingInBed)
    {
      Structures_Bed dwellingStructure = this._brain.GetAssignedDwellingStructure();
      foreach (int multipleFollowerId in dwellingStructure.Data.MultipleFollowerIDs)
      {
        FollowerInfo infoById = FollowerInfo.GetInfoByID(multipleFollowerId);
        if (infoById != null && infoById.DwellingSlot == this._brain._directInfoAccess.DwellingSlot)
        {
          for (int slot = 0; slot < dwellingStructure.SlotCount; ++slot)
          {
            if (!dwellingStructure.CheckIfSlotIsOccupied(slot))
            {
              this._brain._directInfoAccess.DwellingSlot = slot;
              break;
            }
          }
          break;
        }
      }
    }
    this.SetState(FollowerTaskState.GoingTo);
    ObjectiveManager.CheckObjectives(Objectives.TYPES.SEND_FOLLOWER_BED_REST);
  }

  public override void OnArrive()
  {
    Debug.Log((object) (this._brain.Info.Name + " OnArrive!"));
    if (!this._brain.HasHome)
    {
      Action<int> onHomelessSleep = FollowerTask_SleepBedRest.OnHomelessSleep;
      if (onHomelessSleep != null)
        onHomelessSleep(this._brain.Info.ID);
    }
    base.OnArrive();
  }

  public override void OnEnd()
  {
    base.OnEnd();
    if (this._brain.CurrentOverrideTaskType != FollowerTaskType.SleepBedRest)
      return;
    this._brain.ClearPersonalOverrideTaskProvider();
  }

  public override void OnComplete()
  {
    Debug.Log((object) (this._brain.Info.Name + " OnComplete!"));
    Action<int> onWake = FollowerTask_SleepBedRest.OnWake;
    if (onWake == null)
      return;
    onWake(this._brain.Info.ID);
  }

  public override void TaskTick(float deltaGameTime)
  {
    if (this.State == FollowerTaskState.Doing)
    {
      if ((double) this._brain.Stats.Illness > 0.0 && (double) (this.HealingDelay += deltaGameTime) > 15.0)
      {
        ThoughtData thought = this._brain.GetThought(Thought.Ill);
        if (thought == null)
        {
          Debug.Log((object) "ill thought missing");
          this._brain.AddThought(Thought.Ill);
        }
        else if ((double) TimeManager.TotalElapsedGameTime - (double) thought.TimeStarted[0] < 1200.0)
        {
          Debug.Log((object) "update thought duration");
          thought.TimeStarted[0] = TimeManager.TotalElapsedGameTime;
        }
        else
          Debug.Log((object) ("C  " + thought.Duration.ToString()));
        this.HealingDelay = 0.0f;
        if ((double) this._brain.Stats.Illness <= 0.0 && this._brain.CurrentOverrideTaskType == FollowerTaskType.SleepBedRest)
          this._brain.ClearPersonalOverrideTaskProvider();
      }
      if ((double) this._brain.Stats.Illness <= 0.0 && TimeManager.CurrentPhase != DayPhase.Night)
      {
        Follower follower = FollowerManager.FindFollowerByID(this._brain.Info.ID);
        FollowerBrainStats.StatStateChangedEvent illnessStateChanged = FollowerBrainStats.OnIllnessStateChanged;
        if (illnessStateChanged != null)
          illnessStateChanged(this._brain.Info.ID, FollowerStatState.Off, FollowerStatState.On);
        if ((UnityEngine.Object) follower != (UnityEngine.Object) null)
        {
          this.SetState(FollowerTaskState.Wait);
          Debug.Log((object) (this._brain.Info.Name + " Do reactions!"));
          follower.TimedAnimation("Reactions/react-happy1", 2.1f, (System.Action) (() =>
          {
            Debug.Log((object) (this._brain.Info.Name + " Second reactions!"));
            follower.TimedAnimation("Reactions/react-enlightened2", 2.1f, (System.Action) (() =>
            {
              if (this._brain.CurrentOverrideTaskType == FollowerTaskType.SleepBedRest)
              {
                this._brain.Stats.Illness = 0.0f;
                this._brain.ClearPersonalOverrideTaskProvider();
              }
              this.End();
            }));
          }));
        }
        else
          this.End();
      }
    }
    if (this.sleepingOnFloor && this._sleepingInBed && this.State == FollowerTaskState.Doing)
    {
      this.ClearDestination();
      this.SetState(FollowerTaskState.GoingTo);
    }
    if (this._brain.HasHome)
      return;
    Dwelling.DwellingAndSlot freeDwellingAndSlot = StructureManager.GetFreeDwellingAndSlot(this.Location, this._brain._directInfoAccess);
    if (freeDwellingAndSlot == null || StructureManager.GetStructureByID<Structures_Bed>(freeDwellingAndSlot.ID).ReservedForTask)
      return;
    StructureManager.GetStructureByID<Structures_Bed>(freeDwellingAndSlot.ID).ReservedForTask = true;
    this._brain.AssignDwelling(freeDwellingAndSlot, this._brain.Info.ID, false);
    this._brain.HardSwapToTask((FollowerTask) new FollowerTask_ClaimDwelling(freeDwellingAndSlot));
  }

  public override float RestChange(float deltaGameTime)
  {
    return this.State == FollowerTaskState.Doing ? (float) (100.0 * 1.0 * ((double) deltaGameTime / 240.0)) : base.RestChange(deltaGameTime);
  }

  public override float IllnessChange(float deltaGameTime)
  {
    if (this.State != FollowerTaskState.Doing)
      return (float) (100.0 * ((double) deltaGameTime / 3600.0));
    float num = 1f;
    if (this._brain.HasTrait(FollowerTrait.TraitType.Sickly))
      num *= 0.85f;
    if (this._brain.HasTrait(FollowerTrait.TraitType.IronStomach))
      num *= 1.15f;
    return (float) -(100.0 * (double) num * ((double) deltaGameTime / 2400.0));
  }

  public void UpdateSleepChanges(float timePassed)
  {
    this._brain.Stats.Rest = Mathf.Clamp(this._brain.Stats.Rest + this.RestChange(timePassed), 0.0f, 100f);
    this._brain.Stats.Illness = Mathf.Clamp(this._brain.Stats.Illness + this.IllnessChange(timePassed), 0.0f, 100f);
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    Vector3 zero = Vector3.zero;
    Vector3 vector3;
    if (this._brain.HasHome)
    {
      Structures_Bed dwellingStructure = this._brain.GetAssignedDwellingStructure();
      vector3 = dwellingStructure.Data.Position;
      if (dwellingStructure != null && dwellingStructure.IsCollapsed)
      {
        vector3 = TownCentre.RandomPositionInCachedTownCentre();
        this.sleepingOnFloor = true;
      }
      else
      {
        Dwelling dwelling = this.FindDwelling();
        if ((UnityEngine.Object) dwelling != (UnityEngine.Object) null)
          vector3 = dwelling.GetDwellingSlotPosition(this._brain._directInfoAccess.DwellingSlot);
        this.sleepingOnFloor = false;
      }
    }
    else
    {
      this.ClosestTile = StructureManager.GetCloseTile(follower.transform.position, this._brain.Location);
      if (this.ClosestTile != null)
      {
        vector3 = this.ClosestTile.WorldPosition;
        this.ClaimReservations();
      }
      else
        vector3 = TownCentre.RandomPositionInCachedTownCentre();
      this.sleepingOnFloor = true;
    }
    return vector3;
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Sick/idle-sick");
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Sick/walk-sick");
  }

  public override void OnDoingBegin(Follower follower)
  {
    follower.ClearPath();
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    if (this._sleepingInBed)
      follower.SimpleAnimator.Animate("sleep_bedrest_justhead", 1, true, 0.0f);
    else
      follower.SimpleAnimator.AddAnimate("sleep_bedrest", 1, true, 0.0f);
    if (this._sleepingInBed)
    {
      Dwelling dwelling = this.FindDwelling();
      Dwelling.DwellingAndSlot dwellingAndSlot = this._brain.GetDwellingAndSlot();
      if ((UnityEngine.Object) dwelling != (UnityEngine.Object) null)
        dwelling.SetBedImage(dwellingAndSlot.dwellingslot, Dwelling.SlotState.IN_USE);
    }
    foreach (FollowerPet dlcFollowerPet in follower.DLCFollowerPets)
      dlcFollowerPet.Sleep();
  }

  public override void Cleanup(Follower follower)
  {
    Debug.Log((object) (this._brain.Info.Name + " Cleanup!"));
    base.Cleanup(follower);
    if (this._brain.HasHome)
    {
      Dwelling dwelling = this.FindDwelling();
      if ((UnityEngine.Object) dwelling != (UnityEngine.Object) null)
      {
        Dwelling.DwellingAndSlot dwellingAndSlot = this._brain.GetDwellingAndSlot();
        dwelling.SetBedImage(dwellingAndSlot.dwellingslot, Dwelling.SlotState.CLAIMED);
      }
    }
    foreach (FollowerPet dlcFollowerPet in follower.DLCFollowerPets)
      dlcFollowerPet.StopSleeping();
  }

  public override float ExhaustionChange(float deltaGameTime)
  {
    float num = 1f;
    if (this._sleepingInBed)
    {
      switch (this._brain.GetAssignedDwellingStructure().Data.Type)
      {
        case StructureBrain.TYPES.BED:
          num = 1f;
          break;
        case StructureBrain.TYPES.BED_2:
        case StructureBrain.TYPES.BED_3:
        case StructureBrain.TYPES.SHARED_HOUSE:
          num = 1.25f;
          break;
      }
    }
    return this.State == FollowerTaskState.Doing ? (float) (100.0 * ((double) deltaGameTime / 480.0) * (double) num * -1.0) : 0.0f;
  }

  public override void SimCleanup(SimFollower simFollower) => base.SimCleanup(simFollower);

  public Dwelling FindDwelling()
  {
    Dwelling dwelling = (Dwelling) null;
    Structures_Bed dwellingStructure = this._brain.GetAssignedDwellingStructure();
    if (dwellingStructure != null)
      dwelling = Dwelling.GetDwellingByID(dwellingStructure.Data.ID);
    return dwelling;
  }

  public override float SocialChange(float deltaGameTime) => 0.0f;

  public override float SatiationChange(float deltaGameTime) => 0.0f;

  [CompilerGenerated]
  public void \u003CTaskTick\u003Eb__29_1()
  {
    if (this._brain.CurrentOverrideTaskType == FollowerTaskType.SleepBedRest)
    {
      this._brain.Stats.Illness = 0.0f;
      this._brain.ClearPersonalOverrideTaskProvider();
    }
    this.End();
  }
}
