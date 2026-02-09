// Decompiled with JetBrains decompiler
// Type: SermonsAndRituals
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

#nullable disable
public class SermonsAndRituals
{
  public static SermonsAndRituals.SermonRitualType CurrentSelectedType;
  public static FollowerInfo CurrentSelectedFollowerInfo;

  public static List<SermonsAndRituals.SermonRitualType> GetVisibleRituals()
  {
    List<SermonsAndRituals.SermonRitualType> visibleRituals = new List<SermonsAndRituals.SermonRitualType>()
    {
      SermonsAndRituals.SermonRitualType.RITUAL_SACRIFICE_FOLLOWER,
      SermonsAndRituals.SermonRitualType.RITUAL_REBIRTH
    };
    for (int index = 0; index < visibleRituals.Count; ++index)
    {
      if (!SermonsAndRituals.CheckUnlocked(visibleRituals[index]))
        visibleRituals.RemoveAt(index--);
    }
    return visibleRituals;
  }

  public static List<SermonsAndRituals.SermonRitualType> GetAvailableRituals()
  {
    List<SermonsAndRituals.SermonRitualType> availableRituals = new List<SermonsAndRituals.SermonRitualType>()
    {
      SermonsAndRituals.SermonRitualType.RITUAL_SACRIFICE_FOLLOWER,
      SermonsAndRituals.SermonRitualType.RITUAL_REBIRTH
    };
    for (int index = 0; index < availableRituals.Count; ++index)
    {
      if (!SermonsAndRituals.CheckAvailabilityRequirementsMet(availableRituals[index]))
        availableRituals.RemoveAt(index--);
    }
    return availableRituals;
  }

  public static List<SermonsAndRituals.SermonRitualType> GetVisibleSermons()
  {
    List<SermonsAndRituals.SermonRitualType> visibleSermons = new List<SermonsAndRituals.SermonRitualType>();
    for (int index = 0; index < visibleSermons.Count; ++index)
    {
      if (!SermonsAndRituals.CheckUnlocked(visibleSermons[index]))
        visibleSermons.RemoveAt(index--);
    }
    return visibleSermons;
  }

  public static List<SermonsAndRituals.SermonRitualType> GetAvailableSermons()
  {
    List<SermonsAndRituals.SermonRitualType> availableSermons = new List<SermonsAndRituals.SermonRitualType>();
    for (int index = 0; index < availableSermons.Count; ++index)
    {
      if (!SermonsAndRituals.CheckAvailabilityRequirementsMet(availableSermons[index]))
        availableSermons.RemoveAt(index--);
    }
    return availableSermons;
  }

  public static UnityEngine.Sprite Sprite(SermonsAndRituals.SermonRitualType type)
  {
    SpriteAtlas spriteAtlas = Resources.Load<SpriteAtlas>("Atlases/Ritual&SermonIcons");
    string name = "";
    switch (type)
    {
      case SermonsAndRituals.SermonRitualType.RITUAL_REBIRTH:
        name = "Ritual_Rebirth";
        break;
      case SermonsAndRituals.SermonRitualType.RITUAL_CULT_ACCESSION_I:
        name = "Ritual_Cult Accension";
        break;
      case SermonsAndRituals.SermonRitualType.RITUAL_CULT_ACCESSION_II:
        name = "Ritual_Cult Accension";
        break;
      case SermonsAndRituals.SermonRitualType.SERMON_PURGE_SICKNESS:
        name = "Sermon_PurgeSickness";
        break;
      case SermonsAndRituals.SermonRitualType.SERMON_THREAT_DISSENTERS:
        name = "Sermon_ThreatDissenters";
        break;
      case SermonsAndRituals.SermonRitualType.SERMON_DENOUNCE_NONBELEIVERS:
        name = "Sermon_NonBeleivers";
        break;
      case SermonsAndRituals.SermonRitualType.SERMON_LOVE_THY_NEIGHBOUR:
        name = "Sermon_LoveThyNeighbour";
        break;
      case SermonsAndRituals.SermonRitualType.SERMON_RENOUNCE_FOOD:
        name = "Sermon_RenounceFood";
        break;
      case SermonsAndRituals.SermonRitualType.SERMON_OF_ENLIGHTENMENT:
        name = "Sermon_Enlightenment";
        break;
      case SermonsAndRituals.SermonRitualType.RITUAL_SACRIFICE_FOLLOWER:
        name = "Ritual_Sacrifice";
        break;
    }
    return spriteAtlas.GetSprite(name);
  }

