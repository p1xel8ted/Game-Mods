// Decompiled with JetBrains decompiler
// Type: NodeCanvas.BehaviourTrees.PrioritySelector
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace NodeCanvas.BehaviourTrees;

[Color("b3ff7f")]
[ParadoxNotion.Design.Icon("Priority", false, "")]
[Description("Used for Utility AI, the Priority Selector executes the child with the highest priority value. If it fails, the Prioerity Selector will continue with the next highest priority child until one Succeeds, or until all Fail (similar to how a normal Selector does).")]
[Category("Composites")]
public class PrioritySelector : BTComposite
{
  public List<BBParameter<float>> priorities = new List<BBParameter<float>>();
  public List<Connection> orderedConnections = new List<Connection>();
  public int current;

  public override string name => base.name.ToUpper();

  public override void OnChildConnected(int index)
  {
    List<BBParameter<float>> priorities = this.priorities;
    int index1 = index;
    BBParameter<float> bbParameter = new BBParameter<float>();
    bbParameter.value = 1f;
    bbParameter.bb = this.graphBlackboard;
    priorities.Insert(index1, bbParameter);
  }

  public override void OnChildDisconnected(int index) => this.priorities.RemoveAt(index);

  public override NodeCanvas.Status OnExecute(Component agent, IBlackboard blackboard)
  {
    if (this.status == NodeCanvas.Status.Resting)
      this.orderedConnections = this.outConnections.OrderBy<Connection, float>((Func<Connection, float>) (c => this.priorities[this.outConnections.IndexOf(c)].value)).Reverse<Connection>().ToList<Connection>();
    for (int current = this.current; current < this.orderedConnections.Count; ++current)
    {
      this.status = this.orderedConnections[current].Execute(agent, blackboard);
      if (this.status == NodeCanvas.Status.Success)
        return NodeCanvas.Status.Success;
      if (this.status == NodeCanvas.Status.Running)
      {
        this.current = current;
        return NodeCanvas.Status.Running;
      }
    }
    return NodeCanvas.Status.Failure;
  }

  public override void OnReset() => this.current = 0;

  [CompilerGenerated]
  public float \u003COnExecute\u003Eb__7_0(Connection c)
  {
    return this.priorities[this.outConnections.IndexOf(c)].value;
  }
}
