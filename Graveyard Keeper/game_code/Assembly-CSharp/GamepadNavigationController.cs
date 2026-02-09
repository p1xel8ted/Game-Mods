// Decompiled with JetBrains decompiler
// Type: GamepadNavigationController
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Text;
using UnityEngine;

#nullable disable
public class GamepadNavigationController : MonoBehaviour
{
  public const float LOOP_OFFSET = 50f;
  public bool avaible = true;
  [Space]
  [SerializeField]
  [RuntimeValue]
  public List<GamepadNavigationItem> selectable_items = new List<GamepadNavigationItem>();
  public bool auto_select;
  public bool perfect_grid;
  public bool restore_last_in_group;
  public GamepadNavigationSettings vertical_settings;
  public GamepadNavigationSettings horizontal_settings;
  public Dictionary<int, GamepadNavigationItem> _last_focused_items = new Dictionary<int, GamepadNavigationItem>();
  public bool _is_enabled;
  public float _gui_scale;

  public static GamepadNavigationController current
  {
    get
    {
      return !((Object) BaseGUI.active_gui == (Object) null) ? BaseGUI.active_gui.gamepad_controller : (GamepadNavigationController) null;
    }
  }

  public GamepadNavigationItem focused_item
  {
    get
    {
      foreach (GamepadNavigationItem selectableItem in this.selectable_items)
      {
        if (selectableItem.is_focused)
          return selectableItem;
      }
      return (GamepadNavigationItem) null;
    }
  }

  public int focused_item_index
  {
    get
    {
      GamepadNavigationItem focusedItem = this.focused_item;
      return !((Object) focusedItem != (Object) null) ? -1 : focusedItem.index;
    }
  }

  public bool is_enabled => this._is_enabled;

  public void Enable()
  {
    if (this._is_enabled)
      return;
    this._is_enabled = true;
  }

  public void Enable(GamepadNavigationController.OpenMethod method)
  {
    this.Enable();
    if ((Object) this.focused_item != (Object) null)
      this.focused_item.UnFocus();
    switch (method)
    {
      case GamepadNavigationController.OpenMethod.GetAll:
        this.ReinitItems(false);
        break;
      case GamepadNavigationController.OpenMethod.GetAllAndFocus:
        this.ReinitItems(true);
        break;
      case GamepadNavigationController.OpenMethod.GetAllAndSelect:
        this.ReinitItems(true);
        this.SelectFocusedItem();
        break;
    }
  }

  public void ReinitItems(bool focus_on_first_active)
  {
    this._gui_scale = this.transform.lossyScale.x;
    this.selectable_items.Clear();
    this.selectable_items.AddRange((IEnumerable<GamepadNavigationItem>) this.GetComponentsInChildren<GamepadNavigationItem>());
    this.RemoveNullsAndSetIndexes(this.selectable_items);
    this.PrintItemsList(this.selectable_items, "GetComponentsInChildren() result");
    int num = 0;
    foreach (GamepadNavigationItem selectableItem in this.selectable_items)
      selectableItem.Init(num++, this, this._gui_scale);
    if (!focus_on_first_active)
      return;
    this.FocusOnFirstActive();
  }

  public void RemoveNullsAndSetIndexes(List<GamepadNavigationItem> items)
  {
    items.RemoveUnityNulls<GamepadNavigationItem>();
    for (int index = 0; index < items.Count; ++index)
      items[index].index = index;
  }

  public void Disable()
  {
    if (!this._is_enabled)
      return;
    this._is_enabled = false;
    foreach (GamepadNavigationItem selectableItem in this.selectable_items)
      selectableItem.UnFocus();
    this.selectable_items.Clear();
  }

  public void RememberFocused(GamepadNavigationItem item)
  {
    if ((Object) item == (Object) null)
      return;
    if (this._last_focused_items.ContainsKey(item.group))
      this._last_focused_items[item.group] = item;
    else
      this._last_focused_items.Add(item.group, item);
  }

