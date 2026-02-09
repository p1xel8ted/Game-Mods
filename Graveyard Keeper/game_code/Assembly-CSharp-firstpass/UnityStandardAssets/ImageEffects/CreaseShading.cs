// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.ImageEffects.CreaseShading
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace UnityStandardAssets.ImageEffects;

[RequireComponent(typeof (Camera))]
[AddComponentMenu("Image Effects/Edge Detection/Crease Shading")]
[ExecuteInEditMode]
public class CreaseShading : PostEffectsBase
{
  public float intensity = 0.5f;
  public int softness = 1;
  public float spread = 1f;
  public Shader blurShader;
  public Material blurMaterial;
  public Shader depthFetchShader;
  public Material depthFetchMaterial;
  public Shader creaseApplyShader;
  public Material creaseApplyMaterial;

  public override bool CheckResources()
  {
    this.CheckSupport(true);
    this.blurMaterial = this.CheckShaderAndCreateMaterial(this.blurShader, this.blurMaterial);
    this.depthFetchMaterial = this.CheckShaderAndCreateMaterial(this.depthFetchShader, this.depthFetchMaterial);
    this.creaseApplyMaterial = this.CheckShaderAndCreateMaterial(this.creaseApplyShader, this.creaseApplyMaterial);
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
      float num1 = (float) (1.0 * (double) width / (1.0 * (double) height));
      float num2 = 1f / 512f;
      RenderTexture temporary1 = RenderTexture.GetTemporary(width, height, 0);
      RenderTexture renderTexture1 = RenderTexture.GetTemporary(width / 2, height / 2, 0);
      Graphics.Blit((Texture) source, temporary1, this.depthFetchMaterial);
      Graphics.Blit((Texture) temporary1, renderTexture1);
      for (int index = 0; index < this.softness; ++index)
      {
        RenderTexture temporary2 = RenderTexture.GetTemporary(width / 2, height / 2, 0);
        this.blurMaterial.SetVector("offsets", new Vector4(0.0f, this.spread * num2, 0.0f, 0.0f));
        Graphics.Blit((Texture) renderTexture1, temporary2, this.blurMaterial);
        RenderTexture.ReleaseTemporary(renderTexture1);
        RenderTexture renderTexture2 = temporary2;
        RenderTexture temporary3 = RenderTexture.GetTemporary(width / 2, height / 2, 0);
        this.blurMaterial.SetVector("offsets", new Vector4(this.spread * num2 / num1, 0.0f, 0.0f, 0.0f));
        Graphics.Blit((Texture) renderTexture2, temporary3, this.blurMaterial);
        RenderTexture.ReleaseTemporary(renderTexture2);
        renderTexture1 = temporary3;
      }
      this.creaseApplyMaterial.SetTexture("_HrDepthTex", (Texture) temporary1);
      this.creaseApplyMaterial.SetTexture("_LrDepthTex", (Texture) renderTexture1);
      this.creaseApplyMaterial.SetFloat("intensity", this.intensity);
      Graphics.Blit((Texture) source, destination, this.creaseApplyMaterial);
      RenderTexture.ReleaseTemporary(temporary1);
      RenderTexture.ReleaseTemporary(renderTexture1);
    }
  }
}
