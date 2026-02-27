// Decompiled with JetBrains decompiler
// Type: Lamb.UI.BuildMenu.FaithCategory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI.BuildMenu;

public class FaithCategory : BuildMenuCategory
{
  [Header("Content")]
  [SerializeField]
  public RectTransform _content;
  [Header("Sin")]
  [SerializeField]
  public RectTransform _SinContent;
  [Header("Counts")]
  [SerializeField]
  public TextMeshProUGUI _unlocked;
  [SerializeField]
  public TextMeshProUGUI _sinUnlocked;

  public override void Populate()
  {
    this.Populate(FaithCategory.AllStructures(), this._content);
    this.Populate(FaithCategory.SinStructures(), this._SinContent);
    this.SetUnlockedText(this._unlocked, FaithCategory.Category.General);
    this.SetUnlockedText(this._sinUnlocked, FaithCategory.Category.Sin);
  }

  public void SetUnlockedText(TextMeshProUGUI target, FaithCategory.Category category)
  {
    int num = 0;
    List<StructureBrain.TYPES> structuresForCategory = this.GetStructuresForCategory(category);
    if (structuresForCategory == null)
      return;
    foreach (StructureBrain.TYPES Types in structuresForCategory)
    {
      if (StructuresData.GetUnlocked(Types))
        ++num;
    }
    string str = LocalizeIntegration.ReverseText($"{num}/{structuresForCategory.Count}");
    target.text = string.Format(ScriptLocalization.UI.Collected, (object) str);
  }

  public List<StructureBrain.TYPES> GetStructuresForCategory(FaithCategory.Category category)
  {
    if (category == FaithCategory.Category.General)
      return FaithCategory.AllStructures();
    return category == FaithCategory.Category.Sin ? FaithCategory.SinStructures() : (List<StructureBrain.TYPES>) null;
  }

  public static List<StructureBrain.TYPES> AllStructures()
  {
    return new List<StructureBrain.TYPES>()
    {
      StructureBrain.TYPES.SHRINE,
      StructureBrain.TYPES.TEMPLE,
      StructureBrain.TYPES.CONFESSION_BOOTH,
      StructureBrain.TYPES.MISSIONARY,
      StructureBrain.TYPES.MISSIONARY_II,
      StructureBrain.TYPES.MISSIONARY_III,
      StructureBrain.TYPES.SHRINE_PASSIVE,
      StructureBrain.TYPES.SHRINE_PASSIVE_II,
      StructureBrain.TYPES.SHRINE_PASSIVE_III,
      StructureBrain.TYPES.OFFERING_STATUE,
      StructureBrain.TYPES.SHRINE_DISCIPLE_BOOST,
      StructureBrain.TYPES.SHRINE_DISCIPLE_COLLECTION,
      StructureBrain.TYPES.KNUCKLEBONES_ARENA
    };
  }

  public static List<StructureBrain.TYPES> SinStructures()
  {
    return new List<StructureBrain.TYPES>()
    {
      StructureBrain.TYPES.PUB,
      StructureBrain.TYPES.PUB_2,
      StructureBrain.TYPES.MATING_TENT,
      StructureBrain.TYPES.HATCHERY,
      StructureBrain.TYPES.HATCHERY_2,
      StructureBrain.TYPES.DRUM_CIRCLE,
      StructureBrain.TYPES.DAYCARE
    };
  }

  public enum Category
  {
    General,
    Sin,
  }
}
