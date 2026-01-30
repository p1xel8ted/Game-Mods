// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.LanguageData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Rewired.UI.ControlMapper;

[Serializable]
public class LanguageData : LanguageDataBase
{
  [SerializeField]
  public string _yes = "Yes";
  [SerializeField]
  public string _no = "No";
  [SerializeField]
  public string _add = "Add";
  [SerializeField]
  public string _replace = "Replace";
  [SerializeField]
  public string _remove = "Remove";
  [SerializeField]
  public string _swap = "Swap";
  [SerializeField]
  public string _cancel = "Cancel";
  [SerializeField]
  public string _none = "None";
  [SerializeField]
  public string _okay = "Okay";
  [SerializeField]
  public string _done = "Done";
  [SerializeField]
  public string _default = "Default";
  [SerializeField]
  public string _assignControllerWindowTitle = "Choose Controller";
  [SerializeField]
  public string _assignControllerWindowMessage = "Press any button or move an axis on the controller you would like to use.";
  [SerializeField]
  public string _controllerAssignmentConflictWindowTitle = "Controller Assignment";
  [SerializeField]
  [Tooltip("{0} = Joystick Name\n{1} = Other Player Name\n{2} = This Player Name")]
  public string _controllerAssignmentConflictWindowMessage = "{0} is already assigned to {1}. Do you want to assign this controller to {2} instead?";
  [SerializeField]
  public string _elementAssignmentPrePollingWindowMessage = "First center or zero all sticks and axes and press any button or wait for the timer to finish.";
  [SerializeField]
  [Tooltip("{0} = Action Name")]
  public string _joystickElementAssignmentPollingWindowMessage = "Now press a button or move an axis to assign it to {0}.";
  [SerializeField]
  [Tooltip("This text is only displayed when split-axis fields have been disabled and the user clicks on the full-axis field. Button/key/D-pad input cannot be assigned to a full-axis field.\n{0} = Action Name")]
  public string _joystickElementAssignmentPollingWindowMessage_fullAxisFieldOnly = "Now move an axis to assign it to {0}.";
  [SerializeField]
  [Tooltip("{0} = Action Name")]
  public string _keyboardElementAssignmentPollingWindowMessage = "Press a key to assign it to {0}. Modifier keys may also be used. To assign a modifier key alone, hold it down for 1 second.";
  [SerializeField]
  [Tooltip("{0} = Action Name")]
  public string _mouseElementAssignmentPollingWindowMessage = "Press a mouse button or move an axis to assign it to {0}.";
  [SerializeField]
  [Tooltip("This text is only displayed when split-axis fields have been disabled and the user clicks on the full-axis field. Button/key/D-pad input cannot be assigned to a full-axis field.\n{0} = Action Name")]
  public string _mouseElementAssignmentPollingWindowMessage_fullAxisFieldOnly = "Move an axis to assign it to {0}.";
  [SerializeField]
  public string _elementAssignmentConflictWindowMessage = "Assignment Conflict";
  [SerializeField]
  [Tooltip("{0} = Element Name")]
  public string _elementAlreadyInUseBlocked = "{0} is already in use cannot be replaced.";
  [SerializeField]
  [Tooltip("{0} = Element Name")]
  public string _elementAlreadyInUseCanReplace = "{0} is already in use. Do you want to replace it?";
  [SerializeField]
  [Tooltip("{0} = Element Name")]
  public string _elementAlreadyInUseCanReplace_conflictAllowed = "{0} is already in use. Do you want to replace it? You may also choose to add the assignment anyway.";
  [SerializeField]
  public string _mouseAssignmentConflictWindowTitle = "Mouse Assignment";
  [SerializeField]
  [Tooltip("{0} = Other Player Name\n{1} = This Player Name")]
  public string _mouseAssignmentConflictWindowMessage = "The mouse is already assigned to {0}. Do you want to assign the mouse to {1} instead?";
  [SerializeField]
  public string _calibrateControllerWindowTitle = "Calibrate Controller";
  [SerializeField]
  public string _calibrateAxisStep1WindowTitle = "Calibrate Zero";
  [SerializeField]
  [Tooltip("{0} = Axis Name")]
  public string _calibrateAxisStep1WindowMessage = "Center or zero {0} and press any button or wait for the timer to finish.";
  [SerializeField]
  public string _calibrateAxisStep2WindowTitle = "Calibrate Range";
  [SerializeField]
  [Tooltip("{0} = Axis Name")]
  public string _calibrateAxisStep2WindowMessage = "Move {0} through its entire range then press any button or wait for the timer to finish.";
  [SerializeField]
  public string _inputBehaviorSettingsWindowTitle = "Sensitivity Settings";
  [SerializeField]
  public string _restoreDefaultsWindowTitle = "Restore Defaults";
  [SerializeField]
  [Tooltip("Message for a single player game.")]
  public string _restoreDefaultsWindowMessage_onePlayer = "This will restore the default input configuration. Are you sure you want to do this?";
  [SerializeField]
  [Tooltip("Message for a multi-player game.")]
  public string _restoreDefaultsWindowMessage_multiPlayer = "This will restore the default input configuration for all players. Are you sure you want to do this?";
  [SerializeField]
  public string _actionColumnLabel = "Actions";
  [SerializeField]
  public string _keyboardColumnLabel = "Keyboard";
  [SerializeField]
  public string _mouseColumnLabel = "Mouse";
  [SerializeField]
  public string _controllerColumnLabel = "Controller";
  [SerializeField]
  public string _removeControllerButtonLabel = "Remove";
  [SerializeField]
  public string _calibrateControllerButtonLabel = "Calibrate";
  [SerializeField]
  public string _assignControllerButtonLabel = "Assign Controller";
  [SerializeField]
  public string _inputBehaviorSettingsButtonLabel = "Sensitivity";
  [SerializeField]
  public string _doneButtonLabel = "Done";
  [SerializeField]
  public string _restoreDefaultsButtonLabel = "Restore Defaults";
  [SerializeField]
  public string _playersGroupLabel = "Players:";
  [SerializeField]
  public string _controllerSettingsGroupLabel = "Controller:";
  [SerializeField]
  public string _assignedControllersGroupLabel = "Assigned Controllers:";
  [SerializeField]
  public string _settingsGroupLabel = "Settings:";
  [SerializeField]
  public string _mapCategoriesGroupLabel = "Categories:";
  [SerializeField]
  public string _calibrateWindow_deadZoneSliderLabel = "Dead Zone:";
  [SerializeField]
  public string _calibrateWindow_zeroSliderLabel = "Zero:";
  [SerializeField]
  public string _calibrateWindow_sensitivitySliderLabel = "Sensitivity:";
  [SerializeField]
  public string _calibrateWindow_invertToggleLabel = "Invert";
  [SerializeField]
  public string _calibrateWindow_calibrateButtonLabel = "Calibrate";
  [SerializeField]
  public LanguageData.ModifierKeys _modifierKeys;
  [SerializeField]
  public LanguageData.CustomEntry[] _customEntries;
  public bool _initialized;
  public Dictionary<string, string> customDict;

