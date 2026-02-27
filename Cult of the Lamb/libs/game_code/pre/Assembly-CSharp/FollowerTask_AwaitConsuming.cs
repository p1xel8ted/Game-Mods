// Decompiled with JetBrains decompiler
// Type: FollowerTask_AwaitConsuming
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerTask_AwaitConsuming : FollowerTask
{
  public static List<Follower> awaitingConsumingFollowers = new List<Follower>();
  private Follower follower;

  public override FollowerTaskType Type => FollowerTaskType.AwaitConsuming;

  public override FollowerLocation Location => this._brain.Location;

  public override bool BlockReactTasks => true;

  public override bool BlockTaskChanges => true;

  protected override int GetSubTaskCode() => 0;

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    this.follower = follower;
    FollowerTask_AwaitConsuming.awaitingConsumingFollowers.Add(follower);
    follower.transform.position = this.UpdateDestination(follower);
  }

  protected override Vector3 UpdateDestination(Follower follower) => follower.transform.position;

  protected override void TaskTick(float deltaGameTime)
  {
  }

  public override void Cleanup(Follower follower) => base.Cleanup(follower);
}
