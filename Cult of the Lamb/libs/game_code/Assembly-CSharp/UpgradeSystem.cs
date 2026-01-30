// Decompiled with JetBrains decompiler
// Type: UpgradeSystem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;
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
  public static System.Action OnDLCAbilityPointDelta;
  public static System.Action OnBuildingUnlocked;
  public static System.Action OnDisciplePointDelta;
  public static Action<UpgradeSystem.Type> OnAbilityUnlocked;
  public static Action<UpgradeSystem.Type> OnAbilityLocked;
  public static List<UpgradeSystem.Type> UnlocksToReveal = new List<UpgradeSystem.Type>();
  public static System.Action OnCoolDownAdded;
  public static UpgradeSystem.Type PrimaryRitual1 = UpgradeSystem.Type.Ritual_HeartsOfTheFaithful1;
  public static UpgradeSystem.Type[] SecondaryRituals = new UpgradeSystem.Type[6]
  {
    UpgradeSystem.Type.Ritual_FirePit,
    UpgradeSystem.Type.Ritual_Sacrifice,
    UpgradeSystem.Type.Ritual_Brainwashing,
    UpgradeSystem.Type.Ritual_BecomeDisciple,
    UpgradeSystem.Type.Ritual_Snowman,
    UpgradeSystem.Type.Ritual_FollowerWedding
  };
  public static UpgradeSystem.Type[] SecondaryRitualPairs = new UpgradeSystem.Type[22]
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
    UpgradeSystem.Type.Ritual_Wedding,
    UpgradeSystem.Type.Ritual_Nudism,
    UpgradeSystem.Type.Ritual_Purge,
    UpgradeSystem.Type.Ritual_AtoneSin,
    UpgradeSystem.Type.Ritual_Cannibal,
    UpgradeSystem.Type.Ritual_RanchHarvest,
    UpgradeSystem.Type.Ritual_RanchMeat
  };
  public static UpgradeSystem.Type[] SingleRituals = new UpgradeSystem.Type[2]
  {
    UpgradeSystem.Type.Ritual_Ascend,
    UpgradeSystem.Type.Ritual_ConvertToRot
  };
  public static UpgradeSystem.Type[] SpecialRituals = new UpgradeSystem.Type[1]
  {
    UpgradeSystem.Type.Ritual_Halloween
  };
  public static UpgradeSystem.Type[] LegendaryWeapons = new UpgradeSystem.Type[7]
  {
    UpgradeSystem.Type.Blacksmith_Legendary_Hammer,
    UpgradeSystem.Type.Blacksmith_Legendary_Sword,
    UpgradeSystem.Type.Blacksmith_Legendary_Dagger,
    UpgradeSystem.Type.Blacksmith_Legendary_Gauntlets,
    UpgradeSystem.Type.Blacksmith_Legendary_Blunderbuss,
    UpgradeSystem.Type.Blacksmith_Legendary_Axe,
    UpgradeSystem.Type.Blacksmith_Legendary_Chain
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
      if (value > DataManager.Instance.AbilityPoints && GameManager.HasUnlockAvailable())
        NotificationCentreScreen.Play(NotificationCentre.NotificationType.NewUpgradePoint);
      DataManager.Instance.AbilityPoints = Mathf.Max(value, 0);
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
    if (Type == UpgradeSystem.Type.Ritual_FirePit && UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ritual_FirePit_2))
      Type = UpgradeSystem.Type.Ritual_FirePit_2;
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
    if (Type == UpgradeSystem.Type.PUpgrade_CursePack1 || Type == UpgradeSystem.Type.PUpgrade_CursePack2 || Type == UpgradeSystem.Type.PUpgrade_CursePack3 || Type == UpgradeSystem.Type.PUpgrade_CursePack4 || Type == UpgradeSystem.Type.PUpgrade_CursePack5 || Type == UpgradeSystem.Type.Curses_Fire || Type == UpgradeSystem.Type.Curses_Teleport || Type == UpgradeSystem.Type.Curses_Barrier)
      return string.Format(ScriptLocalization.UpgradeSystem_PUpgrade_CursePacks.Description, (object) 3);
    if (Type == UpgradeSystem.Type.Building_FollowerFarming)
      return $"{$"<color=#FFD201>{StructuresData.GetLocalizedNameStatic(StructureBrain.TYPES.FARM_STATION)}: </color>{StructuresData.LocalizedDescription(StructureBrain.TYPES.FARM_STATION)}" + "<br><br>"}<color=#FFD201>{StructuresData.GetLocalizedNameStatic(StructureBrain.TYPES.SILO_SEED)}: </color>{StructuresData.LocalizedDescription(StructureBrain.TYPES.SILO_SEED)}";
    if (Type == UpgradeSystem.Type.Shrine_Flame)
      return $"{$"<color=#FFD201>{LocalizationManager.GetTranslation("UpgradeSystem/Shrine_Flame_Building/Name")}: </color>{LocalizationManager.GetTranslation("UpgradeSystem/Shrine_Flame_Building/Description")}" + "<br><br>"}<color=#FFD201>{LocalizationManager.GetTranslation("UpgradeSystem/Shrine_PassiveShrinesFlames/Name")}: </color>{LocalizationManager.GetTranslation("UpgradeSystem/Shrine_PassiveShrinesFlames/Description")}";
    if (Type == UpgradeSystem.Type.Ritual_FirePit && UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ritual_FirePit_2))
      Type = UpgradeSystem.Type.Ritual_FirePit_2;
    else if (Type == UpgradeSystem.Type.Ritual_FirePit)
      return ScriptLocalization.FollowerInteractions_GiveQuest.UseFirePit;
    if (Type == UpgradeSystem.Type.Ritual_Cannibal)
      return LocalizationManager.GetTranslation($"UpgradeSystem/{Type}/Description");
    if (Type.ToString().Contains("PUpgrade_"))
      return LocalizationManager.GetTranslation($"UpgradeSystem/{Type}/Description");
    if (Type.ToString().Contains("Ritual_"))
      return LocalizationManager.GetTranslation($"UpgradeSystem/{Type}/Description");
    if (Type == UpgradeSystem.Type.Temple_CheaperRituals)
      Type = UpgradeSystem.Type.Temple_CheaperRitualsII;
    if (Type == UpgradeSystem.Type.Temple_FasterCoolDowns)
      Type = UpgradeSystem.Type.Temple_FasterCoolDownsII;
    if (Type <= UpgradeSystem.Type.Relic_Pack2)
    {
      if (Type <= UpgradeSystem.Type.Temple_DonationBoxII)
      {
        if (Type != UpgradeSystem.Type.Ability_Eat && Type != UpgradeSystem.Type.Ability_Resurrection)
        {
          switch (Type)
          {
            case UpgradeSystem.Type.Shrine_Flame:
            case UpgradeSystem.Type.Shrine_FlameIII:
            case UpgradeSystem.Type.Shrine_PassiveShrinesFlames:
            case UpgradeSystem.Type.Shrine_FlameII:
            case UpgradeSystem.Type.Temple_MonksUpgrade:
            case UpgradeSystem.Type.Temple_SermonEfficiency:
            case UpgradeSystem.Type.Temple_SermonEfficiencyII:
            case UpgradeSystem.Type.Temple_CheaperRituals:
            case UpgradeSystem.Type.Temple_CheaperRitualsII:
            case UpgradeSystem.Type.Temple_FasterCoolDowns:
            case UpgradeSystem.Type.Temple_FasterCoolDownsII:
            case UpgradeSystem.Type.Temple_DonationBoxII:
              break;
            default:
              goto label_31;
          }
        }
      }
      else if ((uint) (Type - 128 /*0x80*/) > 1U && (uint) (Type - 139) > 3U && (uint) (Type - 233) > 3U)
        goto label_31;
    }
    else if (Type <= UpgradeSystem.Type.Building_MatingTent)
    {
      if (Type != UpgradeSystem.Type.Relic_Pack_Default && Type != UpgradeSystem.Type.Building_UpgradedIndoctrination && Type != UpgradeSystem.Type.Building_MatingTent)
        goto label_31;
    }
    else if (Type <= UpgradeSystem.Type.Relics_Ice)
    {
      if (Type != UpgradeSystem.Type.Building_LeaderTent && (uint) (Type - 304) > 1U)
        goto label_31;
    }
    else if ((uint) (Type - 324) > 1U && Type != UpgradeSystem.Type.Ability_WinterChoice)
      goto label_31;
    return LocalizationManager.GetTranslation($"UpgradeSystem/{Type}/Description");
