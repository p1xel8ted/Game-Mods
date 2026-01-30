// Decompiled with JetBrains decompiler
// Type: Kino.Mirror
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace Kino;

[ExecuteInEditMode]
[AddComponentMenu("Kino Image Effects/Mirror")]
public class Mirror : MonoBehaviour
{
  [SerializeField]
  public int _repeat;
  [SerializeField]
  public float _offset;
  [SerializeField]
  public float _roll;
  [SerializeField]
  public bool _symmetry;
  [SerializeField]
  public Shader _shader;
  public Material _material;

  public int repeat
  {
    get => this._repeat;
    set => this._repeat = value;
  }

  public float offset
  {
    get => this._offset;
    set => this._offset = value;
  }

  public float roll
  {
    get => this._roll;
    set => this._roll = value;
  }

  public bool symmetry
  {
    get => this._symmetry;
    set => this._symmetry = value;
  }

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if ((UnityEngine.Object) this._material == (UnityEngine.Object) null)
    {
      this._material = new Material(this._shader);
      this._material.hideFlags = HideFlags.DontSave;
    }
    this._material.SetFloat("_Divisor", 6.28318548f / (float) Mathf.Max(1, this._repeat));
    this._material.SetFloat("_Offset", this._offset * ((float) Math.PI / 180f));
    this._material.SetFloat("_Roll", this._roll * ((float) Math.PI / 180f));
    if (this._symmetry)
      this._material.EnableKeyword("SYMMETRY_ON");
    else
      this._material.DisableKeyword("SYMMETRY_ON");
    Graphics.Blit((Texture) source, destination, this._material);
  }
}
