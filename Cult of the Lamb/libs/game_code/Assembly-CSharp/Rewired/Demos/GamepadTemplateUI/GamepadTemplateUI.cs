// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Rewired.Demos.GamepadTemplateUI;

public class GamepadTemplateUI : MonoBehaviour
{
  public const float stickRadius = 20f;
  public int playerId;
  [SerializeField]
  public RectTransform leftStick;
  [SerializeField]
  public RectTransform rightStick;
  [SerializeField]
  public ControllerUIElement leftStickX;
  [SerializeField]
  public ControllerUIElement leftStickY;
  [SerializeField]
  public ControllerUIElement leftStickButton;
  [SerializeField]
  public ControllerUIElement rightStickX;
  [SerializeField]
  public ControllerUIElement rightStickY;
  [SerializeField]
  public ControllerUIElement rightStickButton;
  [SerializeField]
  public ControllerUIElement actionBottomRow1;
  [SerializeField]
  public ControllerUIElement actionBottomRow2;
  [SerializeField]
  public ControllerUIElement actionBottomRow3;
  [SerializeField]
  public ControllerUIElement actionTopRow1;
  [SerializeField]
  public ControllerUIElement actionTopRow2;
  [SerializeField]
  public ControllerUIElement actionTopRow3;
  [SerializeField]
  public ControllerUIElement leftShoulder;
  [SerializeField]
  public ControllerUIElement leftTrigger;
  [SerializeField]
  public ControllerUIElement rightShoulder;
  [SerializeField]
  public ControllerUIElement rightTrigger;
  [SerializeField]
  public ControllerUIElement center1;
  [SerializeField]
  public ControllerUIElement center2;
  [SerializeField]
  public ControllerUIElement center3;
  [SerializeField]
  public ControllerUIElement dPadUp;
  [SerializeField]
  public ControllerUIElement dPadRight;
  [SerializeField]
  public ControllerUIElement dPadDown;
  [SerializeField]
  public ControllerUIElement dPadLeft;
  public Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement[] _uiElementsArray;
  public Dictionary<int, ControllerUIElement> _uiElements = new Dictionary<int, ControllerUIElement>();
  public IList<ControllerTemplateElementTarget> _tempTargetList = (IList<ControllerTemplateElementTarget>) new List<ControllerTemplateElementTarget>(2);
  public Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.Stick[] _sticks;

  public Player player => ReInput.players.GetPlayer(this.playerId);

  public void Awake()
  {
    this._uiElementsArray = new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement[23]
    {
      new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(0, this.leftStickX),
      new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(1, this.leftStickY),
      new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(17, this.leftStickButton),
      new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(2, this.rightStickX),
      new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(3, this.rightStickY),
      new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(18, this.rightStickButton),
      new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(4, this.actionBottomRow1),
      new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(5, this.actionBottomRow2),
      new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(6, this.actionBottomRow3),
      new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(7, this.actionTopRow1),
      new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(8, this.actionTopRow2),
      new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(9, this.actionTopRow3),
      new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(14, this.center1),
      new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(15, this.center2),
      new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(16 /*0x10*/, this.center3),
      new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(19, this.dPadUp),
      new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(20, this.dPadRight),
      new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(21, this.dPadDown),
      new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(22, this.dPadLeft),
      new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(10, this.leftShoulder),
      new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(11, this.leftTrigger),
      new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(12, this.rightShoulder),
      new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(13, this.rightTrigger)
    };
    for (int index = 0; index < this._uiElementsArray.Length; ++index)
      this._uiElements.Add(this._uiElementsArray[index].id, this._uiElementsArray[index].element);
    this._sticks = new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.Stick[2]
    {
      new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.Stick(this.leftStick, 0, 1),
      new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.Stick(this.rightStick, 2, 3)
    };
    ReInput.ControllerConnectedEvent += (Action<ControllerStatusChangedEventArgs>) new Action<ControllerStatusChangedEventArgs>(this.OnControllerConnected);
    ReInput.ControllerDisconnectedEvent += (Action<ControllerStatusChangedEventArgs>) new Action<ControllerStatusChangedEventArgs>(this.OnControllerDisconnected);
  }

  public void Start()
  {
    if (!ReInput.isReady)
      return;
    this.DrawLabels();
  }

