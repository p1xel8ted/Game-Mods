// Decompiled with JetBrains decompiler
// Type: GameBalance
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using LinqTools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class GameBalance : GameBalanceBase
{
  public static GameBalance _instance;
  public List<JobDefinition> jobs_data = new List<JobDefinition>();
  public List<CharDefinition> chars_data = new List<CharDefinition>();
  public List<JobAtomDefinition> jobs_atom_data = new List<JobAtomDefinition>();
  public List<LevelGrades> grade_levels = new List<LevelGrades>();
  public List<ObjectDefinition> objs_data = new List<ObjectDefinition>();
  public List<ItemDefinition> items_data = new List<ItemDefinition>();
  public List<VendorDefinition> vendors_data = new List<VendorDefinition>();
  public List<CraftDefinition> craft_data = new List<CraftDefinition>();
  public List<ObjectCraftDefinition> craft_obj_data = new List<ObjectCraftDefinition>();
  public List<ToolTypeDefinition> tools_data = new List<ToolTypeDefinition>();
  public List<AuraDefinition> auras_data = new List<AuraDefinition>();
  public List<ObjectGroupDefinition> object_groups = new List<ObjectGroupDefinition>();
  public List<QuestDefinition> quests_data = new List<QuestDefinition>();
  public List<SpawnerDefinition> spawners_data = new List<SpawnerDefinition>();
  public List<ProjectileDefinition> projectiles_data = new List<ProjectileDefinition>();
  public List<BodyDefinition> bodies_data = new List<BodyDefinition>();
  public List<SoulDefinition> souls_data = new List<SoulDefinition>();
  public List<GraveRequirement> grave_requirement_data = new List<GraveRequirement>();
  public List<TechBranchDefinition> tech_branches_data = new List<TechBranchDefinition>();
  public List<TechDefinition> techs_data = new List<TechDefinition>();
  public List<WorkDefinition> works_data = new List<WorkDefinition>();
  public List<LogicDefinition> logics_data = new List<LogicDefinition>();
  public List<ProductTypeDefinition> product_types_data = new List<ProductTypeDefinition>();
  public List<WorldZoneDefinition> world_zones_data = new List<WorldZoneDefinition>();
  public List<PerkDefinition> perks_data = new List<PerkDefinition>();
  public List<ReservoirsDefinition> reservoirs_data = new List<ReservoirsDefinition>();
  public List<FishDefinition> fishes_data = new List<FishDefinition>();
  public List<BuffDefinition> buffs_data = new List<BuffDefinition>();
  public List<PrayEventDefinition> pray_events_data = new List<PrayEventDefinition>();
  public List<AchievementDefinition> achievements_data = new List<AchievementDefinition>();
  public List<WorkerDefinition> workers_data = new List<WorkerDefinition>();
  public List<TransportPathsDefinition> transport_paths = new List<TransportPathsDefinition>();
  public List<CutscenesDLCDefinition> cutscenes_data = new List<CutscenesDLCDefinition>();
  public List<TavernEventDefinition> tavern_events = new List<TavernEventDefinition>();
  public Dictionary<string, List<CraftDefinition>> _craft_obj_hash = new Dictionary<string, List<CraftDefinition>>();
  public Dictionary<string, List<ObjectDefinition>> _craft_item_obj_hash = new Dictionary<string, List<ObjectDefinition>>();
  public Dictionary<string, List<string>> _items_basename_cache = new Dictionary<string, List<string>>();
  public Dictionary<ItemDefinition.ItemType, float> _tool_minimum_efficiency = new Dictionary<ItemDefinition.ItemType, float>();
  public const bool NORMALIZE_TOOLS_EFFICIENCY_FOR_MINUMUM_100_PERCENT = false;
  public static List<string> EMPTY_STRING_LIST = new List<string>();

  public override Dictionary<string, IList> GetAllDataListsAndGoogleTabs()
  {
    return new Dictionary<string, IList>()
    {
      {
        "ProductType",
        (IList) this.product_types_data
      },
      {
        "Perks",
        (IList) this.perks_data
      },
      {
        "Items",
        (IList) this.items_data
      },
      {
        "Objects",
        (IList) this.objs_data
      },
      {
        "Buffs",
        (IList) this.buffs_data
      },
      {
        "Vendors",
        (IList) this.vendors_data
      },
      {
        "Craft",
        (IList) this.craft_data
      },
      {
        "Survey",
        (IList) this.craft_data
      },
      {
        "Mix Craft",
        (IList) this.craft_data
      },
      {
        "Alchemy Ingr",
        (IList) this.craft_data
      },
      {
        "Sermon",
        (IList) this.craft_data
      },
      {
        "Put_Remove",
        (IList) this.craft_obj_data
      },
      {
        "Tool Types",
        (IList) this.tools_data
      },
      {
        "Auras_Buffs",
        (IList) this.auras_data
      },
      {
        "Obj Groups",
        (IList) this.object_groups
      },
      {
        "Quests",
        (IList) this.quests_data
      },
      {
        "Bodies",
        (IList) this.bodies_data
      },
      {
        "Souls",
        (IList) this.souls_data
      },
      {
        "Spawners",
        (IList) this.spawners_data
      },
      {
        "Projectiles",
        (IList) this.projectiles_data
      },
      {
        "Works",
        (IList) this.works_data
      },
      {
        "Logics",
        (IList) this.logics_data
      },
      {
        "TechBranches",
        (IList) this.tech_branches_data
      },
      {
        "Techs",
        (IList) this.techs_data
      },
      {
        "WorldZone",
        (IList) this.world_zones_data
      },
      {
        "Reservoirs",
        (IList) this.reservoirs_data
      },
      {
        "Fishes",
        (IList) this.fishes_data
      },
      {
        "PrayEvents",
        (IList) this.pray_events_data
      },
      {
        "Achievements",
        (IList) this.achievements_data
      },
      {
        "Workers",
        (IList) this.workers_data
      },
      {
        "TransportPaths",
        (IList) this.transport_paths
      },
      {
        "CutscenesDLC",
        (IList) this.cutscenes_data
      },
      {
        "Tavern Events",
        (IList) this.tavern_events
      }
    };
  }

  public static void Unload() => GameBalance._instance = (GameBalance) null;

  public static GameBalance me
  {
    get
    {
      if ((bool) (UnityEngine.Object) GameBalance._instance)
        return GameBalance._instance;
      GameBalance.LoadGameBalance();
      return GameBalance._instance;
    }
  }

  public static void LoadGameBalance()
  {
    GameBalance._instance = Resources.Load<GameBalance>("game_data");
    if ((UnityEngine.Object) GameBalance._instance == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Game data load failed");
    }
    else
    {
      GameBalance._instance.CreateIDsCache();
      GameBalance._instance.CreateItemsBaseNameCache();
      GameBalance._instance.CreateToolsCache();
      GameBalance._instance.CreateCraftsCache();
      ObjectGroupDefinition.LinkObjectsToGroups();
    }
  }

  public LevelGrades GetLevelGradeById(string id)
  {
    return this.grade_levels.Find((Predicate<LevelGrades>) (p => p.grade_id == id));
  }

  public GameRes GetObjectRes(string obj_id)
  {
    ObjectDefinition dataOrNull = this.GetDataOrNull<ObjectDefinition>(obj_id);
    return dataOrNull != null ? dataOrNull.res : new GameRes();
  }

  public List<CraftDefinition> GetCraftsForObject(string obj_id)
  {
    if (this._craft_obj_hash.ContainsKey(obj_id))
      return this._craft_obj_hash[obj_id];
    List<CraftDefinition> craftsForObject = new List<CraftDefinition>();
    foreach (CraftDefinition craftDefinition in this.craft_data)
    {
      if (craftDefinition.craft_in.Contains(obj_id))
        craftsForObject.Add(craftDefinition);
    }
    this._craft_obj_hash.Add(obj_id, craftsForObject);
    return craftsForObject;
  }

  public CraftDefinition GetFixCraftForItem(string item_id)
  {
    foreach (CraftDefinition fixCraftForItem in this.craft_data)
    {
      if (fixCraftForItem.craft_type == CraftDefinition.CraftType.Fixing && fixCraftForItem.output.Count > 0 && fixCraftForItem.output[0].id == item_id)
        return fixCraftForItem;
    }
    return (CraftDefinition) null;
  }

  public CraftDefinition GetRemoveCraftForItem(string obj_id, string item_id)
  {
    foreach (CraftDefinition removeCraftForItem in this.craft_data)
    {
      if (removeCraftForItem.craft_in.Contains(obj_id) && removeCraftForItem.set_out_wgo_params_on_start && removeCraftForItem.output.Count > 0 && removeCraftForItem.output[0].id == item_id)
        return removeCraftForItem;
    }
    Debug.LogError((object) ("Couldn't find a removal craft for item: " + item_id));
    return (CraftDefinition) null;
  }

  public void CreateItemsBaseNameCache()
  {
    this._items_basename_cache.Clear();
    foreach (ItemDefinition itemDefinition in this.items_data)
    {
      if (itemDefinition.id.Contains(":"))
      {
        string withoutQualitySuffix = itemDefinition.GetNameWithoutQualitySuffix();
        if (!this._items_basename_cache.ContainsKey(withoutQualitySuffix))
          this._items_basename_cache.Add(withoutQualitySuffix, new List<string>());
        this._items_basename_cache[withoutQualitySuffix].Add(itemDefinition.id);
      }
    }
  }

  public List<string> GetItemsOfBaseName(string base_name)
  {
    List<string> stringList;
    return !this._items_basename_cache.TryGetValue(base_name, out stringList) ? GameBalance.EMPTY_STRING_LIST : stringList;
  }

  public void CreateToolsCache()
  {
    this._tool_minimum_efficiency.Clear();
    foreach (ItemDefinition itemDefinition in this.items_data)
    {
      if (itemDefinition.type != ItemDefinition.ItemType.None)
      {
        if (!this._tool_minimum_efficiency.ContainsKey(itemDefinition.type))
          this._tool_minimum_efficiency.Add(itemDefinition.type, itemDefinition.efficiency);
        else if ((double) itemDefinition.efficiency < (double) this._tool_minimum_efficiency[itemDefinition.type])
          this._tool_minimum_efficiency[itemDefinition.type] = itemDefinition.efficiency;
      }
    }
    foreach (ItemDefinition.ItemType key in this._tool_minimum_efficiency.Keys.ToList<ItemDefinition.ItemType>())
      this._tool_minimum_efficiency[key] = 1f;
  }

  public int GetToolEfficiencyPercent(ItemDefinition tool_item)
  {
    return !this._tool_minimum_efficiency.ContainsKey(tool_item.type) ? 0 : Mathf.RoundToInt(tool_item.efficiency * 100f / this._tool_minimum_efficiency[tool_item.type]);
  }

  public void CreateCraftsCache()
  {
    this._craft_obj_hash.Clear();
    this._craft_item_obj_hash.Clear();
    foreach (CraftDefinition craftDefinition in this.craft_data)
    {
      foreach (string str in craftDefinition.craft_in)
      {
        if (!this._craft_obj_hash.ContainsKey(str))
          this._craft_obj_hash.Add(str, new List<CraftDefinition>());
        this._craft_obj_hash[str].Add(craftDefinition);
        if (!(str == "grave_ground") && !craftDefinition.hidden && !craftDefinition.dont_show_in_hint)
        {
          foreach (Item obj in craftDefinition.output)
          {
            if (!this._craft_item_obj_hash.ContainsKey(obj.id))
              this._craft_item_obj_hash.Add(obj.id, new List<ObjectDefinition>());
            ObjectDefinition data = this.GetData<ObjectDefinition>(str);
            if (data != null && !this._craft_item_obj_hash[obj.id].Contains(data))
              this._craft_item_obj_hash[obj.id].Add(data);
          }
        }
      }
    }
  }

  public List<ObjectDefinition> GetItemCraftsIn(string item_id)
  {
    List<ObjectDefinition> itemCraftsIn = (List<ObjectDefinition>) null;
    if (this._craft_item_obj_hash.TryGetValue(item_id, out itemCraftsIn))
      return itemCraftsIn;
    int num = 0;
    while (item_id.Contains(":"))
    {
      item_id = item_id.Substring(0, item_id.LastIndexOf(":"));
      if (this._craft_item_obj_hash.TryGetValue(item_id, out itemCraftsIn))
        return itemCraftsIn;
      if (num++ > 10)
        break;
    }
    return new List<ObjectDefinition>();
  }
}
