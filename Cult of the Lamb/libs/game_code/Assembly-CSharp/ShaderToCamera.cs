// Decompiled with JetBrains decompiler
// Type: ShaderToCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class ShaderToCamera : BaseMonoBehaviour
{
  public Material material;
  [Range(0.0f, 1f)]
  public float materialFloat = 1f;
  public string materialString;

  public void Update()
  {
    if (!(this.materialString != ""))
      return;
    this.material.SetFloat(this.materialString, this.materialFloat);
  }

  public void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
  {
    if (!((Object) this.material != (Object) null))
      return;
    Graphics.Blit((Texture) sourceTexture, destTexture, this.material);
  }
}
