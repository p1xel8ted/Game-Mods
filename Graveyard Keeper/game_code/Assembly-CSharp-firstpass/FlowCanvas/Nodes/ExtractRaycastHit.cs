// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ExtractRaycastHit
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

public class ExtractRaycastHit : ExtractorNode<RaycastHit, GameObject, float, Vector3, Vector3>
{
  public override void Invoke(
    RaycastHit hit,
    out GameObject gameObject,
    out float distance,
    out Vector3 normal,
    out Vector3 point)
  {
    gameObject = (Object) hit.collider != (Object) null ? hit.collider.gameObject : (GameObject) null;
    distance = hit.distance;
    normal = hit.normal;
    point = hit.point;
  }
}
