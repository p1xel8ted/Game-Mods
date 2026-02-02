// Decompiled with JetBrains decompiler
// Type: ChangeShader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (SpriteRenderer))]
public class ChangeShader : BaseMonoBehaviour
{
  public Shader shader;
  public SpriteRenderer _spriteRenderer;

  public void Start()
  {
  }

  public void UpdateShader()
  {
    this._spriteRenderer = this.GetComponent<SpriteRenderer>();
    this._spriteRenderer.sharedMaterial.shader = this.shader;
  }

  public void Update()
  {
  }
}
