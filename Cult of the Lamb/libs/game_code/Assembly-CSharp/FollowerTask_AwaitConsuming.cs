// Decompiled with JetBrains decompiler
// Type: FollowerTask_AwaitConsuming
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerTask_AwaitConsuming : FollowerTask
{
  public static List<Follower> awaitingConsumingFollowers = new List<Follower>();
  public Follower follower;

  public override FollowerTaskType Type => FollowerTaskType.AwaitConsuming;

  public override FollowerLocation Location => this._brain.Location;

  public override bool BlockReactTasks => true;

  public override bool BlockTaskChanges => true;

  public override int GetSubTaskCode() => 0;

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    this.follower = follower;
    FollowerTask_AwaitConsuming.awaitingConsumingFollowers.Add(follower);
    follower.transform.position = this.UpdateDestination(follower);
  }

  public override Vector3 UpdateDestination(Follower follower) => follower.transform.position;

  public override void TaskTick(float deltaGameTime)
  {
  }

  public override void Cleanup(Follower follower) => base.Cleanup(follower);
}
