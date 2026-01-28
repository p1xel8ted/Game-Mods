// Decompiled with JetBrains decompiler
// Type: DynamicNotification_InjuredFolllower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class DynamicNotification_InjuredFolllower : DynamicNotificationData
{
  public List<int> _injuredFollowerIDs = new List<int>();

  public override NotificationCentre.NotificationType Type
  {
    get => NotificationCentre.NotificationType.Dynamic_Injured;
  }

  public override bool IsEmpty => this._injuredFollowerIDs.Count == 0;

  public override bool HasProgress => true;

  public override bool HasDynamicProgress => true;

  public override float CurrentProgress
  {
    get
    {
      float num = 0.0f;
      foreach (FollowerInfo followerInfo in FollowerManager.FindFollowersByID(this._injuredFollowerIDs))
        num += followerInfo.Injured / 100f;
      return num / (float) this._injuredFollowerIDs.Count;
    }
  }

  public override float TotalCount => (float) this._injuredFollowerIDs.Count;

  public override string SkinName
  {
    get => FollowerBrain.FindBrainByID(this._injuredFollowerIDs[0]).Info.SkinName;
  }

  public override int SkinColor
  {
    get => FollowerBrain.FindBrainByID(this._injuredFollowerIDs[0]).Info.SkinColour;
  }

  public void AddFollower(FollowerBrain brain)
  {
    if (this._injuredFollowerIDs.Contains(brain.Info.ID))
      return;
    this._injuredFollowerIDs.Add(brain.Info.ID);
    System.Action dataChanged = this.DataChanged;
    if (dataChanged == null)
      return;
    dataChanged();
  }

  public void RemoveFollower(int brainID)
  {
    this._injuredFollowerIDs.Remove(brainID);
    System.Action dataChanged = this.DataChanged;
    if (dataChanged == null)
      return;
    dataChanged();
  }

  public void UpdateFollowers()
  {
    for (int index = this._injuredFollowerIDs.Count - 1; index >= 0; --index)
    {
      FollowerBrain brainById = FollowerBrain.FindBrainByID(this._injuredFollowerIDs[index]);
      if (brainById != null && (double) brainById.Stats.Injured <= 0.0)
        this.RemoveFollower(this._injuredFollowerIDs[index]);
    }
  }
}
