// Decompiled with JetBrains decompiler
// Type: GameSave
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using DLCRefugees;
using LinqTools;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

#nullable disable
[Serializable]
public class GameSave
{
  [SerializeField]
  public Item _inventory = new Item();
  public QuestSystem quests = new QuestSystem();
  public AchievementsSystem achievements = new AchievementsSystem();
  public int day = 1;
  [HideInInspector]
  public SerializableGameMap map = new SerializableGameMap();
  public CraftsInventory crafts = new CraftsInventory();
  public CraftsInventory obj_crafts = new CraftsInventory();
  public SavedDungeonsList dungeons = new SavedDungeonsList();
  public SavedWorkersList workers = new SavedWorkersList();
  public List<string> unlocked_techs = new List<string>();
  public List<string> unlocked_crafts = new List<string>();
  public List<string> locked_crafts = new List<string>();
  public List<string> unlocked_works = new List<string>();
  public List<string> unlocked_phrases = new List<string>();
  public List<string> unlocked_perks = new List<string>();
  public List<string> black_list_of_phrases = new List<string>();
  public List<string> completed_one_time_crafts = new List<string>();
  public List<string> known_world_zones = new List<string>();
  public List<string> known_fishes = new List<string>();
  public List<string> known_fishes_clear = new List<string>();
  public List<string> last_bait_reservoirs = new List<string>();
  public List<string> last_bait_baits = new List<string>();
  public List<string> revealed_techs = new List<string>();
  public List<string> visible_techs = new List<string>();
  public List<int> unlocked_tech_branches = new List<int>();
  public PlayersTavernEngine players_tavern_engine = new PlayersTavernEngine();
  public RefugeeCampData refugees_camp_data = new RefugeeCampData();
  public string[] equipped_items = new string[4];
  public GameLogics game_logics = new GameLogics();
  public int max_hp = 100;
  public int max_energy = 100;
  public int max_sanity = 100;
  public Vector3 player_position;
  public int dungeon_seed;
  public long unique_id_iterator = 1;
  public bool has_global_craft_control;
  [SerializeField]
  public EnvironmentEngine.EnvironmentEngineData _environment_engine_data;
  [SerializeField]
  public TimeOfDay.SerializedTimeOfDay _serialized_time_of_day;
  public List<GameSave.SavedDropItem> drops = new List<GameSave.SavedDropItem>();
  [SerializeField]
  public string _environment_preset = "";
  public string _stored_environment_preset = "";
  public List<PlayerBuff> buffs = new List<PlayerBuff>();
  public KnownNPCList known_npcs = new KnownNPCList();
  public List<GameSave.SavedGDPoint> gd_points = new List<GameSave.SavedGDPoint>();
  public float game_version = LazyConsts.VERSION;
  public float max_version = LazyConsts.VERSION;

  public int cur_time => Mathf.FloorToInt(Time.time * 1f);

  public ItemDefinition GetToolForAction(ItemDefinition.ItemType item_type)
  {
    return (ItemDefinition) null;
  }

  public void QuickSave()
  {
    File.WriteAllText("map.json", this.map.ToJSON());
    Debug.Log((object) "Map saved");
  }

  public void UnlockAllTechsForTest()
  {
    foreach (BalanceBaseObject balanceBaseObject in GameBalance.me.techs_data)
      this.UnlockTech(balanceBaseObject.id);
  }

  public void UnlockTech(string tech_id)
  {
    TechDefinition data = GameBalance.me.GetData<TechDefinition>(tech_id);
    if (data == null)
    {
      Debug.LogError((object) ("No such tech: " + tech_id));
    }
    else
    {
      if (!this.unlocked_techs.Contains(tech_id))
        this.unlocked_techs.Add(tech_id);
      if (data.hidden)
        this.RevealHiddenTech(tech_id);
      if (data.invisible)
        this.MakeVisibleInvisibleTech(tech_id);
      this.CopyLists(data.crafts, this.unlocked_crafts);
      this.CopyLists(data.works, this.unlocked_works);
      this.CopyLists(data.phrases, this.unlocked_phrases);
      data.ApplyTech();
      Stats.DesignEvent("Tech:" + tech_id);
    }
  }

  public void UnlockPhrase(string phrase)
  {
    if (this.unlocked_phrases.Contains(phrase))
      return;
    this.unlocked_phrases.Add(phrase);
  }

  public void UnlockCraft(string craft_id)
  {
    if (this.unlocked_crafts.Contains(craft_id))
      return;
    this.unlocked_crafts.Add(craft_id);
  }

  public void LockCraft(string craft_id)
  {
    if (!this.unlocked_crafts.Contains(craft_id))
      return;
    this.unlocked_crafts.Remove(craft_id);
  }

  public void LockCraftForever(string craft_id) => this.locked_crafts.Add(craft_id);

  public void AddPhraseToBlackList(string phrase)
  {
    if (this.black_list_of_phrases.Contains(phrase))
      return;
    this.black_list_of_phrases.Add(phrase);
  }

  public void SetToolbarEquipped(string item_id, int toolbar_index)
  {
    if (toolbar_index < 0 || toolbar_index >= this.equipped_items.Length)
      return;
    this.UnEquip(item_id);
    this.equipped_items[toolbar_index] = item_id;
  }

  public int GetEquippedIndex(string item_id)
  {
    for (int equippedIndex = 0; equippedIndex < this.equipped_items.Length; ++equippedIndex)
    {
      if (this.equipped_items[equippedIndex] == item_id)
        return equippedIndex;
    }
    return -1;
  }

  public void UnEquip(int toolbar_index)
  {
    if (toolbar_index < 0 || toolbar_index >= this.equipped_items.Length)
      return;
    this.equipped_items[toolbar_index] = "";
  }

  public void UnEquip(string item_id)
  {
    for (int index = 0; index < this.equipped_items.Length; ++index)
    {
      if (this.equipped_items[index] == item_id)
        this.equipped_items[index] = "";
    }
  }

  public string GetEquippedItem(int toolbar_index)
  {
    return toolbar_index < 0 || toolbar_index >= this.equipped_items.Length ? "" : this.equipped_items[toolbar_index];
  }

  public void CopyLists(List<string> from, List<string> to)
  {
    foreach (string str1 in from)
    {
      string str2;
      if ((str2 = str1)[0] == '@')
        str2 = str2.Substring(1);
      if (!to.Contains(str2))
        to.Add(str2);
    }
  }

