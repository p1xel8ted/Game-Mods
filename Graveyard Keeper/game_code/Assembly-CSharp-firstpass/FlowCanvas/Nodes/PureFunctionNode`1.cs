// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.PureFunctionNode`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Runtime.CompilerServices;

#nullable disable
namespace FlowCanvas.Nodes;

public abstract class PureFunctionNode<TResult> : PureFunctionNodeBase
{
  public abstract TResult Invoke();

  public sealed override void OnRegisterPorts(FlowNode node)
  {
    node.AddValueOutput<TResult>("Value", (ValueHandler<TResult>) (() => this.Invoke()));
  }

  [CompilerGenerated]
  public TResult \u003COnRegisterPorts\u003Eb__1_0() => this.Invoke();
}
