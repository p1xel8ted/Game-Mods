// Decompiled with JetBrains decompiler
// Type: FollowerTask_Poet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerTask_Poet : FollowerTask
{
  public override FollowerTaskType Type => FollowerTaskType.Poet;

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
    if (this.State != FollowerTaskState.Doing)
      return;
    this.Brain._directInfoAccess.PoemProgress += deltaGameTime;
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
    double num = (double) follower.SetBodyAnimation("Activities/activity-read-start", false);
    follower.AddBodyAnimation("Activities/activity-poet", true, 0.0f);
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Activities/activity-poet");
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    follower.SimpleAnimator.ResetAnimationsToDefaults();
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "idle");
  }

  public void AddInspiredThought()
  {
    Thought randomThoughtFromSet = FollowerThoughts.GetRandomThoughtFromSet(Thought.Inspired_0, Thought.Inspired_1, Thought.Inspired_2, Thought.Inspired_3, Thought.Inspired_4);
    if (this._brain.HasThought(randomThoughtFromSet))
      return;
    this._brain.AddThought(randomThoughtFromSet);
  }
}
