// Decompiled with JetBrains decompiler
// Type: NormalMapSprite
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (SpriteRenderer))]
[ExecuteInEditMode]
public class NormalMapSprite : MaterialPropertyModifier
{
  public Texture2D normal_map;
  public SpriteRenderer _spr_renderer;
  public Texture2D _last_normal_map;
  public UnityEngine.Sprite _last_spr;

  public void Awake() => this._spr_renderer = this.GetComponent<SpriteRenderer>();

  public override void UpdateRenderer()
  {
    this.DoUpdateRenderer((Renderer) this._spr_renderer, (MaterialPropertyModifier.PropertyModifier) (prop =>
    {
      if (!((Object) this.normal_map != (Object) null))
        return;
      prop.SetTexture("_NormalDepth", (Texture) this.normal_map);
    }));
    this._last_normal_map = this.normal_map;
  }

  [CompilerGenerated]
  public void \u003CUpdateRenderer\u003Eb__5_0(MaterialPropertyBlock prop)
  {
    if (!((Object) this.normal_map != (Object) null))
      return;
    prop.SetTexture("_NormalDepth", (Texture) this.normal_map);
  }
}
