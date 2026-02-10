// Decompiled with JetBrains decompiler
// Type: Structures_Temple
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class Structures_Temple : StructureBrain
{
  public List<FollowerTask_Study> Studiers = new List<FollowerTask_Study>();
  public const int AvailableStudySlotsMax = 4;
  public const float TempleDurationForXP = 12f;
  public const float TempleIncrementXP = 0.05f;

  public bool StudyAvailable
  {
    get
    {
      return this.Studiers.Count < Structures_Temple.AvailableStudySlots && (double) DataManager.Instance.TempleStudyXP < (double) Structures_Temple.TempleMaxStudyXP;
    }
  }

  public static int AvailableStudySlots
  {
    get => !UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Temple_MonksUpgrade) ? 2 : 4;
  }

  public static float TempleMaxStudyXP
  {
    get => UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Temple_MonksUpgrade) ? 12f : 6f;
  }

  public bool CheckOverrideComplete() => false;

  public void AddStudier(FollowerTask_Study studier)
  {
    if (this.Studiers.Contains(studier))
      return;
    this.Studiers.Add(studier);
  }

  public void RemoveStudier(FollowerTask_Study studier)
  {
    if (!this.Studiers.Contains(studier))
      return;
    this.Studiers.Remove(studier);
  }
}
