// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.ImageEffects.DepthOfFieldDeprecated
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
[AddComponentMenu("Image Effects/Camera/Depth of Field (deprecated)")]
public class DepthOfFieldDeprecated : PostEffectsBase
{
  public static int SMOOTH_DOWNSAMPLE_PASS = 6;
  public static float BOKEH_EXTRA_BLUR = 2f;
  public DepthOfFieldDeprecated.Dof34QualitySetting quality = DepthOfFieldDeprecated.Dof34QualitySetting.OnlyBackground;
  public DepthOfFieldDeprecated.DofResolution resolution = DepthOfFieldDeprecated.DofResolution.Low;
  public bool simpleTweakMode = true;
  public float focalPoint = 1f;
  public float smoothness = 0.5f;
  public float focalZDistance;
  public float focalZStartCurve = 1f;
  public float focalZEndCurve = 1f;
  public float focalStartCurve = 2f;
  public float focalEndCurve = 2f;
  public float focalDistance01 = 0.1f;
  public Transform objectFocus;
  public float focalSize;
  public DepthOfFieldDeprecated.DofBlurriness bluriness = DepthOfFieldDeprecated.DofBlurriness.High;
  public float maxBlurSpread = 1.75f;
  public float foregroundBlurExtrude = 1.15f;
  public Shader dofBlurShader;
  public Material dofBlurMaterial;
  public Shader dofShader;
  public Material dofMaterial;
  public bool visualize;
  public DepthOfFieldDeprecated.BokehDestination bokehDestination = DepthOfFieldDeprecated.BokehDestination.Background;
  public float widthOverHeight = 1.25f;
  public float oneOverBaseSize = 1f / 512f;
  public bool bokeh;
  public bool bokehSupport = true;
  public Shader bokehShader;
  public Texture2D bokehTexture;
  public float bokehScale = 2.4f;
  public float bokehIntensity = 0.15f;
  public float bokehThresholdContrast = 0.1f;
  public float bokehThresholdLuminance = 0.55f;
  public int bokehDownsample = 1;
  public Material bokehMaterial;
  public Camera _camera;
  public RenderTexture foregroundTexture;
  public RenderTexture mediumRezWorkTexture;
  public RenderTexture finalDefocus;
  public RenderTexture lowRezWorkTexture;
  public RenderTexture bokehSource;
  public RenderTexture bokehSource2;

  public void CreateMaterials()
  {
    this.dofBlurMaterial = this.CheckShaderAndCreateMaterial(this.dofBlurShader, this.dofBlurMaterial);
    this.dofMaterial = this.CheckShaderAndCreateMaterial(this.dofShader, this.dofMaterial);
    this.bokehSupport = this.bokehShader.isSupported;
    if (!this.bokeh || !this.bokehSupport || !(bool) (Object) this.bokehShader)
      return;
    this.bokehMaterial = this.CheckShaderAndCreateMaterial(this.bokehShader, this.bokehMaterial);
  }

  public override bool CheckResources()
  {
    this.CheckSupport(true);
    this.dofBlurMaterial = this.CheckShaderAndCreateMaterial(this.dofBlurShader, this.dofBlurMaterial);
    this.dofMaterial = this.CheckShaderAndCreateMaterial(this.dofShader, this.dofMaterial);
    this.bokehSupport = this.bokehShader.isSupported;
    if (this.bokeh && this.bokehSupport && (bool) (Object) this.bokehShader)
      this.bokehMaterial = this.CheckShaderAndCreateMaterial(this.bokehShader, this.bokehMaterial);
    if (!this.isSupported)
      this.ReportAutoDisable();
    return this.isSupported;
  }

  public void OnDisable() => Quads.Cleanup();

  public new void OnEnable()
  {
    this._camera = this.GetComponent<Camera>();
    this._camera.depthTextureMode |= DepthTextureMode.Depth;
  }

  public float FocalDistance01(float worldDist)
  {
    return this._camera.WorldToViewportPoint((worldDist - this._camera.nearClipPlane) * this._camera.transform.forward + this._camera.transform.position).z / (this._camera.farClipPlane - this._camera.nearClipPlane);
  }

