// Decompiled with JetBrains decompiler
// Type: InventoryItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using MMBiomeGeneration;
using MMRoomGeneration;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class InventoryItem
{
  public int type;
  public int quantity = 1;
  public int QuantityReserved;
  private static List<InventoryItem.ITEM_TYPE> ItemsThatCanBeGivenToFollower = new List<InventoryItem.ITEM_TYPE>()
  {
    InventoryItem.ITEM_TYPE.GIFT_MEDIUM,
    InventoryItem.ITEM_TYPE.GIFT_SMALL,
    InventoryItem.ITEM_TYPE.Necklace_1,
    InventoryItem.ITEM_TYPE.Necklace_2,
    InventoryItem.ITEM_TYPE.Necklace_3,
    InventoryItem.ITEM_TYPE.Necklace_4,
    InventoryItem.ITEM_TYPE.Necklace_5
  };

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
    if (!DataManager.Instance.EnabledSpells || PlayerFarming.Location == FollowerLocation.IntroDungeon)
      return (BlackSoul) null;
    int num = quantity;
    UnityEngine.Random.Range(0, 360);
    BlackSoul blackSoul = (BlackSoul) null;
    Transform transform = GameObject.FindGameObjectWithTag("Unit Layer").transform;
    while (--num >= 0)
    {
      if (BlackSoul.BlackSouls.Count >= 20 && SettingsManager.Settings.Graphics.EnvironmentDetail == 0)
      {
        ++PlayerFarming.LeftoverSouls;
      }
      else
      {
        if (BiomeGenerator.Instance?.CurrentRoom != null)
          transform = BiomeGenerator.Instance.CurrentRoom.GameObject.transform;
        blackSoul = BiomeConstants.Instance.SpawnBlackSouls(position, transform, 360f / (float) quantity * (float) num, simulated);
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
      case InventoryItem.ITEM_TYPE.PUMPKIN:
        return InventoryItem.ITEM_TYPE.SEED_PUMPKIN;
      case InventoryItem.ITEM_TYPE.FLOWER_RED:
        return InventoryItem.ITEM_TYPE.SEED_FLOWER_RED;
      case InventoryItem.ITEM_TYPE.FLOWER_WHITE:
        return InventoryItem.ITEM_TYPE.SEED_FLOWER_WHITE;
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
        InventoryItem.ITEM_TYPE.SEED_CAULIFLOWER
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
        InventoryItem.ITEM_TYPE.CAULIFLOWER
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

  public static PickUp Spawn(
    InventoryItem.ITEM_TYPE type,
    int quantity,
    Vector3 position,
    float StartSpeed = 4f,
    Action<PickUp> result = null)
  {
    if (type == InventoryItem.ITEM_TYPE.NONE)
      return (PickUp) null;
    if (PlayerFarming.Location == FollowerLocation.IntroDungeon && type != InventoryItem.ITEM_TYPE.BLACK_GOLD)
      return (PickUp) null;
    if (!DataManager.Instance.ShowLoyaltyBars && (type == InventoryItem.ITEM_TYPE.GIFT_SMALL || type == InventoryItem.ITEM_TYPE.GIFT_MEDIUM))
      type = InventoryItem.ITEM_TYPE.BLACK_GOLD;
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
        path = "Red Heart";
        break;
      case InventoryItem.ITEM_TYPE.HALF_HEART:
        path = "Half Heart";
        break;
      case InventoryItem.ITEM_TYPE.BLUE_HEART:
        path = "Blue Heart";
        break;
      case InventoryItem.ITEM_TYPE.HALF_BLUE_HEART:
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
        path = "BlackGold";
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
        path = "Black Heart";
        break;
      case InventoryItem.ITEM_TYPE.PERMANENT_HALF_HEART:
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
      case InventoryItem.ITEM_TYPE.WEBBER_SKULL:
        path = "Assets/Prefabs/Structures/Buildings/Webber Skull.prefab";
        flag = true;
        break;
      default:
        Debug.Log((object) ("failed to get: " + (object) type));
        break;
    }
    Transform transform = GameObject.FindGameObjectWithTag("Unit Layer")?.transform;
    PickUp p = (PickUp) null;
    while (--quantity >= 0)
    {
      if (BiomeGenerator.Instance?.CurrentRoom != null)
        transform = BiomeGenerator.Instance.CurrentRoom.GameObject.transform;
      if ((UnityEngine.Object) transform == (UnityEngine.Object) null && (UnityEngine.Object) GenerateRoom.Instance != (UnityEngine.Object) null)
        transform = GenerateRoom.Instance.transform;
      if (!((UnityEngine.Object) transform == (UnityEngine.Object) null))
      {
        if (flag)
        {
          ObjectPool.Spawn(path, position, Quaternion.identity, transform, (Action<GameObject>) (obj =>
          {
            p = obj.GetComponent<PickUp>();
            if ((UnityEngine.Object) p != (UnityEngine.Object) null)
              p.Speed = StartSpeed;
            Action<PickUp> action = result;
            if (action == null)
              return;
            action(p);
          }));
        }
        else
        {
          GameObject gameObject = (Resources.Load("Prefabs/Resources/" + path) as GameObject).Spawn(transform);
          gameObject.transform.position = position;
          gameObject.transform.eulerAngles = Vector3.zero;
          p = gameObject.GetComponent<PickUp>();
          if ((UnityEngine.Object) p != (UnityEngine.Object) null)
            p.Speed = StartSpeed;
        }
      }
      else
        break;
    }
    return p;
  }

  public static bool IsGift(InventoryItem.ITEM_TYPE type) => DataManager.AllGifts.Contains(type);

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
        return 1;
      case InventoryItem.ITEM_TYPE.GRASS:
        return 3;
      case InventoryItem.ITEM_TYPE.GOLD_REFINED:
        return 15;
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
        return true;
      default:
        return false;
    }
  }

  public static bool IsFood(InventoryItem.ITEM_TYPE Type)
  {
    if (InventoryItem.IsFish(Type))
      return true;
    if (Type <= InventoryItem.ITEM_TYPE.PUMPKIN)
    {
      if (Type != InventoryItem.ITEM_TYPE.MEAT && Type != InventoryItem.ITEM_TYPE.BERRY && Type != InventoryItem.ITEM_TYPE.PUMPKIN)
        goto label_6;
    }
    else if (Type != InventoryItem.ITEM_TYPE.BEETROOT && Type != InventoryItem.ITEM_TYPE.CAULIFLOWER && Type != InventoryItem.ITEM_TYPE.MEAT_MORSEL)
      goto label_6;
    return true;
label_6:
    return false;
  }

  public static bool IsBigFish(InventoryItem.ITEM_TYPE Type)
  {
    switch (Type)
    {
      case InventoryItem.ITEM_TYPE.FISH_BIG:
      case InventoryItem.ITEM_TYPE.FISH_LOBSTER:
      case InventoryItem.ITEM_TYPE.FISH_OCTOPUS:
      case InventoryItem.ITEM_TYPE.FISH_SWORDFISH:
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
        return new Action<Follower, InventoryItem.ITEM_TYPE, System.Action>(InventoryItem.OnGetNecklace);
      default:
        return (Action<Follower, InventoryItem.ITEM_TYPE, System.Action>) null;
    }
  }

  private static void OnGetNecklace(
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
    follower.TimedAnimation("Reactions/react-enlightened1", 2f, (System.Action) (() =>
    {
      System.Action action = Callback;
      if (action == null)
        return;
      action();
    }), false, false);
    follower.AddBodyAnimation("idle", true, 0.0f);
  }

  private static void OnGetGift(Follower follower, InventoryItem.ITEM_TYPE Item, System.Action Callback)
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

  private static void OnFeedFood(Follower follower, InventoryItem.ITEM_TYPE Item, System.Action Callback)
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

  private static void OnFeedBigMushroom(
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
    return str;
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
    WEBBER_SKULL,
  }
}
