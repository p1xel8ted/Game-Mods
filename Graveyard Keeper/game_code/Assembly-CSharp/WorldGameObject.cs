// Decompiled with JetBrains decompiler
// Type: WorldGameObject
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DungeonGenerator;
using FlowCanvas;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using SmartPools;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;

#nullable disable
public class WorldGameObject : CustomNetworkBehaviour
{
  public bool temp_do_work;
  public const string ID_NOT_SET = "_not_set_";
  public const string DISABLED_INTERACTIONS = "disabled_interactions";
  public const string DYING_TRIGGER = "do_dying";
  public const string TIREDNESS = "tiredness";
  public const float MAX_TIREDNESS = 300f;
  public const string TIRED_BUFF_NAME = "buff_tired";
  public const string TIRED_PARAM_NAME = "tired";
  public const string OUT_ANY_CUSTOM_LOOP = "any_custom_loop_out";
  [SerializeField]
  public CustomDrawers _custom_drawers;
  [HideInInspector]
  public string obj_id = "";
  public string _obj_id = "_not_set_";
  public string custom_tag = "";
  public WorldObjectPart wop;
  public List<WorldObjectPart> additional_wops = new List<WorldObjectPart>();
  public WorldGameObject _linked_worker;
  public bool _has_linked_worker;
  public long linked_worker_unique_id;
  public WorldGameObject linked_workbench;
  public long worker_unique_id = -1;
  public PorterStation _porter_station;
  public Vendor _vendor;
  public Transform content_tf;
  public Vector3 _content_local_pos;
  [HideInInspector]
  public Transform tf;
  public ObjectDefinition obj_def;
  [HideInInspector]
  public int path_cell_size = 1;
  [SerializeField]
  public Item _data = new Item();
  [NonSerialized]
  public long unique_id = -1;
  public bool _prepared_for_interaction;
  public bool _is_player;
  public ItemDefinition.ItemType _current_item_type;
  public float _anim_action_delay;
  public DockPoint[] _dock_points = new DockPoint[0];
  public Camera _cam;
  public bool _cam_cached;
  public BehaviourTreeOwner _beh_tree;
  public Blackboard _blackboard;
  public CanGoTransparent[] _trnsps;
  public Vector3 _cached_pos = Vector3.zero;
  public int _cached_pos_frame = -1;
  public float _object_alpha = 1f;
  public int variation;
  public int variation_2;
  public float auto_craft_time_spent;
  public FlowScriptController _fsc;
  public WorldZone _zone;
  [NonSerialized]
  public GameRes totem_effect = new GameRes();
  public SkinChanger _skin_changer;
  public ComponentsManager _components_manager;
  public bool _components_inited;
  public bool _has_removal_craft;
  public bool _tried_to_find_removal_craft;
  [NonSerialized]
  public List<string> custom_interaction_events = new List<string>();
  public WGOMark.MarkType _mark_type;
  public WGOMark _mark;
  public bool _obj_modified_this_frame;
  public float _obj_quality;
  public BubbleWidgetDataContainer _bubble;
  [NonSerialized]
  public bool show_quality_hint;
  [NonSerialized]
  public List<IntVector2> _cells = new List<IntVector2>();
  [NonSerialized]
  public List<IntVector2> _cells_totem_local = new List<IntVector2>();
  [NonSerialized]
  public bool is_removed;
  public bool is_dead;
  public bool _already_dropped_drop;
  public string cur_gd_point = string.Empty;
  public string cur_zone = string.Empty;
  [NonSerialized]
  public bool just_built;
  public WorldGameObject.SerializableEvents _events = new WorldGameObject.SerializableEvents();
  [NonSerialized]
  public GDPoint _parent_gd_point;
  [NonSerialized]
  public bool _parent_gd_point_inited;
  public Transform _stored_parent_tf;
  public bool _is_marked_removable;
  public bool _is_sort_over_everything;
  public string last_opened_tab = string.Empty;
  public bool _round_and_sort_inited;
  public RoundAndSortComponent _round_and_sort;
  public bool _shown_tutorial_disabled;
  public bool _was_ever_active;
  public bool _ondestroy_was_called;
  [CompilerGenerated]
  public bool \u003Cdont_update\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003Cplaying_disappearing_anim\u003Ek__BackingField;
  public bool is_current_craft_gratitude;

  public bool has_linked_worker => this._has_linked_worker;

  public RoundAndSortComponent round_and_sort
  {
    get
    {
      if (!this._round_and_sort_inited)
      {
        this._round_and_sort_inited = true;
        this._round_and_sort = this.GetComponent<RoundAndSortComponent>();
        if ((UnityEngine.Object) this.round_and_sort == (UnityEngine.Object) null)
          this._round_and_sort = this.gameObject.AddComponent<RoundAndSortComponent>();
      }
      return this._round_and_sort;
    }
  }

  public ComponentsManager components
  {
    get
    {
      if (!this._components_inited)
      {
        this._components_inited = true;
        this._components_manager = new ComponentsManager(this);
      }
      return this._components_manager;
    }
  }

  public void ForceDeinitComponents() => this._components_inited = false;

  public CustomDrawers custom_drawers
  {
    get
    {
      if (this._custom_drawers == null)
        this._custom_drawers = new CustomDrawers();
      if ((UnityEngine.Object) this._custom_drawers.wobj != (UnityEngine.Object) this)
        this._custom_drawers.SetWobj(this);
      return this._custom_drawers;
    }
  }

  public WorldObjectPart GetWOP()
  {
    if ((UnityEngine.Object) this.wop == (UnityEngine.Object) null)
      this.wop = this.GetComponentInChildren<WorldObjectPart>(true);
    return this.wop;
  }

  public Vendor vendor
  {
    get
    {
      if (this._vendor == null)
      {
        VendorDefinition dataOrNull = GameBalance.me.GetDataOrNull<VendorDefinition>(this.obj_id);
        if (this.data.inventory_size == 0)
          this.data.SetInventorySize(this.obj_def.inventory_size);
        if (dataOrNull != null)
          this._vendor = new Vendor(new MultiInventory(new Inventory(this)), dataOrNull, this._data);
      }
      return this._vendor;
    }
  }

  public float quality_k => this.GetParam(nameof (quality_k), 1f);

  public float quality
  {
    get
    {
      if (this.obj_def == null)
        return 0.0f;
      if (this._data == null)
        return this.obj_def.quality.EvaluateFloat();
      if ((double) Mathf.Abs(this.obj_def.quality_multiplier) < 1.0 / 1000.0)
        return 0.0f;
      if (this.obj_def.quality_type == ObjectDefinition.QualityType.Grave)
      {
        Item bodyFromInventory = this.GetBodyFromInventory();
        if (bodyFromInventory == null)
          return this.obj_def.quality.EvaluateFloat();
        string ignore_item_id = (string) null;
        if (this.components.craft.is_crafting)
        {
          if (this.components.craft.current_craft.id.StartsWith("set_"))
            ignore_item_id = this.components.craft.current_craft.needs[0].id;
          else if (this.components.craft.current_craft.id.StartsWith("rem_"))
            ignore_item_id = this.components.craft.current_craft.output[0].id;
        }
        int negative;
        int positive_avaialble;
        bodyFromInventory.GetBodySkulls(out negative, out int _, out positive_avaialble);
        return Mathf.Min(Mathf.Floor(this._data.GetInventoryQuality(ignore_item_id)) - (float) negative, (float) positive_avaialble);
      }
      float num = 1f - this.GetDecayFactor();
      return Mathf.Round((float) (((double) this.obj_def.quality.EvaluateFloat() + (double) this._data.GetInventoryQuality() * (double) this._data.GetInventoryQualityMultiplier() * (double) num) * (double) this.obj_def.quality_multiplier * (double) this.quality_k * 10.0)) / 10f;
    }
  }

  public BubbleWidgetDataContainer bubble
  {
    get
    {
      if (this._bubble == null)
        this._bubble = new BubbleWidgetDataContainer(this);
      return this._bubble;
    }
  }

  public WorldGameObject linked_worker
  {
    get => this._linked_worker;
    set
    {
      if ((UnityEngine.Object) this._linked_worker != (UnityEngine.Object) null)
        this._linked_worker.linked_workbench = (WorldGameObject) null;
      this._linked_worker = value;
      if ((UnityEngine.Object) this._linked_worker != (UnityEngine.Object) null && !this._linked_worker.IsWorker())
      {
        Debug.LogError((object) "Tried to set non-worker wgo as worker!");
        this._linked_worker = (WorldGameObject) null;
      }
      this._has_linked_worker = (UnityEngine.Object) this._linked_worker != (UnityEngine.Object) null;
      this.linked_worker_unique_id = (UnityEngine.Object) this._linked_worker != (UnityEngine.Object) null ? this._linked_worker.unique_id : -1L;
      if ((UnityEngine.Object) this._linked_worker != (UnityEngine.Object) null)
        this._linked_worker.linked_workbench = this;
      if (this.obj_def.IsPorterStation())
      {
        this.porter_station.has_linked_worker = this._has_linked_worker;
        this.porter_station.waiting_point = this.GetAvailableDockPointForZombie();
        if (!this._has_linked_worker)
          return;
        this._linked_worker.worker.UpdateWorkerSkin(Worker.WorkerActivity.Porter);
      }
      else
      {
        if (!((UnityEngine.Object) this._linked_worker != (UnityEngine.Object) null))
          return;
        this._linked_worker.worker.UpdateWorkerSkin(Worker.WorkerActivity.Worker);
      }
    }
  }

  public Worker worker
  {
    get
    {
      return this.worker_unique_id <= 0L ? (Worker) null : MainGame.me.save.workers.GetWorker(this.worker_unique_id);
    }
    set
    {
      if (value == null)
        this.worker_unique_id = -1L;
      else
        this.worker_unique_id = value.worker_unique_id;
    }
  }

  public PorterStation porter_station
  {
    get
    {
      if (this.obj_def.type != ObjectDefinition.ObjType.PorterStation)
        return (PorterStation) null;
      if ((UnityEngine.Object) this._porter_station == (UnityEngine.Object) null)
      {
        try
        {
          this._porter_station = this.GetComponentInChildren<PorterStation>();
          if ((UnityEngine.Object) this._porter_station == (UnityEngine.Object) null)
          {
            this._porter_station = this.gameObject.AddComponent<PorterStation>();
            Debug.Log((object) $"Added PorterStation component to wgo {this.name}[{this.obj_id}]");
          }
          if (!this._porter_station.is_correctly_inited)
            this._porter_station.Init();
          ChunkedGameObject component = this.GetComponent<ChunkedGameObject>();
          if ((UnityEngine.Object) component == (UnityEngine.Object) null)
            Debug.LogError((object) "PorterStation creation error: ChunkedGameObject not found");
          else
            component.active_now_because_of_work = true;
        }
        catch (Exception ex)
        {
          Debug.LogError((object) ex);
        }
      }
      return this._porter_station;
    }
  }

  public bool IsWorker() => this.worker_unique_id > 0L;

  public bool IsInvisibleWorker() => this.obj_id == "worker_invisible";

  public void CheckPositionCache()
  {
    if (this._cached_pos_frame == Time.frameCount)
      return;
    this.RefreshPositionCache();
  }

  public void RefreshPositionCache()
  {
    try
    {
      this._cached_pos = this.tf.position;
    }
    catch (MissingReferenceException ex)
    {
      Debug.LogException(new Exception("tf is null for current WGO " + this.name), (UnityEngine.Object) this);
      this.tf = this.transform;
      this._cached_pos = this.tf.position;
    }
    this._cached_pos_frame = Time.frameCount;
  }

  public void OnValidate() => this.custom_drawers.OnValidate();

  public Vector2 pos
  {
    get
    {
      if (MainGame.game_started)
        this.CheckPositionCache();
      return (Vector2) this._cached_pos;
    }
  }

  public Vector3 pos3
  {
    get
    {
      if (MainGame.game_started)
        this.CheckPositionCache();
      return this._cached_pos;
    }
  }

  public Vector2 grid_pos => this.pos / 96f;

  public bool prepared_for_interaction => this._prepared_for_interaction;

  public bool is_player
  {
    get => this._is_player;
    set => this._is_player = value;
  }

  public bool has_dock_points => this._dock_points.Length != 0;

  public bool dont_update
  {
    get => this.\u003Cdont_update\u003Ek__BackingField;
    set => this.\u003Cdont_update\u003Ek__BackingField = value;
  }

  public Item data => this._data;

  public void RestoreSavedInventory(Item inventory) => this._data = inventory;

  public bool is_autopsy_table
  {
    get => this.obj_id == "autopsi_table" || this.obj_id.Contains("mf_preparation");
  }

  public bool is_body_storage
  {
    get => this.obj_id.StartsWith("corpse_bed") || this.obj_id.StartsWith("corpse_fridge");
  }

  public bool is_soul_extractor_table
  {
    get
    {
      return this.obj_id == "soul_extractor" || this.obj_id == "soul_extractor_2" || this.obj_id == "soul_extractor_3";
    }
  }

  public bool is_rat_cell => this.obj_id.StartsWith("rat_cell");

  public bool playing_disappearing_anim
  {
    get => this.\u003Cplaying_disappearing_anim\u003Ek__BackingField;
    set => this.\u003Cplaying_disappearing_anim\u003Ek__BackingField = value;
  }

  public BubbleCornerPoint GetBubbleCornerPoint()
  {
    GameObject gameObject = (UnityEngine.Object) this.wop == (UnityEngine.Object) null ? this.gameObject : this.wop.gameObject;
    BubbleCornerPoint[] componentsInChildren = gameObject.GetComponentsInChildren<BubbleCornerPoint>(true);
    if (componentsInChildren.Length == 0)
    {
      BubbleCornerPoint bubbleCornerPoint = new GameObject("auto bubble pos").AddComponent<BubbleCornerPoint>();
      bubbleCornerPoint.transform.SetParent(gameObject.transform, false);
      bubbleCornerPoint.transform.localPosition = new Vector3(0.0f, 0.55f, 0.0f);
      bubbleCornerPoint.transform.localScale = Vector3.one;
      return bubbleCornerPoint;
    }
    if (componentsInChildren.Length == 1)
      return componentsInChildren[0];
    foreach (BubbleCornerPoint bubbleCornerPoint in componentsInChildren)
    {
      if (bubbleCornerPoint.gameObject.activeInHierarchy)
        return bubbleCornerPoint;
    }
    return componentsInChildren[0];
  }

  public Transform bubble_pos_tf
  {
    get
    {
      if ((UnityEngine.Object) this.wop == (UnityEngine.Object) null)
        return this.tf;
      BubbleCornerPoint bubbleCornerPoint = this.GetBubbleCornerPoint();
      return !((UnityEngine.Object) bubbleCornerPoint != (UnityEngine.Object) null) ? this.tf : bubbleCornerPoint.transform;
    }
  }

  public Vector3 bubble_pos => this.bubble_pos_tf.position;

  public Camera cam
  {
    get
    {
      if (this._cam_cached)
        return this._cam;
      this._cam_cached = true;
      return this._cam = Camera.main;
    }
  }

  public void Start()
  {
    this._was_ever_active = true;
    if (this._bubble == null)
      this._bubble = new BubbleWidgetDataContainer(this);
    this.NetRegisterDelegate(new CustomNetworkBehaviour.DelegateWGOFloat(this.DoActionNetSynced));
    this._prepared_for_interaction = false;
    this.tf = this.transform;
    this._beh_tree = this.gameObject.GetComponent<BehaviourTreeOwner>();
    this._blackboard = this.gameObject.GetComponent<Blackboard>();
    this.RefindContentParent();
    if (!Application.isPlaying)
      return;
    this.InitNewObject(true);
    this.components.StartComponents();
    RoundAndSortComponent roundAndSort = this.round_and_sort;
    this.InitDockPoints();
    this.UpdateTransparentParts();
    this.custom_drawers.OnObjectRedraw(true);
    if ((UnityEngine.Object) this.GetWOP() != (UnityEngine.Object) null && !string.IsNullOrEmpty(this.wop.skin_id))
      this.ApplySkin(this.wop.skin_id);
    WorldMap.OnAddNewWGO(this);
  }

  public void Awake()
  {
    this._was_ever_active = true;
    DynamicLights.SearchForLightsInNewObject(this.gameObject);
  }

  public void OnDestroy()
  {
    if (this._ondestroy_was_called)
      return;
    this._ondestroy_was_called = true;
    this.is_removed = true;
    DynamicLights.SearchForLightsInDestroyedObject(this.gameObject);
    if (Application.isPlaying)
      InteractionBubbleGUI.RemoveBubble(this.unique_id);
    WorldMap.OnDestroyWGO(this);
  }

  public void RefindContentParent()
  {
    if (this.tf.childCount == 0)
    {
      this.content_tf = this.transform;
      this._content_local_pos = Vector3.zero;
    }
    else if ((UnityEngine.Object) this.tf.GetChild(0).gameObject.GetComponent<WorldObjectPart>() != (UnityEngine.Object) null)
    {
      this.content_tf = this.transform;
      this._content_local_pos = Vector3.zero;
    }
    else
    {
      this.content_tf = this.tf.GetChild(0);
      this._content_local_pos = this.content_tf.localPosition;
    }
  }

