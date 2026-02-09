// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.DelegateCallbackEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;
using ParadoxNotion.Design;
using ParadoxNotion.Serialization;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Delegate Callback", 1)]
[Description("The exposed Delegate points directly to the 'Callback' output. You can connect this delegate as listener to a Unity or C# Event using the AddListener function of that Unity Event, or the += function of that C# Event. When that event is raised, this node will be called.")]
[FlowNode.ContextDefinedOutputs(new System.Type[] {typeof (Flow), typeof (Delegate)})]
[Category("Events/Custom")]
public class DelegateCallbackEvent : EventNode
{
  [SerializeField]
  public SerializedTypeInfo _type;
  public ReflectedDelegateEvent reflectedEvent;
  public ValueOutput delegatePort;
  public FlowOutput callbackPort;
  public object[] args;

  public System.Type delegateType
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
    this.delegateType = System.Type.op_Inequality(this.delegateType, (System.Type) null) ? this.delegateType : typeof (Delegate);
    this.delegatePort = this.AddValueOutput(this.delegateType.FriendlyName(), "Delegate", this.delegateType, (ValueHandlerObject) (() => (object) this.reflectedEvent.AsDelegate()));
    this.callbackPort = this.AddFlowOutput("Callback");
    if (System.Type.op_Equality(this.delegateType, typeof (Delegate)))
      return;
    if (this.reflectedEvent == null)
    {
      this.reflectedEvent = new ReflectedDelegateEvent(this.delegateType);
      this.reflectedEvent.Add(new ReflectedDelegateEvent.DelegateEventCallback(this.Callback));
    }
    ParameterInfo[] delegateTypeParameters = this.delegateType.RTGetDelegateTypeParameters();
    for (int index = 0; index < delegateTypeParameters.Length; ++index)
    {
      int i = index;
      ParameterInfo parameterInfo = delegateTypeParameters[i];
      this.AddValueOutput(parameterInfo.Name, "arg" + i.ToString(), parameterInfo.ParameterType, (ValueHandlerObject) (() => this.args[i]));
    }
  }

  public void Callback(params object[] args)
  {
    this.args = args;
    this.callbackPort.Call(new Flow());
  }

  public override void OnPortConnected(Port port, Port otherPort)
  {
    if (port != this.delegatePort || !otherPort.type.RTIsSubclassOf(typeof (Delegate)))
      return;
    this.delegateType = otherPort.type;
    this.GatherPorts();
  }

  [CompilerGenerated]
  public object \u003CRegisterPorts\u003Eb__8_0() => (object) this.reflectedEvent.AsDelegate();
}
