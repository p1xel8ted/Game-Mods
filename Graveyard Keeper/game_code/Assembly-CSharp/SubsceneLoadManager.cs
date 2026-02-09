// Decompiled with JetBrains decompiler
// Type: SubsceneLoadManager
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using LinqTools;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

#nullable disable
public static class SubsceneLoadManager
{
  public static List<SubsceneLoadManager.SubsceneLoader> _subscene_loaders = new List<SubsceneLoadManager.SubsceneLoader>();

  public static void Load(string scene_name, System.Action on_scene_loaded)
  {
    SubsceneLoadManager.SubsceneLoader subsceneLoader = new SubsceneLoadManager.SubsceneLoader(scene_name, on_scene_loaded);
    SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(subsceneLoader.OnSceneLoaded);
    SceneManager.LoadSceneAsync(scene_name, LoadSceneMode.Additive);
    SubsceneLoadManager._subscene_loaders.Add(subsceneLoader);
  }

  public static void Unload()
  {
    if (SubsceneLoadManager._subscene_loaders.Count <= 0)
      return;
    SubsceneLoadManager.SubsceneLoader subsceneLoader = SubsceneLoadManager._subscene_loaders.LastElement<SubsceneLoadManager.SubsceneLoader>();
    WorldMap.ExportWGOsList(subsceneLoader.Scene_wgos);
    WorldMap.ExportGDPointsOnUnloadedScene(subsceneLoader.Scene_gd_points);
    SubsceneLoadManager.RecalculateAStarForScene(subsceneLoader.Subscene_Load_Dependencies?.a_star_rescan_collider);
    SubsceneLoadManager.UpdateGraph(WorldMap.gd_points);
    SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(subsceneLoader.Name));
    SubsceneLoadManager._subscene_loaders.RemoveAt(SubsceneLoadManager._subscene_loaders.Count - 1);
  }

  public static void UnloadLastScene()
  {
    SubsceneLoadManager.Unload();
    Resources.UnloadUnusedAssets();
  }

  public static void UnloadAllScenes()
  {
    int count = SubsceneLoadManager._subscene_loaders.Count;
    for (int index = 0; index < count; ++index)
      SubsceneLoadManager.Unload();
    Resources.UnloadUnusedAssets();
  }

  public static void RescanGDPointsForLoadedScene(GameObject subworld, out List<GDPoint> gd_points)
  {
    gd_points = (List<GDPoint>) null;
    if (!((UnityEngine.Object) subworld != (UnityEngine.Object) null))
      return;
    gd_points = ((IEnumerable<GDPoint>) subworld.GetComponentsInChildren<GDPoint>(true)).ToList<GDPoint>();
    foreach (GDPoint gdPoint in gd_points)
      gdPoint.ResetPos();
    Debug.Log((object) ("RescanGDPoints, count = " + gd_points.Count.ToString()));
  }

  public static void GetGDPoints(List<GDPoint> gd_points_list)
  {
    foreach (SubsceneLoadManager.SubsceneLoader subsceneLoader in SubsceneLoadManager._subscene_loaders)
    {
      foreach (GDPoint sceneGdPoint in subsceneLoader.Scene_gd_points)
        gd_points_list.Add(sceneGdPoint);
    }
  }

  public static void CameraFlyToLastScene(GJCommons.VoidDelegate on_camera_moved)
  {
    if (SubsceneLoadManager._subscene_loaders.Count > 0)
    {
      SubsceneLoadManager.SubsceneLoader subsceneLoader = SubsceneLoadManager._subscene_loaders.Last<SubsceneLoadManager.SubsceneLoader>();
      GameObject cameraPos = subsceneLoader.Subscene_Load_Dependencies?.camera_pos;
      if ((UnityEngine.Object) cameraPos != (UnityEngine.Object) null)
      {
        CameraTools.CameraFlyTo(cameraPos.transform, on_camera_moved, 0.0f);
      }
      else
      {
        Debug.Log((object) "Subscene's camera pos didn't set. Choosing SubsceneLoadDependencies position");
        CameraTools.CameraFlyTo(subsceneLoader.Subscene_Load_Dependencies.transform, on_camera_moved, 0.0f);
      }
    }
    else
    {
      Debug.LogError((object) "There is no loaded scenes");
      on_camera_moved();
    }
  }

  public static void RecalculateAStarForScene(BoxCollider2D zone)
  {
    if ((UnityEngine.Object) zone != (UnityEngine.Object) null)
      AStarTools.UpdateAstarBounds(zone.bounds);
    else
      Debug.LogError((object) "Rescan zone is null");
  }

  public static SubsceneLoadDependencies FindSubsceneLoadDependenciesComponent(Scene s)
  {
    foreach (GameObject rootGameObject in s.GetRootGameObjects())
    {
      SubsceneLoadDependencies componentInChildren = rootGameObject.GetComponentInChildren<SubsceneLoadDependencies>();
      if ((UnityEngine.Object) componentInChildren != (UnityEngine.Object) null)
        return componentInChildren;
    }
    Debug.Log((object) ("SubsceneLoadDependencies component has not found in scene" + s.name));
    return (SubsceneLoadDependencies) null;
  }

  public static void UpdateGraph(List<GDPoint> gd_points_input)
  {
    ((GDPointGraph) AstarPath.active.astarData.FindGraphOfType(typeof (GDPointGraph))).UpdateGraph(gd_points_input);
    Debug.Log((object) ("SubsceneLoadManager:UpdateGraph, updated points: " + gd_points_input.Count.ToString()));
  }

  public static void ActivateWGOsOnLoadedScene(List<WorldGameObject> wgo_list)
  {
    foreach (Component wgo in wgo_list)
      WorldMap.ActivateGameObject(wgo.gameObject);
    Debug.Log((object) ("SubsceneLoadManager:ActivateWGOsOnLoadedScene, WGOs activated: " + wgo_list.Count.ToString()));
  }

  public static void ScanWGOsOnLoadedScene(GameObject subworld, out List<WorldGameObject> wgo_list)
  {
    wgo_list = (List<WorldGameObject>) null;
    if ((UnityEngine.Object) subworld != (UnityEngine.Object) null)
    {
      List<WorldGameObject> list = ((IEnumerable<WorldGameObject>) subworld.GetComponentsInChildren<WorldGameObject>(true)).ToList<WorldGameObject>();
      SubsceneLoadManager.ActivateWGOsOnLoadedScene(list);
      wgo_list = list;
    }
    else
      Debug.LogError((object) "SubsceneLoadManager:ScanWGOsOnLoadedScene, subworld is null");
  }

  public class SubsceneLoader
  {
    public System.Action _on_scene_loaded;
    public List<GDPoint> _scene_gd_points;
    public List<WorldGameObject> _scene_wgos;
    public SubsceneLoadDependencies _subscene_load_dependencies;
    [CompilerGenerated]
    public string \u003CName\u003Ek__BackingField;

    public string Name => this.\u003CName\u003Ek__BackingField;

    public List<GDPoint> Scene_gd_points => this._scene_gd_points;

    public List<WorldGameObject> Scene_wgos => this._scene_wgos;

    public SubsceneLoadDependencies Subscene_Load_Dependencies => this._subscene_load_dependencies;

    public SubsceneLoader(string scene_name, System.Action on_scene_loaded)
    {
      this.\u003CName\u003Ek__BackingField = scene_name;
      this._on_scene_loaded = on_scene_loaded;
    }

    public void OnSceneLoaded(Scene s, LoadSceneMode mode)
    {
      if (!(s.name == this.Name))
        return;
      SceneManager.sceneLoaded -= new UnityAction<Scene, LoadSceneMode>(this.OnSceneLoaded);
      this._subscene_load_dependencies = SubsceneLoadManager.FindSubsceneLoadDependenciesComponent(s);
      SubsceneLoadManager.ScanWGOsOnLoadedScene(this._subscene_load_dependencies.subworld, out this._scene_wgos);
      SubsceneLoadManager.RescanGDPointsForLoadedScene(this._subscene_load_dependencies.subworld, out this._scene_gd_points);
      WorldMap.ImportGDPointsOnLoadedScene(this._scene_gd_points);
      SubsceneLoadManager.RecalculateAStarForScene(this._subscene_load_dependencies?.a_star_rescan_collider);
      SubsceneLoadManager.UpdateGraph(WorldMap.gd_points);
      GJTimer.AddTimer(0.02f, (GJTimer.VoidDelegate) (() => this._on_scene_loaded()));
    }

    [CompilerGenerated]
    public void \u003COnSceneLoaded\u003Eb__14_0() => this._on_scene_loaded();
  }
}
