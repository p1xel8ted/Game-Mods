// Decompiled with JetBrains decompiler
// Type: Lamb.UI.BuildMenu.FaithCategory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private RectTransform _content;
  [Header("Counts")]
  [SerializeField]
  private TextMeshProUGUI _unlocked;

  protected override void Populate()
  {
    this.Populate(FaithCategory.AllStructures(), this._content);
    this.SetUnlockedText(this._unlocked);
  }

  private void SetUnlockedText(TextMeshProUGUI target)
  {
    int num = 0;
    List<StructureBrain.TYPES> typesList = FaithCategory.AllStructures();
    foreach (StructureBrain.TYPES Types in typesList)
    {
      if (StructuresData.GetUnlocked(Types))
        ++num;
    }
    target.text = string.Format(ScriptLocalization.UI.Collected, (object) $"{num}/{typesList.Count}");
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
      StructureBrain.TYPES.OFFERING_STATUE
    };
  }
}
