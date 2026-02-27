// Decompiled with JetBrains decompiler
// Type: FollowerTask_Floating
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerTask_Floating : FollowerTask
{
  public float _gameTimeToNextStateUpdate;
  public float timeProgress;
  public const float timeBetweenPleasure = 24f;
  public Follower follower;
  public EventInstance loopy;
  public bool CrownAnimate;

  public override FollowerTaskType Type => FollowerTaskType.Floating;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockReactTasks => true;

  public override bool BlockSermon => true;

  public override bool BlockSocial => true;

  public override bool BlockTaskChanges => true;

  public override bool BlockThoughts => true;

  public PlayerFarming PlayerToFollow
  {
    get
    {
      if ((UnityEngine.Object) this.follower != (UnityEngine.Object) null && this.follower.gameObject.activeInHierarchy)
      {
        PlayerFarming closestPlayer = PlayerFarming.FindClosestPlayer(this.follower.transform.position, true, true);
        if ((UnityEngine.Object) closestPlayer != (UnityEngine.Object) null)
          return closestPlayer;
      }
      return PlayerFarming.Instance;
    }
  }

  public override int GetSubTaskCode() => 0;

  public override void OnStart()
  {
    this.SetState(FollowerTaskState.Idle);
    this.follower.GetComponentInChildren<FollowerPleasureUI>()?.HideCompleted();
    this.follower.Interaction_FollowerInteraction.Interactable = false;
    this.follower.HideAllFollowerIcons();
    this.follower.TimedAnimationWithDuration("Sin/sin-start", (System.Action) (() =>
    {
      this.follower.UsePathing = false;
      this.follower.Interaction_FollowerInteraction.Interactable = true;
    }), false, NextAnim: "Sin/sin-floating");
    this.loopy = AudioManager.Instance.CreateLoop("event:/dialogue/followers/possessed/sinned", this.follower.gameObject, true);
  }

  public override void TaskTick(float deltaGameTime)
  {
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if ((double) Vector2.Distance((Vector2) player.transform.position, (Vector2) this.follower.transform.position) < 5.0)
        this.CrownAnim(player);
      else
        this.ClearCrownAnim(player);
    }
    if (PlayerFarming.Location == FollowerLocation.Base)
      return;
    this.End();
  }

  public override void OnAbort()
  {
    base.OnAbort();
    this.follower.ClearPath();
    AudioManager.Instance.StopLoop(this.loopy);
  }

  public override void OnEnd()
  {
    base.OnEnd();
    this.follower.ClearPath();
    AudioManager.Instance.StopLoop(this.loopy);
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    PlayerFarming playerToFollow = this.PlayerToFollow;
    return (UnityEngine.Object) playerToFollow != (UnityEngine.Object) null ? playerToFollow.transform.position : TownCentre.RandomCircleFromTownCentre(3f);
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    this.follower = follower;
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Sin/sin-floating");
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Sin/sin-floating");
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    double num = (double) follower.SetBodyAnimation("Sin/sin-floating", true);
    follower.UsePathing = false;
    follower.Interaction_FollowerInteraction.Interactable = true;
    follower.Brain.CheckChangeState();
    this.SetState(FollowerTaskState.Idle);
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    follower.LockToGround = true;
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "idle");
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run");
    follower.UsePathing = true;
    follower.ShowAllFollowerIcons();
    follower.Interaction_FollowerInteraction.Interactable = true;
    foreach (PlayerFarming player in PlayerFarming.players)
      this.ClearCrownAnim(player);
    follower.UpdateOutfit();
    follower.SetEmotionAnimation();
    AudioManager.Instance.StopLoop(this.loopy);
  }

  public void CrownAnim(PlayerFarming player)
  {
  }

  public void ClearCrownAnim(PlayerFarming player)
  {
  }

  [CompilerGenerated]
  public void \u003COnStart\u003Eb__22_0()
  {
    this.follower.UsePathing = false;
    this.follower.Interaction_FollowerInteraction.Interactable = true;
  }
}
