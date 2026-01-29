// Decompiled with JetBrains decompiler
// Type: DoctrineUpgradeSystem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

#nullable disable
public class DoctrineUpgradeSystem
{
  public const int kMaxSermonLevel = 4;
  public static Action<DoctrineUpgradeSystem.DoctrineType> OnDoctrineUnlocked;
  public static SpriteAtlas doctrineIcons;
  public static AsyncOperationHandle<SpriteAtlas> handle;
  public static bool GiveInstantCheat;
  public static System.Action OnAbilityPointDelta;
  public const int BribeCost = 3;

  public static void Initialise()
  {
    if ((UnityEngine.Object) DoctrineUpgradeSystem.doctrineIcons != (UnityEngine.Object) null)
      return;
    DoctrineUpgradeSystem.handle = Addressables.LoadAssetAsync<SpriteAtlas>((object) "DoctrineAbilityIcons");
    DoctrineUpgradeSystem.handle.Completed += (Action<AsyncOperationHandle<SpriteAtlas>>) (asyncOperation =>
    {
      if (asyncOperation.Status == AsyncOperationStatus.Succeeded)
        DoctrineUpgradeSystem.doctrineIcons = asyncOperation.Result;
      else
        Debug.LogError((object) "Failed to load SpriteAtlas using Addressables");
    });
  }

  public static void DeInitialise()
  {
    if (!DoctrineUpgradeSystem.handle.IsValid())
      return;
    Addressables.Release<SpriteAtlas>(DoctrineUpgradeSystem.handle);
  }

