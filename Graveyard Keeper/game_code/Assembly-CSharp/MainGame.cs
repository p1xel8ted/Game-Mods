// Decompiled with JetBrains decompiler
// Type: MainGame
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using CodeStage.AdvancedFPSCounter;
using Com.LuisPedroFonseca.ProCamera2D;
using FlowCanvas;
using LinqTools;
using ParadoxNotion.Services;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;

#nullable disable
public class MainGame : MonoBehaviour
{
  public const float RES_DEATH_LOOSE = 0.1f;
  public const bool BUILD_FOR_TRAILER = false;
  public static MainGame me;
  public static bool loaded_from_scene_main;
  public BuildGrid build_grid;
  public GameObject build_grid_cell;
  public DockPointMarker dock_point_marker;
  public WorldGameObject player;
  [NonSerialized]
  public World world;
  [HideInInspector]
  public BaseCharacterComponent player_char;
  public Camera _camera;
  public bool _camera_set;
  public UIRoot ui_root;
  public Com.LuisPedroFonseca.ProCamera2D.ProCamera2D pro_camera;
  public Camera world_cam;
  public Camera gui_cam;
  public MainGame.GameMode game_mode;
  public Transform world_root;
  public FloatingWorldGameObject _floating_obj;
  [NonSerialized]
  public GameSave save = new GameSave();
  [NonSerialized]
  public SaveSlotData save_slot = new SaveSlotData();
  public AstarPath astar;
  public GUIElements gui_elements;
  public static bool game_starting;
  public static bool _initial_preparations_done;
  public int gui_pixel_zoom = 2;
  [NonSerialized]
  public Vector3 player_pos = Vector3.zero;
  [NonSerialized]
  public BuildModeLogics build_mode_logics = new BuildModeLogics();
  public float _session_start;
  public static bool game_started;
  public static float camera_z;
  public static bool disable_all_game;
  public static float _start_time;
  public NoiseAndGrain grain_fx_component;
  public bool _editor_superfastforward_mode;
  public Texture2D mouse_cursor;
  public static bool paused;
  public TextureDrawer _dungeon_root;
  public bool _dungeon_root_set;
  public const string _STRANGER_SINS_POPUP_TEXT = "stranger_sins_popup_window";
  public const string _REFUGEES_POPUP_TEXT = "game_of_crone_popup_window";
  public const string _SOULS_POPUP_TEXT = "better_save_soul_popup_window";

  public static float game_time => (float) MainGame.me.save.day + TimeOfDay.me.GetTimeK();

  public static float session_time => MainGame.game_time - MainGame.me._session_start;

  public TextureDrawer dungeon_root
  {
    get
    {
      if (!this._dungeon_root_set)
      {
        this._dungeon_root = UnityEngine.Object.FindObjectOfType<TextureDrawer>();
        this._dungeon_root_set = true;
      }
      return this._dungeon_root;
    }
  }

  public PlayerComponent player_component => this.player.GetComponentInChildren<PlayerComponent>();

  public void Awake()
  {
    Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
    Application.SetStackTraceLogType(LogType.Warning, StackTraceLogType.None);
    if (MainGame._initial_preparations_done)
    {
      Debug.Log((object) "Second MainGame scene awake");
    }
    else
    {
      MainGame._initial_preparations_done = true;
      MainGame.game_starting = true;
      Preloader.OnMainGameLoaded();
      this.StartGameLoading();
    }
  }

  public void StartGameLoading()
  {
    MainGame.disable_all_game = false;
    MainGame._start_time = Time.realtimeSinceStartup;
    Debug.Log((object) ("StartGameLoading, time = " + Time.time.ToString()));
    MainGame.me = this;
    PlatformSpecific.Init();
    SmartResourceHelper.me.max_simultaneous_loading_files = 4;
    MainGame.PreloadResources();
    AstarPath.can_scan_on_startup = false;
    GameSettings.me.ApplyLanguageChange();
    EasySpritesCollection.Load();
    MainGame.camera_z = this.transform.position.z;
    this.grain_fx_component = this.GetComponent<NoiseAndGrain>();
    GameSettings.Init();
    Time.fixedDeltaTime = 0.0166666675f;
    string name = this.gameObject.scene.name;
    if (name.Contains("DontDestroyOnLoad"))
      name = SceneManager.GetActiveScene().name;
    Debug.Log((object) ("Starting scene: " + name), (UnityEngine.Object) this.gameObject);
    MainGame.loaded_from_scene_main = true;
    if (MainGame.loaded_from_scene_main)
      GameLoader.InitGameFromGUIScene();
    else
      GameLoader.InitGameFromWorldScene();
    DungeonRoomsContainer.FillRoomsDict();
  }

