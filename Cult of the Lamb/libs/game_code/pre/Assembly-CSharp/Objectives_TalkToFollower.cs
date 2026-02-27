// Decompiled with JetBrains decompiler
// Type: Objectives_TalkToFollower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class Objectives_TalkToFollower : ObjectivesData
{
  public bool Done;
  public string ResponseTerm = "";
  public int TargetFollower = -1;

  public override string Text
  {
    get
    {
      return string.IsNullOrEmpty(this.ResponseTerm) ? string.Format(ScriptLocalization.Objectives.CollectReward, (object) FollowerInfo.GetInfoByID(this.Follower, true)?.Name) : string.Format(ScriptLocalization.Objectives.TalkToFollower, (object) FollowerInfo.GetInfoByID(this.TargetFollower, true)?.Name);
    }
  }

  public Objectives_TalkToFollower()
  {
  }

  public Objectives_TalkToFollower(string groupId, string term = "", float expireTimestamp = -1f)
    : base(groupId, expireTimestamp)
  {
    this.Type = Objectives.TYPES.TALK_TO_FOLLOWER;
    this.ResponseTerm = term;
  }

  public override ObjectivesDataFinalized GetFinalizedData()
  {
    Objectives_TalkToFollower.FinalizedData_TalkToFollower dataTalkToFollower = new Objectives_TalkToFollower.FinalizedData_TalkToFollower();
    dataTalkToFollower.GroupId = this.GroupId;
    dataTalkToFollower.Index = this.Index;
    dataTalkToFollower.UniqueGroupID = this.UniqueGroupID;
    Objectives_TalkToFollower.FinalizedData_TalkToFollower finalizedData = dataTalkToFollower;
    if (string.IsNullOrEmpty(this.ResponseTerm))
    {
      finalizedData.LocKey = "Objectives/CollectReward";
      finalizedData.TargetFollowerName = FollowerInfo.GetInfoByID(this.Follower, true)?.Name;
    }
    else
    {
      finalizedData.LocKey = "Objectives/TalkToFollower";
      finalizedData.TargetFollowerName = FollowerInfo.GetInfoByID(this.TargetFollower, true)?.Name;
    }
    return (ObjectivesDataFinalized) finalizedData;
  }

  protected override bool CheckComplete()
  {
    if (string.IsNullOrEmpty(this.ResponseTerm))
    {
      FollowerInfo infoById = FollowerInfo.GetInfoByID(this.Follower);
      if (!this.IsFailed && this.Follower != -1 && (infoById == null || DataManager.Instance.Followers_Dead.Contains(infoById) || !this.TargetFollowerAllowOldAge && infoById.CursedState == Thought.OldAge))
      {
        if (DataManager.Instance.Followers.Count > 0)
        {
          int follower1 = this.Follower;
          List<FollowerInfo> followerInfoList = new List<FollowerInfo>();
          foreach (FollowerInfo follower2 in DataManager.Instance.Followers)
          {
            if (!FollowerManager.FollowerLocked(follower2.ID))
              followerInfoList.Add(follower2);
          }
          this.Follower = followerInfoList.Count > 0 ? followerInfoList[UnityEngine.Random.Range(0, followerInfoList.Count)].ID : DataManager.Instance.Followers[UnityEngine.Random.Range(0, DataManager.Instance.Followers.Count)].ID;
          if (DataManager.Instance.CompletedQuestFollowerIDs.Contains(follower1))
            DataManager.Instance.CompletedQuestFollowerIDs.Remove(follower1);
          DataManager.Instance.CompletedQuestFollowerIDs.Add(this.Follower);
          FollowerManager.FindFollowerByID(this.Follower)?.ShowCompletedQuestIcon(true);
          ObjectiveManager.UpdateObjective((ObjectivesData) this);
          return false;
        }
        if (!this.FailLocked)
        {
          this.Failed();
          return false;
        }
      }
      return base.CheckComplete() && this.Done;
    }
    if (base.CheckComplete())
      return this.Done;
    if (FollowerInfo.GetInfoByID(this.TargetFollower) != null)
      return false;
    this.Failed();
    return false;
  }

  public override void Update()
  {
    base.Update();
    if (string.IsNullOrEmpty(this.ResponseTerm) || this.IsFailed || this.TargetFollower == -1 || FollowerInfo.GetInfoByID(this.TargetFollower) != null)
      return;
    this.Failed();
  }

  public override void Complete()
  {
    if (!string.IsNullOrEmpty(this.ResponseTerm))
      base.Complete();
    else if (!this.IsComplete)
      DataManager.Instance.AddToCompletedQuestHistory(this.GetFinalizedData());
    this.IsComplete = true;
  }

  [Serializable]
  public class FinalizedData_TalkToFollower : ObjectivesDataFinalized
  {
    public string LocKey;
    public string TargetFollowerName;

    public override string GetText()
    {
      return string.Format(LocalizationManager.GetTranslation(this.LocKey), (object) this.TargetFollowerName);
    }
  }
}