  public override void Initialize()
  {
    if (this._initialized)
      return;
    this.customDict = LanguageData.CustomEntry.ToDictionary(this._customEntries);
    this._initialized = true;
  }

  public override string GetCustomEntry(string key)
  {
    string str;
    return string.IsNullOrEmpty(key) || !this.customDict.TryGetValue(key, out str) ? string.Empty : str;
  }

  public override bool ContainsCustomEntryKey(string key)
  {
    return !string.IsNullOrEmpty(key) && this.customDict.ContainsKey(key);
  }

  public override string yes => this._yes;

  public override string no => this._no;

  public override string add => this._add;

  public override string replace => this._replace;

  public override string remove => this._remove;

  public override string swap => this._swap;

  public override string cancel => this._cancel;

  public override string none => this._none;

  public override string okay => this._okay;

  public override string done => this._done;

  public override string default_ => this._default;

  public override string assignControllerWindowTitle => this._assignControllerWindowTitle;

  public override string assignControllerWindowMessage => this._assignControllerWindowMessage;

  public override string controllerAssignmentConflictWindowTitle
  {
    get => this._controllerAssignmentConflictWindowTitle;
  }

  public override string elementAssignmentPrePollingWindowMessage
  {
    get => this._elementAssignmentPrePollingWindowMessage;
  }