  public void GeneralInit()
  {
    Debug.Log((object) ("GeneralInit, time = " + Time.time.ToString()));
    Stats.Init();
    LazyEngine.Init(this.world_root, (LazyEngineCallbacks) new EngineCallbacks());
    ChunkManager.Init();
    GameAwakenerEngine.Init();
    DynamicLights.Init();
    LazyInput.Init();
    Sounds.Init();
    MonoManager.Create();
    GameSettings.me.ApplyVolume();
    if (!Preloader.awake_was_done)
      GameSettings.me.ApplyScreenMode();
    ShadowsPool.Init();
    Time.fixedDeltaTime = 0.0166666675f;
    this.player_char = this.player.components.character;
    this.gui_elements.Init();
    GameBalance.LoadGameBalance();
    ObjectDynamicShadow.InstantiateAllAdditionalShadows();
    this.save.obj_crafts.AddCraft("p:grave_place");
    this.save.obj_crafts.AddCraft("p:working_table");
    this.player.GetComponent<ChunkedGameObject>().always_active = true;
    this.gui_elements.Init();
    this.gui_elements.InitAtGameStart();
    TechDefinition.LinkTechs();
    CameraTools.ReCache();
    this._session_start = MainGame.game_time;
    if (!(bool) (UnityEngine.Object) this.ui_root)
      this.ui_root = UnityEngine.Object.FindObjectOfType<UIRoot>();
    if (!(bool) (UnityEngine.Object) this.pro_camera)
      this.pro_camera = UnityEngine.Object.FindObjectOfType<Com.LuisPedroFonseca.ProCamera2D.ProCamera2D>();
    this.OnScreenSizeChanged();
    this.player.GetComponent<WorldGameObject>().ApplyObjectGroup(GameBalance.me.GetData<ObjectGroupDefinition>("_char"));
    this.player.gameObject.SetActive(true);
    FogObject.InitFog(Resources.Load<FogObject>("fog object prefab"));
    MainGame.game_starting = false;
    CameraTools.MoveToPos((Vector2) this.player.tf.position);
    CustomUpdateManager.Init();
    CustomNetworkManager.is_running = UnityEngine.Object.FindObjectOfType<GameDebugConfiguration>().multiplayer;
    ChunkManager.RescanAllObjects();
    RoundAndSortComponent.DisableComponentOnStaticObjects();
    Debug.Log((object) ("GeneralInit - Done, time = " + Time.time.ToString()));
    MixerLightIntegration.Init();
    GJTimer.AddTimer(0.0f, (GJTimer.VoidDelegate) (() =>
    {
      EasySpritesCollection.LoadAllAtlasesAsync();
      DarkTonic.MasterAudio.MasterAudio.StopAllPlaylists();
      DarkTonic.MasterAudio.MasterAudio.TriggerPlaylistClip("menu", "menu");
      this.ShowDLCPopUpWindowIfNeed();
    }));
  }

  public void InitGUIScene(SceneDescription world_scene)
  {
    if ((UnityEngine.Object) world_scene == (UnityEngine.Object) null)
      Debug.LogError((object) "world_scene is null");
    if ((UnityEngine.Object) world_scene.main_camera == (UnityEngine.Object) null)
      Debug.LogError((object) "world_scene.main_camera is null");
    Debug.Log((object) $"InitGUIScene, this: {this.name}, world: {world_scene.main_camera.name}");
    MainGame.me = this;
    this.player = world_scene.main_camera.player;
    UnityEngine.Object.Destroy((UnityEngine.Object) world_scene.main_camera.gameObject.GetFirstParent());
    World.InitWorldOnApplicationStart();
    this.GeneralInit();
  }

