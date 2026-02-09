// Decompiled with JetBrains decompiler
// Type: TimeManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using MMTools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class TimeManager : BaseMonoBehaviour
{
  public static int SIMULATION_FRAME_SPREAD = 30;
  public const int REAL_SECONDS_PER_DAY = 480;
  public const int GAME_HOURS_PER_PHASE = 4;
  public const int GAME_MINUTES_PER_PHASE = 240 /*0xF0*/;
  public const int GAME_HOURS_PER_DAY = 20;
  public const int GAME_MINUTES_PER_DAY = 1200;
  public static System.Action OnNewDayStarted;
  public static System.Action OnNewPhaseStarted;
  public static System.Action OnScheduleChanged;
  public const float PlayerHungerDepletion = 0.05f;
  public const float PlayerSleepDepletion = 0.035f;
  public const float PlayerDungeonDepletionMultiplier = 0.75f;
  public const int MaxSurvivalTallies = 3;
  public static int SurvivalTallies = 3;
  public static System.Action OnSurvivalLossTally;
  public static float SurvivalDamagedTimer;
  public const float SurvivalDamagedDuration = 20f;
  [CompilerGenerated]
  public static float \u003CDeltaGameTime\u003Ek__BackingField;
  [CompilerGenerated]
  public static int \u003CCurrentHour\u003Ek__BackingField;
  [CompilerGenerated]
  public static int \u003CCurrentMinute\u003Ek__BackingField;
  public static Dictionary<Vector3Int, List<LongGrass>> GrassRegions = new Dictionary<Vector3Int, List<LongGrass>>();
  public static Dictionary<Vector3Int, List<SwayInWind>> WheatRegions = new Dictionary<Vector3Int, List<SwayInWind>>();
  public const int GrassRegionSize = 3;
  public static Vector3 Position;
  public static float _gameTimeSinceLastProgress = 0.0f;
  public const float Hunger_SurvivalNotificationThreshold = 10f;
  public const float Sleep_SurvivalNotificationThreshold = 10f;
  public static ITaskProvider _currentOverrideTaskProvider;

  public static float TotalElapsedGameTime
  {
    get => (float) (TimeManager.CurrentDay * 1200) + TimeManager.CurrentGameTime;
  }

  public static float CurrentGameTime
  {
    get => DataManager.Instance.CurrentGameTime;
    set => DataManager.Instance.CurrentGameTime = value;
  }

  public static float DeltaGameTime
  {
    get => TimeManager.\u003CDeltaGameTime\u003Ek__BackingField;
    set => TimeManager.\u003CDeltaGameTime\u003Ek__BackingField = value;
  }

  public static int CurrentDay
  {
    get => DataManager.Instance.CurrentDayIndex;
    set => DataManager.Instance.CurrentDayIndex = value;
  }

  public static DayPhase CurrentPhase
  {
    get => (DayPhase) DataManager.Instance.CurrentPhaseIndex;
    set => DataManager.Instance.CurrentPhaseIndex = (int) value;
  }

  public static DayPhase NextPhase => (DayPhase) ((int) (TimeManager.CurrentPhase + 1) % 5);

  public static int CurrentHour
  {
    get => TimeManager.\u003CCurrentHour\u003Ek__BackingField;
    set => TimeManager.\u003CCurrentHour\u003Ek__BackingField = value;
  }

  public static int CurrentMinute
  {
    get => TimeManager.\u003CCurrentMinute\u003Ek__BackingField;
    set => TimeManager.\u003CCurrentMinute\u003Ek__BackingField = value;
  }

  public static bool IsDay
  {
    get
    {
      return TimeManager.CurrentPhase == DayPhase.Dawn || TimeManager.CurrentPhase == DayPhase.Morning || TimeManager.CurrentPhase == DayPhase.Afternoon;
    }
  }

  public static bool IsNight => TimeManager.CurrentPhase == DayPhase.Night;

  public static int GetCurrentDayLength() => !TimeManager.IsLongNight ? 1200 : 1440;

  public static float CurrentPhaseProgress
  {
    get
    {
      double currentGameTime = (double) TimeManager.CurrentGameTime;
      int num1 = 240 /*0xF0*/;
      if (TimeManager.IsLongNight && TimeManager.CurrentPhase == DayPhase.Night)
        num1 *= 2;
      double num2 = (double) ((int) TimeManager.CurrentPhase * 240 /*0xF0*/);
      return (float) (currentGameTime - num2) / (float) num1;
    }
  }

  public static float CurrentDayProgress
  {
    get
    {
      double currentGameTime = (double) TimeManager.CurrentGameTime;
      int num1 = 1200;
      int num2 = 480;
      if (TimeManager.IsLongNight)
      {
        num1 += 240 /*0xF0*/;
        num2 += 96 /*0x60*/;
      }
      double num3 = (double) (num2 / num1);
      return (float) (currentGameTime - num3) / (float) num1;
    }
  }

  public static float TimeRemainingInCurrentPhase()
  {
    return (float) (240 /*0xF0*/ * (int) (TimeManager.CurrentPhase + 1)) - TimeManager.CurrentGameTime;
  }

  public static float TimeRemainingUntilPhase(DayPhase phase)
  {
    float num = (float) (240 /*0xF0*/ * (int) phase) - TimeManager.CurrentGameTime;
    if (phase <= TimeManager.CurrentPhase)
    {
      num += 1200f;
      if (TimeManager.IsLongNight)
        num += 240f;
    }
    return num;
  }

  public static bool IsLongNight
  {
    get
    {
      return SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter && DataManager.Instance.LongNightActive;
    }
  }

  public static void StartNewDay()
  {
    Debug.Log((object) "NEW DAY!");
    ++TimeManager.CurrentDay;
    TimeManager.CurrentGameTime -= 1200f;
    if (TimeManager.CurrentPhase == DayPhase.Dawn && TimeManager.IsLongNight)
      TimeManager.CurrentGameTime -= 240f;
    System.Action onNewDayStarted = TimeManager.OnNewDayStarted;
    if (onNewDayStarted != null)
      onNewDayStarted();
    DataManager.Instance.EndlessModeOnCooldown = false;
    DataManager.Instance.EndlessModeSinOncooldown = false;
    DataManager.Instance.FollowersPlayedKnucklebonesToday.Clear();
    DataManager.Instance.FishCaughtInsideWhaleToday = 0;
    string str1 = "0";
    string str2 = "0";
    string str3 = "0";
    string str4 = "0";
    foreach (InventoryItem inventoryItem in Inventory.items)
    {
      if (inventoryItem.type == 172)
        str1 = inventoryItem.quantity.ToString();
      else if (inventoryItem.type == 139)
        str2 = inventoryItem.quantity.ToString();
      else if (inventoryItem.type == 230)
        str3 = inventoryItem.quantity.ToString();
      else if (inventoryItem.type == 165)
        str3 = inventoryItem.quantity.ToString();
    }
    AnalyticsLogger.LogEvent(AnalyticsLogger.EventType.NewDay, str1, str2, str3, str4);
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
    if (DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.EmbraceRot))
    {
      for (int index = 0; index < DataManager.Instance.Followers.Count; ++index)
      {
        if (DataManager.Instance.Followers[index].Traits.Contains(FollowerTrait.TraitType.Mutated))
        {
          CultFaithManager.AddThought(Thought.Cult_RotFollowersInCult);
          break;
        }
      }
    }
    if (!((UnityEngine.Object) NotificationCentre.Instance != (UnityEngine.Object) null))
      return;
    NotificationCentre.Instance.PlayGenericNotification("Notifications/Cult_Sermon/Notification/Ready");
  }

  public static void StartNewPhase(DayPhase phase)
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
  }

  public static void CheckNewPhase()
  {
    int num = (int) (TimeManager.CurrentPhase + 1) * 240 /*0xF0*/;
    if (TimeManager.IsLongNight && TimeManager.CurrentPhase == DayPhase.Night)
      num += 240 /*0xF0*/;
    if ((double) TimeManager.CurrentGameTime <= (double) num)
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

  public static void AddToRegion(SwayInWind w)
  {
    Vector3Int regions = TimeManager.PositionToRegions(w.Position);
    List<SwayInWind> swayInWindList;
    if (!TimeManager.WheatRegions.TryGetValue(regions, out swayInWindList))
    {
      TimeManager.WheatRegions[regions] = new List<SwayInWind>()
      {
        w
      };
    }
    else
    {
      if (swayInWindList.Contains(w))
        return;
      swayInWindList.Add(w);
    }
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

  public static void RemoveWheat(SwayInWind w)
  {
    Vector3Int regions = TimeManager.PositionToRegions(w.Position);
    List<SwayInWind> swayInWindList;
    if (!TimeManager.WheatRegions.TryGetValue(regions, out swayInWindList))
      return;
    swayInWindList.Remove(w);
    if (swayInWindList.Count != 0)
      return;
    TimeManager.WheatRegions.Remove(regions);
  }

  public static Vector3Int PositionToRegions(Vector3 Position)
  {
    return Vector3Int.FloorToInt(Position) / 3;
  }

  public static void UpdateGrassRegions()
  {
    if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
    {
      for (int index = 0; index < PlayerFarming.players.Count; ++index)
      {
        PlayerFarming player = PlayerFarming.players[index];
        if ((UnityEngine.Object) player != (UnityEngine.Object) null)
        {
          TimeManager.Position = player.transform.position;
          if (TimeManager.GrassRegions.ContainsKey(TimeManager.PositionToRegions(TimeManager.Position)))
          {
            foreach (LongGrass longGrass in TimeManager.GrassRegions[TimeManager.PositionToRegions(TimeManager.Position)])
            {
              if ((Time.frameCount + longGrass.FrameIntervalOffset) % longGrass.UpdateInterval == 0 && (double) Vector3.Distance(longGrass.Position, TimeManager.Position) < 1.0)
                longGrass.Colliding(player.gameObject);
            }
          }
          if (TimeManager.WheatRegions.ContainsKey(TimeManager.PositionToRegions(TimeManager.Position)))
          {
            foreach (SwayInWind swayInWind in TimeManager.WheatRegions[TimeManager.PositionToRegions(TimeManager.Position)])
            {
              if ((Time.frameCount + swayInWind.FrameIntervalOffset) % swayInWind.UpdateInterval == 0 && (double) Vector3.Distance(swayInWind.Position, TimeManager.Position) < 1.0)
                swayInWind.Colliding(player.gameObject);
            }
          }
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
        if (TimeManager.WheatRegions.ContainsKey(TimeManager.PositionToRegions(TimeManager.Position)))
        {
          foreach (SwayInWind swayInWind in TimeManager.WheatRegions[TimeManager.PositionToRegions(TimeManager.Position)])
          {
            if ((Time.frameCount + swayInWind.FrameIntervalOffset) % swayInWind.UpdateInterval == 0 && (double) Vector3.Distance(swayInWind.Position, TimeManager.Position) < 1.0)
              swayInWind.Colliding(health.gameObject);
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
      if (TimeManager.WheatRegions.ContainsKey(TimeManager.PositionToRegions(TimeManager.Position)))
      {
        foreach (SwayInWind swayInWind in TimeManager.WheatRegions[TimeManager.PositionToRegions(TimeManager.Position)])
        {
          if ((Time.frameCount + swayInWind.FrameIntervalOffset) % swayInWind.UpdateInterval == 0 && (double) Vector3.Distance(swayInWind.Position, TimeManager.Position) < 1.0)
            swayInWind.Colliding(follower.gameObject);
        }
      }
    }
    if (!((UnityEngine.Object) WoolhavenCritterManager.instance != (UnityEngine.Object) null) || !WoolhavenCritterManager.instance.enabled)
      return;
    foreach (CritterRat spawnedCritter in WoolhavenCritterManager.instance.spawnedCritters)
    {
      if (!((UnityEngine.Object) spawnedCritter == (UnityEngine.Object) null))
      {
        TimeManager.Position = spawnedCritter.transform.position;
        if (TimeManager.GrassRegions.ContainsKey(TimeManager.PositionToRegions(TimeManager.Position)))
        {
          foreach (LongGrass longGrass in TimeManager.GrassRegions[TimeManager.PositionToRegions(TimeManager.Position)])
          {
            if ((Time.frameCount + longGrass.FrameIntervalOffset) % longGrass.UpdateInterval == 0 && (double) Vector3.Distance(longGrass.Position, TimeManager.Position) < 1.0)
              longGrass.Colliding(spawnedCritter.gameObject);
          }
        }
        if (TimeManager.WheatRegions.ContainsKey(TimeManager.PositionToRegions(TimeManager.Position)))
        {
          foreach (SwayInWind swayInWind in TimeManager.WheatRegions[TimeManager.PositionToRegions(TimeManager.Position)])
          {
            if ((Time.frameCount + swayInWind.FrameIntervalOffset) % swayInWind.UpdateInterval == 0 && (double) Vector3.Distance(swayInWind.Position, TimeManager.Position) < 1.0)
              swayInWind.Colliding(spawnedCritter.gameObject);
          }
        }
      }
    }
  }

  public static void Simulate(float deltaGameTime, bool skippingTime = false)
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
    SeasonsManager.Update(TimeManager.DeltaGameTime);
    TimeManager.UpdateGrassRegions();
    for (int index1 = 0; index1 < 98; ++index1)
    {
      FollowerLocation location = (FollowerLocation) index1;
      List<SimFollower> simFollowerList = FollowerManager.SimFollowersAtLocation(location);
      for (int index2 = 0; index2 < simFollowerList.Count; ++index2)
      {
        SimFollower simFollower = simFollowerList[index2];
        if (simFollower.Brain.LeftCult)
        {
          Follower followerById = FollowerManager.FindFollowerByID(simFollower.Brain.Info.ID);
          if ((bool) (UnityEngine.Object) followerById && (bool) (UnityEngine.Object) followerById.gameObject)
            followerById.Leave(followerById.Brain.LeftCultWithReason);
          else
            simFollower.Brain.Leave(simFollower.Brain.LeftCultWithReason);
        }
      }
      List<Follower> followerList = FollowerManager.FollowersAtLocation(location);
      for (int index3 = 0; index3 < followerList.Count; ++index3)
      {
        Follower follower = followerList[index3];
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
            if ((UnityEngine.Object) follower != (UnityEngine.Object) null && follower.Brain != null && follower.Brain.Location == FollowerLocation.Base && !FollowerManager.FollowerLocked(follower.Brain.Info.ID) && !follower.Brain._directInfoAccess.IsSnowman && (follower.Brain.CurrentTask == null || !follower.Brain.CurrentTask.BlockTaskChanges && follower.Brain.CurrentTaskType != FollowerTaskType.Sleep) && follower.Brain.Info.CursedState == Thought.None && !follower.Brain.Info.Traits.Contains(FollowerTrait.TraitType.Mutated) && follower.Brain._directInfoAccess.CurrentPlayerQuest == null)
            {
              if ((double) Vector3.Distance(follower.transform.position, PlayerFarming.Instance.transform.position) < 6.0)
                followerList2.Add(follower);
              if (Quests.GetCurrentStoryObjective(follower.Brain.Info.ID) != null)
                followerList3.Add(follower);
              else if (follower.Brain.Info.IsDrunk)
                followerList3.Add(follower);
            }
          }
          Follower follower1 = (Follower) null;
          if (followerList3.Count > 0)
            follower1 = followerList3[UnityEngine.Random.Range(0, followerList3.Count)];
          else if (followerList2.Count > 0)
            follower1 = followerList2[UnityEngine.Random.Range(0, followerList2.Count)];
          if ((UnityEngine.Object) follower1 != (UnityEngine.Object) null && !TwitchHelpHinder.Active)
          {
            follower1.Brain.HardSwapToTask((FollowerTask) new FollowerTask_GetAttention(Follower.ComplaintType.GiveQuest));
            TimeManager.TimeSinceLastQuest = 0.0f;
          }
          else
            TimeManager.TimeSinceLastQuest = 240f;
        }
        TimeManager.TimeSinceLastQuest += deltaGameTime;
      }
    }
    for (int index4 = 0; index4 < 98; ++index4)
    {
      FollowerLocation location = (FollowerLocation) index4;
      int num2 = 0;
      foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
      {
        if (allBrain.HomeLocation == location && allBrain.ShouldReconsiderTask && allBrain.BeginReconsider())
          ++num2;
      }
      if (num2 > 0)
      {
        ScheduledActivity selectedActivity;
        List<FollowerTask> priorityFollowerTasks = FollowerBrain.GetTopPriorityFollowerTasks(location, out selectedActivity);
        List<FollowerTask> availableTasks = new List<FollowerTask>();
        if (selectedActivity != ScheduledActivity.Work)
          availableTasks = FollowerBrain.GetTopPriorityFollowerTasks(ScheduledActivity.Work, location);
        else
          availableTasks.AddRange((IEnumerable<FollowerTask>) priorityFollowerTasks);
        List<int> intList = new List<int>();
        foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
        {
          if (allBrain.HomeLocation == location)
          {
            foreach (int index5 in intList)
            {
              if (index5 != -1)
                availableTasks[index5] = (FollowerTask) null;
            }
            if (allBrain.ShouldReconsiderTask && TimeManager.GetScheduledActivity(location) == ScheduledActivity.Sleep && allBrain.Info.CursedState == Thought.None && allBrain._directInfoAccess.WorkThroughNight)
            {
              if (!FollowerBrainStats.ShouldWork && !allBrain.CanWork && !allBrain._directInfoAccess.IsSnowman)
                allBrain.ClaimNextAvailableTask(new List<FollowerTask>()
                {
                  (FollowerTask) new FollowerTask_FakeLeisure()
                });
              else
                intList.Add(allBrain.ClaimNextAvailableTask(availableTasks));
            }
            else if (allBrain.ShouldReconsiderTask)
            {
              if (priorityFollowerTasks.Count == 0)
                allBrain.TryClaimExistingTask(priorityFollowerTasks);
              else
                intList.Add(allBrain.TryClaimExistingTask(availableTasks));
            }
          }
        }
        int num3 = 5;
        bool flag1;
        do
        {
          flag1 = false;
          --num3;
          for (int index6 = 0; index6 < priorityFollowerTasks.Count; ++index6)
          {
            FollowerTask followerTask1 = priorityFollowerTasks[index6];
            if (followerTask1 != null && !intList.Contains(index6))
            {
              FollowerBrain brain = (FollowerBrain) null;
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
                      if (priorityCategory2 != PriorityCategory.Ignore)
                      {
                        bool flag2 = false;
                        if (followerTask1.GetPriorityCategory(allBrain.Info.FollowerRole, allBrain.Info.WorkerPriority, allBrain) < priorityCategory2)
                          flag2 = true;
                        else if (followerTask1.GetPriorityCategory(allBrain.Info.FollowerRole, allBrain.Info.WorkerPriority, allBrain) == priorityCategory2)
                          flag2 = (double) followerTask1.Priorty > (double) followerTask2.Priorty;
                        if (flag2)
                        {
                          if (allBrain.PendingTask.Task == null)
                          {
                            brain = allBrain;
                            break;
                          }
                          if (priorityCategory2 > priorityCategory1 || priorityCategory2 == priorityCategory1 && (double) followerTask2.Priorty < (double) num4)
                          {
                            brain = allBrain;
                            priorityCategory1 = priorityCategory2;
                            num4 = followerTask2.Priorty;
                          }
                        }
                      }
                    }
                  }
                  else if (allBrain.PendingTask.Task == null && followerTask1.GetPriorityCategory(allBrain.Info.FollowerRole, allBrain.Info.WorkerPriority, allBrain) != PriorityCategory.Ignore)
                  {
                    brain = allBrain;
                    break;
                  }
                }
              }
              if (brain != null)
              {
                flag1 = true;
                if (followerTask1.GetPriorityCategory(brain.Info.FollowerRole, brain.Info.WorkerPriority, brain) != PriorityCategory.Ignore)
                {
                  if (brain.PendingTask.Task != null)
                    priorityFollowerTasks[brain.PendingTask.ListIndex] = brain.PendingTask.Task;
                  brain.PendingTask.KeepExistingTask = false;
                  brain.PendingTask.Task = followerTask1;
                  brain.PendingTask.ListIndex = index6;
                  priorityFollowerTasks[index6] = (FollowerTask) null;
                }
              }
            }
          }
        }
        while (num3 > 0 & flag1);
        foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
        {
          if (allBrain.HomeLocation == location && allBrain.ShouldReconsiderTask && allBrain.CurrentTaskType != FollowerTaskType.ManualControl && allBrain.CurrentTaskType != FollowerTaskType.EnforcerManualControl)
            allBrain.EndReconsider();
        }
      }
    }
    if (DataManager.Instance.SurvivalModeActive && !SimulationManager.IsPaused && (UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
    {
      float num5 = GameManager.IsDungeon(PlayerFarming.Location) ? 0.75f : 1f;
      double survivalModeHunger = (double) DataManager.Instance.SurvivalMode_Hunger;
      DataManager.Instance.SurvivalMode_Hunger = Mathf.Clamp(DataManager.Instance.SurvivalMode_Hunger - 0.05f * TimeManager.DeltaGameTime * num5, 0.0f, 100f);
      if (survivalModeHunger > 10.0 && (double) DataManager.Instance.SurvivalMode_Hunger <= 10.0)
        NotificationCentre.Instance.PlayGenericNotification("Notifications/SurvivalHungry/Notification/On");
      double survivalModeSleep = (double) DataManager.Instance.SurvivalMode_Sleep;
      DataManager.Instance.SurvivalMode_Sleep = Mathf.Clamp(DataManager.Instance.SurvivalMode_Sleep - 0.035f * TimeManager.DeltaGameTime * num5, 0.0f, 100f);
      if (survivalModeSleep > 10.0 && (double) DataManager.Instance.SurvivalMode_Sleep <= 10.0)
        NotificationCentre.Instance.PlayGenericNotification("Notifications/SurvivalExhausted/Notification/On");
      if (!TimeManager.PlayerIsBusy() || GameManager.IsDungeon(PlayerFarming.Location))
      {
        if ((double) DataManager.Instance.SurvivalMode_Hunger <= 0.0 || (double) DataManager.Instance.SurvivalMode_Sleep <= 0.0)
        {
          TimeManager.SurvivalDamagedTimer += Time.deltaTime;
          if ((double) TimeManager.SurvivalDamagedTimer > 20.0)
          {
            if (TimeManager.SurvivalTallies <= 1)
            {
              System.Action survivalLossTally = TimeManager.OnSurvivalLossTally;
              if (survivalLossTally != null)
                survivalLossTally();
              DataManager.Instance.GameOver = true;
            }
            else
            {
              --TimeManager.SurvivalTallies;
              if (TimeManager.SurvivalTallies != 3)
              {
                System.Action survivalLossTally = TimeManager.OnSurvivalLossTally;
                if (survivalLossTally != null)
                  survivalLossTally();
              }
              UIManager.PlayAudio("event:/ui/level_node_die");
              TimeManager.SurvivalDamagedTimer = 0.0f;
            }
          }
        }
        else
        {
          TimeManager.SurvivalTallies = 3;
          TimeManager.SurvivalDamagedTimer = 0.0f;
        }
        if ((double) DataManager.Instance.SurvivalMode_Hunger <= 10.0 && DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.PlayerStarving))
        {
          MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.PlayerStarving);
          Time.timeScale = 0.0f;
        }
        if ((double) DataManager.Instance.SurvivalMode_Sleep <= 10.0 && DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.PlayerExhausted))
        {
          MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.PlayerExhausted);
          Time.timeScale = 0.0f;
        }
        float norm = (float) (1.0 - (double) Mathf.Min(DataManager.Instance.SurvivalMode_Hunger, DataManager.Instance.SurvivalMode_Sleep) / 10.0);
        if ((UnityEngine.Object) HUD_Manager.Instance != (UnityEngine.Object) null)
          HUD_Manager.Instance.SetHunderSleepOverlay(norm);
      }
      else if ((UnityEngine.Object) HUD_Manager.Instance != (UnityEngine.Object) null && ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null || PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.CustomAction0))
        HUD_Manager.Instance.SetHunderSleepOverlay(0.0f);
    }
    CultFaithManager.UpdateSimulation(TimeManager.DeltaGameTime);
    HungerBar.UpdateSimulation(TimeManager.DeltaGameTime);
    IllnessBar.UpdateSimulation(TimeManager.DeltaGameTime);
    WarmthBar.UpdateSimulation(TimeManager.DeltaGameTime);
    foreach (Follower locationFollower in FollowerManager.ActiveLocationFollowers())
    {
      if (Time.frameCount % TimeManager.SIMULATION_FRAME_SPREAD == locationFollower.Brain.Info.ID % TimeManager.SIMULATION_FRAME_SPREAD)
        locationFollower.Tick(TimeManager.DeltaGameTime * (float) TimeManager.SIMULATION_FRAME_SPREAD);
    }
    for (int location = 0; location < 98; ++location)
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
            if ((UnityEngine.Object) follower != (UnityEngine.Object) null)
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
    for (int index7 = 0; index7 < 98; ++index7)
    {
      FollowerLocation location = (FollowerLocation) index7;
      List<SimFollower> simFollowerList = FollowerManager.SimFollowersAtLocation(location);
      for (int index8 = 0; index8 < simFollowerList.Count; ++index8)
      {
        SimFollower simFollower = simFollowerList[index8];
        if (simFollower.Brain.DesiredLocation != simFollower.Brain.Location)
        {
          simFollowerList.RemoveAt(index8--);
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
    for (int index9 = 0; index9 < 98; ++index9)
    {
      FollowerLocation location = (FollowerLocation) index9;
      if (LocationManager.GetLocationState(location) != LocationState.Active)
      {
        List<SimFollower> simFollowerList = FollowerManager.SimFollowersAtLocation(location);
        for (int index10 = 0; index10 < simFollowerList.Count; ++index10)
        {
          if (skippingTime)
            simFollowerList[index10].Tick(TimeManager.DeltaGameTime);
          else if (Time.frameCount % TimeManager.SIMULATION_FRAME_SPREAD == simFollowerList[index10].Brain.Info.ID % TimeManager.SIMULATION_FRAME_SPREAD)
            simFollowerList[index10].Tick(TimeManager.DeltaGameTime * (float) TimeManager.SIMULATION_FRAME_SPREAD);
        }
      }
    }
    for (int location = 0; location < 98; ++location)
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

  public static int GetLastUsedDayIndex(SermonsAndRituals.SermonRitualType type)
  {
    TimeManager.CheckResizeCooldownArray();
    return DataManager.Instance.LastUsedSermonRitualDayIndex[(int) type];
  }

  public static void SetSermonRitualUsed(SermonsAndRituals.SermonRitualType type)
  {
    TimeManager.CheckResizeCooldownArray();
    DataManager.Instance.LastUsedSermonRitualDayIndex[(int) type] = DataManager.Instance.CurrentDayIndex;
  }

  public static void CheckResizeCooldownArray()
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

  public static void ClearOverrideTaskProvider()
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

  public static IEnumerator SkipTimeIE(float duration)
  {
    while (PlayerFarming.Location != FollowerLocation.Base || LetterBox.IsPlaying || MMConversation.isPlaying || SimulationManager.IsPaused || PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.Idle && PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.Moving)
      yield return (object) null;
    float targetTime = TimeManager.TotalElapsedGameTime + duration;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(TownCentre.Instance.gameObject, 20f);
    GameManager.GetInstance().StartCoroutine((IEnumerator) TimeManager.\u003CSkipTimeIE\u003Eg__Delay\u007C99_0());
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
  }

  public static bool PlayerIsBusy()
  {
    return LetterBox.IsPlaying || MMConversation.isPlaying || SimulationManager.IsPaused || (UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null || LetterBox.IsPlaying && PlayerFarming.Instance.state.CURRENT_STATE == StateMachine.State.CustomAnimation || PlayerFarming.Instance.state.CURRENT_STATE == StateMachine.State.InActive || (UnityEngine.Object) PlacementObject.Instance != (UnityEngine.Object) null;
  }

  [CompilerGenerated]
  public static IEnumerator \u003CSkipTimeIE\u003Eg__Delay\u007C99_0()
  {
    yield return (object) new WaitForSeconds(1.2f);
    HUD_Manager.Instance.TimeTransitions.MoveBackInFunction();
  }
}
