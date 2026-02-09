// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.CustomFunctionEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using ParadoxNotion;
using ParadoxNotion.Design;
using ParadoxNotion.Serialization;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[DeserializeFrom(new string[] {"FlowCanvas.Nodes.RelayFlowOutput"})]
[Category("Functions/Custom")]
[Description("A custom function, defined by any number of parameters and an optional return value. It can be called using the 'Call Custom Function' node. To return a value, the 'Return' node should be used.")]
[Name("New Custom Function", 10)]
public class CustomFunctionEvent : EventNode, IEditorMenuCallbackReceiver
{
  [Tooltip("The identifier name of the function")]
  public string identifier = "MyFunction";
  [SerializeField]
  public List<DynamicPortDefinition> _parameters = new List<DynamicPortDefinition>();
  [SerializeField]
  public DynamicPortDefinition _returns = new DynamicPortDefinition("Value", (System.Type) null);
  public object[] args;
  public object returnValue;
  public FlowOutput onInvoke;

  public List<DynamicPortDefinition> parameters
  {
    get => this._parameters;
    set => this._parameters = value;
  }

  public DynamicPortDefinition returns
  {
    get => this._returns;
    set => this._returns = value;
  }

  public System.Type returnType => this.returns.type;

  public System.Type[] parameterTypes
  {
    get
    {
      return this.parameters.Select<DynamicPortDefinition, System.Type>((Func<DynamicPortDefinition, System.Type>) (p => p.type)).ToArray<System.Type>();
    }
  }

  public override string name => "➥ " + this.identifier;

  public override void RegisterPorts()
  {
    this.onInvoke = this.AddFlowOutput(" ");
    for (int index = 0; index < this.parameters.Count; ++index)
    {
      int i = index;
      DynamicPortDefinition parameter = this.parameters[i];
      this.AddValueOutput(parameter.name, parameter.ID, parameter.type, (ValueHandlerObject) (() => this.args[i]));
    }
  }

  public object Invoke(Flow f, params object[] args)
  {
    this.args = args;
    f.ReturnType = this.returns.type;
    f.Return = (FlowReturn) (o => this.returnValue = o);
    this.onInvoke.Call(f);
    return this.returnValue;
  }

  public void InvokeAsync(Flow f, FlowHandler Callback, params object[] args)
  {
    this.args = args;
    f.ReturnType = this.returns.type;
    f.Return = (FlowReturn) (o =>
    {
      this.returnValue = o;
      Callback(f);
    });
    this.onInvoke.Call(f);
  }

  public object GetReturnValue() => this.returnValue;

  public void AddParameter(System.Type type)
  {
    this.parameters.Add(new DynamicPortDefinition(type.FriendlyName(), type));
    this.GatherPortsUpdateRefs();
  }

  public void GatherPortsUpdateRefs()
  {
    this.GatherPorts();
    foreach (FlowNode flowNode in this.flowGraph.GetAllNodesOfType<CustomFunctionCall>().Where<CustomFunctionCall>((Func<CustomFunctionCall, bool>) (n => n.sourceFunction == this)))
      flowNode.GatherPorts();
  }

  [CompilerGenerated]
  public void \u003CInvoke\u003Eb__19_0(object o) => this.returnValue = o;

  [CompilerGenerated]
  public bool \u003CGatherPortsUpdateRefs\u003Eb__23_0(CustomFunctionCall n)
  {
    return n.sourceFunction == this;
  }
}
