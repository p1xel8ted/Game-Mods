// Decompiled with JetBrains decompiler
// Type: GroundDetails
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using BlendModes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[ComponentExtension.ExtendedComponent(typeof (SpriteRenderer))]
public class GroundDetails : SpriteRendererExtension
{
  public override ShaderProperty[] GetDefaultShaderProperties()
  {
    return ((IEnumerable<ShaderProperty>) base.GetDefaultShaderProperties()).Concat<ShaderProperty>((IEnumerable<ShaderProperty>) new ShaderProperty[10]
    {
      new ShaderProperty("_MainTex", ShaderPropertyType.Texture, (object) Texture2D.whiteTexture),
      new ShaderProperty("AnimatedOffsetUV_X_1", ShaderPropertyType.Float, (object) 1f),
      new ShaderProperty("AnimatedOffsetUV_Y_1", ShaderPropertyType.Float, (object) 1f),
      new ShaderProperty("AnimatedOffsetUV_ZoomX_1", ShaderPropertyType.Float, (object) 1f),
      new ShaderProperty("AnimatedOffsetUV_ZoomY_1", ShaderPropertyType.Float, (object) 1f),
      new ShaderProperty("AnimatedOffsetUV_Speed_1", ShaderPropertyType.Float, (object) 1f),
      new ShaderProperty("_NewTex_1", ShaderPropertyType.Texture, (object) Texture2D.whiteTexture),
      new ShaderProperty("_Threshold_Fade_1", ShaderPropertyType.Float, (object) 1f),
      new ShaderProperty("_MaskRGBA_Fade_1", ShaderPropertyType.Float, (object) 1f),
      new ShaderProperty("SpriteFade", ShaderPropertyType.Float, (object) 1f)
    }).ToArray<ShaderProperty>();
  }

  public override string[] GetSupportedShaderFamilies()
  {
    return ((IEnumerable<string>) base.GetSupportedShaderFamilies()).Concat<string>((IEnumerable<string>) new string[1]
    {
      nameof (GroundDetails)
    }).ToArray<string>();
  }
}
