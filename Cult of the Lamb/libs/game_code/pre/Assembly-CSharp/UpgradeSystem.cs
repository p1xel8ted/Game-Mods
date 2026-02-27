// Decompiled with JetBrains decompiler
// Type: UpgradeSystem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

#nullable disable
public class UpgradeSystem : BaseMonoBehaviour
{
  public static UpgradeSystem.UpgradeTypes UpgradeType = UpgradeSystem.UpgradeTypes.Both;
  public static System.Action OnAbilityPointDelta;
  public static System.Action OnBuildingUnlocked;
  public static System.Action OnDisciplePointDelta;
  public static Action<UpgradeSystem.Type> OnAbilityUnlocked;
  public static Action<UpgradeSystem.Type> OnAbilityLocked;
  private static List<UpgradeSystem.Type> UnlocksToReveal = new List<UpgradeSystem.Type>();
  public static System.Action OnCoolDownAdded;
  public static readonly UpgradeSystem.Type PrimaryRitual1 = UpgradeSystem.Type.Ritual_HeartsOfTheFaithful1;
  public static readonly UpgradeSystem.Type[] SecondaryRituals = new UpgradeSystem.Type[3]
  {
    UpgradeSystem.Type.Ritual_FirePit,
    UpgradeSystem.Type.Ritual_Sacrifice,
    UpgradeSystem.Type.Ritual_Brainwashing
  };
  public static readonly UpgradeSystem.Type[] SecondaryRitualPairs = new UpgradeSystem.Type[16 /*0x10*/]
  {
    UpgradeSystem.Type.Ritual_WorkThroughNight,
    UpgradeSystem.Type.Ritual_Holiday,
    UpgradeSystem.Type.Ritual_FasterBuilding,
    UpgradeSystem.Type.Ritual_Enlightenment,
    UpgradeSystem.Type.Ritual_AlmsToPoor,
    UpgradeSystem.Type.Ritual_DonationRitual,
    UpgradeSystem.Type.Ritual_Fast,
    UpgradeSystem.Type.Ritual_Feast,
    UpgradeSystem.Type.Ritual_HarvestRitual,
    UpgradeSystem.Type.Ritual_FishingRitual,
    UpgradeSystem.Type.Ritual_Funeral,
    UpgradeSystem.Type.Ritual_Ressurect,
    UpgradeSystem.Type.Ritual_AssignFaithEnforcer,
    UpgradeSystem.Type.Ritual_AssignTaxCollector,
    UpgradeSystem.Type.Ritual_Fightpit,
    UpgradeSystem.Type.Ritual_Wedding
  };
  public static readonly UpgradeSystem.Type[] SingleRituals = new UpgradeSystem.Type[1]
  {
    UpgradeSystem.Type.Ritual_Ascend
  };
  public static readonly UpgradeSystem.Type[] SpecialRituals = new UpgradeSystem.Type[1]
  {
    UpgradeSystem.Type.Ritual_Halloween
  };

  public static List<UpgradeSystem.Type> UnlockedUpgrades
  {
    get => DataManager.Instance.UnlockedUpgrades;
    set => DataManager.Instance.UnlockedUpgrades = value;
  }

  public static int AbilityPoints
  {
    get => DataManager.Instance.AbilityPoints;
    set
    {
      if (value > DataManager.Instance.AbilityPoints)
        NotificationCentreScreen.Play(NotificationCentre.NotificationType.NewUpgradePoint);
      DataManager.Instance.AbilityPoints = value;
      System.Action abilityPointDelta = UpgradeSystem.OnAbilityPointDelta;
      if (abilityPointDelta == null)
        return;
      abilityPointDelta();
    }
  }

  public static int DisciplePoints
  {
    get => DataManager.Instance.DiscipleAbilityPoints;
    set
    {
      DataManager.Instance.DiscipleAbilityPoints = value;
      System.Action disciplePointDelta = UpgradeSystem.OnDisciplePointDelta;
      if (disciplePointDelta == null)
        return;
      disciplePointDelta();
    }
  }

  public static event UpgradeSystem.UnlockEvent OnUpgradeUnlocked;

  public static float Foraging
  {
    get
    {
      List<Structures_ForagingShrine> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_ForagingShrine>(FollowerLocation.Base);
      if (structuresOfType.Count > 0)
      {
        foreach (StructureBrain structureBrain in structuresOfType)
        {
          if (structureBrain.Data.FullyFueled)
            return 0.75f;
        }
      }
      return 0.0f;
    }
  }

  public static float Chopping
  {
    get
    {
      List<Structures_ChoppingShrine> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_ChoppingShrine>(FollowerLocation.Base);
      if (structuresOfType.Count > 0)
      {
        foreach (StructureBrain structureBrain in structuresOfType)
        {
          if (structureBrain.Data.FullyFueled)
            return 1.5f;
        }
      }
      return 0.0f;
    }
  }

  public static float Mining
  {
    get
    {
      List<Structures_MiningShrine> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_MiningShrine>(FollowerLocation.Base);
      if (structuresOfType.Count > 0)
      {
        foreach (StructureBrain structureBrain in structuresOfType)
        {
          if (structureBrain.Data.FullyFueled)
            return 7.5f;
        }
      }
      return 0.0f;
    }
  }