  public bool FocusOnFirstActive(int group = -1)
  {
    int focus_index = -1;
    bool flag = group >= 0;
    foreach (GamepadNavigationItem selectableItem in this.selectable_items)
    {
      ++focus_index;
      if ((Object) selectableItem == (Object) null)
        Debug.LogError((object) ("Found a null item while selecting group = " + group.ToString()));
      else if (selectableItem.isActiveAndEnabled && selectableItem.active && (!flag || selectableItem.group == group))
      {
        this.SetFocusedItem(focus_index);
        return true;
      }
    }
    Debug.LogWarning((object) "no selectable_items", (Object) this);
    return false;
  }

  public bool HaveSavedFocusForGroup(int group)
  {
    if (!this._last_focused_items.ContainsKey(group))
      return false;
    if ((Object) this._last_focused_items[group] == (Object) null)
    {
      this._last_focused_items.Remove(group);
      return false;
    }
    if (this._last_focused_items[group].isActiveAndEnabled && this._last_focused_items[group].active)
      return true;
    this._last_focused_items.Remove(group);
    return false;
  }

  public void RestoreFocus(int group = 0)
  {
    if (this.HaveSavedFocusForGroup(group))
    {
      this.SetFocusedItem(this._last_focused_items[group]);
    }
    else
    {
      Debug.LogWarning((object) $"no saved last focus for group {group.ToString()}, trying to select any in this group");
      this.FocusOnFirstActive(group);
    }
  }

  public void RestoreSelection(int group)
  {
    this.RestoreFocus(group);
    this.SelectFocusedItem();
  }

  public void SetFocusedItem(GamepadNavigationItem item, bool animate_auto_scroll = true)
  {
    this.SetFocusedItem(this.selectable_items.IndexOf(item), animate_auto_scroll);
  }

  public int GetFocusedItemIndex(GamepadNavigationItem item) => this.selectable_items.IndexOf(item);

  public void SetFocusedItem(int focus_index, bool animate_auto_scroll = true)
  {
    if (focus_index < 0 || focus_index >= this.selectable_items.Count)
      focus_index = 0;
    GamepadNavigationItem gamepadNavigationItem = (GamepadNavigationItem) null;
    for (int index = 0; index < this.selectable_items.Count; ++index)
    {
      if (index == focus_index)
      {
        if (this.selectable_items[index].active)
          gamepadNavigationItem = this.selectable_items[index];
      }
      else
        this.selectable_items[index].UnFocus();
    }
    if (!((Object) gamepadNavigationItem != (Object) null))
      return;
    gamepadNavigationItem.Focus(animate_auto_scroll);
    this.RememberFocused(gamepadNavigationItem);
  }

  public void SelectFocusedItem()
  {
    GamepadNavigationItem focusedItem = this.focused_item;
    if (!((Object) focusedItem != (Object) null))
      return;
    focusedItem.Select();
  }

