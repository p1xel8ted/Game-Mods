// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.PressAnyButtonToJoinExample_Assigner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Rewired.Demos;

[AddComponentMenu("")]
public class PressAnyButtonToJoinExample_Assigner : MonoBehaviour
{
  private void Update()
  {
    if (!ReInput.isReady)
      return;
    this.AssignJoysticksToPlayers();
  }

  private void AssignJoysticksToPlayers()
  {
    IList<Joystick> joysticks = (IList<Joystick>) ReInput.controllers.Joysticks;
    for (int index = 0; index < joysticks.Count; ++index)
    {
      Joystick joystick = joysticks[index];
      if (!ReInput.controllers.IsControllerAssigned(joystick.type, joystick.id) && joystick.GetAnyButtonDown())
        this.FindPlayerWithoutJoystick()?.controllers.AddController((Controller) joystick, false);
    }
    if (!this.DoAllPlayersHaveJoysticks())
      return;
    ReInput.configuration.autoAssignJoysticks = true;
    this.enabled = false;
  }

  private Player FindPlayerWithoutJoystick()
  {
    IList<Player> players = (IList<Player>) ReInput.players.Players;
    for (int index = 0; index < players.Count; ++index)
    {
      if (players[index].controllers.joystickCount <= 0)
        return players[index];
    }
    return (Player) null;
  }

  private bool DoAllPlayersHaveJoysticks() => this.FindPlayerWithoutJoystick() == null;
}
