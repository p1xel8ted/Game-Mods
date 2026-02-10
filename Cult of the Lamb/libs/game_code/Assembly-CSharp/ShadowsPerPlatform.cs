// Decompiled with JetBrains decompiler
// Type: ShadowsPerPlatform
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
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
