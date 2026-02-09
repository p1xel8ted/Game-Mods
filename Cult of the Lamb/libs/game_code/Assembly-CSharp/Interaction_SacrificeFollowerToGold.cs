// Decompiled with JetBrains decompiler
// Type: Interaction_SacrificeFollowerToGold
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using MMTools;
using Spine;
using Spine.Unity;
using src.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_SacrificeFollowerToGold : Interaction
{
  public const int CHOSEN_CHILD_MEDITATION_DAYS = 3;
  public List<Interaction_SimpleConversation> StatueConversations = new List<Interaction_SimpleConversation>();
  public SkeletonAnimation Spine;
  public List<GameObject> Statues = new List<GameObject>();
  [SerializeField]
  public Transform chosenChildPosition;
  public FollowerManager.SpawnedFollower spawnedChosenChild;
  public string sLabel;
  public string sLeaveChosenChildQuest;
  public string sBringBackChosenChildQuest;
  public Interaction_KeyPiece KeyPiecePrefab;

  public bool canLeaveChild
  {
    get
    {
      return !DataManager.Instance.ChosenChildLeftInTheMidasCave && ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.LegendarySword) && FollowerInfo.GetInfoByID(100000) != null;
    }
  }

  public bool canBringBackChild
  {
    get
    {
      return TimeManager.CurrentDay - DataManager.Instance.ChosenChildMeditationQuestDay >= 3 && DataManager.Instance.ChosenChildMeditationQuestDay > 0 && DataManager.Instance.ChosenChildLeftInTheMidasCave;
    }
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if (this.spawnedChosenChild == null)
      return;
    FollowerManager.CleanUpCopyFollower(this.spawnedChosenChild);
  }

  public IEnumerator Start()
  {
    Interaction_SacrificeFollowerToGold sacrificeFollowerToGold = this;
    sacrificeFollowerToGold.UpdateLocalisation();
    int index = -1;
    while (++index < sacrificeFollowerToGold.Statues.Count)
      sacrificeFollowerToGold.Statues[index].SetActive(index < DataManager.Instance.MidasFollowerStatueCount);
    sacrificeFollowerToGold.Spine.gameObject.SetActive(false);
    while (PlayerFarming.Location != FollowerLocation.Dungeon_Location_3)
      yield return (object) null;
    if (DataManager.Instance.ChosenChildLeftInTheMidasCave && sacrificeFollowerToGold.spawnedChosenChild == null)
    {
      FollowerBrain brainById = FollowerBrain.FindBrainByID(100000);
      if (brainById.CurrentTaskType != FollowerTaskType.LeftInTheDungeon)
        brainById.HardSwapToTask((FollowerTask) new FollowerTask_LeftInTheDungeon());
      FollowerInfo infoById = FollowerInfo.GetInfoByID(100000);
      sacrificeFollowerToGold.spawnedChosenChild = sacrificeFollowerToGold.SpawnFollower(infoById, sacrificeFollowerToGold.chosenChildPosition.position);
      FollowerManager.FollowersAtLocation(FollowerLocation.LeftInTheDungeon).Add(sacrificeFollowerToGold.spawnedChosenChild.Follower);
    }
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sLabel = ScriptLocalization.Interactions.SacrificeFollower;
    this.sLeaveChosenChildQuest = LocalizationManager.GetTranslation("Interactions/LeaveChosenChild");
    this.sBringBackChosenChildQuest = LocalizationManager.GetTranslation("Interactions/CollectChosenChild");
  }

  public override void GetLabel()
  {
    if (this.canLeaveChild)
      this.Label = this.sLeaveChosenChildQuest;
    else if (this.canBringBackChild)
      this.Label = this.sBringBackChosenChildQuest;
    else if (DataManager.Instance.MidasFollowerStatueCount < 4)
      this.Label = this.sLabel;
    else
      this.Label = "";
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.canLeaveChild)
    {
      DataManager.Instance.ChosenChildLeftInTheMidasCave = true;
      DataManager.Instance.ChosenChildMeditationQuestDay = TimeManager.CurrentDay;
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.LegendarySword, 100000);
      Objectives_LegendarySwordReturn objective = new Objectives_LegendarySwordReturn("Objectives/GroupTitles/Quest", FollowerInfo.GetInfoByID(100000).Name);
      objective.FailLocked = true;
      ObjectiveManager.Add((ObjectivesData) objective, true, true);
      this.StartCoroutine((IEnumerator) this.SpawnChosenChild());
    }
    else if (this.canBringBackChild)
    {
      AudioManager.Instance.PlayOneShot("event:/dlc/env/chosenchild/collect");
      this.PlayEndMeditationConvo();
    }
    else if (FollowerBrain.AllBrains.Count > 0)
    {
      base.OnInteract(state);
      this.playerFarming.GoToAndStop(this.transform.position + new Vector3(-1f, -2.5f), this.gameObject, GoToCallback: (System.Action) (() => this.StartCoroutine((IEnumerator) this.SacrificeFollowerRoutine())));
    }
    else
      this.playerFarming.indicator.PlayShake();
  }

  public IEnumerator SacrificeFollowerRoutine()
  {
    Interaction_SacrificeFollowerToGold sacrificeFollowerToGold = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(sacrificeFollowerToGold.state.gameObject);
    yield return (object) new WaitForSeconds(0.5f);
    List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
      followerSelectEntries.Add(new FollowerSelectEntry(follower, FollowerManager.GetFollowerAvailabilityStatus(follower)));
    UIFollowerSelectMenuController followerSelectInstance = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
    followerSelectInstance.VotingType = TwitchVoting.VotingType.SACRIFICE_TO_MIDAS;
    followerSelectInstance.Show(followerSelectEntries, false, UpgradeSystem.Type.Count, true, true, true, true, true);
    UIFollowerSelectMenuController selectMenuController1 = followerSelectInstance;
    selectMenuController1.OnFollowerSelected = selectMenuController1.OnFollowerSelected + (System.Action<FollowerInfo>) (followerInfo =>
    {
      AudioManager.Instance.PlayOneShot("event:/ritual_sacrifice/select_follower", this.playerFarming.gameObject);
      this.StartCoroutine((IEnumerator) this.SpawnFollower(followerInfo.ID));
    });
    UIFollowerSelectMenuController selectMenuController2 = followerSelectInstance;
    selectMenuController2.OnHidden = selectMenuController2.OnHidden + (System.Action) (() => followerSelectInstance = (UIFollowerSelectMenuController) null);
    UIFollowerSelectMenuController selectMenuController3 = followerSelectInstance;
    selectMenuController3.OnCancel = selectMenuController3.OnCancel + (System.Action) (() => GameManager.GetInstance().OnConversationEnd());
  }

  public IEnumerator SpawnFollower(int ID)
  {
    Interaction_SacrificeFollowerToGold sacrificeFollowerToGold = this;
    while (MMConversation.isPlaying)
      yield return (object) null;
    AudioManager.Instance.SetMusicRoomID(1, "drum_layer");
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(sacrificeFollowerToGold.state.gameObject);
    sacrificeFollowerToGold.Spine.gameObject.SetActive(true);
    while (sacrificeFollowerToGold.Spine.AnimationState == null)
      yield return (object) null;
    sacrificeFollowerToGold.Spine.AnimationState.SetAnimation(0, "enter", false);
    sacrificeFollowerToGold.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    yield return (object) new WaitForSeconds(1f);
    FollowerManager.SpawnedFollower spawnedFollower = FollowerManager.SpawnCopyFollower(FollowerManager.FindFollowerInfo(ID), sacrificeFollowerToGold.transform.position, sacrificeFollowerToGold.transform.parent, PlayerFarming.Location);
    spawnedFollower.Follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    spawnedFollower.Follower.Spine.AnimationState.SetAnimation(1, "spawn-in", false);
    spawnedFollower.Follower.Spine.AnimationState.AddAnimation(1, "Reactions/react-scared-long", false, 0.0f);
    spawnedFollower.Follower.Spine.AnimationState.AddAnimation(1, "idle-sad", true, 0.0f);
    FollowerManager.RemoveFollowerBrain(ID);
    ObjectiveManager.FailUniqueFollowerObjectives(ID);
    GameManager.GetInstance().OnConversationNext(spawnedFollower.Follower.gameObject);
    yield return (object) new WaitForSeconds(2f);
    sacrificeFollowerToGold.Spine.AnimationState.SetAnimation(0, "warning", true);
    float Progress = 0.0f;
    float Duration = 3.75f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      GameManager.GetInstance().CameraSetTargetZoom(Mathf.Lerp(9f, 4f, Mathf.SmoothStep(0.0f, 1f, Progress / Duration)));
      CameraManager.shakeCamera((float) (0.30000001192092896 + 0.5 * ((double) Progress / (double) Duration)));
      yield return (object) null;
    }
    CameraManager.instance.ShakeCameraForDuration(0.5f, 0.7f, 0.3f);
    FollowerManager.CleanUpCopyFollower(spawnedFollower);
    AudioManager.Instance.PlayOneShot("event:/followers/turn_to_gold_sequence", sacrificeFollowerToGold.transform.position);
    GameObject s = sacrificeFollowerToGold.Statues[DataManager.Instance.MidasFollowerStatueCount];
    s.SetActive(true);
    s.transform.localScale = Vector3.one * 1.2f;
    s.transform.DOScale(Vector3.one, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    Vector3 TargetPosition = s.transform.position;
    s.transform.position = sacrificeFollowerToGold.transform.position;
    BiomeConstants.Instance.EmitSmokeInteractionVFX(s.transform.position, new Vector3(2f, 2f, 1f));
    sacrificeFollowerToGold.Spine.AnimationState.SetAnimation(0, "idle", true);
    GameManager.GetInstance().OnConversationNext(s, 5f);
    GameManager.GetInstance().HitStop();
    yield return (object) new WaitForSeconds(2f);
    GameManager.GetInstance().OnConversationNext(s, 8f);
    sacrificeFollowerToGold.Spine.AnimationState.SetAnimation(0, "warning", true);
    s.transform.DOMove(sacrificeFollowerToGold.transform.position + Vector3.back, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    yield return (object) new WaitForSeconds(0.5f);
    s.transform.DOMove(TargetPosition + Vector3.back, 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    yield return (object) new WaitForSeconds(1.5f);
    s.transform.DOMove(TargetPosition, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      BiomeConstants.Instance.EmitSmokeInteractionVFX(s.transform.position, new Vector3(2f, 2f, 1f));
      CameraManager.instance.ShakeCameraForDuration(0.5f, 0.7f, 0.3f);
      this.Spine.AnimationState.SetAnimation(0, "idle", true);
    }));
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().OnConversationEnd();
    sacrificeFollowerToGold.StatueConversations[DataManager.Instance.MidasFollowerStatueCount].gameObject.SetActive(true);
    sacrificeFollowerToGold.StatueConversations[DataManager.Instance.MidasFollowerStatueCount].Callback.AddListener((UnityAction) (() =>
    {
      this.HasChanged = true;
      ++DataManager.Instance.MidasFollowerStatueCount;
      this.enabled = true;
      this.StartCoroutine((IEnumerator) this.GiveKeyPieceRoutine());
    }));
    sacrificeFollowerToGold.enabled = false;
    yield return (object) new WaitForEndOfFrame();
    HUD_Manager.Instance.Hide(true);
  }

  public IEnumerator GiveKeyPieceRoutine()
  {
    Interaction_SacrificeFollowerToGold sacrificeFollowerToGold = this;
    while (MMConversation.isPlaying)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNew(true, true, sacrificeFollowerToGold.playerFarming);
    Interaction_KeyPiece KeyPiece = UnityEngine.Object.Instantiate<Interaction_KeyPiece>(sacrificeFollowerToGold.KeyPiecePrefab, sacrificeFollowerToGold.Spine.transform.position + Vector3.back * 0.75f, Quaternion.identity, sacrificeFollowerToGold.transform.parent);
    KeyPiece.transform.localScale = Vector3.zero;
    KeyPiece.transform.DOScale(Vector3.one, 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    GameManager.GetInstance().OnConversationNext(KeyPiece.gameObject, 6f);
    yield return (object) new WaitForSeconds(0.5f);
    yield return (object) new WaitForSeconds(1f);
    KeyPiece.transform.DOMove(sacrificeFollowerToGold.playerFarming.transform.position + Vector3.back * 0.5f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
    yield return (object) new WaitForSeconds(1f);
    yield return (object) null;
    TrackEntry trackEntry = sacrificeFollowerToGold.Spine.AnimationState.SetAnimation(0, "exit", false);
    GameManager.GetInstance().OnConversationEnd(false);
    KeyPiece.OnInteract(sacrificeFollowerToGold.playerFarming.state);
    yield return (object) new WaitForSpineAnimationComplete(trackEntry);
    sacrificeFollowerToGold.Spine.gameObject.SetActive(false);
  }

  public IEnumerator SpawnChosenChild()
  {
    yield return (object) new WaitForEndOfFrame();
    FollowerBrain brainById = FollowerBrain.FindBrainByID(100000);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(this.chosenChildPosition.gameObject);
    AudioManager.Instance.PlayOneShot("event:/Stings/refuse_kneel_sting");
    DataManager.Instance.Followers_LeftInTheDungeon_IDs.Add(brainById.Info.ID);
    brainById.HardSwapToTask((FollowerTask) new FollowerTask_LeftInTheDungeon());
    BiomeConstants.Instance.EmitSmokeExplosionVFX(this.chosenChildPosition.position);
    BiomeConstants.Instance.ShakeCamera();
    CameraManager.instance.ShakeCameraForDuration(0.0f, 1f, 0.5f);
    this.spawnedChosenChild = this.SpawnFollower(brainById._directInfoAccess, this.chosenChildPosition.position);
    FollowerManager.FollowersAtLocation(FollowerLocation.LeftInTheDungeon).Add(this.spawnedChosenChild.Follower);
    GameManager.GetInstance().OnConversationNext(this.chosenChildPosition.gameObject, 5f);
    yield return (object) new WaitForSeconds(1.5f);
    this.PlayStartMeditationConvo();
  }

  public FollowerManager.SpawnedFollower SpawnFollower(FollowerInfo followerInfo, Vector3 position)
  {
    FollowerManager.SpawnedFollower spawnedFollower = FollowerManager.SpawnCopyFollower(FollowerManager.FollowerPrefab, followerInfo, position, this.transform.parent, PlayerFarming.Location);
    spawnedFollower.Follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    spawnedFollower.Follower.Spine.AnimationState.SetAnimation(1, "spawn-in", false);
    spawnedFollower.Follower.Spine.AnimationState.AddAnimation(1, "meditate-enlightenment", true, 0.0f);
    return spawnedFollower;
  }

  public void PlayStartMeditationConvo()
  {
    this.spawnedChosenChild.Follower.FacePosition(this.playerFarming.transform.position);
    string str = "Conversation_NPC/ChosenChild/LegendarySword/StartMeditation/";
    List<ConversationEntry> Entries = new List<ConversationEntry>()
    {
      new ConversationEntry(this.spawnedChosenChild.Follower.gameObject, str + "0"),
      new ConversationEntry(this.spawnedChosenChild.Follower.gameObject, str + "1")
    };
    foreach (ConversationEntry conversationEntry in Entries)
    {
      conversationEntry.CharacterName = this.spawnedChosenChild.FollowerBrain.Info.Name;
      conversationEntry.Animation = "worship/talk";
    }
    MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) null));
  }

  public void PlayEndMeditationConvo()
  {
    string str = "Conversation_NPC/ChosenChild/LegendarySword/EndMeditation/";
    List<ConversationEntry> Entries = new List<ConversationEntry>()
    {
      new ConversationEntry(this.spawnedChosenChild.Follower.gameObject, str + "0"),
      new ConversationEntry(this.spawnedChosenChild.Follower.gameObject, str + "1")
    };
    foreach (ConversationEntry conversationEntry in Entries)
    {
      conversationEntry.CharacterName = this.spawnedChosenChild.FollowerBrain.Info.Name;
      conversationEntry.Animation = "worship/talk";
    }
    MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) null), false, SetPlayerIdleOnComplete: false);
    DataManager.Instance.ChosenChildLeftInTheMidasCave = false;
    this.StartCoroutine((IEnumerator) this.GiveLegendaryFragmentSequence());
  }

  public IEnumerator GiveLegendaryFragmentSequence()
  {
    Interaction_SacrificeFollowerToGold sacrificeFollowerToGold = this;
    while (MMConversation.isPlaying)
      yield return (object) null;
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", sacrificeFollowerToGold.transform.position);
    HUD_Manager.Instance.Hide(true);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(sacrificeFollowerToGold.chosenChildPosition.gameObject);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    PlayerSimpleInventory component = PlayerFarming.Instance.GetComponent<PlayerSimpleInventory>();
    Vector3 EndPosition = new Vector3(component.ItemImage.transform.position.x, component.ItemImage.transform.position.y, -1f);
    bool isMoving = true;
    PermanentHeart_CustomTarget.Create(sacrificeFollowerToGold.chosenChildPosition.position, EndPosition, 2f, (System.Action<Interaction_PermanentHeart>) (heart => heart.OnFinishPickUp += (System.Action) (() => isMoving = false)));
    while (isMoving)
      yield return (object) null;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.InActive;
    yield return (object) new WaitForSeconds(1f);
    ObjectiveManager.CompleteLegendarySwordReturnObjective();
    sacrificeFollowerToGold.StartCoroutine((IEnumerator) sacrificeFollowerToGold.TeleportOutSequence());
  }

  public IEnumerator TeleportOutSequence()
  {
    Interaction_SacrificeFollowerToGold sacrificeFollowerToGold = this;
    DataManager.Instance.Followers_LeftInTheDungeon_IDs.Remove(sacrificeFollowerToGold.spawnedChosenChild.Follower.Brain.Info.ID);
    DataManager.Instance.ChosenChildLeftInTheMidasCave = false;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(sacrificeFollowerToGold.spawnedChosenChild.Follower.gameObject);
    List<ConversationEntry> Entries = new List<ConversationEntry>()
    {
      new ConversationEntry(sacrificeFollowerToGold.spawnedChosenChild.Follower.gameObject, "Conversation_NPC/ChosenChild/ByeForever/0"),
      new ConversationEntry(sacrificeFollowerToGold.spawnedChosenChild.Follower.gameObject, "Conversation_NPC/ChosenChild/ByeForever/1")
    };
    foreach (ConversationEntry conversationEntry in Entries)
    {
      conversationEntry.CharacterName = sacrificeFollowerToGold.spawnedChosenChild.FollowerBrain.Info.Name;
      conversationEntry.Animation = "worship/talk";
    }
    MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) null), false, SetPlayerIdleOnComplete: false);
    yield return (object) new WaitForEndOfFrame();
    while (MMConversation.isPlaying)
      yield return (object) null;
    bool waiting = true;
    FollowerSkinCustomTarget.Create(sacrificeFollowerToGold.spawnedChosenChild.Follower.transform.position, sacrificeFollowerToGold.playerFarming.transform.position, (Transform) null, 2f, "ChosenChild", (System.Action) (() => waiting = false));
    AudioManager.Instance.PlayOneShot("event:/dlc/env/chosenchild/item_appear");
    yield return (object) new WaitForSeconds(1.5f);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(sacrificeFollowerToGold.playerFarming.gameObject);
    while (waiting)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(sacrificeFollowerToGold.spawnedChosenChild.Follower.gameObject);
    TrackEntry trackEntry = sacrificeFollowerToGold.spawnedChosenChild.Follower.Spine.AnimationState.SetAnimation(1, "ascend", false);
    AudioManager.Instance.PlayOneShot("event:/dlc/env/chosenchild/ascension", sacrificeFollowerToGold.spawnedChosenChild.Follower.transform.position);
    yield return (object) new WaitForSeconds(trackEntry.Animation.Duration);
    sacrificeFollowerToGold.spawnedChosenChild.Follower.gameObject.SetActive(false);
    FollowerManager.RemoveFollower(FollowerBrain.FindBrainByID(100000).Info.ID);
    FollowerManager.CleanUpCopyFollower(sacrificeFollowerToGold.spawnedChosenChild);
    GameManager.GetInstance().OnConversationEnd();
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__17_0()
  {
    this.StartCoroutine((IEnumerator) this.SacrificeFollowerRoutine());
  }
}
