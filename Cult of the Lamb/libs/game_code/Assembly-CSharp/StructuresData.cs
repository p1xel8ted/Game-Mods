// Decompiled with JetBrains decompiler
// Type: StructuresData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI.BuildMenu;
using MessagePack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public sealed class StructuresData
{
  [Key(0)]
  public StructureBrain.TYPES Type;
  [Key(1)]
  public int VariantIndex = -1;
  [Key(2)]
  public string PrefabPath;
  [Key(3)]
  public bool RemoveOnDie = true;
  [Key(4)]
  public float ProgressTarget = 10f;
  [Key(5)]
  public bool WorkIsRequiredForProgress = true;
  [Key(6)]
  public bool IsUpgrade;
  [Key(7)]
  public bool IsUpgradeDestroyPrevious = true;
  [Key(8)]
  public bool IgnoreGrid;
  [Key(9)]
  public bool IsBuildingProject;
  [Key(10)]
  public bool IsCollapsed;
  [Key(11)]
  public bool IsAflame;
  [Key(12)]
  public bool IsSnowedUnder;
  [Key(13)]
  public float AflameCollapseTarget;
  [Key(14)]
  public StructureBrain.TYPES UpgradeFromType;
  [Key(15)]
  public StructureBrain.TYPES RequiresType;
  [Key(16 /*0x10*/)]
  public int TILE_WIDTH = 1;
  [Key(17)]
  public int TILE_HEIGHT = 1;
  [Key(18)]
  public bool CanBeMoved = true;
  [Key(19)]
  public bool CanBeRecycled = true;
  [Key(20)]
  public bool IsObstruction;
  [Key(21)]
  public bool DoesNotOccupyGrid;
  [Key(22)]
  public bool isDeletable = true;
  [Key(23)]
  public Vector2Int LootCountToDropRange;
  [Key(24)]
  public Vector2Int CropLootCountToDropRange;
  [Key(25)]
  public List<InventoryItem.ITEM_TYPE> MultipleLootToDrop;
  [Key(26)]
  public List<int> MultipleLootToDropChance;
  [Key(27)]
  public InventoryItem.ITEM_TYPE LootToDrop;
  [Key(28)]
  public int LootCountToDrop;
  [Key(29)]
  public int ID;
  [Key(30)]
  public FollowerLocation Location = FollowerLocation.None;
  [Key(31 /*0x1F*/)]
  public bool DontLoadMe;
  [Key(32 /*0x20*/)]
  public bool Destroyed;
  [Key(33)]
  public int GridX;
  [Key(34)]
  public int GridY;
  [Key(35)]
  public Vector2Int Bounds = new Vector2Int(1, 1);
  [Key(36)]
  public List<InventoryItem> Inventory = new List<InventoryItem>();
  [Key(37)]
  public float Progress;
  [Key(38)]
  public float PowerRequirement;
  [Key(39)]
  public Vector3 Position;
  [Key(40)]
  public Vector3 Offset;
  [Key(41)]
  public float OffsetMax;
  [Key(42)]
  public bool Repaired;
  [Key(43)]
  public Vector2Int GridTilePosition = StructuresData.NullPosition;
  [Key(44)]
  public Vector3Int PlacementRegionPosition;
  public static Vector2Int NullPosition = new Vector2Int(-2147483647 /*0x80000001*/, -2147483647 /*0x80000001*/);
  [Key(45)]
  public int Age;
  [Key(46)]
  public bool Exhausted;
  [Key(47)]
  public int UpgradeLevel;
  [Key(48 /*0x30*/)]
  public bool ClaimedByPlayer;
  [Key(49)]
  public int AvailableSlots = 1;
  [Key(124)]
  public List<StructuresData.LogisticsSlot> LogisticSlots = new List<StructuresData.LogisticsSlot>();
  [Key(50)]
  public List<StructuresData.PathData> pathData = new List<StructuresData.PathData>();
  [Key(51)]
  public int Direction = 1;
  [Key(52)]
  public int Rotation;
  [Key(53)]
  public Villager_Info v_i;
  [Key(54)]
  public int SoulCount;
  [Key(55)]
  public int Level;
  [Key(56)]
  public StructureBrain.TYPES ToBuildType;
  [Key(57)]
  public StructuresData.Phase CurrentPhase;
  [Key(58)]
  public bool Purchased;
  [IgnoreMember]
  public List<PlacementRegion.TileGridTile> grid = new List<PlacementRegion.TileGridTile>();
  [JsonIgnore]
  [IgnoreMember]
  public Dictionary<Vector2Int, PlacementRegion.TileGridTile> GridTileLookup = new Dictionary<Vector2Int, PlacementRegion.TileGridTile>();
  [Key(59)]
  public int FollowerID = -1;
  [Key(60)]
  public List<int> MultipleFollowerIDs = new List<int>();
  [Key(61)]
  public List<int> FollowersClaimedSlots = new List<int>();
  [Key(62)]
  public int BedpanCount;
  [Key(63 /*0x3F*/)]
  public bool HasFood;
  [Key(64 /*0x40*/)]
  public float FollowerImprisonedTimestamp;
  [Key(65)]
  public float FollowerImprisonedFaith;
  [Key(66)]
  public bool GivenGift;
  [Key(67)]
  public int Dir = 1;
  [Key(68)]
  public bool BodyWrapped;
  [Key(69)]
  public bool BeenInMorgueAlready;
  [Key(70)]
  public bool Prioritised;
  [Key(71)]
  public bool PrioritisedAsBuildingObstruction;
  [Key(72)]
  public bool WeedsAndRubblePlaced;
  [Key(73)]
  public List<StructuresData.Ranchable_Animal> Animals = new List<StructuresData.Ranchable_Animal>();
  [Key(74)]
  public DayPhase TargetPhase;
  [Key(75)]
  public GateType GateType;
  [Key(76)]
  public bool CanBecomeRotten = true;
  [Key(77)]
  public bool Rotten;
  [Key(78)]
  public bool Burned;
  [Key(79)]
  public bool Eaten;
  [Key(80 /*0x50*/)]
  public int GatheringEndPhase = -1;
  [Key(81)]
  public bool IsSapling;
  [Key(82)]
  public float GrowthStage;
  [Key(83)]
  public bool CanRegrow = true;
  [Key(84)]
  public bool BenefitedFromFertilizer;
  [Key(85)]
  public int RemainingHarvests;
  [Key(86)]
  public bool Withered;
  [Key(87)]
  public string Animation = "";
  [Key(88)]
  public float StartingScale;
  [Key(89)]
  public bool Picked;
  [Key(90)]
  public bool Watered;
  [Key(91)]
  public int WateredCount;
  [Key(92)]
  public bool HasBird;
  [Key(93)]
  public int TotalPoops;
  [Key(94)]
  public InventoryItem.ITEM_TYPE SignPostItem;
  [Key(95)]
  public bool GivenHealth;
  [Key(96 /*0x60*/)]
  public StructuresData.EggData EggInfo;
  [Key(97)]
  public bool HasEgg;
  [Key(98)]
  public bool EggReady;
  [Key(99)]
  public bool MatingFailed;
  [Key(100)]
  public int WeedType = -1;
  [Key(101)]
  public float LastPrayTime = -1f;
  [Key(102)]
  public int Fuel;
  [Key(103)]
  public int MaxFuel = 50;
  [Key(104)]
  public bool FullyFueled;
  [Key(105)]
  public int FuelDepletionDayTimestamp = -1;
  [Key(106)]
  public bool onlyDepleteWhenFullyFueled;
  [Key(107)]
  public DayPhase PhaseAddedFuel = DayPhase.None;
  [Key(108)]
  public List<int> QueuedRefineryVariants = new List<int>();
  [Key(109)]
  public List<InventoryItem.ITEM_TYPE> QueuedResources = new List<InventoryItem.ITEM_TYPE>();
  [Key(110)]
  public List<StructuresData.ClothingStruct> QueuedClothings = new List<StructuresData.ClothingStruct>();
  [Key(111)]
  public List<StructuresData.ClothingStruct> AllClothing = new List<StructuresData.ClothingStruct>();
  [Key(112 /*0x70*/)]
  public List<StructuresData.ClothingStruct> ReservedClothing = new List<StructuresData.ClothingStruct>();
  [Key(113)]
  public StructuresData.ClothingStruct CurrentTailoringClothes;
  [Key(114)]
  public List<Interaction_Kitchen.QueuedMeal> QueuedMeals = new List<Interaction_Kitchen.QueuedMeal>();
  [Key(115)]
  public Interaction_Kitchen.QueuedMeal CurrentCookingMeal;
  [Key(116)]
  public Dictionary<int, int> ReservedFollowers = new Dictionary<int, int>();
  [Key(117)]
  public float WeaponUpgradePointProgress;
  [Key(118)]
  public float WeaponUpgradePointDuration;
  [Key(119)]
  public WeaponUpgradeSystem.WeaponType CurrentUnlockingWeaponType;
  [Key(120)]
  public WeaponUpgradeSystem.WeaponUpgradeType CurrentUnlockingUpgradeType;
  [Key(121)]
  public bool DefrostedCrop;
  public static System.Action OnResearchBegin;
  public static List<StructureBrain.TYPES> AllStructures = new List<StructureBrain.TYPES>()
  {
    StructureBrain.TYPES.BED,
    StructureBrain.TYPES.COOKING_FIRE,
    StructureBrain.TYPES.HARVEST_TOTEM,
    StructureBrain.TYPES.SCARECROW,
    StructureBrain.TYPES.COLLECTED_RESOURCES_CHEST,
    StructureBrain.TYPES.DANCING_FIREPIT,
    StructureBrain.TYPES.BODY_PIT,
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
    StructureBrain.TYPES.JANITOR_STATION_2,
    StructureBrain.TYPES.MEDIC,
    StructureBrain.TYPES.RACING_GATE,
    StructureBrain.TYPES.ROTSTONE_MINE,
    StructureBrain.TYPES.ROTSTONE_MINE_2,
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
    StructureBrain.TYPES.SHARED_HOUSE,
    StructureBrain.TYPES.SILO_SEED,
    StructureBrain.TYPES.SILO_FERTILISER,
    StructureBrain.TYPES.POOP_BUCKET,
    StructureBrain.TYPES.SEED_BUCKET,
    StructureBrain.TYPES.SURVEILLANCE,
    StructureBrain.TYPES.FISHING_HUT_2,
    StructureBrain.TYPES.OUTHOUSE_2,
    StructureBrain.TYPES.SCARECROW_2,
    StructureBrain.TYPES.HARVEST_TOTEM_2,
    StructureBrain.TYPES.CHOPPING_SHRINE,
    StructureBrain.TYPES.MINING_SHRINE,
    StructureBrain.TYPES.FORAGING_SHRINE,
    StructureBrain.TYPES.KITCHEN,
    StructureBrain.TYPES.MORGUE_1,
    StructureBrain.TYPES.MORGUE_2,
    StructureBrain.TYPES.CRYPT_1,
    StructureBrain.TYPES.CRYPT_2,
    StructureBrain.TYPES.CRYPT_3,
    StructureBrain.TYPES.PUB,
    StructureBrain.TYPES.PUB_2,
    StructureBrain.TYPES.MATING_TENT,
    StructureBrain.TYPES.HATCHERY,
    StructureBrain.TYPES.HATCHERY_2,
    StructureBrain.TYPES.DAYCARE,
    StructureBrain.TYPES.KNUCKLEBONES_ARENA,
    StructureBrain.TYPES.WEATHER_VANE,
    StructureBrain.TYPES.VOLCANIC_SPA,
    StructureBrain.TYPES.WOOLY_SHACK,
    StructureBrain.TYPES.RANCH,
    StructureBrain.TYPES.RANCH_2,
    StructureBrain.TYPES.RANCH_FENCE,
    StructureBrain.TYPES.RANCH_TROUGH,
    StructureBrain.TYPES.RANCH_HUTCH,
    StructureBrain.TYPES.RANCH_CHOPPING_BLOCK,
    StructureBrain.TYPES.LOGISTICS,
    StructureBrain.TYPES.WOLF_TRAP,
    StructureBrain.TYPES.LIGHTNING_ROD,
    StructureBrain.TYPES.LIGHTNING_ROD_2,
    StructureBrain.TYPES.FURNACE_1,
    StructureBrain.TYPES.FURNACE_2,
    StructureBrain.TYPES.FURNACE_3,
    StructureBrain.TYPES.PROXIMITY_FURNACE,
    StructureBrain.TYPES.TOOLSHED,
    StructureBrain.TYPES.FARM_CROP_GROWER,
    StructureBrain.TYPES.TRAIT_MANIPULATOR_1,
    StructureBrain.TYPES.TRAIT_MANIPULATOR_2,
    StructureBrain.TYPES.TRAIT_MANIPULATOR_3,
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
    StructureBrain.TYPES.TAILOR,
    StructureBrain.TYPES.SHRINE_DISCIPLE_BOOST,
    StructureBrain.TYPES.SHRINE_DISCIPLE_COLLECTION,
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
    StructureBrain.TYPES.DECORATION_DLC_ROT_BOTTLE,
    StructureBrain.TYPES.DECORATION_DLC_ROT_BUCKET,
    StructureBrain.TYPES.DECORATION_DLC_ROT_CAGE1,
    StructureBrain.TYPES.DECORATION_DLC_ROT_CAGE2,
    StructureBrain.TYPES.DECORATION_DLC_ROT_CAULDRON,
    StructureBrain.TYPES.DECORATION_DLC_ROT_DIORAMA,
    StructureBrain.TYPES.DECORATION_DLC_ROT_FIREMACHINE,
    StructureBrain.TYPES.DECORATION_DLC_ROT_FLOOR,
    StructureBrain.TYPES.DECORATION_DLC_ROT_IRONMAIDEN,
    StructureBrain.TYPES.DECORATION_DLC_ROT_LUMP1,
    StructureBrain.TYPES.DECORATION_DLC_ROT_LUMP2,
    StructureBrain.TYPES.DECORATION_DLC_ROT_PILLAR1,
    StructureBrain.TYPES.DECORATION_DLC_ROT_PILLAR2,
    StructureBrain.TYPES.DECORATION_DLC_ROT_STONE1,
    StructureBrain.TYPES.DECORATION_DLC_ROT_STONE2,
    StructureBrain.TYPES.DECORATION_DLC_ROT_TENTACLE,
    StructureBrain.TYPES.DECORATION_DLC_ROT_WALL,
    StructureBrain.TYPES.DECORATION_DLC_WOLF_BULB,
    StructureBrain.TYPES.DECORATION_DLC_WOLF_CULTIST1,
    StructureBrain.TYPES.DECORATION_DLC_WOLF_CULTIST2,
    StructureBrain.TYPES.DECORATION_DLC_WOLF_DIORAMA,
    StructureBrain.TYPES.DECORATION_DLC_WOLF_FIREPIT,
    StructureBrain.TYPES.DECORATION_DLC_WOLF_LAMPPOST,
    StructureBrain.TYPES.DECORATION_DLC_WOLF_PILLAR1,
    StructureBrain.TYPES.DECORATION_DLC_WOLF_PILLAR2,
    StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE1,
    StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE2,
    StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE3,
    StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE4,
    StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE5,
    StructureBrain.TYPES.DECORATION_DLC_WOLF_TESLA,
    StructureBrain.TYPES.DECORATION_DLC_WOLF_TREE1,
    StructureBrain.TYPES.DECORATION_DLC_WOLF_WIRES,
    StructureBrain.TYPES.DECORATION_EASTEREGG_WARRACKA,
    StructureBrain.TYPES.DECORATION_EASTEREGG_TURUA,
    StructureBrain.TYPES.DECORATION_EASTEREGG_HAROSTATUE,
    StructureBrain.TYPES.DECORATION_EASTEREGG_EGG,
    StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_CHIMNEY_1,
    StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_CHIMNEY_2,
    StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_LAMPPOST,
    StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_CLOCK,
    StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_WALL,
    StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_PLANT,
    StructureBrain.TYPES.DECORATION_DLC_YNGYA_CANDLE,
    StructureBrain.TYPES.DECORATION_DLC_YNGYA_FLOWERBUCKET,
    StructureBrain.TYPES.DECORATION_DLC_YNGYA_STICKBUNDLE,
    StructureBrain.TYPES.DECORATION_DLC_YNGYA_TALLFLOWERS,
    StructureBrain.TYPES.DECORATION_DLC_YNGYA_TREEBUSH,
    StructureBrain.TYPES.DECORATION_DLC_YNGYA_TREEPOT,
    StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BUSH1,
    StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BUSH2,
    StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_FOUNTAIN,
    StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_SMALLSTATUE1,
    StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_SMALLSTATUE2,
    StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_STREETLAMP,
    StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_TREE,
    StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_WALL,
    StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_FLOOR,
    StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_WOLF,
    StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_YNGYA,
    StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_EXECUTIONER,
    StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_JUG,
    StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_RUG,
    StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BARREL1,
    StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BARREL2,
    StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_CANDLE,
    StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BELLS,
    StructureBrain.TYPES.TILE_WATER,
    StructureBrain.TYPES.DECORATION_SINFUL_STATUE,
    StructureBrain.TYPES.DECORATION_SINFUL_CRUCIFIX,
    StructureBrain.TYPES.DECORATION_SINFUL_FLOWERS1,
    StructureBrain.TYPES.DECORATION_SINFUL_FLOWERS2,
    StructureBrain.TYPES.DECORATION_SINFUL_SKULL,
    StructureBrain.TYPES.DECORATION_SINFUL_INCENSE,
    StructureBrain.TYPES.DECORATION_CNY_LANTERN,
    StructureBrain.TYPES.DECORATION_CNY_DRAGON,
    StructureBrain.TYPES.DECORATION_CNY_TREE,
    StructureBrain.TYPES.DECORATION_PILGRIM_WALL,
    StructureBrain.TYPES.DECORATION_PILGRIM_BONSAI,
    StructureBrain.TYPES.DECORATION_PILGRIM_LANTERN,
    StructureBrain.TYPES.DECORATION_PILGRIM_PAGODA,
    StructureBrain.TYPES.DECORATION_PILGRIM_VASE,
    StructureBrain.TYPES.DECORATION_GNOME1,
    StructureBrain.TYPES.DECORATION_GNOME2,
    StructureBrain.TYPES.DECORATION_GNOME3,
    StructureBrain.TYPES.DECORATION_GOAT_LANTERN,
    StructureBrain.TYPES.DECORATION_GOAT_STATUE,
    StructureBrain.TYPES.DECORATION_GOAT_PLANT,
    StructureBrain.TYPES.DECORATION_TWITCH_FLAG_CROWN,
    StructureBrain.TYPES.DECORATION_TWITCH_MUSHROOM_BAG,
    StructureBrain.TYPES.DECORATION_TWITCH_ROSE_BUSH,
    StructureBrain.TYPES.DECORATION_TWITCH_STONE_FLAG,
    StructureBrain.TYPES.DECORATION_TWITCH_STONE_STATUE,
    StructureBrain.TYPES.DECORATION_TWITCH_WOODEN_GUARDIAN,
    StructureBrain.TYPES.DECORATION_CHRISTMAS_TREE,
    StructureBrain.TYPES.DECORATION_CHRISTMAS_SNOWMAN,
    StructureBrain.TYPES.DECORATION_CHRISTMAS_CANDLE,
    StructureBrain.TYPES.DECORATION_CHRISTMAS_CANDYCANE,
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
    StructureBrain.TYPES.DECORATION_APPLE_BUSH,
    StructureBrain.TYPES.DECORATION_APPLE_LANTERN,
    StructureBrain.TYPES.DECORATION_APPLE_STATUE,
    StructureBrain.TYPES.DECORATION_APPLE_VASE,
    StructureBrain.TYPES.DECORATION_APPLE_WELL,
    StructureBrain.TYPES.DECORATION_OLDFAITH_CRYSTAL,
    StructureBrain.TYPES.DECORATION_OLDFAITH_FLAG,
    StructureBrain.TYPES.DECORATION_OLDFAITH_FOUNTAIN,
    StructureBrain.TYPES.DECORATION_OLDFAITH_IRONMAIDEN,
    StructureBrain.TYPES.DECORATION_OLDFAITH_SHRINE,
    StructureBrain.TYPES.DECORATION_OLDFAITH_TORCH,
    StructureBrain.TYPES.DECORATION_OLDFAITH_WALL,
    StructureBrain.TYPES.TILE_OLDFAITH,
    StructureBrain.TYPES.DECORATION_DST_ALCHEMY,
    StructureBrain.TYPES.DECORATION_DST_DEERCLOPS,
    StructureBrain.TYPES.DECORATION_DST_MARBLETREE,
    StructureBrain.TYPES.DECORATION_DST_PIGSTICK,
    StructureBrain.TYPES.DECORATION_DST_SCIENCEMACHINE,
    StructureBrain.TYPES.DECORATION_DST_TREE,
    StructureBrain.TYPES.DECORATION_DST_BEEFALOSKELETON,
    StructureBrain.TYPES.DECORATION_DST_GLOMMERSTATUE,
    StructureBrain.TYPES.DECORATION_PALWORLD_LAMB,
    StructureBrain.TYPES.DECORATION_PALWORLD_LANTERN,
    StructureBrain.TYPES.DECORATION_PALWORLD_PLANT,
    StructureBrain.TYPES.DECORATION_PALWORLD_STATUE,
    StructureBrain.TYPES.DECORATION_PALWORLD_TREE,
    StructureBrain.TYPES.DECORATION_FLOWER_BOTTLE,
    StructureBrain.TYPES.DECORATION_MUSHROOM_CANDLE_2,
    StructureBrain.TYPES.DECORATION_LEAFY_SCULPTURE,
    StructureBrain.TYPES.DECORATION_VIDEO,
    StructureBrain.TYPES.LEADER_TENT
  };
  public static List<StructureBrain.TYPES> HiddenStructuresUntilUnlocked = new List<StructureBrain.TYPES>()
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
    StructureBrain.TYPES.DECORATION_OLDFAITH_CRYSTAL,
    StructureBrain.TYPES.DECORATION_OLDFAITH_FLAG,
    StructureBrain.TYPES.DECORATION_OLDFAITH_FOUNTAIN,
    StructureBrain.TYPES.DECORATION_OLDFAITH_IRONMAIDEN,
    StructureBrain.TYPES.DECORATION_OLDFAITH_SHRINE,
    StructureBrain.TYPES.DECORATION_OLDFAITH_TORCH,
    StructureBrain.TYPES.DECORATION_OLDFAITH_WALL,
    StructureBrain.TYPES.TILE_OLDFAITH,
    StructureBrain.TYPES.DECORATION_BOSS_TROPHY_5,
    StructureBrain.TYPES.DECORATION_VIDEO,
    StructureBrain.TYPES.DECORATION_TWITCH_FLAG_CROWN,
    StructureBrain.TYPES.DECORATION_TWITCH_MUSHROOM_BAG,
    StructureBrain.TYPES.DECORATION_TWITCH_ROSE_BUSH,
    StructureBrain.TYPES.DECORATION_TWITCH_STONE_FLAG,
    StructureBrain.TYPES.DECORATION_TWITCH_STONE_STATUE,
    StructureBrain.TYPES.DECORATION_TWITCH_WOODEN_GUARDIAN,
    StructureBrain.TYPES.DECORATION_CHRISTMAS_TREE,
    StructureBrain.TYPES.DECORATION_CHRISTMAS_SNOWMAN,
    StructureBrain.TYPES.DECORATION_CHRISTMAS_CANDLE,
    StructureBrain.TYPES.DECORATION_CHRISTMAS_CANDYCANE,
    StructureBrain.TYPES.DECORATION_HALLOWEEN_CANDLE,
    StructureBrain.TYPES.DECORATION_HALLOWEEN_PUMPKIN,
    StructureBrain.TYPES.DECORATION_HALLOWEEN_TREE,
    StructureBrain.TYPES.DECORATION_HALLOWEEN_SKULL,
    StructureBrain.TYPES.DECORATION_DST_ALCHEMY,
    StructureBrain.TYPES.DECORATION_DST_DEERCLOPS,
    StructureBrain.TYPES.DECORATION_DST_MARBLETREE,
    StructureBrain.TYPES.DECORATION_DST_PIGSTICK,
    StructureBrain.TYPES.DECORATION_DST_SCIENCEMACHINE,
    StructureBrain.TYPES.DECORATION_DST_TREE,
    StructureBrain.TYPES.DECORATION_DST_WALL,
    StructureBrain.TYPES.DECORATION_DST_BEEFALOSKELETON,
    StructureBrain.TYPES.DECORATION_DST_GLOMMERSTATUE,
    StructureBrain.TYPES.DECORATION_PALWORLD_LAMB,
    StructureBrain.TYPES.DECORATION_PALWORLD_LANTERN,
    StructureBrain.TYPES.DECORATION_PALWORLD_PLANT,
    StructureBrain.TYPES.DECORATION_PALWORLD_STATUE,
    StructureBrain.TYPES.DECORATION_PALWORLD_TREE,
    StructureBrain.TYPES.DECORATION_FLOWER_BOTTLE,
    StructureBrain.TYPES.DECORATION_MUSHROOM_CANDLE_2,
    StructureBrain.TYPES.DECORATION_LEAFY_SCULPTURE,
    StructureBrain.TYPES.DECORATION_GNOME1,
    StructureBrain.TYPES.DECORATION_GNOME2,
    StructureBrain.TYPES.DECORATION_GNOME3,
    StructureBrain.TYPES.DECORATION_SINFUL_STATUE,
    StructureBrain.TYPES.DECORATION_SINFUL_CRUCIFIX,
    StructureBrain.TYPES.DECORATION_SINFUL_FLOWERS1,
    StructureBrain.TYPES.DECORATION_SINFUL_FLOWERS2,
    StructureBrain.TYPES.DECORATION_SINFUL_SKULL,
    StructureBrain.TYPES.DECORATION_SINFUL_INCENSE,
    StructureBrain.TYPES.DECORATION_GOAT_LANTERN,
    StructureBrain.TYPES.DECORATION_GOAT_STATUE,
    StructureBrain.TYPES.DECORATION_GOAT_PLANT,
    StructureBrain.TYPES.LEADER_TENT,
    StructureBrain.TYPES.DECORATION_CNY_DRAGON,
    StructureBrain.TYPES.DECORATION_CNY_LANTERN,
    StructureBrain.TYPES.DECORATION_CNY_TREE,
    StructureBrain.TYPES.DECORATION_PILGRIM_WALL,
    StructureBrain.TYPES.DECORATION_PILGRIM_BONSAI,
    StructureBrain.TYPES.DECORATION_PILGRIM_LANTERN,
    StructureBrain.TYPES.DECORATION_PILGRIM_PAGODA,
    StructureBrain.TYPES.DECORATION_PILGRIM_VASE,
    StructureBrain.TYPES.DECORATION_FLOWER_CART,
    StructureBrain.TYPES.DECORATION_POST_BOX,
    StructureBrain.TYPES.DECORATION_STONE_CANDLE_LAMP
  };
  public const float GoldModifier = 2f;

  [IgnoreMember]
  public bool IsDeletable
  {
    get
    {
      if (this.Type != StructureBrain.TYPES.BODY_PIT && this.Type != StructureBrain.TYPES.GRAVE && this.Type != StructureBrain.TYPES.CRYPT_1 && this.Type != StructureBrain.TYPES.CRYPT_2 && this.Type != StructureBrain.TYPES.CRYPT_3 && this.Type != StructureBrain.TYPES.MORGUE_1 && this.Type != StructureBrain.TYPES.MORGUE_2 || this.FollowerID == -1 && this.MultipleFollowerIDs.Count <= 0)
      {
        if (this.Type == StructureBrain.TYPES.FARM_PLOT)
        {
          InventoryItem plantedSeed = Structures_FarmerPlot.GetPlantedSeed(this.Inventory);
          if ((plantedSeed != null ? (plantedSeed.type == 160 /*0xA0*/ ? 1 : 0) : 0) != 0)
            goto label_3;
        }
        return this.isDeletable;
      }
label_3:
      return false;
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
  [IgnoreMember]
  public List<PlacementRegion.TileGridTile> Grid => this.grid;

  [IgnoreMember]
  public bool IsGatheringActive
  {
    get
    {
      return this.GatheringEndPhase != -1 && TimeManager.CurrentPhase != (DayPhase) this.GatheringEndPhase;
    }
  }

  [IgnoreMember]
  public bool IsFull => this.Inventory.Count >= Structures_Outhouse.Capacity(this.Type);

  [IgnoreMember]
  public bool WeaponUpgradingInProgress
  {
    get
    {
      return (double) this.WeaponUpgradePointDuration > 0.0 && (double) this.WeaponUpgradePointProgress < (double) this.WeaponUpgradePointDuration;
    }
  }

  [IgnoreMember]
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
    if (Type == StructureBrain.TYPES.MISSIONARY || Type == StructureBrain.TYPES.MISSIONARY_II || Type == StructureBrain.TYPES.MISSIONARY_III)
    {
      int num = 1;
      switch (Type)
      {
        case StructureBrain.TYPES.MISSIONARY_II:
          num = 2;
          break;
        case StructureBrain.TYPES.MISSIONARY_III:
          num = 3;
          break;
      }
      string str1 = " <br><br><sprite name=\"icon_wood\"> <sprite name=\"icon_stone\"> <sprite name=\"icon_blackgold\"> <sprite name=\"icon_meat\">";
      if (Type == StructureBrain.TYPES.MISSIONARY_II || Type == StructureBrain.TYPES.MISSIONARY_III)
        str1 += " <sprite name=\"icon_bones\"> <sprite name=\"icon_Followers\"> <sprite name=\"icon_seed\">";
      if (Type == StructureBrain.TYPES.MISSIONARY_III)
        str1 += " <sprite name=\"icon_LogRefined\"> <sprite name=\"icon_StoneRefined\">";
      string str2 = $"{num.ToString()}x {ScriptLocalization.Inventory.FOLLOWERS} <sprite name=\"icon_Followers\">";
      return $"{LocalizationManager.GetTranslation($"Structures/{Type}/Description")} <br><color=#FFD201>{str2}</color>{str1}";
    }
    if (Type == StructureBrain.TYPES.LUMBERJACK_STATION_2 || Type == StructureBrain.TYPES.BLOODSTONE_MINE_2)
    {
      string str = (Type == StructureBrain.TYPES.LUMBERJACK_STATION_2 ? "<sprite name=\"icon_wood\">" : "<sprite name=\"icon_stone\">") + " <sprite name=\"icon_FaithDoubleUp\">";
      return $"{LocalizationManager.GetTranslation($"Structures/{Type}/Description")}<br><br>{str}";
    }
    if (Type == StructureBrain.TYPES.REFINERY_2)
    {
      string str = "<sprite name=\"icon_GoldRefined\"><sprite name=\"icon_LogRefined\"><sprite name=\"icon_StoneRefined\"> <sprite name=\"icon_FaithDoubleUp\">";
      return $"{LocalizationManager.GetTranslation($"Structures/{Type}/Description")}<br><br>{str}";
    }
    if (Type == StructureBrain.TYPES.DECORATION_MUSHROOM_SCULPTURE && DataManager.Instance.SozoDecorationQuestActive)
    {
      string str = $"{ScriptLocalization.Objectives_GroupTitles.VisitSozo.Colour(Color.yellow)}: {string.Format(ScriptLocalization.Objectives.BuildStructure, (object) StructuresData.LocalizedName(Type))}";
      return $"{LocalizationManager.GetTranslation($"Structures/{Type}/Description")}<br><br>{str}";
    }
    if (Type == StructureBrain.TYPES.CRYPT_1 || Type == StructureBrain.TYPES.CRYPT_2 || Type == StructureBrain.TYPES.CRYPT_3)
    {
      string str = $"{LocalizeIntegration.ReverseText(Structures_Crypt.GetCapacity(Type).ToString())}x {ScriptLocalization.Inventory.FOLLOWERS} <sprite name=\"icon_Followers\">";
      return $"{LocalizationManager.GetTranslation($"Structures/{Type}/Description")}<br><br><color=#FFD201>{str}</color>";
    }
    if (Type == StructureBrain.TYPES.MORGUE_1 || Type == StructureBrain.TYPES.MORGUE_2)
    {
      string str = $"{LocalizeIntegration.ReverseText(Structures_Morgue.GetCapacity(Type).ToString())}x {ScriptLocalization.Inventory.FOLLOWERS} <sprite name=\"icon_Followers\">";
      return $"{LocalizationManager.GetTranslation($"Structures/{Type}/Description")}<br><br><color=#FFD201>{str}</color>";
    }
    Debug.Log((object) ("Type: " + Type.ToString()));
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
          PrefabPath = "Prefabs/Structures/Buildings/Building Follower Kitchen"
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
      case StructureBrain.TYPES.COTTON_BUSH:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/CottonBush",
          DontLoadMe = true,
          ProgressTarget = 10f,
          MultipleLootToDrop = new List<InventoryItem.ITEM_TYPE>()
          {
            InventoryItem.ITEM_TYPE.COTTON,
            InventoryItem.ITEM_TYPE.SEED_COTTON
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
          RequiresType = StructureBrain.TYPES.FARM_STATION,
          ProgressTarget = 20f
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
        infoByType = new StructuresData[2]
        {
          new StructuresData()
          {
            PrefabPath = "Prefabs/Structures/Other/Rubble",
            LootToDrop = InventoryItem.ITEM_TYPE.STONE
          },
          new StructuresData()
          {
            PrefabPath = "Prefabs/Structures/Other/Rubble Rot",
            LootToDrop = InventoryItem.ITEM_TYPE.MAGMA_STONE
          }
        }[variantIndex];
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
          ProgressTarget = 10f,
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
          ProgressTarget = 10f,
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
          ProgressTarget = 10f,
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
          ProgressTarget = 10f,
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
          ProgressTarget = 10f,
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
          ProgressTarget = 10f,
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
          ProgressTarget = 10f,
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
      case StructureBrain.TYPES.MORGUE_1:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Morgue 1"
        };
        break;
      case StructureBrain.TYPES.MORGUE_2:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Morgue 2",
          IsUpgrade = true,
          UpgradeFromType = StructureBrain.TYPES.MORGUE_1
        };
        break;
      case StructureBrain.TYPES.CRYPT_1:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Crypt 1"
        };
        break;
      case StructureBrain.TYPES.CRYPT_2:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Crypt 2",
          IsUpgrade = true,
          UpgradeFromType = StructureBrain.TYPES.CRYPT_1
        };
        break;
      case StructureBrain.TYPES.CRYPT_3:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Crypt 3",
          IsUpgrade = true,
          UpgradeFromType = StructureBrain.TYPES.CRYPT_2
        };
        break;
      case StructureBrain.TYPES.SHARED_HOUSE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Shared House"
        };
        break;
      case StructureBrain.TYPES.DECORATION_OLDFAITH_CRYSTAL:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration OldFaith Crystal"
        };
        break;
      case StructureBrain.TYPES.DECORATION_OLDFAITH_FLAG:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration OldFaith Flag"
        };
        break;
      case StructureBrain.TYPES.DECORATION_OLDFAITH_FOUNTAIN:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration OldFaith Fountain"
        };
        break;
      case StructureBrain.TYPES.DECORATION_OLDFAITH_IRONMAIDEN:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration OldFaith IronMaiden"
        };
        break;
      case StructureBrain.TYPES.DECORATION_OLDFAITH_SHRINE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration OldFaith Shrine"
        };
        break;
      case StructureBrain.TYPES.DECORATION_OLDFAITH_TORCH:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration OldFaith Torch"
        };
        break;
      case StructureBrain.TYPES.DECORATION_OLDFAITH_WALL:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Wall OldFaith"
        };
        break;
      case StructureBrain.TYPES.TILE_OLDFAITH:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Path OldFaith"
        };
        break;
      case StructureBrain.TYPES.WEBBER_SKULL:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Webber Skull",
          IgnoreGrid = true,
          CanBeMoved = false,
          isDeletable = false
        };
        break;
      case StructureBrain.TYPES.DECORATION_DST_ALCHEMY:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration DST AlchemyEngine"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DST_DEERCLOPS:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration DST Deerclops"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DST_MARBLETREE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration DST Marble Tree"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DST_PIGSTICK:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration DST PigStick"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DST_SCIENCEMACHINE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration DST ScienceMachine"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DST_TREE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration DST Tree"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DST_WALL:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration DST Wall Hay"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DST_GLOMMERSTATUE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration DST GlommerStatue"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DST_BEEFALOSKELETON:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration DST Beefalo"
        };
        break;
      case StructureBrain.TYPES.TAILOR:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Tailor"
        };
        break;
      case StructureBrain.TYPES.MEAL_EGG:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/Meal Egg",
          IgnoreGrid = true
        };
        break;
      case StructureBrain.TYPES.SHRINE_WEATHER_BLIZZARD:
      case StructureBrain.TYPES.SHRINE_WEATHER_HEATWAVE:
      case StructureBrain.TYPES.SHRINE_WEATHER_TYPHOON:
        infoByType = new StructuresData()
        {
          DontLoadMe = true
        };
        break;
      case StructureBrain.TYPES.POOP_BUCKET:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Dump Fertiliser"
        };
        break;
      case StructureBrain.TYPES.SEED_BUCKET:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Dump Seed"
        };
        break;
      case StructureBrain.TYPES.JANITOR_STATION_2:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Janitor Station 2",
          IsUpgrade = true,
          UpgradeFromType = StructureBrain.TYPES.JANITOR_STATION
        };
        break;
      case StructureBrain.TYPES.SHRINE_DISCIPLE_BOOST:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Shrine Disciple Boost"
        };
        break;
      case StructureBrain.TYPES.POOP_GOLD:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Poop Gold",
          IgnoreGrid = true,
          CanBeMoved = false,
          isDeletable = false
        };
        break;
      case StructureBrain.TYPES.POOP_RAINBOW:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Poop Rainbow",
          IgnoreGrid = true,
          CanBeMoved = false,
          isDeletable = false
        };
        break;
      case StructureBrain.TYPES.POOP_MASSIVE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Poop Massive",
          IgnoreGrid = true,
          CanBeMoved = false,
          isDeletable = false
        };
        break;
      case StructureBrain.TYPES.POOP_DEVOTION:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Poop Devotion",
          IgnoreGrid = true,
          CanBeMoved = false,
          isDeletable = false
        };
        break;
      case StructureBrain.TYPES.POOP_PET:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Poop Pet",
          IgnoreGrid = true,
          CanBeMoved = false,
          isDeletable = false
        };
        break;
      case StructureBrain.TYPES.POOP_GLOW:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Poop Glow",
          IgnoreGrid = true,
          CanBeMoved = false,
          isDeletable = false
        };
        break;
      case StructureBrain.TYPES.PUB:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Pub"
        };
        break;
      case StructureBrain.TYPES.SHRINE_PLEASURE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Shrine Pleasure"
        };
        break;
      case StructureBrain.TYPES.HOPS_BUSH:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/HopsBush",
          DontLoadMe = true,
          ProgressTarget = 10f,
          MultipleLootToDrop = new List<InventoryItem.ITEM_TYPE>()
          {
            InventoryItem.ITEM_TYPE.HOPS,
            InventoryItem.ITEM_TYPE.SEED_HOPS
          },
          MultipleLootToDropChance = new List<int>()
          {
            85,
            15
          },
          LootCountToDropRange = new Vector2Int(1, 2),
          CropLootCountToDropRange = new Vector2Int(1, 2)
        };
        break;
      case StructureBrain.TYPES.GRAPES_BUSH:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/GrapesBush",
          DontLoadMe = true,
          ProgressTarget = 20f,
          MultipleLootToDrop = new List<InventoryItem.ITEM_TYPE>()
          {
            InventoryItem.ITEM_TYPE.GRAPES,
            InventoryItem.ITEM_TYPE.SEED_GRAPES
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
      case StructureBrain.TYPES.EGG_FOLLOWER:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Egg Follower",
          IgnoreGrid = true,
          CanBeMoved = false,
          isDeletable = false
        };
        break;
      case StructureBrain.TYPES.SHRINE_DISCIPLE_COLLECTION:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Shrine Disciple Collection"
        };
        break;
      case StructureBrain.TYPES.HATCHERY:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Hatchery",
          ProgressTarget = 3f
        };
        break;
      case StructureBrain.TYPES.PUB_2:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Pub 2"
        };
        break;
      case StructureBrain.TYPES.HATCHERY_2:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Hatchery 2",
          ProgressTarget = 2f
        };
        break;
      case StructureBrain.TYPES.DECORATION_GNOME1:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Gnome1"
        };
        break;
      case StructureBrain.TYPES.DECORATION_GNOME2:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Gnome2"
        };
        break;
      case StructureBrain.TYPES.DECORATION_GNOME3:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Gnome3"
        };
        break;
      case StructureBrain.TYPES.DECORATION_SINFUL_STATUE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Sinful Statue"
        };
        break;
      case StructureBrain.TYPES.DECORATION_SINFUL_CRUCIFIX:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Sinful Crucifix"
        };
        break;
      case StructureBrain.TYPES.DECORATION_SINFUL_FLOWERS1:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Sinful Flowers1"
        };
        break;
      case StructureBrain.TYPES.DECORATION_SINFUL_FLOWERS2:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Sinful Flowers2"
        };
        break;
      case StructureBrain.TYPES.DECORATION_SINFUL_SKULL:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Sinful Skull"
        };
        break;
      case StructureBrain.TYPES.DECORATION_SINFUL_INCENSE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Sinful Incense"
        };
        break;
      case StructureBrain.TYPES.DECORATION_CNY_LANTERN:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration CNY Lantern"
        };
        break;
      case StructureBrain.TYPES.DECORATION_CNY_DRAGON:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration CNY Dragon"
        };
        break;
      case StructureBrain.TYPES.DECORATION_CNY_TREE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration CNY Tree"
        };
        break;
      case StructureBrain.TYPES.SOZO_BUSH:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/Sozo Crop",
          DontLoadMe = true,
          ProgressTarget = 10f,
          MultipleLootToDropChance = new List<int>()
          {
            85,
            15
          },
          LootCountToDropRange = new Vector2Int(3, 4),
          CropLootCountToDropRange = new Vector2Int(3, 4)
        };
        break;
      case StructureBrain.TYPES.LEADER_TENT:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building_LeaderTent"
        };
        break;
      case StructureBrain.TYPES.DAYCARE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Daycare"
        };
        break;
      case StructureBrain.TYPES.DECORATION_PILGRIM_WALL:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Pilgrim Bamboo Wall"
        };
        break;
      case StructureBrain.TYPES.DECORATION_PILGRIM_BONSAI:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Pilgrim Bonsai"
        };
        break;
      case StructureBrain.TYPES.DECORATION_PILGRIM_LANTERN:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Pilgrim Lantern"
        };
        break;
      case StructureBrain.TYPES.DECORATION_PILGRIM_PAGODA:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Pilgrim Pagoda"
        };
        break;
      case StructureBrain.TYPES.DECORATION_PILGRIM_VASE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Pilgrim Vase"
        };
        break;
      case StructureBrain.TYPES.DECORATION_GOAT_LANTERN:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Goat Lantern"
        };
        break;
      case StructureBrain.TYPES.DECORATION_GOAT_STATUE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Goat Statue"
        };
        break;
      case StructureBrain.TYPES.DECORATION_GOAT_PLANT:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Goat Plant"
        };
        break;
      case StructureBrain.TYPES.WEATHER_VANE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Weather Vane",
          isDeletable = false
        };
        break;
      case StructureBrain.TYPES.WOOLY_SHACK:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Wooly Shack"
        };
        break;
      case StructureBrain.TYPES.VOLCANIC_SPA:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Volcanic Spa"
        };
        break;
      case StructureBrain.TYPES.SNOW_FRUIT_BUSH:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/SnowFruitBush",
          DontLoadMe = true,
          ProgressTarget = 20f,
          MultipleLootToDrop = new List<InventoryItem.ITEM_TYPE>()
          {
            InventoryItem.ITEM_TYPE.SNOW_FRUIT
          },
          MultipleLootToDropChance = new List<int>()
          {
            85,
            15
          },
          LootCountToDropRange = new Vector2Int(1, 1),
          CropLootCountToDropRange = new Vector2Int(1, 1)
        };
        break;
      case StructureBrain.TYPES.MEAL_SPICY:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/Meal Spicy",
          IgnoreGrid = true
        };
        break;
      case StructureBrain.TYPES.CHILLI_BUSH:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/ChilliBush",
          DontLoadMe = true,
          ProgressTarget = 20f,
          MultipleLootToDrop = new List<InventoryItem.ITEM_TYPE>()
          {
            InventoryItem.ITEM_TYPE.CHILLI,
            InventoryItem.ITEM_TYPE.SEED_CHILLI
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
      case StructureBrain.TYPES.GRASS_BUSH:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/GrassBush",
          DontLoadMe = true,
          ProgressTarget = 20f,
          MultipleLootToDrop = new List<InventoryItem.ITEM_TYPE>()
          {
            InventoryItem.ITEM_TYPE.GRASS,
            InventoryItem.ITEM_TYPE.GRASS
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
      case StructureBrain.TYPES.ICE_BLOCK:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/Ice Block",
          LootToDrop = InventoryItem.ITEM_TYPE.NONE,
          CanBeMoved = false,
          isDeletable = false
        };
        break;
      case StructureBrain.TYPES.RANCH:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Ranch"
        };
        break;
      case StructureBrain.TYPES.RANCH_FENCE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Ranch Fence"
        };
        break;
      case StructureBrain.TYPES.RANCH_TROUGH:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Ranch Trough"
        };
        break;
      case StructureBrain.TYPES.MEAL_SNOW_FRUIT:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/Meal Snow Fruit",
          IgnoreGrid = true
        };
        break;
      case StructureBrain.TYPES.MEDIC:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Medic"
        };
        break;
      case StructureBrain.TYPES.RANCH_2:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Ranch 2"
        };
        break;
      case StructureBrain.TYPES.RANCH_HUTCH:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Ranch Hutch"
        };
        break;
      case StructureBrain.TYPES.RACING_GATE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Racing Gate"
        };
        break;
      case StructureBrain.TYPES.SNOWMAN:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Snowman",
          CanBeMoved = false,
          isDeletable = false
        };
        break;
      case StructureBrain.TYPES.DECORATION_PALWORLD_LAMB:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Palworld Lamb"
        };
        break;
      case StructureBrain.TYPES.DECORATION_PALWORLD_LANTERN:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Palworld Lantern"
        };
        break;
      case StructureBrain.TYPES.DECORATION_PALWORLD_PLANT:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Palworld Plant"
        };
        break;
      case StructureBrain.TYPES.DECORATION_PALWORLD_STATUE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Palworld Statue"
        };
        break;
      case StructureBrain.TYPES.DECORATION_PALWORLD_TREE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Palworld Tree"
        };
        break;
      case StructureBrain.TYPES.LOGISTICS:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Logistics"
        };
        break;
      case StructureBrain.TYPES.WOLF_TRAP:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Wolf Trap"
        };
        break;
      case StructureBrain.TYPES.LIGHTNING_ROD:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Lightning Rod"
        };
        break;
      case StructureBrain.TYPES.FURNACE_1:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Furnace",
          isDeletable = false
        };
        break;
      case StructureBrain.TYPES.FURNACE_2:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Furnace 2",
          isDeletable = false
        };
        break;
      case StructureBrain.TYPES.FURNACE_3:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Furnace 3",
          isDeletable = false
        };
        break;
      case StructureBrain.TYPES.TOXIC_WASTE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Toxic Waste",
          LootCountToDrop = 0,
          LootToDrop = InventoryItem.ITEM_TYPE.SOOT
        };
        break;
      case StructureBrain.TYPES.LIGHTNING_ROD_2:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Lightning Rod 2"
        };
        break;
      case StructureBrain.TYPES.TOOLSHED:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Toolshed"
        };
        break;
      case StructureBrain.TYPES.FARM_CROP_GROWER:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Farm Crop Grower"
        };
        break;
      case StructureBrain.TYPES.SACRIFICE_TABLE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Sacrifice Table"
        };
        break;
      case StructureBrain.TYPES.POOP_ROTSTONE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Poop Rotstone",
          IgnoreGrid = true,
          CanBeMoved = false,
          isDeletable = false
        };
        break;
      case StructureBrain.TYPES.TRAIT_MANIPULATOR_1:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Trait Manipulator"
        };
        break;
      case StructureBrain.TYPES.TRAIT_MANIPULATOR_2:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Trait Manipulator 2"
        };
        break;
      case StructureBrain.TYPES.TRAIT_MANIPULATOR_3:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Trait Manipulator 3"
        };
        break;
      case StructureBrain.TYPES.ICE_SCULPTURE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Ice Sculpture",
          CanBeMoved = false,
          isDeletable = false
        };
        break;
      case StructureBrain.TYPES.SNOW_DRIFT:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/Snow Drift",
          LootToDrop = InventoryItem.ITEM_TYPE.NONE,
          DoesNotOccupyGrid = true,
          CanBeMoved = false,
          isDeletable = false
        };
        break;
      case StructureBrain.TYPES.SNOW_BALL:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/Snow Ball",
          LootToDrop = InventoryItem.ITEM_TYPE.NONE,
          DoesNotOccupyGrid = true,
          IgnoreGrid = true,
          CanBeMoved = false,
          isDeletable = false
        };
        break;
      case StructureBrain.TYPES.MEAL_MILK_BAD:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/Meal Milk Bad",
          IgnoreGrid = true
        };
        break;
      case StructureBrain.TYPES.MEAL_MILK_GOOD:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/Meal Milk Good",
          IgnoreGrid = true
        };
        break;
      case StructureBrain.TYPES.MEAL_MILK_GREAT:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Other/Meal Milk Great",
          IgnoreGrid = true
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_ROT_BOTTLE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Rot/Decoration Rot Bottle"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_ROT_BUCKET:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Rot/Decoration Rot Bucket"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_ROT_CAGE1:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Rot/Decoration Rot Cage 1"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_ROT_CAGE2:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Rot/Decoration Rot Cage 2"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_ROT_CAULDRON:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Rot/Decoration Rot Cauldron"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_ROT_DIORAMA:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Rot/Decoration Rot Diorama"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_ROT_FIREMACHINE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Rot/Decoration Rot FireMachine"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_ROT_FLOOR:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Rot/Decoration Path Rot Floor"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_ROT_IRONMAIDEN:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Rot/Decoration Rot IronMaiden"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_ROT_LUMP1:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Rot/Decoration Rot Lump 1"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_ROT_LUMP2:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Rot/Decoration Rot Lump 2"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_ROT_PILLAR1:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Rot/Decoration Rot Pillar 1"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_ROT_PILLAR2:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Rot/Decoration Rot Pillar 2"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_ROT_STONE1:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Rot/Decoration Rot RotStone 1"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_ROT_STONE2:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Rot/Decoration Rot RotStone 2"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_ROT_TENTACLE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Rot/Decoration Rot Tentacle"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_ROT_WALL:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Rot/Decoration Rot Wall"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_BULB:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Wolf/Decoration Wolf Bulb"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_CULTIST1:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Wolf/Decoration Wolf Cultist 1"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_CULTIST2:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Wolf/Decoration Wolf Cultist 2"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_DIORAMA:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Wolf/Decoration Wolf Diorama"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_FIREPIT:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Wolf/Decoration Wolf FirePit"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_LAMPPOST:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Wolf/Decoration Wolf LampPost"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_PILLAR1:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Wolf/Decoration Wolf Pillar 1"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_PILLAR2:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Wolf/Decoration Wolf Pillar 2"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE1:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Wolf/Decoration Wolf Statue 1"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE2:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Wolf/Decoration Wolf Statue 2"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE3:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Wolf/Decoration Wolf Statue 3"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE4:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Wolf/Decoration Wolf Statue 4"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_TESLA:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Wolf/Decoration Wolf Tesla"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_TREE1:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Wolf/Decoration Wolf Tree 1"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_WIRES:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Wolf/Decoration Wolf Wires"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_CHIMNEY_1:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/SteamPunk/Decoration Steampunk Chimney 1"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_CHIMNEY_2:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/SteamPunk/Decoration Steampunk Chimney 2"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_LAMPPOST:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/SteamPunk/Decoration Steampunk LampPost"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_CLOCK:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/SteamPunk/Decoration Steampunk Clock"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_WALL:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/SteamPunk/Decoration Steampunk Wall"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_PLANT:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/SteamPunk/Decoration Steampunk Plant"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_YNGYA_CANDLE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Yngya/Decoration Yngya Candle"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_YNGYA_FLOWERBUCKET:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Yngya/Decoration Yngya Flowerbucket"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_YNGYA_STICKBUNDLE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Yngya/Decoration Yngya StickBundle"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_YNGYA_TALLFLOWERS:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Yngya/Decoration Yngya Tallflowers"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_YNGYA_TREEBUSH:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Yngya/Decoration Yngya TreeBush"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_YNGYA_TREEPOT:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Yngya/Decoration Yngya Treepot"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BUSH1:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Woolhaven/Decoration Woolhaven Bush1"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BUSH2:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Woolhaven/Decoration Woolhaven Bush2"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_FOUNTAIN:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Woolhaven/Decoration Woolhaven Fountain"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_SMALLSTATUE1:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Woolhaven/Decoration Woolhaven SmallStatue1"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_SMALLSTATUE2:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Woolhaven/Decoration Woolhaven SmallStatue2"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_STREETLAMP:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Woolhaven/Decoration Woolhaven StreetLamp"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_TREE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Woolhaven/Decoration Woolhaven Tree"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_WALL:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Woolhaven/Decoration Woolhaven Wall"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_FLOOR:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Woolhaven/Decoration Path Woolhaven Floor"
        };
        break;
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_WOLF:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Wolf Shrine"
        };
        break;
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_YNGYA:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Yngya Shrine"
        };
        break;
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_EXECUTIONER:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Executioner Shrine"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE5:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Wolf/Decoration Wolf Statue 5"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BARREL1:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Woolhaven/Decoration Woolhaven Barrel1"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BARREL2:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Woolhaven/Decoration Woolhaven Barrel2"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BELLS:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Woolhaven/Decoration Woolhaven Bells"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_JUG:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Woolhaven/Decoration Woolhaven Jug"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_RUG:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Woolhaven/Decoration Woolhaven Rug"
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_CANDLE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/DLC/Woolhaven/Decoration Woolhaven Candle"
        };
        break;
      case StructureBrain.TYPES.RANCH_CHOPPING_BLOCK:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Ranch Chopping Block"
        };
        break;
      case StructureBrain.TYPES.PROXIMITY_FURNACE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Proximity Furnace"
        };
        break;
      case StructureBrain.TYPES.ROTSTONE_MINE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Rotstone Mine",
          LootToDrop = InventoryItem.ITEM_TYPE.MAGMA_STONE
        };
        break;
      case StructureBrain.TYPES.ROTSTONE_MINE_2:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Building Rotstone Mine 2",
          LootToDrop = InventoryItem.ITEM_TYPE.MAGMA_STONE
        };
        break;
      case StructureBrain.TYPES.DECORATION_APPLE_BUSH:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Apple Bush"
        };
        break;
      case StructureBrain.TYPES.DECORATION_APPLE_LANTERN:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Apple Lantern"
        };
        break;
      case StructureBrain.TYPES.DECORATION_APPLE_STATUE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Apple Statue"
        };
        break;
      case StructureBrain.TYPES.DECORATION_APPLE_VASE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Apple Vase"
        };
        break;
      case StructureBrain.TYPES.DECORATION_APPLE_WELL:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Apple Well"
        };
        break;
      case StructureBrain.TYPES.DECORATION_EASTEREGG_EGG:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Easter Egg Egg"
        };
        break;
      case StructureBrain.TYPES.DECORATION_EASTEREGG_HAROSTATUE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Easter Egg Haro"
        };
        break;
      case StructureBrain.TYPES.DECORATION_EASTEREGG_TURUA:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Easter Egg Turua"
        };
        break;
      case StructureBrain.TYPES.DECORATION_EASTEREGG_WARRACKA:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Easter Egg Warracka"
        };
        break;
      case StructureBrain.TYPES.ICE_SCULPTURE_1:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Ice Sculpture 1",
          CanBeMoved = false,
          isDeletable = false
        };
        break;
      case StructureBrain.TYPES.ICE_SCULPTURE_2:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Ice Sculpture 2",
          CanBeMoved = false,
          isDeletable = false
        };
        break;
      case StructureBrain.TYPES.ICE_SCULPTURE_3:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Ice Sculpture 3",
          CanBeMoved = false,
          isDeletable = false
        };
        break;
      case StructureBrain.TYPES.DECORATION_CHRISTMAS_TREE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Xmas Tree"
        };
        break;
      case StructureBrain.TYPES.DECORATION_CHRISTMAS_SNOWMAN:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Xmas Snowman"
        };
        break;
      case StructureBrain.TYPES.DECORATION_CHRISTMAS_CANDLE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Xmas Candle"
        };
        break;
      case StructureBrain.TYPES.DECORATION_CHRISTMAS_CANDYCANE:
        infoByType = new StructuresData()
        {
          PrefabPath = "Prefabs/Structures/Buildings/Decoration Xmas Cane"
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
    if (type <= StructureBrain.TYPES.COOKING_FIRE)
    {
      if (type != StructureBrain.TYPES.SHRINE && type != StructureBrain.TYPES.COOKING_FIRE)
        goto label_4;
    }
    else if (type != StructureBrain.TYPES.TEMPLE && type != StructureBrain.TYPES.FURNACE_1)
      goto label_4;
    return false;
label_4:
    return true;
  }

  public static bool RequiresRanchToBuild(StructureBrain.TYPES type)
  {
    switch (type)
    {
      case StructureBrain.TYPES.RANCH_FENCE:
      case StructureBrain.TYPES.RANCH_TROUGH:
      case StructureBrain.TYPES.RANCH_HUTCH:
      case StructureBrain.TYPES.RANCH_CHOPPING_BLOCK:
        return true;
      default:
        return false;
    }
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
      case StructureBrain.TYPES.MATING_TENT:
      case StructureBrain.TYPES.HATCHERY:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_MatingTent);
      case StructureBrain.TYPES.BLOODSTONE_MINE:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Economy_Mine);
      case StructureBrain.TYPES.BLOODSTONE_MINE_2:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Economy_MineII);
      case StructureBrain.TYPES.CONFESSION_BOOTH:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_ConfessionBooth);
      case StructureBrain.TYPES.DRUM_CIRCLE:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Drum);
      case StructureBrain.TYPES.KNUCKLEBONES_ARENA:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Knucklebones);
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
      case StructureBrain.TYPES.FARM_STATION_II:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_FarmStationII);
      case StructureBrain.TYPES.MORGUE_1:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Morgue_1);
      case StructureBrain.TYPES.MORGUE_2:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Morgue_2);
      case StructureBrain.TYPES.CRYPT_1:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Crypt_1);
      case StructureBrain.TYPES.CRYPT_2:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Crypt_2);
      case StructureBrain.TYPES.CRYPT_3:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Crypt_3);
      case StructureBrain.TYPES.SHARED_HOUSE:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Shared_House);
      case StructureBrain.TYPES.TAILOR:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Tailor);
      case StructureBrain.TYPES.POOP_BUCKET:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_PoopBucket);
      case StructureBrain.TYPES.SEED_BUCKET:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_SeedBucket);
      case StructureBrain.TYPES.JANITOR_STATION_2:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_JanitorStation_2);
      case StructureBrain.TYPES.SHRINE_DISCIPLE_BOOST:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Shrine_Disciple_Boost);
      case StructureBrain.TYPES.PUB:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Pub);
      case StructureBrain.TYPES.SHRINE_PLEASURE:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Shrine_Pleasure);
      case StructureBrain.TYPES.SHRINE_DISCIPLE_COLLECTION:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Shrine_Disciple_Collection);
      case StructureBrain.TYPES.PUB_2:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Pub_2);
      case StructureBrain.TYPES.HATCHERY_2:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Hatchery_2);
      case StructureBrain.TYPES.LEADER_TENT:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_LeaderTent);
      case StructureBrain.TYPES.DAYCARE:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Daycare);
      case StructureBrain.TYPES.WEATHER_VANE:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Weather_Vane);
      case StructureBrain.TYPES.WOOLY_SHACK:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Wooly_Shack);
      case StructureBrain.TYPES.VOLCANIC_SPA:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Volcanic_Spa);
      case StructureBrain.TYPES.RANCH:
      case StructureBrain.TYPES.RANCH_FENCE:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Ranch);
      case StructureBrain.TYPES.RANCH_TROUGH:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_RanchTrough);
      case StructureBrain.TYPES.MEDIC:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Medic);
      case StructureBrain.TYPES.RANCH_2:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Ranch_2);
      case StructureBrain.TYPES.RANCH_HUTCH:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_RanchHutch);
      case StructureBrain.TYPES.RACING_GATE:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_RacingGate);
      case StructureBrain.TYPES.LOGISTICS:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Logistics);
      case StructureBrain.TYPES.WOLF_TRAP:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Wolf_Trap);
      case StructureBrain.TYPES.LIGHTNING_ROD:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_LightningRod);
      case StructureBrain.TYPES.FURNACE_1:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Furnace);
      case StructureBrain.TYPES.FURNACE_2:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Furnace_2);
      case StructureBrain.TYPES.FURNACE_3:
      case StructureBrain.TYPES.PROXIMITY_FURNACE:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Furnace_3);
      case StructureBrain.TYPES.LIGHTNING_ROD_2:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_LightningRod_2);
      case StructureBrain.TYPES.TOOLSHED:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Toolshed);
      case StructureBrain.TYPES.FARM_CROP_GROWER:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Farm_Crop_Grower);
      case StructureBrain.TYPES.TRAIT_MANIPULATOR_1:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Trait_Manipulator_1);
      case StructureBrain.TYPES.TRAIT_MANIPULATOR_2:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Trait_Manipulator_2);
      case StructureBrain.TYPES.TRAIT_MANIPULATOR_3:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Trait_Manipulator_3);
      case StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_CHIMNEY_1:
      case StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_CHIMNEY_2:
      case StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_LAMPPOST:
      case StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_CLOCK:
      case StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_WALL:
      case StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_PLANT:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.DLC_Building_Decorations2);
      case StructureBrain.TYPES.DECORATION_DLC_YNGYA_STICKBUNDLE:
      case StructureBrain.TYPES.DECORATION_DLC_YNGYA_TALLFLOWERS:
      case StructureBrain.TYPES.DECORATION_DLC_YNGYA_TREEBUSH:
      case StructureBrain.TYPES.DECORATION_DLC_YNGYA_TREEPOT:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.DLC_Building_Decorations1);
      case StructureBrain.TYPES.RANCH_CHOPPING_BLOCK:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_RanchChoppingBlock);
      case StructureBrain.TYPES.ROTSTONE_MINE:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_RotstoneMine);
      case StructureBrain.TYPES.ROTSTONE_MINE_2:
        return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_RotstoneMine_2);
      default:
        return DataManager.Instance.UnlockedStructures.Contains(Types);
    }
  }

  public static bool IsTempleBuiltOrBeingBuilt()
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
      case StructureBrain.TYPES.RANCH_FENCE:
        return 30;
      case StructureBrain.TYPES.RANCH_2:
        return 0;
      case StructureBrain.TYPES.FURNACE_1:
        return 30;
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

  public static List<StructureBrain.TYPES> GetUpgradePathNonEmpty(StructureBrain.TYPES type)
  {
    List<StructureBrain.TYPES> upgradePath = StructuresData.GetUpgradePath(type);
    if (upgradePath != null)
      return upgradePath;
    return new List<StructureBrain.TYPES>() { type };
  }

  public static bool IsInSameUpgradePath(StructureBrain.TYPES x, StructureBrain.TYPES y)
  {
    return StructuresData.GetUpgradePathNonEmpty(x).Contains(y);
  }

  public static List<StructureBrain.TYPES> GetUpgradePath(StructureBrain.TYPES type)
  {
    if (type <= StructureBrain.TYPES.TEMPLE_IV)
    {
      if (type <= StructureBrain.TYPES.LUMBERJACK_STATION_2)
      {
        if (type <= StructureBrain.TYPES.SHRINE)
        {
          if (type != StructureBrain.TYPES.BED)
          {
            if (type != StructureBrain.TYPES.FARM_STATION)
            {
              if (type == StructureBrain.TYPES.SHRINE)
                goto label_40;
              goto label_68;
            }
            goto label_50;
          }
        }
        else
        {
          if (type <= StructureBrain.TYPES.GRAVE)
          {
            if ((uint) (type - 40) > 1U)
            {
              if (type != StructureBrain.TYPES.GRAVE)
                goto label_68;
            }
            else
              goto label_54;
          }
          else if (type != StructureBrain.TYPES.BODY_PIT)
          {
            if ((uint) (type - 63 /*0x3F*/) <= 1U)
              return new List<StructureBrain.TYPES>()
              {
                StructureBrain.TYPES.LUMBERJACK_STATION,
                StructureBrain.TYPES.LUMBERJACK_STATION_2
              };
            goto label_68;
          }
          return new List<StructureBrain.TYPES>()
          {
            StructureBrain.TYPES.GRAVE,
            StructureBrain.TYPES.BODY_PIT
          };
        }
label_54:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.BED,
          StructureBrain.TYPES.BED_2,
          StructureBrain.TYPES.BED_3
        };
      }
      if (type <= StructureBrain.TYPES.TEMPLE)
      {
        switch (type - 71)
        {
          case StructureBrain.TYPES.NONE:
            return new List<StructureBrain.TYPES>()
            {
              StructureBrain.TYPES.COOKING_FIRE
            };
          case StructureBrain.TYPES.BUILDER:
          case StructureBrain.TYPES.SACRIFICIAL_TEMPLE:
            goto label_68;
          case StructureBrain.TYPES.BED:
          case StructureBrain.TYPES.PORTAL:
            return new List<StructureBrain.TYPES>()
            {
              StructureBrain.TYPES.FOOD_STORAGE,
              StructureBrain.TYPES.FOOD_STORAGE_2
            };
          case StructureBrain.TYPES.WOOD_STORE:
          case StructureBrain.TYPES.BLACKSMITH:
            return new List<StructureBrain.TYPES>()
            {
              StructureBrain.TYPES.BLOODSTONE_MINE,
              StructureBrain.TYPES.BLOODSTONE_MINE_2
            };
          default:
            if (type != StructureBrain.TYPES.OUTHOUSE)
            {
              if (type == StructureBrain.TYPES.TEMPLE)
                break;
              goto label_68;
            }
            goto label_45;
        }
      }
      else
      {
        if (type <= StructureBrain.TYPES.SHRINE_II)
        {
          if ((uint) (type - 116) > 1U)
          {
            if (type == StructureBrain.TYPES.SHRINE_II)
              goto label_40;
            goto label_68;
          }
          return new List<StructureBrain.TYPES>()
          {
            StructureBrain.TYPES.TEMPLE_BASE_EXTENSION1,
            StructureBrain.TYPES.TEMPLE_BASE_EXTENSION2
          };
        }
        switch (type - 134)
        {
          case StructureBrain.TYPES.NONE:
            goto label_56;
          case StructureBrain.TYPES.BUILDER:
            goto label_68;
          case StructureBrain.TYPES.BED:
            goto label_46;
          case StructureBrain.TYPES.PORTAL:
            goto label_47;
          default:
            switch (type - 144 /*0x90*/)
            {
              case StructureBrain.TYPES.NONE:
              case StructureBrain.TYPES.CROP:
              case StructureBrain.TYPES.NIGHTMARE_MACHINE:
                break;
              case StructureBrain.TYPES.SACRIFICIAL_TEMPLE:
                goto label_49;
              case StructureBrain.TYPES.WOOD_STORE:
              case StructureBrain.TYPES.KITCHEN:
              case StructureBrain.TYPES.COOKED_FOOD_SILO:
                return new List<StructureBrain.TYPES>()
                {
                  StructureBrain.TYPES.SHRINE_PASSIVE,
                  StructureBrain.TYPES.SHRINE_PASSIVE_II,
                  StructureBrain.TYPES.SHRINE_PASSIVE_III
                };
              case StructureBrain.TYPES.TAVERN:
              case StructureBrain.TYPES.FARM_STATION:
                goto label_40;
              default:
                goto label_68;
            }
            break;
        }
      }
      return new List<StructureBrain.TYPES>()
      {
        StructureBrain.TYPES.TEMPLE,
        StructureBrain.TYPES.TEMPLE_II,
        StructureBrain.TYPES.TEMPLE_III,
        StructureBrain.TYPES.TEMPLE_IV
      };
label_40:
      return new List<StructureBrain.TYPES>()
      {
        StructureBrain.TYPES.SHRINE,
        StructureBrain.TYPES.SHRINE_II,
        StructureBrain.TYPES.SHRINE_III,
        StructureBrain.TYPES.SHRINE_IV
      };
    }
    if (type <= StructureBrain.TYPES.MORGUE_2)
    {
      if (type <= StructureBrain.TYPES.DEMON_SUMMONER)
      {
        switch (type - 166)
        {
          case StructureBrain.TYPES.NONE:
            goto label_56;
          case StructureBrain.TYPES.BUILDER:
            break;
          case StructureBrain.TYPES.BED:
          case StructureBrain.TYPES.PORTAL:
          case StructureBrain.TYPES.SACRIFICIAL_TEMPLE:
          case StructureBrain.TYPES.WOOD_STORE:
          case StructureBrain.TYPES.BLACKSMITH:
            goto label_68;
          case StructureBrain.TYPES.TAVERN:
            goto label_45;
          case StructureBrain.TYPES.FARM_STATION:
            goto label_46;
          case StructureBrain.TYPES.WHEAT_SILO:
            goto label_47;
          case StructureBrain.TYPES.KITCHEN:
            goto label_49;
          default:
            if (type != StructureBrain.TYPES.JANITOR_STATION)
            {
              if (type == StructureBrain.TYPES.DEMON_SUMMONER)
                goto label_44;
              goto label_68;
            }
            goto label_67;
        }
      }
      else if (type <= StructureBrain.TYPES.MISSIONARY_III)
      {
        if ((uint) (type - 252) > 1U)
        {
          if ((uint) (type - 263) > 1U)
            goto label_68;
        }
        else
          goto label_44;
      }
      else
      {
        if (type != StructureBrain.TYPES.FARM_STATION_II)
        {
          if ((uint) (type - 304) <= 1U)
            return new List<StructureBrain.TYPES>()
            {
              StructureBrain.TYPES.MORGUE_1,
              StructureBrain.TYPES.MORGUE_2
            };
          goto label_68;
        }
        goto label_50;
      }
      return new List<StructureBrain.TYPES>()
      {
        StructureBrain.TYPES.MISSIONARY,
        StructureBrain.TYPES.MISSIONARY_II,
        StructureBrain.TYPES.MISSIONARY_III
      };
label_44:
      return new List<StructureBrain.TYPES>()
      {
        StructureBrain.TYPES.DEMON_SUMMONER,
        StructureBrain.TYPES.DEMON_SUMMONER_2,
        StructureBrain.TYPES.DEMON_SUMMONER_3
      };
    }
    if (type <= StructureBrain.TYPES.HATCHERY_2)
    {
      if (type <= StructureBrain.TYPES.JANITOR_STATION_2)
      {
        if ((uint) (type - 306) > 2U)
        {
          if (type != StructureBrain.TYPES.JANITOR_STATION_2)
            goto label_68;
        }
        else
          return new List<StructureBrain.TYPES>()
          {
            StructureBrain.TYPES.CRYPT_1,
            StructureBrain.TYPES.CRYPT_2,
            StructureBrain.TYPES.CRYPT_3
          };
      }
      else
      {
        if (type != StructureBrain.TYPES.PUB)
        {
          switch (type - 349)
          {
            case StructureBrain.TYPES.NONE:
            case StructureBrain.TYPES.BED:
              return new List<StructureBrain.TYPES>()
              {
                StructureBrain.TYPES.HATCHERY,
                StructureBrain.TYPES.HATCHERY_2
              };
            case StructureBrain.TYPES.BUILDER:
              break;
            default:
              goto label_68;
          }
        }
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.PUB,
          StructureBrain.TYPES.PUB_2
        };
      }
    }
    else
    {
      if (type <= StructureBrain.TYPES.RANCH_2)
      {
        if (type == StructureBrain.TYPES.RANCH || type == StructureBrain.TYPES.RANCH_2)
          return new List<StructureBrain.TYPES>()
          {
            StructureBrain.TYPES.RANCH,
            StructureBrain.TYPES.RANCH_2
          };
        goto label_68;
      }
      switch (type - 410)
      {
        case StructureBrain.TYPES.NONE:
        case StructureBrain.TYPES.WOOD_STORE:
          return new List<StructureBrain.TYPES>()
          {
            StructureBrain.TYPES.LIGHTNING_ROD,
            StructureBrain.TYPES.LIGHTNING_ROD_2
          };
        case StructureBrain.TYPES.BUILDER:
        case StructureBrain.TYPES.BED:
        case StructureBrain.TYPES.PORTAL:
          return new List<StructureBrain.TYPES>()
          {
            StructureBrain.TYPES.FURNACE_1,
            StructureBrain.TYPES.FURNACE_2,
            StructureBrain.TYPES.FURNACE_3
          };
        case StructureBrain.TYPES.SACRIFICIAL_TEMPLE:
        case StructureBrain.TYPES.BLACKSMITH:
        case StructureBrain.TYPES.TAVERN:
        case StructureBrain.TYPES.FARM_STATION:
        case StructureBrain.TYPES.WHEAT_SILO:
          goto label_68;
        case StructureBrain.TYPES.KITCHEN:
        case StructureBrain.TYPES.COOKED_FOOD_SILO:
        case StructureBrain.TYPES.CROP:
          return new List<StructureBrain.TYPES>()
          {
            StructureBrain.TYPES.TRAIT_MANIPULATOR_1,
            StructureBrain.TYPES.TRAIT_MANIPULATOR_2,
            StructureBrain.TYPES.TRAIT_MANIPULATOR_3
          };
        default:
          if ((uint) (type - 498) <= 1U)
            return new List<StructureBrain.TYPES>()
            {
              StructureBrain.TYPES.ROTSTONE_MINE,
              StructureBrain.TYPES.ROTSTONE_MINE_2
            };
          goto label_68;
      }
    }
