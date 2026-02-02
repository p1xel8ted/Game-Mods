// Decompiled with JetBrains decompiler
// Type: Kino.Datamosh
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Kino;

public class Datamosh : MonoBehaviour
{
  [SerializeField]
  [Tooltip("Size of compression macroblock.")]
  public int _blockSize = 16 /*0x10*/;
  [SerializeField]
  [Range(0.0f, 1f)]
  [Tooltip("Entropy coefficient. The larger value makes the stronger noise.")]
  public float _entropy = 0.5f;
  [SerializeField]
  [Range(0.5f, 4f)]
  [Tooltip("Contrast of stripe-shaped noise.")]
  public float _noiseContrast = 1f;
  [SerializeField]
  [Range(0.0f, 2f)]
  [Tooltip("Scale factor for velocity vectors.")]
  public float _velocityScale = 0.8f;
  [SerializeField]
  [Range(0.0f, 2f)]
  [Tooltip("Amount of random displacement.")]
  public float _diffusion = 0.4f;
  [SerializeField]
  public Shader _shader;
  public Material _material;
  public RenderTexture _workBuffer;
  public RenderTexture _dispBuffer;
  public int _sequence;
  public int _lastFrame;

  public int blockSize
  {
    get => Mathf.Max(4, this._blockSize);
    set => this._blockSize = value;
  }

  public float entropy
  {
    get => this._entropy;
    set => this._entropy = value;
  }

  public float noiseContrast
  {
    get => this._noiseContrast;
    set => this._noiseContrast = value;
  }

  public float velocityScale
  {
    get => this._velocityScale;
    set => this._velocityScale = value;
  }

  public float diffusion
  {
    get => this._diffusion;
    set => this._diffusion = value;
  }

  public void Glitch() => this._sequence = 1;

  public void Reset() => this._sequence = 0;

  public RenderTexture NewWorkBuffer(RenderTexture source)
  {
    return RenderTexture.GetTemporary(source.width, source.height);
  }

  public RenderTexture NewDispBuffer(RenderTexture source)
  {
    RenderTexture temporary = RenderTexture.GetTemporary(source.width / this._blockSize, source.height / this._blockSize, 0, RenderTextureFormat.ARGBHalf);
    temporary.filterMode = FilterMode.Point;
    return temporary;
  }

  public void ReleaseBuffer(RenderTexture buffer)
  {
    if (!((Object) buffer != (Object) null))
      return;
    RenderTexture.ReleaseTemporary(buffer);
  }

  public void OnEnable()
  {
    this._material = new Material(Shader.Find("Hidden/Kino/Datamosh"));
    this._material.hideFlags = HideFlags.DontSave;
    this.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth | DepthTextureMode.MotionVectors;
    this._sequence = 0;
  }

  public void OnDisable()
  {
    this.ReleaseBuffer(this._workBuffer);
    this._workBuffer = (RenderTexture) null;
    this.ReleaseBuffer(this._dispBuffer);
    this._dispBuffer = (RenderTexture) null;
    Object.DestroyImmediate((Object) this._material);
    this._material = (Material) null;
  }

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    this._material.SetFloat("_BlockSize", (float) this._blockSize);
    this._material.SetFloat("_Quality", 1f - this._entropy);
    this._material.SetFloat("_Contrast", this._noiseContrast);
    this._material.SetFloat("_Velocity", this._velocityScale);
    this._material.SetFloat("_Diffusion", this._diffusion);
    if (this._sequence == 0)
    {
      this.ReleaseBuffer(this._workBuffer);
      this._workBuffer = this.NewWorkBuffer(source);
      Graphics.Blit((Texture) source, this._workBuffer);
      Graphics.Blit((Texture) source, destination);
    }
    else if (this._sequence == 1)
    {
      this.ReleaseBuffer(this._dispBuffer);
      this._dispBuffer = this.NewDispBuffer(source);
      Graphics.Blit((Texture) null, this._dispBuffer, this._material, 0);
      Graphics.Blit((Texture) this._workBuffer, destination);
      ++this._sequence;
    }
    else
    {
      if (Time.frameCount != this._lastFrame)
      {
        RenderTexture dest1 = this.NewDispBuffer(source);
        Graphics.Blit((Texture) this._dispBuffer, dest1, this._material, 1);
        this.ReleaseBuffer(this._dispBuffer);
        this._dispBuffer = dest1;
        RenderTexture dest2 = this.NewWorkBuffer(source);
        this._material.SetTexture("_WorkTex", (Texture) this._workBuffer);
        this._material.SetTexture("_DispTex", (Texture) this._dispBuffer);
        Graphics.Blit((Texture) source, dest2, this._material, 2);
        this.ReleaseBuffer(this._workBuffer);
        this._workBuffer = dest2;
        this._lastFrame = Time.frameCount;
      }
      Graphics.Blit((Texture) this._workBuffer, destination);
    }
  }
}
