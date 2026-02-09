// Decompiled with JetBrains decompiler
// Type: Item
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using LinqTools;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

#nullable disable
[Serializable]
public class Item : ISerializationCallbackReceiver
{
  public const bool ALLOW_DEFINITION_CACHE = true;
  public const string TAKEN_FROM_PLAYER_INV = "taken_from_player_inventory";
  public const string INVENTORY_SIZE = "inventory_size";
  public const string RAT_SPEED = "rat_speed";
  public const string RAT_OBEDIENCE = "rat_obedience";
  public const int TOOLBELT_INVENTORY_SIZE = 7;
  public const string WHITE_SKULL_MOD_RES_NAME = "bp_plus_mod";
  public const string RED_SKULL_MOD_RES_NAME = "bp_minus_mod";
  public string id;
  public int value;
  public SmartExpression min_value;
  public SmartExpression max_value;
  public int linked_id = -1;
  public SmartExpression self_chance = new SmartExpression();
  public SmartExpression common_chance = new SmartExpression();
  public int chance_group = -1;
  public bool is_unique;
  public List<string> multiquality_items = new List<string>();
  public ItemDefinition.EquipmentType equipped_as;
  public string drop_zone_id = string.Empty;
  public long worker_unique_id = -1;
  [SerializeField]
  public GameRes _params = new GameRes();
  [SmartSerialize]
  [NonSerialized]
  public List<Item> inventory = new List<Item>();
  [SmartSerialize]
  [NonSerialized]
  public List<Item> secondary_inventory = new List<Item>();
  [SerializeField]
  [HideInInspector]
  public int _serialize_depth;
  [SmartDontSerialize]
  [HideInInspector]
  [SerializeField]
  public string _serialized_data = "";
  public string _cached_drop_zone_id = string.Empty;
  public WorldZoneDefinition _cached_world_zone;
  public float _cached_zone_dur_k = 1f;
  public string sub_name = "";
  [NonSerialized]
  public BalanceBaseObject _definition;
  [NonSerialized]
  public bool _definition_set;
  [NonSerialized]
  public string _definition_set_id = string.Empty;

  public Worker worker
  {
    get
    {
      return this.worker_unique_id > 0L ? MainGame.me.save.workers.GetWorker(this.worker_unique_id) : (Worker) null;
    }
  }

  public bool is_worker => this.worker_unique_id > 0L;

  public bool is_bag => this.definition != null && this.definition.is_bag;

  public int inventory_size
  {
    get => this._params.GetInt(nameof (inventory_size));
    set => this._params.Set(nameof (inventory_size), (float) value);
  }

  public float durability
  {
    get => this._params.durability;
    set => this._params.durability = Mathf.Clamp01(value);
  }

  public Item.DurabilityState durability_state
  {
    get
    {
      float durability = this.durability;
      if ((double) durability >= 0.99998998641967773)
        return Item.DurabilityState.Full;
      if ((double) durability <= 9.9999997473787516E-05)
        return Item.DurabilityState.Broken;
      return (double) durability < 0.20000000298023224 ? Item.DurabilityState.PreBroken : Item.DurabilityState.Used;
    }
  }

  public static Item empty => new Item();

  public bool is_equipped => this.equipped_as != 0;

  public bool is_equipped_to_toolbar => this.toolbar_index != -1;

  public int toolbar_index => MainGame.me.save.GetEquippedIndex(this.id);

  public bool is_multiquality => this.multiquality_items.Count > 0;

  public ItemDefinition definition
  {
    get
    {
      if (!this._definition_set || this._definition_set_id != this.id)
      {
        this._definition = string.IsNullOrEmpty(this.id) || this.id == "empty" || this.id == "_empty_" ? (BalanceBaseObject) null : (BalanceBaseObject) GameBalance.me.GetDataOrNull<ItemDefinition>(this.id);
        this._definition_set = true;
        this._definition_set_id = this.id;
      }
      return (ItemDefinition) this._definition;
    }
  }

  public Item()
    : this(nameof (empty), -1)
  {
  }

  public Item(string item_id)
    : this(item_id, 1)
  {
  }

  public Item(string item_id, int item_value)
  {
    this.id = item_id;
    this.value = item_value;
    this.is_unique = false;
    this.durability = 1f;
    this._definition_set = false;
    this.CheckAndInitBagItem();
  }

  public Item(Item item)
  {
    this._definition_set = false;
    if (item == null)
    {
      this.id = nameof (empty);
      this.value = -1;
    }
    else
    {
      this.id = item.id;
      this.value = item.value;
      this.is_unique = item.is_unique;
      this.linked_id = item.linked_id;
      this.max_value = item.max_value;
      this.min_value = item.min_value;
      this._params += item._params;
      this.sub_name = item.sub_name;
      this.durability = item.durability;
      this.equipped_as = item.equipped_as;
      this.CheckAndInitBagItem();
      this.worker_unique_id = item.worker_unique_id;
      foreach (Item obj in item.inventory)
        this.inventory.Add(new Item(obj));
    }
  }

  public void SetItemID(string new_id)
  {
    this.id = new_id;
    this._definition_set = false;
    this.CheckAndInitBagItem();
  }

  public void Sort() => this.inventory.Sort(new Comparison<Item>(this.CompareItems));

  public bool CheckAndInitBagItem()
  {
    if (string.IsNullOrEmpty(this.id) || this.id == "empty" || this.id == "_empty_" || this.definition == null || this.definition.type != ItemDefinition.ItemType.Bag)
      return false;
    this.inventory_size = this.definition.bag_size_x * this.definition.bag_size_y;
    return true;
  }

  public int CompareItems(Item left, Item right)
  {
    if (left == null && right == null)
      return 0;
    if (left == null)
      return 1;
    if (right == null)
      return -1;
    if (left.definition == null && right.definition == null)
      return 0;
    if (left.definition == null)
      return 1;
    if (right.definition == null)
      return -1;
    if (left.definition.type > right.definition.type)
      return 1;
    if (left.definition.type < right.definition.type || left.definition.product_weight > right.definition.product_weight)
      return -1;
    if (left.definition.product_weight < right.definition.product_weight || left.definition.product_tier < right.definition.product_tier)
      return 1;
    if (left.definition.product_tier > right.definition.product_tier || (double) left.definition.base_price > (double) right.definition.base_price)
      return -1;
    if ((double) left.definition.base_price < (double) right.definition.base_price)
      return 1;
    int num = string.Compare(left.definition.id, right.definition.id, StringComparison.Ordinal);
    if (num != 0)
      return num;
    if (left.value < right.value)
      return 1;
    if (left.value > right.value)
      return -1;
    if ((double) left.durability < (double) right.durability)
      return 1;
    return (double) left.durability > (double) right.durability ? -1 : 0;
  }

  public bool IsEmpty() => this.value <= 0 || string.IsNullOrEmpty(this.id) || this.id == "empty";

  public bool IsNotEmpty() => !this.IsEmpty();

  public bool ParsIsEmpty() => this._params.IsEmpty();

  public string GetParamAsString(string param_name)
  {
    return ((int) this._params.Get(param_name)).ToString();
  }

  public float GetParam(string param_name, float default_value = 0.0f)
  {
    return this._params.Get(param_name, default_value);
  }

  public int GetParamInt(string param_name) => this._params.GetInt(param_name);

  public bool HasParam(string param_name) => this._params.Has(param_name);

