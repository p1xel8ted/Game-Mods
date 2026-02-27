// Decompiled with JetBrains decompiler
// Type: ShaderToCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class ShaderToCamera : BaseMonoBehaviour
{
  public Material material;
  [Range(0.0f, 1f)]
  public float materialFloat = 1f;
  public string materialString;

  private void Update()
  {
    if (!(this.materialString != ""))
      return;
    this.material.SetFloat(this.materialString, this.materialFloat);
  }

  private void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
  {
    if (!((Object) this.material != (Object) null))
      return;
    Graphics.Blit((Texture) sourceTexture, destTexture, this.material);
  }
}
