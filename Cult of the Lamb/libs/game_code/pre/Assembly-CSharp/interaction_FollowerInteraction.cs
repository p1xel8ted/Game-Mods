// Decompiled with JetBrains decompiler
// Type: interaction_FollowerInteraction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using Lamb.UI.FollowerInteractionWheel;
using MMTools;
using Spine.Unity.Examples;
using src.Extensions;
using src.UI.Overlays.TutorialOverlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class interaction_FollowerInteraction : Interaction
{
  private bool Activated;
  public Follower follower;
  private FollowerTask_ManualControl Task;
  private SimpleSpineAnimator CacheSpineAnimator;
  private string CacheAnimation;
  private float CacheAnimationProgress;
  private float CacheFacing;
  private bool CacheLoop;
  public FollowerAdorationUI AdorationOrb;
  public UIFollowerPrayingProgress FollowerPrayingProgress;
  private bool ShowDivineInspirationTutorialOnClose;
  private FollowerRecruit followerRecruit;
  private string generalTalkVO = "event:/dialogue/followers/general_talk";
  private string generalAcknowledgeVO = "event:/dialogue/followers/general_acknowledge";
  private string negativeAcknowledgeVO = "event:/dialogue/followers/negative_acknowledge";
  private string positiveAcknowledgeVO = "event:/dialogue/followers/positive_acknowledge";
  public string bowVO = "event:/followers/Speech/FollowerBow";
  public FollowerSpineEventListener eventListener;
  public RectTransform LayoutContent;
  private Thought transitioningCursedState;
  private string sSpeakTo;
  private string sColllectReward;
  private string sCompleteQuest;
  private FollowerTaskType previousTaskType;
  public System.Action OnGivenRewards;
  private bool GiveDoctrinePieceOnClose;
  private RendererMaterialSwap rendMatSwap;
  private Material prevMat;
  public Material followerUIMat;

  public override void IndicateHighlighted()
  {
    base.IndicateHighlighted();
    if (!this.follower.Brain.ThoughtExists(Thought.Dissenter) && !this.follower.WorshipperBubble.Active)
      this.AdorationOrb.Show();
    if (!(this.follower.Brain.CurrentTask is FollowerTask_Pray) && !(this.follower.Brain.CurrentTask is FollowerTask_PrayPassive))
      return;
    this.FollowerPrayingProgress.Hide();
  }

  public override void EndIndicateHighlighted()
  {
    base.EndIndicateHighlighted();
    this.AdorationOrb.Hide();
    this.GetComponentInChildren<UIFollowerTwitchName>()?.Show();
    if (!(this.follower.Brain.CurrentTask is FollowerTask_Pray) && !(this.follower.Brain.CurrentTask is FollowerTask_PrayPassive))
      return;
    this.FollowerPrayingProgress.Show();
  }

  public override void GetLabel()
  {
    if ((UnityEngine.Object) this.follower == (UnityEngine.Object) null)
      this.Label = "";
    else if (PlayerFarming.Location != FollowerLocation.Base || !this.Interactable)
      this.Label = "";
    else if (this.follower.Brain != null && DataManager.Instance.CompletedQuestFollowerIDs.Contains(this.follower.Brain.Info.ID))
      this.Label = this.sCompleteQuest;
    else if (this.follower.Brain != null && (double) this.follower.Brain.Stats.Adoration >= (double) this.follower.Brain.Stats.MAX_ADORATION)
    {
      this.Label = this.sColllectReward;
    }
    else
    {
      string str;
      if (!this.Interactable)
        str = "";
      else
        str = $"{this.sSpeakTo} <color=yellow>{this.follower.Brain.Info.Name}</color>{(this.follower.Brain.Info.XPLevel > 1 ? $" {ScriptLocalization.Interactions.Level} {this.follower.Brain.Info.XPLevel.ToNumeral()}" : "")}{(this.follower.Brain.Info.MarriedToLeader ? " <sprite name=\"icon_Married\">" : "")}";
      this.Label = str;
    }
  }

  private void Awake() => this.followerRecruit = this.GetComponent<FollowerRecruit>();

  public override void OnEnableInteraction() => base.OnEnableInteraction();

  public override void OnDisableInteraction() => base.OnDisableInteraction();

  private void Start()
  {
    this.ActivateDistance = 2f;
    this.UpdateLocalisation();
    this.HasSecondaryInteraction = false;
    this.follower.WorshipperBubble.OnBubblePlay += (System.Action) (() => this.AdorationOrb.Hide());
    this.follower.WorshipperBubble.OnBubbleHide += (System.Action) (() =>
    {
      if (!((UnityEngine.Object) Interactor.CurrentInteraction == (UnityEngine.Object) this) || this.follower.Brain.ThoughtExists(Thought.Dissenter))
        return;
      this.AdorationOrb.Show();
    });
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sSpeakTo = ScriptLocalization.Interactions.SpeakTo;
    this.sColllectReward = ScriptLocalization.Interactions.CollectDiscipleReward;
    this.sCompleteQuest = ScriptLocalization.Interactions.CompleteQuest;
  }

  protected override void Update()
  {
    base.Update();
    if (!((UnityEngine.Object) this.follower != (UnityEngine.Object) null) || this.follower.Brain == null)
      return;
    if (this.follower.Brain.CurrentTaskType == FollowerTaskType.GetPlayerAttention && this.follower.Brain.CurrentTask != null)
    {
      if ((this.follower.Brain.CurrentTask as FollowerTask_GetAttention).ComplaintType == Follower.ComplaintType.GiveOnboarding && (this.follower.Brain.CurrentTask as FollowerTask_GetAttention).AutoInteract)
        this.AutomaticallyInteract = true;
      else
        this.AutomaticallyInteract = false;
      this.PriorityWeight = 2f;
      this.ActivateDistance = 2f;
    }
    else if ((this.follower.Brain.CurrentTaskType == FollowerTaskType.Sleep || this.follower.Brain.CurrentTaskType == FollowerTaskType.SleepBedRest) && this.follower.Brain.CurrentTask != null && this.follower.Brain.CurrentTask.State == FollowerTaskState.Doing && this.follower.Brain.HasHome)
    {
      this.AutomaticallyInteract = false;
      this.PriorityWeight = 2f;
      this.ActivateDistance = 1.5f;
    }
    else
    {
      this.AutomaticallyInteract = false;
      this.PriorityWeight = 1f;
      this.ActivateDistance = 2f;
    }
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.Activated)
      return;
    base.OnInteract(state);
    Follower.ComplaintType complaintType = Follower.ComplaintType.None;
    FollowerTaskType taskType = this.follower.Brain.CurrentTaskType;
    this.AutomaticallyInteract = false;
    this.follower.WorshipperBubble.gameObject.SetActive(false);
    this.follower.CompletedQuestIcon.gameObject.SetActive(false);
    this.previousTaskType = this.follower.Brain.CurrentTask != null ? this.follower.Brain.CurrentTask.Type : FollowerTaskType.None;
    this.GetComponentInChildren<UIFollowerTwitchName>()?.Hide(false);
    GameManager.GetInstance().CamFollowTarget.SnappyMovement = true;
    BiomeConstants.Instance.DepthOfFieldTween(1.5f, 4.5f, 10f, 1f, 145f);
    List<CommandItem> commandItems = FollowerCommandGroups.DefaultCommands(this.follower);
    this.CacheAndSetFollower();
    this.HideOtherFollowers();
    List<ObjectivesData> quests = Quests.GetUnCompletedFollowerQuests(this.follower.Brain.Info.ID, "");
    Objectives_TalkToFollower talkToFollowerQuest = (Objectives_TalkToFollower) null;
    foreach (ObjectivesData objectivesData in quests)
    {
      if (objectivesData is Objectives_TalkToFollower && string.IsNullOrEmpty(((Objectives_TalkToFollower) objectivesData).ResponseTerm))
      {
        talkToFollowerQuest = objectivesData as Objectives_TalkToFollower;
        complaintType = Follower.ComplaintType.CompletedQuest;
        break;
      }
    }
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      if (objective is Objectives_TalkToFollower)
      {
        Objectives_TalkToFollower objectivesTalkToFollower = objective as Objectives_TalkToFollower;
        if (objectivesTalkToFollower.TargetFollower == this.follower.Brain.Info.ID)
        {
          this.CloseAndSpeak(objectivesTalkToFollower.ResponseTerm);
          objectivesTalkToFollower.Done = true;
          objectivesTalkToFollower.Complete();
          ObjectiveManager.CheckObjectives();
          return;
        }
      }
    }
    if (this.follower.Brain.Info.CursedState == Thought.OldAge && !DataManager.Instance.OldFollowerSpoken)
    {
      DataManager.Instance.OldFollowerSpoken = true;
      this.eventListener.PlayFollowerVO(this.negativeAcknowledgeVO);
      this.CloseAndSpeak("TooOldToWork", (System.Action) (() => this.OnInteract(state)), false);
      GameManager.GetInstance().CamFollowTarget.SnappyMovement = true;
    }
    else
    {
      complaintType = talkToFollowerQuest != null || this.follower.Brain.CurrentTask == null || this.follower.Brain.CurrentTaskType != FollowerTaskType.GetPlayerAttention ? complaintType : (this.follower.Brain.CurrentTask as FollowerTask_GetAttention).ComplaintType;
      PlayerFarming.Instance.GoToAndStop(this.follower.transform.position + new Vector3((float) (2.0 * ((double) this.follower.transform.position.x < (double) PlayerFarming.Instance.transform.position.x ? 1.0 : -1.0)), 0.0f), this.follower.gameObject, DisableCollider: true, GoToCallback: (System.Action) (() =>
      {
        SimulationManager.Pause();
        if ((double) this.follower.Brain.Stats.Adoration >= (double) this.follower.Brain.Stats.MAX_ADORATION)
          this.StartCoroutine((IEnumerator) this.GiveDiscipleRewardRoutine(this.previousTaskType, (System.Action) (() =>
          {
            if (complaintType != Follower.ComplaintType.CompletedQuest || talkToFollowerQuest == null)
              return;
            this.follower.ShowCompletedQuestIcon(true);
          })));
        else if (taskType == FollowerTaskType.GetPlayerAttention || talkToFollowerQuest != null)
        {
          if (complaintType == Follower.ComplaintType.GiveOnboarding)
          {
            quests = Onboarding.Instance.GetOnboardingQuests(this.follower.Brain.Info.ID);
            if (quests.Count == 0)
            {
              DataManager.Instance.CurrentOnboardingFollowerID = -1;
              DataManager.Instance.CurrentOnboardingFollowerTerm = "";
              if (ObjectiveManager.GetNumberOfObjectivesInGroup("Objectives/GroupTitles/Quest") < 1)
                complaintType = Follower.ComplaintType.GiveQuest;
            }
          }
          ObjectivesData objective = (ObjectivesData) null;
          if (complaintType == Follower.ComplaintType.GiveQuest)
          {
            List<int> ts = new List<int>();
            int targetFollowerID_1 = -1;
            int targetFollowerID_2 = -1;
            int deadFollowerID = -1;
            foreach (Follower follower in FollowerManager.FollowersAtLocation(FollowerLocation.Base))
            {
              if (follower.Brain.Info.ID != this.follower.Brain.Info.ID && !FollowerManager.FollowerLocked(follower.Brain.Info.ID) && follower.Brain.Info.CursedState == Thought.None && follower.Brain.Info.ID != 666)
                ts.Add(follower.Brain.Info.ID);
            }
            ts.Shuffle<int>();
            foreach (int num in ts)
            {
              if (targetFollowerID_1 == -1)
                targetFollowerID_1 = num;
              else if (targetFollowerID_2 == -1)
                targetFollowerID_2 = num;
            }
            ts.Clear();
            foreach (FollowerInfo followerInfo in DataManager.Instance.Followers_Dead)
            {
              if (!followerInfo.HadFuneral)
                ts.Add(followerInfo.ID);
            }
            ts.Shuffle<int>();
            foreach (int num in ts)
            {
              if (deadFollowerID == -1)
              {
                deadFollowerID = num;
                break;
              }
            }
            objective = Quests.GetRandomBaseQuest(this.follower.Brain.Info.ID, targetFollowerID_1, targetFollowerID_2, deadFollowerID);
          }
          else if (complaintType == Follower.ComplaintType.CompletedQuest && talkToFollowerQuest != null)
            objective = (ObjectivesData) talkToFollowerQuest;
          bool WillLevelUp = this.follower.Brain.GetWillLevelUp(FollowerBrain.AdorationActions.Quest) && complaintType == Follower.ComplaintType.CompletedQuest;
          Coroutine routine = (Coroutine) null;
          if (complaintType == Follower.ComplaintType.CompletedQuest)
          {
            foreach (ObjectivesData completedObjective in DataManager.Instance.CompletedObjectives)
            {
              if (completedObjective.Type == Objectives.TYPES.COLLECT_ITEM && completedObjective.GroupId == "Objectives/GroupTitles/Quest")
              {
                routine = this.StartCoroutine((IEnumerator) this.GiveItemsRoutine(((Objectives_CollectItem) completedObjective).ItemType, ((Objectives_CollectItem) completedObjective).Target));
                break;
              }
            }
          }
          GameManager.GetInstance().OnConversationNew();
          GameManager.GetInstance().OnConversationNext(this.follower.gameObject, 4f);
          this.StartCoroutine((IEnumerator) this.WaitForRoutine(routine, (System.Action) (() =>
          {
            this.follower.AdorationUI.Hide();
            this.follower.GetComponentInChildren<UIFollowerTwitchName>()?.Hide();
            MMConversation.OnConversationEnd += new MMConversation.ConversationEnd(this.OnConversationEnd);
            MMConversation.Play(new ConversationObject(this.GetConversationEntry(complaintType, objective), (List<MMTools.Response>) null, (System.Action) (() =>
            {
              string animation = "Reactions/react-bow";
              this.eventListener.PlayFollowerVO(this.bowVO);
              float timer = 1.86666667f;
              this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
              switch (complaintType)
              {
                case Follower.ComplaintType.CompletedQuest:
                  CultFaithManager.AddThought(Thought.Cult_CompleteQuest);
                  talkToFollowerQuest.Done = true;
                  ObjectiveManager.UpdateObjective((ObjectivesData) talkToFollowerQuest);
                  this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
                  this.follower.Brain.AddThought(Thought.LeaderDidQuest);
                  AudioManager.Instance.PlayOneShot("event:/followers/gain_loyalty", this.gameObject.transform.position);
                  TimeManager.TimeSinceLastQuest = 0.0f;
                  this.StartCoroutine((IEnumerator) this.FrameDelayCallback((System.Action) (() =>
                  {
                    GameManager.GetInstance().OnConversationNew();
                    GameManager.GetInstance().OnConversationNext(this.gameObject, 4f);
                    this.follower.Brain.AddAdoration(FollowerBrain.AdorationActions.Quest, (System.Action) (() =>
                    {
                      if ((double) this.follower.Brain.Stats.Adoration >= (double) this.follower.Brain.Stats.MAX_ADORATION)
                        this.StartCoroutine((IEnumerator) this.GiveDiscipleRewardRoutine(this.previousTaskType, GoToAndStop: false));
                      else
                        this.Close();
                    }));
                    DataManager.Instance.CompletedQuestFollowerIDs.Remove(this.follower.Brain.Info.ID);
                    this.follower.ShowCompletedQuestIcon(false);
                    if (this.previousTaskType != FollowerTaskType.Sleep)
                      return;
                    this.follower.Brain._directInfoAccess.WakeUpDay = -1;
                  })));
                  return;
                case Follower.ComplaintType.FailedQuest:
                  CultFaithManager.AddThought(Thought.Cult_FailQuest);
                  ObjectiveManager.ObjectiveRemoved(this.follower.Brain._directInfoAccess.CurrentPlayerQuest);
                  NotificationCentreScreen.Play(NotificationCentre.NotificationType.QuestFailed);
                  animation = "Reactions/react-sad";
                  this.eventListener.PlayFollowerVO(this.negativeAcknowledgeVO);
                  this.follower.Brain.AddThought(Thought.LeaderFailedQuest);
                  break;
                case Follower.ComplaintType.GiveOnboarding:
                  if (DataManager.Instance.CurrentOnboardingFollowerID == -1)
                    quests.Clear();
                  TimeManager.TimeSinceLastQuest = 0.0f;
                  animation = "Reactions/react-happy1";
                  switch (DataManager.Instance.CurrentOnboardingFollowerTerm)
                  {
                    case "Conversation_NPC/FollowerOnboarding/CleanUpBase":
                      IllnessBar.Instance?.Reveal();
                      if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Illness))
                      {
                        MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Illness);
                        break;
                      }
                      break;
                    case "Conversation_NPC/FollowerOnboarding/NameCult":
                      this.StartCoroutine((IEnumerator) this.FrameDelayCallback((System.Action) (() =>
                      {
                        GameManager.GetInstance().OnConversationNew();
                        GameManager.GetInstance().OnConversationNext(this.gameObject, 4f);
                        UICultNameMenuController cultNameMenuInstance = MonoSingleton<UIManager>.Instance.CultNameMenuTemplate.Instantiate<UICultNameMenuController>();
                        cultNameMenuInstance.Show(false, true);
                        cultNameMenuInstance.OnNameConfirmed += new System.Action<string>(this.NamedCult);
                        cultNameMenuInstance.OnHide += (System.Action) (() => { });
                        cultNameMenuInstance.OnHidden += (System.Action) (() => cultNameMenuInstance = (UICultNameMenuController) null);
                        DataManager.Instance.CurrentOnboardingFollowerID = -1;
                        DataManager.Instance.LastFollowerQuestGivenTime = TimeManager.TotalElapsedGameTime;
                      })));
                      return;
                    case "Conversation_NPC/FollowerOnboarding/SickFollower":
                      this.StartCoroutine((IEnumerator) this.FrameDelayCallback((System.Action) (() =>
                      {
                        IllnessBar.Instance?.Reveal();
                        if (!DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Illness))
                          return;
                        MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Illness);
                      })));
                      animation = "Sick/chunder";
                      break;
                    case "Conversation_NPC/FollowerOnboarding/CureDissenter":
                      this.StartCoroutine((IEnumerator) this.FrameDelayCallback((System.Action) (() =>
                      {
                        if (!DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Dissenter))
                          return;
                        MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Dissenter);
                      })));
                      animation = "tantrum-big";
                      timer = 6f;
                      break;
                    case "Conversation_NPC/FollowerOnboardOldAge":
                      quests.Clear();
                      this.follower.SetOutfit(FollowerOutfitType.Old, false);
                      break;
                    case "FollowerInteractions/GiveQuest/CrisisOfFaith":
                      animation = $"Conversations/react-hate{UnityEngine.Random.Range(1, 3)}";
                      timer = 2f;
                      break;
                  }
                  foreach (ObjectivesData objectivesData in quests)
                  {
                    objective = objectivesData;
                    ObjectiveManager.Add(objective, true);
                  }
                  DataManager.Instance.CurrentOnboardingFollowerID = -1;
                  DataManager.Instance.LastFollowerQuestGivenTime = TimeManager.TotalElapsedGameTime;
                  break;
                default:
                  if (complaintType == Follower.ComplaintType.GiveQuest && objective != null)
                  {
                    this.StartCoroutine((IEnumerator) this.AcceptQuestIE(objective));
                    return;
                  }
                  break;
              }
              this.Close(false);
              switch (DataManager.Instance.CurrentOnboardingFollowerTerm)
              {
                case "Conversation_NPC/FollowerOnboarding/SickFollower":
                  this.transitioningCursedState = Thought.Ill;
                  break;
                case "Conversation_NPC/FollowerOnboarding/CureDissenter":
                  this.transitioningCursedState = Thought.Dissenter;
                  break;
                case "Conversation_NPC/FollowerOnboardOldAge":
                  this.transitioningCursedState = Thought.OldAge;
                  break;
              }
              this.follower.TimedAnimation(animation, timer, (System.Action) (() =>
              {
                this.follower.Brain.CompleteCurrentTask();
                if (this.transitioningCursedState == Thought.Ill)
                {
                  this.follower.Brain.MakeSick();
                  StructuresData infoByType = StructuresData.GetInfoByType(StructureBrain.TYPES.VOMIT, 0);
                  infoByType.FollowerID = this.follower.Brain.Info.ID;
                  PlacementRegion.TileGridTile tileGridTile = (PlacementRegion.TileGridTile) null;
                  if ((bool) (UnityEngine.Object) PlacementRegion.Instance)
                    tileGridTile = StructureManager.GetClosestTileGridTileAtWorldPosition(this.transform.position, PlacementRegion.Instance.StructureInfo.Grid, 1f);
                  if (tileGridTile != null)
                  {
                    infoByType.GridTilePosition = tileGridTile.Position;
                    StructureManager.BuildStructure(FollowerLocation.Base, infoByType, tileGridTile.WorldPosition, Vector2Int.one, false);
                  }
                  else
                    StructureManager.BuildStructure(FollowerLocation.Base, infoByType, this.transform.position, Vector2Int.one, false);
                }
                else if (this.transitioningCursedState == Thought.Dissenter)
                  this.follower.Brain.MakeDissenter();
                else if (this.transitioningCursedState == Thought.OldAge)
                  this.follower.Brain.MakeOld();
                this.transitioningCursedState = Thought.None;
                DataManager.Instance.CurrentOnboardingFollowerTerm = "";
                this.ResetFollower();
              }));
            })), !WillLevelUp);
          })));
        }
        else
        {
          UnityEngine.Object.FindObjectOfType<CameraFollowTarget>().SetOffset(new Vector3(0.0f, 0.0f, -1f));
          UIFollowerInteractionWheelOverlayController overlayController = MonoSingleton<UIManager>.Instance.FollowerInteractionWheelTemplate.Instantiate<UIFollowerInteractionWheelOverlayController>();
          overlayController.Show(this.follower, commandItems);
          overlayController.OnItemChosen = overlayController.OnItemChosen + new System.Action<FollowerCommands[]>(this.OnFollowerCommandFinalized);
          overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() => { });
          overlayController.OnCancel = overlayController.OnCancel + new System.Action(this.Close);
          GameManager.GetInstance().OnConversationNew();
          GameManager.GetInstance().OnConversationNext(this.gameObject, 4f);
          HUD_Manager.Instance.Hide(false, 0);
        }
      }));
    }
  }

  protected override void OnDisable()
  {
    base.OnDisable();
    if (this.transitioningCursedState == Thought.None)
      return;
    if (this.transitioningCursedState == Thought.Ill)
      this.follower.Brain.MakeSick();
    else if (this.transitioningCursedState == Thought.Dissenter)
      this.follower.Brain.MakeDissenter();
    else if (this.transitioningCursedState == Thought.OldAge)
      this.follower.Brain.MakeOld();
    this.transitioningCursedState = Thought.None;
  }

  private IEnumerator WaitForRoutine(Coroutine routine, System.Action callback)
  {
    if (routine != null)
    {
      yield return (object) new WaitForSeconds(1f);
      yield return (object) routine;
    }
    System.Action action = callback;
    if (action != null)
      action();
  }

  private IEnumerator GiveItemsRoutine(InventoryItem.ITEM_TYPE itemType, int quantity)
  {
    interaction_FollowerInteraction followerInteraction = this;
    for (int i = 0; i < Mathf.Max(quantity, 10); ++i)
    {
      ResourceCustomTarget.Create(followerInteraction.gameObject, PlayerFarming.Instance.transform.position, itemType, (System.Action) null);
      yield return (object) new WaitForSeconds(0.025f);
    }
    Inventory.ChangeItemQuantity(itemType, -quantity);
  }

  private IEnumerator DelayedCurse(Thought cursedType, float delay)
  {
    yield return (object) new WaitForSeconds(delay);
    switch (cursedType)
    {
      case Thought.Dissenter:
        this.follower.Brain.MakeDissenter();
        break;
      case Thought.Ill:
        this.follower.Brain.MakeSick();
        break;
    }
  }

  private void OnConversationEnd(bool SetPlayerToIdle = true, bool ShowHUD = true)
  {
    MMConversation.OnConversationEnd -= new MMConversation.ConversationEnd(this.OnConversationEnd);
    this.ShowOtherFollowers();
  }

  public IEnumerator GiveDiscipleRewardRoutine(
    FollowerTaskType previousTask,
    System.Action Callback = null,
    bool GoToAndStop = true)
  {
    interaction_FollowerInteraction followerInteraction = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(followerInteraction.gameObject, 4f);
    HUD_Manager.Instance.Hide(false, 0);
    yield return (object) new WaitForEndOfFrame();
    SimulationManager.Pause();
    followerInteraction.follower.FacePosition(PlayerFarming.Instance.transform.position);
    GameManager.GetInstance().OnConversationNext(followerInteraction.gameObject, 4f);
    GameManager.GetInstance().AddPlayerToCamera();
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    followerInteraction.follower.State.CURRENT_STATE = StateMachine.State.Dancing;
    yield return (object) null;
    PlayerFarming.Instance.simpleSpineAnimator.Animate("sermons/sermon-start", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("sermons/sermon-loop", 0, true, 0.0f);
    double num1 = (double) followerInteraction.follower.SetBodyAnimation("devotion/devotion-start", false);
    followerInteraction.follower.AddBodyAnimation("devotion/devotion-waiting", true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/player/float_follower", followerInteraction.follower.gameObject);
    followerInteraction.follower.AdorationUI.Show();
    yield return (object) new WaitForSeconds(0.25f);
    followerInteraction.follower.Brain.Stats.Adoration = 0.0f;
    ++followerInteraction.follower.Brain.Info.XPLevel;
    System.Action onGivenRewards = followerInteraction.OnGivenRewards;
    if (onGivenRewards != null)
      onGivenRewards();
    float SpeedUpSequenceMultiplier = 0.75f;
    followerInteraction.follower.AdorationUI.BarController.ShrinkBarToEmpty(2f * SpeedUpSequenceMultiplier);
    AudioManager.Instance.PlayOneShot("event:/player/speak_to_follower_noBookPage", followerInteraction.transform.position);
    AudioManager.Instance.PlayOneShot("event:/Stings/generic_positive", followerInteraction.transform.position);
    float DevotionToGive = 20f;
    while ((double) --DevotionToGive >= 0.0)
    {
      if (GameManager.HasUnlockAvailable())
        SoulCustomTarget.Create(PlayerFarming.Instance.CameraBone, followerInteraction.follower.CameraBone.transform.position, Color.white, (System.Action) (() => PlayerFarming.Instance.GetSoul(1)));
      else
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, followerInteraction.transform.position + Vector3.back, 0.0f).SetInitialSpeedAndDiraction(8f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
      yield return (object) new WaitForSeconds(0.1f * SpeedUpSequenceMultiplier);
    }
    yield return (object) new WaitForSeconds(0.2f);
    AudioManager.Instance.PlayOneShot("event:/followers/gain_loyalty", followerInteraction.transform.position);
    if (DoctrineUpgradeSystem.TrySermonsStillAvailable() && DoctrineUpgradeSystem.TryGetStillDoctrineStone() && DataManager.Instance.GetVariable(DataManager.Variables.FirstDoctrineStone))
    {
      PickUp pickUp = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.DOCTRINE_STONE, 1, followerInteraction.transform.position);
      if ((UnityEngine.Object) pickUp != (UnityEngine.Object) null)
      {
        Interaction_DoctrineStone component = pickUp.GetComponent<Interaction_DoctrineStone>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        {
          component.AutomaticallyInteract = true;
          component.MagnetToPlayer();
        }
      }
    }
    PlayerFarming.Instance.simpleSpineAnimator.Animate("sermons/sermon-stop", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    double num2 = (double) followerInteraction.follower.SetBodyAnimation("Reactions/react-bow", false);
    followerInteraction.follower.AddBodyAnimation("idle", true, 0.0f);
    yield return (object) new WaitForSeconds(0.5f);
    Debug.Log((object) "Complete task!");
    followerInteraction.follower.Brain.CompleteCurrentTask();
    yield return (object) new WaitForSeconds(0.5f);
    if (previousTask == FollowerTaskType.Sleep)
      followerInteraction.follower.Brain._directInfoAccess.WakeUpDay = -1;
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.LoyaltyCollectReward);
    System.Action action = Callback;
    if (action != null)
      action();
    followerInteraction.Close();
  }

  private IEnumerator AcceptQuestIE(ObjectivesData objective)
  {
    interaction_FollowerInteraction followerInteraction = this;
    yield return (object) new WaitForEndOfFrame();
    Objectives_Story story = (Objectives_Story) null;
    if (objective is Objectives_Story)
    {
      story = objective as Objectives_Story;
      ((Objectives_Story) objective).StoryDataItem.QuestGiven = true;
      if (((Objectives_Story) objective).ParentStoryDataItem != null)
      {
        foreach (StoryDataItem childStoryDataItem in ((Objectives_Story) objective).ParentStoryDataItem.ChildStoryDataItems)
        {
          if (!childStoryDataItem.QuestGiven)
            childStoryDataItem.QuestLocked = true;
        }
      }
      objective = ((Objectives_Story) objective).StoryDataItem.Objective;
      objective.ResetInitialisation();
    }
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(followerInteraction.gameObject, 4f);
    GameObject g = UnityEngine.Object.Instantiate(UnityEngine.Resources.Load("Prefabs/UI/Choice Indicator"), GameObject.FindWithTag("Canvas").transform) as GameObject;
    ChoiceIndicator choice = g.GetComponent<ChoiceIndicator>();
    choice.Offset = new Vector3(0.0f, -300f);
    if (!objective.IsInitialised())
      objective.Init(true);
    objective.Follower = followerInteraction.follower.Brain.Info.ID;
    Quests.AddObjectiveToHistory(objective.Index, objective.QuestCooldown);
    choice.Show("UI/Generic/Accept", "FollowerInteractions/AcceptQuest", "UI/Generic/Decline", "FollowerInteractions/DeclineQuest", (System.Action) (() =>
    {
      this.AcceptedQuest(objective);
      this.follower.TimedAnimation("Reactions/react-happy1", 1.86666667f, (System.Action) (() => this.follower.Brain.CompleteCurrentTask()));
    }), (System.Action) (() =>
    {
      if (story != null)
      {
        story.StoryDataItem.QuestDeclined = true;
        foreach (StoryDataItem childStoryDataItem in story.StoryDataItem.ChildStoryDataItems)
          childStoryDataItem.QuestDeclined = true;
      }
      this.follower.TimedAnimation("Reactions/react-sad", 1.86666667f, (System.Action) (() => this.follower.Brain.CompleteCurrentTask()));
      CultFaithManager.AddThought(Thought.Cult_DeclinedQuest);
    }), followerInteraction.transform.position);
    choice.ShowObjectivesBox(objective);
    while ((UnityEngine.Object) g != (UnityEngine.Object) null)
    {
      choice.UpdatePosition(followerInteraction.transform.position);
      yield return (object) null;
    }
    if (objective is Objectives_RecruitCursedFollower recruitCursedFollower && recruitCursedFollower.CursedState == Thought.Dissenter && DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Dissenter))
    {
      UITutorialOverlayController overlayController = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Dissenter);
      overlayController.OnHidden = overlayController.OnHidden + new System.Action(followerInteraction.Close);
    }
    else
      followerInteraction.Close();
  }

  private void AcceptedQuest(ObjectivesData quest)
  {
    if (quest is Objectives_RecruitCursedFollower)
    {
      Objectives_RecruitCursedFollower recruitCursedFollower = quest as Objectives_RecruitCursedFollower;
      if (BiomeBaseManager.Instance.SpawnExistingRecruits && DataManager.Instance.Followers_Recruit.Count == 0)
        BiomeBaseManager.Instance.SpawnExistingRecruits = false;
      for (int index = 0; index < recruitCursedFollower.Target; ++index)
      {
        FollowerInfo f = FollowerInfo.NewCharacter(FollowerLocation.Base);
        f.StartingCursedState = recruitCursedFollower.CursedState;
        FollowerManager.CreateNewRecruit(f, NotificationCentre.NotificationType.NewRecruit);
      }
    }
    if (quest.Type == Objectives.TYPES.EAT_MEAL && ((Objectives_EatMeal) quest).MealType == StructureBrain.TYPES.MEAL_POOP && !DataManager.Instance.RecipesDiscovered.Contains(InventoryItem.ITEM_TYPE.MEAL_POOP))
      DataManager.Instance.RecipesDiscovered.Add(InventoryItem.ITEM_TYPE.MEAL_POOP);
    if (quest.Type == Objectives.TYPES.EAT_MEAL && ((Objectives_EatMeal) quest).MealType == StructureBrain.TYPES.MEAL_GRASS && !DataManager.Instance.RecipesDiscovered.Contains(InventoryItem.ITEM_TYPE.MEAL_GRASS))
      DataManager.Instance.RecipesDiscovered.Add(InventoryItem.ITEM_TYPE.MEAL_GRASS);
    ObjectiveManager.Add(quest);
  }

  private void NamedCult(string result)
  {
    DataManager.Instance.CultName = result;
    ConversationEntry conversationEntry = new ConversationEntry(this.gameObject, string.Format(LocalizationManager.GetTranslation("Conversation_NPC/FollowerOnboarding/NameCult2"), (object) result));
    conversationEntry.CharacterName = $"<color=yellow>{this.follower.Brain.Info.Name}</color>";
    ConversationObject ConversationObject = new ConversationObject(new List<ConversationEntry>()
    {
      conversationEntry
    }, (List<MMTools.Response>) null, (System.Action) (() =>
    {
      this.eventListener.PlayFollowerVO(this.positiveAcknowledgeVO);
      this.follower.TimedAnimation("Reactions/react-happy1", 1.86666667f, (System.Action) (() => this.follower.Brain.CompleteCurrentTask()));
      this.follower.Brain.AddThought(Thought.LeaderDidQuest);
      AudioManager.Instance.PlayOneShot("event:/followers/gain_loyalty", this.gameObject.transform.position);
      this.StartCoroutine((IEnumerator) this.FrameDelayCallback((System.Action) (() =>
      {
        GameManager.GetInstance().OnConversationNew();
        GameManager.GetInstance().OnConversationNext(this.gameObject, 4f);
        this.follower.Brain.AddAdoration(FollowerBrain.AdorationActions.Quest, (System.Action) (() =>
        {
          if ((double) this.follower.Brain.Stats.Adoration >= (double) this.follower.Brain.Stats.MAX_ADORATION)
            this.StartCoroutine((IEnumerator) this.GiveDiscipleRewardRoutine(this.previousTaskType, GoToAndStop: false));
          else
            this.Close();
        }));
      })));
    }));
    conversationEntry.SetZoom = true;
    conversationEntry.Zoom = 4f;
    MMConversation.Play(ConversationObject, Translate: false);
  }

  public void UpdateLayoutContent()
  {
    LayoutRebuilder.ForceRebuildLayoutImmediate(this.LayoutContent);
  }

  private void CacheAndSetFollower()
  {
    this.StartCoroutine((IEnumerator) this.CacheAndSetFollowerRoutine());
  }

  private IEnumerator CacheAndSetFollowerRoutine()
  {
    interaction_FollowerInteraction followerInteraction = this;
    yield return (object) new WaitForEndOfFrame();
    if (followerInteraction.follower.Brain != null)
    {
      followerInteraction.Task = new FollowerTask_ManualControl();
      followerInteraction.follower.Brain.HardSwapToTask((FollowerTask) followerInteraction.Task);
    }
    yield return (object) new WaitForEndOfFrame();
    followerInteraction.CacheSpineAnimator = followerInteraction.follower.GetComponentInChildren<SimpleSpineAnimator>();
    if ((bool) (UnityEngine.Object) followerInteraction.CacheSpineAnimator)
      followerInteraction.CacheSpineAnimator.enabled = false;
    if ((UnityEngine.Object) followerInteraction.follower != (UnityEngine.Object) null && (UnityEngine.Object) followerInteraction.follower.Spine != (UnityEngine.Object) null && followerInteraction.follower.Spine.AnimationState != null && followerInteraction.follower.Spine.AnimationState.GetCurrent(1) != null)
    {
      followerInteraction.CacheAnimation = followerInteraction.follower.Spine.AnimationState.GetCurrent(1).Animation.Name;
      followerInteraction.CacheLoop = followerInteraction.follower.Spine.AnimationState.GetCurrent(1).Loop;
      followerInteraction.CacheAnimationProgress = followerInteraction.follower.Spine.AnimationState.GetCurrent(1).TrackTime;
      followerInteraction.CacheFacing = followerInteraction.follower.Spine.Skeleton.ScaleX;
    }
    string animName = "Worship/worship";
    if (followerInteraction.follower.Brain.Info.CursedState == Thought.Dissenter)
      animName = "Worship/worship-dissenter";
    else if (followerInteraction.follower.Brain.Info.CursedState == Thought.Ill)
      animName = "Worship/worship-sick";
    else if (followerInteraction.follower.Brain.Info.CursedState == Thought.BecomeStarving)
      animName = "Worship/worship-hungry";
    else if (followerInteraction.follower.Brain.CurrentState != null && followerInteraction.follower.Brain.CurrentState.Type == FollowerStateType.Exhausted)
      animName = "Worship/worship-fatigued";
    double num = (double) followerInteraction.follower.SetBodyAnimation(animName, true);
    followerInteraction.follower.FacePosition(followerInteraction.state.transform.position);
    while (PlayerFarming.Instance.GoToAndStopping)
      yield return (object) null;
  }

  private void ResetFollower()
  {
    if ((UnityEngine.Object) this.follower == (UnityEngine.Object) null)
      return;
    if ((bool) (UnityEngine.Object) this.CacheSpineAnimator)
      this.CacheSpineAnimator.enabled = true;
    if ((UnityEngine.Object) this.follower.Spine != (UnityEngine.Object) null)
    {
      if (!string.IsNullOrEmpty(this.CacheAnimation) && this.follower.Spine.Skeleton.Data.FindAnimation(this.CacheAnimation) != null)
      {
        double num = (double) this.follower.SetBodyAnimation(this.CacheAnimation, this.CacheLoop);
      }
      if (this.follower.Spine.Skeleton != null)
        this.follower.Spine.Skeleton.ScaleX = this.CacheFacing;
      if (this.follower.Spine.AnimationState.GetCurrent(1) != null)
        this.follower.Spine.AnimationState.GetCurrent(1).TrackTime = this.CacheAnimationProgress;
    }
    if (this.follower.Brain == null || this.follower.Brain.CurrentTaskType != FollowerTaskType.ManualControl)
      return;
    this.follower.Brain.CompleteCurrentTask();
  }

  public void SelectTask(StateMachine state, bool Cancellable, bool GiveDoctrinePieceOnClose)
  {
    this.GiveDoctrinePieceOnClose = GiveDoctrinePieceOnClose;
    this.StartCoroutine((IEnumerator) this.SelectTaskRoutine(state));
  }

  private IEnumerator SelectTaskRoutine(StateMachine state)
  {
    interaction_FollowerInteraction followerInteraction = this;
    followerInteraction.state = state;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(followerInteraction.gameObject, 4f);
    GameManager.GetInstance().AddPlayerToCamera();
    GameManager.GetInstance().CameraSetOffset(new Vector3(0.0f, 0.0f, -1f));
    yield return (object) followerInteraction.StartCoroutine((IEnumerator) followerInteraction.CacheAndSetFollowerRoutine());
    HUD_Manager.Instance.Hide(false, 0);
    followerInteraction.follower.Spine.transform.localScale = new Vector3(1f, 1f, 1f);
    SimulationManager.Pause();
    if (followerInteraction.follower.Brain._directInfoAccess.StartingCursedState != Thought.None)
    {
      Debug.Log((object) "1 CALL BACK!");
      followerInteraction.Close();
    }
    else if (FollowerCommandGroups.GiveWorkerCommands(followerInteraction.follower).Count > 0)
    {
      UIFollowerInteractionWheelOverlayController overlayController = MonoSingleton<UIManager>.Instance.FollowerInteractionWheelTemplate.Instantiate<UIFollowerInteractionWheelOverlayController>();
      overlayController.Show(followerInteraction.follower, FollowerCommandGroups.GiveWorkerCommands(followerInteraction.follower), cancellable: false);
      overlayController.OnItemChosen = overlayController.OnItemChosen + new System.Action<FollowerCommands[]>(followerInteraction.OnFollowerCommandFinalized);
    }
    else
    {
      Debug.Log((object) "3 CALL BACK!");
      followerInteraction.Close();
    }
  }

  private void OnFollowerCommandFinalized(params FollowerCommands[] followerCommands)
  {
    FollowerCommands followerCommand = followerCommands[0];
    FollowerCommands preFinalCommand = followerCommands.Length > 1 ? followerCommands[1] : FollowerCommands.None;
    Debug.Log((object) $"Follower Command Finalized with {followerCommand} and {preFinalCommand}".Colour(Color.green));
    switch (followerCommand)
    {
      case FollowerCommands.GiveWorkerCommand_2:
      case FollowerCommands.MakeDemand:
        if (this.follower.Brain.Info.CursedState == Thought.OldAge)
        {
          this.eventListener.PlayFollowerVO(this.negativeAcknowledgeVO);
          this.CloseAndSpeak("TooOldToWork");
          break;
        }
        if (this.follower.Brain.Info.CursedState == Thought.Ill)
        {
          this.eventListener.PlayFollowerVO(this.negativeAcknowledgeVO);
          this.CloseAndSpeak("TooIllToWork");
          break;
        }
        if (this.follower.Brain.Info.CursedState == Thought.BecomeStarving)
        {
          this.eventListener.PlayFollowerVO(this.negativeAcknowledgeVO);
          this.CloseAndSpeak("TooHungryToWork");
          break;
        }
        if (FollowerBrainStats.IsHoliday)
        {
          this.eventListener.PlayFollowerVO(this.negativeAcknowledgeVO);
          this.CloseAndSpeak("HolidayIsActive");
          break;
        }
        goto case FollowerCommands.ChangeRole;
      case FollowerCommands.ChangeRole:
      case FollowerCommands.Talk:
        if (followerCommands.Length == 1 && followerCommands[0] == FollowerCommands.GiveWorkerCommand_2)
        {
          this.CloseAndSpeak("NoTasksAvailable");
          break;
        }
        this.Close();
        break;
      case FollowerCommands.BedRest:
        this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
        List<Structures_HealingBay> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_HealingBay>(FollowerLocation.Base);
        bool flag1 = false;
        for (int index = structuresOfType.Count - 1; index >= 0; --index)
        {
          if (!structuresOfType[index].CheckIfOccupied() || structuresOfType[index].Data.FollowerID == this.follower.Brain.Info.ID)
          {
            flag1 = true;
            break;
          }
        }
        if (!flag1 && this.follower.Brain.HasHome && (UnityEngine.Object) Dwelling.GetDwellingByID(this.follower.Brain._directInfoAccess.DwellingID) != (UnityEngine.Object) null && Dwelling.GetDwellingByID(this.follower.Brain._directInfoAccess.DwellingID).StructureBrain.IsCollapsed)
        {
          this.CloseAndSpeak("SendToBedRest/BrokenBed", (System.Action) (() =>
          {
            this.follower.Brain.CompleteCurrentTask();
            this.follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_SleepBedRest());
            this.follower.Brain.SetPersonalOverrideTask(FollowerTaskType.SleepBedRest);
          }));
          break;
        }
        if (this.previousTaskType != FollowerTaskType.ClaimDwelling)
        {
          this.follower.Brain.CompleteCurrentTask();
          this.follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_SleepBedRest());
          this.follower.Brain.SetPersonalOverrideTask(FollowerTaskType.SleepBedRest);
          goto case FollowerCommands.ChangeRole;
        }
        this.StartCoroutine((IEnumerator) this.DelayCallback(1f, (System.Action) (() => this.follower.Brain.CurrentOverrideTaskType = FollowerTaskType.SleepBedRest)));
        goto case FollowerCommands.ChangeRole;
      case FollowerCommands.ExtortMoney:
        if (this.follower.Brain.Stats.PaidTithes)
        {
          this.eventListener.PlayFollowerVO(this.negativeAcknowledgeVO);
          Debug.Log((object) "ALREADY TITHES!");
          this.CloseAndSpeak("AlreadyPaidTithes");
          break;
        }
        this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
        this.follower.Brain.Stats.PaidTithes = true;
        this.StartCoroutine((IEnumerator) this.ExtortMoneyRoutine());
        AudioManager.Instance.PlayOneShot("event:/followers/pop_in", this.transform.position);
        goto case FollowerCommands.ChangeRole;
      case FollowerCommands.Dance:
        if (this.follower.Brain.Stats.Inspired)
        {
          this.eventListener.PlayFollowerVO(this.negativeAcknowledgeVO);
          this.CloseAndSpeak("AlreadyInspired");
          break;
        }
        this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
        this.follower.Brain.Stats.Inspired = true;
        this.Task.GoToAndStop(this.follower, PlayerFarming.Instance.transform.position + Vector3.left * ((double) this.follower.transform.position.x < (double) PlayerFarming.Instance.transform.position.x ? 1.5f : -1.5f), (System.Action) (() => this.StartCoroutine((IEnumerator) this.DanceRoutine(true))));
        break;
      case FollowerCommands.Gift:
        this.eventListener.PlayFollowerVO(this.negativeAcknowledgeVO);
        this.CloseAndSpeak("NoGifts");
        break;
      case FollowerCommands.Imprison:
        BiomeConstants.Instance.DepthOfFieldTween(1.5f, 5f, 10f, 1f, 145f);
        this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
        double num1 = (double) this.follower.SetBodyAnimation("picked-up-hate", true);
        Prison ClosestPrison = this.GetClosestPrison();
        ClosestPrison.StructureInfo.FollowerID = this.follower.Brain.Info.ID;
        PlayerFarming.Instance.PickUpFollower(this.follower);
        PlayerFarming.Instance.GoToAndStop(ClosestPrison.PrisonerLocation.gameObject, ClosestPrison.gameObject, true, true, (System.Action) (() =>
        {
          PlayerFarming.Instance.DropFollower();
          foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
          {
            if (allBrain.Location == this.follower.Brain.Location && allBrain != this.follower.Brain)
            {
              if ((double) this.follower.Brain.Stats.Reeducation > 0.0)
              {
                if (allBrain.CurrentTaskType == FollowerTaskType.Sleep)
                  allBrain.AddThought(Thought.DissenterImprisonedSleeping);
                else if (allBrain.HasTrait(FollowerTrait.TraitType.Libertarian))
                  allBrain.AddThought(Thought.ImprisonedLibertarian);
                else
                  allBrain.AddThought(Thought.DissenterImprisoned);
              }
              else if (allBrain.CurrentTaskType == FollowerTaskType.Sleep)
                allBrain.AddThought(Thought.InnocentImprisonedSleeping);
              else if (allBrain.HasTrait(FollowerTrait.TraitType.Disciplinarian))
                allBrain.AddThought(Thought.InnocentImprisonedDisciplinarian);
              else if (allBrain.HasTrait(FollowerTrait.TraitType.Libertarian))
                allBrain.AddThought(Thought.ImprisonedLibertarian);
              else
                allBrain.AddThought(Thought.InnocentImprisoned);
            }
          }
          if (this.follower.Brain.Info.CursedState != Thought.Dissenter)
          {
            bool flag2 = false;
            foreach (ObjectivesData objective in DataManager.Instance.Objectives)
            {
              if (objective.Type == Objectives.TYPES.CUSTOM && ((Objectives_Custom) objective).CustomQuestType == Objectives.CustomQuestTypes.SendFollowerToPrison && ((Objectives_Custom) objective).TargetFollowerID == this.follower.Brain.Info.ID)
              {
                flag2 = true;
                break;
              }
            }
            if (!flag2)
            {
              if (DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.Disciplinarian))
                CultFaithManager.AddThought(Thought.Cult_Imprison_Trait);
              else
                CultFaithManager.AddThought(Thought.Cult_Imprison);
            }
          }
          ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.SendFollowerToPrison, this.follower.Brain.Info.ID);
          AudioManager.Instance.PlayOneShot("event:/followers/imprison", this.follower.transform.position);
          this.follower.Brain.TransitionToTask((FollowerTask) new FollowerTask_Imprisoned(ClosestPrison.StructureInfo.ID));
          ClosestPrison.StructureInfo.FollowerID = this.follower.Brain.Info.ID;
          ClosestPrison.StructureInfo.FollowerImprisonedTimestamp = TimeManager.TotalElapsedGameTime;
          ClosestPrison.StructureInfo.FollowerImprisonedFaith = this.follower.Brain.Stats.Reeducation;
          if (!DataManager.Instance.Followers_Imprisoned_IDs.Contains(this.follower.Brain.Info.ID))
            DataManager.Instance.Followers_Imprisoned_IDs.Add(this.follower.Brain.Info.ID);
          this.Close(false);
        }), 30f);
        break;
      case FollowerCommands.CutTrees:
      case FollowerCommands.ForageBerries:
      case FollowerCommands.ClearWeeds:
      case FollowerCommands.ClearRubble:
      case FollowerCommands.WorshipAtShrine:
      case FollowerCommands.Cook_2:
      case FollowerCommands.Farmer_2:
      case FollowerCommands.FaithEnforcer:
      case FollowerCommands.TaxEnforcer:
      case FollowerCommands.CollectTax:
      case FollowerCommands.Janitor_2:
      case FollowerCommands.Refiner_2:
        this.ConvertToWorker();
        this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
        this.follower.Brain.Stats.WorkerBeenGivenOrders = true;
        FollowerRole followerRole = this.follower.Brain.Info.FollowerRole;
        FollowerRole NewRole = followerRole;
        if (followerCommand <= FollowerCommands.Cook_2)
        {
          switch (followerCommand - 14)
          {
            case FollowerCommands.None:
              NewRole = FollowerRole.Lumberjack;
              break;
            case FollowerCommands.GiveWorkerCommand_2:
              NewRole = FollowerRole.Forager;
              break;
            case FollowerCommands.ChangeRole:
              break;
            case FollowerCommands.GiveItem:
              NewRole = FollowerRole.StoneMiner;
              break;
            default:
              if (followerCommand != FollowerCommands.WorshipAtShrine)
              {
                if (followerCommand == FollowerCommands.Cook_2)
                {
                  NewRole = FollowerRole.Chef;
                  break;
                }
                break;
              }
              NewRole = FollowerRole.Worshipper;
              if (followerRole != NewRole)
              {
                this.follower.Brain.CheckChangeTask();
                this.ShowDivineInspirationTutorialOnClose = true;
                break;
              }
              break;
          }
        }
        else if (followerCommand <= FollowerCommands.CollectTax)
        {
          if (followerCommand != FollowerCommands.Farmer_2)
          {
            if (followerCommand == FollowerCommands.CollectTax)
            {
              for (int index = 0; index < this.follower.Brain._directInfoAccess.TaxCollected; ++index)
                ResourceCustomTarget.Create(PlayerFarming.Instance.gameObject, this.follower.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, (System.Action) null);
              AudioManager.Instance.PlayOneShot("event:/followers/pop_in", this.follower.transform.position);
              Inventory.AddItem(20, this.follower.Brain._directInfoAccess.TaxCollected);
              this.follower.Brain._directInfoAccess.TaxCollected = 0;
            }
          }
          else
            NewRole = FollowerRole.Farmer;
        }
        else if (followerCommand != FollowerCommands.Janitor_2)
        {
          if (followerCommand == FollowerCommands.Refiner_2)
            NewRole = FollowerRole.Refiner;
        }
        else
          NewRole = FollowerRole.Janitor;
        if (NewRole != followerRole)
        {
          this.follower.Brain.NewRoleSet(NewRole);
          this.follower.Brain.SetPersonalOverrideTask(FollowerTask.GetFollowerTaskFromRole(NewRole));
          this.StartCoroutine((IEnumerator) this.FrameDelayCallback((System.Action) (() =>
          {
            this.follower.Brain.Info.FollowerRole = NewRole;
            this.follower.Brain.Info.Outfit = FollowerOutfitType.Follower;
            this.follower.SetOutfit(FollowerOutfitType.Follower, false);
            List<FollowerTask> unoccupiedTasksOfType = FollowerBrain.GetAllUnoccupiedTasksOfType(FollowerTask.GetFollowerTaskFromRole(NewRole));
            if (unoccupiedTasksOfType.Count <= 0)
              return;
            this.follower.Brain.HardSwapToTask(unoccupiedTasksOfType[UnityEngine.Random.Range(0, unoccupiedTasksOfType.Count)]);
          })));
          goto case FollowerCommands.ChangeRole;
        }
        goto case FollowerCommands.ChangeRole;
      case FollowerCommands.Romance:
        if (this.follower.Brain.Stats.KissedAction)
        {
          this.eventListener.PlayFollowerVO(this.negativeAcknowledgeVO);
          this.CloseAndSpeak("AlreadySmooched");
          break;
        }
        this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
        this.follower.Brain.Stats.KissedAction = true;
        this.Task.GoToAndStop(this.follower, PlayerFarming.Instance.transform.position + Vector3.left * ((double) this.follower.transform.position.x < (double) PlayerFarming.Instance.transform.position.x ? 1f : -1f), (System.Action) (() => this.StartCoroutine((IEnumerator) this.RomanceRoutine())));
        break;
      case FollowerCommands.WakeUp:
        this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
        if (this.previousTaskType == FollowerTaskType.Sleep && this.follower.Brain.CurrentOverrideTaskType == FollowerTaskType.Sleep || this.previousTaskType == FollowerTaskType.SleepBedRest && this.follower.Brain.CurrentOverrideTaskType == FollowerTaskType.SleepBedRest)
          this.follower.Brain.ClearPersonalOverrideTaskProvider();
        this.follower.Brain.AddThought(Thought.SleepInterrupted);
        this.follower.Brain._directInfoAccess.BrainwashedUntil = TimeManager.CurrentDay;
        if (TimeManager.IsNight)
          CultFaithManager.AddThought(Thought.Cult_WokeUpFollower, this.follower.Brain.Info.ID);
        this.Close(false);
        this.enabled = true;
        this.follower.TimedAnimation("tantrum", 3.16666675f, (System.Action) (() =>
        {
          this.Close();
          this.follower.Brain._directInfoAccess.WakeUpDay = TimeManager.CurrentDay;
          this.follower.Brain.CheckChangeTask();
        }));
        break;
      case FollowerCommands.EatSomething:
        if (FollowerBrainStats.Fasting)
        {
          this.eventListener.PlayFollowerVO(this.negativeAcknowledgeVO);
          this.CloseAndSpeak("CantEatFasting");
          break;
        }
        this.eventListener.PlayFollowerVO(this.negativeAcknowledgeVO);
        this.CloseAndSpeak("NoMeals");
        break;
      case FollowerCommands.Sleep:
        this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
        this.follower.Brain.SetPersonalOverrideTask(FollowerTaskType.Sleep);
        this.follower.Brain.CompleteCurrentTask();
        this.follower.Brain._directInfoAccess.WakeUpDay = -1;
        goto case FollowerCommands.ChangeRole;
      case FollowerCommands.Build:
        int num2 = StructureManager.GetAllStructuresOfType<Structures_BuildSite>(PlayerFarming.Location).Count + StructureManager.GetAllStructuresOfType<Structures_BuildSiteProject>(PlayerFarming.Location).Count;
        this.follower.Brain.Stats.WorkerBeenGivenOrders = true;
        if (num2 > 0)
        {
          this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
          this.ConvertToWorker();
          this.follower.Brain.Info.WorkerPriority = WorkerPriority.None;
          this.follower.Brain.CompleteCurrentTask();
          goto case FollowerCommands.ChangeRole;
        }
        this.eventListener.PlayFollowerVO(this.negativeAcknowledgeVO);
        this.CloseAndSpeak("NoBuildingSites");
        break;
      case FollowerCommands.Meal:
        this.follower.Brain.CancelTargetedMeal(StructureBrain.TYPES.MEAL);
        this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
        this.follower.Brain.SetPersonalOverrideTask(FollowerTaskType.EatMeal, StructureBrain.TYPES.MEAL);
        this.follower.Brain.CompleteCurrentTask();
        goto case FollowerCommands.ChangeRole;
      case FollowerCommands.MealGrass:
        this.follower.Brain.CancelTargetedMeal(StructureBrain.TYPES.MEAL_GRASS);
        this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
        this.follower.Brain.SetPersonalOverrideTask(FollowerTaskType.EatMeal, StructureBrain.TYPES.MEAL_GRASS);
        this.follower.Brain.CompleteCurrentTask();
        goto case FollowerCommands.ChangeRole;
      case FollowerCommands.MealPoop:
        this.follower.Brain.CancelTargetedMeal(StructureBrain.TYPES.MEAL_POOP);
        this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
        this.follower.Brain.SetPersonalOverrideTask(FollowerTaskType.EatMeal, StructureBrain.TYPES.MEAL_POOP);
        this.follower.Brain.CompleteCurrentTask();
        goto case FollowerCommands.ChangeRole;
      case FollowerCommands.MealGoodFish:
        this.follower.Brain.CancelTargetedMeal(StructureBrain.TYPES.MEAL_GOOD_FISH);
        this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
        this.follower.Brain.SetPersonalOverrideTask(FollowerTaskType.EatMeal, StructureBrain.TYPES.MEAL_GOOD_FISH);
        this.follower.Brain.CompleteCurrentTask();
        goto case FollowerCommands.ChangeRole;
      case FollowerCommands.MealFollowerMeat:
        this.follower.Brain.CancelTargetedMeal(StructureBrain.TYPES.MEAL_FOLLOWER_MEAT);
        this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
        this.follower.Brain.SetPersonalOverrideTask(FollowerTaskType.EatMeal, StructureBrain.TYPES.MEAL_FOLLOWER_MEAT);
        this.follower.Brain.CompleteCurrentTask();
        goto case FollowerCommands.ChangeRole;
      case FollowerCommands.MealGreat:
        this.follower.Brain.CancelTargetedMeal(StructureBrain.TYPES.MEAL_GREAT);
        this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
        this.follower.Brain.SetPersonalOverrideTask(FollowerTaskType.EatMeal, StructureBrain.TYPES.MEAL_GREAT);
        this.follower.Brain.CompleteCurrentTask();
        goto case FollowerCommands.ChangeRole;
      case FollowerCommands.MealMushrooms:
        this.follower.Brain.CancelTargetedMeal(StructureBrain.TYPES.MEAL_MUSHROOMS);
        this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
        this.follower.Brain.SetPersonalOverrideTask(FollowerTaskType.EatMeal, StructureBrain.TYPES.MEAL_MUSHROOMS);
        this.follower.Brain.CompleteCurrentTask();
        goto case FollowerCommands.ChangeRole;
      case FollowerCommands.MealMeat:
        this.follower.Brain.CancelTargetedMeal(StructureBrain.TYPES.MEAL_MEAT);
        this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
        this.follower.Brain.SetPersonalOverrideTask(FollowerTaskType.EatMeal, StructureBrain.TYPES.MEAL_MEAT);
        this.follower.Brain.CompleteCurrentTask();
        goto case FollowerCommands.ChangeRole;
      case FollowerCommands.AreYouSureYes:
        this.Task.GoToAndStop(this.follower, PlayerFarming.Instance.transform.position + Vector3.left * ((double) this.follower.transform.position.x < (double) PlayerFarming.Instance.transform.position.x ? 1.5f : -1.5f), (System.Action) (() =>
        {
          switch (preFinalCommand)
          {
            case FollowerCommands.Murder:
              this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
              PlayerFarming.Instance.StartCoroutine((IEnumerator) this.MurderFollower());
              break;
            case FollowerCommands.Ascend:
              this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
              PlayerFarming.Instance.StartCoroutine((IEnumerator) this.AscendFollower());
              break;
          }
        }));
        break;
      case FollowerCommands.Study:
        if (this.follower.Brain.Info.FollowerRole != FollowerRole.Monk)
        {
          this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
          this.follower.Brain.Info.FollowerRole = FollowerRole.Monk;
          this.follower.Brain.Info.Outfit = FollowerOutfitType.Follower;
          this.follower.SetOutfit(FollowerOutfitType.Follower, false);
          this.follower.Brain.CheckChangeTask();
        }
        this.follower.Brain.Stats.WorkerBeenGivenOrders = true;
        goto case FollowerCommands.ChangeRole;
      case FollowerCommands.Intimidate:
        if (this.follower.Brain.Stats.Intimidated)
        {
          this.eventListener.PlayFollowerVO(this.negativeAcknowledgeVO);
          this.CloseAndSpeak("AlreadyIntimidated");
          break;
        }
        this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
        this.follower.Brain.Stats.Intimidated = true;
        this.Task.GoToAndStop(this.follower, PlayerFarming.Instance.transform.position + Vector3.left * ((double) this.follower.transform.position.x < (double) PlayerFarming.Instance.transform.position.x ? 1.5f : -1.5f), (System.Action) (() => this.StartCoroutine((IEnumerator) this.IntimidateRoutine(true))));
        break;
      case FollowerCommands.Bribe:
        if (this.follower.Brain.Stats.Bribed)
        {
          this.eventListener.PlayFollowerVO(this.negativeAcknowledgeVO);
          Debug.Log((object) "ALREADY BRIBED! ");
          this.CloseAndSpeak("AlreadyBribed");
          break;
        }
        if (Inventory.GetItemQuantity(20) < 3)
        {
          this.eventListener.PlayFollowerVO(this.negativeAcknowledgeVO);
          this.CloseAndSpeak("NoGoldBribe");
          break;
        }
        this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
        this.StartCoroutine((IEnumerator) this.BribeRoutine());
        break;
      case FollowerCommands.Ascend:
        if ((double) this.follower.Brain.Stats.Happiness >= 80.0)
          break;
        this.eventListener.PlayFollowerVO(this.negativeAcknowledgeVO);
        this.CloseAndSpeak("AscendFaithTooLow");
        break;
      case FollowerCommands.Surveillance:
        UIFollowerInteractionWheelOverlayController activeMenu = UIManager.GetActiveMenu<UIFollowerInteractionWheelOverlayController>();
        if ((UnityEngine.Object) activeMenu != (UnityEngine.Object) null)
          activeMenu.OnHidden = (System.Action) null;
        if ((double) this.transform.position.x < (double) this.state.transform.position.x)
          GameManager.GetInstance().CameraSetOffset(new Vector3(-1f, 0.0f, -1f));
        else
          GameManager.GetInstance().CameraSetOffset(new Vector3(-2.5f, 0.0f, -1f));
        UIFollowerSummaryMenuController followerSummaryMenuInstance = MonoSingleton<UIManager>.Instance.ShowFollowerSummaryMenu(this.follower);
        UIFollowerSummaryMenuController summaryMenuController = followerSummaryMenuInstance;
        summaryMenuController.OnHidden = summaryMenuController.OnHidden + (System.Action) (() =>
        {
          followerSummaryMenuInstance = (UIFollowerSummaryMenuController) null;
          ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.ReadMind);
          this.Close();
        });
        HUD_Manager.Instance.Hide(false, 0);
        break;
      case FollowerCommands.Gift_Small:
        this.StartCoroutine((IEnumerator) this.GiveItemRoutine(InventoryItem.ITEM_TYPE.GIFT_SMALL));
        break;
      case FollowerCommands.Gift_Medium:
        this.StartCoroutine((IEnumerator) this.GiveItemRoutine(InventoryItem.ITEM_TYPE.GIFT_MEDIUM));
        break;
      case FollowerCommands.Gift_Necklace1:
        this.StartCoroutine((IEnumerator) this.GiveItemRoutine(InventoryItem.ITEM_TYPE.Necklace_1));
        break;
      case FollowerCommands.Gift_Necklace2:
        this.StartCoroutine((IEnumerator) this.GiveItemRoutine(InventoryItem.ITEM_TYPE.Necklace_2));
        break;
      case FollowerCommands.Gift_Necklace3:
        this.StartCoroutine((IEnumerator) this.GiveItemRoutine(InventoryItem.ITEM_TYPE.Necklace_3));
        break;
      case FollowerCommands.Gift_Necklace4:
        this.StartCoroutine((IEnumerator) this.GiveItemRoutine(InventoryItem.ITEM_TYPE.Necklace_4));
        break;
      case FollowerCommands.Gift_Necklace5:
        this.StartCoroutine((IEnumerator) this.GiveItemRoutine(InventoryItem.ITEM_TYPE.Necklace_5));
        break;
      case FollowerCommands.Bless:
        if (this.follower.Brain.Stats.ReceivedBlessing)
        {
          this.eventListener.PlayFollowerVO(this.negativeAcknowledgeVO);
          this.CloseAndSpeak("AlreadyGivenBlessing");
          break;
        }
        this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
        this.follower.Brain.Stats.ReceivedBlessing = true;
        this.Task.GoToAndStop(this.follower, PlayerFarming.Instance.transform.position + Vector3.left * ((double) this.follower.transform.position.x < (double) PlayerFarming.Instance.transform.position.x ? 1.5f : -1.5f), (System.Action) (() => this.StartCoroutine((IEnumerator) this.BlessRoutine(true))));
        break;
      case FollowerCommands.MealGreatFish:
        this.follower.Brain.CancelTargetedMeal(StructureBrain.TYPES.MEAL_GREAT_FISH);
        this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
        this.follower.Brain.SetPersonalOverrideTask(FollowerTaskType.EatMeal, StructureBrain.TYPES.MEAL_GREAT_FISH);
        this.follower.Brain.CompleteCurrentTask();
        goto case FollowerCommands.ChangeRole;
      case FollowerCommands.MealBadFish:
        this.follower.Brain.CancelTargetedMeal(StructureBrain.TYPES.MEAL_BAD_FISH);
        this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
        this.follower.Brain.SetPersonalOverrideTask(FollowerTaskType.EatMeal, StructureBrain.TYPES.MEAL_BAD_FISH);
        this.follower.Brain.CompleteCurrentTask();
        goto case FollowerCommands.ChangeRole;
      case FollowerCommands.RemoveNecklace:
        this.StartCoroutine((IEnumerator) this.GiveItemRoutine(InventoryItem.ITEM_TYPE.Necklace_5));
        break;
      case FollowerCommands.Reeducate:
        if (this.follower.Brain.Stats.ReeducatedAction)
        {
          this.eventListener.PlayFollowerVO(this.negativeAcknowledgeVO);
          this.CloseAndSpeak("AlreadyReeducated");
          break;
        }
        this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
        this.follower.Brain.Stats.ReeducatedAction = true;
        this.Task.GoToAndStop(this.follower, PlayerFarming.Instance.transform.position + Vector3.left * ((double) this.follower.transform.position.x < (double) PlayerFarming.Instance.transform.position.x ? 1.5f : -1.5f), (System.Action) (() => this.StartCoroutine((IEnumerator) this.ReeducateRoutine())));
        break;
      case FollowerCommands.MealBerries:
        this.follower.Brain.CancelTargetedMeal(StructureBrain.TYPES.MEAL_BERRIES);
        this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
        this.follower.Brain.SetPersonalOverrideTask(FollowerTaskType.EatMeal, StructureBrain.TYPES.MEAL_BERRIES);
        this.follower.Brain.CompleteCurrentTask();
        goto case FollowerCommands.ChangeRole;
      case FollowerCommands.MealMediumVeg:
        this.follower.Brain.CancelTargetedMeal(StructureBrain.TYPES.MEAL_MEDIUM_VEG);
        this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
        this.follower.Brain.SetPersonalOverrideTask(FollowerTaskType.EatMeal, StructureBrain.TYPES.MEAL_MEDIUM_VEG);
        this.follower.Brain.CompleteCurrentTask();
        goto case FollowerCommands.ChangeRole;
      case FollowerCommands.MealMixedLow:
        this.follower.Brain.CancelTargetedMeal(StructureBrain.TYPES.MEAL_BAD_MIXED);
        this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
        this.follower.Brain.SetPersonalOverrideTask(FollowerTaskType.EatMeal, StructureBrain.TYPES.MEAL_BAD_MIXED);
        this.follower.Brain.CompleteCurrentTask();
        goto case FollowerCommands.ChangeRole;
      case FollowerCommands.MealMixedMedium:
        this.follower.Brain.CancelTargetedMeal(StructureBrain.TYPES.MEAL_MEDIUM_MIXED);
        this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
        this.follower.Brain.SetPersonalOverrideTask(FollowerTaskType.EatMeal, StructureBrain.TYPES.MEAL_MEDIUM_MIXED);
        this.follower.Brain.CompleteCurrentTask();
        goto case FollowerCommands.ChangeRole;
      case FollowerCommands.MealMixedHigh:
        this.follower.Brain.CancelTargetedMeal(StructureBrain.TYPES.MEAL_GREAT_MIXED);
        this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
        this.follower.Brain.SetPersonalOverrideTask(FollowerTaskType.EatMeal, StructureBrain.TYPES.MEAL_GREAT_MIXED);
        this.follower.Brain.CompleteCurrentTask();
        goto case FollowerCommands.ChangeRole;
      case FollowerCommands.MealDeadly:
        this.follower.Brain.CancelTargetedMeal(StructureBrain.TYPES.MEAL_DEADLY);
        this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
        this.follower.Brain.SetPersonalOverrideTask(FollowerTaskType.EatMeal, StructureBrain.TYPES.MEAL_DEADLY);
        this.follower.Brain.CompleteCurrentTask();
        goto case FollowerCommands.ChangeRole;
      case FollowerCommands.MealMeatLow:
        this.follower.Brain.CancelTargetedMeal(StructureBrain.TYPES.MEAL_BAD_MEAT);
        this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
        this.follower.Brain.SetPersonalOverrideTask(FollowerTaskType.EatMeal, StructureBrain.TYPES.MEAL_BAD_MEAT);
        this.follower.Brain.CompleteCurrentTask();
        goto case FollowerCommands.ChangeRole;
      case FollowerCommands.MealMeatHigh:
        this.follower.Brain.CancelTargetedMeal(StructureBrain.TYPES.MEAL_GREAT_MEAT);
        this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
        this.follower.Brain.SetPersonalOverrideTask(FollowerTaskType.EatMeal, StructureBrain.TYPES.MEAL_GREAT_MEAT);
        this.follower.Brain.CompleteCurrentTask();
        goto case FollowerCommands.ChangeRole;
      case FollowerCommands.ViewTraits:
        this.Close();
        MonoSingleton<UIManager>.Instance.ShowFollowerSummaryMenu(this.follower);
        break;
      case FollowerCommands.PetDog:
        if (this.follower.Brain.Stats.PetDog)
        {
          this.eventListener.PlayFollowerVO(this.negativeAcknowledgeVO);
          this.Close();
          break;
        }
        this.follower.Brain.Stats.PetDog = true;
        this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
        this.Task.GoToAndStop(this.follower, PlayerFarming.Instance.transform.position + Vector3.left * ((double) this.follower.transform.position.x < (double) PlayerFarming.Instance.transform.position.x ? 1f : -1f), (System.Action) (() => this.StartCoroutine((IEnumerator) this.PetDogRoutine())));
        break;
      default:
        Debug.Log((object) $"Warning! Unhandled Follower Command: {followerCommand}".Colour(Color.red));
        goto case FollowerCommands.ChangeRole;
    }
  }

  private IEnumerator ExtortMoneyRoutine()
  {
    interaction_FollowerInteraction followerInteraction = this;
    ResourceCustomTarget.Create(followerInteraction.state.gameObject, followerInteraction.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, (System.Action) (() => Inventory.AddItem(20, 1)));
    yield return (object) new WaitForSeconds(0.2f);
    ResourceCustomTarget.Create(followerInteraction.state.gameObject, followerInteraction.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, (System.Action) (() => Inventory.AddItem(20, 1)));
  }

  private IEnumerator FrameDelayCallback(System.Action callback)
  {
    yield return (object) new WaitForEndOfFrame();
    System.Action action = callback;
    if (action != null)
      action();
  }

  private IEnumerator DelayCallback(float delay, System.Action callback)
  {
    yield return (object) new WaitForSeconds(delay);
    System.Action action = callback;
    if (action != null)
      action();
  }

  private Prison GetClosestPrison()
  {
    Prison closestPrison = (Prison) null;
    float num1 = float.MaxValue;
    foreach (Prison prison in Prison.Prisons)
    {
      if (prison.StructureInfo.FollowerID == -1)
      {
        float num2 = Vector2.Distance((Vector2) this.state.transform.position, (Vector2) prison.transform.position);
        if ((double) num2 < (double) num1)
        {
          closestPrison = prison;
          num1 = num2;
        }
      }
    }
    return closestPrison;
  }

  public bool AvailablePrisons()
  {
    foreach (Prison prison in Prison.Prisons)
    {
      if (prison.StructureInfo.FollowerID == -1)
        return true;
    }
    return false;
  }

  private IEnumerator MurderFollower()
  {
    this.follower.HideAllFollowerIcons();
    this.follower.FacePosition(PlayerFarming.Instance.transform.position);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) null;
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "murder", false);
    PlayerFarming.Instance.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/player/speak_to_follower");
    AudioManager.Instance.PlayOneShot("event:/player/murder_follower_sequence");
    double num = (double) this.follower.SetBodyAnimation("murder", false);
    float Duration = this.follower.Spine.AnimationState.GetCurrent(1).Animation.Duration;
    GameManager.GetInstance().AddToCamera(this.follower.gameObject);
    yield return (object) new WaitForSeconds(0.1f);
    this.follower.Spine.CustomMaterialOverride.Clear();
    this.follower.Spine.CustomMaterialOverride.Add(this.follower.NormalMaterial, this.follower.BW_Material);
    PlayerFarming.Instance.Spine.CustomMaterialOverride.Clear();
    PlayerFarming.Instance.Spine.CustomMaterialOverride.Add(PlayerFarming.Instance.originalMaterial, PlayerFarming.Instance.BW_Material);
    HUD_Manager.Instance.ShowBW(0.33f, 0.0f, 1f);
    BiomeConstants.Instance.EmitSmokeInteractionVFX(this.follower.transform.position, new Vector3(0.5f, 0.5f, 1f));
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    yield return (object) new WaitForSeconds(1.6f);
    BiomeConstants.Instance.EmitSmokeInteractionVFX(this.follower.transform.position, new Vector3(1f, 1f, 1f));
    CameraManager.instance.ShakeCameraForDuration(0.8f, 1f, 0.3f);
    PlayerFarming.Instance.Spine.CustomMaterialOverride.Clear();
    this.follower.Spine.CustomMaterialOverride.Clear();
    HUD_Manager.Instance.ShowBW(0.33f, 1f, 0.0f);
    yield return (object) new WaitForSeconds((float) ((double) Duration - 0.10000000149011612 - 1.7000000476837158));
    JudgementMeter.ShowModify(-1);
    int scaleX = (int) this.follower.Spine.Skeleton.ScaleX;
    this.Close();
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.MurderFollower, this.follower.Brain.Info.ID);
    if (TimeManager.CurrentPhase == DayPhase.Night)
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.MurderFollowerAtNight, this.follower.Brain.Info.ID);
    this.follower.Die(NotificationCentre.NotificationType.MurderedByYou, false, scaleX);
    ++DataManager.Instance.STATS_Murders;
  }

  private IEnumerator AscendFollower()
  {
    this.follower.FacePosition(PlayerFarming.Instance.transform.position);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) null;
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "resurrect", false);
    PlayerFarming.Instance.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    float seconds = this.follower.SetBodyAnimation("sacrifice", false);
    GameManager.GetInstance().AddToCamera(this.follower.gameObject);
    this.follower.Spine.CustomMaterialOverride.Clear();
    this.follower.Spine.CustomMaterialOverride.Add(this.follower.NormalMaterial, this.follower.BW_Material);
    PlayerFarming.Instance.Spine.CustomMaterialOverride.Clear();
    PlayerFarming.Instance.Spine.CustomMaterialOverride.Add(PlayerFarming.Instance.originalMaterial, PlayerFarming.Instance.BW_Material);
    HUD_Manager.Instance.ShowBW(0.33f, 0.0f, 1f);
    yield return (object) new WaitForSeconds(seconds);
    PlayerFarming.Instance.Spine.CustomMaterialOverride.Clear();
    this.follower.Spine.CustomMaterialOverride.Clear();
    HUD_Manager.Instance.ShowBW(0.33f, 1f, 0.0f);
    this.Close();
    this.follower.Die(NotificationCentre.NotificationType.Ascended, false);
  }

  private IEnumerator IntimidateRoutine(bool hostFollower)
  {
    interaction_FollowerInteraction followerInteraction = this;
    if (hostFollower)
    {
      float num = 3f;
      List<Follower> followerList = new List<Follower>();
      foreach (Follower follower in Follower.Followers)
      {
        if ((UnityEngine.Object) follower != (UnityEngine.Object) followerInteraction.follower && follower.Brain.Info.CursedState != Thought.Dissenter && !FollowerManager.FollowerLocked(follower.Brain.Info.ID) && (double) Vector3.Distance(follower.transform.position, followerInteraction.transform.position) < (double) num && !follower.Brain.Stats.Intimidated)
          followerList.Add(follower);
      }
      foreach (Follower follower1 in followerList)
      {
        Follower follower = follower1;
        FollowerTask_ManualControl nextTask = new FollowerTask_ManualControl();
        follower.Brain.HardSwapToTask((FollowerTask) nextTask);
        nextTask.GoToAndStop(follower, followerInteraction.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 1.5f, (System.Action) (() =>
        {
          follower.Brain.Stats.Intimidated = true;
          follower.StartCoroutine((IEnumerator) follower.GetComponent<interaction_FollowerInteraction>().IntimidateRoutine(false));
        }));
      }
      GameManager.GetInstance().OnConversationNext(followerInteraction.gameObject, 4f);
      GameManager.GetInstance().AddPlayerToCamera();
      GameManager.GetInstance().CameraSetOffset(Vector3.zero);
      if (followerList.Count > 0)
        yield return (object) new WaitForSeconds(0.35f);
      PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      AudioManager.Instance.PlayOneShot("event:/player/speak_to_follower", PlayerFarming.Instance.transform.position);
      AudioManager.Instance.PlayOneShot("event:/Stings/white_eyes", PlayerFarming.Instance.transform.position);
      HUD_Manager.Instance.ShowBW(0.33f, 0.0f, 1f);
    }
    followerInteraction.follower.FacePosition(PlayerFarming.Instance.transform.position);
    followerInteraction.follower.Spine.CustomMaterialOverride.Clear();
    followerInteraction.follower.Spine.CustomMaterialOverride.Add(followerInteraction.follower.NormalMaterial, followerInteraction.follower.BW_Material);
    PlayerFarming.Instance.Spine.CustomMaterialOverride.Clear();
    PlayerFarming.Instance.Spine.CustomMaterialOverride.Add(PlayerFarming.Instance.originalMaterial, PlayerFarming.Instance.BW_Material);
    followerInteraction.follower.State.CURRENT_STATE = StateMachine.State.Dancing;
    yield return (object) null;
    float num1 = followerInteraction.follower.SetBodyAnimation("Reactions/react-intimidate", false);
    followerInteraction.follower.AddBodyAnimation("idle", true, 0.0f);
    if (hostFollower)
    {
      PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "intimidate", false);
      PlayerFarming.Instance.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
      AudioManager.Instance.PlayOneShot("event:/player/intimidate_follower", PlayerFarming.Instance.gameObject);
    }
    yield return (object) new WaitForSeconds(num1 - 2.25f);
    followerInteraction.follower.Spine.CustomMaterialOverride.Clear();
    if (hostFollower)
    {
      PlayerFarming.Instance.Spine.CustomMaterialOverride.Clear();
      HUD_Manager.Instance.ShowBW(0.33f, 1f, 0.0f);
      yield return (object) new WaitForSeconds(0.5f);
      AudioManager.Instance.PlayOneShot("event:/followers/gain_loyalty", followerInteraction.gameObject.transform.position);
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.BlessAFollower);
    }
    else
      yield return (object) new WaitForSeconds(0.5f);
    followerInteraction.follower.Brain.AddAdoration(FollowerBrain.AdorationActions.Intimidate, (System.Action) (() =>
    {
      this.follower.Brain.AddThought(Thought.Intimidated);
      if (hostFollower)
      {
        if ((double) this.follower.Brain.Stats.Adoration >= (double) this.follower.Brain.Stats.MAX_ADORATION)
          this.StartCoroutine((IEnumerator) this.GiveDiscipleRewardRoutine(this.previousTaskType));
        else
          this.Close();
      }
      else
        this.follower.Brain.CompleteCurrentTask();
    }));
  }

  public IEnumerator BlessRoutine(bool hostFollower)
  {
    interaction_FollowerInteraction followerInteraction = this;
    if (hostFollower)
    {
      float num = 3f;
      List<Follower> followerList = new List<Follower>();
      foreach (Follower follower in Follower.Followers)
      {
        if ((UnityEngine.Object) follower != (UnityEngine.Object) followerInteraction.follower && follower.Brain.Info.CursedState != Thought.Dissenter && !FollowerManager.FollowerLocked(follower.Brain.Info.ID) && (double) Vector3.Distance(follower.transform.position, followerInteraction.transform.position) < (double) num && !follower.Brain.Stats.ReceivedBlessing)
          followerList.Add(follower);
      }
      foreach (Follower follower1 in followerList)
      {
        Follower follower = follower1;
        FollowerTask_ManualControl nextTask = new FollowerTask_ManualControl();
        follower.Brain.HardSwapToTask((FollowerTask) nextTask);
        nextTask.GoToAndStop(follower, followerInteraction.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 1.5f, (System.Action) (() =>
        {
          follower.Brain.Stats.ReceivedBlessing = true;
          follower.StartCoroutine((IEnumerator) follower.GetComponent<interaction_FollowerInteraction>().BlessRoutine(false));
        }));
      }
      GameManager.GetInstance().OnConversationNext(followerInteraction.gameObject, 4f);
      GameManager.GetInstance().AddPlayerToCamera();
      GameManager.GetInstance().CameraSetOffset(Vector3.zero);
      if (followerList.Count > 0)
        yield return (object) new WaitForSeconds(0.35f);
      PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      AudioManager.Instance.PlayOneShot("event:/player/speak_to_follower", PlayerFarming.Instance.transform.position);
      AudioManager.Instance.PlayOneShot("event:/Stings/white_eyes", PlayerFarming.Instance.transform.position);
    }
    followerInteraction.follower.FacePosition(PlayerFarming.Instance.transform.position);
    followerInteraction.follower.State.CURRENT_STATE = StateMachine.State.Dancing;
    yield return (object) null;
    double num1 = (double) followerInteraction.follower.SetBodyAnimation("devotion/devotion-start", false);
    followerInteraction.follower.AddBodyAnimation("devotion/devotion-waiting", true, 0.0f);
    if (hostFollower)
    {
      yield return (object) PlayerFarming.Instance.Spine.YieldForAnimation("bless");
      PlayerFarming.Instance.simpleSpineAnimator.Animate("idle", 0, true, 0.0f);
    }
    else
      yield return (object) new WaitForSeconds(1.25f);
    double num2 = (double) followerInteraction.follower.SetBodyAnimation("idle", true);
    yield return (object) new WaitForSeconds(0.5f);
    AudioManager.Instance.PlayOneShot("event:/followers/gain_loyalty", followerInteraction.gameObject.transform.position);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.BlessAFollower);
    followerInteraction.follower.Brain.AddAdoration(FollowerBrain.AdorationActions.Bless, (System.Action) (() =>
    {
      CultFaithManager.AddThought(Thought.Cult_Bless);
      if (hostFollower)
      {
        if ((double) this.follower.Brain.Stats.Adoration >= (double) this.follower.Brain.Stats.MAX_ADORATION)
          this.StartCoroutine((IEnumerator) this.GiveDiscipleRewardRoutine(this.previousTaskType));
        else
          this.Close();
      }
      else
        this.follower.Brain.CompleteCurrentTask();
    }));
  }

  private IEnumerator ReeducateRoutine()
  {
    interaction_FollowerInteraction followerInteraction = this;
    followerInteraction.follower.FacePosition(PlayerFarming.Instance.transform.position);
    GameManager.GetInstance().OnConversationNext(followerInteraction.gameObject, 4f);
    GameManager.GetInstance().AddPlayerToCamera();
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    followerInteraction.follower.State.CURRENT_STATE = StateMachine.State.Dancing;
    yield return (object) null;
    int num1 = 1;
    if (((double) followerInteraction.follower.Brain.Stats.Reeducation + 7.5) / 100.0 >= 1.0)
      num1 = 3;
    else if (((double) followerInteraction.follower.Brain.Stats.Reeducation + 7.5) / 100.0 > 0.5)
      num1 = 2;
    PlayerFarming.Instance.simpleSpineAnimator.Animate("reeducate-" + num1.ToString(), 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/player/speak_to_follower", PlayerFarming.Instance.transform.position);
    double num2 = (double) followerInteraction.follower.SetBodyAnimation("reeducate-" + num1.ToString(), false);
    followerInteraction.follower.AddBodyAnimation("idle", true, 0.0f);
    yield return (object) new WaitForSeconds(0.5f);
    yield return (object) new WaitForSeconds(1.5f);
    followerInteraction.follower.Brain.Stats.Reeducation -= 7.5f;
    if ((double) followerInteraction.follower.Brain.Stats.Reeducation > 0.0 && (double) followerInteraction.follower.Brain.Stats.Reeducation < 2.0)
      followerInteraction.follower.Brain.Stats.Reeducation = 0.0f;
    AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", followerInteraction.gameObject.transform.position);
    BiomeConstants.Instance.EmitHeartPickUpVFX(followerInteraction.gameObject.transform.position, 0.0f, "black", "burst_big", false);
    yield return (object) new WaitForSeconds(1f);
    if ((double) followerInteraction.follower.Brain.Stats.Reeducation <= 100.0)
    {
      // ISSUE: reference to a compiler-generated method
      followerInteraction.follower.TimedAnimation("Reactions/react-enlightened1", 2f, new System.Action(followerInteraction.\u003CReeducateRoutine\u003Eb__64_0));
    }
    else
    {
      yield return (object) new WaitForSeconds(0.5f);
      followerInteraction.Close();
    }
  }

  private IEnumerator RomanceRoutine()
  {
    interaction_FollowerInteraction followerInteraction = this;
    followerInteraction.follower.FacePosition(PlayerFarming.Instance.transform.position);
    PlayerFarming.Instance.state.facingAngle = Utils.GetAngle(PlayerFarming.Instance.transform.position, followerInteraction.transform.position);
    GameManager.GetInstance().OnConversationNext(followerInteraction.gameObject, 4f);
    GameManager.GetInstance().AddPlayerToCamera();
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    followerInteraction.follower.State.CURRENT_STATE = StateMachine.State.Dancing;
    yield return (object) null;
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "kiss-follower", false);
    PlayerFarming.Instance.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    double num = (double) followerInteraction.follower.SetBodyAnimation("kiss", true);
    followerInteraction.follower.AddBodyAnimation("idle", true, 0.0f);
    yield return (object) new WaitForSeconds(0.8333333f);
    AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", followerInteraction.gameObject.transform.position);
    BiomeConstants.Instance.EmitHeartPickUpVFX(followerInteraction.transform.position, 0.0f, "red", "burst_big", false);
    followerInteraction.follower.Brain.AddThought(Thought.SpouseKiss);
    yield return (object) new WaitForSeconds(3f);
    PlayerFarming.Instance.Spine.CustomMaterialOverride.Clear();
    followerInteraction.follower.Spine.CustomMaterialOverride.Clear();
    AudioManager.Instance.PlayOneShot("event:/followers/gain_loyalty", followerInteraction.gameObject.transform.position);
    // ISSUE: reference to a compiler-generated method
    followerInteraction.follower.Brain.AddAdoration(FollowerBrain.AdorationActions.SmoochSpouse, new System.Action(followerInteraction.\u003CRomanceRoutine\u003Eb__65_0));
  }

  private IEnumerator PetDogRoutine()
  {
    interaction_FollowerInteraction followerInteraction = this;
    followerInteraction.follower.FacePosition(PlayerFarming.Instance.transform.position);
    PlayerFarming.Instance.state.facingAngle = Utils.GetAngle(PlayerFarming.Instance.transform.position, followerInteraction.transform.position);
    GameManager.GetInstance().OnConversationNext(followerInteraction.gameObject, 4f);
    GameManager.GetInstance().AddPlayerToCamera();
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    followerInteraction.follower.State.CURRENT_STATE = StateMachine.State.Dancing;
    yield return (object) null;
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "pet-dog", false);
    PlayerFarming.Instance.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    double num = (double) followerInteraction.follower.SetBodyAnimation("pet-dog", true);
    followerInteraction.follower.AddBodyAnimation("idle", true, 0.0f);
    yield return (object) new WaitForSeconds(0.8333333f);
    AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", followerInteraction.gameObject.transform.position);
    BiomeConstants.Instance.EmitHeartPickUpVFX(followerInteraction.transform.position, 0.0f, "red", "burst_big", false);
    yield return (object) new WaitForSeconds(1f);
    PlayerFarming.Instance.Spine.CustomMaterialOverride.Clear();
    followerInteraction.follower.Spine.CustomMaterialOverride.Clear();
    yield return (object) new WaitForSeconds(0.5f);
    // ISSUE: reference to a compiler-generated method
    followerInteraction.follower.Brain.AddAdoration(FollowerBrain.AdorationActions.PetDog, new System.Action(followerInteraction.\u003CPetDogRoutine\u003Eb__66_0));
  }

  private IEnumerator DanceRoutine(bool hostFollower)
  {
    interaction_FollowerInteraction followerInteraction = this;
    if (hostFollower)
    {
      float num = 3f;
      List<Follower> followerList = new List<Follower>();
      foreach (Follower follower in Follower.Followers)
      {
        if ((UnityEngine.Object) follower != (UnityEngine.Object) followerInteraction.follower && follower.Brain.Info.CursedState != Thought.Dissenter && !FollowerManager.FollowerLocked(follower.Brain.Info.ID) && (double) Vector3.Distance(follower.transform.position, followerInteraction.transform.position) < (double) num && !follower.Brain.Stats.Inspired)
          followerList.Add(follower);
      }
      foreach (Follower follower1 in followerList)
      {
        Follower follower = follower1;
        FollowerTask_ManualControl nextTask = new FollowerTask_ManualControl();
        follower.Brain.HardSwapToTask((FollowerTask) nextTask);
        nextTask.GoToAndStop(follower, followerInteraction.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 1.5f, (System.Action) (() =>
        {
          follower.Brain.Stats.Inspired = true;
          follower.StartCoroutine((IEnumerator) follower.GetComponent<interaction_FollowerInteraction>().DanceRoutine(false));
        }));
      }
      GameManager.GetInstance().OnConversationNext(followerInteraction.gameObject, 4f);
      GameManager.GetInstance().AddPlayerToCamera();
      GameManager.GetInstance().CameraSetOffset(Vector3.zero);
      if (followerList.Count > 0)
        yield return (object) new WaitForSeconds(0.35f);
      PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      AudioManager.Instance.PlayOneShot("event:/player/speak_to_follower", PlayerFarming.Instance.transform.position);
      AudioManager.Instance.PlayOneShot("event:/Stings/white_eyes", PlayerFarming.Instance.transform.position);
    }
    followerInteraction.follower.State.CURRENT_STATE = StateMachine.State.Dancing;
    yield return (object) null;
    if (hostFollower)
    {
      PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "dance", true);
      AudioManager.Instance.PlayOneShot("event:/player/speak_to_follower", PlayerFarming.Instance.transform.position);
      AudioManager.Instance.SetFollowersDance(1f);
    }
    double num1 = (double) followerInteraction.follower.SetBodyAnimation("dance", true);
    yield return (object) new WaitForSeconds(1.5f);
    if (hostFollower)
    {
      AudioManager.Instance.SetFollowersDance(0.0f);
      AudioManager.Instance.PlayOneShot("event:/followers/gain_loyalty", followerInteraction.gameObject.transform.position);
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.BlessAFollower);
    }
    followerInteraction.follower.Brain.AddAdoration(FollowerBrain.AdorationActions.Inspire, (System.Action) (() =>
    {
      this.follower.Brain.AddThought(Thought.DancedWithLeader);
      CultFaithManager.AddThought(Thought.Cult_Inspire);
      if (hostFollower)
      {
        this.eventListener.PlayFollowerVO(this.bowVO);
        if ((double) this.follower.Brain.Stats.Adoration >= (double) this.follower.Brain.Stats.MAX_ADORATION)
          this.StartCoroutine((IEnumerator) this.GiveDiscipleRewardRoutine(this.previousTaskType));
        else
          this.Close();
      }
      else
        this.follower.Brain.CompleteCurrentTask();
    }));
  }

  private IEnumerator GiveItemRoutine(InventoryItem.ITEM_TYPE itemToGive)
  {
    interaction_FollowerInteraction followerInteraction = this;
    if (followerInteraction.follower.Brain.Info.Necklace != InventoryItem.ITEM_TYPE.NONE && (itemToGive == InventoryItem.ITEM_TYPE.Necklace_1 || itemToGive == InventoryItem.ITEM_TYPE.Necklace_2 || itemToGive == InventoryItem.ITEM_TYPE.Necklace_3 || itemToGive == InventoryItem.ITEM_TYPE.Necklace_4 || itemToGive == InventoryItem.ITEM_TYPE.Necklace_5))
    {
      followerInteraction.eventListener.PlayFollowerVO(followerInteraction.negativeAcknowledgeVO);
      followerInteraction.CloseAndSpeak("AlreadyHaveNecklace");
    }
    else
    {
      followerInteraction.eventListener.PlayFollowerVO(followerInteraction.positiveAcknowledgeVO);
      GameManager.GetInstance().OnConversationNext(followerInteraction.gameObject, 3f);
      GameManager.GetInstance().CameraSetOffset(Vector3.zero);
      yield return (object) new WaitForSeconds(1f);
      DataManager.Instance.GivenFollowerGift = true;
      PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      float faithMultiplier = 7f;
      if (itemToGive == InventoryItem.ITEM_TYPE.GIFT_MEDIUM)
      {
        PlayerFarming.Instance.simpleSpineAnimator.Animate("give-item/gift-medium", 0, false);
        faithMultiplier = 10f;
        JudgementMeter.ShowModify(1);
      }
      else if (itemToGive == InventoryItem.ITEM_TYPE.GIFT_SMALL)
      {
        PlayerFarming.Instance.simpleSpineAnimator.Animate("give-item/gift-small", 0, false);
        faithMultiplier = 5f;
      }
      else
        PlayerFarming.Instance.simpleSpineAnimator.Animate("give-item/generic", 0, false);
      CultFaithManager.AddThought(Thought.Cult_GaveFollowerItem, -1, faithMultiplier, InventoryItem.LocalizedName(itemToGive));
      PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
      GameManager.GetInstance().OnConversationNext(followerInteraction.gameObject, 3f);
      switch (itemToGive)
      {
        case InventoryItem.ITEM_TYPE.GIFT_SMALL:
          followerInteraction.follower.Brain.GetWillLevelUp(FollowerBrain.AdorationActions.Gift);
          break;
        case InventoryItem.ITEM_TYPE.GIFT_MEDIUM:
          followerInteraction.follower.Brain.GetWillLevelUp(FollowerBrain.AdorationActions.BigGift);
          break;
        case InventoryItem.ITEM_TYPE.Necklace_1:
        case InventoryItem.ITEM_TYPE.Necklace_2:
        case InventoryItem.ITEM_TYPE.Necklace_3:
        case InventoryItem.ITEM_TYPE.Necklace_4:
        case InventoryItem.ITEM_TYPE.Necklace_5:
          followerInteraction.follower.Brain.GetWillLevelUp(FollowerBrain.AdorationActions.Necklace);
          break;
      }
      int Waiting = 0;
      AudioManager.Instance.PlayOneShot("event:/followers/pop_in", followerInteraction.transform.position);
      ResourceCustomTarget.Create(followerInteraction.follower.gameObject, PlayerFarming.Instance.CameraBone.transform.position, itemToGive, (System.Action) (() =>
      {
        switch (itemToGive)
        {
          case InventoryItem.ITEM_TYPE.GIFT_SMALL:
            this.follower.TimedAnimation($"Gifts/gift-small-{UnityEngine.Random.Range(1, 4)}", 3.66666675f, (System.Action) (() =>
            {
              AudioManager.Instance.PlayOneShot("event:/followers/gain_loyalty", this.gameObject.transform.position);
              this.follower.Brain.AddAdoration(FollowerBrain.AdorationActions.Gift, (System.Action) (() => ++Waiting));
              System.Action<Follower, InventoryItem.ITEM_TYPE, System.Action> followerCallbacks = InventoryItem.GiveToFollowerCallbacks(itemToGive);
              if (followerCallbacks == null)
                return;
              followerCallbacks(this.follower, itemToGive, (System.Action) (() => ++Waiting));
            }));
            break;
          case InventoryItem.ITEM_TYPE.GIFT_MEDIUM:
            this.follower.TimedAnimation($"Gifts/gift-medium-{UnityEngine.Random.Range(1, 4)}", 3.66666675f, (System.Action) (() =>
            {
              AudioManager.Instance.PlayOneShot("event:/followers/gain_loyalty", this.gameObject.transform.position);
              this.follower.Brain.AddAdoration(FollowerBrain.AdorationActions.BigGift, (System.Action) (() => ++Waiting));
              System.Action<Follower, InventoryItem.ITEM_TYPE, System.Action> followerCallbacks = InventoryItem.GiveToFollowerCallbacks(itemToGive);
              if (followerCallbacks == null)
                return;
              followerCallbacks(this.follower, itemToGive, (System.Action) (() => ++Waiting));
            }));
            break;
          case InventoryItem.ITEM_TYPE.Necklace_1:
          case InventoryItem.ITEM_TYPE.Necklace_2:
          case InventoryItem.ITEM_TYPE.Necklace_3:
          case InventoryItem.ITEM_TYPE.Necklace_4:
          case InventoryItem.ITEM_TYPE.Necklace_5:
            AudioManager.Instance.PlayOneShot("event:/followers/gain_loyalty", this.gameObject.transform.position);
            this.follower.Brain.AddAdoration(FollowerBrain.AdorationActions.Necklace, (System.Action) (() => ++Waiting));
            System.Action<Follower, InventoryItem.ITEM_TYPE, System.Action> followerCallbacks1 = InventoryItem.GiveToFollowerCallbacks(itemToGive);
            if (followerCallbacks1 != null)
            {
              followerCallbacks1(this.follower, itemToGive, (System.Action) (() => ++Waiting));
              break;
            }
            break;
          default:
            ++Waiting;
            break;
        }
        Inventory.ChangeItemQuantity((int) itemToGive, -1);
      }), false);
      while (Waiting < 2)
        yield return (object) null;
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.GiveGift);
      if ((double) followerInteraction.follower.Brain.Stats.Adoration >= (double) followerInteraction.follower.Brain.Stats.MAX_ADORATION)
        followerInteraction.StartCoroutine((IEnumerator) followerInteraction.GiveDiscipleRewardRoutine(followerInteraction.previousTaskType, new System.Action(followerInteraction.Close)));
      else
        followerInteraction.Close();
    }
  }

  private IEnumerator BribeRoutine()
  {
    interaction_FollowerInteraction followerInteraction = this;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.Instance.simpleSpineAnimator.Animate("give-item/generic", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    followerInteraction.follower.AddThought(Thought.Bribed);
    int i = -1;
    while (++i <= 2)
    {
      if (i < 2)
      {
        AudioManager.Instance.PlayOneShot("event:/followers/pop_in", followerInteraction.transform.position);
        ResourceCustomTarget.Create(followerInteraction.follower.gameObject, PlayerFarming.Instance.CameraBone.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, (System.Action) null, false);
        yield return (object) new WaitForSeconds(0.3f);
      }
      else
      {
        AudioManager.Instance.PlayOneShot("event:/followers/pop_in", followerInteraction.transform.position);
        // ISSUE: reference to a compiler-generated method
        ResourceCustomTarget.Create(followerInteraction.follower.gameObject, PlayerFarming.Instance.CameraBone.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, new System.Action(followerInteraction.\u003CBribeRoutine\u003Eb__69_0), false);
      }
    }
  }

  private List<ConversationEntry> GetConversationEntry(
    Follower.ComplaintType ComplaintForBark,
    ObjectivesData objective)
  {
    List<ConversationEntry> conversationEntryList1 = new List<ConversationEntry>();
    List<ConversationEntry> conversationEntry1;
    switch (ComplaintForBark)
    {
      case Follower.ComplaintType.GiveQuest:
        List<ConversationEntry> conversationEntry2;
        if (objective == null)
          conversationEntry2 = new List<ConversationEntry>()
          {
            new ConversationEntry(this.gameObject, LocalizationManager.GetTranslation("FollowerInteractions/AbortGivingQuest"))
          };
        else if (objective != null && objective is Objectives_Story)
        {
          Objectives_Story objectivesStory = objective as Objectives_Story;
          string translation = LocalizationManager.GetTranslation("FollowerInteractions/" + objectivesStory.StoryDataItem.StoryObjectiveData.GiveQuestTerm);
          List<string> stringList = new List<string>();
          if (objectivesStory.StoryDataItem.TargetFollowerID_1 != -1 && objectivesStory.StoryDataItem.TargetFollowerID_1 != objectivesStory.StoryDataItem.QuestGiverFollowerID && FollowerInfo.GetInfoByID(objectivesStory.StoryDataItem.TargetFollowerID_1, true) != null)
            stringList.Add(FollowerInfo.GetInfoByID(objectivesStory.StoryDataItem.TargetFollowerID_1, true).Name);
          if (objectivesStory.StoryDataItem.TargetFollowerID_2 != -1 && FollowerInfo.GetInfoByID(objectivesStory.StoryDataItem.TargetFollowerID_2, true) != null)
            stringList.Add(FollowerInfo.GetInfoByID(objectivesStory.StoryDataItem.TargetFollowerID_2, true).Name);
          if (objectivesStory.StoryDataItem.DeadFollowerID != -1 && FollowerInfo.GetInfoByID(objectivesStory.StoryDataItem.DeadFollowerID, true) != null)
            stringList.Add(FollowerInfo.GetInfoByID(objectivesStory.StoryDataItem.DeadFollowerID, true).Name);
          string[] array = stringList.ToArray();
          string TermToSpeak = string.Format(translation, (object[]) array);
          List<ConversationEntry> conversationEntryList2 = new List<ConversationEntry>();
          conversationEntryList2.Add(new ConversationEntry(this.gameObject, TermToSpeak));
          int num = 0;
          while (LocalizationManager.GetTermData($"FollowerInteractions/{objectivesStory.StoryDataItem.StoryObjectiveData.GiveQuestTerm}{$"/{++num}"}") != null)
            conversationEntryList2.Add(new ConversationEntry(this.gameObject, $"FollowerInteractions/{objectivesStory.StoryDataItem.StoryObjectiveData.GiveQuestTerm}{$"/{num}"}"));
          conversationEntry2 = conversationEntryList2;
        }
        else if (objective.Type == Objectives.TYPES.PERFORM_RITUAL)
        {
          Objectives_PerformRitual objectivesPerformRitual = objective as Objectives_PerformRitual;
          conversationEntry2 = new List<ConversationEntry>()
          {
            new ConversationEntry(this.gameObject, string.Format(LocalizationManager.GetTranslation("FollowerInteractions/GiveQuest/PerformRitual/" + objectivesPerformRitual.Ritual.ToString()), (object) FollowerInfo.GetInfoByID(objectivesPerformRitual.TargetFollowerID_1, true)?.Name, (object) FollowerInfo.GetInfoByID(objectivesPerformRitual.TargetFollowerID_2, true)?.Name))
          };
        }
        else if (objective is Objectives_Custom && (((Objectives_Custom) objective).CustomQuestType == Objectives.CustomQuestTypes.SendFollowerOnMissionary || ((Objectives_Custom) objective).CustomQuestType == Objectives.CustomQuestTypes.SendFollowerToPrison || ((Objectives_Custom) objective).CustomQuestType == Objectives.CustomQuestTypes.UseFirePit || ((Objectives_Custom) objective).CustomQuestType == Objectives.CustomQuestTypes.UseFeastTable || ((Objectives_Custom) objective).CustomQuestType == Objectives.CustomQuestTypes.MurderFollower || ((Objectives_Custom) objective).CustomQuestType == Objectives.CustomQuestTypes.MurderFollowerAtNight))
        {
          Objectives_Custom objectivesCustom = objective as Objectives_Custom;
          conversationEntry2 = new List<ConversationEntry>()
          {
            new ConversationEntry(this.gameObject, string.Format(LocalizationManager.GetTranslation("FollowerInteractions/GiveQuest/" + objectivesCustom.CustomQuestType.ToString()), (object) FollowerInfo.GetInfoByID(objectivesCustom.TargetFollowerID)?.Name))
          };
        }
        else if (objective is Objectives_RecruitCursedFollower)
          conversationEntry2 = new List<ConversationEntry>()
          {
            new ConversationEntry(this.gameObject, LocalizationManager.GetTranslation("FollowerInteractions/GiveQuest/RecruitCursedFollower/" + (objective as Objectives_RecruitCursedFollower).CursedState.ToString()))
          };
        else if (objective.Type == Objectives.TYPES.FIND_FOLLOWER)
          conversationEntry2 = new List<ConversationEntry>()
          {
            new ConversationEntry(this.gameObject, LocalizationManager.GetTranslation($"FollowerInteractions/GiveQuest/FindFollower/Variant_{(objective as Objectives_FindFollower).ObjectiveVariant}"))
          };
        else if (objective.Type == Objectives.TYPES.COLLECT_ITEM)
          conversationEntry2 = new List<ConversationEntry>()
          {
            new ConversationEntry(this.gameObject, LocalizationManager.GetTranslation("FollowerInteractions/GiveQuest/CollectItem/" + (objective as Objectives_CollectItem).ItemType.ToString()))
          };
        else if (objective.Type == Objectives.TYPES.COOK_MEALS)
        {
          Objectives_CookMeal objectivesCookMeal = objective as Objectives_CookMeal;
          conversationEntry2 = new List<ConversationEntry>()
          {
            new ConversationEntry(this.gameObject, string.Format(LocalizationManager.GetTranslation("FollowerInteractions/GiveQuest/CookMeal/" + objectivesCookMeal.MealType.ToString()), (object) CookingData.GetLocalizedName(objectivesCookMeal.MealType)))
          };
        }
        else if (objective.Type == Objectives.TYPES.PLACE_STRUCTURES)
          conversationEntry2 = new List<ConversationEntry>()
          {
            new ConversationEntry(this.gameObject, LocalizationManager.GetTranslation("FollowerInteractions/GiveQuest/PlaceStructure/" + (objective as Objectives_PlaceStructure).category.ToString()))
          };
        else if (objective.Type == Objectives.TYPES.BUILD_STRUCTURE)
          conversationEntry2 = new List<ConversationEntry>()
          {
            new ConversationEntry(this.gameObject, LocalizationManager.GetTranslation("FollowerInteractions/GiveQuest/BuildStructure/" + (objective as Objectives_BuildStructure).StructureType.ToString()))
          };
        else if (objective.Type == Objectives.TYPES.EAT_MEAL)
        {
          conversationEntry2 = new List<ConversationEntry>()
          {
            new ConversationEntry(this.gameObject, LocalizationManager.GetTranslation("FollowerInteractions/GiveQuest/EatMeal/" + (objective as Objectives_EatMeal).MealType.ToString()))
          };
        }
        else
        {
          int max = -1;
          object[] objArray;
          do
          {
            objArray = new object[4]
            {
              (object) "FollowerInteractions/",
              (object) ComplaintForBark,
              (object) "_",
              (object) ++max
            };
          }
          while (LocalizationManager.GetTermData(string.Concat(objArray)) != null);
          conversationEntry2 = this.GetConversationEntry($"{(object) ComplaintForBark}_{(object) UnityEngine.Random.Range(0, max)}");
        }
        foreach (ConversationEntry conversationEntry3 in conversationEntry2)
        {
          conversationEntry3.pitchValue = this.follower.Brain._directInfoAccess.follower_pitch;
          conversationEntry3.vibratoValue = this.follower.Brain._directInfoAccess.follower_vibrato;
          conversationEntry3.CharacterName = $"<color=yellow>{this.follower.Brain.Info.Name}</color>";
          conversationEntry3.Animation = "talk-nice1";
          conversationEntry3.SetZoom = true;
          conversationEntry3.Zoom = 4f;
          conversationEntry3.soundPath = this.generalTalkVO;
        }
        return conversationEntry2;
      case Follower.ComplaintType.CompletedQuest:
        if (!string.IsNullOrEmpty(objective.CompleteTerm))
        {
          conversationEntry1 = this.GetConversationEntry(objective.CompleteTerm);
          break;
        }
        goto default;
      case Follower.ComplaintType.GiveOnboarding:
        string TermToSpeak1 = DataManager.Instance.CurrentOnboardingFollowerTerm;
        if (DataManager.Instance.CurrentOnboardingFollowerID == -1)
          TermToSpeak1 = LocalizationManager.GetTranslation("FollowerInteractions/AbortGivingQuest");
        List<ConversationEntry> conversationEntry4 = new List<ConversationEntry>()
        {
          new ConversationEntry(this.gameObject, TermToSpeak1)
        };
        string str = "worship";
        switch (DataManager.Instance.CurrentOnboardingFollowerTerm)
        {
          case "Conversation_NPC/FollowerOnboarding/SickFollower":
            str = "Emotions/emotion-sick";
            break;
          case "Conversation_NPC/FollowerOnboarding/CureDissenter":
            str = "Worship/worship-dissenter";
            double num1 = (double) this.follower.SetBodyAnimation("Worship/worship-dissenter", true);
            break;
        }
        conversationEntry4[0].pitchValue = this.follower.Brain._directInfoAccess.follower_pitch;
        conversationEntry4[0].vibratoValue = this.follower.Brain._directInfoAccess.follower_vibrato;
        conversationEntry4[0].soundPath = this.generalTalkVO;
        conversationEntry4[0].SkeletonData = this.follower.Spine;
        conversationEntry4[0].CharacterName = $"<color=yellow>{this.follower.Brain.Info.Name}</color>";
        conversationEntry4[0].Animation = str;
        conversationEntry4[0].SetZoom = true;
        conversationEntry4[0].Zoom = 4f;
        return conversationEntry4;
      default:
        int max1 = -1;
        object[] objArray1;
        do
        {
          objArray1 = new object[4]
          {
            (object) "FollowerInteractions/",
            (object) ComplaintForBark,
            (object) "_",
            (object) ++max1
          };
        }
        while (LocalizationManager.GetTermData(string.Concat(objArray1)) != null);
        conversationEntry1 = this.GetConversationEntry($"{(object) ComplaintForBark}_{(object) UnityEngine.Random.Range(0, max1)}");
        break;
    }
    foreach (ConversationEntry conversationEntry5 in conversationEntry1)
    {
      conversationEntry5.Speaker = this.follower.gameObject;
      conversationEntry5.soundPath = this.generalTalkVO;
      conversationEntry5.SkeletonData = this.follower.Spine;
    }
    switch (ComplaintForBark)
    {
      case Follower.ComplaintType.Hunger:
        double num2 = (double) this.follower.SetBodyAnimation("Worship/worship-hungry", true);
        break;
      case Follower.ComplaintType.Homeless:
      case Follower.ComplaintType.NeedBetterHouse:
        double num3 = (double) this.follower.SetBodyAnimation("Worship/worship-unhappy", true);
        break;
      default:
        if ((double) this.follower.Brain.Stats.Happiness >= 0.699999988079071)
        {
          double num4 = (double) this.follower.SetBodyAnimation("Worship/worship-happy", true);
          break;
        }
        double num5 = (double) this.follower.SetBodyAnimation("Worship/worship", true);
        break;
    }
    return conversationEntry1;
  }

  private List<ConversationEntry> GetConversationEntry(string Entry)
  {
    List<ConversationEntry> conversationEntry1 = new List<ConversationEntry>()
    {
      new ConversationEntry(this.gameObject, "FollowerInteractions/" + Entry, "worship")
    };
    conversationEntry1[0].CharacterName = $"<color=yellow>{this.follower.Brain.Info.Name}</color>";
    conversationEntry1[0].Animation = "worship";
    conversationEntry1[0].pitchValue = this.follower.Brain._directInfoAccess.follower_pitch;
    conversationEntry1[0].vibratoValue = this.follower.Brain._directInfoAccess.follower_vibrato;
    conversationEntry1[0].SetZoom = true;
    conversationEntry1[0].Zoom = 4f;
    foreach (ConversationEntry conversationEntry2 in conversationEntry1)
    {
      conversationEntry2.Speaker = this.follower.gameObject;
      conversationEntry2.soundPath = this.generalTalkVO;
      conversationEntry2.SkeletonData = this.follower.Spine;
      conversationEntry2.soundPath = this.generalTalkVO;
      conversationEntry2.pitchValue = this.follower.Brain._directInfoAccess.follower_pitch;
      conversationEntry2.vibratoValue = this.follower.Brain._directInfoAccess.follower_vibrato;
    }
    return conversationEntry1;
  }

  private void CloseAndSpeak(string ConversationEntryTerm, System.Action callback = null, bool PlayBow = true)
  {
    this.Close(false, false);
    this.follower.FacePosition(PlayerFarming.Instance.transform.position);
    MMConversation.Play(new ConversationObject(this.GetConversationEntry(ConversationEntryTerm), (List<MMTools.Response>) null, (System.Action) (() =>
    {
      this.UnPause();
      this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
      if (PlayBow)
      {
        this.eventListener.PlayFollowerVO(this.bowVO);
        this.follower.TimedAnimation("Reactions/react-bow", 1.86666667f, (System.Action) (() =>
        {
          this.follower.Brain.CompleteCurrentTask();
          System.Action action = callback;
          if (action == null)
            return;
          action();
        }));
      }
      else
        this.StartCoroutine((IEnumerator) this.WaitFrameToClose(callback));
    })));
    double num = (double) this.follower.SetBodyAnimation("worship-talk", true);
  }

  private IEnumerator WaitFrameToClose(System.Action callback = null)
  {
    yield return (object) null;
    this.follower.Brain.CompleteCurrentTask();
    System.Action action = callback;
    if (action != null)
      action();
  }

  private void ConvertToWorker()
  {
    if (this.follower.Brain.Info.FollowerRole == FollowerRole.Worker)
      return;
    this.follower.Brain.Info.FollowerRole = FollowerRole.Worker;
    this.follower.Brain.Info.Outfit = FollowerOutfitType.Follower;
    this.follower.SetOutfit(FollowerOutfitType.Follower, false);
  }

  private void Close() => this.Close(true);

  private void Close(bool DoResetFollower, bool unpause = true)
  {
    if (unpause)
      this.UnPause();
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    if ((UnityEngine.Object) this.state != (UnityEngine.Object) null)
      this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.Activated = false;
    if (DoResetFollower)
      this.ResetFollower();
    BiomeConstants.Instance.DepthOfFieldTween(0.15f, 8.7f, 26f, 1f, 200f);
    if (MMConversation.CURRENT_CONVERSATION == null)
    {
      GameManager.GetInstance().OnConversationEnd();
      GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    }
    GameManager.GetInstance().CamFollowTarget.SnappyMovement = false;
    if (this.follower.Brain._directInfoAccess.StartingCursedState == Thought.Ill)
      this.follower.Brain.MakeSick();
    else if (this.follower.Brain._directInfoAccess.StartingCursedState == Thought.BecomeStarving)
      this.follower.Brain.MakeStarve();
    else if (this.follower.Brain._directInfoAccess.StartingCursedState == Thought.Dissenter)
      this.follower.Brain.MakeDissenter();
    else if (this.follower.Brain._directInfoAccess.StartingCursedState == Thought.OldAge)
    {
      this.follower.Brain.ApplyCurseState(Thought.OldAge);
      this.follower.Brain.Info.Age = this.follower.Brain.Info.LifeExpectancy;
    }
    this.follower.Brain._directInfoAccess.StartingCursedState = Thought.None;
    if (this.GiveDoctrinePieceOnClose && DoctrineUpgradeSystem.TrySermonsStillAvailable() && DoctrineUpgradeSystem.TryGetStillDoctrineStone() && DataManager.Instance.GetVariable(DataManager.Variables.FirstDoctrineStone))
    {
      Debug.Log((object) "CALL BACK 2!");
      PickUp pickUp = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.DOCTRINE_STONE, 1, this.transform.position);
      if ((UnityEngine.Object) pickUp != (UnityEngine.Object) null)
      {
        Interaction_DoctrineStone component = pickUp.GetComponent<Interaction_DoctrineStone>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        {
          component.MagnetToPlayer();
          component.AutomaticallyInteract = true;
        }
      }
    }
    if (this.ShowDivineInspirationTutorialOnClose && DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.DivineInspiration))
    {
      this.ShowDivineInspirationTutorialOnClose = false;
      MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.DivineInspiration);
    }
    this.follower.WorshipperBubble.StopAllCoroutines();
    this.follower.WorshipperBubble.Close();
    this.ShowOtherFollowers();
    this.HasChanged = true;
  }

  private void UnPause() => SimulationManager.UnPause();

  private void HideOtherFollowers()
  {
  }

  private void ShowOtherFollowers()
  {
  }
}
