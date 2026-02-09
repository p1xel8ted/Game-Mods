// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ExtractAnimationCurve
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

public class ExtractAnimationCurve : 
  ExtractorNode<AnimationCurve, Keyframe[], float, WrapMode, WrapMode>
{
  public override void Invoke(
    AnimationCurve curve,
    out Keyframe[] keys,
    out float length,
    out WrapMode postWrapMode,
    out WrapMode preWrapMode)
  {
    keys = curve.keys;
    length = (float) curve.length;
    postWrapMode = curve.postWrapMode;
    preWrapMode = curve.preWrapMode;
  }
}
