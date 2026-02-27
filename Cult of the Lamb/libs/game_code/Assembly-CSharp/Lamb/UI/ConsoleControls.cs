// Decompiled with JetBrains decompiler
// Type: Lamb.UI.ConsoleControls
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public MMSelectable_HorizontalSelector _controlLayout;
  [SerializeField]
  public MMHorizontalSelector _controlLayoutSelector;
  [Header("Layouts")]
  [SerializeField]
  public GamepadLayout _layout;
  [SerializeField]
  public GamepadBindings _bindings;
  public UIMenuBase _currentMenu;
  public string[] _allLayouts = new string[4]
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

  public void Update()
  {
    if (!((UnityEngine.Object) this._currentMenu == (UnityEngine.Object) this._bindings) || MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable == null)
      return;
    KeybindItemProxy component1;
    if (InputManager.UI.GetResetBindingButtonDown() && MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.Selectable.gameObject.TryGetComponent<KeybindItemProxy>(out component1))
    {
      if (!component1.KeybindItem.IsRebindable)
        return;
      BindingItem joystickBinding = component1.KeybindItem.JoystickBinding;
      ControllerMap controllerMapForCategory = InputManager.General.GetControllerMapForCategory(joystickBinding.Category, joystickBinding.ControllerType, MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer);
      if (controllerMapForCategory != null)
        ControlSettingsUtilities.ResetBinding(controllerMapForCategory, joystickBinding.Action, joystickBinding.AxisContribution);
    }
    KeybindItemProxy component2;
    if (!InputManager.UI.GetUnbindButtonDown() || !MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.Selectable.gameObject.TryGetComponent<KeybindItemProxy>(out component2) || !component2.KeybindItem.IsRebindable)
      return;
    BindingItem joystickBinding1 = component2.KeybindItem.JoystickBinding;
    ControllerMap controllerMapForCategory1 = InputManager.General.GetControllerMapForCategory(joystickBinding1.Category, joystickBinding1.ControllerType, MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer);
    if (controllerMapForCategory1 == null)
      return;
    ActionElementMap actionElementMap = controllerMapForCategory1.GetActionElementMap(joystickBinding1.Action, joystickBinding1.AxisContribution);
    if (actionElementMap == null || !ControlSettingsUtilities.DeleteElementMap(controllerMapForCategory1, actionElementMap))
      return;
    if ((UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer == (UnityEngine.Object) null || MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer.isLamb)
      SettingsManager.Settings.Control.GamepadBindingsUnbound.Add(actionElementMap.ToUnboundBinding());
    else
      SettingsManager.Settings.Control.GamepadBindingsUnbound_P2.Add(actionElementMap.ToUnboundBinding());
  }

  public override bool ValidInputType(InputType inputType) => inputType != InputType.Keyboard;

  public override void Configure(InputType inputType)
  {
    this._bindings.Configure(inputType);
    this._layout.Configure(InputManager.General.GetLastActiveController(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer));
    this.Configure(SettingsManager.Settings.Control);
  }

  public override void Configure(SettingsData.ControlSettings controlSettings)
  {
    this._controlLayoutSelector.ContentIndex = (UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer == (UnityEngine.Object) null || MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer.isLamb ? controlSettings.GamepadLayout : controlSettings.GamepadLayout_P2;
    this.UpdateScreen(this._controlLayoutSelector.ContentIndex);
  }

  public void OnLayoutSelectionChanged(int index)
  {
    Debug.Log((object) $"ControlSettings(Gamepad) - Layout changed to {index}".Colour(Color.yellow));
    if ((UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer == (UnityEngine.Object) null || MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer.isLamb)
      SettingsManager.Settings.Control.GamepadLayout = index;
    else
      SettingsManager.Settings.Control.GamepadLayout_P2 = index;
    ControlSettingsUtilities.SetGamepadLayout();
    this.UpdateScreen(index);
  }

  public void UpdateScreen(int layout)
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
