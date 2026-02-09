// Decompiled with JetBrains decompiler
// Type: UIDragScrollView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("NGUI/Interaction/Drag Scroll View")]
public class UIDragScrollView : MonoBehaviour
{
  public UIScrollView scrollView;
  public bool dragable;
  [SerializeField]
  [HideInInspector]
  public UIScrollView draggablePanel;
  public Transform mTrans;
  public UIScrollView mScroll;
  public bool mAutoFind;
  public bool mStarted;
  [NonSerialized]
  public bool mPressed;

  public void OnEnable()
  {
    this.mTrans = this.transform;
    if ((UnityEngine.Object) this.scrollView == (UnityEngine.Object) null && (UnityEngine.Object) this.draggablePanel != (UnityEngine.Object) null)
    {
      this.scrollView = this.draggablePanel;
      this.draggablePanel = (UIScrollView) null;
    }
    if (!this.mStarted || !this.mAutoFind && !((UnityEngine.Object) this.mScroll == (UnityEngine.Object) null))
      return;
    this.FindScrollView();
  }

  public void Start()
  {
    this.mStarted = true;
    this.FindScrollView();
  }

  public void FindScrollView()
  {
    UIScrollView inParents = NGUITools.FindInParents<UIScrollView>(this.mTrans);
    if ((UnityEngine.Object) this.scrollView == (UnityEngine.Object) null || this.mAutoFind && (UnityEngine.Object) inParents != (UnityEngine.Object) this.scrollView)
    {
      this.scrollView = inParents;
      this.mAutoFind = true;
    }
    else if ((UnityEngine.Object) this.scrollView == (UnityEngine.Object) inParents)
      this.mAutoFind = true;
    this.mScroll = this.scrollView;
  }

  public void OnDisable()
  {
    if (!this.mPressed || !((UnityEngine.Object) this.mScroll != (UnityEngine.Object) null) || !((UnityEngine.Object) this.mScroll.GetComponentInChildren<UIWrapContent>() == (UnityEngine.Object) null))
      return;
    this.mScroll.Press(false);
    this.mScroll = (UIScrollView) null;
  }

  public void OnPress(bool pressed)
  {
    this.mPressed = pressed;
    if (this.mAutoFind && (UnityEngine.Object) this.mScroll != (UnityEngine.Object) this.scrollView)
    {
      this.mScroll = this.scrollView;
      this.mAutoFind = false;
    }
    if (!(bool) (UnityEngine.Object) this.scrollView || !this.enabled || !NGUITools.GetActive(this.gameObject))
      return;
    this.scrollView.Press(pressed);
    if (pressed || !this.mAutoFind)
      return;
    this.scrollView = NGUITools.FindInParents<UIScrollView>(this.mTrans);
    this.mScroll = this.scrollView;
  }

  public void OnDrag(Vector2 delta)
  {
    if (!this.dragable || !(bool) (UnityEngine.Object) this.scrollView || !NGUITools.GetActive((Behaviour) this))
      return;
    this.scrollView.Drag();
  }

  public void OnScroll(float delta)
  {
    if (!(bool) (UnityEngine.Object) this.scrollView || !NGUITools.GetActive((Behaviour) this))
      return;
    this.scrollView.Scroll(delta);
  }

  public void OnPan(Vector2 delta)
  {
    if (!(bool) (UnityEngine.Object) this.scrollView || !NGUITools.GetActive((Behaviour) this))
      return;
    this.scrollView.OnPan(delta);
  }
}
