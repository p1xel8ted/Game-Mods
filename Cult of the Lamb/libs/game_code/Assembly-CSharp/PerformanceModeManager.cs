// Decompiled with JetBrains decompiler
// Type: PerformanceModeManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using BlendModes;
using System;
using UnityEngine;

#nullable disable
public class PerformanceModeManager
{
  public static PerformanceModeManager _instance;
  public const int PerformanceVSync = 1;
  public const int PerformanceFrameRate = 60;
  public const int DefaultVSync = 2;
  public const int DefaultFrameRate = 30;
  public bool _performanceModeEnabled = true;
  public static Action<bool> OnPerformanceModeChanged;
  public Camera _mainCamera;
  public Material _grabPassMaterial;

  static PerformanceModeManager()
  {
    if (PerformanceModeManager._instance != null)
      return;
    PerformanceModeManager._instance = new PerformanceModeManager();
  }

  public static bool IsPerformanceMode()
  {
    return PerformanceModeManager._instance._performanceModeEnabled;
  }

  public Camera GetMainCamera()
  {
    if ((UnityEngine.Object) this._mainCamera != (UnityEngine.Object) null)
      return this._mainCamera;
    this._mainCamera = Camera.main;
    if ((UnityEngine.Object) this._mainCamera == (UnityEngine.Object) null)
      Debug.LogError((object) "PerformanceModeManager : Unable to cache main Camera");
    return this._mainCamera;
  }

  public static int GetVSync() => PerformanceModeManager._instance._performanceModeEnabled ? 1 : 2;

  public static int GetFrameRate()
  {
    return PerformanceModeManager._instance._performanceModeEnabled ? 60 : 30;
  }

  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
  public static async void Init()
  {
  }

  public static async System.Threading.Tasks.Task WaitForLoadedSettings()
  {
    while (SettingsManager.Settings == null || SettingsManager.Settings.Game == null)
      await System.Threading.Tasks.Task.Yield();
    Debug.Log((object) "PerformanceMode, WaitForLoadedSettings() : Completed");
  }

  public static void PerformanceModeChanged(bool value)
  {
    Debug.Log((object) $"PerformanceMode Enabled : {value}");
    PerformanceModeManager._instance._performanceModeEnabled = value;
    if (!value)
    {
      PerformanceModeManager._instance.OnPerformanceModeDisabled();
      Action<bool> performanceModeChanged = PerformanceModeManager.OnPerformanceModeChanged;
      if (performanceModeChanged == null)
        return;
      performanceModeChanged(value);
    }
    else
    {
      PerformanceModeManager._instance.OnPerformanceModeEnabled();
      Action<bool> performanceModeChanged = PerformanceModeManager.OnPerformanceModeChanged;
      if (performanceModeChanged != null)
        performanceModeChanged(value);
      GraphicsSettingsUtilities.UpdatePostProcessing();
    }
  }

  public void OnPerformanceModeEnabled()
  {
    Application.targetFrameRate = 60;
    QualitySettings.vSyncCount = 1;
    Shader.EnableKeyword("_PERFORMANCE_MODE");
    this.EnableGrabPass(false);
    this.GetMainCamera().depthTextureMode = DepthTextureMode.None;
  }

  public void OnPerformanceModeDisabled()
  {
    Application.targetFrameRate = 30;
    QualitySettings.vSyncCount = 2;
    Shader.DisableKeyword("_PERFORMANCE_MODE");
    this.GetMainCamera().depthTextureMode = DepthTextureMode.Depth;
    this.EnableGrabPass(true);
  }

  public void EnableGrabPass(bool value)
  {
    BlendModeEffect.GrabPassEnabled = value;
    BlendModeEffect[] objectsOfTypeAll = Resources.FindObjectsOfTypeAll<BlendModeEffect>();
    for (int index = 0; index < objectsOfTypeAll.Length; ++index)
    {
      Renderer component;
      if (objectsOfTypeAll[index].TryGetComponent<Renderer>(out component))
      {
        Material sharedMaterial = component.sharedMaterial;
        if (!((UnityEngine.Object) sharedMaterial == (UnityEngine.Object) null) && BlendModeEffect.GrabShaders.ContainsKey(sharedMaterial.name))
          objectsOfTypeAll[index].ApplyGrabShaderChanges();
      }
    }
  }
}
