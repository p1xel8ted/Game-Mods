// Decompiled with JetBrains decompiler
// Type: GameLogics
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class GameLogics
{
  [SerializeField]
  public List<LogicData> _logics = new List<LogicData>();
  [SerializeField]
  public List<string> _black_list = new List<string>();
  [SerializeField]
  public bool _initialized;

  public void Init()
  {
    List<LogicData> logicDataList = new List<LogicData>();
    foreach (LogicDefinition logicDefinition in GameBalance.me.logics_data)
      logicDataList.Add(this.GetLogicByID(logicDefinition.id) ?? new LogicData(logicDefinition.id));
    this._logics = logicDataList;
    this._initialized = true;
  }

  public LogicData GetLogicByID(string id)
  {
    foreach (LogicData logic in this._logics)
    {
      if (logic.id == id)
        return logic;
    }
    return (LogicData) null;
  }

  public void Update()
  {
    if (!this._initialized || !MainGame.game_started || MainGame.paused || (UnityEngine.Object) EnvironmentEngine.me != (UnityEngine.Object) null && EnvironmentEngine.me.IsTimeStopped())
      return;
    float deltaTime = Time.deltaTime;
    LogicData logicData = (LogicData) null;
    foreach (LogicData logic in this._logics)
    {
      if (!this._black_list.Contains(logic.id))
      {
        if (logic.definition == null)
        {
          Debug.LogError((object) $"Logic definition not found for id = {logic.id}, removing logics");
          logicData = logic;
          break;
        }
        logic.Update(deltaTime);
      }
    }
    if (logicData == null)
      return;
    this._logics.Remove(logicData);
  }

  public bool AddToBlackList(string logic_id)
  {
    if (this._black_list.Contains(logic_id))
      return false;
    this._black_list.Add(logic_id);
    return true;
  }

  public bool ForceExecute(string id) => this.ForceExecute(id, false);

  public bool ForceExecuteCond(string id) => this.ForceExecute(id, true);

  public bool ForceExecute(string id, bool check_condition)
  {
    if (check_condition && this._black_list.Contains(id))
      return false;
    LogicData logicById = this.GetLogicByID(id);
    if (logicById != null)
      return logicById.ForceExecute(check_condition);
    Debug.LogError((object) ("Force logic execute fail, no logic with id: " + id));
    return false;
  }
}
