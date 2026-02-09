// Decompiled with JetBrains decompiler
// Type: debugGlobalShaders
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class debugGlobalShaders : BaseMonoBehaviour
{
  public float _GlobalTimeUnscaled;
  public Color _ItemInWoodsColor;
  public Vector2 windDirection;
  public float _WindSpeed;
  public float _WindDensity;
  public Texture _Lighting_RenderTexture;
  public float _VerticalFog_ZOffset;
  public float _VerticalFog_GradientSpread;
  public float _CloudDensity;
  public float _CloudAlpha;
  public Vector3 _PlayerPosition;
  public float _GlobalDither;

  public void Start()
  {
  }

  public void GetGlobalShaders()
  {
    this._GlobalTimeUnscaled = Shader.GetGlobalFloat("_GlobalTimeUnscaled");
    this.windDirection = (Vector2) Shader.GetGlobalVector("_WindDiection");
    this._WindSpeed = Shader.GetGlobalFloat("_WindSpeed");
    this._WindDensity = Shader.GetGlobalFloat("_WindDensity");
    this._Lighting_RenderTexture = Shader.GetGlobalTexture("_Lighting_RenderTexture");
    this._VerticalFog_ZOffset = Shader.GetGlobalFloat("_VerticalFog_ZOffset");
    this._VerticalFog_GradientSpread = Shader.GetGlobalFloat("_VerticalFog_GradientSpread");
    this._ItemInWoodsColor = Shader.GetGlobalColor("_ItemInWoodsColor");
    this._CloudDensity = Shader.GetGlobalFloat("_CloudDensity");
    this._CloudAlpha = Shader.GetGlobalFloat("_CloudAlpha");
    this._PlayerPosition = (Vector3) Shader.GetGlobalVector("_PlayerPosition");
    this._GlobalDither = Shader.GetGlobalFloat("_GlobalDitherIntensity");
  }

  public void Update() => this.GetGlobalShaders();
}
