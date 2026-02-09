// Decompiled with JetBrains decompiler
// Type: Tooltip
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Tooltip : MonoBehaviour
{
  public bool available = true;
  public TooltipBubbleGUI linked_tooltip;
  public UIWidget _widget;
  public bool _shown;
  public UIScrollView _scroll_view;
  public UIPanel _scroll_panel;
  public Transform _scroll_view_tf;
  public bool _is_scroll_table_item;
  public bool _initialized;
  public WidgetsBubbleGUI.Alignment alignment = WidgetsBubbleGUI.Alignment.Left;
  public BubbleWidgetDataContainer data = new BubbleWidgetDataContainer(WidgetsBubbleGUI.Alignment.Center, Array.Empty<BubbleWidgetData>());
  public string init_text = "";

  public bool has_info => this.data.has_data;

  public void Init()
  {
    if (this._initialized)
      return;
    this._widget = this.GetComponent<UIWidget>();
    this._scroll_view = this.GetComponentInParent<UIScrollView>();
    this._is_scroll_table_item = (UnityEngine.Object) this._scroll_view != (UnityEngine.Object) null;
    this._scroll_panel = this._is_scroll_table_item ? this._scroll_view.GetComponent<UIPanel>() : (UIPanel) null;
    this._scroll_view_tf = this._is_scroll_table_item ? this._scroll_view.transform : (Transform) null;
    this._initialized = true;
  }

  public void OnEnable()
  {
    if (string.IsNullOrEmpty(this.init_text))
      return;
    this.Init();
    this.SetText(GJL.L(this.init_text));
  }

  public bool IsScrolling()
  {
    return this._is_scroll_table_item && DOTween.IsTweening((object) this._scroll_view_tf);
  }

  public bool IsClippedByScrollView()
  {
    if (!this._is_scroll_table_item || (UnityEngine.Object) this._widget == (UnityEngine.Object) null)
      return false;
    Bounds bounds = this._widget.CalculateBounds(this._scroll_view_tf);
    Vector2 constrainOffset = (Vector2) this._scroll_panel.CalculateConstrainOffset((Vector2) bounds.min, (Vector2) bounds.max);
    return (double) Mathf.Abs(constrainOffset.x) > (double) this._widget.width / 3.0 || (double) Mathf.Abs(constrainOffset.y) > (double) this._widget.height / 3.0;
  }

  public void Show(bool for_gamepad)
  {
    if (this._shown || !this.available || !this.has_info || !this.data.has_data)
      return;
    this.data.alignment = this.alignment;
    this.linked_tooltip = TooltipBubbleGUI.Show(this.data, for_gamepad, this.GetComponent<Collider2D>());
    if ((UnityEngine.Object) this.linked_tooltip == (UnityEngine.Object) null)
      return;
    this._shown = true;
  }

  public void Hide()
  {
    if (!this._shown)
      return;
    this._shown = false;
    if (!((UnityEngine.Object) this.linked_tooltip != (UnityEngine.Object) null))
      return;
    this.linked_tooltip.DestroyBubble();
    this.linked_tooltip = (TooltipBubbleGUI) null;
  }

  public void SetText(string text, UITextStyles.TextStyle style = UITextStyles.TextStyle.Usual)
  {
    this.Init();
    this.available = true;
    this.data.ClearData();
    this.data.SetData((BubbleWidgetData) new BubbleWidgetTextData(text, style));
  }

  public void SetData(params BubbleWidgetData[] data_params)
  {
    this.Init();
    this.available = true;
    this.data.SetData(data_params);
  }

  public void SetData(List<BubbleWidgetData> tooltip_datas)
  {
    this.Init();
    this.available = true;
    this.data.SetData(tooltip_datas);
  }

  public void AddData(BubbleWidgetData tooltip_data)
  {
    this.Init();
    this.available = true;
    this.data.AddData(tooltip_data);
  }

  public void ClearData() => this.data.ClearData();

  public void SetCraftDefinition(CraftDefinition definition)
  {
    this.available = true;
    this.data.ClearData();
  }

  public void OnDestroy() => this.Hide();
}
