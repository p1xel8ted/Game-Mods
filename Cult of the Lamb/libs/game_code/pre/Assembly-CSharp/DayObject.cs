// Decompiled with JetBrains decompiler
// Type: DayObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class DayObject
{
  public int MoonPhase;
  public int TotalDays;

  public void Init(int MoonPhase, int TotalDays)
  {
    this.MoonPhase = MoonPhase;
    this.TotalDays = TotalDays;
  }

  public static void Reset()
  {
    DataManager.Instance.DayList = new List<DayObject>();
    int MoonPhase = -1;
    while (++MoonPhase < 3)
    {
      DayObject dayObject = new DayObject();
      dayObject.Init(MoonPhase, MoonPhase + 1);
      DataManager.Instance.DayList.Add(dayObject);
    }
    DataManager.Instance.CurrentDay = DataManager.Instance.DayList[0];
  }

  public static void NewDay()
  {
    if (DataManager.Instance.DayList.Count <= 0)
      return;
    DataManager.Instance.DayList.RemoveAt(0);
    DataManager.Instance.CurrentDay = DataManager.Instance.DayList[0];
    DayObject day = DataManager.Instance.DayList[DataManager.Instance.DayList.Count - 1];
    int MoonPhase = (day.MoonPhase + 1) % 6;
    int TotalDays = day.TotalDays + 1;
    DayObject dayObject = new DayObject();
    dayObject.Init(MoonPhase, TotalDays);
    DataManager.Instance.DayList.Add(dayObject);
  }
}
