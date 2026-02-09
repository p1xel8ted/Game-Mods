// Decompiled with JetBrains decompiler
// Type: UIScrollBar
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("NGUI/Interaction/NGUI Scroll Bar")]
[ExecuteInEditMode]
public class UIScrollBar : UISlider
{
  [HideInInspector]
  [SerializeField]
  public float mSize = 1f;
  [HideInInspector]
  [SerializeField]
  public float mScroll;
  [SerializeField]
  [HideInInspector]
  public UIScrollBar.Direction mDir = UIScrollBar.Direction.Upgraded;

  [Obsolete("Use 'value' instead")]
  public float scrollValue
  {
    get => this.value;
    set => this.value = value;
  }

  public float barSize
  {
    get => this.mSize;
    set
    {
      float num = Mathf.Clamp01(value);
      if ((double) this.mSize == (double) num)
        return;
      this.mSize = num;
      this.mIsDirty = true;
      if (!NGUITools.GetActive((Behaviour) this))
        return;
      if ((UnityEngine.Object) UIProgressBar.current == (UnityEngine.Object) null && this.onChange != null)
      {
        UIProgressBar.current = (UIProgressBar) this;
        EventDelegate.Execute(this.onChange);
        UIProgressBar.current = (UIProgressBar) null;
      }
      this.ForceUpdate();
    }
  }

  public override void Upgrade()
  {
    if (this.mDir == UIScrollBar.Direction.Upgraded)
      return;
    this.mValue = this.mScroll;
    if (this.mDir == UIScrollBar.Direction.Horizontal)
      this.mFill = this.mInverted ? UIProgressBar.FillDirection.RightToLeft : UIProgressBar.FillDirection.LeftToRight;
    else
      this.mFill = this.mInverted ? UIProgressBar.FillDirection.BottomToTop : UIProgressBar.FillDirection.TopToBottom;
    this.mDir = UIScrollBar.Direction.Upgraded;
  }

  public override void OnStart()
  {
    base.OnStart();
    if (!((UnityEngine.Object) this.mFG != (UnityEngine.Object) null) || !((UnityEngine.Object) this.mFG.gameObject != (UnityEngine.Object) this.gameObject) || ((UnityEngine.Object) this.mFG.GetComponent<Collider>() != (UnityEngine.Object) null ? 1 : ((UnityEngine.Object) this.mFG.GetComponent<Collider2D>() != (UnityEngine.Object) null ? 1 : 0)) == 0)
      return;
    UIEventListener uiEventListener = UIEventListener.Get(this.mFG.gameObject);
    uiEventListener.onPress += new UIEventListener.BoolDelegate(((UISlider) this).OnPressForeground);
    uiEventListener.onDrag += new UIEventListener.VectorDelegate(((UISlider) this).OnDragForeground);
    this.mFG.autoResizeBoxCollider = true;
  }

  public override float LocalToValue(Vector2 localPos)
  {
    if (!((UnityEngine.Object) this.mFG != (UnityEngine.Object) null))
      return base.LocalToValue(localPos);
    float num1 = Mathf.Clamp01(this.mSize) * 0.5f;
    float t1 = num1;
    float t2 = 1f - num1;
    Vector3[] localCorners = this.mFG.localCorners;
    if (this.isHorizontal)
    {
      float num2 = Mathf.Lerp(localCorners[0].x, localCorners[2].x, t1);
      float num3 = Mathf.Lerp(localCorners[0].x, localCorners[2].x, t2);
      float num4 = num3 - num2;
      if ((double) num4 == 0.0)
        return this.value;
      return !this.isInverted ? (localPos.x - num2) / num4 : (num3 - localPos.x) / num4;
    }
    float num5 = Mathf.Lerp(localCorners[0].y, localCorners[1].y, t1);
    float num6 = Mathf.Lerp(localCorners[3].y, localCorners[2].y, t2);
    float num7 = num6 - num5;
    if ((double) num7 == 0.0)
      return this.value;
    return !this.isInverted ? (localPos.y - num5) / num7 : (num6 - localPos.y) / num7;
  }

  public override void ForceUpdate()
  {
    if ((UnityEngine.Object) this.mFG != (UnityEngine.Object) null)
    {
      this.mIsDirty = false;
      float a = Mathf.Clamp01(this.mSize) * 0.5f;
      double num1 = (double) Mathf.Lerp(a, 1f - a, this.value);
      float num2 = (float) num1 - a;
      float num3 = (float) num1 + a;
      if (this.isHorizontal)
        this.mFG.drawRegion = this.isInverted ? new Vector4(1f - num3, 0.0f, 1f - num2, 1f) : new Vector4(num2, 0.0f, num3, 1f);
      else
        this.mFG.drawRegion = this.isInverted ? new Vector4(0.0f, 1f - num3, 1f, 1f - num2) : new Vector4(0.0f, num2, 1f, num3);
      if (!((UnityEngine.Object) this.thumb != (UnityEngine.Object) null))
        return;
      Vector4 drawingDimensions = this.mFG.drawingDimensions;
      this.SetThumbPosition(this.mFG.cachedTransform.TransformPoint(new Vector3(Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, 0.5f), Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, 0.5f))));
    }
    else
      base.ForceUpdate();
  }

  public new enum Direction
  {
    Horizontal,
    Vertical,
    Upgraded,
  }
}
