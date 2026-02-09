// Decompiled with JetBrains decompiler
// Type: QuestSystem
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using LinqTools;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class QuestSystem
{
  [SerializeField]
  public List<string> _failed_quests = new List<string>();
  [SerializeField]
  public List<string> _succed_quests = new List<string>();
  [SerializeField]
  public List<QuestState> _currnet_quests = new List<QuestState>();
  [SerializeField]
  public List<string> _executed_quests = new List<string>();
  public Dictionary<string, List<QuestDefinition>> _quests_by_staring_key = new Dictionary<string, List<QuestDefinition>>();
  public bool ending_quests_in_progress;
  public bool starting_quests_in_progress;
  public List<QuestState> _ending_quests = new List<QuestState>();
  public List<QuestDefinition> _quest_ready_to_start = new List<QuestDefinition>();

  public bool IsQuestCurrent(string quest_id)
  {
    return this._currnet_quests.FindIndex((Predicate<QuestState>) (p => p.definition.id == quest_id)) != -1;
  }

  public bool IsQuestFaild(string quest_id) => this._failed_quests.Contains(quest_id);

  public bool IsQuestSucced(string quest_id) => this._succed_quests.Contains(quest_id);

  public void GiveReward(bool succeed, QuestDefinition quest)
  {
    if (succeed)
      MainGame.me.player.AddToParams(quest.rew_on_success_params);
    else
      MainGame.me.player.AddToParams(quest.rew_on_fail_params);
  }

  public bool CheckKeyQuests(string key)
  {
    MainGame.me.save.achievements.CheckKeyQuests(key);
    string empty = string.Empty;
    bool flag = false;
    if (this._quests_by_staring_key.ContainsKey(key))
    {
      List<QuestDefinition> questDefinitionList = this._quests_by_staring_key[key];
      if (questDefinitionList == null)
        return false;
      foreach (QuestDefinition quest in questDefinitionList)
      {
        if (!this.IsQuestInUse(quest) && quest.IsReadyToStart(this))
        {
          Debug.Log((object) ("quest added to starting list id = " + quest.id));
          this._quest_ready_to_start.Add(quest);
          flag = true;
        }
      }
      this.ProcessNextStartingQuest();
    }
    this.CheckQuestsState();
    return flag;
  }

  public void StartQuest(QuestDefinition quest)
  {
    Debug.Log((object) ("Start Quest id = " + quest.id));
    QuestState quest_state = new QuestState();
    quest_state.definition = quest;
    this._currnet_quests.Add(quest_state);
    this.OnQuestStart(quest_state);
  }

  public void ForceQuestEnd(string quest_id, bool succesfull_finish)
  {
    Debug.Log((object) $"Force Quest end id = {quest_id}, succesfull_finish = {succesfull_finish.ToString()}");
    for (int index = 0; index < this._currnet_quests.Count; ++index)
    {
      QuestState currnetQuest = this._currnet_quests[index];
      if (currnetQuest.definition.id == quest_id)
      {
        this._currnet_quests.RemoveAt(index);
        currnetQuest.state = succesfull_finish ? QuestState.State.Succeeded : QuestState.State.InProgress;
        this.EndQuest(currnetQuest);
        return;
      }
    }
    Debug.Log((object) "Couldn't find quest to finish");
  }

  public void OnQuestStart(QuestState quest_state)
  {
    quest_state.definition.InitQuestEndTriggers();
    quest_state.definition.StartStartingScripts();
    this.OnQuestExecuted(quest_state.definition.id);
    this.OnStartScriptFinished();
    if (!((UnityEngine.Object) GUIElements.me.quest_list != (UnityEngine.Object) null))
      return;
    GUIElements.me.quest_list.Redraw();
  }

  public void OnQuestExecuted(string quest_id)
  {
    if (this._executed_quests.Contains(quest_id))
      return;
    this._executed_quests.Add(quest_id);
  }

  public bool CheckIfQuestWasExecuted(string quest_id) => this._executed_quests.Contains(quest_id);

  public void CheckQuestsState()
  {
    int index = 0;
    while (index < this._currnet_quests.Count)
    {
      QuestState currnetQuest = this._currnet_quests[index];
      currnetQuest.state = currnetQuest.CheckQuestProgress();
      if (currnetQuest.state == QuestState.State.InProgress)
      {
        ++index;
      }
      else
      {
        this._ending_quests.Add(currnetQuest);
        this._currnet_quests.RemoveAt(index);
      }
    }
    this.ProcessNextEndingQuest();
  }

  public void ProcessNextEndingQuest()
  {
    if (this._ending_quests.Count == 0 || this.QuestScriptInProgress())
      return;
    this.ending_quests_in_progress = true;
    QuestState q_to_end = this._ending_quests.Last<QuestState>();
    this._ending_quests.RemoveAt(this._ending_quests.Count - 1);
    this.EndQuest(q_to_end);
  }

  public void ProcessNextStartingQuest()
  {
    if (this._quest_ready_to_start.Count == 0 || this.QuestScriptInProgress())
      return;
    this.starting_quests_in_progress = true;
    QuestDefinition quest = this._quest_ready_to_start[0];
    this._quest_ready_to_start.RemoveAt(0);
    this.StartQuest(quest);
  }

  public bool QuestScriptInProgress()
  {
    return this.starting_quests_in_progress && this.ending_quests_in_progress;
  }

  public void EndQuest(QuestState q_to_end)
  {
    Debug.Log((object) $"EndQuest #{q_to_end.definition.id}, success = {q_to_end.state.ToString()}");
    if (q_to_end.state == QuestState.State.Succeeded)
      this.OnQuestSucceed(q_to_end);
    else
      this.OnQuestFailed(q_to_end);
    MainGame.me.save.quests.CheckKeyQuests("quest_finished");
    if (!((UnityEngine.Object) GUIElements.me.quest_list != (UnityEngine.Object) null))
      return;
    GUIElements.me.quest_list.Redraw();
  }

  public void OnQuestSucceed(QuestState q_to_end)
  {
    Debug.Log((object) ("OnQuestSucceed " + q_to_end.definition.id));
    this._succed_quests.Add(q_to_end.definition.id);
    this.GiveReward(true, q_to_end.definition);
    q_to_end.definition.ExpressionsSuccess();
    q_to_end.definition.StartSucceedScript();
    this.OnEndScriptFinished();
  }

  public void OnQuestFailed(QuestState q_to_end)
  {
    Debug.Log((object) ("OnQuestFailed " + q_to_end.definition.id));
    this._failed_quests.Add(q_to_end.definition.id);
    this.GiveReward(false, q_to_end.definition);
    q_to_end.definition.ExpressionsFail();
    q_to_end.definition.StartFailedScript();
    this.OnEndScriptFinished();
  }

  public bool IsQuestInUse(QuestDefinition quest)
  {
    return (this._failed_quests.Contains(quest.id) || this._succed_quests.Contains(quest.id)) && quest.one_time_quest || this.IsQuestCurrent(quest.id);
  }

  public void InitQuestSystem()
  {
    foreach (QuestDefinition questDefinition in GameBalance.me.quests_data)
      questDefinition.InitQuestStartTriggers();
    this.FillQuestCheckDic();
  }

  public void FillQuestCheckDic()
  {
    this._quests_by_staring_key.Clear();
    foreach (QuestDefinition quest in GameBalance.me.quests_data)
    {
      if (!this.IsQuestInUse(quest))
      {
        foreach (string key in quest.start_key)
        {
          if (!this._quests_by_staring_key.ContainsKey(key))
            this._quests_by_staring_key.Add(key, new List<QuestDefinition>());
          this._quests_by_staring_key[key].Add(quest);
        }
      }
    }
  }

  public void OnStartScriptFinished()
  {
    this.starting_quests_in_progress = false;
    this.ProcessNextStartingQuest();
  }

  public void OnEndScriptFinished()
  {
    this.ending_quests_in_progress = false;
    this.ProcessNextEndingQuest();
  }

  public List<QuestState> GetCurrentQuests() => this._currnet_quests;
}
