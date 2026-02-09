// Decompiled with JetBrains decompiler
// Type: MultiInventory
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MultiInventory
{
  public List<Inventory> _inventories = new List<Inventory>();

  public List<Inventory> all => this._inventories;

  public MultiInventory(Inventory inventory1 = null, Inventory inventory2 = null, Inventory inventory3 = null)
  {
    if (inventory1 != null)
      this._inventories.Add(inventory1);
    if (inventory2 != null)
      this._inventories.Add(inventory2);
    if (inventory3 == null)
      return;
    this._inventories.Add(inventory3);
  }

  public MultiInventory(List<Inventory> inventories) => this._inventories = inventories;

  public void AddInventory(Inventory inventory, int specific_position_num = -1)
  {
    if (inventory == null)
      return;
    if (specific_position_num > -1 && specific_position_num < this._inventories.Count - 1)
      this._inventories.Insert(specific_position_num, inventory);
    else
      this._inventories.Add(inventory);
  }

  public void SetInventories(List<Inventory> inventories) => this._inventories = inventories;

  public bool AddItem(string id, int count)
  {
    if (this.IsEmpty())
      return false;
    Item obj = new Item(id, count);
    if (!this.CanAddItem(obj))
      return false;
    this.AddItem(obj);
    return true;
  }

  public bool AddItems(List<Item> items)
  {
    if (this.IsEmpty() || !this.CanAddItems(items))
      return false;
    foreach (Item obj in items)
      this.AddItemNoCheck(obj);
    return true;
  }

  public bool AddItems(List<Item> items, bool allow_bags)
  {
    if (this.IsEmpty() || !this.CanAddItems(items, allow_bags))
      return false;
    foreach (Item obj in items)
      this.AddItemNoCheck(obj, allow_bags);
    return true;
  }

  public bool AddItem(Item item)
  {
    if (this.IsEmpty() || !this.CanAddItem(item))
      return false;
    this._inventories[0].data.AddItem(item);
    return true;
  }

  public void AddItemNoCheck(Item item, bool allow_bag = true)
  {
    this._inventories[0].data.AddItem(item, allow_bag);
  }

  public bool RemoveItem(
    string item_id,
    int item_value,
    MultiInventory.DestinationType destination = MultiInventory.DestinationType.AllFromLast)
  {
    return this.RemoveItem(new Item(item_id, item_value), item_value, destination);
  }

  public bool RemoveItems(
    List<Item> items,
    MultiInventory.DestinationType destination = MultiInventory.DestinationType.AllFromLast,
    List<string> multiquality_ids = null,
    List<Item> out_really_removed_items = null)
  {
    if (this.IsEmpty() || !this.IsEnoughItems(items, destination, multiquality_ids))
      return false;
    if (multiquality_ids == null)
    {
      foreach (Item obj in items)
        this.RemoveItemNoCheck(obj, obj.value, destination, out_really_removed_items: out_really_removed_items);
    }
    else
    {
      for (int index = 0; index < items.Count; ++index)
      {
        Item obj = items[index];
        string multiqualityId = index < multiquality_ids.Count ? multiquality_ids[index] : "";
        if (string.IsNullOrEmpty(multiqualityId) && obj.multiquality_items.Count > 1 && obj.definition == null)
        {
          int num = obj.value;
          foreach (string multiqualityItem in obj.multiquality_items)
          {
            int item_value = this.GetTotalCount(multiqualityItem, destination);
            if (item_value > num)
              item_value = num;
            this.RemoveItem(multiqualityItem, item_value, destination);
            num -= item_value;
            out_really_removed_items?.Add(new Item(multiqualityItem, item_value));
            if (num <= 0)
              break;
          }
        }
        else
          this.RemoveItemNoCheck(obj, obj.value, destination, multiqualityId, out_really_removed_items);
      }
    }
    return true;
  }

  public bool RemoveItem(Item item, int count = 0, MultiInventory.DestinationType destination = MultiInventory.DestinationType.AllFromLast)
  {
    if (this.IsEmpty() || !this.IsEnoughItem(item, destination))
      return false;
    if (count == 0)
      count = item.value;
    this.RemoveItemNoCheck(item, count, destination);
    return true;
  }

  public bool TryRemoveSpecificItemNoCheck(Item item, MultiInventory.DestinationType destination = MultiInventory.DestinationType.AllFromLast)
  {
    foreach (Inventory inventory in this._inventories)
    {
      for (int index1 = 0; index1 < inventory.data.inventory.Count; ++index1)
      {
        if (item == inventory.data.inventory[index1])
        {
          inventory.data.inventory.RemoveAt(index1);
          return true;
        }
        if (inventory.data.inventory[index1].is_bag && item.CanBeInsertedInBag(inventory.data.inventory[index1]))
        {
          for (int index2 = 0; index2 < inventory.data.inventory[index1].inventory.Count; ++index2)
          {
            Item obj = inventory.data.inventory[index1].inventory[index2];
            if (item == obj)
            {
              inventory.data.inventory[index1].inventory.RemoveAt(index2);
              return true;
            }
          }
        }
      }
      if (destination == MultiInventory.DestinationType.OnlyFirst)
        break;
    }
    Debug.LogError((object) ("Couldn't find a specific item: " + item.id));
    return false;
  }

  public void RemoveItemNoCheck(
    Item item,
    int count = 0,
    MultiInventory.DestinationType destination = MultiInventory.DestinationType.AllFromLast,
    string multiquality_id = "",
    List<Item> out_really_removed_items = null)
  {
    if (destination == MultiInventory.DestinationType.OnlyFirst || this._inventories.Count == 1)
    {
      this._inventories[0].data.RemoveItemNoCheck(item, count, multiquality_id, out_really_removed_items);
    }
    else
    {
      int first;
      int last;
      int step;
      this.GetInteractionIndexes(destination, out first, out last, out step);
      for (int index = first; (step > 0 ? (index < last ? 1 : 0) : (index >= last ? 1 : 0)) != 0; index += step)
      {
        count = this._inventories[index].data.RemoveItemOrReturnLeftCount(item, count, multiquality_id, out_really_removed_items);
        if (count == 0)
          break;
      }
    }
  }

  public Item GetItem(string item_id, Item.ItemFindLogics item_find_logics = Item.ItemFindLogics.FirstFound, bool allow_bags = true)
  {
    List<Item> items = (List<Item>) null;
    if (item_find_logics != Item.ItemFindLogics.FirstFound)
      items = new List<Item>();
    foreach (Inventory inventory in this._inventories)
    {
      Item itemWithId = inventory.data.GetItemWithID(item_id, item_find_logics, allow_bags);
      if (itemWithId != null)
      {
        if (item_find_logics == Item.ItemFindLogics.FirstFound)
          return itemWithId;
        items.Add(itemWithId);
      }
    }
    return item_find_logics != Item.ItemFindLogics.FirstFound && items.Count != 0 ? Item.SortItemsListByFindLogics(items, item_find_logics)[0] : (Item) null;
  }

  public bool IsEnoughItem(
    Item item,
    MultiInventory.DestinationType destination = MultiInventory.DestinationType.AllFromFirst,
    string multiquality_id = "",
    int used_items = 0,
    int multiplier = 1)
  {
    if (item == null || item.IsEmpty())
      return true;
    if (this.IsEmpty())
      return false;
    if (destination == MultiInventory.DestinationType.OnlyFirst || this._inventories.Count == 1)
      return this._inventories[0].data.IsEnoughItems(item, multiquality_id, used_items, multiplier);
    int num1 = !string.IsNullOrEmpty(multiquality_id) ? 0 : (item.multiquality_items.Count > 1 ? 1 : 0);
    int num2 = 0;
    List<string> stringList;
    if (num1 == 0)
      stringList = new List<string>()
      {
        string.IsNullOrEmpty(multiquality_id) ? item.id : multiquality_id
      };
    else
      stringList = item.multiquality_items;
    foreach (string item_id in stringList)
      num2 += this.GetTotalCount(item_id, destination);
    return num2 - used_items >= item.value * multiplier;
  }

  public bool IsEnoughItems(
    List<Item> items,
    MultiInventory.DestinationType destination = MultiInventory.DestinationType.AllFromFirst,
    List<string> multiquality_ids = null,
    int multiplier = 1)
  {
    if (this.IsEmpty())
      return false;
    if (multiquality_ids == null)
    {
      foreach (Item obj in items)
      {
        if (!this.IsEnoughItem(obj, destination, multiplier: multiplier))
          return false;
      }
    }
    else
    {
      Dictionary<string, int> dictionary = new Dictionary<string, int>();
      for (int index = 0; index < items.Count; ++index)
      {
        Item obj = items[index];
        string multiqualityId = index < multiquality_ids.Count ? multiquality_ids[index] : "";
        int used_items = 0;
        if (!string.IsNullOrEmpty(multiqualityId))
          dictionary.TryGetValue(multiqualityId, out used_items);
        if (!this.IsEnoughItem(obj, destination, multiqualityId, used_items, multiplier))
          return false;
        if (!string.IsNullOrEmpty(multiqualityId))
        {
          if (used_items == 0)
            dictionary.Add(multiqualityId, obj.value);
          else
            dictionary[multiqualityId] += obj.value * multiplier;
        }
      }
    }
    return true;
  }

  public int GetTotalCount(
    string item_id,
    MultiInventory.DestinationType destination = MultiInventory.DestinationType.AllFromFirst,
    bool count_in_bags = true)
  {
    if (this.IsEmpty())
      return 0;
    if (destination == MultiInventory.DestinationType.OnlyFirst || this._inventories.Count == 1)
      return this._inventories[0].data.GetTotalCount(item_id, count_in_bags);
    int totalCount = 0;
    int first;
    int last;
    int step;
    this.GetInteractionIndexes(destination, out first, out last, out step);
    for (int index = first; (step > 0 ? (index < last ? 1 : 0) : (index >= last ? 1 : 0)) != 0; index += step)
    {
      if (this._inventories[index].data.is_bag)
        Debug.Log((object) $"#BAG# Found bag in multiinventory: {this._inventories[index].data.id}, first: {this._inventories[0].data.id}");
      else
        totalCount += this._inventories[index].data.GetTotalCount(item_id, count_in_bags);
    }
    return totalCount;
  }

  public int CanAddCount(string item_id, bool count_bags = false)
  {
    if (this.IsEmpty())
      return 0;
    return this._inventories[0].data.CanAddCount(item_id, true, count_bags);
  }

  public bool CanAddItem(Item item, bool allow_bags = true)
  {
    return !this.IsEmpty() && this._inventories[0].data.CanAddItem(item, count_bags: allow_bags);
  }

  public bool CanAddItems(List<Item> items, bool include_bags = false)
  {
    return !this.IsEmpty() && this._inventories[0].data.CanAddItems(items, include_bags);
  }

  public bool MoveItemTo(
    MultiInventory another_inventory,
    Item item,
    int count = 0,
    bool use_only_first_from_inventory = false,
    bool allow_bag = true)
  {
    count = this.CheckCountOfMovingItem(item, count, use_only_first_from_inventory ? MultiInventory.DestinationType.OnlyFirst : MultiInventory.DestinationType.AllFromFirst);
    Item obj = item;
    item = new Item(item)
    {
      value = count,
      equipped_as = ItemDefinition.EquipmentType.None
    };
    if (!this.IsEnoughItem(item) || !another_inventory.CanAddItem(item, allow_bag))
      return false;
    if (count == 1 && obj.definition.stack_count == 1)
    {
      if (!this.TryRemoveSpecificItemNoCheck(obj))
        this.RemoveItemNoCheck(item);
    }
    else
      this.RemoveItemNoCheck(item, destination: use_only_first_from_inventory ? MultiInventory.DestinationType.OnlyFirst : MultiInventory.DestinationType.AllFromLast);
    if (another_inventory.all[0].IsTavernPalette() && item.id.Contains("cup_beer"))
      MainGame.me.save.quests.CheckKeyQuests("put_beer_into_taverns_rack");
    another_inventory.AddItemNoCheck(item, allow_bag);
    return true;
  }

  public bool MoveItemTo(Item another_inventory, Item item, int count = 0)
  {
    count = this.CheckCountOfMovingItem(item, count);
    Item obj = item;
    item = new Item(item)
    {
      value = count,
      equipped_as = ItemDefinition.EquipmentType.None
    };
    if (!this.IsEnoughItem(item) || !another_inventory.CanAddItem(item))
      return false;
    if (count == 1 && obj.definition.stack_count == 1)
    {
      if (!this.TryRemoveSpecificItemNoCheck(obj))
        this.RemoveItemNoCheck(item);
    }
    else
      this.RemoveItemNoCheck(item);
    another_inventory.AddNotFoldedItem(item);
    return true;
  }

  public int CheckCountOfMovingItem(Item item, int count, MultiInventory.DestinationType dest = MultiInventory.DestinationType.AllFromFirst)
  {
    if (count == 0)
      return item.value;
    int totalCount = this.GetTotalCount(item.id, dest);
    return count > totalCount ? totalCount : count;
  }

  public bool IsEmpty(bool print_log = true)
  {
    int num = this._inventories.Count == 0 ? 1 : 0;
    if ((num & (print_log ? 1 : 0)) == 0)
      return num != 0;
    Debug.LogError((object) "empty MultiInventories");
    return num != 0;
  }

  public void GetInteractionIndexes(
    MultiInventory.DestinationType destination,
    out int first,
    out int last,
    out int step)
  {
    int count = this._inventories.Count;
    first = destination != MultiInventory.DestinationType.AllFromLast ? 0 : count - 1;
    step = destination != MultiInventory.DestinationType.AllFromLast ? 1 : -1;
    if (destination != MultiInventory.DestinationType.AllFromFirst)
    {
      if (destination == MultiInventory.DestinationType.AllFromLast)
        last = 0;
      else
        last = count > 1 ? 1 : count;
    }
    else
      last = count;
  }

  public float money
  {
    get
    {
      float money = 0.0f;
      foreach (Inventory inventory in this._inventories)
        money += inventory.data.money;
      return money;
    }
  }

  public enum DestinationType
  {
    OnlyFirst,
    AllFromFirst,
    AllFromLast,
  }

  public enum PlayerMultiInventory
  {
    DontChange,
    IncludePlayer,
    ExcludePlayer,
  }
}
