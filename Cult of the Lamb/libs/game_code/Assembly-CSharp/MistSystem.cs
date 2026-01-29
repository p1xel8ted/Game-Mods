// Decompiled with JetBrains decompiler
// Type: MistSystem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMBiomeGeneration;
using UnityEngine;

#nullable disable
public class MistSystem : MonoBehaviour
{
  [SerializeField]
  public bool useWindSettings = true;
  [SerializeField]
  public bool useLightingSettings;
  public ParticleSystem ps;
  public Material mat;
  public Vector2 _windSettings;
  public Color _lightingSettings;

  public void Start()
  {
    if (QualitySettings.GetQualityLevel() == 0)
      this.gameObject.SetActive(false);
    this.ps = this.gameObject.GetComponent<ParticleSystem>();
    this.mat = this.ps.GetComponent<ParticleSystemRenderer>().material;
  }

  public void OnEnable()
  {
    BiomeGenerator.OnBiomeGenerated += new BiomeGenerator.BiomeAction(this.UpdateSettings);
    TimeManager.OnNewPhaseStarted += new System.Action(this.UpdateSettings);
  }

  public void OnDisable()
  {
    BiomeGenerator.OnBiomeGenerated -= new BiomeGenerator.BiomeAction(this.UpdateSettings);
    TimeManager.OnNewPhaseStarted -= new System.Action(this.UpdateSettings);
  }

  public void UpdateSettings()
  {
    if (this.useLightingSettings)
    {
      this._lightingSettings = RenderSettings.ambientLight;
      this.mat.SetColor("_MistColor", this._lightingSettings);
    }
    if (!this.useWindSettings)
      return;
    this._windSettings = (Vector2) Shader.GetGlobalVector("_WindDiection");
    this.ps.main.emitterVelocity = new Vector3(this._windSettings.x, this._windSettings.y, 0.0f);
    this.mat.SetFloat("_NoiseMoveSpeedX", this._windSettings.x * 0.01f);
    this.mat.SetFloat("_NoiseMoveSpeedY", this._windSettings.y * 0.01f);
  }
}
