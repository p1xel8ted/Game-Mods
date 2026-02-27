// Decompiled with JetBrains decompiler
// Type: FollowerTask_Sleep
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class FollowerTask_Sleep : FollowerTask
{
  public const int SLEEP_DURATION_GAME_MINUTES = 240 /*0xF0*/;
  private bool _exhausted;
  public static Action<int> OnHomelessSleep;
  public static Action<int> OnWake;
  private bool isSleeping;
  private bool targetingCollapsedBed = true;
  private bool sleepingOnFloor;
  private bool passedOut;
  private float HealingDelay;
  private float DevotionProgress;
  private float DevotionDuration = 60f;
  private PlacementRegion.TileGridTile ClosestTile;

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

  private bool _sleepingInBed
  {
    get
    {
      return this._brain.HasHome && this._brain.GetAssignedDwellingStructure() != null && !this._brain.GetAssignedDwellingStructure().IsCollapsed;
    }
  }

  public FollowerTask_Sleep(bool passedOut = false) => this.passedOut = passedOut;

  protected override int GetSubTaskCode() => 0;

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

  protected override void OnStart()
  {
    if (TimeManager.CurrentPhase != DayPhase.Night)
      this._exhausted = (double) this._brain.Stats.Exhaustion >= 100.0;
    this.SetState(FollowerTaskState.GoingTo);
  }

  protected override void OnArrive()
  {
    if (!this._brain.HasHome)
    {
      Action<int> onHomelessSleep = FollowerTask_Sleep.OnHomelessSleep;
      if (onHomelessSleep != null)
        onHomelessSleep(this._brain.Info.ID);
    }
    base.OnArrive();
  }

  protected override void OnComplete()
  {
    this.isSleeping = false;
    Action<int> onWake = FollowerTask_Sleep.OnWake;
    if (onWake == null)
      return;
    onWake(this._brain.Info.ID);
  }

  protected override void TaskTick(float deltaGameTime)
  {
    if (!this._exhausted && (double) this._brain.Stats.Exhaustion >= 100.0 && TimeManager.CurrentPhase != DayPhase.Night)
    {
      this._exhausted = true;
      if (!this.isSleeping)
        this.RecalculateDestination();
      this._exhausted = false;
    }
    if ((double) this._brain.Stats.Exhaustion <= 0.0 && TimeManager.CurrentPhase != DayPhase.Night)
    {
      this._exhausted = false;
      this.End();
    }
    if (this.sleepingOnFloor && this._sleepingInBed && this.isSleeping)
    {
      this.ClearDestination();
      this.SetState(FollowerTaskState.GoingTo);
      this.isSleeping = false;
    }
    if (!this._brain.HasHome && !this._exhausted)
    {
      Dwelling.DwellingAndSlot freeDwellingAndSlot = StructureManager.GetFreeDwellingAndSlot(this.Location, this._brain._directInfoAccess);
      if (freeDwellingAndSlot != null && !StructureManager.GetStructureByID<Structures_Bed>(freeDwellingAndSlot.ID).ReservedForTask)
      {
        StructureManager.GetStructureByID<Structures_Bed>(freeDwellingAndSlot.ID).ReservedForTask = true;
        this._brain.HardSwapToTask((FollowerTask) new FollowerTask_ClaimDwelling(freeDwellingAndSlot));
      }
    }
    if (this.sleepingOnFloor || !this._sleepingInBed || !this.isSleeping || (double) (this.DevotionProgress += deltaGameTime) <= (double) this.DevotionDuration)
      return;
    this.DevotionProgress = 0.0f;
    if (this._brain.GetAssignedDwellingStructure().Data.Type != StructureBrain.TYPES.BED_3)
      return;
    this.DepositSoul(1);
  }

  protected virtual void DepositSoul(int DevotionToGive)
  {
    this._brain.GetAssignedDwellingStructure().SoulCount += DevotionToGive;
  }

  protected override float RestChange(float deltaGameTime)
  {
    return this.State == FollowerTaskState.Doing ? (float) (100.0 * ((double) deltaGameTime / 240.0)) : base.RestChange(deltaGameTime);
  }

  protected override float ExhaustionChange(float deltaGameTime)
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
          num = 1.25f;
          break;
        case StructureBrain.TYPES.BED_3:
          num = 1.25f;
          break;
      }
    }
    return this.State == FollowerTaskState.Doing ? (float) (100.0 * ((double) deltaGameTime / 480.0) * (double) num * -1.0) : 0.0f;
  }

  protected override float SatiationChange(float deltaGameTime) => 0.0f;

  protected override Vector3 UpdateDestination(Follower follower)
  {
    Vector3 vector3;
    if (this._exhausted || this.passedOut)
      vector3 = follower.transform.position;
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
        this.sleepingOnFloor = false;
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

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    if (!this.isSleeping)
      return;
    this.SetFollowerAnimation(follower);
    if (!this._sleepingInBed)
      return;
    Dwelling dwelling = this.FindDwelling();
    Dwelling.DwellingAndSlot dwellingAndSlot = this._brain.GetDwellingAndSlot();
    if (!((UnityEngine.Object) dwelling != (UnityEngine.Object) null))
      return;
    dwelling.SetBedImage(dwellingAndSlot.dwellingslot, Dwelling.SlotState.IN_USE);
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
    if (this.targetingCollapsedBed && this._brain.GetAssignedDwellingStructure() != null && this._brain.GetAssignedDwellingStructure().IsCollapsed)
    {
      follower.TimedAnimation("Conversations/react-hate" + (object) UnityEngine.Random.Range(1, 3), 2f, (System.Action) (() =>
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
      if (this.isSleeping)
        return;
      this.isSleeping = true;
      follower.TimedAnimation("sleepy", 1f, (System.Action) (() => this.SetFollowerAnimation(follower)));
      if (!this._sleepingInBed)
        return;
      Dwelling dwelling = this.FindDwelling();
      Dwelling.DwellingAndSlot dwellingAndSlot = this._brain.GetDwellingAndSlot();
      if (!((UnityEngine.Object) dwelling != (UnityEngine.Object) null))
        return;
      dwelling.SetBedImage(dwellingAndSlot.dwellingslot, Dwelling.SlotState.IN_USE);
    }
  }

  private void SetFollowerAnimation(Follower follower)
  {
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    if (this._sleepingInBed && !this.passedOut)
      follower.SimpleAnimator.Animate("sleep_justhead", 1, true, 0.0f);
    else
      follower.SimpleAnimator.AddAnimate("sleep", 1, true, 0.0f);
    follower.IllnessAura.SetActive(follower.Brain.Info.CursedState == Thought.Ill);
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
          brainsWithinRadiu.MakeSick();
          DataManager.Instance.LastFollowerToBecomeIllFromSleepingNearIllFollower = TimeManager.TotalElapsedGameTime;
          break;
        }
      }
    }
    follower.StartCoroutine((IEnumerator) this.RandomDelay(follower));
  }

  private IEnumerator RandomDelay(Follower follower)
  {
    yield return (object) new WaitForSeconds(UnityEngine.Random.Range(0.0f, 1f));
    follower.TimedAnimation("morning", 4.7f, (System.Action) (() => base.OnFinaliseBegin(follower)));
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
    if (!(bool) (UnityEngine.Object) follower)
      return;
    follower.IllnessAura.SetActive(false);
  }

  public override void SimCleanup(SimFollower simFollower) => base.SimCleanup(simFollower);

  private Dwelling FindDwelling()
  {
    Dwelling dwelling = (Dwelling) null;
    Structures_Bed dwellingStructure = this._brain.GetAssignedDwellingStructure();
    if (dwellingStructure != null)
      dwelling = Dwelling.GetDwellingByID(dwellingStructure.Data.ID);
    return dwelling;
  }

  protected override float SocialChange(float deltaGameTime) => 0.0f;
}
