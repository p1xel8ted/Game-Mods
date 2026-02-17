// Decompiled with JetBrains decompiler
// Type: ObjectivesData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Data.Serialization;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using UnityEngine;

#nullable disable
[MessagePackObject(false)]
[Union(0, typeof (Objective_FindRelic))]
[Union(1, typeof (Objectives_AssignClothing))]
[Union(2, typeof (Objectives_BedRest))]
[Union(3, typeof (Objectives_BlizzardOffering))]
[Union(4, typeof (Objectives_BuildStructure))]
[Union(5, typeof (Objectives_CollectItem))]
[Union(6, typeof (Objectives_CookMeal))]
[Union(7, typeof (Objectives_CraftClothing))]
[Union(8, typeof (Objectives_Custom))]
[Union(9, typeof (Objectives_DefeatKnucklebones))]
[Union(10, typeof (Objectives_DepositFood))]
[Union(11, typeof (Objectives_Drink))]
[Union(12, typeof (Objectives_EatMeal))]
[Union(13, typeof (Objectives_FindChildren))]
[Union(14, typeof (Objectives_FindFollower))]
[Union(15, typeof (Objectives_FinishRace))]
[Union(16 /*0x10*/, typeof (Objectives_FlowerBaskets))]
[Union(17, typeof (Objectives_GetAnimal))]
[Union(18, typeof (Objectives_GiveItem))]
[Union(19, typeof (Objectives_KillEnemies))]
[Union(20, typeof (Objectives_LegendaryWeaponRun))]
[Union(21, typeof (Objectives_Mating))]
[Union(22, typeof (Objectives_PerformRitual))]
[Union(23, typeof (Objectives_PlaceStructure))]
[Union(24, typeof (Objectives_RecruitCursedFollower))]
[Union(25, typeof (Objectives_RecruitFollower))]
[Union(26, typeof (Objectives_RemoveStructure))]
[Union(27, typeof (Objectives_ShootDummy))]
[Union(28, typeof (Objectives_ShowFleece))]
[Union(29, typeof (Objectives_Story))]
[Union(30, typeof (Objectives_TalkToFollower))]
[Union(31 /*0x1F*/, typeof (Objectives_UnlockUpgrade))]
[Union(32 /*0x20*/, typeof (Objectives_UseRelic))]
[Union(33, typeof (Objectives_WinFlockadeBet))]
[Union(34, typeof (Objectives_RoomChallenge))]
[Union(35, typeof (Objectives_NoDodge))]
[Union(36, typeof (Objectives_NoDamage))]
[Union(37, typeof (Objectives_NoCurses))]
[Union(38, typeof (Objectives_NoHealing))]
[Union(39, typeof (Objectives_BuildWinterDecorations))]
[Union(40, typeof (Objectives_FeedAnimal))]
[Union(41, typeof (Objectives_WalkAnimal))]
[Union(42, typeof (Objectives_LegendarySwordReturn))]
[XmlType(Namespace = "Objectives")]
[Serializable]
public abstract class ObjectivesData
{
  [Key(0)]
  public Objectives.TYPES Type;
  [Key(1)]
  public string GroupId;
  [Key(2)]
  public bool IsComplete;
  [Key(3)]
  public bool IsFailed;
  [Key(4)]
  public bool FailLocked;
  [Key(5)]
  public bool AutoRemoveQuestOnceComplete = true;
  [Key(6)]
  public bool TargetFollowerAllowOldAge = true;
  [Key(7)]
  public float QuestCooldown = 12000f;
  [Key(8)]
  public float QuestExpireDuration = -1f;
  [Key(9)]
  public float ExpireTimestamp = -1f;
  [Key(10)]
  public int ID;
  [Key(11)]
  public int Index = -1;
  [Key(12)]
  public string UniqueGroupID = "";
  [CompilerGenerated]
  public int \u003CFollower\u003Ek__BackingField = -1;
  [Key(13)]
  public string CompleteTerm = "";
  [Key(14)]
  public string[] CompleteTermArguments;
  [IgnoreMember]
  public bool initialised;
  [Key(15)]
  public bool IsWinterObjective;
  [CompilerGenerated]
  public Objectives.TIMER_TYPE \u003CTimerType\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CAutoTrack\u003Ek__BackingField;
  [IgnoreMember]
  public string subtitle;

  [IgnoreMember]
  public abstract string Text { get; }

  [IgnoreMember]
  public bool HasExpiry
  {
    get => (double) this.ExpireTimestamp > -1.0 && !this.IsFailed && !this.IsComplete;
  }

  [IgnoreMember]
  public float ExpiryTimeNormalized
  {
    get => (this.ExpireTimestamp - TimeManager.TotalElapsedGameTime) / this.QuestExpireDuration;
  }

  [Key(25)]
  public int Follower
  {
    get => this.\u003CFollower\u003Ek__BackingField;
    set => this.\u003CFollower\u003Ek__BackingField = value;
  }

  [Key(26)]
  public virtual Objectives.TIMER_TYPE TimerType
  {
    get => this.\u003CTimerType\u003Ek__BackingField;
    set => this.\u003CTimerType\u003Ek__BackingField = value;
  }

  [IgnoreMember]
  public virtual bool AutoTrack => this.\u003CAutoTrack\u003Ek__BackingField;

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
    if ((((double) this.QuestExpireDuration == -1.0 ? 0 : ((double) this.ExpireTimestamp == -1.0 ? 1 : 0)) & (initialAssigning ? 1 : 0)) == 0)
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

  public virtual bool CheckComplete()
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
      bool isTracked = ObjectiveManager.IsTracked(this.UniqueGroupID);
      GameManager.GetInstance().WaitForSeconds(0.0f, (System.Action) (() => ObjectiveManager.InvokeOrQueue((System.Action) (() =>
      {
        List<ObjectivesData> objectivesDataList = new List<ObjectivesData>((IEnumerable<ObjectivesData>) Quests.GetUnCompletedFollowerQuests(this.Follower, this.GroupId));
        if (objectivesDataList.Contains(this))
          objectivesDataList.Remove(this);
        if (objectivesDataList.Count != 0)
          return;
        Objectives_TalkToFollower objective = new Objectives_TalkToFollower(this.GroupId);
        objective.CompleteTerm = this.CompleteTerm;
        objective.CompleteTermArguments = this.CompleteTermArguments;
        objective.Follower = this.Follower;
        ObjectiveManager.Add((ObjectivesData) objective, isTracked);
        FollowerManager.FindFollowerByID(objective.Follower)?.ShowCompletedQuestIcon(true);
        if (!DataManager.Instance.CompletedQuestFollowerIDs.Contains(this.Follower))
          DataManager.Instance.CompletedQuestFollowerIDs.Add(this.Follower);
        objective.CheckComplete();
      }))));
    }
    if (!this.IsComplete)
      DataManager.Instance.AddToCompletedQuestHistory(this.GetFinalizedData());
    this.IsComplete = true;
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

  public ObjectivesData Clone()
  {
    return MessagePackSerializer.Deserialize<ObjectivesData>(ReadOnlyMemory<byte>.op_Implicit(MessagePackSerializer.Serialize<ObjectivesData>(this, MPSerialization.options)), MPSerialization.options);
  }
}
