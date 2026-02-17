// Decompiled with JetBrains decompiler
// Type: Kino.DigitalGlitch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Kino;

[ExecuteInEditMode]
public class DigitalGlitch : MonoBehaviour
{
  [SerializeField]
  [Range(0.0f, 1f)]
  public float _intensity;
  [SerializeField]
  public Shader _shader;
  public Material _material;
  public Texture2D _noiseTexture;
  public RenderTexture _trashFrame1;
  public RenderTexture _trashFrame2;

  public float intensity
  {
    get => this._intensity;
    set => this._intensity = value;
  }

  public static Color RandomColor()
  {
    return new Color(Random.value, Random.value, Random.value, Random.value);
  }

  public void SetUpResources()
  {
    if ((Object) this._material != (Object) null)
      return;
    this._material = new Material(this._shader);
    this._material.hideFlags = HideFlags.DontSave;
    this._noiseTexture = new Texture2D(64 /*0x40*/, 32 /*0x20*/, TextureFormat.ARGB32, false);
    this._noiseTexture.hideFlags = HideFlags.DontSave;
    this._noiseTexture.wrapMode = TextureWrapMode.Clamp;
    this._noiseTexture.filterMode = FilterMode.Point;
    this._trashFrame1 = new RenderTexture(Screen.width, Screen.height, 0);
    this._trashFrame2 = new RenderTexture(Screen.width, Screen.height, 0);
    this._trashFrame1.hideFlags = HideFlags.DontSave;
    this._trashFrame2.hideFlags = HideFlags.DontSave;
    this.UpdateNoiseTexture();
  }

  public void UpdateNoiseTexture()
  {
    Color color = DigitalGlitch.RandomColor();
    for (int y = 0; y < this._noiseTexture.height; ++y)
    {
      for (int x = 0; x < this._noiseTexture.width; ++x)
      {
        if ((double) Random.value > 0.88999998569488525)
          color = DigitalGlitch.RandomColor();
        this._noiseTexture.SetPixel(x, y, color);
      }
    }
    this._noiseTexture.Apply();
  }

  public void Update()
  {
    if ((double) Random.value <= (double) Mathf.Lerp(0.9f, 0.5f, this._intensity))
      return;
    this.SetUpResources();
    this.UpdateNoiseTexture();
  }

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    this.SetUpResources();
    int frameCount = Time.frameCount;
    if (frameCount % 13 == 0)
      Graphics.Blit((Texture) source, this._trashFrame1);
    if (frameCount % 73 == 0)
      Graphics.Blit((Texture) source, this._trashFrame2);
    this._material.SetFloat("_Intensity", this._intensity);
    this._material.SetTexture("_NoiseTex", (Texture) this._noiseTexture);
    this._material.SetTexture("_TrashTex", (double) Random.value > 0.5 ? (Texture) this._trashFrame1 : (Texture) this._trashFrame2);
    Graphics.Blit((Texture) source, destination, this._material);
  }
}
