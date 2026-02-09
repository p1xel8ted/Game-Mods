// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ExtractRay
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

public class ExtractRay : ExtractorNode<Ray, Vector3, Vector3>
{
  public override void Invoke(Ray ray, out Vector3 origin, out Vector3 direction)
  {
    origin = ray.origin;
    direction = ray.direction;
  }
}
