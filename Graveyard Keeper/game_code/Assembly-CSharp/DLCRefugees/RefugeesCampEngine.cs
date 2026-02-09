// Decompiled with JetBrains decompiler
// Type: DLCRefugees.RefugeesCampEngine
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace DLCRefugees;

public class RefugeesCampEngine : MonoBehaviour
{
  public const string CAMP_STORAGE_CUSTOM_TAG = "refugee_camp_depot";
  public const string CAMP_PROGRESS_OBJ_CUSTOM_TAG = "refugee_camp_progress_obj";
  public const string CAMP_HAPPINESS_RES_NAME = "cur_refugee_happiness";
  public const string CAMP_HAPPINESS_ITEM_NAME = "refugee_happiness_item";
  public const string CAMP_TENT_CUSTOM_TAG = "refuee_camp_tent";
  public const string CAMP_TENT_PLACE_AVAILABLE_ITEM = "refugee_tent_place_available_item";
  public const string CAMP_CURRENT_PROGRESS_BAR = "refugee_current_progress_bar";
  public const string CAMP_AVAILABLE_SLOTS_ITEM_NAME = "refugee_available_slots";
  public const float WATER_NEEDS_FOR_REFUGEE_PER_DAY = 3f;
  public const float ENERGY_NEEDS_FOR_REFUGEE_PER_DAY = 30f;
  public const float MAX_WATER_SATIETY_COEFF = 2f;
  public const float MAX_ENERGY_SATIETY_COEFF = 2f;
  public const int MAX_REFUGEE_CAMP_QUALITY = 16 /*0x10*/;
  public List<string> _home_additional_tents_gd_point_names_list = new List<string>()
  {
    "gd_refugee_camp_wp_home_4",
    "gd_refugee_camp_wp_home_5",
    "gd_refugee_camp_wp_home_6",
    "gd_refugee_camp_wp_home_7"
  };
  public RefugeeCampData _data;
  public WorldGameObject _camp_storage_cached;
  public WorldGameObject _camp_progress_object_cached;
  public MaskProgressBar _camp_progress_bar_cached;
  public WorldZone _camp_zone;
  public WorldZone _master_alarich_zone;
  public static RefugeesCampEngine _instance;
  [CompilerGenerated]
  public float \u003Ccamp_zone_quality\u003Ek__BackingField;

  public RefugeeCampData Data
  {
    get
    {
      if (this._data == null)
        this._data = MainGame.me.save.refugees_camp_data;
      return this._data;
    }
    set => this._data = value;
  }

  public static RefugeesCampEngine instance
  {
    get
    {
      if ((UnityEngine.Object) RefugeesCampEngine._instance == (UnityEngine.Object) null)
      {
        RefugeesCampEngine._instance = SingletonGameObjects.FindOrCreate<RefugeesCampEngine>();
        if ((UnityEngine.Object) RefugeesCampEngine._instance == (UnityEngine.Object) null)
          Debug.LogError((object) "RefugeesCampEngine: error finding type");
      }
      return RefugeesCampEngine._instance;
    }
  }

  public WorldGameObject camp_storage
  {
    get
    {
      if ((UnityEngine.Object) this._camp_storage_cached == (UnityEngine.Object) null)
        this._camp_storage_cached = WorldMap.GetWorldGameObjectByCustomTag("refugee_camp_depot");
      return this._camp_storage_cached;
    }
  }

  public WorldZone camp_zone
  {
    get
    {
      if ((UnityEngine.Object) this._camp_zone == (UnityEngine.Object) null)
        this._camp_zone = WorldZone.GetZoneByID("refugees_camp");
      return this._camp_zone;
    }
  }

  public WorldZone master_alarich_zone
  {
    get
    {
      if ((UnityEngine.Object) this._master_alarich_zone == (UnityEngine.Object) null)
        this._master_alarich_zone = WorldZone.GetZoneByID("alarich_tent_inside");
      return this._master_alarich_zone;
    }
  }

  public WorldGameObject camp_progress_object
  {
    get
    {
      if ((UnityEngine.Object) this._camp_progress_object_cached == (UnityEngine.Object) null)
        this._camp_progress_object_cached = WorldMap.GetWorldGameObjectByCustomTag("refugee_camp_progress_obj");
      return this._camp_progress_object_cached;
    }
  }

