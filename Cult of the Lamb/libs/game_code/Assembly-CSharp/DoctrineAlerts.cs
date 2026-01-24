// Decompiled with JetBrains decompiler
// Type: DoctrineAlerts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using System;
using System.Collections.Generic;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class DoctrineAlerts : AlertCategory<DoctrineUpgradeSystem.DoctrineType>
{
  public override bool IsValidAlert(DoctrineUpgradeSystem.DoctrineType alert)
  {
    switch (alert)
    {
      case DoctrineUpgradeSystem.DoctrineType.Special_Sacrifice:
      case DoctrineUpgradeSystem.DoctrineType.Special_ReadMind:
      case DoctrineUpgradeSystem.DoctrineType.Special_Bonfire:
      case DoctrineUpgradeSystem.DoctrineType.Special_EmbraceRot:
      case DoctrineUpgradeSystem.DoctrineType.Special_RejectRot:
      case DoctrineUpgradeSystem.DoctrineType.Special_HealingTouch:
        return false;
      default:
        return true;
    }
  }

  public List<DoctrineUpgradeSystem.DoctrineType> GetAlertsForCategory(SermonCategory sermonCategory)
  {
    List<DoctrineUpgradeSystem.DoctrineType> alertsForCategory = new List<DoctrineUpgradeSystem.DoctrineType>();
    foreach (DoctrineUpgradeSystem.DoctrineType alert in this._alerts)
    {
      if (DoctrineUpgradeSystem.GetCategory(alert) == sermonCategory)
        alertsForCategory.Add(alert);
    }
    return alertsForCategory;
  }
}
