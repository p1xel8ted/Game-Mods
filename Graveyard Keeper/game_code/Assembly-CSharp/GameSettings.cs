// Decompiled with JetBrains decompiler
// Type: GameSettings
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class GameSettings
{
  public static GameSettings _me;
  public int volume_master = 100;
  public int volume_music = 100;
  public int volume_sfx = 100;
  public int volume_speech = 100;
  public int screen_mode;
  public GameSettings.CursorMode cursor_mode = GameSettings.CursorMode.Software;
  public string language = "";
  public static string _cur_lng = "";
  public int res_x;
  public int res_y;
  public int vsync_mode;
  public static ResolutionConfig current_resolution = (ResolutionConfig) null;
  public string keys_binding_json = "";
  public bool is_stranger_sins_popup_window_shown;
  public bool is_refugees_popup_window_shown;
  public bool is_souls_popup_window_shown;

  public static GameSettings me
  {
    get
    {
      if (GameSettings._me == null)
      {
        GameSettings._me = PlatformSpecific.LoadGameSettings();
        KeyBindings.FromJSON(GameSettings._me.keys_binding_json);
      }
      return GameSettings._me;
    }
  }

  public static void Init()
  {
    GameSettings.me.ApplyVolume();
    if (!((UnityEngine.Object) MainGame.me.grain_fx_component != (UnityEngine.Object) null))
      return;
    MainGame.me.grain_fx_component.enabled = true;
  }

  public void ApplyVolume()
  {
    SmartAudioEngine.me.SetChannelVolume("master", (float) this.volume_master / 100f);
    SmartAudioEngine.me.SetChannelVolume("music", (float) this.volume_music / 100f);
    SmartAudioEngine.me.SetChannelVolume("sfx", (float) this.volume_sfx / 100f);
    SmartAudioEngine.me.SetChannelVolume("speech", (float) this.volume_speech / 100f, 9f);
    GameSettings.Save();
  }

  public static void Save()
  {
    GameSettings.me.keys_binding_json = KeyBindings.ToJSON();
    PlatformSpecific.SaveGameSettings(GameSettings.me);
  }

  public void ApplyScreenMode()
  {
    Debug.Log((object) "ApplyScreenMode()");
    if (this.res_x == 0 || this.res_y == 0)
    {
      this.res_x = 1920;
      this.res_y = 1080;
    }
    this.ApplyCustomScreenParameters(this.screen_mode, this.cursor_mode, this.res_x, this.res_y, this.vsync_mode);
  }

  public void ApplyCustomScreenParameters(
    int screen_mode,
    GameSettings.CursorMode cursor_mode,
    int res_x,
    int res_y,
    int vsync,
    bool retrying = false)
  {
    Debug.Log((object) $"ApplyCustomScreenParameters, screen_mode = {((GameSettings.ScreenMode) screen_mode).ToString()}{$", cursor = {cursor_mode}, res = {res_x}x{res_y}"}");
    PlatformSpecific.ApplyFullScreenMode((GameSettings.ScreenMode) screen_mode, res_x, res_y, vsync);
    if (cursor_mode == GameSettings.CursorMode.Default)
      PlatformSpecific.SetCursor((Texture2D) null, Vector2.zero);
    else
      PlatformSpecific.SetCursor(Resources.Load<Texture2D>("mouse_cursor"), new Vector2(2f, -2f), cursor_mode == GameSettings.CursorMode.Software);
    if ((UnityEngine.Object) MainGame.me != (UnityEngine.Object) null)
      MainGame.me.OnScreenSizeChanged(res_x, res_y);
    GameSettings.current_resolution = ResolutionConfig.GetResolutionConfigOrNull(res_x, res_y);
    if (GameSettings.current_resolution == null || !GameSettings.current_resolution.IsHardwareSupported())
    {
      if (retrying)
      {
        Debug.LogError((object) "Coudln't apply FullHD... Don't know what to do.");
        GameSettings.current_resolution = new ResolutionConfig(res_x, res_y);
      }
      else
      {
        Debug.LogError((object) $"Couldn't find a suitable configuration for resolution {res_x}x{res_y}, trying FullHD...");
        this.ApplyCustomScreenParameters(screen_mode, cursor_mode, 1920, 1080, vsync, true);
      }
    }
    else
      Debug.Log((object) ("Applied resolution: " + GameSettings.current_resolution?.ToString()));
  }

  public void ApplyLanguageChange()
  {
    if (string.IsNullOrEmpty(this.language))
      this.language = GJL.GetCurrentLocaleCode();
    if (GameSettings._cur_lng == this.language)
      return;
    GameSettings._cur_lng = this.language;
    GJL.LoadLanguageResource(this.language);
    GUIElements.UpdateLanguageChangeForAllBaseGUI();
    foreach (ItemDefinition itemDefinition in GameBalance.me.items_data)
      itemDefinition.ResetLanguageCache();
    foreach (TechDefinition techDefinition in GameBalance.me.techs_data)
      techDefinition.ResetLanguageCache();
    LabelSizeCalculator.ApplyLanguageChange();
  }

  public static string GetCurrentLanguage() => GameSettings._cur_lng;

  public enum CursorMode
  {
    Default,
    Hardware,
    Software,
  }

  public enum ScreenMode
  {
    Borderless,
    FullScreen,
    Windowed,
  }
}
