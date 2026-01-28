// Decompiled with JetBrains decompiler
// Type: FollowerTask_Flute
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using UnityEngine;

#nullable disable
public class FollowerTask_Flute : FollowerTask
{
  public EventInstance loopSfx;

  public override FollowerTaskType Type => FollowerTaskType.Flute;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override void OnStart()
  {
    this.SetState(FollowerTaskState.GoingTo);
    if ((double) Random.value >= 0.30000001192092896)
      return;
    this.AddInspiredThought();
  }

  public override int GetSubTaskCode() => 0;

  public override void TaskTick(float deltaGameTime)
  {
    if (this.State == FollowerTaskState.Doing)
      this.Brain._directInfoAccess.PoemProgress += deltaGameTime;
    if (PlayerFarming.Location == FollowerLocation.Base)
      return;
    this.End();
    AudioManager.Instance.StopLoop(this.loopSfx);
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(0.0f, -1f));
    LayerMask layerMask = (LayerMask) ((int) new LayerMask() | 1 << LayerMask.NameToLayer("Island"));
    return (Vector3) (Physics2D.Raycast((Vector2) Vector3.zero, direction, 1000f, (int) layerMask).point + -direction * Random.Range(2f, 4f));
  }

  public override void OnDoingBegin(Follower follower)
  {
    base.OnDoingBegin(follower);
    double num = (double) follower.SetBodyAnimation("Activities/activity-flute-start", false);
    follower.AddBodyAnimation("Activities/activity-flute", true, 0.0f);
    this.loopSfx = AudioManager.Instance.CreateLoop("event:/dlc/env/seaice/riverboy_flute_base_loop", follower.gameObject, true);
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Activities/activity-flute");
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    follower.SimpleAnimator.ResetAnimationsToDefaults();
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "idle");
    AudioManager.Instance.StopLoop(this.loopSfx);
  }

  public void AddInspiredThought()
  {
    Thought randomThoughtFromSet = FollowerThoughts.GetRandomThoughtFromSet(Thought.Inspired_0, Thought.Inspired_1, Thought.Inspired_2, Thought.Inspired_3, Thought.Inspired_4);
    if (this._brain.HasThought(randomThoughtFromSet))
      return;
    this._brain.AddThought(randomThoughtFromSet);
  }
}
