// Decompiled with JetBrains decompiler
// Type: PlayerShadowOverlay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using BlendModes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
[ComponentExtension.ExtendedComponent(typeof (Image))]
public class PlayerShadowOverlay : UIImageExtension
{
  public override ShaderProperty[] GetDefaultShaderProperties()
  {
    return ((IEnumerable<ShaderProperty>) base.GetDefaultShaderProperties()).Concat<ShaderProperty>((IEnumerable<ShaderProperty>) new ShaderProperty[12]
    {
      new ShaderProperty("_MainTex", ShaderPropertyType.Texture, (object) Texture2D.whiteTexture),
      new ShaderProperty("_SourceRenderTextureTex_9(RGB)", ShaderPropertyType.Texture, (object) Texture2D.whiteTexture),
      new ShaderProperty("RenderTex_1(RGB)", ShaderPropertyType.Texture, (object) Texture2D.whiteTexture),
      new ShaderProperty("_BlurHQ_Intensity_1", ShaderPropertyType.Float, (object) 1),
      new ShaderProperty("_AlphaIntensity_Fade_1", ShaderPropertyType.Color, (object) 1),
      new ShaderProperty("PositionUV_X_1", ShaderPropertyType.Float, (object) 0),
      new ShaderProperty("_TurnAlphaToBlack_Fade_1", ShaderPropertyType.Float, (object) 0),
      new ShaderProperty("_MaskAlpha_Fade_1", ShaderPropertyType.Float, (object) 0),
      new ShaderProperty("_AlphaIntensity_Fade_2", ShaderPropertyType.Color, (object) 1),
      new ShaderProperty("PositionUV_X_1", ShaderPropertyType.Float, (object) 0),
      new ShaderProperty("PositionUV_Y_1", ShaderPropertyType.Float, (object) 0),
      new ShaderProperty("SpriteFade", ShaderPropertyType.Float, (object) 1)
    }).ToArray<ShaderProperty>();
  }

  public override string[] GetSupportedShaderFamilies()
  {
    return ((IEnumerable<string>) base.GetSupportedShaderFamilies()).Concat<string>((IEnumerable<string>) new string[1]
    {
      nameof (PlayerShadowOverlay)
    }).ToArray<string>();
  }
}