  public float GetCalculatedParam(string param_name)
  {
    float num = 0.0f;
    ItemDefinition definition = this.definition;
    if (definition != null)
      num += definition.parameters.Get(param_name);
    return num + this._params.Get(param_name);
  }

  public void SetParam(string param_name, float value) => this._params.Set(param_name, value);

  public void SetParam(GameResAtom game_res) => this._params.Set(game_res.type, game_res.value);

  public void SetParam(GameRes game_res)
  {
    foreach (GameResAtom atom in game_res.ToAtomList())
      this.SetParam(atom);
  }

  public void AddToParams(string param_name, float value) => this._params.Add(param_name, value);

  public void AddToParams(GameResAtom game_res) => this._params.Add(game_res.type, game_res.value);

  public void SubFromParams(GameResAtom game_res)
  {
    this._params.Sub(game_res.type, game_res.value);
  }

  public void AddToParams(GameRes game_res)
  {
    foreach (GameResAtom atom in game_res.ToAtomList())
      this.AddToParams(atom);
  }

  public void SubFromParams(GameRes game_res)
  {
    foreach (GameResAtom atom in game_res.ToAtomList())
      this.SubFromParams(atom);
  }

  public void SubFromParams(string param_name, float param_value)
  {
    this._params.Sub(param_name, param_value);
  }

  public void RemoveZeroParams() => this._params.RemoveZeroValues();

  public bool SetInventorySize(int size)
  {
    this._params.Set("inventory_size", (float) size);
    return true;
  }

  public List<Item> GetInventoryCopy()
  {
    return this.inventory.Select<Item, Item>((Func<Item, Item>) (item => new Item(item))).ToList<Item>();
  }

  public bool AddNotFoldedItem(Item item) => this.AddItem(item.id, item.value);

  public bool AddItem(string item_id, int item_value)
  {
    return this.AddItem(new Item(item_id, item_value));
  }

  public bool AddItemByIndex(Item item, int index)
  {
    if (index < 0 || index >= this.inventory_size)
    {
      Debug.LogError((object) $"Can't add item [{item.id}:{item.value}], index [{index}] out of range");
      return false;
    }
    if (this.inventory.Count < index + 1)
      this.InitializeInventoryTillIndex(index);
    Item obj = this.inventory[index];
    if (obj != null && !obj.IsEmpty())
    {
      if (obj.id != item.id)
      {
        Debug.LogError((object) $"Can't add item [{item.id}:{item.value}], at index [{index}], there's another item inserted [{obj.id}:{obj.value}]");
        return false;
      }
      if (obj.value + item.value <= obj.definition.stack_count)
      {
        obj.value += item.value;
        return false;
      }
      Debug.LogError((object) $"Can't add item [{item.id}:{item.value}], at index [{index}], not enough space");
      return false;
    }
    if (item.value <= item.definition.stack_count)
    {
      this.inventory[index] = item;
      return true;
    }
    Debug.LogError((object) $"Can't add item [{item.id}:{item.value}], at index [{index}], not enough space");
    return false;
  }

  public bool RemoveItemByIndex(int index)
  {
    if (index < 0 || index >= this.inventory_size)
    {
      Debug.LogError((object) $"Can't remove item, index [{index}] out of range");
      return false;
    }
    if (this.inventory.Count < index + 1)
      this.InitializeInventoryTillIndex(index);
    this.inventory[index] = new Item();
    return true;
  }

  public Item GetItemByIndex(int index)
  {
    if (index < 0 || index >= this.inventory_size)
    {
      Debug.LogError((object) $"Can't return item, index [{index}] out of range");
      return (Item) null;
    }
    if (this.inventory.Count < index + 1)
      this.InitializeInventoryTillIndex(index);
    return this.inventory[index];
  }

  public bool AddItems(List<Item> items, bool return_false_if_cannot_add_all)
  {
    if (return_false_if_cannot_add_all && !this.CanAddItems(items))
      return false;
    foreach (Item obj in items)
      this.AddItem(obj);
    return true;
  }

  public void AddNotFoldedItemsWithoutCheck(List<Item> items)
  {
    foreach (Item obj in items)
      this.AddItem(obj.id, obj.value);
  }

  public void RemoveNotFoldedItem(Item i) => this.RemoveItem(i.id, i.value);

  public bool RemoveItem(string item_id, int item_value, Item try_from_bag = null)
  {
    return this.RemoveItem(new Item(item_id, item_value), try_from_bag: try_from_bag);
  }

  public bool RemoveItems(List<Item> items, int multiplier = 1)
  {
    bool flag = true;
    foreach (Item obj in items)
      flag &= this.RemoveItem(obj, obj.value * multiplier);
    return flag;
  }

