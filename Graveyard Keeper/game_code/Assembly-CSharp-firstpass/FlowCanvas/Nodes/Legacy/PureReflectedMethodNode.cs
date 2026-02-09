// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Legacy.PureReflectedMethodNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;
using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace FlowCanvas.Nodes.Legacy;

public sealed class PureReflectedMethodNode : ReflectedMethodNode
{
  public MethodInfo method;
  public ValueInput instanceInput;
  public List<ValueInput> inputs;
  public List<ValueInput> paramsInputs;
  public Type paramsArrayType;
  public object[] args;
  public object instance;
  public object returnValue;

  public override void RegisterPorts(
    FlowNode node,
    MethodInfo method,
    ReflectedMethodRegistrationOptions options)
  {
    this.method = method;
    ParameterInfo[] parameters = method.GetParameters();
    if (options.callable)
    {
      FlowOutput o = node.AddFlowOutput(" ");
      node.AddFlowInput(" ", (FlowHandler) (f =>
      {
        this.CallMethod();
        o.Call(f);
      }));
    }
    if (!method.IsStatic)
    {
      this.instanceInput = node.AddValueInput(method.DeclaringType.FriendlyName(), method.DeclaringType);
      if (options.callable)
        node.AddValueOutput(method.DeclaringType.FriendlyName(), method.DeclaringType, (ValueHandlerObject) (() => this.instance));
    }
    if (Type.op_Inequality(method.ReturnType, typeof (void)))
      node.AddValueOutput("Value", method.ReturnType, (ValueHandlerObject) (() => !options.callable ? this.CallMethod() : this.returnValue));
    this.inputs = new List<ValueInput>();
    for (int index1 = 0; index1 < parameters.Length; ++index1)
    {
      int i = index1;
      ParameterInfo parameter = parameters[i];
      string name = parameter.Name;
      if (this.instanceInput != null && name == this.instanceInput.name)
        name += " ";
      if (parameter.IsOut || parameter.ParameterType.IsByRef)
      {
        node.AddValueOutput(name, parameter.ParameterType.GetElementType(), (ValueHandlerObject) (() =>
        {
          if (options.callable)
            return this.args[i];
          this.CallMethod();
          return this.args[i];
        }));
        this.inputs.Add((ValueInput) new ValueInput<object>((FlowNode) null, (string) null, (string) null));
      }
      else if (options.exposeParams && parameter.IsParams(parameters))
      {
        this.paramsInputs = new List<ValueInput>();
        this.paramsArrayType = parameter.ParameterType;
        for (int index2 = 0; index2 < options.exposedParamsCount; ++index2)
          this.paramsInputs.Add(node.AddValueInput($"{name} #{index2.ToString()}", parameter.ParameterType.GetEnumerableElementType(), name + index2.ToString()));
      }
      else
      {
        ValueInput valueInput = node.AddValueInput(name, parameter.ParameterType);
        if (parameter.IsOptional && valueInput != null)
          valueInput.serializedValue = parameter.DefaultValue;
        this.inputs.Add(valueInput);
      }
    }
  }

  public object CallMethod()
  {
    if (this.args == null)
      this.args = new object[this.inputs.Count + (this.paramsInputs != null ? 1 : 0)];
    for (int index = 0; index < this.inputs.Count; ++index)
      this.args[index] = this.inputs[index].value;
    if (this.paramsInputs != null)
    {
      Array instance = Array.CreateInstance(this.paramsArrayType.GetElementType(), this.paramsInputs.Count);
      for (int index = 0; index < this.paramsInputs.Count; ++index)
        instance.SetValue(this.paramsInputs[index].value, index);
      this.args[this.args.Length - 1] = (object) instance;
    }
    if (this.method.IsStatic)
      return this.returnValue = this.method.Invoke((object) null, this.args);
    this.instance = this.instanceInput.value;
    return this.instance == null || this.instance.Equals((object) null) ? (this.returnValue = (object) null) : (this.returnValue = this.method.Invoke(this.instance, this.args));
  }
}
