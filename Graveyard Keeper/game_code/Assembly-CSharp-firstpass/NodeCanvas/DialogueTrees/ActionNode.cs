// Decompiled with JetBrains decompiler
// Type: NodeCanvas.DialogueTrees.ActionNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections;
using UnityEngine;

#nullable disable
namespace NodeCanvas.DialogueTrees;

[Description("Execute an Action Task for the Dialogue Actor selected.")]
[Name("Task Action", 0)]
public class ActionNode : DTNode, ITaskAssignable<ActionTask>, ITaskAssignable
{
  [SerializeField]
  public ActionTask _action;

  public ActionTask action
  {
    get => this._action;
    set => this._action = value;
  }

  public Task task
  {
    get => (Task) this.action;
    set => this.action = (ActionTask) value;
  }

  public override bool requireActorSelection => true;

  public override NodeCanvas.Status OnExecute(Component agent, IBlackboard bb)
  {
    if (this.action == null)
      return this.Error("Action is null on Dialogue Action Node");
    this.status = NodeCanvas.Status.Running;
    this.StartCoroutine(this.UpdateAction((Component) this.finalActor.transform));
    return this.status;
  }

  public IEnumerator UpdateAction(Component actionAgent)
  {
    ActionNode actionNode = this;
    while (actionNode.status == NodeCanvas.Status.Running)
    {
      NodeCanvas.Status status = actionNode.action.ExecuteAction(actionAgent, actionNode.graphBlackboard);
      if (status != NodeCanvas.Status.Running)
      {
        actionNode.OnActionEnd(status == NodeCanvas.Status.Success);
        break;
      }
      yield return (object) null;
    }
  }

  public void OnActionEnd(bool success)
  {
    if (success)
    {
      this.status = NodeCanvas.Status.Success;
      this.DLGTree.Continue();
    }
    else
    {
      this.status = NodeCanvas.Status.Failure;
      this.DLGTree.Stop(false);
    }
  }

  public override void OnReset()
  {
    if (this.action == null)
      return;
    this.action.EndAction(new bool?());
  }

  public override void OnGraphPaused()
  {
    if (this.action == null)
      return;
    this.action.PauseAction();
  }
}
