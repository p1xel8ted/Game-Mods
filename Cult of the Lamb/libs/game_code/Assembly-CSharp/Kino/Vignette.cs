// Decompiled with JetBrains decompiler
// Type: Kino.Vignette
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Kino;

[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
[AddComponentMenu("Kino Image Effects/Vignette")]
public class Vignette : MonoBehaviour
{
  [SerializeField]
  [Range(0.0f, 1f)]
  public float _falloff = 0.5f;
  [SerializeField]
  public Shader _shader;
  public Material _material;

  public float intensity
  {
    get => this._falloff;
    set => this._falloff = value;
  }

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if ((Object) this._material == (Object) null)
    {
      this._material = new Material(this._shader);
      this._material.hideFlags = HideFlags.DontSave;
    }
    this._material.SetVector("_Aspect", (Vector4) new Vector2(this.GetComponent<Camera>().aspect, 1f));
    this._material.SetFloat("_Falloff", this._falloff);
    Graphics.Blit((Texture) source, destination, this._material, 0);
  }
}
