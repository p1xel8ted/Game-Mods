// Decompiled with JetBrains decompiler
// Type: SerializableWGO
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using LinqTools;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public struct SerializableWGO
{
  public Vector3 position;
  public string obj_id;
  public Quaternion rotation;
  public Vector3 scale;
  public string custom_tag;
  [SmartDontSerialize]
  public string item_data;
  [SmartSerialize]
  public Item item;
  public int variation;
  public int variation_2;
  public float auto_craft_time_spent;
  public SerializableWGO.SerializableCraft craft;
  public SerializableWGO.SerializebleMovementComponent movement_component;
  public string wop_skin_id;
  public long unique_id;
  public long linked_worker_unique_id;
  public long worker_unique_id;
  public SerializableWGO.SerializableChunk chunk;
  public string interaction_events;
  public string events_json;
  public WorldGameObject.SerializableEvents events_as_class;
  public string cur_gd_point;
  public string cur_zone;
  public float floor_line;
  public int fine_tune_z;
  public string anim_state_json;
  public string parent_gd_point;
  public string last_opened_tab;
  public bool idle_serialized;
  public BaseCharacterIdle.SerializableCharacterIdle idle;
  public bool has_spawner;
  public Vector2 spawner_coords;
  public SerializableWGO.SerializeblePorterStation porter_station;
  public bool is_current_craft_gratitude;

  public static SerializableWGO FromWGO(WorldGameObject wgo)
  {
    Transform transform = wgo.transform;
    if (string.IsNullOrEmpty(wgo.obj_id))
      Debug.LogError((object) ("WGO with an empty ID found, name = " + wgo.name), (UnityEngine.Object) wgo);
    SerializableWGO d = new SerializableWGO()
    {
      obj_id = wgo.obj_id,
      unique_id = wgo.unique_id,
      linked_worker_unique_id = wgo.linked_worker_unique_id,
      worker_unique_id = wgo.worker_unique_id,
      position = transform.position,
      rotation = transform.localRotation,
      scale = transform.localScale,
      custom_tag = wgo.custom_tag,
      variation = wgo.variation,
      variation_2 = wgo.variation_2,
      auto_craft_time_spent = wgo.auto_craft_time_spent,
      craft = wgo.components.craft.GetSerializedCraftComponent(),
      wop_skin_id = ((UnityEngine.Object) wgo.GetWOP() == (UnityEngine.Object) null ? "" : wgo.wop.skin_id),
      interaction_events = (wgo.custom_interaction_events.Count == 0 ? (string) null : string.Join(",", wgo.custom_interaction_events.ToArray())),
      cur_gd_point = wgo.cur_gd_point,
      cur_zone = wgo.cur_zone,
      anim_state_json = "",
      last_opened_tab = wgo.last_opened_tab,
      is_current_craft_gratitude = wgo.is_current_craft_gratitude
    } with
    {
      item = wgo.data
    };
    if (wgo.obj_def.IsPorterStation())
      d.porter_station = wgo.porter_station.Serialize();
    if (wgo.obj_def != null && wgo.obj_def.IsNPC())
      d.anim_state_json = JsonUtility.ToJson((object) wgo.components.animator.stored_state);
    GDPoint parentGdPoint = wgo.GetParentGDPoint();
    d.parent_gd_point = (UnityEngine.Object) parentGdPoint == (UnityEngine.Object) null ? (string) null : parentGdPoint.gd_tag;
    RoundAndSortComponent component = wgo.gameObject.GetComponent<RoundAndSortComponent>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      d.floor_line = component.floor_line;
      d.fine_tune_z = component.fine_tune_z;
    }
    wgo.AdditionalSerialize(ref d);
    ChunkedGameObject chunkedGameObject = wgo.GetComponentInChildren<ChunkedGameObject>();
    if ((UnityEngine.Object) chunkedGameObject == (UnityEngine.Object) null)
      chunkedGameObject = wgo.gameObject.AddComponent<ChunkedGameObject>();
    d.chunk = chunkedGameObject.SerializeChunk();
    d.movement_component = wgo.components.character.GetSerializedMovementComponent();
    d.idle = wgo.components.character.idle_used ? wgo.components.character.idle.Serialize() : (BaseCharacterIdle.SerializableCharacterIdle) null;
    d.idle_serialized = d.idle != null;
    return d;
  }

  public void ToWGO(WorldGameObject wgo, out Item wgo_data)
  {
    Transform transform = wgo.transform;
    transform.position = this.position;
    transform.localRotation = this.rotation;
    transform.localScale = this.scale;
    wgo.unique_id = this.unique_id;
    wgo.linked_worker_unique_id = this.linked_worker_unique_id;
    wgo.worker_unique_id = this.worker_unique_id;
    wgo.custom_tag = this.custom_tag;
    WorldGameObject worldGameObject = wgo;
    List<string> stringList;
    if (this.interaction_events != null)
      stringList = ((IEnumerable<string>) this.interaction_events.Split(new char[1]
      {
        ','
      }, StringSplitOptions.RemoveEmptyEntries)).ToList<string>();
    else
      stringList = new List<string>();
    worldGameObject.custom_interaction_events = stringList;
    wgo.is_current_craft_gratitude = this.is_current_craft_gratitude;
    wgo.round_and_sort.floor_line = this.floor_line;
    wgo.round_and_sort.fine_tune_z = this.fine_tune_z;
    wgo_data = !string.IsNullOrEmpty(this.item_data) ? JsonUtility.FromJson<Item>(this.item_data) : (this.item == null ? new Item() : this.item);
    wgo.variation = this.variation;
    wgo.variation_2 = this.variation_2;
    wgo.auto_craft_time_spent = this.auto_craft_time_spent;
    wgo.obj_id = this.obj_id;
    wgo.obj_def = GameBalance.me.GetData<ObjectDefinition>(this.obj_id);
    wgo.cur_gd_point = this.cur_gd_point;
    wgo.cur_zone = this.cur_zone;
    wgo.last_opened_tab = this.last_opened_tab;
    wgo.components.InitAllComponents();
    wgo.GetComponentInChildren<ChunkedGameObject>().DeserializeChunk(this);
    if (!string.IsNullOrEmpty(this.wop_skin_id))
      wgo.ApplySkin(this.wop_skin_id);
    wgo.components.character.DeserializeMovementComponent(this.movement_component);
    wgo.components.craft.DeserializeCraftComponent(this.craft);
    wgo.AdditionalDeserialize(ref this);
    if (!string.IsNullOrEmpty(this.parent_gd_point))
    {
      GDPoint gdPointByGdTag = WorldMap.GetGDPointByGDTag(this.parent_gd_point, skip_disabled: false);
      if ((UnityEngine.Object) gdPointByGdTag == (UnityEngine.Object) null)
        Debug.LogError((object) ("Couldn't find GD point with tag = " + this.parent_gd_point));
      else
        wgo.transform.SetParent(gdPointByGdTag.transform);
    }
    if (!this.idle_serialized)
      return;
    wgo.components.character.DeserializeIdle(this.idle);
  }

  public void ToWGOAfterSetID(WorldGameObject wgo)
  {
    if (!string.IsNullOrEmpty(this.anim_state_json))
    {
      wgo.components.animator.DeserializeFromSavedState(this.anim_state_json);
      if (wgo.components.animator.stored_state.HasParameter("direction_angle"))
      {
        float parameterFloat = wgo.components.animator.stored_state.GetParameterFloat("direction_angle");
        if (parameterFloat.EqualsTo(90f))
          wgo.components.character.direction = Vector2.up;
        else if (parameterFloat.EqualsTo(180f) || parameterFloat.EqualsTo(-180f))
          wgo.components.character.direction = Vector2.left;
        else if (parameterFloat.EqualsTo(0.0f))
          wgo.components.character.direction = Vector2.right;
      }
    }
    if (!wgo.obj_def.IsPorterStation())
      return;
    wgo.porter_station.Deserialize(this.porter_station);
  }

  [Serializable]
  public struct SerializableChunk
  {
    public bool always_active;
    public bool active_now_because_of_movement;
    public bool active_now_because_of_events;
    public bool active_now_because_of_work;
  }

  [Serializable]
  public struct SerializableCraft
  {
    public bool available;
    public bool is_crafting;
    public string cur_craft_id;
    public string cur_item_id;
    public float cur_item_dur;
    public string dur_item_id;
    public float dur_item_dur;
    public string multiquality_item_id;
    public CraftDefinition.MultiqualityCraftResult multiquality_craft_result;
    public int craft_amount;
    public string last_craft_id;
    public string last_craft_id_2;
    public int cur_last_craft_slot;
    public List<CraftComponent.CraftQueueItem> queue;
    public List<Item> cur_craft_items_used;
    public bool is_gratitude_points_spent_for_craft;
  }

  [Serializable]
  public struct SerializebleMovementComponent
  {
    public bool avaliable;
    public string cur_astar_path;
    public int path_waypoint;
    public MovementComponent.MovementState state;
    public string event_on_complete;
    public string anchor_gd_tag;
    public string anchor_custom_tag;
    public bool anchor_is_wgo;
    public bool using_gd_path;
    public int idle_animation;
    public string target_gd_point_tag;
    public Vector2 astar_dest;
    public Vector2 current_point_pos;
    public Vector3 current_pos;
    public float stored_speed;
    public bool in_stored_speed_mode;
  }

  [Serializable]
  public struct SerializeblePorterStation
  {
    public PorterStation.PorterState state;
    public List<string> items_black_list;
  }
}
