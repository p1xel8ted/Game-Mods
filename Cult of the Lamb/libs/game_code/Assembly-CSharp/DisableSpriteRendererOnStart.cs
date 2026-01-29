// Decompiled with JetBrains decompiler
// Type: DisableSpriteRendererOnStart
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DisableSpriteRendererOnStart : BaseMonoBehaviour
{
  public SpriteRenderer spriterenderer;
  public bool grabSpriteRenderer;

  public void Start()
  {
    if (this.grabSpriteRenderer)
      this.spriterenderer = this.gameObject.GetComponent<SpriteRenderer>();
    this.spriterenderer.enabled = false;
  }
}
