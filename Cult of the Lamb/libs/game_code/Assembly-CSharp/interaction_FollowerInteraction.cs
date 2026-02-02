// Decompiled with JetBrains decompiler
// Type: interaction_FollowerInteraction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.FollowerInteractionWheel;
using MMTools;
using Spine.Unity;
using Spine.Unity.Examples;
using src.Extensions;
using src.Managers;
using src.UI;
using src.UI.Overlays.TutorialOverlay;
using src.UINavigator;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class interaction_FollowerInteraction : Interaction
{
  public bool Activated;
  public Follower follower;
  public FollowerTask_ManualControl Task;
  public SimpleSpineAnimator CacheSpineAnimator;
  public string CacheAnimation;
  public float CacheAnimationProgress;
  public float CacheFacing;
  public bool CacheLoop;
  public FollowerAdorationUI AdorationOrb;
  public FollowerPleasureUI PleasureOrb;
  public UIFollowerPrayingProgress FollowerPrayingProgress;
  public bool ShowDivineInspirationTutorialOnClose;
  public LayerMask collisionMask;
  public FollowerRecruit followerRecruit;
  public OverrideLightingProperties overrideLightingProperties;
  public BiomeLightingSettings LightingSettings;
  public GameObject LightningContainer;
  public SpriteRenderer LightningIndicator;
  public SpriteRenderer LightningIndicator2;
  public string generalAcknowledgeVO = "event:/dialogue/followers/general_acknowledge";
  public string negativeAcknowledgeVO = "event:/dialogue/followers/negative_acknowledge";
  public string positiveAcknowledgeVO = "event:/dialogue/followers/positive_acknowledge";
  public string bowVO = "event:/followers/Speech/FollowerBow";
  public FollowerSpineEventListener eventListener;
  public Thought transitioningCursedState;
  public bool lightningIncoming;
  public static float preventLightningStrikeTimestamp;
  public string sSpeakTo;
  public string sColllectReward;
  public string sCompleteQuest;
  public FollowerTaskType previousTaskType;
  public Follower.ComplaintType complaintType = Follower.ComplaintType.None;
  public UIFollowerInteractionWheelOverlayController followerInteractionWheelInstance;
  public System.Action OnGivenRewards;
  public bool GiveDoctrinePieceOnClose;
  public RendererMaterialSwap rendMatSwap;
  public Material prevMat;
  public Material followerUIMat;
  [HideInInspector]
  public bool preventCloseOnRemovingPlayer;
  public bool isLoadingAssets;
  public UIRebuildBedMinigameOverlayController _uiCookingMinigameOverlayController;
  public Coroutine lightningStrikeRoutine;
  public EventInstance lightningLoopSfx;
  public bool struckByLightning;

  public string generalTalkVO
  {
    get
    {
      if (this.follower.Brain.Info.ID == 666)
        return "event:/dialogue/followers/boss/fol_deathcat";
      if (this.follower.Brain.Info.ID == 99995)
        return "event:/dialogue/followers/boss/fol_guardian_a";
      if (this.follower.Brain.Info.ID == 99994)
        return "event:/dialogue/followers/boss/fol_guardian_b";
      if (this.follower.Brain.Info.ID == 99992)
        return "event:/dialogue/followers/boss/fol_kallamar";
      if (this.follower.Brain.Info.ID == 99990)
        return "event:/dialogue/followers/boss/fol_leshy";
      if (this.follower.Brain.Info.ID == 99993)
        return "event:/dialogue/followers/boss/fol_shamura";
      if (this.follower.Brain.Info.ID == 99991)
        return "event:/dialogue/followers/boss/fol_heket";
      if (this.follower.Brain.Info.ID == 99996)
        return "event:/dialogue/followers/boss/fol_sozo_standard";
      if (this.follower.Brain.Info.CursedState == Thought.Child)
        return "event:/dialogue/followers/babies/gibberish";
      if (this.follower.Brain.Info.ID == 100007)
        return "event:/dlc/dialogue/yngya/follower_general_talk";
      if (this.follower.Brain.Info.ID == 10011)
        return "event:/dlc/dialogue/miniboss_dog/general_talk";
      if (this.follower.Brain.Info.ID == 10012 || this.follower.Brain.Info.ID == 10013)
        return "event:/dialogue/followers/general_talk";
      if (this.follower.Brain.Info.ID == 10014 || this.follower.Brain.Info.ID == 10015 || this.follower.Brain.Info.ID == 10016)
        return "event:/dlc/dialogue/miniboss_wolf_guardian_trio/general_talk";
      return this.follower.Brain.Info.ID == 10010 ? "event:/dlc/dialogue/executioner/follower_general_talk" : "event:/dialogue/followers/general_talk";
    }
  }

  public bool LightningIncoming => this.lightningIncoming;

  public bool IsBishop => FollowerManager.BishopIDs.Contains(this.follower.Brain.Info.ID);

  public bool PlayVO => true;

  public override void IndicateHighlighted(PlayerFarming playerFarming)
  {
    base.IndicateHighlighted(playerFarming);
    if (this.follower.Brain.Info.IsSnowman || this.follower.Brain.Info.HasTrait(FollowerTrait.TraitType.Mutated))
      return;
    if (!this.follower.Brain.ThoughtExists(Thought.Dissenter) && !this.follower.WorshipperBubble.Active && this.follower.Brain.CurrentTaskType != FollowerTaskType.Floating)
    {
      this.AdorationOrb.Show();
      if (this.follower.Brain.Info.Pleasure > 0)
        this.PleasureOrb.Show();
    }
    if (!(this.follower.Brain.CurrentTask is FollowerTask_Pray) && !(this.follower.Brain.CurrentTask is FollowerTask_PrayPassive))
      return;
    this.FollowerPrayingProgress.Hide();
  }

  public override void EndIndicateHighlighted(PlayerFarming playerFarming)
  {
    base.EndIndicateHighlighted(playerFarming);
    this.AdorationOrb.Hide();
    this.PleasureOrb.Hide();
    this.GetComponentInChildren<UIFollowerName>()?.Show();
    if (!(this.follower.Brain.CurrentTask is FollowerTask_Pray) && !(this.follower.Brain.CurrentTask is FollowerTask_PrayPassive) || this.follower.Brain.CurrentTask.State == FollowerTaskState.GoingTo || this.follower.Brain.HasTrait(FollowerTrait.TraitType.Spy))
      return;
    this.FollowerPrayingProgress.Show();
  }

  public override void GetLabel()
  {
    if ((UnityEngine.Object) this.follower == (UnityEngine.Object) null)
      this.Label = "";
    if (this.lightningIncoming)
    {
      this.Interactable = true;
      this.Label = LocalizationManager.GetTranslation("FollowerInteractions/Protect");
    }
    else if (PlayerFarming.Location != FollowerLocation.Base || !this.Interactable)
      this.Label = "";
    else if (this.follower.Brain.CurrentTaskType == FollowerTaskType.LeaveCult && this.follower.Brain.Info.CursedState != Thought.Dissenter && this.follower.Brain.HasTrait(FollowerTrait.TraitType.Spy))
      this.Label = ScriptLocalization.Interactions.Catch;
    else if (this.follower.Brain.CurrentTaskType == FollowerTaskType.Floating || this.follower.Brain.CanGiveSin())
      this.Label = string.Format(ScriptLocalization.Interactions.AbsolveSin, (object) this.follower.Brain.Info.Name);
    else if (this.follower.Brain != null && DataManager.Instance.CompletedQuestFollowerIDs.Contains(this.follower.Brain.Info.ID))
      this.Label = this.sCompleteQuest;
    else if (this.follower.Brain != null && this.follower.Brain.CanLevelUp())
    {
      this.Label = this.sColllectReward;
    }
    else
    {
      string str1 = this.follower.Brain.Info.XPLevel > 1 ? $" {ScriptLocalization.Interactions.Level} {this.follower.Brain.Info.XPLevel.ToNumeral()}" : "";
      if (this.follower.Brain.Info.IsDisciple)
        str1 += " <sprite name=\"icon_Disciple\">";
      string str2 = this.follower.Brain.Info.MarriedToLeader ? " <sprite name=\"icon_Married\">" : "";
      string str3 = " <color=yellow>";
      if (this.follower.Brain.Info.HasTrait(FollowerTrait.TraitType.Mutated))
        str3 = " <color=red>";
      string str4;
      if (!this.Interactable)
        str4 = "";
      else
        str4 = $"{this.sSpeakTo}{str3}{this.follower.Brain.Info.Name}</color>{str1}{str2}";
      this.Label = str4;
    }
  }

  public void Awake()
  {
    this.followerRecruit = this.GetComponent<FollowerRecruit>();
    if (!((UnityEngine.Object) this.LightningContainer != (UnityEngine.Object) null))
      return;
    this.LightningContainer.gameObject.SetActive(false);
  }

  public override void OnEnableInteraction() => base.OnEnableInteraction();

  public override void OnDisableInteraction() => base.OnDisableInteraction();

  public void Start()
  {
    this.ActivateDistance = 2f;
    this.UpdateLocalisation();
    this.HasSecondaryInteraction = false;
    this.collisionMask = (LayerMask) ((int) this.collisionMask | 1 << LayerMask.NameToLayer("Island"));
    this.collisionMask = (LayerMask) ((int) this.collisionMask | 1 << LayerMask.NameToLayer("Obstacles"));
    this.follower.WorshipperBubble.OnBubblePlay += new System.Action(this.OnBubblePlay);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if (this.lightningIncoming)
      this.LightningStrike();
    if ((UnityEngine.Object) this.LightningContainer != (UnityEngine.Object) null)
      this.LightningContainer.gameObject.SetActive(false);
    this.lightningIncoming = false;
    if ((UnityEngine.Object) this.follower != (UnityEngine.Object) null && (UnityEngine.Object) this.follower.WorshipperBubble != (UnityEngine.Object) null)
      this.follower.WorshipperBubble.OnBubblePlay += new System.Action(this.OnBubblePlay);
    AudioManager.Instance.StopLoop(this.lightningLoopSfx);
  }

  public void OnBubblePlay()
  {
    if (!((UnityEngine.Object) this.AdorationOrb != (UnityEngine.Object) null))
      return;
    this.AdorationOrb.Hide();
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sSpeakTo = ScriptLocalization.Interactions.SpeakTo;
    this.sColllectReward = ScriptLocalization.Interactions.CollectDiscipleReward;
    this.sCompleteQuest = ScriptLocalization.Interactions.CompleteQuest;
  }

  public override void Update()
  {
    base.Update();
    if (!((UnityEngine.Object) this.follower != (UnityEngine.Object) null) || this.follower.Brain == null)
      return;
    if (this.follower.Brain.HasTrait(FollowerTrait.TraitType.ChosenOne) && this.follower.Brain.CurrentTaskType != FollowerTaskType.GetPlayerAttention)
      this.Interactable = false;
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
    else if (this.follower.Brain.CurrentTaskType == FollowerTaskType.Floating || this.follower.Brain.CanGiveSin() || this.lightningIncoming)
    {
      this.AutomaticallyInteract = false;
      this.PriorityWeight = 2f;
      this.ActivateDistance = 2f;
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
    this.preventCloseOnRemovingPlayer = false;
    if (Interaction_Daycare.IsInDaycare(this.follower.Brain.Info.ID))
      Interaction_Daycare.RemoveFromDaycare(this.follower.Brain.Info.ID);
    base.OnInteract(state);
    PlayerFarming.SetMainPlayer(state.GetComponent<PlayerFarming>());
    if (this.lightningIncoming)
      this.StartCoroutine((IEnumerator) this.ProtectFromLightningIE());
    else if (this.follower.Brain.CurrentTaskType == FollowerTaskType.Floating || this.follower.Brain.CanGiveSin())
    {
      if (this.follower.Brain.CurrentTaskType != FollowerTaskType.Floating)
      {
        GameManager.GetInstance().OnConversationNew();
        GameManager.GetInstance().OnConversationNext(this.gameObject, 8f);
        this.follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_Floating());
        GameManager.GetInstance().WaitForSeconds(1f, (System.Action) (() => this.follower.GiveSinToPlayer((System.Action) (() => this.Close(false, reshowMenu: false)))));
      }
      else
        this.follower.GiveSinToPlayer((System.Action) (() => this.Close(false, reshowMenu: false)));
    }
    else
    {
      this.complaintType = Follower.ComplaintType.None;
      FollowerTaskType taskType = this.follower.Brain.CurrentTaskType;
      this.AutomaticallyInteract = false;
      this.follower.WorshipperBubble.gameObject.SetActive(false);
      this.follower.CompletedQuestIcon.gameObject.SetActive(false);
      this.previousTaskType = this.follower.Brain.CurrentTask != null ? this.follower.Brain.CurrentTask.Type : FollowerTaskType.None;
      this.GetComponentInChildren<UIFollowerName>()?.Hide(false);
      GameManager.GetInstance().CamFollowTarget.SnappyMovement = true;
      BiomeConstants.Instance.DepthOfFieldTween(1.5f, 4.5f, 10f, 1f, 145f);
      List<CommandItem> commandItems = FollowerCommandGroups.DefaultCommands(this.follower);
      List<ObjectivesData> quests = Quests.GetUnCompletedFollowerQuests(this.follower.Brain.Info.ID, "");
      Objectives_TalkToFollower talkToFollowerQuest = (Objectives_TalkToFollower) null;
      foreach (ObjectivesData objectivesData in quests)
      {
        if (objectivesData is Objectives_TalkToFollower && string.IsNullOrEmpty(((Objectives_TalkToFollower) objectivesData).ResponseTerm))
        {
          talkToFollowerQuest = objectivesData as Objectives_TalkToFollower;
          this.complaintType = Follower.ComplaintType.CompletedQuest;
          break;
        }
      }
      this.complaintType = talkToFollowerQuest != null || this.follower.Brain.CurrentTask == null || this.follower.Brain.CurrentTaskType != FollowerTaskType.GetPlayerAttention ? this.complaintType : (this.follower.Brain.CurrentTask as FollowerTask_GetAttention).ComplaintType;
      FollowerTask_GetAttention attentionTask = this.follower.Brain.CurrentTaskType == FollowerTaskType.GetPlayerAttention ? this.follower.Brain.CurrentTask as FollowerTask_GetAttention : (FollowerTask_GetAttention) null;
      bool isSleeping = false;
      if (this.follower.Brain.CurrentTaskType == FollowerTaskType.Sleep && this.follower.Brain.CurrentTask.State == FollowerTaskState.Doing)
        isSleeping = !((FollowerTask_Sleep) this.follower.Brain.CurrentTask).isAwake;
      else if (this.follower.Brain.CurrentTaskType == FollowerTaskType.SleepBedRest && this.follower.Brain.CurrentTask.State == FollowerTaskState.Doing)
        isSleeping = true;
      this.CacheAndSetFollower(attentionTask, !isSleeping);
      this.HideOtherFollowers();
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
      if (this.follower.Brain.Info.CursedState == Thought.Injured && !DataManager.Instance.InjuredFollowerSpoken && !isSleeping)
      {
        DataManager.Instance.InjuredFollowerSpoken = true;
        this.eventListener.PlayFollowerVO(this.negativeAcknowledgeVO);
        this.CloseAndSpeak("Injured/Help/0", (System.Action) (() => this.OnInteract(state)), false);
        GameManager.GetInstance().CamFollowTarget.SnappyMovement = true;
      }
      else if (this.follower.Brain.Info.CursedState == Thought.OldAge && !DataManager.Instance.OldFollowerSpoken && !isSleeping)
      {
        DataManager.Instance.OldFollowerSpoken = true;
        this.eventListener.PlayFollowerVO(this.negativeAcknowledgeVO);
        this.CloseAndSpeak("TooOldToWork", (System.Action) (() => this.OnInteract(state)), false);
        GameManager.GetInstance().CamFollowTarget.SnappyMovement = true;
      }
      else
      {
        if (!(bool) (UnityEngine.Object) this.playerFarming || !this.playerFarming.isLamb && PlayerFarming.players.Count == 1)
          this.playerFarming = PlayerFarming.FindClosestPlayer(this.transform.position);
        int num1 = (double) this.follower.transform.position.x < (double) this.playerFarming.transform.position.x ? 1 : -1;
        Vector3 vector3 = this.follower.transform.position + new Vector3(2f * (float) num1, 0.0f);
        Collider2D collider2D = Physics2D.OverlapCircle((Vector2) vector3, 1.9f, (int) this.collisionMask);
        if ((UnityEngine.Object) collider2D != (UnityEngine.Object) null && ((double) collider2D.ClosestPoint((Vector2) vector3).x > (double) this.follower.transform.position.x && num1 == 1 || (double) collider2D.ClosestPoint((Vector2) vector3).x < (double) this.follower.transform.position.x && num1 == -1))
          vector3 = this.follower.transform.position + new Vector3(2f * (float) num1, 0.0f);
        this.playerFarming.GoToAndStop(vector3, this.follower.gameObject, DisableCollider: true, GoToCallback: (System.Action) (() =>
        {
          SimulationManager.Pause();
          if (this.previousTaskType == FollowerTaskType.LeaveCult && this.follower.Brain.Info.CursedState != Thought.Dissenter && this.follower.Brain.HasTrait(FollowerTrait.TraitType.Spy))
            this.SpyCatch();
          else if (this.follower.Brain.Info.Traits.Contains(FollowerTrait.TraitType.Poet) && (double) this.follower.Brain._directInfoAccess.PoemProgress >= 240.0 && !isSleeping)
            this.GivePoem();
          else if (this.follower.Brain.Info.ID == 100006 && this.complaintType == Follower.ComplaintType.GiveOnboarding && this.follower.Brain.Info.CursedState == Thought.Child)
          {
            DataManager.Instance.CurrentOnboardingFollowerTerm = "";
            this.Close(false, reshowMenu: false);
          }
          else if (this.follower.Brain.CanLevelUp())
            this.StartCoroutine((IEnumerator) this.LevelUpRoutine(this.previousTaskType, (System.Action) (() =>
            {
              if (this.complaintType != Follower.ComplaintType.CompletedQuest || talkToFollowerQuest == null)
                return;
              this.follower.ShowCompletedQuestIcon(true);
            })));
          else if (taskType == FollowerTaskType.GetPlayerAttention && this.complaintType != Follower.ComplaintType.ShowTwitchMessage || talkToFollowerQuest != null)
          {
            if (this.complaintType == Follower.ComplaintType.GiveOnboarding)
            {
              quests = Onboarding.Instance.GetOnboardingQuests(this.follower.Brain.Info.ID);
              if (quests.Count == 0)
              {
                DataManager.Instance.CurrentOnboardingFollowerID = -1;
                DataManager.Instance.CurrentOnboardingFollowerTerm = "";
                if (ObjectiveManager.GetNumberOfObjectivesInGroup("Objectives/GroupTitles/Quest") < 1)
                  this.complaintType = Follower.ComplaintType.GiveQuest;
              }
            }
            ObjectivesData objective = (ObjectivesData) null;
            if (this.complaintType == Follower.ComplaintType.GiveQuest)
            {
              List<int> ts = new List<int>();
              List<int> excludeList = new List<int>();
              excludeList.Add(this.follower.Brain.Info.ID);
              int targetFollowerID_1 = FollowerManager.GetPossibleQuestFollowerID(excludeList);
              excludeList.Add(targetFollowerID_1);
              int targetFollowerID_2 = FollowerManager.GetPossibleQuestFollowerID(excludeList);
              excludeList.Add(targetFollowerID_2);
              int deadFollowerID = -1;
              ts.Clear();
              foreach (FollowerInfo followerInfo in DataManager.Instance.Followers_Dead)
              {
                if (!followerInfo.HadFuneral)
                  ts.Add(followerInfo.ID);
              }
              ts.Shuffle<int>();
              foreach (int num2 in ts)
              {
                if (deadFollowerID == -1)
                {
                  deadFollowerID = num2;
                  break;
                }
              }
              if (FollowerManager.UniqueFollowerIDs.Contains(this.follower.Brain.Info.ID))
              {
                foreach (StoryObjectiveData storyObjectiveData in Quests.AllStoryObjectiveDatas)
                {
                  if (storyObjectiveData.QuestGiverRequiresID == this.follower.Brain.Info.ID)
                  {
                    bool flag1;
                    bool flag2;
                    if (storyObjectiveData.RequireTarget_1 && storyObjectiveData.Target1FollowerID != -1)
                    {
                      ref int local1 = ref storyObjectiveData.Target1FollowerID;
                      // ISSUE: explicit reference operation
                      ref bool local2 = @false;
                      // ISSUE: explicit reference operation
                      ref bool local3 = @false;
                      // ISSUE: explicit reference operation
                      ref bool local4 = @false;
                      flag1 = true;
                      ref bool local5 = ref flag1;
                      flag2 = false;
                      ref bool local6 = ref flag2;
                      if (!FollowerManager.FollowerLocked(in local1, in local2, in local3, in local4, in local5, in local6) && FollowerInfo.GetInfoByID(storyObjectiveData.Target1FollowerID) != null)
                        targetFollowerID_1 = storyObjectiveData.Target1FollowerID;
                    }
                    if (storyObjectiveData.RequireTarget_2 && storyObjectiveData.Target2FollowerID != -1)
                    {
                      ref int local7 = ref storyObjectiveData.Target2FollowerID;
                      // ISSUE: explicit reference operation
                      ref bool local8 = @false;
                      // ISSUE: explicit reference operation
                      ref bool local9 = @false;
                      // ISSUE: explicit reference operation
                      ref bool local10 = @false;
                      flag1 = true;
                      ref bool local11 = ref flag1;
                      flag2 = false;
                      ref bool local12 = ref flag2;
                      if (!FollowerManager.FollowerLocked(in local7, in local8, in local9, in local10, in local11, in local12) && FollowerInfo.GetInfoByID(storyObjectiveData.Target2FollowerID) != null)
                        targetFollowerID_2 = storyObjectiveData.Target2FollowerID;
                    }
                    if (storyObjectiveData.RequireTarget_Deadbody && storyObjectiveData.DeadBodyFollowerID != -1)
                    {
                      ref int local13 = ref storyObjectiveData.DeadBodyFollowerID;
                      // ISSUE: explicit reference operation
                      ref bool local14 = @false;
                      // ISSUE: explicit reference operation
                      ref bool local15 = @false;
                      // ISSUE: explicit reference operation
                      ref bool local16 = @false;
                      flag1 = true;
                      ref bool local17 = ref flag1;
                      flag2 = false;
                      ref bool local18 = ref flag2;
                      if (!FollowerManager.FollowerLocked(in local13, in local14, in local15, in local16, in local17, in local18) && FollowerInfo.GetInfoByID(storyObjectiveData.DeadBodyFollowerID) != null)
                      {
                        deadFollowerID = storyObjectiveData.DeadBodyFollowerID;
                        break;
                      }
                      break;
                    }
                    break;
                  }
                }
              }
              objective = Quests.GetRandomBaseQuest(this.follower.Brain.Info.ID, targetFollowerID_1, targetFollowerID_2, deadFollowerID);
            }
            else if (this.complaintType == Follower.ComplaintType.CompletedQuest && talkToFollowerQuest != null)
              objective = (ObjectivesData) talkToFollowerQuest;
            bool WillLevelUp = this.follower.Brain.GetWillLevelUp(FollowerBrain.AdorationActions.Quest) && this.complaintType == Follower.ComplaintType.CompletedQuest;
            Coroutine routine = (Coroutine) null;
            if (this.complaintType == Follower.ComplaintType.CompletedQuest)
            {
              for (int index = DataManager.Instance.CompletedObjectives.Count - 1; index >= 0; --index)
              {
                ObjectivesData completedObjective = DataManager.Instance.CompletedObjectives[index];
                if (completedObjective.Type == Objectives.TYPES.COLLECT_ITEM && completedObjective.GroupId == "Objectives/GroupTitles/Quest" && completedObjective.Follower == this.follower.Brain.Info.ID)
                {
                  routine = this.StartCoroutine((IEnumerator) this.GiveItemsRoutine(((Objectives_CollectItem) completedObjective).ItemType, ((Objectives_CollectItem) completedObjective).Target));
                  break;
                }
                if (DataManager.Instance.HasBuildGoodSnowmanQuestAccepted && !DataManager.Instance.HasLifeToTheIceRitualQuestAccepted && completedObjective.Type == Objectives.TYPES.CUSTOM && completedObjective.Follower == this.follower.Brain.Info.ID && ((Objectives_Custom) completedObjective).CustomQuestType == Objectives.CustomQuestTypes.BuildGoodSnowman)
                {
                  Objectives_PerformRitual objective1 = new Objectives_PerformRitual("Objectives/GroupTitles/Quest", UpgradeSystem.Type.Ritual_Snowman);
                  objective1.CompleteTerm = "GiveQuest/PerformRitual/Ritual_Snowman/Complete";
                  DataManager.Instance.HasLifeToTheIceRitualQuestAccepted = true;
                  objective1.AutoRemoveQuestOnceComplete = false;
                  objective1.Follower = this.follower.Brain.Info.ID;
                  objective1.FailLocked = true;
                  ObjectiveManager.RemoveCompleteObjective(completedObjective);
                  ObjectiveManager.Add((ObjectivesData) objective1, true, true);
                }
              }
            }
            GameManager.GetInstance().OnConversationNew();
            GameManager.GetInstance().OnConversationNext(this.follower.gameObject, 4f);
            this.StartCoroutine((IEnumerator) this.WaitForRoutine(routine, (System.Action) (() =>
            {
              this.follower.AdorationUI.Hide();
              this.follower.PleasureUI.Hide();
              this.follower.GetComponentInChildren<UIFollowerName>()?.Hide();
              MMConversation.OnConversationEnd += new MMConversation.ConversationEnd(this.OnConversationEnd);
              List<ConversationEntry> conv = this.GetConversationEntry(this.complaintType, objective, attentionTask);
              foreach (ConversationEntry conversationEntry in conv)
                conversationEntry.soundPath = this.PlayVO ? conversationEntry.soundPath : "";
              if (FollowerManager.BishopIDs.Contains(this.follower.Brain.Info.ID) && objective != null && !string.IsNullOrEmpty(conv[0].TermName))
                objective.CompleteTerm = conv[0].TermName.Replace("FollowerInteractions/", "");
              MMConversation.Play(new ConversationObject(conv, (List<MMTools.Response>) null, (System.Action) (() =>
              {
                string animation = "Reactions/react-bow";
                if (this.follower.Brain.Info.CursedState != Thought.Child)
                  this.eventListener.PlayFollowerVO(this.bowVO);
                float timer = 1.86666667f;
                if (this.follower.Brain.Info.CursedState != Thought.Child)
                  this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
                if (this.complaintType == Follower.ComplaintType.CompletedQuest)
                {
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
                      if (this.follower.Brain.CanLevelUp())
                      {
                        this.StartCoroutine((IEnumerator) this.LevelUpRoutine(this.previousTaskType, (System.Action) (() =>
                        {
                          if (conv.Count > 0)
                          {
                            switch (conv[0].TermToSpeak)
                            {
                              case "FollowerInteractions/BecameDisciple/0":
                              case "FollowerInteractions/BecameDisciple/BornInCult/0":
                              case "FollowerInteractions/BecameDisciple/Bishop/0":
                                MonoSingleton<UIManager>.Instance.ShowUpgradeTree((System.Action) (() => this.Close(true, reshowMenu: false)), UpgradeSystem.Type.DiscipleSystem);
                                break;
                              case "FollowerInteractions/PilgrimsPartFour/0":
                                UIMenuConfirmationWindow confirmationWindow = MonoSingleton<UIManager>.Instance.ConfirmationWindowTemplate.Instantiate<UIMenuConfirmationWindow>();
                                confirmationWindow.Show();
                                confirmationWindow.Configure(ScriptLocalization.UI_PostGameUnlock.Header, LocalizationManager.GetTranslation("UI/ComicsBonusPages"), true);
                                confirmationWindow.OnHidden += (System.Action) (() =>
                                {
                                  PersistenceManager.PersistentData.UnlockedBonusComicPages = true;
                                  PersistenceManager.Save();
                                  this.Close(true, reshowMenu: false);
                                });
                                break;
                              default:
                                this.Close(true, reshowMenu: false);
                                break;
                            }
                          }
                          else
                            this.Close(true, reshowMenu: false);
                        }), false));
                      }
                      else
                      {
                        // ISSUE: reference to a compiler-generated method
                        this.\u003COnInteract\u003Eg__Callback\u007C10();
                      }
                    }));
                    DataManager.Instance.CompletedQuestFollowerIDs.Remove(this.follower.Brain.Info.ID);
                    this.follower.ShowCompletedQuestIcon(false);
                    if (this.previousTaskType != FollowerTaskType.Sleep)
                      return;
                    this.follower.Brain._directInfoAccess.WakeUpDay = -1;
                  })));
                }
                else
                {
                  if (this.complaintType == Follower.ComplaintType.FailedQuest)
                  {
                    CultFaithManager.AddThought(Thought.Cult_FailQuest);
                    ObjectiveManager.ObjectiveRemoved(this.follower.Brain._directInfoAccess.CurrentPlayerQuest);
                    NotificationCentreScreen.Play(NotificationCentre.NotificationType.QuestFailed);
                    animation = "Reactions/react-sad";
                    this.eventListener.PlayFollowerVO(this.negativeAcknowledgeVO);
                    this.follower.Brain.AddThought(Thought.LeaderFailedQuest);
                  }
                  else if (this.complaintType == Follower.ComplaintType.GiveOnboarding)
                  {
                    if (DataManager.Instance.CurrentOnboardingFollowerID == -1)
                    {
                      DataManager.Instance.CurrentOnboardingFollowerTerm = "";
                      quests.Clear();
                    }
                    bool isDLCObjective = false;
                    TimeManager.TimeSinceLastQuest = 0.0f;
                    animation = "Reactions/react-happy1";
                    switch (DataManager.Instance.CurrentOnboardingFollowerTerm)
                    {
                      case "Conversation_NPC/ChosenChild/LegendarySword/Call/0":
                      case "Conversation_NPC/Executioner/Follower/LegendaryAxe/0":
                      case "Conversation_NPC/Follower/FirstSnow/0":
                      case "Conversation_NPC/MidasFollowerQuestHelp/0":
                      case "Conversation_NPC/OnboardChoppingBlock/0":
                      case "FollowerInteractions/GiveQuest/BreedPureBlood/0":
                      case "FollowerInteractions/GiveQuest/BuildGoodSnowman":
                      case "FollowerInteractions/GiveQuest/MatingTent/Yngya/0":
                      case "FollowerInteractions/GiveQuest/Ritual_Bonfire/Yngya/0":
                      case "FollowerInteractions/GiveQuest/WoolhavenFlowers/Yngya/0":
                        isDLCObjective = true;
                        break;
                      case "Conversation_NPC/FollowerOnboardOldAge":
                        quests.Clear();
                        this.follower.SetOutfit(FollowerOutfitType.Old, false);
                        break;
                      case "Conversation_NPC/FollowerOnboarding/CleanUpBase":
                        IllnessBar.Instance?.Reveal();
                        if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Illness))
                        {
                          MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Illness);
                          break;
                        }
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
                      case "Conversation_NPC/FollowerOnboarding/Freezing/0":
                        this.StartCoroutine((IEnumerator) this.FrameDelayCallback((System.Action) (() =>
                        {
                          if (!DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Freezing))
                            return;
                          MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Freezing, callback: (System.Action) (() =>
                          {
                            this.follower.Brain.MakeFreezing();
                            if (!UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Volcanic_Spa))
                              ObjectiveManager.Add((ObjectivesData) new Objectives_UnlockUpgrade("Objectives/GroupTitles/FreezingFollower", UpgradeSystem.Type.Building_Volcanic_Spa), true, true);
                            if (StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.VOLCANIC_SPA).Count > 0)
                              return;
                            ObjectiveManager.Add((ObjectivesData) new Objectives_BuildStructure("Objectives/GroupTitles/FreezingFollower", StructureBrain.TYPES.VOLCANIC_SPA), true, true);
                          }));
                        })));
                        animation = "Freezing/tantrum";
                        timer = 3.2f;
                        isDLCObjective = true;
                        break;
                      case "Conversation_NPC/FollowerOnboarding/NameCult":
                        this.StartCoroutine((IEnumerator) this.FrameDelayCallback((System.Action) (() =>
                        {
                          this.preventCloseOnRemovingPlayer = true;
                          CoopManager.Instance.OnPlayerLeft += (System.Action) (() => PlayerFarming.Instance.GoToAndStop(this.follower.transform.position + Vector3.right * ((double) this.follower.transform.position.x < (double) this.playerFarming.transform.position.x ? 1f : -1f), this.follower.gameObject));
                          GameManager.GetInstance().OnConversationNew();
                          GameManager.GetInstance().OnConversationNext(this.gameObject, 4f);
                          this.StartCoroutine((IEnumerator) UIManager.LoadAssets(MonoSingleton<UIManager>.Instance.LoadCultNameAssets(), (System.Action) (() =>
                          {
                            UICultNameMenuController cultNameMenuInstance = MonoSingleton<UIManager>.Instance.CultNameMenuTemplate.Instantiate<UICultNameMenuController>();
                            cultNameMenuInstance.Show(false, true);
                            cultNameMenuInstance.OnNameConfirmed += new System.Action<string>(this.NamedCult);
                            cultNameMenuInstance.OnHide += (System.Action) (() => { });
                            cultNameMenuInstance.OnHidden += (System.Action) (() =>
                            {
                              cultNameMenuInstance = (UICultNameMenuController) null;
                              CoopManager.Instance.OnPlayerLeft -= (System.Action) (() => PlayerFarming.Instance.GoToAndStop(this.follower.transform.position + Vector3.right * ((double) this.follower.transform.position.x < (double) this.playerFarming.transform.position.x ? 1f : -1f), this.follower.gameObject));
                            });
                            DataManager.Instance.CurrentOnboardingFollowerID = -1;
                            DataManager.Instance.LastFollowerQuestGivenTime = TimeManager.TotalElapsedGameTime;
                          })));
                        })));
                        return;
                      case "Conversation_NPC/FollowerOnboarding/Overheating/0":
                        this.StartCoroutine((IEnumerator) this.FrameDelayCallback((System.Action) (() =>
                        {
                          if (!DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Overheating))
                            return;
                          MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Overheating);
                        })));
                        animation = "Overheated/tantrum";
                        timer = 3.2f;
                        break;
                      case "Conversation_NPC/FollowerOnboarding/Pleasure/0":
                        MonoSingleton<UIManager>.Instance.ShowUpgradeTree(revealType: UpgradeSystem.Type.PleasureSystem);
                        break;
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
                      case "Conversation_NPC/TraitConversation/Scared/FightPit/0":
                      case "Conversation_NPC/TraitConversation/Scared/FollowerFight/0":
                        animation = "Reactions/react-worried" + UnityEngine.Random.Range(1, 3).ToString();
                        timer = 1.93333328f;
                        break;
                      case "FollowerInteractions/GiveQuest/CrisisOfFaith":
                        animation = $"Conversations/react-hate{UnityEngine.Random.Range(1, 3)}";
                        timer = 2f;
                        break;
                    }
                    bool flag = false;
                    if (DataManager.Instance.CurrentOnboardingFollowerTerm.Contains("SozoFollower") && DataManager.Instance.SozoMushroomCount >= 1 && this.follower.Brain._directInfoAccess.SozoBrainshed)
                      flag = true;
                    else if (DataManager.Instance.CurrentOnboardingFollowerTerm.Contains("Valentines"))
                      flag = true;
                    else if (DataManager.Instance.CurrentOnboardingFollowerTerm.Contains("Pilgrim"))
                      flag = true;
                    else if (DataManager.Instance.CurrentOnboardingFollowerTerm.Contains("GiveQuest/MatingTent/Yngya/0"))
                      flag = true;
                    foreach (ObjectivesData objectivesData in quests)
                    {
                      if (objectivesData != null)
                      {
                        objective = objectivesData;
                        if (!flag)
                          ObjectiveManager.Add(objective, true, isDLCObjective);
                        if (objective.Type == Objectives.TYPES.PERFORM_RITUAL && ((Objectives_PerformRitual) objective).Ritual == UpgradeSystem.Type.Ritual_BecomeDisciple)
                        {
                          UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_BecomeDisciple);
                          objective.FailLocked = false;
                        }
                      }
                    }
                    DataManager.Instance.CurrentOnboardingFollowerID = -1;
                    DataManager.Instance.LastFollowerQuestGivenTime = TimeManager.TotalElapsedGameTime;
                    if (flag)
                    {
                      this.StartCoroutine((IEnumerator) this.AcceptQuestIE(objective));
                      return;
                    }
                    if (DataManager.Instance.CurrentOnboardingFollowerTerm.Contains("DeathCat/Relic/First/0"))
                    {
                      this.Close(true, reshowMenu: false);
                      GameManager.GetInstance().WaitForSeconds(0.0f, new System.Action(this.WalkWithDeathCat));
                      return;
                    }
                    if (DataManager.Instance.CurrentOnboardingFollowerTerm.Contains("Executioner/Follower/LegendaryAxe/0"))
                    {
                      this.Close(true, reshowMenu: false);
                      GameManager.GetInstance().WaitForSeconds(0.0f, (System.Action) (() => this.StartCoroutine((IEnumerator) this.GiveLegendaryAxeWeapon())));
                      return;
                    }
                  }
                  else if (this.complaintType == Follower.ComplaintType.GiveQuest && objective != null)
                  {
                    this.StartCoroutine((IEnumerator) this.AcceptQuestIE(objective));
                    return;
                  }
                  this.Close(false, reshowMenu: false);
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
                        tileGridTile = PlacementRegion.Instance.GetFreeClosestTileGridTileAtWorldPosition(this.transform.position);
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
                }
              })), !WillLevelUp);
              MMConversation.PlayVO = this.PlayVO;
            })));
          }
          else
          {
            UnityEngine.Object.FindObjectOfType<CameraFollowTarget>().SetOffset(new Vector3(0.0f, 0.0f, -1f));
            MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = this.playerFarming;
            if ((bool) (UnityEngine.Object) this.followerInteractionWheelInstance)
              this.followerInteractionWheelInstance.Hide(true);
            this.followerInteractionWheelInstance = MonoSingleton<UIManager>.Instance.FollowerInteractionWheelTemplate.Instantiate<UIFollowerInteractionWheelOverlayController>();
            this.followerInteractionWheelInstance.Show(this.follower, commandItems);
            UIFollowerInteractionWheelOverlayController interactionWheelInstance1 = this.followerInteractionWheelInstance;
            interactionWheelInstance1.OnItemChosen = interactionWheelInstance1.OnItemChosen + new System.Action<FollowerCommands[]>(this.OnFollowerCommandFinalized);
            CoopManager.Instance.OnPlayerLeft += new System.Action(this.UIOnPlayerLeft);
            UIFollowerInteractionWheelOverlayController interactionWheelInstance2 = this.followerInteractionWheelInstance;
            interactionWheelInstance2.OnHidden = interactionWheelInstance2.OnHidden + (System.Action) (() => CoopManager.Instance.OnPlayerLeft -= new System.Action(this.UIOnPlayerLeft));
            UIFollowerInteractionWheelOverlayController interactionWheelInstance3 = this.followerInteractionWheelInstance;
            interactionWheelInstance3.OnCancel = interactionWheelInstance3.OnCancel + (System.Action) (() => this.Close(true, reshowMenu: false));
            GameManager.GetInstance().OnConversationNew();
            GameManager.GetInstance().OnConversationNext(this.gameObject, 4f);
            HUD_Manager.Instance.Hide(false, 0);
          }
        }), AbortGoToCallback: (System.Action) (() => this.Close(true, reshowMenu: false)));
      }
    }
  }

  public void UIOnPlayerLeft()
  {
    if (!(bool) (UnityEngine.Object) this.followerInteractionWheelInstance)
      return;
    bool flag = false;
    foreach (UnityEngine.Object player in PlayerFarming.players)
    {
      if ((UnityEngine.Object) this.playerFarming == player)
        flag = true;
    }
    if (flag)
      return;
    this.followerInteractionWheelInstance.Hide();
  }

  public override void OnDisable()
  {
    base.OnDisable();
    if (this.transitioningCursedState != Thought.None)
    {
      if (this.transitioningCursedState == Thought.Ill)
        this.follower.Brain.MakeSick();
      else if (this.transitioningCursedState == Thought.Dissenter)
        this.follower.Brain.MakeDissenter();
      else if (this.transitioningCursedState == Thought.OldAge)
        this.follower.Brain.MakeOld();
      this.transitioningCursedState = Thought.None;
    }
    if (this.lightningIncoming)
      this.LightningStrike();
    if ((UnityEngine.Object) this.LightningContainer != (UnityEngine.Object) null)
      this.LightningContainer.gameObject.SetActive(false);
    this.lightningIncoming = false;
  }

  public IEnumerator WaitForRoutine(Coroutine routine, System.Action callback)
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

  public IEnumerator GiveItemsRoutine(InventoryItem.ITEM_TYPE itemType, int quantity)
  {
    interaction_FollowerInteraction followerInteraction = this;
    for (int i = 0; i < Mathf.Max(quantity, 10); ++i)
    {
      ResourceCustomTarget.Create(followerInteraction.gameObject, followerInteraction.playerFarming.transform.position, itemType, (System.Action) null);
      yield return (object) new WaitForSeconds(0.025f);
    }
    Inventory.ChangeItemQuantity(itemType, -quantity);
  }

  public IEnumerator DelayedCurse(Thought cursedType, float delay)
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

  public void OnConversationEnd(bool SetPlayerToIdle = true, bool ShowHUD = true)
  {
    MMConversation.OnConversationEnd -= new MMConversation.ConversationEnd(this.OnConversationEnd);
    this.ShowOtherFollowers();
  }

  public IEnumerator LevelUpRoutine(
    FollowerTaskType previousTask,
    System.Action Callback = null,
    bool GoToAndStop = true,
    bool onFinishClose = true,
    bool addPlayerToCamera = true)
  {
    interaction_FollowerInteraction followerInteraction = this;
    if ((UnityEngine.Object) followerInteraction.playerFarming != (UnityEngine.Object) null && (UnityEngine.Object) followerInteraction.playerFarming != (UnityEngine.Object) PlayerFarming.Instance && PlayerFarming.players.Count == 1)
      followerInteraction.playerFarming = PlayerFarming.Instance;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(followerInteraction.gameObject, 4f);
    HUD_Manager.Instance.Hide(false, 0);
    yield return (object) new WaitForEndOfFrame();
    SimulationManager.Pause();
    followerInteraction.follower.FacePosition(followerInteraction.playerFarming.transform.position);
    GameManager.GetInstance().OnConversationNext(followerInteraction.gameObject, 4f);
    if (addPlayerToCamera)
      GameManager.GetInstance().AddPlayerToCamera();
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    followerInteraction.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    followerInteraction.follower.State.CURRENT_STATE = StateMachine.State.Dancing;
    yield return (object) null;
    followerInteraction.playerFarming.simpleSpineAnimator.Animate("sermons/sermon-start", 0, false);
    followerInteraction.playerFarming.simpleSpineAnimator.AddAnimate("sermons/sermon-loop", 0, true, 0.0f);
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
    if (followerInteraction._playerFarming.isLamb)
      AudioManager.Instance.PlayOneShot("event:/player/speak_to_follower_noBookPage", followerInteraction.transform.position);
    else
      AudioManager.Instance.PlayOneShot("event:/player/goat_player/goat_speak_to_follower_noBookPage", followerInteraction.transform.position);
    float increment;
    if (followerInteraction.follower.Brain.HasTrait(FollowerTrait.TraitType.BornToTheRot))
    {
      float rotToGive = (float) UnityEngine.Random.Range(5, 10);
      increment = 1f / rotToGive;
      for (int x = 0; (double) x < (double) rotToGive; ++x)
      {
        AudioManager.Instance.PlayOneShot("event:/followers/pop_in", followerInteraction.transform.position);
        ResourceCustomTarget.Create(followerInteraction.playerFarming.gameObject, followerInteraction.transform.position, InventoryItem.ITEM_TYPE.MAGMA_STONE, (System.Action) null);
        Inventory.ChangeItemQuantity(172, 1);
        yield return (object) new WaitForSeconds(increment * SpeedUpSequenceMultiplier);
      }
    }
    else
    {
      increment = 20f;
      while ((double) --increment >= 0.0)
      {
        if ((GameManager.HasUnlockAvailable() ? 1 : (DataManager.Instance.DeathCatBeaten ? 1 : 0)) != 0)
          SoulCustomTarget.Create(followerInteraction.playerFarming.CameraBone, followerInteraction.follower.CameraBone.transform.position, Color.white, new System.Action(followerInteraction.\u003CLevelUpRoutine\u003Eb__60_0));
        else
          InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, followerInteraction.transform.position + Vector3.back, 0.0f).SetInitialSpeedAndDiraction(8f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
        yield return (object) new WaitForSeconds(0.1f * SpeedUpSequenceMultiplier);
      }
    }
    yield return (object) new WaitForSeconds(0.2f);
    if (followerInteraction.follower.Brain.Info.CursedState != Thought.Child && DoctrineUpgradeSystem.TryGetStillDoctrineStone() && DataManager.Instance.GetVariable(DataManager.Variables.FirstDoctrineStone))
    {
      switch (++DataManager.Instance.SpaceOutDoctrineStones % 3)
      {
        case 0:
          PickUp pickUp = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.DOCTRINE_STONE, 1, followerInteraction.transform.position);
          if ((UnityEngine.Object) pickUp != (UnityEngine.Object) null)
          {
            Interaction_DoctrineStone component = pickUp.GetComponent<Interaction_DoctrineStone>();
            if ((UnityEngine.Object) component != (UnityEngine.Object) null)
            {
              component.AutomaticallyInteract = true;
              component.MagnetToPlayer(followerInteraction.playerFarming.gameObject);
              break;
            }
            break;
          }
          break;
      }
    }
    followerInteraction.playerFarming.simpleSpineAnimator.Animate("sermons/sermon-stop", 0, false);
    followerInteraction.playerFarming.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    double num2 = (double) followerInteraction.follower.SetBodyAnimation("Reactions/react-bow", false);
    followerInteraction.follower.AddBodyAnimation("idle", true, 0.0f);
    yield return (object) new WaitForSeconds(0.5f);
    Debug.Log((object) "Complete task!");
    followerInteraction.follower.Brain.CompleteCurrentTask();
    yield return (object) new WaitForSeconds(0.5f);
    if (previousTask == FollowerTaskType.Sleep)
      followerInteraction.follower.Brain._directInfoAccess.WakeUpDay = -1;
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.LoyaltyCollectReward);
    if (onFinishClose)
      followerInteraction.Close(true, reshowMenu: false);
    System.Action action = Callback;
    if (action != null)
      action();
  }

  public IEnumerator AcceptQuestIE(ObjectivesData objective)
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
      if (!story.StoryDataItem.StoryObjectiveData.HasTimer)
        objective.QuestExpireDuration = -1f;
      objective.ResetInitialisation();
    }
    if (objective != null && objective.Type == Objectives.TYPES.EAT_MEAL && ((Objectives_EatMeal) objective).MealType == StructureBrain.TYPES.MEAL_POOP && !DataManager.Instance.RecipesDiscovered.Contains(InventoryItem.ITEM_TYPE.MEAL_POOP))
      DataManager.Instance.RecipesDiscovered.Add(InventoryItem.ITEM_TYPE.MEAL_POOP);
    if (objective != null && objective.Type == Objectives.TYPES.EAT_MEAL && ((Objectives_EatMeal) objective).MealType == StructureBrain.TYPES.MEAL_GRASS && !DataManager.Instance.RecipesDiscovered.Contains(InventoryItem.ITEM_TYPE.MEAL_GRASS))
      DataManager.Instance.RecipesDiscovered.Add(InventoryItem.ITEM_TYPE.MEAL_GRASS);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(followerInteraction.gameObject, 4f);
    GameObject g = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/UI/Choice Indicator"), GameObject.FindWithTag("Canvas").transform) as GameObject;
    ChoiceIndicator choice = g.GetComponent<ChoiceIndicator>();
    choice.Offset = new Vector3(0.0f, -300f);
    if (objective != null)
    {
      if (!objective.IsInitialised())
        objective.Init(true);
      objective.Follower = followerInteraction.follower.Brain.Info.ID;
      Quests.AddObjectiveToHistory(objective.Index, objective.QuestCooldown);
    }
    bool rejected = false;
    string choice1Term = "UI/Generic/Accept";
    string choice1SubtitleTerm = "FollowerInteractions/AcceptQuest";
    string choice2Term = "UI/Generic/Decline";
    string choice2SubtitleTerm = "FollowerInteractions/DeclineQuest";
    if (objective == null || objective.Type == Objectives.TYPES.NONE)
    {
      choice1SubtitleTerm = "";
      choice2SubtitleTerm = "";
    }
    choice.Show(choice1Term, choice1SubtitleTerm, choice2Term, choice2SubtitleTerm, (System.Action) (() =>
    {
      if (objective != null)
        this.AcceptedQuest(objective);
      if (DataManager.Instance.CurrentOnboardingFollowerTerm.Contains("Valentine"))
        this.follower.TimedAnimation("Reactions/react-embarrassed", 3f, (System.Action) (() =>
        {
          if (!(bool) (UnityEngine.Object) this.playerFarming)
            this.playerFarming = PlayerFarming.FindClosestPlayer(this.transform.position);
          this.Task.GoToAndStop(this.follower, this.playerFarming.transform.position + Vector3.left * ((double) this.follower.transform.position.x < (double) this.playerFarming.transform.position.x ? 1f : -1f), (System.Action) (() => this.StartCoroutine((IEnumerator) this.RomanceRoutine(false))));
        }));
      else
        this.follower.TimedAnimation("Reactions/react-happy1", 1.86666667f, (System.Action) (() => this.follower.Brain.CompleteCurrentTask()));
      g = (GameObject) null;
    }), (System.Action) (() =>
    {
      rejected = true;
      if (story != null && story.StoryDataItem != null)
      {
        story.StoryDataItem.QuestDeclined = true;
        foreach (StoryDataItem childStoryDataItem in story.StoryDataItem.ChildStoryDataItems)
          childStoryDataItem.QuestDeclined = true;
      }
      if (this.follower.Brain.Info.ID == 99996 && this.complaintType == Follower.ComplaintType.GiveOnboarding)
      {
        List<ConversationEntry> Entries = new List<ConversationEntry>()
        {
          new ConversationEntry(this.gameObject, "Conversation_NPC/SozoFollower/Declined/0"),
          new ConversationEntry(this.gameObject, "Conversation_NPC/SozoFollower/Declined/1"),
          new ConversationEntry(this.gameObject, "Conversation_NPC/SozoFollower/Declined/2")
        };
        foreach (ConversationEntry conversationEntry in Entries)
          conversationEntry.CharacterName = ScriptLocalization.NAMES.Sozo;
        double num = (double) this.follower.SetBodyAnimation("Conversations/talk-hate" + UnityEngine.Random.Range(1, 4).ToString(), true);
        MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) (() =>
        {
          this.Close(true, reshowMenu: false);
          this.follower.Brain.AddThought((Thought) UnityEngine.Random.Range(387, 392));
          this.follower.Brain.MakeDissenter(true);
          DataManager.Instance.SozoAteMushroomDay = 0;
        })));
      }
      else if (DataManager.Instance.CurrentOnboardingFollowerTerm.Contains("Valentine"))
      {
        List<ConversationEntry> Entries = new List<ConversationEntry>()
        {
          new ConversationEntry(this.gameObject, "Conversation_NPC/Valentines/Rejected")
        };
        foreach (ConversationEntry conversationEntry in Entries)
          conversationEntry.CharacterName = this.follower.Brain.Info.Name;
        double num = (double) this.follower.SetBodyAnimation("Conversations/talk-hate" + UnityEngine.Random.Range(1, 4).ToString(), true);
        MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) (() =>
        {
          this.Close(true, reshowMenu: false);
          this.follower.Brain.MakeDissenter();
          DataManager.Instance.CurrentOnboardingFollowerTerm = "";
        })));
      }
      else if (DataManager.Instance.CurrentOnboardingFollowerTerm.Contains("Pilgrim") && this.complaintType == Follower.ComplaintType.GiveOnboarding)
      {
        if (DataManager.Instance.HasAcceptedPilgrimPart3)
        {
          DataManager.Instance.HasAcceptedPilgrimPart3 = false;
          DataManager.Instance.PilgrimPart3TargetDay = TimeManager.CurrentDay + 1;
        }
        else if (DataManager.Instance.HasAcceptedPilgrimPart2)
        {
          DataManager.Instance.HasAcceptedPilgrimPart2 = false;
          DataManager.Instance.PilgrimPart2TargetDay = TimeManager.CurrentDay + 1;
        }
        else
        {
          DataManager.Instance.HasAcceptedPilgrimPart1 = false;
          DataManager.Instance.PilgrimPart1TargetDay = TimeManager.CurrentDay + 1;
        }
        DataManager.Instance.CurrentOnboardingFollowerTerm = "";
      }
      else
        this.follower.TimedAnimation("Reactions/react-sad", 1.86666667f, (System.Action) (() => this.follower.Brain.CompleteCurrentTask()));
      if (objective != null && objective.Type != Objectives.TYPES.NONE)
        CultFaithManager.AddThought(Thought.Cult_DeclinedQuest);
      g = (GameObject) null;
    }), followerInteraction.transform.position);
    if (objective != null && objective.Type != Objectives.TYPES.NONE)
      choice.ShowObjectivesBox(objective);
    while ((UnityEngine.Object) g != (UnityEngine.Object) null)
    {
      choice.UpdatePosition(followerInteraction.transform.position);
      yield return (object) null;
    }
    if (DataManager.Instance.CurrentOnboardingFollowerTerm.Contains("GiveQuest/MatingTent/Yngya/0"))
      DataManager.Instance.CurrentOnboardingFollowerTerm = "";
    else if (objective is Objectives_RecruitCursedFollower recruitCursedFollower && recruitCursedFollower.CursedState == Thought.Dissenter && DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Dissenter))
    {
      UITutorialOverlayController overlayController = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Dissenter);
      overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() => this.Close(true, reshowMenu: false));
    }
    else if (((followerInteraction.follower.Brain.Info.ID != 99996 ? 0 : (followerInteraction.complaintType == Follower.ComplaintType.GiveOnboarding ? 1 : 0)) & (rejected ? 1 : 0)) == 0 && (!DataManager.Instance.CurrentOnboardingFollowerTerm.Contains("Valentine") || followerInteraction.complaintType != Follower.ComplaintType.GiveOnboarding))
      followerInteraction.Close(true, reshowMenu: false);
  }

  public void WalkWithDeathCat()
  {
    this.follower.Brain.CurrentState = (FollowerState) new FollowerState_Default();
    this.follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    this.follower.SpeedMultiplier = 1.5f;
    this.follower.GoTo(new Vector3(0.0f, 20f, 1f), (System.Action) (() => this.follower.gameObject.SetActive(false)));
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(this.playerFarming.gameObject);
    bool changedRoom = false;
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      player.DropDeadFollower(true);
      player.unitObject.SpeedMultiplier = 0.55f;
      player.GoToAndStop(new Vector3(0.0f, 20f, 0.0f), GoToCallback: (System.Action) (() =>
      {
        if (changedRoom)
          return;
        changedRoom = true;
        this.follower.gameObject.SetActive(true);
        this.follower.SpeedMultiplier = 1f;
        this.follower.Brain.CompleteCurrentTask();
        BiomeBaseManager.Instance.ActivateDoorRoom();
        DoorRoomLocationManager.Instance.DeathCatRelicSequence();
      }));
    }
  }

  public IEnumerator GiveLegendaryAxeWeapon()
  {
    interaction_FollowerInteraction followerInteraction = this;
    PickUp pickup = (PickUp) null;
    InventoryItem.Spawn(InventoryItem.ITEM_TYPE.LEGENDARY_WEAPON_FRAGMENT, 1, followerInteraction.transform.position, result: (System.Action<PickUp>) (p =>
    {
      pickup = p;
      pickup.GetComponent<Interaction_BrokenWeapon>().SetWeapon(EquipmentType.Axe_Legendary);
    }));
    while ((UnityEngine.Object) pickup == (UnityEngine.Object) null)
      yield return (object) null;
    pickup.enabled = false;
    pickup.child.transform.localScale = Vector3.one;
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", followerInteraction.transform.position);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject, 5f);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    PlayerSimpleInventory component = PlayerFarming.Instance.GetComponent<PlayerSimpleInventory>();
    Vector3 itemTargetPosition = new Vector3(component.ItemImage.transform.position.x, component.ItemImage.transform.position.y, -1f);
    bool isMoving = true;
    TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = pickup.transform.DOMove(itemTargetPosition, 1.5f).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(0.2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine);
    tweenerCore.onComplete = tweenerCore.onComplete + (TweenCallback) (() => isMoving = false);
    while (isMoving)
      yield return (object) null;
    pickup.transform.position = itemTargetPosition;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.FoundItem;
    yield return (object) new WaitForSeconds(1.5f);
    Inventory.AddItem(InventoryItem.ITEM_TYPE.LEGENDARY_WEAPON_FRAGMENT, 1);
    DataManager.Instance.AddLegendaryWeaponToUnlockQueue(EquipmentType.Axe_Legendary);
    pickup.GetComponent<Interaction_BrokenWeapon>().StartBringWeaponToBlacksmithObjective();
    UnityEngine.Object.Destroy((UnityEngine.Object) pickup.gameObject);
    GameManager.GetInstance().OnConversationEnd();
  }

  public void AcceptedQuest(ObjectivesData quest)
  {
    if (quest.Type == Objectives.TYPES.NONE)
      return;
    if (quest is Objectives_RecruitCursedFollower)
    {
      Objectives_RecruitCursedFollower recruitCursedFollower = quest as Objectives_RecruitCursedFollower;
      if (BiomeBaseManager.Instance.SpawnExistingRecruits && DataManager.Instance.Followers_Recruit.Count == 0)
        BiomeBaseManager.Instance.SpawnExistingRecruits = false;
      for (int index = 0; index < recruitCursedFollower.Target; ++index)
      {
        FollowerInfo f = FollowerInfo.NewCharacter(FollowerLocation.Base);
        f.StartingCursedState = recruitCursedFollower.CursedState;
        if (recruitCursedFollower.FollowerID != -1)
          f.ID = recruitCursedFollower.FollowerID;
        if (recruitCursedFollower.FollowerName != "")
          f.Name = recruitCursedFollower.FollowerName;
        if (recruitCursedFollower.FollowerSkin != "")
        {
          f.SkinColour = 0;
          f.SkinName = recruitCursedFollower.FollowerSkin;
          f.SkinCharacter = WorshipperData.Instance.GetSkinIndexFromName(f.SkinName);
        }
        FollowerManager.CreateNewRecruit(f, NotificationCentre.NotificationType.NewRecruit);
      }
    }
    ObjectiveManager.Add(quest, this.complaintType == Follower.ComplaintType.GiveOnboarding);
  }

  public void NamedCult(string result)
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
      this.follower.TimedAnimation("Reactions/react-happy1", 1.86666667f, (System.Action) (() =>
      {
        if (this.follower.Brain.GetWillLevelUp(FollowerBrain.AdorationActions.Quest))
          return;
        this.follower.Brain.CompleteCurrentTask();
      }));
      this.follower.Brain.AddThought(Thought.LeaderDidQuest);
      AudioManager.Instance.PlayOneShot("event:/followers/gain_loyalty", this.gameObject.transform.position);
      this.StartCoroutine((IEnumerator) this.FrameDelayCallback((System.Action) (() =>
      {
        GameManager.GetInstance().OnConversationNew();
        GameManager.GetInstance().OnConversationNext(this.gameObject, 4f);
        this.follower.Brain.AddAdoration(FollowerBrain.AdorationActions.Quest, (System.Action) (() =>
        {
          if (this.follower.Brain.CanLevelUp())
            this.StartCoroutine((IEnumerator) this.LevelUpRoutine(this.previousTaskType, GoToAndStop: false));
          else
            this.Close();
        }));
      })));
    }));
    conversationEntry.SetZoom = true;
    conversationEntry.Zoom = 4f;
    MMConversation.Play(ConversationObject, Translate: false);
    MMConversation.PlayVO = this.PlayVO;
  }

  public void CacheAndSetFollower(FollowerTask_GetAttention getAttentionTask = null, bool setManualControl = true)
  {
    this.StartCoroutine((IEnumerator) this.CacheAndSetFollowerRoutine(getAttentionTask, setManualControl));
  }

  public IEnumerator CacheAndSetFollowerRoutine(
    FollowerTask_GetAttention getAttentionTask = null,
    bool setManualControl = true)
  {
    interaction_FollowerInteraction followerInteraction = this;
    yield return (object) new WaitForEndOfFrame();
    if (followerInteraction.follower.Brain != null & setManualControl)
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
    if (followerInteraction.follower.Brain.HasTrait(FollowerTrait.TraitType.Mutated))
      animName = "Worship/worship-mutated";
    else if (followerInteraction.follower.Brain.Info.CursedState == Thought.Dissenter)
      animName = "Worship/worship-dissenter";
    else if (followerInteraction.follower.Brain.Info.CursedState == Thought.Ill)
      animName = "Worship/worship-sick";
    else if (followerInteraction.follower.Brain.Info.CursedState == Thought.BecomeStarving)
      animName = "Worship/worship-hungry";
    else if (followerInteraction.follower.Brain.CurrentState != null && followerInteraction.follower.Brain.CurrentState.Type == FollowerStateType.Exhausted)
      animName = "Worship/worship-fatigued";
    else if (followerInteraction.follower.Brain.Info.HasTrait(FollowerTrait.TraitType.Scared) || followerInteraction.follower.Brain.Info.HasTrait(FollowerTrait.TraitType.CriminalScarred))
      animName = "Scared/scared-covering-loop";
    else if (followerInteraction.follower.Brain.Info.IsDrunk)
      animName = "Worship/worship-drunk";
    else if (followerInteraction.follower.Brain.HasTrait(FollowerTrait.TraitType.Zombie))
      animName = "Zombie/zombie-idle";
    else if (followerInteraction.follower.Brain.HasTrait(FollowerTrait.TraitType.MissionaryTerrified))
      animName = "Existential Dread/dread-idle";
    else if (followerInteraction.follower.Brain.CanFreeze() && SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter)
      animName = "Snow/talk-cold";
    else if (getAttentionTask != null && getAttentionTask.ComplaintType == Follower.ComplaintType.Speak && getAttentionTask.Term.Contains("Bastard"))
      animName = "Reactions/react-laugh";
    double num = (double) followerInteraction.follower.SetBodyAnimation(animName, true);
    followerInteraction.follower.FacePosition(followerInteraction.state.transform.position);
    while (followerInteraction.playerFarming.GoToAndStopping)
      yield return (object) null;
    followerInteraction.follower.FacePosition(followerInteraction.state.transform.position);
  }

  public void ResetFollower()
  {
    if ((UnityEngine.Object) this.follower == (UnityEngine.Object) null)
      return;
    if ((bool) (UnityEngine.Object) this.CacheSpineAnimator)
      this.CacheSpineAnimator.enabled = true;
    if ((UnityEngine.Object) this.follower.Spine != (UnityEngine.Object) null && this.follower.Spine.AnimationState != null)
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
    if (this.follower.Brain != null && this.follower.Brain.CurrentTaskType == FollowerTaskType.ManualControl)
      this.follower.Brain.CompleteCurrentTask();
    if (this.follower.Brain == null || this.follower.Brain.CurrentState == null)
      return;
    this.follower.Brain.CurrentState.SetStateAnimations(this.follower);
  }

  public void SelectTask(StateMachine state, bool Cancellable, bool GiveDoctrinePieceOnClose)
  {
    this.GiveDoctrinePieceOnClose = GiveDoctrinePieceOnClose;
    this.StartCoroutine((IEnumerator) this.SelectTaskRoutine(state));
  }

  public IEnumerator SelectTaskRoutine(StateMachine state)
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
    if (followerInteraction.follower.Brain._directInfoAccess.StartingCursedState != Thought.None || !FollowerBrainStats.ShouldWork || !followerInteraction.follower.Brain.CanWork)
    {
      Debug.Log((object) "1 CALL BACK!");
      if (followerInteraction.follower.Brain.HasTrait(FollowerTrait.TraitType.Mutated) && DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.MutatedFollower))
      {
        GameManager.GetInstance().OnConversationNew();
        MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.MutatedFollower, callback: new System.Action(followerInteraction.\u003CSelectTaskRoutine\u003Eb__71_0));
      }
      followerInteraction.Close(true, reshowMenu: false);
    }
    else if (FollowerCommandGroups.GiveWorkerCommands(followerInteraction.follower).Count > 0)
    {
      if ((bool) (UnityEngine.Object) followerInteraction.followerInteractionWheelInstance)
        followerInteraction.followerInteractionWheelInstance.Hide(true);
      followerInteraction.followerInteractionWheelInstance = MonoSingleton<UIManager>.Instance.FollowerInteractionWheelTemplate.Instantiate<UIFollowerInteractionWheelOverlayController>();
      followerInteraction.followerInteractionWheelInstance.Show(followerInteraction.follower, FollowerCommandGroups.GiveWorkerCommands(followerInteraction.follower), cancellable: false);
      UIFollowerInteractionWheelOverlayController interactionWheelInstance1 = followerInteraction.followerInteractionWheelInstance;
      interactionWheelInstance1.OnItemChosen = interactionWheelInstance1.OnItemChosen + new System.Action<FollowerCommands[]>(followerInteraction.OnFollowerCommandFinalized);
      CoopManager.Instance.OnPlayerLeft += new System.Action(followerInteraction.UIOnPlayerLeft);
      UIFollowerInteractionWheelOverlayController interactionWheelInstance2 = followerInteraction.followerInteractionWheelInstance;
      interactionWheelInstance2.OnHidden = interactionWheelInstance2.OnHidden + new System.Action(followerInteraction.\u003CSelectTaskRoutine\u003Eb__71_2);
    }
    else
    {
      if (followerInteraction.follower.Brain.HasTrait(FollowerTrait.TraitType.Mutated) && DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.MutatedFollower))
      {
        GameManager.GetInstance().OnConversationNew();
        MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.MutatedFollower, callback: new System.Action(followerInteraction.\u003CSelectTaskRoutine\u003Eb__71_1));
      }
      Debug.Log((object) "3 CALL BACK!");
      followerInteraction.Close();
    }
  }

  public void OnFollowerCommandFinalized(params FollowerCommands[] followerCommands)
  {
    if ((UnityEngine.Object) this.follower == (UnityEngine.Object) null || !this.follower.gameObject.activeInHierarchy)
    {
      this.Close(false, reshowMenu: false);
    }
    else
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
          if (FollowerBrainStats.IsNudism)
          {
            this.eventListener.PlayFollowerVO(this.negativeAcknowledgeVO);
            this.CloseAndSpeak("NudismIsActive");
            break;
          }
          if (this.follower.Brain.HasTrait(FollowerTrait.TraitType.JiltedLover))
          {
            this.eventListener.PlayFollowerVO(this.negativeAcknowledgeVO);
            this.CloseAndSpeak("JiltedLoverRefuseToWork");
            break;
          }
          goto case FollowerCommands.ChangeRole;
        case FollowerCommands.ChangeRole:
        case FollowerCommands.Talk:
          if (followerCommands.Length == 1 && followerCommands[0] == FollowerCommands.GiveWorkerCommand_2)
          {
            if (SeasonsManager.CurrentWeatherEvent == SeasonsManager.WeatherEvent.Blizzard && !this.follower.Brain.CanWork)
            {
              this.eventListener.PlayFollowerVO(this.negativeAcknowledgeVO);
              this.CloseAndSpeak("TooColdToWork");
              break;
            }
            if (SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter && (double) WarmthBar.WarmthNormalized <= 0.0)
            {
              this.eventListener.PlayFollowerVO(this.negativeAcknowledgeVO);
              this.CloseAndSpeak("TooColdToWork/Heket");
              break;
            }
            if (this.follower.Brain.Info.CursedState == Thought.Injured)
            {
              this.CloseAndSpeak("TooInjuredToWork");
              break;
            }
            this.CloseAndSpeak("NoTasksAvailable");
            break;
          }
          this.Close(true, reshowMenu: false);
          if (!this.follower.Brain.HasTrait(FollowerTrait.TraitType.Drowsy) || (double) UnityEngine.Random.value >= 0.89999997615814209 || this.follower.Brain.Info.FollowerRole != FollowerRole.Worker && this.follower.Brain.Info.FollowerRole != FollowerRole.Lumberjack && this.follower.Brain.Info.FollowerRole != FollowerRole.Bartender && this.follower.Brain.Info.FollowerRole != FollowerRole.Chef && this.follower.Brain.Info.FollowerRole != FollowerRole.Farmer && this.follower.Brain.Info.FollowerRole != FollowerRole.Forager && this.follower.Brain.Info.FollowerRole != FollowerRole.Janitor && this.follower.Brain.Info.FollowerRole != FollowerRole.Monk && this.follower.Brain.Info.FollowerRole != FollowerRole.Refiner && this.follower.Brain.Info.FollowerRole != FollowerRole.StoneMiner && this.follower.Brain.Info.FollowerRole != FollowerRole.Undertaker && this.follower.Brain.Info.FollowerRole != FollowerRole.Rancher && this.follower.Brain.Info.FollowerRole != FollowerRole.Medic && this.follower.Brain.Info.FollowerRole != FollowerRole.Worshipper)
            break;
          this.DoDrowsySleepAfterCommandGiven();
          break;
        case FollowerCommands.BedRest:
          if (this.follower.Brain.Info.CursedState == Thought.Ill)
          {
            this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
            List<Structures_HealingBay> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_HealingBay>(FollowerLocation.Base);
            bool flag = false;
            for (int index = structuresOfType.Count - 1; index >= 0; --index)
            {
              if (!structuresOfType[index].CheckIfOccupied() || structuresOfType[index].Data.FollowerID == this.follower.Brain.Info.ID)
              {
                flag = true;
                break;
              }
            }
            if (!flag && this.follower.Brain.HasHome && (UnityEngine.Object) Dwelling.GetDwellingByID(this.follower.Brain._directInfoAccess.DwellingID) != (UnityEngine.Object) null && Dwelling.GetDwellingByID(this.follower.Brain._directInfoAccess.DwellingID).StructureBrain.IsCollapsed)
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
          }
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
          break;
        case FollowerCommands.Dance:
          if (this.follower.Brain.Stats.Inspired)
          {
            this.eventListener.PlayFollowerVO(this.negativeAcknowledgeVO);
            this.CloseAndSpeak("AlreadyInspired");
            break;
          }
          this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
          this.follower.Brain.Stats.Inspired = true;
          this.Task.GoToAndStop(this.follower, this.playerFarming.transform.position + Vector3.left * ((double) this.follower.transform.position.x < (double) this.playerFarming.transform.position.x ? 1.5f : -1.5f), (System.Action) (() => this.StartCoroutine((IEnumerator) this.DanceRoutine(true))));
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
          this.playerFarming.PickUpFollower(this.follower);
          this.playerFarming.GoToAndStop(ClosestPrison.PrisonerLocation.gameObject, ClosestPrison.gameObject, true, true, (System.Action) (() =>
          {
            this.playerFarming.DropFollower();
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
              bool flag1 = false;
              foreach (ObjectivesData objective in DataManager.Instance.Objectives)
              {
                if (objective.Type == Objectives.TYPES.CUSTOM && ((Objectives_Custom) objective).CustomQuestType == Objectives.CustomQuestTypes.SendFollowerToPrison && ((Objectives_Custom) objective).TargetFollowerID == this.follower.Brain.Info.ID)
                {
                  flag1 = true;
                  break;
                }
              }
              bool flag2 = true;
              if (this.follower.Brain.HasTrait(FollowerTrait.TraitType.Spy))
              {
                flag2 = false;
              }
              else
              {
                foreach (Follower follower in Follower.Followers)
                {
                  if (follower.Brain._directInfoAccess.MurderedBy == this.follower.Brain.Info.ID)
                  {
                    flag2 = false;
                    break;
                  }
                }
              }
              if (!flag1 & flag2)
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
            int imprisonedDay = this.follower.Brain._directInfoAccess.ImprisonedDay;
            this.follower.Brain._directInfoAccess.ImprisonedDay = TimeManager.CurrentDay;
            ClosestPrison.StructureInfo.FollowerID = this.follower.Brain.Info.ID;
            ClosestPrison.StructureInfo.FollowerImprisonedTimestamp = TimeManager.TotalElapsedGameTime;
            ClosestPrison.StructureInfo.FollowerImprisonedFaith = this.follower.Brain.Stats.Reeducation;
            if (!DataManager.Instance.Followers_Imprisoned_IDs.Contains(this.follower.Brain.Info.ID))
              DataManager.Instance.Followers_Imprisoned_IDs.Add(this.follower.Brain.Info.ID);
            if (this.follower.Brain.Info.ID == 99996 && this.follower.Brain._directInfoAccess.SozoBrainshed && this.follower.Brain.Info.CursedState == Thought.Dissenter && TimeManager.CurrentDay - imprisonedDay >= 1)
            {
              List<ConversationEntry> Entries = new List<ConversationEntry>()
              {
                new ConversationEntry(this.gameObject, "Conversation_NPC/SozoFollower/Imprisoned/0")
              };
              foreach (ConversationEntry conversationEntry in Entries)
              {
                conversationEntry.CharacterName = ScriptLocalization.NAMES.Sozo;
                conversationEntry.DefaultAnimation = this.follower.Spine.AnimationState.GetCurrent(1).Animation.Name;
              }
              MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) (() =>
              {
                this.Close(false, reshowMenu: false);
                this.follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Prison/stocks");
              })));
            }
            else
              this.Close(false, reshowMenu: false);
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
        case FollowerCommands.Undertaker:
        case FollowerCommands.Brew:
        case FollowerCommands.Medic:
        case FollowerCommands.Rancher:
        case FollowerCommands.Logistics:
        case FollowerCommands.Handyman:
        case FollowerCommands.TraitManipulator:
        case FollowerCommands.MineRotstone:
          if (SeasonsManager.CurrentWeatherEvent == SeasonsManager.WeatherEvent.Blizzard && !this.follower.Brain.CanWork)
          {
            this.eventListener.PlayFollowerVO(this.negativeAcknowledgeVO);
            this.CloseAndSpeak("TooColdToWork");
            break;
          }
          this.ConvertToWorker();
          this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
          this.follower.Brain.Stats.WorkerBeenGivenOrders = true;
          FollowerRole followerRole = this.follower.Brain.Info.FollowerRole;
          FollowerRole NewRole = followerRole;
          switch (followerCommand)
          {
            case FollowerCommands.CutTrees:
              NewRole = FollowerRole.Lumberjack;
              break;
            case FollowerCommands.ForageBerries:
              NewRole = FollowerRole.Forager;
              break;
            case FollowerCommands.ClearRubble:
              NewRole = FollowerRole.StoneMiner;
              break;
            case FollowerCommands.WorshipAtShrine:
              NewRole = FollowerRole.Worshipper;
              if (followerRole != NewRole)
              {
                this.follower.Brain.CheckChangeTask();
                this.ShowDivineInspirationTutorialOnClose = true;
                break;
              }
              break;
            case FollowerCommands.Cook_2:
              NewRole = FollowerRole.Chef;
              break;
            case FollowerCommands.Farmer_2:
              NewRole = FollowerRole.Farmer;
              break;
            case FollowerCommands.CollectTax:
              for (int index = 0; index < this.follower.Brain._directInfoAccess.TaxCollected; ++index)
                ResourceCustomTarget.Create(this.playerFarming.gameObject, this.follower.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, (System.Action) null);
              AudioManager.Instance.PlayOneShot("event:/followers/pop_in", this.follower.transform.position);
              Inventory.AddItem(20, this.follower.Brain._directInfoAccess.TaxCollected);
              this.follower.Brain._directInfoAccess.TaxCollected = 0;
              break;
            case FollowerCommands.Janitor_2:
              NewRole = FollowerRole.Janitor;
              break;
            case FollowerCommands.Refiner_2:
              NewRole = FollowerRole.Refiner;
              break;
            case FollowerCommands.Undertaker:
              NewRole = FollowerRole.Undertaker;
              break;
            case FollowerCommands.Brew:
              NewRole = FollowerRole.Bartender;
              break;
            case FollowerCommands.Medic:
              NewRole = FollowerRole.Medic;
              break;
            case FollowerCommands.Rancher:
              NewRole = FollowerRole.Rancher;
              break;
            case FollowerCommands.Logistics:
              NewRole = FollowerRole.Logistics;
              break;
            case FollowerCommands.Handyman:
              NewRole = FollowerRole.Handyman;
              break;
            case FollowerCommands.TraitManipulator:
              NewRole = FollowerRole.TraitManipulator;
              break;
            case FollowerCommands.MineRotstone:
              NewRole = FollowerRole.RotstoneMiner;
              break;
          }
          if (NewRole != followerRole)
          {
            this.follower.Brain.NewRoleSet(NewRole);
            this.follower.Brain.SetPersonalOverrideTask(FollowerTask.GetFollowerTaskFromRole(NewRole));
            this.StartCoroutine((IEnumerator) this.FrameDelayCallback((System.Action) (() =>
            {
              if (this.follower.Brain.HasTrait(FollowerTrait.TraitType.Drowsy) && this.follower.Brain.CurrentTask.Type == FollowerTaskType.Sleep)
                return;
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
          this.Task.GoToAndStop(this.follower, this.playerFarming.transform.position + Vector3.left * ((double) this.follower.transform.position.x < (double) this.playerFarming.transform.position.x ? 1f : -1f), (System.Action) (() => this.StartCoroutine((IEnumerator) this.RomanceRoutine())));
          break;
        case FollowerCommands.WakeUp:
          if (SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter && this.follower.Brain.HasTrait(FollowerTrait.TraitType.Hibernation) && !this.follower.Brain._directInfoAccess.WorkThroughNight)
          {
            this.eventListener.PlayFollowerVO(this.negativeAcknowledgeVO);
            this.CloseAndSpeak("SleepUntilSummer");
            break;
          }
          if (SeasonsManager.CurrentSeason == SeasonsManager.Season.Spring && this.follower.Brain.HasTrait(FollowerTrait.TraitType.Aestivation) && !this.follower.Brain._directInfoAccess.WorkThroughNight)
          {
            this.eventListener.PlayFollowerVO(this.negativeAcknowledgeVO);
            this.CloseAndSpeak("SleepUntilWinter");
            break;
          }
          if (SeasonsManager.CurrentWeatherEvent == SeasonsManager.WeatherEvent.Blizzard && !this.follower.Brain.CanWork)
          {
            this.eventListener.PlayFollowerVO(this.negativeAcknowledgeVO);
            this.CloseAndSpeak("TooColdToWork");
            break;
          }
          this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
          if (this.previousTaskType == FollowerTaskType.Sleep && this.follower.Brain.CurrentOverrideTaskType == FollowerTaskType.Sleep || this.previousTaskType == FollowerTaskType.SleepBedRest && this.follower.Brain.CurrentOverrideTaskType == FollowerTaskType.SleepBedRest)
            this.follower.Brain.ClearPersonalOverrideTaskProvider();
          if (TimeManager.IsNight)
            this.follower.Brain.AddThought(Thought.SleepInterrupted);
          this.follower.Brain._directInfoAccess.BrainwashedUntil = TimeManager.CurrentDay;
          float faithMultiplier = this.follower.Brain.HasTrait(FollowerTrait.TraitType.Drowsy) ? 0.0f : 1f;
          if (TimeManager.IsNight)
            CultFaithManager.AddThought(Thought.Cult_WokeUpFollower, this.follower.Brain.Info.ID, faithMultiplier);
          this.Close(false, reshowMenu: false);
          this.enabled = true;
          this.follower.TimedAnimation("tantrum", 3.16666675f, (System.Action) (() =>
          {
            this.follower.Brain.CompleteCurrentTask();
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
          if (this.follower.Brain.Info.IsDrunk && this.previousTaskType != FollowerTaskType.Sleep)
          {
            CultFaithManager.AddThought(Thought.Cult_SentDrunkFollowerToSleep, this.follower.Brain.Info.ID);
            goto case FollowerCommands.ChangeRole;
          }
          goto case FollowerCommands.ChangeRole;
        case FollowerCommands.Build:
          int num2 = StructureManager.GetAllStructuresOfType<Structures_BuildSite>(in PlayerFarming.Location).Count + StructureManager.GetAllStructuresOfType<Structures_BuildSiteProject>(in PlayerFarming.Location).Count;
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
          this.Task.GoToAndStop(this.follower, this.playerFarming.transform.position + Vector3.left * ((double) this.follower.transform.position.x < (double) this.playerFarming.transform.position.x ? 1.5f : -1.5f), (System.Action) (() =>
          {
            switch (preFinalCommand)
            {
              case FollowerCommands.Murder:
                this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
                this.playerFarming.StartCoroutine((IEnumerator) this.MurderFollower());
                break;
              case FollowerCommands.Ascend:
                this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
                this.playerFarming.StartCoroutine((IEnumerator) this.AscendFollower());
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
          this.Task.GoToAndStop(this.follower, this.playerFarming.transform.position + Vector3.left * ((double) this.follower.transform.position.x < (double) this.playerFarming.transform.position.x ? 1.5f : -1.5f), (System.Action) (() => this.StartCoroutine((IEnumerator) this.IntimidateRoutine(true, this.playerFarming))));
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
            this.Close(true, reshowMenu: false);
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
          this.Task.GoToAndStop(this.follower, this.playerFarming.transform.position + Vector3.left * ((double) this.follower.transform.position.x < (double) this.playerFarming.transform.position.x ? 1.5f : -1.5f), (System.Action) (() => this.StartCoroutine((IEnumerator) this.BlessRoutine(true, this.playerFarming))));
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
          this.StartCoroutine((IEnumerator) this.RemoveNecklaceRoutine());
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
          this.Task.GoToAndStop(this.follower, this.playerFarming.transform.position + Vector3.left * ((double) this.follower.transform.position.x < (double) this.playerFarming.transform.position.x ? 1.5f : -1.5f), (System.Action) (() => this.StartCoroutine((IEnumerator) this.ReeducateRoutine())));
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
          this.Close(true, reshowMenu: false);
          MonoSingleton<UIManager>.Instance.ShowFollowerSummaryMenu(this.follower);
          break;
        case FollowerCommands.PetDog:
          if (this.follower.Brain.Stats.PetDog)
          {
            this.eventListener.PlayFollowerVO(this.negativeAcknowledgeVO);
            this.Close(true, reshowMenu: false);
            break;
          }
          this.follower.Brain.Stats.PetDog = true;
          this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
          this.Task.GoToAndStop(this.follower, this.playerFarming.transform.position + Vector3.left * ((double) this.follower.transform.position.x < (double) this.playerFarming.transform.position.x ? 1f : -1f), (System.Action) (() => this.StartCoroutine((IEnumerator) this.PetDogRoutine())));
          break;
        case FollowerCommands.Gift_Necklace_Light:
          this.StartCoroutine((IEnumerator) this.GiveItemRoutine(InventoryItem.ITEM_TYPE.Necklace_Light));
          break;
        case FollowerCommands.Gift_Necklace_Dark:
          this.StartCoroutine((IEnumerator) this.GiveItemRoutine(InventoryItem.ITEM_TYPE.Necklace_Dark));
          break;
        case FollowerCommands.Gift_Necklace_Missionary:
          this.StartCoroutine((IEnumerator) this.GiveItemRoutine(InventoryItem.ITEM_TYPE.Necklace_Missionary));
          break;
        case FollowerCommands.Gift_Necklace_Demonic:
          this.StartCoroutine((IEnumerator) this.GiveItemRoutine(InventoryItem.ITEM_TYPE.Necklace_Demonic));
          break;
        case FollowerCommands.Gift_Necklace_Loyalty:
          this.StartCoroutine((IEnumerator) this.GiveItemRoutine(InventoryItem.ITEM_TYPE.Necklace_Loyalty));
          break;
        case FollowerCommands.Gift_Necklace_Gold_Skull:
          this.StartCoroutine((IEnumerator) this.GiveItemRoutine(InventoryItem.ITEM_TYPE.Necklace_Gold_Skull));
          break;
        case FollowerCommands.GiveLeaderItem:
          FollowerLocation location = FollowerLocation.Dungeon1_1;
          InventoryItem.ITEM_TYPE item = InventoryItem.ITEM_TYPE.FLOWER_RED;
          if (this.follower.Brain.Info.ID == 99991)
          {
            location = FollowerLocation.Dungeon1_2;
            item = InventoryItem.ITEM_TYPE.MUSHROOM_SMALL;
          }
          else if (this.follower.Brain.Info.ID == 99992)
          {
            location = FollowerLocation.Dungeon1_3;
            item = InventoryItem.ITEM_TYPE.CRYSTAL;
          }
          else if (this.follower.Brain.Info.ID == 99993)
          {
            location = FollowerLocation.Dungeon1_4;
            item = InventoryItem.ITEM_TYPE.SPIDER_WEB;
          }
          else if (this.follower.Brain.Info.ID == 100007)
          {
            location = FollowerLocation.Dungeon1_5;
            item = InventoryItem.ITEM_TYPE.WOOL;
          }
          GameManager.GetInstance().OnConversationNew();
          GameManager.GetInstance().OnConversationNext(this.gameObject, 4f);
          DataManager.Instance.SecretItemsGivenToFollower.Add(location);
          GameManager.GetInstance().WaitForSeconds(1f, (System.Action) (() =>
          {
            Inventory.ChangeItemQuantity(item, -1);
            ResourceCustomTarget.Create(this.follower.gameObject, this.playerFarming.CameraBone.transform.position, item, (System.Action) (() =>
            {
              List<ConversationEntry> Entries = new List<ConversationEntry>()
              {
                new ConversationEntry(this.gameObject, $"Conversation_NPC/{location}/SecretQuest/0")
              };
              Entries[0].pitchValue = this.follower.Brain._directInfoAccess.follower_pitch;
              Entries[0].vibratoValue = this.follower.Brain._directInfoAccess.follower_vibrato;
              Entries[0].soundPath = this.generalTalkVO;
              Entries[0].SkeletonData = this.follower.Spine;
              Entries[0].CharacterName = $"<color=yellow>{this.follower.Brain.Info.Name}</color>";
              Entries[0].SetZoom = true;
              Entries[0].Zoom = 4f;
              int num4 = 0;
              while (LocalizationManager.GetTermData(ConversationEntry.Clone(Entries[0]).TermToSpeak.Replace("0", (++num4).ToString())) != null)
              {
                ConversationEntry conversationEntry = ConversationEntry.Clone(Entries[0]);
                conversationEntry.TermToSpeak = conversationEntry.TermToSpeak.Replace("0", num4.ToString());
                Entries.Add(conversationEntry);
              }
              foreach (ConversationEntry conversationEntry in Entries)
                conversationEntry.soundPath = this.PlayVO ? conversationEntry.soundPath : "";
              MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) (() => this.Close(true, reshowMenu: false))));
              MMConversation.PlayVO = this.PlayVO;
            }));
          }));
          break;
        case FollowerCommands.MealBurnt:
          this.follower.Brain.CancelTargetedMeal(StructureBrain.TYPES.MEAL_BURNED);
          this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
          this.follower.Brain.SetPersonalOverrideTask(FollowerTaskType.EatMeal, StructureBrain.TYPES.MEAL_BURNED);
          this.follower.Brain.CompleteCurrentTask();
          goto case FollowerCommands.ChangeRole;
        case FollowerCommands.Bully:
          if (this.follower.Brain.Stats.ScaredTraitInteracted)
          {
            this.eventListener.PlayFollowerVO(this.negativeAcknowledgeVO);
            this.Close(true, reshowMenu: false);
            break;
          }
          this.follower.Brain.Stats.ScaredTraitInteracted = true;
          this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
          this.playerFarming.GoToAndStop(this.follower.transform.position + Vector3.right * ((double) this.follower.transform.position.x < (double) this.playerFarming.transform.position.x ? 1f : -1f), GoToCallback: (System.Action) (() => this.StartCoroutine((IEnumerator) this.BullyRoutine())), forcePositionOnTimeout: true);
          break;
        case FollowerCommands.Reassure:
          if (this.follower.Brain.Stats.ScaredTraitInteracted)
          {
            this.eventListener.PlayFollowerVO(this.negativeAcknowledgeVO);
            this.Close(true, reshowMenu: false);
            break;
          }
          this.follower.Brain.Stats.ScaredTraitInteracted = true;
          this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
          this.playerFarming.GoToAndStop(this.follower.transform.position + Vector3.right * ((double) this.follower.transform.position.x < (double) this.playerFarming.transform.position.x ? 1f : -1f), GoToCallback: (System.Action) (() => this.StartCoroutine((IEnumerator) this.ReassureRoutine())), forcePositionOnTimeout: true);
          break;
        case FollowerCommands.MealEgg:
          this.follower.Brain.CancelTargetedMeal(StructureBrain.TYPES.MEAL_EGG);
          this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
          this.follower.Brain.SetPersonalOverrideTask(FollowerTaskType.EatMeal, StructureBrain.TYPES.MEAL_EGG);
          this.follower.Brain.CompleteCurrentTask();
          goto case FollowerCommands.ChangeRole;
        case FollowerCommands.DrinkSomething:
          this.eventListener.PlayFollowerVO(this.negativeAcknowledgeVO);
          this.CloseAndSpeak("NoDrinks");
          break;
        case FollowerCommands.DrinkBeer:
          this.follower.Brain.CancelTargetedDrink(InventoryItem.ITEM_TYPE.DRINK_BEER);
          this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
          this.follower.Brain.SetPersonalOverrideTask(FollowerTaskType.Drinking, StructureBrain.TYPES.DRINK_BEER);
          this.follower.Brain.CompleteCurrentTask();
          goto case FollowerCommands.ChangeRole;
        case FollowerCommands.DrinkGin:
          this.follower.Brain.CancelTargetedDrink(InventoryItem.ITEM_TYPE.DRINK_GIN);
          this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
          this.follower.Brain.SetPersonalOverrideTask(FollowerTaskType.Drinking, StructureBrain.TYPES.DRINK_GIN);
          this.follower.Brain.CompleteCurrentTask();
          goto case FollowerCommands.ChangeRole;
        case FollowerCommands.DrinkCocktail:
          this.follower.Brain.CancelTargetedDrink(InventoryItem.ITEM_TYPE.DRINK_COCKTAIL);
          this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
          this.follower.Brain.SetPersonalOverrideTask(FollowerTaskType.Drinking, StructureBrain.TYPES.DRINK_COCKTAIL);
          this.follower.Brain.CompleteCurrentTask();
          goto case FollowerCommands.ChangeRole;
        case FollowerCommands.DrinkWine:
          this.follower.Brain.CancelTargetedDrink(InventoryItem.ITEM_TYPE.DRINK_WINE);
          this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
          this.follower.Brain.SetPersonalOverrideTask(FollowerTaskType.Drinking, StructureBrain.TYPES.DRINK_WINE);
          this.follower.Brain.CompleteCurrentTask();
          goto case FollowerCommands.ChangeRole;
        case FollowerCommands.DrinkMushroomJuice:
          this.follower.Brain.CancelTargetedDrink(InventoryItem.ITEM_TYPE.DRINK_MUSHROOM_JUICE);
          this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
          this.follower.Brain.SetPersonalOverrideTask(FollowerTaskType.Drinking, StructureBrain.TYPES.DRINK_MUSHROOMJUICE);
          this.follower.Brain.CompleteCurrentTask();
          goto case FollowerCommands.ChangeRole;
        case FollowerCommands.DrinkPoopJuice:
          this.follower.Brain.CancelTargetedDrink(InventoryItem.ITEM_TYPE.DRINK_POOP_JUICE);
          this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
          this.follower.Brain.SetPersonalOverrideTask(FollowerTaskType.Drinking, StructureBrain.TYPES.DRINK_POOPJUICE);
          this.follower.Brain.CompleteCurrentTask();
          goto case FollowerCommands.ChangeRole;
        case FollowerCommands.DrinkEggnog:
          this.follower.Brain.CancelTargetedDrink(InventoryItem.ITEM_TYPE.DRINK_EGGNOG);
          this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
          this.follower.Brain.SetPersonalOverrideTask(FollowerTaskType.Drinking, StructureBrain.TYPES.DRINK_EGGNOG);
          this.follower.Brain.CompleteCurrentTask();
          goto case FollowerCommands.ChangeRole;
        case FollowerCommands.CuddleBaby:
          if (this.follower.Brain.Stats.Cuddled || this.follower.Brain.Info.Age >= 18)
          {
            this.eventListener.PlayFollowerVO(this.negativeAcknowledgeVO);
            this.Close(true, reshowMenu: false);
            break;
          }
          CultFaithManager.AddThought(Thought.ChildCuddle_0, this.follower.Brain.Info.ID);
          this.follower.Brain.AddThought((Thought) UnityEngine.Random.Range(393, 397));
          ++this.follower.Brain._directInfoAccess.CuddledAmount;
          this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
          if (!this.follower.Brain.Info.HasTrait(FollowerTrait.TraitType.Zombie))
            this.follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, this.follower.Brain.Info.Age < 10 ? "Baby/baby-crawl" : "Baby/baby-walk");
          if (this.Task != null)
            this.Task.GoToAndStop(this.follower, this.playerFarming.transform.position + Vector3.left * ((double) this.follower.transform.position.x < (double) this.playerFarming.transform.position.x ? 1f : -1f), (System.Action) null);
          GameManager.GetInstance().WaitForSeconds(this.Task != null ? 2f : 0.0f, (System.Action) (() => this.StartCoroutine((IEnumerator) this.CuddleBabyRoutine())));
          break;
        case FollowerCommands.HideNecklace:
        case FollowerCommands.ShowNecklace:
          this.follower.Brain.Info.ShowingNecklace = !this.follower.Brain.Info.ShowingNecklace;
          FollowerBrain.SetFollowerCostume(this.follower.Spine.Skeleton, this.follower.Brain._directInfoAccess, forceUpdate: true);
          this.Close(false, false);
          break;
        case FollowerCommands.SendToDaycare:
          if (this.follower.Brain.Info.Age >= 14)
          {
            this.eventListener.PlayFollowerVO(this.negativeAcknowledgeVO);
            this.CloseAndSpeak("CantSendToNursery");
            break;
          }
          BiomeConstants.Instance.DepthOfFieldTween(1.5f, 5f, 10f, 1f, 145f);
          this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
          double num5 = (double) this.follower.SetBodyAnimation("picked-up-hate", true);
          Interaction_Daycare ClosestDaycare = this.GetClosestDaycare();
          ClosestDaycare.Structure.Brain.Data.FollowerID = this.follower.Brain.Info.ID;
          this.playerFarming.PickUpFollower(this.follower);
          this.playerFarming.GoToAndStop(ClosestDaycare.MiddlePosition, ClosestDaycare.gameObject, true, true, (System.Action) (() =>
          {
            this.playerFarming.DropFollower();
            if (this.follower.Brain.HasTrait(FollowerTrait.TraitType.Zombie))
              this.follower.Brain.TransitionToTask((FollowerTask) new FollowerTask_ChildZombie(ClosestDaycare.Structure.Brain));
            else
              this.follower.Brain.TransitionToTask((FollowerTask) new FollowerTask_Child(ClosestDaycare.Structure.Brain));
            ClosestDaycare.Structure.Brain.Data.MultipleFollowerIDs.Add(this.follower.Brain.Info.ID);
            this.Interactable = false;
            this.follower.HideAllFollowerIcons();
            this.Close(false, reshowMenu: false);
          }), 30f);
          break;
        case FollowerCommands.PetFollower:
          if (this.follower.Brain.Stats.PetDog)
          {
            this.eventListener.PlayFollowerVO(this.negativeAcknowledgeVO);
            this.Close(true, reshowMenu: false);
            break;
          }
          this.follower.Brain.Stats.PetDog = true;
          this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
          if (!(bool) (UnityEngine.Object) this.playerFarming)
            this.playerFarming = PlayerFarming.FindClosestPlayer(this.transform.position);
          this.Task.GoToAndStop(this.follower, this.playerFarming.transform.position + Vector3.left * ((double) this.follower.transform.position.x < (double) this.playerFarming.transform.position.x ? 1f : -1f), (System.Action) (() => this.StartCoroutine((IEnumerator) this.PetDogRoutine())));
          break;
        case FollowerCommands.MealSpicy:
          this.follower.Brain.CancelTargetedMeal(StructureBrain.TYPES.MEAL_SPICY);
          this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
          this.follower.Brain.SetPersonalOverrideTask(FollowerTaskType.EatMeal, StructureBrain.TYPES.MEAL_SPICY);
          this.follower.Brain.CompleteCurrentTask();
          goto case FollowerCommands.ChangeRole;
        case FollowerCommands.MealSnowFruit:
          this.follower.Brain.CancelTargetedMeal(StructureBrain.TYPES.MEAL_SNOW_FRUIT);
          this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
          this.follower.Brain.SetPersonalOverrideTask(FollowerTaskType.EatMeal, StructureBrain.TYPES.MEAL_SNOW_FRUIT);
          this.follower.Brain.CompleteCurrentTask();
          goto case FollowerCommands.ChangeRole;
        case FollowerCommands.Gift_Necklace_Deaths_Door:
          this.StartCoroutine((IEnumerator) this.GiveItemRoutine(InventoryItem.ITEM_TYPE.Necklace_Deaths_Door));
          break;
        case FollowerCommands.Gift_Necklace_Winter:
          this.StartCoroutine((IEnumerator) this.GiveItemRoutine(InventoryItem.ITEM_TYPE.Necklace_Winter));
          break;
        case FollowerCommands.Gift_Necklace_Frozen:
          this.StartCoroutine((IEnumerator) this.GiveItemRoutine(InventoryItem.ITEM_TYPE.Necklace_Frozen));
          break;
        case FollowerCommands.Gift_Necklace_Weird:
          this.StartCoroutine((IEnumerator) this.GiveItemRoutine(InventoryItem.ITEM_TYPE.Necklace_Weird));
          break;
        case FollowerCommands.Gift_Necklace_Targeted:
          this.StartCoroutine((IEnumerator) this.GiveItemRoutine(InventoryItem.ITEM_TYPE.Necklace_Targeted));
          break;
        case FollowerCommands.DrinkChilli:
          this.follower.Brain.CancelTargetedDrink(InventoryItem.ITEM_TYPE.DRINK_CHILLI);
          this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
          this.follower.Brain.SetPersonalOverrideTask(FollowerTaskType.Drinking, StructureBrain.TYPES.DRINK_CHILLI);
          this.follower.Brain.CompleteCurrentTask();
          goto case FollowerCommands.ChangeRole;
        case FollowerCommands.DrinkLightning:
          this.follower.Brain.CancelTargetedDrink(InventoryItem.ITEM_TYPE.DRINK_LIGHTNING);
          this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
          this.follower.Brain.SetPersonalOverrideTask(FollowerTaskType.Drinking, StructureBrain.TYPES.DRINK_LIGHTNING);
          this.follower.Brain.CompleteCurrentTask();
          goto case FollowerCommands.ChangeRole;
        case FollowerCommands.DrinkSin:
          this.follower.Brain.CancelTargetedDrink(InventoryItem.ITEM_TYPE.DRINK_SIN);
          this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
          this.follower.Brain.SetPersonalOverrideTask(FollowerTaskType.Drinking, StructureBrain.TYPES.DRINK_SIN);
          this.follower.Brain.CompleteCurrentTask();
          goto case FollowerCommands.ChangeRole;
        case FollowerCommands.DrinkGrass:
          this.follower.Brain.CancelTargetedDrink(InventoryItem.ITEM_TYPE.DRINK_GRASS);
          this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
          this.follower.Brain.SetPersonalOverrideTask(FollowerTaskType.Drinking, StructureBrain.TYPES.DRINK_GRASS);
          this.follower.Brain.CompleteCurrentTask();
          goto case FollowerCommands.ChangeRole;
        case FollowerCommands.MealMilkBad:
          this.follower.Brain.CancelTargetedMeal(StructureBrain.TYPES.MEAL_MILK_BAD);
          this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
          this.follower.Brain.SetPersonalOverrideTask(FollowerTaskType.EatMeal, StructureBrain.TYPES.MEAL_MILK_BAD);
          this.follower.Brain.CompleteCurrentTask();
          goto case FollowerCommands.ChangeRole;
        case FollowerCommands.MealMilkGood:
          this.follower.Brain.CancelTargetedMeal(StructureBrain.TYPES.MEAL_MILK_GOOD);
          this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
          this.follower.Brain.SetPersonalOverrideTask(FollowerTaskType.EatMeal, StructureBrain.TYPES.MEAL_MILK_GOOD);
          this.follower.Brain.CompleteCurrentTask();
          goto case FollowerCommands.ChangeRole;
        case FollowerCommands.MealMilkGreat:
          this.follower.Brain.CancelTargetedMeal(StructureBrain.TYPES.MEAL_MILK_GREAT);
          this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
          this.follower.Brain.SetPersonalOverrideTask(FollowerTaskType.EatMeal, StructureBrain.TYPES.MEAL_MILK_GREAT);
          this.follower.Brain.CompleteCurrentTask();
          goto case FollowerCommands.ChangeRole;
        case FollowerCommands.Give_Goat_Pet:
          this.StartCoroutine((IEnumerator) this.GivePetRoutine(InventoryItem.ITEM_TYPE.ANIMAL_GOAT));
          break;
        case FollowerCommands.Give_Cow_Pet:
          this.StartCoroutine((IEnumerator) this.GivePetRoutine(InventoryItem.ITEM_TYPE.ANIMAL_COW));
          break;
        case FollowerCommands.Give_Spider_Pet:
          this.StartCoroutine((IEnumerator) this.GivePetRoutine(InventoryItem.ITEM_TYPE.ANIMAL_SPIDER));
          break;
        case FollowerCommands.Give_Llama_Pet:
          this.StartCoroutine((IEnumerator) this.GivePetRoutine(InventoryItem.ITEM_TYPE.ANIMAL_LLAMA));
          break;
        case FollowerCommands.Give_Crab_Pet:
          this.StartCoroutine((IEnumerator) this.GivePetRoutine(InventoryItem.ITEM_TYPE.ANIMAL_CRAB));
          break;
        case FollowerCommands.Give_Snail_Pet:
          this.StartCoroutine((IEnumerator) this.GivePetRoutine(InventoryItem.ITEM_TYPE.ANIMAL_SNAIL));
          break;
        case FollowerCommands.Give_Turtle_Pet:
          this.StartCoroutine((IEnumerator) this.GivePetRoutine(InventoryItem.ITEM_TYPE.ANIMAL_TURTLE));
          break;
        case FollowerCommands.DrinkMilkshake:
          this.follower.Brain.CancelTargetedDrink(InventoryItem.ITEM_TYPE.DRINK_MILKSHAKE);
          this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
          this.follower.Brain.SetPersonalOverrideTask(FollowerTaskType.Drinking, StructureBrain.TYPES.DRINK_MILKSHAKE);
          this.follower.Brain.CompleteCurrentTask();
          goto case FollowerCommands.ChangeRole;
        default:
          Debug.Log((object) $"Warning! Unhandled Follower Command: {followerCommand}".Colour(Color.red));
          goto case FollowerCommands.ChangeRole;
      }
    }
  }

  public IEnumerator ExtortMoneyRoutine()
  {
    interaction_FollowerInteraction followerInteraction = this;
    yield return (object) new WaitForEndOfFrame();
    followerInteraction.follower.FacePosition(followerInteraction.playerFarming.transform.position);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(followerInteraction.gameObject, 4f);
    yield return (object) new WaitForSeconds(0.25f);
    for (int i = 0; i < UnityEngine.Random.Range(3, 7); ++i)
    {
      ResourceCustomTarget.Create(followerInteraction.state.gameObject, followerInteraction.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, (System.Action) (() => Inventory.AddItem(20, 1)));
      yield return (object) new WaitForSeconds(0.1f);
    }
    yield return (object) new WaitForSeconds(0.25f);
    GameManager.GetInstance().OnConversationEnd();
    followerInteraction.Close();
  }

  public IEnumerator FrameDelayCallback(System.Action callback)
  {
    yield return (object) new WaitForEndOfFrame();
    System.Action action = callback;
    if (action != null)
      action();
  }

  public IEnumerator DelayCallback(float delay, System.Action callback)
  {
    yield return (object) new WaitForSeconds(delay);
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void DoDrowsySleepAfterCommandGiven()
  {
    NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("Notifications/DrowsyFellAsleep", this.follower.Brain.Info.Name);
    this.follower.Brain.SetPersonalOverrideTask(FollowerTaskType.Sleep);
    this.follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_Sleep());
  }

  public Prison GetClosestPrison()
  {
    Prison closestPrison = (Prison) null;
    float num1 = float.MaxValue;
    foreach (Prison prison in Prison.Prisons)
    {
      if (prison.StructureInfo.FollowerID == -1 && !prison.Structure.Structure_Info.IsCollapsed)
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

  public Interaction_Daycare GetClosestDaycare()
  {
    Interaction_Daycare closestDaycare = (Interaction_Daycare) null;
    float num1 = float.MaxValue;
    foreach (Interaction_Daycare daycare in Interaction_Daycare.Daycares)
    {
      if (!daycare.Structure.Brain.IsFull)
      {
        float num2 = Vector2.Distance((Vector2) this.state.transform.position, (Vector2) daycare.transform.position);
        if ((double) num2 < (double) num1)
        {
          closestDaycare = daycare;
          num1 = num2;
        }
      }
    }
    return closestDaycare;
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

  public IEnumerator MurderFollower()
  {
    interaction_FollowerInteraction followerInteraction = this;
    followerInteraction.follower.HideAllFollowerIcons();
    followerInteraction.follower.FacePosition(followerInteraction.playerFarming.transform.position);
    followerInteraction.follower.Brain.DiedFromMurder = true;
    followerInteraction.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) null;
    followerInteraction.playerFarming.Spine.AnimationState.SetAnimation(0, "murder", false);
    followerInteraction.playerFarming.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    if (followerInteraction._playerFarming.isLamb)
      AudioManager.Instance.PlayOneShot("event:/player/speak_to_follower");
    else
      AudioManager.Instance.PlayOneShot("event:/player/goat_player/goat_speak_to_follower");
    AudioManager.Instance.PlayOneShot("event:/player/murder_follower_sequence");
    double num = (double) followerInteraction.follower.SetBodyAnimation("murder", false);
    float Duration = followerInteraction.follower.Spine.AnimationState.GetCurrent(1).Animation.Duration;
    GameManager.GetInstance().AddToCamera(followerInteraction.follower.gameObject);
    yield return (object) new WaitForSeconds(0.1f);
    followerInteraction.follower.Spine.CustomMaterialOverride.Clear();
    followerInteraction.follower.Spine.CustomMaterialOverride.Add(followerInteraction.follower.NormalMaterial, followerInteraction.follower.BW_Material);
    followerInteraction.playerFarming.Spine.CustomMaterialOverride.Clear();
    followerInteraction.playerFarming.Spine.CustomMaterialOverride.Add(followerInteraction.playerFarming.originalMaterial, followerInteraction.playerFarming.BW_Material);
    HUD_Manager.Instance.ShowBW(0.33f, 0.0f, 1f);
    BiomeConstants.Instance.EmitSmokeInteractionVFX(followerInteraction.follower.transform.position, new Vector3(0.5f, 0.5f, 1f));
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    yield return (object) new WaitForSeconds(1.6f);
    BiomeConstants.Instance.EmitSmokeInteractionVFX(followerInteraction.follower.transform.position, new Vector3(1f, 1f, 1f));
    CameraManager.instance.ShakeCameraForDuration(0.8f, 1f, 0.3f);
    followerInteraction.playerFarming.Spine.CustomMaterialOverride.Clear();
    followerInteraction.follower.Spine.CustomMaterialOverride.Clear();
    HUD_Manager.Instance.ShowBW(0.33f, 1f, 0.0f);
    yield return (object) new WaitForSeconds((float) ((double) Duration - 0.10000000149011612 - 1.7000000476837158));
    JudgementMeter.ShowModify(-1);
    int scaleX = (int) followerInteraction.follower.Spine.Skeleton.ScaleX;
    followerInteraction.Close(false, reshowMenu: false);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.MurderFollower, followerInteraction.follower.Brain.Info.ID);
    if (TimeManager.CurrentPhase == DayPhase.Night)
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.MurderFollowerAtNight, followerInteraction.follower.Brain.Info.ID);
    followerInteraction.follower.Die(NotificationCentre.NotificationType.MurderedByYou, false, scaleX);
    ++DataManager.Instance.STATS_Murders;
  }

  public IEnumerator AscendFollower()
  {
    interaction_FollowerInteraction followerInteraction = this;
    followerInteraction.follower.FacePosition(followerInteraction.playerFarming.transform.position);
    followerInteraction.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) null;
    followerInteraction.playerFarming.Spine.AnimationState.SetAnimation(0, "resurrect", false);
    followerInteraction.playerFarming.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    float seconds = followerInteraction.follower.SetBodyAnimation("sacrifice", false);
    GameManager.GetInstance().AddToCamera(followerInteraction.follower.gameObject);
    followerInteraction.follower.Spine.CustomMaterialOverride.Clear();
    followerInteraction.follower.Spine.CustomMaterialOverride.Add(followerInteraction.follower.NormalMaterial, followerInteraction.follower.BW_Material);
    followerInteraction.playerFarming.Spine.CustomMaterialOverride.Clear();
    followerInteraction.playerFarming.Spine.CustomMaterialOverride.Add(followerInteraction.playerFarming.originalMaterial, followerInteraction.playerFarming.BW_Material);
    HUD_Manager.Instance.ShowBW(0.33f, 0.0f, 1f);
    yield return (object) new WaitForSeconds(seconds);
    followerInteraction.playerFarming.Spine.CustomMaterialOverride.Clear();
    followerInteraction.follower.Spine.CustomMaterialOverride.Clear();
    HUD_Manager.Instance.ShowBW(0.33f, 1f, 0.0f);
    followerInteraction.Close();
    followerInteraction.follower.Die(NotificationCentre.NotificationType.Ascended, false);
  }

  public IEnumerator IntimidateRoutine(bool hostFollower, PlayerFarming playerToFace)
  {
    interaction_FollowerInteraction followerInteraction = this;
    if (hostFollower)
    {
      float num = 3f;
      List<Follower> followerList = new List<Follower>();
      foreach (Follower follower in Follower.Followers)
      {
        if ((UnityEngine.Object) follower != (UnityEngine.Object) followerInteraction.follower && follower.Brain.Info.CursedState != Thought.Dissenter && !FollowerManager.FollowerLocked(follower.Brain.Info.ID) && (double) Vector3.Distance(follower.transform.position, followerInteraction.transform.position) < (double) num && !follower.Brain.Stats.Intimidated && follower.Brain.CurrentTaskType != FollowerTaskType.Sleep && !follower.Brain.HasTrait(FollowerTrait.TraitType.Mutated))
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
          follower.StartCoroutine((IEnumerator) follower.GetComponent<interaction_FollowerInteraction>().IntimidateRoutine(false, playerToFace));
        }));
      }
      GameManager.GetInstance().OnConversationNext(followerInteraction.gameObject, 4f);
      GameManager.GetInstance().AddPlayerToCamera();
      GameManager.GetInstance().CameraSetOffset(Vector3.zero);
      if (followerList.Count > 0)
        yield return (object) new WaitForSeconds(0.35f);
      playerToFace.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      if (playerToFace.isLamb)
        AudioManager.Instance.PlayOneShot("event:/player/speak_to_follower", playerToFace.transform.position);
      else
        AudioManager.Instance.PlayOneShot("event:/player/goat_player/goat_speak_to_follower", playerToFace.transform.position);
      AudioManager.Instance.PlayOneShot("event:/Stings/white_eyes", playerToFace.transform.position);
      HUD_Manager.Instance.ShowBW(0.33f, 0.0f, 1f);
    }
    followerInteraction.follower.FacePosition(playerToFace.transform.position);
    followerInteraction.follower.Spine.CustomMaterialOverride.Clear();
    followerInteraction.follower.Spine.CustomMaterialOverride.Add(followerInteraction.follower.NormalMaterial, followerInteraction.follower.BW_Material);
    if (hostFollower)
    {
      playerToFace.Spine.CustomMaterialOverride.Clear();
      playerToFace.Spine.CustomMaterialOverride.Add(playerToFace.originalMaterial, playerToFace.BW_Material);
    }
    followerInteraction.follower.State.CURRENT_STATE = StateMachine.State.Dancing;
    yield return (object) null;
    float num1 = followerInteraction.follower.SetBodyAnimation("Reactions/react-intimidate", false);
    followerInteraction.follower.AddBodyAnimation("idle", true, 0.0f);
    if (hostFollower)
    {
      playerToFace.Spine.AnimationState.SetAnimation(0, "intimidate", false);
      playerToFace.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
      AudioManager.Instance.PlayOneShot("event:/player/intimidate_follower", followerInteraction.playerFarming.gameObject);
    }
    yield return (object) new WaitForSeconds(num1 - 2.25f);
    playerToFace.Spine.CustomMaterialOverride.Clear();
    followerInteraction.follower.Spine.CustomMaterialOverride.Clear();
    if (hostFollower)
    {
      playerToFace.Spine.CustomMaterialOverride.Clear();
      playerToFace.Spine.CustomMaterialOverride.Add(playerToFace.originalMaterial, playerToFace.originalMaterial);
      HUD_Manager.Instance.ShowBW(0.33f, 1f, 0.0f);
      yield return (object) new WaitForSeconds(0.5f);
      AudioManager.Instance.PlayOneShot("event:/followers/gain_loyalty", followerInteraction.gameObject.transform.position);
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.BlessAFollower);
    }
    else
      yield return (object) new WaitForSeconds(0.5f);
    if (hostFollower && (double) UnityEngine.Random.value < 0.05000000074505806 && !followerInteraction.follower.Brain.HasTrait(FollowerTrait.TraitType.Scared) && !followerInteraction.follower.Brain.HasTrait(FollowerTrait.TraitType.CriminalScarred))
      followerInteraction.follower.AddTrait(FollowerTrait.TraitType.Scared, true);
    followerInteraction.follower.Brain.AddAdoration(FollowerBrain.AdorationActions.Intimidate, (System.Action) (() =>
    {
      this.follower.Brain.AddThought(Thought.Intimidated);
      if (hostFollower)
      {
        if (this.follower.Brain.CanLevelUp())
          this.StartCoroutine((IEnumerator) this.LevelUpRoutine(this.previousTaskType));
        else
          this.Close();
      }
      else
        this.follower.Brain.CompleteCurrentTask();
    }));
  }

  public IEnumerator BlessRoutine(bool hostFollower, PlayerFarming playerToFace)
  {
    interaction_FollowerInteraction followerInteraction = this;
    if (hostFollower)
    {
      float num = 3f;
      List<Follower> followerList = new List<Follower>();
      foreach (Follower follower in Follower.Followers)
      {
        if ((UnityEngine.Object) follower != (UnityEngine.Object) followerInteraction.follower && follower.Brain.Info.CursedState != Thought.Dissenter && !FollowerManager.FollowerLocked(follower.Brain.Info.ID) && (double) Vector3.Distance(follower.transform.position, followerInteraction.transform.position) < (double) num && !follower.Brain.Stats.ReceivedBlessing && follower.Brain.CurrentTaskType != FollowerTaskType.Sleep && !follower.Brain.HasTrait(FollowerTrait.TraitType.Mutated))
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
          follower.StartCoroutine((IEnumerator) follower.GetComponent<interaction_FollowerInteraction>().BlessRoutine(false, playerToFace));
        }));
      }
      GameManager.GetInstance().OnConversationNext(followerInteraction.gameObject, 4f);
      GameManager.GetInstance().AddPlayerToCamera();
      GameManager.GetInstance().CameraSetOffset(Vector3.zero);
      if (followerList.Count > 0)
        yield return (object) new WaitForSeconds(0.35f);
      playerToFace.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      if (playerToFace.isLamb)
        AudioManager.Instance.PlayOneShot("event:/player/speak_to_follower", playerToFace.transform.position);
      else
        AudioManager.Instance.PlayOneShot("event:/player/goat_player/goat_speak_to_follower", playerToFace.transform.position);
      AudioManager.Instance.PlayOneShot("event:/Stings/white_eyes", playerToFace.transform.position);
    }
    followerInteraction.follower.FacePosition(playerToFace.transform.position);
    followerInteraction.follower.State.CURRENT_STATE = StateMachine.State.Dancing;
    yield return (object) null;
    double num1 = (double) followerInteraction.follower.SetBodyAnimation("devotion/devotion-start", false);
    followerInteraction.follower.AddBodyAnimation("devotion/devotion-waiting", true, 0.0f);
    if (hostFollower)
    {
      yield return (object) playerToFace.Spine.YieldForAnimation("bless");
      followerInteraction.playerFarming.simpleSpineAnimator.Animate("idle", 0, true, 0.0f);
    }
    else
      yield return (object) new WaitForSeconds(1.25f);
    double num2 = (double) followerInteraction.follower.SetBodyAnimation("idle", true);
    yield return (object) new WaitForSeconds(0.5f);
    AudioManager.Instance.PlayOneShot("event:/followers/gain_loyalty", followerInteraction.gameObject.transform.position);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.BlessAFollower);
    followerInteraction.follower.Brain.AddAdoration(FollowerBrain.AdorationActions.Bless, (System.Action) (() =>
    {
      CultFaithManager.AddThought(Thought.Cult_Bless, this.follower.Brain.Info.ID);
      if (hostFollower)
      {
        if (this.follower.Brain.CanLevelUp())
          this.StartCoroutine((IEnumerator) this.LevelUpRoutine(this.previousTaskType));
        else
          this.Close();
      }
      else
        this.follower.Brain.CompleteCurrentTask();
    }));
  }

  public IEnumerator ReeducateRoutine()
  {
    interaction_FollowerInteraction followerInteraction = this;
    followerInteraction.follower.FacePosition(followerInteraction.playerFarming.transform.position);
    GameManager.GetInstance().OnConversationNext(followerInteraction.gameObject, 4f);
    GameManager.GetInstance().AddPlayerToCamera();
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    followerInteraction.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    followerInteraction.follower.State.CURRENT_STATE = StateMachine.State.Dancing;
    yield return (object) null;
    int num1 = 1;
    if (((double) followerInteraction.follower.Brain.Stats.Reeducation + 7.5) / 100.0 >= 1.0)
      num1 = 3;
    else if (((double) followerInteraction.follower.Brain.Stats.Reeducation + 7.5) / 100.0 > 0.5)
      num1 = 2;
    followerInteraction.playerFarming.simpleSpineAnimator.Animate("reeducate-" + num1.ToString(), 0, false);
    followerInteraction.playerFarming.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    if (followerInteraction._playerFarming.isLamb)
      AudioManager.Instance.PlayOneShot("event:/player/speak_to_follower", followerInteraction.playerFarming.transform.position);
    else
      AudioManager.Instance.PlayOneShot("event:/player/goat_player/goat_speak_to_follower", followerInteraction.playerFarming.transform.position);
    double num2 = (double) followerInteraction.follower.SetBodyAnimation("reeducate-" + num1.ToString(), false);
    followerInteraction.follower.AddBodyAnimation("idle", true, 0.0f);
    yield return (object) new WaitForSeconds(0.5f);
    yield return (object) new WaitForSeconds(1.5f);
    followerInteraction.follower.Brain.Stats.Reeducation -= 7.5f;
    if ((double) followerInteraction.follower.Brain.Stats.Reeducation > 0.0 && (double) followerInteraction.follower.Brain.Stats.Reeducation < 4.0)
      followerInteraction.follower.Brain.Stats.Reeducation = 0.0f;
    AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", followerInteraction.gameObject.transform.position);
    BiomeConstants.Instance.EmitHeartPickUpVFX(followerInteraction.gameObject.transform.position, 0.0f, "black", "burst_big", false);
    yield return (object) new WaitForSeconds(1f);
    if (followerInteraction.follower.Brain.Info.CursedState == Thought.None)
      followerInteraction.follower.Brain.AddPleasure(FollowerBrain.PleasureActions.RemovedDissent);
    if ((double) followerInteraction.follower.Brain.Stats.Reeducation <= 100.0)
    {
      followerInteraction.follower.TimedAnimation("Reactions/react-enlightened1", 2f, new System.Action(followerInteraction.\u003CReeducateRoutine\u003Eb__87_0));
    }
    else
    {
      yield return (object) new WaitForSeconds(0.5f);
      followerInteraction.Close();
    }
  }

  public IEnumerator RomanceRoutine(bool reshowMenu = true)
  {
    interaction_FollowerInteraction followerInteraction = this;
    followerInteraction.follower.FacePosition(followerInteraction.playerFarming.transform.position);
    followerInteraction.playerFarming.state.facingAngle = Utils.GetAngle(followerInteraction.playerFarming.transform.position, followerInteraction.transform.position);
    GameManager.GetInstance().OnConversationNext(followerInteraction.gameObject, 4f);
    GameManager.GetInstance().AddPlayerToCamera();
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    followerInteraction.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    followerInteraction.follower.State.CURRENT_STATE = StateMachine.State.Dancing;
    yield return (object) null;
    followerInteraction.playerFarming.Spine.AnimationState.SetAnimation(0, "kiss-follower", false);
    followerInteraction.playerFarming.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    double num = (double) followerInteraction.follower.SetBodyAnimation("kiss", true);
    followerInteraction.follower.AddBodyAnimation("idle", true, 0.0f);
    yield return (object) new WaitForSeconds(0.8333333f);
    AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", followerInteraction.gameObject.transform.position);
    BiomeConstants.Instance.EmitHeartPickUpVFX(followerInteraction.transform.position, 0.0f, "red", "burst_big", false);
    followerInteraction.follower.Brain.AddThought(Thought.SpouseKiss);
    yield return (object) new WaitForSeconds(3f);
    if (followerInteraction.follower.Brain.Info.HasTrait(FollowerTrait.TraitType.Zombie))
    {
      followerInteraction.playerFarming.Spine.AnimationState.SetAnimation(0, "eat-react-bad", false);
      followerInteraction.playerFarming.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
      yield return (object) new WaitForSeconds(0.233333334f);
      AudioManager.Instance.PlayOneShot("event:/dialogue/followers/hold_back_vom", followerInteraction.gameObject);
      yield return (object) new WaitForSeconds(0.733333349f);
      AudioManager.Instance.PlayOneShot("event:/dialogue/followers/vom", followerInteraction.gameObject);
      yield return (object) new WaitForSeconds(2f);
    }
    followerInteraction.playerFarming.Spine.CustomMaterialOverride.Clear();
    followerInteraction.follower.Spine.CustomMaterialOverride.Clear();
    ++followerInteraction.follower.Brain._directInfoAccess.SmoochCount;
    if (followerInteraction.follower.Brain._directInfoAccess.SmoochCount >= 3 && followerInteraction.follower.Brain.HasTrait(FollowerTrait.TraitType.MarriedUnhappily))
    {
      followerInteraction.follower.Brain.RemoveTrait(FollowerTrait.TraitType.MarriedUnhappily, true);
      followerInteraction.follower.Brain._directInfoAccess.SmoochCount = 0;
    }
    else if (followerInteraction.follower.Brain._directInfoAccess.SmoochCount >= 3 && !followerInteraction.follower.Brain.HasTrait(FollowerTrait.TraitType.MarriedHappily))
    {
      followerInteraction.follower.Brain.AddTrait(FollowerTrait.TraitType.MarriedHappily, true);
      followerInteraction.follower.Brain._directInfoAccess.SmoochCount = 0;
    }
    AudioManager.Instance.PlayOneShot("event:/followers/gain_loyalty", followerInteraction.gameObject.transform.position);
    followerInteraction.follower.Brain.AddAdoration(FollowerBrain.AdorationActions.SmoochSpouse, (System.Action) (() =>
    {
      if (this.follower.Brain.CanLevelUp())
        this.StartCoroutine((IEnumerator) this.LevelUpRoutine(this.previousTaskType));
      else
        this.Close(true, reshowMenu: reshowMenu);
    }));
  }

  public IEnumerator CuddleBabyRoutine()
  {
    interaction_FollowerInteraction followerInteraction = this;
    followerInteraction.follower.FacePosition(followerInteraction.playerFarming.transform.position);
    followerInteraction.playerFarming.state.facingAngle = Utils.GetAngle(followerInteraction.playerFarming.transform.position, followerInteraction.transform.position);
    GameManager.GetInstance().OnConversationNext(followerInteraction.gameObject, 4f);
    GameManager.GetInstance().AddPlayerToCamera();
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    followerInteraction.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    followerInteraction.follower.State.CURRENT_STATE = StateMachine.State.Dancing;
    yield return (object) null;
    followerInteraction.playerFarming.Spine.AnimationState.SetAnimation(0, "pet-dog", false);
    followerInteraction.playerFarming.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    double num = (double) followerInteraction.follower.SetBodyAnimation("pet-dog", true);
    followerInteraction.follower.AddBodyAnimation("idle", true, 0.0f);
    yield return (object) new WaitForSeconds(0.8333333f);
    AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", followerInteraction.gameObject.transform.position);
    BiomeConstants.Instance.EmitHeartPickUpVFX(followerInteraction.transform.position, 0.0f, "red", "burst_big", false);
    followerInteraction.follower.Brain.Stats.Cuddled = true;
    yield return (object) new WaitForSeconds(1f);
    followerInteraction.playerFarming.Spine.CustomMaterialOverride.Clear();
    followerInteraction.follower.Spine.CustomMaterialOverride.Clear();
    yield return (object) new WaitForSeconds(0.5f);
    followerInteraction.follower.Brain.AddAdoration(FollowerBrain.AdorationActions.CuddleBaby, new System.Action(followerInteraction.\u003CCuddleBabyRoutine\u003Eb__89_0));
  }

  public IEnumerator PetDogRoutine()
  {
    interaction_FollowerInteraction followerInteraction = this;
    followerInteraction.follower.FacePosition(followerInteraction.playerFarming.transform.position);
    followerInteraction.playerFarming.state.facingAngle = Utils.GetAngle(followerInteraction.playerFarming.transform.position, followerInteraction.transform.position);
    GameManager.GetInstance().OnConversationNext(followerInteraction.gameObject, 4f);
    GameManager.GetInstance().AddPlayerToCamera();
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    followerInteraction.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    followerInteraction.follower.State.CURRENT_STATE = StateMachine.State.Dancing;
    yield return (object) null;
    followerInteraction.playerFarming.Spine.AnimationState.SetAnimation(0, "pet-dog", false);
    followerInteraction.playerFarming.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    double num = (double) followerInteraction.follower.SetBodyAnimation("pet-dog", true);
    followerInteraction.follower.AddBodyAnimation("idle", true, 0.0f);
    yield return (object) new WaitForSeconds(0.8333333f);
    AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", followerInteraction.gameObject.transform.position);
    BiomeConstants.Instance.EmitHeartPickUpVFX(followerInteraction.transform.position, 0.0f, "red", "burst_big", false);
    yield return (object) new WaitForSeconds(1f);
    followerInteraction.playerFarming.Spine.CustomMaterialOverride.Clear();
    followerInteraction.follower.Spine.CustomMaterialOverride.Clear();
    yield return (object) new WaitForSeconds(0.5f);
    followerInteraction.follower.Brain.AddAdoration(FollowerBrain.AdorationActions.PetDog, new System.Action(followerInteraction.\u003CPetDogRoutine\u003Eb__90_0));
  }

  public IEnumerator ReassureRoutine()
  {
    interaction_FollowerInteraction followerInteraction = this;
    followerInteraction.follower.FacePosition(followerInteraction.playerFarming.transform.position);
    followerInteraction.playerFarming.state.facingAngle = Utils.GetAngle(followerInteraction.playerFarming.transform.position, followerInteraction.transform.position);
    GameManager.GetInstance().OnConversationNext(followerInteraction.gameObject, 4f);
    GameManager.GetInstance().AddPlayerToCamera();
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    followerInteraction.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    followerInteraction.follower.State.CURRENT_STATE = StateMachine.State.Dancing;
    yield return (object) null;
    followerInteraction.playerFarming.Spine.AnimationState.SetAnimation(0, "bully/reassure", false);
    followerInteraction.playerFarming.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    double num = (double) followerInteraction.follower.SetBodyAnimation("Scared/scared-reassured", true);
    followerInteraction.follower.AddBodyAnimation("idle", true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/player/reassure_baah", followerInteraction.playerFarming.gameObject);
    yield return (object) new WaitForSeconds(0.8333333f);
    AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", followerInteraction.gameObject.transform.position);
    BiomeConstants.Instance.EmitHeartPickUpVFX(followerInteraction.transform.position, 0.0f, "red", "burst_big", false);
    yield return (object) new WaitForSeconds(1f);
    yield return (object) new WaitForSeconds(0.5f);
    followerInteraction.follower.Brain._directInfoAccess.ReassuranceCount = Mathf.Clamp(++followerInteraction.follower.Brain._directInfoAccess.ReassuranceCount, 1, 3);
    if (followerInteraction.follower.Brain._directInfoAccess.ReassuranceCount >= 3)
    {
      followerInteraction.follower.Brain._directInfoAccess.ReassuranceCount = 0;
      if (followerInteraction.follower.Brain.HasTrait(FollowerTrait.TraitType.Terrified))
        followerInteraction.follower.Brain.RemoveTrait(FollowerTrait.TraitType.Terrified, true);
      if (followerInteraction.follower.Brain.HasTrait(FollowerTrait.TraitType.Scared))
        followerInteraction.follower.Brain.RemoveTrait(FollowerTrait.TraitType.Scared, true);
      if (followerInteraction.follower.Brain.HasTrait(FollowerTrait.TraitType.CriminalScarred))
        followerInteraction.follower.Brain.RemoveTrait(FollowerTrait.TraitType.CriminalScarred, true);
      if (followerInteraction.follower.Brain.Info.HasTrait(FollowerTrait.TraitType.MissionaryTerrified))
        followerInteraction.follower.Brain.RemoveTrait(FollowerTrait.TraitType.MissionaryTerrified, true);
      if (followerInteraction.follower.Brain.Info.HasTrait(FollowerTrait.TraitType.Bastard))
        followerInteraction.follower.Brain.RemoveTrait(FollowerTrait.TraitType.Bastard, true);
      if (followerInteraction.follower.Brain.Info.HasTrait(FollowerTrait.TraitType.CriminalHardened))
        followerInteraction.follower.Brain.RemoveTrait(FollowerTrait.TraitType.CriminalHardened, true);
    }
    followerInteraction.follower.Brain.AddAdoration(FollowerBrain.AdorationActions.Reassure, new System.Action(followerInteraction.\u003CReassureRoutine\u003Eb__91_0));
  }

  public IEnumerator BullyRoutine()
  {
    interaction_FollowerInteraction followerInteraction = this;
    GameManager.GetInstance().OnConversationNext(followerInteraction.gameObject, 4f);
    GameManager.GetInstance().AddPlayerToCamera();
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    followerInteraction.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    if (followerInteraction._playerFarming.isLamb)
      AudioManager.Instance.PlayOneShot("event:/player/speak_to_follower", followerInteraction.playerFarming.transform.position);
    else
      AudioManager.Instance.PlayOneShot("event:/player/goat_player/goat_speak_to_follower", followerInteraction.playerFarming.transform.position);
    AudioManager.Instance.PlayOneShot("event:/Stings/white_eyes", followerInteraction.playerFarming.transform.position);
    followerInteraction.follower.FacePosition(followerInteraction.playerFarming.transform.position);
    followerInteraction.follower.State.CURRENT_STATE = StateMachine.State.Dancing;
    yield return (object) null;
    float num = followerInteraction.follower.SetBodyAnimation("Scared/scared-bullied", false);
    followerInteraction.follower.AddBodyAnimation("idle", true, 0.0f);
    followerInteraction.playerFarming.Spine.AnimationState.SetAnimation(0, "bully/bully", false);
    followerInteraction.playerFarming.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/player/long_laugh_baah", followerInteraction.playerFarming.gameObject);
    yield return (object) new WaitForSeconds(num - 2.25f);
    yield return (object) new WaitForSeconds(0.5f);
    AudioManager.Instance.PlayOneShot("event:/followers/gain_loyalty", followerInteraction.gameObject.transform.position);
    followerInteraction.follower.Brain._directInfoAccess.ReassuranceCount = Mathf.Clamp(--followerInteraction.follower.Brain._directInfoAccess.ReassuranceCount, -3, -1);
    if (!followerInteraction.follower.Brain.HasTrait(FollowerTrait.TraitType.Terrified) && followerInteraction.follower.Brain._directInfoAccess.ReassuranceCount <= -3)
    {
      followerInteraction.follower.Brain.AddTrait(FollowerTrait.TraitType.Terrified, true);
      followerInteraction.follower.Brain._directInfoAccess.ReassuranceCount = 0;
    }
    followerInteraction.follower.Brain.AddAdoration(FollowerBrain.AdorationActions.Bully, new System.Action(followerInteraction.\u003CBullyRoutine\u003Eb__92_0));
  }

  public IEnumerator DanceRoutine(bool hostFollower)
  {
    interaction_FollowerInteraction followerInteraction = this;
    if (hostFollower)
    {
      float num = 3f;
      List<Follower> followerList = new List<Follower>();
      foreach (Follower follower in Follower.Followers)
      {
        if ((UnityEngine.Object) follower != (UnityEngine.Object) followerInteraction.follower && follower.Brain.Info.CursedState != Thought.Dissenter && !FollowerManager.FollowerLocked(follower.Brain.Info.ID) && (double) Vector3.Distance(follower.transform.position, followerInteraction.transform.position) < (double) num && !follower.Brain.Stats.Inspired && follower.Brain.CurrentTaskType != FollowerTaskType.Sleep && follower.Brain.CurrentTaskType != FollowerTaskType.GetPlayerAttention && !follower.Brain.HasTrait(FollowerTrait.TraitType.Mutated))
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
      followerInteraction.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      if (followerInteraction._playerFarming.isLamb)
        AudioManager.Instance.PlayOneShot("event:/player/speak_to_follower", followerInteraction.playerFarming.transform.position);
      else
        AudioManager.Instance.PlayOneShot("event:/player/goat_player/goat_speak_to_follower", followerInteraction.playerFarming.transform.position);
      AudioManager.Instance.PlayOneShot("event:/Stings/white_eyes", followerInteraction.playerFarming.transform.position);
    }
    followerInteraction.follower.State.CURRENT_STATE = StateMachine.State.Dancing;
    yield return (object) null;
    if (hostFollower)
    {
      followerInteraction.playerFarming.Spine.AnimationState.SetAnimation(0, "dance", true);
      if (followerInteraction._playerFarming.isLamb)
        AudioManager.Instance.PlayOneShot("event:/player/speak_to_follower", followerInteraction.playerFarming.transform.position);
      else
        AudioManager.Instance.PlayOneShot("event:/player/goat_player/goat_speak_to_follower", followerInteraction.playerFarming.transform.position);
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
      CultFaithManager.AddThought(Thought.Cult_Inspire, this.follower.Brain.Info.ID);
      if (hostFollower)
      {
        this.eventListener.PlayFollowerVO(this.bowVO);
        if (this.follower.Brain.CanLevelUp())
          this.StartCoroutine((IEnumerator) this.LevelUpRoutine(this.previousTaskType));
        else
          this.Close();
      }
      else
        this.follower.Brain.CompleteCurrentTask();
    }));
  }

  public IEnumerator GiveItemRoutine(InventoryItem.ITEM_TYPE itemToGive)
  {
    interaction_FollowerInteraction followerInteraction = this;
    if (followerInteraction.follower.Brain.Info.Necklace != InventoryItem.ITEM_TYPE.NONE && DataManager.AllNecklaces.Contains(itemToGive))
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
      followerInteraction.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      float faithMultiplier = 7f;
      if (itemToGive == InventoryItem.ITEM_TYPE.GIFT_MEDIUM)
      {
        followerInteraction.playerFarming.simpleSpineAnimator.Animate("give-item/gift-medium", 0, false);
        faithMultiplier = 10f;
        JudgementMeter.ShowModify(1);
      }
      else if (itemToGive == InventoryItem.ITEM_TYPE.GIFT_SMALL)
      {
        followerInteraction.playerFarming.simpleSpineAnimator.Animate("give-item/gift-small", 0, false);
        faithMultiplier = 5f;
      }
      else
        followerInteraction.playerFarming.simpleSpineAnimator.Animate("give-item/generic", 0, false);
      CultFaithManager.AddThought(Thought.Cult_GaveFollowerItem, -1, faithMultiplier, InventoryItem.LocalizedName(itemToGive));
      followerInteraction.playerFarming.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
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
        case InventoryItem.ITEM_TYPE.Necklace_Loyalty:
        case InventoryItem.ITEM_TYPE.Necklace_Demonic:
        case InventoryItem.ITEM_TYPE.Necklace_Dark:
        case InventoryItem.ITEM_TYPE.Necklace_Light:
        case InventoryItem.ITEM_TYPE.Necklace_Missionary:
        case InventoryItem.ITEM_TYPE.Necklace_Gold_Skull:
        case InventoryItem.ITEM_TYPE.Necklace_Deaths_Door:
        case InventoryItem.ITEM_TYPE.Necklace_Winter:
        case InventoryItem.ITEM_TYPE.Necklace_Frozen:
        case InventoryItem.ITEM_TYPE.Necklace_Weird:
        case InventoryItem.ITEM_TYPE.Necklace_Targeted:
          if (!followerInteraction.follower.Brain._directInfoAccess.HasReceivedNecklace)
          {
            followerInteraction.follower.Brain.GetWillLevelUp(FollowerBrain.AdorationActions.Necklace);
            break;
          }
          break;
      }
      int Waiting = 0;
      AudioManager.Instance.PlayOneShot("event:/followers/pop_in", followerInteraction.transform.position);
      ResourceCustomTarget.Create(followerInteraction.follower.gameObject, followerInteraction.playerFarming.CameraBone.transform.position, itemToGive, (System.Action) (() =>
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
          case InventoryItem.ITEM_TYPE.Necklace_Loyalty:
          case InventoryItem.ITEM_TYPE.Necklace_Demonic:
          case InventoryItem.ITEM_TYPE.Necklace_Dark:
          case InventoryItem.ITEM_TYPE.Necklace_Light:
          case InventoryItem.ITEM_TYPE.Necklace_Missionary:
          case InventoryItem.ITEM_TYPE.Necklace_Gold_Skull:
          case InventoryItem.ITEM_TYPE.Necklace_Deaths_Door:
          case InventoryItem.ITEM_TYPE.Necklace_Winter:
          case InventoryItem.ITEM_TYPE.Necklace_Frozen:
          case InventoryItem.ITEM_TYPE.Necklace_Weird:
          case InventoryItem.ITEM_TYPE.Necklace_Targeted:
            AudioManager.Instance.PlayOneShot("event:/followers/gain_loyalty", this.gameObject.transform.position);
            if (this.follower.Brain._directInfoAccess.HasReceivedNecklace)
              ++Waiting;
            else
              this.follower.Brain.AddAdoration(FollowerBrain.AdorationActions.Necklace, (System.Action) (() => ++Waiting));
            System.Action<Follower, InventoryItem.ITEM_TYPE, System.Action> followerCallbacks1 = InventoryItem.GiveToFollowerCallbacks(itemToGive);
            if (followerCallbacks1 != null)
              followerCallbacks1(this.follower, itemToGive, (System.Action) (() => ++Waiting));
            this.follower.Brain._directInfoAccess.HasReceivedNecklace = true;
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
      if (followerInteraction.follower.Brain.CanLevelUp())
        followerInteraction.StartCoroutine((IEnumerator) followerInteraction.LevelUpRoutine(followerInteraction.previousTaskType, new System.Action(followerInteraction.Close), onFinishClose: false));
      else
        followerInteraction.Close();
    }
  }

  public IEnumerator RemoveNecklaceRoutine()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    interaction_FollowerInteraction followerInteraction = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      CultFaithManager.AddThought(Thought.Cult_RemovedFollowerNecklace, followerInteraction.follower.Brain.Info.ID);
      followerInteraction.Close();
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    int necklace = (int) followerInteraction.follower.Brain.Info.Necklace;
    followerInteraction.RemoveTraitGivenByItem();
    followerInteraction.follower.Brain.Info.Necklace = InventoryItem.ITEM_TYPE.NONE;
    followerInteraction.follower.UpdateOutfit();
    Vector3 position = followerInteraction.transform.position + Vector3.back * 0.5f;
    PickUp pickUp = InventoryItem.Spawn((InventoryItem.ITEM_TYPE) necklace, 1, position);
    GameManager.GetInstance().OnConversationNext(pickUp.gameObject, 6f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(1.5f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator GivePetRoutine(InventoryItem.ITEM_TYPE itemToGive)
  {
    interaction_FollowerInteraction followerInteraction = this;
    if (followerInteraction.follower.Brain._directInfoAccess.DLCPets.Count > 0)
    {
      followerInteraction.eventListener.PlayFollowerVO(followerInteraction.negativeAcknowledgeVO);
      followerInteraction.CloseAndSpeak("AlreadyHavePet");
    }
    else
    {
      followerInteraction.eventListener.PlayFollowerVO(followerInteraction.positiveAcknowledgeVO);
      GameManager.GetInstance().OnConversationNext(followerInteraction.gameObject, 3f);
      GameManager.GetInstance().CameraSetOffset(Vector3.zero);
      yield return (object) new WaitForSeconds(1f);
      DataManager.Instance.GivenFollowerGift = true;
      followerInteraction.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      followerInteraction.playerFarming.simpleSpineAnimator.Animate("give-item/gift-medium", 0, false);
      followerInteraction.playerFarming.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
      float faithMultiplier = 10f;
      JudgementMeter.ShowModify(1);
      CultFaithManager.AddThought(Thought.Cult_GaveFollowerItem, -1, faithMultiplier, InventoryItem.LocalizedName(itemToGive));
      GameManager.GetInstance().OnConversationNext(followerInteraction.gameObject, 3f);
      followerInteraction.follower.Brain.GetWillLevelUp(FollowerBrain.AdorationActions.GiftPet);
      int Waiting = 0;
      AudioManager.Instance.PlayOneShot("event:/followers/pop_in", followerInteraction.transform.position);
      ResourceCustomTarget.Create(followerInteraction.follower.gameObject, followerInteraction.playerFarming.CameraBone.transform.position, itemToGive, (System.Action) (() =>
      {
        AudioManager.Instance.PlayOneShot("event:/followers/gain_loyalty", this.gameObject.transform.position);
        this.follower.Brain.AddAdoration(FollowerBrain.AdorationActions.GiftPet, (System.Action) (() => ++Waiting));
        System.Action<Follower, InventoryItem.ITEM_TYPE, System.Action> followerCallbacks = InventoryItem.GiveToFollowerCallbacks(itemToGive);
        if (followerCallbacks != null)
          followerCallbacks(this.follower, itemToGive, (System.Action) (() => ++Waiting));
        Inventory.ChangeItemQuantity(itemToGive, -1);
        this.follower.CreateNewPet(itemToGive, this.transform.position, (System.Action<FollowerPet>) (pet =>
        {
          if (!((UnityEngine.Object) pet != (UnityEngine.Object) null))
            return;
          this.follower.Brain._directInfoAccess.DLCPets.Add(pet.DLCPetData);
        }));
      }), false);
      while (Waiting < 2)
        yield return (object) null;
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.GiveGift);
      if (followerInteraction.follower.Brain.CanLevelUp())
        followerInteraction.StartCoroutine((IEnumerator) followerInteraction.LevelUpRoutine(followerInteraction.previousTaskType, new System.Action(followerInteraction.Close), onFinishClose: false));
      else
        followerInteraction.Close();
    }
  }

  public void RemoveTraitGivenByItem()
  {
    if (this.follower.Brain.Info.Necklace != InventoryItem.ITEM_TYPE.Necklace_Gold_Skull || 666 == this.follower.Brain.Info.ID || 10009 == this.follower.Brain.Info.ID)
      return;
    this.follower.Brain.Info.Traits.Remove(FollowerTrait.TraitType.Immortal);
  }

  public IEnumerator BribeRoutine()
  {
    interaction_FollowerInteraction followerInteraction = this;
    followerInteraction.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    followerInteraction.playerFarming.simpleSpineAnimator.Animate("give-item/generic", 0, false);
    followerInteraction.playerFarming.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    followerInteraction.follower.AddThought(Thought.Bribed);
    int i = -1;
    while (++i <= 2)
    {
      if (i < 2)
      {
        AudioManager.Instance.PlayOneShot("event:/followers/pop_in", followerInteraction.transform.position);
        ResourceCustomTarget.Create(followerInteraction.follower.gameObject, followerInteraction.playerFarming.CameraBone.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, (System.Action) null, false);
        yield return (object) new WaitForSeconds(0.3f);
      }
      else
      {
        AudioManager.Instance.PlayOneShot("event:/followers/pop_in", followerInteraction.transform.position);
        ResourceCustomTarget.Create(followerInteraction.follower.gameObject, followerInteraction.playerFarming.CameraBone.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, new System.Action(followerInteraction.\u003CBribeRoutine\u003Eb__98_0), false);
      }
    }
  }

  public List<ConversationEntry> GetConversationEntry(
    Follower.ComplaintType ComplaintForBark,
    ObjectivesData objective,
    FollowerTask_GetAttention getAttentionTask = null)
  {
    List<ConversationEntry> conversationEntryList1 = new List<ConversationEntry>();
    string str1 = "";
    if (this.follower.Brain.Info.ID == 99990)
      str1 = "Leshy";
    else if (this.follower.Brain.Info.ID == 99991)
      str1 = "Heket";
    else if (this.follower.Brain.Info.ID == 99992)
      str1 = "Kallamar";
    else if (this.follower.Brain.Info.ID == 99993)
      str1 = "Shamura";
    else if (this.follower.Brain.Info.ID == 666)
      str1 = "Narinder";
    switch (ComplaintForBark)
    {
      case Follower.ComplaintType.GiveQuest:
        List<ConversationEntry> conversationEntry1;
        if (objective == null)
        {
          string translation = LocalizationManager.GetTranslation($"FollowerInteractions/AbortGivingQuest_{UnityEngine.Random.Range(0, 8).ToString()}_Abort");
          if (this.IsBishop)
          {
            if ((double) UnityEngine.Random.value < 0.5)
              translation = LocalizationManager.GetTranslation("FollowerInteractions/AbortGivingQuest_6_Abort/AnyBishop");
            else if (this.follower.Brain.Info.ID == 99990)
              translation = LocalizationManager.GetTranslation("FollowerInteractions/AbortGivingQuest_1_Abort/Leshy");
            else if (this.follower.Brain.Info.ID == 99991)
              translation = LocalizationManager.GetTranslation("FollowerInteractions/AbortGivingQuest_2_Abort/Heket");
            else if (this.follower.Brain.Info.ID == 99992)
              translation = LocalizationManager.GetTranslation("FollowerInteractions/AbortGivingQuest_3_Abort/Kallamar");
            else if (this.follower.Brain.Info.ID == 99993)
              translation = LocalizationManager.GetTranslation("FollowerInteractions/AbortGivingQuest_4_Abort/Shamura");
            else if (this.follower.Brain.Info.ID == 666)
              translation = LocalizationManager.GetTranslation("FollowerInteractions/AbortGivingQuest_5_Abort/Narinder");
          }
          conversationEntry1 = new List<ConversationEntry>()
          {
            new ConversationEntry(this.gameObject, translation)
          };
        }
        else if (objective != null && objective is Objectives_Story)
        {
          Objectives_Story objectivesStory = objective as Objectives_Story;
          string translation = LocalizationManager.GetTranslation("FollowerInteractions/" + objectivesStory.StoryDataItem.StoryObjectiveData.GiveQuestTerm);
          List<string> stringList = new List<string>();
          if (objectivesStory.StoryDataItem.TargetFollowerID_1 != -1 && objectivesStory.StoryDataItem.TargetFollowerID_1 != objectivesStory.StoryDataItem.QuestGiverFollowerID && (objectivesStory.StoryDataItem.StoryObjectiveData.RequireTarget_1 || objectivesStory.StoryDataItem.StoryObjectiveData.TargetQuestGiver))
            stringList.Add(FollowerInfo.GetInfoByID(!objectivesStory.StoryDataItem.StoryObjectiveData.TargetQuestGiver || objectivesStory.StoryDataItem.StoryObjectiveData.RequireTarget_1 ? objectivesStory.StoryDataItem.TargetFollowerID_1 : objectivesStory.StoryDataItem.QuestGiverFollowerID, true).Name);
          if (objectivesStory.StoryDataItem.TargetFollowerID_2 != -1 && objectivesStory.StoryDataItem.StoryObjectiveData.RequireTarget_2)
            stringList.Add(FollowerInfo.GetInfoByID(objectivesStory.StoryDataItem.TargetFollowerID_2, true).Name);
          if (objectivesStory.StoryDataItem.DeadFollowerID != -1 && objectivesStory.StoryDataItem.StoryObjectiveData.RequireTarget_Deadbody && FollowerInfo.GetInfoByID(objectivesStory.StoryDataItem.DeadFollowerID, true) != null)
            stringList.Add(FollowerInfo.GetInfoByID(objectivesStory.StoryDataItem.DeadFollowerID, true).Name);
          if (objectivesStory.StoryDataItem.CachedTargetFollowerID_1 != -1)
            stringList.Add(FollowerInfo.GetInfoByID(objectivesStory.StoryDataItem.CachedTargetFollowerID_1, true).Name);
          if (objectivesStory.StoryDataItem.CachedTargetFollowerID_2 != -1)
            stringList.Add(FollowerInfo.GetInfoByID(objectivesStory.StoryDataItem.CachedTargetFollowerID_2, true).Name);
          string[] array = stringList.ToArray();
          string TermToSpeak1 = string.Format(translation, (object[]) array);
          List<ConversationEntry> conversationEntryList2 = new List<ConversationEntry>();
          conversationEntryList2.Add(new ConversationEntry(this.gameObject, TermToSpeak1));
          int num = 0;
          while (LocalizationManager.GetTermData($"FollowerInteractions/{objectivesStory.StoryDataItem.StoryObjectiveData.GiveQuestTerm}{$"/{++num}"}") != null)
          {
            string TermToSpeak2 = string.Format(LocalizationManager.GetTranslation($"FollowerInteractions/{objectivesStory.StoryDataItem.StoryObjectiveData.GiveQuestTerm}{$"/{num}"}"), (object[]) array);
            conversationEntryList2.Add(new ConversationEntry(this.gameObject, TermToSpeak2));
          }
          conversationEntry1 = conversationEntryList2;
        }
        else if (objective.Type == Objectives.TYPES.PERFORM_RITUAL)
        {
          string str2 = "FollowerInteractions/GiveQuest/PerformRitual/";
          Objectives_PerformRitual objectivesPerformRitual = objective as Objectives_PerformRitual;
          string translation = LocalizationManager.GetTranslation(str2 + objectivesPerformRitual.Ritual.ToString());
          FollowerInfo infoById = FollowerInfo.GetInfoByID(objective.Follower);
          if (objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_Wedding && infoById != null && (double) infoById.Drunk > 0.0)
            translation = LocalizationManager.GetTranslation(str2 + "Drunk/" + objectivesPerformRitual.Ritual.ToString());
          else if (objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_Divorce && this.follower.Brain.Info.MarriedToLeader)
            translation = LocalizationManager.GetTranslation(str2 + "Player/" + objectivesPerformRitual.Ritual.ToString());
          else if (FollowerManager.BishopIDs.Contains(objective.Follower))
            translation = LocalizationManager.GetTranslation($"{str2}{objectivesPerformRitual.Ritual.ToString()}/{str1}");
          List<ConversationEntry> conversationEntryList3 = new List<ConversationEntry>();
          if (objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_Divorce && !this.follower.Brain.Info.MarriedToLeader || objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_ConvertToRot)
          {
            int num = 0;
            while (LocalizationManager.GetTermData(translation + $"/{++num}") != null)
            {
              string TermToSpeak = string.Format(LocalizationManager.GetTranslation(translation + $"/{num}"), (object) FollowerInfo.GetInfoByID(objectivesPerformRitual.TargetFollowerID_1, true)?.Name, (object) FollowerInfo.GetInfoByID(objectivesPerformRitual.TargetFollowerID_2, true)?.Name);
              conversationEntryList3.Add(new ConversationEntry(this.gameObject, TermToSpeak));
            }
          }
          else
          {
            string TermToSpeak = string.Format(translation, (object) FollowerInfo.GetInfoByID(objectivesPerformRitual.TargetFollowerID_1, true)?.Name, (object) FollowerInfo.GetInfoByID(objectivesPerformRitual.TargetFollowerID_2, true)?.Name);
            conversationEntryList3.Add(new ConversationEntry(this.gameObject, TermToSpeak));
          }
          conversationEntry1 = conversationEntryList3;
        }
        else if (objective is Objectives_Custom objectivesCustom2 && (objectivesCustom2.CustomQuestType == Objectives.CustomQuestTypes.SendFollowerOnMissionary || objectivesCustom2.CustomQuestType == Objectives.CustomQuestTypes.SendSpouseOnMissionary || objectivesCustom2.CustomQuestType == Objectives.CustomQuestTypes.SendFollowerToPrison || objectivesCustom2.CustomQuestType == Objectives.CustomQuestTypes.UseFirePit || objectivesCustom2.CustomQuestType == Objectives.CustomQuestTypes.UseFeastTable || objectivesCustom2.CustomQuestType == Objectives.CustomQuestTypes.MurderFollower || objectivesCustom2.CustomQuestType == Objectives.CustomQuestTypes.MurderFollowerAtNight || objectivesCustom2.CustomQuestType == Objectives.CustomQuestTypes.OpenPub || objectivesCustom2.CustomQuestType == Objectives.CustomQuestTypes.DrumCircle || objectivesCustom2.CustomQuestType == Objectives.CustomQuestTypes.BuildGoodSnowman || objectivesCustom2.CustomQuestType == Objectives.CustomQuestTypes.ChangeTraits || objectivesCustom2.CustomQuestType == Objectives.CustomQuestTypes.TakeCultsPhoto || objectivesCustom2.CustomQuestType == Objectives.CustomQuestTypes.KillInFurnace))
        {
          Objectives_Custom objectivesCustom = objective as Objectives_Custom;
          string Term = "FollowerInteractions/GiveQuest/" + objectivesCustom.CustomQuestType.ToString();
          if (FollowerManager.BishopIDs.Contains(objective.Follower))
            Term = $"{Term}/{str1}";
          string translation = LocalizationManager.GetTranslation(Term);
          conversationEntry1 = new List<ConversationEntry>()
          {
            new ConversationEntry(this.gameObject, objectivesCustom2.CustomQuestType != Objectives.CustomQuestTypes.TakeCultsPhoto ? string.Format(translation, (object) FollowerInfo.GetInfoByID(objectivesCustom.TargetFollowerID)?.Name) : string.Format(translation, (object) DataManager.Instance.CultName))
          };
          conversationEntry1[0].TermName = Term;
        }
        else if (objective is Objectives_RecruitCursedFollower)
          conversationEntry1 = new List<ConversationEntry>()
          {
            new ConversationEntry(this.gameObject, LocalizationManager.GetTranslation("FollowerInteractions/GiveQuest/RecruitCursedFollower/" + (objective as Objectives_RecruitCursedFollower).CursedState.ToString()))
          };
        else if (objective.Type == Objectives.TYPES.FIND_FOLLOWER)
          conversationEntry1 = new List<ConversationEntry>()
          {
            new ConversationEntry(this.gameObject, LocalizationManager.GetTranslation($"FollowerInteractions/GiveQuest/FindFollower/Variant_{(objective as Objectives_FindFollower).ObjectiveVariant}"))
          };
        else if (objective.Type == Objectives.TYPES.COLLECT_ITEM)
          conversationEntry1 = new List<ConversationEntry>()
          {
            new ConversationEntry(this.gameObject, LocalizationManager.GetTranslation("FollowerInteractions/GiveQuest/CollectItem/" + (objective as Objectives_CollectItem).ItemType.ToString()))
          };
        else if (objective.Type == Objectives.TYPES.COOK_MEALS)
        {
          Objectives_CookMeal objectivesCookMeal = objective as Objectives_CookMeal;
          conversationEntry1 = new List<ConversationEntry>()
          {
            new ConversationEntry(this.gameObject, string.Format(LocalizationManager.GetTranslation("FollowerInteractions/GiveQuest/CookMeal/" + objectivesCookMeal.MealType.ToString()), (object) CookingData.GetLocalizedName(objectivesCookMeal.MealType)))
          };
        }
        else if (objective.Type == Objectives.TYPES.PLACE_STRUCTURES)
          conversationEntry1 = new List<ConversationEntry>()
          {
            new ConversationEntry(this.gameObject, LocalizationManager.GetTranslation("FollowerInteractions/GiveQuest/PlaceStructure/" + (objective as Objectives_PlaceStructure).category.ToString()))
          };
        else if (objective.Type == Objectives.TYPES.BUILD_STRUCTURE)
          conversationEntry1 = new List<ConversationEntry>()
          {
            new ConversationEntry(this.gameObject, LocalizationManager.GetTranslation("FollowerInteractions/GiveQuest/BuildStructure/" + (objective as Objectives_BuildStructure).StructureType.ToString()))
          };
        else if (objective.Type == Objectives.TYPES.EAT_MEAL)
        {
          string Term = "FollowerInteractions/GiveQuest/EatMeal/" + (objective as Objectives_EatMeal).MealType.ToString();
          if (FollowerManager.BishopIDs.Contains(objective.Follower))
            Term = $"{Term}/{str1}";
          conversationEntry1 = new List<ConversationEntry>()
          {
            new ConversationEntry(this.gameObject, LocalizationManager.GetTranslation(Term))
          };
          conversationEntry1[0].TermName = Term;
        }
        else if (objective.Type == Objectives.TYPES.MATING)
        {
          Objectives_Mating objectivesMating = objective as Objectives_Mating;
          string Term = "FollowerInteractions/GiveQuest/Mating";
          if (FollowerManager.BishopIDs.Contains(objective.Follower))
            Term = $"{Term}/{str1}";
          conversationEntry1 = new List<ConversationEntry>()
          {
            new ConversationEntry(this.gameObject, string.Format(LocalizationManager.GetTranslation(Term), (object) FollowerInfo.GetInfoByID(objectivesMating.TargetFollower_2).Name, (object) FollowerInfo.GetInfoByID(objectivesMating.TargetFollower_2)))
          };
          conversationEntry1[0].TermName = Term;
        }
        else if (objective.Type == Objectives.TYPES.FINISH_RACE)
        {
          Objectives_FinishRace objectivesFinishRace = objective as Objectives_FinishRace;
          string str3 = "FollowerInteractions/GiveQuest/FinishRace";
          int num = 0;
          List<ConversationEntry> conversationEntryList4 = new List<ConversationEntry>();
          for (; LocalizationManager.GetTermData(str3 + $"/{num}") != null; ++num)
          {
            string TermToSpeak = string.Format(LocalizationManager.GetTranslation(str3 + $"/{num}"), (object) objectivesFinishRace.RaceTargetTime.ToString());
            conversationEntryList4.Add(new ConversationEntry(this.gameObject, TermToSpeak));
          }
          conversationEntry1 = conversationEntryList4;
        }
        else if (objective.Type == Objectives.TYPES.DRINK)
        {
          if ((objective as Objectives_Drink).DrinkType == InventoryItem.ITEM_TYPE.DRINK_CHILLI)
          {
            string Term = "FollowerInteractions/GiveQuest/WinterDrink/0";
            conversationEntry1 = new List<ConversationEntry>()
            {
              new ConversationEntry(this.gameObject, LocalizationManager.GetTranslation(Term))
            };
            conversationEntry1[0].TermName = Term;
          }
          else
          {
            int maxExclusive = -1;
            do
              ;
            while (LocalizationManager.GetTermData($"FollowerInteractions/{ComplaintForBark.ToString()}_{(++maxExclusive).ToString()}") != null);
            conversationEntry1 = this.GetConversationEntry($"{ComplaintForBark.ToString()}_{UnityEngine.Random.Range(0, maxExclusive).ToString()}");
          }
        }
        else if (objective is Objectives_Custom objectivesCustom1 && objectivesCustom1.CustomQuestType == Objectives.CustomQuestTypes.SendFollowerToMatingTent)
        {
          string Term = "FollowerInteractions/GiveQuest/" + objectivesCustom1.CustomQuestType.ToString();
          if (this.follower.Brain.HasTrait(FollowerTrait.TraitType.PureBlood) || this.follower.Brain.HasTrait(FollowerTrait.TraitType.PureBlood_1) || this.follower.Brain.HasTrait(FollowerTrait.TraitType.PureBlood_2) || this.follower.Brain.HasTrait(FollowerTrait.TraitType.PureBlood_3))
            Term = "FollowerInteractions/GiveQuest/BreedPureBlood/0";
          conversationEntry1 = new List<ConversationEntry>()
          {
            new ConversationEntry(this.gameObject, string.Format(LocalizationManager.GetTranslation(Term), (object) FollowerInfo.GetInfoByID(objectivesCustom1.TargetFollowerID)?.Name))
          };
          conversationEntry1[0].TermName = Term;
        }
        else
        {
          int maxExclusive = -1;
          do
            ;
          while (LocalizationManager.GetTermData($"FollowerInteractions/{ComplaintForBark.ToString()}_{(++maxExclusive).ToString()}") != null);
          conversationEntry1 = this.GetConversationEntry($"{ComplaintForBark.ToString()}_{UnityEngine.Random.Range(0, maxExclusive).ToString()}");
        }
        foreach (ConversationEntry conversationEntry2 in conversationEntry1)
        {
          conversationEntry2.pitchValue = this.follower.Brain._directInfoAccess.follower_pitch;
          conversationEntry2.vibratoValue = this.follower.Brain._directInfoAccess.follower_vibrato;
          conversationEntry2.CharacterName = $"<color=yellow>{this.follower.Brain.Info.Name}</color>";
          conversationEntry2.Animation = "talk-nice1";
          conversationEntry2.SetZoom = true;
          conversationEntry2.Zoom = 4f;
          conversationEntry2.soundPath = this.generalTalkVO;
        }
        return conversationEntry1;
      case Follower.ComplaintType.GiveOnboarding:
        string TermToSpeak3 = DataManager.Instance.CurrentOnboardingFollowerTerm;
        if (DataManager.Instance.CurrentOnboardingFollowerID == -1)
          TermToSpeak3 = LocalizationManager.GetTranslation($"FollowerInteractions/AbortGivingQuest_{UnityEngine.Random.Range(0, 8).ToString()}_Abort");
        List<ConversationEntry> conversationEntry3 = new List<ConversationEntry>()
        {
          new ConversationEntry(this.gameObject, TermToSpeak3)
        };
        string animName = "worship";
        switch (DataManager.Instance.CurrentOnboardingFollowerTerm)
        {
          case "Conversation_NPC/FollowerOnboarding/SickFollower":
            animName = "Worship/worship-sick";
            break;
          case "Conversation_NPC/FollowerOnboarding/CureDissenter":
            animName = "Worship/worship-dissenter";
            double num1 = (double) this.follower.SetBodyAnimation(animName, true);
            break;
          case "Conversation_NPC/FollowerOnboarding/Freezing/0":
            animName = "Worship/worship-freezing";
            double num2 = (double) this.follower.SetBodyAnimation(animName, true);
            break;
          case "Conversation_NPC/FollowerOnboarding/Overheating/0":
            animName = "Worship/worship-hot";
            double num3 = (double) this.follower.SetBodyAnimation(animName, true);
            break;
        }
        if (this.follower.Brain.Info.IsDrunk)
          animName = "Worship/worship-drunk";
        conversationEntry3[0].pitchValue = this.follower.Brain._directInfoAccess.follower_pitch;
        conversationEntry3[0].vibratoValue = this.follower.Brain._directInfoAccess.follower_vibrato;
        conversationEntry3[0].soundPath = this.generalTalkVO;
        conversationEntry3[0].SkeletonData = this.follower.Spine;
        conversationEntry3[0].CharacterName = $"<color=yellow>{this.follower.Brain.Info.Name}</color>";
        conversationEntry3[0].Animation = animName;
        conversationEntry3[0].SetZoom = true;
        conversationEntry3[0].Zoom = 4f;
        if (DataManager.Instance.CurrentOnboardingFollowerTerm.Contains("DrunkConvo_"))
          conversationEntry3[0].soundPath = "event:/dialogue/followers/drunk/talk_drunk";
        int num4 = 0;
        if (conversationEntry3[0].TermToSpeak[conversationEntry3[0].TermToSpeak.Length - 1] == '0')
        {
          while (LocalizationManager.GetTermData(ConversationEntry.Clone(conversationEntry3[0]).TermToSpeak.Replace("0", (++num4).ToString())) != null)
          {
            ConversationEntry conversationEntry4 = ConversationEntry.Clone(conversationEntry3[0]);
            conversationEntry4.TermToSpeak = conversationEntry4.TermToSpeak.Replace("0", num4.ToString());
            conversationEntry3.Add(conversationEntry4);
          }
        }
        if (conversationEntry3[0].TermToSpeak.Contains("Poem"))
        {
          ConversationEntry conversationEntry5 = ConversationEntry.Clone(conversationEntry3[0]);
          ConversationEntry conversationEntry6 = ConversationEntry.Clone(conversationEntry3[0]);
          conversationEntry5.TermToSpeak = $"FollowerInteractions/PoemConvo_Intro_{UnityEngine.Random.Range(0, 5)}";
          conversationEntry6.TermToSpeak = $"FollowerInteractions/PoemConvo_Outro_{UnityEngine.Random.Range(0, 5)}";
          if (conversationEntry3[0].TermToSpeak.Contains("Amateur"))
            conversationEntry6.TermToSpeak = $"FollowerInteractions/PoemConvo_Outro_{UnityEngine.Random.Range(1, 3)}";
          conversationEntry3.Insert(0, conversationEntry5);
          conversationEntry3.Add(conversationEntry6);
        }
        if (DataManager.Instance.CurrentOnboardingFollowerTerm.Contains("PilgrimsPartTwo") & conversationEntry3.Count >= 2)
          conversationEntry3[2].TermToSpeak = string.Format(LocalizationManager.GetTranslation(conversationEntry3[2].TermToSpeak), (object) LocalizationManager.GetTranslation($"NAMES/Places/{DataManager.Instance.PilgrimTargetLocation}"));
        return conversationEntry3;
      default:
        List<ConversationEntry> conversationEntry7;
        if (ComplaintForBark == Follower.ComplaintType.CompletedQuest && !string.IsNullOrEmpty(objective.CompleteTerm))
        {
          string completeTerm = objective.CompleteTerm;
          if (FollowerManager.BishopIDs.Contains(this.follower.Brain.Info.ID) && !completeTerm.Contains("QuestCompleted") && !completeTerm.Contains("/Complete") && !completeTerm.Contains("BecameDisciple"))
            completeTerm += "/Complete";
          conversationEntry7 = this.GetConversationEntry(completeTerm, objective.CompleteTermArguments);
          int num5 = 0;
          if (conversationEntry7[0].TermToSpeak[conversationEntry7[0].TermToSpeak.Length - 1] == '0')
          {
            while (LocalizationManager.GetTermData(ConversationEntry.Clone(conversationEntry7[0]).TermToSpeak.Replace("/0", "/" + (++num5).ToString())) != null)
            {
              ConversationEntry conversationEntry8 = ConversationEntry.Clone(conversationEntry7[0]);
              conversationEntry8.TermToSpeak = conversationEntry8.TermToSpeak.Replace("/0", "/" + num5.ToString());
              conversationEntry7.Add(conversationEntry8);
            }
          }
          if (this.follower.Brain.Info.ID == 99996 && objective.CompleteTerm.Contains("SozoFollower"))
            ++DataManager.Instance.SozoMushroomCount;
        }
        else
        {
          if (ComplaintForBark == Follower.ComplaintType.Speak)
          {
            List<ConversationEntry> conversationEntry9 = new List<ConversationEntry>()
            {
              new ConversationEntry(this.gameObject, "Conversation_NPC/" + getAttentionTask.Term)
            };
            conversationEntry9[0].pitchValue = this.follower.Brain._directInfoAccess.follower_pitch;
            conversationEntry9[0].vibratoValue = this.follower.Brain._directInfoAccess.follower_vibrato;
            conversationEntry9[0].soundPath = this.generalTalkVO;
            conversationEntry9[0].SkeletonData = this.follower.Spine;
            conversationEntry9[0].CharacterName = $"<color=yellow>{this.follower.Brain.Info.Name}</color>";
            conversationEntry9[0].Animation = "Reactions/react-laugh";
            conversationEntry9[0].SetZoom = true;
            conversationEntry9[0].Zoom = 4f;
            int num6 = 0;
            if (conversationEntry9[0].TermToSpeak[conversationEntry9[0].TermToSpeak.Length - 1] == '0')
            {
              while (LocalizationManager.GetTermData(ConversationEntry.Clone(conversationEntry9[0]).TermToSpeak.Replace("/0", "/" + (++num6).ToString())) != null)
              {
                ConversationEntry conversationEntry10 = ConversationEntry.Clone(conversationEntry9[0]);
                conversationEntry10.TermToSpeak = conversationEntry10.TermToSpeak.Replace("/0", "/" + num6.ToString());
                conversationEntry9.Add(conversationEntry10);
              }
            }
            return conversationEntry9;
          }
          int maxExclusive = -1;
          do
            ;
          while (LocalizationManager.GetTermData($"FollowerInteractions/{ComplaintForBark.ToString()}_{(++maxExclusive).ToString()}") != null);
          conversationEntry7 = this.GetConversationEntry($"{ComplaintForBark.ToString()}_{UnityEngine.Random.Range(0, maxExclusive).ToString()}");
        }
        foreach (ConversationEntry conversationEntry11 in conversationEntry7)
        {
          conversationEntry11.Speaker = this.follower.gameObject;
          conversationEntry11.soundPath = this.generalTalkVO;
          conversationEntry11.SkeletonData = this.follower.Spine;
        }
        switch (ComplaintForBark)
        {
          case Follower.ComplaintType.Hunger:
            double num7 = (double) this.follower.SetBodyAnimation("Worship/worship-hungry", true);
            break;
          case Follower.ComplaintType.Homeless:
          case Follower.ComplaintType.NeedBetterHouse:
            double num8 = (double) this.follower.SetBodyAnimation("Worship/worship-unhappy", true);
            break;
          default:
            if (this.follower.Brain.Info.IsDrunk)
            {
              double num9 = (double) this.follower.SetBodyAnimation("Worship/worship-drunk", true);
              break;
            }
            if ((double) this.follower.Brain.Stats.Happiness >= 0.699999988079071)
            {
              double num10 = (double) this.follower.SetBodyAnimation("Worship/worship-happy", true);
              break;
            }
            double num11 = (double) this.follower.SetBodyAnimation("Worship/worship", true);
            break;
        }
        return conversationEntry7;
    }
  }

  public List<ConversationEntry> GetConversationEntry(string Entry, string[] arguments = null)
  {
    string str = "FollowerInteractions/" + Entry;
    if (arguments != null && arguments.Length != 0)
      str = string.Format(LocalizationManager.GetTranslation(str), (object[]) arguments);
    List<ConversationEntry> conversationEntry1 = new List<ConversationEntry>()
    {
      new ConversationEntry(this.gameObject, str, "worship")
    };
    conversationEntry1[0].CharacterName = $"<color=yellow>{this.follower.Brain.Info.Name}</color>";
    conversationEntry1[0].Animation = this.follower.Brain.Info.IsDrunk ? "Worship/worship-drunk" : "worship";
    conversationEntry1[0].pitchValue = this.follower.Brain._directInfoAccess.follower_pitch;
    conversationEntry1[0].vibratoValue = this.follower.Brain._directInfoAccess.follower_vibrato;
    conversationEntry1[0].SetZoom = true;
    conversationEntry1[0].Zoom = 4f;
    foreach (ConversationEntry conversationEntry2 in conversationEntry1)
    {
      conversationEntry2.Speaker = this.follower.gameObject;
      conversationEntry2.soundPath = this.generalTalkVO;
      conversationEntry2.SkeletonData = this.follower.Spine;
      conversationEntry2.pitchValue = this.follower.Brain._directInfoAccess.follower_pitch;
      conversationEntry2.vibratoValue = this.follower.Brain._directInfoAccess.follower_vibrato;
      conversationEntry2.followerID = this.follower.Brain.Info.ID;
    }
    return conversationEntry1;
  }

  public void CloseAndSpeak(string ConversationEntryTerm, System.Action callback = null, bool PlayBow = true)
  {
    this.Close(false, false, false);
    this.follower.FacePosition(this.playerFarming.transform.position);
    if (this.follower.Brain.Info.ID == 99991)
      ConversationEntryTerm += "/Heket";
    List<ConversationEntry> conversationEntry1 = this.GetConversationEntry(ConversationEntryTerm);
    foreach (ConversationEntry conversationEntry2 in conversationEntry1)
      conversationEntry2.soundPath = this.PlayVO ? conversationEntry2.soundPath : "";
    MMConversation.Play(new ConversationObject(conversationEntry1, (List<MMTools.Response>) null, (System.Action) (() =>
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
    MMConversation.PlayVO = this.PlayVO;
    double num = (double) this.follower.SetBodyAnimation("worship-talk", true);
  }

  public void GivePoem()
  {
    List<string> stringList = new List<string>();
    if (this.follower.Brain._directInfoAccess.PoemStatus < 3)
    {
      for (int index = 0; index < 6; ++index)
        stringList.Add($"FollowerInteractions/PoemConvo_Amateur_{index + 1}/0");
      if (SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter)
      {
        for (int index = 0; index < 3; ++index)
          stringList.Add($"FollowerInteractions/Winter_PoemConvo_Amateur_{index + 1}/0");
      }
    }
    else if (this.follower.Brain._directInfoAccess.PoemStatus < 6)
    {
      for (int index = 0; index < 6; ++index)
        stringList.Add($"FollowerInteractions/PoemConvo_Mid_{index + 1}/0");
      if (SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter)
      {
        for (int index = 0; index < 3; ++index)
          stringList.Add($"FollowerInteractions/Winter_PoemConvo_Mid_{index + 1}/0");
      }
    }
    else if (this.follower.Brain._directInfoAccess.PoemStatus < 10)
    {
      for (int index = 0; index < 5; ++index)
        stringList.Add($"FollowerInteractions/PoemConvo_Pro_{index + 1}/0");
      if (SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter)
      {
        for (int index = 0; index < 3; ++index)
          stringList.Add($"FollowerInteractions/Winter_PoemConvo_Pro_{index + 1}/0");
      }
    }
    else
    {
      for (int index = 0; index < 5; ++index)
        stringList.Add($"FollowerInteractions/PoemConvo_Disciple_{index + 1}/0");
      if (SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter)
      {
        for (int index = 0; index < 3; ++index)
          stringList.Add($"FollowerInteractions/Winter_PoemConvo_Disciple_{index + 1}/0");
      }
    }
    this.follower.Brain._directInfoAccess.PoemStatus += UnityEngine.Random.Range(1, 3);
    if (DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_1) && this.follower.Brain.Info.ID != 99990 && this.follower.Brain._directInfoAccess.PoemStatus > 5)
      stringList.Add("FollowerInteractions/PoemConvo_AfterLeshy");
    if (DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_2) && this.follower.Brain.Info.ID != 99991 && this.follower.Brain._directInfoAccess.PoemStatus > 5)
      stringList.Add("FollowerInteractions/PoemConvo_AfterHeket");
    if (DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_3) && this.follower.Brain.Info.ID != 99992 && this.follower.Brain._directInfoAccess.PoemStatus > 5)
      stringList.Add("FollowerInteractions/PoemConvo_AfterKallamar");
    if (DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_4) && this.follower.Brain.Info.ID != 99993 && this.follower.Brain._directInfoAccess.PoemStatus > 5)
      stringList.Add("FollowerInteractions/PoemConvo_AfterShamura");
    DataManager.Instance.PoemIncrement = (int) Mathf.Repeat((float) (DataManager.Instance.PoemIncrement + 1), (float) stringList.Count);
    this.follower.Brain._directInfoAccess.PoemProgress = 0.0f;
    this.Close(false, false, false);
    if (!(bool) (UnityEngine.Object) this.playerFarming)
      this.playerFarming = PlayerFarming.FindClosestPlayer(this.transform.position);
    this.follower.FacePosition(this.playerFarming.transform.position);
    string onboardingFollowerTerm = DataManager.Instance.CurrentOnboardingFollowerTerm;
    int onboardingFollowerId = DataManager.Instance.CurrentOnboardingFollowerID;
    DataManager.Instance.CurrentOnboardingFollowerTerm = stringList[DataManager.Instance.PoemIncrement];
    DataManager.Instance.CurrentOnboardingFollowerID = this.follower.Brain.Info.ID;
    this.complaintType = Follower.ComplaintType.GiveOnboarding;
    List<ConversationEntry> conversationEntry1 = this.GetConversationEntry(this.complaintType, (ObjectivesData) null);
    DataManager.Instance.CurrentOnboardingFollowerTerm = onboardingFollowerTerm;
    DataManager.Instance.CurrentOnboardingFollowerID = onboardingFollowerId;
    foreach (ConversationEntry conversationEntry2 in conversationEntry1)
      conversationEntry2.soundPath = this.PlayVO ? conversationEntry2.soundPath : "";
    List<Follower> followerList = new List<Follower>((IEnumerable<Follower>) Follower.Followers);
    for (int index = followerList.Count - 1; index >= 0; --index)
    {
      if (followerList[index].Brain.Info.ID == this.follower.Brain.Info.ID)
        followerList.Remove(followerList[index]);
      else if ((conversationEntry1[1].TermToSpeak == "FollowerInteractions/PoemConvo_Mid_5/0" || conversationEntry1[1].TermToSpeak == "FollowerInteractions/PoemConvo_Pro_4/0" || conversationEntry1[1].TermToSpeak == "FollowerInteractions/PoemConvo_Amateur_2/0") && (FollowerManager.AreRelated(this.follower.Brain.Info.ID, followerList[index].Brain.Info.ID) || followerList[index].Brain.Info.Age < 18 || followerList[index].Brain.Info.CursedState == Thought.Child))
        followerList.RemoveAt(index);
    }
    string name1;
    string name2;
    if (followerList.Count < 2)
    {
      name1 = FollowerInfo.GenerateName();
      name2 = FollowerInfo.GenerateName();
    }
    else
    {
      Follower follower = followerList[UnityEngine.Random.Range(0, followerList.Count)];
      name1 = follower.Brain.Info.Name;
      followerList.Remove(follower);
      name2 = followerList[UnityEngine.Random.Range(0, followerList.Count)].Brain.Info.Name;
    }
    switch (conversationEntry1[1].TermToSpeak)
    {
      case "FollowerInteractions/PoemConvo_Amateur_2/0":
      case "FollowerInteractions/PoemConvo_Amateur_3/0":
        conversationEntry1[1].TermToSpeak = string.Format(LocalizationManager.GetTranslation(conversationEntry1[1].TermToSpeak), (object) name1, (object) name2);
        break;
      case "FollowerInteractions/PoemConvo_Amateur_4/0":
      case "FollowerInteractions/PoemConvo_Mid_6/0":
        conversationEntry1[1].TermToSpeak = string.Format(LocalizationManager.GetTranslation(conversationEntry1[1].TermToSpeak), (object) DataManager.Instance.CultName);
        break;
      case "FollowerInteractions/PoemConvo_Mid_5/0":
      case "FollowerInteractions/PoemConvo_Pro_4/0":
      case "FollowerInteractions/Winter_PoemConvo_Amateur_3/0":
      case "FollowerInteractions/Winter_PoemConvo_Mid_1/0":
      case "FollowerInteractions/Winter_PoemConvo_Mid_3/0":
        conversationEntry1[1].TermToSpeak = string.Format(LocalizationManager.GetTranslation(conversationEntry1[1].TermToSpeak), (object) name1);
        break;
    }
    MMConversation.Play(new ConversationObject(conversationEntry1, (List<MMTools.Response>) null, (System.Action) (() =>
    {
      this.UnPause();
      this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
      this.StartCoroutine((IEnumerator) this.WaitFrameToClose());
    })));
    MMConversation.PlayVO = this.PlayVO;
    double num = (double) this.follower.SetBodyAnimation("worship-talk", true);
  }

  public IEnumerator WaitFrameToClose(System.Action callback = null)
  {
    yield return (object) null;
    this.follower.Brain.CompleteCurrentTask();
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void ConvertToWorker()
  {
    if (this.follower.Brain.Info.FollowerRole == FollowerRole.Worker)
      return;
    this.follower.Brain.Info.FollowerRole = FollowerRole.Worker;
    this.follower.Brain.Info.Outfit = FollowerOutfitType.Follower;
    this.follower.SetOutfit(FollowerOutfitType.Follower, false);
  }

  public void Close() => this.Close(true);

  public void Close(bool DoResetFollower, bool unpause = true, bool reshowMenu = true)
  {
    if (this.preventCloseOnRemovingPlayer && CoopManager.Instance.currentlyRemovingPlayer)
      return;
    if (unpause)
      this.UnPause();
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    if ((UnityEngine.Object) this.state != (UnityEngine.Object) null)
      this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.Activated = false;
    if (DoResetFollower)
      this.ResetFollower();
    BiomeConstants.Instance.DepthOfFieldTween(0.15f, 8.7f, 26f, 1f, 200f);
    if (MMConversation.CURRENT_CONVERSATION == null && !reshowMenu)
    {
      GameManager.GetInstance().OnConversationEnd();
      GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    }
    this.AutomaticallyInteract = false;
    GameManager.GetInstance().CamFollowTarget.SnappyMovement = false;
    if ((UnityEngine.Object) this.follower == (UnityEngine.Object) null || this.follower.Brain == null)
    {
      GameManager.GetInstance().OnConversationEnd();
      MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = (PlayerFarming) null;
      if (!(bool) (UnityEngine.Object) this.followerInteractionWheelInstance)
        return;
      this.followerInteractionWheelInstance.Hide();
    }
    else
    {
      if (this.follower.Brain._directInfoAccess.StartingCursedState == Thought.Ill)
        this.follower.Brain.MakeSick();
      else if (this.follower.Brain._directInfoAccess.StartingCursedState == Thought.BecomeStarving)
        this.follower.Brain.MakeStarve();
      else if (this.follower.Brain._directInfoAccess.StartingCursedState == Thought.Dissenter)
        this.follower.Brain.MakeDissenter();
      else if (this.follower.Brain._directInfoAccess.StartingCursedState == Thought.Freezing)
        this.follower.Brain.MakeFreezing();
      else if (this.follower.Brain._directInfoAccess.StartingCursedState == Thought.OldAge)
      {
        this.follower.Brain.ApplyCurseState(Thought.OldAge);
        this.follower.Brain.Info.Age = this.follower.Brain.Info.LifeExpectancy;
      }
      else if (this.follower.Brain._directInfoAccess.StartingCursedState == Thought.Injured)
        this.follower.Brain.MakeInjured();
      if (this.follower.Brain.Info.ID == 99997 && this.follower.Brain._directInfoAccess.StartingCursedState == Thought.Injured)
      {
        Follower followerById = FollowerManager.FindFollowerByID(99998);
        if ((UnityEngine.Object) followerById != (UnityEngine.Object) null && !FollowerManager.FollowerLocked(99998))
          this.follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_HugFollower(followerById.Brain));
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
      if ((UnityEngine.Object) this.follower.WorshipperBubble != (UnityEngine.Object) null)
      {
        this.follower.WorshipperBubble.StopAllCoroutines();
        this.follower.WorshipperBubble.Close();
      }
      this.ShowOtherFollowers();
      this.HasChanged = true;
      if (reshowMenu)
      {
        this.follower.AdorationUI.Hide();
        this.follower.PleasureUI.Hide();
        this.follower.GetComponentInChildren<UIFollowerName>()?.Hide();
        this.OnInteract(this.state);
      }
      else
      {
        if (this.follower.Brain.Info.CursedState != Thought.Child || this.follower.Brain.Info.Age < 18 || this.complaintType != Follower.ComplaintType.GiveOnboarding || this.follower.Brain.Info.ID == 100000)
          return;
        if (this.follower.Brain.Info.IsSnowman)
          this.follower.Brain.MakeAdult();
        else if (this.follower.Brain._directInfoAccess.BabyIgnored && !this.follower.Brain.HasTrait(FollowerTrait.TraitType.Mutated))
        {
          this.follower.Brain.MakeAdult();
          this.follower.Brain.MakeDissenter();
          this.follower.Brain.Stats.Reeducation = 100f;
          float itemQuantity = (float) Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD);
          this.follower.Brain.Stats.DissentGold = Mathf.Floor(UnityEngine.Random.Range(itemQuantity * 0.1f, itemQuantity * 0.25f));
          this.follower.Brain.LeavingCult = true;
        }
        else
          this.StartCoroutine((IEnumerator) this.SimpleNewRecruitRoutine());
      }
    }
  }

  public IEnumerator LevelUpRoutineTemple(PlayerFarming player)
  {
    interaction_FollowerInteraction followerInteraction = this;
    followerInteraction.follower.FacePosition(player.transform.position);
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
    if (player.isLamb)
      AudioManager.Instance.PlayOneShot("event:/player/speak_to_follower_noBookPage");
    else
      AudioManager.Instance.PlayOneShot("event:/player/goat_player/goat_speak_to_follower_noBookPage");
    AudioManager.Instance.PlayOneShot("event:/Stings/generic_positive", followerInteraction.transform.position);
    float increment;
    if (followerInteraction.follower.Brain.HasTrait(FollowerTrait.TraitType.BornToTheRot))
    {
      float rotToGive = (float) UnityEngine.Random.Range(5, 10);
      increment = 1f / rotToGive;
      for (int x = 0; (double) x < (double) rotToGive; ++x)
      {
        AudioManager.Instance.PlayOneShot("event:/followers/pop_in", followerInteraction.transform.position);
        ResourceCustomTarget.Create(followerInteraction.playerFarming.gameObject, followerInteraction.transform.position, InventoryItem.ITEM_TYPE.MAGMA_STONE, (System.Action) null);
        Inventory.ChangeItemQuantity(172, 1);
        yield return (object) new WaitForSeconds(increment * SpeedUpSequenceMultiplier);
      }
    }
    else
    {
      increment = 20f;
      while ((double) --increment >= 0.0)
      {
        if ((GameManager.HasUnlockAvailable() ? 1 : (DataManager.Instance.DeathCatBeaten ? 1 : 0)) != 0)
          SoulCustomTarget.Create(player.CameraBone, followerInteraction.follower.CameraBone.transform.position, Color.white, (System.Action) (() => player.GetSoul(1)));
        else
          InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, followerInteraction.transform.position + Vector3.back, 0.0f).SetInitialSpeedAndDiraction(8f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
        yield return (object) new WaitForSeconds(0.1f * SpeedUpSequenceMultiplier);
      }
    }
    yield return (object) new WaitForSeconds(0.2f);
    AudioManager.Instance.PlayOneShot("event:/followers/gain_loyalty", followerInteraction.transform.position);
    if (DoctrineUpgradeSystem.TrySermonsStillAvailable() && DoctrineUpgradeSystem.TryGetStillDoctrineStone() && DataManager.Instance.GetVariable(DataManager.Variables.FirstDoctrineStone))
    {
      switch (++DataManager.Instance.SpaceOutDoctrineStones % 3)
      {
        case 0:
          PickUp pickUp = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.DOCTRINE_STONE, 1, followerInteraction.transform.position);
          if ((UnityEngine.Object) pickUp != (UnityEngine.Object) null)
          {
            Interaction_DoctrineStone component = pickUp.GetComponent<Interaction_DoctrineStone>();
            if ((UnityEngine.Object) component != (UnityEngine.Object) null)
            {
              component.AutomaticallyInteract = true;
              component.MagnetToPlayer((UnityEngine.Object) player != (UnityEngine.Object) null ? player.gameObject : (GameObject) null);
              break;
            }
            break;
          }
          break;
      }
    }
    double num2 = (double) followerInteraction.follower.SetBodyAnimation("Reactions/react-bow", false);
    followerInteraction.follower.AddBodyAnimation("idle", true, 0.0f);
    followerInteraction.follower.HideAllFollowerIcons();
    yield return (object) new WaitForSeconds(0.5f);
    Debug.Log((object) "Complete task!");
    yield return (object) new WaitForSeconds(0.5f);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.LoyaltyCollectReward);
  }

  public void UnPause() => SimulationManager.UnPause();

  public void SpyCatch()
  {
    if (this.isLoadingAssets)
      return;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(this.gameObject, 6f);
    this.follower.SpeedMultiplier = 1f;
    this.follower.Brain.CheckChangeState();
    this.isLoadingAssets = true;
    this.StartCoroutine((IEnumerator) UIManager.LoadAssets(MonoSingleton<UIManager>.Instance.LoadRebuildBedMinigameAssets(), (System.Action) (() =>
    {
      this.isLoadingAssets = false;
      this._uiCookingMinigameOverlayController = MonoSingleton<UIManager>.Instance.RebuildBedMinigameOverlayControllerTemplate.Instantiate<UIRebuildBedMinigameOverlayController>();
      this._uiCookingMinigameOverlayController.Initialise("Interactions/Catch");
      this._uiCookingMinigameOverlayController.OnCook += new System.Action(this.SpyCatchSuccess);
      this._uiCookingMinigameOverlayController.OnUnderCook += new System.Action(this.SpyCatchFail);
      this._uiCookingMinigameOverlayController.OnBurn += new System.Action(this.SpyCatchFail);
      this.state.facingAngle = Utils.GetAngle(this.state.transform.position, this.transform.position);
    })));
  }

  public void SpyCatchSuccess()
  {
    AudioManager.Instance.PlayOneShot("event:/Stings/generic_positive");
    CameraManager.instance.ShakeCameraForDuration(0.1f, 0.2f, 0.5f);
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this._uiCookingMinigameOverlayController.OnCook -= new System.Action(this.SpyCatchSuccess);
    this._uiCookingMinigameOverlayController.OnUnderCook -= new System.Action(this.SpyCatchFail);
    this._uiCookingMinigameOverlayController.OnBurn -= new System.Action(this.SpyCatchFail);
    UnityEngine.Object.Destroy((UnityEngine.Object) this._uiCookingMinigameOverlayController.gameObject);
    this._uiCookingMinigameOverlayController = (UIRebuildBedMinigameOverlayController) null;
    GameManager.GetInstance().WaitForSeconds(0.3f, (System.Action) (() =>
    {
      List<ConversationEntry> Entries = new List<ConversationEntry>()
      {
        new ConversationEntry(this.gameObject, "FollowerInteractions/Spy/Caught/Success/0"),
        new ConversationEntry(this.gameObject, "FollowerInteractions/Spy/Caught/Success/1")
      };
      foreach (ConversationEntry conversationEntry in Entries)
      {
        conversationEntry.CharacterName = $"<color=yellow>{this.follower.Brain.Info.Name}</color>";
        conversationEntry.Speaker = this.follower.gameObject;
        conversationEntry.soundPath = this.generalTalkVO;
        conversationEntry.SkeletonData = this.follower.Spine;
        conversationEntry.pitchValue = this.follower.Brain._directInfoAccess.follower_pitch;
        conversationEntry.vibratoValue = this.follower.Brain._directInfoAccess.follower_vibrato;
        conversationEntry.SetZoom = true;
        conversationEntry.Zoom = 4f;
      }
      MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) (() => this.StartCoroutine((IEnumerator) this.SpyChoiceIE()))));
    }));
  }

  public IEnumerator SpyChoiceIE()
  {
    interaction_FollowerInteraction followerInteraction = this;
    yield return (object) new WaitForEndOfFrame();
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(followerInteraction.gameObject, 6f);
    GameObject g = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/UI/Choice Indicator"), GameObject.FindWithTag("Canvas").transform) as GameObject;
    ChoiceIndicator choice = g.GetComponent<ChoiceIndicator>();
    choice.Offset = new Vector3(0.0f, -300f);
    choice.Show("Interactions/Indoctrinate", "FollowerInteractions/Murder", new System.Action(followerInteraction.\u003CSpyChoiceIE\u003Eb__114_0), new System.Action(followerInteraction.\u003CSpyChoiceIE\u003Eb__114_1), followerInteraction.transform.position);
    while ((UnityEngine.Object) g != (UnityEngine.Object) null)
    {
      choice.UpdatePosition(followerInteraction.transform.position);
      yield return (object) null;
    }
  }

  public void SpyCatchFail()
  {
    CameraManager.instance.ShakeCameraForDuration(0.1f, 0.2f, 0.5f);
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this._uiCookingMinigameOverlayController.OnCook -= new System.Action(this.SpyCatchSuccess);
    this._uiCookingMinigameOverlayController.OnUnderCook -= new System.Action(this.SpyCatchFail);
    this._uiCookingMinigameOverlayController.OnBurn -= new System.Action(this.SpyCatchFail);
    UnityEngine.Object.Destroy((UnityEngine.Object) this._uiCookingMinigameOverlayController.gameObject);
    this._uiCookingMinigameOverlayController = (UIRebuildBedMinigameOverlayController) null;
    GameManager.GetInstance().WaitForSeconds(0.3f, (System.Action) (() =>
    {
      this.HasChanged = true;
      this.Interactable = false;
      this.enabled = false;
      GameManager.GetInstance().OnConversationEnd();
      this.CloseAndSpeak("Spy/Caught/Fail", PlayBow: false);
    }));
  }

  public IEnumerator OnboardRotstoneShrineIE()
  {
    interaction_FollowerInteraction followerInteraction = this;
    DataManager.Instance.OnboardedRotstone = true;
    GameManager.GetInstance().OnConversationNext(Interaction_DLCShrine.Instance.gameObject);
    yield return (object) new WaitForSeconds(1f);
    DataManager.Instance.YngyaOffering = -2;
    yield return (object) followerInteraction.StartCoroutine((IEnumerator) Interaction_DLCShrine.Instance.RotRevealIE());
    GameManager.GetInstance().OnConversationEnd();
  }

  public void HideOtherFollowers()
  {
  }

  public void ShowOtherFollowers()
  {
  }

  public IEnumerator SimpleNewRecruitRoutine()
  {
    interaction_FollowerInteraction followerInteraction = this;
    yield return (object) new WaitForEndOfFrame();
    while (MMConversation.isPlaying)
      yield return (object) null;
    if (followerInteraction.follower.Brain.Info.SkinName == "Abomination")
      DataManager.Instance.FollowerSkinsUnlocked.Add(followerInteraction.follower.Brain.Info.SkinName);
    if (followerInteraction.follower.Brain.Info.CursedState == Thought.Child && followerInteraction.follower.Brain.Info.ID != 100000)
      followerInteraction.follower.Brain.MakeAdult();
    FollowerBrain.SetFollowerCostume(followerInteraction.follower.Spine.Skeleton, followerInteraction.follower.Brain._directInfoAccess, forceUpdate: true);
    if (followerInteraction.follower.Brain.Info.ID == 100006 && followerInteraction.follower.Brain.Info.CursedState != Thought.Child && !DataManager.Instance.CompletedMidasFollowerQuest)
    {
      followerInteraction.StartCoroutine((IEnumerator) followerInteraction.MidasSequenceIE());
    }
    else
    {
      SimulationManager.Pause();
      GameManager.GetInstance().OnConversationNew();
      List<MeshRenderer> FollowersTurnedOff = new List<MeshRenderer>();
      GameManager.GetInstance().OnConversationNext(followerInteraction.gameObject, 4f);
      followerInteraction.playerFarming.state.facingAngle = Utils.GetAngle(followerInteraction.playerFarming.transform.position, followerInteraction.transform.position);
      yield return (object) new WaitForSeconds(0.3f);
      foreach (Follower locationFollower in FollowerManager.ActiveLocationFollowers())
      {
        SkeletonAnimation spine = locationFollower.Spine;
        if (spine.gameObject.activeSelf && (double) Vector3.Distance(spine.transform.position, followerInteraction.transform.position) < 1.0 && (double) spine.transform.position.y < (double) followerInteraction.transform.position.y)
        {
          Debug.Log((object) ("Turning off gameobject: " + spine.name));
          MeshRenderer component = spine.gameObject.GetComponent<MeshRenderer>();
          component.enabled = false;
          FollowersTurnedOff.Add(component);
        }
      }
      GameManager.GetInstance().CameraSetOffset(new Vector3(-2f, 0.0f, 0.0f));
      UIFollowerIndoctrinationMenuController indoctrinationMenuInstance = MonoSingleton<UIManager>.Instance.ShowIndoctrinationMenu(followerInteraction.follower, new OriginalFollowerLookData(followerInteraction.follower.Brain._directInfoAccess));
      indoctrinationMenuInstance.OnIndoctrinationCompleted += (System.Action) (() => this.StartCoroutine((IEnumerator) this.CharacterSetupCallback()));
      UIFollowerIndoctrinationMenuController indoctrinationMenuController1 = indoctrinationMenuInstance;
      indoctrinationMenuController1.OnShown = indoctrinationMenuController1.OnShown + (System.Action) (() =>
      {
        LightingManager.Instance.inOverride = true;
        this.LightingSettings.overrideLightingProperties = this.overrideLightingProperties;
        LightingManager.Instance.overrideSettings = this.LightingSettings;
        LightingManager.Instance.transitionDurationMultiplier = 0.0f;
        LightingManager.Instance.UpdateLighting(true);
      });
      UIFollowerIndoctrinationMenuController indoctrinationMenuController2 = indoctrinationMenuInstance;
      indoctrinationMenuController2.OnHide = indoctrinationMenuController2.OnHide + (System.Action) (() =>
      {
        foreach (Renderer renderer in FollowersTurnedOff)
          renderer.enabled = true;
        FollowersTurnedOff.Clear();
        LightingManager.Instance.inOverride = false;
        LightingManager.Instance.overrideSettings = (BiomeLightingSettings) null;
        LightingManager.Instance.transitionDurationMultiplier = 1f;
        LightingManager.Instance.lerpActive = false;
        LightingManager.Instance.UpdateLighting(true);
      });
      UIFollowerIndoctrinationMenuController indoctrinationMenuController3 = indoctrinationMenuInstance;
      indoctrinationMenuController3.OnHidden = indoctrinationMenuController3.OnHidden + (System.Action) (() => indoctrinationMenuInstance = (UIFollowerIndoctrinationMenuController) null);
    }
  }

  public IEnumerator MidasSequenceIE()
  {
    // ISSUE: reference to a compiler-generated field
    int num1 = this.\u003C\u003E1__state;
    interaction_FollowerInteraction followerInteraction = this;
    if (num1 != 0)
    {
      if (num1 != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      followerInteraction.follower.GoTo(followerInteraction._playerFarming.transform.position, (System.Action) null);
      GameManager.GetInstance().WaitForSeconds(0.5f, new System.Action(followerInteraction.\u003CMidasSequenceIE\u003Eb__120_0));
      GameManager.GetInstance().WaitForSeconds(2f, new System.Action(followerInteraction.\u003CMidasSequenceIE\u003Eb__120_1));
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(followerInteraction.follower.gameObject, 6f);
    BiomeConstants.Instance.EmitSmokeExplosionVFX(followerInteraction.transform.position, Color.white);
    double num2 = (double) followerInteraction.follower.SetBodyAnimation("Egg/mating-pose3", false);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(1f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator CharacterSetupCallback()
  {
    interaction_FollowerInteraction followerInteraction = this;
    followerInteraction.follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    if ((UnityEngine.Object) followerInteraction.state == (UnityEngine.Object) null)
    {
      followerInteraction.state = PlayerFarming.Instance.state;
      followerInteraction.playerFarming = PlayerFarming.Instance;
    }
    followerInteraction.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    followerInteraction.playerFarming.simpleSpineAnimator.Animate("recruit", 0, false);
    followerInteraction.playerFarming.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    followerInteraction.playerFarming.state.facingAngle = Utils.GetAngle(followerInteraction.playerFarming.transform.position, followerInteraction.transform.position);
    yield return (object) new WaitForSeconds(0.5f);
    AudioManager.Instance.PlayOneShot("event:/dialogue/followers/positive_acknowledge", followerInteraction.gameObject);
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", followerInteraction.playerFarming.gameObject);
    double num = (double) followerInteraction.follower.SetBodyAnimation("Indoctrinate/indoctrinate-finish", false);
    yield return (object) new WaitForSeconds(4f);
    followerInteraction.follower.SimpleAnimator?.ResetAnimationsToDefaults();
    yield return (object) new WaitForEndOfFrame();
    followerInteraction.state.CURRENT_STATE = StateMachine.State.Idle;
    GameManager.GetInstance().OnConversationEnd();
    FollowerBrain.SetFollowerCostume(followerInteraction.follower.Spine.Skeleton, followerInteraction.follower.Brain._directInfoAccess, forceUpdate: true);
    yield return (object) null;
    SimulationManager.UnPause();
    followerInteraction.OnRecruitFinished();
  }

  public static void CancelAllLightningStrikes()
  {
    foreach (Follower follower in Follower.Followers)
    {
      if (!((UnityEngine.Object) follower == (UnityEngine.Object) null))
      {
        interaction_FollowerInteraction component = follower.GetComponent<interaction_FollowerInteraction>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.lightningIncoming)
          component.CancelLightningStrike();
      }
    }
  }

  public void CancelLightningStrike()
  {
    if (this.lightningStrikeRoutine != null)
      this.StopCoroutine(this.lightningStrikeRoutine);
    this.lightningStrikeRoutine = (Coroutine) null;
    this.lightningIncoming = false;
    if ((UnityEngine.Object) this.LightningContainer != (UnityEngine.Object) null)
      this.LightningContainer.gameObject.SetActive(false);
    if ((UnityEngine.Object) HUD_Manager.Instance != (UnityEngine.Object) null)
      HUD_Manager.Instance.ClearLightningTarget();
    this.StartCoroutine((IEnumerator) this.EnsureLightningOff());
    AudioManager.Instance.StopLoop(this.lightningLoopSfx);
  }

  public IEnumerator EnsureLightningOff()
  {
    yield return (object) new WaitForSeconds(3f);
    if ((UnityEngine.Object) this.LightningContainer != (UnityEngine.Object) null)
      this.LightningContainer.gameObject.SetActive(false);
    if ((UnityEngine.Object) HUD_Manager.Instance != (UnityEngine.Object) null)
      HUD_Manager.Instance.ClearLightningTarget();
  }

  public void LightningStrikeIncoming()
  {
    if ((UnityEngine.Object) this.follower != (UnityEngine.Object) null)
    {
      foreach (Interaction_VolcanicSpa healingBay in Interaction_VolcanicSpa.HealingBays)
      {
        if ((UnityEngine.Object) healingBay != (UnityEngine.Object) null && healingBay.currentSpaOccupants != null && healingBay.currentSpaOccupants.Contains(this.follower))
          return;
      }
    }
    this.lightningIncoming = true;
    NotificationCentre.Instance.PlayFaithNotification("Notifications/FollowerLightningIncoming", 0.0f, NotificationBase.Flair.Negative, this.follower.Brain.Info.ID, this.follower.Brain.Info.Name);
    AudioManager.Instance.PlayOneShot("event:/weapon/crit_hit", PlayerFarming.Instance.gameObject);
    this.lightningStrikeRoutine = GameManager.GetInstance().StartCoroutine((IEnumerator) this.LightningStrikeIncomingIE());
  }

  public IEnumerator LightningStrikeIncomingIE()
  {
    this.struckByLightning = false;
    float timeUntilStrike = 15f;
    float time = 0.0f;
    Color indicatorColor = Color.white;
    float flashTickTimer = 0.0f;
    this.lightningLoopSfx = AudioManager.Instance.CreateLoop("event:/dlc/follower/lightning_warning_loop", this.follower.gameObject, true);
    this.LightningContainer.gameObject.SetActive(true);
    if (this.follower.Brain.CurrentTaskType != FollowerTaskType.Sleep || this.follower.Brain.CurrentTaskType != FollowerTaskType.SleepBedRest)
      this.follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_FakeLeisure());
    bool cancelled = false;
    while ((double) (time += Time.deltaTime) < (double) timeUntilStrike && PlayerFarming.Location == FollowerLocation.Base)
    {
      if (DataManager.Instance.Followers_Dead_IDs.Contains(this.follower.Brain.Info.ID))
        yield break;
      flashTickTimer += Time.deltaTime;
      if ((double) flashTickTimer >= 0.11999999731779099 && BiomeConstants.Instance.IsFlashLightsActive)
      {
        indicatorColor = indicatorColor == Color.white ? Color.red : Color.white;
        this.LightningIndicator.material.SetColor("_Color", indicatorColor);
        this.LightningIndicator2.material.SetColor("_Color", indicatorColor);
        flashTickTimer = 0.0f;
      }
      if ((double) Time.time < (double) interaction_FollowerInteraction.preventLightningStrikeTimestamp)
      {
        cancelled = true;
        break;
      }
      yield return (object) null;
    }
    AudioManager.Instance.StopLoop(this.lightningLoopSfx);
    this.lightningStrikeRoutine = (Coroutine) null;
    this.lightningIncoming = false;
    if (cancelled)
    {
      Debug.Log((object) "Queued Lightning cancelled");
      this.LightningContainer.gameObject.SetActive(false);
    }
    else
      this.LightningStrike();
  }

  public IEnumerator ProtectFromLightningIE()
  {
    interaction_FollowerInteraction followerInteraction = this;
    AudioManager.Instance.PlayOneShot("event:/dlc/follower/lightning_protect", followerInteraction.transform.position);
    AudioManager.Instance.StopLoop(followerInteraction.lightningLoopSfx);
    if ((UnityEngine.Object) HUD_Manager.Instance != (UnityEngine.Object) null)
      HUD_Manager.Instance.ClearLightningTarget();
    if (followerInteraction.lightningStrikeRoutine != null)
      followerInteraction.StopCoroutine(followerInteraction.lightningStrikeRoutine);
    followerInteraction.lightningStrikeRoutine = (Coroutine) null;
    followerInteraction.lightningIncoming = false;
    followerInteraction.follower.ClearPath();
    followerInteraction.follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(followerInteraction.playerFarming.gameObject, 7f);
    followerInteraction.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    followerInteraction.playerFarming.simpleSpineAnimator.Animate("sermons/sermon-start-nobook", 0, false);
    followerInteraction.playerFarming.transform.DOMove(followerInteraction.transform.position, 0.5f);
    followerInteraction.playerFarming.Spine.transform.DOLocalMove(Vector3.back / 2f, 0.5f);
    yield return (object) new WaitForEndOfFrame();
    double num = (double) followerInteraction.follower.SetBodyAnimation("Scared/scared-scream", false);
    if ((double) UnityEngine.Random.value < 0.5)
      followerInteraction.follower.AddBodyAnimation("Reactions/react-embarrassed", false, 0.0f);
    else
      followerInteraction.follower.AddBodyAnimation("Reactions/react-enlightened" + UnityEngine.Random.Range(1, 3).ToString(), false, 0.0f);
    followerInteraction.follower.AddBodyAnimation("idle", false, 0.0f);
    yield return (object) new WaitForSeconds(1.26666665f);
    WeatherSystemController.Instance.TriggerLightningStrike(followerInteraction.transform.position + Vector3.back * 1.5f);
    yield return (object) new WaitForSeconds(0.2f);
    followerInteraction.LambHitSequence(followerInteraction.follower);
    followerInteraction.playerFarming.Spine.transform.localPosition = Vector3.zero;
    GameManager.GetInstance().OnConversationNext(followerInteraction.playerFarming.gameObject);
    InventoryItem.Spawn(InventoryItem.ITEM_TYPE.LIGHTNING_SHARD, UnityEngine.Random.Range(3, 6), followerInteraction.transform.position);
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().OnConversationEnd();
    followerInteraction.follower.Brain.CompleteCurrentTask();
  }

  public void LambHitSequence(Follower follower)
  {
    PlayerFarming instance = PlayerFarming.Instance;
    AudioManager.Instance.PlayOneShot("event:/player/gethit", instance.transform.position);
    BiomeConstants.Instance.EmitHitVFX(instance.transform.position, Quaternion.identity.z, "HitFX_Blocked");
    instance.simpleSpineAnimator.FlashWhite(false);
    CameraManager.instance.ShakeCameraForDuration(1.5f, 1.7f, 0.2f);
    instance.state.facingAngle = Utils.GetAngle(PlayerFarming.Instance.transform.position, follower.transform.position);
    instance.state.CURRENT_STATE = StateMachine.State.HitThrown;
    instance.playerController.MakeUntouchable(1f, false);
  }

  public void LightningStrike()
  {
    if (this.struckByLightning)
      return;
    this.struckByLightning = true;
    WeatherSystemController.Instance.TriggerLightningStrike(this.follower.Brain);
  }

  public void OnRecruitFinished()
  {
    GameManager.GetInstance().OnConversationEnd();
    this.follower.Brain.CompleteCurrentTask();
    SimulationManager.UnPause();
    FollowerBrain.SetFollowerCostume(this.follower.Spine.Skeleton, this.follower.Brain._directInfoAccess, forceUpdate: true);
    TwitchFollowers.SendFollowers();
  }

  [CompilerGenerated]
  public void \u003CLevelUpRoutine\u003Eb__60_0() => this.playerFarming.GetSoul(1);

  [CompilerGenerated]
  public void \u003CNamedCult\u003Eb__65_0()
  {
    this.eventListener.PlayFollowerVO(this.positiveAcknowledgeVO);
    this.follower.TimedAnimation("Reactions/react-happy1", 1.86666667f, (System.Action) (() =>
    {
      if (this.follower.Brain.GetWillLevelUp(FollowerBrain.AdorationActions.Quest))
        return;
      this.follower.Brain.CompleteCurrentTask();
    }));
    this.follower.Brain.AddThought(Thought.LeaderDidQuest);
    AudioManager.Instance.PlayOneShot("event:/followers/gain_loyalty", this.gameObject.transform.position);
    this.StartCoroutine((IEnumerator) this.FrameDelayCallback((System.Action) (() =>
    {
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(this.gameObject, 4f);
      this.follower.Brain.AddAdoration(FollowerBrain.AdorationActions.Quest, (System.Action) (() =>
      {
        if (this.follower.Brain.CanLevelUp())
          this.StartCoroutine((IEnumerator) this.LevelUpRoutine(this.previousTaskType, GoToAndStop: false));
        else
          this.Close();
      }));
    })));
  }

  [CompilerGenerated]
  public void \u003CNamedCult\u003Eb__65_1()
  {
    if (this.follower.Brain.GetWillLevelUp(FollowerBrain.AdorationActions.Quest))
      return;
    this.follower.Brain.CompleteCurrentTask();
  }

  [CompilerGenerated]
  public void \u003CNamedCult\u003Eb__65_2()
  {
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(this.gameObject, 4f);
    this.follower.Brain.AddAdoration(FollowerBrain.AdorationActions.Quest, (System.Action) (() =>
    {
      if (this.follower.Brain.CanLevelUp())
        this.StartCoroutine((IEnumerator) this.LevelUpRoutine(this.previousTaskType, GoToAndStop: false));
      else
        this.Close();
    }));
  }

  [CompilerGenerated]
  public void \u003CNamedCult\u003Eb__65_3()
  {
    if (this.follower.Brain.CanLevelUp())
      this.StartCoroutine((IEnumerator) this.LevelUpRoutine(this.previousTaskType, GoToAndStop: false));
    else
      this.Close();
  }

  [CompilerGenerated]
  public void \u003CSelectTaskRoutine\u003Eb__71_0()
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.OnboardRotstoneShrineIE());
  }

  [CompilerGenerated]
  public void \u003CSelectTaskRoutine\u003Eb__71_2()
  {
    CoopManager.Instance.OnPlayerLeft -= new System.Action(this.UIOnPlayerLeft);
    if (!this.follower.Brain.HasTrait(FollowerTrait.TraitType.Mutated) || !DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.MutatedFollower))
      return;
    GameManager.GetInstance().OnConversationNew();
    MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.MutatedFollower, callback: (System.Action) (() => GameManager.GetInstance().StartCoroutine((IEnumerator) this.OnboardRotstoneShrineIE())));
  }

  [CompilerGenerated]
  public void \u003CSelectTaskRoutine\u003Eb__71_3()
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.OnboardRotstoneShrineIE());
  }

  [CompilerGenerated]
  public void \u003CSelectTaskRoutine\u003Eb__71_1()
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.OnboardRotstoneShrineIE());
  }

  [CompilerGenerated]
  public void \u003CReeducateRoutine\u003Eb__87_0() => this.Close();

  [CompilerGenerated]
  public void \u003CCuddleBabyRoutine\u003Eb__89_0()
  {
    if (this.follower.Brain.CanLevelUp())
      this.StartCoroutine((IEnumerator) this.LevelUpRoutine(this.previousTaskType));
    else
      this.Close();
  }

  [CompilerGenerated]
  public void \u003CPetDogRoutine\u003Eb__90_0()
  {
    if (this.follower.Brain.CanLevelUp())
      this.StartCoroutine((IEnumerator) this.LevelUpRoutine(this.previousTaskType));
    else
      this.Close();
  }

  [CompilerGenerated]
  public void \u003CReassureRoutine\u003Eb__91_0()
  {
    if (this.follower.Brain.CanLevelUp())
      this.StartCoroutine((IEnumerator) this.LevelUpRoutine(this.previousTaskType));
    else
      this.Close();
  }

  [CompilerGenerated]
  public void \u003CBullyRoutine\u003Eb__92_0()
  {
    this.follower.Brain.AddThought(Thought.Intimidated);
    if (this.follower.Brain.CanLevelUp())
      this.StartCoroutine((IEnumerator) this.LevelUpRoutine(this.previousTaskType));
    else
      this.Close();
  }

  [CompilerGenerated]
  public void \u003CBribeRoutine\u003Eb__98_0()
  {
    double num = (double) this.follower.SetBodyAnimation("Reactions/react-love2", false);
    this.follower.AddBodyAnimation("idle", true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/followers/gain_loyalty", this.gameObject.transform.position);
    this.follower.Brain.Stats.Bribed = true;
    Inventory.ChangeItemQuantity(20, -3);
    this.follower.Brain.AddAdoration(FollowerBrain.AdorationActions.Bribe, (System.Action) (() =>
    {
      if (this.follower.Brain.CanLevelUp())
      {
        Debug.Log((object) ("previousTaskType: " + this.previousTaskType.ToString()));
        this.StartCoroutine((IEnumerator) this.LevelUpRoutine(this.previousTaskType, new System.Action(this.Close), onFinishClose: false));
      }
      else
        this.Close();
    }));
  }

  [CompilerGenerated]
  public void \u003CBribeRoutine\u003Eb__98_1()
  {
    if (this.follower.Brain.CanLevelUp())
    {
      Debug.Log((object) ("previousTaskType: " + this.previousTaskType.ToString()));
      this.StartCoroutine((IEnumerator) this.LevelUpRoutine(this.previousTaskType, new System.Action(this.Close), onFinishClose: false));
    }
    else
      this.Close();
  }

  [CompilerGenerated]
  public void \u003CGivePoem\u003Eb__102_0()
  {
    this.UnPause();
    this.eventListener.PlayFollowerVO(this.generalAcknowledgeVO);
    this.StartCoroutine((IEnumerator) this.WaitFrameToClose());
  }

  [CompilerGenerated]
  public void \u003CSpyCatch\u003Eb__112_0()
  {
    this.isLoadingAssets = false;
    this._uiCookingMinigameOverlayController = MonoSingleton<UIManager>.Instance.RebuildBedMinigameOverlayControllerTemplate.Instantiate<UIRebuildBedMinigameOverlayController>();
    this._uiCookingMinigameOverlayController.Initialise("Interactions/Catch");
    this._uiCookingMinigameOverlayController.OnCook += new System.Action(this.SpyCatchSuccess);
    this._uiCookingMinigameOverlayController.OnUnderCook += new System.Action(this.SpyCatchFail);
    this._uiCookingMinigameOverlayController.OnBurn += new System.Action(this.SpyCatchFail);
    this.state.facingAngle = Utils.GetAngle(this.state.transform.position, this.transform.position);
  }

  [CompilerGenerated]
  public void \u003CSpyCatchSuccess\u003Eb__113_0()
  {
    List<ConversationEntry> Entries = new List<ConversationEntry>()
    {
      new ConversationEntry(this.gameObject, "FollowerInteractions/Spy/Caught/Success/0"),
      new ConversationEntry(this.gameObject, "FollowerInteractions/Spy/Caught/Success/1")
    };
    foreach (ConversationEntry conversationEntry in Entries)
    {
      conversationEntry.CharacterName = $"<color=yellow>{this.follower.Brain.Info.Name}</color>";
      conversationEntry.Speaker = this.follower.gameObject;
      conversationEntry.soundPath = this.generalTalkVO;
      conversationEntry.SkeletonData = this.follower.Spine;
      conversationEntry.pitchValue = this.follower.Brain._directInfoAccess.follower_pitch;
      conversationEntry.vibratoValue = this.follower.Brain._directInfoAccess.follower_vibrato;
      conversationEntry.SetZoom = true;
      conversationEntry.Zoom = 4f;
    }
    MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) (() => this.StartCoroutine((IEnumerator) this.SpyChoiceIE()))));
  }

  [CompilerGenerated]
  public void \u003CSpyCatchSuccess\u003Eb__113_1()
  {
    this.StartCoroutine((IEnumerator) this.SpyChoiceIE());
  }

  [CompilerGenerated]
  public void \u003CSpyChoiceIE\u003Eb__114_0()
  {
    this.follower.Brain.LeavingCult = false;
    this.follower.Brain.Info.Traits.Remove(FollowerTrait.TraitType.Spy);
    this.follower.Brain.Info.Traits.Add(FollowerTrait.TraitType.CriminalReformed);
    this.StartCoroutine((IEnumerator) this.SimpleNewRecruitRoutine());
  }

  [CompilerGenerated]
  public void \u003CSpyChoiceIE\u003Eb__114_1()
  {
    this.StartCoroutine((IEnumerator) this.MurderFollower());
  }

  [CompilerGenerated]
  public void \u003CSpyCatchFail\u003Eb__115_0()
  {
    this.HasChanged = true;
    this.Interactable = false;
    this.enabled = false;
    GameManager.GetInstance().OnConversationEnd();
    this.CloseAndSpeak("Spy/Caught/Fail", PlayBow: false);
  }

  [CompilerGenerated]
  public void \u003CMidasSequenceIE\u003Eb__120_0()
  {
    DataManager.Instance.TimeSinceLastStolenCoins = TimeManager.TotalElapsedGameTime + 0.5f;
    this.follower.GetComponentInChildren<FollowerSpineEventListener>().ReplaceEvent("VO/Laugh", "event:/dialogue/midas/laugh_adult");
    this.follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_StealFromLamb());
  }

  [CompilerGenerated]
  public void \u003CMidasSequenceIE\u003Eb__120_1()
  {
    this.follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    Vector3 targetPos = new Vector3((double) this.follower.transform.position.x > 0.0 ? PlacementRegion.X_Constraints.y : PlacementRegion.X_Constraints.x, this.follower.transform.position.y, 0.0f);
    this.follower.Brain.CurrentState = (FollowerState) new FollowerState_Midas();
    this.follower.OverridingEmotions = true;
    this.follower.SetFaceAnimation("Emotions/emotion-happy", true);
    this.follower.SpeedMultiplier = 2.5f;
    SimulationManager.Pause();
    this.follower.GoTo(targetPos, (System.Action) (() =>
    {
      MMConversation.mmConversation.Close();
      this.follower.Spine.AnimationState.SetAnimation(1, "Midas/jump", false);
      AudioManager.Instance.PlayOneShot("event:/dlc/env/midas/jump_launch", this.follower.gameObject);
      this.follower.transform.DOMoveX(this.follower.transform.position.x + ((double) targetPos.x > 0.0 ? 2f : -2f), 0.72f).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(0.36f).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
      {
        AudioManager.Instance.PlayOneShot("event:/dlc/env/midas/jump_land", this.follower.gameObject);
        SimulationManager.UnPause();
        DataManager.Instance.MidasHiddenDay = TimeManager.CurrentDay;
        DataManager.Instance.HasMidasHiding = true;
        DataManager.Instance.MidasFollowerInfo = this.follower.Brain._directInfoAccess;
        GameManager.GetInstance().OnConversationEnd();
        MidasBaseController.Instance.SpawnFakeMidas();
        FollowerManager.RemoveFollower(100006);
        FollowerManager.RemoveFollowerBrain(100006);
        Follower.Followers.Remove(this.follower);
        UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
      }));
      this.follower.Spine.transform.DOLocalMoveY(2f, 0.36f).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(0.36f);
      this.follower.Spine.transform.DOLocalMoveY(0.0f, 0.36f).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(0.72f);
    }));
    MMConversation.PlayBark(new ConversationObject(new List<ConversationEntry>()
    {
      new ConversationEntry(this.follower.gameObject, "Conversation_NPC/Midas_ReadyToBecomeAdult/0", "Midas/run-away", "event:/dialogue/midas/laugh_adult")
    }, (List<MMTools.Response>) null, (System.Action) null));
  }

  [CompilerGenerated]
  public void \u003CMidasSequenceIE\u003Eb__120_3()
  {
    AudioManager.Instance.PlayOneShot("event:/dlc/env/midas/jump_land", this.follower.gameObject);
    SimulationManager.UnPause();
    DataManager.Instance.MidasHiddenDay = TimeManager.CurrentDay;
    DataManager.Instance.HasMidasHiding = true;
    DataManager.Instance.MidasFollowerInfo = this.follower.Brain._directInfoAccess;
    GameManager.GetInstance().OnConversationEnd();
    MidasBaseController.Instance.SpawnFakeMidas();
    FollowerManager.RemoveFollower(100006);
    FollowerManager.RemoveFollowerBrain(100006);
    Follower.Followers.Remove(this.follower);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }
}