  public void OnScreenSizeChanged(int w = -1, int h = -1)
  {
    if (w == -1)
      w = Screen.width;
    if (h == -1)
      h = Screen.height;
    Vector2 vector2 = new Vector2((float) w, (float) h);
    Debug.Log((object) $"OnScreenSizeChanged {vector2}");
    if (GameSettings.current_resolution == null)
    {
      Debug.LogError((object) $"Unsupported resolution {w}x{h}!");
      GameSettings.current_resolution = new ResolutionConfig(w, h);
    }
    Debug.Log((object) $"Setting camera size: {vector2 * 2f / (float) GameSettings.current_resolution.pixel_size} for pixel size: {GameSettings.current_resolution.pixel_size}");
    this.pro_camera.GetComponent<ProCamera2DPixelPerfect>().ViewportAutoScale = AutoScaleMode.None;
    this.GetComponent<Camera>().orthographicSize = (float) (h / GameSettings.current_resolution.pixel_size);
    this.ui_root.manualHeight = (int) vector2.y / this.gui_pixel_zoom;
    if ((UnityEngine.Object) GUIElements.me != (UnityEngine.Object) null)
      GUIElements.me.RecalcScreenResolution(w, h);
    ChunkManager.RecalculateResolution(w, h);
  }

  public GameObject InstantiatePrefab(string prefab_name)
  {
    GameObject original = Resources.Load<GameObject>(prefab_name);
    if ((UnityEngine.Object) original == (UnityEngine.Object) null)
    {
      Debug.LogError((object) ("Error loading prefab: " + prefab_name));
      return (GameObject) null;
    }
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
    gameObject.transform.SetParent(this.world_root, false);
    gameObject.transform.localPosition = Vector3.zero;
    return gameObject;
  }

  public void Update()
  {
    QualitySettings.vSyncCount = 1;
    if (MainGame.game_started)
      this.InGameUpdate();
    if (GamePadController.cheat_combination_pressed)
      UnityEngine.Object.FindObjectOfType<AFPSCounter>().SwitchCounter();
    CustomDebugInfoPanel.Update();
  }

  public void InGameUpdate()
  {
    this.save.game_logics.Update();
    if (!MainGame.paused)
      BuffsLogics.RecalculateBuffs();
    if (this.game_mode == MainGame.GameMode.Building)
      this.build_mode_logics.Update();
    if (!BaseGUI.all_guis_closed || this._editor_superfastforward_mode)
      return;
    UtilityStuff.ProcessInGameUpdate();
  }

  public void OnGameLoaded(GameSave save_data)
  {
    Debug.Log((object) nameof (OnGameLoaded));
    this.save = save_data;
    this.save.map.RestoreScene();
    Debug.Log((object) ("OnGameLoaded: complete, time = " + Time.time.ToString()));
  }

  public void RescanPathMap() => this.astar.Scan();

  public List<WorldGameObject> GetListOfWorldObjects()
  {
    return ((IEnumerable<WorldGameObject>) UnityEngine.Object.FindObjectsOfType<WorldGameObject>()).ToList<WorldGameObject>();
  }

  public void OnPlayerDied()
  {
    Debug.Log((object) nameof (OnPlayerDied));
    EnvironmentEngine.State engine_state = EnvironmentEngine.me.data.state;
    GDPoint gdPointByGdTag = WorldMap.GetGDPointByGDTag("gd_player_respawn");
    GameObject target;
    if ((UnityEngine.Object) gdPointByGdTag == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Can't find GD point: gd_player_respawn");
      target = MainGame.me.player.gameObject;
    }
    else
    {
      target = gdPointByGdTag.gameObject;
      engine_state = EnvironmentEngine.State.Inside;
    }
    MainGame.me.player.is_dead = true;
    GUIElements.me.overhead.Deactivate<OverheadGUI>();
    CameraTools.RemoveFromCameraTargets(this.player.transform, 0.0f);
    GS.SetPlayerEnable(false, true);
    EffectBubblesManager.RemoveAllBubbles();
    this.player_char.SetAnimationState(CharAnimState.Idle);
    this.player_char.LookAt(Direction.Down);
    this.player.transform.position = target.transform.position;
    this.player.RefreshPositionCache();
    EnvironmentEngine.me.SetEngineGlobalState(engine_state);
    string text = GJL.L("player_died");
    string str = "";
    string[] strArray = new string[3]{ "_r", "_g", "_b" };
    foreach (string param_name in strArray)
    {
      int num = Mathf.RoundToInt(this.player.GetParam(param_name) * 0.1f);
      if (num != 0)
      {
        this.player.SubParam(param_name, (float) num);
        if (str.Length > 0)
          str += " ";
        str = $"{str}({param_name}){num.ToString()}";
      }
    }
    if (!string.IsNullOrEmpty(str))
      text = $"{text}\n{GJL.L("you_lost")} {str.Replace("_", "")}";
    if (MainGame.me.dungeon_root.dungeon_is_loaded_now)
    {
      if (MainGame.me.dungeon_root.TrySaveDungeon())
        Debug.Log((object) "Successfully saved dungeon.");
      MainGame.me.dungeon_root.DestroyTiles();
      DarkTonic.MasterAudio.MasterAudio.StopPlaylist("ambient");
      SmartAudioEngine.me.StopOvrMusic("dungeon", true);
      EnvironmentEngine.me.SetEngineGlobalState(EnvironmentEngine.State.Inside);
    }
    GUIElements.me.dialog.OpenOK(text, (GJCommons.VoidDelegate) (() => CameraTools.Fade((GJCommons.VoidDelegate) (() =>
    {
      this.player.hp = (float) this.save.max_hp;
      this.player.AddToParams("deaths_count", 1f);
      this.save.quests.CheckKeyQuests("player_dead");
      this.save.quests.CheckKeyQuests("player_dead_" + this.player.GetParamInt("deaths_count").ToString());
      CameraTools.MoveToPos((Vector2) target.transform.position);
      CameraTools.AddToCameraTargets(this.player.transform);
      GJTimer.AddTimer(0.5f, (GJTimer.VoidDelegate) (() => CameraTools.UnFade((GJCommons.VoidDelegate) (() =>
      {
        this.player.is_dead = false;
        GS.SetPlayerEnable(true, true);
        this.save.quests.CheckKeyQuests("player_respawn");
        this.save.quests.CheckKeyQuests("player_respawn_" + this.player.GetParamInt("deaths_count").ToString());
      }), new float?(0.8f))));
    }), new float?(1.5f))));
  }

