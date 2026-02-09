// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.ImageEffects.BloomOptimized
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Bloom and Glow/Bloom (Optimized)")]
[RequireComponent(typeof (Camera))]
public class BloomOptimized : PostEffectsBase
{
  [Range(0.0f, 1.5f)]
  public float threshold = 0.25f;
  [Range(0.0f, 2.5f)]
  public float intensity = 0.75f;
  [Range(0.25f, 5.5f)]
  public float blurSize = 1f;
  public BloomOptimized.Resolution resolution;
  [Range(1f, 4f)]
  public int blurIterations = 1;
  public BloomOptimized.BlurType blurType;
  public Shader fastBloomShader;
  public Material fastBloomMaterial;

  public override bool CheckResources()
  {
    this.CheckSupport(false);
    this.fastBloomMaterial = this.CheckShaderAndCreateMaterial(this.fastBloomShader, this.fastBloomMaterial);
    if (!this.isSupported)
      this.ReportAutoDisable();
    return this.isSupported;
  }

  public void OnDisable()
  {
    if (!(bool) (Object) this.fastBloomMaterial)
      return;
    Object.DestroyImmediate((Object) this.fastBloomMaterial);
  }

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (!this.CheckResources())
    {
      Graphics.Blit((Texture) source, destination);
    }
    else
    {
      int num1 = this.resolution == BloomOptimized.Resolution.Low ? 4 : 2;
      float num2 = this.resolution == BloomOptimized.Resolution.Low ? 0.5f : 1f;
      this.fastBloomMaterial.SetVector("_Parameter", new Vector4(this.blurSize * num2, 0.0f, this.threshold, this.intensity));
      source.filterMode = FilterMode.Bilinear;
      int width = source.width / num1;
      int height = source.height / num1;
      RenderTexture renderTexture1 = RenderTexture.GetTemporary(width, height, 0, source.format);
      renderTexture1.filterMode = FilterMode.Bilinear;
      Graphics.Blit((Texture) source, renderTexture1, this.fastBloomMaterial, 1);
      int num3 = this.blurType == BloomOptimized.BlurType.Standard ? 0 : 2;
      for (int index = 0; index < this.blurIterations; ++index)
      {
        this.fastBloomMaterial.SetVector("_Parameter", new Vector4((float) ((double) this.blurSize * (double) num2 + (double) index * 1.0), 0.0f, this.threshold, this.intensity));
        RenderTexture temporary1 = RenderTexture.GetTemporary(width, height, 0, source.format);
        temporary1.filterMode = FilterMode.Bilinear;
        Graphics.Blit((Texture) renderTexture1, temporary1, this.fastBloomMaterial, 2 + num3);
        RenderTexture.ReleaseTemporary(renderTexture1);
        RenderTexture renderTexture2 = temporary1;
        RenderTexture temporary2 = RenderTexture.GetTemporary(width, height, 0, source.format);
        temporary2.filterMode = FilterMode.Bilinear;
        Graphics.Blit((Texture) renderTexture2, temporary2, this.fastBloomMaterial, 3 + num3);
        RenderTexture.ReleaseTemporary(renderTexture2);
        renderTexture1 = temporary2;
      }
      this.fastBloomMaterial.SetTexture("_Bloom", (Texture) renderTexture1);
      Graphics.Blit((Texture) source, destination, this.fastBloomMaterial, 0);
      RenderTexture.ReleaseTemporary(renderTexture1);
    }
  }

  public enum Resolution
  {
    Low,
    High,
  }

  public enum BlurType
  {
    Standard,
    Sgx,
  }
}
