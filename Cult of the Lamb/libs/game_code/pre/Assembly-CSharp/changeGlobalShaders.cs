// Decompiled with JetBrains decompiler
// Type: changeGlobalShaders
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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

  private void Start()
  {
  }

  private void applyShaders()
  {
    Shader.SetGlobalFloat("_VerticalFog_ZOffset", this.VerticalFog_ZOffset);
    Shader.SetGlobalFloat("_VerticalFog_GradientSpread", this.VerticalFog_GradientSpread);
    Shader.SetGlobalVector("_WindDiection", (Vector4) this.windDirection);
    Shader.SetGlobalFloat("_WindSpeed", this.windSpeed);
    Shader.SetGlobalFloat("_WindDensity", this.windDensity);
    Shader.SetGlobalFloat("_CloudDensity", this.cloudDensity);
    Shader.SetGlobalFloat("_GlobalDitherIntensity", this._GlobalDitherIntensity);
  }

  private void Update()
  {
  }
}