  public void Navigate(Direction direction)
  {
    GamepadNavigationSettings navigationSettings1;
    switch (direction)
    {
      case Direction.None:
        return;
      case Direction.Up:
      case Direction.Down:
        navigationSettings1 = this.vertical_settings;
        break;
      default:
        navigationSettings1 = this.horizontal_settings;
        break;
    }
    GamepadNavigationSettings navigationSettings2 = navigationSettings1;
    this.RemoveNullsAndSetIndexes(this.selectable_items);
    if (this.selectable_items.Count == 0)
      return;
    GamepadNavigationItem focusedItem = this.focused_item;
    if ((Object) focusedItem == (Object) null)
    {
      this.SetFocusedItem(0);
    }
    else
    {
      GamepadNavigationItem customDirectionItem = focusedItem.GetCustomDirectionItem(direction);
      if ((Object) customDirectionItem != (Object) null && this.selectable_items.Contains(customDirectionItem))
      {
        if (!((Object) customDirectionItem != (Object) focusedItem))
          return;
        this.SetFocusedItem(customDirectionItem);
      }
      else
      {
        int group1 = focusedItem.group;
        int subGroup = focusedItem.sub_group;
        Vector2 pos = focusedItem.pos;
        List<GamepadNavigationItem> collection1 = new List<GamepadNavigationItem>();
        foreach (GamepadNavigationItem selectableItem in this.selectable_items)
        {
          if (!((Object) selectableItem == (Object) focusedItem) && !this.SkipItemBecauseOfState(selectableItem) && (!navigationSettings2.stay_in_group || !this.SkipItemBecauseOfGroup(selectableItem, group1)) && (!navigationSettings2.stay_in_sub_group || !this.SkipItemBecauseOfSubGroup(selectableItem, subGroup)) && !this.SkipItemBecauseOfDirection(selectableItem, pos, direction))
            collection1.Add(selectableItem);
        }
        if (collection1.Count == 0)
          return;
        List<GamepadNavigationItem> gamepadNavigationItemList = new List<GamepadNavigationItem>((IEnumerable<GamepadNavigationItem>) collection1);
        List<GamepadNavigationItem> collection2 = new List<GamepadNavigationItem>();
        List<GamepadNavigationItem> collection3 = new List<GamepadNavigationItem>();
        for (int index = 0; index < gamepadNavigationItemList.Count; ++index)
        {
          bool flag = false;
          if (navigationSettings2.try_stay_in_sub_group && this.SkipItemBecauseOfGroup(gamepadNavigationItemList[index], group1))
          {
            collection2.Add(gamepadNavigationItemList[index]);
            flag = true;
          }
          if (!flag && this.SkipItemBecauseOfGrid(gamepadNavigationItemList[index], pos, direction))
          {
            collection3.Add(gamepadNavigationItemList[index]);
            flag = true;
          }
          if (flag)
          {
            gamepadNavigationItemList.RemoveAt(index);
            --index;
          }
        }
        if (gamepadNavigationItemList.Count == 0 && collection3.Count > 0)
          gamepadNavigationItemList.AddRange((IEnumerable<GamepadNavigationItem>) collection3);
        if (gamepadNavigationItemList.Count == 0)
        {
          if (collection2.Count == 0)
            return;
          gamepadNavigationItemList.AddRange((IEnumerable<GamepadNavigationItem>) collection2);
        }
        GamepadNavigationItem gamepadNavigationItem = gamepadNavigationItemList[0];
        float num = gamepadNavigationItem.CalcDistToCurrentPos(pos, direction);
        for (int index = 1; index < gamepadNavigationItemList.Count; ++index)
        {
          float currentPos = gamepadNavigationItemList[index].CalcDistToCurrentPos(pos, direction);
          if ((double) currentPos < (double) num)
          {
            num = currentPos;
            gamepadNavigationItem = gamepadNavigationItemList[index];
          }
        }
        if (this.restore_last_in_group)
        {
          int group2 = gamepadNavigationItem.group;
          if (group2 != group1 && this.HaveSavedFocusForGroup(group2))
          {
            this.RestoreFocus(group2);
            return;
          }
        }
        this.SetFocusedItem(gamepadNavigationItem);
      }
    }
  }

  public bool SkipItemBecauseOfState(GamepadNavigationItem item)
  {
    return item.is_focused || !item.isActiveAndEnabled || !item.active;
  }

  public bool SkipItemBecauseOfGroup(GamepadNavigationItem item, int needed_group)
  {
    return item.group != needed_group;
  }

  public bool SkipItemBecauseOfSubGroup(GamepadNavigationItem item, int needed_group)
  {
    return item.sub_group != needed_group;
  }

  public bool SkipItemBecauseOfDirection(
    GamepadNavigationItem item,
    Vector2 current_pos,
    Direction direction)
  {
    return !item.CorrectDirection(current_pos, direction);
  }

  public bool SkipItemBecauseOfGrid(
    GamepadNavigationItem item,
    Vector2 current_pos,
    Direction direction)
  {
    return !item.CorrectGrid(current_pos, direction);
  }

  public void PrintItemsList(List<GamepadNavigationItem> items, string prefix)
  {
    StringBuilder stringBuilder = new StringBuilder(prefix + ": ");
    if (items.Count == 0)
    {
      stringBuilder.Append("empty");
    }
    else
    {
      foreach (GamepadNavigationItem gamepadNavigationItem in items)
      {
        stringBuilder.Append(gamepadNavigationItem.name);
        stringBuilder.Append(" ");
      }
    }
  }

  public enum OpenMethod
  {
    None,
    GetAll,
    GetAllAndFocus,
    GetAllAndSelect,
  }

  public delegate void ItemInteractionDelegate(GamepadNavigationItem item);
}
