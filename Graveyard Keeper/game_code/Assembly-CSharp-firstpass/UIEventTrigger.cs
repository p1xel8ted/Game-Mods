// Decompiled with JetBrains decompiler
// Type: UIEventTrigger
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("NGUI/Interaction/Event Trigger")]
public class UIEventTrigger : MonoBehaviour
{
  public static UIEventTrigger current;
  public List<EventDelegate> onHoverOver = new List<EventDelegate>();
  public List<EventDelegate> onHoverOut = new List<EventDelegate>();
  public List<EventDelegate> onPress = new List<EventDelegate>();
  public List<EventDelegate> onRelease = new List<EventDelegate>();
  public List<EventDelegate> onSelect = new List<EventDelegate>();
  public List<EventDelegate> onDeselect = new List<EventDelegate>();
  public List<EventDelegate> onClick = new List<EventDelegate>();
  public List<EventDelegate> onDoubleClick = new List<EventDelegate>();
  public List<EventDelegate> onDragStart = new List<EventDelegate>();
  public List<EventDelegate> onDragEnd = new List<EventDelegate>();
  public List<EventDelegate> onDragOver = new List<EventDelegate>();
  public List<EventDelegate> onDragOut = new List<EventDelegate>();
  public List<EventDelegate> onDrag = new List<EventDelegate>();

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

  public void OnHover(bool isOver)
  {
    if ((Object) UIEventTrigger.current != (Object) null || !this.isColliderEnabled)
      return;
    UIEventTrigger.current = this;
    if (isOver)
      EventDelegate.Execute(this.onHoverOver);
    else
      EventDelegate.Execute(this.onHoverOut);
    UIEventTrigger.current = (UIEventTrigger) null;
  }

  public void OnPress(bool pressed)
  {
    if ((Object) UIEventTrigger.current != (Object) null || !this.isColliderEnabled)
      return;
    UIEventTrigger.current = this;
    if (pressed)
      EventDelegate.Execute(this.onPress);
    else
      EventDelegate.Execute(this.onRelease);
    UIEventTrigger.current = (UIEventTrigger) null;
  }

  public void OnSelect(bool selected)
  {
    if ((Object) UIEventTrigger.current != (Object) null || !this.isColliderEnabled)
      return;
    UIEventTrigger.current = this;
    if (selected)
      EventDelegate.Execute(this.onSelect);
    else
      EventDelegate.Execute(this.onDeselect);
    UIEventTrigger.current = (UIEventTrigger) null;
  }

  public void OnClick()
  {
    if ((Object) UIEventTrigger.current != (Object) null || !this.isColliderEnabled)
      return;
    UIEventTrigger.current = this;
    EventDelegate.Execute(this.onClick);
    UIEventTrigger.current = (UIEventTrigger) null;
  }

  public void OnDoubleClick()
  {
    if ((Object) UIEventTrigger.current != (Object) null || !this.isColliderEnabled)
      return;
    UIEventTrigger.current = this;
    EventDelegate.Execute(this.onDoubleClick);
    UIEventTrigger.current = (UIEventTrigger) null;
  }

  public void OnDragStart()
  {
    if ((Object) UIEventTrigger.current != (Object) null)
      return;
    UIEventTrigger.current = this;
    EventDelegate.Execute(this.onDragStart);
    UIEventTrigger.current = (UIEventTrigger) null;
  }

  public void OnDragEnd()
  {
    if ((Object) UIEventTrigger.current != (Object) null)
      return;
    UIEventTrigger.current = this;
    EventDelegate.Execute(this.onDragEnd);
    UIEventTrigger.current = (UIEventTrigger) null;
  }

  public void OnDragOver(GameObject go)
  {
    if ((Object) UIEventTrigger.current != (Object) null || !this.isColliderEnabled)
      return;
    UIEventTrigger.current = this;
    EventDelegate.Execute(this.onDragOver);
    UIEventTrigger.current = (UIEventTrigger) null;
  }

  public void OnDragOut(GameObject go)
  {
    if ((Object) UIEventTrigger.current != (Object) null || !this.isColliderEnabled)
      return;
    UIEventTrigger.current = this;
    EventDelegate.Execute(this.onDragOut);
    UIEventTrigger.current = (UIEventTrigger) null;
  }

  public void OnDrag(Vector2 delta)
  {
    if ((Object) UIEventTrigger.current != (Object) null)
      return;
    UIEventTrigger.current = this;
    EventDelegate.Execute(this.onDrag);
    UIEventTrigger.current = (UIEventTrigger) null;
  }
}
