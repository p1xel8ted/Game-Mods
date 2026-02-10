// Decompiled with JetBrains decompiler
// Type: FollowerTask_SpyPray
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerTask_SpyPray : FollowerTask_Pray
{
  public FollowerTask_SpyPray(int shrineID)
    : base(shrineID)
  {
    this._shrineID = shrineID;
    this._shrine = StructureManager.GetStructureByID<Structures_Shrine>(this._shrineID);
  }

  public override void OnStart()
  {
    this.ClearDestination();
    this.SetState(FollowerTaskState.GoingTo);
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, this._shrine.Data.FullyFueled ? "pray-flame" : this.GetSneakyPrayAnim());
    this.follower = follower;
    if (this._brain == null || this._shrine == null || !(this._brain.LastPosition == this._shrine.Data.Position))
      return;
    follower.Seeker.CancelCurrentPathRequest();
    this.ClearDestination();
    this.SetState(FollowerTaskState.Doing);
    follower.transform.position = this.UpdateDestination(follower);
  }

  public override void OnIdleBegin(Follower follower)
  {
    base.OnIdleBegin(follower);
    this.follower = follower;
    follower.State.facingAngle = Utils.GetAngle(follower.transform.position, this._shrine.Data.Position);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, this._shrine.Data.FullyFueled ? "pray-flame" : this.GetSneakyPrayAnim());
  }

  public string GetSneakyPrayAnim()
  {
    float num = Random.value;
    return (double) num >= 0.33000001311302185 ? ((double) num >= 0.6600000262260437 ? "Prison/Unlawful/pray-suspicious-look" : "Prison/Unlawful/pray-suspicious-eyesclosed") : "Prison/Unlawful/pray-suspicious";
  }
}
