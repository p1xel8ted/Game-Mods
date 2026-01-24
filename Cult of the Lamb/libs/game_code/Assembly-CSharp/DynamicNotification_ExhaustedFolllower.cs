// Decompiled with JetBrains decompiler
// Type: DynamicNotification_ExhaustedFolllower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class DynamicNotification_ExhaustedFolllower : DynamicNotificationData
{
  public List<int> _exhaustedFollowerIDs = new List<int>();

  public override NotificationCentre.NotificationType Type
  {
    get => NotificationCentre.NotificationType.Exhausted;
  }

  public override bool IsEmpty => this._exhaustedFollowerIDs.Count == 0;

  public override bool HasProgress => true;

  public override bool HasDynamicProgress => true;

  public override float CurrentProgress
  {
    get
    {
      float num = 0.0f;
      foreach (FollowerInfo followerInfo in FollowerManager.FindFollowersByID(this._exhaustedFollowerIDs))
        num += followerInfo.Exhaustion / 100f;
      return num / (float) this._exhaustedFollowerIDs.Count;
    }
  }

  public override float TotalCount => (float) this._exhaustedFollowerIDs.Count;

  public override string SkinName
  {
    get => FollowerBrain.FindBrainByID(this._exhaustedFollowerIDs[0]).Info.SkinName;
  }

  public override int SkinColor
  {
    get => FollowerBrain.FindBrainByID(this._exhaustedFollowerIDs[0]).Info.SkinColour;
  }

  public void AddFollower(FollowerBrain brain)
  {
    if (this._exhaustedFollowerIDs.Contains(brain.Info.ID))
      return;
    this._exhaustedFollowerIDs.Add(brain.Info.ID);
    System.Action dataChanged = this.DataChanged;
    if (dataChanged == null)
      return;
    dataChanged();
  }

  public void RemoveFollower(int brainID)
  {
    this._exhaustedFollowerIDs.Remove(brainID);
    System.Action dataChanged = this.DataChanged;
    if (dataChanged == null)
      return;
    dataChanged();
  }

  public void UpdateFollowers()
  {
    for (int index = this._exhaustedFollowerIDs.Count - 1; index >= 0; --index)
    {
      FollowerBrain brainById = FollowerBrain.FindBrainByID(this._exhaustedFollowerIDs[index]);
      if (brainById == null || (double) brainById.Stats.Exhaustion <= 0.0)
        this.RemoveFollower(this._exhaustedFollowerIDs[index]);
    }
  }
}
