// Decompiled with JetBrains decompiler
// Type: TutorialItemGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class TutorialItemGUI : MonoBehaviour
{
  public UIWidget gamepad_frame;
  public string locale_token;
  public GameObject tutorial_window_obj;
  public EventDelegate on_pressed;
  public DLCEngine.DLCVersion dlc_version;
  public BaseTutorialGUI _menu;
  public GamepadNavigationItem _gamepad_item;
  public SmartSlider _slider;
  public SimpleOptionsSwitcher _options_switcher;
  public GameObject additional_go;
  public bool _gamepad_active;
  public bool _overed;
  public const float GAMEPAD_SELECTION_ANIM_TIME = 0.0f;

  public GamepadNavigationItem gamepad_item => this._gamepad_item;

  public bool gamepad_active
  {
    get => this._gamepad_active;
    set
    {
      this._gamepad_active = value;
      this._gamepad_item.active = this._gamepad_active;
    }
  }

  public void Init(BaseTutorialGUI menu)
  {
    if (!DLCEngine.IsDLCAvailable(this.dlc_version))
    {
      GUIElements.me.tutorial_windows_gui.RemoveNotAvailableTutorialItem(this);
    }
    else
    {
      this._menu = menu;
      this._gamepad_item = this.GetComponent<GamepadNavigationItem>();
      this._gamepad_item.SetCallbacks(new GJCommons.VoidDelegate(this.OnOver), (GJCommons.VoidDelegate) (() => this.OnOut()), new GJCommons.VoidDelegate(this.OnTutorialItemSelect));
      this._slider = this.GetComponentInChildren<SmartSlider>(true);
      if ((UnityEngine.Object) this._slider != (UnityEngine.Object) null)
        this._slider.Init();
      this._options_switcher = this.GetComponentInChildren<SimpleOptionsSwitcher>(true);
      LocalizedLabel componentInChildren = this.GetComponentInChildren<LocalizedLabel>(true);
      if ((UnityEngine.Object) componentInChildren != (UnityEngine.Object) null)
      {
        componentInChildren.token = this.locale_token;
        componentInChildren.Localize();
      }
      NGUIExtensionMethods.InitEventTriggers((MonoBehaviour) this, new EventDelegate.Callback(this.OnMouseOvered), new EventDelegate.Callback(this.OnMouseOuted), new EventDelegate.Callback(this.OnMousePressed), true);
    }
  }

  public void Show()
  {
    this._overed = false;
    this.gamepad_frame.Deactivate<UIWidget>();
    this.gamepad_frame.alpha = 0.0f;
  }

  public void SetupSlider(
    int value,
    int min,
    int max,
    Action<int> on_value_changed,
    int game_key_step = 1,
    int number_of_steps = 0)
  {
    if ((UnityEngine.Object) this._slider == (UnityEngine.Object) null)
    {
      Debug.Log((object) "Cannot setup slider because slider doesn't exist", (UnityEngine.Object) this);
    }
    else
    {
      this._slider.Open(value, min, max, on_value_changed, step_for_game_keys: game_key_step);
      this._slider.number_of_steps = number_of_steps;
    }
  }

  public void SetupOptions(
    int current_option_index,
    int max_option_index,
    string current_option_name,
    Action<int, UILabel> on_changed,
    bool call_onchanged_on_init = false)
  {
    if ((UnityEngine.Object) this._options_switcher == (UnityEngine.Object) null)
      Debug.Log((object) "Cannot setup slider because slider doesn't exist", (UnityEngine.Object) this);
    else
      this._options_switcher.Init(current_option_index, max_option_index, current_option_name, on_changed, call_onchanged_on_init);
  }

  public void OnMousePressed()
  {
    if (BaseGUI.for_gamepad)
      return;
    this.OnTutorialItemSelect();
  }

  public void OnMouseOvered()
  {
    if (BaseGUI.for_gamepad)
      return;
    this.OnOver();
  }

  public void OnMouseOuted()
  {
    if (BaseGUI.for_gamepad)
      return;
    this.OnOut();
  }

  public void OnTutorialItemSelect()
  {
    this.on_pressed.TryExecute();
    if ((UnityEngine.Object) this.tutorial_window_obj != (UnityEngine.Object) null && !string.IsNullOrEmpty(this.tutorial_window_obj.name))
      GUIElements.me.tutorial.Open(this.tutorial_window_obj.name);
    Sounds.OnGUIClick();
  }

  public void OnOver()
  {
    this.gamepad_frame.Activate<UIWidget>();
    this.gamepad_frame.ChangeAlpha(0.0f, 1f, 0.0f);
    this._overed = true;
    if ((UnityEngine.Object) this._menu != (UnityEngine.Object) null)
      this._menu.UpdatTip(true);
    if ((UnityEngine.Object) this._slider != (UnityEngine.Object) null)
      this._slider.game_keys_enabled = true;
    if ((UnityEngine.Object) this._options_switcher != (UnityEngine.Object) null)
      this._options_switcher.game_keys_enabled = true;
    if (Sounds.WasAnySoundPlayedThisFrame())
      return;
    Sounds.OnGUIHover(Sounds.ElementType.Button);
  }

  public void OnOut(bool animate = true)
  {
    this._overed = false;
    if (animate)
    {
      this.gamepad_frame.ChangeAlpha(this.gamepad_frame.alpha, 0.0f, 0.0f, (GJCommons.VoidDelegate) (() =>
      {
        if (this._overed)
          return;
        this.gamepad_frame.Deactivate<UIWidget>();
      }));
    }
    else
    {
      this.gamepad_frame.Deactivate<UIWidget>();
      this.gamepad_frame.alpha = 0.0f;
    }
    if ((UnityEngine.Object) this._slider != (UnityEngine.Object) null)
      this._slider.game_keys_enabled = false;
    if (!((UnityEngine.Object) this._options_switcher != (UnityEngine.Object) null))
      return;
    this._options_switcher.game_keys_enabled = false;
  }

  public void OnValidate()
  {
    if (Application.isPlaying || string.IsNullOrEmpty(this.locale_token) || (UnityEngine.Object) this.GetComponentInParent<UIRoot>() == (UnityEngine.Object) null)
      return;
    if (this.name != this.locale_token)
      this.name = this.locale_token;
    if (this.GetComponentInChildren<UILabel>(true).text != this.locale_token)
      this.GetComponentInChildren<UILabel>(true).text = this.locale_token;
    if (!(this.GetComponentInChildren<LocalizedLabel>(true).token != this.locale_token))
      return;
    this.GetComponentInChildren<LocalizedLabel>(true).token = this.locale_token;
  }

  [CompilerGenerated]
  public void \u003CInit\u003Eb__18_0() => this.OnOut();

  [CompilerGenerated]
  public void \u003COnOut\u003Eb__27_0()
  {
    if (this._overed)
      return;
    this.gamepad_frame.Deactivate<UIWidget>();
  }
}
