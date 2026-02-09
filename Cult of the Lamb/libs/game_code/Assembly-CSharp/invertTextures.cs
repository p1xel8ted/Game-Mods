// Decompiled with JetBrains decompiler
// Type: invertTextures
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class invertTextures : BaseMonoBehaviour
{
  public Material material;
  public RenderTexture renderTexture1;
  public RenderTexture renderTexture2;

  public void OnPreRender()
  {
    this.material.SetTexture("_RenderTex_1", (Texture) this.renderTexture1);
    this.material.SetTexture("_RenderTex_2", (Texture) this.renderTexture2);
    Graphics.Blit((Texture) this.renderTexture2, this.renderTexture2, this.material, -1);
  }
}
