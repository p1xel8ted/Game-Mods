// Decompiled with JetBrains decompiler
// Type: WorldZone
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using LinqTools;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

#nullable disable
public class WorldZone : MonoBehaviour
{
  public static Action<string, float> OnQualityComputed;
  public string id = "";
  public static HashSet<string> _recalculated_zones = new HashSet<string>();
  public List<WorldGameObject> _wgos = new List<WorldGameObject>();
  public List<WorldGameObject> _totems = new List<WorldGameObject>();
  public static List<WorldZone> _all_zones = new List<WorldZone>();
  public List<Collider2D> _colliders;
  public WorldZoneDefinition _definition;
  [NonSerialized]
  public GameObject can_be_removed_group;
  public Collider2D whole_zone_collider;
  [SerializeField]
  public Transform _custom_zone_center;
  public string ovr_music;
  public List<string> smart_sounds = new List<string>();
  public static List<WorldGameObject> _pre_refresh_wgos = (List<WorldGameObject>) null;

  public WorldZoneDefinition definition
  {
    get
    {
      if (this._definition == null)
        this._definition = GameBalance.me.GetData<WorldZoneDefinition>(this.id);
      return this._definition;
    }
  }

  public Transform center_tf
  {
    get
    {
      return (UnityEngine.Object) this._custom_zone_center == (UnityEngine.Object) null ? this.transform : this._custom_zone_center;
    }
  }

  public bool IsAvailableForBuild() => true;

  public bool HasBuilder()
  {
    return (UnityEngine.Object) this.GetZoneWGOs().Find((Predicate<WorldGameObject>) (w => w.obj_def.interaction_type == ObjectDefinition.InteractionType.Builder)) != (UnityEngine.Object) null;
  }

  public static WorldZone GetZoneByID(string id, bool null_is_error = true)
  {
    foreach (WorldZone allZone in WorldZone._all_zones)
    {
      if (!(allZone.id != id) && !allZone.IsDisabled())
        return allZone;
    }
    if (null_is_error)
      Debug.LogError((object) $"Could't find zone [{id}]");
    return (WorldZone) null;
  }

  public static void InitZonesSystem()
  {
    WorldZone._recalculated_zones.Clear();
    WorldZone._all_zones = ((IEnumerable<WorldZone>) MainGame.me.world_root.GetComponentsInChildren<WorldZone>(true)).ToList<WorldZone>();
    Debug.Log((object) ("InitZonesSystem, zones = " + WorldZone._all_zones.Count.ToString()));
    foreach (WorldZone allZone in WorldZone._all_zones)
      allZone.Init();
  }

  public static void RecalculateAllZones()
  {
    Debug.Log((object) ("RecalculateAllZones, zones = " + WorldZone._all_zones.Count.ToString()));
    foreach (WorldZone allZone in WorldZone._all_zones)
      allZone.Recalculate();
  }

  public void PreRefreshZone()
  {
    Debug.Log((object) ("WorldZone.PreRefreshZone " + this.id), (UnityEngine.Object) this);
    WorldZone._pre_refresh_wgos = this._wgos;
    this._wgos = new List<WorldGameObject>();
  }

  public void PostRefreshZone()
  {
    Debug.Log((object) ("WorldZone.PostRefreshZone " + this.id), (UnityEngine.Object) this);
    WorldZone.MarkZoneAsDirty(this.id);
    foreach (WorldGameObject preRefreshWgo in WorldZone._pre_refresh_wgos)
      preRefreshWgo.RecalculateZoneBelonging();
    this.Recalculate();
  }

  public void Init()
  {
    this._colliders = ((IEnumerable<Collider2D>) this.GetComponentsInChildren<Collider2D>(true)).ToList<Collider2D>();
    this._wgos.Clear();
  }

  public static void MarkZoneAsDirty(string id)
  {
    if ((UnityEngine.Object) WorldZone.GetZoneByID(id) == (UnityEngine.Object) null || !WorldZone._recalculated_zones.Contains(id))
      return;
    WorldZone._recalculated_zones.Remove(id);
  }

