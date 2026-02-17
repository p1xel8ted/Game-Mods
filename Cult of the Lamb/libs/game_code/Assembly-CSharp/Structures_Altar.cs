// Decompiled with JetBrains decompiler
// Type: Structures_Altar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class Structures_Altar : StructureBrain, ITaskProvider
{
  public DayPhase OverridePhase = DayPhase.None;

  public FollowerTask GetOverrideTask(FollowerBrain brain)
  {
    return (FollowerTask) new FollowerTask_AttendTeaching();
  }

  public bool CheckOverrideComplete()
  {
    return DataManager.Instance.PreviousSermonDayIndex >= TimeManager.CurrentDay;
  }

  public void GetAvailableTasks(ScheduledActivity activity, SortedList<float, FollowerTask> tasks)
  {
  }
}
