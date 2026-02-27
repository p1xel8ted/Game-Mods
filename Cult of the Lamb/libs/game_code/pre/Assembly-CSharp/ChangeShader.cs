// Decompiled with JetBrains decompiler
// Type: ChangeShader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (SpriteRenderer))]
public class ChangeShader : BaseMonoBehaviour
{
  public Shader shader;
  public SpriteRenderer _spriteRenderer;

  private void Start()
  {
  }

  private void UpdateShader()
  {
    this._spriteRenderer = this.GetComponent<SpriteRenderer>();
    this._spriteRenderer.sharedMaterial.shader = this.shader;
  }

  private void Update()
  {
  }
}
