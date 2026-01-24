// Decompiled with JetBrains decompiler
// Type: UnlockManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;

#nullable disable
public class UnlockManager : BaseMonoBehaviour
{
  public HUD_ProblemUnlock ProblemUnlockPrefab;

  public void OnEnable()
  {
    foreach (object type in Enum.GetValues(typeof (UnlockManager.UnlockType)))
    {
      if (!DataManager.Instance.MechanicsUnlocked.Contains((UnlockManager.UnlockType) type))
        this.TriggerProblemUnlock((UnlockManager.UnlockType) type);
    }
  }

  public void OnDisable()
  {
  }

  public void OnNewDayStarted()
  {
    if (DataManager.Instance.CurrentDayIndex < 1)
      return;
    this.TriggerProblemUnlock(UnlockManager.UnlockType.Starting);
    TimeManager.OnNewDayStarted -= new System.Action(this.OnNewDayStarted);
  }

  public void OnIllnessChanged(Villager_Info.StatusState state)
  {
    if (state != Villager_Info.StatusState.On)
      return;
    this.TriggerProblemUnlock(UnlockManager.UnlockType.Illness);
    Villager_Info.OnIllnessChanged -= new Villager_Info.StatusEffectEvent(this.OnIllnessChanged);
  }

  public void OnDissenterChanged(Villager_Info.StatusState state)
  {
    if (state != Villager_Info.StatusState.On)
      return;
    this.TriggerProblemUnlock(UnlockManager.UnlockType.Dissenters);
    Villager_Info.OnDissenterChanged -= new Villager_Info.StatusEffectEvent(this.OnDissenterChanged);
  }

  public void OnStarveChanged(Villager_Info.StatusState state)
  {
    if (state != Villager_Info.StatusState.On)
      return;
    this.TriggerProblemUnlock(UnlockManager.UnlockType.Hunger);
    Villager_Info.OnStarveChanged -= new Villager_Info.StatusEffectEvent(this.OnStarveChanged);
  }

  public void OnWorshipperDied()
  {
    this.TriggerProblemUnlock(UnlockManager.UnlockType.FollowerDeath);
    WorshipperInfoManager.OnWorshipperDied -= new System.Action(this.OnWorshipperDied);
  }

  public void TriggerProblemUnlock(UnlockManager.UnlockType type)
  {
    DataManager.Instance.MechanicsUnlocked.Add(type);
    foreach (UnlockManager.UnlockNotificationData unlockNotification in UnlockManager.GetUnlockNotifications(type))
    {
      if (unlockNotification.SermonRitualType != SermonsAndRituals.SermonRitualType.NONE && !DataManager.Instance.UnlockedSermonsAndRituals.Contains(unlockNotification.SermonRitualType))
        DataManager.Instance.UnlockedSermonsAndRituals.Add(unlockNotification.SermonRitualType);
      if (unlockNotification.StructureType != StructureBrain.TYPES.NONE && !DataManager.Instance.UnlockedStructures.Contains(unlockNotification.StructureType))
        DataManager.Instance.UnlockedStructures.Add(unlockNotification.StructureType);
    }
  }

  public static UnlockManager.UnlockNotificationData[] GetUnlockNotifications(
    UnlockManager.UnlockType type)
  {
    switch (type)
    {
      case UnlockManager.UnlockType.Starting:
        return new UnlockManager.UnlockNotificationData[1]
        {
          new UnlockManager.UnlockNotificationData()
          {
            SermonRitualType = SermonsAndRituals.SermonRitualType.SERMON_OF_ENLIGHTENMENT
          }
        };
      case UnlockManager.UnlockType.Illness:
        return new UnlockManager.UnlockNotificationData[1]
        {
          new UnlockManager.UnlockNotificationData()
          {
            SermonRitualType = SermonsAndRituals.SermonRitualType.SERMON_PURGE_SICKNESS
          }
        };
      case UnlockManager.UnlockType.Dissenters:
        UnlockManager.UnlockNotificationData[] unlockNotifications = new UnlockManager.UnlockNotificationData[2];
        UnlockManager.UnlockNotificationData notificationData = new UnlockManager.UnlockNotificationData();
        notificationData.SermonRitualType = SermonsAndRituals.SermonRitualType.RITUAL_SACRIFICE_FOLLOWER;
        unlockNotifications[0] = notificationData;
        notificationData = new UnlockManager.UnlockNotificationData();
        notificationData.SermonRitualType = SermonsAndRituals.SermonRitualType.SERMON_THREAT_DISSENTERS;
        unlockNotifications[1] = notificationData;
        return unlockNotifications;
      case UnlockManager.UnlockType.Hunger:
        return new UnlockManager.UnlockNotificationData[1]
        {
          new UnlockManager.UnlockNotificationData()
          {
            SermonRitualType = SermonsAndRituals.SermonRitualType.SERMON_RENOUNCE_FOOD
          }
        };
      case UnlockManager.UnlockType.FollowerDeath:
        return new UnlockManager.UnlockNotificationData[1]
        {
          new UnlockManager.UnlockNotificationData()
          {
            SermonRitualType = SermonsAndRituals.SermonRitualType.RITUAL_REBIRTH
          }
        };
      default:
        return new UnlockManager.UnlockNotificationData[0];
    }
  }

  public static string GetUnlockAnimationName(UnlockManager.UnlockType type)
  {
    switch (type)
    {
      case UnlockManager.UnlockType.Starting:
        return "Avatars/avatar-happy";
      case UnlockManager.UnlockType.Illness:
        return "Avatars/avatar-sick";
      case UnlockManager.UnlockType.Dissenters:
        return "Avatars/avatar-dissenter1";
      case UnlockManager.UnlockType.Hunger:
        return "Avatars/avatar-unhappy";
      case UnlockManager.UnlockType.FollowerDeath:
        return "Avatars/avatar-dead";
      default:
        return "";
    }
  }

  public static string GetUnlockTitle(UnlockManager.UnlockType type)
  {
    switch (type)
    {
      case UnlockManager.UnlockType.Starting:
        return "A Gift...";
      case UnlockManager.UnlockType.Illness:
        return "Sickness";
      case UnlockManager.UnlockType.Dissenters:
        return "Dissenters";
      case UnlockManager.UnlockType.Hunger:
        return "Hunger";
      case UnlockManager.UnlockType.FollowerDeath:
        return "Death";
      default:
        return "";
    }
  }

  public static string GetUnlockSubtitle(UnlockManager.UnlockType type)
  {
    switch (type)
    {
      case UnlockManager.UnlockType.Starting:
        return "To help keep your Followers happy";
      case UnlockManager.UnlockType.Illness:
        return "A Follower has become ill";
      case UnlockManager.UnlockType.Dissenters:
        return "A Follower is spreading dissent";
      case UnlockManager.UnlockType.Hunger:
        return "A Follower is starving";
      case UnlockManager.UnlockType.FollowerDeath:
        return "A Follower has died";
      default:
        return "";
    }
  }

  public enum UnlockType
  {
    Starting,
    Illness,
    Dissenters,
    Hunger,
    FollowerDeath,
  }

  public struct UnlockNotificationData
  {
    public SermonsAndRituals.SermonRitualType SermonRitualType;
    public StructureBrain.TYPES StructureType;
  }
}