  public override string elementAssignmentConflictWindowMessage
  {
    get => this._elementAssignmentConflictWindowMessage;
  }

  public override string mouseAssignmentConflictWindowTitle
  {
    get => this._mouseAssignmentConflictWindowTitle;
  }

  public override string calibrateControllerWindowTitle => this._calibrateControllerWindowTitle;

  public override string calibrateAxisStep1WindowTitle => this._calibrateAxisStep1WindowTitle;

  public override string calibrateAxisStep2WindowTitle => this._calibrateAxisStep2WindowTitle;

  public override string inputBehaviorSettingsWindowTitle => this._inputBehaviorSettingsWindowTitle;

  public override string restoreDefaultsWindowTitle => this._restoreDefaultsWindowTitle;

  public override string actionColumnLabel => this._actionColumnLabel;

  public override string keyboardColumnLabel => this._keyboardColumnLabel;

  public override string mouseColumnLabel => this._mouseColumnLabel;

  public override string controllerColumnLabel => this._controllerColumnLabel;

  public override string removeControllerButtonLabel => this._removeControllerButtonLabel;

  public override string calibrateControllerButtonLabel => this._calibrateControllerButtonLabel;

  public override string assignControllerButtonLabel => this._assignControllerButtonLabel;

  public override string inputBehaviorSettingsButtonLabel => this._inputBehaviorSettingsButtonLabel;

  public override string doneButtonLabel => this._doneButtonLabel;

  public override string restoreDefaultsButtonLabel => this._restoreDefaultsButtonLabel;

  public override string controllerSettingsGroupLabel => this._controllerSettingsGroupLabel;

  public override string playersGroupLabel => this._playersGroupLabel;

  public override string assignedControllersGroupLabel => this._assignedControllersGroupLabel;

  public override string settingsGroupLabel => this._settingsGroupLabel;

  public override string mapCategoriesGroupLabel => this._mapCategoriesGroupLabel;

  public override string restoreDefaultsWindowMessage
  {
    get
    {
      return ReInput.players.playerCount > 1 ? this._restoreDefaultsWindowMessage_multiPlayer : this._restoreDefaultsWindowMessage_onePlayer;
    }
  }

  public override string calibrateWindow_deadZoneSliderLabel
  {
    get => this._calibrateWindow_deadZoneSliderLabel;
  }

  public override string calibrateWindow_zeroSliderLabel => this._calibrateWindow_zeroSliderLabel;

  public override string calibrateWindow_sensitivitySliderLabel
  {
    get => this._calibrateWindow_sensitivitySliderLabel;
  }

  public override string calibrateWindow_invertToggleLabel
  {
    get => this._calibrateWindow_invertToggleLabel;
  }

  public override string calibrateWindow_calibrateButtonLabel
  {
    get => this._calibrateWindow_calibrateButtonLabel;
  }

  public override string GetControllerAssignmentConflictWindowMessage(
    string joystickName,
    string otherPlayerName,
    string currentPlayerName)
  {
    return string.Format(this._controllerAssignmentConflictWindowMessage, (object) joystickName, (object) otherPlayerName, (object) currentPlayerName);
  }

  public override string GetJoystickElementAssignmentPollingWindowMessage(string actionName)
  {
    return string.Format(this._joystickElementAssignmentPollingWindowMessage, (object) actionName);
  }

  public override string GetJoystickElementAssignmentPollingWindowMessage_FullAxisFieldOnly(
    string actionName)
  {
    return string.Format(this._joystickElementAssignmentPollingWindowMessage_fullAxisFieldOnly, (object) actionName);
  }