label_67:
    return new List<StructureBrain.TYPES>()
    {
      StructureBrain.TYPES.JANITOR_STATION,
      StructureBrain.TYPES.JANITOR_STATION_2
    };
label_45:
    return new List<StructureBrain.TYPES>()
    {
      StructureBrain.TYPES.OUTHOUSE,
      StructureBrain.TYPES.OUTHOUSE_2
    };
label_46:
    return new List<StructureBrain.TYPES>()
    {
      StructureBrain.TYPES.SCARECROW,
      StructureBrain.TYPES.SCARECROW_2
    };
label_47:
    return new List<StructureBrain.TYPES>()
    {
      StructureBrain.TYPES.HARVEST_TOTEM,
      StructureBrain.TYPES.HARVEST_TOTEM_2
    };
label_49:
    return new List<StructureBrain.TYPES>()
    {
      StructureBrain.TYPES.REFINERY,
      StructureBrain.TYPES.REFINERY_2
    };
label_50:
    return new List<StructureBrain.TYPES>()
    {
      StructureBrain.TYPES.FARM_STATION,
      StructureBrain.TYPES.FARM_STATION_II
    };
label_56:
    return new List<StructureBrain.TYPES>()
    {
      StructureBrain.TYPES.HEALING_BAY,
      StructureBrain.TYPES.HEALING_BAY_2
    };
