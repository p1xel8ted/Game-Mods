// Decompiled with JetBrains decompiler
// Type: StructureBrain
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

#nullable disable
public class StructureBrain
{
  public static List<StructureBrain.TYPES> DecorationsToAdmire = new List<StructureBrain.TYPES>()
  {
    StructureBrain.TYPES.DECORATION_LAMB_STATUE,
    StructureBrain.TYPES.DECORATION_FOUNTAIN,
    StructureBrain.TYPES.DECORATION_CRYSTAL_TREE,
    StructureBrain.TYPES.DECORATION_CRYSTAL_WINDOW,
    StructureBrain.TYPES.DECORATION_FLOWER_ARCH,
    StructureBrain.TYPES.DECORATION_CRYSTAL_ROCK,
    StructureBrain.TYPES.DECORATION_FLAG_SCRIPTURE,
    StructureBrain.TYPES.DECORATION_LAMB_FLAG_STATUE,
    StructureBrain.TYPES.DECORATION_BELL_STATUE,
    StructureBrain.TYPES.DECORATION_BONE_ARCH,
    StructureBrain.TYPES.DECORATION_BONE_CANDLE,
    StructureBrain.TYPES.DECORATION_BONE_FLAG,
    StructureBrain.TYPES.DECORATION_BONE_SCULPTURE,
    StructureBrain.TYPES.DECORATION_BONE_LANTERN,
    StructureBrain.TYPES.DECORATION_CRYSTAL_STATUE,
    StructureBrain.TYPES.DECORATION_FLOWER_ARCH,
    StructureBrain.TYPES.DECORATION_FOUNTAIN,
    StructureBrain.TYPES.DECORATION_BONE_SKULL_BIG,
    StructureBrain.TYPES.DECORATION_BONE_SKULL_PILE,
    StructureBrain.TYPES.DECORATION_LEAFY_FLOWER_SCULPTURE,
    StructureBrain.TYPES.DECORATION_LEAFY_SCULPTURE,
    StructureBrain.TYPES.DECORATION_MUSHROOM_CANDLE_LARGE,
    StructureBrain.TYPES.DECORATION_MUSHROOM_SCULPTURE,
    StructureBrain.TYPES.DECORATION_SPIDER_PILLAR,
    StructureBrain.TYPES.DECORATION_SPIDER_SCULPTURE,
    StructureBrain.TYPES.DECORATION_SPIDER_WEB_CROWN_SCULPTURE,
    StructureBrain.TYPES.DECORATION_STONE_HENGE,
    StructureBrain.TYPES.DECORATION_POND,
    StructureBrain.TYPES.DECORATION_MONSTERSHRINE,
    StructureBrain.TYPES.DECORATION_WEEPINGSHRINE,
    StructureBrain.TYPES.DECORATION_BOSS_TROPHY_1,
    StructureBrain.TYPES.DECORATION_BOSS_TROPHY_2,
    StructureBrain.TYPES.DECORATION_BOSS_TROPHY_3,
    StructureBrain.TYPES.DECORATION_BOSS_TROPHY_4,
    StructureBrain.TYPES.DECORATION_BOSS_TROPHY_5,
    StructureBrain.TYPES.DECORATION_PLUSH,
    StructureBrain.TYPES.DECORATION_TWITCH_STONE_STATUE,
    StructureBrain.TYPES.DECORATION_TWITCH_WOODEN_GUARDIAN,
    StructureBrain.TYPES.DECORATION_HALLOWEEN_TREE,
    StructureBrain.TYPES.DECORATION_HALLOWEEN_CANDLE
  };
  public StructuresData Data;
  public bool ReservedForTask;
  public bool ReservedByPlayer;
  public bool ForceRemoved;
  public Action<float> OnFuelModified;
  public System.Action OnItemDeposited;
  private static Dictionary<int, StructureBrain> _brainsByID = new Dictionary<int, StructureBrain>();

  public virtual void OnAdded()
  {
  }

  public virtual void OnRemoved()
  {
  }

  public virtual bool IsFull => false;

  public virtual void Init(StructuresData data) => this.Data = data;

  public void UpdateFuel(int amountToRemove = 5)
  {
    if (this.Data.Fuel <= 0 || this.Data.onlyDepleteWhenFullyFueled && (!this.Data.onlyDepleteWhenFullyFueled || !this.Data.FullyFueled))
      return;
    this.Data.Fuel = Mathf.Clamp(this.Data.Fuel - amountToRemove, 0, this.Data.MaxFuel);
    if (this.Data.Fuel <= 0)
      this.Data.FullyFueled = false;
    Action<float> onFuelModified = this.OnFuelModified;
    if (onFuelModified == null)
      return;
    onFuelModified((float) this.Data.Fuel / (float) this.Data.MaxFuel);
  }

