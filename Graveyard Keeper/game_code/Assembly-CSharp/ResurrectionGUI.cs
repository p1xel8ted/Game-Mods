// Decompiled with JetBrains decompiler
// Type: ResurrectionGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ResurrectionGUI : BaseGUI, CraftGUIInterface, CraftInterface
{
  public UniversalObjectInfoGUI _universal_info;
  public Item _body;
  public BodyPanelGUI body_panel;
  public CraftItemGUI craft_item;
  public CraftDefinition _craft;
  public WorldGameObject _wgo;
  public bool is_skulls_frame_active;
  public UIButton resurrect_btn;
  public UILabel cant_resurrect_txt;

  public override void Init()
  {
    this._universal_info = this.GetComponentInChildren<UniversalObjectInfoGUI>(true);
    this.craft_item.craft_gui_interface = (CraftGUIInterface) this;
    base.Init();
    this.body_panel.skull_bar.on_enable_skulls_frame += new System.Action(this.OnSkullsOver);
    this.body_panel.skull_bar.on_disable_skulls_frame += new System.Action(this.OnSkullsOut);
  }

  public void Open(WorldGameObject craft_obj)
  {
    this.Open();
    this._body = craft_obj.GetBodyFromInventory();
    this._universal_info.Draw(craft_obj.GetUniversalObjectInfo());
    this._universal_info.text_descr.text = GJL.L("zombie_crafting_table_d");
    this._wgo = craft_obj;
    this.body_panel.Draw(this._body);
    this.craft_item.ingredients_table_universal.gameObject.SetActive(this._body != null);
    if (this._body == null)
    {
      this.resurrect_btn.gameObject.SetActive(true);
      this.cant_resurrect_txt.SetActive(false);
    }
    else
    {
      bool flag = (double) this._body.durability >= 0.89999997615814209;
      this.resurrect_btn.gameObject.SetActive(flag);
      this.cant_resurrect_txt.gameObject.SetActive(!flag);
      this._craft = GameBalance.me.GetData<CraftDefinition>("zombie_craft");
      this.craft_item.Draw(this._craft);
    }
    bool flag1 = this.CanCraftZombie();
    this.resurrect_btn.isEnabled = flag1;
    this.resurrect_btn.SetState(flag1 ? UIButtonColor.State.Normal : UIButtonColor.State.Disabled, true);
    if (BaseGUI.for_gamepad)
    {
      this.button_tips.Print(GameKeyTip.Select(), GameKeyTip.Close());
      this.gamepad_controller.ReinitItems(true);
      this.resurrect_btn.GetComponent<GamepadNavigationItem>().SetCallbacks((GJCommons.VoidDelegate) null, (GJCommons.VoidDelegate) null, new GJCommons.VoidDelegate(this.OnPressedResurrect));
      this.body_panel.btn_remove_body.GetComponent<GamepadNavigationItem>().SetCallbacks((GJCommons.VoidDelegate) null, (GJCommons.VoidDelegate) null, new GJCommons.VoidDelegate(this.DropBody));
    }
    this.UpdateAllAnchors();
  }

  public void DropBody()
  {
    for (int index = 0; index < this._wgo.data.inventory.Count; ++index)
    {
      Item obj = this._wgo.data.inventory[index];
      if (obj.definition.type == ItemDefinition.ItemType.Body)
      {
        this._wgo.GiveItemToPlayersHands(obj);
        this.Hide(false);
        break;
      }
    }
  }

  public override bool OnPressedBack()
  {
    this.OnClosePressed();
    return true;
  }

  public override void Hide(bool play_hide_sound = true) => base.Hide(play_hide_sound);

  public void OnPressedResurrect()
  {
    Debug.Log((object) "Resurrect!");
    if (!this.CanCraft(this._craft, (List<string>) null, 1, (List<Item>) null))
      return;
    this._wgo.components.craft.CraftAsPlayer(this._craft);
    GS.SetPlayerEnable(false, false);
    this._wgo.components.animator.SetTrigger("rise_fx");
    this.Hide(true);
  }

  public bool CanCraftZombie()
  {
    return this._body != null && MainGame.me.player.GetMultiInventoryForInteraction().IsEnoughItems(this._craft.needs) && (double) this._body.durability >= 0.89999997615814209;
  }

  public bool CanCraft(
    CraftDefinition craft_not_used,
    List<string> multiquality_ids_not_used = null,
    int amount_not_used = 1,
    List<Item> override_needs = null)
  {
    return this.CanCraftZombie();
  }

  public bool OnCraft(
    CraftDefinition craft_not_used,
    Item try_use_particular_item_no_used = null,
    List<string> multiquality_ids_not_used = null,
    int amount_not_used = 1,
    List<Item> override_needs_not_used = null,
    WorldGameObject other_obj_override = null)
  {
    return this.CanCraftZombie();
  }

  public new void OnRightClick() => base.OnRightClick();

  public ButtonTipsStr GetButtonTips() => new ButtonTipsStr();

  public GamepadNavigationController GetGamepadController() => this.gamepad_controller;

  public List<CraftItemGUI> GetItemsList() => throw new NotImplementedException();

  public WorldGameObject GetCrafteryWGO() => this._wgo;

  public void OnSkullsOver()
  {
    if (!BaseGUI.for_gamepad)
      return;
    this.button_tips.Print(GameKeyTip.Close(), GameKeyTip.RightStick(text: "move_tip"));
  }

  public void OnSkullsOut()
  {
    if (!BaseGUI.for_gamepad)
      return;
    this.button_tips.Print(GameKeyTip.Select(), GameKeyTip.Close());
  }
}
