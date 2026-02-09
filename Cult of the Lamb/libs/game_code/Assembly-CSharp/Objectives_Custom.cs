// Decompiled with JetBrains decompiler
// Type: Objectives_Custom
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class Objectives_Custom : ObjectivesData
{
  [Key(16 /*0x10*/)]
  public Objectives.CustomQuestTypes CustomQuestType;
  [Key(17)]
  public int TargetFollowerID = -1;
  [Key(18)]
  public int ResultFollowerID = -1;
  [Key(19)]
  public List<FollowerTrait.TraitType> Traits = new List<FollowerTrait.TraitType>();

  public override string Text
  {
    get
    {
      string format = LocalizationManager.GetTranslation($"Objectives/Custom/{this.CustomQuestType}");
      if (this.CustomQuestType == Objectives.CustomQuestTypes.MysticShopReturn)
        format = string.Format(format, string.IsNullOrEmpty(DataManager.Instance.MysticKeeperName) ? (object) ScriptLocalization.NAMES.MysticShopSellerDefault : (object) DataManager.Instance.MysticKeeperName);
      else if (this.CustomQuestType == Objectives.CustomQuestTypes.BlessAFollower)
      {
        if (DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.WorkWorship_Intimidate))
          format = ScriptLocalization.DoctrineUpgradeSystem.WorkWorship_Intimidate;
        else if (DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.WorkWorship_Inspire))
          format = ScriptLocalization.DoctrineUpgradeSystem.WorkWorship_Inspire;
      }
      else if (this.CustomQuestType == Objectives.CustomQuestTypes.ReturnLastLambGhosts)
      {
        int num = 0 + ((DataManager.Instance.NPCGhostGeneric7Rescued ? 1 : 0) + (DataManager.Instance.NPCGhostGeneric8Rescued ? 1 : 0) + (DataManager.Instance.NPCGhostGeneric9Rescued ? 1 : 0) + (DataManager.Instance.NPCGhostGeneric10Rescued ? 1 : 0) + (DataManager.Instance.NPCGhostRancherRescued ? 1 : 0) + (DataManager.Instance.NPCGhostBlacksmithRescued ? 1 : 0) + (DataManager.Instance.NPCGhostTarotRescued ? 1 : 0) + (DataManager.Instance.NPCGhostDecoRescued ? 1 : 0) + (DataManager.Instance.NPCGhostFlockadeRescued ? 1 : 0) + (DataManager.Instance.NPCGhostGraveyardRescued ? 1 : 0));
        format = string.Format(LocalizationManager.GetTranslation($"Objectives/Custom/{this.CustomQuestType}"), (object) num, (object) 10);
      }
      else if (this.CustomQuestType == Objectives.CustomQuestTypes.DepositFollower && this.Traits.Count > 0)
      {
        if (this.Traits.Count == 1)
          format = string.Format(LocalizationManager.GetTranslation($"Objectives/Custom/{this.CustomQuestType}1"), (object) FollowerTrait.GetLocalizedTitle(this.Traits[0]).Colour(StaticColors.YellowColorHex));
        else if (this.Traits.Count == 2)
          format = string.Format(LocalizationManager.GetTranslation($"Objectives/Custom/{this.CustomQuestType}2"), (object) FollowerTrait.GetLocalizedTitle(this.Traits[0]).Colour(StaticColors.YellowColorHex), (object) FollowerTrait.GetLocalizedTitle(this.Traits[1]).Colour(StaticColors.YellowColorHex));
        else if (this.Traits.Count == 3)
          format = string.Format(LocalizationManager.GetTranslation($"Objectives/Custom/{this.CustomQuestType}3"), (object) FollowerTrait.GetLocalizedTitle(this.Traits[0]).Colour(StaticColors.YellowColorHex), (object) FollowerTrait.GetLocalizedTitle(this.Traits[1]).Colour(StaticColors.YellowColorHex), (object) FollowerTrait.GetLocalizedTitle(this.Traits[2]).Colour(StaticColors.YellowColorHex));
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
    if (customQuestType != Objectives.CustomQuestTypes.WaitForBlizzardToFinish)
      return;
    this.TimerType = Objectives.TIMER_TYPE.Large;
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

  public IEnumerator DelayedCompleteCheck()
  {
    Objectives_Custom objectivesCustom = this;
    yield return (object) new WaitForSeconds(0.5f);
    if (objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.CookFirstMeal && objectivesCustom.Follower != -1)
    {
      if (StructureManager.GetAllStructuresOfType(FollowerLocation.Base, StructureBrain.TYPES.MEAL).Count <= 0)
      {
        FollowerInfo infoById = FollowerInfo.GetInfoByID(objectivesCustom.Follower);
        if ((infoById != null ? ((double) infoById.Satiation > 60.0 ? 1 : 0) : 0) == 0)
          yield break;
      }
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.CookFirstMeal);
    }
    else if (objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.CollectDivineInspiration && UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Temple))
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.CollectDivineInspiration);
  }

  public override bool CheckComplete()
  {
    if (!this.IsFailed && this.TargetFollowerID != -1 && FollowerInfo.GetInfoByID(this.TargetFollowerID) == null && !this.FailLocked)
      this.Failed();
    if (this.CustomQuestType == Objectives.CustomQuestTypes.ReturnLastLambGhosts)
      return DataManager.Instance.NPCGhostGeneric7Rescued && DataManager.Instance.NPCGhostGeneric8Rescued && DataManager.Instance.NPCGhostGeneric9Rescued && DataManager.Instance.NPCGhostGeneric10Rescued && DataManager.Instance.NPCGhostRancherRescued && DataManager.Instance.NPCGhostTarotRescued && DataManager.Instance.NPCGhostGraveyardRescued && DataManager.Instance.NPCGhostDecoRescued && DataManager.Instance.NPCGhostFlockadeRescued && DataManager.Instance.NPCGhostBlacksmithRescued;
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

  public void OnFollowerDied(
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
    if (this.FailLocked)
      return;
    this.Failed();
  }

  [MessagePackObject(false)]
  [Serializable]
  public class FinalizedData_Custom : ObjectivesDataFinalized
  {
    [Key(3)]
    public Objectives.CustomQuestTypes CustomQuestType;
    [Key(4)]
    public string TargetFollowerName;
    [Key(5)]
    public List<FollowerTrait.TraitType> Traits = new List<FollowerTrait.TraitType>();

    public override string GetText()
    {
      string format = LocalizationManager.GetTranslation($"Objectives/Custom/{this.CustomQuestType}");
      if (this.CustomQuestType == Objectives.CustomQuestTypes.MysticShopReturn)
        format = string.Format(format, string.IsNullOrEmpty(DataManager.Instance.MysticKeeperName) ? (object) ScriptLocalization.NAMES.MysticShopSellerDefault : (object) DataManager.Instance.MysticKeeperName);
      else if (this.CustomQuestType == Objectives.CustomQuestTypes.ReturnLastLambGhosts)
        format = string.Format(LocalizationManager.GetTranslation($"Objectives/Custom/{this.CustomQuestType}"), (object) 10, (object) 10);
      else if (this.CustomQuestType == Objectives.CustomQuestTypes.DepositFollower && this.Traits.Count > 0)
      {
        if (this.Traits.Count == 1)
          format = string.Format(LocalizationManager.GetTranslation($"Objectives/Custom/{this.CustomQuestType}1"), (object) FollowerTrait.GetLocalizedTitle(this.Traits[0]).Colour(StaticColors.YellowColorHex));
        else if (this.Traits.Count == 2)
          format = string.Format(LocalizationManager.GetTranslation($"Objectives/Custom/{this.CustomQuestType}2"), (object) FollowerTrait.GetLocalizedTitle(this.Traits[0]).Colour(StaticColors.YellowColorHex), (object) FollowerTrait.GetLocalizedTitle(this.Traits[1]).Colour(StaticColors.YellowColorHex));
        else if (this.Traits.Count == 3)
          format = string.Format(LocalizationManager.GetTranslation($"Objectives/Custom/{this.CustomQuestType}3"), (object) FollowerTrait.GetLocalizedTitle(this.Traits[0]).Colour(StaticColors.YellowColorHex), (object) FollowerTrait.GetLocalizedTitle(this.Traits[1]).Colour(StaticColors.YellowColorHex), (object) FollowerTrait.GetLocalizedTitle(this.Traits[2]).Colour(StaticColors.YellowColorHex));
      }
      else
        format = string.Format(format, (object) this.TargetFollowerName);
      return format;
    }
  }
}
