// Decompiled with JetBrains decompiler
// Type: UIButtonRotation
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("NGUI/Interaction/Button Rotation")]
public class UIButtonRotation : MonoBehaviour
{
  public Transform tweenTarget;
  public Vector3 hover = Vector3.zero;
  public Vector3 pressed = Vector3.zero;
  public float duration = 0.2f;
  public Quaternion mRot;
  public bool mStarted;

  public void Start()
  {
    if (this.mStarted)
      return;
    this.mStarted = true;
    if ((Object) this.tweenTarget == (Object) null)
      this.tweenTarget = this.transform;
    this.mRot = this.tweenTarget.localRotation;
  }

  public void OnEnable()
  {
    if (!this.mStarted)
      return;
    this.OnHover(UICamera.IsHighlighted(this.gameObject));
  }

  public void OnDisable()
  {
    if (!this.mStarted || !((Object) this.tweenTarget != (Object) null))
      return;
    TweenRotation component = this.tweenTarget.GetComponent<TweenRotation>();
    if (!((Object) component != (Object) null))
      return;
    component.value = this.mRot;
    component.enabled = false;
  }

  public void OnPress(bool isPressed)
  {
    if (!this.enabled)
      return;
    if (!this.mStarted)
      this.Start();
    TweenRotation.Begin(this.tweenTarget.gameObject, this.duration, isPressed ? this.mRot * Quaternion.Euler(this.pressed) : (UICamera.IsHighlighted(this.gameObject) ? this.mRot * Quaternion.Euler(this.hover) : this.mRot)).method = UITweener.Method.EaseInOut;
  }

  public void OnHover(bool isOver)
  {
    if (!this.enabled)
      return;
    if (!this.mStarted)
      this.Start();
    TweenRotation.Begin(this.tweenTarget.gameObject, this.duration, isOver ? this.mRot * Quaternion.Euler(this.hover) : this.mRot).method = UITweener.Method.EaseInOut;
  }

  public void OnSelect(bool isSelected)
  {
    if (!this.enabled || isSelected && UICamera.currentScheme != UICamera.ControlScheme.Controller)
      return;
    this.OnHover(isSelected);
  }
}