  public void Remove()
  {
    this.RemoveFromGrid();
    StructureManager.RemoveStructure(this);
  }

  public void RemoveFromGrid(Vector2Int gridTilePosition)
  {
    Structures_PlacementRegion placementRegion = this.FindPlacementRegion();
    if (placementRegion != null)
    {
      placementRegion.ClearStructureFromGrid(this);
    }
    else
    {
      if (!((UnityEngine.Object) PlacementRegion.Instance != (UnityEngine.Object) null) || PlacementRegion.Instance.structureBrain == null)
        return;
      PlacementRegion.Instance.structureBrain.ClearStructureFromGrid(this, gridTilePosition);
    }
  }

  public void RemoveFromGrid() => this.RemoveFromGrid(this.Data.GridTilePosition);

  public void AddToGrid(Vector2Int gridTilePosition)
  {
    Structures_PlacementRegion placementRegion = this.FindPlacementRegion();
    if (placementRegion != null)
    {
      placementRegion.AddStructureToGrid(this.Data);
    }
    else
    {
      if (!((UnityEngine.Object) PlacementRegion.Instance != (UnityEngine.Object) null))
        return;
      PlacementRegion.Instance.structureBrain.AddStructureToGrid(this.Data, gridTilePosition);
    }
  }

  public void AddToGrid() => this.AddToGrid(this.Data.GridTilePosition);

  public Structures_PlacementRegion FindPlacementRegion()
  {
    Structures_PlacementRegion placementRegion = (Structures_PlacementRegion) null;
    foreach (StructureBrain structureBrain in StructureManager.StructuresAtLocation(this.Data.Location))
    {
      if (structureBrain.Data.Type == StructureBrain.TYPES.PLACEMENT_REGION && this.Data.PlacementRegionPosition == new Vector3Int((int) structureBrain.Data.Position.x, (int) structureBrain.Data.Position.y, 0))
      {
        placementRegion = structureBrain as Structures_PlacementRegion;
        break;
      }
    }
    return placementRegion;
  }

  public static Structures_PlacementRegion FindPlacementRegion(StructuresData Data)
  {
    Structures_PlacementRegion placementRegion = (Structures_PlacementRegion) null;
    foreach (StructureBrain structureBrain in StructureManager.StructuresAtLocation(Data.Location))
    {
      if (structureBrain.Data.Type == StructureBrain.TYPES.PLACEMENT_REGION && Data.PlacementRegionPosition == new Vector3Int((int) structureBrain.Data.Position.x, (int) structureBrain.Data.Position.y, 0))
      {
        placementRegion = structureBrain as Structures_PlacementRegion;
        break;
      }
    }
    return placementRegion;
  }

  public void DepositItem(InventoryItem.ITEM_TYPE type, int quantity = 1)
  {
    InventoryItem inventoryItem1 = (InventoryItem) null;
    foreach (InventoryItem inventoryItem2 in this.Data.Inventory)
    {
      if ((InventoryItem.ITEM_TYPE) inventoryItem2.type == type)
      {
        inventoryItem1 = inventoryItem2;
        break;
      }
    }
    if (inventoryItem1 == null)
      this.Data.Inventory.Add(new InventoryItem(type, quantity));
    else
      inventoryItem1.quantity += quantity;
    System.Action onItemDeposited = this.OnItemDeposited;
    if (onItemDeposited == null)
      return;
    onItemDeposited();
  }

  public void DepositItemUnstacked(InventoryItem.ITEM_TYPE type)
  {
    this.Data.Inventory.Add(new InventoryItem(type));
    System.Action onItemDeposited = this.OnItemDeposited;
    if (onItemDeposited == null)
      return;
    onItemDeposited();
  }

  public virtual void ToDebugString(StringBuilder sb)
  {
    sb.AppendLine($"{this.Data.Location}; ({this.Data.GridX},{this.Data.GridY}); {this.Data.Position}");
  }

  public static IEnumerable<StructureBrain> AllBrains
  {
    get => (IEnumerable<StructureBrain>) StructureBrain._brainsByID.Values;
  }

