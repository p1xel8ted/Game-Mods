// Decompiled with JetBrains decompiler
// Type: Lamb.UI.GamepadLayout
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Rewired;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class GamepadLayout : UISubmenuBase
{
  [SerializeField]
  private Selectable _selectable;
  [SerializeField]
  private InputDisplay[] _controllers;
  [SerializeField]
  private BindingPrompt[] _bindingPrompts;

  private void OnEnable()
  {
    ControlSettingsUtilities.OnGamepadLayoutChanged += new System.Action(this.OnGamepadLayoutChanged);
  }

  private void OnDisable()
  {
    ControlSettingsUtilities.OnGamepadLayoutChanged -= new System.Action(this.OnGamepadLayoutChanged);
  }

  protected override void OnShowStarted() => this.OnGamepadLayoutChanged();

  private void OnGamepadLayoutChanged()
  {
    this.Configure(InputManager.General.GetLastActiveController());
  }

  public void Configure(Controller controller)
  {
    ControllerMap controllerMapForCategory1 = InputManager.General.GetControllerMapForCategory(0, controller);
    if (controllerMapForCategory1.layoutId == 3)
      return;
    ControllerMap controllerMapForCategory2 = InputManager.General.GetControllerMapForCategory(1, controller);
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

  private bool IsValidUIAction(ActionElementMap actionElementMap)
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
