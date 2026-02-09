// Decompiled with JetBrains decompiler
// Type: SimFollower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class SimFollower
{
  [CompilerGenerated]
  public FollowerBrain \u003CBrain\u003Ek__BackingField;
  public float TravelTimeGameMinutes;
  [CompilerGenerated]
  public bool \u003CRetired\u003Ek__BackingField;

  public FollowerBrain Brain
  {
    get => this.\u003CBrain\u003Ek__BackingField;
    set => this.\u003CBrain\u003Ek__BackingField = value;
  }

  public bool Retired
  {
    get => this.\u003CRetired\u003Ek__BackingField;
    set => this.\u003CRetired\u003Ek__BackingField = value;
  }

  public SimFollower(FollowerBrain brain)
  {
    this.Brain = brain;
    this.Brain.OnTaskChanged += new Action<FollowerTask, FollowerTask>(this.OnTaskChanged);
    if (this.Brain.CurrentTask == null)
      return;
    this.OnTaskChanged(this.Brain.CurrentTask, (FollowerTask) null);
    this.OnFollowerTaskStateChanged(FollowerTaskState.None, this.Brain.CurrentTask.State);
  }

  public void Retire()
  {
    if (this.Retired)
      return;
    this.Retired = true;
    this.Brain.OnTaskChanged -= new Action<FollowerTask, FollowerTask>(this.OnTaskChanged);
    if (this.Brain.CurrentTask == null)
      return;
    this.Brain.CurrentTask.OnFollowerTaskStateChanged -= new FollowerTask.FollowerTaskDelegate(this.OnFollowerTaskStateChanged);
    this.Brain.CurrentTask.SimCleanup(this);
    this.OnTaskChanged((FollowerTask) null, this.Brain.CurrentTask);
  }

  public void TransitionFromFollower(Follower follower)
  {
    if (this.Brain.CurrentTask == null || this.Brain.CurrentTask.State != FollowerTaskState.GoingTo)
      return;
    this.TravelTimeGameMinutes *= this.Brain.CurrentTask.GetDistanceToDestination() / UnityEngine.Random.Range(10f, 50f);
  }

  public void OnTaskChanged(FollowerTask newTask, FollowerTask oldTask)
  {
    if (oldTask != null)
      oldTask.OnFollowerTaskStateChanged -= new FollowerTask.FollowerTaskDelegate(this.OnFollowerTaskStateChanged);
    if (newTask == null)
      return;
    newTask.SimSetup(this);
    if (oldTask == null || oldTask.Type != FollowerTaskType.ChangeLocation)
      newTask.ClaimReservations();
    newTask.OnFollowerTaskStateChanged += new FollowerTask.FollowerTaskDelegate(this.OnFollowerTaskStateChanged);
  }

  public void OnFollowerTaskStateChanged(FollowerTaskState oldState, FollowerTaskState newState)
  {
    if (this.Brain.CurrentTask == null)
      return;
    switch (oldState)
    {
      case FollowerTaskState.Idle:
        this.Brain.CurrentTask.SimIdleEnd(this);
        break;
      case FollowerTaskState.GoingTo:
        this.Brain.CurrentTask.SimGoingToEnd(this);
        break;
      case FollowerTaskState.Doing:
        this.Brain.CurrentTask.SimDoingEnd(this);
        break;
      case FollowerTaskState.Finalising:
        this.Brain.CurrentTask.SimFinaliseEnd(this);
        break;
      case FollowerTaskState.Done:
        Debug.LogError((object) "Should never change a Task state once it's Done!");
        break;
    }
    switch (newState)
    {
      case FollowerTaskState.None:
        Debug.LogError((object) "Should never change a Task state back to None!");
        break;
      case FollowerTaskState.Idle:
        this.Brain.CurrentTask.SimIdleBegin(this);
        break;
      case FollowerTaskState.GoingTo:
        this.Brain.CurrentTask.SimGoingToBegin(this);
        break;
      case FollowerTaskState.Doing:
        this.Brain.CurrentTask.SimDoingBegin(this);
        break;
      case FollowerTaskState.Finalising:
        this.Brain.CurrentTask.SimFinaliseBegin(this);
        break;
      case FollowerTaskState.Done:
        this.Brain.CurrentTask.SimCleanup(this);
        break;
    }
  }

  public void Tick(float deltaGameTime)
  {
    if (this.Retired)
      return;
    if (this.Brain._directInfoAccess.MissionaryFinished && DataManager.Instance.Followers_OnMissionary_IDs.Contains(this.Brain.Info.ID) && this.Brain.CurrentTaskType != FollowerTaskType.ChangeLocation && (this.Brain.CurrentTask == null || this.Brain.CurrentTaskType != FollowerTaskType.MissionaryComplete))
      this.Brain.HardSwapToTask((FollowerTask) new FollowerTask_MissionaryComplete());
    else if (this.Brain.Location == FollowerLocation.Missionary && DataManager.Instance.Followers_OnMissionary_IDs.Contains(this.Brain.Info.ID) && (this.Brain.CurrentTask == null || this.Brain.CurrentTaskType != FollowerTaskType.MissionaryInProgress))
      this.Brain.HardSwapToTask((FollowerTask) new FollowerTask_OnMissionary());
    else if (this.Brain.Location == FollowerLocation.LeftInTheDungeon && DataManager.Instance.Followers_LeftInTheDungeon_IDs.Contains(this.Brain.Info.ID) && (this.Brain.CurrentTask == null || this.Brain.CurrentTaskType != FollowerTaskType.LeftInTheDungeon))
      this.Brain.HardSwapToTask((FollowerTask) new FollowerTask_LeftInTheDungeon());
    else if (this.Brain.Location == FollowerLocation.Demon && (this.Brain.CurrentTask == null || this.Brain.CurrentTaskType != FollowerTaskType.IsDemon))
      this.Brain.HardSwapToTask((FollowerTask) new FollowerTask_IsDemon());
    else if (this.Brain.CurrentTaskType != FollowerTaskType.ChosenChild && this.Brain.CurrentTaskType != FollowerTaskType.GetPlayerAttention && this.Brain.CurrentTaskType != FollowerTaskType.ManualControl && this.Brain.CurrentTaskType != FollowerTaskType.LeftInTheDungeon && this.Brain.Info.ID == 100000)
      this.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ChosenChild());
    if (this.Brain.CurrentTask == null || !this.Brain.CurrentTask.BlockTaskChanges && !this.Brain.CurrentTask.BlockReactTasks)
    {
      this.Brain.SpeakersInRange = 0;
      foreach (SimFollower simFollower in FollowerManager.SimFollowersAtLocation(this.Brain.Location))
      {
        if (simFollower != this && (double) UnityEngine.Random.Range(0.0f, 1f) < 1.0 / 1000.0 && !FollowerManager.FollowerLocked(simFollower.Brain.Info.ID))
        {
          if (this.Brain.CheckForInteraction(simFollower.Brain, 0.0f))
            break;
        }
      }
      foreach (StructureBrain structureBrain in StructureManager.StructuresAtLocation(this.Brain.Location))
      {
        if ((double) UnityEngine.Random.Range(0.0f, 1f) < 1.0 / 1000.0)
        {
          if (this.Brain.CheckForSimInteraction(structureBrain))
            break;
        }
      }
      if (TimeManager.CurrentPhase != DayPhase.Night && this.Brain.Info.CursedState == Thought.None && this.Brain.Info.IsDisciple && (double) TimeManager.TotalElapsedGameTime - (double) StructureAndTime.GetOrAddTime(this.Brain.Info.ID, this.Brain, StructureAndTime.IDTypes.Follower) > 360.0 && (this.Brain.CurrentTask == null || !(this.Brain.CurrentTask is FollowerTask_ReeducateDissenter)))
      {
        foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
        {
          if (!this.Brain._directInfoAccess.FollowersReeducatedToday.Contains(allBrain.Info.ID) && allBrain.Info.CursedState == Thought.Dissenter && (double) TimeManager.TotalElapsedGameTime - (double) StructureAndTime.GetOrAddTime(allBrain.Info.ID, allBrain, StructureAndTime.IDTypes.Follower) > 360.0 && allBrain.Info.ID != 99996)
          {
            StructureAndTime.SetTime(this.Brain.Info.ID, this.Brain, StructureAndTime.IDTypes.Follower);
            StructureAndTime.SetTime(allBrain.Info.ID, allBrain, StructureAndTime.IDTypes.Follower);
            this.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ReeducateDissenter(allBrain));
            break;
          }
        }
      }
      if (this.Brain.SpeakersInRange > 0)
      {
        if (!this.Brain.ThoughtExists(Thought.PropogandaSpeakers))
          this.Brain.AddThought(Thought.PropogandaSpeakers, true);
      }
      else
        this.Brain.RemoveThought(Thought.PropogandaSpeakers, true);
    }
    this.Brain.Tick(deltaGameTime);
    FollowerTask currentTask = this.Brain.CurrentTask;
    if ((currentTask != null ? (currentTask.State == FollowerTaskState.GoingTo ? 1 : 0) : 0) == 0 || (double) this.TravelTimeGameMinutes <= 0.0)
      return;
    this.TravelTimeGameMinutes -= deltaGameTime;
    if ((double) this.TravelTimeGameMinutes > 0.0)
      return;
    this.Brain.CurrentTask.Arrive();
  }

  public void Die(NotificationCentre.NotificationType Notification, Vector3 corpseLocation)
  {
    if (this.Brain.Info.Necklace != InventoryItem.ITEM_TYPE.Necklace_Deaths_Door)
    {
      StructuresData infoByType = StructuresData.GetInfoByType(StructureBrain.TYPES.DEAD_WORSHIPPER, 0);
      infoByType.FollowerID = this.Brain.Info.ID;
      StructureManager.BuildStructure(this.Brain.Location, infoByType, corpseLocation, Vector2Int.one, callback: (Action<GameObject>) (r =>
      {
        if (!((UnityEngine.Object) PlacementRegion.Instance != (UnityEngine.Object) null))
          return;
        PlacementRegion.TileGridTile tileAtWorldPosition = PlacementRegion.Instance.GetFreeClosestTileGridTileAtWorldPosition(corpseLocation);
        if (tileAtWorldPosition == null)
          return;
        r.GetComponent<Structure>().Brain.AddToGrid(tileAtWorldPosition.Position);
      }));
    }
    this.Brain.Die(Notification);
  }
}