  public void OnDestroy()
  {
    ReInput.ControllerConnectedEvent -= (Action<ControllerStatusChangedEventArgs>) new Action<ControllerStatusChangedEventArgs>(this.OnControllerConnected);
    ReInput.ControllerDisconnectedEvent -= (Action<ControllerStatusChangedEventArgs>) new Action<ControllerStatusChangedEventArgs>(this.OnControllerDisconnected);
  }

  public void Update()
  {
    if (!ReInput.isReady)
      return;
    this.DrawActiveElements();
  }

  public void DrawActiveElements()
  {
    for (int index = 0; index < this._uiElementsArray.Length; ++index)
      this._uiElementsArray[index].element.Deactivate();
    for (int index = 0; index < this._sticks.Length; ++index)
      this._sticks[index].Reset();
    IList<InputAction> actions = (IList<InputAction>) ReInput.mapping.Actions;
    for (int index = 0; index < actions.Count; ++index)
      this.ActivateElements(this.player, actions[index].id);
  }

  public void ActivateElements(Player player, int actionId)
  {
    float axis = player.GetAxis(actionId);
    if ((double) axis == 0.0)
      return;
    IList<InputActionSourceData> currentInputSources = (IList<InputActionSourceData>) player.GetCurrentInputSources(actionId);
    for (int index1 = 0; index1 < currentInputSources.Count; ++index1)
    {
      InputActionSourceData actionSourceData = currentInputSources[index1];
      IGamepadTemplate template = actionSourceData.controller.GetTemplate<IGamepadTemplate>();
      if (template != null)
      {
        template.GetElementTargets((ControllerElementTarget) actionSourceData.actionElementMap, (IList<ControllerTemplateElementTarget>) this._tempTargetList);
        for (int index2 = 0; index2 < this._tempTargetList.Count; ++index2)
        {
          ControllerTemplateElementTarget tempTarget = this._tempTargetList[index2];
          int id = tempTarget.element.id;
          ControllerUIElement uiElement = this._uiElements[id];
          if (tempTarget.elementType == ControllerTemplateElementType.Axis)
            uiElement.Activate(axis);
          else if (tempTarget.elementType == ControllerTemplateElementType.Button && (player.GetButton(actionId) || player.GetNegativeButton(actionId)))
            uiElement.Activate(1f);
          this.GetStick(id)?.SetAxisPosition(id, axis * 20f);
        }
      }
    }
  }

  public void DrawLabels()
  {
    for (int index = 0; index < this._uiElementsArray.Length; ++index)
      this._uiElementsArray[index].element.ClearLabels();
    IList<InputAction> actions = (IList<InputAction>) ReInput.mapping.Actions;
    for (int index = 0; index < actions.Count; ++index)
      this.DrawLabels(this.player, actions[index]);
  }

  public void DrawLabels(Player player, InputAction action)
  {
    Controller controllerWithTemplate = player.controllers.GetFirstControllerWithTemplate<IGamepadTemplate>();
    if (controllerWithTemplate == null)
      return;
    IGamepadTemplate template = controllerWithTemplate.GetTemplate<IGamepadTemplate>();
    ControllerMap map = player.controllers.maps.GetMap(controllerWithTemplate, "Default", "Default");
    if (map == null)
      return;
    for (int index = 0; index < this._uiElementsArray.Length; ++index)
    {
      ControllerUIElement element1 = this._uiElementsArray[index].element;
      int id = this._uiElementsArray[index].id;
      IControllerTemplateElement element2 = template.GetElement(id);
      this.DrawLabel(element1, action, map, (IControllerTemplate) template, element2);
    }
  }

