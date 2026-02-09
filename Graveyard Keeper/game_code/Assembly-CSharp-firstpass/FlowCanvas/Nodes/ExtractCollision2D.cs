// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ExtractCollision2D
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

public class ExtractCollision2D : 
  ExtractorNode<Collision2D, ContactPoint2D[], ContactPoint2D, GameObject, Vector2>
{
  public override void Invoke(
    Collision2D collision,
    out ContactPoint2D[] contacts,
    out ContactPoint2D firstContact,
    out GameObject gameObject,
    out Vector2 velocity)
  {
    contacts = collision.contacts;
    firstContact = collision.contacts[0];
    gameObject = collision.gameObject;
    velocity = collision.relativeVelocity;
  }
}
