// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ExtractorNode`3
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;

#nullable disable
namespace FlowCanvas.Nodes;

public abstract class ExtractorNode<TInstance, T1, T2> : ExtractorNode
{
  public T1 a;
  public T2 b;

  public abstract void Invoke(TInstance instance, out T1 a, out T2 b);

  public sealed override void OnRegisterPorts(FlowNode node)
  {
    ValueInput<TInstance> i = node.AddValueInput<TInstance>(typeof (TInstance).FriendlyName());
    node.AddValueOutput<T1>(this.parameters[1].Name, (ValueHandler<T1>) (() =>
    {
      this.Invoke(i.value, out this.a, out this.b);
      return this.a;
    }));
    node.AddValueOutput<T2>(this.parameters[2].Name, (ValueHandler<T2>) (() =>
    {
      this.Invoke(i.value, out this.a, out this.b);
      return this.b;
    }));
  }
}
