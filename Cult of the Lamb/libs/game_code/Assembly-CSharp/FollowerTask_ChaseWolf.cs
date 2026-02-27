// Decompiled with JetBrains decompiler
// Type: FollowerTask_ChaseWolf
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerTask_ChaseWolf : FollowerTask
{
  public const float WOLF_FLEE_DISTANCE = 1f;
  public Follower follower;
  public Interaction_WolfBase wolftoChase;
  public float updateDestination;
  public bool isSuccessChase;

  public override FollowerTaskType Type => FollowerTaskType.ChaseWolf;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockTaskChanges => true;

  public override bool BlockReactTasks => true;

  public override bool BlockSocial => true;

  public FollowerTask_ChaseWolf(Interaction_WolfBase wolf) => this.wolftoChase = wolf;

  public override int GetSubTaskCode() => 0;

  public override Vector3 UpdateDestination(Follower follower)
  {
    return (UnityEngine.Object) this.wolftoChase != (UnityEngine.Object) null ? this.wolftoChase.transform.position : Vector3.zero;
  }

  public override void TaskTick(float deltaGameTime)
  {
    if (((UnityEngine.Object) this.wolftoChase == (UnityEngine.Object) null || this.wolftoChase.CurrentState == Interaction_WolfBase.State.Fleeing) && this.State != FollowerTaskState.Finalising)
      this.End();
    else if ((UnityEngine.Object) this.follower != (UnityEngine.Object) null && !this.isSuccessChase && (double) Vector3.Distance(this.follower.transform.position, this.wolftoChase.transform.position) <= 1.0)
    {
      AudioManager.Instance.PlayOneShot("event:/dlc/env/dog/spank", this.wolftoChase.transform.position);
      this.wolftoChase.Flee();
      this.wolftoChase.SetScared();
      this.isSuccessChase = true;
      this.End();
    }
    else
    {
      this.updateDestination -= deltaGameTime;
      if ((double) this.updateDestination > 0.0)
        return;
      this.RecalculateDestination();
      this.updateDestination = 1.5f;
    }
  }

  public override void OnEnd()
  {
    if (this.isSuccessChase && (UnityEngine.Object) this.follower != (UnityEngine.Object) null)
    {
      if (this.follower.State.CURRENT_STATE == StateMachine.State.TimedAction)
        return;
      this.follower.TimedAnimationWithDuration("Reactions/react-happy1", (System.Action) (() =>
      {
        if ((double) UnityEngine.Random.value < 0.34999999403953552)
          this.follower.Brain.MakeExhausted();
        base.OnEnd();
      }), false);
    }
    else
      base.OnEnd();
  }

  public override void OnFinaliseBegin(Follower follower)
  {
    base.OnFinaliseBegin(follower);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run");
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    this.follower = follower;
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Riot/riot-purge-run3");
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run");
  }

  [CompilerGenerated]
  public void \u003COnEnd\u003Eb__19_0()
  {
    if ((double) UnityEngine.Random.value < 0.34999999403953552)
      this.follower.Brain.MakeExhausted();
    base.OnEnd();
  }
}
