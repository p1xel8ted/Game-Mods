// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Framework.ConditionList
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using ParadoxNotion.Design;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Framework;

[DoNotList]
public class ConditionList : ConditionTask
{
  public ConditionList.ConditionsCheckMode checkMode;
  public List<ConditionTask> conditions = new List<ConditionTask>();
  public List<ConditionTask> initialActiveConditions;

  public bool allTrueRequired
  {
    get => this.checkMode == ConditionList.ConditionsCheckMode.AllTrueRequired;
  }

  public override string info
  {
    get
    {
      if (this.conditions.Count == 0)
        return "No Conditions";
      string info = $"<b>({(this.allTrueRequired ? "ALL True" : "ANY True")})</b>\n";
      for (int index = 0; index < this.conditions.Count; ++index)
      {
        if (this.conditions[index] != null && (this.conditions[index].isActive || this.initialActiveConditions != null && this.initialActiveConditions.Contains(this.conditions[index])))
          info = info + this.conditions[index].summaryInfo + (index == this.conditions.Count - 1 ? "" : "\n");
      }
      return info;
    }
  }

  public override Task Duplicate(ITaskSystem newOwnerSystem)
  {
    ConditionList conditionList = (ConditionList) base.Duplicate(newOwnerSystem);
    conditionList.conditions.Clear();
    foreach (ConditionTask condition in this.conditions)
      conditionList.AddCondition((ConditionTask) condition.Duplicate(newOwnerSystem));
    return (Task) conditionList;
  }

  public override void OnEnable()
  {
    if (this.initialActiveConditions == null)
      this.initialActiveConditions = this.conditions.Where<ConditionTask>((Func<ConditionTask, bool>) (c => c.isActive)).ToList<ConditionTask>();
    for (int index = 0; index < this.initialActiveConditions.Count; ++index)
      this.initialActiveConditions[index].Enable(this.agent, this.blackboard);
  }

  public override void OnDisable()
  {
    for (int index = 0; index < this.initialActiveConditions.Count; ++index)
      this.initialActiveConditions[index].Disable();
  }

  public override bool OnCheck()
  {
    int num = 0;
    for (int index = 0; index < this.conditions.Count; ++index)
    {
      if (!this.conditions[index].isActive)
        ++num;
      else if (this.conditions[index].CheckCondition(this.agent, this.blackboard))
      {
        if (!this.allTrueRequired)
          return true;
        ++num;
      }
      else if (this.allTrueRequired)
        return false;
    }
    return num == this.conditions.Count;
  }

  public override void OnDrawGizmos()
  {
    foreach (Task condition in this.conditions)
      condition.OnDrawGizmos();
  }

  public override void OnDrawGizmosSelected()
  {
    foreach (Task condition in this.conditions)
      condition.OnDrawGizmosSelected();
  }

  public void AddCondition(ConditionTask condition)
  {
    if (condition is ConditionList)
    {
      Debug.LogWarning((object) "Adding a ConditionList within another ConditionList is not allowed for clarity");
    }
    else
    {
      this.conditions.Add(condition);
      condition.SetOwnerSystem(this.ownerSystem);
    }
  }

  public enum ConditionsCheckMode
  {
    AllTrueRequired,
    AnyTrueSuffice,
  }
}
