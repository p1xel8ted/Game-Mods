// Decompiled with JetBrains decompiler
// Type: JobManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class JobManager : BaseMonoBehaviour
{
  public static void NewJob(Vector3 JobLocation, string WorkPlaceID, int WorkPlaceSlot)
  {
    List<Worshipper> worshipperList = new List<Worshipper>();
    foreach (Worshipper worshipper in Worshipper.worshippers)
    {
      if (worshipper.wim.v_i.WorkPlace == WorkPlace.NO_JOB)
        worshipperList.Add(worshipper);
    }
    Worshipper w = (Worshipper) null;
    float num1 = float.MaxValue;
    foreach (Worshipper worshipper in worshipperList)
    {
      float num2 = Vector3.Distance(JobLocation, worshipper.transform.position);
      if ((double) num2 < (double) num1)
      {
        w = worshipper;
        num1 = num2;
      }
    }
    if (!((Object) w != (Object) null))
      return;
    Worshipper.ClearJob(w);
    w.AssignJob(WorkPlaceID, 0);
  }
}