  public void RestartDemoBuild()
  {
    MainGame.me.player.gameObject.SetActive(false);
    Time.timeScale = 1f;
    UnityEngine.Object.Destroy((UnityEngine.Object) GameObject.Find("UI Root"));
    CameraTools.Fade((GJCommons.VoidDelegate) (() =>
    {
      Time.timeScale = 0.0f;
      Application.Quit();
    }));
  }

  public void SetMainPlayer(PlayerComponent p)
  {
    this.player = p.GetComponent<WorldGameObject>();
    this.player_char = p.wgo.components.character;
    this.player_char.can_be_locally_controlled = true;
    this.SetCameraPlayerFollow(p.gameObject.transform);
  }

  public void SetCameraPlayerFollow(Transform t) => CameraTools.AddToCameraTargets(t);

  public void EnterBuildMode(bool removing_mode = false)
  {
    this.game_mode = MainGame.GameMode.Building;
    Debug.Log((object) nameof (EnterBuildMode));
    this.player.components.character.control_enabled = false;
    if (removing_mode)
      this.build_mode_logics.EnterRemoveMode();
    else
      this.build_mode_logics.EnterBuildMode();
  }

  public void EnterScriptBuilding()
  {
    this.game_mode = MainGame.GameMode.Building;
    Debug.Log((object) "EnterWaitingScriptCallbackMode");
    this.build_mode_logics.EnterScriptBuilding();
  }

  public void ExitBuildMode()
  {
    this.game_mode = MainGame.GameMode.Normal;
    Debug.Log((object) nameof (ExitBuildMode));
    this.player.components.character.control_enabled = true;
  }

  public DungeonPreset TeleportToDungeonLevel(int level)
  {
    DungeonPreset preset = Resources.Load<DungeonPreset>("Dungeon/DungeonPresets/" + level.ToString());
    if ((UnityEngine.Object) preset == (UnityEngine.Object) null)
    {
      Debug.LogError((object) $"Dungeon preset, level = {level.ToString()} not found.");
      MainGame.me.player.Say("No more levels in alpha.", type: SpeechBubbleGUI.SpeechBubbleType.InfoBox);
      return (DungeonPreset) null;
    }
    Stats.DesignEvent($"Dungeon:{preset.dungeon_level.ToString()}:Load");
    if (this.dungeon_root.dungeon_is_loaded_now)
    {
      GJTimer.AddTimer(0.01f, (GJTimer.VoidDelegate) (() =>
      {
        if (this.dungeon_root.TrySaveDungeon())
          Debug.Log((object) "Successfully saved dungeon.");
        this.dungeon_root.DestroyTiles();
        this.dungeon_root.DrawTexture(preset);
        this.player.transform.position = this.dungeon_root.enter_to_dunge.transform.position;
        CameraTools.MoveToPos((Vector2) this.player.transform.position);
      }));
    }
    else
    {
      DarkTonic.MasterAudio.MasterAudio.PlaySound("dungeon_enter");
      DarkTonic.MasterAudio.MasterAudio.TriggerPlaylistClip("ambient", "dungeon_loop");
      SmartAudioEngine.me.PlayOvrMusic("dungeon");
      EnvironmentEngine.me.SetEngineGlobalState(EnvironmentEngine.State.Inside);
      if ((UnityEngine.Object) preset.environment_preset != (UnityEngine.Object) null)
        EnvironmentEngine.me.ApplyEnvironmentPreset(preset.environment_preset);
      this.dungeon_root.DrawTexture(preset);
      this.player.transform.position = this.dungeon_root.enter_to_dunge.transform.position;
      CameraTools.MoveToPos((Vector2) this.player.transform.position);
    }
    return preset;
  }

