// Decompiled with JetBrains decompiler
// Type: invertTextures
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class invertTextures : BaseMonoBehaviour
{
  public Material material;
  public RenderTexture renderTexture1;
  public RenderTexture renderTexture2;

  private void OnPreRender()
  {
    this.material.SetTexture("_RenderTex_1", (Texture) this.renderTexture1);
    this.material.SetTexture("_RenderTex_2", (Texture) this.renderTexture2);
    Graphics.Blit((Texture) this.renderTexture2, this.renderTexture2, this.material, -1);
  }
}
