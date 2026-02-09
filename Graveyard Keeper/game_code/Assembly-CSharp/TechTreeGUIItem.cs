// Decompiled with JetBrains decompiler
// Type: TechTreeGUIItem
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using LinqTools;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TechTreeGUIItem : MonoBehaviour
{
  public TechDefinition _tech;
  public UI2DSprite[] sprs;
  public UILabel txt_name;
  public UILabel txt_name_2;
  public UILabel txt_name_3;
  public UILabel txt_cost;
  public static TechTreeGUIUnlockItem _unlock_prefab;
  public UIGrid unlocks_table;
  public GameObject gamepad_frame;
  public List<TechTreeGUIUnlockItem> _unlocks;
  public GameObject go_alpha_locked;
  public GameObject go_purchased;
  public GameObject go_locked;
  public GameObject go_not_purchased;
  public GameObject go_question_mark;
  public Transform pos1;
  public Transform pos2;
  public TechTreeGUI _tech_tree;
  public Color c_price_normal;
  public Color c_price_not_enough;
  public Color c_price_disabled;
  public int _on_out_call_frame;

  public string tech_id => this._tech.id;

  public TechDefinition tech => this._tech;

  public void InitPrefab()
  {
    this.gameObject.SetActive(false);
    TechTreeGUIItem._unlock_prefab = this.GetComponentInChildren<TechTreeGUIUnlockItem>(true);
    this._unlocks = new List<TechTreeGUIUnlockItem>()
    {
      TechTreeGUIItem._unlock_prefab
    };
    TechTreeGUIItem._unlock_prefab.gameObject.SetActive(false);
    for (int index = 0; index < 2; ++index)
    {
      TechTreeGUIUnlockItem treeGuiUnlockItem = TechTreeGUIItem._unlock_prefab.Copy<TechTreeGUIUnlockItem>();
      this._unlocks.Add(treeGuiUnlockItem);
      treeGuiUnlockItem.gameObject.SetActive(false);
    }
    this.unlocks_table.repositionNow = true;
  }

  public void Draw(TechDefinition tech)
  {
    if (this._unlocks == null)
      this._unlocks = ((IEnumerable<TechTreeGUIUnlockItem>) this.GetComponentsInChildren<TechTreeGUIUnlockItem>(true)).ToList<TechTreeGUIUnlockItem>();
    this._tech_tree = GUIElements.me.tech_tree;
    this._tech = tech;
    this.txt_name.text = GJL.L(tech.id);
    if ((Object) this.txt_name_2 != (Object) null)
      this.txt_name_2.text = this.txt_name.text;
    if ((Object) this.txt_name_3 != (Object) null)
      this.txt_name_3.text = this.txt_name.text;
    this.go_question_mark.SetActive(false);
    List<TechUnlock> visibleUnlocksList = tech.GetVisibleUnlocksList();
    TechDefinition.TechState techState = this.GetTechState();
    if (techState == TechDefinition.TechState.Hidden)
      visibleUnlocksList.Clear();
    for (int index = 0; index < this._unlocks.Count; ++index)
      this._unlocks[index].Draw(index < visibleUnlocksList.Count ? visibleUnlocksList[index] : (TechUnlock) null, !BaseGUI.for_gamepad);
    if (BaseGUI.for_gamepad)
      this.InitGamepadTooltip(visibleUnlocksList);
    this.RedrawTechState(techState);
    Color color;
    switch (techState)
    {
      case TechDefinition.TechState.Purchased:
        this.txt_cost.text = "";
        goto label_21;
      case TechDefinition.TechState.Unavailable:
        color = this.c_price_disabled;
        break;
      case TechDefinition.TechState.Hidden:
        this.txt_name.text = "";
        this.txt_cost.text = "";
        goto label_21;
      default:
        color = this.c_price_normal;
        break;
    }
    Color clr_normal = color;
    if (MainGame.me.player.IsEnough(tech.price))
    {
      this.txt_cost.color = clr_normal;
      this.txt_cost.text = tech.price.ToPrintableString();
    }
    else
    {
      this.txt_cost.color = Color.white;
      this.txt_cost.text = tech.price.ToPrintableString(true, clr_normal, this.c_price_not_enough);
    }
label_21:
    this.GetComponent<GamepadNavigationItem>().SetCallbacks(new GJCommons.VoidDelegate(this.OnGamepadOver), new GJCommons.VoidDelegate(this.OnGamepadOut), new GJCommons.VoidDelegate(this.OnClickedTech));
    this.OnGamepadOut();
  }

  public void InitGamepadTooltip(List<TechUnlock> visible_unlocks)
  {
    Tooltip component = this.GetComponent<Tooltip>();
    if ((Object) component == (Object) null)
      return;
    for (int index = 0; index < visible_unlocks.Count; ++index)
    {
      visible_unlocks[index].GetTooltip(component);
      if (index != visible_unlocks.Count - 1)
        component.AddData((BubbleWidgetData) new BubbleWidgetSeparatorData());
    }
  }

  public TechDefinition.TechState GetTechState() => this._tech.GetState();

  public void OnGamepadOver()
  {
    this._tech_tree.gamepad_controller.restore_last_in_group = true;
    this.gamepad_frame.Activate();
    if (!Sounds.WasAnySoundPlayedThisFrame())
      Sounds.OnGUIHover(Sounds.ElementType.ItemCell);
    if (MainGame.me.save.unlocked_techs.Contains(this.tech_id))
      this._tech_tree.button_tips.Print(GameKeyTip.Close());
    else
      this._tech_tree.button_tips.Print(GameKeyTip.Select("buy", MainGame.me.save.CanBuyTech(this.tech_id)), GameKeyTip.Close());
    TooltipsManager.Redraw();
  }

  public void OnGamepadOut() => this.gamepad_frame.Deactivate();

  public void OnMouseOvered()
  {
    if (BaseGUI.for_gamepad)
      return;
    if (this._on_out_call_frame != Time.frameCount && !Sounds.WasAnySoundPlayedThisFrame())
      Sounds.OnGUIHover(Sounds.ElementType.ItemCell);
    this.gamepad_frame.Activate();
    TooltipsManager.Redraw();
  }

  public void OnMouseOuted()
  {
    if (BaseGUI.for_gamepad)
      return;
    this.gamepad_frame.Deactivate();
    this._on_out_call_frame = Time.frameCount;
  }

  public void OnClickedTech()
  {
    if (BaseGUI.IsLastClickRightButton())
      return;
    this._tech_tree.OnClickTech(this._tech);
  }

  public void RedrawTechState(TechDefinition.TechState state)
  {
    this.go_purchased.SetActive(state == TechDefinition.TechState.Purchased);
    this.go_locked.SetActive(state == TechDefinition.TechState.Unavailable || state == TechDefinition.TechState.Hidden);
    this.go_not_purchased.SetActive(state == TechDefinition.TechState.AvailableForPurchase);
    this.go_question_mark.SetActive(state == TechDefinition.TechState.Hidden);
    this.unlocks_table.gameObject.SetActive(state != TechDefinition.TechState.Hidden);
    this.go_alpha_locked.SetActive(false);
  }
}
