// Decompiled with JetBrains decompiler
// Type: FollowerTask_ReactSnow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerTask_ReactSnow : FollowerTask
{
  public override FollowerTaskType Type => FollowerTaskType.ReactSnow;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockTaskChanges => true;

  public override int UsingStructureID => 0;

  public override int GetSubTaskCode() => 0;

  public override void OnStart() => this.SetState(FollowerTaskState.Doing);

  public override void TaskTick(float deltaGameTime)
  {
    if (this._brain.Location == PlayerFarming.Location)
      return;
    this.End();
  }

  public override void ProgressTask() => this.End();

  public override void OnEnd() => base.OnEnd();

  public override Vector3 UpdateDestination(Follower follower) => follower.transform.position;

  public override void OnDoingBegin(Follower follower)
  {
    follower.AddThought(Thought.ReactSnow);
    float timer = 4f;
    string animation = "Snow/look-up-happy";
    float num = UnityEngine.Random.value;
    if ((double) num < 0.10000000149011612)
      animation = "Snow/look-up-happy";
    else if ((double) num < 0.20000000298023224)
      animation = "Snow/look-up-wonder";
    else if ((double) num < 0.30000001192092896)
      animation = "Snow/look-up-worried";
    else if ((double) num < 0.40000000596046448)
    {
      animation = "Snow/idle-look-up-smile";
      timer = 3.86666656f;
    }
    else if ((double) num < 0.5)
    {
      timer = 6.5333333f;
      animation = "Snow/idle-tongueout";
    }
    else if ((double) num < 0.60000002384185791)
    {
      timer = 6.5333333f;
      animation = "Snow/idle-tongueout2";
    }
    follower.TimedAnimation(animation, timer, (System.Action) (() => this.ProgressTask()));
  }

  [CompilerGenerated]
  public void \u003COnDoingBegin\u003Eb__14_0() => this.ProgressTask();
}
