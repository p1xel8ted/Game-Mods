// Decompiled with JetBrains decompiler
// Type: DynamicNotification_ExhaustedFolllower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class DynamicNotification_ExhaustedFolllower : DynamicNotificationData
{
  private List<int> _exhaustedFollowerIDs = new List<int>();

  public override NotificationCentre.NotificationType Type
  {
    get => NotificationCentre.NotificationType.Exhausted;
  }

  public override bool IsEmpty => this._exhaustedFollowerIDs.Count == 0;

  public override bool HasProgress => false;

  public override bool HasDynamicProgress => false;

  public override float CurrentProgress => 0.0f;

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
      if ((double) FollowerBrain.FindBrainByID(this._exhaustedFollowerIDs[index]).Stats.Exhaustion > 0.0)
        this.RemoveFollower(this._exhaustedFollowerIDs[index]);
    }
  }
}
