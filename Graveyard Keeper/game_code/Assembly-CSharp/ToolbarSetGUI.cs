// Decompiled with JetBrains decompiler
// Type: ToolbarSetGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class ToolbarSetGUI : MonoBehaviour
{
  public BaseItemCellGUI[] cells;
  public bool _initialized;
  public Item _equiped_item;

  public void Init(bool interaction_enabled)
  {
    if (this._initialized)
      return;
    this._initialized = true;
    this._equiped_item = new Item();
    foreach (BaseItemCellGUI cell in this.cells)
      cell.interaction_enabled = interaction_enabled;
  }

  public void Redraw()
  {
    if (!this._initialized)
      this.Init(true);
    GameSave save = MainGame.me.save;
    Item data = MainGame.me.player.data;
    for (int index = 0; index < this.cells.Length; ++index)
    {
      this._equiped_item.SetItemID(save.equipped_items[index]);
      int totalCount = data.GetTotalCount(this._equiped_item.id);
      this._equiped_item.value = totalCount;
      this.cells[index].DrawItem(this._equiped_item);
      if (!string.IsNullOrEmpty(this._equiped_item.id))
      {
        this.cells[index].container.counter.text = totalCount.ToString();
        this.cells[index].container.icon.alpha = totalCount > 0 ? 1f : 0.5f;
      }
    }
  }

  public void SetClickCallback(Action<int> callback)
  {
    for (int index1 = 0; index1 < this.cells.Length; ++index1)
    {
      int index = index1;
      this.cells[index1].SetCallbacks((GJCommons.VoidDelegate) null, (GJCommons.VoidDelegate) null, (GJCommons.VoidDelegate) (() =>
      {
        callback(index);
        this.cells[index].SetVisualyOveredState(true, false);
      }));
    }
  }
}
