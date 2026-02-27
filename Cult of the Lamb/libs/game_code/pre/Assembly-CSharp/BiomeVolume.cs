// Decompiled with JetBrains decompiler
// Type: BiomeVolume
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class BiomeVolume
{
  public BiomeVolume.ShaderTypes types;
  public BiomeVolume.ShaderNames _ShaderNames;
  public string shaderName;
  public float valueToGoTo;
  public Color colorToGoTo;
  public Texture textureToGoTo;
  public Vector2 Vector2ToGoTo;

  private void Start()
  {
  }

  public string getName(BiomeVolume.ShaderNames s)
  {
    switch (s)
    {
      case BiomeVolume.ShaderNames.Null:
        return "";
      case BiomeVolume.ShaderNames.UnscaledTime:
        return "_GlobalTimeUnscaled";
      case BiomeVolume.ShaderNames.StartItemsInWoodsColor:
        return "_ItemInWoodsColor";
      case BiomeVolume.ShaderNames.WindDirection:
        return "_WindDiection";
      case BiomeVolume.ShaderNames.WindSpeed:
        return "_WindSpeed";
      case BiomeVolume.ShaderNames.WindDensity:
        return "_WindDensity";
      case BiomeVolume.ShaderNames.LightingRenderTexture:
        return "_Lighting_RenderTexture";
      case BiomeVolume.ShaderNames.Fog_ZOffset:
        return "_VerticalFog_ZOffset";
      case BiomeVolume.ShaderNames.Fog_GradientSpread:
        return "_VerticalFog_GradientSpread";
      case BiomeVolume.ShaderNames.CloudDensity:
        return "_CloudDensity";
      case BiomeVolume.ShaderNames.CloudAlpha:
        return "_CloudAlpha";
      case BiomeVolume.ShaderNames.Dither:
        return "_GlobalDitherIntensity";
      default:
        return "";
    }
  }

  public BiomeVolume(
    BiomeVolume.ShaderTypes types,
    BiomeVolume.ShaderNames shaderName,
    Vector2 valueToGoTo)
  {
    this.types = types;
    this._ShaderNames = shaderName;
    this.shaderName = this.getName(shaderName);
    this.Vector2ToGoTo = valueToGoTo;
  }

  public BiomeVolume(
    BiomeVolume.ShaderTypes types,
    BiomeVolume.ShaderNames shaderName,
    float valueToGoTo)
  {
    this.types = types;
    this._ShaderNames = shaderName;
    this.shaderName = this.getName(shaderName);
    this.valueToGoTo = valueToGoTo;
  }

  public BiomeVolume(
    BiomeVolume.ShaderTypes types,
    BiomeVolume.ShaderNames shaderName,
    Color valueToGoTo)
  {
    this.types = types;
    this._ShaderNames = shaderName;
    this.shaderName = this.getName(shaderName);
    this.colorToGoTo = valueToGoTo;
  }

  public BiomeVolume(
    BiomeVolume.ShaderTypes types,
    BiomeVolume.ShaderNames shaderName,
    Texture valueToGoTo)
  {
    this.types = types;
    this._ShaderNames = shaderName;
    this.shaderName = this.getName(shaderName);
    this.textureToGoTo = valueToGoTo;
  }

  public enum ShaderTypes
  {
    Float,
    Color,
    Texture,
    Vector2,
  }

  public enum ShaderNames
  {
    Null,
    UnscaledTime,
    StartItemsInWoodsColor,
    WindDirection,
    WindSpeed,
    WindDensity,
    LightingRenderTexture,
    Fog_ZOffset,
    Fog_GradientSpread,
    CloudDensity,
    CloudAlpha,
    Dither,
  }
}
