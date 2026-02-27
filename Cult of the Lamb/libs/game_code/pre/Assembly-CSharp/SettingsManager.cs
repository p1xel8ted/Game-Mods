// Decompiled with JetBrains decompiler
// Type: SettingsManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Steamworks;
using System;
using Unify;
using UnityEngine;
using WebSocketSharp;

#nullable disable
public class SettingsManager : Singleton<SettingsManager>
{
  public const string kSettingsFilename = "settings.json";
  private SettingsData _settings;
  private COTLDataReadWriter<SettingsData> _readWriter = new COTLDataReadWriter<SettingsData>();

  public static SettingsData Settings => Singleton<SettingsManager>.Instance._settings;

  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
  public static void LoadUIManager()
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

  private void MakeDefaultFile()
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

  private void Load() => this._readWriter.Read("settings.json");

  public void SaveSettings() => this._readWriter.Write(this._settings, "settings.json", true, true);

  public void ApplySettings()
  {
    Debug.Log((object) "SettingsManager - Apply Settings".Colour(Color.yellow));
    if (this._settings.Game.Language.IsNullOrEmpty())
      this._settings.Game.Language = LanguageUtilities.GetDefaultLanguage();
    if (this._settings.Graphics.Resolution == -1)
    {
      this._settings.Graphics.Resolution = ScreenUtilities.GetDefaultResolution();
      if (SteamAPI.Init() && SteamUtils.IsSteamRunningOnSteamDeck())
      {
        this._settings.Accessibility.TextScale = 1.25f;
        this._settings.Graphics.FullscreenMode = 1;
      }
    }
    LocalizationManager.CurrentLanguage = SettingsManager.Settings.Game.Language;
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
    ControlSettingsUtilities.UpdateGamepadPrompts();
    Application.runInBackground = ScreenUtilities.GetFullScreenMode() != 0;
  }
}
