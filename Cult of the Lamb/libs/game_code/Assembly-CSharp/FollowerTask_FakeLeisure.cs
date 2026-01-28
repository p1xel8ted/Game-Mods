// Decompiled with JetBrains decompiler
// Type: FollowerTask_FakeLeisure
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerTask_FakeLeisure : FollowerTask
{
  [CompilerGenerated]
  public bool \u003CSearching\u003Ek__BackingField;
  public float _gameTimeToNextStateUpdate;
  public float _searchCooldownGameMinutes;
  public int randomLeisure;
  public Follower follower;
  public Vector3 previousPosition;
  public bool winterLeisure;
  public bool blizzardLeisure;

  public override FollowerTaskType Type => FollowerTaskType.FakeLeisure;

  public override FollowerLocation Location => this._brain.HomeLocation;

  public bool Searching
  {
    get => this.\u003CSearching\u003Ek__BackingField;
    set => this.\u003CSearching\u003Ek__BackingField = value;
  }

  public override bool BlockTaskChanges
  {
    get
    {
      return (UnityEngine.Object) this.follower != (UnityEngine.Object) null && this.follower.State.CURRENT_STATE == StateMachine.State.TimedAction;
    }
  }

  public override int GetSubTaskCode() => 0;

  public override void OnStart()
  {
    this._searchCooldownGameMinutes = UnityEngine.Random.Range(30f, 60f);
    if (SeasonsManager.CurrentWeatherEvent != SeasonsManager.WeatherEvent.None)
      this._searchCooldownGameMinutes = 0.0f;
    this.SetState(FollowerTaskState.GoingTo);
  }

  public override void OnArrive()
  {
    this.SetState(FollowerTaskState.Idle);
    this.ChooseActivity((Follower) null, true);
  }

  public override void TaskTick(float deltaGameTime)
  {
    if (!this.Searching && this.State > FollowerTaskState.WaitingForLocation && (double) (this._searchCooldownGameMinutes -= deltaGameTime) < 0.0)
      this.Searching = true;
    if (!this.TryFindCompanionTask())
    {
      if (this._state == FollowerTaskState.Idle)
      {
        this._gameTimeToNextStateUpdate -= deltaGameTime;
        if ((double) this._gameTimeToNextStateUpdate <= 0.0)
        {
          if ((double) UnityEngine.Random.Range(0.0f, 1f) < 0.5)
            this.Wander();
          this._gameTimeToNextStateUpdate = UnityEngine.Random.Range(20f, 30f);
        }
      }
      else if (this._state == FollowerTaskState.GoingTo && this.Brain.LastPosition == this.previousPosition)
        this.SetState(FollowerTaskState.Idle);
    }
    this.previousPosition = this.Brain.LastPosition;
  }

  public override void OnEnd()
  {
    if (this.winterLeisure)
    {
      switch (this.randomLeisure)
      {
        case 0:
          this.Brain.AddRandomThoughtFromList(Thought.PlayedSnow_1, Thought.PlayedSnow_3);
          break;
        case 1:
        case 2:
        case 3:
        case 4:
          this.Brain.AddRandomThoughtFromList(Thought.PlayedSnow_1, Thought.PlayedSnow_3);
          break;
        case 5:
        case 6:
          if (!this.Brain.HasTrait(FollowerTrait.TraitType.Argumentative))
          {
            this.Brain.AddThought(Thought.PlayedSnow_1);
            break;
          }
          break;
      }
      if (this.randomLeisure == 0)
        this.follower.TimedAnimation("Snow/snow-angel-end", 1.33333337f, (System.Action) (() => base.OnEnd()), false);
      else
        base.OnEnd();
    }
    else if (this.blizzardLeisure)
      base.OnEnd();
    else if (this.randomLeisure == 5)
      this.follower.TimedAnimation("Activities/activity-read-end", 1f, (System.Action) (() => base.OnEnd()), false);
    else if (this.randomLeisure == 7)
      this.follower.TimedAnimation("Activities/activity-sit-end", 1.33333337f, (System.Action) (() => base.OnEnd()), false);
    else
      base.OnEnd();
  }

  public void Wander()
  {
    this.ClearDestination();
    this.SetState(FollowerTaskState.GoingTo);
  }

  public bool TryFindCompanionTask()
  {
    if (this.Searching)
    {
      foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
      {
        if (allBrain != this._brain && allBrain.Location == this.Location)
        {
          if (allBrain.CurrentTaskType == FollowerTaskType.DanceCircleLead && (double) UnityEngine.Random.value < 0.5)
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
          else if (allBrain.CurrentTaskType == FollowerTaskType.HuddleForWarmthLeader && !this.Brain._directInfoAccess.IsSnowman)
          {
            FollowerTask_HuddleForWarmthLeader currentTask = (FollowerTask_HuddleForWarmthLeader) allBrain.CurrentTask;
            if (currentTask.RemainingSlotCount > 0)
            {
              this.InitiateLeisureTask((FollowerTask) new FollowerTask_HuddleForWarmthFollower()
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
              if (SeasonsManager.CurrentWeatherEvent == SeasonsManager.WeatherEvent.Blizzard && !this.Brain._directInfoAccess.IsSnowman && !this.Brain._directInfoAccess.Traits.Contains(FollowerTrait.TraitType.Mutated))
              {
                this.InitiateHuddleForWarmth(allBrain, currentTask);
                return true;
              }
              if (SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter && (double) UnityEngine.Random.value < 0.33000001311302185 && !this.Brain._directInfoAccess.IsSnowman && !this.Brain._directInfoAccess.Traits.Contains(FollowerTrait.TraitType.Mutated))
              {
                this.InitiateHuddleForWarmth(allBrain, currentTask);
                return true;
              }
              float num = UnityEngine.Random.Range(0.0f, 1f);
              if ((double) num < 0.33000001311302185)
              {
                this.InitiateChat(allBrain, currentTask);
                return true;
              }
              if ((double) num < 0.6600000262260437)
              {
                this.InitiateDanceCircle(allBrain, currentTask);
                return true;
              }
            }
          }
        }
      }
    }
    return false;
  }

  public void InitiateHuddleForWarmth(
    FollowerBrain otherBrain,
    FollowerTask_FakeLeisure otherLeisure)
  {
    FollowerTask_HuddleForWarmthLeader leisureTask1 = new FollowerTask_HuddleForWarmthLeader();
    FollowerTask_HuddleForWarmthFollower leisureTask2 = new FollowerTask_HuddleForWarmthFollower();
    List<StructureBrain> structuresOfType = StructureManager.GetAllStructuresOfType<StructureBrain>();
    structuresOfType.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType<Structures_Furnace>());
    if (structuresOfType.Count > 0)
    {
      structuresOfType.Shuffle<StructureBrain>();
      StructureBrain structureBrain = (StructureBrain) null;
      for (int index = 0; index < structuresOfType.Count; ++index)
      {
        if (structuresOfType[index].Data.Fuel > 0)
          structureBrain = structuresOfType[index];
      }
      if (structureBrain != null)
        leisureTask1 = new FollowerTask_HuddleForWarmthLeader(180f, structureBrain.Data.Position);
    }
    leisureTask2.LeadTask = leisureTask1;
    this.InitiateLeisureTask((FollowerTask) leisureTask1);
    otherLeisure.InitiateLeisureTask((FollowerTask) leisureTask2);
  }

  public void InitiateChat(FollowerBrain otherBrain, FollowerTask_FakeLeisure otherLeisure)
  {
    FollowerTask_Chat leisureTask1 = new FollowerTask_Chat(otherBrain.Info.ID, true);
    FollowerTask_Chat leisureTask2 = new FollowerTask_Chat(this._brain.Info.ID, false);
    leisureTask1.OtherChatTask = leisureTask2;
    leisureTask2.OtherChatTask = leisureTask1;
    otherLeisure.InitiateLeisureTask((FollowerTask) leisureTask2);
    this.InitiateLeisureTask((FollowerTask) leisureTask1);
  }

  public void InitiateDanceCircle(FollowerBrain otherBrain, FollowerTask_FakeLeisure otherLeisure)
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
    if (this.Brain.CurrentTaskType == FollowerTaskType.ManualControl || this.Brain.CurrentTaskType == FollowerTaskType.EnforcerManualControl)
      return;
    this._brain.TransitionToTask(leisureTask);
  }

  public void UndoStateAnimationChanges(Follower follower)
  {
    SimpleSpineAnimator.SpineChartacterAnimationData animationData = follower.SimpleAnimator.GetAnimationData(StateMachine.State.Idle);
    animationData.Animation = animationData.DefaultAnimation;
    follower.SimpleAnimator.ResetAnimationsToDefaults();
    follower.ResetStateAnimations();
  }

  public override void OnAbort()
  {
    base.OnAbort();
    if (!((UnityEngine.Object) this.follower != (UnityEngine.Object) null))
      return;
    SimpleSpineAnimator.SpineChartacterAnimationData animationData = this.follower.SimpleAnimator.GetAnimationData(StateMachine.State.Idle);
    animationData.Animation = animationData.DefaultAnimation;
    this.follower.ResetStateAnimations();
    if (this.winterLeisure)
    {
      if (this.randomLeisure != 0)
        return;
      this.follower.TimedAnimation("Snow/snow-angel-end", 1.33333337f, Loop: false);
    }
    else
    {
      if (this.blizzardLeisure)
        return;
      if (this.randomLeisure == 5)
      {
        this.follower.TimedAnimation("Activities/activity-read-end", 1f, Loop: false);
      }
      else
      {
        if (this.randomLeisure != 7)
          return;
        this.follower.TimedAnimation("Activities/activity-sit-end", 1.33333337f, Loop: false);
      }
    }
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    return TownCentre.RandomPositionInCachedTownCentre();
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    this.ChooseActivity(follower, false);
  }

  public void ChooseActivity(Follower follower, bool animate)
  {
    if ((UnityEngine.Object) follower == (UnityEngine.Object) null)
      follower = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    if ((UnityEngine.Object) follower == (UnityEngine.Object) null)
      return;
    this.follower = follower;
    this.randomLeisure = UnityEngine.Random.Range(0, 9);
    if (SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter && SeasonsManager.CurrentWeatherEvent == SeasonsManager.WeatherEvent.Blizzard)
    {
      this.blizzardLeisure = true;
      switch (this.randomLeisure)
      {
        case 0:
        case 1:
        case 2:
        case 3:
          follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Snow/blow-on-hands");
          break;
        case 4:
        case 5:
        case 6:
        case 7:
        case 8:
          follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Snow/shuffle");
          break;
      }
    }
    else if (SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter)
    {
      this.winterLeisure = true;
      switch (this.randomLeisure)
      {
        case 0:
          if (animate)
            follower.TimedAnimation("Snow/snow-angel-start", 1.66666663f, Loop: false);
          follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Snow/snow-angel");
          break;
        case 1:
        case 2:
          follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Snow/idle-tongueout");
          break;
        case 3:
        case 4:
          follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Snow/idle-tongueout2");
          break;
        case 5:
        case 6:
          if (this.Brain.HasTrait(FollowerTrait.TraitType.Argumentative))
          {
            follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Snow/kick-snow-angry");
            break;
          }
          follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Snow/kick-snow-playful");
          break;
        case 7:
          follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Snow/sneeze");
          break;
        case 8:
          follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Snow/idle-tongueout");
          break;
      }
    }
    else
    {
      switch (this.randomLeisure)
      {
        case 0:
        case 1:
        case 6:
          follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "meditate");
          break;
        case 2:
          follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Activities/activity-stretch");
          break;
        case 3:
          follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Activities/activity-sunbathing");
          break;
        case 4:
          follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Activities/activity-aerobics");
          break;
        case 5:
          if (animate)
            follower.TimedAnimation("Activities/activity-read-start", 1f, Loop: false);
          follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Activities/activity-read");
          break;
        case 7:
          if (animate)
            follower.TimedAnimation("Activities/activity-sit", 1.66666663f, Loop: false);
          follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Activities/activity-sit-idle");
          break;
        case 8:
          follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "meditate");
          break;
      }
    }
  }

  public override void Cleanup(Follower follower)
  {
    this.UndoStateAnimationChanges(follower);
    if ((bool) (UnityEngine.Object) follower && (bool) (UnityEngine.Object) follower.GetComponent<Interaction_BackToWork>())
      UnityEngine.Object.Destroy((UnityEngine.Object) follower.GetComponent<Interaction_BackToWork>());
    base.Cleanup(follower);
  }

  [CompilerGenerated]
  public void \u003COnEnd\u003Eb__21_0() => base.OnEnd();

  [CompilerGenerated]
  public void \u003COnEnd\u003Eb__21_1() => base.OnEnd();

  [CompilerGenerated]
  public void \u003COnEnd\u003Eb__21_2() => base.OnEnd();
}
