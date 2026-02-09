// Decompiled with JetBrains decompiler
// Type: FlowCanvas.ValueInput`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.CompilerServices;

#nullable disable
namespace FlowCanvas;

public class ValueInput<T> : ValueInput
{
  [CompilerGenerated]
  public ValueHandler<T> \u003Cgetter\u003Ek__BackingField;
  public T _value;

  public ValueInput()
  {
  }

  public ValueInput(FlowNode parent, string name, string ID)
    : base(parent, name, ID)
  {
  }

  public ValueHandler<T> getter
  {
    get => this.\u003Cgetter\u003Ek__BackingField;
    set => this.\u003Cgetter\u003Ek__BackingField = value;
  }

  public T value => this.getter != null ? this.getter() : this._value;

  public override object GetValue() => (object) this.value;

  public override bool isDefaultValue => object.Equals((object) this._value, (object) default (T));

  public override object serializedValue
  {
    get => (object) this._value;
    set => this._value = (T) value;
  }

  public override Type type => typeof (T);

  public override void BindTo(ValueOutput source)
  {
    if (source is ValueOutput<T>)
      this.getter = (source as ValueOutput<T>).getter;
    else
      this.getter = TypeConverter.GetConverterFuncFromTo<T>(source.type, typeof (T), new ValueHandler<object>(source.GetValue));
  }

  public void BindTo(ValueHandler<T> getter) => this.getter = getter;

  public override void UnBind() => this.getter = (ValueHandler<T>) null;

  public static explicit operator T(ValueInput<T> port) => port.value;
}
