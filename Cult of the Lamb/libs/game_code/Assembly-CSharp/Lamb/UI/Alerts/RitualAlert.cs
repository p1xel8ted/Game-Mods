// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Alerts.RitualAlert
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace Lamb.UI.Alerts;

public class RitualAlert : AlertBadge<UpgradeSystem.Type>
{
  public override AlertCategory<UpgradeSystem.Type> _source
  {
    get => (AlertCategory<UpgradeSystem.Type>) DataManager.Instance.Alerts.Rituals;
  }

  public override bool HasAlertSingle()
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
          if (objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_Divorce)
          {
            if (this._alert == UpgradeSystem.Type.Ritual_FollowerWedding && objectivesPerformRitual.RequiredFollowers == 2 || this._alert == UpgradeSystem.Type.Ritual_Wedding)
              return true;
          }
          else if (objectivesPerformRitual.Ritual == this._alert || objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_FirePit_2 && this._alert == UpgradeSystem.Type.Ritual_FirePit)
            return true;
        }
      }
    }
    return base.HasAlertSingle();
  }

  public override bool HasAlertTotal()
  {
    return ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.PerformAnyRitual) || ObjectiveManager.HasCustomObjective<Objectives_PerformRitual>() || base.HasAlertTotal();
  }
}
