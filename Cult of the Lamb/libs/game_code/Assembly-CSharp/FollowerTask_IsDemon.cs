// Decompiled with JetBrains decompiler
// Type: FollowerTask_IsDemon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerTask_IsDemon : FollowerTask
{
  public override FollowerTaskType Type => FollowerTaskType.IsDemon;

  public override FollowerLocation Location => FollowerLocation.Demon;

  public override bool ShouldSaveDestination => true;

  public override bool DisablePickUpInteraction => true;

  public override bool BlockReactTasks => true;

  public override bool BlockSermon => true;

  public override bool BlockTaskChanges => true;

  public override bool BlockThoughts => true;

  public override float Priorty => 1000f;

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

  public override void TaskTick(float deltaGameTime)
  {
    if (!DataManager.Instance.Followers_Demons_IDs.Contains(this._brain.Info.ID))
    {
      this._brain.HardSwapToTask((FollowerTask) new FollowerTask_Idle());
      if (this._brain.CurrentTaskType == FollowerTaskType.ChangeLocation)
        this._brain.CurrentTask.Arrive();
      this._brain.CompleteCurrentTask();
    }
    else
    {
      if (PlayerFarming.Location != FollowerLocation.Base || !((Object) Interaction_DemonSummoner.Instance == (Object) null) || StructureManager.GetAllStructuresOfType<Structures_Demon_Summoner>().Count > 0)
        return;
      DataManager.Instance.Followers_Demons_IDs.Remove(this._brain.Info.ID);
      this._brain.HardSwapToTask((FollowerTask) new FollowerTask_Idle());
      if (this._brain.CurrentTaskType == FollowerTaskType.ChangeLocation)
        this._brain.CurrentTask.Arrive();
      this._brain.CompleteCurrentTask();
    }
  }

  public override Vector3 UpdateDestination(Follower follower) => Vector3.zero;

  public override void Setup(Follower follower) => base.Setup(follower);
}
