// Decompiled with JetBrains decompiler
// Type: StructureAlerts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI.BuildMenu;
using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class StructureAlerts : AlertCategory<StructureBrain.TYPES>
{
  public void CheckStructureUnlocked()
  {
    foreach (StructureBrain.TYPES types in Enum.GetValues(typeof (StructureBrain.TYPES)))
    {
      if (StructuresData.GetUnlocked(types))
        this.AddOnce(types);
    }
  }

  public List<StructureBrain.TYPES> GetAlertsForCategory(UIBuildMenuController.Category category)
  {
    List<StructureBrain.TYPES> alertsForCategory = new List<StructureBrain.TYPES>();
    foreach (StructureBrain.TYPES alert in this._alerts)
    {
      if (StructuresData.CategoryForType(alert) == category)
        alertsForCategory.Add(alert);
    }
    return alertsForCategory;
  }

  public override bool HasAlert(StructureBrain.TYPES alert)
  {
    return alert == StructureBrain.TYPES.DECORATION_MUSHROOM_SCULPTURE && DataManager.Instance.SozoDecorationQuestActive || base.HasAlert(alert);
  }
}