  public MaskProgressBar camp_progress_bar
  {
    get
    {
      if ((UnityEngine.Object) this._camp_progress_bar_cached == (UnityEngine.Object) null)
        this._camp_progress_bar_cached = this.camp_progress_object.GetComponentInChildren<MaskProgressBar>();
      return this._camp_progress_bar_cached;
    }
  }

  public int refugees_count => this.Data.active_refugee_list.Count;

  public float camp_zone_quality
  {
    get => this.\u003Ccamp_zone_quality\u003Ek__BackingField;
    set => this.\u003Ccamp_zone_quality\u003Ek__BackingField = value;
  }

  public void Init()
  {
    this.Data.Init();
    WorldZone.OnQualityComputed += new Action<string, float>(this.WorldZoneComputed);
    if (this.Data.camp_was_started_at_once)
    {
      this._camp_storage_cached = WorldMap.GetWorldGameObjectByCustomTag("refugee_camp_depot");
      this._camp_progress_object_cached = WorldMap.GetWorldGameObjectByCustomTag("refugee_camp_progress_obj");
      this._camp_progress_bar_cached = this.camp_progress_object.GetComponentInChildren<MaskProgressBar>();
    }
    this.SetCampMusic(this.Data.camp_music);
    this.SetCampMusicAlarich(this.Data.camp_music_alarich);
  }

  public void DeInit()
  {
    WorldZone.OnQualityComputed -= new Action<string, float>(this.WorldZoneComputed);
    if (this.Data.camp_was_started_at_once)
    {
      this._camp_storage_cached = (WorldGameObject) null;
      this._camp_progress_object_cached = (WorldGameObject) null;
      this._camp_progress_bar_cached = (MaskProgressBar) null;
    }
    this.Data = (RefugeeCampData) null;
  }

  public void StartCampLive()
  {
    this.Data.is_camp_living = true;
    this.Data.camp_was_started_at_once = true;
    MainGame.me.player.SetParam("camp_is_live", 1f);
    this.UpdateCampProgressObjectBar();
  }

  public void StopCampLive()
  {
    this.Data.is_camp_living = false;
    MainGame.me.player.SetParam("camp_is_live", 0.0f);
  }

  public WorldGameObject SpawnNextRefugeeAtTransform(Transform transform, string home_point_gd_tag)
  {
    RefugeeInfo refugeeInfoToSpawn = this.GetNextRefugeeInfoToSpawn();
    WorldGameObject world_game_object = (WorldGameObject) null;
    Refugee refugee = new Refugee();
    if (refugeeInfoToSpawn != null)
    {
      world_game_object = WorldMap.SpawnWGO(MainGame.me.world_root, refugeeInfoToSpawn.obj_id, new Vector3?(transform.position));
      world_game_object.custom_tag = refugeeInfoToSpawn.custom_tag;
      refugee = this.AddRefugee(world_game_object);
    }
    else
      Debug.LogError((object) "There is no refugee for spawn");
    refugee.home_gd_point_tag = home_point_gd_tag;
    Debug.Log((object) $"Spawned refugee: {world_game_object?.ToString()}, with home gd point tag: {refugee.home_gd_point_tag}");
    return world_game_object;
  }

  public WorldGameObject SpawnNextRefugeeAtTransformFromCraft(Transform transform)
  {
    string homeGdPointForTent = this.GetHomeGDPointForTent(this.GetVacantTentForRefugee());
    return this.SpawnNextRefugeeAtTransform(transform, homeGdPointForTent);
  }

  public void FeedRefugeeForCycle(float time_in_days)
  {
    float overallHappinessDelta = this.CalculateOverallHappinessDelta(this.FeedRefuge(time_in_days), time_in_days);
    Debug.Log((object) ("happiness_delta: " + overallHappinessDelta.ToString()));
    this.UpdateRefugeeCampValues(overallHappinessDelta);
  }

  public float PredictRefugeeHappinessChange(float time_in_days)
  {
    return this.refugees_count == 0 ? 0.0f : this.CalculateOverallHappinessDelta(this.PredictRefugeeFeeding(time_in_days), time_in_days);
  }

