// Decompiled with JetBrains decompiler
// Type: MooshTextures
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class MooshTextures : BaseMonoBehaviour
{
  public Material material;
  public float lerpValue = 0.5f;
  public RenderTexture renderTextureA;
  public RenderTexture renderTextureB;
  public RenderTexture renderTextureC;
  public AmplifyColorBase amplifoo;

  public IEnumerator Start()
  {
    while ((Object) this.renderTextureA == (Object) null)
    {
      this.renderTextureA = this.amplifoo.MaskTexture;
      yield return (object) null;
    }
    this.renderTextureA = this.amplifoo.MaskTexture;
    this.material.SetTexture("_RenderTex_1", (Texture) this.renderTextureA);
    this.material.SetTexture("_RenderTex_2", (Texture) this.renderTextureB);
    this.material.SetFloat("_Lerp_Fade_1", this.lerpValue);
    yield return (object) null;
  }

  public void OnPreRender()
  {
    if ((Object) this.renderTextureA == (Object) null)
    {
      this.renderTextureA = this.amplifoo.MaskTexture;
      this.material.SetTexture("_RenderTex_1", (Texture) this.renderTextureA);
    }
    this.material.SetFloat("_Lerp_Fade_1", this.lerpValue);
    Graphics.Blit((Texture) this.renderTextureC, this.renderTextureC, this.material, -1);
  }
}