  public Vector2 DirTo(WorldGameObject other_obj) => this.DirTo(other_obj.pos);

  public Vector2 DirTo(Vector2 other_pos) => (other_pos - this.pos) / 96f;

  public void RoundContentPos()
  {
    if ((UnityEngine.Object) this.content_tf == (UnityEngine.Object) null)
      return;
    this.content_tf.RoundCamPos(this.cam, this._content_local_pos);
  }

  public void InitDockPoints()
  {
    foreach (DockPoint refindDockPoint in this.RefindDockPoints())
      refindDockPoint.StartDocks(this);
  }

  public void PlaceAtPos(Vector3 position)
  {
    position.z = this.tf.position.z;
    this.tf.position = position;
    if (!this.is_player)
      return;
    CameraTools.MoveToPos(this.pos);
  }

  public int GetParamInt(string param_name) => Mathf.RoundToInt(this.GetParam(param_name));

  public float GetParam(string param_name, float default_value = 0.0f)
  {
    return default_value.EqualsTo(1f) ? this._data.GetParam(param_name, default_value) * this.totem_effect.Get(param_name, default_value) : this._data.GetParam(param_name, default_value) + this.totem_effect.Get(param_name, default_value);
  }

  public void AddToParams(GameRes game_res)
  {
    bool flag = this.IsPlayerInvulnerable();
    foreach (GameResAtom atom in game_res.ToAtomList())
    {
      if (this.is_player)
      {
        switch (atom.type)
        {
          case "hp":
            float num1 = atom.value;
            if (flag && (double) num1 < 0.0)
              num1 = 0.0f;
            this.hp += num1;
            if ((double) this.hp > (double) MainGame.me.save.max_hp)
            {
              this.hp = (float) MainGame.me.save.max_hp;
              continue;
            }
            continue;
          case "energy":
            float num2 = atom.value;
            if (flag && (double) num2 < 0.0)
              num2 = 0.0f;
            this.energy += num2;
            continue;
        }
      }
      this._data.AddToParams(atom);
    }
  }

  public void SubParam(string param_name, float value)
  {
    this._data.SubFromParams(param_name, value);
  }

  public void AddToParams(string param_name, float value)
  {
    this._data.AddToParams(param_name, value);
  }

  public void SetParam(string param_name, float value)
  {
    this._data.SetParam(param_name, value);
    if (!this._is_player)
      return;
    MainGame.me.save.quests.CheckQuestsState();
  }

  public void SetParam(GameRes game_res)
  {
    this._data.SetParam(game_res);
    foreach (GameResAtom atom in game_res.ToAtomList())
    {
      bool flag = false;
      switch (atom.type)
      {
        case "dur_cross":
          Item itemOfType1 = this.GetItemOfType(ItemDefinition.ItemType.GraveStone);
          if (itemOfType1 != null)
            itemOfType1.durability = atom.value;
          flag = true;
          break;
        case "dur_fence":
          Item itemOfType2 = this.GetItemOfType(ItemDefinition.ItemType.GraveFence);
          if (itemOfType2 != null)
            itemOfType2.durability = atom.value;
          flag = true;
          break;
      }
      if (flag)
      {
        this._data.SetParam(atom.type, 0.0f);
        this.Redraw(true);
      }
    }
  }

  public float hp
  {
    get => this._data.hp;
    set => this._data.hp = value;
  }

  public float progress
  {
    get => this._data.progress;
    set => this._data.progress = value;
  }

  public bool AddToInventory(string id, int value) => this.AddToInventory(new Item(id, value));

  public bool AddToInventory(List<Item> items)
  {
    bool inventory = true;
    foreach (Item obj in items)
      inventory = inventory && this.AddToInventory(obj);
    return inventory;
  }

  public bool AddToInventory(Item item)
  {
    this.OnBeganObjectModifications();
    if (!this.is_player && this._data.inventory_size < this.obj_def.inventory_size)
      this._data.SetInventorySize(this.obj_def.inventory_size);
    if (this.obj_def.custom_insertions.Count != 0)
    {
      foreach (CustomItemInsertion customInsertion in this.obj_def.custom_insertions)
      {
        if (customInsertion.item_id == item.id)
        {
          List<Item> objList = new List<Item>();
          if (customInsertion.insertion_type != CustomItemInsertion.InsertionType.OnUse)
            throw new ArgumentOutOfRangeException();
          foreach (Item obj in item.definition.drop_on_use)
          {
            if (!obj.is_tech_point)
              objList.Add(obj);
          }
          if (objList.Count <= 0)
            return true;
          bool inventory = true;
          foreach (Item obj in objList)
            inventory = inventory && this._data.AddItem(obj);
          return inventory;
        }
      }
    }
    if (!this._data.AddItem(item))
      return false;
    if (this.is_player)
    {
      MainGame.me.save.quests.CheckKeyQuests("inventory_change");
      MainGame.me.save.quests.CheckKeyQuests("item_" + item.id);
      GUIElements.me.hud.toolbar.Redraw();
    }
    this.CheckItemAutouse(item);
    return true;
  }

  public bool CheckItemAutouse(Item item)
  {
    if (item == null || item.definition == null || !item.definition.can_be_used || !item.definition.autouse)
      return false;
    this.UseItemFromInventory(item);
    return true;
  }

  public MultiInventory GetMultiInventoryForInteraction(List<WorldGameObject> exceptions = null)
  {
    if (MainGame.me.build_mode_logics.IsBuilding())
      return MainGame.me.build_mode_logics.multi_inventory;
    if (GlobalCraftControlGUI.is_global_control_active)
      return this.GetMultiInventory(exceptions, (string) null, MultiInventory.PlayerMultiInventory.IncludePlayer, true);
    WorldGameObject nearest = this.components.interaction.nearest;
    if (!((UnityEngine.Object) nearest != (UnityEngine.Object) null))
      return this.GetMultiInventory(exceptions, (string) null);
    return (UnityEngine.Object) nearest.GetMyWorldZone() != (UnityEngine.Object) null ? nearest.GetMultiInventory(exceptions, (string) null, MultiInventory.PlayerMultiInventory.IncludePlayer, true) : MainGame.me.player.GetMultiInventory(exceptions, (string) null, include_toolbelt: true);
  }

  public MultiInventory GetMultiInventoryOfWGOWithoutWorldZone(bool duplicate_bags = false)
  {
    MultiInventory withoutWorldZone = new MultiInventory(new Inventory(this));
    if (duplicate_bags)
    {
      foreach (Item data in this.data.inventory)
      {
        if (data != null && !data.IsEmpty() && data.is_bag)
          withoutWorldZone.AddInventory(new Inventory(data, data.id));
      }
    }
    return withoutWorldZone;
  }

  public MultiInventory GetMultiInventory(
    List<WorldGameObject> exceptions = null,
    string force_world_zone = "",
    MultiInventory.PlayerMultiInventory player_mi = MultiInventory.PlayerMultiInventory.DontChange,
    bool include_toolbelt = false,
    bool sortWGOS = false,
    bool include_bags = false)
  {
    MultiInventory multiInventory1 = this.obj_def.open_in_multiinventory ? (this.IsWorker() ? new MultiInventory() : new MultiInventory(new Inventory(this))) : new MultiInventory();
    bool flag1 = false;
    bool flag2 = false;
    if (GlobalCraftControlGUI.is_global_control_active && !this.is_player)
    {
      WorldZone zoneOfObject = WorldZone.GetZoneOfObject(this);
      if ((UnityEngine.Object) zoneOfObject != (UnityEngine.Object) null && !zoneOfObject.IsPlayerInZone())
      {
        player_mi = MultiInventory.PlayerMultiInventory.ExcludePlayer;
        include_bags = false;
      }
      else
      {
        player_mi = MultiInventory.PlayerMultiInventory.IncludePlayer;
        flag2 = true;
      }
    }
    bool flag3 = false;
    if (player_mi == MultiInventory.PlayerMultiInventory.IncludePlayer | flag2 || this.is_player)
    {
      Inventory inventory = new Inventory(MainGame.me.player);
      multiInventory1.AddInventory(inventory);
      flag1 = true;
      if (include_toolbelt)
      {
        Item data = new Item()
        {
          inventory = MainGame.me.player.data.secondary_inventory,
          inventory_size = 7
        };
        multiInventory1.AddInventory(new Inventory(data));
      }
      int specific_position_num = 1;
      foreach (Item data in inventory.data.inventory)
      {
        if (data != null && !data.IsEmpty() && data.is_bag)
          multiInventory1.AddInventory(new Inventory(data, data.id), specific_position_num);
      }
      flag3 = true;
    }
    if (include_bags && !flag3)
    {
      foreach (Item data in this.data.inventory)
      {
        if (data != null && !data.IsEmpty() && data.is_bag)
          multiInventory1.AddInventory(new Inventory(data, data.id));
      }
    }
    WorldZone worldZone = (WorldZone) null;
    if (!this.is_player && this.IsWorker())
    {
      if ((UnityEngine.Object) this.linked_workbench == (UnityEngine.Object) null)
        return multiInventory1;
      worldZone = string.IsNullOrEmpty(force_world_zone) ? this.linked_workbench.GetMyWorldZone() : WorldZone.GetZoneByID(force_world_zone);
    }
    if ((UnityEngine.Object) worldZone == (UnityEngine.Object) null)
      worldZone = string.IsNullOrEmpty(force_world_zone) ? this.GetMyWorldZone() : WorldZone.GetZoneByID(force_world_zone);
    if ((UnityEngine.Object) worldZone != (UnityEngine.Object) null)
    {
      List<Inventory> multiInventory2 = worldZone.GetMultiInventory(exceptions, flag1 ? MultiInventory.PlayerMultiInventory.ExcludePlayer : player_mi, include_toolbelt, sortWGOS);
      if (multiInventory2 != null)
      {
        foreach (Inventory inventory in multiInventory2)
          multiInventory1.AddInventory(inventory);
      }
    }
    if (this.obj_def.additional_worldzone_inventories != null && this.obj_def.additional_worldzone_inventories.Count > 0)
    {
      MultiInventory.PlayerMultiInventory player_mi1 = player_mi;
      if (player_mi1 == MultiInventory.PlayerMultiInventory.IncludePlayer)
        player_mi1 = MultiInventory.PlayerMultiInventory.DontChange;
      foreach (string worldzoneInventory in this.obj_def.additional_worldzone_inventories)
      {
        WorldZone zoneById = WorldZone.GetZoneByID(worldzoneInventory);
        if (!((UnityEngine.Object) zoneById == (UnityEngine.Object) null) && !((UnityEngine.Object) zoneById == (UnityEngine.Object) worldZone))
        {
          foreach (Inventory inventory in zoneById.GetMultiInventory(player_mi: player_mi1, sortWGOS: sortWGOS))
            multiInventory1.AddInventory(inventory);
        }
      }
    }
    else if (!this.is_player && this.IsWorker())
    {
      WorldGameObject linkedWorkbench = this.linked_workbench;
      if (linkedWorkbench?.obj_def?.additional_worldzone_inventories != null && linkedWorkbench != null)
      {
        int? count = linkedWorkbench.obj_def?.additional_worldzone_inventories.Count;
        int num = 0;
        if (count.GetValueOrDefault() > num & count.HasValue)
        {
          MultiInventory.PlayerMultiInventory player_mi2 = player_mi;
          if (player_mi2 == MultiInventory.PlayerMultiInventory.IncludePlayer)
            player_mi2 = MultiInventory.PlayerMultiInventory.DontChange;
          foreach (string worldzoneInventory in linkedWorkbench.obj_def.additional_worldzone_inventories)
          {
            WorldZone zoneById = WorldZone.GetZoneByID(worldzoneInventory);
            if (!((UnityEngine.Object) zoneById == (UnityEngine.Object) null) && !((UnityEngine.Object) zoneById == (UnityEngine.Object) worldZone))
            {
              foreach (Inventory inventory in zoneById.GetMultiInventory(player_mi: player_mi2, sortWGOS: sortWGOS))
                multiInventory1.AddInventory(inventory);
            }
          }
        }
      }
    }
    return multiInventory1;
  }

  public void CustomUpdate()
  {
    if (!Application.isPlaying || this.dont_update || this.is_dead)
      return;
    float deltaTime = Time.deltaTime;
    this._anim_action_delay -= deltaTime;
    this.components.Update(deltaTime);
    if (this._trnsps != null)
      this.UpdateTransparentParts();
    this.UpdateDelayedEvents(deltaTime);
  }

  public void UpdateDelayedEvents(float delta_time)
  {
    for (int index = 0; index < this._events.event_ids.Count; ++index)
    {
      this._events.event_delays[index] -= delta_time;
      if ((double) this._events.event_delays[index] <= 0.0)
      {
        this.FireEvent(this._events.event_ids[index]);
        this._events.event_ids.RemoveAt(index);
        this._events.event_delays.RemoveAt(index);
        --index;
      }
    }
    if (this._events.event_ids.Count != 0)
      return;
    this.GetComponent<ChunkedGameObject>().active_now_because_of_events = false;
  }

  public void UpdateTransparentParts()
  {
    if (!Application.isPlaying || MainGame.disable_all_game)
      return;
    if (this._trnsps == null)
      this._trnsps = this.gameObject.GetComponentsInChildren<CanGoTransparent>();
    float a = 1f;
    Vector3 pos3 = this.pos3;
    Vector3 playerPos = MainGame.me.player_pos;
    float num = playerPos.z - pos3.z;
    if ((double) Mathf.Abs(pos3.x - playerPos.x) < 100.0 && (double) num > 0.0 && (double) num < 50.0)
      a = 0.3f;
    if (a.EqualsTo(this._object_alpha))
      return;
    this._object_alpha = a;
    foreach (CanGoTransparent trnsp in this._trnsps)
      trnsp.SetAlpha(a);
  }

  public void CustomFixedUpdate()
  {
    if (this.dont_update || MainGame.game_starting)
      return;
    this.components.FixedUpdate();
  }

  public void CustomLateUpdate()
  {
    if (!Application.isPlaying || this.dont_update)
      return;
    if (!MainGame.paused)
      this.components.LateUpdate();
    if (this._skin_changer != null)
      this._skin_changer.CustomLateUpdate();
    if (MainGame.paused)
      return;
    if (this._obj_modified_this_frame)
    {
      this._obj_modified_this_frame = false;
      float num = this.quality - this._obj_quality;
      if ((double) Mathf.Abs(num) > 0.05 && !this.is_player)
        this.OnQualityChanged(num);
    }
    this.just_built = false;
  }

  public bool DoAction(WorldGameObject other_obj, float delta_time = -1f)
  {
    if (this.CheckIfDisabledInTutorial())
      return false;
    if ((double) delta_time < 0.0)
      delta_time = Time.deltaTime;
    int num = this.components.DoAction(other_obj, delta_time) ? 1 : 0;
    this.DoAnimAction();
    return num != 0;
  }

  public void DoActionNetSynced(WorldGameObject other_obj, float delta_time)
  {
    this.components.DoAction(other_obj, delta_time);
    this.DoAnimAction();
  }

  public void DoAnimAction()
  {
    if ((double) this._anim_action_delay > 0.0)
      return;
    this._anim_action_delay = 0.5f;
    foreach (InteractionAnimation componentsInChild in this.GetComponentsInChildren<InteractionAnimation>())
      componentsInChild.DoAction();
  }

