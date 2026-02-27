// Decompiled with JetBrains decompiler
// Type: Lamb.UI.BuildMenu.FollowerCategory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private RectTransform _miscContent;
  [SerializeField]
  private RectTransform _foodContent;
  [SerializeField]
  private RectTransform _itemsContent;
  [Header("Counts")]
  [SerializeField]
  private TextMeshProUGUI _miscUnlocked;
  [SerializeField]
  private TextMeshProUGUI _foodUnlocked;
  [SerializeField]
  private TextMeshProUGUI _itemsUnlocked;

  protected override void Populate()
  {
    this.Populate(FollowerCategory.GetStructuresForCategory(FollowerCategory.Category.Misc), this._miscContent);
    this.Populate(FollowerCategory.GetStructuresForCategory(FollowerCategory.Category.Food), this._foodContent);
    this.Populate(FollowerCategory.GetStructuresForCategory(FollowerCategory.Category.Items), this._itemsContent);
    this.SetUnlockedText(this._miscUnlocked, FollowerCategory.Category.Misc);
    this.SetUnlockedText(this._foodUnlocked, FollowerCategory.Category.Food);
    this.SetUnlockedText(this._itemsUnlocked, FollowerCategory.Category.Items);
  }

  private void SetUnlockedText(TextMeshProUGUI target, FollowerCategory.Category category)
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
    target.text = string.Format(ScriptLocalization.UI.Collected, (object) $"{num1}/{num2}");
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
          StructureBrain.TYPES.COOKING_FIRE,
          StructureBrain.TYPES.BODY_PIT,
          StructureBrain.TYPES.GRAVE,
          StructureBrain.TYPES.OUTHOUSE,
          StructureBrain.TYPES.OUTHOUSE_2,
          StructureBrain.TYPES.JANITOR_STATION,
          StructureBrain.TYPES.PRISON,
          StructureBrain.TYPES.PROPAGANDA_SPEAKER,
          StructureBrain.TYPES.DEMON_SUMMONER,
          StructureBrain.TYPES.DEMON_SUMMONER_2,
          StructureBrain.TYPES.DEMON_SUMMONER_3,
          StructureBrain.TYPES.HEALING_BAY,
          StructureBrain.TYPES.HEALING_BAY_2
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
          StructureBrain.TYPES.SILO_FERTILISER
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
