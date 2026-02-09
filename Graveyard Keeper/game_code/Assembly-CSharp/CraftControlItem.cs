// Decompiled with JetBrains decompiler
// Type: CraftControlItem
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class CraftControlItem : MonoBehaviour
{
  public static CraftControlItem current_overed;
  public UI2DSprite object_icon;
  public UI2DSprite zombie_icon;
  public UILabel object_label;
  public UILabel res_label;
  public UILabel zombie_eff;
  public BaseItemCellGUI item_cell;
  public BubbleWidgetProgress progress_bar;
  public GamepadNavigationItem _gamepad_navigation_item;
  public UIWidget selection_frame;
  public PanelAutoScroll auto_scroll;
  public GJCommons.VoidDelegate _on_btn_click;
  public Color c_normal = Color.white;
  public Color c_not_enough = Color.red;
  [HideInInspector]
  public WorldGameObject linked_wgo;
  public bool _was_overed;
  public bool interactable;
  public bool _overed_item;
  public bool _overed_additional;
  public bool has_totem;

  public void Update()
  {
    this.Redraw();
    if (BaseGUI.for_gamepad)
      return;
    bool flag = this._overed_item || this._overed_additional;
    if (flag == this._was_overed)
      return;
    if (flag)
    {
      if ((Object) CraftControlItem.current_overed != (Object) this)
        Sounds.OnGUIHover();
      if ((Object) CraftControlItem.current_overed != (Object) null && (Object) CraftControlItem.current_overed != (Object) this)
        CraftControlItem.current_overed.SetOveredState(false);
      CraftControlItem.current_overed = this;
      this.SetMouseOveredGraphics(true);
    }
    else
    {
      if ((Object) CraftControlItem.current_overed == (Object) this)
        CraftControlItem.current_overed = (CraftControlItem) null;
      this.SetMouseOveredGraphics(false);
    }
    this._was_overed = flag;
  }

  public void Draw(WorldGameObject wgo, bool has_totem)
  {
    this._overed_item = false;
    this._overed_additional = false;
    this._was_overed = false;
    this.has_totem = has_totem;
    this.gameObject.SetActive(true);
    this.linked_wgo = wgo;
    if (BaseGUI.for_gamepad && (Object) this._gamepad_navigation_item != (Object) null)
      this._gamepad_navigation_item.SetCallbacks(new GJCommons.VoidDelegate(this.OnOver), new GJCommons.VoidDelegate(this.OnOut), new GJCommons.VoidDelegate(this.OnItemAction));
    UniversalObjectInfo universalObjectInfo = this.linked_wgo.GetUniversalObjectInfo();
    this.object_icon.sprite2D = EasySpritesCollection.GetSprite(universalObjectInfo.icon);
    this.object_label.text = GJL.L(universalObjectInfo.header);
    this.res_label.text = string.Empty;
    this._overed_additional = this._overed_item = this._was_overed = false;
    this.progress_bar.Draw(new BubbleWidgetProgressData((BubbleWidgetProgressData.ProgressDelegate) (() => this.linked_wgo.components.craft.wgo.progress)));
    this.Update();
  }

  public void Redraw()
  {
    if ((Object) this.linked_wgo == (Object) null)
    {
      this.zombie_icon.gameObject.SetActive(false);
      this.item_cell.gameObject.SetActive(false);
      this.progress_bar.gameObject.SetActive(false);
      this.interactable = false;
      this.object_label.color = this.c_not_enough;
    }
    else
    {
      this.progress_bar.UpdateWidget();
      bool flag1 = this.linked_wgo.has_linked_worker && !this.linked_wgo.linked_worker.IsInvisibleWorker();
      this.zombie_icon.gameObject.SetActive(flag1);
      this.zombie_eff.gameObject.SetActive(flag1);
      if (this.linked_wgo.has_linked_worker)
        this.zombie_eff.text = this.linked_wgo.linked_worker.worker.GetWorkerEfficiencyTextOnlyPercent();
      bool isCrafting = this.linked_wgo.components.craft.is_crafting;
      bool flag2 = this.linked_wgo.components.craft.current_craft != null && this.linked_wgo.components.craft.current_craft.output != null && this.linked_wgo.components.craft.current_craft.output.Count > 0;
      bool flag3 = this.linked_wgo.components.craft.current_craft != null;
      this.item_cell.gameObject.SetActive(isCrafting & flag2);
      this.progress_bar.gameObject.SetActive(isCrafting);
      if (isCrafting & flag3 && this.linked_wgo.components.craft.current_craft.output != null && this.linked_wgo.components.craft.current_craft.output.Count > 0)
      {
        if (flag2)
        {
          Item currentItem = this.linked_wgo.components.craft.current_craft.output[0];
          if (this.linked_wgo.components.craft.current_craft.IsBodyPartInsertionCraft() && this.linked_wgo.components.craft.current_item != null)
            currentItem = this.linked_wgo.components.craft.current_item;
          this.item_cell.DrawItem(currentItem);
        }
        if (flag1)
          this.item_cell.DrawCapIcon(this.linked_wgo.components.craft.worker_is_paused);
        this.item_cell.DrawGratitudeIcon(this.linked_wgo.is_current_craft_gratitude);
      }
      if (!isCrafting && this.linked_wgo.components.craft.HasGratitudeCraftInQueue())
        this.item_cell.DrawGratitudeIcon(true, false);
      this.interactable = this.has_totem && (this.linked_wgo.obj_def.can_insert_zombie || this.linked_wgo.obj_def.tool_actions.no_actions || !this.linked_wgo.components.craft.is_crafting);
      if (this.linked_wgo.obj_def.interaction_type != ObjectDefinition.InteractionType.Craft && this.linked_wgo.obj_def.GetValidInteraction(this.linked_wgo) == null)
        this.interactable = false;
      this.object_label.color = this.interactable ? this.c_normal : this.c_not_enough;
    }
  }

  public void OnMouseOvered()
  {
    if (BaseGUI.for_gamepad)
      return;
    this._overed_item = true;
    this.selection_frame.SetActive(true);
  }

  public void OnMouseOuted()
  {
    if (BaseGUI.for_gamepad)
      return;
    this._overed_item = false;
    this.selection_frame.SetActive(false);
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

  public void SetOveredState(bool ovr)
  {
    this._was_overed = ovr;
    this.SetMouseOveredGraphics(ovr);
  }

  public void SetMouseOveredGraphics(bool overed)
  {
    this.selection_frame.SetActive(overed);
    Sounds.OnGUIHover();
  }

  public void OnOver()
  {
    if (!BaseGUI.for_gamepad)
      return;
    CraftControlItem.current_overed = this;
    GUIElements.me.global_craft_control_gui.GetButtonTips().Print(GameKeyTip.Select(), GameKeyTip.Close());
    this.selection_frame.SetActive(true);
    Sounds.OnGUIHover();
  }

  public void OnOut()
  {
    this.selection_frame.SetActive(false);
    if (!BaseGUI.for_gamepad || (Object) this != (Object) CraftControlItem.current_overed)
      return;
    GUIElements.me.global_craft_control_gui.GetButtonTips().Clear();
  }

  public void OnItemAction()
  {
    if (BaseGUI.IsLastClickRightButton())
    {
      GUIElements.me.global_craft_control_gui.OnRightClick();
    }
    else
    {
      if (!this.interactable)
        return;
      if (this._on_btn_click != null)
      {
        if (BaseGUI.for_gamepad)
          LazyInput.ClearKeyDown(GameKey.Select);
        this._on_btn_click();
      }
      else
      {
        GlobalCraftControlGUI.current_instance.last_selected = this;
        GlobalCraftControlGUI.current_instance.Hide(false);
        GUIElements.me.OpenCraftGUI(this.linked_wgo);
        CraftControlItem.current_overed = (CraftControlItem) null;
      }
    }
  }

  public void OnDisable() => this.OnOut();

  [CompilerGenerated]
  public float \u003CDraw\u003Eb__21_0() => this.linked_wgo.components.craft.wgo.progress;
}