  public void Update()
  {
    if (WorldZone._recalculated_zones.Contains(this.id))
      return;
    this.RecalculateZone();
  }

  public void RecalculateZone()
  {
    if (WorldZone._recalculated_zones.Contains(this.id))
      return;
    WorldZone._recalculated_zones.Add(this.id);
  }

  public bool DoesObjectBelongToZone(WorldGameObject wgo)
  {
    Vector2 position = (Vector2) wgo.transform.position;
    foreach (Collider2D collider in this._colliders)
    {
      if ((UnityEngine.Object) collider == (UnityEngine.Object) null)
        Debug.Log((object) $"WorldZone \"{this.gameObject.name}\" has null collider!");
      else if (collider.OverlapPoint(position))
      {
        if (!this._wgos.Contains(wgo))
        {
          foreach (WorldZone allZone in WorldZone._all_zones)
          {
            if (allZone._wgos.Contains(wgo))
              allZone._wgos.Remove(wgo);
          }
          this._wgos.Add(wgo);
        }
        return true;
      }
    }
    return false;
  }

  public static WorldZone GetZoneOfPoint(Vector2 pos)
  {
    foreach (WorldZone allZone in WorldZone._all_zones)
    {
      if (!allZone.IsDisabled())
      {
        foreach (Collider2D collider in allZone._colliders)
        {
          if (collider.OverlapPoint(pos))
            return allZone;
        }
      }
    }
    return (WorldZone) null;
  }

  public static WorldZone GetZoneOfObject(WorldGameObject wgo)
  {
    foreach (WorldZone allZone in WorldZone._all_zones)
    {
      if (!allZone.IsDisabled() && allZone.DoesObjectBelongToZone(wgo))
        return allZone;
    }
    return (WorldZone) null;
  }

  public void Recalculate()
  {
    this._totems.Clear();
    if (this.IsDisabled())
    {
      this._wgos.Clear();
    }
    else
    {
      List<WorldGameObject> worldGameObjectList = new List<WorldGameObject>();
      for (int index = 0; index < this._wgos.Count; ++index)
      {
        if ((UnityEngine.Object) this._wgos[index] == (UnityEngine.Object) null)
        {
          this._wgos.RemoveAt(index);
          --index;
        }
        else
        {
          WorldGameObject wgo = this._wgos[index];
          if (wgo.is_removed)
          {
            worldGameObjectList.Add(wgo);
          }
          else
          {
            if (wgo.obj_def.IsTotem())
              this._totems.Add(wgo);
            wgo.RecalculateGridShape();
          }
        }
      }
      foreach (WorldGameObject worldGameObject in worldGameObjectList)
        this._wgos.Remove(worldGameObject);
      this.RecalculateTotems();
    }
  }

  public void RecalculateTotems()
  {
    bool flag1 = false;
    bool flag2 = false;
    WorldGameObject worldGameObject = (WorldGameObject) null;
    if (MainGame.me.gui_elements.build_mode_gui.is_shown && (UnityEngine.Object) FloatingWorldGameObject.cur_floating != (UnityEngine.Object) null)
    {
      worldGameObject = FloatingWorldGameObject.cur_floating.wobj;
      if ((UnityEngine.Object) worldGameObject != (UnityEngine.Object) null)
      {
        if (worldGameObject.obj_def.IsTotem())
        {
          if (!this._totems.Contains(worldGameObject))
          {
            this._totems.Add(worldGameObject);
            flag1 = true;
          }
        }
        else if (!this._wgos.Contains(worldGameObject))
        {
          this._wgos.Add(worldGameObject);
          flag2 = true;
        }
      }
    }
    foreach (WorldGameObject wgo in this._wgos)
    {
      if (!wgo.obj_def.IsTotem())
      {
        wgo.totem_effect = new GameRes();
        foreach (WorldGameObject totem in this._totems)
        {
          if (totem.HasTotemInfluenceOnWGO(wgo))
            wgo.totem_effect += totem.obj_def.totem_params;
        }
      }
    }
    if (flag1)
      this._totems.Remove(worldGameObject);
    if (!flag2)
      return;
    this._wgos.Remove(worldGameObject);
  }

