// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.MouseAgentEvents
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
[Description("Called when mouse based operations happen on target collider")]
[Name("Mouse", 0)]
public class MouseAgentEvents : MessageEventNode<Collider>
{
  public FlowOutput onEnter;
  public FlowOutput onOver;
  public FlowOutput onExit;
  public FlowOutput onDown;
  public FlowOutput onUp;
  public FlowOutput onDrag;
  public Collider receiver;
  public RaycastHit hit;

  public override string[] GetTargetMessageEvents()
  {
    return new string[6]
    {
      "OnMouseEnter",
      "OnMouseOver",
      "OnMouseExit",
      "OnMouseDown",
      "OnMouseUp",
      "OnMouseDrag"
    };
  }

  public override void RegisterPorts()
  {
    this.onDown = this.AddFlowOutput("Down");
    this.onUp = this.AddFlowOutput("Up");
    this.onEnter = this.AddFlowOutput("Enter");
    this.onOver = this.AddFlowOutput("Over");
    this.onExit = this.AddFlowOutput("Exit");
    this.onDrag = this.AddFlowOutput("Drag");
    this.AddValueOutput<Collider>("Receiver", (ValueHandler<Collider>) (() => this.receiver));
    this.AddValueOutput<RaycastHit>("Info", (ValueHandler<RaycastHit>) (() => this.hit));
  }

  public void OnMouseEnter(MessageRouter.MessageData msg)
  {
    this.receiver = this.ResolveReceiver(msg.receiver);
    this.StoreHit();
    this.onEnter.Call(new Flow());
  }

  public void OnMouseOver(MessageRouter.MessageData msg)
  {
    this.receiver = this.ResolveReceiver(msg.receiver);
    this.StoreHit();
    this.onOver.Call(new Flow());
  }

  public void OnMouseExit(MessageRouter.MessageData msg)
  {
    this.receiver = this.ResolveReceiver(msg.receiver);
    this.StoreHit();
    this.onExit.Call(new Flow());
  }

  public void OnMouseDown(MessageRouter.MessageData msg)
  {
    this.receiver = this.ResolveReceiver(msg.receiver);
    this.StoreHit();
    this.onDown.Call(new Flow());
  }

  public void OnMouseUp(MessageRouter.MessageData msg)
  {
    this.receiver = this.ResolveReceiver(msg.receiver);
    this.StoreHit();
    this.onUp.Call(new Flow());
  }

  public void OnMouseDrag(MessageRouter.MessageData msg)
  {
    this.receiver = this.ResolveReceiver(msg.receiver);
    this.StoreHit();
    this.onDrag.Call(new Flow());
  }

  public void StoreHit()
  {
    Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out this.hit, float.PositiveInfinity);
  }

  [CompilerGenerated]
  public Collider \u003CRegisterPorts\u003Eb__9_0() => this.receiver;

  [CompilerGenerated]
  public RaycastHit \u003CRegisterPorts\u003Eb__9_1() => this.hit;
}
