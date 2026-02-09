// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.CSharpAutoCallbackEvent
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

[Description("Automatically Subscribes to the target C# Event when the graph is enabled, and is called when the event is raised")]
[Name("C# Event", 0)]
[DoNotList]
public class CSharpAutoCallbackEvent : EventNode
{
  [SerializeField]
  public SerializedEventInfo _event;
  public ReflectedDelegateEvent reflectedEvent;
  public ValueInput instancePort;
  public FlowOutput callback;
  public object instance;
  public object[] args;

  public EventInfo eventInfo => this._event == null ? (EventInfo) null : this._event.Get();

  public bool isStaticEvent
  {
    get => EventInfo.op_Inequality(this.eventInfo, (EventInfo) null) && this.eventInfo.IsStatic();
  }

  public override string name
  {
    get
    {
      return EventInfo.op_Inequality(this.eventInfo, (EventInfo) null) && this.isStaticEvent ? $"{base.name} ({this.eventInfo.RTReflectedType().FriendlyName()})" : base.name;
    }
  }

  public void SetEvent(EventInfo info, object instance = null)
  {
    this._event = new SerializedEventInfo(info);
    this.GatherPorts();
  }

  public override void RegisterPorts()
  {
    if (EventInfo.op_Equality(this.eventInfo, (EventInfo) null))
      return;
    System.Type eventHandlerType = this.eventInfo.EventHandlerType;
    if (this.reflectedEvent == null)
      this.reflectedEvent = new ReflectedDelegateEvent(eventHandlerType);
    if (!this.isStaticEvent)
      this.instancePort = this.AddValueInput(this.eventInfo.RTReflectedType().FriendlyName(), this.eventInfo.RTReflectedType(), "Instance");
    ParameterInfo[] delegateTypeParameters = eventHandlerType.RTGetDelegateTypeParameters();
    this.args = new object[delegateTypeParameters.Length];
    for (int index = 0; index < delegateTypeParameters.Length; ++index)
    {
      int i = index;
      ParameterInfo parameterInfo = delegateTypeParameters[i];
      this.AddValueOutput(parameterInfo.Name, "arg" + i.ToString(), parameterInfo.ParameterType, (ValueHandlerObject) (() => this.args[i]));
    }
    this.callback = this.AddFlowOutput(this.eventInfo.Name, "Event");
  }

  public override void OnGraphStarted()
  {
    if (EventInfo.op_Equality(this.eventInfo, (EventInfo) null))
      return;
    this.instance = (object) null;
    if (!this.isStaticEvent)
    {
      this.instance = this.instancePort.value;
      if (this.instance == null)
      {
        int num = (int) this.Fail("Target is null");
        return;
      }
    }
    this.eventInfo.AddEventHandler(this.instance, this.reflectedEvent.AsDelegate());
    this.reflectedEvent.Add(new ReflectedDelegateEvent.DelegateEventCallback(this.OnEventRaised));
  }

  public override void OnGraphStoped()
  {
    if (!EventInfo.op_Inequality(this.eventInfo, (EventInfo) null))
      return;
    this.eventInfo.RemoveEventHandler(this.instance, this.reflectedEvent.AsDelegate());
    this.reflectedEvent.Remove(new ReflectedDelegateEvent.DelegateEventCallback(this.OnEventRaised));
  }

  public void OnEventRaised(params object[] args)
  {
    this.args = args;
    this.callback.Call(new Flow());
  }
}
