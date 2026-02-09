// Decompiled with JetBrains decompiler
// Type: BaseGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using LinqTools;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BaseGUI : MonoBehaviour
{
  public Dictionary<GameKey, GJCommons.BoolDelegate> gamekey_delegates = new Dictionary<GameKey, GJCommons.BoolDelegate>();
  public static GJCommons.VoidDelegate on_all_closed;
  public static List<BaseGUI> opened_windows = new List<BaseGUI>();
  public static bool _opened_for_gamepad;
  public static BaseGUI _active_gui;
  public bool add_to_opened_stack = true;
  public bool _is_shown;
  public int _open_frame;
  public int _hide_frame;
  public GamepadNavigationController _gamepad_navigation_controller;
  public ButtonTipsStr _button_tips;
  public UIRect _ui_rect;
  public bool _ui_rect_cached;
  public GJCommons.VoidDelegate _on_hide;
  public bool _call_on_hide_only_one_time;
  public bool _need_recalc_anchors;
  public List<ScrollWithKeyboard> _keyboard_scrolls = new List<ScrollWithKeyboard>();

  public static event BaseGUI.OnAnyWindowStateChanged on_window_opened;

  public static event BaseGUI.OnAnyWindowStateChanged on_window_closed;

  public static bool for_gamepad
  {
    get => BaseGUI._opened_for_gamepad;
    set => BaseGUI._opened_for_gamepad = value;
  }

  public static bool all_guis_closed => BaseGUI.opened_windows.Count == 0;

  public static BaseGUI active_gui => BaseGUI._active_gui;

  public bool is_just_opened => Mathf.Abs(this._open_frame - Time.frameCount) <= 1;

  public bool is_just_hided => Mathf.Abs(this._hide_frame - Time.frameCount) <= 1;

  public bool is_shown => this._is_shown;

  public bool is_shown_and_top
  {
    get
    {
      return this._is_shown && BaseGUI.opened_windows.Count > 0 && (UnityEngine.Object) BaseGUI.opened_windows.LastElement<BaseGUI>() == (UnityEngine.Object) this;
    }
  }

  public GamepadNavigationController gamepad_controller => this._gamepad_navigation_controller;

  public ButtonTipsStr button_tips => this._button_tips;

  public UIRect ui_rect
  {
    get
    {
      if (this._ui_rect_cached)
        return this._ui_rect;
      this._ui_rect_cached = true;
      return this._ui_rect = this.GetComponent<UIRect>();
    }
  }

  public string window_type_name
  {
    get
    {
      System.Type type = this.GetType();
      return !System.Type.op_Equality(type, (System.Type) null) ? type.Name : "[???]";
    }
  }

  public virtual void Open() => this.Open(true);

  public void Open(bool play_open_sound)
  {
    Debug.Log((object) ("<color=green>Open GUI:</color> " + this.window_type_name));
    if (this._is_shown)
      Debug.LogWarning((object) $"window {this.name} is already opened", (UnityEngine.Object) this);
    this.UpdateSourceType(false);
    this._open_frame = Time.frameCount;
    this._is_shown = true;
    this.gameObject.SetActive(true);
    LazyInput.WaitForReleaseNavigationKeys();
    this.InitPlatformDependentStuff();
    this.UpdateLocalizedLabels();
    this.UpdatePixelPerfect();
    if (this.add_to_opened_stack)
    {
      if (play_open_sound)
        Sounds.OnWindowOpened();
      BaseGUI baseGui = BaseGUI.opened_windows.LastElement<BaseGUI>();
      if ((UnityEngine.Object) baseGui != (UnityEngine.Object) null && (UnityEngine.Object) baseGui != (UnityEngine.Object) this)
        baseGui.OnHiddenByAnotherGUI();
      if (BaseGUI.opened_windows.Contains(this) && (UnityEngine.Object) baseGui == (UnityEngine.Object) this)
        BaseGUI.opened_windows.Remove(this);
      BaseGUI.opened_windows.Add(this);
      BaseGUI._active_gui = this;
    }
    if (BaseGUI.on_window_opened != null)
      BaseGUI.on_window_opened(this);
    if (BaseGUI.for_gamepad)
      return;
    this._keyboard_scrolls = ((IEnumerable<ScrollWithKeyboard>) this.GetComponentsInChildren<ScrollWithKeyboard>(true)).ToList<ScrollWithKeyboard>();
  }

  public void UpdateSourceType(bool force)
  {
    if (!force && (!this.add_to_opened_stack || BaseGUI.opened_windows.Count != 0))
      return;
    BaseGUI._opened_for_gamepad = LazyInput.gamepad_active;
  }

  public static void UpdateSourceType()
  {
    if (BaseGUI.opened_windows.Count != 0)
      return;
    BaseGUI._opened_for_gamepad = LazyInput.gamepad_active;
  }

  public virtual void UpdateLocalizedLabels()
  {
    foreach (LocalizedLabel componentsInChild in this.GetComponentsInChildren<LocalizedLabel>(true))
      componentsInChild.Localize();
    GJL.EnsureChildLabelsHasCorrectFont(this.gameObject);
  }

  public void UpdatePixelPerfect()
  {
    foreach (PixelPerfectGUI componentsInChild in this.GetComponentsInChildren<PixelPerfectGUI>())
      componentsInChild.LateUpdate();
  }

  public virtual void Update()
  {
    if (!this.is_shown_and_top)
      return;
    foreach (GameKey key in this.gamekey_delegates.Keys)
    {
      if (LazyInput.GetKeyDown(key) && this.gamekey_delegates[key]())
        LazyInput.ClearKeyDown(key);
    }
    if (this.CanCloseWithRightClick() && Input.GetMouseButtonDown(1))
      this.OnRightClick();
    if (BaseGUI.for_gamepad || this._keyboard_scrolls == null || this._keyboard_scrolls.Count <= 0)
      return;
    foreach (ScrollWithKeyboard keyboardScroll in this._keyboard_scrolls)
    {
      if (!((UnityEngine.Object) keyboardScroll.scroll_view == (UnityEngine.Object) null))
      {
        Vector2 direction = LazyInput.GetDirection();
        float num = 0.0f;
        switch (keyboardScroll.scroll_type)
        {
          case ScrollWithKeyboard.KeyboardScrollType.Vertical:
            if (keyboardScroll.scroll_view.shouldMoveVertically)
            {
              num = direction.y * keyboardScroll.scroll_sensivity;
              break;
            }
            break;
          case ScrollWithKeyboard.KeyboardScrollType.Horizontal:
            if (keyboardScroll.scroll_view.shouldMoveHorizontally)
            {
              num = direction.x * keyboardScroll.scroll_sensivity;
              break;
            }
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
        if (!num.EqualsTo(0.0f))
          keyboardScroll.scroll_view.Scroll(num);
      }
    }
  }

  public virtual bool CanCloseWithRightClick() => false;

  public virtual void OnAboveWindowClosed()
  {
  }

  public virtual void OnHiddenByAnotherGUI()
  {
    if (!(bool) (UnityEngine.Object) this.gamepad_controller)
      return;
    this.gamepad_controller.Disable();
  }

  public void InitPlatformDependentStuff()
  {
    if ((UnityEngine.Object) this.gamepad_controller != (UnityEngine.Object) null)
    {
      if (BaseGUI.for_gamepad)
        this.gamepad_controller.Enable();
      else if (this.gamepad_controller.is_enabled)
        this.gamepad_controller.Disable();
    }
    foreach (PlatformDependentElement componentsInChild in this.GetComponentsInChildren<PlatformDependentElement>(true))
      componentsInChild.Init(BaseGUI.for_gamepad);
  }

  public virtual void Hide(bool play_hide_sound = true)
  {
    if (MainGame.game_started)
      Debug.Log((object) ("<color=green>Hide GUI:</color> " + this.window_type_name));
    this._hide_frame = Time.frameCount;
    this.gameObject.SetActive(false);
    this._is_shown = false;
    LazyInput.ClearAllKeysDown();
    LazyInput.WaitForReleaseMouseKeys();
    if ((UnityEngine.Object) this._gamepad_navigation_controller != (UnityEngine.Object) null)
      this._gamepad_navigation_controller.Disable();
    if (BaseGUI.opened_windows.Contains(this))
      BaseGUI.opened_windows.Remove(this);
    BaseGUI._active_gui = BaseGUI.opened_windows.Count == 0 ? (BaseGUI) null : BaseGUI.opened_windows.LastElement<BaseGUI>();
    if (this.add_to_opened_stack && BaseGUI.on_window_closed != null)
      BaseGUI.on_window_closed(this);
    if (this.add_to_opened_stack)
    {
      if (play_hide_sound && MainGame.game_started)
        Sounds.OnClosePressed();
      if (BaseGUI.opened_windows.Count == 0)
        BaseGUI.on_all_closed.TryInvoke();
      else
        BaseGUI.opened_windows.LastElement<BaseGUI>().OnAboveWindowClosed();
    }
    GJCommons.VoidDelegate onHide = this._on_hide;
    if (this._call_on_hide_only_one_time)
      this._on_hide = (GJCommons.VoidDelegate) null;
    onHide.TryInvoke();
    MainGame.me.player.components.interaction.RedrawCurrentInteractiveHint();
    BuffsLogics.CheckBuffsGiveConditions();
  }

  public virtual void Init()
  {
    this._gamepad_navigation_controller = this.GetComponent<GamepadNavigationController>();
    ButtonTipsStr[] componentsInChildren = this.GetComponentsInChildren<ButtonTipsStr>(true);
    if (componentsInChildren.Length != 0)
    {
      foreach (ButtonTipsStr buttonTipsStr in componentsInChildren)
      {
        if (buttonTipsStr.gameObject.activeSelf)
        {
          this._button_tips = buttonTipsStr;
          break;
        }
      }
      if ((UnityEngine.Object) this._button_tips == (UnityEngine.Object) null)
        this._button_tips = componentsInChildren[0];
    }
    if ((UnityEngine.Object) this._button_tips != (UnityEngine.Object) null)
      this._button_tips.Clear();
    foreach (Tooltip componentsInChild in this.GetComponentsInChildren<Tooltip>(true))
      componentsInChild.Init();
    this.Hide();
    this.gamekey_delegates = new Dictionary<GameKey, GJCommons.BoolDelegate>()
    {
      {
        GameKey.Select,
        new GJCommons.BoolDelegate(this.OnPressedSelect)
      },
      {
        GameKey.Back,
        new GJCommons.BoolDelegate(this.OnPressedBack)
      },
      {
        GameKey.Option1,
        new GJCommons.BoolDelegate(this.OnPressedOption1)
      },
      {
        GameKey.Option2,
        new GJCommons.BoolDelegate(this.OnPressedOption2)
      },
      {
        GameKey.SliderDec,
        new GJCommons.BoolDelegate(this.OnPressedSliderDec)
      },
      {
        GameKey.SliderInc,
        new GJCommons.BoolDelegate(this.OnPressedSliderInc)
      },
      {
        GameKey.PrevTab,
        new GJCommons.BoolDelegate(this.OnPressedPrevTab)
      },
      {
        GameKey.NextTab,
        new GJCommons.BoolDelegate(this.OnPressedNextTab)
      },
      {
        GameKey.PrevSubTab,
        new GJCommons.BoolDelegate(this.OnPressedPrevSubTab)
      },
      {
        GameKey.NextSubTub,
        new GJCommons.BoolDelegate(this.OnPressedNextSubTab)
      },
      {
        GameKey.Left,
        new GJCommons.BoolDelegate(this.OnPressedLeft)
      },
      {
        GameKey.Right,
        new GJCommons.BoolDelegate(this.OnPressedRight)
      },
      {
        GameKey.Up,
        new GJCommons.BoolDelegate(this.OnPressedUp)
      },
      {
        GameKey.Down,
        new GJCommons.BoolDelegate(this.OnPressedDown)
      }
    };
  }

  public void SetOnHide(GJCommons.VoidDelegate on_hide, bool call_only_one_time = true)
  {
    this._on_hide = on_hide;
    this._call_on_hide_only_one_time = call_only_one_time;
  }

  public virtual void OnClosePressed() => this.Hide();

  public virtual void OnInputSourceChanged()
  {
  }

  public void SoundOnMouseOverCloseButton() => Sounds.OnGUIHover(Sounds.ElementType.ItemCell);

  public void SoundOnMouseOverButton() => Sounds.OnGUIHover();

  public virtual bool OnPressedSelect()
  {
    if (!this.gamepad_controller.is_enabled || !this.gamepad_controller.auto_select)
      return false;
    this.gamepad_controller.SelectFocusedItem();
    return true;
  }

  public virtual bool OnPressedBack() => false;

  public virtual bool OnPressedOption1() => false;

  public virtual bool OnPressedOption2() => false;

  public virtual bool OnPressedSliderDec() => false;

  public virtual bool OnPressedSliderInc() => false;

  public virtual bool OnPressedPrevTab() => false;

  public virtual bool OnPressedNextTab() => false;

  public virtual bool OnPressedPrevSubTab() => false;

  public virtual bool OnPressedNextSubTab() => false;

  public virtual bool OnPressedLeft()
  {
    if (!LazyInput.gamepad_active || !this.gamepad_controller.is_enabled)
      return false;
    this.gamepad_controller.Navigate(Direction.Left);
    return true;
  }

  public virtual bool OnPressedRight()
  {
    if (!LazyInput.gamepad_active || !this.gamepad_controller.is_enabled)
      return false;
    this.gamepad_controller.Navigate(Direction.Right);
    return true;
  }

  public virtual bool OnPressedUp()
  {
    if (!LazyInput.gamepad_active || !this.gamepad_controller.is_enabled)
      return false;
    this.gamepad_controller.Navigate(Direction.Up);
    return true;
  }

  public virtual bool OnPressedDown()
  {
    if (!LazyInput.gamepad_active || !this.gamepad_controller.is_enabled)
      return false;
    this.gamepad_controller.Navigate(Direction.Down);
    return true;
  }

  public void RecalcAllAnchors()
  {
    this.DoRecalcAllAnchors();
    this._need_recalc_anchors = true;
  }

  public void DoRecalcAllAnchors()
  {
    foreach (UIRect componentsInChild in this.GetComponentsInChildren<UIWidget>(true))
      componentsInChild.UpdateAnchors();
    foreach (UITable componentsInChild in this.GetComponentsInChildren<UITable>(true))
    {
      componentsInChild.repositionNow = true;
      componentsInChild.Reposition();
      componentsInChild.repositionNow = true;
    }
    foreach (UIRect componentsInChild in this.GetComponentsInChildren<UIWidget>(true))
      componentsInChild.UpdateAnchors();
  }

  public void LateUpdate()
  {
    if (!this._need_recalc_anchors)
      return;
    this.DoRecalcAllAnchors();
    this._need_recalc_anchors = false;
  }

  public void UpdateAllAnchors()
  {
    this.BroadcastMessage("UpdateAnchors", SendMessageOptions.DontRequireReceiver);
    this.GetComponentInParent<UIPanel>().BroadcastMessage("UpdateAnchors", SendMessageOptions.DontRequireReceiver);
  }

  public static bool IsLastClickRightButton() => UICamera.currentTouchID == -2;

  public virtual void OnRightClick() => this.OnClosePressed();

  public delegate void OnAnyWindowStateChanged(BaseGUI window_obj);
}
