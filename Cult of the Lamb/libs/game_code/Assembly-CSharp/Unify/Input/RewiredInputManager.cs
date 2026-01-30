// Decompiled with JetBrains decompiler
// Type: Unify.Input.RewiredInputManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Rewired;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Unify.Input;

public class RewiredInputManager : InputManager
{
  public static int[] RewiredPlayerAssignments = new int[8]
  {
    -1,
    -1,
    -1,
    -1,
    -1,
    -1,
    -1,
    -1
  };

  static RewiredInputManager()
  {
    UserHelper.OnPlayerGamePadChanged += new UserHelper.PlayerGamePadChangedDelegate(RewiredInputManager.OnPlayerGamePadChanged);
    UserHelper.OnPlayerUserChanged += new UserHelper.PlayerUserChangedDelegate(RewiredInputManager.OnPlayerUserChanged);
  }

  public static Player MainPlayer => RewiredInputManager.GetPlayer(0);

  public static Player GetPlayer(int playerNo) => ReInput.players.GetPlayer(playerNo);

  public static void OnPlayerUserChanged(int playerNo, User was, User now)
  {
    if (now == null)
      RewiredInputManager.RewiredPlayerAssignments[playerNo] = -1;
    RewiredInputManager.PrintPlayerAssignements();
  }

  public static void OnPlayerGamePadChanged(int playerNo, User user)
  {
    RewiredInputManager.RewiredPlayerAssignments[playerNo] = RewiredGamePad.JoystickToPlayer(user.gamePadId.joystickId);
    RewiredInputManager.PrintPlayerAssignements();
  }

  public static void PrintPlayerAssignements()
  {
    string message = "--- Rewired Player Assignments ---\n";
    for (int index = 0; index < RewiredInputManager.RewiredPlayerAssignments.Length; ++index)
      message += $"Player {index} = {RewiredInputManager.RewiredPlayerAssignments[index]} Rewired\n";
    Debug.Log((object) message);
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
        if (!RewiredInputManager.RewiredPlayerAssignments.Contains<int>(player.id) && player.GetButtonDown(action))
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
        if (!RewiredInputManager.RewiredPlayerAssignments.Contains<int>(player.id) && player.GetAnyButtonDown())
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
