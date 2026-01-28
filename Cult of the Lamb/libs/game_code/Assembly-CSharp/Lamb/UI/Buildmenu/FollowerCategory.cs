// Decompiled with JetBrains decompiler
// Type: Lamb.UI.BuildMenu.FollowerCategory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI.BuildMenu;

public class FollowerCategory : BuildMenuCategory
{
  [Header("Content")]
  [SerializeField]
  public RectTransform _miscContent;
  [SerializeField]
  public RectTransform _foodContent;
  [SerializeField]
  public RectTransform _itemsContent;
  [Header("Counts")]
  [SerializeField]
  public TextMeshProUGUI _miscUnlocked;
  [SerializeField]
  public TextMeshProUGUI _foodUnlocked;
  [SerializeField]
  public TextMeshProUGUI _itemsUnlocked;

  public override void Populate()
  {
    this.Populate(FollowerCategory.GetStructuresForCategory(FollowerCategory.Category.Misc), this._miscContent);
    this.Populate(FollowerCategory.GetStructuresForCategory(FollowerCategory.Category.Food), this._foodContent);
    this.Populate(FollowerCategory.GetStructuresForCategory(FollowerCategory.Category.Items), this._itemsContent);
    this.SetUnlockedText(this._miscUnlocked, FollowerCategory.Category.Misc);
    this.SetUnlockedText(this._foodUnlocked, FollowerCategory.Category.Food);
    this.SetUnlockedText(this._itemsUnlocked, FollowerCategory.Category.Items);
  }

  public void SetUnlockedText(TextMeshProUGUI target, FollowerCategory.Category category)
  {
    int num1 = 0;
    int num2 = 0;
    foreach (StructureBrain.TYPES types in FollowerCategory.GetStructuresForCategory(category))
    {
      if (StructuresData.GetUnlocked(types))
      {
        ++num2;
        ++num1;
      }
      else if (!StructuresData.HiddenUntilUnlocked(types))
        ++num2;
    }
    string str = LocalizeIntegration.ReverseText($"{num1}/{num2}");
    target.text = string.Format(ScriptLocalization.UI.Collected, (object) str);
  }

  public static List<StructureBrain.TYPES> GetStructuresForCategory(
    FollowerCategory.Category category)
  {
    switch (category)
    {
      case FollowerCategory.Category.Misc:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.BED,
          StructureBrain.TYPES.BED_2,
          StructureBrain.TYPES.BED_3,
          StructureBrain.TYPES.SHARED_HOUSE,
          StructureBrain.TYPES.COOKING_FIRE,
          StructureBrain.TYPES.KITCHEN,
          StructureBrain.TYPES.BODY_PIT,
          StructureBrain.TYPES.GRAVE,
          StructureBrain.TYPES.OUTHOUSE,
          StructureBrain.TYPES.OUTHOUSE_2,
          StructureBrain.TYPES.JANITOR_STATION,
          StructureBrain.TYPES.JANITOR_STATION_2,
          StructureBrain.TYPES.PRISON,
          StructureBrain.TYPES.PROPAGANDA_SPEAKER,
          StructureBrain.TYPES.DEMON_SUMMONER,
          StructureBrain.TYPES.DEMON_SUMMONER_2,
          StructureBrain.TYPES.DEMON_SUMMONER_3,
          StructureBrain.TYPES.HEALING_BAY,
          StructureBrain.TYPES.HEALING_BAY_2,
          StructureBrain.TYPES.MORGUE_1,
          StructureBrain.TYPES.MORGUE_2,
          StructureBrain.TYPES.CRYPT_1,
          StructureBrain.TYPES.CRYPT_2,
          StructureBrain.TYPES.CRYPT_3,
          StructureBrain.TYPES.TAILOR,
          StructureBrain.TYPES.LEADER_TENT
        };
      case FollowerCategory.Category.Food:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.FARM_PLOT,
          StructureBrain.TYPES.FARM_STATION,
          StructureBrain.TYPES.FARM_STATION_II,
          StructureBrain.TYPES.SCARECROW,
          StructureBrain.TYPES.SCARECROW_2,
          StructureBrain.TYPES.COMPOST_BIN,
          StructureBrain.TYPES.COMPOST_BIN_DEAD_BODY,
          StructureBrain.TYPES.HARVEST_TOTEM,
          StructureBrain.TYPES.HARVEST_TOTEM_2,
          StructureBrain.TYPES.SILO_SEED,
          StructureBrain.TYPES.SILO_FERTILISER,
          StructureBrain.TYPES.POOP_BUCKET,
          StructureBrain.TYPES.SEED_BUCKET
        };
      case FollowerCategory.Category.Items:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.LUMBERJACK_STATION,
          StructureBrain.TYPES.LUMBERJACK_STATION_2,
          StructureBrain.TYPES.BLOODSTONE_MINE,
          StructureBrain.TYPES.BLOODSTONE_MINE_2,
          StructureBrain.TYPES.REFINERY,
          StructureBrain.TYPES.REFINERY_2
        };
      default:
        return (List<StructureBrain.TYPES>) null;
    }
  }

  public static List<StructureBrain.TYPES> AllStructures()
  {
    List<StructureBrain.TYPES> typesList = new List<StructureBrain.TYPES>();
    typesList.AddRange((IEnumerable<StructureBrain.TYPES>) FollowerCategory.GetStructuresForCategory(FollowerCategory.Category.Misc));
    typesList.AddRange((IEnumerable<StructureBrain.TYPES>) FollowerCategory.GetStructuresForCategory(FollowerCategory.Category.Food));
    typesList.AddRange((IEnumerable<StructureBrain.TYPES>) FollowerCategory.GetStructuresForCategory(FollowerCategory.Category.Items));
    return typesList;
  }

  public enum Category
  {
    Misc,
    Food,
    Items,
  }
}
