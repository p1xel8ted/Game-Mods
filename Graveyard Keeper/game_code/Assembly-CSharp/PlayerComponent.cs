// Decompiled with JetBrains decompiler
// Type: PlayerComponent
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class PlayerComponent : MonoBehaviour
{
  public const string DO_NOT_SHOW_WGO_QUALITIES = "do_not_show_wgo_qualities";
  [NonSerialized]
  public bool is_local_player;
  public LeaveTrailComponent _trail;
  public float _energy_spent_sum;
  public float _sanity_spent_sum;
  public float _gratitude_points_spent_sum;
  public WorldGameObject _wgo;
  public bool _wgo_set;
  public SpriteRenderer spr_overhead_obj;
  public SpriteRenderer spr_tool;
  public SpriteRenderer spr_tool_2;
  public SpriteRenderer fish;
  public SpriteRenderer fishadow;
  public GameObject light_go;
  public CustomNode garry_wash_talk_pos;
  public const float PLAYER_ZONE_UPDATE_PERIOD = 0.5f;
  public float _time_passed_after_zone_update;
  public DropCollectorComponent _drop_collector;
  public BuffDefinition _pray_buff;
  public bool _pray_buff_success;
  public Material player_material;
  public Color player_additional_color = Color.black;
  public CraftDefinition _pray_craft;
  public Item _throwing_item;
  [CompilerGenerated]
  public WorldZone \u003Ccurrent_zone\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003Cshow_wgo_qualities\u003Ek__BackingField;
  public float _last_need_energy_bubble_time;

  public LeaveTrailComponent Trail => this._trail;

  public WorldZone current_zone
  {
    get => this.\u003Ccurrent_zone\u003Ek__BackingField;
    set => this.\u003Ccurrent_zone\u003Ek__BackingField = value;
  }

  public bool show_wgo_qualities
  {
    get => this.\u003Cshow_wgo_qualities\u003Ek__BackingField;
    set => this.\u003Cshow_wgo_qualities\u003Ek__BackingField = value;
  }

  public WorldGameObject wgo
  {
    get
    {
      if (!this._wgo_set)
      {
        this._wgo_set = true;
        this._wgo = this.GetComponent<WorldGameObject>();
      }
      return this._wgo;
    }
  }

  public static GameObject GetPlayerPrefab() => CustomNetworkManager.me.playerPrefab;

  public void Awake()
  {
    Debug.Log((object) "Player Component Awake", (UnityEngine.Object) this);
    if (CustomNetworkManager.is_running)
      this.transform.SetParent(MainGame.me.world_root, false);
    this._trail = new LeaveTrailComponent(this.wgo.components.character, "human");
  }

  public void OnDestroy() => this.UnsubscribeMethods();

  public void OnStartLocalPlayer()
  {
    Debug.Log((object) "<color=cyan>OnStartLocalPlayer</color>", (UnityEngine.Object) this);
    this.ResetPlayerPosition();
    foreach (PlayerComponent componentsInChild in MainGame.me.world_root.GetComponentsInChildren<PlayerComponent>())
    {
      if ((UnityEngine.Object) componentsInChild == (UnityEngine.Object) this)
        MainGame.me.SetMainPlayer(this);
      else
        componentsInChild.GetComponent<BaseCharacterComponent>().can_be_locally_controlled = false;
    }
  }

  public void ResetPlayerPosition()
  {
    Vector3 vector3 = MainGame.me.save.player_position;
    if (!MainGame.loaded_from_scene_main)
      vector3 = World.player_default_pos;
    Debug.Log((object) $"ResetPlayerPosition: {vector3.ToString()}, loaded_from_scene_main = {MainGame.loaded_from_scene_main.ToString()}");
    this.gameObject.transform.SetParent(MainGame.me.world_root, false);
    this.transform.localPosition = vector3;
  }

  public void InitLocalPlayer()
  {
    MainGame.me.SetCameraPlayerFollow(this.transform);
    this.is_local_player = true;
    this.wgo.is_player = true;
    this.ResetPlayerPosition();
    CameraTools.MoveToPos((Vector2) this.transform.position);
    this.wgo.components.character.control_enabled = true;
    this.wgo.components.InitAllComponents();
    this._drop_collector = new DropCollectorComponent();
    this._drop_collector.Init(this.wgo);
    this._drop_collector.StartComponent();
    BaseGUI.on_window_opened += new BaseGUI.OnAnyWindowStateChanged(this.OnAnyWindowOpenStopUsingTool);
  }

  public static PlayerComponent SpawnPlayer(bool is_local_player = true, Item inventory = null)
  {
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Prefabs.me.player_prefab);
    PlayerComponent component = gameObject.GetComponent<PlayerComponent>();
    component.wgo.is_player = true;
    component.ResetPlayerPosition();
    if (is_local_player)
    {
      MainGame.me.player = gameObject.GetComponent<WorldGameObject>();
      AStarSearcher.AddDefaultSeekerModifier(gameObject, true);
      component.InitLocalPlayer();
    }
    if (inventory != null)
      component.wgo.RestoreSavedInventory(inventory);
    BaseCharacterComponent character = component.wgo.components.character;
    if (is_local_player)
      MainGame.me.player_char = character;
    character.SetLocalPlayerState(is_local_player);
    int num = is_local_player ? 1 : 0;
    gameObject.GetComponent<WorldGameObject>().wop.gameObject.transform.localScale = Vector3.one;
    component.show_wgo_qualities = true;
    Debug.Log((object) ("Spawning player, is_local_player = " + is_local_player.ToString()), (UnityEngine.Object) gameObject);
    return component;
  }

  public void Update()
  {
    if (MainGame.paused)
      return;
    if (this.is_local_player)
      MainGame.me.player_pos = this.transform.position;
    if (this._trail != null)
      this._trail.CustomUpdate();
    if (this._drop_collector != null)
      this._drop_collector.UpdateComponent(Time.deltaTime);
    this._time_passed_after_zone_update += Time.deltaTime;
    if ((double) this._time_passed_after_zone_update > 0.5)
      this.UpdateZone();
    this.player_material.SetColor("_AdditionalColour", this.player_additional_color);
  }

  public void CheckShowWGOQuality()
  {
    this.show_wgo_qualities = MainGame.me.player.GetParamInt("do_not_show_wgo_qualities") == 0;
    this.RedrawQualities();
  }

  public void RedrawQualities(bool separate_k = false)
  {
    if (!((UnityEngine.Object) this.current_zone != (UnityEngine.Object) null))
      return;
    this.current_zone.RedrawQualities(new bool?(this.show_wgo_qualities), separate_k);
  }

  public void UpdateZone()
  {
    this._time_passed_after_zone_update = 0.0f;
    WorldZone myWorldZone = this.wgo.GetMyWorldZone();
    if ((UnityEngine.Object) myWorldZone == (UnityEngine.Object) null)
    {
      GUIElements.me.hud.UpdateZoneInfo("...", "");
    }
    else
    {
      string description = string.IsNullOrEmpty(myWorldZone.definition.hud_descr_str) ? "" : GJL.L(myWorldZone.definition.hud_descr_str, myWorldZone.GetQualityString());
      GUIElements.me.hud.UpdateZoneInfo(GJL.L("zone_" + myWorldZone.id), description);
    }
    if (!((UnityEngine.Object) this.current_zone != (UnityEngine.Object) myWorldZone))
      return;
    if ((UnityEngine.Object) this.current_zone != (UnityEngine.Object) null)
      this.current_zone.OnPlayerExit();
    this.current_zone = myWorldZone;
    if (!((UnityEngine.Object) this.current_zone != (UnityEngine.Object) null))
      return;
    this.current_zone.OnPlayerEnter();
  }

  public void ForceTrailChange(Ground.GroudType ground_type)
  {
    this._trail.SteppedOnANewSurface(ground_type);
  }

  public bool CheckEnergy(float need_energy) => (double) this.wgo.energy >= (double) need_energy;

  public bool TrySpendEnergy(float need_energy)
  {
    if (this.wgo.IsPlayerInvulnerable())
      need_energy = 0.0f;
    if ((double) this.wgo.energy >= (double) need_energy)
    {
      this._energy_spent_sum += need_energy;
      this.wgo.energy -= need_energy;
      if ((double) this._energy_spent_sum >= 1.0)
      {
        int num = Mathf.FloorToInt(this._energy_spent_sum);
        this._energy_spent_sum -= (float) num;
        EffectBubblesManager.ShowStackedEnergy(this.wgo, (float) -num);
      }
      return true;
    }
    if (this.wgo.is_player && this.wgo.components.character.anim_state != CharAnimState.Fishing)
      this.wgo.components.character.SetAnimationState(CharAnimState.Idle);
    this.wgo.components.tool.StopUsingTool(true);
    this.ShowNeedEnergyBubble();
    return false;
  }

  public bool IsEnoughEnergyToWork()
  {
    WorldGameObject nearest = this.wgo.components.interaction.nearest;
    float deltaTime = Time.deltaTime;
    if ((UnityEngine.Object) nearest == (UnityEngine.Object) null)
      return false;
    CraftComponent craft = nearest.components.craft;
    return craft.enabled && craft.is_crafting && !craft.current_craft.is_auto ? craft.CanSpendPlayerEnergy(this.wgo, deltaTime) : nearest.components.hp.CanSpendPlayerEnergy(this.wgo, deltaTime);
  }

  public void ShowNeedEnergyBubble()
  {
    if ((double) Time.time - (double) this._last_need_energy_bubble_time < 0.5)
      return;
    this._last_need_energy_bubble_time = Time.time;
    EffectBubblesManager.ShowImmediately(this.wgo.bubble_pos, GJL.L("not_enough_something", "(en)"), EffectBubblesManager.BubbleColor.Energy);
  }

  public void SpendSanity(float need_sanity)
  {
    if ((double) this.wgo.sanity >= (double) need_sanity)
    {
      this.wgo.sanity -= need_sanity;
      this._sanity_spent_sum += need_sanity;
      if ((double) this._sanity_spent_sum <= 1.0)
        return;
      int num = Mathf.FloorToInt(this._sanity_spent_sum);
      this._sanity_spent_sum -= (float) num;
      EffectBubblesManager.ShowStackedSanity(this.wgo, (float) -num);
    }
    else
      this.wgo.sanity = 0.0f;
  }

  public void ResetSpentCounters()
  {
    int num = Mathf.FloorToInt(this._energy_spent_sum);
    if (num <= 0)
      return;
    this._energy_spent_sum -= (float) num;
    EffectBubblesManager.ShowStackedEnergy(this.wgo, (float) -num);
  }

  public void OnTriggerStay2D(Collider2D col)
  {
    if (this._drop_collector == null)
      return;
    this._drop_collector.OnTriggerStay2D(col);
  }

  public void OnTriggerEnter2D(Collider2D col)
  {
    if (this._drop_collector == null)
      return;
    this._drop_collector.OnTriggerEnter2D(col);
  }

  public static string GetTechPointsString(string separator = " ")
  {
    return string.Format("(r){0}{3}(g){1}{3}(b){2}", (object) MainGame.me.player.GetParam("r"), (object) MainGame.me.player.GetParam("g"), (object) MainGame.me.player.GetParam("b"), (object) separator);
  }

  public static float GetTextObfuscationChance() => 0.0f;

  public static float GetTechPointsLoseChance() => 0.0f;

  public void DoSpawnPrayBuff() => WorldMap.GetChurchPulpit().animator.SetTrigger("start_buff");

  public void CreatePrayBuffFlyingObject(Vector2 pos)
  {
    FlyingObject.CreateBuffFlyingObject(this._pray_buff, pos, this._pray_craft.dur_parameter.EqualsTo(0.0f) ? new float?() : new float?(this._pray_craft.dur_parameter)).StartSmoothFly(GUIElements.me.buffs.grid.transform);
  }

  public void StartPrayAnimation(CraftDefinition pray_craft, bool success)
  {
    this._pray_buff = GameBalance.me.GetDataOrNull<BuffDefinition>(pray_craft.buff);
    this._pray_buff_success = success;
    this._pray_craft = pray_craft;
    this.wgo.components.character.StartPrayAnimation(success);
    ChurchPulpit churchPulpit = WorldMap.GetChurchPulpit();
    string sprite_name = pray_craft.icon;
    if (this._pray_buff != null && string.IsNullOrEmpty(sprite_name))
      sprite_name = this._pray_buff.GetIconName();
    Debug.Log((object) $"StartPrayAnimation, icon = {sprite_name}, pray_craft.id = {pray_craft.id}");
    churchPulpit.buff_spr.sprite = EasySpritesCollection.GetSprite(sprite_name);
  }

  public void ThrowBodyInRiver(out bool thrown_worker)
  {
    this._throwing_item = this.wgo.components.character.GetOverheadItem();
    thrown_worker = this._throwing_item.is_worker;
    this.wgo.components.character.SetOverheadItem((Item) null);
    this.wgo.components.character.control_enabled = false;
    this.wgo.components.character.SetAnimationState(CharAnimState.ThrowBody);
  }

  public void OnThrowBodyAnimationFinished()
  {
    this.wgo.components.character.SetAnimationState(CharAnimState.Idle);
    this.wgo.components.character.control_enabled = true;
  }

  public void ProcessDropOfThrowingBody()
  {
    Vector3 pos = MainGame.me.player_pos + new Vector3(-96f, 0.0f, 0.0f);
    ProjectileObject.Create("body_in_water", MainGame.me.world_root, (Vector2) pos, Vector2.down, MainGame.me.player).pop.attack_collider.gameObject.SetActive(false);
  }

  public void UnsubscribeMethods()
  {
    BaseGUI.on_window_opened -= new BaseGUI.OnAnyWindowStateChanged(this.OnAnyWindowOpenStopUsingTool);
  }

  public void OnAnyWindowOpenStopUsingTool(BaseGUI active_gui)
  {
    if (!this.wgo.components.tool.playing_animation)
      return;
    this.wgo.components.tool.StopUsingTool();
    this.wgo.components.character.OnStopped();
  }
}
