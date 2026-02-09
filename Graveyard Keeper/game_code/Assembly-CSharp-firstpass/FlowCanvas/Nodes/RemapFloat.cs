// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.RemapFloat
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Remap To Float", 0)]
[Description("Remaps from input min/max to output min/max, by current value provided between input min/max")]
[Category("Utility")]
public class RemapFloat : PureFunctionNode<float, float, float, float, float, float>
{
  public override float Invoke(float current, float iMin, float iMax = 1f, float oMin = 0.0f, float oMax = 100f)
  {
    return Mathf.Lerp(oMin, oMax, Mathf.InverseLerp(iMin, iMax, current));
  }
}
