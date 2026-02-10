// Decompiled with JetBrains decompiler
// Type: FollowerTask_NudeDancing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerTask_NudeDancing : FollowerTask
{
  public static List<FollowerBrain> FollowersInnerCircle = new List<FollowerBrain>();
  public static int innerIndexOffset = 0;
  public static int outerIndexOffset = 0;
  public static float progressDirectionChange = 0.0f;
  public static float targetDirectionChange = 0.0f;
  public static int direction = 1;
  public static float timeProgress = 0.0f;
  public Follower follower;

  public override FollowerTaskType Type => FollowerTaskType.NudeDancing;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockReactTasks => true;

  public override bool BlockSermon => true;

  public override bool BlockSocial => true;

  public override bool BlockTaskChanges => true;

  public override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  public override void ClaimReservations()
  {
    base.ClaimReservations();
    if (!FollowerTask_NudeDancing.FollowersInnerCircle.Contains(this.Brain))
      FollowerTask_NudeDancing.FollowersInnerCircle.Add(this.Brain);
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain != this._brain && allBrain.CurrentTaskType == FollowerTaskType.NudeDancing)
        allBrain.CurrentTask.RecalculateDestination();
    }
  }

  public override void ReleaseReservations()
  {
    base.ReleaseReservations();
    if (FollowerTask_NudeDancing.FollowersInnerCircle.Contains(this.Brain))
      FollowerTask_NudeDancing.FollowersInnerCircle.Remove(this.Brain);
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain != this._brain && allBrain.CurrentTaskType == FollowerTaskType.NudeDancing)
        allBrain.CurrentTask.RecalculateDestination();
    }
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    this.follower = follower;
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "prance");
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "prance");
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "idle");
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run");
  }

  public override void OnGoingToBegin(Follower follower)
  {
    base.OnGoingToBegin(follower);
    if (!((UnityEngine.Object) follower.Spine != (UnityEngine.Object) null) || follower.Spine.AnimationState == null || follower.Spine.AnimationState.GetCurrent(1) == null || follower.Spine.AnimationState.GetCurrent(1).Animation == null || !(follower.Spine.AnimationState.GetCurrent(1).Animation.Name != "prance"))
      return;
    follower.Spine.AnimationState.SetAnimation(1, "prance", true);
  }

  public override void OnArrive()
  {
    base.OnArrive();
    if (FollowerTask_NudeDancing.FollowersInnerCircle.Count <= 0 || FollowerTask_NudeDancing.FollowersInnerCircle[0] != this._brain)
      return;
    this.InnerRecalculate();
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    int index = (int) Utils.Repeat((float) (FollowerTask_NudeDancing.FollowersInnerCircle.IndexOf(follower.Brain) + FollowerTask_NudeDancing.innerIndexOffset), (float) FollowerTask_NudeDancing.FollowersInnerCircle.Count);
    return this.GetCirclePos(TownCentre.Instance.Centre.transform.position, 3f, index, FollowerTask_NudeDancing.FollowersInnerCircle.Count);
  }

  public Vector3 GetCirclePos(Vector3 center, float distance, int index, int count)
  {
    float f = (float) ((double) index * (360.0 / (double) count) * (Math.PI / 180.0));
    return center + new Vector3(distance * Mathf.Cos(f), distance * Mathf.Sin(f));
  }

  public override int GetSubTaskCode() => 0;

  public override void TaskTick(float deltaGameTime)
  {
    if (this.State == FollowerTaskState.Finalising || this.State == FollowerTaskState.Done)
      return;
    if ((UnityEngine.Object) this.follower != (UnityEngine.Object) null)
      this.follower.FacePosition(TownCentre.Instance.Centre.transform.position);
    if (FollowerTask_NudeDancing.FollowersInnerCircle.Count > 0 && FollowerTask_NudeDancing.FollowersInnerCircle[0] == this._brain)
    {
      FollowerTask_NudeDancing.progressDirectionChange += deltaGameTime;
      double progressDirectionChange = (double) FollowerTask_NudeDancing.progressDirectionChange;
      double targetDirectionChange = (double) FollowerTask_NudeDancing.targetDirectionChange;
    }
    if (FollowerTask_NudeDancing.FollowersInnerCircle.Count <= 0 || FollowerTask_NudeDancing.FollowersInnerCircle[0] != this._brain)
      return;
    FollowerTask_NudeDancing.timeProgress += deltaGameTime;
  }

  public void InnerRecalculate()
  {
    FollowerTask_NudeDancing.innerIndexOffset -= FollowerTask_NudeDancing.direction;
    foreach (FollowerBrain followerBrain in FollowerTask_NudeDancing.FollowersInnerCircle)
      followerBrain.CurrentTask.RecalculateDestination();
  }
}
