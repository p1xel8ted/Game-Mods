// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ExtractorNode`5
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;

#nullable disable
namespace FlowCanvas.Nodes;

public abstract class ExtractorNode<TInstance, T1, T2, T3, T4> : ExtractorNode
{
  public T1 a;
  public T2 b;
  public T3 c;
  public T4 d;

  public abstract void Invoke(TInstance instance, out T1 a, out T2 b, out T3 c, out T4 d);

  public sealed override void OnRegisterPorts(FlowNode node)
  {
    ValueInput<TInstance> i = node.AddValueInput<TInstance>(typeof (TInstance).FriendlyName());
    node.AddValueOutput<T1>(this.parameters[1].Name, (ValueHandler<T1>) (() =>
    {
      this.Invoke(i.value, out this.a, out this.b, out this.c, out this.d);
      return this.a;
    }));
    node.AddValueOutput<T2>(this.parameters[2].Name, (ValueHandler<T2>) (() =>
    {
      this.Invoke(i.value, out this.a, out this.b, out this.c, out this.d);
      return this.b;
    }));
    node.AddValueOutput<T3>(this.parameters[3].Name, (ValueHandler<T3>) (() =>
    {
      this.Invoke(i.value, out this.a, out this.b, out this.c, out this.d);
      return this.c;
    }));
    node.AddValueOutput<T4>(this.parameters[4].Name, (ValueHandler<T4>) (() =>
    {
      this.Invoke(i.value, out this.a, out this.b, out this.c, out this.d);
      return this.d;
    }));
  }
}
