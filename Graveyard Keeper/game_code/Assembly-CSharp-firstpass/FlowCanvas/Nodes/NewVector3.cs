// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.NewVector3
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Utilities/Constructors")]
[Obsolete]
public class NewVector3 : PureFunctionNode<Vector3, float, float, float>
{
  public override Vector3 Invoke(float x, float y, float z) => new Vector3(x, y, z);
}
