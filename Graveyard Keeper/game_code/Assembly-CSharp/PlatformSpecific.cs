// Decompiled with JetBrains decompiler
// Type: PlatformSpecific
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Steamworks;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using UnityEngine;

#nullable disable
public static class PlatformSpecific
{
  public const PlatformSpecific.Platform PLATFORM = PlatformSpecific.Platform.PС;
  public const bool GAMEPAD_SUPPORTED = true;
  public const bool HAS_EXIT_BUTTON_IN_MAIN_MENU = true;
  public const bool HAS_PROFILES_BUTTON_IN_MAIN_MENU = false;
  public const bool ALLOW_GAMEPAD_COMBINATION_FOR_FPS_COUNTER = true;
  public const float DEFAULT_FIXED_DELTA_TIME = 0.0166666675f;
  public const bool EMULATE_CONSOLE_SAFE_ZONES = false;
  public const bool FX_NOISE_AND_GRAIN = true;
  public const int MAX_SHADOWS_CREATED_PER_FRAME = 4;
  public const int MAX_SHADOWS_PER_OBJECT = 4;
  public const bool PRELOAD_WOPS_ASYNC = true;
  public const bool PRELOAD_VISUAL_SCRIPTS_ASYNC = true;
  public const int SIMULTANEOUS_ASYNC_RESOURCE_LOADING_THREADS = 4;
  public const bool HALF_RESOLUTION_MODE = false;
  public static List<SaveSlotData> _slots = new List<SaveSlotData>();
  public static GameEvents.GameStatus _cur_status = GameEvents.GameStatus.Undefined;
  public static FullScreenMode _full_screen_mode = FullScreenMode.FullScreenWindow;

  public static void ReadSaveSlots(
    PlatformSpecific.OnCompleteReadSaveSlotsDelegate on_complete)
  {
    PlatformSpecific.ShowDiskAccessIndicator(PlatformSpecific.DiskOperationType.Load, true);
    PlatformSpecific._slots = new List<SaveSlotData>();
    foreach (string file in Directory.GetFiles(PlatformSpecific.GetSaveFolder(), "*.info", SearchOption.TopDirectoryOnly))
    {
      try
      {
        SaveSlotData saveSlotData = SaveSlotData.FromJSON(File.ReadAllText(file));
        if (saveSlotData != null)
        {
          saveSlotData.filename_no_extension = Path.GetFileNameWithoutExtension(file);
          PlatformSpecific._slots.Add(saveSlotData);
        }
      }
      catch (Exception ex)
      {
        Debug.LogError((object) $"Error reading savegame information, file: {file}\n{ex?.ToString()}");
      }
    }
    PlatformSpecific.ShowDiskAccessIndicator(PlatformSpecific.DiskOperationType.Load, false);
    on_complete(PlatformSpecific._slots);
  }

  public static void LoadGame(SaveSlotData slot, PlatformSpecific.OnGameLoadedDelegate on_lodaded)
  {
    PlatformSpecific.ShowDiskAccessIndicator(PlatformSpecific.DiskOperationType.Load, true);
    Debug.Log((object) ("Load game, slot = " + slot?.ToString()));
    PlatformSpecific.DelayByOneFrame((System.Action) (() =>
    {
      string path = $"{PlatformSpecific.GetSaveFolder()}{slot.filename_no_extension}.dat";
      if (!File.Exists(path))
      {
        Debug.LogError((object) ("Save file not found: " + path));
        PlatformSpecific.ShowDiskAccessIndicator(PlatformSpecific.DiskOperationType.Load, false);
        on_lodaded((GameSave) null);
      }
      else
      {
        GameSave save_data;
        try
        {
          save_data = !slot.IsBinaryFormat() ? GameSave.FromJSON(File.ReadAllText(path)) : GameSave.FromBinary(File.ReadAllBytes(path));
        }
        catch (Exception ex)
        {
          Debug.LogError((object) $"Error reading save file {path}\n{ex?.ToString()}");
          PlatformSpecific.ShowDiskAccessIndicator(PlatformSpecific.DiskOperationType.Load, false);
          LoadingGUI.HideImmediate();
          LoadingGUI.ShowBlackBackground(false);
          GUIElements.me.dialog.OpenOK($"{GJL.L("LoadErrorPrompt header")}\n\n{GJL.L("LoadErrorPrompt body")}", (GJCommons.VoidDelegate) (() => GUIElements.me.main_menu.Open(true)));
          return;
        }
        slot.linked_save = save_data;
        MainGame.me.save_slot = slot;
        PlatformSpecific.ShowDiskAccessIndicator(PlatformSpecific.DiskOperationType.Load, false);
        on_lodaded(save_data);
      }
    }));
  }

