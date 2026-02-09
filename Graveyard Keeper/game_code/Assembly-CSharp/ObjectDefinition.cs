// Decompiled with JetBrains decompiler
// Type: ObjectDefinition
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class ObjectDefinition : BalanceBaseObject
{
  public const string DAMAGE = "damage";
  public const string SPEED = "speed";
  public const string SPEED_BUFF = "speed_buff";
  public const string ACCELERATION = "acceleration";
  public const string ACCELERATE_ALWAYS = "accelerate_always";
  public const string FRICTION = "friction";
  public const string DROP_MAGNET_DIST = "drop_magnet_dist";
  public const string KICK_FRICTION = "kick_friction";
  public const string KICK_SPEED = "kick_speed";
  public const int MAX_DAMAGE_TYPES = 10;
  public ObjectDefinition.ObjType type;
  public GameRes res;
  public List<Item> drop_items;
  public SmartExpression hp;
  public int inventory_size;
  public bool drop_inventory_after_remove;
  public ObjectDefinition.GlobalControlAccess global_craft_control_access;
  public bool do_drop_after_dying_anim_finished;
  public bool open_in_multiinventory;
  public bool dynamic_mob;
  public ObjectDefinition.QualityType quality_type;
  public string inventory_preset = "";
  public ToolActions tool_actions = new ToolActions();
  public ChancedStringValue after_hp_0;
  public bool save_variation;
  public ObjectDefinition.InteractionType interaction_type;
  public bool can_insert_zombie;
  public bool player_cant_work;
  public ObjectInteractionDefinition pre_interaction_1;
  public ObjectInteractionDefinition pre_interaction_2;
  public ObjectInteractionDefinition interaction_1;
  public ObjectInteractionDefinition interaction_2;
  public string script_after_hp_0 = "";
  public string craft_after_hp_0 = "";
  public string work = "";
  public string attached_script = "";
  public bool need_unlock_work;
  [SerializeField]
  public List<string> _object_groups;
  [SerializeField]
  public List<string> _affected_object_groups;
  public ObjectDefinition.DropPoint drop_point;
  public bool has_craft;
  public float damage_factor;
  public GameRes set_param_after_hp_0 = new GameRes();
  public GameRes add_param_after_hp_0 = new GameRes();
  public GameRes set_param_after_hp_0_end = new GameRes();
  public GameRes add_player_param_after_hp_0 = new GameRes();
  public SmartExpression add_player_param_after_hp_0_k;
  public string zone_id;
  public GameRes totem_params = new GameRes();
  public float totem_radius;
  public SmartExpression quality;
  public float quality_multiplier;
  public string overrode_quality_icon;
  public bool has_overrode_quality_icon;
  public bool ignore_counting_at_zone;
  public List<string> can_insert_items = new List<string>();
  public List<CustomItemInsertion> custom_insertions = new List<CustomItemInsertion>();
  public int can_insert_items_limit;
  public float durability_modificator;
  public List<string> res_product_types = new List<string>();
  public string craft_preset = "";
  public SmartSpeechEngine.VoiceID voice_id;
  public string ovr_music = "";
  public string custom_interaction_icon = "";
  public string work_sfx = "";
  public string work_end_sfx = "";
  public float mass;
  public float drag;
  public float armor;
  public bool npc_in_list;
  public bool check_only_interactions;
  public string npc_alias;
  public string custom_head_spr;
  public CraftDefinition.CraftSubType filter_craft_subtype;
  public string custom_icon;
  public List<string> additional_header_items;
  public bool can_belong_to_zone = true;
  public bool interactive_in_tutorial = true;
  public ObjectDefinition.HintPos hint_pos;
  public string day_icon;
  public int sort_n = 99999;
  public string drop_sound = "";
  public bool dont_restore_last_craft;
  public List<string> additional_worldzone_inventories = new List<string>();
  public int multi_inventory_priority = 100;
  public bool always_active;
  public string craft_start_sound = "";
  public string anim_on_craft_start = "";
  public string anim_on_craft_finish = "";
  [NonSerialized]
  public List<ObjectGroupDefinition> _object_group_links;
  [NonSerialized]
  public List<ObjectGroupDefinition> _affected_object_group_links;

  public bool IsNotInteractive(WorldGameObject wgo)
  {
    if (wgo.is_removing)
      return false;
    if (this.check_only_interactions)
      return this.GetValidInteraction(wgo) == null;
    ObjectInteractionDefinition validInteraction = this.GetValidInteraction(wgo);
    return this.interaction_type == ObjectDefinition.InteractionType.None && (validInteraction == null || string.IsNullOrEmpty(validInteraction.script)) && this.tool_actions.no_actions;
  }

  public float Damage(int i) => this.res.Get("damage_" + i.ToString());

  public float Damage(ObjectDefinition.DamageType dmg_type) => this.Damage((int) dmg_type);

  public float acceleration => this.res.Get(nameof (acceleration));

  public bool accelerate_always => (double) this.res.Get(nameof (accelerate_always)) > 0.0;

  public float friction => this.res.Get(nameof (friction));

  public float kick_speed => this.res.Get(nameof (kick_speed));

  public float kick_friction => this.res.Get(nameof (kick_friction));

  public List<ObjectGroupDefinition> object_groups
  {
    get
    {
      if (this._object_group_links == null)
      {
        this._object_group_links = new List<ObjectGroupDefinition>();
        foreach (string objectGroup in this._object_groups)
          this._object_group_links.Add(GameBalance.me.GetData<ObjectGroupDefinition>(objectGroup));
      }
      return this._object_group_links;
    }
  }

  public List<ObjectGroupDefinition> affected_object_groups
  {
    get
    {
      if (this._affected_object_group_links == null)
      {
        this._affected_object_group_links = new List<ObjectGroupDefinition>();
        foreach (string affectedObjectGroup in this._affected_object_groups)
          this._affected_object_group_links.Add(GameBalance.me.GetData<ObjectGroupDefinition>(affectedObjectGroup));
      }
      return this._affected_object_group_links;
    }
  }

  public ObjectInteractionDefinition GetValidInteraction(WorldGameObject wgo)
  {
    if (!Application.isPlaying)
      return (ObjectInteractionDefinition) null;
    ObjectInteractionDefinition[] interactionDefinitionArray;
    if (!this.can_insert_zombie)
      interactionDefinitionArray = new ObjectInteractionDefinition[2]
      {
        this.interaction_1,
        this.interaction_2
      };
    else
      interactionDefinitionArray = new ObjectInteractionDefinition[4]
      {
        this.pre_interaction_1,
        this.pre_interaction_2,
        this.interaction_1,
        this.interaction_2
      };
    foreach (ObjectInteractionDefinition validInteraction in interactionDefinitionArray)
    {
      if (!string.IsNullOrEmpty(validInteraction.hint) && validInteraction.condition.EvaluateBoolean(wgo))
        return validInteraction;
    }
    return (ObjectInteractionDefinition) null;
  }

  public string GetInteractionHint(WorldGameObject wgo)
  {
    if (wgo.is_removing)
      return "";
    ObjectInteractionDefinition validInteraction = this.GetValidInteraction(wgo);
    if (validInteraction == null)
      return "";
    switch (validInteraction.hint.ToLower())
    {
      case "speak":
      case "talk":
        return "(speak)";
      default:
        return validInteraction.hint;
    }
  }

  public bool DoesBelongToGroup(string group_id) => this._object_groups.Contains(group_id);

  public bool IsTotem() => !this.totem_radius.EqualsTo(0.0f);

  public bool HasObjectGroupWithID(string obj_group_id)
  {
    foreach (BalanceBaseObject objectGroup in this.object_groups)
    {
      if (objectGroup.id == obj_group_id)
        return true;
    }
    return false;
  }

  public bool IsCharacter()
  {
    return this.type == ObjectDefinition.ObjType.Mob || this.type == ObjectDefinition.ObjType.NPC;
  }

  public bool IsMob() => this.type == ObjectDefinition.ObjType.Mob;

  public bool IsNPC() => this.type == ObjectDefinition.ObjType.NPC;

  public bool IsPorterStation() => this.type == ObjectDefinition.ObjType.PorterStation;

  public bool IsRelationVisible()
  {
    if (!string.IsNullOrEmpty(this.npc_alias))
      return true;
    return this.IsNPC() && this.npc_in_list;
  }

  public enum DamageType
  {
    Damage_0,
    Damage_1,
    Damage_2,
    Damage_3,
    Damage_4,
    Damage_5,
    Damage_6,
    Damage_7,
    Damage_8,
    Damage_9,
  }

  public enum ObjType
  {
    Default,
    Mob,
    NPC,
    PorterStation,
    SoulTotem,
  }

  public enum InteractionType
  {
    None = 0,
    Craft = 1,
    RunScript = 2,
    Builder = 4,
    Chest = 5,
    Grave = 6,
  }

  public enum DropPoint
  {
    Auto,
    Center,
  }

  public enum QualityType
  {
    Hidden,
    Shown,
    Grave,
  }

  public enum GlobalControlAccess
  {
    None,
    Ignore,
    ForceAdd,
  }

  public enum HintPos
  {
    Default,
    AbovePlayer,
  }
}
