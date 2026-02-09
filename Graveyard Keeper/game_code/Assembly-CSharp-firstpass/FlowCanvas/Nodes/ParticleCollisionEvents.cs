// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ParticleCollisionEvents
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using ParadoxNotion.Services;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Particle Collision", 0)]
[Category("Events/Object")]
[Description("Called when any Particle System collided with the target collider object")]
public class ParticleCollisionEvents : MessageEventNode<Collider>
{
  public FlowOutput onCollision;
  public Collider receiver;
  public ParticleSystem particle;
  public List<ParticleCollisionEvent> collisionEvents;

  public override string[] GetTargetMessageEvents()
  {
    return new string[1]{ "OnParticleCollision" };
  }

  public override void RegisterPorts()
  {
    this.onCollision = this.AddFlowOutput("On Particle Collision");
    this.AddValueOutput<Collider>("Receiver", (ValueHandler<Collider>) (() => this.receiver));
    this.AddValueOutput<ParticleSystem>("Particle System", (ValueHandler<ParticleSystem>) (() => this.particle));
    this.AddValueOutput<Vector3>("Collision Point", (ValueHandler<Vector3>) (() => this.collisionEvents[0].intersection));
    this.AddValueOutput<Vector3>("Collision Normal", (ValueHandler<Vector3>) (() => this.collisionEvents[0].normal));
    this.AddValueOutput<Vector3>("Collision Velocity", (ValueHandler<Vector3>) (() => this.collisionEvents[0].velocity));
  }

  public void OnParticleCollision(MessageRouter.MessageData<GameObject> msg)
  {
    this.receiver = this.ResolveReceiver(msg.receiver);
    this.particle = msg.value.GetComponent<ParticleSystem>();
    this.collisionEvents = new List<ParticleCollisionEvent>();
    if ((Object) this.particle != (Object) null)
      this.particle.GetCollisionEvents(this.receiver.gameObject, this.collisionEvents);
    this.onCollision.Call(new Flow());
  }

  [CompilerGenerated]
  public Collider \u003CRegisterPorts\u003Eb__5_0() => this.receiver;

  [CompilerGenerated]
  public ParticleSystem \u003CRegisterPorts\u003Eb__5_1() => this.particle;

  [CompilerGenerated]
  public Vector3 \u003CRegisterPorts\u003Eb__5_2() => this.collisionEvents[0].intersection;

  [CompilerGenerated]
  public Vector3 \u003CRegisterPorts\u003Eb__5_3() => this.collisionEvents[0].normal;

  [CompilerGenerated]
  public Vector3 \u003CRegisterPorts\u003Eb__5_4() => this.collisionEvents[0].velocity;
}
