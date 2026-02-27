// Decompiled with JetBrains decompiler
// Type: DynamicResolutionManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DynamicResolutionManager : MonoBehaviour
{
  private float _maxResScale = 1f;
  private static float _minResScale = 1f;
  private static float _targetFPS = 60f;
  private float _intensity = 1f;
  private float _attenuation;
  public static float _fps;
  public static float _previousInterp;
  public static double m_gpuFrameTime;
  private static float _aggressiveness = 5.2f;
  private static bool _active = true;
  private float _previousTime;
  private uint m_frameCount;
  private const uint kNumFrameTimings = 2;
  private double m_cpuFrameTime;
  private FrameTiming[] frameTimings = new FrameTiming[3];

  private void Start()
  {
    DynamicResolutionManager._active = true;
    DynamicResolutionManager._fps = DynamicResolutionManager._targetFPS;
    Object.DontDestroyOnLoad((Object) this.gameObject);
    this._previousTime = Time.realtimeSinceStartup;
  }

  public static void UpdateTargetFPS(float target) => DynamicResolutionManager._targetFPS = target;

  public static void Toggle(bool value)
  {
    DynamicResolutionManager._active = value;
    ScalableBufferManager.ResizeBuffers(1f, 1f);
    DynamicResolutionManager._previousInterp = 1f;
  }

  private void Update()
  {
    if (!DynamicResolutionManager._active || (double) DynamicResolutionManager._minResScale >= 1.0)
      return;
    float f = (float) ((DynamicResolutionManager.m_gpuFrameTime == 0.0 ? (double) DynamicResolutionManager._fps : 1000.0 / DynamicResolutionManager.m_gpuFrameTime) - ((double) DynamicResolutionManager._targetFPS - 0.10000000149011612));
    float num1 = 1f;
    if ((double) f < 0.0)
    {
      this._attenuation += (float) ((double) Time.deltaTime * (double) this._intensity * (double) Mathf.Abs(f) * 0.25);
    }
    else
    {
      num1 = 10f;
      this._attenuation -= Time.deltaTime * this._intensity;
    }
    this._attenuation = Mathf.Clamp01(this._attenuation);
    float num2 = Mathf.MoveTowards(DynamicResolutionManager._previousInterp, Mathf.Lerp(this._maxResScale, DynamicResolutionManager._minResScale, this._attenuation), Time.deltaTime * DynamicResolutionManager._aggressiveness * num1);
    if ((double) DynamicResolutionManager._previousInterp != (double) num2 && (double) num2 > 0.0)
      ScalableBufferManager.ResizeBuffers(num2, num2);
    DynamicResolutionManager._previousInterp = num2;
    this.DetermineFPS();
    this.DetermineTimings();
  }

  private void DetermineFPS()
  {
    if (float.IsInfinity(DynamicResolutionManager._fps))
      DynamicResolutionManager._fps = DynamicResolutionManager._targetFPS;
    DynamicResolutionManager._fps = (float) (((double) DynamicResolutionManager._fps + 1.0 / (double) Mathf.Max(Time.realtimeSinceStartup - this._previousTime, 1E-06f)) / 2.0);
    this._previousTime = Time.realtimeSinceStartup;
  }

  private void DetermineTimings()
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
