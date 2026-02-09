// Decompiled with JetBrains decompiler
// Type: ShadowsPerPlatform
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Rendering;

#nullable disable
public class ShadowsPerPlatform : MonoBehaviour
{
  [SerializeField]
  public SpriteRenderer _spriteRenderer;

  public void Start()
  {
    if ((Object) this._spriteRenderer == (Object) null)
      this._spriteRenderer = this.GetComponent<SpriteRenderer>();
    if ((Object) this._spriteRenderer == (Object) null)
      return;
    this._spriteRenderer.shadowCastingMode = ShadowCastingMode.On;
  }
}