  public bool AddItem(Item item, bool bags_allowed = true)
  {
    if (!this.CanAddItem(item))
    {
      Debug.Log((object) $"Can not add item [{item.id}:{item.value.ToString()}]");
      return false;
    }
    string id = item.id;
    int num1 = item.value;
    int stackCount = item.definition.stack_count;
    int num2 = this.inventory_size - this.inventory.Count;
    if (num2 < 0)
      num2 = 0;
    List<Item> objList1 = new List<Item>();
    List<Item> objList2 = new List<Item>();
    bool flag1 = false;
    if (bags_allowed)
    {
      for (int index = 0; index < this.inventory.Count; ++index)
      {
        Item obj1 = this.inventory[index];
        if (obj1 == null || obj1.IsEmpty())
        {
          this.inventory.RemoveAt(index);
          --index;
        }
        else
        {
          if (obj1.id == id)
            flag1 = true;
          if (obj1.is_bag && item.definition.can_be_inserted_in_bag.Contains(obj1.definition.bag_type))
          {
            bool flag2 = false;
            foreach (Item obj2 in obj1.inventory)
            {
              if (obj2.id == id)
              {
                flag2 = true;
                break;
              }
            }
            if (flag2)
              objList1.Add(obj1);
            else
              objList2.Add(obj1);
          }
        }
      }
    }
    else
    {
      for (int index = 0; index < this.inventory.Count; ++index)
      {
        Item obj = this.inventory[index];
        if (obj == null || obj.IsEmpty())
        {
          this.inventory.RemoveAt(index);
          --index;
        }
        else if (obj.id == id)
          flag1 = true;
      }
    }
    if (stackCount == 1)
    {
      bool flag3 = false;
      while (num1 > 0)
      {
        --num1;
        if (flag1 && num2 > 0)
        {
          --num2;
          this.inventory.Add(new Item(item) { value = 1 });
        }
        else
        {
          if (bags_allowed && objList1.Count > 0)
          {
            int num3;
            for (int index = 0; index < objList1.Count; index = num3 + 1)
            {
              Item obj3 = objList1[index];
              if (obj3.inventory_size - obj3.inventory.Count > 0)
              {
                Item obj4 = new Item(item) { value = 1 };
                obj3.inventory.Add(obj4);
                flag3 = true;
                break;
              }
              objList1.RemoveAt(index);
              num3 = index - 1;
              if (num3 >= objList1.Count)
                break;
            }
            if (flag3)
              continue;
          }
          if (num2 > 0)
          {
            --num2;
            this.inventory.Add(new Item(item) { value = 1 });
          }
          else
          {
            if (!bags_allowed || objList2.Count <= 0)
              return false;
            int num4;
            for (int index = 0; index < objList2.Count; index = num4 + 1)
            {
              Item obj5 = objList2[index];
              if (obj5.inventory_size - obj5.inventory.Count > 0)
              {
                Item obj6 = new Item(item) { value = 1 };
                obj5.inventory.Add(obj6);
                break;
              }
              objList2.RemoveAt(index);
              num4 = index - 1;
              if (num4 >= objList2.Count)
                break;
            }
          }
        }
      }
      return true;
    }
    if (flag1)
    {
      if (num1 > 0)
      {
        foreach (Item obj in this.inventory)
        {
          if (!(obj.id != id) && obj.value != stackCount)
          {
            int num5 = stackCount - obj.value;
            if (num5 >= num1)
            {
              obj.value += num1;
              num1 = 0;
            }
            else
            {
              num1 -= num5;
              obj.value = stackCount;
            }
            if (num1 <= 0)
              return true;
          }
        }
      }
      while (num1 > 0 && this.inventory.Count < this.inventory_size)
      {
        if (num1 > stackCount)
        {
          this.inventory.Add(new Item(item)
          {
            value = stackCount
          });
          num1 -= stackCount;
        }
        else
        {
          this.inventory.Add(new Item(item) { value = num1 });
          num1 = 0;
        }
      }
    }
    if (bags_allowed && objList1.Count > 0)
    {
      for (int index = 0; index < objList1.Count; ++index)
      {
        Item obj7 = objList1[index];
        foreach (Item obj8 in obj7.inventory)
        {
          if (!(obj8.id != id) && obj8.value != stackCount)
          {
            int num6 = stackCount - obj8.value;
            if (num6 >= num1)
            {
              obj8.value += num1;
              num1 = 0;
            }
            else
            {
              num1 -= num6;
              obj8.value = stackCount;
            }
            if (num1 <= 0)
              return true;
          }
        }
        while (num1 > 0 && obj7.inventory.Count < obj7.inventory_size)
        {
          if (num1 > stackCount)
          {
            obj7.inventory.Add(new Item(item)
            {
              value = stackCount
            });
            num1 -= stackCount;
          }
          else
          {
            obj7.inventory.Add(new Item(item)
            {
              value = num1
            });
            num1 = 0;
          }
        }
      }
    }
    if (!flag1)
    {
      while (num1 > 0 && (this.inventory.Count < this.inventory_size || objList2.Count <= 0))
      {
        if (num1 > stackCount)
        {
          this.inventory.Add(new Item(item)
          {
            value = stackCount
          });
          num1 -= stackCount;
        }
        else
        {
          this.inventory.Add(new Item(item) { value = num1 });
          num1 = 0;
        }
      }
    }
    if (bags_allowed && objList2.Count > 0)
    {
      for (int index = 0; index < objList2.Count; ++index)
      {
        Item obj9 = objList2[index];
        foreach (Item obj10 in obj9.inventory)
        {
          if (!(obj10.id != id) && obj10.value != stackCount)
          {
            int num7 = stackCount - obj10.value;
            if (num7 >= num1)
            {
              obj10.value += num1;
              num1 = 0;
            }
            else
            {
              num1 -= num7;
              obj10.value = stackCount;
            }
            if (num1 <= 0)
              return true;
          }
        }
        while (num1 > 0 && obj9.inventory.Count < obj9.inventory_size)
        {
          if (num1 > stackCount)
          {
            obj9.inventory.Add(new Item(item)
            {
              value = stackCount
            });
            num1 -= stackCount;
          }
          else
          {
            obj9.inventory.Add(new Item(item)
            {
              value = num1
            });
            num1 = 0;
          }
        }
      }
    }
    return num1 <= 0;
  }

  public bool RemoveItem(Item item, int count = 0, Item try_from_bag = null)
  {
    if (item == null || item.IsEmpty())
      return false;
    if (count == 0)
      count = item.value;
    if (this.GetTotalCount(item.id) < count)
      return false;
    this.RemoveItemNoCheck(item, count, try_from_bag: try_from_bag);
    return true;
  }

  public int RemoveItemNoCheck(
    Item item,
    int count = 0,
    string multiquality_id = "",
    List<Item> out_really_removed_items = null,
    Item try_from_bag = null)
  {
    if (count == 0)
      count = item.value;
    if (try_from_bag != null && (try_from_bag.IsEmpty() || !try_from_bag.is_bag))
      try_from_bag = (Item) null;
    if (string.IsNullOrEmpty(multiquality_id) && item.multiquality_items.Count > 0 && item.definition == null)
    {
      foreach (string multiqualityItem in item.multiquality_items)
      {
        count -= this.RemoveItemNoCheck(new Item(multiqualityItem, count), out_really_removed_items: out_really_removed_items, try_from_bag: try_from_bag);
        if (count < 0)
        {
          Debug.LogError((object) $"Strange happened, count = {count}, mq_item = {multiqualityItem}, item = {item}");
          count = 0;
        }
        if (count == 0)
          break;
      }
      return count;
    }
    int num = 0;
    string str = string.IsNullOrEmpty(multiquality_id) ? item.id : multiquality_id;
    if (GameBalance.me.GetData<ItemDefinition>(str).stack_count == 1)
    {
      while (count > 0)
      {
        if (try_from_bag != null)
        {
          int index = try_from_bag.inventory.LastIndexOf(item);
          if (index < 0)
            index = try_from_bag.GetLastItemIndex(str);
          if (index >= 0)
          {
            try_from_bag.inventory.RemoveAt(index);
            --count;
            ++num;
            if (out_really_removed_items != null)
            {
              // ISSUE: explicit non-virtual call
              __nonvirtual (out_really_removed_items.Add(new Item(str, 1)));
              continue;
            }
            continue;
          }
        }
        int index1 = this.inventory.LastIndexOf(item);
        if (index1 < 0)
          index1 = this.GetLastItemIndex(str);
        if (index1 >= 0)
        {
          this.inventory.RemoveAt(index1);
          --count;
          ++num;
          out_really_removed_items?.Add(new Item(str, 1));
        }
        else
        {
          bool flag = false;
          for (int index2 = 0; index2 < this.inventory.Count; ++index2)
          {
            if (this.inventory[index2] != null && !this.inventory[index2].IsEmpty() && this.inventory[index2].is_bag)
            {
              Item obj = this.inventory[index2];
              int index3 = obj.inventory.LastIndexOf(item);
              if (index3 < 0)
                index3 = obj.GetLastItemIndex(str);
              if (index3 >= 0)
              {
                obj.inventory.RemoveAt(index3);
                --count;
                ++num;
                out_really_removed_items?.Add(new Item(str, 1));
                flag = true;
                break;
              }
            }
          }
          if (!flag)
            return num;
        }
      }
      return num;
    }
    while (count > 0)
    {
      if (try_from_bag != null)
      {
        Item lastItem = try_from_bag.GetLastItem(str);
        if (lastItem != null)
        {
          if (lastItem.value > count)
          {
            num += count;
            lastItem.value -= count;
            out_really_removed_items?.Add(new Item(lastItem.id, count));
            count = 0;
            continue;
          }
          num += lastItem.value;
          count -= lastItem.value;
          out_really_removed_items?.Add(new Item(lastItem.id, this.value));
          try_from_bag.inventory.Remove(lastItem);
          continue;
        }
      }
      Item lastItem1 = this.GetLastItem(str);
      if (lastItem1 != null)
      {
        if (lastItem1.value > count)
        {
          num += count;
          lastItem1.value -= count;
          out_really_removed_items?.Add(new Item(lastItem1.id, count));
          count = 0;
        }
        else
        {
          num += lastItem1.value;
          count -= lastItem1.value;
          out_really_removed_items?.Add(new Item(lastItem1.id, lastItem1.value));
          this.inventory.Remove(lastItem1);
        }
      }
      else
      {
        bool flag = false;
        for (int index = 0; index < this.inventory.Count; ++index)
        {
          if (this.inventory[index] != null && !this.inventory[index].IsEmpty() && this.inventory[index].is_bag)
          {
            Item obj = this.inventory[index];
            Item lastItem2 = obj.GetLastItem(str);
            if (lastItem2 != null)
            {
              if (lastItem2.value > count)
              {
                num += count;
                lastItem2.value -= count;
                out_really_removed_items?.Add(new Item(lastItem2.id, count));
                count = 0;
              }
              else
              {
                num += lastItem2.value;
                count -= lastItem2.value;
                out_really_removed_items?.Add(new Item(lastItem2.id, this.value));
                obj.inventory.Remove(lastItem2);
              }
              flag = true;
              break;
            }
          }
        }
        if (!flag)
          return num;
      }
    }
    return num;
  }