  public bool HasSoulsTotemInZone()
  {
    for (int index = 0; index < this._wgos.Count; ++index)
    {
      if (this._wgos[index].obj_def.type == ObjectDefinition.ObjType.SoulTotem)
        return true;
    }
    return false;
  }

  public void SortWGOSByPriority()
  {
    this._wgos = this._wgos.OrderByDescending<WorldGameObject, int>((Func<WorldGameObject, int>) (p => p.obj_def.multi_inventory_priority)).ToList<WorldGameObject>();
  }

  public float GetTotalQuality()
  {
    WorldZoneDefinition.QualityCalcMethod qualityCalcMethod = this.definition == null ? WorldZoneDefinition.QualityCalcMethod.Sum : this.definition.calc_method;
    if (qualityCalcMethod == WorldZoneDefinition.QualityCalcMethod.None)
      return 0.0f;
    float totalQuality = 0.0f;
    foreach (WorldGameObject wgo in this._wgos)
    {
      if (!wgo.obj_def.ignore_counting_at_zone)
        totalQuality += wgo.quality;
    }
    if (qualityCalcMethod == WorldZoneDefinition.QualityCalcMethod.Average)
      totalQuality /= (float) this._wgos.Count;
    Action<string, float> onQualityComputed = WorldZone.OnQualityComputed;
    if (onQualityComputed != null)
      onQualityComputed(this.id, totalQuality);
    return totalQuality;
  }

  public string GetQualityString()
  {
    string str = this.definition.string_format.Replace("@", this.definition.quality_icon);
    Regex regex = new Regex("^(.*?)\\{\\$([a-zA-Z0-9_]+):([^\\}]+)\\}(.*?)$");
    while (true)
    {
      Match match = regex.Match(str);
      if (match.Success)
        str = match.Groups[1].Captures[0]?.ToString() + string.Format($"{{0:{match.Groups[3].Captures[0]?.ToString()}}}", (object) MainGame.me.player.GetParam(match.Groups[2].Captures[0].ToString())) + match.Groups[4].Captures[0]?.ToString();
      else
        break;
    }
    Match match1 = new Regex("(.*)%([a-zA-Z_]+)(.*)").Match(str);
    if (match1.Success)
      str = match1.Groups[1].Captures[0]?.ToString() + WorldZone.GetZoneByID(match1.Groups[2].Captures[0].ToString()).GetTotalQuality().ToString() + match1.Groups[3].Captures[0]?.ToString();
    return string.Format(str, (object) this.GetTotalQuality());
  }

  public bool IsPlayerInZone() => this.DoesObjectBelongToZone(MainGame.me.player);

  public List<Inventory> GetMultiInventory(
    List<WorldGameObject> exceptions = null,
    MultiInventory.PlayerMultiInventory player_mi = MultiInventory.PlayerMultiInventory.DontChange,
    bool include_toolbelt = false,
    bool sortWGOS = false)
  {
    if (this._wgos == null || this._wgos.Count == 0)
      return (List<Inventory>) null;
    List<Inventory> multiInventory = new List<Inventory>();
    bool flag = false;
    if (sortWGOS)
      this.SortWGOSByPriority();
    foreach (WorldGameObject wgo in this._wgos)
    {
      if ((exceptions == null || !exceptions.Contains(wgo)) && !wgo.IsWorker())
      {
        if (wgo.is_player)
        {
          switch (player_mi)
          {
            case MultiInventory.PlayerMultiInventory.IncludePlayer:
              flag = true;
              break;
            case MultiInventory.PlayerMultiInventory.ExcludePlayer:
              continue;
          }
        }
        if (wgo.obj_def.open_in_multiinventory)
          multiInventory.Add(new Inventory(wgo));
      }
    }
    if (player_mi == MultiInventory.PlayerMultiInventory.IncludePlayer && !flag)
    {
      multiInventory.Add(new Inventory(MainGame.me.player.data));
      if (include_toolbelt)
      {
        Item data = new Item()
        {
          inventory = MainGame.me.player.data.secondary_inventory,
          inventory_size = 7
        };
        multiInventory.Add(new Inventory(data));
      }
    }
    return multiInventory;
  }

