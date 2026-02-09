// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.TriggerEvents
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using ParadoxNotion.Services;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Called when Trigger based event happen on target")]
[Name("Trigger", 0)]
[Category("Events/Object")]
public class TriggerEvents : MessageEventNode<Collider>
{
  public FlowOutput onEnter;
  public FlowOutput onStay;
  public FlowOutput onExit;
  public Collider receiver;
  public GameObject other;

  public override string[] GetTargetMessageEvents()
  {
    return new string[3]
    {
      "OnTriggerEnter",
      "OnTriggerStay",
      "OnTriggerExit"
    };
  }

  public override void RegisterPorts()
  {
    this.onEnter = this.AddFlowOutput("Enter");
    this.onStay = this.AddFlowOutput("Stay");
    this.onExit = this.AddFlowOutput("Exit");
    this.AddValueOutput<Collider>("Receiver", (ValueHandler<Collider>) (() => this.receiver));
    this.AddValueOutput<GameObject>("Other", (ValueHandler<GameObject>) (() => this.other));
  }

  public void OnTriggerEnter(MessageRouter.MessageData<Collider> msg)
  {
    this.receiver = this.ResolveReceiver(msg.receiver);
    this.other = msg.value.gameObject;
    this.onEnter.Call(new Flow());
  }

  public void OnTriggerStay(MessageRouter.MessageData<Collider> msg)
  {
    this.receiver = this.ResolveReceiver(msg.receiver);
    this.other = msg.value.gameObject;
    this.onStay.Call(new Flow());
  }

  public void OnTriggerExit(MessageRouter.MessageData<Collider> msg)
  {
    this.receiver = this.ResolveReceiver(msg.receiver);
    this.other = msg.value.gameObject;
    this.onExit.Call(new Flow());
  }

  [CompilerGenerated]
  public Collider \u003CRegisterPorts\u003Eb__6_0() => this.receiver;

  [CompilerGenerated]
  public GameObject \u003CRegisterPorts\u003Eb__6_1() => this.other;
}
