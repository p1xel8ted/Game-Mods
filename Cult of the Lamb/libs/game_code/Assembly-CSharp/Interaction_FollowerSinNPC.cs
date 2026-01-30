// Decompiled with JetBrains decompiler
// Type: Interaction_FollowerSinNPC
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
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
public class Interaction_FollowerSinNPC : Interaction
{
  [SerializeField]
  public Follower followerPrefab;
  [SerializeField]
  public SkeletonAnimation stripper;
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

  public override void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position + Vector3.left * 2f, 0.5f, Color.green);
  }

  public IEnumerator FirstChatIE()
  {
    Interaction_FollowerSinNPC interactionFollowerSinNpc = this;
    List<ConversationEntry> c = new List<ConversationEntry>()
    {
      new ConversationEntry(interactionFollowerSinNpc.gameObject, "Conversation_NPC/SinFollowerNPC/Line1"),
      new ConversationEntry(interactionFollowerSinNpc.gameObject, "Conversation_NPC/SinFollowerNPC/Line2")
    };
    c[0].CharacterName = LocalizationManager.GetTranslation("NAMES/SinNPC");
    c[0].Offset = new Vector3(0.0f, 2f, 0.0f);
    c[0].soundPath = "event:/dialogue/moth/moth";
    c[1].CharacterName = LocalizationManager.GetTranslation("NAMES/SinNPC");
    c[1].Offset = new Vector3(0.0f, 2f, 0.0f);
    c[1].soundPath = "event:/dialogue/moth/moth";
    interactionFollowerSinNpc.playerFarming.GoToAndStop(interactionFollowerSinNpc.transform.position + Vector3.left * 2f);
    while (interactionFollowerSinNpc.playerFarming.GoToAndStopping)
      yield return (object) null;
    interactionFollowerSinNpc.playerFarming.state.facingAngle = Utils.GetAngle(interactionFollowerSinNpc.playerFarming.transform.position, interactionFollowerSinNpc.transform.position);
    interactionFollowerSinNpc.playerFarming.state.LookAngle = Utils.GetAngle(interactionFollowerSinNpc.playerFarming.transform.position, interactionFollowerSinNpc.transform.position);
    MMConversation.Play(new ConversationObject(c, (List<MMTools.Response>) null, (System.Action) (() => { })));
    MMConversation.mmConversation.SpeechBubble.ScreenOffset = 200f;
    interactionFollowerSinNpc.NPCSpine.AnimationState.SetAnimation(0, "talk", false);
    interactionFollowerSinNpc.NPCSpine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    while (MMConversation.CURRENT_CONVERSATION != null)
      yield return (object) null;
    interactionFollowerSinNpc.Activated = false;
    interactionFollowerSinNpc.firstInteraction = false;
    interactionFollowerSinNpc.GetLabel();
    interactionFollowerSinNpc.HasChanged = true;
    interactionFollowerSinNpc.OnInteract(interactionFollowerSinNpc.state);
  }

  public IEnumerator InteractIE()
  {
    Interaction_FollowerSinNPC interactionFollowerSinNpc = this;
    interactionFollowerSinNpc.playerFarming.GoToAndStop(interactionFollowerSinNpc.transform.position + Vector3.left * 2f);
    while (interactionFollowerSinNpc.playerFarming.GoToAndStopping)
      yield return (object) null;
    interactionFollowerSinNpc.playerFarming.state.facingAngle = Utils.GetAngle(interactionFollowerSinNpc.playerFarming.transform.position, interactionFollowerSinNpc.transform.position);
    interactionFollowerSinNpc.playerFarming.state.LookAngle = Utils.GetAngle(interactionFollowerSinNpc.playerFarming.transform.position, interactionFollowerSinNpc.transform.position);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionFollowerSinNpc.gameObject, 6f);
    yield return (object) new WaitForSeconds(0.5f);
    List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
    foreach (SimFollower simFollower in FollowerManager.SimFollowersAtLocation(FollowerLocation.Base))
    {
      if (simFollower.Brain._directInfoAccess.IsSnowman || simFollower.Brain.CurrentTask is FollowerTask_MissionaryComplete)
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
        followerInfoBox.ShowPleasure(FollowerBrain.PleasureActions.SinNPC);
    });
    UIFollowerSelectMenuController selectMenuController2 = followerSelectInstance;
    selectMenuController2.OnShownCompleted = selectMenuController2.OnShownCompleted + (System.Action) (() =>
    {
      foreach (FollowerInformationBox followerInfoBox in followerSelectInstance.FollowerInfoBoxes)
        followerInfoBox.ShowPleasure(FollowerBrain.PleasureActions.SinNPC);
    });
    UIFollowerSelectMenuController selectMenuController3 = followerSelectInstance;
    selectMenuController3.OnFollowerSelected = selectMenuController3.OnFollowerSelected + (System.Action<FollowerInfo>) (followerInfo =>
    {
      FollowerManager.SpawnedFollower spawnedFollower = FollowerManager.SpawnCopyFollower(followerInfo, this.transform.position + Vector3.left * 1f, this.transform.parent, PlayerFarming.Location);
      Follower follower = spawnedFollower.Follower;
      follower.Init(spawnedFollower.FollowerBrain, new FollowerOutfit(followerInfo));
      follower.Brain.CheckChangeState();
      follower.gameObject.SetActive(true);
      follower.Interaction_FollowerInteraction.enabled = false;
      follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
      follower.ShowAllFollowerIcons();
      follower.State.LookAngle = Utils.GetAngle(follower.transform.position, this.transform.position);
      follower.State.facingAngle = Utils.GetAngle(follower.transform.position, this.transform.position);
      this.NPCSpine.AnimationState.SetAnimation(0, "buy-agree", false);
      this.NPCSpine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
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
    Interaction_FollowerSinNPC interactionFollowerSinNpc = this;
    float increment = 0.0285714287f;
    for (int i = 0; i < 35; ++i)
    {
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, interactionFollowerSinNpc.transform.position);
      yield return (object) new WaitForSeconds(increment);
    }
  }

  public IEnumerator GiveFollowerReward(
    FollowerManager.SpawnedFollower spawnedFollower,
    Follower follower)
  {
    Interaction_FollowerSinNPC interactionFollowerSinNpc = this;
    GameManager.GetInstance().OnConversationNext(follower.gameObject, 4f);
    yield return (object) new WaitForSeconds(1f);
    FollowerBrain.SetFollowerCostume(follower.Spine.Skeleton, follower.Brain.Info.XPLevel, follower.Brain.Info.SkinName, follower.Brain.Info.SkinColour, FollowerOutfitType.Naked, follower.Brain.Info.Hat, FollowerClothingType.Naked, FollowerCustomisationType.None, follower.Brain.Info.Special, follower.Brain.Info.Necklace, follower.Brain.Info.ClothingVariant, follower.Brain._directInfoAccess);
    BiomeConstants.Instance.EmitSmokeExplosionVFX(follower.transform.position);
    yield return (object) new WaitForSeconds(0.5f);
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    follower.GetComponentInChildren<BarControllerNonUI>(true).SetBarSize(follower.Brain.Stats.Adoration / follower.Brain.Stats.MAX_ADORATION, false, true);
    Spine.AnimationState animationState1 = spawnedFollower.Follower.Spine.AnimationState;
    int num = UnityEngine.Random.Range(1, 3);
    string animationName1 = "Sin/sin-dance-start" + num.ToString();
    animationState1.SetAnimation(1, animationName1, false);
    Spine.AnimationState animationState2 = spawnedFollower.Follower.Spine.AnimationState;
    num = UnityEngine.Random.Range(1, 3);
    string animationName2 = "Sin/sin-dance" + num.ToString();
    animationState2.AddAnimation(1, animationName2, false, 0.0f);
    yield return (object) new WaitForSeconds(3f);
    spawnedFollower.FollowerBrain.AddPleasure(FollowerBrain.PleasureActions.SinNPC);
    AudioManager.Instance.PlayOneShot("event:/followers/gain_loyalty", interactionFollowerSinNpc.gameObject.transform.position);
    if (!spawnedFollower.FollowerBrain.CanGiveSin())
      yield return (object) new WaitForSeconds(1.5333333f);
    while (spawnedFollower.Follower.InGiveSinSequence && (UnityEngine.Object) follower != (UnityEngine.Object) null)
      yield return (object) null;
    AudioManager.Instance.PlayOneShot("event:/Stings/generic_positive", interactionFollowerSinNpc.gameObject);
    string convoResult = "Conversation_NPC/SinFollowerNPC/Success_Line1";
    if ((UnityEngine.Object) follower != (UnityEngine.Object) null)
    {
      follower.Spine.AnimationState.AddAnimation(0, "Reactions/react-happy1", false, 0.0f);
      follower.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
      yield return (object) new WaitForSeconds(1.5f);
    }
    GameManager.GetInstance().OnConversationNext(interactionFollowerSinNpc.gameObject, 6f);
    yield return (object) new WaitForSeconds(0.5f);
    if ((UnityEngine.Object) follower != (UnityEngine.Object) null)
    {
      follower.SimpleAnimator.enabled = false;
      yield return (object) new WaitForEndOfFrame();
      follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
      follower.Spine.AnimationState.ClearTracks();
      follower.Spine.AnimationState.SetAnimation(0, "spawn-out", false);
      follower.HideAllFollowerIcons();
    }
    MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
    {
      new ConversationEntry(interactionFollowerSinNpc.gameObject, convoResult)
    }, (List<MMTools.Response>) null, (System.Action) null));
    MMConversation.mmConversation.SpeechBubble.ScreenOffset = 200f;
    interactionFollowerSinNpc.NPCSpine.AnimationState.SetAnimation(0, "talk", false);
    interactionFollowerSinNpc.NPCSpine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    yield return (object) new WaitForSeconds(1f);
    if ((UnityEngine.Object) follower != (UnityEngine.Object) null)
    {
      if (follower.Brain.CurrentTask.Type == FollowerTaskType.ManualControl)
        follower.Brain.CompleteCurrentTask();
      follower.gameObject.SetActive(false);
    }
    FollowerManager.CleanUpCopyFollower(spawnedFollower);
  }

  public void GiveOutfit()
  {
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(this.stripper.gameObject, 7f);
    AudioManager.Instance.PlayOneShot("event:/dlc/env/sin_room/stelle_item_give");
    FollowerOutfitCustomTarget.Create(this.stripper.transform.position, this.playerFarming.transform.position, 2f, FollowerClothingType.Normal_MajorDLC_2, (System.Action) (() => GameManager.GetInstance().OnConversationEnd()));
  }
}
