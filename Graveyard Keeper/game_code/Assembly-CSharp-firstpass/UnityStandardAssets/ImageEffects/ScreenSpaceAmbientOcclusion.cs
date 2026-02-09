// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.ImageEffects.ScreenSpaceAmbientOcclusion
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

#nullable disable
namespace UnityStandardAssets.ImageEffects;

[AddComponentMenu("Image Effects/Rendering/Screen Space Ambient Occlusion")]
[RequireComponent(typeof (Camera))]
[ExecuteInEditMode]
public class ScreenSpaceAmbientOcclusion : MonoBehaviour
{
  [Range(0.05f, 1f)]
  public float m_Radius = 0.4f;
  public ScreenSpaceAmbientOcclusion.SSAOSamples m_SampleCount = ScreenSpaceAmbientOcclusion.SSAOSamples.Medium;
  [Range(0.5f, 4f)]
  public float m_OcclusionIntensity = 1.5f;
  [Range(0.0f, 4f)]
  public int m_Blur = 2;
  [Range(1f, 6f)]
  public int m_Downsampling = 2;
  [Range(0.2f, 2f)]
  public float m_OcclusionAttenuation = 1f;
  [Range(1E-05f, 0.5f)]
  public float m_MinZ = 0.01f;
  public Shader m_SSAOShader;
  public Material m_SSAOMaterial;
  public Texture2D m_RandomTexture;
  public bool m_Supported;

  public static Material CreateMaterial(Shader shader)
  {
    if (!(bool) (UnityEngine.Object) shader)
      return (Material) null;
    Material material = new Material(shader);
    material.hideFlags = HideFlags.HideAndDontSave;
    return material;
  }

  public static void DestroyMaterial(Material mat)
  {
    if (!(bool) (UnityEngine.Object) mat)
      return;
    UnityEngine.Object.DestroyImmediate((UnityEngine.Object) mat);
    mat = (Material) null;
  }

  public void OnDisable() => ScreenSpaceAmbientOcclusion.DestroyMaterial(this.m_SSAOMaterial);

  public void Start()
  {
    if (!SystemInfo.supportsImageEffects || !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
    {
      this.m_Supported = false;
      this.enabled = false;
    }
    else
    {
      this.CreateMaterials();
      if (!(bool) (UnityEngine.Object) this.m_SSAOMaterial || this.m_SSAOMaterial.passCount != 5)
      {
        this.m_Supported = false;
        this.enabled = false;
      }
      else
        this.m_Supported = true;
    }
  }

  public void OnEnable()
  {
    this.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.DepthNormals;
  }

  public void CreateMaterials()
  {
    if ((bool) (UnityEngine.Object) this.m_SSAOMaterial || !this.m_SSAOShader.isSupported)
      return;
    this.m_SSAOMaterial = ScreenSpaceAmbientOcclusion.CreateMaterial(this.m_SSAOShader);
    this.m_SSAOMaterial.SetTexture("_RandomTexture", (Texture) this.m_RandomTexture);
  }

  [ImageEffectOpaque]
  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (!this.m_Supported || !this.m_SSAOShader.isSupported)
    {
      this.enabled = false;
    }
    else
    {
      this.CreateMaterials();
      this.m_Downsampling = Mathf.Clamp(this.m_Downsampling, 1, 6);
      this.m_Radius = Mathf.Clamp(this.m_Radius, 0.05f, 1f);
      this.m_MinZ = Mathf.Clamp(this.m_MinZ, 1E-05f, 0.5f);
      this.m_OcclusionIntensity = Mathf.Clamp(this.m_OcclusionIntensity, 0.5f, 4f);
      this.m_OcclusionAttenuation = Mathf.Clamp(this.m_OcclusionAttenuation, 0.2f, 2f);
      this.m_Blur = Mathf.Clamp(this.m_Blur, 0, 4);
      RenderTexture renderTexture = RenderTexture.GetTemporary(source.width / this.m_Downsampling, source.height / this.m_Downsampling, 0);
      double fieldOfView = (double) this.GetComponent<Camera>().fieldOfView;
      float farClipPlane = this.GetComponent<Camera>().farClipPlane;
      float y = Mathf.Tan((float) (fieldOfView * (Math.PI / 180.0) * 0.5)) * farClipPlane;
      this.m_SSAOMaterial.SetVector("_FarCorner", (Vector4) new Vector3(y * this.GetComponent<Camera>().aspect, y, farClipPlane));
      int num1;
      int num2;
      if ((bool) (UnityEngine.Object) this.m_RandomTexture)
      {
        num1 = this.m_RandomTexture.width;
        num2 = this.m_RandomTexture.height;
      }
      else
      {
        num1 = 1;
        num2 = 1;
      }
      this.m_SSAOMaterial.SetVector("_NoiseScale", (Vector4) new Vector3((float) renderTexture.width / (float) num1, (float) renderTexture.height / (float) num2, 0.0f));
      this.m_SSAOMaterial.SetVector("_Params", new Vector4(this.m_Radius, this.m_MinZ, 1f / this.m_OcclusionAttenuation, this.m_OcclusionIntensity));
      int num3 = this.m_Blur > 0 ? 1 : 0;
      Graphics.Blit(num3 != 0 ? (Texture) null : (Texture) source, renderTexture, this.m_SSAOMaterial, (int) this.m_SampleCount);
      if (num3 != 0)
      {
        RenderTexture temporary1 = RenderTexture.GetTemporary(source.width, source.height, 0);
        this.m_SSAOMaterial.SetVector("_TexelOffsetScale", new Vector4((float) this.m_Blur / (float) source.width, 0.0f, 0.0f, 0.0f));
        this.m_SSAOMaterial.SetTexture("_SSAO", (Texture) renderTexture);
        Graphics.Blit((Texture) null, temporary1, this.m_SSAOMaterial, 3);
        RenderTexture.ReleaseTemporary(renderTexture);
        RenderTexture temporary2 = RenderTexture.GetTemporary(source.width, source.height, 0);
        this.m_SSAOMaterial.SetVector("_TexelOffsetScale", new Vector4(0.0f, (float) this.m_Blur / (float) source.height, 0.0f, 0.0f));
        this.m_SSAOMaterial.SetTexture("_SSAO", (Texture) temporary1);
        Graphics.Blit((Texture) source, temporary2, this.m_SSAOMaterial, 3);
        RenderTexture.ReleaseTemporary(temporary1);
        renderTexture = temporary2;
      }
      this.m_SSAOMaterial.SetTexture("_SSAO", (Texture) renderTexture);
      Graphics.Blit((Texture) source, destination, this.m_SSAOMaterial, 4);
      RenderTexture.ReleaseTemporary(renderTexture);
    }
  }

  public enum SSAOSamples
  {
    Low,
    Medium,
    High,
  }
}
