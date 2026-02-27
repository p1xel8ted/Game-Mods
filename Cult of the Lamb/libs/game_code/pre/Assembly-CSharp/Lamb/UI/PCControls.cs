// Decompiled with JetBrains decompiler
// Type: Lamb.UI.PCControls
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Rewired;
using src.Extensions;
using src.UINavigator;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class PCControls : ControlsScreenBase
{
  [SerializeField]
  private PCBindings _bindings;

  private void Update()
  {
    if (MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable == null)
      return;
    BindingItem component1;
    if (InputManager.UI.GetResetBindingButtonDown() && MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.Selectable.gameObject.TryGetComponent<BindingItem>(out component1))
    {
      ControllerMap controllerMapForCategory = InputManager.General.GetControllerMapForCategory(component1.Category, component1.ControllerType);
      if (controllerMapForCategory != null)
        ControlSettingsUtilities.ResetBinding(controllerMapForCategory, component1.Action, component1.AxisContribution);
    }
    BindingItem component2;
    if (!InputManager.UI.GetUnbindButtonDown() || !MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.Selectable.gameObject.TryGetComponent<BindingItem>(out component2))
      return;
    ControllerMap controllerMapForCategory1 = InputManager.General.GetControllerMapForCategory(component2.Category, component2.ControllerType);
    ActionElementMap actionElementMap = controllerMapForCategory1.GetActionElementMap(component2.Action, component2.AxisContribution);
    if (actionElementMap == null || !ControlSettingsUtilities.DeleteElementMap(controllerMapForCategory1, actionElementMap))
      return;
    if (component2.ControllerType == ControllerType.Keyboard)
    {
      SettingsManager.Settings.Control.KeyboardBindingsUnbound.Add(actionElementMap.ToUnboundBinding());
    }
    else
    {
      if (component2.ControllerType != ControllerType.Mouse)
        return;
      SettingsManager.Settings.Control.MouseBindingsUnbound.Add(actionElementMap.ToUnboundBinding());
    }
  }

  protected override void OnShowStarted() => this._bindings.Show(true);

  public override void Configure(InputType inputType) => this._bindings.Configure(inputType);

  public override void Configure(SettingsData.ControlSettings controlSettings)
  {
  }

  public override bool ValidInputType(InputType inputType) => inputType == InputType.Keyboard;

  public override bool ShowBindingPrompts() => true;
}
