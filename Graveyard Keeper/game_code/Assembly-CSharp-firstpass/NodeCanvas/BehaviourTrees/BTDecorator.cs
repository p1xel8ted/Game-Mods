// Decompiled with JetBrains decompiler
// Type: NodeCanvas.BehaviourTrees.BTDecorator
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion;

#nullable disable
namespace NodeCanvas.BehaviourTrees;

public abstract class BTDecorator : BTNode
{
  public sealed override int maxOutConnections => 1;

  public sealed override Alignment2x2 commentsAlignment => Alignment2x2.Right;

  public Connection decoratedConnection
  {
    get
    {
      try
      {
        return this.outConnections[0];
      }
      catch
      {
        return (Connection) null;
      }
    }
  }

  public Node decoratedNode
  {
    get
    {
      try
      {
        return this.outConnections[0].targetNode;
      }
      catch
      {
        return (Node) null;
      }
    }
  }
}
