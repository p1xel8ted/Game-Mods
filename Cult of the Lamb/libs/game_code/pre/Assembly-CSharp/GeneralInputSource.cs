// Decompiled with JetBrains decompiler
// Type: GeneralInputSource
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Rewired;
using Steamworks;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class GeneralInputSource : InputSource
{
  public Action<Controller> OnActiveControllerChanged;
  public static System.Action OnBindingsReset;
  private bool _mouseInputActive;

  public bool MouseInputActive
  {
    get => this._mouseInputActive;
    set => this._mouseInputActive = value;
  }

  public bool InputIsController()
  {
    if (this._rewiredPlayer == null)
      return false;
    Controller activeController = this.GetLastActiveController();
    return activeController != null && activeController.type == ControllerType.Joystick;
  }

  public Controller GetController(ControllerType controllerType)
  {
    if (this._rewiredPlayer == null)
      return (Controller) null;
    if (controllerType == ControllerType.Joystick)
    {
      foreach (Joystick joystick in (IEnumerable<Joystick>) this._rewiredPlayer.controllers.Joysticks)
      {
        if (joystick.isConnected && joystick.enabled)
          return this._rewiredPlayer.controllers.GetController(controllerType, joystick.id);
      }
    }
    return this._rewiredPlayer.controllers.GetController(controllerType, 0);
  }

  public Controller GetLastActiveController()
  {
    return this._rewiredPlayer != null ? this._rewiredPlayer.controllers.GetLastActiveController() : (Controller) null;
  }

  public ControllerMap GetMap(Controller controller)
  {
    return this._rewiredPlayer.controllers.maps.GetMap(controller, 0);
  }

  public ControllerMap GetControllerMapForCategory(int category, ControllerType controllerType)
  {
    return this.GetControllerMapForCategory(category, this.GetController(controllerType));
  }

  public ControllerMap GetControllerMapForCategory(int category, Controller controller)
  {
    return category == 0 ? InputManager.Gameplay.GetControllerMap(controller) : InputManager.UI.GetControllerMap(controller);
  }

  protected override void UpdateRewiredPlayer()
  {
    base.UpdateRewiredPlayer();
    this._rewiredPlayer.controllers.AddLastActiveControllerChangedDelegate(new PlayerActiveControllerChangedDelegate(((InputSource) this).OnLastActiveControllerChanged));
  }

  public void RemoveController(ControllerType controllerType)
  {
    this._rewiredPlayer.controllers.RemoveController(controllerType, 0);
  }

  public void AddController(ControllerType controllerType)
  {
    this._rewiredPlayer.controllers.AddController(controllerType, 0, false);
  }

  protected override void OnLastActiveControllerChanged(Player player, Controller controller)
  {
    if (controller == null || InputMapper.Default.status == InputMapper.Status.Listening)
      return;
    Cursor.visible = controller.type == ControllerType.Keyboard || controller.type == ControllerType.Mouse;
    Debug.Log((object) controller.type.ToString().Colour(Color.yellow));
    Action<Controller> controllerChanged = this.OnActiveControllerChanged;
    if (controllerChanged == null)
      return;
    controllerChanged(controller);
  }

  public static ESteamInputType GetSteamInputType()
  {
    return SteamInput.Init(false) ? SteamInput.GetInputTypeForHandle(SteamInput.GetControllerForGamepadIndex(0)) : ESteamInputType.k_ESteamInputType_Unknown;
  }

  public bool GetAnyButton()
  {
    if (this._rewiredPlayer != null)
    {
      Controller activeController = this.GetLastActiveController();
      if (activeController != null)
        return activeController.GetAnyButton();
    }
    return false;
  }

  public Vector2 GetMousePosition() => this._rewiredPlayer.controllers.Mouse.screenPosition;

  public void ResetBindings()
  {
    this._rewiredPlayer.controllers.maps.LoadDefaultMaps(ControllerType.Keyboard);
    this._rewiredPlayer.controllers.maps.LoadDefaultMaps(ControllerType.Joystick);
    this._rewiredPlayer.controllers.maps.LoadDefaultMaps(ControllerType.Mouse);
    InputManager.Gameplay.ApplyAllBindings();
    InputManager.UI.ApplyAllBindings();
    System.Action onBindingsReset = GeneralInputSource.OnBindingsReset;
    if (onBindingsReset == null)
      return;
    onBindingsReset();
  }
}
