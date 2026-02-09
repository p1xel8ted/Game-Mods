// Decompiled with JetBrains decompiler
// Type: TimeMachineItemGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using LinqTools;
using System;
using UnityEngine;

#nullable disable
public class TimeMachineItemGUI : MonoBehaviour
{
  public const string CLOSED_ITEM_LOCALE = "dlc_cutscene_name_locked";
  public const string CLOSED_DATE_LOCALE = "dlc_cutscene_date_locked";
  [SerializeField]
  public UIButton _button;
  [SerializeField]
  public UILabel _scene_name_label;
  [SerializeField]
  public UILabel _scene_date_label;
  [SerializeField]
  public UI2DSprite _icon;
  [SerializeField]
  [Header("Settings")]
  public Color _closed_color;
  [SerializeField]
  public Color _opened_color;
  [SerializeField]
  public UnityEngine.Sprite _closed_icon;
  [Space]
  [SerializeField]
  public GameObject _disabled_button_image;
  [SerializeField]
  public GameObject _selection_frame;
  public string _scene_id;
  public UnityEngine.Sprite _scene_sprite;
  public string _flow_script;
  public string _name_locale_key;
  public string _date_locale_key;
  public Action<string> _on_pressed;
  public GamepadNavigationItem _gamepad_navigation_item;
  public TimeMachineGUI _time_machine_gui;
  public PanelAutoScroll auto_scroll;

  public GamepadNavigationItem gamepad_navigation_item
  {
    get
    {
      if ((UnityEngine.Object) this._gamepad_navigation_item == (UnityEngine.Object) null)
        this._gamepad_navigation_item = this.GetComponentInChildren<GamepadNavigationItem>(true);
      return this._gamepad_navigation_item;
    }
  }

  public void Initialize(
    string scene_id,
    string item_name,
    string flow_script,
    Action<string> on_pressed)
  {
    this._scene_id = scene_id;
    this._scene_sprite = EasySpritesCollection.GetSprite(item_name);
    this._flow_script = flow_script;
    this._name_locale_key = scene_id + "_name";
    this._date_locale_key = scene_id + "_date";
    this._on_pressed = on_pressed;
    this._time_machine_gui = GUIElements.me.time_machine_gui;
    if (!BaseGUI.for_gamepad || !((UnityEngine.Object) this.gamepad_navigation_item != (UnityEngine.Object) null))
      return;
    this.gamepad_navigation_item.SetCallbacks(new GJCommons.VoidDelegate(this.OnOver), new GJCommons.VoidDelegate(this.OnOut), new GJCommons.VoidDelegate(this.OnItemAction));
  }

  public void CheckEnabled(bool is_enabled)
  {
    if (is_enabled)
    {
      this._scene_name_label.color = this._opened_color;
      this._scene_date_label.color = this._opened_color;
      this._scene_name_label.applyGradient = true;
      this._scene_date_label.applyGradient = true;
      this._icon.sprite2D = this._scene_sprite;
      this._icon.ResizeByContent();
      this._button.enabled = true;
      this._disabled_button_image.SetActive(false);
      this._scene_name_label.text = GJL.L(this._name_locale_key);
      this._scene_date_label.text = GJL.L(this._date_locale_key);
    }
    else
    {
      this._scene_name_label.color = this._closed_color;
      this._scene_date_label.color = this._closed_color;
      this._scene_name_label.applyGradient = false;
      this._scene_date_label.applyGradient = false;
      this._icon.sprite2D = this._closed_icon;
      this._icon.ResizeByContent();
      this._button.enabled = false;
      this._disabled_button_image.SetActive(true);
      this._scene_name_label.text = GJL.L("dlc_cutscene_name_locked");
      this._scene_date_label.text = GJL.L("dlc_cutscene_date_locked");
    }
  }

  public void OnPlayPressed()
  {
    Action<string> onPressed = this._on_pressed;
    if (onPressed == null)
      return;
    onPressed(this._flow_script);
  }

  public void OnOver()
  {
    if (!BaseGUI.for_gamepad)
      return;
    if ((UnityEngine.Object) this.auto_scroll != (UnityEngine.Object) null && this._time_machine_gui.items.Count > 0 && ((UnityEngine.Object) this == (UnityEngine.Object) this._time_machine_gui.items[0] || (UnityEngine.Object) this == (UnityEngine.Object) this._time_machine_gui.items.Last<TimeMachineItemGUI>()) && (UnityEngine.Object) this.auto_scroll != (UnityEngine.Object) null)
      this.auto_scroll.Perform();
    this._time_machine_gui.button_tips.Print(GameKeyTip.Select("select", this._button.enabled), GameKeyTip.Close());
  }

  public void OnOut()
  {
  }

  public void OnItemAction()
  {
    if (!this._button.enabled)
      return;
    this.OnPlayPressed();
  }

  public void OnMouseOvered()
  {
    if (BaseGUI.for_gamepad)
      return;
    this._selection_frame.gameObject.SetActive(true);
  }

  public void OnMouseOuted()
  {
    if (BaseGUI.for_gamepad)
      return;
    this._selection_frame.gameObject.SetActive(false);
  }
}
