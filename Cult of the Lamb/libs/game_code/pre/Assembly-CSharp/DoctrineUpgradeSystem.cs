// Decompiled with JetBrains decompiler
// Type: DoctrineUpgradeSystem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

#nullable disable
public class DoctrineUpgradeSystem : BaseMonoBehaviour
{
  public const int kMaxSermonLevel = 4;
  public static Action<DoctrineUpgradeSystem.DoctrineType> OnDoctrineUnlocked;
  public static bool GiveInstantCheat;
  public static System.Action OnAbilityPointDelta;
  public const int BribeCost = 3;

  public static bool TryGetStillDoctrineStone()
  {
    return (0 + (4 - DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.Afterlife)) + (4 - DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.Food)) + (4 - DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.Possession)) + (4 - DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.WorkAndWorship)) + (4 - DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.LawAndOrder)) - DataManager.Instance.CompletedDoctrineStones) * 3 - DataManager.Instance.DoctrineCurrentCount > 0;
  }

  public static bool TrySermonsStillAvailable()
  {
    return (DataManager.Instance.dungeonRun >= 2 || DataManager.Instance.ForceDoctrineStones) && (DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.Afterlife) < 4 || DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.Food) < 4 || DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.Possession) < 4 || DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.WorkAndWorship) < 4 || DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.LawAndOrder) < 4 || DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.Special) < 1);
  }

  public static float GetXPBySermon(SermonCategory sermonCategory)
  {
    switch (sermonCategory)
    {
      case SermonCategory.Afterlife:
        return DataManager.Instance.Doctrine_Afterlife_XP;
      case SermonCategory.Food:
        return DataManager.Instance.Doctrine_Food_XP;
      case SermonCategory.LawAndOrder:
        return DataManager.Instance.Doctrine_LawAndOrder_XP;
      case SermonCategory.Possession:
        return DataManager.Instance.Doctrine_Possessions_XP;
      case SermonCategory.WorkAndWorship:
        return DataManager.Instance.Doctrine_WorkWorship_XP;
      case SermonCategory.PlayerUpgrade:
        return DataManager.Instance.Doctrine_PlayerUpgrade_XP;
      default:
        return 0.0f;
    }
  }

  public static float GetXPBySermonNormalised(SermonCategory sermonCategory)
  {
    switch (sermonCategory)
    {
      case SermonCategory.Afterlife:
        return DataManager.Instance.Doctrine_Afterlife_XP / DoctrineUpgradeSystem.GetXPTargetBySermon(sermonCategory);
      case SermonCategory.Food:
        return DataManager.Instance.Doctrine_Food_XP / DoctrineUpgradeSystem.GetXPTargetBySermon(sermonCategory);
      case SermonCategory.LawAndOrder:
        return DataManager.Instance.Doctrine_LawAndOrder_XP / DoctrineUpgradeSystem.GetXPTargetBySermon(sermonCategory);
      case SermonCategory.Possession:
        return DataManager.Instance.Doctrine_Possessions_XP / DoctrineUpgradeSystem.GetXPTargetBySermon(sermonCategory);
      case SermonCategory.WorkAndWorship:
        return DataManager.Instance.Doctrine_WorkWorship_XP / DoctrineUpgradeSystem.GetXPTargetBySermon(sermonCategory);
      case SermonCategory.Special:
        return DataManager.Instance.Doctrine_Special_XP / DoctrineUpgradeSystem.GetXPTargetBySermon(sermonCategory);
      case SermonCategory.PlayerUpgrade:
        return DataManager.Instance.Doctrine_PlayerUpgrade_XP / DoctrineUpgradeSystem.GetXPTargetBySermon(sermonCategory);
      default:
        return 0.0f;
    }
  }

  public static float GetXPTargetBySermon(SermonCategory sermonCategory)
  {
    switch (sermonCategory)
    {
      case SermonCategory.Afterlife:
        return DataManager.DoctrineTargetXP[Mathf.Min(DoctrineUpgradeSystem.GetLevelBySermon(sermonCategory), DataManager.DoctrineTargetXP.Count - 1)];
      case SermonCategory.Food:
        return DataManager.DoctrineTargetXP[Mathf.Min(DoctrineUpgradeSystem.GetLevelBySermon(sermonCategory), DataManager.DoctrineTargetXP.Count - 1)];
      case SermonCategory.LawAndOrder:
        return DataManager.DoctrineTargetXP[Mathf.Min(DoctrineUpgradeSystem.GetLevelBySermon(sermonCategory), DataManager.DoctrineTargetXP.Count - 1)];
      case SermonCategory.Possession:
        return DataManager.DoctrineTargetXP[Mathf.Min(DoctrineUpgradeSystem.GetLevelBySermon(sermonCategory), DataManager.DoctrineTargetXP.Count - 1)];
      case SermonCategory.WorkAndWorship:
        return DataManager.DoctrineTargetXP[Mathf.Min(DoctrineUpgradeSystem.GetLevelBySermon(sermonCategory), DataManager.DoctrineTargetXP.Count - 1)];
      case SermonCategory.Special:
        return (float) Ritual.FollowersAvailableToAttendSermon() * 0.1f;
      case SermonCategory.PlayerUpgrade:
        return DataManager.PlayerUpgradeTargetXP[Mathf.Min(DoctrineUpgradeSystem.GetLevelBySermon(sermonCategory), DataManager.PlayerUpgradeTargetXP.Count - 1)];
      default:
        return 0.0f;
    }
  }

  public static void SetXPBySermon(SermonCategory sermonCategory, float Value)
  {
    switch (sermonCategory)
    {
      case SermonCategory.Afterlife:
        DataManager.Instance.Doctrine_Afterlife_XP = Value;
        break;
      case SermonCategory.Food:
        DataManager.Instance.Doctrine_Food_XP = Value;
        break;
      case SermonCategory.LawAndOrder:
        DataManager.Instance.Doctrine_LawAndOrder_XP = Value;
        break;
      case SermonCategory.Possession:
        DataManager.Instance.Doctrine_Possessions_XP = Value;
        break;
      case SermonCategory.WorkAndWorship:
        DataManager.Instance.Doctrine_WorkWorship_XP = Value;
        break;
      case SermonCategory.Special:
        DataManager.Instance.Doctrine_Special_XP = Value;
        break;
      case SermonCategory.PlayerUpgrade:
        DataManager.Instance.Doctrine_PlayerUpgrade_XP = Value;
        break;
    }
  }

  public static int GetLevelBySermon(SermonCategory sermonCategory)
  {
    switch (sermonCategory)
    {
      case SermonCategory.Afterlife:
        return DataManager.Instance.Doctrine_Afterlife_Level;
      case SermonCategory.Food:
        return DataManager.Instance.Doctrine_Food_Level;
      case SermonCategory.LawAndOrder:
        return DataManager.Instance.Doctrine_LawAndOrder_Level;
      case SermonCategory.Possession:
        return DataManager.Instance.Doctrine_Possessions_Level;
      case SermonCategory.WorkAndWorship:
        return DataManager.Instance.Doctrine_WorkWorship_Level;
      case SermonCategory.Special:
        return DataManager.Instance.Doctrine_Special_Level;
      case SermonCategory.PlayerUpgrade:
        return DataManager.Instance.Doctrine_PlayerUpgrade_Level;
      default:
        return 0;
    }
  }

  public static void SetLevelBySermon(SermonCategory sermonCategory, int Value)
  {
    Debug.Log((object) "SET LEVEL!");
    switch (sermonCategory)
    {
      case SermonCategory.Afterlife:
        DataManager.Instance.Doctrine_Afterlife_Level = Value;
        break;
      case SermonCategory.Food:
        DataManager.Instance.Doctrine_Food_Level = Value;
        break;
      case SermonCategory.LawAndOrder:
        DataManager.Instance.Doctrine_LawAndOrder_Level = Value;
        break;
      case SermonCategory.Possession:
        DataManager.Instance.Doctrine_Possessions_Level = Value;
        break;
      case SermonCategory.WorkAndWorship:
        DataManager.Instance.Doctrine_WorkWorship_Level = Value;
        break;
      case SermonCategory.Special:
        DataManager.Instance.Doctrine_Special_Level = Value;
        break;
      case SermonCategory.PlayerUpgrade:
        DataManager.Instance.Doctrine_PlayerUpgrade_Level = Value;
        break;
    }
    if (DataManager.Instance.Doctrine_Afterlife_Level != 4 || DataManager.Instance.Doctrine_Food_Level != 4 || DataManager.Instance.Doctrine_LawAndOrder_Level != 4 || DataManager.Instance.Doctrine_Possessions_Level != 4 || DataManager.Instance.Doctrine_WorkWorship_Level != 4)
      return;
    AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("UPGRADE_ALL_SERMONS"));
  }

  public static string GetSermonCategoryIcon(SermonCategory Category)
  {
    switch (Category)
    {
      case SermonCategory.Afterlife:
        return "\uF714";
      case SermonCategory.Food:
        return "\uF623";
      case SermonCategory.LawAndOrder:
        return "\uF0E3";
      case SermonCategory.Possession:
        return "\uF890";
      case SermonCategory.WorkAndWorship:
        return "\uF683";
      case SermonCategory.Special:
        return "";
      case SermonCategory.PlayerUpgrade:
        return "";
      default:
        return "Uh Oh something happened";
    }
  }

  public static string GetSermonCategoryLocalizedName(SermonCategory Category)
  {
    return LocalizationManager.GetTranslation($"DoctrineUpgradeSystem/{Category}");
  }

  public static string GetSermonCategoryLocalizedDescription(SermonCategory Category)
  {
    return LocalizationManager.GetTranslation($"DoctrineUpgradeSystem/{Category}/Description");
  }

  public static SermonCategory GetCategory(DoctrineUpgradeSystem.DoctrineType d)
  {
    switch (d)
    {
      case DoctrineUpgradeSystem.DoctrineType.WorkWorship_Inspire:
      case DoctrineUpgradeSystem.DoctrineType.WorkWorship_Intimidate:
      case DoctrineUpgradeSystem.DoctrineType.WorkWorship_FasterBuilding:
      case DoctrineUpgradeSystem.DoctrineType.WorkWorship_Enlightenment:
      case DoctrineUpgradeSystem.DoctrineType.WorkWorship_FaithfulTrait:
      case DoctrineUpgradeSystem.DoctrineType.WorkWorship_GoodWorkerTrait:
      case DoctrineUpgradeSystem.DoctrineType.WorkWorship_WorkThroughNightRitual:
      case DoctrineUpgradeSystem.DoctrineType.WorkWorship_HolidayRitual:
        return SermonCategory.WorkAndWorship;
      case DoctrineUpgradeSystem.DoctrineType.Possessions_ExtortTithes:
      case DoctrineUpgradeSystem.DoctrineType.Possessions_Bribe:
      case DoctrineUpgradeSystem.DoctrineType.Possessions_MoreFaithFromHomes:
      case DoctrineUpgradeSystem.DoctrineType.Possessions_MoreFaithFromRituals:
      case DoctrineUpgradeSystem.DoctrineType.Possessions_TraitMaterialistic:
      case DoctrineUpgradeSystem.DoctrineType.Possessions_TraitFalseIdols:
      case DoctrineUpgradeSystem.DoctrineType.Possessions_AlmsToPoorRitual:
      case DoctrineUpgradeSystem.DoctrineType.Possessions_DonationRitual:
        return SermonCategory.Possession;
      case DoctrineUpgradeSystem.DoctrineType.Sustenance_Fast:
      case DoctrineUpgradeSystem.DoctrineType.Sustenance_Feast:
      case DoctrineUpgradeSystem.DoctrineType.Sustenance_TraitMushroomEncouraged:
      case DoctrineUpgradeSystem.DoctrineType.Sustenance_TraitMushroomBanned:
      case DoctrineUpgradeSystem.DoctrineType.Sustenance_TraitCannibal:
      case DoctrineUpgradeSystem.DoctrineType.Sustenance_TraitGrassEater:
      case DoctrineUpgradeSystem.DoctrineType.Sustenance_TraitHarvestRitual:
      case DoctrineUpgradeSystem.DoctrineType.Sustenance_TraitFishingRitual:
        return SermonCategory.Food;
      case DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_TraitSacrificeEnthusiast:
      case DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_TraitDesensitisedToDeath:
      case DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_RessurectionRitual:
      case DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_Funeral:
      case DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_TraitRespectElders:
      case DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_TraitOldDieYoung:
      case DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_BuildingReturnToEarth:
      case DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_BuildingGoodGraves:
        return SermonCategory.Afterlife;
      case DoctrineUpgradeSystem.DoctrineType.LawOrder_MurderFollower:
      case DoctrineUpgradeSystem.DoctrineType.LawOrder_AscendFollower:
      case DoctrineUpgradeSystem.DoctrineType.LawOrder_FightPitRitual:
      case DoctrineUpgradeSystem.DoctrineType.LawOrder_JudgementRitual:
      case DoctrineUpgradeSystem.DoctrineType.LawOrder_AssignFaithEnforcerRitual:
      case DoctrineUpgradeSystem.DoctrineType.LawOrder_AssignTaxCollectorRitual:
      case DoctrineUpgradeSystem.DoctrineType.LawOrder_TraitDisciplinarian:
      case DoctrineUpgradeSystem.DoctrineType.LawOrder_TraitLibertarian:
        return SermonCategory.LawAndOrder;
      default:
        return SermonCategory.None;
    }
  }

  public static DoctrineUpgradeSystem.DoctrineCategory ShowDoctrineTutorialForType(
    DoctrineUpgradeSystem.DoctrineType Type)
  {
    switch (Type)
    {
      case DoctrineUpgradeSystem.DoctrineType.WorkWorship_Inspire:
      case DoctrineUpgradeSystem.DoctrineType.WorkWorship_Intimidate:
      case DoctrineUpgradeSystem.DoctrineType.Possessions_ExtortTithes:
      case DoctrineUpgradeSystem.DoctrineType.Possessions_Bribe:
      case DoctrineUpgradeSystem.DoctrineType.LawOrder_MurderFollower:
      case DoctrineUpgradeSystem.DoctrineType.LawOrder_AscendFollower:
      case DoctrineUpgradeSystem.DoctrineType.Special_ReadMind:
        return DoctrineUpgradeSystem.DoctrineCategory.FollowerAction;
      case DoctrineUpgradeSystem.DoctrineType.WorkWorship_FasterBuilding:
      case DoctrineUpgradeSystem.DoctrineType.WorkWorship_Enlightenment:
      case DoctrineUpgradeSystem.DoctrineType.WorkWorship_WorkThroughNightRitual:
      case DoctrineUpgradeSystem.DoctrineType.WorkWorship_HolidayRitual:
      case DoctrineUpgradeSystem.DoctrineType.Possessions_AlmsToPoorRitual:
      case DoctrineUpgradeSystem.DoctrineType.Possessions_DonationRitual:
      case DoctrineUpgradeSystem.DoctrineType.Sustenance_Fast:
      case DoctrineUpgradeSystem.DoctrineType.Sustenance_Feast:
      case DoctrineUpgradeSystem.DoctrineType.Sustenance_TraitHarvestRitual:
      case DoctrineUpgradeSystem.DoctrineType.Sustenance_TraitFishingRitual:
      case DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_RessurectionRitual:
      case DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_Funeral:
      case DoctrineUpgradeSystem.DoctrineType.LawOrder_FightPitRitual:
      case DoctrineUpgradeSystem.DoctrineType.LawOrder_JudgementRitual:
      case DoctrineUpgradeSystem.DoctrineType.LawOrder_AssignFaithEnforcerRitual:
      case DoctrineUpgradeSystem.DoctrineType.LawOrder_AssignTaxCollectorRitual:
      case DoctrineUpgradeSystem.DoctrineType.Special_Brainwashed:
      case DoctrineUpgradeSystem.DoctrineType.Special_Sacrifice:
      case DoctrineUpgradeSystem.DoctrineType.Special_Bonfire:
        return DoctrineUpgradeSystem.DoctrineCategory.Ritual;
      case DoctrineUpgradeSystem.DoctrineType.WorkWorship_FaithfulTrait:
      case DoctrineUpgradeSystem.DoctrineType.WorkWorship_GoodWorkerTrait:
      case DoctrineUpgradeSystem.DoctrineType.Possessions_MoreFaithFromHomes:
      case DoctrineUpgradeSystem.DoctrineType.Possessions_MoreFaithFromRituals:
      case DoctrineUpgradeSystem.DoctrineType.Possessions_TraitMaterialistic:
      case DoctrineUpgradeSystem.DoctrineType.Possessions_TraitFalseIdols:
      case DoctrineUpgradeSystem.DoctrineType.Sustenance_TraitMushroomEncouraged:
      case DoctrineUpgradeSystem.DoctrineType.Sustenance_TraitMushroomBanned:
      case DoctrineUpgradeSystem.DoctrineType.Sustenance_TraitCannibal:
      case DoctrineUpgradeSystem.DoctrineType.Sustenance_TraitGrassEater:
      case DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_TraitSacrificeEnthusiast:
      case DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_TraitDesensitisedToDeath:
      case DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_TraitRespectElders:
      case DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_TraitOldDieYoung:
      case DoctrineUpgradeSystem.DoctrineType.LawOrder_TraitDisciplinarian:
      case DoctrineUpgradeSystem.DoctrineType.LawOrder_TraitLibertarian:
        return DoctrineUpgradeSystem.DoctrineCategory.Trait;
      default:
        return DoctrineUpgradeSystem.DoctrineCategory.None;
    }
  }

  public static DoctrineUpgradeSystem.DoctrineType GetSermonReward(
    SermonCategory sermonCategory,
    int Level,
    bool FirstChoice)
  {
    switch (sermonCategory)
    {
      case SermonCategory.Afterlife:
        switch (Level)
        {
          case 1:
            return !FirstChoice ? DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_TraitDesensitisedToDeath : DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_TraitSacrificeEnthusiast;
          case 2:
            return !FirstChoice ? DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_Funeral : DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_RessurectionRitual;
          case 3:
            return !FirstChoice ? DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_TraitOldDieYoung : DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_TraitRespectElders;
          case 4:
            return !FirstChoice ? DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_BuildingGoodGraves : DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_BuildingReturnToEarth;
        }
        break;
      case SermonCategory.Food:
        switch (Level)
        {
          case 1:
            return !FirstChoice ? DoctrineUpgradeSystem.DoctrineType.Sustenance_Feast : DoctrineUpgradeSystem.DoctrineType.Sustenance_Fast;
          case 2:
            return !FirstChoice ? DoctrineUpgradeSystem.DoctrineType.Sustenance_TraitGrassEater : DoctrineUpgradeSystem.DoctrineType.Sustenance_TraitCannibal;
          case 3:
            return !FirstChoice ? DoctrineUpgradeSystem.DoctrineType.Sustenance_TraitFishingRitual : DoctrineUpgradeSystem.DoctrineType.Sustenance_TraitHarvestRitual;
          case 4:
            return !FirstChoice ? DoctrineUpgradeSystem.DoctrineType.Sustenance_TraitMushroomBanned : DoctrineUpgradeSystem.DoctrineType.Sustenance_TraitMushroomEncouraged;
        }
        break;
      case SermonCategory.LawAndOrder:
        switch (Level)
        {
          case 1:
            return !FirstChoice ? DoctrineUpgradeSystem.DoctrineType.LawOrder_AscendFollower : DoctrineUpgradeSystem.DoctrineType.LawOrder_MurderFollower;
          case 2:
            return !FirstChoice ? DoctrineUpgradeSystem.DoctrineType.LawOrder_JudgementRitual : DoctrineUpgradeSystem.DoctrineType.LawOrder_FightPitRitual;
          case 3:
            return !FirstChoice ? DoctrineUpgradeSystem.DoctrineType.LawOrder_TraitLibertarian : DoctrineUpgradeSystem.DoctrineType.LawOrder_TraitDisciplinarian;
          case 4:
            return !FirstChoice ? DoctrineUpgradeSystem.DoctrineType.LawOrder_AssignTaxCollectorRitual : DoctrineUpgradeSystem.DoctrineType.LawOrder_AssignFaithEnforcerRitual;
        }
        break;
      case SermonCategory.Possession:
        switch (Level)
        {
          case 1:
            return !FirstChoice ? DoctrineUpgradeSystem.DoctrineType.Possessions_Bribe : DoctrineUpgradeSystem.DoctrineType.Possessions_ExtortTithes;
          case 2:
            return !FirstChoice ? DoctrineUpgradeSystem.DoctrineType.Possessions_TraitFalseIdols : DoctrineUpgradeSystem.DoctrineType.Possessions_TraitMaterialistic;
          case 3:
            return !FirstChoice ? DoctrineUpgradeSystem.DoctrineType.Possessions_DonationRitual : DoctrineUpgradeSystem.DoctrineType.Possessions_AlmsToPoorRitual;
          case 4:
            return !FirstChoice ? DoctrineUpgradeSystem.DoctrineType.Possessions_MoreFaithFromRituals : DoctrineUpgradeSystem.DoctrineType.Possessions_MoreFaithFromHomes;
        }
        break;
      case SermonCategory.WorkAndWorship:
        switch (Level)
        {
          case 1:
            return !FirstChoice ? DoctrineUpgradeSystem.DoctrineType.WorkWorship_GoodWorkerTrait : DoctrineUpgradeSystem.DoctrineType.WorkWorship_FaithfulTrait;
          case 2:
            return !FirstChoice ? DoctrineUpgradeSystem.DoctrineType.WorkWorship_Intimidate : DoctrineUpgradeSystem.DoctrineType.WorkWorship_Inspire;
          case 3:
            return !FirstChoice ? DoctrineUpgradeSystem.DoctrineType.WorkWorship_Enlightenment : DoctrineUpgradeSystem.DoctrineType.WorkWorship_FasterBuilding;
          case 4:
            return !FirstChoice ? DoctrineUpgradeSystem.DoctrineType.WorkWorship_HolidayRitual : DoctrineUpgradeSystem.DoctrineType.WorkWorship_WorkThroughNightRitual;
        }
        break;
      case SermonCategory.Special:
        switch (Level)
        {
          case 1:
            return DoctrineUpgradeSystem.DoctrineType.Special_Bonfire;
          case 2:
            return DoctrineUpgradeSystem.DoctrineType.Special_ReadMind;
          case 3:
            return DoctrineUpgradeSystem.DoctrineType.Special_Sacrifice;
          case 4:
            return DoctrineUpgradeSystem.DoctrineType.Special_Brainwashed;
        }
        break;
    }
    return DoctrineUpgradeSystem.DoctrineType.None;
  }

  public static List<DoctrineUpgradeSystem.DoctrineType> UnlockedUpgrades
  {
    get => DataManager.Instance.DoctrineUnlockedUpgrades;
    set => DataManager.Instance.DoctrineUnlockedUpgrades = value;
  }

  public static List<DoctrineUpgradeSystem.DoctrineType> GetUnlockedDoctrinesForCategory(
    SermonCategory sermonCategory)
  {
    List<DoctrineUpgradeSystem.DoctrineType> doctrinesForCategory = new List<DoctrineUpgradeSystem.DoctrineType>();
    foreach (DoctrineUpgradeSystem.DoctrineType doctrineUnlockedUpgrade in DataManager.Instance.DoctrineUnlockedUpgrades)
    {
      if (DoctrineUpgradeSystem.GetCategory(doctrineUnlockedUpgrade) == sermonCategory)
        doctrinesForCategory.Add(doctrineUnlockedUpgrade);
    }
    return doctrinesForCategory;
  }

  public static string GetLocalizedName(DoctrineUpgradeSystem.DoctrineType Type)
  {
    return LocalizationManager.GetTranslation($"DoctrineUpgradeSystem/{Type}");
  }

  public static string GetLocalizedDescription(DoctrineUpgradeSystem.DoctrineType Type)
  {
    return Type == DoctrineUpgradeSystem.DoctrineType.Special_Bonfire ? $"{LocalizationManager.GetTranslation($"DoctrineUpgradeSystem/{Type}/Description")}<br><sprite name=\"icon_Faith\">{(" +" + (object) UpgradeSystem.GetRitualFaithChange(UpgradeSystem.Type.Ritual_FirePit)).Colour(StaticColors.GreenColor)}" : LocalizationManager.GetTranslation($"DoctrineUpgradeSystem/{Type}/Description");
  }

  public static bool GetUnlocked(DoctrineUpgradeSystem.DoctrineType Type)
  {
    return DoctrineUpgradeSystem.UnlockedUpgrades.Contains(Type);
  }

  private static Sprite GetIcon(string icon)
  {
    return Resources.Load<SpriteAtlas>("Atlases/DoctrineAbilityIcons").GetSprite(icon);
  }

  public static Sprite GetIcon(DoctrineUpgradeSystem.DoctrineType type)
  {
    return DoctrineUpgradeSystem.GetIcon(type.ToString());
  }

  public static Sprite GetIconForRitual(UpgradeSystem.Type type)
  {
    string icon = "";
    switch (type)
    {
      case UpgradeSystem.Type.Ritual_Sacrifice:
        icon = DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_TraitSacrificeEnthusiast.ToString();
        break;
      case UpgradeSystem.Type.Ritual_Reindoctrinate:
        icon = "Ritual_ReindoctrinateFollower";
        break;
      case UpgradeSystem.Type.Ritual_ConsumeFollower:
        icon = DoctrineUpgradeSystem.DoctrineType.Sustenance_TraitCannibal.ToString();
        break;
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful1:
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful2:
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful3:
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful4:
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful5:
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful6:
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful7:
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful8:
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful9:
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful10:
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful11:
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful12:
        icon = "Ritual_HeartsOfTheFaithful";
        break;
      case UpgradeSystem.Type.Ritual_UnlockWeapon:
        icon = UpgradeSystem.Type.Ritual_UnlockWeapon.ToString();
        break;
      case UpgradeSystem.Type.Ritual_UnlockCurse:
        icon = UpgradeSystem.Type.Ritual_UnlockCurse.ToString();
        break;
      case UpgradeSystem.Type.Ritual_FasterBuilding:
        icon = DoctrineUpgradeSystem.DoctrineType.WorkWorship_FasterBuilding.ToString();
        break;
      case UpgradeSystem.Type.Ritual_Enlightenment:
        icon = DoctrineUpgradeSystem.DoctrineType.WorkWorship_Enlightenment.ToString();
        break;
      case UpgradeSystem.Type.Ritual_WorkThroughNight:
        icon = DoctrineUpgradeSystem.DoctrineType.WorkWorship_WorkThroughNightRitual.ToString();
        break;
      case UpgradeSystem.Type.Ritual_Holiday:
        icon = DoctrineUpgradeSystem.DoctrineType.WorkWorship_HolidayRitual.ToString();
        break;
      case UpgradeSystem.Type.Ritual_AlmsToPoor:
        icon = DoctrineUpgradeSystem.DoctrineType.Possessions_AlmsToPoorRitual.ToString();
        break;
      case UpgradeSystem.Type.Ritual_DonationRitual:
        icon = DoctrineUpgradeSystem.DoctrineType.Possessions_DonationRitual.ToString();
        break;
      case UpgradeSystem.Type.Ritual_Fast:
        icon = DoctrineUpgradeSystem.DoctrineType.Sustenance_Fast.ToString();
        break;
      case UpgradeSystem.Type.Ritual_Feast:
        icon = DoctrineUpgradeSystem.DoctrineType.Sustenance_Feast.ToString();
        break;
      case UpgradeSystem.Type.Ritual_HarvestRitual:
        icon = DoctrineUpgradeSystem.DoctrineType.Sustenance_TraitHarvestRitual.ToString();
        break;
      case UpgradeSystem.Type.Ritual_FishingRitual:
        icon = DoctrineUpgradeSystem.DoctrineType.Sustenance_TraitFishingRitual.ToString();
        break;
      case UpgradeSystem.Type.Ritual_Ressurect:
        icon = DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_RessurectionRitual.ToString();
        break;
      case UpgradeSystem.Type.Ritual_Funeral:
        icon = DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_Funeral.ToString();
        break;
      case UpgradeSystem.Type.Ritual_Fightpit:
        icon = DoctrineUpgradeSystem.DoctrineType.LawOrder_FightPitRitual.ToString();
        break;
      case UpgradeSystem.Type.Ritual_Wedding:
        icon = DoctrineUpgradeSystem.DoctrineType.LawOrder_JudgementRitual.ToString();
        break;
      case UpgradeSystem.Type.Ritual_AssignFaithEnforcer:
        icon = DoctrineUpgradeSystem.DoctrineType.LawOrder_AssignFaithEnforcerRitual.ToString();
        break;
      case UpgradeSystem.Type.Ritual_AssignTaxCollector:
        icon = DoctrineUpgradeSystem.DoctrineType.LawOrder_AssignTaxCollectorRitual.ToString();
        break;
      case UpgradeSystem.Type.Ritual_Brainwashing:
        icon = DoctrineUpgradeSystem.DoctrineType.Special_Brainwashed.ToString();
        break;
      case UpgradeSystem.Type.Ritual_Ascend:
        icon = DoctrineUpgradeSystem.DoctrineType.LawOrder_AscendFollower.ToString();
        break;
      case UpgradeSystem.Type.Ritual_FirePit:
        icon = DoctrineUpgradeSystem.DoctrineType.Special_Bonfire.ToString();
        break;
      case UpgradeSystem.Type.Ritual_Halloween:
        icon = DoctrineUpgradeSystem.DoctrineType.Special_Halloween.ToString();
        break;
    }
    return DoctrineUpgradeSystem.GetIcon(icon);
  }

  public static bool UnlockAbility(DoctrineUpgradeSystem.DoctrineType Type)
  {
    if (DoctrineUpgradeSystem.UnlockedUpgrades.Contains(Type))
      return false;
    DoctrineUpgradeSystem.UnlockedUpgrades.Add(Type);
    DataManager.Instance.Alerts.Doctrine.Add(Type);
    DoctrineUpgradeSystem.OnUnlockAbility(Type);
    return true;
  }

  private static void OnUnlockAbility(DoctrineUpgradeSystem.DoctrineType Type)
  {
    Debug.Log((object) ("UNLOCKED!! " + (object) Type));
    switch (Type)
    {
      case DoctrineUpgradeSystem.DoctrineType.WorkWorship_FasterBuilding:
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_FasterBuilding);
        break;
      case DoctrineUpgradeSystem.DoctrineType.WorkWorship_Enlightenment:
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Enlightenment);
        break;
      case DoctrineUpgradeSystem.DoctrineType.WorkWorship_FaithfulTrait:
        FollowerTrait.AddCultTrait(FollowerTrait.TraitType.Faithful);
        break;
      case DoctrineUpgradeSystem.DoctrineType.WorkWorship_GoodWorkerTrait:
        FollowerTrait.AddCultTrait(FollowerTrait.TraitType.Industrious);
        break;
      case DoctrineUpgradeSystem.DoctrineType.WorkWorship_WorkThroughNightRitual:
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_WorkThroughNight);
        break;
      case DoctrineUpgradeSystem.DoctrineType.WorkWorship_HolidayRitual:
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Holiday);
        JudgementMeter.ShowModify(1);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Possessions_MoreFaithFromHomes:
        FollowerTrait.AddCultTrait(FollowerTrait.TraitType.ConstructionEnthusiast);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Possessions_MoreFaithFromRituals:
        FollowerTrait.AddCultTrait(FollowerTrait.TraitType.SermonEnthusiast);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Possessions_TraitMaterialistic:
        FollowerTrait.AddCultTrait(FollowerTrait.TraitType.Materialistic);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Possessions_TraitFalseIdols:
        FollowerTrait.AddCultTrait(FollowerTrait.TraitType.FalseIdols);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Possessions_AlmsToPoorRitual:
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_AlmsToPoor);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Possessions_DonationRitual:
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_DonationRitual);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Sustenance_Fast:
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Fast);
        JudgementMeter.ShowModify(-1);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Sustenance_Feast:
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Feast);
        JudgementMeter.ShowModify(1);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Sustenance_TraitMushroomEncouraged:
        FollowerTrait.AddCultTrait(FollowerTrait.TraitType.MushroomEncouraged);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Sustenance_TraitMushroomBanned:
        FollowerTrait.AddCultTrait(FollowerTrait.TraitType.MushroomBanned);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Sustenance_TraitCannibal:
        FollowerTrait.AddCultTrait(FollowerTrait.TraitType.Cannibal);
        JudgementMeter.ShowModify(-1);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Sustenance_TraitGrassEater:
        FollowerTrait.AddCultTrait(FollowerTrait.TraitType.GrassEater);
        JudgementMeter.ShowModify(1);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Sustenance_TraitHarvestRitual:
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_HarvestRitual);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Sustenance_TraitFishingRitual:
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_FishingRitual);
        break;
      case DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_TraitSacrificeEnthusiast:
        FollowerTrait.AddCultTrait(FollowerTrait.TraitType.SacrificeEnthusiast);
        break;
      case DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_TraitDesensitisedToDeath:
        FollowerTrait.AddCultTrait(FollowerTrait.TraitType.DesensitisedToDeath);
        break;
      case DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_RessurectionRitual:
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Ressurect);
        JudgementMeter.ShowModify(-1);
        break;
      case DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_Funeral:
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Funeral);
        JudgementMeter.ShowModify(1);
        break;
      case DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_TraitRespectElders:
        FollowerTrait.AddCultTrait(FollowerTrait.TraitType.LoveElderly);
        JudgementMeter.ShowModify(1);
        break;
      case DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_TraitOldDieYoung:
        FollowerTrait.AddCultTrait(FollowerTrait.TraitType.HateElderly);
        JudgementMeter.ShowModify(-1);
        break;
      case DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_BuildingReturnToEarth:
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Building_NaturalBurial);
        JudgementMeter.ShowModify(-1);
        break;
      case DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_BuildingGoodGraves:
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Building_Graves);
        JudgementMeter.ShowModify(1);
        break;
      case DoctrineUpgradeSystem.DoctrineType.LawOrder_MurderFollower:
        JudgementMeter.ShowModify(-1);
        break;
      case DoctrineUpgradeSystem.DoctrineType.LawOrder_AscendFollower:
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Ascend);
        JudgementMeter.ShowModify(1);
        break;
      case DoctrineUpgradeSystem.DoctrineType.LawOrder_FightPitRitual:
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Fightpit);
        JudgementMeter.ShowModify(-1);
        break;
      case DoctrineUpgradeSystem.DoctrineType.LawOrder_JudgementRitual:
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Wedding);
        JudgementMeter.ShowModify(1);
        break;
      case DoctrineUpgradeSystem.DoctrineType.LawOrder_AssignFaithEnforcerRitual:
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_AssignFaithEnforcer);
        break;
      case DoctrineUpgradeSystem.DoctrineType.LawOrder_AssignTaxCollectorRitual:
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_AssignTaxCollector);
        break;
      case DoctrineUpgradeSystem.DoctrineType.LawOrder_TraitDisciplinarian:
        FollowerTrait.AddCultTrait(FollowerTrait.TraitType.Disciplinarian);
        JudgementMeter.ShowModify(-1);
        break;
      case DoctrineUpgradeSystem.DoctrineType.LawOrder_TraitLibertarian:
        FollowerTrait.AddCultTrait(FollowerTrait.TraitType.Libertarian);
        JudgementMeter.ShowModify(1);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Special_Brainwashed:
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Brainwashing);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Special_Sacrifice:
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Sacrifice);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Special_Consume:
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_ConsumeFollower);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Special_ReadMind:
        DataManager.Instance.CanReadMinds = true;
        break;
      case DoctrineUpgradeSystem.DoctrineType.Special_Bonfire:
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_FirePit);
        break;
    }
    Action<DoctrineUpgradeSystem.DoctrineType> doctrineUnlocked = DoctrineUpgradeSystem.OnDoctrineUnlocked;
    if (doctrineUnlocked == null)
      return;
    doctrineUnlocked(Type);
  }

  public static UpgradeSystem.Type RitualForDoctrineUpgrade(DoctrineUpgradeSystem.DoctrineType type)
  {
    switch (type)
    {
      case DoctrineUpgradeSystem.DoctrineType.WorkWorship_FasterBuilding:
        return UpgradeSystem.Type.Ritual_FasterBuilding;
      case DoctrineUpgradeSystem.DoctrineType.WorkWorship_Enlightenment:
        return UpgradeSystem.Type.Ritual_Enlightenment;
      case DoctrineUpgradeSystem.DoctrineType.WorkWorship_WorkThroughNightRitual:
        return UpgradeSystem.Type.Ritual_WorkThroughNight;
      case DoctrineUpgradeSystem.DoctrineType.WorkWorship_HolidayRitual:
        return UpgradeSystem.Type.Ritual_Holiday;
      case DoctrineUpgradeSystem.DoctrineType.Possessions_AlmsToPoorRitual:
        return UpgradeSystem.Type.Ritual_AlmsToPoor;
      case DoctrineUpgradeSystem.DoctrineType.Possessions_DonationRitual:
        return UpgradeSystem.Type.Ritual_DonationRitual;
      case DoctrineUpgradeSystem.DoctrineType.Sustenance_Fast:
        return UpgradeSystem.Type.Ritual_Fast;
      case DoctrineUpgradeSystem.DoctrineType.Sustenance_Feast:
        return UpgradeSystem.Type.Ritual_Feast;
      case DoctrineUpgradeSystem.DoctrineType.Sustenance_TraitHarvestRitual:
        return UpgradeSystem.Type.Ritual_HarvestRitual;
      case DoctrineUpgradeSystem.DoctrineType.Sustenance_TraitFishingRitual:
        return UpgradeSystem.Type.Ritual_FishingRitual;
      case DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_RessurectionRitual:
        return UpgradeSystem.Type.Ritual_Ressurect;
      case DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_Funeral:
        return UpgradeSystem.Type.Ritual_Funeral;
      case DoctrineUpgradeSystem.DoctrineType.LawOrder_AscendFollower:
        return UpgradeSystem.Type.Ritual_Ascend;
      case DoctrineUpgradeSystem.DoctrineType.LawOrder_FightPitRitual:
        return UpgradeSystem.Type.Ritual_Fightpit;
      case DoctrineUpgradeSystem.DoctrineType.LawOrder_JudgementRitual:
        return UpgradeSystem.Type.Ritual_Wedding;
      case DoctrineUpgradeSystem.DoctrineType.LawOrder_AssignFaithEnforcerRitual:
        return UpgradeSystem.Type.Ritual_AssignFaithEnforcer;
      case DoctrineUpgradeSystem.DoctrineType.LawOrder_AssignTaxCollectorRitual:
        return UpgradeSystem.Type.Ritual_AssignTaxCollector;
      case DoctrineUpgradeSystem.DoctrineType.Special_Brainwashed:
        return UpgradeSystem.Type.Ritual_Brainwashing;
      case DoctrineUpgradeSystem.DoctrineType.Special_Sacrifice:
        return UpgradeSystem.Type.Ritual_Sacrifice;
      case DoctrineUpgradeSystem.DoctrineType.Special_Consume:
        return UpgradeSystem.Type.Ritual_ConsumeFollower;
      case DoctrineUpgradeSystem.DoctrineType.Special_Bonfire:
        return UpgradeSystem.Type.Ritual_FirePit;
      default:
        return UpgradeSystem.Type.Count;
    }
  }

  public static string GetDoctrineUnlockString(DoctrineUpgradeSystem.DoctrineType Type)
  {
    switch (DoctrineUpgradeSystem.GetUnlockType(Type))
    {
      case DoctrineUpgradeSystem.DoctrineUnlockType.None:
        return "";
      case DoctrineUpgradeSystem.DoctrineUnlockType.Ritual:
        return ScriptLocalization.DoctrineUpgradeSystem.UnlockType_Ritual;
      case DoctrineUpgradeSystem.DoctrineUnlockType.FollowerAbility:
        return ScriptLocalization.DoctrineUpgradeSystem.UnlockType_FollowerAbility;
      case DoctrineUpgradeSystem.DoctrineUnlockType.Building:
        return ScriptLocalization.DoctrineUpgradeSystem.UnlockType_Building;
      case DoctrineUpgradeSystem.DoctrineUnlockType.Trait:
        return ScriptLocalization.DoctrineUpgradeSystem.UnlockType_Trait;
      default:
        return "";
    }
  }

  public static string GetDoctrineUnlockIcon(DoctrineUpgradeSystem.DoctrineType Type)
  {
    switch (DoctrineUpgradeSystem.GetUnlockType(Type))
    {
      case DoctrineUpgradeSystem.DoctrineUnlockType.None:
        return "";
      case DoctrineUpgradeSystem.DoctrineUnlockType.Ritual:
        return "\uF755";
      case DoctrineUpgradeSystem.DoctrineUnlockType.FollowerAbility:
        return "\uF683";
      case DoctrineUpgradeSystem.DoctrineUnlockType.Building:
        return "\uF6E3";
      case DoctrineUpgradeSystem.DoctrineUnlockType.Trait:
        return "\uF118";
      default:
        return "";
    }
  }

  private static DoctrineUpgradeSystem.DoctrineUnlockType GetUnlockType(
    DoctrineUpgradeSystem.DoctrineType Type)
  {
    switch (Type)
    {
      case DoctrineUpgradeSystem.DoctrineType.WorkWorship_Inspire:
      case DoctrineUpgradeSystem.DoctrineType.WorkWorship_Intimidate:
      case DoctrineUpgradeSystem.DoctrineType.Special_ReadMind:
        return DoctrineUpgradeSystem.DoctrineUnlockType.FollowerAbility;
      case DoctrineUpgradeSystem.DoctrineType.WorkWorship_FasterBuilding:
      case DoctrineUpgradeSystem.DoctrineType.WorkWorship_Enlightenment:
      case DoctrineUpgradeSystem.DoctrineType.WorkWorship_WorkThroughNightRitual:
      case DoctrineUpgradeSystem.DoctrineType.WorkWorship_HolidayRitual:
      case DoctrineUpgradeSystem.DoctrineType.Possessions_AlmsToPoorRitual:
      case DoctrineUpgradeSystem.DoctrineType.Possessions_DonationRitual:
      case DoctrineUpgradeSystem.DoctrineType.Sustenance_Fast:
      case DoctrineUpgradeSystem.DoctrineType.Sustenance_Feast:
      case DoctrineUpgradeSystem.DoctrineType.Sustenance_TraitHarvestRitual:
      case DoctrineUpgradeSystem.DoctrineType.Sustenance_TraitFishingRitual:
      case DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_RessurectionRitual:
      case DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_Funeral:
      case DoctrineUpgradeSystem.DoctrineType.LawOrder_FightPitRitual:
      case DoctrineUpgradeSystem.DoctrineType.LawOrder_JudgementRitual:
      case DoctrineUpgradeSystem.DoctrineType.LawOrder_AssignFaithEnforcerRitual:
      case DoctrineUpgradeSystem.DoctrineType.LawOrder_AssignTaxCollectorRitual:
      case DoctrineUpgradeSystem.DoctrineType.Special_Sacrifice:
      case DoctrineUpgradeSystem.DoctrineType.Special_Consume:
      case DoctrineUpgradeSystem.DoctrineType.Special_Bonfire:
        return DoctrineUpgradeSystem.DoctrineUnlockType.Ritual;
      case DoctrineUpgradeSystem.DoctrineType.WorkWorship_FaithfulTrait:
      case DoctrineUpgradeSystem.DoctrineType.WorkWorship_GoodWorkerTrait:
      case DoctrineUpgradeSystem.DoctrineType.Possessions_TraitMaterialistic:
      case DoctrineUpgradeSystem.DoctrineType.Possessions_TraitFalseIdols:
      case DoctrineUpgradeSystem.DoctrineType.Sustenance_TraitMushroomEncouraged:
      case DoctrineUpgradeSystem.DoctrineType.Sustenance_TraitMushroomBanned:
      case DoctrineUpgradeSystem.DoctrineType.Sustenance_TraitCannibal:
      case DoctrineUpgradeSystem.DoctrineType.Sustenance_TraitGrassEater:
      case DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_TraitSacrificeEnthusiast:
      case DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_TraitDesensitisedToDeath:
      case DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_TraitRespectElders:
      case DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_TraitOldDieYoung:
      case DoctrineUpgradeSystem.DoctrineType.LawOrder_TraitDisciplinarian:
      case DoctrineUpgradeSystem.DoctrineType.LawOrder_TraitLibertarian:
        return DoctrineUpgradeSystem.DoctrineUnlockType.Trait;
      case DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_BuildingGoodGraves:
        return DoctrineUpgradeSystem.DoctrineUnlockType.Building;
      default:
        Debug.Log((object) $"Uh oh {(object) Type} Hasn't been set an unlock type");
        return DoctrineUpgradeSystem.DoctrineUnlockType.None;
    }
  }

  public enum DoctrineType
  {
    None,
    Category_Afterlife,
    Category_Food,
    Category_LawAndOrder,
    Category_Possession,
    Category_WorkAndWorship,
    WorkWorship_Inspire,
    WorkWorship_Intimidate,
    WorkWorship_FasterBuilding,
    WorkWorship_Enlightenment,
    WorkWorship_FaithfulTrait,
    WorkWorship_GoodWorkerTrait,
    WorkWorship_WorkThroughNightRitual,
    WorkWorship_HolidayRitual,
    Possessions_ExtortTithes,
    Possessions_Bribe,
    Possessions_MoreFaithFromHomes,
    Possessions_MoreFaithFromRituals,
    Possessions_TraitMaterialistic,
    Possessions_TraitFalseIdols,
    Possessions_AlmsToPoorRitual,
    Possessions_DonationRitual,
    Sustenance_Fast,
    Sustenance_Feast,
    Sustenance_TraitMushroomEncouraged,
    Sustenance_TraitMushroomBanned,
    Sustenance_TraitCannibal,
    Sustenance_TraitGrassEater,
    Sustenance_TraitHarvestRitual,
    Sustenance_TraitFishingRitual,
    DeathSacrifice_TraitSacrificeEnthusiast,
    DeathSacrifice_TraitDesensitisedToDeath,
    DeathSacrifice_RessurectionRitual,
    DeathSacrifice_Funeral,
    DeathSacrifice_TraitRespectElders,
    DeathSacrifice_TraitOldDieYoung,
    DeathSacrifice_BuildingReturnToEarth,
    DeathSacrifice_BuildingGoodGraves,
    LawOrder_MurderFollower,
    LawOrder_AscendFollower,
    LawOrder_FightPitRitual,
    LawOrder_JudgementRitual,
    LawOrder_AssignFaithEnforcerRitual,
    LawOrder_AssignTaxCollectorRitual,
    LawOrder_TraitDisciplinarian,
    LawOrder_TraitLibertarian,
    Special_Brainwashed,
    Special_Sacrifice,
    Special_Consume,
    Special_ReadMind,
    Special_Bonfire,
    Special_Halloween,
    Count,
  }

  public enum DoctrineCategory
  {
    None,
    Trait,
    FollowerAction,
    Ritual,
  }

  public enum DoctrineUnlockType
  {
    None,
    Ritual,
    FollowerAbility,
    Building,
    Trait,
  }
}
