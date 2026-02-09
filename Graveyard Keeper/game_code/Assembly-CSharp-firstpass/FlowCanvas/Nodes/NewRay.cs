// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.NewRay
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
public class NewRay : PureFunctionNode<Ray, Vector3, Vector3>
{
  public override Ray Invoke(Vector3 origin, Vector3 direction) => new Ray(origin, direction);
}
