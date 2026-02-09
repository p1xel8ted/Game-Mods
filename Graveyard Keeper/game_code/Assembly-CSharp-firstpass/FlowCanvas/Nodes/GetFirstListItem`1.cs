// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.GetFirstListItem`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using ParadoxNotion.Design;
using System.Collections.Generic;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Collections/Lists")]
[ExposeAsDefinition]
public class GetFirstListItem<T> : PureFunctionNode<T, IList<T>>
{
  public override T Invoke(IList<T> list) => list.FirstOrDefault<T>();
}
