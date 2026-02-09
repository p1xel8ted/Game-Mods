// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.ImageEffects.ContrastEnhance
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
[AddComponentMenu("Image Effects/Color Adjustments/Contrast Enhance (Unsharp Mask)")]
public class ContrastEnhance : PostEffectsBase
{
  [Range(0.0f, 1f)]
  public float intensity = 0.5f;
  [Range(0.0f, 0.999f)]
  public float threshold;
  public Material separableBlurMaterial;
  public Material contrastCompositeMaterial;
  [Range(0.0f, 1f)]
  public float blurSpread = 1f;
  public Shader separableBlurShader;
  public Shader contrastCompositeShader;

  public override bool CheckResources()
  {
    this.CheckSupport(false);
    this.contrastCompositeMaterial = this.CheckShaderAndCreateMaterial(this.contrastCompositeShader, this.contrastCompositeMaterial);
    this.separableBlurMaterial = this.CheckShaderAndCreateMaterial(this.separableBlurShader, this.separableBlurMaterial);
    if (!this.isSupported)
      this.ReportAutoDisable();
    return this.isSupported;
  }

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (!this.CheckResources())
    {
      Graphics.Blit((Texture) source, destination);
    }
    else
    {
      int width = source.width;
      int height = source.height;
      RenderTexture temporary1 = RenderTexture.GetTemporary(width / 2, height / 2, 0);
      Graphics.Blit((Texture) source, temporary1);
      RenderTexture temporary2 = RenderTexture.GetTemporary(width / 4, height / 4, 0);
      Graphics.Blit((Texture) temporary1, temporary2);
      RenderTexture.ReleaseTemporary(temporary1);
      this.separableBlurMaterial.SetVector("offsets", new Vector4(0.0f, this.blurSpread * 1f / (float) temporary2.height, 0.0f, 0.0f));
      RenderTexture temporary3 = RenderTexture.GetTemporary(width / 4, height / 4, 0);
      Graphics.Blit((Texture) temporary2, temporary3, this.separableBlurMaterial);
      RenderTexture.ReleaseTemporary(temporary2);
      this.separableBlurMaterial.SetVector("offsets", new Vector4(this.blurSpread * 1f / (float) temporary2.width, 0.0f, 0.0f, 0.0f));
      RenderTexture temporary4 = RenderTexture.GetTemporary(width / 4, height / 4, 0);
      Graphics.Blit((Texture) temporary3, temporary4, this.separableBlurMaterial);
      RenderTexture.ReleaseTemporary(temporary3);
      this.contrastCompositeMaterial.SetTexture("_MainTexBlurred", (Texture) temporary4);
      this.contrastCompositeMaterial.SetFloat("intensity", this.intensity);
      this.contrastCompositeMaterial.SetFloat("threshold", this.threshold);
      Graphics.Blit((Texture) source, destination, this.contrastCompositeMaterial);
      RenderTexture.ReleaseTemporary(temporary4);
    }
  }
}
