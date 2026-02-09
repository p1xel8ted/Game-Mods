// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ShuffleList`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Collections/Lists")]
[ExposeAsDefinition]
public class ShuffleList<T> : CallableFunctionNode<IList<T>, IList<T>>
{
  public override IList<T> Invoke(IList<T> list)
  {
    for (int index1 = list.Count - 1; index1 > 0; --index1)
    {
      int index2 = (int) Mathf.Floor(UnityEngine.Random.value * (float) (index1 + 1));
      T obj = list[index1];
      list[index1] = list[index2];
      list[index2] = obj;
    }
    return list;
  }
}
