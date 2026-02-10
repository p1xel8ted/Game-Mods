// Decompiled with JetBrains decompiler
// Type: FollowerTask_Knucklebones
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections;
using UnityEngine;

#nullable disable
public class FollowerTask_Knucklebones : FollowerTask
{
  public FollowerBrain targetFollower;
  public Follower target;
  public Follower leader;
  public Coroutine playingRoutine;
  public int structureID;
  public bool isLeader;

  public override FollowerTaskType Type => FollowerTaskType.Knucklebones;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockReactTasks => true;

  public override bool BlockSocial => true;

  public override bool BlockTaskChanges => true;

  public FollowerTask_Knucklebones(FollowerBrain targetFollower, int structureID, bool isLeader)
  {
    this.structureID = structureID;
    this.targetFollower = targetFollower;
    this.isLeader = isLeader;
  }

  public override int GetSubTaskCode() => this.structureID;

  public override void TaskTick(float deltaGameTime)
  {
    if (this.targetFollower.CurrentTaskType != FollowerTaskType.Knucklebones)
      this.End();
    if (TimeManager.IsNight)
      this.End();
    if (PlayerFarming.Location == FollowerLocation.Base && this.playingRoutine == null && this.isLeader && this._state == FollowerTaskState.Doing)
    {
      this.BeginPlaying();
    }
    else
    {
      if (PlayerFarming.Location == FollowerLocation.Base)
        return;
      this.playingRoutine = (Coroutine) null;
    }
  }

  public override void OnStart()
  {
    base.OnStart();
    this.OnFollowerTaskStateChanged = this.OnFollowerTaskStateChanged + new FollowerTask.FollowerTaskDelegate(this.StateChange);
    if (this.targetFollower != null && this.isLeader)
    {
      this.targetFollower.CurrentTask?.Abort();
      this.targetFollower.HardSwapToTask((FollowerTask) new FollowerTask_Knucklebones(this.Brain, this.structureID, false));
    }
    this.ClearDestination();
    this.SetState(FollowerTaskState.GoingTo);
  }

  public override void OnDoingBegin(Follower follower)
  {
    base.OnDoingBegin(follower);
    if (!(bool) (Object) follower || !((Object) follower.gameObject.GetComponent<Interaction_BackToWork>() == (Object) null))
      return;
    Interaction_BackToWork interactionBackToWork = follower.gameObject.AddComponent<Interaction_BackToWork>();
    interactionBackToWork.Init(follower);
    interactionBackToWork.LockPosition = follower.transform;
  }

  public void StateChange(FollowerTaskState oldState, FollowerTaskState newState)
  {
    if (oldState != FollowerTaskState.GoingTo || newState != FollowerTaskState.Doing)
      return;
    this.BeginPlaying();
  }

  public void BeginPlaying()
  {
    Follower target = (Follower) null;
    Follower enforcer = (Follower) null;
    foreach (Follower follower in FollowerManager.FollowersAtLocation(FollowerLocation.Base))
    {
      if (follower.Brain.Info.ID == this.targetFollower.Info.ID)
        target = this.target = follower;
      else if (follower.Brain.Info.ID == this._brain.Info.ID)
        enforcer = this.leader = follower;
    }
    if ((bool) (Object) target && (bool) (Object) enforcer && enforcer.gameObject.activeInHierarchy)
    {
      if (!this.isLeader)
        return;
      if (this.playingRoutine != null)
        enforcer.StopCoroutine(this.playingRoutine);
      this.playingRoutine = enforcer.StartCoroutine((IEnumerator) this.PlayingIE(target, enforcer));
    }
    else
    {
      this.targetFollower.CompleteCurrentTask();
      this.Brain.CompleteCurrentTask();
    }
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    this.OnFollowerTaskStateChanged = this.OnFollowerTaskStateChanged - new FollowerTask.FollowerTaskDelegate(this.StateChange);
    Interaction component = (Interaction) follower.GetComponent<interaction_FollowerInteraction>();
    if ((bool) (Object) component)
      component.enabled = true;
    if ((bool) (Object) follower.GetComponent<Interaction_BackToWork>())
      Object.Destroy((Object) follower.GetComponent<Interaction_BackToWork>());
    if (this.playingRoutine == null)
      return;
    follower.StopCoroutine(this.playingRoutine);
  }

