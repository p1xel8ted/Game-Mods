// Decompiled with JetBrains decompiler
// Type: WeaponUpgradeSystem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System;
using System.Collections.Generic;

#nullable disable
public class WeaponUpgradeSystem
{
  public static List<string> UnlockedUpgrades
  {
    get => DataManager.Instance.WeaponUnlockedUpgrades;
    set => DataManager.Instance.WeaponUnlockedUpgrades = value;
  }

  public static string GetLocalizedName(WeaponUpgradeSystem.WeaponUpgradeType Type)
  {
    return LocalizationManager.GetTranslation($"WeaponUpgradeSystem/{Type}/Name");
  }

  public static string GetLocalizedDescription(WeaponUpgradeSystem.WeaponUpgradeType Type)
  {
    return LocalizationManager.GetTranslation($"WeaponUpgradeSystem/{Type}/Description");
  }

  public static string GetLocalizedName(WeaponUpgradeSystem.WeaponType Type)
  {
    return LocalizationManager.GetTranslation($"WeaponUpgradeSystem/{Type}/Name");
  }

  public static string GetLocalizedDescription(WeaponUpgradeSystem.WeaponType Type)
  {
    return LocalizationManager.GetTranslation($"WeaponUpgradeSystem/{Type}/Description");
  }

  public static bool GetUnlocked(
    WeaponUpgradeSystem.WeaponType weapon,
    WeaponUpgradeSystem.WeaponUpgradeType upgradeType)
  {
    return WeaponUpgradeSystem.UnlockedUpgrades.Contains($"{weapon}_{upgradeType}");
  }

  public enum WeaponType
  {
    Sword,
    Dagger,
    Axe,
    Blunderbuss,
    Hammer,
  }

  public enum WeaponUpgradeType
  {
    Level_1_A,
    Level_1_B,
    Level_2_A,
    Level_2_B,
    Level_2_C,
    Level_3,
    Level_4_A,
    Level_4_B,
    Level_5_A,
    Level_5_B,
    Level_6,
  }

  [Serializable]
  public struct RequiredResources
  {
    public InventoryItem.ITEM_TYPE ItemType;
    public int Quantity;
  }
}
