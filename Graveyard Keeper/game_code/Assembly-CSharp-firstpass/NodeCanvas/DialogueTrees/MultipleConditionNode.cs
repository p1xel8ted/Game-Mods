// Decompiled with JetBrains decompiler
// Type: NodeCanvas.DialogueTrees.MultipleConditionNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace NodeCanvas.DialogueTrees;

[Color("b3ff7f")]
[ParadoxNotion.Design.Icon("Selector", false, "")]
[Name("Multiple Task Condition", 0)]
[Category("Branch")]
[Description("Will continue with the first child node which condition returns true. The Dialogue Actor selected will be used for the checks")]
public class MultipleConditionNode : DTNode, ISubTasksContainer
{
  [SerializeField]
  public List<ConditionTask> conditions = new List<ConditionTask>();

  public override int maxOutConnections => -1;

  public Task[] GetSubTasks() => (Task[]) this.conditions.ToArray();

  public override void OnChildConnected(int index)
  {
    this.conditions.Insert(index, (ConditionTask) null);
  }

  public override void OnChildDisconnected(int index) => this.conditions.RemoveAt(index);

  public override NodeCanvas.Status OnExecute(Component agent, IBlackboard bb)
  {
    if (this.outConnections.Count == 0)
      return this.Error("There are no connections on the Dialogue Condition Node");
    for (int index = 0; index < this.outConnections.Count; ++index)
    {
      if (this.conditions[index] == null || this.conditions[index].CheckCondition((Component) this.finalActor.transform, this.graphBlackboard))
      {
        this.DLGTree.Continue(index);
        return NodeCanvas.Status.Success;
      }
    }
    Debug.LogWarning((object) "No condition is true. Dialogue Stops");
    this.DLGTree.Stop(false);
    return NodeCanvas.Status.Failure;
  }
}
