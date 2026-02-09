// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ToList`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using ParadoxNotion.Design;
using System;
using System.Collections.Generic;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Utilities/Converters")]
[Obsolete]
public class ToList<T> : PureFunctionNode<List<T>, IList<T>>
{
  public override List<T> Invoke(IList<T> list) => list.ToList<T>();
}
