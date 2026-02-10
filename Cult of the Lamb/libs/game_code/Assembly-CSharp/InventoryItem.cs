// Decompiled with JetBrains decompiler
// Type: InventoryItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;
using MMRoomGeneration;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[MessagePackObject(false)]
public class InventoryItem
{
  [Key(0)]
  public int type;
  [Key(1)]
  public int quantity = 1;
  [Key(2)]
  public int QuantityReserved;
  public static Transform _layer;
  public static List<InventoryItem.ITEM_TYPE> AllAnimals = new List<InventoryItem.ITEM_TYPE>()
  {
    InventoryItem.ITEM_TYPE.ANIMAL_GOAT,
    InventoryItem.ITEM_TYPE.ANIMAL_COW,
    InventoryItem.ITEM_TYPE.ANIMAL_LLAMA,
    InventoryItem.ITEM_TYPE.ANIMAL_CRAB,
    InventoryItem.ITEM_TYPE.ANIMAL_SNAIL,
    InventoryItem.ITEM_TYPE.ANIMAL_SPIDER,
    InventoryItem.ITEM_TYPE.ANIMAL_TURTLE
  };
  public static List<InventoryItem.ITEM_TYPE> ItemsThatCanBeGivenToFollower = new List<InventoryItem.ITEM_TYPE>()
  {
    InventoryItem.ITEM_TYPE.GIFT_MEDIUM,
    InventoryItem.ITEM_TYPE.GIFT_SMALL,
    InventoryItem.ITEM_TYPE.Necklace_1,
    InventoryItem.ITEM_TYPE.Necklace_2,
    InventoryItem.ITEM_TYPE.Necklace_3,
    InventoryItem.ITEM_TYPE.Necklace_4,
    InventoryItem.ITEM_TYPE.Necklace_5,
    InventoryItem.ITEM_TYPE.Necklace_Dark,
    InventoryItem.ITEM_TYPE.Necklace_Demonic,
    InventoryItem.ITEM_TYPE.Necklace_Loyalty,
    InventoryItem.ITEM_TYPE.Necklace_Light,
    InventoryItem.ITEM_TYPE.Necklace_Missionary,
    InventoryItem.ITEM_TYPE.Necklace_Gold_Skull,
    InventoryItem.ITEM_TYPE.Necklace_Bell,
    InventoryItem.ITEM_TYPE.Necklace_Deaths_Door,
    InventoryItem.ITEM_TYPE.Necklace_Winter,
    InventoryItem.ITEM_TYPE.Necklace_Frozen,
    InventoryItem.ITEM_TYPE.Necklace_Weird,
    InventoryItem.ITEM_TYPE.Necklace_Targeted
  };
  public static List<InventoryItem.ITEM_TYPE> Necklaces_DLC = new List<InventoryItem.ITEM_TYPE>()
  {
    InventoryItem.ITEM_TYPE.Necklace_Deaths_Door,
    InventoryItem.ITEM_TYPE.Necklace_Winter,
    InventoryItem.ITEM_TYPE.Necklace_Frozen,
    InventoryItem.ITEM_TYPE.Necklace_Targeted,
    InventoryItem.ITEM_TYPE.Necklace_Weird
  };

  [IgnoreMember]
  public int UnreservedQuantity => this.quantity - this.QuantityReserved;

  public InventoryItem()
  {
    this.type = 0;
    this.quantity = 1;
  }

  public InventoryItem(InventoryItem.ITEM_TYPE Type)
  {
    this.type = (int) Type;
    this.quantity = 1;
  }

  public InventoryItem(InventoryItem.ITEM_TYPE Type, int Quantity)
  {
    this.type = (int) Type;
    this.quantity = Quantity;
  }

  public InventoryItem(InventoryItem inventoryItem)
  {
    this.type = inventoryItem.type;
    this.quantity = inventoryItem.quantity;
  }

  public void Init(int type, int quantity)
  {
    this.type = type;
    this.quantity = quantity;
  }

