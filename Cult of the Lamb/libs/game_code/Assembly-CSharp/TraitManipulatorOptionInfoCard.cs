// Decompiled with JetBrains decompiler
// Type: TraitManipulatorOptionInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using TMPro;
using UnityEngine;

#nullable disable
public class TraitManipulatorOptionInfoCard : UIInfoCardBase<UITraitManipulatorMenuController.Type>
{
  [SerializeField]
  public TMP_Text header;
  [SerializeField]
  public TMP_Text description;

  public override void Configure(UITraitManipulatorMenuController.Type config)
  {
    this.header.text = LocalizationManager.GetTranslation($"UI/TraitManipulator/{config}");
    this.description.text = LocalizationManager.GetTranslation($"UI/TraitManipulator/{config}/Description");
    StructureBrain.TYPES type = this.GetComponentInParent<UITraitManipulatorMenuController>().StructureBrain.Data.Type;
    if (config == UITraitManipulatorMenuController.Type.Remove && type == StructureBrain.TYPES.TRAIT_MANIPULATOR_1)
    {
      TMP_Text description = this.description;
      description.text = $"{description.text}<br><br><color=#FFD201>{string.Format(ScriptLocalization.UI_UpgradeTree.Requires, (object) StructuresData.LocalizedName(StructureBrain.TYPES.TRAIT_MANIPULATOR_2))}";
    }
    else
    {
      if (config != UITraitManipulatorMenuController.Type.Add || type != StructureBrain.TYPES.TRAIT_MANIPULATOR_1 && type != StructureBrain.TYPES.TRAIT_MANIPULATOR_2)
        return;
      TMP_Text description = this.description;
      description.text = $"{description.text}<br><br><color=#FFD201>{string.Format(ScriptLocalization.UI_UpgradeTree.Requires, (object) StructuresData.LocalizedName(StructureBrain.TYPES.TRAIT_MANIPULATOR_3))}";
    }
  }
}
