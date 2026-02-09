// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.DeltaTimedVector3
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Per Second (Vector3)", 0)]
[Category("Time")]
[Description("Mutliply input value by Time.deltaTime and optional multiplier")]
public class DeltaTimedVector3 : PureFunctionNode<Vector3, Vector3, float>
{
  public override Vector3 Invoke(Vector3 value, float multiplier = 1f)
  {
    return value * multiplier * Time.deltaTime;
  }
}
