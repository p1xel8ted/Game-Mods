// Decompiled with JetBrains decompiler
// Type: TaskAndTime
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public class TaskAndTime
{
  public FollowerTaskType Task;
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
