// Decompiled with JetBrains decompiler
// Type: RatCellGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using LinqTools;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RatCellGUI : BaseGUI
{
  public const string BUTTON_INSERT_RAT = "button_insert_rat";
  public const string BUTTON_EXTRACT_RAT = "button_extract_rat";
  public const string BUFF_OR_PERK_ITEM = "Buff or Perk item";
  public GameObject no_rat_label;
  public UILabel button_label;
  public UI2DSprite rat_sprite;
  public UITable ui_table;
  public UITable status_table;
  public UITable buffs_table;
  public UIButton train_button;
  public UI2DSprite train_button_inactive;
  public UILabel rat_descr;
  public UniversalObjectInfoGUI _universal_info;
  public WorldGameObject _rat_cell_wgo;
  public Item _rat;

  public override void Init()
  {
    this._universal_info = this.GetComponentInChildren<UniversalObjectInfoGUI>(true);
    foreach (RatBuffItemGUI componentsInChild in this.GetComponentsInChildren<RatBuffItemGUI>(true))
      componentsInChild.Init();
    base.Init();
  }

  public void Open(WorldGameObject rat_cell_wgo)
  {
    this.Open();
    this._rat_cell_wgo = rat_cell_wgo;
    this._rat = (Item) null;
    foreach (Item obj in this._rat_cell_wgo.data.inventory)
    {
      if (obj.definition.type == ItemDefinition.ItemType.Rat)
      {
        this._rat = obj;
        break;
      }
    }
    this._universal_info.Draw(this._rat_cell_wgo.GetUniversalObjectInfo());
    if (this._rat == null)
      this.DrawEmpty();
    else
      this.DrawRat();
  }

  public void Redraw()
  {
    if (this._rat == null)
      this.DrawEmpty();
    else
      this.DrawRat();
  }

  public void DrawEmpty()
  {
    this.no_rat_label.SetActive(true);
    this.button_label.text = GJL.L("button_insert_rat");
    this.UpdateBuffsList(new List<Item>());
    this.train_button.isEnabled = false;
    this.train_button_inactive.SetActive(true);
    this.rat_descr.text = string.Empty;
  }

  public void DrawRat()
  {
    this.no_rat_label.SetActive(false);
    this.button_label.text = GJL.L("button_extract_rat");
    this.UpdateBuffsList(this._rat.GetAllRatBuffs());
    this.train_button.isEnabled = true;
    this.train_button_inactive.SetActive(false);
    this.rat_descr.text = this._rat.GetRatDescription();
  }

  public void OnBuffInsertionButtonPressed()
  {
    if (this._rat == null)
      return;
    GUIElements.me.craft.OpenAsRatCell(this._rat_cell_wgo, this._rat);
  }

  public void OnRatInsertionButtonPressed()
  {
    if (this._rat == null)
    {
      WorldGameObject worldGameObject = MainGame.me.player;
      if (GlobalCraftControlGUI.is_global_control_active && (UnityEngine.Object) this._rat_cell_wgo != (UnityEngine.Object) null)
        worldGameObject = this._rat_cell_wgo;
      GUIElements.me.resource_picker.Open(worldGameObject, (InventoryWidget.ItemFilterDelegate) ((item, widget) =>
      {
        if (item == null || item.IsEmpty())
          return InventoryWidget.ItemFilterResult.Hide;
        return item.definition.type != ItemDefinition.ItemType.Rat ? InventoryWidget.ItemFilterResult.Inactive : InventoryWidget.ItemFilterResult.Active;
      }), new CraftResourcesSelectGUI.ResourceSelectResultDelegate(this.OnRatForInsertionPicked));
    }
    else
    {
      this._rat_cell_wgo.DropItem(this._rat);
      this._rat_cell_wgo.data.RemoveItem(new Item(this._rat.id));
      this._rat = (Item) null;
      this.Hide(false);
    }
  }

  public void OnRatForInsertionPicked(Item rat)
  {
    if (rat == null)
    {
      this.DrawEmpty();
    }
    else
    {
      this._rat_cell_wgo.AddToInventory(rat);
      foreach (Item obj in this._rat_cell_wgo.data.inventory)
      {
        if (obj.definition.type == ItemDefinition.ItemType.Rat)
        {
          this._rat = obj;
          break;
        }
      }
      bool flag = false;
      foreach (Inventory inventory in MainGame.me.player.GetMultiInventory().all)
      {
        if (inventory.data.inventory.Contains(rat))
        {
          if (!inventory.data.inventory.Remove(rat))
          {
            Debug.LogError((object) ("FATAL ERROR: error while removing rat item from multi inventory\n id=" + rat.id));
          }
          else
          {
            flag = true;
            break;
          }
        }
      }
      if (!flag)
        Debug.LogError((object) ("FATAL ERROR: not found rat item in multi inventory\n id=" + rat.id));
      this.DrawRat();
    }
  }

  public override void Hide(bool play_hide_sound = true) => base.Hide(play_hide_sound);

  public override void OnClosePressed() => this.Hide(true);

  public void UpdateBuffsList(List<Item> buffs)
  {
    List<RatBuffItemGUI> list1 = ((IEnumerable<RatBuffItemGUI>) this.status_table.GetComponentsInChildren<RatBuffItemGUI>(true)).ToList<RatBuffItemGUI>();
    List<RatBuffItemGUI> list2 = ((IEnumerable<RatBuffItemGUI>) this.buffs_table.GetComponentsInChildren<RatBuffItemGUI>(true)).ToList<RatBuffItemGUI>();
    if (list1.Count < 1)
      Debug.LogError((object) "FATAL ERROR: Not found PerkBuffItemGUI in RatCellGUI.status_table table! Call Bulat.");
    else if (list2.Count < 1)
    {
      Debug.LogError((object) "FATAL ERROR: Not found PerkBuffItemGUI in RatCellGUI.buffs_table table! Call Bulat.");
    }
    else
    {
      List<Item> objList1 = new List<Item>();
      List<Item> objList2 = new List<Item>();
      foreach (Item buff in buffs)
      {
        if (buff.definition.has_durability)
          objList1.Add(buff);
        else
          objList2.Add(buff);
      }
      if (objList1.Count == 0)
      {
        foreach (MonoBehaviour behaviour in list1)
          behaviour.SetActive(false);
      }
      else
      {
        int index1;
        for (index1 = 0; index1 < objList1.Count; ++index1)
        {
          if (index1 >= list1.Count)
            list1.Add(list1[list1.Count - 1].Copy<RatBuffItemGUI>(this.status_table.transform, name: "Buff or Perk item"));
          list1[index1].SetActive(true);
          try
          {
            list1[index1].Draw(objList1[index1]);
          }
          catch (ArgumentOutOfRangeException ex)
          {
            Debug.LogError((object) $"ArgumentOutOfRangeException: status_gui_items.Count={list1.Count}, status_items.Count={objList1.Count}, i={index1}. Error: {ex}");
            throw;
          }
        }
        for (int index2 = index1 + 1; index2 < list1.Count; ++index2)
          list1[index2].SetActive(false);
      }
      if (objList2.Count == 0)
      {
        foreach (MonoBehaviour behaviour in list2)
          behaviour.SetActive(false);
      }
      else
      {
        int index3;
        for (index3 = 0; index3 < objList2.Count; ++index3)
        {
          if (index3 >= list2.Count)
            list2.Add(list2[list2.Count - 1].Copy<RatBuffItemGUI>(this.buffs_table.transform, name: "Buff or Perk item"));
          list2[index3].SetActive(true);
          list2[index3].Draw(objList2[index3]);
        }
        for (int index4 = index3 + 1; index4 < list2.Count; ++index4)
          list2[index4].SetActive(false);
      }
      this.status_table.Reposition();
      this.buffs_table.Reposition();
      this.ui_table.Reposition();
    }
  }
}
