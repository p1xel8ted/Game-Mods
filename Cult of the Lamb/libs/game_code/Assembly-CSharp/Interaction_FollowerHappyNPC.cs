// Decompiled with JetBrains decompiler
// Type: Interaction_FollowerHappyNPC
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using MMTools;
using Spine.Unity;
using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_FollowerHappyNPC : Interaction
{
  [SerializeField]
  public Follower followerPrefab;
  public SkeletonAnimation NPCSpine;
  public const int cost = 35;
  public bool firstInteraction = true;
  public List<SimFollower> simFollowers;
  public bool Activated;

  public override void GetLabel()
  {
    if (this.simFollowers == null)
      this.simFollowers = FollowerManager.SimFollowersAtLocation(FollowerLocation.Base);
    if (this.Activated)
    {
      this.Label = "";
    }
    else
    {
      string str = "";
      if (!this.Activated && (this.firstInteraction || this.simFollowers.Count > 0))
      {
        this.Interactable = true;
        str = !this.firstInteraction ? ScriptLocalization.FollowerInteractions.MakeDemand : ScriptLocalization.Interactions.Talk;
      }
      else if (this.simFollowers.Count <= 0)
      {
        str = ScriptLocalization.Interactions.NoFollowers;
        this.Interactable = false;
      }
      this.Label = str;
    }
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (this.firstInteraction)
    {
      this.StartCoroutine((IEnumerator) this.FirstChatIE());
      this.Activated = true;
    }
    else
    {
      this.StartCoroutine((IEnumerator) this.InteractIE());
      this.Activated = true;
    }
  }

  public IEnumerator FirstChatIE()
  {
    Interaction_FollowerHappyNPC followerHappyNpc = this;
    List<ConversationEntry> Entries = new List<ConversationEntry>();
    Entries.Add(new ConversationEntry(followerHappyNpc.gameObject, "Conversation_NPC/HappyFollowerNPC/Line1"));
    Entries.Add(new ConversationEntry(followerHappyNpc.gameObject, "Conversation_NPC/HappyFollowerNPC/Line2"));
    Entries[0].CharacterName = LocalizationManager.GetTranslation("NAMES/MothNPC");
    Entries[0].Offset = new Vector3(0.0f, 2f, 0.0f);
    Entries[0].soundPath = "event:/dialogue/moth/moth";
    Entries[1].CharacterName = LocalizationManager.GetTranslation("NAMES/MothNPC");
    Entries[1].Offset = new Vector3(0.0f, 2f, 0.0f);
    Entries[1].soundPath = "event:/dialogue/moth/moth";
    followerHappyNpc.playerFarming.state.facingAngle = Utils.GetAngle(followerHappyNpc.playerFarming.transform.position, followerHappyNpc.transform.position);
    followerHappyNpc.playerFarming.state.LookAngle = Utils.GetAngle(followerHappyNpc.playerFarming.transform.position, followerHappyNpc.transform.position);
    MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) (() => { })));
    MMConversation.mmConversation.SpeechBubble.ScreenOffset = 200f;
    followerHappyNpc.NPCSpine.AnimationState.SetAnimation(0, "talk1", false);
    followerHappyNpc.NPCSpine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    while (MMConversation.CURRENT_CONVERSATION != null)
      yield return (object) null;
    followerHappyNpc.Activated = false;
    followerHappyNpc.firstInteraction = false;
    followerHappyNpc.GetLabel();
    followerHappyNpc.HasChanged = true;
    followerHappyNpc.OnInteract(followerHappyNpc.state);
  }

  public IEnumerator InteractIE()
  {
    Interaction_FollowerHappyNPC followerHappyNpc = this;
    followerHappyNpc.playerFarming.GoToAndStop(followerHappyNpc.transform.position + Vector3.left * 2f);
    while (followerHappyNpc.playerFarming.GoToAndStopping)
      yield return (object) null;
    followerHappyNpc.playerFarming.state.facingAngle = Utils.GetAngle(followerHappyNpc.playerFarming.transform.position, followerHappyNpc.transform.position);
    followerHappyNpc.playerFarming.state.LookAngle = Utils.GetAngle(followerHappyNpc.playerFarming.transform.position, followerHappyNpc.transform.position);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(followerHappyNpc.gameObject, 6f);
    yield return (object) new WaitForSeconds(0.5f);
    List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
    foreach (SimFollower simFollower in FollowerManager.SimFollowersAtLocation(FollowerLocation.Base))
    {
      if (simFollower.Brain._directInfoAccess.IsSnowman || simFollower.Brain.HasTrait(FollowerTrait.TraitType.Mutated))
        followerSelectEntries.Add(new FollowerSelectEntry(simFollower, FollowerSelectEntry.Status.Unavailable));
      else if (simFollower.Brain.Info.CursedState == Thought.Child)
        followerSelectEntries.Add(new FollowerSelectEntry(simFollower, FollowerSelectEntry.Status.UnavailableChild));
      else
        followerSelectEntries.Add(new FollowerSelectEntry(simFollower));
    }
    followerSelectEntries.Sort((Comparison<FollowerSelectEntry>) ((a, b) => b.FollowerInfo.XPLevel.CompareTo(a.FollowerInfo.XPLevel)));
    UIFollowerSelectMenuController followerSelectInstance = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
    followerSelectInstance.VotingType = TwitchVoting.VotingType.FOLLOWER_TO_GAIN_XP;
    followerSelectInstance.Show(followerSelectEntries, false, UpgradeSystem.Type.Count, true, true, true, false, true);
    UIFollowerSelectMenuController selectMenuController1 = followerSelectInstance;
    selectMenuController1.OnShow = selectMenuController1.OnShow + (System.Action) (() =>
    {
      foreach (FollowerInformationBox followerInfoBox in followerSelectInstance.FollowerInfoBoxes)
        followerInfoBox.ShowFaithGain(FollowerBrain.AdorationsAndActions[FollowerBrain.AdorationActions.HappyFollowerNPC], followerInfoBox.followBrain.Stats.MAX_ADORATION);
    });
    UIFollowerSelectMenuController selectMenuController2 = followerSelectInstance;
    selectMenuController2.OnShownCompleted = selectMenuController2.OnShownCompleted + (System.Action) (() =>
    {
      foreach (FollowerInformationBox followerInfoBox in followerSelectInstance.FollowerInfoBoxes)
        followerInfoBox.ShowFaithGain(FollowerBrain.AdorationsAndActions[FollowerBrain.AdorationActions.HappyFollowerNPC], followerInfoBox.followBrain.Stats.MAX_ADORATION);
    });
    UIFollowerSelectMenuController selectMenuController3 = followerSelectInstance;
    selectMenuController3.OnFollowerSelected = selectMenuController3.OnFollowerSelected + (System.Action<FollowerInfo>) (followerInfo =>
    {
      FollowerManager.SpawnedFollower spawnedFollower = FollowerManager.SpawnCopyFollower(followerInfo, this.transform.position + Vector3.right * 2f, this.transform.parent, PlayerFarming.Location);
      Follower follower = spawnedFollower.Follower;
      follower.Init(spawnedFollower.FollowerBrain, new FollowerOutfit(followerInfo));
      follower.Brain.CheckChangeState();
      follower.gameObject.SetActive(true);
      follower.Interaction_FollowerInteraction.enabled = false;
      follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
      follower.ShowAllFollowerIcons();
      follower.State.LookAngle = Utils.GetAngle(follower.transform.position, this.transform.position);
      follower.State.facingAngle = Utils.GetAngle(follower.transform.position, this.transform.position);
      this.NPCSpine.AnimationState.AddAnimation(0, "dance", true, 0.0f);
      AudioManager.Instance.PlayOneShot("event:/player/Curses/enemy_charmed", this.gameObject);
      follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
      follower.Spine.AnimationState.SetAnimation(0, "spawn-in", false);
      follower.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
      this.StartCoroutine((IEnumerator) this.GiveFollowerReward(spawnedFollower, follower));
    });
    UIFollowerSelectMenuController selectMenuController4 = followerSelectInstance;
    selectMenuController4.OnCancel = selectMenuController4.OnCancel + (System.Action) (() =>
    {
      GameManager.GetInstance().OnConversationEnd();
      this.Activated = false;
    });
    UIFollowerSelectMenuController selectMenuController5 = followerSelectInstance;
    selectMenuController5.OnHidden = selectMenuController5.OnHidden + (System.Action) (() => followerSelectInstance = (UIFollowerSelectMenuController) null);
    yield return (object) new WaitForEndOfFrame();
  }

  public IEnumerator RefundCoins()
  {
    Interaction_FollowerHappyNPC followerHappyNpc = this;
    float increment = 0.0285714287f;
    for (int i = 0; i < 35; ++i)
    {
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, followerHappyNpc.transform.position);
      yield return (object) new WaitForSeconds(increment);
    }
  }

  public IEnumerator GiveFollowerReward(
    FollowerManager.SpawnedFollower spawnedFollower,
    Follower follower)
  {
    Interaction_FollowerHappyNPC followerHappyNpc = this;
    GameManager.GetInstance().OnConversationNext(follower.gameObject, 4f);
    yield return (object) new WaitForSeconds(1f);
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    spawnedFollower.FollowerBrain.AddThought(Thought.HappyFromDungeonNPC);
    spawnedFollower.FollowerFakeBrain.AddThought(Thought.HappyFromDungeonNPC);
    follower.GetComponentInChildren<BarControllerNonUI>(true).SetBarSize(follower.Brain.Stats.Adoration / follower.Brain.Stats.MAX_ADORATION, false, true);
    bool waiting = true;
    spawnedFollower.FollowerFakeBrain.AddAdoration(spawnedFollower.Follower, FollowerBrain.AdorationActions.HappyFollowerNPC, (System.Action) (() => waiting = false));
    AudioManager.Instance.PlayOneShot("event:/followers/gain_loyalty", followerHappyNpc.gameObject.transform.position);
    while (waiting)
      yield return (object) null;
    AudioManager.Instance.PlayOneShot("event:/Stings/generic_positive", followerHappyNpc.gameObject);
    follower.Spine.AnimationState.AddAnimation(0, "Reactions/react-happy1", false, 0.0f);
    string convoResult = "Conversation_NPC/HappyFollowerNPC/Success_Line1";
    follower.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    yield return (object) new WaitForSeconds(1.5f);
    GameManager.GetInstance().OnConversationNext(followerHappyNpc.gameObject, 6f);
    yield return (object) new WaitForSeconds(0.5f);
    follower.SimpleAnimator.enabled = false;
    yield return (object) new WaitForEndOfFrame();
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    follower.Spine.AnimationState.ClearTracks();
    follower.Spine.AnimationState.SetAnimation(0, "spawn-out", false);
    follower.HideAllFollowerIcons();
    MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
    {
      new ConversationEntry(followerHappyNpc.gameObject, convoResult)
    }, (List<MMTools.Response>) null, (System.Action) null));
    MMConversation.mmConversation.SpeechBubble.ScreenOffset = 200f;
    followerHappyNpc.NPCSpine.AnimationState.SetAnimation(0, "talk2", false);
    followerHappyNpc.NPCSpine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    yield return (object) new WaitForSeconds(1f);
    if (follower.Brain.CurrentTask.Type == FollowerTaskType.ManualControl)
      follower.Brain.CompleteCurrentTask();
    follower.gameObject.SetActive(false);
    FollowerManager.CleanUpCopyFollower(spawnedFollower);
  }
}
