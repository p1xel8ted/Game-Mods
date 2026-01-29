// Decompiled with JetBrains decompiler
// Type: DynamicNotification_StarvingFollower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DynamicNotification_StarvingFollower : DynamicNotificationData
{
  public List<int> _starvingFollowerIDs = new List<int>();
  public int _soonestStarverID;
  public float PrevTotalCount;

  public override NotificationCentre.NotificationType Type
  {
    get => NotificationCentre.NotificationType.Dynamic_Starving;
  }

  public override bool IsEmpty => this._starvingFollowerIDs.Count == 0;

  public override bool HasProgress => true;

  public override bool HasDynamicProgress => true;

  public override float CurrentProgress
  {
    get
    {
      float currentProgress = 1f;
      if (this._soonestStarverID != 0)
      {
        FollowerBrain brainById = FollowerBrain.FindBrainByID(this._soonestStarverID);
        if (brainById != null)
          currentProgress = Mathf.Clamp((float) (1.0 - (75.0 - (double) brainById.Stats.Starvation) / 75.0), 0.1f, 1f);
        else
          this.RemoveFollower(this._soonestStarverID);
      }
      else
      {
        this._starvingFollowerIDs.Clear();
        System.Action dataChanged = this.DataChanged;
        if (dataChanged != null)
          dataChanged();
      }
      return currentProgress;
    }
  }

  public override float TotalCount => (float) this._starvingFollowerIDs.Count;

  public override string SkinName
  {
    get => FollowerBrain.FindBrainByID(this._soonestStarverID).Info.SkinName;
  }

  public override int SkinColor
  {
    get => FollowerBrain.FindBrainByID(this._soonestStarverID).Info.SkinColour;
  }

  public void AddFollower(FollowerBrain brain)
  {
    this.PrevTotalCount = this.TotalCount;
    if (!this._starvingFollowerIDs.Contains(brain.Info.ID))
    {
      this._starvingFollowerIDs.Add(brain.Info.ID);
      this.RecalculateSoonestStarver();
    }
    this.CheckThoughts();
    System.Action dataChanged = this.DataChanged;
    if (dataChanged == null)
      return;
    dataChanged();
  }

  public void RemoveFollower(int brainID)
  {
    this.PrevTotalCount = this.TotalCount;
    this._starvingFollowerIDs.Remove(brainID);
    this.RecalculateSoonestStarver();
    this.CheckThoughts();
    System.Action dataChanged = this.DataChanged;
    if (dataChanged == null)
      return;
    dataChanged();
  }

  public void CheckThoughts()
  {
    if ((double) this.PrevTotalCount == (double) this.TotalCount)
      return;
    if ((double) this.PrevTotalCount == 0.0 && (double) this.TotalCount >= 0.0 && StructureManager.GetAllStructuresOfType<Structures_Meal>().Count == 0)
      CultFaithManager.AddThought(Thought.Cult_Starving);
    if ((double) this.PrevTotalCount < 0.0 || (double) this.TotalCount != 0.0)
      return;
    CultFaithManager.AddThought(Thought.Cult_No_Longer_Starving);
  }

  public void RecalculateSoonestStarver()
  {
    this._soonestStarverID = 0;
    float num = 0.0f;
    foreach (int starvingFollowerId in this._starvingFollowerIDs)
    {
      FollowerBrain brainById = FollowerBrain.FindBrainByID(starvingFollowerId);
      if (brainById != null && (double) brainById.Stats.Starvation > (double) num)
      {
        this._soonestStarverID = starvingFollowerId;
        num = brainById.Stats.Starvation;
      }
    }
  }

  public void UpdateFollowers()
  {
    for (int index = this._starvingFollowerIDs.Count - 1; index >= 0; --index)
    {
      FollowerBrain brainById = FollowerBrain.FindBrainByID(this._starvingFollowerIDs[index]);
      if (brainById == null || brainById.Info.CursedState != Thought.BecomeStarving)
        this.RemoveFollower(this._starvingFollowerIDs[index]);
    }
  }
}
