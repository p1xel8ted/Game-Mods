// Decompiled with JetBrains decompiler
// Type: DynamicNotification_DrunkFolllower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class DynamicNotification_DrunkFolllower : DynamicNotificationData
{
  public List<int> _drunkFollowerIDs = new List<int>();

  public override NotificationCentre.NotificationType Type
  {
    get => NotificationCentre.NotificationType.Dynamic_Drunk;
  }

  public override bool IsEmpty => this._drunkFollowerIDs.Count == 0;

  public override bool HasProgress => true;

  public override bool HasDynamicProgress => true;

  public override float CurrentProgress
  {
    get
    {
      float num = 0.0f;
      foreach (FollowerInfo followerInfo in FollowerManager.FindFollowersByID(this._drunkFollowerIDs))
        num += followerInfo.Drunk / 100f;
      return num / (float) this._drunkFollowerIDs.Count;
    }
  }

  public override float TotalCount => (float) this._drunkFollowerIDs.Count;

  public override string SkinName
  {
    get => FollowerBrain.FindBrainByID(this._drunkFollowerIDs[0]).Info.SkinName;
  }

  public override int SkinColor
  {
    get => FollowerBrain.FindBrainByID(this._drunkFollowerIDs[0]).Info.SkinColour;
  }

  public void AddFollower(FollowerBrain brain)
  {
    if (this._drunkFollowerIDs.Contains(brain.Info.ID))
      return;
    this._drunkFollowerIDs.Add(brain.Info.ID);
    System.Action dataChanged = this.DataChanged;
    if (dataChanged == null)
      return;
    dataChanged();
  }

  public void RemoveFollower(int brainID)
  {
    this._drunkFollowerIDs.Remove(brainID);
    System.Action dataChanged = this.DataChanged;
    if (dataChanged == null)
      return;
    dataChanged();
  }

  public void UpdateFollowers()
  {
    for (int index = this._drunkFollowerIDs.Count - 1; index >= 0; --index)
    {
      FollowerBrain brainById = FollowerBrain.FindBrainByID(this._drunkFollowerIDs[index]);
      if (brainById == null || (double) brainById.Stats.Drunk <= 0.0)
        this.RemoveFollower(this._drunkFollowerIDs[index]);
    }
  }
}
