// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Legacy.PureReflectedFieldNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;
using System;
using System.Reflection;

#nullable disable
namespace FlowCanvas.Nodes.Legacy;

public sealed class PureReflectedFieldNode : ReflectedFieldNode
{
  public override void RegisterPorts(
    FlowNode node,
    FieldInfo field,
    ReflectedFieldNodeWrapper.AccessMode accessMode)
  {
    if (field.IsConstant())
    {
      object constantValue = field.GetValue((object) null);
      node.AddValueOutput("Value", field.FieldType, (ValueHandlerObject) (() => constantValue));
    }
    else
    {
      Type declaringType = field.DeclaringType;
      if (accessMode == ReflectedFieldNodeWrapper.AccessMode.GetField)
      {
        ValueInput instanceInput = node.AddValueInput(declaringType.FriendlyName(), declaringType);
        node.AddValueOutput("Value", field.FieldType, (ValueHandlerObject) (() => field.GetValue(instanceInput.value)));
      }
      else
      {
        object instance = (object) null;
        ValueInput instanceInput = node.AddValueInput(declaringType.FriendlyName(), declaringType);
        ValueInput valueInput = node.AddValueInput("Value", field.FieldType);
        FlowOutput flowOut = node.AddFlowOutput(" ");
        node.AddValueOutput(declaringType.FriendlyName(), declaringType, (ValueHandlerObject) (() => instance));
        node.AddFlowInput(" ", (FlowHandler) (f =>
        {
          instance = instanceInput.value;
          field.SetValue(instance, valueInput.value);
          flowOut.Call(f);
        }));
      }
    }
  }
}
