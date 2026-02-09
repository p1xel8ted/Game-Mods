// Decompiled with JetBrains decompiler
// Type: QuestListGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Text;
using UnityEngine;

#nullable disable
public class QuestListGUI : MonoBehaviour
{
  public UITable table;
  public List<QuestListItemGUI> _quests = new List<QuestListItemGUI>();
  public QuestListItemGUI _item_prefab;
  public const float REDRAW_RATE = 1f;
  public float _refresh_time;
  public List<string> _shown_quests = new List<string>();
  public GameObject new_quest_fx_prefab;
  public StringBuilder _cur_quests_ids = new StringBuilder();

  public void Init()
  {
    this.table = this.GetComponentInChildren<UITable>(true);
    this._item_prefab = this.GetComponentInChildren<QuestListItemGUI>(true);
    this._item_prefab.gameObject.SetActive(false);
    if (!((Object) this.new_quest_fx_prefab != (Object) null))
      return;
    this.new_quest_fx_prefab.SetActive(false);
  }

  public void ResetAtGameStart() => this._shown_quests.Clear();

  public void Redraw()
  {
    foreach (QuestListItemGUI quest in this._quests)
    {
      quest.transform.SetParent((Transform) null, false);
      Object.Destroy((Object) quest.gameObject);
    }
    this._quests.Clear();
    WorldGameObject wgo = (WorldGameObject) null;
    this._cur_quests_ids.Length = 0;
    foreach (QuestState currentQuest in MainGame.me.save.quests.GetCurrentQuests())
    {
      if (currentQuest.definition.quest_visible)
      {
        this._cur_quests_ids.Append(currentQuest.definition.id);
        this._cur_quests_ids.Append(';');
        bool is_new = false;
        if (this.gameObject.activeInHierarchy && !this._shown_quests.Contains(currentQuest.definition.id))
        {
          this._shown_quests.Add(currentQuest.definition.id);
          is_new = true;
        }
        QuestListItemGUI questListItemGui = this._item_prefab.Copy<QuestListItemGUI>();
        questListItemGui.gameObject.SetActive(true);
        this._quests.Add(questListItemGui);
        questListItemGui.Draw(currentQuest, is_new);
        if (!string.IsNullOrEmpty(currentQuest.definition.arrow_wgo_custom_tag) && (Object) wgo == (Object) null)
          wgo = WorldMap.GetWorldGameObjectByCustomTag(currentQuest.definition.arrow_wgo_custom_tag);
        if (!string.IsNullOrEmpty(currentQuest.definition.arrow_wgo_obj_id) && (Object) wgo == (Object) null)
        {
          List<WorldGameObject> gameObjectsByObjId = WorldMap.GetWorldGameObjectsByObjId(currentQuest.definition.arrow_wgo_obj_id);
          float num = float.MaxValue;
          foreach (WorldGameObject worldGameObject in gameObjectsByObjId)
          {
            float sqrMagnitude = (MainGame.me.player.pos - worldGameObject.pos).sqrMagnitude;
            if ((double) sqrMagnitude < (double) num)
            {
              num = sqrMagnitude;
              wgo = worldGameObject;
            }
          }
        }
      }
    }
    this.table.Reposition();
    this.table.repositionNow = true;
    foreach (UIRect componentsInChild in this.GetComponentsInChildren<UIRect>())
      componentsInChild.UpdateAnchors();
    if (!((Object) MainGame.me.gui_elements.tutorial_arrow != (Object) null))
      return;
    MainGame.me.gui_elements.tutorial_arrow.AttachToWGO(wgo);
  }

  public void Update()
  {
    this._refresh_time -= Time.deltaTime;
    if ((double) this._refresh_time >= 0.0)
      return;
    this._refresh_time = 60f;
    this.Redraw();
  }
}
