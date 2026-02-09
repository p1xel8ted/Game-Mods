// Decompiled with JetBrains decompiler
// Type: OptimizedCircleCollider2D
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class OptimizedCircleCollider2D : OptimizedCollider2D
{
  public Vector2 offset = Vector2.zero;
  public float radius = 1f;
  public CircleCollider2D _collider;
  public const bool DRAW_AS_POLYGONS = true;

  public override void OnInit()
  {
    this._collider = this.gameObject.AddComponent<CircleCollider2D>();
    this._collider.offset = this.offset;
    this._collider.radius = this.radius;
    this._collider.isTrigger = this.is_trigger;
  }

  public override void OnDrawGizmosSelected()
  {
  }

  public override Bounds CalculateLocalBounds()
  {
    return new Bounds((Vector3) this.offset, (Vector3) (Vector2.one * this.radius * 2f));
  }
}
