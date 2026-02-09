// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ExtractCollision
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

public class ExtractCollision : 
  ExtractorNode<Collision, ContactPoint[], ContactPoint, GameObject, Vector3>
{
  public override void Invoke(
    Collision collision,
    out ContactPoint[] contacts,
    out ContactPoint firstContact,
    out GameObject gameObject,
    out Vector3 velocity)
  {
    contacts = collision.contacts;
    firstContact = collision.contacts[0];
    gameObject = collision.gameObject;
    velocity = collision.relativeVelocity;
  }
}
