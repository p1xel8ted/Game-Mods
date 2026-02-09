// Decompiled with JetBrains decompiler
// Type: UIButtonKeys
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Button Keys (Legacy)")]
public class UIButtonKeys : UIKeyNavigation
{
  public UIButtonKeys selectOnClick;
  public UIButtonKeys selectOnUp;
  public UIButtonKeys selectOnDown;
  public UIButtonKeys selectOnLeft;
  public UIButtonKeys selectOnRight;

  public override void OnEnable()
  {
    this.Upgrade();
    base.OnEnable();
  }

  public void Upgrade()
  {
    if ((Object) this.onClick == (Object) null && (Object) this.selectOnClick != (Object) null)
    {
      this.onClick = this.selectOnClick.gameObject;
      this.selectOnClick = (UIButtonKeys) null;
      NGUITools.SetDirty((Object) this);
    }
    if ((Object) this.onLeft == (Object) null && (Object) this.selectOnLeft != (Object) null)
    {
      this.onLeft = this.selectOnLeft.gameObject;
      this.selectOnLeft = (UIButtonKeys) null;
      NGUITools.SetDirty((Object) this);
    }
    if ((Object) this.onRight == (Object) null && (Object) this.selectOnRight != (Object) null)
    {
      this.onRight = this.selectOnRight.gameObject;
      this.selectOnRight = (UIButtonKeys) null;
      NGUITools.SetDirty((Object) this);
    }
    if ((Object) this.onUp == (Object) null && (Object) this.selectOnUp != (Object) null)
    {
      this.onUp = this.selectOnUp.gameObject;
      this.selectOnUp = (UIButtonKeys) null;
      NGUITools.SetDirty((Object) this);
    }
    if (!((Object) this.onDown == (Object) null) || !((Object) this.selectOnDown != (Object) null))
      return;
    this.onDown = this.selectOnDown.gameObject;
    this.selectOnDown = (UIButtonKeys) null;
    NGUITools.SetDirty((Object) this);
  }
}
