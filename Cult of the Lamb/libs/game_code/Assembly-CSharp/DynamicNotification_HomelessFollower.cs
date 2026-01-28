// Decompiled with JetBrains decompiler
// Type: DynamicNotification_HomelessFollower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DynamicNotification_HomelessFollower : DynamicNotificationData
{
  public int HouseCount;
  public int FollowersCount;
  public float PrevTotalCount;

  public override NotificationCentre.NotificationType Type
  {
    get => NotificationCentre.NotificationType.Dynamic_Homeless;
  }

  public override bool IsEmpty => (double) this.TotalCount <= 0.0;

  public override bool HasProgress => false;

  public override bool HasDynamicProgress => false;

  public override float CurrentProgress => 0.0f;

  public override float TotalCount => (float) Mathf.Max(0, this.FollowersCount - this.HouseCount);

  public override string SkinName => "";

  public override int SkinColor => 0;

  public void CheckAll()
  {
    if (!DataManager.Instance.OnboardedHomeless || TimeManager.CurrentDay == 1 && TimeManager.CurrentPhase != DayPhase.Night)
      return;
    this.PrevTotalCount = this.TotalCount;
    this.HouseCount = StructureManager.GetTotalHomesCount(includeUpgradeSites: true);
    this.FollowersCount = DataManager.Instance.Followers.Count;
    this.CheckEnoughBeds();
    System.Action dataChanged = this.DataChanged;
    if (dataChanged == null)
      return;
    dataChanged();
  }

  public void CheckFollowerCount()
  {
    if (!DataManager.Instance.OnboardedHomeless || TimeManager.CurrentDay == 1 && TimeManager.CurrentPhase != DayPhase.Night)
      return;
    this.PrevTotalCount = this.TotalCount;
    this.HouseCount = StructureManager.GetTotalHomesCount(includeUpgradeSites: true);
    this.FollowersCount = DataManager.Instance.Followers.Count;
    this.CheckEnoughBeds();
    System.Action dataChanged = this.DataChanged;
    if (dataChanged == null)
      return;
    dataChanged();
  }

  public void OnStructuresPlaced()
  {
    if (!DataManager.Instance.OnboardedHomeless || TimeManager.CurrentDay == 1 && TimeManager.CurrentPhase != DayPhase.Night)
      return;
    this.PrevTotalCount = this.TotalCount;
    this.HouseCount = StructureManager.GetTotalHomesCount(includeUpgradeSites: true);
    this.FollowersCount = DataManager.Instance.Followers.Count;
    this.CheckEnoughBeds();
    System.Action dataChanged = this.DataChanged;
    if (dataChanged == null)
      return;
    dataChanged();
  }

  public void OnStructureAdded(StructuresData structure)
  {
    if (!DataManager.Instance.OnboardedHomeless || TimeManager.CurrentDay == 1 && TimeManager.CurrentPhase != DayPhase.Night)
      return;
    this.PrevTotalCount = this.TotalCount;
    this.HouseCount = StructureManager.GetTotalHomesCount(includeUpgradeSites: true);
    this.FollowersCount = DataManager.Instance.Followers.Count;
    this.CheckEnoughBeds();
    System.Action dataChanged = this.DataChanged;
    if (dataChanged == null)
      return;
    dataChanged();
  }

  public void CheckEnoughBeds()
  {
    if (!DataManager.Instance.ShowCultFaith || !(bool) (UnityEngine.Object) BaseLocationManager.Instance || !BaseLocationManager.Instance.StructuresPlaced || !BaseLocationManager.Instance.FollowersSpawned)
      return;
    if ((double) this.TotalCount > 0.0 && (double) this.PrevTotalCount == 0.0 && !CultFaithManager.HasThought(Thought.Cult_NotEnoughBeds))
      CultFaithManager.AddThought(Thought.Cult_NotEnoughBeds);
    if ((double) this.TotalCount != 0.0 || (double) this.PrevTotalCount < 0.0 || !CultFaithManager.HasThought(Thought.Cult_NotEnoughBeds))
      return;
    CultFaithManager.RemoveThought(Thought.Cult_NotEnoughBeds);
  }
}
