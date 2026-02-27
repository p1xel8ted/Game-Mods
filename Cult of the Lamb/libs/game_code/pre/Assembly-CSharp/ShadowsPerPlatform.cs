// Decompiled with JetBrains decompiler
// Type: ShadowsPerPlatform
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Rendering;

#nullable disable
public class ShadowsPerPlatform : MonoBehaviour
{
  [SerializeField]
  private SpriteRenderer _spriteRenderer;

  private void Start()
  {
    if ((Object) this._spriteRenderer == (Object) null)
      this._spriteRenderer = this.GetComponent<SpriteRenderer>();
    if ((Object) this._spriteRenderer == (Object) null)
      return;
    this._spriteRenderer.shadowCastingMode = SettingsManager.Settings.Graphics.Shadows ? ShadowCastingMode.On : ShadowCastingMode.Off;
  }
}