  public DungeonPreset TeleportToDungeonLevelCustom(int level, bool is_without_sound = true)
  {
    DungeonPreset preset = Resources.Load<DungeonPreset>("Dungeon/DungeonPresets/" + level.ToString());
    if ((UnityEngine.Object) preset == (UnityEngine.Object) null)
    {
      Debug.LogError((object) $"Dungeon preset, level = {level.ToString()} not found.");
      MainGame.me.player.Say("No more levels in alpha.", type: SpeechBubbleGUI.SpeechBubbleType.InfoBox);
      return (DungeonPreset) null;
    }
    Stats.DesignEvent($"Dungeon:{preset.dungeon_level.ToString()}:Load");
    if (this.dungeon_root.dungeon_is_loaded_now)
    {
      GJTimer.AddTimer(0.01f, (GJTimer.VoidDelegate) (() =>
      {
        if (this.dungeon_root.TrySaveDungeon())
          Debug.Log((object) "Successfully saved dungeon.");
        this.dungeon_root.DestroyTiles();
        this.dungeon_root.DrawTexture(preset);
        this.player.transform.position = this.dungeon_root.enter_to_dunge.transform.position;
        CameraTools.MoveToPos((Vector2) this.player.transform.position);
      }));
    }
    else
    {
      if (!is_without_sound)
      {
        DarkTonic.MasterAudio.MasterAudio.PlaySound("dungeon_enter");
        DarkTonic.MasterAudio.MasterAudio.TriggerPlaylistClip("ambient", "dungeon_loop");
        SmartAudioEngine.me.PlayOvrMusic("dungeon");
      }
      EnvironmentEngine.me.SetEngineGlobalState(EnvironmentEngine.State.Inside);
      if ((UnityEngine.Object) preset.environment_preset != (UnityEngine.Object) null)
        EnvironmentEngine.me.ApplyEnvironmentPreset(preset.environment_preset);
      this.dungeon_root.DrawTexture(preset);
      this.player.transform.position = this.dungeon_root.enter_to_dunge.transform.position;
      CameraTools.MoveToPos((Vector2) this.player.transform.position);
    }
    return preset;
  }

  public void OnExitDungeon()
  {
    DarkTonic.MasterAudio.MasterAudio.PlaySound("door");
    DarkTonic.MasterAudio.MasterAudio.StopPlaylist("ambient");
    SmartAudioEngine.me.StopOvrMusic("dungeon", true);
    EnvironmentEngine.me.SetEngineGlobalState(EnvironmentEngine.State.Inside);
    string id = "mortuary";
    Debug.Log((object) ("ApplyCurrentEnvironmentPreset, id = " + id));
    EnvironmentEngine.me.ApplyEnvironmentPreset(EnvironmentPreset.Load(id));
  }

  public void OpenBuildObjectGUI(WorldGameObject build_desk)
  {
    if (this.player.GetParamInt("in_tutorial") == 1 && this.player.GetParamInt("tut_shown_tut_1") == 0)
    {
      this.player.Say("cant_do_it_now");
    }
    else
    {
      BuildModeLogics.last_build_desk = build_desk;
      string id = build_desk.obj_def.id;
      CraftsInventory crafts_inventory = new CraftsInventory();
      foreach (ObjectCraftDefinition craft in GameBalance.me.craft_obj_data)
      {
        if (craft.builder_ids.Contains(id) && this.save.IsCraftVisible((CraftDefinition) craft))
          crafts_inventory.AddCraft(craft.id);
      }
      this.build_mode_logics.SetCurrentBuildZone(build_desk.obj_def.zone_id);
      this.gui_elements.craft.OpenAsBuild(build_desk, crafts_inventory);
      MainGame.paused = true;
    }
  }

