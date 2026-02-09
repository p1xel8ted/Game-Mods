// Decompiled with JetBrains decompiler
// Type: LogicDefinition
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class LogicDefinition : BalanceBaseObject
{
  public SmartExpression condition = new SmartExpression();
  public SmartExpression condition_1 = new SmartExpression();
  public SmartExpression condition_2 = new SmartExpression();
  public List<SmartExpression> expressions = new List<SmartExpression>();
  public List<SmartExpression> execute_expressions = new List<SmartExpression>();
  public List<string> execute_scripts = new List<string>();
  public List<string> scripts_1 = new List<string>();
  public List<string> scripts_2 = new List<string>();
  public List<LogicDefinition.ExecutableEvent> execute_events = new List<LogicDefinition.ExecutableEvent>();
  public float start_time;
  public float period_time;

  [Serializable]
  public struct ExecutableEvent
  {
    public string custom_tag;
    public string event_name;
  }
}
