// Decompiled with JetBrains decompiler
// Type: Inventory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI;
using MMBiomeGeneration;
using src.Extensions;
using System.Collections.Generic;
using Unify;
using UnityEngine;

#nullable disable
public class Inventory : BaseMonoBehaviour
{
  public static Inventory.InventoryUpdated OnInventoryUpdated;
  public static List<InventoryWeapon> weapons = new List<InventoryWeapon>()
  {
    new InventoryWeapon(InventoryWeapon.ITEM_TYPE.SWORD, 100),
    new InventoryWeapon(InventoryWeapon.ITEM_TYPE.SEED_BAG, 100)
  };
  public static Inventory.GetFollowerToken OnGetFollowerToken;

  public static int KeyPieces
  {
    get => DataManager.Instance.CurrentKeyPieces;
    set
    {
      DataManager.Instance.CurrentKeyPieces = value;
      if (DataManager.Instance.CurrentKeyPieces < 4)
        return;
      DataManager.Instance.CurrentKeyPieces = 0;
      Inventory.AddItem(InventoryItem.ITEM_TYPE.TALISMAN, 1);
    }
  }

  public static int TempleKeys => Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.TALISMAN);

  public static List<InventoryItem> itemsDungeon
  {
    get => DataManager.Instance.itemsDungeon;
    set => DataManager.Instance.itemsDungeon = value;
  }

  public static List<InventoryItem> items
  {
    get => DataManager.Instance.items;
    set => DataManager.Instance.items = value;
  }

  public static int CURRENT_WEAPON
  {
    get => DataManager.Instance.CURRENT_WEAPON;
    set => DataManager.Instance.CURRENT_WEAPON = value;
  }

  public static int Souls
  {
    get => DataManager.Instance.Souls;
    set => DataManager.Instance.Souls = value;
  }

  public static int BlackSouls
  {
    get => DataManager.Instance.BlackSouls;
    set => DataManager.Instance.BlackSouls = value;
  }

  public static int FollowerTokens
  {
    get => DataManager.Instance.FollowerTokens;
    set
    {
      DataManager.Instance.FollowerTokens = value;
      Inventory.GetFollowerToken getFollowerToken = Inventory.OnGetFollowerToken;
      if (getFollowerToken == null)
        return;
      getFollowerToken();
    }
  }

  public static List<InventoryItem> Food => DataManager.Instance.Food;

  public static int TotalItems()
  {
    int num = 0;
    foreach (InventoryItem inventoryItem in Inventory.items)
      num += inventoryItem.quantity;
    return num;
  }

  public static event Inventory.ItemAddedToInventory OnItemAddedToInventory;

  public static event Inventory.ItemAddedToInventory OnItemRemovedFromInventory;

  public static event Inventory.ItemAddedToDungeonInventory OnItemAddedToDungeonInventory;

  public static void AddItemDungeon(int type, int quantity)
  {
    if (quantity <= 0)
      return;
    InventoryItem dungeonItemByType = Inventory.GetDungeonItemByType(type);
    if (dungeonItemByType != null)
    {
      dungeonItemByType.quantity += quantity;
    }
    else
    {
      InventoryItem inventoryItem = new InventoryItem();
      inventoryItem.Init(type, quantity);
      Inventory.itemsDungeon.Add(inventoryItem);
    }
    if (Inventory.OnItemAddedToDungeonInventory != null)
      Inventory.OnItemAddedToDungeonInventory((InventoryItem.ITEM_TYPE) type);
    if (DataManager.Instance.ShownInventoryTutorial || UIInventoryPromptOverlay.Showing)
      return;
    MonoSingleton<UIManager>.Instance.InventoryPromptTemplate.Instantiate<UIInventoryPromptOverlay>();
  }

  public static void ClearDungeonItems(bool includeBlacklist = true)
  {
    List<InventoryItem.ITEM_TYPE> itemTypeList = new List<InventoryItem.ITEM_TYPE>()
    {
      InventoryItem.ITEM_TYPE.BEHOLDER_EYE,
      InventoryItem.ITEM_TYPE.MONSTER_HEART
    };
    if (includeBlacklist)
    {
      Inventory.itemsDungeon.Clear();
    }
    else
    {
      for (int index = Inventory.itemsDungeon.Count - 1; index >= 0; --index)
      {
        if (!itemTypeList.Contains((InventoryItem.ITEM_TYPE) Inventory.itemsDungeon[index].type))
          Inventory.itemsDungeon.RemoveAt(index);
      }
    }
  }

  public static void AddItem(InventoryItem.ITEM_TYPE type, int quantity, bool forceNormalInventory = false)
  {
    Inventory.AddItem((int) type, quantity, forceNormalInventory);
  }

  public static void AddItem(int type, int quantity, bool ForceNormalInventory = false)
  {
    if (quantity <= 0)
      return;
    if ((Object) BiomeGenerator.Instance != (Object) null && !ForceNormalInventory)
      Inventory.AddItemDungeon(type, quantity);
    InventoryItem itemByType = Inventory.GetItemByType(type);
    if (itemByType != null)
    {
      itemByType.quantity += quantity;
    }
    else
    {
      InventoryItem inventoryItem = new InventoryItem();
      inventoryItem.Init(type, quantity);
      Inventory.items.Add(inventoryItem);
    }
    if (Inventory.OnItemAddedToInventory != null)
      Inventory.OnItemAddedToInventory((InventoryItem.ITEM_TYPE) type, quantity);
    if (Inventory.OnInventoryUpdated != null)
      Inventory.OnInventoryUpdated();
    if (type != 20 || Inventory.GetItemQuantity(20) < 666)
      return;
    AchievementsWrapper.UnlockAchievement(Achievements.Instance.Lookup("666_GOLD"));
  }

  public static void ChangeItemQuantity(InventoryItem.ITEM_TYPE type, int quantity, int reserved = 0)
  {
    Inventory.ChangeItemQuantity((int) type, quantity, reserved);
  }

  public static void ChangeItemQuantity(int type, int quantity, int reserved = 0)
  {
    if (quantity > 0)
    {
      Inventory.AddItem(type, quantity);
    }
    else
    {
      switch (type)
      {
        case 10:
          if ((Object) PlayerFarming.Instance != (Object) null)
          {
            PlayerFarming.Instance.GetSoul(quantity);
            break;
          }
          Inventory.Souls += quantity;
          break;
        case 30:
          if ((Object) PlayerFarming.Instance != (Object) null)
          {
            PlayerFarming.Instance.GetBlackSoul(quantity, false);
            break;
          }
          Inventory.BlackSouls += quantity;
          break;
        case 85:
          break;
        case 116:
          if ((Object) PlayerDoctrineStone.Instance != (Object) null)
          {
            PlayerDoctrineStone.Instance.CompletedDoctrineStones += quantity;
            break;
          }
          DataManager.Instance.CompletedDoctrineStones += quantity;
          break;
        default:
          InventoryItem itemByType = Inventory.GetItemByType(type);
          if (itemByType != null)
          {
            itemByType.quantity += quantity;
            itemByType.QuantityReserved += reserved;
            Inventory.CheckQuantities();
          }
          Inventory.ItemAddedToInventory removedFromInventory = Inventory.OnItemRemovedFromInventory;
          if (removedFromInventory != null)
            removedFromInventory((InventoryItem.ITEM_TYPE) type, quantity);
          if (Inventory.OnInventoryUpdated == null)
            break;
          Inventory.OnInventoryUpdated();
          break;
      }
    }
  }

  public static void RemoveAll() => Inventory.items = new List<InventoryItem>();

  public static void SetItemQuantity(int type, int quantity)
  {
    InventoryItem itemByType = Inventory.GetItemByType(type);
    if (itemByType == null)
      return;
    itemByType.quantity = quantity;
    Inventory.CheckQuantities();
  }

  private static void CheckQuantities()
  {
    for (int index = 0; index < Inventory.items.Count; ++index)
    {
      InventoryItem inventoryItem = Inventory.items[index];
      if (inventoryItem.quantity <= 0)
      {
        Debug.Log((object) "remove inventory items");
        Inventory.items.Remove(inventoryItem);
      }
    }
  }

  public static bool CheckCapacityFull(InventoryItem.ITEM_TYPE type)
  {
    switch (type)
    {
      case InventoryItem.ITEM_TYPE.LOG:
        return Inventory.GetItemQuantity(1) < DataManager.LogCapacity[DataManager.Instance.LogCapacityLevel];
      case InventoryItem.ITEM_TYPE.STONE:
        return Inventory.GetItemQuantity(2) < DataManager.StoneCapacity[DataManager.Instance.StoneCapacityLevel];
      case InventoryItem.ITEM_TYPE.MEALS:
        return Inventory.GetItemQuantity(67) < DataManager.FoodCapacity[DataManager.Instance.FoodCapacityLevel];
      case InventoryItem.ITEM_TYPE.INGREDIENTS:
        return Inventory.GetItemQuantity(68) < DataManager.IngredientsCapacity[DataManager.Instance.IngredientsCapacityLevel];
      default:
        return false;
    }
  }

  public static float GetResourceCapacity(InventoryItem.ITEM_TYPE type)
  {
    switch (type)
    {
      case InventoryItem.ITEM_TYPE.LOG:
        return (float) Inventory.GetItemQuantity(1) / (float) DataManager.LogCapacity[DataManager.Instance.LogCapacityLevel];
      case InventoryItem.ITEM_TYPE.STONE:
        return (float) Inventory.GetItemQuantity(2) / (float) DataManager.StoneCapacity[DataManager.Instance.StoneCapacityLevel];
      case InventoryItem.ITEM_TYPE.MEALS:
        return (float) Inventory.GetItemQuantity(67) / (float) DataManager.FoodCapacity[DataManager.Instance.FoodCapacityLevel];
      case InventoryItem.ITEM_TYPE.INGREDIENTS:
        return (float) Inventory.GetItemQuantity(68) / (float) DataManager.IngredientsCapacity[DataManager.Instance.IngredientsCapacityLevel];
      default:
        return -1f;
    }
  }

  public static int GetItemQuantity(InventoryItem.ITEM_TYPE itemType)
  {
    return Inventory.GetItemQuantity((int) itemType);
  }

  public static int GetItemQuantities(List<InventoryItem.ITEM_TYPE> items)
  {
    return Inventory.GetItemQuantities(items.ToArray());
  }

  public static int GetItemQuantities(params InventoryItem.ITEM_TYPE[] items)
  {
    int itemQuantities = 0;
    foreach (InventoryItem.ITEM_TYPE itemType in items)
      itemQuantities += Inventory.GetItemQuantity(itemType);
    return itemQuantities;
  }

  public static int GetItemQuantity(int type)
  {
    switch (type)
    {
      case 10:
        return Inventory.Souls;
      case 30:
        return Inventory.BlackSouls;
      case 85:
        return DataManager.Instance.Followers.Count;
      case 104:
        return UpgradeSystem.DisciplePoints;
      case 116:
        return DataManager.Instance.CompletedDoctrineStones;
      default:
        foreach (InventoryItem inventoryItem in Inventory.items)
        {
          if (inventoryItem.type == type)
            return inventoryItem.quantity;
        }
        return 0;
    }
  }

  public static bool HasGift()
  {
    return Inventory.GetItemQuantity(45) > 0 || Inventory.GetItemQuantity(46) > 0 || Inventory.GetItemQuantity(47) > 0 || Inventory.GetItemQuantity(48 /*0x30*/) > 0 || Inventory.GetItemQuantity(49) > 0 || Inventory.GetItemQuantity(43) > 0 || Inventory.GetItemQuantity(44) > 0;
  }

  public static InventoryItem GetItemByType(InventoryItem.ITEM_TYPE itemType)
  {
    return Inventory.GetItemByType((int) itemType);
  }

  public static InventoryItem GetItemByType(int type)
  {
    foreach (InventoryItem itemByType in Inventory.items)
    {
      if (itemByType.type == type)
        return itemByType;
    }
    return (InventoryItem) null;
  }

  public static InventoryItem GetDungeonItemByType(int type)
  {
    foreach (InventoryItem dungeonItemByType in Inventory.itemsDungeon)
    {
      if (dungeonItemByType.type == type)
        return dungeonItemByType;
    }
    return (InventoryItem) null;
  }

  public static InventoryItem GetDungeonItemByTypeReturnObject(int type)
  {
    foreach (InventoryItem typeReturnObject in Inventory.itemsDungeon)
    {
      if (typeReturnObject.type == type)
        return typeReturnObject;
    }
    return new InventoryItem() { type = type, quantity = 0 };
  }

  public static InventoryItem[] GetItemsByCategory(InventoryItem.ITEM_CATEGORIES category)
  {
    List<InventoryItem> inventoryItemList = new List<InventoryItem>();
    foreach (InventoryItem inventoryItem in Inventory.items)
    {
      if (InventoryItem.GetItemCategory((InventoryItem.ITEM_TYPE) inventoryItem.type) == category)
        inventoryItemList.Add(inventoryItem);
    }
    return inventoryItemList.ToArray();
  }

  public static int GetFoodAmount()
  {
    int foodAmount = 0;
    foreach (InventoryItem inventoryItem in Inventory.items)
    {
      CookingData.IngredientType ingredientType = CookingData.GetIngredientType((InventoryItem.ITEM_TYPE) inventoryItem.type);
      if (CookingData.GetIngredientCategory(ingredientType) != CookingData.IngredientType.NONE && CookingData.GetIngredientCategory(ingredientType) != CookingData.IngredientType.SPECIAL)
        foodAmount += inventoryItem.quantity;
    }
    return foodAmount;
  }

  public delegate void InventoryUpdated();

  public delegate void GetFollowerToken();

  public delegate void ItemAddedToInventory(InventoryItem.ITEM_TYPE ItemType, int Delta);

  public delegate void ItemAddedToDungeonInventory(InventoryItem.ITEM_TYPE ItemType);
}
