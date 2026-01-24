// Decompiled with JetBrains decompiler
// Type: StructureBrain
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
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
    StructureBrain.TYPES.DECORATION_HALLOWEEN_CANDLE,
    StructureBrain.TYPES.DECORATION_OLDFAITH_FOUNTAIN,
    StructureBrain.TYPES.DECORATION_OLDFAITH_CRYSTAL,
    StructureBrain.TYPES.DECORATION_DST_TREE,
    StructureBrain.TYPES.DECORATION_SINFUL_STATUE,
    StructureBrain.TYPES.DECORATION_PILGRIM_PAGODA,
    StructureBrain.TYPES.DECORATION_GOAT_STATUE,
    StructureBrain.TYPES.ICE_SCULPTURE,
    StructureBrain.TYPES.ICE_SCULPTURE_1,
    StructureBrain.TYPES.ICE_SCULPTURE_2,
    StructureBrain.TYPES.ICE_SCULPTURE_3,
    StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_WOLF,
    StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_YNGYA,
    StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_EXECUTIONER,
    StructureBrain.TYPES.DECORATION_APPLE_WELL,
    StructureBrain.TYPES.DECORATION_CHRISTMAS_TREE
  };
  public static HashSet<StructureBrain.TYPES> WoolhavenDecos = DataManager.MajorDLCDecorationsDisplay(DataManager.DecorationMajorDLCGrouping.Woolhaven).ToHashSet<StructureBrain.TYPES>();
  public static HashSet<StructureBrain.TYPES> RotDecos = DataManager.MajorDLCDecorationsDisplay(DataManager.DecorationMajorDLCGrouping.Rot).ToHashSet<StructureBrain.TYPES>();
  public static HashSet<StructureBrain.TYPES> EwefallDecos = DataManager.MajorDLCDecorationsDisplay(DataManager.DecorationMajorDLCGrouping.Ewefall).ToHashSet<StructureBrain.TYPES>();
  public static HashSet<StructureBrain.TYPES> MiscWoolhavenDecos = DataManager.MajorDLCDecorationsDisplay(DataManager.DecorationMajorDLCGrouping.Major_DLC).ToHashSet<StructureBrain.TYPES>();
  public StructuresData Data;
  public bool ReservedForTask;
  public bool ReservedByPlayer;
  public bool ForceRemoved;
  public Action<int> OnSoulsGained;
  public Action<float> OnFuelModified;
  public System.Action OnItemDeposited;
  public System.Action OnItemRemoved;
  public static Dictionary<int, int> _brainsIDtoIndex = new Dictionary<int, int>();
  public static List<StructureBrain> _brains = new List<StructureBrain>();
  public static List<StructureBrain> AllBrains = StructureBrain._brains;
  public System.Action OnCollapse;
  public System.Action OnSnowedUnder;
  public System.Action OnRepaired;
  public System.Action OnDefrosted;
  public System.Action OnAflamed;
  public System.Action OnExtinguished;
  public Vector2 FLAME_COLLAPSE_DURATION = new Vector2(480f, 2400f);

  public virtual void OnAdded()
  {
    SeasonsManager.OnSeasonChanged += new SeasonsManager.SeasonEvent(this.OnSeasonChanged);
    TimeManager.OnNewPhaseStarted += new System.Action(this.OnNewPhaseStarted);
  }

  public virtual void OnRemoved()
  {
    SeasonsManager.OnSeasonChanged -= new SeasonsManager.SeasonEvent(this.OnSeasonChanged);
    TimeManager.OnNewPhaseStarted -= new System.Action(this.OnNewPhaseStarted);
  }

  public virtual void OnSeasonChanged(SeasonsManager.Season season)
  {
  }

  public virtual int SoulMax => 10;

  public int SoulCount
  {
    get => this.Data.SoulCount;
    set
    {
      int soulCount = this.SoulCount;
      this.Data.SoulCount = Mathf.Clamp(value, 0, this.SoulMax);
      Action<int> onSoulsGained = this.OnSoulsGained;
      if (onSoulsGained == null)
        return;
      onSoulsGained(this.SoulCount - soulCount);
    }
  }

  public virtual bool IsFull => false;

  public virtual void Init(StructuresData data)
  {
    this.Data = data;
    if (this.Data.IsCollapsed)
      this.Collapse(false);
    else if (this.Data.IsAflame)
    {
      this.SetAflame(false);
    }
    else
    {
      if (!this.Data.IsSnowedUnder)
        return;
      this.SnowedUnder(false);
    }
  }

  public virtual void UpdateFuel(int amountToRemove = 5)
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

  public virtual void AddToGrid(Vector2Int gridTilePosition)
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

  public virtual void DepositItem(InventoryItem.ITEM_TYPE type, int quantity = 1)
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
    for (int index = this.Data.Inventory.Count - 1; index >= 0; --index)
    {
      if (this.Data.Inventory[index] == null)
        this.Data.Inventory.RemoveAt(index);
    }
    this.Data.Inventory.Add(new InventoryItem(type));
    System.Action onItemDeposited = this.OnItemDeposited;
    if (onItemDeposited == null)
      return;
    onItemDeposited();
  }

  public void RemoveItems(InventoryItem.ITEM_TYPE type, int quantity)
  {
    for (int index = this.Data.Inventory.Count - 1; index >= 0; --index)
    {
      if (this.Data.Inventory[index] != null && (InventoryItem.ITEM_TYPE) this.Data.Inventory[index].type == type)
      {
        int num = quantity;
        quantity = Mathf.Clamp(quantity - this.Data.Inventory[index].quantity, 0, quantity);
        this.Data.Inventory[index].quantity -= num;
        if (this.Data.Inventory[index].quantity <= 0)
          this.Data.Inventory.RemoveAt(index);
      }
    }
    System.Action onItemRemoved = this.OnItemRemoved;
    if (onItemRemoved == null)
      return;
    onItemRemoved();
  }

  public virtual void ToDebugString(StringBuilder sb)
  {
    sb.AppendLine($"{this.Data.Location}; ({this.Data.GridX},{this.Data.GridY}); {this.Data.Position}");
  }

  public static bool TryAddBrain(in int id, in StructureBrain brain)
  {
    if (StructureBrain._brainsIDtoIndex.ContainsKey(id))
      return false;
    StructureBrain._brains.Add(brain);
    StructureBrain._brainsIDtoIndex[id] = StructureBrain._brains.Count - 1;
    return true;
  }

  public static StructureBrain CreateBrain(StructuresData data)
  {
    if (StructureBrain._brainsIDtoIndex.ContainsKey(data.ID))
      return (StructureBrain) null;
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
      case StructureBrain.TYPES.COTTON_BUSH:
        brain = (StructureBrain) new Structures_BerryBush();
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
      case StructureBrain.TYPES.MEAL_EGG:
      case StructureBrain.TYPES.MEAL_SPICY:
      case StructureBrain.TYPES.MEAL_SNOW_FRUIT:
      case StructureBrain.TYPES.MEAL_MILK_BAD:
      case StructureBrain.TYPES.MEAL_MILK_GOOD:
      case StructureBrain.TYPES.MEAL_MILK_GREAT:
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
      case StructureBrain.TYPES.MATING_TENT:
        brain = (StructureBrain) new Structures_MatingTent();
        break;
      case StructureBrain.TYPES.BLOODSTONE_MINE:
      case StructureBrain.TYPES.BLOODSTONE_MINE_2:
        brain = (StructureBrain) new Structures_MinerStation();
        break;
      case StructureBrain.TYPES.DRUM_CIRCLE:
        brain = new StructureBrain();
        break;
      case StructureBrain.TYPES.FISHING_HUT:
        brain = (StructureBrain) new Structures_FishingHut();
        break;
      case StructureBrain.TYPES.KNUCKLEBONES_ARENA:
        brain = new StructureBrain();
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
      case StructureBrain.TYPES.POOP:
      case StructureBrain.TYPES.POOP_GOLD:
      case StructureBrain.TYPES.POOP_RAINBOW:
      case StructureBrain.TYPES.POOP_MASSIVE:
      case StructureBrain.TYPES.POOP_DEVOTION:
      case StructureBrain.TYPES.POOP_PET:
      case StructureBrain.TYPES.POOP_GLOW:
      case StructureBrain.TYPES.POOP_ROTSTONE:
        brain = (StructureBrain) new Structures_Poop();
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
      case StructureBrain.TYPES.SHRINE_DISCIPLE_BOOST:
      case StructureBrain.TYPES.SHRINE_PLEASURE:
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
      case StructureBrain.TYPES.SEED_BUCKET:
        brain = (StructureBrain) new Structures_SiloSeed();
        break;
      case StructureBrain.TYPES.SILO_FERTILISER:
      case StructureBrain.TYPES.POOP_BUCKET:
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
      case StructureBrain.TYPES.DECORATION_TORCH_BIG:
      case StructureBrain.TYPES.DECORATION_SPIDER_TORCH:
      case StructureBrain.TYPES.DECORATION_OLDFAITH_TORCH:
        brain = (StructureBrain) new Structures_WinterTorch();
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
      case StructureBrain.TYPES.JANITOR_STATION_2:
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
      case StructureBrain.TYPES.FARM_STATION_II:
        brain = (StructureBrain) new Structures_FarmerStation();
        break;
      case StructureBrain.TYPES.MORGUE_1:
        brain = (StructureBrain) new Structures_Morgue();
        break;
      case StructureBrain.TYPES.MORGUE_2:
        brain = (StructureBrain) new Structures_Morgue();
        break;
      case StructureBrain.TYPES.CRYPT_1:
        brain = (StructureBrain) new Structures_Crypt();
        break;
      case StructureBrain.TYPES.CRYPT_2:
        brain = (StructureBrain) new Structures_Crypt();
        break;
      case StructureBrain.TYPES.CRYPT_3:
        brain = (StructureBrain) new Structures_Crypt();
        break;
      case StructureBrain.TYPES.SHARED_HOUSE:
        brain = (StructureBrain) new Structures_SharedHouse();
        break;
      case StructureBrain.TYPES.TAILOR:
        brain = (StructureBrain) new Structures_Tailor();
        break;
      case StructureBrain.TYPES.SHRINE_WEATHER_BLIZZARD:
      case StructureBrain.TYPES.SHRINE_WEATHER_HEATWAVE:
      case StructureBrain.TYPES.SHRINE_WEATHER_TYPHOON:
        brain = (StructureBrain) new Structures_Shrine_Weather();
        break;
      case StructureBrain.TYPES.PUB:
      case StructureBrain.TYPES.PUB_2:
        brain = (StructureBrain) new Structures_Pub();
        break;
      case StructureBrain.TYPES.HOPS_BUSH:
        brain = (StructureBrain) new Structures_BerryBush();
        break;
      case StructureBrain.TYPES.GRAPES_BUSH:
        brain = (StructureBrain) new Structures_BerryBush();
        break;
      case StructureBrain.TYPES.EGG_FOLLOWER:
        brain = (StructureBrain) new Structures_EggFollower();
        break;
      case StructureBrain.TYPES.SHRINE_DISCIPLE_COLLECTION:
        brain = (StructureBrain) new Structures_Shrine_Disciple_Collection();
        break;
      case StructureBrain.TYPES.HATCHERY:
      case StructureBrain.TYPES.HATCHERY_2:
        brain = (StructureBrain) new Structures_Hatchery();
        break;
      case StructureBrain.TYPES.SOZO_BUSH:
        brain = (StructureBrain) new Structures_BerryBush();
        break;
      case StructureBrain.TYPES.LEADER_TENT:
        brain = (StructureBrain) new Structures_Bed();
        break;
      case StructureBrain.TYPES.DAYCARE:
        brain = (StructureBrain) new Structures_Daycare();
        break;
      case StructureBrain.TYPES.WEATHER_VANE:
        brain = (StructureBrain) new Structures_WeatherVane();
        break;
      case StructureBrain.TYPES.WOOLY_SHACK:
        brain = (StructureBrain) new Structures_WoolyShack();
        break;
      case StructureBrain.TYPES.VOLCANIC_SPA:
        brain = new StructureBrain();
        break;
      case StructureBrain.TYPES.SNOW_FRUIT_BUSH:
        brain = (StructureBrain) new Structures_BerryBush();
        break;
      case StructureBrain.TYPES.CHILLI_BUSH:
        brain = (StructureBrain) new Structures_BerryBush();
        break;
      case StructureBrain.TYPES.GRASS_BUSH:
        brain = (StructureBrain) new Structures_BerryBush();
        break;
      case StructureBrain.TYPES.ICE_BLOCK:
      case StructureBrain.TYPES.SNOWMAN:
      case StructureBrain.TYPES.SNOW_DRIFT:
      case StructureBrain.TYPES.SNOW_BALL:
        brain = (StructureBrain) new Structures_IceBlock();
        break;
      case StructureBrain.TYPES.RANCH:
        brain = (StructureBrain) new Structures_Ranch();
        break;
      case StructureBrain.TYPES.RANCH_FENCE:
        brain = (StructureBrain) new Structures_RanchFence();
        break;
      case StructureBrain.TYPES.RANCH_TROUGH:
        brain = (StructureBrain) new Structures_RanchTrough();
        break;
      case StructureBrain.TYPES.MEDIC:
        brain = (StructureBrain) new Structures_Medic();
        break;
      case StructureBrain.TYPES.RANCH_2:
        brain = (StructureBrain) new Structures_Ranch();
        break;
      case StructureBrain.TYPES.RANCH_HUTCH:
        brain = (StructureBrain) new Structures_RanchHutch();
        break;
      case StructureBrain.TYPES.RACING_GATE:
        brain = new StructureBrain();
        break;
      case StructureBrain.TYPES.LOGISTICS:
        brain = (StructureBrain) new Structures_Logistics();
        break;
      case StructureBrain.TYPES.WOLF_TRAP:
        brain = new StructureBrain();
        break;
      case StructureBrain.TYPES.LIGHTNING_ROD:
      case StructureBrain.TYPES.LIGHTNING_ROD_2:
        brain = (StructureBrain) new Structures_LightningRod();
        break;
      case StructureBrain.TYPES.FURNACE_1:
      case StructureBrain.TYPES.FURNACE_2:
      case StructureBrain.TYPES.FURNACE_3:
        brain = (StructureBrain) new Structures_Furnace();
        break;
      case StructureBrain.TYPES.TOXIC_WASTE:
        brain = (StructureBrain) new Structures_Waste();
        break;
      case StructureBrain.TYPES.TOOLSHED:
        brain = (StructureBrain) new Structures_Toolshed();
        break;
      case StructureBrain.TYPES.FARM_CROP_GROWER:
        brain = (StructureBrain) new Structures_FarmCropGrower();
        break;
      case StructureBrain.TYPES.SACRIFICE_TABLE:
        brain = new StructureBrain();
        break;
      case StructureBrain.TYPES.TRAIT_MANIPULATOR_1:
      case StructureBrain.TYPES.TRAIT_MANIPULATOR_2:
      case StructureBrain.TYPES.TRAIT_MANIPULATOR_3:
        brain = (StructureBrain) new Structures_TraitManipulator();
        break;
      case StructureBrain.TYPES.ICE_SCULPTURE:
      case StructureBrain.TYPES.ICE_SCULPTURE_1:
      case StructureBrain.TYPES.ICE_SCULPTURE_2:
      case StructureBrain.TYPES.ICE_SCULPTURE_3:
        brain = (StructureBrain) new Structures_IceSculpture();
        break;
      case StructureBrain.TYPES.RANCH_CHOPPING_BLOCK:
        brain = new StructureBrain();
        break;
      case StructureBrain.TYPES.PROXIMITY_FURNACE:
        brain = (StructureBrain) new Structures_ProximityFurnace();
        break;
      case StructureBrain.TYPES.ROTSTONE_MINE:
      case StructureBrain.TYPES.ROTSTONE_MINE_2:
        brain = (StructureBrain) new Structures_RotstoneStation();
        break;
      default:
        brain = new StructureBrain();
        break;
    }
    StructureBrain.ApplyConfigToData(data);
    brain.Init(data);
    StructureBrain.TryAddBrain(in data.ID, in brain);
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
    data.isDeletable = infoByType.isDeletable;
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

  public static void RemoveBrain(in StructureBrain brain)
  {
    if (!StructureBrain._brainsIDtoIndex.ContainsKey(brain.Data.ID))
      return;
    int index1 = StructureBrain._brainsIDtoIndex[brain.Data.ID];
    StructureBrain._brainsIDtoIndex.Remove(brain.Data.ID);
    StructureBrain._brains.RemoveAt(index1);
    StructureBrain._brainsIDtoIndex.Clear();
    for (int index2 = 0; index2 < StructureBrain._brains.Count; ++index2)
      StructureBrain._brainsIDtoIndex[StructureBrain._brains[index2].Data.ID] = index2;
    brain.OnRemoved();
  }

  public static bool TryFindBrainByID(in int ID, out StructureBrain result)
  {
    result = (StructureBrain) null;
    if (!StructureBrain._brainsIDtoIndex.ContainsKey(ID))
      return false;
    result = StructureBrain._brains[StructureBrain._brainsIDtoIndex[ID]];
    return true;
  }

  public static bool TryFindBrainByID<T>(in int ID, out T result) where T : StructureBrain
  {
    result = default (T);
    if (!StructureBrain._brainsIDtoIndex.ContainsKey(ID))
      return false;
    result = StructureBrain._brains[StructureBrain._brainsIDtoIndex[ID]] as T;
    return (object) result != null;
  }

  public static StructureBrain GetOrCreateBrain(StructuresData data)
  {
    StructureBrain result;
    if (!StructureBrain.TryFindBrainByID(in data.ID, out result))
    {
      result = StructureBrain.CreateBrain(data);
      result.OnAdded();
    }
    return result;
  }

  public static bool IsPath(StructureBrain.TYPES type)
  {
    return type == StructureBrain.TYPES.PLANK_PATH || type == StructureBrain.TYPES.TILE_PATH || type == StructureBrain.TYPES.TILE_HAY || type == StructureBrain.TYPES.TILE_BLOOD || type == StructureBrain.TYPES.TILE_ROCKS || type == StructureBrain.TYPES.TILE_WATER || type == StructureBrain.TYPES.TILE_BRICKS || type == StructureBrain.TYPES.TILE_PLANKS || type == StructureBrain.TYPES.TILE_FLOWERS || type == StructureBrain.TYPES.TILE_REDGRASS || type == StructureBrain.TYPES.TILE_SPOOKYPLANKS || type == StructureBrain.TYPES.TILE_GOLD || type == StructureBrain.TYPES.TILE_MOSAIC || type == StructureBrain.TYPES.TILE_FLOWERSROCKY || type == StructureBrain.TYPES.TILE_OLDFAITH || type == StructureBrain.TYPES.DECORATION_DLC_ROT_FLOOR || type == StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_FLOOR;
  }

  public virtual void OnNewPhaseStarted()
  {
    if ((double) this.Data.AflameCollapseTarget == 0.0 || (double) TimeManager.TotalElapsedGameTime <= (double) this.Data.AflameCollapseTarget)
      return;
    this.Collapse();
  }

  public virtual bool Collapse(
    bool showNotifications = true,
    bool refreshFollowerTasks = true,
    bool struckByLightning = false)
  {
    if (this.Data.ClaimedByPlayer || !StructureManager.IsCollapsible(this.Data.Type) || this.Data.IsCollapsed || this.Data.IsSnowedUnder)
      return false;
    if (this.Data.IsAflame)
    {
      if (PlayerFarming.Location == FollowerLocation.Base)
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.CHARCOAL, UnityEngine.Random.Range(3, 6), this.Data.Position);
      Structures_CollectedResourceChest result;
      if (StructureManager.TryGetFirstStructureOfType<Structures_CollectedResourceChest>(out result, in PlayerFarming.Location))
        result.AddItem(InventoryItem.ITEM_TYPE.CHARCOAL, UnityEngine.Random.Range(3, 6));
      System.Action onExtinguished = this.OnExtinguished;
      if (onExtinguished != null)
        onExtinguished();
    }
    this.Data.IsCollapsed = true;
    this.Data.IsSnowedUnder = false;
    System.Action onCollapse = this.OnCollapse;
    if (onCollapse != null)
      onCollapse();
    this.Data.AflameCollapseTarget = 0.0f;
    if ((UnityEngine.Object) PlacementRegion.Instance != (UnityEngine.Object) null)
    {
      PlacementRegion.TileGridTile tileGridTile = PlacementRegion.Instance.GetTileGridTile(this.Data.GridTilePosition);
      if (tileGridTile != null)
        tileGridTile.Collapsed = true;
    }
    if (showNotifications)
      NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("Notifications/StructureCollapsed", $"<color=#FFD201>{this.Data.GetLocalizedName()}</color>");
    if (refreshFollowerTasks)
    {
      foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
      {
        if ((this.Data.FollowerID == allBrain.Info.ID || this.Data.MultipleFollowerIDs.Contains(allBrain.Info.ID) || allBrain.CurrentTask != null && allBrain.CurrentTask.SubTaskCode == this.Data.ID) && (!(this is Structures_Bed) || allBrain.CurrentTaskType != FollowerTaskType.Sleep))
          allBrain.CurrentTask?.Abort();
      }
    }
    this.ReservedForTask = true;
    StructureManager.StructureChanged onStructureMoved = StructureManager.OnStructureMoved;
    if (onStructureMoved != null)
      onStructureMoved(this.Data);
    return true;
  }

  public virtual void Repaired()
  {
    if ((UnityEngine.Object) PlacementRegion.Instance != (UnityEngine.Object) null)
    {
      PlacementRegion.TileGridTile tileGridTile = PlacementRegion.Instance.GetTileGridTile(this.Data.GridTilePosition);
      if (tileGridTile != null && tileGridTile.ObjectID == this.Data.ID)
        tileGridTile.Collapsed = false;
    }
    this.Data.IsSnowedUnder = false;
    this.Data.IsCollapsed = false;
    this.ReservedForTask = false;
    System.Action onRepaired = this.OnRepaired;
    if (onRepaired != null)
      onRepaired();
    StructureManager.StructureChanged onStructureMoved = StructureManager.OnStructureMoved;
    if (onStructureMoved == null)
      return;
    onStructureMoved(this.Data);
  }

  public virtual bool SnowedUnder(bool showNotifications = true, bool refreshFollowerTasks = true)
  {
    if (this.Data.ClaimedByPlayer || !StructureManager.IsCollapsible(this.Data.Type) || this.Data.IsCollapsed || this.Data.IsSnowedUnder || this.Data.Type == StructureBrain.TYPES.VOLCANIC_SPA)
      return false;
    this.Data.IsSnowedUnder = true;
    this.Data.IsCollapsed = false;
    System.Action onSnowedUnder = this.OnSnowedUnder;
    if (onSnowedUnder != null)
      onSnowedUnder();
    if ((UnityEngine.Object) PlacementRegion.Instance != (UnityEngine.Object) null)
    {
      PlacementRegion.TileGridTile tileGridTile = PlacementRegion.Instance.GetTileGridTile(this.Data.GridTilePosition);
      if (tileGridTile != null)
        tileGridTile.Collapsed = true;
    }
    if (showNotifications)
      NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("Notifications/StructureSnowedUnder", $"<color=#FFD201>{this.Data.GetLocalizedName()}</color>");
    if (refreshFollowerTasks)
    {
      foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
      {
        if (this.Data.FollowerID == allBrain.Info.ID || this.Data.MultipleFollowerIDs.Contains(allBrain.Info.ID) || allBrain.CurrentTask != null && allBrain.CurrentTask.SubTaskCode == this.Data.ID)
          allBrain.CurrentTask?.Abort();
      }
    }
    if (PlayerFarming.Location == FollowerLocation.Base)
      AudioManager.Instance.PlayOneShot("event:/dlc/env/snowdrift/appear", this.Data.Position);
    this.ReservedForTask = true;
    StructureManager.StructureChanged onStructureMoved = StructureManager.OnStructureMoved;
    if (onStructureMoved != null)
      onStructureMoved(this.Data);
    return true;
  }

  public virtual void Defrost()
  {
    if ((UnityEngine.Object) PlacementRegion.Instance != (UnityEngine.Object) null)
    {
      PlacementRegion.TileGridTile tileGridTile = PlacementRegion.Instance.GetTileGridTile(this.Data.GridTilePosition);
      if (tileGridTile != null && tileGridTile.ObjectID == this.Data.ID)
        tileGridTile.Collapsed = false;
    }
    if (PlayerFarming.Location == FollowerLocation.Base)
      AudioManager.Instance.PlayOneShot("event:/dlc/env/snowdrift/disappear", this.Data.Position);
    this.Data.IsSnowedUnder = false;
    this.Data.IsCollapsed = false;
    this.ReservedForTask = false;
    System.Action onDefrosted = this.OnDefrosted;
    if (onDefrosted != null)
      onDefrosted();
    StructureManager.StructureChanged onStructureMoved = StructureManager.OnStructureMoved;
    if (onStructureMoved == null)
      return;
    onStructureMoved(this.Data);
  }

  public virtual bool SetAflame(bool showNotifications = true)
  {
    if (this.Data.ClaimedByPlayer || !StructureManager.IsCollapsible(this.Data.Type))
      return false;
    this.Data.IsAflame = true;
    this.Data.AflameCollapseTarget = TimeManager.TotalElapsedGameTime + UnityEngine.Random.Range(this.FLAME_COLLAPSE_DURATION.x, this.FLAME_COLLAPSE_DURATION.y);
    System.Action onAflamed = this.OnAflamed;
    if (onAflamed != null)
      onAflamed();
    if (showNotifications)
      NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("Notifications/StructureAflamed", this.Data.GetLocalizedName());
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (this.Data.FollowerID == allBrain.Info.ID || this.Data.MultipleFollowerIDs.Contains(allBrain.Info.ID) || allBrain.CurrentTask != null && allBrain.CurrentTask.SubTaskCode == this.Data.ID)
        allBrain.CurrentTask?.Abort();
    }
    this.ReservedForTask = true;
    return true;
  }

  public virtual void SetExtinguished()
  {
    this.Data.IsAflame = false;
    this.ReservedForTask = false;
    System.Action onExtinguished = this.OnExtinguished;
    if (onExtinguished != null)
      onExtinguished();
    this.Data.AflameCollapseTarget = 0.0f;
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
    MAJOR_DLC,
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
    COTTON_BUSH,
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
    MORGUE_1,
    MORGUE_2,
    CRYPT_1,
    CRYPT_2,
    CRYPT_3,
    SHARED_HOUSE,
    DECORATION_OLDFAITH_CRYSTAL,
    DECORATION_OLDFAITH_FLAG,
    DECORATION_OLDFAITH_FOUNTAIN,
    DECORATION_OLDFAITH_IRONMAIDEN,
    DECORATION_OLDFAITH_SHRINE,
    DECORATION_OLDFAITH_TORCH,
    DECORATION_OLDFAITH_WALL,
    TILE_OLDFAITH,
    WEBBER_SKULL,
    DECORATION_DST_ALCHEMY,
    DECORATION_DST_DEERCLOPS,
    DECORATION_DST_MARBLETREE,
    DECORATION_DST_PIGSTICK,
    DECORATION_DST_SCIENCEMACHINE,
    DECORATION_DST_TREE,
    DECORATION_DST_WALL,
    DECORATION_DST_GLOMMERSTATUE,
    DECORATION_DST_BEEFALOSKELETON,
    TAILOR,
    MEAL_EGG,
    SHRINE_WEATHER_BLIZZARD,
    SHRINE_WEATHER_HEATWAVE,
    SHRINE_WEATHER_TYPHOON,
    POOP_BUCKET,
    SEED_BUCKET,
    JANITOR_STATION_2,
    SHRINE_DISCIPLE_BOOST,
    POOP_GOLD,
    POOP_RAINBOW,
    POOP_MASSIVE,
    POOP_DEVOTION,
    POOP_PET,
    POOP_GLOW,
    PUB,
    SHRINE_PLEASURE,
    HOPS_BUSH,
    GRAPES_BUSH,
    EGG_FOLLOWER,
    SHRINE_DISCIPLE_COLLECTION,
    HATCHERY,
    PUB_2,
    HATCHERY_2,
    IGNORE_1,
    IGNORE_2,
    DECORATION_GNOME1,
    DECORATION_GNOME2,
    DECORATION_GNOME3,
    DECORATION_SINFUL_STATUE,
    DECORATION_SINFUL_CRUCIFIX,
    DECORATION_SINFUL_FLOWERS1,
    DECORATION_SINFUL_FLOWERS2,
    DECORATION_SINFUL_SKULL,
    DECORATION_SINFUL_INCENSE,
    DECORATION_CNY_LANTERN,
    DECORATION_CNY_DRAGON,
    DECORATION_CNY_TREE,
    SOZO_BUSH,
    DRINK_BEER,
    DRINK_WINE,
    DRINK_COCKTAIL,
    DRINK_EGGNOG,
    DRINK_MUSHROOMJUICE,
    DRINK_POOPJUICE,
    DRINK_GIN,
    LEADER_TENT,
    DAYCARE,
    DECORATION_PILGRIM_WALL,
    DECORATION_PILGRIM_BONSAI,
    DECORATION_PILGRIM_LANTERN,
    DECORATION_PILGRIM_PAGODA,
    DECORATION_PILGRIM_VASE,
    DECORATION_GOAT_LANTERN,
    DECORATION_GOAT_STATUE,
    DECORATION_GOAT_PLANT,
    WEATHER_VANE,
    WOOLY_SHACK,
    VOLCANIC_SPA,
    SNOW_FRUIT_BUSH,
    MEAL_SPICY,
    CHILLI_BUSH,
    GRASS_BUSH,
    ICE_BLOCK,
    RANCH,
    RANCH_FENCE,
    RANCH_TROUGH,
    MEAL_SNOW_FRUIT,
    MEDIC,
    RANCH_2,
    RANCH_HUTCH,
    RACING_GATE,
    SNOWMAN,
    DECORATION_PALWORLD_LAMB,
    DECORATION_PALWORLD_LANTERN,
    DECORATION_PALWORLD_PLANT,
    DECORATION_PALWORLD_STATUE,
    DECORATION_PALWORLD_TREE,
    MAGMA_STONE_HITTABLE,
    LOGISTICS,
    LIGHTNING_SHARD_HITTABLE,
    WOLF_TRAP,
    LIGHTNING_ROD,
    FURNACE_1,
    FURNACE_2,
    FURNACE_3,
    TOXIC_WASTE,
    LIGHTNING_ROD_2,
    TOOLSHED,
    FARM_CROP_GROWER,
    SACRIFICE_TABLE,
    POOP_ROTSTONE,
    TRAIT_MANIPULATOR_1,
    TRAIT_MANIPULATOR_2,
    TRAIT_MANIPULATOR_3,
    ICE_SCULPTURE,
    DRINK_CHILLI,
    DRINK_LIGHTNING,
    DRINK_SIN,
    DRINK_GRASS,
    SNOW_DRIFT,
    SNOW_BALL,
    MEAL_MILK_BAD,
    MEAL_MILK_GOOD,
    MEAL_MILK_GREAT,
    DECORATION_DLC_ROT_BOTTLE,
    DECORATION_DLC_ROT_BUCKET,
    DECORATION_DLC_ROT_CAGE1,
    DECORATION_DLC_ROT_CAGE2,
    DECORATION_DLC_ROT_CAULDRON,
    DECORATION_DLC_ROT_DIORAMA,
    DECORATION_DLC_ROT_FIREMACHINE,
    DECORATION_DLC_ROT_FLOOR,
    DECORATION_DLC_ROT_IRONMAIDEN,
    DECORATION_DLC_ROT_LUMP1,
    DECORATION_DLC_ROT_LUMP2,
    DECORATION_DLC_ROT_PILLAR1,
    DECORATION_DLC_ROT_PILLAR2,
    DECORATION_DLC_ROT_STONE1,
    DECORATION_DLC_ROT_STONE2,
    DECORATION_DLC_ROT_TENTACLE,
    DECORATION_DLC_ROT_WALL,
    DECORATION_DLC_WOLF_BULB,
    DECORATION_DLC_WOLF_CULTIST1,
    DECORATION_DLC_WOLF_CULTIST2,
    DECORATION_DLC_WOLF_DIORAMA,
    DECORATION_DLC_WOLF_FIREPIT,
    DECORATION_DLC_WOLF_LAMPPOST,
    DECORATION_DLC_WOLF_PILLAR1,
    DECORATION_DLC_WOLF_PILLAR2,
    DECORATION_DLC_WOLF_STATUE1,
    DECORATION_DLC_WOLF_STATUE2,
    DECORATION_DLC_WOLF_STATUE3,
    DECORATION_DLC_WOLF_STATUE4,
    DECORATION_DLC_WOLF_TESLA,
    DECORATION_DLC_WOLF_TREE1,
    DECORATION_DLC_WOLF_WIRES,
    DECORATION_DLC_STEAMPUNK_CHIMNEY_1,
    DECORATION_DLC_STEAMPUNK_CHIMNEY_2,
    DECORATION_DLC_STEAMPUNK_LAMPPOST,
    DECORATION_DLC_STEAMPUNK_CLOCK,
    DECORATION_DLC_STEAMPUNK_WALL,
    DECORATION_DLC_STEAMPUNK_PLANT,
    DECORATION_DLC_YNGYA_CANDLE,
    DECORATION_DLC_YNGYA_FLOWERBUCKET,
    DECORATION_DLC_YNGYA_STICKBUNDLE,
    DECORATION_DLC_YNGYA_TALLFLOWERS,
    DECORATION_DLC_YNGYA_TREEBUSH,
    DECORATION_DLC_YNGYA_TREEPOT,
    DECORATION_DLC_WOOLHAVEN_BUSH1,
    DECORATION_DLC_WOOLHAVEN_BUSH2,
    DECORATION_DLC_WOOLHAVEN_FOUNTAIN,
    DECORATION_DLC_WOOLHAVEN_SMALLSTATUE1,
    DECORATION_DLC_WOOLHAVEN_SMALLSTATUE2,
    DECORATION_DLC_WOOLHAVEN_STREETLAMP,
    DECORATION_DLC_WOOLHAVEN_TREE,
    DECORATION_DLC_WOOLHAVEN_WALL,
    DECORATION_DLC_WOOLHAVEN_FLOOR,
    DECORATION_BOSS_TROPHY_DLC_WOLF,
    DECORATION_BOSS_TROPHY_DLC_YNGYA,
    DECORATION_BOSS_TROPHY_DLC_EXECUTIONER,
    DECORATION_DLC_WOLF_STATUE5,
    DECORATION_DLC_WOOLHAVEN_BARREL1,
    DECORATION_DLC_WOOLHAVEN_BARREL2,
    DECORATION_DLC_WOOLHAVEN_BELLS,
    DECORATION_DLC_WOOLHAVEN_JUG,
    DECORATION_DLC_WOOLHAVEN_RUG,
    DECORATION_DLC_WOOLHAVEN_CANDLE,
    RANCH_CHOPPING_BLOCK,
    PROXIMITY_FURNACE,
    ROTSTONE_MINE,
    ROTSTONE_MINE_2,
    DECORATION_APPLE_BUSH,
    DECORATION_APPLE_LANTERN,
    DECORATION_APPLE_STATUE,
    DECORATION_APPLE_VASE,
    DECORATION_APPLE_WELL,
    DECORATION_EASTEREGG_EGG,
    DECORATION_EASTEREGG_HAROSTATUE,
    DECORATION_EASTEREGG_TURUA,
    DECORATION_EASTEREGG_WARRACKA,
    ICE_SCULPTURE_1,
    ICE_SCULPTURE_2,
    ICE_SCULPTURE_3,
    DRINK_MILKSHAKE,
    DECORATION_CHRISTMAS_TREE,
    DECORATION_CHRISTMAS_SNOWMAN,
    DECORATION_CHRISTMAS_CANDLE,
    DECORATION_CHRISTMAS_CANDYCANE,
  }
}
