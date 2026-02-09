// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.UniversalDelegateParam`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;
using System;
using System.Reflection;

#nullable disable
namespace FlowCanvas.Nodes;

public class UniversalDelegateParam<T> : UniversalDelegateParam
{
  public T value;
  public ValueInput<T> valueInput;
  public static FieldInfo _fieldInfo;

  public override Type GetCurrentType() => typeof (T);

  public override void RegisterAsInput(FlowNode node)
  {
    if (this.paramDef.paramMode != ParamMode.Instance && this.paramDef.paramMode != ParamMode.In && this.paramDef.paramMode != ParamMode.Ref && this.paramDef.paramMode != ParamMode.Result)
      return;
    this.valueInput = node.AddValueInput<T>(this.paramDef.portName, this.paramDef.portId);
  }

  public void RegisterAsOutputInternal(FlowNode node, Delegate beforeReturn)
  {
    if (this.paramDef.paramMode != ParamMode.Instance && this.paramDef.paramMode != ParamMode.Out && this.paramDef.paramMode != ParamMode.Ref && this.paramDef.paramMode != ParamMode.Result)
      return;
    ValueHandler<T> getter = (ValueHandler<T>) (() =>
    {
      if (beforeReturn is Action action3)
        action3();
      if (beforeReturn is Action<UniversalDelegateParam> action4)
        action4((UniversalDelegateParam) this);
      return this.value;
    });
    node.AddValueOutput<T>(this.paramDef.portName, getter, this.paramDef.portId);
  }

  public override void RegisterAsOutput(FlowNode node)
  {
    this.RegisterAsOutputInternal(node, (Delegate) null);
  }

  public override void RegisterAsOutput(FlowNode node, Action beforeReturn)
  {
    this.RegisterAsOutputInternal(node, (Delegate) beforeReturn);
  }

  public override void RegisterAsOutput(FlowNode node, Action<UniversalDelegateParam> beforeReturn)
  {
    this.RegisterAsOutputInternal(node, (Delegate) beforeReturn);
  }

  public override void SetFromInput()
  {
    if (this.valueInput == null)
      return;
    this.value = this.valueInput.value;
  }

  public override void SetFromValue(object newValue) => this.value = (T) newValue;

  public override FieldInfo ValueField
  {
    get
    {
      return UniversalDelegateParam<T>._fieldInfo ?? (UniversalDelegateParam<T>._fieldInfo = this.GetType().RTGetField("value"));
    }
  }
}
