// Decompiled with JetBrains decompiler
// Type: SettingsManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Steamworks;
using System;
using System.Runtime.CompilerServices;
using Unify;
using UnityEngine;
using WebSocketSharp;

#nullable disable
public class SettingsManager : Singleton<SettingsManager>
{
  public const string kSettingsFilename = "settings.json";
  public SettingsData _settings;
  public COTLDataReadWriter<SettingsData> _readWriter = new COTLDataReadWriter<SettingsData>();

  public static SettingsData Settings => Singleton<SettingsManager>.Instance._settings;

  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
  public static void LoadSettingsManager()
  {
    if (UnifyManager.platform != UnifyManager.Platform.Standalone && UnifyManager.platform != UnifyManager.Platform.None)
      return;
    Singleton<SettingsManager>.Instance.LoadAndApply();
  }

  public SettingsManager()
  {
    COTLDataReadWriter<SettingsData> readWriter1 = this._readWriter;
    readWriter1.OnReadCompleted = readWriter1.OnReadCompleted + (Action<SettingsData>) (data => this._settings = data);
    COTLDataReadWriter<SettingsData> readWriter2 = this._readWriter;
    readWriter2.OnCreateDefault = readWriter2.OnCreateDefault + (System.Action) (() => this.MakeDefaultFile());
    COTLDataReadWriter<SettingsData> readWriter3 = this._readWriter;
    readWriter3.OnReadError = readWriter3.OnReadError + (Action<MMReadWriteError>) (error => this.MakeDefaultFile());
  }

  public void MakeDefaultFile()
  {
    this._settings = new SettingsData();
    this._readWriter.Write(this._settings, "settings.json", true, true);
  }

  public void LoadAndApply(bool forceReload = false)
  {
    if (this._settings != null && !forceReload)
      return;
    this.Load();
    this.ApplySettings();
  }

  public void Load() => this._readWriter.Read("settings.json");

  public void SaveSettings() => this._readWriter.Write(this._settings, "settings.json", true, true);

  public void ApplySettings()
  {
    Debug.Log((object) "SettingsManager - Apply Settings".Colour(Color.yellow));
    if (this._settings.Game.Language.IsNullOrEmpty())
      this._settings.Game.Language = LanguageUtilities.GetDefaultLanguage();
    LocalizationManager.CurrentLanguage = SettingsManager.Settings.Game.Language;
    LocalizationManager.EnableChangingCultureInfo(true);
    LocalizationManager.SetupFonts();
    if (this._settings.Graphics.Resolution == -1)
    {
      this._settings.Graphics.Resolution = ScreenUtilities.GetDefaultResolution();
      if (SteamAPI.Init() && SteamUtils.IsSteamRunningOnSteamDeck())
      {
        this._settings.Accessibility.TextScale = 1.25f;
        this._settings.Graphics.FullscreenMode = 1;
      }
    }
    if ((UnityEngine.Object) CameraFollowTarget.Instance != (UnityEngine.Object) null)
      CameraFollowTarget.Instance.CamWobbleSettings = (float) (1 - SettingsManager.Settings.Accessibility.ReduceCameraMotion.ToInt());
    ScreenUtilities.ApplyScreenSettings();
    GraphicsSettingsUtilities.SetTargetFramerate(SettingsManager.Settings.Graphics.TargetFrameRate);
    GraphicsSettingsUtilities.UpdatePostProcessing();
    GraphicsSettingsUtilities.SetLightingQuality(SettingsManager.Settings.Graphics.LightingQuality);
    GraphicsSettingsUtilities.UpdateShadows(SettingsManager.Settings.Graphics.Shadows);
    AudioManager.Instance.SetMasterBusVolume(SettingsManager.Settings.Audio.MasterVolume);
    AudioManager.Instance.SetMusicBusVolume(SettingsManager.Settings.Audio.MusicVolume);
    AudioManager.Instance.SetSFXBusVolume(SettingsManager.Settings.Audio.SFXVolume);
    AudioManager.Instance.SetVOBusVolume(SettingsManager.Settings.Audio.VOVolume);
    MMVibrate.SetHapticsIntensity(SettingsManager.Settings.Game.RumbleIntensity);
    AccessibilityManager.UpdateTextStyling();
    Singleton<AccessibilityManager>.Instance.DispatchAll();
    AccessibilityManager.UpdateDitheringFadeDistance(SettingsManager.Settings.Accessibility.DitherFadeDistance);
    Singleton<AccessibilityManager>.Instance.SetHighContrastText(SettingsManager.Settings.Accessibility.HighContrastText);
    LocalizationManager.LocalizeAll(true);
    ControlSettingsUtilities.UpdateGamepadPrompts();
    Application.runInBackground = ScreenUtilities.GetFullScreenMode() != 0;
  }

  [CompilerGenerated]
  public void \u003C\u002Ector\u003Eb__6_0(SettingsData data) => this._settings = data;

  [CompilerGenerated]
  public void \u003C\u002Ector\u003Eb__6_1() => this.MakeDefaultFile();

  [CompilerGenerated]
  public void \u003C\u002Ector\u003Eb__6_2(MMReadWriteError error) => this.MakeDefaultFile();
}