  public int RemoveItemOrReturnLeftCount(
    Item item,
    int count = 0,
    string multiquality_id = "",
    List<Item> out_really_removed_items = null)
  {
    if (item == null || item.IsEmpty())
      return 0;
    if (count == 0)
      count = item.value;
    int totalCount = this.GetTotalCount(string.IsNullOrEmpty(multiquality_id) ? item.id : multiquality_id);
    if (totalCount >= count)
    {
      this.RemoveItemNoCheck(item, count, multiquality_id, out_really_removed_items);
      return 0;
    }
    int num = count - totalCount;
    this.RemoveItemNoCheck(item, totalCount, multiquality_id, out_really_removed_items);
    return num;
  }

  public int GetLastItemIndex(string item_id)
  {
    for (int index = this.inventory.Count - 1; index >= 0; --index)
    {
      if (!(this.inventory[index].id != item_id))
        return index;
    }
    return -1;
  }

  public Item GetLastItem(string item_id)
  {
    int lastItemIndex = this.GetLastItemIndex(item_id);
    return lastItemIndex >= 0 ? this.inventory[lastItemIndex] : (Item) null;
  }

  public bool CanAddItem(Item item, bool count_empty = true, bool count_bags = true)
  {
    return item != null && !item.IsEmpty() && this.CanAddCount(item, count_empty, count_bags) >= item.value;
  }

  public bool CanCollectItemAsDrop(Item item)
  {
    ItemDefinition definition = item.definition;
    return (definition == null ? 0 : (definition.item_size > 1 ? 1 : 0)) == 0 && this.CanAddItem(item);
  }

  public int CanAddCount(Item item, bool count_empty, bool count_bags = true)
  {
    return item == null || item.IsEmpty() ? 0 : this.CanAddCount(item.id, count_empty, count_bags);
  }

  public int CanAddCount(string item_id, bool count_empty, bool count_bags = true)
  {
    if (string.IsNullOrEmpty(item_id))
      return 0;
    ItemDefinition data = GameBalance.me.GetData<ItemDefinition>(item_id);
    if (data == null)
      return 0;
    int stackCount = data.stack_count;
    if (stackCount <= 1 && !count_empty)
      return 0;
    int num1 = 0;
    bool flag = false;
    if (stackCount > 1)
    {
      foreach (Item obj1 in this.inventory)
      {
        if (obj1.id == item_id)
          num1 += stackCount - obj1.value;
        else if (count_bags && obj1.is_bag && data.can_be_inserted_in_bag.Contains(obj1.definition.bag_type))
        {
          foreach (Item obj2 in obj1.inventory)
          {
            if (obj2.id == item_id)
              num1 += stackCount - obj2.value;
          }
          if (count_empty)
          {
            ItemDefinition definition = obj1.definition;
            int num2 = definition.bag_size_x * definition.bag_size_y - obj1.inventory.Count;
            if (num2 < 0)
              num2 = 0;
            num1 += stackCount * num2;
            flag = true;
          }
        }
      }
    }
    if (count_empty)
    {
      int num3 = this.inventory_size - this.inventory.Count;
      if (num3 < 0)
        num3 = 0;
      num1 += stackCount * num3;
      if (count_bags && !flag)
      {
        foreach (Item obj in this.inventory)
        {
          if (obj.is_bag && data.can_be_inserted_in_bag.Contains(obj.definition.bag_type))
          {
            int num4 = obj.inventory_size - obj.inventory.Count;
            if (num4 < 0)
              num4 = 0;
            num1 += num4 * stackCount;
          }
        }
      }
    }
    return num1;
  }

  public bool CanAddItems(List<Item> items_to_add, bool include_bags = false)
  {
    if (items_to_add.Count == 0)
      return true;
    Dictionary<string, int> dictionary = new Dictionary<string, int>();
    int num = 0;
    if (!include_bags)
    {
      foreach (Item obj in items_to_add)
      {
        if (obj.definition.stack_count <= 1)
        {
          ++num;
        }
        else
        {
          string id = obj.id;
          if (!dictionary.ContainsKey(id))
            dictionary.Add(id, this.CanAddCount(id, false, include_bags));
          if (dictionary[id] > 0)
          {
            dictionary[id] -= obj.value;
            if (dictionary[id] >= 0)
              continue;
          }
          ++num;
        }
      }
      return this.inventory_size - this.inventory.Count >= num;
    }
    Item obj1 = this.MakeInventoryCopy();
    for (int index = 0; index < items_to_add.Count; ++index)
    {
      if (!items_to_add[index].IsItemInsertableInBag())
      {
        Item obj2 = items_to_add[index];
        items_to_add.RemoveAt(index);
        items_to_add.Insert(0, obj2);
      }
    }
    foreach (Item obj3 in items_to_add)
    {
      if (!obj1.AddItem(obj3))
        return false;
    }
    return true;
  }

  public bool IsEnoughParams(GameRes pars)
  {
    return pars == (GameRes) null || pars.IsEmpty() || this._params.IsEnough(pars);
  }

  public bool IsEnoughParam(GameResAtom par)
  {
    return par == null || par.IsEmpty() || this._params.IsEnough(par);
  }

  public bool IsEnoughItems(Item item, string multiquality_id = "", int used_items = 0, int multiplier = 1)
  {
    if (item == null || item.IsEmpty())
      return true;
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
      num2 += this.GetTotalCount(item_id);
    return num2 - used_items >= item.value * multiplier;
  }

  public bool IsEnoughItems(List<Item> items, int multiplier = 1)
  {
    foreach (Item obj in items)
    {
      if (!this.IsEnoughItems(obj, multiplier: multiplier))
        return false;
    }
    return true;
  }

  public int GetTotalCount(string item_id, bool count_in_bags = true)
  {
    if (string.IsNullOrEmpty(item_id))
      return 0;
    int totalCount = 0;
    if (this.id == item_id)
      totalCount += this.value;
    foreach (Item obj1 in this.inventory)
    {
      if (obj1.id == item_id)
        totalCount += obj1.value;
      if (count_in_bags && obj1.is_bag)
      {
        foreach (Item obj2 in obj1.inventory)
        {
          if (obj2.id == item_id)
            totalCount += obj2.value;
        }
      }
    }
    return totalCount;
  }

