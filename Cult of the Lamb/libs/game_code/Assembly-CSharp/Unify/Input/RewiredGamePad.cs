// Decompiled with JetBrains decompiler
// Type: Unify.Input.RewiredGamePad
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Rewired;
using System.Collections.Generic;

#nullable disable
namespace Unify.Input;

public class RewiredGamePad : GamePad
{
  public int playerId = -1;

  public int PlayerId => this.playerId;

  public static int PlayerToJoystick(int player) => player + 1;

  public static int JoystickToPlayer(int joystick) => joystick - 1;

  public RewiredGamePad(int player)
  {
    this.playerId = player;
    this.joystickId = RewiredGamePad.PlayerToJoystick(this.playerId);
    this.connected = false;
    this.vibrationEnabled = true;
  }

  public bool AxisCheck(InputManager.Actions action)
  {
    Player player = ReInput.players.GetPlayer(this.playerId);
    switch (action)
    {
      case InputManager.Actions.Up:
        return (double) player.GetAxis(RewiredUnifyInput.Instance.VertAxis) > 0.5;
      case InputManager.Actions.Down:
        return (double) player.GetAxis(RewiredUnifyInput.Instance.VertAxis) < -0.5;
      case InputManager.Actions.Left:
        return (double) player.GetAxis(RewiredUnifyInput.Instance.HorzAxis) < -0.5;
      case InputManager.Actions.Right:
        return (double) player.GetAxis(RewiredUnifyInput.Instance.HorzAxis) > 0.5;
      default:
        return false;
    }
  }

  public static string ActionToString(InputManager.Actions action)
  {
    string str = "";
    switch (action)
    {
      case InputManager.Actions.Submit:
        str = RewiredUnifyInput.Instance.SubmitAction;
        break;
      case InputManager.Actions.Cancel:
        str = RewiredUnifyInput.Instance.CancelAction;
        break;
      case InputManager.Actions.Menu:
        str = RewiredUnifyInput.Instance.QuitAction;
        break;
    }
    return str;
  }

  public override bool GetButtonDown(InputManager.Actions action)
  {
    string actionName = RewiredGamePad.ActionToString(action);
    return ReInput.players.GetPlayer(this.playerId).GetButtonDown(actionName);
  }

  public override bool GetButtonPressed(InputManager.Actions action)
  {
    if (this.AxisCheck(action))
      return true;
    string actionName = RewiredGamePad.ActionToString(action);
    return ReInput.players.GetPlayer(this.playerId).GetButton(actionName);
  }

  public override bool GetButtonUp(InputManager.Actions action)
  {
    string actionName = RewiredGamePad.ActionToString(action);
    return ReInput.players.GetPlayer(this.playerId).GetButtonUp(actionName);
  }

  public override bool IsConnected()
  {
    int num = 0;
    if (this.playerId < 0)
      return false;
    if (ReInput.players.GetPlayer(this.playerId) != null)
      num += ((ICollection<Joystick>) ReInput.players.GetPlayer(this.playerId).controllers.Joysticks).Count;
    if ((UnifyManager.platform == UnifyManager.Platform.Standalone || UnifyManager.platform == UnifyManager.Platform.None) && ReInput.players.GetPlayer(this.playerId).controllers.hasKeyboard)
      ++num;
    return num > 0;
  }

  public override void Vibrate(GamePad.VibrationType type, float motorLevel, float duration)
  {
    if (!this.vibrationEnabled)
      return;
    ReInput.players.GetPlayer(this.playerId)?.SetVibration(0, motorLevel, duration);
  }

  public override string ToString() => "GamePad.Rewired, id: " + this.joystickId.ToString();
}
