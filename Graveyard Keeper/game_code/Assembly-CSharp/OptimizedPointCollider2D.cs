// Decompiled with JetBrains decompiler
// Type: OptimizedPointCollider2D
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class OptimizedPointCollider2D : OptimizedCollider2D
{
  public Vector2 offset = Vector2.zero;
  public CircleCollider2D _collider;

  public override void OnInit()
  {
    this._collider = this.gameObject.AddComponent<CircleCollider2D>();
    this._collider.offset = this.offset;
    this._collider.radius = 0.01f;
    this._collider.isTrigger = this.is_trigger;
  }

  public override void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.yellow;
    float x = this.transform.lossyScale.x;
    Gizmos.DrawWireSphere(this.transform.position + (Vector3) this.offset * x, 0.01f * x);
  }

  public override Bounds CalculateLocalBounds()
  {
    return new Bounds((Vector3) this.offset, Vector3.one * 0.01f);
  }
}
