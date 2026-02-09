// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ExtractQuaternion
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

public class ExtractQuaternion : ExtractorNode<Quaternion, float, float, float, float, Vector3>
{
  public override void Invoke(
    Quaternion quaternion,
    out float x,
    out float y,
    out float z,
    out float w,
    out Vector3 eulerAngles)
  {
    x = quaternion.x;
    y = quaternion.y;
    z = quaternion.z;
    w = quaternion.w;
    eulerAngles = quaternion.eulerAngles;
  }
}