  public override string GetKeyboardElementAssignmentPollingWindowMessage(string actionName)
  {
    return string.Format(this._keyboardElementAssignmentPollingWindowMessage, (object) actionName);
  }

  public override string GetMouseElementAssignmentPollingWindowMessage(string actionName)
  {
    return string.Format(this._mouseElementAssignmentPollingWindowMessage, (object) actionName);
  }

  public override string GetMouseElementAssignmentPollingWindowMessage_FullAxisFieldOnly(
    string actionName)
  {
    return string.Format(this._mouseElementAssignmentPollingWindowMessage_fullAxisFieldOnly, (object) actionName);
  }

  public override string GetElementAlreadyInUseBlocked(string elementName)
  {
    return string.Format(this._elementAlreadyInUseBlocked, (object) elementName);
  }

  public override string GetElementAlreadyInUseCanReplace(string elementName, bool allowConflicts)
  {
    return !allowConflicts ? string.Format(this._elementAlreadyInUseCanReplace, (object) elementName) : string.Format(this._elementAlreadyInUseCanReplace_conflictAllowed, (object) elementName);
  }

  public override string GetMouseAssignmentConflictWindowMessage(
    string otherPlayerName,
    string thisPlayerName)
  {
    return string.Format(this._mouseAssignmentConflictWindowMessage, (object) otherPlayerName, (object) thisPlayerName);
  }

  public override string GetCalibrateAxisStep1WindowMessage(string axisName)
  {
    return string.Format(this._calibrateAxisStep1WindowMessage, (object) axisName);
  }

  public override string GetCalibrateAxisStep2WindowMessage(string axisName)
  {
    return string.Format(this._calibrateAxisStep2WindowMessage, (object) axisName);
  }

  public override string GetPlayerName(int playerId)
  {
    return (ReInput.players.GetPlayer(playerId) ?? throw new ArgumentException("Invalid player id: " + playerId.ToString())).descriptiveName;
  }

  public override string GetControllerName(Controller controller)
  {
    return controller != null ? controller.name : throw new ArgumentNullException(nameof (controller));
  }

  public override string GetElementIdentifierName(ActionElementMap actionElementMap)
  {
    if (actionElementMap == null)
      throw new ArgumentNullException(nameof (actionElementMap));
    if (this.isLocalizationSystemEnabled)
      return actionElementMap.elementIdentifierName;
    return actionElementMap.controllerMap.controllerType == ControllerType.Keyboard ? this.GetElementIdentifierName(actionElementMap.keyCode, actionElementMap.modifierKeyFlags) : this.GetElementIdentifierName(actionElementMap.controllerMap.controller, actionElementMap.elementIdentifierId, actionElementMap.axisRange);
  }

  public override string GetElementIdentifierName(
    Controller controller,
    int elementIdentifierId,
    AxisRange axisRange)
  {
    ControllerElementIdentifier elementIdentifier = controller != null ? controller.GetElementIdentifierById(elementIdentifierId) : throw new ArgumentNullException(nameof (controller));
    if (elementIdentifier == null)
      throw new ArgumentException("Invalid element identifier id: " + elementIdentifierId.ToString());
    Controller.Element elementById = controller.GetElementById(elementIdentifierId);
    return elementById == null ? string.Empty : elementIdentifier.GetDisplayName(elementById.type, axisRange);
  }

  public override string GetElementIdentifierName(
    KeyCode keyCode,
    ModifierKeyFlags modifierKeyFlags)
  {
    if (this.isLocalizationSystemEnabled)
      return Keyboard.GetKeyName(keyCode, modifierKeyFlags);
    return modifierKeyFlags != ModifierKeyFlags.None ? $"{this.ModifierKeyFlagsToString(modifierKeyFlags)}{this._modifierKeys.separator}{Keyboard.GetKeyName(keyCode)}" : Keyboard.GetKeyName(keyCode);
  }

  public override string GetActionName(int actionId)
  {
    return (ReInput.mapping.GetAction(actionId) ?? throw new ArgumentException("Invalid action id: " + actionId.ToString())).descriptiveName;
  }