  public static string LocalisedName(SermonsAndRituals.SermonRitualType type)
  {
    return LocalizationManager.GetTranslation($"SermonsAndRituals/{type}/Name");
  }

  public static string LocalisedLore(SermonsAndRituals.SermonRitualType type)
  {
    return LocalizationManager.GetTranslation($"SermonsAndRituals/{type}/Lore");
  }

  public static string LocalisedDescription(SermonsAndRituals.SermonRitualType type)
  {
    return LocalizationManager.GetTranslation($"SermonsAndRituals/{type}/Description");
  }

  public static string LocalisedEffects(SermonsAndRituals.SermonRitualType type)
  {
    return LocalizationManager.GetTranslation($"SermonsAndRituals/{type}/Effects");
  }

  public static string LocalisedPros(SermonsAndRituals.SermonRitualType type)
  {
    return LocalizationManager.GetTranslation($"SermonsAndRituals/{type}/Pros");
  }

  public static string LocalisedCons(SermonsAndRituals.SermonRitualType type)
  {
    return LocalizationManager.GetTranslation($"SermonsAndRituals/{type}/Cons");
  }

  public static int CooldownDays(SermonsAndRituals.SermonRitualType type)
  {
    switch (type)
    {
      case SermonsAndRituals.SermonRitualType.SERMON_PURGE_SICKNESS:
        return 0;
      case SermonsAndRituals.SermonRitualType.SERMON_THREAT_DISSENTERS:
        return 7;
      case SermonsAndRituals.SermonRitualType.SERMON_DENOUNCE_NONBELEIVERS:
        return 7;
      case SermonsAndRituals.SermonRitualType.SERMON_LOVE_THY_NEIGHBOUR:
        return 7;
      case SermonsAndRituals.SermonRitualType.SERMON_RENOUNCE_FOOD:
        return 5;
      case SermonsAndRituals.SermonRitualType.SERMON_OF_ENLIGHTENMENT:
        return 2;
      default:
        return 0;
    }
  }

  public static int CostDevotion(SermonsAndRituals.SermonRitualType type)
  {
    switch (type)
    {
      case SermonsAndRituals.SermonRitualType.RITUAL_REBIRTH:
        return 25;
      case SermonsAndRituals.SermonRitualType.SERMON_PURGE_SICKNESS:
        return 10;
      case SermonsAndRituals.SermonRitualType.SERMON_THREAT_DISSENTERS:
        return 7;
      case SermonsAndRituals.SermonRitualType.SERMON_DENOUNCE_NONBELEIVERS:
        return 7;
      case SermonsAndRituals.SermonRitualType.SERMON_LOVE_THY_NEIGHBOUR:
        return 7;
      case SermonsAndRituals.SermonRitualType.SERMON_RENOUNCE_FOOD:
        return 7;
      case SermonsAndRituals.SermonRitualType.SERMON_OF_ENLIGHTENMENT:
        return 0;
      default:
        return 0;
    }
  }

  public static string SkinAnimationIndex(SermonsAndRituals.SermonRitualType type)
  {
    switch (type)
    {
      case SermonsAndRituals.SermonRitualType.RITUAL_REBIRTH:
        return "2";
      case SermonsAndRituals.SermonRitualType.RITUAL_CULT_ACCESSION_I:
        return "1";
      case SermonsAndRituals.SermonRitualType.RITUAL_CULT_ACCESSION_II:
        return "1";
      case SermonsAndRituals.SermonRitualType.SERMON_PURGE_SICKNESS:
        return "4";
      case SermonsAndRituals.SermonRitualType.SERMON_THREAT_DISSENTERS:
        return "6";
      case SermonsAndRituals.SermonRitualType.SERMON_DENOUNCE_NONBELEIVERS:
        return "7";
      case SermonsAndRituals.SermonRitualType.SERMON_LOVE_THY_NEIGHBOUR:
        return "3";
      case SermonsAndRituals.SermonRitualType.SERMON_RENOUNCE_FOOD:
        return "5";
      case SermonsAndRituals.SermonRitualType.SERMON_OF_ENLIGHTENMENT:
        return "2";
      case SermonsAndRituals.SermonRitualType.RITUAL_SACRIFICE_FOLLOWER:
        return "1";
      default:
        return "";
    }
  }

