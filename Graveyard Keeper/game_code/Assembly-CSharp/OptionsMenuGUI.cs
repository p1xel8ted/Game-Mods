// Decompiled with JetBrains decompiler
// Type: OptionsMenuGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using LinqTools;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class OptionsMenuGUI : BaseMenuGUI
{
  public string[] _resolutions = new string[1]
  {
    "1920x1080"
  };
  public string[] _screen_modes = new string[3]
  {
    "Borderless",
    "Fullscreen",
    "Windowed"
  };
  public string[] _cursor_modes = new string[3]
  {
    "Default",
    "Hardware",
    "Software"
  };
  public string[] _mixer_light_modes = new string[2]
  {
    "disabled",
    "enabled"
  };
  public string[] _vsync_mode_modes = new string[2]
  {
    "vsync_off",
    "vsync_on"
  };
  public MenuItemGUI slider_master;
  public MenuItemGUI slider_music;
  public MenuItemGUI slider_sfx;
  public MenuItemGUI language_switcher;
  public MenuItemGUI resolution_switcher;
  public MenuItemGUI screen_mode_switcher;
  public MenuItemGUI cursor_mode_switcher;
  public MenuItemGUI mixer_light_switcher;
  public MenuItemGUI vsync_mode_switcher;
  public MenuItemGUI slider_speech;
  public int _screen_mode_index;
  public int _cursor_mode_index;
  public int _screen_res_index;
  public int _cur_screen_res_index;
  public int _vsync_mode_index;
  public const int REVERT_SETTINGS_TIME = 10;
  public float _revert_time_start;
  public bool _revert_dialog_closed_with_button;
  public GJTimer _revert_timer;

  public override void Open()
  {
    base.Open();
    string[] _temp_languages = new string[GJL.LANGUAGES.Length];
    List<string> list = ((IEnumerable<string>) GJL.AVAILABLE_LOCALES).ToList<string>();
    int current_option_index = 0;
    for (int index1 = 0; index1 < _temp_languages.Length; ++index1)
    {
      string str = GJL.LANGUAGES[index1];
      if (str == GameSettings.me.language)
        current_option_index = index1;
      int index2 = list.IndexOf(str);
      if (index2 == -1)
        Debug.LogError((object) ("Can't find a language name for: " + str));
      else
        str = GJL.AVAILABLE_LOCALE_NAMES[index2];
      _temp_languages[index1] = str;
    }
    GameSettings me = GameSettings.me;
    this.slider_master.SetupSlider(me.volume_master, 0, 100, (Action<int>) (volume =>
    {
      GameSettings.me.volume_master = volume;
      GameSettings.me.ApplyVolume();
    }), 10, 11);
    this.slider_music.SetupSlider(me.volume_music, 0, 100, (Action<int>) (volume =>
    {
      GameSettings.me.volume_music = volume;
      GameSettings.me.ApplyVolume();
    }), 10, 11);
    this.slider_sfx.SetupSlider(me.volume_sfx, 0, 100, (Action<int>) (volume =>
    {
      GameSettings.me.volume_sfx = volume;
      GameSettings.me.ApplyVolume();
    }), 10, 11);
    this.slider_speech.SetupSlider(me.volume_speech, 0, 100, (Action<int>) (volume =>
    {
      GameSettings.me.volume_speech = volume;
      GameSettings.me.ApplyVolume();
    }), 10, 11);
    Debug.Log((object) ("cur_lng_index = " + current_option_index.ToString()));
    this.language_switcher.SetupOptions(current_option_index, _temp_languages.Length - 1, _temp_languages[current_option_index], (Action<int, UILabel>) ((current_index, label) =>
    {
      label.text = _temp_languages[current_index];
      GameSettings.me.language = GJL.LANGUAGES[current_index];
      GameSettings.me.ApplyLanguageChange();
      GameSettings.Save();
    }));
    this._screen_mode_index = me.screen_mode;
    this._cursor_mode_index = (int) me.cursor_mode;
    this._vsync_mode_index = me.vsync_mode;
    if ((UnityEngine.Object) this.resolution_switcher != (UnityEngine.Object) null)
    {
      this._resolutions = ResolutionConfig.GetResolutionsStringArray();
      this._screen_res_index = 0;
      string resolutionName = GameSettings.current_resolution.GetResolutionName();
      Debug.Log((object) $"Current resolution name = {resolutionName}, {GameSettings.current_resolution?.ToString()}");
      int num = -1;
      foreach (string resolution in this._resolutions)
      {
        ++num;
        string str = resolutionName;
        if (resolution == str)
        {
          Debug.Log((object) ("Found resolution index = " + num.ToString()));
          this._screen_res_index = num;
          break;
        }
      }
      this.resolution_switcher.SetupOptions(this._screen_res_index, this._resolutions.Length - 1, this._resolutions[this._screen_res_index], (Action<int, UILabel>) ((index, label) =>
      {
        label.text = this._resolutions[index];
        this._screen_res_index = index;
      }), true);
      this._cur_screen_res_index = this._screen_res_index;
    }
    if ((UnityEngine.Object) this.screen_mode_switcher != (UnityEngine.Object) null)
      this.screen_mode_switcher.SetupOptions(this._screen_mode_index, this._screen_modes.Length - 1, this._screen_modes[this._screen_mode_index], (Action<int, UILabel>) ((index, label) =>
      {
        label.text = GJL.L(this._screen_modes[index]);
        this._screen_mode_index = index;
      }), true);
    if ((UnityEngine.Object) this.vsync_mode_switcher != (UnityEngine.Object) null)
      this.vsync_mode_switcher.SetupOptions(this._vsync_mode_index, this._vsync_mode_modes.Length - 1, this._vsync_mode_modes[this._vsync_mode_index], (Action<int, UILabel>) ((index, label) =>
      {
        label.text = GJL.L(this._vsync_mode_modes[index]);
        this._vsync_mode_index = index;
      }), true);
    if ((UnityEngine.Object) this.cursor_mode_switcher != (UnityEngine.Object) null)
      this.cursor_mode_switcher.SetupOptions(this._cursor_mode_index, this._cursor_modes.Length - 1, this._cursor_modes[this._cursor_mode_index], (Action<int, UILabel>) ((index, label) =>
      {
        label.text = GJL.L(this._cursor_modes[index]);
        this._cursor_mode_index = index;
      }), true);
    if ((UnityEngine.Object) this.mixer_light_switcher != (UnityEngine.Object) null)
      this.mixer_light_switcher.gameObject.SetActive(false);
    if (!BaseGUI.for_gamepad)
      return;
    this.slider_master.OnOver();
  }

  public void OnPressedLog() => Debug.Log((object) "Option menu log");

  public void OnBackBtnClicked() => this.OnClosePressed();

  public override bool OnPressedBack()
  {
    this.OnClosePressed();
    return true;
  }

  public override void OnClosePressed()
  {
    base.OnClosePressed();
    if (GameSettings.me.screen_mode != this._screen_mode_index || GameSettings.me.cursor_mode != (GameSettings.CursorMode) this._cursor_mode_index || this._cur_screen_res_index != this._screen_res_index || GameSettings.me.vsync_mode != this._vsync_mode_index)
    {
      this.ShowVideoOptionsConfirmationWindow((System.Action) (() =>
      {
        if (MainGame.game_started)
          return;
        GUIElements.me.main_menu.Open(false);
      }));
    }
    else
    {
      if (MainGame.game_started)
        return;
      GUIElements.me.main_menu.Open(false);
    }
  }

  public void ShowVideoOptionsConfirmationWindow(System.Action on_closed)
  {
    ResolutionConfig res = ResolutionConfig.GetResolutionByIndex(this._screen_res_index);
    GameSettings.me.ApplyCustomScreenParameters(this._screen_mode_index, (GameSettings.CursorMode) this._cursor_mode_index, res.x, res.y, this._vsync_mode_index);
    this._revert_time_start = Time.time;
    this._revert_dialog_closed_with_button = false;
    System.Action on_dialog_closed = (System.Action) (() =>
    {
      Debug.Log((object) "Video settings confirmation: on_dialog_closed");
      this._revert_dialog_closed_with_button = true;
      if ((UnityEngine.Object) this._revert_timer != (UnityEngine.Object) null)
        this._revert_timer.Stop();
      this._revert_timer = (GJTimer) null;
      on_closed.TryInvoke();
    });
    if (GUIElements.me.ingame_menu.is_shown)
      GUIElements.me.ingame_menu.Hide(false);
    GUIElements.me.dialog.Open(GJL.L("video_changed"), GJL.L("apply"), (GJCommons.VoidDelegate) (() =>
    {
      GameSettings.me.screen_mode = this._screen_mode_index;
      GameSettings.me.cursor_mode = (GameSettings.CursorMode) this._cursor_mode_index;
      GameSettings.me.res_x = res.x;
      GameSettings.me.res_y = res.y;
      GameSettings.me.vsync_mode = this._vsync_mode_index;
      GameSettings.Save();
      SmartAudioEngine.me.SetDullMusicMode(false);
      GUIElements.me.main_menu.ShiftMenuElementsForSmallResolution();
    }), GJL.L("revert"), (GJCommons.VoidDelegate) (() =>
    {
      GameSettings.me.ApplyScreenMode();
      this.Open();
    }), new GJCommons.VoidDelegate(((ExtentionTools) on_dialog_closed).TryInvoke));
    OptionsMenuGUI.UpdateTimerInVideoSettingsDialog(10f);
    this._revert_timer = GJTimer.AddConditionalChecker((GJTimer.BoolDelegate) (() => this._revert_dialog_closed_with_button), (GJTimer.VoidDelegate) (() =>
    {
      double time_left = 10.0 - ((double) Time.time - (double) this._revert_time_start);
      OptionsMenuGUI.UpdateTimerInVideoSettingsDialog((float) time_left);
      if (time_left > 0.0)
        return;
      GameSettings.me.ApplyScreenMode();
      GUIElements.me.dialog.OnClosePressed();
      this.Open();
      on_dialog_closed();
    }), (GJTimer.VoidDelegate) (() => { }));
  }

  public static void UpdateTimerInVideoSettingsDialog(float time_left)
  {
    GUIElements.me.dialog.label_1.text = LocalizedLabel.ColorizeTags(GJL.L("video_changed", $"<{Mathf.CeilToInt(time_left).ToString()}>"), LocalizedLabel.TextColor.Tutorial);
  }
}
