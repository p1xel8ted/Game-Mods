// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIControlBindingOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI.Assets;
using Rewired;
using src.Extensions;
using src.UINavigator;
using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using TMPro;
using Unify;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class UIControlBindingOverlayController : UIMenuBase
{
  [SerializeField]
  public TextMeshProUGUI _header;
  [SerializeField]
  public Localize _promptLocalize;
  [SerializeField]
  public TextMeshProUGUI _prompt;
  public int _category;
  public int _action;
  public Pole _axisContribution;
  public ControllerType _controllerType;
  public InputMapper _inputMapper;
  public string _term;
  public Controller _controller;
  public ControllerMap _controllerMap;
  public ControllerMap _uiControllerMap;
  public ControllerMap _photoModeControllerMap;
  public ControllerMap _cancelMap;
  public ControllerMap _targetControllerMap;
  public ActionElementMap _actionElementMap;
  public BindingConflictResolver _bindingConflictResolver;
  public BindingConflictResolver.BindingEntry _bindingEntry;

  public void Show(
    BindingConflictResolver bindingConflictResolver,
    string term,
    int category,
    int action,
    Pole axisContribution,
    ControllerType controllerType,
    bool instant = false)
  {
    this._bindingConflictResolver = bindingConflictResolver;
    this._bindingEntry = this._bindingConflictResolver.GetEntry(category, action);
    this._term = term;
    this._category = category;
    this._action = action;
    this._axisContribution = axisContribution;
    this._controllerType = controllerType;
    this.Show(instant);
  }

  public override IEnumerator DoShowAnimation()
  {
    if (this._controllerType == ControllerType.Keyboard)
      InputManager.General.RemoveController(ControllerType.Mouse);
    yield return (object) null;
    this._controller = InputManager.General.GetController(this._controllerType, MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer);
    this._controllerMap = InputManager.Gameplay.GetControllerMap(this._controller, MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer);
    this._uiControllerMap = InputManager.UI.GetControllerMap(this._controller, MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer);
    this._photoModeControllerMap = InputManager.PhotoMode.GetControllerMap(this._controller, MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer);
    this._cancelMap = this._controllerType != ControllerType.Mouse ? InputManager.UI.GetControllerMap(this._controller, MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer) : InputManager.UI.GetControllerMap(InputManager.General.GetController(ControllerType.Keyboard), MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer);
    if (this._category == 0)
      this._targetControllerMap = this._controllerMap;
    else if (this._category == 1)
      this._targetControllerMap = this._uiControllerMap;
    else if (this._category == 2)
      this._targetControllerMap = this._photoModeControllerMap;
    foreach (ActionElementMap actionElementMap in this._targetControllerMap.GetElementMapsWithAction(this._action))
    {
      if (actionElementMap.axisContribution == this._axisContribution)
      {
        this._actionElementMap = actionElementMap;
        break;
      }
    }
    ActionElementMap actionElementMap1 = this._cancelMap.GetElementMapsWithAction(61)[0];
    this._header.text = LocalizationManager.GetTranslation(this._term);
    if (this._controllerType == ControllerType.Joystick)
    {
      this._promptLocalize.Term = UnifyManager.platform == UnifyManager.Platform.PS4 || UnifyManager.platform == UnifyManager.Platform.PS5 ? "UI/Settings/Controls/Bindings/BindPromptAlt_PLAYSTATION" : "UI/Settings/Controls/Bindings/BindPromptAlt";
    }
    else
    {
      this._promptLocalize.Term = "UI/Settings/Controls/Bindings/BindPrompt";
      this._prompt.text = ScriptLocalization.UI_Settings_Controls_Bindings.BindPrompt;
      this._prompt.text = string.Format(this._prompt.text, (object) LocalizeIntegration.Arabic_ReverseNonRTL(actionElementMap1.keyboardKeyCode.ToString()));
    }
    yield return (object) this.\u003C\u003En__0();
  }

  public override void OnShowCompleted()
  {
    InputMapper.Context mappingContext = new InputMapper.Context()
    {
      controllerMap = this._targetControllerMap,
      actionId = this._action,
      actionElementMapToReplace = this._actionElementMap,
      actionRange = this._axisContribution.ToAxisRange()
    };
    this._inputMapper = InputMapper.Default;
    InputMapper.Options options = this._inputMapper.options;
    options.isElementAllowedCallback = (Predicate<ControllerPollingInfo>) Delegate.Combine((Delegate) options.isElementAllowedCallback, (Delegate) new Predicate<ControllerPollingInfo>(this.IsElementAllowed));
    this._inputMapper.options.allowKeyboardModifierKeyAsPrimary = true;
    this._inputMapper.options.holdDurationToMapKeyboardModifierKeyAsPrimary = 0.0f;
    this._inputMapper.Start(mappingContext);
    this._inputMapper.InputMappedEvent += (Action<InputMapper.InputMappedEventData>) new Action<InputMapper.InputMappedEventData>(this.InputMappedEvent);
    this._inputMapper.ConflictFoundEvent += (Action<InputMapper.ConflictFoundEventData>) new Action<InputMapper.ConflictFoundEventData>(this.ConflictFoundEvent);
  }

  public bool IsElementAllowed(ControllerPollingInfo pollingInfo)
  {
    // ISSUE: variable of a compiler-generated type
    UIControlBindingOverlayController.\u003C\u003Ec__DisplayClass21_0 cDisplayClass210;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass210.\u003C\u003E4__this = this;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass210.pollingInfo = pollingInfo;
    UnityEngine.Debug.Log((object) "Is Element Allowed?".Colour(Color.yellow));
    foreach (ActionElementMap actionElementMap in this._cancelMap.GetElementMapsWithAction(61))
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (cDisplayClass210.pollingInfo.controllerType == ControllerType.Keyboard && actionElementMap.keyCode == cDisplayClass210.pollingInfo.keyboardKey)
      {
        UnityEngine.Debug.Log((object) "Key Matched Disallowed Action - Element not allowed!".Colour(Color.red));
        return false;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (cDisplayClass210.pollingInfo.controllerType == ControllerType.Joystick && actionElementMap.elementIdentifierId == cDisplayClass210.pollingInfo.elementIdentifierId)
      {
        UnityEngine.Debug.Log((object) "Key Matched Disallowed Action - Element not allowed!".Colour(Color.red));
        return false;
      }
    }
    if (this._bindingEntry != null && this._bindingEntry.ConflictingBindings != null)
    {
      foreach (int conflictingBinding in this._bindingEntry.ConflictingBindings)
      {
        // ISSUE: variable of a compiler-generated type
        UIControlBindingOverlayController.\u003C\u003Ec__DisplayClass21_1 cDisplayClass211;
        // ISSUE: reference to a compiler-generated field
        cDisplayClass211.aem = this._cancelMap.GetActionElementMap(conflictingBinding, this._axisContribution);
        // ISSUE: reference to a compiler-generated field
        if (cDisplayClass211.aem != null && (this.\u003CIsElementAllowed\u003Eg__CheckLockedConflict\u007C21_0(1, conflictingBinding, ref cDisplayClass210, ref cDisplayClass211) || this.\u003CIsElementAllowed\u003Eg__CheckLockedConflict\u007C21_0(0, conflictingBinding, ref cDisplayClass210, ref cDisplayClass211) || this.\u003CIsElementAllowed\u003Eg__CheckLockedConflict\u007C21_0(2, conflictingBinding, ref cDisplayClass210, ref cDisplayClass211)))
          return false;
      }
    }
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    if (cDisplayClass210.pollingInfo.elementType != ControllerElementType.Axis || cDisplayClass210.pollingInfo.elementIdentifierId == 4 || cDisplayClass210.pollingInfo.elementIdentifierId == 5)
      return true;
    UnityEngine.Debug.Log((object) "Element Disallowed Action - Axis not allowed!".Colour(Color.red));
    return false;
  }

  public void InputMappedEvent(InputMapper.InputMappedEventData data)
  {
    if (this._controllerType == ControllerType.Keyboard)
      UnityEngine.Debug.Log((object) $"Keyboard Input Mapped Event - Map {data.actionElementMap.actionId} to {data.actionElementMap.keyCode}".Colour(Color.yellow));
    else if (this._controllerType == ControllerType.Mouse)
      UnityEngine.Debug.Log((object) $"Mouse Input Mapped Event - Map {data.actionElementMap.actionId} to {(Enum) (MouseInputElement) data.actionElementMap.elementIdentifierId}".Colour(Color.yellow));
    else if (this._controllerType == ControllerType.Joystick)
      UnityEngine.Debug.Log((object) $"Joystick Input Mapped Event - Map {data.actionElementMap.actionId} to {data.actionElementMap.elementIdentifierId}".Colour(Color.yellow));
    this.Hide();
    ControlSettingsUtilities.AddBinding(data.actionElementMap.ToBinding());
  }

  public void ConflictFoundEvent(InputMapper.ConflictFoundEventData data)
  {
    UnityEngine.Debug.Log((object) "Conflict Found Event".Colour(Color.red));
    ((Action<InputMapper.ConflictResponse>) data.responseCallback)(InputMapper.ConflictResponse.Add);
  }

  public void Update()
  {
    if (!InputManager.UI.GetCancelBindingButtonDown())
      return;
    this.CancelBinding();
  }

  public void CancelBinding()
  {
    if (this._inputMapper != null && this._inputMapper.status == InputMapper.Status.Listening)
      this._inputMapper.Stop();
    if (!this._canvasGroup.interactable)
      return;
    this.Hide();
  }

  public override IEnumerator DoHide()
  {
    yield return (object) null;
    yield return (object) this.\u003C\u003En__1();
  }

  public override void OnHideCompleted()
  {
    if (this._controllerType == ControllerType.Keyboard)
      InputManager.General.AddController(ControllerType.Mouse);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  [CompilerGenerated]
  [DebuggerHidden]
  public IEnumerator \u003C\u003En__0() => base.DoShowAnimation();

  [CompilerGenerated]
  public bool \u003CIsElementAllowed\u003Eg__CheckLockedConflict\u007C21_0(
    int category,
    int conflictAction,
    [In] ref UIControlBindingOverlayController.\u003C\u003Ec__DisplayClass21_0 obj2,
    [In] ref UIControlBindingOverlayController.\u003C\u003Ec__DisplayClass21_1 obj3)
  {
    BindingConflictResolver.BindingEntry entry = this._bindingConflictResolver.GetEntry(category, conflictAction);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    return entry != null && obj3.aem.elementIdentifierId == obj2.pollingInfo.elementIdentifierId && (entry.LockedOnGamepad && this._controller.type == ControllerType.Joystick || entry.LockedOnKeyboard && this._controller.type == ControllerType.Keyboard || entry.LockedOnMouse && this._controller.type == ControllerType.Mouse);
  }

  [CompilerGenerated]
  [DebuggerHidden]
  public IEnumerator \u003C\u003En__1() => base.DoHide();
}
