// Decompiled with JetBrains decompiler
// Type: FollowerManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerManager
{
  public static Dictionary<FollowerLocation, List<Follower>> Followers = new Dictionary<FollowerLocation, List<Follower>>();
  public static Dictionary<FollowerLocation, List<SimFollower>> SimFollowers = new Dictionary<FollowerLocation, List<SimFollower>>();
  public static FollowerManager.FollowerChanged OnFollowerAdded;
  public static FollowerManager.FollowerChanged OnFollowerRemoved;
  public static FollowerManager.FollowerGoneEvent OnFollowerDie;
  public static FollowerManager.FollowerGoneEvent OnFollowerLeave;
  public static int CopyFollowersActive = 0;

  public static Follower FollowerPrefab
  {
    get => Resources.Load<Follower>("Prefabs/Units/Villagers/Follower");
  }

  public static Follower CombatFollowerPrefab
  {
    get => Resources.Load<Follower>("Prefabs/Units/Villagers/Enemy Follower");
  }

  public static FollowerRecruit RecruitPrefab
  {
    get => Resources.Load<FollowerRecruit>("Prefabs/Units/Villagers/Recruit Variant");
  }

  public static IEnumerable<Follower> ActiveLocationFollowers()
  {
    foreach (FollowerLocation location in LocationManager.LocationsInState(LocationState.Active))
    {
      foreach (Follower follower in FollowerManager.FollowersAtLocation(location))
        yield return follower;
    }
  }

  public static IEnumerable<FollowerBrain> FollowerBrainsByHomeLocation(
    FollowerLocation homeLocation)
  {
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.HomeLocation == homeLocation)
        yield return allBrain;
    }
  }

  public static List<Follower> FollowersAtLocation(FollowerLocation location)
  {
    List<Follower> followerList = (List<Follower>) null;
    if (location == FollowerLocation.None)
      followerList = new List<Follower>();
    else if (!FollowerManager.Followers.TryGetValue(location, out followerList))
    {
      followerList = new List<Follower>();
      FollowerManager.Followers[location] = followerList;
    }
    return followerList;
  }

  public static List<SimFollower> SimFollowersAtLocation(FollowerLocation location)
  {
    List<SimFollower> simFollowerList = (List<SimFollower>) null;
    if (location == FollowerLocation.None)
      simFollowerList = new List<SimFollower>();
    else if (!FollowerManager.SimFollowers.TryGetValue(location, out simFollowerList))
    {
      simFollowerList = new List<SimFollower>();
      FollowerManager.SimFollowers[location] = simFollowerList;
    }
    return simFollowerList;
  }

  public static FollowerInfo FindFollowerInfo(int ID)
  {
    FollowerInfo followerInfo = (FollowerInfo) null;
    for (int index = 0; index < DataManager.Instance.Followers.Count; ++index)
    {
      if (DataManager.Instance.Followers[index].ID == ID)
      {
        followerInfo = DataManager.Instance.Followers[index];
        break;
      }
    }
    return followerInfo;
  }

  public static FollowerInfo RemoveFollower(int ID)
  {
    FollowerInfo followerInfo = (FollowerInfo) null;
    for (int index = 0; index < DataManager.Instance.Followers.Count; ++index)
    {
      if (DataManager.Instance.Followers[index].ID == ID)
      {
        followerInfo = DataManager.Instance.Followers[index];
        DataManager.Instance.Followers.RemoveAt(index);
        FollowerManager.FollowerChanged onFollowerRemoved = FollowerManager.OnFollowerRemoved;
        if (onFollowerRemoved != null)
          onFollowerRemoved(ID);
        FollowerManager.DestroyPausedFollowerByID(ID);
        FollowerManager.RetireSimFollowerByID(ID);
        break;
      }
    }
    return followerInfo;
  }

  public static bool FollowerLocked(int ID, bool exludeStarving = false)
  {
    return FollowerInfo.GetInfoByID(ID) == null || DataManager.Instance.Followers_Imprisoned_IDs.Contains(ID) || DataManager.Instance.Followers_OnMissionary_IDs.Contains(ID) || DataManager.Instance.Followers_Demons_IDs.Contains(ID) || DataManager.Instance.Followers_Transitioning_IDs.Contains(ID) || DataManager.Instance.Followers_Recruit.Contains(FollowerInfo.GetInfoByID(ID)) || FollowerInfo.GetInfoByID(ID).IsStarving && !exludeStarving || FollowerInfo.GetInfoByID(ID).LeavingCult;
  }

  public static void ConsumeFollower(int ID)
  {
    FollowerInfo infoById = FollowerInfo.GetInfoByID(ID);
    if (infoById != null)
      DataManager.Instance.Followers_Dead.Add(infoById);
    DataManager.Instance.Followers_Dead_IDs.Add(ID);
    FollowerManager.RemoveFollower(ID);
    FollowerBrain.RemoveBrain(ID);
  }

  public static void FollowerLeave(
    int ID,
    NotificationCentre.NotificationType leaveNotificationType)
  {
    FollowerManager.FollowerGoneEvent onFollowerLeave = FollowerManager.OnFollowerLeave;
    if (onFollowerLeave == null)
      return;
    onFollowerLeave(ID, leaveNotificationType);
  }

  public static void FollowerDie(
    int ID,
    NotificationCentre.NotificationType deathNotificationType)
  {
    switch (deathNotificationType)
    {
      case NotificationCentre.NotificationType.DiedFromStarvation:
        ++DataManager.Instance.STATS_FollowersStarvedToDeath;
        if (DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.DesensitisedToDeath))
          CultFaithManager.AddThought(Thought.Cult_FollowerDied_Trait, ID);
        else
          CultFaithManager.AddThought(Thought.Cult_FollowerDied, ID);
        if (FollowerInfo.GetInfoByID(ID, true).CursedState == Thought.OldAge && DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.HateElderly))
        {
          CultFaithManager.AddThought(Thought.Cult_HateElderly_Trait_Cons);
          goto case NotificationCentre.NotificationType.Ascended;
        }
        goto case NotificationCentre.NotificationType.Ascended;
      case NotificationCentre.NotificationType.DiedFromIllness:
        CultFaithManager.AddThought(Thought.DiedFromIllness, ID);
        goto case NotificationCentre.NotificationType.Ascended;
      case NotificationCentre.NotificationType.SacrificeFollower:
        if (FollowerInfo.GetInfoByID(ID, true).CursedState == Thought.OldAge && DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.HateElderly))
        {
          CultFaithManager.AddThought(Thought.Cult_HateElderly_Trait_Pros);
          goto case NotificationCentre.NotificationType.Ascended;
        }
        goto case NotificationCentre.NotificationType.Ascended;
      case NotificationCentre.NotificationType.MurderedByYou:
        bool flag = false;
        foreach (ObjectivesData completedObjective in DataManager.Instance.CompletedObjectives)
        {
          if (completedObjective is Objectives_Custom && completedObjective.Follower == ID && (((Objectives_Custom) completedObjective).CustomQuestType == Objectives.CustomQuestTypes.MurderFollower || ((Objectives_Custom) completedObjective).CustomQuestType == Objectives.CustomQuestTypes.KillFollower || ((Objectives_Custom) completedObjective).CustomQuestType == Objectives.CustomQuestTypes.MurderFollowerAtNight && TimeManager.IsNight))
            flag = true;
        }
        if (TimeManager.IsNight)
        {
          int num = 0;
          foreach (Follower follower in Follower.Followers)
          {
            if (!((UnityEngine.Object) follower == (UnityEngine.Object) null) && follower.Brain != null && follower.Brain.CurrentTask != null && (follower.Brain.CurrentTaskType == FollowerTaskType.Sleep || follower.Brain.CurrentTaskType == FollowerTaskType.SleepBedRest) && follower.Brain.CurrentTask.State == FollowerTaskState.Doing)
              ++num;
          }
          CultFaithManager.AddThought(Thought.Cult_MurderAtNightNoWitnesses, ID, flag ? 0.0f : 1f);
        }
        else
          CultFaithManager.AddThought(Thought.Cult_Murder, ID, flag ? 0.0f : 1f);
        if (FollowerInfo.GetInfoByID(ID, true).CursedState == Thought.OldAge && DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.HateElderly))
        {
          CultFaithManager.AddThought(Thought.Cult_HateElderly_Trait_Pros);
          goto case NotificationCentre.NotificationType.Ascended;
        }
        goto case NotificationCentre.NotificationType.Ascended;
      case NotificationCentre.NotificationType.DiedFromOldAge:
        ++DataManager.Instance.STATS_NaturalDeaths;
        CultFaithManager.AddThought(Thought.Cult_FollowerDiedOfOldAge, ID);
        goto case NotificationCentre.NotificationType.Ascended;
      case NotificationCentre.NotificationType.Ascended:
        foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
        {
          if (allBrain != null && allBrain.Info != null && allBrain.Info.CursedState != Thought.Zombie && allBrain.Info.ID != ID)
          {
            switch (deathNotificationType)
            {
              case NotificationCentre.NotificationType.SacrificeFollower:
                if (allBrain.HasTrait(FollowerTrait.TraitType.HateElderly) && DataManager.Instance.Followers_Elderly_IDs.Contains(ID))
                  allBrain.AddThought(Thought.ElderlySacrificedHateElderly);
                else if (allBrain.HasTrait(FollowerTrait.TraitType.LoveElderly) && DataManager.Instance.Followers_Elderly_IDs.Contains(ID))
                  allBrain.AddThought(Thought.ElderlySacrificedLoveElderly);
                else if (allBrain.HasTrait(FollowerTrait.TraitType.AgainstSacrifice))
                  allBrain.AddThought(Thought.CultMemberWasSacrificedAgainstSacrificeTrait);
                else if (allBrain.HasTrait(FollowerTrait.TraitType.SacrificeEnthusiast))
                  allBrain.AddThought(Thought.CultMemberWasSacrificedSacrificeEnthusiastTrait);
                else
                  allBrain.AddThought(Thought.CultMemberWasSacrificed);
                if (allBrain.HasTrait(FollowerTrait.TraitType.AgainstSacrifice) && FollowerInfo.GetInfoByID(allBrain.Info.ID) != null)
                {
                  CultFaithManager.AddThought(Thought.Cult_Sacrifice_Trait_Scared, allBrain.Info.ID);
                  continue;
                }
                continue;
              case NotificationCentre.NotificationType.MurderedByYou:
                if (allBrain.HasTrait(FollowerTrait.TraitType.HateElderly) && DataManager.Instance.Followers_Elderly_IDs.Contains(ID))
                {
                  allBrain.AddThought(Thought.LeaderMurderedAFollowerHateElderly);
                  continue;
                }
                if (allBrain.HasTrait(FollowerTrait.TraitType.LoveElderly) && DataManager.Instance.Followers_Elderly_IDs.Contains(ID))
                {
                  allBrain.AddThought(Thought.LeaderMurderedAFollowerLoveElderly);
                  continue;
                }
                allBrain.AddThought(Thought.LeaderMurderedAFollower);
                continue;
              case NotificationCentre.NotificationType.Ascended:
                allBrain.AddThought(Thought.FollowerAscended);
                continue;
              case NotificationCentre.NotificationType.KilledInAFightPit:
                allBrain.AddThought(Thought.FightPitExecution);
                continue;
              default:
                if (allBrain.HasTrait(FollowerTrait.TraitType.FearOfDeath) && allBrain.Info.ID != ID && FollowerInfo.GetInfoByID(allBrain.Info.ID) != null)
                {
                  allBrain.AddThought(Thought.CultMemberDiedScaredOfDeathTrait);
                  CultFaithManager.AddThought(Thought.Cult_FollowerDied_Trait_Scared, allBrain.Info.ID);
                  continue;
                }
                if (allBrain.HasTrait(FollowerTrait.TraitType.DesensitisedToDeath))
                {
                  allBrain.AddThought(Thought.CultMemberDiedScaredOfDesensitized);
                  continue;
                }
                switch (allBrain.Info.GetOrCreateRelationship(ID).CurrentRelationshipState)
                {
                  case IDAndRelationship.RelationshipState.Enemies:
                    allBrain.AddThought(Thought.EnemyDied);
                    continue;
                  case IDAndRelationship.RelationshipState.Strangers:
                    allBrain.AddThought(Thought.StrangerDied);
                    continue;
                  case IDAndRelationship.RelationshipState.Friends:
                    allBrain.AddThought(Thought.FriendDied);
                    continue;
                  case IDAndRelationship.RelationshipState.Lovers:
                    allBrain.AddThought(Thought.LoverDied);
                    continue;
                  default:
                    continue;
                }
            }
          }
        }
        FollowerManager.FollowerGoneEvent onFollowerDie = FollowerManager.OnFollowerDie;
        if (onFollowerDie != null)
          onFollowerDie(ID, deathNotificationType);
        FollowerInfo followerInfo = FollowerManager.RemoveFollower(ID);
        if (followerInfo != null && followerInfo.CursedState == Thought.Dissenter)
        {
          ObjectiveManager.FailLockCustomObjective(Objectives.CustomQuestTypes.CureDissenter, true);
          ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.CureDissenter, ID);
          ObjectiveManager.FailLockCustomObjective(Objectives.CustomQuestTypes.CureDissenter, false);
        }
        if (followerInfo != null)
        {
          ObjectiveManager.FailLockCustomObjective(Objectives.CustomQuestTypes.KillFollower, true);
          ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.KillFollower, ID);
          ObjectiveManager.FailLockCustomObjective(Objectives.CustomQuestTypes.KillFollower, false);
        }
        ObjectiveManager.FailCustomObjective(Objectives.CustomQuestTypes.FollowerRecoverIllness);
        if (followerInfo != null)
        {
          DataManager.Instance.Followers_Dead.Add(followerInfo);
          DataManager.Instance.Followers_Dead_IDs.Add(followerInfo.ID);
        }
        FollowerManager.RemoveFollowerBrain(ID);
        if (followerInfo == null)
          break;
        foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
        {
          if (allBrain.HasTrait(FollowerTrait.TraitType.LoveElderly))
            allBrain.RemoveThought(Thought.FollowerIsElderlyLoveElderly, false);
          else if (allBrain.HasTrait(FollowerTrait.TraitType.HateElderly))
            allBrain.RemoveThought(Thought.FollowerIsElderlyHateElderly, false);
        }
        if (followerInfo.CursedState != Thought.Zombie)
          break;
        ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.ZombieExists);
        using (List<FollowerBrain>.Enumerator enumerator = FollowerBrain.AllBrains.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            FollowerBrain current = enumerator.Current;
            if (current.Info.CursedState != Thought.Zombie)
              current.AddThought(Thought.ZombieDied);
          }
          break;
        }
      case NotificationCentre.NotificationType.KilledInAFightPit:
      case NotificationCentre.NotificationType.ConsumeFollower:
        if (FollowerInfo.GetInfoByID(ID, true).CursedState == Thought.OldAge && DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.HateElderly))
        {
          CultFaithManager.AddThought(Thought.Cult_HateElderly_Trait_Pros);
          goto case NotificationCentre.NotificationType.Ascended;
        }
        goto case NotificationCentre.NotificationType.Ascended;
      default:
        if (DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.DesensitisedToDeath))
          CultFaithManager.AddThought(Thought.Cult_FollowerDied_Trait, ID);
        else
          CultFaithManager.AddThought(Thought.Cult_FollowerDied, ID);
        if (FollowerInfo.GetInfoByID(ID, true).CursedState == Thought.OldAge && DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.HateElderly))
        {
          CultFaithManager.AddThought(Thought.Cult_HateElderly_Trait_Cons);
          goto case NotificationCentre.NotificationType.Ascended;
        }
        goto case NotificationCentre.NotificationType.Ascended;
    }
  }

  public static void RemoveFollowerBrain(int ID)
  {
    FollowerInfo followerInfo = FollowerManager.RemoveFollower(ID);
    if (followerInfo != null)
    {
      DataManager.Instance.Followers_Imprisoned_IDs.Remove(followerInfo.ID);
      DataManager.Instance.Followers_OnMissionary_IDs.Remove(ID);
      DataManager.Instance.Followers_Elderly_IDs.Remove(followerInfo.ID);
      DataManager.Instance.Followers_Transitioning_IDs.Remove(followerInfo.ID);
    }
    FollowerBrain.RemoveBrain(ID);
    ObjectiveManager.CheckObjectives();
  }

  public static FollowerBrain GetHungriestFollowerBrain()
  {
    float num = float.MinValue;
    FollowerBrain hungriestFollowerBrain = (FollowerBrain) null;
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (!FollowerManager.FollowerLocked(allBrain.Info.ID, true))
      {
        float hungerScore = allBrain.GetHungerScore();
        if ((double) hungerScore > (double) num)
        {
          num = hungerScore;
          hungriestFollowerBrain = allBrain;
        }
      }
    }
    return hungriestFollowerBrain;
  }

  public static FollowerBrain GetHungriestFollowerBrainNotStarving()
  {
    float num = float.MinValue;
    FollowerBrain brainNotStarving = (FollowerBrain) null;
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (!FollowerManager.FollowerLocked(allBrain.Info.ID) && !allBrain.HasTrait(FollowerTrait.TraitType.DontStarve))
      {
        float scoreNotStarving = allBrain.GetHungerScoreNotStarving();
        if ((double) scoreNotStarving > (double) num)
        {
          num = scoreNotStarving;
          brainNotStarving = allBrain;
        }
      }
    }
    return brainNotStarving;
  }

  public static FollowerBrain GetHungriestFollowerBrainNoCursedState()
  {
    float num = float.MinValue;
    FollowerBrain brainNoCursedState = (FollowerBrain) null;
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (!FollowerManager.FollowerLocked(allBrain.Info.ID))
      {
        float scoreNoCursedState = allBrain.GetHungerScoreNoCursedState();
        if ((double) scoreNoCursedState > (double) num)
        {
          num = scoreNoCursedState;
          brainNoCursedState = allBrain;
        }
      }
    }
    return brainNoCursedState;
  }

  public static int GetHungerScoreIndex(FollowerBrain brain)
  {
    int hungerScoreIndex = 0;
    float hungerScore = brain.GetHungerScore();
    if ((double) hungerScore == 0.0)
    {
      hungerScoreIndex = -1;
    }
    else
    {
      foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
      {
        if (allBrain != brain && (double) allBrain.GetHungerScore() > (double) hungerScore)
          ++hungerScoreIndex;
      }
    }
    return hungerScoreIndex;
  }

  public static void DestroyPausedFollowerByID(int followerID)
  {
    for (int location = 0; location < 84; ++location)
    {
      List<Follower> followerList = FollowerManager.FollowersAtLocation((FollowerLocation) location);
      foreach (Follower follower in followerList)
      {
        if (follower.Brain.Info.ID == followerID)
        {
          followerList.Remove(follower);
          FollowerManager.FollowerChanged onFollowerRemoved = FollowerManager.OnFollowerRemoved;
          if (onFollowerRemoved != null)
            onFollowerRemoved(follower.Brain.Info.ID);
          if (!follower.IsPaused)
            return;
          UnityEngine.Object.Destroy((UnityEngine.Object) follower.gameObject);
          return;
        }
      }
    }
  }

  public static void RetireSimFollowerByID(int followerID)
  {
    for (int location = 0; location < 84; ++location)
    {
      foreach (SimFollower simFollower in FollowerManager.SimFollowersAtLocation((FollowerLocation) location))
      {
        if (simFollower.Brain.Info.ID == followerID)
        {
          simFollower.Retire();
          return;
        }
      }
    }
  }

  public static FollowerManager.SpawnedFollower SpawnCopyFollower(
    FollowerInfo followerInfo,
    Vector3 position,
    Transform parent,
    FollowerLocation location)
  {
    return FollowerManager.SpawnCopyFollower(FollowerManager.FollowerPrefab, followerInfo, position, parent, location);
  }

  public static FollowerManager.SpawnedFollower SpawnCopyFollower(
    Follower followerPrefab,
    FollowerInfo followerInfo,
    Vector3 position,
    Transform parent,
    FollowerLocation location)
  {
    FollowerManager.SpawnedFollower spawnedFollower = new FollowerManager.SpawnedFollower();
    ++FollowerManager.CopyFollowersActive;
    spawnedFollower.FollowerFakeInfo = FollowerInfo.NewCharacter(location);
    spawnedFollower.FollowerFakeInfo.IsFakeBrain = true;
    spawnedFollower.FollowerFakeInfo.ViewerID = followerInfo.ViewerID;
    spawnedFollower.FollowerFakeInfo.Name = followerInfo.Name;
    spawnedFollower.Follower = UnityEngine.Object.Instantiate<Follower>((UnityEngine.Object) followerPrefab != (UnityEngine.Object) null ? followerPrefab : FollowerManager.FollowerPrefab, position, Quaternion.identity, parent);
    spawnedFollower.FollowerBrain = FollowerBrain.GetOrCreateBrain(followerInfo);
    spawnedFollower.FollowerFakeBrain = FollowerBrain.GetOrCreateBrain(spawnedFollower.FollowerFakeInfo);
    spawnedFollower.FollowerFakeBrain.Stats = spawnedFollower.FollowerBrain.Stats;
    spawnedFollower.Follower.Init(spawnedFollower.FollowerFakeBrain, new FollowerOutfit(followerInfo));
    spawnedFollower.Follower.Brain.CheckChangeState();
    spawnedFollower.Follower.gameObject.SetActive(true);
    spawnedFollower.Follower.Interaction_FollowerInteraction.enabled = false;
    spawnedFollower.Follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    spawnedFollower.Follower.State.LookAngle = Utils.GetAngle(spawnedFollower.Follower.transform.position, position);
    spawnedFollower.Follower.State.facingAngle = Utils.GetAngle(spawnedFollower.Follower.transform.position, position);
    spawnedFollower.Follower.HideAllFollowerIcons();
    return spawnedFollower;
  }

  public static void CleanUpCopyFollower(FollowerManager.SpawnedFollower spawnedFollower)
  {
    FollowerManager.CopyFollowersActive = Mathf.Clamp(FollowerManager.CopyFollowersActive - 1, 0, int.MaxValue);
    if ((UnityEngine.Object) spawnedFollower.Follower != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) spawnedFollower.Follower.gameObject);
    FollowerBrain.RemoveBrain(spawnedFollower.FollowerFakeBrain.Info.ID);
  }

  public static FollowerInfo GetDeadFollowerInfoByID(int followerID)
  {
    FollowerInfo followerInfoById = (FollowerInfo) null;
    foreach (FollowerInfo followerInfo in DataManager.Instance.Followers_Dead)
    {
      if (followerInfo.ID == followerID)
      {
        followerInfoById = followerInfo;
        break;
      }
    }
    return followerInfoById;
  }

  public static Follower FindFollowerByViewerID(string viewerID)
  {
    Follower followerByViewerId = (Follower) null;
    for (int location = 0; location < 84; ++location)
    {
      foreach (Follower follower in FollowerManager.FollowersAtLocation((FollowerLocation) location))
      {
        if (follower.Brain.Info.ViewerID == viewerID)
        {
          followerByViewerId = follower;
          goto label_9;
        }
      }
    }
label_9:
    return followerByViewerId;
  }

  public static Follower FindFollowerByID(int ID)
  {
    Follower followerById = (Follower) null;
    for (int location = 0; location < 84; ++location)
    {
      foreach (Follower follower in FollowerManager.FollowersAtLocation((FollowerLocation) location))
      {
        if (follower.Brain.Info.ID == ID)
        {
          followerById = follower;
          goto label_9;
        }
      }
    }
label_9:
    return followerById;
  }

  public static SimFollower FindSimFollowerByID(int ID)
  {
    SimFollower simFollowerById = (SimFollower) null;
    for (int location = 0; location < 84; ++location)
    {
      foreach (SimFollower simFollower in FollowerManager.SimFollowersAtLocation((FollowerLocation) location))
      {
        if (simFollower.Brain.Info.ID == ID)
        {
          simFollowerById = simFollower;
          goto label_9;
        }
      }
    }
label_9:
    return simFollowerById;
  }

  public static void KillFollowerOrSimByID(int ID, NotificationCentre.NotificationType Notification)
  {
    Follower followerById = FollowerManager.FindFollowerByID(ID);
    if ((UnityEngine.Object) followerById != (UnityEngine.Object) null)
    {
      followerById.Die(Notification);
    }
    else
    {
      SimFollower simFollowerById = FollowerManager.FindSimFollowerByID(ID);
      simFollowerById?.Die(Notification, simFollowerById.Brain.LastPosition);
    }
  }

  public static void SpawnExistingRecruits(Vector3 position)
  {
    if (DataManager.Instance.Followers_Recruit.Count <= 0)
      return;
    FollowerManager.SpawnRecruit(DataManager.Instance.Followers_Recruit[0], position);
  }

  public static float GetTotalNonLockedFollowers()
  {
    int nonLockedFollowers = 0;
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (!FollowerManager.FollowerLocked(allBrain.Info.ID))
        ++nonLockedFollowers;
    }
    return (float) nonLockedFollowers;
  }

  public static void CreateNewRecruit(
    FollowerLocation location,
    NotificationCentre.NotificationType Notification)
  {
    DataManager.Instance.Followers_Recruit.Add(FollowerInfo.NewCharacter(location));
    NotificationCentre.Instance.PlayGenericNotification(Notification);
  }

  public static FollowerInfo CreateNewRecruit(
    FollowerLocation location,
    string ForceSkin,
    NotificationCentre.NotificationType Notification)
  {
    FollowerInfo newRecruit = FollowerInfo.NewCharacter(location, ForceSkin);
    DataManager.Instance.Followers_Recruit.Add(newRecruit);
    NotificationCentreScreen.Play(Notification);
    return newRecruit;
  }

  public static FollowerRecruit CreateNewRecruit(FollowerLocation location, Vector3 position)
  {
    return FollowerManager.CreateNewRecruit(FollowerInfo.NewCharacter(location), position);
  }

  public static void CreateNewRecruit(
    FollowerInfo f,
    NotificationCentre.NotificationType Notification)
  {
    DataManager.Instance.Followers_Recruit.Add(f);
    DataManager.SetFollowerSkinUnlocked(f.SkinName);
    NotificationCentreScreen.Play(Notification);
    if (!GameManager.IsDungeon(PlayerFarming.Location))
      return;
    ++DataManager.Instance.FollowersRecruitedThisNode;
  }

  public static FollowerRecruit CreateNewRecruit(FollowerInfo f, Vector3 position)
  {
    DataManager.Instance.Followers_Recruit.Add(f);
    DataManager.SetFollowerSkinUnlocked(f.SkinName);
    return FollowerManager.SpawnRecruit(f, position);
  }

  private static FollowerRecruit SpawnRecruit(FollowerInfo f, Vector3 position, bool Force = false)
  {
    FollowerRecruit followerRecruit = (FollowerRecruit) null;
    FollowerBrain brain = FollowerBrain.GetOrCreateBrain(f);
    LocationManager locationManager = LocationManager.LocationManagers[f.Location];
    if ((UnityEngine.Object) locationManager != (UnityEngine.Object) null)
      followerRecruit = locationManager.SpawnRecruit(brain, position);
    brain.TransitionToTask((FollowerTask) new FollowerTask_ManualControl());
    followerRecruit.SpawnAnim(true);
    return followerRecruit;
  }

  public static FollowerInfo RemoveRecruit(int ID)
  {
    FollowerInfo followerInfo = (FollowerInfo) null;
    for (int index = 0; index < DataManager.Instance.Followers_Recruit.Count; ++index)
    {
      if (DataManager.Instance.Followers_Recruit[index].ID == ID)
      {
        followerInfo = DataManager.Instance.Followers_Recruit[index];
        DataManager.Instance.Followers_Recruit.RemoveAt(index);
        break;
      }
    }
    return followerInfo;
  }

  public static void RecruitFollower(FollowerRecruit recruit, bool followPlayer)
  {
    FollowerInfo f = FollowerManager.RemoveRecruit(recruit.Follower.Brain.Info.ID);
    FollowerManager.AddFollower(f, recruit.transform.position);
    FollowerManager.FollowersAtLocation(f.Location).Add(recruit.Follower);
    recruit.Follower.Brain.FollowingPlayer = followPlayer;
    recruit.Follower.Resume();
    recruit.Follower.Brain.CompleteCurrentTask();
  }

  public static Follower CreateNewFollower(
    FollowerLocation location,
    Vector3 position,
    bool followPlayer = false)
  {
    return FollowerManager.CreateNewFollower(FollowerInfo.NewCharacter(location), position, followPlayer);
  }

  public static Follower CreateNewFollower(FollowerInfo f, Vector3 position, bool followPlayer = false)
  {
    FollowerManager.AddFollower(f, position);
    return FollowerManager.SpawnFollower(f, position, followPlayer);
  }

  private static void AddFollower(FollowerInfo f, Vector3 position)
  {
    DataManager.Instance.Followers.Add(f);
    DataManager.Instance.GetFirstFollower = true;
    FollowerManager.FollowerChanged onFollowerAdded = FollowerManager.OnFollowerAdded;
    if (onFollowerAdded == null)
      return;
    onFollowerAdded(f.ID);
  }

  public static Follower SpawnFollower(FollowerInfo f, Vector3 position, bool followPlayer = false)
  {
    Follower follower1 = (Follower) null;
    FollowerBrain brain = FollowerBrain.GetOrCreateBrain(f);
    brain.FollowingPlayer = followPlayer;
    LocationManager locationManager = LocationManager.LocationManagers[f.Location];
    if ((UnityEngine.Object) locationManager != (UnityEngine.Object) null)
    {
      Follower follower2 = locationManager.SpawnFollower(brain, position);
      FollowerManager.FollowersAtLocation(f.Location).Add(follower2);
      follower2.Resume();
      follower1 = follower2;
    }
    else
    {
      SimFollower simFollower = new SimFollower(brain);
      FollowerManager.SimFollowersAtLocation(f.Location).Add(simFollower);
    }
    brain.CompleteCurrentTask();
    return follower1;
  }

  public static void ReviveFollower(int ID, FollowerLocation location, Vector3 position)
  {
    FollowerInfo f = (FollowerInfo) null;
    for (int index = 0; index < DataManager.Instance.Followers_Dead.Count; ++index)
    {
      if (DataManager.Instance.Followers_Dead[index].ID == ID)
      {
        f = DataManager.Instance.Followers_Dead[index];
        DataManager.Instance.Followers_Dead.RemoveAt(index);
        DataManager.Instance.Followers_Dead_IDs.RemoveAt(index);
        break;
      }
    }
    if (f == null)
      return;
    f.Location = location;
    FollowerManager.AddFollower(f, position);
  }

  public static void Reset()
  {
    foreach (KeyValuePair<FollowerLocation, List<Follower>> follower1 in FollowerManager.Followers)
    {
      if (follower1.Value != null)
      {
        foreach (Follower follower2 in follower1.Value)
        {
          if ((UnityEngine.Object) follower2 != (UnityEngine.Object) null)
            follower2.gameObject.SetActive(false);
        }
      }
    }
    if (FollowerManager.Followers != null)
      FollowerManager.Followers.Clear();
    foreach (FollowerBrain followerBrain in new List<FollowerBrain>((IEnumerable<FollowerBrain>) FollowerBrain.AllBrains))
    {
      followerBrain.Cleanup();
      if (followerBrain.Info != null)
        FollowerBrain.RemoveBrain(followerBrain.Info.ID);
    }
    if (FollowerManager.SimFollowers != null)
      FollowerManager.SimFollowers.Clear();
    PlayerFarming.LastLocation = FollowerLocation.None;
    PlayerFarming.Location = FollowerLocation.None;
  }

  public static void GiveFollowersRandomCurse(int amountOfFollowers, Thought curseType = Thought.None)
  {
    List<FollowerBrain> followerBrainList = new List<FollowerBrain>((IEnumerable<FollowerBrain>) FollowerBrain.AllBrains);
    for (int index = followerBrainList.Count - 1; index >= 0; --index)
    {
      if (FollowerManager.FollowerLocked(followerBrainList[index].Info.ID) || followerBrainList[index].Info.CursedState != Thought.None || curseType == Thought.BecomeStarving && followerBrainList[index].HasTrait(FollowerTrait.TraitType.DontStarve))
        followerBrainList.Remove(followerBrainList[index]);
    }
    for (int index = 0; index < amountOfFollowers; ++index)
    {
      Thought thought = curseType;
      if (thought == Thought.None)
      {
        switch (UnityEngine.Random.Range(0, 3))
        {
          case 0:
            thought = Thought.Ill;
            break;
          case 1:
            thought = Thought.Dissenter;
            break;
          case 2:
            thought = Thought.BecomeStarving;
            break;
        }
      }
      if (followerBrainList.Count <= 0)
        break;
      FollowerBrain followerBrain = followerBrainList[UnityEngine.Random.Range(0, followerBrainList.Count)];
      followerBrainList.Remove(followerBrain);
      switch (thought)
      {
        case Thought.Dissenter:
          followerBrain.MakeDissenter();
          break;
        case Thought.Ill:
          followerBrain.MakeSick();
          break;
        case Thought.BecomeStarving:
          followerBrain.MakeStarve();
          break;
      }
    }
  }

  public static void MakeAllFollowersFallAsleep()
  {
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      FollowerBrain brain = allBrain;
      if (!FollowerManager.FollowerLocked(brain.Info.ID))
        GameManager.GetInstance().StartCoroutine((IEnumerator) FollowerManager.Delay(UnityEngine.Random.Range(0.0f, 1f), (System.Action) (() =>
        {
          if (FollowerManager.FollowerLocked(brain.Info.ID))
            return;
          brain.HardSwapToTask((FollowerTask) new FollowerTask_Sleep(true));
        })));
    }
  }

  private static IEnumerator Delay(float delay, System.Action callback)
  {
    yield return (object) new WaitForSeconds(delay);
    System.Action action = callback;
    if (action != null)
      action();
  }

  public static void KillRandomFollower(bool diedFromTwitchChat)
  {
    List<FollowerBrain> followerBrainList = new List<FollowerBrain>();
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (!FollowerManager.FollowerLocked(allBrain.Info.ID))
        followerBrainList.Add(allBrain);
    }
    FollowerBrain followerBrain = followerBrainList[UnityEngine.Random.Range(0, followerBrainList.Count)];
    followerBrain.DiedFromTwitchChat = diedFromTwitchChat;
    if ((UnityEngine.Object) FollowerManager.FindFollowerByID(followerBrain.Info.ID) != (UnityEngine.Object) null)
      FollowerManager.FindFollowerByID(followerBrain.Info.ID).Die();
    else
      followerBrain.Die(NotificationCentre.NotificationType.Died);
  }

  public static void MakeAllFollowersPoopOrVomit()
  {
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      FollowerBrain brain = allBrain;
      if (!FollowerManager.FollowerLocked(brain.Info.ID))
        GameManager.GetInstance().StartCoroutine((IEnumerator) FollowerManager.Delay(UnityEngine.Random.Range(0.0f, 1f), (System.Action) (() =>
        {
          if (FollowerManager.FollowerLocked(brain.Info.ID))
            return;
          if ((double) UnityEngine.Random.value < 0.5)
            brain.HardSwapToTask((FollowerTask) new FollowerTask_InstantPoop());
          else
            brain.HardSwapToTask((FollowerTask) new FollowerTask_Vomit());
        })));
    }
  }

  public static void CureAllCursedFollowers()
  {
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.Info.CursedState == Thought.Dissenter)
      {
        if (allBrain.Info.CursedState == Thought.Dissenter)
          allBrain.Stats.Reeducation = 0.0f;
      }
      else if (allBrain.Info.CursedState == Thought.Ill)
      {
        allBrain.Stats.Illness = 0.0f;
        FollowerBrainStats.StatStateChangedEvent illnessStateChanged = FollowerBrainStats.OnIllnessStateChanged;
        if (illnessStateChanged != null)
          illnessStateChanged(allBrain.Info.ID, FollowerStatState.Off, FollowerStatState.On);
      }
      else if (allBrain.Info.CursedState == Thought.BecomeStarving)
        allBrain.Stats.Starvation = 0.0f;
      else if ((double) allBrain.Stats.Exhaustion > 0.0)
      {
        allBrain.Stats.Rest = 100f;
        allBrain.Stats.Exhaustion = 0.0f;
      }
    }
  }

  public static void ResurrectBurriedFollower()
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) FollowerManager.ResurrectBurriedFollowerIE());
  }

  private static IEnumerator ResurrectBurriedFollowerIE()
  {
    while (PlayerFarming.Location != FollowerLocation.Base || LetterBox.IsPlaying || MMConversation.isPlaying || SimulationManager.IsPaused || PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.Idle && PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.Moving)
      yield return (object) null;
    List<Structures_Grave> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_Grave>(FollowerLocation.Base);
    List<FollowerInfo> followerInfoList = new List<FollowerInfo>((IEnumerable<FollowerInfo>) DataManager.Instance.Followers_Dead);
    for (int index = followerInfoList.Count - 1; index >= 0; --index)
    {
      bool flag = false;
      foreach (StructureBrain structureBrain in structuresOfType)
      {
        if (structureBrain.Data.FollowerID == followerInfoList[index].ID)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        followerInfoList.RemoveAt(index);
    }
    FollowerInfo info = followerInfoList[UnityEngine.Random.Range(0, followerInfoList.Count)];
    DataManager.Instance.Followers_Dead.Remove(info);
    DataManager.Instance.Followers_Dead_IDs.Remove(info.ID);
    Structures_Grave structuresGrave1 = (Structures_Grave) null;
    foreach (Structures_Grave structuresGrave2 in structuresOfType)
    {
      if (structuresGrave2.Data.FollowerID == info.ID)
      {
        structuresGrave1 = structuresGrave2;
        break;
      }
    }
    foreach (Grave grave in Grave.Graves)
    {
      if (grave.StructureBrain.Data.FollowerID == structuresGrave1.Data.FollowerID)
      {
        structuresGrave1.Data.FollowerID = -1;
        grave.SetGameObjects();
        break;
      }
    }
    FollowerBrain resurrectingFollower = FollowerBrain.GetOrCreateBrain(info);
    resurrectingFollower.ResetStats();
    if (resurrectingFollower.Info.Age > resurrectingFollower.Info.LifeExpectancy)
      resurrectingFollower.Info.LifeExpectancy = resurrectingFollower.Info.Age + UnityEngine.Random.Range(20, 30);
    else
      resurrectingFollower.Info.LifeExpectancy += UnityEngine.Random.Range(20, 30);
    Follower revivedFollower = FollowerManager.CreateNewFollower(resurrectingFollower._directInfoAccess, structuresGrave1.Data.Position);
    resurrectingFollower.Location = FollowerLocation.Base;
    resurrectingFollower.DesiredLocation = FollowerLocation.Base;
    resurrectingFollower.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    revivedFollower.SetOutfit(FollowerOutfitType.Worker, false);
    revivedFollower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(revivedFollower.gameObject, 7f);
    yield return (object) new WaitForEndOfFrame();
    double num = (double) revivedFollower.SetBodyAnimation("Sermons/resurrect", false);
    revivedFollower.AddBodyAnimation("Reactions/react-enlightened1", false, 0.0f);
    revivedFollower.AddBodyAnimation("idle", true, 0.0f);
    yield return (object) new WaitForSeconds(6f);
    resurrectingFollower.CompleteCurrentTask();
    GameManager.GetInstance().OnConversationEnd();
  }

  public delegate void FollowerChanged(int followerID);

  public delegate void FollowerGoneEvent(
    int followerID,
    NotificationCentre.NotificationType notificationType);

  public struct SpawnedFollower
  {
    public FollowerBrain FollowerBrain;
    public FollowerBrain FollowerFakeBrain;
    public FollowerInfo FollowerFakeInfo;
    public Follower Follower;
  }
}
