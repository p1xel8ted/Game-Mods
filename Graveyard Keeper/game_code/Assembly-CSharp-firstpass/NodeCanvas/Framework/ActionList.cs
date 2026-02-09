// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Framework.ActionList
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Framework;

[DoNotList]
public class ActionList : ActionTask
{
  public ActionList.ActionsExecutionMode executionMode;
  public List<ActionTask> actions = new List<ActionTask>();
  public int currentActionIndex;
  public List<int> finishedIndeces = new List<int>();

  public override string info
  {
    get
    {
      if (this.actions.Count == 0)
        return "No Actions";
      string info = string.Empty;
      for (int index = 0; index < this.actions.Count; ++index)
      {
        ActionTask action = this.actions[index];
        if (action != null && action.isActive)
        {
          string str = action.isPaused ? "<b>||</b> " : (action.isRunning ? "► " : "");
          info = info + str + action.summaryInfo + (index == this.actions.Count - 1 ? "" : "\n");
        }
      }
      return info;
    }
  }

  public override Task Duplicate(ITaskSystem newOwnerSystem)
  {
    ActionList actionList = (ActionList) base.Duplicate(newOwnerSystem);
    actionList.actions.Clear();
    foreach (ActionTask action in this.actions)
      actionList.AddAction((ActionTask) action.Duplicate(newOwnerSystem));
    return (Task) actionList;
  }

  public override void OnExecute()
  {
    this.finishedIndeces.Clear();
    this.currentActionIndex = 0;
  }

  public override void OnUpdate()
  {
    if (this.actions.Count == 0)
      this.EndAction();
    else if (this.executionMode == ActionList.ActionsExecutionMode.ActionsRunInParallel)
    {
      for (int index = 0; index < this.actions.Count; ++index)
      {
        if (!this.finishedIndeces.Contains(index))
        {
          if (!this.actions[index].isActive)
          {
            this.finishedIndeces.Add(index);
          }
          else
          {
            switch (this.actions[index].ExecuteAction(this.agent, this.blackboard))
            {
              case NodeCanvas.Status.Failure:
                this.EndAction(false);
                return;
              case NodeCanvas.Status.Success:
                this.finishedIndeces.Add(index);
                continue;
              default:
                continue;
            }
          }
        }
      }
      if (this.finishedIndeces.Count != this.actions.Count)
        return;
      this.EndAction(true);
    }
    else
    {
      for (int currentActionIndex = this.currentActionIndex; currentActionIndex < this.actions.Count; ++currentActionIndex)
      {
        if (this.actions[currentActionIndex].isActive)
        {
          switch (this.actions[currentActionIndex].ExecuteAction(this.agent, this.blackboard))
          {
            case NodeCanvas.Status.Failure:
              this.EndAction(false);
              return;
            case NodeCanvas.Status.Running:
              this.currentActionIndex = currentActionIndex;
              return;
            default:
              continue;
          }
        }
      }
      this.EndAction(true);
    }
  }

  public override void OnStop()
  {
    for (int index = 0; index < this.actions.Count; ++index)
      this.actions[index].EndAction(new bool?());
  }

  public override void OnPause()
  {
    for (int index = 0; index < this.actions.Count; ++index)
      this.actions[index].PauseAction();
  }

  public override void OnDrawGizmos()
  {
    for (int index = 0; index < this.actions.Count; ++index)
      this.actions[index].OnDrawGizmos();
  }

  public override void OnDrawGizmosSelected()
  {
    for (int index = 0; index < this.actions.Count; ++index)
      this.actions[index].OnDrawGizmosSelected();
  }

  public void AddAction(ActionTask action)
  {
    if (action is ActionList)
    {
      Debug.LogWarning((object) "Adding an ActionList within another ActionList is not allowed for clarity");
    }
    else
    {
      this.actions.Add(action);
      action.SetOwnerSystem(this.ownerSystem);
    }
  }

  public enum ActionsExecutionMode
  {
    ActionsRunInSequence,
    ActionsRunInParallel,
  }
}
