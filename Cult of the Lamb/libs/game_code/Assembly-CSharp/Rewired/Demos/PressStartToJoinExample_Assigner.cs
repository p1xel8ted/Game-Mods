// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.PressStartToJoinExample_Assigner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Rewired.Demos;

[AddComponentMenu("")]
public class PressStartToJoinExample_Assigner : MonoBehaviour
{
  public static PressStartToJoinExample_Assigner instance;
  public int maxPlayers = 4;
  public List<PressStartToJoinExample_Assigner.PlayerMap> playerMap;
  public int gamePlayerIdCounter;

  public static Player GetRewiredPlayer(int gamePlayerId)
  {
    if (!ReInput.isReady)
      return (Player) null;
    if ((Object) PressStartToJoinExample_Assigner.instance == (Object) null)
    {
      Debug.LogError((object) "Not initialized. Do you have a PressStartToJoinPlayerSelector in your scehe?");
      return (Player) null;
    }
    for (int index = 0; index < PressStartToJoinExample_Assigner.instance.playerMap.Count; ++index)
    {
      if (PressStartToJoinExample_Assigner.instance.playerMap[index].gamePlayerId == gamePlayerId)
        return ReInput.players.GetPlayer(PressStartToJoinExample_Assigner.instance.playerMap[index].rewiredPlayerId);
    }
    return (Player) null;
  }

  public void Awake()
  {
    this.playerMap = new List<PressStartToJoinExample_Assigner.PlayerMap>();
    PressStartToJoinExample_Assigner.instance = this;
  }

  public void Update()
  {
    for (int index = 0; index < ReInput.players.playerCount; ++index)
    {
      if (ReInput.players.GetPlayer(index).GetButtonDown("JoinGame"))
        this.AssignNextPlayer(index);
    }
  }

  public void AssignNextPlayer(int rewiredPlayerId)
  {
    if (this.playerMap.Count >= this.maxPlayers)
    {
      Debug.LogError((object) "Max player limit already reached!");
    }
    else
    {
      int nextGamePlayerId = this.GetNextGamePlayerId();
      this.playerMap.Add(new PressStartToJoinExample_Assigner.PlayerMap(rewiredPlayerId, nextGamePlayerId));
      Player player = ReInput.players.GetPlayer(rewiredPlayerId);
      player.controllers.maps.SetMapsEnabled(false, "Assignment");
      player.controllers.maps.SetMapsEnabled(true, "Default");
      Debug.Log((object) $"Added Rewired Player id {rewiredPlayerId.ToString()} to game player {nextGamePlayerId.ToString()}");
    }
  }

  public int GetNextGamePlayerId() => this.gamePlayerIdCounter++;

  public class PlayerMap
  {
    public int rewiredPlayerId;
    public int gamePlayerId;

    public PlayerMap(int rewiredPlayerId, int gamePlayerId)
    {
      this.rewiredPlayerId = rewiredPlayerId;
      this.gamePlayerId = gamePlayerId;
    }
  }
}
