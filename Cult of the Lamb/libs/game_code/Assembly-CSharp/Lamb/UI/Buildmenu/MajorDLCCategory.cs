// Decompiled with JetBrains decompiler
// Type: Lamb.UI.BuildMenu.MajorDLCCategory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI.BuildMenu;

public class MajorDLCCategory : BuildMenuCategory
{
  [Header("Content")]
  [SerializeField]
  public RectTransform _miscContent;
  [SerializeField]
  public RectTransform _winterContent;
  [SerializeField]
  public RectTransform _ranchingContent;
  [Header("Counts")]
  [SerializeField]
  public TextMeshProUGUI _miscUnlocked;
  [SerializeField]
  public TextMeshProUGUI _winterUnlocked;
  [SerializeField]
  public TextMeshProUGUI _ranchingUnlocked;

  public override void Populate()
  {
    this.Populate(MajorDLCCategory.GetStructuresForCategory(MajorDLCCategory.Category.Misc), this._miscContent);
    this.Populate(MajorDLCCategory.GetStructuresForCategory(MajorDLCCategory.Category.Winter), this._winterContent);
    this.Populate(MajorDLCCategory.GetStructuresForCategory(MajorDLCCategory.Category.Ranching), this._ranchingContent);
    this.SetUnlockedText(this._miscUnlocked, MajorDLCCategory.Category.Misc);
    this.SetUnlockedText(this._winterUnlocked, MajorDLCCategory.Category.Winter);
    this.SetUnlockedText(this._ranchingUnlocked, MajorDLCCategory.Category.Ranching);
  }

  public void SetUnlockedText(TextMeshProUGUI target, MajorDLCCategory.Category category)
  {
    int num1 = 0;
    int num2 = 0;
    foreach (StructureBrain.TYPES types in MajorDLCCategory.GetStructuresForCategory(category))
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
    MajorDLCCategory.Category category)
  {
    switch (category)
    {
      case MajorDLCCategory.Category.Misc:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.LOGISTICS,
          StructureBrain.TYPES.MEDIC,
          StructureBrain.TYPES.TOOLSHED,
          StructureBrain.TYPES.TRAIT_MANIPULATOR_1,
          StructureBrain.TYPES.TRAIT_MANIPULATOR_2,
          StructureBrain.TYPES.TRAIT_MANIPULATOR_3
        };
      case MajorDLCCategory.Category.Winter:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.WEATHER_VANE,
          StructureBrain.TYPES.VOLCANIC_SPA,
          StructureBrain.TYPES.LIGHTNING_ROD,
          StructureBrain.TYPES.FURNACE_1,
          StructureBrain.TYPES.FURNACE_2,
          StructureBrain.TYPES.PROXIMITY_FURNACE,
          StructureBrain.TYPES.FURNACE_3,
          StructureBrain.TYPES.ROTSTONE_MINE,
          StructureBrain.TYPES.ROTSTONE_MINE_2
        };
      case MajorDLCCategory.Category.Ranching:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.RANCH,
          StructureBrain.TYPES.RANCH_2,
          StructureBrain.TYPES.RANCH_FENCE,
          StructureBrain.TYPES.RANCH_TROUGH,
          StructureBrain.TYPES.RANCH_HUTCH,
          StructureBrain.TYPES.WOLF_TRAP,
          StructureBrain.TYPES.RACING_GATE,
          StructureBrain.TYPES.RANCH_CHOPPING_BLOCK
        };
      default:
        return (List<StructureBrain.TYPES>) null;
    }
  }

  public static List<StructureBrain.TYPES> AllStructures()
  {
    List<StructureBrain.TYPES> typesList = new List<StructureBrain.TYPES>();
    typesList.AddRange((IEnumerable<StructureBrain.TYPES>) MajorDLCCategory.GetStructuresForCategory(MajorDLCCategory.Category.Winter));
    typesList.AddRange((IEnumerable<StructureBrain.TYPES>) MajorDLCCategory.GetStructuresForCategory(MajorDLCCategory.Category.Misc));
    typesList.AddRange((IEnumerable<StructureBrain.TYPES>) MajorDLCCategory.GetStructuresForCategory(MajorDLCCategory.Category.Ranching));
    return typesList;
  }

  public enum Category
  {
    Misc,
    Winter,
    Ranching,
  }
}
