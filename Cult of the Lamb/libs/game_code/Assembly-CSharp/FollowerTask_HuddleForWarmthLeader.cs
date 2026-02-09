// Decompiled with JetBrains decompiler
// Type: FollowerTask_HuddleForWarmthLeader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerTask_HuddleForWarmthLeader : FollowerTask
{
  public const float DANCE_DURATION_GAME_MINUTES = 180f;
  public const float EMPTY_CIRCLE_TIMEOUT_GAME_MINUTES = 10f;
  public List<FollowerTask_HuddleForWarmthFollower> _dancers = new List<FollowerTask_HuddleForWarmthFollower>();
  public float _emptyDancerListCountdown;
  public float _remainingDanceDuration;
  public bool TargetingStructure;
  [CompilerGenerated]
  public Vector3 \u003CCenterPosition\u003Ek__BackingField;
  [CompilerGenerated]
  public float \u003CDistance\u003Ek__BackingField = 0.6f;
  public Interaction_BackToWork backToWorkInteraction;

  public override FollowerTaskType Type => FollowerTaskType.HuddleForWarmthLeader;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockReactTasks => true;

  public override bool BlockSocial => true;

  public int LeaderID => this._brain == null || this._brain.Info == null ? 0 : this._brain.Info.ID;

  public int RemainingSlotCount => 4 - this._dancers.Count;

  public Vector3 CenterPosition
  {
    get => this.\u003CCenterPosition\u003Ek__BackingField;
    set => this.\u003CCenterPosition\u003Ek__BackingField = value;
  }

  public float Distance
  {
    get => this.\u003CDistance\u003Ek__BackingField;
    set => this.\u003CDistance\u003Ek__BackingField = value;
  }

  public FollowerTask_HuddleForWarmthLeader() => this._remainingDanceDuration = 180f;

  public FollowerTask_HuddleForWarmthLeader(float remainingDuration, Vector3 centerPosition)
  {
    this._remainingDanceDuration = remainingDuration;
    this.CenterPosition = centerPosition;
  }

  public override int GetSubTaskCode() => 0;

  public override void OnStart()
  {
    LocationState locationState = LocationManager.GetLocationState(this.Location);
    Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    List<StructureBrain> structuresOfTypes = StructureManager.GetAllStructuresOfTypes(StructureBrain.TYPES.DECORATION_TORCH_BIG, StructureBrain.TYPES.DECORATION_SPIDER_TORCH);
    structuresOfTypes.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType<Structures_Furnace>());
    for (int index = structuresOfTypes.Count - 1; index >= 0; --index)
    {
      if (structuresOfTypes[index].Data.Fuel <= 0)
        structuresOfTypes.RemoveAt(index);
    }
    if (structuresOfTypes.Count > 0)
    {
      this.Distance = 1f;
      this.CenterPosition = structuresOfTypes[UnityEngine.Random.Range(0, structuresOfTypes.Count)].Data.Position;
      this.TargetingStructure = true;
    }
    else
      this.CenterPosition = locationState != LocationState.Active || !((UnityEngine.Object) followerById != (UnityEngine.Object) null) ? TownCentre.RandomPositionInCachedTownCentre() : followerById.transform.position;
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
    if (!TimeManager.IsNight || this._brain._directInfoAccess.WorkThroughNight)
      return;
    this._brain.CheckChangeTask();
  }

  public void TransferLeadership()
  {
    FollowerTask_HuddleForWarmthFollower dancer = this._dancers[0];
    FollowerTask_HuddleForWarmthLeader newCircle1 = new FollowerTask_HuddleForWarmthLeader(this._remainingDanceDuration, this.CenterPosition);
    FollowerTask_HuddleForWarmthLeader newCircle2 = newCircle1;
    dancer.BecomeLeader(newCircle2);
    for (int index = 1; index < this._dancers.Count; ++index)
      this._dancers[index].TransferToNewCircle(newCircle1);
  }

  public void JoinCircle(FollowerTask_HuddleForWarmthFollower newDancer)
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
    if (num1 >= 0)
    {
      int num2 = this._dancers.Count + 1;
      float f = (float) ((double) num1 * (360.0 / (double) num2) * (Math.PI / 180.0));
      return this.CenterPosition + new Vector3(this.Distance * Mathf.Cos(f), this.Distance * Mathf.Sin(f)) + (Vector3) UnityEngine.Random.insideUnitCircle;
    }
    return (UnityEngine.Object) TownCentre.Instance != (UnityEngine.Object) null ? TownCentre.Instance.RandomPositionInTownCentre() : new Vector3((float) UnityEngine.Random.Range(-5, 5), (float) UnityEngine.Random.Range(-15, -5), 0.0f);
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
    if (this.State == FollowerTaskState.Doing)
    {
      string NewAnimation = "Snow/idle-sad";
      follower.State.facingAngle = Utils.GetAngle(follower.transform.position, this.CenterPosition);
      if (SeasonsManager.CurrentWeatherEvent == SeasonsManager.WeatherEvent.Blizzard)
      {
        float num = UnityEngine.Random.value;
        if ((double) num < 0.20000000298023224)
          NewAnimation = "Snow/blow-on-hands";
        else if ((double) num < 0.40000000596046448)
          NewAnimation = "Snow/shuffle";
      }
      else
        NewAnimation = !this.TargetingStructure ? ((double) WarmthBar.WarmthNormalized > 0.25 ? "Snow/idle-smile" : "Snow/idle-sad") : $"Furnace/furnace-warm{UnityEngine.Random.Range(1, 6)}";
      follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, NewAnimation);
    }
    follower.gameObject.AddComponent<Interaction_BackToWork>().Init(follower, true);
  }

  public override void OnDoingBegin(Follower follower)
  {
    string NewAnimation = "Snow/idle-sad";
    follower.State.facingAngle = Utils.GetAngle(follower.transform.position, this.CenterPosition);
    if (SeasonsManager.CurrentWeatherEvent == SeasonsManager.WeatherEvent.Blizzard)
    {
      float num = UnityEngine.Random.value;
      if ((double) num < 0.20000000298023224)
        NewAnimation = "Snow/blow-on-hands";
      else if ((double) num < 0.40000000596046448)
        NewAnimation = "Snow/shuffle";
    }
    else
      NewAnimation = !this.TargetingStructure ? ((double) WarmthBar.WarmthNormalized > 0.25 ? "Snow/idle-smile" : "Snow/idle-sad") : $"Furnace/furnace-warm{UnityEngine.Random.Range(1, 6)}";
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, NewAnimation);
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
