// Decompiled with JetBrains decompiler
// Type: LogicData
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class LogicData
{
  public string id;
  [SerializeField]
  public float _next_execution_time;
  [SerializeField]
  public bool _started;
  public bool _executing;
  public List<string> _running_scripts = new List<string>();
  public List<string> _waiting_finish_scripts = new List<string>();
  public LogicDefinition _definition;

  public LogicData()
  {
  }

  public LogicDefinition definition
  {
    get
    {
      if (this._definition == null)
        this._definition = GameBalance.me.GetData<LogicDefinition>(this.id);
      return this._definition;
    }
  }

  public LogicData(string id)
  {
    this.id = id;
    this._next_execution_time = this.GetNextExecutionTime();
  }

  public float GetNextExecutionTime()
  {
    if (string.IsNullOrEmpty(this.id))
      return 0.0f;
    if (this.definition == null)
    {
      Debug.LogError((object) $"Definition for logic \"{this.id}\" is NULL!");
      return 0.0f;
    }
    float nextExecutionTime = (Mathf.Floor(Mathf.Max(-this.definition.period_time, MainGame.game_time - this.definition.start_time) / this.definition.period_time) + 1f) * this.definition.period_time + this.definition.start_time;
    if (this._started && (double) Mathf.Abs(nextExecutionTime - this._next_execution_time) < (double) this.definition.period_time - 0.0099999997764825821)
    {
      Debug.LogWarning((object) $"Something wrong with GameLogics \"{this.id}\".[{nextExecutionTime.ToString()} <=> {this._next_execution_time.ToString()} ]");
      nextExecutionTime += this.definition.period_time;
    }
    return nextExecutionTime;
  }

  public void Update(float delta_time)
  {
    if (!this.CheckPeriod(delta_time))
      return;
    foreach (SmartExpression expression in this.definition.expressions)
      expression.Evaluate();
    if (this.definition.condition.has_expression && !this.definition.condition.EvaluateChance())
      return;
    this.Execute();
  }

  public bool ForceExecute(bool check_condition)
  {
    if (this._executing)
      return false;
    foreach (SmartExpression expression in this.definition.expressions)
      expression.Evaluate();
    if (check_condition && !this.definition.condition.EvaluateChance())
      return false;
    this._next_execution_time = this.GetNextExecutionTime();
    this.Execute();
    return true;
  }

  public void Execute()
  {
    this._started = true;
    if (this._executing)
    {
      Debug.LogError((object) $"<color=red>Execute logic error:</color> {this.definition.id} is now already executing");
    }
    else
    {
      Debug.Log((object) $"<color=yellow>Execute logic:</color> \"{this.definition.id}\" at {MainGame.game_time.ToString()}");
      foreach (SmartExpression executeExpression in this.definition.execute_expressions)
        executeExpression.Evaluate();
      this.RunScripts(this.definition.execute_scripts);
      if (this.definition.execute_events != null && this.definition.execute_events.Count > 0)
      {
        foreach (LogicDefinition.ExecutableEvent executeEvent in this.definition.execute_events)
        {
          List<WorldGameObject> objectsByCustomTag = WorldMap.GetWorldGameObjectsByCustomTag(executeEvent.custom_tag);
          if (objectsByCustomTag != null && objectsByCustomTag.Count != 0)
          {
            foreach (WorldGameObject worldGameObject in objectsByCustomTag)
            {
              WorldGameObject t_wgo = worldGameObject;
              LogicDefinition.ExecutableEvent t_ev = executeEvent;
              GJTimer.AddTimer(0.01f, (GJTimer.VoidDelegate) (() => t_wgo.FireEvent(t_ev.event_name)));
            }
          }
        }
      }
      if (!this.definition.condition_1.has_expression || !this.definition.condition_1.EvaluateChance())
        return;
      this.RunScripts(this.definition.scripts_1);
      if (!this.definition.condition_2.has_expression || !this.definition.condition_2.EvaluateChance())
        return;
      this.RunScripts(this.definition.scripts_2);
    }
  }

  public bool CheckPeriod(float delta_time)
  {
    if (this._next_execution_time.EqualsTo(0.0f) || (double) this._next_execution_time > (double) MainGame.game_time)
      return false;
    if ((double) this.definition.period_time <= 0.0099999997764825821)
      return !this._started;
    this._next_execution_time = this.GetNextExecutionTime();
    return true;
  }

  public void RunScripts(List<string> scripts)
  {
    if (scripts.Count == 1 && scripts[0][0] == ':')
    {
      FlowScriptEngine.SendEvent(scripts[0].Substring(1));
    }
    else
    {
      if (scripts.Count > 0)
      {
        this._executing = true;
        foreach (string script in scripts)
        {
          if (script != ">")
            this._waiting_finish_scripts.Add(script);
        }
      }
      foreach (string script in scripts)
      {
        if (script == ">")
          break;
        this.RunScript(script, scripts);
      }
    }
  }

  public void OnScriptFinished(List<string> scripts_list, string script)
  {
    this._running_scripts.Remove(script);
    this._waiting_finish_scripts.Remove(script);
    if (this._waiting_finish_scripts.Count == 0)
      this._executing = false;
    int index1 = scripts_list.IndexOf(script) + 1;
    if (index1 >= scripts_list.Count - 1 || scripts_list[index1] != ">")
      return;
    for (int index2 = index1 + 1; index2 < scripts_list.Count && !(scripts_list[index2] == ">"); ++index2)
      this.RunScript(scripts_list[index2], scripts_list);
  }

  public void RunScript(string script, List<string> scripts)
  {
    if ((UnityEngine.Object) GS.RunFlowScript(script, (CustomFlowScript.OnFinishedDelegate) (finished_script => this.OnScriptFinished(scripts, finished_script))) != (UnityEngine.Object) null)
    {
      this._running_scripts.Add(script);
    }
    else
    {
      this._waiting_finish_scripts.Remove(script);
      if (this._waiting_finish_scripts.Count != 0)
        return;
      this._executing = false;
    }
  }
}
