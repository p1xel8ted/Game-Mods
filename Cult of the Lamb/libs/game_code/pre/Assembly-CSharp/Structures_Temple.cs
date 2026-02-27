// Decompiled with JetBrains decompiler
// Type: Structures_Temple
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
