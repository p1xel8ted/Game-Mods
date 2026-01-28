// Decompiled with JetBrains decompiler
// Type: GeneralInputSource
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public bool _mouseInputActive;
  public bool _mouseInputEnabled;

  public bool MouseInputActive
  {
    get => this._mouseInputActive;
    set => this._mouseInputActive = value;
  }

  public bool MouseInputEnabled
  {
    get => this._mouseInputEnabled;
    set => this._mouseInputEnabled = value;
  }

  public bool InputIsController(PlayerFarming playerFarming = null)
  {
    if ((UnityEngine.Object) playerFarming != (UnityEngine.Object) null && playerFarming.rewiredPlayer != null)
      return this.InputIsController(this.GetLastActiveController(playerFarming));
    return this._rewiredPlayer != null && this.InputIsController(this.GetLastActiveController());
  }

  public bool InputIsController(Controller controller)
  {
    return controller != null && controller.type == ControllerType.Joystick;
  }

  public Controller GetController(
    ControllerType controllerType,
    PlayerFarming playerFarming = null,
    Player rewiredPlayer = null)
  {
    if ((UnityEngine.Object) playerFarming != (UnityEngine.Object) null || rewiredPlayer != null)
    {
      if (rewiredPlayer == null)
      {
        if ((UnityEngine.Object) playerFarming == (UnityEngine.Object) null)
          return this.GetController(controllerType);
        rewiredPlayer = playerFarming.rewiredPlayer;
        if (rewiredPlayer == null)
          return (Controller) null;
      }
      if (controllerType == ControllerType.Joystick)
      {
        foreach (Joystick joystick in (IEnumerable<Joystick>) rewiredPlayer.controllers.Joysticks)
        {
          if (joystick.isConnected && joystick.enabled)
            return rewiredPlayer.controllers.GetController(controllerType, joystick.id);
        }
      }
    }
    else
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
    }
    return this._rewiredPlayer.controllers.GetController(controllerType, 0);
  }

  public Controller GetLastActiveController(PlayerFarming playerFarming = null)
  {
    return (UnityEngine.Object) playerFarming != (UnityEngine.Object) null ? (playerFarming.rewiredPlayer != null ? playerFarming.rewiredPlayer.controllers.GetLastActiveController() : this.GetLastActiveController()) : (this._rewiredPlayer != null ? this._rewiredPlayer.controllers.GetLastActiveController() : (Controller) null);
  }

  public Controller GetDefaultController(PlayerFarming playerFarming)
  {
    if ((UnityEngine.Object) playerFarming != (UnityEngine.Object) null && playerFarming.rewiredPlayer != null)
    {
      using (IEnumerator<Controller> enumerator = ((IEnumerable<Controller>) playerFarming.rewiredPlayer.controllers.Controllers).GetEnumerator())
      {
        if (enumerator.MoveNext())
          return enumerator.Current;
      }
    }
    return (Controller) null;
  }

  public ControllerMap GetMap(Controller controller)
  {
    return this._rewiredPlayer.controllers.maps.GetMap(controller, 0);
  }

  public ControllerMap GetControllerMapForCategory(
    int category,
    ControllerType controllerType,
    PlayerFarming playerFarming = null,
    Player rewiredPlayer = null)
  {
    return this.GetControllerMapForCategory(category, this.GetController(controllerType, playerFarming, rewiredPlayer), playerFarming, rewiredPlayer);
  }

  public ControllerMap GetControllerMapForCategory(
    int category,
    Controller controller,
    PlayerFarming playerFarming = null,
    Player rewiredPlayer = null)
  {
    if (category == 0)
      return InputManager.Gameplay.GetControllerMap(controller, playerFarming, rewiredPlayer);
    return category == 2 ? InputManager.PhotoMode.GetControllerMap(controller, playerFarming, rewiredPlayer) : InputManager.UI.GetControllerMap(controller, playerFarming, rewiredPlayer);
  }

  public override void UpdateRewiredPlayer()
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

  public override void OnLastActiveControllerChanged(Player player, Controller controller)
  {
    if (controller == null || InputMapper.Default.status == InputMapper.Status.Listening)
      return;
    Cursor.visible = controller.type == ControllerType.Keyboard || controller.type == ControllerType.Mouse;
    Action<Controller> controllerChanged = this.OnActiveControllerChanged;
    if (controllerChanged == null)
      return;
    controllerChanged(controller);
  }

  public static ESteamInputType GetSteamInputType()
  {
    return SteamInput.Init(false) ? SteamInput.GetInputTypeForHandle(SteamInput.GetControllerForGamepadIndex(0)) : ESteamInputType.k_ESteamInputType_Unknown;
  }

  public bool GetAnyButton(PlayerFarming playerFarming = null)
  {
    if ((UnityEngine.Object) playerFarming != (UnityEngine.Object) null && playerFarming.rewiredPlayer != null)
    {
      Controller activeController = this.GetLastActiveController(playerFarming);
      if (activeController != null)
        return activeController.GetAnyButton();
    }
    else if (this._rewiredPlayer != null)
    {
      Controller activeController = this.GetLastActiveController();
      if (activeController != null)
        return activeController.GetAnyButton();
    }
    return false;
  }

  public bool GetAnyAxisHold()
  {
    return (double) Mathf.Abs(this.GetAxis(0)) > 0.10000000149011612 || (double) Mathf.Abs(this.GetAxis(1)) > 0.10000000149011612;
  }

  public Vector2 GetMousePosition(PlayerFarming playerFarming = null)
  {
    if ((UnityEngine.Object) playerFarming == (UnityEngine.Object) null)
      playerFarming = PlayerFarming.Instance;
    return !((UnityEngine.Object) playerFarming != (UnityEngine.Object) null) ? this._rewiredPlayer.controllers.Mouse.screenPosition : playerFarming.rewiredPlayer.controllers.Mouse.screenPosition;
  }

  public void ResetBindings()
  {
    this._rewiredPlayer.controllers.maps.LoadDefaultMaps(ControllerType.Keyboard);
    this._rewiredPlayer.controllers.maps.LoadDefaultMaps(ControllerType.Joystick);
    this._rewiredPlayer.controllers.maps.LoadDefaultMaps(ControllerType.Mouse);
    InputManager.Gameplay.ApplyAllBindings();
    InputManager.UI.ApplyAllBindings();
    InputManager.PhotoMode.ApplyAllBindings();
    System.Action onBindingsReset = GeneralInputSource.OnBindingsReset;
    if (onBindingsReset == null)
      return;
    onBindingsReset();
  }
}
