// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.MyPureFunctionNode`1
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[DoNotList]
public abstract class MyPureFunctionNode<TResult> : PureFunctionNodeBase
{
  public FlowNode _node;

  public abstract TResult Invoke();

  public sealed override void OnRegisterPorts(FlowNode node)
  {
    node.AddValueOutput<TResult>("Value", (ValueHandler<TResult>) (() =>
    {
      this._node = node;
      return this.Invoke();
    }));
  }

  public WorldGameObject wgo => MyFlowNode.GetWGOFromNode(this._node);
}