label_68:
    return (List<StructureBrain.TYPES>) null;
  }

  public static StructureBrain.TYPES GetUpgradePrerequisite(StructureBrain.TYPES type)
  {
    switch (type)
    {
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
      case StructureBrain.TYPES.FARM_STATION_II:
        return StructureBrain.TYPES.FARM_STATION;
      case StructureBrain.TYPES.MORGUE_2:
        return StructureBrain.TYPES.MORGUE_1;
      case StructureBrain.TYPES.CRYPT_2:
        return StructureBrain.TYPES.CRYPT_1;
      case StructureBrain.TYPES.CRYPT_3:
        return StructureBrain.TYPES.CRYPT_2;
      case StructureBrain.TYPES.JANITOR_STATION_2:
        return StructureBrain.TYPES.JANITOR_STATION;
      case StructureBrain.TYPES.PUB_2:
        return StructureBrain.TYPES.PUB;
      case StructureBrain.TYPES.HATCHERY_2:
        return StructureBrain.TYPES.HATCHERY;
      case StructureBrain.TYPES.RANCH_2:
        return StructureBrain.TYPES.RANCH;
      case StructureBrain.TYPES.FURNACE_2:
        return StructureBrain.TYPES.FURNACE_1;
      case StructureBrain.TYPES.FURNACE_3:
        return StructureBrain.TYPES.FURNACE_2;
      case StructureBrain.TYPES.LIGHTNING_ROD_2:
        return StructureBrain.TYPES.LIGHTNING_ROD;
      case StructureBrain.TYPES.TRAIT_MANIPULATOR_2:
        return StructureBrain.TYPES.TRAIT_MANIPULATOR_1;
      case StructureBrain.TYPES.TRAIT_MANIPULATOR_3:
        return StructureBrain.TYPES.TRAIT_MANIPULATOR_2;
      case StructureBrain.TYPES.ROTSTONE_MINE_2:
        return StructureBrain.TYPES.ROTSTONE_MINE;
      default:
        return StructureBrain.TYPES.NONE;
    }
  }

  public static bool GetBuildOnlyOne(StructureBrain.TYPES Type)
  {
    if (Type <= StructureBrain.TYPES.SURVEILLANCE)
    {
      if (Type <= StructureBrain.TYPES.TEMPLE_EXTENSION2)
      {
        if (Type <= StructureBrain.TYPES.COOKING_FIRE)
        {
          if (Type != StructureBrain.TYPES.SHRINE && Type != StructureBrain.TYPES.FARM_PLOT_SOZO && Type != StructureBrain.TYPES.COOKING_FIRE)
            goto label_26;
        }
        else if (Type <= StructureBrain.TYPES.DRUM_CIRCLE)
        {
          if (Type != StructureBrain.TYPES.MATING_TENT && (uint) (Type - 78) > 1U)
            goto label_26;
        }
        else if (Type != StructureBrain.TYPES.KNUCKLEBONES_ARENA && (uint) (Type - 115) > 2U)
          goto label_26;
      }
      else if (Type <= StructureBrain.TYPES.TEMPLE_II)
      {
        if ((uint) (Type - 121) > 5U && Type != StructureBrain.TYPES.HEALING_BAY && Type != StructureBrain.TYPES.TEMPLE_II)
          goto label_26;
      }
      else if (Type <= StructureBrain.TYPES.TEMPLE_IV)
      {
        if ((uint) (Type - 151) > 1U && (uint) (Type - 156) > 1U)
          goto label_26;
      }
      else if ((uint) (Type - 166) > 1U && Type != StructureBrain.TYPES.SURVEILLANCE)
        goto label_26;
    }
    else if (Type <= StructureBrain.TYPES.MORGUE_2)
    {
      if (Type <= StructureBrain.TYPES.DEMON_SUMMONER_3)
      {
        if ((uint) (Type - 212) > 2U && Type != StructureBrain.TYPES.DEMON_SUMMONER && (uint) (Type - 252) > 1U)
          goto label_26;
      }
      else if (Type <= StructureBrain.TYPES.DECORATION_MONSTERSHRINE)
      {
        if ((uint) (Type - 263) > 1U && Type != StructureBrain.TYPES.DECORATION_MONSTERSHRINE)
          goto label_26;
      }
      else if ((uint) (Type - 287) > 6U && (uint) (Type - 304) > 1U)
        goto label_26;
    }
    else if (Type <= StructureBrain.TYPES.PUB_2)
    {
      if (Type <= StructureBrain.TYPES.SEED_BUCKET)
      {
        if (Type != StructureBrain.TYPES.TAILOR && (uint) (Type - 333) > 1U)
          goto label_26;
      }
      else if ((uint) (Type - 343) > 1U && Type != StructureBrain.TYPES.PUB_2)
        goto label_26;
    }
    else if (Type <= StructureBrain.TYPES.VOLCANIC_SPA)
    {
      if (Type != StructureBrain.TYPES.LEADER_TENT && (uint) (Type - 384) > 2U)
        goto label_26;
    }
    else if (Type != StructureBrain.TYPES.LOGISTICS && (uint) (Type - 411) > 2U)
      goto label_26;
    return true;
label_26:
    return false;
  }

  public static DataManager.CultLevel GetRequiredLevel(StructureBrain.TYPES Type)
  {
    if (Type <= StructureBrain.TYPES.TILE_PATH)
    {
      if (Type <= StructureBrain.TYPES.KNUCKLEBONES_ARENA)
      {
        if (Type <= StructureBrain.TYPES.PRISON)
        {
          if (Type != StructureBrain.TYPES.BED)
          {
            switch (Type - 6)
            {
              case StructureBrain.TYPES.NONE:
                break;
              case StructureBrain.TYPES.BUILDER:
              case StructureBrain.TYPES.PORTAL:
                goto label_29;
              case StructureBrain.TYPES.BED:
              case StructureBrain.TYPES.SACRIFICIAL_TEMPLE:
                goto label_28;
              default:
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
                  case StructureBrain.TYPES.TAVERN:
                  case StructureBrain.TYPES.FARM_STATION:
                  case StructureBrain.TYPES.WHEAT_SILO:
                  case StructureBrain.TYPES.CROP:
                  case StructureBrain.TYPES.NIGHTMARE_MACHINE:
                  case StructureBrain.TYPES.GRASS:
                  case StructureBrain.TYPES.FIRE:
                  case StructureBrain.TYPES.STORAGE_PIT:
                    goto label_28;
                  default:
                    goto label_29;
                }
                break;
            }
          }
        }
        else if (Type != StructureBrain.TYPES.LUMBERJACK_STATION)
        {
          switch (Type - 71)
          {
            case StructureBrain.TYPES.NONE:
            case StructureBrain.TYPES.BUILDER:
            case StructureBrain.TYPES.BED:
            case StructureBrain.TYPES.KITCHEN:
              break;
            case StructureBrain.TYPES.PORTAL:
            case StructureBrain.TYPES.SACRIFICIAL_TEMPLE:
            case StructureBrain.TYPES.WOOD_STORE:
            case StructureBrain.TYPES.BLACKSMITH:
            case StructureBrain.TYPES.TAVERN:
            case StructureBrain.TYPES.FARM_STATION:
            case StructureBrain.TYPES.WHEAT_SILO:
              goto label_29;
            default:
              if (Type == StructureBrain.TYPES.KNUCKLEBONES_ARENA)
                goto label_28;
              goto label_29;
          }
        }
      }
      else if (Type <= StructureBrain.TYPES.HEALING_BAY)
      {
        if (Type != StructureBrain.TYPES.OUTHOUSE && (uint) (Type - 126) > 1U && Type != StructureBrain.TYPES.HEALING_BAY)
          goto label_29;
      }
      else if ((uint) (Type - 136) > 1U)
      {
        switch (Type - 149)
        {
          case StructureBrain.TYPES.NONE:
          case StructureBrain.TYPES.SACRIFICIAL_TEMPLE:
            break;
          case StructureBrain.TYPES.BUILDER:
          case StructureBrain.TYPES.PORTAL:
            goto label_29;
          case StructureBrain.TYPES.BED:
            goto label_28;
          default:
            if (Type == StructureBrain.TYPES.TILE_PATH)
              break;
            goto label_29;
        }
      }
    }
    else if (Type <= StructureBrain.TYPES.TAILOR)
    {
      if (Type <= StructureBrain.TYPES.JANITOR_STATION)
      {
        if (Type == StructureBrain.TYPES.HEALING_BAY_2 || (uint) (Type - 212) > 2U && Type == StructureBrain.TYPES.JANITOR_STATION)
          goto label_28;
      }
      else if (Type != StructureBrain.TYPES.FARM_STATION_II)
      {
        switch (Type - 304)
        {
          case StructureBrain.TYPES.NONE:
          case StructureBrain.TYPES.BED:
            break;
          case StructureBrain.TYPES.BUILDER:
          case StructureBrain.TYPES.PORTAL:
            goto label_28;
          case StructureBrain.TYPES.SACRIFICIAL_TEMPLE:
          case StructureBrain.TYPES.WOOD_STORE:
            goto label_29;
          default:
            if (Type == StructureBrain.TYPES.TAILOR)
              goto label_28;
            goto label_29;
        }
      }
      else
        goto label_28;
    }
    else
    {
      if (Type <= StructureBrain.TYPES.HATCHERY_2)
      {
        if ((uint) (Type - 335) <= 1U || Type == StructureBrain.TYPES.PUB || (uint) (Type - 348) <= 3U)
          goto label_29;
        goto label_29;
      }
      if (Type <= StructureBrain.TYPES.TRAIT_MANIPULATOR_3)
      {
        if (Type != StructureBrain.TYPES.DAYCARE)
        {
          switch (Type - 384)
          {
            case StructureBrain.TYPES.CROP:
            case StructureBrain.TYPES.REPAIRABLE_HEARTS:
            case StructureBrain.TYPES.REPAIRABLE_ASTROLOGY:
            case StructureBrain.TYPES.REPAIRABLE_VOODOO:
              goto label_28;
            case StructureBrain.TYPES.DEFENCE_TOWER:
              break;
            default:
              goto label_29;
          }
        }
        else
          goto label_29;
      }
      else if ((uint) (Type - 496) > 1U)
      {
        int num = (int) (Type - 498);
      }
      else
        goto label_29;
    }
    return DataManager.CultLevel.One;
