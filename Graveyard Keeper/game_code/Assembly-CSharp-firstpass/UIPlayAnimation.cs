// Decompiled with JetBrains decompiler
// Type: UIPlayAnimation
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using AnimationOrTween;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("NGUI/Interaction/Play Animation")]
[ExecuteInEditMode]
public class UIPlayAnimation : MonoBehaviour
{
  public static UIPlayAnimation current;
  public Animation target;
  public Animator animator;
  public string clipName;
  public AnimationOrTween.Trigger trigger;
  public AnimationOrTween.Direction playDirection = AnimationOrTween.Direction.Forward;
  public bool resetOnPlay;
  public bool clearSelection;
  public EnableCondition ifDisabledOnPlay;
  public DisableCondition disableWhenFinished;
  public List<EventDelegate> onFinished = new List<EventDelegate>();
  [HideInInspector]
  [SerializeField]
  public GameObject eventReceiver;
  [HideInInspector]
  [SerializeField]
  public string callWhenFinished;
  public bool mStarted;
  public bool mActivated;
  public bool dragHighlight;

  public bool dualState => this.trigger == AnimationOrTween.Trigger.OnPress || this.trigger == AnimationOrTween.Trigger.OnHover;

  public void Awake()
  {
    UIButton component = this.GetComponent<UIButton>();
    if ((Object) component != (Object) null)
      this.dragHighlight = component.dragHighlight;
    if (!((Object) this.eventReceiver != (Object) null) || !EventDelegate.IsValid(this.onFinished))
      return;
    this.eventReceiver = (GameObject) null;
    this.callWhenFinished = (string) null;
  }

  public void Start()
  {
    this.mStarted = true;
    if ((Object) this.target == (Object) null && (Object) this.animator == (Object) null)
      this.animator = this.GetComponentInChildren<Animator>();
    if ((Object) this.animator != (Object) null)
    {
      if (!this.animator.enabled)
        return;
      this.animator.enabled = false;
    }
    else
    {
      if ((Object) this.target == (Object) null)
        this.target = this.GetComponentInChildren<Animation>();
      if (!((Object) this.target != (Object) null) || !this.target.enabled)
        return;
      this.target.enabled = false;
    }
  }

  public void OnEnable()
  {
    if (this.mStarted)
      this.OnHover(UICamera.IsHighlighted(this.gameObject));
    if (UICamera.currentTouch != null)
    {
      if (this.trigger == AnimationOrTween.Trigger.OnPress || this.trigger == AnimationOrTween.Trigger.OnPressTrue)
        this.mActivated = (Object) UICamera.currentTouch.pressed == (Object) this.gameObject;
      if (this.trigger == AnimationOrTween.Trigger.OnHover || this.trigger == AnimationOrTween.Trigger.OnHoverTrue)
        this.mActivated = (Object) UICamera.currentTouch.current == (Object) this.gameObject;
    }
    UIToggle component = this.GetComponent<UIToggle>();
    if (!((Object) component != (Object) null))
      return;
    EventDelegate.Add(component.onChange, new EventDelegate.Callback(this.OnToggle));
  }

  public void OnDisable()
  {
    UIToggle component = this.GetComponent<UIToggle>();
    if (!((Object) component != (Object) null))
      return;
    EventDelegate.Remove(component.onChange, new EventDelegate.Callback(this.OnToggle));
  }

  public void OnHover(bool isOver)
  {
    if (!this.enabled || this.trigger != AnimationOrTween.Trigger.OnHover && !(this.trigger == AnimationOrTween.Trigger.OnHoverTrue & isOver) && (this.trigger != AnimationOrTween.Trigger.OnHoverFalse || isOver))
      return;
    this.Play(isOver, this.dualState);
  }

  public void OnPress(bool isPressed)
  {
    if (!this.enabled || UICamera.currentTouchID == -2 || UICamera.currentTouchID == -3 || this.trigger != AnimationOrTween.Trigger.OnPress && !(this.trigger == AnimationOrTween.Trigger.OnPressTrue & isPressed) && (this.trigger != AnimationOrTween.Trigger.OnPressFalse || isPressed))
      return;
    this.Play(isPressed, this.dualState);
  }

