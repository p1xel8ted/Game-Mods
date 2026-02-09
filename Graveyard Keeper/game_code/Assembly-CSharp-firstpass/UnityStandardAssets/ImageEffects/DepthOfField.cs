// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.ImageEffects.DepthOfField
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

#nullable disable
namespace UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
[AddComponentMenu("Image Effects/Camera/Depth of Field (Lens Blur, Scatter, DX11)")]
public class DepthOfField : PostEffectsBase
{
  public bool visualizeFocus;
  public float focalLength = 10f;
  public float focalSize = 0.05f;
  public float aperture = 0.5f;
  public Transform focalTransform;
  public float maxBlurSize = 2f;
  public bool highResolution;
  public DepthOfField.BlurType blurType;
  public DepthOfField.BlurSampleCount blurSampleCount = DepthOfField.BlurSampleCount.High;
  public bool nearBlur;
  public float foregroundOverlap = 1f;
  public Shader dofHdrShader;
  public Material dofHdrMaterial;
  public Shader dx11BokehShader;
  public Material dx11bokehMaterial;
  public float dx11BokehThreshold = 0.5f;
  public float dx11SpawnHeuristic = 0.0875f;
  public Texture2D dx11BokehTexture;
  public float dx11BokehScale = 1.2f;
  public float dx11BokehIntensity = 2.5f;
  public float focalDistance01 = 10f;
  public ComputeBuffer cbDrawArgs;
  public ComputeBuffer cbPoints;
  public float internalBlurWidth = 1f;
  public Camera cachedCamera;

  public override bool CheckResources()
  {
    this.CheckSupport(true);
    this.dofHdrMaterial = this.CheckShaderAndCreateMaterial(this.dofHdrShader, this.dofHdrMaterial);
    if (this.supportDX11 && this.blurType == DepthOfField.BlurType.DX11)
    {
      this.dx11bokehMaterial = this.CheckShaderAndCreateMaterial(this.dx11BokehShader, this.dx11bokehMaterial);
      this.CreateComputeResources();
    }
    if (!this.isSupported)
      this.ReportAutoDisable();
    return this.isSupported;
  }

  public new void OnEnable()
  {
    this.cachedCamera = this.GetComponent<Camera>();
    this.cachedCamera.depthTextureMode |= DepthTextureMode.Depth;
  }

