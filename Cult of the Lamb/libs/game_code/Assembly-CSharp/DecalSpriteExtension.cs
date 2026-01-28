// Decompiled with JetBrains decompiler
// Type: DecalSpriteExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using BlendModes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
[ComponentExtension.ExtendedComponent(typeof (MeshRenderer))]
[Preserve]
public class DecalSpriteExtension : MeshRendererExtension
{
  public override ShaderProperty[] GetDefaultShaderProperties()
  {
    return ((IEnumerable<ShaderProperty>) base.GetDefaultShaderProperties()).Concat<ShaderProperty>((IEnumerable<ShaderProperty>) new ShaderProperty[4]
    {
      new ShaderProperty("_MainTex", ShaderPropertyType.Texture, (object) Texture2D.whiteTexture),
      new ShaderProperty("_Color", ShaderPropertyType.Color, (object) new Color(1f, 1f, 1f, 1f)),
      new ShaderProperty("_ProjectionScale", ShaderPropertyType.Float, (object) 1),
      new ShaderProperty("_DoodleUVOn", ShaderPropertyType.Float, (object) 0)
    }).ToArray<ShaderProperty>();
  }

  public override string[] GetSupportedShaderFamilies()
  {
    return ((IEnumerable<string>) base.GetSupportedShaderFamilies()).Concat<string>((IEnumerable<string>) new string[1]
    {
      "DecalSpriteLighting"
    }).ToArray<string>();
  }
}