  public static StructureBrain CreateBrain(StructuresData data)
  {
    StructureBrain brain;
    switch (data.Type)
    {
      case StructureBrain.TYPES.BED:
        brain = (StructureBrain) new Structures_Bed();
        break;
      case StructureBrain.TYPES.FARM_STATION:
        brain = (StructureBrain) new Structures_FarmerStation();
        break;
      case StructureBrain.TYPES.KITCHEN:
        brain = (StructureBrain) new Structures_Kitchen();
        break;
      case StructureBrain.TYPES.TREE:
        brain = (StructureBrain) new Structures_Tree();
        break;
      case StructureBrain.TYPES.SHRINE:
      case StructureBrain.TYPES.SHRINE_FUNDAMENTALIST:
      case StructureBrain.TYPES.SHRINE_UTOPIANIST:
      case StructureBrain.TYPES.OUTPOST_SHRINE:
      case StructureBrain.TYPES.SHRINE_II:
      case StructureBrain.TYPES.SHRINE_III:
      case StructureBrain.TYPES.SHRINE_IV:
        brain = (StructureBrain) new Structures_Shrine();
        break;
      case StructureBrain.TYPES.BUILD_SITE:
        brain = (StructureBrain) new Structures_BuildSite();
        break;
      case StructureBrain.TYPES.ALTAR:
        brain = (StructureBrain) new Structures_Altar();
        break;
      case StructureBrain.TYPES.PLACEMENT_REGION:
        brain = (StructureBrain) new Structures_PlacementRegion();
        break;
      case StructureBrain.TYPES.BED_2:
        brain = (StructureBrain) new Structures_Bed2();
        break;
      case StructureBrain.TYPES.BED_3:
        brain = (StructureBrain) new Structures_Bed3();
        break;
      case StructureBrain.TYPES.SHRINE_MISFIT:
        brain = (StructureBrain) new Structures_Shrine_Misfit();
        break;
      case StructureBrain.TYPES.FARM_PLOT:
        brain = (StructureBrain) new Structures_FarmerPlot();
        break;
      case StructureBrain.TYPES.GRAVE:
      case StructureBrain.TYPES.BODY_PIT:
      case StructureBrain.TYPES.GRAVE2:
        brain = (StructureBrain) new Structures_Grave();
        break;
      case StructureBrain.TYPES.DEAD_WORSHIPPER:
        brain = (StructureBrain) new Structures_DeadWorshipper();
        break;
      case StructureBrain.TYPES.MEAL:
      case StructureBrain.TYPES.MEAL_MEAT:
      case StructureBrain.TYPES.MEAL_GREAT:
      case StructureBrain.TYPES.MEAL_GRASS:
      case StructureBrain.TYPES.MEAL_GOOD_FISH:
      case StructureBrain.TYPES.MEAL_FOLLOWER_MEAT:
      case StructureBrain.TYPES.MEAL_MUSHROOMS:
      case StructureBrain.TYPES.MEAL_POOP:
      case StructureBrain.TYPES.MEAL_GREAT_FISH:
      case StructureBrain.TYPES.MEAL_BAD_FISH:
      case StructureBrain.TYPES.MEAL_BERRIES:
      case StructureBrain.TYPES.MEAL_MEDIUM_VEG:
      case StructureBrain.TYPES.MEAL_BAD_MIXED:
      case StructureBrain.TYPES.MEAL_MEDIUM_MIXED:
      case StructureBrain.TYPES.MEAL_GREAT_MIXED:
      case StructureBrain.TYPES.MEAL_DEADLY:
      case StructureBrain.TYPES.MEAL_BAD_MEAT:
      case StructureBrain.TYPES.MEAL_GREAT_MEAT:
      case StructureBrain.TYPES.MEAL_BURNED:
        brain = (StructureBrain) new Structures_Meal();
        break;
      case StructureBrain.TYPES.DANCING_FIREPIT:
        brain = (StructureBrain) new Structures_DancingFirePit();
        break;
      case StructureBrain.TYPES.PRISON:
        brain = (StructureBrain) new Structures_Prison();
        break;
      case StructureBrain.TYPES.RUBBLE:
      case StructureBrain.TYPES.BLOOD_STONE:
      case StructureBrain.TYPES.GOLD_ORE:
        brain = (StructureBrain) new Structures_Rubble(0);
        break;
      case StructureBrain.TYPES.WEEDS:
        brain = (StructureBrain) new Structures_Weeds();
        break;
      case StructureBrain.TYPES.LUMBERJACK_STATION:
      case StructureBrain.TYPES.LUMBERJACK_STATION_2:
        brain = (StructureBrain) new Structures_LumberjackStation();
        break;
      case StructureBrain.TYPES.RESEARCH_1:
        brain = (StructureBrain) new Structures_Research1();
        break;
      case StructureBrain.TYPES.RESEARCH_2:
        brain = (StructureBrain) new Structures_Research2();
        break;
      case StructureBrain.TYPES.COOKING_FIRE:
        brain = (StructureBrain) new Structures_CookingFire();
        break;
      case StructureBrain.TYPES.FOOD_STORAGE:
        brain = (StructureBrain) new Structures_FoodStorage(0);
        break;
      case StructureBrain.TYPES.FOOD_STORAGE_2:
        brain = (StructureBrain) new Structures_FoodStorage(1);
        break;
      case StructureBrain.TYPES.BLOODSTONE_MINE:
      case StructureBrain.TYPES.BLOODSTONE_MINE_2:
        brain = (StructureBrain) new Structures_MinerStation();
        break;
      case StructureBrain.TYPES.FISHING_HUT:
        brain = (StructureBrain) new Structures_FishingHut();
        break;
      case StructureBrain.TYPES.BERRY_BUSH:
        brain = (StructureBrain) new Structures_BerryBush();
        break;
      case StructureBrain.TYPES.SLEEPING_BAG:
        brain = (StructureBrain) new Structures_SleepingBag();
        break;
      case StructureBrain.TYPES.OUTHOUSE:
        brain = (StructureBrain) new Structures_Outhouse();
        break;
      case StructureBrain.TYPES.LUMBER_MINE:
        brain = (StructureBrain) new Structures_LumberMine();
        break;
      case StructureBrain.TYPES.COMPOST_BIN:
        brain = (StructureBrain) new Structures_CompostBin();
        break;
      case StructureBrain.TYPES.PUMPKIN_BUSH:
        brain = (StructureBrain) new Structures_BerryBush();
        break;
      case StructureBrain.TYPES.TEMPLE:
      case StructureBrain.TYPES.TEMPLE_II:
      case StructureBrain.TYPES.TEMPLE_III:
      case StructureBrain.TYPES.TEMPLE_IV:
        brain = (StructureBrain) new Structures_Temple();
        break;
      case StructureBrain.TYPES.BUILDSITE_BUILDINGPROJECT:
        brain = (StructureBrain) new Structures_BuildSiteProject();
        break;
      case StructureBrain.TYPES.COLLECTED_RESOURCES_CHEST:
        brain = (StructureBrain) new Structures_CollectedResourceChest();
        break;
      case StructureBrain.TYPES.HEALING_BAY:
        brain = (StructureBrain) new Structures_HealingBay(0);
        break;
      case StructureBrain.TYPES.SCARECROW:
      case StructureBrain.TYPES.SCARECROW_2:
        brain = (StructureBrain) new Structures_Scarecrow();
        break;
      case StructureBrain.TYPES.HARVEST_TOTEM:
      case StructureBrain.TYPES.HARVEST_TOTEM_2:
        brain = (StructureBrain) new Structures_HarvestTotem();
        break;
      case StructureBrain.TYPES.MUSHROOM_BUSH:
        brain = (StructureBrain) new Structures_BerryBush();
        break;
      case StructureBrain.TYPES.RED_FLOWER_BUSH:
        brain = (StructureBrain) new Structures_BerryBush();
        break;
      case StructureBrain.TYPES.WHITE_FLOWER_BUSH:
        brain = (StructureBrain) new Structures_BerryBush();
        break;
      case StructureBrain.TYPES.MISSION_SHRINE:
        brain = (StructureBrain) new Structures_MissionShrine();
        break;
      case StructureBrain.TYPES.REFINERY:
      case StructureBrain.TYPES.REFINERY_2:
        brain = (StructureBrain) new Structures_Refinery();
        break;
      case StructureBrain.TYPES.SHRINE_PASSIVE:
      case StructureBrain.TYPES.SHRINE_PASSIVE_II:
      case StructureBrain.TYPES.SHRINE_PASSIVE_III:
        brain = (StructureBrain) new Structures_Shrine_Passive();
        break;
      case StructureBrain.TYPES.OFFERING_STATUE:
        brain = (StructureBrain) new Structures_OfferingShrine();
        break;
      case StructureBrain.TYPES.RUBBLE_BIG:
        brain = (StructureBrain) new Structures_Rubble(1);
        break;
      case StructureBrain.TYPES.WATER_SMALL:
      case StructureBrain.TYPES.WATER_MEDIUM:
      case StructureBrain.TYPES.WATER_BIG:
        brain = (StructureBrain) new Structures_Water();
        break;
      case StructureBrain.TYPES.RATAU_SHRINE:
        brain = (StructureBrain) new Structures_Shrine_Ratau();
        break;
      case StructureBrain.TYPES.PROPAGANDA_SPEAKER:
        brain = (StructureBrain) new Structures_PropagandaSpeaker();
        break;
      case StructureBrain.TYPES.HEALING_BAY_2:
        brain = (StructureBrain) new Structures_HealingBay(1);
        break;
      case StructureBrain.TYPES.MISSIONARY:
        brain = (StructureBrain) new Structures_Missionary();
        break;
      case StructureBrain.TYPES.FEAST_TABLE:
        brain = (StructureBrain) new Structures_FeastTable();
        break;
      case StructureBrain.TYPES.SILO_SEED:
        brain = (StructureBrain) new Structures_SiloSeed();
        break;
      case StructureBrain.TYPES.SILO_FERTILISER:
        brain = (StructureBrain) new Structures_SiloFertiliser();
        break;
      case StructureBrain.TYPES.SURVEILLANCE:
        brain = (StructureBrain) new Structures_Surveillance();
        break;
      case StructureBrain.TYPES.FISHING_HUT_2:
        brain = (StructureBrain) new Structures_FishingHut2();
        break;
      case StructureBrain.TYPES.OUTHOUSE_2:
        brain = (StructureBrain) new Structures_Outhouse();
        break;
      case StructureBrain.TYPES.CHOPPING_SHRINE:
        brain = (StructureBrain) new Structures_ChoppingShrine();
        break;
      case StructureBrain.TYPES.MINING_SHRINE:
        brain = (StructureBrain) new Structures_MiningShrine();
        break;
      case StructureBrain.TYPES.FORAGING_SHRINE:
        brain = (StructureBrain) new Structures_ForagingShrine();
        break;
      case StructureBrain.TYPES.JANITOR_STATION:
        brain = (StructureBrain) new Structures_JanitorStation();
        break;
      case StructureBrain.TYPES.DEMON_SUMMONER:
        brain = (StructureBrain) new Structures_Demon_Summoner();
        break;
      case StructureBrain.TYPES.COMPOST_BIN_DEAD_BODY:
        brain = (StructureBrain) new Structures_DeadBodyCompost();
        break;
      case StructureBrain.TYPES.BEETROOT_BUSH:
        brain = (StructureBrain) new Structures_BerryBush();
        break;
      case StructureBrain.TYPES.CAULIFLOWER_BUSH:
        brain = (StructureBrain) new Structures_BerryBush();
        break;
      case StructureBrain.TYPES.DEMON_SUMMONER_2:
        brain = (StructureBrain) new Structures_Demon_Summoner();
        break;
      case StructureBrain.TYPES.DEMON_SUMMONER_3:
        brain = (StructureBrain) new Structures_Demon_Summoner();
        break;
      case StructureBrain.TYPES.FISHING_SPOT:
        brain = (StructureBrain) new Structures_FishingSpot();
        break;
      case StructureBrain.TYPES.MISSIONARY_II:
        brain = (StructureBrain) new Structures_Missionary();
        break;
      case StructureBrain.TYPES.MISSIONARY_III:
        brain = (StructureBrain) new Structures_Missionary();
        break;
      case StructureBrain.TYPES.KITCHEN_II:
        brain = (StructureBrain) new Structures_Kitchen();
        break;
      case StructureBrain.TYPES.FARM_STATION_II:
        brain = (StructureBrain) new Structures_FarmerStation();
        break;
      default:
        brain = new StructureBrain();
        break;
    }
    StructureBrain.ApplyConfigToData(data);
    brain.Init(data);
    StructureBrain._brainsByID.Add(data.ID, brain);
    StructureManager.StructuresAtLocation(data.Location).Add(brain);
    return brain;
  }

