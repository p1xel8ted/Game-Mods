// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.CSharpEventCallback
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;
using ParadoxNotion.Design;
using ParadoxNotion.Serialization;
using System.Reflection;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Providing a C# Event, Register a callback to be called when that event is raised.")]
[FlowNode.ContextDefinedInputs(new System.Type[] {typeof (SharpEvent)})]
[Category("Events/Custom")]
[Name("C# Event Callback", 2)]
public class CSharpEventCallback : EventNode
{
  [SerializeField]
  public SerializedTypeInfo _type;
  [SerializeField]
  public bool _autoHandleRegistration;
  public ReflectedDelegateEvent reflectedEvent;
  public FlowOutput flowCallback;
  public ValueInput eventInput;
  public object[] args;

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

  public bool autoHandleRegistration
  {
    get => this._autoHandleRegistration;
    set
    {
      if (this._autoHandleRegistration == value)
        return;
      this._autoHandleRegistration = value;
      this.GatherPorts();
    }
  }

  public override void OnGraphStarted()
  {
    if (!this.autoHandleRegistration || !(this.eventInput.value is SharpEvent sharpEvent))
      return;
    sharpEvent.StartListening(this.reflectedEvent, new ReflectedDelegateEvent.DelegateEventCallback(this.Callback));
  }

  public override void OnGraphStoped()
  {
    if (!this.autoHandleRegistration || !(this.eventInput.value is SharpEvent sharpEvent))
      return;
    sharpEvent.StopListening(this.reflectedEvent, new ReflectedDelegateEvent.DelegateEventCallback(this.Callback));
  }

  public override void RegisterPorts()
  {
    this.type = System.Type.op_Inequality(this.type, (System.Type) null) ? this.type : typeof (SharpEvent);
    this.eventInput = this.AddValueInput("Event", this.type);
    if (System.Type.op_Equality(this.type, typeof (SharpEvent)))
      return;
    System.Type genericArgument = this.type.RTGetGenericArguments()[0];
    if (this.reflectedEvent == null)
      this.reflectedEvent = new ReflectedDelegateEvent(genericArgument);
    ParameterInfo[] delegateTypeParameters = genericArgument.RTGetDelegateTypeParameters();
    for (int index = 0; index < delegateTypeParameters.Length; ++index)
    {
      int i = index;
      ParameterInfo parameterInfo = delegateTypeParameters[i];
      this.AddValueOutput(parameterInfo.Name, "arg" + i.ToString(), parameterInfo.ParameterType, (ValueHandlerObject) (() => this.args[i]));
    }
    this.flowCallback = this.AddFlowOutput("Callback");
    if (this.autoHandleRegistration)
      return;
    this.AddFlowInput("Register", new FlowHandler(this.Register), "Add");
    this.AddFlowInput("Unregister", new FlowHandler(this.Unregister), "Remove");
  }

  public void Register(Flow f)
  {
    if (!(this.eventInput.value is SharpEvent sharpEvent))
      return;
    sharpEvent.StopListening(this.reflectedEvent, new ReflectedDelegateEvent.DelegateEventCallback(this.Callback));
    sharpEvent.StartListening(this.reflectedEvent, new ReflectedDelegateEvent.DelegateEventCallback(this.Callback));
  }

  public void Unregister(Flow f)
  {
    if (!(this.eventInput.value is SharpEvent sharpEvent))
      return;
    sharpEvent.StopListening(this.reflectedEvent, new ReflectedDelegateEvent.DelegateEventCallback(this.Callback));
  }

  public void Callback(params object[] args)
  {
    this.args = args;
    this.flowCallback.Call(new Flow());
  }

  public override System.Type GetNodeWildDefinitionType() => typeof (SharpEvent);

  public override void OnPortConnected(Port port, Port otherPort)
  {
    if (port != this.eventInput)
      return;
    this.type = otherPort.type;
    this.GatherPorts();
  }
}
