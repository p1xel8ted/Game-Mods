// Decompiled with JetBrains decompiler
// Type: DappleLighting_BlendModeExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using BlendModes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
[ComponentExtension.ExtendedComponent(typeof (MeshRenderer))]
[Preserve]
public class DappleLighting_BlendModeExtension : MeshRendererExtension
{
  public override ShaderProperty[] GetDefaultShaderProperties()
  {
    return ((IEnumerable<ShaderProperty>) base.GetDefaultShaderProperties()).Concat<ShaderProperty>((IEnumerable<ShaderProperty>) new ShaderProperty[11]
    {
      new ShaderProperty("_TexA", ShaderPropertyType.Texture, (object) Texture2D.blackTexture),
      new ShaderProperty("_InvertOn", ShaderPropertyType.Float, (object) 0),
      new ShaderProperty("_ShadowColor", ShaderPropertyType.Color, (object) new Color(0.5f, 0.5f, 0.7f, 1f)),
      new ShaderProperty("_LightThreshold", ShaderPropertyType.Float, (object) 0.5),
      new ShaderProperty("_LightClamp", ShaderPropertyType.Float, (object) 0.5),
      new ShaderProperty("_BloomAmnt", ShaderPropertyType.Float, (object) 1),
      new ShaderProperty("_DistortAmnt", ShaderPropertyType.Float, (object) 0.5f),
      new ShaderProperty("_WaveSpeed", ShaderPropertyType.Float, (object) 1),
      new ShaderProperty("_WaveScale", ShaderPropertyType.Float, (object) 1),
      new ShaderProperty("_SrcBlend", ShaderPropertyType.Float, (object) 1),
      new ShaderProperty("_DstBlend", ShaderPropertyType.Float, (object) 0)
    }).ToArray<ShaderProperty>();
  }

  public override string[] GetSupportedShaderFamilies()
  {
    return ((IEnumerable<string>) base.GetSupportedShaderFamilies()).Concat<string>((IEnumerable<string>) new string[2]
    {
      "DappleLighting",
      "DecalSpriteLighting"
    }).ToArray<string>();
  }
}
