// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.NewVector4
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
public class NewVector4 : PureFunctionNode<Vector4, float, float, float, float>
{
  public override Vector4 Invoke(float x, float y, float z, float w) => new Vector4(x, y, z, w);
}
