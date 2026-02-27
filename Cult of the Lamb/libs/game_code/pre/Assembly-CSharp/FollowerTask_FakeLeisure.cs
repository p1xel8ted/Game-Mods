// Decompiled with JetBrains decompiler
// Type: FollowerTask_FakeLeisure
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerTask_FakeLeisure : FollowerTask
{
  private float _gameTimeToNextStateUpdate;
  private float _searchCooldownGameMinutes;

  public override FollowerTaskType Type => FollowerTaskType.FakeLeisure;

  public override FollowerLocation Location => this._brain.HomeLocation;

  public bool Searching { get; private set; }

  protected override int GetSubTaskCode() => 0;

  protected override void OnStart()
  {
    this._searchCooldownGameMinutes = 10f;
    this.SetState(FollowerTaskState.GoingTo);
  }

  protected override void OnArrive() => this.SetState(FollowerTaskState.Idle);

  protected override void TaskTick(float deltaGameTime)
  {
    if (!this.Searching && this.State > FollowerTaskState.WaitingForLocation && (double) (this._searchCooldownGameMinutes -= deltaGameTime) < 0.0)
      this.Searching = true;
    if (this.TryFindCompanionTask() || this._state != FollowerTaskState.Idle)
      return;
    this._gameTimeToNextStateUpdate -= deltaGameTime;
    if ((double) this._gameTimeToNextStateUpdate > 0.0)
      return;
    if ((double) Random.Range(0.0f, 1f) < 0.5)
      this.Wander();
    this._gameTimeToNextStateUpdate = Random.Range(20f, 30f);
  }

  private void Wander()
  {
    this.ClearDestination();
    this.SetState(FollowerTaskState.GoingTo);
  }

  private bool TryFindCompanionTask()
  {
    if (this.Searching)
    {
      foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
      {
        if (allBrain != this._brain && allBrain.Location == this.Location)
        {
          if (allBrain.CurrentTaskType == FollowerTaskType.DanceCircleLead)
          {
            FollowerTask_DanceCircleLead currentTask = (FollowerTask_DanceCircleLead) allBrain.CurrentTask;
            if (currentTask.RemainingSlotCount > 0)
            {
              this.InitiateLeisureTask((FollowerTask) new FollowerTask_DanceCircleFollow()
              {
                LeadTask = currentTask
              });
              return true;
            }
          }
          if (allBrain.CurrentTaskType == FollowerTaskType.FakeLeisure)
          {
            FollowerTask_FakeLeisure currentTask = (FollowerTask_FakeLeisure) allBrain.CurrentTask;
            if (currentTask.Searching)
            {
              if ((double) Random.Range(0.0f, 1f) < 0.5)
              {
                this.InitiateChat(allBrain, currentTask);
                return true;
              }
              this.InitiateDanceCircle(allBrain, currentTask);
              return true;
            }
          }
        }
      }
    }
    return false;
  }

  private void InitiateChat(FollowerBrain otherBrain, FollowerTask_FakeLeisure otherLeisure)
  {
    FollowerTask_Chat leisureTask1 = new FollowerTask_Chat(otherBrain.Info.ID, true);
    FollowerTask_Chat leisureTask2 = new FollowerTask_Chat(this._brain.Info.ID, false);
    leisureTask1.OtherChatTask = leisureTask2;
    leisureTask2.OtherChatTask = leisureTask1;
    otherLeisure.InitiateLeisureTask((FollowerTask) leisureTask2);
    this.InitiateLeisureTask((FollowerTask) leisureTask1);
  }

  private void InitiateDanceCircle(FollowerBrain otherBrain, FollowerTask_FakeLeisure otherLeisure)
  {
    FollowerTask_DanceCircleLead leisureTask1 = new FollowerTask_DanceCircleLead();
    FollowerTask_DanceCircleFollow leisureTask2 = new FollowerTask_DanceCircleFollow();
    leisureTask2.LeadTask = leisureTask1;
    this.InitiateLeisureTask((FollowerTask) leisureTask1);
    otherLeisure.InitiateLeisureTask((FollowerTask) leisureTask2);
  }

  public void InitiateLeisureTask(FollowerTask leisureTask)
  {
    this.Searching = false;
    if (this.Brain.CurrentTaskType == FollowerTaskType.ManualControl)
      return;
    this._brain.TransitionToTask(leisureTask);
  }

  private void UndoStateAnimationChanges(Follower follower)
  {
    SimpleSpineAnimator.SpineChartacterAnimationData animationData = follower.SimpleAnimator.GetAnimationData(StateMachine.State.Idle);
    animationData.Animation = animationData.DefaultAnimation;
    follower.ResetStateAnimations();
  }

  protected override Vector3 UpdateDestination(Follower follower)
  {
    return TownCentre.RandomPositionInCachedTownCentre();
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "meditate");
  }

  public override void Cleanup(Follower follower)
  {
    this.UndoStateAnimationChanges(follower);
    if ((bool) (Object) follower && (bool) (Object) follower.GetComponent<Interaction_BackToWork>())
      Object.Destroy((Object) follower.GetComponent<Interaction_BackToWork>());
    base.Cleanup(follower);
  }
}
