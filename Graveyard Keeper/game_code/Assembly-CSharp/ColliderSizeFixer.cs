// Decompiled with JetBrains decompiler
// Type: ColliderSizeFixer
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ColliderSizeFixer : MonoBehaviour
{
  public BoxCollider2D _collider;
  public Transform _tf;
  public bool _fixed;

  public void Start() => this._fixed = this.CheckIfFixed();

  public void OnEnable() => this._fixed = this.CheckIfFixed();

  public void Update()
  {
    if (this._fixed)
      return;
    this.Init();
    if (!this._collider.enabled)
      return;
    this._fixed = this.CheckIfFixed();
    if (this._fixed)
      return;
    this._collider.size = new Vector2(this._collider.size.x + 2f, this._collider.size.y + 2f);
  }

  public bool CheckIfFixed()
  {
    this.Init();
    foreach (Object @object in Physics2D.OverlapPointAll((Vector2) this._tf.position, 8192 /*0x2000*/))
    {
      if (@object.GetInstanceID() == this._collider.GetInstanceID())
        return true;
    }
    return false;
  }

  public void Init()
  {
    if ((Object) this._collider != (Object) null)
      return;
    this._collider = this.GetComponent<BoxCollider2D>();
    this._tf = this.transform;
  }
}
