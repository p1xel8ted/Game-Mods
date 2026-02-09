// Decompiled with JetBrains decompiler
// Type: BaseCharacterComponent
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class BaseCharacterComponent : MovementComponent
{
  public const string GLOBAL_STATE = "global_state";
  public const string DIRECTION_X = "direction_x";
  public const string DIRECTION_Y = "direction_y";
  public const string DIRECTION_ANGLE = "direction_angle";
  public const string SUB_STATE = "sub_state";
  public const string WAS_DAMAGED = "was_damaged";
  public const string WAS_DAMAGED_DIRECTION = "was_damaged_direction";
  public const string WORK_TOOL_NUM = "work_tool_num";
  public const string DIAGONAL_DIR_ANGLE = "diagonal_direction_angle";
  public const float ANIM_EPS = 0.1f;
  public const float AUTO_DOCK_DIST = 144f;
  public const float PLAYER_RADIUS = 15.36f;
  public const float RE_ACTION = 0.25f;
  public const int DOCKS_LAYER = 11;
  public const int DOCKS_REFIND_FREQUENCY = 1;
  [SerializeField]
  public bool _control_enabled;
  [CompilerGenerated]
  public bool \u003Ccan_be_locally_controlled\u003Ek__BackingField;
  [Space]
  [SerializeField]
  public Item overhead_item;
  [HideInInspector]
  public Vector2 direction = Vector2.down;
  public bool auto_do_action;
  public WorldGameObject _docked_obj;
  public Camera _cam;
  public Transform _content_tf;
  public Vector3 _content_local_pos;
  public Direction _anim_direction = Direction.Right;
  public CharAnimState _anim_state;
  public int _global_state;
  public bool _needed_tool_bubble_shown;
  public Vector2 _last_anim_vec_dir;
  [NonSerialized]
  public List<WorldGameObject> chests_in_area = new List<WorldGameObject>();
  public float _anim_dir_angle;
  public float _dir_angle;
  public DockPoint _current_dp;
  public Vector2 spawner_coords;
  public MobSpawner spawner;
  public GameObject _anchor_obj;
  public bool anchor_is_wgo;
  public string anchor_obj_gd_point_tag = "";
  public string anchor_obj_wgo_custom_tag = "";
  public DockPoint _nearest_dock_point;
  public float _last_pressed_attack_time = -1f;
  [SerializeField]
  [RuntimeValue]
  public bool _was_damaged;
  [SerializeField]
  [RuntimeValue]
  public bool _ignore_was_damaged;
  public SpriteRenderer[] _sprs;
  public CharacterSkin skin = new CharacterSkin();
  public bool _disabled_interaction_shown;
  [NonSerialized]
  public bool idle_used;
  public BaseCharacterComponent.Environment cur_environment;
  [NonSerialized]
  public WorldGameObject wgo_hilighted_for_work;
  public bool _deserialize_idle_on_use;
  public BaseCharacterIdle.SerializableCharacterIdle _serialized_idle_data;
  [NonSerialized]
  public bool dont_work_anymore;
  public bool _attack_cached;
  public BaseCharacterAttack _attack;
  public bool _idle_cached;
  public BaseCharacterIdle _idle;

  public bool can_be_locally_controlled
  {
    set => this.\u003Ccan_be_locally_controlled\u003Ek__BackingField = value;
    get => this.\u003Ccan_be_locally_controlled\u003Ek__BackingField;
  }

  public GameObject anchor_obj
  {
    get
    {
      if ((UnityEngine.Object) this._anchor_obj == (UnityEngine.Object) null)
      {
        if (!this.anchor_is_wgo && !string.IsNullOrEmpty(this.anchor_obj_gd_point_tag))
        {
          GDPoint gdPointByGdTag = WorldMap.GetGDPointByGDTag(this.anchor_obj_gd_point_tag);
          this._anchor_obj = (UnityEngine.Object) gdPointByGdTag == (UnityEngine.Object) null ? (GameObject) null : gdPointByGdTag.gameObject;
        }
        else if (this.anchor_is_wgo && !string.IsNullOrEmpty(this.anchor_obj_wgo_custom_tag))
          this._anchor_obj = WorldMap.GetWorldGameObjectByCustomTag(this.anchor_obj_wgo_custom_tag).gameObject;
      }
      return this._anchor_obj;
    }
  }

  public void SetAnchor(GDPoint anchor_gd_point)
  {
    if ((UnityEngine.Object) anchor_gd_point == (UnityEngine.Object) null)
    {
      this._anchor_obj = (GameObject) null;
      this.anchor_obj_gd_point_tag = "";
      this.anchor_obj_wgo_custom_tag = "";
    }
    else if (string.IsNullOrEmpty(anchor_gd_point.gd_tag))
    {
      Debug.LogError((object) $"GD Tag of GDPoint {anchor_gd_point.name} is null!", (UnityEngine.Object) anchor_gd_point);
    }
    else
    {
      this._anchor_obj = anchor_gd_point.gameObject;
      this.anchor_obj_gd_point_tag = anchor_gd_point.gd_tag;
      this.anchor_obj_wgo_custom_tag = "";
      this.anchor_is_wgo = false;
    }
  }

  public void SetAnchor(WorldGameObject anchor_wgo)
  {
    if ((UnityEngine.Object) anchor_wgo == (UnityEngine.Object) null)
    {
      this._anchor_obj = (GameObject) null;
      this.anchor_obj_gd_point_tag = "";
      this.anchor_obj_wgo_custom_tag = "";
    }
    else if (string.IsNullOrEmpty(anchor_wgo.custom_tag))
    {
      Debug.LogError((object) $"Custom tag of WGO {anchor_wgo.name} is null!", (UnityEngine.Object) anchor_wgo);
    }
    else
    {
      this._anchor_obj = anchor_wgo.gameObject;
      this.anchor_obj_wgo_custom_tag = anchor_wgo.custom_tag;
      this.anchor_obj_gd_point_tag = "";
      this.anchor_is_wgo = true;
    }
  }

  public void SetAnchor(GameObject anchor_go)
  {
    if ((UnityEngine.Object) anchor_go == (UnityEngine.Object) null)
    {
      this._anchor_obj = (GameObject) null;
      this.anchor_obj_gd_point_tag = "";
      this.anchor_obj_wgo_custom_tag = "";
    }
    else
    {
      GDPoint component1 = anchor_go.GetComponent<GDPoint>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      {
        this.SetAnchor(component1);
      }
      else
      {
        WorldGameObject component2 = anchor_go.GetComponent<WorldGameObject>();
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
          this.SetAnchor(component2);
        else
          Debug.LogError((object) $"Can not set anchor: {anchor_go.name} is not GDPoint or WGO!", (UnityEngine.Object) anchor_go);
      }
    }
  }

  public bool playing_work_animation
  {
    get
    {
      return this.components.tool.enabled && this.components.tool.playing_animation && this.anim_state == CharAnimState.Tool;
    }
  }

  public bool playing_animation
  {
    get
    {
      if (this.playing_work_animation)
        return true;
      return this.attack.enabled && this.attack.performing_attack;
    }
  }

  public CharAnimState anim_state => this._anim_state;

  public Direction anim_direction => this._anim_direction;

  public bool has_overhead
  {
    get
    {
      return this.overhead_item != null && this.overhead_item.IsNotEmpty() && this.overhead_item.definition != null;
    }
  }

  public bool control_enabled
  {
    get => this._control_enabled;
    set
    {
      this._control_enabled = value;
      if (this._control_enabled || this.anim_state == CharAnimState.Idle && !this.playing_animation)
        return;
      this.SetMovementDir(Vector2.zero);
      this.components.tool.TryStop();
    }
  }

  public float dir_angle => this._dir_angle;

  public float anim_dir_angle => this._anim_dir_angle;

  public bool repeat_action => (double) Time.time - (double) this._last_pressed_attack_time < 0.25;

  public override void StartComponent()
  {
    if (this.started)
      return;
    base.StartComponent();
    this.on_move_dir = new MovementComponent.OnMove(this.OnChangeDir);
    this.InitAnimator();
    BaseCharacterAttack attack = this.attack;
    if ((UnityEngine.Object) attack != (UnityEngine.Object) null)
      attack.StartComponent();
    BaseCharacterIdle idle = this.idle;
    if ((UnityEngine.Object) idle != (UnityEngine.Object) null)
      idle.StartComponent();
    this.SetAnimationState(CharAnimState.Idle);
    this._cam = Camera.main;
    this.overhead_item = (Item) null;
    this.ProcessDirection(this.direction);
    if (!this.wgo.is_player)
      return;
    this.wgo.SetCurrentItem(ItemDefinition.ItemType.None);
    this.SetToolGraphics(0);
    GUIElements.me.hud.toolbar.keyboard.SetClickCallback(new Action<int>(this.UseItemFromToolbar));
  }

  public void OnStopped() => this.SetAnimationState(CharAnimState.Idle);

  public void OnStartWalking() => this.SetAnimationState(CharAnimState.Walking);

  public override int GetExecutionOrder() => 5;

  public void TeleportWithFade(
    Vector2 dest,
    GJCommons.VoidDelegate middle_delegate = null,
    GJCommons.VoidDelegate finished_delegate = null)
  {
    MainGame.me.save.quests.CheckKeyQuests("teleport");
    if (!this.wgo.is_player)
    {
      this.tf.position = (Vector3) (dest * 96f);
    }
    else
    {
      if (this.wgo.dont_update)
        return;
      this.wgo.dont_update = true;
      GUIElements.ChangeBubblesVisibility(false);
      CameraTools.Fade((GJCommons.VoidDelegate) (() =>
      {
        this.tf.position = (Vector3) (dest * 96f);
        this.wgo.GetComponent<ChunkedGameObject>().RecalculateChunk();
        CameraTools.MoveToPos((Vector2) this.tf.position);
        middle_delegate.TryInvoke();
        CameraTools.UnFade((GJCommons.VoidDelegate) (() =>
        {
          this.wgo.dont_update = false;
          GUIElements.ChangeBubblesVisibility(true);
          finished_delegate.TryInvoke();
        }));
      }));
    }
  }

  public void TeleportWithFade(
    WorldGameObject wgo,
    GJCommons.VoidDelegate middle_delegate = null,
    GJCommons.VoidDelegate finished_delegate = null)
  {
    this.TeleportWithFade(wgo.grid_pos, middle_delegate, finished_delegate);
  }

  public void TeleportWithFade(
    Transform trnsfrm,
    GJCommons.VoidDelegate middle_delegate = null,
    GJCommons.VoidDelegate finished_delegate = null)
  {
    this.TeleportWithFade((Vector2) (trnsfrm.position / 96f), middle_delegate, finished_delegate);
  }

  public bool IsInSector(
    BaseCharacterComponent other_char,
    int sector_index = 0,
    bool ignore_obstacles = true)
  {
    return this.IsInSector((Vector3) other_char.wgo.pos, sector_index, ignore_obstacles);
  }

  public bool IsInSector(WorldGameObject wgo, int sector_index = 0, bool ignore_obstacles = true)
  {
    return this.IsInSector((Vector3) wgo.pos, sector_index, ignore_obstacles);
  }

  public bool IsInSector(Vector3 pos, int sector_index = 0, bool ignore_obstacles = true)
  {
    WorldObjectPart wop = this.wgo.GetWOP();
    if ((UnityEngine.Object) wop == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "IsInSector(): NULL WorldObjectPart", (UnityEngine.Object) this.wgo);
      return false;
    }
    if (sector_index >= wop.visibility_sectors.Length)
    {
      Debug.LogError((object) ("IsInSector(): wrong sector index: " + sector_index.ToString()), (UnityEngine.Object) this.wgo);
      return false;
    }
    VisibilitySector visibilitySector = wop.visibility_sectors[sector_index];
    if (!((UnityEngine.Object) visibilitySector == (UnityEngine.Object) null))
      return visibilitySector.IsTouching(pos, ignore_obstacles);
    Debug.LogError((object) "IsInSector(): NULL sector", (UnityEngine.Object) this.wgo);
    return false;
  }

  public override bool HasUpdate() => true;

  public BaseCharacterAttack attack
  {
    get
    {
      if (!this._attack_cached)
      {
        try
        {
          BaseCharacterAttack[] componentsInChildren = this.wgo.GetComponentsInChildren<BaseCharacterAttack>(true);
          if (componentsInChildren.Length != 0)
          {
            this._attack = componentsInChildren[0];
          }
          else
          {
            WorldObjectPart wop = this.wgo.GetWOP();
            if ((UnityEngine.Object) wop == (UnityEngine.Object) null)
            {
              Debug.LogError((object) ("Can't find a WOP for object " + this.wgo.obj_id), (UnityEngine.Object) this.wgo);
              return (BaseCharacterAttack) null;
            }
            this._attack = wop.gameObject.AddComponent<BaseCharacterAttack>();
          }
          this._attack_cached = true;
        }
        catch (Exception ex)
        {
          Debug.LogError((object) $"Exception at WGO \"{this.wgo.name}\": {ex?.ToString()}", (UnityEngine.Object) this.wgo);
          Debug.LogError((object) ex.StackTrace);
          Debug.LogError((object) ("Is WGO null? - " + ((UnityEngine.Object) this.wgo == (UnityEngine.Object) null).ToString()));
          return (BaseCharacterAttack) null;
        }
      }
      return this._attack;
    }
  }

  public BaseCharacterIdle idle
  {
    get
    {
      if (!this._idle_cached)
      {
        try
        {
          BaseCharacterIdle[] componentsInChildren = this.wgo.GetComponentsInChildren<BaseCharacterIdle>(true);
          this._idle = componentsInChildren.Length == 0 ? this.wgo.GetWOP().gameObject.AddComponent<BaseCharacterIdle>() : componentsInChildren[0];
          this._idle_cached = true;
        }
        catch (Exception ex)
        {
          Debug.LogError((object) $"Exception at WGO \"{this.wgo.name}\": {ex?.ToString()}", (UnityEngine.Object) this.wgo);
          Debug.LogError((object) ex.StackTrace);
          Debug.LogError((object) ("Is WGO null? - " + ((UnityEngine.Object) this.wgo == (UnityEngine.Object) null).ToString()));
          return (BaseCharacterIdle) null;
        }
      }
      if (this._deserialize_idle_on_use)
      {
        this._deserialize_idle_on_use = false;
        this._idle.Deserialize(this._serialized_idle_data);
      }
      return this._idle;
    }
  }

  public override void UpdateComponent(float delta_time)
  {
    if (this.wgo.is_player && !this.player_controlled_by_script)
      this.UpdatePlayer(delta_time);
    base.UpdateComponent(delta_time);
    BaseCharacterAttack attack = this.attack;
    if ((UnityEngine.Object) attack != (UnityEngine.Object) null)
      attack.UpdateComponent(delta_time);
    BaseCharacterIdle idle = this.idle;
    if ((UnityEngine.Object) idle != (UnityEngine.Object) null)
      idle.UpdateComponent(delta_time);
    this.CheckAnimatorStates();
    if (!this.components.animator.ParamExists("global_state"))
      return;
    int integer = this.components.animator.GetInteger("global_state");
    if (integer == this._global_state)
      return;
    Debug.LogError((object) $"Animator error: _global_state: {this._global_state.ToString()} differs from the animator: {integer.ToString()} for wgo: {this.wgo.name}", (UnityEngine.Object) this.wgo);
  }

  public bool PlayerControlIsDisabled()
  {
    return !this.control_enabled || !BaseGUI.all_guis_closed || !this.can_be_locally_controlled || MainGame.me.build_mode_logics.IsBuilding();
  }

  public void UpdatePlayer(float delta_time)
  {
    if (this.PlayerControlIsDisabled())
    {
      if (this.movement_state == MovementComponent.MovementState.None && (double) this.movement_dir.magnitude <= 0.0)
        return;
      this.StopMovement();
      this.movement_dir = Vector2.zero;
    }
    else if (LazyInput.GetKeyDown(GameKey.GameGUI))
      GUIElements.me.game_gui.Open();
    else if (LazyInput.GetKeyDown(GameKey.IngameMenu))
    {
      GUIElements.me.ingame_menu.Open();
    }
    else
    {
      if (LazyInput.GetKeyDown(GameKey.Inventory))
        GUIElements.me.game_gui.OpenAtTab(GameGUI.TabType.Inventory);
      if (LazyInput.GetKeyDown(GameKey.KnownNPCs))
        GUIElements.me.game_gui.OpenAtTab(GameGUI.TabType.NPCs);
      if (LazyInput.GetKeyDown(GameKey.Techs))
        GUIElements.me.game_gui.OpenAtTab(GameGUI.TabType.Techs);
      if (LazyInput.GetKeyDown(GameKey.Map))
        GUIElements.me.game_gui.OpenAtTab(GameGUI.TabType.Map);
      this.ProcessToolbar();
      if (this.ProcessAttack() || this.ProcessWork())
        return;
      this.RefindDocks();
      if (!LoadingGUI.is_shown && this.ProcessInteraction())
        return;
      this.OnChangeDir(LazyInput.GetDirection());
    }
  }

  public void ProcessToolbar()
  {
    for (int index = 0; index < 4; ++index)
    {
      if (LazyInput.GetKeyDown(LazyInput.toolbar_keys[index]))
      {
        this.UseItemFromToolbar(index);
        LazyInput.ClearKeyDown(LazyInput.toolbar_keys[index]);
        break;
      }
    }
  }

  public bool IsEnoughEnergyForDash()
  {
    if ((double) this.wgo.energy > 2.0)
      return true;
    EffectBubblesManager.ShowImmediately(this.wgo.bubble_pos, GJL.L("not_enough_something", "(en)"), EffectBubblesManager.BubbleColor.Energy);
    return false;
  }

  public bool ProcessDash()
  {
    if ((UnityEngine.Object) EnvironmentEngine.me != (UnityEngine.Object) null && EnvironmentEngine.me.IsTimeStopped() || LazyInput.GetKey(GameKey.Work) || this.wgo.temp_do_work)
      return false;
    if ((LazyInput.GetKeyDown(GameKey.Dash) || LazyInput.GetKeyDown(GameKey.Dash2)) && this.IsEnoughEnergyForDash())
      this.last_pressed_dash_time = Time.time;
    if ((double) this.dash_remaining_time < 0.0)
    {
      if ((double) Time.time - (double) this.last_pressed_dash_time < 0.02500000037252903)
      {
        if (!this.components.character.player.TrySpendEnergy(2f))
        {
          Debug.LogError((object) "FATAL ERROR! Not enough energy for dash, but dash was started!");
          return false;
        }
        this.ProcessDirection(LazyInput.GetDirection());
        this.dash_direction = this.direction.normalized;
        this.movement_dir = Vector2.zero;
        this.state = MovementComponent.MovementState.Dash;
        this.dash_remaining_time = 0.1f;
        Debug.Log((object) "Started Dash");
      }
      else
      {
        if ((double) this.dash_remaining_time > -1.0)
        {
          this.state = MovementComponent.MovementState.None;
          this.dash_remaining_time = -3f;
          Debug.Log((object) "Ended dash");
        }
        return false;
      }
    }
    return true;
  }

  public bool CheckEnegryForPlayerAtack()
  {
    Item equippedWeapon = this.wgo.is_player ? this.wgo.GetEquippedWeapon() : (Item) null;
    if (!this.wgo.is_player || equippedWeapon == null || equippedWeapon.definition == null || !(equippedWeapon.definition.params_on_use != (GameRes) null) || equippedWeapon.definition.params_on_use.IsEmpty() || (double) this.wgo.energy >= (double) equippedWeapon.definition.params_on_use.Get("energy") * -1.0)
      return true;
    EffectBubblesManager.ShowImmediately(this.wgo.bubble_pos, GJL.L("not_enough_something", "(en)"), EffectBubblesManager.BubbleColor.Energy);
    return false;
  }

  public bool ProcessAttack()
  {
    if (this.state == MovementComponent.MovementState.Dash)
      return false;
    if (LazyInput.GetKeyDown(GameKey.Attack) && this.CheckEnegryForPlayerAtack())
      this._last_pressed_attack_time = Time.time;
    if (this.attack.performing_attack)
      return true;
    if ((LazyInput.GetKeyDown(GameKey.Attack) || this.repeat_action) && !this.playing_work_animation)
    {
      Item equippedWeapon = this.wgo.is_player ? this.wgo.GetEquippedWeapon() : (Item) null;
      ItemDefinition.ItemType tool_n = equippedWeapon != null ? equippedWeapon.definition.type : ItemDefinition.ItemType.None;
      if (!this.wgo.is_player && tool_n == ItemDefinition.ItemType.None || tool_n == ItemDefinition.ItemType.Sword)
      {
        if (!this.CheckEnegryForPlayerAtack())
          return false;
        Debug.Log((object) "ATTACK!!!");
        this.ProcessDirection(LazyInput.GetDirection());
        this.SetWeaponGraphics((int) tool_n);
        if (this.attack.Perform(this.anim_direction, 0, new BaseCharacterAttack.AttackResult(this.OnPlayersAttackPerformed)))
          this.CheckPossibleStopWalking();
      }
      if (this.has_overhead)
        this.DropOverheadItem(this.attack.performing_attack);
    }
    return this.attack.performing_attack;
  }

  public bool ProcessWork()
  {
    bool flag = LazyInput.GetKey(GameKey.Work) || this.wgo.temp_do_work;
    if ((UnityEngine.Object) this.components.interaction.nearest != (UnityEngine.Object) null && this.components.interaction.nearest.has_linked_worker || this.wgo.player_cant_work)
      return false;
    if (this.dont_work_anymore)
    {
      if (flag)
        flag = false;
      else
        this.dont_work_anymore = false;
    }
    if (!flag)
    {
      this._needed_tool_bubble_shown = false;
      this.components.tool.TryStop();
      this.ResetDockPoints();
      if (this.movement_state == MovementComponent.MovementState.Following || this.movement_state == MovementComponent.MovementState.GoTo)
        this.StopMovement();
    }
    if (!flag && !this.playing_animation)
    {
      this.components.tool.ResetLastActionTime();
      return false;
    }
    bool keyDown = LazyInput.GetKeyDown(GameKey.Work);
    if (flag)
    {
      if (!this.DoDockCheck(keyDown))
      {
        if (this.has_overhead)
          this.DropOverheadItem();
        return false;
      }
      if ((UnityEngine.Object) this._current_dp != (UnityEngine.Object) null && (UnityEngine.Object) this._docked_obj == (UnityEngine.Object) null)
      {
        if (this.has_overhead)
          this.DropOverheadItem(true);
        return true;
      }
    }
    if (!this.components.tool.UseTool(this.DockIsOk()))
      return (UnityEngine.Object) this._current_dp != (UnityEngine.Object) null;
    this.CheckPossibleStopWalking();
    if (this.has_overhead)
      this.DropOverheadItem();
    return true;
  }

  public bool ProcessInteraction()
  {
    if (!LazyInput.GetKey(GameKey.Interaction))
      this.components.interaction.StopInteraction();
    if (this.PlayerControlIsDisabled() || !LazyInput.GetKeyDown(GameKey.Interaction))
      return false;
    bool flag = this.TryOtherInteractions();
    if (!flag && this.components.interaction.Interact(true))
    {
      flag = true;
      this.CheckPossibleStopWalking();
    }
    if (!flag && this.has_overhead)
      this.DropOverheadItem();
    return flag;
  }

  public void RefindDocks(bool force = false)
  {
    if (!force && Time.frameCount % 1 != 0)
      return;
    this._nearest_dock_point = this.GetNearestDockPoint();
    int num = (UnityEngine.Object) this._nearest_dock_point != (UnityEngine.Object) null ? 1 : 0;
  }

  public void UseItemFromToolbar(int index)
  {
    Item itemById = this.wgo.GetItemById(MainGame.me.save.GetEquippedItem(index));
    if (itemById == null)
      return;
    this.wgo.UseItemFromInventory(itemById);
  }

  public void ShowCustomNeedBubble(string text)
  {
    if (this._needed_tool_bubble_shown)
      return;
    this.wgo.Say(text);
    this._needed_tool_bubble_shown = true;
  }

  public void ShowNeededToolBubble(bool show_tech_lock)
  {
    if (this._needed_tool_bubble_shown)
      return;
    ObjectDefinition nearestDefinition = this.components.interaction.nearest_definition;
    if (nearestDefinition == null || nearestDefinition.tool_actions.no_actions)
      return;
    if (show_tech_lock)
      this.ShowCustomNeedBubble("no_tech_bubble");
    else
      this.ShowCustomNeedBubble($"no_{nearestDefinition.tool_actions.action_tools[0].ToString().ToLower()}_bubble");
  }

  public void OnPlayersAttackPerformed(bool success)
  {
    if (!this.repeat_action)
    {
      this.SetWeaponGraphics(0);
    }
    else
    {
      this.ProcessDirection(LazyInput.GetDirection());
      this._last_pressed_attack_time = 0.0f;
      this.attack.Perform(this.anim_direction, 0, new BaseCharacterAttack.AttackResult(this.OnPlayersAttackPerformed));
    }
  }

  public void InterruptAttack()
  {
    this.attack.InterruptAttack();
    this.SetAnimationState(CharAnimState.Idle);
  }

  public virtual bool TryOtherInteractions()
  {
    if (!((UnityEngine.Object) DropResGameObject.currently_higlighted_obj != (UnityEngine.Object) null) || !DropResGameObject.currently_higlighted_obj.CanPickupWithInteraction(this))
      return false;
    if (this.has_overhead)
      this.DropOverheadItem();
    Item obj = new Item(DropResGameObject.currently_higlighted_obj.res);
    if (DropResGameObject.currently_higlighted_obj.res != null)
      obj.sub_name = DropResGameObject.currently_higlighted_obj.res.sub_name;
    this.SetOverheadItem(obj);
    DropResGameObject.currently_higlighted_obj.is_collected = true;
    DropResGameObject.currently_higlighted_obj.DestroyLinkedHint();
    DropResGameObject.currently_higlighted_obj = (DropResGameObject) null;
    this.OnChangeDir(LazyInput.GetDirection());
    return true;
  }

  public bool DoDockCheck(bool try_same_dir_point = true)
  {
    if ((UnityEngine.Object) this.components.interaction.nearest != (UnityEngine.Object) null && this.components.interaction.nearest.has_linked_worker || this.wgo.player_cant_work)
      return false;
    try_same_dir_point = false;
    if (this.playing_animation)
      return true;
    if ((UnityEngine.Object) this._current_dp == (UnityEngine.Object) null)
    {
      this.FindDockPoint();
      if ((UnityEngine.Object) this._current_dp == (UnityEngine.Object) null)
      {
        WorldGameObject nearest = this.components.interaction.nearest;
        if ((UnityEngine.Object) nearest != (UnityEngine.Object) null && nearest.obj_def.tool_actions.no_actions)
        {
          int num = nearest.is_removing ? 1 : 0;
        }
        return false;
      }
    }
    if (!this._current_dp.parent_wgo.CanProcessWork())
    {
      this._current_dp = this._nearest_dock_point = (DockPoint) null;
      return false;
    }
    this._current_dp.CheckIfReached();
    if (this._current_dp.just_rotate)
      this.ProcessDirection(this._current_dp.reach_dir);
    if (this._current_dp.reached || this._current_dp.just_rotate)
    {
      this.StopMovement();
      this._docked_obj = this._current_dp.parent_wgo;
      this.tf.SetXY((Vector2) this._current_dp.tf.position);
    }
    else
    {
      Direction actionDir = this._current_dp.GetActionDir();
      Direction direction = ((Vector2) this._current_dp.tf.position - this.wgo.pos).ToDirection();
      if (try_same_dir_point && actionDir != direction)
      {
        Debug.Log((object) $"dp = {actionDir.ToString()}, to_dir = {direction.ToString()}");
        foreach (DockPoint componentsInChild in this._current_dp.parent_wgo.GetComponentsInChildren<DockPoint>())
        {
          if (componentsInChild.GetActionDir() == direction)
          {
            componentsInChild.SetTarget(this._current_dp.target);
            this._current_dp.SetTarget((BaseCharacterComponent) null);
            this._current_dp = componentsInChild;
            return this.DoDockCheck(false);
          }
        }
      }
      this.GoTo((Vector2) this._current_dp.tf.position);
    }
    return true;
  }

  public void ResetDockPoints()
  {
    if (!((UnityEngine.Object) this._current_dp != (UnityEngine.Object) null))
      return;
    this._current_dp.SetTarget((BaseCharacterComponent) null);
    this._current_dp = (DockPoint) null;
  }

  public bool DockIsOk()
  {
    return (UnityEngine.Object) DropResGameObject.currently_higlighted_obj == (UnityEngine.Object) null && (UnityEngine.Object) this._current_dp != (UnityEngine.Object) null && (UnityEngine.Object) this.components.interaction.nearest != (UnityEngine.Object) null && (UnityEngine.Object) this._current_dp.parent_wgo == (UnityEngine.Object) this._docked_obj && this._current_dp.GetActionDir() == this.anim_direction && this._current_dp.CalcDistToTarget().EqualsTo(0.0f, 1f / 500f);
  }

  public DockPoint GetNearestDockPoint()
  {
    List<DockPoint> dockPointList = new List<DockPoint>();
    if ((UnityEngine.Object) MainGame.me.player.components.character.wgo_hilighted_for_work != (UnityEngine.Object) null)
    {
      DockPoint[] componentsInChildren = MainGame.me.player.components.character.wgo_hilighted_for_work.GetComponentsInChildren<DockPoint>();
      if (componentsInChildren.Length != 0)
        dockPointList.AddRange((IEnumerable<DockPoint>) componentsInChildren);
    }
    if (dockPointList.Count == 0)
    {
      Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(this.wgo.pos, 144f, 2048 /*0x0800*/);
      if (collider2DArray.Length == 0)
        return (DockPoint) null;
      WorldGameObject nearest = this.components.interaction.nearest;
      bool nearestHasAction = this.components.interaction.nearest_has_action;
      if ((UnityEngine.Object) nearest != (UnityEngine.Object) null && LazyInput.GetKey(GameKey.Select))
        Debug.Log((object) ("nearest: " + nearest?.ToString()), (UnityEngine.Object) nearest);
      foreach (Collider2D collider2D in collider2DArray)
      {
        if (!((UnityEngine.Object) collider2D == (UnityEngine.Object) null))
        {
          DockPoint component = collider2D.GetComponent<DockPoint>();
          if (!((UnityEngine.Object) component == (UnityEngine.Object) null) && !component.shouldnt_be_used && (!nearestHasAction || !((UnityEngine.Object) component.parent_wgo != (UnityEngine.Object) nearest)) && !((UnityEngine.Object) component.parent_wgo == (UnityEngine.Object) null) && component.parent_wgo.obj_def != null && (!component.parent_wgo.obj_def.tool_actions.no_actions || component.parent_wgo.is_removing) && !component.IsUnreachable(15.36f) && !component.parent_wgo.has_linked_worker && !component.parent_wgo.player_cant_work && (!component.craft.enabled || (!component.craft.is_crafting || !component.craft.current_craft.is_auto || component.craft.current_craft.hidden) && (component.craft.is_crafting || !component.craft.IsCraftQueueEmpty())))
            dockPointList.Add(component);
        }
      }
    }
    if (dockPointList.Count == 0)
      return (DockPoint) null;
    if (dockPointList.Count == 1)
      return dockPointList[0];
    Vector2 vector2 = (Vector2) this.tf.position + (Vector2) this.anim_direction.ToVec3() * 19.2f;
    float num = float.MaxValue;
    int index1 = 0;
    for (int index2 = 0; index2 < dockPointList.Count; ++index2)
    {
      DockPoint dockPoint = dockPointList[index2];
      float sqrMagnitude = ((Vector2) dockPoint.tf.position - vector2).sqrMagnitude;
      if (dockPoint.GetActionDir() != this.anim_direction)
        sqrMagnitude += 3500f;
      if ((double) sqrMagnitude < (double) num)
      {
        num = sqrMagnitude;
        index1 = index2;
      }
    }
    return dockPointList[index1];
  }

  public void FindDockPoint()
  {
    this._current_dp = this._nearest_dock_point;
    if ((UnityEngine.Object) this._current_dp == (UnityEngine.Object) null)
      this._current_dp = this.GetNearestDockPoint();
    if (!((UnityEngine.Object) this._current_dp != (UnityEngine.Object) null))
      return;
    this._current_dp.SetTarget(this);
  }

  public override bool HasLateUpdate() => true;

  public override void LateUpdateComponent() => this.wgo.RoundContentPos();

  public void SetAnimationState(CharAnimState state, ItemDefinition.ItemType item_type = ItemDefinition.ItemType.None)
  {
    this._anim_state = state;
    this.SetGlobalState(state, item_type);
  }

  public void SetGlobalState(CharAnimState state, ItemDefinition.ItemType item_type = ItemDefinition.ItemType.None)
  {
    int new_state = state == CharAnimState.Tool ? (int) (100 + item_type) : (int) state;
    if (this._global_state == new_state)
      return;
    if (state == CharAnimState.Tool)
      Debug.Log((object) $"Set global state: {item_type.ToString()} tool");
    if (new_state == 4)
    {
      Debug.Log((object) ("Trigger " + new_state.ToString()));
      this.components.animator.SetTrigger("use_tool_" + new_state.ToString());
    }
    this.SetGlobalState(new_state);
  }

  public void DeserializeGlobalState(int new_state) => this._global_state = new_state;

  public void SetGlobalState(int new_state)
  {
    if (this._global_state == new_state)
      return;
    this._global_state = new_state;
    this.CheckAnimatorStates();
    this.components.animator.enabled = true;
    this.components.animator.SetInteger("global_state", this._global_state);
    if (new_state != 0)
      return;
    this.components.animator.SetInteger("sub_state", this.components.character.idle_animation);
  }

  public void StartPrayAnimation(bool success)
  {
    this.control_enabled = false;
    this.SetGlobalState(-12);
    this.components.animator.SetTrigger("start_pray");
    WorldMap.GetChurchPulpit().animator.SetBool(nameof (success), success);
  }

  public void ProcessDirection(Vector2 s)
  {
    if (this.playing_animation || s.magnitude.Equals(0.0f))
      return;
    if (!this.started)
      this.components.StartComponents();
    this.direction = s;
    this.SetDirectionVectorForAnimator(s);
  }

  public void LookAt(WorldGameObject wobj) => this.ProcessDirection(this.wgo.DirTo(wobj.pos));

  public void LookAt(GameObject go)
  {
    this.ProcessDirection(this.wgo.DirTo((Vector2) go.transform.position));
  }

  public void LookAt(Direction dir) => this.ProcessDirection(dir.ToVec());

  public void LookAt(Vector2 dir) => this.ProcessDirection(dir);

  public void OnChangeDir(Vector2 dir)
  {
    this.ProcessDirection(dir);
    this.ProcessMovement(dir);
  }

  public void ProcessMovement(Vector2 s)
  {
    if ((double) s.magnitude > 0.0)
    {
      this.OnStartWalking();
      this._docked_obj = (WorldGameObject) null;
      this.movement_dir = s.normalized;
      if (!this.wgo.is_player || !this.control_enabled)
        return;
      FloatingWorldGameObject.MoveCurrentFloatingObject((Vector2) MainGame.me.player.transform.localPosition, false);
    }
    else
    {
      this.OnStopped();
      this.movement_dir = Vector2.zero;
    }
  }

  public void CheckPossibleStopWalking()
  {
    this.movement_dir = Vector2.zero;
    if (this.anim_state != CharAnimState.Walking && this._global_state != -1)
      return;
    this.SetAnimationState(CharAnimState.Idle);
  }

  public void SetDirectionVectorForAnimator(Vector2 dir)
  {
    if (dir.EqualsTo(this._last_anim_vec_dir, 0.1f))
      return;
    float x = dir.x;
    float y = dir.y;
    this._dir_angle = Mathf.Atan2(y, x) * 57.29578f;
    this._anim_dir_angle = Mathf.Round(this._dir_angle / 90f) * 90f;
    this.components.animator.SetFloat("diagonal_direction_angle", Mathf.Round(this._dir_angle / 45f) * 45f);
    this.CheckAnimatorStates();
    this.components.animator.SetFloat("direction_angle", this._anim_dir_angle);
    this._last_anim_vec_dir.x = x;
    this._last_anim_vec_dir.y = y;
    this._anim_direction = this._last_anim_vec_dir.ToDirection();
  }

  public void OnEnterChestArea(WorldGameObject chest)
  {
    if (this.chests_in_area.Contains(chest))
      return;
    this.chests_in_area.Add(chest);
  }

  public void OnExitChestArea(WorldGameObject chest)
  {
    if (!this.chests_in_area.Contains(chest))
      return;
    this.chests_in_area.Remove(chest);
  }

  public Item GetOverheadItem() => !this.has_overhead ? (Item) null : this.overhead_item;

  public void TryDropOverheadItem()
  {
    if (!this.has_overhead)
      return;
    this.wgo.DropItem(this.overhead_item);
    this.SetOverheadItem((Item) null);
  }

  public void SetOverheadItem(Item item)
  {
    Debug.Log((object) ("SetOverheadItem = " + (item == null ? "null" : item.id)));
    Item overheadItem = this.overhead_item;
    this.overhead_item = item;
    PlayerComponent player = this.player;
    if ((UnityEngine.Object) player.spr_overhead_obj == (UnityEngine.Object) null)
      return;
    if ((bool) (UnityEngine.Object) player.spr_tool)
      player.spr_tool.sprite = (UnityEngine.Sprite) null;
    if ((bool) (UnityEngine.Object) player.spr_tool_2)
      player.spr_tool_2.sprite = (UnityEngine.Sprite) null;
    if (!MainGame.game_starting && overheadItem != item)
      Sounds.PlaySound(item == null ? "item_2h_drop" : "item_2h_pickup");
    player.spr_overhead_obj.gameObject.SetActive(item != null && item.IsNotEmpty());
    if (item == null || item.IsEmpty())
    {
      this.SetWalkAnimationType(BaseCharacterComponent.WalkAnimationType.Standard);
      if (Application.isPlaying)
        MainGame.me.save.quests.CheckKeyQuests("overhead_none");
    }
    else
    {
      this.SetWalkAnimationType(BaseCharacterComponent.WalkAnimationType.OverheadItem);
      player.spr_overhead_obj.sprite = EasySpritesCollection.GetSprite(item.GetOverheadIcon());
      if (Application.isPlaying)
      {
        string id = item.id;
        MainGame.me.save.quests.CheckKeyQuests("overhead_" + id);
      }
    }
    if (item?.definition == null || !item.definition.autouse)
      return;
    item.UseItem(MainGame.me.player);
  }

  public void SetCarryingItem(Item item)
  {
    Debug.Log((object) ("SetCarryingItem = " + (item == null ? "null" : item.id)));
    SpriteRenderer carryingItemSprite = this.wgo?.wop?.carrying_item_sprite;
    if ((UnityEngine.Object) carryingItemSprite == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "SetCarryingItem error: spr is null!");
    }
    else
    {
      Collider2D component = carryingItemSprite.GetComponent<Collider2D>();
      if (item == null || item.IsEmpty())
      {
        carryingItemSprite.sprite = (UnityEngine.Sprite) null;
        if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
          return;
        component.enabled = false;
      }
      else
      {
        carryingItemSprite.sprite = EasySpritesCollection.GetSprite(item.GetIcon());
        if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
          return;
        component.enabled = true;
      }
    }
  }

  public void SwitchTorch()
  {
    ItemDefinition.ItemType itemType = this.wgo.GetCurrentItemType() == ItemDefinition.ItemType.Torch ? ItemDefinition.ItemType.None : ItemDefinition.ItemType.Torch;
    this.wgo.SetCurrentItem(itemType);
    this.SetToolGraphics((int) itemType);
  }

  public void SetToolGraphics(int tool_n)
  {
    PlayerComponent player = this.player;
    if ((UnityEngine.Object) player.spr_tool == (UnityEngine.Object) null)
      return;
    Debug.Log((object) ("SetToolGraphics " + tool_n.ToString()));
    this.skin.weapon = tool_n;
    if (tool_n > 0)
    {
      this.SetWalkAnimationType(BaseCharacterComponent.WalkAnimationType.WithTool);
      Debug.Log((object) ("Set sprite: " + player.spr_tool.sprite?.ToString()));
      player.spr_overhead_obj.gameObject.SetActive(false);
    }
    else
    {
      player.spr_tool.sprite = (UnityEngine.Sprite) null;
      this.SetOverheadItem(this.overhead_item);
    }
    if ((bool) (UnityEngine.Object) player.spr_tool_2)
      player.spr_tool_2.sprite = player.spr_tool.sprite;
    if (!(bool) (UnityEngine.Object) player.light_go)
      return;
    player.light_go.SetActive(tool_n == 9);
  }

  public void SetWeaponGraphics(int tool_n)
  {
    PlayerComponent player = this.player;
    if ((UnityEngine.Object) player.spr_tool == (UnityEngine.Object) null)
      return;
    this.skin.weapon = tool_n;
    if ((bool) (UnityEngine.Object) player.spr_tool_2)
      player.spr_tool_2.sprite = player.spr_tool.sprite;
    if (!(bool) (UnityEngine.Object) player.light_go)
      return;
    player.light_go.SetActive(tool_n == 9);
  }

  public void SetWalkAnimationType(BaseCharacterComponent.WalkAnimationType a)
  {
    float v;
    switch (a)
    {
      case BaseCharacterComponent.WalkAnimationType.Standard:
        v = 0.0f;
        break;
      case BaseCharacterComponent.WalkAnimationType.OverheadItem:
        v = 0.1f;
        break;
      case BaseCharacterComponent.WalkAnimationType.WithTool:
        v = -0.1f;
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof (a));
    }
    Debug.Log((object) ("Set walk animation type = " + a.ToString()));
    this.components.animator.SetFloat("walk_type_f", v);
  }

  public void InitAnimator()
  {
  }

  public void CheckAnimatorStates()
  {
  }

  public void DropOverheadItem(bool to_right = false)
  {
    if (!this.has_overhead)
    {
      Debug.LogError((object) "Trying to drop null overhead item");
    }
    else
    {
      DropResGameObject.Drop(this.tf.position, this.overhead_item, this.tf.parent, to_right ? this.anim_direction.ClockwiseDir() : this.anim_direction, 3f, UnityEngine.Random.Range(0, 2));
      this.SetOverheadItem((Item) null);
    }
  }

  public void OnWasDamaged(float damage_direction)
  {
    if (!this._ignore_was_damaged)
      this._was_damaged = true;
    damage_direction = Mathf.Round(damage_direction / 90f) * 90f;
    this.components.animator.SetFloat("was_damaged_direction", damage_direction);
    this.components.animator.SetTrigger("was_damaged");
  }

  public bool WasDamaged(bool clear_flag)
  {
    if (!this._was_damaged)
      return false;
    if (clear_flag)
      this._was_damaged = false;
    return true;
  }

  public void ChangeDamageFlagIgnoring(bool ignore)
  {
    this._ignore_was_damaged = ignore;
    this._was_damaged = false;
  }

  public PlayerComponent player => this.wgo.GetComponent<PlayerComponent>();

  public void SetLocalPlayerState(bool is_local_player)
  {
    Debug.Log((object) ("SetLocalPlayerState, is_local = " + is_local_player.ToString()), (UnityEngine.Object) this.wgo);
    this.can_be_locally_controlled = is_local_player;
    if (is_local_player)
      MainGame.me.SetMainPlayer(this.player);
    this.wgo.GetComponent<ChunkedGameObject>().always_active = true;
  }

  public bool ShowBubbleToLeft(bool? to_left) => to_left ?? this.anim_direction == Direction.Right;

  public void RescanChildSprites()
  {
    this._sprs = this.wgo.GetComponentsInChildren<SpriteRenderer>(true);
  }

  public void LateUpdate()
  {
    if (this._sprs == null)
      this.RescanChildSprites();
    foreach (SpriteRenderer spr in this._sprs)
    {
      if (!((UnityEngine.Object) spr == (UnityEngine.Object) null) && !((UnityEngine.Object) spr.sprite == (UnityEngine.Object) null))
      {
        string name = spr.sprite.name;
        string sprite_name = this.skin.ReplaceSpriteName(name);
        if (name != sprite_name)
          spr.sprite = EasySpritesCollection.GetSprite(sprite_name);
      }
    }
  }

  public Ground.GroudType GetGroundTypeUnderCharacter()
  {
    return WorldMap.GetGroundType((Vector2) this.tf.position);
  }

  public override void UpdateEnableState(ObjectDefinition.ObjType obj_type)
  {
    this.enabled = this.wgo.is_player || this.wgo.obj_def.IsCharacter();
  }

  public void Recache() => this._attack_cached = this._idle_cached = false;

  public void ShowDisabledInteractionBubble(WorldGameObject wgo)
  {
    if (this._disabled_interaction_shown)
      return;
    this._disabled_interaction_shown = true;
    MainGame.me.player.Say("disabled_interactions", (GJCommons.VoidDelegate) (() => this._disabled_interaction_shown = false));
  }

  public override void RefreshComponentBubbleData(bool show_interaction_buttons)
  {
  }

  public void DeserializeIdle(BaseCharacterIdle.SerializableCharacterIdle data)
  {
    this._deserialize_idle_on_use = true;
    this._serialized_idle_data = data;
  }

  public void SetWorkerToolNum(ItemDefinition.ItemType tool_type)
  {
    try
    {
      if (!this.wgo.gameObject.activeInHierarchy)
        this.wgo.gameObject.SetActive(true);
      ChunkedGameObject component = this.wgo.GetComponent<ChunkedGameObject>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        component.active_now_because_of_work = tool_type != 0;
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ex);
    }
    if (!this.components.animator.ParamExists("work_tool_num"))
      return;
    this.components.animator.SetInteger("work_tool_num", (int) tool_type);
  }

  public void SetNoWorkerTool()
  {
    try
    {
      if (!this.wgo.gameObject.activeInHierarchy)
        this.wgo.gameObject.SetActive(true);
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ex);
    }
    if (this.components.animator.ParamExists("work_tool_num"))
      this.components.animator.SetInteger("work_tool_num", 0);
    try
    {
      ChunkedGameObject component = this.wgo.GetComponent<ChunkedGameObject>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      component.active_now_because_of_work = false;
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ex);
    }
  }

  [CompilerGenerated]
  public void \u003CShowDisabledInteractionBubble\u003Eb__163_0()
  {
    this._disabled_interaction_shown = false;
  }

  public enum Environment
  {
    Outside,
    Inside,
  }

  public enum WalkAnimationType
  {
    Standard,
    OverheadItem,
    WithTool,
  }
}
