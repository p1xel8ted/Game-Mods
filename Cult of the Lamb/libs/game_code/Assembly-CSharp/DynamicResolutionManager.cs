// Decompiled with JetBrains decompiler
// Type: DynamicResolutionManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DynamicResolutionManager : MonoBehaviour
{
  public float _maxResScale = 1f;
  public static float _minResScale = 1f;
  public static float _targetFPS = 60f;
  public float _intensity = 1f;
  public float _attenuation;
  public static float _fps;
  public static float _previousInterp;
  public static double m_gpuFrameTime;
  public static float _aggressiveness = 5.2f;
  public static bool _active = true;
  public float _previousTime;
  public uint m_frameCount;
  public const uint kNumFrameTimings = 2;
  public double m_cpuFrameTime;
  public FrameTiming[] frameTimings = new FrameTiming[3];
  public const float TIME_BETWEEN_CHECKS = 5f;
  public float timerBetweenChecks = 5f;

  public void Start()
  {
  }

  public void OnDestroy()
  {
  }

  public static void OnPerformanceModeChange(bool state)
  {
    DynamicResolutionManager.UpdateTargetFPS((float) PerformanceModeManager.GetFrameRate());
    QualitySettings.vSyncCount = PerformanceModeManager.GetVSync();
    Application.targetFrameRate = PerformanceModeManager.GetFrameRate();
  }

  public static void UpdateTargetFPS(float target) => DynamicResolutionManager._targetFPS = target;

  public static bool Toggle(bool value)
  {
    int num = DynamicResolutionManager._active == value ? 1 : 0;
    DynamicResolutionManager._active = value;
    ScalableBufferManager.ResizeBuffers(1f, 1f);
    DynamicResolutionManager._previousInterp = 1f;
    return num != 0;
  }

  public void UpdateMaxScale()
  {
  }

  public void Update()
  {
    this.DetermineFPS();
    this.DetermineTimings();
  }

  public bool IsCameraMoving()
  {
    return !((Object) CameraFollowTarget.Instance != (Object) null) || CameraFollowTarget.Instance.IsMoving;
  }

  public void DetermineFPS()
  {
    if (float.IsInfinity(DynamicResolutionManager._fps))
      DynamicResolutionManager._fps = DynamicResolutionManager._targetFPS;
    DynamicResolutionManager._fps = (float) (((double) DynamicResolutionManager._fps + 1.0 / (double) Mathf.Max(Time.realtimeSinceStartup - this._previousTime, 1E-06f)) / 2.0);
    this._previousTime = Time.realtimeSinceStartup;
  }

  public void DetermineTimings()
  {
    ++this.m_frameCount;
    if (this.m_frameCount <= 2U)
      return;
    FrameTimingManager.CaptureFrameTimings();
    int latestTimings = (int) FrameTimingManager.GetLatestTimings(2U, this.frameTimings);
    if (this.frameTimings.Length < 2)
    {
      Debug.LogFormat("Skipping frame {0}, didn't get enough frame timings.", (object) this.m_frameCount);
    }
    else
    {
      DynamicResolutionManager.m_gpuFrameTime = this.frameTimings[0].gpuFrameTime;
      this.m_cpuFrameTime = this.frameTimings[0].cpuFrameTime;
    }
  }
}
