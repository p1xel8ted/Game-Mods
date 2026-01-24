// Decompiled with JetBrains decompiler
// Type: ClothingData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#nullable disable
[CreateAssetMenu(menuName = "Massive Monster/Clothing Data")]
public class ClothingData : ScriptableObject
{
  public SkeletonAnimation SkeletonData;
  public FollowerClothingType ClothingType;
  public FollowerProtectionType ProtectionType;
  public bool ForSale;
  public bool SpecialClothing;
  public bool IsSecret;
  public bool IsDLC;
  public bool IsMajorDLC;
  public bool IsHeretic;
  public bool IsCultist;
  public bool IsSinful;
  public bool IsPilgrim;
  public bool HideOnTailorMenu;
  public bool CanBeCrafted = true;
  public CookingData.MealEffect[] Effects;
  [SpineSkin("", "SkeletonData", true, false, false)]
  public List<string> Variants = new List<string>();
  public List<WorshipperData.SlotsAndColours> SlotAndColours = new List<WorshipperData.SlotsAndColours>();

  public ClothingData.CostItem[] Cost
  {
    get
    {
      switch (this.ClothingType)
      {
        case FollowerClothingType.None:
          return new ClothingData.CostItem[0];
        case FollowerClothingType.Jumper:
          return new ClothingData.CostItem[1]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.COTTON, 4)
          };
        case FollowerClothingType.Singlet:
          return new ClothingData.CostItem[1]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.COTTON, 4)
          };
        case FollowerClothingType.Robe:
          return new ClothingData.CostItem[1]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.COTTON, 4)
          };
        case FollowerClothingType.Robes_Baal:
          return new ClothingData.CostItem[2]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.COTTON, 7),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.PLEASURE_POINT, 1)
          };
        case FollowerClothingType.Robes_Aym:
          return new ClothingData.CostItem[2]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.COTTON, 7),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.PLEASURE_POINT, 1)
          };
        case FollowerClothingType.Robes_Fancy:
          return new ClothingData.CostItem[3]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.SILK_THREAD, 4),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.COTTON, 9),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.PLEASURE_POINT, 1)
          };
        case FollowerClothingType.Warrior:
          return new ClothingData.CostItem[3]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.SILK_THREAD, 6),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.LOG_REFINED, 2),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.PLEASURE_POINT, 1)
          };
        case FollowerClothingType.Raincoat:
          return new ClothingData.CostItem[1]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.COTTON, 4)
          };
        case FollowerClothingType.Suit_Fancy:
          return new ClothingData.CostItem[3]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.SILK_THREAD, 4),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.FLOWER_RED, 1),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.PLEASURE_POINT, 1)
          };
        case FollowerClothingType.Cultist_DLC:
          return new ClothingData.CostItem[2]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.GRASS, 20),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.FLOWER_RED, 4)
          };
        case FollowerClothingType.Cultist_DLC2:
          return new ClothingData.CostItem[2]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.GRASS, 30),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.FLOWER_RED, 4)
          };
        case FollowerClothingType.Heretic_DLC:
          return new ClothingData.CostItem[2]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.COTTON, 9),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.BONE, 50)
          };
        case FollowerClothingType.Heretic_DLC2:
          return new ClothingData.CostItem[2]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.COTTON, 9),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.BONE, 75)
          };
        case FollowerClothingType.DLC_1:
          return new ClothingData.CostItem[2]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.SILK_THREAD, 1),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.COTTON, 4)
          };
        case FollowerClothingType.DLC_2:
          return new ClothingData.CostItem[2]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.COTTON, 4),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.LOG, 7)
          };
        case FollowerClothingType.DLC_3:
          return new ClothingData.CostItem[2]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.COTTON, 9),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.FOLLOWER_MEAT, 3)
          };
        case FollowerClothingType.DLC_4:
          return new ClothingData.CostItem[2]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.COTTON, 9),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.CRYSTAL, 10)
          };
        case FollowerClothingType.DLC_5:
          return new ClothingData.CostItem[2]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.COTTON, 4),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.GOLD_REFINED, 2)
          };
        case FollowerClothingType.Special_1:
          return new ClothingData.CostItem[3]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.SILK_THREAD, 4),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.YOLK, 1),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.PLEASURE_POINT, 1)
          };
        case FollowerClothingType.Special_2:
          return new ClothingData.CostItem[3]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.SILK_THREAD, 6),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.GOLD_REFINED, 2),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.PLEASURE_POINT, 1)
          };
        case FollowerClothingType.Special_3:
          return new ClothingData.CostItem[3]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.SILK_THREAD, 4),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.CRYSTAL, 10),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.PLEASURE_POINT, 1)
          };
        case FollowerClothingType.Special_4:
          return new ClothingData.CostItem[3]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.SILK_THREAD, 6),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.COTTON, 6),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.PLEASURE_POINT, 1)
          };
        case FollowerClothingType.Special_5:
          return new ClothingData.CostItem[3]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.SILK_THREAD, 4),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.BONE, 75),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.PLEASURE_POINT, 1)
          };
        case FollowerClothingType.Special_6:
          return new ClothingData.CostItem[3]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.SILK_THREAD, 4),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.COTTON, 9),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.PLEASURE_POINT, 1)
          };
        case FollowerClothingType.Special_7:
          return new ClothingData.CostItem[3]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.SILK_THREAD, 8),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.COTTON, 4),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.PLEASURE_POINT, 1)
          };
        case FollowerClothingType.Normal_1:
          return new ClothingData.CostItem[2]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.COTTON, 4),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.GOLD_NUGGET, 12)
          };
        case FollowerClothingType.Normal_2:
          return new ClothingData.CostItem[2]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.COTTON, 4),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.SPIDER_WEB, 6)
          };
        case FollowerClothingType.Normal_3:
          return new ClothingData.CostItem[2]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.COTTON, 3),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.FOLLOWER_MEAT, 2)
          };
        case FollowerClothingType.Normal_4:
          return new ClothingData.CostItem[2]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.COTTON, 4),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.SPIDER_WEB, 8)
          };
        case FollowerClothingType.Normal_5:
          return new ClothingData.CostItem[2]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.COTTON, 6),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.CRYSTAL, 7)
          };
        case FollowerClothingType.Normal_6:
          return new ClothingData.CostItem[2]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.COTTON, 4),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.SPIDER_WEB, 6)
          };
        case FollowerClothingType.Normal_7:
          return new ClothingData.CostItem[2]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.COTTON, 2),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.GRASS, 20)
          };
        case FollowerClothingType.Normal_8:
          return new ClothingData.CostItem[2]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.COTTON, 4),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.CRYSTAL, 5)
          };
        case FollowerClothingType.Normal_9:
          return new ClothingData.CostItem[2]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.COTTON, 4),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.FLOWER_RED, 3)
          };
        case FollowerClothingType.Normal_10:
          return new ClothingData.CostItem[2]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.COTTON, 4),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.BLACK_GOLD, 10)
          };
        case FollowerClothingType.Normal_11:
          return new ClothingData.CostItem[2]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.COTTON, 6),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.BONE, 35)
          };
        case FollowerClothingType.Normal_12:
          return new ClothingData.CostItem[2]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.COTTON, 6),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.SPIDER_WEB, 8)
          };
        case FollowerClothingType.Naked:
          return new ClothingData.CostItem[1]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.PLEASURE_POINT, 1)
          };
        case FollowerClothingType.DLC_6:
          return new ClothingData.CostItem[1]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.PLEASURE_POINT, 1)
          };
        case FollowerClothingType.Pilgrim_DLC:
          return new ClothingData.CostItem[2]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.COTTON, 9),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.SILK_THREAD, 4)
          };
        case FollowerClothingType.Pilgrim_DLC2:
          return new ClothingData.CostItem[2]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.COTTON, 9),
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.SILK_THREAD, 4)
          };
        case FollowerClothingType.Winter_1:
          return new ClothingData.CostItem[1]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.WOOL, 4)
          };
        case FollowerClothingType.Winter_2:
          return new ClothingData.CostItem[1]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.WOOL, 4)
          };
        case FollowerClothingType.Winter_3:
          return new ClothingData.CostItem[1]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.WOOL, 4)
          };
        case FollowerClothingType.Winter_4:
          return new ClothingData.CostItem[1]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.WOOL, 4)
          };
        case FollowerClothingType.Winter_5:
          return new ClothingData.CostItem[1]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.WOOL, 4)
          };
        case FollowerClothingType.Winter_6:
          return new ClothingData.CostItem[1]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.WOOL, 4)
          };
        case FollowerClothingType.Normal_MajorDLC_1:
          return new ClothingData.CostItem[1]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.WOOL, 1)
          };
        case FollowerClothingType.Normal_MajorDLC_2:
          return new ClothingData.CostItem[1]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.WOOL, 1)
          };
        case FollowerClothingType.Normal_MajorDLC_3:
          return new ClothingData.CostItem[1]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.WOOL, 1)
          };
        case FollowerClothingType.Normal_MajorDLC_4:
          return new ClothingData.CostItem[1]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.WOOL, 1)
          };
        case FollowerClothingType.Normal_MajorDLC_5:
          return new ClothingData.CostItem[1]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.WOOL, 1)
          };
        case FollowerClothingType.Normal_MajorDLC_6:
          return new ClothingData.CostItem[1]
          {
            new ClothingData.CostItem(InventoryItem.ITEM_TYPE.WOOL, 1)
          };
        default:
          return new ClothingData.CostItem[0];
      }
    }
  }

  public void ExportData()
  {
    TextWriter textWriter = (TextWriter) new StreamWriter(Application.persistentDataPath + "/Clothing Data.json", false);
    ClothingData[] clothingDataArray = Resources.LoadAll<ClothingData>("Data/Equipment Data/Clothing");
    ClothingData.ClothingDataJson clothingDataJson = new ClothingData.ClothingDataJson();
    clothingDataJson.outfits = new ClothingData.ClothingDataJsonObject[clothingDataArray.Length];
    for (int index1 = 0; index1 < clothingDataArray.Length; ++index1)
    {
      ClothingData.ClothingDataJsonObject clothingDataJsonObject = new ClothingData.ClothingDataJsonObject();
      clothingDataJsonObject.ID = clothingDataArray[index1].ClothingType.ToString();
      clothingDataJsonObject.Variants = clothingDataArray[index1].Variants.ToArray();
      clothingDataJsonObject.Colors = new ClothingData.ClothingDataColorJson[clothingDataArray[index1].SlotAndColours.Count];
      for (int index2 = 0; index2 < clothingDataArray[index1].SlotAndColours.Count; ++index2)
      {
        for (int index3 = 0; index3 < clothingDataArray[index1].SlotAndColours[index2].SlotAndColours.Count; ++index3)
        {
          if (clothingDataArray[index1].SlotAndColours[index2].SlotAndColours[index3].Slot == "SLEEVE_RIGHT_BTM")
            clothingDataJsonObject.Colors[index2].SLEEVE_RIGHT_BTM = "#" + ColorUtility.ToHtmlStringRGB(clothingDataArray[index1].SlotAndColours[index2].SlotAndColours[index3].color);
          else if (clothingDataArray[index1].SlotAndColours[index2].SlotAndColours[index3].Slot == "SLEEVE_RIGHT_TOP")
            clothingDataJsonObject.Colors[index2].SLEEVE_RIGHT_TOP = "#" + ColorUtility.ToHtmlStringRGB(clothingDataArray[index1].SlotAndColours[index2].SlotAndColours[index3].color);
          else if (clothingDataArray[index1].SlotAndColours[index2].SlotAndColours[index3].Slot == "SLEEVE_LEFT_BTM")
            clothingDataJsonObject.Colors[index2].SLEEVE_LEFT_BTM = "#" + ColorUtility.ToHtmlStringRGB(clothingDataArray[index1].SlotAndColours[index2].SlotAndColours[index3].color);
          else if (clothingDataArray[index1].SlotAndColours[index2].SlotAndColours[index3].Slot == "SLEEVE_LEFT_TOP")
            clothingDataJsonObject.Colors[index2].SLEEVE_LEFT_TOP = "#" + ColorUtility.ToHtmlStringRGB(clothingDataArray[index1].SlotAndColours[index2].SlotAndColours[index3].color);
          else if (clothingDataArray[index1].SlotAndColours[index2].SlotAndColours[index3].Slot == "BODY_BTM")
            clothingDataJsonObject.Colors[index2].BODY_BTM = "#" + ColorUtility.ToHtmlStringRGB(clothingDataArray[index1].SlotAndColours[index2].SlotAndColours[index3].color);
          else if (clothingDataArray[index1].SlotAndColours[index2].SlotAndColours[index3].Slot == "BODY_TOP")
            clothingDataJsonObject.Colors[index2].BODY_TOP = "#" + ColorUtility.ToHtmlStringRGB(clothingDataArray[index1].SlotAndColours[index2].SlotAndColours[index3].color);
          else if (clothingDataArray[index1].SlotAndColours[index2].SlotAndColours[index3].Slot == "BODY_EXTRA")
            clothingDataJsonObject.Colors[index2].BODY_EXTRA = "#" + ColorUtility.ToHtmlStringRGB(clothingDataArray[index1].SlotAndColours[index2].SlotAndColours[index3].color);
          else if (clothingDataArray[index1].SlotAndColours[index2].SlotAndColours[index3].Slot == "HOOD_BTM")
            clothingDataJsonObject.Colors[index2].HOOD_BTM = "#" + ColorUtility.ToHtmlStringRGB(clothingDataArray[index1].SlotAndColours[index2].SlotAndColours[index3].color);
          else if (clothingDataArray[index1].SlotAndColours[index2].SlotAndColours[index3].Slot == "HOOD_TOP")
            clothingDataJsonObject.Colors[index2].HOOD_TOP = "#" + ColorUtility.ToHtmlStringRGB(clothingDataArray[index1].SlotAndColours[index2].SlotAndColours[index3].color);
          else if (clothingDataArray[index1].SlotAndColours[index2].SlotAndColours[index3].Slot == "SHAWL_BTM")
            clothingDataJsonObject.Colors[index2].SHAWL_BTM = "#" + ColorUtility.ToHtmlStringRGB(clothingDataArray[index1].SlotAndColours[index2].SlotAndColours[index3].color);
          else if (clothingDataArray[index1].SlotAndColours[index2].SlotAndColours[index3].Slot == "SHAWL_TOP")
            clothingDataJsonObject.Colors[index2].SHAWL_TOP = "#" + ColorUtility.ToHtmlStringRGB(clothingDataArray[index1].SlotAndColours[index2].SlotAndColours[index3].color);
        }
      }
      clothingDataJson.outfits[index1] = clothingDataJsonObject;
    }
    string json = JsonUtility.ToJson((object) clothingDataJson, true);
    textWriter.Write(json);
    textWriter.Close();
  }

  [Serializable]
  public struct CostItem(InventoryItem.ITEM_TYPE ItemType, int Cost)
  {
    public InventoryItem.ITEM_TYPE ItemType = ItemType;
    public int Cost = Cost;
  }

  [Serializable]
  public struct ClothingDataColorJson
  {
    public string SLEEVE_RIGHT_BTM;
    public string SLEEVE_RIGHT_TOP;
    public string SLEEVE_LEFT_BTM;
    public string SLEEVE_LEFT_TOP;
    public string BODY_BTM;
    public string BODY_TOP;
    public string BODY_EXTRA;
    public string HOOD_BTM;
    public string HOOD_TOP;
    public string SHAWL_BTM;
    public string SHAWL_TOP;
  }

  [Serializable]
  public struct ClothingDataJsonObject
  {
    public string ID;
    public string[] Variants;
    public ClothingData.ClothingDataColorJson[] Colors;
  }

  [Serializable]
  public struct ClothingDataJson
  {
    public ClothingData.ClothingDataJsonObject[] outfits;
  }
}
