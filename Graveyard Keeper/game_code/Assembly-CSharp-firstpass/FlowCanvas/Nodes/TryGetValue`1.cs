// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.TryGetValue`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable disable
namespace FlowCanvas.Nodes;

[ExposeAsDefinition]
[Category("Collections/Dictionaries")]
public class TryGetValue<T> : CallableFunctionNode<T, IDictionary<string, T>, string>
{
  [CompilerGenerated]
  public bool \u003Cexists\u003Ek__BackingField;

  public bool exists
  {
    get => this.\u003Cexists\u003Ek__BackingField;
    set => this.\u003Cexists\u003Ek__BackingField = value;
  }

  public override T Invoke(IDictionary<string, T> dict, string key)
  {
    T obj = default (T);
    this.exists = dict.TryGetValue(key, out obj);
    return obj;
  }
}
