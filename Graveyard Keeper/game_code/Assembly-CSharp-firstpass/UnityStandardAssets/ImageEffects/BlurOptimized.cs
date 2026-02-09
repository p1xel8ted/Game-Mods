// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.ImageEffects.BlurOptimized
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace UnityStandardAssets.ImageEffects;

[RequireComponent(typeof (Camera))]
[AddComponentMenu("Image Effects/Blur/Blur (Optimized)")]
[ExecuteInEditMode]
public class BlurOptimized : PostEffectsBase
{
  [Range(0.0f, 2f)]
  public int downsample = 1;
  [Range(0.0f, 10f)]
  public float blurSize = 3f;
  [Range(1f, 4f)]
  public int blurIterations = 2;
  public BlurOptimized.BlurType blurType;
  public Shader blurShader;
  public Material blurMaterial;

  public override bool CheckResources()
  {
    this.CheckSupport(false);
    this.blurMaterial = this.CheckShaderAndCreateMaterial(this.blurShader, this.blurMaterial);
    if (!this.isSupported)
      this.ReportAutoDisable();
    return this.isSupported;
  }

  public void OnDisable()
  {
    if (!(bool) (Object) this.blurMaterial)
      return;
    Object.DestroyImmediate((Object) this.blurMaterial);
  }

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (!this.CheckResources())
    {
      Graphics.Blit((Texture) source, destination);
    }
    else
    {
      float num1 = (float) (1.0 / (1.0 * (double) (1 << this.downsample)));
      this.blurMaterial.SetVector("_Parameter", new Vector4(this.blurSize * num1, -this.blurSize * num1, 0.0f, 0.0f));
      source.filterMode = FilterMode.Bilinear;
      int width = source.width >> this.downsample;
      int height = source.height >> this.downsample;
      RenderTexture renderTexture1 = RenderTexture.GetTemporary(width, height, 0, source.format);
      renderTexture1.filterMode = FilterMode.Bilinear;
      Graphics.Blit((Texture) source, renderTexture1, this.blurMaterial, 0);
      int num2 = this.blurType == BlurOptimized.BlurType.StandardGauss ? 0 : 2;
      for (int index = 0; index < this.blurIterations; ++index)
      {
        float num3 = (float) index * 1f;
        this.blurMaterial.SetVector("_Parameter", new Vector4(this.blurSize * num1 + num3, -this.blurSize * num1 - num3, 0.0f, 0.0f));
        RenderTexture temporary1 = RenderTexture.GetTemporary(width, height, 0, source.format);
        temporary1.filterMode = FilterMode.Bilinear;
        Graphics.Blit((Texture) renderTexture1, temporary1, this.blurMaterial, 1 + num2);
        RenderTexture.ReleaseTemporary(renderTexture1);
        RenderTexture renderTexture2 = temporary1;
        RenderTexture temporary2 = RenderTexture.GetTemporary(width, height, 0, source.format);
        temporary2.filterMode = FilterMode.Bilinear;
        Graphics.Blit((Texture) renderTexture2, temporary2, this.blurMaterial, 2 + num2);
        RenderTexture.ReleaseTemporary(renderTexture2);
        renderTexture1 = temporary2;
      }
      Graphics.Blit((Texture) renderTexture1, destination);
      RenderTexture.ReleaseTemporary(renderTexture1);
    }
  }

  public enum BlurType
  {
    StandardGauss,
    SgxGauss,
  }
}
