// Decompiled with JetBrains decompiler
// Type: ChangingLightSprite
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (SpriteRenderer))]
[ExecuteInEditMode]
public class ChangingLightSprite : MonoBehaviour
{
  public SpriteRenderer _spr;
  public bool _spr_set;

  public void Update() => this.spr.color = TimeOfDay.light_sprites_color;

  public SpriteRenderer spr
  {
    get
    {
      if (!this._spr_set)
      {
        this._spr_set = true;
        this._spr = this.GetComponent<SpriteRenderer>();
      }
      return this._spr;
    }
  }
}
