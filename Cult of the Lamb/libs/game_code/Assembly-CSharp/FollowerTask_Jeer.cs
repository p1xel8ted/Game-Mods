// Decompiled with JetBrains decompiler
// Type: FollowerTask_Jeer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using Spine;
using UnityEngine;

#nullable disable
public class FollowerTask_Jeer : FollowerTask
{
  public Follower follower;
  public bool interrupted;
  public float progress;
  public EventInstance loop;

  public override FollowerTaskType Type => FollowerTaskType.ReactOuthouse;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockTaskChanges => true;

  public override bool BlockReactTasks => true;

  public override bool BlockSocial => true;

  public override int UsingStructureID => 0;

  public override int GetSubTaskCode() => 0;

  public override void OnStart() => this.SetState(FollowerTaskState.Doing);

  public override void TaskTick(float deltaGameTime)
  {
    if (this.State != FollowerTaskState.Doing)
      return;
    if (!this.interrupted)
    {
      if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null && this.Brain != null && (double) Vector3.Distance(this.Brain.LastPosition, PlayerFarming.Instance.transform.position) < 2.0)
      {
        AudioManager.Instance.StopLoop(this.loop);
        this.interrupted = true;
        this.follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
        double num = (double) this.follower.SetBodyAnimation("Jeer/jeer-act-vague", false);
      }
      if (!(bool) (UnityEngine.Object) this.follower)
        return;
      this.follower.FacePosition(PlayerFarming.Instance.transform.position);
    }
    else
    {
      this.progress += deltaGameTime;
      if ((double) this.progress <= 2.7999999523162842)
        return;
      this.End();
    }
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    this.follower = follower;
    follower.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    follower.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
    AudioManager.Instance.StopLoop(this.loop);
  }

  public override void OnEnd()
  {
    base.OnEnd();
    AudioManager.Instance.StopLoop(this.loop);
  }

  public void HandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
  {
    switch (e.Data.Name)
    {
      case "Audio/Idle_Sing Fol":
        AudioManager.Instance.PlayOneShot("event:/dialogue/followers/idle_sing", this.follower.gameObject);
        break;
      case "Audio/plotting_laugh Fol":
        AudioManager.Instance.PlayOneShot("event:/dialogue/followers/plotting_laugh", this.follower.gameObject);
        break;
      case "Audio/jeer Fol":
        AudioManager.Instance.PlayOneShot("event:/dialogue/followers/jeer", this.follower.gameObject);
        break;
      case "Audio/tongue_waggle  Fol":
        AudioManager.Instance.PlayOneShot("event:/dialogue/followers/tongue_waggle", this.follower.gameObject);
        break;
    }
  }

  public override Vector3 UpdateDestination(Follower follower) => follower.transform.position;

  public override void OnDoingBegin(Follower follower)
  {
    follower.TimedAnimation("Jeer/jeer-plotting" + ((double) UnityEngine.Random.value < 0.5 ? "2" : ""), 1.93333328f, (System.Action) (() =>
    {
      if (!((UnityEngine.Object) follower != (UnityEngine.Object) null))
        return;
      if ((double) UnityEngine.Random.value < 0.5)
      {
        AudioManager.Instance.PlayOneShot("event:/dialogue/followers/tease", follower.gameObject);
        follower.TimedAnimation("Jeer/jeer-at-player2", 3f, (System.Action) (() =>
        {
          if (this.interrupted)
            return;
          this.End();
        }));
      }
      else
        follower.TimedAnimation("Jeer/jeer-at-player", 3.86666656f, (System.Action) (() =>
        {
          if (this.interrupted)
            return;
          this.End();
        }));
    }));
  }
}
