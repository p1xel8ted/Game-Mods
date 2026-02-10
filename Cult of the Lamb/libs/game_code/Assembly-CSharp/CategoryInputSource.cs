// Decompiled with JetBrains decompiler
// Type: CategoryInputSource
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Rewired;
using src.Extensions;
using src.UINavigator;
using System.Collections.Generic;
using System.Linq;
using Unify.Input;
using UnityEngine;

#nullable disable
public abstract class CategoryInputSource : InputSource
{
  public List<Binding> _defaultKeyboardBindings = new List<Binding>();
  public List<Binding> _defaultMouseBindings = new List<Binding>();
  public List<Binding> _defaultGamepadBindings = new List<Binding>();
  public ControllerMap _keyboardMap;
  public ControllerMap _mouseMap;
  public ControllerMap _gamepadMap;

  public abstract int Category { get; }

  public override void UpdateRewiredPlayer()
  {
    base.UpdateRewiredPlayer();
    this.WaitAndRecord();
  }

  public async System.Threading.Tasks.Task WaitAndRecord()
  {
    CategoryInputSource categoryInputSource = this;
    await System.Threading.Tasks.Task.Delay(100);
    categoryInputSource.RecordDefaultBindings((Controller) categoryInputSource._rewiredPlayer.controllers.Keyboard, categoryInputSource._defaultKeyboardBindings);
    categoryInputSource.RecordDefaultBindings((Controller) categoryInputSource._rewiredPlayer.controllers.Mouse, categoryInputSource._defaultMouseBindings);
    foreach (Joystick joystick in (IEnumerable<Joystick>) categoryInputSource._rewiredPlayer.controllers.Joysticks)
      categoryInputSource.RecordDefaultBindings((Controller) joystick, categoryInputSource._defaultGamepadBindings);
    categoryInputSource.ApplyAllBindings();
  }

  public override void OnLastActiveControllerChanged(Player player, Controller controller)
  {
    if (controller == null || controller.type != ControllerType.Joystick)
      return;
    this.LoadAndBind(controller);
  }

