// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.PureReflectedExtractorNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace FlowCanvas.Nodes;

public class PureReflectedExtractorNode : BaseReflectedExtractorNode
{
  public static object[] EmptyParams = new object[0];
  public ValueInput instanceInput;

  public override bool InitInternal()
  {
    this.instanceInput = (ValueInput) null;
    return true;
  }

  public ValueHandler<object> GetPortHandler(FieldInfo info)
  {
    return FieldInfo.op_Inequality(info, (FieldInfo) null) ? (ValueHandler<object>) (() => info.GetValue(this.instanceInput != null ? this.instanceInput.value : (object) null)) : (ValueHandler<object>) null;
  }

  public ValueHandler<object> GetPortHandler(MethodInfo info)
  {
    return MethodInfo.op_Inequality(info, (MethodInfo) null) ? (ValueHandler<object>) (() => info.Invoke(this.instanceInput != null ? this.instanceInput.value : (object) null, PureReflectedExtractorNode.EmptyParams)) : (ValueHandler<object>) null;
  }

  public override void RegisterPorts(FlowNode node)
  {
    this.instanceInput = (ValueInput) null;
    ParamDef instanceDef = this.Params.instanceDef;
    if (instanceDef.paramMode != ParamMode.Undefined)
      this.instanceInput = node.AddValueInput(instanceDef.portName, instanceDef.paramType, instanceDef.portId);
    List<ParamDef> paramDefinitions = this.Params.paramDefinitions;
    if (paramDefinitions == null)
      return;
    for (int index = 0; index <= paramDefinitions.Count - 1; ++index)
    {
      ParamDef paramDef = paramDefinitions[index];
      if (paramDef.paramMode == ParamMode.Out)
      {
        ValueHandler<object> getter = (ValueHandler<object>) null;
        FieldInfo presentedInfo1 = paramDef.presentedInfo as FieldInfo;
        if (FieldInfo.op_Inequality(presentedInfo1, (FieldInfo) null))
          getter = this.GetPortHandler(presentedInfo1);
        MethodInfo presentedInfo2 = paramDef.presentedInfo as MethodInfo;
        if (MethodInfo.op_Inequality(presentedInfo2, (MethodInfo) null))
          getter = this.GetPortHandler(presentedInfo2);
        if (getter != null)
          node.AddValueOutput<object>(paramDef.portName, getter, paramDef.portId);
      }
    }
  }
}
