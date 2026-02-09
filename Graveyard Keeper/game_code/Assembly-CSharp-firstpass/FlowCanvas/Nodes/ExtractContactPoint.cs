// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ExtractContactPoint
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

public class ExtractContactPoint : ExtractorNode<ContactPoint, Vector3, Vector3, Collider, Collider>
{
  public override void Invoke(
    ContactPoint contactPoint,
    out Vector3 normal,
    out Vector3 point,
    out Collider colliderA,
    out Collider colliderB)
  {
    normal = contactPoint.normal;
    point = contactPoint.point;
    colliderA = contactPoint.thisCollider;
    colliderB = contactPoint.otherCollider;
  }
}