  public int GetDividerBasedOnQuality()
  {
    int dividerBasedOnQuality = 1;
    if (this.resolution == DepthOfFieldDeprecated.DofResolution.Medium)
      dividerBasedOnQuality = 2;
    else if (this.resolution == DepthOfFieldDeprecated.DofResolution.Low)
      dividerBasedOnQuality = 2;
    return dividerBasedOnQuality;
  }

  public int GetLowResolutionDividerBasedOnQuality(int baseDivider)
  {
    int dividerBasedOnQuality = baseDivider;
    if (this.resolution == DepthOfFieldDeprecated.DofResolution.High)
      dividerBasedOnQuality *= 2;
    if (this.resolution == DepthOfFieldDeprecated.DofResolution.Low)
      dividerBasedOnQuality *= 2;
    return dividerBasedOnQuality;
  }

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (!this.CheckResources())
    {
      Graphics.Blit((Texture) source, destination);
    }
    else
    {
      if ((double) this.smoothness < 0.10000000149011612)
        this.smoothness = 0.1f;
      this.bokeh = this.bokeh && this.bokehSupport;
      float num1 = this.bokeh ? DepthOfFieldDeprecated.BOKEH_EXTRA_BLUR : 1f;
      bool flag = this.quality > DepthOfFieldDeprecated.Dof34QualitySetting.OnlyBackground;
      float num2 = this.focalSize / (this._camera.farClipPlane - this._camera.nearClipPlane);
      bool blurForeground;
      if (this.simpleTweakMode)
      {
        this.focalDistance01 = (bool) (Object) this.objectFocus ? this._camera.WorldToViewportPoint(this.objectFocus.position).z / this._camera.farClipPlane : this.FocalDistance01(this.focalPoint);
        this.focalStartCurve = this.focalDistance01 * this.smoothness;
        this.focalEndCurve = this.focalStartCurve;
        blurForeground = flag && (double) this.focalPoint > (double) this._camera.nearClipPlane + (double) Mathf.Epsilon;
      }
      else
      {
        if ((bool) (Object) this.objectFocus)
        {
          Vector3 viewportPoint = this._camera.WorldToViewportPoint(this.objectFocus.position);
          viewportPoint.z /= this._camera.farClipPlane;
          this.focalDistance01 = viewportPoint.z;
        }
        else
          this.focalDistance01 = this.FocalDistance01(this.focalZDistance);
        this.focalStartCurve = this.focalZStartCurve;
        this.focalEndCurve = this.focalZEndCurve;
        blurForeground = flag && (double) this.focalPoint > (double) this._camera.nearClipPlane + (double) Mathf.Epsilon;
      }
      this.widthOverHeight = (float) (1.0 * (double) source.width / (1.0 * (double) source.height));
      this.oneOverBaseSize = 1f / 512f;
      this.dofMaterial.SetFloat("_ForegroundBlurExtrude", this.foregroundBlurExtrude);
      this.dofMaterial.SetVector("_CurveParams", new Vector4(this.simpleTweakMode ? 1f / this.focalStartCurve : this.focalStartCurve, this.simpleTweakMode ? 1f / this.focalEndCurve : this.focalEndCurve, num2 * 0.5f, this.focalDistance01));
      this.dofMaterial.SetVector("_InvRenderTargetSize", new Vector4((float) (1.0 / (1.0 * (double) source.width)), (float) (1.0 / (1.0 * (double) source.height)), 0.0f, 0.0f));
      int dividerBasedOnQuality1 = this.GetDividerBasedOnQuality();
      int dividerBasedOnQuality2 = this.GetLowResolutionDividerBasedOnQuality(dividerBasedOnQuality1);
      this.AllocateTextures(blurForeground, source, dividerBasedOnQuality1, dividerBasedOnQuality2);
      Graphics.Blit((Texture) source, source, this.dofMaterial, 3);
      this.Downsample(source, this.mediumRezWorkTexture);
      this.Blur(this.mediumRezWorkTexture, this.mediumRezWorkTexture, DepthOfFieldDeprecated.DofBlurriness.Low, 4, this.maxBlurSpread);
      if (this.bokeh && (DepthOfFieldDeprecated.BokehDestination.Foreground & this.bokehDestination) != (DepthOfFieldDeprecated.BokehDestination) 0)
      {
        this.dofMaterial.SetVector("_Threshhold", new Vector4(this.bokehThresholdContrast, this.bokehThresholdLuminance, 0.95f, 0.0f));
        Graphics.Blit((Texture) this.mediumRezWorkTexture, this.bokehSource2, this.dofMaterial, 11);
        Graphics.Blit((Texture) this.mediumRezWorkTexture, this.lowRezWorkTexture);
        this.Blur(this.lowRezWorkTexture, this.lowRezWorkTexture, this.bluriness, 0, this.maxBlurSpread * num1);
      }
      else
      {
        this.Downsample(this.mediumRezWorkTexture, this.lowRezWorkTexture);
        this.Blur(this.lowRezWorkTexture, this.lowRezWorkTexture, this.bluriness, 0, this.maxBlurSpread);
      }
      this.dofBlurMaterial.SetTexture("_TapLow", (Texture) this.lowRezWorkTexture);
      this.dofBlurMaterial.SetTexture("_TapMedium", (Texture) this.mediumRezWorkTexture);
      Graphics.Blit((Texture) null, this.finalDefocus, this.dofBlurMaterial, 3);
      if (this.bokeh && (DepthOfFieldDeprecated.BokehDestination.Foreground & this.bokehDestination) != (DepthOfFieldDeprecated.BokehDestination) 0)
        this.AddBokeh(this.bokehSource2, this.bokehSource, this.finalDefocus);
      this.dofMaterial.SetTexture("_TapLowBackground", (Texture) this.finalDefocus);
      this.dofMaterial.SetTexture("_TapMedium", (Texture) this.mediumRezWorkTexture);
      Graphics.Blit((Texture) source, blurForeground ? this.foregroundTexture : destination, this.dofMaterial, this.visualize ? 2 : 0);
      if (blurForeground)
      {
        Graphics.Blit((Texture) this.foregroundTexture, source, this.dofMaterial, 5);
        this.Downsample(source, this.mediumRezWorkTexture);
        this.BlurFg(this.mediumRezWorkTexture, this.mediumRezWorkTexture, DepthOfFieldDeprecated.DofBlurriness.Low, 2, this.maxBlurSpread);
        if (this.bokeh && (DepthOfFieldDeprecated.BokehDestination.Foreground & this.bokehDestination) != (DepthOfFieldDeprecated.BokehDestination) 0)
        {
          this.dofMaterial.SetVector("_Threshhold", new Vector4(this.bokehThresholdContrast * 0.5f, this.bokehThresholdLuminance, 0.0f, 0.0f));
          Graphics.Blit((Texture) this.mediumRezWorkTexture, this.bokehSource2, this.dofMaterial, 11);
          Graphics.Blit((Texture) this.mediumRezWorkTexture, this.lowRezWorkTexture);
          this.BlurFg(this.lowRezWorkTexture, this.lowRezWorkTexture, this.bluriness, 1, this.maxBlurSpread * num1);
        }
        else
          this.BlurFg(this.mediumRezWorkTexture, this.lowRezWorkTexture, this.bluriness, 1, this.maxBlurSpread);
        Graphics.Blit((Texture) this.lowRezWorkTexture, this.finalDefocus);
        this.dofMaterial.SetTexture("_TapLowForeground", (Texture) this.finalDefocus);
        Graphics.Blit((Texture) source, destination, this.dofMaterial, this.visualize ? 1 : 4);
        if (this.bokeh && (DepthOfFieldDeprecated.BokehDestination.Foreground & this.bokehDestination) != (DepthOfFieldDeprecated.BokehDestination) 0)
          this.AddBokeh(this.bokehSource2, this.bokehSource, destination);
      }
      this.ReleaseTextures();
    }
  }

  public void Blur(
    RenderTexture from,
    RenderTexture to,
    DepthOfFieldDeprecated.DofBlurriness iterations,
    int blurPass,
    float spread)
  {
    RenderTexture temporary = RenderTexture.GetTemporary(to.width, to.height);
    if (iterations > DepthOfFieldDeprecated.DofBlurriness.Low)
    {
      this.BlurHex(from, to, blurPass, spread, temporary);
      if (iterations > DepthOfFieldDeprecated.DofBlurriness.High)
      {
        this.dofBlurMaterial.SetVector("offsets", new Vector4(0.0f, spread * this.oneOverBaseSize, 0.0f, 0.0f));
        Graphics.Blit((Texture) to, temporary, this.dofBlurMaterial, blurPass);
        this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, 0.0f, 0.0f, 0.0f));
        Graphics.Blit((Texture) temporary, to, this.dofBlurMaterial, blurPass);
      }
    }
    else
    {
      this.dofBlurMaterial.SetVector("offsets", new Vector4(0.0f, spread * this.oneOverBaseSize, 0.0f, 0.0f));
      Graphics.Blit((Texture) from, temporary, this.dofBlurMaterial, blurPass);
      this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, 0.0f, 0.0f, 0.0f));
      Graphics.Blit((Texture) temporary, to, this.dofBlurMaterial, blurPass);
    }
    RenderTexture.ReleaseTemporary(temporary);
  }

  public void BlurFg(
    RenderTexture from,
    RenderTexture to,
    DepthOfFieldDeprecated.DofBlurriness iterations,
    int blurPass,
    float spread)
  {
    this.dofBlurMaterial.SetTexture("_TapHigh", (Texture) from);
    RenderTexture temporary = RenderTexture.GetTemporary(to.width, to.height);
    if (iterations > DepthOfFieldDeprecated.DofBlurriness.Low)
    {
      this.BlurHex(from, to, blurPass, spread, temporary);
      if (iterations > DepthOfFieldDeprecated.DofBlurriness.High)
      {
        this.dofBlurMaterial.SetVector("offsets", new Vector4(0.0f, spread * this.oneOverBaseSize, 0.0f, 0.0f));
        Graphics.Blit((Texture) to, temporary, this.dofBlurMaterial, blurPass);
        this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, 0.0f, 0.0f, 0.0f));
        Graphics.Blit((Texture) temporary, to, this.dofBlurMaterial, blurPass);
      }
    }
    else
    {
      this.dofBlurMaterial.SetVector("offsets", new Vector4(0.0f, spread * this.oneOverBaseSize, 0.0f, 0.0f));
      Graphics.Blit((Texture) from, temporary, this.dofBlurMaterial, blurPass);
      this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, 0.0f, 0.0f, 0.0f));
      Graphics.Blit((Texture) temporary, to, this.dofBlurMaterial, blurPass);
    }
    RenderTexture.ReleaseTemporary(temporary);
  }

  public void BlurHex(
    RenderTexture from,
    RenderTexture to,
    int blurPass,
    float spread,
    RenderTexture tmp)
  {
    this.dofBlurMaterial.SetVector("offsets", new Vector4(0.0f, spread * this.oneOverBaseSize, 0.0f, 0.0f));
    Graphics.Blit((Texture) from, tmp, this.dofBlurMaterial, blurPass);
    this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, 0.0f, 0.0f, 0.0f));
    Graphics.Blit((Texture) tmp, to, this.dofBlurMaterial, blurPass);
    this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, spread * this.oneOverBaseSize, 0.0f, 0.0f));
    Graphics.Blit((Texture) to, tmp, this.dofBlurMaterial, blurPass);
    this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, -spread * this.oneOverBaseSize, 0.0f, 0.0f));
    Graphics.Blit((Texture) tmp, to, this.dofBlurMaterial, blurPass);
  }

  public void Downsample(RenderTexture from, RenderTexture to)
  {
    this.dofMaterial.SetVector("_InvRenderTargetSize", new Vector4((float) (1.0 / (1.0 * (double) to.width)), (float) (1.0 / (1.0 * (double) to.height)), 0.0f, 0.0f));
    Graphics.Blit((Texture) from, to, this.dofMaterial, DepthOfFieldDeprecated.SMOOTH_DOWNSAMPLE_PASS);
  }

  public void AddBokeh(RenderTexture bokehInfo, RenderTexture tempTex, RenderTexture finalTarget)
  {
    if (!(bool) (Object) this.bokehMaterial)
      return;
    Mesh[] meshes = Quads.GetMeshes(tempTex.width, tempTex.height);
    RenderTexture.active = tempTex;
    GL.Clear(false, true, new Color(0.0f, 0.0f, 0.0f, 0.0f));
    GL.PushMatrix();
    GL.LoadIdentity();
    bokehInfo.filterMode = FilterMode.Point;
    float num = (float) ((double) bokehInfo.width * 1.0 / ((double) bokehInfo.height * 1.0));
    float x = (float) (2.0 / (1.0 * (double) bokehInfo.width)) + this.bokehScale * this.maxBlurSpread * DepthOfFieldDeprecated.BOKEH_EXTRA_BLUR * this.oneOverBaseSize;
    this.bokehMaterial.SetTexture("_Source", (Texture) bokehInfo);
    this.bokehMaterial.SetTexture("_MainTex", (Texture) this.bokehTexture);
    this.bokehMaterial.SetVector("_ArScale", new Vector4(x, x * num, 0.5f, 0.5f * num));
    this.bokehMaterial.SetFloat("_Intensity", this.bokehIntensity);
    this.bokehMaterial.SetPass(0);
    foreach (Mesh mesh in meshes)
    {
      if ((bool) (Object) mesh)
        Graphics.DrawMeshNow(mesh, Matrix4x4.identity);
    }
    GL.PopMatrix();
    Graphics.Blit((Texture) tempTex, finalTarget, this.dofMaterial, 8);
    bokehInfo.filterMode = FilterMode.Bilinear;
  }

  public void ReleaseTextures()
  {
    if ((bool) (Object) this.foregroundTexture)
      RenderTexture.ReleaseTemporary(this.foregroundTexture);
    if ((bool) (Object) this.finalDefocus)
      RenderTexture.ReleaseTemporary(this.finalDefocus);
    if ((bool) (Object) this.mediumRezWorkTexture)
      RenderTexture.ReleaseTemporary(this.mediumRezWorkTexture);
    if ((bool) (Object) this.lowRezWorkTexture)
      RenderTexture.ReleaseTemporary(this.lowRezWorkTexture);
    if ((bool) (Object) this.bokehSource)
      RenderTexture.ReleaseTemporary(this.bokehSource);
    if (!(bool) (Object) this.bokehSource2)
      return;
    RenderTexture.ReleaseTemporary(this.bokehSource2);
  }

  public void AllocateTextures(
    bool blurForeground,
    RenderTexture source,
    int divider,
    int lowTexDivider)
  {
    this.foregroundTexture = (RenderTexture) null;
    if (blurForeground)
      this.foregroundTexture = RenderTexture.GetTemporary(source.width, source.height, 0);
    this.mediumRezWorkTexture = RenderTexture.GetTemporary(source.width / divider, source.height / divider, 0);
    this.finalDefocus = RenderTexture.GetTemporary(source.width / divider, source.height / divider, 0);
    this.lowRezWorkTexture = RenderTexture.GetTemporary(source.width / lowTexDivider, source.height / lowTexDivider, 0);
    this.bokehSource = (RenderTexture) null;
    this.bokehSource2 = (RenderTexture) null;
    if (this.bokeh)
    {
      this.bokehSource = RenderTexture.GetTemporary(source.width / (lowTexDivider * this.bokehDownsample), source.height / (lowTexDivider * this.bokehDownsample), 0, RenderTextureFormat.ARGBHalf);
      this.bokehSource2 = RenderTexture.GetTemporary(source.width / (lowTexDivider * this.bokehDownsample), source.height / (lowTexDivider * this.bokehDownsample), 0, RenderTextureFormat.ARGBHalf);
      this.bokehSource.filterMode = FilterMode.Bilinear;
      this.bokehSource2.filterMode = FilterMode.Bilinear;
      RenderTexture.active = this.bokehSource2;
      GL.Clear(false, true, new Color(0.0f, 0.0f, 0.0f, 0.0f));
    }
    source.filterMode = FilterMode.Bilinear;
    this.finalDefocus.filterMode = FilterMode.Bilinear;
    this.mediumRezWorkTexture.filterMode = FilterMode.Bilinear;
    this.lowRezWorkTexture.filterMode = FilterMode.Bilinear;
    if (!(bool) (Object) this.foregroundTexture)
      return;
    this.foregroundTexture.filterMode = FilterMode.Bilinear;
  }

  public enum Dof34QualitySetting
  {
    OnlyBackground = 1,
    BackgroundAndForeground = 2,
  }

  public enum DofResolution
  {
    High = 2,
    Medium = 3,
    Low = 4,
  }

  public enum DofBlurriness
  {
    Low = 1,
    High = 2,
    VeryHigh = 4,
  }

  public enum BokehDestination
  {
    Background = 1,
    Foreground = 2,
    BackgroundAndForeground = 3,
  }
}