  public static List<Follower> GetAffectedFollowers(SermonsAndRituals.SermonRitualType type)
  {
    return new List<Follower>();
  }

  public static List<string> GetReactions(SermonsAndRituals.SermonRitualType type)
  {
    switch (type)
    {
      case SermonsAndRituals.SermonRitualType.SERMON_PURGE_SICKNESS:
        return new List<string>() { "Sermons/sermon-heal" };
      case SermonsAndRituals.SermonRitualType.SERMON_THREAT_DISSENTERS:
        return new List<string>()
        {
          "Reactions/react-worried1",
          "Reactions/react-worried2"
        };
      case SermonsAndRituals.SermonRitualType.SERMON_DENOUNCE_NONBELEIVERS:
        return new List<string>()
        {
          "Reactions/react-determined1",
          "Reactions/react-determined2"
        };
      case SermonsAndRituals.SermonRitualType.SERMON_LOVE_THY_NEIGHBOUR:
        return new List<string>()
        {
          "Reactions/react-love1",
          "Reactions/react-love2"
        };
      case SermonsAndRituals.SermonRitualType.SERMON_RENOUNCE_FOOD:
        return new List<string>()
        {
          "Reactions/react-fasting1",
          "Reactions/react-fasting2"
        };
      case SermonsAndRituals.SermonRitualType.SERMON_OF_ENLIGHTENMENT:
        return new List<string>()
        {
          "Reactions/react-enlightened1",
          "Reactions/react-enlightened2"
        };
      default:
        return new List<string>();
    }
  }

  public static Action<Follower> PerFollowerCallbacks(SermonsAndRituals.SermonRitualType type)
  {
    switch (type)
    {
      case SermonsAndRituals.SermonRitualType.SERMON_PURGE_SICKNESS:
        return (Action<Follower>) (f => f.Brain.Stats.Illness = 0.0f);
      case SermonsAndRituals.SermonRitualType.SERMON_THREAT_DISSENTERS:
        return (Action<Follower>) (f => f.Brain.Stats.Reeducation = 0.0f);
      case SermonsAndRituals.SermonRitualType.SERMON_DENOUNCE_NONBELEIVERS:
        return (Action<Follower>) (f => f.Brain.Stats.IncreasedDevotionOutputUntil = DataManager.Instance.CurrentDayIndex + 2);
      case SermonsAndRituals.SermonRitualType.SERMON_LOVE_THY_NEIGHBOUR:
        return (Action<Follower>) (f => f.Brain.Stats.GuaranteedGoodInteractionsUntil = DataManager.Instance.CurrentDayIndex + 2);
      case SermonsAndRituals.SermonRitualType.SERMON_RENOUNCE_FOOD:
        return (Action<Follower>) (f =>
        {
          f.Brain.Stats.Starvation = 0.0f;
          f.Brain.Stats.Satiation = 100f;
        });
      default:
        return (Action<Follower>) null;
    }
  }

  public static System.Action EffectCallbacks(SermonsAndRituals.SermonRitualType type)
  {
    return type == SermonsAndRituals.SermonRitualType.RITUAL_REBIRTH ? new System.Action(SermonsAndRituals.Rebirth) : (System.Action) null;
  }

  public static void ApplySermonRitualBonus()
  {
    foreach (Follower follower in FollowerManager.FollowersAtLocation(FollowerLocation.Church))
      ;
  }

  public static System.Action FinishCallbacks(SermonsAndRituals.SermonRitualType type)
  {
    if (type == SermonsAndRituals.SermonRitualType.RITUAL_CULT_ACCESSION_I)
      return new System.Action(SermonsAndRituals.Ascension1);
    return type == SermonsAndRituals.SermonRitualType.RITUAL_CULT_ACCESSION_II ? new System.Action(SermonsAndRituals.Ascension2) : (System.Action) null;
  }

