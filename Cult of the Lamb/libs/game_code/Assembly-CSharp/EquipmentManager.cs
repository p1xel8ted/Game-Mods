// Decompiled with JetBrains decompiler
// Type: EquipmentManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class EquipmentManager
{
  public static Action<RelicType> OnRelicUnlocked;
  public static WeaponData[] WeaponsData;
  public static CurseData[] CursesData;
  public static global::RelicData[] relicData;
  public const string WeaponsPath = "Data/Equipment Data/Weapons";
  public const string CursesPath = "Data/Equipment Data/Curses";
  public const string RelicsPath = "Data/Equipment Data/Relics";
  public static List<EquipmentType> LegendaryWeapons = new List<EquipmentType>()
  {
    EquipmentType.Axe_Legendary,
    EquipmentType.Sword_Legendary,
    EquipmentType.Dagger_Legendary,
    EquipmentType.Gauntlet_Legendary,
    EquipmentType.Hammer_Legendary,
    EquipmentType.Blunderbuss_Legendary,
    EquipmentType.Chain_Legendary
  };
  public static RelicType PrevRandomRelic = RelicType.None;
  public static RelicType NextRandomRelic = RelicType.None;
  public static List<RelicType> RandomRelics = new List<RelicType>()
  {
    RelicType.SpawnTentacle,
    RelicType.GungeonBlank,
    RelicType.SpawnBombs,
    RelicType.LightningStrike,
    RelicType.FiftyFiftyGamble,
    RelicType.FillUpFervour,
    RelicType.FreezeTime,
    RelicType.ProjectileRing,
    RelicType.PoisonAll,
    RelicType.RandomEnemyIntoCritter,
    RelicType.SpawnDemon,
    RelicType.SpawnCombatFollower,
    RelicType.RerollCurse,
    RelicType.RerollWeapon
  };
  public static List<RelicType> RandomBlessedRelics = new List<RelicType>()
  {
    RelicType.GungeonBlank,
    RelicType.FreezeTime,
    RelicType.LightningStrike_Blessed,
    RelicType.FiftyFiftyGamble_Blessed,
    RelicType.HeartConversion_Blessed,
    RelicType.FillUpFervour,
    RelicType.FreezeAll
  };
  public static List<RelicType> RandomDamnedRelics = new List<RelicType>()
  {
    RelicType.SpawnTentacle,
    RelicType.SpawnBombs,
    RelicType.LightningStrike_Dammed,
    RelicType.FiftyFiftyGamble_Dammed,
    RelicType.HeartConversion_Dammed,
    RelicType.ProjectileRing,
    RelicType.PoisonAll
  };
  public static List<RelicType> CoopRelics = new List<RelicType>()
  {
    RelicType.DamageEye_Coop,
    RelicType.Explosive_Coop,
    RelicType.FriendlyHealing_Coop,
    RelicType.LoadoutSwap_Coop,
    RelicType.Sacrifice_Coop
  };
  public static List<RelicType> MajorDLCRelics = new List<RelicType>()
  {
    RelicType.IgniteOnTouch_Familiar,
    RelicType.IgniteAll,
    RelicType.FireballsRain,
    RelicType.FieryBlood,
    RelicType.FieryBurrow,
    RelicType.FrozenGhosts,
    RelicType.IceyBlood,
    RelicType.IceyBurrow,
    RelicType.IceyCoat,
    RelicType.IceSpikes
  };

  public static global::RelicData[] RelicData
  {
    get
    {
      if (EquipmentManager.relicData == null)
        EquipmentManager.relicData = Resources.LoadAll<global::RelicData>("Data/Equipment Data/Relics");
      return EquipmentManager.relicData;
    }
  }

  public static EquipmentData GetEquipmentData(EquipmentType equipmentType)
  {
    EquipmentData weaponData = (EquipmentData) EquipmentManager.GetWeaponData(equipmentType);
    return (bool) (UnityEngine.Object) weaponData ? weaponData : (EquipmentData) EquipmentManager.GetCurseData(equipmentType);
  }

  public static WeaponData GetWeaponData(EquipmentType weaponType)
  {
    if (EquipmentManager.WeaponsData == null)
      EquipmentManager.WeaponsData = Resources.LoadAll<WeaponData>("Data/Equipment Data/Weapons");
    foreach (WeaponData weaponData in EquipmentManager.WeaponsData)
    {
      if (weaponData.EquipmentType == weaponType)
        return weaponData;
    }
    return (WeaponData) null;
  }

  public static bool IsPoisonWeapon(EquipmentType weaponType)
  {
    return weaponType == EquipmentType.Axe_Poison || weaponType == EquipmentType.Dagger_Poison || weaponType == EquipmentType.Gauntlet_Poison || weaponType == EquipmentType.Hammer_Poison || weaponType == EquipmentType.Sword_Poison || weaponType == EquipmentType.Blunderbuss_Poison || weaponType == EquipmentType.Chain_Poison;
  }

  public static bool IsLegendaryWeapon(EquipmentType weaponType)
  {
    WeaponData weaponData = EquipmentManager.GetWeaponData(weaponType);
    return (UnityEngine.Object) weaponData != (UnityEngine.Object) null && EquipmentManager.LegendaryWeapons.Contains(weaponData.EquipmentType);
  }

  public static CurseData GetCurseData(EquipmentType curseType)
  {
    if (EquipmentManager.CursesData == null)
      EquipmentManager.CursesData = Resources.LoadAll<CurseData>("Data/Equipment Data/Curses");
    foreach (CurseData curseData in EquipmentManager.CursesData)
    {
      if (curseData.EquipmentType == curseType)
        return curseData;
    }
    return (CurseData) null;
  }

  public static bool IsIceCurse(EquipmentType curseType)
  {
    return curseType == EquipmentType.MegaSlash_Ice || curseType == EquipmentType.EnemyBlast_Ice || curseType == EquipmentType.Tentacles_Ice;
  }

  public static global::RelicData GetRelicData(RelicType relicType)
  {
    foreach (global::RelicData relicData in EquipmentManager.RelicData)
    {
      if (relicData.RelicType == relicType)
        return relicData;
    }
    Debug.Log((object) ("1 No relic found for " + relicType.ToString()));
    return (global::RelicData) null;
  }

  public static List<global::RelicData> GetRelicData(List<RelicType> relicTypes)
  {
    List<global::RelicData> relicData = new List<global::RelicData>();
    foreach (RelicType relicType in relicTypes)
      relicData.Add(EquipmentManager.GetRelicData(relicType));
    return relicData;
  }

  public static bool GetRelicSingleUse(RelicType relicType, PlayerFarming playerFarming)
  {
    foreach (global::RelicData relicData in EquipmentManager.RelicData)
    {
      if (relicData.RelicType == relicType && (relicData.InteractionType == RelicInteractionType.Fragile || TrinketManager.AreRelicsFragile(playerFarming)))
        return true;
    }
    return false;
  }

  public static Sprite GetRelicIcon(RelicType relicType)
  {
    foreach (global::RelicData relicData in EquipmentManager.RelicData)
    {
      if (relicData.RelicType == relicType)
        return relicData.Sprite;
    }
    return (Sprite) null;
  }

  public static Sprite GetRelicIconOutline(RelicType relicType)
  {
    foreach (global::RelicData relicData in EquipmentManager.RelicData)
    {
      if (relicData.RelicType == relicType)
        return relicData.SpriteOutline;
    }
    return (Sprite) null;
  }

  public static void PickRandomRelicData(bool includeSpawnedRelicsThisRun, RelicSubType subType = RelicSubType.Any)
  {
    int num = 100;
    List<RelicType> relicTypeList = new List<RelicType>((IEnumerable<RelicType>) EquipmentManager.RandomRelics);
    if (PlayerFleeceManager.FleeceSwapsWeaponForCurse())
      relicTypeList.Remove(RelicType.RerollWeapon);
    EquipmentManager.PrevRandomRelic = EquipmentManager.NextRandomRelic;
    while (EquipmentManager.PrevRandomRelic == EquipmentManager.NextRandomRelic && --num > 0)
    {
      switch (subType)
      {
        case RelicSubType.Any:
          EquipmentManager.NextRandomRelic = relicTypeList[UnityEngine.Random.Range(0, relicTypeList.Count)];
          break;
        case RelicSubType.Dammed:
          EquipmentManager.NextRandomRelic = EquipmentManager.RandomDamnedRelics[UnityEngine.Random.Range(0, EquipmentManager.RandomDamnedRelics.Count)];
          break;
        case RelicSubType.Blessed:
          EquipmentManager.NextRandomRelic = EquipmentManager.RandomBlessedRelics[UnityEngine.Random.Range(0, EquipmentManager.RandomBlessedRelics.Count)];
          break;
      }
      if (DataManager.Instance.PlayerFleece == 6 && EquipmentManager.NextRandomRelic == RelicType.RerollWeapon)
        EquipmentManager.NextRandomRelic = EquipmentManager.PrevRandomRelic;
    }
  }

  public static global::RelicData GetRandomRelicData(
    bool includeSpawnedRelicsThisRun,
    PlayerFarming playerFarming,
    RelicSubType subType = RelicSubType.Any)
  {
    List<global::RelicData> relicDataList = new List<global::RelicData>((IEnumerable<global::RelicData>) EquipmentManager.RelicData);
    if (relicDataList.Count <= 1)
    {
      Debug.Log((object) "RELIC FALLBACK HIT");
      return EquipmentManager.GetRelicData(RelicType.LightningStrike);
    }
    if ((double) UnityEngine.Random.value < 0.20000000298023224)
    {
      if ((double) UnityEngine.Random.value > 0.5 && UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Relics_Blessed_1))
        subType = RelicSubType.Blessed;
      else if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Relics_Dammed_1))
        subType = RelicSubType.Dammed;
    }
    int index1;
    for (index1 = relicDataList.Count - 1; index1 >= 0; --index1)
    {
      if (DungeonSandboxManager.Active && relicDataList[index1].RequiresCult)
        relicDataList.RemoveAt(index1);
      else if (DataManager.Instance.PreviousRelic == relicDataList[index1].RelicType)
        relicDataList.RemoveAt(index1);
      else if (subType == RelicSubType.Dammed && !relicDataList[index1].RelicType.ToString().Contains("Dammed"))
        relicDataList.RemoveAt(index1);
      else if (subType == RelicSubType.Blessed && !relicDataList[index1].RelicType.ToString().Contains("Blessed"))
        relicDataList.RemoveAt(index1);
      else if (!includeSpawnedRelicsThisRun && DataManager.Instance.SpawnedRelicsThisRun.Contains(relicDataList[index1].RelicType))
        relicDataList.RemoveAt(index1);
      else if (!GameManager.DungeonUseAllLayers && (relicDataList[index1].RelicType == RelicType.TeleportToBoss || relicDataList[index1].RelicType == RelicType.RandomTeleport))
        relicDataList.RemoveAt(index1);
      else if (relicDataList[index1].RelicType.ToString().Contains("Dammed") && !UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Relics_Dammed_1) || relicDataList[index1].RelicType.ToString().Contains("Blessed") && !UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Relics_Blessed_1))
        relicDataList.RemoveAt(index1);
      else if (DataManager.Instance.ForceBlessedRelic && !relicDataList[index1].RelicType.ToString().Contains("Blessed") || DataManager.Instance.ForceDammedRelic && !relicDataList[index1].RelicType.ToString().Contains("Dammed"))
        relicDataList.RemoveAt(index1);
      else if (!DataManager.Instance.PlayerFoundRelics.Contains(relicDataList[index1].RelicType))
        relicDataList.RemoveAt(index1);
      else if ((relicDataList[index1].RelicType == RelicType.DestroyTarotDealDamge || relicDataList[index1].RelicType == RelicType.DestroyTarotDealDamge_Blessed || relicDataList[index1].RelicType == RelicType.DestroyTarotDealDamge_Dammed || relicDataList[index1].RelicType == RelicType.DestroyTarotGainBuff || relicDataList[index1].RelicType == RelicType.DestroyTarotGainBuff_Blessed || relicDataList[index1].RelicType == RelicType.DestroyTarotGainBuff_Dammed) && (UnityEngine.Object) playerFarming != (UnityEngine.Object) null && playerFarming.RunTrinkets.Count <= 0)
        relicDataList.RemoveAt(index1);
      else if (relicDataList[index1].RelicType == RelicType.ProjectileRing && PlayerFarming.Instance.RunTrinkets.Count <= 1)
        relicDataList.RemoveAt(index1);
      else if (relicDataList[index1].RelicType == RelicType.Enlarge && TrinketManager.HasTrinket(TarotCards.Card.WalkThroughBlocks, PlayerFarming.Instance))
        relicDataList.RemoveAt(index1);
      else if (relicDataList[index1].RelicType == RelicType.RerollWeapon && DataManager.Instance.PlayerFleece == 6)
        relicDataList.RemoveAt(index1);
      else if (relicDataList[index1].RelicSubType == RelicSubType.Coop && PlayerFarming.playersCount <= 1)
        relicDataList.RemoveAt(index1);
      else if (DataManager.Instance.PlayerFleece == 6 && (relicDataList[index1].RelicType == RelicType.DestroyTarotDealDamge || relicDataList[index1].RelicType == RelicType.DestroyTarotDealDamge_Blessed || relicDataList[index1].RelicType == RelicType.DestroyTarotDealDamge_Dammed))
        relicDataList.RemoveAt(index1);
      else if (relicDataList[index1].RelicType == RelicType.DeathForLife_Corrupted)
      {
        bool flag = false;
        for (int index2 = 0; index2 < DataManager.Instance.Followers.Count; ++index2)
        {
          if (!FollowerManager.FollowerLocked(in DataManager.Instance.Followers[index2].ID))
          {
            flag = true;
            break;
          }
        }
        if (!flag || (double) PlayerFarming.Instance.health.HP >= (double) PlayerFarming.Instance.health.totalHP)
          relicDataList.RemoveAt(index1);
      }
    }
    Debug.Log((object) $"{index1.ToString()} relicPool count: {relicDataList.Count.ToString()}");
    if (relicDataList.Count <= 0)
    {
      Debug.Log((object) "RELIC FALLBACK HIT");
      return EquipmentManager.GetRelicData(RelicType.LightningStrike);
    }
    float num1 = 0.0f;
    for (int index3 = 0; index3 < relicDataList.Count; ++index3)
    {
      float weight = relicDataList[index3].Weight;
      if (float.IsPositiveInfinity(weight))
        return relicDataList[index3];
      if ((double) weight >= 0.0 && !float.IsNaN(weight))
        num1 += weight;
    }
    float num2 = UnityEngine.Random.value;
    float num3 = 0.0f;
    for (int index4 = 0; index4 < relicDataList.Count; ++index4)
    {
      float weight = relicDataList[index4].Weight;
      if (!float.IsNaN(weight) && (double) weight > 0.0)
      {
        num3 += weight / (float) relicDataList.Count;
        if ((double) num3 >= (double) num2)
        {
          Debug.Log((object) ("Chosen: " + ((object) relicDataList[index4])?.ToString()));
          return relicDataList[index4];
        }
      }
    }
    return EquipmentManager.GetRandomRelicData(includeSpawnedRelicsThisRun, playerFarming, subType);
  }

  public static void UnlockRelics(UpgradeSystem.Type upgrade)
  {
    foreach (global::RelicData relicData in EquipmentManager.RelicData)
    {
      if (relicData.UpgradeType == upgrade && !DataManager.Instance.PlayerFoundRelics.Contains(relicData.RelicType))
      {
        DataManager.UnlockRelic(relicData.RelicType);
        Action<RelicType> onRelicUnlocked = EquipmentManager.OnRelicUnlocked;
        if (onRelicUnlocked != null)
          onRelicUnlocked(relicData.RelicType);
      }
    }
  }

  public static List<global::RelicData> GetRelicsForUpgradeType(UpgradeSystem.Type upgrade)
  {
    List<global::RelicData> relicsForUpgradeType = new List<global::RelicData>();
    foreach (global::RelicData relicData in EquipmentManager.RelicData)
    {
      if (upgrade == relicData.UpgradeType)
        relicsForUpgradeType.Add(relicData);
    }
    return relicsForUpgradeType;
  }
}
