// Decompiled with JetBrains decompiler
// Type: CraftItemGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using LinqTools;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class CraftItemGUI : MonoBehaviour
{
  public static CraftItemGUI current_overed;
  public BaseItemCellGUI item_gui;
  [SerializeField]
  [HideInInspector]
  public CraftGUI _craft_gui;
  [NonSerialized]
  public CraftGUIInterface craft_gui_interface;
  public List<CraftDefinition> _possible_crafts = new List<CraftDefinition>();
  public int _current_craft_index;
  public BaseItemCellGUI[] _ingredients;
  public CraftIngredientButtonsPair[] _ingredient_buttons;
  public CraftItemPerkGUI[] _perk_icons;
  public UITable ingredients_table;
  public UITableOrGrid ingredients_table_universal;
  public UIWidget selection_frame;
  public UILabel label_name;
  public UILabel label_descr;
  public Color c_normal = Color.white;
  public Color c_not_enough = Color.red;
  public UIButton button;
  public Tooltip tooltip;
  public Item _body_item;
  public GJCommons.VoidDelegate _on_btn_click;
  public GamepadNavigationItem _gamepad_navigation_item;
  public int no_multi_quality_height = 45;
  public int multi_quality_height = 55;
  public int multi_quality_height_full = 100;
  public GameObject multi_quality_go;
  public GameObject full_detailed_go;
  public bool full_detailed_view;
  public UIWidget hgt_widget;
  public int widget_hgt_normal;
  public int widget_hgt_detailed;
  public UILabel no_influenced_perks_txt;
  public UILabel txt_spend;
  public List<string> _multiquality_ids = new List<string>();
  public UIButton multiquality_craft_btn;
  public UILabel value_items;
  public UILabel value_perks;
  public UILabel value_items_and_perks_sum;
  public UILabel value_difficulty;
  public UILabel value_result;
  public UILabel[] quality_probabilities;
  public UI2DSprite quality_bar;
  public Transform quality_bar_edge_container;
  public bool _just_outed;
  public Color color_probability_0;
  public Color color_probability_100;
  public Color color_probability_other;
  public PanelAutoScroll auto_scroll;
  public UIWidget inner_selection_frame;
  public UIWidget _current_inner_selectable_item;
  public BaseItemCellGUI _current_inner_ingredient;
  public bool _inited;
  public CraftDefinition craft_definition;
  public GameObject amount_buttons;
  public UIButton btn_amount_plus;
  public UIButton btn_amount_minus;
  public bool _overed_item;
  public bool _overed_additional;
  public bool _was_overed;
  public bool auto_height = true;
  public static CraftItemGUI last_item_pressed;
  [NonSerialized]
  public int _amount = 1;

  public CraftDefinition current_craft
  {
    get
    {
      return this._current_craft_index >= this._possible_crafts.Count ? (CraftDefinition) null : this._possible_crafts[this._current_craft_index];
    }
  }

  public GamepadNavigationItem gamepad_navigation_item
  {
    get
    {
      if ((UnityEngine.Object) this._gamepad_navigation_item == (UnityEngine.Object) null)
        this._gamepad_navigation_item = this.GetComponentInChildren<GamepadNavigationItem>(true);
      return this._gamepad_navigation_item;
    }
  }

  public GameObject GetIngredientsTableGameObject()
  {
    if ((UnityEngine.Object) this.ingredients_table != (UnityEngine.Object) null)
      return this.ingredients_table.gameObject;
    return this.ingredients_table_universal?.gameObject;
  }

  public void Init()
  {
    if (this._inited)
      return;
    this._inited = true;
    this._craft_gui = GUIElements.me.craft;
    this.craft_gui_interface = (CraftGUIInterface) GUIElements.me.craft;
    this.selection_frame.SetActive(false);
    this.gameObject.SetActive(false);
    if ((UnityEngine.Object) this.button != (UnityEngine.Object) null)
      this.button.gameObject.SetActive(false);
    if ((UnityEngine.Object) this.label_descr != (UnityEngine.Object) null)
      this.label_descr.text = "";
    foreach (UIEventTrigger componentsInChild in this.GetComponentsInChildren<UIEventTrigger>(true))
    {
      componentsInChild.onHoverOver.Add(new EventDelegate(new EventDelegate.Callback(this.OnMouseOvered)));
      componentsInChild.onHoverOut.Add(new EventDelegate(new EventDelegate.Callback(this.OnMouseOuted)));
      if ((UnityEngine.Object) componentsInChild.GetComponent<UIButton>() != (UnityEngine.Object) null)
        componentsInChild.onHoverOver.Add(new EventDelegate(new EventDelegate.Callback(((BaseGUI) this._craft_gui).SoundOnMouseOverCloseButton)));
    }
    UIScrollView componentInParent = this.GetComponentInParent<UIScrollView>();
    foreach (Collider2D componentsInChild in this.GetComponentsInChildren<Collider2D>())
    {
      if (!((UnityEngine.Object) componentsInChild.GetComponent<UIDragScrollView>() != (UnityEngine.Object) null))
        componentsInChild.gameObject.AddComponent<UIDragScrollView>().scrollView = componentInParent;
    }
  }

  public void DrawBody(Item body, GJCommons.VoidDelegate on_click = null)
  {
    this._body_item = body;
    this.label_name.text = "";
    this._on_btn_click = on_click;
    this.item_gui.x1.container.SetActive(false);
    this.item_gui.x2.container.SetActive(true);
    this.label_descr.gameObject.SetActive(true);
    GraveGUI.FillBodyDescription(body, this.item_gui.x2.icon, this.label_name, this.label_descr);
    this.GetIngredientsTableGameObject().SetActive(false);
    if ((UnityEngine.Object) this.button != (UnityEngine.Object) null)
      this.button.gameObject.SetActive(true);
    if (!BaseGUI.for_gamepad || !((UnityEngine.Object) this.gamepad_navigation_item != (UnityEngine.Object) null))
      return;
    this.gamepad_navigation_item.SetCallbacks(new GJCommons.VoidDelegate(this.OnOver), new GJCommons.VoidDelegate(this.OnOut), new GJCommons.VoidDelegate(this.OnItemAction));
  }

  public void Draw(CraftDefinition craft_definition)
  {
    this.craft_definition = craft_definition;
    this._possible_crafts.Clear();
    this._possible_crafts.Add(craft_definition);
    this._multiquality_ids.Clear();
    foreach (Item need in craft_definition.needs)
      this._multiquality_ids.Add(!need.is_multiquality || !this.current_craft.IsMultiqualityOutput() ? (string) null : need.multiquality_items.FirstOrDefault<string>());
    this._current_craft_index = 0;
    this._body_item = (Item) null;
    this._on_btn_click = (GJCommons.VoidDelegate) null;
    this._ingredient_buttons = this.GetComponentsInChildren<CraftIngredientButtonsPair>(true);
    this._perk_icons = this.GetComponentsInChildren<CraftItemPerkGUI>(true);
    this._ingredients = this.GetIngredientsTableGameObject().GetComponentsInChildren<BaseItemCellGUI>(true);
    for (int index = 0; index < this._ingredients.Length; ++index)
    {
      int ingredient_index = index;
      this._ingredients[index].GetComponentInChildren<UIEventTrigger>().onPress.Add(new EventDelegate((EventDelegate.Callback) (() => this.OnIngredientPressed(ingredient_index))));
    }
    this.item_gui.SetCallbacks(new GJCommons.VoidDelegate(this.OnOver), new GJCommons.VoidDelegate(this.OnOut), new GJCommons.VoidDelegate(this.OnItemAction));
    if (BaseGUI.for_gamepad)
    {
      if ((UnityEngine.Object) this.gamepad_navigation_item != (UnityEngine.Object) null)
        this.gamepad_navigation_item.SetCallbacks(new GJCommons.VoidDelegate(this.OnOver), new GJCommons.VoidDelegate(this.OnOut), new GJCommons.VoidDelegate(this.OnItemAction));
      foreach (GamepadNavigationItem componentsInChild in this.GetComponentsInChildren<GamepadNavigationItem>())
      {
        if (!((UnityEngine.Object) this.gamepad_navigation_item == (UnityEngine.Object) componentsInChild))
        {
          GamepadNavigationItem i = componentsInChild;
          componentsInChild.SetCallbacks((GJCommons.VoidDelegate) (() => this.OnChildElementOver(i)), (GJCommons.VoidDelegate) null, (GJCommons.VoidDelegate) (() => this.OnChildElementSelect(i)));
          componentsInChild.sub_group = craft_definition.id.GetHashCode();
        }
      }
      if ((UnityEngine.Object) this.multiquality_craft_btn != (UnityEngine.Object) null)
        this.multiquality_craft_btn.GetComponent<GamepadNavigationItem>().sub_group = craft_definition.id.GetHashCode();
    }
    this._overed_additional = this._overed_item = this._was_overed = false;
    this.Redraw();
    this.SetMouseOveredGraphics(false);
  }

  public void Redraw()
  {
    bool flag1 = this.current_craft.IsMultiqualityOutput();
    if (!this.current_craft.CanCraftMultiple())
      this._amount = 1;
    this.GetIngredientsTableGameObject().SetActive(true);
    if ((UnityEngine.Object) this.button != (UnityEngine.Object) null)
      this.button.gameObject.SetActive(false);
    this.item_gui.quality_icon.enabled = true;
    this.item_gui.quality_icon.gameObject.SetActive(false);
    this.item_gui.DrawItem(this.current_craft.output.Count > 0 ? this.current_craft.GetFirstRealOutput() : Item.empty);
    if (!string.IsNullOrEmpty(this.current_craft.icon))
      this.item_gui.DrawIcon(this.current_craft.icon, hide_quality_icon: this.current_craft.hide_quality_icon);
    if ((UnityEngine.Object) this.item_gui.x1.icon != (UnityEngine.Object) null)
      this.item_gui.x1.icon.MakePixelPerfect();
    this.item_gui.InitInputBehaviour();
    if ((UnityEngine.Object) this.item_gui?.x1?.counter != (UnityEngine.Object) null)
      this.item_gui.x1.counter.text = GUIElements.me.craft?.GetCrafteryWGO()?.GetCraftAmountCounter(this.craft_definition, this._amount);
    string str1 = GJL.L(this.current_craft.GetNameNonLocalized());
    string str2 = this.current_craft.GetDescription();
    if ((UnityEngine.Object) MainGame.me.build_mode_logics.cur_build_zone != (UnityEngine.Object) null && MainGame.me.build_mode_logics.cur_build_zone.definition != null)
      str2 = str2.Replace("(*)", MainGame.me.build_mode_logics.cur_build_zone.definition.quality_icon);
    this.label_name.text = str1;
    this.label_descr.gameObject.SetActive(!string.IsNullOrEmpty(str2));
    this.label_descr.text = str2;
    this.tooltip.available = !string.IsNullOrEmpty(str2);
    this.UpdateLabelsVisibility();
    BaseItemCellGUI.DrawIngredients(this._ingredients, this.current_craft.needs, !GlobalCraftControlGUI.is_global_control_active ? MainGame.me.player.GetMultiInventoryForInteraction() : GUIElements.me.craft.multi_inventory, this._multiquality_ids, this._amount);
    if (flag1)
    {
      this.item_gui.quality_icon.enabled = false;
      this.full_detailed_go.SetActive(this.full_detailed_view);
      this.multi_quality_go.SetActive(!this.full_detailed_view);
      if (this.auto_height)
        this.GetComponent<UIWidget>().height = this.full_detailed_view ? this.multi_quality_height_full : this.multi_quality_height;
      this.hgt_widget.height = this.full_detailed_view ? this.widget_hgt_detailed : this.widget_hgt_normal;
      if (this.full_detailed_view)
      {
        bool flag2 = this.CanCraft();
        this.multiquality_craft_btn.SetState(flag2 ? UIButtonColor.State.Normal : UIButtonColor.State.Disabled, true);
        this.multiquality_craft_btn.GetComponent<Collider2D>().enabled = flag2;
        this.label_name.text = "";
        this.RedrawIngredientsButtons();
        this.RedrawPerksAndValues();
        this.UpdateMultiQualityHint();
        this.UpdateInnerSelection();
      }
    }
    else
    {
      if ((UnityEngine.Object) this.full_detailed_go != (UnityEngine.Object) null)
        this.full_detailed_go.SetActive(false);
      if ((UnityEngine.Object) this.multi_quality_go != (UnityEngine.Object) null)
        this.multi_quality_go.SetActive(false);
      if (this.auto_height)
        this.GetComponent<UIWidget>().height = this.no_multi_quality_height;
      if ((UnityEngine.Object) this.hgt_widget != (UnityEngine.Object) null)
        this.hgt_widget.height = 2;
      if (this.craft_definition.output.Count > 0)
      {
        this.craft_definition.output[0].definition?.TryDrawQualityOrDisableGameObject(this.item_gui.quality_icon);
        if (!string.IsNullOrEmpty(this.craft_definition.icon) && this.craft_definition.hide_quality_icon)
          this.item_gui.quality_icon.sprite2D = (UnityEngine.Sprite) null;
      }
    }
    if ((UnityEngine.Object) this.full_detailed_go != (UnityEngine.Object) null && (UnityEngine.Object) this.GetComponent<BoxCollider2D>() != (UnityEngine.Object) null)
      this.GetComponent<BoxCollider2D>().enabled = !this.full_detailed_go.activeSelf;
    if (this.full_detailed_view && !BaseGUI.for_gamepad)
      this.selection_frame?.gameObject?.SetActive(false);
    this.selection_frame?.UpdateAnchors();
    this.label_name.color = this.CanCraft() ? this.c_normal : this.c_not_enough;
    if ((UnityEngine.Object) this.ingredients_table != (UnityEngine.Object) null)
      this.ingredients_table.repositionNow = true;
    if ((UnityEngine.Object) this.ingredients_table_universal != (UnityEngine.Object) null)
      this.ingredients_table_universal.Reposition();
    if ((UnityEngine.Object) this.txt_spend != (UnityEngine.Object) null)
      this.txt_spend.text = this.current_craft.GetSpendTxt(this.craft_gui_interface.GetCrafteryWGO(), this._amount);
    if (this.label_descr.gameObject.activeSelf && this.auto_height)
    {
      UIWidget component = this.label_descr.gameObject.transform.parent.GetComponent<UIWidget>();
      component.UpdateAnchors();
      this.label_descr.ProcessText();
      int num = this.label_descr.height - 20;
      if (flag1)
      {
        if (!this.full_detailed_view && num > 0)
        {
          this.GetComponent<UIWidget>().height = this.multi_quality_height + num;
          this.hgt_widget.height = 2 + num / 2;
          component.bottomAnchor.absolute = -num;
        }
      }
      else if (num > 0)
      {
        this.GetComponent<UIWidget>().height = this.no_multi_quality_height + num;
        this.hgt_widget.height = 2 + num / 2;
        component.bottomAnchor.absolute = -num;
      }
    }
    if ((UnityEngine.Object) this.ingredients_table_universal != (UnityEngine.Object) null)
      this.ingredients_table_universal.Reposition();
    else
      this.GetComponentInChildren<SimpleUITable>().Reposition();
    BoxCollider2D component1 = this.GetComponent<BoxCollider2D>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
    {
      UIWidget component2 = this.GetComponent<UIWidget>();
      component1.size = new Vector2(component1.size.x, (float) component2.height);
      component1.offset = new Vector2(component1.offset.x, (float) -component2.height / 2f);
    }
    this.GetComponent<UIWidget>().BroadcastMessage("UpdateAnchors");
    if ((UnityEngine.Object) this._current_inner_ingredient != (UnityEngine.Object) null)
      TooltipsManager.Redraw();
    this.RedrawAmountButtons();
  }

  public void SetOveredState(bool ovr)
  {
    this._was_overed = ovr;
    this.SetMouseOveredGraphics(ovr);
  }

  public void Update()
  {
    if (!BaseGUI.for_gamepad)
    {
      bool flag = this._overed_item || this._overed_additional;
      if (flag != this._was_overed)
      {
        if (flag)
        {
          Sounds.OnGUIHover();
          if ((UnityEngine.Object) CraftItemGUI.current_overed != (UnityEngine.Object) null)
            CraftItemGUI.current_overed.SetOveredState(false);
          CraftItemGUI.current_overed = this;
          this.SetMouseOveredGraphics(true);
        }
        else
        {
          if ((UnityEngine.Object) CraftItemGUI.current_overed == (UnityEngine.Object) this)
            CraftItemGUI.current_overed = (CraftItemGUI) null;
          this.SetMouseOveredGraphics(false);
        }
        this._was_overed = flag;
      }
    }
    if (!BaseGUI.for_gamepad || (UnityEngine.Object) CraftItemGUI.current_overed != (UnityEngine.Object) this || (UnityEngine.Object) this._craft_gui != (UnityEngine.Object) null && !this._craft_gui.is_shown_and_top)
      return;
    this.UpdateInnerSelection();
  }

  public bool ProcessIngredientStep(int step)
  {
    if (step == 0 || !this.full_detailed_view || (UnityEngine.Object) this._current_inner_ingredient == (UnityEngine.Object) null)
      return false;
    for (int ingredient_index = 0; ingredient_index < this._ingredients.Length; ++ingredient_index)
    {
      if (!((UnityEngine.Object) this._ingredients[ingredient_index] != (UnityEngine.Object) this._current_inner_ingredient) && this.IngredientHasOption(ingredient_index, step))
      {
        this.OnChangeIngredient(ingredient_index, step);
        return true;
      }
    }
    return false;
  }

  public void RedrawPerksAndValues()
  {
    List<string> neededPerks = this.current_craft.GetNeededPerks();
    List<string> linkedBuffs = this.current_craft.linked_buffs;
    int num1 = 0;
    foreach (string perk_id in neededPerks)
    {
      if (num1 != this._perk_icons.Length)
        this._perk_icons[num1++].DrawPerk(perk_id);
      else
        break;
    }
    foreach (string buff_id in linkedBuffs)
    {
      if (num1 != this._perk_icons.Length)
      {
        if (BuffsLogics.FindBuffByID(buff_id) != null)
          this._perk_icons[num1++].DrawBuff(buff_id);
      }
      else
        break;
    }
    for (int index = num1; index < this._perk_icons.Length; ++index)
      this._perk_icons[index].DrawPerk("");
    this.no_influenced_perks_txt.gameObject.SetActive(num1 == 0);
    CraftDefinition.MultiqualityCraftResult multiqualityResult = this.current_craft.GetMultiqualityResult(this._multiquality_ids);
    this.value_items.text = "(s1)" + multiqualityResult.value_items.ToString("0.0");
    this.value_perks.text = "(s1)" + multiqualityResult.value_perks.ToString("0.0");
    this.value_items_and_perks_sum.text = this.FloatValueToStr(multiqualityResult.value_items_and_perks_sum);
    this.value_difficulty.text = this.FloatValueToStr(-multiqualityResult.value_difficulty);
    this.value_result.text = multiqualityResult.value_result.ToString("0.0");
    float num2 = 0.0f;
    bool flag = false;
    for (int index = 2; index >= 0; --index)
    {
      num2 += multiqualityResult.quality_probabilities[index];
      int num3 = Mathf.RoundToInt(multiqualityResult.quality_probabilities[index] * 100f);
      this.quality_probabilities[index].text = num3 != 100 || !flag ? num3.ToString() + "%" : "";
      switch (num3)
      {
        case 0:
          this.quality_probabilities[index].color = this.color_probability_0;
          break;
        case 100:
          this.quality_probabilities[index].color = this.color_probability_100;
          flag = true;
          break;
        default:
          this.quality_probabilities[index].color = this.color_probability_other;
          break;
      }
    }
    this.quality_bar.fillAmount = num2 / 3f;
    this.quality_bar_edge_container.localScale = Vector3.right * this.quality_bar.fillAmount;
  }

  public string FloatValueToStr(float value)
  {
    string str = "(s1)";
    if ((double) value > 0.0)
      str += "+";
    else if ((double) value < 0.0)
      str += "-";
    return str + Mathf.Abs(value).ToString("0.0");
  }

  public void AddOneMoreCraftDefinition(CraftDefinition craft_definition)
  {
    this._possible_crafts.Add(craft_definition);
  }

  public void UpdateLabelsVisibility()
  {
    this.label_descr.SetActive(!string.IsNullOrEmpty(this.label_descr.text) && !this.full_detailed_view);
  }

  public void UpdateInnerSelection()
  {
    if (!this.full_detailed_view)
      return;
    this.inner_selection_frame.SetActive((UnityEngine.Object) CraftItemGUI.current_overed == (UnityEngine.Object) this && (UnityEngine.Object) this._current_inner_selectable_item != (UnityEngine.Object) null);
    if ((UnityEngine.Object) this._current_inner_selectable_item == (UnityEngine.Object) null)
      return;
    this.inner_selection_frame.transform.position = this._current_inner_selectable_item.transform.position;
    this.inner_selection_frame.width = this._current_inner_selectable_item.width;
    this.inner_selection_frame.height = this._current_inner_selectable_item.height;
  }

  public void OnMouseOvered()
  {
    if (BaseGUI.for_gamepad)
      return;
    this._overed_item = true;
  }

  public void OnMouseOuted()
  {
    if (BaseGUI.for_gamepad)
      return;
    this._overed_item = false;
  }

  public void OnMouseOveredAdditionalButtons()
  {
    if (BaseGUI.for_gamepad)
      return;
    this._overed_additional = true;
  }

  public void OnMouseOutedAdditionalButtons()
  {
    if (BaseGUI.for_gamepad)
      return;
    this._overed_additional = false;
  }

  public void OnOver()
  {
    if (!BaseGUI.for_gamepad)
      return;
    CraftItemGUI.current_overed = this;
    if (this._body_item == null)
    {
      if (this.current_craft.IsMultiqualityOutput())
      {
        this.UpdateMultiQualityHint();
        this.UpdateInnerSelection();
      }
      else
        this.craft_gui_interface.GetButtonTips().Print(GameKeyTip.Select("craft", this.CanCraft()), GameKeyTip.Option2("information"), GameKeyTip.Close());
    }
    else
      this.craft_gui_interface.GetButtonTips().Print(GameKeyTip.Select(), GameKeyTip.Close());
    this.selection_frame.SetActive(true);
    this.RedrawAmountButtons();
    Sounds.OnGUIHover();
    if (this.craft_gui_interface.GetItemsList().Count <= 0 || !((UnityEngine.Object) this == (UnityEngine.Object) this.craft_gui_interface.GetItemsList()[0]) && !((UnityEngine.Object) this == (UnityEngine.Object) this.craft_gui_interface.GetItemsList().Last<CraftItemGUI>()) || !((UnityEngine.Object) this.auto_scroll != (UnityEngine.Object) null))
      return;
    this.auto_scroll.Perform();
  }

  public void RedrawAmountButtons()
  {
    if ((UnityEngine.Object) this.amount_buttons == (UnityEngine.Object) null)
      return;
    this.amount_buttons.SetActive(((this._craft_gui.has_amount_buttons ? 1 : 0) & (this.current_craft == null ? 0 : (this.current_craft.CanCraftMultiple() ? 1 : 0))) != 0);
  }

  public void OnAboveWindowClosed()
  {
    if (!this.current_craft.IsMultiqualityOutput() || !this.full_detailed_view)
      return;
    if ((UnityEngine.Object) this._current_inner_selectable_item == (UnityEngine.Object) null)
    {
      this.craft_gui_interface.GetGamepadController().Enable(GamepadNavigationController.OpenMethod.GetAllAndFocus);
    }
    else
    {
      this.craft_gui_interface.GetGamepadController().Enable(GamepadNavigationController.OpenMethod.GetAll);
      this.craft_gui_interface.GetGamepadController().SetFocusedItem(this._current_inner_selectable_item.GetComponent<GamepadNavigationItem>());
    }
  }

  public void OnChildElementOver(GamepadNavigationItem item)
  {
    this._current_inner_selectable_item = item.GetComponent<UIWidget>();
    this._current_inner_ingredient = this._current_inner_selectable_item.GetComponent<BaseItemCellGUI>();
    this.UpdateInnerSelection();
    this.UpdateMultiQualityHint();
  }

  public void OnChildElementSelect(GamepadNavigationItem item)
  {
    UIButton componentInChildren1 = item.GetComponentInChildren<UIButton>();
    if ((UnityEngine.Object) componentInChildren1 != (UnityEngine.Object) null)
    {
      foreach (EventDelegate eventDelegate in componentInChildren1.onClick)
        eventDelegate.Execute();
    }
    UIEventTrigger componentInChildren2 = item.GetComponentInChildren<UIEventTrigger>();
    if (!((UnityEngine.Object) componentInChildren2 != (UnityEngine.Object) null))
      return;
    foreach (EventDelegate eventDelegate in componentInChildren2.onPress)
      eventDelegate.Execute();
  }

  public void UpdateMultiQualityHint()
  {
    List<GameKeyTip> tips = new List<GameKeyTip>();
    if (this.full_detailed_view)
    {
      if ((UnityEngine.Object) this._current_inner_selectable_item == (UnityEngine.Object) this.multiquality_craft_btn.GetComponent<UIWidget>())
        tips.Add(GameKeyTip.Select("craft", this.CanCraft()));
      if ((UnityEngine.Object) this._current_inner_ingredient != (UnityEngine.Object) null)
      {
        for (int ingredient_index = 0; ingredient_index < this._ingredients.Length; ++ingredient_index)
        {
          if (!((UnityEngine.Object) this._ingredients[ingredient_index] != (UnityEngine.Object) this._current_inner_ingredient) && this.IsSwitchableIngredient(ingredient_index))
            tips.Add(GameKeyTip.Select("select"));
        }
      }
    }
    else
      tips.Add(GameKeyTip.Select("expand"));
    if (this.full_detailed_view)
      tips.Add(GameKeyTip.Back());
    else
      tips.Add(GameKeyTip.Close());
    this.craft_gui_interface.GetButtonTips().Print(tips);
  }

  public void OnOut()
  {
    this.selection_frame.SetActive(false);
    this.inner_selection_frame.Deactivate<UIWidget>();
    if (!BaseGUI.for_gamepad || (UnityEngine.Object) this != (UnityEngine.Object) CraftItemGUI.current_overed)
      return;
    this.craft_gui_interface.GetButtonTips().Clear();
    this.RedrawAmountButtons();
  }

  public void OnItemAction()
  {
    if (BaseGUI.IsLastClickRightButton())
      this.craft_gui_interface.OnRightClick();
    else if (this._on_btn_click != null)
    {
      if (BaseGUI.for_gamepad)
        LazyInput.ClearKeyDown(GameKey.Select);
      this._on_btn_click();
    }
    else
    {
      if (UtilityStuff.ProcessCraftItemAction(this, this.current_craft))
        return;
      if (this.current_craft.IsMultiqualityOutput())
      {
        if (this.full_detailed_view)
        {
          if (BaseGUI.for_gamepad)
            this.OnCraftPressed();
          else
            this._craft_gui.CollapseItem();
        }
        else
        {
          this.selection_frame.Deactivate<UIWidget>();
          this.OnOpenDetailsButtonPressed();
          this._craft_gui.ExpandItem(this);
        }
      }
      else
        this.OnCraftPressed();
    }
  }

  public void OnCraftPressed()
  {
    if (CraftGUI.in_redraw_mode)
      return;
    Sounds.OnGUIClick();
    CraftItemGUI.last_item_pressed = this;
    bool flag1 = this.CanCraft();
    int amount1 = 0;
    int amount2 = this._amount;
    bool flag2 = this._craft_gui.GetCrafteryWGO().obj_def.can_insert_zombie;
    if (this._craft_gui.custom_craft_interface != null)
      flag2 = false;
    object[] objArray = new object[4]
    {
      (object) this._craft_gui.custom_craft_interface,
      (object) flag1,
      (object) this._amount,
      null
    };
    List<string> multiqualityIds = this._multiquality_ids;
    objArray[3] = (object) (multiqualityIds != null ? multiqualityIds.JoinToString<string>() : (string) null);
    Debug.Log((object) string.Format("OnCraftPressed, custom_ui={0}, can_craft={1}, n={2}, _multiquality_ids={3}", objArray));
    CraftComponent craft = this._craft_gui.GetCrafteryWGO()?.components?.craft;
    if (craft != null)
    {
      bool flag3 = !this.current_craft.CanEnqueue();
      if (craft.is_crafting && craft.current_craft != null && !craft.current_craft.CanEnqueue())
        flag3 = true;
      if (this._multiquality_ids != null && this._multiquality_ids.Count > 0 && (craft.is_crafting || !craft.IsCraftQueueEmpty()))
      {
        foreach (string multiqualityId in this._multiquality_ids)
        {
          if (!string.IsNullOrEmpty(multiqualityId))
            break;
        }
      }
      if (!craft.IsCraftQueueEmpty() && craft.craft_queue.Count > 0 && !craft.craft_queue[0].craft.CanEnqueue())
        flag3 = true;
      if (craft.IsCraftQueueEmpty() && !craft.is_crafting)
        flag3 = false;
      if (flag3)
      {
        CraftGUI.NeedToCancelCraftsDialog(craft, (System.Action) (() =>
        {
          GUIElements.me.dialog.Hide();
          this.OnCraftPressed();
        }));
        return;
      }
    }
    if (flag2)
    {
      amount1 = amount2;
      amount2 = 0;
      if (this.CanCraft(new int?(1)) && craft != null && craft.IsCraftQueueEmpty() && !craft.is_crafting)
      {
        amount2 = 1;
        --amount1;
      }
    }
    else if (!flag1)
      return;
    bool flag4 = true;
    if (amount2 > 0)
      flag4 = this._craft_gui.custom_craft_interface == null ? this.craft_gui_interface.OnCraft(this.current_craft, multiquality_ids: this._multiquality_ids, amount: amount2) : this._craft_gui.custom_craft_interface.OnCraft(this.current_craft, multiquality_ids: this._multiquality_ids, amount: amount2);
    if (amount1 > 0)
    {
      if (!this.current_craft.CanEnqueue())
      {
        GUIElements.me.dialog.OpenOK("not_enough_resources", new GJCommons.VoidDelegate(CraftGUI.RestoreFocusAfterDialogWindow));
        LazyInput.ClearKeyDown(GameKey.Select);
        return;
      }
      craft?.EnqueueCraft(this.current_craft, this._multiquality_ids, amount1, true);
      flag4 = true;
    }
    if (this._craft_gui.custom_craft_interface == null)
      this._craft_gui.queue?.Redraw();
    if (flag4 && BaseGUI.for_gamepad)
      LazyInput.ClearKeyDown(GameKey.Select);
    MainGame.me.player.components.interaction.RedrawCurrentInteractiveHint();
  }

  public bool CanCraft(int? amount = null)
  {
    if (this.craft_gui_interface == null)
      this.craft_gui_interface = (CraftGUIInterface) GUIElements.me.craft;
    return this._craft_gui?.custom_craft_interface != null ? this._craft_gui.custom_craft_interface.CanCraft(this.current_craft, this._multiquality_ids, amount ?? this._amount) : this.craft_gui_interface.CanCraft(this.current_craft, this._multiquality_ids, amount ?? this._amount);
  }

  public void SetMouseOveredGraphics(bool overed)
  {
    if (this.full_detailed_view)
      return;
    this.selection_frame.SetActive(overed);
  }

  public void OnOpenDetailsButtonPressed()
  {
    if (BaseGUI.IsLastClickRightButton())
      return;
    if (this.full_detailed_view)
    {
      this._craft_gui.CollapseItem(this);
    }
    else
    {
      this._craft_gui.ExpandItem(this);
      if (!((UnityEngine.Object) this.auto_scroll != (UnityEngine.Object) null))
        return;
      GJTimer.AddTimer(0.03f, (GJTimer.VoidDelegate) (() => this.auto_scroll.Perform()));
    }
  }

  public void OnIngredientPressed(int ingredient_index)
  {
    if (!this.current_craft.IsMultiqualityOutput() || !this.full_detailed_view)
      return;
    WorldGameObject worldGameObject = MainGame.me.player;
    if (GlobalCraftControlGUI.is_global_control_active && (UnityEngine.Object) this._craft_gui.GetCrafteryWGO() != (UnityEngine.Object) null)
      worldGameObject = this._craft_gui.GetCrafteryWGO();
    if (BaseGUI.for_gamepad)
      this.craft_gui_interface.GetButtonTips().Deactivate<ButtonTipsStr>();
    GUIElements.me.resource_picker.Open(worldGameObject, (InventoryWidget.ItemFilterDelegate) ((item, widget) => this.IngredientFiler(ingredient_index, item)), (CraftResourcesSelectGUI.ResourceSelectResultDelegate) (item => this.OnIngredientChanged(ingredient_index, item)));
  }

  public void OnIngredientChanged(int ingredient_index, Item item)
  {
    if (item == null || item.IsEmpty() || ingredient_index >= this.current_craft.needs.Count)
      return;
    Item need1 = this.current_craft.needs[ingredient_index];
    if (item.id == need1.id || item.id == this._multiquality_ids[ingredient_index])
      return;
    if (need1.is_multiquality && need1.multiquality_items.Contains(item.id))
    {
      this._multiquality_ids[ingredient_index] = item.id;
      this.Redraw();
    }
    else
    {
      foreach (CraftDefinition possibleCraft in this._possible_crafts)
      {
        Item need2 = possibleCraft.needs[ingredient_index];
        if (!(need2.id != item.id) || need2.is_multiquality && need2.multiquality_items.Contains(item.id))
        {
          this._current_craft_index = this._possible_crafts.IndexOf(possibleCraft);
          this._multiquality_ids.Clear();
          foreach (Item need3 in this.current_craft.needs)
            this._multiquality_ids.Add(need3.multiquality_items.FirstOrDefault<string>());
          this.Redraw();
          break;
        }
      }
    }
  }

  public InventoryWidget.ItemFilterResult IngredientFiler(int ingredient_index, Item item)
  {
    if (item == null || item.IsEmpty() || ingredient_index >= this.current_craft.needs.Count)
      return InventoryWidget.ItemFilterResult.Inactive;
    foreach (CraftDefinition possibleCraft in this._possible_crafts)
    {
      Item need = possibleCraft.needs[ingredient_index];
      if (need.id == item.id || need.is_multiquality && need.multiquality_items.Contains(item.id))
        return InventoryWidget.ItemFilterResult.Active;
    }
    return InventoryWidget.ItemFilterResult.Inactive;
  }

  public void RedrawIngredientsButtons()
  {
    for (int ingredient_index1 = 0; ingredient_index1 < this._ingredient_buttons.Length; ++ingredient_index1)
    {
      int ingredient_index = ingredient_index1;
      bool available = this.IsSwitchableIngredient(ingredient_index1);
      this._ingredient_buttons[ingredient_index1].Init(available, (Action<int>) (step => this.OnChangeIngredient(ingredient_index, step)));
      if (available)
        this._ingredient_buttons[ingredient_index1].SetEnabled(this.IngredientHasOption(ingredient_index1, 1), this.IngredientHasOption(ingredient_index1, -1));
    }
  }

  public void OnChangeIngredient(int ingredient_index, int step)
  {
    if (ingredient_index >= this.current_craft.needs.Count)
      return;
    Item need1 = this.current_craft.needs[ingredient_index];
    if (need1.is_multiquality)
    {
      List<string> multiqualityItems = need1.multiquality_items;
      string multiqualityId = this._multiquality_ids[ingredient_index];
      int index = multiqualityItems.IndexOf(multiqualityId) + step;
      if (index >= 0 && index < multiqualityItems.Count)
      {
        this._multiquality_ids[ingredient_index] = multiqualityItems[index];
        this.Redraw();
        return;
      }
    }
    int num1 = this._current_craft_index + step;
    if (num1 < 0 && num1 >= this._possible_crafts.Count)
      return;
    this._current_craft_index = num1;
    this._multiquality_ids.Clear();
    int num2 = 0;
    foreach (Item need2 in this.current_craft.needs)
    {
      string str = need2.multiquality_items.FirstOrDefault<string>();
      if (num2 == ingredient_index && step < 0 && need2.multiquality_items != null)
        str = need2.multiquality_items.Last<string>();
      this._multiquality_ids.Add(str);
      ++num2;
    }
    this.Redraw();
  }

  public bool IngredientHasOption(int ingredient_index, int step)
  {
    if (ingredient_index >= this.current_craft.needs.Count)
      return false;
    Item need = this.current_craft.needs[ingredient_index];
    if (this._possible_crafts.Count == 1 && !need.is_multiquality)
      return false;
    if (need.is_multiquality)
    {
      List<string> multiqualityItems = need.multiquality_items;
      string multiqualityId = this._multiquality_ids[ingredient_index];
      if (step > 0 && multiqualityId != multiqualityItems.LastElement<string>() || step < 0 && multiqualityId != multiqualityItems[0])
        return true;
    }
    int index = this._current_craft_index + step;
    return index >= 0 && index < this._possible_crafts.Count && need.id != this._possible_crafts[index].needs[ingredient_index].id;
  }

  public bool IsSwitchableIngredient(int ingredient_index)
  {
    return this.IngredientHasOption(ingredient_index, -1) || this.IngredientHasOption(ingredient_index, 1);
  }

  public void OnAmountPlus()
  {
    if (!this.current_craft.CanCraftMultiple())
      return;
    ++this._amount;
    this.Redraw();
    this.OnOver();
  }

  public void OnAmountMinus()
  {
    if (!this.current_craft.CanCraftMultiple() || this._amount == 1)
      return;
    --this._amount;
    this.Redraw();
    this.OnOver();
  }

  [CompilerGenerated]
  public void \u003COnCraftPressed\u003Eb__90_0()
  {
    GUIElements.me.dialog.Hide();
    this.OnCraftPressed();
  }

  [CompilerGenerated]
  public void \u003COnOpenDetailsButtonPressed\u003Eb__93_0() => this.auto_scroll.Perform();
}
