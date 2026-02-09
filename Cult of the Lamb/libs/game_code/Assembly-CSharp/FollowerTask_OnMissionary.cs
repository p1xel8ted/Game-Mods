// Decompiled with JetBrains decompiler
// Type: FollowerTask_OnMissionary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerTask_OnMissionary : FollowerTask
{
  public Follower follower;

  public override FollowerTaskType Type => FollowerTaskType.MissionaryInProgress;

  public override FollowerLocation Location => FollowerLocation.Missionary;

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

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    this.follower = follower;
  }

  public override void OnAbort()
  {
    base.OnAbort();
    this._brain.DesiredLocation = FollowerLocation.Base;
    DataManager.Instance.Followers_OnMissionary_IDs.Remove(this._brain.Info.ID);
    if (StructureManager.GetAllStructuresOfType<Structures_Missionary>(FollowerLocation.Base).Count > 0)
      StructureManager.GetAllStructuresOfType<Structures_Missionary>(FollowerLocation.Base)?[0].Data.MultipleFollowerIDs.Remove(this._brain.Info.ID);
    if (!(bool) (Object) this.follower)
      return;
    this.follower.SetOutfit(FollowerOutfitType.Follower, false);
    this.follower.Interaction_FollowerInteraction.Interactable = true;
  }

  public override void OnComplete()
  {
    base.OnComplete();
    Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    if ((Object) followerById != (Object) null)
      followerById.SetOutfit(FollowerOutfitType.Follower, false);
    else
      this._brain._directInfoAccess.Outfit = FollowerOutfitType.Follower;
  }

  public override void TaskTick(float deltaGameTime)
  {
    if ((double) TimeManager.TotalElapsedGameTime >= (double) this._brain._directInfoAccess.MissionaryTimestamp + (double) this._brain._directInfoAccess.MissionaryDuration)
    {
      this._brain.CompleteCurrentTask();
      if (DataManager.Instance.Followers_OnMissionary_IDs.Contains(this.Brain.Info.ID))
      {
        this._brain.HardSwapToTask((FollowerTask) new FollowerTask_MissionaryComplete());
        ++DataManager.Instance.MissionariesCompleted;
        if (DataManager.Instance.MissionariesCompleted <= 2)
          this._brain._directInfoAccess.MissionaryChance = 100f;
        this._brain._directInfoAccess.MissionaryRewards = this._brain._directInfoAccess.MissionarySuccessful ? MissionaryManager.GetReward((InventoryItem.ITEM_TYPE) this._brain._directInfoAccess.MissionaryType, float.MaxValue, this._brain.Info.ID) : new InventoryItem[0];
        this._brain._directInfoAccess.MissionaryFinished = true;
        int num = Random.Range(0, 10);
        if (num == 0)
          this._brain.AddThought(Thought.TiredFromMissionaryScared);
        if (num == 1)
          this._brain.AddThought(Thought.TiredFromMissionaryHappy);
        else
          this._brain.AddThought(Thought.TiredFromMissionary);
        if (this._brain.CurrentTaskType == FollowerTaskType.ChangeLocation)
          this._brain.CurrentTask.Arrive();
      }
    }
    if (PlayerFarming.Location != FollowerLocation.Base)
      return;
    Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    if ((bool) (Object) followerById)
    {
      FollowerManager.FollowersAtLocation(FollowerLocation.Base).Remove(followerById);
      FollowerManager.FollowersAtLocation(FollowerLocation.Missionary).Add(followerById);
      Object.Destroy((Object) followerById.gameObject);
    }
    if (!((Object) Interaction_Missionaries.Instance == (Object) null) || StructureManager.GetAllStructuresOfType<Structures_Missionary>().Count > 0)
      return;
    this.Abort();
  }

  public override Vector3 UpdateDestination(Follower follower) => Vector3.zero;
}
