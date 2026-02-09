// Decompiled with JetBrains decompiler
// Type: GameTabItemGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class GameTabItemGUI : MonoBehaviour
{
  public UIButton _button;
  public UI2DSprite _sprite;
  public UILabel _label;
  public GameGUI.TabType _tab_type;
  public Color _default_color;
  public BaseGameGUI _gui;
  public bool _selected;
  public GameObject go_active;

  public void Init(GameGUI.TabType tab_type, BaseGameGUI gui)
  {
    this._tab_type = tab_type;
    this._sprite = this.GetComponentInChildren<UI2DSprite>(true);
    this._button = this.GetComponentInChildren<UIButton>(true);
    this._default_color = this._button.defaultColor;
    this._label = this.GetComponentInChildren<UILabel>(true);
    this._label.text = GJL.L("tab_" + tab_type.ToString());
    this.name = tab_type.ToString();
    (this.gameObject.GetComponent<LocalizedLabel>() ?? this.gameObject.AddComponent<LocalizedLabel>()).token = "tab_" + tab_type.ToString();
    this._gui = gui;
    this._selected = false;
    this.Activate<GameTabItemGUI>();
  }

  public void OnPressed()
  {
    if (BaseGUI.IsLastClickRightButton())
      return;
    GUIElements.me.game_gui.SelectTab(this._tab_type);
    Sounds.OnGUITabClick();
  }

  public void Select()
  {
    if (this._selected)
      return;
    this._selected = true;
    this._button.defaultColor = this._button.pressed;
    this._button.enabled = false;
    Sounds.ignore_window_sounds = true;
    this._gui.OpenFromGameGUI();
    Sounds.ignore_window_sounds = false;
    this.go_active.SetActive(true);
  }

  public void Unselect()
  {
    if (!this._selected)
      return;
    this._selected = false;
    this._button.defaultColor = this._default_color;
    this._button.enabled = true;
    Sounds.ignore_window_sounds = true;
    this._gui.CloseFromGameGUI();
    Sounds.ignore_window_sounds = false;
    this.go_active.SetActive(false);
  }
}
