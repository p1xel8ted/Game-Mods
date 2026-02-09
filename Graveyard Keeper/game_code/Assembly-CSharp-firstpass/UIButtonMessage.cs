// Decompiled with JetBrains decompiler
// Type: UIButtonMessage
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("NGUI/Interaction/Button Message (Legacy)")]
public class UIButtonMessage : MonoBehaviour
{
  public GameObject target;
  public string functionName;
  public UIButtonMessage.Trigger trigger;
  public bool includeChildren;
  public bool mStarted;

  public void Start() => this.mStarted = true;

  public void OnEnable()
  {
    if (!this.mStarted)
      return;
    this.OnHover(UICamera.IsHighlighted(this.gameObject));
  }

  public void OnHover(bool isOver)
  {
    if (!this.enabled || (!isOver || this.trigger != UIButtonMessage.Trigger.OnMouseOver) && (isOver || this.trigger != UIButtonMessage.Trigger.OnMouseOut))
      return;
    this.Send();
  }

  public void OnPress(bool isPressed)
  {
    if (!this.enabled || (!isPressed || this.trigger != UIButtonMessage.Trigger.OnPress) && (isPressed || this.trigger != UIButtonMessage.Trigger.OnRelease))
      return;
    this.Send();
  }

  public void OnSelect(bool isSelected)
  {
    if (!this.enabled || isSelected && UICamera.currentScheme != UICamera.ControlScheme.Controller)
      return;
    this.OnHover(isSelected);
  }

  public void OnClick()
  {
    if (!this.enabled || this.trigger != UIButtonMessage.Trigger.OnClick)
      return;
    this.Send();
  }

  public void OnDoubleClick()
  {
    if (!this.enabled || this.trigger != UIButtonMessage.Trigger.OnDoubleClick)
      return;
    this.Send();
  }

  public void Send()
  {
    if (string.IsNullOrEmpty(this.functionName))
      return;
    if ((Object) this.target == (Object) null)
      this.target = this.gameObject;
    if (this.includeChildren)
    {
      Transform[] componentsInChildren = this.target.GetComponentsInChildren<Transform>();
      int index = 0;
      for (int length = componentsInChildren.Length; index < length; ++index)
        componentsInChildren[index].gameObject.SendMessage(this.functionName, (object) this.gameObject, SendMessageOptions.DontRequireReceiver);
    }
    else
      this.target.SendMessage(this.functionName, (object) this.gameObject, SendMessageOptions.DontRequireReceiver);
  }

  public enum Trigger
  {
    OnClick,
    OnMouseOver,
    OnMouseOut,
    OnPress,
    OnRelease,
    OnDoubleClick,
  }
}
