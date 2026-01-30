// Decompiled with JetBrains decompiler
// Type: Godrays_BlendModeExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using BlendModes;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
[ComponentExtension.ExtendedComponent(typeof (ParticleSystemRenderer))]
[Preserve]
public class Godrays_BlendModeExtension : RendererExtension<ParticleSystemRenderer>
{
  public static ShaderProperty[] cachedDefaultProperties;

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

  public override string GetDefaultShaderName() => "Particles/Standard Unlit";
}
