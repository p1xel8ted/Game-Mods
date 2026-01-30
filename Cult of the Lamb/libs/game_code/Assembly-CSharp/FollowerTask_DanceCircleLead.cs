// Decompiled with JetBrains decompiler
// Type: FollowerTask_DanceCircleLead
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerTask_DanceCircleLead : FollowerTask
{
  public const float DANCE_DURATION_GAME_MINUTES = 180f;
  public const float EMPTY_CIRCLE_TIMEOUT_GAME_MINUTES = 10f;
  public List<FollowerTask_DanceCircleFollow> _dancers = new List<FollowerTask_DanceCircleFollow>();
  public float _emptyDancerListCountdown;
  public float _remainingDanceDuration;
  [CompilerGenerated]
  public Vector3 \u003CCenterPosition\u003Ek__BackingField;
  public Interaction_BackToWork backToWorkInteraction;

  public override FollowerTaskType Type => FollowerTaskType.DanceCircleLead;

  public override FollowerLocation Location => FollowerLocation.Base;

  public int LeaderID => this._brain == null || this._brain.Info == null ? 0 : this._brain.Info.ID;

  public int RemainingSlotCount => 4 - this._dancers.Count;

  public Vector3 CenterPosition
  {
    get => this.\u003CCenterPosition\u003Ek__BackingField;
    set => this.\u003CCenterPosition\u003Ek__BackingField = value;
  }

  public FollowerTask_DanceCircleLead() => this._remainingDanceDuration = 180f;

  public FollowerTask_DanceCircleLead(float remainingDuration, Vector3 centerPosition)
  {
    this._remainingDanceDuration = remainingDuration;
    this.CenterPosition = centerPosition;
  }

  public override int GetSubTaskCode() => 0;

  public override void OnStart()
  {
    int locationState = (int) LocationManager.GetLocationState(this.Location);
    Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    this.CenterPosition = locationState != 3 || !((UnityEngine.Object) followerById != (UnityEngine.Object) null) ? TownCentre.RandomPositionInCachedTownCentre() : followerById.transform.position;
    this.SetState(FollowerTaskState.GoingTo);
  }

  public override void OnAbort()
  {
    base.OnAbort();
    this._remainingDanceDuration = -1f;
  }

  public override void OnComplete()
  {
    if ((double) this._remainingDanceDuration > 0.0 && (double) this._emptyDancerListCountdown > 0.0 && this._dancers.Count > 1)
    {
      this.TransferLeadership();
    }
    else
    {
      foreach (FollowerTask dancer in this._dancers)
        dancer.End();
    }
    this._dancers.Clear();
  }

  public override void TaskTick(float deltaGameTime)
  {
    if ((double) (this._remainingDanceDuration -= deltaGameTime) <= 0.0)
      this.End();
    else if (this._dancers.Count == 0)
    {
      if ((double) (this._emptyDancerListCountdown -= deltaGameTime) <= 0.0)
        this.End();
    }
    else
      this._emptyDancerListCountdown = 10f;
    if (TimeManager.IsNight && !this._brain._directInfoAccess.WorkThroughNight)
      this._brain.CheckChangeTask();
    if (SeasonsManager.CurrentWeatherEvent != SeasonsManager.WeatherEvent.Blizzard || this._brain.CanWork)
      return;
    this.End();
  }

  public void TransferLeadership()
  {
    FollowerTask_DanceCircleFollow dancer = this._dancers[0];
    FollowerTask_DanceCircleLead newCircle1 = new FollowerTask_DanceCircleLead(this._remainingDanceDuration, this.CenterPosition);
    FollowerTask_DanceCircleLead newCircle2 = newCircle1;
    dancer.BecomeLeader(newCircle2);
    for (int index = 1; index < this._dancers.Count; ++index)
      this._dancers[index].TransferToNewCircle(newCircle1);
  }

  public void JoinCircle(FollowerTask_DanceCircleFollow newDancer)
  {
    this._dancers.Add(newDancer);
    foreach (FollowerTask dancer in this._dancers)
      dancer.RecalculateDestination();
  }

  public Vector3 GetCirclePosition(FollowerBrain brain)
  {
    int num1 = -1;
    if (this._brain != null)
    {
      if (this._brain.Info.ID == brain.Info.ID)
      {
        num1 = 0;
      }
      else
      {
        for (int index = 0; index < this._dancers.Count; ++index)
        {
          if (this._dancers[index].DancerID == brain.Info.ID)
          {
            num1 = index + 1;
            break;
          }
        }
      }
    }
    if (num1 < 0)
      return Vector3.zero;
    int num2 = this._dancers.Count + 1;
    float f = (float) ((double) num1 * (360.0 / (double) num2) * (Math.PI / 180.0));
    return this.CenterPosition + new Vector3(1f * Mathf.Cos(f), 1f * Mathf.Sin(f));
  }

  public void UndoStateAnimationChanges(Follower follower)
  {
    SimpleSpineAnimator.SpineChartacterAnimationData animationData = follower.SimpleAnimator.GetAnimationData(StateMachine.State.Idle);
    animationData.Animation = animationData.DefaultAnimation;
    follower.ResetStateAnimations();
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    return this.GetCirclePosition(this._brain);
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    if (this.State != FollowerTaskState.Doing)
      return;
    follower.State.facingAngle = Utils.GetAngle(follower.transform.position, this.CenterPosition);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "dance");
  }

  public override void OnDoingBegin(Follower follower)
  {
    follower.State.facingAngle = Utils.GetAngle(follower.transform.position, this.CenterPosition);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "dance");
  }

  public override void Cleanup(Follower follower)
  {
    this._brain.AddThought(Thought.DanceCircleLed);
    this.UndoStateAnimationChanges(follower);
    if ((bool) (UnityEngine.Object) follower.GetComponent<Interaction_BackToWork>())
      UnityEngine.Object.Destroy((UnityEngine.Object) follower.GetComponent<Interaction_BackToWork>());
    base.Cleanup(follower);
  }

  public override void SimCleanup(SimFollower simFollower)
  {
    this._brain.AddThought(Thought.DanceCircleLed);
    base.SimCleanup(simFollower);
  }
}