  public bool IsTechBranchVisible(int branch_id)
  {
    if (branch_id == 0)
      return false;
    TechBranchDefinition dataOrNull = GameBalance.me.GetDataOrNull<TechBranchDefinition>(branch_id.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    if (dataOrNull != null)
      return !dataOrNull.is_locked || this.unlocked_tech_branches.Contains(branch_id);
    Debug.LogError((object) $"Not found {typeof (TechBranchDefinition)} with id [{branch_id}]");
    return true;
  }

  public void UnlockTechBranch(int branch_id)
  {
    foreach (BalanceBaseObject balanceBaseObject in GameBalance.me.tech_branches_data)
    {
      if (int.Parse(balanceBaseObject.id, (IFormatProvider) CultureInfo.InvariantCulture) == branch_id)
      {
        this.unlocked_tech_branches.Add(branch_id);
        return;
      }
    }
    Debug.LogError((object) $"Branch with id [{branch_id}] not found");
  }

  public string ToJSON()
  {
    Debug.Log((object) "GameSave.ToJSON");
    this.map.SaveSceneToMe();
    return JsonUtility.ToJson((object) this);
  }

  public byte[] ToBinary()
  {
    Debug.Log((object) "GameSave.ToBinary");
    this.map.SaveSceneToMe();
    byte[] binary = SmartSerializer.Serialize<GameSave>(this);
    GC.Collect();
    return binary;
  }

  public static GameSave FromJSON(string s)
  {
    if (!string.IsNullOrEmpty(s))
      return JsonUtility.FromJson<GameSave>(s);
    Debug.LogError((object) "Error loading an empty save file");
    return (GameSave) null;
  }

  public static GameSave FromBinary(byte[] data) => SmartSerializer.Deserialize<GameSave>(data);

  public static void CreateNewSave(System.Action on_complete)
  {
    Debug.Log((object) nameof (CreateNewSave));
    MainGame.me.save = new GameSave()
    {
      player_position = World.player_default_pos,
      dungeon_seed = UnityEngine.Random.Range(0, 100000)
    };
    LoadingGUI.SetProgressBar(0.2f);
    GJTimer.AddTimer(0.0f, (GJTimer.VoidDelegate) (() =>
    {
      GDPoint.RestoreGDPointsState();
      MainGame.me.save.map.RestoreSceneToInitialState();
      MainGame.me.save.InitPlayersInventory();
      MainGame.me.save.quests.InitQuestSystem();
      LoadingGUI.SetProgressBar(0.3f);
      GJTimer.AddTimer(0.0f, new GJTimer.VoidDelegate(((ExtentionTools) on_complete).TryInvoke));
    }));
  }

  public void InitPlayersInventory()
  {
    Debug.Log((object) nameof (InitPlayersInventory));
    this._inventory = new Item("inventory");
    this._inventory.SetInventorySize(20);
    this._inventory.hp = (float) this.max_hp;
    this._inventory.SetParam("sanity", (float) this.max_sanity);
    this._inventory.SetParam("energy", (float) this.max_energy);
    for (int index = 1; index < 7; ++index)
      this._inventory.SetParam(Sins.SIN_NAMES[index], 250f);
  }

  public void PrepareForSave()
  {
    if ((UnityEngine.Object) MainGame.me.player == (UnityEngine.Object) null)
      return;
    this._inventory = MainGame.me.player.data;
    this.game_version = LazyConsts.VERSION;
    if ((double) this.game_version > (double) this.max_version)
      this.max_version = this.game_version;
    this.player_position = MainGame.me.player.transform.localPosition;
    EnvironmentEngine.me.PrepareForSave();
    this._environment_engine_data = EnvironmentEngine.me.data;
    this._serialized_time_of_day = TimeOfDay.me.ToSerialized();
    WorldMap.ToGameSave(this);
    foreach (GDPoint gdPoint1 in WorldMap.gd_points)
    {
      bool flag = false;
      foreach (GameSave.SavedGDPoint gdPoint2 in this.gd_points)
      {
        if (gdPoint2.gd_tag == gdPoint1.gd_tag)
        {
          gdPoint2.enabled = gdPoint1.gameObject.activeSelf;
          flag = true;
          break;
        }
      }
      if (!flag)
        this.gd_points.Add(new GameSave.SavedGDPoint()
        {
          gd_tag = gdPoint1.gd_tag,
          enabled = gdPoint1.gameObject.activeSelf
        });
    }
    this.unique_id_iterator = UniqueID.GetUniqueID();
  }

  public void PrepareAfterLoad()
  {
    Debug.Log((object) "Save.PrepareAfterLoad");
    EnvironmentEngine.me.DeserializeData(this._environment_engine_data);
    if (this.achievements == null)
      this.achievements = new AchievementsSystem();
    this.achievements.VerifyAndSetMissedAchievements();
    if (this._serialized_time_of_day != null)
    {
      TimeOfDay.me.FromSerialized(this._serialized_time_of_day);
      Debug.Log((object) ("Restoring time: " + this._serialized_time_of_day.time_of_day.ToString()));
      EnvironmentEngine.SetTime(this._serialized_time_of_day.time_of_day);
    }
    else
      TimeOfDay.me.SetTimeK(0.5f);
    if (this.gd_points != null)
    {
      foreach (GameSave.SavedGDPoint gdPoint1 in this.gd_points)
      {
        bool flag = false;
        foreach (GDPoint gdPoint2 in WorldMap.gd_points)
        {
          if (gdPoint1.gd_tag == gdPoint2.gd_tag)
          {
            gdPoint2.gameObject.SetActive(gdPoint1.enabled);
            flag = true;
            break;
          }
        }
        if (!flag)
          Debug.LogError((object) ("Couldn't de-serialize GDPoint with gd_tag = " + gdPoint1.gd_tag));
      }
    }
    GameSettings.me.ApplyVolume();
    if (this.last_bait_baits == null)
      this.last_bait_baits = new List<string>();
    if (this.last_bait_reservoirs == null)
      this.last_bait_reservoirs = new List<string>();
    if (this.last_bait_baits.Count != this.last_bait_reservoirs.Count)
    {
      this.last_bait_baits = new List<string>();
      this.last_bait_reservoirs = new List<string>();
    }
    if (this.known_fishes_clear != null && (this.known_fishes_clear.Count != 0 || this.known_fishes.Count <= 0))
      return;
    this.known_fishes_clear = new List<string>();
    Debug.Log((object) "OLD_SAVE_FIX: Recreating known fishes list");
    foreach (string knownFish in this.known_fishes)
    {
      string[] strArray = knownFish.Split(new string[1]
      {
        ":"
      }, StringSplitOptions.RemoveEmptyEntries);
      string empty = string.Empty;
      if (strArray.Length < 3)
      {
        Debug.LogError((object) $"Wrong known fish: \"{this.known_fishes?.ToString()}\"");
      }
      else
      {
        string str;
        if (strArray.Length > 3)
        {
          str = strArray[2];
          for (int index = 3; index < strArray.Length; ++index)
            str = $"{str}:{strArray[index]}";
        }
        else
          str = strArray[2];
        if (!string.IsNullOrEmpty(str) && !this.known_fishes_clear.Contains(str))
          this.known_fishes_clear.Add(str);
      }
    }
  }

  public void LateSaveFixer()
  {
    int num1 = Mathf.RoundToInt(this.game_version * 1000f);
    if ((double) this.game_version < 1.014)
    {
      if (this.unlocked_techs.Contains("Improvement"))
      {
        this.UnlockCraft("mining_builddesk:p:mf_box_stuff_place");
        this.UnlockCraft("vineyard_builddesk:p:mf_box_stuff_place");
        this.UnlockCraft("graveyard_builddesk:p:mf_box_stuff_place");
        this.UnlockCraft("cremation_builddesk:p:mf_box_stuff_place");
      }
      if (this.unlocked_techs.Contains("Price of faith"))
        this.UnlockPerk("p_cardinal");
      if (this.unlocked_techs.Contains("Strong alcohol"))
      {
        this.UnlockCraft("cellar_builddesk:p:mf_distcube_3_place");
        this.UnlockCraft("booze_from_bottle_apple_braga");
        this.UnlockCraft("booze_from_bottle_berry_braga");
        this.UnlockCraft("booze_from_bottle_red_vine_1");
        this.UnlockCraft("booze_from_bottle_red_vine_2");
        this.UnlockCraft("booze_from_bottle_red_vine_3");
      }
    }
    if ((double) this.game_version < 1021.0 / 1000.0)
    {
      WorldGameObject objectByCustomTag = WorldMap.GetWorldGameObjectByCustomTag("donkey");
      if ((UnityEngine.Object) objectByCustomTag == (UnityEngine.Object) null)
        Debug.LogError((object) "Can't fix donkey stuck: not found donkey!");
      else if ((double) objectByCustomTag.GetParam("donkey_must_wait_at_cemetery") > 0.5 && (objectByCustomTag.custom_interaction_events == null || objectByCustomTag.custom_interaction_events.Count == 0) && (UnityEngine.Object) WorldMap.GetWorldGameObjectByCustomTag("carrot_box", true) == (UnityEngine.Object) null)
      {
        Debug.Log((object) "Fixed donkey stuck");
        objectByCustomTag.FireEvent("add_donkey_strike");
      }
    }
    if ((double) this.game_version < 1023.0 / 1000.0)
    {
      if (this.black_list_of_phrases.Contains("@gypsy_recipe_done_1"))
        this.UnlockTech("Fish shish kebab");
      if (this.black_list_of_phrases.Contains("snake_back_12b"))
        this.UnlockCraft("pail_blood");
    }
    if ((double) this.game_version < 1.026)
    {
      List<WorldGameObject> gameObjectsByObjId = WorldMap.GetWorldGameObjectsByObjId("bush_horizontal_2");
      if (gameObjectsByObjId == null || gameObjectsByObjId.Count == 0)
      {
        Debug.LogError((object) "FATAL ERROR: Not found any \"bush_horizontal_3\"!");
      }
      else
      {
        for (int index = 0; index < gameObjectsByObjId.Count; ++index)
        {
          if (!((UnityEngine.Object) gameObjectsByObjId[index] == (UnityEngine.Object) null))
          {
            Vector3 position = gameObjectsByObjId[index].transform.position;
            if (position.x.EqualsTo(872f, 0.1f) && position.y.EqualsTo(-984f, 0.1f))
            {
              gameObjectsByObjId[index].DestroyMe();
              Debug.Log((object) ("Destroyed redundand bush_horizontal_2 with coords " + position.ToString()));
              break;
            }
          }
        }
      }
    }
    if (num1 <= 1026 && this.unlocked_techs.Contains("Glass-blower 2"))
      this.UnlockPerk("p_t_sand_improve");
    if (num1 <= 1026)
    {
      WorldGameObject gameObjectByObjId = WorldMap.GetWorldGameObjectByObjId("npc_astrologer");
      if ((UnityEngine.Object) gameObjectByObjId != (UnityEngine.Object) null && gameObjectByObjId.GetParamInt("astrologer_2a_check_for_lock") >= 2)
      {
        if (!this.black_list_of_phrases.Contains("@astrologer_2a_1a_1"))
          this.UnlockPhrase("@astrologer_trade");
        if (!this.black_list_of_phrases.Contains("astrologer_2a_1b_6c"))
        {
          MainGame.me.player.DropItem(new Item("quest_key_astrologer"));
          this.SetTaskState("npc_astrologer", "astrologer_diary", KnownNPC.TaskState.State.Visible);
          this.UnlockPhrase("@snake_give_key");
          this.UnlockPhrase("@astrologer_diary");
        }
      }
      List<WorldGameObject> gameObjectsByObjId = WorldMap.GetWorldGameObjectsByObjId("stone_ore");
      bool flag = false;
      for (int index = 0; index < gameObjectsByObjId.Count; ++index)
      {
        if (!((UnityEngine.Object) gameObjectsByObjId[index] == (UnityEngine.Object) null))
        {
          Vector3 position = gameObjectsByObjId[index].transform.position;
          if (position.x.EqualsTo(-3996f, 0.1f) && position.y.EqualsTo(6672f, 0.1f))
          {
            flag = true;
            break;
          }
        }
      }
      if (!flag)
      {
        Debug.Log((object) "Save fix: added \"stone_ore\" to map.");
        WorldMap.SpawnWGO(MainGame.me.world_root, "stone_ore", new Vector3?(new Vector3(-3996f, 6672f, 1389.167f)));
      }
      if (this.black_list_of_phrases.Contains("@horadric_2_1h_4b") && !this.unlocked_phrases.Contains("@actress_2b_1a"))
        this.UnlockPhrase("@actress_2b_1a");
    }
    if (num1 <= 1027 && this.black_list_of_phrases.Contains("@actor_2_1e") && this.black_list_of_phrases.Contains("@actor_2_1d") && (!this.unlocked_phrases.Contains("@actor_2_1e") || !this.unlocked_phrases.Contains("@actor_2_1d")))
      this.UnlockPhrase("@astrologer_2a_1a_1");
    if (num1 <= 1028)
      WorldMap.SpawnWGO(MainGame.me.world_root, "teleport_point", new Vector3?(new Vector3(25728f, -2208f))).custom_tag = "tp_lighthouse_a";
    if (num1 <= 1030)
    {
      if (this.black_list_of_phrases.Contains("@snake_magic_100") && !this.black_list_of_phrases.Contains("@snake_instrument_ready"))
        MainGame.me.player.DropItem(new Item("restoration_instrument"));
      if (this.black_list_of_phrases.Contains("@snake_magic_100"))
        this.black_list_of_phrases.Add("@snake_final_ritual");
    }
    if (num1 <= 1030)
    {
      WorldGameObject gameObjectByObjId = WorldMap.GetWorldGameObjectByObjId("donkey");
      if (gameObjectByObjId.GetParamInt("strike_completed") >= 2)
        gameObjectByObjId.SetParam("get_carrots_not_locked", 1f);
    }
    if (num1 <= 1032)
    {
      float num2 = MainGame.me.player.GetParam("body_max");
      float num3 = MainGame.me.player.GetParam("body_min");
      if (BuffsLogics.FindBuffByID("buff_skull") != null && (double) Math.Abs(num3 - num2) < 0.0099999997764825821 || BuffsLogics.FindBuffByID("buff_skull") == null && (double) num2 < (double) num3 && (double) Math.Abs(num3 - num2) > 0.0099999997764825821)
      {
        MainGame.me.player.SetParam("body_max", num2 + 1f);
        MainGame.me.player.SetParam("cur_bodies_count", 0.0f);
      }
    }
    if (num1 <= 1032 && MainGame.me.save.black_list_of_phrases.Contains("@merchant_2b") && !MainGame.me.save.unlocked_phrases.Contains("@merchant_favore") && !MainGame.me.save.unlocked_phrases.Contains("@merchant_forgive"))
      this.UnlockPhrase("@merchant_favore");
    if (num1 <= 1034 && !MainGame.me.save.unlocked_phrases.Contains("@merchant_2e") && !MainGame.me.save.unlocked_phrases.Contains("@merchant_forgive") && !MainGame.me.save.unlocked_phrases.Contains("@merchant_on_deal_done_4a") && MainGame.me.save.unlocked_phrases.Contains("@merchant_favore"))
    {
      WorldMap.GetWorldGameObjectByObjId("npc_merchant");
      MainGame.me.player.AddToParams("_rel_npc_merchant", 10f);
      this.UnlockPhrase("@merchant_on_deal_done_4a");
      this.AddPhraseToBlackList("@merchant_on_deal_done_4a");
      MainGame.me.save.SetTaskState("npc_merchant", "merchant_trade", KnownNPC.TaskState.State.Visible);
      this.UnlockPhrase("@merchant_traide_license");
    }
    if (num1 <= 1034 && MainGame.me.save.black_list_of_phrases.Contains("@snake_magic_100"))
      WorldMap.GetWorldGameObjectByObjId("npc_cultist").SetParam("lock_spawn_and_despawn", 0.0f);
    if (num1 <= 1034)
    {
      if (MainGame.me.save.unlocked_techs.Contains("Simple gravestones"))
      {
        MainGame.me.save.UnlockCraft("destroy_wd_cross");
        MainGame.me.save.UnlockCraft("destroy_wd_cross_2");
        MainGame.me.save.UnlockCraft("destroy_wd_fence");
        MainGame.me.save.UnlockCraft("destroy_grave_top_stn_plate_1");
      }
      if (MainGame.me.save.unlocked_techs.Contains("Stone gravestones"))
      {
        MainGame.me.save.UnlockCraft("destroy_grave_bot_stn_2");
        MainGame.me.save.UnlockCraft("destroy_grave_top_stella_stn_1");
        MainGame.me.save.UnlockCraft("destroy_grave_top_stn_cross_2");
      }
      if (MainGame.me.save.unlocked_techs.Contains("Grave monuments"))
      {
        MainGame.me.save.UnlockCraft("destroy_grave_top_sculpt_stn_1");
        MainGame.me.save.UnlockCraft("destroy_grave_top_sculpt_stn_2");
      }
    }
    if (num1 <= 1037 && MainGame.me.save.unlocked_crafts.Contains("garden_builddesk:p:packing_table_place"))
      MainGame.me.save.UnlockCraft("mf_wood_builddesk::elevator_place");
    if (num1 <= 1037 && MainGame.me.save.unlocked_techs.Contains("Engineer"))
      MainGame.me.save.UnlockCraft("mf_wood_builddesk::well_pump_place");
    if (num1 < 1100)
      new Scene1037_To_Scene1100().Execute();
    if (num1 == 1100 || num1 == 1101)
    {
      WorldGameObject objectByCustomTag1 = WorldMap.GetWorldGameObjectByCustomTag("steep_yellow_blockage_R_o");
      if ((UnityEngine.Object) objectByCustomTag1 == (UnityEngine.Object) null)
        Debug.LogError((object) "Can't fix zombie: steep not found!");
      else if (objectByCustomTag1.GetParam("allowed_for_player_interaction").EqualsTo(1f) && objectByCustomTag1.GetParam("zombie_was_found").EqualsTo(1f))
      {
        objectByCustomTag1.custom_tag = "steep_yellow_blockage_R_o____";
        WorldGameObject objectByCustomTag2 = WorldMap.GetWorldGameObjectByCustomTag("steep_yellow_blockage_R_o", true);
        if ((UnityEngine.Object) objectByCustomTag2 != (UnityEngine.Object) null)
          objectByCustomTag2.DestroyMe();
        objectByCustomTag1.custom_tag = "steep_yellow_blockage_R_o";
        objectByCustomTag1.hp = 50f;
        objectByCustomTag1.AddInteractionEvent("info");
      }
    }
    if (num1 >= 1100 && num1 <= 1103)
    {
      GDPoint gdPointByGdTag = WorldMap.GetGDPointByGDTag("gd_zmb_sawmill_decor_place", false, false);
      if ((UnityEngine.Object) gdPointByGdTag != (UnityEngine.Object) null)
        WorldMap.SpawnWGO(MainGame.me.world_root, "zombie_sawmill_decor_object", new Vector3?(gdPointByGdTag.pos));
    }
    if (num1 <= 1106)
    {
      List<WorldGameObject> gameObjectsByObjId = WorldMap.GetWorldGameObjectsByObjId("lantern_2");
      bool flag = false;
      foreach (WorldGameObject worldGameObject in gameObjectsByObjId)
      {
        if (!((UnityEngine.Object) worldGameObject == (UnityEngine.Object) null))
        {
          Vector3 position = worldGameObject.transform.position;
          if (position.x.EqualsTo(15928f, 0.1f) && position.y.EqualsTo(-1848f, 0.1f))
          {
            flag = true;
            worldGameObject.DestroyMe();
            break;
          }
        }
      }
      if (flag)
      {
        Debug.Log((object) "Save fix: added \"lantern_2\" to map.");
        WorldMap.SpawnWGO(MainGame.me.world_root, "lantern_2", new Vector3?(new Vector3(15944f, -1912f, -396.0533f))).transform.localScale = new Vector3(-1f, 1f, 1f);
      }
    }
    if (num1 <= 1111)
    {
      foreach (string completedOneTimeCraft in this.completed_one_time_crafts)
      {
        if (completedOneTimeCraft.StartsWith("mix:"))
        {
          string craft_id = "mix";
          string str1 = completedOneTimeCraft;
          char[] chArray = new char[1]{ ':' };
          foreach (string str2 in str1.Split(chArray))
          {
            if (!(str2 == "mix") && !str2.StartsWith("mf_alchemy_craft_0") && !string.IsNullOrEmpty(str2) && !(str2 == "_") && !(str2 == "taste_booster"))
              craft_id = $"{craft_id}_{str2}";
          }
          if (!(craft_id == "mix"))
          {
            this.UnlockCraft(craft_id);
            Debug.Log((object) ("Save fixer: Unlocked zombie alchemy mix craft: " + craft_id));
          }
        }
        else if (completedOneTimeCraft.StartsWith("alch:"))
        {
          string[] strArray = completedOneTimeCraft.Split(':');
          if (strArray.Length >= 3)
          {
            string str3 = "alchemy_";
            string str4;
            switch (strArray[1])
            {
              case "mf_alchemy_mill":
                str4 = str3 + "1_";
                break;
              case "mf_alchemy_stirrer_01":
                str4 = str3 + "2_";
                break;
              case "mf_distcube_2_clay":
              case "mf_distcube_2_cuprum":
                str4 = str3 + "3_";
                break;
              default:
                continue;
            }
            string craft_id = str4 + strArray[2];
            this.UnlockCraft(craft_id);
            Debug.Log((object) ("Save fixer: Unlocked zombie alchemy decompose craft: " + craft_id));
          }
        }
      }
    }
    List<WorldGameObject> gameObjectsByObjId1 = WorldMap.GetWorldGameObjectsByObjId("zombie_sawmill_completed");
    if (gameObjectsByObjId1 != null && gameObjectsByObjId1.Count > 0)
    {
      foreach (WorldGameObject worldGameObject in gameObjectsByObjId1)
      {
        try
        {
          if ((double) ((Vector2) worldGameObject.transform.position - new Vector2(1920f, 3744f)).sqrMagnitude < 10.0)
          {
            if (!worldGameObject.components.craft.is_crafting)
            {
              Debug.LogError((object) "FATAL ERROR: zombie_sawmill_completed has no craft! Fixing in GameSave");
              worldGameObject.TryStartCraft("zombie_sawmill_wood_production");
              break;
            }
            break;
          }
        }
        catch (Exception ex)
        {
          Debug.LogError((object) ex);
        }
      }
    }
    List<WorldGameObject> gameObjectsByObjId2 = WorldMap.GetWorldGameObjectsByObjId("mine_zombie_bench");
    if (gameObjectsByObjId2 != null && gameObjectsByObjId2.Count > 0)
    {
      foreach (WorldGameObject worldGameObject in gameObjectsByObjId2)
      {
        try
        {
          Vector2 position = (Vector2) worldGameObject.transform.position;
          Vector2 vector2 = position - new Vector2(-2560f, 6914f);
          if ((double) vector2.sqrMagnitude >= 10.0)
          {
            vector2 = position - new Vector2(-2254f, 6872f);
            if ((double) vector2.sqrMagnitude >= 10.0)
              continue;
          }
          if (!worldGameObject.components.craft.is_crafting)
          {
            Debug.LogError((object) "FATAL ERROR: mine_zombie_bench has no craft! Fixing in GameSave");
            worldGameObject.TryStartCraft("mine_zombie_bench_iron_production");
          }
        }
        catch (Exception ex)
        {
          Debug.LogError((object) ex);
        }
      }
    }
    List<WorldGameObject> gameObjectsByObjId3 = WorldMap.GetWorldGameObjectsByObjId("zombie_mine_fence_left_front");
    if (gameObjectsByObjId3 != null && gameObjectsByObjId3.Count > 0)
    {
      foreach (WorldGameObject worldGameObject in gameObjectsByObjId3)
      {
        try
        {
          if ((double) ((Vector2) worldGameObject.transform.position - new Vector2(-4128f, 6524f)).sqrMagnitude < 10.0)
          {
            if (!worldGameObject.components.craft.is_crafting)
            {
              Debug.LogError((object) "FATAL ERROR: zombie_mine_fence_left_front has no craft! Fixing in GameSave");
              worldGameObject.TryStartCraft("zombie_mine_stone_production");
            }
          }
        }
        catch (Exception ex)
        {
          Debug.LogError((object) ex);
        }
      }
    }
    List<WorldGameObject> gameObjectsByObjId4 = WorldMap.GetWorldGameObjectsByObjId("zombie_mine_fence_front");
    if (gameObjectsByObjId4 != null && gameObjectsByObjId4.Count > 0)
    {
      foreach (WorldGameObject worldGameObject in gameObjectsByObjId4)
      {
        try
        {
          Vector2 position = (Vector2) worldGameObject.transform.position;
          Vector2 vector2 = position - new Vector2(-3932f, 6622f);
          if ((double) vector2.sqrMagnitude < 10.0)
          {
            if (!worldGameObject.components.craft.is_crafting)
            {
              Debug.LogError((object) "FATAL ERROR: zombie_mine_fence_front has no craft! Fixing in GameSave");
              worldGameObject.TryStartCraft("zombie_mine_stone_production");
            }
          }
          else
          {
            vector2 = position - new Vector2(-3732f, 6616f);
            if ((double) vector2.sqrMagnitude >= 10.0)
            {
              vector2 = position - new Vector2(-3382f, 6622f);
              if ((double) vector2.sqrMagnitude >= 10.0)
                continue;
            }
            if (!worldGameObject.components.craft.is_crafting)
            {
              Debug.LogError((object) "FATAL ERROR: zombie_mine_fence_front has no craft! Fixing in GameSave");
              worldGameObject.TryStartCraft("zombie_mine_marble_production");
            }
          }
        }
        catch (Exception ex)
        {
          Debug.LogError((object) ex);
        }
      }
    }
    if (num1 <= 1111)
    {
      bool flag1 = false;
      bool flag2 = false;
      foreach (Component component in WorldMap.GetWorldGameObjectsByObjId("church_candle"))
      {
        Vector2 position = (Vector2) component.transform.position;
        if ((double) Mathf.Abs((position - new Vector2(7788f, -10224f)).magnitude) < 1.0)
          flag1 = true;
        else if ((double) Mathf.Abs((position - new Vector2(7536f, -10224f)).magnitude) < 1.0)
          flag2 = true;
      }
      if (!flag1)
        WorldMap.SpawnWGO(MainGame.me.world_root, "church_candle", new Vector3?(new Vector3(7788f, -10224f, -2127.544f)));
      if (!flag2)
        WorldMap.SpawnWGO(MainGame.me.world_root, "church_candle", new Vector3?(new Vector3(7536f, -10224f, -2127.597f)));
    }
    if (num1 <= 1111 && MainGame.me.save.black_list_of_phrases.Contains("@horadric_2_1h_4b"))
    {
      MainGame.me.save.LockCraft("stamped_meat");
      if (MainGame.me.save.unlocked_crafts.Contains("stamped_meat_new"))
        MainGame.me.save.LockCraft("insert_stamp_into_table");
      else
        MainGame.me.save.UnlockCraft("insert_stamp_into_table");
    }
    if (num1 <= 1111 && MainGame.me.save.revealed_techs.Contains("Random text generator"))
      MainGame.me.save.RevealHiddenTech("Zombie alchemy");
    if (num1 <= 1111 && MainGame.me.save.unlocked_techs.Contains("Rules of burning"))
    {
      MainGame.me.save.UnlockCraft("mining_builddesk::lantern_network");
      MainGame.me.save.UnlockCraft("mining_builddesk:p:lantern_place");
    }
    if (num1 <= 1111)
    {
      GDPoint gdPointByGdTag = WorldMap.GetGDPointByGDTag("gd_lantern_2");
      if ((UnityEngine.Object) gdPointByGdTag == (UnityEngine.Object) null)
      {
        Debug.LogError((object) "FATAL ERROR: not found GDPoint with custom tag \"gd_lantern_2\"!");
      }
      else
      {
        List<WorldGameObject> wgOs = WorldMap.FindWGOs(new string[4]
        {
          "lantern_place",
          "lantern_1_clone",
          "lantern_2_clone",
          "lantern_3_clone"
        }, (Vector2) gdPointByGdTag.transform.position, string.Empty);
        if (wgOs != null && wgOs.Count > 0)
          Debug.Log((object) "Save fixer: do nothing with lanterns shit");
        else if (MainGame.me.save.completed_one_time_crafts.Contains("mining_builddesk::lantern_network"))
          MainGame.me.save.completed_one_time_crafts.Remove("mining_builddesk::lantern_network");
      }
    }
    WorldGameObject objectByCustomTag3 = WorldMap.GetWorldGameObjectByCustomTag("well_pump", true);
    if ((UnityEngine.Object) objectByCustomTag3 != (UnityEngine.Object) null)
    {
      try
      {
        if (!objectByCustomTag3.components.craft.is_crafting)
        {
          Debug.LogError((object) "FATAL ERROR: well_pump has no craft");
          objectByCustomTag3.TryStartCraft("water_pumping");
        }
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ex);
      }
    }
    WorldGameObject gameObjectByObjId1 = WorldMap.GetWorldGameObjectByObjId("refugee_camp_well", true);
    if ((UnityEngine.Object) gameObjectByObjId1 != (UnityEngine.Object) null)
    {
      try
      {
        if (!gameObjectByObjId1.components.craft.is_crafting)
        {
          Debug.LogError((object) "FATAL ERROR: refugee_camp_well has no craft");
          gameObjectByObjId1.TryStartCraft("refugee_well");
        }
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ex);
      }
    }
    WorldGameObject gameObjectByObjId2 = WorldMap.GetWorldGameObjectByObjId("refugee_camp_well_2", true);
    if ((UnityEngine.Object) gameObjectByObjId2 != (UnityEngine.Object) null)
    {
      try
      {
        if (!gameObjectByObjId2.components.craft.is_crafting)
        {
          Debug.LogError((object) "FATAL ERROR: refugee_camp_well has no craft");
          gameObjectByObjId2.TryStartCraft("refugee_well_2");
        }
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ex);
      }
    }
    foreach (WorldGameObject worldGameObject in WorldMap.GetWorldGameObjectsByObjId("refugee_camp_hive"))
    {
      if ((UnityEngine.Object) worldGameObject != (UnityEngine.Object) null)
      {
        try
        {
          if (!worldGameObject.components.craft.is_crafting)
          {
            Debug.LogError((object) "FATAL ERROR: one of refugee_camp_hive has no craft");
            worldGameObject.TryStartCraft("refugee_honey_production");
          }
        }
        catch (Exception ex)
        {
          Debug.LogError((object) ex);
        }
      }
    }
    if (num1 < 1200)
    {
      if (this.unlocked_techs.Contains("Zombie logistic"))
      {
        List<WorldGameObject> gameObjectsByObjId5 = WorldMap.GetWorldGameObjectsByObjId("wood_obstacle_v");
        if (gameObjectsByObjId5 != null && gameObjectsByObjId5.Count > 0)
        {
          Vector2 vector2 = new Vector2(5738f, 750f);
          foreach (Component component in gameObjectsByObjId5)
          {
            if ((double) ((Vector2) component.transform.position - vector2).magnitude < 1.0)
            {
              this.UnlockCraft("vineyard_builddesk:p:porter_station");
              Debug.Log((object) "Fix for DLC: unlocked Porter Station building on vineyard zone");
              break;
            }
          }
        }
      }
      List<WorldGameObject> gameObjectsByObjId6 = WorldMap.GetWorldGameObjectsByObjId("cellar_builddesk");
      Vector2 vector2_1 = new Vector2(11112f, -9216f);
      Vector2 vector2_2 = new Vector2(11136f, -9216f);
      bool flag = false;
      foreach (WorldGameObject worldGameObject in gameObjectsByObjId6)
      {
        try
        {
          if ((double) ((Vector2) worldGameObject.transform.position - vector2_1).magnitude < 1.0)
          {
            worldGameObject.transform.position = (Vector3) vector2_2;
            flag = true;
          }
        }
        catch (Exception ex)
        {
          Debug.LogError((object) ("SaveFixer exception: " + ex?.ToString()));
        }
        if (flag)
          break;
      }
      WorldMap.SpawnWGO(MainGame.me.world_root, "wall_cellar_1tile", new Vector3?((Vector3) new Vector2(11040f, -9216f))).custom_tag = "cellar_porter_station_2";
      WorldMap.SpawnWGO(MainGame.me.world_root, "teleport_point", new Vector3?((Vector3) new Vector2(9840f, -14720f))).custom_tag = "tp_adam_b";
      WorldMap.SpawnWGO(MainGame.me.world_root, "teleport_inside", new Vector3?((Vector3) new Vector2(9840f, -14745f))).custom_tag = "tp_adam_a_";
      WorldMap.SpawnWGO(MainGame.me.world_root, "teleport_point", new Vector3?((Vector3) new Vector2(14049.6f, -2829.6f))).custom_tag = "tp_adam_a";
      WorldMap.SpawnWGO(MainGame.me.world_root, "teleport_outside", new Vector3?((Vector3) new Vector2(14049.6f, -2793.6f))).custom_tag = "tp_adam_b_";
      WorldGameObject worldGameObject1 = WorldMap.SpawnWGO(MainGame.me.world_root, "chest", new Vector3?((Vector3) new Vector2(9608f, -14424f)));
      worldGameObject1.custom_tag = "adams_house_chest";
      worldGameObject1.AddToInventory("tr_key", 1);
      worldGameObject1.AddToInventory("ceramic_1", 20);
      worldGameObject1.AddToInventory("ceramic_2", 5);
      worldGameObject1.AddToInventory("ceramic_3", 3);
      worldGameObject1.AddToInventory("meal:beet_slice", 1);
      worldGameObject1.AddToInventory("dessert:jelly_red", 3);
      worldGameObject1.AddToInventory("bottle_berry_juice", 2);
      worldGameObject1.AddToInventory("hammer_0", 1);
      WorldMap.SpawnWGO(MainGame.me.world_root, "teleport_point", new Vector3?((Vector3) new Vector2(19398f, -8760f))).custom_tag = "tp_tavern_from_cellar_b";
      WorldMap.SpawnWGO(MainGame.me.world_root, "teleport_micro", new Vector3?((Vector3) new Vector2(19398f, -8688f))).custom_tag = "tp_tavern_to_cellar_b_";
      WorldMap.SpawnWGO(MainGame.me.world_root, "teleport_point", new Vector3?((Vector3) new Vector2(16944f, -8928f))).custom_tag = "tp_tavern_to_cellar_b";
      WorldMap.SpawnWGO(MainGame.me.world_root, "teleport_inside", new Vector3?((Vector3) new Vector2(16944f, -8976f))).custom_tag = "tp_tavern_from_cellar_b_";
      WorldMap.SpawnWGO(MainGame.me.world_root, "players_tavern_builddesk", new Vector3?((Vector3) new Vector2(19872f, -8736f))).custom_tag = "players_tavern_builddesk";
      WorldMap.SpawnWGO(MainGame.me.world_root, "players_tavern_cellar_builddesk", new Vector3?((Vector3) new Vector2(16800f, -8736f))).custom_tag = "players_tavern_cellar_builddesk";
      WorldMap.SpawnWGO(MainGame.me.world_root, "teleport_inside", new Vector3?((Vector3) new Vector2(12912f, -15312f))).custom_tag = "tp_farmer_a_";
      WorldMap.SpawnWGO(MainGame.me.world_root, "teleport_point", new Vector3?((Vector3) new Vector2(12912f, -15312f))).custom_tag = "tp_farmer_b";
      WorldMap.SpawnWGO(MainGame.me.world_root, "teleport_outside", new Vector3?((Vector3) new Vector2(12420f, -3684f))).custom_tag = "tp_farmer_b_";
      WorldMap.SpawnWGO(MainGame.me.world_root, "teleport_point", new Vector3?((Vector3) new Vector2(12420f, -3684f))).custom_tag = "tp_farmer_a";
      WorldGameObject worldGameObject2 = WorldMap.SpawnWGO(MainGame.me.world_root, "chest", new Vector3?((Vector3) new Vector2(12734f, -14424f)));
      worldGameObject2.custom_tag = "farmers_house_chest";
      worldGameObject2.AddToInventory("tr_helmet", 1);
      worldGameObject2.AddToInventory("onion_seed:2", 20);
      worldGameObject2.AddToInventory("carrot_seed", 14);
      worldGameObject2.AddToInventory("pumpkin_crop:2", 16 /*0x10*/);
      worldGameObject2.AddToInventory("lentils_crop:2", 12);
      worldGameObject2.AddToInventory("meal:soup_red_yellow:2", 3);
      worldGameObject2.AddToInventory("meal:soup_yellow_green:2", 1);
      worldGameObject2.AddToInventory("meal:baked_pumpkin:3", 2);
      worldGameObject2.AddToInventory("sack_clock_gold", 6);
      worldGameObject2.AddToInventory("sack_star_gold", 4);
      WorldMap.SpawnWGO(MainGame.me.world_root, "tavern_kitchen", new Vector3?((Vector3) new Vector2(17184f, -8832f))).custom_tag = "tavern_kitchen";
      WorldMap.SpawnWGO(MainGame.me.world_root, "tavern_oven", new Vector3?((Vector3) new Vector2(17184f, -8832f))).custom_tag = "tavern_oven";
      WorldMap.SpawnWGO(MainGame.me.world_root, "tavern_time_machin_wall_inactive", new Vector2(16176f, -9024f), "tavern_time_machine");
      WorldMap.SpawnWGO(MainGame.me.world_root, "tavern_cellar_rack", new Vector2(16588f, -9090f), "tavern_cellar_rack");
      List<string> collection = new List<string>();
      foreach (string unlockedCraft in this.unlocked_crafts)
      {
        CraftDefinition data = GameBalance.me.GetData<CraftDefinition>(unlockedCraft);
        if (data != null && (data.craft_in.Contains("oven") || data.craft_in.Contains("cooking_table") || data.craft_in.Contains("cooking_table_2") || data.craft_in.Contains("cooking_bonfire")))
        {
          string id = "t_" + unlockedCraft;
          if (GameBalance.me.GetDataOrNull<CraftDefinition>(id) != null)
            collection.Add(id);
        }
      }
      if (collection.Count > 0)
        this.unlocked_crafts.AddRange((IEnumerable<string>) collection);
      WorldGameObject worldGameObject3 = (WorldGameObject) null;
      Vector2 vector2_3 = new Vector2(11280f, -1152f);
      foreach (WorldGameObject worldGameObject4 in WorldMap.GetWorldGameObjectsByObjId("grave_ground"))
      {
        if ((double) ((Vector2) worldGameObject4.transform.position - vector2_3).sqrMagnitude < 1.0)
        {
          worldGameObject3 = worldGameObject4;
          break;
        }
      }
      if ((UnityEngine.Object) worldGameObject3 == (UnityEngine.Object) null)
      {
        Debug.Log((object) "Not found Bella's grave in the world.");
      }
      else
      {
        Item bodyFromInventory = worldGameObject3.GetBodyFromInventory();
        if (bodyFromInventory != null && bodyFromInventory.IsNotEmpty())
        {
          if (bodyFromInventory.inventory_size < 99)
            bodyFromInventory.inventory_size = 99;
          if (bodyFromInventory.inventory.Count == 0 || bodyFromInventory.inventory_size <= bodyFromInventory.inventory.Count)
          {
            string[] strArray = new string[8]
            {
              "skull",
              "bone",
              "flesh",
              "blood",
              "skin",
              "brain:brain_4_1",
              "heart:heart_4_1",
              "intestine:intestine_0_0"
            };
            foreach (string item_id in strArray)
            {
              Item obj = new Item(item_id, 1);
              if (obj?.definition != null)
                bodyFromInventory.AddItem(obj);
            }
            Debug.Log((object) "SaveFixer: fixed Bella's body");
          }
        }
      }
    }
    if (num1 < 1123)
    {
      foreach (WorldGameObject worldGameObject in WorldMap.GetWorldGameObjectsByObjId("grave_ground"))
      {
        Item obj = worldGameObject.data.inventory.Find((Predicate<Item>) (body => body.id == nameof (body)));
        if (obj != null)
          obj.inventory_size = 99;
      }
    }
    if (num1 < 1202)
    {
      foreach (Component componentsInChild in MainGame.me.world_root.GetComponentsInChildren<GDPoint>(true))
      {
        WorldGameObject[] componentsInParent = componentsInChild.GetComponentsInParent<WorldGameObject>(true);
        if (componentsInParent != null && componentsInParent.Length != 0)
        {
          componentsInParent[0].Redraw(force_redraw_part: true);
          Debug.Log((object) ("Fixed object with GDPoint inside: " + componentsInParent[0].name));
        }
      }
      GJTimer.AddTimer(0.1f, (GJTimer.VoidDelegate) (() => WorldMap.RescanGDPoints()));
    }
    if (num1 < 1202 && this.unlocked_techs.Contains("Zombie logistic") && !this.unlocked_crafts.Contains("vineyard_builddesk:p:porter_station"))
    {
      List<WorldGameObject> gameObjectsByObjId7 = WorldMap.GetWorldGameObjectsByObjId("wood_obstacle_v");
      bool flag = false;
      if (gameObjectsByObjId7 != null && gameObjectsByObjId7.Count > 0)
      {
        Vector2 vector2 = new Vector2(5738f, 750f);
        foreach (Component component in gameObjectsByObjId7)
        {
          if ((double) ((Vector2) component.transform.position - vector2).magnitude < 1.0)
          {
            flag = true;
            break;
          }
        }
      }
      if (!flag)
      {
        this.UnlockCraft("vineyard_builddesk:p:porter_station");
        Debug.Log((object) "Fix for DLC: unlocked Porter Station building on vineyard zone");
      }
    }
    if (num1 < 1205 && this.revealed_techs.Contains("Zombie mining"))
    {
      this.RevealHiddenTech("Zombie vineyard");
      this.RevealHiddenTech("Zombie brewing");
      this.RevealHiddenTech("Zombie winemaking");
      Debug.Log((object) "Revealed zombie technologies: Zombie vineyard, Zombie brewing, Zombie winemaking");
    }
    WorldGameObject objectByCustomTag4 = WorldMap.GetWorldGameObjectByCustomTag("refugee_camp_cooking_table", true);
    if ((UnityEngine.Object) objectByCustomTag4 != (UnityEngine.Object) null)
    {
      objectByCustomTag4.GetComponent<ChunkedGameObject>().always_active = true;
      objectByCustomTag4.SetActive(true);
    }
    if (this.unlocked_techs.Contains("Persistence") && !this.buffs.Exists((Predicate<PlayerBuff>) (p => p.buff_id == "buff_dlc_refugee_persistence")))
      BuffsLogics.AddBuff("buff_dlc_refugee_persistence");
    if (num1 <= 1301)
    {
      GameSave save = MainGame.me.save;
      AchievementsSystem achievements = MainGame.me.save.achievements;
      if (save.unlocked_phrases.Contains("@zone_refugees_camp_tp"))
      {
        AchievementDefinition dataOrNull = GameBalance.me.GetDataOrNull<AchievementDefinition>("dlc_refugees_s1");
        if (dataOrNull != null)
          PlatformSpecific.OnAchievementComplete(dataOrNull);
      }
      KnownNPC npc1 = this.known_npcs.GetNPC("npc_tavern owner");
      if (npc1 != null)
      {
        switch (npc1.GetQuestState("s_ev_5_find_the_vampire"))
        {
          case KnownNPC.TaskState.State.Visible:
          case KnownNPC.TaskState.State.Complete:
            AchievementDefinition dataOrNull1 = GameBalance.me.GetDataOrNull<AchievementDefinition>("dlc_refugees_s5");
            if (dataOrNull1 != null)
            {
              PlatformSpecific.OnAchievementComplete(dataOrNull1);
              break;
            }
            break;
        }
      }
      KnownNPC npc2 = this.known_npcs.GetNPC("npc_master_alarich");
      if (npc2 != null && npc2.GetQuestState("s_ev_9_2_night_wait") == KnownNPC.TaskState.State.Complete)
      {
        AchievementDefinition dataOrNull2 = GameBalance.me.GetDataOrNull<AchievementDefinition>("dlc_refugees_s10");
        if (dataOrNull2 != null)
          PlatformSpecific.OnAchievementComplete(dataOrNull2);
      }
      KnownNPC npc3 = this.known_npcs.GetNPC("npc_master_alarich");
      if (npc3 != null)
      {
        switch (npc3.GetQuestState("s_ev_12_details"))
        {
          case KnownNPC.TaskState.State.Visible:
          case KnownNPC.TaskState.State.Complete:
            AchievementDefinition dataOrNull3 = GameBalance.me.GetDataOrNull<AchievementDefinition>("dlc_refugees_s12");
            if (dataOrNull3 != null)
            {
              PlatformSpecific.OnAchievementComplete(dataOrNull3);
              break;
            }
            break;
        }
      }
      KnownNPC npc4 = this.known_npcs.GetNPC("npc_marquis_teodoro_jr");
      if (npc4 != null)
      {
        switch (npc4.GetQuestState("s_ev_15_1_teodoro_inform"))
        {
          case KnownNPC.TaskState.State.Visible:
          case KnownNPC.TaskState.State.Complete:
            AchievementDefinition dataOrNull4 = GameBalance.me.GetDataOrNull<AchievementDefinition>("dlc_refugees_s15_1");
            if (dataOrNull4 != null)
            {
              PlatformSpecific.OnAchievementComplete(dataOrNull4);
              break;
            }
            break;
        }
      }
      KnownNPC npc5 = this.known_npcs.GetNPC("npc_master_alarich");
      if (npc5 != null && npc5.GetQuestState("s_ev_12_bring_blood") == KnownNPC.TaskState.State.Complete)
      {
        AchievementDefinition dataOrNull5 = GameBalance.me.GetDataOrNull<AchievementDefinition>("dlc_refugees_s15_2");
        if (dataOrNull5 != null)
          PlatformSpecific.OnAchievementComplete(dataOrNull5);
      }
      if (save.black_list_of_phrases.Contains("@refugees_s15_3"))
      {
        AchievementDefinition dataOrNull6 = GameBalance.me.GetDataOrNull<AchievementDefinition>("dlc_refugees_s15_3");
        if (dataOrNull6 != null)
          PlatformSpecific.OnAchievementComplete(dataOrNull6);
      }
      if (save.unlocked_phrases.Contains("@refugees_s19_1"))
      {
        AchievementDefinition dataOrNull7 = GameBalance.me.GetDataOrNull<AchievementDefinition>("dlc_refugees_s18");
        if (dataOrNull7 != null)
          PlatformSpecific.OnAchievementComplete(dataOrNull7);
      }
      if (save.unlocked_phrases.Contains("@refugees_s24_1"))
      {
        AchievementDefinition dataOrNull8 = GameBalance.me.GetDataOrNull<AchievementDefinition>("dlc_refugees_s23");
        if (dataOrNull8 != null)
          PlatformSpecific.OnAchievementComplete(dataOrNull8);
      }
      if (MainGame.me.player.GetParamInt("ev_28_golem_killed_count") >= 3)
      {
        AchievementDefinition dataOrNull9 = GameBalance.me.GetDataOrNull<AchievementDefinition>("dlc_refugees_s28");
        if (dataOrNull9 != null)
          PlatformSpecific.OnAchievementComplete(dataOrNull9);
      }
      if (save.unlocked_phrases.Contains("@refugee_s34"))
      {
        AchievementDefinition dataOrNull10 = GameBalance.me.GetDataOrNull<AchievementDefinition>("dlc_refugees_s33");
        if (dataOrNull10 != null)
          PlatformSpecific.OnAchievementComplete(dataOrNull10);
      }
      if (MainGame.me.player.GetParamInt("event_s38_complete") == 1)
      {
        AchievementDefinition dataOrNull11 = GameBalance.me.GetDataOrNull<AchievementDefinition>("\tdlc_refugees_s37");
        if (dataOrNull11 != null)
          PlatformSpecific.OnAchievementComplete(dataOrNull11);
      }
      if (save.black_list_of_phrases.Contains("refugee_s40_12"))
      {
        AchievementDefinition dataOrNull12 = GameBalance.me.GetDataOrNull<AchievementDefinition>("dlc_refugees_s40");
        if (dataOrNull12 != null)
          PlatformSpecific.OnAchievementComplete(dataOrNull12);
      }
      if (save.black_list_of_phrases.Contains("@refugee_s45") || save.black_list_of_phrases.Contains("@refugee_s53"))
      {
        AchievementDefinition dataOrNull13 = GameBalance.me.GetDataOrNull<AchievementDefinition>("dlc_refugees_s46_or_s53");
        if (dataOrNull13 != null)
          PlatformSpecific.OnAchievementComplete(dataOrNull13);
      }
      KnownNPC npc6 = this.known_npcs.GetNPC("donkey");
      if (npc6 != null)
      {
        switch (npc6.GetQuestState("dlc_refugees_s_ev_d1_box"))
        {
          case KnownNPC.TaskState.State.Visible:
          case KnownNPC.TaskState.State.Complete:
            AchievementDefinition dataOrNull14 = GameBalance.me.GetDataOrNull<AchievementDefinition>("dlc_refugees_d1");
            if (dataOrNull14 != null)
            {
              PlatformSpecific.OnAchievementComplete(dataOrNull14);
              break;
            }
            break;
        }
      }
      KnownNPC npc7 = this.known_npcs.GetNPC("donkey");
      if (npc7 != null)
      {
        switch (npc7.GetQuestState("dlc_refugees_s_ev_d3_aphorisms"))
        {
          case KnownNPC.TaskState.State.Visible:
          case KnownNPC.TaskState.State.Complete:
            AchievementDefinition dataOrNull15 = GameBalance.me.GetDataOrNull<AchievementDefinition>("dlc_refugees_d3");
            if (dataOrNull15 != null)
            {
              PlatformSpecific.OnAchievementComplete(dataOrNull15);
              break;
            }
            break;
        }
      }
      KnownNPC npc8 = this.known_npcs.GetNPC("donkey");
      if (npc8 != null)
      {
        switch (npc8.GetQuestState("dlc_refugees_s_ev_d6_battle_supply"))
        {
          case KnownNPC.TaskState.State.Visible:
          case KnownNPC.TaskState.State.Complete:
            AchievementDefinition dataOrNull16 = GameBalance.me.GetDataOrNull<AchievementDefinition>("dlc_refugees_d6");
            if (dataOrNull16 != null)
            {
              PlatformSpecific.OnAchievementComplete(dataOrNull16);
              break;
            }
            break;
        }
      }
      KnownNPC npc9 = this.known_npcs.GetNPC("donkey");
      if (npc9 != null)
      {
        switch (npc9.GetQuestState("dlc_refugees_s_ev_d11_cans"))
        {
          case KnownNPC.TaskState.State.Visible:
          case KnownNPC.TaskState.State.Complete:
            AchievementDefinition dataOrNull17 = GameBalance.me.GetDataOrNull<AchievementDefinition>("dlc_refugees_d10");
            if (dataOrNull17 != null)
            {
              PlatformSpecific.OnAchievementComplete(dataOrNull17);
              break;
            }
            break;
        }
      }
      WorldGameObject objectByCustomTag5 = WorldMap.GetWorldGameObjectByCustomTag("donkey");
      if ((UnityEngine.Object) objectByCustomTag5 != (UnityEngine.Object) null && objectByCustomTag5.GetParamInt("is_d16_completed") == 1)
      {
        AchievementDefinition dataOrNull18 = GameBalance.me.GetDataOrNull<AchievementDefinition>("dlc_refugees_d16");
        if (dataOrNull18 != null)
          PlatformSpecific.OnAchievementComplete(dataOrNull18);
      }
      if (MainGame.me.player.GetParamInt("has_witchers_eye") == 1)
      {
        AchievementDefinition dataOrNull19 = GameBalance.me.GetDataOrNull<AchievementDefinition>("dlc_refugees_city_1_fin");
        if (dataOrNull19 != null)
          PlatformSpecific.OnAchievementComplete(dataOrNull19);
      }
      if (MainGame.me.player.GetParamInt("is_citybuilder_fin_2_completed") == 1)
      {
        AchievementDefinition dataOrNull20 = GameBalance.me.GetDataOrNull<AchievementDefinition>("dlc_refugees_city_2_fin");
        if (dataOrNull20 != null)
          PlatformSpecific.OnAchievementComplete(dataOrNull20);
      }
      if (MainGame.me.player.GetParamInt("event_city_3_3_complete") == 1)
      {
        AchievementDefinition dataOrNull21 = GameBalance.me.GetDataOrNull<AchievementDefinition>("dlc_refugees_city_3_fin");
        if (dataOrNull21 != null)
          PlatformSpecific.OnAchievementComplete(dataOrNull21);
      }
      if (save.unlocked_phrases.Contains("@cook_vendor_inventory_1"))
      {
        AchievementDefinition dataOrNull22 = GameBalance.me.GetDataOrNull<AchievementDefinition>("dlc_refugees_cook");
        if (dataOrNull22 != null)
          PlatformSpecific.OnAchievementComplete(dataOrNull22);
      }
      if (save.unlocked_phrases.Contains("@advanced_gravestones"))
      {
        AchievementDefinition dataOrNull23 = GameBalance.me.GetDataOrNull<AchievementDefinition>("dlc_refugees_undertaker");
        if (dataOrNull23 != null)
          PlatformSpecific.OnAchievementComplete(dataOrNull23);
      }
      if (save.unlocked_phrases.Contains("@tech_bag_alchemy"))
      {
        AchievementDefinition dataOrNull24 = GameBalance.me.GetDataOrNull<AchievementDefinition>("dlc_refugees_tanner");
        if (dataOrNull24 != null)
          PlatformSpecific.OnAchievementComplete(dataOrNull24);
      }
    }
    if (num1 <= 1301 && MainGame.me.save.black_list_of_phrases.Contains("@refugees_s17"))
      MainGame.me.save.UnlockPhrase("@buy_memory_powder");
    if (num1 <= 1302)
    {
      KnownNPC npc = this.known_npcs.GetNPC("donkey");
      if (npc != null)
      {
        KnownNPC.TaskState.State questState1 = npc.GetQuestState("s_ev_22_make_amulet");
        KnownNPC.TaskState.State questState2 = npc.GetQuestState("s_ev_21_take_hair");
        if ((questState1 == KnownNPC.TaskState.State.Complete || questState1 == KnownNPC.TaskState.State.Visible) && questState2 == KnownNPC.TaskState.State.Visible)
          npc.SetQuestState("s_ev_21_take_hair", KnownNPC.TaskState.State.Complete);
      }
    }
    if (num1 <= 1302)
    {
      KnownNPC npc = this.known_npcs.GetNPC("npc_marquis_teodoro_jr");
      if (npc != null && npc.GetQuestState("dlc_refugees_s_ev_68_teodoro") == KnownNPC.TaskState.State.Visible && MainGame.me.player.GetParamInt("is_need_activate_ev_s51") == 0)
        GS.RunFlowScript("refugee_ev_s51_prepare");
    }
    if (num1 <= 1303)
    {
      GDPoint gdPointByGdTag = WorldMap.GetGDPointByGDTag("gd_refugees_camp_garden");
      if ((UnityEngine.Object) gdPointByGdTag != (UnityEngine.Object) null && !gdPointByGdTag.IsDisabled())
      {
        List<WorldGameObject> wgOs = WorldMap.FindWGOs(new string[1]
        {
          "bush_6"
        }, new Vector2(3744f, 4428f), string.Empty);
        if (wgOs != null && wgOs.Count != 0)
          wgOs[0].DestroyMe();
      }
    }
    if (num1 <= 1304)
    {
      foreach (string unlockedTech in this.unlocked_techs)
      {
        TechDefinition data = GameBalance.me.GetData<TechDefinition>(unlockedTech);
        if (data != null && data.invisible && !this.visible_techs.Contains(unlockedTech))
          this.visible_techs.Add(unlockedTech);
      }
    }
    if (num1 <= 1304)
    {
      WorldGameObject gameObjectByObjId3 = WorldMap.GetWorldGameObjectByObjId("golem_ev_28_red", true);
      if ((UnityEngine.Object) gameObjectByObjId3 != (UnityEngine.Object) null)
      {
        GDPoint gdPointByGdTag = WorldMap.GetGDPointByGDTag("refugee_s28_golem_red_anchor");
        gameObjectByObjId3.transform.position = gdPointByGdTag.transform.position;
      }
    }
    if (num1 <= 1304)
    {
      KnownNPC npc10 = this.known_npcs.GetNPC("npc_marquis_teodoro_jr");
      KnownNPC npc11 = this.known_npcs.GetNPC("npc_master_alarich");
      if (npc10 != null && npc11 != null && npc11.GetQuestState("s_ev_8_alarich") == KnownNPC.TaskState.State.Complete)
        npc10.SetQuestState("s_ev_8_teodoro", KnownNPC.TaskState.State.Complete);
    }
    if (num1 <= 1304)
    {
      KnownNPC npc = this.known_npcs.GetNPC("npc_hunchback");
      if (npc != null && MainGame.me.save.black_list_of_phrases.Contains("@refugee_s42_1_3"))
        npc.SetQuestState("s_ev_41_koukol", KnownNPC.TaskState.State.Complete);
    }
    WorldGameObject gameObjectByObjId4 = WorldMap.GetWorldGameObjectByObjId("church_big_quality_obj", true);
    if ((UnityEngine.Object) gameObjectByObjId4 != (UnityEngine.Object) null)
    {
      gameObjectByObjId4.DestroyMe();
      Debug.Log((object) "WGO church_big_quality_obj was destroyed");
    }
    if (num1 <= 1305)
    {
      WorldGameObject objectByCustomTag6 = WorldMap.GetWorldGameObjectByCustomTag("npc_mrs chain");
      GDPoint gdPointByGdTag = WorldMap.GetGDPointByGDTag("refugee_s34_chain_point_1");
      if ((UnityEngine.Object) objectByCustomTag6 != (UnityEngine.Object) null && (UnityEngine.Object) gdPointByGdTag != (UnityEngine.Object) null && objectByCustomTag6.GetParamInt("s34_gd_zone_active") == 1)
      {
        objectByCustomTag6.SetParam("is_busy", 1f);
        if (objectByCustomTag6.transform.position != gdPointByGdTag.transform.position)
        {
          objectByCustomTag6.components.character?.StopMovement();
          objectByCustomTag6.TeleportToGDPoint("refugee_s34_chain_point_1");
          objectByCustomTag6.components.character?.LookAt(Direction.Right);
        }
      }
    }
    if (num1 <= 1307)
    {
      KnownNPC npc = this.known_npcs.GetNPC("npc_marquis_teodoro_jr");
      if (MainGame.me.save.black_list_of_phrases.Contains("@refugees_s26_1_ready") && npc != null)
        npc.SetQuestState("s_ev_26_3_teodoro_ask", KnownNPC.TaskState.State.Complete);
    }
    foreach (WorldGameObject worldGameObject in WorldMap.GetWorldGameObjectsByObjId("worker_invisible"))
    {
      if ((UnityEngine.Object) worldGameObject.linked_workbench == (UnityEngine.Object) null)
      {
        Debug.Log((object) ("Destroyed invisible worker without workbench: " + worldGameObject?.ToString()));
        worldGameObject.DestroyMe();
      }
    }
    List<WorldGameObject> gameObjectsByObjId8 = WorldMap.GetWorldGameObjectsByObjId("refugee_camp_garden_bed_1");
    gameObjectsByObjId8.AddRange((IEnumerable<WorldGameObject>) WorldMap.GetWorldGameObjectsByObjId("refugee_camp_garden_bed_2"));
    gameObjectsByObjId8.AddRange((IEnumerable<WorldGameObject>) WorldMap.GetWorldGameObjectsByObjId("refugee_camp_garden_bed_3"));
    foreach (WorldGameObject workbench in gameObjectsByObjId8)
    {
      if (!workbench.has_linked_worker)
      {
        if (!workbench.gameObject.activeSelf)
          workbench.gameObject.SetActive(true);
        bool flag = WorldMap.AttachInvisibleWorker(workbench, out WorldGameObject _);
        Debug.Log((object) $"Reattached Invisible worker for: {workbench?.ToString()}, is success: {flag.ToString()}");
      }
    }
    if (num1 <= 1309)
    {
      GDPoint gdPointByGdTag1 = WorldMap.GetGDPointByGDTag("gd_flat_under_waterflow_3_before_refugee");
      GDPoint gdPointByGdTag2 = WorldMap.GetGDPointByGDTag("gd_refugee_buildzone");
      if (MainGame.me.save.unlocked_phrases.Contains("@zone_refugees_camp_tp"))
      {
        if (gdPointByGdTag1.gameObject.activeSelf)
        {
          WorldZone.GetZoneByID("flat_under_waterflow_3").DisableWorldZone();
          gdPointByGdTag1.gameObject.SetActive(false);
        }
        gdPointByGdTag2.gameObject.SetActive(true);
        WorldZone.GetZoneByID("refugees_camp").EnableWorldZone();
      }
      else
      {
        if (gdPointByGdTag2.gameObject.activeSelf)
        {
          WorldZone.GetZoneByID("refugees_camp").DisableWorldZone();
          gdPointByGdTag2.gameObject.SetActive(false);
        }
        gdPointByGdTag1.gameObject.SetActive(true);
        WorldZone.GetZoneByID("flat_under_waterflow_3").EnableWorldZone();
      }
      MainGame.me.save.black_list_of_phrases.Remove("mailbox_aristocrat_paper");
      for (int index = 0; index < DropsList.me.drops.Count; ++index)
      {
        DropResGameObject drop = DropsList.me.drops[index];
        if (drop.res.id == "stone" || drop.res.id == "marble")
        {
          Vector2 position = (Vector2) drop.transform.position;
          if ((double) position.x >= -5096.0 && (double) position.x <= -3075.0 && (double) position.y >= 6680.0 && (double) position.y <= 19476.0)
          {
            Debug.Log((object) $"Destroy drop: {drop.res.id} with position: {position.ToString()}");
            UnityEngine.Object.Destroy((UnityEngine.Object) drop);
            DropsList.me.drops.Remove(drop);
            --index;
          }
        }
      }
    }
    if (num1 <= 1310)
    {
      WorldMap.SpawnWGO(MainGame.me.world_root, "souls_zone_wall_closed", new Vector2(10752f, -11040f), "souls_zone_wall_closed");
      WorldMap.SpawnWGO(MainGame.me.world_root, "smilers_box_closed", new Vector3?((Vector3) new Vector2(11280f, -10808f)));
      WorldMap.SpawnWGO(MainGame.me.world_root, "pile_of_broken_glass_3", new Vector3?((Vector3) new Vector2(11054f, -10910f)));
      WorldMap.SpawnWGO(MainGame.me.world_root, "pile_of_broken_glass_2", new Vector3?((Vector3) new Vector2(12038f, -11472f)));
      WorldMap.SpawnWGO(MainGame.me.world_root, "pile_of_broken_glass_1", new Vector3?((Vector3) new Vector2(11970f, -10966f)));
      WorldMap.SpawnWGO(MainGame.me.world_root, "pile_of_broken_glass_6", new Vector3?((Vector3) new Vector2(11618f, -11336f)));
      WorldMap.SpawnWGO(MainGame.me.world_root, "pile_of_broken_glass_5", new Vector3?((Vector3) new Vector2(12376f, -10918f)));
      WorldMap.SpawnWGO(MainGame.me.world_root, "pile_of_broken_glass_4", new Vector3?((Vector3) new Vector2(11088f, -11502f)));
      WorldMap.SpawnWGO(MainGame.me.world_root, "hatch_rust", new Vector3?((Vector3) new Vector2(11520f, -10944f)));
      WorldMap.SpawnWGO(MainGame.me.world_root, "soul_healer_broken", new Vector3?((Vector3) new Vector2(12192f, -11092f)));
      WorldMap.SpawnWGO(MainGame.me.world_root, "souls_builddesk", new Vector3?((Vector3) new Vector2(11058f, -10748f)));
      WorldMap.SpawnWGO(MainGame.me.world_root, "soul_portal_broken", new Vector3?((Vector3) new Vector2(12078f, -11344f)));
      WorldMap.SpawnWGO(MainGame.me.world_root, "soul_extractor_broken", new Vector2(11334f, -11334f), "soul_extractor");
      WorldMap.SpawnWGO(MainGame.me.world_root, "candelabrum_3_3_souls", new Vector3?((Vector3) new Vector2(12332f, -11518f)));
      WorldMap.SpawnWGO(MainGame.me.world_root, "dungeon_source_diamond", new Vector3?((Vector3) new Vector2(11802f, -11514f)));
      WorldMap.SpawnWGO(MainGame.me.world_root, "eurics_room_old_bed", new Vector3?((Vector3) new Vector2(20064f, -11904f)));
      WorldMap.SpawnWGO(MainGame.me.world_root, "eurics_room_table_broken", new Vector3?((Vector3) new Vector2(20516f, -11536f)));
      WorldMap.SpawnWGO(MainGame.me.world_root, "eurics_room_woodbox_abandoned", new Vector3?((Vector3) new Vector2(20304f, -11544f)));
      WorldMap.SpawnWGO(MainGame.me.world_root, "eurics_room_side_rack_destroyed", new Vector3?((Vector3) new Vector2(20568f, -11808f)));
      WorldMap.SpawnWGO(MainGame.me.world_root, "eurics_room_front_wardrobe", new Vector2(20208f, -12048f), "eurics_room_front_wardrobe_left");
      WorldMap.SpawnWGO(MainGame.me.world_root, "eurics_room_front_wardrobe", new Vector2(20400f, -12048f), "eurics_room_front_wardrobe_right");
      WorldMap.SpawnWGO(MainGame.me.world_root, "eurics_room_carpet_destroyed", new Vector3?((Vector3) new Vector2(20208f, -11928f))).SetVariationByIndex(0);
      WorldMap.SpawnWGO(MainGame.me.world_root, "stained_glass_window", new Vector3?((Vector3) new Vector2(20254f, -11806f)));
      WorldMap.SpawnWGO(MainGame.me.world_root, "candelabrum_2_1", new Vector3?((Vector3) new Vector2(20366f, -11818f)));
      WorldMap.SpawnWGO(MainGame.me.world_root, "teleport_inside_euric_room", new Vector2(20110f, -11530f), "tp_mortuary_from_euric_b_");
      WorldMap.SpawnWGO(MainGame.me.world_root, "teleport_point", new Vector2(20110f, -11582f), "tp_euric_from_mortuary_b");
      WorldMap.SpawnWGO(MainGame.me.world_root, "teleport_point", new Vector2(11424f, -10944f), "tp_mortuary_from_euric_b");
    }
    if (num1 <= 1312)
    {
      WorldMap.SpawnWGO(MainGame.me.world_root, "cow", new Vector3?((Vector3) new Vector2(16070f, 446f)));
      WorldMap.SpawnWGO(MainGame.me.world_root, "drying_rack_repaired", new Vector3?((Vector3) new Vector2(15552f, -3048f)));
    }
    if (num1 <= 1314)
    {
      WorldMap.SpawnWGO(MainGame.me.world_root, "keeper_room_builddesk", new Vector3?((Vector3) new Vector2(3276f, -6166f)));
      WorldMap.SpawnWGO(MainGame.me.world_root, "keeper_room_carpet", new Vector3?((Vector3) new Vector2(2496f, -6312f)));
      if (MainGame.me.save.unlocked_techs.Contains("The Beginning Of Alchemy"))
      {
        MainGame.me.save.UnlockCraft("keeper_room_builddesk:p:keeper_room_carpet_blue_1");
        MainGame.me.save.UnlockCraft("keeper_room_builddesk:p:keeper_room_carpet_brown_1");
        MainGame.me.save.UnlockCraft("keeper_room_builddesk:p:keeper_room_carpet_green_1");
        MainGame.me.save.UnlockCraft("keeper_room_builddesk:p:keeper_room_carpet_green_2");
        MainGame.me.save.UnlockCraft("keeper_room_builddesk:p:keeper_room_carpet_red_1");
        MainGame.me.save.UnlockCraft("keeper_room_builddesk:p:keeper_room_carpet_violet_1");
        MainGame.me.save.UnlockCraft("keeper_room_builddesk:p:keeper_room_carpet_violet_2");
        MainGame.me.save.UnlockCraft("keeper_room_builddesk:p:keeper_room_carpet_yellow_1");
        MainGame.me.save.UnlockCraft("keeper_room_builddesk::keeper_room_bed_blue_1");
        MainGame.me.save.UnlockCraft("keeper_room_builddesk::keeper_room_bed_brown_1");
        MainGame.me.save.UnlockCraft("keeper_room_builddesk::keeper_room_bed_green_1");
        MainGame.me.save.UnlockCraft("keeper_room_builddesk::keeper_room_bed_green_2");
        MainGame.me.save.UnlockCraft("keeper_room_builddesk::keeper_room_bed_red_1");
        MainGame.me.save.UnlockCraft("keeper_room_builddesk::keeper_room_bed_violet_1");
        MainGame.me.save.UnlockCraft("keeper_room_builddesk::keeper_room_bed_violet_2");
        MainGame.me.save.UnlockCraft("keeper_room_builddesk::keeper_room_bed_yellow_1");
        MainGame.me.save.UnlockCraft("keeper_room_builddesk:p:keeper_room_picture_1");
      }
      if ((double) MainGame.me.player.GetParam("met_donkey") >= 1.0)
        MainGame.me.save.UnlockCraft("keeper_room_builddesk:p:keeper_room_picture_7");
      if ((double) MainGame.me.player.GetParam("met_bishop") >= 1.0)
        MainGame.me.save.UnlockCraft("keeper_room_builddesk:p:keeper_room_picture_4");
      if ((double) MainGame.me.player.GetParam("met_inquisitor") >= 1.0)
        MainGame.me.save.UnlockCraft("keeper_room_builddesk:p:keeper_room_picture_5");
      if ((double) WorldMap.GetWorldGameObjectByObjId("npc_actress").GetParam("met_inquisitor") >= 1.0)
        MainGame.me.save.UnlockCraft("keeper_room_builddesk:p:keeper_room_picture_6");
    }
    if (num1 <= 1315)
    {
      WorldGameObject gameObjectByObjId5 = WorldMap.GetWorldGameObjectByObjId("soul_extractor_broken");
      if ((UnityEngine.Object) gameObjectByObjId5 == (UnityEngine.Object) null)
        gameObjectByObjId5 = WorldMap.GetWorldGameObjectByObjId("soul_extractor");
      if ((UnityEngine.Object) gameObjectByObjId5 != (UnityEngine.Object) null && string.IsNullOrEmpty(gameObjectByObjId5.custom_tag))
        gameObjectByObjId5.custom_tag = "soul_extractor";
    }
    if (num1 <= 1316 && MainGame.me.save.unlocked_techs.Contains("The Beginning Of Alchemy"))
    {
      WorldMap.SpawnWGO(MainGame.me.world_root, "keeper_room_walls", new Vector2(2688f, -6528f), "home_walls");
      MainGame.me.save.UnlockCraft("keeper_room_builddesk::keeper_room_walls_blue_1");
      MainGame.me.save.UnlockCraft("keeper_room_builddesk::keeper_room_walls_brown_1");
      MainGame.me.save.UnlockCraft("keeper_room_builddesk::keeper_room_walls_green_1");
      MainGame.me.save.UnlockCraft("keeper_room_builddesk::keeper_room_walls_green_2");
      MainGame.me.save.UnlockCraft("keeper_room_builddesk::keeper_room_walls_red_1");
      MainGame.me.save.UnlockCraft("keeper_room_builddesk::keeper_room_walls_violet_1");
      MainGame.me.save.UnlockCraft("keeper_room_builddesk::keeper_room_walls_violet_2");
      MainGame.me.save.UnlockCraft("keeper_room_builddesk::keeper_room_walls_yellow_1");
      MainGame.me.save.UnlockCraft("keeper_room_builddesk::keeper_room_walls_white_1");
      MainGame.me.save.UnlockCraft("keeper_room_builddesk::keeper_room_walls_black_1");
      MainGame.me.save.UnlockCraft("keeper_room_builddesk::keeper_room_bed_white_1");
      MainGame.me.save.UnlockCraft("keeper_room_builddesk::keeper_room_bed_black_1");
      MainGame.me.save.UnlockCraft("keeper_room_builddesk:p:keeper_room_carpet_white_1");
      MainGame.me.save.UnlockCraft("keeper_room_builddesk:p:keeper_room_carpet_black_1");
    }
    if (num1 <= 1320)
    {
      WorldGameObject gameObjectByObjId6 = WorldMap.GetWorldGameObjectByObjId("stained_glass_window");
      if ((UnityEngine.Object) gameObjectByObjId6 != (UnityEngine.Object) null)
        gameObjectByObjId6.transform.position = (Vector3) new Vector2(20254f, -11806f);
      List<WorldGameObject> wgOs1 = WorldMap.FindWGOs(new string[1]
      {
        "candelabrum_3_3"
      }, new Vector2(12332f, -11518f), string.Empty);
      if (wgOs1 != null && wgOs1.Count > 0)
        wgOs1[0].ReplaceWithObject("candelabrum_3_3_souls");
      List<WorldGameObject> wgOs2 = WorldMap.FindWGOs(new string[1]
      {
        "teleport_outside"
      }, new Vector2(20110f, -11584f), string.Empty);
      if (wgOs2 != null && wgOs2.Count > 0)
      {
        wgOs2[0].DestroyMe();
        WorldMap.SpawnWGO(MainGame.me.world_root, "teleport_inside_euric_room", new Vector2(20110f, -11530f), "tp_mortuary_from_euric_b_");
      }
      WorldGameObject gameObjectByObjId7 = WorldMap.GetWorldGameObjectByObjId("cooking_table");
      if ((UnityEngine.Object) gameObjectByObjId7 != (UnityEngine.Object) null)
        gameObjectByObjId7.custom_tag = "home_cook_table";
      WorldGameObject gameObjectByObjId8 = WorldMap.GetWorldGameObjectByObjId("cooking_stand");
      if ((UnityEngine.Object) gameObjectByObjId8 != (UnityEngine.Object) null)
        gameObjectByObjId8.custom_tag = "home_cook_stand";
    }
    if (num1 <= 1323)
    {
      if (MainGame.me.save.unlocked_techs.Contains("First slice"))
        MainGame.me.save.UnlockCraft("souls_builddesk:p:corpse_bed_place");
      WorldGameObject gameObjectByObjId9 = WorldMap.GetWorldGameObjectByObjId("cupboard");
      if ((UnityEngine.Object) gameObjectByObjId9 != (UnityEngine.Object) null)
        gameObjectByObjId9.custom_tag = "home_cupboard";
    }
    if (num1 <= 1328)
    {
      WorldGameObject objectByCustomTag7 = WorldMap.GetWorldGameObjectByCustomTag("home_cupboard");
      if ((UnityEngine.Object) objectByCustomTag7 != (UnityEngine.Object) null)
      {
        objectByCustomTag7.round_and_sort.floor_line = 0.0f;
        objectByCustomTag7.round_and_sort.MarkPositionDirty();
      }
    }
    if (num1 <= 1330)
    {
      List<WorldGameObject> wgOs3 = WorldMap.FindWGOs(new string[1]
      {
        "bush_4"
      }, new Vector2(5664f, -1896f), "");
      if (wgOs3 != null)
      {
        foreach (WorldGameObject worldGameObject in wgOs3)
          worldGameObject.DestroyMe();
      }
      List<WorldGameObject> wgOs4 = WorldMap.FindWGOs(new string[1]
      {
        "npc_farmer"
      }, new Vector2(4990f, -7974f), "");
      if (wgOs4 != null)
      {
        foreach (WorldGameObject worldGameObject in wgOs4)
          worldGameObject.DestroyMe();
      }
      WorldGameObject objectByCustomTag8 = WorldMap.GetWorldGameObjectByCustomTag("home_cupboard");
      if ((UnityEngine.Object) objectByCustomTag8 != (UnityEngine.Object) null)
        objectByCustomTag8.ReplaceWithObject("cupboard_home");
    }
    if (num1 <= 1400)
    {
      WorldGameObject objectByCustomTag9 = WorldMap.GetWorldGameObjectByCustomTag("home_walls", true);
      Debug.Log((object) $"#dbg_wall# keeper_room_wall is null: {(UnityEngine.Object) objectByCustomTag9 == (UnityEngine.Object) null}");
      if ((UnityEngine.Object) objectByCustomTag9 == (UnityEngine.Object) null)
      {
        Debug.Log((object) "#dbg_wall# keeper_room_wall has spawned");
        WorldMap.SpawnWGO(MainGame.me.world_root, "keeper_room_walls", new Vector2(2688f, -6528f), "home_walls");
      }
    }
    if (num1 <= 1402)
    {
      List<WorldGameObject> wgOs = WorldMap.FindWGOs(new string[1]
      {
        "tavern_cellar_rack"
      }, new Vector2(16588f, -9090f), "");
      if (wgOs != null)
      {
        foreach (WorldGameObject worldGameObject in wgOs)
        {
          WorldMap.SpawnWGO(MainGame.me.world_root, "tavern_cellar_rack_2", new Vector3?((Vector3) new Vector2(16588f, -9090f))).data.inventory = worldGameObject.data.inventory;
          worldGameObject.data.inventory = (List<Item>) null;
          worldGameObject.DestroyMe();
        }
      }
      else
        WorldMap.SpawnWGO(MainGame.me.world_root, "tavern_cellar_rack_2", new Vector3?((Vector3) new Vector2(16588f, -9090f)));
    }
    if (num1 <= 1403)
      WorldMap.TryRemoveStackedChurchVisitors();
    if (num1 <= 1404 && MainGame.me.save.black_list_of_phrases.Contains("@souls_s_s46_ask"))
      WorldMap.GetWorldGameObjectByObjId("smilers_box_opened")?.ReplaceWithObject("smilers_box_abandoned");
    if (num1 > 1406 || !MainGame.me.save.unlocked_crafts.Contains("glass_broken_0"))
      return;
    MainGame.me.save.UnlockCraft("glass_broken_2");
  }

  public void GlobalEventsCheck()
  {
    foreach (GlobalEventBase globalEventBase in new List<GlobalEventBase>()
    {
      new GlobalEventBase("halloween", new DateTime(2018, 10, 29), new TimeSpan(14, 0, 0, 0))
      {
        on_start_script = (IHasExecute) new Scene1100_To_SceneHelloween(),
        on_finish_script = (IHasExecute) new SceneHelloween_To_Scene1100()
      }
    })
      globalEventBase.Process();
  }

  public Item GetSavedPlayerInventory()
  {
    Debug.Log((object) nameof (GetSavedPlayerInventory));
    return this._inventory;
  }

  public bool BuyTech(string tech_id)
  {
    if (!this.CanBuyTech(tech_id))
      return false;
    TechDefinition data = GameBalance.me.GetData<TechDefinition>(tech_id);
    MainGame.me.player.data.SubFromParams(data.price);
    this.UnlockTech(tech_id);
    return true;
  }

  public bool CanBuyTech(string tech_id)
  {
    if (this.unlocked_techs.Contains(tech_id))
      return false;
    TechDefinition data = GameBalance.me.GetData<TechDefinition>(tech_id);
    if (data == null || !MainGame.me.player.data.IsEnoughParams(data.price))
      return false;
    foreach (BalanceBaseObject parent in data.parents)
    {
      if (!this.unlocked_techs.Contains(parent.id))
        return false;
    }
    return true;
  }

  public void SetEnvironmentPreset(string[] parts)
  {
    string str = ((IEnumerable<string>) parts).Last<string>();
    string part = parts[1];
    switch (str)
    {
      case "a":
        this._environment_preset = (string) null;
        break;
      case "b":
        this._environment_preset = part;
        break;
    }
    this.ApplyCurrentEnvironmentPreset();
  }

  public void SetEnvironmentPreset(string preset_name)
  {
    this._environment_preset = preset_name;
    this.ApplyCurrentEnvironmentPreset();
  }

  public void ApplyCurrentEnvironmentPreset()
  {
    string environmentPreset = string.IsNullOrEmpty(this._environment_preset) ? (string) null : this._environment_preset;
    Debug.Log((object) ("ApplyCurrentEnvironmentPreset, id = " + environmentPreset));
    EnvironmentEngine.me.ApplyEnvironmentPreset(EnvironmentPreset.Load(environmentPreset));
  }

  public int day_of_week => this.day % 6;

  public bool GetSinState(Sins.SinType sin)
  {
    return (double) MainGame.me.player.GetParam(Sins.SIN_NAMES[(int) sin]) >= 100.0;
  }

  public float GetHPPercentage() => MainGame.me.player.hp / (float) this.max_hp;

  public void OnFinishedCraft(CraftDefinition craft)
  {
    MainGame.me.player.GetComponent<PlayerComponent>().ResetSpentCounters();
    this.quests.CheckKeyQuests("craft_" + craft.id);
    Stats.DesignEvent($"Craft:{craft.id.Replace(":", "_")}:Finished");
    if (!this.completed_one_time_crafts.Contains(craft.id) && !craft.id.Contains(":_:"))
    {
      this.completed_one_time_crafts.Add(craft.id);
      this.quests.CheckKeyQuests("newcraft_" + craft.id);
      if (!string.IsNullOrEmpty(craft.ach_key))
        MainGame.me.save.achievements.CheckKeyQuests("new_" + craft.ach_key);
    }
    if (!string.IsNullOrEmpty(craft.ach_key))
      MainGame.me.save.achievements.CheckKeyQuests(craft.ach_key);
    if (craft.needs.Count > 0 && craft.craft_type == CraftDefinition.CraftType.AlchemyDecompose)
    {
      string id = craft.needs[0]?.id;
      if (string.IsNullOrEmpty(id))
        return;
      ItemDefinition.AlchemyType? alchType = craft.GetFirstRealOutput()?.definition?.alch_type;
      if (!alchType.HasValue)
        return;
      int num = (int) alchType.Value;
      switch (num)
      {
        case 1:
        case 2:
        case 3:
          CraftDefinition data = GameBalance.me.GetData<CraftDefinition>($"alchemy_{num.ToString()}_{id}");
          if (data == null)
            break;
          this.UnlockCraft(data.id);
          break;
      }
    }
    else
    {
      if (craft.needs.Count <= 0 || craft.craft_type != CraftDefinition.CraftType.MixedCraft)
        return;
      string craft_id = "mix";
      foreach (Item need in craft.needs)
      {
        if (!TechDefinition.TECH_POINTS.Contains(need.id))
        {
          string id = need.id;
          if (id.Contains(":"))
          {
            string[] strArray = id.Split(new string[1]
            {
              ":"
            }, StringSplitOptions.RemoveEmptyEntries);
            id = strArray[strArray.Length - 1];
          }
          craft_id = $"{craft_id}_{id}";
        }
      }
      if (!(craft_id != "mix"))
        return;
      this.UnlockCraft(craft_id);
    }
  }

  public bool IsCraftVisible(CraftDefinition craft)
  {
    return !craft.hidden && !craft.IsLocked() && (!craft.one_time_craft || !this.completed_one_time_crafts.Contains(craft.id));
  }

  public bool IsWorkAvailible(ObjectDefinition work_obj)
  {
    string id = work_obj.object_groups.Count > 0 ? work_obj.object_groups[0].id : "";
    return !work_obj.need_unlock_work || this.unlocked_works.Contains(id);
  }

  public bool IsSurveyComplete(string item_id)
  {
    return this.IsSurveyComplete(CraftDefinition.CraftSubType.None, item_id);
  }

  public bool IsSurveyComplete(CraftDefinition.CraftSubType type, string item_id)
  {
    return item_id.Contains(":") && this.completed_one_time_crafts.Contains("surv:" + ItemDefinition.StaticGetNameWithoutQualitySuffix(item_id)) || this.completed_one_time_crafts.Contains("surv:" + item_id);
  }

  public void RevealHiddenTech(string tech_id)
  {
    TechDefinition data = GameBalance.me.GetData<TechDefinition>(tech_id);
    if (data == null)
      Debug.LogError((object) ("Couldn't find technology id = " + tech_id));
    else if (!data.hidden)
    {
      Debug.LogError((object) ("Can't reveal non-hidden tech id = " + tech_id));
    }
    else
    {
      if (!this.revealed_techs.Contains(tech_id))
        this.revealed_techs.Add(tech_id);
      if (!data.invisible || this.visible_techs.Contains(tech_id))
        return;
      this.visible_techs.Add(tech_id);
    }
  }

  public void MakeVisibleInvisibleTech(string tech_id)
  {
    TechDefinition data = GameBalance.me.GetData<TechDefinition>(tech_id);
    if (data == null)
      Debug.LogError((object) ("Couldn't find technology id = " + tech_id));
    else if (!data.invisible)
    {
      Debug.LogError((object) ("Can't make visible non-invisible tech id = " + tech_id));
    }
    else
    {
      if (this.visible_techs.Contains(tech_id))
        return;
      this.visible_techs.Add(tech_id);
    }
  }

  public void OnMetNPC(string npc_id) => this.known_npcs.GetOrCreateNPC(npc_id);

  public void SetTaskState(
    string npc_id,
    string task_id,
    KnownNPC.TaskState.State state,
    System.Action on_finished = null)
  {
    Debug.Log((object) $"SetTaskState npc={npc_id}, task={task_id}, state={state.ToString()}");
    if (state == KnownNPC.TaskState.State.Visible)
    {
      if (this.known_npcs.GetOrCreateNPC(npc_id).GetQuestState(task_id) == KnownNPC.TaskState.State.Complete)
      {
        on_finished.TryInvoke();
        return;
      }
      this.known_npcs.GetOrCreateNPC(npc_id).SetQuestState(task_id, state);
      GUIElements.me.relation.npc_tasks.Redraw();
    }
    WorldGameObject npc = WorldMap.GetWorldGameObjectByObjId(npc_id, true);
    if ((UnityEngine.Object) npc == (UnityEngine.Object) null)
      npc = MainGame.me.player;
    Transform bubblePosTf = MainGame.me.player.bubble_pos_tf;
    Transform markerPointOfTask = GUIElements.me.relation.npc_tasks.GetMarkerPointOfTask(npc_id, task_id);
    string gui_sprite_name = "icon_quest_mark_small";
    if (task_id.StartsWith("dlc_stories_"))
      gui_sprite_name = "dlc_quest_mrk";
    else if (task_id.StartsWith("dlc_refugees") || task_id.StartsWith("s_ev"))
      gui_sprite_name = "quest_marker_violet";
    else if (task_id.StartsWith("dlc_souls"))
      gui_sprite_name = "Icon_quest_mark_small_blue";
    if ((UnityEngine.Object) markerPointOfTask == (UnityEngine.Object) null && (UnityEngine.Object) npc != (UnityEngine.Object) MainGame.me.player)
    {
      GUIElements.me.relation_additional.npc_tasks.Redraw();
      GUIElements.me.relation_additional.Open(npc);
      Transform additional_point = GUIElements.me.relation_additional.npc_tasks.GetMarkerPointOfTask(npc_id, task_id);
      if ((UnityEngine.Object) additional_point != (UnityEngine.Object) null)
      {
        UIWidget relation_widget = GUIElements.me.relation_additional.GetComponent<UIWidget>();
        relation_widget.alpha = 0.0f;
        DOTween.To((DOGetter<float>) (() => relation_widget.alpha), (DOSetter<float>) (x => relation_widget.alpha = x), 1f, 0.2f);
        FlyingObject flyingGuiSprite = FlyingObject.CreateFlyingGUISprite(gui_sprite_name, bubblePosTf);
        flyingGuiSprite.StartSmoothFlyAndBounceToAMovingObject((TweenToAMovingTarget.GetTransformDelegate) (() => additional_point));
        UIWidget component = additional_point.gameObject.GetComponentInParent<HUDTaskItemGUI>().GetComponent<UIWidget>();
        if (state == KnownNPC.TaskState.State.Visible)
        {
          component.alpha = 0.0f;
          GUIElements.me.relation_additional.npc_tasks.SetTaskHiddenState(true, task_id);
        }
        flyingGuiSprite.on_reached_dest = (System.Action) (() =>
        {
          GUIElements.me.relation_additional.npc_tasks.SetTaskHiddenState(state != 0, task_id);
          if (state == KnownNPC.TaskState.State.Complete)
          {
            this.known_npcs.GetOrCreateNPC(npc_id).SetQuestState(task_id, state);
            GUIElements.me.relation_additional.npc_tasks.Redraw();
          }
          GJTimer.AddTimer(2.5f, (GJTimer.VoidDelegate) (() => DOTween.To((DOGetter<float>) (() => relation_widget.alpha), (DOSetter<float>) (x => relation_widget.alpha = x), 0.0f, 0.2f).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => GUIElements.me.relation_additional.Hide()))));
        });
      }
      else
        GUIElements.me.relation_additional.Hide();
    }
    if ((UnityEngine.Object) markerPointOfTask != (UnityEngine.Object) null)
    {
      bubblePosTf = npc.bubble_pos_tf;
      FlyingObject flyingGuiSprite = FlyingObject.CreateFlyingGUISprite(gui_sprite_name, bubblePosTf);
      flyingGuiSprite.StartSmoothFlyAndBounceToAMovingObject((TweenToAMovingTarget.GetTransformDelegate) (() => GUIElements.me.relation.npc_tasks.GetMarkerPointOfTask(npc_id, task_id)));
      Debug.Log((object) "Create flying object", (UnityEngine.Object) flyingGuiSprite);
      UIWidget component = markerPointOfTask.gameObject.GetComponentInParent<HUDTaskItemGUI>().GetComponent<UIWidget>();
      if (state == KnownNPC.TaskState.State.Visible)
      {
        component.alpha = 0.0f;
        GUIElements.me.relation.npc_tasks.SetTaskHiddenState(true, task_id);
      }
      flyingGuiSprite.on_reached_dest = (System.Action) (() =>
      {
        GUIElements.me.relation.npc_tasks.SetTaskHiddenState(state != 0, task_id);
        if (state == KnownNPC.TaskState.State.Complete)
        {
          this.known_npcs.GetOrCreateNPC(npc_id).SetQuestState(task_id, state);
          GUIElements.me.relation.npc_tasks.Redraw();
        }
        on_finished.TryInvoke();
      });
    }
    else
    {
      if (state == KnownNPC.TaskState.State.Complete)
        this.known_npcs.GetOrCreateNPC(npc_id).SetQuestState(task_id, state);
      on_finished.TryInvoke();
    }
    EffectBubblesManager.ShowImmediately(bubblePosTf.position, GJL.L("task_bubble_" + state.ToString().ToLower()), EffectBubblesManager.BubbleColor.Relation, custom_time: 1f);
  }

  public void UnlockPerk(string perk_id)
  {
    if (this.unlocked_perks.Contains(perk_id))
      return;
    this.unlocked_perks.Add(perk_id);
    PerkDefinition data = GameBalance.me.GetData<PerkDefinition>(perk_id);
    if (data == null)
      return;
    MainGame.me.player.AddToParams(data.output_res);
  }

  public Item GenerateBody(int tier_min, int tier_max, int soul_tier_min = -1, int soul_tier_max = -1)
  {
    List<BodyDefinition> list = new List<BodyDefinition>();
    foreach (BodyDefinition bodyDefinition in GameBalance.me.bodies_data)
    {
      if (bodyDefinition.tier >= tier_min && bodyDefinition.tier <= tier_max)
        list.Add(bodyDefinition);
    }
    if (list.Count == 0)
    {
      Debug.LogError((object) $"Couldn't generate body - no suitable BodyDefenition found. tier_min = {tier_min.ToString()}, tier_max = {tier_max.ToString()}");
      return (Item) null;
    }
    BodyDefinition bodyDefinition1 = list.RandomElement<BodyDefinition>();
    Item bodyItem = bodyDefinition1.GenerateBodyItem();
    Debug.Log((object) $"Generated body with tiers {tier_min.ToString()}..{tier_max.ToString()}, id = {bodyDefinition1.id}, tier = {bodyDefinition1.tier.ToString()}");
    if (soul_tier_min > -1 && soul_tier_max > -1)
    {
      Item soul = this.GenerateSoul(soul_tier_min, soul_tier_max);
      if (soul != null)
        bodyItem.inventory.Add(soul);
    }
    MixerLightIntegration.ProcessBody(bodyItem);
    return bodyItem;
  }

  public Item GenerateSoul(int tier_min, int tier_max)
  {
    List<SoulDefinition> list = new List<SoulDefinition>();
    foreach (SoulDefinition soulDefinition in GameBalance.me.souls_data)
    {
      if (soulDefinition.tier >= tier_min && soulDefinition.tier <= tier_max)
        list.Add(soulDefinition);
    }
    if (list.Count == 0)
    {
      Debug.LogError((object) $"There's no body to generate withing tiers range: [{tier_min}-{tier_max}]");
      return (Item) null;
    }
    SoulDefinition soulDefinition1 = list.RandomElement<SoulDefinition>();
    Item soulItem = soulDefinition1.GenerateSoulItem();
    Debug.Log((object) $"Generated soul with tiers {tier_min.ToString()}..{tier_max.ToString()}, id = {soulDefinition1.id}, tier = {soulDefinition1.tier.ToString()}");
    return soulItem;
  }

  public bool IsInTutorial() => MainGame.me.player.GetParamInt("in_tutorial") > 0;

  public void OnEnteredWorldZone(WorldZone z)
  {
    if (this.known_world_zones.Contains(z.id))
      return;
    this.known_world_zones.Add(z.id);
    this.achievements.CheckKeyQuests("newzone_" + z.id);
  }

  public bool IsWorldZoneKnown(string id) => this.known_world_zones.Contains(id);

  public void StoreEnvironmentPreset()
  {
    this._stored_environment_preset = this._environment_preset;
  }

  public void RestoreEnvironmentPreset()
  {
    this._environment_preset = this._stored_environment_preset;
    this.ApplyCurrentEnvironmentPreset();
    this._stored_environment_preset = "";
  }

  [Serializable]
  public struct SavedDropItem
  {
    public Item res;
    public Vector3 pos;
    public string zone_id;
  }

  [Serializable]
  public class SavedGDPoint
  {
    public string gd_tag;
    public bool enabled;
  }
}