  public float GetTotalDarkPoints()
  {
    if (this.definition == null)
      return 0.0f;
    float totalDarkPoints = 0.0f;
    foreach (WorldGameObject wgo in this._wgos)
    {
      if (!((UnityEngine.Object) wgo == (UnityEngine.Object) null) && !(wgo.obj_id != "grave_ground"))
      {
        Item bodyFromInventory = wgo.GetBodyFromInventory();
        if (bodyFromInventory != null && (double) bodyFromInventory.GetParam("dark") > 0.0)
          totalDarkPoints += bodyFromInventory.GetParam("dark");
      }
    }
    return totalDarkPoints;
  }

  public List<WorldGameObject> GetDarkGraves()
  {
    if (this.definition == null)
      return (List<WorldGameObject>) null;
    List<WorldGameObject> darkGraves = new List<WorldGameObject>();
    foreach (WorldGameObject wgo in this._wgos)
    {
      if (!((UnityEngine.Object) wgo == (UnityEngine.Object) null) && !(wgo.obj_id != "grave_ground"))
      {
        Item bodyFromInventory = wgo.GetBodyFromInventory();
        if (bodyFromInventory != null && (double) bodyFromInventory.GetParam("dark") > 0.0)
          darkGraves.Add(wgo);
      }
    }
    return darkGraves;
  }

  public List<WorldGameObject> GetZoneWGOs() => this._wgos;

  public void OnPlayerEnter()
  {
    if (!MainGame.game_started)
      return;
    this.RedrawQualities();
    if (!string.IsNullOrEmpty(this.ovr_music))
      SmartAudioEngine.me.PlayOvrMusic(this.ovr_music);
    foreach (string smartSound in this.smart_sounds)
      SmartAudioEngine.me.PlaySoundWithFade(smartSound);
    MainGame.me.save.OnEnteredWorldZone(this);
  }

  public void OnPlayerExit()
  {
    this.RedrawQualities(new bool?(false));
    if (!string.IsNullOrEmpty(this.ovr_music))
      SmartAudioEngine.me.StopOvrMusic(this.ovr_music);
    foreach (string smartSound in this.smart_sounds)
      SmartAudioEngine.me.StopSoundWithFade(smartSound);
  }

  public Bounds GetBounds()
  {
    Bounds bounds1 = new Bounds();
    Bounds bounds2;
    if ((UnityEngine.Object) this.whole_zone_collider != (UnityEngine.Object) null)
    {
      bounds2 = this.whole_zone_collider.bounds;
    }
    else
    {
      if (this._colliders.Count == 0)
      {
        Debug.LogError((object) "Cannot get bounds of zone without colliders");
        return bounds1;
      }
      bounds2 = this._colliders[0].bounds;
      foreach (Collider2D collider in this._colliders)
        bounds2.Encapsulate(collider.bounds);
    }
    bounds2.min = (Vector3) (Vector2) bounds2.min;
    bounds2.max = (Vector3) (Vector2) bounds2.max;
    return bounds2;
  }

  public void RedrawQualities(bool? show = null, bool separate_k = false)
  {
    if (string.IsNullOrEmpty(this.definition.quality_icon))
      return;
    if (!show.HasValue)
      show = new bool?(MainGame.me.player_char.player.show_wgo_qualities);
    foreach (WorldGameObject wgo in this._wgos)
      wgo.SetQualityHint(show.Value);
  }

  public bool IsDisabled()
  {
    foreach (Component component in this.gameObject.GetComponentsInParent<GDPoint>(true))
    {
      if (!component.gameObject.activeSelf)
        return true;
    }
    return false;
  }

