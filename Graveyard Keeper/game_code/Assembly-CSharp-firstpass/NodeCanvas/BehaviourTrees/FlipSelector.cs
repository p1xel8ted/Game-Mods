// Decompiled with JetBrains decompiler
// Type: NodeCanvas.BehaviourTrees.FlipSelector
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.BehaviourTrees;

[Category("Composites")]
[Description("Works like a normal Selector, but when a child node returns Success, that child will be moved to the end.\nAs a result, previously Failed children will always be checked first and recently Successful children last")]
[Color("b3ff7f")]
[ParadoxNotion.Design.Icon("FlipSelector", false, "")]
public class FlipSelector : BTComposite
{
  public int current;

  public override string name => base.name.ToUpper();

  public override NodeCanvas.Status OnExecute(Component agent, IBlackboard blackboard)
  {
    for (int current = this.current; current < this.outConnections.Count; ++current)
    {
      this.status = this.outConnections[current].Execute(agent, blackboard);
      if (this.status == NodeCanvas.Status.Running)
      {
        this.current = current;
        return NodeCanvas.Status.Running;
      }
      if (this.status == NodeCanvas.Status.Success)
      {
        this.SendToBack(current);
        return NodeCanvas.Status.Success;
      }
    }
    return NodeCanvas.Status.Failure;
  }

  public void SendToBack(int i)
  {
    Connection outConnection = this.outConnections[i];
    this.outConnections.RemoveAt(i);
    this.outConnections.Add(outConnection);
  }

  public override void OnReset() => this.current = 0;
}
