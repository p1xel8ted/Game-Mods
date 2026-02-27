// Decompiled with JetBrains decompiler
// Type: ScreenUtilities
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ScreenUtilities
{
  public static Vector2[] GetAvailableResolutions()
  {
    List<Vector2> vector2List = new List<Vector2>();
    foreach (Resolution resolution in Screen.resolutions)
    {
      Vector2 vector2 = new Vector2((float) resolution.width, (float) resolution.height);
      if (!vector2List.Contains(vector2))
        vector2List.Add(vector2);
    }
    return vector2List.ToArray();
  }

  public static int GetDefaultResolution(Vector2[] availableResolutions)
  {
    if (SteamAPI.Init() && SteamUtils.IsSteamRunningOnSteamDeck())
    {
      Vector2 vector2 = new Vector2(1280f, 800f);
      if (availableResolutions.Contains<Vector2>(vector2))
        return availableResolutions.IndexOf<Vector2>(vector2);
    }
    Vector2[] vector2Array = new Vector2[4]
    {
      new Vector2(1920f, 1080f),
      new Vector2(1920f, 1200f),
      new Vector2(1600f, 900f),
      new Vector2(1366f, 768f)
    };
    for (int index = 0; index < vector2Array.Length; ++index)
    {
      if (availableResolutions.Contains<Vector2>(vector2Array[index]))
        return availableResolutions.IndexOf<Vector2>(vector2Array[index]);
    }
    return availableResolutions.Length / 2;
  }

  public static int GetDefaultResolution()
  {
    return ScreenUtilities.GetDefaultResolution(ScreenUtilities.GetAvailableResolutions());
  }

  public static FullScreenMode GetFullScreenMode()
  {
    switch (SettingsManager.Settings.Graphics.FullscreenMode)
    {
      case 0:
        return FullScreenMode.Windowed;
      case 1:
        return FullScreenMode.MaximizedWindow;
      default:
        return FullScreenMode.ExclusiveFullScreen;
    }
  }

  public static void ApplyScreenSettings()
  {
    if (SettingsManager.Settings == null)
      return;
    Vector2[] availableResolutions = ScreenUtilities.GetAvailableResolutions();
    Vector2 resolution;
    if (SettingsManager.Settings.Graphics.Resolution != -1)
    {
      try
      {
        resolution = availableResolutions[SettingsManager.Settings.Graphics.Resolution];
      }
      catch (Exception ex)
      {
        Debug.LogWarning((object) "Something went wrong when trying to read and set the resolution, so we are going to reset to the default values.");
        Debug.LogWarning((object) ex.Message);
        DefaultFallback();
      }
    }
    else
      DefaultFallback();
    Resolution currentResolution = Screen.currentResolution;
    if (currentResolution.width != (int) resolution.x || currentResolution.height != (int) resolution.y || Screen.fullScreenMode != ScreenUtilities.GetFullScreenMode())
      Screen.SetResolution((int) resolution.x, (int) resolution.y, ScreenUtilities.GetFullScreenMode());
    ScreenUtilities.ApplyVSyncSettings();

    void DefaultFallback()
    {
      SettingsManager.Settings.Graphics.Resolution = ScreenUtilities.GetDefaultResolution(availableResolutions);
      resolution = availableResolutions[SettingsManager.Settings.Graphics.Resolution];
    }
  }

  public static void ApplyVSyncSettings()
  {
    QualitySettings.vSyncCount = SettingsManager.Settings.Graphics.VSync.ToInt();
  }
}