  public void AddItemsOnTentAppear(WorldGameObject tent_object)
  {
    tent_object.AddToInventory("refugee_tent_place_available_item", tent_object.obj_def.can_insert_items_limit);
  }

  public string GetRefugeesHomeGDTag(WorldGameObject refugee_object)
  {
    for (int index = 0; index < this.Data.active_refugee_list.Count; ++index)
    {
      if ((UnityEngine.Object) refugee_object == (UnityEngine.Object) this.Data.active_refugee_list[index].world_game_object)
        return this.Data.active_refugee_list[index].home_gd_point_tag;
    }
    Debug.LogError((object) "Refugee object not found list", (UnityEngine.Object) refugee_object);
    return string.Empty;
  }

  public List<WorldGameObject> GetSpawnedRefugeeList()
  {
    List<WorldGameObject> spawnedRefugeeList = new List<WorldGameObject>();
    foreach (Refugee activeRefugee in this.Data.active_refugee_list)
      spawnedRefugeeList.Add(activeRefugee.world_game_object);
    return spawnedRefugeeList;
  }

  public void PlaceRoadAndAuxiliaryForTent(WorldGameObject tent_wgo)
  {
    string homeGdPointForTent = this.GetHomeGDPointForTent(tent_wgo);
    string gd_point_tag_to_enable1 = string.Empty;
    string gd_point_tag_to_enable2 = string.Empty;
    switch (homeGdPointForTent)
    {
      case "gd_refugee_camp_wp_home_4":
        gd_point_tag_to_enable1 = "gd_refugees_road_add_tent_1";
        gd_point_tag_to_enable2 = "gd_refugee_stage_2_tent_4";
        break;
      case "gd_refugee_camp_wp_home_5":
        gd_point_tag_to_enable1 = "gd_refugees_road_add_tent_2";
        gd_point_tag_to_enable2 = "gd_refugee_stage_2_tent_5";
        break;
      case "gd_refugee_camp_wp_home_6":
        gd_point_tag_to_enable1 = "gd_refugees_road_add_tent_3";
        gd_point_tag_to_enable2 = "gd_refugee_stage_2_tent_6";
        break;
      case "gd_refugee_camp_wp_home_7":
        gd_point_tag_to_enable1 = "gd_refugees_road_add_tent_4";
        gd_point_tag_to_enable2 = "gd_refugee_stage_2_tent_7";
        break;
    }
    this.EnableGDPoint(gd_point_tag_to_enable1);
    this.EnableGDPoint(gd_point_tag_to_enable2);
  }

  public float StartCooking(WorldGameObject wgo)
  {
    CraftComponent craft1 = wgo.components.craft;
    MultiInventory multiInventory = wgo.GetMultiInventory();
    List<CraftDefinition> craftDefinitionList = new List<CraftDefinition>();
    foreach (CraftDefinition craft2 in craft1.crafts)
    {
      if (!MainGame.me.save.locked_crafts.Contains(craft2.id) && (!craft2.needs_unlock || MainGame.me.save.unlocked_crafts.Contains(craft2.id)) && multiInventory.IsEnoughItems(craft2.needs))
        craftDefinitionList.Add(craft2);
    }
    if (craftDefinitionList.Count == 0)
      return (float) UnityEngine.Random.Range(5, 30);
    if ((double) TimeOfDay.me.GetTimeK() > 0.75)
      return TimeOfDay.me.GetSecondsToTheMorning() + 24.75f;
    CraftDefinition craftDefinition = craftDefinitionList[UnityEngine.Random.Range(0, craftDefinitionList.Count - 1)];
    wgo.TryStartCraft(craftDefinition.id);
    return craftDefinition.craft_time.EvaluateFloat() + 1f;
  }

  public void SetCampMusic(RefugeeCampMusic campMusic = RefugeeCampMusic.Default)
  {
    this.Data.camp_music = campMusic;
    if (!((UnityEngine.Object) this.camp_zone != (UnityEngine.Object) null))
      return;
    switch (campMusic)
    {
      case RefugeeCampMusic.Default:
        this.camp_zone.ovr_music = "dlc_refugee_theme";
        break;
      case RefugeeCampMusic.Sad:
        this.camp_zone.ovr_music = "dlc_refugee_theme_sad";
        break;
      case RefugeeCampMusic.Happy:
        this.camp_zone.ovr_music = "dlc_refugee_theme_happy";
        break;
    }
  }

