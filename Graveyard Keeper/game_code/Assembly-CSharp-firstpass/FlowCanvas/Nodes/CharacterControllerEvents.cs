// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.CharacterControllerEvents
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using ParadoxNotion.Services;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Called when the Character Controller hits a collider while performing a Move")]
[Category("Events/Object")]
[Name("Character Controller", 0)]
public class CharacterControllerEvents : MessageEventNode<CharacterController>
{
  public FlowOutput onHit;
  public CharacterController receiver;
  public ControllerColliderHit hitInfo;

  public override string[] GetTargetMessageEvents()
  {
    return new string[1]{ "OnControllerColliderHit" };
  }

  public override void RegisterPorts()
  {
    this.onHit = this.AddFlowOutput("Collider Hit");
    this.AddValueOutput<CharacterController>("Receiver", (ValueHandler<CharacterController>) (() => this.receiver));
    this.AddValueOutput<GameObject>("Other", (ValueHandler<GameObject>) (() => this.hitInfo.gameObject));
    this.AddValueOutput<Vector3>("Collision Point", (ValueHandler<Vector3>) (() => this.hitInfo.point));
    this.AddValueOutput<ControllerColliderHit>("Collision Info", (ValueHandler<ControllerColliderHit>) (() => this.hitInfo));
  }

  public void OnControllerColliderHit(
    MessageRouter.MessageData<ControllerColliderHit> msg)
  {
    this.receiver = this.ResolveReceiver(msg.receiver);
    this.hitInfo = msg.value;
    this.onHit.Call(new Flow());
  }

  [CompilerGenerated]
  public CharacterController \u003CRegisterPorts\u003Eb__4_0() => this.receiver;

  [CompilerGenerated]
  public GameObject \u003CRegisterPorts\u003Eb__4_1() => this.hitInfo.gameObject;

  [CompilerGenerated]
  public Vector3 \u003CRegisterPorts\u003Eb__4_2() => this.hitInfo.point;

  [CompilerGenerated]
  public ControllerColliderHit \u003CRegisterPorts\u003Eb__4_3() => this.hitInfo;
}
