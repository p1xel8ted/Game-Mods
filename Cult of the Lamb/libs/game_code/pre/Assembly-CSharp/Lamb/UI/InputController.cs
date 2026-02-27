// Decompiled with JetBrains decompiler
// Type: Lamb.UI.InputController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Rewired;
using src.UINavigator;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class InputController : BaseMonoBehaviour
{
  [SerializeField]
  private InputIdentifier[] _buttons;
  private ControllerType[] _controllerTypes = new ControllerType[3]
  {
    ControllerType.Keyboard,
    ControllerType.Mouse,
    ControllerType.Joystick
  };

  private void OnEnable()
  {
    this.HideAll();
    MonoSingleton<UINavigatorNew>.Instance.OnSelectionChanged += new Action<Selectable, Selectable>(this.OnSelectionChanged);
    MonoSingleton<UINavigatorNew>.Instance.OnDefaultSetComplete += new Action<Selectable>(this.OnSelection);
  }

  private void OnDisable()
  {
    if (!((UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance != (UnityEngine.Object) null))
      return;
    MonoSingleton<UINavigatorNew>.Instance.OnSelectionChanged -= new Action<Selectable, Selectable>(this.OnSelectionChanged);
    MonoSingleton<UINavigatorNew>.Instance.OnDefaultSetComplete -= new Action<Selectable>(this.OnSelection);
  }

  private void OnSelectionChanged(Selectable current, Selectable previous)
  {
    this.OnSelection(current);
  }

  private void OnSelection(Selectable current)
  {
    foreach (ControllerType controllerType in this._controllerTypes)
      this.HideAll(controllerType);
    KeybindItemProxy component1;
    if (current.TryGetComponent<KeybindItemProxy>(out component1))
    {
      if (component1.KeybindItem.ShowKeyboardBinding)
        this.Select(component1.KeybindItem.KeyboardBinding);
      if (component1.KeybindItem.ShowMouseBinding)
        this.Select(component1.KeybindItem.MouseBinding);
      if (!component1.KeybindItem.ShowJoystickBinding)
        return;
      this.Select(component1.KeybindItem.JoystickBinding);
    }
    else
    {
      BindingItem component2;
      if (current.TryGetComponent<BindingItem>(out component2))
      {
        this.Select(component2);
      }
      else
      {
        KeybindItemNonBindable component3;
        if (current.TryGetComponent<KeybindItemNonBindable>(out component3))
        {
          foreach (InputIdentifier button in this._buttons)
          {
            if (button.Button == component3.Button)
              button.Show();
            else
              button.Hide();
          }
        }
        else
        {
          BindingPrompt component4;
          if (current.TryGetComponent<BindingPrompt>(out component4))
          {
            foreach (InputIdentifier button in this._buttons)
            {
              if (button.Button == component4.Button)
                button.Show();
              else
                button.Hide();
            }
          }
          else
            this.HideAll();
        }
      }
    }
  }

  private void Select(BindingItem bindingItem)
  {
    if ((UnityEngine.Object) bindingItem == (UnityEngine.Object) null)
      return;
    Controller controller = InputManager.General.GetController(bindingItem.ControllerType);
    if (controller.type != bindingItem.ControllerType)
      return;
    ControllerMap controllerMap;
    if (bindingItem.Category == 0)
    {
      controllerMap = InputManager.Gameplay.GetControllerMap(controller);
    }
    else
    {
      if (bindingItem.Category != 1)
        return;
      controllerMap = InputManager.UI.GetControllerMap(controller);
    }
    if (controllerMap == null)
      return;
    ActionElementMap actionElementMap1 = (ActionElementMap) null;
    ActionElementMap[] elementMapsWithAction = controllerMap.GetElementMapsWithAction(bindingItem.Action);
    foreach (ActionElementMap actionElementMap2 in elementMapsWithAction)
    {
      if (bindingItem.AxisContribution == actionElementMap2.axisContribution)
      {
        actionElementMap1 = actionElementMap2;
        break;
      }
    }
    if (actionElementMap1 == null)
    {
      if (elementMapsWithAction.Length == 0)
        return;
      actionElementMap1 = elementMapsWithAction[0];
    }
    if (bindingItem.ControllerType == ControllerType.Keyboard)
    {
      foreach (InputIdentifier button in this._buttons)
      {
        if (button.ControllerType == controller.type)
        {
          if (button.KeyboardKeyCode == actionElementMap1.keyboardKeyCode)
            button.Show();
          else
            button.Hide();
        }
      }
    }
    else if (bindingItem.ControllerType == ControllerType.Mouse)
    {
      foreach (InputIdentifier button in this._buttons)
      {
        if (button.ControllerType == controller.type)
        {
          if (button.MouseInputElement == (MouseInputElement) actionElementMap1.elementIdentifierId)
            button.Show();
          else
            button.Hide();
        }
      }
    }
    else
    {
      IGamepadTemplate template = controller.GetTemplate<IGamepadTemplate>();
      List<ControllerTemplateElementTarget> templateElementTargetList = new List<ControllerTemplateElementTarget>();
      ControllerElementTarget target = (ControllerElementTarget) actionElementMap1;
      List<ControllerTemplateElementTarget> results = templateElementTargetList;
      template.GetElementTargets(target, (IList<ControllerTemplateElementTarget>) results);
      int id = templateElementTargetList[0].element.id;
      foreach (InputIdentifier button in this._buttons)
      {
        if (button.ControllerType == controller.type)
        {
          if (button.Button == id)
            button.Show();
          else
            button.Hide();
        }
      }
    }
  }

  private void HideAll()
  {
    this.HideAll(ControllerType.Keyboard, true);
    this.HideAll(ControllerType.Mouse, true);
    this.HideAll(ControllerType.Joystick, true);
  }

  private void HideAll(ControllerType controllerType, bool instant = false)
  {
    foreach (InputIdentifier button in this._buttons)
    {
      if (button.ControllerType == controllerType)
        button.Hide(instant);
    }
  }
}
