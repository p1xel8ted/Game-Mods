// Decompiled with JetBrains decompiler
// Type: UniversalObjectInfoGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

#nullable disable
public class UniversalObjectInfoGUI : MonoBehaviour
{
  public UITable table;
  public UILabel text_header;
  public UILabel text_descr;
  public UI2DSprite spr_icon;
  public UITableOrGrid right_items;
  public BaseItemCellGUI right_item_prefab;
  public UI2DSprite spr_icon_back;
  public GameObject top_separator;

  public void Draw(UniversalObjectInfo data)
  {
    if ((Object) this.text_header != (Object) null)
      this.text_header.text = data.header;
    if ((Object) this.text_descr != (Object) null)
    {
      this.text_descr.text = data.descr;
      this.text_descr.gameObject.SetActive(!string.IsNullOrEmpty(data.descr));
    }
    if ((Object) this.spr_icon != (Object) null)
    {
      this.spr_icon.sprite2D = EasySpritesCollection.GetSprite(data.icon, true);
      this.spr_icon.color = NGUIMath.HexToColor(uint.Parse(data.icon_color, NumberStyles.HexNumber));
      if ((Object) this.spr_icon_back != (Object) null)
      {
        string sprite_name = "craft_icon";
        if (data.icon.Contains("_grn_"))
          sprite_name += "_grn";
        else if (data.icon.Contains("_b_goc_"))
          sprite_name += "_goc";
        else if (data.icon.Contains("_b_sls_"))
          sprite_name += "_sls";
        this.spr_icon_back.sprite2D = EasySpritesCollection.GetSprite(sprite_name);
      }
    }
    if ((Object) this.top_separator != (Object) null && (Object) this.text_descr != (Object) null)
      this.top_separator.SetActive(string.IsNullOrEmpty(this.text_header.text) || string.IsNullOrEmpty(this.text_descr.text));
    if ((Object) this.right_item_prefab != (Object) null)
    {
      this.right_item_prefab.gameObject.SetActive(false);
      this.right_items.gameObject.SetActive(data.right_items.Count > 0);
      this.right_items.DestroyChildren<BaseItemCellGUI>(new BaseItemCellGUI[1]
      {
        this.right_item_prefab
      });
      foreach (KeyValuePair<string, int> rightItem in data.right_items)
      {
        BaseItemCellGUI baseItemCellGui = this.right_item_prefab.Copy<BaseItemCellGUI>();
        baseItemCellGui.DrawItem(rightItem.Key, rightItem.Value, false);
        if (string.IsNullOrEmpty(baseItemCellGui.container.counter.text))
          baseItemCellGui.container.counter.text = "0";
      }
      this.right_items.Reposition();
    }
    if (!((Object) this.table != (Object) null))
      return;
    this.table.Reposition();
    this.table.repositionNow = true;
  }
}
