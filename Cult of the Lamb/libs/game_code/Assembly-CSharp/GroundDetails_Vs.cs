// Decompiled with JetBrains decompiler
// Type: GroundDetails_Vs
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using BlendModes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
[ComponentExtension.ExtendedComponent(typeof (SpriteRenderer))]
[Preserve]
public class GroundDetails_Vs : SpriteRendererExtension
{
  public override ShaderProperty[] GetDefaultShaderProperties()
  {
    return ((IEnumerable<ShaderProperty>) base.GetDefaultShaderProperties()).Concat<ShaderProperty>((IEnumerable<ShaderProperty>) new ShaderProperty[15]
    {
      new ShaderProperty("_MainTex", ShaderPropertyType.Texture, (object) Texture2D.whiteTexture),
      new ShaderProperty("AnimatedOffsetUV_X_1", ShaderPropertyType.Float, (object) 1f),
      new ShaderProperty("AnimatedOffsetUV_Y_1", ShaderPropertyType.Float, (object) 1f),
      new ShaderProperty("AnimatedOffsetUV_ZoomX_1", ShaderPropertyType.Float, (object) 1f),
      new ShaderProperty("AnimatedOffsetUV_ZoomY_1", ShaderPropertyType.Float, (object) 1f),
      new ShaderProperty("AnimatedOffsetUV_Speed_1", ShaderPropertyType.Float, (object) 1f),
      new ShaderProperty("_SourceNewTex_1", ShaderPropertyType.Texture, (object) Texture2D.whiteTexture),
      new ShaderProperty("_InnerGlowHQ_Intensity_1", ShaderPropertyType.Float, (object) 1f),
      new ShaderProperty("_InnerGlowHQ_Size_1", ShaderPropertyType.Float, (object) 1f),
      new ShaderProperty("_InnerGlowHQ_Color_1", ShaderPropertyType.Color, (object) (1, 1, 0, 1)),
      new ShaderProperty("_ColorHSV_Hue_1", ShaderPropertyType.Float, (object) 180f),
      new ShaderProperty("_ColorHSV_Saturation_1", ShaderPropertyType.Float, (object) 1f),
      new ShaderProperty("_ColorHSV_Brightness_1", ShaderPropertyType.Float, (object) 1f),
      new ShaderProperty("_Add_Fade_1", ShaderPropertyType.Float, (object) 1f),
      new ShaderProperty("SpriteFade", ShaderPropertyType.Float, (object) 1f)
    }).ToArray<ShaderProperty>();
  }

  public override string[] GetSupportedShaderFamilies()
  {
    return ((IEnumerable<string>) base.GetSupportedShaderFamilies()).Concat<string>((IEnumerable<string>) new string[1]
    {
      nameof (GroundDetails_Vs)
    }).ToArray<string>();
  }
}
