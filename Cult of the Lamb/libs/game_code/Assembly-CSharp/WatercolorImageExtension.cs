// Decompiled with JetBrains decompiler
// Type: WatercolorImageExtension
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
public class WatercolorImageExtension : UIImageExtension
{
  public override ShaderProperty[] GetDefaultShaderProperties()
  {
    return ((IEnumerable<ShaderProperty>) base.GetDefaultShaderProperties()).Concat<ShaderProperty>((IEnumerable<ShaderProperty>) new ShaderProperty[27]
    {
      new ShaderProperty("_MainTex", ShaderPropertyType.Texture, (object) Texture2D.whiteTexture),
      new ShaderProperty("_Color", ShaderPropertyType.Color, (object) new Color(1f, 1f, 1f, 1f)),
      new ShaderProperty("_StencilComp", ShaderPropertyType.Float, (object) 8),
      new ShaderProperty("_Stencil", ShaderPropertyType.Float, (object) 0),
      new ShaderProperty("_StencilOp", ShaderPropertyType.Float, (object) 0),
      new ShaderProperty("_StencilWriteMask", ShaderPropertyType.Float, (object) (int) byte.MaxValue),
      new ShaderProperty("_StencilReadMask", ShaderPropertyType.Float, (object) (int) byte.MaxValue),
      new ShaderProperty("_ColorMask", ShaderPropertyType.Float, (object) 15),
      new ShaderProperty("_UseUIAlphaClip", ShaderPropertyType.Float, (object) 0),
      new ShaderProperty("_BlotchMultiply", ShaderPropertyType.Float, (object) 4.003983),
      new ShaderProperty("_BlotchSubtract", ShaderPropertyType.Float, (object) 2),
      new ShaderProperty("_Texture1", ShaderPropertyType.Texture, (object) Texture2D.whiteTexture),
      new ShaderProperty("_MovementSpeed", ShaderPropertyType.Float, (object) 2),
      new ShaderProperty("_MovementDirection", ShaderPropertyType.Vector, (object) new Vector4(0.0f, 1f, 0.0f, 0.0f)),
      new ShaderProperty("_CloudDensity", ShaderPropertyType.Float, (object) 1),
      new ShaderProperty("_Texture0", ShaderPropertyType.Texture, (object) Texture2D.whiteTexture),
      new ShaderProperty("_TilingUV", ShaderPropertyType.Vector, (object) new Vector4(1f, 1f, 0.0f, 0.0f)),
      new ShaderProperty("_Rotation", ShaderPropertyType.Float, (object) 0),
      new ShaderProperty("_PosX", ShaderPropertyType.Float, (object) 0),
      new ShaderProperty("_PosY", ShaderPropertyType.Float, (object) 0),
      new ShaderProperty("_PowerExp", ShaderPropertyType.Float, (object) 0),
      new ShaderProperty("_MaskMainTex", ShaderPropertyType.Float, (object) 0),
      new ShaderProperty("_TintCOlor", ShaderPropertyType.Color, (object) new Color(1f, 1f, 1f, 1f)),
      new ShaderProperty("_PositionOffset", ShaderPropertyType.Vector, (object) new Vector4(0.0f, 0.0f, 0.0f, 0.0f)),
      new ShaderProperty("_MultiplyTextureAlpha", ShaderPropertyType.Float, (object) 1),
      new ShaderProperty("_Mask", ShaderPropertyType.Texture, (object) Texture2D.whiteTexture),
      new ShaderProperty("_texcoord", ShaderPropertyType.Texture, (object) Texture2D.whiteTexture)
    }).ToArray<ShaderProperty>();
  }

  public override string[] GetSupportedShaderFamilies()
  {
    return ((IEnumerable<string>) base.GetSupportedShaderFamilies()).Concat<string>((IEnumerable<string>) new string[1]
    {
      nameof (WatercolorImageExtension)
    }).ToArray<string>();
  }
}
