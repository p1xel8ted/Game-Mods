// Decompiled with JetBrains decompiler
// Type: Unify.Input.RewiredInputManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Rewired;
using System.Collections.Generic;

#nullable disable
namespace Unify.Input;

public class RewiredInputManager : InputManager
{
  public static Player MainPlayer
  {
    get
    {
      if (!InputManager.inputEnabled)
        return (Player) null;
      GamePad playerGamePad = UserHelper.GetPlayerGamePad(0);
      int playerId = playerGamePad == GamePad.None ? -1 : ((RewiredGamePad) playerGamePad).PlayerId;
      return playerId < 0 ? (Player) null : ReInput.players.GetPlayer(playerId);
    }
  }

  public override GamePad GetGamePad(int joystickId)
  {
    return (GamePad) new RewiredGamePad(RewiredGamePad.JoystickToPlayer(joystickId));
  }

  public override GamePad GetGamePadWithAction(string action)
  {
    if (ReInput.isReady)
    {
      foreach (Player player in (IEnumerable<Player>) ReInput.players.GetPlayers())
      {
        if (player.GetButtonDown(action))
          return (GamePad) new RewiredGamePad(player.id);
      }
    }
    return GamePad.None;
  }

  public override GamePad GetGamePadWithAnyAction()
  {
    if (ReInput.isReady)
    {
      foreach (Player player in (IEnumerable<Player>) ReInput.players.GetPlayers())
      {
        if (player.GetAnyButtonDown())
          return (GamePad) new RewiredGamePad(player.id);
      }
    }
    return GamePad.None;
  }

  public static Player MainPlayerRaw
  {
    get
    {
      GamePad playerGamePad = UserHelper.GetPlayerGamePad(0);
      int playerId = playerGamePad == GamePad.None ? -1 : ((RewiredGamePad) playerGamePad).PlayerId;
      return playerId < 0 ? (Player) null : ReInput.players.GetPlayer(playerId);
    }
  }
}
