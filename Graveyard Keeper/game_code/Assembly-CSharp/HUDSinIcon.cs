// Decompiled with JetBrains decompiler
// Type: HUDSinIcon
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class HUDSinIcon : MonoBehaviour
{
  public UI2DSprite spr_back;
  public UI2DSprite spr_active;
  public Color glow_color;
  public Sins.SinType _sin_type;

  public void Init()
  {
  }

  public void Update()
  {
  }

  public void Draw(Sins.SinType sin_type, UnityEngine.Sprite back, UnityEngine.Sprite active, Color glow)
  {
    this.spr_back.sprite2D = back;
    this.spr_active.sprite2D = active;
    this.glow_color = glow;
    this._sin_type = sin_type;
    this.Redraw();
  }

  public void Redraw() => this.spr_active.enabled = MainGame.me.save.GetSinState(this._sin_type);
}
