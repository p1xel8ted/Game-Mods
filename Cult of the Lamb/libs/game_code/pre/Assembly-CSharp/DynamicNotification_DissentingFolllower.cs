// Decompiled with JetBrains decompiler
// Type: DynamicNotification_DissentingFolllower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DynamicNotification_DissentingFolllower : DynamicNotificationData
{
  private List<int> _dissenterFollowerIDs = new List<int>();
  private int _soonestLeaverID;

  public override NotificationCentre.NotificationType Type
  {
    get => NotificationCentre.NotificationType.Dynamic_Dissenter;
  }

  public override bool IsEmpty => this._dissenterFollowerIDs.Count == 0;

  public override bool HasProgress => true;

  public override bool HasDynamicProgress => false;

  public override float CurrentProgress
  {
    get
    {
      float currentProgress = 1f;
      if (this._soonestLeaverID != 0)
      {
        FollowerBrain brainById = FollowerBrain.FindBrainByID(this._soonestLeaverID);
        if (brainById != null)
          currentProgress = Mathf.Clamp((float) (1.0 - (100.0 - (double) brainById.Stats.Reeducation) / 100.0), 0.1f, 1f);
        else
          this.RemoveFollower(this._soonestLeaverID);
      }
      else
      {
        this._dissenterFollowerIDs.Clear();
        System.Action dataChanged = this.DataChanged;
        if (dataChanged != null)
          dataChanged();
      }
      return currentProgress;
    }
  }

  public override float TotalCount => (float) this._dissenterFollowerIDs.Count;

  public override string SkinName
  {
    get
    {
      return FollowerBrain.FindBrainByID(this._dissenterFollowerIDs[this._soonestLeaverID]).Info.SkinName;
    }
  }

  public override int SkinColor
  {
    get
    {
      return FollowerBrain.FindBrainByID(this._dissenterFollowerIDs[this._soonestLeaverID]).Info.SkinColour;
    }
  }

  public void AddFollower(FollowerBrain brain)
  {
    if (this._dissenterFollowerIDs.Contains(brain.Info.ID))
      return;
    this._dissenterFollowerIDs.Add(brain.Info.ID);
    this.RecalculateSoonestLeaver();
    System.Action dataChanged = this.DataChanged;
    if (dataChanged == null)
      return;
    dataChanged();
  }

  public void RemoveFollower(int brainID)
  {
    this._dissenterFollowerIDs.Remove(brainID);
    this.RecalculateSoonestLeaver();
    System.Action dataChanged = this.DataChanged;
    if (dataChanged == null)
      return;
    dataChanged();
  }

  private void RecalculateSoonestLeaver()
  {
    this._soonestLeaverID = 0;
    float num = 0.0f;
    foreach (int dissenterFollowerId in this._dissenterFollowerIDs)
    {
      FollowerBrain brainById = FollowerBrain.FindBrainByID(dissenterFollowerId);
      if (brainById != null && (double) brainById.Stats.Reeducation > (double) num)
      {
        this._soonestLeaverID = dissenterFollowerId;
        num = brainById.Stats.Reeducation;
      }
    }
  }

  public void UpdateFollowers()
  {
    for (int index = this._dissenterFollowerIDs.Count - 1; index >= 0; --index)
    {
      FollowerBrain brainById = FollowerBrain.FindBrainByID(this._dissenterFollowerIDs[index]);
      if (brainById != null && brainById.Info.CursedState == Thought.Dissenter)
        this.RemoveFollower(this._dissenterFollowerIDs[index]);
    }
  }
}
