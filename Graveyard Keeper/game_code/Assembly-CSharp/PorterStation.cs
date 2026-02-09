// Decompiled with JetBrains decompiler
// Type: PorterStation
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class PorterStation : MonoBehaviour
{
  public const string WORLD_ZONE_POINT_SUFFIX = "_d";
  public const string EVENT_ON_CAME_TO_DESTINATION = "porter_on_came_to_destination";
  public const string EVENT_ON_CAME_TO_SOURCE = "porter_on_came_to_source";
  public const int TWO_HANDED_ITEM_SIZE = 4;
  public bool _is_correctly_inited;
  public WorldGameObject _wgo;
  public WorldZone _source;
  public WorldZone _destination;
  public PorterStation.PorterState state;
  public bool has_linked_worker;
  public DockPoint waiting_point;
  public List<string> _items_black_list = new List<string>();

  public bool is_correctly_inited => this._is_correctly_inited;

  public WorldZone source
  {
    get
    {
      if ((UnityEngine.Object) this._source == (UnityEngine.Object) null)
      {
        this._source = this._wgo.GetMyWorldZone();
        if ((UnityEngine.Object) this._source == (UnityEngine.Object) null)
          Debug.LogError((object) "Source WorldZone is null!");
      }
      return this._source;
    }
  }

  public WorldZone destination
  {
    get
    {
      if ((UnityEngine.Object) this._destination == (UnityEngine.Object) null)
      {
        if (string.IsNullOrEmpty(this.source?.id))
        {
          Debug.LogError((object) "Source WorldZone id is null!");
          return (WorldZone) null;
        }
        string id = string.Empty;
        foreach (TransportPathsDefinition transportPath in GameBalance.me.transport_paths)
        {
          if (!(transportPath.source_zone_id != this.source.id) && !(transportPath.station_wgo_id != this._wgo?.obj_id))
            id = transportPath.destination_zone_id;
        }
        if (string.IsNullOrEmpty(id))
        {
          Debug.LogError((object) $"FATAL ERROR: Not found transport path for source_zone_id=\"{this.source.id}\"; wgo=\"{this._wgo?.obj_id}\"");
          return (WorldZone) null;
        }
        this._destination = WorldZone.GetZoneByID(id);
        if ((UnityEngine.Object) this._destination == (UnityEngine.Object) null)
          Debug.LogError((object) "Destination WorldZone is null!");
      }
      return this._destination;
    }
  }

  public List<string> blacklist
  {
    get => this._items_black_list ?? (this._items_black_list = new List<string>());
    set => this._items_black_list = value ?? new List<string>();
  }

  public bool HasLinkedWorker()
  {
    return !((UnityEngine.Object) this._wgo == (UnityEngine.Object) null) && this._wgo.has_linked_worker && this._wgo.linked_worker.IsWorker();
  }

  public void Start()
  {
    if (this._is_correctly_inited)
      return;
    this.Init();
  }

  public void Init()
  {
    this._is_correctly_inited = false;
    try
    {
      this._wgo = this.GetComponent<WorldGameObject>();
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ex);
    }
    if ((UnityEngine.Object) this._wgo == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Can not init PorterStation: wgo not found!");
    }
    else
    {
      this.has_linked_worker = this.HasLinkedWorker();
      this._is_correctly_inited = true;
      this.state = PorterStation.PorterState.None;
      this._items_black_list = new List<string>();
      Debug.Log((object) $"Inited porter station on wgo \"{this._wgo.obj_id}\"");
    }
  }

  public void Update()
  {
    if (!this._is_correctly_inited || this.state == PorterStation.PorterState.None || !this.has_linked_worker || this.state != PorterStation.PorterState.Waiting || !this.TrySendPorter())
      return;
    Debug.Log((object) ("Porter send to " + this._destination.id));
  }

  public bool TrySendPorter()
  {
    if (!this._is_correctly_inited || (UnityEngine.Object) this.source == (UnityEngine.Object) null || (UnityEngine.Object) this.destination == (UnityEngine.Object) null || !this.has_linked_worker)
      return false;
    Item backpack = this._wgo.linked_worker.worker?.GetBackpack();
    if (backpack == null)
    {
      Debug.LogError((object) "TrySendPorter error: porter_backpack is null!");
      return false;
    }
    GDPoint target_gdp = !(this._destination.id == "mf_wood") ? (!(this._destination.id == "player_tavern_cellar") ? WorldMap.GetGDPointByGDTag(this._destination.id + "_d") : WorldMap.GetGDPointByGDTag($"{this._destination.id}_d{(PorterStation.ShortWayFromCellarToTavernCellarIsAvailable() ? "" : "_2")}")) : WorldMap.GetGDPointByGDTag($"{this._destination.id}_d{(PorterStation.ShortWayFromSteepWorkyardToHomeIsAvailable() ? "" : "_2")}");
    if ((UnityEngine.Object) target_gdp == (UnityEngine.Object) null)
    {
      Debug.LogError((object) $"TrySendPorter error: not found destination_point by tag \"{this._destination.id}_d\"!");
      return false;
    }
    BaseCharacterComponent character = this._wgo.linked_worker.components.character;
    if (character == null)
    {
      Debug.LogError((object) "TrySendPorter error: linked_worker has no BaseCharacterComponent!");
      return false;
    }
    if (!this.FillPorterInventoryFromSource(backpack))
      Debug.LogError((object) "TrySendPorter error: error while FillPorterInventoryFromSource");
    if (backpack.inventory.Count == 0)
      return false;
    this._wgo.linked_worker.TeleportToGDPoint(this.source.id + "_d");
    this._wgo.linked_worker.DrawPuffFX();
    character.GoTo(target_gdp.gameObject, goto_method: MovementComponent.GoToMethod.GDGraph, event_on_complete: "porter_on_came_to_destination", target_gdp: target_gdp);
    character.SetSpeed(1.5f);
    this._wgo.linked_worker.worker.UpdateWorkerSkin(Worker.WorkerActivity.Porter);
    ChunkedGameObject component = this._wgo.linked_worker.GetComponent<ChunkedGameObject>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      component.active_now_because_of_movement = true;
    this.state = PorterStation.PorterState.GoingToDestination;
    return true;
  }

  public List<Item> GetPossibleForCarryingItems_OLD()
  {
    if (!this._is_correctly_inited)
      return (List<Item>) null;
    if ((UnityEngine.Object) this.source == (UnityEngine.Object) null)
      return (List<Item>) null;
    List<Item> carryingItemsOld = new List<Item>();
    List<Inventory> multiInventory = this._source.GetMultiInventory();
    if (multiInventory == null || multiInventory.Count == 0)
      return (List<Item>) null;
    for (int index = 0; index < multiInventory.Count; ++index)
    {
      List<Item> inventory = multiInventory[index].data?.inventory;
      if (inventory != null && inventory.Count != 0)
      {
        foreach (Item obj in inventory)
        {
          if (this.CanCarryItem(obj))
            carryingItemsOld.Add(obj);
        }
      }
    }
    return carryingItemsOld;
  }

  public bool FillPorterInventoryFromSource(Item porter_backpack)
  {
    if (!this._is_correctly_inited || (UnityEngine.Object) this.source == (UnityEngine.Object) null)
      return false;
    List<Inventory> multiInventory = this._source.GetMultiInventory();
    if (multiInventory == null)
      return false;
    for (int index1 = 0; index1 < multiInventory.Count; ++index1)
    {
      List<Item> inventory = multiInventory[index1].data?.inventory;
      if (inventory != null && inventory.Count != 0)
      {
        for (int index2 = 0; index2 < inventory.Count; ++index2)
        {
          if (this.CanCarryItem(inventory[index2]))
          {
            Item obj1 = inventory[index2];
            int backPackCount = this.CanAddToBackPackCount(porter_backpack, obj1);
            if (backPackCount != 0)
            {
              if (obj1.value - backPackCount > 0)
              {
                Item obj2 = new Item(obj1)
                {
                  value = backPackCount
                };
                porter_backpack.AddItem(obj2);
                obj1.value -= backPackCount;
              }
              else
              {
                porter_backpack.AddItem(new Item(obj1));
                inventory.RemoveAt(index2);
                --index2;
              }
            }
          }
        }
      }
    }
    return true;
  }

  public int CanAddToBackPackCount(Item porter_backpack, Item item)
  {
    if (item == null || item.IsEmpty() || porter_backpack == null)
      return 0;
    if (porter_backpack.inventory == null)
      porter_backpack.inventory = new List<Item>();
    int num1 = 10;
    int num2 = 0;
    bool isBig = item.definition.is_big;
    bool isCrate = item.definition.is_crate;
    int stackCount = item.definition.stack_count;
    foreach (Item obj in porter_backpack.inventory)
    {
      if (obj.definition.is_crate)
      {
        num1 = 0;
        num2 = 0;
        break;
      }
      num1 -= obj.definition.is_big ? 4 : 1;
      if (!(isBig | isCrate) && obj.id == item.id)
        num2 += stackCount - obj.value;
    }
    if (isBig)
      return num1 / 4;
    if (!isCrate)
      return num1 * stackCount + num2;
    return num1 != 10 ? 0 : 1;
  }

  public bool CanCarryItem(Item item)
  {
    if (!this._is_correctly_inited)
    {
      Debug.LogError((object) "CanCarryItem error: porter_station is not inited!");
      return false;
    }
    if (item == null || item.IsEmpty())
      return false;
    if ((UnityEngine.Object) this._source == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "CanCarryItem error: _source WorldZone is null!");
      return false;
    }
    bool flag = false;
    foreach (TransportPathsDefinition transportPath in GameBalance.me.transport_paths)
    {
      if (!(transportPath.source_zone_id != this._source.id) && !(transportPath.destination_zone_id != this.destination.id) && !(transportPath.station_wgo_id != this._wgo.obj_id) && transportPath.transport_items.Contains(item.id))
        flag = true;
    }
    return flag && !this.blacklist.Contains(item.id);
  }

  public void OnCameToDestination()
  {
    if ((UnityEngine.Object) this.destination == (UnityEngine.Object) null)
      Debug.LogError((object) "Destination is null!");
    else if ((UnityEngine.Object) this.source == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Source is null!");
    }
    else
    {
      Debug.Log((object) $"Porter {this._wgo.linked_worker.obj_id} is came to destination {this._destination.id}");
      Item backpack = this._wgo.linked_worker.worker.GetBackpack();
      if (backpack == null)
        return;
      this._destination.PutToAllPossibleInventoriesSmart(backpack.inventory, out List<Item> _);
      GDPoint gdPointByGdTag = WorldMap.GetGDPointByGDTag(this._source.id + "_d");
      if ((UnityEngine.Object) gdPointByGdTag == (UnityEngine.Object) null)
      {
        Debug.LogError((object) $"OnCameToDestination error: not found destination_point by tag \"{this._source.id}_d\"!");
      }
      else
      {
        BaseCharacterComponent character = this._wgo.linked_worker.components.character;
        if (character == null)
        {
          Debug.LogError((object) "OnCameToDestination error: linked_worker has no BaseCharacterComponent!");
        }
        else
        {
          character.GoTo(gdPointByGdTag.gameObject, goto_method: MovementComponent.GoToMethod.GDGraph, event_on_complete: "porter_on_came_to_source", target_gdp: gdPointByGdTag);
          character.SetSpeed(1.5f);
          this._wgo.linked_worker.worker.UpdateWorkerSkin(Worker.WorkerActivity.Porter);
          ChunkedGameObject component = this._wgo.linked_worker.GetComponent<ChunkedGameObject>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null)
            component.active_now_because_of_movement = true;
          this.state = PorterStation.PorterState.GoingToSource;
        }
      }
    }
  }

  public void OnCameToSource()
  {
    WorldGameObject linkedWorker = this._wgo.linked_worker;
    ChunkedGameObject component = linkedWorker.GetComponent<ChunkedGameObject>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      component.active_now_because_of_movement = false;
    if ((UnityEngine.Object) this.source == (UnityEngine.Object) null)
      Debug.LogError((object) $"Source on WGO name=\"{this._wgo?.name}\" with obj_id=\"{this._wgo?.obj_id}\" is null!");
    Item backpack = linkedWorker.worker.GetBackpack();
    List<Item> drop_list = new List<Item>();
    for (int index = 0; index < backpack.inventory.Count; ++index)
    {
      if (!this.CanCarryItem(backpack.inventory[index]))
      {
        drop_list.Add(new Item(backpack.inventory[index]));
        backpack.inventory.RemoveAt(index);
        --index;
      }
    }
    if (drop_list.Count > 0)
    {
      List<Item> cant_insert;
      this.source.PutToAllPossibleInventoriesSmart(drop_list, out cant_insert);
      if (cant_insert != null && cant_insert.Count > 0)
        linkedWorker.DropItems(cant_insert);
    }
    if (this.TrySendPorter())
      return;
    this.state = PorterStation.PorterState.Waiting;
    this._wgo.linked_worker.worker.UpdateWorkerSkin(Worker.WorkerActivity.None);
    if ((UnityEngine.Object) this.waiting_point == (UnityEngine.Object) null)
      this.RefindWaitingPoint();
    if (!((UnityEngine.Object) this.waiting_point != (UnityEngine.Object) null))
      return;
    linkedWorker.transform.position = this.waiting_point.transform.position;
    linkedWorker.RefreshPositionCache();
    linkedWorker.gameObject.SetActive(true);
    linkedWorker.components.character.LookAt(this.waiting_point.action_dir);
    linkedWorker.DrawPuffFX();
  }

  public void RefindWaitingPoint()
  {
    DockPoint[] dockPointArray = this._wgo.RefindDockPointsAndGet();
    if (dockPointArray == null || dockPointArray.Length == 0)
      return;
    this.waiting_point = dockPointArray[0];
  }

  public SerializableWGO.SerializeblePorterStation Serialize()
  {
    return new SerializableWGO.SerializeblePorterStation()
    {
      state = this.state,
      items_black_list = this._items_black_list
    };
  }

  public void Deserialize(SerializableWGO.SerializeblePorterStation data)
  {
    this.Init();
    this.state = data.state;
    this._items_black_list = data.items_black_list;
  }

  public static bool ShortWayFromSteepWorkyardToHomeIsAvailable()
  {
    Vector2 vector2 = new Vector2(2208f, 2112f);
    foreach (WorldGameObject worldGameObject in WorldMap.GetWorldGameObjectsByObjId("steep_yellow_blockage"))
    {
      try
      {
        if ((double) ((Vector2) worldGameObject.transform.position - vector2).sqrMagnitude < 1.0)
          return false;
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ("ShortWayFromSteepWorkyardToHomeIsAvailable exeption: " + ex?.ToString()));
      }
    }
    return true;
  }

  public static bool ShortWayFromCellarToTavernCellarIsAvailable()
  {
    Vector2 vector2 = new Vector2(10992f, -9840f);
    foreach (WorldGameObject worldGameObject in WorldMap.GetWorldGameObjectsByObjId("blockage_H_high"))
    {
      try
      {
        if ((double) ((Vector2) worldGameObject.transform.position - vector2).sqrMagnitude < 1.0)
          return false;
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ("ShortWayFromCellarToTavernCellarIsAvailable exeption: " + ex?.ToString()));
      }
    }
    return true;
  }

  public enum PorterState
  {
    None,
    Waiting,
    GoingToDestination,
    GoingToSource,
  }
}
