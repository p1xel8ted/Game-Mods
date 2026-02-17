// Decompiled with JetBrains decompiler
// Type: Lamb.UI.GamepadLayout
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Rewired;
using src.UINavigator;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class GamepadLayout : UISubmenuBase
{
  [SerializeField]
  public Selectable _selectable;
  [SerializeField]
  public InputDisplay[] _controllers;
  [SerializeField]
  public BindingPrompt[] _bindingPrompts;

  public void OnEnable()
  {
    ControlSettingsUtilities.OnGamepadLayoutChanged += new System.Action(this.OnGamepadLayoutChanged);
  }

  public new void OnDisable()
  {
    ControlSettingsUtilities.OnGamepadLayoutChanged -= new System.Action(this.OnGamepadLayoutChanged);
  }

  public override void OnShowStarted() => this.OnGamepadLayoutChanged();

  public void OnGamepadLayoutChanged()
  {
    this.Configure(InputManager.General.GetLastActiveController(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer));
  }

  public void Configure(Controller controller)
  {
    if (controller.type == ControllerType.Keyboard || controller.type == ControllerType.Mouse)
      return;
    ControllerMap controllerMapForCategory1 = InputManager.General.GetControllerMapForCategory(0, controller, MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer);
    if (controllerMapForCategory1 == null || controllerMapForCategory1.layoutId == 3)
      return;
    ControllerMap controllerMapForCategory2 = InputManager.General.GetControllerMapForCategory(1, controller, MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer);
    InputType currentInputType = ControlUtilities.GetCurrentInputType(controller);
    Platform platformFromInputType = ControlUtilities.GetPlatformFromInputType(currentInputType);
    IGamepadTemplate template = controller.GetTemplate<IGamepadTemplate>();
    List<ControllerTemplateElementTarget> results = new List<ControllerTemplateElementTarget>();
    foreach (InputDisplay controller1 in this._controllers)
      controller1.Configure(currentInputType);
    foreach (BindingPrompt bindingPrompt in this._bindingPrompts)
    {
      bindingPrompt.Platform = platformFromInputType;
      bindingPrompt.Clear();
      foreach (ActionElementMap allMap in (IEnumerable<ActionElementMap>) controllerMapForCategory1.AllMaps)
      {
        template.GetElementTargets((ControllerElementTarget) allMap, (IList<ControllerTemplateElementTarget>) results);
        bindingPrompt.TryAddAction(results[0], allMap);
      }
      foreach (ActionElementMap allMap in (IEnumerable<ActionElementMap>) controllerMapForCategory2.AllMaps)
      {
        if (this.IsValidUIAction(allMap))
        {
          template.GetElementTargets((ControllerElementTarget) allMap, (IList<ControllerTemplateElementTarget>) results);
          bindingPrompt.TryAddAction(results[0], allMap);
        }
      }
      bindingPrompt.FinalizeBinding();
    }
  }

  public bool IsValidUIAction(ActionElementMap actionElementMap)
  {
    switch (actionElementMap.actionId)
    {
      case 39:
      case 43:
      case 44:
        return true;
      default:
        return false;
    }
  }

  public Selectable ProvideSelectableForLayoutSelector() => this._selectable;
}
