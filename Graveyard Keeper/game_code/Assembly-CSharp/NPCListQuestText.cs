// Decompiled with JetBrains decompiler
// Type: NPCListQuestText
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class NPCListQuestText : MonoBehaviour
{
  public UILabel txt;

  public void Draw(KnownNPC.TaskState task_state)
  {
    string str = " ";
    if (GJL.IsEastern())
      str = "";
    if (task_state.is_dlc_stories_task)
      this.txt.text = $"(*2){str}{task_state.GetTaskText()}";
    else if (task_state.is_dlc_refugee_task)
      this.txt.text = $"(q_marker_refugee){str}{task_state.GetTaskText()}";
    else if (task_state.is_dlc_souls_task)
      this.txt.text = $"(q_souls){str}{task_state.GetTaskText()}";
    else
      this.txt.text = $"(*){str}{task_state.GetTaskText()}";
  }
}
