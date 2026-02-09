// Decompiled with JetBrains decompiler
// Type: Task_IdleWorhipper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Task_IdleWorhipper : Task
{
  public GameObject MoveToObject;
  public static List<Task_IdleWorhipper> Worshippers = new List<Task_IdleWorhipper>();
  public Worshipper w;
  public float ConversationCooledDown = 3f;
  public float VomitCooldown;
  public Vomit ClosestVomit;
  public float DeadWorshipperCooldown;
  public DeadWorshipper ClosestDeadWorshipper;
  public Task_IdleWorhipper cTarget;
  public IDAndRelationship cRelationship;
  public IDAndRelationship Relationship;
  public Vector3 cTargetPosition;
  public GameObject MoveToGameObject;
  public int Score;
  public GraphNode Node;
  public GraphNode cNode;

  public override void StartTask(TaskDoer t, GameObject TargetObject)
  {
    base.StartTask(t, TargetObject);
    Task_IdleWorhipper.Worshippers.Add(this);
    this.w = t.GetComponent<Worshipper>();
    this.Type = Task_Type.NONE;
    t.InConversation = false;
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.MoveToObject = new GameObject();
  }

  public override void ClearTask()
  {
    UnityEngine.Object.Destroy((UnityEngine.Object) this.MoveToObject);
    this.t.StopAllCoroutines();
    this.Timer = 0.0f;
    if (this.cTarget != null)
    {
      if ((UnityEngine.Object) this.cTarget.t != (UnityEngine.Object) null)
      {
        this.cTarget.t.StopAllCoroutines();
        this.cTarget.t.InConversation = false;
      }
      this.cTarget.Timer = 0.0f;
      this.cTarget.state.CURRENT_STATE = StateMachine.State.Idle;
    }
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    if ((UnityEngine.Object) this.MoveToGameObject != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.MoveToGameObject);
    this.MoveToGameObject = (GameObject) null;
    this.t.ClearPaths();
    this.t.InConversation = false;
    base.ClearTask();
    Task_IdleWorhipper.Worshippers.Remove(this);
  }

  public override void TaskUpdate()
  {
    if (this.t.InConversation)
      return;
    this.ConversationCooledDown -= Time.deltaTime;
    if (this.state.CURRENT_STATE != StateMachine.State.Idle)
      return;
    if ((double) (this.Timer -= Time.deltaTime) < 0.0)
    {
      this.Timer = UnityEngine.Random.Range(5f, 7f);
      this.t.givePath(TownCentre.Instance.RandomPositionInTownCentre());
    }
    else
    {
      if ((double) this.ConversationCooledDown > 0.0)
        return;
      if ((double) (this.VomitCooldown -= Time.deltaTime) < 0.0 && this.CheckVomit())
      {
        this.VomitCooldown = 10f;
        float f = Utils.GetAngle(this.ClosestVomit.transform.position, this.t.transform.position) * ((float) Math.PI / 180f);
        float num = UnityEngine.Random.Range(0.5f, 2f);
        this.MoveToObject.transform.position = this.ClosestVomit.transform.position + new Vector3(num * Mathf.Cos(f), num * Mathf.Sin(f));
        this.w.GoToAndStop(this.MoveToObject, new System.Action(this.ReactSicken), this.ClosestVomit.gameObject, false);
      }
      else if ((double) (this.DeadWorshipperCooldown -= Time.deltaTime) < 0.0 && this.CheckDeadWorshipper())
      {
        this.DeadWorshipperCooldown = 10f;
        float f = Utils.GetAngle(this.ClosestDeadWorshipper.transform.position, this.t.transform.position) * ((float) Math.PI / 180f);
        float num = UnityEngine.Random.Range(0.5f, 2f);
        this.MoveToObject.transform.position = this.ClosestDeadWorshipper.transform.position + new Vector3(num * Mathf.Cos(f), num * Mathf.Sin(f));
        if (this.ClosestDeadWorshipper.StructureInfo.Age >= 5)
          this.w.GoToAndStop(this.MoveToObject, new System.Action(this.ReactSicken), this.ClosestDeadWorshipper.gameObject, false);
        else
          this.w.GoToAndStop(this.MoveToObject, new System.Action(this.ReactGreive), this.ClosestDeadWorshipper.gameObject, false);
      }
      else
      {
        foreach (Task_IdleWorhipper worshipper in Task_IdleWorhipper.Worshippers)
        {
          if (worshipper.t.CurrentTask != null && worshipper.t.CurrentTask.Type == Task_Type.NONE && !worshipper.t.InConversation && (double) worshipper.ConversationCooledDown <= 0.0 && worshipper != this && (double) Vector3.Distance(worshipper.t.transform.position, this.t.transform.position) < 3.0)
          {
            this.cTarget = worshipper;
            this.cTarget.cTarget = this;
            this.BeginConversation();
            break;
          }
        }
      }
    }
  }

  public bool CheckVomit()
  {
    this.ClosestVomit = (Vomit) null;
    float num1 = 10f;
    foreach (Vomit vomit in Vomit.Vomits)
    {
      float num2 = Vector3.Distance(this.t.transform.position, vomit.transform.position);
      if ((double) num2 < (double) num1)
      {
        num1 = num2;
        this.ClosestVomit = vomit;
      }
    }
    return (UnityEngine.Object) this.ClosestVomit != (UnityEngine.Object) null;
  }

  public void ReactSicken()
  {
    this.w.wim.v_i.Illness += 10f;
    this.w.TimedAnimation("Reactions/react-sick", 2.9666667f, new System.Action(this.w.BackToIdle));
  }

  public bool CheckDeadWorshipper()
  {
    this.ClosestDeadWorshipper = (DeadWorshipper) null;
    float num1 = 10f;
    foreach (DeadWorshipper deadWorshipper in DeadWorshipper.DeadWorshippers)
    {
      float num2 = Vector3.Distance(this.t.transform.position, deadWorshipper.transform.position);
      if ((double) num2 < (double) num1)
      {
        num1 = num2;
        this.ClosestDeadWorshipper = deadWorshipper;
      }
    }
    return (UnityEngine.Object) this.ClosestDeadWorshipper != (UnityEngine.Object) null;
  }

  public void ReactGreive()
  {
    this.w.wim.v_i.Illness += 5f;
    switch (this.w.wim.GetRelationship(this.ClosestDeadWorshipper.StructureInfo.FollowerID).CurrentRelationshipState)
    {
      case IDAndRelationship.RelationshipState.Enemies:
        this.w.TimedAnimation("Reactions/react-laugh", 3.33333325f, new System.Action(this.w.BackToIdle));
        this.w.wim.v_i.Faith += 10f;
        break;
      case IDAndRelationship.RelationshipState.Friends:
      case IDAndRelationship.RelationshipState.Lovers:
        this.w.TimedAnimation("Reactions/react-cry", 9f, new System.Action(this.w.BackToIdle));
        this.w.wim.v_i.Faith -= 15f;
        break;
      default:
        this.w.wim.v_i.Faith -= 5f;
        this.w.TimedAnimation("Reactions/react-sad", 2.9666667f, new System.Action(this.w.BackToIdle));
        break;
    }
  }

  public void BeginConversation()
  {
    this.t.ClearPaths();
    this.t.InConversation = true;
    this.cTarget.t.InConversation = true;
    this.cTarget.t.ClearPaths();
    this.ConversationCooledDown = 5f;
    this.cTarget.ConversationCooledDown = 5f;
    this.cTargetPosition = this.t.transform.position + Vector3.right * ((double) this.cTarget.t.transform.position.x < (double) this.t.transform.position.x ? -1f : 1f);
    this.MoveToGameObject = new GameObject();
    this.MoveToGameObject.transform.position = this.cTargetPosition;
    this.cRelationship = this.cTarget.w.wim.GetRelationship(this.w.wim.v_i.ID);
    this.cTarget.w.GoToAndStop(this.MoveToGameObject, new System.Action(this.Conversation), this.t.gameObject, false);
    this.cTarget.w.SetAnimation("Conversations/walkpast-" + this.GetRelationshipAnimation(this.cRelationship.Relationship), true);
    this.Relationship = this.w.wim.GetRelationship(this.cTarget.w.wim.v_i.ID);
    this.w.SetAnimation("Conversations/greet-" + this.GetRelationshipAnimation(this.Relationship.Relationship), false);
    this.w.AddAnimation("Conversations/idle-" + this.GetRelationshipAnimationGoodConversation(this.Relationship.Relationship), true);
    this.state.facingAngle = Utils.GetAngle(this.t.transform.position, this.cTarget.t.transform.position);
  }

  public void Conversation()
  {
    if (!((UnityEngine.Object) this.t != (UnityEngine.Object) null))
      return;
    this.t.StartCoroutine((IEnumerator) this.DoConversation());
  }

  public IEnumerator DoConversation()
  {
    Task_IdleWorhipper taskIdleWorhipper = this;
    taskIdleWorhipper.Node = AstarPath.active.GetNearest(taskIdleWorhipper.t.transform.position).node;
    taskIdleWorhipper.cNode = AstarPath.active.GetNearest(taskIdleWorhipper.cTarget.t.transform.position).node;
    taskIdleWorhipper.Node.Walkable = true;
    taskIdleWorhipper.cNode.Walkable = true;
    taskIdleWorhipper.state.facingAngle = Utils.GetAngle(taskIdleWorhipper.t.transform.position, taskIdleWorhipper.cTarget.t.transform.position);
    taskIdleWorhipper.cTarget.state.facingAngle = Utils.GetAngle(taskIdleWorhipper.cTarget.t.transform.position, taskIdleWorhipper.t.transform.position);
    taskIdleWorhipper.cTarget.t.InConversation = true;
    taskIdleWorhipper.cTarget.t.StopAllCoroutines();
    taskIdleWorhipper.cTarget.t.ClearPaths();
    taskIdleWorhipper.cTarget.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    taskIdleWorhipper.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    taskIdleWorhipper.Score = 0;
    yield return (object) new WaitForEndOfFrame();
    bool TalkNice1 = UnityEngine.Random.Range(0, 100) < 50 || taskIdleWorhipper.w.GuaranteedGoodInteractions && taskIdleWorhipper.cTarget.w.GuaranteedGoodInteractions;
    taskIdleWorhipper.Talk(TalkNice1, taskIdleWorhipper.w, taskIdleWorhipper.cTarget.w, taskIdleWorhipper.Relationship.Relationship, taskIdleWorhipper.cRelationship.Relationship);
    yield return (object) new WaitForSeconds(taskIdleWorhipper.w.simpleAnimator.Duration());
    bool TalkNice2 = UnityEngine.Random.Range(0, 100) < 50 || taskIdleWorhipper.w.GuaranteedGoodInteractions && taskIdleWorhipper.cTarget.w.GuaranteedGoodInteractions;
    taskIdleWorhipper.Talk(TalkNice2, taskIdleWorhipper.cTarget.w, taskIdleWorhipper.w, taskIdleWorhipper.cRelationship.Relationship, taskIdleWorhipper.Relationship.Relationship);
    yield return (object) new WaitForSeconds(taskIdleWorhipper.cTarget.w.simpleAnimator.Duration());
    bool TalkNice3 = UnityEngine.Random.Range(0, 100) < 50 || taskIdleWorhipper.w.GuaranteedGoodInteractions && taskIdleWorhipper.cTarget.w.GuaranteedGoodInteractions;
    taskIdleWorhipper.Talk(TalkNice3, taskIdleWorhipper.w, taskIdleWorhipper.cTarget.w, taskIdleWorhipper.Relationship.Relationship, taskIdleWorhipper.cRelationship.Relationship);
    yield return (object) new WaitForSeconds(taskIdleWorhipper.w.simpleAnimator.Duration());
    bool Fight = false;
    float seconds;
    if (taskIdleWorhipper.Score >= 2)
    {
      ++taskIdleWorhipper.cRelationship.Relationship;
      ++taskIdleWorhipper.Relationship.Relationship;
      if (taskIdleWorhipper.Relationship.CurrentRelationshipState < IDAndRelationship.RelationshipState.Friends && (double) taskIdleWorhipper.Relationship.Relationship >= (double) taskIdleWorhipper.FriendThreshold)
      {
        taskIdleWorhipper.cRelationship.CurrentRelationshipState = IDAndRelationship.RelationshipState.Friends;
        taskIdleWorhipper.Relationship.CurrentRelationshipState = IDAndRelationship.RelationshipState.Friends;
        taskIdleWorhipper.w.SetAnimation("Conversations/become-friends", false);
        taskIdleWorhipper.cTarget.w.SetAnimation("Conversations/become-friends", false);
        taskIdleWorhipper.w.bubble.Play(WorshipperBubble.SPEECH_TYPE.FRIENDS);
        taskIdleWorhipper.cTarget.w.bubble.Play(WorshipperBubble.SPEECH_TYPE.FRIENDS);
        seconds = 5.5f;
      }
      else if (taskIdleWorhipper.Relationship.CurrentRelationshipState < IDAndRelationship.RelationshipState.Lovers && (double) taskIdleWorhipper.Relationship.Relationship >= (double) taskIdleWorhipper.LoveThreshold)
      {
        taskIdleWorhipper.cRelationship.CurrentRelationshipState = IDAndRelationship.RelationshipState.Lovers;
        taskIdleWorhipper.Relationship.CurrentRelationshipState = IDAndRelationship.RelationshipState.Lovers;
        taskIdleWorhipper.w.SetAnimation("Conversations/become-lovers", false);
        taskIdleWorhipper.cTarget.w.SetAnimation("Conversations/become-lovers", false);
        taskIdleWorhipper.w.bubble.Play(WorshipperBubble.SPEECH_TYPE.LOVE);
        taskIdleWorhipper.cTarget.w.bubble.Play(WorshipperBubble.SPEECH_TYPE.LOVE);
        seconds = 5.5f;
      }
      else if (taskIdleWorhipper.Relationship.CurrentRelationshipState == IDAndRelationship.RelationshipState.Lovers)
      {
        taskIdleWorhipper.w.SetAnimation("loving", true);
        taskIdleWorhipper.cTarget.w.SetAnimation("loving", true);
        seconds = 5.33333349f;
      }
      else
      {
        Worshipper w1 = taskIdleWorhipper.w;
        string goodConversation1 = taskIdleWorhipper.GetRelationshipAnimationGoodConversation(taskIdleWorhipper.Relationship.Relationship);
        int num = UnityEngine.Random.Range(1, 4);
        string str1 = num.ToString();
        string Animation1 = $"Conversations/react-{goodConversation1}{str1}";
        w1.SetAnimation(Animation1, false);
        Worshipper w2 = taskIdleWorhipper.cTarget.w;
        string goodConversation2 = taskIdleWorhipper.GetRelationshipAnimationGoodConversation(taskIdleWorhipper.cRelationship.Relationship);
        num = UnityEngine.Random.Range(1, 4);
        string str2 = num.ToString();
        string Animation2 = $"Conversations/react-{goodConversation2}{str2}";
        w2.SetAnimation(Animation2, false);
        seconds = 2f;
      }
      taskIdleWorhipper.w.AddAnimation("idle", true);
      taskIdleWorhipper.cTarget.w.AddAnimation("idle", true);
    }
    else
    {
      --taskIdleWorhipper.cRelationship.Relationship;
      --taskIdleWorhipper.Relationship.Relationship;
      if (taskIdleWorhipper.Relationship.CurrentRelationshipState > IDAndRelationship.RelationshipState.Enemies && (double) taskIdleWorhipper.Relationship.Relationship <= (double) taskIdleWorhipper.HateThreshold)
      {
        taskIdleWorhipper.cRelationship.CurrentRelationshipState = IDAndRelationship.RelationshipState.Enemies;
        taskIdleWorhipper.Relationship.CurrentRelationshipState = IDAndRelationship.RelationshipState.Enemies;
        taskIdleWorhipper.w.SetAnimation("Conversations/become-enemies", false);
        taskIdleWorhipper.cTarget.w.SetAnimation("Conversations/become-enemies", false);
        taskIdleWorhipper.w.bubble.Play(WorshipperBubble.SPEECH_TYPE.ENEMIES);
        taskIdleWorhipper.cTarget.w.bubble.Play(WorshipperBubble.SPEECH_TYPE.ENEMIES);
        seconds = 5.5f;
      }
      else if (taskIdleWorhipper.Relationship.CurrentRelationshipState == IDAndRelationship.RelationshipState.Enemies && UnityEngine.Random.Range(0, 100) < 33)
      {
        taskIdleWorhipper.w.SetAnimation("fight", true);
        taskIdleWorhipper.cTarget.w.SetAnimation("fight", true);
        seconds = 5f;
        Fight = true;
      }
      else
      {
        Worshipper w3 = taskIdleWorhipper.w;
        string animationBadConversation1 = taskIdleWorhipper.GetRelationshipAnimationBadConversation(taskIdleWorhipper.Relationship.Relationship);
        int num = UnityEngine.Random.Range(1, 4);
        string str3 = num.ToString();
        string Animation3 = $"Conversations/react-{animationBadConversation1}{str3}";
        w3.SetAnimation(Animation3, false);
        Worshipper w4 = taskIdleWorhipper.cTarget.w;
        string animationBadConversation2 = taskIdleWorhipper.GetRelationshipAnimationBadConversation(taskIdleWorhipper.cRelationship.Relationship);
        num = UnityEngine.Random.Range(1, 4);
        string str4 = num.ToString();
        string Animation4 = $"Conversations/react-{animationBadConversation2}{str4}";
        w4.SetAnimation(Animation4, false);
        seconds = 2f;
      }
      taskIdleWorhipper.w.AddAnimation("Conversations/idle-" + taskIdleWorhipper.GetRelationshipAnimation(taskIdleWorhipper.Relationship.Relationship), true);
      taskIdleWorhipper.cTarget.w.AddAnimation("Conversations/idle-" + taskIdleWorhipper.GetRelationshipAnimation(taskIdleWorhipper.cRelationship.Relationship), true);
    }
    yield return (object) new WaitForSeconds(seconds);
    taskIdleWorhipper.Node.Walkable = true;
    taskIdleWorhipper.cNode.Walkable = true;
    taskIdleWorhipper.t.InConversation = false;
    taskIdleWorhipper.Timer = 0.0f;
    taskIdleWorhipper.cTarget.t.InConversation = false;
    taskIdleWorhipper.cTarget.Timer = 0.0f;
    taskIdleWorhipper.cTarget.state.CURRENT_STATE = StateMachine.State.Idle;
    taskIdleWorhipper.state.CURRENT_STATE = StateMachine.State.Idle;
    UnityEngine.Object.Destroy((UnityEngine.Object) taskIdleWorhipper.MoveToGameObject);
    taskIdleWorhipper.MoveToGameObject = (GameObject) null;
    if (Fight)
      taskIdleWorhipper.w.Die();
  }

  public void Talk(
    bool TalkNice,
    Worshipper Speaker,
    Worshipper Reactor,
    int Relationship,
    int ReactorRelationship)
  {
    Speaker.SetAnimation(TalkNice ? $"Conversations/talk-{this.GetRelationshipAnimationGoodConversation(Relationship)}{UnityEngine.Random.Range(1, 4).ToString()}" : $"Conversations/talk-{this.GetRelationshipAnimationBadConversation(Relationship)}{UnityEngine.Random.Range(1, 4).ToString()}", true);
    Reactor.SetAnimation("Conversations/idle-" + this.GetRelationshipAnimation(ReactorRelationship), true);
    this.Score += TalkNice ? 1 : 0;
  }

  public string GetRelationshipAnimation(int Relationship)
  {
    if ((double) Relationship < (double) this.HateThreshold)
      return "hate";
    if (Utils.WithinRange((float) this.cRelationship.Relationship, this.HateThreshold, -1f))
      return "mean";
    return Utils.WithinRange((float) this.cRelationship.Relationship, 0.0f, this.LoveThreshold) || (double) Relationship <= (double) this.LoveThreshold ? "nice" : "love";
  }

  public string GetRelationshipAnimationGoodConversation(int Relationship)
  {
    return (double) Relationship > (double) this.LoveThreshold ? "love" : "nice";
  }

  public string GetRelationshipAnimationBadConversation(int Relationship)
  {
    return (double) Relationship < (double) this.HateThreshold ? "hate" : "mean";
  }

  public float HateThreshold => Villager_Info.RelationshipHateThreshold;

  public float FriendThreshold => Villager_Info.RelationshipFriendThreshold;

  public float LoveThreshold => Villager_Info.RelationshipLoveThreshold;
}
