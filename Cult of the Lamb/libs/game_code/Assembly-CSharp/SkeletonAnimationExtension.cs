// Decompiled with JetBrains decompiler
// Type: SkeletonAnimationExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using BlendModes;
using Spine.Unity;
using UnityEngine;

#nullable disable
[ComponentExtension.ExtendedComponent(typeof (SkeletonAnimation))]
public class SkeletonAnimationExtension : ComponentExtension
{
  public override string[] GetSupportedShaderFamilies()
  {
    return new string[1]{ "Spine-Skeleton" };
  }

  public override ShaderProperty[] GetDefaultShaderProperties()
  {
    return new ShaderProperty[3]
    {
      new ShaderProperty("_MainTex", ShaderPropertyType.Texture, (object) Texture2D.whiteTexture),
      new ShaderProperty("_Color", ShaderPropertyType.Color, (object) Color.white),
      new ShaderProperty("_Cutoff", ShaderPropertyType.Float, (object) 0.5f)
    };
  }

  public override Material GetRenderMaterial()
  {
    SkeletonAnimation extendedComponent = this.GetExtendedComponent<SkeletonAnimation>();
    if ((Object) extendedComponent == (Object) null)
      return (Material) null;
    Renderer component = extendedComponent.GetComponent<Renderer>();
    return (Object) component == (Object) null ? (Material) null : component.sharedMaterial;
  }

  public override void SetRenderMaterial(Material material)
  {
    SkeletonAnimation extendedComponent = this.GetExtendedComponent<SkeletonAnimation>();
    if ((Object) extendedComponent == (Object) null)
      return;
    Renderer component = extendedComponent.GetComponent<Renderer>();
    if (!((Object) component != (Object) null))
      return;
    extendedComponent.CustomMaterialOverride.Clear();
    extendedComponent.CustomMaterialOverride[component.sharedMaterial] = material;
  }

  public override void OnEffectDisabled()
  {
    if (!this.IsExtendedComponentValid)
      return;
    SkeletonAnimation extendedComponent = this.GetExtendedComponent<SkeletonAnimation>();
    if (!((Object) extendedComponent != (Object) null))
      return;
    extendedComponent.CustomMaterialOverride.Clear();
    Renderer component = extendedComponent.GetComponent<Renderer>();
    if (!((Object) component != (Object) null))
      return;
    component.sharedMaterial = new Material(Shader.Find("Spine/Skeleton"));
  }
}
