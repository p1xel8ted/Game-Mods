// Decompiled with JetBrains decompiler
// Type: src.VFXScreenShaker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace src;

public class VFXScreenShaker : MonoBehaviour
{
  [SerializeField]
  public float2 _intensityMinMax = new float2(0.1f, 0.2f);
  [SerializeField]
  public float _duration = 0.5f;

  public void ScreenShake()
  {
    CameraManager.instance.ShakeCameraForDuration(this._intensityMinMax.x, this._intensityMinMax.y, this._duration, false);
  }
}
