// Decompiled with JetBrains decompiler
// Type: FollowerTask_Terrified
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerTask_Terrified : FollowerTask
{
  public override FollowerTaskType Type => FollowerTaskType.Terrified;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockReactTasks => true;

  public override bool BlockSocial => true;

  public override bool BlockTaskChanges => false;

  public override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  public override int GetSubTaskCode() => 0;

  public override void TaskTick(float deltaGameTime)
  {
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(0.0f, -1f));
    LayerMask layerMask = (LayerMask) ((int) new LayerMask() | 1 << LayerMask.NameToLayer("Island"));
    return (Vector3) (Physics2D.Raycast((Vector2) Vector3.zero, direction, 1000f, (int) layerMask).point + -direction * Random.Range(2f, 4f));
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "unconverted");
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "idle");
  }
}
