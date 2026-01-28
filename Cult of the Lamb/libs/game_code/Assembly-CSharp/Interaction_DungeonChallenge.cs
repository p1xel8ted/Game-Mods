// Decompiled with JetBrains decompiler
// Type: Interaction_DungeonChallenge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MMBiomeGeneration;
using MMTools;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_DungeonChallenge : Interaction
{
  [SerializeField]
  public SkeletonAnimation Spine;
  [SerializeField]
  public bool giveReward;
  public ObjectivesData completedObjective;
  public string sLabel = "";
  public bool rewardGiven;

  public void Start()
  {
    if (this.giveReward)
      this.AutomaticallyInteract = true;
    bool flag = false;
    foreach (ObjectivesData completedObjective in DataManager.Instance.CompletedObjectives)
    {
      if (completedObjective is Objectives_RoomChallenge)
      {
        flag = true;
        this.completedObjective = completedObjective;
        break;
      }
    }
    if (this.giveReward && !flag)
      this.gameObject.SetActive(false);
    this.sLabel = ScriptLocalization.Interactions.DungeonChallenge;
    this.StartCoroutine((IEnumerator) this.SetAnimation());
  }

  public IEnumerator SetAnimation()
  {
    while ((UnityEngine.Object) this.Spine == (UnityEngine.Object) null || this.Spine.AnimationState == null)
      yield return (object) null;
    this.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
  }

  public override void GetLabel() => this.Label = this.Interactable ? this.sLabel : "";

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.Interactable = false;
    if (this.giveReward)
      this.StartCoroutine((IEnumerator) this.GiveRewardIE());
    else
      this.StartCoroutine((IEnumerator) this.GiveChallengeIE());
  }

  public IEnumerator GiveRewardIE()
  {
    Interaction_DungeonChallenge dungeonChallenge = this;
    dungeonChallenge.playerFarming.GoToAndStop(dungeonChallenge.transform.position + Vector3.left * 2f);
    while (dungeonChallenge.playerFarming.GoToAndStopping)
      yield return (object) null;
    List<ConversationEntry> Entries = new List<ConversationEntry>();
    Entries.Add(new ConversationEntry(dungeonChallenge.gameObject, "Conversation_NPC/Challenge/Reward/Line1"));
    Entries.Add(new ConversationEntry(dungeonChallenge.gameObject, "Conversation_NPC/Challenge/Reward/Line2"));
    Entries.Add(new ConversationEntry(dungeonChallenge.gameObject, "Conversation_NPC/Challenge/Reward/Line3"));
    Entries[0].CharacterName = "NAMES/Ratoo";
    Entries[0].Offset = new Vector3(0.0f, 2f, 0.0f);
    Entries[1].CharacterName = "NAMES/Ratoo";
    Entries[1].Offset = new Vector3(0.0f, 2f, 0.0f);
    Entries[2].CharacterName = "NAMES/Ratoo";
    Entries[2].Offset = new Vector3(0.0f, 2f, 0.0f);
    Entries[0].soundPath = "event:/dialogue/ratoo/standard_ratoo";
    Entries[1].soundPath = "event:/dialogue/ratoo/standard_ratoo";
    Entries[2].soundPath = "event:/dialogue/ratoo/standard_ratoo";
    dungeonChallenge.playerFarming.state.facingAngle = Utils.GetAngle(dungeonChallenge.playerFarming.transform.position, dungeonChallenge.transform.position);
    dungeonChallenge.playerFarming.state.LookAngle = Utils.GetAngle(dungeonChallenge.playerFarming.transform.position, dungeonChallenge.transform.position);
    MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) null));
    MMConversation.mmConversation.SpeechBubble.ScreenOffset = 200f;
    while (MMConversation.CURRENT_CONVERSATION != null)
      yield return (object) null;
    int connectionType1 = (int) BiomeGenerator.Instance.CurrentRoom.N_Room.ConnectionType;
    int connectionType2 = (int) BiomeGenerator.Instance.CurrentRoom.S_Room.ConnectionType;
    int connectionType3 = (int) BiomeGenerator.Instance.CurrentRoom.W_Room.ConnectionType;
    int connectionType4 = (int) BiomeGenerator.Instance.CurrentRoom.E_Room.ConnectionType;
    InventoryItem.ITEM_TYPE type = InventoryItem.ITEM_TYPE.BLACK_GOLD;
    while (type == InventoryItem.ITEM_TYPE.BLACK_GOLD || type == InventoryItem.ITEM_TYPE.GOLD_NUGGET)
      type = RewardsItem.Instance.ReturnItemType(RewardsItem.Instance.GetGoodReward(false, true));
    if (!DataManager.TrinketUnlocked(TarotCards.Card.DecreaseRelicCharge) && DataManager.Instance.OnboardedRelics)
    {
      GameManager.GetInstance().OnConversationNew();
      TarotCustomTarget tarotCustomTarget = TarotCustomTarget.Create(dungeonChallenge.transform.position, dungeonChallenge.playerFarming.transform.position, 1f, TarotCards.Card.DecreaseRelicCharge, (System.Action) (() => GameManager.GetInstance().OnConversationEnd()));
      GameManager.GetInstance().OnConversationNext(tarotCustomTarget.gameObject);
    }
    else
      InventoryItem.Spawn(type, 1, dungeonChallenge.transform.position + Vector3.back, 0.0f).SetInitialSpeedAndDiraction(5f, 250f);
    dungeonChallenge.sLabel = "";
    ObjectiveManager.ObjectiveRemoved(dungeonChallenge.completedObjective);
    dungeonChallenge.rewardGiven = true;
    dungeonChallenge.Spine.AnimationState.SetAnimation(0, "exit", false);
    yield return (object) new WaitForSeconds(2f);
    if (dungeonChallenge.giveReward)
      UnityEngine.Object.Destroy((UnityEngine.Object) dungeonChallenge.gameObject);
  }

  public IEnumerator GiveChallengeIE()
  {
    Interaction_DungeonChallenge dungeonChallenge1 = this;
    dungeonChallenge1.playerFarming.GoToAndStop(dungeonChallenge1.transform.position + Vector3.left * 2f);
    while (dungeonChallenge1.playerFarming.GoToAndStopping)
      yield return (object) null;
    Health.team2.Clear();
    ObjectivesData dungeonChallenge2 = Quests.GetRandomDungeonChallenge();
    ObjectiveManager.Add(dungeonChallenge2);
    string TermToSpeak = "";
    if (dungeonChallenge2.Type == Objectives.TYPES.NO_DAMAGE)
      TermToSpeak = "Conversation_NPC/Challenge/Line2_NoDamage";
    else if (dungeonChallenge2.Type == Objectives.TYPES.NO_CURSES)
      TermToSpeak = "Conversation_NPC/Challenge/Line2_NoCurses";
    else if (dungeonChallenge2.Type == Objectives.TYPES.NO_DODGE)
      TermToSpeak = "Conversation_NPC/Challenge/Line2_NoDodging";
    List<ConversationEntry> Entries = new List<ConversationEntry>();
    Entries.Add(new ConversationEntry(dungeonChallenge1.gameObject, "Conversation_NPC/Challenge/Line1"));
    Entries.Add(new ConversationEntry(dungeonChallenge1.gameObject, TermToSpeak));
    Entries.Add(new ConversationEntry(dungeonChallenge1.gameObject, "Conversation_NPC/Challenge/Line3"));
    Entries[0].CharacterName = "NAMES/Ratoo";
    Entries[0].Offset = new Vector3(0.0f, 2f, 0.0f);
    Entries[1].CharacterName = "NAMES/Ratoo";
    Entries[1].Offset = new Vector3(0.0f, 2f, 0.0f);
    Entries[2].CharacterName = "NAMES/Ratoo";
    Entries[2].Offset = new Vector3(0.0f, 2f, 0.0f);
    Entries[0].soundPath = "event:/dialogue/ratoo/standard_ratoo";
    Entries[1].soundPath = "event:/dialogue/ratoo/standard_ratoo";
    Entries[2].soundPath = "event:/dialogue/ratoo/standard_ratoo";
    dungeonChallenge1.playerFarming.state.facingAngle = Utils.GetAngle(dungeonChallenge1.playerFarming.transform.position, dungeonChallenge1.transform.position);
    dungeonChallenge1.playerFarming.state.LookAngle = Utils.GetAngle(dungeonChallenge1.playerFarming.transform.position, dungeonChallenge1.transform.position);
    MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) null));
    MMConversation.mmConversation.SpeechBubble.ScreenOffset = 200f;
    while (MMConversation.CURRENT_CONVERSATION != null)
      yield return (object) null;
  }

  public override void OnDisable()
  {
    base.OnDisable();
    if (!this.giveReward || !this.rewardGiven)
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }
}
