// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.RemapVector3
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Utility")]
[Description("Remaps from input min/max to output min/max, by current value provided between input min/max")]
[Name("Remap To Vector3", 0)]
public class RemapVector3 : PureFunctionNode<Vector3, float, float, float, Vector3, Vector3>
{
  public override Vector3 Invoke(
    float current,
    float iMin,
    float iMax,
    Vector3 oMin,
    Vector3 oMax)
  {
    return Vector3.Lerp(oMin, oMax, Mathf.InverseLerp(iMin, iMax, current));
  }
}
