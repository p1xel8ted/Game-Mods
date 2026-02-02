// Decompiled with JetBrains decompiler
// Type: DungeonMissionaryFollower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMBiomeGeneration;
using MMTools;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class DungeonMissionaryFollower : MonoBehaviour
{
  public DungeonMissionaryFollower.HiddenType Type;
  [CompilerGenerated]
  public bool \u003CIsActivated\u003Ek__BackingField;

  public bool IsActivated
  {
    get => this.\u003CIsActivated\u003Ek__BackingField;
    set => this.\u003CIsActivated\u003Ek__BackingField = value;
  }

  public void OnDisable()
  {
    if (!this.IsActivated)
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public void Update()
  {
    if (this.Type != DungeonMissionaryFollower.HiddenType.HiddenInBush || this.IsActivated || !BiomeGenerator.Instance.CurrentRoom.Completed)
      return;
    this.PlayHiddenInBushSequence();
  }

  public void PlayHiddenInBushSequence()
  {
    this.IsActivated = true;
    this.StartCoroutine((IEnumerator) this.HiddenInBushSequence());
  }

  public IEnumerator HiddenInBushSequence()
  {
    DungeonMissionaryFollower missionaryFollower = this;
    GameManager.GetInstance().OnConversationNew();
    Follower follower = missionaryFollower.GetComponent<Follower>();
    PlayerFarming playerFarming = PlayerFarming.Instance;
    GameManager.GetInstance().OnConversationNext(follower.gameObject);
    yield return (object) new WaitForSeconds(3.83333325f);
    playerFarming.state.facingAngle = playerFarming.state.LookAngle = Utils.GetAngle(playerFarming.transform.position, missionaryFollower.transform.position);
    follower.FacePosition(playerFarming.transform.position);
    string str;
    if ((double) UnityEngine.Random.value < 0.5)
    {
      double num = (double) follower.SetBodyAnimation("idle", true);
      follower.Brain.CurrentState = (FollowerState) new FollowerState_Default();
      str = "Normal";
    }
    else
    {
      double num = (double) follower.SetBodyAnimation("Injured/idle", true);
      follower.Brain.CurrentState = (FollowerState) new FollowerState_Injured();
      str = "Injured";
    }
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, follower.Brain.CurrentState.OverrideIdleAnim);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, follower.Brain.CurrentState.OverrideWalkAnim == null ? "run" : follower.Brain.CurrentState.OverrideWalkAnim);
    List<ConversationEntry> Entries = new List<ConversationEntry>()
    {
      new ConversationEntry(follower.gameObject, $"Conversation_NPC/MissionaryHidden/{str}/0"),
      new ConversationEntry(follower.gameObject, $"Conversation_NPC/MissionaryHidden/{str}/1")
    };
    foreach (ConversationEntry conversationEntry in Entries)
    {
      conversationEntry.CharacterName = follower.Brain.Info.Name;
      conversationEntry.Speaker = follower.gameObject;
    }
    MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) null), false);
    yield return (object) null;
    while (MMConversation.isPlaying)
      yield return (object) null;
    GameManager.GetInstance().OnConversationEnd();
    yield return (object) new WaitForSeconds(1f);
    missionaryFollower.GoToRandomEntrance();
  }

  public void GoToRandomEntrance()
  {
    UnitObject component = this.GetComponent<UnitObject>();
    this.GetComponent<CircleCollider2D>().enabled = false;
    component.state.CURRENT_STATE = StateMachine.State.Idle;
    Door door = Door.Doors[UnityEngine.Random.Range(0, Door.Doors.Count)];
    component.givePath(door.PlayerPosition.position, forceAStar: true);
    this.StartCoroutine((IEnumerator) this.WaitForEndOfPath(component.state));
  }

  public IEnumerator WaitForEndOfPath(StateMachine state)
  {
    DungeonMissionaryFollower missionaryFollower = this;
    yield return (object) null;
    while (state.CURRENT_STATE == StateMachine.State.Moving)
      yield return (object) null;
    UnityEngine.Object.Destroy((UnityEngine.Object) missionaryFollower.gameObject);
  }

  public enum HiddenType
  {
    HiddenInBush,
    HiddenInChest,
  }
}
