// Decompiled with JetBrains decompiler
// Type: GameLoader
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

#nullable disable
public static class GameLoader
{
  public static string _cur_loading_scene = "";
  public static SceneDescription _sd_world = (SceneDescription) null;
  public static SceneDescription _sd_main = (SceneDescription) null;
  public static PlayerComponent _player_prefab = (PlayerComponent) null;
  public static Vector3 player_prefab_pos = Vector3.zero;
  public static string initial_map_json = "";
  public static byte[] initial_map_bin;
  public static List<AsyncOperation> _additional_loaders = new List<AsyncOperation>();
  [CompilerGenerated]
  public static bool \u003Ccamera_initialized\u003Ek__BackingField;

  public static bool camera_initialized
  {
    get => GameLoader.\u003Ccamera_initialized\u003Ek__BackingField;
    set => GameLoader.\u003Ccamera_initialized\u003Ek__BackingField = value;
  }

  public static void InitGameFromGUIScene()
  {
    string scene_name = GameLoader.GetWorldSceneName();
    Debug.Log((object) ("Init game from GUI scene. Loading world scene: " + scene_name));
    LoadingGUI.ShowWithProgressBar();
    GJTimer.AddTimer(0.01f, (GJTimer.VoidDelegate) (() =>
    {
      GameLoader.CommonLoadPart();
      LoadingGUI.LinkAsyncProcess(GameLoader.StartLoadingScene(scene_name));
    }));
  }

  public static void InitGameFromWorldScene()
  {
    Debug.Log((object) "Init game from World scene");
    GameLoader.CommonLoadPart();
    GameLoader.FindPlayerPrefabOnTheScene(new Scene());
    GameLoader.StartLoadingScene("scene_main");
  }

  public static void CommonLoadPart()
  {
    if (false)
      return;
    AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("title_screen", LoadSceneMode.Additive);
    GameLoader._additional_loaders.Add(asyncOperation);
    SceneManager.LoadSceneAsync("intro", LoadSceneMode.Additive);
    GameLoader._additional_loaders.Add(asyncOperation);
  }

  public static bool IsAdditionalLoadersFinished()
  {
    foreach (AsyncOperation additionalLoader in GameLoader._additional_loaders)
    {
      if (!additionalLoader.isDone)
        return false;
    }
    return true;
  }

  public static void FindPlayerPrefabOnTheScene(Scene s)
  {
    GameLoader._player_prefab = UnityEngine.Object.FindObjectOfType<PlayerComponent>();
    if ((UnityEngine.Object) GameLoader._player_prefab == (UnityEngine.Object) null)
    {
      Debug.Log((object) ("FindPlayerPrefabOnTheScene " + s.name));
      foreach (GameObject rootGameObject in s.GetRootGameObjects())
      {
        Debug.Log((object) ("Checking obj " + rootGameObject.name), (UnityEngine.Object) rootGameObject);
        GameLoader._player_prefab = rootGameObject.GetComponent<PlayerComponent>();
        if (!((UnityEngine.Object) GameLoader._player_prefab != (UnityEngine.Object) null))
        {
          GameLoader._player_prefab = rootGameObject.GetComponentInChildren<PlayerComponent>(true);
          if ((UnityEngine.Object) GameLoader._player_prefab != (UnityEngine.Object) null)
            break;
        }
        else
          break;
      }
    }
    if ((UnityEngine.Object) GameLoader._player_prefab == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Player prefab not found on the scene");
    }
    else
    {
      GameLoader.player_prefab_pos = GameLoader._player_prefab.transform.position;
      UnityEngine.Object.Destroy((UnityEngine.Object) GameLoader._player_prefab.gameObject);
    }
  }

  public static string GetWorldSceneName() => "scene_graveyard";

  public static AsyncOperation StartLoadingScene(string scene_name)
  {
    Debug.Log((object) ("StartLoadingScene " + scene_name));
    GameLoader._cur_loading_scene = scene_name;
    SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(GameLoader.OnSceneLoaded);
    Application.backgroundLoadingPriority = ThreadPriority.Low;
    return SceneManager.LoadSceneAsync(scene_name, LoadSceneMode.Additive);
  }