  public IEnumerator PlayingIE(Follower target, Follower enforcer)
  {
    FollowerTask_Knucklebones taskKnucklebones = this;
    Interaction_KnucklebonesBuilding building = (Interaction_KnucklebonesBuilding) null;
    foreach (Interaction_KnucklebonesBuilding knuckleboneBuilding in Interaction_KnucklebonesBuilding.KnuckleboneBuildings)
    {
      if (knuckleboneBuilding.Structure.Brain.Data.ID == taskKnucklebones.structureID)
        building = knuckleboneBuilding;
    }
    enforcer.State.facingAngle = Utils.GetAngle(enforcer.transform.position, building.transform.position);
    while (target.Brain.CurrentTask != null && target.Brain.CurrentTask.State != FollowerTaskState.Doing)
      yield return (object) null;
    while (enforcer.Brain.CurrentTask != null && enforcer.Brain.CurrentTask.State != FollowerTaskState.Doing)
      yield return (object) null;
    target.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    enforcer.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    target.State.facingAngle = Utils.GetAngle(target.transform.position, building.transform.position);
    foreach (GameObject die in building.Dice)
      die.gameObject.SetActive(false);
    int turns = Random.Range(10, 20);
    string[] anims = new string[4]
    {
      "knucklebones/knucklebones-playing",
      "knucklebones/knucklebones-playing2",
      "knucklebones/knucklebones-playing3",
      "knucklebones/knucklebones-playing4"
    };
    for (int i = 0; i < turns; ++i)
    {
      double num1 = (double) target.SetBodyAnimation(anims[Random.Range(0, anims.Length)], false);
      target.AddBodyAnimation("idle", true, 0.0f);
      yield return (object) new WaitForSeconds(1f);
      int index1 = Random.Range(0, building.Dice.Length);
      building.Dice[index1].gameObject.SetActive(true);
      building.Dice[index1].transform.DOPunchScale(Vector3.one * 0.2f, 0.25f).SetEase<Tweener>(Ease.OutBounce);
      yield return (object) new WaitForSeconds(Random.Range(0.5f, 1.25f));
      double num2 = (double) enforcer.SetBodyAnimation(anims[Random.Range(0, anims.Length)], false);
      enforcer.AddBodyAnimation("idle", true, 0.0f);
      yield return (object) new WaitForSeconds(1f);
      int index2 = Random.Range(0, building.Dice.Length);
      building.Dice[index2].gameObject.SetActive(true);
      building.Dice[index2].transform.DOPunchScale(Vector3.one * 0.2f, 0.25f).SetEase<Tweener>(Ease.OutBounce);
      yield return (object) new WaitForSeconds(Random.Range(0.5f, 1.25f));
    }
    taskKnucklebones.DoResultsThoughts();
    taskKnucklebones.Complete();
  }

  public void DoResultsThoughts()
  {
    float num = Random.value;
    ThoughtData thought1;
    ThoughtData thought2;
    if ((double) num < 0.40000000596046448)
    {
      thought1 = this.GetWinThought();
      thought2 = this.GetLoseThought();
    }
    else if ((double) num < 0.800000011920929)
    {
      thought1 = this.GetLoseThought();
      thought2 = this.GetWinThought();
    }
    else
    {
      thought1 = this.GetDrawThought();
      thought2 = this.GetDrawThought();
    }
    thought1.Init();
    thought2.Init();
    this._brain.AddThought(thought1);
    this.targetFollower.AddThought(thought2);
  }

  public ThoughtData GetWinThought()
  {
    return FollowerThoughts.GetData((double) Random.value < 0.5 ? Thought.Knucklebones_Win_0 : Thought.Knucklebones_Win_2);
  }

  public ThoughtData GetLoseThought()
  {
    return FollowerThoughts.GetData((double) Random.value < 0.5 ? Thought.Knucklebones_Lost_0 : Thought.Knucklebones_Lost_1);
  }

  public ThoughtData GetDrawThought()
  {
    return FollowerThoughts.GetData((double) Random.value < 0.5 ? Thought.Knucklebones_Draw_0 : Thought.Knucklebones_Draw_1);
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    Interaction_KnucklebonesBuilding knucklebonesBuilding = (Interaction_KnucklebonesBuilding) null;
    foreach (Interaction_KnucklebonesBuilding knuckleboneBuilding in Interaction_KnucklebonesBuilding.KnuckleboneBuildings)
    {
      if (knuckleboneBuilding.Structure.Brain.Data.ID == this.structureID)
        knucklebonesBuilding = knuckleboneBuilding;
    }
    return (Object) knucklebonesBuilding != (Object) null ? knucklebonesBuilding.FollowerPositions[this.isLeader ? 0 : 1].transform.position : Vector3.zero;
  }

  public override void OnAbort()
  {
    base.OnAbort();
    this.OnFollowerTaskStateChanged = this.OnFollowerTaskStateChanged - new FollowerTask.FollowerTaskDelegate(this.StateChange);
    if (this.playingRoutine == null || !((Object) this.leader != (Object) null) || !this.leader.gameObject.activeInHierarchy)
      return;
    this.leader.StopCoroutine(this.playingRoutine);
    this.playingRoutine = (Coroutine) null;
    if (this.targetFollower == null)
      return;
    this.targetFollower.CompleteCurrentTask();
  }

  public void CheckToEndTask()
  {
    foreach (Follower follower in FollowerManager.FollowersAtLocation(FollowerLocation.Base))
    {
      if (follower.Brain.Info.ID == this.targetFollower.Info.ID)
        this.target = follower;
      else if (follower.Brain.Info.ID == this._brain.Info.ID)
        this.leader = follower;
    }
    if (this.targetFollower != null)
    {
      this.End();
      if ((Object) this.leader != (Object) null && this.leader.Brain != null && this.leader.Brain.CurrentTask != null && this.leader.Brain.CurrentTaskType == FollowerTaskType.Knucklebones)
        this.leader.Brain.CurrentTask?.Abort();
    }
    if (!((Object) this.leader != (Object) null))
      return;
    this.End();
    if (this.targetFollower == null || this.targetFollower.CurrentTask == null || this.targetFollower.CurrentTaskType != FollowerTaskType.Knucklebones)
      return;
    this.targetFollower.CurrentTask?.Abort();
  }
}
