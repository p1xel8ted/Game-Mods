// Decompiled with JetBrains decompiler
// Type: BodyCraftGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;

#nullable disable
public class BodyCraftGUI : ResourceBasedCraftGUI
{
  public Item _body;
  public BodyPanelGUI body_panel;
  public UniversalObjectInfoGUI obj_info;
  public bool _can_start_craft;

  public override void Init()
  {
    base.Init();
    this.body_panel.skull_bar.on_enable_skulls_frame += new System.Action(this.OnSkullsOver);
    this.body_panel.skull_bar.on_disable_skulls_frame += new System.Action(this.OnSkullsOut);
  }

  public void Open(WorldGameObject craftery_wgo)
  {
    this.Open(craftery_wgo, CraftDefinition.CraftType.ResourcesBasedCraft);
    this._body = craftery_wgo.GetBodyFromInventory();
    this.obj_info.Draw(craftery_wgo.GetUniversalObjectInfo());
    if (this._body != null)
      this._body.inventory_size = 99;
    this.body_panel.Draw(this._body);
    if ((UnityEngine.Object) this.body_panel?.btn_remove_body != (UnityEngine.Object) null)
      this.body_panel.btn_remove_body.GetComponent<GamepadSelectableButton>().SetCallbacks(new GJCommons.VoidDelegate(this.DropBody), (GJCommons.VoidDelegate) (() =>
      {
        if (!BaseGUI.for_gamepad)
          return;
        this.button_tips.Print(GameKeyTip.Select(this._body != null), GameKeyTip.Close());
      }));
    this.Redraw();
  }

  public void DropBody()
  {
    if (this._body == null)
      return;
    this.craftery_wgo.GiveItemToPlayersHands(this._body);
    this.Hide(false);
  }

  public new void Redraw()
  {
    this.label_resourse_hint.text = GJL.L("pick_embalming");
    this._can_start_craft = false;
    if (this._selected_item != null)
    {
      this.label_resourse_hint.text = $"{GJL.L("embalming_effect")}\n{this._selected_item.GetItemBodyModificators()}";
      if (this._body != null)
        this.CheckEmbalmAvailability();
    }
    base.Redraw();
  }

  public override bool CanCraft() => this._can_start_craft && base.CanCraft();

  public void CheckEmbalmAvailability()
  {
    this._can_start_craft = false;
    if (this._body.HasItemInInventory(this._selected_item.id))
    {
      UILabel labelResourseHint = this.label_resourse_hint;
      labelResourseHint.text = $"{labelResourseHint.text}\n\n{GJL.L("embalm_already_applied")}";
    }
    else
    {
      int negative;
      int positive;
      this._body.GetBodySkulls(out negative, out positive, out int _);
      if (this._selected_item.GetRedSkullsValue() < 0 && -this._selected_item.GetRedSkullsValue() > negative)
      {
        UILabel labelResourseHint = this.label_resourse_hint;
        labelResourseHint.text = $"{labelResourseHint.text}\n\n{GJL.L("embalm_not_enough", "(rskull)")}";
      }
      else if (this._selected_item.GetWhiteSkullsValue() < 0 && -this._selected_item.GetWhiteSkullsValue() > positive)
      {
        UILabel labelResourseHint = this.label_resourse_hint;
        labelResourseHint.text = $"{labelResourseHint.text}\n\n{GJL.L("embalm_not_enough", "(skull)")}";
      }
      else
        this._can_start_craft = true;
    }
  }

  public override void OnResourcePickerClosed(Item item)
  {
    base.OnResourcePickerClosed(item);
    this.Redraw();
  }

  public override void Hide(bool play_hide_sound = true)
  {
    if (GlobalCraftControlGUI.is_global_control_active)
      GUIElements.me.global_craft_control_gui.Open();
    base.Hide(play_hide_sound);
  }

  public void OnSkullsOver()
  {
    if (!BaseGUI.for_gamepad)
      return;
    this.button_tips.Print(GameKeyTip.Close(), GameKeyTip.RightStick(text: "move_tip"));
  }

  public void OnSkullsOut()
  {
    int num = BaseGUI.for_gamepad ? 1 : 0;
  }

  [CompilerGenerated]
  public void \u003COpen\u003Eb__5_0()
  {
    if (!BaseGUI.for_gamepad)
      return;
    this.button_tips.Print(GameKeyTip.Select(this._body != null), GameKeyTip.Close());
  }
}