  public static void ApplyConfigToData(StructuresData data)
  {
    StructuresData infoByType = StructuresData.GetInfoByType(data.Type, data.VariantIndex);
    data.PrefabPath = infoByType.PrefabPath;
    data.RemoveOnDie = infoByType.RemoveOnDie;
    data.ProgressTarget = infoByType.ProgressTarget;
    data.WorkIsRequiredForProgress = infoByType.WorkIsRequiredForProgress;
    data.IsUpgrade = infoByType.IsUpgrade;
    data.UpgradeFromType = infoByType.UpgradeFromType;
    data.RequiresType = infoByType.RequiresType;
    data.TILE_WIDTH = infoByType.TILE_WIDTH;
    data.TILE_HEIGHT = infoByType.TILE_HEIGHT;
    data.CanBeMoved = infoByType.CanBeMoved;
    data.CanBeRecycled = infoByType.CanBeRecycled;
    data.IsObstruction = infoByType.IsObstruction;
    data.DoesNotOccupyGrid = infoByType.DoesNotOccupyGrid;
    data.LootToDrop = infoByType.LootToDrop;
    data.LootCountToDrop = infoByType.LootCountToDrop;
    data.IsUpgrade = infoByType.IsUpgrade;
    data.IsUpgradeDestroyPrevious = infoByType.IsUpgradeDestroyPrevious;
    data.IgnoreGrid = infoByType.IgnoreGrid;
    data.IsBuildingProject = infoByType.IsBuildingProject;
  }

