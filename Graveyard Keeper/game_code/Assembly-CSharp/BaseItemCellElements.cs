// Decompiled with JetBrains decompiler
// Type: BaseItemCellElements
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class BaseItemCellElements
{
  public GameObject container;
  public UI2DSprite icon;
  public UI2DSprite back;
  public UI2DSprite selection;
  public UI2DSprite gamepad_frame;
  public UILabel counter;
  public Tooltip tooltip;
  public UI2DSprite gratitude_craft_label;
  public GameObject empty_item_gfx;
  public UI2DSprite icon_cap_limit;
  [SerializeField]
  [HideInInspector]
  public Collider2D _collider;

  public Collider2D collider
  {
    get
    {
      if ((UnityEngine.Object) this._collider == (UnityEngine.Object) null)
        this._collider = this.container.GetComponent<Collider2D>();
      if ((UnityEngine.Object) this._collider == (UnityEngine.Object) null)
        this._collider = this.container.GetComponentInChildren<Collider2D>();
      int num = (UnityEngine.Object) this._collider == (UnityEngine.Object) null ? 1 : 0;
      return this._collider;
    }
  }
}
