// Decompiled with JetBrains decompiler
// Type: BagInventoryWidget
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BagInventoryWidget : InventoryWidget
{
  public UI2DSprite bag_type_sprite;
  public UIGrid inventory_table;
  public static List<string> bag_type_sprites;

  public override void Init()
  {
    base.Init();
    BagInventoryWidget.bag_type_sprites = new List<string>()
    {
      string.Empty,
      string.Empty,
      "char_wndw_bag_inventory_bag_i_01",
      "char_wndw_bag_inventory_bag_i_02",
      "char_wndw_bag_inventory_bag_i_04",
      "char_wndw_bag_inventory_bag_i_03",
      "char_wndw_bag_inventory_bag_i_05",
      "char_wndw_bag_inventory_bag_i_06",
      "char_wndw_bag_inventory_bag_i_07"
    };
  }

  public override void Open(
    Inventory inventory,
    bool for_gamepad,
    int navigation_group = 0,
    int navigation_sub_group = 0,
    bool dont_show_empty_rows = false,
    int custom_line_length = -1)
  {
    base.Open(inventory, for_gamepad, navigation_group, navigation_sub_group, dont_show_empty_rows, custom_line_length);
    ItemDefinition definition = inventory?.data?.definition;
    if (definition == null)
      Debug.LogError((object) "Bag definition is null!");
    else
      this.SetBagTypeSprite(definition.bag_type);
  }

  public void SetBagTypeSprite(ItemDefinition.BagType bag_type)
  {
    int index = (int) bag_type;
    if (index >= BagInventoryWidget.bag_type_sprites.Count)
      Debug.LogError((object) "Bag types more than bag icons!");
    string bagTypeSprite = BagInventoryWidget.bag_type_sprites[index];
    UnityEngine.Sprite sprite = (UnityEngine.Sprite) null;
    if (!string.IsNullOrEmpty(bagTypeSprite))
      sprite = EasySpritesCollection.GetSprite(bagTypeSprite);
    this.bag_type_sprite.sprite2D = sprite;
  }

  public override void Redraw()
  {
    base.Redraw();
    this.RecalculatWidgetSizeAndPosition();
  }

  public void RecalculatWidgetSizeAndPosition()
  {
    if ((Object) this.inventory_table == (Object) null)
      Debug.LogError((object) "Inventory table is null!");
    else if (this.inventory_item == null || this.inventory_item.IsEmpty() || this.inventory_item.definition == null)
    {
      Debug.LogError((object) "BagInventoryWidget error: wrong inventory_item!");
    }
    else
    {
      ItemDefinition definition = this.inventory_item.definition;
      int bagSizeX = definition.bag_size_x;
      int bagSizeY = definition.bag_size_y;
      this.inventory_table.maxPerLine = bagSizeX;
      float x = (float) (110.0 - (double) (bagSizeX - 1) * 21.0);
      Transform transform = this.inventory_table.transform;
      Vector2 localPosition = (Vector2) transform.localPosition;
      transform.localPosition = (Vector3) new Vector2(x, localPosition.y);
      this.inventory_table.Reposition();
      this.GetComponent<UIWidget>().height = 29 + bagSizeY * 42 + this.auto_height_offset;
    }
  }
}
