// Decompiled with JetBrains decompiler
// Type: NodeCanvas.DialogueTrees.SubDialogueTree
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using NodeCanvas.Framework;
using NodeCanvas.Framework.Internal;
using ParadoxNotion.Design;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace NodeCanvas.DialogueTrees;

[Category("Nested")]
[Name("Sub Dialogue Tree", 0)]
[Description("Execute a Sub Dialogue Tree. When that Dialogue Tree is finished, this node will continue either in Success or Failure if it has any connections.\nUseful for making reusable and self-contained Dialogue Trees.")]
public class SubDialogueTree : DTNode, IGraphAssignable, ISubParametersContainer
{
  [SerializeField]
  public BBParameter<DialogueTree> _subTree;
  [SerializeField]
  public Dictionary<string, string> actorParametersMap = new Dictionary<string, string>();
  [SerializeField]
  public Dictionary<string, BBObjectParameter> variablesMap = new Dictionary<string, BBObjectParameter>();
  public Dictionary<DialogueTree, DialogueTree> instances = new Dictionary<DialogueTree, DialogueTree>();

  public override int maxOutConnections => 2;

  public DialogueTree subTree
  {
    get => this._subTree.value;
    set => this._subTree.value = value;
  }

  Graph IGraphAssignable.nestedGraph
  {
    get => (Graph) this.subTree;
    set => this.subTree = (DialogueTree) value;
  }

  Graph[] IGraphAssignable.GetInstances()
  {
    return (Graph[]) this.instances.Values.ToArray<DialogueTree>();
  }

  BBParameter[] ISubParametersContainer.GetSubParameters()
  {
    return (BBParameter[]) this.variablesMap.Values.ToArray<BBObjectParameter>();
  }

  public override NodeCanvas.Status OnExecute(Component agent, IBlackboard bb)
  {
    if ((UnityEngine.Object) this.subTree == (UnityEngine.Object) null)
      return this.Error("No Sub Dialogue Tree assigned!");
    this.CheckInstance();
    this.SetActorParametersMapping();
    this.SetVariablesMapping();
    this.subTree.StartGraph(this.finalActor is Component ? (Component) this.finalActor : (Component) this.finalActor.transform, bb, true, new Action<bool>(this.OnSubDialogueFinish));
    return NodeCanvas.Status.Running;
  }

  public void SetActorParametersMapping()
  {
    foreach (KeyValuePair<string, string> actorParameters in this.actorParametersMap)
    {
      DialogueTree.ActorParameter parameterById1 = this.subTree.GetParameterByID(actorParameters.Key);
      DialogueTree.ActorParameter parameterById2 = this.DLGTree.GetParameterByID(actorParameters.Value);
      if (parameterById1 != null && parameterById2 != null)
        this.subTree.SetActorReference(parameterById1.name, parameterById2.actor);
    }
  }

  public void SetVariablesMapping()
  {
    foreach (KeyValuePair<string, BBObjectParameter> variables in this.variablesMap)
    {
      if (!variables.Value.isNone)
      {
        Variable variableById = this.subTree.blackboard.GetVariableByID(variables.Key);
        if (variableById != null)
          variableById.value = variables.Value.value;
      }
    }
  }

  public void OnSubDialogueFinish(bool success)
  {
    this.status = success ? NodeCanvas.Status.Success : NodeCanvas.Status.Failure;
    this.DLGTree.Continue(success ? 0 : 1);
  }

  public override void OnGraphStoped()
  {
    if (!this.IsInstance(this.subTree))
      return;
    this.subTree.Stop();
  }

  public override void OnGraphPaused()
  {
    if (!this.IsInstance(this.subTree))
      return;
    this.subTree.Pause();
  }

  public bool IsInstance(DialogueTree dt) => this.instances.Values.Contains<DialogueTree>(dt);

  public void CheckInstance()
  {
    if (this.IsInstance(this.subTree))
      return;
    DialogueTree dialogueTree = (DialogueTree) null;
    if (!this.instances.TryGetValue(this.subTree, out dialogueTree))
    {
      dialogueTree = Graph.Clone<DialogueTree>(this.subTree);
      this.instances[this.subTree] = dialogueTree;
    }
    dialogueTree.agent = this.graphAgent;
    dialogueTree.blackboard = this.graphBlackboard;
    this.subTree = dialogueTree;
  }
}
