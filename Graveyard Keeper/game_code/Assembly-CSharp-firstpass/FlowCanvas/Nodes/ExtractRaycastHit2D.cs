// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ExtractRaycastHit2D
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

public class ExtractRaycastHit2D : 
  ExtractorNode<RaycastHit2D, GameObject, float, float, Vector3, Vector3>
{
  public override void Invoke(
    RaycastHit2D hit,
    out GameObject gameObject,
    out float distance,
    out float fraction,
    out Vector3 normal,
    out Vector3 point)
  {
    gameObject = (Object) hit.collider != (Object) null ? hit.collider.gameObject : (GameObject) null;
    distance = hit.distance;
    fraction = hit.fraction;
    normal = (Vector3) hit.normal;
    point = (Vector3) hit.point;
  }
}