  public void SetCampMusicAlarich(RefugeeCampMusicAlarich campMusicAlarich = RefugeeCampMusicAlarich.None)
  {
    this.Data.camp_music_alarich = campMusicAlarich;
    if (!((UnityEngine.Object) this.master_alarich_zone != (UnityEngine.Object) null))
      return;
    if (campMusicAlarich != RefugeeCampMusicAlarich.None)
    {
      if (campMusicAlarich != RefugeeCampMusicAlarich.AlarichsTheme)
        return;
      this.master_alarich_zone.ovr_music = "master_alarich_music";
    }
    else
      this.master_alarich_zone.ovr_music = string.Empty;
  }

  public void UpdateRefugeeCampValues(
    float happiness_delta,
    RefugeesCampEngine.UpdateHappinessItemsMode mode = RefugeesCampEngine.UpdateHappinessItemsMode.GameResUpdatesItem)
  {
    if (mode == RefugeesCampEngine.UpdateHappinessItemsMode.ItemUpdatesGameRes)
      this.UpdateCampProgressObjectSlots();
    this.UpdateHappiness(happiness_delta);
    this.UpdateCampProgressObjectHappinessItems(mode);
    this.UpdateCampProgressObjectBar();
  }

  public static float GetPlayersDebt()
  {
    float playersDebt = 0.0f;
    WorldGameObject objectByCustomTag = WorldMap.GetWorldGameObjectByCustomTag("npc_refugee_6", true);
    if ((UnityEngine.Object) objectByCustomTag != (UnityEngine.Object) null && objectByCustomTag.GetParamInt("is_player_took_money") == 1)
      playersDebt = objectByCustomTag.GetParam("money_debd_amount");
    return playersDebt;
  }

  public RefugeeWealth FeedRefuge(float time_in_days)
  {
    float total_water_need_for_cycle;
    float total_energy_need_for_cycle;
    this.GetRefugeeNeeds(time_in_days, out total_water_need_for_cycle, out total_energy_need_for_cycle);
    float water_satiety_coeff = this.GetWaterFromStorage(Mathf.CeilToInt(total_water_need_for_cycle));
    float energy_satiety_coeff = this.GetEnergyFromMealInStorage(total_energy_need_for_cycle);
    if ((double) water_satiety_coeff > 2.0)
      water_satiety_coeff = 2f;
    if ((double) energy_satiety_coeff > 2.0)
      energy_satiety_coeff = 2f;
    return new RefugeeWealth(water_satiety_coeff, energy_satiety_coeff, this.refugees_count);
  }

  public void GetRefugeeNeeds(
    float time_in_days,
    out float total_water_need_for_cycle,
    out float total_energy_need_for_cycle)
  {
    total_water_need_for_cycle = 3f * (float) this.refugees_count * time_in_days;
    total_energy_need_for_cycle = 30f * (float) this.refugees_count * time_in_days;
  }

  public void WorldZoneComputed(string world_zone_id, float world_zone_quality)
  {
    if (!this.Data.is_camp_living || !(world_zone_id == "refugees_camp"))
      return;
    this.camp_zone_quality = world_zone_quality;
    this.UpdateRefugeeCampValues(0.0f, RefugeesCampEngine.UpdateHappinessItemsMode.ItemUpdatesGameRes);
    if ((double) world_zone_quality != 16.0)
      return;
    MainGame.me.save.quests.CheckKeyQuests("dlc_refugees_paradise");
  }

  public RefugeeWealth PredictRefugeeFeeding(float time_in_days)
  {
    float total_water_need_for_cycle;
    float total_energy_need_for_cycle;
    this.GetRefugeeNeeds(time_in_days, out total_water_need_for_cycle, out total_energy_need_for_cycle);
    return new RefugeeWealth(this.NeedWaterFromStorage(Mathf.CeilToInt(total_water_need_for_cycle), out int _), this.NeedEnergyMealFromStorage(total_energy_need_for_cycle, out List<SimplifiedItem> _), this.refugees_count);
  }

