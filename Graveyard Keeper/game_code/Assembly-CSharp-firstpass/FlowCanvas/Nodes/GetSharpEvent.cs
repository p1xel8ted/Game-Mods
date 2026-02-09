// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.GetSharpEvent
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

[Description("Returns a reference of a C# event, which can be used with the C# Event Callback node.")]
[DoNotList]
public class GetSharpEvent : FlowNode
{
  [SerializeField]
  public SerializedEventInfo _event;
  public ValueInput instancePort;

  public EventInfo eventInfo => this._event == null ? (EventInfo) null : this._event.Get();

  public override string name
  {
    get
    {
      if (!EventInfo.op_Inequality(this.eventInfo, (EventInfo) null))
        return base.name;
      return this.eventInfo.IsStatic() ? $"{this.eventInfo.DeclaringType.FriendlyName()}.{this.eventInfo.Name}" : this.eventInfo.Name;
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
    if (!this.eventInfo.IsStatic())
      this.instancePort = this.AddValueInput(this.eventInfo.RTReflectedType().FriendlyName(), this.eventInfo.RTReflectedType(), "Instance");
    SharpEvent wrapper = SharpEvent.Create(this.eventInfo);
    this.AddValueOutput("Value", wrapper.GetType(), (ValueHandlerObject) (() =>
    {
      wrapper.instance = this.instancePort != null ? this.instancePort.value : (object) null;
      return (object) wrapper;
    }));
  }
}