  public static void OnSceneLoaded(Scene s, LoadSceneMode mode)
  {
    Debug.Log((object) ("OnSceneLoaded: " + s.name));
    if (s.name == "title_screen")
      GameLoader.OnTitleScreenSceneLoaded();
    if (s.name == "intro")
      GameLoader.OnIntroSceneLoaded(s);
    if (s.name != GameLoader._cur_loading_scene)
    {
      Debug.Log((object) "Skipping wrong scene");
    }
    else
    {
      World.InitWorldOnApplicationStart();
      foreach (SceneDescription sceneDescription in UnityEngine.Object.FindObjectsOfType<SceneDescription>())
      {
        switch (sceneDescription.scene_type)
        {
          case SceneDescription.SceneType.WorldMap:
            GameLoader._sd_world = sceneDescription;
            GameLoader.FindPlayerPrefabOnTheScene(sceneDescription.gameObject.scene);
            break;
          case SceneDescription.SceneType.MainScene:
            GameLoader._sd_main = sceneDescription;
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
      if ((UnityEngine.Object) GameLoader._sd_main == (UnityEngine.Object) null)
        Debug.LogError((object) "Scene description for the Main (GUI) scene not found!");
      else if ((UnityEngine.Object) GameLoader._sd_world == (UnityEngine.Object) null)
      {
        Debug.LogError((object) "Scene description for the World scene not found!");
      }
      else
      {
        MainGame.me.world.FindAndRemovePlayerPrefab();
        GameLoader._sd_main.main_camera.InitGUIScene(GameLoader._sd_world);
        GameLoader.initial_map_bin = MainGame.me.save.map.ToBinary(true);
        Com.LuisPedroFonseca.ProCamera2D.ProCamera2D.SetInstance(GameLoader._sd_main.main_camera.GetComponentInChildren<Com.LuisPedroFonseca.ProCamera2D.ProCamera2D>());
        GameLoader.EnableNetworkDisabledComponents();
        GJTimer.AddTimer(0.0f, (GJTimer.VoidDelegate) (() =>
        {
          GDPoint.StoreGDPointsState();
          GJTimer.AddTimer(0.02f, new GJTimer.VoidDelegate(GameLoader.OnBothMainScenesLoaded));
        }));
        Debug.Log((object) "OnSceneLoaded: Done");
      }
    }
  }

  public static void OnBothMainScenesLoaded()
  {
    if (!GameLoader.IsAdditionalLoadersFinished())
    {
      GJTimer.AddTimer(0.05f, new GJTimer.VoidDelegate(GameLoader.OnBothMainScenesLoaded));
    }
    else
    {
      GameLoader.camera_initialized = true;
      if (true)
        LoadingGUI.Hide();
      if (!Preloader.is_shown)
        return;
      Preloader.Hide();
    }
  }

  public static void OnTitleScreenSceneLoaded()
  {
    Debug.Log((object) nameof (OnTitleScreenSceneLoaded));
  }

  public static void OnIntroSceneLoaded(Scene s) => Debug.Log((object) nameof (OnIntroSceneLoaded));

  public static void EnableNetworkDisabledComponents()
  {
  }

  public static MainGame GetMainGameFromTheScene(Scene s)
  {
    foreach (GameObject rootGameObject in s.GetRootGameObjects())
    {
      Scene scene = rootGameObject.scene;
      if (scene.name != s.name)
      {
        string[] strArray = new string[6]
        {
          "Skipping go = ",
          rootGameObject.name,
          ", go.scene = ",
          null,
          null,
          null
        };
        scene = rootGameObject.scene;
        strArray[3] = scene.name;
        strArray[4] = ", looking for = ";
        strArray[5] = s.name;
        Debug.Log((object) string.Concat(strArray));
      }
      else
      {
        Debug.Log((object) ("Checking go = " + rootGameObject.name));
        MainGame component = rootGameObject.GetComponent<MainGame>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          return component;
        MainGame componentInChildren = rootGameObject.GetComponentInChildren<MainGame>();
        if ((UnityEngine.Object) componentInChildren != (UnityEngine.Object) null)
          return componentInChildren;
      }
    }
    Debug.LogError((object) "MainGame component not found in a loaded scene");
    return (MainGame) null;
  }

  public static PlayerComponent GetPlayerPrefab() => GameLoader._player_prefab;
}
