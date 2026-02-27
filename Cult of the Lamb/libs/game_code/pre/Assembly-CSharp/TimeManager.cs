// Decompiled with JetBrains decompiler
// Type: TimeManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI;
using MMTools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TimeManager : BaseMonoBehaviour
{
  public const int SIMULATION_FRAME_SPREAD = 3;
  public const int REAL_SECONDS_PER_DAY = 480;
  public const int GAME_HOURS_PER_PHASE = 4;
  public const int GAME_MINUTES_PER_PHASE = 240 /*0xF0*/;
  public const int GAME_HOURS_PER_DAY = 20;
  public const int GAME_MINUTES_PER_DAY = 1200;
  public static System.Action OnNewDayStarted;
  public static System.Action OnNewPhaseStarted;
  public static System.Action OnScheduleChanged;
  public static Dictionary<Vector3Int, List<LongGrass>> GrassRegions = new Dictionary<Vector3Int, List<LongGrass>>();
  public const int GrassRegionSize = 3;
  private static Vector3 Position;
  private static float _gameTimeSinceLastProgress = 0.0f;
  private static ITaskProvider _currentOverrideTaskProvider;

  public static float TotalElapsedGameTime
  {
    get => (float) (TimeManager.CurrentDay * 1200) + TimeManager.CurrentGameTime;
  }

  public static float CurrentGameTime
  {
    get => DataManager.Instance.CurrentGameTime;
    set => DataManager.Instance.CurrentGameTime = value;
  }

  public static float DeltaGameTime { get; private set; }

  public static int CurrentDay
  {
    get => DataManager.Instance.CurrentDayIndex;
    private set => DataManager.Instance.CurrentDayIndex = value;
  }

  public static DayPhase CurrentPhase
  {
    get => (DayPhase) DataManager.Instance.CurrentPhaseIndex;
    private set => DataManager.Instance.CurrentPhaseIndex = (int) value;
  }

  public static DayPhase NextPhase => (DayPhase) ((int) (TimeManager.CurrentPhase + 1) % 5);

  public static int CurrentHour { get; private set; }

  public static int CurrentMinute { get; private set; }

  public static bool IsDay
  {
    get
    {
      return TimeManager.CurrentPhase == DayPhase.Dawn || TimeManager.CurrentPhase == DayPhase.Morning || TimeManager.CurrentPhase == DayPhase.Afternoon;
    }
  }

  public static bool IsNight => TimeManager.CurrentPhase == DayPhase.Night;

  public static float CurrentPhaseProgress
  {
    get
    {
      return (float) (((double) TimeManager.CurrentGameTime - (double) ((int) TimeManager.CurrentPhase * 240 /*0xF0*/)) / 240.0);
    }
  }

  public static float CurrentDayProgress
  {
    get => (float) (((double) TimeManager.CurrentGameTime - 0.0) / 1200.0);
  }

  public static float TimeRemainingInCurrentPhase()
  {
    return (float) (240 /*0xF0*/ * (int) (TimeManager.CurrentPhase + 1)) - TimeManager.CurrentGameTime;
  }

  public static float TimeRemainingUntilPhase(DayPhase phase)
  {
    float num = (float) (240 /*0xF0*/ * (int) phase) - TimeManager.CurrentGameTime;
    if (phase <= TimeManager.CurrentPhase)
      num += 1200f;
    return num;
  }

  private static void StartNewDay()
  {
    Debug.Log((object) "NEW DAY!");
    ++TimeManager.CurrentDay;
    TimeManager.CurrentGameTime -= 1200f;
    System.Action onNewDayStarted = TimeManager.OnNewDayStarted;
    if (onNewDayStarted != null)
      onNewDayStarted();
    StructureManager.UpdateWeeds(FollowerLocation.Base);
    if (DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.Libertarian))
    {
      List<StructureBrain> structuresOfType = StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.PRISON);
      bool flag = true;
      foreach (StructureBrain structureBrain in structuresOfType)
      {
        if (structureBrain.Data.FollowerID != -1)
        {
          flag = false;
          break;
        }
      }
      if (flag)
        CultFaithManager.AddThought(Thought.Cult_Libertarian);
    }
    NotificationCentre.Instance.PlayGenericNotification("Notifications/Cult_Sermon/Notification/Ready");
  }

  private static void StartNewPhase(DayPhase phase)
  {
    if (TimeManager.GetScheduledActivity(phase) == ScheduledActivity.Sleep)
      TimeManager.SetOverrideScheduledActivity(ScheduledActivity.None);
    TimeManager.CurrentPhase = phase;
    System.Action onNewPhaseStarted = TimeManager.OnNewPhaseStarted;
    if (onNewPhaseStarted != null)
      onNewPhaseStarted();
    switch (TimeManager.CurrentPhase)
    {
      case DayPhase.Dawn:
        AudioManager.Instance.ToggleFilter(SoundParams.Night, false);
        TimeManager.StartNewDay();
        break;
      case DayPhase.Night:
        AudioManager.Instance.ToggleFilter(SoundParams.Night, true);
        break;
    }
    if (!DataManager.Instance.GameOverEnabled || DataManager.Instance.Followers.Count > 0 || DataManager.Instance.InGameOver)
      return;
    DataManager.Instance.DisplayGameOverWarning = true;
    Debug.Break();
  }

  private static void CheckNewPhase()
  {
    if ((double) TimeManager.CurrentGameTime <= (double) ((int) (TimeManager.CurrentPhase + 1) * 240 /*0xF0*/))
      return;
    TimeManager.StartNewPhase(TimeManager.NextPhase);
  }

  public static float TimeSinceLastComplaint
  {
    get => DataManager.Instance.TimeSinceLastComplaint;
    set => DataManager.Instance.TimeSinceLastComplaint = value;
  }

  public static float TimeSinceLastQuest
  {
    get => DataManager.Instance.TimeSinceLastQuest;
    set => DataManager.Instance.TimeSinceLastQuest = value;
  }

  public static bool PauseGameTime
  {
    get => DataManager.Instance.PauseGameTime;
    set => DataManager.Instance.PauseGameTime = value;
  }

  public static void AddToRegion(LongGrass l)
  {
    Vector3Int regions = TimeManager.PositionToRegions(l.Position);
    if (!TimeManager.GrassRegions.ContainsKey(regions))
      TimeManager.GrassRegions.Add(regions, new List<LongGrass>()
      {
        l
      });
    else
      TimeManager.GrassRegions[regions].Add(l);
  }

  public static void RemoveLongGrass(LongGrass l)
  {
    Vector3Int regions = TimeManager.PositionToRegions(l.Position);
    if (!TimeManager.GrassRegions.ContainsKey(regions))
      return;
    TimeManager.GrassRegions[regions].Remove(l);
    if (TimeManager.GrassRegions[regions].Count > 0)
      return;
    TimeManager.GrassRegions.Remove(regions);
  }

  private static Vector3Int PositionToRegions(Vector3 Position)
  {
    return Vector3Int.FloorToInt(Position) / 3;
  }

  private static void UpdateGrassRegions()
  {
    if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
    {
      TimeManager.Position = PlayerFarming.Instance.transform.position;
      if (TimeManager.GrassRegions.ContainsKey(TimeManager.PositionToRegions(TimeManager.Position)))
      {
        foreach (LongGrass longGrass in TimeManager.GrassRegions[TimeManager.PositionToRegions(TimeManager.Position)])
        {
          if ((Time.frameCount + longGrass.FrameIntervalOffset) % longGrass.UpdateInterval == 0 && (double) Vector3.Distance(longGrass.Position, TimeManager.Position) < 1.0)
            longGrass.Colliding(PlayerFarming.Instance.gameObject);
        }
      }
    }
    foreach (Health health in Health.team2)
    {
      if (!((UnityEngine.Object) health == (UnityEngine.Object) null))
      {
        TimeManager.Position = health.transform.position;
        if (TimeManager.GrassRegions.ContainsKey(TimeManager.PositionToRegions(TimeManager.Position)))
        {
          foreach (LongGrass longGrass in TimeManager.GrassRegions[TimeManager.PositionToRegions(TimeManager.Position)])
          {
            if ((Time.frameCount + longGrass.FrameIntervalOffset) % longGrass.UpdateInterval == 0 && (double) Vector3.Distance(longGrass.Position, TimeManager.Position) < 1.0)
              longGrass.Colliding(health.gameObject);
          }
        }
      }
    }
    foreach (Follower follower in Follower.Followers)
    {
      TimeManager.Position = follower.transform.position;
      if (TimeManager.GrassRegions.ContainsKey(TimeManager.PositionToRegions(TimeManager.Position)))
      {
        foreach (LongGrass longGrass in TimeManager.GrassRegions[TimeManager.PositionToRegions(TimeManager.Position)])
        {
          if ((Time.frameCount + longGrass.FrameIntervalOffset) % longGrass.UpdateInterval == 0 && (double) Vector3.Distance(longGrass.Position, TimeManager.Position) < 1.0)
            longGrass.Colliding(follower.gameObject);
        }
      }
    }
  }

  public static void Simulate(float deltaGameTime)
  {
    if ((double) deltaGameTime <= 0.0)
      return;
    double num1 = (double) deltaGameTime / 1200.0;
    TimeManager.DeltaGameTime = deltaGameTime;
    if (!TimeManager.PauseGameTime)
      TimeManager.CurrentGameTime += TimeManager.DeltaGameTime;
    TimeManager.CheckNewPhase();
    int currentHour = TimeManager.CurrentHour;
    TimeManager.CurrentHour = Mathf.FloorToInt(TimeManager.CurrentGameTime / 60f);
    TimeManager.CurrentMinute = Mathf.FloorToInt(TimeManager.CurrentGameTime % 60f);
    StructureEffectManager.Tick();
    if (TimeManager.CurrentHour != currentHour)
    {
      int instantActivity = (int) TimeManager.GetInstantActivity(TimeManager.CurrentHour);
    }
    if (TimeManager._currentOverrideTaskProvider != null && TimeManager._currentOverrideTaskProvider.CheckOverrideComplete())
    {
      TimeManager.ClearOverrideTaskProvider();
      Debug.Log((object) "Override Task Finished");
    }
    TimeManager.UpdateGrassRegions();
    for (int location = 0; location < 84; ++location)
    {
      List<SimFollower> simFollowerList = FollowerManager.SimFollowersAtLocation((FollowerLocation) location);
      for (int index = 0; index < simFollowerList.Count; ++index)
      {
        SimFollower simFollower = simFollowerList[index];
        if (simFollower.Brain.LeftCult)
          simFollower.Brain.Leave(simFollower.Brain.LeftCultWithReason);
      }
    }
    for (int location = 0; location < 84; ++location)
    {
      List<Follower> followerList = FollowerManager.FollowersAtLocation((FollowerLocation) location);
      for (int index = 0; index < followerList.Count; ++index)
      {
        Follower follower = followerList[index];
        if (follower.Brain.LeftCult)
          follower.Leave(follower.Brain.LeftCultWithReason);
      }
    }
    if (DataManager.Instance.dungeonRun >= 3 && TimeManager.CurrentDay >= 3 && DataManager.Instance.ShowCultFaith)
    {
      List<Follower> followerList1 = FollowerManager.FollowersAtLocation(PlayerFarming.Location);
      if (followerList1.Count > 0)
      {
        if ((double) TimeManager.TimeSinceLastQuest > 480.0 && DataManager.Instance.CurrentOnboardingFollowerID == -1 && ObjectiveManager.GetNumberOfObjectivesInGroup("Objectives/GroupTitles/Quest") < 1)
        {
          List<Follower> followerList2 = new List<Follower>();
          List<Follower> followerList3 = new List<Follower>();
          foreach (Follower follower in followerList1)
          {
            if ((UnityEngine.Object) follower != (UnityEngine.Object) null && follower.Brain != null && follower.Brain.Location == FollowerLocation.Base && !FollowerManager.FollowerLocked(follower.Brain.Info.ID) && (follower.Brain.CurrentTask == null || !follower.Brain.CurrentTask.BlockTaskChanges && follower.Brain.CurrentTaskType != FollowerTaskType.Sleep) && follower.Brain.Info.CursedState == Thought.None && follower.Brain._directInfoAccess.CurrentPlayerQuest == null)
            {
              if ((double) Vector3.Distance(follower.transform.position, PlayerFarming.Instance.transform.position) < 6.0)
                followerList2.Add(follower);
              if (Quests.GetCurrentStoryObjective(follower.Brain.Info.ID) != null)
                followerList3.Add(follower);
            }
          }
          Follower follower1 = (Follower) null;
          if (followerList3.Count > 0)
            follower1 = followerList3[UnityEngine.Random.Range(0, followerList3.Count)];
          else if (followerList2.Count > 0)
            follower1 = followerList2[UnityEngine.Random.Range(0, followerList2.Count)];
          if ((UnityEngine.Object) follower1 != (UnityEngine.Object) null)
          {
            follower1.Brain.HardSwapToTask((FollowerTask) new FollowerTask_GetAttention(Follower.ComplaintType.GiveQuest));
            TimeManager.TimeSinceLastQuest = 0.0f;
          }
        }
        TimeManager.TimeSinceLastQuest += deltaGameTime;
      }
    }
    for (int index1 = 0; index1 < 84; ++index1)
    {
      FollowerLocation location = (FollowerLocation) index1;
      int num2 = 0;
      foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
      {
        if (allBrain.HomeLocation == location && allBrain.ShouldReconsiderTask && allBrain.BeginReconsider())
          ++num2;
      }
      if (num2 > 0)
      {
        List<FollowerTask> priorityFollowerTasks1 = FollowerBrain.GetTopPriorityFollowerTasks(location);
        List<int> intList = new List<int>();
        foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
        {
          if (allBrain.HomeLocation == location)
          {
            List<FollowerTask> priorityFollowerTasks2 = FollowerBrain.GetTopPriorityFollowerTasks(ScheduledActivity.Work, location);
            foreach (int index2 in intList)
            {
              if (index2 != -1)
                priorityFollowerTasks2[index2] = (FollowerTask) null;
            }
            if (allBrain.ShouldReconsiderTask && TimeManager.GetScheduledActivity(location) == ScheduledActivity.Sleep && allBrain.Info.CursedState == Thought.None && allBrain._directInfoAccess.WorkThroughNight)
            {
              if (FollowerBrainStats.IsHoliday)
                allBrain.ClaimNextAvailableTask(new List<FollowerTask>()
                {
                  (FollowerTask) new FollowerTask_FakeLeisure()
                });
              else
                intList.Add(allBrain.ClaimNextAvailableTask(priorityFollowerTasks2));
            }
            else if (allBrain.ShouldReconsiderTask)
            {
              if (priorityFollowerTasks1.Count == 0)
                allBrain.TryClaimExistingTask(priorityFollowerTasks1);
              else
                intList.Add(allBrain.TryClaimExistingTask(priorityFollowerTasks2));
            }
          }
        }
        int num3 = 5;
        bool flag1;
        do
        {
          flag1 = false;
          --num3;
          for (int index3 = 0; index3 < priorityFollowerTasks1.Count; ++index3)
          {
            FollowerTask followerTask1 = priorityFollowerTasks1[index3];
            if (followerTask1 != null && !intList.Contains(index3))
            {
              FollowerBrain followerBrain = (FollowerBrain) null;
              PriorityCategory priorityCategory1 = PriorityCategory.ExtremelyUrgent;
              float num4 = float.MaxValue;
              foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
              {
                if (allBrain.HomeLocation == location)
                {
                  FollowerTask followerTask2 = (FollowerTask) null;
                  if (allBrain.PendingTask.Task != null)
                    followerTask2 = !allBrain.PendingTask.KeepExistingTask ? allBrain.PendingTask.Task : allBrain.CurrentTask;
                  if (followerTask2 != null)
                  {
                    if (allBrain.ShouldReconsiderTask && FollowerTask.RequiredFollowerLevel(allBrain.Info.FollowerRole, followerTask1.Type))
                    {
                      PriorityCategory priorityCategory2 = followerTask2.GetPriorityCategory(allBrain.Info.FollowerRole, allBrain.Info.WorkerPriority, allBrain);
                      bool flag2 = false;
                      if (followerTask1.GetPriorityCategory(allBrain.Info.FollowerRole, allBrain.Info.WorkerPriority, allBrain) < priorityCategory2)
                        flag2 = true;
                      else if (followerTask1.GetPriorityCategory(allBrain.Info.FollowerRole, allBrain.Info.WorkerPriority, allBrain) == priorityCategory2)
                        flag2 = (double) followerTask1.Priorty > (double) followerTask2.Priorty;
                      if (flag2)
                      {
                        if (allBrain.PendingTask.Task == null)
                        {
                          followerBrain = allBrain;
                          break;
                        }
                        if (priorityCategory2 > priorityCategory1 || priorityCategory2 == priorityCategory1 && (double) followerTask2.Priorty < (double) num4)
                        {
                          followerBrain = allBrain;
                          priorityCategory1 = priorityCategory2;
                          num4 = followerTask2.Priorty;
                        }
                      }
                    }
                  }
                  else if (allBrain.PendingTask.Task == null)
                  {
                    followerBrain = allBrain;
                    break;
                  }
                }
              }
              if (followerBrain != null)
              {
                flag1 = true;
                if (followerBrain.PendingTask.Task != null)
                  priorityFollowerTasks1[followerBrain.PendingTask.ListIndex] = followerBrain.PendingTask.Task;
                followerBrain.PendingTask.KeepExistingTask = false;
                followerBrain.PendingTask.Task = followerTask1;
                followerBrain.PendingTask.ListIndex = index3;
                priorityFollowerTasks1[index3] = (FollowerTask) null;
              }
            }
          }
        }
        while (num3 > 0 & flag1);
        foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
        {
          if (allBrain.HomeLocation == location && allBrain.ShouldReconsiderTask && allBrain.CurrentTaskType != FollowerTaskType.ManualControl)
            allBrain.EndReconsider();
        }
      }
    }
    CultFaithManager.UpdateSimulation(TimeManager.DeltaGameTime);
    HungerBar.UpdateSimulation(TimeManager.DeltaGameTime);
    IllnessBar.UpdateSimulation(TimeManager.DeltaGameTime);
    foreach (Follower locationFollower in FollowerManager.ActiveLocationFollowers())
    {
      if (Time.frameCount % 3 == locationFollower.Brain.Info.ID % 3)
        locationFollower.Tick(TimeManager.DeltaGameTime * 3f);
    }
    for (int location = 0; location < 84; ++location)
    {
      List<Follower> followerList = FollowerManager.FollowersAtLocation((FollowerLocation) location);
      for (int index = 0; index < followerList.Count; ++index)
      {
        Follower follower = followerList[index];
        if (follower.Brain.DesiredLocation != follower.Brain.Location)
        {
          followerList.RemoveAt(index--);
          if (LocationManager.GetLocationState(follower.Brain.DesiredLocation) == LocationState.Unloaded)
          {
            SimFollower simFollower = FollowerManager.FindSimFollowerByID(follower.Brain.Info.ID);
            if (simFollower == null)
              simFollower = new SimFollower(follower.Brain);
            else
              FollowerManager.SimFollowersAtLocation(follower.Brain.Location).Remove(simFollower);
            follower.Brain.Location = follower.Brain.DesiredLocation;
            FollowerManager.SimFollowersAtLocation(follower.Brain.Location).Add(simFollower);
            UnityEngine.Object.Destroy((UnityEngine.Object) follower.gameObject);
          }
          else
          {
            LocationManager locationManager;
            if (!LocationManager.LocationManagers.TryGetValue(follower.Brain.DesiredLocation, out locationManager))
            {
              Debug.LogError((object) $"No LocationManager for Location.{follower.Brain.DesiredLocation}, move failed");
              follower.Brain.DesiredLocation = follower.Brain.Location;
            }
            else
              locationManager.AddFollower(follower);
          }
        }
      }
    }
    for (int index4 = 0; index4 < 84; ++index4)
    {
      FollowerLocation location = (FollowerLocation) index4;
      List<SimFollower> simFollowerList = FollowerManager.SimFollowersAtLocation(location);
      for (int index5 = 0; index5 < simFollowerList.Count; ++index5)
      {
        SimFollower simFollower = simFollowerList[index5];
        if (simFollower.Brain.DesiredLocation != simFollower.Brain.Location)
        {
          simFollowerList.RemoveAt(index5--);
          int desiredLocation1 = (int) simFollower.Brain.DesiredLocation;
          int desiredLocation2 = (int) simFollower.Brain.DesiredLocation;
          LocationState locationState = LocationManager.GetLocationState(simFollower.Brain.DesiredLocation);
          if (locationState != LocationState.Unloaded)
            LocationManager.LocationManagers[simFollower.Brain.DesiredLocation].SpawnFollower(simFollower, locationState == LocationState.Active);
          if (locationState != LocationState.Active)
          {
            if (simFollower.Brain.DesiredLocation == FollowerLocation.None)
              throw new InvalidOperationException($"Invalid desired FollowerLocation.{location} !!");
            Debug.Log((object) $"Moving (sim) {simFollower.Brain.Info.Name} from {simFollower.Brain.Location} to {simFollower.Brain.DesiredLocation}");
            simFollower.Brain.Location = simFollower.Brain.DesiredLocation;
            FollowerManager.SimFollowersAtLocation(simFollower.Brain.DesiredLocation).Add(simFollower);
          }
        }
      }
    }
    for (int index = 0; index < 84; ++index)
    {
      FollowerLocation location = (FollowerLocation) index;
      if (LocationManager.GetLocationState(location) != LocationState.Active)
      {
        foreach (SimFollower simFollower in FollowerManager.SimFollowersAtLocation(location))
        {
          if (Time.frameCount % 3 == simFollower.Brain.Info.ID % 3)
            simFollower.Tick(TimeManager.DeltaGameTime * 3f);
        }
      }
    }
    for (int location = 0; location < 84; ++location)
    {
      List<SimFollower> simFollowerList = FollowerManager.SimFollowersAtLocation((FollowerLocation) location);
      for (int index = 0; index < simFollowerList.Count; ++index)
      {
        if (simFollowerList[index].Retired)
          simFollowerList.RemoveAt(index--);
      }
    }
    TimeManager._gameTimeSinceLastProgress += deltaGameTime;
    for (int index = 0; index < DataManager.Instance.CurrentResearch.Count; ++index)
    {
      StructuresData.ResearchObject researchObject = DataManager.Instance.CurrentResearch[index];
      researchObject.Progress += TimeManager._gameTimeSinceLastProgress;
      if ((double) researchObject.Progress >= (double) researchObject.TargetProgress)
        StructuresData.CompleteResearch(researchObject.Type);
    }
    TimeManager._gameTimeSinceLastProgress = 0.0f;
  }

  public static int GetSermonRitualCooldownRemaining(SermonsAndRituals.SermonRitualType type)
  {
    int num = SermonsAndRituals.CooldownDays(type);
    int lastUsedDayIndex = TimeManager.GetLastUsedDayIndex(type);
    return lastUsedDayIndex == -1 ? 0 : Mathf.Max(0, lastUsedDayIndex + num - DataManager.Instance.CurrentDayIndex);
  }

  private static int GetLastUsedDayIndex(SermonsAndRituals.SermonRitualType type)
  {
    TimeManager.CheckResizeCooldownArray();
    return DataManager.Instance.LastUsedSermonRitualDayIndex[(int) type];
  }

  public static void SetSermonRitualUsed(SermonsAndRituals.SermonRitualType type)
  {
    TimeManager.CheckResizeCooldownArray();
    DataManager.Instance.LastUsedSermonRitualDayIndex[(int) type] = DataManager.Instance.CurrentDayIndex;
  }

  private static void CheckResizeCooldownArray()
  {
    int length = DataManager.Instance.LastUsedSermonRitualDayIndex.Length;
    int num = 23;
    if (length > num)
      return;
    Array.Resize<int>(ref DataManager.Instance.LastUsedSermonRitualDayIndex, 23);
    for (int index = length; index < num; ++index)
      DataManager.Instance.LastUsedSermonRitualDayIndex[index] = -1;
  }

  public static void SetScheduledActivity(DayPhase phase, ScheduledActivity activity)
  {
    DataManager.Instance.ScheduledActivityIndexes[(int) phase] = (int) activity;
  }

  public static void SetOverrideScheduledActivity(ScheduledActivity activity)
  {
    DataManager.Instance.OverrideScheduledActivity = (int) activity;
    System.Action onScheduleChanged = TimeManager.OnScheduleChanged;
    if (onScheduleChanged == null)
      return;
    onScheduleChanged();
  }

  public static ScheduledActivity GetOverrideScheduledActivity()
  {
    return (ScheduledActivity) DataManager.Instance.OverrideScheduledActivity;
  }

  public static ScheduledActivity GetScheduledActivity(FollowerLocation location)
  {
    ScheduledActivity scheduledActivity = TimeManager.GetOverrideScheduledActivity();
    if (scheduledActivity == ScheduledActivity.None)
      scheduledActivity = TimeManager.GetScheduledActivity(TimeManager.CurrentPhase);
    if (!DataManager.Instance.HappinessEnabled && scheduledActivity == ScheduledActivity.Leisure)
      scheduledActivity = ScheduledActivity.Work;
    else if ((!DataManager.Instance.TeachingsEnabled || location != FollowerLocation.Base) && scheduledActivity == ScheduledActivity.Study)
      scheduledActivity = ScheduledActivity.Work;
    else if ((!DataManager.Instance.PrayerEnabled || !DataManager.Instance.PrayerOrdered) && scheduledActivity == ScheduledActivity.Pray)
      scheduledActivity = ScheduledActivity.Work;
    return scheduledActivity;
  }

  public static ScheduledActivity GetScheduledActivity(DayPhase phase)
  {
    int num = DataManager.Instance.ScheduledActivityIndexes[(int) phase];
    if (num >= 5)
      num = 0;
    ScheduledActivity scheduledActivity = (ScheduledActivity) num;
    if (!DataManager.Instance.HappinessEnabled && scheduledActivity == ScheduledActivity.Leisure)
      scheduledActivity = ScheduledActivity.Work;
    else if (!DataManager.Instance.TeachingsEnabled && scheduledActivity == ScheduledActivity.Study)
      scheduledActivity = ScheduledActivity.Work;
    return scheduledActivity;
  }

  public static void SetInstantActivity(InstantActivity activity, int hour)
  {
    DataManager.Instance.InstantActivityIndexes[(int) activity] = hour;
  }

  public static InstantActivity GetInstantActivity(int hour)
  {
    InstantActivity instantActivity = InstantActivity.None;
    for (int index = 0; index < 1; ++index)
    {
      if (DataManager.Instance.InstantActivityIndexes[index] == hour)
      {
        instantActivity = (InstantActivity) index;
        break;
      }
    }
    return instantActivity;
  }

  public static int GetInstantActivityHour(InstantActivity activity)
  {
    return DataManager.Instance.InstantActivityIndexes[(int) activity];
  }

  public static void SetOverrideTaskProvider(ITaskProvider provider)
  {
    TimeManager.ClearOverrideTaskProvider();
    TimeManager._currentOverrideTaskProvider = provider;
    System.Action onScheduleChanged = TimeManager.OnScheduleChanged;
    if (onScheduleChanged == null)
      return;
    onScheduleChanged();
  }

  private static void ClearOverrideTaskProvider()
  {
    TimeManager._currentOverrideTaskProvider = (ITaskProvider) null;
  }

  public static FollowerTask GetOverrideTask(FollowerBrain brain)
  {
    FollowerTask overrideTask1 = (FollowerTask) null;
    if (TimeManager._currentOverrideTaskProvider != null)
    {
      FollowerTask overrideTask2 = TimeManager._currentOverrideTaskProvider.GetOverrideTask(brain);
      if (overrideTask2 != null && (overrideTask2.Location == brain.HomeLocation || overrideTask2.Location == FollowerLocation.Church && brain.HomeLocation == FollowerLocation.Base))
        overrideTask1 = overrideTask2;
    }
    return overrideTask1;
  }

  public static string GetOverrideTaskString()
  {
    string overrideTaskString = "";
    if (TimeManager._currentOverrideTaskProvider != null)
      overrideTaskString = TimeManager._currentOverrideTaskProvider.GetType().ToString();
    return overrideTaskString;
  }

  public static void SkipTime(float time)
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) TimeManager.SkipTimeIE(time));
  }

  private static IEnumerator SkipTimeIE(float duration)
  {
    while (PlayerFarming.Location != FollowerLocation.Base || LetterBox.IsPlaying || MMConversation.isPlaying || SimulationManager.IsPaused || PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.Idle && PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.Moving)
      yield return (object) null;
    float targetTime = TimeManager.TotalElapsedGameTime + duration;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(TownCentre.Instance.gameObject, 20f);
    GameManager.GetInstance().StartCoroutine((IEnumerator) Delay());
    float maxTimescale = 20f;
    float timer = 0.0f;
    while ((double) TimeManager.TotalElapsedGameTime < (double) targetTime)
    {
      Time.timeScale = !MonoSingleton<UIManager>.Instance.MenusBlocked ? Mathf.Lerp(1f, maxTimescale, timer / 1f) : 0.0f;
      timer += Time.fixedDeltaTime;
      yield return (object) null;
    }
    timer = 0.0f;
    while ((double) Time.timeScale != 1.0)
    {
      Time.timeScale = !MonoSingleton<UIManager>.Instance.MenusBlocked ? Mathf.Lerp(maxTimescale, 1f, timer / 1f) : 0.0f;
      timer += Time.fixedDeltaTime;
      yield return (object) null;
    }
    GameManager.GetInstance().OnConversationEnd();
    Time.timeScale = 1f;

    static IEnumerator Delay()
    {
      yield return (object) new WaitForSeconds(1.2f);
      HUD_Manager.Instance.TimeTransitions.MoveBackInFunction();
    }
  }
}
