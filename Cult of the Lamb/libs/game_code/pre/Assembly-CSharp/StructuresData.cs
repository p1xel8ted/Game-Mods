// Decompiled with JetBrains decompiler
// Type: StructuresData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI.BuildMenu;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public sealed class StructuresData
{
  public StructureBrain.TYPES Type;
  public int VariantIndex;
  public string PrefabPath;
  public bool RemoveOnDie = true;
  public float ProgressTarget = 10f;
  public bool WorkIsRequiredForProgress = true;
  public bool IsUpgrade;
  public bool IsUpgradeDestroyPrevious = true;
  public bool IgnoreGrid;
  public bool IsBuildingProject;
  public bool IsCollapsed;
  public StructureBrain.TYPES UpgradeFromType;
  public StructureBrain.TYPES RequiresType;
  public int TILE_WIDTH = 1;
  public int TILE_HEIGHT = 1;
  public bool CanBeMoved = true;
  public bool CanBeRecycled = true;
  public bool IsObstruction;
  public bool DoesNotOccupyGrid;
  public bool isDeletable = true;
  public Vector2Int LootCountToDropRange;
  public Vector2Int CropLootCountToDropRange;
  public List<InventoryItem.ITEM_TYPE> MultipleLootToDrop;
  public List<int> MultipleLootToDropChance;
  public InventoryItem.ITEM_TYPE LootToDrop;
  public int LootCountToDrop;
  public int ID;
  public FollowerLocation Location = FollowerLocation.None;
  public bool DontLoadMe;
  public bool Destroyed;
  public int GridX;
  public int GridY;
  public Vector2Int Bounds = new Vector2Int(1, 1);
  public List<InventoryItem> Inventory = new List<InventoryItem>();
  public float Progress;
  public float PowerRequirement;
  public Vector3 Position;
  public Vector3 Offset;
  public float OffsetMax;
  public bool Repaired;
  public Vector2Int GridTilePosition = StructuresData.NullPosition;
  public Vector3Int PlacementRegionPosition;
  public static readonly Vector2Int NullPosition = new Vector2Int(-2147483647 /*0x80000001*/, -2147483647 /*0x80000001*/);
  public int Age;
  public bool Exhausted;
  public int UpgradeLevel;
  public List<StructuresData.PathData> pathData = new List<StructuresData.PathData>();
  public int Direction = 1;
  public Villager_Info v_i;
  public int SoulCount;
  public int Level;
  public StructureBrain.TYPES ToBuildType;
  public StructuresData.Phase CurrentPhase;
  public bool Purchased;
  private List<PlacementRegion.TileGridTile> grid = new List<PlacementRegion.TileGridTile>();
  public int FollowerID = -1;
  public List<int> MultipleFollowerIDs = new List<int>();
  public bool Claimed;
  public int BedpanCount;
  public bool HasFood;
  public float FollowerImprisonedTimestamp;
  public float FollowerImprisonedFaith;
  public bool GivenGift;
  public int Dir = 1;
  public bool BodyWrapped;
  public bool Prioritised;
  public bool PrioritisedAsBuildingObstruction;
  public bool WeedsAndRubblePlaced;
  public bool Rotten;
  public bool Burned;
  public bool Eaten;
  public int GatheringEndPhase = -1;
  public int DayPreviouslyUsed = -1;
  public bool IsSapling;
  public float GrowthStage;
  public bool CanRegrow = true;
  public bool BenefitedFromFertilizer;
  public int RemainingHarvests;
  public string Animation = "";
  public float StartingScale;
  public bool Picked;
  public bool Watered;
  public int WateredCount;
  public bool HasBird;
  public int TotalPoops;
  public InventoryItem.ITEM_TYPE SignPostItem;
  public bool GivenHealth;
  public int WeedType = -1;
  public float LastPrayTime = -1f;
  public int Fuel;
  public int MaxFuel = 50;
  public bool FullyFueled;
  public int FuelDepletionDayTimestamp = -1;
  public bool onlyDepleteWhenFullyFueled;
  public List<InventoryItem.ITEM_TYPE> QueuedResources = new List<InventoryItem.ITEM_TYPE>();
  public List<Interaction_Kitchen.QueuedMeal> QueuedMeals = new List<Interaction_Kitchen.QueuedMeal>();
  public Interaction_Kitchen.QueuedMeal CurrentCookingMeal;
  public float WeaponUpgradePointProgress;
  public float WeaponUpgradePointDuration;
  public WeaponUpgradeSystem.WeaponType CurrentUnlockingWeaponType;
  public WeaponUpgradeSystem.WeaponUpgradeType CurrentUnlockingUpgradeType;
  public static System.Action OnResearchBegin;
  public static readonly List<StructureBrain.TYPES> AllStructures = new List<StructureBrain.TYPES>()
  {
    StructureBrain.TYPES.BED,
    StructureBrain.TYPES.COOKING_FIRE,
    StructureBrain.TYPES.HARVEST_TOTEM,
    StructureBrain.TYPES.SCARECROW,
    StructureBrain.TYPES.COLLECTED_RESOURCES_CHEST,
    StructureBrain.TYPES.DANCING_FIREPIT,
    StructureBrain.TYPES.BODY_PIT,
    StructureBrain.TYPES.KITCHEN,
    StructureBrain.TYPES.KITCHEN_II,
    StructureBrain.TYPES.BLACKSMITH,
    StructureBrain.TYPES.OUTHOUSE,
    StructureBrain.TYPES.COMPOST_BIN,
    StructureBrain.TYPES.BED_2,
    StructureBrain.TYPES.GRAVE,
    StructureBrain.TYPES.FARM_STATION,
    StructureBrain.TYPES.FARM_STATION_II,
    StructureBrain.TYPES.FARM_PLOT,
    StructureBrain.TYPES.FOOD_STORAGE,
    StructureBrain.TYPES.FOOD_STORAGE_2,
    StructureBrain.TYPES.JANITOR_STATION,
    StructureBrain.TYPES.LUMBERJACK_STATION,
    StructureBrain.TYPES.LUMBERJACK_STATION_2,
    StructureBrain.TYPES.BLOODSTONE_MINE,
    StructureBrain.TYPES.BLOODSTONE_MINE_2,
    StructureBrain.TYPES.PROPAGANDA_SPEAKER,
    StructureBrain.TYPES.MISSIONARY,
    StructureBrain.TYPES.MISSIONARY_II,
    StructureBrain.TYPES.MISSIONARY_III,
    StructureBrain.TYPES.DEMON_SUMMONER,
    StructureBrain.TYPES.DEMON_SUMMONER_2,
    StructureBrain.TYPES.DEMON_SUMMONER_3,
    StructureBrain.TYPES.FISHING_HUT,
    StructureBrain.TYPES.PRISON,
    StructureBrain.TYPES.HEALING_BAY,
    StructureBrain.TYPES.HEALING_BAY_2,
    StructureBrain.TYPES.TAROT_BUILDING,
    StructureBrain.TYPES.BED_3,
    StructureBrain.TYPES.SILO_SEED,
    StructureBrain.TYPES.SILO_FERTILISER,
    StructureBrain.TYPES.SURVEILLANCE,
    StructureBrain.TYPES.FISHING_HUT_2,
    StructureBrain.TYPES.OUTHOUSE_2,
    StructureBrain.TYPES.SCARECROW_2,
    StructureBrain.TYPES.HARVEST_TOTEM_2,
    StructureBrain.TYPES.CHOPPING_SHRINE,
    StructureBrain.TYPES.MINING_SHRINE,
    StructureBrain.TYPES.FORAGING_SHRINE,
    StructureBrain.TYPES.CONFESSION_BOOTH,
    StructureBrain.TYPES.TEMPLE,
    StructureBrain.TYPES.TEMPLE_II,
    StructureBrain.TYPES.TEMPLE_III,
    StructureBrain.TYPES.TEMPLE_IV,
    StructureBrain.TYPES.SHRINE_PASSIVE,
    StructureBrain.TYPES.OFFERING_STATUE,
    StructureBrain.TYPES.SHRINE,
    StructureBrain.TYPES.SHRINE_II,
    StructureBrain.TYPES.SHRINE_III,
    StructureBrain.TYPES.SHRINE_IV,
    StructureBrain.TYPES.PLANK_PATH,
    StructureBrain.TYPES.TILE_PATH,
    StructureBrain.TYPES.TILE_HAY,
    StructureBrain.TYPES.TILE_BLOOD,
    StructureBrain.TYPES.TILE_ROCKS,
    StructureBrain.TYPES.TILE_WATER,
    StructureBrain.TYPES.TILE_BRICKS,
    StructureBrain.TYPES.TILE_PLANKS,
    StructureBrain.TYPES.TILE_FLOWERS,
    StructureBrain.TYPES.TILE_REDGRASS,
    StructureBrain.TYPES.TILE_SPOOKYPLANKS,
    StructureBrain.TYPES.TILE_GOLD,
    StructureBrain.TYPES.TILE_MOSAIC,
    StructureBrain.TYPES.TILE_FLOWERSROCKY,
    StructureBrain.TYPES.DECORATION_STONE,
    StructureBrain.TYPES.DECORATION_TREE,
    StructureBrain.TYPES.DECORATION_TORCH,
    StructureBrain.TYPES.DECORATION_FLOWER_BOX_1,
    StructureBrain.TYPES.DECORATION_SMALL_STONE_CANDLE,
    StructureBrain.TYPES.DECORATION_FLAG_CROWN,
    StructureBrain.TYPES.DECORATION_FLAG_SCRIPTURE,
    StructureBrain.TYPES.DECORATION_WALL_TWIGS,
    StructureBrain.TYPES.DECORATION_LAMB_FLAG_STATUE,
    StructureBrain.TYPES.FARM_PLOT_SIGN,
    StructureBrain.TYPES.DECORATION_BARROW,
    StructureBrain.TYPES.DECORATION_BELL_STATUE,
    StructureBrain.TYPES.DECORATION_BONE_ARCH,
    StructureBrain.TYPES.DECORATION_BONE_BARREL,
    StructureBrain.TYPES.DECORATION_BONE_CANDLE,
    StructureBrain.TYPES.DECORATION_BONE_FLAG,
    StructureBrain.TYPES.DECORATION_BONE_LANTERN,
    StructureBrain.TYPES.DECORATION_BONE_PILLAR,
    StructureBrain.TYPES.DECORATION_BONE_SCULPTURE,
    StructureBrain.TYPES.DECORATION_CANDLE_BARREL,
    StructureBrain.TYPES.DECORATION_CRYSTAL_LAMP,
    StructureBrain.TYPES.DECORATION_CRYSTAL_LIGHT,
    StructureBrain.TYPES.DECORATION_CRYSTAL_ROCK,
    StructureBrain.TYPES.DECORATION_CRYSTAL_STATUE,
    StructureBrain.TYPES.DECORATION_CRYSTAL_TREE,
    StructureBrain.TYPES.DECORATION_CRYSTAL_WINDOW,
    StructureBrain.TYPES.DECORATION_FLOWER_ARCH,
    StructureBrain.TYPES.DECORATION_FOUNTAIN,
    StructureBrain.TYPES.DECORATION_POST_BOX,
    StructureBrain.TYPES.DECORATION_PUMPKIN_PILE,
    StructureBrain.TYPES.DECORATION_PUMPKIN_STOOL,
    StructureBrain.TYPES.DECORATION_STONE_CANDLE,
    StructureBrain.TYPES.DECORATION_STONE_FLAG,
    StructureBrain.TYPES.DECORATION_STONE_MUSHROOM,
    StructureBrain.TYPES.DECORATION_TORCH_BIG,
    StructureBrain.TYPES.DECORATION_TWIG_LAMP,
    StructureBrain.TYPES.DECORATION_WALL_BONE,
    StructureBrain.TYPES.DECORATION_WALL_GRASS,
    StructureBrain.TYPES.DECORATION_WALL_STONE,
    StructureBrain.TYPES.DECORATION_WREATH_STICK,
    StructureBrain.TYPES.DECORATION_STUMP_LAMB_STATUE,
    StructureBrain.TYPES.DECORATION_BELL_SMALL,
    StructureBrain.TYPES.DECORATION_BONE_SKULL_BIG,
    StructureBrain.TYPES.DECORATION_BONE_SKULL_PILE,
    StructureBrain.TYPES.DECORATION_FLAG_CRYSTAL,
    StructureBrain.TYPES.DECORATION_FLAG_MUSHROOM,
    StructureBrain.TYPES.DECORATION_FLOWER_BOTTLE,
    StructureBrain.TYPES.DECORATION_FLOWER_CART,
    StructureBrain.TYPES.DECORATION_HAY_BALE,
    StructureBrain.TYPES.DECORATION_HAY_PILE,
    StructureBrain.TYPES.DECORATION_LEAFY_FLOWER_SCULPTURE,
    StructureBrain.TYPES.DECORATION_LEAFY_LANTERN,
    StructureBrain.TYPES.DECORATION_LEAFY_SCULPTURE,
    StructureBrain.TYPES.DECORATION_MUSHROOM_1,
    StructureBrain.TYPES.DECORATION_MUSHROOM_2,
    StructureBrain.TYPES.DECORATION_MUSHROOM_CANDLE_1,
    StructureBrain.TYPES.DECORATION_MUSHROOM_CANDLE_2,
    StructureBrain.TYPES.DECORATION_MUSHROOM_CANDLE_LARGE,
    StructureBrain.TYPES.DECORATION_MUSHROOM_SCULPTURE,
    StructureBrain.TYPES.DECORATION_SPIDER_LANTERN,
    StructureBrain.TYPES.DECORATION_SPIDER_PILLAR,
    StructureBrain.TYPES.DECORATION_SPIDER_SCULPTURE,
    StructureBrain.TYPES.DECORATION_SPIDER_TORCH,
    StructureBrain.TYPES.DECORATION_SPIDER_WEB_CROWN_SCULPTURE,
    StructureBrain.TYPES.DECORATION_STONE_CANDLE_LAMP,
    StructureBrain.TYPES.DECORATION_STONE_HENGE,
    StructureBrain.TYPES.DECORATION_WALL_SPIDER,
    StructureBrain.TYPES.DECORATION_POND,
    StructureBrain.TYPES.DECORATION_MONSTERSHRINE,
    StructureBrain.TYPES.DECORATION_FLOWERPOTWALL,
    StructureBrain.TYPES.DECORATION_LEAFYLAMPPOST,
    StructureBrain.TYPES.DECORATION_FLOWERVASE,
    StructureBrain.TYPES.DECORATION_WATERINGCAN,
    StructureBrain.TYPES.DECORATION_FLOWER_CART_SMALL,
    StructureBrain.TYPES.DECORATION_WEEPINGSHRINE,
    StructureBrain.TYPES.DECORATION_PLUSH,
    StructureBrain.TYPES.DECORATION_TWITCH_FLAG_CROWN,
    StructureBrain.TYPES.DECORATION_TWITCH_MUSHROOM_BAG,
    StructureBrain.TYPES.DECORATION_TWITCH_ROSE_BUSH,
    StructureBrain.TYPES.DECORATION_TWITCH_STONE_FLAG,
    StructureBrain.TYPES.DECORATION_TWITCH_STONE_STATUE,
    StructureBrain.TYPES.DECORATION_TWITCH_WOODEN_GUARDIAN,
    StructureBrain.TYPES.SHRINE_BLUEHEART,
    StructureBrain.TYPES.SHRINE_BLACKHEART,
    StructureBrain.TYPES.SHRINE_REDHEART,
    StructureBrain.TYPES.SHRINE_TAROT,
    StructureBrain.TYPES.SHRINE_DAMAGE,
    StructureBrain.TYPES.FARM_PLOT_SOZO,
    StructureBrain.TYPES.DECORATION_BOSS_TROPHY_1,
    StructureBrain.TYPES.DECORATION_BOSS_TROPHY_2,
    StructureBrain.TYPES.DECORATION_BOSS_TROPHY_3,
    StructureBrain.TYPES.DECORATION_BOSS_TROPHY_4,
    StructureBrain.TYPES.DECORATION_BOSS_TROPHY_5,
    StructureBrain.TYPES.DECORATION_HALLOWEEN_CANDLE,
    StructureBrain.TYPES.DECORATION_HALLOWEEN_PUMPKIN,
    StructureBrain.TYPES.DECORATION_HALLOWEEN_TREE,
    StructureBrain.TYPES.DECORATION_HALLOWEEN_SKULL,
    StructureBrain.TYPES.DECORATION_VIDEO
  };
  private static readonly List<StructureBrain.TYPES> HiddenStructuresUntilUnlocked = new List<StructureBrain.TYPES>()
  {
    StructureBrain.TYPES.GRAVE,
    StructureBrain.TYPES.COMPOST_BIN_DEAD_BODY,
    StructureBrain.TYPES.DECORATION_MONSTERSHRINE,
    StructureBrain.TYPES.DECORATION_PLUSH,
    StructureBrain.TYPES.DECORATION_MUSHROOM_1,
    StructureBrain.TYPES.DECORATION_MUSHROOM_SCULPTURE,
    StructureBrain.TYPES.TILE_FLOWERS,
    StructureBrain.TYPES.DECORATION_FLOWERPOTWALL,
    StructureBrain.TYPES.DECORATION_LEAFYLAMPPOST,
    StructureBrain.TYPES.DECORATION_FLOWERVASE,
    StructureBrain.TYPES.DECORATION_WATERINGCAN,
    StructureBrain.TYPES.DECORATION_FLOWER_CART_SMALL,
    StructureBrain.TYPES.DECORATION_WEEPINGSHRINE,
    StructureBrain.TYPES.DECORATION_BOSS_TROPHY_5,
    StructureBrain.TYPES.DECORATION_VIDEO,
    StructureBrain.TYPES.DECORATION_TWITCH_FLAG_CROWN,
    StructureBrain.TYPES.DECORATION_TWITCH_MUSHROOM_BAG,
    StructureBrain.TYPES.DECORATION_TWITCH_ROSE_BUSH,
    StructureBrain.TYPES.DECORATION_TWITCH_STONE_FLAG,
    StructureBrain.TYPES.DECORATION_TWITCH_STONE_STATUE,
    StructureBrain.TYPES.DECORATION_TWITCH_WOODEN_GUARDIAN,
    StructureBrain.TYPES.DECORATION_HALLOWEEN_CANDLE,
    StructureBrain.TYPES.DECORATION_HALLOWEEN_PUMPKIN,
    StructureBrain.TYPES.DECORATION_HALLOWEEN_TREE,
    StructureBrain.TYPES.DECORATION_HALLOWEEN_SKULL
  };
  private const float GoldModifier = 2f;

  public bool IsDeletable
  {
    get
    {
      return (this.Type != StructureBrain.TYPES.BODY_PIT && this.Type != StructureBrain.TYPES.GRAVE || this.FollowerID == -1) && this.isDeletable;
    }
  }

  public void SetPathData(Vector2Int tilePos, Vector3 worldPosition, int pathID)
  {
    for (int index = this.pathData.Count - 1; index >= 0; --index)
    {
      if (this.pathData[index].TilePosition == tilePos)
        this.pathData.RemoveAt(index);
    }
    this.pathData.Add(new StructuresData.PathData()
    {
      TilePosition = tilePos,
      WorldPosition = worldPosition,
      PathID = pathID
    });
  }

  [JsonIgnore]
  public List<PlacementRegion.TileGridTile> Grid => this.grid;

  public bool IsGatheringActive
  {
    get
    {
      return this.GatheringEndPhase != -1 && TimeManager.CurrentPhase != (DayPhase) this.GatheringEndPhase;
    }
  }

  public bool IsFull => this.Inventory.Count >= Structures_Outhouse.Capacity(this.Type);

  public bool WeaponUpgradingInProgress
  {
    get
    {
      return (double) this.WeaponUpgradePointDuration > 0.0 && (double) this.WeaponUpgradePointProgress < (double) this.WeaponUpgradePointDuration;
    }
  }

  public bool WeaponUpgradingCompleted
  {
    get
    {
      return (double) this.WeaponUpgradePointProgress > 0.0 && (double) this.WeaponUpgradePointProgress >= (double) this.WeaponUpgradePointDuration;
    }
  }

  public void CreateStructure(FollowerLocation location, Vector3 position, Vector2Int bounds)
  {
    this.ID = ++DataManager.Instance.StructureID;
    this.Location = location;
    this.GridX = (int) position.x;
    this.GridY = (int) position.y;
    this.Position = position;
    this.Bounds = bounds;
    this.Offset = new Vector3(UnityEngine.Random.Range(-this.OffsetMax, this.OffsetMax), UnityEngine.Random.Range(-this.OffsetMax, this.OffsetMax));
  }

  public static string GetLocalizedNameStatic(StructureBrain.TYPES Type)
  {
    return LocalizationManager.GetTranslation($"Structures/{Type}");
  }

  public static string LocalizedName(StructureBrain.TYPES Type)
  {
    return LocalizationManager.GetTranslation($"Structures/{Type}");
  }

  public static string LocalizedDescription(StructureBrain.TYPES Type)
  {
    switch (Type)
    {
      case StructureBrain.TYPES.LUMBERJACK_STATION_2:
      case StructureBrain.TYPES.BLOODSTONE_MINE_2:
        string str1 = (Type == StructureBrain.TYPES.LUMBERJACK_STATION_2 ? "<sprite name=\"icon_wood\">" : "<sprite name=\"icon_stone\">") + " <sprite name=\"icon_FaithDoubleUp\">";
        return $"{LocalizationManager.GetTranslation($"Structures/{Type}/Description")}<br><br>{str1}";
      case StructureBrain.TYPES.MISSIONARY:
      case StructureBrain.TYPES.MISSIONARY_II:
      case StructureBrain.TYPES.MISSIONARY_III:
        int num = 1;
        if (Type == StructureBrain.TYPES.MISSIONARY_II)
          num = 2;
        else if (Type == StructureBrain.TYPES.MISSIONARY_III)
          num = 3;
        string str2 = " <br><br><sprite name=\"icon_wood\"> <sprite name=\"icon_stone\"> <sprite name=\"icon_blackgold\"> <sprite name=\"icon_meat\">";
        if (Type == StructureBrain.TYPES.MISSIONARY_II || Type == StructureBrain.TYPES.MISSIONARY_III)
          str2 += " <sprite name=\"icon_bones\"> <sprite name=\"icon_Followers\"> <sprite name=\"icon_seed\">";
        if (Type == StructureBrain.TYPES.MISSIONARY_III)
          str2 += " <sprite name=\"icon_LogRefined\"> <sprite name=\"icon_StoneRefined\">";
        string str3 = $"{num.ToString()}x {ScriptLocalization.Inventory.FOLLOWERS} <sprite name=\"icon_Followers\">";
        return $"{LocalizationManager.GetTranslation($"Structures/{Type}/Description")} <br><color=#FFD201>{str3}</color>{str2}";
      case StructureBrain.TYPES.REFINERY_2:
        string str4 = "<sprite name=\"icon_GoldRefined\"><sprite name=\"icon_LogRefined\"><sprite name=\"icon_StoneRefined\"> <sprite name=\"icon_FaithDoubleUp\">";
        return $"{LocalizationManager.GetTranslation($"Structures/{Type}/Description")}<br><br>{str4}";
      case StructureBrain.TYPES.DECORATION_MUSHROOM_SCULPTURE:
        if (DataManager.Instance.SozoDecorationQuestActive)
        {
          string str5 = $"{ScriptLocalization.Objectives_GroupTitles.VisitSozo.Colour(Color.yellow)}: {string.Format(ScriptLocalization.Objectives.BuildStructure, (object) StructuresData.LocalizedName(Type))}";
          return $"{LocalizationManager.GetTranslation($"Structures/{Type}/Description")}<br><br>{str5}";
        }
        break;
    }
    Debug.Log((object) ("Type: " + (object) Type));
    return LocalizationManager.GetTranslation($"Structures/{Type}/Description");
  }

  public static string LocalizedPros(StructureBrain.TYPES Type)
  {
    return LocalizationManager.GetTranslation($"Structures/{Type}/Pros");
  }

  public static string LocalizedCons(StructureBrain.TYPES Type)
  {
    return LocalizationManager.GetTranslation($"Structures/{Type}/Cons");
  }

  public string GetLocalizedName() => LocalizationManager.GetTranslation($"Structures/{this.Type}");

  public string GetLocalizedDescription()
  {
    return LocalizationManager.GetTranslation($"Structures/{this.Type}/Description");
  }

  public string GetLocalizedLore()
  {
    return LocalizationManager.GetTranslation($"Structures/{this.Type.ToString()}/Lore");
  }

  public string GetLocalizedName(bool plural, bool withArticle, bool definite)
  {
    string str = $"Structures/{this.Type.ToString()}{(plural ? "/Plural" : "")}{(withArticle ? (definite ? "/Definite" : "/Indefinite") : "")}";
    Debug.Log((object) str);
    return LocalizationManager.GetTranslation(str);
  }

  public static StructuresData GetInfoByType(StructureBrain.TYPES Type, int variantIndex)
  {
    StructuresData infoByType = (StructuresData) null;
    switch (Type)
    {
      case StructureBrain.TYPES.BUILDER:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Builder"
        };
        break;
      case StructureBrain.TYPES.BED:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building_House"
        };
        break;
      case StructureBrain.TYPES.PORTAL:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/PORTAL",
          TILE_WIDTH = 5,
          TILE_HEIGHT = 5
        };
        break;
      case StructureBrain.TYPES.SACRIFICIAL_TEMPLE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/SacrificalTemple"
        };
        break;
      case StructureBrain.TYPES.WOOD_STORE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/WoodStore"
        };
        break;
      case StructureBrain.TYPES.BLACKSMITH:
        infoByType = new StructuresData()
        {
          TILE_WIDTH = 4,
          TILE_HEIGHT = 4,
          PrefabPath = "Prefabs/Structures/Buildings/Building Blacksmith"
        };
        break;
      case StructureBrain.TYPES.TAVERN:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Tavern"
        };
        break;
      case StructureBrain.TYPES.FARM_STATION:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Farm House"
        };
        break;
      case StructureBrain.TYPES.WHEAT_SILO:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Wheat Silo"
        };
        break;
      case StructureBrain.TYPES.KITCHEN:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Kitchen"
        };
        break;
      case StructureBrain.TYPES.COOKED_FOOD_SILO:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Cooked Food Silo"
        };
        break;
      case StructureBrain.TYPES.CROP:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Crops/Crop",
          OffsetMax = 0.1f
        };
        break;
      case StructureBrain.TYPES.NIGHTMARE_MACHINE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Nightmare Machine"
        };
        break;
      case StructureBrain.TYPES.DEFENCE_TOWER:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Defence Tower"
        };
        break;
      case StructureBrain.TYPES.TREE:
        infoByType = new StructuresData[4]
        {
          new StructuresData()
          {
            PrefabPath = "Prefabs/Structures/Other/Tree1",
            DontLoadMe = true,
            ProgressTarget = 7f,
            LootToDrop = InventoryItem.ITEM_TYPE.LOG,
            LootCountToDrop = 5
          },
          new StructuresData()
          {
            PrefabPath = "Prefabs/Structures/Other/Tree2",
            DontLoadMe = true,
            ProgressTarget = 5f,
            LootToDrop = InventoryItem.ITEM_TYPE.LOG,
            LootCountToDrop = 3
          },
          new StructuresData()
          {
            PrefabPath = "Prefabs/Structures/Other/Tree3",
            DontLoadMe = true,
            ProgressTarget = 100f,
            LootToDrop = InventoryItem.ITEM_TYPE.LOG,
            LootCountToDrop = 15
          },
          new StructuresData()
          {
            PrefabPath = "Prefabs/Structures/Other/Tree4",
            DontLoadMe = true,
            ProgressTarget = 3f,
            LootToDrop = InventoryItem.ITEM_TYPE.LOG,
            LootCountToDrop = 1
          }
        }[variantIndex];
        break;
      case StructureBrain.TYPES.FOLLOWER_RECRUIT:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Recruit/Recruit",
          v_i = Villager_Info.NewCharacter()
        };
        break;
      case StructureBrain.TYPES.HEALING_BATH:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Healing Bath"
        };
        break;
      case StructureBrain.TYPES.FIRE_PIT:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Fire Pit"
        };
        break;
      case StructureBrain.TYPES.BUILD_PLOT:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Build Plot",
          TILE_WIDTH = 3,
          TILE_HEIGHT = 3
        };
        break;
      case StructureBrain.TYPES.SHRINE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Base Shrine",
          CanBeMoved = false,
          CanBeRecycled = false,
          IgnoreGrid = true,
          isDeletable = false,
          UpgradeFromType = StructureBrain.TYPES.SHRINE_BASE
        };
        break;
      case StructureBrain.TYPES.BARRACKS:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Barracks"
        };
        break;
      case StructureBrain.TYPES.ASTROLOGIST:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Astrologist"
        };
        break;
      case StructureBrain.TYPES.STORAGE_PIT:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/StoragePit"
        };
        break;
      case StructureBrain.TYPES.BUILD_SITE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building_BuildSite"
        };
        break;
      case StructureBrain.TYPES.ALTAR:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Altar"
        };
        break;
      case StructureBrain.TYPES.PLACEMENT_REGION:
        infoByType = new StructuresData()
        {
          DontLoadMe = true,
          DoesNotOccupyGrid = true
        };
        break;
      case StructureBrain.TYPES.DECORATION_TREE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Tree 1"
        };
        break;
      case StructureBrain.TYPES.DECORATION_STONE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Stone 1"
        };
        break;
      case StructureBrain.TYPES.REPAIRABLE_HEARTS:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Repairable Heart Statue",
          Repaired = true
        };
        break;
      case StructureBrain.TYPES.REPAIRABLE_ASTROLOGY:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Repairable Astrology Statue",
          Repaired = true
        };
        break;
      case StructureBrain.TYPES.REPAIRABLE_VOODOO:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Repairable Voodoo Statue",
          Repaired = true
        };
        break;
      case StructureBrain.TYPES.REPAIRABLE_CURSES:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Repairable Curse Statue",
          Repaired = true
        };
        break;
      case StructureBrain.TYPES.BED_2:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building_House2"
        };
        break;
      case StructureBrain.TYPES.BED_3:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building_House3"
        };
        break;
      case StructureBrain.TYPES.SHRINE_FUNDAMENTALIST:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building_Fundamentalist Shrine"
        };
        break;
      case StructureBrain.TYPES.SHRINE_MISFIT:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building_Misfit Shrine"
        };
        break;
      case StructureBrain.TYPES.SHRINE_UTOPIANIST:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building_Utopianist Shrine"
        };
        break;
      case StructureBrain.TYPES.FARM_PLOT:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Farm Plot"
        };
        break;
      case StructureBrain.TYPES.GRAVE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Grave"
        };
        break;
      case StructureBrain.TYPES.DEAD_WORSHIPPER:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Dead Worshipper",
          IgnoreGrid = true,
          CanBeMoved = false,
          isDeletable = false
        };
        break;
      case StructureBrain.TYPES.VOMIT:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Vomit",
          IgnoreGrid = true,
          CanBeMoved = false,
          isDeletable = false
        };
        break;
      case StructureBrain.TYPES.DEMOLISH_STRUCTURE:
        infoByType = new StructuresData();
        break;
      case StructureBrain.TYPES.MOVE_STRUCTURE:
        infoByType = new StructuresData();
        break;
      case StructureBrain.TYPES.FARM_PLOT_SOZO:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Farm Plot - Sozo",
          RequiresType = StructureBrain.TYPES.FARM_STATION
        };
        break;
      case StructureBrain.TYPES.MEAL:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/Meal",
          IgnoreGrid = true
        };
        break;
      case StructureBrain.TYPES.BODY_PIT:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building BodyPit"
        };
        break;
      case StructureBrain.TYPES.TAROT_BUILDING:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Tarot Building"
        };
        break;
      case StructureBrain.TYPES.CULT_UPGRADE1:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Cult Upgrade 1 Building"
        };
        break;
      case StructureBrain.TYPES.DANCING_FIREPIT:
      case StructureBrain.TYPES.TEMPLE_BASE:
      case StructureBrain.TYPES.TEMPLE_BASE_EXTENSION1:
      case StructureBrain.TYPES.TEMPLE_BASE_EXTENSION2:
      case StructureBrain.TYPES.SHRINE_BASE:
      case StructureBrain.TYPES.FEAST_TABLE:
      case StructureBrain.TYPES.FISHING_SPOT:
        infoByType = new StructuresData()
        {
          DontLoadMe = true,
          IgnoreGrid = true
        };
        break;
      case StructureBrain.TYPES.CULT_UPGRADE2:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Cult Upgrade 2 Building"
        };
        break;
      case StructureBrain.TYPES.PLANK_PATH:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Path Plank"
        };
        break;
      case StructureBrain.TYPES.PRISON:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building_Prison"
        };
        break;
      case StructureBrain.TYPES.GRAVE2:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building BodyPit"
        };
        break;
      case StructureBrain.TYPES.RUBBLE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/Rubble",
          LootToDrop = InventoryItem.ITEM_TYPE.STONE
        };
        break;
      case StructureBrain.TYPES.WEEDS:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Weeds",
          IsObstruction = true,
          CanBeMoved = false,
          isDeletable = false
        };
        break;
      case StructureBrain.TYPES.LUMBERJACK_STATION:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Lumberjack",
          LootToDrop = InventoryItem.ITEM_TYPE.LOG
        };
        break;
      case StructureBrain.TYPES.LUMBERJACK_STATION_2:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Lumberjack Lvl2",
          LootToDrop = InventoryItem.ITEM_TYPE.LOG
        };
        break;
      case StructureBrain.TYPES.RESEARCH_1:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Research 1"
        };
        break;
      case StructureBrain.TYPES.RESEARCH_2:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Research 1"
        };
        break;
      case StructureBrain.TYPES.ONE:
        infoByType = new StructuresData();
        break;
      case StructureBrain.TYPES.TWO:
        infoByType = new StructuresData();
        break;
      case StructureBrain.TYPES.THREE:
        infoByType = new StructuresData();
        break;
      case StructureBrain.TYPES.SACRIFICIAL_TEMPLE_2:
        infoByType = new StructuresData();
        break;
      case StructureBrain.TYPES.COOKING_FIRE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Kitchen"
        };
        break;
      case StructureBrain.TYPES.ALCHEMY_CAULDRON:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Alchemy Cauldron"
        };
        break;
      case StructureBrain.TYPES.FOOD_STORAGE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Food Storage"
        };
        break;
      case StructureBrain.TYPES.FOOD_STORAGE_2:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Food Storage Lvl2"
        };
        break;
      case StructureBrain.TYPES.MATING_TENT:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Mating Tent"
        };
        break;
      case StructureBrain.TYPES.BLOODSTONE_MINE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building BloodstoneMine",
          LootToDrop = InventoryItem.ITEM_TYPE.STONE
        };
        break;
      case StructureBrain.TYPES.BLOODSTONE_MINE_2:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building BloodstoneMine Lvl2",
          LootToDrop = InventoryItem.ITEM_TYPE.STONE
        };
        break;
      case StructureBrain.TYPES.CONFESSION_BOOTH:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Confession Booth"
        };
        break;
      case StructureBrain.TYPES.DRUM_CIRCLE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Drum Circle"
        };
        break;
      case StructureBrain.TYPES.ENEMY_TRAP:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building EnemyTrap"
        };
        break;
      case StructureBrain.TYPES.FISHING_HUT:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Fishing Hut"
        };
        break;
      case StructureBrain.TYPES.GHOST_CIRCLE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Ghost Circle"
        };
        break;
      case StructureBrain.TYPES.HIPPY_TENT:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Hippy Tent"
        };
        break;
      case StructureBrain.TYPES.HUNTERS_HUT:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Hunters Hut"
        };
        break;
      case StructureBrain.TYPES.KNUCKLEBONES_ARENA:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Knucklebones"
        };
        break;
      case StructureBrain.TYPES.MEDITATION_MAT:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Meditation Mat"
        };
        break;
      case StructureBrain.TYPES.SCARIFICATIONIST:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Scarificationist"
        };
        break;
      case StructureBrain.TYPES.SECURITY_TURRET:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Security Turret"
        };
        break;
      case StructureBrain.TYPES.SECURITY_TURRET_2:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Security Turret Lvl 2"
        };
        break;
      case StructureBrain.TYPES.WITCH_DOCTOR:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Witch Doctor"
        };
        break;
      case StructureBrain.TYPES.MAYPOLE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Maypole"
        };
        break;
      case StructureBrain.TYPES.FLOWER_GARDEN:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Flower Garden"
        };
        break;
      case StructureBrain.TYPES.BERRY_BUSH:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/BerryBush",
          DontLoadMe = true,
          ProgressTarget = 100f,
          MultipleLootToDrop = new List<InventoryItem.ITEM_TYPE>()
          {
            InventoryItem.ITEM_TYPE.BERRY,
            InventoryItem.ITEM_TYPE.SEED
          },
          MultipleLootToDropChance = new List<int>()
          {
            85,
            15
          },
          LootCountToDropRange = new Vector2Int(3, 4),
          CropLootCountToDropRange = new Vector2Int(3, 4)
        };
        break;
      case StructureBrain.TYPES.SLEEPING_BAG:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building_SleepingBag"
        };
        break;
      case StructureBrain.TYPES.BLOOD_STONE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Bloodstone",
          LootToDrop = InventoryItem.ITEM_TYPE.BLOOD_STONE,
          DontLoadMe = true
        };
        break;
      case StructureBrain.TYPES.OUTHOUSE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Outhouse"
        };
        break;
      case StructureBrain.TYPES.POOP:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Poop",
          IgnoreGrid = true,
          CanBeMoved = false,
          isDeletable = false
        };
        break;
      case StructureBrain.TYPES.OUTPOST_SHRINE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Outpost Shrine"
        };
        break;
      case StructureBrain.TYPES.LUMBER_MINE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Lumber Mine",
          Inventory = new List<InventoryItem>()
          {
            new InventoryItem(InventoryItem.ITEM_TYPE.LOG)
            {
              quantity = 200
            }
          }
        };
        break;
      case StructureBrain.TYPES.COMPOST_BIN:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Compost Bin"
        };
        break;
      case StructureBrain.TYPES.BLOODMOON_OFFERING:
        infoByType = new StructuresData()
        {
          DontLoadMe = true,
          CanBeMoved = false,
          CanBeRecycled = false
        };
        break;
      case StructureBrain.TYPES.DECORATION_TORCH:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/BaseTorch"
        };
        break;
      case StructureBrain.TYPES.PUMPKIN_BUSH:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/PumpkinPatch",
          DontLoadMe = true,
          ProgressTarget = 100f,
          MultipleLootToDrop = new List<InventoryItem.ITEM_TYPE>()
          {
            InventoryItem.ITEM_TYPE.PUMPKIN,
            InventoryItem.ITEM_TYPE.SEED_PUMPKIN
          },
          MultipleLootToDropChance = new List<int>()
          {
            85,
            15
          },
          LootCountToDropRange = new Vector2Int(3, 4),
          CropLootCountToDropRange = new Vector2Int(3, 4)
        };
        break;
      case StructureBrain.TYPES.SACRIFICIAL_STONE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Sacrificial Stone",
          TILE_WIDTH = 5,
          TILE_HEIGHT = 5
        };
        break;
      case StructureBrain.TYPES.DECORATION_FLOWER_BOX_1:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Flower Box 1"
        };
        break;
      case StructureBrain.TYPES.DECORATION_SMALL_STONE_CANDLE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Small Stone Candle"
        };
        break;
      case StructureBrain.TYPES.DECORATION_FLAG_CROWN:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Flag Crown"
        };
        break;
      case StructureBrain.TYPES.DECORATION_FLAG_SCRIPTURE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Flag Scripture"
        };
        break;
      case StructureBrain.TYPES.DECORATION_WALL_TWIGS:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Wall Twigs"
        };
        break;
      case StructureBrain.TYPES.DECORATION_LAMB_FLAG_STATUE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Lamb Flag Statue"
        };
        break;
      case StructureBrain.TYPES.TEMPLE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Temple",
          isDeletable = false
        };
        break;
      case StructureBrain.TYPES.TEMPLE_EXTENSION1:
        infoByType = new StructuresData()
        {
          IsUpgradeDestroyPrevious = false,
          PrefabPath = "Prefabs/Structures/Buildings/Building Temple Extension 1",
          IgnoreGrid = true
        };
        break;
      case StructureBrain.TYPES.TEMPLE_EXTENSION2:
        infoByType = new StructuresData()
        {
          IsUpgradeDestroyPrevious = false,
          PrefabPath = "Prefabs/Structures/Buildings/Building Temple Extension 2",
          IgnoreGrid = true
        };
        break;
      case StructureBrain.TYPES.BUILDSITE_BUILDINGPROJECT:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building_BuildSiteBuildingProject",
          IgnoreGrid = true
        };
        break;
      case StructureBrain.TYPES.SHRINE_BLUEHEART:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/BlueHeartShrine"
        };
        break;
      case StructureBrain.TYPES.SHRINE_REDHEART:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/RedHeartShrine"
        };
        break;
      case StructureBrain.TYPES.SHRINE_BLACKHEART:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/BlackHeartShrine"
        };
        break;
      case StructureBrain.TYPES.SHRINE_TAROT:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Base Shrine II"
        };
        break;
      case StructureBrain.TYPES.SHRINE_DAMAGE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DamageShrine"
        };
        break;
      case StructureBrain.TYPES.SHRINE_II:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Base Shrine II",
          CanBeMoved = false,
          CanBeRecycled = false,
          IgnoreGrid = true,
          isDeletable = false
        };
        break;
      case StructureBrain.TYPES.COLLECTED_RESOURCES_CHEST:
        infoByType = new StructuresData()
        {
          DontLoadMe = true,
          PrefabPath = "Prefabs/Structures/Buildings/Collected Resource Chest"
        };
        break;
      case StructureBrain.TYPES.MEAL_MEAT:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/Meal Good",
          IgnoreGrid = true
        };
        break;
      case StructureBrain.TYPES.MEAL_GREAT:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/Meal Great",
          IgnoreGrid = true
        };
        break;
      case StructureBrain.TYPES.MEAL_GRASS:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/Meal Grass",
          IgnoreGrid = true
        };
        break;
      case StructureBrain.TYPES.MEAL_GOOD_FISH:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/Meal Good Fish",
          IgnoreGrid = true
        };
        break;
      case StructureBrain.TYPES.MEAL_FOLLOWER_MEAT:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/Meal Follower Meat",
          IgnoreGrid = true
        };
        break;
      case StructureBrain.TYPES.HEALING_BAY:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Healing Bay"
        };
        break;
      case StructureBrain.TYPES.APOTHECARY:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Apothecary"
        };
        break;
      case StructureBrain.TYPES.SCARECROW:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Farm Scarecrow"
        };
        break;
      case StructureBrain.TYPES.HARVEST_TOTEM:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Farm Harvest Totem"
        };
        break;
      case StructureBrain.TYPES.FARM_PLOT_SIGN:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Farm Plot Signpost"
        };
        break;
      case StructureBrain.TYPES.MUSHROOM_BUSH:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/MushroomPatch",
          DontLoadMe = true,
          ProgressTarget = 100f,
          MultipleLootToDrop = new List<InventoryItem.ITEM_TYPE>()
          {
            InventoryItem.ITEM_TYPE.MUSHROOM_SMALL,
            InventoryItem.ITEM_TYPE.SEED_MUSHROOM
          },
          MultipleLootToDropChance = new List<int>()
          {
            85,
            15
          },
          LootCountToDropRange = new Vector2Int(3, 4),
          CropLootCountToDropRange = new Vector2Int(3, 4)
        };
        break;
      case StructureBrain.TYPES.RED_FLOWER_BUSH:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/RedFlowerBush",
          DontLoadMe = true,
          ProgressTarget = 100f,
          MultipleLootToDrop = new List<InventoryItem.ITEM_TYPE>()
          {
            InventoryItem.ITEM_TYPE.FLOWER_RED,
            InventoryItem.ITEM_TYPE.SEED_FLOWER_RED
          },
          MultipleLootToDropChance = new List<int>()
          {
            85,
            15
          },
          LootCountToDropRange = new Vector2Int(3, 4),
          CropLootCountToDropRange = new Vector2Int(3, 4)
        };
        break;
      case StructureBrain.TYPES.WHITE_FLOWER_BUSH:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/WhiteFlowerBush",
          DontLoadMe = true,
          ProgressTarget = 100f,
          MultipleLootToDrop = new List<InventoryItem.ITEM_TYPE>()
          {
            InventoryItem.ITEM_TYPE.FLOWER_WHITE,
            InventoryItem.ITEM_TYPE.SEED_FLOWER_WHITE
          },
          MultipleLootToDropChance = new List<int>()
          {
            85,
            15
          },
          LootCountToDropRange = new Vector2Int(3, 4),
          CropLootCountToDropRange = new Vector2Int(3, 4)
        };
        break;
      case StructureBrain.TYPES.MEAL_MUSHROOMS:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/Meal Mushrooms",
          IgnoreGrid = true
        };
        break;
      case StructureBrain.TYPES.TEMPLE_II:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Temple 2",
          isDeletable = false
        };
        break;
      case StructureBrain.TYPES.MEAL_POOP:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/Meal Poop",
          IgnoreGrid = true
        };
        break;
      case StructureBrain.TYPES.MEAL_ROTTEN:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/Meal Rotten",
          IgnoreGrid = true
        };
        break;
      case StructureBrain.TYPES.MISSION_SHRINE:
        infoByType = new StructuresData()
        {
          DontLoadMe = true,
          PrefabPath = "Prefabs/Structures/Buildings/Mission Shrine"
        };
        break;
      case StructureBrain.TYPES.REFINERY:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Refinery"
        };
        break;
      case StructureBrain.TYPES.SHRINE_PASSIVE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Shrine Passive"
        };
        break;
      case StructureBrain.TYPES.RESOURCE:
        infoByType = new StructuresData[3]
        {
          new StructuresData()
          {
            PrefabPath = "Prefabs/Resources/Log Refined"
          },
          new StructuresData()
          {
            PrefabPath = "Prefabs/Resources/Stone Refined"
          },
          new StructuresData()
          {
            PrefabPath = "Prefabs/Resources/Rope"
          }
        }[variantIndex];
        break;
      case StructureBrain.TYPES.SHRINE_III:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Base Shrine III",
          CanBeMoved = false,
          CanBeRecycled = false,
          IgnoreGrid = true,
          isDeletable = false
        };
        break;
      case StructureBrain.TYPES.SHRINE_IV:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Base Shrine IV",
          CanBeMoved = false,
          CanBeRecycled = false,
          IgnoreGrid = true,
          isDeletable = false
        };
        break;
      case StructureBrain.TYPES.OFFERING_STATUE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Offering Statue"
        };
        break;
      case StructureBrain.TYPES.SHRINE_PASSIVE_II:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Shrine Passive II"
        };
        break;
      case StructureBrain.TYPES.SHRINE_PASSIVE_III:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Shrine Passive III"
        };
        break;
      case StructureBrain.TYPES.TEMPLE_III:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Temple 3",
          isDeletable = false
        };
        break;
      case StructureBrain.TYPES.TEMPLE_IV:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Temple 4",
          isDeletable = false
        };
        break;
      case StructureBrain.TYPES.TILE_PATH:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Path Tile"
        };
        break;
      case StructureBrain.TYPES.RUBBLE_BIG:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/Rubble Big",
          TILE_WIDTH = 5,
          TILE_HEIGHT = 5,
          LootToDrop = InventoryItem.ITEM_TYPE.STONE
        };
        break;
      case StructureBrain.TYPES.WATER_SMALL:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Water_3x3",
          TILE_WIDTH = 3,
          TILE_HEIGHT = 3
        };
        break;
      case StructureBrain.TYPES.WATER_MEDIUM:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Water_3x4",
          TILE_WIDTH = 4,
          TILE_HEIGHT = 3
        };
        break;
      case StructureBrain.TYPES.WATER_BIG:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Water_4x4",
          TILE_WIDTH = 4,
          TILE_HEIGHT = 4
        };
        break;
      case StructureBrain.TYPES.RATAU_SHRINE:
        infoByType = new StructuresData()
        {
          DontLoadMe = true
        };
        break;
      case StructureBrain.TYPES.GOLD_ORE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Gold Ore",
          LootToDrop = InventoryItem.ITEM_TYPE.GOLD_NUGGET,
          DontLoadMe = true
        };
        break;
      case StructureBrain.TYPES.PROPAGANDA_SPEAKER:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Propaganda Speakers"
        };
        break;
      case StructureBrain.TYPES.HEALING_BAY_2:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Healing Bay 2"
        };
        break;
      case StructureBrain.TYPES.MISSIONARY:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Missionary"
        };
        break;
      case StructureBrain.TYPES.SILO_SEED:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Silo Seed"
        };
        break;
      case StructureBrain.TYPES.SILO_FERTILISER:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Silo Fertiliser"
        };
        break;
      case StructureBrain.TYPES.SURVEILLANCE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Surveillance Tower"
        };
        break;
      case StructureBrain.TYPES.FISHING_HUT_2:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Fishing Hut 2"
        };
        break;
      case StructureBrain.TYPES.OUTHOUSE_2:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Outhouse 2"
        };
        break;
      case StructureBrain.TYPES.SCARECROW_2:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Farm Scarecrow 2"
        };
        break;
      case StructureBrain.TYPES.HARVEST_TOTEM_2:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Farm Harvest Totem 2"
        };
        break;
      case StructureBrain.TYPES.REFINERY_2:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Refinery 2"
        };
        break;
      case StructureBrain.TYPES.TREE_HITTABLE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/Tree Hittable"
        };
        break;
      case StructureBrain.TYPES.STONE_HITTABLE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/Stone Hittable"
        };
        break;
      case StructureBrain.TYPES.BONES_HITTABLE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/Bones Hittable"
        };
        break;
      case StructureBrain.TYPES.POOP_HITTABLE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/Poop Hittable"
        };
        break;
      case StructureBrain.TYPES.DECORATION_BARROW:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Barrow"
        };
        break;
      case StructureBrain.TYPES.DECORATION_BELL_STATUE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Bell Statue"
        };
        break;
      case StructureBrain.TYPES.DECORATION_BONE_ARCH:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Bone Arch"
        };
        break;
      case StructureBrain.TYPES.DECORATION_BONE_BARREL:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Bone Barrel"
        };
        break;
      case StructureBrain.TYPES.DECORATION_BONE_CANDLE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Bone Candle"
        };
        break;
      case StructureBrain.TYPES.DECORATION_BONE_FLAG:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Bone Flag Crown"
        };
        break;
      case StructureBrain.TYPES.DECORATION_BONE_LANTERN:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Bone Lantern"
        };
        break;
      case StructureBrain.TYPES.DECORATION_BONE_PILLAR:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Bone Pillar"
        };
        break;
      case StructureBrain.TYPES.DECORATION_BONE_SCULPTURE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Bone Sculpture"
        };
        break;
      case StructureBrain.TYPES.DECORATION_CANDLE_BARREL:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Candle Barrel"
        };
        break;
      case StructureBrain.TYPES.DECORATION_CRYSTAL_LAMP:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Crystal Lamp"
        };
        break;
      case StructureBrain.TYPES.DECORATION_CRYSTAL_LIGHT:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Crystal Light"
        };
        break;
      case StructureBrain.TYPES.DECORATION_CRYSTAL_ROCK:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Crystal Rock"
        };
        break;
      case StructureBrain.TYPES.DECORATION_CRYSTAL_STATUE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Crystal Statue"
        };
        break;
      case StructureBrain.TYPES.DECORATION_CRYSTAL_TREE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Crystal Tree"
        };
        break;
      case StructureBrain.TYPES.DECORATION_CRYSTAL_WINDOW:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Crystal Window"
        };
        break;
      case StructureBrain.TYPES.DECORATION_FLOWER_ARCH:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Flower Arch"
        };
        break;
      case StructureBrain.TYPES.DECORATION_FOUNTAIN:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Fountain"
        };
        break;
      case StructureBrain.TYPES.DECORATION_POST_BOX:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Post Box"
        };
        break;
      case StructureBrain.TYPES.DECORATION_PUMPKIN_PILE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Pumpkin Pile"
        };
        break;
      case StructureBrain.TYPES.DECORATION_PUMPKIN_STOOL:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Pumpkin Stool"
        };
        break;
      case StructureBrain.TYPES.DECORATION_STONE_CANDLE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Stone Candle"
        };
        break;
      case StructureBrain.TYPES.DECORATION_STONE_FLAG:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Stone Flag"
        };
        break;
      case StructureBrain.TYPES.DECORATION_STONE_MUSHROOM:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Stone Mushroom"
        };
        break;
      case StructureBrain.TYPES.DECORATION_TORCH_BIG:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Torch Big"
        };
        break;
      case StructureBrain.TYPES.DECORATION_TWIG_LAMP:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Twig Lamp"
        };
        break;
      case StructureBrain.TYPES.DECORATION_WALL_BONE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Wall Bone"
        };
        break;
      case StructureBrain.TYPES.DECORATION_WALL_STONE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Wall Stone"
        };
        break;
      case StructureBrain.TYPES.DECORATION_WALL_GRASS:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Wall Grass"
        };
        break;
      case StructureBrain.TYPES.DECORATION_WREATH_STICK:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Wreath Stick"
        };
        break;
      case StructureBrain.TYPES.DECORATION_STUMP_LAMB_STATUE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Stump Lamb Statue"
        };
        break;
      case StructureBrain.TYPES.CHOPPING_SHRINE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Chopping Shrine"
        };
        break;
      case StructureBrain.TYPES.MINING_SHRINE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Mining Shrine"
        };
        break;
      case StructureBrain.TYPES.FORAGING_SHRINE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Foraging Shrine"
        };
        break;
      case StructureBrain.TYPES.EDIT_BUILDINGS:
        infoByType = new StructuresData();
        break;
      case StructureBrain.TYPES.JANITOR_STATION:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Janitor Station"
        };
        break;
      case StructureBrain.TYPES.DECORATION_BELL_SMALL:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Bell Statue Small"
        };
        break;
      case StructureBrain.TYPES.DECORATION_BONE_SKULL_BIG:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Bone Skull Big"
        };
        break;
      case StructureBrain.TYPES.DECORATION_BONE_SKULL_PILE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Bone Skull Pile"
        };
        break;
      case StructureBrain.TYPES.DECORATION_FLAG_CRYSTAL:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Flag Crystal"
        };
        break;
      case StructureBrain.TYPES.DECORATION_FLAG_MUSHROOM:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Flag Mushroom"
        };
        break;
      case StructureBrain.TYPES.DECORATION_FLOWER_BOTTLE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Flower Bottle"
        };
        break;
      case StructureBrain.TYPES.DECORATION_FLOWER_CART:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Flower Cart"
        };
        break;
      case StructureBrain.TYPES.DECORATION_HAY_BALE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Hay Bale"
        };
        break;
      case StructureBrain.TYPES.DECORATION_HAY_PILE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Hay Pile"
        };
        break;
      case StructureBrain.TYPES.DECORATION_LEAFY_FLOWER_SCULPTURE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Leafy Flower Sculpture"
        };
        break;
      case StructureBrain.TYPES.DECORATION_LEAFY_LANTERN:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Leafy Lantern"
        };
        break;
      case StructureBrain.TYPES.DECORATION_LEAFY_SCULPTURE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Leafy Sculpture"
        };
        break;
      case StructureBrain.TYPES.DECORATION_MUSHROOM_1:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Mushroom 1"
        };
        break;
      case StructureBrain.TYPES.DECORATION_MUSHROOM_2:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Mushroom 2"
        };
        break;
      case StructureBrain.TYPES.DECORATION_MUSHROOM_CANDLE_1:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Mushroom Candle 1"
        };
        break;
      case StructureBrain.TYPES.DECORATION_MUSHROOM_CANDLE_2:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Mushroom Candle 2"
        };
        break;
      case StructureBrain.TYPES.DECORATION_MUSHROOM_CANDLE_LARGE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Mushroom Candle Large"
        };
        break;
      case StructureBrain.TYPES.DECORATION_MUSHROOM_SCULPTURE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Mushroom Sculpture"
        };
        break;
      case StructureBrain.TYPES.DECORATION_SPIDER_LANTERN:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Spider Lantern"
        };
        break;
      case StructureBrain.TYPES.DECORATION_SPIDER_PILLAR:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Spider Pillar"
        };
        break;
      case StructureBrain.TYPES.DECORATION_SPIDER_SCULPTURE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Spider Sculpture"
        };
        break;
      case StructureBrain.TYPES.DECORATION_SPIDER_TORCH:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Spider Torch"
        };
        break;
      case StructureBrain.TYPES.DECORATION_SPIDER_WEB_CROWN_SCULPTURE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Spider Web Crown Sculpture"
        };
        break;
      case StructureBrain.TYPES.DECORATION_STONE_CANDLE_LAMP:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Stone Candle Lamp"
        };
        break;
      case StructureBrain.TYPES.DECORATION_STONE_HENGE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Stone Henge"
        };
        break;
      case StructureBrain.TYPES.DECORATION_WALL_SPIDER:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Wall Spider"
        };
        break;
      case StructureBrain.TYPES.DECORATION_POND:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Pond"
        };
        break;
      case StructureBrain.TYPES.DEMON_SUMMONER:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Demon Summoner"
        };
        break;
      case StructureBrain.TYPES.COMPOST_BIN_DEAD_BODY:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Compost Bin Dead Body"
        };
        break;
      case StructureBrain.TYPES.MEAL_GREAT_FISH:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/Meal Great Fish",
          IgnoreGrid = true
        };
        break;
      case StructureBrain.TYPES.MEAL_BAD_FISH:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/Meal Bad Fish",
          IgnoreGrid = true
        };
        break;
      case StructureBrain.TYPES.BEETROOT_BUSH:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/BeetrootBush",
          DontLoadMe = true,
          ProgressTarget = 100f,
          MultipleLootToDrop = new List<InventoryItem.ITEM_TYPE>()
          {
            InventoryItem.ITEM_TYPE.BEETROOT,
            InventoryItem.ITEM_TYPE.SEED_BEETROOT
          },
          MultipleLootToDropChance = new List<int>()
          {
            85,
            15
          },
          LootCountToDropRange = new Vector2Int(3, 4),
          CropLootCountToDropRange = new Vector2Int(3, 4)
        };
        break;
      case StructureBrain.TYPES.CAULIFLOWER_BUSH:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/CauliflowerBush",
          DontLoadMe = true,
          ProgressTarget = 100f,
          MultipleLootToDrop = new List<InventoryItem.ITEM_TYPE>()
          {
            InventoryItem.ITEM_TYPE.CAULIFLOWER,
            InventoryItem.ITEM_TYPE.SEED_CAULIFLOWER
          },
          MultipleLootToDropChance = new List<int>()
          {
            85,
            15
          },
          LootCountToDropRange = new Vector2Int(3, 4),
          CropLootCountToDropRange = new Vector2Int(3, 4)
        };
        break;
      case StructureBrain.TYPES.BED_1_COLLAPSED:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building_House_Collapsed"
        };
        break;
      case StructureBrain.TYPES.BED_2_COLLAPSED:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building_House_2_Collapsed"
        };
        break;
      case StructureBrain.TYPES.DEMON_SUMMONER_2:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Demon Summoner 2"
        };
        break;
      case StructureBrain.TYPES.DEMON_SUMMONER_3:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Demon Summoner 3"
        };
        break;
      case StructureBrain.TYPES.MEAL_BERRIES:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/Meal Berries",
          IgnoreGrid = true
        };
        break;
      case StructureBrain.TYPES.MEAL_MEDIUM_VEG:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/Meal Medium Veg",
          IgnoreGrid = true
        };
        break;
      case StructureBrain.TYPES.MEAL_BAD_MIXED:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/Meal Bad Mixed",
          IgnoreGrid = true
        };
        break;
      case StructureBrain.TYPES.MEAL_MEDIUM_MIXED:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/Meal Medium Mixed",
          IgnoreGrid = true
        };
        break;
      case StructureBrain.TYPES.MEAL_GREAT_MIXED:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/Meal Great Mixed",
          IgnoreGrid = true
        };
        break;
      case StructureBrain.TYPES.MEAL_DEADLY:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/Meal Deadly",
          IgnoreGrid = true
        };
        break;
      case StructureBrain.TYPES.MEAL_BAD_MEAT:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/Meal Bad Meat",
          IgnoreGrid = true
        };
        break;
      case StructureBrain.TYPES.MEAL_GREAT_MEAT:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/Meal Great Meat",
          IgnoreGrid = true
        };
        break;
      case StructureBrain.TYPES.MISSIONARY_II:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Missionary 2"
        };
        break;
      case StructureBrain.TYPES.MISSIONARY_III:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Missionary 3"
        };
        break;
      case StructureBrain.TYPES.KITCHEN_II:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Kitchen 2"
        };
        break;
      case StructureBrain.TYPES.FARM_STATION_II:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Farm House 2"
        };
        break;
      case StructureBrain.TYPES.MEAL_BURNED:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/Meal Burned",
          IgnoreGrid = true
        };
        break;
      case StructureBrain.TYPES.TILE_FLOWERS:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Path Flowers"
        };
        break;
      case StructureBrain.TYPES.TILE_HAY:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Path Hay"
        };
        break;
      case StructureBrain.TYPES.TILE_PLANKS:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Path Planks"
        };
        break;
      case StructureBrain.TYPES.TILE_SPOOKYPLANKS:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Path SpookyPlanks"
        };
        break;
      case StructureBrain.TYPES.TILE_REDGRASS:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Path RedGrass"
        };
        break;
      case StructureBrain.TYPES.TILE_ROCKS:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Path Rocks"
        };
        break;
      case StructureBrain.TYPES.TILE_BRICKS:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Path Bricks"
        };
        break;
      case StructureBrain.TYPES.TILE_BLOOD:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Path Blood"
        };
        break;
      case StructureBrain.TYPES.TILE_WATER:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Path Water"
        };
        break;
      case StructureBrain.TYPES.TILE_GOLD:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Path Gold"
        };
        break;
      case StructureBrain.TYPES.TILE_MOSAIC:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Path Mosaic"
        };
        break;
      case StructureBrain.TYPES.TILE_FLOWERSROCKY:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Path FloweryRocky"
        };
        break;
      case StructureBrain.TYPES.DECORATION_MONSTERSHRINE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration MassiveMonster Shrine"
        };
        break;
      case StructureBrain.TYPES.DECORATION_FLOWERPOTWALL:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Wall Flower Pots"
        };
        break;
      case StructureBrain.TYPES.DECORATION_LEAFYLAMPPOST:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Leafy Lamp Post"
        };
        break;
      case StructureBrain.TYPES.DECORATION_FLOWERVASE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Flower Vase"
        };
        break;
      case StructureBrain.TYPES.DECORATION_WATERINGCAN:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Flower Watering Can"
        };
        break;
      case StructureBrain.TYPES.DECORATION_FLOWER_CART_SMALL:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Flower Cart Small"
        };
        break;
      case StructureBrain.TYPES.DECORATION_WEEPINGSHRINE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Flower Weeping Shrine"
        };
        break;
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_1:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Crown Shrine_0"
        };
        break;
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_2:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Crown Shrine_1"
        };
        break;
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_3:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Crown Shrine_2"
        };
        break;
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_4:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Crown Shrine_3"
        };
        break;
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_5:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Deathcat Shrine"
        };
        break;
      case StructureBrain.TYPES.DECORATION_VIDEO:
        infoByType = new StructuresData()
        {
          PrefabPath = "Assets/Prefabs/Placement Objects/Placement Object VideoDecoration"
        };
        break;
      case StructureBrain.TYPES.DECORATION_PLUSH:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Plush"
        };
        break;
      case StructureBrain.TYPES.DECORATION_TWITCH_FLAG_CROWN:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Twitch Flag Crown"
        };
        break;
      case StructureBrain.TYPES.DECORATION_TWITCH_MUSHROOM_BAG:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Twitch Mushroom Bag"
        };
        break;
      case StructureBrain.TYPES.DECORATION_TWITCH_ROSE_BUSH:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Twitch Rose Bush"
        };
        break;
      case StructureBrain.TYPES.DECORATION_TWITCH_STONE_FLAG:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Twitch Stone Flag"
        };
        break;
      case StructureBrain.TYPES.DECORATION_TWITCH_STONE_STATUE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Twitch Stone Statue"
        };
        break;
      case StructureBrain.TYPES.DECORATION_TWITCH_WOODEN_GUARDIAN:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Twitch Wooden Guardian"
        };
        break;
      case StructureBrain.TYPES.DECORATION_HALLOWEEN_PUMPKIN:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Halloween Pumpkin"
        };
        break;
      case StructureBrain.TYPES.DECORATION_HALLOWEEN_SKULL:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Halloween Skull"
        };
        break;
      case StructureBrain.TYPES.DECORATION_HALLOWEEN_CANDLE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Halloween Candle"
        };
        break;
      case StructureBrain.TYPES.DECORATION_HALLOWEEN_TREE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Halloween Tree"
        };
        break;
    }
    if (infoByType != null)
    {
      infoByType.Type = Type;
      infoByType.VariantIndex = variantIndex;
      infoByType.IsUpgrade = StructuresData.IsUpgradeStructure(infoByType.Type);
      infoByType.UpgradeFromType = StructuresData.GetUpgradePrerequisite(infoByType.Type);
    }
    return infoByType;
  }

  public static List<StructuresData.ItemCost> GetResearchCostList(
    StructureBrain.TYPES Type,
    TypeAndPlacementObjects.Tier Tier)
  {
    switch (Type)
    {
      case StructureBrain.TYPES.ONE:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 15),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_SOUL, 150)
        };
      case StructureBrain.TYPES.TWO:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 15),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_SOUL, 300),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 3)
        };
      case StructureBrain.TYPES.THREE:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 60),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_SOUL, 450),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 20)
        };
      default:
        switch (Tier)
        {
          case TypeAndPlacementObjects.Tier.One:
            return new List<StructuresData.ItemCost>()
            {
              new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_SOUL, 50)
            };
          case TypeAndPlacementObjects.Tier.Two:
            return new List<StructuresData.ItemCost>()
            {
              new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_SOUL, 150)
            };
          case TypeAndPlacementObjects.Tier.Three:
            return new List<StructuresData.ItemCost>()
            {
              new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_SOUL, 300)
            };
          default:
            return new List<StructuresData.ItemCost>();
        }
    }
  }

  public static int GetResearchCost(StructureBrain.TYPES Type)
  {
    return Type == StructureBrain.TYPES.FARM_PLOT ? 10 : 5;
  }

  public static StructuresData.ResearchState GetResearchStateByType(StructureBrain.TYPES Types)
  {
    if (StructuresData.GetUnlocked(Types))
      return StructuresData.ResearchState.Researched;
    return StructuresData.ResearchExists(Types) ? StructuresData.ResearchState.Researching : StructuresData.ResearchState.Unresearched;
  }

  public static bool HasTemple()
  {
    return DataManager.Instance.HasBuiltTemple1 && !StructureManager.IsBuilding(StructureBrain.TYPES.TEMPLE);
  }

  public static bool RequiresTempleToBuild(StructureBrain.TYPES type)
  {
    return type != StructureBrain.TYPES.SHRINE && type != StructureBrain.TYPES.COOKING_FIRE && type != StructureBrain.TYPES.TEMPLE;
  }

  public static bool GetUnlocked(StructureBrain.TYPES Types)
  {
    if (CheatConsole.AllBuildingsUnlocked)
      return true;
    switch (Types)
    {
      case StructureBrain.TYPES.BED:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Beds);
      case StructureBrain.TYPES.FARM_STATION:
      case StructureBrain.TYPES.SILO_SEED:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_FollowerFarming);
      case StructureBrain.TYPES.KITCHEN:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Kitchen);
      case StructureBrain.TYPES.SHRINE:
        return DataManager.Instance.CanBuildShrine;
      case StructureBrain.TYPES.DECORATION_TREE:
      case StructureBrain.TYPES.DECORATION_STONE:
      case StructureBrain.TYPES.DECORATION_SMALL_STONE_CANDLE:
      case StructureBrain.TYPES.DECORATION_FLAG_CROWN:
      case StructureBrain.TYPES.DECORATION_WALL_TWIGS:
      case StructureBrain.TYPES.DECORATION_CANDLE_BARREL:
      case StructureBrain.TYPES.DECORATION_WALL_GRASS:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Decorations1);
      case StructureBrain.TYPES.BED_2:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_BetterBeds);
      case StructureBrain.TYPES.BED_3:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Beds3);
      case StructureBrain.TYPES.FARM_PLOT:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Farms);
      case StructureBrain.TYPES.GRAVE:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Graves);
      case StructureBrain.TYPES.BODY_PIT:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_BodyPit);
      case StructureBrain.TYPES.DANCING_FIREPIT:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_DancingFirepit);
      case StructureBrain.TYPES.PRISON:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Prison);
      case StructureBrain.TYPES.LUMBERJACK_STATION:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Economy_Lumberyard);
      case StructureBrain.TYPES.LUMBERJACK_STATION_2:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Economy_LumberyardII);
      case StructureBrain.TYPES.COOKING_FIRE:
        return true;
      case StructureBrain.TYPES.FOOD_STORAGE:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_FoodStorage);
      case StructureBrain.TYPES.FOOD_STORAGE_2:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_FoodStorage2);
      case StructureBrain.TYPES.BLOODSTONE_MINE:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Economy_Mine);
      case StructureBrain.TYPES.BLOODSTONE_MINE_2:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Economy_MineII);
      case StructureBrain.TYPES.CONFESSION_BOOTH:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_ConfessionBooth);
      case StructureBrain.TYPES.OUTHOUSE:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Outhouse);
      case StructureBrain.TYPES.COMPOST_BIN:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Followers_Compost);
      case StructureBrain.TYPES.TEMPLE:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Temple);
      case StructureBrain.TYPES.SHRINE_II:
      case StructureBrain.TYPES.TEMPLE_II:
      case StructureBrain.TYPES.SHRINE_III:
      case StructureBrain.TYPES.SHRINE_IV:
      case StructureBrain.TYPES.TEMPLE_III:
      case StructureBrain.TYPES.TEMPLE_IV:
        return false;
      case StructureBrain.TYPES.HEALING_BAY:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_HealingBay);
      case StructureBrain.TYPES.SCARECROW:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_AdvancedFarming);
      case StructureBrain.TYPES.HARVEST_TOTEM:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_HarvestTotem);
      case StructureBrain.TYPES.REFINERY:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Economy_Refinery);
      case StructureBrain.TYPES.SHRINE_PASSIVE:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Shrine_PassiveShrines);
      case StructureBrain.TYPES.OFFERING_STATUE:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Shrine_OfferingStatue);
      case StructureBrain.TYPES.SHRINE_PASSIVE_II:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Shrine_PassiveShrinesII);
      case StructureBrain.TYPES.SHRINE_PASSIVE_III:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Shrine_PassiveShrinesIII);
      case StructureBrain.TYPES.PROPAGANDA_SPEAKER:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_PropagandaSpeakers);
      case StructureBrain.TYPES.HEALING_BAY_2:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_HealingBay2);
      case StructureBrain.TYPES.MISSIONARY:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Missionary);
      case StructureBrain.TYPES.SILO_FERTILISER:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_SiloFertiliser);
      case StructureBrain.TYPES.SURVEILLANCE:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Surveillance);
      case StructureBrain.TYPES.FISHING_HUT_2:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_FishingHut2);
      case StructureBrain.TYPES.OUTHOUSE_2:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Outhouse2);
      case StructureBrain.TYPES.SCARECROW_2:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Scarecrow2);
      case StructureBrain.TYPES.HARVEST_TOTEM_2:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_HarvestTotem2);
      case StructureBrain.TYPES.REFINERY_2:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Economy_Refinery_2);
      case StructureBrain.TYPES.DECORATION_BONE_ARCH:
      case StructureBrain.TYPES.DECORATION_BONE_BARREL:
      case StructureBrain.TYPES.DECORATION_BONE_CANDLE:
      case StructureBrain.TYPES.DECORATION_BONE_FLAG:
      case StructureBrain.TYPES.DECORATION_BONE_LANTERN:
      case StructureBrain.TYPES.DECORATION_BONE_PILLAR:
      case StructureBrain.TYPES.DECORATION_BONE_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_WALL_BONE:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Decorations2);
      case StructureBrain.TYPES.CHOPPING_SHRINE:
      case StructureBrain.TYPES.MINING_SHRINE:
      case StructureBrain.TYPES.FORAGING_SHRINE:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_ShrinesOfNature);
      case StructureBrain.TYPES.JANITOR_STATION:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_JanitorStation);
      case StructureBrain.TYPES.DEMON_SUMMONER:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_DemonSummoner);
      case StructureBrain.TYPES.COMPOST_BIN_DEAD_BODY:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_NaturalBurial);
      case StructureBrain.TYPES.DEMON_SUMMONER_2:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_DemonSummoner_2);
      case StructureBrain.TYPES.DEMON_SUMMONER_3:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_DemonSummoner_3);
      case StructureBrain.TYPES.MISSIONARY_II:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_MissionaryII);
      case StructureBrain.TYPES.MISSIONARY_III:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_MissionaryIII);
      case StructureBrain.TYPES.KITCHEN_II:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_KitchenII);
      case StructureBrain.TYPES.FARM_STATION_II:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_FarmStationII);
      default:
        return DataManager.Instance.UnlockedStructures.Contains(Types);
    }
  }

  private static bool IsTempleBuiltOrBeingBuilt()
  {
    int num = Structure.CountStructuresOfType(StructureBrain.TYPES.TEMPLE) + Structure.CountStructuresOfType(StructureBrain.TYPES.TEMPLE_II) + Structure.CountStructuresOfType(StructureBrain.TYPES.TEMPLE_III) + Structure.CountStructuresOfType(StructureBrain.TYPES.TEMPLE_IV);
    foreach (Structures_BuildSite structuresBuildSite in StructureManager.GetAllStructuresOfType<Structures_BuildSite>())
    {
      if (structuresBuildSite.Data.ToBuildType == StructureBrain.TYPES.TEMPLE || structuresBuildSite.Data.ToBuildType == StructureBrain.TYPES.TEMPLE_II || structuresBuildSite.Data.ToBuildType == StructureBrain.TYPES.TEMPLE_III || structuresBuildSite.Data.ToBuildType == StructureBrain.TYPES.TEMPLE_IV)
        ++num;
    }
    return num > 0;
  }

  public static void CompleteResearch(StructureBrain.TYPES Types)
  {
    if (!StructuresData.GetUnlocked(Types))
      DataManager.Instance.UnlockedStructures.Add(Types);
    NotificationCentre.Instance.PlayGenericNotification(NotificationCentre.NotificationType.ResearchComplete);
    int index = -1;
    while (++index < DataManager.Instance.CurrentResearch.Count)
    {
      if (DataManager.Instance.CurrentResearch[index].Type == Types)
        DataManager.Instance.CurrentResearch.RemoveAt(index);
    }
  }

  public static void BeginResearch(StructureBrain.TYPES Types)
  {
    if (StructuresData.ResearchExists(Types))
      return;
    DataManager.Instance.CurrentResearch.Add(new StructuresData.ResearchObject(Types));
    System.Action onResearchBegin = StructuresData.OnResearchBegin;
    if (onResearchBegin == null)
      return;
    onResearchBegin();
  }

  public static bool ResearchExists(StructureBrain.TYPES Types)
  {
    foreach (StructuresData.ResearchObject researchObject in DataManager.Instance.CurrentResearch)
    {
      if (researchObject.Type == Types)
        return true;
    }
    return false;
  }

  public static bool GetAnyResearchExists() => DataManager.Instance.CurrentResearch.Count > 0;

  public static float ResearchProgressByType(StructureBrain.TYPES Types)
  {
    foreach (StructuresData.ResearchObject researchObject in DataManager.Instance.CurrentResearch)
    {
      if (researchObject.Type == Types)
        return researchObject.Progress / researchObject.TargetProgress;
    }
    return 0.0f;
  }

  public static float CurrentResearchProgress()
  {
    return DataManager.Instance.CurrentResearch.Count > 0 ? DataManager.Instance.CurrentResearch[0].Progress / DataManager.Instance.CurrentResearch[0].TargetProgress : 0.5f;
  }

  public static float GetResearchTime(StructureBrain.TYPES Types)
  {
    return StructuresData.ResearchObject.GetResearchTimeInDays(Types);
  }

  public static StructureBrain.TYPES GetMealStructureType(InventoryItem.ITEM_TYPE mealType)
  {
    StructureBrain.TYPES result;
    Enum.TryParse<StructureBrain.TYPES>(mealType.ToString(), true, out result);
    return result;
  }

  public static InventoryItem.ITEM_TYPE GetMealType(StructureBrain.TYPES structureType)
  {
    InventoryItem.ITEM_TYPE result;
    Enum.TryParse<InventoryItem.ITEM_TYPE>(structureType.ToString(), true, out result);
    return result;
  }

  public static int BuildDurationGameMinutes(StructureBrain.TYPES Type)
  {
    switch (Type)
    {
      case StructureBrain.TYPES.BED:
        return 30;
      case StructureBrain.TYPES.SHRINE:
        return 30;
      case StructureBrain.TYPES.DECORATION_TREE:
      case StructureBrain.TYPES.DECORATION_STONE:
      case StructureBrain.TYPES.DECORATION_SMALL_STONE_CANDLE:
      case StructureBrain.TYPES.DECORATION_FLAG_CROWN:
      case StructureBrain.TYPES.DECORATION_WALL_TWIGS:
      case StructureBrain.TYPES.DECORATION_CANDLE_BARREL:
      case StructureBrain.TYPES.DECORATION_WALL_GRASS:
        return 30;
      case StructureBrain.TYPES.REPAIRABLE_HEARTS:
        return 600;
      case StructureBrain.TYPES.REPAIRABLE_CURSES:
        return 600;
      case StructureBrain.TYPES.FARM_PLOT:
        return 30;
      case StructureBrain.TYPES.GRAVE:
        return 30;
      case StructureBrain.TYPES.BODY_PIT:
        return 30;
      case StructureBrain.TYPES.PLANK_PATH:
        return 10;
      case StructureBrain.TYPES.COOKING_FIRE:
        return 20;
      case StructureBrain.TYPES.TEMPLE:
        return 120;
      case StructureBrain.TYPES.TEMPLE_EXTENSION1:
        return 6000;
      case StructureBrain.TYPES.TEMPLE_EXTENSION2:
        return 9000;
      case StructureBrain.TYPES.SHRINE_BLUEHEART:
        return 600;
      case StructureBrain.TYPES.SHRINE_REDHEART:
        return 600;
      case StructureBrain.TYPES.SHRINE_BLACKHEART:
        return 600;
      case StructureBrain.TYPES.SHRINE_TAROT:
        return 600;
      case StructureBrain.TYPES.SHRINE_DAMAGE:
        return 600;
      case StructureBrain.TYPES.SHRINE_II:
        return 180;
      case StructureBrain.TYPES.FARM_PLOT_SIGN:
        return 60;
      case StructureBrain.TYPES.DECORATION_SHRUB:
        return 15;
      case StructureBrain.TYPES.TEMPLE_II:
        return 300;
      case StructureBrain.TYPES.SHRINE_III:
        return 180;
      case StructureBrain.TYPES.SHRINE_IV:
        return 180;
      case StructureBrain.TYPES.TEMPLE_III:
        return 300;
      case StructureBrain.TYPES.TEMPLE_IV:
        return 300;
      case StructureBrain.TYPES.TILE_PATH:
        return 10;
      default:
        return StructuresData.GetCategory(Type) == StructureBrain.Categories.AESTHETIC ? 120 : 300;
    }
  }

  public static List<StructuresData> GetStructuresList(
    List<StructureBrain.TYPES> filterTypeList,
    bool useWhitelist)
  {
    List<StructuresData> structuresList = new List<StructuresData>();
    foreach (StructureBrain.TYPES allStructure in StructuresData.AllStructures)
    {
      if (useWhitelist && filterTypeList.Contains(allStructure) || !useWhitelist && !filterTypeList.Contains(allStructure))
        structuresList.Add(StructuresData.GetInfoByType(allStructure, 0));
    }
    return structuresList;
  }

  public static bool CategoryHasUnrevealed(StructureBrain.Categories Category)
  {
    foreach (StructureBrain.TYPES allStructure in StructuresData.AllStructures)
    {
      if (!StructuresData.HasRevealed(allStructure) && StructuresData.GetCategory(allStructure) == Category && StructuresData.GetOldAvailability(allStructure) == StructuresData.Availabilty.Available)
        return true;
    }
    return false;
  }

  public static bool CategoryHasAvailable(StructureBrain.Categories Category)
  {
    foreach (StructureBrain.TYPES allStructure in StructuresData.AllStructures)
    {
      if (StructuresData.GetCategory(allStructure) == Category && (StructuresData.GetOldAvailability(allStructure) == StructuresData.Availabilty.Available || StructuresData.GetOldAvailability(allStructure) == StructuresData.Availabilty.Locked))
        return true;
    }
    return false;
  }

  public static bool CategoryHasResearchedBuilding(StructureBrain.Categories Category)
  {
    foreach (StructureBrain.TYPES allStructure in StructuresData.AllStructures)
    {
      if (StructuresData.GetCategory(allStructure) == Category && StructuresData.GetUnlocked(allStructure))
        return true;
    }
    return false;
  }

  public static bool IsUpgradeStructure(StructureBrain.TYPES type)
  {
    return (type != StructureBrain.TYPES.SHRINE || !DataManager.Instance.HasBuiltShrine1) && StructuresData.GetUpgradePrerequisite(type) != 0;
  }

  public static List<StructureBrain.TYPES> GetUpgradePath(StructureBrain.TYPES type)
  {
    switch (type)
    {
      case StructureBrain.TYPES.BED:
      case StructureBrain.TYPES.BED_2:
      case StructureBrain.TYPES.BED_3:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.BED,
          StructureBrain.TYPES.BED_2,
          StructureBrain.TYPES.BED_3
        };
      case StructureBrain.TYPES.FARM_STATION:
      case StructureBrain.TYPES.FARM_STATION_II:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.FARM_STATION,
          StructureBrain.TYPES.FARM_STATION_II
        };
      case StructureBrain.TYPES.KITCHEN:
      case StructureBrain.TYPES.COOKING_FIRE:
      case StructureBrain.TYPES.KITCHEN_II:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.COOKING_FIRE,
          StructureBrain.TYPES.KITCHEN,
          StructureBrain.TYPES.KITCHEN_II
        };
      case StructureBrain.TYPES.SHRINE:
      case StructureBrain.TYPES.SHRINE_II:
      case StructureBrain.TYPES.SHRINE_III:
      case StructureBrain.TYPES.SHRINE_IV:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.SHRINE,
          StructureBrain.TYPES.SHRINE_II,
          StructureBrain.TYPES.SHRINE_III,
          StructureBrain.TYPES.SHRINE_IV
        };
      case StructureBrain.TYPES.GRAVE:
      case StructureBrain.TYPES.BODY_PIT:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.GRAVE,
          StructureBrain.TYPES.BODY_PIT
        };
      case StructureBrain.TYPES.LUMBERJACK_STATION:
      case StructureBrain.TYPES.LUMBERJACK_STATION_2:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.LUMBERJACK_STATION,
          StructureBrain.TYPES.LUMBERJACK_STATION_2
        };
      case StructureBrain.TYPES.FOOD_STORAGE:
      case StructureBrain.TYPES.FOOD_STORAGE_2:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.FOOD_STORAGE,
          StructureBrain.TYPES.FOOD_STORAGE_2
        };
      case StructureBrain.TYPES.BLOODSTONE_MINE:
      case StructureBrain.TYPES.BLOODSTONE_MINE_2:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.BLOODSTONE_MINE,
          StructureBrain.TYPES.BLOODSTONE_MINE_2
        };
      case StructureBrain.TYPES.OUTHOUSE:
      case StructureBrain.TYPES.OUTHOUSE_2:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.OUTHOUSE,
          StructureBrain.TYPES.OUTHOUSE_2
        };
      case StructureBrain.TYPES.TEMPLE:
      case StructureBrain.TYPES.TEMPLE_II:
      case StructureBrain.TYPES.TEMPLE_III:
      case StructureBrain.TYPES.TEMPLE_IV:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.TEMPLE,
          StructureBrain.TYPES.TEMPLE_II,
          StructureBrain.TYPES.TEMPLE_III,
          StructureBrain.TYPES.TEMPLE_IV
        };
      case StructureBrain.TYPES.TEMPLE_EXTENSION1:
      case StructureBrain.TYPES.TEMPLE_EXTENSION2:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.TEMPLE_BASE_EXTENSION1,
          StructureBrain.TYPES.TEMPLE_BASE_EXTENSION2
        };
      case StructureBrain.TYPES.HEALING_BAY:
      case StructureBrain.TYPES.HEALING_BAY_2:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.HEALING_BAY,
          StructureBrain.TYPES.HEALING_BAY_2
        };
      case StructureBrain.TYPES.SCARECROW:
      case StructureBrain.TYPES.SCARECROW_2:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.SCARECROW,
          StructureBrain.TYPES.SCARECROW_2
        };
      case StructureBrain.TYPES.HARVEST_TOTEM:
      case StructureBrain.TYPES.HARVEST_TOTEM_2:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.HARVEST_TOTEM,
          StructureBrain.TYPES.HARVEST_TOTEM_2
        };
      case StructureBrain.TYPES.REFINERY:
      case StructureBrain.TYPES.REFINERY_2:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.REFINERY,
          StructureBrain.TYPES.REFINERY_2
        };
      case StructureBrain.TYPES.SHRINE_PASSIVE:
      case StructureBrain.TYPES.SHRINE_PASSIVE_II:
      case StructureBrain.TYPES.SHRINE_PASSIVE_III:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.SHRINE_PASSIVE,
          StructureBrain.TYPES.SHRINE_PASSIVE_II,
          StructureBrain.TYPES.SHRINE_PASSIVE_III
        };
      case StructureBrain.TYPES.MISSIONARY:
      case StructureBrain.TYPES.MISSIONARY_II:
      case StructureBrain.TYPES.MISSIONARY_III:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.MISSIONARY,
          StructureBrain.TYPES.MISSIONARY_II,
          StructureBrain.TYPES.MISSIONARY_III
        };
      case StructureBrain.TYPES.DEMON_SUMMONER:
      case StructureBrain.TYPES.DEMON_SUMMONER_2:
      case StructureBrain.TYPES.DEMON_SUMMONER_3:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.DEMON_SUMMONER,
          StructureBrain.TYPES.DEMON_SUMMONER_2,
          StructureBrain.TYPES.DEMON_SUMMONER_3
        };
      default:
        return (List<StructureBrain.TYPES>) null;
    }
  }

  public static StructureBrain.TYPES GetUpgradePrerequisite(StructureBrain.TYPES type)
  {
    switch (type)
    {
      case StructureBrain.TYPES.KITCHEN:
        return StructureBrain.TYPES.COOKING_FIRE;
      case StructureBrain.TYPES.SHRINE:
        return StructureBrain.TYPES.SHRINE_BASE;
      case StructureBrain.TYPES.BED_2:
        return StructureBrain.TYPES.BED;
      case StructureBrain.TYPES.BED_3:
        return StructureBrain.TYPES.BED_2;
      case StructureBrain.TYPES.SHRINE_FUNDAMENTALIST:
        return StructureBrain.TYPES.SHRINE;
      case StructureBrain.TYPES.SHRINE_MISFIT:
        return StructureBrain.TYPES.SHRINE;
      case StructureBrain.TYPES.SHRINE_UTOPIANIST:
        return StructureBrain.TYPES.SHRINE;
      case StructureBrain.TYPES.GRAVE:
        return StructureBrain.TYPES.BODY_PIT;
      case StructureBrain.TYPES.CULT_UPGRADE2:
        return StructureBrain.TYPES.CULT_UPGRADE1;
      case StructureBrain.TYPES.LUMBERJACK_STATION_2:
        return StructureBrain.TYPES.LUMBERJACK_STATION;
      case StructureBrain.TYPES.RESEARCH_2:
        return StructureBrain.TYPES.CULT_UPGRADE1;
      case StructureBrain.TYPES.FOOD_STORAGE_2:
        return StructureBrain.TYPES.FOOD_STORAGE;
      case StructureBrain.TYPES.BLOODSTONE_MINE_2:
        return StructureBrain.TYPES.BLOODSTONE_MINE;
      case StructureBrain.TYPES.TEMPLE_EXTENSION1:
        return StructureBrain.TYPES.TEMPLE_BASE_EXTENSION1;
      case StructureBrain.TYPES.TEMPLE_EXTENSION2:
        return StructureBrain.TYPES.TEMPLE_BASE_EXTENSION2;
      case StructureBrain.TYPES.SHRINE_II:
        return StructureBrain.TYPES.SHRINE;
      case StructureBrain.TYPES.TEMPLE_II:
        return StructureBrain.TYPES.TEMPLE;
      case StructureBrain.TYPES.SHRINE_III:
        return StructureBrain.TYPES.SHRINE_II;
      case StructureBrain.TYPES.SHRINE_IV:
        return StructureBrain.TYPES.SHRINE_III;
      case StructureBrain.TYPES.SHRINE_PASSIVE_II:
        return StructureBrain.TYPES.SHRINE_PASSIVE;
      case StructureBrain.TYPES.SHRINE_PASSIVE_III:
        return StructureBrain.TYPES.SHRINE_PASSIVE_II;
      case StructureBrain.TYPES.TEMPLE_III:
        return StructureBrain.TYPES.TEMPLE_II;
      case StructureBrain.TYPES.TEMPLE_IV:
        return StructureBrain.TYPES.TEMPLE_III;
      case StructureBrain.TYPES.HEALING_BAY_2:
        return StructureBrain.TYPES.HEALING_BAY;
      case StructureBrain.TYPES.OUTHOUSE_2:
        return StructureBrain.TYPES.OUTHOUSE;
      case StructureBrain.TYPES.SCARECROW_2:
        return StructureBrain.TYPES.SCARECROW;
      case StructureBrain.TYPES.HARVEST_TOTEM_2:
        return StructureBrain.TYPES.HARVEST_TOTEM;
      case StructureBrain.TYPES.REFINERY_2:
        return StructureBrain.TYPES.REFINERY;
      case StructureBrain.TYPES.DEMON_SUMMONER_2:
        return StructureBrain.TYPES.DEMON_SUMMONER;
      case StructureBrain.TYPES.DEMON_SUMMONER_3:
        return StructureBrain.TYPES.DEMON_SUMMONER_2;
      case StructureBrain.TYPES.MISSIONARY_II:
        return StructureBrain.TYPES.MISSIONARY;
      case StructureBrain.TYPES.MISSIONARY_III:
        return StructureBrain.TYPES.MISSIONARY_II;
      case StructureBrain.TYPES.KITCHEN_II:
        return StructureBrain.TYPES.KITCHEN;
      case StructureBrain.TYPES.FARM_STATION_II:
        return StructureBrain.TYPES.FARM_STATION;
      default:
        return StructureBrain.TYPES.NONE;
    }
  }

  public static bool GetBuildOnlyOne(StructureBrain.TYPES Type)
  {
    switch (Type)
    {
      case StructureBrain.TYPES.SHRINE:
      case StructureBrain.TYPES.FARM_PLOT_SOZO:
      case StructureBrain.TYPES.COOKING_FIRE:
      case StructureBrain.TYPES.CONFESSION_BOOTH:
      case StructureBrain.TYPES.TEMPLE:
      case StructureBrain.TYPES.TEMPLE_EXTENSION1:
      case StructureBrain.TYPES.TEMPLE_EXTENSION2:
      case StructureBrain.TYPES.SHRINE_BLUEHEART:
      case StructureBrain.TYPES.SHRINE_REDHEART:
      case StructureBrain.TYPES.SHRINE_BLACKHEART:
      case StructureBrain.TYPES.SHRINE_TAROT:
      case StructureBrain.TYPES.SHRINE_DAMAGE:
      case StructureBrain.TYPES.SHRINE_II:
      case StructureBrain.TYPES.HEALING_BAY:
      case StructureBrain.TYPES.TEMPLE_II:
      case StructureBrain.TYPES.SHRINE_III:
      case StructureBrain.TYPES.SHRINE_IV:
      case StructureBrain.TYPES.TEMPLE_III:
      case StructureBrain.TYPES.TEMPLE_IV:
      case StructureBrain.TYPES.HEALING_BAY_2:
      case StructureBrain.TYPES.MISSIONARY:
      case StructureBrain.TYPES.SURVEILLANCE:
      case StructureBrain.TYPES.CHOPPING_SHRINE:
      case StructureBrain.TYPES.MINING_SHRINE:
      case StructureBrain.TYPES.FORAGING_SHRINE:
      case StructureBrain.TYPES.DEMON_SUMMONER:
      case StructureBrain.TYPES.DEMON_SUMMONER_2:
      case StructureBrain.TYPES.DEMON_SUMMONER_3:
      case StructureBrain.TYPES.MISSIONARY_II:
      case StructureBrain.TYPES.MISSIONARY_III:
      case StructureBrain.TYPES.DECORATION_MONSTERSHRINE:
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_1:
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_2:
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_3:
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_4:
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_5:
      case StructureBrain.TYPES.DECORATION_VIDEO:
      case StructureBrain.TYPES.DECORATION_PLUSH:
        return true;
      default:
        return false;
    }
  }

  public static DataManager.CultLevel GetRequiredLevel(StructureBrain.TYPES Type)
  {
    if (Type <= StructureBrain.TYPES.COLLECTED_RESOURCES_CHEST)
    {
      if (Type <= StructureBrain.TYPES.LUMBERJACK_STATION)
      {
        if (Type <= StructureBrain.TYPES.KITCHEN)
        {
          if (Type != StructureBrain.TYPES.BED)
          {
            switch (Type - 6)
            {
              case StructureBrain.TYPES.NONE:
              case StructureBrain.TYPES.SACRIFICIAL_TEMPLE:
                break;
              case StructureBrain.TYPES.BED:
                goto label_20;
              default:
                goto label_22;
            }
          }
        }
        else
        {
          switch (Type - 27)
          {
            case StructureBrain.TYPES.NONE:
            case StructureBrain.TYPES.HEALING_BATH:
            case StructureBrain.TYPES.BUILD_PLOT:
            case StructureBrain.TYPES.SHRINE:
            case StructureBrain.TYPES.BARRACKS:
            case StructureBrain.TYPES.ASTROLOGIST:
            case StructureBrain.TYPES.BUILD_SITE:
            case StructureBrain.TYPES.ALTAR:
              break;
            case StructureBrain.TYPES.BUILDER:
            case StructureBrain.TYPES.BED:
            case StructureBrain.TYPES.PORTAL:
            case StructureBrain.TYPES.SACRIFICIAL_TEMPLE:
            case StructureBrain.TYPES.WOOD_STORE:
            case StructureBrain.TYPES.BLACKSMITH:
            case StructureBrain.TYPES.KITCHEN:
            case StructureBrain.TYPES.COOKED_FOOD_SILO:
            case StructureBrain.TYPES.DEFENCE_TOWER:
            case StructureBrain.TYPES.TREE:
            case StructureBrain.TYPES.BUSH:
            case StructureBrain.TYPES.ROCK:
            case StructureBrain.TYPES.FOLLOWER_RECRUIT:
            case StructureBrain.TYPES.SEED_FLOWER:
            case StructureBrain.TYPES.COTTON_PLANT:
            case StructureBrain.TYPES.FIRE_PIT:
              goto label_22;
            case StructureBrain.TYPES.TAVERN:
            case StructureBrain.TYPES.FARM_STATION:
            case StructureBrain.TYPES.WHEAT_SILO:
            case StructureBrain.TYPES.CROP:
            case StructureBrain.TYPES.NIGHTMARE_MACHINE:
            case StructureBrain.TYPES.GRASS:
            case StructureBrain.TYPES.FIRE:
            case StructureBrain.TYPES.STORAGE_PIT:
              goto label_20;
            case StructureBrain.TYPES.MONSTER_HIVE:
              goto label_21;
            default:
              if (Type == StructureBrain.TYPES.LUMBERJACK_STATION)
                break;
              goto label_22;
          }
        }
      }
      else if (Type <= StructureBrain.TYPES.FISHING_HUT)
      {
        if ((uint) (Type - 71) > 2U && Type != StructureBrain.TYPES.FISHING_HUT)
          goto label_22;
      }
      else if (Type != StructureBrain.TYPES.OUTHOUSE && (uint) (Type - 126) > 1U)
        goto label_22;
    }
    else if (Type <= StructureBrain.TYPES.TILE_PATH)
    {
      if (Type <= StructureBrain.TYPES.HARVEST_TOTEM)
      {
        if (Type != StructureBrain.TYPES.HEALING_BAY && (uint) (Type - 136) > 1U)
          goto label_22;
      }
      else
      {
        switch (Type - 149)
        {
          case StructureBrain.TYPES.NONE:
          case StructureBrain.TYPES.SACRIFICIAL_TEMPLE:
            break;
          case StructureBrain.TYPES.BUILDER:
            goto label_22;
          case StructureBrain.TYPES.BED:
            goto label_20;
          case StructureBrain.TYPES.PORTAL:
            goto label_21;
          default:
            if (Type == StructureBrain.TYPES.TILE_PATH)
              break;
            goto label_22;
        }
      }
    }
    else if (Type <= StructureBrain.TYPES.FORAGING_SHRINE)
    {
      if (Type != StructureBrain.TYPES.HEALING_BAY_2)
      {
        if ((uint) (Type - 212) > 2U)
          goto label_22;
      }
      else
        goto label_20;
    }
    else if (Type == StructureBrain.TYPES.JANITOR_STATION || Type != StructureBrain.TYPES.KITCHEN_II && Type == StructureBrain.TYPES.FARM_STATION_II)
      goto label_20;
    return DataManager.CultLevel.One;
label_20:
    return DataManager.CultLevel.Two;
label_21:
    return DataManager.CultLevel.Three;
label_22:
    return DataManager.CultLevel.One;
  }

  public static string GetBuildSfx(StructureBrain.TYPES Type)
  {
    switch (Type)
    {
      case StructureBrain.TYPES.BED:
      case StructureBrain.TYPES.BED_2:
      case StructureBrain.TYPES.BED_3:
      case StructureBrain.TYPES.DECORATION_FLAG_CROWN:
      case StructureBrain.TYPES.DECORATION_FLAG_SCRIPTURE:
      case StructureBrain.TYPES.DECORATION_WALL_GRASS:
      case StructureBrain.TYPES.DECORATION_FLAG_CRYSTAL:
      case StructureBrain.TYPES.DECORATION_FLAG_MUSHROOM:
      case StructureBrain.TYPES.DECORATION_SPIDER_PILLAR:
      case StructureBrain.TYPES.DECORATION_SPIDER_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_SPIDER_TORCH:
      case StructureBrain.TYPES.DECORATION_WALL_SPIDER:
      case StructureBrain.TYPES.DECORATION_PLUSH:
      case StructureBrain.TYPES.DECORATION_TWITCH_FLAG_CROWN:
      case StructureBrain.TYPES.DECORATION_TWITCH_MUSHROOM_BAG:
        return "event:/building/finished_fabric";
      case StructureBrain.TYPES.BLACKSMITH:
      case StructureBrain.TYPES.SHRINE:
      case StructureBrain.TYPES.DECORATION_STONE:
      case StructureBrain.TYPES.DECORATION_SMALL_STONE_CANDLE:
      case StructureBrain.TYPES.DECORATION_LAMB_FLAG_STATUE:
      case StructureBrain.TYPES.SHRINE_BLUEHEART:
      case StructureBrain.TYPES.SHRINE_REDHEART:
      case StructureBrain.TYPES.SHRINE_BLACKHEART:
      case StructureBrain.TYPES.SHRINE_TAROT:
      case StructureBrain.TYPES.SHRINE_DAMAGE:
      case StructureBrain.TYPES.SHRINE_II:
      case StructureBrain.TYPES.SHRINE_PASSIVE:
      case StructureBrain.TYPES.SHRINE_III:
      case StructureBrain.TYPES.SHRINE_IV:
      case StructureBrain.TYPES.OFFERING_STATUE:
      case StructureBrain.TYPES.SHRINE_PASSIVE_II:
      case StructureBrain.TYPES.SHRINE_PASSIVE_III:
      case StructureBrain.TYPES.PROPAGANDA_SPEAKER:
      case StructureBrain.TYPES.DECORATION_FOUNTAIN:
      case StructureBrain.TYPES.DECORATION_STONE_CANDLE:
      case StructureBrain.TYPES.DECORATION_STONE_FLAG:
      case StructureBrain.TYPES.DECORATION_STONE_MUSHROOM:
      case StructureBrain.TYPES.DECORATION_WALL_STONE:
      case StructureBrain.TYPES.DECORATION_STONE_CANDLE_LAMP:
      case StructureBrain.TYPES.DECORATION_STONE_HENGE:
      case StructureBrain.TYPES.DECORATION_POND:
      case StructureBrain.TYPES.DECORATION_MONSTERSHRINE:
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_1:
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_2:
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_3:
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_4:
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_5:
      case StructureBrain.TYPES.DECORATION_VIDEO:
      case StructureBrain.TYPES.DECORATION_TWITCH_STONE_FLAG:
      case StructureBrain.TYPES.DECORATION_TWITCH_STONE_STATUE:
      case StructureBrain.TYPES.DECORATION_TWITCH_WOODEN_GUARDIAN:
      case StructureBrain.TYPES.DECORATION_HALLOWEEN_SKULL:
      case StructureBrain.TYPES.DECORATION_HALLOWEEN_CANDLE:
        return "event:/building/finished_stone";
      case StructureBrain.TYPES.FARM_STATION:
      case StructureBrain.TYPES.KITCHEN:
      case StructureBrain.TYPES.DECORATION_TREE:
      case StructureBrain.TYPES.PLANK_PATH:
      case StructureBrain.TYPES.PRISON:
      case StructureBrain.TYPES.LUMBERJACK_STATION:
      case StructureBrain.TYPES.COOKING_FIRE:
      case StructureBrain.TYPES.FOOD_STORAGE:
      case StructureBrain.TYPES.FOOD_STORAGE_2:
      case StructureBrain.TYPES.FISHING_HUT:
      case StructureBrain.TYPES.OUTHOUSE:
      case StructureBrain.TYPES.DECORATION_TORCH:
      case StructureBrain.TYPES.DECORATION_FLOWER_BOX_1:
      case StructureBrain.TYPES.DECORATION_FLOWER_BOX_2:
      case StructureBrain.TYPES.DECORATION_WALL_TWIGS:
      case StructureBrain.TYPES.COLLECTED_RESOURCES_CHEST:
      case StructureBrain.TYPES.HEALING_BAY:
      case StructureBrain.TYPES.SCARECROW:
      case StructureBrain.TYPES.HARVEST_TOTEM:
      case StructureBrain.TYPES.FARM_PLOT_SIGN:
      case StructureBrain.TYPES.TILE_PATH:
      case StructureBrain.TYPES.HEALING_BAY_2:
      case StructureBrain.TYPES.SILO_SEED:
      case StructureBrain.TYPES.SILO_FERTILISER:
      case StructureBrain.TYPES.SURVEILLANCE:
      case StructureBrain.TYPES.FISHING_HUT_2:
      case StructureBrain.TYPES.OUTHOUSE_2:
      case StructureBrain.TYPES.SCARECROW_2:
      case StructureBrain.TYPES.HARVEST_TOTEM_2:
      case StructureBrain.TYPES.DECORATION_BARROW:
      case StructureBrain.TYPES.DECORATION_BELL_STATUE:
      case StructureBrain.TYPES.DECORATION_BONE_ARCH:
      case StructureBrain.TYPES.DECORATION_BONE_BARREL:
      case StructureBrain.TYPES.DECORATION_BONE_CANDLE:
      case StructureBrain.TYPES.DECORATION_BONE_FLAG:
      case StructureBrain.TYPES.DECORATION_BONE_LANTERN:
      case StructureBrain.TYPES.DECORATION_BONE_PILLAR:
      case StructureBrain.TYPES.DECORATION_BONE_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_CANDLE_BARREL:
      case StructureBrain.TYPES.DECORATION_CRYSTAL_LAMP:
      case StructureBrain.TYPES.DECORATION_CRYSTAL_LIGHT:
      case StructureBrain.TYPES.DECORATION_CRYSTAL_ROCK:
      case StructureBrain.TYPES.DECORATION_CRYSTAL_STATUE:
      case StructureBrain.TYPES.DECORATION_CRYSTAL_TREE:
      case StructureBrain.TYPES.DECORATION_CRYSTAL_WINDOW:
      case StructureBrain.TYPES.DECORATION_FLOWER_ARCH:
      case StructureBrain.TYPES.DECORATION_POST_BOX:
      case StructureBrain.TYPES.DECORATION_PUMPKIN_PILE:
      case StructureBrain.TYPES.DECORATION_PUMPKIN_STOOL:
      case StructureBrain.TYPES.DECORATION_TORCH_BIG:
      case StructureBrain.TYPES.DECORATION_TWIG_LAMP:
      case StructureBrain.TYPES.DECORATION_WALL_BONE:
      case StructureBrain.TYPES.DECORATION_WREATH_STICK:
      case StructureBrain.TYPES.DECORATION_STUMP_LAMB_STATUE:
      case StructureBrain.TYPES.JANITOR_STATION:
      case StructureBrain.TYPES.DECORATION_BELL_SMALL:
      case StructureBrain.TYPES.DECORATION_BONE_SKULL_BIG:
      case StructureBrain.TYPES.DECORATION_BONE_SKULL_PILE:
      case StructureBrain.TYPES.DECORATION_FLOWER_BOTTLE:
      case StructureBrain.TYPES.DECORATION_FLOWER_CART:
      case StructureBrain.TYPES.DECORATION_LEAFY_FLOWER_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_LEAFY_LANTERN:
      case StructureBrain.TYPES.DECORATION_LEAFY_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_MUSHROOM_1:
      case StructureBrain.TYPES.DECORATION_MUSHROOM_2:
      case StructureBrain.TYPES.DECORATION_MUSHROOM_CANDLE_1:
      case StructureBrain.TYPES.DECORATION_MUSHROOM_CANDLE_2:
      case StructureBrain.TYPES.DECORATION_MUSHROOM_CANDLE_LARGE:
      case StructureBrain.TYPES.DECORATION_MUSHROOM_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_SPIDER_LANTERN:
      case StructureBrain.TYPES.DECORATION_SPIDER_WEB_CROWN_SCULPTURE:
      case StructureBrain.TYPES.KITCHEN_II:
      case StructureBrain.TYPES.FARM_STATION_II:
      case StructureBrain.TYPES.DECORATION_FLOWERPOTWALL:
      case StructureBrain.TYPES.DECORATION_LEAFYLAMPPOST:
      case StructureBrain.TYPES.DECORATION_FLOWERVASE:
      case StructureBrain.TYPES.DECORATION_WATERINGCAN:
      case StructureBrain.TYPES.DECORATION_FLOWER_CART_SMALL:
      case StructureBrain.TYPES.DECORATION_WEEPINGSHRINE:
      case StructureBrain.TYPES.DECORATION_TWITCH_ROSE_BUSH:
      case StructureBrain.TYPES.DECORATION_HALLOWEEN_PUMPKIN:
      case StructureBrain.TYPES.DECORATION_HALLOWEEN_TREE:
        return "event:/building/finished_wood";
      case StructureBrain.TYPES.FARM_PLOT:
      case StructureBrain.TYPES.FARM_PLOT_SOZO:
      case StructureBrain.TYPES.DECORATION_HAY_BALE:
      case StructureBrain.TYPES.DECORATION_HAY_PILE:
        return "event:/building/finished_farmplot";
      case StructureBrain.TYPES.GRAVE:
      case StructureBrain.TYPES.BODY_PIT:
        return "event:/building/finished_grave";
      default:
        return "event:/building/finished_wood";
    }
  }

  public static bool GetRequiresUnlock(StructureBrain.TYPES Type) => false;

  public static UIBuildMenuController.Category CategoryForType(StructureBrain.TYPES type)
  {
    switch (type)
    {
      case StructureBrain.TYPES.SHRINE:
      case StructureBrain.TYPES.DANCING_FIREPIT:
      case StructureBrain.TYPES.CONFESSION_BOOTH:
      case StructureBrain.TYPES.TEMPLE:
      case StructureBrain.TYPES.SHRINE_II:
      case StructureBrain.TYPES.TEMPLE_II:
      case StructureBrain.TYPES.SHRINE_PASSIVE:
      case StructureBrain.TYPES.SHRINE_III:
      case StructureBrain.TYPES.SHRINE_IV:
      case StructureBrain.TYPES.OFFERING_STATUE:
      case StructureBrain.TYPES.SHRINE_PASSIVE_II:
      case StructureBrain.TYPES.SHRINE_PASSIVE_III:
      case StructureBrain.TYPES.TEMPLE_III:
      case StructureBrain.TYPES.TEMPLE_IV:
      case StructureBrain.TYPES.MISSIONARY:
      case StructureBrain.TYPES.MISSIONARY_II:
      case StructureBrain.TYPES.MISSIONARY_III:
        return UIBuildMenuController.Category.Faith;
      case StructureBrain.TYPES.DECORATION_TREE:
      case StructureBrain.TYPES.DECORATION_STONE:
      case StructureBrain.TYPES.PLANK_PATH:
      case StructureBrain.TYPES.DECORATION_TORCH:
      case StructureBrain.TYPES.DECORATION_FLOWER_BOX_1:
      case StructureBrain.TYPES.DECORATION_SMALL_STONE_CANDLE:
      case StructureBrain.TYPES.DECORATION_FLAG_CROWN:
      case StructureBrain.TYPES.DECORATION_FLAG_SCRIPTURE:
      case StructureBrain.TYPES.DECORATION_WALL_TWIGS:
      case StructureBrain.TYPES.DECORATION_LAMB_FLAG_STATUE:
      case StructureBrain.TYPES.FARM_PLOT_SIGN:
      case StructureBrain.TYPES.TILE_PATH:
      case StructureBrain.TYPES.DECORATION_BARROW:
      case StructureBrain.TYPES.DECORATION_BELL_STATUE:
      case StructureBrain.TYPES.DECORATION_BONE_ARCH:
      case StructureBrain.TYPES.DECORATION_BONE_BARREL:
      case StructureBrain.TYPES.DECORATION_BONE_CANDLE:
      case StructureBrain.TYPES.DECORATION_BONE_FLAG:
      case StructureBrain.TYPES.DECORATION_BONE_LANTERN:
      case StructureBrain.TYPES.DECORATION_BONE_PILLAR:
      case StructureBrain.TYPES.DECORATION_BONE_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_CANDLE_BARREL:
      case StructureBrain.TYPES.DECORATION_CRYSTAL_LAMP:
      case StructureBrain.TYPES.DECORATION_CRYSTAL_LIGHT:
      case StructureBrain.TYPES.DECORATION_CRYSTAL_ROCK:
      case StructureBrain.TYPES.DECORATION_CRYSTAL_STATUE:
      case StructureBrain.TYPES.DECORATION_CRYSTAL_TREE:
      case StructureBrain.TYPES.DECORATION_CRYSTAL_WINDOW:
      case StructureBrain.TYPES.DECORATION_FLOWER_ARCH:
      case StructureBrain.TYPES.DECORATION_FOUNTAIN:
      case StructureBrain.TYPES.DECORATION_POST_BOX:
      case StructureBrain.TYPES.DECORATION_PUMPKIN_PILE:
      case StructureBrain.TYPES.DECORATION_PUMPKIN_STOOL:
      case StructureBrain.TYPES.DECORATION_STONE_CANDLE:
      case StructureBrain.TYPES.DECORATION_STONE_FLAG:
      case StructureBrain.TYPES.DECORATION_STONE_MUSHROOM:
      case StructureBrain.TYPES.DECORATION_TORCH_BIG:
      case StructureBrain.TYPES.DECORATION_TWIG_LAMP:
      case StructureBrain.TYPES.DECORATION_WALL_BONE:
      case StructureBrain.TYPES.DECORATION_WALL_STONE:
      case StructureBrain.TYPES.DECORATION_WALL_GRASS:
      case StructureBrain.TYPES.DECORATION_WREATH_STICK:
      case StructureBrain.TYPES.DECORATION_STUMP_LAMB_STATUE:
      case StructureBrain.TYPES.DECORATION_BELL_SMALL:
      case StructureBrain.TYPES.DECORATION_BONE_SKULL_BIG:
      case StructureBrain.TYPES.DECORATION_BONE_SKULL_PILE:
      case StructureBrain.TYPES.DECORATION_FLAG_CRYSTAL:
      case StructureBrain.TYPES.DECORATION_FLAG_MUSHROOM:
      case StructureBrain.TYPES.DECORATION_FLOWER_BOTTLE:
      case StructureBrain.TYPES.DECORATION_FLOWER_CART:
      case StructureBrain.TYPES.DECORATION_HAY_BALE:
      case StructureBrain.TYPES.DECORATION_HAY_PILE:
      case StructureBrain.TYPES.DECORATION_LEAFY_FLOWER_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_LEAFY_LANTERN:
      case StructureBrain.TYPES.DECORATION_LEAFY_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_MUSHROOM_1:
      case StructureBrain.TYPES.DECORATION_MUSHROOM_2:
      case StructureBrain.TYPES.DECORATION_MUSHROOM_CANDLE_1:
      case StructureBrain.TYPES.DECORATION_MUSHROOM_CANDLE_2:
      case StructureBrain.TYPES.DECORATION_MUSHROOM_CANDLE_LARGE:
      case StructureBrain.TYPES.DECORATION_MUSHROOM_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_SPIDER_LANTERN:
      case StructureBrain.TYPES.DECORATION_SPIDER_PILLAR:
      case StructureBrain.TYPES.DECORATION_SPIDER_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_SPIDER_TORCH:
      case StructureBrain.TYPES.DECORATION_SPIDER_WEB_CROWN_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_STONE_CANDLE_LAMP:
      case StructureBrain.TYPES.DECORATION_STONE_HENGE:
      case StructureBrain.TYPES.DECORATION_WALL_SPIDER:
      case StructureBrain.TYPES.DECORATION_POND:
      case StructureBrain.TYPES.TILE_FLOWERS:
      case StructureBrain.TYPES.TILE_HAY:
      case StructureBrain.TYPES.TILE_PLANKS:
      case StructureBrain.TYPES.TILE_SPOOKYPLANKS:
      case StructureBrain.TYPES.TILE_REDGRASS:
      case StructureBrain.TYPES.TILE_ROCKS:
      case StructureBrain.TYPES.TILE_BRICKS:
      case StructureBrain.TYPES.TILE_BLOOD:
      case StructureBrain.TYPES.TILE_WATER:
      case StructureBrain.TYPES.TILE_GOLD:
      case StructureBrain.TYPES.TILE_MOSAIC:
      case StructureBrain.TYPES.TILE_FLOWERSROCKY:
      case StructureBrain.TYPES.DECORATION_MONSTERSHRINE:
      case StructureBrain.TYPES.DECORATION_FLOWERPOTWALL:
      case StructureBrain.TYPES.DECORATION_LEAFYLAMPPOST:
      case StructureBrain.TYPES.DECORATION_FLOWERVASE:
      case StructureBrain.TYPES.DECORATION_WATERINGCAN:
      case StructureBrain.TYPES.DECORATION_FLOWER_CART_SMALL:
      case StructureBrain.TYPES.DECORATION_WEEPINGSHRINE:
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_1:
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_2:
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_3:
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_4:
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_5:
      case StructureBrain.TYPES.DECORATION_VIDEO:
      case StructureBrain.TYPES.DECORATION_PLUSH:
      case StructureBrain.TYPES.DECORATION_TWITCH_FLAG_CROWN:
      case StructureBrain.TYPES.DECORATION_TWITCH_MUSHROOM_BAG:
      case StructureBrain.TYPES.DECORATION_TWITCH_ROSE_BUSH:
      case StructureBrain.TYPES.DECORATION_TWITCH_STONE_FLAG:
      case StructureBrain.TYPES.DECORATION_TWITCH_STONE_STATUE:
      case StructureBrain.TYPES.DECORATION_TWITCH_WOODEN_GUARDIAN:
      case StructureBrain.TYPES.DECORATION_HALLOWEEN_PUMPKIN:
      case StructureBrain.TYPES.DECORATION_HALLOWEEN_SKULL:
      case StructureBrain.TYPES.DECORATION_HALLOWEEN_CANDLE:
      case StructureBrain.TYPES.DECORATION_HALLOWEEN_TREE:
        return UIBuildMenuController.Category.Aesthetic;
      default:
        return UIBuildMenuController.Category.Follower;
    }
  }

  public static StructureBrain.Categories GetCategory(StructureBrain.TYPES Type)
  {
    switch (Type)
    {
      case StructureBrain.TYPES.BED:
        return StructureBrain.Categories.FOLLOWERS;
      case StructureBrain.TYPES.FARM_STATION:
        return StructureBrain.Categories.FOOD;
      case StructureBrain.TYPES.KITCHEN:
        return StructureBrain.Categories.FOOD;
      case StructureBrain.TYPES.SHRINE:
      case StructureBrain.TYPES.DANCING_FIREPIT:
      case StructureBrain.TYPES.PRISON:
      case StructureBrain.TYPES.CONFESSION_BOOTH:
      case StructureBrain.TYPES.TEMPLE_BASE:
      case StructureBrain.TYPES.TEMPLE:
      case StructureBrain.TYPES.TEMPLE_EXTENSION1:
      case StructureBrain.TYPES.TEMPLE_EXTENSION2:
      case StructureBrain.TYPES.TEMPLE_BASE_EXTENSION1:
      case StructureBrain.TYPES.TEMPLE_BASE_EXTENSION2:
      case StructureBrain.TYPES.SHRINE_II:
      case StructureBrain.TYPES.TEMPLE_II:
      case StructureBrain.TYPES.SHRINE_III:
      case StructureBrain.TYPES.SHRINE_IV:
      case StructureBrain.TYPES.TEMPLE_III:
      case StructureBrain.TYPES.TEMPLE_IV:
      case StructureBrain.TYPES.CHOPPING_SHRINE:
      case StructureBrain.TYPES.MINING_SHRINE:
      case StructureBrain.TYPES.FORAGING_SHRINE:
        return StructureBrain.Categories.FAITH;
      case StructureBrain.TYPES.DECORATION_TREE:
      case StructureBrain.TYPES.DECORATION_STONE:
      case StructureBrain.TYPES.PLANK_PATH:
      case StructureBrain.TYPES.DECORATION_TORCH:
      case StructureBrain.TYPES.DECORATION_FLOWER_BOX_1:
      case StructureBrain.TYPES.DECORATION_SMALL_STONE_CANDLE:
      case StructureBrain.TYPES.DECORATION_FLAG_CROWN:
      case StructureBrain.TYPES.DECORATION_FLAG_SCRIPTURE:
      case StructureBrain.TYPES.DECORATION_WALL_TWIGS:
      case StructureBrain.TYPES.DECORATION_LAMB_FLAG_STATUE:
      case StructureBrain.TYPES.FARM_PLOT_SIGN:
      case StructureBrain.TYPES.TILE_PATH:
      case StructureBrain.TYPES.DECORATION_BARROW:
      case StructureBrain.TYPES.DECORATION_BELL_STATUE:
      case StructureBrain.TYPES.DECORATION_BONE_ARCH:
      case StructureBrain.TYPES.DECORATION_BONE_BARREL:
      case StructureBrain.TYPES.DECORATION_BONE_CANDLE:
      case StructureBrain.TYPES.DECORATION_BONE_FLAG:
      case StructureBrain.TYPES.DECORATION_BONE_LANTERN:
      case StructureBrain.TYPES.DECORATION_BONE_PILLAR:
      case StructureBrain.TYPES.DECORATION_BONE_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_CANDLE_BARREL:
      case StructureBrain.TYPES.DECORATION_CRYSTAL_LAMP:
      case StructureBrain.TYPES.DECORATION_CRYSTAL_LIGHT:
      case StructureBrain.TYPES.DECORATION_CRYSTAL_ROCK:
      case StructureBrain.TYPES.DECORATION_CRYSTAL_STATUE:
      case StructureBrain.TYPES.DECORATION_CRYSTAL_TREE:
      case StructureBrain.TYPES.DECORATION_CRYSTAL_WINDOW:
      case StructureBrain.TYPES.DECORATION_FLOWER_ARCH:
      case StructureBrain.TYPES.DECORATION_FOUNTAIN:
      case StructureBrain.TYPES.DECORATION_POST_BOX:
      case StructureBrain.TYPES.DECORATION_PUMPKIN_PILE:
      case StructureBrain.TYPES.DECORATION_PUMPKIN_STOOL:
      case StructureBrain.TYPES.DECORATION_STONE_CANDLE:
      case StructureBrain.TYPES.DECORATION_STONE_FLAG:
      case StructureBrain.TYPES.DECORATION_STONE_MUSHROOM:
      case StructureBrain.TYPES.DECORATION_TORCH_BIG:
      case StructureBrain.TYPES.DECORATION_TWIG_LAMP:
      case StructureBrain.TYPES.DECORATION_WALL_BONE:
      case StructureBrain.TYPES.DECORATION_WALL_STONE:
      case StructureBrain.TYPES.DECORATION_WALL_GRASS:
      case StructureBrain.TYPES.DECORATION_WREATH_STICK:
      case StructureBrain.TYPES.DECORATION_STUMP_LAMB_STATUE:
      case StructureBrain.TYPES.DECORATION_BELL_SMALL:
      case StructureBrain.TYPES.DECORATION_BONE_SKULL_BIG:
      case StructureBrain.TYPES.DECORATION_BONE_SKULL_PILE:
      case StructureBrain.TYPES.DECORATION_FLAG_CRYSTAL:
      case StructureBrain.TYPES.DECORATION_FLAG_MUSHROOM:
      case StructureBrain.TYPES.DECORATION_FLOWER_BOTTLE:
      case StructureBrain.TYPES.DECORATION_FLOWER_CART:
      case StructureBrain.TYPES.DECORATION_HAY_BALE:
      case StructureBrain.TYPES.DECORATION_HAY_PILE:
      case StructureBrain.TYPES.DECORATION_LEAFY_FLOWER_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_LEAFY_LANTERN:
      case StructureBrain.TYPES.DECORATION_LEAFY_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_MUSHROOM_1:
      case StructureBrain.TYPES.DECORATION_MUSHROOM_2:
      case StructureBrain.TYPES.DECORATION_MUSHROOM_CANDLE_1:
      case StructureBrain.TYPES.DECORATION_MUSHROOM_CANDLE_2:
      case StructureBrain.TYPES.DECORATION_MUSHROOM_CANDLE_LARGE:
      case StructureBrain.TYPES.DECORATION_MUSHROOM_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_SPIDER_LANTERN:
      case StructureBrain.TYPES.DECORATION_SPIDER_PILLAR:
      case StructureBrain.TYPES.DECORATION_SPIDER_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_SPIDER_TORCH:
      case StructureBrain.TYPES.DECORATION_SPIDER_WEB_CROWN_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_STONE_CANDLE_LAMP:
      case StructureBrain.TYPES.DECORATION_STONE_HENGE:
      case StructureBrain.TYPES.DECORATION_WALL_SPIDER:
      case StructureBrain.TYPES.DECORATION_POND:
      case StructureBrain.TYPES.TILE_FLOWERS:
      case StructureBrain.TYPES.TILE_HAY:
      case StructureBrain.TYPES.TILE_PLANKS:
      case StructureBrain.TYPES.TILE_SPOOKYPLANKS:
      case StructureBrain.TYPES.TILE_REDGRASS:
      case StructureBrain.TYPES.TILE_ROCKS:
      case StructureBrain.TYPES.TILE_BRICKS:
      case StructureBrain.TYPES.TILE_BLOOD:
      case StructureBrain.TYPES.TILE_WATER:
      case StructureBrain.TYPES.TILE_GOLD:
      case StructureBrain.TYPES.TILE_MOSAIC:
      case StructureBrain.TYPES.TILE_FLOWERSROCKY:
      case StructureBrain.TYPES.DECORATION_MONSTERSHRINE:
      case StructureBrain.TYPES.DECORATION_FLOWERPOTWALL:
      case StructureBrain.TYPES.DECORATION_LEAFYLAMPPOST:
      case StructureBrain.TYPES.DECORATION_FLOWERVASE:
      case StructureBrain.TYPES.DECORATION_WATERINGCAN:
      case StructureBrain.TYPES.DECORATION_FLOWER_CART_SMALL:
      case StructureBrain.TYPES.DECORATION_WEEPINGSHRINE:
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_1:
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_2:
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_3:
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_4:
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_5:
      case StructureBrain.TYPES.DECORATION_VIDEO:
      case StructureBrain.TYPES.DECORATION_PLUSH:
      case StructureBrain.TYPES.DECORATION_TWITCH_FLAG_CROWN:
      case StructureBrain.TYPES.DECORATION_TWITCH_MUSHROOM_BAG:
      case StructureBrain.TYPES.DECORATION_TWITCH_ROSE_BUSH:
      case StructureBrain.TYPES.DECORATION_TWITCH_STONE_FLAG:
      case StructureBrain.TYPES.DECORATION_TWITCH_STONE_STATUE:
      case StructureBrain.TYPES.DECORATION_TWITCH_WOODEN_GUARDIAN:
      case StructureBrain.TYPES.DECORATION_HALLOWEEN_PUMPKIN:
      case StructureBrain.TYPES.DECORATION_HALLOWEEN_SKULL:
      case StructureBrain.TYPES.DECORATION_HALLOWEEN_CANDLE:
      case StructureBrain.TYPES.DECORATION_HALLOWEEN_TREE:
        return StructureBrain.Categories.AESTHETIC;
      case StructureBrain.TYPES.BED_2:
        return StructureBrain.Categories.FOLLOWERS;
      case StructureBrain.TYPES.FARM_PLOT:
        return StructureBrain.Categories.FOOD;
      case StructureBrain.TYPES.GRAVE:
        return StructureBrain.Categories.FOLLOWERS;
      case StructureBrain.TYPES.FARM_PLOT_SOZO:
        return StructureBrain.Categories.FOOD;
      case StructureBrain.TYPES.BODY_PIT:
        return StructureBrain.Categories.FOLLOWERS;
      case StructureBrain.TYPES.LUMBERJACK_STATION:
      case StructureBrain.TYPES.FISHING_HUT:
      case StructureBrain.TYPES.COLLECTED_RESOURCES_CHEST:
        return StructureBrain.Categories.ECONOMY;
      case StructureBrain.TYPES.COOKING_FIRE:
        return StructureBrain.Categories.FOOD;
      case StructureBrain.TYPES.FOOD_STORAGE:
        return StructureBrain.Categories.FOOD;
      case StructureBrain.TYPES.FOOD_STORAGE_2:
        return StructureBrain.Categories.FOOD;
      case StructureBrain.TYPES.OUTHOUSE:
        return StructureBrain.Categories.FOLLOWERS;
      case StructureBrain.TYPES.COMPOST_BIN:
        return StructureBrain.Categories.FOOD;
      case StructureBrain.TYPES.SHRINE_BLUEHEART:
      case StructureBrain.TYPES.SHRINE_REDHEART:
      case StructureBrain.TYPES.SHRINE_BLACKHEART:
      case StructureBrain.TYPES.SHRINE_TAROT:
      case StructureBrain.TYPES.SHRINE_DAMAGE:
        return StructureBrain.Categories.COMBAT;
      case StructureBrain.TYPES.HEALING_BAY:
        return StructureBrain.Categories.FOLLOWERS;
      case StructureBrain.TYPES.SCARECROW:
        return StructureBrain.Categories.FOOD;
      case StructureBrain.TYPES.HARVEST_TOTEM:
        return StructureBrain.Categories.FOOD;
      case StructureBrain.TYPES.REFINERY:
        return StructureBrain.Categories.FOOD;
      case StructureBrain.TYPES.HEALING_BAY_2:
        return StructureBrain.Categories.FOLLOWERS;
      case StructureBrain.TYPES.REFINERY_2:
        return StructureBrain.Categories.FOOD;
      case StructureBrain.TYPES.JANITOR_STATION:
        return StructureBrain.Categories.FOLLOWERS;
      case StructureBrain.TYPES.KITCHEN_II:
        return StructureBrain.Categories.FOOD;
      case StructureBrain.TYPES.FARM_STATION_II:
        return StructureBrain.Categories.FOOD;
      default:
        return StructureBrain.Categories.TECH;
    }
  }

  public static void SetRevealed(StructureBrain.TYPES Type)
  {
    if (DataManager.Instance.RevealedStructures.Contains(Type))
      return;
    DataManager.Instance.RevealedStructures.Add(Type);
  }

  public static bool HasRevealed(StructureBrain.TYPES Type)
  {
    return DataManager.Instance.RevealedStructures.Contains(Type);
  }

  public static StructuresData.Availabilty GetOldAvailability(StructureBrain.TYPES Type)
  {
    StructuresData.Availabilty oldAvailability = !StructuresData.GetUnlocked(Type) ? StructuresData.Availabilty.Locked : StructuresData.Availabilty.Available;
    if (Type == StructureBrain.TYPES.SACRIFICIAL_TEMPLE)
      oldAvailability = StructuresData.Availabilty.Hidden;
    if (StructuresData.GetBuildOnlyOne(Type) && (Structure.CountStructuresOfType(Type) > 0 || BuildSitePlot.StructureOfTypeUnderConstruction(Type) || BuildSitePlotProject.StructureOfTypeUnderConstruction(Type)))
      oldAvailability = StructuresData.Availabilty.Hidden;
    if (Type == StructureBrain.TYPES.FARM_PLOT_SOZO && !DataManager.Instance.SozoQuestComplete)
      oldAvailability = StructuresData.Availabilty.Hidden;
    return oldAvailability;
  }

  public static bool HiddenUntilUnlocked(StructureBrain.TYPES structure)
  {
    return StructuresData.HiddenStructuresUntilUnlocked.Contains(structure);
  }

  private static int GetModifiedGold(int Cost) => Mathf.CeilToInt((float) Cost * 2f);

  public static List<StructuresData.ItemCost> GetCost(StructureBrain.TYPES Type)
  {
    List<StructuresData.ItemCost> itemCostList = new List<StructuresData.ItemCost>();
    List<StructuresData.ItemCost> cost;
    switch (Type)
    {
      case StructureBrain.TYPES.BED:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 5)
        };
        break;
      case StructureBrain.TYPES.FARM_STATION:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 20),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 10)
        };
        break;
      case StructureBrain.TYPES.KITCHEN:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 25),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 5)
        };
        break;
      case StructureBrain.TYPES.SHRINE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 15)
        };
        break;
      case StructureBrain.TYPES.DECORATION_TREE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_STONE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 1)
        };
        break;
      case StructureBrain.TYPES.BED_2:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 3),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 5)
        };
        break;
      case StructureBrain.TYPES.BED_3:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 2),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 1),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.POOP, 3)
        };
        break;
      case StructureBrain.TYPES.FARM_PLOT:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1)
        };
        break;
      case StructureBrain.TYPES.GRAVE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 3),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 10)
        };
        break;
      case StructureBrain.TYPES.FARM_PLOT_SOZO:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.MUSHROOM_SMALL, 5)
        };
        break;
      case StructureBrain.TYPES.BODY_PIT:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 5)
        };
        break;
      case StructureBrain.TYPES.DANCING_FIREPIT:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 30),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 5)
        };
        break;
      case StructureBrain.TYPES.PLANK_PATH:
        cost = new List<StructuresData.ItemCost>();
        break;
      case StructureBrain.TYPES.PRISON:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 25),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 10)
        };
        break;
      case StructureBrain.TYPES.LUMBERJACK_STATION:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 15),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 15)
        };
        break;
      case StructureBrain.TYPES.LUMBERJACK_STATION_2:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 3),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 1),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.CRYSTAL, 5)
        };
        break;
      case StructureBrain.TYPES.COOKING_FIRE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 2)
        };
        break;
      case StructureBrain.TYPES.FOOD_STORAGE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 3),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 10)
        };
        break;
      case StructureBrain.TYPES.FOOD_STORAGE_2:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 20),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 15)
        };
        break;
      case StructureBrain.TYPES.BLOODSTONE_MINE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 15)
        };
        break;
      case StructureBrain.TYPES.BLOODSTONE_MINE_2:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 2),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 1),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.SPIDER_WEB, 10)
        };
        break;
      case StructureBrain.TYPES.CONFESSION_BOOTH:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 3),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 5)
        };
        break;
      case StructureBrain.TYPES.FISHING_HUT:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 20),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FISH, 2)
        };
        break;
      case StructureBrain.TYPES.OUTHOUSE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 10)
        };
        break;
      case StructureBrain.TYPES.COMPOST_BIN:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.SPIDER_WEB, 10)
        };
        break;
      case StructureBrain.TYPES.DECORATION_TORCH:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 2)
        };
      case StructureBrain.TYPES.DECORATION_FLOWER_BOX_1:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FLOWER_RED, 2)
        };
      case StructureBrain.TYPES.DECORATION_SMALL_STONE_CANDLE:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 2)
        };
      case StructureBrain.TYPES.DECORATION_FLAG_CROWN:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 1)
        };
      case StructureBrain.TYPES.DECORATION_FLAG_SCRIPTURE:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 1)
        };
      case StructureBrain.TYPES.DECORATION_WALL_TWIGS:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 1)
        };
      case StructureBrain.TYPES.DECORATION_LAMB_FLAG_STATUE:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 5)
        };
      case StructureBrain.TYPES.TEMPLE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 15),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 5)
        };
        break;
      case StructureBrain.TYPES.SHRINE_II:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 25)
        };
        break;
      case StructureBrain.TYPES.COLLECTED_RESOURCES_CHEST:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 10)
        };
        break;
      case StructureBrain.TYPES.HEALING_BAY:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FLOWER_RED, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 10)
        };
        break;
      case StructureBrain.TYPES.SCARECROW:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 25),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 15)
        };
        break;
      case StructureBrain.TYPES.HARVEST_TOTEM:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 2),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 10)
        };
        break;
      case StructureBrain.TYPES.FARM_PLOT_SIGN:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 2)
        };
      case StructureBrain.TYPES.TEMPLE_II:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 20),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 25)
        };
        break;
      case StructureBrain.TYPES.REFINERY:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 10)
        };
        break;
      case StructureBrain.TYPES.SHRINE_PASSIVE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 10)
        };
        break;
      case StructureBrain.TYPES.SHRINE_III:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 15),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 15),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 30)
        };
        break;
      case StructureBrain.TYPES.SHRINE_IV:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 15)
        };
        break;
      case StructureBrain.TYPES.OFFERING_STATUE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 20),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 15)
        };
        break;
      case StructureBrain.TYPES.SHRINE_PASSIVE_II:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 15)
        };
        break;
      case StructureBrain.TYPES.SHRINE_PASSIVE_III:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 5)
        };
        break;
      case StructureBrain.TYPES.TEMPLE_III:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 10)
        };
        break;
      case StructureBrain.TYPES.TEMPLE_IV:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 20),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 25)
        };
        break;
      case StructureBrain.TYPES.TILE_PATH:
        cost = new List<StructuresData.ItemCost>();
        break;
      case StructureBrain.TYPES.PROPAGANDA_SPEAKER:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 2),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 5)
        };
        break;
      case StructureBrain.TYPES.HEALING_BAY_2:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FLOWER_RED, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 5)
        };
        break;
      case StructureBrain.TYPES.MISSIONARY:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 20),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 20)
        };
        break;
      case StructureBrain.TYPES.SILO_SEED:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 10)
        };
        break;
      case StructureBrain.TYPES.SILO_FERTILISER:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 10)
        };
        break;
      case StructureBrain.TYPES.SURVEILLANCE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 15)
        };
        break;
      case StructureBrain.TYPES.FISHING_HUT_2:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FISH, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 10)
        };
        break;
      case StructureBrain.TYPES.OUTHOUSE_2:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 7),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.CRYSTAL, 10)
        };
        break;
      case StructureBrain.TYPES.SCARECROW_2:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 5)
        };
        break;
      case StructureBrain.TYPES.HARVEST_TOTEM_2:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 5)
        };
        break;
      case StructureBrain.TYPES.REFINERY_2:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 3),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_BARROW:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 3),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FLOWER_RED, 1)
        };
        break;
      case StructureBrain.TYPES.DECORATION_BELL_STATUE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 4)
        };
        break;
      case StructureBrain.TYPES.DECORATION_BONE_ARCH:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 4)
        };
        break;
      case StructureBrain.TYPES.DECORATION_BONE_BARREL:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 1),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 1)
        };
        break;
      case StructureBrain.TYPES.DECORATION_BONE_CANDLE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 1)
        };
        break;
      case StructureBrain.TYPES.DECORATION_BONE_FLAG:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_BONE_LANTERN:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_BONE_PILLAR:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_BONE_SCULPTURE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 5)
        };
        break;
      case StructureBrain.TYPES.DECORATION_CANDLE_BARREL:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 1)
        };
        break;
      case StructureBrain.TYPES.DECORATION_CRYSTAL_LAMP:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.CRYSTAL, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_CRYSTAL_LIGHT:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.CRYSTAL, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_CRYSTAL_ROCK:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.CRYSTAL, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_CRYSTAL_STATUE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 2),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.CRYSTAL, 5)
        };
        break;
      case StructureBrain.TYPES.DECORATION_CRYSTAL_TREE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 3),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.CRYSTAL, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_CRYSTAL_WINDOW:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.CRYSTAL, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_FLOWER_ARCH:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 10)
        };
        break;
      case StructureBrain.TYPES.DECORATION_FOUNTAIN:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 5)
        };
        break;
      case StructureBrain.TYPES.DECORATION_POST_BOX:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 1)
        };
        break;
      case StructureBrain.TYPES.DECORATION_PUMPKIN_PILE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.PUMPKIN, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_PUMPKIN_STOOL:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 1),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.PUMPKIN, 1)
        };
        break;
      case StructureBrain.TYPES.DECORATION_STONE_CANDLE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 1)
        };
        break;
      case StructureBrain.TYPES.DECORATION_STONE_FLAG:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_STONE_MUSHROOM:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_TORCH_BIG:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 2),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_TWIG_LAMP:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_WALL_BONE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 1)
        };
        break;
      case StructureBrain.TYPES.DECORATION_WALL_STONE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 1)
        };
        break;
      case StructureBrain.TYPES.DECORATION_WALL_GRASS:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 1)
        };
        break;
      case StructureBrain.TYPES.DECORATION_WREATH_STICK:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 4)
        };
        break;
      case StructureBrain.TYPES.DECORATION_STUMP_LAMB_STATUE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 3)
        };
        break;
      case StructureBrain.TYPES.CHOPPING_SHRINE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 25),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 5)
        };
        break;
      case StructureBrain.TYPES.MINING_SHRINE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 5)
        };
        break;
      case StructureBrain.TYPES.FORAGING_SHRINE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 5)
        };
        break;
      case StructureBrain.TYPES.JANITOR_STATION:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_BELL_SMALL:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_BONE_SKULL_BIG:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 5)
        };
        break;
      case StructureBrain.TYPES.DECORATION_BONE_SKULL_PILE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 5)
        };
        break;
      case StructureBrain.TYPES.DECORATION_FLAG_CRYSTAL:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.CRYSTAL, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_FLAG_MUSHROOM:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 2),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.MUSHROOM_SMALL, 1)
        };
        break;
      case StructureBrain.TYPES.DECORATION_FLOWER_BOTTLE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FLOWER_RED, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_FLOWER_CART:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 3),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FLOWER_RED, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_HAY_BALE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_HAY_PILE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_LEAFY_FLOWER_SCULPTURE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 5)
        };
        break;
      case StructureBrain.TYPES.DECORATION_LEAFY_LANTERN:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 5)
        };
        break;
      case StructureBrain.TYPES.DECORATION_LEAFY_SCULPTURE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 5)
        };
        break;
      case StructureBrain.TYPES.DECORATION_MUSHROOM_1:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.MUSHROOM_SMALL, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_MUSHROOM_2:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.MUSHROOM_SMALL, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_MUSHROOM_CANDLE_1:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.MUSHROOM_SMALL, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_MUSHROOM_CANDLE_2:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.MUSHROOM_SMALL, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_MUSHROOM_CANDLE_LARGE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.MUSHROOM_SMALL, 5)
        };
        break;
      case StructureBrain.TYPES.DECORATION_MUSHROOM_SCULPTURE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.MUSHROOM_SMALL, 35),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 10)
        };
        break;
      case StructureBrain.TYPES.DECORATION_SPIDER_LANTERN:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.SPIDER_WEB, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_SPIDER_PILLAR:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.SPIDER_WEB, 8)
        };
        break;
      case StructureBrain.TYPES.DECORATION_SPIDER_SCULPTURE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.SPIDER_WEB, 10)
        };
        break;
      case StructureBrain.TYPES.DECORATION_SPIDER_TORCH:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.SPIDER_WEB, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_SPIDER_WEB_CROWN_SCULPTURE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.SPIDER_WEB, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_STONE_CANDLE_LAMP:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_STONE_HENGE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_WALL_SPIDER:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.SPIDER_WEB, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_POND:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 2)
        };
        break;
      case StructureBrain.TYPES.DEMON_SUMMONER:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 10)
        };
        break;
      case StructureBrain.TYPES.COMPOST_BIN_DEAD_BODY:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 3)
        };
        break;
      case StructureBrain.TYPES.DEMON_SUMMONER_2:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 1),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 3),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 2)
        };
        break;
      case StructureBrain.TYPES.DEMON_SUMMONER_3:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 3),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.SPIDER_WEB, 15)
        };
        break;
      case StructureBrain.TYPES.MISSIONARY_II:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 40)
        };
        break;
      case StructureBrain.TYPES.MISSIONARY_III:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 20),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.CRYSTAL, 20)
        };
        break;
      case StructureBrain.TYPES.KITCHEN_II:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 25),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.CRYSTAL, 10)
        };
        break;
      case StructureBrain.TYPES.FARM_STATION_II:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 5)
        };
        break;
      case StructureBrain.TYPES.DECORATION_MONSTERSHRINE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 5)
        };
        break;
      case StructureBrain.TYPES.DECORATION_FLOWERPOTWALL:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 2),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FLOWER_RED, 1)
        };
        break;
      case StructureBrain.TYPES.DECORATION_LEAFYLAMPPOST:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 2),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_FLOWERVASE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FLOWER_RED, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_WATERINGCAN:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 1),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FLOWER_RED, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_FLOWER_CART_SMALL:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 1),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FLOWER_RED, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_WEEPINGSHRINE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_1:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FLOWER_RED, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 3),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_2:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.MUSHROOM_SMALL, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_3:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.CRYSTAL, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_4:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.SPIDER_WEB, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_5:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 10)
        };
        break;
      case StructureBrain.TYPES.DECORATION_VIDEO:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 10)
        };
        break;
      case StructureBrain.TYPES.DECORATION_PLUSH:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 20)
        };
        break;
      case StructureBrain.TYPES.DECORATION_TWITCH_FLAG_CROWN:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_TWITCH_MUSHROOM_BAG:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.MUSHROOM_SMALL, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_TWITCH_ROSE_BUSH:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 5)
        };
        break;
      case StructureBrain.TYPES.DECORATION_TWITCH_STONE_FLAG:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_TWITCH_STONE_STATUE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_TWITCH_WOODEN_GUARDIAN:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_HALLOWEEN_PUMPKIN:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.PUMPKIN, 3),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_HALLOWEEN_SKULL:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 2),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FLOWER_RED, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_HALLOWEEN_CANDLE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 3),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 1)
        };
        break;
      case StructureBrain.TYPES.DECORATION_HALLOWEEN_TREE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 10)
        };
        break;
      default:
        return new List<StructuresData.ItemCost>();
    }
    if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Economy_MineII))
    {
      foreach (StructuresData.ItemCost itemCost in cost)
        itemCost.CostValue = Mathf.CeilToInt((float) itemCost.CostValue * UpgradeSystem.GetPriceModifier);
    }
    foreach (StructuresData.ItemCost itemCost in cost)
    {
      if (itemCost.CostItem == InventoryItem.ITEM_TYPE.BLACK_GOLD || itemCost.CostItem == InventoryItem.ITEM_TYPE.GOLD_REFINED)
        itemCost.CostValue = StructuresData.GetModifiedGold(itemCost.CostValue);
    }
    return cost;
  }

  public static string GetCostText(StructureBrain.TYPES type, bool includeCurrent)
  {
    string costText = "";
    List<StructuresData.ItemCost> cost = StructuresData.GetCost(type);
    for (int index = 0; index < cost.Count; ++index)
    {
      int itemQuantity = global::Inventory.GetItemQuantity((int) cost[index].CostItem);
      int costValue = cost[index].CostValue;
      if (includeCurrent)
        costText += costValue > itemQuantity ? "<color=#ff0000>" : "<color=#00ff00>";
      string str = $"{costText + FontImageNames.GetIconByType(cost[index].CostItem)} x{costValue.ToString()}";
      if (includeCurrent)
        str = $"{str}   ({itemQuantity.ToString()})</color>";
      costText = str + "\n";
    }
    return costText;
  }

  public static bool CanAfford(StructureBrain.TYPES type)
  {
    List<StructuresData.ItemCost> cost = StructuresData.GetCost(type);
    for (int index = 0; index < cost.Count; ++index)
    {
      if (global::Inventory.GetItemQuantity((int) cost[index].CostItem) < cost[index].CostValue)
        return false;
    }
    return true;
  }

  public static bool CreateBuildSite(StructureBrain.TYPES Type)
  {
    switch (Type)
    {
      case StructureBrain.TYPES.RUBBLE:
      case StructureBrain.TYPES.WEEDS:
      case StructureBrain.TYPES.RUBBLE_BIG:
        return false;
      default:
        return true;
    }
  }

  public static bool CanBeFlipped(StructureBrain.TYPES type)
  {
    switch (type)
    {
      case StructureBrain.TYPES.SHRINE:
      case StructureBrain.TYPES.REFINERY:
      case StructureBrain.TYPES.SHRINE_PASSIVE:
      case StructureBrain.TYPES.SHRINE_PASSIVE_II:
      case StructureBrain.TYPES.SHRINE_PASSIVE_III:
      case StructureBrain.TYPES.REFINERY_2:
        return false;
      default:
        return true;
    }
  }

  [Serializable]
  public struct PathData
  {
    public Vector2Int TilePosition;
    public Vector3 WorldPosition;
    public int PathID;
  }

  public enum Phase
  {
    Hidden,
    Available,
    Built,
  }

  public enum ResearchState
  {
    Unresearched,
    Researching,
    Researched,
  }

  [Serializable]
  public class ResearchObject
  {
    public StructureBrain.TYPES Type;
    public float Progress;

    public ResearchObject()
    {
    }

    public ResearchObject(StructureBrain.TYPES Type) => this.Type = Type;

    public float TargetProgress => this.Type == StructureBrain.TYPES.FARM_PLOT ? 480f : 240f;

    public static float GetResearchTimeInDays(StructureBrain.TYPES Type)
    {
      return new StructuresData.ResearchObject(Type).TargetProgress / 480f;
    }
  }

  public class ItemCost : IComparable<StructuresData.ItemCost>
  {
    public InventoryItem.ITEM_TYPE CostItem;
    public int CostValue;

    public ItemCost(InventoryItem.ITEM_TYPE CostItem, int CostValue)
    {
      this.CostItem = CostItem;
      this.CostValue = CostValue;
    }

    public int CompareTo(StructuresData.ItemCost other)
    {
      return other == null || this.CostItem != InventoryItem.ITEM_TYPE.FOLLOWERS ? 1 : 0;
    }

    public override string ToString() => CostFormatter.FormatCost(this, false);

    public string ToStringShowQuantity() => CostFormatter.FormatCost(this);

    public static string GetCostString(params StructuresData.ItemCost[] itemCosts)
    {
      return CostFormatter.FormatCosts(itemCosts, false);
    }

    public static string GetCostStringWithQuantity(params StructuresData.ItemCost[] itemCosts)
    {
      return CostFormatter.FormatCosts(itemCosts);
    }

    public static string GetCostStringWithQuantity(List<StructuresData.ItemCost> itemCosts)
    {
      return StructuresData.ItemCost.GetCostStringWithQuantity(itemCosts.ToArray());
    }

    public static string GetCostString(List<StructuresData.ItemCost> itemCosts)
    {
      return StructuresData.ItemCost.GetCostString(itemCosts.ToArray());
    }

    public bool CanAfford() => global::Inventory.GetItemQuantity((int) this.CostItem) >= this.CostValue;
  }

  public enum Availabilty
  {
    Available,
    Locked,
    Hidden,
  }
}