  public static void Ascension1()
  {
    DataManager.Instance.CurrentCultLevel = DataManager.CultLevel.Two;
    UIAbilityUnlock.Play(UIAbilityUnlock.Ability.CultLevel1);
  }

  public static void Ascension2()
  {
    DataManager.Instance.CurrentCultLevel = DataManager.CultLevel.Three;
    UIAbilityUnlock.Play(UIAbilityUnlock.Ability.CultLevel2);
  }

  public static void Rebirth()
  {
    FollowerManager.ReviveFollower(SermonsAndRituals.CurrentSelectedFollowerInfo.ID, FollowerLocation.Church, ChurchFollowerManager.Instance.RitualCenterPosition.position);
    SermonsAndRituals.CurrentSelectedFollowerInfo = (FollowerInfo) null;
    foreach (StructureBrain structureBrain in StructureManager.StructuresAtLocation(FollowerLocation.Base))
    {
      if ((structureBrain.Data.Type == StructureBrain.TYPES.GRAVE || structureBrain.Data.Type == StructureBrain.TYPES.BODY_PIT || structureBrain.Data.Type == StructureBrain.TYPES.GRAVE2) && structureBrain.Data.FollowerID == SermonsAndRituals.CurrentSelectedFollowerInfo.ID)
        structureBrain.Data.FollowerID = -1;
    }
  }

  public static bool CheckAvailabilityRequirementsMet(SermonsAndRituals.SermonRitualType type)
  {
    bool flag = TimeManager.GetSermonRitualCooldownRemaining(type) <= 0 && Inventory.Souls >= SermonsAndRituals.CostDevotion(type);
    switch (type)
    {
      case SermonsAndRituals.SermonRitualType.RITUAL_REBIRTH:
        flag &= DataManager.Instance.Followers_Dead.Count >= 1;
        break;
      case SermonsAndRituals.SermonRitualType.RITUAL_CULT_ACCESSION_I:
        flag = ((flag ? 1 : 0) & (DataManager.Instance.CurrentCultLevel != DataManager.CultLevel.One || DataManager.Instance.Souls <= 1 ? 0 : (DataManager.Instance.Followers.Count >= 1 ? 1 : 0))) != 0;
        break;
      case SermonsAndRituals.SermonRitualType.RITUAL_CULT_ACCESSION_II:
        flag &= DataManager.Instance.CurrentCultLevel == DataManager.CultLevel.Two;
        break;
      case SermonsAndRituals.SermonRitualType.RITUAL_SACRIFICE_FOLLOWER:
        flag &= DataManager.Instance.Followers.Count >= 1;
        break;
    }
    return flag;
  }

  public static bool CheckUnlocked(SermonsAndRituals.SermonRitualType type)
  {
    return DataManager.Instance.UnlockedSermonsAndRituals.Contains(type);
  }

  public enum SermonRitualType
  {
    NONE,
    RITUAL_REBIRTH,
    RITUAL_CULT_ACCESSION_I,
    RITUAL_CULT_ACCESSION_II,
    SERMON_PURGE_SICKNESS,
    SERMON_THREAT_DISSENTERS,
    SERMON_DENOUNCE_NONBELEIVERS,
    SERMON_LOVE_THY_NEIGHBOUR,
    SERMON_RENOUNCE_FOOD,
    SERMON_OF_ENLIGHTENMENT,
    SERMON_UTOPIANISTS,
    SERMON_FUNDAMENTALISTS,
    SERMON_MISFITS,
    SERMON_DENOUNCE_GOAT,
    SERMON_DENOUNCE_OWL,
    SERMON_DENOUNCE_SNAKE,
    SERMON_DENOUNCE_FOLLOWER,
    SERMON_DILLIGENCE,
    RITUAL_PROMOTE_FOLLOWER,
    RITUAL_SACRIFICE_FOLLOWER,
    RITUAL_WEDDING,
    RITUAL_HEAL_SICK,
    RITUAL_NATURES_BOUNTY,
    Count,
  }
}
