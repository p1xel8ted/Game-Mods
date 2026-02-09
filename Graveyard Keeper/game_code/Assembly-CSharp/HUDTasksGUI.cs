// Decompiled with JetBrains decompiler
// Type: HUDTasksGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class HUDTasksGUI : MonoBehaviour
{
  public UITableOrGrid table;
  public HUDTaskItemGUI item_prefab;
  public string _npc_id;
  public List<HUDTaskItemGUI> _items = new List<HUDTaskItemGUI>();
  public List<string> _hidden_tasks = new List<string>();

  public void Init() => this.item_prefab.SetActive(false);

  public void Draw(string npc_id)
  {
    this._npc_id = npc_id;
    if (string.IsNullOrEmpty(npc_id))
    {
      this.gameObject.SetActive(false);
    }
    else
    {
      this.gameObject.SetActive(true);
      this.table.DestroyChildren<HUDTaskItemGUI>(new HUDTaskItemGUI[1]
      {
        this.item_prefab
      });
      this._items.Clear();
      foreach (KnownNPC.TaskState task in MainGame.me.save.known_npcs.GetOrCreateNPC(npc_id).tasks)
      {
        if (task.state != KnownNPC.TaskState.State.Complete)
        {
          HUDTaskItemGUI hudTaskItemGui = this.item_prefab.Copy<HUDTaskItemGUI>();
          hudTaskItemGui.txt.text = task.GetTaskText();
          hudTaskItemGui.quest_marker.text = !task.is_dlc_stories_task ? (!task.is_dlc_refugee_task ? (!task.is_dlc_souls_task ? "(*)" : "(q_souls)") : "(q_marker_refugee)") : "(*2)";
          hudTaskItemGui.linked_task = task;
          if (this._hidden_tasks.Contains(task.id))
            hudTaskItemGui.GetComponent<UIWidget>().alpha = 0.0f;
          this._items.Add(hudTaskItemGui);
        }
      }
      this.table.Reposition();
      this.gameObject.SetActive(false);
      this.gameObject.SetActive(this._items.Count > 0);
    }
  }

  public void Redraw()
  {
    this.Draw(this._npc_id);
    this.GetComponentInParent<UIPanel>().BroadcastMessage("UpdateAnchors");
  }

  public Transform GetMarkerPointOfTask(string npc_id, string task_id)
  {
    if (this._npc_id != npc_id)
      return (Transform) null;
    if (!this.gameObject.activeInHierarchy)
      return (Transform) null;
    foreach (HUDTaskItemGUI hudTaskItemGui in this._items)
    {
      if (hudTaskItemGui.linked_task.id == task_id)
        return hudTaskItemGui.marker_point;
    }
    return (Transform) null;
  }

  public void SetTaskHiddenState(bool hidden, string task_id)
  {
    if (hidden)
    {
      if (this._hidden_tasks.Contains(task_id))
        return;
      this._hidden_tasks.Add(task_id);
    }
    else
    {
      if (this._hidden_tasks.Contains(task_id))
        this._hidden_tasks.Remove(task_id);
      foreach (HUDTaskItemGUI hudTaskItemGui in this._items)
      {
        UIWidget w = hudTaskItemGui.GetComponent<UIWidget>();
        if (hudTaskItemGui.linked_task.id == task_id)
          DOTween.To((DOGetter<float>) (() => w.alpha), (DOSetter<float>) (x => w.alpha = x), 1f, 0.2f);
      }
      this.GetComponentInParent<UIPanel>().BroadcastMessage("UpdateAnchors");
    }
  }
}
