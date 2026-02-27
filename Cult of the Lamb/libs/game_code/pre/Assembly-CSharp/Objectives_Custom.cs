// Decompiled with JetBrains decompiler
// Type: Objectives_Custom
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
[Serializable]
public class Objectives_Custom : ObjectivesData
{
  public Objectives.CustomQuestTypes CustomQuestType;
  public int TargetFollowerID = -1;
  public int ResultFollowerID = -1;

  public override string Text
  {
    get
    {
      string format = LocalizationManager.GetTranslation($"Objectives/Custom/{this.CustomQuestType}");
      if (this.CustomQuestType == Objectives.CustomQuestTypes.BlessAFollower)
      {
        if (DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.WorkWorship_Intimidate))
          format = ScriptLocalization.DoctrineUpgradeSystem.WorkWorship_Intimidate;
        else if (DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.WorkWorship_Inspire))
          format = ScriptLocalization.DoctrineUpgradeSystem.WorkWorship_Inspire;
      }
      return this.TargetFollowerID != -1 ? string.Format(format, (object) FollowerInfo.GetInfoByID(this.TargetFollowerID, true)?.Name) : format;
    }
  }

  public Objectives_Custom()
  {
  }

  public Objectives_Custom(
    string groupId,
    Objectives.CustomQuestTypes customQuestType,
    int targetFollowerID = -1,
    float questExpireDuration = -1f)
    : base(groupId, questExpireDuration)
  {
    this.Type = Objectives.TYPES.CUSTOM;
    this.CustomQuestType = customQuestType;
    this.TargetFollowerID = targetFollowerID;
  }

  public override void Init(bool initialAssigning)
  {
    if (!this.initialised)
      FollowerManager.OnFollowerDie += new FollowerManager.FollowerGoneEvent(this.OnFollowerDied);
    base.Init(initialAssigning);
    GameManager.GetInstance()?.StartCoroutine((IEnumerator) this.DelayedCompleteCheck());
  }

  public override ObjectivesDataFinalized GetFinalizedData()
  {
    Objectives_Custom.FinalizedData_Custom finalizedData = new Objectives_Custom.FinalizedData_Custom();
    finalizedData.GroupId = this.GroupId;
    finalizedData.Index = this.Index;
    finalizedData.CustomQuestType = this.CustomQuestType;
    finalizedData.TargetFollowerName = this.TargetFollowerID != -1 ? FollowerInfo.GetInfoByID(this.TargetFollowerID, true)?.Name : "";
    finalizedData.UniqueGroupID = this.UniqueGroupID;
    return (ObjectivesDataFinalized) finalizedData;
  }

  private IEnumerator DelayedCompleteCheck()
  {
    Objectives_Custom objectivesCustom = this;
    yield return (object) new WaitForSeconds(0.5f);
    if (objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.CookFirstMeal && objectivesCustom.Follower != -1)
    {
      if (StructureManager.GetAllStructuresOfType(FollowerLocation.Base, StructureBrain.TYPES.MEAL).Count > 0 || (double) FollowerInfo.GetInfoByID(objectivesCustom.Follower).Satiation > 60.0)
        ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.CookFirstMeal);
    }
    else if (objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.CollectDivineInspiration && UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Temple))
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.CollectDivineInspiration);
  }

  protected override bool CheckComplete()
  {
    if (!this.IsFailed && this.TargetFollowerID != -1 && FollowerInfo.GetInfoByID(this.TargetFollowerID) == null && !this.FailLocked)
      this.Failed();
    if (this.IsFailed)
      return false;
    if (this.ResultFollowerID == this.TargetFollowerID)
      return true;
    return this.TargetFollowerID == -1 && base.CheckComplete();
  }

  public override void Complete()
  {
    base.Complete();
    FollowerManager.OnFollowerDie -= new FollowerManager.FollowerGoneEvent(this.OnFollowerDied);
  }

  private void OnFollowerDied(
    int followerID,
    NotificationCentre.NotificationType notificationType)
  {
    if (this.CustomQuestType != Objectives.CustomQuestTypes.MurderFollower && this.CustomQuestType != Objectives.CustomQuestTypes.MurderFollowerAtNight || followerID != this.TargetFollowerID || this.IsFailed || this.IsComplete || this.FailLocked)
      return;
    this.Failed();
    FollowerManager.OnFollowerDie -= new FollowerManager.FollowerGoneEvent(this.OnFollowerDied);
  }

  public override void Update()
  {
    base.Update();
    if (this.IsFailed || this.IsComplete || this.TargetFollowerID == -1)
      return;
    if (!this.TargetFollowerAllowOldAge)
    {
      FollowerInfo infoById = FollowerInfo.GetInfoByID(this.TargetFollowerID);
      if ((infoById != null ? (infoById.CursedState == Thought.OldAge ? 1 : 0) : 0) != 0)
        goto label_4;
    }
    if (FollowerInfo.GetInfoByID(this.TargetFollowerID) != null)
      return;
label_4:
    this.Failed();
  }

  [Serializable]
  public class FinalizedData_Custom : ObjectivesDataFinalized
  {
    public Objectives.CustomQuestTypes CustomQuestType;
    public string TargetFollowerName;

    public override string GetText()
    {
      return string.Format(LocalizationManager.GetTranslation($"Objectives/Custom/{this.CustomQuestType}"), (object) this.TargetFollowerName);
    }
  }
}