  public void PutToAllPossibleInventories(List<Item> drop_list, out List<Item> cant_insert)
  {
    foreach (WorldGameObject zoneWgO in this.GetZoneWGOs())
    {
      if (!((UnityEngine.Object) zoneWgO == (UnityEngine.Object) null))
      {
        ObjectDefinition objDef = zoneWgO.obj_def;
        if (objDef == null)
          Debug.LogError((object) $"Not found object definition for WGO \"{zoneWgO.name}\", obj_def={zoneWgO.obj_id}");
        else if (objDef.open_in_multiinventory)
        {
          bool flag = objDef.can_insert_items != null && objDef.can_insert_items.Count > 0;
          for (int index = 0; index < drop_list.Count; ++index)
          {
            Item drop = drop_list[index];
            if (!flag && !drop.definition.is_big || objDef.can_insert_items != null && objDef.can_insert_items.Contains(drop.id) && (objDef.can_insert_items_limit == 0 || objDef.can_insert_items_limit > zoneWgO.data.GetItemsCount(drop.id)))
            {
              int num1 = zoneWgO.data.CanAddCount(drop.id, true);
              if (num1 > 0)
              {
                int num2 = drop.value - num1;
                if (num2 > 0)
                {
                  Item obj = new Item(drop) { value = num1 };
                  zoneWgO.data.AddItem(obj);
                  drop.value = num2;
                }
                else
                {
                  zoneWgO.data.AddItem(drop);
                  drop_list.RemoveAt(index);
                  --index;
                }
              }
            }
          }
        }
      }
    }
    cant_insert = drop_list;
  }

  public void PutToAllPossibleInventoriesSmart(List<Item> drop_list, out List<Item> cant_insert)
  {
    cant_insert = new List<Item>();
    try
    {
      List<WorldGameObject> zoneWgOs = this.GetZoneWGOs();
      for (int index1 = 0; index1 < drop_list.Count; ++index1)
      {
        Item drop = drop_list[index1];
        if (!string.IsNullOrEmpty(drop?.id) && drop.value > 0)
        {
          SortedListWithDuplicatableKeys<WorldGameObject> duplicatableKeys = new SortedListWithDuplicatableKeys<WorldGameObject>();
          foreach (WorldGameObject worldGameObject in zoneWgOs)
          {
            if (worldGameObject?.obj_def != null && worldGameObject.obj_def.open_in_multiinventory)
              duplicatableKeys.Insert(worldGameObject.GetItemInsertionCoeff(drop), worldGameObject);
          }
          if (duplicatableKeys.Count != 0)
          {
            for (int index2 = duplicatableKeys.Count - 1; index2 >= 0; --index2)
            {
              WorldGameObject worldGameObject = duplicatableKeys.values[index2];
              if (!drop.definition.is_big && worldGameObject.obj_def.can_insert_items.Count <= 0 || worldGameObject.CanInsertItem(drop))
              {
                int num1 = worldGameObject.data.CanAddCount(drop.id, true);
                if (num1 > 0)
                {
                  int num2 = drop.value - num1;
                  if (num2 > 0)
                  {
                    Item obj = new Item(drop)
                    {
                      value = num1
                    };
                    worldGameObject.AddToInventory(obj);
                    drop.value = num2;
                  }
                  else
                  {
                    worldGameObject.AddToInventory(drop);
                    drop_list.RemoveAt(index1);
                    --index1;
                    break;
                  }
                }
              }
            }
          }
        }
      }
      cant_insert = drop_list;
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ex);
    }
  }

  public void EnableWorldZone()
  {
    foreach (Behaviour collider in this._colliders)
      collider.enabled = true;
    foreach (Behaviour component in this.GetComponents<Collider2D>())
      component.enabled = true;
    this.RefreshWGOsBelongingsToZone();
    this.Recalculate();
  }

  public void DisableWorldZone()
  {
    foreach (Behaviour collider in this._colliders)
      collider.enabled = false;
    foreach (Behaviour component in this.GetComponents<Collider2D>())
      component.enabled = false;
    this._wgos.Clear();
  }

  public void RefreshWGOsBelongingsToZone()
  {
    this._wgos.Clear();
    foreach (WorldGameObject wgo in WorldMap.objs)
    {
      if ((UnityEngine.Object) wgo != (UnityEngine.Object) null && this.DoesObjectBelongToZone(wgo))
        wgo.RecalculateZoneBelonging();
    }
  }
}
