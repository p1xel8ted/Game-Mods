// Decompiled with JetBrains decompiler
// Type: CraftQueueItemGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CraftQueueItemGUI : MonoBehaviour
{
  public BaseItemCellGUI icon;
  public UILabel counter;
  public Color default_color;
  public Color gratitude_color;
  public bool _is_current;
  public GameObject amount_buttons;
  public bool is_gratitude_points_element;
  public GameObject selection_frame_default;
  public GameObject selection_frame_gratitude;
  public bool _overed_item;
  public bool _overed_side_buttons;
  public bool _overed_infinity_buttons;
  public GamepadNavigationItem _gamepad_navigation_item;
  public CraftComponent.CraftQueueItem _ci;
  public static CraftQueueItemGUI current_over;
  public WorldGameObject _craftery_wgo;
  public GameObject focus_frame;

  public void Draw(
    CraftComponent.CraftQueueItem ci,
    WorldGameObject craftery_wgo,
    bool add_one_current = false)
  {
    this._ci = ci;
    this._craftery_wgo = craftery_wgo;
    this._is_current = add_one_current;
    this.Redraw();
    if ((Object) CraftQueueItemGUI.current_over == (Object) this)
      this.OnOver();
    this.focus_frame.SetActive(false);
    this._gamepad_navigation_item = this.GetComponentsInChildren<GamepadNavigationItem>(true)[0];
    this.Update();
  }

  public void Redraw()
  {
    if (this._ci?.craft == null)
      return;
    Sounds.OnGUIClick();
    Item i = this._ci.craft.output.Count > 0 ? this._ci.craft.output[0] : (Item) null;
    this.icon.DrawItem(i);
    if (!string.IsNullOrEmpty(this._ci.craft.icon) && (i?.definition == null || !i.definition.is_big))
    {
      this.icon.DrawIcon(this._ci.craft.icon, hide_quality_icon: this._ci.craft.hide_quality_icon);
      if (i != null)
        this.icon.container.counter.text = this._craftery_wgo.GetCraftAmountCounter(this._ci.craft);
    }
    int n = this._ci.n;
    if (this._is_current)
      ++n;
    if (this.is_gratitude_points_element)
    {
      this.counter.color = this.gratitude_color;
      this.selection_frame_gratitude.SetActive(true);
      this.selection_frame_default.SetActive(false);
      this.icon.x1.counter.color = this.gratitude_color;
      this.icon.x2.counter.color = this.gratitude_color;
    }
    else
    {
      this.counter.color = this.default_color;
      this.selection_frame_gratitude.SetActive(false);
      this.selection_frame_default.SetActive(true);
      this.icon.x1.counter.color = this.default_color;
      this.icon.x2.counter.color = this.default_color;
    }
    this.counter.text = this._ci.infinite ? "∞" : "x" + n.ToString();
  }

  public void OnDeletePressed()
  {
    Sounds.OnGUIClick();
    CraftQueueGUI.current_instance.OnDeleteItemPressed(this._ci, this._is_current);
  }

  public void ForceRemoveSelectionFrame()
  {
    this._overed_item = this._overed_side_buttons = this._overed_infinity_buttons = false;
    this.Update();
    CraftQueueItemGUI.current_over = (CraftQueueItemGUI) null;
  }

  public void OnOver() => this._overed_item = true;

  public void OnOut() => this._overed_item = false;

  public void OnOverSideButton() => this._overed_side_buttons = true;

  public void OnOutSideButton() => this._overed_side_buttons = false;

  public void OnOverInfinityButton() => this._overed_infinity_buttons = true;

  public void OnOutInfinityButton() => this._overed_infinity_buttons = false;

  public void Update()
  {
    bool flag = this._overed_item || this._overed_side_buttons || this._overed_infinity_buttons;
    if (BaseGUI.for_gamepad && (Object) CraftQueueGUI.current_instance.gamepad_controller.focused_item == (Object) this._gamepad_navigation_item)
      flag = true;
    if ((Object) this.amount_buttons == (Object) null)
      return;
    this.amount_buttons.SetActive(flag);
    if (!BaseGUI.for_gamepad)
      this.focus_frame.SetActive(flag);
    if (flag)
    {
      CraftQueueItemGUI.current_over = this;
    }
    else
    {
      if (!((Object) CraftQueueItemGUI.current_over == (Object) this))
        return;
      CraftQueueItemGUI.current_over = (CraftQueueItemGUI) null;
    }
  }

  public void OnIncreasePressed()
  {
    this._ci.infinite = false;
    if (this._is_current && this._ci.n == 0)
      CraftQueueGUI.current_instance.AddANewItemForCurrent(ref this._ci);
    else if (++this._ci.n > 100)
      this._ci.n = 100;
    this.Redraw();
  }

  public void OnDecreasePressed()
  {
    this._ci.infinite = false;
    if (this._is_current && this._ci.n == 0 || !this._is_current && this._ci.n == 1)
    {
      this.OnDeletePressed();
    }
    else
    {
      --this._ci.n;
      this.Redraw();
    }
  }

  public void OnInfinityButtonPressed()
  {
    bool flag = false;
    if (!this._ci.infinite && this._is_current && this._ci.n == 0)
    {
      this._ci.infinite = true;
      this.OnIncreasePressed();
      flag = true;
    }
    this._ci.infinite = !this._ci.infinite;
    this.Redraw();
    if (!flag)
      return;
    CraftQueueGUI.current_instance.Redraw();
  }
}
