// Decompiled with JetBrains decompiler
// Type: DynamicNotification_FreezingFolllower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class DynamicNotification_FreezingFolllower : DynamicNotificationData
{
  public List<int> _freezingFollowerIDs = new List<int>();

  public override NotificationCentre.NotificationType Type
  {
    get => NotificationCentre.NotificationType.Dynamic_Freezing;
  }

  public override bool IsEmpty => this._freezingFollowerIDs.Count == 0;

  public override bool HasProgress => true;

  public override bool HasDynamicProgress => true;

  public override float CurrentProgress
  {
    get
    {
      float num = 0.0f;
      foreach (FollowerInfo followerInfo in FollowerManager.FindFollowersByID(this._freezingFollowerIDs))
        num += followerInfo.Freezing / 100f;
      return num / (float) this._freezingFollowerIDs.Count;
    }
  }

  public override float TotalCount => (float) this._freezingFollowerIDs.Count;

  public override string SkinName
  {
    get => FollowerBrain.FindBrainByID(this._freezingFollowerIDs[0]).Info.SkinName;
  }

  public override int SkinColor
  {
    get => FollowerBrain.FindBrainByID(this._freezingFollowerIDs[0]).Info.SkinColour;
  }

  public void AddFollower(FollowerBrain brain)
  {
    if (this._freezingFollowerIDs.Contains(brain.Info.ID))
      return;
    this._freezingFollowerIDs.Add(brain.Info.ID);
    System.Action dataChanged = this.DataChanged;
    if (dataChanged == null)
      return;
    dataChanged();
  }

  public void RemoveFollower(int brainID)
  {
    this._freezingFollowerIDs.Remove(brainID);
    System.Action dataChanged = this.DataChanged;
    if (dataChanged == null)
      return;
    dataChanged();
  }

  public void UpdateFollowers()
  {
    for (int index = this._freezingFollowerIDs.Count - 1; index >= 0; --index)
    {
      FollowerBrain brainById = FollowerBrain.FindBrainByID(this._freezingFollowerIDs[index]);
      if (brainById == null || (double) brainById.Stats.Freezing <= 0.0)
        this.RemoveFollower(this._freezingFollowerIDs[index]);
    }
  }
}
