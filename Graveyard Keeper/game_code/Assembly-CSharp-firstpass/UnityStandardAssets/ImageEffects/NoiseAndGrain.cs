// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.ImageEffects.NoiseAndGrain
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
[AddComponentMenu("Image Effects/Noise/Noise And Grain (Filmic)")]
public class NoiseAndGrain : PostEffectsBase
{
  public float intensityMultiplier = 0.25f;
  public float generalIntensity = 0.5f;
  public float blackIntensity = 1f;
  public float whiteIntensity = 1f;
  public float midGrey = 0.2f;
  public bool dx11Grain;
  public float softness;
  public bool monochrome;
  public Vector3 intensities = new Vector3(1f, 1f, 1f);
  public Vector3 tiling = new Vector3(64f, 64f, 64f);
  public float monochromeTiling = 64f;
  public FilterMode filterMode = FilterMode.Bilinear;
  public Texture2D noiseTexture;
  public Shader noiseShader;
  public Material noiseMaterial;
  public Shader dx11NoiseShader;
  public Material dx11NoiseMaterial;
  public static float TILE_AMOUNT = 64f;

  public override bool CheckResources()
  {
    this.CheckSupport(false);
    this.noiseMaterial = this.CheckShaderAndCreateMaterial(this.noiseShader, this.noiseMaterial);
    if (this.dx11Grain && this.supportDX11)
      this.dx11NoiseMaterial = this.CheckShaderAndCreateMaterial(this.dx11NoiseShader, this.dx11NoiseMaterial);
    if (!this.isSupported)
      this.ReportAutoDisable();
    return this.isSupported;
  }

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (!this.CheckResources() || (Object) null == (Object) this.noiseTexture)
    {
      Graphics.Blit((Texture) source, destination);
      if (!((Object) null == (Object) this.noiseTexture))
        return;
      Debug.LogWarning((object) "Noise & Grain effect failing as noise texture is not assigned. please assign.", (Object) this.transform);
    }
    else
    {
      this.softness = Mathf.Clamp(this.softness, 0.0f, 0.99f);
      if (this.dx11Grain && this.supportDX11)
      {
        this.dx11NoiseMaterial.SetFloat("_DX11NoiseTime", (float) Time.frameCount);
        this.dx11NoiseMaterial.SetTexture("_NoiseTex", (Texture) this.noiseTexture);
        this.dx11NoiseMaterial.SetVector("_NoisePerChannel", (Vector4) (this.monochrome ? Vector3.one : this.intensities));
        this.dx11NoiseMaterial.SetVector("_MidGrey", (Vector4) new Vector3(this.midGrey, (float) (1.0 / (1.0 - (double) this.midGrey)), -1f / this.midGrey));
        this.dx11NoiseMaterial.SetVector("_NoiseAmount", (Vector4) (new Vector3(this.generalIntensity, this.blackIntensity, this.whiteIntensity) * this.intensityMultiplier));
        if ((double) this.softness > (double) Mathf.Epsilon)
        {
          RenderTexture temporary = RenderTexture.GetTemporary((int) ((double) source.width * (1.0 - (double) this.softness)), (int) ((double) source.height * (1.0 - (double) this.softness)));
          NoiseAndGrain.DrawNoiseQuadGrid(source, temporary, this.dx11NoiseMaterial, this.noiseTexture, this.monochrome ? 3 : 2);
          this.dx11NoiseMaterial.SetTexture("_NoiseTex", (Texture) temporary);
          Graphics.Blit((Texture) source, destination, this.dx11NoiseMaterial, 4);
          RenderTexture.ReleaseTemporary(temporary);
        }
        else
          NoiseAndGrain.DrawNoiseQuadGrid(source, destination, this.dx11NoiseMaterial, this.noiseTexture, this.monochrome ? 1 : 0);
      }
      else
      {
        if ((bool) (Object) this.noiseTexture)
        {
          this.noiseTexture.wrapMode = TextureWrapMode.Repeat;
          this.noiseTexture.filterMode = this.filterMode;
        }
        this.noiseMaterial.SetTexture("_NoiseTex", (Texture) this.noiseTexture);
        this.noiseMaterial.SetVector("_NoisePerChannel", (Vector4) (this.monochrome ? Vector3.one : this.intensities));
        this.noiseMaterial.SetVector("_NoiseTilingPerChannel", (Vector4) (this.monochrome ? Vector3.one * this.monochromeTiling : this.tiling));
        this.noiseMaterial.SetVector("_MidGrey", (Vector4) new Vector3(this.midGrey, (float) (1.0 / (1.0 - (double) this.midGrey)), -1f / this.midGrey));
        this.noiseMaterial.SetVector("_NoiseAmount", (Vector4) (new Vector3(this.generalIntensity, this.blackIntensity, this.whiteIntensity) * this.intensityMultiplier));
        if ((double) this.softness > (double) Mathf.Epsilon)
        {
          RenderTexture temporary = RenderTexture.GetTemporary((int) ((double) source.width * (1.0 - (double) this.softness)), (int) ((double) source.height * (1.0 - (double) this.softness)));
          NoiseAndGrain.DrawNoiseQuadGrid(source, temporary, this.noiseMaterial, this.noiseTexture, 2);
          this.noiseMaterial.SetTexture("_NoiseTex", (Texture) temporary);
          Graphics.Blit((Texture) source, destination, this.noiseMaterial, 1);
          RenderTexture.ReleaseTemporary(temporary);
        }
        else
          NoiseAndGrain.DrawNoiseQuadGrid(source, destination, this.noiseMaterial, this.noiseTexture, 0);
      }
    }
  }

  public static void DrawNoiseQuadGrid(
    RenderTexture source,
    RenderTexture dest,
    Material fxMaterial,
    Texture2D noise,
    int passNr)
  {
    RenderTexture.active = dest;
    float num1 = (float) noise.width * 1f;
    float num2 = 1f * (float) source.width / NoiseAndGrain.TILE_AMOUNT;
    fxMaterial.SetTexture("_MainTex", (Texture) source);
    GL.PushMatrix();
    GL.LoadOrtho();
    float num3 = (float) (1.0 * (double) source.width / (1.0 * (double) source.height));
    float num4 = 1f / num2;
    float num5 = num4 * num3;
    float num6 = num1 / ((float) noise.width * 1f);
    fxMaterial.SetPass(passNr);
    GL.Begin(7);
    for (float x1 = 0.0f; (double) x1 < 1.0; x1 += num4)
    {
      for (float y1 = 0.0f; (double) y1 < 1.0; y1 += num5)
      {
        float num7 = Random.Range(0.0f, 1f);
        float num8 = Random.Range(0.0f, 1f);
        float x2 = Mathf.Floor(num7 * num1) / num1;
        float y2 = Mathf.Floor(num8 * num1) / num1;
        float num9 = 1f / num1;
        GL.MultiTexCoord2(0, x2, y2);
        GL.MultiTexCoord2(1, 0.0f, 0.0f);
        GL.Vertex3(x1, y1, 0.1f);
        GL.MultiTexCoord2(0, x2 + num6 * num9, y2);
        GL.MultiTexCoord2(1, 1f, 0.0f);
        GL.Vertex3(x1 + num4, y1, 0.1f);
        GL.MultiTexCoord2(0, x2 + num6 * num9, y2 + num6 * num9);
        GL.MultiTexCoord2(1, 1f, 1f);
        GL.Vertex3(x1 + num4, y1 + num5, 0.1f);
        GL.MultiTexCoord2(0, x2, y2 + num6 * num9);
        GL.MultiTexCoord2(1, 0.0f, 1f);
        GL.Vertex3(x1, y1 + num5, 0.1f);
      }
    }
    GL.End();
    GL.PopMatrix();
  }
}
