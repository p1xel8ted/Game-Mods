// Decompiled with JetBrains decompiler
// Type: Kino.Binary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Kino;

[ExecuteInEditMode]
[AddComponentMenu("Kino Image Effects/Binary")]
public class Binary : MonoBehaviour
{
  [SerializeField]
  public Binary.DitherType _ditherType;
  [SerializeField]
  [Range(1f, 8f)]
  public int _ditherScale = 1;
  [SerializeField]
  public Color _color0 = Color.black;
  [SerializeField]
  public Color _color1 = Color.white;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float _opacity = 1f;
  [SerializeField]
  [HideInInspector]
  public Shader _shader;
  [SerializeField]
  [HideInInspector]
  public Texture2D _bayer2x2Texture;
  [SerializeField]
  [HideInInspector]
  public Texture2D _bayer3x3Texture;
  [SerializeField]
  [HideInInspector]
  public Texture2D _bayer4x4Texture;
  [SerializeField]
  [HideInInspector]
  public Texture2D _bayer8x8Texture;
  [SerializeField]
  [HideInInspector]
  public Texture2D _bnoise64x64Texture;
  public Material _material;

  public Binary.DitherType ditherType
  {
    get => this._ditherType;
    set => this._ditherType = value;
  }

  public int ditherScale
  {
    get => this._ditherScale;
    set => this._ditherScale = value;
  }

  public Color color0
  {
    get => this._color0;
    set => this._color0 = value;
  }

  public Color color1
  {
    get => this._color1;
    set => this._color1 = value;
  }

  public float Opacity
  {
    get => this._opacity;
    set => this._opacity = value;
  }

  public Texture2D DitherTexture
  {
    get
    {
      switch (this._ditherType)
      {
        case Binary.DitherType.Bayer2x2:
          return this._bayer2x2Texture;
        case Binary.DitherType.Bayer3x3:
          return this._bayer3x3Texture;
        case Binary.DitherType.Bayer4x4:
          return this._bayer4x4Texture;
        case Binary.DitherType.Bayer8x8:
          return this._bayer8x8Texture;
        default:
          return this._bnoise64x64Texture;
      }
    }
  }

  public void OnDestroy()
  {
    if (!((Object) this._material != (Object) null))
      return;
    if (Application.isPlaying)
      Object.Destroy((Object) this._material);
    else
      Object.DestroyImmediate((Object) this._material);
  }

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if ((Object) this._material == (Object) null)
    {
      this._material = new Material(this._shader);
      this._material.hideFlags = HideFlags.DontSave;
    }
    this._material.SetTexture("_DitherTex", (Texture) this.DitherTexture);
    this._material.SetFloat("_Scale", (float) this._ditherScale);
    this._material.SetColor("_Color0", this._color0);
    this._material.SetColor("_Color1", this._color1);
    this._material.SetFloat("_Opacity", this._opacity);
    Graphics.Blit((Texture) source, destination, this._material);
  }

  public enum DitherType
  {
    Bayer2x2,
    Bayer3x3,
    Bayer4x4,
    Bayer8x8,
    BlueNoise64x64,
  }
}
