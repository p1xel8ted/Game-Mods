// Decompiled with JetBrains decompiler
// Type: ObjectivesData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

#nullable disable
[XmlType(Namespace = "Objectives")]
[Serializable]
public abstract class ObjectivesData
{
  public Objectives.TYPES Type;
  public string GroupId;
  public bool IsComplete;
  public bool IsFailed;
  public bool FailLocked;
  public bool AutoRemoveQuestOnceComplete = true;
  public bool TargetFollowerAllowOldAge = true;
  public float QuestCooldown = 12000f;
  public float QuestExpireDuration = -1f;
  public float ExpireTimestamp = -1f;
  public int ID;
  public int Index = -1;
  public string UniqueGroupID = "";
  public string CompleteTerm = "";
  protected bool initialised;

  public abstract string Text { get; }

  public bool HasExpiry
  {
    get => (double) this.ExpireTimestamp > -1.0 && !this.IsFailed && !this.IsComplete;
  }

  public float ExpiryTimeNormalized
  {
    get => (this.ExpireTimestamp - TimeManager.TotalElapsedGameTime) / this.QuestExpireDuration;
  }

  public int Follower { get; set; } = -1;

  public virtual bool AutoTrack { get; }

  public ObjectivesData()
  {
  }

  public ObjectivesData(string groupId, float questExpireDuration = -1f)
  {
    this.GroupId = groupId;
    this.QuestExpireDuration = questExpireDuration;
    this.ID = UnityEngine.Random.Range(0, int.MaxValue);
    this.initialised = false;
  }

  public virtual void Init(bool initialAssigning)
  {
    this.initialised = true;
    this.IsComplete = false;
    this.IsFailed = false;
    if ((double) this.QuestExpireDuration == -1.0 || (double) this.ExpireTimestamp != -1.0)
      return;
    this.ExpireTimestamp = TimeManager.TotalElapsedGameTime + this.QuestExpireDuration;
  }

  public abstract ObjectivesDataFinalized GetFinalizedData();

  public bool TryComplete()
  {
    bool flag = false;
    if (this.CheckComplete())
    {
      this.Complete();
      flag = true;
    }
    return flag;
  }

  protected virtual bool CheckComplete()
  {
    FollowerInfo infoById = FollowerInfo.GetInfoByID(this.Follower);
    if (this.IsFailed || this.Follower == -1 || infoById != null && !DataManager.Instance.Followers_Dead.Contains(infoById) && (this.TargetFollowerAllowOldAge || infoById.CursedState != Thought.OldAge) || this.FailLocked)
      return true;
    this.Failed();
    return false;
  }

  public virtual void Complete()
  {
    if (!this.IsComplete && this.Follower != -1)
    {
      List<ObjectivesData> objectivesDataList = new List<ObjectivesData>((IEnumerable<ObjectivesData>) Quests.GetUnCompletedFollowerQuests(this.Follower, this.GroupId));
      if (objectivesDataList.Contains(this))
        objectivesDataList.Remove(this);
      if (objectivesDataList.Count == 0)
      {
        Objectives_TalkToFollower objective = new Objectives_TalkToFollower(this.GroupId);
        objective.CompleteTerm = this.CompleteTerm;
        objective.Follower = this.Follower;
        ObjectiveManager.Add((ObjectivesData) objective);
        FollowerManager.FindFollowerByID(objective.Follower)?.ShowCompletedQuestIcon(true);
        DataManager.Instance.CompletedQuestFollowerIDs.Add(this.Follower);
        objective.CheckComplete();
      }
    }
    if (!this.IsComplete)
      DataManager.Instance.AddToCompletedQuestHistory(this.GetFinalizedData());
    this.IsComplete = true;
    this.ExpireTimestamp = -1f;
  }

  public virtual void Failed()
  {
    if (this.IsComplete)
      return;
    this.IsFailed = true;
    ObjectiveManager.UpdateObjective(this);
    if (this.Follower != -1)
    {
      this.AutoRemoveQuestOnceComplete = true;
      FollowerInfo infoById = FollowerInfo.GetInfoByID(this.Follower);
      if (infoById != null)
      {
        CultFaithManager.AddThought(Thought.Cult_FailQuest);
        FollowerBrain.GetOrCreateBrain(infoById)?.AddThought(Thought.LeaderFailedQuest);
      }
    }
    this.ExpireTimestamp = -1f;
    ObjectiveManager.ObjectiveFailed(this);
    DataManager.Instance.AddToFailedQuestHistory(this.GetFinalizedData());
  }

  public virtual void Update()
  {
    if ((double) this.ExpireTimestamp == -1.0 || (double) TimeManager.TotalElapsedGameTime < (double) this.ExpireTimestamp)
      return;
    Debug.Log((object) "UPDATE in Objectives");
    this.Failed();
  }

  public bool IsInitialised() => this.initialised;

  public void ResetInitialisation() => this.initialised = false;
}
