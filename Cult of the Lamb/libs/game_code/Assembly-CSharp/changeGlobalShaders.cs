// Decompiled with JetBrains decompiler
// Type: changeGlobalShaders
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class changeGlobalShaders : BaseMonoBehaviour
{
  public Color biomeColor;
  public float VerticalFog_ZOffset = 0.25f;
  public float VerticalFog_GradientSpread = 1f;
  public Vector2 windDirection = new Vector2(1f, 0.0f);
  public float windSpeed = 3f;
  public float windDensity = 0.1f;
  public float cloudDensity = 1f;
  public float _GlobalDitherIntensity = 1f;

  public void Start()
  {
  }

  public void applyShaders()
  {
    Shader.SetGlobalFloat("_VerticalFog_ZOffset", this.VerticalFog_ZOffset);
    Shader.SetGlobalFloat("_VerticalFog_GradientSpread", this.VerticalFog_GradientSpread);
    Shader.SetGlobalVector("_WindDiection", (Vector4) this.windDirection);
    Shader.SetGlobalFloat("_WindSpeed", this.windSpeed);
    Shader.SetGlobalFloat("_WindDensity", this.windDensity);
    Shader.SetGlobalFloat("_CloudDensity", this.cloudDensity);
    Shader.SetGlobalFloat("_GlobalDitherIntensity", this._GlobalDitherIntensity);
  }

  public void Update()
  {
  }
}
