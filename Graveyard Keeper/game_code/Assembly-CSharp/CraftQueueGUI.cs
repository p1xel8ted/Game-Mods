// Decompiled with JetBrains decompiler
// Type: CraftQueueGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CraftQueueGUI : MonoBehaviour
{
  public CraftGUI _craft_gui;
  public CraftQueueItemGUI prefab;
  public CraftComponent _craft_component;
  public UITableOrGrid table;
  public UILabel gamepad_hint;
  public bool _is_empty;
  public WorldGameObject _craftery_wgo;
  public static CraftQueueGUI current_instance;

  public void Init(CraftGUI craft_gui)
  {
    this._craft_gui = craft_gui;
    this.prefab.gameObject.SetActive(false);
  }

  public void Redraw() => this.Draw(this._craft_component);

  public GamepadNavigationController gamepad_controller => this._craft_gui.gamepad_controller;

  public void Draw(CraftComponent craft_component)
  {
    this._craftery_wgo = craft_component.wgo;
    CraftQueueGUI.current_instance = this;
    this._craft_component = craft_component;
    this.table.DestroyChildren<CraftQueueItemGUI>(new CraftQueueItemGUI[1]
    {
      this.prefab
    });
    bool is_empty = true;
    if (craft_component?.craft_queue != null)
    {
      is_empty = craft_component.IsCraftQueueEmpty() && !craft_component.is_crafting;
      CraftDefinition currentCraft = craft_component.is_crafting ? craft_component.current_craft : (CraftDefinition) null;
      bool flag = false;
      CraftQueueItemGUI craftQueueItemGui1 = this.prefab.Copy<CraftQueueItemGUI>();
      craftQueueItemGui1.gameObject.SetActive(false);
      foreach (CraftComponent.CraftQueueItem craft in craft_component.craft_queue)
      {
        if (craft.n != 0 && craft.craft != null)
        {
          CraftQueueItemGUI craftQueueItemGui2 = this.prefab.Copy<CraftQueueItemGUI>();
          craftQueueItemGui2.is_gratitude_points_element = craft.is_gratitude_points_craft;
          bool add_one_current = false;
          if (!flag && currentCraft != null && craft.craft.id == currentCraft.id)
          {
            flag = true;
            add_one_current = true;
          }
          craftQueueItemGui2.Draw(craft, this._craftery_wgo, add_one_current);
        }
      }
      if (!flag && currentCraft != null)
      {
        craftQueueItemGui1.gameObject.SetActive(true);
        CraftQueueItemGUI craftQueueItemGui3 = craftQueueItemGui1;
        CraftComponent.CraftQueueItem ci = new CraftComponent.CraftQueueItem();
        ci.id = currentCraft.id;
        ci.n = 0;
        ci.is_gratitude_points_craft = GlobalCraftControlGUI.is_global_control_active;
        WorldGameObject crafteryWgo = this._craftery_wgo;
        craftQueueItemGui3.Draw(ci, crafteryWgo, true);
      }
    }
    Sounds.OnGUIClick();
    this.SetEmptyState(is_empty);
    this.table.Reposition();
    this._craft_gui.gamepad_controller.ReinitItems(false);
  }

  public bool IsEmpty() => this._is_empty;

  public void SetEmptyState(bool is_empty)
  {
    this.gameObject.SetActive(!is_empty);
    this._is_empty = is_empty;
  }

  public void OnDeleteItemPressed(CraftComponent.CraftQueueItem ci, bool is_current_craft)
  {
    if (this._craft_component?.craft_queue == null)
      return;
    this._craft_component?.craft_queue.Remove(ci);
    if (ci.is_gratitude_points_craft)
      this._craft_component.ReturnPlayerGratitudePoints();
    if (is_current_craft)
      this._craft_component.Cancel();
    this.Redraw();
    if (!BaseGUI.for_gamepad)
      return;
    CraftComponent craftComponent = this._craft_component;
    if ((craftComponent != null ? (craftComponent.craft_queue.Count == 0 ? 1 : 0) : 0) != 0)
      this._craft_gui.ExitFromQueueArea();
    else
      this._craft_gui.gamepad_controller.FocusOnFirstActive(5);
  }

  public void AddANewItemForCurrent(ref CraftComponent.CraftQueueItem ci)
  {
    if (this._craft_component?.craft_queue == null)
      return;
    CraftComponent.CraftQueueItem craftQueueItem = new CraftComponent.CraftQueueItem()
    {
      id = ci.id,
      n = 1,
      is_gratitude_points_craft = ci.is_gratitude_points_craft
    };
    this._craft_component.craft_queue.Insert(0, craftQueueItem);
    if (GlobalCraftControlGUI.is_global_control_active)
      this._craft_component.TryStartCraftFromQueue(start_by_player: false);
    ci = craftQueueItem;
    this.Redraw();
  }
}
