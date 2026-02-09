// Decompiled with JetBrains decompiler
// Type: GameGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using LinqTools;
using System;
using System.Collections.Generic;

#nullable disable
public class GameGUI : BaseGUI
{
  public UILabel LB;
  public UILabel RB;
  public Dictionary<GameGUI.TabType, GameTabItemGUI> _tabs;
  public UITable _tabs_grid;
  public GameGUI.TabType _current_tab_type;
  public int _current_tab_index;
  public Dictionary<GameGUI.TabType, BaseGameGUI> TABS;

  public override void Init()
  {
    GameTabItemGUI componentInChildren = this.GetComponentInChildren<GameTabItemGUI>();
    this.TABS = new Dictionary<GameGUI.TabType, BaseGameGUI>()
    {
      {
        GameGUI.TabType.Inventory,
        (BaseGameGUI) GUIElements.me.inventory
      },
      {
        GameGUI.TabType.Techs,
        (BaseGameGUI) GUIElements.me.tech_tree
      },
      {
        GameGUI.TabType.NPCs,
        (BaseGameGUI) GUIElements.me.npcs_list
      },
      {
        GameGUI.TabType.Map,
        (BaseGameGUI) GUIElements.me.map
      }
    };
    this._tabs = new Dictionary<GameGUI.TabType, GameTabItemGUI>();
    foreach (GameGUI.TabType key in this.TABS.Keys)
    {
      GameTabItemGUI gameTabItemGui = componentInChildren.Copy<GameTabItemGUI>();
      gameTabItemGui.Init(key, this.TABS[key]);
      this._tabs.Add(key, gameTabItemGui);
    }
    componentInChildren.Deactivate<GameTabItemGUI>();
    this._tabs_grid = this.GetComponentInChildren<UITable>(true);
    this._tabs_grid.Reposition();
    this.gameObject.Deactivate();
  }

  public override void Open()
  {
    GUIElements.me.hud.OnAnyWindowOpened((BaseGUI) this);
    Sounds.OnWindowOpened();
    base.Open();
    this._tabs_grid.Reposition();
    this.SelectTab(this._current_tab_type);
    MainGame.SetPausedMode(true);
    if ((UnityEngine.Object) this.LB != (UnityEngine.Object) null)
      this.LB.text = GameKeyTip.GetIcon(GameKey.RotateLeft);
    if (!((UnityEngine.Object) this.RB != (UnityEngine.Object) null))
      return;
    this.RB.text = GameKeyTip.GetIcon(GameKey.RotateRight);
  }

  public void OpenAtTab(GameGUI.TabType tab)
  {
    this._current_tab_type = tab;
    this.Open();
  }

  public void OpenOrSelectTab(GameGUI.TabType tab)
  {
    if (this.is_shown)
    {
      this.SelectTab(tab);
    }
    else
    {
      this._current_tab_type = tab;
      this.Open();
    }
  }

  public override void Hide(bool play_hide_sound = true)
  {
    GUIElements.me.hud.OnAnyWindowClosed((BaseGUI) this);
    Sounds.OnClosePressed();
    base.Hide(play_hide_sound);
    foreach (GameTabItemGUI gameTabItemGui in this._tabs.Values)
      gameTabItemGui.Unselect();
    MainGame.me.player.components.interaction.UpdateNearestHint();
    MainGame.SetPausedMode(false);
  }

  public void NextTab(int step)
  {
    if (GUIElements.me.equip_to_toolbar.is_shown)
      GUIElements.me.equip_to_toolbar.Hide(false);
    this._current_tab_index += step;
    if (this._current_tab_index < 0)
      this._current_tab_index = this._tabs.Values.Count - 1;
    else if (this._current_tab_index >= this._tabs.Values.Count)
      this._current_tab_index = 0;
    this._tabs.Values.ElementAt<GameTabItemGUI>(this._current_tab_index).OnPressed();
  }

  public void SelectTab(GameGUI.TabType tab_type)
  {
    GameTabItemGUI gameTabItemGui = (GameTabItemGUI) null;
    int num = -1;
    foreach (GameGUI.TabType key in this._tabs.Keys)
    {
      ++num;
      if (tab_type == key)
      {
        gameTabItemGui = this._tabs[key];
        this._current_tab_index = num;
      }
      else
        this._tabs[key].Unselect();
    }
    if ((UnityEngine.Object) gameTabItemGui != (UnityEngine.Object) null)
      gameTabItemGui.Select();
    this._current_tab_type = tab_type;
  }

  public override void Update()
  {
    base.Update();
    if (LazyInput.GetKeyDown(GameKey.GameGUI))
    {
      this.Hide(true);
      LazyInput.ClearKeyDown(GameKey.GameGUI);
    }
    if (GUIElements.me.item_count.is_shown)
      return;
    if (LazyInput.GetKeyDown(GameKey.PrevTab))
      this.NextTab(-1);
    if (!LazyInput.GetKeyDown(GameKey.NextTab))
      return;
    this.NextTab(1);
  }

  public override bool OnPressedBack()
  {
    this.Hide(true);
    return true;
  }

  public override bool OnPressedPrevTab()
  {
    if (GUIElements.me.item_count.is_shown)
      return false;
    this.NextTab(-1);
    return true;
  }

  public override bool OnPressedNextTab()
  {
    if (GUIElements.me.item_count.is_shown)
      return false;
    this.NextTab(1);
    return true;
  }

  [Serializable]
  public enum TabType
  {
    Inventory,
    Techs,
    NPCs,
    Bodies,
    Map,
  }
}