  public static bool TryGetStillDoctrineStone()
  {
    int num = 0 + (4 - DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.Afterlife)) + (4 - DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.Food)) + (4 - DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.Possession)) + (4 - DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.WorkAndWorship)) + (4 - DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.LawAndOrder));
    if (DataManager.Instance.PleasureDoctrineEnabled)
      num += 4 - DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.Pleasure);
    if (DataManager.Instance.WinterDoctrineAvailable)
      num += 4 - DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.Winter);
    return (num - DataManager.Instance.CompletedDoctrineStones) * 3 - DataManager.Instance.DoctrineCurrentCount > 0;
  }

  public static bool TrySermonsStillAvailable()
  {
    return (DataManager.Instance.dungeonRun >= 2 || DataManager.Instance.ForceDoctrineStones) && (DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.Afterlife) < 4 || DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.Food) < 4 || DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.Possession) < 4 || DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.WorkAndWorship) < 4 || DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.LawAndOrder) < 4 || DataManager.Instance.PleasureDoctrineEnabled && DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.Pleasure) < 4 || DataManager.Instance.WinterDoctrineAvailable && DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.Winter) < 4 || DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.Special) < 1);
  }

  public static int TotalRemainingDoctrinesAvailable()
  {
    int num = 40 - DoctrineUpgradeSystem.GetUnlockedDoctrinesForCategory(SermonCategory.Afterlife).Count - DoctrineUpgradeSystem.GetUnlockedDoctrinesForCategory(SermonCategory.Food).Count - DoctrineUpgradeSystem.GetUnlockedDoctrinesForCategory(SermonCategory.Possession).Count - DoctrineUpgradeSystem.GetUnlockedDoctrinesForCategory(SermonCategory.WorkAndWorship).Count - DoctrineUpgradeSystem.GetUnlockedDoctrinesForCategory(SermonCategory.LawAndOrder).Count;
    if (DataManager.Instance.PleasureDoctrineEnabled)
      num = num + 8 - DoctrineUpgradeSystem.GetUnlockedDoctrinesForCategory(SermonCategory.Pleasure).Count;
    if (DataManager.Instance.WinterDoctrineEnabled)
      num = num + 8 - DoctrineUpgradeSystem.GetUnlockedDoctrinesForCategory(SermonCategory.Winter).Count;
    return num;
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
      case SermonCategory.Pleasure:
        return DataManager.Instance.Doctrine_Pleasure_XP;
      case SermonCategory.Winter:
        return DataManager.Instance.Doctrine_Winter_XP;
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
      case SermonCategory.Pleasure:
        return DataManager.Instance.Doctrine_Pleasure_XP / DoctrineUpgradeSystem.GetXPTargetBySermon(sermonCategory);
      case SermonCategory.Winter:
        return DataManager.Instance.Doctrine_Winter_XP / DoctrineUpgradeSystem.GetXPTargetBySermon(sermonCategory);
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
      case SermonCategory.Pleasure:
        return DataManager.PlayerUpgradeTargetXP[Mathf.Min(DoctrineUpgradeSystem.GetLevelBySermon(sermonCategory), DataManager.DoctrineTargetXP.Count - 1)];
      case SermonCategory.Winter:
        return DataManager.PlayerUpgradeTargetXP[Mathf.Min(DoctrineUpgradeSystem.GetLevelBySermon(sermonCategory), DataManager.DoctrineTargetXP.Count - 1)];
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
      case SermonCategory.Pleasure:
        DataManager.Instance.Doctrine_Pleasure_XP = Value;
        break;
      case SermonCategory.Winter:
        DataManager.Instance.Doctrine_Winter_XP = Value;
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
      case SermonCategory.Pleasure:
        return DataManager.Instance.Doctrine_Pleasure_Level;
      case SermonCategory.Winter:
        return DataManager.Instance.Doctrine_Winter_Level;
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
      case SermonCategory.Pleasure:
        DataManager.Instance.Doctrine_Pleasure_Level = Value;
        break;
      case SermonCategory.Winter:
        DataManager.Instance.Doctrine_Winter_Level = Value;
        break;
    }
    if (DataManager.Instance.Doctrine_Afterlife_Level != 4 || DataManager.Instance.Doctrine_Food_Level != 4 || DataManager.Instance.Doctrine_LawAndOrder_Level != 4 || DataManager.Instance.Doctrine_Possessions_Level != 4 || DataManager.Instance.Doctrine_WorkWorship_Level != 4 || DataManager.Instance.Doctrine_Pleasure_Level != 4)
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
      case SermonCategory.Pleasure:
        return "<sprite name=\"icon_SinDoctrine\">";
      case SermonCategory.Winter:
        return "<sprite name=\"icon_Freezing\">";
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

  public static DoctrineUpgradeSystem.DoctrineType[] GetDoctrinesForCategory(
    SermonCategory sermonCategory)
  {
    switch (sermonCategory)
    {
      case SermonCategory.Afterlife:
        return new DoctrineUpgradeSystem.DoctrineType[8]
        {
          DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_TraitSacrificeEnthusiast,
          DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_TraitDesensitisedToDeath,
          DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_RessurectionRitual,
          DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_Funeral,
          DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_TraitRespectElders,
          DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_TraitOldDieYoung,
          DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_BuildingReturnToEarth,
          DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_BuildingGoodGraves
        };
      case SermonCategory.Food:
        return new DoctrineUpgradeSystem.DoctrineType[8]
        {
          DoctrineUpgradeSystem.DoctrineType.Sustenance_Fast,
          DoctrineUpgradeSystem.DoctrineType.Sustenance_Feast,
          DoctrineUpgradeSystem.DoctrineType.Sustenance_TraitMushroomEncouraged,
          DoctrineUpgradeSystem.DoctrineType.Sustenance_TraitMushroomBanned,
          DoctrineUpgradeSystem.DoctrineType.Sustenance_TraitCannibal,
          DoctrineUpgradeSystem.DoctrineType.Sustenance_TraitGrassEater,
          DoctrineUpgradeSystem.DoctrineType.Sustenance_TraitHarvestRitual,
          DoctrineUpgradeSystem.DoctrineType.Sustenance_TraitFishingRitual
        };
      case SermonCategory.LawAndOrder:
        return new DoctrineUpgradeSystem.DoctrineType[8]
        {
          DoctrineUpgradeSystem.DoctrineType.LawOrder_MurderFollower,
          DoctrineUpgradeSystem.DoctrineType.LawOrder_AscendFollower,
          DoctrineUpgradeSystem.DoctrineType.LawOrder_FightPitRitual,
          DoctrineUpgradeSystem.DoctrineType.LawOrder_JudgementRitual,
          DoctrineUpgradeSystem.DoctrineType.LawOrder_AssignFaithEnforcerRitual,
          DoctrineUpgradeSystem.DoctrineType.LawOrder_AssignTaxCollectorRitual,
          DoctrineUpgradeSystem.DoctrineType.LawOrder_TraitDisciplinarian,
          DoctrineUpgradeSystem.DoctrineType.LawOrder_TraitLibertarian
        };
      case SermonCategory.Possession:
        return new DoctrineUpgradeSystem.DoctrineType[8]
        {
          DoctrineUpgradeSystem.DoctrineType.Possessions_ExtortTithes,
          DoctrineUpgradeSystem.DoctrineType.Possessions_Bribe,
          DoctrineUpgradeSystem.DoctrineType.Possessions_MoreFaithFromHomes,
          DoctrineUpgradeSystem.DoctrineType.Possessions_MoreFaithFromRituals,
          DoctrineUpgradeSystem.DoctrineType.Possessions_TraitMaterialistic,
          DoctrineUpgradeSystem.DoctrineType.Possessions_TraitFalseIdols,
          DoctrineUpgradeSystem.DoctrineType.Possessions_AlmsToPoorRitual,
          DoctrineUpgradeSystem.DoctrineType.Possessions_DonationRitual
        };
      case SermonCategory.WorkAndWorship:
        return new DoctrineUpgradeSystem.DoctrineType[8]
        {
          DoctrineUpgradeSystem.DoctrineType.WorkWorship_Inspire,
          DoctrineUpgradeSystem.DoctrineType.WorkWorship_Intimidate,
          DoctrineUpgradeSystem.DoctrineType.WorkWorship_FasterBuilding,
          DoctrineUpgradeSystem.DoctrineType.WorkWorship_Enlightenment,
          DoctrineUpgradeSystem.DoctrineType.WorkWorship_FaithfulTrait,
          DoctrineUpgradeSystem.DoctrineType.WorkWorship_GoodWorkerTrait,
          DoctrineUpgradeSystem.DoctrineType.WorkWorship_WorkThroughNightRitual,
          DoctrineUpgradeSystem.DoctrineType.WorkWorship_HolidayRitual
        };
      case SermonCategory.Pleasure:
        return new DoctrineUpgradeSystem.DoctrineType[8]
        {
          DoctrineUpgradeSystem.DoctrineType.Pleasure_Nudist,
          DoctrineUpgradeSystem.DoctrineType.Pleasure_Purge,
          DoctrineUpgradeSystem.DoctrineType.Pleasure_AtoneSin,
          DoctrineUpgradeSystem.DoctrineType.Pleasure_Cannibal,
          DoctrineUpgradeSystem.DoctrineType.Pleasure_Doctrinal_Extremist,
          DoctrineUpgradeSystem.DoctrineType.Pleasure_Violent_Extremist,
          DoctrineUpgradeSystem.DoctrineType.Pleasure_Fertility,
          DoctrineUpgradeSystem.DoctrineType.Pleasure_Allegiance
        };
      case SermonCategory.Winter:
        return new DoctrineUpgradeSystem.DoctrineType[8]
        {
          DoctrineUpgradeSystem.DoctrineType.Winter_FurnaceFollower,
          DoctrineUpgradeSystem.DoctrineType.Winter_FurnaceAnimal,
          DoctrineUpgradeSystem.DoctrineType.Winter_ConvertToRot,
          DoctrineUpgradeSystem.DoctrineType.Winter_RemoveRot,
          DoctrineUpgradeSystem.DoctrineType.Winter_WorkThroughBlizzard_Trait,
          DoctrineUpgradeSystem.DoctrineType.Winter_ColdEnthusiast_Trait,
          DoctrineUpgradeSystem.DoctrineType.Winter_RanchHarvest,
          DoctrineUpgradeSystem.DoctrineType.Winter_RanchMeat
        };
      default:
        return (DoctrineUpgradeSystem.DoctrineType[]) null;
    }
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
      case DoctrineUpgradeSystem.DoctrineType.Pleasure_Nudist:
      case DoctrineUpgradeSystem.DoctrineType.Pleasure_Purge:
      case DoctrineUpgradeSystem.DoctrineType.Pleasure_Doctrinal_Extremist:
      case DoctrineUpgradeSystem.DoctrineType.Pleasure_Violent_Extremist:
      case DoctrineUpgradeSystem.DoctrineType.Pleasure_Fertility:
      case DoctrineUpgradeSystem.DoctrineType.Pleasure_Allegiance:
      case DoctrineUpgradeSystem.DoctrineType.Pleasure_Cannibal:
      case DoctrineUpgradeSystem.DoctrineType.Pleasure_AtoneSin:
        return SermonCategory.Pleasure;
      case DoctrineUpgradeSystem.DoctrineType.Winter_WorkThroughBlizzard_Trait:
      case DoctrineUpgradeSystem.DoctrineType.Winter_ColdEnthusiast_Trait:
      case DoctrineUpgradeSystem.DoctrineType.Winter_RanchHarvest:
      case DoctrineUpgradeSystem.DoctrineType.Winter_RanchMeat:
      case DoctrineUpgradeSystem.DoctrineType.Winter_FurnaceFollower:
      case DoctrineUpgradeSystem.DoctrineType.Winter_FurnaceAnimal:
      case DoctrineUpgradeSystem.DoctrineType.Winter_ConvertToRot:
      case DoctrineUpgradeSystem.DoctrineType.Winter_RemoveRot:
        return SermonCategory.Winter;
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
          case 5:
            return DoctrineUpgradeSystem.DoctrineType.Winter_Snowman_Ritual;
          case 6:
            return DoctrineUpgradeSystem.DoctrineType.Winter_Follower_Wedding_Ritual;
          case 7:
            return DoctrineUpgradeSystem.DoctrineType.Special_EmbraceRot;
          case 8:
            return DoctrineUpgradeSystem.DoctrineType.Special_RejectRot;
          case 9:
            return DoctrineUpgradeSystem.DoctrineType.Special_HealingTouch;
        }
        break;
      case SermonCategory.Pleasure:
        switch (Level)
        {
          case 1:
            return !FirstChoice ? DoctrineUpgradeSystem.DoctrineType.Pleasure_Purge : DoctrineUpgradeSystem.DoctrineType.Pleasure_Nudist;
          case 2:
            return !FirstChoice ? DoctrineUpgradeSystem.DoctrineType.Pleasure_Cannibal : DoctrineUpgradeSystem.DoctrineType.Pleasure_AtoneSin;
          case 3:
            return !FirstChoice ? DoctrineUpgradeSystem.DoctrineType.Pleasure_Violent_Extremist : DoctrineUpgradeSystem.DoctrineType.Pleasure_Doctrinal_Extremist;
          case 4:
            return !FirstChoice ? DoctrineUpgradeSystem.DoctrineType.Pleasure_Allegiance : DoctrineUpgradeSystem.DoctrineType.Pleasure_Fertility;
        }
        break;
      case SermonCategory.Winter:
        switch (Level)
        {
          case 1:
            return !FirstChoice ? DoctrineUpgradeSystem.DoctrineType.Winter_FurnaceFollower : DoctrineUpgradeSystem.DoctrineType.Winter_FurnaceAnimal;
          case 2:
            return !FirstChoice ? DoctrineUpgradeSystem.DoctrineType.Winter_RemoveRot : DoctrineUpgradeSystem.DoctrineType.Winter_ConvertToRot;
          case 3:
            return !FirstChoice ? DoctrineUpgradeSystem.DoctrineType.Winter_WorkThroughBlizzard_Trait : DoctrineUpgradeSystem.DoctrineType.Winter_ColdEnthusiast_Trait;
          case 4:
            return !FirstChoice ? DoctrineUpgradeSystem.DoctrineType.Winter_RanchHarvest : DoctrineUpgradeSystem.DoctrineType.Winter_RanchMeat;
        }
        break;
    }
    if (Level < 5 || DoctrineUpgradeSystem.GetRemainingDoctrines(sermonCategory).Count <= 0)
      return DoctrineUpgradeSystem.DoctrineType.None;
    if (FirstChoice)
      return DoctrineUpgradeSystem.GetRemainingDoctrines(sermonCategory)[0];
    return DoctrineUpgradeSystem.GetRemainingDoctrines(sermonCategory).Count <= 1 ? DoctrineUpgradeSystem.DoctrineType.None : DoctrineUpgradeSystem.GetRemainingDoctrines(sermonCategory)[1];
  }

  public static List<DoctrineUpgradeSystem.DoctrineType> GetRemainingDoctrines(
    SermonCategory sermonCategory)
  {
    List<DoctrineUpgradeSystem.DoctrineType> remainingDoctrines = new List<DoctrineUpgradeSystem.DoctrineType>();
    for (int Level = 1; Level < 5; ++Level)
    {
      DoctrineUpgradeSystem.DoctrineType sermonReward1 = DoctrineUpgradeSystem.GetSermonReward(sermonCategory, Level, true);
      DoctrineUpgradeSystem.DoctrineType sermonReward2 = DoctrineUpgradeSystem.GetSermonReward(sermonCategory, Level, false);
      if (DoctrineUpgradeSystem.UnlockedUpgrades.Contains(sermonReward1) || DoctrineUpgradeSystem.UnlockedUpgrades.Contains(sermonReward2))
      {
        if (!DoctrineUpgradeSystem.UnlockedUpgrades.Contains(sermonReward1))
          remainingDoctrines.Add(sermonReward1);
        if (!DoctrineUpgradeSystem.UnlockedUpgrades.Contains(sermonReward2))
          remainingDoctrines.Add(sermonReward2);
      }
    }
    return remainingDoctrines;
  }

  public static List<DoctrineUpgradeSystem.DoctrineType> GetAllRemainingDoctrines()
  {
    List<DoctrineUpgradeSystem.DoctrineType> remainingDoctrines = new List<DoctrineUpgradeSystem.DoctrineType>();
    remainingDoctrines.AddRange((IEnumerable<DoctrineUpgradeSystem.DoctrineType>) DoctrineUpgradeSystem.GetRemainingDoctrines(SermonCategory.WorkAndWorship));
    remainingDoctrines.AddRange((IEnumerable<DoctrineUpgradeSystem.DoctrineType>) DoctrineUpgradeSystem.GetRemainingDoctrines(SermonCategory.Food));
    remainingDoctrines.AddRange((IEnumerable<DoctrineUpgradeSystem.DoctrineType>) DoctrineUpgradeSystem.GetRemainingDoctrines(SermonCategory.Afterlife));
    remainingDoctrines.AddRange((IEnumerable<DoctrineUpgradeSystem.DoctrineType>) DoctrineUpgradeSystem.GetRemainingDoctrines(SermonCategory.LawAndOrder));
    remainingDoctrines.AddRange((IEnumerable<DoctrineUpgradeSystem.DoctrineType>) DoctrineUpgradeSystem.GetRemainingDoctrines(SermonCategory.Possession));
    if (DataManager.Instance.PleasureDoctrineEnabled)
      remainingDoctrines.AddRange((IEnumerable<DoctrineUpgradeSystem.DoctrineType>) DoctrineUpgradeSystem.GetRemainingDoctrines(SermonCategory.Pleasure));
    if (DataManager.Instance.WinterDoctrineEnabled)
      remainingDoctrines.AddRange((IEnumerable<DoctrineUpgradeSystem.DoctrineType>) DoctrineUpgradeSystem.GetRemainingDoctrines(SermonCategory.Winter));
    return remainingDoctrines;
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
    if (Type == DoctrineUpgradeSystem.DoctrineType.Special_Bonfire && UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ritual_FirePit_2))
      Type = DoctrineUpgradeSystem.DoctrineType.Special_Bonfire_2;
    return LocalizationManager.GetTranslation($"DoctrineUpgradeSystem/{Type}");
  }

  public static string GetLocalizedDescription(DoctrineUpgradeSystem.DoctrineType Type)
  {
    if (Type != DoctrineUpgradeSystem.DoctrineType.Special_Bonfire)
      return LocalizationManager.GetTranslation($"DoctrineUpgradeSystem/{Type}/Description");
    if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ritual_FirePit_2))
      Type = DoctrineUpgradeSystem.DoctrineType.Special_Bonfire_2;
    return $"{LocalizationManager.GetTranslation($"DoctrineUpgradeSystem/{Type}/Description")}<br><sprite name=\"icon_Faith\">{(" +" + UpgradeSystem.GetRitualFaithChange(UpgradeSystem.Type.Ritual_FirePit).ToString()).Colour(StaticColors.GreenColor)}";
  }

  public static bool GetUnlocked(DoctrineUpgradeSystem.DoctrineType Type)
  {
    return DoctrineUpgradeSystem.UnlockedUpgrades.Contains(Type);
  }

  public static Sprite GetIcon(string icon) => DoctrineUpgradeSystem.doctrineIcons.GetSprite(icon);

  public static Sprite GetIcon(DoctrineUpgradeSystem.DoctrineType type)
  {
    return DoctrineUpgradeSystem.GetIcon(type.ToString());
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

  public static void OnUnlockAbility(DoctrineUpgradeSystem.DoctrineType Type)
  {
    Debug.Log((object) ("UNLOCKED!! " + Type.ToString()));
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
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Divorce);
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
      case DoctrineUpgradeSystem.DoctrineType.Special_BecomeDisciple:
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_BecomeDisciple);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Pleasure_Nudist:
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Nudism);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Pleasure_Purge:
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Purge);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Pleasure_Doctrinal_Extremist:
        FollowerTrait.AddCultTrait(FollowerTrait.TraitType.DoctrinalExtremist);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Pleasure_Violent_Extremist:
        FollowerTrait.AddCultTrait(FollowerTrait.TraitType.ViolentExtremist);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Pleasure_Fertility:
        FollowerTrait.AddCultTrait(FollowerTrait.TraitType.Fertility);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Pleasure_Allegiance:
        FollowerTrait.AddCultTrait(FollowerTrait.TraitType.Allegiance);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Pleasure_Cannibal:
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Cannibal);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Pleasure_AtoneSin:
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_AtoneSin);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Winter_RitualWarmth:
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Warmth);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Winter_RitualMidwinter:
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Midwinter);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Winter_Furnace_Full:
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Upgrade_Furnace_Full);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Winter_WorkThroughBlizzard_Trait:
        FollowerTrait.AddCultTrait(FollowerTrait.TraitType.WorkThroughBlizzard);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Winter_ColdEnthusiast_Trait:
        FollowerTrait.AddCultTrait(FollowerTrait.TraitType.ColdEnthusiast);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Winter_Rotstone_Spread:
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Upgrade_Rotstone_Spread);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Winter_Divorce_Ritual:
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Divorce);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Winter_Follower_Wedding_Ritual:
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Divorce);
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_FollowerWedding);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Winter_Snowman_Ritual:
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Snowman);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Winter_RanchHarvest:
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_RanchHarvest);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Winter_RanchMeat:
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_RanchMeat);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Winter_FurnaceFollower:
        FollowerTrait.AddCultTrait(FollowerTrait.TraitType.FurnaceFollower);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Winter_FurnaceAnimal:
        FollowerTrait.AddCultTrait(FollowerTrait.TraitType.FurnaceAnimal);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Winter_ConvertToRot:
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_ConvertToRot);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Special_EmbraceRot:
        FollowerTrait.AddCultTrait(FollowerTrait.TraitType.EmbraceRot);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Special_RejectRot:
        FollowerTrait.AddCultTrait(FollowerTrait.TraitType.RejectRot);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Winter_RemoveRot:
        FollowerTrait.AddCultTrait(FollowerTrait.TraitType.RemoveRot);
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
      case DoctrineUpgradeSystem.DoctrineType.Special_BecomeDisciple:
        return UpgradeSystem.Type.Ritual_BecomeDisciple;
      case DoctrineUpgradeSystem.DoctrineType.Pleasure_Nudist:
        return UpgradeSystem.Type.Ritual_Nudism;
      case DoctrineUpgradeSystem.DoctrineType.Pleasure_Purge:
        return UpgradeSystem.Type.Ritual_Purge;
      case DoctrineUpgradeSystem.DoctrineType.Pleasure_Cannibal:
        return UpgradeSystem.Type.Ritual_Cannibal;
      case DoctrineUpgradeSystem.DoctrineType.Pleasure_AtoneSin:
        return UpgradeSystem.Type.Ritual_AtoneSin;
      case DoctrineUpgradeSystem.DoctrineType.Winter_Snowman_Ritual:
        return UpgradeSystem.Type.Ritual_Snowman;
      case DoctrineUpgradeSystem.DoctrineType.Winter_RanchHarvest:
        return UpgradeSystem.Type.Ritual_RanchHarvest;
      case DoctrineUpgradeSystem.DoctrineType.Winter_ConvertToRot:
        return UpgradeSystem.Type.Ritual_ConvertToRot;
      case DoctrineUpgradeSystem.DoctrineType.Winter_RemoveRot:
        return UpgradeSystem.Type.Winter_RemoveRot;
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

  public static DoctrineUpgradeSystem.DoctrineUnlockType GetUnlockType(
    DoctrineUpgradeSystem.DoctrineType Type)
  {
    switch (Type)
    {
      case DoctrineUpgradeSystem.DoctrineType.WorkWorship_Inspire:
      case DoctrineUpgradeSystem.DoctrineType.WorkWorship_Intimidate:
      case DoctrineUpgradeSystem.DoctrineType.Special_ReadMind:
      case DoctrineUpgradeSystem.DoctrineType.Special_HealingTouch:
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
      case DoctrineUpgradeSystem.DoctrineType.Pleasure_Nudist:
      case DoctrineUpgradeSystem.DoctrineType.Pleasure_Purge:
      case DoctrineUpgradeSystem.DoctrineType.Pleasure_Cannibal:
      case DoctrineUpgradeSystem.DoctrineType.Pleasure_AtoneSin:
      case DoctrineUpgradeSystem.DoctrineType.Winter_RanchHarvest:
      case DoctrineUpgradeSystem.DoctrineType.Winter_RanchMeat:
      case DoctrineUpgradeSystem.DoctrineType.Winter_ConvertToRot:
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
      case DoctrineUpgradeSystem.DoctrineType.Pleasure_Doctrinal_Extremist:
      case DoctrineUpgradeSystem.DoctrineType.Pleasure_Violent_Extremist:
      case DoctrineUpgradeSystem.DoctrineType.Pleasure_Fertility:
      case DoctrineUpgradeSystem.DoctrineType.Pleasure_Allegiance:
      case DoctrineUpgradeSystem.DoctrineType.Winter_WorkThroughBlizzard_Trait:
      case DoctrineUpgradeSystem.DoctrineType.Winter_ColdEnthusiast_Trait:
      case DoctrineUpgradeSystem.DoctrineType.Winter_FurnaceFollower:
      case DoctrineUpgradeSystem.DoctrineType.Winter_FurnaceAnimal:
      case DoctrineUpgradeSystem.DoctrineType.Winter_RemoveRot:
        return DoctrineUpgradeSystem.DoctrineUnlockType.Trait;
      case DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_BuildingReturnToEarth:
      case DoctrineUpgradeSystem.DoctrineType.DeathSacrifice_BuildingGoodGraves:
        return DoctrineUpgradeSystem.DoctrineUnlockType.Building;
      default:
        Debug.Log((object) $"Uh oh {Type.ToString()} Hasn't been set an unlock type");
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
    Special_BecomeDisciple,
    Pleasure_Nudist,
    Pleasure_Purge,
    Pleasure_Doctrinal_Extremist,
    Pleasure_Violent_Extremist,
    Pleasure_Fertility,
    Pleasure_Allegiance,
    Pleasure_Cannibal,
    Pleasure_AtoneSin,
    Winter_RitualWarmth,
    Winter_RitualMidwinter,
    Winter_Furnace_Full,
    Winter_WorkThroughBlizzard_Trait,
    Winter_ColdEnthusiast_Trait,
    Winter_Rotstone_Spread,
    Winter_Divorce_Ritual,
    Winter_Follower_Wedding_Ritual,
    Winter_Snowman_Ritual,
    Winter_RanchHarvest,
    Winter_RanchMeat,
    Winter_FurnaceFollower,
    Winter_FurnaceAnimal,
    Winter_ConvertToRot,
    Special_Bonfire_2,
    Special_EmbraceRot,
    Special_RejectRot,
    Winter_RemoveRot,
    Special_HealingTouch,
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
