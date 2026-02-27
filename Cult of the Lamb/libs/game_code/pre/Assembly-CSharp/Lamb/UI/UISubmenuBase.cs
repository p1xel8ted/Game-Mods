// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UISubmenuBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Lamb.UI;

public abstract class UISubmenuBase : UIMenuBase
{
  protected UIMenuBase _parent;

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
