// Decompiled with JetBrains decompiler
// Type: Lamb.UI.KeybindConflictLookup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Rewired;
using src.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

[RequireComponent(typeof (KeybindItem))]
public class KeybindConflictLookup : MonoBehaviour
{
  [SerializeField]
  private KeybindItem[] _ignoreConflicts;
  private KeybindItem _keybindItem;
  private List<KeybindItem> _conflictingKeybinds = new List<KeybindItem>();

  private void OnEnable()
  {
    ControlSettingsUtilities.OnRebind += new Action<Binding>(this.OnRebind);
  }

  private void OnDisable()
  {
    ControlSettingsUtilities.OnRebind -= new Action<Binding>(this.OnRebind);
  }

  public void Configure(KeybindItem[] keybindItems)
  {
    if ((UnityEngine.Object) this._keybindItem == (UnityEngine.Object) null)
      this._keybindItem = this.GetComponent<KeybindItem>();
    foreach (KeybindItem keybindItem in keybindItems)
    {
      if ((UnityEngine.Object) keybindItem != (UnityEngine.Object) this._keybindItem && !this._ignoreConflicts.Contains<KeybindItem>(keybindItem) && keybindItem.Category == this._keybindItem.Category)
        this._conflictingKeybinds.Add(keybindItem);
    }
  }

  private void OnRebind(Binding binding)
  {
    if (this._conflictingKeybinds.Count == 0 || binding.Category != this._keybindItem.Category || binding.Action != this._keybindItem.Action || binding.AxisContribution != this._keybindItem.AxisContribution)
      return;
    Debug.Log((object) ("Check Conflict for " + this.gameObject.name).Colour(Color.yellow));
    foreach (KeybindItem conflictingKeybind in this._conflictingKeybinds)
    {
      ControllerMap controllerMapForCategory = InputManager.General.GetControllerMapForCategory(conflictingKeybind.Category, binding.ControllerType);
      ActionElementMap actionElementMap = controllerMapForCategory.GetActionElementMap(conflictingKeybind.Action, conflictingKeybind.AxisContribution);
      if (actionElementMap != null)
      {
        if (binding.ControllerType == ControllerType.Keyboard)
        {
          if (actionElementMap.keyCode == binding.KeyCode)
          {
            if (ControlSettingsUtilities.DeleteElementMap(controllerMapForCategory, actionElementMap))
              SettingsManager.Settings.Control.KeyboardBindingsUnbound.Add(actionElementMap.ToUnboundBinding());
            Debug.Log((object) $"Found a conflicting bind for Action {binding.Action} for {binding.ControllerType} - {conflictingKeybind.gameObject.name}".Colour(Color.red));
          }
        }
        else if (binding.ControllerType == ControllerType.Mouse)
        {
          if (actionElementMap.elementIdentifierId == binding.ElementIdentifierID)
          {
            if (ControlSettingsUtilities.DeleteElementMap(controllerMapForCategory, actionElementMap))
              SettingsManager.Settings.Control.MouseBindingsUnbound.Add(actionElementMap.ToUnboundBinding());
            Debug.Log((object) $"Found a conflicting bind for Action {binding.Action} for {binding.ControllerType} - {conflictingKeybind.gameObject.name}".Colour(Color.red));
          }
        }
        else if (binding.ControllerType == ControllerType.Joystick && actionElementMap.elementIdentifierId == binding.ElementIdentifierID)
        {
          if (ControlSettingsUtilities.DeleteElementMap(controllerMapForCategory, actionElementMap))
            SettingsManager.Settings.Control.GamepadBindingsUnbound.Add(actionElementMap.ToUnboundBinding());
          Debug.Log((object) $"Found a conflicting bind for Action {binding.Action} for {binding.ControllerType} - {conflictingKeybind.gameObject.name}".Colour(Color.red));
        }
      }
    }
  }
}
