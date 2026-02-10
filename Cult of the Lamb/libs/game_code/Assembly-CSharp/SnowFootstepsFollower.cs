// Decompiled with JetBrains decompiler
// Type: SnowFootstepsFollower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SnowFootstepsFollower : MonoBehaviour
{
  [SerializeField]
  public ParticleSystem _particles;
  public ParticleSystem.EmissionModule emission;
  public float localSnowIntensity;
  public static int SnowIntensity = Shader.PropertyToID("_Snow_Intensity");

  public void Start() => this.emission = this._particles.emission;

  public void Update()
  {
    if (SettingsManager.Settings != null && SettingsManager.Settings.Graphics.EnvironmentDetail == 0)
    {
      this._particles.gameObject.SetActive(false);
    }
    else
    {
      this.localSnowIntensity = Shader.GetGlobalFloat(SnowFootstepsFollower.SnowIntensity);
      if ((double) this.localSnowIntensity == 0.0)
        this._particles.gameObject.SetActive(false);
      else
        this._particles.gameObject.SetActive(true);
      this.emission.rateOverDistance = new ParticleSystem.MinMaxCurve(2.5f * this.localSnowIntensity);
    }
  }
}
