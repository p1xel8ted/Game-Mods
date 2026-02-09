// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.GetChildTransforms
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Unity")]
[Description("Get all child transforms of specified parent")]
public class GetChildTransforms : PureFunctionNode<Transform[], Transform>
{
  public override Transform[] Invoke(Transform parent)
  {
    return parent.Cast<Transform>().ToArray<Transform>();
  }
}
