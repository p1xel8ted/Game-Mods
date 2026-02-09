// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.CustomEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using ParadoxNotion.Services;
using System.Runtime.CompilerServices;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Custom Event", 100)]
[Description("Called when a custom event is received on target.\n- To send an event from a graph use the SendEvent node.\n- To send an event from code use:'FlowScriptController.SendEvent(string)'")]
[Category("Events/Custom")]
[Color("ffffe6")]
public class CustomEvent : MessageEventNode<GraphOwner>
{
  [RequiredField]
  public BBParameter<string> eventName = (BBParameter<string>) "EventName";
  public FlowOutput onReceived;
  public GraphOwner receiver;

  public override string name => base.name + $" [ <color=#1a1a00>{this.eventName}</color> ]";

  public override string[] GetTargetMessageEvents()
  {
    return new string[1]{ "OnCustomEvent" };
  }

  public override void RegisterPorts()
  {
    this.onReceived = this.AddFlowOutput("Received");
    this.AddValueOutput<GraphOwner>("Receiver", (ValueHandler<GraphOwner>) (() => this.receiver));
  }

  public void OnCustomEvent(MessageRouter.MessageData<EventData> msg)
  {
    if (!(msg.value.name == this.eventName.value))
      return;
    this.receiver = this.ResolveReceiver(msg.receiver);
    this.onReceived.Call(new Flow());
  }

  [CompilerGenerated]
  public GraphOwner \u003CRegisterPorts\u003Eb__6_0() => this.receiver;
}
