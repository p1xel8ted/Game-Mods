// Decompiled with JetBrains decompiler
// Type: FollowerTask_SnowballFight
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class FollowerTask_SnowballFight : FollowerTask
{
  public FollowerTask_SnowballFight.ConvoStage _convoStage;
  public float _doingTimer;
  public Follower follower;
  public Follower targetFollower;
  public Vector3 targetPos;
  public bool instant;
  public bool angry;
  public bool ignoreLockedFollower;
  public string snowballImpactUnit = "event:/dlc/material/snowball_impact_follower";

  public override FollowerTaskType Type => FollowerTaskType.SnowballFight;

  public override FollowerLocation Location => this._brain.Location;

  public override bool BlockTaskChanges => true;

  public override bool BlockReactTasks => true;

  public FollowerTask_SnowballFight()
  {
  }

  public FollowerTask_SnowballFight(Vector3 targetPosition, bool instant, bool angry = true)
  {
    this.angry = angry;
    this.instant = instant;
    this.targetPos = targetPosition;
  }

  public FollowerTask_SnowballFight(Follower targetFollower, bool instant)
  {
    this.instant = instant;
    this.targetFollower = targetFollower;
    this.ignoreLockedFollower = true;
  }

  public override int GetSubTaskCode() => this.Brain.Info.ID;

  public override void OnStart()
  {
    if (FollowerBrainStats.ShouldWork && this._brain.CanWork)
    {
      Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
      if ((bool) (UnityEngine.Object) followerById && (UnityEngine.Object) followerById.gameObject.GetComponent<Interaction_BackToWork>() == (UnityEngine.Object) null)
      {
        Interaction_BackToWork interactionBackToWork = followerById.gameObject.AddComponent<Interaction_BackToWork>();
        interactionBackToWork.Init(followerById);
        interactionBackToWork.LockPosition = followerById.transform;
      }
    }
    this.SetState(FollowerTaskState.Doing);
    this._doingTimer = 0.0f;
  }

  public override void OnArrive()
  {
    base.OnArrive();
    this.SetState(FollowerTaskState.Doing);
    this._doingTimer = 0.0f;
  }

  public override void OnEnd()
  {
    base.OnEnd();
    this.Brain.AddRandomThoughtFromList(Thought.PlayedSnow_1, Thought.PlayedSnow_2);
  }

  public override void TaskTick(float deltaGameTime)
  {
    if (this.State != FollowerTaskState.Doing || (double) (this._doingTimer -= deltaGameTime) > 0.0)
      return;
    this.UpdateState();
  }

  public void UpdateState()
  {
    if ((UnityEngine.Object) this.follower == (UnityEngine.Object) null && !this.instant)
      this.End();
    else if (PlayerFarming.Location != FollowerLocation.Base && !this.instant)
    {
      this.End();
    }
    else
    {
      ++this._convoStage;
      this.follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
      if (this._convoStage == FollowerTask_SnowballFight.ConvoStage.FindTarget && this.instant)
        ++this._convoStage;
      else if (this._convoStage >= FollowerTask_SnowballFight.ConvoStage.Response && this.instant)
      {
        this.End();
        return;
      }
      switch (this._convoStage)
      {
        case FollowerTask_SnowballFight.ConvoStage.Make:
          double num1 = (double) this.follower.SetBodyAnimation(this.GetMakeAnimation(), false);
          break;
        case FollowerTask_SnowballFight.ConvoStage.Idle:
          double num2 = (double) this.follower.SetBodyAnimation(this.GetIdleAnimation(), true);
          break;
        case FollowerTask_SnowballFight.ConvoStage.FindTarget:
          if (Follower.Followers.Count > 0)
          {
            this.targetFollower = Follower.Followers[0];
            foreach (Follower follower in Follower.Followers)
            {
              if (!FollowerManager.FollowerLocked(follower.Brain.Info.ID) && (follower.Brain.CurrentTask == null || !follower.Brain.CurrentTask.BlockSocial) && ((UnityEngine.Object) this.targetFollower == (UnityEngine.Object) this.follower || (double) Vector3.Distance(this.follower.transform.position, follower.transform.position) < (double) Vector3.Distance(this.targetFollower.transform.position, this.follower.transform.position)))
                this.targetFollower = follower;
            }
          }
          if (Follower.Followers.Count <= 0 || (UnityEngine.Object) this.targetFollower == (UnityEngine.Object) this.follower)
          {
            this.End();
            return;
          }
          this.follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, this.GetRunAnimation());
          this.SetState(FollowerTaskState.GoingTo);
          break;
        case FollowerTask_SnowballFight.ConvoStage.Throw:
          double num3 = (double) this.follower.SetBodyAnimation(this.GetThrowAnimation(), false);
          if (this.instant && (UnityEngine.Object) this.targetFollower == (UnityEngine.Object) null)
          {
            this.follower.FacePosition(this.targetPos);
            GameManager.GetInstance().WaitForSeconds(0.766666651f, (System.Action) (() => this.ThrowSnowball()));
            break;
          }
          if ((UnityEngine.Object) this.targetFollower != (UnityEngine.Object) null && (!FollowerManager.FollowerLocked(this.targetFollower.Brain.Info.ID) || this.ignoreLockedFollower) && (this.targetFollower.Brain.CurrentTask == null || !this.targetFollower.Brain.CurrentTask.BlockSocial || this.ignoreLockedFollower))
          {
            this.follower.FacePosition(this.targetFollower.transform.position);
            if (this.targetFollower.State.CURRENT_STATE != StateMachine.State.CustomAnimation)
              this.targetFollower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
            GameManager.GetInstance().WaitForSeconds(0.766666651f, (System.Action) (() => this.ThrowSnowball()));
            GameManager.GetInstance().WaitForSeconds(0.9166666f, (System.Action) (() =>
            {
              this.targetFollower.SnowballHitFX.Play();
              AudioManager.Instance.PlayOneShot(this.snowballImpactUnit, this.targetFollower.transform.position);
              if (this.targetFollower.State.CURRENT_STATE == StateMachine.State.CustomAnimation)
                return;
              this.targetFollower.TimedAnimation(this.GetHitAnimation(), 2.33333325f, (System.Action) (() =>
              {
                float num5 = UnityEngine.Random.value;
                FollowerTask currentTask = this.targetFollower.Brain.CurrentTask;
                if ((currentTask != null ? (currentTask.BlockTaskChanges ? 1 : 0) : 0) == 0 && (double) num5 < 0.20000000298023224)
                  this.targetFollower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_SnowballFight());
                else
                  this.targetFollower.Brain.CompleteCurrentTask();
              }));
            }));
            break;
          }
          this.End();
          break;
        case FollowerTask_SnowballFight.ConvoStage.Response:
          double num6 = (double) this.follower.SetBodyAnimation(this.GetResponseAnimation(), false);
          break;
        case FollowerTask_SnowballFight.ConvoStage.Finished:
          this.End();
          break;
      }
      this._doingTimer = this.ConvertAnimTimeToGameTime(this.follower.SimpleAnimator.Duration());
    }
  }

  public void UndoStateAnimationChanges(Follower follower)
  {
    SimpleSpineAnimator.SpineChartacterAnimationData animationData = follower.SimpleAnimator.GetAnimationData(StateMachine.State.Moving);
    animationData.Animation = animationData.DefaultAnimation;
    follower.ResetStateAnimations();
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    return (UnityEngine.Object) this.targetFollower != (UnityEngine.Object) null ? this.targetFollower.transform.position - (this.targetFollower.transform.position - follower.transform.position).normalized * 2f : follower.transform.position;
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    this.follower = follower;
  }

  public override void Cleanup(Follower follower)
  {
    if ((bool) (UnityEngine.Object) follower.GetComponent<Interaction_BackToWork>())
      UnityEngine.Object.Destroy((UnityEngine.Object) follower.GetComponent<Interaction_BackToWork>());
    if ((UnityEngine.Object) this.targetFollower != (UnityEngine.Object) null && this.targetFollower.Brain.CurrentTaskType == FollowerTaskType.ManualControl)
      this.targetFollower.Brain.CompleteCurrentTask();
    this.UndoStateAnimationChanges(follower);
  }

  public override void OnAbort()
  {
    base.OnAbort();
    if (!((UnityEngine.Object) this.targetFollower != (UnityEngine.Object) null) || this.targetFollower.Brain.CurrentTaskType != FollowerTaskType.ManualControl)
      return;
    this.targetFollower.Brain.CompleteCurrentTask();
  }

  public override void OnComplete()
  {
    base.OnComplete();
    if (!((UnityEngine.Object) this.targetFollower != (UnityEngine.Object) null) || this.targetFollower.Brain.CurrentTaskType != FollowerTaskType.ManualControl)
      return;
    this.targetFollower.Brain.CompleteCurrentTask();
  }

  public float ConvertAnimTimeToGameTime(float duration) => duration * 2f;

  public string GetHitAnimation()
  {
    return this.follower.SimpleAnimator.Dir == this.targetFollower.SimpleAnimator.Dir ? "Snow/hit-back-fall" : "Snow/hit-front-fall";
  }

  public string GetResponseAnimation() => "Reactions/react-laugh";

  public string GetThrowAnimation()
  {
    string str = "Snow/snowball-throw-";
    return !this.Brain.HasTrait(FollowerTrait.TraitType.Bastard) ? (this.Brain.HasTrait(FollowerTrait.TraitType.Argumentative) || this.angry ? str + "revengeful" : str + "playful") : str + "mischievous";
  }

  public string GetRunAnimation()
  {
    string str = "Snow/snowball-run-";
    return !this.Brain.HasTrait(FollowerTrait.TraitType.Bastard) ? (!this.Brain.HasTrait(FollowerTrait.TraitType.Argumentative) ? str + "playful" : str + "revengeful") : str + "mischievous";
  }

  public string GetIdleAnimation()
  {
    string str = "Snow/snowball-idle-";
    return !this.Brain.HasTrait(FollowerTrait.TraitType.Bastard) ? (this.Brain.HasTrait(FollowerTrait.TraitType.Argumentative) || this.angry ? str + "revengeful" : str + "playful") : str + "mischievous";
  }

  public string GetMakeAnimation()
  {
    string str = "Snow/snowball-make-";
    return !this.Brain.HasTrait(FollowerTrait.TraitType.Bastard) ? (this.Brain.HasTrait(FollowerTrait.TraitType.Argumentative) || this.angry ? str + "revengeful" : str + "playful") : str + "mischievous";
  }

  public void ThrowSnowball()
  {
    Addressables.InstantiateAsync((object) "Assets/Prefabs/Enemies/Weapons/ArrowSnowballSmall.prefab").Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      Projectile arrow = obj.Result.GetComponent<Projectile>();
      arrow.enabled = false;
      arrow.transform.position = this.follower.transform.position;
      arrow.transform.DOMove((UnityEngine.Object) this.targetFollower != (UnityEngine.Object) null ? this.targetFollower.transform.position : this.targetPos, 0.15f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
      {
        if (PlayerFarming.Location == FollowerLocation.Base)
          BiomeConstants.Instance.EmitSnowImpactVFX(arrow.transform.position);
        UnityEngine.Object.Destroy((UnityEngine.Object) arrow.gameObject);
      }));
    });
  }

  public float HateThreshold => -10f;

  public float FriendThreshold => 5f;

  public float LoveThreshold => 10f;

  [CompilerGenerated]
  public void \u003CUpdateState\u003Eb__26_0() => this.ThrowSnowball();

  [CompilerGenerated]
  public void \u003CUpdateState\u003Eb__26_1() => this.ThrowSnowball();

  [CompilerGenerated]
  public void \u003CUpdateState\u003Eb__26_2()
  {
    this.targetFollower.SnowballHitFX.Play();
    AudioManager.Instance.PlayOneShot(this.snowballImpactUnit, this.targetFollower.transform.position);
    if (this.targetFollower.State.CURRENT_STATE == StateMachine.State.CustomAnimation)
      return;
    this.targetFollower.TimedAnimation(this.GetHitAnimation(), 2.33333325f, (System.Action) (() =>
    {
      float num = UnityEngine.Random.value;
      FollowerTask currentTask = this.targetFollower.Brain.CurrentTask;
      if ((currentTask != null ? (currentTask.BlockTaskChanges ? 1 : 0) : 0) == 0 && (double) num < 0.20000000298023224)
        this.targetFollower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_SnowballFight());
      else
        this.targetFollower.Brain.CompleteCurrentTask();
    }));
  }

  [CompilerGenerated]
  public void \u003CUpdateState\u003Eb__26_3()
  {
    float num = UnityEngine.Random.value;
    FollowerTask currentTask = this.targetFollower.Brain.CurrentTask;
    if ((currentTask != null ? (currentTask.BlockTaskChanges ? 1 : 0) : 0) == 0 && (double) num < 0.20000000298023224)
      this.targetFollower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_SnowballFight());
    else
      this.targetFollower.Brain.CompleteCurrentTask();
  }

  [CompilerGenerated]
  public void \u003CThrowSnowball\u003Eb__40_0(AsyncOperationHandle<GameObject> obj)
  {
    Projectile arrow = obj.Result.GetComponent<Projectile>();
    arrow.enabled = false;
    arrow.transform.position = this.follower.transform.position;
    arrow.transform.DOMove((UnityEngine.Object) this.targetFollower != (UnityEngine.Object) null ? this.targetFollower.transform.position : this.targetPos, 0.15f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      if (PlayerFarming.Location == FollowerLocation.Base)
        BiomeConstants.Instance.EmitSnowImpactVFX(arrow.transform.position);
      UnityEngine.Object.Destroy((UnityEngine.Object) arrow.gameObject);
    }));
  }

  public enum ConvoStage
  {
    None,
    Make,
    Idle,
    FindTarget,
    Throw,
    Response,
    Finished,
  }
}
