// Decompiled with JetBrains decompiler
// Type: Godrays_BlendModeExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using BlendModes;
using UnityEngine;

#nullable disable
[ComponentExtension.ExtendedComponent(typeof (ParticleSystemRenderer))]
public class Godrays_BlendModeExtension : RendererExtension<ParticleSystemRenderer>
{
  private static ShaderProperty[] cachedDefaultProperties;

  public override string[] GetSupportedShaderFamilies()
  {
    return new string[2]
    {
      "ParticlesAdditive",
      "ParticlesHsbc"
    };
  }

  public override ShaderProperty[] GetDefaultShaderProperties()
  {
    ShaderProperty[] defaultProperties = Godrays_BlendModeExtension.cachedDefaultProperties;
    if (defaultProperties != null)
      return defaultProperties;
    return Godrays_BlendModeExtension.cachedDefaultProperties = new ShaderProperty[7]
    {
      new ShaderProperty("_MainTex", ShaderPropertyType.Texture, (object) Texture2D.whiteTexture),
      new ShaderProperty("_TintColor", ShaderPropertyType.Color, (object) Color.white),
      new ShaderProperty("_InvFade", ShaderPropertyType.Float, (object) 1f),
      new ShaderProperty("_Hue", ShaderPropertyType.Float, (object) 0),
      new ShaderProperty("_Saturation", ShaderPropertyType.Float, (object) 0),
      new ShaderProperty("_Brightness", ShaderPropertyType.Float, (object) 0),
      new ShaderProperty("_Contrast", ShaderPropertyType.Float, (object) 0)
    };
  }

  protected override string GetDefaultShaderName() => "Particles/Standard Unlit";
}
