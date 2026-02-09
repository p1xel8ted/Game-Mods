// Decompiled with JetBrains decompiler
// Type: StructureAlerts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.BuildMenu;
using MessagePack;
using System;
using System.Collections.Generic;

#nullable disable
[MessagePackObject(false)]
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
      if (StructuresData.CategoryForType(alert) == category && DataManager.Instance.UnlockedStructures.Contains(alert))
        alertsForCategory.Add(alert);
    }
    return alertsForCategory;
  }

  public override bool HasAlert(StructureBrain.TYPES alert)
  {
    return alert == StructureBrain.TYPES.DECORATION_MUSHROOM_SCULPTURE && DataManager.Instance.SozoDecorationQuestActive || base.HasAlert(alert);
  }
}
