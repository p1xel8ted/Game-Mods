// Decompiled with JetBrains decompiler
// Type: QuestListItemGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class QuestListItemGUI : MonoBehaviour
{
  public UILabel txt;
  public UIWidget fx_point;
  public string _quest_id;
  public static Dictionary<string, UIWidget> _effects = new Dictionary<string, UIWidget>();

  public void Draw(QuestState qs, bool is_new = false)
  {
    this._quest_id = qs.definition.id;
    this.txt.text = GJL.L("qt_" + this._quest_id) + "(quest)";
    if (is_new)
    {
      this.ShowNewQuestEffect();
    }
    else
    {
      Debug.Log((object) ("re-linking effect for q = " + this._quest_id), (Object) this);
      if (!QuestListItemGUI._effects.ContainsKey(this._quest_id))
        return;
      Debug.Log((object) "re-link found", (Object) QuestListItemGUI._effects[this._quest_id]);
      QuestListItemGUI._effects[this._quest_id].SetAnchor(this.fx_point.gameObject);
    }
  }

  public void ShowNewQuestEffect()
  {
    if ((Object) GUIElements.me.quest_list.new_quest_fx_prefab == (Object) null)
      return;
    GameObject go = GUIElements.me.quest_list.new_quest_fx_prefab.Copy(this.fx_point.transform);
    go.transform.localPosition = Vector3.zero;
    go.SetActive(true);
    go.transform.SetParent(GUIElements.me.quest_list.transform, false);
    UIWidget component = go.GetComponent<UIWidget>();
    component.SetAnchor(this.fx_point.gameObject);
    QuestListItemGUI._effects.Add(this._quest_id, component);
    GJTimer.AddTimer(5f, (GJTimer.VoidDelegate) (() =>
    {
      if (!((Object) go != (Object) null) || !((Object) go.gameObject != (Object) null) || !((Object) go.transform.parent != (Object) null))
        return;
      QuestListItemGUI._effects.Remove(this._quest_id);
      Object.Destroy((Object) go);
    }));
  }
}
