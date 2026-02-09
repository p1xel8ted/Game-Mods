// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.DeltaTimed
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Mutliply input value by Time.deltaTime and optional multiplier")]
[Category("Time")]
[Name("Per Second (Float)", 0)]
public class DeltaTimed : PureFunctionNode<float, float, float>
{
  public override float Invoke(float value, float multiplier = 1f)
  {
    return value * multiplier * Time.deltaTime;
  }
}
