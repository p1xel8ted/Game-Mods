// Decompiled with JetBrains decompiler
// Type: FlowCanvas.ValueOutput
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;
using System;

#nullable disable
namespace FlowCanvas;

public abstract class ValueOutput : Port
{
  public ValueOutput()
  {
  }

  public ValueOutput(FlowNode parent, string name, string ID)
    : base(parent, name, ID)
  {
  }

  public static ValueOutput<T> CreateInstance<T>(
    FlowNode parent,
    string name,
    string ID,
    ValueHandler<T> getter)
  {
    return new ValueOutput<T>(parent, name, ID, getter);
  }

  public static ValueOutput CreateInstance(
    Type t,
    FlowNode parent,
    string name,
    string ID,
    ValueHandlerObject getter)
  {
    return (ValueOutput) Activator.CreateInstance(typeof (ValueOutput<>).RTMakeGenericType(t), (object) parent, (object) name, (object) ID, (object) getter);
  }

  public abstract object GetValue();
}