  public void Interact(WorldGameObject other_obj, bool interaction_start, float delta_time = -1f)
  {
    if ((double) delta_time < 0.0)
      delta_time = Time.deltaTime;
    if (this.CheckDisabledInteractions())
      return;
    bool flag = false;
    if (!string.IsNullOrEmpty(this.obj_def.attached_script))
    {
      string event_id = "interaction";
      if ((!this.obj_def.IsNPC() || this.components.character.anim_state != CharAnimState.Walking) && this.custom_interaction_events.Count > 0)
      {
        event_id = this.custom_interaction_events[0];
        this.custom_interaction_events.RemoveAt(0);
        flag = true;
      }
      Debug.Log((object) $"Fire interaction event = '{event_id}' on wgo = '{((UnityEngine.Object) other_obj == (UnityEngine.Object) null ? "null" : this.name)}'", (UnityEngine.Object) this);
      if (this.obj_def.IsNPC())
        this.AnimatorExitAnyCustomLoop();
      this.FireEvent(event_id);
      this.RedrawBubble();
      if (flag)
        return;
    }
    switch (this.obj_def.interaction_type)
    {
      case ObjectDefinition.InteractionType.Craft:
        if (interaction_start)
        {
          ObjectInteractionDefinition validInteraction = this.obj_def.GetValidInteraction(this);
          if (validInteraction != null)
          {
            string flowscript_name = this.ReplaceStringParams(validInteraction.script);
            if (!string.IsNullOrEmpty(flowscript_name))
            {
              Debug.Log((object) $"<color=yellow>Running interaction script</color> \"{flowscript_name}\" on WGO: '{this.name}' by: '{((UnityEngine.Object) other_obj == (UnityEngine.Object) null ? "null" : other_obj.name)}'", (UnityEngine.Object) this);
              this.AttachFlowScript(flowscript_name, other_obj, (CustomFlowScript.OnFinishedDelegate) (script_name => MainGame.me.player.components.interaction.UpdateNearestHint()));
              this.RedrawBubble();
              return;
            }
            break;
          }
          break;
        }
        break;
      case ObjectDefinition.InteractionType.RunScript:
        if (interaction_start)
        {
          ObjectInteractionDefinition validInteraction = this.obj_def.GetValidInteraction(this);
          if (validInteraction != null)
          {
            string flowscript_name = this.ReplaceStringParams(validInteraction.script);
            if (!string.IsNullOrEmpty(flowscript_name))
            {
              Debug.Log((object) $"<color=yellow>Running interaction script</color> \"{flowscript_name}\" on WGO: '{this.name}' by: '{((UnityEngine.Object) other_obj == (UnityEngine.Object) null ? "null" : other_obj.name)}'", (UnityEngine.Object) this);
              this.AttachFlowScript(flowscript_name, other_obj, (CustomFlowScript.OnFinishedDelegate) (script_name => MainGame.me.player.components.interaction.UpdateNearestHint()));
            }
          }
        }
        this.RedrawBubble();
        return;
      case ObjectDefinition.InteractionType.Builder:
        if (this.obj_id == "mf_wood_builddesk" && MainGame.me.player.GetParamInt("tut_build_wndw_not_shown") == 1)
        {
          MainGame.me.player.SetParam("tut_build_wndw_not_shown", 0.0f);
          GUIElements.me.tutorial.Open("tut_build", (GJCommons.VoidDelegate) (() => { }));
          if (this.custom_interaction_events == null || !this.custom_interaction_events.Contains("first_use"))
            return;
          this.custom_interaction_events.Remove("first_use");
          return;
        }
        MainGame.me.OpenBuildObjectGUI(this);
        this.RedrawBubble();
        return;
      case ObjectDefinition.InteractionType.Chest:
        ObjectInteractionDefinition validInteraction1 = this.obj_def.GetValidInteraction(this);
        if (validInteraction1 != null)
        {
          string flowscript_name = this.ReplaceStringParams(validInteraction1.script);
          if (!string.IsNullOrEmpty(flowscript_name))
          {
            Debug.Log((object) $"<color=yellow>Running interaction script</color> \"{flowscript_name}\" on WGO: '{this.name}' by: '{((UnityEngine.Object) other_obj == (UnityEngine.Object) null ? "null" : other_obj.name)}'", (UnityEngine.Object) this);
            this.AttachFlowScript(flowscript_name, other_obj, (CustomFlowScript.OnFinishedDelegate) (script_name => MainGame.me.player.components.interaction.UpdateNearestHint()));
          }
        }
        else
          GUIElements.me.chest.Open(this);
        this.RedrawBubble();
        return;
      case ObjectDefinition.InteractionType.Grave:
        GUIElements.me.grave.Open(this);
        this.RedrawBubble();
        return;
    }
    this.components.Interact(other_obj, interaction_start, delta_time);
  }

  public void AnimatorExitAnyCustomLoop()
  {
    if ((UnityEngine.Object) this.components.animator == (UnityEngine.Object) null)
      return;
    this.components.animator.SetTrigger("any_custom_loop_out");
    GJTimer.AddTimer(0.0f, (GJTimer.VoidDelegate) (() => this.components.animator.ResetTrigger("any_custom_loop_out")));
  }

  public virtual void PrepareForInteraction(BaseCharacterComponent for_whom)
  {
    if (this._prepared_for_interaction || !for_whom.wgo.is_player || this.obj_def.type == ObjectDefinition.ObjType.Mob)
      return;
    if (this.obj_def.IsRelationVisible())
      GUIElements.me.relation.Open(this);
    this.components.PrepareForInteraction(for_whom);
    this._prepared_for_interaction = true;
    this.RedrawBubble(new bool?(true));
  }

  public virtual void UnprepareForInteraction()
  {
    if (!this._prepared_for_interaction)
      return;
    if (this.obj_def.IsNPC() || !string.IsNullOrEmpty(this.obj_def.npc_alias))
      GUIElements.me.relation.Hide();
    this.components.UnprepareForInteraction();
    this._prepared_for_interaction = false;
    this.RedrawBubble(new bool?(false));
  }

