// Decompiled with JetBrains decompiler
// Type: DebugBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DebugBar : MonoBehaviour
{
  private Texture2D blackTexture;
  private int _cameraCount;
  private double _sumFPS;
  private int _sumCount;
  private double _avg;
  private float _min = 999f;
  private float _max;
  public static int CameraCount;

  public float _fps => DynamicResolutionManager._fps;

  public double _previousInterp => (double) DynamicResolutionManager._previousInterp;

  public double m_gpuFrameTime => DynamicResolutionManager.m_gpuFrameTime;
}
