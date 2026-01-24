// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UISubmenuBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Lamb.UI;

public abstract class UISubmenuBase : UIMenuBase
{
  public UIMenuBase _parent;

  public override void Awake()
  {
    base.Awake();
    this.Hide(true);
    this._parent = this.GetComponentInParent<UIMenuBase>();
  }

  public override T Push<T>(T menu)
  {
    return (Object) this._parent != (Object) null ? this._parent.Push<T>(menu) : base.Push<T>(menu);
  }

  public override T PushInstance<T>(T menu)
  {
    return (Object) this._parent != (Object) null ? this._parent.PushInstance<T>(menu) : base.PushInstance<T>(menu);
  }
}
