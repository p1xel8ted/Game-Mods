// Decompiled with JetBrains decompiler
// Type: ChangeShader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
