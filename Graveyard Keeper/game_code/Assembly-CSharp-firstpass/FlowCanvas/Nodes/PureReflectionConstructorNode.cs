// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.PureReflectionConstructorNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Reflection;

#nullable disable
namespace FlowCanvas.Nodes;

public class PureReflectionConstructorNode : BaseReflectedConstructorNode
{
  public object resultObject;
  public object[] callParams;
  public ValueInput[] inputs;
  public ValueInput[] arrayInputs;
  public int arrayParamsInput = -1;
  public Type arrayParamsType;

  public override bool InitInternal(ConstructorInfo constructor)
  {
    if (this.options.exposeParams)
    {
      for (int index = 0; index <= this.paramDefinitions.Count - 1; ++index)
      {
        ParamDef paramDefinition = this.paramDefinitions[index];
        if (paramDefinition.isParamsArray)
        {
          this.arrayParamsInput = index;
          this.arrayParamsType = paramDefinition.arrayType;
          break;
        }
      }
      if (this.arrayParamsInput >= 0 && this.options.exposedParamsCount >= 0)
        this.arrayInputs = new ValueInput[this.options.exposedParamsCount];
    }
    this.callParams = new object[this.paramDefinitions.Count];
    this.inputs = new ValueInput[this.paramDefinitions.Count];
    this.resultObject = (object) null;
    return true;
  }

  public void Call()
  {
    for (int index1 = 0; index1 <= this.callParams.Length - 1; ++index1)
    {
      if (this.options.exposeParams && this.arrayParamsInput == index1 && this.arrayInputs != null)
      {
        Array instance = Array.CreateInstance(this.arrayParamsType, this.arrayInputs.Length);
        for (int index2 = 0; index2 <= this.arrayInputs.Length - 1; ++index2)
          instance.SetValue(this.arrayInputs[index2].value, index2);
        this.callParams[index1] = (object) instance;
      }
      else if (this.inputs[index1] != null)
        this.callParams[index1] = this.inputs[index1].value;
    }
    this.resultObject = this.constructorInfo.Invoke(this.callParams);
  }

  public void RegisterOutput(FlowNode node, bool callable, ParamDef def, int idx)
  {
    node.AddValueOutput(def.portName, def.paramType, (ValueHandlerObject) (() =>
    {
      if (!callable)
        this.Call();
      return this.callParams[idx];
    }), def.portId);
  }

  public void RegisterInput(FlowNode node, ParamDef def, int idx)
  {
    if (this.options.exposeParams && this.arrayParamsInput == idx && def.isParamsArray)
    {
      for (int index = 0; index <= this.options.exposedParamsCount - 1; ++index)
        this.arrayInputs[index] = node.AddValueInput($"{def.portName} #{index.ToString()}", def.arrayType, def.portId + index.ToString());
    }
    else
      this.inputs[idx] = node.AddValueInput(def.portName, def.paramType, def.portId);
  }

  public override void RegisterPorts(FlowNode node, ReflectedMethodRegistrationOptions options)
  {
    if (options.callable)
    {
      FlowOutput output = node.AddFlowOutput(" ");
      node.AddFlowInput(" ", (FlowHandler) (flow =>
      {
        this.Call();
        output.Call(flow);
      }));
    }
    if (this.resultDef.paramMode != ParamMode.Undefined)
      node.AddValueOutput(this.resultDef.portName, this.resultDef.portId, this.resultDef.paramType, (ValueHandlerObject) (() =>
      {
        if (!options.callable)
          this.Call();
        return this.resultObject;
      }));
    for (int index = 0; index <= this.paramDefinitions.Count - 1; ++index)
    {
      ParamDef paramDefinition = this.paramDefinitions[index];
      if (paramDefinition.paramMode == ParamMode.Ref)
      {
        this.RegisterInput(node, paramDefinition, index);
        this.RegisterOutput(node, options.callable, paramDefinition, index);
      }
      else if (paramDefinition.paramMode == ParamMode.In)
        this.RegisterInput(node, paramDefinition, index);
      else if (paramDefinition.paramMode == ParamMode.Out)
        this.RegisterOutput(node, options.callable, paramDefinition, index);
    }
  }
}
