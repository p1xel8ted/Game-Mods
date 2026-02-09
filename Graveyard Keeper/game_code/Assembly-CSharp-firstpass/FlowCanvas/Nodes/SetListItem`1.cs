// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.SetListItem`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System.Collections.Generic;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Collections/Lists")]
[ExposeAsDefinition]
public class SetListItem<T> : CallableFunctionNode<IList<T>, IList<T>, int, T>
{
  public override IList<T> Invoke(IList<T> list, int index, T item)
  {
    try
    {
      list[index] = item;
      return list;
    }
    catch
    {
      return (IList<T>) null;
    }
  }
}
