// Decompiled with JetBrains decompiler
// Type: CameraDepthTexture
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
public class CameraDepthTexture : BaseMonoBehaviour
{
  public void Awake()
  {
    if (SettingsManager.Settings == null || SettingsManager.Settings.Game.PerformanceMode)
      return;
    this.GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
  }
}
