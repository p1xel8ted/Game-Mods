// Decompiled with JetBrains decompiler
// Type: SoulContainerWidget
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SoulContainerWidget : MonoBehaviour
{
  public const string INSERT_SOUL_BUTTON_LABEL = "insert_soul";
  public const string TAKE_OUT_SOUL_BUTTON_LABEL = "takeout_soul";
  public const string SLOT_SOUL_ITEM_ID_PREFIX = "soul_placed_";
  [SerializeField]
  public UI2DSprite _item_icon;
  [SerializeField]
  public UI2DSprite _no_icon;
  [SerializeField]
  public GameObject _button_obj;
  [SerializeField]
  public UILabel _buttons_label;
  [SerializeField]
  public SoulExtractorPanelBarGUI _bar_gui;
  public bool _is_item_set;
  public Item _inserted_item;
  public WorldGameObject _wgo;
  public int _ordinal_number;

  public void Draw(int ordinal_number, WorldGameObject container_obj)
  {
    this._ordinal_number = ordinal_number;
    this._inserted_item = container_obj.data.GetItemByIndex(ordinal_number);
    this._is_item_set = this._inserted_item != null && !this._inserted_item.IsEmpty();
    this._wgo = container_obj;
    this._bar_gui.SetActive(false);
    this._item_icon.SetActive(false);
    this._no_icon.SetActive(false);
    foreach (UIButtonColor componentsInChild in this._button_obj.GetComponentsInChildren<UIButton>(true))
      componentsInChild.isEnabled = true;
    this._buttons_label.text = GJL.L(this._is_item_set ? "takeout_soul" : "insert_soul");
    if (!this._is_item_set)
    {
      this._no_icon.SetActive(true);
    }
    else
    {
      int num = MainGame.me.player.data.CanAddCount(this._inserted_item, true);
      foreach (UIButtonColor componentsInChild in this._button_obj.GetComponentsInChildren<UIButton>(true))
        componentsInChild.isEnabled = num > 0;
      this._item_icon.SetActive(true);
      this._bar_gui.SetActive(true);
      this._bar_gui.SetData(this._inserted_item.durability);
      this._bar_gui.Redraw();
    }
  }

  public void OnButtonPressed()
  {
    if (this._is_item_set)
    {
      MainGame.me.player.data.AddItem(this._inserted_item);
      this._wgo.data.RemoveItemByIndex(this._ordinal_number);
      this.Draw(this._ordinal_number, this._wgo);
      GUIElements.me.soul_container_gui.Redraw(this._wgo);
    }
    else
    {
      WorldGameObject worldGameObject = MainGame.me.player;
      if (GlobalCraftControlGUI.is_global_control_active && (Object) this._wgo != (Object) null)
        worldGameObject = this._wgo;
      GUIElements.me.resource_picker.Open(worldGameObject, (InventoryWidget.ItemFilterDelegate) ((itm, widget) => SoulContainerWidget.SoulItemsFilter(itm)), new CraftResourcesSelectGUI.ResourceSelectResultDelegate(this.OnItemPicked));
    }
  }

  public static InventoryWidget.ItemFilterResult SoulItemsFilter(Item item)
  {
    if (item == null || item.IsEmpty())
      return InventoryWidget.ItemFilterResult.Hide;
    return item.definition.type != ItemDefinition.ItemType.Soul ? InventoryWidget.ItemFilterResult.Inactive : InventoryWidget.ItemFilterResult.Active;
  }

  public void OnItemPicked(Item item)
  {
    if (item == null || item.IsEmpty())
      return;
    this._wgo.data.AddItemByIndex(item, this._ordinal_number);
    MainGame.me.player.data.RemoveItem(item);
    GUIElements.me.soul_container_gui.Redraw(this._wgo);
  }
}
