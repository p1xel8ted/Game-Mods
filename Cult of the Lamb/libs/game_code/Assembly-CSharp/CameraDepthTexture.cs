// Decompiled with JetBrains decompiler
// Type: CameraDepthTexture
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
