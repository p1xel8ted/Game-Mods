// Decompiled with JetBrains decompiler
// Type: DynamicNotification_SickFolllower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DynamicNotification_SickFolllower : DynamicNotificationData
{
  public List<int> _sickFollowerIDs = new List<int>();
  public int _soonestDeathID;

  public override NotificationCentre.NotificationType Type
  {
    get => NotificationCentre.NotificationType.Dynamic_Sick;
  }

  public override bool IsEmpty => this._sickFollowerIDs.Count == 0;

  public override bool HasProgress => true;

  public override bool HasDynamicProgress => false;

  public override float CurrentProgress
  {
    get
    {
      float currentProgress = 1f;
      if (this._soonestDeathID != 0)
      {
        FollowerBrain brainById = FollowerBrain.FindBrainByID(this._soonestDeathID);
        if (brainById != null)
          currentProgress = Mathf.Clamp((float) (1.0 - (100.0 - (double) brainById.Stats.Illness) / 100.0), 0.1f, 1f);
        else
          this.RemoveFollower(this._soonestDeathID);
      }
      else
      {
        this._sickFollowerIDs.Clear();
        System.Action dataChanged = this.DataChanged;
        if (dataChanged != null)
          dataChanged();
      }
      return currentProgress;
    }
  }

  public override float TotalCount => (float) this._sickFollowerIDs.Count;

  public override string SkinName
  {
    get => FollowerBrain.FindBrainByID(this._sickFollowerIDs[0]).Info.SkinName;
  }

  public override int SkinColor
  {
    get => FollowerBrain.FindBrainByID(this._sickFollowerIDs[0]).Info.SkinColour;
  }

  public void AddFollower(FollowerBrain brain)
  {
    if (this._sickFollowerIDs.Contains(brain.Info.ID))
      return;
    this._sickFollowerIDs.Add(brain.Info.ID);
    this.RecalculateSoonestDeath();
    System.Action dataChanged = this.DataChanged;
    if (dataChanged == null)
      return;
    dataChanged();
  }

  public void RemoveFollower(int brainID)
  {
    this._sickFollowerIDs.Remove(brainID);
    this.RecalculateSoonestDeath();
    System.Action dataChanged = this.DataChanged;
    if (dataChanged == null)
      return;
    dataChanged();
  }

  public void RecalculateSoonestDeath()
  {
    this._soonestDeathID = 0;
    float num = 0.0f;
    foreach (int sickFollowerId in this._sickFollowerIDs)
    {
      FollowerBrain brainById = FollowerBrain.FindBrainByID(sickFollowerId);
      if (brainById != null && (double) brainById.Stats.Illness > (double) num)
      {
        this._soonestDeathID = sickFollowerId;
        num = brainById.Stats.Illness;
      }
    }
  }

  public void UpdateFollowers()
  {
    for (int index = this._sickFollowerIDs.Count - 1; index >= 0; --index)
    {
      FollowerBrain brainById = FollowerBrain.FindBrainByID(this._sickFollowerIDs[index]);
      if (brainById == null || brainById.Info.CursedState != Thought.Ill)
        this.RemoveFollower(this._sickFollowerIDs[index]);
    }
  }
}
