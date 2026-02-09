// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.SwitchEnum
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;
using ParadoxNotion.Design;
using ParadoxNotion.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[FlowNode.ContextDefinedInputs(new System.Type[] {typeof (Enum)})]
[Description("Branch the Flow based on an enum value.\nPlease connect an Enum first for the options to show.")]
[Category("Flow Controllers/Switchers")]
public class SwitchEnum : FlowControlNode
{
  [SerializeField]
  public SerializedTypeInfo _type;

  public System.Type type
  {
    get => this._type == null ? (System.Type) null : this._type.Get();
    set
    {
      if (this._type != null && !System.Type.op_Inequality(this._type.Get(), value))
        return;
      this._type = new SerializedTypeInfo(value);
    }
  }

  public override void RegisterPorts()
  {
    if (System.Type.op_Equality(this.type, (System.Type) null))
      this.type = typeof (Enum);
    ValueInput e = this.AddValueInput(this.type.Name, this.type, "Enum");
    if (!System.Type.op_Inequality(this.type, typeof (Enum)))
      return;
    List<FlowOutput> outs = new List<FlowOutput>();
    foreach (string name in Enum.GetNames(this.type))
      outs.Add(this.AddFlowOutput(name));
    this.AddFlowInput("In", (FlowHandler) (f => outs[(int) Enum.Parse(e.value.GetType(), e.value.ToString())].Call(f)));
  }

  public override System.Type GetNodeWildDefinitionType() => typeof (Enum);

  public override void OnPortConnected(Port port, Port otherPort)
  {
    if (!System.Type.op_Equality(this.type, typeof (Enum)) || !typeof (Enum).RTIsAssignableFrom(otherPort.type))
      return;
    this.type = otherPort.type;
    this.GatherPorts();
  }
}
