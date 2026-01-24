// Decompiled with JetBrains decompiler
// Type: Lamb.UI.KeybindConflictLookup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.Assets;
using Rewired;
using src.Extensions;
using src.UINavigator;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

[RequireComponent(typeof (KeybindItem))]
public class KeybindConflictLookup : MonoBehaviour
{
  public KeybindItem _keybindItem;
  public int[] _conflictingBindings;

  public void OnEnable() => ControlSettingsUtilities.OnRebind += new Action<Binding>(this.OnRebind);

  public void OnDisable()
  {
    ControlSettingsUtilities.OnRebind -= new Action<Binding>(this.OnRebind);
  }

  public void Configure(BindingConflictResolver bindingConflictResolver)
  {
    if ((UnityEngine.Object) this._keybindItem == (UnityEngine.Object) null)
      this._keybindItem = this.GetComponent<KeybindItem>();
    BindingConflictResolver.BindingEntry entry = bindingConflictResolver.GetEntry(this._keybindItem);
    if (entry == null)
      return;
    this._conflictingBindings = new int[entry.ConflictingBindings.Count];
    for (int index = 0; index < this._conflictingBindings.Length; ++index)
      this._conflictingBindings[index] = entry.ConflictingBindings[index];
  }

  public void OnRebind(Binding binding)
  {
    // ISSUE: variable of a compiler-generated type
    KeybindConflictLookup.\u003C\u003Ec__DisplayClass5_0 cDisplayClass50;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass50.binding = binding;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass50.\u003C\u003E4__this = this;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    if (this._conflictingBindings == null || this._conflictingBindings.Length == 0 || cDisplayClass50.binding.Action == this._keybindItem.Action && cDisplayClass50.binding.AxisContribution == this._keybindItem.AxisContribution)
      return;
    Debug.Log((object) ("Check Conflict for " + this.gameObject.name).Colour(Color.yellow));
    // ISSUE: reference to a compiler-generated field
    if (!this._conflictingBindings.Contains<int>(cDisplayClass50.binding.Action) || this.\u003COnRebind\u003Eg__CheckBind\u007C5_0(0, ref cDisplayClass50) || this.\u003COnRebind\u003Eg__CheckBind\u007C5_0(1, ref cDisplayClass50))
      return;
    this.\u003COnRebind\u003Eg__CheckBind\u007C5_0(2, ref cDisplayClass50);
  }

  [CompilerGenerated]
  public bool \u003COnRebind\u003Eg__CheckBind\u007C5_0(
    int category,
    [In] ref KeybindConflictLookup.\u003C\u003Ec__DisplayClass5_0 obj1)
  {
    // ISSUE: reference to a compiler-generated field
    ControllerMap controllerMapForCategory = InputManager.General.GetControllerMapForCategory(category, obj1.binding.ControllerType, MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer);
    ActionElementMap actionElementMap = controllerMapForCategory.GetActionElementMap(this._keybindItem.Action, this._keybindItem.AxisContribution);
    if (actionElementMap == null)
      return false;
    // ISSUE: reference to a compiler-generated field
    if (obj1.binding.ControllerType == ControllerType.Keyboard)
    {
      // ISSUE: reference to a compiler-generated field
      if (actionElementMap.keyCode == obj1.binding.KeyCode)
      {
        if (ControlSettingsUtilities.DeleteElementMap(controllerMapForCategory, actionElementMap))
          SettingsManager.Settings.Control.KeyboardBindingsUnbound.Add(actionElementMap.ToUnboundBinding());
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Debug.Log((object) $"Found a conflicting bind for Action {obj1.binding.Action} for {obj1.binding.ControllerType} - {this._keybindItem.gameObject.name}".Colour(Color.red));
        return true;
      }
    }
    else
    {
      // ISSUE: reference to a compiler-generated field
      if (obj1.binding.ControllerType == ControllerType.Mouse)
      {
        // ISSUE: reference to a compiler-generated field
        if (actionElementMap.elementIdentifierId == obj1.binding.ElementIdentifierID)
        {
          if (ControlSettingsUtilities.DeleteElementMap(controllerMapForCategory, actionElementMap))
            SettingsManager.Settings.Control.MouseBindingsUnbound.Add(actionElementMap.ToUnboundBinding());
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Debug.Log((object) $"Found a conflicting bind for Action {obj1.binding.Action} for {obj1.binding.ControllerType} - {this._keybindItem.gameObject.name}".Colour(Color.red));
          return true;
        }
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (obj1.binding.ControllerType == ControllerType.Joystick && actionElementMap.elementIdentifierId == obj1.binding.ElementIdentifierID)
        {
          if (ControlSettingsUtilities.DeleteElementMap(controllerMapForCategory, actionElementMap))
          {
            if ((UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer == (UnityEngine.Object) null || MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer.isLamb)
              SettingsManager.Settings.Control.GamepadBindingsUnbound.Add(actionElementMap.ToUnboundBinding());
            else
              SettingsManager.Settings.Control.GamepadBindingsUnbound_P2.Add(actionElementMap.ToUnboundBinding());
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Debug.Log((object) $"Found a conflicting bind for Action {obj1.binding.Action} for {obj1.binding.ControllerType} - {this._keybindItem.gameObject.name}".Colour(Color.red));
          return true;
        }
      }
    }
    return false;
  }
}
