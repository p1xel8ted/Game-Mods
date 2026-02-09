// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.ImageEffects.VignetteAndChromaticAberration
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Camera/Vignette and Chromatic Aberration")]
[RequireComponent(typeof (Camera))]
public class VignetteAndChromaticAberration : PostEffectsBase
{
  public VignetteAndChromaticAberration.AberrationMode mode;
  public float intensity = 0.036f;
  public float chromaticAberration = 0.2f;
  public float axialAberration = 0.5f;
  public float blur;
  public float blurSpread = 0.75f;
  public float luminanceDependency = 0.25f;
  public float blurDistance = 2.5f;
  public Shader vignetteShader;
  public Shader separableBlurShader;
  public Shader chromAberrationShader;
  public Material m_VignetteMaterial;
  public Material m_SeparableBlurMaterial;
  public Material m_ChromAberrationMaterial;

  public override bool CheckResources()
  {
    this.CheckSupport(false);
    this.m_VignetteMaterial = this.CheckShaderAndCreateMaterial(this.vignetteShader, this.m_VignetteMaterial);
    this.m_SeparableBlurMaterial = this.CheckShaderAndCreateMaterial(this.separableBlurShader, this.m_SeparableBlurMaterial);
    this.m_ChromAberrationMaterial = this.CheckShaderAndCreateMaterial(this.chromAberrationShader, this.m_ChromAberrationMaterial);
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
      bool flag = (double) Mathf.Abs(this.blur) > 0.0 || (double) Mathf.Abs(this.intensity) > 0.0;
      float num = (float) (1.0 * (double) width / (1.0 * (double) height));
      RenderTexture renderTexture1 = (RenderTexture) null;
      RenderTexture renderTexture2 = (RenderTexture) null;
      if (flag)
      {
        renderTexture1 = RenderTexture.GetTemporary(width, height, 0, source.format);
        if ((double) Mathf.Abs(this.blur) > 0.0)
        {
          renderTexture2 = RenderTexture.GetTemporary(width / 2, height / 2, 0, source.format);
          Graphics.Blit((Texture) source, renderTexture2, this.m_ChromAberrationMaterial, 0);
          for (int index = 0; index < 2; ++index)
          {
            this.m_SeparableBlurMaterial.SetVector("offsets", new Vector4(0.0f, this.blurSpread * (1f / 512f), 0.0f, 0.0f));
            RenderTexture temporary = RenderTexture.GetTemporary(width / 2, height / 2, 0, source.format);
            Graphics.Blit((Texture) renderTexture2, temporary, this.m_SeparableBlurMaterial);
            RenderTexture.ReleaseTemporary(renderTexture2);
            this.m_SeparableBlurMaterial.SetVector("offsets", new Vector4(this.blurSpread * (1f / 512f) / num, 0.0f, 0.0f, 0.0f));
            renderTexture2 = RenderTexture.GetTemporary(width / 2, height / 2, 0, source.format);
            Graphics.Blit((Texture) temporary, renderTexture2, this.m_SeparableBlurMaterial);
            RenderTexture.ReleaseTemporary(temporary);
          }
        }
        this.m_VignetteMaterial.SetFloat("_Intensity", (float) (1.0 / (1.0 - (double) this.intensity) - 1.0));
        this.m_VignetteMaterial.SetFloat("_Blur", (float) (1.0 / (1.0 - (double) this.blur) - 1.0));
        this.m_VignetteMaterial.SetTexture("_VignetteTex", (Texture) renderTexture2);
        Graphics.Blit((Texture) source, renderTexture1, this.m_VignetteMaterial, 0);
      }
      this.m_ChromAberrationMaterial.SetFloat("_ChromaticAberration", this.chromaticAberration);
      this.m_ChromAberrationMaterial.SetFloat("_AxialAberration", this.axialAberration);
      this.m_ChromAberrationMaterial.SetVector("_BlurDistance", (Vector4) new Vector2(-this.blurDistance, this.blurDistance));
      this.m_ChromAberrationMaterial.SetFloat("_Luminance", 1f / Mathf.Max(Mathf.Epsilon, this.luminanceDependency));
      if (flag)
        renderTexture1.wrapMode = TextureWrapMode.Clamp;
      else
        source.wrapMode = TextureWrapMode.Clamp;
      Graphics.Blit(flag ? (Texture) renderTexture1 : (Texture) source, destination, this.m_ChromAberrationMaterial, this.mode == VignetteAndChromaticAberration.AberrationMode.Advanced ? 2 : 1);
      RenderTexture.ReleaseTemporary(renderTexture1);
      RenderTexture.ReleaseTemporary(renderTexture2);
    }
  }

  public enum AberrationMode
  {
    Simple,
    Advanced,
  }
}
