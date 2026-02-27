// Decompiled with JetBrains decompiler
// Type: FollowerTask_Chat
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class FollowerTask_Chat : FollowerTask
{
  private int _otherFollowerID;
  public FollowerTask_Chat OtherChatTask;
  private bool _isLeader;
  private bool _isSpeaker;
  private FollowerTask_Chat.ConvoStage _convoStage;
  private FollowerTask_Chat.ConvoOutcome _convoOutcome;
  private float _doingTimer;
  private int _score;
  private bool _talkNice;
  private Coroutine _greetCoroutine;
  public static Action<FollowerInfo, FollowerInfo, IDAndRelationship.RelationshipState> OnChangeRelationship;

  public override FollowerTaskType Type => FollowerTaskType.Chat;

  public override FollowerLocation Location => this._brain.Location;

  public override bool BlockTaskChanges => true;

  public override bool BlockReactTasks => true;

  public Vector3 ChatPosition { get; private set; }

  public FollowerTask_Chat(int followerID, bool leader)
  {
    this._otherFollowerID = followerID;
    this._isLeader = leader;
  }

  protected override int GetSubTaskCode() => this._otherFollowerID;

  protected override void OnStart()
  {
    if (this._isLeader)
    {
      this.OtherChatTask = new FollowerTask_Chat(this._brain.Info.ID, false);
      this.OtherChatTask.OtherChatTask = this;
      FollowerBrain brainById = FollowerBrain.FindBrainByID(this._otherFollowerID);
      if (brainById != null)
      {
        brainById.TransitionToTask((FollowerTask) this.OtherChatTask);
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

  protected override void OnArrive()
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

  protected override void OnComplete()
  {
    FollowerTask_Chat otherChatTask = this.OtherChatTask;
    otherChatTask.OnFollowerTaskStateChanged = otherChatTask.OnFollowerTaskStateChanged - new FollowerTask.FollowerTaskDelegate(this.OnTaskStateChanged);
  }

  public void CommenceConvo()
  {
    this._convoStage = FollowerTask_Chat.ConvoStage.Chat1;
    if (!this._isLeader)
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

  private void NextConvoStage()
  {
    ++this._convoStage;
    this.SetState(FollowerTaskState.Idle);
  }

  private bool GetTalkNice()
  {
    int num = 70;
    if (this._brain.Stats.GuaranteedGoodInteractions)
      num = 100;
    return UnityEngine.Random.Range(0, 100) < num;
  }

  protected override void TaskTick(float deltaGameTime)
  {
    if (this.State == FollowerTaskState.Idle)
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
            if (this._score >= 2)
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
      if (this.State != FollowerTaskState.Doing || !this._isSpeaker || (double) (this._doingTimer -= deltaGameTime) > 0.0)
        return;
      this.NextConvoStage();
      this.OtherChatTask.NextConvoStage();
    }
  }

  public void Talk(bool talkNice)
  {
    this._talkNice = talkNice;
    this._score += this._talkNice ? 1 : 0;
  }

  private void RelationshipUp()
  {
    IDAndRelationship relationship = this._brain.Info.GetOrCreateRelationship(this._otherFollowerID);
    ++relationship.Relationship;
    if (relationship.CurrentRelationshipState < IDAndRelationship.RelationshipState.Friends && (double) relationship.Relationship >= (double) this.FriendThreshold)
    {
      relationship.CurrentRelationshipState = IDAndRelationship.RelationshipState.Friends;
      this._convoOutcome = FollowerTask_Chat.ConvoOutcome.BecomeFriends;
      this._brain.AddThought(Thought.NewFriend);
    }
    else if (relationship.CurrentRelationshipState < IDAndRelationship.RelationshipState.Lovers && (double) relationship.Relationship >= (double) this.LoveThreshold)
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

  private void RelationshipDown()
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

  private void OnTaskStateChanged(FollowerTaskState oldState, FollowerTaskState newState)
  {
    if (newState != FollowerTaskState.Done)
      return;
    this.End();
  }

  private void UndoStateAnimationChanges(Follower follower)
  {
    SimpleSpineAnimator.SpineChartacterAnimationData animationData = follower.SimpleAnimator.GetAnimationData(StateMachine.State.Moving);
    animationData.Animation = animationData.DefaultAnimation;
    follower.ResetStateAnimations();
  }

  protected override Vector3 UpdateDestination(Follower follower)
  {
    Vector3 vector3;
    if (this._isLeader)
    {
      FollowerBrain brainById = FollowerBrain.FindBrainByID(this._otherFollowerID);
      if (brainById != null && brainById.CurrentTaskType == FollowerTaskType.Chat)
      {
        FollowerTask_Chat currentTask = (FollowerTask_Chat) brainById.CurrentTask;
        vector3 = currentTask.ChatPosition + Vector3.right * ((double) currentTask.ChatPosition.x < (double) follower.transform.position.x ? 1.5f : -1.5f);
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
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Conversations/walkpast-" + this.GetRelationshipAnimation(relationship1.Relationship));
    followerById.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    double num = (double) followerById.SetBodyAnimation("Conversations/greet-" + this.GetRelationshipAnimation(relationship2.Relationship), true);
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
            double num = (double) follower.SetBodyAnimation(this._talkNice ? $"Conversations/talk-{this.GetRelationshipAnimationGoodConversation(relationship.Relationship)}{(object) UnityEngine.Random.Range(1, 4)}" : $"Conversations/talk-{this.GetRelationshipAnimationBadConversation(relationship.Relationship)}{(object) UnityEngine.Random.Range(1, 4)}", true);
            this._doingTimer = this.ConvertAnimTimeToGameTime(follower.SimpleAnimator.Duration());
            break;
          }
          follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
          double num1 = (double) follower.SetBodyAnimation("Conversations/idle-" + this.GetRelationshipAnimation(relationship.Relationship), true);
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
  }

  private IEnumerator WaitForGreetCoroutine(Follower follower, Follower otherFollower)
  {
    FollowerTask_Chat followerTaskChat = this;
    while (followerTaskChat.State == FollowerTaskState.GoingTo && (double) Vector3.Distance(follower.transform.position, otherFollower.transform.position) > 3.0)
      yield return (object) null;
    followerTaskChat._greetCoroutine = (Coroutine) null;
  }

  private float ConvertAnimTimeToGameTime(float duration) => duration * 2f;

  private void DoFinale(Follower follower, IDAndRelationship relationship)
  {
    switch (this._convoOutcome)
    {
      case FollowerTask_Chat.ConvoOutcome.BecomeFriends:
        double num1 = (double) follower.SetBodyAnimation("Conversations/become-friends", false);
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
        double num2 = (double) follower.SetBodyAnimation("Conversations/become-lovers", false);
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
        double num4 = (double) follower.SetBodyAnimation($"Conversations/react-{this.GetRelationshipAnimationGoodConversation(relationship.Relationship)}{(object) UnityEngine.Random.Range(1, 4)}", false);
        this._doingTimer = this.ConvertAnimTimeToGameTime(2f);
        break;
      case FollowerTask_Chat.ConvoOutcome.BecomeEnemies:
        double num5 = (double) follower.SetBodyAnimation("Conversations/become-enemies", false);
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
        double num7 = (double) follower.SetBodyAnimation($"Conversations/react-{this.GetRelationshipAnimationBadConversation(relationship.Relationship)}{(object) UnityEngine.Random.Range(1, 4)}", false);
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
        follower.AddBodyAnimation("Conversations/idle-" + this.GetRelationshipAnimation(relationship.Relationship), true, 0.0f);
        break;
    }
  }

  private string GetRelationshipAnimation(int relationship)
  {
    if ((double) relationship < (double) this.HateThreshold)
      return "hate";
    if (Utils.WithinRange((float) relationship, this.HateThreshold, -1f))
      return "mean";
    return Utils.WithinRange((float) relationship, 0.0f, this.LoveThreshold) || (double) relationship <= (double) this.LoveThreshold ? "nice" : "love";
  }

  private string GetRelationshipAnimationGoodConversation(int Relationship)
  {
    return (double) Relationship > (double) this.LoveThreshold ? "love" : "nice";
  }

  private string GetRelationshipAnimationBadConversation(int Relationship)
  {
    return (double) Relationship < (double) this.HateThreshold ? "hate" : "mean";
  }

  private float HateThreshold => -10f;

  private float FriendThreshold => 5f;

  private float LoveThreshold => 10f;

  public override void SimDoingBegin(SimFollower simFollower) => this._doingTimer = 5f;

  protected override float SocialChange(float deltaGameTime) => 0.0f;

  protected override float SatiationChange(float deltaGameTime) => 0.0f;

  private enum ConvoStage
  {
    None,
    Chat1,
    Chat2,
    Chat3,
    Finale,
    Finished,
  }

  private enum ConvoOutcome
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
