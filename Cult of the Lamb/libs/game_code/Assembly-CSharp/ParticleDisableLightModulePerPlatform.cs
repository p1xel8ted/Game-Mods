// Decompiled with JetBrains decompiler
// Type: ParticleDisableLightModulePerPlatform
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ParticleDisableLightModulePerPlatform : MonoBehaviour
{
  public bool disableOnDesktop;
  public bool disableOnConsole;
  public bool disableOnSwitch;
  public bool disableOnLowQuality;
  public ParticleSystem particleSystem;

  public void Start()
  {
    if ((Object) this.particleSystem == (Object) null)
      this.particleSystem = this.GetComponent<ParticleSystem>();
    ParticleSystem.LightsModule lights = this.particleSystem.lights;
    if (this.disableOnLowQuality && SettingsManager.Settings.Graphics.EnvironmentDetail == 0)
      lights.enabled = false;
    else if (!this.disableOnDesktop)
    {
      this.gameObject.SetActive(true);
    }
    else
    {
      lights.enabled = false;
      this.gameObject.SetActive(false);
    }
  }
}