  public override string GetActionName(int actionId, AxisRange axisRange)
  {
    InputAction action = ReInput.mapping.GetAction(actionId);
    if (action == null)
      throw new ArgumentException("Invalid action id: " + actionId.ToString());
    if (this.isLocalizationSystemEnabled)
      return action.GetDisplayName(axisRange);
    switch (axisRange)
    {
      case AxisRange.Full:
        return action.descriptiveName;
      case AxisRange.Positive:
        return string.IsNullOrEmpty(action.positiveDescriptiveName) ? action.descriptiveName + " +" : action.positiveDescriptiveName;
      case AxisRange.Negative:
        return string.IsNullOrEmpty(action.negativeDescriptiveName) ? action.descriptiveName + " -" : action.negativeDescriptiveName;
      default:
        throw new NotImplementedException();
    }
  }

  public override string GetMapCategoryName(int id)
  {
    return (ReInput.mapping.GetMapCategory(id) ?? throw new ArgumentException("Invalid map category id: " + id.ToString())).descriptiveName;
  }

  public override string GetActionCategoryName(int id)
  {
    return (ReInput.mapping.GetActionCategory(id) ?? throw new ArgumentException("Invalid action category id: " + id.ToString())).descriptiveName;
  }

  public override string GetLayoutName(ControllerType controllerType, int id)
  {
    return (ReInput.mapping.GetLayout(controllerType, id) ?? throw new ArgumentException($"Invalid {controllerType.ToString()} layout id: {id.ToString()}")).descriptiveName;
  }

  public override string ModifierKeyFlagsToString(ModifierKeyFlags flags)
  {
    if (this.isLocalizationSystemEnabled)
      return Keyboard.ModifierKeyFlagsToString(flags);
    int num1 = 0;
    string empty = string.Empty;
    if (Keyboard.ModifierKeyFlagsContain(flags, ModifierKey.Control))
    {
      empty += this._modifierKeys.control;
      ++num1;
    }
    if (Keyboard.ModifierKeyFlagsContain(flags, ModifierKey.Command))
    {
      if (num1 > 0 && !string.IsNullOrEmpty(this._modifierKeys.separator))
        empty += this._modifierKeys.separator;
      empty += this._modifierKeys.command;
      ++num1;
    }
    if (Keyboard.ModifierKeyFlagsContain(flags, ModifierKey.Alt))
    {
      if (num1 > 0 && !string.IsNullOrEmpty(this._modifierKeys.separator))
        empty += this._modifierKeys.separator;
      empty += this._modifierKeys.alt;
      ++num1;
    }
    if (num1 >= 3 || !Keyboard.ModifierKeyFlagsContain(flags, ModifierKey.Shift))
      return empty;
    if (num1 > 0 && !string.IsNullOrEmpty(this._modifierKeys.separator))
      empty += this._modifierKeys.separator;
    empty += this._modifierKeys.shift;
    int num2 = num1 + 1;
    return empty;
  }

  [Serializable]
  public class CustomEntry
  {
    public string key;
    public string value;

    public CustomEntry()
    {
    }

    public CustomEntry(string key, string value)
    {
      this.key = key;
      this.value = value;
    }

    public static Dictionary<string, string> ToDictionary(LanguageData.CustomEntry[] array)
    {
      if (array == null)
        return new Dictionary<string, string>();
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      for (int index = 0; index < array.Length; ++index)
      {
        if (array[index] != null && !string.IsNullOrEmpty(array[index].key) && !string.IsNullOrEmpty(array[index].value))
        {
          if (dictionary.ContainsKey(array[index].key))
            Debug.LogError((object) $"Key \"{array[index].key}\" is already in dictionary!");
          else
            dictionary.Add(array[index].key, array[index].value);
        }
      }
      return dictionary;
    }
  }

  [Serializable]
  public class ModifierKeys
  {
    public string control = "Control";
    public string alt = "Alt";
    public string shift = "Shift";
    public string command = "Command";
    public string separator = " + ";
  }
}
