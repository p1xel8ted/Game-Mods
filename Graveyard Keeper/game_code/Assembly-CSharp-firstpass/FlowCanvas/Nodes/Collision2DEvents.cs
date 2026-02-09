// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Collision2DEvents
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using ParadoxNotion.Services;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Called when 2D Collision based events happen on target and expose collision information")]
[Category("Events/Object")]
[Name("Collision2D", 0)]
public class Collision2DEvents : MessageEventNode<Collider2D>
{
  public FlowOutput onEnter;
  public FlowOutput onStay;
  public FlowOutput onExit;
  public Collider2D receiver;
  public Collision2D collision;

  public override string[] GetTargetMessageEvents()
  {
    return new string[3]
    {
      "OnCollisionEnter2D",
      "OnCollisionStay2D",
      "OnCollisionExit2D"
    };
  }

  public override void RegisterPorts()
  {
    this.onEnter = this.AddFlowOutput("Enter");
    this.onStay = this.AddFlowOutput("Stay");
    this.onExit = this.AddFlowOutput("Exit");
    this.AddValueOutput<Collider2D>("Receiver", (ValueHandler<Collider2D>) (() => this.receiver));
    this.AddValueOutput<GameObject>("Other", (ValueHandler<GameObject>) (() => this.collision.gameObject));
    this.AddValueOutput<ContactPoint2D>("Contact Point", (ValueHandler<ContactPoint2D>) (() => this.collision.contacts[0]));
    this.AddValueOutput<Collision2D>("Collision Info", (ValueHandler<Collision2D>) (() => this.collision));
  }

  public void OnCollisionEnter2D(MessageRouter.MessageData<Collision2D> msg)
  {
    this.receiver = this.ResolveReceiver(msg.receiver);
    this.collision = msg.value;
    this.onEnter.Call(new Flow());
  }

  public void OnCollisionStay2D(MessageRouter.MessageData<Collision2D> msg)
  {
    this.receiver = this.ResolveReceiver(msg.receiver);
    this.collision = msg.value;
    this.onStay.Call(new Flow());
  }

  public void OnCollisionExit2D(MessageRouter.MessageData<Collision2D> msg)
  {
    this.receiver = this.ResolveReceiver(msg.receiver);
    this.collision = msg.value;
    this.onExit.Call(new Flow());
  }

  [CompilerGenerated]
  public Collider2D \u003CRegisterPorts\u003Eb__6_0() => this.receiver;

  [CompilerGenerated]
  public GameObject \u003CRegisterPorts\u003Eb__6_1() => this.collision.gameObject;

  [CompilerGenerated]
  public ContactPoint2D \u003CRegisterPorts\u003Eb__6_2() => this.collision.contacts[0];

  [CompilerGenerated]
  public Collision2D \u003CRegisterPorts\u003Eb__6_3() => this.collision;
}
