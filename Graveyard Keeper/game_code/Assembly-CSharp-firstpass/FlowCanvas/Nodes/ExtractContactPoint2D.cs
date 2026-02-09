// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ExtractContactPoint2D
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

public class ExtractContactPoint2D : 
  ExtractorNode<ContactPoint2D, Vector2, Vector2, Collider2D, Collider2D>
{
  public override void Invoke(
    ContactPoint2D contactPoint,
    out Vector2 normal,
    out Vector2 point,
    out Collider2D colliderA,
    out Collider2D colliderB)
  {
    normal = contactPoint.normal;
    point = contactPoint.point;
    colliderA = contactPoint.collider;
    colliderB = contactPoint.otherCollider;
  }
}
