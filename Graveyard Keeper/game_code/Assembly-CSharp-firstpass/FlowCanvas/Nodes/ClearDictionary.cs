// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ClearDictionary
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System.Collections;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Collections/Dictionaries")]
[ExposeAsDefinition]
public class ClearDictionary : CallableFunctionNode<IDictionary, IDictionary>
{
  public override IDictionary Invoke(IDictionary dict)
  {
    dict.Clear();
    return dict;
  }
}
