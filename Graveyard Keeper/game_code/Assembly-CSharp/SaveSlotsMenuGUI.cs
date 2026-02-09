// Decompiled with JetBrains decompiler
// Type: SaveSlotsMenuGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DG.Tweening;
using DLCRefugees;
using NodeCanvas.Framework;
using SmartPools;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class SaveSlotsMenuGUI : BaseMenuGUI
{
  public SaveSlotGUI _save_slot_prefab;
  public UIScrollView _scroll_view;
  public SimpleUITable _slots_table;
  public List<SaveSlotGUI> _slots = new List<SaveSlotGUI>();
  public List<SaveSlotData> _slot_datas = new List<SaveSlotData>();
  public SaveSlotData _gamepad_focused_slot;

  public override void Init()
  {
    this._save_slot_prefab = this.GetComponentInChildren<SaveSlotGUI>(true);
    this._save_slot_prefab.InitPrefab(this);
    this._scroll_view = this.GetComponentInChildren<UIScrollView>(true);
    this._slots_table = this.GetComponentInChildren<SimpleUITable>(true);
    this._slots_table.Reposition();
    this._scroll_view.ResetPosition();
    base.Init();
  }

  public override void Open()
  {
    this._gamepad_focused_slot = (SaveSlotData) null;
    base.Open();
    GUIElements.me.main_menu.Hide(true);
    this.button_tips.Clear();
    this.OnSlotsLoaded(new List<SaveSlotData>());
    PlatformSpecific.ReadSaveSlots(new PlatformSpecific.OnCompleteReadSaveSlotsDelegate(this.OnSlotsLoaded));
  }

  public new void LateUpdate()
  {
    if (!DOTween.IsTweening((object) this._scroll_view.transform) || !this._scroll_view.RestrictWithinBounds(false))
      return;
    this._scroll_view.transform.DOKill();
  }

  public void OnSlotsLoaded(List<SaveSlotData> slots)
  {
    Debug.Log((object) ("loaded slots count: " + slots.Count.ToString()));
    this.RedrawSlots(slots, true);
  }

  public void RedrawSlots(List<SaveSlotData> slot_datas, bool focus_on_first = false)
  {
    this.Clear();
    List<SaveSlotData> saveSlotDataList = new List<SaveSlotData>();
    foreach (SaveSlotData slotData in slot_datas)
    {
      if (slotData.game_time.EqualsTo(0.0f))
        saveSlotDataList.Add(slotData);
    }
    foreach (SaveSlotData slot in saveSlotDataList)
    {
      slot_datas.Remove(slot);
      PlatformSpecific.DeleteSlot(slot, (PlatformSpecific.OnCompleteDelegate) (() => { }));
    }
    this._slot_datas = slot_datas;
    SaveSlotGUI saveSlotGui1 = this._save_slot_prefab.Copy<SaveSlotGUI>();
    saveSlotGui1.Show((SaveSlotData) null);
    this._slots.Add(saveSlotGui1);
    foreach (SaveSlotData slotData in slot_datas)
    {
      SaveSlotGUI saveSlotGui2 = this._save_slot_prefab.Copy<SaveSlotGUI>();
      saveSlotGui2.Show(slotData);
      this._slots.Add(saveSlotGui2);
    }
    this._slots_table.Reposition();
    this._scroll_view.ResetPosition();
    if (!BaseGUI.for_gamepad || !this.is_shown)
      return;
    this.gamepad_controller.ReinitItems(false);
    if (!focus_on_first)
      return;
    this.gamepad_controller.FocusOnFirstActive();
  }

  public void Clear()
  {
    foreach (SaveSlotGUI slot in this._slots)
    {
      slot.Deactivate<SaveSlotGUI>();
      slot.DestroyGO<SaveSlotGUI>();
    }
    this._slots.Clear();
    this._scroll_view.transform.DOKill();
    this._scroll_view.ResetPosition();
  }

  public void OnSlotGamepadOvered(SaveSlotData slot_data, SaveSlotGUI slot_gui)
  {
    if (!BaseGUI.for_gamepad)
      return;
    this._gamepad_focused_slot = slot_data;
    List<GameKeyTip> tips = new List<GameKeyTip>();
    tips.Add(GameKeyTip.Select());
    if (slot_data != null)
      tips.Add(GameKeyTip.Option2("delete"));
    tips.Add(GameKeyTip.Close());
    this.button_tips.Print(tips);
  }

  public void OnSelectSlotPressed(SaveSlotData slot)
  {
    Debug.Log((object) ("OnSelectSlotPressed, null = " + (slot == null).ToString()));
    this.Hide(true);
    if (slot == null)
    {
      LoadingGUI.Show((GJCommons.VoidDelegate) (() =>
      {
        LoadingGUI.ShowBlackBackground(true);
        this.HideWithMainMenu();
        GUIElements.me.hud.Hide();
        this.OnNewGame(new GJCommons.VoidDelegate(this.StartPlayingGame));
      }));
      LoadingGUI.LinkAsyncProcess((AsyncOperation) null);
      LoadingGUI.ShowProgressBar();
    }
    else
    {
      LoadingGUI.Show((GJCommons.VoidDelegate) (() =>
      {
        LoadingGUI.ShowBlackBackground(true);
        this.OnLoadSlot(slot, new GJCommons.VoidDelegate(this.StartPlayingGame));
      }));
      LoadingGUI.LinkAsyncProcess((AsyncOperation) null);
      LoadingGUI.ShowProgressBar();
    }
  }

  public static void PrepareScene()
  {
    Debug.Log((object) nameof (PrepareScene));
    CraftComponent.ClearCraftsListOnGameStart();
    MainGame.game_started = false;
  }

  public void StartPlayingGame()
  {
    Debug.Log((object) "Loading: StartPlayingGame");
    LoadingGUI.SetProgressBar(0.400000036f);
    if (MainGame.me.save.unique_id_iterator == -1L)
      MainGame.me.save.unique_id_iterator = 500000L;
    UniqueID.SetIterator(MainGame.me.save.unique_id_iterator);
    MainGame.SetPausedMode(false);
    MainGame.game_started = false;
    GUIElements.me.hud_enabled = true;
    GUIElements.me.hud.gameObject.GetComponent<UIPanel>().alpha = 1f;
    EnvironmentEngine.me.Init();
    KickComponent.ResetAtGameStart();
    ChunkManager.ClearChunksList();
    MainGame.me.world.FindAndRemovePlayerPrefab();
    MainGame.me.save.dungeons.SetGlobalSeed(MainGame.me.save.dungeon_seed);
    GUIElements.me.quest_list.ResetAtGameStart();
    MainGame.me.save.known_npcs.GetOrCreateNPC("player");
    MimicAnimationController.Init();
    Graph.globally_enabled = true;
    EasySpriteCollectionManager.EnsureAllAtlasesLoaded((System.Action) (() =>
    {
      LoadingGUI.SetProgressBar(0.450000018f);
      GJTimer.AddTimer(0.0f, (GJTimer.VoidDelegate) (() =>
      {
        Debug.Log((object) "Loading: StartPlayingGame 2");
        LoadingGUI.SetProgressBar(0.5f);
        GJTimer.AddTimer(0.0f, (GJTimer.VoidDelegate) (() =>
        {
          GameAwakenerEngine.ScanMap();
          LoadingGUI.SetProgressBar(0.6f);
          GameAwakenerEngine.PreWarm();
          GJTimer.AddConditionalChecker((GJTimer.BoolDelegate) (() => GameAwakenerEngine.prewarm_finished), (GJTimer.VoidDelegate) (() =>
          {
            if (GameAwakenerEngine.was_objects_to_prewarm == 0)
              return;
            LoadingGUI.SetProgressBar((float) (0.60000002384185791 + 0.19999998807907104 * (1.0 - (double) GameAwakenerEngine.left_objects_to_prewarm / (double) GameAwakenerEngine.was_objects_to_prewarm)));
          }), (GJTimer.VoidDelegate) (() =>
          {
            LoadingGUI.SetProgressBar(0.8f);
            GJTimer.AddTimer(0.0f, (GJTimer.VoidDelegate) (() =>
            {
              AStarTools.InitialAstarScan();
              LoadingGUI.IncreaseProgressBar();
              GJTimer.AddTimer(0.0f, (GJTimer.VoidDelegate) (() =>
              {
                AstarPath.active.FlushGraphUpdates();
                DropsList.me.RemoveAllDropsFromTheScene();
                WorldMap.RescanGDPoints();
                GDPoint.RestoreGDPointsState();
                MainGame.me.save.PrepareAfterLoad();
                MainGame.me.world_root.gameObject.SetActive(true);
                WorldZone.InitZonesSystem();
                ChunkManager.RescanAllObjects();
                WorldGameObject.InitAllWorldWGOs();
                WorldZone.RecalculateAllZones();
                PlayerComponent.SpawnPlayer(inventory: MainGame.me.save.GetSavedPlayerInventory());
                MainGame.me.save.quests.InitQuestSystem();
                MainGame.me.save.ApplyCurrentEnvironmentPreset();
                this.HideWithMainMenu();
                GUIElements.me.hud.Hide();
                MainGame.game_started = true;
                WorldMap.RescanWGOsList();
                WorldMap.RescanSpawnersList();
                WorldMap.FromGameSave(MainGame.me.save);
                WorldMap.RescanDropItemsList();
                GUIElements.me.quest_list.Redraw();
                LeaveTrailComponent.RemoveAllTrailsFromTheScene();
                ObjectDynamicShadow.InitOnGameStart();
                WorldMap.RestoreBubbles();
                MainGame.game_started = false;
                WorldMap.DeserializeAllLinkedWorkers();
                SaveGameFixer.OnAfterAllInits();
                CameraTools.Init();
                MainGame.me.save.map.DeserializeTechPoints();
                MainGame.me.save.players_tavern_engine.Init();
                RefugeesCampEngine.instance.Init();
                GJTimer.AddTimer(0.0f, (GJTimer.VoidDelegate) (() =>
                {
                  Debug.Log((object) "Loading: StartPlayingGame 3");
                  LoadingGUI.SetProgressBar(1f);
                  LoadingGUI.Hide();
                  Intro.ShowIntro((System.Action) (() =>
                  {
                    Debug.Log((object) "Loading: StartPlayingGame 4");
                    MainGame.game_started = true;
                    GUIElements.me.hud.Open();
                    GUIElements.me.buffs.Redraw();
                    if (MainGame.loaded_from_scene_main)
                    {
                      string gd_point_tag = "gd_player_respawn";
                      if (MainGame.me.player.GetParamInt("sleep_bed_number") > 0)
                        gd_point_tag = $"{gd_point_tag}_{MainGame.me.player.GetParamInt("sleep_bed_number").ToString()}";
                      MainGame.me.player.TeleportToGDPoint(gd_point_tag);
                    }
                    GameAwakenerEngine.StartRestoringSimplifiedObjects();
                    GJTimer.AddTimer(0.05f, (GJTimer.VoidDelegate) (() =>
                    {
                      bool flag = true;
                      DarkTonic.MasterAudio.MasterAudio.StopAllPlaylists();
                      DarkTonic.MasterAudio.MasterAudio.TriggerPlaylistClip("music", "main");
                      if (flag)
                        MainGame.me.save.quests.CheckKeyQuests("game_start");
                      MainGame.me.save.game_logics.Init();
                      MainGame.me.player.data.SetParam("speed", LazyConsts.PLAYER_SPEED);
                      MainGame.me.OnGameStartedPlaying();
                      SmartPooler.PausePool<WorldGameObject>();
                      PlatformSpecific.SetGameStatus(GameEvents.GameStatus.InGame);
                      MixerLightIntegration.OnStartPlayingGame();
                      FlowScriptEngine.StartAllBehaviours();
                      GJTimer.AddTimer(0.1f, (GJTimer.VoidDelegate) (() => LoadingGUI.ShowBlackBackground(false, true)));
                      MainGame.me.save.LateSaveFixer();
                      MainGame.me.save.GlobalEventsCheck();
                    }));
                  }));
                }));
              }));
            }));
          }));
        }));
      }));
    }));
  }

  public void StopPlayingGame()
  {
    Debug.Log((object) nameof (StopPlayingGame));
    GUIElements.me.tutorial_arrow.AttachToWGO((WorldGameObject) null);
    GUIElements.me.overhead.SetActive(false);
    MainGame.game_started = false;
    Graph.globally_enabled = false;
    GameAwakenerEngine.Stop();
    MultiAnswerGUI.HideAnyctive();
    GUIElements.me.buffs.Redraw();
    ItemsDurabilityManager.Stop();
    DropResHint.DestroyAll();
    InteractionBubbleGUI.DestroyAll();
    DropsList.me.RemoveAllDropsFromTheScene();
    TechPointsDrop.DestroyAllTechspointsBeforeGameExit();
    EnvironmentEngine.me.ResetStates();
    SmartPooler.ResumePool<WorldGameObject>();
    TechPointDrop.DestroyAll();
    MixerLightIntegration.OnStopPlayingGame();
    FlowScriptEngine.StopAllBehaviours();
    SubsceneLoadManager.UnloadAllScenes();
    SmartAudioEngine.me.StopAllSmartSounds();
  }

  public override void Hide(bool play_sound = true)
  {
    this._scroll_view.StopScrolling();
    this.Clear();
    base.Hide(play_sound);
  }

  public override void OnClosePressed()
  {
    base.OnClosePressed();
    GUIElements.me.hud.Hide();
    GUIElements.me.main_menu.Open(false);
  }

  public void HideWithMainMenu()
  {
    this.Hide(false);
    GUIElements.me.main_menu.Hide(false);
  }

  public void OnDeleteSlotPressed(SaveSlotData slot)
  {
    if (slot == null)
      return;
    if (BaseGUI.for_gamepad)
      this.gamepad_controller.Disable();
    GUIElements.me.dialog.OpenYesNo(GJL.L("delete_slot"), (GJCommons.VoidDelegate) (() => PlatformSpecific.DeleteSlot(slot, (PlatformSpecific.OnCompleteDelegate) (() =>
    {
      this._slot_datas.Remove(slot);
      this.RedrawSlots(this._slot_datas, true);
    }))), on_hide: (GJCommons.VoidDelegate) (() =>
    {
      if (!BaseGUI.for_gamepad)
        return;
      this.gamepad_controller.Enable(GamepadNavigationController.OpenMethod.GetAll);
    }));
  }

  public override bool OnPressedSelect()
  {
    if (BaseGUI.for_gamepad)
      this.OnSelectSlotPressed(this._gamepad_focused_slot);
    return BaseGUI.for_gamepad;
  }

  public override bool OnPressedBack()
  {
    this.OnClosePressed();
    GUIElements.me.main_menu.Open(true);
    return BaseGUI.for_gamepad;
  }

  public override bool OnPressedOption2()
  {
    if (BaseGUI.for_gamepad)
      this.OnDeleteSlotPressed(this._gamepad_focused_slot);
    return BaseGUI.for_gamepad;
  }

  public void OnSaveSlot()
  {
    PlatformSpecific.SaveGame((SaveSlotData) null, MainGame.me.save, (PlatformSpecific.OnSaveCompleteDelegate) (slot => PlatformSpecific.ReadSaveSlots(new PlatformSpecific.OnCompleteReadSaveSlotsDelegate(this.OnSlotsLoaded))));
  }

  public void OnNewGame(GJCommons.VoidDelegate on_loaded)
  {
    GameSave.CreateNewSave((System.Action) (() =>
    {
      MainGame.me.player = (WorldGameObject) null;
      Intro.need_show_first_intro = true;
      PlatformSpecific.SaveGame((SaveSlotData) null, MainGame.me.save, (PlatformSpecific.OnSaveCompleteDelegate) (slot =>
      {
        MainGame.me.save_slot = slot;
        GUIElements.me.hud.Hide();
        on_loaded();
      }));
    }));
  }

  public void OnLoadSlot(SaveSlotData slot, GJCommons.VoidDelegate on_loaded)
  {
    Intro.need_show_first_intro = false;
    PlatformSpecific.LoadGame(slot, (PlatformSpecific.OnGameLoadedDelegate) (save =>
    {
      if (save.unlocked_phrases.Contains("@cognac_about_1") && !DLCEngine.IsDLCAvailable(DLCEngine.DLCVersion.Stories))
      {
        this.Hide(true);
        LoadingGUI.Hide();
        LoadingGUI.ShowBlackBackground(false);
        GUIElements.me.dialog.OpenOK("you_need_dlc_stories", (GJCommons.VoidDelegate) (() => GUIElements.me.main_menu.Open(true)));
      }
      else if (save.unlocked_phrases.Contains("@zone_refugees_camp_tp") && !DLCEngine.IsDLCAvailable(DLCEngine.DLCVersion.Refugees))
      {
        this.Hide(true);
        LoadingGUI.Hide();
        LoadingGUI.ShowBlackBackground(false);
        GUIElements.me.dialog.OpenOK("you_need_dlc_stories", (GJCommons.VoidDelegate) (() => GUIElements.me.main_menu.Open(true)));
      }
      else if (save.unlocked_phrases.Contains("@souls_s_s4_ask_1") && !DLCEngine.IsDLCAvailable(DLCEngine.DLCVersion.Souls))
      {
        this.Hide(true);
        LoadingGUI.Hide();
        LoadingGUI.ShowBlackBackground(false);
        GUIElements.me.dialog.OpenOK("you_need_dlc_stories", (GJCommons.VoidDelegate) (() => GUIElements.me.main_menu.Open(true)));
      }
      else
      {
        MainGame.me.save = save;
        GJTimer.AddTimer(0.0f, (GJTimer.VoidDelegate) (() =>
        {
          LoadingGUI.IncreaseProgressBar();
          MainGame.me.save.map.RestoreScene();
          GJTimer.AddTimer(0.0f, (GJTimer.VoidDelegate) (() =>
          {
            LoadingGUI.IncreaseProgressBar();
            on_loaded();
          }));
        }));
      }
    }));
  }

  public void OnDeleteSlot(SaveSlotData slot, GJCommons.VoidDelegate on_deleted)
  {
    PlatformSpecific.DeleteSlot(slot, (PlatformSpecific.OnCompleteDelegate) (() => on_deleted()));
  }

  [CompilerGenerated]
  public void \u003CStartPlayingGame\u003Eb__15_0()
  {
    LoadingGUI.SetProgressBar(0.450000018f);
    GJTimer.AddTimer(0.0f, (GJTimer.VoidDelegate) (() =>
    {
      Debug.Log((object) "Loading: StartPlayingGame 2");
      LoadingGUI.SetProgressBar(0.5f);
      GJTimer.AddTimer(0.0f, (GJTimer.VoidDelegate) (() =>
      {
        GameAwakenerEngine.ScanMap();
        LoadingGUI.SetProgressBar(0.6f);
        GameAwakenerEngine.PreWarm();
        GJTimer.AddConditionalChecker((GJTimer.BoolDelegate) (() => GameAwakenerEngine.prewarm_finished), (GJTimer.VoidDelegate) (() =>
        {
          if (GameAwakenerEngine.was_objects_to_prewarm == 0)
            return;
          LoadingGUI.SetProgressBar((float) (0.60000002384185791 + 0.19999998807907104 * (1.0 - (double) GameAwakenerEngine.left_objects_to_prewarm / (double) GameAwakenerEngine.was_objects_to_prewarm)));
        }), (GJTimer.VoidDelegate) (() =>
        {
          LoadingGUI.SetProgressBar(0.8f);
          GJTimer.AddTimer(0.0f, (GJTimer.VoidDelegate) (() =>
          {
            AStarTools.InitialAstarScan();
            LoadingGUI.IncreaseProgressBar();
            GJTimer.AddTimer(0.0f, (GJTimer.VoidDelegate) (() =>
            {
              AstarPath.active.FlushGraphUpdates();
              DropsList.me.RemoveAllDropsFromTheScene();
              WorldMap.RescanGDPoints();
              GDPoint.RestoreGDPointsState();
              MainGame.me.save.PrepareAfterLoad();
              MainGame.me.world_root.gameObject.SetActive(true);
              WorldZone.InitZonesSystem();
              ChunkManager.RescanAllObjects();
              WorldGameObject.InitAllWorldWGOs();
              WorldZone.RecalculateAllZones();
              PlayerComponent.SpawnPlayer(inventory: MainGame.me.save.GetSavedPlayerInventory());
              MainGame.me.save.quests.InitQuestSystem();
              MainGame.me.save.ApplyCurrentEnvironmentPreset();
              this.HideWithMainMenu();
              GUIElements.me.hud.Hide();
              MainGame.game_started = true;
              WorldMap.RescanWGOsList();
              WorldMap.RescanSpawnersList();
              WorldMap.FromGameSave(MainGame.me.save);
              WorldMap.RescanDropItemsList();
              GUIElements.me.quest_list.Redraw();
              LeaveTrailComponent.RemoveAllTrailsFromTheScene();
              ObjectDynamicShadow.InitOnGameStart();
              WorldMap.RestoreBubbles();
              MainGame.game_started = false;
              WorldMap.DeserializeAllLinkedWorkers();
              SaveGameFixer.OnAfterAllInits();
              CameraTools.Init();
              MainGame.me.save.map.DeserializeTechPoints();
              MainGame.me.save.players_tavern_engine.Init();
              RefugeesCampEngine.instance.Init();
              GJTimer.AddTimer(0.0f, (GJTimer.VoidDelegate) (() =>
              {
                Debug.Log((object) "Loading: StartPlayingGame 3");
                LoadingGUI.SetProgressBar(1f);
                LoadingGUI.Hide();
                Intro.ShowIntro((System.Action) (() =>
                {
                  Debug.Log((object) "Loading: StartPlayingGame 4");
                  MainGame.game_started = true;
                  GUIElements.me.hud.Open();
                  GUIElements.me.buffs.Redraw();
                  if (MainGame.loaded_from_scene_main)
                  {
                    string gd_point_tag = "gd_player_respawn";
                    if (MainGame.me.player.GetParamInt("sleep_bed_number") > 0)
                      gd_point_tag = $"{gd_point_tag}_{MainGame.me.player.GetParamInt("sleep_bed_number").ToString()}";
                    MainGame.me.player.TeleportToGDPoint(gd_point_tag);
                  }
                  GameAwakenerEngine.StartRestoringSimplifiedObjects();
                  GJTimer.AddTimer(0.05f, (GJTimer.VoidDelegate) (() =>
                  {
                    bool flag = true;
                    DarkTonic.MasterAudio.MasterAudio.StopAllPlaylists();
                    DarkTonic.MasterAudio.MasterAudio.TriggerPlaylistClip("music", "main");
                    if (flag)
                      MainGame.me.save.quests.CheckKeyQuests("game_start");
                    MainGame.me.save.game_logics.Init();
                    MainGame.me.player.data.SetParam("speed", LazyConsts.PLAYER_SPEED);
                    MainGame.me.OnGameStartedPlaying();
                    SmartPooler.PausePool<WorldGameObject>();
                    PlatformSpecific.SetGameStatus(GameEvents.GameStatus.InGame);
                    MixerLightIntegration.OnStartPlayingGame();
                    FlowScriptEngine.StartAllBehaviours();
                    GJTimer.AddTimer(0.1f, (GJTimer.VoidDelegate) (() => LoadingGUI.ShowBlackBackground(false, true)));
                    MainGame.me.save.LateSaveFixer();
                    MainGame.me.save.GlobalEventsCheck();
                  }));
                }));
              }));
            }));
          }));
        }));
      }));
    }));
  }

  [CompilerGenerated]
  public void \u003CStartPlayingGame\u003Eb__15_1()
  {
    Debug.Log((object) "Loading: StartPlayingGame 2");
    LoadingGUI.SetProgressBar(0.5f);
    GJTimer.AddTimer(0.0f, (GJTimer.VoidDelegate) (() =>
    {
      GameAwakenerEngine.ScanMap();
      LoadingGUI.SetProgressBar(0.6f);
      GameAwakenerEngine.PreWarm();
      GJTimer.AddConditionalChecker((GJTimer.BoolDelegate) (() => GameAwakenerEngine.prewarm_finished), (GJTimer.VoidDelegate) (() =>
      {
        if (GameAwakenerEngine.was_objects_to_prewarm == 0)
          return;
        LoadingGUI.SetProgressBar((float) (0.60000002384185791 + 0.19999998807907104 * (1.0 - (double) GameAwakenerEngine.left_objects_to_prewarm / (double) GameAwakenerEngine.was_objects_to_prewarm)));
      }), (GJTimer.VoidDelegate) (() =>
      {
        LoadingGUI.SetProgressBar(0.8f);
        GJTimer.AddTimer(0.0f, (GJTimer.VoidDelegate) (() =>
        {
          AStarTools.InitialAstarScan();
          LoadingGUI.IncreaseProgressBar();
          GJTimer.AddTimer(0.0f, (GJTimer.VoidDelegate) (() =>
          {
            AstarPath.active.FlushGraphUpdates();
            DropsList.me.RemoveAllDropsFromTheScene();
            WorldMap.RescanGDPoints();
            GDPoint.RestoreGDPointsState();
            MainGame.me.save.PrepareAfterLoad();
            MainGame.me.world_root.gameObject.SetActive(true);
            WorldZone.InitZonesSystem();
            ChunkManager.RescanAllObjects();
            WorldGameObject.InitAllWorldWGOs();
            WorldZone.RecalculateAllZones();
            PlayerComponent.SpawnPlayer(inventory: MainGame.me.save.GetSavedPlayerInventory());
            MainGame.me.save.quests.InitQuestSystem();
            MainGame.me.save.ApplyCurrentEnvironmentPreset();
            this.HideWithMainMenu();
            GUIElements.me.hud.Hide();
            MainGame.game_started = true;
            WorldMap.RescanWGOsList();
            WorldMap.RescanSpawnersList();
            WorldMap.FromGameSave(MainGame.me.save);
            WorldMap.RescanDropItemsList();
            GUIElements.me.quest_list.Redraw();
            LeaveTrailComponent.RemoveAllTrailsFromTheScene();
            ObjectDynamicShadow.InitOnGameStart();
            WorldMap.RestoreBubbles();
            MainGame.game_started = false;
            WorldMap.DeserializeAllLinkedWorkers();
            SaveGameFixer.OnAfterAllInits();
            CameraTools.Init();
            MainGame.me.save.map.DeserializeTechPoints();
            MainGame.me.save.players_tavern_engine.Init();
            RefugeesCampEngine.instance.Init();
            GJTimer.AddTimer(0.0f, (GJTimer.VoidDelegate) (() =>
            {
              Debug.Log((object) "Loading: StartPlayingGame 3");
              LoadingGUI.SetProgressBar(1f);
              LoadingGUI.Hide();
              Intro.ShowIntro((System.Action) (() =>
              {
                Debug.Log((object) "Loading: StartPlayingGame 4");
                MainGame.game_started = true;
                GUIElements.me.hud.Open();
                GUIElements.me.buffs.Redraw();
                if (MainGame.loaded_from_scene_main)
                {
                  string gd_point_tag = "gd_player_respawn";
                  if (MainGame.me.player.GetParamInt("sleep_bed_number") > 0)
                    gd_point_tag = $"{gd_point_tag}_{MainGame.me.player.GetParamInt("sleep_bed_number").ToString()}";
                  MainGame.me.player.TeleportToGDPoint(gd_point_tag);
                }
                GameAwakenerEngine.StartRestoringSimplifiedObjects();
                GJTimer.AddTimer(0.05f, (GJTimer.VoidDelegate) (() =>
                {
                  bool flag = true;
                  DarkTonic.MasterAudio.MasterAudio.StopAllPlaylists();
                  DarkTonic.MasterAudio.MasterAudio.TriggerPlaylistClip("music", "main");
                  if (flag)
                    MainGame.me.save.quests.CheckKeyQuests("game_start");
                  MainGame.me.save.game_logics.Init();
                  MainGame.me.player.data.SetParam("speed", LazyConsts.PLAYER_SPEED);
                  MainGame.me.OnGameStartedPlaying();
                  SmartPooler.PausePool<WorldGameObject>();
                  PlatformSpecific.SetGameStatus(GameEvents.GameStatus.InGame);
                  MixerLightIntegration.OnStartPlayingGame();
                  FlowScriptEngine.StartAllBehaviours();
                  GJTimer.AddTimer(0.1f, (GJTimer.VoidDelegate) (() => LoadingGUI.ShowBlackBackground(false, true)));
                  MainGame.me.save.LateSaveFixer();
                  MainGame.me.save.GlobalEventsCheck();
                }));
              }));
            }));
          }));
        }));
      }));
    }));
  }

  [CompilerGenerated]
  public void \u003CStartPlayingGame\u003Eb__15_2()
  {
    GameAwakenerEngine.ScanMap();
    LoadingGUI.SetProgressBar(0.6f);
    GameAwakenerEngine.PreWarm();
    GJTimer.AddConditionalChecker((GJTimer.BoolDelegate) (() => GameAwakenerEngine.prewarm_finished), (GJTimer.VoidDelegate) (() =>
    {
      if (GameAwakenerEngine.was_objects_to_prewarm == 0)
        return;
      LoadingGUI.SetProgressBar((float) (0.60000002384185791 + 0.19999998807907104 * (1.0 - (double) GameAwakenerEngine.left_objects_to_prewarm / (double) GameAwakenerEngine.was_objects_to_prewarm)));
    }), (GJTimer.VoidDelegate) (() =>
    {
      LoadingGUI.SetProgressBar(0.8f);
      GJTimer.AddTimer(0.0f, (GJTimer.VoidDelegate) (() =>
      {
        AStarTools.InitialAstarScan();
        LoadingGUI.IncreaseProgressBar();
        GJTimer.AddTimer(0.0f, (GJTimer.VoidDelegate) (() =>
        {
          AstarPath.active.FlushGraphUpdates();
          DropsList.me.RemoveAllDropsFromTheScene();
          WorldMap.RescanGDPoints();
          GDPoint.RestoreGDPointsState();
          MainGame.me.save.PrepareAfterLoad();
          MainGame.me.world_root.gameObject.SetActive(true);
          WorldZone.InitZonesSystem();
          ChunkManager.RescanAllObjects();
          WorldGameObject.InitAllWorldWGOs();
          WorldZone.RecalculateAllZones();
          PlayerComponent.SpawnPlayer(inventory: MainGame.me.save.GetSavedPlayerInventory());
          MainGame.me.save.quests.InitQuestSystem();
          MainGame.me.save.ApplyCurrentEnvironmentPreset();
          this.HideWithMainMenu();
          GUIElements.me.hud.Hide();
          MainGame.game_started = true;
          WorldMap.RescanWGOsList();
          WorldMap.RescanSpawnersList();
          WorldMap.FromGameSave(MainGame.me.save);
          WorldMap.RescanDropItemsList();
          GUIElements.me.quest_list.Redraw();
          LeaveTrailComponent.RemoveAllTrailsFromTheScene();
          ObjectDynamicShadow.InitOnGameStart();
          WorldMap.RestoreBubbles();
          MainGame.game_started = false;
          WorldMap.DeserializeAllLinkedWorkers();
          SaveGameFixer.OnAfterAllInits();
          CameraTools.Init();
          MainGame.me.save.map.DeserializeTechPoints();
          MainGame.me.save.players_tavern_engine.Init();
          RefugeesCampEngine.instance.Init();
          GJTimer.AddTimer(0.0f, (GJTimer.VoidDelegate) (() =>
          {
            Debug.Log((object) "Loading: StartPlayingGame 3");
            LoadingGUI.SetProgressBar(1f);
            LoadingGUI.Hide();
            Intro.ShowIntro((System.Action) (() =>
            {
              Debug.Log((object) "Loading: StartPlayingGame 4");
              MainGame.game_started = true;
              GUIElements.me.hud.Open();
              GUIElements.me.buffs.Redraw();
              if (MainGame.loaded_from_scene_main)
              {
                string gd_point_tag = "gd_player_respawn";
                if (MainGame.me.player.GetParamInt("sleep_bed_number") > 0)
                  gd_point_tag = $"{gd_point_tag}_{MainGame.me.player.GetParamInt("sleep_bed_number").ToString()}";
                MainGame.me.player.TeleportToGDPoint(gd_point_tag);
              }
              GameAwakenerEngine.StartRestoringSimplifiedObjects();
              GJTimer.AddTimer(0.05f, (GJTimer.VoidDelegate) (() =>
              {
                bool flag = true;
                DarkTonic.MasterAudio.MasterAudio.StopAllPlaylists();
                DarkTonic.MasterAudio.MasterAudio.TriggerPlaylistClip("music", "main");
                if (flag)
                  MainGame.me.save.quests.CheckKeyQuests("game_start");
                MainGame.me.save.game_logics.Init();
                MainGame.me.player.data.SetParam("speed", LazyConsts.PLAYER_SPEED);
                MainGame.me.OnGameStartedPlaying();
                SmartPooler.PausePool<WorldGameObject>();
                PlatformSpecific.SetGameStatus(GameEvents.GameStatus.InGame);
                MixerLightIntegration.OnStartPlayingGame();
                FlowScriptEngine.StartAllBehaviours();
                GJTimer.AddTimer(0.1f, (GJTimer.VoidDelegate) (() => LoadingGUI.ShowBlackBackground(false, true)));
                MainGame.me.save.LateSaveFixer();
                MainGame.me.save.GlobalEventsCheck();
              }));
            }));
          }));
        }));
      }));
    }));
  }

  [CompilerGenerated]
  public void \u003CStartPlayingGame\u003Eb__15_5()
  {
    LoadingGUI.SetProgressBar(0.8f);
    GJTimer.AddTimer(0.0f, (GJTimer.VoidDelegate) (() =>
    {
      AStarTools.InitialAstarScan();
      LoadingGUI.IncreaseProgressBar();
      GJTimer.AddTimer(0.0f, (GJTimer.VoidDelegate) (() =>
      {
        AstarPath.active.FlushGraphUpdates();
        DropsList.me.RemoveAllDropsFromTheScene();
        WorldMap.RescanGDPoints();
        GDPoint.RestoreGDPointsState();
        MainGame.me.save.PrepareAfterLoad();
        MainGame.me.world_root.gameObject.SetActive(true);
        WorldZone.InitZonesSystem();
        ChunkManager.RescanAllObjects();
        WorldGameObject.InitAllWorldWGOs();
        WorldZone.RecalculateAllZones();
        PlayerComponent.SpawnPlayer(inventory: MainGame.me.save.GetSavedPlayerInventory());
        MainGame.me.save.quests.InitQuestSystem();
        MainGame.me.save.ApplyCurrentEnvironmentPreset();
        this.HideWithMainMenu();
        GUIElements.me.hud.Hide();
        MainGame.game_started = true;
        WorldMap.RescanWGOsList();
        WorldMap.RescanSpawnersList();
        WorldMap.FromGameSave(MainGame.me.save);
        WorldMap.RescanDropItemsList();
        GUIElements.me.quest_list.Redraw();
        LeaveTrailComponent.RemoveAllTrailsFromTheScene();
        ObjectDynamicShadow.InitOnGameStart();
        WorldMap.RestoreBubbles();
        MainGame.game_started = false;
        WorldMap.DeserializeAllLinkedWorkers();
        SaveGameFixer.OnAfterAllInits();
        CameraTools.Init();
        MainGame.me.save.map.DeserializeTechPoints();
        MainGame.me.save.players_tavern_engine.Init();
        RefugeesCampEngine.instance.Init();
        GJTimer.AddTimer(0.0f, (GJTimer.VoidDelegate) (() =>
        {
          Debug.Log((object) "Loading: StartPlayingGame 3");
          LoadingGUI.SetProgressBar(1f);
          LoadingGUI.Hide();
          Intro.ShowIntro((System.Action) (() =>
          {
            Debug.Log((object) "Loading: StartPlayingGame 4");
            MainGame.game_started = true;
            GUIElements.me.hud.Open();
            GUIElements.me.buffs.Redraw();
            if (MainGame.loaded_from_scene_main)
            {
              string gd_point_tag = "gd_player_respawn";
              if (MainGame.me.player.GetParamInt("sleep_bed_number") > 0)
                gd_point_tag = $"{gd_point_tag}_{MainGame.me.player.GetParamInt("sleep_bed_number").ToString()}";
              MainGame.me.player.TeleportToGDPoint(gd_point_tag);
            }
            GameAwakenerEngine.StartRestoringSimplifiedObjects();
            GJTimer.AddTimer(0.05f, (GJTimer.VoidDelegate) (() =>
            {
              bool flag = true;
              DarkTonic.MasterAudio.MasterAudio.StopAllPlaylists();
              DarkTonic.MasterAudio.MasterAudio.TriggerPlaylistClip("music", "main");
              if (flag)
                MainGame.me.save.quests.CheckKeyQuests("game_start");
              MainGame.me.save.game_logics.Init();
              MainGame.me.player.data.SetParam("speed", LazyConsts.PLAYER_SPEED);
              MainGame.me.OnGameStartedPlaying();
              SmartPooler.PausePool<WorldGameObject>();
              PlatformSpecific.SetGameStatus(GameEvents.GameStatus.InGame);
              MixerLightIntegration.OnStartPlayingGame();
              FlowScriptEngine.StartAllBehaviours();
              GJTimer.AddTimer(0.1f, (GJTimer.VoidDelegate) (() => LoadingGUI.ShowBlackBackground(false, true)));
              MainGame.me.save.LateSaveFixer();
              MainGame.me.save.GlobalEventsCheck();
            }));
          }));
        }));
      }));
    }));
  }

  [CompilerGenerated]
  public void \u003CStartPlayingGame\u003Eb__15_6()
  {
    AStarTools.InitialAstarScan();
    LoadingGUI.IncreaseProgressBar();
    GJTimer.AddTimer(0.0f, (GJTimer.VoidDelegate) (() =>
    {
      AstarPath.active.FlushGraphUpdates();
      DropsList.me.RemoveAllDropsFromTheScene();
      WorldMap.RescanGDPoints();
      GDPoint.RestoreGDPointsState();
      MainGame.me.save.PrepareAfterLoad();
      MainGame.me.world_root.gameObject.SetActive(true);
      WorldZone.InitZonesSystem();
      ChunkManager.RescanAllObjects();
      WorldGameObject.InitAllWorldWGOs();
      WorldZone.RecalculateAllZones();
      PlayerComponent.SpawnPlayer(inventory: MainGame.me.save.GetSavedPlayerInventory());
      MainGame.me.save.quests.InitQuestSystem();
      MainGame.me.save.ApplyCurrentEnvironmentPreset();
      this.HideWithMainMenu();
      GUIElements.me.hud.Hide();
      MainGame.game_started = true;
      WorldMap.RescanWGOsList();
      WorldMap.RescanSpawnersList();
      WorldMap.FromGameSave(MainGame.me.save);
      WorldMap.RescanDropItemsList();
      GUIElements.me.quest_list.Redraw();
      LeaveTrailComponent.RemoveAllTrailsFromTheScene();
      ObjectDynamicShadow.InitOnGameStart();
      WorldMap.RestoreBubbles();
      MainGame.game_started = false;
      WorldMap.DeserializeAllLinkedWorkers();
      SaveGameFixer.OnAfterAllInits();
      CameraTools.Init();
      MainGame.me.save.map.DeserializeTechPoints();
      MainGame.me.save.players_tavern_engine.Init();
      RefugeesCampEngine.instance.Init();
      GJTimer.AddTimer(0.0f, (GJTimer.VoidDelegate) (() =>
      {
        Debug.Log((object) "Loading: StartPlayingGame 3");
        LoadingGUI.SetProgressBar(1f);
        LoadingGUI.Hide();
        Intro.ShowIntro((System.Action) (() =>
        {
          Debug.Log((object) "Loading: StartPlayingGame 4");
          MainGame.game_started = true;
          GUIElements.me.hud.Open();
          GUIElements.me.buffs.Redraw();
          if (MainGame.loaded_from_scene_main)
          {
            string gd_point_tag = "gd_player_respawn";
            if (MainGame.me.player.GetParamInt("sleep_bed_number") > 0)
              gd_point_tag = $"{gd_point_tag}_{MainGame.me.player.GetParamInt("sleep_bed_number").ToString()}";
            MainGame.me.player.TeleportToGDPoint(gd_point_tag);
          }
          GameAwakenerEngine.StartRestoringSimplifiedObjects();
          GJTimer.AddTimer(0.05f, (GJTimer.VoidDelegate) (() =>
          {
            bool flag = true;
            DarkTonic.MasterAudio.MasterAudio.StopAllPlaylists();
            DarkTonic.MasterAudio.MasterAudio.TriggerPlaylistClip("music", "main");
            if (flag)
              MainGame.me.save.quests.CheckKeyQuests("game_start");
            MainGame.me.save.game_logics.Init();
            MainGame.me.player.data.SetParam("speed", LazyConsts.PLAYER_SPEED);
            MainGame.me.OnGameStartedPlaying();
            SmartPooler.PausePool<WorldGameObject>();
            PlatformSpecific.SetGameStatus(GameEvents.GameStatus.InGame);
            MixerLightIntegration.OnStartPlayingGame();
            FlowScriptEngine.StartAllBehaviours();
            GJTimer.AddTimer(0.1f, (GJTimer.VoidDelegate) (() => LoadingGUI.ShowBlackBackground(false, true)));
            MainGame.me.save.LateSaveFixer();
            MainGame.me.save.GlobalEventsCheck();
          }));
        }));
      }));
    }));
  }

  [CompilerGenerated]
  public void \u003CStartPlayingGame\u003Eb__15_7()
  {
    AstarPath.active.FlushGraphUpdates();
    DropsList.me.RemoveAllDropsFromTheScene();
    WorldMap.RescanGDPoints();
    GDPoint.RestoreGDPointsState();
    MainGame.me.save.PrepareAfterLoad();
    MainGame.me.world_root.gameObject.SetActive(true);
    WorldZone.InitZonesSystem();
    ChunkManager.RescanAllObjects();
    WorldGameObject.InitAllWorldWGOs();
    WorldZone.RecalculateAllZones();
    PlayerComponent.SpawnPlayer(inventory: MainGame.me.save.GetSavedPlayerInventory());
    MainGame.me.save.quests.InitQuestSystem();
    MainGame.me.save.ApplyCurrentEnvironmentPreset();
    this.HideWithMainMenu();
    GUIElements.me.hud.Hide();
    MainGame.game_started = true;
    WorldMap.RescanWGOsList();
    WorldMap.RescanSpawnersList();
    WorldMap.FromGameSave(MainGame.me.save);
    WorldMap.RescanDropItemsList();
    GUIElements.me.quest_list.Redraw();
    LeaveTrailComponent.RemoveAllTrailsFromTheScene();
    ObjectDynamicShadow.InitOnGameStart();
    WorldMap.RestoreBubbles();
    MainGame.game_started = false;
    WorldMap.DeserializeAllLinkedWorkers();
    SaveGameFixer.OnAfterAllInits();
    CameraTools.Init();
    MainGame.me.save.map.DeserializeTechPoints();
    MainGame.me.save.players_tavern_engine.Init();
    RefugeesCampEngine.instance.Init();
    GJTimer.AddTimer(0.0f, (GJTimer.VoidDelegate) (() =>
    {
      Debug.Log((object) "Loading: StartPlayingGame 3");
      LoadingGUI.SetProgressBar(1f);
      LoadingGUI.Hide();
      Intro.ShowIntro((System.Action) (() =>
      {
        Debug.Log((object) "Loading: StartPlayingGame 4");
        MainGame.game_started = true;
        GUIElements.me.hud.Open();
        GUIElements.me.buffs.Redraw();
        if (MainGame.loaded_from_scene_main)
        {
          string gd_point_tag = "gd_player_respawn";
          if (MainGame.me.player.GetParamInt("sleep_bed_number") > 0)
            gd_point_tag = $"{gd_point_tag}_{MainGame.me.player.GetParamInt("sleep_bed_number").ToString()}";
          MainGame.me.player.TeleportToGDPoint(gd_point_tag);
        }
        GameAwakenerEngine.StartRestoringSimplifiedObjects();
        GJTimer.AddTimer(0.05f, (GJTimer.VoidDelegate) (() =>
        {
          bool flag = true;
          DarkTonic.MasterAudio.MasterAudio.StopAllPlaylists();
          DarkTonic.MasterAudio.MasterAudio.TriggerPlaylistClip("music", "main");
          if (flag)
            MainGame.me.save.quests.CheckKeyQuests("game_start");
          MainGame.me.save.game_logics.Init();
          MainGame.me.player.data.SetParam("speed", LazyConsts.PLAYER_SPEED);
          MainGame.me.OnGameStartedPlaying();
          SmartPooler.PausePool<WorldGameObject>();
          PlatformSpecific.SetGameStatus(GameEvents.GameStatus.InGame);
          MixerLightIntegration.OnStartPlayingGame();
          FlowScriptEngine.StartAllBehaviours();
          GJTimer.AddTimer(0.1f, (GJTimer.VoidDelegate) (() => LoadingGUI.ShowBlackBackground(false, true)));
          MainGame.me.save.LateSaveFixer();
          MainGame.me.save.GlobalEventsCheck();
        }));
      }));
    }));
  }

  [CompilerGenerated]
  public void \u003COnSaveSlot\u003Eb__24_0(SaveSlotData slot)
  {
    PlatformSpecific.ReadSaveSlots(new PlatformSpecific.OnCompleteReadSaveSlotsDelegate(this.OnSlotsLoaded));
  }
}
