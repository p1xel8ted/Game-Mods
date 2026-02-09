// Decompiled with JetBrains decompiler
// Type: GrassDeformSprite
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
[ExecuteInEditMode]
[RequireComponent(typeof (SpriteRenderer))]
public class GrassDeformSprite : MaterialPropertyModifier
{
  public SpriteRenderer _spr_renderer;
  public float skew;
  public float _wind_phase;
  public float wind_coef = 1f;

  public void Awake()
  {
    this._spr_renderer = this.GetComponent<SpriteRenderer>();
    this._wind_phase = Random.Range(0.0f, 3f);
  }

  public override void UpdateRenderer()
  {
    this.DoUpdateRenderer((Renderer) this._spr_renderer, (MaterialPropertyModifier.PropertyModifier) (prop =>
    {
      prop.SetFloat("_Skew", this.skew);
      prop.SetFloat("_WindPhase", this._wind_phase);
      prop.SetFloat("_WindK", this.wind_coef);
    }));
  }

  [CompilerGenerated]
  public void \u003CUpdateRenderer\u003Eb__5_0(MaterialPropertyBlock prop)
  {
    prop.SetFloat("_Skew", this.skew);
    prop.SetFloat("_WindPhase", this._wind_phase);
    prop.SetFloat("_WindK", this.wind_coef);
  }
}
