// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ExtractorNode`7
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;

#nullable disable
namespace FlowCanvas.Nodes;

public abstract class ExtractorNode<TInstance, T1, T2, T3, T4, T5, T6> : ExtractorNode
{
  public T1 a;
  public T2 b;
  public T3 c;
  public T4 d;
  public T5 e;
  public T6 f;

  public abstract void Invoke(
    TInstance instance,
    out T1 a,
    out T2 b,
    out T3 c,
    out T4 d,
    out T5 e,
    out T6 f);

  public sealed override void OnRegisterPorts(FlowNode node)
  {
    ValueInput<TInstance> i = node.AddValueInput<TInstance>(typeof (TInstance).FriendlyName());
    node.AddValueOutput<T1>(this.parameters[1].Name, (ValueHandler<T1>) (() =>
    {
      this.Invoke(i.value, out this.a, out this.b, out this.c, out this.d, out this.e, out this.f);
      return this.a;
    }));
    node.AddValueOutput<T2>(this.parameters[2].Name, (ValueHandler<T2>) (() =>
    {
      this.Invoke(i.value, out this.a, out this.b, out this.c, out this.d, out this.e, out this.f);
      return this.b;
    }));
    node.AddValueOutput<T3>(this.parameters[3].Name, (ValueHandler<T3>) (() =>
    {
      this.Invoke(i.value, out this.a, out this.b, out this.c, out this.d, out this.e, out this.f);
      return this.c;
    }));
    node.AddValueOutput<T4>(this.parameters[4].Name, (ValueHandler<T4>) (() =>
    {
      this.Invoke(i.value, out this.a, out this.b, out this.c, out this.d, out this.e, out this.f);
      return this.d;
    }));
    node.AddValueOutput<T5>(this.parameters[5].Name, (ValueHandler<T5>) (() =>
    {
      this.Invoke(i.value, out this.a, out this.b, out this.c, out this.d, out this.e, out this.f);
      return this.e;
    }));
    node.AddValueOutput<T6>(this.parameters[6].Name, (ValueHandler<T6>) (() =>
    {
      this.Invoke(i.value, out this.a, out this.b, out this.c, out this.d, out this.e, out this.f);
      return this.f;
    }));
  }
}
