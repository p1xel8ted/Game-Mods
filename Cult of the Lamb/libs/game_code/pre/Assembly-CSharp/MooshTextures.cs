// Decompiled with JetBrains decompiler
// Type: MooshTextures
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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

  private IEnumerator Start()
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

  private void OnPreRender()
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
