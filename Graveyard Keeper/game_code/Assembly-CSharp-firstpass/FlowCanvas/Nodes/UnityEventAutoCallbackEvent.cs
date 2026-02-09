// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.UnityEventAutoCallbackEvent
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

[Description("Automatically Subscribes to the target UnityEvent when the graph is enabled, and is called when the event is raised")]
[Name("Unity Event", 0)]
[DoNotList]
public class UnityEventAutoCallbackEvent : EventNode
{
  [SerializeField]
  public SerializedFieldInfo _field;
  public ReflectedUnityEvent reflectedEvent;
  public UnityEventBase unityEvent;
  public ValueInput instancePort;
  public FlowOutput callback;
  public object[] args;

  public FieldInfo field => this._field == null ? (FieldInfo) null : this._field.Get();

  public override string name
  {
    get
    {
      return FieldInfo.op_Inequality(this.field, (FieldInfo) null) && this.field.IsStatic ? $"{base.name} ({this.field.RTReflectedType().FriendlyName()})" : base.name;
    }
  }

  public void SetEvent(FieldInfo field, object instance = null)
  {
    this._field = new SerializedFieldInfo(field);
    this.GatherPorts();
  }

  public override void RegisterPorts()
  {
    if (FieldInfo.op_Equality(this.field, (FieldInfo) null))
      return;
    if (this.reflectedEvent == null)
      this.reflectedEvent = new ReflectedUnityEvent(this.field.FieldType);
    if (!this.field.IsStatic)
      this.instancePort = this.AddValueInput(this.field.RTReflectedType().FriendlyName(), this.field.RTReflectedType(), "Instance");
    this.args = new object[this.reflectedEvent.parameters.Length];
    for (int index = 0; index < this.reflectedEvent.parameters.Length; ++index)
    {
      int i = index;
      ParameterInfo parameter = this.reflectedEvent.parameters[i];
      this.AddValueOutput(parameter.Name, "arg" + i.ToString(), parameter.ParameterType, (ValueHandlerObject) (() => this.args[i]));
    }
    this.callback = this.AddFlowOutput(this.field.Name, "Event");
  }

  public override void OnGraphStarted()
  {
    if (FieldInfo.op_Equality(this.field, (FieldInfo) null))
      return;
    object obj = (object) null;
    if (!this.field.IsStatic)
    {
      obj = this.instancePort.value;
      if (obj == null)
      {
        int num = (int) this.Fail("Target is null");
        return;
      }
    }
    this.unityEvent = (UnityEventBase) this.field.GetValue(obj);
    if (this.unityEvent == null)
      return;
    this.reflectedEvent.StartListening(this.unityEvent, new ReflectedUnityEvent.UnityEventCallback(this.OnEventRaised));
  }

  public override void OnGraphStoped()
  {
    if (this.unityEvent == null)
      return;
    this.reflectedEvent.StopListening(this.unityEvent, new ReflectedUnityEvent.UnityEventCallback(this.OnEventRaised));
  }

  public void OnEventRaised(params object[] args)
  {
    this.args = args;
    this.callback.Call(new Flow());
  }
}
