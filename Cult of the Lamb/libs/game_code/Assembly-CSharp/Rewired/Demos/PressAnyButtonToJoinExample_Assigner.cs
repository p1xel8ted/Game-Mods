// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.PressAnyButtonToJoinExample_Assigner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Rewired.Demos;

[AddComponentMenu("")]
public class PressAnyButtonToJoinExample_Assigner : MonoBehaviour
{
  public void Update()
  {
    if (!ReInput.isReady)
      return;
    this.AssignJoysticksToPlayers();
  }

  public void AssignJoysticksToPlayers()
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

  public Player FindPlayerWithoutJoystick()
  {
    IList<Player> players = (IList<Player>) ReInput.players.Players;
    for (int index = 0; index < players.Count; ++index)
    {
      if (players[index].controllers.joystickCount <= 0)
        return players[index];
    }
    return (Player) null;
  }

  public bool DoAllPlayersHaveJoysticks() => this.FindPlayerWithoutJoystick() == null;
}
