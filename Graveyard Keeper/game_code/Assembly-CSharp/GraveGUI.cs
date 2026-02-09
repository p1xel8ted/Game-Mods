// Decompiled with JetBrains decompiler
// Type: GraveGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class GraveGUI : BaseGUI, CraftInterface
{
  public UI2DSprite icon_cross;
  public UI2DSprite icon_fence;
  public UI2DSprite icon_body;
  public UILabel txt_cross;
  public UILabel txt_cross_q;
  public UILabel txt_fence;
  public UILabel txt_fence_q;
  public UILabel txt_grave;
  public UILabel txt_grave_total;
  public UILabel txt_grave_total_q;
  public UILabel txt_grave_condition_v;
  public UILabel txt_fix_grave_descr;
  public UIButton btn_cross_extract;
  public UIButton btn_fence_extract;
  public UIButton btn_body_extract;
  public UIButton btn_grave_fix;
  [NonSerialized]
  public BaseItemCellGUI[] fix_ingredients;
  public UITable ingredients_table;
  public Item _body;
  public Item _fence;
  public Item _cross;
  public WorldGameObject _grave_wgo;
  public BodyPanelGUI body_panel;
  public List<Item> _fix_price = new List<Item>();
  public BaseItemCellGUI prefab_grave_item;
  public UITableOrGrid grave_inventory_table;
  public UIWidget red_vertical_bar;
  public UITableOrGrid tb_skulls;
  public UITableOrGrid tb_slots;
  public GameObject prefab_w;
  public GameObject prefab_w_red;
  public GameObject prefab_slot_red;
  public GameObject prefab_slot_grey;
  public GameObject go_result_negative;
  public Tooltip exhume_tooltip;
  public GamepadNavigationItem middle_bar_item;
  public bool button_enabled_for_tip;
  public UIScrollView _scroll_view;
  public UI2DSprite frame;
  public int scroll_skulls_limit = 10;

  public override void Init()
  {
    base.Init();
    if ((UnityEngine.Object) this.middle_bar_item != (UnityEngine.Object) null)
      this.middle_bar_item.SetCallbacks(new GJCommons.VoidDelegate(this.EnableSelectionFrame), new GJCommons.VoidDelegate(this.DisableSelectionFrame), (GJCommons.VoidDelegate) null);
    this.fix_ingredients = this.ingredients_table.GetComponentsInChildren<BaseItemCellGUI>(true);
    foreach (GamepadSelectableButton componentsInChild in this.GetComponentsInChildren<GamepadSelectableButton>(true))
    {
      GamepadSelectableButton button = componentsInChild;
      button.Init();
      button.SetCallbacks((GJCommons.VoidDelegate) (() => this.OnGamepadSelect(button)), (GJCommons.VoidDelegate) (() => this.OnGamepadOver(button)));
    }
    this.prefab_slot_red.SetActive(false);
    this.prefab_slot_grey.SetActive(false);
    this.prefab_w.SetActive(false);
    this.prefab_w_red.SetActive(false);
    this.body_panel.skull_bar.on_enable_skulls_frame += new System.Action(this.OnSkullsOver);
    this.body_panel.skull_bar.on_disable_skulls_frame += new System.Action(this.OnSkullsOut);
  }

  public void Open(WorldGameObject grave_wgo)
  {
    if (grave_wgo.components.craft.is_crafting)
      return;
    this.Open();
    this._grave_wgo = grave_wgo;
    this._body = grave_wgo.GetBodyFromInventory();
    this._cross = grave_wgo.data.GetItemOfType(ItemDefinition.ItemType.GraveStone);
    this._fence = grave_wgo.data.GetItemOfType(ItemDefinition.ItemType.GraveFence);
    this.Redraw();
    if (!BaseGUI.for_gamepad)
      return;
    this.gamepad_controller.ReinitItems(true);
  }

  public void Redraw()
  {
    this.btn_cross_extract.isEnabled = this.btn_fence_extract.isEnabled = this.btn_body_extract.isEnabled = true;
    this.body_panel.Draw(this._body);
    if (this._body == null)
      this.btn_body_extract.isEnabled = false;
    this.txt_grave.text = this._grave_wgo.GetObjectConditionString();
    this.txt_grave_total.text = GJL.L("grave_total_q");
    float quality = this._grave_wgo.quality;
    this.txt_grave_total_q.text = quality.ToString("0");
    this.txt_grave_condition_v.text = this._grave_wgo.GetObjectConditionString("\n");
    this.go_result_negative.gameObject.SetActive((double) quality < 0.0);
    if ((UnityEngine.Object) this.exhume_tooltip != (UnityEngine.Object) null)
    {
      this.exhume_tooltip.available = true;
      this.exhume_tooltip.SetText(GJL.L("cant_exhume"));
    }
    if (this._grave_wgo.components.craft.is_crafting)
      this.btn_body_extract.isEnabled = this.btn_fence_extract.isEnabled = this.btn_body_extract.isEnabled = false;
    if (this._fence != null || this._cross != null)
      this.btn_body_extract.isEnabled = false;
    if ((UnityEngine.Object) this.exhume_tooltip != (UnityEngine.Object) null && (this.btn_body_extract.isEnabled || this._body == null))
      this.exhume_tooltip.available = false;
    MultiInventory.PlayerMultiInventory player_mi = MultiInventory.PlayerMultiInventory.DontChange;
    if (GlobalCraftControlGUI.is_global_control_active && !WorldZone.GetZoneOfObject(this._grave_wgo).IsPlayerInZone())
      player_mi = MultiInventory.PlayerMultiInventory.ExcludePlayer;
    MultiInventory multiInventory = MainGame.me.player.GetMultiInventory(player_mi: player_mi, sortWGOS: true);
    bool flag = false;
    for (int index = 0; index < this.fix_ingredients.Length; ++index)
    {
      if (index >= this._fix_price.Count)
      {
        this.fix_ingredients[index].DrawEmpty();
      }
      else
      {
        this.fix_ingredients[index].DrawIngredient(this._fix_price[index], multiInventory, init_tooltip: true);
        flag = true;
      }
    }
    this.btn_cross_extract.isEnabled &= !flag;
    this.btn_fence_extract.isEnabled &= !flag;
    this.btn_grave_fix.isEnabled = flag;
    this.RedrawGraveInventory();
  }

  public static void FillBodyDescription(
    Item body,
    UI2DSprite icon,
    UILabel hdr,
    UILabel descr,
    UILabel value = null)
  {
    if (body == null)
    {
      icon.sprite2D = (UnityEngine.Sprite) null;
      descr.text = "";
      hdr.text = GJL.L("none");
    }
    else
    {
      icon.sprite2D = EasySpritesCollection.GetSprite("i_" + body.definition.id);
      hdr.text = GJL.L(body.definition.id);
      if ((UnityEngine.Object) value == (UnityEngine.Object) null)
      {
        UILabel uiLabel = hdr;
        uiLabel.text = $"{uiLabel.text} {body.GetItemQualityString()}";
      }
      else
        value.text = body.GetItemQualityString();
      descr.text = $"({body.GetItemConditionString()})";
    }
  }

  public void OnGravePartPressed(UIButton button, Item item, ItemDefinition.ItemType type)
  {
    Debug.Log((object) ("OnGravePartPressed: " + type.ToString()));
    if ((UnityEngine.Object) button != (UnityEngine.Object) null && !button.isEnabled)
      return;
    if (item == null)
    {
      if (this._grave_wgo.components.craft.is_crafting)
      {
        Debug.LogError((object) "Cancel craft not implemented");
      }
      else
      {
        WorldGameObject worldGameObject = MainGame.me.player;
        if (GlobalCraftControlGUI.is_global_control_active && (UnityEngine.Object) this._grave_wgo != (UnityEngine.Object) null)
          worldGameObject = this._grave_wgo;
        GUIElements.me.resource_picker.Open(worldGameObject, (InventoryWidget.ItemFilterDelegate) ((itm, widget) => GraveGUI.GravePartsFilter(itm, type)), new CraftResourcesSelectGUI.ResourceSelectResultDelegate(this.OnResourcePickerClosed));
        if (BaseGUI.for_gamepad)
          this.button_tips.Deactivate<ButtonTipsStr>();
      }
      this.Redraw();
    }
    else
      GUIElements.me.craft.OpenAsGrave(this._grave_wgo, item, type);
  }

  public void OnResourcePickerClosed(Item item)
  {
    if (item == null)
      return;
    CraftDefinition gravePartPutCraft = this.FindGravePartPutCraft(item);
    if (gravePartPutCraft != null)
      this._grave_wgo.components.craft.CraftAsPlayer(gravePartPutCraft, item);
    this.Hide(true);
  }

  public CraftDefinition FindGravePartPutCraft(Item item)
  {
    foreach (CraftDefinition gravePartPutCraft in GameBalance.me.craft_data)
    {
      if (gravePartPutCraft.craft_in.Contains(this._grave_wgo.obj_id) && gravePartPutCraft.craft_type == CraftDefinition.CraftType.ResourcesBasedCraft && gravePartPutCraft.transfer_needs_to_wgo && gravePartPutCraft.needs.Count > 0 && gravePartPutCraft.needs[0].id == item.id)
        return gravePartPutCraft;
    }
    Debug.LogError((object) ("Couldn't find a put craft for item: " + item.id));
    return (CraftDefinition) null;
  }

  public CraftDefinition FindBodyExhumeCraft()
  {
    return GameBalance.me.GetData<CraftDefinition>("gr_exhume");
  }

  public static InventoryWidget.ItemFilterResult GravePartsFilter(
    Item item,
    ItemDefinition.ItemType type)
  {
    if (item == null || item.definition == null)
      return InventoryWidget.ItemFilterResult.Hide;
    return item.definition.type != type ? InventoryWidget.ItemFilterResult.Inactive : InventoryWidget.ItemFilterResult.Active;
  }

  public void OnFenceExtractPressed()
  {
    int num = this.btn_fence_extract.isEnabled ? 1 : 0;
  }

  public void OnBodyExtractPressed()
  {
    if (!this.btn_body_extract.isEnabled)
      return;
    CraftDefinition exhume_craft = this.FindBodyExhumeCraft();
    if (exhume_craft == null)
      return;
    if (MainGame.me.player.GetMultiInventory().IsEnoughItems(exhume_craft.needs))
      GUIElements.me.dialog.OpenDialog("exhume_confirmation", GJL.L("OK"), (GJCommons.VoidDelegate) (() =>
      {
        this._grave_wgo.components.craft.CraftAsPlayer(exhume_craft);
        this.Hide(true);
      }), GJL.L("Cancel"), text2: "exhume_confirmation_bot", item: exhume_craft.needs[0]);
    else
      GUIElements.me.dialog.OpenDialog("exhume_confirmation", GJL.L("OK"), (GJCommons.VoidDelegate) null, item: exhume_craft.needs[0]);
    if (!BaseGUI.for_gamepad)
      return;
    this.button_tips.Deactivate<ButtonTipsStr>();
  }

  public override void OnAboveWindowClosed()
  {
    if (!BaseGUI.for_gamepad)
      return;
    this.button_tips.Activate<ButtonTipsStr>();
    this.gamepad_controller.Enable(GamepadNavigationController.OpenMethod.GetAll);
    this.gamepad_controller.RestoreFocus();
  }

  public void OnGamepadOver(GamepadSelectableButton button)
  {
    if (!BaseGUI.for_gamepad)
      return;
    UIButton component = button.GetComponent<UIButton>();
    this.button_enabled_for_tip = component.isEnabled;
    this.button_tips.Print(GameKeyTip.Select(component.isEnabled), GameKeyTip.Close());
    Sounds.OnGUIHover();
  }

  public void OnGamepadSelect(GamepadSelectableButton button)
  {
    if (!BaseGUI.for_gamepad || !button.GetComponent<UIButton>().isEnabled)
      return;
    foreach (EventDelegate event_delegate in button.GetComponent<UIEventTrigger>().onPress)
      event_delegate.TryExecute();
  }

  public override bool OnPressedBack()
  {
    this.OnClosePressed();
    return true;
  }

  public override void Hide(bool play_hide_sound = true)
  {
    MainGame.me.player.components.interaction.UpdateNearestHint();
    GJTimer.AddTimer(0.5f, new GJTimer.VoidDelegate(MainGame.me.player.components.interaction.UpdateNearestHint));
    this.DisableSelectionFrame();
    base.Hide(play_hide_sound);
  }

  public void RedrawGraveInventory()
  {
    this.grave_inventory_table.DestroyChildren<BaseItemCellGUI>(new BaseItemCellGUI[1]
    {
      this.prefab_grave_item
    });
    this.prefab_grave_item.gameObject.SetActive(false);
    int num1 = 1;
    BaseItemCellGUI o1 = this.prefab_grave_item.Copy<BaseItemCellGUI>();
    Transform transform1 = o1.transform;
    int index1 = num1;
    int num2 = index1 + 1;
    transform1.SetSiblingIndex(index1);
    BaseItemCellGUI o2 = this.prefab_grave_item.Copy<BaseItemCellGUI>();
    Transform transform2 = o2.transform;
    int index2 = num2;
    int num3 = index2 + 1;
    transform2.SetSiblingIndex(index2);
    float f = 0.0f + this.DrawGravePart(o1, this._cross, ItemDefinition.ItemType.GraveStone) + this.DrawGravePart(o2, this._fence, ItemDefinition.ItemType.GraveFence);
    this.grave_inventory_table.Reposition();
    int negative = 0;
    int positive_avaialble = 0;
    this.body_panel.skull_bar.filled = 0;
    this.body_panel.skull_bar.RedrawFilledSkulls();
    if (this._body != null)
      this._body.GetBodySkulls(out negative, out int _, out positive_avaialble);
    this.red_vertical_bar.gameObject.SetActive(negative > 0);
    this.red_vertical_bar.height = 7 + 13 * negative;
    this.tb_slots.DestroyChildren(new Transform[2]
    {
      this.prefab_slot_grey.transform,
      this.prefab_slot_red.transform
    });
    this.tb_skulls.DestroyChildren(new Transform[2]
    {
      this.prefab_w.transform,
      this.prefab_w_red.transform
    });
    double quality = (double) this._grave_wgo.quality;
    for (int index3 = 0; index3 < positive_avaialble; ++index3)
      this.prefab_slot_grey.Copy(this.tb_slots.transform);
    for (int index4 = 0; index4 < negative; ++index4)
      this.prefab_slot_red.Copy(this.tb_slots.transform);
    List<GameObject> gameObjectList = new List<GameObject>();
    for (int index5 = 0; index5 < Mathf.FloorToInt(f); ++index5)
    {
      GameObject gameObject = (index5 < negative ? this.prefab_w_red : this.prefab_w).Copy(this.tb_skulls.transform);
      if (index5 >= negative)
        gameObjectList.Add(gameObject);
    }
    foreach (GameObject gameObject in gameObjectList)
      gameObject.transform.SetSiblingIndex(0);
    this.tb_slots.Reposition();
    this.tb_skulls.Reposition();
    int num4 = Math.Max(Mathf.FloorToInt(f), positive_avaialble + negative);
    bool flag = num4 > this.scroll_skulls_limit && this.gameObject.activeSelf;
    this._scroll_view.transform.localPosition = Vector3.zero;
    this._scroll_view.panel.UpdateAnchors();
    this._scroll_view.enabled = flag;
    this.middle_bar_item.active = flag && this.middle_bar_item.gameObject.activeInHierarchy;
    this._scroll_view.StopScrolling();
    this._scroll_view.transform.DOKill();
    this._scroll_view.RestrictWithinBounds(false);
    this._scroll_view.transform.localPosition = Vector3.zero;
    this._scroll_view.ResetPosition();
    if (flag)
      this._scroll_view.transform.localPosition = this._scroll_view.transform.localPosition with
      {
        y = (float) ((num4 - this.scroll_skulls_limit) * -13 + 3)
      };
    else
      this._scroll_view.transform.localPosition = Vector3.zero;
    this._scroll_view.panel.UpdateAnchors();
    this.Update();
  }

  public override void Update()
  {
    base.Update();
    if (this.red_vertical_bar.gameObject.activeSelf)
    {
      Transform transform = this.red_vertical_bar.transform;
      transform.localPosition = transform.localPosition with
      {
        y = (float) Math.Round((double) this._scroll_view.transform.localPosition.y - 167.0, MidpointRounding.AwayFromZero)
      };
    }
    if (!LazyInput.gamepad_active || !this.frame.gameObject.activeSelf)
      return;
    float num = LazyInput.GetDirection2().y * 0.1f;
    if (num.EqualsTo(0.0f))
      return;
    this._scroll_view.Scroll(num);
  }

  public float DrawGravePart(BaseItemCellGUI o, Item item, ItemDefinition.ItemType type)
  {
    o.SetCallbacks((GJCommons.VoidDelegate) null, (GJCommons.VoidDelegate) null, (GJCommons.VoidDelegate) (() => this.OnGravePartPressed((UIButton) null, item, type)));
    o.GetComponent<GamepadNavigationItem>().SetCallbacks((GJCommons.VoidDelegate) (() =>
    {
      this.button_tips.Print(GameKeyTip.Select(), GameKeyTip.Close());
      Sounds.OnGUIHover();
    }), (GJCommons.VoidDelegate) null, (GJCommons.VoidDelegate) (() => this.OnGravePartPressed((UIButton) null, item, type)));
    if (item == null)
    {
      o.container = o.x1;
      o.progress.gameObject.SetActive(false);
      switch (type)
      {
        case ItemDefinition.ItemType.GraveStone:
          o.x1.icon.sprite2D = EasySpritesCollection.GetSprite("i_additem");
          o.item_name.text = GJL.L("no_grave_cross");
          break;
        case ItemDefinition.ItemType.GraveFence:
          o.x1.icon.sprite2D = EasySpritesCollection.GetSprite("i_additem");
          o.item_name.text = GJL.L("no_grave_fence");
          break;
        default:
          o.x1.icon.sprite2D = (UnityEngine.Sprite) null;
          break;
      }
      if ((UnityEngine.Object) o.x1.icon.sprite2D != (UnityEngine.Object) null)
      {
        o.interaction_enabled = true;
        o.SetInactiveState(false);
      }
      o.item_description.text = "";
      return 0.0f;
    }
    o.DrawItem(item);
    float itemQuality = item.GetItemQuality();
    o.item_description.text = "(wr)" + itemQuality.ToString();
    return itemQuality;
  }

  public bool CanCraft(
    CraftDefinition craft,
    List<string> multiquality_ids = null,
    int amount = 1,
    List<Item> override_needs = null)
  {
    return (!(craft is ObjectCraftDefinition) || (craft as ObjectCraftDefinition).enabled) && MainGame.me.player.GetMultiInventory().IsEnoughItems(craft.needs);
  }

  public bool OnCraft(
    CraftDefinition craft,
    Item try_use_particular_item = null,
    List<string> multiquality_ids = null,
    int amount = 1,
    List<Item> override_needs = null,
    WorldGameObject other_obj_override = null)
  {
    string id = craft.id;
    Debug.Log((object) $"Grave craft: {craft.id} ==> {id}");
    if (!this._grave_wgo.components.craft.CraftAsPlayer(GameBalance.me.GetData<CraftDefinition>(id), override_needs: craft.needs, ignore_crafts_list: true))
      return false;
    GUIElements.me.craft.Hide(false);
    this.Hide(true);
    return true;
  }

  public new void OnRightClick() => base.OnRightClick();

  public void EnableSelectionFrame()
  {
    if ((UnityEngine.Object) this.frame == (UnityEngine.Object) null || !this._scroll_view.enabled && this.gameObject.activeInHierarchy)
      return;
    this.frame.gameObject.SetActive(true);
    this.OnSkullsOver();
    Sounds.OnGUIHover();
  }

  public void DisableSelectionFrame()
  {
    if ((UnityEngine.Object) this.frame == (UnityEngine.Object) null || !this._scroll_view.enabled)
      return;
    this.OnSkullsOut();
    this.frame.gameObject.SetActive(false);
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
}
