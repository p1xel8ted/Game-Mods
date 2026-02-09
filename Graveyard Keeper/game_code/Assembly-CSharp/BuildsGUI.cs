// Decompiled with JetBrains decompiler
// Type: BuildsGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections.Generic;

#nullable disable
public class BuildsGUI : BaseGameGUI
{
  public UIScrollView _scroll_view;
  public UIPanel _scroll_view_panel;
  public BuildItemGUI _item_prefab;
  public SimpleUITable _items_table;
  public List<ObjectCraftDefinition> _crafts;
  public List<BuildItemGUI> _build_items;
  public WorldGameObject _build_desk;
  public UniversalObjectInfoGUI object_info;

  public override void Init()
  {
    this._items_table = this.GetComponentInChildren<SimpleUITable>(true);
    this._item_prefab = this.GetComponentInChildren<BuildItemGUI>(true);
    this._item_prefab.InitPrefab();
    this._crafts = new List<ObjectCraftDefinition>();
    this._build_items = new List<BuildItemGUI>();
    this._scroll_view = this.GetComponentInChildren<UIScrollView>(true);
    this._scroll_view_panel = this._scroll_view.GetComponent<UIPanel>();
    base.Init();
  }

  public override void OpenFromGameGUI()
  {
    this.Open(MainGame.me.save.obj_crafts, this._build_desk);
    TooltipBubbleGUI.ChangeAvaibility(true);
  }

  public override void CloseFromGameGUI()
  {
    base.CloseFromGameGUI();
    this.Clear();
    TooltipBubbleGUI.ChangeAvaibility(false);
  }

  public void Open(CraftsInventory crafts_inventory, WorldGameObject build_desk)
  {
    this.Open();
    this._build_desk = build_desk;
    this.object_info.Draw(build_desk.GetUniversalObjectInfo());
    GUIElements.me.build_mode_gui.Hide();
    this.Clear();
    if (crafts_inventory != null)
    {
      foreach (ObjectCraftDefinition objectCrafts in crafts_inventory.GetObjectCraftsList())
      {
        if (MainGame.me.save.IsCraftVisible((CraftDefinition) objectCrafts))
          this._crafts.Add(objectCrafts);
      }
    }
    MultiInventory multiInventory = MainGame.me.build_mode_logics.multi_inventory;
    foreach (ObjectCraftDefinition craft in this._crafts)
    {
      BuildItemGUI buildItemGui = this._item_prefab.Copy<BuildItemGUI>();
      bool can_craft = multiInventory.IsEnoughItems(craft.needs);
      buildItemGui.Init(craft, can_craft, BaseGUI.for_gamepad);
      this._build_items.Add(buildItemGui);
    }
    this._items_table.Reposition();
    this._scroll_view.RestrictWithinBounds(false);
    if (!BaseGUI.for_gamepad)
      return;
    if (this.isActiveAndEnabled)
      this.gamepad_controller.ReinitItems(false);
    CraftDefinition craftDefinition = (CraftDefinition) null;
    foreach (BuildItemGUI buildItem in this._build_items)
    {
      if (buildItem.definition == craftDefinition)
      {
        this.gamepad_controller.SetFocusedItem(buildItem.gamepad_item, false);
        return;
      }
    }
    this._scroll_view.RestrictWithinBounds(false);
    this.gamepad_controller.FocusOnFirstActive();
    GJTimer.AddTimer(1f / 1000f, (GJTimer.VoidDelegate) (() => { }));
    if (this._crafts.Count != 0)
      return;
    this.button_tips.PrintClose();
  }

  public new void LateUpdate()
  {
    if (!this._scroll_view.RestrictWithinBounds(false) || !DOTween.IsTweening((object) this._scroll_view.transform))
      return;
    this._scroll_view.transform.DOKill();
  }

  public override void Hide(bool play_hide_sound = true)
  {
    this.Clear();
    this._scroll_view.transform.DOKill();
    this._scroll_view.RestrictWithinBounds(false);
    this._scroll_view.ResetPosition();
    base.Hide(play_hide_sound);
  }

  public void Clear()
  {
    this._crafts.Clear();
    foreach (BuildItemGUI buildItem in this._build_items)
      buildItem.DestroyGO<BuildItemGUI>();
    this._build_items.Clear();
  }

  public void UpdateTip(bool can_craft)
  {
    this.button_tips.Print(GameKeyTip.Select("build", can_craft), GameKeyTip.Close());
  }

  public override bool OnPressedBack()
  {
    this.OnClosePressed();
    return true;
  }

  public override void OnClosePressed()
  {
    this.Hide(true);
    MainGame.me.build_mode_logics.SetCurrentBuildZone(string.Empty);
  }
}
