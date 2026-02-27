// Decompiled with JetBrains decompiler
// Type: Lamb.UI.ConsoleControls
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Rewired;
using src.Extensions;
using src.UINavigator;
using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class ConsoleControls : ControlsScreenBase
{
  [Header("UI Controls")]
  [SerializeField]
  private MMSelectable_HorizontalSelector _controlLayout;
  [SerializeField]
  private MMHorizontalSelector _controlLayoutSelector;
  [Header("Layouts")]
  [SerializeField]
  private GamepadLayout _layout;
  [SerializeField]
  private GamepadBindings _bindings;
  private UIMenuBase _currentMenu;
  private string[] _allLayouts = new string[4]
  {
    "UI/Settings/Controls/Layout/LayoutA",
    "UI/Settings/Controls/Layout/LayoutB",
    "UI/Settings/Controls/Layout/LayoutC",
    "UI/Settings/Graphics/QualitySettingsOption/Custom"
  };

  public override void Awake()
  {
    base.Awake();
    this._controlLayoutSelector.LocalizeContent = true;
    this._controlLayoutSelector.PrefillContent(this._allLayouts);
    this._controlLayoutSelector.OnSelectionChanged += new Action<int>(this.OnLayoutSelectionChanged);
  }

  private void Update()
  {
    if (!((UnityEngine.Object) this._currentMenu == (UnityEngine.Object) this._bindings) || MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable == null)
      return;
    KeybindItemProxy component1;
    if (InputManager.UI.GetResetBindingButtonDown() && MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.Selectable.gameObject.TryGetComponent<KeybindItemProxy>(out component1))
    {
      if (!component1.KeybindItem.IsRebindable)
        return;
      BindingItem joystickBinding = component1.KeybindItem.JoystickBinding;
      ControllerMap controllerMapForCategory = InputManager.General.GetControllerMapForCategory(joystickBinding.Category, joystickBinding.ControllerType);
      if (controllerMapForCategory != null)
        ControlSettingsUtilities.ResetBinding(controllerMapForCategory, joystickBinding.Action, joystickBinding.AxisContribution);
    }
    KeybindItemProxy component2;
    if (!InputManager.UI.GetUnbindButtonDown() || !MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.Selectable.gameObject.TryGetComponent<KeybindItemProxy>(out component2) || !component2.KeybindItem.IsRebindable)
      return;
    BindingItem joystickBinding1 = component2.KeybindItem.JoystickBinding;
    ControllerMap controllerMapForCategory1 = InputManager.General.GetControllerMapForCategory(joystickBinding1.Category, joystickBinding1.ControllerType);
    ActionElementMap actionElementMap = controllerMapForCategory1.GetActionElementMap(joystickBinding1.Action, joystickBinding1.AxisContribution);
    if (actionElementMap == null || !ControlSettingsUtilities.DeleteElementMap(controllerMapForCategory1, actionElementMap))
      return;
    if (joystickBinding1.ControllerType == ControllerType.Keyboard)
    {
      SettingsManager.Settings.Control.KeyboardBindingsUnbound.Add(actionElementMap.ToUnboundBinding());
    }
    else
    {
      if (joystickBinding1.ControllerType != ControllerType.Mouse)
        return;
      SettingsManager.Settings.Control.MouseBindingsUnbound.Add(actionElementMap.ToUnboundBinding());
    }
  }

  public override bool ValidInputType(InputType inputType) => inputType != InputType.Keyboard;

  public override void Configure(InputType inputType)
  {
    this._bindings.Configure(inputType);
    this._layout.Configure(InputManager.General.GetLastActiveController());
    this.Configure(SettingsManager.Settings.Control);
  }

  public override void Configure(SettingsData.ControlSettings controlSettings)
  {
    this._controlLayoutSelector.ContentIndex = controlSettings.GamepadLayout;
    this.UpdateScreen(controlSettings.GamepadLayout);
  }

  private void OnLayoutSelectionChanged(int index)
  {
    Debug.Log((object) $"ControlSettings(Gamepad) - Layout changed to {index}".Colour(Color.yellow));
    SettingsManager.Settings.Control.GamepadLayout = index;
    ControlSettingsUtilities.SetGamepadLayout();
    this.UpdateScreen(index);
  }

  private void UpdateScreen(int layout)
  {
    UIMenuBase uiMenuBase = layout != 3 ? (UIMenuBase) this._layout : (UIMenuBase) this._bindings;
    if (!((UnityEngine.Object) this._currentMenu != (UnityEngine.Object) uiMenuBase))
      return;
    if ((UnityEngine.Object) this._currentMenu != (UnityEngine.Object) null)
      this._currentMenu.Hide();
    uiMenuBase.Show((UnityEngine.Object) this._currentMenu == (UnityEngine.Object) null);
    this._currentMenu = uiMenuBase;
    this._controlLayout.navigation = this._controlLayout.navigation with
    {
      mode = Navigation.Mode.Explicit,
      selectOnDown = !((UnityEngine.Object) this._currentMenu == (UnityEngine.Object) this._bindings) ? this._layout.ProvideSelectableForLayoutSelector() : this._bindings.ProvideSelectableForLayoutSelector()
    };
  }

  public override bool ShowBindingPrompts()
  {
    return (UnityEngine.Object) this._currentMenu == (UnityEngine.Object) this._bindings;
  }
}