  public static void Spawn(
    GameObject ItemToSpawn,
    int quantity,
    Vector3 position,
    float StartSpeed = -1f)
  {
    if ((UnityEngine.Object) ItemToSpawn == (UnityEngine.Object) null)
      return;
    GameObject gameObject = ItemToSpawn.Spawn((UnityEngine.Object) RoomManager.Instance != (UnityEngine.Object) null ? RoomManager.Instance.CurrentRoomPrefab.transform : GameObject.FindGameObjectWithTag("Unit Layer").transform);
    gameObject.transform.position = position;
    gameObject.transform.eulerAngles = Vector3.zero;
    PickUp component = gameObject.GetComponent<PickUp>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || (double) StartSpeed == -1.0)
      return;
    component.Speed = StartSpeed;
  }

  public static BlackSoul SpawnBlackSoul(
    int quantity,
    Vector3 position,
    bool giveXP = false,
    bool simulated = false)
  {
    if (!DataManager.Instance.EnabledSpells || PlayerFarming.Location == FollowerLocation.IntroDungeon || PlayerFleeceManager.FleeceSwapsCurseForRelic())
      return (BlackSoul) null;
    int num = quantity;
    UnityEngine.Random.Range(0, 360);
    BlackSoul blackSoul = (BlackSoul) null;
    Transform Layer = !((UnityEngine.Object) MMBiomeGeneration.BiomeGenerator.Instance != (UnityEngine.Object) null) || MMBiomeGeneration.BiomeGenerator.Instance.CurrentRoom == null ? GameObject.FindGameObjectWithTag("Unit Layer").transform : MMBiomeGeneration.BiomeGenerator.Instance.CurrentRoom.GameObject.transform;
    while (--num >= 0)
    {
      if (BlackSoulUpdater.BlackSouls.Count >= 20 && SettingsManager.Settings.Graphics.EnvironmentDetail == 0)
      {
        ++PlayerFarming.LeftoverSouls;
      }
      else
      {
        blackSoul = BiomeConstants.Instance.SpawnBlackSouls(position, Layer, 360f / (float) quantity * (float) num, simulated);
        blackSoul.GiveXP = giveXP;
      }
    }
    return blackSoul;
  }

  public static InventoryItem.ITEM_CATEGORIES GetItemCategory(InventoryItem.ITEM_TYPE type)
  {
    switch (type)
    {
      case InventoryItem.ITEM_TYPE.LOG:
        return InventoryItem.ITEM_CATEGORIES.LOGS;
      case InventoryItem.ITEM_TYPE.SOUL:
        return InventoryItem.ITEM_CATEGORIES.SOULS;
      case InventoryItem.ITEM_TYPE.BLACK_GOLD:
        return InventoryItem.ITEM_CATEGORIES.COINS;
      case InventoryItem.ITEM_TYPE.GRASS:
        return InventoryItem.ITEM_CATEGORIES.GRASS;
      case InventoryItem.ITEM_TYPE.POOP:
      case InventoryItem.ITEM_TYPE.POOP_GOLD:
      case InventoryItem.ITEM_TYPE.POOP_RAINBOW:
      case InventoryItem.ITEM_TYPE.POOP_GLOW:
      case InventoryItem.ITEM_TYPE.POOP_DEVOTION:
      case InventoryItem.ITEM_TYPE.POOP_ROTSTONE:
        return InventoryItem.ITEM_CATEGORIES.POOP;
      default:
        return InventoryItem.ITEM_CATEGORIES.NONE;
    }
  }

  public static InventoryItem.ITEM_TYPE GetSeedType(InventoryItem.ITEM_TYPE type)
  {
    switch (type)
    {
      case InventoryItem.ITEM_TYPE.LOG:
        return InventoryItem.ITEM_TYPE.SEED_TREE;
      case InventoryItem.ITEM_TYPE.BERRY:
        return InventoryItem.ITEM_TYPE.SEED;
      case InventoryItem.ITEM_TYPE.MUSHROOM_SMALL:
        return InventoryItem.ITEM_TYPE.SEED_MUSHROOM;
      case InventoryItem.ITEM_TYPE.GRASS:
        return InventoryItem.ITEM_TYPE.GRASS;
      case InventoryItem.ITEM_TYPE.PUMPKIN:
        return InventoryItem.ITEM_TYPE.SEED_PUMPKIN;
      case InventoryItem.ITEM_TYPE.FLOWER_RED:
        return InventoryItem.ITEM_TYPE.SEED_FLOWER_RED;
      case InventoryItem.ITEM_TYPE.FLOWER_WHITE:
        return InventoryItem.ITEM_TYPE.SEED_FLOWER_WHITE;
      case InventoryItem.ITEM_TYPE.FLOWER_PURPLE:
        return InventoryItem.ITEM_TYPE.SEED_FLOWER_PURPLE;
      case InventoryItem.ITEM_TYPE.BEETROOT:
        return InventoryItem.ITEM_TYPE.SEED_BEETROOT;
      case InventoryItem.ITEM_TYPE.CAULIFLOWER:
        return InventoryItem.ITEM_TYPE.SEED_CAULIFLOWER;
      case InventoryItem.ITEM_TYPE.COTTON:
        return InventoryItem.ITEM_TYPE.SEED_COTTON;
      case InventoryItem.ITEM_TYPE.HOPS:
        return InventoryItem.ITEM_TYPE.SEED_HOPS;
      case InventoryItem.ITEM_TYPE.GRAPES:
        return InventoryItem.ITEM_TYPE.SEED_GRAPES;
      case InventoryItem.ITEM_TYPE.SNOW_FRUIT:
        return InventoryItem.ITEM_TYPE.SEED_SNOW_FRUIT;
      case InventoryItem.ITEM_TYPE.CHILLI:
        return InventoryItem.ITEM_TYPE.SEED_CHILLI;
      default:
        return InventoryItem.ITEM_TYPE.NONE;
    }
  }

  public static List<InventoryItem.ITEM_TYPE> AllSeeds
  {
    get
    {
      return new List<InventoryItem.ITEM_TYPE>()
      {
        InventoryItem.ITEM_TYPE.SEED,
        InventoryItem.ITEM_TYPE.SEED_PUMPKIN,
        InventoryItem.ITEM_TYPE.SEED_MUSHROOM,
        InventoryItem.ITEM_TYPE.SEED_FLOWER_RED,
        InventoryItem.ITEM_TYPE.SEED_BEETROOT,
        InventoryItem.ITEM_TYPE.SEED_CAULIFLOWER,
        InventoryItem.ITEM_TYPE.SEED_COTTON,
        InventoryItem.ITEM_TYPE.SEED_HOPS,
        InventoryItem.ITEM_TYPE.SEED_GRAPES,
        InventoryItem.ITEM_TYPE.SEED_SNOW_FRUIT,
        InventoryItem.ITEM_TYPE.SEED_CHILLI,
        InventoryItem.ITEM_TYPE.GRASS
      };
    }
  }

  public static InventoryItem.ITEM_TYPE GetAnimalResourceType(InventoryItem.ITEM_TYPE animal)
  {
    return animal == InventoryItem.ITEM_TYPE.ANIMAL_GOAT ? InventoryItem.ITEM_TYPE.MEAT : InventoryItem.ITEM_TYPE.NONE;
  }

  public static List<InventoryItem.ITEM_TYPE> AllPoops
  {
    get
    {
      return new List<InventoryItem.ITEM_TYPE>()
      {
        InventoryItem.ITEM_TYPE.POOP,
        InventoryItem.ITEM_TYPE.POOP_GOLD,
        InventoryItem.ITEM_TYPE.POOP_GLOW,
        InventoryItem.ITEM_TYPE.POOP_RAINBOW,
        InventoryItem.ITEM_TYPE.POOP_DEVOTION
      };
    }
  }

  public static List<InventoryItem.ITEM_TYPE> AllFlowers
  {
    get
    {
      return new List<InventoryItem.ITEM_TYPE>()
      {
        InventoryItem.ITEM_TYPE.FLOWER_RED,
        InventoryItem.ITEM_TYPE.FLOWER_WHITE,
        InventoryItem.ITEM_TYPE.FLOWER_PURPLE
      };
    }
  }

  public static List<InventoryItem.ITEM_TYPE> AllPlantables
  {
    get
    {
      return new List<InventoryItem.ITEM_TYPE>()
      {
        InventoryItem.ITEM_TYPE.BERRY,
        InventoryItem.ITEM_TYPE.PUMPKIN,
        InventoryItem.ITEM_TYPE.MUSHROOM_SMALL,
        InventoryItem.ITEM_TYPE.FLOWER_RED,
        InventoryItem.ITEM_TYPE.BEETROOT,
        InventoryItem.ITEM_TYPE.CAULIFLOWER,
        InventoryItem.ITEM_TYPE.COTTON,
        InventoryItem.ITEM_TYPE.HOPS,
        InventoryItem.ITEM_TYPE.GRAPES,
        InventoryItem.ITEM_TYPE.SNOW_FRUIT,
        InventoryItem.ITEM_TYPE.CHILLI,
        InventoryItem.ITEM_TYPE.GRASS
      };
    }
  }

  public static List<InventoryItem.ITEM_TYPE> AllFoods
  {
    get
    {
      return new List<InventoryItem.ITEM_TYPE>()
      {
        InventoryItem.ITEM_TYPE.BERRY,
        InventoryItem.ITEM_TYPE.PUMPKIN,
        InventoryItem.ITEM_TYPE.BEETROOT,
        InventoryItem.ITEM_TYPE.CAULIFLOWER,
        InventoryItem.ITEM_TYPE.FISH,
        InventoryItem.ITEM_TYPE.FISH_BIG,
        InventoryItem.ITEM_TYPE.FISH_BLOWFISH,
        InventoryItem.ITEM_TYPE.FISH_CRAB,
        InventoryItem.ITEM_TYPE.FISH_LOBSTER,
        InventoryItem.ITEM_TYPE.FISH_OCTOPUS,
        InventoryItem.ITEM_TYPE.FISH_SMALL,
        InventoryItem.ITEM_TYPE.FISH_SQUID,
        InventoryItem.ITEM_TYPE.FISH_SWORDFISH,
        InventoryItem.ITEM_TYPE.FISH_COD,
        InventoryItem.ITEM_TYPE.FISH_CATFISH,
        InventoryItem.ITEM_TYPE.FISH_PIKE,
        InventoryItem.ITEM_TYPE.POOP,
        InventoryItem.ITEM_TYPE.GRASS,
        InventoryItem.ITEM_TYPE.FOLLOWER_MEAT,
        InventoryItem.ITEM_TYPE.MEAT,
        InventoryItem.ITEM_TYPE.MEAT_MORSEL,
        InventoryItem.ITEM_TYPE.YOLK,
        InventoryItem.ITEM_TYPE.CHILLI,
        InventoryItem.ITEM_TYPE.MILK
      };
    }
  }

  public static List<InventoryItem.ITEM_TYPE> AllBurnableFuel
  {
    get
    {
      return new List<InventoryItem.ITEM_TYPE>()
      {
        InventoryItem.ITEM_TYPE.LOG,
        InventoryItem.ITEM_TYPE.GRASS
      };
    }
  }

  public static List<InventoryItem.ITEM_TYPE> AllBrokenWeapons
  {
    get
    {
      return new List<InventoryItem.ITEM_TYPE>()
      {
        InventoryItem.ITEM_TYPE.BROKEN_WEAPON_HAMMER,
        InventoryItem.ITEM_TYPE.BROKEN_WEAPON_SWORD,
        InventoryItem.ITEM_TYPE.BROKEN_WEAPON_DAGGER,
        InventoryItem.ITEM_TYPE.BROKEN_WEAPON_AXE,
        InventoryItem.ITEM_TYPE.BROKEN_WEAPON_BLUNDERBUSS,
        InventoryItem.ITEM_TYPE.BROKEN_WEAPON_GAUNTLETS,
        InventoryItem.ITEM_TYPE.BROKEN_WEAPON_CHAIN
      };
    }
  }

  public static List<InventoryItem.ITEM_TYPE> AllRepairedWeapons
  {
    get
    {
      return new List<InventoryItem.ITEM_TYPE>()
      {
        InventoryItem.ITEM_TYPE.REPAIRED_WEAPON_HAMMER,
        InventoryItem.ITEM_TYPE.REPAIRED_WEAPON_SWORD,
        InventoryItem.ITEM_TYPE.REPAIRED_WEAPON_DAGGER,
        InventoryItem.ITEM_TYPE.REPAIRED_WEAPON_BLUNDERBUSS,
        InventoryItem.ITEM_TYPE.REPAIRED_WEAPON_GAUNTLETS,
        InventoryItem.ITEM_TYPE.REPAIRED_WEAPON_CHAIN
      };
    }
  }

  public static List<InventoryItem.ITEM_TYPE> AllAnimalFood
  {
    get
    {
      return new List<InventoryItem.ITEM_TYPE>()
      {
        InventoryItem.ITEM_TYPE.GRASS,
        InventoryItem.ITEM_TYPE.BERRY,
        InventoryItem.ITEM_TYPE.PUMPKIN,
        InventoryItem.ITEM_TYPE.BEETROOT,
        InventoryItem.ITEM_TYPE.CAULIFLOWER,
        InventoryItem.ITEM_TYPE.SNOW_FRUIT,
        InventoryItem.ITEM_TYPE.FOLLOWER_MEAT
      };
    }
  }

  public static void Spawn(
    InventoryItem.ITEM_TYPE type,
    int quantity,
    Vector3 position,
    float startSpeed,
    FollowerLocation location,
    Action<PickUp> result = null)
  {
    if (PlayerFarming.Location != location)
    {
      List<Structures_CollectedResourceChest> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_CollectedResourceChest>(FollowerLocation.Base);
      if (structuresOfType.Count <= 0)
        return;
      structuresOfType[0].AddItem(type, quantity);
    }
    else
      InventoryItem.Spawn(type, quantity, position, startSpeed, result);
  }

  public static PickUp Spawn(
    InventoryItem.ITEM_TYPE type,
    int quantity,
    Vector3 position,
    float StartSpeed = 4f,
    Action<PickUp> result = null)
  {
    switch (type)
    {
      case InventoryItem.ITEM_TYPE.NONE:
        return (PickUp) null;
      case InventoryItem.ITEM_TYPE.FOUND_ITEM_OUTFIT:
        NotificationCentre.Instance.PlayGenericNotification("Notifications/UnlockedNewOutfit/Notification/On", NotificationBase.Flair.Positive);
        break;
    }
    if (DungeonSandboxManager.Active && type != InventoryItem.ITEM_TYPE.BLACK_GOLD && type != InventoryItem.ITEM_TYPE.RELIC && type != InventoryItem.ITEM_TYPE.HALF_HEART && type != InventoryItem.ITEM_TYPE.HALF_BLUE_HEART && type != InventoryItem.ITEM_TYPE.RED_HEART && type != InventoryItem.ITEM_TYPE.BLUE_HEART && type != InventoryItem.ITEM_TYPE.TRINKET_CARD && type != InventoryItem.ITEM_TYPE.GOD_TEAR)
      return (PickUp) null;
    if (PlayerFarming.Location == FollowerLocation.IntroDungeon && type != InventoryItem.ITEM_TYPE.BLACK_GOLD)
      return (PickUp) null;
    if (!DataManager.Instance.ShowLoyaltyBars && (type == InventoryItem.ITEM_TYPE.GIFT_SMALL || type == InventoryItem.ITEM_TYPE.GIFT_MEDIUM))
      type = InventoryItem.ITEM_TYPE.BLACK_GOLD;
    if (type == InventoryItem.ITEM_TYPE.LIGHTNING_SHARD && !DataManager.Instance.OnboardedLightningShardDungeon)
      DataManager.Instance.OnboardedLightningShardDungeon = true;
    if (type == InventoryItem.ITEM_TYPE.BLACK_GOLD)
    {
      List<PlayerFarming> players = PlayerFarming.players;
      float num = 1f;
      foreach (PlayerFarming playerFarming in players)
        num *= TrinketManager.GetCoinsDropMultiplier(playerFarming);
      quantity = Mathf.CeilToInt((float) quantity * num);
    }
    if ((type == InventoryItem.ITEM_TYPE.RED_HEART || type == InventoryItem.ITEM_TYPE.HALF_HEART) && DataManager.Instance.PlayerFleece == 5)
      type = type == InventoryItem.ITEM_TYPE.RED_HEART ? InventoryItem.ITEM_TYPE.BLUE_HEART : InventoryItem.ITEM_TYPE.HALF_BLUE_HEART;
    bool flag = false;
    string path = "";
    switch (type)
    {
      case InventoryItem.ITEM_TYPE.LOG:
        path = "Log";
        break;
      case InventoryItem.ITEM_TYPE.STONE:
        path = "Rock1";
        break;
      case InventoryItem.ITEM_TYPE.ROCK2:
        path = "Rock2";
        break;
      case InventoryItem.ITEM_TYPE.ROCK3:
        path = "Rock3";
        break;
      case InventoryItem.ITEM_TYPE.SEED_SWORD:
        path = "Seed - Sword";
        break;
      case InventoryItem.ITEM_TYPE.MEAT:
        path = "Meat";
        break;
      case InventoryItem.ITEM_TYPE.WHEAT:
        path = "Wheat";
        break;
      case InventoryItem.ITEM_TYPE.SEED:
        path = "Seed";
        break;
      case InventoryItem.ITEM_TYPE.BONE:
        if (!DataManager.Instance.BonesEnabled)
          return (PickUp) null;
        ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.DestroyBodiesForBones);
        path = "VileBones";
        break;
      case InventoryItem.ITEM_TYPE.SOUL:
        path = "Soul";
        break;
      case InventoryItem.ITEM_TYPE.VINES:
        path = "GildedVine";
        break;
      case InventoryItem.ITEM_TYPE.RED_HEART:
        if (PlayerFleeceManager.FleecePreventsHealthPickups() || DataManager.Instance.NoHeartDrops)
          return (PickUp) null;
        path = "Red Heart";
        break;
      case InventoryItem.ITEM_TYPE.HALF_HEART:
        if (PlayerFleeceManager.FleecePreventsHealthPickups(true) || DataManager.Instance.NoHeartDrops)
          return (PickUp) null;
        path = "Half Heart";
        break;
      case InventoryItem.ITEM_TYPE.BLUE_HEART:
        if (PlayerFleeceManager.FleecePreventsHealthPickups() || DataManager.Instance.NoHeartDrops)
          return (PickUp) null;
        path = "Blue Heart";
        break;
      case InventoryItem.ITEM_TYPE.HALF_BLUE_HEART:
        if (PlayerFleeceManager.FleecePreventsHealthPickups() || DataManager.Instance.NoHeartDrops)
          return (PickUp) null;
        path = "Half Blue Heart";
        break;
      case InventoryItem.ITEM_TYPE.TIME_TOKEN:
        path = "Time Token";
        break;
      case InventoryItem.ITEM_TYPE.GENERIC:
        path = "Generic Pick Up";
        break;
      case InventoryItem.ITEM_TYPE.STAINED_GLASS:
        path = "StainedGlass";
        break;
      case InventoryItem.ITEM_TYPE.FLOWERS:
        path = "SacredFlower";
        break;
      case InventoryItem.ITEM_TYPE.BLACK_GOLD:
        path = !DungeonSandboxManager.Active ? "BlackGold" : "ChallengeGold";
        break;
      case InventoryItem.ITEM_TYPE.BERRY:
        path = "Berries";
        break;
      case InventoryItem.ITEM_TYPE.MONSTER_HEART:
        path = "Monster Heart";
        break;
      case InventoryItem.ITEM_TYPE.TRINKET_CARD:
        path = "TarotCard";
        break;
      case InventoryItem.ITEM_TYPE.SOUL_FRAGMENT:
        path = "SoulFragment";
        break;
      case InventoryItem.ITEM_TYPE.FISH:
        path = "Fish";
        break;
      case InventoryItem.ITEM_TYPE.MUSHROOM_SMALL:
        path = "Mushroom Small";
        break;
      case InventoryItem.ITEM_TYPE.BLACK_SOUL:
        path = "Black Soul";
        break;
      case InventoryItem.ITEM_TYPE.MUSHROOM_BIG:
        path = "Mushroom Big";
        break;
      case InventoryItem.ITEM_TYPE.MEAL:
        path = "Assets/Prefabs/Structures/Other/Meal.prefab";
        flag = true;
        break;
      case InventoryItem.ITEM_TYPE.FISH_SMALL:
        path = "Fish Small";
        break;
      case InventoryItem.ITEM_TYPE.FISH_BIG:
        path = "Fish Big";
        break;
      case InventoryItem.ITEM_TYPE.GRASS:
        path = "Grass";
        break;
      case InventoryItem.ITEM_TYPE.THORNS:
        path = "Thorns";
        break;
      case InventoryItem.ITEM_TYPE.KEY_PIECE:
        path = "Key Piece";
        break;
      case InventoryItem.ITEM_TYPE.POOP:
        path = "Poop";
        break;
      case InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION:
        path = "FoundItem";
        break;
      case InventoryItem.ITEM_TYPE.FOUND_ITEM_WEAPON:
        path = "FoundItemWeapon";
        break;
      case InventoryItem.ITEM_TYPE.FOUND_ITEM_CURSE:
        path = "FoundItemCurse";
        break;
      case InventoryItem.ITEM_TYPE.GIFT_SMALL:
        path = "Gift Small";
        break;
      case InventoryItem.ITEM_TYPE.GIFT_MEDIUM:
        path = "Gift Medium";
        break;
      case InventoryItem.ITEM_TYPE.Necklace_1:
        path = "Necklace 1";
        break;
      case InventoryItem.ITEM_TYPE.Necklace_2:
        path = "Necklace 2";
        break;
      case InventoryItem.ITEM_TYPE.Necklace_3:
        path = "Necklace 3";
        break;
      case InventoryItem.ITEM_TYPE.Necklace_4:
        path = "Necklace 4";
        break;
      case InventoryItem.ITEM_TYPE.Necklace_5:
        path = "Necklace 5";
        break;
      case InventoryItem.ITEM_TYPE.PUMPKIN:
        path = "Pumpkin";
        break;
      case InventoryItem.ITEM_TYPE.SEED_PUMPKIN:
        path = "Seed Pumpkin";
        break;
      case InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN:
        path = "FoundItemSkin";
        break;
      case InventoryItem.ITEM_TYPE.BLACK_HEART:
        if (PlayerFleeceManager.FleecePreventsHealthPickups() || DataManager.Instance.NoHeartDrops)
          return (PickUp) null;
        path = "Black Heart";
        break;
      case InventoryItem.ITEM_TYPE.PERMANENT_HALF_HEART:
        if (PlayerFleeceManager.FleecePreventsHealthPickups() || DataManager.Instance.NoHeartDrops)
          return (PickUp) null;
        path = "Permanent Half Heart";
        break;
      case InventoryItem.ITEM_TYPE.FLOWER_RED:
        path = "Flower_red";
        break;
      case InventoryItem.ITEM_TYPE.FLOWER_WHITE:
        path = "Flower_White";
        break;
      case InventoryItem.ITEM_TYPE.MEAL_GRASS:
        path = "Assets/Prefabs/Structures/Other/Meal Grass.prefab";
        flag = true;
        break;
      case InventoryItem.ITEM_TYPE.MEAL_MEAT:
        path = "Assets/Prefabs/Structures/Other/Meal Good.prefab";
        flag = true;
        break;
      case InventoryItem.ITEM_TYPE.MEAL_GREAT:
        path = "Assets/Prefabs/Structures/Other/Meal Great.prefab";
        flag = true;
        break;
      case InventoryItem.ITEM_TYPE.MEAL_GOOD_FISH:
        path = "Assets/Prefabs/Structures/Other/Meal Good Fish.prefab";
        flag = true;
        break;
      case InventoryItem.ITEM_TYPE.MEAT_ROTTEN:
        path = "Meat Rotten";
        break;
      case InventoryItem.ITEM_TYPE.FOLLOWER_MEAT:
        path = "Follower Meat";
        break;
      case InventoryItem.ITEM_TYPE.FOLLOWER_MEAT_ROTTEN:
        path = "Follower Meat";
        break;
      case InventoryItem.ITEM_TYPE.MEAL_FOLLOWER_MEAT:
        path = "Assets/Prefabs/Structures/Other/Meal Follower Meat.prefab";
        flag = true;
        break;
      case InventoryItem.ITEM_TYPE.MEAL_POOP:
        path = "Assets/Prefabs/Structures/Other/Meal Poop.prefab";
        flag = true;
        break;
      case InventoryItem.ITEM_TYPE.SEED_MUSHROOM:
        path = "Seed Mushroom";
        break;
      case InventoryItem.ITEM_TYPE.SEED_FLOWER_WHITE:
        path = "Seed White Flower";
        break;
      case InventoryItem.ITEM_TYPE.SEED_FLOWER_RED:
        path = "Seed Red Flower";
        break;
      case InventoryItem.ITEM_TYPE.GRASS2:
        path = "Grass 2";
        break;
      case InventoryItem.ITEM_TYPE.GRASS3:
        path = "Grass 3";
        break;
      case InventoryItem.ITEM_TYPE.GRASS4:
        path = "Grass 4";
        break;
      case InventoryItem.ITEM_TYPE.GRASS5:
        path = "Grass 5";
        break;
      case InventoryItem.ITEM_TYPE.FLOWER_PURPLE:
        path = "Flower_Purple";
        break;
      case InventoryItem.ITEM_TYPE.SEED_TREE:
        path = "Seed_tree";
        break;
      case InventoryItem.ITEM_TYPE.MAP:
        path = "FoundItemMap";
        break;
      case InventoryItem.ITEM_TYPE.MEAL_MUSHROOMS:
        path = "Assets/Prefabs/Structures/Other/Meal Mushrooms.prefab";
        flag = true;
        break;
      case InventoryItem.ITEM_TYPE.LOG_REFINED:
        path = "Log Refined";
        break;
      case InventoryItem.ITEM_TYPE.STONE_REFINED:
        path = "Stone Refined";
        break;
      case InventoryItem.ITEM_TYPE.GOLD_NUGGET:
        path = "Gold Nugget";
        break;
      case InventoryItem.ITEM_TYPE.ROPE:
        path = "Rope";
        break;
      case InventoryItem.ITEM_TYPE.GOLD_REFINED:
        path = "Gold Refined";
        break;
      case InventoryItem.ITEM_TYPE.BLOOD_STONE:
        path = "Bloodstone";
        break;
      case InventoryItem.ITEM_TYPE.TRINKET_CARD_UNLOCKED:
        path = "TarotCardUnlocked";
        break;
      case InventoryItem.ITEM_TYPE.CRYSTAL:
        path = "Crystal";
        break;
      case InventoryItem.ITEM_TYPE.SPIDER_WEB:
        path = "Spider Web";
        break;
      case InventoryItem.ITEM_TYPE.FISH_CRAB:
        path = "Fish Crab";
        break;
      case InventoryItem.ITEM_TYPE.FISH_LOBSTER:
        path = "Fish Lobster";
        break;
      case InventoryItem.ITEM_TYPE.FISH_OCTOPUS:
        path = "Fish Octopus";
        break;
      case InventoryItem.ITEM_TYPE.FISH_SQUID:
        path = "Fish Squid";
        break;
      case InventoryItem.ITEM_TYPE.FISH_SWORDFISH:
        path = "Fish Swordfish";
        break;
      case InventoryItem.ITEM_TYPE.FISH_BLOWFISH:
        path = "Fish Blowfish";
        break;
      case InventoryItem.ITEM_TYPE.BEETROOT:
        path = "Beetroot";
        break;
      case InventoryItem.ITEM_TYPE.SEED_BEETROOT:
        path = "Seed Beetroot";
        break;
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_FISH:
        path = "Assets/Prefabs/Structures/Other/Meal Great Fish.prefab";
        flag = true;
        break;
      case InventoryItem.ITEM_TYPE.MEAL_BAD_FISH:
        path = "Assets/Prefabs/Structures/Other/Meal Bad Fish.prefab";
        flag = true;
        break;
      case InventoryItem.ITEM_TYPE.BEHOLDER_EYE:
        path = "Beholder Eye";
        break;
      case InventoryItem.ITEM_TYPE.CAULIFLOWER:
        path = "Cauliflower";
        break;
      case InventoryItem.ITEM_TYPE.SEED_CAULIFLOWER:
        path = "Seed Cauliflower";
        break;
      case InventoryItem.ITEM_TYPE.MEAT_MORSEL:
        path = "Meat Morsel";
        break;
      case InventoryItem.ITEM_TYPE.MEAL_BERRIES:
        path = "Assets/Prefabs/Structures/Other/Meal Berries.prefab";
        flag = true;
        break;
      case InventoryItem.ITEM_TYPE.MEAL_MEDIUM_VEG:
        path = "Assets/Prefabs/Structures/Other/Meal Medium Veg.prefab";
        flag = true;
        break;
      case InventoryItem.ITEM_TYPE.MEAL_BAD_MIXED:
        path = "Assets/Prefabs/Structures/Other/Meal Bad Mixed.prefab";
        flag = true;
        break;
      case InventoryItem.ITEM_TYPE.MEAL_MEDIUM_MIXED:
        path = "Assets/Prefabs/Structures/Other/Meal Medium Mixed.prefab";
        flag = true;
        break;
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_MIXED:
        path = "Assets/Prefabs/Structures/Other/Meal Great Mixed.prefab";
        flag = true;
        break;
      case InventoryItem.ITEM_TYPE.MEAL_DEADLY:
        path = "Assets/Prefabs/Structures/Other/Meal Deadly.prefab";
        flag = true;
        break;
      case InventoryItem.ITEM_TYPE.MEAL_BAD_MEAT:
        path = "Assets/Prefabs/Structures/Other/Meal Bad Meat.prefab";
        flag = true;
        break;
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_MEAT:
        path = "Assets/Prefabs/Structures/Other/Meal Great Meat.prefab";
        flag = true;
        break;
      case InventoryItem.ITEM_TYPE.MEAL_BURNED:
        path = "Assets/Prefabs/Structures/Other/Meal Burned.prefab";
        flag = true;
        break;
      case InventoryItem.ITEM_TYPE.DOCTRINE_STONE:
        if (!DoctrineUpgradeSystem.TrySermonsStillAvailable() || !DoctrineUpgradeSystem.TryGetStillDoctrineStone())
          return (PickUp) null;
        path = "Doctrine Stone Piece";
        break;
      case InventoryItem.ITEM_TYPE.SHELL:
        path = "Shell";
        break;
      case InventoryItem.ITEM_TYPE.RELIC:
        path = "Relic";
        break;
      case InventoryItem.ITEM_TYPE.GOD_TEAR:
        path = "God Tear";
        break;
      case InventoryItem.ITEM_TYPE.Necklace_Loyalty:
        path = "Necklace Loyalty";
        break;
      case InventoryItem.ITEM_TYPE.Necklace_Demonic:
        path = "Necklace Demonic";
        break;
      case InventoryItem.ITEM_TYPE.Necklace_Dark:
        path = "Necklace Dark";
        break;
      case InventoryItem.ITEM_TYPE.Necklace_Light:
        path = "Necklace Light";
        break;
      case InventoryItem.ITEM_TYPE.Necklace_Missionary:
        path = "Necklace Missionary";
        break;
      case InventoryItem.ITEM_TYPE.Necklace_Gold_Skull:
        path = "Necklace Gold Skull";
        break;
      case InventoryItem.ITEM_TYPE.WEBBER_SKULL:
        flag = true;
        path = "Assets/Prefabs/Structures/Buildings/Webber Skull.prefab";
        break;
      case InventoryItem.ITEM_TYPE.SNOW_CHUNK:
        flag = true;
        path = "Assets/Prefabs/Structures/Other/Snow Ball.prefab";
        break;
      case InventoryItem.ITEM_TYPE.CHARCOAL:
        path = "Charcoal";
        break;
      case InventoryItem.ITEM_TYPE.SILK_THREAD:
        path = "Silk Thread";
        break;
      case InventoryItem.ITEM_TYPE.COTTON:
        path = "Cotton";
        break;
      case InventoryItem.ITEM_TYPE.MEAL_SPICY:
        path = "Assets/Prefabs/Structures/Other/Meal Spicy.prefab";
        flag = true;
        break;
      case InventoryItem.ITEM_TYPE.FISH_COD:
        path = "Fish Cod";
        break;
      case InventoryItem.ITEM_TYPE.FISH_PIKE:
        path = "Fish Pike";
        break;
      case InventoryItem.ITEM_TYPE.FISH_CATFISH:
        path = "Fish Catfish";
        break;
      case InventoryItem.ITEM_TYPE.FOUND_ITEM_OUTFIT:
        path = "FoundItem_Outfit";
        break;
      case InventoryItem.ITEM_TYPE.LIGHTNING_SHARD:
        path = "Lightning Shard";
        break;
      case InventoryItem.ITEM_TYPE.SEED_COTTON:
        path = "Seed Cotton";
        break;
      case InventoryItem.ITEM_TYPE.LORE_STONE:
        path = "LoreStone";
        break;
      case InventoryItem.ITEM_TYPE.POOP_GOLD:
        path = "Poop Gold";
        break;
      case InventoryItem.ITEM_TYPE.POOP_RAINBOW:
        path = "Poop Rainbow";
        break;
      case InventoryItem.ITEM_TYPE.POOP_GLOW:
        path = "Poop Glow";
        break;
      case InventoryItem.ITEM_TYPE.HOPS:
        path = "Hops";
        break;
      case InventoryItem.ITEM_TYPE.GRAPES:
        path = "Grapes";
        break;
      case InventoryItem.ITEM_TYPE.SEED_HOPS:
        path = "Seed Hops";
        break;
      case InventoryItem.ITEM_TYPE.SEED_GRAPES:
        path = "Seed Grapes";
        break;
      case InventoryItem.ITEM_TYPE.PLEASURE_POINT:
        path = "Pleasure Point Pick Up";
        break;
      case InventoryItem.ITEM_TYPE.EGG_FOLLOWER:
        flag = true;
        path = "Assets/Prefabs/Structures/Buildings/Egg Follower.prefab";
        break;
      case InventoryItem.ITEM_TYPE.YOLK:
        path = "Yolk";
        break;
      case InventoryItem.ITEM_TYPE.SEED_SOZO:
        path = "Seed Sozo";
        break;
      case InventoryItem.ITEM_TYPE.MEAL_EGG:
        path = "Assets/Prefabs/Structures/Other/Meal Egg.prefab";
        flag = true;
        break;
      case InventoryItem.ITEM_TYPE.POOP_DEVOTION:
        path = "Poop Devotion";
        break;
      case InventoryItem.ITEM_TYPE.Necklace_Bell:
        path = "Necklace Bell";
        break;
      case InventoryItem.ITEM_TYPE.WOOL:
        path = "Wool";
        break;
      case InventoryItem.ITEM_TYPE.SEED_SNOW_FRUIT:
        path = "Seed Snow Fruit";
        break;
      case InventoryItem.ITEM_TYPE.SNOW_FRUIT:
        path = "SnowFruit";
        break;
      case InventoryItem.ITEM_TYPE.CHILLI:
        path = "Chilli";
        break;
      case InventoryItem.ITEM_TYPE.SEED_CHILLI:
        path = "Seed Chilli";
        break;
      case InventoryItem.ITEM_TYPE.ANIMAL_GOAT:
        path = "Animal Goat";
        break;
      case InventoryItem.ITEM_TYPE.MEAL_SNOW_FRUIT:
        path = "Assets/Prefabs/Structures/Other/Meal Snow Fruit.prefab";
        flag = true;
        break;
      case InventoryItem.ITEM_TYPE.MAGMA_STONE:
        path = "Magma Stone";
        break;
      case InventoryItem.ITEM_TYPE.ELECTRIFIED_MAGMA:
        path = "Electrified Magma";
        break;
      case InventoryItem.ITEM_TYPE.FIRE_HEART:
        if (PlayerFleeceManager.FleecePreventsHealthPickups() || DataManager.Instance.NoHeartDrops)
          return (PickUp) null;
        path = "Fire Heart";
        break;
      case InventoryItem.ITEM_TYPE.ICE_HEART:
        if (PlayerFleeceManager.FleecePreventsHealthPickups() || DataManager.Instance.NoHeartDrops)
          return (PickUp) null;
        path = "Ice Heart";
        break;
      case InventoryItem.ITEM_TYPE.ANIMAL_TURTLE:
        path = "Animal Turtle";
        break;
      case InventoryItem.ITEM_TYPE.ANIMAL_CRAB:
        path = "Animal Crab";
        break;
      case InventoryItem.ITEM_TYPE.ANIMAL_SPIDER:
        path = "Animal Spider";
        break;
      case InventoryItem.ITEM_TYPE.ANIMAL_SNAIL:
        path = "Animal Snail";
        break;
      case InventoryItem.ITEM_TYPE.Necklace_Deaths_Door:
        path = "Necklace Deaths Door";
        break;
      case InventoryItem.ITEM_TYPE.Necklace_Winter:
        path = "Necklace Winter";
        break;
      case InventoryItem.ITEM_TYPE.Necklace_Frozen:
        path = "Necklace Frozen";
        break;
      case InventoryItem.ITEM_TYPE.Necklace_Weird:
        path = "Necklace Weird";
        break;
      case InventoryItem.ITEM_TYPE.Necklace_Targeted:
        path = "Necklace Targeted";
        break;
      case InventoryItem.ITEM_TYPE.SOOT:
        path = "Soot";
        break;
      case InventoryItem.ITEM_TYPE.POOP_ROTSTONE:
        path = "Poop Rotstone";
        break;
      case InventoryItem.ITEM_TYPE.ANIMAL_COW:
        path = "Animal Cow";
        break;
      case InventoryItem.ITEM_TYPE.ANIMAL_LLAMA:
        path = "Animal Llama";
        break;
      case InventoryItem.ITEM_TYPE.BROKEN_WEAPON_HAMMER:
      case InventoryItem.ITEM_TYPE.BROKEN_WEAPON_SWORD:
      case InventoryItem.ITEM_TYPE.BROKEN_WEAPON_DAGGER:
      case InventoryItem.ITEM_TYPE.BROKEN_WEAPON_GAUNTLETS:
      case InventoryItem.ITEM_TYPE.BROKEN_WEAPON_AXE:
      case InventoryItem.ITEM_TYPE.BROKEN_WEAPON_BLUNDERBUSS:
      case InventoryItem.ITEM_TYPE.BROKEN_WEAPON_CHAIN:
      case InventoryItem.ITEM_TYPE.REPAIRED_WEAPON_HAMMER:
      case InventoryItem.ITEM_TYPE.REPAIRED_WEAPON_SWORD:
      case InventoryItem.ITEM_TYPE.REPAIRED_WEAPON_DAGGER:
      case InventoryItem.ITEM_TYPE.REPAIRED_WEAPON_GAUNTLETS:
      case InventoryItem.ITEM_TYPE.REPAIRED_WEAPON_BLUNDERBUSS:
      case InventoryItem.ITEM_TYPE.REPAIRED_WEAPON_CHAIN:
      case InventoryItem.ITEM_TYPE.LEGENDARY_WEAPON_FRAGMENT:
        path = "Broken Legendary Weapon";
        break;
      case InventoryItem.ITEM_TYPE.MILK:
        path = "Milk";
        break;
      case InventoryItem.ITEM_TYPE.MEAL_MILK_BAD:
        path = "Assets/Prefabs/Structures/Other/Meal Milk Bad.prefab";
        flag = true;
        break;
      case InventoryItem.ITEM_TYPE.MEAL_MILK_GOOD:
        path = "Assets/Prefabs/Structures/Other/Meal Milk Good.prefab";
        flag = true;
        break;
      case InventoryItem.ITEM_TYPE.MEAL_MILK_GREAT:
        path = "Assets/Prefabs/Structures/Other/Meal Milk Great.prefab";
        flag = true;
        break;
      case InventoryItem.ITEM_TYPE.YNGYA_GHOST:
        path = "Yngya Ghost";
        break;
      case InventoryItem.ITEM_TYPE.SPECIAL_WOOL_RANCHER:
        path = "Fleece1";
        break;
      case InventoryItem.ITEM_TYPE.SPECIAL_WOOL_LAMBWAR:
        path = "Fleece2";
        break;
      case InventoryItem.ITEM_TYPE.SPECIAL_WOOL_BLACKSMITH:
        path = "Fleece3";
        break;
      case InventoryItem.ITEM_TYPE.SPECIAL_WOOL_TAROT:
        path = "Fleece4";
        break;
      case InventoryItem.ITEM_TYPE.SPECIAL_WOOL_DECORATION:
        path = "Fleece5";
        break;
      case InventoryItem.ITEM_TYPE.SPECIAL_WOOL_6:
        path = "Fleece6";
        break;
      case InventoryItem.ITEM_TYPE.SPECIAL_WOOL_7:
        path = "Fleece7";
        break;
      case InventoryItem.ITEM_TYPE.SPECIAL_WOOL_8:
        path = "Fleece8";
        break;
      case InventoryItem.ITEM_TYPE.SPECIAL_WOOL_9:
        path = "Fleece9";
        break;
      case InventoryItem.ITEM_TYPE.SPECIAL_WOOL_10:
        path = "Fleece10";
        break;
      case InventoryItem.ITEM_TYPE.RATAU_STAFF:
        path = "Ratau Staff";
        break;
      case InventoryItem.ITEM_TYPE.BOP:
        path = "Bop";
        break;
      case InventoryItem.ITEM_TYPE.FLOCKADE_PIECE:
        path = "FlockadePieceCollectable";
        break;
      case InventoryItem.ITEM_TYPE.YEW_CURSED:
        path = "Yew Cursed";
        break;
      case InventoryItem.ITEM_TYPE.YEW_HOLY:
        path = "Yew Holy";
        break;
      case InventoryItem.ITEM_TYPE.BEHOLDER_EYE_ROT:
        path = "Beholder Eye Rot";
        break;
      case InventoryItem.ITEM_TYPE.SEED_FLOWER_PURPLE:
        path = "Seed Purple Flower";
        break;
      case InventoryItem.ITEM_TYPE.SPECIAL_WOOL_11:
        path = "Fleece11";
        break;
      default:
        Debug.Log((object) ("failed to get: " + type.ToString()));
        break;
    }
    if ((UnityEngine.Object) InventoryItem._layer == (UnityEngine.Object) null || !InventoryItem._layer.gameObject.activeInHierarchy)
    {
      Transform transform = GameObject.FindGameObjectWithTag("Unit Layer")?.transform;
      if ((UnityEngine.Object) transform != (UnityEngine.Object) null)
        InventoryItem._layer = transform;
    }
    PickUp p = (PickUp) null;
    while (--quantity >= 0)
    {
      if (MMBiomeGeneration.BiomeGenerator.Instance?.CurrentRoom != null)
        InventoryItem._layer = MMBiomeGeneration.BiomeGenerator.Instance.CurrentRoom.GameObject.transform;
      if ((UnityEngine.Object) InventoryItem._layer == (UnityEngine.Object) null && (UnityEngine.Object) GenerateRoom.Instance != (UnityEngine.Object) null)
        InventoryItem._layer = GenerateRoom.Instance.transform;
      if (!((UnityEngine.Object) InventoryItem._layer == (UnityEngine.Object) null))
      {
        if (flag)
          ObjectPool.Spawn(path, position, Quaternion.identity, InventoryItem._layer, (Action<GameObject>) (obj =>
          {
            obj.transform.position = position;
            p = obj.GetComponent<PickUp>();
            if ((UnityEngine.Object) p != (UnityEngine.Object) null)
              p.Speed = StartSpeed;
            Action<PickUp> action = result;
            if (action == null)
              return;
            action(p);
          }));
        else
          ObjectPool.Spawn("Prefabs/Resources/" + path, position, Quaternion.identity, InventoryItem._layer, (Action<GameObject>) (obj =>
          {
            obj.transform.position = position;
            obj.transform.eulerAngles = Vector3.zero;
            p = obj.GetComponent<PickUp>();
            if ((UnityEngine.Object) p != (UnityEngine.Object) null)
              p.Speed = StartSpeed;
            Action<PickUp> action = result;
            if (action == null)
              return;
            action(p);
          }), false);
      }
      else
        break;
    }
    return p;
  }

  public static bool IsGiftOrNecklace(InventoryItem.ITEM_TYPE type)
  {
    return DataManager.AllGifts.Contains(type) || DataManager.AllNecklaces.Contains(type);
  }

  public static string Name(InventoryItem.ITEM_TYPE Type) => InventoryItem.LocalizedName(Type);

  public static string LocalizedName(InventoryItem.ITEM_TYPE Type)
  {
    return LocalizationManager.GetTranslation($"Inventory/{Type}");
  }

  public static string LocalizedLore(InventoryItem.ITEM_TYPE Type)
  {
    return LocalizationManager.GetTranslation($"Inventory/{Type}/Lore");
  }

  public static string LocalizedDescription(InventoryItem.ITEM_TYPE Type)
  {
    return LocalizationManager.GetTranslation($"Inventory/{Type}/Description");
  }

  public static string Lore(InventoryItem.ITEM_TYPE Type)
  {
    return InventoryItem.LocalizedDescription(Type);
  }

  public static string Description(InventoryItem.ITEM_TYPE Type)
  {
    switch (Type)
    {
      case InventoryItem.ITEM_TYPE.LOG:
        return "A simple material used for building structures.";
      case InventoryItem.ITEM_TYPE.STONE:
        return "Stone";
      case InventoryItem.ITEM_TYPE.MEAT:
        return "Meat";
      case InventoryItem.ITEM_TYPE.WHEAT:
        return "Wheat";
      case InventoryItem.ITEM_TYPE.BONE:
        return "Bone";
      case InventoryItem.ITEM_TYPE.VINES:
        return "Vines";
      case InventoryItem.ITEM_TYPE.RED_HEART:
        return "Heart";
      case InventoryItem.ITEM_TYPE.HALF_HEART:
        return "Half Heart";
      case InventoryItem.ITEM_TYPE.BLUE_HEART:
        return "Additional HP that is not permenant, once lost they cannot be replenished.";
      case InventoryItem.ITEM_TYPE.HALF_BLUE_HEART:
        return "Half Blue Heart";
      case InventoryItem.ITEM_TYPE.TIME_TOKEN:
        return "Time Token";
      case InventoryItem.ITEM_TYPE.MONSTER_HEART:
        return "The heart of a vile monstrosity, slain by your hand.";
      case InventoryItem.ITEM_TYPE.BLUE_PRINT:
        return "Blueprint";
      case InventoryItem.ITEM_TYPE.MUSHROOM_SMALL:
        return "Mushroom Sample";
      case InventoryItem.ITEM_TYPE.MUSHROOM_BIG:
        return "Metricide Mushroom";
      case InventoryItem.ITEM_TYPE.GRASS:
        return "A simple material used for building structures.";
      case InventoryItem.ITEM_TYPE.POOP:
        return "Rich in... nutrients...";
      case InventoryItem.ITEM_TYPE.BLOOD_STONE:
        return "Blood Stone";
      default:
        Debug.Log((object) (Type.ToString() + " description not set"));
        return "Not Set";
    }
  }

  public static int FuelWeight(int type)
  {
    return InventoryItem.FuelWeight((InventoryItem.ITEM_TYPE) type);
  }

  public static int FuelWeight(InventoryItem.ITEM_TYPE type)
  {
    switch (type)
    {
      case InventoryItem.ITEM_TYPE.LOG:
        return 13;
      case InventoryItem.ITEM_TYPE.MEAT:
      case InventoryItem.ITEM_TYPE.BERRY:
      case InventoryItem.ITEM_TYPE.FISH:
      case InventoryItem.ITEM_TYPE.MUSHROOM_SMALL:
      case InventoryItem.ITEM_TYPE.MUSHROOM_BIG:
      case InventoryItem.ITEM_TYPE.FISH_SMALL:
      case InventoryItem.ITEM_TYPE.FISH_BIG:
      case InventoryItem.ITEM_TYPE.PUMPKIN:
      case InventoryItem.ITEM_TYPE.FOLLOWER_MEAT:
      case InventoryItem.ITEM_TYPE.FOLLOWER_MEAT_ROTTEN:
      case InventoryItem.ITEM_TYPE.FISH_CRAB:
      case InventoryItem.ITEM_TYPE.FISH_LOBSTER:
      case InventoryItem.ITEM_TYPE.FISH_OCTOPUS:
      case InventoryItem.ITEM_TYPE.FISH_SQUID:
      case InventoryItem.ITEM_TYPE.FISH_SWORDFISH:
      case InventoryItem.ITEM_TYPE.FISH_BLOWFISH:
      case InventoryItem.ITEM_TYPE.BEETROOT:
      case InventoryItem.ITEM_TYPE.CAULIFLOWER:
      case InventoryItem.ITEM_TYPE.FISH_COD:
      case InventoryItem.ITEM_TYPE.FISH_PIKE:
      case InventoryItem.ITEM_TYPE.FISH_CATFISH:
      case InventoryItem.ITEM_TYPE.CHILLI:
        return 1;
      case InventoryItem.ITEM_TYPE.GRASS:
        return 3;
      case InventoryItem.ITEM_TYPE.GOLD_REFINED:
        return 15;
      case InventoryItem.ITEM_TYPE.LIGHTNING_SHARD:
        return 5;
      case InventoryItem.ITEM_TYPE.MAGMA_STONE:
        return 14700;
      default:
        return 1;
    }
  }

  public static int FoodSatitation(InventoryItem.ITEM_TYPE Type)
  {
    return CookingData.GetSatationAmount(Type);
  }

  public static bool IsFish(InventoryItem.ITEM_TYPE Type)
  {
    switch (Type)
    {
      case InventoryItem.ITEM_TYPE.FISH:
      case InventoryItem.ITEM_TYPE.FISH_SMALL:
      case InventoryItem.ITEM_TYPE.FISH_BIG:
      case InventoryItem.ITEM_TYPE.FISH_CRAB:
      case InventoryItem.ITEM_TYPE.FISH_LOBSTER:
      case InventoryItem.ITEM_TYPE.FISH_OCTOPUS:
      case InventoryItem.ITEM_TYPE.FISH_SQUID:
      case InventoryItem.ITEM_TYPE.FISH_SWORDFISH:
      case InventoryItem.ITEM_TYPE.FISH_BLOWFISH:
      case InventoryItem.ITEM_TYPE.FISH_COD:
      case InventoryItem.ITEM_TYPE.FISH_PIKE:
      case InventoryItem.ITEM_TYPE.FISH_CATFISH:
        return true;
      default:
        return false;
    }
  }

  public static bool IsMeat(InventoryItem.ITEM_TYPE Type)
  {
    return Type == InventoryItem.ITEM_TYPE.MEAT || Type == InventoryItem.ITEM_TYPE.FOLLOWER_MEAT || Type == InventoryItem.ITEM_TYPE.MEAT_MORSEL;
  }

  public static bool IsFood(InventoryItem.ITEM_TYPE Type)
  {
    if (InventoryItem.IsFish(Type))
      return true;
    switch (Type)
    {
      case InventoryItem.ITEM_TYPE.MEAT:
      case InventoryItem.ITEM_TYPE.BERRY:
      case InventoryItem.ITEM_TYPE.PUMPKIN:
      case InventoryItem.ITEM_TYPE.BEETROOT:
      case InventoryItem.ITEM_TYPE.CAULIFLOWER:
      case InventoryItem.ITEM_TYPE.MEAT_MORSEL:
      case InventoryItem.ITEM_TYPE.SNOW_FRUIT:
      case InventoryItem.ITEM_TYPE.CHILLI:
        return true;
      default:
        return false;
    }
  }

  public static bool IsBigFish(InventoryItem.ITEM_TYPE Type)
  {
    switch (Type)
    {
      case InventoryItem.ITEM_TYPE.FISH_BIG:
      case InventoryItem.ITEM_TYPE.FISH_LOBSTER:
      case InventoryItem.ITEM_TYPE.FISH_OCTOPUS:
      case InventoryItem.ITEM_TYPE.FISH_SWORDFISH:
      case InventoryItem.ITEM_TYPE.FISH_COD:
        return true;
      default:
        return false;
    }
  }

  public static bool IsHeart(InventoryItem.ITEM_TYPE Type)
  {
    switch (Type)
    {
      case InventoryItem.ITEM_TYPE.RED_HEART:
      case InventoryItem.ITEM_TYPE.HALF_HEART:
      case InventoryItem.ITEM_TYPE.BLUE_HEART:
      case InventoryItem.ITEM_TYPE.HALF_BLUE_HEART:
      case InventoryItem.ITEM_TYPE.BLACK_HEART:
      case InventoryItem.ITEM_TYPE.PERMANENT_HALF_HEART:
        return true;
      default:
        return false;
    }
  }

  public static bool HasGifts()
  {
    foreach (int type in InventoryItem.ItemsThatCanBeGivenToFollower)
    {
      if (Inventory.GetItemByType(type) != null)
        return true;
    }
    return false;
  }

  public static bool CanBeGivenToFollower(InventoryItem.ITEM_TYPE Type)
  {
    return InventoryItem.ItemsThatCanBeGivenToFollower.Contains(Type);
  }

  public static Action<Follower, InventoryItem.ITEM_TYPE, System.Action> GiveToFollowerCallbacks(
    InventoryItem.ITEM_TYPE Type)
  {
    switch (Type)
    {
      case InventoryItem.ITEM_TYPE.MEAT:
      case InventoryItem.ITEM_TYPE.BERRY:
      case InventoryItem.ITEM_TYPE.FISH:
      case InventoryItem.ITEM_TYPE.FISH_SMALL:
      case InventoryItem.ITEM_TYPE.FISH_BIG:
        return new Action<Follower, InventoryItem.ITEM_TYPE, System.Action>(InventoryItem.OnFeedFood);
      case InventoryItem.ITEM_TYPE.MUSHROOM_BIG:
        return new Action<Follower, InventoryItem.ITEM_TYPE, System.Action>(InventoryItem.OnFeedBigMushroom);
      case InventoryItem.ITEM_TYPE.GIFT_SMALL:
      case InventoryItem.ITEM_TYPE.GIFT_MEDIUM:
        return new Action<Follower, InventoryItem.ITEM_TYPE, System.Action>(InventoryItem.OnGetGift);
      case InventoryItem.ITEM_TYPE.Necklace_1:
      case InventoryItem.ITEM_TYPE.Necklace_2:
      case InventoryItem.ITEM_TYPE.Necklace_3:
      case InventoryItem.ITEM_TYPE.Necklace_4:
      case InventoryItem.ITEM_TYPE.Necklace_5:
      case InventoryItem.ITEM_TYPE.Necklace_Loyalty:
      case InventoryItem.ITEM_TYPE.Necklace_Demonic:
      case InventoryItem.ITEM_TYPE.Necklace_Dark:
      case InventoryItem.ITEM_TYPE.Necklace_Light:
      case InventoryItem.ITEM_TYPE.Necklace_Missionary:
      case InventoryItem.ITEM_TYPE.Necklace_Gold_Skull:
      case InventoryItem.ITEM_TYPE.Necklace_Bell:
      case InventoryItem.ITEM_TYPE.Necklace_Deaths_Door:
      case InventoryItem.ITEM_TYPE.Necklace_Winter:
      case InventoryItem.ITEM_TYPE.Necklace_Frozen:
      case InventoryItem.ITEM_TYPE.Necklace_Weird:
      case InventoryItem.ITEM_TYPE.Necklace_Targeted:
        return new Action<Follower, InventoryItem.ITEM_TYPE, System.Action>(InventoryItem.OnGetNecklace);
      case InventoryItem.ITEM_TYPE.ANIMAL_GOAT:
      case InventoryItem.ITEM_TYPE.ANIMAL_TURTLE:
      case InventoryItem.ITEM_TYPE.ANIMAL_CRAB:
      case InventoryItem.ITEM_TYPE.ANIMAL_SPIDER:
      case InventoryItem.ITEM_TYPE.ANIMAL_SNAIL:
      case InventoryItem.ITEM_TYPE.ANIMAL_COW:
      case InventoryItem.ITEM_TYPE.ANIMAL_LLAMA:
        return new Action<Follower, InventoryItem.ITEM_TYPE, System.Action>(InventoryItem.OnGetPet);
      default:
        return (Action<Follower, InventoryItem.ITEM_TYPE, System.Action>) null;
    }
  }

  public static void OnGetNecklace(
    Follower follower,
    InventoryItem.ITEM_TYPE ItemType,
    System.Action Callback)
  {
    Debug.Log((object) "Spawning necklace");
    if (follower.Brain.Info.Necklace != InventoryItem.ITEM_TYPE.NONE)
      InventoryItem.Spawn(follower.Brain.Info.Necklace, 1, follower.transform.position);
    follower.Brain.Info.Necklace = ItemType;
    follower.SetOutfit(FollowerOutfitType.Follower, false);
    follower.Brain.AddThought(Thought.ReceivedNecklace);
    if (ItemType == InventoryItem.ITEM_TYPE.Necklace_Gold_Skull && !follower.Brain.HasTrait(FollowerTrait.TraitType.Immortal))
      follower.Brain.AddTrait(FollowerTrait.TraitType.Immortal);
    if (ItemType == InventoryItem.ITEM_TYPE.Necklace_5)
      follower.Brain.Stats.Rest = 100f;
    follower.TimedAnimation("Reactions/react-enlightened1", 2f, (System.Action) (() =>
    {
      System.Action action = Callback;
      if (action == null)
        return;
      action();
    }), false, false);
    follower.AddBodyAnimation("idle", true, 0.0f);
  }

  public static void OnGetGift(Follower follower, InventoryItem.ITEM_TYPE Item, System.Action Callback)
  {
    follower.Brain.AddThought(Thought.ReceivedGift);
    follower.TimedAnimation("Reactions/react-enlightened1", 2f, (System.Action) (() =>
    {
      System.Action action = Callback;
      if (action == null)
        return;
      action();
    }), false, false);
    follower.AddBodyAnimation("idle", true, 0.0f);
  }

  public static void OnGetPet(Follower follower, InventoryItem.ITEM_TYPE ItemType, System.Action Callback)
  {
    follower.Brain.AddThought(Thought.ReceivedGift);
    follower.TimedAnimation("Reactions/react-enlightened1", 2f, (System.Action) (() =>
    {
      System.Action action = Callback;
      if (action == null)
        return;
      action();
    }), false, false);
    follower.AddBodyAnimation("idle", true, 0.0f);
  }

  public static void OnFeedFood(Follower follower, InventoryItem.ITEM_TYPE Item, System.Action Callback)
  {
    follower.TimedAnimation("Food/food_eat", 2f, (System.Action) (() =>
    {
      follower.Brain.Stats.Satiation += (float) InventoryItem.FoodSatitation(Item);
      follower.Brain.Stats.TargetBathroom = 30f;
      System.Action action = Callback;
      if (action == null)
        return;
      action();
    }), useDeltaTime: false);
    switch (Item)
    {
      case InventoryItem.ITEM_TYPE.MEAL:
        follower.Brain.AddThought(Thought.AteMeal);
        break;
      case InventoryItem.ITEM_TYPE.MEAL_GRASS:
        follower.Brain.AddThought(Thought.AteSpecialMealBad);
        break;
      case InventoryItem.ITEM_TYPE.MEAL_MEAT:
        follower.Brain.AddThought(Thought.AteGoodMeal);
        break;
      case InventoryItem.ITEM_TYPE.MEAL_GREAT:
        follower.Brain.AddThought(Thought.AteSpecialMealGood);
        break;
      case InventoryItem.ITEM_TYPE.MEAL_GOOD_FISH:
        follower.Brain.AddThought(Thought.AteGoodMealFish);
        break;
      default:
        follower.Brain.AddThought(Thought.AteRawFood);
        break;
    }
  }

  public static void OnFeedBigMushroom(
    Follower follower,
    InventoryItem.ITEM_TYPE InventoryItem,
    System.Action Callback)
  {
    follower.TimedAnimation("Food/food_eat", 2f, (System.Action) (() =>
    {
      follower.Brain.Stats.Brainwash(follower.Brain);
      if ((double) UnityEngine.Random.Range(0.0f, 1f) <= 0.20000000298023224)
        follower.Brain.Stats.Illness = 100f;
      System.Action action = Callback;
      if (action == null)
        return;
      action();
    }), useDeltaTime: false);
  }

  public static string CapacityString(InventoryItem.ITEM_TYPE type, int minimum)
  {
    int itemQuantity = Inventory.GetItemQuantity(type);
    string str = $"{FontImageNames.GetIconByType(type)} {itemQuantity}/{minimum}";
    if (itemQuantity < minimum)
      str = str.Colour(StaticColors.RedColor);
    LocalizeIntegration.ReverseText(str);
    return str;
  }

  public static InventoryItem.ITEM_TYPE GetInventoryItemTypeOf(GameObject pickupObject)
  {
    PickUp component = pickupObject.GetComponent<PickUp>();
    return (UnityEngine.Object) component != (UnityEngine.Object) null ? component.type : InventoryItem.ITEM_TYPE.NONE;
  }

  public static InventoryItem.ITEM_TYPE GetPoopTypeFromStructure(StructureBrain.TYPES type)
  {
    switch (type)
    {
      case StructureBrain.TYPES.POOP_GOLD:
        return InventoryItem.ITEM_TYPE.POOP_GOLD;
      case StructureBrain.TYPES.POOP_RAINBOW:
        return InventoryItem.ITEM_TYPE.POOP_RAINBOW;
      case StructureBrain.TYPES.POOP_DEVOTION:
        return InventoryItem.ITEM_TYPE.POOP_DEVOTION;
      case StructureBrain.TYPES.POOP_GLOW:
        return InventoryItem.ITEM_TYPE.POOP_GLOW;
      case StructureBrain.TYPES.POOP_ROTSTONE:
        return InventoryItem.ITEM_TYPE.POOP_ROTSTONE;
      default:
        return InventoryItem.ITEM_TYPE.POOP;
    }
  }

  public static InventoryItem.ITEM_TYPE GetBrokenWeaponTypeFromEquipmentType(EquipmentType type)
  {
    switch (type)
    {
      case EquipmentType.Sword_Legendary:
        return InventoryItem.ITEM_TYPE.BROKEN_WEAPON_SWORD;
      case EquipmentType.Axe_Legendary:
        return InventoryItem.ITEM_TYPE.BROKEN_WEAPON_AXE;
      case EquipmentType.Hammer_Legendary:
        return InventoryItem.ITEM_TYPE.BROKEN_WEAPON_HAMMER;
      case EquipmentType.Dagger_Legendary:
        return InventoryItem.ITEM_TYPE.BROKEN_WEAPON_DAGGER;
      case EquipmentType.Gauntlet_Legendary:
        return InventoryItem.ITEM_TYPE.BROKEN_WEAPON_GAUNTLETS;
      case EquipmentType.Blunderbuss_Legendary:
        return InventoryItem.ITEM_TYPE.BROKEN_WEAPON_BLUNDERBUSS;
      case EquipmentType.Chain_Legendary:
        return InventoryItem.ITEM_TYPE.BROKEN_WEAPON_CHAIN;
      default:
        return InventoryItem.ITEM_TYPE.NONE;
    }
  }

  public static InventoryItem.ITEM_TYPE GetRepairedWeaponTypeFromEquipmentType(EquipmentType type)
  {
    switch (type)
    {
      case EquipmentType.Sword_Legendary:
        return InventoryItem.ITEM_TYPE.REPAIRED_WEAPON_SWORD;
      case EquipmentType.Hammer_Legendary:
        return InventoryItem.ITEM_TYPE.REPAIRED_WEAPON_HAMMER;
      case EquipmentType.Dagger_Legendary:
        return InventoryItem.ITEM_TYPE.REPAIRED_WEAPON_DAGGER;
      case EquipmentType.Gauntlet_Legendary:
        return InventoryItem.ITEM_TYPE.REPAIRED_WEAPON_GAUNTLETS;
      case EquipmentType.Blunderbuss_Legendary:
        return InventoryItem.ITEM_TYPE.REPAIRED_WEAPON_BLUNDERBUSS;
      case EquipmentType.Chain_Legendary:
        return InventoryItem.ITEM_TYPE.REPAIRED_WEAPON_CHAIN;
      default:
        return InventoryItem.ITEM_TYPE.NONE;
    }
  }

  public enum ITEM_CATEGORIES
  {
    NONE,
    LOGS,
    STONES,
    SEEDS,
    INGREDIENTS,
    MEALS,
    SOULS,
    POOP,
    COINS,
    GRASS,
  }

  public enum ITEM_TYPE
  {
    NONE,
    LOG,
    STONE,
    ROCK2,
    ROCK3,
    SEED_SWORD,
    MEAT,
    WHEAT,
    SEED,
    BONE,
    SOUL,
    VINES,
    RED_HEART,
    HALF_HEART,
    BLUE_HEART,
    HALF_BLUE_HEART,
    TIME_TOKEN,
    GENERIC,
    STAINED_GLASS,
    FLOWERS,
    BLACK_GOLD,
    BERRY,
    MONSTER_HEART,
    BLUE_PRINT,
    WEAPON_CARD,
    CURSE_CARD,
    TRINKET_CARD,
    SOUL_FRAGMENT,
    FISH,
    MUSHROOM_SMALL,
    BLACK_SOUL,
    MUSHROOM_BIG,
    MEAL,
    FISH_SMALL,
    FISH_BIG,
    GRASS,
    THORNS,
    KEY_PIECE,
    KEY,
    POOP,
    FOUND_ITEM_DECORATION,
    FOUND_ITEM_WEAPON,
    FOUND_ITEM_CURSE,
    GIFT_SMALL,
    GIFT_MEDIUM,
    Necklace_1,
    Necklace_2,
    Necklace_3,
    Necklace_4,
    Necklace_5,
    PUMPKIN,
    SEED_PUMPKIN,
    FOUND_ITEM_FOLLOWERSKIN,
    BLACK_HEART,
    PERMANENT_HALF_HEART,
    FLOWER_RED,
    FLOWER_WHITE,
    MEAL_GRASS,
    MEAL_MEAT,
    MEAL_GREAT,
    MEAL_GOOD_FISH,
    MEAT_ROTTEN,
    FOLLOWER_MEAT,
    FOLLOWER_MEAT_ROTTEN,
    MEAL_ROTTEN,
    MEAL_FOLLOWER_MEAT,
    SEEDS,
    MEALS,
    INGREDIENTS,
    MEAL_POOP,
    SEED_MUSHROOM,
    SEED_FLOWER_WHITE,
    SEED_FLOWER_RED,
    GRASS2,
    GRASS3,
    GRASS4,
    GRASS5,
    FLOWER_PURPLE,
    SEED_TREE,
    MAP,
    MEAL_MUSHROOMS,
    LOG_REFINED,
    STONE_REFINED,
    GOLD_NUGGET,
    ROPE,
    FOLLOWERS,
    GOLD_REFINED,
    BLOOD_STONE,
    TRINKET_CARD_UNLOCKED,
    CRYSTAL,
    SPIDER_WEB,
    FISH_CRAB,
    FISH_LOBSTER,
    FISH_OCTOPUS,
    FISH_SQUID,
    FISH_SWORDFISH,
    FISH_BLOWFISH,
    BEETROOT,
    SEED_BEETROOT,
    MEAL_GREAT_FISH,
    MEAL_BAD_FISH,
    BEHOLDER_EYE,
    CAULIFLOWER,
    SEED_CAULIFLOWER,
    DISCIPLE_POINTS,
    MEAT_MORSEL,
    MEAL_BERRIES,
    MEAL_MEDIUM_VEG,
    MEAL_BAD_MIXED,
    MEAL_MEDIUM_MIXED,
    MEAL_GREAT_MIXED,
    MEAL_DEADLY,
    MEAL_BAD_MEAT,
    MEAL_GREAT_MEAT,
    TALISMAN,
    MEAL_BURNED,
    DOCTRINE_STONE,
    SHELL,
    RELIC,
    GOD_TEAR,
    CRYSTAL_DOCTRINE_FRAGMENT,
    CRYSTAL_DOCTRINE_STONE,
    Necklace_Loyalty,
    Necklace_Demonic,
    Necklace_Dark,
    Necklace_Light,
    Necklace_Missionary,
    Necklace_Gold_Skull,
    GOD_TEAR_FRAGMENT,
    WEBBER_SKULL,
    SNOW_CHUNK,
    CHARCOAL,
    SILK_THREAD,
    COTTON,
    MEAL_SPICY,
    FISH_COD,
    FISH_PIKE,
    FISH_CATFISH,
    FOUND_ITEM_OUTFIT,
    LIGHTNING_SHARD,
    SEED_COTTON,
    LORE_STONE,
    POOP_GOLD,
    POOP_RAINBOW,
    POOP_GLOW,
    DRINK_BEER,
    DRINK_WINE,
    DRINK_COCKTAIL,
    DRINK_EGGNOG,
    DRINK_POOP_JUICE,
    HOPS,
    GRAPES,
    SEED_HOPS,
    SEED_GRAPES,
    PLEASURE_POINT,
    EGG_FOLLOWER,
    DRINK_GIN,
    UNUSED,
    DRINK_MUSHROOM_JUICE,
    YOLK,
    SEED_SOZO,
    MEAL_EGG,
    POOP_DEVOTION,
    Necklace_Bell,
    FOUND_ITEM_DECORATION_ALT,
    WOOL,
    SEED_SNOW_FRUIT,
    SNOW_FRUIT,
    CHILLI,
    SEED_CHILLI,
    ANIMAL_GOAT,
    MEAL_SNOW_FRUIT,
    MAGMA_STONE,
    ELECTRIFIED_MAGMA,
    FIRE_HEART,
    ICE_HEART,
    ANIMAL_TURTLE,
    ANIMAL_CRAB,
    ANIMAL_SPIDER,
    ANIMAL_SNAIL,
    Necklace_Deaths_Door,
    Necklace_Winter,
    Necklace_Frozen,
    Necklace_Weird,
    Necklace_Targeted,
    DLC_NECKLACE,
    SOOT,
    POOP_ROTSTONE,
    ANIMAL_COW,
    ANIMAL_LLAMA,
    DRINK_CHILLI,
    DRINK_LIGHTNING,
    DRINK_SIN,
    DRINK_GRASS,
    FORGE_FLAME,
    BROKEN_WEAPON_HAMMER,
    BROKEN_WEAPON_SWORD,
    MILK,
    MEAL_MILK_BAD,
    MEAL_MILK_GOOD,
    MEAL_MILK_GREAT,
    BROKEN_WEAPON_DAGGER,
    BROKEN_WEAPON_GAUNTLETS,
    BROKEN_WEAPON_AXE,
    BROKEN_WEAPON_BLUNDERBUSS,
    BROKEN_WEAPON_CHAIN,
    ILLEGIBLE_LETTER_SCYLLA,
    ILLEGIBLE_LETTER_CHARYBDIS,
    FISHING_ROD,
    YNGYA_GHOST,
    REPAIRED_WEAPON_HAMMER,
    REPAIRED_WEAPON_SWORD,
    REPAIRED_WEAPON_DAGGER,
    REPAIRED_WEAPON_GAUNTLETS,
    REPAIRED_WEAPON_BLUNDERBUSS,
    REPAIRED_WEAPON_CHAIN,
    SPECIAL_WOOL_RANCHER,
    SPECIAL_WOOL_LAMBWAR,
    SPECIAL_WOOL_BLACKSMITH,
    SPECIAL_WOOL_TAROT,
    SPECIAL_WOOL_DECORATION,
    SPECIAL_WOOL_6,
    SPECIAL_WOOL_7,
    SPECIAL_WOOL_8,
    SPECIAL_WOOL_9,
    SPECIAL_WOOL_10,
    ANIMALS,
    RATAU_STAFF,
    BOP,
    FLOCKADE_PIECE,
    YEW_CURSED,
    YEW_HOLY,
    BEHOLDER_EYE_ROT,
    LEGENDARY_WEAPON_FRAGMENT,
    SEED_FLOWER_PURPLE,
    SPECIAL_WOOL_11,
    DRINK_MILKSHAKE,
  }
}
