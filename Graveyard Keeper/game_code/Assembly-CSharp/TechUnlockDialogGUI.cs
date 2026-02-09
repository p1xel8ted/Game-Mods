// Decompiled with JetBrains decompiler
// Type: TechUnlockDialogGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class TechUnlockDialogGUI : BaseGUI
{
  public UILabel label_header;
  public UILabel label_cost;
  public Color c_price_normal;
  public Color c_price_not_enough;
  public Color c_price_disabled;
  public UIWidget[] widgets_to_update;
  public List<TechTreeGUIUnlockItem> _unlocks = new List<TechTreeGUIUnlockItem>();
  public DialogButtonsGUI _dialog_buttons;
  public GameObject subheader_container;
  public UILabel subheader;
  public UITableOrGrid table;
  public UIWidget background;
  public bool _opened_as_infobox;
  public bool _needs_resize;

  public override void Init()
  {
    this._dialog_buttons = this.GetComponentInChildren<DialogButtonsGUI>(true);
    TechTreeGUIUnlockItem componentInChildren = this.GetComponentInChildren<TechTreeGUIUnlockItem>(true);
    this._unlocks.Add(componentInChildren);
    for (int index = 1; index < 3; ++index)
    {
      TechTreeGUIUnlockItem behaviour = componentInChildren.Copy<TechTreeGUIUnlockItem>();
      behaviour.Deactivate<TechTreeGUIUnlockItem>();
      this._unlocks.Add(behaviour);
    }
    base.Init();
  }

  public void Open(
    TechDefinition tech,
    GJCommons.VoidDelegate on_hide,
    bool forced_unlock = false,
    bool reveal_tech = false,
    bool show_tech_tree_after = false,
    bool pseudotech = false)
  {
    Debug.Log((object) $"TechUnlockDialogGUI, tech = {(tech == null ? "NULL" : tech.id)}, forced_unlock = {forced_unlock.ToString()}, reveal_tech = {reveal_tech.ToString()}, show_tech_tree_after = {show_tech_tree_after.ToString()}");
    this.Open();
    this._opened_as_infobox = false;
    TooltipsManager.Redraw();
    this.subheader_container.SetActive(forced_unlock);
    this.label_header.text = GJL.L(tech.id);
    this.subheader.text = GJL.L("youve_unlocked_tech");
    int state = (int) tech.GetState();
    bool flag1 = MainGame.me.player.IsEnough(tech.price);
    bool flag2 = state == 1;
    List<TechUnlock> visibleUnlocksList = tech.GetVisibleUnlocksList();
    for (int index = 0; index < this._unlocks.Count; ++index)
      this._unlocks[index].Draw(index < visibleUnlocksList.Count ? visibleUnlocksList[index] : (TechUnlock) null, false);
    Color clr_normal = flag2 ? this.c_price_disabled : this.c_price_normal;
    if (flag1)
    {
      this.label_cost.color = clr_normal;
      this.label_cost.text = tech.price.ToPrintableString();
    }
    else
    {
      this.label_cost.color = Color.white;
      this.label_cost.text = tech.price.ToPrintableString(true, clr_normal, this.c_price_not_enough);
    }
    if (forced_unlock)
    {
      Sounds.PlaySound("unlock");
      this.label_cost.text = "";
      this._dialog_buttons.Set("OK", (GJCommons.VoidDelegate) (() =>
      {
        if (!pseudotech)
        {
          if (reveal_tech)
            MainGame.me.save.RevealHiddenTech(tech.id);
          else
            MainGame.me.save.UnlockTech(tech.id);
        }
        this.Hide();
        if (!show_tech_tree_after)
          return;
        GUIElements.me.tech_tree.OpenTech(tech.id);
      }));
    }
    else
    {
      if (reveal_tech)
        Debug.LogError((object) ("Reveal tech dialog should be forced, not optional, tech id = " + tech.id));
      this._dialog_buttons.Set("unlock", (GJCommons.VoidDelegate) (() =>
      {
        Sounds.PlaySound("unlock");
        MainGame.me.save.BuyTech(tech.id);
        this.Hide();
      }), "cancel", (GJCommons.VoidDelegate) (() => this.Hide()));
      this._dialog_buttons.SetEnabled(flag1 && !flag2);
    }
    this.SetOnHide(on_hide);
    this.UpdatePixelPerfect();
    foreach (SimpleUITable componentsInChild in this.GetComponentsInChildren<SimpleUITable>())
      componentsInChild.Reposition();
    foreach (UITable componentsInChild in this.GetComponentsInChildren<UITable>())
      componentsInChild.Reposition();
    this.table.Reposition();
    this.UpdateAllAnchors();
    this._needs_resize = true;
  }

  public void OpenAsItemsList(
    string sub_header_translated,
    List<Item> items,
    GJCommons.VoidDelegate on_hide)
  {
    this.Open();
    TooltipsManager.Redraw();
    this.label_cost.text = "";
    this._opened_as_infobox = true;
    this.SetOnHide(on_hide);
    this.label_header.text = GJL.L("information");
    this.subheader.text = sub_header_translated;
    this._dialog_buttons.Set("OK", (GJCommons.VoidDelegate) (() => this.Hide()));
    for (int index = 0; index < this._unlocks.Count; ++index)
    {
      if (index >= items.Count)
      {
        this._unlocks[index].Draw((TechUnlock.TechUnlockData) null);
      }
      else
      {
        ItemDefinition definition = items[index].definition;
        if (definition == null)
        {
          Debug.LogWarning((object) ("Item definition is null for id = " + items[index].id));
          this._unlocks[index].Draw((TechUnlock.TechUnlockData) null);
        }
        else
          this._unlocks[index].Draw(new TechUnlock.TechUnlockData()
          {
            sprite = EasySpritesCollection.GetSprite(definition.GetIcon()),
            name = definition.GetItemName(),
            quality_icon = definition.TryGetQualitySprite(),
            description = definition.GetItemDescription()
          });
      }
    }
    this.table.Reposition();
    this.UpdatePixelPerfect();
    this.UpdateAllAnchors();
    if ((Object) this.background != (Object) null)
    {
      this.background.UpdateAnchors();
      this.UpdateAllAnchors();
    }
    this._needs_resize = true;
  }

  public new void LateUpdate()
  {
    if (!this._needs_resize)
      return;
    this._needs_resize = false;
    this.UpdateWidgets();
  }

  public override bool OnPressedBack()
  {
    if (!this._opened_as_infobox)
      return base.OnPressedBack();
    this.OnClosePressed();
    return true;
  }

  public void UpdateWidgets()
  {
    foreach (UIWidget componentsInChild in this.GetComponentsInChildren<UIWidget>(true))
    {
      componentsInChild.Update();
      componentsInChild.UpdateAnchors();
    }
  }

  [CompilerGenerated]
  public void \u003COpenAsItemsList\u003Eb__16_0() => this.Hide();
}
