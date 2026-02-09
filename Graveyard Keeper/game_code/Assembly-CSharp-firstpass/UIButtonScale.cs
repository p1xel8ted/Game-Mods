// Decompiled with JetBrains decompiler
// Type: UIButtonScale
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("NGUI/Interaction/Button Scale")]
public class UIButtonScale : MonoBehaviour
{
  public Transform tweenTarget;
  public Vector3 hover = new Vector3(1.1f, 1.1f, 1.1f);
  public Vector3 pressed = new Vector3(1.05f, 1.05f, 1.05f);
  public float duration = 0.2f;
  public Vector3 mScale;
  public bool mStarted;

  public void Start()
  {
    if (this.mStarted)
      return;
    this.mStarted = true;
    if ((Object) this.tweenTarget == (Object) null)
      this.tweenTarget = this.transform;
    this.mScale = this.tweenTarget.localScale;
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
    TweenScale component = this.tweenTarget.GetComponent<TweenScale>();
    if (!((Object) component != (Object) null))
      return;
    component.value = this.mScale;
    component.enabled = false;
  }

  public void OnPress(bool isPressed)
  {
    if (!this.enabled)
      return;
    if (!this.mStarted)
      this.Start();
    TweenScale.Begin(this.tweenTarget.gameObject, this.duration, isPressed ? Vector3.Scale(this.mScale, this.pressed) : (UICamera.IsHighlighted(this.gameObject) ? Vector3.Scale(this.mScale, this.hover) : this.mScale)).method = UITweener.Method.EaseInOut;
  }

  public void OnHover(bool isOver)
  {
    if (!this.enabled)
      return;
    if (!this.mStarted)
      this.Start();
    TweenScale.Begin(this.tweenTarget.gameObject, this.duration, isOver ? Vector3.Scale(this.mScale, this.hover) : this.mScale).method = UITweener.Method.EaseInOut;
  }

  public void OnSelect(bool isSelected)
  {
    if (!this.enabled || isSelected && UICamera.currentScheme != UICamera.ControlScheme.Controller)
      return;
    this.OnHover(isSelected);
  }
}
