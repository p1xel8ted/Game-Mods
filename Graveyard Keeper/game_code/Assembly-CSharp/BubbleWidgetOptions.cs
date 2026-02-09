// Decompiled with JetBrains decompiler
// Type: BubbleWidgetOptions
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BubbleWidgetOptions : BubbleWidget<BubbleWidgetDataOptions>
{
  [HideInInspector]
  [SerializeField]
  public UITable table;
  [SerializeField]
  [HideInInspector]
  public BubbleWidgetOptionsItem item_prefab;
  public List<BubbleWidgetOptionsItem> options = new List<BubbleWidgetOptionsItem>();

  public override void Init()
  {
    this.table = this.GetComponentInChildren<UITable>();
    this.item_prefab = this.GetComponentInChildren<BubbleWidgetOptionsItem>();
    this.item_prefab.Init();
    base.Init();
  }

  public override void Draw(BubbleWidgetDataOptions data)
  {
    if ((UnityEngine.Object) this.table == (UnityEngine.Object) null || (UnityEngine.Object) this.item_prefab == (UnityEngine.Object) null)
      this.Init();
    this.data = data;
    this.options.Clear();
    foreach (BubbleWidgetDataOptions.OptionData option1 in data.options)
    {
      BubbleWidgetDataOptions.OptionData option = option1;
      BubbleWidgetOptionsItem widgetOptionsItem = this.item_prefab.Copy<BubbleWidgetOptionsItem>();
      widgetOptionsItem.Draw(option.name, (System.Action) (() => this.OnOptionSelected(option.callback)), option.enabled);
      this.options.Add(widgetOptionsItem);
    }
    this.item_prefab.Deactivate<BubbleWidgetOptionsItem>();
    this.table.Reposition();
  }

  public void OnOptionSelected(System.Action opt_delegate)
  {
    opt_delegate.TryInvoke();
    this.data.on_hide.TryInvoke();
  }

  public override Vector2 GetSize()
  {
    if (!this.initialized || (UnityEngine.Object) this.ui_widget == (UnityEngine.Object) null)
      this.Init();
    Vector2 zero = Vector2.zero;
    if (!Application.isPlaying)
    {
      this.options.Clear();
      this.options.AddRange((IEnumerable<BubbleWidgetOptionsItem>) this.GetComponentsInChildren<BubbleWidgetOptionsItem>());
    }
    foreach (BubbleWidgetOptionsItem option in this.options)
    {
      Vector2 size = option.GetSize();
      zero.y += size.y;
      if ((double) size.x > (double) zero.x)
        zero.x = size.x;
    }
    zero.y -= (float) (2 * this.options.Count);
    this.ui_widget.width = Mathf.CeilToInt(zero.x);
    this.ui_widget.height = Mathf.CeilToInt(zero.y);
    return base.GetSize();
  }
}
