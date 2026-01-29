// Decompiled with JetBrains decompiler
// Type: Kino.Bloom
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Kino;

[ExecuteInEditMode]
[AddComponentMenu("Kino Image Effects/Bloom")]
public class Bloom : MonoBehaviour
{
  [SerializeField]
  [Tooltip("Filters out pixels under this level of brightness.")]
  public float _threshold = 0.8f;
  [SerializeField]
  [Range(0.0f, 1f)]
  [Tooltip("Makes transition between under/over-threshold gradual.")]
  public float _softKnee = 0.5f;
  [SerializeField]
  [Range(1f, 7f)]
  [Tooltip("Changes extent of veiling effects\nin a screen resolution-independent fashion.")]
  public float _radius = 2.5f;
  [SerializeField]
  [Tooltip("Blend factor of the result image.")]
  public float _intensity = 0.8f;
  [SerializeField]
  [Tooltip("Controls filter quality and buffer resolution.")]
  public bool _highQuality = true;
  [SerializeField]
  [Tooltip("Reduces flashing noise with an additional filter.")]
  public bool _antiFlicker = true;
  [SerializeField]
  [HideInInspector]
  public Shader _shader;
  public Material _material;
  public const int kMaxIterations = 16 /*0x10*/;
  public RenderTexture[] _blurBuffer1 = new RenderTexture[16 /*0x10*/];
  public RenderTexture[] _blurBuffer2 = new RenderTexture[16 /*0x10*/];

  public float thresholdGamma
  {
    get => Mathf.Max(this._threshold, 0.0f);
    set => this._threshold = value;
  }

  public float thresholdLinear
  {
    get => this.GammaToLinear(this.thresholdGamma);
    set => this._threshold = this.LinearToGamma(value);
  }

  public float softKnee
  {
    get => this._softKnee;
    set => this._softKnee = value;
  }

  public float radius
  {
    get => this._radius;
    set => this._radius = value;
  }

  public float intensity
  {
    get => Mathf.Max(this._intensity, 0.0f);
    set => this._intensity = value;
  }

  public bool highQuality
  {
    get => this._highQuality;
    set => this._highQuality = value;
  }

  public bool antiFlicker
  {
    get => this._antiFlicker;
    set => this._antiFlicker = value;
  }

  public float LinearToGamma(float x) => Mathf.LinearToGammaSpace(x);

  public float GammaToLinear(float x) => Mathf.GammaToLinearSpace(x);

  public void OnEnable()
  {
    this._material = new Material((bool) (Object) this._shader ? this._shader : Shader.Find("Hidden/Kino/Bloom"));
    this._material.hideFlags = HideFlags.DontSave;
  }

  public void OnDisable() => Object.DestroyImmediate((Object) this._material);

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    int num1 = Application.isMobilePlatform ? 1 : 0;
    int width = source.width;
    int height = source.height;
    if (!this._highQuality)
    {
      width /= 2;
      height /= 2;
    }
    RenderTextureFormat format = num1 != 0 ? RenderTextureFormat.Default : RenderTextureFormat.DefaultHDR;
    float num2 = (float) ((double) Mathf.Log((float) height, 2f) + (double) this._radius - 8.0);
    int num3 = (int) num2;
    int num4 = Mathf.Clamp(num3, 1, 16 /*0x10*/);
    float thresholdLinear = this.thresholdLinear;
    this._material.SetFloat("_Threshold", thresholdLinear);
    float num5 = (float) ((double) thresholdLinear * (double) this._softKnee + 9.9999997473787516E-06);
    this._material.SetVector("_Curve", (Vector4) new Vector3(thresholdLinear - num5, num5 * 2f, 0.25f / num5));
    this._material.SetFloat("_PrefilterOffs", !this._highQuality && this._antiFlicker ? -0.5f : 0.0f);
    this._material.SetFloat("_SampleScale", 0.5f + num2 - (float) num3);
    this._material.SetFloat("_Intensity", this.intensity);
    RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, format);
    int pass1 = this._antiFlicker ? 1 : 0;
    Graphics.Blit((Texture) source, temporary, this._material, pass1);
    RenderTexture source1 = temporary;
    for (int index = 0; index < num4; ++index)
    {
      this._blurBuffer1[index] = RenderTexture.GetTemporary(source1.width / 2, source1.height / 2, 0, format);
      int pass2 = index == 0 ? (this._antiFlicker ? 3 : 2) : 4;
      Graphics.Blit((Texture) source1, this._blurBuffer1[index], this._material, pass2);
      source1 = this._blurBuffer1[index];
    }
    for (int index = num4 - 2; index >= 0; --index)
    {
      RenderTexture renderTexture = this._blurBuffer1[index];
      this._material.SetTexture("_BaseTex", (Texture) renderTexture);
      this._blurBuffer2[index] = RenderTexture.GetTemporary(renderTexture.width, renderTexture.height, 0, format);
      int pass3 = this._highQuality ? 6 : 5;
      Graphics.Blit((Texture) source1, this._blurBuffer2[index], this._material, pass3);
      source1 = this._blurBuffer2[index];
    }
    this._material.SetTexture("_BaseTex", (Texture) source);
    int pass4 = this._highQuality ? 8 : 7;
    Graphics.Blit((Texture) source1, destination, this._material, pass4);
    for (int index = 0; index < 16 /*0x10*/; ++index)
    {
      if ((Object) this._blurBuffer1[index] != (Object) null)
        RenderTexture.ReleaseTemporary(this._blurBuffer1[index]);
      if ((Object) this._blurBuffer2[index] != (Object) null)
        RenderTexture.ReleaseTemporary(this._blurBuffer2[index]);
      this._blurBuffer1[index] = (RenderTexture) null;
      this._blurBuffer2[index] = (RenderTexture) null;
    }
    RenderTexture.ReleaseTemporary(temporary);
  }
}
