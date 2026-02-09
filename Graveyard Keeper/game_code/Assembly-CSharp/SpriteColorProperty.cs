// Decompiled with JetBrains decompiler
// Type: SpriteColorProperty
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
[ExecuteInEditMode]
[RequireComponent(typeof (SpriteRenderer))]
public class SpriteColorProperty : MaterialPropertyModifier
{
  public Color color2 = Color.black;
  public SpriteRenderer _spr_renderer;
  public Color _last_color = Color.black;

  public void Awake() => this._spr_renderer = this.GetComponent<SpriteRenderer>();

  public void Start() => this.UpdateRenderer();

  public void Update()
  {
    if (!(this.color2 != this._last_color))
      return;
    this.UpdateRenderer();
  }

  public override void UpdateRenderer()
  {
    this.DoUpdateRenderer((Renderer) this._spr_renderer, (MaterialPropertyModifier.PropertyModifier) (prop => prop.SetColor("_Color2", this.color2)));
    this._last_color = this.color2;
  }

  [CompilerGenerated]
  public void \u003CUpdateRenderer\u003Eb__6_0(MaterialPropertyBlock prop)
  {
    prop.SetColor("_Color2", this.color2);
  }
}
