// Decompiled with JetBrains decompiler
// Type: FlowCanvas.ValueOutput`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.CompilerServices;

#nullable disable
namespace FlowCanvas;

public class ValueOutput<T> : ValueOutput
{
  [CompilerGenerated]
  public ValueHandler<T> \u003Cgetter\u003Ek__BackingField;

  public ValueOutput()
  {
  }

  public ValueOutput(FlowNode parent, string name, string ID, ValueHandler<T> getter)
    : base(parent, name, ID)
  {
    this.getter = getter;
  }

  public ValueOutput(FlowNode parent, string name, string ID, ValueHandlerObject getter)
    : base(parent, name, ID)
  {
    this.getter = (ValueHandler<T>) (() => (T) getter());
  }

  public ValueHandler<T> getter
  {
    get => this.\u003Cgetter\u003Ek__BackingField;
    set => this.\u003Cgetter\u003Ek__BackingField = value;
  }

  public override object GetValue() => (object) this.getter();

  public override Type type => typeof (T);

  public static explicit operator T(ValueOutput<T> port) => port.getter();
}
