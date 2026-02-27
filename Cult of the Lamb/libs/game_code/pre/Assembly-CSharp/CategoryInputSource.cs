// Decompiled with JetBrains decompiler
// Type: CategoryInputSource
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Rewired;
using src.Extensions;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public abstract class CategoryInputSource : InputSource
{
  private List<Binding> _defaultKeyboardBindings = new List<Binding>();
  private List<Binding> _defaultMouseBindings = new List<Binding>();
  private List<Binding> _defaultGamepadBindings = new List<Binding>();
  private ControllerMap _keyboardMap;
  private ControllerMap _mouseMap;
  private ControllerMap _gamepadMap;

  protected abstract int Category { get; }

  protected override void UpdateRewiredPlayer()
  {
    base.UpdateRewiredPlayer();
    this.WaitAndRecord();
  }

  private async System.Threading.Tasks.Task WaitAndRecord()
  {
    CategoryInputSource categoryInputSource = this;
    await System.Threading.Tasks.Task.Delay(100);
    categoryInputSource.RecordDefaultBindings((Controller) categoryInputSource._rewiredPlayer.controllers.Keyboard, categoryInputSource._defaultKeyboardBindings);
    categoryInputSource.RecordDefaultBindings((Controller) categoryInputSource._rewiredPlayer.controllers.Mouse, categoryInputSource._defaultMouseBindings);
    foreach (Joystick joystick in (IEnumerable<Joystick>) categoryInputSource._rewiredPlayer.controllers.Joysticks)
      categoryInputSource.RecordDefaultBindings((Controller) joystick, categoryInputSource._defaultGamepadBindings);
    categoryInputSource.ApplyAllBindings();
  }

  protected override void OnLastActiveControllerChanged(Player player, Controller controller)
  {
    if (controller == null || controller.type != ControllerType.Joystick)
      return;
    this.LoadAndBind(controller);
  }

  private void RecordDefaultBindings(Controller controller, List<Binding> target)
  {
    if (controller == null)
    {
      Debug.Log((object) "RecordDefaultBindings - Controller was null");
    }
    else
    {
      if (target.Count > 0)
        return;
      ControllerMap controllerMap = controller.type != ControllerType.Joystick ? this.GetControllerMap(controller) : this.GetControllerMap(controller, 3);
      if (controllerMap == null)
      {
        Debug.Log((object) "Controller map is null. Can't record binds");
      }
      else
      {
        foreach (ActionElementMap allMap in (IEnumerable<ActionElementMap>) controllerMap.AllMaps)
          target.Add(allMap.ToBinding());
      }
    }
  }

  public List<Binding> GetDefaultBindingsForControllerType(ControllerType controllerType)
  {
    switch (controllerType)
    {
      case ControllerType.Keyboard:
        return this._defaultKeyboardBindings;
      case ControllerType.Mouse:
        return this._defaultMouseBindings;
      case ControllerType.Joystick:
        return this._defaultGamepadBindings;
      default:
        return (List<Binding>) null;
    }
  }

  public ControllerMap GetControllerMap(Controller controller)
  {
    return controller.type == ControllerType.Joystick ? this.GetControllerMap(controller, SettingsManager.Settings.Control.GamepadLayout) : this.GetControllerMap(controller, 0);
  }

  public ControllerMap GetControllerMap(Controller controller, int mapIndex)
  {
    if (this._rewiredPlayer == null)
      return (ControllerMap) null;
    if (controller.type != ControllerType.Keyboard)
    {
      int type = (int) controller.type;
    }
    return this._rewiredPlayer.controllers.maps.GetMap(controller, this.Category, mapIndex);
  }

  public IList<ControllerMap> GetControllerMaps(Controller controller)
  {
    return this._rewiredPlayer == null ? (IList<ControllerMap>) null : (IList<ControllerMap>) this._rewiredPlayer.controllers.maps.GetMaps(controller);
  }

  public void ApplyBindings() => this.LoadAndBind(InputManager.General.GetLastActiveController());

  public void ApplyAllBindings()
  {
    this.LoadAndBind((Controller) this._rewiredPlayer.controllers.Keyboard);
    this.LoadAndBind((Controller) this._rewiredPlayer.controllers.Mouse);
    foreach (Controller joystick in (IEnumerable<Joystick>) this._rewiredPlayer.controllers.Joysticks)
      this.LoadAndBind(joystick);
  }

  private void LoadAndBind(Controller controller)
  {
    if (controller == null)
    {
      Debug.Log((object) "LoadAndBind - Controller was null, could not bind");
    }
    else
    {
      if (controller.type == ControllerType.Keyboard)
      {
        this.RecordDefaultBindings(controller, this._defaultKeyboardBindings);
        ControllerMap controllerMap = this.GetControllerMap(controller);
        ControlSettingsUtilities.ApplyBindings(controllerMap, SettingsManager.Settings.Control.KeyboardBindings);
        ControlSettingsUtilities.DeleteUnboundBindings(controllerMap, SettingsManager.Settings.Control.KeyboardBindingsUnbound);
      }
      if (controller.type == ControllerType.Joystick)
      {
        foreach (ControllerMap controllerMap in (IEnumerable<ControllerMap>) this.GetControllerMaps(controller))
        {
          if (controllerMap.layoutId != SettingsManager.Settings.Control.GamepadLayout)
          {
            this._rewiredPlayer.controllers.maps.RemoveMap(ControllerType.Joystick, controller.id, this.Category, controllerMap.layoutId);
            break;
          }
        }
        this._rewiredPlayer.controllers.maps.LoadMap(ControllerType.Joystick, controller.id, this.Category, SettingsManager.Settings.Control.GamepadLayout);
        this._rewiredPlayer.controllers.maps.SetMapsEnabled(true, ControllerType.Joystick, this.Category, SettingsManager.Settings.Control.GamepadLayout);
        if (SettingsManager.Settings.Control.GamepadLayout == 3)
        {
          this.RecordDefaultBindings(controller, this._defaultGamepadBindings);
          ControllerMap controllerMap = this.GetControllerMap(controller);
          ControlSettingsUtilities.ApplyBindings(controllerMap, SettingsManager.Settings.Control.GamepadBindings);
          ControlSettingsUtilities.DeleteUnboundBindings(controllerMap, SettingsManager.Settings.Control.GamepadBindingsUnbound);
        }
      }
      if (controller.type != ControllerType.Mouse)
        return;
      this.RecordDefaultBindings(controller, this._defaultMouseBindings);
      ControllerMap controllerMap1 = this.GetControllerMap(controller);
      ControlSettingsUtilities.ApplyBindings(controllerMap1, SettingsManager.Settings.Control.MouseBindings);
      ControlSettingsUtilities.DeleteUnboundBindings(controllerMap1, SettingsManager.Settings.Control.MouseBindingsUnbound);
    }
  }
}