  public void OnClick()
  {
    if (UICamera.currentTouchID == -2 || UICamera.currentTouchID == -3 || !this.enabled || this.trigger != AnimationOrTween.Trigger.OnClick)
      return;
    this.Play(true, false);
  }

  public void OnDoubleClick()
  {
    if (UICamera.currentTouchID == -2 || UICamera.currentTouchID == -3 || !this.enabled || this.trigger != AnimationOrTween.Trigger.OnDoubleClick)
      return;
    this.Play(true, false);
  }

  public void OnSelect(bool isSelected)
  {
    if (!this.enabled || this.trigger != AnimationOrTween.Trigger.OnSelect && !(this.trigger == AnimationOrTween.Trigger.OnSelectTrue & isSelected) && (this.trigger != AnimationOrTween.Trigger.OnSelectFalse || isSelected))
      return;
    this.Play(isSelected, this.dualState);
  }

  public void OnToggle()
  {
    if (!this.enabled || (Object) UIToggle.current == (Object) null || this.trigger != AnimationOrTween.Trigger.OnActivate && (this.trigger != AnimationOrTween.Trigger.OnActivateTrue || !UIToggle.current.value) && (this.trigger != AnimationOrTween.Trigger.OnActivateFalse || UIToggle.current.value))
      return;
    this.Play(UIToggle.current.value, this.dualState);
  }

  public void OnDragOver()
  {
    if (!this.enabled || !this.dualState)
      return;
    if ((Object) UICamera.currentTouch.dragged == (Object) this.gameObject)
    {
      this.Play(true, true);
    }
    else
    {
      if (!this.dragHighlight || this.trigger != AnimationOrTween.Trigger.OnPress)
        return;
      this.Play(true, true);
    }
  }

  public void OnDragOut()
  {
    if (!this.enabled || !this.dualState || !((Object) UICamera.hoveredObject != (Object) this.gameObject))
      return;
    this.Play(false, true);
  }

  public void OnDrop(GameObject go)
  {
    if (!this.enabled || this.trigger != AnimationOrTween.Trigger.OnPress || !((Object) UICamera.currentTouch.dragged != (Object) this.gameObject))
      return;
    this.Play(false, true);
  }

  public void Play(bool forward) => this.Play(forward, true);

  public void Play(bool forward, bool onlyIfDifferent)
  {
    if (!(bool) (Object) this.target && !(bool) (Object) this.animator)
      return;
    if (onlyIfDifferent)
    {
      if (this.mActivated == forward)
        return;
      this.mActivated = forward;
    }
    if (this.clearSelection && (Object) UICamera.selectedObject == (Object) this.gameObject)
      UICamera.selectedObject = (GameObject) null;
    int num = -(int) this.playDirection;
    AnimationOrTween.Direction playDirection = forward ? this.playDirection : (AnimationOrTween.Direction) num;
    ActiveAnimation activeAnimation = (bool) (Object) this.target ? ActiveAnimation.Play(this.target, this.clipName, playDirection, this.ifDisabledOnPlay, this.disableWhenFinished) : ActiveAnimation.Play(this.animator, this.clipName, playDirection, this.ifDisabledOnPlay, this.disableWhenFinished);
    if (!((Object) activeAnimation != (Object) null))
      return;
    if (this.resetOnPlay)
      activeAnimation.Reset();
    for (int index = 0; index < this.onFinished.Count; ++index)
      EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.OnFinished), true);
  }

  public void PlayForward() => this.Play(true);

  public void PlayReverse() => this.Play(false);

  public void OnFinished()
  {
    if (!((Object) UIPlayAnimation.current == (Object) null))
      return;
    UIPlayAnimation.current = this;
    EventDelegate.Execute(this.onFinished);
    if ((Object) this.eventReceiver != (Object) null && !string.IsNullOrEmpty(this.callWhenFinished))
      this.eventReceiver.SendMessage(this.callWhenFinished, SendMessageOptions.DontRequireReceiver);
    this.eventReceiver = (GameObject) null;
    UIPlayAnimation.current = (UIPlayAnimation) null;
  }
}
