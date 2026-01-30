// Decompiled with JetBrains decompiler
// Type: CultUpgradeData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System.Collections.Generic;

#nullable disable
public class CultUpgradeData
{
  public static string GetLockedText()
  {
    return LocalizationManager.GetTranslation("UI/CultUpgrade/BorderLocked");
  }

  public static string GetLocalizedName(CultUpgradeData.TYPE Type)
  {
    return LocalizationManager.GetTranslation($"UI/CultUpgrade/{Type}/Name");
  }

  public static string GetLocalizedDescription(CultUpgradeData.TYPE Type)
  {
    return LocalizationManager.GetTranslation($"UI/CultUpgrade/{Type}/Description");
  }

  public static List<StructuresData.ItemCost> GetCost(CultUpgradeData.TYPE Type)
  {
    switch (Type)
    {
      case CultUpgradeData.TYPE.Cult1:
      case CultUpgradeData.TYPE.Cult2:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.PLEASURE_POINT, 1),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 3)
        };
      case CultUpgradeData.TYPE.Cult3:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.PLEASURE_POINT, 2),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 5)
        };
      case CultUpgradeData.TYPE.Cult4:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.PLEASURE_POINT, 3),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 5)
        };
      case CultUpgradeData.TYPE.Cult5:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.PLEASURE_POINT, 4),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 10)
        };
      case CultUpgradeData.TYPE.Cult6:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.PLEASURE_POINT, 6),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 10)
        };
      case CultUpgradeData.TYPE.Cult7:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.PLEASURE_POINT, 8),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 15)
        };
      case CultUpgradeData.TYPE.Cult8:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.PLEASURE_POINT, 10),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 20)
        };
      case CultUpgradeData.TYPE.Cult9:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.PLEASURE_POINT, 12),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 25)
        };
      case CultUpgradeData.TYPE.Cult10:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.PLEASURE_POINT, 15),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 50)
        };
      case CultUpgradeData.TYPE.Border1:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.PLEASURE_POINT, 1)
        };
      case CultUpgradeData.TYPE.Border2:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.PLEASURE_POINT, 3)
        };
      case CultUpgradeData.TYPE.Border3:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.PLEASURE_POINT, 6)
        };
      case CultUpgradeData.TYPE.Border4:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.PLEASURE_POINT, 12)
        };
      case CultUpgradeData.TYPE.Border5:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.PLEASURE_POINT, 18)
        };
      case CultUpgradeData.TYPE.Border6:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.PLEASURE_POINT, 24)
        };
      default:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.PLEASURE_POINT, 1),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, 1)
        };
    }
  }

  public static bool IsUpgradeMaxed() => DataManager.Instance.TempleLevel >= 10;

  public static bool UserCanAffordUpgrade(CultUpgradeData.TYPE type)
  {
    if (CheatConsole.BuildingsFree)
      return true;
    List<StructuresData.ItemCost> cost = CultUpgradeData.GetCost(type);
    for (int index = 0; index < cost.Count; ++index)
    {
      if (Inventory.GetItemQuantity((int) cost[index].CostItem) < cost[index].CostValue)
        return false;
    }
    return true;
  }

  public static bool IsUnlocked(CultUpgradeData.TYPE type)
  {
    switch (type)
    {
      case CultUpgradeData.TYPE.Border1:
        return DataManager.Instance.TempleLevel >= 1;
      case CultUpgradeData.TYPE.Border2:
        return DataManager.Instance.TempleLevel >= 3;
      case CultUpgradeData.TYPE.Border3:
        return DataManager.Instance.TempleLevel >= 6;
      case CultUpgradeData.TYPE.Border4:
        return DataManager.Instance.TempleLevel >= 9;
      case CultUpgradeData.TYPE.Border5:
        return DataManager.Instance.TempleUnlockedBorder5;
      case CultUpgradeData.TYPE.Border6:
        return DataManager.Instance.TempleUnlockedBorder6;
      default:
        return true;
    }
  }

  public static bool IsBorder(CultUpgradeData.TYPE type)
  {
    return type >= CultUpgradeData.TYPE.Border1 && type <= CultUpgradeData.TYPE.Border6;
  }

  public static float GetCoolDownNormalised(CultUpgradeData.TYPE type) => 0.0f;

  public enum TYPE
  {
    None = 0,
    Cult1 = 1,
    Cult2 = 2,
    Cult3 = 3,
    Cult4 = 4,
    Cult5 = 5,
    Cult6 = 6,
    Cult7 = 7,
    Cult8 = 8,
    Cult9 = 9,
    Cult10 = 10, // 0x0000000A
    Border1 = 100, // 0x00000064
    Border2 = 101, // 0x00000065
    Border3 = 102, // 0x00000066
    Border4 = 103, // 0x00000067
    Border5 = 104, // 0x00000068
    Border6 = 105, // 0x00000069
    Count = 106, // 0x0000006A
  }
}
