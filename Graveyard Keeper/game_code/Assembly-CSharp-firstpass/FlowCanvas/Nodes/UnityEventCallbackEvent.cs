// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.UnityEventCallbackEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;
using ParadoxNotion.Design;
using ParadoxNotion.Serialization;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Register a callback on a UnityEvent.\nWhen that event is raised, this node will get called.")]
[FlowNode.ContextDefinedInputs(new System.Type[] {typeof (UnityEventBase)})]
[Category("Events/Custom")]
[Name("Unity Event Callback", 3)]
public class UnityEventCallbackEvent : EventNode
{
  [SerializeField]
  public bool _autoHandleRegistration;
  [SerializeField]
  public SerializedTypeInfo _type;
  public object[] argValues;
  public ValueInput eventInput;
  public FlowOutput callback;
  public ReflectedUnityEvent reflectedEvent;

  public System.Type eventType
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
    if (!this.autoHandleRegistration || !(this.eventInput.value is UnityEventBase targetEvent))
      return;
    this.reflectedEvent.StartListening(targetEvent, new ReflectedUnityEvent.UnityEventCallback(this.OnEventRaised));
  }

  public override void OnGraphStoped()
  {
    if (!this.autoHandleRegistration || !(this.eventInput.value is UnityEventBase targetEvent))
      return;
    this.reflectedEvent.StopListening(targetEvent, new ReflectedUnityEvent.UnityEventCallback(this.OnEventRaised));
  }

  public override void RegisterPorts()
  {
    this.eventType = System.Type.op_Inequality(this.eventType, (System.Type) null) ? this.eventType : typeof (UnityEventBase);
    this.eventInput = this.AddValueInput("Event", this.eventType);
    if (System.Type.op_Equality(this.eventType, typeof (UnityEventBase)))
      return;
    if (this.reflectedEvent == null)
      this.reflectedEvent = new ReflectedUnityEvent(this.eventType);
    if (System.Type.op_Inequality(this.reflectedEvent.eventType, this.eventType))
      this.reflectedEvent.InitForEventType(this.eventType);
    this.argValues = new object[this.reflectedEvent.parameters.Length];
    for (int index = 0; index < this.reflectedEvent.parameters.Length; ++index)
    {
      int i = index;
      ParameterInfo parameter = this.reflectedEvent.parameters[i];
      this.AddValueOutput(parameter.Name, "arg" + i.ToString(), parameter.ParameterType, (ValueHandlerObject) (() => this.argValues[i]));
    }
    this.callback = this.AddFlowOutput("Callback");
    if (this.autoHandleRegistration)
      return;
    this.AddFlowInput("Register", new FlowHandler(this.Register), "Add");
    this.AddFlowInput("Unregister", new FlowHandler(this.Unregister), "Remove");
  }

  public void Register(Flow f)
  {
    if (!(this.eventInput.value is UnityEventBase targetEvent))
      return;
    this.reflectedEvent.StopListening(targetEvent, new ReflectedUnityEvent.UnityEventCallback(this.OnEventRaised));
    this.reflectedEvent.StartListening(targetEvent, new ReflectedUnityEvent.UnityEventCallback(this.OnEventRaised));
  }

  public void Unregister(Flow f)
  {
    if (!(this.eventInput.value is UnityEventBase targetEvent))
      return;
    this.reflectedEvent.StopListening(targetEvent, new ReflectedUnityEvent.UnityEventCallback(this.OnEventRaised));
  }

  public void OnEventRaised(params object[] args)
  {
    this.argValues = args;
    this.callback.Call(new Flow());
  }

  public override System.Type GetNodeWildDefinitionType() => typeof (UnityEventBase);

  public override void OnPortConnected(Port port, Port otherPort)
  {
    if (port != this.eventInput || !otherPort.type.RTIsSubclassOf(typeof (UnityEventBase)))
      return;
    this.eventType = otherPort.type;
    this.GatherPorts();
  }
}
