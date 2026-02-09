// Decompiled with JetBrains decompiler
// Type: QuestDefinition
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class QuestDefinition : BalanceBaseObject
{
  public SmartExpression start_trigger = new SmartExpression();
  public SmartExpression success_trigger = new SmartExpression();
  public SmartExpression fail_trigger = new SmartExpression();
  public List<string> start_key;
  public GameRes rew_on_success_params = new GameRes();
  public GameRes rew_on_fail_params = new GameRes();
  public bool one_time_quest;
  public string start_script;
  public string success_script;
  public string fail_script;
  public bool quest_visible;
  public string arrow_wgo_custom_tag;
  public string arrow_wgo_obj_id;
  public List<SmartExpression> success_expressions = new List<SmartExpression>();
  public List<SmartExpression> fail_expressions = new List<SmartExpression>();

  public bool IsReadyToStart(QuestSystem quest_system)
  {
    return (!this.one_time_quest || quest_system == null || !quest_system.CheckIfQuestWasExecuted(this.id)) && this.start_trigger.EvaluateChance();
  }

  public bool IsSucceed() => this.success_trigger.EvaluateChance();

  public bool IsFailed() => this.fail_trigger.EvaluateChance();

  public void InitQuestStartTriggers()
  {
  }

  public void InitQuestEndTriggers()
  {
  }

  public void StartStartingScripts()
  {
    if (string.IsNullOrEmpty(this.start_script))
      return;
    GS.RunFlowScript(this.start_script);
  }

  public void StartSucceedScript()
  {
    if (string.IsNullOrEmpty(this.success_script))
      return;
    GS.RunFlowScript(this.success_script);
  }

  public void StartFailedScript()
  {
    if (string.IsNullOrEmpty(this.fail_script))
      return;
    GS.RunFlowScript(this.fail_script);
  }

  public void ExpressionsSuccess()
  {
    if (this.success_expressions == null || this.success_expressions.Count <= 0)
      return;
    foreach (SmartExpression successExpression in this.success_expressions)
      successExpression?.Evaluate();
  }

  public void ExpressionsFail()
  {
    if (this.fail_expressions == null || this.fail_expressions.Count <= 0)
      return;
    foreach (SmartExpression failExpression in this.fail_expressions)
      failExpression?.Evaluate();
  }
}