  public float CalculateOverallHappinessDelta(RefugeeWealth refugee_wealth, float time_in_days)
  {
    return (float) ((3.0 * ((double) refugee_wealth.energy_satiety_coeff + (double) refugee_wealth.water_satiety_coeff) - 2.0 * (double) time_in_days) / 100.0) * (float) refugee_wealth.refugees_count;
  }

  public Refugee AddRefugee(WorldGameObject world_game_object)
  {
    Refugee refugee = new Refugee(world_game_object);
    this.Data.active_refugee_list.Add(refugee);
    return refugee;
  }

  public RefugeeInfo GetNextRefugeeInfoToSpawn()
  {
    RefugeeInfo refugeeInfoToSpawn = (RefugeeInfo) null;
    if (this.Data.refugee_to_spawn_ordered_list.Count > 0)
    {
      this.Data.refugee_already_spawned_list.Add(this.Data.refugee_to_spawn_ordered_list[0]);
      this.Data.refugee_to_spawn_ordered_list.RemoveAt(0);
      refugeeInfoToSpawn = this.Data.refugee_already_spawned_list[this.Data.refugee_already_spawned_list.Count - 1];
    }
    else
      Debug.LogError((object) "No refugee for spawn");
    return refugeeInfoToSpawn;
  }

  public float GetWaterFromStorage(int water_amount_need)
  {
    MultiInventory multiInventory = this.camp_storage.GetMultiInventory();
    int water_amount_to_remove;
    float waterFromStorage = this.NeedWaterFromStorage(water_amount_need, out water_amount_to_remove);
    int item_value = water_amount_to_remove;
    multiInventory.RemoveItem("water", item_value);
    Debug.Log((object) ("water satiety: " + waterFromStorage.ToString()));
    return waterFromStorage;
  }

  public float NeedWaterFromStorage(int water_amount_need, out int water_amount_to_remove)
  {
    int totalCount = this.camp_storage.GetMultiInventory().GetTotalCount("water");
    water_amount_to_remove = 0;
    water_amount_to_remove = totalCount <= water_amount_need ? totalCount : water_amount_need;
    return (float) water_amount_to_remove / (float) water_amount_need;
  }

  public float NeedEnergyMealFromStorage(
    float energy_amount,
    out List<SimplifiedItem> items_for_remove)
  {
    List<SimplifiedItem> simplifiedItemList = new List<SimplifiedItem>();
    items_for_remove = new List<SimplifiedItem>();
    List<Item> inventory = this.camp_storage.data.inventory;
    for (int index1 = 0; index1 < inventory.Count; ++index1)
    {
      Item obj = inventory[index1];
      if (obj != null && !obj.IsEmpty() && obj.value > 0)
      {
        ItemDefinition definition = obj.definition;
        float give_energy = definition.params_on_use.Get("energy");
        if (definition.can_be_used && (double) give_energy > 0.0)
        {
          bool flag = false;
          for (int index2 = 0; index2 < simplifiedItemList.Count; ++index2)
          {
            if (simplifiedItemList[index2].id == obj.id)
            {
              flag = true;
              simplifiedItemList[index2].count += obj.value;
              break;
            }
          }
          if (!flag)
          {
            int index3 = 0;
            if (simplifiedItemList.Count > 0)
            {
              index3 = 0;
              while (index3 < simplifiedItemList.Count && (double) simplifiedItemList[index3].give_energy >= (double) give_energy)
                ++index3;
            }
            if (index3 == simplifiedItemList.Count)
              simplifiedItemList.Add(new SimplifiedItem(obj.id, obj.value, give_energy));
            else
              simplifiedItemList.Insert(index3, new SimplifiedItem(obj.id, obj.value, give_energy));
          }
        }
      }
    }
    int count = inventory.Count;
    Debug.Log((object) ("camp_storage_inventory count:" + count.ToString()));
    count = simplifiedItemList.Count;
    Debug.Log((object) ("items_for_roll count:" + count.ToString()));
    float num = energy_amount;
    while ((double) num > 0.0 && simplifiedItemList.Count != 0)
    {
      int index4 = 0;
      if (simplifiedItemList.Count > 4)
        index4 = UnityEngine.Random.Range(0, 4);
      SimplifiedItem simplifiedItem = simplifiedItemList[index4];
      num -= simplifiedItem.give_energy;
      bool flag = false;
      for (int index5 = 0; index5 < items_for_remove.Count; ++index5)
      {
        if (items_for_remove[index5].id == simplifiedItem.id)
        {
          ++items_for_remove[index5].count;
          flag = true;
          break;
        }
      }
      if (!flag)
        items_for_remove.Add(new SimplifiedItem(simplifiedItem.id, 1, simplifiedItem.give_energy));
      --simplifiedItem.count;
      if (simplifiedItem.count == 0)
        simplifiedItemList.RemoveAt(index4);
    }
    return (energy_amount - num) / energy_amount;
  }

