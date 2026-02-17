// Decompiled with JetBrains decompiler
// Type: ControlSettingsUtilities
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Rewired;
using src.Extensions;
using src.UINavigator;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ControlSettingsUtilities
{
  public const int kCustomBindingsIndex = 3;
  public static System.Action OnGamepadLayoutChanged;
  public static Action<Binding> OnRebind;
  public static Action<int> OnBindingReset;
  public static System.Action OnGamepadPromptsChanged;

  public static void ApplyBindings(ControllerMap controllerMap, List<Binding> bindings)
  {
    foreach (Binding binding in bindings)
    {
      if (controllerMap.categoryId == binding.Category)
        ControlSettingsUtilities.ApplyBinding(controllerMap, binding);
    }
  }

  public static bool ApplyBinding(ControllerMap controllerMap, Binding binding)
  {
    ActionElementMap actionElementMap = controllerMap.GetActionElementMap(binding.Action, binding.AxisContribution);
    if (actionElementMap == null)
    {
      if (controllerMap.CreateElementMap(binding.ToElementAssigment()))
      {
        Debug.Log((object) "Map created".Colour(Color.cyan));
        return true;
      }
      Debug.Log((object) $"Unable to assign binding for {binding.Action} in Binding attempt!".Colour(Color.red));
      return false;
    }
    ElementAssignment elementAssignment = binding.ToElementAssignment(actionElementMap);
    if (controllerMap.ReplaceElementMap(elementAssignment))
    {
      Debug.Log((object) "Map replaced".Colour(Color.cyan));
      return true;
    }
    Debug.Log((object) $"Unable to assign binding for {binding.Action} in Binding attempt!".Colour(Color.red));
    return false;
  }

  public static List<Binding> GetBindings(ControllerType controllerType)
  {
    bool flag = (UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer == (UnityEngine.Object) null || MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer.isLamb;
    switch (controllerType)
    {
      case ControllerType.Keyboard:
        return SettingsManager.Settings.Control.KeyboardBindings;
      case ControllerType.Mouse:
        return SettingsManager.Settings.Control.MouseBindings;
      case ControllerType.Joystick:
        return !flag ? SettingsManager.Settings.Control.GamepadBindings_P2 : SettingsManager.Settings.Control.GamepadBindings;
      default:
        return (List<Binding>) null;
    }
  }

  public static bool AddBinding(Binding binding)
  {
    List<Binding> bindings = ControlSettingsUtilities.GetBindings(binding.ControllerType);
    List<UnboundBinding> unboundBindings = ControlSettingsUtilities.GetUnboundBindings(binding.ControllerType);
    if (bindings == null)
      return false;
    ControlSettingsUtilities.RemoveBinding(bindings, binding);
    ControlSettingsUtilities.RemoveUnboundBinding(unboundBindings, binding);
    bindings.Add(binding);
    Action<Binding> onRebind = ControlSettingsUtilities.OnRebind;
    if (onRebind != null)
      onRebind(binding);
    return true;
  }

  public static bool RemoveBinding(List<Binding> bindings, Binding binding)
  {
    bool flag = false;
    for (int index = bindings.Count - 1; index >= 0; --index)
    {
      Binding binding1 = bindings[index];
      if (binding1.Category == binding.Category && binding1.Action == binding.Action && binding1.AxisContribution == binding.AxisContribution)
      {
        Debug.Log((object) $"Cleared binding for action {binding1.Action}".Colour(Color.red));
        bindings.Remove(binding1);
        flag = true;
      }
    }
    return flag;
  }

  public static bool ResetBinding(ControllerMap controllerMap, int action, Pole axisContribution)
  {
    ActionElementMap actionElementMap = controllerMap.GetActionElementMap(action, axisContribution);
    if (actionElementMap != null)
    {
      if (controllerMap.DeleteElementMap(actionElementMap.id))
      {
        ControlSettingsUtilities.RemoveBinding(ControlSettingsUtilities.GetBindings(controllerMap.controllerType), actionElementMap.ToBinding());
        foreach (Binding defaultBinding in ControlSettingsUtilities.GetDefaultBindings(controllerMap.categoryId, controllerMap.controllerType))
        {
          if (defaultBinding.Action == action && defaultBinding.AxisContribution == axisContribution)
            controllerMap.CreateElementMap(defaultBinding.ToElementAssigment());
        }
        Debug.Log((object) "Map Reset!".Colour(Color.yellow));
        Action<int> onBindingReset = ControlSettingsUtilities.OnBindingReset;
        if (onBindingReset != null)
          onBindingReset(action);
        return true;
      }
    }
    else
    {
      ControlSettingsUtilities.RemoveUnboundBinding(ControlSettingsUtilities.GetUnboundBindings(controllerMap.controllerType), action, controllerMap.categoryId, axisContribution);
      foreach (Binding defaultBinding in ControlSettingsUtilities.GetDefaultBindings(controllerMap.categoryId, controllerMap.controllerType))
      {
        if (defaultBinding.Action == action && defaultBinding.AxisContribution == axisContribution && controllerMap.CreateElementMap(defaultBinding.ToElementAssigment()))
        {
          Action<int> onBindingReset = ControlSettingsUtilities.OnBindingReset;
          if (onBindingReset != null)
            onBindingReset(action);
          return true;
        }
      }
    }
    Debug.Log((object) "Map not Reset!".Colour(Color.red));
    return false;
  }

  public static bool RemoveUnboundBinding(
    List<UnboundBinding> unboundBindings,
    UnboundBinding unboundBinding)
  {
    return ControlSettingsUtilities.RemoveUnboundBinding(unboundBindings, unboundBinding.Action, unboundBinding.Category, unboundBinding.AxisContribution);
  }

  public static bool RemoveUnboundBinding(List<UnboundBinding> unboundBindings, Binding binding)
  {
    return ControlSettingsUtilities.RemoveUnboundBinding(unboundBindings, binding.Action, binding.Category, binding.AxisContribution);
  }

  public static bool RemoveUnboundBinding(
    List<UnboundBinding> unboundBindings,
    int action,
    int category,
    Pole axisContribution)
  {
    bool flag = false;
    for (int index = unboundBindings.Count - 1; index >= 0; --index)
    {
      UnboundBinding unboundBinding = unboundBindings[index];
      if (unboundBinding.Category == category && unboundBinding.Action == action && unboundBinding.AxisContribution == axisContribution)
      {
        Debug.Log((object) $"Cleared unbound binding for action {unboundBinding.Action}".Colour(Color.red));
        unboundBindings.Remove(unboundBinding);
        flag = true;
      }
    }
    return flag;
  }

  public static void DeleteUnboundBindings(
    ControllerMap controllerMap,
    List<UnboundBinding> unboundBindings)
  {
    foreach (UnboundBinding unboundBinding in unboundBindings)
    {
      if (unboundBinding.Category == controllerMap.categoryId)
      {
        ActionElementMap actionElementMap = controllerMap.GetActionElementMap(unboundBinding.Action, unboundBinding.AxisContribution);
        if (actionElementMap != null)
          ControlSettingsUtilities.DeleteElementMap(controllerMap, actionElementMap);
      }
    }
  }

  public static bool DeleteElementMap(
    ControllerMap controllerMap,
    ActionElementMap actionElementMap)
  {
    if (!controllerMap.DeleteElementMap(actionElementMap.id))
      return false;
    Action<int> onBindingReset = ControlSettingsUtilities.OnBindingReset;
    if (onBindingReset != null)
      onBindingReset(actionElementMap.actionId);
    return true;
  }

  public static List<Binding> GetDefaultBindings(int category, ControllerType controllerType)
  {
    if (category == 0)
      return InputManager.Gameplay.GetDefaultBindingsForControllerType(controllerType);
    return category == 2 ? InputManager.PhotoMode.GetDefaultBindingsForControllerType(controllerType) : InputManager.UI.GetDefaultBindingsForControllerType(controllerType);
  }

  public static List<UnboundBinding> GetUnboundBindings(ControllerType controllerType)
  {
    bool flag = (UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer == (UnityEngine.Object) null || MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer.isLamb;
    switch (controllerType)
    {
      case ControllerType.Keyboard:
        return SettingsManager.Settings.Control.KeyboardBindingsUnbound;
      case ControllerType.Joystick:
        return !flag ? SettingsManager.Settings.Control.GamepadBindingsUnbound_P2 : SettingsManager.Settings.Control.GamepadBindingsUnbound;
      default:
        return SettingsManager.Settings.Control.MouseBindingsUnbound;
    }
  }

  public static void UpdateGamepadPrompts()
  {
    System.Action gamepadPromptsChanged = ControlSettingsUtilities.OnGamepadPromptsChanged;
    if (gamepadPromptsChanged == null)
      return;
    gamepadPromptsChanged();
  }

  public static void SetGamepadLayout()
  {
    InputManager.UI.ApplyBindings();
    InputManager.Gameplay.ApplyBindings();
    InputManager.PhotoMode.ApplyBindings();
    System.Action gamepadLayoutChanged = ControlSettingsUtilities.OnGamepadLayoutChanged;
    if (gamepadLayoutChanged == null)
      return;
    gamepadLayoutChanged();
  }
}