  public void OnEquippedToolBroken(Item equipped_tool)
  {
    Debug.Log((object) ("OnEquippedToolBroken : " + equipped_tool.id));
    MainGame.me.player.UnEquipItem(equipped_tool);
  }

  public void OnGameStartedPlaying()
  {
    Debug.Log((object) ("<color=green>OnGameStartedPlaying</color>, time elapsed = " + Mathf.Round(Time.realtimeSinceStartup - MainGame._start_time).ToString()));
  }

  public static void PreloadResources()
  {
    Debug.Log((object) "SmartResourceHelper.PreloadResources");
    SmartResourceHelper.me.LoadListAsync<FlowScript>(Resources.Load<ResourceFileList>("res_scripts").files);
    SmartResourceHelper.me.LoadListAsync<MonoBehaviour>(Resources.Load<ResourceFileList>("res_wops").files);
  }

  public void LateUpdate()
  {
  }

  public void EnableHalfResolutionMode()
  {
    RenderTexture renderTexture = new RenderTexture(Screen.width / 2, Screen.height / 2, 0, RenderTextureFormat.Default);
    renderTexture.filterMode = FilterMode.Point;
    GUIElements.me.half_resolution_camera.gameObject.SetActive(true);
    GUIElements.me.half_resolution_mesh.sharedMaterial.mainTexture = (Texture) renderTexture;
    this.GetComponent<Camera>().targetTexture = renderTexture;
    this.GetComponent<Camera>().orthographicSize = (float) renderTexture.height;
    this.GetComponent<ProCamera2DPixelPerfect>().enabled = false;
  }

  public static void SetPausedMode(bool is_paused)
  {
    if (MainGame.paused == is_paused)
      return;
    Debug.Log((object) ("SetPausedMode: " + is_paused.ToString()));
    MainGame.paused = is_paused;
  }

  public void ShowDLCPopUpWindowIfNeed()
  {
    bool flag1 = !GameSettings.me.is_stranger_sins_popup_window_shown && DLCEngine.IsDLCAvailable(DLCEngine.DLCVersion.Stories);
    bool flag2 = !GameSettings.me.is_refugees_popup_window_shown && DLCEngine.IsDLCAvailable(DLCEngine.DLCVersion.Refugees);
    bool flag3 = !GameSettings.me.is_souls_popup_window_shown && DLCEngine.IsDLCAvailable(DLCEngine.DLCVersion.Souls);
    if (flag1)
      GameSettings.me.is_stranger_sins_popup_window_shown = true;
    if (flag2)
      GameSettings.me.is_refugees_popup_window_shown = true;
    if (flag3)
      GameSettings.me.is_souls_popup_window_shown = true;
    if (flag2 | flag3 | flag1)
      GameSettings.Save();
    if (flag1 & flag2 & flag3)
      GUIElements.me.dialog.OpenOK("stranger_sins_popup_window", text2: "game_of_crone_popup_window", separate_with_stars: true, text3: "better_save_soul_popup_window");
    else if (flag1 & flag2)
      GUIElements.me.dialog.OpenOK("stranger_sins_popup_window", text2: "game_of_crone_popup_window", separate_with_stars: true);
    else if (flag1 & flag3)
      GUIElements.me.dialog.OpenOK("stranger_sins_popup_window", text2: "better_save_soul_popup_window", separate_with_stars: true);
    else if (flag2 & flag3)
      GUIElements.me.dialog.OpenOK("game_of_crone_popup_window", text2: "better_save_soul_popup_window", separate_with_stars: true);
    else if (flag1)
      GUIElements.me.dialog.OpenOK("stranger_sins_popup_window");
    else if (flag2)
    {
      GUIElements.me.dialog.OpenOK("game_of_crone_popup_window");
    }
    else
    {
      if (!flag3)
        return;
      GUIElements.me.dialog.OpenOK("better_save_soul_popup_window");
    }
  }

  [CompilerGenerated]
  public void \u003CGeneralInit\u003Eb__53_0()
  {
    EasySpritesCollection.LoadAllAtlasesAsync();
    DarkTonic.MasterAudio.MasterAudio.StopAllPlaylists();
    DarkTonic.MasterAudio.MasterAudio.TriggerPlaylistClip("menu", "menu");
    this.ShowDLCPopUpWindowIfNeed();
  }

  public enum GameMode
  {
    Normal,
    Building,
  }
}