  public float GetEnergyFromMealInStorage(float energy_amount)
  {
    List<SimplifiedItem> items_for_remove;
    float fromMealInStorage = this.NeedEnergyMealFromStorage(energy_amount, out items_for_remove);
    for (int index = 0; index < items_for_remove.Count; ++index)
    {
      if (!this.camp_storage.data.RemoveItem(items_for_remove[index].id, items_for_remove[index].count))
        Debug.LogError((object) $"FATAL ERROR: can not remove item id=\"{items_for_remove[index].id}\", count={items_for_remove[index].count.ToString()}; from camp storage!");
    }
    Debug.Log((object) ("energy satiety: " + fromMealInStorage.ToString()));
    return fromMealInStorage;
  }

  public void UpdateCampProgressObjectSlots()
  {
    this.camp_progress_object.SetParam("refugee_available_slots", (float) Mathf.RoundToInt(this.camp_zone_quality));
  }

  public void UpdateHappiness(float happiness_delta)
  {
    float num1 = MainGame.me.player.GetParam("cur_refugee_happiness");
    int paramInt = this.camp_progress_object.GetParamInt("refugee_available_slots");
    float num2 = num1 + happiness_delta;
    if ((double) num2 < 0.0)
      num2 = 0.0f;
    if ((double) num2 > (double) paramInt)
      num2 = (float) paramInt;
    MainGame.me.player.SetParam("cur_refugee_happiness", num2);
  }

  public void UpdateCampProgressObjectHappinessItems(
    RefugeesCampEngine.UpdateHappinessItemsMode mode = RefugeesCampEngine.UpdateHappinessItemsMode.GameResUpdatesItem)
  {
    int itemsCount1 = this.camp_progress_object.data.GetItemsCount("refugee_happiness_item");
    int paramInt = this.camp_progress_object.GetParamInt("refugee_available_slots");
    int totalHappiness = this.GetTotalHappiness();
    if (mode == RefugeesCampEngine.UpdateHappinessItemsMode.GameResUpdatesItem)
    {
      if (totalHappiness >= itemsCount1)
      {
        this.camp_progress_object.data.AddItem("refugee_happiness_item", totalHappiness - itemsCount1);
      }
      else
      {
        if (totalHappiness >= itemsCount1)
          return;
        this.camp_progress_object.data.RemoveItem("refugee_happiness_item", itemsCount1 - totalHappiness);
      }
    }
    else
    {
      if (itemsCount1 > paramInt)
        this.camp_progress_object.data.RemoveItem("refugee_happiness_item", itemsCount1 - paramInt);
      int itemsCount2 = this.camp_progress_object.data.GetItemsCount("refugee_happiness_item");
      MainGame.me.player.AddToParams("cur_refugee_happiness", (float) (itemsCount2 - totalHappiness));
    }
  }

  public void UpdateCampProgressObjectBar()
  {
    this.camp_progress_object.SetParam("refugee_current_progress_bar", this.GetCampHappinessProgress());
    this.camp_progress_bar.UpdateBar();
  }

  public float GetCampHappinessProgress()
  {
    return MainGame.me.player.GetParam("cur_refugee_happiness") - (float) this.GetTotalHappiness();
  }

  public int GetTotalHappiness()
  {
    return Mathf.FloorToInt(MainGame.me.player.GetParam("cur_refugee_happiness"));
  }