  public void OnWorkAction()
  {
    try
    {
      if (!this.gameObject.activeInHierarchy)
        this.gameObject.SetActive(true);
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ex);
      return;
    }
    Animator animator = (UnityEngine.Object) this.GetWOP() == (UnityEngine.Object) null ? (Animator) null : this.wop.GetComponent<Animator>();
    if ((UnityEngine.Object) animator == (UnityEngine.Object) null)
    {
      animator = this.GetComponentInChildren<Animator>(true);
      if ((UnityEngine.Object) animator == (UnityEngine.Object) null)
        return;
    }
    if (animator.HasParam("work_action"))
      animator.SetTrigger("work_action");
    if (!this.components.craft.enabled || !animator.HasParam("mf_state") || !this.components.craft.is_crafting || this.is_removing)
      return;
    animator.SetInteger("mf_state", 2);
  }

  public void OnWorkFinished()
  {
    if (!this.components.craft.enabled)
      return;
    this.UpdateCraftStateAnim();
  }

  public void OnCraftStateChanged()
  {
    if (!Application.isPlaying)
      return;
    this.UpdateCraftStateAnim();
    this.RedrawBubble();
  }

  public void UpdateCraftStateAnim()
  {
    if (!this.components.craft.enabled)
      return;
    Animator animator = (UnityEngine.Object) this.GetWOP() == (UnityEngine.Object) null ? (Animator) null : this.wop.GetComponent<Animator>();
    if ((UnityEngine.Object) animator == (UnityEngine.Object) null)
    {
      animator = this.GetComponentInChildren<Animator>(true);
      if ((UnityEngine.Object) animator == (UnityEngine.Object) null)
        return;
    }
    if (!animator.HasParam("mf_state"))
      return;
    animator.SetInteger("mf_state", this.components.craft.is_crafting ? 1 : 0);
  }

  public void EquipItem(Item item, int toolbar_index = -1, Item try_from_bag = null)
  {
    if (item == null || item.IsEmpty() || item.durability_state == Item.DurabilityState.Broken)
      return;
    if (toolbar_index != -1)
    {
      MainGame.me.save.SetToolbarEquipped(item.id, toolbar_index);
    }
    else
    {
      ItemDefinition.EquipmentType equipmentType = item.definition.equipment_type;
      if (equipmentType == ItemDefinition.EquipmentType.None)
        return;
      foreach (Item obj in this._data.inventory)
      {
        if (!obj.IsEmpty() && obj.equipped_as == equipmentType)
          obj.equipped_as = ItemDefinition.EquipmentType.None;
      }
      Item itemFromToolbelt = this.GetItemFromToolbelt(equipmentType);
      if (itemFromToolbelt != null)
        this.UnequipItemFromToolbelt(itemFromToolbelt);
      this.EquipItemToToolbelt(item, try_from_bag);
    }
  }

  public Item GetItemFromToolbelt(ItemDefinition.EquipmentType equipment_type)
  {
    foreach (Item itemFromToolbelt in this._data.secondary_inventory)
    {
      if (itemFromToolbelt != null)
      {
        ItemDefinition.EquipmentType? equipmentType1 = itemFromToolbelt.definition?.equipment_type;
        ItemDefinition.EquipmentType equipmentType2 = equipment_type;
        if (equipmentType1.GetValueOrDefault() == equipmentType2 & equipmentType1.HasValue)
          return itemFromToolbelt;
      }
    }
    return (Item) null;
  }

  public void UnequipItemFromToolbelt(Item item)
  {
    if (!this._data.secondary_inventory.Contains(item))
    {
      Debug.LogError((object) ("Error: Can't unequip item from the toolbelt: not equipped, id = " + item.id));
    }
    else
    {
      this._data.secondary_inventory.Remove(item);
      this._data.inventory.Add(item);
    }
  }

  public bool EquipItemToToolbelt(Item item, Item try_from_bag = null)
  {
    if (this._data.secondary_inventory.Contains(item))
    {
      Debug.LogError((object) ("Error: Can't equip item to the toolbelt: already equipped, id = " + item.id));
      return false;
    }
    this._data.secondary_inventory.Add(item);
    this._data.RemoveItem(item, 1, try_from_bag);
    return true;
  }

  public void TryEquipPickupedDrop(Item item, bool check_last_item = true)
  {
    Item obj1 = this.data.inventory.LastElement<Item>();
    if (check_last_item)
    {
      if (obj1 == null || !(item.id == obj1.id))
        return;
      item = obj1;
    }
    else
    {
      foreach (Item obj2 in this._data.inventory)
      {
        if (obj2.id == item.id)
        {
          item = obj2;
          break;
        }
      }
    }
    ItemDefinition.EquipmentType equipmentType = item.definition.equipment_type;
    if (equipmentType == ItemDefinition.EquipmentType.None || this.GetEquippedItem(equipmentType) != null)
      return;
    this.EquipItem(item);
  }

  public void UnEquipItem(Item item)
  {
    if (item == null || item.IsEmpty())
      return;
    item.equipped_as = ItemDefinition.EquipmentType.None;
    if (!this.is_player)
      return;
    if (MainGame.me.player.data.secondary_inventory.Contains(item))
    {
      this.UnequipItemFromToolbelt(item);
      this.DropItemIfDoesntFitInventory(item);
    }
    else
    {
      if (item.equipped_as == ItemDefinition.EquipmentType.None || !item.is_equipped_to_toolbar)
        return;
      MainGame.me.save.UnEquip(item.id);
    }
  }

  public void DropItemIfDoesntFitInventory(Item item)
  {
    if (this.data.inventory.Count <= this.data.inventory_size)
      return;
    this.DropItem(item);
    this.data.inventory.Remove(item);
  }

  public bool SetCurrentItem(ItemDefinition.ItemType new_item)
  {
    Debug.Log((object) ("set " + new_item.ToString()), (UnityEngine.Object) this);
    this._current_item_type = new_item;
    return true;
  }

  public ItemDefinition.ItemType GetCurrentItemType() => this._current_item_type;

  public ItemDefinition.ItemType GetEquippedWeaponType()
  {
    if (!this.is_player)
      return ItemDefinition.ItemType.None;
    Item equippedWeapon = this.GetEquippedWeapon();
    return equippedWeapon != null ? equippedWeapon.definition.type : ItemDefinition.ItemType.None;
  }

  public Item GetEquippedWeapon() => this.GetEquippedTool(ItemDefinition.ItemType.Sword);

  public Item GetEquippedTool()
  {
    return this.components.interaction.enabled ? this.components.interaction.GetWorkToolTypeForNearest() : (Item) null;
  }

  public ItemDefinition.ItemType GetEquippedToolType()
  {
    Item equippedTool = this.GetEquippedTool();
    return equippedTool != null ? equippedTool.definition.type : ItemDefinition.ItemType.None;
  }

  public Item GetEquippedTool(ItemDefinition.ItemType item_type)
  {
    if (item_type == ItemDefinition.ItemType.Hand)
      return new Item("hand_tool", 1);
    foreach (Item equippedTool in this._data.secondary_inventory)
    {
      if (equippedTool != null)
      {
        ItemDefinition.ItemType? type = equippedTool.definition?.type;
        ItemDefinition.ItemType itemType = item_type;
        if (type.GetValueOrDefault() == itemType & type.HasValue)
          return equippedTool;
      }
    }
    foreach (Item equippedTool in this._data.inventory)
    {
      if (equippedTool != null && equippedTool.equipped_as != ItemDefinition.EquipmentType.None && equippedTool.definition.type == item_type)
        return equippedTool;
    }
    return (Item) null;
  }

  public Item GetEquippedItem(ItemDefinition.EquipmentType eq_type)
  {
    foreach (Item equippedItem in this._data.secondary_inventory)
    {
      if (equippedItem != null)
      {
        ItemDefinition.EquipmentType? equipmentType1 = equippedItem.definition?.equipment_type;
        ItemDefinition.EquipmentType equipmentType2 = eq_type;
        if (equipmentType1.GetValueOrDefault() == equipmentType2 & equipmentType1.HasValue)
          return equippedItem;
      }
    }
    foreach (Item equippedItem in this._data.inventory)
    {
      if (equippedItem != null && equippedItem.equipped_as == eq_type)
        return equippedItem;
    }
    return (Item) null;
  }

  public float GetDamage(ObjectDefinition.DamageType damage_type)
  {
    int num = (int) damage_type;
    string param_name = "damage" + (num == 0 ? "" : "_" + num.ToString());
    if (!this.is_player)
      return this._data.GetParam(param_name);
    Item equippedWeapon = this.GetEquippedWeapon();
    if (equippedWeapon != null)
      return equippedWeapon.GetCalculatedParam(param_name) + this.GetParam("add_damage");
    Debug.LogError((object) "Weapon is nul!!!");
    return 10f;
  }

  public bool ActionCanBeDone(ItemDefinition.ItemType item_type)
  {
    return this.obj_def.tool_actions.HasToolK(item_type);
  }

  public void GetAllComponentsAndSort()
  {
  }

  public void DoPreZeroHPActivity()
  {
    Debug.Log((object) ("Do pre zero HP zctivity on " + this.name), (UnityEngine.Object) this);
    if (this.is_player || this.obj_def.type == ObjectDefinition.ObjType.Mob)
    {
      BaseCharacterComponent character = this.components.character;
      if (character.enabled && character.attack.enabled && character.attack.performing_attack)
        character.InterruptAttack();
      if (this.is_player && character.movement_state != MovementComponent.MovementState.None)
        character.StopMovement();
    }
    this._already_dropped_drop = false;
    if (!this.is_player && !this.is_dead && this.obj_def.type == ObjectDefinition.ObjType.Default)
    {
      this.DoZeroHPActivity();
    }
    else
    {
      bool flag = true;
      if ((UnityEngine.Object) this.components.animator != (UnityEngine.Object) null)
      {
        foreach (AnimatorControllerParameter parameter in this.components.animator.parameters)
        {
          if (parameter.type == AnimatorControllerParameterType.Trigger && parameter.name == "do_dying")
            flag = false;
        }
      }
      if (flag)
      {
        this.DoZeroHPActivity();
      }
      else
      {
        this.components.animator.SetTrigger("do_dying");
        this.components.character.SetAnimationState(CharAnimState.Dying);
        this.is_dead = true;
        this.components.character.body.bodyType = RigidbodyType2D.Static;
        if (this.obj_def.type == ObjectDefinition.ObjType.Mob)
        {
          BehaviourTreeOwner component = this.wop.GetComponent<BehaviourTreeOwner>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null)
            component.enabled = false;
        }
        if (this.obj_def.do_drop_after_dying_anim_finished)
          return;
        List<Item> items = ResModificator.ProcessItemsListBeforeDrop(this.obj_def.drop_items, this, MainGame.me.player);
        if (this.obj_def.drop_inventory_after_remove && string.IsNullOrEmpty(this.obj_def.after_hp_0.GetValue(this, MainGame.me.player)))
        {
          foreach (Item obj in this.data.inventory)
          {
            if (obj != null && !string.IsNullOrEmpty(obj.id) && obj.value >= 1)
              items.Add(obj);
          }
        }
        if (!string.IsNullOrEmpty(this.obj_def.drop_sound))
          Sounds.PlaySound(this.obj_def.drop_sound);
        this.DropItems(items);
        this._already_dropped_drop = true;
      }
    }
  }

  public void DoZeroHPActivity()
  {
    Debug.Log((object) "do zero activity", (UnityEngine.Object) this);
    Stats.DesignEvent("HP:ZeroHP:" + this.obj_id);
    if (this.is_player)
    {
      MainGame.me.OnPlayerDied();
    }
    else
    {
      this.UnlinkWithSpawnerIfExists();
      this.RewardForWork();
      ObjectDefinition def = this.obj_def;
      this.SetParam(def.set_param_after_hp_0);
      this.AddToParams(def.add_param_after_hp_0);
      GameRes game_res = new GameRes(def.add_player_param_after_hp_0);
      if (def.add_player_param_after_hp_0_k.has_expression)
      {
        float num = def.add_player_param_after_hp_0_k.EvaluateFloat(this);
        foreach (GameResAtom atom in game_res.ToAtomList())
          game_res.Set(atom.type, atom.value * num);
      }
      MainGame.me.player.AddToParams(game_res);
      if ((double) game_res.Get("hp") < 0.0)
        EffectBubblesManager.ShowStackedHP(MainGame.me.player, game_res.Get("hp"));
      this.ResetDocks(true);
      MainGame.me.save.quests.CheckKeyQuests("zerohp_" + this.obj_id);
      string after_hp_0 = def.after_hp_0.GetValue(this, MainGame.me.player);
      if (!string.IsNullOrEmpty(after_hp_0))
      {
        string craft_name = (string) null;
        if (!string.IsNullOrEmpty(def.craft_after_hp_0))
          craft_name = def.craft_after_hp_0;
        bool need_save_var = def.save_variation;
        int t_varioation = this.variation;
        GJCommons.VoidDelegate on_done = (GJCommons.VoidDelegate) (() =>
        {
          if (!this._already_dropped_drop)
          {
            List<Item> items = ResModificator.ProcessItemsListBeforeDrop(def.drop_items, this, MainGame.me.player);
            if (def.drop_inventory_after_remove)
            {
              foreach (Item obj in this.data.inventory)
              {
                if (obj != null && !string.IsNullOrEmpty(obj.id) && obj.value >= 1)
                  items.Add(obj);
              }
              this.data.inventory.Clear();
            }
            if (!string.IsNullOrEmpty(def.drop_sound))
              Sounds.PlaySound(def.drop_sound);
            this.DropItems(items);
            this._already_dropped_drop = true;
          }
          this.ReplaceWithObject(after_hp_0);
          this.SetParam(def.set_param_after_hp_0_end);
          this.ForceRedrawInSmartDrawer();
          this.variation = need_save_var ? t_varioation : 0;
          this.RedrawVariation();
          this.UpdatePathCell();
          this.playing_disappearing_anim = false;
          this.TryStartCraft(craft_name);
        });
        DisappearAnimation componentInChildren = this.GetComponentInChildren<DisappearAnimation>();
        if ((UnityEngine.Object) componentInChildren == (UnityEngine.Object) null)
        {
          on_done();
        }
        else
        {
          componentInChildren.StartAnimation(on_done);
          this.playing_disappearing_anim = true;
        }
        if (string.IsNullOrEmpty(def.script_after_hp_0))
          return;
        if (def.script_after_hp_0.StartsWith("g:"))
          GS.RunFlowScript(this.ReplaceStringParams(def.script_after_hp_0.Substring(2)));
        else
          this.AttachFlowScript(this.ReplaceStringParams(def.script_after_hp_0));
      }
      else
      {
        if (!this._already_dropped_drop)
        {
          List<Item> items = ResModificator.ProcessItemsListBeforeDrop(def.drop_items, this, MainGame.me.player);
          if (def.drop_inventory_after_remove)
          {
            foreach (Item obj in this.data.inventory)
            {
              if (obj != null && !string.IsNullOrEmpty(obj.id) && obj.value >= 1)
                items.Add(obj);
            }
          }
          this.DropItems(items);
          if (!string.IsNullOrEmpty(def.drop_sound))
            Sounds.PlaySound(def.drop_sound);
          this._already_dropped_drop = true;
        }
        if (!string.IsNullOrEmpty(def.script_after_hp_0))
        {
          if (def.script_after_hp_0.StartsWith("g:"))
            GS.RunFlowScript(this.ReplaceStringParams(def.script_after_hp_0.Substring(2)));
          else
            this.AttachFlowScript(this.ReplaceStringParams(def.script_after_hp_0));
        }
        this.SetParam(def.set_param_after_hp_0_end);
        CameraTools.RemoveFromCameraTargets(this.tf);
        this.DestroyMe();
        this.UpdatePathCell();
      }
    }
  }

  public void ProcessMultiQualityOutput_OLD(List<Item> output_items)
  {
    bool flag = this.obj_id.Contains("garden_") && this.obj_id.Contains("_ready");
    foreach (Item outputItem in output_items)
    {
      if (outputItem.is_multiquality)
      {
        int num = flag ? 1 : 0;
      }
    }
  }

  public void RewardForWork()
  {
    if (this.is_player || string.IsNullOrEmpty(this.obj_def.work))
      return;
    WorkDefinition data = GameBalance.me.GetData<WorkDefinition>(this.obj_def.work);
    if (data == null)
      return;
    MainGame.me.player.AddToParams(data.reward);
  }

  public void UpdatePathCell() => this.GetComponent<ChunkedGameObject>().RescanAStar();

  public void ReplaceWithObject(string new_obj_id, bool show_puff = false, int obj_variation_index = -1)
  {
    if (this.is_removed || (UnityEngine.Object) this.gameObject == (UnityEngine.Object) null)
      return;
    Debug.Log((object) $"ReplaceWithObject id = {this.obj_id}, new_id = {new_obj_id}, show_puff = {show_puff}");
    if (((!Application.isPlaying ? 0 : (MainGame.game_started ? 1 : 0)) & (show_puff ? 1 : 0)) != 0)
      this.DrawPuffFX();
    DynamicLights.SearchForLightsInDestroyedObject(this.gameObject);
    this.OnBeganObjectModifications();
    this.SetParam("hp_inited", 0.0f);
    this.SetObject(this.ReplaceStringParams(new_obj_id));
    if (obj_variation_index != -1)
      this.SetVariationByIndex(obj_variation_index);
    DynamicLights.SearchForLightsInNewObject(this.gameObject);
    this.ForceRedrawInSmartDrawer();
    this.bubble.ClearData();
    this._tried_to_find_removal_craft = false;
    this.GetComponent<ChunkedGameObject>().Init(true);
  }

  public string ReplaceStringParams(string s)
  {
    Match match;
    for (; s.Contains("{"); s = match.Groups[1].Captures[0]?.ToString() + this._data.GetParamAsString(match.Groups[2].Captures[0].ToString()) + match.Groups[3].Captures[0]?.ToString())
    {
      match = Regex.Match(s, "(.*){([^}]+)}(.*)");
      if (!match.Success)
      {
        Debug.LogError((object) ("Syntax error in obj_id: " + s));
        return s;
      }
    }
    return s;
  }

  public void DropItemAndFly(Item item, Vector2 dest_point)
  {
    DropResGameObject.DropAndFly(this.GetDropPos(), item, this.tf.parent, dest_point);
  }

  public void DropItem(
    Item item,
    Direction direction = Direction.None,
    Vector3 pos = default (Vector3),
    float force = 1f,
    bool check_walls = true)
  {
    Vector3 pos1 = pos.magnitude.EqualsTo(0.0f) ? this.GetDropPos() : pos;
    if (direction == Direction.ToPlayer)
    {
      Vector2 vec = (Vector2) (MainGame.me.player_pos - pos1);
      force = 1f;
      direction = vec.ToDirection();
      pos1 = (Vector3) ((Vector2) MainGame.me.player_pos - direction.ToVec() * 80f);
    }
    DropResGameObject.Drop(pos1, item, this.tf.parent, direction, force, check_walls: check_walls);
  }

  public Vector3 GetDropPos()
  {
    if (this.obj_def.drop_point == ObjectDefinition.DropPoint.Center || this._dock_points == null || this._dock_points.Length == 0)
      return this.tf.position;
    bool flag = false;
    foreach (DockPoint dockPoint in this._dock_points)
    {
      flag = (UnityEngine.Object) dockPoint == (UnityEngine.Object) null || (UnityEngine.Object) dockPoint.tf == (UnityEngine.Object) null;
      if (flag)
        break;
    }
    if (flag)
      this.InitDockPoints();
    DockPoint dockPoint1 = this._dock_points[0];
    if (this._dock_points.Length > 1)
    {
      Vector3 position = MainGame.me.player.tf.position;
      float num1 = dockPoint1.tf.position.DistSqrTo(position);
      for (int index = 1; index < this._dock_points.Length; ++index)
      {
        float num2 = this._dock_points[index].tf.position.DistSqrTo(position);
        if ((double) num2 < (double) num1)
        {
          num1 = num2;
          dockPoint1 = this._dock_points[index];
        }
      }
    }
    return !((UnityEngine.Object) dockPoint1 == (UnityEngine.Object) null) ? dockPoint1.GetDropPos() : Vector3.zero;
  }

  public void DropItems(List<Item> items, Direction direction = Direction.None)
  {
    if (items == null || items.Count == 0)
      return;
    Vector3 dropPos = this.GetDropPos();
    int r = 0;
    int g = 0;
    int b = 0;
    foreach (Item obj in items)
    {
      if (obj.is_tech_point)
      {
        switch (obj.id)
        {
          case "r":
            r += obj.value;
            continue;
          case "g":
            g += obj.value;
            continue;
          case "b":
            b += obj.value;
            continue;
          default:
            continue;
        }
      }
    }
    if (r + g + b > 0)
      TechPointsDrop.Drop(dropPos, r, g, b);
    foreach (Item obj in items)
    {
      if (!obj.is_tech_point)
        this.DropItem(obj, direction, dropPos);
    }
  }

  public void InitNewObject(bool at_obj_start)
  {
    if (!this.is_player)
      this._data.SetItemID(this.obj_id);
    if (this.obj_id == "")
      return;
    this.ResetDocks(false);
    this.obj_def = GameBalance.me.GetData<ObjectDefinition>(this.obj_id);
    if (this.obj_def == null)
    {
      Debug.LogError((object) $"null object definition for [{this.obj_id}]");
    }
    else
    {
      AuraEmitter auraEmitter = this.components.aura_emitter;
      if (auraEmitter.enabled)
        auraEmitter.Clear();
      AuraReceiver auraReceiver = this.components.aura_receiver;
      if (auraReceiver.enabled)
        auraReceiver.Clear();
      foreach (ObjectGroupDefinition objectGroup in this.obj_def.object_groups)
        this.ApplyObjectGroup(objectGroup);
      if (!at_obj_start)
        this.components.UpdateComponentsSet();
      this._data.SetInventorySize(this.obj_def.inventory_size);
      if (this._skin_changer == null)
        return;
      this._skin_changer.OnWGOChanged();
    }
  }

  public void ResetDocks(bool shouldnt_be_used)
  {
    foreach (DockPoint refindDockPoint in this.RefindDockPoints())
    {
      if ((UnityEngine.Object) refindDockPoint == (UnityEngine.Object) null)
        Debug.LogError((object) ("Dock point is null at: " + this.name), (UnityEngine.Object) this);
      else
        refindDockPoint.Reset(shouldnt_be_used);
    }
  }

  public void ApplyObjectGroup(ObjectGroupDefinition grp)
  {
  }

  public void SetBuildingColor(Color c)
  {
    foreach (SpriteRenderer componentsInChild in this.GetComponentsInChildren<SpriteRenderer>(true))
    {
      if ((!(componentsInChild.color != Color.white) || !(componentsInChild.color != Color.red)) && !((UnityEngine.Object) componentsInChild.gameObject.GetComponent<DynamicSprite>() != (UnityEngine.Object) null))
        componentsInChild.color = c;
    }
  }

  public void MoveWhenPlacingGlobalPos(Vector2 pos)
  {
    this.tf.position = (Vector3) pos with
    {
      z = this.tf.position.z
    };
    this.round_and_sort.DoRound();
  }

  public void MoveWhenPlacingLocalPos(Vector2 pos)
  {
    this.tf.localPosition = (Vector3) pos with
    {
      z = this.tf.localPosition.z
    };
    this.round_and_sort.DoRound();
  }

  public void SetObject(string id)
  {
    if (id == "0")
    {
      this.DestroyMe();
    }
    else
    {
      this.obj_id = id;
      RoundAndSortComponent componentInChildren;
      try
      {
        componentInChildren = this.GetComponentInChildren<RoundAndSortComponent>();
      }
      catch (MissingReferenceException ex)
      {
        Debug.LogError((object) $"MissingReferenceException: obj_id = {this.obj_id}: {ex.ToString()}", (UnityEngine.Object) this);
        return;
      }
      ChunkManager.OnDestroyObject(componentInChildren.GetComponent<ChunkedGameObject>());
      this.InitNewObject(false);
      if (this.obj_def == null)
        return;
      this.Redraw();
      if ((bool) (UnityEngine.Object) componentInChildren)
        componentInChildren.OnChangedSprite();
      foreach (DockPoint refindDockPoint in this.RefindDockPoints())
        refindDockPoint.StartDocks(this);
      if (this.name.Contains("wgo") && !string.IsNullOrEmpty(id))
        this.name = id;
      ChunkManager.OnAddNewObject(this.GetComponentInChildren<ChunkedGameObject>());
      if (!Application.isPlaying || string.IsNullOrEmpty(id) || (UnityEngine.Object) MainGame.me.player == (UnityEngine.Object) null)
        return;
      if ((UnityEngine.Object) this == (UnityEngine.Object) MainGame.me.player.components.interaction.nearest)
        MainGame.me.player.components.interaction.UpdateNearestHint();
      this.components.PreStartComponents();
      WorldMap.OnAddNewWGO(this);
      this.CheckNeededAttachedScript();
    }
  }

  public void OnQualityChanged(float diff)
  {
    if (diff.EqualsTo(0.0f))
      return;
    WorldZone myWorldZone = this.GetMyWorldZone();
    if ((UnityEngine.Object) myWorldZone == (UnityEngine.Object) null || string.IsNullOrEmpty(myWorldZone.definition.quality_icon))
      return;
    string str = !this.obj_def.has_overrode_quality_icon ? myWorldZone.definition.quality_icon : this.obj_def.overrode_quality_icon;
    if (this.just_built)
      return;
    EffectBubblesManager.ShowImmediately(this.bubble_pos, ((double) diff < 0.0 ? "-" : "+") + string.Format("{1}{0:0.0}", (object) Mathf.Abs(diff), (object) str), (double) diff < 0.0 ? EffectBubblesManager.BubbleColor.Red : EffectBubblesManager.BubbleColor.Green, custom_time: 3f);
  }

  public DockPoint[] RefindDockPoints()
  {
    return (UnityEngine.Object) this.wop == (UnityEngine.Object) null ? new DockPoint[0] : (this._dock_points = this.wop.GetComponentsInChildren<DockPoint>(true));
  }

  public void ResetObject()
  {
    if (this.is_player)
      Debug.LogError((object) "cant reset player");
    else if (string.IsNullOrEmpty(this.obj_id))
      Debug.LogError((object) "null or empty obj_id", (UnityEngine.Object) this);
    else
      this.SetObject(this.obj_id);
  }

  [ContextMenu("Redraw")]
  public void EditorRedraw() => this.Redraw(true);

  public void DrawPuffFX(Bounds? bounds = null) => PuffFX.Create(this, bounds);

  public virtual void Redraw(bool force_redraw = false, bool force_redraw_part = false, bool draw_puff = false)
  {
    try
    {
      if ((UnityEngine.Object) this.gameObject == (UnityEngine.Object) null)
      {
        Debug.LogError((object) ("Error: Trying to redraw a null WGO, obj_id = " + this.obj_id));
        return;
      }
    }
    catch (Exception ex)
    {
      Debug.LogError((object) $"Error: Exception in redraw a WGO, obj_id = {this.obj_id}: {ex}");
      return;
    }
    Bounds totalBounds = this.GetTotalBounds();
    if (this.obj_id == "")
    {
      this.InitDockPoints();
    }
    else
    {
      if (this.obj_def == null)
        return;
      this.GetWOP();
      string path_prefix = "objects/";
      switch (this.obj_def.type)
      {
        case ObjectDefinition.ObjType.Mob:
        case ObjectDefinition.ObjType.NPC:
          path_prefix = "mobs/";
          break;
      }
      if (((!Application.isPlaying ? 1 : (this._obj_id != this.obj_id ? 1 : 0)) | (force_redraw_part ? 1 : 0)) != 0)
      {
        this.RedrawPart(ref this.wop, this.obj_id, path_prefix, 0.0f);
        if (Application.isPlaying)
        {
          if (this._obj_id != "_not_set_" && !string.IsNullOrEmpty(this._obj_id))
            draw_puff = true;
          this._obj_id = this.obj_id;
        }
      }
      InteractionBubbleGUI bubble = InteractionBubbleGUI.GetBubble(this.unique_id);
      if ((UnityEngine.Object) bubble != (UnityEngine.Object) null)
        bubble.LinkTransform(this.bubble_pos_tf);
      if ((UnityEngine.Object) this.wop != (UnityEngine.Object) null)
      {
        Blackboard component1 = this.GetComponent<Blackboard>();
        BehaviourTreeOwner component2 = this.wop.GetComponent<BehaviourTreeOwner>();
        if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && (UnityEngine.Object) component2 != (UnityEngine.Object) null)
          component2.blackboard = (IBlackboard) component1;
      }
      this.playing_disappearing_anim = false;
      this.custom_drawers.OnObjectRedraw(force_redraw);
      this.components.UpdateComponentsSet();
      if (this.components.craft.enabled)
        this.components.craft.FillCraftsList();
      this.InitDockPoints();
      this._trnsps = this.gameObject.GetComponentsInChildren<CanGoTransparent>();
      this.UpdateTransparentParts();
      int num1 = this.is_removing ? 1 : 0;
      if ((UnityEngine.Object) this.wop != (UnityEngine.Object) null)
      {
        if (this.wop.variations != null)
        {
          if (!this.wop.variation_can_be_none && this.variation == 0 && this.wop.variations.Count > 1)
            this.variation = 1;
          for (int index = 0; index < this.wop.variations.Count; ++index)
          {
            int num2 = 1 << index;
            GameObject variation = this.wop.variations[index];
            if (!((UnityEngine.Object) variation == (UnityEngine.Object) null))
              variation.SetActive((this.variation & num2) > 0);
          }
        }
        if (this.wop.variations_2 != null)
        {
          if (!this.wop.variation_2_can_be_none && this.variation_2 == 0 && this.wop.variations_2.Count > 1)
            this.variation_2 = 1;
          for (int index = 0; index < this.wop.variations_2.Count; ++index)
          {
            int num3 = 1 << index;
            List<GameObject> list = this.wop.variations_2[index].list;
            if (list != null && list.Count != 0)
            {
              bool flag = (this.variation_2 & num3) > 0;
              foreach (GameObject gameObject in list)
                gameObject.SetActive(flag);
            }
          }
        }
      }
      this.RedrawGroundSprites();
      if (!Application.isPlaying || !MainGame.game_started)
        return;
      if (draw_puff)
        this.DrawPuffFX(new Bounds?(totalBounds));
      BuffsLogics.CheckBuffsGiveConditions();
    }
  }

  public bool CanBeRotatedWhilePlacing()
  {
    return this.wop.variations_are_radiobutton && this.wop.variations != null && this.wop.variations.Count != 0;
  }

  public void NextVariationRadiobutton()
  {
    if (!this.CanBeRotatedWhilePlacing())
      return;
    if (this.variation == 0)
    {
      this.variation = 1;
      this.RedrawVariation();
    }
    else if ((double) this.variation >= Math.Pow(2.0, (double) (this.wop.variations.Count - 1)))
    {
      this.variation = 1;
      this.RedrawVariation();
    }
    else
    {
      this.variation <<= 1;
      this.RedrawVariation();
    }
  }

  public void SetVariationByIndex(int index)
  {
    this.variation = (int) Math.Pow(2.0, (double) index);
    this.RedrawVariation();
  }

  public void PrevVariationRadiobutton()
  {
    if (!this.CanBeRotatedWhilePlacing())
      return;
    if (this.variation == 0)
    {
      this.variation = 1;
      this.RedrawVariation();
    }
    else if (this.variation == 1)
    {
      this.variation = 1 << this.wop.variations.Count - 1;
      this.RedrawVariation();
    }
    else
    {
      this.variation >>= 1;
      this.RedrawVariation();
    }
  }

  public void RedrawVariation()
  {
    if (!((UnityEngine.Object) this.wop != (UnityEngine.Object) null))
      return;
    if (this.wop.variations != null)
    {
      if (!this.wop.variation_can_be_none && this.variation == 0 && this.wop.variations.Count > 1)
        this.variation = 1;
      for (int index = 0; index < this.wop.variations.Count; ++index)
      {
        int num = 1 << index;
        GameObject variation = this.wop.variations[index];
        if (!((UnityEngine.Object) variation == (UnityEngine.Object) null))
          variation.SetActive((this.variation & num) > 0);
      }
    }
    if (this.wop.variations_2 == null)
      return;
    if (!this.wop.variation_2_can_be_none && this.variation_2 == 0 && this.wop.variations_2.Count > 1)
      this.variation_2 = 1;
    for (int index = 0; index < this.wop.variations_2.Count; ++index)
    {
      int num = 1 << index;
      List<GameObject> list = this.wop.variations_2[index].list;
      if (list != null && list.Count != 0)
      {
        bool flag = (this.variation_2 & num) > 0;
        foreach (GameObject gameObject in list)
          gameObject.SetActive(flag);
      }
    }
  }

  public void Say(
    string text,
    GJCommons.VoidDelegate on_disappeared = null,
    bool? to_left = null,
    SpeechBubbleGUI.SpeechBubbleType type = SpeechBubbleGUI.SpeechBubbleType.Talk,
    SmartSpeechEngine.VoiceID force_voice = SmartSpeechEngine.VoiceID.None,
    bool say_as_player = false,
    Transform overrode_pos = null)
  {
    Debug.Log((object) $"Say \"{text}\" on wgo = {this.name} ({this.obj_id})");
    SmartSpeechEngine.VoiceID voice = force_voice == SmartSpeechEngine.VoiceID.None ? (this.is_player | say_as_player ? SmartSpeechEngine.VoiceID.Player : this.obj_def.voice_id) : force_voice;
    SpeechBubbleGUI.ShowMessage(this.unique_id, text, (UnityEngine.Object) overrode_pos == (UnityEngine.Object) null ? this.bubble_pos_tf : overrode_pos, on_disappeared, this.ShowBubbleToLeft(to_left), type: type, is_player: this.is_player | say_as_player, voice: voice);
  }

  public void ShowMultianswer(
    List<AnswerVisualData> answers,
    MultiAnswerGUI.MultiAnswerResult on_chosen,
    bool? to_left = null,
    GJCommons.VoidDelegate on_disappeared = null,
    WorldGameObject talker = null)
  {
    bool control_was_enabled = this.is_player && this.components.character.control_enabled;
    if (this.is_player)
      this.components.character.control_enabled = false;
    Debug.Log((object) $"ShowMultianswer, talker = {((UnityEngine.Object) talker == (UnityEngine.Object) null ? "null" : talker.name)}, answers = {answers.Count.ToString()}");
    MultiAnswerGUI.ShowAnswers(answers, this.bubble_pos_tf, (MultiAnswerGUI.MultiAnswerResult) (chosen =>
    {
      on_chosen(chosen);
      if (!(this.is_player & control_was_enabled))
        return;
      this.components.character.control_enabled = true;
    }), this.components.character.enabled && this.components.character.ShowBubbleToLeft(to_left), on_disappeared, talker);
  }

  public bool ShowBubbleToLeft(bool? to_left)
  {
    return this.components.character.enabled ? this.components.character.ShowBubbleToLeft(to_left) : to_left ?? (double) MainGame.me.player.tf.position.x > (double) this.tf.position.x;
  }

  public void RedrawPart(ref WorldObjectPart o, string object_id, string path_prefix, float z)
  {
    if ((UnityEngine.Object) o != (UnityEngine.Object) null)
    {
      ChunkedGameObject componentInChildren = o.gameObject.GetComponentInChildren<ChunkedGameObject>();
      if ((UnityEngine.Object) componentInChildren != (UnityEngine.Object) null)
        ChunkManager.OnDestroyObject(componentInChildren);
      GJCommons.Destroy((UnityEngine.Object) o.gameObject);
    }
    WorldObjectPart resource = object_id == "" || object_id == "0" ? (WorldObjectPart) null : SmartResourceHelper.GetResource<WorldObjectPart>(path_prefix + object_id);
    if ((UnityEngine.Object) resource != (UnityEngine.Object) null)
    {
      o = this.SmartInstantiate(resource, z);
      if (this.obj_def.IsCharacter())
        o.gameObject.layer = 9;
    }
    if (!((UnityEngine.Object) this.GetComponent<FloatingWorldGameObject>() != (UnityEngine.Object) null))
      return;
    foreach (Behaviour componentsInChild in this.GetComponentsInChildren<Collider2D>(true))
      componentsInChild.enabled = false;
  }

  public WorldObjectPart SmartInstantiate(WorldObjectPart prefab, float z)
  {
    this.RefindContentParent();
    WorldObjectPart worldObjectPart = UnityEngine.Object.Instantiate<WorldObjectPart>(prefab, this.content_tf);
    worldObjectPart.transform.localPosition = new Vector3(prefab.transform.localPosition.x, prefab.transform.localPosition.y, z);
    worldObjectPart.CacheSectors();
    return worldObjectPart;
  }

  public static void ResetAllObjects()
  {
    foreach (WorldGameObject worldGameObject in UnityEngine.Object.FindObjectsOfType<WorldGameObject>())
      worldGameObject.ResetObject();
  }

  public static void ResetMobs()
  {
    foreach (WorldGameObject worldGameObject in UnityEngine.Object.FindObjectsOfType<WorldGameObject>())
    {
      if (worldGameObject.obj_def.type == ObjectDefinition.ObjType.Mob)
        worldGameObject.ResetObject();
    }
  }

  public CustomFlowScript AttachFlowScript(
    string flowscript_name,
    WorldGameObject interractor = null,
    CustomFlowScript.OnFinishedDelegate on_finished = null)
  {
    if (string.IsNullOrEmpty(flowscript_name))
    {
      if (on_finished != null)
        on_finished(flowscript_name);
      return (CustomFlowScript) null;
    }
    if (flowscript_name[0] == ':')
    {
      SmartExpression.ParseExpression(flowscript_name.Substring(1)).Evaluate(this);
      if (on_finished != null)
        on_finished(flowscript_name);
      return (CustomFlowScript) null;
    }
    CustomFlowScript customFlowScript = CustomFlowScript.Create(this.gameObject, flowscript_name, on_finished: on_finished);
    if ((UnityEngine.Object) customFlowScript == (UnityEngine.Object) null)
      return (CustomFlowScript) null;
    customFlowScript.current_interractor = interractor;
    return customFlowScript;
  }

  public Item GetItemOfType(ItemDefinition.ItemType item_type)
  {
    foreach (Item itemOfType in this._data.inventory)
    {
      if (itemOfType != null && itemOfType.definition != null && !itemOfType.definition.is_placeholder && itemOfType.definition.type == item_type)
        return itemOfType;
    }
    return (Item) null;
  }

  public Item GetItemById(string item_id, bool check_bags = true)
  {
    foreach (Item itemById1 in this._data.inventory)
    {
      if (itemById1 != null && !itemById1.IsEmpty())
      {
        if (itemById1.id == item_id)
          return itemById1;
        if (itemById1.is_bag)
        {
          foreach (Item itemById2 in itemById1.inventory)
          {
            if (itemById2 != null && !itemById2.IsEmpty() && !(itemById2.id != item_id))
              return itemById2;
          }
        }
      }
    }
    return (Item) null;
  }

  public float energy
  {
    get => this.GetParam(nameof (energy));
    set
    {
      float f = value - this.GetParam(nameof (energy));
      this.SetParam(nameof (energy), (double) value > (double) MainGame.me.save.max_energy ? (float) MainGame.me.save.max_energy : value);
      if (!this._is_player || (double) f >= 0.0)
        return;
      this.AddToParams("tiredness", Mathf.Abs(f));
      if ((double) this.GetParam("tiredness") <= 300.0)
        return;
      this.SetParam("tiredness", 300f);
      if (this.GetParamInt("tired") != 0)
        return;
      BuffsLogics.AddBuff("buff_tired");
    }
  }

  public float gratitude_points
  {
    get => this.GetParam(nameof (gratitude_points));
    set
    {
      if ((double) value < 0.0)
        value = 0.0f;
      this.SetParam(nameof (gratitude_points), value);
    }
  }

  public void ClearTiredness() => this.SetParam("tiredness", 0.0f);

  public float sanity
  {
    get => this.GetParam(nameof (sanity));
    set
    {
      this.SetParam(nameof (sanity), value);
      if ((double) this.sanity <= (double) MainGame.me.save.max_sanity)
        return;
      this.sanity = (float) MainGame.me.save.max_sanity;
    }
  }

  public GameRes UseItemFromInventory(Item item, Vector3? effect_bubble_pos = null, Item use_from_bag = null)
  {
    if (item == null)
    {
      Debug.LogError((object) "Use item is null");
      return new GameRes();
    }
    if (!item.definition.can_be_used)
    {
      Debug.LogError((object) ("This item can't be used: " + item?.ToString()));
      return new GameRes();
    }
    if (item.definition.stay_on_use || this.data.RemoveItem(item.id, 1, use_from_bag))
      return item.UseItem(this, effect_bubble_pos);
    Debug.LogError((object) "Trying to use absent item", (UnityEngine.Object) this);
    return new GameRes();
  }

  public bool CanInsertItem(Item item)
  {
    if (item == null || this.is_removing)
      return false;
    Item itemOfType = this.GetItemOfType(item.definition.type);
    if (this.obj_def.can_insert_items.Count > 0)
    {
      if (!this.obj_def.can_insert_items.Contains(item.id))
      {
        if (this.obj_def.custom_insertions.Count == 0)
          return false;
        CustomItemInsertion customItemInsertion = (CustomItemInsertion) null;
        foreach (CustomItemInsertion customInsertion in this.obj_def.custom_insertions)
        {
          if (customInsertion.item_id == item.id)
          {
            customItemInsertion = customInsertion;
            break;
          }
        }
        if (customItemInsertion == null)
          return false;
        List<Item> objList = new List<Item>();
        if (customItemInsertion.insertion_type != CustomItemInsertion.InsertionType.OnUse)
          throw new ArgumentOutOfRangeException();
        foreach (Item obj in item.definition.drop_on_use)
          objList.Add(obj);
        foreach (Item obj in objList)
        {
          if (!this.obj_def.can_insert_items.Contains(obj.id))
          {
            Debug.Log((object) $"Can not insert item \"{obj}\" through custom insertion of item \"{item.id}\" to wgo=\"{this.obj_id}\"");
            return false;
          }
        }
        if (this.obj_def.can_insert_items_limit == 0)
          return true;
        int num1 = 0;
        foreach (Item obj in this.data.inventory)
        {
          if (this.obj_def.can_insert_items.Contains(obj.id))
            num1 += obj.value;
        }
        int num2 = 0;
        foreach (Item obj in objList)
          num2 += obj.value;
        return num2 + num1 <= this.obj_def.can_insert_items_limit;
      }
      if (this.obj_def.can_insert_items_limit == 0)
        return true;
      int num = 0;
      foreach (Item obj in this.data.inventory)
      {
        if (this.obj_def.can_insert_items.Contains(obj.id))
          num += obj.value;
      }
      return num < this.obj_def.can_insert_items_limit;
    }
    switch (this.obj_def.id)
    {
      case "autopsi_table":
        if (item.definition.type == ItemDefinition.ItemType.Body || item.definition.type == ItemDefinition.ItemType.ZombieWorker)
          return itemOfType == null;
        break;
      case "bar_barmens_place":
        return !(item.id != "bartender_doll") && this.data.GetTotalCount("bartender_doll") <= 0;
      case "grave_empty":
        if (item.definition.type == ItemDefinition.ItemType.Body)
          return itemOfType == null;
        break;
      case "grave_ground":
        switch (item.definition.type)
        {
          case ItemDefinition.ItemType.GraveStone:
          case ItemDefinition.ItemType.GraveFence:
          case ItemDefinition.ItemType.GraveCover:
            return itemOfType == null;
        }
        break;
      case "mf_crematorium":
      case "mf_pyre":
        return !item.is_worker && item.definition.type == ItemDefinition.ItemType.Body && itemOfType == null && !item.is_worker;
      case "mf_ore_1_complete":
        return !(item.id != "ore_metal") && this.data.GetTotalCount("ore_metal") < this.data.inventory_size;
      case "mf_stones_1":
        return !(item.id != "stone") && this.data.GetTotalCount("stone") < this.data.inventory_size;
      case "mf_timber_1":
        return !(item.id != "wood") && this.data.GetTotalCount("wood") < this.data.inventory_size;
      case "tavern_outside_zombie_fence":
        return !(item.id != "working_zombie_pseudoitem_1") && this.data.GetTotalCount("working_zombie_pseudoitem_1") <= 0;
      case "witch_pylon":
        return item.id == "wood";
      case "working_table":
        return item.id == "wood";
      case "zombie_crafting_table":
        return !item.is_worker && item.definition.type == ItemDefinition.ItemType.Body && itemOfType == null;
    }
    return false;
  }

  public void DestroyMe()
  {
    if (this.is_removed)
      return;
    try
    {
      Debug.Log((object) ("WGO:DestroyMe " + ((UnityEngine.Object) this.gameObject == (UnityEngine.Object) null ? "NULL" : this.gameObject.name)), (UnityEngine.Object) this);
    }
    catch (MissingReferenceException ex)
    {
    }
    if (this.obj_def != null && this.obj_def.drop_inventory_after_remove && this.data != null && this.data.inventory != null && this.data.inventory.Count > 0)
    {
      foreach (Item obj in this.data.inventory)
      {
        if (obj != null && !string.IsNullOrEmpty(obj.id) && obj.value >= 1)
          this.DropItem(obj);
      }
    }
    this.components.craft.enabled = false;
    this.components.timer.enabled = false;
    this.components.hp.enabled = false;
    ChunkManager.OnDestroyObject(this);
    if (this._bubble != null)
    {
      InteractionBubbleGUI.RemoveBubble(this.unique_id);
      this._bubble = (BubbleWidgetDataContainer) null;
    }
    this.UnlinkWithSpawnerIfExists();
    this.is_removed = true;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    if (!this._was_ever_active)
      this.OnDestroy();
    if (!((UnityEngine.Object) this._zone != (UnityEngine.Object) null))
      return;
    this._zone.Recalculate();
  }

  public void UnlinkWithSpawnerIfExists()
  {
    BaseCharacterComponent character = this.components.character;
    if (character == null || !((UnityEngine.Object) character.spawner != (UnityEngine.Object) null) || character.spawner.spawned_mobs == null)
      return;
    if (character.spawner.spawned_mobs.Contains(this))
      character.spawner.spawned_mobs.Remove(this);
    character.spawner = (MobSpawner) null;
  }

  public bool IsInRange(WorldGameObject other_wgo, float range)
  {
    return (double) this.pos.GridDistTo(other_wgo.pos) < (double) range;
  }

  public bool IsInRange(GameObject other_go, float range)
  {
    return (double) this.pos.GridDistTo((Vector2) other_go.transform.position) < (double) range;
  }

  public void RestoreFromSerializedObject(SerializableWGO o, bool change_hierarchy = true)
  {
    if (string.IsNullOrEmpty(o.obj_id))
    {
      Debug.LogError((object) "Can't deserialize WGO with an empty id.", (UnityEngine.Object) this);
    }
    else
    {
      if (change_hierarchy)
        this.transform.SetParent(MainGame.me.world_root, false);
      o.ToWGO(this, out this._data);
      this.SetObject(o.obj_id);
      NetworkIdentity component = this.gameObject.GetComponent<NetworkIdentity>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) component);
      o.ToWGOAfterSetID(this);
      this.GetComponent<ChunkedGameObject>().OnJustSpawnedWGO();
      this.round_and_sort.DoUpdateStuff();
      this.name = "[wgo] " + o.obj_id;
      if (!Application.isPlaying)
        return;
      SaveGameFixer.UnstuckWGO(this);
    }
  }

  public void MarkForRemoval()
  {
    if (this.is_removing)
    {
      this.is_removing = false;
      if (this.components.craft.current_craft == BuildModeLogics.GetObjectRemoveCraftDefinition(this.obj_id))
      {
        this.components.craft.CancelRemovalCraft();
        this.RedrawBubble();
      }
    }
    else
    {
      ObjectCraftDefinition removeCraftDefinition = BuildModeLogics.GetObjectRemoveCraftDefinition(this.obj_id);
      if (removeCraftDefinition == null)
      {
        Debug.Log((object) $"Object id = {this.obj_id} has no removal craft");
        return;
      }
      if (removeCraftDefinition.IsLocked())
        return;
      this.is_removing = true;
      if (this.obj_id.EndsWith("_place", StringComparison.InvariantCulture))
      {
        ObjectCraftDefinition putCraftDefinition = BuildModeLogics.GetObjectPutCraftDefinition(this.obj_id);
        if (putCraftDefinition == null)
        {
          Debug.LogError((object) ("Error: Couldn't find obj craft = " + this.obj_id));
        }
        else
        {
          if (!string.IsNullOrEmpty(removeCraftDefinition.end_script))
            GS.RunFlowScript(removeCraftDefinition.end_script);
          if (putCraftDefinition.one_time_craft && MainGame.me.save.completed_one_time_crafts.Contains(putCraftDefinition.id))
            MainGame.me.save.completed_one_time_crafts.Remove(putCraftDefinition.id);
          this.DropItems(putCraftDefinition.needs);
          this.DestroyMe();
          return;
        }
      }
      else if (removeCraftDefinition.is_remove_without_hp_work)
      {
        if (!string.IsNullOrEmpty(removeCraftDefinition.end_script))
          GS.RunFlowScript(removeCraftDefinition.end_script);
        this.DropItems(removeCraftDefinition.output);
        if (removeCraftDefinition.is_destroy_worker_on_remove && (UnityEngine.Object) this.linked_worker != (UnityEngine.Object) null)
          this.linked_worker.DestroyMe();
        this.DestroyMe();
        return;
      }
      this.components.craft.StartRemovalCraft((CraftDefinition) removeCraftDefinition);
      this.RedrawBubble();
      InteractionBubbleGUI.GetBubble(this.unique_id).Activate<InteractionBubbleGUI>();
    }
    this.Redraw();
  }

  public bool is_removing
  {
    get => (double) this.data.GetParam("_removing") > 0.1;
    set => this.data.SetParam("_removing", value ? 1f : 0.0f);
  }

  public void TryStartCraft(string craft_name)
  {
    if (string.IsNullOrEmpty(craft_name))
      return;
    GJTimer.AddTimer(0.1f, (GJTimer.VoidDelegate) (() =>
    {
      CraftDefinition dataOrNull = GameBalance.me.GetDataOrNull<CraftDefinition>(craft_name);
      if (dataOrNull != null)
      {
        if (this.components == null)
          Debug.LogError((object) "Components is null!");
        else if (!this.components.craft.Craft(dataOrNull))
          Debug.LogError((object) "Failed to start craft!");
        else
          Debug.Log((object) ("Started craft: " + dataOrNull.id));
      }
      else
        Debug.LogError((object) $"Craft definition [{craft_name}] is null!");
    }));
  }

  public void OnEnable()
  {
    if (Application.isPlaying)
      CustomUpdateManager.wgos.Add(this);
    if (string.IsNullOrEmpty(this.cur_gd_point))
      return;
    GDPoint gdPointByGdTag = WorldMap.GetGDPointByGDTag(this.cur_gd_point);
    if (!((UnityEngine.Object) gdPointByGdTag != (UnityEngine.Object) null) || string.IsNullOrEmpty(gdPointByGdTag.smart_anim_trigger))
      return;
    this.TriggerSmartAnimation(gdPointByGdTag.smart_anim_trigger);
  }

  public void ResetAnimator() => this.components.animator.SetTrigger("reset");

  public void PreDisable()
  {
    ObjectDefinition objDef = this.obj_def;
    if ((objDef != null ? (objDef.type == ObjectDefinition.ObjType.Mob ? 1 : 0) : 0) == 0)
      return;
    GraphOwner[] componentsInChildren = this.GetComponentsInChildren<GraphOwner>();
    if (componentsInChildren == null || componentsInChildren.Length == 0)
      return;
    foreach (GraphOwner graphOwner in componentsInChildren)
      graphOwner?.StopBehaviour();
  }

  public void OnDisable()
  {
    if (!Application.isPlaying)
      return;
    CustomUpdateManager.wgos.Remove(this);
  }

  public void FireEvent(string event_id, float delay = 0.0f)
  {
    switch (event_id)
    {
      case "porter_on_came_to_destination":
        this.linked_workbench.porter_station.OnCameToDestination();
        break;
      case "porter_on_came_to_source":
        this.linked_workbench.porter_station.OnCameToSource();
        break;
    }
    if (this.is_removed)
      return;
    if (!this.gameObject.activeInHierarchy && delay.EqualsTo(0.0f))
      delay = 0.1f;
    if (delay.EqualsTo(0.0f))
    {
      if (!((UnityEngine.Object) this._fsc != (UnityEngine.Object) null))
        return;
      this._fsc.SendEvent(event_id);
    }
    else
    {
      this._events.event_delays.Add(delay);
      this._events.event_ids.Add(event_id);
      this.GetComponent<ChunkedGameObject>().active_now_because_of_events = true;
    }
  }

  public void FireEvent(string event_id, float delay, string param)
  {
    switch (event_id)
    {
      case "porter_on_came_to_destination":
        this.linked_workbench.porter_station.OnCameToDestination();
        break;
      case "porter_on_came_to_source":
        this.linked_workbench.porter_station.OnCameToSource();
        break;
    }
    if (this.is_removed)
      return;
    if (!this.gameObject.activeInHierarchy && delay.EqualsTo(0.0f))
      delay = 0.1f;
    if (delay.EqualsTo(0.0f))
    {
      if (!((UnityEngine.Object) this._fsc != (UnityEngine.Object) null))
        return;
      this._fsc.SendEvent<string>(event_id, param);
    }
    else
    {
      this._events.event_delays.Add(delay);
      this._events.event_ids.Add(event_id);
      this.GetComponent<ChunkedGameObject>().active_now_because_of_events = true;
    }
  }

  public bool ContainsSerializedEvent(string event_id)
  {
    foreach (string eventId in this._events.event_ids)
    {
      if (eventId == event_id)
        return true;
    }
    return false;
  }

  public static void InitAllWorldWGOs()
  {
    WorldGameObject[] componentsInChildren = MainGame.me.world.GetComponentsInChildren<WorldGameObject>(true);
    Debug.Log((object) ("InitAllWorldWGOs, count = " + componentsInChildren.Length.ToString()));
    foreach (WorldGameObject worldGameObject in componentsInChildren)
      worldGameObject.OnJustSpawned();
  }

  public void OnJustSpawned()
  {
    if (this.obj_def == null)
    {
      this.obj_def = GameBalance.me.GetData<ObjectDefinition>(this.obj_id);
      if (this.obj_def == null)
      {
        Debug.LogError((object) ("obj_def is null for id = " + this.obj_id));
        return;
      }
    }
    this.components.InitAllComponents();
    this.CheckNeededAttachedScript();
    this.RecalculateZoneBelonging();
    this._cached_pos = this.transform.position;
  }

  public void CheckNeededAttachedScript()
  {
    Blackboard blackboard = (Blackboard) null;
    foreach (BehaviourTreeOwner componentsInChild in this.GetComponentsInChildren<BehaviourTreeOwner>(true))
    {
      if (componentsInChild.blackboard == null)
      {
        if ((UnityEngine.Object) blackboard == (UnityEngine.Object) null)
          blackboard = this.gameObject.AddComponent<Blackboard>();
        componentsInChild.blackboard = (IBlackboard) blackboard;
      }
    }
    if (string.IsNullOrEmpty(this.obj_def.attached_script))
      return;
    FlowGraph graph = CustomFlowScript.GetGraph("WGO Scripts/" + this.obj_def.attached_script);
    if (!((UnityEngine.Object) graph != (UnityEngine.Object) null))
      return;
    bool flag = false;
    if ((UnityEngine.Object) this._fsc != (UnityEngine.Object) null)
    {
      this._fsc.PauseBehaviour();
      flag = true;
    }
    else
    {
      this._fsc = this.gameObject.AddComponent<FlowScriptController>();
      if ((UnityEngine.Object) blackboard == (UnityEngine.Object) null)
        blackboard = this.gameObject.AddComponent<Blackboard>();
      this._fsc.blackboard = (IBlackboard) blackboard;
      this._fsc.disableAction = GraphOwner.DisableAction.DoNothing;
    }
    this._fsc.graph = (Graph) graph;
    this._fsc.graph.blackboard = this._fsc.blackboard;
    if (!flag)
      return;
    this._fsc.StartBehaviour();
  }

  public void RecalculateZoneBelonging()
  {
    if (!this.obj_def.can_belong_to_zone)
      return;
    this._zone = WorldZone.GetZoneOfObject(this);
  }

  public int CanCollectDrop(DropResGameObject drop)
  {
    if (drop.res.is_tech_point || this.data.CanCollectItemAsDrop(drop.res))
      return drop.res.value;
    ItemDefinition definition = drop.res?.definition;
    return definition != null && definition.item_size == 1 && this.data.HasItemInInventory(definition.id) ? this.data.CanAddCount(definition.id, true) : 0;
  }

  public static WorldGameObject InstantiateWGOPrefab()
  {
    return SmartPooler.CreateObject<WorldGameObject>();
  }

  public WorldZone GetMyWorldZone()
  {
    if (this.is_removed)
    {
      Debug.LogError((object) ("ERROR: Trying to get world zone of a removed game object, id: " + this.obj_id));
      return (WorldZone) null;
    }
    if ((UnityEngine.Object) this.gameObject == (UnityEngine.Object) null)
    {
      Debug.LogError((object) ("ERROR: Trying to get world zone of a null game object, id: " + this.obj_id));
      return (WorldZone) null;
    }
    Collider2D[] collider2DArray = Physics2D.OverlapPointAll((Vector2) this.gameObject.transform.position, 524288 /*0x080000*/);
    if (collider2DArray == null || collider2DArray.Length == 0)
      return (WorldZone) null;
    foreach (Component component in collider2DArray)
    {
      WorldZone componentInParent = component.gameObject.GetComponentInParent<WorldZone>();
      if ((UnityEngine.Object) componentInParent != (UnityEngine.Object) null)
        return componentInParent;
    }
    return (WorldZone) null;
  }

  public bool HasSoulsTotemInZone()
  {
    WorldZone myWorldZone = this.GetMyWorldZone();
    return !((UnityEngine.Object) myWorldZone == (UnityEngine.Object) null) && myWorldZone.HasSoulsTotemInZone();
  }

  public string GetMyWorldZoneId()
  {
    WorldZone myWorldZone = this.GetMyWorldZone();
    return !((UnityEngine.Object) myWorldZone == (UnityEngine.Object) null) ? myWorldZone.id : "";
  }

  [ContextMenu("Apply Current Skin")]
  public void ApplyCurrentSkin()
  {
    if ((UnityEngine.Object) this.wop == (UnityEngine.Object) null)
      return;
    if (this._skin_changer == null)
      this._skin_changer = new SkinChanger(this);
    this._skin_changer.ApplySkin(string.IsNullOrEmpty(this.wop.skin_id) ? (SkinPreset) null : SkinPreset.Load(this.wop.skin_id));
  }

  public void ApplySkin(string skin_id)
  {
    if ((UnityEngine.Object) this.GetWOP() == (UnityEngine.Object) null)
      return;
    this.wop.skin_id = skin_id;
    this.ApplyCurrentSkin();
  }

  public Bounds GetTotalBounds()
  {
    Bounds? nullable = new Bounds?();
    foreach (Collider2D componentsInChild in this.GetComponentsInChildren<Collider2D>(true))
    {
      if (!nullable.HasValue)
        nullable = new Bounds?(componentsInChild.bounds);
      else
        nullable.Value.Encapsulate(componentsInChild.bounds);
    }
    return nullable.GetValueOrDefault();
  }

  public void AnimationEventAction()
  {
    if (!this.components.tool.enabled)
      return;
    this.components.tool.AnimationEventAction();
  }

  public void FailAnimationEventAction()
  {
    if (!this.components.tool.enabled)
      return;
    this.components.tool.FailAnimationEventAction();
  }

  public bool IsEnough(SmartRes res)
  {
    if (res == null)
    {
      Debug.LogError((object) "SmartRes is null in IsEnough()");
      return true;
    }
    switch (res.res_type)
    {
      case SmartRes.ResType.Empty:
        return true;
      case SmartRes.ResType.Item:
        return this.data.IsEnoughItems(res.item);
      case SmartRes.ResType.GameRes:
        return this.data.IsEnoughParam(res.res);
      default:
        Debug.LogError((object) ("IsEnough() is not supported for res type: " + res.res_type.ToString()), (UnityEngine.Object) this);
        return false;
    }
  }

  public bool IsEnough(GameRes res) => this.data.IsEnoughParams(res);

  public void RemoveSmartRes(SmartRes res)
  {
    if (res == null)
      return;
    switch (res.res_type)
    {
      case SmartRes.ResType.Empty:
        break;
      case SmartRes.ResType.Item:
        this.data.RemoveItem(res.item);
        break;
      case SmartRes.ResType.GameRes:
        GameResAtom res1 = res.res;
        this.data.SetParam(res1.type, this.data.GetParam(res1.type) - res1.value);
        break;
      default:
        Debug.LogError((object) ("RemoveSmartRes() is not supported for res type: " + res.res_type.ToString()), (UnityEngine.Object) this);
        break;
    }
  }

  public void ReceiveSmartRes(SmartRes res, WorldGameObject giver = null)
  {
    switch (res.res_type)
    {
      case SmartRes.ResType.Item:
        ((UnityEngine.Object) giver == (UnityEngine.Object) null ? MainGame.me.player : giver).DropItem(res.item);
        break;
      case SmartRes.ResType.GameRes:
        this.data.AddToParams(res.res);
        if (res.res.type == "money")
          DropCollectGUI.OnMoneyCollected(res.res.value);
        if (!res.res.type.Contains("_rel") || !((UnityEngine.Object) giver != (UnityEngine.Object) null))
          break;
        giver.ShowRelationChangeBubble((int) res.res.value);
        break;
      default:
        Debug.LogError((object) ("RemoveSmartRes() is not supported for res type: " + res.res_type.ToString()), (UnityEngine.Object) this);
        throw new ArgumentOutOfRangeException();
    }
  }

  public UniversalObjectInfo GetUniversalObjectInfo()
  {
    UniversalObjectInfo universalObjectInfo = new UniversalObjectInfo();
    universalObjectInfo.header = GJL.L(this.obj_def.id);
    universalObjectInfo.descr = "id: " + this.obj_def.id;
    if (this.is_autopsy_table)
    {
      if (this.GetBodyFromInventory() == null)
      {
        universalObjectInfo.header = GJL.L("hdr_autopsy_empty");
        universalObjectInfo.descr = "";
      }
      else
      {
        universalObjectInfo.header = GJL.L("hdr_autopsy_body");
        universalObjectInfo.descr = "";
      }
      universalObjectInfo.icon = string.IsNullOrEmpty(this.obj_def.custom_icon) ? "i_b_" + this.obj_def.id : this.obj_def.custom_icon;
      return universalObjectInfo;
    }
    switch (this.obj_def.interaction_type)
    {
      case ObjectDefinition.InteractionType.Craft:
        universalObjectInfo.descr = GJL.L("select_item_craft");
        universalObjectInfo.icon = string.IsNullOrEmpty(this.obj_def.custom_icon) ? "i_b_" + this.obj_def.id : this.obj_def.custom_icon;
        ObjectCraftDefinition putCraftDefinition = BuildModeLogics.GetObjectPutCraftDefinition(this.obj_def.id);
        if (putCraftDefinition != null)
          universalObjectInfo.icon = putCraftDefinition.icon;
        if (this.obj_def.additional_header_items.Count > 0)
        {
          using (List<string>.Enumerator enumerator = this.obj_def.additional_header_items.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              string current = enumerator.Current;
              universalObjectInfo.right_items.Add(current, this.data.GetItemsCount(current));
            }
            break;
          }
        }
        break;
      case ObjectDefinition.InteractionType.RunScript:
      case ObjectDefinition.InteractionType.Builder:
        WorldZone zoneById = WorldZone.GetZoneByID(this.obj_def.zone_id, false);
        universalObjectInfo.descr = (UnityEngine.Object) zoneById == (UnityEngine.Object) null || string.IsNullOrEmpty(zoneById.definition.gui_descr_str) ? "" : GJL.L(zoneById.definition.gui_descr_str, zoneById.GetQualityString());
        universalObjectInfo.icon = string.IsNullOrEmpty(this.obj_def.custom_icon) ? "i_z_" + this.obj_def.id : this.obj_def.custom_icon;
        break;
    }
    if (this.HasSoulsTotemInZone() && GlobalCraftControlGUI.is_global_control_active && !GlobalCraftControlGUI.current_instance.is_shown && !universalObjectInfo.right_items.ContainsKey("gratitude_as_item"))
      universalObjectInfo.right_items.Add("gratitude_as_item", (int) MainGame.me.player.gratitude_points);
    switch (this.obj_def.id)
    {
      case "grave_ground":
        Item bodyFromInventory = this.GetBodyFromInventory();
        universalObjectInfo.icon = "i_b_grave_place";
        if (bodyFromInventory == null)
        {
          universalObjectInfo.header = GJL.L("grave_empty_hdr");
          universalObjectInfo.descr = "";
          break;
        }
        universalObjectInfo.header = GJL.L("grave_body_hdr");
        universalObjectInfo.descr = "";
        break;
      case "mf_balsamation_1":
      case "mf_balsamation_2":
        universalObjectInfo.icon = "i_b_" + this.obj_def.id;
        universalObjectInfo.descr = "";
        break;
    }
    return universalObjectInfo;
  }

  public Item GetBodyFromInventory(bool first = true)
  {
    return this.data.GetItemOfType(ItemDefinition.ItemType.Body, first);
  }

  public bool CanSeeDarkness() => true;

  public string GetObjectConditionString(string separator = "")
  {
    return $"(hp){separator}{Item.FloatNumberToPercentString(1f - this.GetDecayFactor())}";
  }

  public float GetDecayFactor()
  {
    float decayFactor = this.GetParam("decay") / 100f;
    if ((double) decayFactor < 0.0)
      decayFactor = 0.0f;
    if ((double) decayFactor > 1.0)
      decayFactor = 1f;
    return decayFactor;
  }

  public bool has_removal_craft
  {
    get
    {
      if (!this._tried_to_find_removal_craft)
      {
        this._tried_to_find_removal_craft = true;
        ObjectCraftDefinition removeCraftDefinition = BuildModeLogics.GetObjectRemoveCraftDefinition(this.obj_id);
        this._has_removal_craft = removeCraftDefinition != null;
        if (removeCraftDefinition != null && removeCraftDefinition.IsLocked())
          this._has_removal_craft = false;
      }
      return this._has_removal_craft;
    }
  }

  public void ShowMark(WGOMark.MarkType mark_type)
  {
    if ((UnityEngine.Object) this._mark == (UnityEngine.Object) null)
    {
      this._mark = Prefabs.mark_prefab.Copy<WGOMark>(this.transform);
      this._mark.transform.localPosition = Vector3.zero;
    }
    if (this._mark_type != WGOMark.MarkType.None)
      this.RemoveMark();
    this._mark.gameObject.SetActive(true);
    this._mark_type = mark_type;
    this._mark.Draw(this._mark_type);
  }

  public void MarkObjectAsCanBeRemoved(GameObject group)
  {
    if (this._is_marked_removable)
      return;
    this._is_marked_removable = true;
    this._stored_parent_tf = this.transform.parent;
    this.transform.SetParent(group.transform);
    this.Redraw();
    foreach (SpriteRenderer componentsInChild in this.gameObject.GetComponentsInChildren<SpriteRenderer>(true))
    {
      WGOSpriteInCanBeRemovedMode canBeRemovedMode = componentsInChild.GetComponent<WGOSpriteInCanBeRemovedMode>();
      if ((UnityEngine.Object) canBeRemovedMode == (UnityEngine.Object) null)
        canBeRemovedMode = componentsInChild.gameObject.AddComponent<WGOSpriteInCanBeRemovedMode>();
      canBeRemovedMode.sorting_order = componentsInChild.sortingOrder;
      componentsInChild.sortingOrder = -Mathf.RoundToInt(componentsInChild.transform.position.z);
    }
  }

  public void CancelCanBeRemoved()
  {
    if (!this._is_marked_removable)
      return;
    this._is_marked_removable = false;
    this.transform.SetParent((UnityEngine.Object) this._stored_parent_tf != (UnityEngine.Object) null ? this._stored_parent_tf : MainGame.me.world_root);
    this.Redraw();
    foreach (WGOSpriteInCanBeRemovedMode componentsInChild in this.gameObject.GetComponentsInChildren<WGOSpriteInCanBeRemovedMode>(true))
    {
      componentsInChild.GetComponent<SpriteRenderer>().sortingOrder = componentsInChild.sorting_order;
      UnityEngine.Object.Destroy((UnityEngine.Object) componentsInChild);
    }
  }

  public void SetSortOverEverything()
  {
    this._is_sort_over_everything = true;
    this._stored_parent_tf = this.transform.parent;
    SortingGroup sortingGroup = this.content_tf.gameObject.AddComponent<SortingGroup>();
    sortingGroup.sortingLayerName = "over everything";
    sortingGroup.enabled = true;
    this.Redraw();
  }

  public void CancelSortOverEverything()
  {
    if (!this._is_sort_over_everything)
      return;
    this._is_sort_over_everything = false;
    SortingGroup component = this.content_tf.GetComponent<SortingGroup>();
    component.enabled = false;
    UnityEngine.Object.Destroy((UnityEngine.Object) component);
    this.transform.SetParent((UnityEngine.Object) this._stored_parent_tf != (UnityEngine.Object) null ? this._stored_parent_tf : MainGame.me.world_root);
    this.Redraw();
  }

  public void RemoveMark()
  {
    if (this._mark_type == WGOMark.MarkType.None)
      return;
    if ((UnityEngine.Object) this._mark != (UnityEngine.Object) null)
      this._mark.gameObject.SetActive(false);
    this._mark_type = WGOMark.MarkType.None;
  }

  public void OnBeganObjectModifications()
  {
    if (this._obj_modified_this_frame)
      return;
    this._obj_modified_this_frame = true;
    this._obj_quality = this.quality;
  }

  public void OnCameToGDPoint(GDPoint p)
  {
    if ((UnityEngine.Object) p == (UnityEngine.Object) null)
      return;
    this.components.character.idle_animation = p.idle_animation;
    if (this.obj_def.IsNPC())
      this.components.animator.ResetTrigger("any_custom_loop_out");
    if (p.direction != Direction.None)
      this.components.character.LookAt(p.direction);
    if (!string.IsNullOrEmpty(p.smart_anim_trigger))
      this.TriggerSmartAnimation(p.smart_anim_trigger);
    this.cur_gd_point = p.gd_tag;
  }

  public void WGOLog(string s)
  {
  }

  public bool CheckDisabledInteractions()
  {
    bool flag = this.GetParamInt("disabled_interactions") != 0;
    if (this.obj_def != null && this.obj_def.IsNPC() && this.components.character.astar != null && this.components.character.astar.finding)
      flag = true;
    if (flag)
      MainGame.me.player.components.character.ShowDisabledInteractionBubble(this);
    return flag;
  }

  public BubbleWidgetTextData GetQualityWidgetData()
  {
    if (this.obj_def == null)
    {
      Debug.LogError((object) "GetQualityWidgetData: obj_def is null", (UnityEngine.Object) this);
      return (BubbleWidgetTextData) null;
    }
    try
    {
      if (this.obj_def.quality_type != ObjectDefinition.QualityType.Hidden && !((UnityEngine.Object) this._zone == (UnityEngine.Object) null))
      {
        if (this.show_quality_hint)
          goto label_6;
      }
      return (BubbleWidgetTextData) null;
    }
    catch (NullReferenceException ex)
    {
      Debug.LogError((object) ex, (UnityEngine.Object) this);
      return (BubbleWidgetTextData) null;
    }
label_6:
    int num1 = MainGame.me.gui_elements.build_mode_gui.is_shown ? 1 : 0;
    float quality = this.quality;
    int num2 = !this.quality_k.EqualsTo(0.0f) ? 1 : 0;
    int num3 = num1 & num2;
    if (num3 != 0)
      quality /= this.quality_k;
    string text = (!this.obj_def.has_overrode_quality_icon ? this._zone.definition.quality_icon : this.obj_def.overrode_quality_icon) + quality.ToString("0.#");
    if (num3 != 0 && !this.quality_k.EqualsTo(1f))
      text = $"{text}\n{("×" + this.quality_k.ToString("0.#")).ColorizeText(InteractionBubbleGUI.quality_k_color)}";
    if (num3 != 0 && !this.totem_effect.IsEmpty())
    {
      string str = this.totem_effect.ToPrintableString(force_parentheses: true, float_format: true, skip: new List<string>()
      {
        "quality_k"
      }).Replace("(quality)", this._zone.definition.quality_icon);
      if (!string.IsNullOrEmpty(str))
        text = $"{text}\n+{str}";
    }
    if (text.Contains("(wskull)"))
    {
      text = text.Replace("(wskull)", "(wskull)\n");
      if ((double) quality < 0.0)
        text += " ";
    }
    BubbleWidgetTextData qualityWidgetData = new BubbleWidgetTextData(text, UITextStyles.TextStyle.QualityHint);
    qualityWidgetData.widget_id = BubbleWidgetData.WidgetID.Quality;
    return qualityWidgetData;
  }

  public BubbleWidgetTextData SetBubbleWidgetData(string text, BubbleWidgetData.WidgetID widget_id)
  {
    if (string.IsNullOrEmpty(text))
    {
      this.SetBubbleWidgetData((BubbleWidgetData) null, widget_id);
      return (BubbleWidgetTextData) null;
    }
    BubbleWidgetTextData wdata = new BubbleWidgetTextData(text, UITextStyles.TextStyle.InteractionHint);
    this.SetBubbleWidgetData((BubbleWidgetData) wdata, widget_id);
    return wdata;
  }

  public void SetBubbleWidgetData(BubbleWidgetData wdata, BubbleWidgetData.WidgetID widget_id)
  {
    this.bubble.SetWidgetDataWithID(wdata, widget_id);
    this.bubble.Redraw();
  }

  public void DrawFishingPullBubble(string hint)
  {
    this.bubble.ClearData();
    if (!string.IsNullOrEmpty(hint))
      this.SetBubbleWidgetData(GameKeyTip.Get(GameKey.Interaction, GJL.L(hint)), BubbleWidgetData.WidgetID.Interaction);
    this.bubble.Redraw();
    if (string.IsNullOrEmpty(hint))
      return;
    this.bubble.GetBubbleGUI().MakeBottomAligned();
  }

  public void RedrawBubble(bool? show_interaction_buttons = null)
  {
    this.components.RefreshBubblesData(show_interaction_buttons);
    if (GUIElements.me.IsAnyMassiveWindowOpened())
      this.bubble.SetWidgetDataWithID((BubbleWidgetData) null, BubbleWidgetData.WidgetID.Interaction);
    this.bubble.Redraw();
    InteractionBubbleGUI bubbleGui = this.bubble.GetBubbleGUI();
    if (!((UnityEngine.Object) bubbleGui != (UnityEngine.Object) null))
      return;
    bubbleGui.RefreshAlign(this);
  }

  public void SetQualityHint(bool show_quality)
  {
    this.show_quality_hint = show_quality;
    this.RedrawBubble();
  }

  public bool CanProcessWork()
  {
    return !this.components.craft.enabled || this.components.craft.is_crafting || !this.components.craft.IsCraftQueueEmpty();
  }

  public void RecalculateGridShape()
  {
    if (this.is_removed)
      return;
    foreach (OptimizedCollider2D componentsInChild in this.GetComponentsInChildren<OptimizedCollider2D>(true))
      componentsInChild.Init();
    Bounds totalBounds = this.GetTotalBounds();
    totalBounds.Expand((Vector3) (Vector2.one * 32f));
    List<Collider2D> target_colliders = new List<Collider2D>();
    foreach (Collider2D componentsInChild in this.GetComponentsInChildren<Collider2D>(true))
    {
      if (!BuildGrid.SkipCollider(componentsInChild, this))
        target_colliders.Add(componentsInChild);
    }
    this._cells.Clear();
    this._cells_totem_local.Clear();
    for (int index1 = Mathf.FloorToInt(totalBounds.min.x / 32f); index1 <= Mathf.CeilToInt(totalBounds.max.x / 32f); ++index1)
    {
      for (int index2 = Mathf.FloorToInt(totalBounds.min.y / 32f); index2 <= Mathf.CeilToInt(totalBounds.max.y / 32f); ++index2)
      {
        if (BuildGrid.IsCellBusy(new Vector2((float) index1, (float) index2) * 32f, target_colliders))
          this._cells.Add(new IntVector2(index1, index2));
      }
    }
    if (!this.obj_def.IsTotem())
      return;
    float num = this.obj_def.totem_radius * this.obj_def.totem_radius;
    Vector2 centerInLocalCoords = this.GetTotemCenterInLocalCoords();
    for (int index3 = -Mathf.CeilToInt(this.obj_def.totem_radius + 1f); (double) index3 < (double) this.obj_def.totem_radius + 1.0; ++index3)
    {
      for (int index4 = -Mathf.CeilToInt(this.obj_def.totem_radius + 1f); (double) index4 < (double) this.obj_def.totem_radius + 1.0; ++index4)
      {
        if ((double) (new Vector2((float) index3, (float) index4) - centerInLocalCoords).sqrMagnitude <= (double) num)
          this._cells_totem_local.Add(new IntVector2(index3, index4));
      }
    }
  }

  public Vector2 GetTotemCenterInLocalCoords() => Vector2.zero;

  public bool DoesIncludeGridPos(int x, int y)
  {
    foreach (IntVector2 cell in this._cells)
    {
      if (cell.x == x && cell.y == y)
        return true;
    }
    return false;
  }

  public bool HasTotemInfluenceOnWGO(WorldGameObject o)
  {
    if (!this.obj_def.IsTotem())
    {
      Debug.LogError((object) "HasTotemInfluenceOnWGO must be called only on a totem");
      return false;
    }
    int num1 = Mathf.RoundToInt(this.pos.x / 32f);
    int num2 = Mathf.RoundToInt(this.pos.y / 32f);
    foreach (IntVector2 intVector2 in this._cells_totem_local)
    {
      if (o.DoesIncludeGridPos(intVector2.x + num1, intVector2.y + num2))
        return true;
    }
    return false;
  }

  public void ProcessRemove()
  {
    InteractionBubbleGUI.RemoveBubble(this.unique_id, true);
    this.DestroyMe();
  }

  public override string ToString()
  {
    return $"[WGO name={this.gameObject.name} obj_id={this.obj_id} instance_id={this.gameObject.GetInstanceID()}]";
  }

  public void ForceInitOptimizedColliders()
  {
    foreach (OptimizedCollider2D componentsInChild in this.gameObject.GetComponentsInChildren<OptimizedCollider2D>(true))
      componentsInChild.Init();
  }

  public void AdditionalDeserialize(ref SerializableWGO d)
  {
    this._events = d.events_as_class == null ? new WorldGameObject.SerializableEvents() : d.events_as_class;
  }

  public void AdditionalSerialize(ref SerializableWGO d) => d.events_as_class = this._events;

  public void AddInteractionEvent(string event_id)
  {
    if (this.custom_interaction_events == null)
      this.custom_interaction_events = new List<string>();
    if (this.custom_interaction_events.Contains(event_id))
      Debug.LogWarning((object) $"AddInteractionEvent: Trying to add existing event {event_id} to WGO {this.name}", (UnityEngine.Object) this);
    else
      this.custom_interaction_events.Add(event_id);
    this.RedrawBubble();
  }

  public UnityEngine.Sprite GetHeadSprite()
  {
    UnityEngine.Sprite headSprite = (UnityEngine.Sprite) null;
    ObjectDefinition objectDefinition = string.IsNullOrEmpty(this.obj_def.npc_alias) ? this.obj_def : GameBalance.me.GetData<ObjectDefinition>(this.obj_def.npc_alias);
    if (!string.IsNullOrEmpty(objectDefinition.custom_head_spr))
      return EasySpritesCollection.GetSprite(objectDefinition.custom_head_spr);
    if (this.obj_def.IsCharacter())
    {
      SkinPreset skinPreset = string.IsNullOrEmpty(this.wop.skin_id) ? (SkinPreset) null : SkinPreset.Load(this.wop.skin_id);
      if ((UnityEngine.Object) skinPreset != (UnityEngine.Object) null)
        headSprite = EasySpritesCollection.GetSprite(skinPreset.head.ToString("0##") + "_hed_down");
    }
    return headSprite;
  }

  public static int GetRelation(string obj_id)
  {
    string str = obj_id;
    ObjectDefinition dataOrNull = GameBalance.me.GetDataOrNull<ObjectDefinition>(obj_id);
    if (dataOrNull != null && !string.IsNullOrEmpty(dataOrNull.npc_alias))
      str = dataOrNull.npc_alias;
    string param_name = "_rel_" + str;
    int relation = MainGame.me.player.GetParamInt(param_name);
    if (relation < 0)
    {
      relation = 0;
      MainGame.me.player.SetParam(param_name, (float) relation);
    }
    if (relation > 100)
    {
      relation = 100;
      MainGame.me.player.SetParam(param_name, (float) relation);
    }
    return relation;
  }

  public GDPoint GetParentGDPoint()
  {
    if (!this._parent_gd_point_inited)
    {
      this._parent_gd_point_inited = true;
      GDPoint[] componentsInParent = this.gameObject.GetComponentsInParent<GDPoint>(true);
      this._parent_gd_point = componentsInParent.Length != 0 ? componentsInParent[0] : (GDPoint) null;
    }
    return this._parent_gd_point;
  }

  public bool IsDisabled()
  {
    GDPoint parentGdPoint = this.GetParentGDPoint();
    return (UnityEngine.Object) parentGdPoint != (UnityEngine.Object) null && !parentGdPoint.gameObject.activeSelf;
  }

  public void TeleportToGDPoint(string gd_point_tag, bool dont_move_camera_while_tp = false)
  {
    GDPoint gdPointByGdTag = WorldMap.GetGDPointByGDTag(gd_point_tag);
    if ((UnityEngine.Object) gdPointByGdTag == (UnityEngine.Object) null)
    {
      Debug.LogError((object) ("Can't find GD point: " + gd_point_tag));
    }
    else
    {
      Debug.Log((object) $"Teleporting {this.name} to GD point: {gdPointByGdTag.name}", (UnityEngine.Object) gdPointByGdTag.gameObject);
      this.transform.position = gdPointByGdTag.transform.position;
      if (this.is_player)
      {
        if (!dont_move_camera_while_tp)
          CameraTools.MoveToPos((Vector2) gdPointByGdTag.transform.position);
        GameAwakenerEngine.OnPlayerMoved();
      }
      else
        this.OnCameToGDPoint(gdPointByGdTag);
      this.RefreshPositionCache();
      this.GetComponent<ChunkedGameObject>().RecalculateChunk();
    }
  }

  public void ShowRelationChangeBubble(int delta)
  {
    Debug.Log((object) $"ShowRelationChangeBubble {delta.ToString()} of obj {this.obj_id}", (UnityEngine.Object) this);
    EffectBubblesManager.ShowImmediately(this.bubble_pos, HUDRelationBubble.GetRelationChangeString(delta), EffectBubblesManager.BubbleColor.Relation, custom_time: 1f);
    GUIElements.me.relation.OnShownRelationBubble(this);
  }

  public void TriggerSmartAnimation(string smart_anim_trigger)
  {
    if (string.IsNullOrEmpty(smart_anim_trigger))
      return;
    SmartAnimationController componentInChildren = this.GetComponentInChildren<SmartAnimationController>();
    if ((UnityEngine.Object) componentInChildren == (UnityEngine.Object) null)
      Debug.LogError((object) $"Can't trigger animation {smart_anim_trigger} because SmartAnimationController is not found", (UnityEngine.Object) this);
    else
      componentInChildren.TriggerAimation(smart_anim_trigger);
  }

  public void TriggerSmartAnimation(
    string smart_anim_trigger,
    System.Action on_anim_finished,
    float workaround_time = 1f)
  {
    if ((UnityEngine.Object) this.GetComponent<StateAnimationListener>() != (UnityEngine.Object) null)
      Debug.LogError((object) $"WGO already has a StateAnimationListener, anim \"{smart_anim_trigger}\" for object \"{this.name}\"", (UnityEngine.Object) this);
    StateAnimationListener animationListener = this.gameObject.AddComponent<StateAnimationListener>();
    animationListener.AddWorkaroundTimer(workaround_time, $"Forcing workaround anim \"{smart_anim_trigger}\" for object \"{this.name}\"");
    animationListener.on_exit = on_anim_finished;
    this.TriggerSmartAnimation(smart_anim_trigger);
  }

  public void DropStory(float bronze, float silver, float gold)
  {
    float num1 = bronze + silver + gold;
    if (num1.EqualsTo(0.0f))
      return;
    float num2 = UnityEngine.Random.Range(0.0f, num1);
    string item_id = "story:1";
    if ((double) num2 < (double) gold)
      item_id = "story:3";
    else if ((double) num2 < (double) gold + (double) silver)
      item_id = "story:2";
    this.DropItem(new Item(item_id, 1), Direction.ToPlayer);
  }

  public void GiveItemToPlayersHands(Item item)
  {
    this.data.RemoveItem(item);
    BaseCharacterComponent character = MainGame.me.player.components.character;
    if (character.has_overhead)
      character.DropOverheadItem();
    character.SetOverheadItem(item);
    this.Redraw();
    SmartDrawer componentInChildren = this.GetComponentInChildren<SmartDrawer>();
    if ((UnityEngine.Object) componentInChildren != (UnityEngine.Object) null)
      componentInChildren.Redraw(true);
    if (item.is_worker)
      item.worker.UpdateWorkerInventoryFromItem(item);
    Sounds.PlaySound("item_2h_drop");
  }

  public bool CheckIfDisabledInTutorial()
  {
    if (this.obj_def.interactive_in_tutorial || !MainGame.me.save.IsInTutorial())
      return false;
    if (!this._shown_tutorial_disabled)
    {
      MainGame.me.player.Say("disabled_interactions", (GJCommons.VoidDelegate) (() => this._shown_tutorial_disabled = false), type: SpeechBubbleGUI.SpeechBubbleType.Think);
      this._shown_tutorial_disabled = true;
    }
    return true;
  }

  public bool IsMoving()
  {
    return this.obj_def != null && this.obj_def.IsNPC() && this.components.character.IsInMovingState();
  }

  public bool CanPutToAllPossibleInventories(List<Item> items_to_put, out List<Item> can_not_put)
  {
    if (items_to_put == null || items_to_put.Count == 0)
    {
      can_not_put = new List<Item>();
      return true;
    }
    can_not_put = new List<Item>();
    foreach (Item obj in items_to_put)
      can_not_put.Add(new Item(obj));
    WorldZone myWorldZone = this.GetMyWorldZone();
    if ((UnityEngine.Object) myWorldZone != (UnityEngine.Object) null)
    {
      List<WorldGameObject> worldGameObjectList = myWorldZone.GetZoneWGOs();
      if (this.obj_id == "tavern_kitchen" || this.obj_id == "tavern_oven")
      {
        WorldGameObject gameObjectByObjId = WorldMap.GetWorldGameObjectByObjId("npc_tavern_barman");
        if ((UnityEngine.Object) gameObjectByObjId == (UnityEngine.Object) null)
          Debug.LogError((object) "Can not put tavern_kitchen output to barmen: not found barmen WGO! Call Bulat.");
        else
          worldGameObjectList = new List<WorldGameObject>()
          {
            gameObjectByObjId
          };
      }
      foreach (WorldGameObject worldGameObject in worldGameObjectList)
      {
        if (!((UnityEngine.Object) worldGameObject == (UnityEngine.Object) null))
        {
          Item obj1 = worldGameObject.data.MakeInventoryCopy();
          ObjectDefinition objDef = worldGameObject.obj_def;
          if (objDef == null)
            Debug.LogError((object) $"Not found object definition for WGO \"{worldGameObject.name}\", obj_def={worldGameObject.obj_id}");
          else if (objDef.open_in_multiinventory)
          {
            bool flag = objDef.can_insert_items != null && objDef.can_insert_items.Count > 0;
            for (int index = 0; index < can_not_put.Count; ++index)
            {
              Item obj2 = can_not_put[index];
              if (!flag && !obj2.definition.is_big || objDef.can_insert_items != null && objDef.can_insert_items.Contains(obj2.id) && (objDef.can_insert_items_limit == 0 || objDef.can_insert_items_limit > obj1.GetItemsCount(obj2.id)))
              {
                int b = obj1.CanAddCount(obj2.id, true);
                if (b > 0)
                {
                  int num = obj2.value - b;
                  obj1.AddItem(obj2.id, Mathf.Min(obj2.value, b));
                  if (num > 0)
                  {
                    obj2.value = num;
                  }
                  else
                  {
                    can_not_put.RemoveAt(index);
                    --index;
                  }
                }
              }
            }
          }
        }
      }
    }
    if (can_not_put.Count == 0)
      return true;
    foreach (Item obj in can_not_put)
    {
      if (!obj.IsEmpty() && obj.value != 0)
        return false;
    }
    return true;
  }

  public void PutToAllPossibleInventories(List<Item> drop_list, out List<Item> cant_insert)
  {
    WorldZone myWorldZone = this.GetMyWorldZone();
    if (this.obj_id == "tavern_kitchen" || this.obj_id == "tavern_oven")
    {
      WorldGameObject gameObjectByObjId = WorldMap.GetWorldGameObjectByObjId("npc_tavern_barman");
      if ((UnityEngine.Object) gameObjectByObjId == (UnityEngine.Object) null)
      {
        Debug.LogError((object) "Can not put tavern_kitchen output to barmen: not found barmen WGO! Call Bulat. #1");
        cant_insert = drop_list;
      }
      else
      {
        int count1 = drop_list.Count;
        gameObjectByObjId.TryPutToInventory(drop_list, out cant_insert);
        int count2 = cant_insert.Count;
        if (count1 <= count2)
          return;
        this.SetParam("do_roll_anim", 1f);
      }
    }
    else if ((UnityEngine.Object) myWorldZone != (UnityEngine.Object) null)
      myWorldZone.PutToAllPossibleInventoriesSmart(drop_list, out cant_insert);
    else
      cant_insert = drop_list;
  }

  public void TryPutToInventory(List<Item> items_to_insert, out List<Item> cant_insert)
  {
    for (int index = 0; index < items_to_insert.Count; ++index)
    {
      Item obj = items_to_insert[index];
      if (obj.IsEmpty())
      {
        items_to_insert.RemoveAt(index);
        --index;
      }
      else if (this.is_player || this.obj_def.can_insert_items.Contains(obj.id) && (this.obj_def.can_insert_items_limit == 0 || this.obj_def.can_insert_items_limit > obj.value))
      {
        int num1 = this.data.CanAddCount(obj.id, true);
        if (num1 > 0)
        {
          int num2 = obj.value - num1;
          if (num2 > 0)
          {
            this.data.AddItem(new Item(obj) { value = num1 });
            obj.value = num2;
          }
          else
          {
            this.data.AddItem(obj);
            items_to_insert.RemoveAt(index);
            --index;
          }
        }
      }
    }
    cant_insert = items_to_insert;
  }

  public DockPoint GetAvailableDockPointForZombie()
  {
    if (this._dock_points == null || this._dock_points.Length == 0)
      this.RefindDockPoints();
    if (this._dock_points == null || this._dock_points.Length == 0)
    {
      if (MainGame.game_started)
        Debug.LogError((object) $"Not found any dock point at wgo \"{this.name}\", obj_id={this.obj_id}");
      return (DockPoint) null;
    }
    List<DockPoint> dockPointList1 = new List<DockPoint>();
    List<DockPoint> dockPointList2 = new List<DockPoint>();
    foreach (DockPoint dockPoint in this._dock_points)
    {
      if (!((UnityEngine.Object) dockPoint == (UnityEngine.Object) null) && !((UnityEngine.Object) dockPoint.tf == (UnityEngine.Object) null) && !((UnityEngine.Object) dockPoint.gameObject == (UnityEngine.Object) null) && dockPoint.gameObject.activeInHierarchy && dockPoint.can_place_worker)
      {
        dockPointList2.Add(dockPoint);
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll((Vector2) dockPoint.tf.position, 19.2f, 1);
        if (collider2DArray != null && collider2DArray.Length != 0)
        {
          bool flag = false;
          foreach (Component component in collider2DArray)
          {
            WorldGameObject componentInParent = component.GetComponentInParent<WorldGameObject>();
            if ((!((UnityEngine.Object) componentInParent != (UnityEngine.Object) null) || componentInParent.is_player) && (!((UnityEngine.Object) componentInParent != (UnityEngine.Object) null) || !((UnityEngine.Object) componentInParent == (UnityEngine.Object) this)))
            {
              flag = true;
              break;
            }
          }
          if (flag)
            continue;
        }
        dockPointList1.Add(dockPoint);
      }
    }
    if (this.obj_def.type == ObjectDefinition.ObjType.PorterStation && dockPointList2.Count > 0)
      return dockPointList2[0];
    if (dockPointList1.Count == 0)
      return (DockPoint) null;
    if (dockPointList1.Count == 1)
      return dockPointList1[0];
    DockPoint dockPointForZombie = dockPointList1[0];
    Vector2 pos = MainGame.me.player.pos;
    float num = (pos - (Vector2) dockPointForZombie.tf.position).magnitude;
    for (int index = 1; index < dockPointList1.Count; ++index)
    {
      float magnitude = ((Vector2) dockPointList1[index].tf.position - pos).magnitude;
      if ((double) num > (double) magnitude)
      {
        num = magnitude;
        dockPointForZombie = dockPointList1[index];
      }
    }
    return dockPointForZombie;
  }

  public DockPoint[] RefindDockPointsAndGet()
  {
    this.RefindDockPoints();
    return this._dock_points;
  }

  public bool player_cant_work => !this.is_removing && this.obj_def.player_cant_work;

  public int GetItemInsertionCoeff(Item item) => this.data.GetItemsCount(item.id);

  public string GetCraftAmountCounter(CraftDefinition craft_definition, int amount = 1)
  {
    List<Item> objList = ResModificator.ProcessItemsListBeforeDrop(craft_definition.output, this, MainGame.me.player);
    int num1 = 0;
    if (objList.Count > 0)
    {
      Item obj = objList[0];
      int num2 = Mathf.RoundToInt(obj.min_value.EvaluateFloat(this, MainGame.me.player));
      int num3 = Mathf.RoundToInt(obj.max_value.EvaluateFloat(this, MainGame.me.player));
      if (num2 < 0)
        num2 = 0;
      num1 = num3 >= num2 ? (num2 + num3) / 2 : num2;
    }
    if (craft_definition.output_to_wgo.Count > 0 && craft_definition.output_to_wgo[0].id == "fire")
      num1 = craft_definition.output_to_wgo[0].value;
    int num4 = num1 * amount;
    return num4 > 1 ? num4.ToString() : "";
  }

  public void ForceRedrawInSmartDrawer()
  {
    foreach (SmartDrawer componentsInChild in this.GetComponentsInChildren<SmartDrawer>())
      componentsInChild.Redraw(true);
  }

  public void RedrawGroundSprites()
  {
    if (this.is_dead)
      return;
    try
    {
      if ((UnityEngine.Object) this.gameObject == (UnityEngine.Object) null)
        return;
    }
    catch (NullReferenceException ex)
    {
      return;
    }
    bool flag = this.components.character.enabled || this.just_built;
    GroundObject[] componentsInChildren = this.GetComponentsInChildren<GroundObject>(true);
    for (int index = 0; index < componentsInChildren.Length; ++index)
    {
      componentsInChildren[index].can_move = flag;
      if (this.just_built && !this.components.character.enabled)
        componentsInChildren[index].can_move = false;
    }
  }

  public bool IsPlayerInvulnerable()
  {
    return this.is_player && MainGame.me.player.GetParamInt("is_invulnerable") == 1;
  }

  [CompilerGenerated]
  public void \u003CAnimatorExitAnyCustomLoop\u003Eb__191_0()
  {
    this.components.animator.ResetTrigger("any_custom_loop_out");
  }

  [CompilerGenerated]
  public void \u003CCheckIfDisabledInTutorial\u003Eb__346_0()
  {
    this._shown_tutorial_disabled = false;
  }

  public enum CraftState
  {
    None,
    CraftingWithoutPlayer,
    CraftingAndWorking,
  }

  [Serializable]
  public class SerializableEvents
  {
    public List<float> event_delays = new List<float>();
    public List<string> event_ids = new List<string>();
  }
}
