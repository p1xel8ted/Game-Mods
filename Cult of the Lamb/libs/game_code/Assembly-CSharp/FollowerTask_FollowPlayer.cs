// Decompiled with JetBrains decompiler
// Type: FollowerTask_FollowPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class FollowerTask_FollowPlayer : FollowerTask_AssistPlayerBase
{
  public override FollowerTaskType Type => FollowerTaskType.FollowPlayer;

  public override FollowerLocation Location => this._brain.Location;

  public FollowerTask_FollowPlayer() => this._helpingPlayer = true;

  public override void OnArrive() => this.SetState(FollowerTaskState.Idle);

  public override void AssistPlayerTick(float deltaGameTime)
  {
    PlayerFarming instance = PlayerFarming.Instance;
    if ((UnityEngine.Object) instance == (UnityEngine.Object) null)
      return;
    Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    if (!((UnityEngine.Object) followerById != (UnityEngine.Object) null))
      return;
    if (this.State == FollowerTaskState.Idle)
    {
      if ((double) Vector3.Distance(instance.transform.position, followerById.transform.position) <= 3.0)
        return;
      this.ClearDestination();
      this.SetState(FollowerTaskState.GoingTo);
    }
    else
    {
      if (this.State != FollowerTaskState.GoingTo || !this._currentDestination.HasValue || (double) Vector3.Distance(instance.transform.position, this._currentDestination.Value) <= 3.0)
        return;
      this.RecalculateDestination();
    }
  }

  public override void OnPlayerLocationChange()
  {
    this._brain.DesiredLocation = PlayerFarming.Location;
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    PlayerFarming instance = PlayerFarming.Instance;
    if ((UnityEngine.Object) instance == (UnityEngine.Object) null)
      return follower.transform.position;
    float num = 1f;
    float f = Utils.GetAngle(instance.transform.position, follower.transform.position) * ((float) Math.PI / 180f);
    return instance.transform.position + new Vector3(num * Mathf.Cos(f), num * Mathf.Sin(f));
  }
}
