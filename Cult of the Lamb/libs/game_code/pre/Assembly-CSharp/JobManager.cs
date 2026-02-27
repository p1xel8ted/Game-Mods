// Decompiled with JetBrains decompiler
// Type: JobManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