  public static int GetForageIncreaseModifier
  {
    get => !UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Economy_Foraging2) ? 0 : 1;
  }

  public static float GetPriceModifier => 0.8f;

  public static string GetLocalizedName(UpgradeSystem.Type Type)
  {
    if (Type == UpgradeSystem.Type.Ritual_FirePit)
      return ScriptLocalization.UpgradeSystem_Building_DancingFirepit.Name;
    return Type == UpgradeSystem.Type.Ritual_HeartsOfTheFaithful1 ? ScriptLocalization.DoctrineUpgradeSystem.DeclareDoctrine : LocalizationManager.GetTranslation($"UpgradeSystem/{Type}/Name");
  }

  public static string GetUnlockLocalizedName(UpgradeSystem.Type Type)
  {
    return $"{ScriptLocalization.UpgradeSystem.Unlock} <color=yellow>{StructuresData.GetLocalizedNameStatic(UpgradeSystem.GetStructureTypeFromUpgrade(Type))}</color>";
  }

  public static string GetLocalizedDescription(UpgradeSystem.Type Type)
  {
    switch (Type)
    {
      case UpgradeSystem.Type.Building_FollowerFarming:
        return $"{$"<color=#FFD201>{StructuresData.GetLocalizedNameStatic(StructureBrain.TYPES.FARM_STATION)}: </color>{StructuresData.LocalizedDescription(StructureBrain.TYPES.FARM_STATION)}" + "<br><br>"}<color=#FFD201>{StructuresData.GetLocalizedNameStatic(StructureBrain.TYPES.SILO_SEED)}: </color>{StructuresData.LocalizedDescription(StructureBrain.TYPES.SILO_SEED)}";
      case UpgradeSystem.Type.Shrine_Flame:
        return $"{$"<color=#FFD201>{LocalizationManager.GetTranslation("UpgradeSystem/Shrine_Flame_Building/Name")}: </color>{LocalizationManager.GetTranslation("UpgradeSystem/Shrine_Flame_Building/Description")}" + "<br><br>"}<color=#FFD201>{LocalizationManager.GetTranslation("UpgradeSystem/Shrine_PassiveShrinesFlames/Name")}: </color>{LocalizationManager.GetTranslation("UpgradeSystem/Shrine_PassiveShrinesFlames/Description")}";
      case UpgradeSystem.Type.PUpgrade_CursePack1:
      case UpgradeSystem.Type.PUpgrade_CursePack2:
      case UpgradeSystem.Type.PUpgrade_CursePack3:
      case UpgradeSystem.Type.PUpgrade_CursePack4:
      case UpgradeSystem.Type.PUpgrade_CursePack5:
        return string.Format(ScriptLocalization.UpgradeSystem_PUpgrade_CursePacks.Description, (object) 3);
      case UpgradeSystem.Type.Ritual_FirePit:
        return ScriptLocalization.FollowerInteractions_GiveQuest.UseFirePit;
      default:
        if (Type.ToString().Contains("PUpgrade_"))
          return LocalizationManager.GetTranslation($"UpgradeSystem/{Type}/Description");
        if (Type.ToString().Contains("Ritual_"))
          return LocalizationManager.GetTranslation($"UpgradeSystem/{Type}/Description");
        if (Type == UpgradeSystem.Type.Temple_CheaperRituals)
          Type = UpgradeSystem.Type.Temple_CheaperRitualsII;
        if (Type == UpgradeSystem.Type.Temple_FasterCoolDowns)
          Type = UpgradeSystem.Type.Temple_FasterCoolDownsII;
        if (Type <= UpgradeSystem.Type.Ability_Resurrection)
        {
          if (Type != UpgradeSystem.Type.Ability_Eat && Type != UpgradeSystem.Type.Ability_Resurrection)
            goto label_18;
        }
        else
        {
          switch (Type - 73)
          {
            case UpgradeSystem.Type.Combat_ExtraHeart1:
            case UpgradeSystem.Type.Combat_Shrine:
            case UpgradeSystem.Type.Combat_EnemiesDropXP:
            case UpgradeSystem.Type.Followers_Shelter:
            case UpgradeSystem.Type.Followers_FoodBasics:
            case UpgradeSystem.Type.Followers_FoodAutomation:
            case UpgradeSystem.Type.Followers_Compost:
            case UpgradeSystem.Type.Followers_Chef:
            case UpgradeSystem.Type.Followers_AdvancedFoods:
            case UpgradeSystem.Type.Followers_SeedSilo:
            case UpgradeSystem.Type.Cult_RitualCircle:
            case UpgradeSystem.Type.Cult_ConfessionBooth:
              break;
            case UpgradeSystem.Type.Combat_ExtraHeart2:
            case UpgradeSystem.Type.Combat_Swords2:
            case UpgradeSystem.Type.Combat_Arrows:
            case UpgradeSystem.Type.Combat_Arrows2:
            case UpgradeSystem.Type.Combat_Dash2:
            case UpgradeSystem.Type.Combat_MoreBlackSouls:
            case UpgradeSystem.Type.Followers_Bathrooms:
            case UpgradeSystem.Type.Followers_Clothing:
            case UpgradeSystem.Type.Followers_Reproduction:
              goto label_18;
            default:
              if ((uint) (Type - 128 /*0x80*/) <= 1U || (uint) (Type - 139) <= 3U)
                break;
              goto label_18;
          }
        }
        return LocalizationManager.GetTranslation($"UpgradeSystem/{Type}/Description");
label_18:
        Debug.Log((object) ("D " + (object) Type));
        return StructuresData.LocalizedDescription(UpgradeSystem.GetStructureTypeFromUpgrade(Type));
    }
  }

  public static StructureBrain.TYPES GetStructureTypeFromUpgrade(UpgradeSystem.Type Type)
  {
    switch (Type)
    {
      case UpgradeSystem.Type.Followers_Compost:
        return StructureBrain.TYPES.COMPOST_BIN;
      case UpgradeSystem.Type.Economy_MineII:
        return StructureBrain.TYPES.BLOODSTONE_MINE_2;
      case UpgradeSystem.Type.Economy_Mine:
        return StructureBrain.TYPES.BLOODSTONE_MINE;
      case UpgradeSystem.Type.Economy_Lumberyard:
        return StructureBrain.TYPES.LUMBERJACK_STATION;
      case UpgradeSystem.Type.Economy_Refinery:
        return StructureBrain.TYPES.REFINERY;
      case UpgradeSystem.Type.Economy_Refinery_2:
        return StructureBrain.TYPES.REFINERY_2;
      case UpgradeSystem.Type.Economy_LumberyardII:
        return StructureBrain.TYPES.LUMBERJACK_STATION_2;
      case UpgradeSystem.Type.Building_Temple:
        return StructureBrain.TYPES.TEMPLE;
      case UpgradeSystem.Type.Building_Farms:
        return StructureBrain.TYPES.FARM_PLOT;
      case UpgradeSystem.Type.Building_AdvancedFarming:
        return StructureBrain.TYPES.SCARECROW;
      case UpgradeSystem.Type.Building_Prison:
        return StructureBrain.TYPES.PRISON;
      case UpgradeSystem.Type.Building_Outhouse:
        return StructureBrain.TYPES.OUTHOUSE;
      case UpgradeSystem.Type.Building_ConfessionBooth:
        return StructureBrain.TYPES.CONFESSION_BOOTH;
      case UpgradeSystem.Type.Building_Beds:
        return StructureBrain.TYPES.BED;
      case UpgradeSystem.Type.Building_FollowerFarming:
        return StructureBrain.TYPES.FARM_STATION;
      case UpgradeSystem.Type.Building_NaturalBurial:
        return StructureBrain.TYPES.COMPOST_BIN_DEAD_BODY;
      case UpgradeSystem.Type.Building_BetterBeds:
        return StructureBrain.TYPES.BED_2;
      case UpgradeSystem.Type.Building_Graves:
        return StructureBrain.TYPES.GRAVE;
      case UpgradeSystem.Type.Building_Temple2:
        return StructureBrain.TYPES.TEMPLE_II;
      case UpgradeSystem.Type.Building_Kitchen:
        return StructureBrain.TYPES.KITCHEN;
      case UpgradeSystem.Type.Shrine_II:
        return StructureBrain.TYPES.SHRINE_II;
      case UpgradeSystem.Type.Shrine_OfferingStatue:
        return StructureBrain.TYPES.OFFERING_STATUE;
      case UpgradeSystem.Type.Shrine_III:
        return StructureBrain.TYPES.SHRINE_III;
      case UpgradeSystem.Type.Shrine_PassiveShrines:
        return StructureBrain.TYPES.SHRINE_PASSIVE;
      case UpgradeSystem.Type.Shrine_PassiveShrinesII:
        return StructureBrain.TYPES.SHRINE_PASSIVE_II;
      case UpgradeSystem.Type.Shrine_IV:
        return StructureBrain.TYPES.SHRINE_IV;
      case UpgradeSystem.Type.Shrine_PassiveShrinesIII:
        return StructureBrain.TYPES.SHRINE_PASSIVE_III;
      case UpgradeSystem.Type.Temple_III:
        return StructureBrain.TYPES.TEMPLE_III;
      case UpgradeSystem.Type.Temple_IV:
        return StructureBrain.TYPES.TEMPLE_IV;
      case UpgradeSystem.Type.Building_PropagandaSpeakers:
        return StructureBrain.TYPES.PROPAGANDA_SPEAKER;
      case UpgradeSystem.Type.Building_Missionary:
        return StructureBrain.TYPES.MISSIONARY;
      case UpgradeSystem.Type.Building_Beds3:
        return StructureBrain.TYPES.BED_3;
      case UpgradeSystem.Type.Building_FeastTable:
        return StructureBrain.TYPES.FEAST_TABLE;
      case UpgradeSystem.Type.Building_SiloSeed:
        return StructureBrain.TYPES.SILO_SEED;
      case UpgradeSystem.Type.Building_SiloFertiliser:
        return StructureBrain.TYPES.SILO_FERTILISER;
      case UpgradeSystem.Type.Building_Surveillance:
        return StructureBrain.TYPES.SURVEILLANCE;
      case UpgradeSystem.Type.Building_FishingHut2:
        return StructureBrain.TYPES.FISHING_HUT_2;
      case UpgradeSystem.Type.Building_Outhouse2:
        return StructureBrain.TYPES.OUTHOUSE_2;
      case UpgradeSystem.Type.Building_Scarecrow2:
        return StructureBrain.TYPES.SCARECROW_2;
      case UpgradeSystem.Type.Building_HarvestTotem2:
        return StructureBrain.TYPES.HARVEST_TOTEM_2;
      case UpgradeSystem.Type.Building_DancingFirepit:
        return StructureBrain.TYPES.DANCING_FIREPIT;
      case UpgradeSystem.Type.Building_BodyPit:
        return StructureBrain.TYPES.BODY_PIT;
      case UpgradeSystem.Type.Building_HarvestTotem:
        return StructureBrain.TYPES.HARVEST_TOTEM;
      case UpgradeSystem.Type.Building_HealingBay:
        return StructureBrain.TYPES.HEALING_BAY;
      case UpgradeSystem.Type.Building_HealingBay2:
        return StructureBrain.TYPES.HEALING_BAY_2;
      case UpgradeSystem.Type.Building_FoodStorage:
        return StructureBrain.TYPES.FOOD_STORAGE;
      case UpgradeSystem.Type.Building_FoodStorage2:
        return StructureBrain.TYPES.FOOD_STORAGE_2;
      case UpgradeSystem.Type.Building_JanitorStation:
        return StructureBrain.TYPES.JANITOR_STATION;
      case UpgradeSystem.Type.Building_DemonSummoner:
        return StructureBrain.TYPES.DEMON_SUMMONER;
      case UpgradeSystem.Type.Building_DemonSummoner_2:
        return StructureBrain.TYPES.DEMON_SUMMONER_2;
      case UpgradeSystem.Type.Building_DemonSummoner_3:
        return StructureBrain.TYPES.DEMON_SUMMONER_3;
      case UpgradeSystem.Type.Building_MissionaryII:
        return StructureBrain.TYPES.MISSIONARY_II;
      case UpgradeSystem.Type.Building_MissionaryIII:
        return StructureBrain.TYPES.MISSIONARY_III;
      case UpgradeSystem.Type.Building_KitchenII:
        return StructureBrain.TYPES.KITCHEN_II;
      case UpgradeSystem.Type.Building_FarmStationII:
        return StructureBrain.TYPES.FARM_STATION_II;
      default:
        return StructureBrain.TYPES.SHRINE;
    }
  }

  public static string GetLocalizedActivated(UpgradeSystem.Type Type)
  {
    return LocalizationManager.GetTranslation($"UpgradeSystem/{Type}/Activated");
  }

  public static bool GetUnlocked(UpgradeSystem.Type Type)
  {
    return UpgradeSystem.UnlockedUpgrades.Contains(Type);
  }

  public static Sprite GetIcon(UpgradeSystem.Type Type)
  {
    return Resources.Load<SpriteAtlas>("Atlases/AbilityIcons").GetSprite(Type.ToString());
  }

  public static bool IsRitualActive(UpgradeSystem.Type type)
  {
    switch (type)
    {
      case UpgradeSystem.Type.Ritual_FasterBuilding:
        return FollowerBrainStats.IsConstruction;
      case UpgradeSystem.Type.Ritual_Enlightenment:
        return FollowerBrainStats.IsEnlightened;
      case UpgradeSystem.Type.Ritual_WorkThroughNight:
        return FollowerBrainStats.IsWorkThroughTheNight;
      case UpgradeSystem.Type.Ritual_Holiday:
        return FollowerBrainStats.IsHoliday;
      case UpgradeSystem.Type.Ritual_Fast:
        return FollowerBrainStats.Fasting;
      case UpgradeSystem.Type.Ritual_FishingRitual:
        return FollowerBrainStats.IsFishing;
      case UpgradeSystem.Type.Ritual_Brainwashing:
        return FollowerBrainStats.BrainWashed;
      default:
        return false;
    }
  }

  public static bool UnlockAbility(UpgradeSystem.Type Type)
  {
    Debug.Log((object) ("UnlockAbilit " + (object) Type));
    if (UpgradeSystem.UnlockedUpgrades.Contains(Type))
      return false;
    switch (Type)
    {
      case UpgradeSystem.Type.PUpgrade_WeaponGodly:
        AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("ALL_WEAPONS_UNLOCKED"));
        break;
      case UpgradeSystem.Type.PUpgrade_CursePack3:
        AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("ALL_CURSES_UNLOCKED"));
        break;
    }
    UpgradeSystem.UnlockedUpgrades.Add(Type);
    UpgradeSystem.UnlocksToReveal.Add(Type);
    Action<UpgradeSystem.Type> onAbilityUnlocked = UpgradeSystem.OnAbilityUnlocked;
    if (onAbilityUnlocked != null)
      onAbilityUnlocked(Type);
    return true;
  }

  public static void LockAbility(UpgradeSystem.Type type)
  {
    UpgradeSystem.UnlockedUpgrades.Remove(type);
    Action<UpgradeSystem.Type> onAbilityLocked = UpgradeSystem.OnAbilityLocked;
    if (onAbilityLocked != null)
      onAbilityLocked(type);
    ObjectiveManager.CheckObjectives(Objectives.TYPES.PERFORM_RITUAL);
  }

  public static IEnumerator ListOfUnlocksRoutine()
  {
    foreach (UpgradeSystem.Type Type in new List<UpgradeSystem.Type>((IEnumerable<UpgradeSystem.Type>) UpgradeSystem.UnlocksToReveal))
      yield return (object) GameManager.GetInstance().StartCoroutine((IEnumerator) UpgradeSystem.OnUnlockAbility(Type));
    UpgradeSystem.UnlocksToReveal.Clear();
  }

  public static IEnumerator OnUnlockAbility(UpgradeSystem.Type Type)
  {
    yield return (object) null;
    Debug.Log((object) ("Type unlocked: " + (object) Type));
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.CollectDivineInspiration);
    switch (Type)
    {
      case UpgradeSystem.Type.Combat_ExtraHeart1:
      case UpgradeSystem.Type.Combat_ExtraHeart2:
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful1:
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful2:
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful3:
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful4:
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful5:
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful6:
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful7:
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful8:
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful9:
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful10:
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful11:
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful12:
        Debug.Log((object) ("UNLOCK : " + (object) Type));
        break;
      case UpgradeSystem.Type.Economy_Refinery:
      case UpgradeSystem.Type.Building_Farms:
      case UpgradeSystem.Type.Building_AdvancedFarming:
      case UpgradeSystem.Type.Building_Prison:
      case UpgradeSystem.Type.Building_Outhouse:
      case UpgradeSystem.Type.Building_LumberMine:
      case UpgradeSystem.Type.Building_Burial:
      case UpgradeSystem.Type.Building_Apothecary:
      case UpgradeSystem.Type.Building_ConfessionBooth:
      case UpgradeSystem.Type.Building_FollowerFarming:
      case UpgradeSystem.Type.Building_NaturalBurial:
      case UpgradeSystem.Type.Building_BetterBeds:
      case UpgradeSystem.Type.Building_BetterShrine:
      case UpgradeSystem.Type.Building_Graves:
      case UpgradeSystem.Type.Shrine_OfferingStatue:
      case UpgradeSystem.Type.Shrine_PassiveShrines:
      case UpgradeSystem.Type.Shrine_PassiveShrinesII:
      case UpgradeSystem.Type.Shrine_PassiveShrinesIII:
      case UpgradeSystem.Type.Building_FeastTable:
      case UpgradeSystem.Type.Building_Scarecrow2:
        DataManager.Instance.NewBuildings = true;
        System.Action buildingUnlocked1 = UpgradeSystem.OnBuildingUnlocked;
        if (buildingUnlocked1 != null)
        {
          buildingUnlocked1();
          break;
        }
        break;
      case UpgradeSystem.Type.Building_Temple:
        DataManager.Instance.NewBuildings = true;
        System.Action buildingUnlocked2 = UpgradeSystem.OnBuildingUnlocked;
        if (buildingUnlocked2 != null)
          buildingUnlocked2();
        Debug.Log((object) "AA!");
        break;
      case UpgradeSystem.Type.Building_Beds:
        DataManager.Instance.OnboardedHomelessAtNight = true;
        DataManager.Instance.NewBuildings = true;
        System.Action buildingUnlocked3 = UpgradeSystem.OnBuildingUnlocked;
        if (buildingUnlocked3 != null)
        {
          buildingUnlocked3();
          break;
        }
        break;
      case UpgradeSystem.Type.Shrine_Flame:
        UpgradeSystem.UnlockedUpgrades.Add(UpgradeSystem.Type.Shrine_PassiveShrinesFlames);
        break;
      case UpgradeSystem.Type.Building_Decorations1:
        DataManager.Instance.UnlockedStructures.Add(StructureBrain.TYPES.DECORATION_WALL_TWIGS);
        DataManager.Instance.UnlockedStructures.Add(StructureBrain.TYPES.DECORATION_WALL_GRASS);
        DataManager.Instance.UnlockedStructures.Add(StructureBrain.TYPES.DECORATION_TREE);
        DataManager.Instance.UnlockedStructures.Add(StructureBrain.TYPES.DECORATION_SMALL_STONE_CANDLE);
        DataManager.Instance.UnlockedStructures.Add(StructureBrain.TYPES.DECORATION_CANDLE_BARREL);
        DataManager.Instance.UnlockedStructures.Add(StructureBrain.TYPES.DECORATION_STONE);
        DataManager.Instance.UnlockedStructures.Add(StructureBrain.TYPES.DECORATION_FLAG_CROWN);
        break;
      case UpgradeSystem.Type.Building_Decorations2:
        DataManager.Instance.UnlockedStructures.Add(StructureBrain.TYPES.DECORATION_BONE_ARCH);
        DataManager.Instance.UnlockedStructures.Add(StructureBrain.TYPES.DECORATION_BONE_BARREL);
        DataManager.Instance.UnlockedStructures.Add(StructureBrain.TYPES.DECORATION_BONE_CANDLE);
        DataManager.Instance.UnlockedStructures.Add(StructureBrain.TYPES.DECORATION_BONE_FLAG);
        DataManager.Instance.UnlockedStructures.Add(StructureBrain.TYPES.DECORATION_BONE_LANTERN);
        DataManager.Instance.UnlockedStructures.Add(StructureBrain.TYPES.DECORATION_BONE_PILLAR);
        DataManager.Instance.UnlockedStructures.Add(StructureBrain.TYPES.DECORATION_BONE_SCULPTURE);
        DataManager.Instance.UnlockedStructures.Add(StructureBrain.TYPES.DECORATION_WALL_BONE);
        break;
      case UpgradeSystem.Type.PUpgrade_Heart_1:
      case UpgradeSystem.Type.PUpgrade_Heart_2:
      case UpgradeSystem.Type.PUpgrade_Heart_3:
      case UpgradeSystem.Type.PUpgrade_Heart_4:
      case UpgradeSystem.Type.PUpgrade_Heart_5:
      case UpgradeSystem.Type.PUpgrade_Heart_6:
        HealthPlayer objectOfType = UnityEngine.Object.FindObjectOfType<HealthPlayer>();
        ++objectOfType.totalHP;
        objectOfType.HP = objectOfType.totalHP;
        ++DataManager.Instance.PLAYER_HEARTS_LEVEL;
        break;
      case UpgradeSystem.Type.PUpgrade_Gauntlets_0:
        DataManager.Instance.AddWeapon(EquipmentType.Gauntlet);
        DataManager.Instance.ForcedStartingWeapon = EquipmentType.Gauntlet;
        if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_WeaponFervor))
          DataManager.Instance.AddWeapon(EquipmentType.Gauntlet_Fervour);
        if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_WeaponNecromancy))
          DataManager.Instance.AddWeapon(EquipmentType.Gauntlet_Nercomancy);
        if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_WeaponHeal))
        {
          DataManager.Instance.AddWeapon(EquipmentType.Gauntlet_Healing);
          break;
        }
        break;
      case UpgradeSystem.Type.PUpgrade_Hammer_0:
        DataManager.Instance.AddWeapon(EquipmentType.Hammer);
        DataManager.Instance.ForcedStartingWeapon = EquipmentType.Hammer;
        if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_WeaponFervor))
          DataManager.Instance.AddWeapon(EquipmentType.Hammer_Fervour);
        if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_WeaponNecromancy))
          DataManager.Instance.AddWeapon(EquipmentType.Hammer_Nercomancy);
        if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_WeaponHeal))
        {
          DataManager.Instance.AddWeapon(EquipmentType.Hammer_Healing);
          break;
        }
        break;
      case UpgradeSystem.Type.PUpgrade_Tentacles_0:
        DataManager.Instance.AddCurse(EquipmentType.Tentacles);
        DataManager.Instance.ForcedStartingCurse = EquipmentType.Tentacles;
        if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_CursePack3))
        {
          DataManager.Instance.AddCurse(EquipmentType.Tentacles_Circular);
          break;
        }
        break;
      case UpgradeSystem.Type.PUpgrade_MegaSlash_0:
        DataManager.Instance.AddCurse(EquipmentType.MegaSlash);
        DataManager.Instance.ForcedStartingCurse = EquipmentType.MegaSlash;
        if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_CursePack2))
        {
          DataManager.Instance.AddCurse(EquipmentType.MegaSlash_Necromancy);
          break;
        }
        break;
      case UpgradeSystem.Type.PUpgrade_WeaponCritHit:
        DataManager.Instance.AddWeapon(EquipmentType.Axe_Critical);
        DataManager.Instance.AddWeapon(EquipmentType.Sword_Critical);
        DataManager.Instance.AddWeapon(EquipmentType.Dagger_Critical);
        DataManager.Instance.AddWeapon(EquipmentType.Gauntlet_Critical);
        DataManager.Instance.AddWeapon(EquipmentType.Hammer_Critical);
        DataManager.Instance.ForcedStartingWeapon = (double) UnityEngine.Random.value > 0.5 ? ((double) UnityEngine.Random.value > 0.5 ? EquipmentType.Sword_Critical : EquipmentType.Dagger_Critical) : EquipmentType.Axe_Critical;
        break;
      case UpgradeSystem.Type.PUpgrade_WeaponPoison:
        DataManager.Instance.AddWeapon(EquipmentType.Axe_Poison);
        DataManager.Instance.AddWeapon(EquipmentType.Sword_Poison);
        DataManager.Instance.AddWeapon(EquipmentType.Dagger_Poison);
        DataManager.Instance.AddWeapon(EquipmentType.Hammer_Poison);
        DataManager.Instance.AddWeapon(EquipmentType.Gauntlet_Poison);
        DataManager.Instance.ForcedStartingWeapon = (double) UnityEngine.Random.value > 0.5 ? ((double) UnityEngine.Random.value > 0.5 ? EquipmentType.Sword_Poison : EquipmentType.Dagger_Poison) : EquipmentType.Axe_Poison;
        break;
      case UpgradeSystem.Type.PUpgrade_WeaponFervor:
        DataManager.Instance.AddWeapon(EquipmentType.Axe_Fervour);
        DataManager.Instance.AddWeapon(EquipmentType.Sword_Fervour);
        DataManager.Instance.AddWeapon(EquipmentType.Dagger_Fervour);
        DataManager.Instance.AddWeapon(EquipmentType.Hammer_Fervour);
        DataManager.Instance.AddWeapon(EquipmentType.Gauntlet_Fervour);
        DataManager.Instance.ForcedStartingWeapon = (double) UnityEngine.Random.value > 0.5 ? ((double) UnityEngine.Random.value > 0.5 ? EquipmentType.Sword_Fervour : EquipmentType.Dagger_Fervour) : EquipmentType.Axe_Fervour;
        break;
      case UpgradeSystem.Type.PUpgrade_WeaponNecromancy:
        DataManager.Instance.AddWeapon(EquipmentType.Axe_Nercomancy);
        DataManager.Instance.AddWeapon(EquipmentType.Sword_Nercomancy);
        DataManager.Instance.AddWeapon(EquipmentType.Dagger_Nercomancy);
        DataManager.Instance.AddWeapon(EquipmentType.Hammer_Nercomancy);
        DataManager.Instance.AddWeapon(EquipmentType.Gauntlet_Nercomancy);
        DataManager.Instance.ForcedStartingWeapon = (double) UnityEngine.Random.value > 0.5 ? ((double) UnityEngine.Random.value > 0.5 ? EquipmentType.Sword_Nercomancy : EquipmentType.Dagger_Nercomancy) : EquipmentType.Axe_Nercomancy;
        break;
      case UpgradeSystem.Type.PUpgrade_WeaponHeal:
        DataManager.Instance.AddWeapon(EquipmentType.Axe_Healing);
        DataManager.Instance.AddWeapon(EquipmentType.Sword_Healing);
        DataManager.Instance.AddWeapon(EquipmentType.Dagger_Healing);
        DataManager.Instance.AddWeapon(EquipmentType.Hammer_Healing);
        DataManager.Instance.AddWeapon(EquipmentType.Gauntlet_Healing);
        DataManager.Instance.ForcedStartingWeapon = (double) UnityEngine.Random.value > 0.5 ? ((double) UnityEngine.Random.value > 0.5 ? EquipmentType.Sword_Healing : EquipmentType.Dagger_Healing) : EquipmentType.Axe_Healing;
        break;
      case UpgradeSystem.Type.PUpgrade_WeaponGodly:
        DataManager.Instance.AddWeapon(EquipmentType.Axe_Godly);
        DataManager.Instance.AddWeapon(EquipmentType.Sword_Godly);
        DataManager.Instance.AddWeapon(EquipmentType.Dagger_Godly);
        DataManager.Instance.AddWeapon(EquipmentType.Hammer_Godly);
        DataManager.Instance.AddWeapon(EquipmentType.Gauntlet_Godly);
        DataManager.Instance.ForcedStartingWeapon = (double) UnityEngine.Random.value > 0.5 ? ((double) UnityEngine.Random.value > 0.5 ? EquipmentType.Sword_Godly : EquipmentType.Dagger_Godly) : EquipmentType.Axe_Godly;
        break;
      case UpgradeSystem.Type.PUpgrade_CursePack1:
        DataManager.Instance.AddCurse(EquipmentType.EnemyBlast_Ice);
        DataManager.Instance.AddCurse(EquipmentType.MegaSlash_Ice);
        DataManager.Instance.AddCurse(EquipmentType.Tentacles_Ice);
        DataManager.Instance.ForcedStartingCurse = (double) UnityEngine.Random.value > 0.5 ? ((double) UnityEngine.Random.value > 0.5 ? EquipmentType.Tentacles_Ice : EquipmentType.MegaSlash_Ice) : EquipmentType.EnemyBlast_Ice;
        break;
      case UpgradeSystem.Type.PUpgrade_CursePack2:
        DataManager.Instance.AddCurse(EquipmentType.Tentacles_Circular);
        DataManager.Instance.AddCurse(EquipmentType.Fireball_Swarm);
        DataManager.Instance.AddCurse(EquipmentType.ProjectileAOE_ExplosiveImpact);
        DataManager.Instance.ForcedStartingCurse = (double) UnityEngine.Random.value > 0.5 ? ((double) UnityEngine.Random.value > 0.5 ? EquipmentType.Tentacles_Circular : EquipmentType.Fireball_Swarm) : EquipmentType.ProjectileAOE_ExplosiveImpact;
        break;
      case UpgradeSystem.Type.PUpgrade_CursePack3:
        DataManager.Instance.AddCurse(EquipmentType.Fireball_Charm);
        DataManager.Instance.AddCurse(EquipmentType.MegaSlash_Charm);
        DataManager.Instance.AddCurse(EquipmentType.ProjectileAOE_Charm);
        DataManager.Instance.ForcedStartingCurse = (double) UnityEngine.Random.value > 0.5 ? ((double) UnityEngine.Random.value > 0.5 ? EquipmentType.Fireball_Charm : EquipmentType.MegaSlash_Charm) : EquipmentType.ProjectileAOE_Charm;
        break;
      case UpgradeSystem.Type.PUpgrade_CursePack4:
        DataManager.Instance.AddCurse(EquipmentType.MegaSlash_Necromancy);
        DataManager.Instance.AddCurse(EquipmentType.Tentacles_Necromancy);
        DataManager.Instance.AddCurse(EquipmentType.EnemyBlast_Poison);
        DataManager.Instance.ForcedStartingCurse = (double) UnityEngine.Random.value > 0.5 ? ((double) UnityEngine.Random.value > 0.5 ? EquipmentType.MegaSlash_Necromancy : EquipmentType.Tentacles_Necromancy) : EquipmentType.EnemyBlast_Poison;
        break;
      case UpgradeSystem.Type.PUpgrade_CursePack5:
        DataManager.Instance.AddCurse(EquipmentType.ProjectileAOE_GoopTrail);
        DataManager.Instance.AddCurse(EquipmentType.EnemyBlast_DeflectsProjectiles);
        DataManager.Instance.AddCurse(EquipmentType.Fireball_Triple);
        DataManager.Instance.ForcedStartingCurse = (double) UnityEngine.Random.value > 0.5 ? ((double) UnityEngine.Random.value > 0.5 ? EquipmentType.ProjectileAOE_GoopTrail : EquipmentType.EnemyBlast_DeflectsProjectiles) : EquipmentType.Fireball_Triple;
        break;
    }
    UpgradeSystem.UnlockEvent onUpgradeUnlocked = UpgradeSystem.OnUpgradeUnlocked;
    if (onUpgradeUnlocked != null)
      onUpgradeUnlocked(Type);
  }

  private static void UnlockUpgradeWeapon(TarotCards.Card Weapon)
  {
    if (false)
      UpgradeSystem.UpgradeWeapon(Weapon);
    else
      UpgradeSystem.UnlockWeapon(Weapon);
  }

  private static void UpgradeWeapon(TarotCards.Card Weapon)
  {
  }

  private static void UnlockWeapon(TarotCards.Card Weapon)
  {
  }

  private static void UnlockUpgradeCurse(TarotCards.Card Curse)
  {
  }

  private static void UpgradeCurse(TarotCards.Card Curse)
  {
  }

  private static void UnlockCurse(TarotCards.Card Curse)
  {
  }

  public static float GetRitualFaithChange(UpgradeSystem.Type Type)
  {
    switch (Type)
    {
      case UpgradeSystem.Type.Ritual_Sacrifice:
        return DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.SacrificeEnthusiast) ? FollowerThoughts.GetData(Thought.Cult_Sacrifice_Trait).Modifier : FollowerThoughts.GetData(Thought.Cult_Sacrifice).Modifier;
      case UpgradeSystem.Type.Ritual_ConsumeFollower:
        return FollowerThoughts.GetData(Thought.Cult_ConsumeFollower).Modifier;
      case UpgradeSystem.Type.Ritual_FasterBuilding:
        return FollowerThoughts.GetData(Thought.Cult_FasterBuilding).Modifier;
      case UpgradeSystem.Type.Ritual_Enlightenment:
        return FollowerThoughts.GetData(Thought.Cult_Enlightenment).Modifier;
      case UpgradeSystem.Type.Ritual_WorkThroughNight:
        return FollowerThoughts.GetData(Thought.Cult_WorkThroughNight).Modifier;
      case UpgradeSystem.Type.Ritual_Holiday:
        return FollowerThoughts.GetData(Thought.Cult_Holiday).Modifier;
      case UpgradeSystem.Type.Ritual_AlmsToPoor:
        return FollowerThoughts.GetData(Thought.Cult_AlmsToPoor).Modifier;
      case UpgradeSystem.Type.Ritual_DonationRitual:
        return FollowerThoughts.GetData(Thought.Cult_DonationRitual).Modifier;
      case UpgradeSystem.Type.Ritual_Fast:
        return FollowerThoughts.GetData(Thought.Cult_Fast).Modifier;
      case UpgradeSystem.Type.Ritual_Feast:
        return FollowerThoughts.GetData(Thought.Cult_Feast).Modifier;
      case UpgradeSystem.Type.Ritual_HarvestRitual:
        return FollowerThoughts.GetData(Thought.Cult_HarvestRitual).Modifier;
      case UpgradeSystem.Type.Ritual_FishingRitual:
        return FollowerThoughts.GetData(Thought.Cult_FishingRitual).Modifier;
      case UpgradeSystem.Type.Ritual_Ressurect:
        return FollowerThoughts.GetData(Thought.Cult_Ressurection).Modifier;
      case UpgradeSystem.Type.Ritual_Funeral:
        return FollowerThoughts.GetData(Thought.Cult_Funeral).Modifier;
      case UpgradeSystem.Type.Ritual_Fightpit:
        return FollowerThoughts.GetData(Thought.Cult_FightPit).Modifier;
      case UpgradeSystem.Type.Ritual_Wedding:
        return FollowerThoughts.GetData(Thought.Cult_Wedding).Modifier;
      case UpgradeSystem.Type.Ritual_AssignFaithEnforcer:
        return FollowerThoughts.GetData(Thought.Cult_FaithEnforcer).Modifier;
      case UpgradeSystem.Type.Ritual_AssignTaxCollector:
        return FollowerThoughts.GetData(Thought.Cult_TaxEnforcer).Modifier;
      case UpgradeSystem.Type.Ritual_Brainwashing:
        return DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.MushroomEncouraged) ? FollowerThoughts.GetData(Thought.Cult_MushroomEncouraged_Trait).Modifier : FollowerThoughts.GetData(Thought.Brainwashed).Modifier;
      case UpgradeSystem.Type.Ritual_Ascend:
        return FollowerThoughts.GetData(Thought.Cult_Ascend).Modifier;
      case UpgradeSystem.Type.Ritual_FirePit:
        return FollowerThoughts.GetData(Thought.DancePit).Modifier;
      default:
        return 0.0f;
    }
  }

  public static FollowerTrait.TraitType GetRitualTrait(UpgradeSystem.Type Type)
  {
    if (Type == UpgradeSystem.Type.Ritual_Sacrifice)
      return FollowerTrait.TraitType.SacrificeEnthusiast;
    return Type == UpgradeSystem.Type.Ritual_Brainwashing ? FollowerTrait.TraitType.MushroomEncouraged : FollowerTrait.TraitType.None;
  }

  public static List<StructuresData.ItemCost> GetCost(UpgradeSystem.Type Type)
  {
    switch (Type)
    {
      case UpgradeSystem.Type.Economy_FishingRod:
        return new List<StructuresData.ItemCost>();
      case UpgradeSystem.Type.Ability_Eat:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.MONSTER_HEART, 1)
        };
      case UpgradeSystem.Type.Ability_UpgradeHeal:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.MONSTER_HEART, 1)
        };
      case UpgradeSystem.Type.Ability_Resurrection:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.MONSTER_HEART, 1)
        };
      case UpgradeSystem.Type.Ritual_Sacrifice:
        return UpgradeSystem.ApplyRitualDiscount(new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 75),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FOLLOWERS, 1)
        });
      case UpgradeSystem.Type.Ritual_ConsumeFollower:
        return UpgradeSystem.ApplyRitualDiscount(new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 75)
        });
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful1:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.DOCTRINE_STONE, 1)
        };
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful2:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.DISCIPLE_POINTS, 1)
        };
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful3:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.DISCIPLE_POINTS, 1)
        };
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful4:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.DISCIPLE_POINTS, 1)
        };
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful5:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.DISCIPLE_POINTS, 1)
        };
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful6:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.DISCIPLE_POINTS, 1)
        };
      case UpgradeSystem.Type.Ritual_UnlockWeapon:
        return new List<StructuresData.ItemCost>();
      case UpgradeSystem.Type.Ritual_UnlockCurse:
        return new List<StructuresData.ItemCost>();
      case UpgradeSystem.Type.Ritual_FasterBuilding:
        return UpgradeSystem.ApplyRitualDiscount(new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 75)
        });
      case UpgradeSystem.Type.Ritual_Enlightenment:
        return UpgradeSystem.ApplyRitualDiscount(new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 75)
        });
      case UpgradeSystem.Type.Ritual_WorkThroughNight:
        return UpgradeSystem.ApplyRitualDiscount(new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 75)
        });
      case UpgradeSystem.Type.Ritual_Holiday:
        return UpgradeSystem.ApplyRitualDiscount(new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 75)
        });
      case UpgradeSystem.Type.Ritual_AlmsToPoor:
        return UpgradeSystem.ApplyRitualDiscount(new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 50)
        });
      case UpgradeSystem.Type.Ritual_DonationRitual:
        return UpgradeSystem.ApplyRitualDiscount(new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 125)
        });
      case UpgradeSystem.Type.Ritual_Fast:
        return UpgradeSystem.ApplyRitualDiscount(new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 75)
        });
      case UpgradeSystem.Type.Ritual_Feast:
        return UpgradeSystem.ApplyRitualDiscount(new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 75)
        });
      case UpgradeSystem.Type.Ritual_HarvestRitual:
        return UpgradeSystem.ApplyRitualDiscount(new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 75)
        });
      case UpgradeSystem.Type.Ritual_FishingRitual:
        return UpgradeSystem.ApplyRitualDiscount(new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 75)
        });
      case UpgradeSystem.Type.Ritual_Ressurect:
        return UpgradeSystem.ApplyRitualDiscount(new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 150)
        });
      case UpgradeSystem.Type.Ritual_Funeral:
        return UpgradeSystem.ApplyRitualDiscount(new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 75),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FLOWER_RED, 5)
        });
      case UpgradeSystem.Type.Ritual_Fightpit:
        return UpgradeSystem.ApplyRitualDiscount(new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FOLLOWERS, 2),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 75)
        });
      case UpgradeSystem.Type.Ritual_Wedding:
        return UpgradeSystem.ApplyRitualDiscount(new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 75)
        });
      case UpgradeSystem.Type.Ritual_AssignFaithEnforcer:
        return UpgradeSystem.ApplyRitualDiscount(new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 75)
        });
      case UpgradeSystem.Type.Ritual_AssignTaxCollector:
        return UpgradeSystem.ApplyRitualDiscount(new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 75)
        });
      case UpgradeSystem.Type.Temple_DonationBox:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 10)
        };
      case UpgradeSystem.Type.Ability_BlackHeart:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.MONSTER_HEART, 1)
        };
      case UpgradeSystem.Type.Depreciated3:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 5)
        };
      case UpgradeSystem.Type.Depreciated4:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 20),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 5)
        };
      case UpgradeSystem.Type.Depreciated5:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 5)
        };
      case UpgradeSystem.Type.Depreciated6:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 15)
        };
      case UpgradeSystem.Type.Ability_TeleportHome:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.MONSTER_HEART, 1)
        };
      case UpgradeSystem.Type.Ritual_Brainwashing:
        return UpgradeSystem.ApplyRitualDiscount(new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.MUSHROOM_SMALL, 25)
        });
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful7:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FOLLOWERS, 19),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 40)
        };
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful8:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FOLLOWERS, 21),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 45)
        };
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful9:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FOLLOWERS, 22),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 50)
        };
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful10:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FOLLOWERS, 23),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 55)
        };
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful11:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FOLLOWERS, 24),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 60)
        };
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful12:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FOLLOWERS, 25),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 65)
        };
      case UpgradeSystem.Type.Ritual_Ascend:
        return UpgradeSystem.ApplyRitualDiscount(new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 75),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FOLLOWERS, 1)
        });
      case UpgradeSystem.Type.Ritual_FirePit:
        return UpgradeSystem.ApplyRitualDiscount(new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 25)
        });
      case UpgradeSystem.Type.Ritual_Halloween:
        return UpgradeSystem.ApplyRitualDiscount(new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.PUMPKIN, 40)
        });
      default:
        return new List<StructuresData.ItemCost>();
    }
  }

  public static bool UserCanAffordUpgrade(UpgradeSystem.Type type)
  {
    if (CheatConsole.BuildingsFree)
      return true;
    List<StructuresData.ItemCost> cost = UpgradeSystem.GetCost(type);
    for (int index = 0; index < cost.Count; ++index)
    {
      if (Inventory.GetItemQuantity((int) cost[index].CostItem) < cost[index].CostValue)
        return false;
    }
    return true;
  }

  public static bool CanAffordRatauAbility()
  {
    return UpgradeSystem.UserCanAffordUpgrade(UpgradeSystem.Type.Ability_Eat) && UpgradeSystem.UserCanAffordUpgrade(UpgradeSystem.Type.Ability_BlackHeart) && UpgradeSystem.UserCanAffordUpgrade(UpgradeSystem.Type.Ability_Resurrection) && UpgradeSystem.UserCanAffordUpgrade(UpgradeSystem.Type.Ability_TeleportHome);
  }

  public static bool CanAffordDoctrine()
  {
    return Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.DOCTRINE_STONE) >= 1 && (double) UpgradeSystem.GetCoolDownNormalised(UpgradeSystem.Type.Ritual_HeartsOfTheFaithful1) <= 0.0 && DoctrineUpgradeSystem.TrySermonsStillAvailable();
  }

  private static List<StructuresData.ItemCost> ApplyRitualDiscount(
    List<StructuresData.ItemCost> result)
  {
    if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Temple_CheaperRituals))
    {
      foreach (StructuresData.ItemCost itemCost in result)
      {
        if (itemCost.CostItem != InventoryItem.ITEM_TYPE.FOLLOWERS)
          itemCost.CostValue = Mathf.Max(1, Mathf.FloorToInt((float) itemCost.CostValue * 0.5f));
      }
    }
    return result;
  }

  public static bool PlayerHasRequiredBuildings(UpgradeSystem.Type Type)
  {
    List<StructureBrain.TYPES> requiredBuilding = UpgradeSystem.GetRequiredBuilding(Type);
    if (requiredBuilding == null)
      return true;
    foreach (StructureBrain.TYPES types in requiredBuilding)
    {
      if (!DataManager.Instance.HistoryOfStructures.Contains(types))
        return false;
    }
    return true;
  }

  public static List<StructureBrain.TYPES> GetRequiredBuilding(UpgradeSystem.Type Type)
  {
    switch (Type)
    {
      case UpgradeSystem.Type.Followers_Compost:
      case UpgradeSystem.Type.Economy_Refinery_2:
      case UpgradeSystem.Type.Building_ConfessionBooth:
      case UpgradeSystem.Type.Building_Graves:
      case UpgradeSystem.Type.Shrine_OfferingStatue:
      case UpgradeSystem.Type.Building_SiloSeed:
      case UpgradeSystem.Type.Building_SiloFertiliser:
      case UpgradeSystem.Type.Building_DancingFirepit:
      case UpgradeSystem.Type.Building_HarvestTotem:
      case UpgradeSystem.Type.Building_FoodStorage:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.REFINERY
        };
      case UpgradeSystem.Type.Economy_MineII:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.BLOODSTONE_MINE,
          StructureBrain.TYPES.REFINERY
        };
      case UpgradeSystem.Type.Economy_LumberyardII:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.LUMBERJACK_STATION,
          StructureBrain.TYPES.REFINERY
        };
      case UpgradeSystem.Type.Building_BetterBeds:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.BED,
          StructureBrain.TYPES.REFINERY
        };
      case UpgradeSystem.Type.Building_Temple2:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.TEMPLE
        };
      case UpgradeSystem.Type.Building_Kitchen:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.COOKING_FIRE,
          StructureBrain.TYPES.REFINERY
        };
      case UpgradeSystem.Type.Shrine_II:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.SHRINE,
          StructureBrain.TYPES.REFINERY
        };
      case UpgradeSystem.Type.Shrine_III:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.SHRINE_II,
          StructureBrain.TYPES.REFINERY
        };
      case UpgradeSystem.Type.Shrine_PassiveShrines:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.REFINERY
        };
      case UpgradeSystem.Type.Shrine_PassiveShrinesII:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.REFINERY,
          StructureBrain.TYPES.SHRINE_PASSIVE
        };
      case UpgradeSystem.Type.Shrine_IV:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.SHRINE_III,
          StructureBrain.TYPES.REFINERY
        };
      case UpgradeSystem.Type.Shrine_PassiveShrinesIII:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.REFINERY,
          StructureBrain.TYPES.SHRINE_PASSIVE_II
        };
      case UpgradeSystem.Type.Temple_III:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.TEMPLE_II,
          StructureBrain.TYPES.REFINERY
        };
      case UpgradeSystem.Type.Temple_IV:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.TEMPLE_III,
          StructureBrain.TYPES.REFINERY
        };
      case UpgradeSystem.Type.Building_Beds3:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.BED_2,
          StructureBrain.TYPES.REFINERY
        };
      case UpgradeSystem.Type.Building_Outhouse2:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.OUTHOUSE,
          StructureBrain.TYPES.REFINERY
        };
      case UpgradeSystem.Type.Building_Scarecrow2:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.SCARECROW,
          StructureBrain.TYPES.REFINERY
        };
      case UpgradeSystem.Type.Building_HarvestTotem2:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.HARVEST_TOTEM,
          StructureBrain.TYPES.REFINERY
        };
      case UpgradeSystem.Type.Building_HealingBay2:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.HEALING_BAY,
          StructureBrain.TYPES.REFINERY
        };
      case UpgradeSystem.Type.Building_FoodStorage2:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.BED_2,
          StructureBrain.TYPES.REFINERY
        };
      default:
        return (List<StructureBrain.TYPES>) null;
    }
  }

  public static void AddCooldown(UpgradeSystem.Type type, float duration)
  {
    if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Temple_FasterCoolDowns))
      duration *= 0.5f;
    DataManager.Instance.UpgradeCoolDowns.Add(new UpgradeSystem.UpgradeCoolDown()
    {
      TotalElapsedGameTime = TimeManager.TotalElapsedGameTime,
      Type = type,
      Duration = duration
    });
    System.Action onCoolDownAdded = UpgradeSystem.OnCoolDownAdded;
    if (onCoolDownAdded == null)
      return;
    onCoolDownAdded();
  }

  public static void ClearAllCoolDowns()
  {
    for (int index = DataManager.Instance.UpgradeCoolDowns.Count - 1; index >= 0; --index)
    {
      if (!UpgradeSystem.IsRitualActive(DataManager.Instance.UpgradeCoolDowns[index].Type))
        DataManager.Instance.UpgradeCoolDowns.RemoveAt(index);
    }
  }

  public static float GetCoolDownNormalised(UpgradeSystem.Type Type)
  {
    foreach (UpgradeSystem.UpgradeCoolDown upgradeCoolDown in DataManager.Instance.UpgradeCoolDowns)
    {
      if (upgradeCoolDown.Type == Type)
      {
        if ((double) TimeManager.TotalElapsedGameTime < (double) upgradeCoolDown.TotalElapsedGameTime + (double) upgradeCoolDown.Duration)
          return (float) (1.0 - ((double) TimeManager.TotalElapsedGameTime - (double) upgradeCoolDown.TotalElapsedGameTime) / (double) upgradeCoolDown.Duration);
        DataManager.Instance.UpgradeCoolDowns.Remove(upgradeCoolDown);
        return 0.0f;
      }
    }
    return 0.0f;
  }

  public static UpgradeSystem.Type[] AllRituals()
  {
    List<UpgradeSystem.Type> typeList = new List<UpgradeSystem.Type>();
    typeList.AddRange((IEnumerable<UpgradeSystem.Type>) UpgradeSystem.SecondaryRituals);
    typeList.AddRange((IEnumerable<UpgradeSystem.Type>) UpgradeSystem.SecondaryRitualPairs);
    typeList.AddRange((IEnumerable<UpgradeSystem.Type>) UpgradeSystem.SingleRituals);
    typeList.AddRange((IEnumerable<UpgradeSystem.Type>) UpgradeSystem.SpecialRituals);
    return typeList.ToArray();
  }

  public static bool IsSpecialRitual(UpgradeSystem.Type type)
  {
    return UpgradeSystem.SpecialRituals.Contains<UpgradeSystem.Type>(type);
  }

  public static bool IsUpgradeMaxed(UpgradeSystem.Type upgrade)
  {
    switch (upgrade)
    {
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful1:
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful2:
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful3:
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful4:
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful5:
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful6:
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful7:
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful8:
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful9:
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful10:
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful11:
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful12:
        return DataManager.Instance.PLAYER_HEARTS_LEVEL + DataManager.Instance.PLAYER_DAMAGE_LEVEL >= 12;
      case UpgradeSystem.Type.Ritual_UnlockWeapon:
        return true;
      case UpgradeSystem.Type.Ritual_UnlockCurse:
        return true;
      default:
        return false;
    }
  }

  public enum UpgradeTypes
  {
    BlackSouls,
    Devotion,
    Both,
  }

  public enum Category
  {
    NONE,
    CULT,
    ECONOMY,
    FOLLOWERS,
    COMBAT,
    FAITH,
    ASTHETIC,
    DEATH,
    POOP,
    FARMING,
    SLEEP,
    ILLNESS,
    P_HEALTH,
    P_STRENGTH,
    P_WEAPON,
    P_CURSE,
    P_FERVOR,
  }

  public enum Type
  {
    Combat_ExtraHeart1,
    Combat_ExtraHeart2,
    Combat_Swords2,
    Combat_Arrows,
    Combat_Arrows2,
    Combat_Shrine,
    Combat_Dash2,
    Combat_MoreBlackSouls,
    Combat_EnemiesDropXP,
    Followers_Shelter,
    Followers_Bathrooms,
    Followers_Clothing,
    Followers_FoodBasics,
    Followers_FoodAutomation,
    Followers_Compost,
    Followers_Reproduction,
    Followers_Chef,
    Followers_AdvancedFoods,
    Followers_SeedSilo,
    Cult_RitualCircle,
    Cult_ConfessionBooth,
    Cult_Punishment,
    Cult_SacrificialStone,
    Cult_ReusingCorpses,
    Cult_RaiseFromDead,
    Cult_Brainwashing,
    Cult_Propaganda,
    Cult_Punishment2,
    Economy_Foraging,
    Economy_Foraging2,
    Economy_FishingRod,
    Economy_MineII,
    Economy_Mine,
    Economy_Lumberyard,
    Economy_Refinery,
    Economy_Refinery_2,
    Economy_LumberyardII,
    Combat_RedHeartShrine,
    Combat_BlueHeartShrine,
    Combat_BlackHeartShrine,
    Combat_TarotCardShrine,
    Combat_DamageShrine,
    Building_Temple,
    Building_Farms,
    Building_AdvancedFarming,
    Building_Prison,
    Building_Outhouse,
    Building_LumberMine,
    Building_Burial,
    Building_Apothecary,
    Building_ConfessionBooth,
    Building_Beds,
    Building_FollowerFarming,
    Building_NaturalBurial,
    Ability_Eat,
    Building_BetterBeds,
    Building_BetterShrine,
    Building_Graves,
    Ability_UpgradeHeal,
    Ability_Resurrection,
    Ritual_Sacrifice,
    Ritual_Reindoctrinate,
    Ritual_ConsumeFollower,
    Ritual_Placeholder2,
    Building_Temple2,
    Ritual_HeartsOfTheFaithful1,
    Ritual_HeartsOfTheFaithful2,
    Ritual_HeartsOfTheFaithful3,
    Ritual_HeartsOfTheFaithful4,
    Ritual_HeartsOfTheFaithful5,
    Ritual_HeartsOfTheFaithful6,
    Building_Kitchen,
    Shrine_II,
    Shrine_Flame,
    Shrine_OfferingStatue,
    Shrine_III,
    Shrine_PassiveShrines,
    Shrine_PassiveShrinesII,
    Shrine_FlameIII,
    Shrine_IV,
    Shrine_PassiveShrinesIII,
    Shrine_PassiveShrinesFlames,
    Shrine_FlameII,
    Temple_III,
    Temple_IV,
    Temple_MonksUpgrade,
    Temple_SermonEfficiency,
    Temple_SermonEfficiencyII,
    Temple_SuperSermon,
    Temple_CheaperRituals,
    Temple_CheaperRitualsII,
    Temple_FasterCoolDowns,
    Temple_FasterCoolDownsII,
    Temple_DonationBoxII,
    UseForSomethingElse1,
    UseForSomethingElse2,
    UseForSomethingElse3,
    UseForSomethingElse4,
    Ritual_UnlockWeapon,
    Ritual_UnlockCurse,
    Ritual_FasterBuilding,
    Ritual_Enlightenment,
    Ritual_WorkThroughNight,
    Ritual_Holiday,
    Ritual_AlmsToPoor,
    Ritual_DonationRitual,
    Ritual_Fast,
    Ritual_Feast,
    Ritual_HarvestRitual,
    Ritual_FishingRitual,
    Ritual_Ressurect,
    Ritual_Funeral,
    Ritual_Fightpit,
    Ritual_Wedding,
    Ritual_AssignFaithEnforcer,
    Ritual_AssignTaxCollector,
    Building_PropagandaSpeakers,
    Building_Missionary,
    Building_Beds3,
    Building_FeastTable,
    Building_SiloSeed,
    Building_SiloFertiliser,
    Building_Surveillance,
    Building_FishingHut2,
    Building_Outhouse2,
    Building_Scarecrow2,
    Building_HarvestTotem2,
    Building_DancingFirepit,
    Temple_DonationBox,
    Ability_BlackHeart,
    Depreciated3,
    Depreciated4,
    Depreciated5,
    Depreciated6,
    Building_BodyPit,
    Building_HarvestTotem,
    Building_HealingBay,
    Building_HealingBay2,
    Building_FoodStorage,
    Building_Decorations1,
    Building_Decorations2,
    Building_ShrinesOfNature,
    Ability_TeleportHome,
    Building_FoodStorage2,
    Building_JanitorStation,
    Building_DemonSummoner,
    Ritual_Brainwashing,
    Ritual_Blank,
    Ritual_HeartsOfTheFaithful7,
    Ritual_HeartsOfTheFaithful8,
    Ritual_HeartsOfTheFaithful9,
    Ritual_HeartsOfTheFaithful10,
    Ritual_HeartsOfTheFaithful11,
    Ritual_HeartsOfTheFaithful12,
    Ritual_Ascend,
    Building_DemonSummoner_2,
    Building_DemonSummoner_3,
    PUpgrade_Heart_1,
    PUpgrade_Heart_2,
    PUpgrade_Heart_3,
    PUpgrade_Heart_4,
    PUpgrade_Heart_5,
    PUpgrade_Heart_6,
    PUpgrade_Sword_0,
    PUpgrade_Sword_1,
    PUpgrade_Sword_2,
    PUpgrade_Sword_3,
    PUpgrade_Axe_0,
    PUpgrade_Axe_1,
    PUpgrade_Axe_2,
    PUpgrade_Axe_3,
    PUpgrade_Dagger_0,
    PUpgrade_Dagger_1,
    PUpgrade_Dagger_2,
    PUpgrade_Dagger_3,
    PUpgrade_Gauntlets_0,
    PUpgrade_Gauntlets_1,
    PUpgrade_Gauntlets_2,
    PUpgrade_Gauntlets_3,
    PUpgrade_Hammer_0,
    PUpgrade_Hammer_1,
    PUpgrade_Hammer_2,
    PUpgrade_Hammer_3,
    PUpgrade_Fireball_0,
    PUpgrade_Fireball_1,
    PUpgrade_Fireball_2,
    PUpgrade_Fireball_3,
    PUpgrade_EnemyBlast_0,
    PUpgrade_EnemyBlast_1,
    PUpgrade_EnemyBlast_2,
    PUpgrade_EnemyBlast_3,
    PUpgrade_ProjectileAOE_0,
    PUpgrade_ProjectileAOE_1,
    PUpgrade_ProjectileAOE_2,
    PUpgrade_ProjectileAOE_3,
    PUpgrade_Tentacles_0,
    PUpgrade_Tentacles_1,
    PUpgrade_Tentacles_2,
    PUpgrade_Tentacles_3,
    PUpgrade_Vortex_0,
    PUpgrade_Vortex_1,
    PUpgrade_Vortex_2,
    PUpgrade_Vortex_3,
    PUpgrade_MegaSlash_0,
    PUpgrade_MegaSlash_1,
    PUpgrade_MegaSlash_2,
    PUpgrade_MegaSlash_3,
    PUpgrade_Ammo_1,
    PUpgrade_Ammo_2,
    PUpgrade_Ammo_3,
    Building_MissionaryII,
    Building_MissionaryIII,
    Building_KitchenII,
    Building_FarmStationII,
    PUpgrade_WeaponCritHit,
    PUpgrade_WeaponPoison,
    PUpgrade_WeaponFervor,
    PUpgrade_WeaponNecromancy,
    PUpgrade_WeaponHeal,
    PUpgrade_WeaponGodly,
    PUpgrade_CursePack1,
    PUpgrade_CursePack2,
    PUpgrade_CursePack3,
    PUpgrade_StartingWeapon_1,
    PUpgrade_StartingWeapon_2,
    PUpgrade_StartingWeapon_3,
    PUpgrade_StartingWeapon_4,
    PUpgrade_StartingWeapon_5,
    PUpgrade_StartingWeapon_6,
    Ritual_FirePit,
    PUpgrade_CursePack4,
    PUpgrade_CursePack5,
    Ritual_Halloween,
    Count,
  }

  public delegate void UnlockEvent(UpgradeSystem.Type upgradeType);

  [Serializable]
  public class UpgradeCoolDown
  {
    public UpgradeSystem.Type Type;
    public float TotalElapsedGameTime;
    public float Duration;
  }
}
