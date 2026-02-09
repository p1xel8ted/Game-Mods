// Decompiled with JetBrains decompiler
// Type: UIButtonOffset
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("NGUI/Interaction/Button Offset")]
public class UIButtonOffset : MonoBehaviour
{
  public Transform tweenTarget;
  public Vector3 hover = Vector3.zero;
  public Vector3 pressed = new Vector3(2f, -2f);
  public float duration = 0.2f;
  [NonSerialized]
  public Vector3 mPos;
  [NonSerialized]
  public bool mStarted;
  [NonSerialized]
  public bool mPressed;

  public void Start()
  {
    if (this.mStarted)
      return;
    this.mStarted = true;
    if ((UnityEngine.Object) this.tweenTarget == (UnityEngine.Object) null)
      this.tweenTarget = this.transform;
    this.mPos = this.tweenTarget.localPosition;
  }

  public void OnEnable()
  {
    if (!this.mStarted)
      return;
    this.OnHover(UICamera.IsHighlighted(this.gameObject));
  }

  public void OnDisable()
  {
    if (!this.mStarted || !((UnityEngine.Object) this.tweenTarget != (UnityEngine.Object) null))
      return;
    TweenPosition component = this.tweenTarget.GetComponent<TweenPosition>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.value = this.mPos;
    component.enabled = false;
  }

  public void OnPress(bool isPressed)
  {
    this.mPressed = isPressed;
    if (!this.enabled)
      return;
    if (!this.mStarted)
      this.Start();
    TweenPosition.Begin(this.tweenTarget.gameObject, this.duration, isPressed ? this.mPos + this.pressed : (UICamera.IsHighlighted(this.gameObject) ? this.mPos + this.hover : this.mPos)).method = UITweener.Method.EaseInOut;
  }

  public void OnHover(bool isOver)
  {
    if (!this.enabled)
      return;
    if (!this.mStarted)
      this.Start();
    TweenPosition.Begin(this.tweenTarget.gameObject, this.duration, isOver ? this.mPos + this.hover : this.mPos).method = UITweener.Method.EaseInOut;
  }

  public void OnDragOver()
  {
    if (!this.mPressed)
      return;
    TweenPosition.Begin(this.tweenTarget.gameObject, this.duration, this.mPos + this.hover).method = UITweener.Method.EaseInOut;
  }

  public void OnDragOut()
  {
    if (!this.mPressed)
      return;
    TweenPosition.Begin(this.tweenTarget.gameObject, this.duration, this.mPos).method = UITweener.Method.EaseInOut;
  }

  public void OnSelect(bool isSelected)
  {
    if (!this.enabled || isSelected && UICamera.currentScheme != UICamera.ControlScheme.Controller)
      return;
    this.OnHover(isSelected);
  }
}
