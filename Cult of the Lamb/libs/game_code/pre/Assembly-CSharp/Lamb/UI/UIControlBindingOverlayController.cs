// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIControlBindingOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Rewired;
using src.Extensions;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class UIControlBindingOverlayController : UIMenuBase
{
  [SerializeField]
  private TextMeshProUGUI _header;
  [SerializeField]
  private Localize _promptLocalize;
  [SerializeField]
  private TextMeshProUGUI _prompt;
  private int _category;
  private int _action;
  private Pole _axisContribution;
  private ControllerType _controllerType;
  private InputMapper _inputMapper;
  private string _term;
  private Controller _controller;
  private ControllerMap _controllerMap;
  private ControllerMap _uiControllerMap;
  private ControllerMap _cancelMap;
  private ControllerMap _targetControllerMap;
  private ActionElementMap _actionElementMap;

  public void Show(
    string term,
    int category,
    int action,
    Pole axisContribution,
    ControllerType controllerType,
    bool instant = false)
  {
    this._term = term;
    this._category = category;
    this._action = action;
    this._axisContribution = axisContribution;
    this._controllerType = controllerType;
    this.Show(instant);
  }

  protected override IEnumerator DoShowAnimation()
  {
    if (this._controllerType == ControllerType.Keyboard)
      InputManager.General.RemoveController(ControllerType.Mouse);
    yield return (object) null;
    this._controller = InputManager.General.GetController(this._controllerType);
    this._controllerMap = InputManager.Gameplay.GetControllerMap(this._controller);
    this._uiControllerMap = InputManager.UI.GetControllerMap(this._controller);
    this._cancelMap = this._controllerType != ControllerType.Mouse ? InputManager.UI.GetControllerMap(this._controller) : InputManager.UI.GetControllerMap(InputManager.General.GetController(ControllerType.Keyboard));
    if (this._category == 0)
      this._targetControllerMap = this._controllerMap;
    else if (this._category == 1)
      this._targetControllerMap = this._uiControllerMap;
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
      this._promptLocalize.Term = "UI/Settings/Controls/Bindings/BindPromptAlt";
    }
    else
    {
      this._promptLocalize.Term = "UI/Settings/Controls/Bindings/BindPrompt";
      this._prompt.text = string.Format(this._prompt.text, (object) actionElementMap1.keyboardKeyCode.ToString());
    }
    yield return (object) base.DoShowAnimation();
  }

  protected override void OnShowCompleted()
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

  private bool IsElementAllowed(ControllerPollingInfo pollingInfo)
  {
    Debug.Log((object) "Is Element Allowed?".Colour(Color.yellow));
    foreach (ActionElementMap actionElementMap in this._cancelMap.GetElementMapsWithAction(61))
    {
      if (pollingInfo.controllerType == ControllerType.Keyboard && actionElementMap.keyCode == pollingInfo.keyboardKey)
      {
        Debug.Log((object) "Key Matched Disallowed Action - Element not allowed!".Colour(Color.red));
        return false;
      }
      if (pollingInfo.controllerType == ControllerType.Joystick && actionElementMap.elementIdentifierId == pollingInfo.elementIdentifierId)
      {
        Debug.Log((object) "Key Matched Disallowed Action - Element not allowed!".Colour(Color.red));
        return false;
      }
    }
    if (pollingInfo.elementType != ControllerElementType.Axis || pollingInfo.elementIdentifierId == 4 || pollingInfo.elementIdentifierId == 5)
      return true;
    Debug.Log((object) "Element Disallowed Action - Axis not allowed!".Colour(Color.red));
    return false;
  }

  private void InputMappedEvent(InputMapper.InputMappedEventData data)
  {
    if (this._controllerType == ControllerType.Keyboard)
      Debug.Log((object) $"Keyboard Input Mapped Event - Map {data.actionElementMap.actionId} to {data.actionElementMap.keyCode}".Colour(Color.yellow));
    else if (this._controllerType == ControllerType.Mouse)
      Debug.Log((object) $"Mouse Input Mapped Event - Map {data.actionElementMap.actionId} to {(Enum) (MouseInputElement) data.actionElementMap.elementIdentifierId}".Colour(Color.yellow));
    else if (this._controllerType == ControllerType.Joystick)
      Debug.Log((object) $"Joystick Input Mapped Event - Map {data.actionElementMap.actionId} to {data.actionElementMap.elementIdentifierId}".Colour(Color.yellow));
    this.Hide();
    ControlSettingsUtilities.AddBinding(data.actionElementMap.ToBinding());
  }

  private void ConflictFoundEvent(InputMapper.ConflictFoundEventData data)
  {
    Debug.Log((object) "Conflict Found Event".Colour(Color.red));
    ((Action<InputMapper.ConflictResponse>) data.responseCallback)(InputMapper.ConflictResponse.Add);
  }

  private void Update()
  {
    if (!InputManager.UI.GetCancelBindingButtonDown())
      return;
    this.CancelBinding();
  }

  private void CancelBinding()
  {
    if (this._inputMapper != null && this._inputMapper.status == InputMapper.Status.Listening)
      this._inputMapper.Stop();
    if (!this._canvasGroup.interactable)
      return;
    this.Hide();
  }

  protected override IEnumerator DoHide()
  {
    yield return (object) null;
    yield return (object) base.DoHide();
  }

  protected override void OnHideCompleted()
  {
    if (this._controllerType == ControllerType.Keyboard)
      InputManager.General.AddController(ControllerType.Mouse);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }
}