  public static void RemoveBrain(StructureBrain brain)
  {
    StructureBrain._brainsByID.Remove(brain.Data.ID);
    brain.OnRemoved();
  }

  public static StructureBrain FindBrainByID(int ID)
  {
    StructureBrain brainById = (StructureBrain) null;
    StructureBrain._brainsByID.TryGetValue(ID, out brainById);
    return brainById;
  }

  public static StructureBrain GetOrCreateBrain(StructuresData data)
  {
    StructureBrain brain = StructureBrain.FindBrainByID(data.ID);
    if (brain == null)
    {
      brain = StructureBrain.CreateBrain(data);
      brain.OnAdded();
    }
    return brain;
  }

  public static bool IsPath(StructureBrain.TYPES type)
  {
    return type == StructureBrain.TYPES.PLANK_PATH || type == StructureBrain.TYPES.TILE_PATH || type == StructureBrain.TYPES.TILE_HAY || type == StructureBrain.TYPES.TILE_BLOOD || type == StructureBrain.TYPES.TILE_ROCKS || type == StructureBrain.TYPES.TILE_WATER || type == StructureBrain.TYPES.TILE_BRICKS || type == StructureBrain.TYPES.TILE_PLANKS || type == StructureBrain.TYPES.TILE_FLOWERS || type == StructureBrain.TYPES.TILE_REDGRASS || type == StructureBrain.TYPES.TILE_SPOOKYPLANKS || type == StructureBrain.TYPES.TILE_GOLD || type == StructureBrain.TYPES.TILE_MOSAIC || type == StructureBrain.TYPES.TILE_FLOWERSROCKY;
  }

