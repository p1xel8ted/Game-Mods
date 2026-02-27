// Decompiled with JetBrains decompiler
// Type: ParticleDisableLightModulePerPlatform
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ParticleDisableLightModulePerPlatform : MonoBehaviour
{
  public bool disableOnDesktop;
  public bool disableOnConsole;
  public bool disableOnSwitch;
  public bool disableOnLowQuality;
  private ParticleSystem particleSystem;

  private void Start()
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
