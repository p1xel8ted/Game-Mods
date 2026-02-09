// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.CollisionEvents
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using ParadoxNotion.Services;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Events/Object")]
[Description("Called when Collision based events happen on target and expose collision information")]
[Name("Collision", 0)]
public class CollisionEvents : MessageEventNode<Collider>
{
  public FlowOutput onEnter;
  public FlowOutput onStay;
  public FlowOutput onExit;
  public Collider receiver;
  public Collision collision;

  public override string[] GetTargetMessageEvents()
  {
    return new string[3]
    {
      "OnCollisionEnter",
      "OnCollisionStay",
      "OnCollisionExit"
    };
  }

  public override void RegisterPorts()
  {
    this.onEnter = this.AddFlowOutput("Enter");
    this.onStay = this.AddFlowOutput("Stay");
    this.onExit = this.AddFlowOutput("Exit");
    this.AddValueOutput<Collider>("Receiver", (ValueHandler<Collider>) (() => this.receiver));
    this.AddValueOutput<GameObject>("Other", (ValueHandler<GameObject>) (() => this.collision.gameObject));
    this.AddValueOutput<ContactPoint>("Contact Point", (ValueHandler<ContactPoint>) (() => this.collision.contacts[0]));
    this.AddValueOutput<Collision>("Collision Info", (ValueHandler<Collision>) (() => this.collision));
  }

  public void OnCollisionEnter(MessageRouter.MessageData<Collision> msg)
  {
    this.receiver = this.ResolveReceiver(msg.receiver);
    this.collision = msg.value;
    this.onEnter.Call(new Flow());
  }

  public void OnCollisionStay(MessageRouter.MessageData<Collision> msg)
  {
    this.receiver = this.ResolveReceiver(msg.receiver);
    this.collision = msg.value;
    this.onStay.Call(new Flow());
  }

  public void OnCollisionExit(MessageRouter.MessageData<Collision> msg)
  {
    this.receiver = this.ResolveReceiver(msg.receiver);
    this.collision = msg.value;
    this.onExit.Call(new Flow());
  }

  [CompilerGenerated]
  public Collider \u003CRegisterPorts\u003Eb__6_0() => this.receiver;

  [CompilerGenerated]
  public GameObject \u003CRegisterPorts\u003Eb__6_1() => this.collision.gameObject;

  [CompilerGenerated]
  public ContactPoint \u003CRegisterPorts\u003Eb__6_2() => this.collision.contacts[0];

  [CompilerGenerated]
  public Collision \u003CRegisterPorts\u003Eb__6_3() => this.collision;
}
