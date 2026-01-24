// Decompiled with JetBrains decompiler
// Type: CameraParticles
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class CameraParticles : MonoBehaviour
{
  public float currentCameraParticles = 1f;

  public void Start()
  {
    if (SettingsManager.Settings == null)
      return;
    Singleton<AccessibilityManager>.Instance.OnCameraParticlesChanged += new Action<float>(this.OnCameraParticlesChanged);
    if (SettingsManager.Settings == null || SettingsManager.Settings.Accessibility == null)
      return;
    this.currentCameraParticles = SettingsManager.Settings.Accessibility.CameraParticles;
    this.UpdateObjectState();
  }

  public void OnCameraParticlesChanged(float value)
  {
    this.currentCameraParticles = value;
    this.UpdateObjectState();
  }

  public void UpdateObjectState()
  {
    this.gameObject.SetActive((double) this.currentCameraParticles > 0.0);
  }
}
