// Decompiled with JetBrains decompiler
// Type: SimpleUITable
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class SimpleUITable : MonoBehaviour
{
  public int offset;
  public SimpleUITable.Alignment alignment;
  public bool dont_change_x;
  public bool resize_widget_height;
  public List<UIWidget> _widgets = new List<UIWidget>();
  public Dictionary<int, UIWidget> _widgets_hash = new Dictionary<int, UIWidget>();
  public Dictionary<int, UILabel> _labels_hash = new Dictionary<int, UILabel>();
  [NonSerialized]
  public bool use_hash;
  public UIWidget _widget;

  public UIWidget widget
  {
    get
    {
      if (!this.use_hash || (UnityEngine.Object) this._widget == (UnityEngine.Object) null)
        this._widget = this.GetComponent<UIWidget>();
      return this._widget;
    }
  }

  public void OnValidate()
  {
    if (!this.enabled)
      return;
    this.Reposition();
  }

  public void ClearHashes()
  {
    this._widgets_hash.Clear();
    this._labels_hash.Clear();
  }

  public UIWidget GetWidgetByTransform(Transform t)
  {
    int instanceId = t.GetInstanceID();
    UIWidget component;
    if (!this.use_hash || !this._widgets_hash.TryGetValue(instanceId, out component))
    {
      component = t.GetComponent<UIWidget>();
      if (this.use_hash)
        this._widgets_hash.Add(instanceId, component);
    }
    return component;
  }

  public UILabel GetLabelOfObject(MonoBehaviour o)
  {
    int instanceId = o.GetInstanceID();
    UILabel component;
    if (!this.use_hash || !this._labels_hash.TryGetValue(instanceId, out component))
    {
      component = o.GetComponent<UILabel>();
      if (this.use_hash)
        this._labels_hash.Add(instanceId, component);
    }
    return component;
  }

  public void Reposition()
  {
    Transform transform1 = this.transform;
    this._widgets.Clear();
    int childCount = transform1.childCount;
    int num1 = 0;
    int num2 = 0;
    int offset_y;
    for (int index = 0; index < childCount; ++index)
    {
      UIWidget widgetByTransform = this.GetWidgetByTransform(transform1.GetChild(index));
      if (!((UnityEngine.Object) widgetByTransform == (UnityEngine.Object) null) && widgetByTransform.gameObject.activeInHierarchy)
      {
        if (widgetByTransform.width > num1)
          num1 = widgetByTransform.width;
        num2 += this.CalculateWidgetHeight(widgetByTransform, out offset_y);
        if (index < childCount - 1)
          num2 += this.offset;
        this._widgets.Add(widgetByTransform);
      }
    }
    if (this._widgets.Count == 0)
      return;
    SimpleUITable.Alignment alignment1 = (SimpleUITable.Alignment) ((int) this.alignment % 10);
    SimpleUITable.Alignment alignment2 = (SimpleUITable.Alignment) (Mathf.FloorToInt((float) this.alignment / 10f) * 10);
    int num3 = 0;
    int num4 = 0;
    if (alignment2 != SimpleUITable.Alignment.Top)
      num3 += alignment2 == SimpleUITable.Alignment.Center ? Mathf.RoundToInt((float) num2 / 2f) : num2;
    for (int index = 0; index < this._widgets.Count; ++index)
    {
      UIWidget widget = this._widgets[index];
      int widgetHeight = this.CalculateWidgetHeight(widget, out offset_y);
      int num5 = Mathf.CeilToInt((float) widgetHeight / 2f);
      num4 += widgetHeight;
      int num6 = Mathf.CeilToInt((float) widget.height / 2f);
      int x = 0;
      if (alignment1 != SimpleUITable.Alignment.Center)
        x = (Mathf.CeilToInt((float) num1 / 2f) - num5) * (alignment1 == SimpleUITable.Alignment.Left ? -1 : 1);
      int num7 = num3 - num6;
      num3 -= widgetHeight + this.offset;
      if (index > 0)
        num4 += this.offset;
      switch (widget.pivot)
      {
        case UIWidget.Pivot.TopLeft:
        case UIWidget.Pivot.Top:
        case UIWidget.Pivot.TopRight:
          num7 += num6;
          break;
        case UIWidget.Pivot.BottomLeft:
        case UIWidget.Pivot.Bottom:
        case UIWidget.Pivot.BottomRight:
          num7 -= num6;
          break;
      }
      switch (widget.pivot)
      {
        case UIWidget.Pivot.TopLeft:
        case UIWidget.Pivot.Left:
        case UIWidget.Pivot.BottomLeft:
          x -= num5;
          break;
        case UIWidget.Pivot.TopRight:
        case UIWidget.Pivot.Right:
        case UIWidget.Pivot.BottomRight:
          x += num5;
          break;
      }
      Transform transform2 = widget.transform;
      if (this.dont_change_x)
        x = Mathf.RoundToInt(transform2.localPosition.x);
      transform2.localPosition = new Vector3((float) x, (float) (num7 + offset_y));
    }
    if (!this.resize_widget_height)
      return;
    this.widget.height = num4;
  }

  public int CalculateWidgetHeight(UIWidget w, out int offset_y)
  {
    int height = w.height;
    offset_y = 0;
    UILabel labelOfObject = this.GetLabelOfObject((MonoBehaviour) w);
    if (!((UnityEngine.Object) labelOfObject != (UnityEngine.Object) null))
      return height;
    offset_y -= Mathf.FloorToInt((float) labelOfObject.spacingY / 2f);
    return height;
  }

  public enum Alignment
  {
    NotSet = -1, // 0xFFFFFFFF
    Center = 0,
    Left = 1,
    Right = 2,
    Top = 10, // 0x0000000A
    TopLeft = 11, // 0x0000000B
    TopRight = 12, // 0x0000000C
    Bottom = 20, // 0x00000014
    BottomLeft = 21, // 0x00000015
    BottomRight = 22, // 0x00000016
  }
}
