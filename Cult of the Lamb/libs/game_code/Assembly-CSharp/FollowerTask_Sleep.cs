// Decompiled with JetBrains decompiler
// Type: FollowerTask_Sleep
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerTask_Sleep : FollowerTask
{
  public const int SLEEP_DURATION_GAME_MINUTES = 240 /*0xF0*/;
  public bool _exhausted;
  public static Action<int> OnHomelessSleep;
  public static Action<int> OnWake;
  public bool isSleeping;
  public bool targetingCollapsedBed = true;
  public bool sleepingOnFloor;
  public bool passedOut;
  public bool isForced;
  public static Dictionary<PlacementRegion.TileGridTile, int> s_sleepClaims = new Dictionary<PlacementRegion.TileGridTile, int>(2048 /*0x0800*/);
  public PlacementRegion.TileGridTile _claimedTile;
  public bool hibernating;
  public float forceTimeStamp = -1f;
  public Vector3 targetSleepPosition = Vector3.zero;
  public float snorerTimestamp;
  public Follower follower;
  public float HealingDelay;
  public float DevotionProgress;
  public float DevotionDuration = 60f;
  public PlacementRegion.TileGridTile ClosestTile;
  public const float WAKE_FOLLOWER_RADIUS = 5f;
  public List<Follower> awakenFollowers = new List<Follower>();
  public bool playedNotification;

  public override FollowerTaskType Type => FollowerTaskType.Sleep;

  public override FollowerLocation Location
  {
    get
    {
      if (this._sleepingInBed)
      {
        Structures_Bed dwellingStructure = this._brain.GetAssignedDwellingStructure();
        return dwellingStructure == null ? this._brain.HomeLocation : dwellingStructure.Data.Location;
      }
      return !this._exhausted ? this._brain.HomeLocation : this._brain.Location;
    }
  }

  public override bool BlockTaskChanges
  {
    get
    {
      return this._brain != null && this._brain.Stats != null && (double) this._brain.Stats.Exhaustion > 0.0;
    }
  }

  public override bool BlockReactTasks => true;

  public override int UsingStructureID
  {
    get
    {
      int usingStructureId = 0;
      if (this._sleepingInBed)
      {
        Structures_Bed dwellingStructure = this._brain.GetAssignedDwellingStructure();
        if (dwellingStructure != null)
          usingStructureId = dwellingStructure.Data.ID;
      }
      return usingStructureId;
    }
  }

  public bool _sleepingInBed
  {
    get
    {
      return this._brain.HasHome && this._brain.GetAssignedDwellingStructure() != null && !this._brain.GetAssignedDwellingStructure().IsCollapsed && !this.sleepingInDaycare;
    }
  }

  public bool IsSleeping => this.isSleeping;

  public bool isAwake
  {
    get
    {
      if (SeasonsManager.CurrentWeatherEvent != SeasonsManager.WeatherEvent.Blizzard || this.Brain.HasTrait(FollowerTrait.TraitType.Hibernation))
        return false;
      return this.Brain.HasTrait(FollowerTrait.TraitType.Insomniac) && TimeManager.CurrentPhase == DayPhase.Night || !this.Brain.HasTrait(FollowerTrait.TraitType.Insomniac) && TimeManager.CurrentPhase != DayPhase.Night || this.Brain.Info.Necklace == InventoryItem.ITEM_TYPE.Necklace_5;
    }
  }

  public bool sleepingInDaycare
  {
    get => this._brain != null && Interaction_Daycare.IsInDaycare(this._brain.Info.ID);
  }

  public FollowerTask_Sleep(bool passedOut = false, bool forced = false, bool hibernating = false)
  {
    this.passedOut = passedOut;
    this.snorerTimestamp = TimeManager.CurrentGameTime + UnityEngine.Random.Range(60f, 120f);
    this.isForced = forced;
    if (!hibernating)
      return;
    this.hibernating = true;
    this.passedOut = true;
    if (PlayerFarming.Location == FollowerLocation.Base)
      this.targetSleepPosition = this.GetEmptyTileInRange();
    else
      this.targetSleepPosition = new Vector3(UnityEngine.Random.Range(PlacementRegion.X_Constraints.x, PlacementRegion.X_Constraints.y), UnityEngine.Random.Range(PlacementRegion.Y_Constraints.x, PlacementRegion.Y_Constraints.y), 0.0f);
  }

  public Vector3 GetEmptyTileInRange()
  {
    Vector3 zero = Vector3.zero;
    PlacementRegion.TileGridTile tileAtWorldPosition = PlacementRegion.Instance.GetClosestTileGridTileAtWorldPosition(zero);
    List<PlacementRegion.TileGridTile> tilesInRange = PlacementRegion.GetTilesInRange(tileAtWorldPosition, 15);
    int ownerId = this.OwnerId;
    FollowerTask_Sleep.Release(this._claimedTile, ownerId);
    this._claimedTile = (PlacementRegion.TileGridTile) null;
    float num = -1f;
    PlacementRegion.TileGridTile tile1 = (PlacementRegion.TileGridTile) null;
    int maxExclusive = 0;
    foreach (PlacementRegion.TileGridTile tile2 in tilesInRange)
    {
      if (!this.IsTileBlocked(tile2, ownerId))
      {
        float sqrMagnitude = (tile2.WorldPosition - zero).sqrMagnitude;
        if (tile1 == null || (double) sqrMagnitude > (double) num + 9.9999997473787516E-05)
        {
          num = sqrMagnitude;
          tile1 = tile2;
          maxExclusive = 1;
        }
        else if ((double) Mathf.Abs(sqrMagnitude - num) <= 9.9999997473787516E-05)
        {
          ++maxExclusive;
          if (UnityEngine.Random.Range(0, maxExclusive) == 0)
            tile1 = tile2;
        }
      }
    }
    if (tile1 == null)
      return tileAtWorldPosition.WorldPosition;
    FollowerTask_Sleep.Claim(tile1, ownerId);
    this._claimedTile = tile1;
    return tile1.WorldPosition;
  }

  public bool IsTileBlocked(PlacementRegion.TileGridTile tile, int ownerID)
  {
    return tile == null || tile.ObjectID != -1 || FollowerTask_Sleep.IsClaimedByOther(tile, ownerID);
  }

  public FollowerTask_Sleep(Vector3 pos)
  {
    this.passedOut = true;
    this.targetSleepPosition = pos;
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
    this._exhausted = (double) this._brain.Stats.Exhaustion >= 100.0;
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
  }

  public override void OnArrive()
  {
    if (!this._brain.HasHome)
    {
      Action<int> onHomelessSleep = FollowerTask_Sleep.OnHomelessSleep;
      if (onHomelessSleep != null)
        onHomelessSleep(this._brain.Info.ID);
    }
    base.OnArrive();
  }

  public override void OnComplete()
  {
    this.isSleeping = false;
    Action<int> onWake = FollowerTask_Sleep.OnWake;
    if (onWake == null)
      return;
    onWake(this._brain.Info.ID);
  }

  public override void TaskTick(float deltaGameTime)
  {
    if (this.isAwake)
      return;
    if (!this._exhausted && (double) this._brain.Stats.Exhaustion >= 100.0 && TimeManager.CurrentPhase != DayPhase.Night)
    {
      this._exhausted = true;
      if (!this.isSleeping)
        this.RecalculateDestination();
      this._exhausted = false;
    }
    if ((!this.Brain.HasTrait(FollowerTrait.TraitType.Hibernation) || SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter || this.Brain._directInfoAccess.WorkThroughNight) && (!this.Brain.HasTrait(FollowerTrait.TraitType.Aestivation) || SeasonsManager.CurrentSeason != SeasonsManager.Season.Spring || this.Brain._directInfoAccess.WorkThroughNight))
    {
      if ((double) this._brain.Stats.Exhaustion <= 0.0 && TimeManager.CurrentPhase != DayPhase.Night && this._brain.CurrentOverrideTaskType != FollowerTaskType.Sleep && !this.isForced && !this.Brain.HasTrait(FollowerTrait.TraitType.Insomniac) && !this.Brain.HasTrait(FollowerTrait.TraitType.Drowsy))
      {
        this._exhausted = false;
        this.End();
      }
      if (this.Brain.HasTrait(FollowerTrait.TraitType.Insomniac) && TimeManager.CurrentPhase == DayPhase.Night)
        this.End();
      if (this.isSleeping && this.isForced && (double) TimeManager.TotalElapsedGameTime - (double) this.forceTimeStamp > 480.0 && TimeManager.CurrentPhase != DayPhase.Night)
        this.End();
    }
    if (this.sleepingOnFloor && this._sleepingInBed && this.isSleeping)
    {
      this.ClearDestination();
      this.SetState(FollowerTaskState.GoingTo);
      this.isSleeping = false;
    }
    if (!this._brain.HasHome && !this._exhausted && !this.sleepingInDaycare)
    {
      Dwelling.DwellingAndSlot freeDwellingAndSlot = StructureManager.GetFreeDwellingAndSlot(this.Location, this._brain._directInfoAccess);
      if (freeDwellingAndSlot != null && !StructureManager.GetStructureByID<Structures_Bed>(freeDwellingAndSlot.ID).ReservedForTask)
      {
        StructureManager.GetStructureByID<Structures_Bed>(freeDwellingAndSlot.ID).ReservedForTask = true;
        this._brain.AssignDwelling(freeDwellingAndSlot, this._brain.Info.ID, false);
        this._brain.HardSwapToTask((FollowerTask) new FollowerTask_ClaimDwelling(freeDwellingAndSlot));
      }
    }
    if (!this.sleepingOnFloor && this._sleepingInBed && this.isSleeping && (double) (this.DevotionProgress += deltaGameTime) > (double) this.DevotionDuration)
    {
      this.DevotionProgress = 0.0f;
      if (this._brain.GetAssignedDwellingStructure().Data.Type == StructureBrain.TYPES.BED_3)
        this.DepositSoul(1);
    }
    if (PlayerFarming.Location == FollowerLocation.Base && this._brain.HasTrait(FollowerTrait.TraitType.Snorer) && (double) TimeManager.CurrentGameTime > (double) this.snorerTimestamp && (double) this.snorerTimestamp != -1.0 && this.State == FollowerTaskState.Doing)
      this.SnorerWakeFollowers();
    if (!((UnityEngine.Object) this.follower != (UnityEngine.Object) null) || this.State != FollowerTaskState.Doing || !this._sleepingInBed || !(this.Brain.LastPosition != this.UpdateDestination(this.follower)))
      return;
    this.follower.transform.position = this.UpdateDestination(this.follower);
  }

  public virtual void DepositSoul(int DevotionToGive)
  {
    this._brain.GetAssignedDwellingStructure().SoulCount += DevotionToGive;
  }

  public override float RestChange(float deltaGameTime)
  {
    return this.State == FollowerTaskState.Doing ? (float) (100.0 * ((double) deltaGameTime / 240.0)) : base.RestChange(deltaGameTime);
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

  public override float DrunkChange(float deltaGameTime)
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

  public override float SatiationChange(float deltaGameTime) => 0.0f;

  public void UpdateSleepChanges(float timePassed)
  {
    this._brain.Stats.Rest = Mathf.Clamp(this._brain.Stats.Rest + this.RestChange(timePassed), 0.0f, 100f);
    this._brain.Stats.Exhaustion = Mathf.Clamp(this._brain.Stats.Exhaustion + this.ExhaustionChange(timePassed), 0.0f, 100f);
    this._brain.Stats.Drunk = Mathf.Clamp(this._brain.Stats.Drunk + this.DrunkChange(timePassed), 0.0f, 100f);
    this._brain.Stats.Satiation = Mathf.Clamp(this._brain.Stats.Satiation + this.SatiationChange(timePassed), 0.0f, 100f);
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    Vector3 vector3;
    if (this._exhausted || this.passedOut || this.sleepingInDaycare || this.Brain != null && this.Brain.HasTrait(FollowerTrait.TraitType.Drowsy))
      vector3 = this.targetSleepPosition == Vector3.zero ? follower.transform.position : this.targetSleepPosition;
    else if (this._brain.HasHome)
    {
      Structures_Bed dwellingStructure = this._brain.GetAssignedDwellingStructure();
      vector3 = dwellingStructure != null ? dwellingStructure.Data.Position : TownCentre.RandomPositionInCachedTownCentre();
      if (dwellingStructure != null && dwellingStructure.IsCollapsed)
      {
        if (!this.targetingCollapsedBed)
        {
          vector3 = TownCentre.RandomPositionInCachedTownCentre();
          this.sleepingOnFloor = true;
        }
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
      if ((UnityEngine.Object) follower != (UnityEngine.Object) null)
      {
        this.ClosestTile = StructureManager.GetCloseTile(follower.transform.position, this._brain.Location);
        if (this.ClosestTile != null)
        {
          vector3 = this.ClosestTile.WorldPosition;
          this.ClaimReservations();
        }
        else
          vector3 = TownCentre.RandomPositionInCachedTownCentre();
      }
      else
        vector3 = TownCentre.RandomPositionInCachedTownCentre();
      this.sleepingOnFloor = true;
    }
    return vector3;
  }

  public override void OnNewPhaseStarted()
  {
    base.OnNewPhaseStarted();
    if (PlayerFarming.Location != FollowerLocation.Base || this.State != FollowerTaskState.Doing)
      return;
    Follower followerById = FollowerManager.FindFollowerByID(this.Brain.Info.ID);
    if (!((UnityEngine.Object) followerById != (UnityEngine.Object) null))
      return;
    this.SetFollowerAnimation(followerById);
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    if (this.isSleeping)
    {
      this.SetFollowerAnimation(follower);
      if (this._sleepingInBed)
      {
        Dwelling dwelling = this.FindDwelling();
        Dwelling.DwellingAndSlot dwellingAndSlot = this._brain.GetDwellingAndSlot();
        if ((UnityEngine.Object) dwelling != (UnityEngine.Object) null)
          dwelling.SetBedImage(dwellingAndSlot.dwellingslot, Dwelling.SlotState.IN_USE);
      }
    }
    if (Interaction_Daycare.IsInDaycare(follower.Brain.Info.ID))
      follower.Interaction_FollowerInteraction.Interactable = false;
    this.follower = follower;
  }

  public override void SimDoingBegin(SimFollower simFollower)
  {
    base.SimDoingBegin(simFollower);
    if (this.isSleeping)
      return;
    this.isSleeping = true;
  }

  public override void OnDoingBegin(Follower follower)
  {
    base.OnDoingBegin(follower);
    follower.OverridingEmotions = false;
    follower.ClearPath();
    if (this.targetingCollapsedBed && this._brain.GetAssignedDwellingStructure() != null && this._brain.GetAssignedDwellingStructure().IsCollapsed)
    {
      follower.TimedAnimation("Conversations/react-hate" + UnityEngine.Random.Range(1, 3).ToString(), 2f, (System.Action) (() =>
      {
        this.ClearDestination();
        this.targetingCollapsedBed = false;
        this.SetState(FollowerTaskState.GoingTo);
      }));
    }
    else
    {
      if (this.isSleeping)
        this.SetFollowerAnimation(follower);
      if (!this.isSleeping)
      {
        this.isSleeping = true;
        if (this.isAwake)
        {
          this.SetFollowerAnimation(follower);
        }
        else
        {
          string animation = "sleepy";
          if (this.hibernating)
            animation = "Hibernation/start" + (this.Brain.HasTrait(FollowerTrait.TraitType.Aestivation) ? "_summer" : "");
          follower.TimedAnimation(animation, 1f, (System.Action) (() => this.SetFollowerAnimation(follower)));
        }
        if (this._sleepingInBed)
        {
          Dwelling dwelling = this.FindDwelling();
          Dwelling.DwellingAndSlot dwellingAndSlot = this._brain.GetDwellingAndSlot();
          if ((UnityEngine.Object) dwelling != (UnityEngine.Object) null)
            dwelling.SetBedImage(dwellingAndSlot.dwellingslot, Dwelling.SlotState.IN_USE);
        }
        if (this.isForced)
          this.forceTimeStamp = TimeManager.TotalElapsedGameTime;
      }
      foreach (FollowerPet dlcFollowerPet in follower.DLCFollowerPets)
        dlcFollowerPet.Sleep();
      follower.InsomniacIcon.gameObject.SetActive(follower.Brain.HasTrait(FollowerTrait.TraitType.Insomniac));
      follower.DrowsyIcon.gameObject.SetActive(follower.Brain.HasTrait(FollowerTrait.TraitType.Drowsy));
      follower.HibernationIcon.gameObject.SetActive(follower.Brain.HasTrait(FollowerTrait.TraitType.Hibernation));
      follower.AestivationIcon.gameObject.SetActive(follower.Brain.HasTrait(FollowerTrait.TraitType.Aestivation));
    }
  }

  public override void OnEnd()
  {
    if (SeasonsManager.CurrentWeatherEvent == SeasonsManager.WeatherEvent.Blizzard && !this._brain.CanWork)
      this.SetState(FollowerTaskState.GoingTo);
    else
      base.OnEnd();
    FollowerTask_Sleep.Release(this._claimedTile, this.OwnerId);
    this._claimedTile = (PlacementRegion.TileGridTile) null;
  }

  public void SetFollowerAnimation(Follower follower)
  {
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    if (this.Brain.HasTrait(FollowerTrait.TraitType.Hibernation) && SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter || this.Brain.HasTrait(FollowerTrait.TraitType.Aestivation) && SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter)
      follower.SimpleAnimator.Animate("Hibernation/sleep" + (this.Brain.HasTrait(FollowerTrait.TraitType.Aestivation) ? "_summer" : ""), 1, true, 0.0f);
    else if (this._sleepingInBed && !this.passedOut)
    {
      if (SeasonsManager.CurrentWeatherEvent == SeasonsManager.WeatherEvent.Blizzard && this.isAwake)
        follower.SimpleAnimator.Animate("Snow/sleep_justhead_cold", 1, true, 0.0f);
      else
        follower.SimpleAnimator.Animate(follower.Brain.HasTrait(FollowerTrait.TraitType.Snorer) ? "sleep_justhead_snore" : "sleep_justhead", 1, true, 0.0f);
    }
    else
      follower.SimpleAnimator.AddAnimate(follower.Brain.HasTrait(FollowerTrait.TraitType.Snorer) ? "sleep_snore" : "sleep", 1, true, 0.0f);
  }

  public override void OnFinaliseBegin(Follower follower)
  {
    if (!this._brain.HasHome)
    {
      ++this._brain._directInfoAccess.DaysSleptOutside;
      if (this._brain._directInfoAccess.DaysSleptOutside > 3)
        DataManager.Instance.OnboardedBuildingHouse = false;
    }
    else
      this._brain._directInfoAccess.DaysSleptOutside = 0;
    if (this._brain.Info.CursedState == Thought.Ill && (double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastFollowerToBecomeIllFromSleepingNearIllFollower > 2400.0 / (double) DifficultyManager.GetTimeBetweenIllnessMultiplier())
    {
      foreach (FollowerBrain brainsWithinRadiu in FollowerBrain.GetBrainsWithinRadius(this._brain.LastPosition, 3f))
      {
        if (brainsWithinRadiu != this._brain && brainsWithinRadiu.Info.CursedState == Thought.None)
        {
          DataManager.Instance.LastFollowerToBecomeIllFromSleepingNearIllFollower = TimeManager.TotalElapsedGameTime;
          brainsWithinRadiu.MakeSick();
          break;
        }
      }
    }
    follower.StartCoroutine((IEnumerator) this.RandomDelay(follower));
    follower.InsomniacIcon.gameObject.SetActive(false);
    follower.HibernationIcon.gameObject.SetActive(false);
    follower.AestivationIcon.gameObject.SetActive(false);
    follower.DrowsyIcon.gameObject.SetActive(false);
  }

  public IEnumerator RandomDelay(Follower follower)
  {
    FollowerTask_Sleep followerTaskSleep = this;
    yield return (object) new WaitForSeconds(UnityEngine.Random.Range(0.0f, 1f));
    if (FollowerManager.FollowerLocked(followerTaskSleep._brain.Info.ID))
    {
      followerTaskSleep.\u003C\u003En__0(follower);
    }
    else
    {
      string animation = "morning";
      float timer = 4.7f;
      if (followerTaskSleep.hibernating)
        animation = "Hibernation/morning" + (followerTaskSleep.Brain.HasTrait(FollowerTrait.TraitType.Aestivation) ? "_summer" : "");
      else if (followerTaskSleep._brain.GetAssignedDwellingStructure() != null && followerTaskSleep._brain.GetAssignedDwellingStructure().IsCollapsed)
      {
        animation = "Activities/activity-badsleep";
        timer = 4f;
      }
      follower.TimedAnimation(animation, timer, (System.Action) (() => this.\u003C\u003En__0(follower)));
    }
  }

  public override void Cleanup(Follower follower)
  {
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
    if ((bool) (UnityEngine.Object) follower)
    {
      follower.InsomniacIcon.gameObject.SetActive(false);
      follower.DrowsyIcon.gameObject.SetActive(false);
      follower.AestivationIcon.gameObject.SetActive(false);
      follower.HibernationIcon.gameObject.SetActive(false);
      follower.IllnessAura.SetActive(false);
      follower.OverridingEmotions = false;
      follower.SetEmotionAnimation();
      foreach (FollowerPet dlcFollowerPet in follower.DLCFollowerPets)
        dlcFollowerPet.StopSleeping();
      if (this.hibernating && (follower.State.CURRENT_STATE == StateMachine.State.CustomAnimation || follower.State.CURRENT_STATE == StateMachine.State.TimedAction))
        follower.State.CURRENT_STATE = StateMachine.State.Idle;
    }
    FollowerTask_Sleep.Release(this._claimedTile, this.OwnerId);
    this._claimedTile = (PlacementRegion.TileGridTile) null;
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

  public void SnorerWakeFollowers()
  {
    this.snorerTimestamp = TimeManager.CurrentGameTime + UnityEngine.Random.Range(36f, 60f);
    List<Follower> followerList = new List<Follower>();
    float num = 5f;
    foreach (Follower follower in Follower.Followers)
    {
      if ((double) Vector3.Distance(follower.transform.position, this.Brain.LastPosition) <= (double) num && follower.Brain.CurrentTaskType == FollowerTaskType.Sleep && follower.Brain.CurrentTask.State == FollowerTaskState.Doing && follower.Brain.Info.CursedState == Thought.None && follower.Brain.Info.ID != this._brain.Info.ID && !this.awakenFollowers.Contains(follower) && !follower.Brain.HasTrait(FollowerTrait.TraitType.DeepSleeper))
        followerList.Add(follower);
    }
    if (followerList.Count <= 0)
      return;
    Follower follower1 = followerList[UnityEngine.Random.Range(0, followerList.Count)];
    if (!this.playedNotification)
      NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("Notifications/SnorerWokeFollowers", this._brain.Info.Name);
    this.playedNotification = true;
    follower1.StartCoroutine((IEnumerator) this.WakeUpFollowerAnnoyed(follower1));
  }

  public IEnumerator WakeUpFollowerAnnoyed(Follower follower)
  {
    FollowerTask_Sleep followerTaskSleep = this;
    if (followerTaskSleep._sleepingInBed && !followerTaskSleep.passedOut)
    {
      double num1 = (double) follower.SetBodyAnimation("sleep_justhead_annoyed", true);
    }
    else
    {
      double num2 = (double) follower.SetBodyAnimation("sleep_annoyed", true);
    }
    yield return (object) new WaitForSeconds(UnityEngine.Random.Range(3f, 12f));
    if (SeasonsManager.CurrentWeatherEvent == SeasonsManager.WeatherEvent.Blizzard && !followerTaskSleep._brain.CanWork)
    {
      follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_Sleep());
    }
    else
    {
      string previousAnim = follower.Spine.AnimationState.GetCurrent(1).Animation.Name;
      follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
      follower.FacePosition(followerTaskSleep.Brain.LastPosition);
      follower.TimedAnimation("tantrum", 3.2f);
      follower.OverridingEmotions = true;
      follower.SetFaceAnimation("Emotions/emotion-angry", true);
      yield return (object) new WaitForSeconds(0.5f);
      follower.WorshipperBubble.gameObject.SetActive(true);
      follower.WorshipperBubble.Play(WorshipperBubble.SPEECH_TYPE.ENEMIES);
      yield return (object) new WaitForSeconds(2.7f);
      double num3 = (double) follower.SetBodyAnimation("sleepy", false);
      follower.AddBodyAnimation(previousAnim, true, 0.0f);
      LayerMask layerMask = (LayerMask) ((int) new LayerMask() | 1 << LayerMask.NameToLayer("Island"));
      Vector3 vector3;
      do
      {
        Vector2 direction = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(0.0f, -1f));
        vector3 = (Vector3) (Physics2D.Raycast((Vector2) Vector3.zero, direction, 1000f, (int) layerMask).point + -direction * UnityEngine.Random.Range(2f, 4f));
      }
      while ((double) Vector3.Distance(vector3, followerTaskSleep._brain.LastPosition) < 5.0);
      follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_Sleep(vector3));
    }
  }

  public int OwnerId
  {
    get
    {
      return this._brain == null || this._brain.Info == null ? this.GetHashCode() : this._brain.Info.ID;
    }
  }

  public static bool IsClaimedByOther(PlacementRegion.TileGridTile tile, int ownerId)
  {
    int num;
    return tile != null && FollowerTask_Sleep.s_sleepClaims.TryGetValue(tile, out num) && num != ownerId;
  }

  public static void Claim(PlacementRegion.TileGridTile tile, int ownerId)
  {
    if (tile == null)
      return;
    FollowerTask_Sleep.s_sleepClaims[tile] = ownerId;
  }

  public static void Release(PlacementRegion.TileGridTile tile, int ownerId)
  {
    int num;
    if (tile == null || !FollowerTask_Sleep.s_sleepClaims.TryGetValue(tile, out num) || num != ownerId)
      return;
    FollowerTask_Sleep.s_sleepClaims.Remove(tile);
  }

  [CompilerGenerated]
  [DebuggerHidden]
  public void \u003C\u003En__0(Follower follower) => base.OnFinaliseBegin(follower);
}