  public static void DeleteSlot(SaveSlotData slot, PlatformSpecific.OnCompleteDelegate on_complete)
  {
    Debug.Log((object) ("DeleteSlot, slot = " + slot?.ToString()));
    string str = PlatformSpecific.GetSaveFolder() + slot.filename_no_extension;
    File.Delete(str + ".dat");
    File.Delete(str + ".info");
    on_complete();
  }

  public static void SaveGame(
    SaveSlotData slot,
    GameSave save,
    PlatformSpecific.OnSaveCompleteDelegate on_complete)
  {
    Debug.Log((object) $"SaveGame, slot = {slot?.ToString()}, filename = {(slot == null ? "null" : slot.filename_no_extension)}");
    if (slot != null)
    {
      PlatformSpecific.ShowDiskAccessIndicator(PlatformSpecific.DiskOperationType.Save, true);
      slot.PrepareForSave();
    }
    PlatformSpecific.DelayByOneFrame((System.Action) (() =>
    {
      if (slot == null)
      {
        slot = new SaveSlotData()
        {
          filename_no_extension = PlatformSpecific.GetNewSlotFilename(),
          linked_save = save
        };
        Debug.Log((object) ("Created a new save filename = " + slot.filename_no_extension));
      }
      PlatformSpecific.SaveSlotInfoAndData(slot, save, (PlatformSpecific.OnSaveCompleteDelegate) (s =>
      {
        PlatformSpecific.ShowDiskAccessIndicator(PlatformSpecific.DiskOperationType.Save, false);
        on_complete(s);
      }));
    }));
  }

  public static void ShowDiskAccessIndicator(PlatformSpecific.DiskOperationType type, bool show)
  {
    if (type != PlatformSpecific.DiskOperationType.Save)
      return;
    GUIElements.me.ShowSavingStatus(show);
  }

  public static string GetSaveFolder() => Application.persistentDataPath + "/";

  public static void Init()
  {
    GameKeyTip.SetPlatformPrefix("");
    Debug.Log((object) ("SteamManager.Initialized = " + SteamManager.Initialized.ToString()));
  }

  public static string GetNewSlotFilename()
  {
    int num = 0;
    while (num++ <= 1000)
    {
      string newSlotFilename = num.ToString();
      bool flag = false;
      foreach (SaveSlotData slot in PlatformSpecific._slots)
      {
        if (slot.filename_no_extension == newSlotFilename)
          flag = true;
      }
      if (!flag)
        return newSlotFilename;
    }
    Debug.LogError((object) "GetNewSlotFilename: too many iterations");
    return (string) null;
  }

  public static void SaveSlotInfoAndData(
    SaveSlotData slot,
    GameSave save,
    PlatformSpecific.OnSaveCompleteDelegate on_complete)
  {
    if (slot == null)
    {
      Debug.LogError((object) "SaveSlotData: Can't save to a null slot");
      on_complete((SaveSlotData) null);
    }
    else
    {
      File.WriteAllText($"{PlatformSpecific.GetSaveFolder()}{slot.filename_no_extension}.info", slot.ToJSON());
      PlatformSpecific.SaveGameDataToSlot(slot, save, (PlatformSpecific.OnCompleteDelegate) (() => on_complete(slot)));
    }
  }

