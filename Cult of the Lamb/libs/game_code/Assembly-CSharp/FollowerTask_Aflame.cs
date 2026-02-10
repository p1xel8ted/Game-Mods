// Decompiled with JetBrains decompiler
// Type: FollowerTask_Aflame
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
public class FollowerTask_Aflame : FollowerTask
{
  public const float IDLE_DURATION_GAME_MINUTES_MIN = 1f;
  public const float IDLE_DURATION_GAME_MINUTES_MAX = 2f;
  public float _gameTimeToNextStateUpdate;
  public GameObject flame;

  public override FollowerTaskType Type => FollowerTaskType.Aflame;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockReactTasks => true;

  public override bool BlockSermon => true;

  public override int GetSubTaskCode() => 0;

  public override void TaskTick(float deltaGameTime)
  {
    if (this._state != FollowerTaskState.Idle && this._state != FollowerTaskState.Doing)
      return;
    this._gameTimeToNextStateUpdate -= deltaGameTime;
    if ((double) this._gameTimeToNextStateUpdate > 0.0)
      return;
    this.ClearDestination();
    this.SetState(FollowerTaskState.GoingTo);
    this._gameTimeToNextStateUpdate = UnityEngine.Random.Range(1f, 2f);
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Insane/idle-insane");
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Insane/run-insane");
    follower.GetComponent<interaction_FollowerInteraction>().enabled = false;
    follower.gameObject.AddComponent<Interaction_ExtinguishFollower>().OutlineTarget = follower.GetComponent<interaction_FollowerInteraction>().OutlineTarget;
    if (!((UnityEngine.Object) this.flame == (UnityEngine.Object) null))
      return;
    Addressables.InstantiateAsync((object) "Assets/Prefabs/Base/Structure_Fire.prefab", follower.transform).Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      obj.Result.transform.localPosition = Vector3.zero;
      obj.Result.transform.localScale = Vector3.zero;
      obj.Result.transform.DOScale(2f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBounce);
      this.flame = obj.Result;
    });
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    return TownCentre.RandomCircleFromTownCentre(16f);
  }

  public override float RestChange(float deltaGameTime) => 100f;

  [CompilerGenerated]
  public void \u003CSetup\u003Eb__14_0(AsyncOperationHandle<GameObject> obj)
  {
    obj.Result.transform.localPosition = Vector3.zero;
    obj.Result.transform.localScale = Vector3.zero;
    obj.Result.transform.DOScale(2f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBounce);
    this.flame = obj.Result;
  }
}
