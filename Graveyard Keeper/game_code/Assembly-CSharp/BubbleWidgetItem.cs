// Decompiled with JetBrains decompiler
// Type: BubbleWidgetItem
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BubbleWidgetItem : BubbleWidget<BubbleWidgetItemData>
{
  [HideInInspector]
  [SerializeField]
  public BaseItemCellGUI _item_gui;
  [HideInInspector]
  [SerializeField]
  public GameObject _back;

  public override void Init()
  {
    this._item_gui = this.GetComponent<BaseItemCellGUI>();
    this._back = this._item_gui.x1.back.gameObject;
    base.Init();
  }

  public override void Draw(BubbleWidgetItemData data)
  {
    if (!this.initialized)
      this.Init();
    this.data = data;
    if (data.IsEmpty())
    {
      this.Deactivate<BubbleWidgetItem>();
    }
    else
    {
      this._item_gui.DrawItem(data.item_id, data.counter, false, true, data.infinity_counter);
      if (!string.IsNullOrEmpty(data.icon_id))
      {
        ItemDefinition data1 = GameBalance.me.GetData<ItemDefinition>(data.item_id);
        if (data1 == null || !data1.is_big)
        {
          this._item_gui.DrawIcon(data.icon_id, hide_quality_icon: false);
          if ((Object) this._item_gui.container.counter != (Object) null)
            this._item_gui.container.counter.text = !data.infinity_counter ? (data.counter == 1 ? "" : data.counter.ToString()) : "∞";
        }
      }
      if ((Object) this._item_gui.container.counter != (Object) null)
        this._item_gui.container.counter.gameObject.SetActive(true);
      if ((Object) this._item_gui.quality_icon != (Object) null)
        this._item_gui.quality_icon.SetActive(data.show_quality);
      this._back.SetActive(data.show_back);
      this._item_gui.DrawCapIcon(data.cap_limit);
      this._item_gui.DrawGratitudeIcon(data.is_gratitude, data.is_enough_gratitude);
      this.GetSize();
    }
  }

  public override Vector2 GetSize()
  {
    if (!this.initialized)
      this.Init();
    if (this.data != null && this.data.IsEmpty())
      return Vector2.zero;
    bool activeSelf = this._back.activeSelf;
    this.ui_widget.width = activeSelf ? 48 /*0x30*/ : 34;
    this.ui_widget.height = activeSelf ? 48 /*0x30*/ : 34;
    return base.GetSize();
  }
}
