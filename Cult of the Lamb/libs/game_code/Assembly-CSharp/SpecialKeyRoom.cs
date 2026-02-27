// Decompiled with JetBrains decompiler
// Type: SpecialKeyRoom
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using MMTools;
using src.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class SpecialKeyRoom : Interaction
{
  [SerializeField]
  public GameObject ratoo;
  [SerializeField]
  public GameObject spawnPosition;
  [SerializeField]
  public GameObject playerPosition;
  [TermsPopup("")]
  [SerializeField]
  public string characterName;
  [TermsPopup("")]
  [SerializeField]
  public string responseSacrifice;
  [TermsPopup("")]
  [SerializeField]
  public string responseIgnore;
  [Space]
  [SerializeField]
  public Interaction_KeyPiece keyPiecePrefab;

  public override void GetLabel()
  {
    base.GetLabel();
    this.Label = "T";
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.Interactable = false;
    this.playerFarming.GoToAndStop(this.playerPosition, GoToCallback: (System.Action) (() =>
    {
      string line = this.GetLine();
      string TermToSpeak1 = line + "1";
      string TermToSpeak2 = line + "2";
      List<ConversationEntry> Entries = new List<ConversationEntry>();
      Entries.Add(new ConversationEntry(this.gameObject, TermToSpeak1));
      Entries.Add(new ConversationEntry(this.gameObject, TermToSpeak2));
      string str1 = "Conversation_NPC/Ratoo/KeyRoom/AnswerA";
      string str2 = "Conversation_NPC/Ratoo/KeyRoom/AnswerB";
      List<MMTools.Response> Responses = new List<MMTools.Response>()
      {
        new MMTools.Response(str1, (System.Action) (() => this.StartCoroutine(this.SacrificeFollowerIE())), str1),
        new MMTools.Response(str2, (System.Action) (() => this.StartCoroutine(this.IgnoreIE())), str2)
      };
      Entries[0].CharacterName = this.characterName;
      Entries[1].CharacterName = this.characterName;
      MMConversation.Play(new ConversationObject(Entries, Responses, (System.Action) null), false);
    }));
  }

  public string GetLine()
  {
    switch (PlayerFarming.Location)
    {
      case FollowerLocation.Dungeon1_1:
        return "Conversation_NPC/Ratoo/KeyRoom/Encounter1/";
      case FollowerLocation.Dungeon1_2:
        return "Conversation_NPC/Ratoo/KeyRoom/Encounter2/";
      case FollowerLocation.Dungeon1_3:
        return "Conversation_NPC/Ratoo/KeyRoom/Encounter3/";
      case FollowerLocation.Dungeon1_4:
        return "Conversation_NPC/Ratoo/KeyRoom/Encounter4/";
      default:
        return "";
    }
  }

  public IEnumerator SacrificeFollowerIE()
  {
    SpecialKeyRoom specialKeyRoom = this;
    yield return (object) new WaitForEndOfFrame();
    GameManager.GetInstance().OnConversationNew();
    FollowerInfo selectedFollower = (FollowerInfo) null;
    List<FollowerBrain> allBrains = FollowerBrain.AllBrains;
    if (allBrains.Count > 0)
    {
      List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
      foreach (FollowerBrain followerBrain in allBrains)
        followerSelectEntries.Add(new FollowerSelectEntry(followerBrain, FollowerManager.GetFollowerAvailabilityStatus(followerBrain)));
      UIFollowerSelectMenuController selectMenuController = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
      selectMenuController.VotingType = TwitchVoting.VotingType.RITUAL_SACRIFICE;
      selectMenuController.Show(followerSelectEntries, false, UpgradeSystem.Type.Count, true, true, true, false, true);
      selectMenuController.OnFollowerSelected = selectMenuController.OnFollowerSelected + (System.Action<FollowerInfo>) (f => selectedFollower = f);
      while (selectedFollower == null)
        yield return (object) null;
      FollowerManager.SpawnedFollower spawnedFollower = FollowerManager.SpawnCopyFollower(selectedFollower, specialKeyRoom.spawnPosition.transform.position, specialKeyRoom.transform, PlayerFarming.Location);
      spawnedFollower.Follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
      double num = (double) spawnedFollower.Follower.SetBodyAnimation("fox-spawn", false);
      spawnedFollower.Follower.AddBodyAnimation("fox-sacrifice", false, 0.0f);
      GameManager.GetInstance().OnConversationNext(spawnedFollower.Follower.gameObject, 6f);
      yield return (object) new WaitForSeconds(3f);
      FollowerManager.CleanUpCopyFollower(spawnedFollower);
      spawnedFollower.FollowerBrain.Die(NotificationCentre.NotificationType.SacrificedAwayFromCult);
      MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
      {
        new ConversationEntry(specialKeyRoom.gameObject, specialKeyRoom.responseSacrifice)
      }, (List<MMTools.Response>) null, (System.Action) null), false);
      while (MMConversation.isPlaying)
        yield return (object) null;
      Interaction_KeyPiece KeyPiece = UnityEngine.Object.Instantiate<Interaction_KeyPiece>(specialKeyRoom.keyPiecePrefab, specialKeyRoom.ratoo.transform.position + new Vector3(0.0f, -0.2f, -1.25f), Quaternion.identity, specialKeyRoom.transform.parent);
      KeyPiece.transform.localScale = Vector3.zero;
      bool waiting = true;
      KeyPiece.transform.DOScale(Vector3.one, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => KeyPiece.transform.DOMove(this.playerFarming.transform.position + Vector3.back * 0.5f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
      {
        GameManager.GetInstance().OnConversationEnd(false);
        KeyPiece.OnInteract(this.playerFarming.state);
        waiting = false;
      }))));
      while (waiting)
        yield return (object) null;
      specialKeyRoom.SetCompleteVariable();
      yield return (object) new WaitForSeconds(4f);
      spawnedFollower = (FollowerManager.SpawnedFollower) null;
    }
    else
    {
      MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
      {
        new ConversationEntry(specialKeyRoom.gameObject, "Conversation_NPC/Ratoo/KeyRoom/NoFollowers")
      }, (List<MMTools.Response>) null, (System.Action) null), false);
      while (MMConversation.isPlaying)
        yield return (object) null;
    }
    GameManager.GetInstance().OnConversationEnd();
  }

  public IEnumerator IgnoreIE()
  {
    SpecialKeyRoom specialKeyRoom = this;
    MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
    {
      new ConversationEntry(specialKeyRoom.gameObject, specialKeyRoom.responseIgnore)
    }, (List<MMTools.Response>) null, (System.Action) null), false);
    while (MMConversation.isPlaying)
      yield return (object) null;
    GameManager.GetInstance().OnConversationEnd();
  }

  public void SetCompleteVariable()
  {
    switch (PlayerFarming.Location)
    {
      case FollowerLocation.Dungeon1_1:
        DataManager.Instance.SetVariable(DataManager.Variables.DungeonKeyRoomCompleted1, true);
        break;
      case FollowerLocation.Dungeon1_2:
        DataManager.Instance.SetVariable(DataManager.Variables.DungeonKeyRoomCompleted2, true);
        break;
      case FollowerLocation.Dungeon1_3:
        DataManager.Instance.SetVariable(DataManager.Variables.DungeonKeyRoomCompleted3, true);
        break;
      case FollowerLocation.Dungeon1_4:
        DataManager.Instance.SetVariable(DataManager.Variables.DungeonKeyRoomCompleted4, true);
        break;
    }
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__8_0()
  {
    string line = this.GetLine();
    string TermToSpeak1 = line + "1";
    string TermToSpeak2 = line + "2";
    List<ConversationEntry> Entries = new List<ConversationEntry>();
    Entries.Add(new ConversationEntry(this.gameObject, TermToSpeak1));
    Entries.Add(new ConversationEntry(this.gameObject, TermToSpeak2));
    string str1 = "Conversation_NPC/Ratoo/KeyRoom/AnswerA";
    string str2 = "Conversation_NPC/Ratoo/KeyRoom/AnswerB";
    List<MMTools.Response> Responses = new List<MMTools.Response>()
    {
      new MMTools.Response(str1, (System.Action) (() => this.StartCoroutine(this.SacrificeFollowerIE())), str1),
      new MMTools.Response(str2, (System.Action) (() => this.StartCoroutine(this.IgnoreIE())), str2)
    };
    Entries[0].CharacterName = this.characterName;
    Entries[1].CharacterName = this.characterName;
    MMConversation.Play(new ConversationObject(Entries, Responses, (System.Action) null), false);
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__8_1() => this.StartCoroutine(this.SacrificeFollowerIE());

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__8_2() => this.StartCoroutine(this.IgnoreIE());
}
