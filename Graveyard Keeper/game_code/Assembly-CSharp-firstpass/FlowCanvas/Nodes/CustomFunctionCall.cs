// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.CustomFunctionCall
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using ParadoxNotion.Design;
using ParadoxNotion.Serialization;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Function Call", 0)]
[DeserializeFrom(new string[] {"FlowCanvas.Nodes.RelayFlowInput"})]
[Color("d86b13")]
[Category("Functions/Custom")]
[Description("Calls an existing Custom Function")]
[DoNotList]
public class CustomFunctionCall : FlowControlNode
{
  [SerializeField]
  public string _sourceOutputUID;
  public ValueInput[] portArgs;
  public object[] objectArgs;
  public FlowOutput fOut;
  public object _sourceFunction;

  public string sourceFunctionUID
  {
    get => this._sourceOutputUID;
    set => this._sourceOutputUID = value;
  }

  public CustomFunctionEvent sourceFunction
  {
    get
    {
      if (this._sourceFunction == null)
      {
        this._sourceFunction = (object) this.graph.GetAllNodesOfType<CustomFunctionEvent>().FirstOrDefault<CustomFunctionEvent>((Func<CustomFunctionEvent, bool>) (i => i.UID == this.sourceFunctionUID));
        if (this._sourceFunction == null)
          this._sourceFunction = new object();
      }
      return this._sourceFunction as CustomFunctionEvent;
    }
    set => this._sourceFunction = (object) value;
  }

  public override string name
  {
    get
    {
      return $"Call {(this.sourceFunction != null ? (object) this.sourceFunction.identifier : (object) "NONE")} ()";
    }
  }

  public override string description
  {
    get
    {
      return this.sourceFunction == null || string.IsNullOrEmpty(this.sourceFunction.nodeComment) ? base.description : this.sourceFunction.nodeComment;
    }
  }

  public void SetFunction(CustomFunctionEvent func)
  {
    this.sourceFunctionUID = func?.UID;
    this.sourceFunction = func != null ? func : (CustomFunctionEvent) null;
    this.GatherPorts();
  }

  public override void RegisterPorts()
  {
    this.AddFlowInput(" ", new FlowHandler(this.Invoke));
    if (this.sourceFunction == null)
      return;
    List<DynamicPortDefinition> parameters = this.sourceFunction.parameters;
    this.portArgs = new ValueInput[parameters.Count];
    for (int index1 = 0; index1 < parameters.Count; ++index1)
    {
      int index2 = index1;
      DynamicPortDefinition dynamicPortDefinition = parameters[index2];
      this.portArgs[index2] = this.AddValueInput(dynamicPortDefinition.name, dynamicPortDefinition.type, dynamicPortDefinition.ID);
    }
    if (System.Type.op_Inequality(this.sourceFunction.returns.type, (System.Type) null))
      this.AddValueOutput(this.sourceFunction.returns.name, this.sourceFunction.returns.ID, this.sourceFunction.returns.type, new ValueHandlerObject(this.sourceFunction.GetReturnValue));
    this.fOut = this.AddFlowOutput(" ");
  }

  public void Invoke(Flow f)
  {
    if (this.sourceFunction == null)
      return;
    if (this.objectArgs == null)
      this.objectArgs = new object[this.portArgs.Length];
    for (int index = 0; index < this.portArgs.Length; ++index)
      this.objectArgs[index] = this.portArgs[index].value;
    this.sourceFunction.InvokeAsync(f, new FlowHandler(this.fOut.Call), this.objectArgs);
  }

  [CompilerGenerated]
  public bool \u003Cget_sourceFunction\u003Eb__9_0(CustomFunctionEvent i)
  {
    return i.UID == this.sourceFunctionUID;
  }
}