  public static void SaveGameDataToSlot(
    SaveSlotData slot,
    GameSave save,
    PlatformSpecific.OnCompleteDelegate on_complete)
  {
    if (slot == null)
    {
      Debug.LogError((object) "SaveGameDataToSlot: Can't save to a null slot");
      on_complete();
    }
    else if (save == null)
    {
      Debug.LogError((object) "SaveGameDataToSlot: Can't save a null game data");
      on_complete();
    }
    else
    {
      save.PrepareForSave();
      string str = $"{PlatformSpecific.GetSaveFolder()}{slot.filename_no_extension}.dat";
      GC.Collect();
      Resources.UnloadUnusedAssets();
      Debug.Log((object) "Serializing save...");
      byte[] binary = save.ToBinary();
      Debug.Log((object) ("Serialized length: " + binary.Length.ToString()));
      try
      {
        File.WriteAllBytes(str + ".new", binary);
        if (File.Exists(str + ".backup.2"))
          File.Delete(str + ".backup.2");
        if (File.Exists(str + ".backup.1"))
          File.Move(str + ".backup.1", str + ".backup.2");
        if (File.Exists(str))
          File.Move(str, str + ".backup.1");
        File.Move(str + ".new", str);
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ("Error saving file: " + ex?.ToString()));
        GUIElements.me.dialog.OpenOK("Error saving file!\n\n" + ex?.ToString());
      }
      on_complete();
    }
  }

  public static void OnProfileSelect()
  {
  }

  public static void SaveGameSettings(GameSettings data)
  {
    PlayerPrefs.SetString("settings", JsonUtility.ToJson((object) data));
  }

  public static GameSettings LoadGameSettings()
  {
    Debug.Log((object) nameof (LoadGameSettings));
    string json = "";
    if (PlayerPrefs.HasKey("settings"))
      json = PlayerPrefs.GetString("settings");
    GameSettings gameSettings = string.IsNullOrEmpty(json) ? new GameSettings() : JsonUtility.FromJson<GameSettings>(json);
    if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && (Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.RightAlt)) && (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)))
    {
      Debug.Log((object) "Resetting video settings...");
      gameSettings.screen_mode = 0;
      gameSettings.res_x = 1920;
      gameSettings.res_y = 1080;
      gameSettings.cursor_mode = GameSettings.CursorMode.Software;
    }
    return gameSettings;
  }

  public static string GetInteractionButtonHint(bool for_gamepad)
  {
    return !for_gamepad ? "(E)" : PlatformSpecific.GetGamepadButtonSymbol(GamePadButton.A, "");
  }

  public static void DelayByOneFrame(System.Action action)
  {
    if (action == null)
      return;
    GJTimer.AddTimer(1f / 1000f, (GJTimer.VoidDelegate) (() => action()));
  }

  public static void SetCursor(Texture2D tx, Vector2 pos, bool force_software = false)
  {
    Cursor.SetCursor(tx, pos, force_software ? UnityEngine.CursorMode.ForceSoftware : UnityEngine.CursorMode.Auto);
  }

  public static void FixResolutionAfterStart()
  {
  }

  public static void FixScreenModeAfterStart()
  {
  }

  public static void ApplyFullScreenMode(
    GameSettings.ScreenMode mode,
    int scr_w,
    int scr_h,
    int vsync)
  {
    switch (mode)
    {
      case GameSettings.ScreenMode.Borderless:
        PlatformSpecific._full_screen_mode = FullScreenMode.FullScreenWindow;
        break;
      case GameSettings.ScreenMode.FullScreen:
        PlatformSpecific._full_screen_mode = FullScreenMode.ExclusiveFullScreen;
        break;
      case GameSettings.ScreenMode.Windowed:
        PlatformSpecific._full_screen_mode = FullScreenMode.Windowed;
        break;
      default:
        throw new Exception("Unsupported screen mode: " + mode.ToString());
    }
    Debug.Log((object) $"ApplyFullScreenMode {scr_w}x{scr_h}, {PlatformSpecific._full_screen_mode}, vsync = {vsync}");
    int preferred_refresh_rate = 0;
    if (PlatformSpecific._full_screen_mode != FullScreenMode.ExclusiveFullScreen || vsync == 0)
      preferred_refresh_rate = 60;
    if (false)
      return;
    Debug.Log((object) "SetResolution #1");
    Screen.SetResolution(scr_w, scr_h, PlatformSpecific._full_screen_mode, preferred_refresh_rate);
    ResolutionHelper.OnResolutionChanged(scr_w, scr_h);
    GJTimer.AddTimer(0.0f, (GJTimer.VoidDelegate) (() =>
    {
      Debug.Log((object) "SetResolution #2");
      Screen.SetResolution(scr_w, scr_h, PlatformSpecific._full_screen_mode, preferred_refresh_rate);
    }));
    if (vsync != 0)
    {
      if (vsync == 1)
      {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = -1;
      }
      else
        Debug.LogError((object) $"Unsupported vsync mode = {vsync}");
    }
    else
    {
      QualitySettings.vSyncCount = 0;
      Application.targetFrameRate = 60;
    }
  }

  public static void ApplyScreenSafeZones()
  {
  }

  public static string GetGamepadButtonSymbol(GamePadButton b, string suffix = " ")
  {
    switch (b)
    {
      case GamePadButton.None:
        return string.Empty;
      case GamePadButton.B:
        return $"({GameKeyTip.GetPlatformPrefix()}B){suffix}";
      case GamePadButton.A:
        return $"({GameKeyTip.GetPlatformPrefix()}A){suffix}";
      case GamePadButton.X:
        return $"({GameKeyTip.GetPlatformPrefix()}X){suffix}";
      case GamePadButton.Y:
        return $"({GameKeyTip.GetPlatformPrefix()}Y){suffix}";
      default:
        Debug.LogWarning((object) ("No symbol for a gamepad button = " + b.ToString()));
        return string.Empty;
    }
  }

  public static void SetGameStatus(GameEvents.GameStatus status)
  {
    if (PlatformSpecific._cur_status == status)
      return;
    PlatformSpecific._cur_status = status;
    Debug.Log((object) ("SetGameStatus: " + status.ToString()));
  }

  public static void SetGameMetric(GameEvents.GameMetric metric, float value)
  {
    Debug.Log((object) $"SetGameMetric {metric.ToString()} = {value.ToString()}");
  }

  public static void OnAchievementComplete(AchievementDefinition ach)
  {
    Debug.Log((object) $"OnAchievementComplete: <color=green>{ach.id}</color>");
    if (!SteamManager.Initialized)
    {
      Debug.LogWarning((object) "No steam - no achievement");
    }
    else
    {
      SteamUserStats.SetAchievement(ach.id);
      SteamUserStats.StoreStats();
      SteamAPI.RunCallbacks();
    }
  }

  public static void SetDefaultCultureInfo()
  {
    Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
    CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
  }

  public static void OpenStoreLink(DLCEngine.DLCVersion version)
  {
    string str = string.Empty;
    switch (version)
    {
      case DLCEngine.DLCVersion.Stories:
        str = "https://store.steampowered.com/app/1163770/Graveyard_Keeper__Stranger_Sins/";
        break;
      case DLCEngine.DLCVersion.Refugees:
        str = "https://store.steampowered.com/app/1430990/Graveyard_Keeper__Game_Of_Crone/";
        break;
      case DLCEngine.DLCVersion.Souls:
        str = "https://store.steampowered.com/app/1788370/Graveyard_Keeper__Better_Save_Soul/";
        break;
    }
    if (SteamManager.Initialized)
    {
      Debug.Log((object) "SteamManager is initialized");
      SteamFriends.ActivateGameOverlayToWebPage(str);
    }
    else
    {
      Debug.Log((object) "SteamManager isn't initialized");
      Application.OpenURL(str);
    }
  }

  public enum Platform
  {
    PС,
    XBox,
    PS4,
    Switch,
  }

  public delegate void OnCompleteReadSaveSlotsDelegate(List<SaveSlotData> slots);

  public delegate void OnSaveCompleteDelegate(SaveSlotData slot);

  public delegate void OnGameLoadedDelegate(GameSave save_data);

  public delegate void OnCompleteDelegate();

  public enum DiskOperationType
  {
    Other,
    Save,
    Load,
  }
}
