// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Alerts.RitualAlert
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace Lamb.UI.Alerts;

public class RitualAlert : AlertBadge<UpgradeSystem.Type>
{
  protected override AlertCategory<UpgradeSystem.Type> _source
  {
    get => (AlertCategory<UpgradeSystem.Type>) DataManager.Instance.Alerts.Rituals;
  }

  protected override bool HasAlertSingle()
  {
    if (this.HasAlertTotal())
    {
      if (UpgradeSystem.GetUnlocked(this._alert) && ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.PerformAnyRitual))
        return true;
      List<Objectives_PerformRitual> objectivesOfType = ObjectiveManager.GetObjectivesOfType<Objectives_PerformRitual>();
      if (objectivesOfType != null)
      {
        foreach (Objectives_PerformRitual objectivesPerformRitual in objectivesOfType)
        {
          if (objectivesPerformRitual.Ritual == this._alert)
            return true;
        }
      }
    }
    return base.HasAlertSingle();
  }

  protected override bool HasAlertTotal()
  {
    return ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.PerformAnyRitual) || ObjectiveManager.HasCustomObjective<Objectives_PerformRitual>() || base.HasAlertTotal();
  }
}
