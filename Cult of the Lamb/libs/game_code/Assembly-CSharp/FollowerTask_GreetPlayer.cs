// Decompiled with JetBrains decompiler
// Type: FollowerTask_GreetPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class FollowerTask_GreetPlayer : FollowerTask
{
  public static int MAX_GREETERS = 3;
  public int _slotIndex;
  public Coroutine _greetCoroutine;

  public override FollowerTaskType Type => FollowerTaskType.GreetPlayer;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockTaskChanges => true;

  public override int GetSubTaskCode() => 0;

  public FollowerTask_GreetPlayer(int slotIndex) => this._slotIndex = slotIndex;

  public override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  public override void OnArrive() => this.SetState(FollowerTaskState.Doing);

  public override void TaskTick(float deltaGameTime)
  {
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    Vector3 position = BiomeBaseManager.Instance.PlayerGreetLocation.transform.position;
    float num = 1.5f;
    float f = (float) ((double) this._slotIndex * (-180.0 / (double) (FollowerTask_GreetPlayer.MAX_GREETERS - 1)) * (Math.PI / 180.0));
    Vector3 vector3 = new Vector3(num * Mathf.Cos(f), num * Mathf.Sin(f));
    return position + vector3;
  }

  public override void OnGoingToBegin(Follower follower)
  {
    follower.transform.position = Vector3.Lerp(follower.transform.position, this.GetDestination(follower), 0.8f);
    base.OnGoingToBegin(follower);
  }

  public override void OnDoingBegin(Follower follower)
  {
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "pray");
    this._greetCoroutine = follower.StartCoroutine((IEnumerator) this.WaitForGreetCoroutine(follower));
  }

  public override void Cleanup(Follower follower)
  {
    if (this._greetCoroutine == null)
      return;
    follower.StopCoroutine(this._greetCoroutine);
    this._greetCoroutine = (Coroutine) null;
  }

  public IEnumerator WaitForGreetCoroutine(Follower follower)
  {
    FollowerTask_GreetPlayer followerTaskGreetPlayer = this;
    while ((double) Vector3.Distance(follower.transform.position, PlayerFarming.Instance.transform.position) > 2.5)
      yield return (object) null;
    double num = (double) follower.SetBodyAnimation("cheer", false);
    yield return (object) new WaitForSeconds(3f);
    followerTaskGreetPlayer._greetCoroutine = (Coroutine) null;
    followerTaskGreetPlayer.End();
  }

  public override void SimSetup(SimFollower simFollower)
  {
    throw new InvalidOperationException("FollowerTask_GreetPlayer is not compatible with SimFollower");
  }
}
