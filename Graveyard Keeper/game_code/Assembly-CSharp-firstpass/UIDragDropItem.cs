// Decompiled with JetBrains decompiler
// Type: UIDragDropItem
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("NGUI/Interaction/Drag and Drop Item")]
public class UIDragDropItem : MonoBehaviour
{
  public UIDragDropItem.Restriction restriction;
  public bool cloneOnDrag;
  [HideInInspector]
  public float pressAndHoldDelay = 1f;
  public bool interactable = true;
  [NonSerialized]
  public Transform mTrans;
  [NonSerialized]
  public Transform mParent;
  [NonSerialized]
  public Collider mCollider;
  [NonSerialized]
  public Collider2D mCollider2D;
  [NonSerialized]
  public UIButton mButton;
  [NonSerialized]
  public UIRoot mRoot;
  [NonSerialized]
  public UIGrid mGrid;
  [NonSerialized]
  public UITable mTable;
  [NonSerialized]
  public float mDragStartTime;
  [NonSerialized]
  public UIDragScrollView mDragScrollView;
  [NonSerialized]
  public bool mPressed;
  [NonSerialized]
  public bool mDragging;
  [NonSerialized]
  public UICamera.MouseOrTouch mTouch;
  public static List<UIDragDropItem> draggedItems = new List<UIDragDropItem>();

  public virtual void Awake()
  {
    this.mTrans = this.transform;
    this.mCollider = this.gameObject.GetComponent<Collider>();
    this.mCollider2D = this.gameObject.GetComponent<Collider2D>();
  }

  public virtual void OnEnable()
  {
  }

  public virtual void OnDisable()
  {
    if (!this.mDragging)
      return;
    this.StopDragging(UICamera.hoveredObject);
  }

  public virtual void Start()
  {
    this.mButton = this.GetComponent<UIButton>();
    this.mDragScrollView = this.GetComponent<UIDragScrollView>();
  }

  public virtual void OnPress(bool isPressed)
  {
    if (!this.interactable || UICamera.currentTouchID == -2 || UICamera.currentTouchID == -3)
      return;
    if (isPressed)
    {
      if (this.mPressed)
        return;
      this.mTouch = UICamera.currentTouch;
      this.mDragStartTime = RealTime.time + this.pressAndHoldDelay;
      this.mPressed = true;
    }
    else
    {
      if (!this.mPressed || this.mTouch != UICamera.currentTouch)
        return;
      this.mPressed = false;
      this.mTouch = (UICamera.MouseOrTouch) null;
    }
  }

  public virtual void Update()
  {
    if (this.restriction != UIDragDropItem.Restriction.PressAndHold || !this.mPressed || this.mDragging || (double) this.mDragStartTime >= (double) RealTime.time)
      return;
    this.StartDragging();
  }

  public virtual void OnDragStart()
  {
    if (!this.interactable || !this.enabled || this.mTouch != UICamera.currentTouch)
      return;
    if (this.restriction != UIDragDropItem.Restriction.None)
    {
      if (this.restriction == UIDragDropItem.Restriction.Horizontal)
      {
        Vector2 totalDelta = this.mTouch.totalDelta;
        if ((double) Mathf.Abs(totalDelta.x) < (double) Mathf.Abs(totalDelta.y))
          return;
      }
      else if (this.restriction == UIDragDropItem.Restriction.Vertical)
      {
        Vector2 totalDelta = this.mTouch.totalDelta;
        if ((double) Mathf.Abs(totalDelta.x) > (double) Mathf.Abs(totalDelta.y))
          return;
      }
      else if (this.restriction == UIDragDropItem.Restriction.PressAndHold)
        return;
    }
    this.StartDragging();
  }

  public virtual void StartDragging()
  {
    if (!this.interactable || this.mDragging)
      return;
    if (this.cloneOnDrag)
    {
      this.mPressed = false;
      GameObject gameObject = this.transform.parent.gameObject.AddChild(this.gameObject);
      gameObject.transform.localPosition = this.transform.localPosition;
      gameObject.transform.localRotation = this.transform.localRotation;
      gameObject.transform.localScale = this.transform.localScale;
      UIButtonColor component1 = gameObject.GetComponent<UIButtonColor>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
        component1.defaultColor = this.GetComponent<UIButtonColor>().defaultColor;
      if (this.mTouch != null && (UnityEngine.Object) this.mTouch.pressed == (UnityEngine.Object) this.gameObject)
      {
        this.mTouch.current = gameObject;
        this.mTouch.pressed = gameObject;
        this.mTouch.dragged = gameObject;
        this.mTouch.last = gameObject;
      }
      UIDragDropItem component2 = gameObject.GetComponent<UIDragDropItem>();
      component2.mTouch = this.mTouch;
      component2.mPressed = true;
      component2.mDragging = true;
      component2.Start();
      component2.OnClone(this.gameObject);
      component2.OnDragDropStart();
      if (UICamera.currentTouch == null)
        UICamera.currentTouch = this.mTouch;
      this.mTouch = (UICamera.MouseOrTouch) null;
      UICamera.Notify(this.gameObject, "OnPress", (object) false);
      UICamera.Notify(this.gameObject, "OnHover", (object) false);
    }
    else
    {
      this.mDragging = true;
      this.OnDragDropStart();
    }
  }

  public virtual void OnClone(GameObject original)
  {
  }