  public void RecordDefaultBindings(
    Controller controller,
    List<Binding> target,
    PlayerFarming playerFarming = null)
  {
    if (controller == null)
    {
      Debug.Log((object) "RecordDefaultBindings - Controller was null");
    }
    else
    {
      if (target.Count > 0)
        return;
      ControllerMap controllerMap = controller.type != ControllerType.Joystick ? this.GetControllerMap(controller, playerFarming) : this.GetControllerMap(controller, 3, playerFarming);
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

  public ControllerMap GetControllerMap(
    Controller controller,
    PlayerFarming playerFarming = null,
    Player rewiredPlayer = null)
  {
    if (controller == null)
      return (ControllerMap) null;
    if (controller.type == ControllerType.Joystick)
    {
      try
      {
        int mapIndex = (Object) playerFarming == (Object) null || playerFarming.isLamb ? SettingsManager.Settings.Control.GamepadLayout : SettingsManager.Settings.Control.GamepadLayout_P2;
        return this.GetControllerMap(controller, mapIndex, playerFarming, rewiredPlayer);
      }
      catch
      {
        this.GetControllerMap(controller, 0, playerFarming, rewiredPlayer);
      }
    }
    return this.GetControllerMap(controller, 0, playerFarming, rewiredPlayer);
  }

  public ControllerMap GetControllerMap(
    Controller controller,
    int mapIndex,
    PlayerFarming playerFarming = null,
    Player rewiredPlayer = null)
  {
    if ((bool) (Object) playerFarming && rewiredPlayer == null)
      rewiredPlayer = playerFarming.rewiredPlayer;
    else if (rewiredPlayer == null)
      rewiredPlayer = RewiredInputManager.GetPlayer(0);
    if (rewiredPlayer != null && rewiredPlayer.controllers != null && rewiredPlayer.controllers.maps != null)
      return rewiredPlayer.controllers.maps.GetMap(controller, this.Category, mapIndex);
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

  public void ApplyBindings()
  {
    this.LoadAndBind(InputManager.General.GetLastActiveController(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer));
  }

  public void ApplyAllBindings()
  {
    this.LoadAndBind((Controller) this._rewiredPlayer.controllers.Keyboard);
    this.LoadAndBind((Controller) this._rewiredPlayer.controllers.Mouse);
    foreach (Controller joystick in (IEnumerable<Joystick>) this._rewiredPlayer.controllers.Joysticks)
      this.LoadAndBind(joystick);
  }

  public void ApplyAllBindingsAllPlayers()
  {
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      foreach (Controller joystick in (IEnumerable<Joystick>) player.rewiredPlayer.controllers.Joysticks)
        this.LoadAndBind(joystick);
    }
  }

  public void LoadAndBind(Controller controller)
  {
    if (controller == null)
      Debug.Log((object) "LoadAndBind - Controller was null, could not bind");
    else if (SettingsManager.Settings == null)
    {
      Debug.LogWarning((object) "LoadAndBind - SettingsManager.Settings is not available yet (null).");
    }
    else
    {
      Player rewiredPlayer = this._rewiredPlayer;
      int layoutId = SettingsManager.Settings.Control.GamepadLayout;
      List<Binding> bindings = SettingsManager.Settings.Control.GamepadBindings;
      List<UnboundBinding> unboundBindings = SettingsManager.Settings.Control.GamepadBindingsUnbound;
      PlayerFarming playerFarming = MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer;
      foreach (PlayerFarming player in PlayerFarming.players)
      {
        if (player.rewiredPlayer != null && ((IEnumerable<Controller>) player.rewiredPlayer.controllers.Controllers).Contains<Controller>(controller) && !player.isLamb)
        {
          Debug.Log((object) "LoadAndBind - Controller player 2");
          playerFarming = player;
          layoutId = SettingsManager.Settings.Control.GamepadLayout_P2;
          bindings = SettingsManager.Settings.Control.GamepadBindings_P2;
          unboundBindings = SettingsManager.Settings.Control.GamepadBindingsUnbound_P2;
        }
      }
      if ((Object) playerFarming != (Object) null && playerFarming.rewiredPlayer != null)
        rewiredPlayer = playerFarming.rewiredPlayer;
      if (controller.type == ControllerType.Joystick)
      {
        foreach (ControllerMap controllerMap in (IEnumerable<ControllerMap>) this.GetControllerMaps(controller))
        {
          if (controllerMap.layoutId != layoutId)
          {
            rewiredPlayer.controllers.maps.RemoveMap(ControllerType.Joystick, controller.id, this.Category, controllerMap.layoutId);
            break;
          }
        }
        rewiredPlayer.controllers.maps.LoadMap(ControllerType.Joystick, controller.id, this.Category, layoutId);
        rewiredPlayer.controllers.maps.SetMapsEnabled(true, ControllerType.Joystick, this.Category, layoutId);
        if (layoutId == 3)
        {
          this.RecordDefaultBindings(controller, this._defaultGamepadBindings, playerFarming);
          ControllerMap controllerMap = this.GetControllerMap(controller, playerFarming);
          ControlSettingsUtilities.ApplyBindings(controllerMap, bindings);
          ControlSettingsUtilities.DeleteUnboundBindings(controllerMap, unboundBindings);
        }
      }
      if (controller.type == ControllerType.Keyboard)
      {
        this.RecordDefaultBindings(controller, this._defaultKeyboardBindings);
        ControllerMap controllerMap = this.GetControllerMap(controller);
        ControlSettingsUtilities.ApplyBindings(controllerMap, SettingsManager.Settings.Control.KeyboardBindings);
        ControlSettingsUtilities.DeleteUnboundBindings(controllerMap, SettingsManager.Settings.Control.KeyboardBindingsUnbound);
      }
      if (controller.type == ControllerType.Mouse)
      {
        this.RecordDefaultBindings(controller, this._defaultMouseBindings);
        ControllerMap controllerMap = this.GetControllerMap(controller);
        ControlSettingsUtilities.ApplyBindings(controllerMap, SettingsManager.Settings.Control.MouseBindings);
        ControlSettingsUtilities.DeleteUnboundBindings(controllerMap, SettingsManager.Settings.Control.MouseBindingsUnbound);
      }
      Debug.Log((object) $"LoadAndBind - Successfully bound {controller.name} ({controller.type}) with layout {layoutId}");
    }
  }
}
