// Decompiled with JetBrains decompiler
// Type: UIForwardEvents
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("NGUI/Interaction/Forward Events (Legacy)")]
public class UIForwardEvents : MonoBehaviour
{
  public GameObject target;
  public bool onHover;
  public bool onPress;
  public bool onClick;
  public bool onDoubleClick;
  public bool onSelect;
  public bool onDrag;
  public bool onDrop;
  public bool onSubmit;
  public bool onScroll;

  public void OnHover(bool isOver)
  {
    if (!this.onHover || !((Object) this.target != (Object) null))
      return;
    this.target.SendMessage(nameof (OnHover), (object) isOver, SendMessageOptions.DontRequireReceiver);
  }

  public void OnPress(bool pressed)
  {
    if (!this.onPress || !((Object) this.target != (Object) null))
      return;
    this.target.SendMessage(nameof (OnPress), (object) pressed, SendMessageOptions.DontRequireReceiver);
  }

  public void OnClick()
  {
    if (!this.onClick || !((Object) this.target != (Object) null))
      return;
    this.target.SendMessage(nameof (OnClick), SendMessageOptions.DontRequireReceiver);
  }

  public void OnDoubleClick()
  {
    if (!this.onDoubleClick || !((Object) this.target != (Object) null))
      return;
    this.target.SendMessage(nameof (OnDoubleClick), SendMessageOptions.DontRequireReceiver);
  }

  public void OnSelect(bool selected)
  {
    if (!this.onSelect || !((Object) this.target != (Object) null))
      return;
    this.target.SendMessage(nameof (OnSelect), (object) selected, SendMessageOptions.DontRequireReceiver);
  }

  public void OnDrag(Vector2 delta)
  {
    if (!this.onDrag || !((Object) this.target != (Object) null))
      return;
    this.target.SendMessage(nameof (OnDrag), (object) delta, SendMessageOptions.DontRequireReceiver);
  }

  public void OnDrop(GameObject go)
  {
    if (!this.onDrop || !((Object) this.target != (Object) null))
      return;
    this.target.SendMessage(nameof (OnDrop), (object) go, SendMessageOptions.DontRequireReceiver);
  }

  public void OnSubmit()
  {
    if (!this.onSubmit || !((Object) this.target != (Object) null))
      return;
    this.target.SendMessage(nameof (OnSubmit), SendMessageOptions.DontRequireReceiver);
  }

  public void OnScroll(float delta)
  {
    if (!this.onScroll || !((Object) this.target != (Object) null))
      return;
    this.target.SendMessage(nameof (OnScroll), (object) delta, SendMessageOptions.DontRequireReceiver);
  }
}