  public virtual void OnDrag(Vector2 delta)
  {
    if (!this.interactable || !this.mDragging || !this.enabled || this.mTouch != UICamera.currentTouch)
      return;
    if ((UnityEngine.Object) this.mRoot != (UnityEngine.Object) null)
      this.OnDragDropMove(delta * this.mRoot.pixelSizeAdjustment);
    else
      this.OnDragDropMove(delta);
  }

  public virtual void OnDragEnd()
  {
    if (!this.interactable || !this.enabled || this.mTouch != UICamera.currentTouch)
      return;
    this.StopDragging(UICamera.hoveredObject);
  }

  public void StopDragging(GameObject go = null)
  {
    if (!this.mDragging)
      return;
    this.mDragging = false;
    this.OnDragDropRelease(go);
  }

  public virtual void OnDragDropStart()
  {
    if (!UIDragDropItem.draggedItems.Contains(this))
      UIDragDropItem.draggedItems.Add(this);
    if ((UnityEngine.Object) this.mDragScrollView != (UnityEngine.Object) null)
      this.mDragScrollView.enabled = false;
    if ((UnityEngine.Object) this.mButton != (UnityEngine.Object) null)
      this.mButton.isEnabled = false;
    else if ((UnityEngine.Object) this.mCollider != (UnityEngine.Object) null)
      this.mCollider.enabled = false;
    else if ((UnityEngine.Object) this.mCollider2D != (UnityEngine.Object) null)
      this.mCollider2D.enabled = false;
    this.mParent = this.mTrans.parent;
    this.mRoot = NGUITools.FindInParents<UIRoot>(this.mParent);
    this.mGrid = NGUITools.FindInParents<UIGrid>(this.mParent);
    this.mTable = NGUITools.FindInParents<UITable>(this.mParent);
    if ((UnityEngine.Object) UIDragDropRoot.root != (UnityEngine.Object) null)
      this.mTrans.parent = UIDragDropRoot.root;
    this.mTrans.localPosition = this.mTrans.localPosition with
    {
      z = 0.0f
    };
    TweenPosition component1 = this.GetComponent<TweenPosition>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      component1.enabled = false;
    SpringPosition component2 = this.GetComponent<SpringPosition>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
      component2.enabled = false;
    NGUITools.MarkParentAsChanged(this.gameObject);
    if ((UnityEngine.Object) this.mTable != (UnityEngine.Object) null)
      this.mTable.repositionNow = true;
    if (!((UnityEngine.Object) this.mGrid != (UnityEngine.Object) null))
      return;
    this.mGrid.repositionNow = true;
  }

  public virtual void OnDragDropMove(Vector2 delta)
  {
    this.mTrans.localPosition += this.mTrans.InverseTransformDirection((Vector3) delta);
  }

  public virtual void OnDragDropRelease(GameObject surface)
  {
    if (!this.cloneOnDrag)
    {
      foreach (UIDragScrollView componentsInChild in this.GetComponentsInChildren<UIDragScrollView>())
        componentsInChild.scrollView = (UIScrollView) null;
      if ((UnityEngine.Object) this.mButton != (UnityEngine.Object) null)
        this.mButton.isEnabled = true;
      else if ((UnityEngine.Object) this.mCollider != (UnityEngine.Object) null)
        this.mCollider.enabled = true;
      else if ((UnityEngine.Object) this.mCollider2D != (UnityEngine.Object) null)
        this.mCollider2D.enabled = true;
      UIDragDropContainer inParents = (bool) (UnityEngine.Object) surface ? NGUITools.FindInParents<UIDragDropContainer>(surface) : (UIDragDropContainer) null;
      if ((UnityEngine.Object) inParents != (UnityEngine.Object) null)
      {
        this.mTrans.parent = (UnityEngine.Object) inParents.reparentTarget != (UnityEngine.Object) null ? inParents.reparentTarget : inParents.transform;
        this.mTrans.localPosition = this.mTrans.localPosition with
        {
          z = 0.0f
        };
      }
      else
        this.mTrans.parent = this.mParent;
      this.mParent = this.mTrans.parent;
      this.mGrid = NGUITools.FindInParents<UIGrid>(this.mParent);
      this.mTable = NGUITools.FindInParents<UITable>(this.mParent);
      if ((UnityEngine.Object) this.mDragScrollView != (UnityEngine.Object) null)
        this.Invoke("EnableDragScrollView", 1f / 1000f);
      NGUITools.MarkParentAsChanged(this.gameObject);
      if ((UnityEngine.Object) this.mTable != (UnityEngine.Object) null)
        this.mTable.repositionNow = true;
      if ((UnityEngine.Object) this.mGrid != (UnityEngine.Object) null)
        this.mGrid.repositionNow = true;
    }
    else
      NGUITools.Destroy((UnityEngine.Object) this.gameObject);
    this.OnDragDropEnd();
  }

  public virtual void OnDragDropEnd() => UIDragDropItem.draggedItems.Remove(this);

  public void EnableDragScrollView()
  {
    if (!((UnityEngine.Object) this.mDragScrollView != (UnityEngine.Object) null))
      return;
    this.mDragScrollView.enabled = true;
  }

  public void OnApplicationFocus(bool focus)
  {
    if (focus)
      return;
    this.StopDragging();
  }

  public enum Restriction
  {
    None,
    Horizontal,
    Vertical,
    PressAndHold,
  }
}