  public enum Categories
  {
    FOLLOWERS,
    FOOD,
    FAITH,
    DUNGEON,
    RESOURCES,
    TECH,
    AESTHETIC,
    SECURITY,
    WORK,
    LEISURE,
    SLEEP,
    WORSHIP,
    STUDY,
    ECONOMY,
    CULT,
    COMBAT,
  }

  [Serializable]
  public enum TYPES
  {
    NONE,
    BUILDER,
    BED,
    PORTAL,
    SACRIFICIAL_TEMPLE,
    WOOD_STORE,
    BLACKSMITH,
    TAVERN,
    FARM_STATION,
    WHEAT_SILO,
    KITCHEN,
    COOKED_FOOD_SILO,
    CROP,
    NIGHTMARE_MACHINE,
    MONSTER_HIVE,
    DEFENCE_TOWER,
    TREE,
    BUSH,
    GRASS,
    FIRE,
    ROCK,
    FOLLOWER_RECRUIT,
    SEED_FLOWER,
    COTTON_PLANT,
    HEALING_BATH,
    FIRE_PIT,
    BUILD_PLOT,
    SHRINE,
    BARRACKS,
    ASTROLOGIST,
    STORAGE_PIT,
    BUILD_SITE,
    ALTAR,
    PLACEMENT_REGION,
    DECORATION_TREE,
    DECORATION_STONE,
    REPAIRABLE_HEARTS,
    REPAIRABLE_ASTROLOGY,
    REPAIRABLE_VOODOO,
    REPAIRABLE_CURSES,
    BED_2,
    BED_3,
    SHRINE_FUNDAMENTALIST,
    SHRINE_MISFIT,
    SHRINE_UTOPIANIST,
    FARM_PLOT,
    GRAVE,
    DEAD_WORSHIPPER,
    VOMIT,
    DEMOLISH_STRUCTURE,
    MOVE_STRUCTURE,
    FARM_PLOT_SOZO,
    MEAL,
    BODY_PIT,
    TAROT_BUILDING,
    CULT_UPGRADE1,
    DANCING_FIREPIT,
    CULT_UPGRADE2,
    PLANK_PATH,
    PRISON,
    GRAVE2,
    RUBBLE,
    WEEDS,
    LUMBERJACK_STATION,
    LUMBERJACK_STATION_2,
    RESEARCH_1,
    RESEARCH_2,
    ONE,
    TWO,
    THREE,
    SACRIFICIAL_TEMPLE_2,
    COOKING_FIRE,
    ALCHEMY_CAULDRON,
    FOOD_STORAGE,
    FOOD_STORAGE_2,
    MATING_TENT,
    BLOODSTONE_MINE,
    BLOODSTONE_MINE_2,
    CONFESSION_BOOTH,
    DRUM_CIRCLE,
    ENEMY_TRAP,
    FISHING_HUT,
    GHOST_CIRCLE,
    HIPPY_TENT,
    HUNTERS_HUT,
    KNUCKLEBONES_ARENA,
    MEDITATION_MAT,
    SCARIFICATIONIST,
    SECURITY_TURRET,
    SECURITY_TURRET_2,
    WITCH_DOCTOR,
    MAYPOLE,
    FLOWER_GARDEN,
    BERRY_BUSH,
    SLEEPING_BAG,
    BLOOD_STONE,
    OUTHOUSE,
    POOP,
    OUTPOST_SHRINE,
    LUMBER_MINE,
    COMPOST_BIN,
    BLOODMOON_OFFERING,
    DECORATION_LAMB_STATUE,
    DECORATION_TORCH,
    PUMPKIN_BUSH,
    SACRIFICIAL_STONE,
    DECORATION_FLOWER_BOX_1,
    DECORATION_FLOWER_BOX_2,
    DECORATION_FLOWER_BOX_3,
    DECORATION_SMALL_STONE_CANDLE,
    DECORATION_FLAG_CROWN,
    DECORATION_FLAG_SCRIPTURE,
    DECORATION_WALL_TWIGS,
    DECORATION_LAMB_FLAG_STATUE,
    TEMPLE_BASE,
    TEMPLE,
    TEMPLE_EXTENSION1,
    TEMPLE_EXTENSION2,
    BUILDSITE_BUILDINGPROJECT,
    TEMPLE_BASE_EXTENSION1,
    TEMPLE_BASE_EXTENSION2,
    SHRINE_BLUEHEART,
    SHRINE_REDHEART,
    SHRINE_BLACKHEART,
    SHRINE_TAROT,
    SHRINE_DAMAGE,
    SHRINE_II,
    COLLECTED_RESOURCES_CHEST,
    SHRINE_BASE,
    MEAL_MEAT,
    MEAL_GREAT,
    MEAL_GRASS,
    MEAL_GOOD_FISH,
    MEAL_FOLLOWER_MEAT,
    HEALING_BAY,
    APOTHECARY,
    SCARECROW,
    HARVEST_TOTEM,
    FARM_PLOT_SIGN,
    MUSHROOM_BUSH,
    RED_FLOWER_BUSH,
    WHITE_FLOWER_BUSH,
    DECORATION_SHRUB,
    MEAL_MUSHROOMS,
    TEMPLE_II,
    MEAL_POOP,
    MEAL_ROTTEN,
    MISSION_SHRINE,
    REFINERY,
    SHRINE_PASSIVE,
    RESOURCE,
    SHRINE_III,
    SHRINE_IV,
    OFFERING_STATUE,
    SHRINE_PASSIVE_II,
    SHRINE_PASSIVE_III,
    TEMPLE_III,
    TEMPLE_IV,
    TILE_PATH,
    RUBBLE_BIG,
    WATER_SMALL,
    WATER_MEDIUM,
    WATER_BIG,
    RATAU_SHRINE,
    GOLD_ORE,
    PROPAGANDA_SPEAKER,
    HEALING_BAY_2,
    MISSIONARY,
    FEAST_TABLE,
    SILO_SEED,
    SILO_FERTILISER,
    SURVEILLANCE,
    FISHING_HUT_2,
    OUTHOUSE_2,
    SCARECROW_2,
    HARVEST_TOTEM_2,
    REFINERY_2,
    TREE_HITTABLE,
    STONE_HITTABLE,
    BONES_HITTABLE,
    POOP_HITTABLE,
    DECORATION_BARROW,
    DECORATION_BELL_STATUE,
    DECORATION_BONE_ARCH,
    DECORATION_BONE_BARREL,
    DECORATION_BONE_CANDLE,
    DECORATION_BONE_FLAG,
    DECORATION_BONE_LANTERN,
    DECORATION_BONE_PILLAR,
    DECORATION_BONE_SCULPTURE,
    DECORATION_CANDLE_BARREL,
    DECORATION_CRYSTAL_LAMP,
    DECORATION_CRYSTAL_LIGHT,
    DECORATION_CRYSTAL_ROCK,
    DECORATION_CRYSTAL_STATUE,
    DECORATION_CRYSTAL_TREE,
    DECORATION_CRYSTAL_WINDOW,
    DECORATION_FLOWER_ARCH,
    DECORATION_FOUNTAIN,
    DECORATION_POST_BOX,
    DECORATION_PUMPKIN_PILE,
    DECORATION_PUMPKIN_STOOL,
    DECORATION_STONE_CANDLE,
    DECORATION_STONE_FLAG,
    DECORATION_STONE_MUSHROOM,
    DECORATION_TORCH_BIG,
    DECORATION_TWIG_LAMP,
    DECORATION_WALL_BONE,
    DECORATION_WALL_STONE,
    DECORATION_WALL_GRASS,
    DECORATION_WREATH_STICK,
    DECORATION_STUMP_LAMB_STATUE,
    CHOPPING_SHRINE,
    MINING_SHRINE,
    FORAGING_SHRINE,
    EDIT_BUILDINGS,
    JANITOR_STATION,
    DECORATION_BELL_SMALL,
    DECORATION_BONE_SKULL_BIG,
    DECORATION_BONE_SKULL_PILE,
    DECORATION_FLAG_CRYSTAL,
    DECORATION_FLAG_MUSHROOM,
    DECORATION_FLOWER_BOTTLE,
    DECORATION_FLOWER_CART,
    DECORATION_HAY_BALE,
    DECORATION_HAY_PILE,
    DECORATION_LEAFY_FLOWER_SCULPTURE,
    DECORATION_LEAFY_LANTERN,
    DECORATION_LEAFY_SCULPTURE,
    DECORATION_MUSHROOM_1,
    DECORATION_MUSHROOM_2,
    DECORATION_MUSHROOM_CANDLE_1,
    DECORATION_MUSHROOM_CANDLE_2,
    DECORATION_MUSHROOM_CANDLE_LARGE,
    DECORATION_MUSHROOM_SCULPTURE,
    DECORATION_SPIDER_LANTERN,
    DECORATION_SPIDER_PILLAR,
    DECORATION_SPIDER_SCULPTURE,
    DECORATION_SPIDER_TORCH,
    DECORATION_SPIDER_WEB_CROWN_SCULPTURE,
    DECORATION_STONE_CANDLE_LAMP,
    DECORATION_STONE_HENGE,
    DECORATION_WALL_SPIDER,
    DECORATION_POND,
    DEMON_SUMMONER,
    COMPOST_BIN_DEAD_BODY,
    MEAL_GREAT_FISH,
    MEAL_BAD_FISH,
    BEETROOT_BUSH,
    CAULIFLOWER_BUSH,
    BED_1_COLLAPSED,
    BED_2_COLLAPSED,
    DEMON_SUMMONER_2,
    DEMON_SUMMONER_3,
    MEAL_BERRIES,
    MEAL_MEDIUM_VEG,
    MEAL_BAD_MIXED,
    MEAL_MEDIUM_MIXED,
    MEAL_GREAT_MIXED,
    MEAL_DEADLY,
    MEAL_BAD_MEAT,
    MEAL_GREAT_MEAT,
    FISHING_SPOT,
    MISSIONARY_II,
    MISSIONARY_III,
    KITCHEN_II,
    FARM_STATION_II,
    MEAL_BURNED,
    TILE_FLOWERS,
    TILE_HAY,
    TILE_PLANKS,
    TILE_SPOOKYPLANKS,
    TILE_REDGRASS,
    TILE_ROCKS,
    TILE_BRICKS,
    TILE_BLOOD,
    TILE_WATER,
    TILE_GOLD,
    TILE_MOSAIC,
    TILE_FLOWERSROCKY,
    DECORATION_MONSTERSHRINE,
    DECORATION_FLOWERPOTWALL,
    DECORATION_LEAFYLAMPPOST,
    DECORATION_FLOWERVASE,
    DECORATION_WATERINGCAN,
    DECORATION_FLOWER_CART_SMALL,
    DECORATION_WEEPINGSHRINE,
    DECORATION_BOSS_TROPHY_1,
    DECORATION_BOSS_TROPHY_2,
    DECORATION_BOSS_TROPHY_3,
    DECORATION_BOSS_TROPHY_4,
    DECORATION_BOSS_TROPHY_5,
    DECORATION_VIDEO,
    DECORATION_PLUSH,
    DECORATION_TWITCH_FLAG_CROWN,
    DECORATION_TWITCH_MUSHROOM_BAG,
    DECORATION_TWITCH_ROSE_BUSH,
    DECORATION_TWITCH_STONE_FLAG,
    DECORATION_TWITCH_STONE_STATUE,
    DECORATION_TWITCH_WOODEN_GUARDIAN,
    DECORATION_HALLOWEEN_PUMPKIN,
    DECORATION_HALLOWEEN_SKULL,
    DECORATION_HALLOWEEN_CANDLE,
    DECORATION_HALLOWEEN_TREE,
  }
}
