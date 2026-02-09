// Decompiled with JetBrains decompiler
// Type: KnownNPC
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class KnownNPC
{
  public string npc_id;
  public List<KnownNPC.TaskState> tasks = new List<KnownNPC.TaskState>();

  public void SetQuestState(string task_id, KnownNPC.TaskState.State state)
  {
    Stats.DesignEvent($"Task:{task_id}:{state.ToString()}");
    foreach (KnownNPC.TaskState task in this.tasks)
    {
      if (task.id == task_id)
      {
        task.state = state;
        return;
      }
    }
    this.tasks.Add(new KnownNPC.TaskState()
    {
      id = task_id,
      state = state
    });
  }

  public KnownNPC.TaskState.State GetQuestState(string task_id)
  {
    foreach (KnownNPC.TaskState task in this.tasks)
    {
      if (task.id == task_id)
        return task.state;
    }
    return KnownNPC.TaskState.State.Unknown;
  }

  public int sort_order
  {
    get
    {
      if (this.npc_id == "player")
        return 0;
      ObjectDefinition dataOrNull = GameBalance.me.GetDataOrNull<ObjectDefinition>(this.npc_id);
      return dataOrNull != null ? dataOrNull.sort_n : 999999;
    }
  }

  [Serializable]
  public class TaskState
  {
    public KnownNPC.TaskState.State state;
    public string id;

    public string GetTaskText()
    {
      return LocalizedLabel.ColorizeTags(GJL.L("task_" + this.id), LocalizedLabel.TextColor.SpeechBubble);
    }

    public bool is_dlc_stories_task => this.id.StartsWith("dlc_stories_");

    public bool is_dlc_refugee_task
    {
      get => this.id.StartsWith("dlc_refugees") || this.id.StartsWith("s_ev");
    }

    public bool is_dlc_souls_task => this.id.StartsWith("dlc_souls");

    [Serializable]
    public enum State
    {
      Visible,
      Complete,
      Unknown,
    }
  }
}
