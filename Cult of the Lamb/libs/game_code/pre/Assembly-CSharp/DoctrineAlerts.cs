// Decompiled with JetBrains decompiler
// Type: DoctrineAlerts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class DoctrineAlerts : AlertCategory<DoctrineUpgradeSystem.DoctrineType>
{
  protected override bool IsValidAlert(DoctrineUpgradeSystem.DoctrineType alert)
  {
    switch (alert)
    {
      case DoctrineUpgradeSystem.DoctrineType.Special_Sacrifice:
      case DoctrineUpgradeSystem.DoctrineType.Special_ReadMind:
      case DoctrineUpgradeSystem.DoctrineType.Special_Bonfire:
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
