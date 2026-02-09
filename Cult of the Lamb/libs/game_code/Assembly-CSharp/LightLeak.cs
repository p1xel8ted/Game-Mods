// Decompiled with JetBrains decompiler
// Type: LightLeak
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using BlendModes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;

#nullable disable
[ComponentExtension.ExtendedComponent(typeof (Image))]
[Preserve]
public class LightLeak : UIImageExtension
{
  public override ShaderProperty[] GetDefaultShaderProperties()
  {
    return ((IEnumerable<ShaderProperty>) base.GetDefaultShaderProperties()).Concat<ShaderProperty>((IEnumerable<ShaderProperty>) new ShaderProperty[14]
    {
      new ShaderProperty("_SourceRenderTextureTex_17", ShaderPropertyType.Texture, (object) Texture2D.whiteTexture),
      new ShaderProperty("_BlurHQ_Intensity_2", ShaderPropertyType.Float, (object) 1),
      new ShaderProperty("_BlurHQ_Intensity_1", ShaderPropertyType.Float, (object) 1),
      new ShaderProperty("_FillColor_Color_3", ShaderPropertyType.Color, (object) 1),
      new ShaderProperty("_FillColor_Color_2", ShaderPropertyType.Color, (object) 1),
      new ShaderProperty("_FillColor_Color_1", ShaderPropertyType.Color, (object) 1),
      new ShaderProperty("PositionUV_X_1", ShaderPropertyType.Float, (object) 0),
      new ShaderProperty("PositionUV_Y_1", ShaderPropertyType.Float, (object) 0),
      new ShaderProperty("_TurnAlphaToBlack_Fade_1", ShaderPropertyType.Float, (object) 0),
      new ShaderProperty("_TurnAlphaToBlack_Fade_2", ShaderPropertyType.Float, (object) 0),
      new ShaderProperty("_MaskRGBA_Fade_1", ShaderPropertyType.Float, (object) 0),
      new ShaderProperty("_MaskRGBA_Fade_2", ShaderPropertyType.Float, (object) 0),
      new ShaderProperty("_HdrCreate_Value_1", ShaderPropertyType.Float, (object) 0),
      new ShaderProperty("SpriteFade", ShaderPropertyType.Float, (object) 1)
    }).ToArray<ShaderProperty>();
  }

  public override string[] GetSupportedShaderFamilies()
  {
    return ((IEnumerable<string>) base.GetSupportedShaderFamilies()).Concat<string>((IEnumerable<string>) new string[1]
    {
      nameof (LightLeak)
    }).ToArray<string>();
  }
}