  public void OnDisable()
  {
    this.ReleaseComputeResources();
    if ((bool) (UnityEngine.Object) this.dofHdrMaterial)
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.dofHdrMaterial);
    this.dofHdrMaterial = (Material) null;
    if ((bool) (UnityEngine.Object) this.dx11bokehMaterial)
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.dx11bokehMaterial);
    this.dx11bokehMaterial = (Material) null;
  }

  public void ReleaseComputeResources()
  {
    if (this.cbDrawArgs != null)
      this.cbDrawArgs.Release();
    this.cbDrawArgs = (ComputeBuffer) null;
    if (this.cbPoints != null)
      this.cbPoints.Release();
    this.cbPoints = (ComputeBuffer) null;
  }

  public void CreateComputeResources()
  {
    if (this.cbDrawArgs == null)
    {
      this.cbDrawArgs = new ComputeBuffer(1, 16 /*0x10*/, ComputeBufferType.DrawIndirect);
      this.cbDrawArgs.SetData((Array) new int[4]
      {
        0,
        1,
        0,
        0
      });
    }
    if (this.cbPoints != null)
      return;
    this.cbPoints = new ComputeBuffer(90000, 28, ComputeBufferType.Append);
  }

  public float FocalDistance01(float worldDist)
  {
    return this.cachedCamera.WorldToViewportPoint((worldDist - this.cachedCamera.nearClipPlane) * this.cachedCamera.transform.forward + this.cachedCamera.transform.position).z / (this.cachedCamera.farClipPlane - this.cachedCamera.nearClipPlane);
  }

  public void WriteCoc(RenderTexture fromTo, bool fgDilate)
  {
    this.dofHdrMaterial.SetTexture("_FgOverlap", (Texture) null);
    if (this.nearBlur & fgDilate)
    {
      int width = fromTo.width / 2;
      int height = fromTo.height / 2;
      RenderTexture temporary1 = RenderTexture.GetTemporary(width, height, 0, fromTo.format);
      Graphics.Blit((Texture) fromTo, temporary1, this.dofHdrMaterial, 4);
      float num = this.internalBlurWidth * this.foregroundOverlap;
      this.dofHdrMaterial.SetVector("_Offsets", new Vector4(0.0f, num, 0.0f, num));
      RenderTexture temporary2 = RenderTexture.GetTemporary(width, height, 0, fromTo.format);
      Graphics.Blit((Texture) temporary1, temporary2, this.dofHdrMaterial, 2);
      RenderTexture.ReleaseTemporary(temporary1);
      this.dofHdrMaterial.SetVector("_Offsets", new Vector4(num, 0.0f, 0.0f, num));
      RenderTexture temporary3 = RenderTexture.GetTemporary(width, height, 0, fromTo.format);
      Graphics.Blit((Texture) temporary2, temporary3, this.dofHdrMaterial, 2);
      RenderTexture.ReleaseTemporary(temporary2);
      this.dofHdrMaterial.SetTexture("_FgOverlap", (Texture) temporary3);
      fromTo.MarkRestoreExpected();
      Graphics.Blit((Texture) fromTo, fromTo, this.dofHdrMaterial, 13);
      RenderTexture.ReleaseTemporary(temporary3);
    }
    else
    {
      fromTo.MarkRestoreExpected();
      Graphics.Blit((Texture) fromTo, fromTo, this.dofHdrMaterial, 0);
    }
  }

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (!this.CheckResources())
    {
      Graphics.Blit((Texture) source, destination);
    }
    else
    {
      if ((double) this.aperture < 0.0)
        this.aperture = 0.0f;
      if ((double) this.maxBlurSize < 0.10000000149011612)
        this.maxBlurSize = 0.1f;
      this.focalSize = Mathf.Clamp(this.focalSize, 0.0f, 2f);
      this.internalBlurWidth = Mathf.Max(this.maxBlurSize, 0.0f);
      this.focalDistance01 = (bool) (UnityEngine.Object) this.focalTransform ? this.cachedCamera.WorldToViewportPoint(this.focalTransform.position).z / this.cachedCamera.farClipPlane : this.FocalDistance01(this.focalLength);
      this.dofHdrMaterial.SetVector("_CurveParams", new Vector4(1f, this.focalSize, (float) (1.0 / (1.0 - (double) this.aperture) - 1.0), this.focalDistance01));
      RenderTexture renderTexture1 = (RenderTexture) null;
      RenderTexture renderTexture2 = (RenderTexture) null;
      float num1 = this.internalBlurWidth * this.foregroundOverlap;
      if (this.visualizeFocus)
      {
        this.WriteCoc(source, true);
        Graphics.Blit((Texture) source, destination, this.dofHdrMaterial, 16 /*0x10*/);
      }
      else if (this.blurType == DepthOfField.BlurType.DX11 && (bool) (UnityEngine.Object) this.dx11bokehMaterial)
      {
        if (this.highResolution)
        {
          this.internalBlurWidth = (double) this.internalBlurWidth < 0.10000000149011612 ? 0.1f : this.internalBlurWidth;
          float num2 = this.internalBlurWidth * this.foregroundOverlap;
          renderTexture1 = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
          RenderTexture temporary1 = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
          this.WriteCoc(source, false);
          RenderTexture temporary2 = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format);
          RenderTexture temporary3 = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format);
          Graphics.Blit((Texture) source, temporary2, this.dofHdrMaterial, 15);
          this.dofHdrMaterial.SetVector("_Offsets", new Vector4(0.0f, 1.5f, 0.0f, 1.5f));
          Graphics.Blit((Texture) temporary2, temporary3, this.dofHdrMaterial, 19);
          this.dofHdrMaterial.SetVector("_Offsets", new Vector4(1.5f, 0.0f, 0.0f, 1.5f));
          Graphics.Blit((Texture) temporary3, temporary2, this.dofHdrMaterial, 19);
          if (this.nearBlur)
            Graphics.Blit((Texture) source, temporary3, this.dofHdrMaterial, 4);
          this.dx11bokehMaterial.SetTexture("_BlurredColor", (Texture) temporary2);
          this.dx11bokehMaterial.SetFloat("_SpawnHeuristic", this.dx11SpawnHeuristic);
          this.dx11bokehMaterial.SetVector("_BokehParams", new Vector4(this.dx11BokehScale, this.dx11BokehIntensity, Mathf.Clamp(this.dx11BokehThreshold, 0.005f, 4f), this.internalBlurWidth));
          this.dx11bokehMaterial.SetTexture("_FgCocMask", this.nearBlur ? (Texture) temporary3 : (Texture) null);
          Graphics.SetRandomWriteTarget(1, this.cbPoints);
          Graphics.Blit((Texture) source, renderTexture1, this.dx11bokehMaterial, 0);
          Graphics.ClearRandomWriteTargets();
          if (this.nearBlur)
          {
            this.dofHdrMaterial.SetVector("_Offsets", new Vector4(0.0f, num2, 0.0f, num2));
            Graphics.Blit((Texture) temporary3, temporary2, this.dofHdrMaterial, 2);
            this.dofHdrMaterial.SetVector("_Offsets", new Vector4(num2, 0.0f, 0.0f, num2));
            Graphics.Blit((Texture) temporary2, temporary3, this.dofHdrMaterial, 2);
            Graphics.Blit((Texture) temporary3, renderTexture1, this.dofHdrMaterial, 3);
          }
          Graphics.Blit((Texture) renderTexture1, temporary1, this.dofHdrMaterial, 20);
          this.dofHdrMaterial.SetVector("_Offsets", new Vector4(this.internalBlurWidth, 0.0f, 0.0f, this.internalBlurWidth));
          Graphics.Blit((Texture) renderTexture1, source, this.dofHdrMaterial, 5);
          this.dofHdrMaterial.SetVector("_Offsets", new Vector4(0.0f, this.internalBlurWidth, 0.0f, this.internalBlurWidth));
          Graphics.Blit((Texture) source, temporary1, this.dofHdrMaterial, 21);
          Graphics.SetRenderTarget(temporary1);
          ComputeBuffer.CopyCount(this.cbPoints, this.cbDrawArgs, 0);
          this.dx11bokehMaterial.SetBuffer("pointBuffer", this.cbPoints);
          this.dx11bokehMaterial.SetTexture("_MainTex", (Texture) this.dx11BokehTexture);
          this.dx11bokehMaterial.SetVector("_Screen", (Vector4) new Vector3((float) (1.0 / (1.0 * (double) source.width)), (float) (1.0 / (1.0 * (double) source.height)), this.internalBlurWidth));
          this.dx11bokehMaterial.SetPass(2);
          Graphics.DrawProceduralIndirectNow(MeshTopology.Points, this.cbDrawArgs);
          Graphics.Blit((Texture) temporary1, destination);
          RenderTexture.ReleaseTemporary(temporary1);
          RenderTexture.ReleaseTemporary(temporary2);
          RenderTexture.ReleaseTemporary(temporary3);
        }
        else
        {
          renderTexture1 = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format);
          renderTexture2 = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format);
          float num3 = this.internalBlurWidth * this.foregroundOverlap;
          this.WriteCoc(source, false);
          source.filterMode = FilterMode.Bilinear;
          Graphics.Blit((Texture) source, renderTexture1, this.dofHdrMaterial, 6);
          RenderTexture temporary4 = RenderTexture.GetTemporary(renderTexture1.width >> 1, renderTexture1.height >> 1, 0, renderTexture1.format);
          RenderTexture temporary5 = RenderTexture.GetTemporary(renderTexture1.width >> 1, renderTexture1.height >> 1, 0, renderTexture1.format);
          Graphics.Blit((Texture) renderTexture1, temporary4, this.dofHdrMaterial, 15);
          this.dofHdrMaterial.SetVector("_Offsets", new Vector4(0.0f, 1.5f, 0.0f, 1.5f));
          Graphics.Blit((Texture) temporary4, temporary5, this.dofHdrMaterial, 19);
          this.dofHdrMaterial.SetVector("_Offsets", new Vector4(1.5f, 0.0f, 0.0f, 1.5f));
          Graphics.Blit((Texture) temporary5, temporary4, this.dofHdrMaterial, 19);
          RenderTexture renderTexture3 = (RenderTexture) null;
          if (this.nearBlur)
          {
            renderTexture3 = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format);
            Graphics.Blit((Texture) source, renderTexture3, this.dofHdrMaterial, 4);
          }
          this.dx11bokehMaterial.SetTexture("_BlurredColor", (Texture) temporary4);
          this.dx11bokehMaterial.SetFloat("_SpawnHeuristic", this.dx11SpawnHeuristic);
          this.dx11bokehMaterial.SetVector("_BokehParams", new Vector4(this.dx11BokehScale, this.dx11BokehIntensity, Mathf.Clamp(this.dx11BokehThreshold, 0.005f, 4f), this.internalBlurWidth));
          this.dx11bokehMaterial.SetTexture("_FgCocMask", (Texture) renderTexture3);
          Graphics.SetRandomWriteTarget(1, this.cbPoints);
          Graphics.Blit((Texture) renderTexture1, renderTexture2, this.dx11bokehMaterial, 0);
          Graphics.ClearRandomWriteTargets();
          RenderTexture.ReleaseTemporary(temporary4);
          RenderTexture.ReleaseTemporary(temporary5);
          if (this.nearBlur)
          {
            this.dofHdrMaterial.SetVector("_Offsets", new Vector4(0.0f, num3, 0.0f, num3));
            Graphics.Blit((Texture) renderTexture3, renderTexture1, this.dofHdrMaterial, 2);
            this.dofHdrMaterial.SetVector("_Offsets", new Vector4(num3, 0.0f, 0.0f, num3));
            Graphics.Blit((Texture) renderTexture1, renderTexture3, this.dofHdrMaterial, 2);
            Graphics.Blit((Texture) renderTexture3, renderTexture2, this.dofHdrMaterial, 3);
          }
          this.dofHdrMaterial.SetVector("_Offsets", new Vector4(this.internalBlurWidth, 0.0f, 0.0f, this.internalBlurWidth));
          Graphics.Blit((Texture) renderTexture2, renderTexture1, this.dofHdrMaterial, 5);
          this.dofHdrMaterial.SetVector("_Offsets", new Vector4(0.0f, this.internalBlurWidth, 0.0f, this.internalBlurWidth));
          Graphics.Blit((Texture) renderTexture1, renderTexture2, this.dofHdrMaterial, 5);
          Graphics.SetRenderTarget(renderTexture2);
          ComputeBuffer.CopyCount(this.cbPoints, this.cbDrawArgs, 0);
          this.dx11bokehMaterial.SetBuffer("pointBuffer", this.cbPoints);
          this.dx11bokehMaterial.SetTexture("_MainTex", (Texture) this.dx11BokehTexture);
          this.dx11bokehMaterial.SetVector("_Screen", (Vector4) new Vector3((float) (1.0 / (1.0 * (double) renderTexture2.width)), (float) (1.0 / (1.0 * (double) renderTexture2.height)), this.internalBlurWidth));
          this.dx11bokehMaterial.SetPass(1);
          Graphics.DrawProceduralIndirectNow(MeshTopology.Points, this.cbDrawArgs);
          this.dofHdrMaterial.SetTexture("_LowRez", (Texture) renderTexture2);
          this.dofHdrMaterial.SetTexture("_FgOverlap", (Texture) renderTexture3);
          this.dofHdrMaterial.SetVector("_Offsets", (float) (1.0 * (double) source.width / (1.0 * (double) renderTexture2.width)) * this.internalBlurWidth * Vector4.one);
          Graphics.Blit((Texture) source, destination, this.dofHdrMaterial, 9);
          if ((bool) (UnityEngine.Object) renderTexture3)
            RenderTexture.ReleaseTemporary(renderTexture3);
        }
      }
      else
      {
        source.filterMode = FilterMode.Bilinear;
        if (this.highResolution)
          this.internalBlurWidth *= 2f;
        this.WriteCoc(source, true);
        renderTexture1 = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format);
        renderTexture2 = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format);
        int pass = this.blurSampleCount == DepthOfField.BlurSampleCount.High || this.blurSampleCount == DepthOfField.BlurSampleCount.Medium ? 17 : 11;
        if (this.highResolution)
        {
          this.dofHdrMaterial.SetVector("_Offsets", new Vector4(0.0f, this.internalBlurWidth, 0.025f, this.internalBlurWidth));
          Graphics.Blit((Texture) source, destination, this.dofHdrMaterial, pass);
        }
        else
        {
          this.dofHdrMaterial.SetVector("_Offsets", new Vector4(0.0f, this.internalBlurWidth, 0.1f, this.internalBlurWidth));
          Graphics.Blit((Texture) source, renderTexture1, this.dofHdrMaterial, 6);
          Graphics.Blit((Texture) renderTexture1, renderTexture2, this.dofHdrMaterial, pass);
          this.dofHdrMaterial.SetTexture("_LowRez", (Texture) renderTexture2);
          this.dofHdrMaterial.SetTexture("_FgOverlap", (Texture) null);
          this.dofHdrMaterial.SetVector("_Offsets", Vector4.one * (float) (1.0 * (double) source.width / (1.0 * (double) renderTexture2.width)) * this.internalBlurWidth);
          Graphics.Blit((Texture) source, destination, this.dofHdrMaterial, this.blurSampleCount == DepthOfField.BlurSampleCount.High ? 18 : 12);
        }
      }
      if ((bool) (UnityEngine.Object) renderTexture1)
        RenderTexture.ReleaseTemporary(renderTexture1);
      if (!(bool) (UnityEngine.Object) renderTexture2)
        return;
      RenderTexture.ReleaseTemporary(renderTexture2);
    }
  }

  public enum BlurType
  {
    DiscBlur,
    DX11,
  }

  public enum BlurSampleCount
  {
    Low,
    Medium,
    High,
  }
}
