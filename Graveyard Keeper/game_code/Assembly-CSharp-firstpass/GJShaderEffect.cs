// Decompiled with JetBrains decompiler
// Type: GJShaderEffect
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
public class GJShaderEffect : MonoBehaviour
{
  public Shader shader;
  public Material _material;

  public Material material
  {
    get
    {
      if ((Object) this._material == (Object) null)
      {
        this._material = new Material(this.shader);
        this._material.hideFlags = HideFlags.HideAndDontSave;
      }
      return this._material;
    }
  }

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if ((Object) this.shader == (Object) null)
      return;
    Material material = this.material;
    this.SetValues(material);
    Graphics.Blit((Texture) source, destination, material);
  }

  public virtual void SetValues(Material mat)
  {
  }

  public void OnDisable()
  {
    if (!(bool) (Object) this._material)
      return;
    Object.DestroyImmediate((Object) this._material);
  }
}
