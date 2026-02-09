// Decompiled with JetBrains decompiler
// Type: Kino.Tube
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Kino;

[ExecuteInEditMode]
public class Tube : MonoBehaviour
{
  [SerializeField]
  [Range(0.0f, 1f)]
  public float _bleeding = 0.5f;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float _fringing = 0.5f;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float _scanline = 0.5f;
  [SerializeField]
  public Shader _shader;
  public Material _material;

  public float bleeding
  {
    get => this._bleeding;
    set => this._bleeding = value;
  }

  public float fringing
  {
    get => this._fringing;
    set => this._fringing = value;
  }

  public float scanline
  {
    get => this._scanline;
    set => this._scanline = value;
  }

  public void OnDestroy()
  {
    if (!((Object) this._material == (Object) null))
      return;
    if (Application.isPlaying)
      Object.Destroy((Object) this._material);
    else
      Object.DestroyImmediate((Object) this._material);
  }

  public void OnRenderImage(RenderTexture source, RenderTexture dest)
  {
    if ((Object) this._material == (Object) null)
    {
      this._material = new Material(this._shader);
      this._material.hideFlags = HideFlags.DontSave;
    }
    double num1 = 0.039999999105930328 * (double) this._bleeding;
    int num2 = Mathf.CeilToInt((float) num1 / (2.5f / (float) source.width));
    float num3 = (float) num1 / (float) num2;
    float num4 = 1f / 400f * this._fringing;
    this._material.SetInt("_BleedTaps", num2);
    this._material.SetFloat("_BleedDelta", num3);
    this._material.SetFloat("_FringeDelta", num4);
    this._material.SetFloat("_Scanline", this._scanline);
    Graphics.Blit((Texture) source, dest, this._material, 0);
  }
}