label_31:
    Debug.Log((object) ("D " + Type.ToString()));
    return StructuresData.LocalizedDescription(UpgradeSystem.GetStructureTypeFromUpgrade(Type));
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
      case UpgradeSystem.Type.Building_FarmStationII:
        return StructureBrain.TYPES.FARM_STATION_II;
      case UpgradeSystem.Type.Building_Morgue_1:
        return StructureBrain.TYPES.MORGUE_1;
      case UpgradeSystem.Type.Building_Morgue_2:
        return StructureBrain.TYPES.MORGUE_2;
      case UpgradeSystem.Type.Building_Crypt_1:
        return StructureBrain.TYPES.CRYPT_1;
      case UpgradeSystem.Type.Building_Crypt_2:
        return StructureBrain.TYPES.CRYPT_2;
      case UpgradeSystem.Type.Building_Crypt_3:
        return StructureBrain.TYPES.CRYPT_3;
      case UpgradeSystem.Type.Building_Shared_House:
        return StructureBrain.TYPES.SHARED_HOUSE;
      case UpgradeSystem.Type.Building_LightningRod:
        return StructureBrain.TYPES.LIGHTNING_ROD;
      case UpgradeSystem.Type.Building_Tailor:
        return StructureBrain.TYPES.TAILOR;
      case UpgradeSystem.Type.Building_PoopBucket:
        return StructureBrain.TYPES.POOP_BUCKET;
      case UpgradeSystem.Type.Building_SeedBucket:
        return StructureBrain.TYPES.SEED_BUCKET;
      case UpgradeSystem.Type.Building_JanitorStation_2:
        return StructureBrain.TYPES.JANITOR_STATION_2;
      case UpgradeSystem.Type.Building_Shrine_Disciple_Boost:
        return StructureBrain.TYPES.SHRINE_DISCIPLE_BOOST;
      case UpgradeSystem.Type.Building_Pub:
        return StructureBrain.TYPES.PUB;
      case UpgradeSystem.Type.Building_Shrine_Pleasure:
        return StructureBrain.TYPES.SHRINE_PLEASURE;
      case UpgradeSystem.Type.Building_Drum:
        return StructureBrain.TYPES.DRUM_CIRCLE;
      case UpgradeSystem.Type.Building_MatingTent:
        return StructureBrain.TYPES.MATING_TENT;
      case UpgradeSystem.Type.Building_Shrine_Disciple_Collection:
        return StructureBrain.TYPES.SHRINE_DISCIPLE_COLLECTION;
      case UpgradeSystem.Type.Building_Hatchery:
        return StructureBrain.TYPES.HATCHERY;
      case UpgradeSystem.Type.Building_Pub_2:
        return StructureBrain.TYPES.PUB_2;
      case UpgradeSystem.Type.Building_Hatchery_2:
        return StructureBrain.TYPES.HATCHERY_2;
      case UpgradeSystem.Type.Building_LeaderTent:
        return StructureBrain.TYPES.LEADER_TENT;
      case UpgradeSystem.Type.Building_Daycare:
        return StructureBrain.TYPES.DAYCARE;
      case UpgradeSystem.Type.Building_Knucklebones:
        return StructureBrain.TYPES.KNUCKLEBONES_ARENA;
      case UpgradeSystem.Type.Building_Weather_Vane:
        return StructureBrain.TYPES.WEATHER_VANE;
      case UpgradeSystem.Type.Building_Volcanic_Spa:
        return StructureBrain.TYPES.VOLCANIC_SPA;
      case UpgradeSystem.Type.Building_Wooly_Shack:
        return StructureBrain.TYPES.WOOLY_SHACK;
      case UpgradeSystem.Type.Building_Ranch:
        return StructureBrain.TYPES.RANCH;
      case UpgradeSystem.Type.Building_Medic:
        return StructureBrain.TYPES.MEDIC;
      case UpgradeSystem.Type.Building_RanchTrough:
        return StructureBrain.TYPES.RANCH_TROUGH;
      case UpgradeSystem.Type.Building_Ranch_2:
        return StructureBrain.TYPES.RANCH_2;
      case UpgradeSystem.Type.Building_RanchHutch:
        return StructureBrain.TYPES.RANCH_HUTCH;
      case UpgradeSystem.Type.Building_RacingGate:
        return StructureBrain.TYPES.RACING_GATE;
      case UpgradeSystem.Type.Building_Logistics:
        return StructureBrain.TYPES.LOGISTICS;
      case UpgradeSystem.Type.Building_Wolf_Trap:
        return StructureBrain.TYPES.WOLF_TRAP;
      case UpgradeSystem.Type.Building_Furnace:
        return StructureBrain.TYPES.FURNACE_1;
      case UpgradeSystem.Type.Building_Furnace_2:
        return StructureBrain.TYPES.FURNACE_2;
      case UpgradeSystem.Type.Building_Furnace_3:
        return StructureBrain.TYPES.FURNACE_3;
      case UpgradeSystem.Type.Building_LightningRod_2:
        return StructureBrain.TYPES.LIGHTNING_ROD_2;
      case UpgradeSystem.Type.Building_Toolshed:
        return StructureBrain.TYPES.TOOLSHED;
      case UpgradeSystem.Type.Building_Farm_Crop_Grower:
        return StructureBrain.TYPES.FARM_CROP_GROWER;
      case UpgradeSystem.Type.Building_Trait_Manipulator_1:
        return StructureBrain.TYPES.TRAIT_MANIPULATOR_1;
      case UpgradeSystem.Type.Building_Trait_Manipulator_2:
        return StructureBrain.TYPES.TRAIT_MANIPULATOR_2;
      case UpgradeSystem.Type.Building_Trait_Manipulator_3:
        return StructureBrain.TYPES.TRAIT_MANIPULATOR_3;
      case UpgradeSystem.Type.Building_RanchChoppingBlock:
        return StructureBrain.TYPES.RANCH_CHOPPING_BLOCK;
      case UpgradeSystem.Type.Building_RotstoneMine:
        return StructureBrain.TYPES.ROTSTONE_MINE;
      case UpgradeSystem.Type.Building_RotstoneMine_2:
        return StructureBrain.TYPES.ROTSTONE_MINE_2;
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
      case UpgradeSystem.Type.Ritual_Purge:
        return FollowerBrainStats.IsPurge;
      case UpgradeSystem.Type.Ritual_Nudism:
        return FollowerBrainStats.IsNudism;
      default:
        return false;
    }
  }

  public static bool UnlockAbility(UpgradeSystem.Type Type, bool instant = false)
  {
    Debug.Log((object) ("UnlockAbilit " + Type.ToString()));
    if (UpgradeSystem.UnlockedUpgrades.Contains(Type))
      return false;
    UpgradeSystem.UnlockedUpgrades.Add(Type);
    UpgradeSystem.UnlocksToReveal.Add(Type);
    Action<UpgradeSystem.Type> onAbilityUnlocked = UpgradeSystem.OnAbilityUnlocked;
    if (onAbilityUnlocked != null)
      onAbilityUnlocked(Type);
    UpgradeSystem.CheckAchievementsOnUnlock(Type);
    if (instant)
      GameManager.GetInstance().StartCoroutine((IEnumerator) UpgradeSystem.ListOfUnlocksRoutine());
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
    {
      if (DungeonSandboxManager.Active)
        GameManager.GetInstance().StartCoroutine((IEnumerator) UpgradeSystem.OnUnlockAbility(Type));
      else
        yield return (object) GameManager.GetInstance().StartCoroutine((IEnumerator) UpgradeSystem.OnUnlockAbility(Type));
    }
    UpgradeSystem.UnlocksToReveal.Clear();
  }

  public static IEnumerator OnUnlockAbility(UpgradeSystem.Type Type)
  {
    if (!DungeonSandboxManager.Active)
      yield return (object) null;
    Debug.Log((object) ("Type unlocked: " + Type.ToString()));
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
        Debug.Log((object) ("UNLOCK : " + Type.ToString()));
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
      case UpgradeSystem.Type.Building_LightningRod:
      case UpgradeSystem.Type.Building_Weather_Vane:
      case UpgradeSystem.Type.Building_Volcanic_Spa:
      case UpgradeSystem.Type.Building_Ranch:
      case UpgradeSystem.Type.Building_Medic:
      case UpgradeSystem.Type.Building_RanchTrough:
      case UpgradeSystem.Type.Building_Ranch_2:
      case UpgradeSystem.Type.Building_RanchHutch:
      case UpgradeSystem.Type.Building_RacingGate:
      case UpgradeSystem.Type.Building_Logistics:
      case UpgradeSystem.Type.Building_Wolf_Trap:
      case UpgradeSystem.Type.Building_Furnace:
      case UpgradeSystem.Type.Building_Furnace_2:
      case UpgradeSystem.Type.Building_Furnace_3:
      case UpgradeSystem.Type.Building_LightningRod_2:
      case UpgradeSystem.Type.Building_Toolshed:
      case UpgradeSystem.Type.Building_Trait_Manipulator_1:
      case UpgradeSystem.Type.Building_Trait_Manipulator_2:
      case UpgradeSystem.Type.Building_Trait_Manipulator_3:
      case UpgradeSystem.Type.Building_RanchChoppingBlock:
      case UpgradeSystem.Type.Building_RotstoneMine:
      case UpgradeSystem.Type.Building_RotstoneMine_2:
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
        DataManager.Instance.AddWeapon(EquipmentType.Blunderbuss_Critical);
        DataManager.Instance.AddWeapon(EquipmentType.Chain_Critical);
        DataManager.Instance.ForcedStartingWeapon = (double) UnityEngine.Random.value > 0.5 ? ((double) UnityEngine.Random.value > 0.5 ? EquipmentType.Sword_Critical : EquipmentType.Dagger_Critical) : EquipmentType.Axe_Critical;
        break;
      case UpgradeSystem.Type.PUpgrade_WeaponPoison:
        DataManager.Instance.AddWeapon(EquipmentType.Axe_Poison);
        DataManager.Instance.AddWeapon(EquipmentType.Sword_Poison);
        DataManager.Instance.AddWeapon(EquipmentType.Dagger_Poison);
        DataManager.Instance.AddWeapon(EquipmentType.Hammer_Poison);
        DataManager.Instance.AddWeapon(EquipmentType.Gauntlet_Poison);
        DataManager.Instance.AddWeapon(EquipmentType.Blunderbuss_Poison);
        DataManager.Instance.AddWeapon(EquipmentType.Chain_Poison);
        DataManager.Instance.ForcedStartingWeapon = (double) UnityEngine.Random.value > 0.5 ? ((double) UnityEngine.Random.value > 0.5 ? EquipmentType.Sword_Poison : EquipmentType.Dagger_Poison) : EquipmentType.Axe_Poison;
        break;
      case UpgradeSystem.Type.PUpgrade_WeaponFervor:
        DataManager.Instance.AddWeapon(EquipmentType.Axe_Fervour);
        DataManager.Instance.AddWeapon(EquipmentType.Sword_Fervour);
        DataManager.Instance.AddWeapon(EquipmentType.Dagger_Fervour);
        DataManager.Instance.AddWeapon(EquipmentType.Hammer_Fervour);
        DataManager.Instance.AddWeapon(EquipmentType.Gauntlet_Fervour);
        DataManager.Instance.AddWeapon(EquipmentType.Blunderbuss_Fervour);
        DataManager.Instance.AddWeapon(EquipmentType.Chain_Fervour);
        DataManager.Instance.ForcedStartingWeapon = (double) UnityEngine.Random.value > 0.5 ? ((double) UnityEngine.Random.value > 0.5 ? EquipmentType.Sword_Fervour : EquipmentType.Dagger_Fervour) : EquipmentType.Axe_Fervour;
        break;
      case UpgradeSystem.Type.PUpgrade_WeaponNecromancy:
        DataManager.Instance.AddWeapon(EquipmentType.Axe_Nercomancy);
        DataManager.Instance.AddWeapon(EquipmentType.Sword_Nercomancy);
        DataManager.Instance.AddWeapon(EquipmentType.Dagger_Nercomancy);
        DataManager.Instance.AddWeapon(EquipmentType.Hammer_Nercomancy);
        DataManager.Instance.AddWeapon(EquipmentType.Gauntlet_Nercomancy);
        DataManager.Instance.AddWeapon(EquipmentType.Blunderbuss_Nercomancy);
        DataManager.Instance.AddWeapon(EquipmentType.Chain_Nercomancy);
        DataManager.Instance.ForcedStartingWeapon = (double) UnityEngine.Random.value > 0.5 ? ((double) UnityEngine.Random.value > 0.5 ? EquipmentType.Sword_Nercomancy : EquipmentType.Dagger_Nercomancy) : EquipmentType.Axe_Nercomancy;
        break;
      case UpgradeSystem.Type.PUpgrade_WeaponHeal:
        DataManager.Instance.AddWeapon(EquipmentType.Axe_Healing);
        DataManager.Instance.AddWeapon(EquipmentType.Sword_Healing);
        DataManager.Instance.AddWeapon(EquipmentType.Dagger_Healing);
        DataManager.Instance.AddWeapon(EquipmentType.Hammer_Healing);
        DataManager.Instance.AddWeapon(EquipmentType.Gauntlet_Healing);
        DataManager.Instance.AddWeapon(EquipmentType.Blunderbuss_Healing);
        DataManager.Instance.AddWeapon(EquipmentType.Chain_Healing);
        DataManager.Instance.ForcedStartingWeapon = (double) UnityEngine.Random.value > 0.5 ? ((double) UnityEngine.Random.value > 0.5 ? EquipmentType.Sword_Healing : EquipmentType.Dagger_Healing) : EquipmentType.Axe_Healing;
        break;
      case UpgradeSystem.Type.PUpgrade_WeaponGodly:
        DataManager.Instance.AddWeapon(EquipmentType.Axe_Godly);
        DataManager.Instance.AddWeapon(EquipmentType.Sword_Godly);
        DataManager.Instance.AddWeapon(EquipmentType.Dagger_Godly);
        DataManager.Instance.AddWeapon(EquipmentType.Hammer_Godly);
        DataManager.Instance.AddWeapon(EquipmentType.Gauntlet_Godly);
        DataManager.Instance.AddWeapon(EquipmentType.Blunderbuss_Godly);
        DataManager.Instance.AddWeapon(EquipmentType.Chain_Godly);
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
      case UpgradeSystem.Type.Relics_Blessed_1:
        if (!DataManager.Instance.ForceDammedRelic)
          DataManager.Instance.ForceBlessedRelic = true;
        EquipmentManager.UnlockRelics(Type);
        break;
      case UpgradeSystem.Type.Relic_Pack1:
      case UpgradeSystem.Type.Relic_Pack2:
      case UpgradeSystem.Type.Relic_Pack_Default:
      case UpgradeSystem.Type.Relic_Pack_Coop:
      case UpgradeSystem.Type.Relic_Pack_Corrupted:
      case UpgradeSystem.Type.Relics_Fire:
      case UpgradeSystem.Type.Relics_Ice:
        EquipmentManager.UnlockRelics(Type);
        break;
      case UpgradeSystem.Type.Relics_Dammed_1:
        if (!DataManager.Instance.ForceBlessedRelic)
          DataManager.Instance.ForceDammedRelic = true;
        EquipmentManager.UnlockRelics(Type);
        break;
      case UpgradeSystem.Type.Curses_Fire:
        DataManager.Instance.AddCurse(EquipmentType.EnemyBlast_Flame);
        DataManager.Instance.AddCurse(EquipmentType.MegaSlash_Flame);
        DataManager.Instance.AddCurse(EquipmentType.Tentacles_Flame);
        DataManager.Instance.ForcedStartingCurse = (double) UnityEngine.Random.value > 0.5 ? ((double) UnityEngine.Random.value > 0.5 ? EquipmentType.EnemyBlast_Flame : EquipmentType.MegaSlash_Flame) : EquipmentType.Tentacles_Flame;
        break;
      case UpgradeSystem.Type.Curses_Teleport:
        DataManager.Instance.AddCurse(EquipmentType.Teleport);
        DataManager.Instance.AddCurse(EquipmentType.Teleport_Goop);
        DataManager.Instance.AddCurse(EquipmentType.Teleport_Invincible);
        DataManager.Instance.ForcedStartingCurse = EquipmentType.Teleport;
        break;
      case UpgradeSystem.Type.Curses_Barrier:
        DataManager.Instance.AddCurse(EquipmentType.Barrier);
        DataManager.Instance.AddCurse(EquipmentType.Barrier_Absorb);
        DataManager.Instance.AddCurse(EquipmentType.Barrier_Deflection);
        DataManager.Instance.ForcedStartingCurse = EquipmentType.Barrier;
        break;
      case UpgradeSystem.Type.DLC_Building_Decorations1:
        DataManager.Instance.UnlockedStructures.Add(StructureBrain.TYPES.DECORATION_DLC_YNGYA_TREEPOT);
        DataManager.Instance.UnlockedStructures.Add(StructureBrain.TYPES.DECORATION_DLC_YNGYA_TREEBUSH);
        DataManager.Instance.UnlockedStructures.Add(StructureBrain.TYPES.DECORATION_DLC_YNGYA_TALLFLOWERS);
        DataManager.Instance.UnlockedStructures.Add(StructureBrain.TYPES.DECORATION_DLC_YNGYA_STICKBUNDLE);
        DataManager.Instance.UnlockedStructures.Add(StructureBrain.TYPES.DECORATION_DLC_YNGYA_FLOWERBUCKET);
        DataManager.Instance.UnlockedStructures.Add(StructureBrain.TYPES.DECORATION_DLC_YNGYA_CANDLE);
        break;
      case UpgradeSystem.Type.DLC_Building_Decorations2:
        DataManager.Instance.UnlockedStructures.Add(StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_CHIMNEY_1);
        DataManager.Instance.UnlockedStructures.Add(StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_CHIMNEY_2);
        DataManager.Instance.UnlockedStructures.Add(StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_CLOCK);
        DataManager.Instance.UnlockedStructures.Add(StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_LAMPPOST);
        DataManager.Instance.UnlockedStructures.Add(StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_PLANT);
        DataManager.Instance.UnlockedStructures.Add(StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_WALL);
        break;
      case UpgradeSystem.Type.Blacksmith_Legendary_Sword:
        DataManager.Instance.AddWeapon(EquipmentType.Sword_Legendary);
        DataManager.Instance.ForcedStartingWeapon = EquipmentType.Sword_Legendary;
        UpgradeSystem.CheckAchievementsOnUnlock(UpgradeSystem.Type.Blacksmith_Legendary_Sword);
        break;
      case UpgradeSystem.Type.Blacksmith_Legendary_Axe:
        DataManager.Instance.AddWeapon(EquipmentType.Axe_Legendary);
        DataManager.Instance.ForcedStartingWeapon = EquipmentType.Axe_Legendary;
        UpgradeSystem.CheckAchievementsOnUnlock(UpgradeSystem.Type.Blacksmith_Legendary_Axe);
        break;
      case UpgradeSystem.Type.Blacksmith_Legendary_Dagger:
        DataManager.Instance.AddWeapon(EquipmentType.Dagger_Legendary);
        DataManager.Instance.ForcedStartingWeapon = EquipmentType.Dagger_Legendary;
        UpgradeSystem.CheckAchievementsOnUnlock(UpgradeSystem.Type.Blacksmith_Legendary_Dagger);
        break;
      case UpgradeSystem.Type.Blacksmith_Legendary_Hammer:
        DataManager.Instance.AddWeapon(EquipmentType.Hammer_Legendary);
        DataManager.Instance.ForcedStartingWeapon = EquipmentType.Hammer_Legendary;
        UpgradeSystem.CheckAchievementsOnUnlock(UpgradeSystem.Type.Blacksmith_Legendary_Hammer);
        break;
      case UpgradeSystem.Type.Blacksmith_Legendary_Gauntlets:
        DataManager.Instance.AddWeapon(EquipmentType.Gauntlet_Legendary);
        DataManager.Instance.ForcedStartingWeapon = EquipmentType.Gauntlet_Legendary;
        UpgradeSystem.CheckAchievementsOnUnlock(UpgradeSystem.Type.Blacksmith_Legendary_Gauntlets);
        break;
      case UpgradeSystem.Type.Blacksmith_Legendary_Blunderbuss:
        DataManager.Instance.AddWeapon(EquipmentType.Blunderbuss_Legendary);
        DataManager.Instance.ForcedStartingWeapon = EquipmentType.Blunderbuss_Legendary;
        UpgradeSystem.CheckAchievementsOnUnlock(UpgradeSystem.Type.Blacksmith_Legendary_Blunderbuss);
        break;
      case UpgradeSystem.Type.Blacksmith_Legendary_Chain:
        DataManager.Instance.AddWeapon(EquipmentType.Chain_Legendary);
        DataManager.Instance.ForcedStartingWeapon = EquipmentType.Chain_Legendary;
        UpgradeSystem.CheckAchievementsOnUnlock(UpgradeSystem.Type.Blacksmith_Legendary_Chain);
        break;
    }
    UpgradeSystem.UnlockEvent onUpgradeUnlocked = UpgradeSystem.OnUpgradeUnlocked;
    if (onUpgradeUnlocked != null)
      onUpgradeUnlocked(Type);
  }

  public static void UnlockUpgradeWeapon(TarotCards.Card Weapon)
  {
    if (false)
      UpgradeSystem.UpgradeWeapon(Weapon);
    else
      UpgradeSystem.UnlockWeapon(Weapon);
  }

  public static void UpgradeWeapon(TarotCards.Card Weapon)
  {
  }

  public static void UnlockWeapon(TarotCards.Card Weapon)
  {
  }

  public static void UnlockUpgradeCurse(TarotCards.Card Curse)
  {
  }

  public static void UpgradeCurse(TarotCards.Card Curse)
  {
  }

  public static void UnlockCurse(TarotCards.Card Curse)
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
      case UpgradeSystem.Type.Ritual_BecomeDisciple:
        return FollowerThoughts.GetData(Thought.Cult_BecomeDisciple).Modifier;
      case UpgradeSystem.Type.Ritual_Purge:
        return FollowerThoughts.GetData(Thought.Cult_PurgeRitual).Modifier;
      case UpgradeSystem.Type.Ritual_Nudism:
        return FollowerThoughts.GetData(Thought.Cult_NudistRitual).Modifier;
      case UpgradeSystem.Type.Ritual_Cannibal:
        return FollowerThoughts.GetData(Thought.Cult_CannibalRitual).Modifier;
      case UpgradeSystem.Type.Ritual_AtoneSin:
        return FollowerThoughts.GetData(Thought.Cult_DrinkingFestival).Modifier;
      case UpgradeSystem.Type.Ritual_Midwinter:
        return FollowerThoughts.GetData(Thought.Cult_MidwinterRitual).Modifier;
      case UpgradeSystem.Type.Ritual_Warmth:
        return FollowerThoughts.GetData(Thought.Cult_WarmthRitual).Modifier;
      case UpgradeSystem.Type.Ritual_FollowerWedding:
        return FollowerThoughts.GetData(Thought.Cult_FollowerWeddingRitual).Modifier;
      case UpgradeSystem.Type.Ritual_Divorce:
        return FollowerThoughts.GetData(Thought.Cult_DivorceRitual).Modifier;
      case UpgradeSystem.Type.Ritual_RanchMeat:
        return FollowerThoughts.GetData(Thought.Cult_RanchMeat).Modifier;
      case UpgradeSystem.Type.Ritual_RanchHarvest:
        return FollowerThoughts.GetData(Thought.Cult_RanchHarvest).Modifier;
      case UpgradeSystem.Type.Ritual_ConvertToRot:
        return FollowerThoughts.GetData(Thought.Cult_ConvertToRot).Modifier;
      case UpgradeSystem.Type.Winter_RemoveRot:
        return FollowerThoughts.GetData(Thought.Cult_RemoveRot).Modifier;
      default:
        return 0.0f;
    }
  }

  public static float GetRitualWarmthChange(UpgradeSystem.Type type)
  {
    return type == UpgradeSystem.Type.Ritual_FirePit ? (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ritual_FirePit_2) ? 100f : 0.0f) : 0.0f;
  }

  public static int GetRitualSinChange(UpgradeSystem.Type type)
  {
    switch (type)
    {
      case UpgradeSystem.Type.Ritual_Purge:
        return FollowerBrain.GetPleasureAmount(FollowerBrain.PleasureActions.Purge);
      case UpgradeSystem.Type.Ritual_Nudism:
        return FollowerBrain.GetPleasureAmount(FollowerBrain.PleasureActions.NudismWinner);
      case UpgradeSystem.Type.Ritual_Cannibal:
        return FollowerBrain.GetPleasureAmount(FollowerBrain.PleasureActions.Cannibal);
      case UpgradeSystem.Type.Ritual_AtoneSin:
        int ritualSinChange = 0;
        foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
        {
          if (!FollowerManager.FollowerLocked(allBrain.Info.ID))
            ritualSinChange += Mathf.Max(allBrain.Info.Pleasure, 1);
        }
        return ritualSinChange;
      default:
        return 0;
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
      case UpgradeSystem.Type.Ritual_CrystalDoctrine:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.CRYSTAL_DOCTRINE_STONE, 1)
        };
      case UpgradeSystem.Type.Ritual_BecomeDisciple:
        return UpgradeSystem.ApplyRitualDiscount(new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 150)
        });
      case UpgradeSystem.Type.Ritual_Purge:
        return UpgradeSystem.ApplyRitualDiscount(new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 125)
        });
      case UpgradeSystem.Type.Ritual_Nudism:
        return UpgradeSystem.ApplyRitualDiscount(new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 100),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FLOWER_RED, 15)
        });
      case UpgradeSystem.Type.Ritual_Cannibal:
        return UpgradeSystem.ApplyRitualDiscount(new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 125),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FOLLOWERS, 1)
        });
      case UpgradeSystem.Type.Ritual_AtoneSin:
        return UpgradeSystem.ApplyRitualDiscount(new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 100)
        });
      case UpgradeSystem.Type.Ritual_Snowman:
        return UpgradeSystem.ApplyRitualDiscount(new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.MAGMA_STONE, 35),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 75)
        });
      case UpgradeSystem.Type.Ritual_Midwinter:
        return UpgradeSystem.ApplyRitualDiscount(new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FLOWER_PURPLE, 20),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FLOWER_WHITE, 20)
        });
      case UpgradeSystem.Type.Ritual_Warmth:
        return UpgradeSystem.ApplyRitualDiscount(new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LIGHTNING_SHARD, 20),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 75)
        });
      case UpgradeSystem.Type.Ritual_FollowerWedding:
        return UpgradeSystem.ApplyRitualDiscount(new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FOLLOWERS, 2),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 100)
        });
      case UpgradeSystem.Type.Ritual_Divorce:
        return UpgradeSystem.ApplyRitualDiscount(new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FOLLOWERS, 1),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 75)
        });
      case UpgradeSystem.Type.Ritual_RanchMeat:
        return UpgradeSystem.ApplyRitualDiscount(new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 125),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.WOOL, 5)
        });
      case UpgradeSystem.Type.Ritual_RanchHarvest:
        return UpgradeSystem.ApplyRitualDiscount(new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 125),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.WOOL, 5)
        });
      case UpgradeSystem.Type.Ritual_ConvertToRot:
        return UpgradeSystem.ApplyRitualDiscount(new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.MAGMA_STONE, 20),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 50)
        });
      case UpgradeSystem.Type.Ability_WinterChoice:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.MONSTER_HEART, 1)
        };
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
    if ((double) UpgradeSystem.GetCoolDownNormalised(UpgradeSystem.Type.Ritual_HeartsOfTheFaithful1) > 0.0)
      return false;
    if ((double) UpgradeSystem.GetCoolDownNormalised(UpgradeSystem.Type.Ritual_CrystalDoctrine) <= 0.0 && DoctrineUpgradeSystem.GetAllRemainingDoctrines().Count > 0 && Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.CRYSTAL_DOCTRINE_STONE) > 0)
      return true;
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.DOCTRINE_STONE) < 1 || !DoctrineUpgradeSystem.TrySermonsStillAvailable())
      return false;
    if (!DataManager.Instance.PostGameFleecesOnboarded && DataManager.Instance.DeathCatBeaten || !DataManager.Instance.CowboyFleeceOnboarded && DataManager.Instance.WeaponPool.Contains(EquipmentType.Blunderbuss) || DataManager.Instance.GoatFleeceOnboarded)
      return true;
    int num = DataManager.Instance.DeathCatBeaten ? 1 : 0;
    return true;
  }

  public static List<StructuresData.ItemCost> ApplyRitualDiscount(
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
    if (DataManager.Instance.NextRitualFree)
    {
      for (int index = result.Count - 1; index >= 0; --index)
      {
        if (result[index].CostItem != InventoryItem.ITEM_TYPE.FOLLOWERS)
          result.RemoveAt(index);
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

  public static void ClearCooldown(UpgradeSystem.Type type)
  {
    for (int index = DataManager.Instance.UpgradeCoolDowns.Count - 1; index >= 0; --index)
    {
      if (DataManager.Instance.UpgradeCoolDowns[index].Type == type && !UpgradeSystem.IsRitualActive(DataManager.Instance.UpgradeCoolDowns[index].Type))
      {
        DataManager.Instance.UpgradeCoolDowns.RemoveAt(index);
        break;
      }
    }
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
        return DataManager.Instance.PLAYER_HEARTS_LEVEL >= 12;
      case UpgradeSystem.Type.Ritual_UnlockWeapon:
        return true;
      case UpgradeSystem.Type.Ritual_UnlockCurse:
        return true;
      case UpgradeSystem.Type.Ritual_CrystalDoctrine:
        return DoctrineUpgradeSystem.TotalRemainingDoctrinesAvailable() <= 0;
      default:
        return false;
    }
  }

  public static void CheckAchievementsOnUnlock(UpgradeSystem.Type upgrade)
  {
    if (upgrade == UpgradeSystem.Type.PUpgrade_WeaponGodly && !DungeonSandboxManager.Active)
      AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("ALL_WEAPONS_UNLOCKED"));
    else if (upgrade == UpgradeSystem.Type.PUpgrade_CursePack3 && !DungeonSandboxManager.Active)
      AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("ALL_CURSES_UNLOCKED"));
    if (!UpgradeSystem.LegendaryWeapons.Contains<UpgradeSystem.Type>(upgrade) || !UpgradeSystem.UnlockedAllLegendaryWeapons())
      return;
    AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup(AchievementsWrapper.Tags.ALL_LEGENDARY_WEAPONS));
  }

  public static bool UnlockedAllLegendaryWeapons()
  {
    return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Blacksmith_Legendary_Hammer) && UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Blacksmith_Legendary_Sword) && UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Blacksmith_Legendary_Dagger) && UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Blacksmith_Legendary_Gauntlets) && UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Blacksmith_Legendary_Blunderbuss) && UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Blacksmith_Legendary_Axe) && UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Blacksmith_Legendary_Chain);
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
    SIN,
    WINTER,
    RANCHING,
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
    Relics_Blessed_1,
    Relic_Pack1,
    Relics_Dammed_1,
    Relic_Pack2,
    Building_Morgue_1,
    Building_Morgue_2,
    Building_Morgue_3,
    PUpgrade_HeavyAttacks,
    PUpgrade_HA_Sword,
    PUpgrade_HA_Axe,
    PUpgrade_HA_Dagger,
    PUpgrade_HA_Hammer,
    PUpgrade_HA_Gauntlets,
    Building_Crypt_1,
    Building_Crypt_2,
    Building_Crypt_3,
    Building_Shared_House,
    Relic_Pack_Default,
    PUpgrade_ResummonWeapon,
    Ritual_CrystalDoctrine,
    Building_LightningRod,
    Building_Tailor,
    Building_UpgradedIndoctrination,
    PUpgrade_HA_Blunderbuss,
    PUpgrade_HA_Shield,
    Building_PoopBucket,
    Building_SeedBucket,
    Building_JanitorStation_2,
    Ritual_BecomeDisciple,
    Building_Shrine_Disciple_Boost,
    Building_Pub,
    Building_Shrine_Pleasure,
    PleasureSystem,
    Building_Drum,
    Ritual_Purge,
    Ritual_Nudism,
    Building_MatingTent,
    DiscipleSystem,
    Building_Shrine_Disciple_Collection,
    Building_Hatchery,
    TailorSystem,
    Building_Pub_2,
    Building_Hatchery_2,
    Ritual_Cannibal,
    Ritual_AtoneSin,
    PUpgrade_Shield_CounterAttack,
    PUpgrade_Shield_Health,
    PUpgrade_Blunder_Ammo,
    System_PlayerTent,
    Building_LeaderTent,
    Building_Daycare,
    Building_Knucklebones,
    Relic_Pack_Coop,
    Relic_Pack_Corrupted,
    Building_Weather_Vane,
    Building_Volcanic_Spa,
    Building_Wooly_Shack,
    WinterSystem,
    Building_Ranch,
    Building_Medic,
    RanchingSystem,
    Building_RanchTrough,
    Building_Ranch_2,
    Building_RanchHutch,
    Building_RacingGate,
    Ritual_Snowman,
    Ritual_Midwinter,
    Ritual_Warmth,
    Building_Logistics,
    Major_DLC_Sermon_Packs,
    PUpgrade_HA_Chain,
    Relics_Fire,
    Relics_Ice,
    Curses_Fire,
    Curses_Teleport,
    Curses_Barrier,
    Building_Wolf_Trap,
    Ritual_FollowerWedding,
    Building_Furnace,
    Building_Furnace_2,
    Building_Furnace_3,
    Ritual_Divorce,
    Building_LightningRod_2,
    Building_Toolshed,
    Building_Farm_Crop_Grower,
    Building_Sacrifice_Table,
    Building_Trait_Manipulator_1,
    Building_Trait_Manipulator_2,
    Building_Trait_Manipulator_3,
    Upgrade_Furnace_Full,
    Upgrade_Rotstone_Spread,
    DLC_Building_Decorations1,
    DLC_Building_Decorations2,
    Blacksmith_Legendary_Sword,
    Blacksmith_Legendary_Axe,
    Blacksmith_Legendary_Dagger,
    Blacksmith_Legendary_Hammer,
    Blacksmith_Legendary_Gauntlets,
    Blacksmith_Legendary_Blunderbuss,
    Blacksmith_Legendary_Shield,
    Blacksmith_Legendary_Chain,
    Ritual_RanchMeat,
    Ritual_RanchHarvest,
    Ritual_ConvertToRot,
    Winter_RemoveRot,
    Ritual_FirePit_2,
    Special_EmbraceRot,
    Special_RejectRot,
    PUpgrade_StartingWeapon_7,
    PUpgrade_StartingWeapon_8,
    Building_RanchChoppingBlock,
    Building_RotstoneMine,
    Building_RotstoneMine_2,
    Ability_WinterChoice,
    Count,
  }

  public delegate void UnlockEvent(UpgradeSystem.Type upgradeType);

  [MessagePackObject(false)]
  [Serializable]
  public class UpgradeCoolDown
  {
    [Key(0)]
    public UpgradeSystem.Type Type;
    [Key(1)]
    public float TotalElapsedGameTime;
    [Key(2)]
    public float Duration;
  }
}
