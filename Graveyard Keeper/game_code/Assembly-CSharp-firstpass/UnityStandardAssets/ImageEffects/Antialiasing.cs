// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.ImageEffects.Antialiasing
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
[AddComponentMenu("Image Effects/Other/Antialiasing")]
public class Antialiasing : PostEffectsBase
{
  public AAMode mode = AAMode.FXAA3Console;
  public bool showGeneratedNormals;
  public float offsetScale = 0.2f;
  public float blurRadius = 18f;
  public float edgeThresholdMin = 0.05f;
  public float edgeThreshold = 0.2f;
  public float edgeSharpness = 4f;
  public bool dlaaSharp;
  public Shader ssaaShader;
  public Material ssaa;
  public Shader dlaaShader;
  public Material dlaa;
  public Shader nfaaShader;
  public Material nfaa;
  public Shader shaderFXAAPreset2;
  public Material materialFXAAPreset2;
  public Shader shaderFXAAPreset3;
  public Material materialFXAAPreset3;
  public Shader shaderFXAAII;
  public Material materialFXAAII;
  public Shader shaderFXAAIII;
  public Material materialFXAAIII;

  public Material CurrentAAMaterial()
  {
    Material material;
    switch (this.mode)
    {
      case AAMode.FXAA2:
        material = this.materialFXAAII;
        break;
      case AAMode.FXAA3Console:
        material = this.materialFXAAIII;
        break;
      case AAMode.FXAA1PresetA:
        material = this.materialFXAAPreset2;
        break;
      case AAMode.FXAA1PresetB:
        material = this.materialFXAAPreset3;
        break;
      case AAMode.NFAA:
        material = this.nfaa;
        break;
      case AAMode.SSAA:
        material = this.ssaa;
        break;
      case AAMode.DLAA:
        material = this.dlaa;
        break;
      default:
        material = (Material) null;
        break;
    }
    return material;
  }

  public override bool CheckResources()
  {
    this.CheckSupport(false);
    this.materialFXAAPreset2 = this.CreateMaterial(this.shaderFXAAPreset2, this.materialFXAAPreset2);
    this.materialFXAAPreset3 = this.CreateMaterial(this.shaderFXAAPreset3, this.materialFXAAPreset3);
    this.materialFXAAII = this.CreateMaterial(this.shaderFXAAII, this.materialFXAAII);
    this.materialFXAAIII = this.CreateMaterial(this.shaderFXAAIII, this.materialFXAAIII);
    this.nfaa = this.CreateMaterial(this.nfaaShader, this.nfaa);
    this.ssaa = this.CreateMaterial(this.ssaaShader, this.ssaa);
    this.dlaa = this.CreateMaterial(this.dlaaShader, this.dlaa);
    if (!this.ssaaShader.isSupported)
    {
      this.NotSupported();
      this.ReportAutoDisable();
    }
    return this.isSupported;
  }

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (!this.CheckResources())
      Graphics.Blit((Texture) source, destination);
    else if (this.mode == AAMode.FXAA3Console && (Object) this.materialFXAAIII != (Object) null)
    {
      this.materialFXAAIII.SetFloat("_EdgeThresholdMin", this.edgeThresholdMin);
      this.materialFXAAIII.SetFloat("_EdgeThreshold", this.edgeThreshold);
      this.materialFXAAIII.SetFloat("_EdgeSharpness", this.edgeSharpness);
      Graphics.Blit((Texture) source, destination, this.materialFXAAIII);
    }
    else if (this.mode == AAMode.FXAA1PresetB && (Object) this.materialFXAAPreset3 != (Object) null)
      Graphics.Blit((Texture) source, destination, this.materialFXAAPreset3);
    else if (this.mode == AAMode.FXAA1PresetA && (Object) this.materialFXAAPreset2 != (Object) null)
    {
      source.anisoLevel = 4;
      Graphics.Blit((Texture) source, destination, this.materialFXAAPreset2);
      source.anisoLevel = 0;
    }
    else if (this.mode == AAMode.FXAA2 && (Object) this.materialFXAAII != (Object) null)
      Graphics.Blit((Texture) source, destination, this.materialFXAAII);
    else if (this.mode == AAMode.SSAA && (Object) this.ssaa != (Object) null)
      Graphics.Blit((Texture) source, destination, this.ssaa);
    else if (this.mode == AAMode.DLAA && (Object) this.dlaa != (Object) null)
    {
      source.anisoLevel = 0;
      RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height);
      Graphics.Blit((Texture) source, temporary, this.dlaa, 0);
      Graphics.Blit((Texture) temporary, destination, this.dlaa, this.dlaaSharp ? 2 : 1);
      RenderTexture.ReleaseTemporary(temporary);
    }
    else if (this.mode == AAMode.NFAA && (Object) this.nfaa != (Object) null)
    {
      source.anisoLevel = 0;
      this.nfaa.SetFloat("_OffsetScale", this.offsetScale);
      this.nfaa.SetFloat("_BlurRadius", this.blurRadius);
      Graphics.Blit((Texture) source, destination, this.nfaa, this.showGeneratedNormals ? 1 : 0);
    }
    else
      Graphics.Blit((Texture) source, destination);
  }
}
