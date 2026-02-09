// Decompiled with JetBrains decompiler
// Type: Worker
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class Worker
{
  public const string WORKING_K = "working_k";
  public const string BACKPACK_ID = "porter_backpack";
  public const int BACKPACK_SIZE = 10;
  public const float WORKER_MOVEMENT_SPEED = 1.5f;
  public const string SKIN_CARRIER_BACKPACK_EMPTY = "zombie_1_backpack_empty";
  public const string SKIN_CARRIER_BACKPACK_HALF = "zombie_1_backpack_half";
  public const string SKIN_CARRIER_BACKPACK_FULL = "zombie_1_backpack_full";
  public const string SKIN_CARRIER_BOX = "zombie_1_box";
  public const string SKIN_ZOMBIE_WORKER = "zombie_1";
  public const string INVISIBLE_WORKER_ID = "worker_invisible";
  public const float DEFAULT_WORKER_K = 1f;
  public string id;
  public long worker_unique_id;
  public bool _force_worker_k;
  public float _forced_worker_k;
  public WorkerDefinition _definition;
  [SerializeField]
  public long _wgo_unique_id;
  public WorldGameObject _worker_wgo;

  public WorkerDefinition definition
  {
    get
    {
      if (this._definition == null)
      {
        if (string.IsNullOrEmpty(this.id))
        {
          Debug.LogError((object) "Can not get WorkerDefinition: Worker with empty id!");
          return (WorkerDefinition) null;
        }
        this._definition = GameBalance.me.GetData<WorkerDefinition>(this.id);
      }
      return this._definition;
    }
  }

  public WorldGameObject worker_wgo
  {
    get
    {
      if ((UnityEngine.Object) this._worker_wgo == (UnityEngine.Object) null)
      {
        if (this._wgo_unique_id <= 0L)
        {
          Debug.LogError((object) ("Can not get worker_wgo: _wgo_unique_id=" + this._wgo_unique_id.ToString()));
          return (WorldGameObject) null;
        }
        this._worker_wgo = WorldMap.GetWorldGameObjectByUniqueId(this._wgo_unique_id);
      }
      return this._worker_wgo;
    }
  }

  public Worker()
  {
  }

  public Worker(WorldGameObject worker_wgo, long worker_unique_id)
  {
    this._worker_wgo = worker_wgo;
    this._wgo_unique_id = this._worker_wgo.unique_id;
    this.worker_unique_id = worker_unique_id;
    this._worker_wgo.worker_unique_id = worker_unique_id;
    Debug.Log((object) $"Created new Worker with _wgo_unique_id={this._wgo_unique_id}, worker_unique_id={this.worker_unique_id}");
  }

  public Item GetOnGroundItem()
  {
    if (string.IsNullOrEmpty(this.id))
    {
      Debug.LogError((object) "GetOnGroundItem error: worker id is null!");
      return (Item) null;
    }
    string itemOnGround = this.definition?.item_on_ground;
    if (string.IsNullOrEmpty(itemOnGround))
    {
      Debug.LogError((object) "GetOnGroundItem error: on_ground_item_name is null!");
      return (Item) null;
    }
    Item onGroundItem = new Item(itemOnGround);
    onGroundItem.worker_unique_id = this.worker_unique_id;
    onGroundItem.inventory_size = 99;
    foreach (Item obj in this.worker_wgo.data.inventory)
    {
      if (!(obj.id == "porter_backpack"))
        onGroundItem.inventory.Add(obj);
    }
    return onGroundItem;
  }

  public Item GetOverheadItem()
  {
    if (string.IsNullOrEmpty(this.id))
    {
      Debug.LogError((object) "GetOverheadItem error: worker id is null!");
      return (Item) null;
    }
    string itemOverhead = this.definition?.item_overhead;
    if (string.IsNullOrEmpty(itemOverhead))
    {
      Debug.LogError((object) "GetOverheadItem error: overhead_item_name is null!");
      return (Item) null;
    }
    Item overheadItem = new Item(itemOverhead);
    overheadItem.worker_unique_id = this.worker_unique_id;
    overheadItem.inventory_size = 99;
    foreach (Item obj in this.worker_wgo.data.inventory)
    {
      if (!(obj.id == "porter_backpack"))
        overheadItem.inventory.Add(obj);
    }
    return overheadItem;
  }

  public void UpdateWorkerInventoryFromItem(Item worker_item)
  {
    if (worker_item == null)
      Debug.LogError((object) "UpdateWorkerInventoryFromItem: worker_item is NULL!");
    else if (!worker_item.is_worker)
      Debug.LogError((object) "UpdateWorkerInventoryFromItem: worker_item is NOT worker!");
    else if ((UnityEngine.Object) this.worker_wgo == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "UpdateWorkerInventoryFromItem: worker_wgo is NULL!");
    }
    else
    {
      Item obj1 = (Item) null;
      foreach (Item obj2 in this.worker_wgo.data.inventory)
      {
        if (obj2.id == "porter_backpack")
        {
          obj1 = obj2;
          break;
        }
      }
      this.worker_wgo.data.inventory = worker_item.inventory;
      if (obj1 == null)
        this.GetBackpack();
      else
        this.worker_wgo.data.inventory.Add(obj1);
      this.UpdateWorkerLevel();
    }
  }

  public void UpdateWorkerLevel()
  {
    float num;
    if (this._force_worker_k)
    {
      num = this._forced_worker_k;
    }
    else
    {
      int positive;
      this.worker_wgo.data.GetBodySkulls(out int _, out positive, out int _, true);
      if (positive <= 0)
        positive = 1;
      num = (float) positive / 40f;
    }
    this.worker_wgo.data.SetParam("working_k", num);
  }

  public static string TransformWorker(
    Worker.WorkerState from_state,
    Item in_item,
    WorldGameObject in_wgo,
    Worker.WorkerState to_state,
    out Item out_item,
    out WorldGameObject out_wgo)
  {
    string empty = string.Empty;
    out_item = (Item) null;
    out_wgo = (WorldGameObject) null;
    Worker worker;
    switch (from_state)
    {
      case Worker.WorkerState.WGO:
        if ((UnityEngine.Object) in_wgo == (UnityEngine.Object) null)
          return empty + "TransformWorker error: in_zombie_wgo is null!";
        worker = in_wgo.worker;
        break;
      case Worker.WorkerState.ItemOverhead:
      case Worker.WorkerState.ItemOnGround:
        if (in_item == null)
          return empty + "TransformWorker error: input item is null!";
        worker = in_item.worker;
        if (in_item != null)
        {
          worker.UpdateWorkerInventoryFromItem(in_item);
          break;
        }
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof (from_state), (object) from_state, (string) null);
    }
    if (worker == null)
      return empty + "TransformWorker error: Worker is null!";
    switch (to_state)
    {
      case Worker.WorkerState.WGO:
        out_wgo = worker.worker_wgo;
        if ((UnityEngine.Object) out_wgo == (UnityEngine.Object) null)
          return empty + "Failed worker transformation: worker_wgo is null!";
        break;
      case Worker.WorkerState.ItemOverhead:
        out_item = worker.GetOverheadItem();
        if (out_item == null)
          return empty + "Failed worker transformation: out_item is null!";
        break;
      case Worker.WorkerState.ItemOnGround:
        out_item = worker.GetOnGroundItem();
        if (out_item == null)
          return empty + "Failed worker transformation: out_item is null!";
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof (to_state), (object) to_state, (string) null);
    }
    return empty;
  }

  public Item GetBackpack()
  {
    if (string.IsNullOrEmpty(this.id))
    {
      Debug.LogError((object) "GetBackpack error: worker id is null!");
      return (Item) null;
    }
    foreach (Item backpack in this.worker_wgo.data.inventory)
    {
      if (backpack.id == "porter_backpack")
        return backpack;
    }
    if (!this.worker_wgo.data.AddItem(new Item("porter_backpack", 1)
    {
      inventory_size = 10
    }))
      Debug.LogError((object) "Error while adding backpack item to worker_wgo!");
    foreach (Item backpack in this.worker_wgo.data.inventory)
    {
      if (backpack.id == "porter_backpack")
        return backpack;
    }
    Debug.LogError((object) "FATAL ERROR: impossible shit happen with worker_wgo backpack! Call Bulat.");
    return (Item) null;
  }

  public void UpdateWorkerSkin(Worker.WorkerActivity worker_activity)
  {
    if ((UnityEngine.Object) this.worker_wgo == (UnityEngine.Object) null)
      return;
    WorldObjectPart wop = this._worker_wgo?.GetWOP();
    if ((UnityEngine.Object) wop == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "UpdateWorkerSkin error: worker_wop is null!");
    }
    else
    {
      string skinId = wop.skin_id;
      if (this._worker_wgo.components.character == null)
      {
        Debug.LogError((object) "UpdateWorkerSkin error: worker_char is null!");
      }
      else
      {
        string skin_id;
        switch (worker_activity)
        {
          case Worker.WorkerActivity.None:
            skin_id = "zombie_1";
            this._worker_wgo.components.character.SetCarryingItem((Item) null);
            break;
          case Worker.WorkerActivity.Worker:
            skin_id = "zombie_1";
            this._worker_wgo.components.character.SetCarryingItem((Item) null);
            break;
          case Worker.WorkerActivity.Porter:
            Item backpack = this.GetBackpack();
            if (backpack == null)
            {
              Debug.LogError((object) "UpdateWorkerSkin error: backpack is null!");
              return;
            }
            int count = backpack.inventory.Count;
            if (count == 0)
            {
              skin_id = "zombie_1_backpack_empty";
              this._worker_wgo.components.character.SetCarryingItem((Item) null);
              break;
            }
            if (backpack.GetItemOfType(ItemDefinition.ItemType.Crate) != null)
            {
              skin_id = "zombie_1_box";
              this._worker_wgo.components.character.SetCarryingItem(backpack.GetItemOfType(ItemDefinition.ItemType.Crate));
              break;
            }
            if (count >= 7)
            {
              skin_id = "zombie_1_backpack_full";
              this._worker_wgo.components.character.SetCarryingItem((Item) null);
              break;
            }
            skin_id = "zombie_1_backpack_half";
            this._worker_wgo.components.character.SetCarryingItem((Item) null);
            break;
          default:
            throw new ArgumentOutOfRangeException(nameof (worker_activity), (object) worker_activity, (string) null);
        }
        if (!(skin_id != skinId))
          return;
        this._worker_wgo.ApplySkin(skin_id);
      }
    }
  }

  public void ForcingWorkerK(bool do_force_working_k, float forced_working_k = 1f)
  {
    this._force_worker_k = do_force_working_k;
    this._forced_worker_k = forced_working_k;
  }

  public string GetWorkerEfficiencyText()
  {
    this.UpdateWorkerLevel();
    return GJL.L("work_effeciency", Mathf.RoundToInt(this.worker_wgo.data.GetParam("working_k") * 100f).ToString() + "%");
  }

  public string GetWorkerEfficiencyTextOnlyPercent()
  {
    this.UpdateWorkerLevel();
    return Mathf.RoundToInt(this.worker_wgo.data.GetParam("working_k") * 100f).ToString() + "%";
  }

  public enum WorkerActivity
  {
    None,
    Worker,
    Porter,
  }

  public enum WorkerTransformationType
  {
    FromOverheadToWGO,
    FromOverheadToOnGround,
    FromOnGroundToOverhead,
    FromOnGroundToWGO,
    FromWGOToOverhead,
    FromWGOToOnGround,
  }

  public enum WorkerState
  {
    WGO,
    ItemOverhead,
    ItemOnGround,
  }
}