label_28:
    return DataManager.CultLevel.Two;
label_29:
    return DataManager.CultLevel.One;
  }

  public static InventoryItem.ITEM_TYPE GetBuildRubbleType(
    StructureBrain.TYPES type,
    bool followerTask = false)
  {
    return type == StructureBrain.TYPES.ROTSTONE_MINE || type == StructureBrain.TYPES.ROTSTONE_MINE_2 ? (!followerTask ? InventoryItem.ITEM_TYPE.MAGMA_STONE : InventoryItem.ITEM_TYPE.STONE) : (StructuresData.GetBuildSfx(type) == "event:/building/finished_stone" ? InventoryItem.ITEM_TYPE.STONE : InventoryItem.ITEM_TYPE.LOG);
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
      case StructureBrain.TYPES.SHARED_HOUSE:
      case StructureBrain.TYPES.TAILOR:
      case StructureBrain.TYPES.HATCHERY:
      case StructureBrain.TYPES.HATCHERY_2:
      case StructureBrain.TYPES.LEADER_TENT:
      case StructureBrain.TYPES.DAYCARE:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_RUG:
        return "event:/building/finished_fabric";
      case StructureBrain.TYPES.BLACKSMITH:
      case StructureBrain.TYPES.KITCHEN:
      case StructureBrain.TYPES.SHRINE:
      case StructureBrain.TYPES.DECORATION_STONE:
      case StructureBrain.TYPES.COOKING_FIRE:
      case StructureBrain.TYPES.BLOODSTONE_MINE:
      case StructureBrain.TYPES.BLOODSTONE_MINE_2:
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
      case StructureBrain.TYPES.MORGUE_1:
      case StructureBrain.TYPES.MORGUE_2:
      case StructureBrain.TYPES.CRYPT_1:
      case StructureBrain.TYPES.CRYPT_2:
      case StructureBrain.TYPES.CRYPT_3:
      case StructureBrain.TYPES.DECORATION_OLDFAITH_CRYSTAL:
      case StructureBrain.TYPES.DECORATION_OLDFAITH_FLAG:
      case StructureBrain.TYPES.DECORATION_OLDFAITH_FOUNTAIN:
      case StructureBrain.TYPES.DECORATION_OLDFAITH_IRONMAIDEN:
      case StructureBrain.TYPES.DECORATION_OLDFAITH_SHRINE:
      case StructureBrain.TYPES.DECORATION_OLDFAITH_TORCH:
      case StructureBrain.TYPES.DECORATION_OLDFAITH_WALL:
      case StructureBrain.TYPES.TILE_OLDFAITH:
      case StructureBrain.TYPES.DECORATION_DST_ALCHEMY:
      case StructureBrain.TYPES.DECORATION_DST_DEERCLOPS:
      case StructureBrain.TYPES.DECORATION_DST_MARBLETREE:
      case StructureBrain.TYPES.DECORATION_DST_SCIENCEMACHINE:
      case StructureBrain.TYPES.DECORATION_DST_GLOMMERSTATUE:
      case StructureBrain.TYPES.DECORATION_DST_BEEFALOSKELETON:
      case StructureBrain.TYPES.SHRINE_DISCIPLE_BOOST:
      case StructureBrain.TYPES.SHRINE_DISCIPLE_COLLECTION:
      case StructureBrain.TYPES.DECORATION_GNOME1:
      case StructureBrain.TYPES.DECORATION_GNOME2:
      case StructureBrain.TYPES.DECORATION_GNOME3:
      case StructureBrain.TYPES.DECORATION_SINFUL_STATUE:
      case StructureBrain.TYPES.DECORATION_SINFUL_CRUCIFIX:
      case StructureBrain.TYPES.DECORATION_SINFUL_INCENSE:
      case StructureBrain.TYPES.DECORATION_GOAT_LANTERN:
      case StructureBrain.TYPES.DECORATION_GOAT_STATUE:
      case StructureBrain.TYPES.DECORATION_GOAT_PLANT:
      case StructureBrain.TYPES.WEATHER_VANE:
      case StructureBrain.TYPES.WOOLY_SHACK:
      case StructureBrain.TYPES.VOLCANIC_SPA:
      case StructureBrain.TYPES.DECORATION_PALWORLD_LAMB:
      case StructureBrain.TYPES.DECORATION_PALWORLD_LANTERN:
      case StructureBrain.TYPES.DECORATION_PALWORLD_STATUE:
      case StructureBrain.TYPES.DECORATION_PALWORLD_TREE:
      case StructureBrain.TYPES.LIGHTNING_ROD:
      case StructureBrain.TYPES.FURNACE_1:
      case StructureBrain.TYPES.FURNACE_2:
      case StructureBrain.TYPES.FURNACE_3:
      case StructureBrain.TYPES.LIGHTNING_ROD_2:
      case StructureBrain.TYPES.TRAIT_MANIPULATOR_1:
      case StructureBrain.TYPES.TRAIT_MANIPULATOR_2:
      case StructureBrain.TYPES.TRAIT_MANIPULATOR_3:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_BUCKET:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_CAGE1:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_CAGE2:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_CAULDRON:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_DIORAMA:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_FIREMACHINE:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_IRONMAIDEN:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_PILLAR1:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_PILLAR2:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_STONE1:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_STONE2:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_BULB:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_CULTIST1:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_CULTIST2:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_DIORAMA:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_FIREPIT:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_PILLAR1:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_PILLAR2:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE1:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE2:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE3:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE4:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_WIRES:
      case StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_CHIMNEY_1:
      case StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_CHIMNEY_2:
      case StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_LAMPPOST:
      case StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_CLOCK:
      case StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_WALL:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_FOUNTAIN:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_SMALLSTATUE1:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_SMALLSTATUE2:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_STREETLAMP:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_WALL:
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_WOLF:
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_YNGYA:
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_EXECUTIONER:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE5:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BELLS:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_JUG:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_CANDLE:
      case StructureBrain.TYPES.PROXIMITY_FURNACE:
      case StructureBrain.TYPES.ROTSTONE_MINE:
      case StructureBrain.TYPES.ROTSTONE_MINE_2:
      case StructureBrain.TYPES.DECORATION_EASTEREGG_EGG:
      case StructureBrain.TYPES.DECORATION_EASTEREGG_HAROSTATUE:
      case StructureBrain.TYPES.DECORATION_EASTEREGG_TURUA:
      case StructureBrain.TYPES.DECORATION_EASTEREGG_WARRACKA:
      case StructureBrain.TYPES.DECORATION_CHRISTMAS_SNOWMAN:
      case StructureBrain.TYPES.DECORATION_CHRISTMAS_CANDLE:
      case StructureBrain.TYPES.DECORATION_CHRISTMAS_CANDYCANE:
        return "event:/building/finished_stone";
      case StructureBrain.TYPES.FARM_STATION:
      case StructureBrain.TYPES.DECORATION_TREE:
      case StructureBrain.TYPES.PLANK_PATH:
      case StructureBrain.TYPES.PRISON:
      case StructureBrain.TYPES.LUMBERJACK_STATION:
      case StructureBrain.TYPES.FOOD_STORAGE:
      case StructureBrain.TYPES.FOOD_STORAGE_2:
      case StructureBrain.TYPES.DRUM_CIRCLE:
      case StructureBrain.TYPES.FISHING_HUT:
      case StructureBrain.TYPES.KNUCKLEBONES_ARENA:
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
      case StructureBrain.TYPES.DECORATION_DST_PIGSTICK:
      case StructureBrain.TYPES.DECORATION_DST_TREE:
      case StructureBrain.TYPES.DECORATION_DST_WALL:
      case StructureBrain.TYPES.POOP_BUCKET:
      case StructureBrain.TYPES.SEED_BUCKET:
      case StructureBrain.TYPES.JANITOR_STATION_2:
      case StructureBrain.TYPES.PUB:
      case StructureBrain.TYPES.PUB_2:
      case StructureBrain.TYPES.DECORATION_SINFUL_FLOWERS1:
      case StructureBrain.TYPES.DECORATION_SINFUL_FLOWERS2:
      case StructureBrain.TYPES.DECORATION_SINFUL_SKULL:
      case StructureBrain.TYPES.DECORATION_CNY_LANTERN:
      case StructureBrain.TYPES.DECORATION_CNY_DRAGON:
      case StructureBrain.TYPES.DECORATION_CNY_TREE:
      case StructureBrain.TYPES.DECORATION_PILGRIM_WALL:
      case StructureBrain.TYPES.DECORATION_PILGRIM_BONSAI:
      case StructureBrain.TYPES.DECORATION_PILGRIM_LANTERN:
      case StructureBrain.TYPES.DECORATION_PILGRIM_PAGODA:
      case StructureBrain.TYPES.DECORATION_PILGRIM_VASE:
      case StructureBrain.TYPES.RANCH:
      case StructureBrain.TYPES.RANCH_FENCE:
      case StructureBrain.TYPES.RANCH_TROUGH:
      case StructureBrain.TYPES.MEDIC:
      case StructureBrain.TYPES.RANCH_2:
      case StructureBrain.TYPES.RANCH_HUTCH:
      case StructureBrain.TYPES.RACING_GATE:
      case StructureBrain.TYPES.LOGISTICS:
      case StructureBrain.TYPES.WOLF_TRAP:
      case StructureBrain.TYPES.TOOLSHED:
      case StructureBrain.TYPES.FARM_CROP_GROWER:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_BOTTLE:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_FLOOR:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_LUMP1:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_LUMP2:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_TENTACLE:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_WALL:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_LAMPPOST:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_TESLA:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_TREE1:
      case StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_PLANT:
      case StructureBrain.TYPES.DECORATION_DLC_YNGYA_CANDLE:
      case StructureBrain.TYPES.DECORATION_DLC_YNGYA_FLOWERBUCKET:
      case StructureBrain.TYPES.DECORATION_DLC_YNGYA_STICKBUNDLE:
      case StructureBrain.TYPES.DECORATION_DLC_YNGYA_TALLFLOWERS:
      case StructureBrain.TYPES.DECORATION_DLC_YNGYA_TREEBUSH:
      case StructureBrain.TYPES.DECORATION_DLC_YNGYA_TREEPOT:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BUSH1:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BUSH2:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_TREE:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_FLOOR:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BARREL1:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BARREL2:
      case StructureBrain.TYPES.RANCH_CHOPPING_BLOCK:
      case StructureBrain.TYPES.DECORATION_APPLE_BUSH:
      case StructureBrain.TYPES.DECORATION_APPLE_LANTERN:
      case StructureBrain.TYPES.DECORATION_APPLE_STATUE:
      case StructureBrain.TYPES.DECORATION_APPLE_VASE:
      case StructureBrain.TYPES.DECORATION_APPLE_WELL:
      case StructureBrain.TYPES.DECORATION_CHRISTMAS_TREE:
        return "event:/building/finished_wood";
      case StructureBrain.TYPES.FARM_PLOT:
      case StructureBrain.TYPES.FARM_PLOT_SOZO:
      case StructureBrain.TYPES.DECORATION_HAY_BALE:
      case StructureBrain.TYPES.DECORATION_HAY_PILE:
      case StructureBrain.TYPES.DECORATION_PALWORLD_PLANT:
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
      case StructureBrain.TYPES.TAILOR:
      case StructureBrain.TYPES.LEADER_TENT:
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
      case StructureBrain.TYPES.DECORATION_OLDFAITH_CRYSTAL:
      case StructureBrain.TYPES.DECORATION_OLDFAITH_FLAG:
      case StructureBrain.TYPES.DECORATION_OLDFAITH_FOUNTAIN:
      case StructureBrain.TYPES.DECORATION_OLDFAITH_IRONMAIDEN:
      case StructureBrain.TYPES.DECORATION_OLDFAITH_SHRINE:
      case StructureBrain.TYPES.DECORATION_OLDFAITH_TORCH:
      case StructureBrain.TYPES.DECORATION_OLDFAITH_WALL:
      case StructureBrain.TYPES.TILE_OLDFAITH:
      case StructureBrain.TYPES.DECORATION_DST_ALCHEMY:
      case StructureBrain.TYPES.DECORATION_DST_DEERCLOPS:
      case StructureBrain.TYPES.DECORATION_DST_MARBLETREE:
      case StructureBrain.TYPES.DECORATION_DST_PIGSTICK:
      case StructureBrain.TYPES.DECORATION_DST_SCIENCEMACHINE:
      case StructureBrain.TYPES.DECORATION_DST_TREE:
      case StructureBrain.TYPES.DECORATION_DST_WALL:
      case StructureBrain.TYPES.DECORATION_DST_GLOMMERSTATUE:
      case StructureBrain.TYPES.DECORATION_DST_BEEFALOSKELETON:
      case StructureBrain.TYPES.DECORATION_GNOME1:
      case StructureBrain.TYPES.DECORATION_GNOME2:
      case StructureBrain.TYPES.DECORATION_GNOME3:
      case StructureBrain.TYPES.DECORATION_SINFUL_STATUE:
      case StructureBrain.TYPES.DECORATION_SINFUL_CRUCIFIX:
      case StructureBrain.TYPES.DECORATION_SINFUL_FLOWERS1:
      case StructureBrain.TYPES.DECORATION_SINFUL_FLOWERS2:
      case StructureBrain.TYPES.DECORATION_SINFUL_SKULL:
      case StructureBrain.TYPES.DECORATION_SINFUL_INCENSE:
      case StructureBrain.TYPES.DECORATION_CNY_LANTERN:
      case StructureBrain.TYPES.DECORATION_CNY_DRAGON:
      case StructureBrain.TYPES.DECORATION_CNY_TREE:
      case StructureBrain.TYPES.DECORATION_PILGRIM_WALL:
      case StructureBrain.TYPES.DECORATION_PILGRIM_BONSAI:
      case StructureBrain.TYPES.DECORATION_PILGRIM_LANTERN:
      case StructureBrain.TYPES.DECORATION_PILGRIM_PAGODA:
      case StructureBrain.TYPES.DECORATION_PILGRIM_VASE:
      case StructureBrain.TYPES.DECORATION_GOAT_LANTERN:
      case StructureBrain.TYPES.DECORATION_GOAT_STATUE:
      case StructureBrain.TYPES.DECORATION_GOAT_PLANT:
      case StructureBrain.TYPES.DECORATION_PALWORLD_LAMB:
      case StructureBrain.TYPES.DECORATION_PALWORLD_LANTERN:
      case StructureBrain.TYPES.DECORATION_PALWORLD_PLANT:
      case StructureBrain.TYPES.DECORATION_PALWORLD_STATUE:
      case StructureBrain.TYPES.DECORATION_PALWORLD_TREE:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_BOTTLE:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_BUCKET:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_CAGE1:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_CAGE2:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_CAULDRON:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_DIORAMA:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_FIREMACHINE:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_FLOOR:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_IRONMAIDEN:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_LUMP1:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_LUMP2:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_PILLAR1:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_PILLAR2:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_STONE1:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_STONE2:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_TENTACLE:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_WALL:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_BULB:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_CULTIST1:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_CULTIST2:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_DIORAMA:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_FIREPIT:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_LAMPPOST:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_PILLAR1:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_PILLAR2:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE1:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE2:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE3:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE4:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_TESLA:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_TREE1:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_WIRES:
      case StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_CHIMNEY_1:
      case StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_CHIMNEY_2:
      case StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_LAMPPOST:
      case StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_CLOCK:
      case StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_WALL:
      case StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_PLANT:
      case StructureBrain.TYPES.DECORATION_DLC_YNGYA_CANDLE:
      case StructureBrain.TYPES.DECORATION_DLC_YNGYA_FLOWERBUCKET:
      case StructureBrain.TYPES.DECORATION_DLC_YNGYA_STICKBUNDLE:
      case StructureBrain.TYPES.DECORATION_DLC_YNGYA_TALLFLOWERS:
      case StructureBrain.TYPES.DECORATION_DLC_YNGYA_TREEBUSH:
      case StructureBrain.TYPES.DECORATION_DLC_YNGYA_TREEPOT:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BUSH1:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BUSH2:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_FOUNTAIN:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_SMALLSTATUE1:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_SMALLSTATUE2:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_STREETLAMP:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_TREE:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_WALL:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_FLOOR:
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_WOLF:
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_YNGYA:
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_EXECUTIONER:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE5:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BARREL1:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BARREL2:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BELLS:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_JUG:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_RUG:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_CANDLE:
      case StructureBrain.TYPES.DECORATION_APPLE_BUSH:
      case StructureBrain.TYPES.DECORATION_APPLE_LANTERN:
      case StructureBrain.TYPES.DECORATION_APPLE_STATUE:
      case StructureBrain.TYPES.DECORATION_APPLE_VASE:
      case StructureBrain.TYPES.DECORATION_APPLE_WELL:
      case StructureBrain.TYPES.DECORATION_EASTEREGG_EGG:
      case StructureBrain.TYPES.DECORATION_EASTEREGG_HAROSTATUE:
      case StructureBrain.TYPES.DECORATION_EASTEREGG_TURUA:
      case StructureBrain.TYPES.DECORATION_EASTEREGG_WARRACKA:
      case StructureBrain.TYPES.DECORATION_CHRISTMAS_TREE:
      case StructureBrain.TYPES.DECORATION_CHRISTMAS_SNOWMAN:
      case StructureBrain.TYPES.DECORATION_CHRISTMAS_CANDLE:
      case StructureBrain.TYPES.DECORATION_CHRISTMAS_CANDYCANE:
        return UIBuildMenuController.Category.Aesthetic;
      case StructureBrain.TYPES.WEATHER_VANE:
      case StructureBrain.TYPES.VOLCANIC_SPA:
      case StructureBrain.TYPES.RANCH:
      case StructureBrain.TYPES.RANCH_FENCE:
      case StructureBrain.TYPES.RANCH_TROUGH:
      case StructureBrain.TYPES.MEDIC:
      case StructureBrain.TYPES.RANCH_2:
      case StructureBrain.TYPES.RANCH_HUTCH:
      case StructureBrain.TYPES.LOGISTICS:
      case StructureBrain.TYPES.WOLF_TRAP:
      case StructureBrain.TYPES.LIGHTNING_ROD:
      case StructureBrain.TYPES.FURNACE_1:
      case StructureBrain.TYPES.FURNACE_2:
      case StructureBrain.TYPES.FURNACE_3:
      case StructureBrain.TYPES.LIGHTNING_ROD_2:
      case StructureBrain.TYPES.TOOLSHED:
      case StructureBrain.TYPES.FARM_CROP_GROWER:
      case StructureBrain.TYPES.TRAIT_MANIPULATOR_1:
      case StructureBrain.TYPES.TRAIT_MANIPULATOR_2:
      case StructureBrain.TYPES.TRAIT_MANIPULATOR_3:
      case StructureBrain.TYPES.RANCH_CHOPPING_BLOCK:
      case StructureBrain.TYPES.PROXIMITY_FURNACE:
      case StructureBrain.TYPES.ROTSTONE_MINE:
      case StructureBrain.TYPES.ROTSTONE_MINE_2:
        return UIBuildMenuController.Category.MajorDLC;
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
      case StructureBrain.TYPES.LEADER_TENT:
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
      case StructureBrain.TYPES.DECORATION_OLDFAITH_CRYSTAL:
      case StructureBrain.TYPES.DECORATION_OLDFAITH_FLAG:
      case StructureBrain.TYPES.DECORATION_OLDFAITH_FOUNTAIN:
      case StructureBrain.TYPES.DECORATION_OLDFAITH_IRONMAIDEN:
      case StructureBrain.TYPES.DECORATION_OLDFAITH_SHRINE:
      case StructureBrain.TYPES.DECORATION_OLDFAITH_TORCH:
      case StructureBrain.TYPES.DECORATION_OLDFAITH_WALL:
      case StructureBrain.TYPES.TILE_OLDFAITH:
      case StructureBrain.TYPES.DECORATION_DST_ALCHEMY:
      case StructureBrain.TYPES.DECORATION_DST_DEERCLOPS:
      case StructureBrain.TYPES.DECORATION_DST_MARBLETREE:
      case StructureBrain.TYPES.DECORATION_DST_PIGSTICK:
      case StructureBrain.TYPES.DECORATION_DST_SCIENCEMACHINE:
      case StructureBrain.TYPES.DECORATION_DST_TREE:
      case StructureBrain.TYPES.DECORATION_DST_WALL:
      case StructureBrain.TYPES.DECORATION_DST_GLOMMERSTATUE:
      case StructureBrain.TYPES.DECORATION_DST_BEEFALOSKELETON:
      case StructureBrain.TYPES.DECORATION_GNOME1:
      case StructureBrain.TYPES.DECORATION_GNOME2:
      case StructureBrain.TYPES.DECORATION_GNOME3:
      case StructureBrain.TYPES.DECORATION_SINFUL_STATUE:
      case StructureBrain.TYPES.DECORATION_SINFUL_CRUCIFIX:
      case StructureBrain.TYPES.DECORATION_SINFUL_FLOWERS1:
      case StructureBrain.TYPES.DECORATION_SINFUL_FLOWERS2:
      case StructureBrain.TYPES.DECORATION_SINFUL_SKULL:
      case StructureBrain.TYPES.DECORATION_SINFUL_INCENSE:
      case StructureBrain.TYPES.DECORATION_CNY_LANTERN:
      case StructureBrain.TYPES.DECORATION_CNY_DRAGON:
      case StructureBrain.TYPES.DECORATION_CNY_TREE:
      case StructureBrain.TYPES.DECORATION_PILGRIM_WALL:
      case StructureBrain.TYPES.DECORATION_PILGRIM_BONSAI:
      case StructureBrain.TYPES.DECORATION_PILGRIM_LANTERN:
      case StructureBrain.TYPES.DECORATION_PILGRIM_PAGODA:
      case StructureBrain.TYPES.DECORATION_PILGRIM_VASE:
      case StructureBrain.TYPES.DECORATION_GOAT_LANTERN:
      case StructureBrain.TYPES.DECORATION_GOAT_STATUE:
      case StructureBrain.TYPES.DECORATION_GOAT_PLANT:
      case StructureBrain.TYPES.DECORATION_PALWORLD_LAMB:
      case StructureBrain.TYPES.DECORATION_PALWORLD_LANTERN:
      case StructureBrain.TYPES.DECORATION_PALWORLD_PLANT:
      case StructureBrain.TYPES.DECORATION_PALWORLD_STATUE:
      case StructureBrain.TYPES.DECORATION_PALWORLD_TREE:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_BOTTLE:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_BUCKET:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_CAGE1:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_CAGE2:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_CAULDRON:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_DIORAMA:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_FIREMACHINE:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_FLOOR:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_IRONMAIDEN:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_LUMP1:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_LUMP2:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_PILLAR1:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_PILLAR2:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_STONE1:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_STONE2:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_TENTACLE:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_WALL:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_BULB:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_CULTIST1:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_CULTIST2:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_DIORAMA:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_FIREPIT:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_LAMPPOST:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_PILLAR1:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_PILLAR2:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE1:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE2:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE3:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE4:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_TESLA:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_TREE1:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_WIRES:
      case StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_CHIMNEY_1:
      case StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_CHIMNEY_2:
      case StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_LAMPPOST:
      case StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_CLOCK:
      case StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_WALL:
      case StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_PLANT:
      case StructureBrain.TYPES.DECORATION_DLC_YNGYA_CANDLE:
      case StructureBrain.TYPES.DECORATION_DLC_YNGYA_FLOWERBUCKET:
      case StructureBrain.TYPES.DECORATION_DLC_YNGYA_STICKBUNDLE:
      case StructureBrain.TYPES.DECORATION_DLC_YNGYA_TALLFLOWERS:
      case StructureBrain.TYPES.DECORATION_DLC_YNGYA_TREEBUSH:
      case StructureBrain.TYPES.DECORATION_DLC_YNGYA_TREEPOT:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BUSH1:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BUSH2:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_FOUNTAIN:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_SMALLSTATUE1:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_SMALLSTATUE2:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_STREETLAMP:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_TREE:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_WALL:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_FLOOR:
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_WOLF:
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_YNGYA:
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_EXECUTIONER:
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE5:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BARREL1:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BARREL2:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BELLS:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_JUG:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_RUG:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_CANDLE:
      case StructureBrain.TYPES.DECORATION_APPLE_BUSH:
      case StructureBrain.TYPES.DECORATION_APPLE_LANTERN:
      case StructureBrain.TYPES.DECORATION_APPLE_STATUE:
      case StructureBrain.TYPES.DECORATION_APPLE_VASE:
      case StructureBrain.TYPES.DECORATION_APPLE_WELL:
      case StructureBrain.TYPES.DECORATION_EASTEREGG_EGG:
      case StructureBrain.TYPES.DECORATION_EASTEREGG_HAROSTATUE:
      case StructureBrain.TYPES.DECORATION_EASTEREGG_TURUA:
      case StructureBrain.TYPES.DECORATION_EASTEREGG_WARRACKA:
      case StructureBrain.TYPES.DECORATION_CHRISTMAS_TREE:
      case StructureBrain.TYPES.DECORATION_CHRISTMAS_SNOWMAN:
      case StructureBrain.TYPES.DECORATION_CHRISTMAS_CANDLE:
      case StructureBrain.TYPES.DECORATION_CHRISTMAS_CANDYCANE:
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
      case StructureBrain.TYPES.FARM_STATION_II:
        return StructureBrain.Categories.FOOD;
      case StructureBrain.TYPES.JANITOR_STATION_2:
        return StructureBrain.Categories.FOLLOWERS;
      case StructureBrain.TYPES.WEATHER_VANE:
      case StructureBrain.TYPES.VOLCANIC_SPA:
      case StructureBrain.TYPES.RANCH:
      case StructureBrain.TYPES.RANCH_FENCE:
      case StructureBrain.TYPES.RANCH_TROUGH:
      case StructureBrain.TYPES.MEDIC:
      case StructureBrain.TYPES.RANCH_2:
      case StructureBrain.TYPES.RANCH_HUTCH:
      case StructureBrain.TYPES.LOGISTICS:
      case StructureBrain.TYPES.WOLF_TRAP:
      case StructureBrain.TYPES.LIGHTNING_ROD:
      case StructureBrain.TYPES.FURNACE_1:
      case StructureBrain.TYPES.FURNACE_2:
      case StructureBrain.TYPES.FURNACE_3:
      case StructureBrain.TYPES.LIGHTNING_ROD_2:
      case StructureBrain.TYPES.TOOLSHED:
      case StructureBrain.TYPES.FARM_CROP_GROWER:
      case StructureBrain.TYPES.TRAIT_MANIPULATOR_1:
      case StructureBrain.TYPES.TRAIT_MANIPULATOR_2:
      case StructureBrain.TYPES.TRAIT_MANIPULATOR_3:
      case StructureBrain.TYPES.RANCH_CHOPPING_BLOCK:
      case StructureBrain.TYPES.PROXIMITY_FURNACE:
        return StructureBrain.Categories.MAJOR_DLC;
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

  public static int GetModifiedGold(int Cost) => Mathf.CeilToInt((float) Cost * 2f);

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
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 5)
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
      case StructureBrain.TYPES.MATING_TENT:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.SILK_THREAD, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 10)
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
      case StructureBrain.TYPES.DRUM_CIRCLE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 6),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 2)
        };
        break;
      case StructureBrain.TYPES.FISHING_HUT:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 20),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FISH, 2)
        };
        break;
      case StructureBrain.TYPES.KNUCKLEBONES_ARENA:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 40)
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
      case StructureBrain.TYPES.MORGUE_1:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 30),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 15)
        };
        break;
      case StructureBrain.TYPES.MORGUE_2:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 3)
        };
        break;
      case StructureBrain.TYPES.CRYPT_1:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 20),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 15)
        };
        break;
      case StructureBrain.TYPES.CRYPT_2:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 6),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 2)
        };
        break;
      case StructureBrain.TYPES.CRYPT_3:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 16 /*0x10*/),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 8)
        };
        break;
      case StructureBrain.TYPES.SHARED_HOUSE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 4),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.POOP, 10)
        };
        break;
      case StructureBrain.TYPES.DECORATION_OLDFAITH_CRYSTAL:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 1),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_OLDFAITH_FLAG:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_OLDFAITH_FOUNTAIN:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FOLLOWER_MEAT, 1)
        };
        break;
      case StructureBrain.TYPES.DECORATION_OLDFAITH_IRONMAIDEN:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 3),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FOLLOWER_MEAT, 1)
        };
        break;
      case StructureBrain.TYPES.DECORATION_OLDFAITH_SHRINE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 2),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FLOWER_RED, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_OLDFAITH_TORCH:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 1),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_OLDFAITH_WALL:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 1)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DST_ALCHEMY:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 5)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DST_DEERCLOPS:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DST_MARBLETREE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DST_PIGSTICK:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.MEAT, 1),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 1)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DST_SCIENCEMACHINE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 5)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DST_TREE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 10)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DST_WALL:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 1)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DST_GLOMMERSTATUE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DST_BEEFALOSKELETON:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 2),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 5)
        };
        break;
      case StructureBrain.TYPES.TAILOR:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.SPIDER_WEB, 20),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 4)
        };
        break;
      case StructureBrain.TYPES.POOP_BUCKET:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 10)
        };
        break;
      case StructureBrain.TYPES.SEED_BUCKET:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 10)
        };
        break;
      case StructureBrain.TYPES.JANITOR_STATION_2:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 8)
        };
        break;
      case StructureBrain.TYPES.SHRINE_DISCIPLE_BOOST:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 10)
        };
        break;
      case StructureBrain.TYPES.PUB:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 15),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.HOPS, 1),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRAPES, 1)
        };
        break;
      case StructureBrain.TYPES.SHRINE_PLEASURE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 4),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 2)
        };
        break;
      case StructureBrain.TYPES.SHRINE_DISCIPLE_COLLECTION:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 10)
        };
        break;
      case StructureBrain.TYPES.HATCHERY:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.COTTON, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 20),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 15)
        };
        break;
      case StructureBrain.TYPES.PUB_2:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 20),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 6),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.PLEASURE_POINT, 1)
        };
        break;
      case StructureBrain.TYPES.HATCHERY_2:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.COTTON, 15),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.PLEASURE_POINT, 1)
        };
        break;
      case StructureBrain.TYPES.DECORATION_GNOME1:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_GNOME2:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_GNOME3:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_SINFUL_STATUE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 50)
        };
        break;
      case StructureBrain.TYPES.DECORATION_SINFUL_CRUCIFIX:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FOLLOWER_MEAT, 1)
        };
        break;
      case StructureBrain.TYPES.DECORATION_SINFUL_FLOWERS1:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FLOWER_RED, 5)
        };
        break;
      case StructureBrain.TYPES.DECORATION_SINFUL_FLOWERS2:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FLOWER_RED, 5)
        };
        break;
      case StructureBrain.TYPES.DECORATION_SINFUL_SKULL:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BONE, 5)
        };
        break;
      case StructureBrain.TYPES.DECORATION_SINFUL_INCENSE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FLOWER_RED, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_CNY_LANTERN:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_CNY_DRAGON:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_CNY_TREE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 5)
        };
        break;
      case StructureBrain.TYPES.LEADER_TENT:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.SPIDER_WEB, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 20)
        };
        break;
      case StructureBrain.TYPES.DAYCARE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.COTTON, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 20),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 15)
        };
        break;
      case StructureBrain.TYPES.DECORATION_PILGRIM_WALL:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_PILGRIM_BONSAI:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_PILGRIM_LANTERN:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_PILGRIM_PAGODA:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_PILGRIM_VASE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 1),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 1)
        };
        break;
      case StructureBrain.TYPES.DECORATION_GOAT_LANTERN:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_GOAT_STATUE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_GOAT_PLANT:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 3)
        };
        break;
      case StructureBrain.TYPES.WEATHER_VANE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LIGHTNING_SHARD, 10)
        };
        break;
      case StructureBrain.TYPES.WOOLY_SHACK:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.COTTON, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 5)
        };
        break;
      case StructureBrain.TYPES.VOLCANIC_SPA:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LIGHTNING_SHARD, 8),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 12)
        };
        break;
      case StructureBrain.TYPES.RANCH:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 4)
        };
        break;
      case StructureBrain.TYPES.RANCH_FENCE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 1)
        };
        break;
      case StructureBrain.TYPES.RANCH_TROUGH:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 4),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 10)
        };
        break;
      case StructureBrain.TYPES.MEDIC:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 4),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 4)
        };
        break;
      case StructureBrain.TYPES.RANCH_2:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 11)
        };
        break;
      case StructureBrain.TYPES.RANCH_HUTCH:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 4),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 5)
        };
        break;
      case StructureBrain.TYPES.RACING_GATE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.YEW_HOLY, 1)
        };
        break;
      case StructureBrain.TYPES.DECORATION_PALWORLD_LAMB:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_PALWORLD_LANTERN:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_PALWORLD_PLANT:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_PALWORLD_STATUE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_PALWORLD_TREE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 3)
        };
        break;
      case StructureBrain.TYPES.LOGISTICS:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.YEW_HOLY, 8),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 5)
        };
        break;
      case StructureBrain.TYPES.WOLF_TRAP:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 1),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.YEW_HOLY, 1)
        };
        break;
      case StructureBrain.TYPES.LIGHTNING_ROD:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LIGHTNING_SHARD, 8),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 5)
        };
        break;
      case StructureBrain.TYPES.FURNACE_1:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 10)
        };
        break;
      case StructureBrain.TYPES.FURNACE_2:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 3),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 6)
        };
        break;
      case StructureBrain.TYPES.FURNACE_3:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 6),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 12)
        };
        break;
      case StructureBrain.TYPES.LIGHTNING_ROD_2:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 4),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LIGHTNING_SHARD, 8)
        };
        break;
      case StructureBrain.TYPES.TOOLSHED:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 10)
        };
        break;
      case StructureBrain.TYPES.FARM_CROP_GROWER:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG_REFINED, 4),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LIGHTNING_SHARD, 15)
        };
        break;
      case StructureBrain.TYPES.TRAIT_MANIPULATOR_1:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.YEW_CURSED, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 3)
        };
        break;
      case StructureBrain.TYPES.TRAIT_MANIPULATOR_2:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.YEW_CURSED, 8),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 6)
        };
        break;
      case StructureBrain.TYPES.TRAIT_MANIPULATOR_3:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.YEW_CURSED, 14),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 10)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_ROT_BOTTLE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.MAGMA_STONE, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_ROT_BUCKET:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.MAGMA_STONE, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_ROT_CAGE1:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.MAGMA_STONE, 1),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_ROT_CAGE2:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.MAGMA_STONE, 1),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.MAGMA_STONE, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_ROT_CAULDRON:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.MAGMA_STONE, 1),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_ROT_DIORAMA:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.MAGMA_STONE, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_ROT_FIREMACHINE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.MAGMA_STONE, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_ROT_FLOOR:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_FLOOR:
        cost = new List<StructuresData.ItemCost>();
        break;
      case StructureBrain.TYPES.DECORATION_DLC_ROT_IRONMAIDEN:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.MAGMA_STONE, 1),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_ROT_LUMP1:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.MAGMA_STONE, 5)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_ROT_LUMP2:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.MAGMA_STONE, 5)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_ROT_PILLAR1:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.MAGMA_STONE, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_ROT_PILLAR2:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.MAGMA_STONE, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_ROT_STONE1:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.MAGMA_STONE, 6),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_ROT_STONE2:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.MAGMA_STONE, 6),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_ROT_TENTACLE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.MAGMA_STONE, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_ROT_WALL:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.MAGMA_STONE, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_BULB:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LIGHTNING_SHARD, 5)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_CULTIST1:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LIGHTNING_SHARD, 1),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_CULTIST2:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LIGHTNING_SHARD, 1),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_DIORAMA:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LIGHTNING_SHARD, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_FIREPIT:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LIGHTNING_SHARD, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_LAMPPOST:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LIGHTNING_SHARD, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_PILLAR1:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LIGHTNING_SHARD, 2),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_PILLAR2:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LIGHTNING_SHARD, 2),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE1:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LIGHTNING_SHARD, 1),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE2:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LIGHTNING_SHARD, 1),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE3:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LIGHTNING_SHARD, 1),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE4:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LIGHTNING_SHARD, 1),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_TESLA:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LIGHTNING_SHARD, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_TREE1:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_WIRES:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LIGHTNING_SHARD, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_CHIMNEY_1:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LIGHTNING_SHARD, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_CHIMNEY_2:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LIGHTNING_SHARD, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_LAMPPOST:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LIGHTNING_SHARD, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_CLOCK:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LIGHTNING_SHARD, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_WALL:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_PLANT:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 1),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 1)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_YNGYA_CANDLE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 1),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_YNGYA_FLOWERBUCKET:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 1),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 5)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_YNGYA_STICKBUNDLE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.YEW_CURSED, 2),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_YNGYA_TALLFLOWERS:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FLOWER_WHITE, 1),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FLOWER_PURPLE, 1)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_YNGYA_TREEBUSH:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FLOWER_WHITE, 2),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 5)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_YNGYA_TREEPOT:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FLOWER_PURPLE, 2),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 5)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BUSH1:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 5)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BUSH2:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 10)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_FOUNTAIN:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_SMALLSTATUE1:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 4)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_SMALLSTATUE2:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 6)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_STREETLAMP:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_TREE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.YEW_CURSED, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_WALL:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_WOLF:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.YEW_HOLY, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 10)
        };
        break;
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_YNGYA:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.YEW_HOLY, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 10)
        };
        break;
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_EXECUTIONER:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.YEW_HOLY, 5),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 10)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE5:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LIGHTNING_SHARD, 1),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BARREL1:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 1)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BARREL2:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BELLS:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 2)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_JUG:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 1)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_RUG:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 1)
        };
        break;
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_CANDLE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LIGHTNING_SHARD, 1)
        };
        break;
      case StructureBrain.TYPES.RANCH_CHOPPING_BLOCK:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 2),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.YEW_CURSED, 2)
        };
        break;
      case StructureBrain.TYPES.PROXIMITY_FURNACE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LIGHTNING_SHARD, 4)
        };
        break;
      case StructureBrain.TYPES.ROTSTONE_MINE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.MAGMA_STONE, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 5)
        };
        break;
      case StructureBrain.TYPES.ROTSTONE_MINE_2:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.MAGMA_STONE, 25),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.YEW_CURSED, 8)
        };
        break;
      case StructureBrain.TYPES.DECORATION_APPLE_BUSH:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 5)
        };
        break;
      case StructureBrain.TYPES.DECORATION_APPLE_LANTERN:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_APPLE_STATUE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_APPLE_VASE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_APPLE_WELL:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 5)
        };
        break;
      case StructureBrain.TYPES.DECORATION_EASTEREGG_EGG:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.YOLK, 1)
        };
        break;
      case StructureBrain.TYPES.DECORATION_EASTEREGG_HAROSTATUE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE_REFINED, 6)
        };
        break;
      case StructureBrain.TYPES.DECORATION_EASTEREGG_TURUA:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.MAGMA_STONE, 5)
        };
        break;
      case StructureBrain.TYPES.DECORATION_EASTEREGG_WARRACKA:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.MAGMA_STONE, 5)
        };
        break;
      case StructureBrain.TYPES.DECORATION_CHRISTMAS_TREE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 5)
        };
        break;
      case StructureBrain.TYPES.DECORATION_CHRISTMAS_SNOWMAN:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_CHRISTMAS_CANDLE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 3)
        };
        break;
      case StructureBrain.TYPES.DECORATION_CHRISTMAS_CANDYCANE:
        cost = new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 3)
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
      case StructureBrain.TYPES.ICE_BLOCK:
      case StructureBrain.TYPES.RANCH_FENCE:
      case StructureBrain.TYPES.SNOW_DRIFT:
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
      case StructureBrain.TYPES.FARM_PLOT:
      case StructureBrain.TYPES.REFINERY:
      case StructureBrain.TYPES.SHRINE_PASSIVE:
      case StructureBrain.TYPES.SHRINE_PASSIVE_II:
      case StructureBrain.TYPES.SHRINE_PASSIVE_III:
      case StructureBrain.TYPES.REFINERY_2:
      case StructureBrain.TYPES.WEATHER_VANE:
      case StructureBrain.TYPES.VOLCANIC_SPA:
      case StructureBrain.TYPES.RACING_GATE:
        return false;
      default:
        return true;
    }
  }

  public void UpdateDictionaryLookup()
  {
    foreach (PlacementRegion.TileGridTile tileGridTile in this.Grid)
    {
      if (!this.GridTileLookup.ContainsKey(tileGridTile.Position))
        this.GridTileLookup.Add(tileGridTile.Position, tileGridTile);
    }
    List<PlacementRegion.TileGridTile> list = this.GridTileLookup.Values.ToList<PlacementRegion.TileGridTile>();
    for (int index = list.Count - 1; index >= 0; --index)
    {
      if (!this.Grid.Contains(list[index]))
        this.GridTileLookup.Remove(list[index].Position);
    }
  }

  [MessagePackObject(false)]
  [Serializable]
  public class LogisticsSlot
  {
    [Key(0)]
    public StructureBrain.TYPES RootStructureType;
    [Key(1)]
    public StructureBrain.TYPES TargetStructureType;
  }

  [MessagePackObject(false)]
  [Serializable]
  public struct PathData
  {
    [Key(0)]
    public Vector2Int TilePosition;
    [Key(1)]
    public Vector3 WorldPosition;
    [Key(2)]
    public int PathID;
  }

  public enum Phase
  {
    Hidden,
    Available,
    Built,
  }

  [MessagePackObject(false)]
  [Serializable]
  public class Ranchable_Animal
  {
    [Key(0)]
    public InventoryItem.ITEM_TYPE Type;
    [Key(1)]
    public int Age = 1;
    [Key(2)]
    public int ID;
    [Key(3)]
    public float Satiation = 50f;
    [Key(32 /*0x20*/)]
    public float Injured = -1f;
    [Key(33)]
    public float FeralCalming;
    [Key(4)]
    public float TimeSincePoop;
    [Key(29)]
    public float TimeSinceLastWash;
    [Key(5)]
    public bool MilkedToday;
    [Key(6)]
    public bool MilkedReady;
    [Key(19)]
    public int HappyAmount;
    [Key(11)]
    public bool PlayerMadeHappyToday;
    [Key(31 /*0x1F*/)]
    public bool RacedToday;
    [Key(7)]
    public bool WorkedToday;
    [Key(8)]
    public bool WorkedReady;
    [Key(9)]
    public bool EatenToday;
    [Key(15)]
    public bool IsFavouriteFoodRevealed;
    [Key(14)]
    public InventoryItem.ITEM_TYPE FavouriteFood;
    [Key(30)]
    public bool BestFriend;
    [Key(10)]
    public bool PetToday;
    [Key(16 /*0x10*/)]
    public Interaction_Ranchable.State State;
    [Key(17)]
    public Interaction_Ranchable.Ailment Ailment;
    [Key(18)]
    public float AilmentGameTime;
    [Key(12)]
    public float _adoration;
    [Key(13)]
    public int Level = 1;
    [Key(21)]
    public string GivenName;
    [Key(22)]
    public int GrowthStage;
    [Key(23)]
    public Interaction_Ranchable.GrowthRate growthRate;
    [Key(20)]
    public Vector3 LastPosition;
    [Key(24)]
    public int Horns = 1;
    [Key(25)]
    public int Ears = 1;
    [Key(26)]
    public int Head = 1;
    [Key(27)]
    public int Colour;
    [Key(28)]
    public float Speed;
    [Key(34)]
    public Interaction_Ranchable.CauseOfDeath CauseOfDeath;

    [IgnoreMember]
    public float Adoration
    {
      get => Mathf.Clamp(this._adoration, 0.0f, 100f);
      set
      {
        this.CheckBestFriend();
        this._adoration = value;
      }
    }

    [IgnoreMember]
    public int GrowthState => Mathf.Clamp(Mathf.FloorToInt((float) (this.Age / 5)), 0, 6);

    public void CheckBestFriend()
    {
      if (this.Level < 3)
        return;
      if (this.Level > DataManager.Instance.bestFriendAnimalLevel)
      {
        DataManager.Instance.bestFriendAnimalLevel = this.Level;
        DataManager.Instance.bestFriendAnimalAdoration = this.Adoration;
        DataManager.Instance.bestFriendAnimal = this;
        this.BestFriend = true;
        this.UpdateBestFriend(this);
        Debug.Log((object) $"New Best Friend Animal: {this.GivenName}, Level: {this.Level.ToString()}, Adoration: {this.Adoration.ToString()}");
      }
      else if (this.Level == DataManager.Instance.bestFriendAnimalLevel)
      {
        if ((double) this.Adoration > (double) DataManager.Instance.bestFriendAnimalAdoration)
        {
          DataManager.Instance.bestFriendAnimalAdoration = this.Adoration;
          DataManager.Instance.bestFriendAnimal = this;
          this.BestFriend = true;
          this.UpdateBestFriend(this);
          Debug.Log((object) $"New Best Friend Animal: {this.GivenName}, Level: {this.Level.ToString()}, Adoration: {this.Adoration.ToString()}");
        }
        else
          this.BestFriend = false;
      }
      else
        this.BestFriend = false;
    }

    public void UpdateBestFriend(StructuresData.Ranchable_Animal newBestFriend)
    {
      foreach (Interaction_Ranchable ranchable in Interaction_Ranchable.Ranchables)
      {
        if (ranchable.Animal != newBestFriend)
          ranchable.Animal.BestFriend = false;
      }
    }

    public bool IsAvailableForFollowerTask()
    {
      bool flag1 = this.WorkedReady && !this.WorkedToday;
      bool flag2 = this.Type == InventoryItem.ITEM_TYPE.ANIMAL_COW && this.MilkedReady && !this.MilkedToday;
      bool flag3 = this.Ailment == Interaction_Ranchable.Ailment.Stinky;
      return this.State == Interaction_Ranchable.State.Default && flag1 | flag2 | flag3;
    }

    public bool IsPlayersAnimal()
    {
      for (int index = 0; index < DataManager.Instance.FollowingPlayerAnimals.Length; ++index)
      {
        if (DataManager.Instance.FollowingPlayerAnimals[index] == this)
          return true;
      }
      return false;
    }

    public void UpdatePlayersPetState()
    {
      if (!this.IsPlayersAnimal())
        return;
      int growthState = this.GrowthState;
      if (TimeManager.CurrentPhase != DayPhase.Dawn)
        return;
      this.Satiation -= UnityEngine.Random.Range(15f, 30f);
      ++this.GrowthStage;
      if (this.GrowthStage > Interaction_Ranchable.getResourceGrowthRateDays(this))
      {
        this.GrowthStage = 0;
        this.WorkedReady = true;
        this.WorkedToday = false;
      }
      this.MilkedReady = true;
      this.MilkedToday = false;
      this.EatenToday = false;
    }

    public int GetLootCount()
    {
      float num1 = 5f;
      if (this.Type == InventoryItem.ITEM_TYPE.ANIMAL_COW)
        num1 = 2f;
      else if (this.Type == InventoryItem.ITEM_TYPE.ANIMAL_LLAMA)
        num1 = 2f;
      else if (this.Type == InventoryItem.ITEM_TYPE.ANIMAL_GOAT)
        num1 = 1f;
      float num2 = (float) Mathf.Clamp(this.Level, 1, 10) / 2f;
      double num3 = (double) num1 + (double) num2;
      float num4 = 1f;
      if (this.Age <= 2)
        num4 *= 0.5f;
      if (this.GrowthStage >= 6)
        num4 *= 1.15f;
      if (this.Age >= 15)
        num4 *= 1.15f;
      if (this.BestFriend)
        num4 *= 1.25f;
      if (FollowerBrainStats.IsRanchHarvest)
        num4 *= 2f;
      double num5 = (double) num4;
      return Mathf.Clamp(Mathf.FloorToInt((float) (num3 * num5)), 1, 99);
    }

    public int GetLootCountLevelModifier()
    {
      int num = Mathf.FloorToInt((float) Mathf.Clamp(this.Level, 1, 10) / 2f);
      return this.Level == 1 ? 0 : num;
    }

    public int GetMeatCount()
    {
      int num1 = 3;
      if (this.Type == InventoryItem.ITEM_TYPE.ANIMAL_COW)
        num1 += 2;
      int num2 = num1 * 3;
      if (this.Age < 2)
        num2 = Mathf.FloorToInt((float) num2 / 2f);
      if (this.Level > 1)
        num2 += Mathf.Clamp(this.Level, 1, 5);
      float num3 = 1f;
      if (this.GrowthStage >= 6)
        num3 += 0.3f;
      if (this.Age >= 15)
        num3 += 0.3f;
      if (this.Ailment == Interaction_Ranchable.Ailment.Feral)
        num3 += 0.5f;
      if (this.BestFriend)
        num3 += 0.25f;
      if (FollowerBrainStats.IsRanchMeat)
        ++num3;
      return Mathf.FloorToInt((float) num2 * num3);
    }

    public int GetMeatCountLevelModifier() => this.Level == 1 ? 0 : Mathf.Clamp(this.Level, 1, 5);

    public string GetName()
    {
      return !string.IsNullOrEmpty(this.GivenName) ? this.GivenName : InventoryItem.LocalizedName(this.Type);
    }
  }

  [MessagePackObject(false)]
  [Serializable]
  public class EggData
  {
    [Key(0)]
    public int EggSeed;
    [Key(1)]
    public int Parent_1_ID;
    [Key(2)]
    public int Parent_2_ID;
    [Key(3)]
    public string Parent_1_SkinName;
    [Key(4)]
    public string Parent_2_SkinName;
    [Key(5)]
    public int Parent_1_SkinColor;
    [Key(6)]
    public int Parent_2_SkinColor;
    [Key(7)]
    public int Parent_1_SkinVariant;
    [Key(8)]
    public int Parent_2_SkinVariant;
    [Key(9)]
    public string Parent1Name;
    [Key(10)]
    public string Parent2Name;
    [Key(11)]
    public List<FollowerTrait.TraitType> Traits;
    [Key(12)]
    public bool Golden;
    [Key(13)]
    public bool Rotting;
    [Key(14)]
    public bool RottingUnique;
    [Key(15)]
    public FollowerSpecialType Special;
  }

  [MessagePackObject(false)]
  [Serializable]
  public struct ClothingStruct(FollowerClothingType clothingType, string variant)
  {
    [Key(0)]
    public FollowerClothingType ClothingType = clothingType;
    [Key(1)]
    public string Variant = variant;
  }

  public enum ResearchState
  {
    Unresearched,
    Researching,
    Researched,
  }

  [MessagePackObject(false)]
  [Serializable]
  public class ResearchObject
  {
    [Key(0)]
    public StructureBrain.TYPES Type;
    [Key(1)]
    public float Progress;

    public ResearchObject()
    {
    }

    public ResearchObject(StructureBrain.TYPES Type) => this.Type = Type;

    [IgnoreMember]
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
