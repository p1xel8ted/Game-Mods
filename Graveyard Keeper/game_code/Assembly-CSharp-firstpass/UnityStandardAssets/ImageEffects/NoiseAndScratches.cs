// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.ImageEffects.NoiseAndScratches
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace UnityStandardAssets.ImageEffects;

[RequireComponent(typeof (Camera))]
[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Noise/Noise and Scratches")]
public class NoiseAndScratches : MonoBehaviour
{
  public bool monochrome = true;
  public bool rgbFallback;
  [Range(0.0f, 5f)]
  public float grainIntensityMin = 0.1f;
  [Range(0.0f, 5f)]
  public float grainIntensityMax = 0.2f;
  [Range(0.1f, 50f)]
  public float grainSize = 2f;
  [Range(0.0f, 5f)]
  public float scratchIntensityMin = 0.05f;
  [Range(0.0f, 5f)]
  public float scratchIntensityMax = 0.25f;
  [Range(1f, 30f)]
  public float scratchFPS = 10f;
  [Range(0.0f, 1f)]
  public float scratchJitter = 0.01f;
  public Texture grainTexture;
  public Texture scratchTexture;
  public Shader shaderRGB;
  public Shader shaderYUV;
  public Material m_MaterialRGB;
  public Material m_MaterialYUV;
  public float scratchTimeLeft;
  public float scratchX;
  public float scratchY;

  public void Start()
  {
    if (!SystemInfo.supportsImageEffects)
      this.enabled = false;
    else if ((Object) this.shaderRGB == (Object) null || (Object) this.shaderYUV == (Object) null)
    {
      Debug.Log((object) "Noise shaders are not set up! Disabling noise effect.");
      this.enabled = false;
    }
    else if (!this.shaderRGB.isSupported)
    {
      this.enabled = false;
    }
    else
    {
      if (this.shaderYUV.isSupported)
        return;
      this.rgbFallback = true;
    }
  }

  public Material material
  {
    get
    {
      if ((Object) this.m_MaterialRGB == (Object) null)
      {
        this.m_MaterialRGB = new Material(this.shaderRGB);
        this.m_MaterialRGB.hideFlags = HideFlags.HideAndDontSave;
      }
      if ((Object) this.m_MaterialYUV == (Object) null && !this.rgbFallback)
      {
        this.m_MaterialYUV = new Material(this.shaderYUV);
        this.m_MaterialYUV.hideFlags = HideFlags.HideAndDontSave;
      }
      return this.rgbFallback || this.monochrome ? this.m_MaterialRGB : this.m_MaterialYUV;
    }
  }

  public void OnDisable()
  {
    if ((bool) (Object) this.m_MaterialRGB)
      Object.DestroyImmediate((Object) this.m_MaterialRGB);
    if (!(bool) (Object) this.m_MaterialYUV)
      return;
    Object.DestroyImmediate((Object) this.m_MaterialYUV);
  }

  public void SanitizeParameters()
  {
    this.grainIntensityMin = Mathf.Clamp(this.grainIntensityMin, 0.0f, 5f);
    this.grainIntensityMax = Mathf.Clamp(this.grainIntensityMax, 0.0f, 5f);
    this.scratchIntensityMin = Mathf.Clamp(this.scratchIntensityMin, 0.0f, 5f);
    this.scratchIntensityMax = Mathf.Clamp(this.scratchIntensityMax, 0.0f, 5f);
    this.scratchFPS = Mathf.Clamp(this.scratchFPS, 1f, 30f);
    this.scratchJitter = Mathf.Clamp(this.scratchJitter, 0.0f, 1f);
    this.grainSize = Mathf.Clamp(this.grainSize, 0.1f, 50f);
  }

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    this.SanitizeParameters();
    if ((double) this.scratchTimeLeft <= 0.0)
    {
      this.scratchTimeLeft = Random.value * 2f / this.scratchFPS;
      this.scratchX = Random.value;
      this.scratchY = Random.value;
    }
    this.scratchTimeLeft -= Time.deltaTime;
    Material material = this.material;
    material.SetTexture("_GrainTex", this.grainTexture);
    material.SetTexture("_ScratchTex", this.scratchTexture);
    float num = 1f / this.grainSize;
    material.SetVector("_GrainOffsetScale", new Vector4(Random.value, Random.value, (float) Screen.width / (float) this.grainTexture.width * num, (float) Screen.height / (float) this.grainTexture.height * num));
    material.SetVector("_ScratchOffsetScale", new Vector4(this.scratchX + Random.value * this.scratchJitter, this.scratchY + Random.value * this.scratchJitter, (float) Screen.width / (float) this.scratchTexture.width, (float) Screen.height / (float) this.scratchTexture.height));
    material.SetVector("_Intensity", new Vector4(Random.Range(this.grainIntensityMin, this.grainIntensityMax), Random.Range(this.scratchIntensityMin, this.scratchIntensityMax), 0.0f, 0.0f));
    Graphics.Blit((Texture) source, destination, material);
  }
}
