// Decompiled with JetBrains decompiler
// Type: UIEventListener
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("NGUI/Internal/Event Listener")]
public class UIEventListener : MonoBehaviour
{
  public object parameter;
  public UIEventListener.VoidDelegate onSubmit;
  public UIEventListener.VoidDelegate onClick;
  public UIEventListener.VoidDelegate onDoubleClick;
  public UIEventListener.BoolDelegate onHover;
  public UIEventListener.BoolDelegate onPress;
  public UIEventListener.BoolDelegate onSelect;
  public UIEventListener.FloatDelegate onScroll;
  public UIEventListener.VoidDelegate onDragStart;
  public UIEventListener.VectorDelegate onDrag;
  public UIEventListener.VoidDelegate onDragOver;
  public UIEventListener.VoidDelegate onDragOut;
  public UIEventListener.VoidDelegate onDragEnd;
  public UIEventListener.ObjectDelegate onDrop;
  public UIEventListener.KeyCodeDelegate onKey;
  public UIEventListener.BoolDelegate onTooltip;

  public bool isColliderEnabled
  {
    get
    {
      Collider component1 = this.GetComponent<Collider>();
      if ((Object) component1 != (Object) null)
        return component1.enabled;
      Collider2D component2 = this.GetComponent<Collider2D>();
      return (Object) component2 != (Object) null && component2.enabled;
    }
  }

  public void OnSubmit()
  {
    if (!this.isColliderEnabled || this.onSubmit == null)
      return;
    this.onSubmit(this.gameObject);
  }

  public void OnClick()
  {
    if (!this.isColliderEnabled || this.onClick == null)
      return;
    this.onClick(this.gameObject);
  }

  public void OnDoubleClick()
  {
    if (!this.isColliderEnabled || this.onDoubleClick == null)
      return;
    this.onDoubleClick(this.gameObject);
  }

  public void OnHover(bool isOver)
  {
    if (!this.isColliderEnabled || this.onHover == null)
      return;
    this.onHover(this.gameObject, isOver);
  }

  public void OnPress(bool isPressed)
  {
    if (!this.isColliderEnabled || this.onPress == null)
      return;
    this.onPress(this.gameObject, isPressed);
  }

  public void OnSelect(bool selected)
  {
    if (!this.isColliderEnabled || this.onSelect == null)
      return;
    this.onSelect(this.gameObject, selected);
  }

  public void OnScroll(float delta)
  {
    if (!this.isColliderEnabled || this.onScroll == null)
      return;
    this.onScroll(this.gameObject, delta);
  }

  public void OnDragStart()
  {
    if (this.onDragStart == null)
      return;
    this.onDragStart(this.gameObject);
  }

  public void OnDrag(Vector2 delta)
  {
    if (this.onDrag == null)
      return;
    this.onDrag(this.gameObject, delta);
  }

  public void OnDragOver()
  {
    if (!this.isColliderEnabled || this.onDragOver == null)
      return;
    this.onDragOver(this.gameObject);
  }

  public void OnDragOut()
  {
    if (!this.isColliderEnabled || this.onDragOut == null)
      return;
    this.onDragOut(this.gameObject);
  }

  public void OnDragEnd()
  {
    if (this.onDragEnd == null)
      return;
    this.onDragEnd(this.gameObject);
  }

  public void OnDrop(GameObject go)
  {
    if (!this.isColliderEnabled || this.onDrop == null)
      return;
    this.onDrop(this.gameObject, go);
  }

  public void OnKey(KeyCode key)
  {
    if (!this.isColliderEnabled || this.onKey == null)
      return;
    this.onKey(this.gameObject, key);
  }

  public void OnTooltip(bool show)
  {
    if (!this.isColliderEnabled || this.onTooltip == null)
      return;
    this.onTooltip(this.gameObject, show);
  }

  public void Clear()
  {
    this.onSubmit = (UIEventListener.VoidDelegate) null;
    this.onClick = (UIEventListener.VoidDelegate) null;
    this.onDoubleClick = (UIEventListener.VoidDelegate) null;
    this.onHover = (UIEventListener.BoolDelegate) null;
    this.onPress = (UIEventListener.BoolDelegate) null;
    this.onSelect = (UIEventListener.BoolDelegate) null;
    this.onScroll = (UIEventListener.FloatDelegate) null;
    this.onDragStart = (UIEventListener.VoidDelegate) null;
    this.onDrag = (UIEventListener.VectorDelegate) null;
    this.onDragOver = (UIEventListener.VoidDelegate) null;
    this.onDragOut = (UIEventListener.VoidDelegate) null;
    this.onDragEnd = (UIEventListener.VoidDelegate) null;
    this.onDrop = (UIEventListener.ObjectDelegate) null;
    this.onKey = (UIEventListener.KeyCodeDelegate) null;
    this.onTooltip = (UIEventListener.BoolDelegate) null;
  }

  public static UIEventListener Get(GameObject go)
  {
    UIEventListener uiEventListener = go.GetComponent<UIEventListener>();
    if ((Object) uiEventListener == (Object) null)
      uiEventListener = go.AddComponent<UIEventListener>();
    return uiEventListener;
  }

  public delegate void VoidDelegate(GameObject go);

  public delegate void BoolDelegate(GameObject go, bool state);

  public delegate void FloatDelegate(GameObject go, float delta);

  public delegate void VectorDelegate(GameObject go, Vector2 delta);

  public delegate void ObjectDelegate(GameObject go, GameObject obj);

  public delegate void KeyCodeDelegate(GameObject go, KeyCode key);
}