  public WorldGameObject GetVacantTentForRefugee()
  {
    List<WorldGameObject> objectsByCustomTag = WorldMap.GetWorldGameObjectsByCustomTag("refuee_camp_tent");
    Dictionary<string, int> source = new Dictionary<string, int>();
    for (int index = 0; index < objectsByCustomTag.Count; ++index)
      source[objectsByCustomTag[index].obj_id] = objectsByCustomTag[index].obj_def.can_insert_items_limit;
    for (int index = 0; index < objectsByCustomTag.Count; ++index)
    {
      string objId = objectsByCustomTag[index].obj_id;
      Debug.Log((object) objId);
      source[objId] -= objectsByCustomTag[index].data.GetItemsCount("refugee_tent_place_available_item");
      Debug.Log((object) ("refugee_tent_place_available_item count: " + objectsByCustomTag[index].data.GetItemsCount("refugee_tent_place_available_item").ToString()));
      source[objId] -= objectsByCustomTag[index].data.GetItemsCount("refugee_tent_place_busy_item");
      Debug.Log((object) ("refugee_tent_place_busy_item count: " + objectsByCustomTag[index].data.GetItemsCount("refugee_tent_place_busy_item").ToString()));
      if (source[objId] == 0)
        source.Remove(objectsByCustomTag[index].obj_id);
    }
    if (source.Count != 1)
      Debug.LogError((object) ("Found vacant tents count: " + source.Count.ToString()));
    KeyValuePair<string, int> keyValuePair = source.First<KeyValuePair<string, int>>();
    int num = keyValuePair.Value;
    if (num != 1)
      Debug.LogError((object) ("Wrong vacant places count in tent: " + num.ToString()));
    keyValuePair = source.First<KeyValuePair<string, int>>();
    string key = keyValuePair.Key;
    Debug.Log((object) ("Vacant tent obj id place:" + key));
    WorldGameObject gameObjectByObjId = WorldMap.GetWorldGameObjectByObjId(key);
    gameObjectByObjId.AddToInventory("refugee_tent_place_busy_item", 1);
    return gameObjectByObjId;
  }

  public string GetHomeGDPointForTent(WorldGameObject tent_object)
  {
    float num = float.MaxValue;
    string homeGdPointForTent = string.Empty;
    for (int index = 0; index < this._home_additional_tents_gd_point_names_list.Count; ++index)
    {
      string tentsGdPointNames = this._home_additional_tents_gd_point_names_list[index];
      float magnitude = ((Vector2) WorldMap.GetGDPointByGDTag(tentsGdPointNames).transform.position - (Vector2) tent_object.transform.position).magnitude;
      if ((double) magnitude < (double) num)
      {
        homeGdPointForTent = tentsGdPointNames;
        num = magnitude;
      }
    }
    Debug.Log((object) $"nearest gd point with tag: {homeGdPointForTent}, and distance: {num.ToString()}");
    return homeGdPointForTent;
  }

  public void TEST_ADD_HAPPINESS()
  {
    MainGame.me.player.AddToParams("cur_refugee_happiness", 0.1f);
    this.UpdateCampProgressObjectBar();
    this.UpdateCampProgressObjectHappinessItems();
  }

  public void EnableGDPoint(string gd_point_tag_to_enable)
  {
    GDPoint gdPointByGdTag = WorldMap.GetGDPointByGDTag(gd_point_tag_to_enable);
    if ((UnityEngine.Object) gdPointByGdTag != (UnityEngine.Object) null)
    {
      gdPointByGdTag.gameObject.SetActive(true);
      RoundAndSortComponent[] componentsInChildren = gdPointByGdTag.GetComponentsInChildren<RoundAndSortComponent>(true);
      if (componentsInChildren == null || componentsInChildren.Length == 0)
        return;
      foreach (RoundAndSortComponent andSortComponent in componentsInChildren)
        andSortComponent.DoUpdateStuff(true);
    }
    else
      Debug.LogError((object) ("GD Point not found by gd tag: " + gd_point_tag_to_enable));
  }

  public enum UpdateHappinessItemsMode
  {
    ItemUpdatesGameRes,
    GameResUpdatesItem,
  }
}
