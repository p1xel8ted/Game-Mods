// Decompiled with JetBrains decompiler
// Type: WidgetsBubbleGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class WidgetsBubbleGUI : BaseBubbleGUI
{
  public const int MIN_WIDTH = 16 /*0x10*/;
  public const int MIN_HEIGHT = 6;
  public static Dictionary<System.Type, BubbleWidgetBase> widget_prefabs = new Dictionary<System.Type, BubbleWidgetBase>();
  public bool for_gamepad;
  public Vector3 pos;
  public Vector3 alternative_pos = Vector3.zero;
  public Collider2D linked_collider;
  [HideInInspector]
  [SerializeField]
  public UIWidget widget;
  [HideInInspector]
  [SerializeField]
  public UITable ui_table;
  [HideInInspector]
  [SerializeField]
  public SimpleUITable simple_table;
  public UITableOrGrid table;
  public List<BubbleWidgetBase> bubble_widgets = new List<BubbleWidgetBase>();
  public bool waiting_for_reposition;
  public bool _recalc_on_late_update;
  public BubbleWidgetDataContainer _last_drawn_data;
  public Dictionary<int, UILabel> _labels_hash = new Dictionary<int, UILabel>();
  [CompilerGenerated]
  public BubbleWidgetDataContainer \u003Cdata\u003Ek__BackingField;
  public UILabel _label;

  public BubbleWidgetDataContainer data
  {
    get => this.\u003Cdata\u003Ek__BackingField;
    set => this.\u003Cdata\u003Ek__BackingField = value;
  }

  public UILabel GetLabelOfObject(MonoBehaviour o)
  {
    int instanceId = o.GetInstanceID();
    UILabel component;
    if (!this._labels_hash.TryGetValue(instanceId, out component))
    {
      component = o.GetComponent<UILabel>();
      this._labels_hash.Add(instanceId, component);
    }
    return component;
  }

  public UILabel label
  {
    get
    {
      if ((UnityEngine.Object) this._label == (UnityEngine.Object) null || (UnityEngine.Object) this.simple_table != (UnityEngine.Object) null && !this.simple_table.use_hash)
        this._label = this.GetComponent<UILabel>();
      return this._label;
    }
  }

  public override void Init()
  {
    this.ui_table = this.GetComponentInChildren<UITable>(true);
    this.simple_table = this.GetComponentInChildren<SimpleUITable>(true);
    if ((UnityEngine.Object) this.simple_table != (UnityEngine.Object) null)
      this.simple_table.use_hash = true;
    this.widget = this.GetComponent<UIWidget>();
    this.bubble_widgets.AddRange((IEnumerable<BubbleWidgetBase>) this.GetComponentsInChildren<BubbleWidgetBase>(true));
    this.Clear();
    base.Init();
  }

  public void OnEnable()
  {
    if ((UnityEngine.Object) this.simple_table != (UnityEngine.Object) null)
    {
      this.simple_table.Reposition();
      this.simple_table.use_hash = true;
    }
    else
    {
      if (!((UnityEngine.Object) this.ui_table != (UnityEngine.Object) null))
        return;
      this.ui_table.Reposition();
    }
  }

  public void InitWidgetsContainer()
  {
    WidgetsBubbleGUI.widget_prefabs.Clear();
    foreach (BubbleWidgetBase componentsInChild in this.GetComponentsInChildren<BubbleWidgetBase>())
      WidgetsBubbleGUI.widget_prefabs.Add(componentsInChild.GetWidgetType(), componentsInChild);
    if (!Application.isPlaying)
      return;
    this.gameObject.SetActive(false);
  }

  public bool HasWidgetOfType(System.Type type)
  {
    foreach (BubbleWidgetBase bubbleWidget in this.bubble_widgets)
    {
      if (System.Type.op_Equality(bubbleWidget.GetWidgetType(), type))
        return true;
    }
    return false;
  }

  public BubbleWidget<T> GetWidget<T>() where T : BubbleWidgetData
  {
    foreach (BubbleWidgetBase bubbleWidget in this.bubble_widgets)
    {
      if (System.Type.op_Equality(bubbleWidget.GetWidgetType(), typeof (T)))
        return bubbleWidget as BubbleWidget<T>;
    }
    return (BubbleWidget<T>) null;
  }

  public void Show(BubbleWidgetDataContainer data_container, bool force_redraw = false)
  {
    this.data = data_container;
    if (force_redraw)
      this._last_drawn_data = (BubbleWidgetDataContainer) null;
    this.Redraw();
  }

  public void Redraw(bool is_hint = false)
  {
    bool flag1 = false;
    if (!this.initialized || (UnityEngine.Object) this.ui_table == (UnityEngine.Object) null && (UnityEngine.Object) this.simple_table == (UnityEngine.Object) null || (UnityEngine.Object) this.widget == (UnityEngine.Object) null)
    {
      this.Init();
      flag1 = true;
    }
    if (!flag1)
    {
      flag1 = true;
      if (this._last_drawn_data?.data_list != null && this.data?.data_list != null && this.data.data_list.Count == this._last_drawn_data.data_list.Count && !((UnityEngine.Object) this.data.linked_wgo != (UnityEngine.Object) this._last_drawn_data.linked_wgo) && this.data.alignment == this._last_drawn_data.alignment)
      {
        bool flag2 = false;
        for (int index = 0; index < this.data.data_list.Count; ++index)
        {
          BubbleWidgetData data1 = this.data.data_list[index];
          BubbleWidgetData data2 = this._last_drawn_data.data_list[index];
          if (data1.widget_id != data2.widget_id)
          {
            flag2 = true;
            break;
          }
          if (System.Type.op_Inequality(data1.GetType(), data2.GetType()))
          {
            flag2 = true;
            break;
          }
          if (data1.IsEmpty() != data2.IsEmpty())
          {
            flag2 = true;
            break;
          }
        }
        if (!flag2)
          flag1 = false;
      }
    }
    if (flag1)
    {
      this.Clear();
      Transform parent = (UnityEngine.Object) this.simple_table != (UnityEngine.Object) null ? this.simple_table.transform : this.ui_table.transform;
      this.SortWidgets();
      foreach (BubbleWidgetData data in this.data.data_list)
      {
        if (!data.IsEmpty())
        {
          System.Type type = data.GetType();
          if (WidgetsBubbleGUI.widget_prefabs.ContainsKey(type))
          {
            BubbleWidgetBase bubbleWidgetBase = WidgetsBubbleGUI.widget_prefabs[type].Copy<BubbleWidgetBase>(parent);
            GJL.EnsureChildLabelsHasCorrectFont(bubbleWidgetBase.gameObject, false);
            GJL.ApplyCustomFontSettings(bubbleWidgetBase.gameObject);
            bubbleWidgetBase.BaseDraw(data);
            this.bubble_widgets.Add(bubbleWidgetBase);
          }
        }
      }
      if ((UnityEngine.Object) this.simple_table == (UnityEngine.Object) null && (UnityEngine.Object) this.ui_table == (UnityEngine.Object) null)
        Debug.LogError((object) (this.name + " widget hasn't any table"), (UnityEngine.Object) this);
      if (!(this is InteractionBubbleGUI))
      {
        switch (this.data.alignment)
        {
          case WidgetsBubbleGUI.Alignment.Center:
            if ((UnityEngine.Object) this.simple_table != (UnityEngine.Object) null)
              this.simple_table.alignment = SimpleUITable.Alignment.Top;
            if ((UnityEngine.Object) this.ui_table != (UnityEngine.Object) null)
            {
              this.ui_table.cellAlignment = UIWidget.Pivot.Top;
              break;
            }
            break;
          case WidgetsBubbleGUI.Alignment.Left:
            if ((UnityEngine.Object) this.simple_table != (UnityEngine.Object) null)
              this.simple_table.alignment = SimpleUITable.Alignment.TopLeft;
            if ((UnityEngine.Object) this.ui_table != (UnityEngine.Object) null)
            {
              this.ui_table.cellAlignment = UIWidget.Pivot.TopLeft;
              break;
            }
            break;
          case WidgetsBubbleGUI.Alignment.Right:
            if ((UnityEngine.Object) this.simple_table != (UnityEngine.Object) null)
              this.simple_table.alignment = SimpleUITable.Alignment.TopRight;
            if ((UnityEngine.Object) this.ui_table != (UnityEngine.Object) null)
            {
              this.ui_table.cellAlignment = UIWidget.Pivot.TopRight;
              break;
            }
            break;
        }
      }
      this.UpdateSizeAndWidgetsPositions();
      this.all_widgets = this.GetComponentsInChildren<UIWidget>();
      this.OnContentChanged();
      if ((UnityEngine.Object) this.simple_table != (UnityEngine.Object) null)
        this.simple_table.Reposition();
      this.Update();
      this._recalc_on_late_update = true;
      foreach (UIRect componentsInChild in this.GetComponentsInChildren<UIWidget>(true))
        componentsInChild.UpdateAnchors();
      this._last_drawn_data = this.data.GetCopy();
    }
    else
    {
      int index = 0;
      foreach (BubbleWidgetData data in this.data.data_list)
      {
        this.bubble_widgets[index].BaseDraw(data);
        this._last_drawn_data.data_list[index] = data;
        ++index;
      }
      this.Update();
    }
  }

  public void MakeBottomAligned()
  {
    switch (this.data.alignment)
    {
      case WidgetsBubbleGUI.Alignment.Center:
        if ((UnityEngine.Object) this.simple_table != (UnityEngine.Object) null)
          this.simple_table.alignment = SimpleUITable.Alignment.Bottom;
        if ((UnityEngine.Object) this.ui_table != (UnityEngine.Object) null)
        {
          this.ui_table.cellAlignment = UIWidget.Pivot.Bottom;
          break;
        }
        break;
      case WidgetsBubbleGUI.Alignment.Left:
        if ((UnityEngine.Object) this.simple_table != (UnityEngine.Object) null)
          this.simple_table.alignment = SimpleUITable.Alignment.BottomLeft;
        if ((UnityEngine.Object) this.ui_table != (UnityEngine.Object) null)
        {
          this.ui_table.cellAlignment = UIWidget.Pivot.BottomLeft;
          break;
        }
        break;
      case WidgetsBubbleGUI.Alignment.Right:
        if ((UnityEngine.Object) this.simple_table != (UnityEngine.Object) null)
          this.simple_table.alignment = SimpleUITable.Alignment.BottomRight;
        if ((UnityEngine.Object) this.ui_table != (UnityEngine.Object) null)
        {
          this.ui_table.cellAlignment = UIWidget.Pivot.BottomRight;
          break;
        }
        break;
    }
    this.UpdateSizeAndWidgetsPositions();
  }

  public void UpdateSizeAndWidgetsPositions()
  {
    if (!this.initialized || (UnityEngine.Object) this.ui_table == (UnityEngine.Object) null && (UnityEngine.Object) this.simple_table == (UnityEngine.Object) null || (UnityEngine.Object) this.widget == (UnityEngine.Object) null)
      this.Init();
    this.UpdateSize();
    this.Reposition();
    this.waiting_for_reposition = true;
    this.widget.UpdateAnchors();
  }

  public void UpdateSize()
  {
    Vector2 zero = Vector2.zero;
    foreach (BubbleWidgetBase bubbleWidget in this.bubble_widgets)
    {
      if (bubbleWidget.gameObject.activeSelf)
      {
        Vector2 size = bubbleWidget.GetSize();
        if ((double) size.x > (double) zero.x)
          zero.x = size.x;
        zero.y += size.y;
      }
    }
    if ((double) zero.x < 16.0)
      zero.x = 16f;
    if ((double) zero.y < 6.0)
      zero.y = 6f;
    this.widget.width = Mathf.CeilToInt(zero.x);
    this.widget.height = Mathf.CeilToInt(zero.y / 2f) * 2;
  }

  public void Reposition()
  {
    if ((UnityEngine.Object) this.simple_table != (UnityEngine.Object) null)
      this.simple_table.Reposition();
    else
      this.ui_table.Reposition();
    this.waiting_for_reposition = true;
    foreach (BubbleWidgetBase bubbleWidget in this.bubble_widgets)
    {
      if (!((UnityEngine.Object) bubbleWidget == (UnityEngine.Object) null) && this.data != null)
      {
        UILabel labelOfObject = this.GetLabelOfObject((MonoBehaviour) bubbleWidget);
        if (this.data.alignment == WidgetsBubbleGUI.Alignment.Left && (UnityEngine.Object) labelOfObject != (UnityEngine.Object) null)
          labelOfObject.GetComponent<UIWidget>().pivot = UIWidget.Pivot.Left;
      }
    }
  }

  public virtual void Update()
  {
    foreach (BubbleWidgetBase bubbleWidget in this.bubble_widgets)
      bubbleWidget.UpdateWidget();
    if ((UnityEngine.Object) this.linked_tf != (UnityEngine.Object) null)
      return;
    if (this.for_gamepad)
    {
      if ((UnityEngine.Object) this.linked_collider == (UnityEngine.Object) null)
      {
        this.DestroyBubble();
        return;
      }
      if (this.widget.alpha.EqualsTo(0.0f))
        this.widget.alpha = 1f;
      Bounds bounds = this.linked_collider.bounds;
      this.pos = (Vector3) new Vector2(bounds.center.x, bounds.max.y);
      this.alternative_pos = (Vector3) new Vector2(bounds.center.x, bounds.min.y);
    }
    else
      this.pos = MainGame.me.gui_cam.ScreenToWorldPoint(Input.mousePosition);
    this.UpdateBubble(this.pos, false, this.alternative_pos, true);
  }

  public override void LateUpdate()
  {
    if (this.waiting_for_reposition)
    {
      this.waiting_for_reposition = false;
      this.Reposition();
    }
    base.LateUpdate();
    if (!this._recalc_on_late_update)
      return;
    this._recalc_on_late_update = false;
    this.OnContentChanged();
  }

  public void LinkColliderForGamepad(bool for_gamepad, Collider2D tooltip_collider)
  {
    if (!for_gamepad || (UnityEngine.Object) tooltip_collider == (UnityEngine.Object) null)
      return;
    this.for_gamepad = true;
    this.linked_collider = tooltip_collider;
  }

  public void LinkTransform(Transform target)
  {
    this.linked_tf = target;
    this.LateUpdate();
  }

  public void Clear()
  {
    if (this.bubble_widgets == null)
    {
      this.bubble_widgets = new List<BubbleWidgetBase>();
    }
    else
    {
      foreach (BubbleWidgetBase bubbleWidget in this.bubble_widgets)
      {
        bubbleWidget.gameObject.SetActive(false);
        bubbleWidget.DestroyGO<BubbleWidgetBase>();
      }
      this.bubble_widgets.Clear();
      this.simple_table?.ClearHashes();
    }
  }

  public static WidgetsBubbleGUI bubble
  {
    get
    {
      return (WidgetsBubbleGUI) UnityEngine.Object.FindObjectOfType<GUIElements>().GetComponentInChildren<InteractionBubbleGUI>(true);
    }
  }

  public virtual void SortWidgets()
  {
  }

  public enum Alignment
  {
    Center,
    Left,
    Right,
  }
}
