// Decompiled with JetBrains decompiler
// Type: MixedCraftPresetGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MixedCraftPresetGUI : MonoBehaviour
{
  [HideInInspector]
  [NonSerialized]
  public BaseItemCellGUI[] items;
  public MixedCraftGUI _mixed_craft_gui;
  public bool _opened;
  public UIWidget _ui_widget;

  public UIWidget ui_widget => this._ui_widget;

  public void Init(
    MixedCraftGUI mixed_craft_gui,
    BaseItemCellGUI.OnItemAction on_over,
    BaseItemCellGUI.OnItemAction on_out,
    BaseItemCellGUI.OnItemAction on_select)
  {
    this.items = this.GetComponentsInChildren<BaseItemCellGUI>(true);
    foreach (BaseItemCellGUI baseItemCellGui in this.items)
      baseItemCellGui.SetCallbacks(on_over, on_out, on_select);
    this._mixed_craft_gui = mixed_craft_gui;
    this._ui_widget = this.GetComponent<UIWidget>();
    this.Deactivate<MixedCraftPresetGUI>();
  }

  public void Open(bool for_gamepad)
  {
    if (this._opened)
    {
      Debug.LogError((object) $"MixCraftGUI preset {this.name} allready opened");
    }
    else
    {
      this._opened = true;
      this.Activate<MixedCraftPresetGUI>();
      foreach (BaseItemCellGUI baseItemCellGui in this.items)
      {
        baseItemCellGui.DrawEmpty();
        baseItemCellGui.InitInputBehaviour();
      }
    }
  }

  public void ClearItems()
  {
    foreach (BaseItemCellGUI baseItemCellGui in this.items)
      baseItemCellGui.DrawEmpty();
  }

  public void Hide()
  {
    if (!this._opened)
      return;
    this.ClearItems();
    this._opened = false;
    this.Deactivate<MixedCraftPresetGUI>();
  }

  public void OnCloseButtonPressed() => this._mixed_craft_gui.OnClosePressed();

  public List<Item> GetSelectedItems()
  {
    List<Item> selectedItems = new List<Item>();
    foreach (BaseItemCellGUI baseItemCellGui in this.items)
      selectedItems.Add(baseItemCellGui.item);
    return selectedItems;
  }
}
