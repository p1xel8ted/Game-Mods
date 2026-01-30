// Decompiled with JetBrains decompiler
// Type: FollowerTask_Chat
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMTools;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerTask_Chat : FollowerTask
{
  public int _otherFollowerID;
  public FollowerBrain _otherFollowerBrain;
  public FollowerTask_Chat OtherChatTask;
  public bool _isLeader;
  public bool _isSpeaker;
  public FollowerTask_Chat.ConvoStage _convoStage;
  public FollowerTask_Chat.ConvoOutcome _convoOutcome;
  public float _doingTimer;
  public int _score;
  public bool _talkNice;
  public Coroutine _greetCoroutine;
  [CompilerGenerated]
  public Vector3 \u003CChatPosition\u003Ek__BackingField;
  public static Action<FollowerInfo, FollowerInfo, IDAndRelationship.RelationshipState> OnChangeRelationship;

  public override FollowerTaskType Type => FollowerTaskType.Chat;

  public override FollowerLocation Location => this._brain.Location;

  public override bool BlockTaskChanges => true;

  public override bool BlockReactTasks => true;

  public Vector3 ChatPosition
  {
    get => this.\u003CChatPosition\u003Ek__BackingField;
    set => this.\u003CChatPosition\u003Ek__BackingField = value;
  }

  public bool IsDiscipleChat
  {
    get
    {
      return this.Brain.Info.IsDisciple && FollowerInfo.GetInfoByID(this._otherFollowerID) != null && FollowerInfo.GetInfoByID(this._otherFollowerID).IsDisciple;
    }
  }

  public FollowerTask_Chat(int followerID, bool leader)
  {
    this._otherFollowerID = followerID;
    this._isLeader = leader;
  }

  public override int GetSubTaskCode() => this._otherFollowerID;

  public override void OnStart()
  {
    this._otherFollowerBrain = FollowerBrain.FindBrainByID(this._otherFollowerID);
    if (this._isLeader)
    {
      this.OtherChatTask = new FollowerTask_Chat(this._brain.Info.ID, false);
      this.OtherChatTask.OtherChatTask = this;
      if (this._otherFollowerBrain != null)
      {
        this._otherFollowerBrain.TransitionToTask((FollowerTask) this.OtherChatTask);
      }
      else
      {
        this.End();
        return;
      }
    }
    this._brain.Stats.Social = 100f;
    int locationState = (int) LocationManager.GetLocationState(this.Location);
    Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    this.ChatPosition = locationState != 3 || !((UnityEngine.Object) followerById != (UnityEngine.Object) null) ? TownCentre.RandomPositionInCachedTownCentre() : followerById.transform.position;
    if (this.OtherChatTask.State != FollowerTaskState.Done)
    {
      FollowerTask_Chat otherChatTask = this.OtherChatTask;
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
    if (this._isLeader)
    {
      this.CommenceConvo();
      this.OtherChatTask.CommenceConvo();
      this.SetState(FollowerTaskState.Idle);
    }
    else
      this.SetState(FollowerTaskState.Idle);
  }

  public override void OnComplete()
  {
    FollowerTask_Chat otherChatTask = this.OtherChatTask;
    otherChatTask.OnFollowerTaskStateChanged = otherChatTask.OnFollowerTaskStateChanged - new FollowerTask.FollowerTaskDelegate(this.OnTaskStateChanged);
  }

  public void CommenceConvo()
  {
    this._convoStage = FollowerTask_Chat.ConvoStage.Chat1;
    if (!this._isLeader || !FollowerBrainStats.ShouldWork || !this._brain.CanWork)
      return;
    Follower followerById1 = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    Follower followerById2 = FollowerManager.FindFollowerByID(this._otherFollowerID);
    if ((bool) (UnityEngine.Object) followerById1 && (bool) (UnityEngine.Object) followerById2 && (UnityEngine.Object) followerById1.gameObject.GetComponent<Interaction_BackToWork>() == (UnityEngine.Object) null)
    {
      Interaction_BackToWork interactionBackToWork = followerById1.gameObject.AddComponent<Interaction_BackToWork>();
      interactionBackToWork.Init(followerById1);
      interactionBackToWork.LockPosition = followerById1.transform;
    }
    if (!(bool) (UnityEngine.Object) followerById1 || !(bool) (UnityEngine.Object) followerById2 || !((UnityEngine.Object) followerById2.gameObject.GetComponent<Interaction_BackToWork>() == (UnityEngine.Object) null))
      return;
    Interaction_BackToWork interactionBackToWork1 = followerById2.gameObject.AddComponent<Interaction_BackToWork>();
    interactionBackToWork1.Init(followerById2);
    interactionBackToWork1.LockPosition = followerById2.transform;
  }

  public void NextConvoStage()
  {
    ++this._convoStage;
    this.SetState(FollowerTaskState.Idle);
  }

  public bool GetTalkNice()
  {
    int num = 70;
    if (this._brain.Stats.GuaranteedGoodInteractions)
      num = 100;
    if (this._brain.HasTrait(FollowerTrait.TraitType.OverworkedParent))
      num = 30;
    if ((this._brain.HasTrait(FollowerTrait.TraitType.MarriedJealous) || this._brain.HasTrait(FollowerTrait.TraitType.MarriedMurderouslyJealous)) && this.OtherChatTask.Brain.Info.MarriedToLeader)
      num = 0;
    return UnityEngine.Random.Range(0, 100) < num;
  }

  public override void TaskTick(float deltaGameTime)
  {
    if (this._otherFollowerBrain != null && this._otherFollowerBrain.CurrentTaskType != FollowerTaskType.Chat)
      this.End();
    else if (this.State == FollowerTaskState.Idle)
    {
      if (this._convoStage == FollowerTask_Chat.ConvoStage.Finished)
      {
        this.End();
      }
      else
      {
        if (this._convoStage == FollowerTask_Chat.ConvoStage.None)
          return;
        switch (this._convoStage)
        {
          case FollowerTask_Chat.ConvoStage.Chat1:
          case FollowerTask_Chat.ConvoStage.Chat3:
            this._isSpeaker = this._isLeader;
            if (this._isSpeaker)
            {
              bool talkNice = this.GetTalkNice();
              this.Talk(talkNice);
              this.OtherChatTask.Talk(talkNice);
              break;
            }
            break;
          case FollowerTask_Chat.ConvoStage.Chat2:
            this._isSpeaker = !this._isLeader;
            if (this._isSpeaker)
            {
              bool talkNice = this.GetTalkNice();
              this.Talk(talkNice);
              this.OtherChatTask.Talk(talkNice);
              break;
            }
            break;
          case FollowerTask_Chat.ConvoStage.Finale:
            this._isSpeaker = this._isLeader;
            if (this._score >= 2 || this.IsDiscipleChat)
            {
              this.RelationshipUp();
              break;
            }
            this.RelationshipDown();
            break;
        }
        this.SetState(FollowerTaskState.Doing);
      }
    }
    else
    {
      if (this.State != FollowerTaskState.Doing)
        return;
      if (this._isSpeaker && (double) (this._doingTimer -= deltaGameTime) <= 0.0)
      {
        this.NextConvoStage();
        this.OtherChatTask.NextConvoStage();
      }
      else
      {
        if (this._isSpeaker || this.OtherChatTask != null && this.OtherChatTask._isSpeaker)
          return;
        this.End();
      }
    }
  }

  public void Talk(bool talkNice)
  {
    this._talkNice = talkNice;
    this._score += this._talkNice ? 1 : 0;
  }

  public void RelationshipUp()
  {
    IDAndRelationship relationship = this._brain.Info.GetOrCreateRelationship(this._otherFollowerID);
    ++relationship.Relationship;
    bool flag1 = FollowerManager.AreSiblings(this._brain.Info.ID, this._otherFollowerID);
    bool flag2 = FollowerManager.IsChildOf(this._brain.Info.ID, this._otherFollowerID) || FollowerManager.IsChildOf(this._otherFollowerID, this._brain.Info.ID);
    if (this._brain.Info.ID == 666 && (this._otherFollowerID == 99994 || this._otherFollowerID == 99995) || this._otherFollowerID == 666 && (this._brain.Info.ID == 99994 || this._brain.Info.ID == 99995))
      flag1 = true;
    if (relationship.CurrentRelationshipState < IDAndRelationship.RelationshipState.Friends && (double) relationship.Relationship >= (double) this.FriendThreshold)
    {
      relationship.CurrentRelationshipState = IDAndRelationship.RelationshipState.Friends;
      this._convoOutcome = FollowerTask_Chat.ConvoOutcome.BecomeFriends;
      this._brain.AddThought(Thought.NewFriend);
    }
    else if (relationship.CurrentRelationshipState < IDAndRelationship.RelationshipState.Lovers && (double) relationship.Relationship >= (double) this.LoveThreshold && !flag1 && !flag2)
    {
      relationship.CurrentRelationshipState = IDAndRelationship.RelationshipState.Lovers;
      this._convoOutcome = FollowerTask_Chat.ConvoOutcome.BecomeLovers;
      this._brain.AddThought(Thought.NewLover);
    }
    else if (relationship.CurrentRelationshipState == IDAndRelationship.RelationshipState.Lovers)
    {
      this._convoOutcome = FollowerTask_Chat.ConvoOutcome.RemainLovers;
      this._brain.AddThought(Thought.GoodChatWithLover);
    }
    else
    {
      this._convoOutcome = FollowerTask_Chat.ConvoOutcome.GoodConvo;
      this._brain.AddThought(Thought.GoodChat);
    }
  }

  public void RelationshipDown()
  {
    IDAndRelationship relationship = this._brain.Info.GetOrCreateRelationship(this._otherFollowerID);
    --relationship.Relationship;
    if (relationship.CurrentRelationshipState > IDAndRelationship.RelationshipState.Enemies && (double) relationship.Relationship <= (double) this.HateThreshold)
    {
      relationship.CurrentRelationshipState = IDAndRelationship.RelationshipState.Enemies;
      this._convoOutcome = FollowerTask_Chat.ConvoOutcome.BecomeEnemies;
      this._brain.AddThought(Thought.NewEnemy);
    }
    else if (relationship.CurrentRelationshipState == IDAndRelationship.RelationshipState.Enemies && UnityEngine.Random.Range(0, 100) < 33)
    {
      this._convoOutcome = FollowerTask_Chat.ConvoOutcome.Fight;
    }
    else
    {
      this._convoOutcome = FollowerTask_Chat.ConvoOutcome.BadConvo;
      this._brain.AddThought(Thought.BadChat);
    }
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
      if (brainById != null && brainById.CurrentTaskType == FollowerTaskType.Chat)
      {
        float num = 1.5f;
        if (this.IsDiscipleChat)
          num = 1f;
        FollowerTask_Chat currentTask = (FollowerTask_Chat) brainById.CurrentTask;
        vector3 = currentTask.ChatPosition + Vector3.right * ((double) currentTask.ChatPosition.x < (double) follower.transform.position.x ? num : -num);
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
    if (!(bool) (UnityEngine.Object) followerById || !(bool) (UnityEngine.Object) follower || !((UnityEngine.Object) follower.State != (UnityEngine.Object) null))
      return;
    follower.State.facingAngle = Utils.GetAngle(follower.transform.position, followerById.transform.position);
  }

  public override void OnGoingToBegin(Follower follower)
  {
    base.OnGoingToBegin(follower);
    if (!this._isLeader)
      return;
    Follower followerById = FollowerManager.FindFollowerByID(this._otherFollowerID);
    this._greetCoroutine = follower.StartCoroutine((IEnumerator) this.WaitForGreetCoroutine(follower, followerById));
    IDAndRelationship relationship1 = this._brain.Info.GetOrCreateRelationship(this._otherFollowerID);
    IDAndRelationship relationship2 = followerById.Brain.Info.GetOrCreateRelationship(this._brain.Info.ID);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, this.GetRelationshipAnimation(relationship1.Relationship));
    followerById.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    double num = (double) followerById.SetBodyAnimation(this.GetRelationshipAnimationGreet(relationship2.Relationship), true);
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
      followerById.FacePosition(follower.transform.position);
      follower.FacePosition(followerById.transform.position);
      IDAndRelationship relationship = this._brain.Info.GetOrCreateRelationship(this._otherFollowerID);
      switch (this._convoStage)
      {
        case FollowerTask_Chat.ConvoStage.None:
          throw new ArgumentException("Doing unstarted convo!!");
        case FollowerTask_Chat.ConvoStage.Chat1:
        case FollowerTask_Chat.ConvoStage.Chat2:
        case FollowerTask_Chat.ConvoStage.Chat3:
          if (this._isSpeaker)
          {
            follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
            double num = (double) follower.SetBodyAnimation(this._talkNice ? this.GetRelationshipAnimationGoodConversationTalk(relationship.Relationship) : this.GetRelationshipAnimationBadConversationTalk(relationship.Relationship), true);
            this._doingTimer = this.ConvertAnimTimeToGameTime(follower.SimpleAnimator.Duration());
            break;
          }
          follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
          double num1 = (double) follower.SetBodyAnimation(this.GetRelationshipAnimationIdle(relationship.Relationship, false), true);
          break;
        case FollowerTask_Chat.ConvoStage.Finale:
          this.DoFinale(follower, relationship);
          break;
      }
    }
  }

  public override void Cleanup(Follower follower)
  {
    if (this._greetCoroutine != null)
    {
      follower.StopCoroutine(this._greetCoroutine);
      this._greetCoroutine = (Coroutine) null;
    }
    if ((bool) (UnityEngine.Object) follower.GetComponent<Interaction_BackToWork>())
      UnityEngine.Object.Destroy((UnityEngine.Object) follower.GetComponent<Interaction_BackToWork>());
    this.UndoStateAnimationChanges(follower);
    if (this._brain.Info.GetOrCreateRelationship(this._otherFollowerID).CurrentRelationshipState != IDAndRelationship.RelationshipState.Enemies || (double) UnityEngine.Random.value >= 0.10000000149011612 || PlayerFarming.Location != FollowerLocation.Base || LetterBox.IsPlaying || MMConversation.isPlaying || SimulationManager.IsPaused)
      return;
    GameManager.GetInstance().WaitForSeconds(1f, (System.Action) (() => this.Brain.HardSwapToTask((FollowerTask) new FollowerTask_FightFollower(this._otherFollowerID, true))));
  }

  public IEnumerator WaitForGreetCoroutine(Follower follower, Follower otherFollower)
  {
    FollowerTask_Chat followerTaskChat = this;
    while (followerTaskChat.State == FollowerTaskState.GoingTo && (double) Vector3.Distance(follower.transform.position, otherFollower.transform.position) > 3.0)
      yield return (object) null;
    followerTaskChat._greetCoroutine = (Coroutine) null;
  }

  public float ConvertAnimTimeToGameTime(float duration) => duration * 2f;

  public void DoFinale(Follower follower, IDAndRelationship relationship)
  {
    switch (this._convoOutcome)
    {
      case FollowerTask_Chat.ConvoOutcome.BecomeFriends:
        double num1 = (double) follower.SetBodyAnimation(follower.Brain.Info.IsDrunk ? "Drunk/drunk-become-friends" : "Conversations/become-friends", false);
        follower.WorshipperBubble.Play(WorshipperBubble.SPEECH_TYPE.FRIENDS);
        this._doingTimer = this.ConvertAnimTimeToGameTime(5.5f);
        if (this._isLeader)
        {
          Action<FollowerInfo, FollowerInfo, IDAndRelationship.RelationshipState> changeRelationship = FollowerTask_Chat.OnChangeRelationship;
          if (changeRelationship != null)
          {
            changeRelationship(FollowerInfo.GetInfoByID(this._brain.Info.ID), FollowerInfo.GetInfoByID(this._otherFollowerID), IDAndRelationship.RelationshipState.Friends);
            break;
          }
          break;
        }
        break;
      case FollowerTask_Chat.ConvoOutcome.BecomeLovers:
        double num2 = (double) follower.SetBodyAnimation(follower.Brain.Info.IsDrunk ? "Drunk/drunk-become-lovers" : "Conversations/become-lovers", false);
        follower.WorshipperBubble.Play(WorshipperBubble.SPEECH_TYPE.LOVE);
        this._doingTimer = this.ConvertAnimTimeToGameTime(5.5f);
        if (this._isLeader)
        {
          Action<FollowerInfo, FollowerInfo, IDAndRelationship.RelationshipState> changeRelationship = FollowerTask_Chat.OnChangeRelationship;
          if (changeRelationship != null)
          {
            changeRelationship(FollowerInfo.GetInfoByID(this._brain.Info.ID), FollowerInfo.GetInfoByID(this._otherFollowerID), IDAndRelationship.RelationshipState.Lovers);
            break;
          }
          break;
        }
        break;
      case FollowerTask_Chat.ConvoOutcome.RemainLovers:
        double num3 = (double) follower.SetBodyAnimation("loving", true);
        this._doingTimer = this.ConvertAnimTimeToGameTime(5.33333349f);
        break;
      case FollowerTask_Chat.ConvoOutcome.GoodConvo:
        double num4 = (double) follower.SetBodyAnimation(this.GetRelationshipAnimationGoodConversationReact(relationship.Relationship), false);
        this._doingTimer = this.ConvertAnimTimeToGameTime(2f);
        break;
      case FollowerTask_Chat.ConvoOutcome.BecomeEnemies:
        double num5 = (double) follower.SetBodyAnimation(follower.Brain.Info.IsDrunk ? "Drunk/drunk-become-enemies" : "Conversations/become-enemies", false);
        follower.WorshipperBubble.Play(WorshipperBubble.SPEECH_TYPE.ENEMIES);
        this._doingTimer = this.ConvertAnimTimeToGameTime(5.5f);
        if (this._isLeader)
        {
          Action<FollowerInfo, FollowerInfo, IDAndRelationship.RelationshipState> changeRelationship = FollowerTask_Chat.OnChangeRelationship;
          if (changeRelationship != null)
          {
            changeRelationship(FollowerInfo.GetInfoByID(this._brain.Info.ID), FollowerInfo.GetInfoByID(this._otherFollowerID), IDAndRelationship.RelationshipState.Enemies);
            break;
          }
          break;
        }
        break;
      case FollowerTask_Chat.ConvoOutcome.Fight:
        double num6 = (double) follower.SetBodyAnimation("fight", true);
        this._doingTimer = this.ConvertAnimTimeToGameTime(5f);
        break;
      case FollowerTask_Chat.ConvoOutcome.BadConvo:
        double num7 = (double) follower.SetBodyAnimation(this.GetRelationshipAnimationBadConversationReact(relationship.Relationship), false);
        this._doingTimer = this.ConvertAnimTimeToGameTime(2f);
        break;
    }
    switch (this._convoOutcome)
    {
      case FollowerTask_Chat.ConvoOutcome.BecomeFriends:
      case FollowerTask_Chat.ConvoOutcome.BecomeLovers:
      case FollowerTask_Chat.ConvoOutcome.RemainLovers:
      case FollowerTask_Chat.ConvoOutcome.GoodConvo:
        follower.AddBodyAnimation("idle", true, 0.0f);
        break;
      case FollowerTask_Chat.ConvoOutcome.BecomeEnemies:
      case FollowerTask_Chat.ConvoOutcome.Fight:
      case FollowerTask_Chat.ConvoOutcome.BadConvo:
        follower.AddBodyAnimation(this.GetRelationshipAnimationIdle(relationship.Relationship, false), true, 0.0f);
        break;
    }
  }

  public string GetRelationshipAnimation(int relationship)
  {
    string relationshipAnimation = "Conversations/walkpast-";
    if (this.Brain.Info.IsDrunk)
      relationshipAnimation = "Drunk/drunk-walkpast-";
    if (this.IsDiscipleChat)
      relationshipAnimation += "nice";
    else if ((double) relationship < (double) this.HateThreshold)
      relationshipAnimation += "hate";
    else if (Utils.WithinRange((float) relationship, this.HateThreshold, -1f))
      relationshipAnimation += "mean";
    else if (Utils.WithinRange((float) relationship, 0.0f, this.LoveThreshold))
      relationshipAnimation += "nice";
    else if ((double) relationship > (double) this.LoveThreshold)
      relationshipAnimation += "love";
    return relationshipAnimation;
  }

  public string GetRelationshipAnimationIdle(int relationship, bool walkpast)
  {
    string str = "Conversations/idle-";
    if (!this.Brain.Info.IsDrunk)
      return !this.IsDiscipleChat ? ((double) relationship >= (double) this.HateThreshold ? (!Utils.WithinRange((float) relationship, this.HateThreshold, -1f) ? (!Utils.WithinRange((float) relationship, 0.0f, this.LoveThreshold) ? ((double) relationship <= (double) this.LoveThreshold ? str + "nice" : str + "love") : str + "nice") : str + "mean") : str + "hate") : str + (walkpast ? "nice" : "disciple");
    if ((double) relationship < (double) this.HateThreshold || Utils.WithinRange((float) relationship, this.HateThreshold, -1f))
      return "Drinking/idle-drunk-angry";
    return this.Brain != null && (this.Brain.HasTrait(FollowerTrait.TraitType.Scared) || this.Brain.HasTrait(FollowerTrait.TraitType.CriminalScarred)) ? "Drinking/idle-drunk-sad" : "Drinking/idle-drunk-happy";
  }

  public string GetRelationshipAnimationGreet(int relationship)
  {
    string str = "Conversations/greet-";
    if (this.Brain.Info.IsDrunk)
      str = "Drunk/drunk-greet-";
    return (double) relationship >= (double) this.HateThreshold ? (!Utils.WithinRange((float) relationship, this.HateThreshold, -1f) ? (!Utils.WithinRange((float) relationship, 0.0f, this.LoveThreshold) ? ((double) relationship <= (double) this.LoveThreshold ? str + "nice" : str + "love") : str + "nice") : str + "mean") : str + "hate";
  }

  public string GetRelationshipAnimationGoodConversationTalk(int Relationship)
  {
    string str = "Conversations/talk-";
    if (this.IsDiscipleChat)
      return $"{str}disciple{UnityEngine.Random.Range(1, 4).ToString()}";
    if (this.Brain.Info.IsDrunk)
      str = "Drunk/drunk-talk-";
    return ((double) Relationship <= (double) this.LoveThreshold ? str + "nice" : str + "love") + UnityEngine.Random.Range(1, 4).ToString();
  }

  public string GetRelationshipAnimationBadConversationTalk(int Relationship)
  {
    string str = "Conversations/talk-";
    if (this.IsDiscipleChat)
      return $"{str}disciple{UnityEngine.Random.Range(1, 4).ToString()}";
    if (this.Brain.Info.IsDrunk)
      str = "Drunk/drunk-talk-";
    return ((double) Relationship <= (double) this.LoveThreshold ? str + "mean" : str + "hate") + UnityEngine.Random.Range(1, 4).ToString();
  }

  public string GetRelationshipAnimationGoodConversationReact(int Relationship)
  {
    string str = "Conversations/react-";
    if (this.IsDiscipleChat)
      return $"{str}disciple{UnityEngine.Random.Range(1, 4).ToString()}";
    if (this.Brain.Info.IsDrunk)
      str = "Drunk/drunk-react-";
    return ((double) Relationship <= (double) this.LoveThreshold ? str + "nice" : str + "love") + UnityEngine.Random.Range(1, 4).ToString();
  }

  public string GetRelationshipAnimationBadConversationReact(int Relationship)
  {
    string str = "Conversations/react-";
    if (this.IsDiscipleChat)
      return $"{str}disciple{UnityEngine.Random.Range(1, 4).ToString()}";
    if (this.Brain.Info.IsDrunk)
      str = "Drunk/drunk-react-";
    return ((double) Relationship <= (double) this.LoveThreshold ? str + "mean" : str + "hate") + UnityEngine.Random.Range(1, 4).ToString();
  }

  public float HateThreshold => -10f;

  public float FriendThreshold => 5f;

  public float LoveThreshold => 10f;

  public override void SimDoingBegin(SimFollower simFollower) => this._doingTimer = 5f;

  public override float SocialChange(float deltaGameTime) => 0.0f;

  public override float SatiationChange(float deltaGameTime) => 0.0f;

  [CompilerGenerated]
  public void \u003CCleanup\u003Eb__45_0()
  {
    this.Brain.HardSwapToTask((FollowerTask) new FollowerTask_FightFollower(this._otherFollowerID, true));
  }

  public enum ConvoStage
  {
    None,
    Chat1,
    Chat2,
    Chat3,
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
