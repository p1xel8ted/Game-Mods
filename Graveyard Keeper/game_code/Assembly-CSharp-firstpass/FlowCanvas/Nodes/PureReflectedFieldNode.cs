// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.PureReflectedFieldNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;
using System.Reflection;
using System.Runtime.CompilerServices;

#nullable disable
namespace FlowCanvas.Nodes;

public class PureReflectedFieldNode : BaseReflectedFieldNode
{
  public ValueInput instanceInput;
  public ValueInput valueInput;
  public object instanceObject;
  public object valueObject;

  public override bool InitInternal(FieldInfo method)
  {
    this.instanceInput = (ValueInput) null;
    this.instanceObject = (object) null;
    this.valueObject = (object) null;
    return true;
  }

  public void SetValue()
  {
    this.valueObject = this.valueInput != null ? this.valueInput.value : (object) null;
    this.instanceObject = this.instanceInput != null ? this.instanceInput.value : (object) null;
    this.fieldInfo.SetValue(this.instanceObject, this.valueObject);
  }

  public void GetValue()
  {
    this.instanceObject = this.instanceInput != null ? this.instanceInput.value : (object) null;
    this.valueObject = this.fieldInfo.GetValue(this.instanceObject);
  }

  public override void RegisterPorts(FlowNode node, ReflectedFieldNodeWrapper.AccessMode accessMode)
  {
    if (FieldInfo.op_Equality(this.fieldInfo, (FieldInfo) null))
      return;
    if (accessMode == ReflectedFieldNodeWrapper.AccessMode.SetField && !this.fieldInfo.IsReadOnly())
    {
      FlowOutput output = node.AddFlowOutput(" ");
      node.AddFlowInput(" ", (FlowHandler) (flow =>
      {
        this.SetValue();
        output.Call(flow);
      }));
    }
    if (this.instanceDef.paramMode != ParamMode.Undefined)
    {
      this.instanceInput = node.AddValueInput(this.instanceDef.portName, this.instanceDef.paramType, this.instanceDef.portId);
      if (accessMode == ReflectedFieldNodeWrapper.AccessMode.SetField && !this.fieldInfo.IsReadOnly())
        node.AddValueOutput(this.instanceDef.portName, this.instanceDef.paramType, (ValueHandlerObject) (() => this.instanceObject), this.instanceDef.portId);
    }
    else
    {
      this.instanceInput = (ValueInput) null;
      this.instanceObject = (object) null;
    }
    if (accessMode == ReflectedFieldNodeWrapper.AccessMode.SetField && !this.fieldInfo.IsReadOnly())
      this.valueInput = node.AddValueInput(this.resultDef.portName, this.resultDef.paramType, this.resultDef.portId);
    else
      node.AddValueOutput(this.resultDef.portName, this.resultDef.portId, this.resultDef.paramType, (ValueHandlerObject) (() =>
      {
        this.GetValue();
        return this.valueObject;
      }));
  }

  [CompilerGenerated]
  public object \u003CRegisterPorts\u003Eb__7_0() => this.instanceObject;

  [CompilerGenerated]
  public object \u003CRegisterPorts\u003Eb__7_1()
  {
    this.GetValue();
    return this.valueObject;
  }
}
