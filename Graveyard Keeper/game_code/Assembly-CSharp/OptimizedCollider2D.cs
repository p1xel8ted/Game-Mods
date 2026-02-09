// Decompiled with JetBrains decompiler
// Type: OptimizedCollider2D
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[DefaultExecutionOrder(9000)]
public abstract class OptimizedCollider2D : MonoBehaviour
{
  public bool is_trigger = true;
  public bool always_visible = true;
  public bool _initialized;
  public Bounds _local_bounds;
  public bool _bounds_inited;

  public void Init()
  {
    if (this._initialized)
      return;
    this._initialized = true;
    this.OnInit();
  }

  public abstract void OnInit();

  public abstract Bounds CalculateLocalBounds();

  public void OnDrawGizmos()
  {
    if (!this.always_visible)
      return;
    this.OnDrawGizmosSelected();
  }

  public virtual void OnDrawGizmosSelected()
  {
  }

  public Bounds local_bounds
  {
    get
    {
      if (!this._bounds_inited)
      {
        this._bounds_inited = true;
        this._local_bounds = this.CalculateLocalBounds();
      }
      return this._local_bounds;
    }
  }
}
