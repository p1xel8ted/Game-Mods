// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.AddDictionaryItem`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System.Collections.Generic;

#nullable disable
namespace FlowCanvas.Nodes;

[ExposeAsDefinition]
[Category("Collections/Dictionaries")]
public class AddDictionaryItem<T> : 
  CallableFunctionNode<IDictionary<string, T>, IDictionary<string, T>, string, T>
{
  public override IDictionary<string, T> Invoke(IDictionary<string, T> dict, string key, T item)
  {
    dict.Add(key, item);
    return dict;
  }
}
