// Decompiled with JetBrains decompiler
// Type: FollowerTask_FightFollower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using I2.Loc;
using MMTools;
using Spine;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerTask_FightFollower : FollowerTask
{
  public int _otherFollowerID;
  public FollowerTask_FightFollower OtherChatTask;
  public bool _isLeader;
  public bool _isSpeaker;
  public FollowerTask_FightFollower.ConvoStage _convoStage;
  public FollowerTask_FightFollower.ConvoOutcome _convoOutcome;
  public float _doingTimer;
  public Coroutine _greetCoroutine;
  public bool _interrupted;
  [CompilerGenerated]
  public bool \u003CFinishedMurderCallback\u003Ek__BackingField;
  [CompilerGenerated]
  public Vector3 \u003CChatPosition\u003Ek__BackingField;
  public float progress;
  public float timeBetweenPleasure = 3f;
  public bool isEatingOtherFollower;
  public bool coroutineHit;
  public Follower follower;
  public Vector3 smokePosition;
  public float emitSmokeTimer;
  public EventInstance loop;
  public bool playingLoop;
  public static Action<FollowerInfo, FollowerInfo, IDAndRelationship.RelationshipState> OnChangeRelationship;

  public override FollowerTaskType Type => FollowerTaskType.FightFollower;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockTaskChanges => true;

  public override bool BlockReactTasks => true;

  public override bool BlockSocial => true;

  public override bool BlockThoughts => true;

  public bool Interrupted => this._interrupted;

  public bool FinishedMurderCallback
  {
    get => this.\u003CFinishedMurderCallback\u003Ek__BackingField;
    set => this.\u003CFinishedMurderCallback\u003Ek__BackingField = value;
  }

  public Vector3 ChatPosition
  {
    get => this.\u003CChatPosition\u003Ek__BackingField;
    set => this.\u003CChatPosition\u003Ek__BackingField = value;
  }

  public FollowerTask_FightFollower(int followerID, bool leader)
  {
    this._otherFollowerID = followerID;
    this._isLeader = leader;
  }

  public override int GetSubTaskCode() => this._otherFollowerID;

  public override void OnStart()
  {
    if (this._isLeader)
    {
      this.OtherChatTask = new FollowerTask_FightFollower(this._brain.Info.ID, false);
      this.OtherChatTask.OtherChatTask = this;
      FollowerBrain brainById = FollowerBrain.FindBrainByID(this._otherFollowerID);
      if (brainById != null)
      {
        brainById.HardSwapToTask((FollowerTask) this.OtherChatTask);
      }
      else
      {
        this.End();
        return;
      }
    }
    this.isEatingOtherFollower = this.Brain.Info.ID == 99996 && this.OtherChatTask.Brain.Info.SkinName == "Mushroom" || this.OtherChatTask.Brain.Info.ID == 99996 && this.Brain.Info.SkinName == "Mushroom";
    if (!this.isEatingOtherFollower)
      this.isEatingOtherFollower = this.Brain.Info.HasTrait(FollowerTrait.TraitType.Zombie) && !this.OtherChatTask.Brain.HasTrait(FollowerTrait.TraitType.Zombie) || this.OtherChatTask.Brain.HasTrait(FollowerTrait.TraitType.Zombie) && !this.Brain.HasTrait(FollowerTrait.TraitType.Zombie);
    int locationState = (int) LocationManager.GetLocationState(this.Location);
    Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    this.ChatPosition = locationState != 3 || !((UnityEngine.Object) followerById != (UnityEngine.Object) null) ? TownCentre.RandomPositionInCachedTownCentre() : followerById.transform.position;
    if (this.OtherChatTask.State != FollowerTaskState.Done)
    {
      FollowerTask_FightFollower otherChatTask = this.OtherChatTask;
      otherChatTask.OnFollowerTaskStateChanged = otherChatTask.OnFollowerTaskStateChanged + new FollowerTask.FollowerTaskDelegate(this.OnTaskStateChanged);
      this.SetState(FollowerTaskState.GoingTo);
    }
    else
    {
      this.OtherChatTask.End();
      this.End();
    }
  }

  public override void OnArrive()
  {
    ++this._brain._directInfoAccess.FollowersFought;
    if (this.isEatingOtherFollower && this._isLeader)
    {
      this._convoStage = FollowerTask_FightFollower.ConvoStage.Murder;
      this._isSpeaker = true;
      this.OtherChatTask._convoStage = FollowerTask_FightFollower.ConvoStage.Murder;
      this.SetState(FollowerTaskState.Idle);
      this.OtherChatTask.SetState(FollowerTaskState.Idle);
    }
    else if (this._isLeader)
    {
      if (!this.isEatingOtherFollower)
      {
        this.CommenceConvo();
        this.OtherChatTask.CommenceConvo();
      }
      this.SetState(FollowerTaskState.Idle);
    }
    else
      this.SetState(FollowerTaskState.Idle);
  }

  public override void OnComplete()
  {
    FollowerTask_FightFollower otherChatTask = this.OtherChatTask;
    otherChatTask.OnFollowerTaskStateChanged = otherChatTask.OnFollowerTaskStateChanged - new FollowerTask.FollowerTaskDelegate(this.OnTaskStateChanged);
  }

  public override void OnAbort()
  {
    base.OnAbort();
    this._interrupted = true;
    AudioManager.Instance.StopLoop(this.loop);
    if (!((UnityEngine.Object) HUD_Manager.Instance != (UnityEngine.Object) null))
      return;
    HUD_Manager.Instance.ClearFightingTarget();
  }

  public void CommenceConvo()
  {
    this._convoStage = FollowerTask_FightFollower.ConvoStage.Chat1;
    if (!this._isLeader)
      return;
    Follower followerById1 = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    Follower followerById2 = FollowerManager.FindFollowerByID(this._otherFollowerID);
    if ((UnityEngine.Object) HUD_Manager.Instance != (UnityEngine.Object) null)
      HUD_Manager.Instance.SetFightingTarget(new HUD_Manager.FightingTarget((followerById1.transform.position + followerById2.transform.position) / 2f + Vector3.up));
    NotificationCentre.Instance.PlayFaithNotification("Notifications/FollowersStartedFighting", 0.0f, NotificationBase.Flair.Negative, -1, followerById1.Brain.Info.Name, followerById2.Brain.Info.Name);
    AudioManager.Instance.PlayOneShot("event:/weapon/crit_hit", PlayerFarming.Instance.gameObject);
    this.smokePosition = (followerById1.transform.position + followerById2.transform.position) / 2f;
    if ((bool) (UnityEngine.Object) followerById1 && (bool) (UnityEngine.Object) followerById2 && (UnityEngine.Object) followerById1.gameObject.GetComponent<Interaction_BreakUpFight>() == (UnityEngine.Object) null)
    {
      Interaction_BreakUpFight interactionBreakUpFight = followerById1.gameObject.AddComponent<Interaction_BreakUpFight>();
      interactionBreakUpFight.Init(followerById1);
      interactionBreakUpFight.LockPosition = followerById1.transform;
      followerById1.AddThought((Thought) UnityEngine.Random.Range(331, 335));
      followerById2.AddThought((Thought) UnityEngine.Random.Range(331, 335));
    }
    if ((bool) (UnityEngine.Object) followerById1 && (bool) (UnityEngine.Object) followerById2 && (UnityEngine.Object) followerById2.gameObject.GetComponent<Interaction_BreakUpFight>() == (UnityEngine.Object) null)
    {
      Interaction_BreakUpFight interactionBreakUpFight = followerById2.gameObject.AddComponent<Interaction_BreakUpFight>();
      interactionBreakUpFight.Init(followerById2);
      interactionBreakUpFight.LockPosition = followerById2.transform;
    }
    if (this.isEatingOtherFollower)
      return;
    FollowerManager.TryStopFollowerFight(this._brain.Info.ID, this._otherFollowerID);
  }

  public void NextConvoStage()
  {
    ++this._convoStage;
    if (!(this._convoStage == FollowerTask_FightFollower.ConvoStage.Murder & true))
      ++this._convoStage;
    if (this._convoStage >= FollowerTask_FightFollower.ConvoStage.Murder)
      AudioManager.Instance.StopLoop(this.loop);
    if (!this._isLeader)
    {
      IDAndRelationship relationship = this._brain.Info.GetOrCreateRelationship(this._otherFollowerID);
      relationship.Relationship = -10;
      relationship.CurrentRelationshipState = IDAndRelationship.RelationshipState.Enemies;
      this._brain.AddThought(Thought.NewEnemy);
    }
    this.SetState(FollowerTaskState.Idle);
  }

  public override void TaskTick(float deltaGameTime)
  {
    if (this.State == FollowerTaskState.Idle)
    {
      if (this._convoStage >= FollowerTask_FightFollower.ConvoStage.Finished)
        this.End();
      else if (this._convoStage != FollowerTask_FightFollower.ConvoStage.None)
      {
        switch (this._convoStage)
        {
          case FollowerTask_FightFollower.ConvoStage.Chat1:
            this._isSpeaker = this._isLeader;
            break;
          case FollowerTask_FightFollower.ConvoStage.Finale:
            this._isSpeaker = this._isLeader;
            break;
        }
        this.SetState(FollowerTaskState.Doing);
      }
    }
    else if (this.State == FollowerTaskState.Doing && this._isSpeaker && (double) (this._doingTimer -= deltaGameTime) <= 0.0)
    {
      this.NextConvoStage();
      this.OtherChatTask.NextConvoStage();
    }
    if (this._convoStage != FollowerTask_FightFollower.ConvoStage.Chat1 || PlayerFarming.Location != FollowerLocation.Base || !this._isLeader)
      return;
    this.emitSmokeTimer -= deltaGameTime;
    if ((double) this.emitSmokeTimer > 0.0)
      return;
    BiomeConstants.Instance.EmitSmokeInteractionVFX(this.smokePosition, Vector3.one);
    this.emitSmokeTimer = 1f;
  }

  public void OnTaskStateChanged(FollowerTaskState oldState, FollowerTaskState newState)
  {
    if (newState != FollowerTaskState.Done)
      return;
    this.End();
  }

  public void UndoStateAnimationChanges(Follower follower)
  {
    SimpleSpineAnimator.SpineChartacterAnimationData animationData = follower.SimpleAnimator.GetAnimationData(StateMachine.State.Moving);
    animationData.Animation = animationData.DefaultAnimation;
    follower.ResetStateAnimations();
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    Vector3 vector3;
    if (this._isLeader)
    {
      FollowerBrain brainById = FollowerBrain.FindBrainByID(this._otherFollowerID);
      if (brainById != null && brainById.CurrentTaskType == FollowerTaskType.FightFollower)
      {
        FollowerTask_FightFollower currentTask = (FollowerTask_FightFollower) brainById.CurrentTask;
        vector3 = currentTask.ChatPosition + Vector3.right * ((double) currentTask.ChatPosition.x < (double) follower.transform.position.x ? 0.6f : -0.6f);
      }
      else
        vector3 = !((UnityEngine.Object) follower != (UnityEngine.Object) null) ? this._brain.LastPosition : follower.transform.position;
    }
    else
      vector3 = this.ChatPosition;
    return vector3;
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    if (this.State != FollowerTaskState.Doing)
      return;
    Follower followerById = FollowerManager.FindFollowerByID(this._otherFollowerID);
    if ((bool) (UnityEngine.Object) followerById && (bool) (UnityEngine.Object) follower && (UnityEngine.Object) follower.State != (UnityEngine.Object) null)
      follower.State.facingAngle = Utils.GetAngle(follower.transform.position, followerById.transform.position);
    if (this.Interrupted || this._convoStage != FollowerTask_FightFollower.ConvoStage.Chat1)
      return;
    if ((UnityEngine.Object) follower != (UnityEngine.Object) null && (UnityEngine.Object) follower.GetComponent<Interaction_BreakUpFight>() == (UnityEngine.Object) null)
      follower.gameObject.AddComponent<Interaction_BreakUpFight>().Init(follower);
    if (!((UnityEngine.Object) followerById != (UnityEngine.Object) null) || !((UnityEngine.Object) followerById.GetComponent<Interaction_BreakUpFight>() == (UnityEngine.Object) null))
      return;
    followerById.gameObject.AddComponent<Interaction_BreakUpFight>().Init(followerById);
  }

  public override void OnGoingToBegin(Follower follower)
  {
    base.OnGoingToBegin(follower);
    if (!this._isLeader)
      return;
    Follower followerById = FollowerManager.FindFollowerByID(this._otherFollowerID);
    this._greetCoroutine = follower.StartCoroutine(this.WaitForGreetCoroutine(follower, followerById));
    this._brain.Info.GetOrCreateRelationship(this._otherFollowerID);
    followerById.Brain.Info.GetOrCreateRelationship(this._brain.Info.ID);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, this.GetWalkpastAnim());
    followerById.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    double num = (double) followerById.SetBodyAnimation(this.GetGreetAnim(), true);
    followerById.FacePosition(follower.transform.position);
  }

  public override void OnDoingBegin(Follower follower)
  {
    this.UndoStateAnimationChanges(follower);
    Follower followerById = FollowerManager.FindFollowerByID(this._otherFollowerID);
    if ((UnityEngine.Object) followerById == (UnityEngine.Object) null || (UnityEngine.Object) follower == (UnityEngine.Object) null)
    {
      this.End();
    }
    else
    {
      if (this.isEatingOtherFollower && this._convoStage != FollowerTask_FightFollower.ConvoStage.Murder)
        this._convoStage = FollowerTask_FightFollower.ConvoStage.Murder;
      followerById.FacePosition(follower.transform.position);
      follower.FacePosition(followerById.transform.position);
      if (this._isSpeaker && !this.playingLoop)
      {
        this.playingLoop = true;
        this.loop = AudioManager.Instance.CreateLoop("event:/followers/fight_loop", follower.gameObject, true);
      }
      IDAndRelationship relationship = this._brain.Info.GetOrCreateRelationship(this._otherFollowerID);
      switch (this._convoStage)
      {
        case FollowerTask_FightFollower.ConvoStage.None:
          throw new ArgumentException("Doing unstarted convo!!");
        case FollowerTask_FightFollower.ConvoStage.Chat1:
          if (this._isSpeaker)
          {
            follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
            double num = (double) follower.SetBodyAnimation(this.GetFightAnim(), true);
            this._doingTimer = this.ConvertAnimTimeToGameTime(follower.SimpleAnimator.Duration()) * 20f;
            break;
          }
          follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
          double num1 = (double) follower.SetBodyAnimation(this.GetFightAnim(), true);
          break;
        case FollowerTask_FightFollower.ConvoStage.Murder:
          AudioManager.Instance.StopLoop(this.loop);
          if ((bool) (UnityEngine.Object) follower.GetComponent<Interaction_BreakUpFight>())
          {
            Interaction_BreakUpFight component = follower.GetComponent<Interaction_BreakUpFight>();
            component.Interactable = false;
            component.HasChanged = true;
          }
          this.follower = follower;
          if (this.isEatingOtherFollower && this._isLeader && !this.coroutineHit && (this.Brain.Info.ID == 99996 || this.Brain.HasTrait(FollowerTrait.TraitType.Zombie)))
          {
            GameManager.GetInstance().StartCoroutine(this.EatSequence(new System.Action(this.MurderCallback)));
            break;
          }
          this.MurderCallback();
          break;
        case FollowerTask_FightFollower.ConvoStage.Finale:
          this.DoFinale(follower, relationship);
          break;
      }
    }
  }

  public void AnimationState_Event(TrackEntry trackEntry, Spine.Event e)
  {
    if (e.Data.Name == "Audio/sozo eat" && this.Brain.Info.ID == 99996)
    {
      AudioManager.Instance.PlayOneShot("event:/dialogue/followers/npc/fol_sozo_eat", this.follower.gameObject);
    }
    else
    {
      if (!(e.Data.Name == "VO/Crazy"))
        return;
      AudioManager.Instance.PlayOneShot("event:/rituals/fight_pit_kill", this.follower.gameObject);
    }
  }

  public override void Cleanup(Follower follower)
  {
    AudioManager.Instance.StopLoop(this.loop);
    if (this._greetCoroutine != null)
    {
      follower.StopCoroutine(this._greetCoroutine);
      this._greetCoroutine = (Coroutine) null;
    }
    if ((bool) (UnityEngine.Object) follower.GetComponent<Interaction_BreakUpFight>())
      UnityEngine.Object.Destroy((UnityEngine.Object) follower.GetComponent<Interaction_BreakUpFight>());
    this.UndoStateAnimationChanges(follower);
  }

  public IEnumerator WaitForGreetCoroutine(Follower follower, Follower otherFollower)
  {
    FollowerTask_FightFollower taskFightFollower = this;
    while (taskFightFollower.State == FollowerTaskState.GoingTo && (double) Vector3.Distance(follower.transform.position, otherFollower.transform.position) > 3.0)
      yield return (object) null;
    taskFightFollower._greetCoroutine = (Coroutine) null;
  }

  public void MurderCallback()
  {
    if (this._isSpeaker)
    {
      this.follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
      double num = (double) this.follower.SetBodyAnimation(this.isEatingOtherFollower ? "sozo-eatshroom" : this.GetFightKillAnim(), false);
      this.follower.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.AnimationState_Event);
      this.follower.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.AnimationState_Event);
      this._doingTimer = this.ConvertAnimTimeToGameTime(this.follower.SimpleAnimator.Duration());
      if (this.isEatingOtherFollower)
        return;
      GameManager.GetInstance().WaitForSeconds(1.1f, (System.Action) (() =>
      {
        this._brain.AddPleasure(FollowerBrain.PleasureActions.Murderer);
        if (!this._brain.HasTrait(FollowerTrait.TraitType.MarriedMurderouslyJealous))
          return;
        this.AddJealousMurderThought(this._brain);
      }));
    }
    else
    {
      FollowerBrain b = this.follower.Brain;
      b._directInfoAccess.MurderedBy = this.OtherChatTask.Brain.Info.ID;
      b._directInfoAccess.MurderedTerm = this.GetMurderedReason(this.OtherChatTask.Brain, b);
      if (this.isEatingOtherFollower)
        return;
      bool isMurderousSpouseFight = b.Info.MarriedToLeader && this.OtherChatTask.Brain.HasTrait(FollowerTrait.TraitType.MarriedMurderouslyJealous);
      bool die = (double) UnityEngine.Random.value < (double) (isMurderousSpouseFight ? 0.5f : 0.25f);
      GameManager.GetInstance().WaitForSeconds(die ? 1.1f : 0.2f, (System.Action) (() =>
      {
        if (this._interrupted)
          return;
        this.follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
        double num1 = (double) this.follower.SetBodyAnimation(this.GetFightDieAnim(die), false);
        GameManager.GetInstance().WaitForSeconds(die ? 1f : 2f, (System.Action) (() =>
        {
          if (this._interrupted)
          {
            this.follower.State.CURRENT_STATE = StateMachine.State.Idle;
            double num2 = (double) this.follower.SetBodyAnimation("idle", true);
          }
          else
          {
            this.FinishedMurderCallback = true;
            if ((UnityEngine.Object) HUD_Manager.Instance != (UnityEngine.Object) null)
              HUD_Manager.Instance.ClearFightingTarget();
            if (die)
            {
              NotificationCentre.NotificationType deathNotificationType = NotificationCentre.NotificationType.MurderedByFollower;
              if ((UnityEngine.Object) this.follower != (UnityEngine.Object) null)
                this.follower.Die(deathNotificationType, false, (double) this.Brain.LastPosition.x > (double) this.OtherChatTask.Brain.LastPosition.x ? 1 : -1, this.GetDieAnim(), this.GetDeadAnim());
              else
                b.Die(deathNotificationType);
              string locKey = isMurderousSpouseFight ? "Notifications/FollowerMurder/Reason/MurderousSpouse" : "Notifications/Cult_FollowerDiedFromMurder/Notification/On";
              NotificationCentre.Instance.PlayFaithNotification(locKey, 0.0f, NotificationBase.Flair.None, b.Info.ID, b.Info.Name, this.OtherChatTask.Brain.Info.Name);
            }
            else
            {
              if ((UnityEngine.Object) this.follower != (UnityEngine.Object) null)
                this.follower.Brain.MakeInjured(true);
              else
                b.MakeInjured(true);
              b.CompleteCurrentTask();
              NotificationCentre.Instance.PlayFaithNotification("Notifications/InjuredFromFight", 0.0f, NotificationBase.Flair.None, b.Info.ID, b.Info.Name, FollowerInfo.GetInfoByID(this._otherFollowerID).Name);
              this.OtherChatTask.Brain.AddAdoration(FollowerBrain.AdorationActions.WonFight, (System.Action) null);
              if (!this.OtherChatTask.Brain.HasTrait(FollowerTrait.TraitType.MarriedJealous) && !this.OtherChatTask.Brain.HasTrait(FollowerTrait.TraitType.MarriedMurderouslyJealous))
                return;
              this.AddJealousFightThought(this.OtherChatTask.Brain);
            }
          }
        }));
      }));
      this._doingTimer = this.ConvertAnimTimeToGameTime(this.follower.SimpleAnimator.Duration() + 1.1f);
    }
  }

  public void AddJealousFightThought(FollowerBrain brain)
  {
    ThoughtData data = FollowerThoughts.GetData((double) UnityEngine.Random.value < 0.5 ? Thought.JealousSpouse_Fight_1 : Thought.JealousSpouse_Fight_2);
    data.Init();
    brain.AddThought(data);
  }

  public void AddJealousMurderThought(FollowerBrain brain)
  {
    ThoughtData data = FollowerThoughts.GetData((double) UnityEngine.Random.value < 0.5 ? Thought.MurderousSpouse_Killed_1 : Thought.MurderousSpouse_Killed_2);
    data.Init();
    brain.AddThought(data);
  }

  public float ConvertAnimTimeToGameTime(float duration) => duration * 2f;

  public void DoFinale(Follower follower, IDAndRelationship relationship)
  {
    AudioManager.Instance.StopLoop(this.loop);
    if (!this._isLeader)
      return;
    double num = (double) follower.SetBodyAnimation("Conversations/become-enemies", false);
    this._doingTimer = this.ConvertAnimTimeToGameTime(5.5f);
  }

  public string GetMurderedReason(FollowerBrain murderer, FollowerBrain target)
  {
    if (murderer.Info.GetOrCreateRelationship(target.Info.ID).CurrentRelationshipState == IDAndRelationship.RelationshipState.Enemies)
      return "Notifications/FollowerMurder/Reason/Hate";
    if (murderer.Info.MarriedToLeader && target.Info.MarriedToLeader)
      return "Notifications/FollowerMurder/Reason/Jealous";
    if (target.Info.CursedState == Thought.Dissenter)
      return "Notifications/FollowerMurder/Reason/Dissenter";
    if (target.ThoughtExists(Thought.AtePoopMeal) && murderer.Info.ID != 99990)
      return "Notifications/FollowerMurder/Reason/EatingPoop";
    if (Interaction_Pub.Pubs.Count > 0 && (double) Vector3.Distance(murderer.LastPosition, Interaction_Pub.Pubs[0].Position) < 3.0)
      return "Notifications/FollowerMurder/Reason/BarFight";
    float num = UnityEngine.Random.value;
    if ((double) num < 0.25)
      return "Notifications/FollowerMurder/Reason/Funny";
    if ((double) num < 0.5)
      return "Notifications/FollowerMurder/Reason/Bored";
    return (double) num < 0.75 ? "Notifications/FollowerMurder/Reason/Accident" : "Notifications/FollowerMurder/Reason/NoReason";
  }

  public void Interrupt() => this._interrupted = true;

  public override void OnEnd()
  {
    base.OnEnd();
    if ((UnityEngine.Object) HUD_Manager.Instance != (UnityEngine.Object) null)
      HUD_Manager.Instance.ClearFightingTarget();
    AudioManager.Instance.StopLoop(this.loop);
  }

  public string GetDeadAnim() => this.Brain.Info.IsDrunk ? "Drunk/drunk-dead" : "dead";

  public string GetDieAnim() => this.Brain.Info.IsDrunk ? "Drunk/drunk-fight-die" : "fight-die";

  public string GetFightAnim() => this.Brain.Info.IsDrunk ? "Drunk/drunk-fight" : "fight";

  public string GetFightKillAnim()
  {
    return this.Brain.Info.IsDrunk ? "Drunk/drunk-fight-kill" : "fight-kill";
  }

  public string GetFightDieAnim(bool willDie)
  {
    if (this.Brain.Info.IsDrunk)
      return "Drunk/drunk-fight-die";
    return !willDie ? "fight-lose" : "fight-die";
  }

  public string GetGreetAnim()
  {
    return this.Brain.Info.IsDrunk ? "Drunk/drunk-greet-hate" : "Conversations/greet-hate";
  }

  public string GetWalkpastAnim()
  {
    return this.Brain.HasTrait(FollowerTrait.TraitType.Zombie) ? ((double) UnityEngine.Random.value >= 0.5 ? "Zombie/zombie-walk" : "Zombie/zombie-walk-limp") : (this.Brain.Info.IsDrunk ? "Drunk/drunk-walkpast-hate" : "Conversations/walkpast-hate");
  }

  public override void SimDoingBegin(SimFollower simFollower)
  {
    switch (this._convoStage)
    {
      case FollowerTask_FightFollower.ConvoStage.None:
        throw new ArgumentException("Doing unstarted convo!!");
      case FollowerTask_FightFollower.ConvoStage.Chat1:
        this._doingTimer = this.ConvertAnimTimeToGameTime(1f) * 20f;
        break;
      case FollowerTask_FightFollower.ConvoStage.Murder:
        this.follower = FollowerManager.FindFollowerByID(simFollower.Brain.Info.ID);
        if (this.isEatingOtherFollower && this._isLeader && !this.coroutineHit && (this.Brain.Info.ID == 99996 || this.Brain.HasTrait(FollowerTrait.TraitType.Zombie)))
        {
          GameManager.GetInstance().StartCoroutine(this.EatSequence(new System.Action(this.MurderCallback)));
          break;
        }
        if (!this._isLeader)
          break;
        if ((UnityEngine.Object) HUD_Manager.Instance != (UnityEngine.Object) null)
          HUD_Manager.Instance.ClearFightingTarget();
        bool flag = this.Brain.HasTrait(FollowerTrait.TraitType.MarriedMurderouslyJealous) && this.OtherChatTask.Brain.Info.MarriedToLeader;
        int num = (double) UnityEngine.Random.value < (flag ? 0.5 : 0.25) ? 1 : 0;
        FollowerBrain target = this._brain;
        FollowerBrain followerBrain1 = this.OtherChatTask.Brain;
        if ((double) UnityEngine.Random.value > 0.5 | flag)
        {
          FollowerBrain followerBrain2 = target;
          target = followerBrain1;
          followerBrain1 = followerBrain2;
        }
        followerBrain1.AddPleasure(FollowerBrain.PleasureActions.Murderer);
        if (num != 0)
        {
          target._directInfoAccess.MurderedBy = followerBrain1.Info.ID;
          target._directInfoAccess.MurderedTerm = this.GetMurderedReason(followerBrain1, target);
          FollowerManager.FindFollowerByID(target.Info.ID).Die(NotificationCentre.NotificationType.MurderedByFollower);
          string locKey = flag ? "Notifications/FollowerMurder/Reason/MurderousSpouse" : "Notifications/Cult_FollowerDiedFromMurder/Notification/On";
          NotificationCentre.Instance.PlayFaithNotification(locKey, 0.0f, NotificationBase.Flair.None, target.Info.ID, target.Info.Name, followerBrain1.Info.Name);
          break;
        }
        target.MakeInjured(true);
        NotificationCentre.Instance.PlayFaithNotification("Notifications/InjuredFromFight", 0.0f, NotificationBase.Flair.None, target.Info.ID, target.Info.Name, followerBrain1.Info.Name);
        this.AddJealousFightThought(followerBrain1);
        break;
      case FollowerTask_FightFollower.ConvoStage.Finale:
        this.End();
        break;
    }
  }

  public IEnumerator EatSequence(System.Action callback)
  {
    FollowerTask_FightFollower taskFightFollower = this;
    Follower otherfollower = FollowerManager.FindFollowerByID(taskFightFollower.OtherChatTask.Brain.Info.ID);
    Follower f = FollowerManager.FindFollowerByID(taskFightFollower._brain.Info.ID);
    taskFightFollower.OtherChatTask.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    taskFightFollower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    taskFightFollower.coroutineHit = true;
    while (PlayerFarming.Location != FollowerLocation.Base || LetterBox.IsPlaying || MMConversation.isPlaying || SimulationManager.IsPaused || taskFightFollower.Brain.Location != FollowerLocation.Base || !PlayerFarming.LongToPerformPlayerStates.Contains(PlayerFarming.Instance.state.CURRENT_STATE))
      yield return (object) null;
    if ((UnityEngine.Object) f != (UnityEngine.Object) null)
    {
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(f.gameObject);
      SimulationManager.Pause();
      string text = string.Format(LocalizationManager.GetTranslation("Notifications/Cult_FollowerDiedFromBeingEaten/Notification/On"), (object) taskFightFollower.OtherChatTask.Brain.Info.Name, (object) taskFightFollower._brain.Info.Name);
      taskFightFollower._doingTimer = float.MaxValue;
      if (!string.IsNullOrEmpty(text))
        LetterBox.Instance.ShowSubtitle(text);
      if ((UnityEngine.Object) otherfollower != (UnityEngine.Object) null)
      {
        taskFightFollower.OtherChatTask.ClearDestination();
        taskFightFollower.ClearDestination();
        yield return (object) new WaitForEndOfFrame();
        otherfollower.transform.position = f.transform.position + Vector3.left / 1.5f;
        f.FacePosition(otherfollower.transform.position);
        otherfollower.FacePosition(f.transform.position);
        double num = (double) otherfollower.SetBodyAnimation("Reactions/react-scared-long", false);
        taskFightFollower.OtherChatTask._doingTimer = float.MaxValue;
        f.Spine.AnimationState.Event += (Spine.AnimationState.TrackEntryEventDelegate) ((trackEntry, e) =>
        {
          if (!(e.Data.Name == "EatShroom"))
            return;
          BiomeConstants.Instance.EmitSmokeExplosionVFX(otherfollower.transform.position - Vector3.forward / 2f);
          otherfollower.gameObject.SetActive(false);
          if (!this.Brain.Info.HasTrait(FollowerTrait.TraitType.Zombie))
            return;
          AudioManager.Instance.PlayOneShot("event:/dialogue/followers/npc/fol_eat_other_fol", f.gameObject);
        });
      }
      yield return (object) new WaitForSeconds(1f);
      taskFightFollower._doingTimer = float.MaxValue;
    }
    System.Action action = callback;
    if (action != null)
      action();
    yield return (object) new WaitForSeconds(3f);
    int id = taskFightFollower.OtherChatTask.Brain.Info.ID;
    NotificationCentre.Instance.PlayFaithNotification("Notifications/MushroomEatenBySozo", 0.0f, NotificationBase.Flair.None, id, taskFightFollower.OtherChatTask.Brain.Info.Name, taskFightFollower.Brain.Info.Name);
    NotificationCentre.NotificationType deathNotificationType = taskFightFollower.Brain.Info.ID != 99996 || !(taskFightFollower.OtherChatTask.Brain.Info.SkinName == "Mushroom") ? NotificationCentre.NotificationType.DiedFromBeingEaten : NotificationCentre.NotificationType.DiedFromBeingEatenBySozo;
    if ((UnityEngine.Object) otherfollower != (UnityEngine.Object) null)
      otherfollower.Die(deathNotificationType, false, (double) taskFightFollower.Brain.LastPosition.x > (double) taskFightFollower.OtherChatTask.Brain.LastPosition.x ? 1 : -1, force: true);
    else
      taskFightFollower.OtherChatTask.Brain.Die(deathNotificationType);
    SimulationManager.UnPause();
    GameManager.GetInstance().OnConversationEnd();
    taskFightFollower.Brain._directInfoAccess.Satiation = 100f;
    taskFightFollower.Brain.CompleteCurrentTask();
    if (taskFightFollower.Brain.Info.ID == 99996)
    {
      taskFightFollower.Brain.AddThought((Thought) UnityEngine.Random.Range(382, 387));
      taskFightFollower.Brain._directInfoAccess.SozoBrainshed = true;
      DataManager.Instance.SozoAteMushroomDay = TimeManager.CurrentDay;
    }
    if (taskFightFollower.Brain.Info.HasTrait(FollowerTrait.TraitType.Zombie))
      DataManager.Instance.TimeSinceLastFollowerEaten = TimeManager.TotalElapsedGameTime + (float) UnityEngine.Random.Range(2400, 6000);
    f.Spine.AnimationState.Event -= (Spine.AnimationState.TrackEntryEventDelegate) ((trackEntry, e) =>
    {
      if (!(e.Data.Name == "EatShroom"))
        return;
      BiomeConstants.Instance.EmitSmokeExplosionVFX(otherfollower.transform.position - Vector3.forward / 2f);
      otherfollower.gameObject.SetActive(false);
      if (!this.Brain.Info.HasTrait(FollowerTrait.TraitType.Zombie))
        return;
      AudioManager.Instance.PlayOneShot("event:/dialogue/followers/npc/fol_eat_other_fol", f.gameObject);
    });
  }

  public override float SocialChange(float deltaGameTime) => 0.0f;

  public override float SatiationChange(float deltaGameTime) => 0.0f;

  [CompilerGenerated]
  public void \u003CMurderCallback\u003Eb__60_0()
  {
    this._brain.AddPleasure(FollowerBrain.PleasureActions.Murderer);
    if (!this._brain.HasTrait(FollowerTrait.TraitType.MarriedMurderouslyJealous))
      return;
    this.AddJealousMurderThought(this._brain);
  }

  public enum ConvoStage
  {
    None,
    Chat1,
    Murder,
    Finale,
    Finished,
  }

  public enum ConvoOutcome
  {
    BecomeFriends,
    BecomeLovers,
    RemainLovers,
    GoodConvo,
    BecomeEnemies,
    Fight,
    BadConvo,
  }
}
