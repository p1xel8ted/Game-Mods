// Decompiled with JetBrains decompiler
// Type: FollowerManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.FollowerSelect;
using MMTools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class FollowerManager
{
  public static Dictionary<FollowerLocation, List<Follower>> Followers = new Dictionary<FollowerLocation, List<Follower>>();
  public static Dictionary<FollowerLocation, List<SimFollower>> SimFollowers = new Dictionary<FollowerLocation, List<SimFollower>>();
  public static FollowerManager.FollowerChanged OnFollowerAdded;
  public static FollowerManager.FollowerChanged OnFollowerRemoved;
  public static FollowerManager.FollowerGoneEvent OnFollowerDie;
  public static FollowerManager.FollowerGoneEvent OnFollowerLeave;
  public static Follower _followerPrefab;
  public static Follower _combatFollowerPrefab;
  public static FollowerRecruit _recruitFollowerPrefab;
  public static AsyncOperationHandle<GameObject> _followerHandle;
  public const int DeathCatID = 666;
  public const int LeshyID = 99990;
  public const int HeketID = 99991;
  public const int KallamarID = 99992;
  public const int ShamuraID = 99993;
  public const int BaalID = 99994;
  public const int AymID = 99995;
  public const int SozoID = 99996;
  public const int JalalaID = 99997;
  public const int YarlenID = 99998;
  public const int RinorID = 99999;
  public const int ChosenChildID = 100000;
  public const int DaedreamID = 100001;
  public const int QuivernID = 100002;
  public const int DepressoID = 100003;
  public const int AnubisID = 100004;
  public const int ChilletID = 100005;
  public const int MidasID = 100006;
  public const int YngyaID = 100007;
  public const int Icegor = 10008;
  public const int NarayanaID = 10009;
  public const int ExecutionerID = 10010;
  public const int AbasID = 10011;
  public const int ThoasID = 10012;
  public const int GorgoID = 10013;
  public const int MestorID = 10014;
  public const int FestorID = 10015;
  public const int NestorID = 10016;
  public const int Lamb1ID = 10017;
  public const int Lamb2ID = 10018;
  public const int Lamb3ID = 10019;
  public const int Lamb4ID = 10020;
  public const int Lamb5ID = 10021;
  public static List<int> UniqueFollowerIDs = new List<int>()
  {
    666,
    99990,
    99991,
    99992,
    99993,
    99994,
    99995,
    99996,
    99997,
    99998,
    99999,
    100000,
    100006,
    100007,
    10009,
    10010,
    10011,
    10012,
    10013,
    10014,
    10015,
    10016
  };
  public static List<List<int>> SiblingIDs = new List<List<int>>()
  {
    new List<int>() { 666, 99990, 99991, 99992, 99993 },
    new List<int>() { 99994, 99995 },
    new List<int>() { 99997, 99998 },
    new List<int>() { 10017, 10018, 10019, 10020, 10021 }
  };
  public static List<int> BishopIDs = new List<int>()
  {
    99990,
    99991,
    99992,
    99993,
    666
  };
  public static List<int> PilrgrimIDs = new List<int>()
  {
    99997,
    99998,
    99999
  };
  public static List<int> PalworldIDs = new List<int>()
  {
    100001,
    100003,
    100004,
    100005,
    100002
  };
  public static List<int> LambIDs = new List<int>()
  {
    10017,
    10018,
    10019,
    10020,
    10021
  };
  public static int CopyFollowersActive = 0;

  public static Follower FollowerPrefab
  {
    get
    {
      if ((UnityEngine.Object) FollowerManager._followerPrefab == (UnityEngine.Object) null)
      {
        FollowerManager._followerHandle = Addressables.LoadAssetAsync<GameObject>((object) "Assets/Resources_moved/Prefabs/Units/Villagers/Follower.prefab");
        FollowerManager._followerHandle.WaitForCompletion();
        FollowerManager._followerPrefab = FollowerManager._followerHandle.Result.GetComponent<Follower>();
      }
      return FollowerManager._followerPrefab;
    }
  }

  public static Follower CombatFollowerPrefab
  {
    get
    {
      if ((UnityEngine.Object) FollowerManager._combatFollowerPrefab == (UnityEngine.Object) null)
        FollowerManager._combatFollowerPrefab = Resources.Load<Follower>("Prefabs/Units/Villagers/Enemy Follower");
      return FollowerManager._combatFollowerPrefab;
    }
  }

  public static FollowerRecruit RecruitPrefab
  {
    get
    {
      if ((UnityEngine.Object) FollowerManager._recruitFollowerPrefab == (UnityEngine.Object) null)
        FollowerManager._recruitFollowerPrefab = Resources.Load<FollowerRecruit>("Prefabs/Units/Villagers/Recruit Variant");
      return FollowerManager._recruitFollowerPrefab;
    }
  }

  public static string GetSpecialFollowerFallback(int followerId)
  {
    return MMConversation.GetFallBackVO(followerId);
  }

  public static IEnumerable<Follower> ActiveLocationFollowers()
  {
    foreach (FollowerLocation location in LocationManager.LocationsInState(LocationState.Active))
    {
      List<Follower> list = FollowerManager.FollowersAtLocation(location);
      for (int i = list.Count - 1; i >= 0; --i)
        yield return list[i];
      list = (List<Follower>) null;
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
    DataManager.TryGetFollowerInfoByID(in ID, out followerInfo);
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

  public static bool FollowerLocked(
    in int ID,
    in bool exludeStarving = false,
    in bool exludeChild = false,
    in bool excludeFreezing = false,
    in bool excludeLightningTargeted = true,
    in bool excludeHotTub = false)
  {
    FollowerInfo infoById = FollowerInfo.GetInfoByID(ID);
    FollowerBrain brain = infoById != null ? FollowerBrain.GetOrCreateBrain(infoById) : (FollowerBrain) null;
    if (infoById == null)
      return true;
    if (excludeLightningTargeted)
    {
      Follower followerById = FollowerManager.FindFollowerByID(ID);
      if ((UnityEngine.Object) followerById != (UnityEngine.Object) null && (UnityEngine.Object) followerById.Interaction_FollowerInteraction != (UnityEngine.Object) null && followerById.Interaction_FollowerInteraction.LightningIncoming)
        return true;
    }
    if (excludeHotTub)
    {
      Follower followerById = FollowerManager.FindFollowerByID(ID);
      foreach (Interaction_VolcanicSpa healingBay in Interaction_VolcanicSpa.HealingBays)
      {
        if ((UnityEngine.Object) healingBay != (UnityEngine.Object) null && healingBay.currentSpaOccupants != null && healingBay.currentSpaOccupants.Contains(followerById))
          return true;
      }
    }
    if (infoById.LeavingCult)
      return true;
    int num1 = exludeStarving ? 0 : (infoById.CursedState == Thought.BecomeStarving ? 1 : 0);
    bool flag1 = !excludeFreezing && infoById.CursedState == Thought.Freezing;
    bool flag2 = !exludeChild && infoById.CursedState == Thought.Child;
    int num2 = flag1 ? 1 : 0;
    return (num1 | num2 | (flag2 ? 1 : 0)) != 0 || ((brain.CurrentTaskType == FollowerTaskType.Floating ? 1 : 0) | (infoById.CursedState == Thought.Child ? (false ? 1 : 0) : (brain.HasTrait(FollowerTrait.TraitType.ExistentialDread) ? (true ? 1 : 0) : (brain.HasTrait(FollowerTrait.TraitType.MissionaryTerrified) ? 1 : 0)))) != 0 || (DataManager.Instance.Followers_Imprisoned_IDs.Contains(ID) || DataManager.Instance.Followers_TraitManipulating_IDs.Contains(ID) || DataManager.Instance.Followers_OnMissionary_IDs.Contains(ID) || DataManager.Instance.Followers_Demons_IDs.Contains(ID) || DataManager.Instance.Followers_Transitioning_IDs.Contains(ID) ? 1 : (DataManager.Instance.Followers_LeftInTheDungeon_IDs.Contains(ID) ? 1 : 0)) != 0 || DataManager.Instance.Followers_Recruit.Contains(infoById);
  }

  public static FollowerSelectEntry.Status GetFollowerAvailabilityStatus(
    FollowerBrain brain,
    int minFollowerLevel,
    bool excludeStarving = false,
    bool excludeDisciples = false)
  {
    FollowerSelectEntry.Status availabilityStatus = FollowerManager.GetFollowerAvailabilityStatus(brain._directInfoAccess, excludeStarving);
    if (brain._directInfoAccess.XPLevel < minFollowerLevel)
      availabilityStatus = FollowerSelectEntry.Status.UnavailableLowLevel;
    else if (excludeDisciples && brain.Info.IsDisciple)
      availabilityStatus = FollowerSelectEntry.Status.UnavailableAlreadyDisciple;
    return availabilityStatus;
  }

  public static FollowerSelectEntry.Status GetFollowerAvailabilityStatus(
    Follower follower,
    bool excludeStarving = false)
  {
    return FollowerManager.GetFollowerAvailabilityStatus(follower.Brain._directInfoAccess, excludeStarving);
  }

  public static FollowerSelectEntry.Status GetFollowerAvailabilityStatus(
    int id,
    bool excludeStarving = false)
  {
    return FollowerManager.GetFollowerAvailabilityStatus(FollowerInfo.GetInfoByID(id), excludeStarving);
  }

  public static FollowerSelectEntry.Status GetFollowerAvailabilityStatus(
    FollowerInfo followerInfo,
    bool excludeStarving = false,
    bool excludeChildren = false)
  {
    if (followerInfo != null)
    {
      FollowerBrain brain = FollowerBrain.GetOrCreateBrain(followerInfo);
      if (followerInfo.CursedState == Thought.Child && !excludeChildren)
        return FollowerSelectEntry.Status.UnavailableChild;
      if (DataManager.Instance.Followers_Imprisoned_IDs.Contains(followerInfo.ID))
        return FollowerSelectEntry.Status.UnavailableImprisoned;
      if (DataManager.Instance.Followers_TraitManipulating_IDs.Contains(followerInfo.ID))
        return FollowerSelectEntry.Status.UnavailableTraitManipulating;
      if (DataManager.Instance.Followers_OnMissionary_IDs.Contains(followerInfo.ID))
        return FollowerSelectEntry.Status.UnavailableMissionary;
      if (DataManager.Instance.Followers_Demons_IDs.Contains(followerInfo.ID))
        return FollowerSelectEntry.Status.UnavailableDemon;
      if (DataManager.Instance.Followers_Transitioning_IDs.Contains(followerInfo.ID))
        return FollowerSelectEntry.Status.Unavailable;
      if (DataManager.Instance.Followers_Recruit.Contains(followerInfo))
        return FollowerSelectEntry.Status.UnavailableNewRecruit;
      if (DataManager.Instance.Followers_LeftInTheDungeon_IDs.Contains(followerInfo.ID) || followerInfo.CursedState == Thought.BecomeStarving && !excludeStarving || followerInfo.LeavingCult)
        return FollowerSelectEntry.Status.Unavailable;
      if (brain.CurrentTaskType == FollowerTaskType.FindPlaceToDie || brain.CurrentTaskType == FollowerTaskType.EnforcerManualControl || brain.CurrentTaskType == FollowerTaskType.ManualControl)
        return FollowerSelectEntry.Status.UnavailableBusy;
    }
    return FollowerSelectEntry.Status.Available;
  }

  public static FollowerSelectEntry.Status GetFollowerAvailabilityStatus(
    FollowerBrain followerBrain,
    bool excludeStarving = false)
  {
    FollowerSelectEntry.Status availabilityStatus = FollowerManager.GetFollowerAvailabilityStatus(followerBrain._directInfoAccess, excludeStarving);
    return followerBrain != null && availabilityStatus == FollowerSelectEntry.Status.Available && followerBrain.CurrentTaskType != FollowerTaskType.Zombie && followerBrain.CurrentTaskType != FollowerTaskType.ExistentialDread && followerBrain.CurrentTask != null && followerBrain.CurrentTask.BlockThoughts ? FollowerSelectEntry.Status.UnavailableBusy : availabilityStatus;
  }

  public static FollowerSelectEntry.Status GetFollowerCursedStateAvailability(
    FollowerBrain followerBrain)
  {
    return FollowerManager.GetFollowerCursedStateAvailability(followerBrain._directInfoAccess);
  }

  public static FollowerSelectEntry.Status GetFollowerCursedStateAvailability(Follower follower)
  {
    return FollowerManager.GetFollowerCursedStateAvailability(follower.Brain._directInfoAccess);
  }

  public static FollowerSelectEntry.Status GetFollowerCursedStateAvailability(
    FollowerInfo followerInfo)
  {
    switch (followerInfo.CursedState)
    {
      case Thought.Dissenter:
        return FollowerSelectEntry.Status.UnavailableDissenting;
      case Thought.Ill:
        return FollowerSelectEntry.Status.UnavailableIll;
      case Thought.BecomeStarving:
        return FollowerSelectEntry.Status.UnavailableStarving;
      case Thought.OldAge:
        return FollowerSelectEntry.Status.UnavailableElderly;
      case Thought.Child:
        return FollowerSelectEntry.Status.UnavailableChild;
      case Thought.Freezing:
        return FollowerSelectEntry.Status.UnavailableFreezing;
      default:
        return FollowerSelectEntry.Status.Unavailable;
    }
  }

  public static void ConsumeFollower(int ID)
  {
    FollowerInfo infoById = FollowerInfo.GetInfoByID(ID);
    if (infoById != null)
      DataManager.Instance.Followers_Dead.Insert(0, infoById);
    DataManager.Instance.Followers_Dead_IDs.Insert(0, infoById.ID);
    FollowerManager.CompleteKillFollowerObjective(ID);
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
        if (FollowerManager.IsHateElderlyTraitActiveWithElderlyFollower(ID) && !DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.LoveElderly))
        {
          CultFaithManager.AddThought(Thought.Cult_HateElderly_Trait_Cons);
          goto case NotificationCentre.NotificationType.Ascended;
        }
        goto case NotificationCentre.NotificationType.Ascended;
      case NotificationCentre.NotificationType.DiedFromIllness:
        CultFaithManager.AddThought(Thought.DiedFromIllness, ID);
        goto case NotificationCentre.NotificationType.Ascended;
      case NotificationCentre.NotificationType.SacrificeFollower:
        if (FollowerManager.IsHateElderlyTraitActiveWithElderlyFollower(ID))
        {
          CultFaithManager.AddThought(Thought.Cult_HateElderly_Trait_Pros);
          goto case NotificationCentre.NotificationType.Ascended;
        }
        goto case NotificationCentre.NotificationType.Ascended;
      case NotificationCentre.NotificationType.MurderedByYou:
        bool flag = false;
        foreach (ObjectivesData completedObjective in DataManager.Instance.CompletedObjectives)
        {
          if (completedObjective is Objectives_Custom && completedObjective.Follower == ID && (((Objectives_Custom) completedObjective).CustomQuestType == Objectives.CustomQuestTypes.MurderFollower || ((Objectives_Custom) completedObjective).CustomQuestType == Objectives.CustomQuestTypes.KillFollower || ((Objectives_Custom) completedObjective).CustomQuestType == Objectives.CustomQuestTypes.KillFollowersSpouse || ((Objectives_Custom) completedObjective).CustomQuestType == Objectives.CustomQuestTypes.MurderFollowerAtNight && TimeManager.IsNight))
            flag = true;
        }
        FollowerInfo infoById1 = FollowerInfo.GetInfoByID(ID, true);
        if (infoById1 != null && infoById1.Traits.Contains(FollowerTrait.TraitType.Spy))
          flag = true;
        if (DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.RejectRot) && infoById1.Traits.Contains(FollowerTrait.TraitType.Mutated))
          CultFaithManager.AddThought(Thought.Cult_MurderedRotFollower, ID, flag ? 0.0f : 1f);
        else if (TimeManager.IsNight)
        {
          int num = 0;
          foreach (Follower follower in Follower.Followers)
          {
            if (!((UnityEngine.Object) follower == (UnityEngine.Object) null) && follower.Brain != null && follower.Brain.CurrentTask != null && (follower.Brain.CurrentTaskType == FollowerTaskType.Sleep || follower.Brain.CurrentTaskType == FollowerTaskType.SleepBedRest) && follower.Brain.CurrentTask.State == FollowerTaskState.Doing)
              ++num;
          }
          if (num >= Follower.Followers.Count - 1)
            CultFaithManager.AddThought(Thought.Cult_MurderAtNightNoWitnesses, ID, flag ? 0.0f : 1f);
          else
            CultFaithManager.AddThought(Thought.Cult_MurderAtNightFewWitnesses, ID, flag ? 0.0f : 1f);
        }
        else
          CultFaithManager.AddThought(Thought.Cult_Murder, ID, flag ? 0.0f : 1f);
        if (FollowerManager.IsHateElderlyTraitActiveWithElderlyFollower(ID))
        {
          CultFaithManager.AddThought(Thought.Cult_HateElderly_Trait_Pros);
          goto case NotificationCentre.NotificationType.Ascended;
        }
        goto case NotificationCentre.NotificationType.Ascended;
      case NotificationCentre.NotificationType.DiedFromOldAge:
        ++DataManager.Instance.STATS_NaturalDeaths;
        CultFaithManager.AddThought(Thought.Cult_FollowerDiedOfOldAge, ID);
        if (FollowerManager.IsHateElderlyTraitActiveWithElderlyFollower(ID) && !DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.LoveElderly))
        {
          CultFaithManager.AddThought(Thought.Cult_HateElderly_Trait_Cons);
          goto case NotificationCentre.NotificationType.Ascended;
        }
        goto case NotificationCentre.NotificationType.Ascended;
      case NotificationCentre.NotificationType.Ascended:
      case NotificationCentre.NotificationType.MurderedByFollower:
      case NotificationCentre.NotificationType.DiedFromBeingEatenBySozo:
      case NotificationCentre.NotificationType.DiedFromBeingEaten:
      case NotificationCentre.NotificationType.MeltedToDeath:
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
                  allBrain.AddThought(Thought.LeaderMurderedAFollowerHateElderly);
                else if (allBrain.HasTrait(FollowerTrait.TraitType.LoveElderly) && DataManager.Instance.Followers_Elderly_IDs.Contains(ID))
                  allBrain.AddThought(Thought.LeaderMurderedAFollowerLoveElderly);
                else
                  allBrain.AddThought(Thought.LeaderMurderedAFollower);
                if (allBrain.HasTrait(FollowerTrait.TraitType.Spy))
                {
                  CultFaithManager.AddThought(Thought.MurderedSpy, allBrain.Info.ID);
                  continue;
                }
                continue;
              case NotificationCentre.NotificationType.Ascended:
                allBrain.AddThought(Thought.FollowerAscended);
                continue;
              case NotificationCentre.NotificationType.KilledInAFightPit:
                allBrain.AddThought(Thought.FightPitExecution);
                continue;
              case NotificationCentre.NotificationType.FrozeToDeath:
              case NotificationCentre.NotificationType.MeltedToDeath:
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
          FollowerManager.CompleteKillFollowerObjective(ID);
        ObjectiveManager.FailCustomObjective(Objectives.CustomQuestTypes.FollowerRecoverIllness, ID);
        if (followerInfo != null && deathNotificationType != NotificationCentre.NotificationType.DiedFromBeingEatenBySozo && deathNotificationType != NotificationCentre.NotificationType.MeltedToDeath && !followerInfo.IsSnowman)
        {
          DataManager.Instance.Followers_Dead.Insert(0, followerInfo);
          DataManager.Instance.Followers_Dead_IDs.Insert(0, followerInfo.ID);
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
        if (followerInfo.CursedState == Thought.Zombie)
        {
          ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.ZombieExists);
          foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
          {
            if (allBrain.Info.CursedState != Thought.Zombie)
              allBrain.AddThought(Thought.ZombieDied);
          }
        }
        TwitchFollowers.SendFollowers();
        break;
      case NotificationCentre.NotificationType.KilledInAFightPit:
      case NotificationCentre.NotificationType.ConsumeFollower:
        if (FollowerManager.IsHateElderlyTraitActiveWithElderlyFollower(ID))
        {
          CultFaithManager.AddThought(Thought.Cult_HateElderly_Trait_Pros);
          goto case NotificationCentre.NotificationType.Ascended;
        }
        goto case NotificationCentre.NotificationType.Ascended;
      case NotificationCentre.NotificationType.FrozeToDeath:
        FollowerInfo infoById2 = FollowerInfo.GetInfoByID(ID, true);
        AnalyticsLogger.LogEvent(AnalyticsLogger.EventType.None, "Froze to Death", $"{infoById2.Name} {infoById2.SkinName}", DataManager.Instance.Followers.Count.ToString(), "");
        CultFaithManager.AddThought(Thought.Cult_FrozeToDeath, ID);
        goto case NotificationCentre.NotificationType.Ascended;
      default:
        if (DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.DesensitisedToDeath))
          CultFaithManager.AddThought(Thought.Cult_FollowerDied_Trait, ID);
        else
          CultFaithManager.AddThought(Thought.Cult_FollowerDied, ID);
        if (FollowerManager.IsHateElderlyTraitActiveWithElderlyFollower(ID) && !DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.LoveElderly) && deathNotificationType != NotificationCentre.NotificationType.DiedFromDeadlyMeal)
        {
          CultFaithManager.AddThought(Thought.Cult_HateElderly_Trait_Cons);
          goto case NotificationCentre.NotificationType.Ascended;
        }
        goto case NotificationCentre.NotificationType.Ascended;
    }
  }

  public static bool IsHateElderlyTraitActiveWithElderlyFollower(int followerID)
  {
    FollowerInfo infoById = FollowerInfo.GetInfoByID(followerID, true);
    return (infoById != null ? (infoById.CursedState == Thought.OldAge ? 1 : 0) : 0) != 0 && DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.HateElderly);
  }

  public static void CompleteKillFollowerObjective(int followerID)
  {
    ObjectiveManager.FailLockCustomObjective(Objectives.CustomQuestTypes.KillFollower, true);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.KillFollower, followerID);
    ObjectiveManager.FailLockCustomObjective(Objectives.CustomQuestTypes.KillFollower, false);
    ObjectiveManager.FailLockCustomObjective(Objectives.CustomQuestTypes.KillFollowersSpouse, true);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.KillFollowersSpouse, followerID);
    ObjectiveManager.FailLockCustomObjective(Objectives.CustomQuestTypes.KillFollowersSpouse, false);
  }

  public static void RemoveFollowerBrain(int ID)
  {
    FollowerInfo followerInfo = FollowerManager.RemoveFollower(ID);
    if (followerInfo != null)
    {
      FollowerBrain brainById = FollowerBrain.FindBrainByID(followerInfo.ID);
      if (brainById != null)
      {
        brainById.Cleanup();
        brainById.ClearDwelling();
      }
      DataManager.Instance.Followers_Imprisoned_IDs.Remove(followerInfo.ID);
      DataManager.Instance.Followers_OnMissionary_IDs.Remove(ID);
      DataManager.Instance.Followers_LeftInTheDungeon_IDs.Remove(ID);
      DataManager.Instance.Followers_Elderly_IDs.Remove(followerInfo.ID);
      DataManager.Instance.Followers_Transitioning_IDs.Remove(followerInfo.ID);
      DataManager.Instance.Followers_TraitManipulating_IDs.Remove(followerInfo.ID);
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

  public static int GetSpyCount()
  {
    int spyCount = 0;
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.HasTrait(FollowerTrait.TraitType.Spy))
        ++spyCount;
    }
    return spyCount;
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
    for (int location = 0; location < 98; ++location)
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
          try
          {
            if (!follower.IsPaused || !((UnityEngine.Object) follower.gameObject != (UnityEngine.Object) null))
              return;
            UnityEngine.Object.Destroy((UnityEngine.Object) follower.gameObject);
            return;
          }
          catch (Exception ex)
          {
            Debug.Log((object) ex);
            return;
          }
        }
      }
    }
  }

  public static void RetireSimFollowerByID(int followerID)
  {
    for (int location = 0; location < 98; ++location)
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
    Follower followerPrefab,
    Vector3 position,
    Transform parent,
    FollowerLocation location)
  {
    FollowerInfo followerInfo = FollowerInfo.NewCharacter(location);
    return FollowerManager.SpawnCopyFollower(followerPrefab, followerInfo, position, parent, location);
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
    spawnedFollower.FollowerFakeInfo = FollowerInfo.NewCharacter(location, followerInfo.SkinName);
    spawnedFollower.FollowerFakeInfo.IsFakeBrain = true;
    spawnedFollower.FollowerFakeInfo.ViewerID = followerInfo.ViewerID;
    spawnedFollower.FollowerFakeInfo.Name = followerInfo.Name;
    spawnedFollower.FollowerFakeInfo.Pets = followerInfo.Pets;
    spawnedFollower.FollowerFakeInfo.Outfit = followerInfo.Outfit;
    spawnedFollower.FollowerFakeInfo.Clothing = followerInfo.Clothing;
    spawnedFollower.FollowerFakeInfo.Customisation = followerInfo.Customisation;
    spawnedFollower.FollowerFakeInfo.Hat = followerInfo.Hat;
    spawnedFollower.FollowerFakeInfo.Necklace = followerInfo.Necklace;
    spawnedFollower.FollowerFakeInfo.Special = followerInfo.Special;
    spawnedFollower.FollowerFakeInfo.ClothingVariant = followerInfo.ClothingVariant;
    spawnedFollower.FollowerFakeInfo.SkinColour = followerInfo.SkinColour;
    spawnedFollower.FollowerFakeInfo.CursedState = followerInfo.CursedState;
    spawnedFollower.FollowerFakeInfo.Traits = followerInfo.Traits;
    spawnedFollower.FollowerFakeInfo.TraitsSet = followerInfo.TraitsSet;
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
    spawnedFollower.Follower.CopyFollowerConfigure();
    if ((bool) (UnityEngine.Object) spawnedFollower.Follower.Interaction_FollowerInteraction && (bool) (UnityEngine.Object) spawnedFollower.Follower.Interaction_FollowerInteraction.eventListener)
      spawnedFollower.Follower.Interaction_FollowerInteraction.eventListener.SetPitchAndVibrator(followerInfo.follower_pitch, followerInfo.follower_vibrato, followerInfo.ID);
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
    for (int location = 0; location < 98; ++location)
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
    for (int location = 0; location < 98; ++location)
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

  public static List<FollowerInfo> FindFollowersByID(List<int> ids)
  {
    List<FollowerInfo> followersById = new List<FollowerInfo>();
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
    {
      if (ids.Contains(follower.ID))
        followersById.Add(follower);
    }
    return followersById;
  }

  public static SimFollower FindSimFollowerByID(int ID)
  {
    SimFollower simFollowerById = (SimFollower) null;
    for (int location = 0; location < 98; ++location)
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

  public static FollowerRecruit SpawnExistingRecruit(Vector3 position)
  {
    return DataManager.Instance.Followers_Recruit.Count > 0 ? FollowerManager.SpawnRecruit(DataManager.Instance.Followers_Recruit[0], position) : (FollowerRecruit) null;
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

  public static FollowerBrain GetRandomNonLockedFollower()
  {
    List<FollowerBrain> followerBrainList = new List<FollowerBrain>();
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (!FollowerManager.FollowerLocked(allBrain.Info.ID))
        followerBrainList.Add(allBrain);
    }
    return followerBrainList.Count > 0 ? followerBrainList[UnityEngine.Random.Range(0, followerBrainList.Count)] : (FollowerBrain) null;
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

  public static FollowerRecruit SpawnRecruit(FollowerInfo f, Vector3 position, bool Force = false)
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

  public static void AddFollower(FollowerInfo f, Vector3 position)
  {
    if (f == null)
      return;
    DataManager.Instance.Followers.Add(f);
    DataManager.Instance.Followers.Sort((Comparison<FollowerInfo>) ((a, b) => a.ID.CompareTo(b.ID)));
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
      follower2.EnsureWithinBounds();
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
        FollowerBrain.RemoveBrain(followerBrain.Info.ID, true);
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
      if (FollowerManager.FollowerLocked(followerBrainList[index].Info.ID) || followerBrainList[index].Info.CursedState != Thought.None || curseType == Thought.BecomeStarving && followerBrainList[index].HasTrait(FollowerTrait.TraitType.DontStarve) || curseType == Thought.Dissenter && (followerBrainList[index].Info.Necklace == InventoryItem.ITEM_TYPE.Necklace_Loyalty || followerBrainList[index].Info.HasTrait(FollowerTrait.TraitType.BishopOfCult)))
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
          if (FollowerManager.FollowerLocked(brain.Info.ID) || brain.CurrentTaskType == FollowerTaskType.Sleep || brain.CurrentTaskType == FollowerTaskType.SleepBedRest)
            return;
          if (brain.Info.Necklace != InventoryItem.ITEM_TYPE.Necklace_5)
            brain.Stats.Rest = 21f;
          brain.HardSwapToTask((FollowerTask) new FollowerTask_Sleep(true, true));
          brain.ShouldReconsiderTask = false;
        })));
    }
  }

  public static IEnumerator Delay(float delay, System.Action callback)
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
        allBrain.Stats.Reeducation = 0.0f;
        allBrain.RemoveCurseState(Thought.Dissenter);
        FollowerBrainStats.StatStateChangedEvent reeducationStateChanged = FollowerBrainStats.OnReeducationStateChanged;
        if (reeducationStateChanged != null)
          reeducationStateChanged(allBrain.Info.ID, FollowerStatState.Off, FollowerStatState.On);
      }
      else if (allBrain.Info.CursedState == Thought.Ill)
      {
        allBrain.Stats.Illness = 0.0f;
        allBrain.RemoveCurseState(Thought.Ill);
        FollowerBrainStats.StatStateChangedEvent illnessStateChanged = FollowerBrainStats.OnIllnessStateChanged;
        if (illnessStateChanged != null)
          illnessStateChanged(allBrain.Info.ID, FollowerStatState.Off, FollowerStatState.On);
      }
      else if (allBrain.Info.CursedState == Thought.BecomeStarving)
      {
        allBrain.Stats.Starvation = 0.0f;
        allBrain.RemoveCurseState(Thought.BecomeStarving);
        FollowerBrainStats.StatStateChangedEvent starvationStateChanged = FollowerBrainStats.OnStarvationStateChanged;
        if (starvationStateChanged != null)
          starvationStateChanged(allBrain.Info.ID, FollowerStatState.Off, FollowerStatState.On);
      }
      else if (allBrain.Info.CursedState == Thought.Freezing)
      {
        allBrain.Stats.Freezing = 0.0f;
        allBrain.RemoveCurseState(Thought.Freezing);
        FollowerBrainStats.StatStateChangedEvent freezingStateChanged = FollowerBrainStats.OnFreezingStateChanged;
        if (freezingStateChanged != null)
          freezingStateChanged(allBrain.Info.ID, FollowerStatState.Off, FollowerStatState.On);
      }
      else if ((double) allBrain.Stats.Exhaustion > 0.0)
      {
        allBrain.Stats.Rest = 100f;
        allBrain.Stats.Exhaustion = 0.0f;
      }
      else if ((double) allBrain.Stats.Drunk > 0.0)
        allBrain.Stats.Drunk = 0.0f;
      else if ((double) allBrain.Stats.Injured > 0.0)
      {
        allBrain.Stats.Injured = 0.0f;
        FollowerBrainStats.StatStateChangedEvent injuredStateChanged = FollowerBrainStats.OnInjuredStateChanged;
        if (injuredStateChanged != null)
          injuredStateChanged(allBrain.Info.ID, FollowerStatState.Off, FollowerStatState.On);
      }
    }
  }

  public static void ResurrectBurriedFollower()
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) FollowerManager.ResurrectBurriedFollowerIE());
  }

  public static IEnumerator ResurrectBurriedFollowerIE()
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
      if (grave.structureBrain.Data.FollowerID == structuresGrave1.Data.FollowerID)
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
    revivedFollower.SetOutfit(FollowerOutfitType.Follower, false);
    revivedFollower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    revivedFollower.State.LockStateChanges = true;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(revivedFollower.gameObject, 7f);
    yield return (object) new WaitForEndOfFrame();
    double num = (double) revivedFollower.SetBodyAnimation("Sermons/resurrect", false);
    revivedFollower.AddBodyAnimation("Reactions/react-enlightened1", false, 0.0f);
    revivedFollower.AddBodyAnimation("idle", true, 0.0f);
    yield return (object) new WaitForSeconds(6f);
    revivedFollower.State.LockStateChanges = false;
    resurrectingFollower.CompleteCurrentTask();
    GameManager.GetInstance().OnConversationEnd();
  }

  public static IEnumerator ResurrectFollower(
    Follower follower,
    string resurectAnimation = null,
    string overrideIdleAnimation = null)
  {
    FollowerBrain resurrectingFollower = follower.Brain;
    resurrectingFollower.ResetStats();
    if (resurrectingFollower.Info.Age > resurrectingFollower.Info.LifeExpectancy)
      resurrectingFollower.Info.LifeExpectancy = resurrectingFollower.Info.Age + UnityEngine.Random.Range(20, 30);
    else
      resurrectingFollower.Info.LifeExpectancy += UnityEngine.Random.Range(20, 30);
    resurrectingFollower.Location = FollowerLocation.Base;
    resurrectingFollower.DesiredLocation = FollowerLocation.Base;
    resurrectingFollower.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    follower.SetOutfit(FollowerOutfitType.Follower, false);
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    follower.State.LockStateChanges = true;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(follower.gameObject, 7f);
    yield return (object) new WaitForEndOfFrame();
    if (string.IsNullOrEmpty(resurectAnimation))
    {
      double num = (double) follower.SetBodyAnimation("resurrect", false);
      if (!string.IsNullOrEmpty(overrideIdleAnimation))
        follower.AddBodyAnimation("Reactions/react-enlightened1", false, 0.0f);
    }
    else
    {
      double num1 = (double) follower.SetBodyAnimation(resurectAnimation, false);
    }
    if (!string.IsNullOrEmpty(overrideIdleAnimation))
      follower.AddBodyAnimation(overrideIdleAnimation, true, 0.0f);
    else
      follower.AddBodyAnimation("idle", true, 0.0f);
    yield return (object) new WaitForSeconds(6f);
    follower.State.LockStateChanges = false;
    resurrectingFollower.CompleteCurrentTask();
    GameManager.GetInstance().OnConversationEnd();
  }

  public static FollowerBrain MakeFollowerGainLevel()
  {
    List<FollowerBrain> followerBrainList = new List<FollowerBrain>((IEnumerable<FollowerBrain>) FollowerBrain.AllBrains);
    for (int index = followerBrainList.Count - 1; index >= 0; --index)
    {
      if (FollowerManager.FollowerLocked(followerBrainList[index].Info.ID) || followerBrainList[index].CanLevelUp())
        followerBrainList.RemoveAt(index);
    }
    FollowerBrain followerBrain = followerBrainList.Count > 0 ? followerBrainList[UnityEngine.Random.Range(0, followerBrainList.Count)] : (FollowerBrain) null;
    followerBrain?.AddAdoration(FollowerBrain.AdorationActions.LevelUp, (System.Action) null);
    return followerBrain;
  }

  public static FollowerBrain MakeFollowerLoseLevel()
  {
    List<FollowerBrain> followerBrainList = new List<FollowerBrain>((IEnumerable<FollowerBrain>) FollowerBrain.AllBrains);
    for (int index = followerBrainList.Count - 1; index >= 0; --index)
    {
      if (FollowerManager.FollowerLocked(followerBrainList[index].Info.ID) || followerBrainList[index].Info.XPLevel <= 1)
        followerBrainList.RemoveAt(index);
    }
    FollowerBrain followerBrain = followerBrainList.Count > 0 ? followerBrainList[UnityEngine.Random.Range(0, followerBrainList.Count)] : (FollowerBrain) null;
    followerBrain?.LevelDown();
    return followerBrain;
  }

  public static bool AreSiblings(int followerID_1, int followerID_2)
  {
    foreach (List<int> siblingId in FollowerManager.SiblingIDs)
    {
      if (siblingId.Contains(followerID_1) && siblingId.Contains(followerID_2))
        return true;
    }
    FollowerInfo infoById1 = FollowerInfo.GetInfoByID(followerID_1, true);
    FollowerInfo infoById2 = FollowerInfo.GetInfoByID(followerID_2, true);
    if (infoById1 == null || infoById2 == null)
      return false;
    return infoById1.Siblings.Contains(infoById2.ID) || infoById2.Siblings.Contains(infoById1.ID);
  }

  public static bool IsChildOf(int followerID_1, int followerID_2)
  {
    FollowerInfo infoById = FollowerInfo.GetInfoByID(followerID_1, true);
    FollowerInfo.GetInfoByID(followerID_2, true);
    return infoById != null && infoById.Parents.Contains(followerID_2);
  }

  public static bool IsChild(int followerID)
  {
    FollowerInfo infoById = FollowerInfo.GetInfoByID(followerID, true);
    return infoById != null && (infoById.SkinName.Contains("Webber") || infoById.CursedState == Thought.Child || infoById.Age < 18);
  }

  public static bool AreRelated(int followerID_1, int followerID_2)
  {
    return FollowerManager.AreSiblings(followerID_1, followerID_2) || FollowerManager.IsChildOf(followerID_1, followerID_2) || FollowerManager.IsChildOf(followerID_2, followerID_1);
  }

  public static int GetPossibleQuestFollowerID(List<int> excludeList = null, StoryDataItem storyQuest = null)
  {
    List<int> ts = new List<int>();
    foreach (Follower follower in FollowerManager.FollowersAtLocation(FollowerLocation.Base))
    {
      if ((excludeList != null && !excludeList.Contains(follower.Brain.Info.ID) || excludeList == null) && !FollowerManager.UniqueFollowerIDs.Contains(follower.Brain.Info.ID))
        ts.Add(follower.Brain.Info.ID);
    }
    ts.Shuffle<int>();
    if (storyQuest != null)
    {
      for (int index = 0; index < ts.Count; ++index)
      {
        if (Quests.IsFollowerValidForStoryQuest(storyQuest, ts[index], true) || Quests.IsFollowerValidForStoryQuest(storyQuest, ts[index], false))
          return ts[index];
      }
    }
    else if (ts.Count > 0)
      return ts[0];
    return -1;
  }

  public static void TryStopFollowerFight(int participantID0, int participantID1)
  {
    List<FollowerBrain> followerBrainList = new List<FollowerBrain>();
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (!FollowerManager.FollowerLocked(in allBrain._directInfoAccess.ID) && allBrain.Info.CursedState == Thought.None && allBrain.Location == FollowerLocation.Base && allBrain.HasTrait(FollowerTrait.TraitType.Pacifist) && (allBrain.CurrentTask == null || !allBrain.CurrentTask.BlockTaskChanges))
        followerBrainList.Add(allBrain);
    }
    FollowerBrain followerBrain1 = (FollowerBrain) null;
    float num1 = float.PositiveInfinity;
    FollowerBrain brainById = FollowerBrain.FindBrainByID(participantID0);
    if (brainById.CurrentTaskType != FollowerTaskType.FightFollower)
      return;
    Vector3 chatPosition = ((FollowerTask_FightFollower) brainById.CurrentTask).ChatPosition;
    if (PlayerFarming.Location == FollowerLocation.Base)
    {
      foreach (FollowerBrain followerBrain2 in followerBrainList)
      {
        Follower followerById = FollowerManager.FindFollowerByID(followerBrain2.Info.ID);
        if ((UnityEngine.Object) followerById != (UnityEngine.Object) null)
        {
          float num2 = Vector2.Distance((Vector2) chatPosition, (Vector2) followerById.transform.position);
          if ((double) num2 < (double) num1)
          {
            num1 = num2;
            followerBrain1 = followerBrain2;
          }
        }
        else
        {
          float num3 = Vector3.Distance(chatPosition, followerBrain2.LastPosition);
          if ((double) num3 < (double) num1)
          {
            num1 = num3;
            followerBrain1 = followerBrain2;
          }
        }
      }
    }
    else
    {
      foreach (FollowerBrain followerBrain3 in followerBrainList)
      {
        float num4 = Vector3.Distance(chatPosition, followerBrain3.LastPosition);
        if ((double) num4 < (double) num1)
        {
          num1 = num4;
          followerBrain1 = followerBrain3;
        }
      }
    }
    followerBrain1?.HardSwapToTask((FollowerTask) new FollowerTask_BreakUpFight(participantID0, participantID1));
  }

  public delegate void FollowerChanged(int followerID);

  public delegate void FollowerGoneEvent(
    int followerID,
    NotificationCentre.NotificationType notificationType);

  public class SpawnedFollower
  {
    public FollowerBrain FollowerBrain;
    public FollowerBrain FollowerFakeBrain;
    public FollowerInfo FollowerFakeInfo;
    public Follower Follower;
  }
}
