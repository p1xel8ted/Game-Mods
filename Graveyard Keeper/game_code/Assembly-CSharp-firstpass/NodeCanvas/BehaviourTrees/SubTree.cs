// Decompiled with JetBrains decompiler
// Type: NodeCanvas.BehaviourTrees.SubTree
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace NodeCanvas.BehaviourTrees;

[ParadoxNotion.Design.Icon("BT", false, "")]
[Category("Nested")]
[Name("SubTree", 0)]
[Description("SubTree Node can be assigned an entire Sub BehaviorTree. The root node of that behaviour will be considered child node of this node and will return whatever it returns.\nThe target SubTree can also be set by using a Blackboard variable as normal.")]
public class SubTree : BTNode, IGraphAssignable
{
  [SerializeField]
  public BBParameter<BehaviourTree> _subTree;
  public Dictionary<BehaviourTree, BehaviourTree> instances = new Dictionary<BehaviourTree, BehaviourTree>();
  public BehaviourTree currentInstance;

  public override string name => base.name.ToUpper();

  public BehaviourTree subTree
  {
    get => this._subTree.value;
    set => this._subTree.value = value;
  }

  Graph IGraphAssignable.nestedGraph
  {
    get => (Graph) this.subTree;
    set => this.subTree = (BehaviourTree) value;
  }

  Graph[] IGraphAssignable.GetInstances()
  {
    return (Graph[]) this.instances.Values.ToArray<BehaviourTree>();
  }

  public override NodeCanvas.Status OnExecute(Component agent, IBlackboard blackboard)
  {
    if ((Object) this.subTree == (Object) null || this.subTree.primeNode == null)
      return NodeCanvas.Status.Failure;
    if (this.status == NodeCanvas.Status.Resting)
      this.currentInstance = this.CheckInstance();
    return this.currentInstance.Tick(agent, blackboard);
  }

  public override void OnReset()
  {
    if (!((Object) this.currentInstance != (Object) null) || this.currentInstance.primeNode == null)
      return;
    this.currentInstance.primeNode.Reset();
  }

  public override void OnGraphStoped()
  {
    if (!((Object) this.currentInstance != (Object) null))
      return;
    for (int index = 0; index < this.currentInstance.allNodes.Count; ++index)
      this.currentInstance.allNodes[index].OnGraphStoped();
  }

  public override void OnGraphPaused()
  {
    if (!((Object) this.currentInstance != (Object) null))
      return;
    for (int index = 0; index < this.currentInstance.allNodes.Count; ++index)
      this.currentInstance.allNodes[index].OnGraphPaused();
  }

  public BehaviourTree CheckInstance()
  {
    if ((Object) this.subTree == (Object) this.currentInstance)
      return this.currentInstance;
    BehaviourTree behaviourTree = (BehaviourTree) null;
    if (!this.instances.TryGetValue(this.subTree, out behaviourTree))
    {
      behaviourTree = Graph.Clone<BehaviourTree>(this.subTree);
      this.instances[this.subTree] = behaviourTree;
      for (int index = 0; index < behaviourTree.allNodes.Count; ++index)
        behaviourTree.allNodes[index].OnGraphStarted();
    }
    behaviourTree.agent = this.graphAgent;
    behaviourTree.blackboard = this.graphBlackboard;
    behaviourTree.UpdateReferences();
    this.subTree = behaviourTree;
    return behaviourTree;
  }
}
