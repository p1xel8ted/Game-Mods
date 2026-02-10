// Decompiled with JetBrains decompiler
// Type: ScreenUtilities
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Steamworks;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

#nullable disable
public class ScreenUtilities
{
  public static Vector2Int[] GetAvailableResolutions()
  {
    List<Vector2Int> vector2IntList = new List<Vector2Int>();
    foreach (Resolution resolution in Screen.resolutions)
    {
      Vector2Int vector2Int = new Vector2Int(resolution.width, resolution.height);
      if (!vector2IntList.Contains(vector2Int))
        vector2IntList.Add(vector2Int);
    }
    return vector2IntList.ToArray();
  }

  public static int GetDefaultResolution(Vector2Int[] availableResolutions)
  {
    if (SteamAPI.Init() && SteamUtils.IsSteamRunningOnSteamDeck())
    {
      Vector2Int vector2Int = new Vector2Int(1280 /*0x0500*/, 800);
      if (availableResolutions.Contains<Vector2Int>(vector2Int))
        return availableResolutions.IndexOf<Vector2Int>(vector2Int);
    }
    Vector2Int[] vector2IntArray = new Vector2Int[4]
    {
      new Vector2Int(1920, 1080),
      new Vector2Int(1920, 1200),
      new Vector2Int(1600, 900),
      new Vector2Int(1366, 768 /*0x0300*/)
    };
    for (int index = 0; index < vector2IntArray.Length; ++index)
    {
      if (availableResolutions.Contains<Vector2Int>(vector2IntArray[index]))
        return availableResolutions.IndexOf<Vector2Int>(vector2IntArray[index]);
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
    // ISSUE: variable of a compiler-generated type
    ScreenUtilities.\u003C\u003Ec__DisplayClass4_0 cDisplayClass40;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass40.availableResolutions = ScreenUtilities.GetAvailableResolutions();
    if (SettingsManager.Settings.Graphics.Resolution != -1)
    {
      try
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cDisplayClass40.resolution = cDisplayClass40.availableResolutions[SettingsManager.Settings.Graphics.Resolution];
      }
      catch (Exception ex)
      {
        Debug.LogWarning((object) "Something went wrong when trying to read and set the resolution, so we are going to reset to the default values.");
        Debug.LogWarning((object) ex.Message);
        ScreenUtilities.\u003CApplyScreenSettings\u003Eg__DefaultFallback\u007C4_0(ref cDisplayClass40);
      }
    }
    else
      ScreenUtilities.\u003CApplyScreenSettings\u003Eg__DefaultFallback\u007C4_0(ref cDisplayClass40);
    Vector2Int vector2Int = new Vector2Int(Screen.width, Screen.height);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    if (vector2Int.x != cDisplayClass40.resolution.x || vector2Int.y != cDisplayClass40.resolution.y || Screen.fullScreenMode != ScreenUtilities.GetFullScreenMode())
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Screen.SetResolution(cDisplayClass40.resolution.x, cDisplayClass40.resolution.y, ScreenUtilities.GetFullScreenMode());
    }
    ScreenUtilities.ApplyVSyncSettings();
  }

  public static void ApplyVSyncSettings()
  {
    QualitySettings.vSyncCount = SettingsManager.Settings.Graphics.VSync.ToInt();
  }

  [CompilerGenerated]
  public static void \u003CApplyScreenSettings\u003Eg__DefaultFallback\u007C4_0(
    [In] ref ScreenUtilities.\u003C\u003Ec__DisplayClass4_0 obj0)
  {
    // ISSUE: reference to a compiler-generated field
    SettingsManager.Settings.Graphics.Resolution = ScreenUtilities.GetDefaultResolution(obj0.availableResolutions);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    obj0.resolution = obj0.availableResolutions[SettingsManager.Settings.Graphics.Resolution];
  }
}
