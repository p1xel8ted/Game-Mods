// Decompiled with JetBrains decompiler
// Type: WorldMap
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using LinqTools;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class WorldMap
{
  public const string IS_DISABLED = "is_disabled";
  public static List<Item> _drop_items = new List<Item>();
  public static List<WorldGameObject> _objs = new List<WorldGameObject>();
  public static HashSet<long> _objs_ids = new HashSet<long>();
  public static List<WorldGameObject> _npcs = new List<WorldGameObject>();
  public static List<GDPoint> _gd_points = new List<GDPoint>();
  public static List<Vendor> _vendors = new List<Vendor>();
  public static HashSet<MobSpawner> _used_spawners = new HashSet<MobSpawner>();
  public static List<MobSpawner> _world_spawners = new List<MobSpawner>();

  public static void ClearWGOsList()
  {
    WorldMap._objs.Clear();
    WorldMap._npcs.Clear();
    WorldMap._objs_ids.Clear();
    WorldMap._used_spawners.Clear();
  }

  public static List<WorldGameObject> objs => WorldMap._objs;

  public static void RescanWGOsList()
  {
    WorldMap.ClearWGOsList();
    Debug.Log((object) "Rescan WGOs list");
    foreach (WorldGameObject componentsInChild in MainGame.me.world.GetComponentsInChildren<WorldGameObject>(true))
    {
      if (componentsInChild.unique_id == -1L)
        componentsInChild.unique_id = UniqueID.GetUniqueID();
      if (componentsInChild.obj_def == null)
      {
        Debug.LogError((object) ("obj def is null for obj " + componentsInChild.obj_id), (UnityEngine.Object) componentsInChild);
      }
      else
      {
        WorldMap._objs_ids.Add(componentsInChild.unique_id);
        WorldMap._objs.Add(componentsInChild);
        if (componentsInChild.obj_def.IsNPC())
          WorldMap._npcs.Add(componentsInChild);
        componentsInChild.GetParentGDPoint();
      }
    }
    ItemsDurabilityManager.Init(WorldMap._objs, WorldMap._drop_items);
  }

  public static void RescanSpawnersList()
  {
    Debug.Log((object) "Rescan Spawners list");
    WorldMap._world_spawners.Clear();
    foreach (MobSpawner componentsInChild in MainGame.me.world.GetComponentsInChildren<MobSpawner>(true))
    {
      if (componentsInChild.unique_id == -1L)
        componentsInChild.unique_id = UniqueID.GetUniqueID();
      WorldMap._world_spawners.Add(componentsInChild);
      componentsInChild.DelayedDeserialize();
    }
  }

  public static void RescanGDPoints(World world = null)
  {
    WorldMap._gd_points = ((IEnumerable<GDPoint>) ((UnityEngine.Object) world == (UnityEngine.Object) null ? (Component) MainGame.me.world : (Component) world).GetComponentsInChildren<GDPoint>(true)).ToList<GDPoint>();
    SubsceneLoadManager.GetGDPoints(WorldMap._gd_points);
    foreach (GDPoint gdPoint in WorldMap._gd_points)
      gdPoint.ResetPos();
    Debug.Log((object) ("RescanGDPoints, count = " + WorldMap._gd_points.Count.ToString()));
  }

  public static void ImportGDPointsOnLoadedScene(List<GDPoint> gd_points_list)
  {
    for (int index = 0; index < gd_points_list.Count; ++index)
      WorldMap._gd_points.Add(gd_points_list[index]);
    Debug.Log((object) ("All the worlds' gd-points on import count is " + WorldMap._gd_points.Count.ToString()));
  }

  public static void ExportGDPointsOnUnloadedScene(List<GDPoint> gd_points_list)
  {
    foreach (GDPoint gdPoints in gd_points_list)
    {
      if (WorldMap._gd_points.Contains(gdPoints))
        WorldMap._gd_points.Remove(gdPoints);
    }
    Debug.Log((object) ("All the worlds' gd-points on export count is " + WorldMap._gd_points.Count.ToString()));
  }

  public static List<MobSpawner> GetMobSpawnersByCustomTag(string c_tag)
  {
    List<MobSpawner> spawnersByCustomTag = new List<MobSpawner>();
    foreach (MobSpawner worldSpawner in WorldMap._world_spawners)
    {
      if (!((UnityEngine.Object) worldSpawner == (UnityEngine.Object) null) && worldSpawner.custom_tag == c_tag)
        spawnersByCustomTag.Add(worldSpawner);
    }
    return spawnersByCustomTag;
  }

  public static void OnNewDropItem(Item drop_item)
  {
    if (drop_item.is_tech_point || WorldMap._drop_items.Contains(drop_item))
      return;
    WorldMap._drop_items.Add(drop_item);
  }

  public static void OnDropItemRemoved(Item drop_item)
  {
    if (!WorldMap._drop_items.Contains(drop_item))
      return;
    WorldMap._drop_items.Remove(drop_item);
  }

  public static void OnAddNewWGO(WorldGameObject wgo)
  {
    if ((UnityEngine.Object) wgo == (UnityEngine.Object) null)
      return;
    if (wgo.obj_def == null)
    {
      Debug.LogError((object) ("OnAddNewWGO error: obj_def is null for obj: " + wgo.obj_id));
    }
    else
    {
      if (wgo.unique_id == -1L)
        wgo.unique_id = UniqueID.GetUniqueID();
      if (!WorldMap._objs_ids.Contains(wgo.unique_id))
      {
        WorldMap._objs_ids.Add(wgo.unique_id);
        WorldMap._objs.Add(wgo);
        if (wgo.obj_def.IsNPC())
          WorldMap._npcs.Add(wgo);
      }
      foreach (GDPoint componentsInChild in wgo.GetComponentsInChildren<GDPoint>(true))
      {
        if (!WorldMap._gd_points.Contains(componentsInChild))
          WorldMap._gd_points.Add(componentsInChild);
      }
    }
  }

  public static void OnDestroyWGO(WorldGameObject wgo)
  {
    if ((UnityEngine.Object) wgo == (UnityEngine.Object) null || !Application.isPlaying)
      return;
    foreach (GDPoint componentsInChild in wgo.GetComponentsInChildren<GDPoint>(true))
    {
      if (WorldMap._gd_points.Contains(componentsInChild))
        WorldMap._gd_points.Remove(componentsInChild);
    }
    if (wgo.unique_id == -1L)
      wgo.unique_id = UniqueID.GetUniqueID();
    if (!WorldMap._objs_ids.Contains(wgo.unique_id))
      return;
    WorldMap._objs.Remove(wgo);
    WorldMap._objs_ids.Remove(wgo.unique_id);
    if (wgo.obj_def == null || !wgo.obj_def.IsNPC())
      return;
    WorldMap._npcs.Remove(wgo);
  }

  public static void RescanDropItemsList()
  {
    WorldMap._drop_items.Clear();
    DropResGameObject[] componentsInChildren = MainGame.me.world_root.GetComponentsInChildren<DropResGameObject>(true);
    DropResGameObject context = (DropResGameObject) null;
    foreach (DropResGameObject dropResGameObject in componentsInChildren)
    {
      if ((UnityEngine.Object) dropResGameObject == (UnityEngine.Object) null || dropResGameObject.res == null || dropResGameObject.res.definition == null)
        context = dropResGameObject;
      else if (dropResGameObject.res.definition.has_durability)
        WorldMap._drop_items.Add(dropResGameObject.res);
    }
    if (!((UnityEngine.Object) context != (UnityEngine.Object) null))
      return;
    Debug.LogError((object) ("Found a wrong item in drops list. Removing. " + context.name), (UnityEngine.Object) context);
    context.is_collected = true;
  }

  public static WorldGameObject GetWorldGameObjectByCustomTag(
    string custom_tag,
    bool ignore_not_found_error = false)
  {
    WorldGameObject objectByComparator = WorldMap.GetWorldGameObjectByComparator((WorldMap.DoesWGOFitDelegate) (wgo => wgo.custom_tag == custom_tag));
    if (!ignore_not_found_error && (UnityEngine.Object) objectByComparator == (UnityEngine.Object) null)
      Debug.LogError((object) ("Error finding WGO by tag: " + custom_tag));
    return objectByComparator;
  }

  public static WorldGameObject GetWorldGameObjectByName(string name, bool ignore_not_found_error = false)
  {
    WorldGameObject objectByComparator = WorldMap.GetWorldGameObjectByComparator((WorldMap.DoesWGOFitDelegate) (wgo => wgo.name == name));
    if (!ignore_not_found_error && (UnityEngine.Object) objectByComparator == (UnityEngine.Object) null)
      Debug.LogError((object) ("Error finding WGO by name: " + name));
    return objectByComparator;
  }

  public static List<WorldGameObject> GetNPCsByComparator(
    WorldMap.DoesWGOFitDelegate dlg,
    bool ignore_not_found_error = false)
  {
    List<WorldGameObject> npCsByComparator = new List<WorldGameObject>();
    for (int index = 0; index < WorldMap._npcs.Count; ++index)
    {
      WorldGameObject npc = WorldMap._npcs[index];
      if ((UnityEngine.Object) npc == (UnityEngine.Object) null)
        Debug.LogError((object) "Null NPC in NPC list");
      else if (dlg(npc))
        npCsByComparator.Add(npc);
    }
    int index1 = 0;
    while (npCsByComparator.Count > 0)
    {
      if ((UnityEngine.Object) npCsByComparator[index1].transform.parent == (UnityEngine.Object) null || string.IsNullOrEmpty(npCsByComparator[index1].gameObject.scene.name))
        npCsByComparator.RemoveAt(index1);
      else
        ++index1;
      if (index1 >= npCsByComparator.Count)
        break;
    }
    if (!ignore_not_found_error && npCsByComparator.Count == 0)
      Debug.LogError((object) "NPC not found", (UnityEngine.Object) MainGame.me.world);
    return npCsByComparator;
  }

  public static WorldGameObject GetNPCByObjID(string npc_obj_id, bool ignore_not_found_error = false)
  {
    List<WorldGameObject> npCsByComparator = WorldMap.GetNPCsByComparator((WorldMap.DoesWGOFitDelegate) (npc => npc.obj_id == npc_obj_id), ignore_not_found_error);
    if (npCsByComparator == null)
    {
      Debug.LogError((object) ("Weird shit happen while finding NPC by ObjID: " + npc_obj_id));
      return (WorldGameObject) null;
    }
    return npCsByComparator.Count != 0 ? npCsByComparator[0] : (WorldGameObject) null;
  }

  public static WorldGameObject GetWorldGameObjectByComparator(WorldMap.DoesWGOFitDelegate dlg)
  {
    List<WorldGameObject> worldGameObjectList = new List<WorldGameObject>();
    List<int> intList = new List<int>();
    for (int index = 0; index < WorldMap._objs.Count; ++index)
    {
      WorldGameObject wgo = WorldMap._objs[index];
      if ((UnityEngine.Object) wgo == (UnityEngine.Object) null)
      {
        Debug.LogError((object) "Null WGO in WGO list");
        intList.Insert(0, index);
      }
      else if (!wgo.IsDisabled() && dlg(wgo))
        worldGameObjectList.Add(wgo);
    }
    foreach (int index in intList)
    {
      if ((UnityEngine.Object) WorldMap._objs[index] == (UnityEngine.Object) null)
        WorldMap._objs.RemoveAt(index);
    }
    while (worldGameObjectList.Count > 0 && ((UnityEngine.Object) worldGameObjectList[0].transform.parent == (UnityEngine.Object) null || string.IsNullOrEmpty(worldGameObjectList[0].gameObject.scene.name)))
      worldGameObjectList.RemoveAt(0);
    if (worldGameObjectList.Count == 0)
    {
      Debug.LogError((object) "World Object not found", (UnityEngine.Object) MainGame.me.world);
      return (WorldGameObject) null;
    }
    if (worldGameObjectList.Count > 1)
    {
      Debug.LogError((object) ("Warning! Multiple objects match this condition. Count = " + worldGameObjectList.Count.ToString()));
      int num = -1;
      foreach (WorldGameObject worldGameObject in worldGameObjectList)
      {
        ++num;
        Debug.Log((object) $"Object #{num.ToString()}: {worldGameObject.name}", (UnityEngine.Object) worldGameObject.gameObject);
      }
    }
    return worldGameObjectList[0];
  }

  public static List<WorldGameObject> GetWorldGameObjectsByCustomTag(
    string custom_tag,
    bool log_if_not_found = false)
  {
    List<WorldGameObject> objectsByComparator = WorldMap.GetWorldGameObjectsByComparator((WorldMap.DoesWGOFitDelegate) (wgo => wgo.custom_tag == custom_tag), log_if_not_found);
    if (objectsByComparator != null)
      return objectsByComparator;
    Debug.LogError((object) ("Weird shit happen while finding WGO by tag: " + custom_tag));
    return objectsByComparator;
  }

  public static List<WorldGameObject> GetWorldGameObjectsByComparator(
    WorldMap.DoesWGOFitDelegate dlg,
    bool log_if_not_found = false)
  {
    List<WorldGameObject> objectsByComparator = new List<WorldGameObject>();
    foreach (WorldGameObject wgo in WorldMap._objs)
    {
      if (dlg(wgo))
        objectsByComparator.Add(wgo);
    }
    if (!log_if_not_found || objectsByComparator.Count != 0)
      return objectsByComparator;
    Debug.LogError((object) "World Object not found", (UnityEngine.Object) MainGame.me.world);
    return (List<WorldGameObject>) null;
  }

  public static WorldGameObject GetWorldGameObjectByUniqueId(long instance_id, bool log_if_null = true)
  {
    if (instance_id == 0L)
    {
      Debug.LogError((object) "World Object not found, zero id");
      return (WorldGameObject) null;
    }
    foreach (WorldGameObject objectByUniqueId in WorldMap._objs)
    {
      if (objectByUniqueId.unique_id == instance_id)
        return objectByUniqueId;
    }
    if (log_if_null)
      Debug.LogError((object) $"World Object not found, object id = {instance_id.ToString()}, total objects: {WorldMap._objs.Count.ToString()}");
    return (WorldGameObject) null;
  }

  public static GDPoint GetGDPointByGDTag(string tag, bool log_if_null = true, bool skip_disabled = true)
  {
    foreach (GDPoint gdPoint in WorldMap._gd_points)
    {
      if (gdPoint.gd_tag == tag && (!skip_disabled || !gdPoint.IsDisabled()))
        return gdPoint;
    }
    if (log_if_null)
      Debug.LogError((object) ("GD point not found, object tag = " + tag));
    return (GDPoint) null;
  }

  public static List<GameObject> GetGDPointsByGDTag(string tag)
  {
    List<GameObject> gdPointsByGdTag = new List<GameObject>();
    foreach (GDPoint gdPoint in WorldMap._gd_points)
    {
      if (gdPoint.gd_tag == tag && !gdPoint.IsDisabled())
        gdPointsByGdTag.Add(gdPoint.gameObject);
    }
    return gdPointsByGdTag;
  }

  public static GDPoint GetGDPointByName(string name, bool log_if_null = true)
  {
    foreach (GDPoint gdPoint in WorldMap._gd_points)
    {
      if (gdPoint.name == name && !gdPoint.IsDisabled())
        return gdPoint;
    }
    if (log_if_null)
      Debug.LogError((object) ("GD point not found, object name) = " + name));
    return (GDPoint) null;
  }

  public static List<GameObject> GetGDPointsByName(string name)
  {
    List<GameObject> gdPointsByName = new List<GameObject>();
    foreach (GDPoint gdPoint in WorldMap._gd_points)
    {
      if (gdPoint.name == name && !gdPoint.IsDisabled())
        gdPointsByName.Add(gdPoint.gameObject);
    }
    return gdPointsByName;
  }

  public static GDPoint FIndNearestGDPointFromList(List<string> gd_point_tags, WorldGameObject wgo)
  {
    GDPoint nearestGdPointFromList = (GDPoint) null;
    float num1 = float.PositiveInfinity;
    foreach (string gdPointTag in gd_point_tags)
    {
      GDPoint gdPointByGdTag = WorldMap.GetGDPointByGDTag(gdPointTag);
      float num2 = Vector2.Distance((Vector2) wgo.transform.position, (Vector2) gdPointByGdTag.transform.position);
      if ((double) num2 <= (double) num1)
      {
        num1 = num2;
        nearestGdPointFromList = gdPointByGdTag;
      }
    }
    return nearestGdPointFromList;
  }

  public static List<WorldGameObject> GetWorldGameObjectsByObjId(string obj_id)
  {
    List<WorldGameObject> gameObjectsByObjId = new List<WorldGameObject>();
    foreach (WorldGameObject worldGameObject in WorldMap._objs)
    {
      if (worldGameObject.obj_id == obj_id)
        gameObjectsByObjId.Add(worldGameObject);
    }
    return gameObjectsByObjId;
  }

  public static WorldGameObject GetWorldGameObjectByObjId(
    string obj_id,
    bool ignore_not_found_error = false)
  {
    foreach (WorldGameObject gameObjectByObjId in WorldMap._objs)
    {
      if (gameObjectByObjId.obj_id == obj_id)
        return gameObjectByObjId;
    }
    if (!ignore_not_found_error)
      Debug.LogError((object) $"WGO with obj ID [{obj_id}] not found!");
    return (WorldGameObject) null;
  }

  public static List<WorldGameObject> GetWorldGameObjectsByObjId(string[] obj_ids)
  {
    List<WorldGameObject> gameObjectsByObjId = new List<WorldGameObject>();
    foreach (string objId in obj_ids)
      gameObjectsByObjId.AddRange((IEnumerable<WorldGameObject>) WorldMap.GetWorldGameObjectsByObjId(objId));
    return gameObjectsByObjId;
  }

  public static WorldGameObject SpawnWGO(Transform parent, string obj_id, Vector3? pos = null)
  {
    WorldGameObject wgo = UnityEngine.Object.Instantiate<WorldGameObject>(Prefabs.wgo_prefab, parent);
    if (pos.HasValue)
      wgo.transform.position = pos.Value;
    WorldMap.ActivateGameObject(wgo.gameObject);
    wgo.SetObject(obj_id);
    GJTimer.AddTimer(0.5f, (GJTimer.VoidDelegate) (() =>
    {
      wgo.just_built = true;
      wgo.RedrawGroundSprites();
    }));
    wgo.OnJustSpawned();
    WorldMap.OnAddNewWGO(wgo);
    if (pos.HasValue)
      wgo.transform.position = pos.Value;
    return wgo;
  }

  public static WorldSimpleObject SpawnWSO(Transform parent, string obj_id, Vector2? pos = null)
  {
    WorldSimpleObject original = Resources.Load<WorldSimpleObject>("objects/WorldSimpleObjects/" + obj_id);
    if ((UnityEngine.Object) original == (UnityEngine.Object) null)
    {
      Debug.LogError((object) $"Can not spawn WSO \"{obj_id}\"");
      return (WorldSimpleObject) null;
    }
    WorldSimpleObject worldSimpleObject = UnityEngine.Object.Instantiate<WorldSimpleObject>(original, parent);
    if (pos.HasValue)
      worldSimpleObject.transform.position = (Vector3) pos.Value;
    return worldSimpleObject;
  }

  public static void ActivateGameObject(GameObject go)
  {
    if ((UnityEngine.Object) go == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "ActivateGameObject: Trying to activate a null object");
    }
    else
    {
      try
      {
        go.SetActive(true);
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ("ActivateGameObject ERROR: " + ex?.ToString()));
        return;
      }
      if (!Application.isPlaying)
        return;
      GJTimer.AddTimer(0.01f, (GJTimer.VoidDelegate) (() =>
      {
        try
        {
          go.SetActive(true);
        }
        catch (Exception ex)
        {
          Debug.LogError((object) ("ActivateGameObject ERROR after timer: " + ex?.ToString()));
        }
      }));
    }
  }

  public static Ground.GroudType GetGroundType(Vector2 point)
  {
    Collider2D[] collider2DArray = Physics2D.OverlapPointAll(point);
    Ground.GroudType groundType = Ground.GroudType.None;
    int num = int.MinValue;
    foreach (Collider2D collider2D in collider2DArray)
    {
      SpriteRenderer component1 = collider2D.gameObject.GetComponent<SpriteRenderer>();
      if (!((UnityEngine.Object) component1 == (UnityEngine.Object) null))
      {
        Ground component2 = collider2D.gameObject.GetComponent<Ground>();
        if (!((UnityEngine.Object) component2 == (UnityEngine.Object) null))
        {
          int layerValueFromId = SortingLayer.GetLayerValueFromID(component1.sortingLayerID);
          if (layerValueFromId > num)
          {
            num = layerValueFromId;
            groundType = component2.type;
          }
        }
      }
    }
    return groundType;
  }

  public static void VendorsTradeWithBank()
  {
    WorldMap.FillVendorsList();
    Debug.Log((object) ("Started vendors level up. Total vendors count: " + WorldMap._vendors.Count.ToString()));
    foreach (Vendor vendor in WorldMap._vendors)
    {
      if (vendor == null)
        Debug.LogError((object) "Found null vendor in vendors list! Call Bulat!");
      else
        vendor.OnEndOfDay();
    }
  }

  public static void FillVendorsList()
  {
    WorldMap._vendors = new List<Vendor>();
    for (int index = 0; index < WorldMap._objs.Count; ++index)
    {
      if (!((UnityEngine.Object) WorldMap._objs[index] == (UnityEngine.Object) null))
      {
        Vendor vendor = WorldMap._objs[index].vendor;
        if (vendor != null)
          WorldMap._vendors.Add(vendor);
      }
    }
  }

  public static void AddVendor(Vendor new_vendor)
  {
    if (WorldMap._vendors == null || WorldMap._vendors.Count == 0)
      WorldMap.FillVendorsList();
    else
      WorldMap._vendors.Add(new_vendor);
  }

  public static void ToGameSave(GameSave save) => DropsList.me.ToGameSave(save);

  public static void FromGameSave(GameSave save) => DropsList.me.FromGameSave(save);

  public static void RestoreBubbles()
  {
    foreach (WorldGameObject worldGameObject in WorldMap._objs)
      worldGameObject.components.RefreshBubblesData(new bool?(false));
  }

  public static List<GDPoint> gd_points => WorldMap._gd_points;

  public static ChurchPulpit GetChurchPulpit()
  {
    WorldGameObject objectByCustomTag = WorldMap.GetWorldGameObjectByCustomTag("church_pulpit");
    ChurchPulpit churchPulpit = (ChurchPulpit) null;
    if ((UnityEngine.Object) objectByCustomTag == (UnityEngine.Object) null)
      Debug.LogError((object) "Couldn't find a church pulpit");
    else
      churchPulpit = objectByCustomTag.GetComponentsInChildren<ChurchPulpit>(true)[0];
    return churchPulpit;
  }

  public static void OnUsedSpawner(MobSpawner spawner)
  {
    if (!WorldMap._used_spawners.Contains(spawner))
      WorldMap._used_spawners.Add(spawner);
    if (WorldMap._world_spawners.Contains(spawner))
      return;
    WorldMap._world_spawners.Add(spawner);
  }

  public static MobSpawner GetSpawnerByCoords(Vector2 pos)
  {
    foreach (MobSpawner worldSpawner in WorldMap._world_spawners)
    {
      if (((Vector2) worldSpawner.transform.position).EqualsTo(pos))
        return worldSpawner;
    }
    Debug.LogWarning((object) $"Couldn't find a spawner by coords {pos.ToString()}, total_spawners = {WorldMap._world_spawners.Count.ToString()}");
    return (MobSpawner) null;
  }

  public static void DeserializeAllLinkedWorkers()
  {
    for (int index = 0; index < WorldMap._objs.Count; ++index)
    {
      WorldGameObject worldGameObject = WorldMap._objs[index];
      if (worldGameObject.linked_worker_unique_id <= 0L)
      {
        worldGameObject.linked_worker = (WorldGameObject) null;
      }
      else
      {
        WorldGameObject objectByUniqueId = WorldMap.GetWorldGameObjectByUniqueId(worldGameObject.linked_worker_unique_id);
        if ((UnityEngine.Object) objectByUniqueId == (UnityEngine.Object) null)
          Debug.LogError((object) $"FATAL ERROR: failed to deserialize linked worker: WGO with unique_id={worldGameObject.unique_id.ToString()} not found!");
        else
          worldGameObject.linked_worker = objectByUniqueId;
      }
    }
  }

  public static string RemoveZombieWorkerToStock(WorldGameObject worker, string gd_point_tag = "")
  {
    string empty = string.Empty;
    GDPoint gdPointByGdTag = WorldMap.GetGDPointByGDTag(!string.IsNullOrEmpty(gd_point_tag) ? gd_point_tag : "default_destroy_point");
    if ((UnityEngine.Object) gdPointByGdTag == (UnityEngine.Object) null)
      return $"{empty}Can't find GD point: {gd_point_tag}";
    Debug.Log((object) $"Teleporting {worker.obj_id} to GD point: {gdPointByGdTag.name}", (UnityEngine.Object) gdPointByGdTag.gameObject);
    worker.transform.position = gdPointByGdTag.transform.position;
    worker.RefreshPositionCache();
    worker.OnCameToGDPoint(gdPointByGdTag);
    worker.SetParam("is_disabled", 1f);
    if ((UnityEngine.Object) worker.linked_workbench != (UnityEngine.Object) null)
    {
      if (worker.linked_workbench.components.craft.is_crafting)
        worker.linked_workbench.OnWorkFinished();
      if (worker.linked_workbench.obj_def.type == ObjectDefinition.ObjType.PorterStation)
      {
        worker.linked_workbench.porter_station.state = PorterStation.PorterState.None;
        Item backpack = worker.worker.GetBackpack();
        if (backpack != null && backpack.inventory.Count > 0)
        {
          MainGame.me.player.DropItems(backpack.inventory);
          backpack.inventory = new List<Item>();
        }
      }
      worker.linked_workbench.linked_worker = (WorldGameObject) null;
    }
    worker.components.character.SetNoWorkerTool();
    worker.components.character.StopMovement();
    worker.worker.UpdateWorkerSkin(Worker.WorkerActivity.None);
    return empty;
  }

  public static string SpawnZombieWorkerFromStock(
    WorldGameObject workbench,
    Item zombie_worker_item,
    out WorldGameObject o,
    out bool is_success)
  {
    string empty = string.Empty;
    is_success = false;
    string str1 = empty + Worker.TransformWorker(Worker.WorkerState.ItemOverhead, zombie_worker_item, (WorldGameObject) null, Worker.WorkerState.WGO, out Item _, out o);
    if ((UnityEngine.Object) o == (UnityEngine.Object) null)
    {
      string str2 = str1 + "FATAL ERROR: not found worker_wgo!\n";
      return zombie_worker_item != null ? (zombie_worker_item.worker != null ? $"{str2}FATAL ERROR: worker {zombie_worker_item.worker.worker_unique_id.ToString()} has no worker_wgo!\n" : str2 + "FATAL ERROR: worker_item is NOT worker!\n") : str2 + "FATAL ERROR: worker_item is NULL!\n";
    }
    if (o.GetParamInt("is_disabled") == 0)
      Debug.LogError((object) $"Zombie_worker \"{o.name}\" is NOT disabled!");
    if ((UnityEngine.Object) workbench == (UnityEngine.Object) null)
      return str1 + "FATAL ERROR: workbench is null!";
    DockPoint dockPointForZombie = workbench.GetAvailableDockPointForZombie();
    if ((UnityEngine.Object) dockPointForZombie == (UnityEngine.Object) null)
    {
      Debug.Log((object) "Can not spawn zombie_worker: not found any available dock point!");
      return str1;
    }
    Debug.Log((object) $"Teleporting {o.obj_id} ({o.name}) to GD point: {workbench.name}", (UnityEngine.Object) workbench.gameObject);
    o.transform.position = dockPointForZombie.transform.position;
    o.RefreshPositionCache();
    o.gameObject.SetActive(true);
    o.components.character.LookAt(dockPointForZombie.action_dir);
    workbench.linked_worker = o;
    if (workbench.obj_def.type == ObjectDefinition.ObjType.PorterStation)
      workbench.porter_station.state = PorterStation.PorterState.Waiting;
    o.SetParam("is_disabled", 0.0f);
    Debug.Log((object) $"Teleporting, output name = {o.name}, obj_id = {o.obj_id}, instance_id = {o.gameObject.GetInstanceID().ToString()}");
    is_success = true;
    return str1;
  }

  public static bool AttachInvisibleWorker(
    WorldGameObject workbench,
    out WorldGameObject worker_wgo)
  {
    worker_wgo = (WorldGameObject) null;
    if ((UnityEngine.Object) workbench == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "AttachInvisibleWorker error: workbench is null!");
      return false;
    }
    if (workbench.is_dead || workbench.is_removed || workbench.is_removing)
    {
      Debug.LogError((object) "AttachInvisibleWorker error: workbench is dead or removed!");
      return false;
    }
    DockPoint dockPointForZombie = workbench.GetAvailableDockPointForZombie();
    if ((UnityEngine.Object) dockPointForZombie == (UnityEngine.Object) null)
    {
      Debug.Log((object) "AttachInvisibleWorker error: not found any available dock point!");
      return false;
    }
    WorkerDefinition data = GameBalance.me.GetData<WorkerDefinition>("worker_invisible");
    if (data == null)
    {
      Debug.Log((object) "AttachInvisibleWorker error: WorkerDefinition is null!");
      return false;
    }
    Item body = MainGame.me.save.GenerateBody(1, 3);
    worker_wgo = WorldMap.SpawnWGO(MainGame.me.world_root, data.worker_wgo, new Vector3?(dockPointForZombie.transform.position));
    Worker newWorker = MainGame.me.save.workers.CreateNewWorker(worker_wgo, data.id, body);
    newWorker.ForcingWorkerK(true);
    newWorker.UpdateWorkerLevel();
    workbench.linked_worker = worker_wgo;
    return true;
  }

  public static bool DoesWGOOnCoordsExist(string obj_id, Vector2 coords_vector2)
  {
    bool flag = false;
    float epsilon = 2f;
    List<WorldGameObject> gameObjectsByObjId = WorldMap.GetWorldGameObjectsByObjId(obj_id);
    if (!gameObjectsByObjId.IsNullOrEmpty<WorldGameObject>())
    {
      foreach (WorldGameObject worldGameObject in gameObjectsByObjId)
      {
        if (!((UnityEngine.Object) worldGameObject == (UnityEngine.Object) null))
        {
          Vector3 position = worldGameObject.transform.position;
          if (position.x.EqualsTo(coords_vector2.x, epsilon) && position.y.EqualsTo(coords_vector2.y, epsilon))
          {
            flag = true;
            break;
          }
        }
      }
    }
    return flag;
  }

  public static WorldGameObject SpawnWGO(
    Transform root,
    string obj_id,
    Vector2 coords,
    string custom_tag)
  {
    WorldGameObject worldGameObject = WorldMap.SpawnWGO(root, obj_id, new Vector3?((Vector3) coords));
    worldGameObject.custom_tag = custom_tag;
    worldGameObject.RecalculateZoneBelonging();
    return worldGameObject;
  }

  public static bool DeleteWGO(string[] possible_ids, Vector2 coords, string custom_tag)
  {
    if (possible_ids == null || possible_ids.Length == 0)
    {
      Debug.LogError((object) "DeleteWGO error: possible_ids is null or empty!");
      return false;
    }
    string empty = string.Empty;
    foreach (string possibleId in possible_ids)
    {
      if (!string.IsNullOrEmpty(empty))
        empty += ", ";
      empty += possibleId;
    }
    List<WorldGameObject> wgOs = WorldMap.FindWGOs(possible_ids, coords, custom_tag);
    if (wgOs != null && wgOs.Count > 0)
    {
      if (wgOs.Count > 1)
        Debug.LogWarning((object) $"Found more than one wgo with ids={{{empty}}}, coords={coords}, custom_tag={custom_tag}");
      Debug.Log((object) $"Destroying WGO with id={wgOs[0].obj_id}, coords={wgOs[0].transform.position}, custom_tag={wgOs[0].custom_tag}");
      wgOs[0].DestroyMe();
      return true;
    }
    Debug.LogError((object) $"DeleteWGO error: not deleted WGO: ids={{{empty}}}, coords={coords}, custom_tag={custom_tag}");
    return false;
  }

  public static bool MoveWGO(
    string[] possible_ids,
    Vector2 coords,
    string custom_tag,
    Vector2 new_coords)
  {
    if (possible_ids == null || possible_ids.Length == 0)
    {
      Debug.LogError((object) "MoveWGO error: possible_ids is null or empty!");
      return false;
    }
    string empty = string.Empty;
    foreach (string possibleId in possible_ids)
    {
      if (!string.IsNullOrEmpty(empty))
        empty += ", ";
      empty += possibleId;
    }
    List<WorldGameObject> wgOs = WorldMap.FindWGOs(possible_ids, coords, custom_tag);
    if (wgOs != null && wgOs.Count > 0)
    {
      if (wgOs.Count > 1)
        Debug.LogWarning((object) $"Found more than one wgo with ids={{{empty}}}, coords={coords}, custom_tag={custom_tag}");
      Debug.Log((object) $"Moveing WGO with id={wgOs[0].obj_id}, coords={wgOs[0].transform.position}, custom_tag={wgOs[0].custom_tag} to new pos={new_coords}");
      wgOs[0].transform.position = (Vector3) new_coords;
      wgOs[0].RecalculateZoneBelonging();
      return true;
    }
    Debug.LogError((object) $"MoveWGO error: not moved WGO: ids={{{empty}}}, coords={coords}, custom_tag={custom_tag}");
    return false;
  }

  public static List<WorldGameObject> FindWGOs(
    string[] possible_ids,
    Vector2 coords,
    string custom_tag)
  {
    if (possible_ids == null || possible_ids.Length == 0)
    {
      Debug.LogError((object) "FindWGO error: possible_ids is null or empty!");
      return (List<WorldGameObject>) null;
    }
    bool check_custom_tag = !string.IsNullOrEmpty(custom_tag);
    foreach (string possibleId in possible_ids)
    {
      string possible_id = possibleId;
      List<WorldGameObject> objectsByComparator = WorldMap.GetWorldGameObjectsByComparator((WorldMap.DoesWGOFitDelegate) (o => !(o.obj_id != possible_id) && (double) ((Vector2) o.transform.position - coords).sqrMagnitude <= 1.0 && (!check_custom_tag || !(o.custom_tag != custom_tag))));
      if (objectsByComparator != null && objectsByComparator.Count > 0)
        return objectsByComparator;
    }
    string empty = string.Empty;
    foreach (string possibleId in possible_ids)
    {
      if (!string.IsNullOrEmpty(empty))
        empty += ", ";
      empty += possibleId;
    }
    Debug.LogWarning((object) $"FindWGO warning: not found WGO: id={{{empty}}}, coords={coords}, custom_tag={custom_tag}");
    return (List<WorldGameObject>) null;
  }

  public static void ImportWGOsList(List<WorldGameObject> wgo_list)
  {
    foreach (WorldGameObject wgo in wgo_list)
      WorldMap._objs.Add(wgo);
    Debug.Log((object) ("WorldMap:ImportWGOsList, imported WGOs : " + wgo_list.Count.ToString()));
  }

  public static void ExportWGOsList(List<WorldGameObject> wgo_list)
  {
    int num = 0;
    foreach (WorldGameObject wgo in wgo_list)
    {
      if (WorldMap._objs.Contains(wgo))
      {
        ++num;
        WorldMap._objs.Remove(wgo);
      }
    }
    Debug.Log((object) $"WorldMap:ExportWGOsList, exported WGOs: {num.ToString()}/{wgo_list.Count.ToString()}");
  }

  public static void TryRemoveStackedChurchVisitors()
  {
    List<WorldGameObject> gameObjectsByObjId = WorldMap.GetWorldGameObjectsByObjId("npc_church_visitor");
    if (gameObjectsByObjId == null)
      return;
    int num = 0;
    foreach (WorldGameObject worldGameObject in gameObjectsByObjId)
    {
      if ((double) worldGameObject.pos.x >= -667.0 && (double) worldGameObject.pos.x <= 1131.0 && (double) worldGameObject.pos.y >= -8818.0 && (double) worldGameObject.pos.y <= -7919.0 && !worldGameObject.IsMoving())
      {
        ++num;
        worldGameObject.DestroyMe();
      }
    }
    Debug.Log((object) $"Remove stacked church visitors, count: {num}");
  }

  public delegate bool DoesWGOFitDelegate(WorldGameObject wgo);
}
