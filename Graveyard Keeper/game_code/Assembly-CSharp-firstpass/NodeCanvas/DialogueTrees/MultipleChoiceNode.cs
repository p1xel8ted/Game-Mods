// Decompiled with JetBrains decompiler
// Type: NodeCanvas.DialogueTrees.MultipleChoiceNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace NodeCanvas.DialogueTrees;

[Category("Branch")]
[ParadoxNotion.Design.Icon("List", false, "")]
[Color("b3ff7f")]
[Description("Prompt a Dialogue Multiple Choice. A choice will be available if the connection's condition is true or there is no condition on that connection. The Actor selected is used for the Condition checks as well as will Say the selection if the option is checked.")]
[Name("Multiple Choice", 0)]
public class MultipleChoiceNode : DTNode, ISubTasksContainer
{
  [SliderField(0.0f, 10f)]
  public float availableTime;
  public bool saySelection;
  [SerializeField]
  public List<MultipleChoiceNode.Choice> availableChoices = new List<MultipleChoiceNode.Choice>();

  public Task[] GetSubTasks()
  {
    return this.availableChoices == null ? new Task[0] : (Task[]) this.availableChoices.Select<MultipleChoiceNode.Choice, ConditionTask>((Func<MultipleChoiceNode.Choice, ConditionTask>) (c => c.condition)).ToArray<ConditionTask>();
  }

  public override int maxOutConnections => this.availableChoices.Count;

  public override bool requireActorSelection => true;

  public override NodeCanvas.Status OnExecute(Component agent, IBlackboard bb)
  {
    if (this.outConnections.Count == 0)
      return this.Error("There are no connections to the Multiple Choice Node!");
    Dictionary<IStatement, int> options = new Dictionary<IStatement, int>();
    for (int index = 0; index < this.availableChoices.Count; ++index)
    {
      ConditionTask condition = this.availableChoices[index].condition;
      if (condition == null || condition.CheckCondition((Component) this.finalActor.transform, bb))
      {
        Statement key = this.availableChoices[index].statement.BlackboardReplace(bb);
        options[(IStatement) key] = index;
      }
    }
    if (options.Count == 0)
    {
      Debug.Log((object) "Multiple Choice Node has no available options. Dialogue Ends");
      this.DLGTree.Stop(false);
      return NodeCanvas.Status.Failure;
    }
    DialogueTree.RequestMultipleChoices(new MultipleChoiceRequestInfo(this.finalActor, options, this.availableTime, new Action<int>(this.OnOptionSelected))
    {
      showLastStatement = this.inConnections.Count > 0 && this.inConnections[0].sourceNode is StatementNode
    });
    return NodeCanvas.Status.Running;
  }

  public void OnOptionSelected(int index)
  {
    this.status = NodeCanvas.Status.Success;
    Action callback = (Action) (() => this.DLGTree.Continue(index));
    if (this.saySelection)
      DialogueTree.RequestSubtitles(new SubtitlesRequestInfo(this.finalActor, (IStatement) this.availableChoices[index].statement.BlackboardReplace(this.graphBlackboard), callback));
    else
      callback();
  }

  [Serializable]
  public class Choice
  {
    public bool isUnfolded = true;
    public Statement statement;
    public ConditionTask condition;

    public Choice()
    {
    }

    public Choice(Statement statement) => this.statement = statement;
  }
}
