// Decompiled with JetBrains decompiler
// Type: TailorManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class TailorManager
{
  public static global::ClothingData[] clothingData;
  public const string ClothingPath = "Data/Equipment Data/Clothing";
  public static List<FollowerClothingType> postBerithQuestDLCOutfits = new List<FollowerClothingType>()
  {
    FollowerClothingType.Normal_MajorDLC_1,
    FollowerClothingType.Normal_MajorDLC_3,
    FollowerClothingType.Normal_MajorDLC_4,
    FollowerClothingType.Normal_MajorDLC_5
  };

  public static global::ClothingData[] ClothingData
  {
    get
    {
      if (TailorManager.clothingData == null)
        TailorManager.clothingData = Resources.LoadAll<global::ClothingData>("Data/Equipment Data/Clothing");
      return TailorManager.clothingData;
    }
  }

  public static global::ClothingData GetClothingData(FollowerClothingType clothingType)
  {
    foreach (global::ClothingData clothingData in TailorManager.ClothingData)
    {
      if (clothingData.ClothingType == clothingType)
        return clothingData;
    }
    return (global::ClothingData) null;
  }

  public static int GetClothingCount(
    bool includeNormal,
    bool includeSpecial,
    bool includeDLC,
    bool includeMajorDLC)
  {
    int clothingCount = 0;
    foreach (global::ClothingData clothingData in TailorManager.ClothingData)
    {
      if (includeNormal && !clothingData.SpecialClothing && !clothingData.IsDLC && !clothingData.IsMajorDLC)
        ++clothingCount;
      if (includeSpecial && clothingData.SpecialClothing && !clothingData.IsDLC && !clothingData.IsMajorDLC)
        ++clothingCount;
      if (includeDLC && clothingData.IsDLC && !clothingData.IsMajorDLC)
        ++clothingCount;
      if (includeMajorDLC && clothingData.IsMajorDLC)
        ++clothingCount;
    }
    return clothingCount;
  }

  public static int GetUnlockedClothingCount(
    bool includeNormal,
    bool includeSpecial,
    bool includeDLC,
    bool includeMajorDLC)
  {
    List<global::ClothingData> clothingDataList = new List<global::ClothingData>();
    foreach (global::ClothingData clothingData in TailorManager.ClothingData)
    {
      if (includeNormal && !clothingData.SpecialClothing && !clothingData.IsDLC && DataManager.Instance.UnlockedClothing.Contains(clothingData.ClothingType))
        clothingDataList.Add(clothingData);
      if (includeSpecial && clothingData.SpecialClothing && !clothingData.IsDLC && DataManager.Instance.UnlockedClothing.Contains(clothingData.ClothingType))
        clothingDataList.Add(clothingData);
      if (includeDLC && clothingData.IsDLC && DataManager.Instance.UnlockedClothing.Contains(clothingData.ClothingType))
        clothingDataList.Add(clothingData);
      if (includeMajorDLC && clothingData.IsMajorDLC && DataManager.Instance.UnlockedClothing.Contains(clothingData.ClothingType))
        clothingDataList.Add(clothingData);
    }
    return clothingDataList.Count;
  }

  public static int GetCraftedCount(FollowerClothingType clothingType)
  {
    if (clothingType == FollowerClothingType.None)
      return int.MaxValue;
    List<Structures_Tailor> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_Tailor>();
    int craftedCount = 0;
    foreach (StructureBrain structureBrain in structuresOfType)
    {
      foreach (StructuresData.ClothingStruct clothingStruct in structureBrain.Data.AllClothing)
      {
        if (clothingStruct.ClothingType == clothingType)
          ++craftedCount;
      }
    }
    return craftedCount;
  }

  public static bool CanAfford(global::ClothingData clothingData)
  {
    List<global::ClothingData.CostItem> costItemList = new List<global::ClothingData.CostItem>((IEnumerable<global::ClothingData.CostItem>) clothingData.Cost);
    for (int index = 0; index < costItemList.Count; ++index)
    {
      if (Inventory.GetItemQuantity(costItemList[index].ItemType) < costItemList[index].Cost)
        return false;
    }
    return true;
  }

  public static List<global::ClothingData> GetAvailableWeatherClothing()
  {
    List<global::ClothingData> availableWeatherClothing = new List<global::ClothingData>((IEnumerable<global::ClothingData>) TailorManager.ClothingData);
    for (int index = availableWeatherClothing.Count - 1; index >= 0; --index)
    {
      if (availableWeatherClothing[index].HideOnTailorMenu || availableWeatherClothing[index].IsDLC || availableWeatherClothing[index].IsMajorDLC || availableWeatherClothing[index].SpecialClothing || !DataManager.Instance.UnlockedClothing.Contains(availableWeatherClothing[index].ClothingType))
        availableWeatherClothing.RemoveAt(index);
    }
    return availableWeatherClothing;
  }

  public static List<global::ClothingData> GetUnavailableWeatherClothing()
  {
    List<global::ClothingData> unavailableWeatherClothing = new List<global::ClothingData>((IEnumerable<global::ClothingData>) TailorManager.ClothingData);
    for (int index = unavailableWeatherClothing.Count - 1; index >= 0; --index)
    {
      if (unavailableWeatherClothing[index].HideOnTailorMenu || unavailableWeatherClothing[index].IsDLC || unavailableWeatherClothing[index].IsMajorDLC || unavailableWeatherClothing[index].SpecialClothing || DataManager.Instance.UnlockedClothing.Contains(unavailableWeatherClothing[index].ClothingType))
        unavailableWeatherClothing.RemoveAt(index);
    }
    return unavailableWeatherClothing;
  }

  public static bool GetClothingAvailable(FollowerClothingType clothing)
  {
    foreach (FollowerClothingType followerClothingType in DataManager.Instance.UnlockedClothing)
    {
      if (followerClothingType == clothing)
        return true;
    }
    return false;
  }

  public static List<global::ClothingData> GetAllWeatherClothing()
  {
    List<global::ClothingData> allWeatherClothing = new List<global::ClothingData>((IEnumerable<global::ClothingData>) TailorManager.ClothingData);
    for (int index = allWeatherClothing.Count - 1; index >= 0; --index)
    {
      if (allWeatherClothing[index].HideOnTailorMenu || allWeatherClothing[index].SpecialClothing)
        allWeatherClothing.RemoveAt(index);
    }
    return allWeatherClothing;
  }

  public static List<global::ClothingData> GetAvailableSpecialClothing()
  {
    List<global::ClothingData> availableSpecialClothing = new List<global::ClothingData>();
    foreach (StructureBrain structureBrain in StructureManager.GetAllStructuresOfType<Structures_Tailor>())
    {
      foreach (StructuresData.ClothingStruct clothingStruct in structureBrain.Data.AllClothing)
      {
        global::ClothingData clothingData = TailorManager.GetClothingData(clothingStruct.ClothingType);
        if (clothingData.SpecialClothing && !availableSpecialClothing.Contains(clothingData))
          availableSpecialClothing.Add(clothingData);
      }
    }
    foreach (global::ClothingData clothingData in TailorManager.GetAllSpecialClothing())
    {
      if (!availableSpecialClothing.Contains(clothingData) && TailorManager.GetFollowerWearingOutfit(clothingData.ClothingType) != null)
        availableSpecialClothing.Add(clothingData);
    }
    return availableSpecialClothing;
  }

  public static List<global::ClothingData> GetAvailableClothing()
  {
    List<global::ClothingData> availableClothing = new List<global::ClothingData>();
    foreach (StructureBrain structureBrain in StructureManager.GetAllStructuresOfType<Structures_Tailor>())
    {
      foreach (StructuresData.ClothingStruct clothingStruct in structureBrain.Data.AllClothing)
      {
        global::ClothingData clothingData = TailorManager.GetClothingData(clothingStruct.ClothingType);
        if (!availableClothing.Contains(clothingData))
          availableClothing.Add(clothingData);
      }
    }
    foreach (global::ClothingData clothingData in TailorManager.ClothingData)
    {
      if (!availableClothing.Contains(clothingData) && TailorManager.GetFollowerWearingOutfit(clothingData.ClothingType) != null)
        availableClothing.Add(clothingData);
    }
    return availableClothing;
  }

  public static List<FollowerClothingType> GetClothingToSell()
  {
    List<FollowerClothingType> followerClothingTypeList = new List<FollowerClothingType>();
    if (PlayerFarming.Location == FollowerLocation.Dungeon1_5 || PlayerFarming.Location == FollowerLocation.Dungeon1_6)
    {
      followerClothingTypeList.Clear();
      foreach (global::ClothingData clothingData in TailorManager.ClothingData)
      {
        if (clothingData.ForSale && clothingData.IsMajorDLC && (!TailorManager.postBerithQuestDLCOutfits.Contains(clothingData.ClothingType) || TailorManager.postBerithQuestDLCOutfits.Contains(clothingData.ClothingType) && DataManager.Instance.BerithTalkedWithBop))
          followerClothingTypeList.Add(clothingData.ClothingType);
      }
    }
    else
    {
      foreach (global::ClothingData clothingData in TailorManager.ClothingData)
      {
        if (clothingData.ForSale && !clothingData.IsMajorDLC)
          followerClothingTypeList.Add(clothingData.ClothingType);
      }
    }
    List<FollowerClothingType> clothingToSell = new List<FollowerClothingType>();
    foreach (FollowerClothingType type in followerClothingTypeList)
    {
      if (!DataManager.Instance.ClothesUnlocked(type))
        clothingToSell.Add(type);
    }
    return clothingToSell;
  }

  public static List<FollowerClothingType> GetClothingFromChest(bool dlcOnly)
  {
    List<FollowerClothingType> followerClothingTypeList = new List<FollowerClothingType>();
    List<FollowerClothingType> clothingFromChest = new List<FollowerClothingType>();
    if (dlcOnly)
    {
      foreach (global::ClothingData clothingData in TailorManager.ClothingData)
      {
        if (clothingData.IsMajorDLC && clothingData.ForSale && !clothingData.IsSecret)
          followerClothingTypeList.Add(clothingData.ClothingType);
      }
    }
    else
    {
      foreach (global::ClothingData clothingData in TailorManager.ClothingData)
      {
        if (clothingData.ForSale && !clothingData.IsMajorDLC)
          followerClothingTypeList.Add(clothingData.ClothingType);
      }
    }
    foreach (FollowerClothingType type in followerClothingTypeList)
    {
      if (!DataManager.Instance.ClothesUnlocked(type))
        clothingFromChest.Add(type);
    }
    return clothingFromChest;
  }

  public static bool IsClothingAvailable(FollowerClothingType clothingType)
  {
    foreach (Structures_Tailor structuresTailor in StructureManager.GetAllStructuresOfType<Structures_Tailor>())
    {
      foreach (StructuresData.ClothingStruct queuedClothing in structuresTailor.Data.QueuedClothings)
      {
        if (queuedClothing.ClothingType == clothingType)
          return false;
      }
      foreach (StructuresData.ClothingStruct clothingStruct in structuresTailor.Data.AllClothing)
      {
        if (clothingStruct.ClothingType == clothingType)
          return false;
      }
      foreach (StructuresData.ClothingStruct clothingStruct in structuresTailor.Data.AllClothing)
      {
        if (clothingStruct.ClothingType == clothingType)
          return false;
      }
    }
    return TailorManager.GetFollowerWearingOutfit(clothingType) == null;
  }

  public static FollowerBrain GetFollowerWearingOutfit(FollowerClothingType type)
  {
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.Info.Clothing == type)
        return allBrain;
    }
    return (FollowerBrain) null;
  }

  public static void RemoveClothingFromDeadFollower(FollowerClothingType type)
  {
    foreach (FollowerInfo followerInfo in DataManager.Instance.Followers_Dead)
    {
      if (followerInfo.Clothing == type)
      {
        followerInfo.Clothing = FollowerClothingType.None;
        followerInfo.ClothingVariant = followerInfo.ClothingPreviousVariant;
      }
    }
  }

  public static void RemoveClothingFromFollower(
    FollowerClothingType type,
    FollowerBrain excludingFollower = null)
  {
    if (type == FollowerClothingType.None)
      return;
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.Info.Clothing == type && allBrain != excludingFollower)
      {
        allBrain.AssignClothing(FollowerClothingType.None);
        Follower followerById = FollowerManager.FindFollowerByID(allBrain.Info.ID);
        if ((Object) followerById != (Object) null)
          FollowerBrain.SetFollowerCostume(followerById.Spine.Skeleton, allBrain._directInfoAccess, forceUpdate: true);
      }
    }
  }

  public static void RemoveClothingFromTailor(FollowerClothingType ct)
  {
    if (ct == FollowerClothingType.None)
      return;
    foreach (Structures_Tailor structuresTailor in StructureManager.GetAllStructuresOfType<Structures_Tailor>())
    {
      foreach (StructuresData.ClothingStruct clothingStruct in structuresTailor.Data.AllClothing)
      {
        if (clothingStruct.ClothingType == ct)
        {
          structuresTailor.Data.AllClothing.Remove(clothingStruct);
          return;
        }
      }
    }
  }

  public static void AddClothingToTailor(FollowerClothingType ct, string variant)
  {
    if (ct == FollowerClothingType.None || !DataManager.Instance.UnlockedClothing.Contains(ct))
      return;
    foreach (StructureBrain structureBrain in StructureManager.GetAllStructuresOfType<Structures_Tailor>())
      structureBrain.Data.AllClothing.Add(new StructuresData.ClothingStruct(ct, variant));
  }

  public static StructuresData.ClothingStruct GetClothingFromTailor(FollowerClothingType ct)
  {
    foreach (StructureBrain structureBrain in StructureManager.GetAllStructuresOfType<Structures_Tailor>())
    {
      foreach (StructuresData.ClothingStruct clothingFromTailor in structureBrain.Data.AllClothing)
      {
        if (clothingFromTailor.ClothingType == ct)
          return clothingFromTailor;
      }
    }
    return new StructuresData.ClothingStruct(ct, "");
  }

  public static int GetClothingQuantity(FollowerClothingType ct)
  {
    int clothingQuantity = 0;
    foreach (StructureBrain structureBrain in StructureManager.GetAllStructuresOfType<Structures_Tailor>())
    {
      foreach (StructuresData.ClothingStruct clothingStruct in structureBrain.Data.AllClothing)
      {
        if (clothingStruct.ClothingType == ct)
          ++clothingQuantity;
      }
    }
    return clothingQuantity;
  }

  public static bool IsSpecialClothing(FollowerClothingType type)
  {
    foreach (global::ClothingData clothingData in TailorManager.ClothingData)
    {
      if (clothingData.ClothingType == type)
        return clothingData.SpecialClothing;
    }
    return false;
  }

  public static List<global::ClothingData> GetAllSpecialClothing()
  {
    List<global::ClothingData> allSpecialClothing = new List<global::ClothingData>();
    foreach (global::ClothingData clothingData in TailorManager.ClothingData)
    {
      if (clothingData.SpecialClothing)
        allSpecialClothing.Add(clothingData);
    }
    return allSpecialClothing;
  }

  public static List<global::ClothingData> GetDLCClothing()
  {
    List<global::ClothingData> dlcClothing = new List<global::ClothingData>();
    foreach (global::ClothingData clothingData in TailorManager.ClothingData)
    {
      if (clothingData.IsDLC && !clothingData.HideOnTailorMenu)
        dlcClothing.Add(clothingData);
    }
    return dlcClothing;
  }

  public static List<global::ClothingData> GetMajorDLCClothing()
  {
    List<global::ClothingData> majorDlcClothing = new List<global::ClothingData>();
    foreach (global::ClothingData clothingData in TailorManager.ClothingData)
    {
      if (clothingData.IsMajorDLC && !clothingData.HideOnTailorMenu)
        majorDlcClothing.Add(clothingData);
    }
    return majorDlcClothing;
  }

  public static List<global::ClothingData> GetUnlockedSpecialClothing()
  {
    List<global::ClothingData> unlockedSpecialClothing = new List<global::ClothingData>();
    foreach (global::ClothingData clothingData in TailorManager.ClothingData)
    {
      if (clothingData.SpecialClothing && DataManager.Instance.UnlockedClothing.Contains(clothingData.ClothingType) && !clothingData.HideOnTailorMenu && !clothingData.IsDLC && !clothingData.IsMajorDLC)
        unlockedSpecialClothing.Add(clothingData);
    }
    return unlockedSpecialClothing;
  }

  public static List<global::ClothingData> GetUnlockedDLCClothing()
  {
    List<global::ClothingData> unlockedDlcClothing = new List<global::ClothingData>();
    foreach (global::ClothingData clothingData in TailorManager.ClothingData)
    {
      if (clothingData.IsDLC && DataManager.Instance.UnlockedClothing.Contains(clothingData.ClothingType) && !clothingData.HideOnTailorMenu)
        unlockedDlcClothing.Add(clothingData);
    }
    return unlockedDlcClothing;
  }

  public static List<global::ClothingData> GetLockedSpecialClothing()
  {
    List<global::ClothingData> lockedSpecialClothing = new List<global::ClothingData>();
    foreach (global::ClothingData clothingData in TailorManager.ClothingData)
    {
      if (clothingData.SpecialClothing && !DataManager.Instance.UnlockedClothing.Contains(clothingData.ClothingType) && !clothingData.HideOnTailorMenu && !clothingData.IsMajorDLC && !clothingData.IsDLC)
        lockedSpecialClothing.Add(clothingData);
    }
    return lockedSpecialClothing;
  }

  public static CookingData.MealEffect[] GetEffectTypes(FollowerClothingType clothingType)
  {
    global::ClothingData clothingData = TailorManager.GetClothingData(clothingType);
    return !((Object) clothingData != (Object) null) || clothingData.Effects == null ? new CookingData.MealEffect[0] : clothingData.Effects;
  }

  public static string LocalizedName(FollowerClothingType clothingType)
  {
    return LocalizationManager.GetTranslation($"Clothing/{clothingType}/Name");
  }

  public static string LocalizedDescription(FollowerClothingType clothingType)
  {
    return LocalizationManager.GetTranslation($"Clothing/{clothingType}/Description");
  }

  public static string LocalizedRequirements(FollowerClothingType clothingType)
  {
    return LocalizationManager.GetTranslation($"Clothing/{clothingType}/Requirements");
  }

  public static string GetSkin(FollowerClothingType clothingType)
  {
    return FollowerBrain.GetClothingName(clothingType);
  }

  public static int GetCraftableAmount(global::ClothingData clothingData)
  {
    int craftableAmount = 0;
    List<InventoryItem> inventoryItemList = new List<InventoryItem>();
    foreach (global::ClothingData.CostItem costItem in clothingData.Cost)
      inventoryItemList.Add(new InventoryItem()
      {
        type = (int) costItem.ItemType,
        quantity = Inventory.GetItemQuantity(costItem.ItemType)
      });
    while (true)
    {
      foreach (global::ClothingData.CostItem costItem in clothingData.Cost)
      {
        foreach (InventoryItem inventoryItem in inventoryItemList)
        {
          if (inventoryItem.quantity < costItem.Cost)
            return craftableAmount;
          inventoryItem.quantity -= costItem.Cost;
        }
      }
      ++craftableAmount;
    }
  }

  public static bool HasUnlockedAllClothing()
  {
    int num1 = 0;
    int num2 = 0;
    foreach (global::ClothingData clothingData in TailorManager.ClothingData)
    {
      if (!DataManager.Instance.Cultist_DLC_Clothing.Contains<FollowerClothingType>(clothingData.ClothingType) && !DataManager.Instance.Heretic_DLC_Clothing.Contains<FollowerClothingType>(clothingData.ClothingType) && !DataManager.Instance.Pilgrim_DLC_Clothing.Contains<FollowerClothingType>(clothingData.ClothingType) && !DataManager.Instance.Sinful_DLC_Clothing.Contains<FollowerClothingType>(clothingData.ClothingType) && !DataManager.Instance.Major_DLC_Clothing.Contains<FollowerClothingType>(clothingData.ClothingType))
      {
        if (DataManager.Instance.UnlockedClothing.Contains(clothingData.ClothingType))
          ++num2;
        ++num1;
      }
    }
    return num2 >= num1;
  }
}
