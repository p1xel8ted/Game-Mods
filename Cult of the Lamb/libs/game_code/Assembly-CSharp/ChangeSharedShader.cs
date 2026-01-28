// Decompiled with JetBrains decompiler
// Type: ChangeSharedShader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (SpriteRenderer))]
public class ChangeSharedShader : BaseMonoBehaviour
{
  public Shader shader;
  public SpriteRenderer _spriteRenderer;

  public void UpdateShader()
  {
    this._spriteRenderer = this.GetComponent<SpriteRenderer>();
    this._spriteRenderer.sharedMaterial.shader = this.shader;
  }
}
