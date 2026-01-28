// Decompiled with JetBrains decompiler
// Type: TaskAndTime
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class TaskAndTime
{
  [Key(0)]
  public FollowerTaskType Task;
  [Key(1)]
  public float Time;

  public static void SetTaskTime(float time, FollowerTaskType task, FollowerBrain Brain)
  {
    foreach (TaskAndTime taskAndTime in Brain._taskMemory)
    {
      if (taskAndTime.Task == task)
      {
        taskAndTime.Time = time;
        return;
      }
    }
    Brain._taskMemory.Add(new TaskAndTime()
    {
      Task = task,
      Time = time
    });
  }

  public static float GetLastTaskTime(FollowerTaskType task, FollowerBrain Brain)
  {
    foreach (TaskAndTime taskAndTime in Brain._taskMemory)
    {
      if (taskAndTime.Task == task)
        return taskAndTime.Time;
    }
    return 0.0f;
  }
}
