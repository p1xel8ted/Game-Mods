// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.MouseAgent2DEvents
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using ParadoxNotion.Services;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Called when mouse based operations happen on target 2D collider")]
[Category("Events/Object")]
[Name("Mouse2D", 0)]
public class MouseAgent2DEvents : MessageEventNode<Collider2D>
{
  public FlowOutput onEnter;
  public FlowOutput onOver;
  public FlowOutput onExit;
  public FlowOutput onDown;
  public FlowOutput onUp;
  public FlowOutput onDrag;
  public Collider2D receiver;
  public RaycastHit2D hit;

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
    this.AddValueOutput<Collider2D>("Receiver", (ValueHandler<Collider2D>) (() => this.receiver));
    this.AddValueOutput<RaycastHit2D>("Info", (ValueHandler<RaycastHit2D>) (() => this.hit));
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
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    this.hit = Physics2D.Raycast((Vector2) ray.origin, (Vector2) ray.direction, float.PositiveInfinity);
  }

  [CompilerGenerated]
  public Collider2D \u003CRegisterPorts\u003Eb__9_0() => this.receiver;

  [CompilerGenerated]
  public RaycastHit2D \u003CRegisterPorts\u003Eb__9_1() => this.hit;
}
