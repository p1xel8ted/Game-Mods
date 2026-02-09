// Decompiled with JetBrains decompiler
// Type: FlowCanvas.ValueInput
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;
using System;

#nullable disable
namespace FlowCanvas;

public abstract class ValueInput : Port
{
  public ValueInput()
  {
  }

  public ValueInput(FlowNode parent, string name, string ID)
    : base(parent, name, ID)
  {
  }

  public static ValueInput<T> CreateInstance<T>(FlowNode parent, string name, string ID)
  {
    return new ValueInput<T>(parent, name, ID);
  }

  public static ValueInput CreateInstance(Type t, FlowNode parent, string name, string ID)
  {
    return (ValueInput) Activator.CreateInstance(typeof (ValueInput<>).RTMakeGenericType(t), (object) parent, (object) name, (object) ID);
  }

  public object value => this.GetValue();

  public abstract void BindTo(ValueOutput target);

  public abstract void UnBind();

  public abstract object GetValue();

  public abstract object serializedValue { get; set; }

  public abstract bool isDefaultValue { get; }

  public abstract override Type type { get; }
}
