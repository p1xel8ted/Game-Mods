// Decompiled with JetBrains decompiler
// Type: GroundObject
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class GroundObject : MonoBehaviour
{
  public SpriteRenderer _spr_renderer;
  public bool _need_reset_haschanged;
  public bool _can_move = true;

  public void Update()
  {
    if (!this._can_move || !this.transform.hasChanged)
      return;
    Vector3 position = this.transform.position;
    position.z = GroundObject.GetGroundZ((Vector2) position);
    this.transform.position = position;
    if ((Object) this._spr_renderer == (Object) null)
      this._spr_renderer = this.GetComponent<SpriteRenderer>();
    if ((Object) this._spr_renderer != (Object) null)
    {
      if (!RoundAndSortComponent.DoesSpriteBelongToGround(this._spr_renderer))
        this._spr_renderer.sortingLayerName = "on_ground";
      this._spr_renderer.sortingOrder = RoundAndSortComponent.GetSpriteOrderN((Vector2) position);
    }
    this._need_reset_haschanged = true;
  }

  public void LateUpdate()
  {
    if (!this._can_move || !Application.isPlaying || !this._need_reset_haschanged)
      return;
    this._need_reset_haschanged = false;
    this.transform.hasChanged = false;
  }

  public static float GetGroundZ(Vector2 pos)
  {
    return (float) (2000.0 + (double) pos.x / 96.0 * (1.0 / 1000.0));
  }

  public bool can_move
  {
    set
    {
      if (!value && this._can_move)
        this.Update();
      this._can_move = value;
    }
  }
}