  public bool MoveItemTo(MultiInventory another_inventory, Item item, int count = 0)
  {
    if (count == 0 || count > item.value)
      count = item.value;
    item = new Item(item) { value = count };
    if (!this.IsEnoughItems(item) || !another_inventory.CanAddItem(item))
      return false;
    this.RemoveItemNoCheck(item);
    another_inventory.AddItemNoCheck(item);
    return true;
  }

  public override string ToString()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("Item(id = ");
    stringBuilder.Append(this.id);
    stringBuilder.Append(", value = ");
    stringBuilder.Append(this.value.ToString());
    stringBuilder.Append(")");
    return stringBuilder.ToString();
  }

  public float hp
  {
    get => this._params.hp;
    set => this._params.hp = value;
  }

  public float progress
  {
    get => this._params.progress;
    set => this._params.progress = value;
  }

  public float money
  {
    get => this._params.money;
    set => this._params.money = value;
  }

  public void OnBeforeSerialize()
  {
    if (this._serialize_depth > 3 || this.inventory == null || this.inventory.Count == 0)
    {
      this._serialized_data = string.Empty;
    }
    else
    {
      JSONObject jsonObject = new JSONObject(JSONObject.Type.ARRAY);
      for (int index = 0; index < this.inventory.Count; ++index)
        jsonObject.Add(this.inventory[index].ToJSON(this._serialize_depth + 1));
      this._serialized_data = jsonObject.Print();
    }
  }

  public void OnAfterDeserialize()
  {
    this.inventory = new List<Item>();
    if (string.IsNullOrEmpty(this._serialized_data))
      return;
    JSONObject jsonObject = new JSONObject(this._serialized_data);
    if (jsonObject.list == null)
      return;
    for (int index = 0; index < jsonObject.list.Count; ++index)
      this.inventory.Add(JsonUtility.FromJson<Item>(jsonObject.list[index].str));
  }

  public string ToJSON(int ser_depth = 0)
  {
    this._serialize_depth = ser_depth;
    return JsonUtility.ToJson((object) this);
  }

  public float GetItemQuality()
  {
    if (this.IsEmpty() || this.definition == null)
      return 0.0f;
    if (this.id == "body" || this.definition.type == ItemDefinition.ItemType.Body)
    {
      float num1 = this.GetParam("min_quality");
      float num2 = this.GetParam("max_quality");
      if ((double) num2 >= (double) num1)
        return (num2 - num1) * this.durability + num1;
      Debug.LogError((object) $"Something wrong with body qualities: [{this.id}] = {{{num1.ToString()}; {num2.ToString()}}}");
      return 0.0f;
    }
    return this.definition.type == ItemDefinition.ItemType.GraveCover || this.definition.type == ItemDefinition.ItemType.GraveFence || this.definition.type == ItemDefinition.ItemType.GraveStone ? Mathf.Round(this.definition.quality * 10f * this.durability) / 10f : this.definition.quality * (float) this.value;
  }

  public string GetItemQualityString() => "(skull)" + this.GetItemQuality().ToString("0.0");

  public string GetItemConditionString()
  {
    return "(hp)" + Item.FloatNumberToPercentString(this.durability);
  }

  public static string FloatNumberToPercentString(float v)
  {
    int num = Mathf.RoundToInt(v * 100f);
    if (num > 100)
      num = 100;
    if (num < 0)
      num = 0;
    return num.ToString() + "%";
  }

  public float GetItemQualityMultiplyer()
  {
    if (this.IsEmpty() || this.definition == null)
      return 1f;
    if (!(this.id == "body") && this.definition.type != ItemDefinition.ItemType.Body)
      return this.definition.quality_multiplyer;
    float num1 = this.GetParam("min_quality");
    float num2 = this.GetParam("max_quality");
    if ((double) num2 > (double) num1)
      return (num2 - num1) * this.durability + num1;
    Debug.LogError((object) $"Something wrong with body qualities: [{this.id}] = {{{num1.ToString()}; {num2.ToString()}}}");
    return 1f;
  }

  public float GetInventoryQuality(string ignore_item_id = null)
  {
    if (this.inventory == null || this.inventory.Count == 0)
      return 0.0f;
    float inventoryQuality = 0.0f;
    foreach (Item obj in this.inventory)
    {
      if (!(obj.id == ignore_item_id) && obj.definition != null)
      {
        if (obj.definition.type == ItemDefinition.ItemType.Body)
          inventoryQuality += obj.GetInventoryQuality(ignore_item_id);
        else
          inventoryQuality += obj.GetItemQuality();
      }
    }
    return inventoryQuality;
  }

  public float GetInventoryQualityMultiplier()
  {
    if (this.inventory == null || this.inventory.Count == 0)
      return 0.0f;
    float qualityMultiplier = 1f;
    foreach (Item obj in this.inventory)
    {
      if (obj != null && obj.definition != null && !obj.definition.quality_multiplyer.EqualsTo(0.0f, 1f / 1000f))
        qualityMultiplier *= obj.definition.quality_multiplyer;
    }
    return qualityMultiplier;
  }

  public void GetBodySkulls(
    out int negative,
    out int positive,
    out int positive_avaialble,
    bool dont_count_self = false)
  {
    if (dont_count_self)
    {
      negative = 0;
      positive = 0;
    }
    else
    {
      negative = this.GetRedSkullsValue();
      positive = this.GetWhiteSkullsValue();
    }
    foreach (Item obj in this.inventory)
    {
      if (obj != null && obj.definition != null)
      {
        negative += obj.GetRedSkullsValue();
        positive += obj.GetWhiteSkullsValue();
      }
    }
    if (negative < 0)
      negative = 0;
    if (positive < 0)
      positive = 0;
    int num = Mathf.CeilToInt(this.durability * 100f);
    if (num > 90)
      num = 100;
    positive_avaialble = Mathf.FloorToInt((float) (positive * num) / 100f);
  }

  public void UpdateDurability(float delta_time, float parent_modificator = 1f)
  {
    if (this.definition == null)
      return;
    if (!this.definition.has_durability)
    {
      if (this.definition.type != ItemDefinition.ItemType.Rat)
        return;
      foreach (Item obj in this.inventory)
        obj.UpdateDurability(delta_time, parent_modificator);
    }
    else
    {
      if (this.definition.is_update_children_durability)
      {
        float modificatorForChildren = this.definition.durability_modificator_for_children;
        foreach (Item obj in this.inventory)
          obj.UpdateDurability(delta_time, modificatorForChildren);
      }
      float durabilityModificator = this.GetChildrenDurabilityModificator();
      float num = 1f;
      if (!string.IsNullOrEmpty(this.drop_zone_id))
      {
        if (this._cached_drop_zone_id != this.drop_zone_id)
        {
          this._cached_drop_zone_id = this.drop_zone_id;
          this._cached_world_zone = GameBalance.me.GetDataOrNull<WorldZoneDefinition>(this.drop_zone_id);
          this._cached_zone_dur_k = this._cached_world_zone.zone_params.Get("dur_k_" + this.id, 1f);
        }
        if (this._cached_world_zone != null)
          num = this._cached_zone_dur_k;
      }
      this.durability -= this.definition.durability_decrease * parent_modificator * durabilityModificator * delta_time * num;
      if ((double) this.durability >= 1.0 / 1000.0 || string.IsNullOrEmpty(this.definition.dur_0_change))
        return;
      this.SetItemID(this.definition.dur_0_change);
      this.durability = 1f;
    }
  }

  public float GetChildrenDurabilityModificator()
  {
    float durabilityModificator = 1f;
    foreach (Item obj in this.inventory)
    {
      if (obj != null && obj.definition != null)
        durabilityModificator *= obj.definition.durability_modificator;
    }
    return durabilityModificator;
  }

  public bool HasItemInInventory(string item_id) => this.GetItemsCount(item_id) > 0;

  public int GetItemsCount(string item_id, bool count_secondary_inventory = false)
  {
    if (this.inventory == null || this.inventory.Count == 0 || string.IsNullOrEmpty(item_id))
      return 0;
    int itemsCount = 0;
    foreach (Item obj1 in this.inventory)
    {
      if (obj1.is_bag)
      {
        foreach (Item obj2 in obj1.inventory)
        {
          if (obj2.id == item_id)
            itemsCount += obj2.value;
        }
      }
      else if (obj1.id == item_id)
        itemsCount += obj1.value;
    }
    if (count_secondary_inventory && this.secondary_inventory != null && this.secondary_inventory.Count > 0)
    {
      foreach (Item obj in this.secondary_inventory)
      {
        if (obj.id == item_id)
          itemsCount += obj.value;
      }
    }
    return itemsCount;
  }

  public Item GetItemWithID(string id, Item.ItemFindLogics item_find_logics = Item.ItemFindLogics.FirstFound, bool allow_bags = true)
  {
    if (item_find_logics == Item.ItemFindLogics.FirstFound || item_find_logics == Item.ItemFindLogics.LastFound)
    {
      Item itemWithId = (Item) null;
      foreach (Item obj1 in this.inventory)
      {
        if (obj1.id == id)
        {
          itemWithId = obj1;
          if (item_find_logics == Item.ItemFindLogics.FirstFound)
            return itemWithId;
        }
        if (obj1.is_bag)
        {
          foreach (Item obj2 in obj1.inventory)
          {
            if (obj2.id == id)
            {
              itemWithId = obj2;
              if (item_find_logics == Item.ItemFindLogics.FirstFound)
                return itemWithId;
            }
          }
        }
      }
      return itemWithId;
    }
    List<Item> items = new List<Item>();
    foreach (Item obj3 in this.inventory)
    {
      if (obj3.id == id)
        items.Add(obj3);
      else if (obj3.is_bag)
      {
        foreach (Item obj4 in obj3.inventory)
        {
          if (obj4.id == id)
            items.Add(obj4);
        }
      }
    }
    return items.Count != 0 ? Item.SortItemsListByFindLogics(items, item_find_logics)[0] : (Item) null;
  }

  public static List<Item> SortItemsListByFindLogics(
    List<Item> items,
    Item.ItemFindLogics item_find_logics)
  {
    if (item_find_logics != Item.ItemFindLogics.WithLowestDurability)
      throw new ArgumentOutOfRangeException(nameof (item_find_logics), (object) item_find_logics, (string) null);
    items.Sort((Comparison<Item>) ((a, b) => a.durability.CompareTo(b.durability)));
    return items;
  }

  public Item GetItemOfType(ItemDefinition.ItemType type, bool first = true)
  {
    Item itemOfType = (Item) null;
    foreach (Item obj in this.inventory)
    {
      if (obj.definition.type == type)
      {
        itemOfType = obj;
        if (first)
          return itemOfType;
      }
    }
    return itemOfType;
  }

  public string GetBodyDescription()
  {
    int num = Mathf.RoundToInt(this.durability * 100f);
    if (num > 100)
      num = 100;
    else if (num < 0)
      num = 0;
    string v3 = "?";
    if (MainGame.me.player.CanSeeDarkness())
      v3 = (double) this.GetParam("dark") > 0.0 ? "dark" : "not dark";
    return GJL.L("body_descr", num.ToString(), this.GetItemQuality().ToString("0.0"), v3);
  }

  public List<Item> GetInventoryAsCollapsedStacks()
  {
    Dictionary<string, int> dictionary = new Dictionary<string, int>();
    foreach (Item obj in this.inventory)
    {
      if (!dictionary.ContainsKey(obj.id))
        dictionary.Add(obj.id, 0);
      dictionary[obj.id] += obj.value;
    }
    List<Item> asCollapsedStacks = new List<Item>();
    foreach (KeyValuePair<string, int> keyValuePair in dictionary)
      asCollapsedStacks.Add(new Item(keyValuePair.Key, keyValuePair.Value));
    return asCollapsedStacks;
  }

  public bool is_tech_point => TechDefinition.TECH_POINTS.Contains(this.id);

  public GameRes GetParams() => this._params;

  public string InitMultiqualityItems()
  {
    string str1 = string.Empty;
    if (this.id == "money" || this.id == "r" || this.id == "g" || this.id == "b")
      return str1;
    ItemDefinition dataOrNull = GameBalance.me.GetDataOrNull<ItemDefinition>(this.id);
    if (dataOrNull != null)
    {
      dataOrNull.not_used = false;
      return str1;
    }
    string str2 = this.id + ":";
    foreach (ItemDefinition itemDefinition in GameBalance.me.items_data)
    {
      if (itemDefinition.id.StartsWith(str2))
      {
        this.multiquality_items.Add(itemDefinition.id);
        itemDefinition.not_used = false;
      }
    }
    if (this.multiquality_items.Count == 0)
      str1 = $"{str1}Not found multiquality items for group \"{this.id}\"";
    return str1;
  }

  public static void RemoveItemWithIDFromTheList(
    List<Item> list,
    string item_id,
    bool dont_remove_but_set_zero = false)
  {
    for (int index = 0; index < list.Count; ++index)
    {
      if (list[index].id == item_id)
      {
        if (dont_remove_but_set_zero)
        {
          list[index] = new Item(list[index].id, 0);
          break;
        }
        list.RemoveAt(index);
        break;
      }
    }
  }

  public float RecalculateTotalCooldown()
  {
    if (!this.definition.cooldown.has_expression)
    {
      Debug.LogError((object) ("ERROR: Can't recalculate cooldown for item id = " + this.id));
      return 0.0f;
    }
    float num = this.definition.cooldown.EvaluateFloat(MainGame.me.player);
    this.SetParam("_cooldown", num);
    return num;
  }

  public int GetGrayedCooldownPercent()
  {
    if (!this.definition.cooldown.has_expression)
      return 0;
    float num = MainGame.me.player.GetParam("_cooldown_" + this.id);
    if ((double) num <= 0.0)
      return 0;
    float a = this.GetParam("_cooldown");
    if (a.EqualsTo(0.0f))
      a = this.RecalculateTotalCooldown();
    int grayedCooldownPercent = Mathf.RoundToInt((float) ((double) num / (double) a * 100.0));
    if (grayedCooldownPercent > 100)
      grayedCooldownPercent = 100;
    if (grayedCooldownPercent < 0)
      grayedCooldownPercent = 0;
    return grayedCooldownPercent;
  }

  public GameRes UseItem(WorldGameObject wgo = null, Vector3? effect_bubble_pos = null)
  {
    if (this.definition.cooldown.has_expression)
    {
      if (this.GetGrayedCooldownPercent() != 0)
      {
        Debug.LogError((object) $"Can't use item '{this.id}' because of cooldown");
        return new GameRes();
      }
      MainGame.me.player.SetParam("_cooldown_" + this.id, this.RecalculateTotalCooldown());
    }
    MainGame.me.save.quests.CheckKeyQuests("use_item_" + this.id);
    Stats.DesignEvent("Item:Use:" + this.id);
    GUIElements.me.hud.toolbar.Redraw();
    if (!string.IsNullOrEmpty(this.definition.on_use_snd))
      Sounds.PlaySound(this.definition.on_use_snd);
    MainGame.me.player.AddToParams(this.definition.params_on_use);
    if (this.definition.drop_on_use.Count > 0)
      ((UnityEngine.Object) wgo == (UnityEngine.Object) null ? MainGame.me.player : wgo).DropItems(this.definition.drop_on_use, Direction.ToPlayer);
    GameRes res = new GameRes(this.definition.params_on_use);
    if (this.definition.on_use_expressions != null && this.definition.on_use_expressions.Count > 0)
    {
      float num1 = MainGame.me.player.GetParam("energy");
      foreach (SmartExpression onUseExpression in this.definition.on_use_expressions)
        onUseExpression.Evaluate();
      float num2 = MainGame.me.player.GetParam("energy") - num1;
      res.Add("energy", num2);
    }
    string str = this.definition.on_use_script;
    if ((UnityEngine.Object) wgo != (UnityEngine.Object) null)
    {
      wgo.Redraw();
      EffectBubblesManager.ShowImmediately(effect_bubble_pos ?? wgo.bubble_pos_tf.position, res);
      str = wgo.ReplaceStringParams(str);
    }
    if (!string.IsNullOrEmpty(str))
      GS.RunFlowScript(str);
    return res;
  }

  public void OnTraded()
  {
    if (this.definition.on_trade_expressions == null || this.definition.on_trade_expressions.Count <= 0)
      return;
    foreach (SmartExpression onTradeExpression in this.definition.on_trade_expressions)
      onTradeExpression.Evaluate();
  }

  public CraftDefinition GetFixCraftAndPrice(out Item fix_price)
  {
    float durability = this.durability;
    fix_price = new Item();
    fix_price.SetInventorySize(100);
    CraftDefinition fixCraftForItem = GameBalance.me.GetFixCraftForItem(this.id);
    if (fixCraftForItem == null)
    {
      Debug.LogError((object) ("Couldn't find a fix craft for item " + this.id));
      return (CraftDefinition) null;
    }
    foreach (Item need in fixCraftForItem.needs)
    {
      int item_value = Mathf.CeilToInt((float) need.value * (1f - durability));
      fix_price.AddItem(need.id, item_value);
    }
    return fixCraftForItem;
  }

  public string GetDurabilityHint()
  {
    return GJL.L("hint_item_condition", Mathf.RoundToInt(this.durability * 100f).ToString() + "%");
  }

  public static Item GetItemFromList(List<Item> list, string item_id)
  {
    foreach (Item itemFromList in list)
    {
      if (itemFromList.id == item_id)
        return itemFromList;
    }
    return (Item) null;
  }

  public string GetIcon()
  {
    if (this.id == "chapter")
      Debug.Log((object) "chapter");
    if (this._definition_set && this.definition != null)
      return this.definition.GetIcon();
    string str = this.id;
    while (str.Contains(":"))
    {
      str = str.Substring(0, str.LastIndexOf(':'));
      if ((UnityEngine.Object) EasySpritesCollection.GetSprite("i_" + str, true) != (UnityEngine.Object) null)
        return "i_" + str;
    }
    if (GameBalance.me.GetDataOrNull<ItemDefinition>(this.id) == null)
    {
      List<string> itemsOfBaseName = GameBalance.me.GetItemsOfBaseName(this.id);
      if (itemsOfBaseName.Count > 0)
        return GameBalance.me.GetData<ItemDefinition>(itemsOfBaseName[0]).GetIcon();
    }
    return this.definition != null ? this.definition.GetIcon() : "";
  }

  public string GetOverheadIcon()
  {
    if (this.definition != null)
      return this.definition.GetOverheadIcon();
    Debug.LogError((object) $"Can not find icon for {this.id}! Call Bulat.");
    return "";
  }

  public string GetItemName()
  {
    switch (this.id)
    {
      case "money":
        return Trading.FormatMoney((float) this.value / 100f);
      case "faith":
        return GJL.L("faith") + $" (x{this.value:0})";
      default:
        string itemName = this.definition.GetItemName();
        if (this.value > 1)
          itemName += $" (x{this.value:0})";
        return itemName;
    }
  }

  public void ReplaceItemIfNeeded()
  {
    if (this.definition == null || this.definition.item_replace == null || MainGame.me.player.GetParamInt(this.definition.item_replace.player_flag) <= 0)
      return;
    this.id = this.definition.item_replace.replace_id;
    this._definition_set = false;
  }

  public string GetMultiqualityItemDescription()
  {
    string s1 = string.Empty;
    if (!this.is_multiquality)
      return s1;
    List<ItemDefinition> itemDefinitionList = new List<ItemDefinition>();
    foreach (string multiqualityItem in this.multiquality_items)
    {
      ItemDefinition dataOrNull = GameBalance.me.GetDataOrNull<ItemDefinition>(multiqualityItem);
      if (dataOrNull != null)
        itemDefinitionList.Add(dataOrNull);
    }
    string lng_id = this.id + "d";
    string s2 = GJL.L(lng_id);
    if (lng_id != s2 && !string.IsNullOrEmpty(s2))
      s1 = s1 + LocalizedLabel.ColorizeTags(s2, LocalizedLabel.TextColor.SpeechBubble) + "\n";
    if (itemDefinitionList.Count == 0)
      return s1;
    List<GameRes> reses = new List<GameRes>();
    foreach (ItemDefinition itemDefinition in itemDefinitionList)
    {
      GameRes gameRes = new GameRes();
      if (itemDefinition.on_use_expressions != null && itemDefinition.on_use_expressions.Count > 0)
      {
        foreach (SmartExpression onUseExpression in itemDefinition.on_use_expressions)
        {
          if (onUseExpression != null && !onUseExpression.HasNoExpresion())
          {
            foreach (GameResAtom atom in GameRes.ParseSmartExpression(onUseExpression).ToAtomList())
              gameRes.Add(atom);
          }
        }
      }
      foreach (GameResAtom atom in itemDefinition.params_on_use.ToAtomList())
        gameRes.Add(atom);
      reses.Add(gameRes);
    }
    string formattedString = GameRes.ToFormattedString(reses);
    if (!itemDefinitionList[0].can_be_used && itemDefinitionList[0].is_tool && !string.IsNullOrEmpty(formattedString))
      s1 = s1.ConcatWithSeparator(GJL.L("tool_energy_spend", formattedString));
    if (itemDefinitionList[0].can_be_used)
    {
      string str1 = "";
      foreach (SmartExpression onUseExpression in itemDefinitionList[0].on_use_expressions)
      {
        Regex regex = new Regex("AddBuff\\(\"(.+?)\"\\)");
        string expressionString = onUseExpression.GetRawExpressionString();
        if (!string.IsNullOrEmpty(expressionString) && expressionString.Contains("AddBuff("))
        {
          Match match = regex.Match(expressionString);
          if (match.Success)
          {
            BuffDefinition data = GameBalance.me.GetData<BuffDefinition>(match.Groups[1].Captures[0].ToString());
            if (data != null)
            {
              str1 = $"[c][C16000]{data.GetLocalizedName()}[-][/c]";
              string descriptionIfExists = data.GetDescriptionIfExists();
              if (!string.IsNullOrEmpty(descriptionIfExists))
                str1 = $"{str1} ({descriptionIfExists})";
            }
          }
        }
      }
      if (!string.IsNullOrEmpty(str1) || !string.IsNullOrEmpty(formattedString))
      {
        string str2 = formattedString;
        if (string.IsNullOrEmpty(formattedString))
          str2 = str1;
        else if (!string.IsNullOrEmpty(str1))
          str2 = $"{str2}, {str1}";
        s1 = $"{s1.ConcatWithSeparator(GJL.L("item_effect_on_use"))} {str2}";
      }
    }
    return s1;
  }

  public void CalcRatParams()
  {
    if (this.definition.type != ItemDefinition.ItemType.Rat)
    {
      Debug.LogWarning((object) $"Trying to calc Rat params on non-Rat item: \"{this.id}\"");
    }
    else
    {
      float ratSpeed = this.definition.rat_speed;
      float num1 = 1f;
      float ratObedience = this.definition.rat_obedience;
      float num2 = 1f;
      foreach (Item obj in this.inventory)
      {
        if (obj?.definition != null && obj.definition.type == ItemDefinition.ItemType.RatBuff)
        {
          ratSpeed += obj.definition.rat_speed_add;
          ratObedience += obj.definition.rat_obedience_add;
          num1 *= obj.definition.rat_speed_multiply;
          num2 *= obj.definition.rat_obedience_multiply;
        }
      }
      float num3 = ratSpeed * num1;
      float num4 = ratObedience * num2;
      this._params.Set("rat_speed", num3);
      this._params.Set("rat_obedience", num4);
    }
  }

  public List<Item> GetAllRatBuffs()
  {
    if (this.definition.type != ItemDefinition.ItemType.Rat)
    {
      Debug.LogError((object) ("FATAL ERROR: trying to get rat buffs from non-rat item! id = " + this.id));
      return (List<Item>) null;
    }
    List<Item> allRatBuffs = new List<Item>();
    foreach (Item obj in this.inventory)
    {
      if (obj != null)
      {
        ItemDefinition.ItemType? type = obj.definition?.type;
        ItemDefinition.ItemType itemType = ItemDefinition.ItemType.RatBuff;
        if (type.GetValueOrDefault() == itemType & type.HasValue)
          allRatBuffs.Add(obj);
      }
    }
    return allRatBuffs;
  }

  public string GetRatDescription(bool include_base = true)
  {
    StringBuilder stringBuilder = new StringBuilder();
    this.CalcRatParams();
    stringBuilder.Append(GJL.L("Speed"));
    stringBuilder.Append(": ");
    stringBuilder.Append(Mathf.Round(this._params.Get("rat_speed") * 10f) / 10f);
    if (include_base)
    {
      stringBuilder.Append("(");
      stringBuilder.Append(this.definition.rat_speed);
      stringBuilder.Append(")");
    }
    stringBuilder.Append("(speed)");
    stringBuilder.Append("\n");
    stringBuilder.Append(GJL.L("Obedience"));
    stringBuilder.Append(": ");
    stringBuilder.Append(Mathf.Round(this._params.Get("rat_obedience") * 10f) / 10f);
    if (include_base)
    {
      stringBuilder.Append("(");
      stringBuilder.Append(this.definition.rat_obedience);
      stringBuilder.Append(")");
    }
    stringBuilder.Append("(obedience)");
    return stringBuilder.ToString();
  }

  public List<Item> GetCustomInsertionResult(CustomItemInsertion.InsertionType insertion_type)
  {
    List<Item> customInsertionResult = new List<Item>();
    if (insertion_type == CustomItemInsertion.InsertionType.OnUse)
      return customInsertionResult;
    throw new ArgumentOutOfRangeException(nameof (insertion_type), (object) insertion_type, (string) null);
  }

  public bool CanBeInsertedInBag(Item bag_item)
  {
    return !this.IsEmpty() && bag_item != null && !bag_item.IsEmpty() && bag_item.is_bag && this.definition?.can_be_inserted_in_bag != null && this.definition.can_be_inserted_in_bag.Contains(bag_item.definition.bag_type);
  }

  public bool IsItemInsertableInBag()
  {
    return this.definition != null && !this.IsEmpty() && this.definition.can_be_inserted_in_bag != null && this.definition.can_be_inserted_in_bag.Count != 0 && !this.definition.can_be_inserted_in_bag.Contains(ItemDefinition.BagType.None);
  }

  public Item MakeInventoryCopy()
  {
    Item obj1 = new Item("inventory_copy");
    obj1.inventory = new List<Item>();
    obj1.inventory_size = this.inventory_size;
    foreach (Item obj2 in this.inventory)
      obj1.inventory.Add(new Item(obj2));
    return obj1;
  }

  public int GetWhiteSkullsValue()
  {
    return this.definition == null || this.IsEmpty() ? 0 : this.definition.q_plus + this.GetParamInt("bp_plus_mod");
  }

  public int GetRedSkullsValue()
  {
    return this.definition == null || this.IsEmpty() ? 0 : this.definition.q_minus + this.GetParamInt("bp_minus_mod");
  }

  public string GetItemBodyModificators()
  {
    if (this.definition == null)
      return string.Empty;
    string s = "";
    if (this.definition.show_q_hint.has_expression && !this.definition.show_q_hint.EvaluateBoolean())
      return s;
    int redSkullsValue = this.GetRedSkullsValue();
    int whiteSkullsValue = this.GetWhiteSkullsValue();
    if (redSkullsValue != 0)
    {
      s = s.ConcatWithSeparator(redSkullsValue < 0 ? "-" : "+", ", ");
      for (int index = 0; index < Mathf.Abs(redSkullsValue); ++index)
        s += "(rskull)";
    }
    if (whiteSkullsValue != 0)
    {
      s = s.ConcatWithSeparator(whiteSkullsValue < 0 ? "-" : "+", ", ");
      for (int index = 0; index < Mathf.Abs(whiteSkullsValue); ++index)
        s += "(skull)";
    }
    return s;
  }

  public bool IsIDNeedToBeReplaced(string id_to_check, out string id_replaced)
  {
    id_replaced = string.Empty;
    switch (id_to_check)
    {
      case "flesh":
        id_replaced = "flesh:flesh";
        return true;
      case "fat":
        id_replaced = "fat:fat";
        return true;
      case "blood":
        id_replaced = "blood:blood";
        return true;
      case "skin":
        id_replaced = "skin:skin";
        return true;
      default:
        return false;
    }
  }

  public void InitializeInventoryTillIndex(int index)
  {
    this.inventory.AddRange(Enumerable.Repeat<Item>(new Item(), index - this.inventory.Count + 1));
  }

  public enum DurabilityState
  {
    Broken,
    PreBroken,
    Used,
    Full,
  }

  public enum ItemFindLogics
  {
    FirstFound,
    WithLowestDurability,
    LastFound,
  }
}
