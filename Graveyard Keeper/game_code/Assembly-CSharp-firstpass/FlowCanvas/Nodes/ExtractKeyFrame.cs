// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ExtractKeyFrame
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

public class ExtractKeyFrame : ExtractorNode<Keyframe, float, float, float, float>
{
  public override void Invoke(
    Keyframe key,
    out float inTangent,
    out float outTangent,
    out float time,
    out float value)
  {
    inTangent = key.inTangent;
    outTangent = key.outTangent;
    time = key.time;
    value = key.value;
  }
}
