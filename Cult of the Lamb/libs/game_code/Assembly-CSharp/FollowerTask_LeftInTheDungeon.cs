// Decompiled with JetBrains decompiler
// Type: FollowerTask_LeftInTheDungeon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerTask_LeftInTheDungeon : FollowerTask
{
  public Follower follower;

  public override FollowerTaskType Type => FollowerTaskType.LeftInTheDungeon;

  public override FollowerLocation Location => FollowerLocation.LeftInTheDungeon;

  public override bool ShouldSaveDestination => false;

  public override bool DisablePickUpInteraction => true;

  public override bool BlockReactTasks => true;

  public override bool BlockSermon => true;

  public override bool BlockTaskChanges => true;

  public override bool BlockThoughts => true;

  public override float Priorty => 1000f;

  public override void OnInitialized()
  {
    base.OnInitialized();
    this._brain.Location = this.Location;
    this._brain.DesiredLocation = this.Location;
  }

  public override PriorityCategory GetPriorityCategory(
    FollowerRole FollowerRole,
    WorkerPriority WorkerPriority,
    FollowerBrain brain)
  {
    return PriorityCategory.ExtremelyUrgent;
  }

  public override int GetSubTaskCode() => 0;

  public override float SatiationChange(float deltaGameTime) => 0.0f;

  public override float RestChange(float deltaGameTime) => 0.0f;

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    this.follower = follower;
  }

  public override void OnAbort()
  {
    base.OnAbort();
    this._brain.DesiredLocation = FollowerLocation.Base;
    this._brain.Location = FollowerLocation.Base;
    DataManager.Instance.Followers_LeftInTheDungeon_IDs.Remove(this._brain.Info.ID);
    if (!(bool) (Object) this.follower)
      return;
    this.follower.SetOutfit(FollowerOutfitType.Follower, false);
    this.follower.Interaction_FollowerInteraction.Interactable = true;
    FollowerManager.FollowersAtLocation(this.Location).Remove(this.follower);
  }

  public override void TaskTick(float deltaGameTime)
  {
    if (!DataManager.Instance.Followers_LeftInTheDungeon_IDs.Contains(this._brain.Info.ID))
      return;
    if (PlayerFarming.Location == FollowerLocation.Base)
    {
      Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
      if ((bool) (Object) followerById)
      {
        FollowerManager.FollowersAtLocation(FollowerLocation.Base).Remove(followerById);
        FollowerManager.FollowersAtLocation(this.Location).Add(followerById);
        Object.Destroy((Object) followerById.gameObject);
      }
    }
    if (this._brain.CurrentTaskType != FollowerTaskType.ChangeLocation)
      return;
    this._brain.CurrentTask.Arrive();
  }

  public override Vector3 UpdateDestination(Follower follower) => follower.transform.position;
}