  public void DrawLabel(
    ControllerUIElement uiElement,
    InputAction action,
    ControllerMap controllerMap,
    IControllerTemplate template,
    IControllerTemplateElement element)
  {
    if (element.source == null)
      return;
    if (element.source.type == ControllerTemplateElementSourceType.Axis)
    {
      IControllerTemplateAxisSource source = element.source as IControllerTemplateAxisSource;
      if (source.splitAxis)
      {
        ActionElementMap withElementTarget1 = controllerMap.GetFirstElementMapWithElementTarget(source.positiveTarget, action.id, true);
        if (withElementTarget1 != null)
          uiElement.SetLabel(withElementTarget1.actionDescriptiveName, AxisRange.Positive);
        ActionElementMap withElementTarget2 = controllerMap.GetFirstElementMapWithElementTarget(source.negativeTarget, action.id, true);
        if (withElementTarget2 == null)
          return;
        uiElement.SetLabel(withElementTarget2.actionDescriptiveName, AxisRange.Negative);
      }
      else
      {
        ActionElementMap withElementTarget3 = controllerMap.GetFirstElementMapWithElementTarget(source.fullTarget, action.id, true);
        if (withElementTarget3 != null)
        {
          uiElement.SetLabel(withElementTarget3.actionDescriptiveName, AxisRange.Full);
        }
        else
        {
          ControllerMap controllerMap1 = controllerMap;
          ControllerElementTarget controllerElementTarget = new ControllerElementTarget(source.fullTarget);
          controllerElementTarget.axisRange = AxisRange.Positive;
          ControllerElementTarget elementTarget1 = controllerElementTarget;
          int id1 = action.id;
          ActionElementMap withElementTarget4 = controllerMap1.GetFirstElementMapWithElementTarget(elementTarget1, id1, true);
          if (withElementTarget4 != null)
            uiElement.SetLabel(withElementTarget4.actionDescriptiveName, AxisRange.Positive);
          ControllerMap controllerMap2 = controllerMap;
          controllerElementTarget = new ControllerElementTarget(source.fullTarget);
          controllerElementTarget.axisRange = AxisRange.Negative;
          ControllerElementTarget elementTarget2 = controllerElementTarget;
          int id2 = action.id;
          ActionElementMap withElementTarget5 = controllerMap2.GetFirstElementMapWithElementTarget(elementTarget2, id2, true);
          if (withElementTarget5 == null)
            return;
          uiElement.SetLabel(withElementTarget5.actionDescriptiveName, AxisRange.Negative);
        }
      }
    }
    else
    {
      if (element.source.type != ControllerTemplateElementSourceType.Button)
        return;
      IControllerTemplateButtonSource source = element.source as IControllerTemplateButtonSource;
      ActionElementMap withElementTarget = controllerMap.GetFirstElementMapWithElementTarget(source.target, action.id, true);
      if (withElementTarget == null)
        return;
      uiElement.SetLabel(withElementTarget.actionDescriptiveName, AxisRange.Full);
    }
  }

  public Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.Stick GetStick(int elementId)
  {
    for (int index = 0; index < this._sticks.Length; ++index)
    {
      if (this._sticks[index].ContainsElement(elementId))
        return this._sticks[index];
    }
    return (Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.Stick) null;
  }

  public void OnControllerConnected(ControllerStatusChangedEventArgs args) => this.DrawLabels();

  public void OnControllerDisconnected(ControllerStatusChangedEventArgs args) => this.DrawLabels();

  public class Stick
  {
    public RectTransform _transform;
    public Vector2 _origPosition;
    public int _xAxisElementId = -1;
    public int _yAxisElementId = -1;

    public Vector2 position
    {
      get
      {
        return !((UnityEngine.Object) this._transform != (UnityEngine.Object) null) ? Vector2.zero : this._transform.anchoredPosition - this._origPosition;
      }
      set
      {
        if ((UnityEngine.Object) this._transform == (UnityEngine.Object) null)
          return;
        this._transform.anchoredPosition = this._origPosition + value;
      }
    }

    public Stick(RectTransform transform, int xAxisElementId, int yAxisElementId)
    {
      if ((UnityEngine.Object) transform == (UnityEngine.Object) null)
        return;
      this._transform = transform;
      this._origPosition = this._transform.anchoredPosition;
      this._xAxisElementId = xAxisElementId;
      this._yAxisElementId = yAxisElementId;
    }

    public void Reset()
    {
      if ((UnityEngine.Object) this._transform == (UnityEngine.Object) null)
        return;
      this._transform.anchoredPosition = this._origPosition;
    }

    public bool ContainsElement(int elementId)
    {
      if ((UnityEngine.Object) this._transform == (UnityEngine.Object) null)
        return false;
      return elementId == this._xAxisElementId || elementId == this._yAxisElementId;
    }

    public void SetAxisPosition(int elementId, float value)
    {
      if ((UnityEngine.Object) this._transform == (UnityEngine.Object) null)
        return;
      Vector2 position = this.position;
      if (elementId == this._xAxisElementId)
        position.x = value;
      else if (elementId == this._yAxisElementId)
        position.y = value;
      this.position = position;
    }
  }

  public class UIElement
  {
    public int id;
    public ControllerUIElement element;

    public UIElement(int id, ControllerUIElement element)
    {
      this.id = id;
      this.element = element;
    }
  }
}
